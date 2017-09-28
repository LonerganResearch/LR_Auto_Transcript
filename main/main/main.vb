Imports CIRS_lib

'To do
'Integrate command line recognise and get
'Detect whether operation success
'curl -X POST -d @"[.JSON_LOCATION]" https://speech.googleapis.com/v1/speech:longrunningrecognize?key=[API_KEY] --header "Content-Type:application/json" > [OUTPUT_FILE]
'curl -X GET https://speech.googleapis.com/v1/operations/[RETURNED_NAME]?key=[API_KEY] > [OUTPUT_FILE]
'Append a list of operations
'Continual polling for 'true' (use str.contains to poll complete i.e. "progressPercent": 100

Public Class main
    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'My.Settings.Reset()
        checkDirs("ffmpeg", "ffmpegPath", "http://ffmpeg.zeranoe.com/builds/")
        checkDirs("curl", "curlPath", "https://curl.haxx.se/dlwiz/?type=bin&os=Win64&flav=-&ver=*&cpu=x86_64")
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
        ofdSelect.Multiselect = True
        If ofdSelect.ShowDialog() = DialogResult.OK Then
            Dim bucket As String = InputBox("Please enter the name of the bucket you wish to upload the file for transcription to. This will default to lr_test_transcript", "Select a bucket directory")

            For Each trackname As String In ofdSelect.FileNames
                If bucket = "" Then
                    bucket = "lr_test_transcript"
                End If
                runCmd(My.Settings.ffmpegPath & "\ffmpeg -y -i """ & trackname & """ -ar " & sampleRate & " -ac 1 """ & IO.Path.GetDirectoryName(trackname) & "\" & IO.Path.GetFileNameWithoutExtension(trackname) & ".flac""")
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
            .StartInfo = New ProcessStartInfo("cmd", String.Format("/k {0} & {1}", command, "exit")) 'MP3 to FLAC conversion
            .Start()
            .WaitForExit()
        End With
    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        End
    End Sub

    Private Sub btnTranscribe_Click(sender As Object, e As EventArgs) Handles btnTranscribe.Click
        My.Settings.apiKey = InputBox("Please paste the API key from APIs & Services > Credentials here.", "Paste API Key")
        If My.Settings.apiKey = "" Then
            MsgBox("More things here")
        Else
            MsgBox("An API key is required to transcribe files", MsgBoxStyle.Critical, "Critical Error")
        End If
    End Sub
End Class

