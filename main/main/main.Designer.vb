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
        Me.dgJobs = New System.Windows.Forms.DataGridView()
        Me.jobNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.jobName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.jobStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnGetTranscript = New System.Windows.Forms.Button()
        Me.btnDeleteOp = New System.Windows.Forms.Button()
        CType(Me.dgJobs, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.sfdExport.FileName = "Output"
        Me.sfdExport.Filter = "Text file|*.txt"
        '
        'dgJobs
        '
        Me.dgJobs.AllowUserToAddRows = False
        Me.dgJobs.AllowUserToDeleteRows = False
        Me.dgJobs.AllowUserToResizeRows = False
        Me.dgJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgJobs.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.jobNumber, Me.jobName, Me.jobStatus})
        Me.dgJobs.Location = New System.Drawing.Point(180, 12)
        Me.dgJobs.MultiSelect = False
        Me.dgJobs.Name = "dgJobs"
        Me.dgJobs.ReadOnly = True
        Me.dgJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgJobs.Size = New System.Drawing.Size(364, 150)
        Me.dgJobs.TabIndex = 6
        '
        'jobNumber
        '
        Me.jobNumber.HeaderText = "Job Number"
        Me.jobNumber.Name = "jobNumber"
        Me.jobNumber.ReadOnly = True
        '
        'jobName
        '
        Me.jobName.HeaderText = "Job Name"
        Me.jobName.Name = "jobName"
        Me.jobName.ReadOnly = True
        '
        'jobStatus
        '
        Me.jobStatus.HeaderText = "Job Progress"
        Me.jobStatus.Name = "jobStatus"
        Me.jobStatus.ReadOnly = True
        '
        'btnGetTranscript
        '
        Me.btnGetTranscript.Enabled = False
        Me.btnGetTranscript.Location = New System.Drawing.Point(214, 168)
        Me.btnGetTranscript.Name = "btnGetTranscript"
        Me.btnGetTranscript.Size = New System.Drawing.Size(162, 23)
        Me.btnGetTranscript.TabIndex = 7
        Me.btnGetTranscript.Text = "Get Transcript"
        Me.btnGetTranscript.UseVisualStyleBackColor = True
        '
        'btnDeleteOp
        '
        Me.btnDeleteOp.Location = New System.Drawing.Point(382, 168)
        Me.btnDeleteOp.Name = "btnDeleteOp"
        Me.btnDeleteOp.Size = New System.Drawing.Size(162, 23)
        Me.btnDeleteOp.TabIndex = 8
        Me.btnDeleteOp.Text = "Delete Operation"
        Me.btnDeleteOp.UseVisualStyleBackColor = True
        '
        'main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 209)
        Me.Controls.Add(Me.btnDeleteOp)
        Me.Controls.Add(Me.btnGetTranscript)
        Me.Controls.Add(Me.dgJobs)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.btnTest)
        Me.Controls.Add(Me.btnPoll)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnImport)
        Me.Name = "main"
        Me.Text = "Automatic Transcript v0.8"
        CType(Me.dgJobs, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents dgJobs As DataGridView
    Friend WithEvents jobNumber As DataGridViewTextBoxColumn
    Friend WithEvents jobName As DataGridViewTextBoxColumn
    Friend WithEvents jobStatus As DataGridViewTextBoxColumn
    Friend WithEvents btnGetTranscript As Button
    Friend WithEvents btnDeleteOp As Button
End Class
