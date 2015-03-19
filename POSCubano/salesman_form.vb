Public Class salesman_form
    Private Sub salesman_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Me.Text = "Agregar vendedor" Then
            TextBox1.Text = ""
            TextBox2.Text = ""
        Else
            TextBox1.Text = salesman_tree.grid1(1, salesman_tree.grid1.CurrentRow.Index).Value
            TextBox2.Text = salesman_tree.grid1(2, salesman_tree.grid1.CurrentRow.Index).Value
        End If
    End Sub

    Private Sub btnAgregarCliente_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        If TextBox1.Text <> "" And TextBox2.Text <> "" Then
            Try
                If Me.Text = "Agregar vendedor" Then
                    executeQueryr(String.Format("INSERT INTO vendedores VALUES('','{0}','{1}')", TextBox1.Text, TextBox2.Text))
                Else
                    Dim id_vendedor As Integer = salesman_tree.grid1(0, salesman_tree.grid1.CurrentRow.Index).Value
                    executeQueryr("UPDATE vendedores SET nombre = '" + TextBox1.Text + "', tienda = '" + TextBox2.Text + "' WHERE id = '" + salesman_tree.grid1(0, salesman_tree.grid1.CurrentRow.Index).Value.ToString + "'")
                End If
                Me.Close()
            Catch ex As Exception
                MsgBox("Debe escribir un valor válido.")
            End Try
        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class