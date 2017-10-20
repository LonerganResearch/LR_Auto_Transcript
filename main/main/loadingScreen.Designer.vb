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
        Me.lblCurrentTask = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'pgbCurrentTask
        '
        Me.pgbCurrentTask.Location = New System.Drawing.Point(12, 25)
        Me.pgbCurrentTask.Maximum = 4
        Me.pgbCurrentTask.Name = "pgbCurrentTask"
        Me.pgbCurrentTask.Size = New System.Drawing.Size(300, 23)
        Me.pgbCurrentTask.Step = 1
        Me.pgbCurrentTask.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pgbCurrentTask.TabIndex = 0
        '
        'lblCurrentTask
        '
        Me.lblCurrentTask.AutoSize = True
        Me.lblCurrentTask.Location = New System.Drawing.Point(12, 9)
        Me.lblCurrentTask.Name = "lblCurrentTask"
        Me.lblCurrentTask.Size = New System.Drawing.Size(64, 13)
        Me.lblCurrentTask.TabIndex = 3
        Me.lblCurrentTask.Text = "Current task"
        '
        'loadingScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(324, 63)
        Me.Controls.Add(Me.lblCurrentTask)
        Me.Controls.Add(Me.pgbCurrentTask)
        Me.Name = "loadingScreen"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Processing..."
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pgbCurrentTask As ProgressBar
    Friend WithEvents lblCurrentTask As Label
End Class
