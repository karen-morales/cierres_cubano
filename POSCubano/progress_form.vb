Public Class progress_form

    Private Sub progress_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lblText.Text = "Iniciando proceso."
        pg.Increment(1)
        pg.Properties.PercentView = True
    End Sub

    Public Function update_status(ByVal value As Integer, ByVal msg As String)
        lblText.Text = msg
        pg.EditValue = value
        My.Application.DoEvents()
        Return True
    End Function
End Class