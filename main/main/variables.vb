Module variables 'Fix this up to let the user change stuff
    Public cirsfile As New CIRS_lib.file 'Import library
    Public sampleRate As Integer = 44100 'Used in the ffmpeg conversion
    'Some defaults can be modified here. ffmpeg will force encoding and sample rate. The rest should be changed in resources\template.json.
    'Encoding is typically FLAC
    'If uri done manually, replace https://storage.googleapis.com/ with gs://
End Module