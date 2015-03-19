Public Class expense_form
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" Then
            Try
                executeQueryr(String.Format("INSERT INTO gastos VALUES('','{0}','{1}','{2}',NOW())", idTienda, TextBox1.Text, CDbl(TextBox2.Text)))
                Me.Close()
            Catch ex As Exception
                MsgBox("Debe escribir un valor válido para el monto.")
            End Try
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub agregarGastoForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub
End Class