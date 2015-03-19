Public Class partner_tree
    Public tipo = 0
    'Cuando e carga el form
    Private Sub clientes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button1_Click(sender, e)
    End Sub
    'Doble click en una columna del grid clientes
    Private Sub gridClientes_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridClientes.CellDoubleClick
        editPartner()
    End Sub
    'Agregar un cliente
    Private Sub btnAgregarCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAgregarCliente.Click
        If tipo = 0 Then
            partner_form.Text = "Agregar cliente"
        Else
            partner_form.Text = "Agregar proveedor"
        End If
        partner_form.Show()
    End Sub

    Public Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        'Cargamos el grid con los datos de clientes
            fillDataGridByMySqlr(gridClientes, "SELECT idCliente AS ID, nombre AS Nombre, rfc AS RFC, calle AS Calle, numero AS No, colonia AS Colonia, ciudad AS Ciudad, estado AS Estado, cp AS CP, telefono AS Telefono, email AS Correo FROM partners WHERE tipo = " + tipo.ToString)
            gridClientes.AutoResizeColumns()

    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        editPartner()
    End Sub

    Private Sub gridClientes_DoubleClick(sender As Object, e As System.EventArgs) Handles gridClientes.DoubleClick
        editPartner()
    End Sub

    Public Function editPartner()
        If tipo = 0 Then
            partner_form.Text = "Editar cliente"
        Else
            partner_form.Text = "Editar proveedor"
        End If
        partner_form.Show()
    End Function
End Class