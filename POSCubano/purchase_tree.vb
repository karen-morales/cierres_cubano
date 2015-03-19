Public Class purchase_tree

    Private Sub purchase_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        updateGrid()

        If rol = 3 Then
            btnAdd.Show()
            GridView1.Columns("Total").Visible = True
        Else
            btnAdd.Hide()
            GridView1.Columns("Total").Visible = False
        End If
    End Sub

    Public Function updateGrid() As Integer
        fillDataGridByMySqlrDX(GridControl1, "SELECT a.identrada_producto AS ID, a.numero_factura AS Factura, a.fecha_entrada AS Fecha, c.nombre AS Proveedor, a.total AS Total, b.nombre AS Usuario,c.idCliente AS proveedor_id FROM entrada_producto a INNER JOIN usuarios b ON (a.usuarios_idusuarios = b.idusuarios) INNER JOIN partners c ON(c.idCliente = a.proveedores_id) ORDER BY id DESC")
        GridView1.Columns.Item("proveedor_id").Visible = False
        updateGrid = 1
    End Function

    Private Sub btnDetail_Click(sender As System.Object, e As System.EventArgs) Handles btnDetail.Click
        If GridView1.GetSelectedRows.Length > 0 Then
            purchase_form.Text = "Detalle de compra"
            purchase_form.ShowDialog()
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridControl1.DoubleClick
        If GridView1.GetSelectedRows.Length > 0 Then
            purchase_form.Text = "Detalle de compra"
            purchase_form.ShowDialog()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        purchase_form.Text = "Agregar compra"
        purchase_form.ShowDialog()
    End Sub

    Private Sub btnRefresh_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click
        updateGrid()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim fecha As Date = fini.Value
        Dim finim As String = fecha.ToString("yyyy-MM-dd") + " 00:00:00"
        fecha = ffin.Value
        Dim ffinm As String = fecha.ToString("yyyy-MM-dd") + " 23:59:59"
        fillDataGridByMySqlrDX(GridControl1, String.Format("SELECT a.identrada_producto AS ID, a.numero_factura AS Factura, a.fecha_entrada AS Fecha, c.nombre AS Proveedor, a.total AS Total, b.nombre AS Usuario,c.idCliente AS proveedor_id FROM entrada_producto a INNER JOIN usuarios b ON (a.usuarios_idusuarios = b.idusuarios) INNER JOIN partners c ON(c.idCliente = a.proveedores_id) WHERE a.fecha_entrada BETWEEN '{0}' AND '{1}' ORDER BY id DESC;", finim, ffinm))
        GridView1.Columns.Item("proveedor_id").Visible = False
    End Sub
End Class