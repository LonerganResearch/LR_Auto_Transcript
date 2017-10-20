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
        Me.sfdExport = New System.Windows.Forms.SaveFileDialog()
        Me.flpOperations = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSettings = New System.Windows.Forms.Button()
        Me.lblProcess = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnImport
        '
        Me.btnImport.BackgroundImage = Global.main.My.Resources.Resources.upload
        Me.btnImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnImport.Location = New System.Drawing.Point(12, 12)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(85, 85)
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
        Me.btnExit.BackgroundImage = Global.main.My.Resources.Resources.quit
        Me.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnExit.Location = New System.Drawing.Point(103, 103)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(85, 85)
        Me.btnExit.TabIndex = 2
        Me.btnExit.UseVisualStyleBackColor = True
        '
        'btnPoll
        '
        Me.btnPoll.BackgroundImage = Global.main.My.Resources.Resources.poll
        Me.btnPoll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnPoll.Enabled = False
        Me.btnPoll.Location = New System.Drawing.Point(103, 12)
        Me.btnPoll.Name = "btnPoll"
        Me.btnPoll.Size = New System.Drawing.Size(85, 85)
        Me.btnPoll.TabIndex = 3
        Me.btnPoll.UseVisualStyleBackColor = True
        '
        'sfdExport
        '
        Me.sfdExport.DefaultExt = "txt"
        '
        'flpOperations
        '
        Me.flpOperations.AutoScroll = True
        Me.flpOperations.Location = New System.Drawing.Point(194, 12)
        Me.flpOperations.Name = "flpOperations"
        Me.flpOperations.Size = New System.Drawing.Size(385, 166)
        Me.flpOperations.TabIndex = 9
        '
        'btnSettings
        '
        Me.btnSettings.BackgroundImage = Global.main.My.Resources.Resources.settings
        Me.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btnSettings.Location = New System.Drawing.Point(12, 103)
        Me.btnSettings.Name = "btnSettings"
        Me.btnSettings.Size = New System.Drawing.Size(85, 85)
        Me.btnSettings.TabIndex = 10
        Me.btnSettings.UseVisualStyleBackColor = True
        '
        'lblProcess
        '
        Me.lblProcess.AutoSize = True
        Me.lblProcess.Location = New System.Drawing.Point(194, 181)
        Me.lblProcess.Name = "lblProcess"
        Me.lblProcess.Size = New System.Drawing.Size(38, 13)
        Me.lblProcess.TabIndex = 11
        Me.lblProcess.Text = "Ready"
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 199)
        Me.Controls.Add(Me.lblProcess)
        Me.Controls.Add(Me.btnSettings)
        Me.Controls.Add(Me.flpOperations)
        Me.Controls.Add(Me.btnPoll)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnImport)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Automatic Transcript v1.3"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnImport As Button
    Friend WithEvents ofdSelect As OpenFileDialog
    Friend WithEvents fbdDir As FolderBrowserDialog
    Friend WithEvents btnExit As Button
    Friend WithEvents btnPoll As Button
    Friend WithEvents sfdExport As SaveFileDialog
    Friend WithEvents flpOperations As FlowLayoutPanel
    Friend WithEvents btnSettings As Button
    Friend WithEvents lblProcess As Label
End Class
