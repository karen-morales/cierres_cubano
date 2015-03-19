Public Class report_sales

    Private Sub reporteVentas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridReporteVentas.SelectedRows()
        For Each row As DataGridViewRow In nots
            Dim id As Integer = executeQueryr(String.Format("SELECT idFactura FROM facturas WHERE folio = {0} AND idTienda = {1}", Me.gridReporteVentas(13, row.Index).Value, tiendas.SelectedValue))
            If id > 0 Then
                generarFactura(id, 1, Me.gridReporteVentas(12, row.Index).Value)
            Else
                MsgBox("Esta nota no ha sido facturada")
            End If
            'End If
        Next
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Dim fecha As Date = fini.Value
        Dim finim As String = fecha.ToString("yyyy-MM-dd") + " 00:00:00"
        fecha = ffin.Value
        Dim ffinm As String = fecha.ToString("yyyy-MM-dd") + " 23:59:59"
        fillDataGridByMySqlr(gridReporteVentas, String.Format("SELECT idVenta AS ID,fecha AS Fecha,monto AS Total,foliosat AS 'Folio SAT',verificada AS Revisada,metodo AS 'Metodo de pago',vendedor AS Vendedor,facturada AS Facturada,folio AS Folio,factura_generica AS 'Folio factura generica',folio_factura AS 'Folio factura' FROM ventas WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))
        Label5.Text = String.Format("{0:n2}", CDbl(executeQueryr(String.Format("SELECT sum(monto) FROM ventas WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))))
        gridReporteVentas.AutoResizeColumns()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridReporteVentas.SelectedRows()
        For Each row As DataGridViewRow In nots
            generarNota(Me.gridReporteVentas(0, row.Index).Value.ToString)
        Next
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridReporteVentas.SelectedRows()
        For Each row As DataGridViewRow In nots
            generarNota(Me.gridReporteVentas(0, row.Index).Value.ToString, True)
        Next
    End Sub
End Class