Imports CIRS_lib

'To do
'Check if authToken is empty
'settings form?
'input box on top
'allow viewing and deletion of files in batch
'remove jobs older than x days
'loading bar
'delete .flac and .json generated?
'dependency unzipping
'bucket selection
'unique op names
'stop from dling transcription without 100%
'if no progress percent say please wait
'Panel too short for ID


Public Class main
    Dim clickedID As String = ""

    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initialise()
        poll()
    End Sub

    'Controls
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click 'Import mp3 file(s) for conversion to .flac and .json generation
        ofdSelect.Filter = "MP3 files (.mp3)|*.mp3"
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            getAuthToken()
            For Each trackName As String In ofdSelect.FileNames
                Dim flacTrack As String = IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".flac"
                runCmd("ffmpeg -y -i """ & trackName & """ -ar " & sampleRate & " -ac 1 """ & flacTrack & """") 'MP3 to FLAC conversion

                runCmd("curl -v --upload-file """ & flacTrack & """ -H ""Authorization: Bearer " & authToken & """ -H ""Content-Type:audio/flac"" ""https://storage.googleapis.com/lr_test_transcript/" & IO.Path.GetFileName(flacTrack) & """") 'Upload file 

                cirsfile.write(My.Resources.makePublic.ToString, AppDomain.CurrentDomain.BaseDirectory & "makePublic.json", False) 'Copy file from resources
                runCmd("curl -X POST --data-binary @""" & AppDomain.CurrentDomain.BaseDirectory & "makePublic.json" & """ -H ""Authorization: Bearer " & authToken & """ -H ""Content-Type:application/json"" ""https://www.googleapis.com/storage/v1/b/" & bucket & "/o/" & IO.Path.GetFileName(flacTrack) & "/acl""") 'Make file public
                IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "makePublic.json")

                cirsfile.write(My.Resources.template.ToString & vbNewLine & "      ""uri"":""gs://" & bucket & "/" & IO.Path.GetFileNameWithoutExtension(trackName) & ".flac""" & vbNewLine & "  }" & vbNewLine & "}", IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".json", False) 'Write .json file in the same directory

                runCmd("curl -X POST -d @""" & IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".json""" & " https://speech.googleapis.com/v1/speech:longrunningrecognize?key=" & apiKey & " --header ""Content-Type:application/json"" > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""") 'Post for transcription

                Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
                If checkErrors(output, """name"": """) = False Then
                    Dim opName As String = InputBox("Please enter the name of the operation. This will default to " & IO.Path.GetFileNameWithoutExtension(trackName) & ".", "Enter operation name")
                    If opName = "" Then
                        opName = IO.Path.GetFileNameWithoutExtension(trackName)
                    End If
                    My.Settings.operationsList.Add(cirsfile.parseInString(output, """name"": """, """") & "|" & opName) 'Retrieve the name of the operation and add it to the operations list and then appends it with |JOBNAME
                    MsgBox("File(s) uploaded for transcription.", MsgBoxStyle.ApplicationModal, "Upload Complete")
                End If

                My.Settings.Save()
                IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
            Next
            poll()
            ofdSelect.Dispose()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        End
    End Sub

    Private Sub btnPoll_Click(sender As Object, e As EventArgs) Handles btnPoll.Click
        poll()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        If MsgBox("Are you sure you want to clear all program settings and respecify dependencies?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Clear Application Settings?") = MsgBoxResult.Yes Then
            My.Settings.Reset()
            Environment.SetEnvironmentVariable("PATH", My.Settings.varPath)
            initialise()
        End If
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        MsgBox("Nothing!")
        'If sfdExport.ShowDialog = DialogResult.OK Then
        'IO.File.WriteAllBytes("C: \Users\rei.kaneko.LONERGAN\Downloads\test\curl.zip", My.Resources.curl)
        'If ofdSelect.ShowDialog = DialogResult.OK Then
        '    If fbdDir.ShowDialog = DialogResult.OK Then
        '        'Unzip
        '    End If
        'End If
        'End If
    End Sub

    Private Sub btnGetTranscript_Click(sender As Object, e As EventArgs) Handles btnGetTranscript.Click
        runCmd("curl -X GET https://speech.googleapis.com/v1/operations/" & clickedID & "?key=" & apiKey & " > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""")

        Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt") 'Retrieve results of the operation
        If checkErrors(output, """progressPercent"": ") = False And output.Contains("progressPercent"": 100") Then
            cleanScript(output)
        End If
    End Sub

    Private Sub btnDeleteOp_Click(sender As Object, e As EventArgs) Handles btnDeleteOp.Click
        If MsgBox("This will permanently remove the operation from the list. The transcript file will be UNRECOVERABLE. Are you sure you want to proceed?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Remove operation permanently?") = MsgBoxResult.Yes Then
            Dim target As String = ""
            For Each op As String In My.Settings.operationsList
                If op.Contains(clickedID) Then
                    target = op
                End If
            Next
            btnGetTranscript.Enabled = False
            My.Settings.operationsList.Remove(target)
            My.Settings.Save()
            poll()
        End If
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
        'If apiKey = "" Then
        '    apiKey = InputBox("Please paste the API key from APIs > Credentials here.", "Paste API Key")
        '    If apiKey = "" Then
        '        MsgBox("An API key is required to transcribe files", MsgBoxStyle.Critical, "Critical Error")
        '        End
        '    End If
        '    My.Settings.Save()
        'End If

        If My.Settings.sAKeyPath = "" Then 'Check a Service Account Key has been located
            MsgBox("A Service Account Key is required to interface with Google. It is located by default in L:\Company Administration\Transcripts\serviceAccountKey.json. Please select it.", MsgBoxStyle.SystemModal, "Service Account Key required")
            With ofdSelect
                .Filter = "JSON file (.json)|*.json"
                .Multiselect = False
            End With
            If ofdSelect.ShowDialog() = DialogResult.OK Then
                My.Settings.sAKeyPath = ofdSelect.FileName
                runCmd("gcloud auth activate-service-account --key-file=""" & My.Settings.sAKeyPath & """") 'Activate service account key with gcloud (first run thing)
                getAuthToken()
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
        IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory & "GoogleCloudSDKInstaller.exe", My.Resources.GoogleCloudSDKInstaller) 'Copy installer from resources
        runCmd("GoogleCloudSDKInstaller /S /allusers /noreporting /nodesktop /D=" & fbdDir.SelectedPath & "\CloudSDK", , True) 'Install gcloud SDK
        IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "GoogleCloudSDKInstaller.exe") 'Delete original installer

        If IO.File.Exists(fbdDir.SelectedPath & "\CloudSDK\uninstaller.exe") Then 'Check that the installation has been completed successfully
            MsgBox("Google Cloud SDK installation complete.", MsgBoxStyle.SystemModal, "Installation completed")
            My.Settings.gcloudPath = fbdDir.SelectedPath & "\CloudSDK\google-cloud-sdk\bin"
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") & ";" & My.Settings.gcloudPath)
        Else
            MsgBox("Google Cloud SDK installation failed. Please seek an administrator for further assistance.", MsgBoxStyle.Critical, "Critical Error")
            End
        End If
    End Sub

    Private Sub getAuthToken()
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", My.Settings.sAKeyPath)
        runCmd("gcloud auth application-default print-access-token > """ & AppDomain.CurrentDomain.BaseDirectory & "authkey.txt""")
        authToken = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "authkey.txt").Substring(0, My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "authkey.txt").Length - 2) 'Remove the CR and LF characters at the end of the authToken
        IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "authkey.txt")
    End Sub

    Private Sub poll()
        flpOperations.Controls.Clear()
        If My.Settings.operationsList.Count = 0 Then
            MsgBox("There are no current operations on file", MsgBoxStyle.Information, "No operations found")
            btnGetTranscript.Enabled = False
            btnDeleteOp.Enabled = False
            btnPoll.Enabled = False
        Else
            btnPoll.Enabled = True
            Dim cmdList As New List(Of Process) 'List of polling processes
            Dim cmdPollDone As Boolean = False

            For Each op As String In My.Settings.operationsList 'Append list of polling processes 
                cmdList.Add(runCmd("curl -X GET https://speech.googleapis.com/v1/operations/" & cirsfile.parseInString(op, "", "|") & "?key=" & apiKey & " > """ & AppDomain.CurrentDomain.BaseDirectory & cirsfile.parseInString(op, "", "|") & ".txt""", False, True))
            Next

            While cmdPollDone = False
                Dim i = 0
                For Each process As Process In cmdList
                    If process.HasExited = True Then
                        i += 1
                    End If
                Next
                If i = cmdList.Count Then
                    cmdPollDone = True
                End If
            End While

            'Filling panels
            For Each op As String In My.Settings.operationsList 'Populate to list
                Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & cirsfile.parseInString(op, "", "|") & ".txt") 'Retrieve results of the operation
                If checkErrors(output, "name"": ") = False Then
                    Dim panelColor As Color = Color.DeepSkyBlue
                    If output.Contains("progressPercent"": 100") Then
                        panelColor = Color.Green
                    End If
                    Dim newpanel As New Panel With
                        {
                        .Margin = New Padding(3, 3, 3, 3),
                        .Height = 55,
                        .Width = 140,
                        .BackColor = panelColor,
                        .Name = cirsfile.parseInString(op, "", "|"),
                        .Tag = cirsfile.parseInString(output, """progressPercent"": ", ",")
                        }
                    Dim ID As New Label With
                        {
                        .Text = cirsfile.parseInString(op, "", "|"),
                        .Font = New Font("Segoe UI", 9, FontStyle.Bold),
                        .Height = 15,
                        .Location = New Point(0, 0),
                        .Name = cirsfile.parseInString(op, "", "|") & ".id"
                        }

                    Dim name As New Label With
                        {
                        .Text = cirsfile.parseInString(op, "|"),
                        .Font = New Font("Segoe UI", 8),
                        .Height = 13,
                        .Location = New Point(0, 15),
                        .Name = cirsfile.parseInString(op, "", "|") + ".name"
                        }

                    Dim progress As New Label With
                        {
                        .Text = cirsfile.parseInString(output, """progressPercent"": ", ",") & "%", 'Get percentage
                        .Font = New Font("Segoe UI", 8),
                        .Height = 13,
                        .Location = New Point(0, 28),
                        .Name = cirsfile.parseInString(op, "", "|") + ".progress"
                        }

                    newpanel.Controls.Add(ID)
                    newpanel.Controls.Add(name)
                    newpanel.Controls.Add(progress)

                    flpOperations.Controls.Add(newpanel)
                    AddHandler newpanel.MouseClick, AddressOf panelClicked
                    AddHandler ID.MouseClick, AddressOf labelClicked
                    AddHandler name.MouseClick, AddressOf labelClicked
                    AddHandler progress.MouseClick, AddressOf labelClicked
                End If
                'IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & cirsfile.parseInString(op, "", "|") & ".txt")
            Next
        End If
    End Sub

    Private Sub cleanScript(input As String)
        MsgBox("Please select a directory to save the transcript file.")
        With sfdExport
            .Filter = "Text file (.txt)|*.txt"
            .OverwritePrompt = False
            .FileName = "Output"
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

    'Functions
    Private Function runCmd(command As String, Optional ByVal waitForExit As Boolean = True, Optional ByVal hidden As Boolean = False)
        Dim cmd As New Process
        With cmd
            .StartInfo = New ProcessStartInfo("cmd", String.Format("/k {0} & {1}", command, "exit"))
            If hidden = True Then
                .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            End If
            .Start()
            If waitForExit = True Then
                .WaitForExit()
            End If
        End With
        Return cmd
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

    'Handlers
    Private Sub panelClicked(sender As Object, e As EventArgs)
        Dim clicked As Panel = sender
        'Panel selection here
        For Each panel As Panel In flpOperations.Controls
            If panel.Tag = "100" Then
                panel.BackColor = Color.Green
            Else
                panel.BackColor = Color.DeepSkyBlue
            End If
        Next
        clicked.BackColor = Color.Orange
        btnDeleteOp.Enabled = True
        btnGetTranscript.Enabled = True
        clickedID = clicked.Name
    End Sub

    Private Sub labelClicked(sender As Object, e As EventArgs)
        panelClicked(sender.Parent, e)
    End Sub
End Class