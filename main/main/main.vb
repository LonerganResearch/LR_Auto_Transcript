Imports CIRS_lib

'To do
'Check if gcloud sdk is installed
'Datagrid for polling
'poll on start
'transcript db?
'file not uploaded error code

Public Class main
    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initialise()
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        ofdSelect.Filter = "MP3 files (.mp3)|*.mp3"
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            Dim bucket As String = InputBox("Please enter the name of the bucket you wish to upload the file for transcription to. This will default to lr_test_transcript", "Select a bucket directory")

            For Each trackname As String In ofdSelect.FileNames
                If bucket = "" Then
                    bucket = "lr_test_transcript"
                End If
                runCmd(My.Settings.ffmpegPath & "\ffmpeg -y -i """ & trackname & """ -ar " & sampleRate & " -ac 1 """ & IO.Path.GetDirectoryName(trackname) & "\" & IO.Path.GetFileNameWithoutExtension(trackname) & ".flac""") 'MP3 to FLAC conversion
                cirsfile.write(My.Resources.template.ToString & vbNewLine & "      ""uri"":""gs://" & bucket & "/" & IO.Path.GetFileNameWithoutExtension(trackname) & ".flac""" & vbNewLine & "  }" & vbNewLine & "}", IO.Path.GetDirectoryName(trackname) & "\" & IO.Path.GetFileNameWithoutExtension(trackname) & ".json", False) 'Write .json file in the same directory
            Next

            MsgBox("MP3 to FLAC conversion and relevant .json file generation complete. Ensure that the FLAC files are uploaded to the specified bucket and made public before requesting transcription.", MsgBoxStyle.ApplicationModal, "Conversion Complete")
            ofdSelect.Dispose()
        End If
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        End
    End Sub

    Private Sub btnTranscribe_Click(sender As Object, e As EventArgs) Handles btnTranscribe.Click
        ofdSelect.Filter = "JSON files (.json)|*.json"
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            For Each trackname As String In ofdSelect.FileNames
                runCmd(My.Settings.curlPath & "\curl -X POST -d @""" & ofdSelect.FileName & """ https://speech.googleapis.com/v1/speech:longrunningrecognize?key=" & My.Settings.apiKey & " --header ""Content-Type:application/json"" > " & AppDomain.CurrentDomain.BaseDirectory & "temp.txt")

                Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
                Select Case True
                    Case output.Contains("code"": 400")
                        MsgBox("Error code 400: API key not valid. Please pass a valid API key.", MsgBoxStyle.SystemModal, "Error 400")
                    Case output.Contains("code"": 403")
                        MsgBox("Error code 403: Source file not made public. Check 'Share publicly' in bucket next to file", MsgBoxStyle.SystemModal, "Error 403")
                    Case output.Contains("""name"": """)
                        My.Settings.operationsList.Add(cirsfile.parse(output, """name"": """, """")) 'Retrieve the name of the operation
                    Case Else
                        MsgBox("Unspecified error: " & output)
                End Select

                My.Settings.Save()
                IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt") 'Uncomment when releasing
            Next

            ofdSelect.Dispose()
            'poll()
        End If
    End Sub

    Private Sub btnPoll_Click(sender As Object, e As EventArgs) Handles btnPoll.Click
        'Test collection
        'For i = 1 To 5 
        '    My.Settings.operationsList.Add(i)
        'Next
        'Dim test As New List(Of String)
        'For Each str As String In My.Settings.operationsList
        '    test.Add(str)
        'Next
        'Dim out As String = ""
        'For Each str As String In test
        '    out += str
        'Next
        'MsgBox(out)
        'poll()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        If MsgBox("Are you sure you want to clear all program settings and respecify dependencies?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Clear Application Settings?") Then
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
            MsgBox("The directory for " & program & " has not been specified. Please select the 'bin' folder of " & program & " installation/extraction", MsgBoxStyle.ApplicationModal + MsgBoxStyle.SystemModal, "Dependency Not Found")
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

    Private Sub runCmd(command As String)
        Dim cmd As New Process
        With cmd
            .StartInfo = New ProcessStartInfo("cmd", String.Format("/k {0} & {1}", command, "exit"))
            .Start()
            .WaitForExit()
        End With
    End Sub

    Private Sub poll()
        Dim opList As New List(Of operation)
        For Each op As String In My.Settings.operationsList 'Populate to list
            Dim operation As New operation
            operation.name = op
            opList.Add(operation)
        Next
        If opList.Count = 0 Then
            MsgBox("There are no current operations on file", MsgBoxStyle.Information, "Error")
        Else
            For Each op As operation In opList
                runCmd("curl -X GET https://speech.googleapis.com/v1/operations/" & op.name & "?key=" & My.Settings.apiKey & " > " & AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
                Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt") 'Retrieve results of the operation
                op.progress = cirsfile.parse(output, """progressPercent"": ", ",") 'Get percentage
                IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
            Next
        End If
    End Sub

    Private Sub cleanScript(input As String)
        MsgBox("Please select a directory to save the transcript file.")
        If sfdExport.ShowDialog() = DialogResult.OK Then
            Dim output As String = ""
            While input.Contains("transcript"": """) = True
                input = input.Remove(0, (input.IndexOf("transcript"": """) - 1)) 'Remove all preceding text before the first transcript chunk
                output += ("Time: " & cirsfile.parse(input, "startTime"": """, "s") & vbNewLine) 'Take timestamp
                output += (cirsfile.parse(input, "transcript"": """, """") & vbNewLine & vbNewLine) 'Take contents of chunk
                input = input.Remove(0, (input.IndexOf("startTime"": """) - 1)) 'Remove all preceding text before the first timestamp
            End While
            cirsfile.write(output, sfdExport.FileName, True)
        End If
        sfdExport.Dispose()
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        MsgBox("Nothing!")
    End Sub
End Class