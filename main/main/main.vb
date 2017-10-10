Imports CIRS_lib

'To do
'Check if gcloud sdk is installed
'Datagrid for polling
'settings form?
'file not uploaded error code
'input box on top
'Multithread polling
'allow viewing and deletion of files in batch

Public Class main
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
                    Dim opName As String = InputBox("Please enter the name of the operation. This will default to the audio file name.", "Enter operation name")
                    If opName = "" Then
                        opName = IO.Path.GetFileNameWithoutExtension(trackName)
                    End If
                    My.Settings.operationsList.Add(cirsfile.parseInString(output, """name"": """, """") & "|" & opName) 'Retrieve the name of the operation and add it to the operations list and then appends it with |JOBNAME
                End If

                My.Settings.Save()
                'IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
            Next
            poll()
            'MsgBox("MP3 to FLAC conversion and relevant .json file generation complete. Ensure that the FLAC files are uploaded to the specified bucket and made public before requesting transcription.", MsgBoxStyle.ApplicationModal, "Conversion Complete")
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

    Private Sub runCmd(command As String, Optional ByVal waitForExit As Boolean = True)
        Dim cmd As New Process
        With cmd
            .StartInfo = New ProcessStartInfo("cmd", String.Format("/k {0} & {1}", command, "exit"))
            .Start()
            If waitForExit = True Then
                .WaitForExit()
            End If
        End With
    End Sub

    Private Sub poll()
        If My.Settings.operationsList.Count = 0 Then
            MsgBox("There are no current operations on file", MsgBoxStyle.Information, "Error")
        Else
            dgJobs.Rows.Clear()
            Dim rowCounter As Integer = 0
            dgJobs.Rows.Add() 'Add initial row to stop things breaking

            For Each op As String In My.Settings.operationsList 'Populate to list
                runCmd(My.Settings.curlPath & "\curl -X GET https://speech.googleapis.com/v1/operations/" & cirsfile.parseInString(op, "", "|") & "?key=" & My.Settings.apiKey & " > """ & AppDomain.CurrentDomain.BaseDirectory & "temp.txt""")

                Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt") 'Retrieve results of the operation
                If checkErrors(output, """progressPercent"": ") = False Then

                    With dgJobs.Rows(rowCounter)
                        .Cells(0).Value = cirsfile.parseInString(op, "", "|")
                        .Cells(1).Value = cirsfile.parseInString(op, "|")
                        .Cells(2).Value = cirsfile.parseInString(output, """progressPercent"": ", ",") & "%" 'Get percentage
                    End With
                    dgJobs.Rows.Add()
                    rowCounter += 1
                End If

                IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
            Next
            dgJobs.CurrentRow.Selected = False
            dgJobs.Rows(dgJobs.RowCount - 1).Selected = True 'Remove extra redundant row
            For Each row As DataGridViewRow In dgJobs.SelectedRows
                dgJobs.Rows.Remove(row)
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
            cirsfile.write(output, sfdExport.FileName, True)
        End If
        sfdExport.Dispose()
    End Sub

    Private Function checkErrors(input As String, clearCondition As String)
        Dim foundError As Boolean = True
        Select Case True
            Case input.Contains("code"": 400")
                MsgBox("Error code 400: API key not valid. Please pass a valid API key.", MsgBoxStyle.Exclamation, "Error 400")
            Case input.Contains("code"": 403")
                MsgBox("Error code 403: Source file not made public. Check 'Share publicly' in bucket next to file", MsgBoxStyle.Exclamation, "Error 403")
            Case input.Contains("code"": 404")
                MsgBox("Error code 404: Requested entity not found. It has likely been too long since the file was transcribed.", MsgBoxStyle.Exclamation, "Error 404")
            Case input.Contains(clearCondition)
                foundError = False
            Case Else
                MsgBox("Unspecified error: " & input, MsgBoxStyle.SystemModal, "Unspecified Error")
        End Select
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
        'Stuff
    End Sub

    Private Sub btnDeleteOp_Click(sender As Object, e As EventArgs) Handles btnDeleteOp.Click
        If MsgBox("This will permanently remove the operation from the list. The transcript file will be UNRECOVERABLE. Are you sure you want to proceed?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Remove operation permanently?") = MsgBoxResult.Yes Then
            Dim target As String = ""
            For Each op As String In My.Settings.operationsList
                If op.Contains(dgJobs.CurrentRow.Cells(0).Value) Then
                    target = op
                End If
            Next
            My.Settings.operationsList.Remove(target)
            poll()
        End If
    End Sub
End Class