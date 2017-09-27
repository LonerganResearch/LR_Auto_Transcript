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
        Me.btnSelectSAKey = New System.Windows.Forms.Button()
        Me.fbdDir = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(12, 12)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(162, 23)
        Me.btnImport.TabIndex = 0
        Me.btnImport.Text = "Import Audio File"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'ofdSelect
        '
        Me.ofdSelect.Filter = "MP3 files (.mp3)|*.mp3"
        '
        'btnSelectSAKey
        '
        Me.btnSelectSAKey.Location = New System.Drawing.Point(12, 41)
        Me.btnSelectSAKey.Name = "btnSelectSAKey"
        Me.btnSelectSAKey.Size = New System.Drawing.Size(162, 23)
        Me.btnSelectSAKey.TabIndex = 1
        Me.btnSelectSAKey.Text = "Select Service Account Key"
        Me.btnSelectSAKey.UseVisualStyleBackColor = True
        '
        'fbdDir
        '
        Me.fbdDir.ShowNewFolderButton = False
        '
        'btnExit
        '
        Me.btnExit.Location = New System.Drawing.Point(12, 112)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(89, 23)
        Me.btnExit.TabIndex = 2
        Me.btnExit.Text = "Exit"
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(287, 147)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnSelectSAKey)
        Me.Controls.Add(Me.btnImport)
        Me.Name = "main"
        Me.Text = "Automatic Transcript v0.4"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnImport As Button
    Friend WithEvents ofdSelect As OpenFileDialog
    Friend WithEvents btnSelectSAKey As Button
    Friend WithEvents fbdDir As FolderBrowserDialog
    Friend WithEvents btnExit As Button
End Class
