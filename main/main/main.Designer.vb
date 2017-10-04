<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.ofdSelect = New System.Windows.Forms.OpenFileDialog()
        Me.btnTranscribe = New System.Windows.Forms.Button()
        Me.fbdDir = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPoll = New System.Windows.Forms.Button()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(12, 12)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(162, 23)
        Me.btnImport.TabIndex = 0
        Me.btnImport.Text = "Import Audio File(s)"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'ofdSelect
        '
        Me.ofdSelect.Multiselect = True
        '
        'btnTranscribe
        '
        Me.btnTranscribe.Location = New System.Drawing.Point(12, 41)
        Me.btnTranscribe.Name = "btnTranscribe"
        Me.btnTranscribe.Size = New System.Drawing.Size(162, 23)
        Me.btnTranscribe.TabIndex = 1
        Me.btnTranscribe.Text = "Transcribe Audio File(s)"
        Me.btnTranscribe.UseVisualStyleBackColor = True
        '
        'fbdDir
        '
        Me.fbdDir.ShowNewFolderButton = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(12, 174)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(89, 23)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPoll
        '
        Me.btnPoll.Location = New System.Drawing.Point(12, 70)
        Me.btnPoll.Name = "btnPoll"
        Me.btnPoll.Size = New System.Drawing.Size(162, 23)
        Me.btnPoll.TabIndex = 3
        Me.btnPoll.Text = "Poll Operations"
        Me.btnPoll.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(107, 174)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(162, 23)
        Me.btnTest.TabIndex = 4
        Me.btnTest.Text = "Test Button"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(12, 145)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(162, 23)
        Me.btnReset.TabIndex = 5
        Me.btnReset.Text = "Reset Settings"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(345, 209)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.btnPoll)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnTranscribe)
        Me.Controls.Add(Me.btnImport)
        Me.Name = "main"
        Me.Text = "Automatic Transcript v0.7"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnImport As Button
    Friend WithEvents ofdSelect As OpenFileDialog
    Friend WithEvents btnTranscribe As Button
    Friend WithEvents fbdDir As FolderBrowserDialog
    Friend WithEvents btnExit As Button
    Friend WithEvents btnPoll As Button
    Friend WithEvents btnTest As Button
    Friend WithEvents btnReset As Button
End Class
