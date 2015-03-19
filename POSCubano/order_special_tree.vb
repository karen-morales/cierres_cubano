Public Class order_special_tree

    Private Sub order_special_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        updateGrid()

        If rol = 2 Then
            btnAdd.Hide()
        Else
            btnAdd.Show()
        End If
    End Sub

    Public Function updateGrid() As Integer
        If rol = 1 Then
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, a.fecha_pedidoEspecial AS Fecha, c.tienda AS Tienda, a.comentario AS Comentario, b.nombre AS Usuario FROM pedidos_especiales a INNER JOIN usuarios b ON (a.usuario = b.idusuarios) INNER JOIN tiendas c ON (c.idTienda = a.tienda) WHERE a.tienda = " + idTienda + " ORDER BY a.id DESC LIMIT 100")
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, a.fecha_pedidoEspecial AS Fecha, c.tienda AS Tienda, a.comentario AS Comentario, b.nombre AS Usuario FROM pedidos_especiales a INNER JOIN usuarios b ON (a.usuario = b.idusuarios) INNER JOIN tiendas c ON (c.idTienda = a.tienda) ORDER BY a.id DESC LIMIT 100")
        End If
        updateGrid = 1
    End Function

    Private Sub GridView1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridView1.DoubleClick
        If GridView1.GetSelectedRows.Length > 0 Then
            order_special_form.Text = "Detalle de pedido"
            order_special_form.ShowDialog()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        order_special_form.Text = "Crear pedido especial"
        order_special_form.ShowDialog()
    End Sub

    Private Sub btnRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click
        updateGrid()
    End Sub

    Private Sub btnDetail_Click(sender As System.Object, e As System.EventArgs) Handles btnDetail.Click
        If GridView1.GetSelectedRows.Length > 0 Then
            order_special_form.Text = "Detalle de pedido"
            order_special_form.ShowDialog()
        End If
    End Sub
End Class