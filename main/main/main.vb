Imports CIRS_lib

'To do
'High Priority
'Sample rate changing
'dependency unzipping
'limit concurrent files

'settings form?
'allow viewing and deletion of files in batch
'remove jobs older than x days
'delete file after use

'Error checking whenever a request is made

'Low priority
'bucket selection
'unique op names
'Add confidence ratings and check if below a threshold


Public Class main
    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initialise()
        poll()
    End Sub

    'Controls
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click 'Import mp3 file(s) for conversion to .flac and .json generation
        With ofdSelect
            .Filter = "MP3 files (.mp3)|*.mp3|FLAC Audio files (.flac)|*.flac|WAV Audio Files (.wav)|*.wav|M4A Audio Files (.m4a)|*.m4a|All files (*.*)|*.*"
            .Multiselect = True
        End With
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            lblProcess.Text = "Getting authentication token..."
            Dim i As Integer = 1
            While authToken = "" And i < 3 'Retry getAuthToken 3 times
                getAuthToken()
                i += 1
            End While
            Dim opThread As Threading.Thread
            If authToken <> "" Then 'Check authentication token actually exists
                pgbProgress.Visible = True
                lblProcess.Text = "Uploading file(s) for transcription..."
                Dim threadList As New List(Of Threading.Thread)

                For Each trackName As String In ofdSelect.FileNames
                    opThread = New System.Threading.Thread(AddressOf runOp)
                    opThread.Start(trackName)
                    threadList.Add(opThread)
                Next

                For Each t In threadList
                    t.Join()
                Next

                My.Settings.Save()
                MsgBox("Operation(s) finished.", MsgBoxStyle.ApplicationModal, "Operation(s) finished")
                poll()
                ofdSelect.Dispose()
            Else
                MsgBox("Error retrieving authentication token. Check that you have selected the right Service Account Key file and are connected to the internet.", MsgBoxStyle.SystemModal, "Error retrieving authentication token")
            End If
            reset()
        End If
    End Sub

    Private Sub runOp(input As String)
        Dim flacTrack As String = IO.Path.GetDirectoryName(input) & "\" & IO.Path.GetFileNameWithoutExtension(input) & ".flac"

        Dim resample As String = ""
        If sampleRateOutsideBounds(input) = True Then
            resample = " -ar 16000"
        End If
        pgbProgress.Value = 1
        lblProcess.Text = "Converting " & IO.Path.GetFileNameWithoutExtension(input) & " to .flac..."
        runCmd("ffmpeg -y -i """ & input & """" & resample & " -ac 1 """ & flacTrack & """") 'MP3 to FLAC conversion forcing resampling if necessary
        pgbProgress.Value = 2
        lblProcess.Text = "Uploading " & IO.Path.GetFileNameWithoutExtension(input) & "..."
        runCmd("curl -v --upload-file """ & flacTrack & """ -H ""Authorization: Bearer " & authToken & """ -H ""Content-Type:audio/flac"" ""https://storage.googleapis.com/lr_test_transcript/" & IO.Path.GetFileName(flacTrack).Replace(" ", "%20") & """") 'Upload file to bucket. Replace all spaces with %20 so curl doesn't throw parsing errors
        IO.File.Delete(flacTrack) 'Clean up flac file after upload
        pgbProgress.Value = 3
        lblProcess.Text = "Setting permissions..."
        runCmd("curl -X POST --data-binary @""" & AppDomain.CurrentDomain.BaseDirectory & "\Resources\makePublic.json" & """ -H ""Authorization: Bearer " & authToken & """ -H ""Content-Type:application/json"" ""https://www.googleapis.com/storage/v1/b/" & bucket & "/o/" & IO.Path.GetFileName(flacTrack).Replace(" ", "%20") & "/acl""") 'Make file public
        cirsfile.write(My.Resources.template.ToString & vbNewLine & "      ""uri"":""gs://" & bucket & "/" & IO.Path.GetFileNameWithoutExtension(input) & ".flac""" & vbNewLine & "  }" & vbNewLine & "}", IO.Path.GetDirectoryName(input) & "\" & IO.Path.GetFileNameWithoutExtension(input) & ".json", False) 'Write .json file in the same directory
        pgbProgress.Value = 4
        lblProcess.Text = "Posting " & IO.Path.GetFileNameWithoutExtension(input) & " for transcription..."
        Dim output As String = runCmd("curl -X POST -d @""" & IO.Path.GetDirectoryName(input) & "\" & IO.Path.GetFileNameWithoutExtension(input) & ".json""" & " https://speech.googleapis.com/v1/speech:longrunningrecognize?key=" & apiKey & " --header ""Content-Type:application/json""")(0) 'Post for transcription
        IO.File.Delete(IO.Path.GetDirectoryName(input) & "\" & IO.Path.GetFileNameWithoutExtension(input) & ".json") 'Clean up .json file after posting

        If checkErrors(output, """name"": """) = False Then
            'Dim opName As String = InputBox("Please enter the name of the operation. This will default to " & IO.Path.GetFileNameWithoutExtension(input) & ".", "Enter operation name") 'New form for inputbox
            'If opName = "" Then
            Dim opName As String = IO.Path.GetFileNameWithoutExtension(input)
            'End If
            My.Settings.operationsList.Add(cirsfile.parseInString(output, """name"": """, """") & "|" & opName) 'Retrieve the name of the operation and add it to the operations list and then appends it with |JOBNAME
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        End
    End Sub

    Private Sub btnPoll_Click(sender As Object, e As EventArgs) Handles btnPoll.Click
        poll()
    End Sub

    'Subs
    Private Sub initialise()
        If My.Settings.varPath = "" Then
            My.Settings.varPath = Environment.GetEnvironmentVariable("PATH") 'Create a backup of the current PATH variable
        End If

        If My.Settings.operationsList Is Nothing Then 'Initialises string collection
            My.Settings.operationsList = New System.Collections.Specialized.StringCollection
        End If

        checkDirs("Google Cloud SDK", "gcloudPath")
        checkDirs("ffmpeg", "ffmpegPath", "http://ffmpeg.zeranoe.com/builds/")
        checkDirs("curl", "curlPath", "https://curl.haxx.se/dlwiz/?type=bin&os=Win64&flav=-&ver=*&cpu=x86_64")

        'API key is hardcoded for the time being
        If apiKey = "" Then
            apiKey = InputBox("Please paste the API key from APIs > Credentials here.", "Paste API Key")
            If apiKey = "" Then
                MsgBox("An API key is required to transcribe files", MsgBoxStyle.Critical, "Critical Error")
                End
            End If
        End If

        If My.Settings.sAKeyPath = "" Then 'Check a Service Account Key has been located
            MsgBox("A Service Account Key is required to interface with Google. It is located by default in L:\Company Administration\Transcripts\serviceAccountKey.json. Please select it.", MsgBoxStyle.SystemModal, "Service Account Key required")
            With ofdSelect
                .Filter = "JSON file (.json)|*.json"
                .Multiselect = False
            End With
            If ofdSelect.ShowDialog() = DialogResult.OK Then
                My.Settings.sAKeyPath = ofdSelect.FileName
                runCmd("gcloud auth activate-service-account --key-file=""" & My.Settings.sAKeyPath & """") 'Activate service account key with gcloud (first run thing)
            End If
            ofdSelect.Dispose()
        End If

        My.Settings.Save()
    End Sub

    Private Sub checkDirs(program As String, path As String, Optional ByVal url As String = "") 'Check that dependencies exist
        If My.Settings.Item(path) = "" Then 'Check that the path is not empty
            Select Case program
                Case "Google Cloud SDK"
                    If MsgBox(program & " Is required to run this application. Please select the topmost folder of the program (CloudSDK), Or press Cancel to initiate installation.", MsgBoxStyle.SystemModal & MsgBoxStyle.OkCancel, "Dependency not found") = MsgBoxResult.Ok Then
                        While fbdDir.ShowDialog() <> DialogResult.OK Or Not IO.Directory.Exists(fbdDir.SelectedPath & "\google-cloud-sdk")
                            If MsgBox(program & "not found. Respecify directory?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Error") = MsgBoxResult.No Then
                                installGcloud()
                            End If
                        End While
                        My.Settings.Item(path) = fbdDir.SelectedPath & "\google-cloud-sdk\bin"
                    Else
                        installGcloud()
                    End If
                Case Else
                    MsgBox("The directory for " & program & " has not been specified. Please select the 'bin' folder containing " & program, MsgBoxStyle.SystemModal, "Dependency Not Found")
                    While fbdDir.ShowDialog() <> DialogResult.OK Or Not IO.File.Exists(fbdDir.SelectedPath & "\" & program & ".exe")
                        If MsgBox(program & ".exe not found. Respecify directory?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Error") = MsgBoxResult.No Then
                            MsgBox(program & " is required to run this application. Please install " & program & " and then relaunch.", MsgBoxStyle.Critical, "Critical Error")
                            Process.Start(url) 'Link to download page
                            End
                        End If
                    End While
                    My.Settings.Item(path) = fbdDir.SelectedPath
            End Select
            fbdDir.Dispose()
        End If
        Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") & ";" & My.Settings.Item(path))
    End Sub

    Private Sub installGcloud()
        MsgBox("Please select a directory to install the Google Cloud SDK", MsgBoxStyle.SystemModal, "Select install directory")
        While fbdDir.ShowDialog <> DialogResult.OK
            If MsgBox("Google Cloud SDK must be installed. Select a directory or press Cancel to quit.", MsgBoxStyle.SystemModal & MsgBoxStyle.OkCancel, "Dependency required") = MsgBoxResult.Cancel Then
                End
            End If
        End While

        MsgBox("Google Cloud SDK is now installing. This may take a while.", MsgBoxStyle.SystemModal, "Installation started")
        runCmd("Resources\GoogleCloudSDKInstaller /S /allusers /noreporting /nodesktop /D=" & fbdDir.SelectedPath & "\CloudSDK", , True) 'Install gcloud SDK

        If IO.File.Exists(fbdDir.SelectedPath & "\CloudSDK\uninstaller.exe") Then 'Check that the installation has been completed successfully
            MsgBox("Google Cloud SDK installation complete.", MsgBoxStyle.SystemModal, "Installation completed")
            My.Settings.gcloudPath = fbdDir.SelectedPath & "\CloudSDK\google-cloud-sdk\bin"
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") & ";" & My.Settings.gcloudPath)
        Else
            MsgBox("Google Cloud SDK installation failed. If you keep seeing this error, you can try manually installing the Google Cloud SDK yourself and pointing this program to it. Otherwise, please seek an administrator for further assistance.", MsgBoxStyle.Critical, "Critical Error")
            End
        End If
    End Sub

    Private Sub getAuthToken()
        authToken = ""
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", My.Settings.sAKeyPath)
        Dim output As String = runCmd("gcloud auth application-default print-access-token")(0)
        If output.Contains("ERROR") Then
            MsgBox("Error: " & cirsfile.parseInString(output, "(gcloud.auth.application-default.print-access-token) "), MsgBoxStyle.SystemModal, "Authentication token error")
        Else
            authToken = output.Substring(0, output.Length - 2) 'Remove the CR and LF characters at the end of the authToken
        End If
    End Sub

    Private Sub poll()
        If My.Settings.operationsList.Count = 0 Then
            MsgBox("There are no current operations on file", MsgBoxStyle.Information, "No operations found")
            btnPoll.Enabled = False
            flpOperations.Controls.Clear()
        Else
            lblProcess.Text = "Polling operations..."
            btnPoll.Enabled = True

            Dim opThread As Threading.Thread
            Dim threadList As New List(Of Threading.Thread)
            operations.Clear()

            For Each opName As String In My.Settings.operationsList 'Append list of polling processes
                opThread = New System.Threading.Thread(AddressOf getOperations)
                opThread.Start(opName)
                threadList.Add(opThread)
            Next

            For Each t In threadList
                t.Join()
            Next

            flpOperations.Controls.Clear()
            populateFlp()

            reset()
        End If
    End Sub

    Private Sub populateFlp() 'Filling flowLayoutPanel
        For Each op As operation In operations 'Populate to list
            If checkErrors(op.output, "name"": ") = False Then
                Dim getEnabled As Boolean = False
                Dim textColor As Color = Color.DeepSkyBlue
                If op.progress = "Done" Then
                    textColor = Color.Green
                    getEnabled = True
                End If

                Dim newpanel As New Panel With
                    {
                    .Margin = New Padding(3, 3, 3, 3),
                    .Height = 50,
                    .Width = 340,
                    .BackColor = Color.LightGray,
                    .Name = op.id,
                    .Tag = op.progress
                    }

                Dim ID As New Label With
                    {
                    .Text = op.id,
                    .Font = New Font("Segoe UI", 9, FontStyle.Bold),
                    .Height = 15,
                    .Width = 250,
                    .Location = New Point(0, 0),
                    .Name = op.id & ".id"
                    }

                Dim name As New Label With
                    {
                    .Text = op.name,
                    .Font = New Font("Segoe UI", 8),
                    .Height = 13,
                    .Width = 250,
                    .Location = New Point(0, 15),
                    .Name = op.id + ".name"
                    }

                Dim progress As New Label With
                    {
                    .Text = op.progress,
                    .Font = New Font("Segoe UI", 8),
                    .Height = 13,
                    .ForeColor = textColor,
                    .Location = New Point(0, 28),
                    .Name = op.id + ".progress"
                    }

                Dim btnGet As New Button With
                    {
                    .Tag = op.id,
                    .Height = 40,
                    .Width = 40,
                    .BackColor = Color.LightSkyBlue,
                    .BackgroundImage = My.Resources.save,
                    .BackgroundImageLayout = ImageLayout.Stretch,
                    .Location = New Point(255, 5),
                    .Enabled = getEnabled
                    }

                Dim btnDel As New Button With
                    {
                    .Tag = op.id,
                    .Height = 40,
                    .Width = 40,
                    .BackColor = Color.IndianRed,
                    .BackgroundImage = My.Resources.delete,
                    .BackgroundImageLayout = ImageLayout.Stretch,
                    .Location = New Point(295, 5)
                    }

                newpanel.Controls.Add(ID)
                newpanel.Controls.Add(name)
                newpanel.Controls.Add(progress)
                newpanel.Controls.Add(btnGet)
                newpanel.Controls.Add(btnDel)

                flpOperations.Controls.Add(newpanel)
                AddHandler btnGet.MouseClick, AddressOf btnGetClicked
                AddHandler btnDel.MouseClick, AddressOf btnDelClicked
            End If
        Next
    End Sub

    Private Sub getOperations(input As String)
        pgbProgress.Visible = True
        Dim obj As Object = runCmd("curl -X GET https://speech.googleapis.com/v1/operations/" & cirsfile.parseInString(input, "", "|") & "?key=" & apiKey, False, True)
        Dim op As New operation With
            {
            .name = cirsfile.parseInString(input, "|"),
            .id = cirsfile.parseInString(input, "", "|"),
            .progress = "Initialising",
            .output = obj(0),
            .process = obj(1)
            }
        If op.output.Contains("progressPercent") Then
            If op.output.Contains("""progressPercent"": 100") Then
                op.progress = "Done"
            Else
                op.progress = cirsfile.parseInString(op.output, """progressPercent"": ", ",") & "%"
            End If
        End If
        operations.Add(op)
        pgbProgress.PerformStep()
    End Sub

    Private Sub cleanScript(input As String, defaultFileName As String)
        MsgBox("Please select a directory to save the transcript file.")
        With sfdExport
            .Filter = "Text file (.txt)|*.txt"
            .OverwritePrompt = False
            .FileName = defaultFileName
        End With
        If sfdExport.ShowDialog() = DialogResult.OK Then
            Dim output As String = ""
            While input.Contains("transcript"": """) = True
                input = input.Remove(0, (input.IndexOf("transcript"": """) - 1)) 'Remove all preceding text before the first transcript chunk
                output += ("Time: " & cirsfile.parseInString(input, "startTime"": """, "s") & vbNewLine) 'Take timestamp
                output += (cirsfile.parseInString(input, "transcript"": """, """") & vbNewLine & vbNewLine) 'Take contents of chunk
                input = input.Remove(0, (input.IndexOf("startTime"": """) - 1)) 'Remove all preceding text before the first timestamp
            End While
            cirsfile.write(output, sfdExport.FileName, False)
        End If
        sfdExport.Dispose()
    End Sub

    Private Sub reset()
        pgbProgress.Value = 0
        lblProcess.Text = "Ready"
    End Sub

    'Functions
    Private Function runCmd(command As String, Optional ByVal waitForExit As Boolean = True, Optional ByVal hidden As Boolean = True) As Object
        Dim output As String = ""
        Dim cmd As New Process
        With cmd
            .StartInfo = New ProcessStartInfo("cmd", String.Format("/k {0} & {1}", command, "exit"))
            If hidden = True Then
                .StartInfo.CreateNoWindow = True
            End If
            .StartInfo.UseShellExecute = False
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardOutput = True
            .Start()
            If waitForExit = True Then
                .WaitForExit()
            End If
        End With

        Return {cmd.StandardOutput.ReadToEnd, cmd}
    End Function

    Private Function checkErrors(input As String, clearCondition As String)
        Dim foundError As Boolean = True
        If input.Contains(clearCondition) Then
            foundError = False
        Else
            MsgBox("Error code " & cirsfile.parseInString(input, "code"": ", ",") & ": " & cirsfile.parseInString(input, "message"": """, """"), MsgBoxStyle.SystemModal, "Error " & cirsfile.parseInString(input, "code"": ", ","))
        End If
        Return foundError
    End Function

    Private Function sampleRateOutsideBounds(track As String)
        Dim outsideBounds As Boolean = False
        Dim output As String = cirsfile.parseInString(runCmd("ffprobe -v error -show_format -show_streams """ & track & """")(0), "sample_rate=", vbCr)
        If CInt(output) < 8000 Or CInt(output) > 44100 Then
            outsideBounds = True
        End If
        Return outsideBounds
    End Function

    'Handlers

    Private Sub btnGetClicked(sender As Object, e As EventArgs)
        Dim clicked As Button = sender
        runCmd("curl -X GET https://speech.googleapis.com/v1/operations/" & clicked.Tag & "?key=" & apiKey & " > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""") 'Retrieve results of the operation
        Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
        If checkErrors(output, """progressPercent"": ") = False Then
            Dim exportName As String = ""
            For Each op As String In My.Settings.operationsList
                If op.Contains(clicked.Tag) Then
                    exportName = cirsfile.parseInString(op, "|")
                End If
            Next
            cleanScript(output, exportName)
        End If
        IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
    End Sub

    Private Sub btnDelClicked(sender As Object, e As EventArgs)
        Dim clicked As Button = sender
        If MsgBox("This will permanently remove this operation from the list. The transcript file will be UNRECOVERABLE. Are you sure you want to proceed?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Remove operation permanently?") = MsgBoxResult.Yes Then
            Dim target As String = ""
            For Each op As String In My.Settings.operationsList
                If op.Contains(clicked.Tag) Then
                    target = op
                End If
            Next
            My.Settings.operationsList.Remove(target)
            My.Settings.Save()
            poll()
        End If
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs) Handles btnSettings.Click
        If MsgBox("Are you sure you want to clear all program settings and respecify dependencies?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Clear Application Settings?") = MsgBoxResult.Yes Then
            My.Settings.Reset()
            Environment.SetEnvironmentVariable("PATH", My.Settings.varPath)
            initialise()
        End If
    End Sub
End Class