Module variables
    Public ffmpegPath As String = "L:\Tools\ffmpeg-20170921-183fd30-win64-static\bin\"

    Public Class track 'Some defaults can be modified here.
        Property name As String
        Property encoding As String = "FLAC" 'Typically FLAC
        Property sampleRate As Integer = "44100" 'Typically 44100 Hz
        Property languageCode As String = "en-UK"
        Property enableWordTimeOffsets As Boolean = False
        Property uri As String 'Replace https://storage.googleapis.com/ with gs://
    End Class
End Module
