Imports System.Xml
Public Class partner_form

    Private Sub cliente_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If Me.Text.Contains("Editar") Then
            Dim i As Integer = partner_tree.gridClientes.CurrentRow.Index
            txtNombre.Text = partner_tree.gridClientes(1, i).Value.ToString
            txtCalle.Text = partner_tree.gridClientes(3, i).Value.ToString
            txtColonia.Text = partner_tree.gridClientes(5, i).Value.ToString
            txtEstado.Text = partner_tree.gridClientes(7, i).Value.ToString
            txtNumero.Text = partner_tree.gridClientes(4, i).Value.ToString
            txtCiudad.Text = partner_tree.gridClientes(6, i).Value.ToString
            txtCp.Text = partner_tree.gridClientes(8, i).Value.ToString
            txtTelefono.Text = partner_tree.gridClientes(9, i).Value.ToString
            txtRfc.Text = partner_tree.gridClientes(2, i).Value.ToString
            txtEmail.Text = partner_tree.gridClientes(10, i).Value.ToString
        End If
        txtNombre.Focus()
    End Sub

    Private Sub cliente_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        txtNombre.Text = ""
        txtRfc.Text = ""
        txtCalle.Text = ""
        txtColonia.Text = ""
        txtEstado.Text = ""
        txtTelefono.Text = ""
        txtNumero.Text = ""
        txtCiudad.Text = ""
        txtCp.Text = ""
        txtEmail.Text = ""
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        Dim nombre As String = txtNombre.Text
        Dim calle As String = txtCalle.Text
        Dim colonia As String = txtColonia.Text
        Dim estado As String = txtEstado.Text
        Dim numero As String = txtNumero.Text
        Dim ciudad As String = txtCiudad.Text
        Dim cp As String = txtCp.Text
        Dim telefono As String = txtTelefono.Text
        Dim rfc As String = txtRfc.Text

        If Me.Text.Contains("Editar") Then
            Dim idCliente As Integer = partner_tree.gridClientes(0, partner_tree.gridClientes.CurrentRow.Index).Value
            executeQueryr(String.Format("UPDATE partners SET nombre = '{0}',calle = '{1}', numero = '{2}',colonia = '{3}',ciudad = '{4}', estado = '{5}',cp = '{6}',telefono = '{7}',rfc = '{8}',email = '{10}' WHERE idCliente = '{9}';", nombre, calle, numero, colonia, ciudad, estado, cp, telefono, rfc, idCliente, txtEmail.Text))
        Else
            If rfc.Equals("XAXX010101000") Then
                executeQueryr(String.Format("INSERT INTO partners (nombre,calle, numero,colonia,ciudad, estado,cp,telefono,rfc,idtienda,email,tipo)  VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');", nombre, calle, numero, colonia, ciudad, estado, cp, telefono, rfc, idTienda, txtEmail.Text, partner_tree.tipo.ToString))
            Else

                Dim id As Integer = executeQueryr(String.Format("SELECT idCliente FROM partners WHERE rfc='" + rfc + "'"))

                If id = 0 Then
                    executeQueryr(String.Format("INSERT INTO partners (nombre,calle, numero,colonia,ciudad, estado,cp,telefono,rfc,idtienda,email,tipo)  VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');", nombre, calle, numero, colonia, ciudad, estado, cp, telefono, rfc, idTienda, txtEmail.Text, partner_tree.tipo.ToString))
                Else
                    MsgBox("El cliente con rfc " + rfc + " ya está registrado.")
                End If
            End If
        End If

        partner_tree.Button1_Click(sender, e)
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub
End Class