Public Class main
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If ofdSelectTrack.ShowDialog() = System.Windows.Forms.DialogResult.OK Then 'Enable controls
            Dim bucket As String = "lr_test_transcript" 'Where the file will be stored
            Dim track As New track
            MsgBox(My.Resources.template.ToString)
            track.name = IO.Path.GetFileNameWithoutExtension(ofdSelectTrack.FileName)
            Process.Start("cmd", String.Format("/k {0} & {1}", ffmpegPath & "ffmpeg -i " & ofdSelectTrack.FileName & " -ar " & track.sampleRate & " -ac 1 " & IO.Path.GetDirectoryName(ofdSelectTrack.FileName) & "\" & IO.Path.GetFileNameWithoutExtension(ofdSelectTrack.FileName) & ".flac", "exit")) 'MP3 to FLAC conversion
            generateFile(My.Resources.template.ToString & vbNewLine & "      ""uri"":""gs://" & bucket & "/" & IO.Path.GetFileName(ofdSelectTrack.FileName) & """" & vbNewLine & "  }" & vbNewLine & "}", IO.Path.GetDirectoryName(ofdSelectTrack.FileName) & "\" & IO.Path.GetFileNameWithoutExtension(ofdSelectTrack.FileName) & ".json") 'Write .json file in the same directory
        End If
    End Sub

    Private Sub generateFile(ByVal x As String, path As String)
        If System.IO.File.Exists(path) = False Then
            System.IO.File.Create(path).Dispose()
        End If
        Try
            Dim objWriter As New System.IO.StreamWriter(path, True)
            objWriter.WriteLine(x)
            objWriter.Close()
        Catch ex As Exception
            MsgBox("Please close the file first.")
        End Try
    End Sub
End Class