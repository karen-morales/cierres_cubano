Public Class product_history_tree

    Private Sub product_history_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, " SELECT 999,'Bodega' UNION SELECT idTienda,tienda FROM tiendas;")
        tiendas.Enabled = False
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        fillDataGridByMySqlrDX(GridControl1, "CALL product_history('" + txtProduct.Text + "','" + DateTimePicker1.Value.ToString("yyyy-MM-dd 00:00:01") + "','" + DateTimePicker2.Value.ToString("yyyy-MM-dd 23:59:59") + "')")
        If GridView1.Columns("Cantidad").Summary.ActiveCount = 0 Then
            GridView1.Columns("Cantidad").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Cantidad", "{0:#,##0.00}")
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        export2PDF(GridControl1, "Historial de producto", "Producto: " + txtProduct.Text + " | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
    End Sub
End Class