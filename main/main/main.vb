Imports CIRS_lib

'To do
'Check if gcloud sdk is installed
'settings form?
'input box on top
'allow viewing and deletion of files in batch
'remove jobs older than x days
'loading bar
'delete .flac and .json generated?
'conversion and posting takes a while

Public Class main
    Dim clickedID As String = ""

    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initialise()
        poll()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click 'Import mp3 file(s) for conversion to .flac and .json generation
        ofdSelect.Filter = "MP3 files (.mp3)|*.mp3"
        If ofdSelect.ShowDialog() = DialogResult.OK Then

            For Each trackName As String In ofdSelect.FileNames
                runCmd(My.Settings.ffmpegPath & "\ffmpeg -y -i """ & trackName & """ -ar " & sampleRate & " -ac 1 """ & IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".flac""") 'MP3 to FLAC conversion

                runCmd(My.Settings.base64Path & "\base64 -e """ & IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".flac"" > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""") 'FLAC to base64 conversion
                Dim base64 As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
                cirsfile.write(My.Resources.template.ToString & vbNewLine & "      ""content"": """ & base64 & """" & vbNewLine & "  }" & vbNewLine & "}", IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".json", False) 'Write .json file in the same directory

                'Post for transcription
                runCmd(My.Settings.curlPath & "\curl -X POST -d @""" & IO.Path.GetDirectoryName(trackName) & "\" & IO.Path.GetFileNameWithoutExtension(trackName) & ".json""" & " https://speech.googleapis.com/v1/speech:longrunningrecognize?key=" & My.Settings.apiKey & " --header ""Content-Type:application/json"" > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""")

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
            initialise()
        End If
    End Sub

    Private Sub initialise()
        If My.Settings.operationsList Is Nothing Then 'Initialises string collection
            My.Settings.operationsList = New System.Collections.Specialized.StringCollection
        End If

        checkDirs("ffmpeg", "ffmpegPath", "http://ffmpeg.zeranoe.com/builds/")
        checkDirs("curl", "curlPath", "https://curl.haxx.se/dlwiz/?type=bin&os=Win64&flav=-&ver=*&cpu=x86_64")
        checkDirs("base64", "base64Path", "https://www.fourmilab.ch/webtools/base64/")

        If My.Settings.apiKey = "" Then
            My.Settings.apiKey = InputBox("Please paste the API key from APIs > Credentials here.", "Paste API Key")
            If My.Settings.apiKey = "" Then
                MsgBox("An API key is required to transcribe files", MsgBoxStyle.Critical, "Critical Error")
                End
            End If
            My.Settings.Save()
        End If
    End Sub

    Private Sub checkDirs(program As String, path As String, url As String) 'Check that dependencies exist
        If My.Settings.Item(path) = "" Then 'Check that the path is not empty
            MsgBox("The directory for " & program & " has not been specified. Please select the folder containing " & program, MsgBoxStyle.ApplicationModal + MsgBoxStyle.SystemModal, "Dependency Not Found")
            While fbdDir.ShowDialog() <> DialogResult.OK Or Not IO.File.Exists(fbdDir.SelectedPath & "\" & program & ".exe")
                If MsgBox(program & ".exe not found. Respecify directory?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Error") = MsgBoxResult.No Then
                    MsgBox(program & " is required to run this application. Please install " & program & " and then relaunch.", MsgBoxStyle.Critical, "Critical Error")
                    Process.Start(url) 'Link to download page
                    End
                End If
            End While
            My.Settings.Item(path) = fbdDir.SelectedPath
            fbdDir.Dispose()
            My.Settings.Save()
        End If
    End Sub

    'Authentication key is not used for long recognise
    'Private Sub getAuthKey()
    '    If ofdSelect.ShowDialog() = DialogResult.OK Then
    '        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", ofdSelect.FileName)
    '        runCmd("gcloud auth application-default print-access-token > " & AppDomain.CurrentDomain.BaseDirectory & "authkey.txt") 'MP3 to FLAC conversion
    '        My.Settings.authToken = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "authkey.txt")
    '        IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "authkey.txt")
    '        ofdSelect.Dispose()
    '    End If
    'End Sub

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
                cmdList.Add(runCmd(My.Settings.curlPath & "\curl -X GET https://speech.googleapis.com/v1/operations/" & cirsfile.parseInString(op, "", "|") & "?key=" & My.Settings.apiKey & " > """ & AppDomain.CurrentDomain.BaseDirectory & cirsfile.parseInString(op, "", "|") & ".txt""", False, True))
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
                IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & cirsfile.parseInString(op, "", "|") & ".txt")
            Next
        End If
    End Sub


    Private Sub cleanScript(input As String)
        MsgBox("Please select a directory to save the transcript file.")
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

    Private Function checkErrors(input As String, clearCondition As String)
        Dim foundError As Boolean = True
        If input.Contains(clearCondition) Then
            foundError = False
        Else
            MsgBox("Error code " & cirsfile.parseInString(input, "code"": ", ",") & ": " & cirsfile.parseInString(input, "message"": """, """"), MsgBoxStyle.SystemModal, "Error " & cirsfile.parseInString(input, "code"": ", ","))
        End If
        Return foundError
    End Function

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        MsgBox("Nothing!")
        'If ofdSelect.ShowDialog() = DialogResult.OK Then
        '    Dim input As String = My.Computer.FileSystem.ReadAllText(ofdSelect.FileName)
        '    MsgBox("Please select a directory to save the transcript file.")
        '    If sfdExport.ShowDialog() = DialogResult.OK Then
        '        Dim output As String = ""
        '        While input.Contains("transcript"": """) = True
        '            input = input.Remove(0, (input.IndexOf("transcript"": """) - 1)) 'Remove all preceding text before the first transcript chunk
        '            output += ("Time: " & cirsfile.parseInString(input, "startTime"": """, "s") & vbNewLine) 'Take timestamp
        '            output += (cirsfile.parseInString(input, "transcript"": """, """") & vbNewLine & vbNewLine) 'Take contents of chunk
        '            input = input.Remove(0, (input.IndexOf("startTime"": """) - 1)) 'Remove all preceding text before the first timestamp
        '        End While
        '        cirsfile.write(output, sfdExport.FileName, True)
        '    End If
        '    sfdExport.Dispose()
        'End If
    End Sub

    Private Sub btnGetTranscript_Click(sender As Object, e As EventArgs) Handles btnGetTranscript.Click
        runCmd(My.Settings.curlPath & "\curl -X GET https://speech.googleapis.com/v1/operations/" & clickedID & "?key=" & My.Settings.apiKey & " > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""")

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