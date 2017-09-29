Module variables 'Fix this up to let the user change stuff
    Public cirsfile As New CIRS_lib.file
    Public sampleRate As Integer = 44100
    Public Class operation
        Property name As String
        Property done As Boolean = False
    End Class
    'Some defaults can be modified here. Note: most of these properties are redundant, as ffmpeg will force encoding and sample rate. The rest should be changed in resources.
    'Encoding is typically FLAC
    'If uri done manually, replace https://storage.googleapis.com/ with gs://
End Module