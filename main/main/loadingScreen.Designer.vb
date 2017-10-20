<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class loadingScreen
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
        Me.pgbCurrentTask = New System.Windows.Forms.ProgressBar()
        Me.pgbTotalTasks = New System.Windows.Forms.ProgressBar()
        Me.lblJobsLeft = New System.Windows.Forms.Label()
        Me.lblCurrentTask = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'pgbCurrentTask
        '
        Me.pgbCurrentTask.Location = New System.Drawing.Point(12, 82)
        Me.pgbCurrentTask.Maximum = 4
        Me.pgbCurrentTask.Name = "pgbCurrentTask"
        Me.pgbCurrentTask.Size = New System.Drawing.Size(300, 23)
        Me.pgbCurrentTask.Step = 1
        Me.pgbCurrentTask.TabIndex = 0
        '
        'pgbTotalTasks
        '
        Me.pgbTotalTasks.Location = New System.Drawing.Point(12, 25)
        Me.pgbTotalTasks.Name = "pgbTotalTasks"
        Me.pgbTotalTasks.Size = New System.Drawing.Size(300, 23)
        Me.pgbTotalTasks.TabIndex = 1
        '
        'lblJobsLeft
        '
        Me.lblJobsLeft.AutoSize = True
        Me.lblJobsLeft.Location = New System.Drawing.Point(12, 9)
        Me.lblJobsLeft.Name = "lblJobsLeft"
        Me.lblJobsLeft.Size = New System.Drawing.Size(32, 13)
        Me.lblJobsLeft.TabIndex = 2
        Me.lblJobsLeft.Text = "x of x"
        '
        'lblCurrentTask
        '
        Me.lblCurrentTask.AutoSize = True
        Me.lblCurrentTask.Location = New System.Drawing.Point(12, 66)
        Me.lblCurrentTask.Name = "lblCurrentTask"
        Me.lblCurrentTask.Size = New System.Drawing.Size(64, 13)
        Me.lblCurrentTask.TabIndex = 3
        Me.lblCurrentTask.Text = "Current task"
        '
        'loadingScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(324, 122)
        Me.Controls.Add(Me.lblCurrentTask)
        Me.Controls.Add(Me.lblJobsLeft)
        Me.Controls.Add(Me.pgbTotalTasks)
        Me.Controls.Add(Me.pgbCurrentTask)
        Me.Name = "loadingScreen"
        Me.Text = "Processing..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pgbCurrentTask As ProgressBar
    Friend WithEvents pgbTotalTasks As ProgressBar
    Friend WithEvents lblJobsLeft As Label
    Friend WithEvents lblCurrentTask As Label
End Class
