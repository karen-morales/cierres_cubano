Public Class order_return_tree

    Private Sub order_return_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
        btnRefresh_Click(sender, e)
    End Sub

    Private Sub btnRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click
        If Me.Text = "Pedidos a bodega" Then
            fillDataGridByMySqlrDX(GridControl1, "SELECT '' AS ID, 'Pendientes' AS Fecha UNION SELECT a.id AS ID, a.fecha AS Fecha FROM lista_pedidos AS a INNER JOIN tienda_pedido AS b ON a.id = b.listaPedidos_id WHERE b.tienda_id = " + tiendas.SelectedValue.ToString + " ORDER BY fecha DESC")
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT '' AS ID, 'Pendientes' AS Fecha UNION SELECT a.id AS ID, a.fecha AS Fecha FROM lista_devoluciones AS a INNER JOIN tienda_pedido AS b ON a.id = b.listaPedidos_id WHERE b.tienda_id = " + tiendas.SelectedValue.ToString + " ORDER BY fecha DESC")
        End If
        GridView1.BestFitColumns()
    End Sub

    Private Sub btnDetail_Click(sender As System.Object, e As System.EventArgs) Handles btnDetail.Click
        If GridView1.SelectedRowsCount > 0 Then
            order_return_line_tree.id = GridView1.GetFocusedRowCellValue("ID").ToString()
            order_return_line_tree.tienda = tiendas.SelectedValue.ToString
            order_return_line_tree.tiendan = tiendas.Text
            order_return_line_tree.fecha = GridView1.GetFocusedRowCellValue("Fecha").ToString()
            If Me.Text = "Pedidos a bodega" Then
                order_return_line_tree.Text = "Pedidos a bodega"
            Else
                order_return_line_tree.Text = "Devoluciones a bodega"
            End If
            order_return_line_tree.Show()
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridControl1.DoubleClick
        btnDetail_Click(sender, e)
    End Sub
End Class