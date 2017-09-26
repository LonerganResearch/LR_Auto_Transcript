Public Class main
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If ofdSelectTrack.ShowDialog() = System.Windows.Forms.DialogResult.OK Then 'Enable controls
            Dim track As New track With
                {
                .name = IO.Path.GetFileNameWithoutExtension(ofdSelectTrack.FileName)
                }
            Process.Start("cmd", String.Format("/k {0} & {1}", ffmpegPath & "ffmpeg -i " & ofdSelectTrack.FileName & " -ac 1 " & IO.Path.GetDirectoryName(ofdSelectTrack.FileName) & "\" & IO.Path.GetFileNameWithoutExtension(ofdSelectTrack.FileName) & ".flac", "exit")) 'MP3 to FLAC conversion
        End If
    End Sub
End Class