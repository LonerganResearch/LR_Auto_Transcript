Module variables
    Public Class track 'Some defaults can be modified here.
        Property name As String
        Property encoding As String = "FLAC" 'Typically FLAC
        Property sampleRate As Integer = "44100" 'Typically 44100 Hz
        Property languageCode As String = "en-UK"
        Property enableWordTimeOffsets As Boolean = False
        Property bucket As String = "lr_test_transcript" 'If done manually, replace https://storage.googleapis.com/ with gs://
    End Class
End Module