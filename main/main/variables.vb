Module variables 'Fix this up to let the user change stuff
    Public cirsfile As New CIRS_lib.file 'Import library
    Public apiKey As String = "AIzaSyCwp5oY4JiLi36scDCTJizQ0PDCC1R3rTg"
    Public authToken As String = ""
    Public bucket As String = "lr_test_transcript"
    'Some defaults can be modified here. ffmpeg will force encoding and sample rate. The rest should be changed in resources\template.json.
    'Encoding is typically FLAC
    'If uri done manually, replace https://storage.googleapis.com/ with gs://
End Module

'Get file sample rate
'ffprobe -v error -show_format -show_streams test.mp3 > hi.txt
'
'Post .json for transcription
'curl -X POST -d @"test.json" https://speech.googleapis.com/v1/speech:longrunningrecognize?key=AIzaSyCwp5oY4JiLi36scDCTJizQ0PDCC1R3rTg --header "Content-Type:application/json" > output.txt
'
'Get job status
'curl -X GET https://speech.googleapis.com/v1/operations/7076749533424581651?key=AIzaSyCwp5oY4JiLi36scDCTJizQ0PDCC1R3rTg
'
'Example job in list
'8635689709546489895|job_name
'
'Upload file to be transcribed
'curl -v --upload-file "C:\Users\rei.kaneko.LONERGAN\Downloads\test - Copy.flac" -H "Authorization: Bearer ya29.ElrmBHcYn3jaKRjiXQSu7WVwSFOhY_EZSDnLe6y_Cgo7m-jpIW2UUGZBgPjU_0WijNf5mc0Mbwhxj_pnHHYqfraLLzrhmIXj23b3mZh59vjrjzh5c5-tFAVLppk" -H "Content-Type:audio/flac" ""https://storage.googleapis.com/lr_test_transcript/test%20Copy.flac""
'
'gcloud SDK silent install
'GoogleCloudSDKInstaller /S /allusers /noreporting /nodesktop /D="install_path"
'
'Activate a service account key file
'gcloud auth activate-service-account --key-file="key_file_path"
'
'Make a file public
'curl -X POST --data-binary @"public.json" -H "Authorization: Bearer ya29.ElrmBOaZKxNmi-HYuSPzyJ7eCMSa1SM4lfqPQw-VDJWyIor4kWvP3rWCOkMN1hYKfLQwYfgB4b2FD6vk1My5rZ3HDxlOhOMQIISjQYmtT0FHZjkaF8xm_Sj1jMc" -H "Content-Type:application/json" "https://www.googleapis.com/storage/v1/b/lr_test_transcript/o/test.flac/acl"
'
'Set variable (vb has its own way of doing this via Environment.SetEnvironmnetVariable())
'set GOOGLE_APPLICATION_CREDENTIALS=L:\Company Administration\Transcripts\serviceAccountKey.json
'
'Print access token
'gcloud auth application-default print-access-token