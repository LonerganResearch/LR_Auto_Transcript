Public Class loadingScreen
    Private Sub loadingScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        While runningTasks = True
            lblCurrentTask.Text = processText(taskStep)
            pgbCurrentTask.Step = taskStep
        End While
    End Sub
End Class