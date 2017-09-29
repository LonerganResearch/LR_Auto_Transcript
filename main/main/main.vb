Imports CIRS_lib

'To do
'Append a list of operations

Public Class main
    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'My.Settings.Reset()
        Dim opList As New List(Of operation)
        If My.Settings.operationsList Is Nothing Then 'Initialises string collection
            My.Settings.operationsList = New System.Collections.Specialized.StringCollection
        End If
        For Each op As String In My.Settings.operationsList 'Populate to 
            Dim operation As New operation
            operation.name = op
            opList.Add(operation)
        Next
        checkDirs("ffmpeg", "ffmpegPath", "http://ffmpeg.zeranoe.com/builds/")
        checkDirs("curl", "curlPath", "https://curl.haxx.se/dlwiz/?type=bin&os=Win64&flav=-&ver=*&cpu=x86_64")
        If My.Settings.apiKey = "" Then
            My.Settings.apiKey = InputBox("Please paste the API key from APIs & Services > Credentials here.", "Paste API Key")
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

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        With ofdSelect
            .Multiselect = True
            .Filter = "MP3 files (.mp3)|*.mp3"
        End With
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            Dim bucket As String = InputBox("Please enter the name of the bucket you wish to upload the file for transcription to. This will default to lr_test_transcript", "Select a bucket directory")

            For Each trackname As String In ofdSelect.FileNames
                If bucket = "" Then
                    bucket = "lr_test_transcript"
                End If
                runCmd(My.Settings.ffmpegPath & "\ffmpeg -y -i """ & trackname & """ -ar " & sampleRate & " -ac 1 """ & IO.Path.GetDirectoryName(trackname) & "\" & IO.Path.GetFileNameWithoutExtension(trackname) & ".flac""") 'MP3 to FLAC conversion
                cirsfile.writeToFile(My.Resources.template.ToString & vbNewLine & "      ""uri"":""gs://" & bucket & "/" & IO.Path.GetFileNameWithoutExtension(trackname) & ".flac""" & vbNewLine & "  }" & vbNewLine & "}", IO.Path.GetDirectoryName(trackname) & "\" & IO.Path.GetFileNameWithoutExtension(trackname) & ".json") 'Write .json file in the same directory
            Next

            MsgBox("MP3 to FLAC conversion and relevant .json file generation complete. Ensure that the FLAC files are uploaded to the specified bucket before requesting transcription.", MsgBoxStyle.ApplicationModal, "Conversion Complete")
            ofdSelect.Dispose()
        End If
        ofdSelect.Multiselect = False
    End Sub

    Private Sub generateFile(ByVal x As String, path As String)
        If IO.File.Exists(path) = False Then
            IO.File.Create(path).Dispose()
        Else
            IO.File.Delete(path) 'Overwrite
        End If
        Try
            Dim objWriter As New IO.StreamWriter(path, True)
            objWriter.WriteLine(x)
            objWriter.Close()
        Catch ex As Exception
            MsgBox("Please close the file first.", MsgBoxStyle.Critical, "Error")
        End Try
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

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        End
    End Sub

    Private Sub btnTranscribe_Click(sender As Object, e As EventArgs) Handles btnTranscribe.Click
        ofdSelect.Filter = "JSON files (.json)|*.json"
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            runCmd("curl -X POST -d @""" & ofdSelect.FileName & """ https: //speech.googleapis.com/v1/speech:longrunningrecognize?key=" & My.Settings.apiKey & " --header ""Content-Type:application/json"" > " & AppDomain.CurrentDomain.BaseDirectory & "temp.txt")

            Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt") 'Retrieve the name of the operation
            Dim x As Integer = (output.IndexOf("""name"": """) + Len("""name"": """))
            Dim name = ""
            While output(x) <> """"
                name += output(x)
                x += 1
            End While
            My.Settings.operationsList.Add(name)
            My.Settings.Save()
            IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
            ofdSelect.Dispose()
        End If
        'select a location to save name
        'start polling
        MsgBox("More things here")
    End Sub

    Private Sub poll(manifest As List(Of operation))
        If manifest.Count = 0 Then
            MsgBox("There are no current operations on file", MsgBoxStyle.Information, "Error")
        Else
            For Each op As operation In manifest
                runCmd("curl -X GET https://speech.googleapis.com/v1/operations/" & op.name & "?key=" & My.Settings.apiKey & " > " & AppDomain.CurrentDomain.BaseDirectory & "temp.txt")
                Dim output As String = My.Computer.FileSystem.ReadAllText(AppDomain.CurrentDomain.BaseDirectory & "temp.txt") 'Retrieve results of the operation
                If output.Contains("""progressPercent"": 100,""") Then
                    op.done = True
                End If
            Next
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
    End Sub
End Class