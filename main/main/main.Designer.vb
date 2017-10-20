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
        Me.fbdDir = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnExit = New System.Windows.Forms.Button()
        Me.btnPoll = New System.Windows.Forms.Button()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.sfdExport = New System.Windows.Forms.SaveFileDialog()
        Me.btnGetTranscript = New System.Windows.Forms.Button()
        Me.btnDeleteOp = New System.Windows.Forms.Button()
        Me.flpOperations = New System.Windows.Forms.FlowLayoutPanel()
        Me.SuspendLayout()
        '
        'btnImport
        '
        Me.btnImport.BackgroundImage = Global.main.My.Resources.Resources.upload
        Me.btnImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnImport.Location = New System.Drawing.Point(12, 12)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(80, 80)
        Me.btnImport.TabIndex = 0
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'ofdSelect
        '
        Me.ofdSelect.Multiselect = True
        '
        'fbdDir
        '
        Me.fbdDir.ShowNewFolderButton = False
        '
        'btnExit
        '
        Me.btnExit.BackgroundImage = Global.main.My.Resources.Resources._exit
        Me.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnExit.Location = New System.Drawing.Point(12, 184)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(80, 80)
        Me.btnExit.TabIndex = 2
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPoll
        '
        Me.btnPoll.BackgroundImage = Global.main.My.Resources.Resources.poll
        Me.btnPoll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnPoll.Enabled = False
        Me.btnPoll.Location = New System.Drawing.Point(12, 98)
        Me.btnPoll.Name = "btnPoll"
        Me.btnPoll.Size = New System.Drawing.Size(80, 80)
        Me.btnPoll.TabIndex = 3
        Me.btnPoll.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(429, 184)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(80, 23)
        Me.btnTest.TabIndex = 4
        Me.btnTest.Text = "Test Button"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        Me.btnReset.Location = New System.Drawing.Point(429, 213)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(80, 23)
        Me.btnReset.TabIndex = 5
        Me.btnReset.Text = "Reset Settings"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'sfdExport
        '
        Me.sfdExport.DefaultExt = "txt"
        '
        'btnGetTranscript
        '
        Me.btnGetTranscript.BackgroundImage = Global.main.My.Resources.Resources.download
        Me.btnGetTranscript.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnGetTranscript.Enabled = False
        Me.btnGetTranscript.Location = New System.Drawing.Point(429, 12)
        Me.btnGetTranscript.Name = "btnGetTranscript"
        Me.btnGetTranscript.Size = New System.Drawing.Size(80, 80)
        Me.btnGetTranscript.TabIndex = 7
        Me.btnGetTranscript.UseVisualStyleBackColor = True
        '
        'btnDeleteOp
        '
        Me.btnDeleteOp.BackgroundImage = Global.main.My.Resources.Resources.delete
        Me.btnDeleteOp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnDeleteOp.Enabled = False
        Me.btnDeleteOp.Location = New System.Drawing.Point(429, 98)
        Me.btnDeleteOp.Name = "btnDeleteOp"
        Me.btnDeleteOp.Size = New System.Drawing.Size(80, 80)
        Me.btnDeleteOp.TabIndex = 8
        Me.btnDeleteOp.UseVisualStyleBackColor = True
        '
        'flpOperations
        '
        Me.flpOperations.AutoScroll = True
        Me.flpOperations.Location = New System.Drawing.Point(98, 12)
        Me.flpOperations.Name = "flpOperations"
        Me.flpOperations.Size = New System.Drawing.Size(325, 252)
        Me.flpOperations.TabIndex = 9
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(517, 282)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.flpOperations)
        Me.Controls.Add(Me.btnDeleteOp)
        Me.Controls.Add(Me.btnGetTranscript)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.btnPoll)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnImport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Transcript v1.3"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnImport As Button
    Friend WithEvents ofdSelect As OpenFileDialog
    Friend WithEvents fbdDir As FolderBrowserDialog
    Friend WithEvents btnExit As Button
    Friend WithEvents btnPoll As Button
    Friend WithEvents btnTest As Button
    Friend WithEvents btnReset As Button
    Friend WithEvents sfdExport As SaveFileDialog
    Friend WithEvents btnGetTranscript As Button
    Friend WithEvents btnDeleteOp As Button
    Friend WithEvents flpOperations As FlowLayoutPanel
End Class
