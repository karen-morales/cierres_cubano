Imports tagcode.xml

Public Class report_invoice

    Private Sub reporteFacturas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs)
    End Sub

    Private Sub gridReporteVentas_CellDoubleClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridReporteVentas.CellDoubleClick
        Button3_Click(sender, e)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridReporteVentas.SelectedRows()
        For Each row As DataGridViewRow In nots
            Dim folio_fiscal As String = Me.gridReporteVentas(4, row.Index).Value.ToString
            If Len(folio_fiscal) < 1 Then
                MsgBox("Al imprimir una factura que aún no ha sido firmada por el PAC se intentará firmar, este proceso puede demorar un poco por favor sea paciente", MsgBoxStyle.Information)
            End If
            If Me.gridReporteVentas(8, row.Index).Value = "" Then
                generarFactura(Me.gridReporteVentas(0, row.Index).Value)
            Else
                generarFactura(Me.gridReporteVentas(0, row.Index).Value, 1)
            End If
            'End If
        Next
        Button4_Click(sender, e)

    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim fecha As Date = fini.Value
        Dim finim As String = fecha.ToString("yyyy-MM-dd") + " 00:00:00"
        fecha = ffin.Value
        Dim ffinm As String = fecha.ToString("yyyy-MM-dd") + " 23:59:59"
        fillDataGridByMySqlr(gridReporteVentas, String.Format("SELECT a.idfactura,b.nombre,a.fecha,a.folio,a.folio_fiscal,a.monto,a.metodo,a.cuenta,COALESCE(a.notas,'') AS notas FROM facturas a INNER JOIN partners b ON (a.idcliente = b.idcliente) WHERE a.fecha BETWEEN '{0}' AND '{1}' AND a.idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))
        Label5.Text = String.Format("{0:n2}", CDbl(executeQueryr(String.Format("SELECT sum(monto) FROM ventas WHERE fecha BETWEEN '{0}' AND '{1}' AND idTienda = '{2}';", finim, ffinm, tiendas.SelectedValue))))
        gridReporteVentas.AutoResizeColumns()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridReporteVentas.SelectedRows()
        For Each row As DataGridViewRow In nots
            Dim folio_fiscal As String = Me.gridReporteVentas(4, row.Index).Value.ToString
            If Len(folio_fiscal) < 1 Then
                MsgBox("Primero debe timbrar la factura para enviarla por correo, de click en el botón imprimir y se intentará timbrar", MsgBoxStyle.Information)
                Return
            End If
            Dim mailf As mail_form = New mail_form
            mailf.folio = Me.gridReporteVentas(3, row.Index).Value.ToString
            mailf.tienda = tiendas.SelectedValue
            If Me.gridReporteVentas(8, row.Index).Value <> "" Then
                mailf.generic = 1
            End If
            mailf.ShowDialog()
        Next
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        If MsgBox("Esta acción cancelará esta factura en el PAC y ya no tendrá validez fiscal, puede volver a timbrarla posteriormente de nuevo si asi se requiere." + vbCrLf + vbCrLf + "¿Esta seguro de querer cancelar esta factura?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim nots As DataGridViewSelectedRowCollection = Me.gridReporteVentas.SelectedRows()
            For Each row As DataGridViewRow In nots
                Dim folio_fiscal As String = Me.gridReporteVentas(4, row.Index).Value.ToString
                If Len(folio_fiscal) < 1 Then
                    MsgBox("Esta factura no está timbrada por lo tanto no se puede cancelar en el PAC", MsgBoxStyle.Information)
                    Return
                End If

                Dim xml_sign As String = System.IO.Path.GetTempFileName + ".xml"
                Dim xml_cancel As String = System.IO.Path.GetTempFileName + ".xml"

                Dim lic As New LicenciasBuildCFDI
                lic.Licencia(executeQueryTextr(String.Format("SELECT lic FROM tiendas WHERE idTienda = {0};", tiendas.SelectedValue)))

                My.Computer.FileSystem.WriteAllText(xml_sign, executeQueryTextr("SELECT xml_timbrado FROM facturas WHERE folio_fiscal = '" + folio_fiscal + "'"), False)

                Dim s As New Servicios
                Dim res As Boolean = False
                Try
                    Dim cuenta_nip = executeQueryTextr(String.Format("SELECT cuenta_nip FROM tiendas WHERE idTienda = '{0}';", tiendas.SelectedValue))
                    Dim pass_pac = executeQueryTextr(String.Format("SELECT pass_pac FROM tiendas WHERE idTienda = '{0}';", tiendas.SelectedValue))
                    res = s.CancelarCFDI(xml_sign, cuenta_nip, pass_pac, xml_cancel)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

                ' Si fue correcta la cancelacion copiamos el XML y el folio fiscal a la tabla de facturas canceladas
                If res Then
                    executeQueryTextr("INSERT INTO facturas_canceladas VALUES ('','" + folio_fiscal + "','" + executeQueryTextr("SELECT xml_timbrado FROM facturas WHERE folio_fiscal = '" + folio_fiscal + "'") + "',NOW(),'" + idUsuario.ToString + "')")
                    executeQueryTextr("UPDATE facturas SET xml_timbrado = '', folio_fiscal = '' WHERE folio_fiscal = '" + folio_fiscal + "'")
                    MsgBox("Cancelación de factura realizada satisfactoriamente.")
                End If
            Next
        End If
    End Sub

    Private Sub Button5_Click_2(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Dim rsel As Integer = gridReporteVentas.CurrentRow.Index
        Dim notas As String = gridReporteVentas(8, rsel).Value.ToString()
        If Not notas = "" Then
            printNF(gridReporteVentas(8, rsel).Value.ToString(), tiendas.SelectedValue.ToString)
        ElseIf notas = "" Then
            MsgBox("No se puede imprimir. No hay notas")
        End If
    End Sub
End Class