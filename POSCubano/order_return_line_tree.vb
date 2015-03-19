Public Class order_return_line_tree
    Public id As String
    Public tienda As String
    Public tiendan As String
    Public fecha As String

    Private Sub order_return_line_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim whereID = ""
        If id = "" Then
            whereID = "IS NULL"
        Else
            whereID = "= " + id
        End If
        If Me.Text = "Pedidos a bodega" Then
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.producto_id AS Código, b.nombre AS Producto, a.fecha_pedido AS Fecha, a.cantidad AS Cantidad, a.comentario AS Comentario FROM tienda_pedido a INNER JOIN producto b ON a.producto_id = b.id WHERE listaPedidos_id " + whereID + " ORDER BY b.nombre")
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.producto_id AS Código, b.nombre AS Producto, a.fecha_devolucion AS Fecha, a.cantidad AS Cantidad, a.comentario AS Comentario FROM tienda_devolucion a INNER JOIN producto b ON a.producto_id = b.id WHERE a.listaDevoluciones_id " + whereID + " ORDER BY b.nombre")
        End If
        GridView1.Columns("Fecha").DisplayFormat.FormatString = "YYYY-MM-DD HH:mm:ss"
        GridView1.BestFitColumns()
    End Sub

    Private Sub btnPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnPrint.Click
        If Me.Text = "Pedidos a bodega" Then
            export2PDF(GridControl1, "Pedido de bodega procesado", "Tienda " + tienda + ", " + tiendan + " | Folio: " + id.ToString + " | Realizado: " + fecha)
        Else
            export2PDF(GridControl1, "Devolución de bodega procesada", "Tienda " + tienda + ", " + tiendan + " | Folio: " + id.ToString + " | Realizado: " + fecha)
        End If
    End Sub
End Class