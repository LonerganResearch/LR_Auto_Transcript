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
        Me.btnPoll.Enabled = False
        Me.btnPoll.Location = New System.Drawing.Point(12, 41)
        Me.btnPoll.Name = "btnPoll"
        Me.btnPoll.Size = New System.Drawing.Size(162, 23)
        Me.btnPoll.TabIndex = 3
        Me.btnPoll.Text = "Poll Operations"
        Me.btnPoll.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(12, 70)
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
        'sfdExport
        '
        Me.sfdExport.DefaultExt = "txt"
        '
        'btnGetTranscript
        '
        Me.btnGetTranscript.Enabled = False
        Me.btnGetTranscript.Location = New System.Drawing.Point(251, 168)
        Me.btnGetTranscript.Name = "btnGetTranscript"
        Me.btnGetTranscript.Size = New System.Drawing.Size(162, 23)
        Me.btnGetTranscript.TabIndex = 7
        Me.btnGetTranscript.Text = "Get Transcript"
        Me.btnGetTranscript.UseVisualStyleBackColor = True
        '
        'btnDeleteOp
        '
        Me.btnDeleteOp.Enabled = False
        Me.btnDeleteOp.Location = New System.Drawing.Point(419, 168)
        Me.btnDeleteOp.Name = "btnDeleteOp"
        Me.btnDeleteOp.Size = New System.Drawing.Size(162, 23)
        Me.btnDeleteOp.TabIndex = 8
        Me.btnDeleteOp.Text = "Delete Operation"
        Me.btnDeleteOp.UseVisualStyleBackColor = True
        '
        'flpOperations
        '
        Me.flpOperations.Location = New System.Drawing.Point(180, 12)
        Me.flpOperations.Name = "flpOperations"
        Me.flpOperations.Size = New System.Drawing.Size(401, 150)
        Me.flpOperations.TabIndex = 9
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 206)
        Me.Controls.Add(Me.flpOperations)
        Me.Controls.Add(Me.btnDeleteOp)
        Me.Controls.Add(Me.btnGetTranscript)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.btnPoll)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnImport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Transcript v0.9"
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
