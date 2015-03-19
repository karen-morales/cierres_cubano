Public Class catalogo_form
    Public table As String
    Public id As String
    Public nombre As String

    Private Sub catalogo_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Me.Text = "Editar datos." Then
            TextBox1.Text = id
            TextBox2.Text = nombre
            TextBox1.Enabled = False
        Else
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox1.Enabled = True
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        If Me.Text = "Editar datos." Then
            executeQueryTextr("UPDATE " + table + " SET nombre = '" + TextBox2.Text + "', id = '" + TextBox1.Text + "' WHERE id = '" + id + "'")
            Me.Close()
        Else
            ' Revisamos si el ID existe
            If executeQueryTextr("SELECT id FROM " + table + " WHERE id = '" + TextBox1.Text + "'") = "" Then
                executeQueryTextr("INSERT INTO " + table + " (id,nombre) VALUES('" + TextBox1.Text + "','" + TextBox2.Text + "')")
                Me.Close()
            Else
                MsgBox("El ID elegido ya esta utilizado por favor ingrese uno nuevo")
            End If
        End If
    End Sub
End Class