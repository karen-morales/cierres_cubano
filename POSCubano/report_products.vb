Public Class report_products

    Private Sub reporteProductos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
        'fillComboByMySqlDX(GridControl1, "SELECT idTienda,tienda FROM tiendas;")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'GridControl1.ExportToPdf("d:\name.pdf")
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim fecha As Date = fini.Value
        Dim finim As String = fecha.ToString("yyyy-MM-dd") + " 00:00:00"
        fecha = ffin.Value
        Dim ffinm As String = fecha.ToString("yyyy-MM-dd") + " 23:59:59"    

        GridView1.Columns.Clear()

        If CheckBox1.Checked Then
            If CheckBox2.Checked Then
                fillDataGridByMySqlrDX(GridControl69, String.Format("SELECT a.descripcion, SUM(a.cantidad) AS cantidad, SUM(a.precio) AS precio, a.fecha FROM facturas_detalle a INNER JOIN producto_tienda b ON (a.descripcion = b.idproductoTienda) WHERE a.fecha BETWEEN '{0}' AND '{1}' AND a.idTienda = '{2}' GROUP BY a.descripcion;", finim, ffinm, tiendas.SelectedValue))
                'Label5.Text = String.Format("{0:n2}", CDbl(executeQueryr(String.Format("SELECT sum(ventas_detalle.precio) FROM ventas_detalle INNER JOIN ventas ON (ventas.idVentaLocal = ventas_detalle.idVenta) WHERE ventas_detalle.fecha BETWEEN '{0}' AND '{1}' AND ventas.fecha BETWEEN '{0}' AND '{1}' AND ventas_detalle.idTienda = '{2}' AND ventas.facturada > 0", finim, ffinm, tiendas.SelectedValue))))
            Else
                fillDataGridByMySqlrDX(GridControl69, String.Format("SELECT a.descripcion, a.cantidad, a.precio, a.fecha FROM facturas_detalle a INNER JOIN producto_tienda b ON (a.descripcion = b.idproductoTienda) WHERE a.fecha BETWEEN '{0}' AND '{1}' AND a.idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))
                'Label5.Text = String.Format("{0:n2}", CDbl(executeQueryr(String.Format("SELECT sum(a.precio) FROM facturas_detalle a INNER JOIN producto_tienda b ON (a.descripcion = b.idproductoTienda) WHERE a.fecha BETWEEN '{0}' AND '{1}' AND a.idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))))
            End If
        Else
            If CheckBox2.Checked Then
                fillDataGridByMySqlrDX(GridControl69, String.Format("SELECT fecha as Fecha,idproductoTienda as Producto,sum(cantidad) as Cantidad,round(sum(precio),2) as Total FROM ventas_detalle WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}' GROUP BY idproductoTienda;", finim, ffinm, tiendas.SelectedValue))
                'Label5.Text = String.Format("{0:n2}", CDbl(executeQueryr(String.Format("SELECT sum(precio) FROM ventas_detalle WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))))
            Else
                fillDataGridByMySqlrDX(GridControl69, String.Format("SELECT * FROM ventas_detalle WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))
                'Label5.Text = String.Format("{0:n2}", CDbl(executeQueryr(String.Format("SELECT sum(precio) FROM ventas_detalle WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))))
            End If
        End If
        GridView1.OptionsBehavior.Editable = False
        GridView1.OptionsFind.AlwaysVisible = True
        GridControl69.RefreshDataSource()
        GridView1.RefreshData()
        GridView1.BestFitColumns() 
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim NombreArchivo As String = System.IO.Path.GetTempFileName + ".xls"
        GridView1.OptionsPrint.AutoWidth = True
        GridControl69.ExportToXls(NombreArchivo)
        System.Diagnostics.Process.Start(NombreArchivo)
    End Sub
End Class