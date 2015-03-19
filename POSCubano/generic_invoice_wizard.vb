Imports MySql.Data.MySqlClient

Public Class generic_invoice_wizard

    Private Sub facturaGenerica_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
    End Sub
    'Buscar registros de venta
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub btnFacturaGenerica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Function calculate() As Integer
        Dim montof As Double = 0
        Dim montosf As Double = 0
        Dim montopf As Double = 0

        'Validamos que solo selccionen las filas sin facturar
        Dim nots As DataGridViewSelectedRowCollection = gridFacturaGenerica.SelectedRows()
        For Each row As DataGridViewRow In nots
            If row.Selected And row.Cells(5).Value Then
                row.Selected = False
            End If
        Next

        'Calculamos los montos
        For i = 0 To Me.gridFacturaGenerica.Rows.Count - 1
            If Me.gridFacturaGenerica(5, i).Value = 0 Then
                Me.gridFacturaGenerica.Rows(i).DefaultCellStyle.BackColor = Color.Orange
            End If
            If Me.gridFacturaGenerica(5, i).Value Then
                montof = String.Format("{0:n2}", montof + Me.gridFacturaGenerica(2, i).Value)
                Me.gridFacturaGenerica.Rows(i).DefaultCellStyle.BackColor = Color.PaleGreen
            ElseIf Me.gridFacturaGenerica.Rows(i).Selected Then
                montopf = String.Format("{0:n2}", montopf + Me.gridFacturaGenerica(2, i).Value)
            Else
                montosf = String.Format("{0:n2}", montosf + Me.gridFacturaGenerica(2, i).Value)
            End If
        Next

        montopf = Math.Round(montopf, 2)
        'mpf.Text = String.Format("{0:n2}", montopf)
        TextBox2.Text = String.Format("{0:n2}", montopf)
        montosf = Math.Round(montosf, 2)
        'msf.Text = String.Format("{0:n2}", montosf)
        TextBox4.Text = String.Format("{0:n2}", montosf)
        montof = Math.Round(montof, 2)
        'mf.Text = String.Format("{0:n2}", montof)
        TextBox1.Text = String.Format("{0:n2}", montof)
        'Label7.Text = String.Format("{0:n2}", montopf + montof)
        TextBox3.Text = String.Format("{0:n2}", montopf + montof)
        calculate = 0
    End Function

    'Cuando cambia el estado de una fila select-unselect
    Private Sub gridFacturaGenerica_RowStateChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowStateChangedEventArgs) Handles gridFacturaGenerica.RowStateChanged
        calculate()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        ' Seleccionamos la tienda
        Dim tienda As String = tiendas.SelectedValue.ToString
        ' Tomamos la fecha de la ultima apertura
        Dim last_close As String = executeQueryTextr("SELECT DATE_FORMAT(fecha_cierre, '%Y-%m-%d %H:%i:%s') FROM aperturas WHERE fecha_cierre IS NOT NULL AND tienda = '" + tienda + "' ORDER BY fecha_apertura ASC")
        Dim last_open As String = executeQueryTextr("SELECT DATE_FORMAT(fecha_apertura, '%Y-%m-%d %H:%i:%s') FROM aperturas WHERE fecha_cierre IS NOT NULL AND tienda = '" + tienda + "' ORDER BY fecha_apertura ASC")

        ' Llenamos el grid
        'fillDataGridByMySqlr(Me.gridFacturaGenerica, "SELECT idVenta,fecha,monto,folio,idTienda,facturada FROM ventas WHERE idTienda = '" + tienda + "' AND verificada = '0' AND idEstatus <> 2 AND fecha < '" + last_close + "'")

        fillDataGridByMySqlr(Me.gridFacturaGenerica, "SELECT idVenta,fecha,monto,folio,idTienda,facturada FROM ventas WHERE idTienda = '" + tienda + "' AND (verificada = '0' AND idEstatus <> 2) AND fecha BETWEEN '" + last_open + "' AND '" + last_close + "'")

        'Calculamos los montos
        For i = 0 To Me.gridFacturaGenerica.Rows.Count - 1
            ' Marcamos todas las notas menores al monto seleccionado
            If Me.gridFacturaGenerica(2, i).Value < monto.Value Then
                Me.gridFacturaGenerica(2, i).Selected = True
            End If
        Next
        calculate()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles btnFacturaGenerica.Click
        'btnFacturaGenerica.Enabled = False
        If gridFacturaGenerica.SelectedRows.Count = 0 Then
            Return
        End If
        'Obtenemos el ultimo folio de las facturas
        Dim tiendaActual As Integer = CInt(tiendas.SelectedValue.ToString)
        Dim folio As Integer = getLastFolioFacturaR(tiendaActual) + 1
        Dim pgf As New progress_form
        Dim conn As MySqlConnection

        'If Not validaFolioFacturr(folio) Then
        'MsgBox("No hay folios disponibles para facturar.")
        'Me.Close()
        'End If

        'Obtenemos el monto de las ventas seleccionadas
        Dim monto As Double = 0
        For Each row As DataGridViewRow In gridFacturaGenerica.Rows
            If row.Selected Then
                monto = monto + CDbl(gridFacturaGenerica(2, row.Index).Value.ToString)
            End If
        Next

        ' Iniciamos transaccion
        conn = mysql_conexion_up()
        mysql_execute("START TRANSACTION;", conn)

        Try
            pgf.Show()
            'Insertamos la factura
            Dim idFactura As Integer = mysql_execute(String.Format("INSERT INTO facturas (idCliente,fecha,idEstatus,folio,idTienda,monto) VALUES (1,NOW(),'1',{1},{2},{3}); SELECT LAST_INSERT_ID() AS id;", "0", folio, tiendaActual, monto.ToString), conn)(0)("id")
            'Dim fgen As Integer = executeQueryr(String.Format("SELECT factura_generica FROM ventas WHERE ", "1", folio, tiendaActual, monto.ToString))
            Dim notas As String = ""

            pgf.update_status(10, "Creando líneas de la factura")
            Dim i As Integer = 0
            Dim foliosat = getFolioSatr(tiendaActual)

            'Insertamos las ventas seleccionadas y marcamos todas la ventas como verificadas 'gridFacturaGenerica.Rows'
            For Each row As DataGridViewRow In gridFacturaGenerica.SelectedRows
                Dim idVenta As String = gridFacturaGenerica(0, row.Index).Value.ToString()

                Dim fecha As Date = gridFacturaGenerica(1, row.Index).Value.ToString()
                Dim fechaForm As String = fecha.ToString("yyyy-MM-dd")

                If row.Selected Then
                    pgf.update_status(10 + (70 / gridFacturaGenerica.SelectedRows.Count), "Creando líneas de la factura")
                    foliosat += 1
                    i += 1
                    ' Copiamos los productos de las notas seleccionadas al detalle de facturas
                    Dim ds As DataSet = New DataSet
                    'foliosNotas = foliosNotas.Substring(0, foliosNotas.Length - 1)
                    ds = fillDataSetByMySqlr(String.Format("SELECT * FROM ventas_detalle WHERE idVenta = '{0}' AND idtienda = '{1}';", idVenta, tiendaActual))
                    'TOTALES                      
                    For Each row2 As DataRow In ds.Tables(0).Rows
                        mysql_execute(String.Format("INSERT INTO facturas_detalle (folio_factura,descripcion,cantidad,precio,fecha,idTienda) VALUES ({0},'{1}',{2},{3},now(),'{4}');", folio, row2("idproductoTienda"), row2("cantidad"), row2("precio") / 1.16, tiendaActual), conn)
                    Next

                    'Dim foliosat = getFolioSatr(tiendaActual) + 1
                    mysql_execute(String.Format("UPDATE ventas SET idEstatus='3', facturada='1', factura_generica = '1', foliosat='{0}', folio_factura='{1}' WHERE idVenta = '{2}';", foliosat, folio, idVenta), conn)
                    'mysql_execute(String.Format("UPDATE foliossat SET foliosat = foliosat + 1 WHERE idTienda = '{0}';", tiendaActual.ToString), conn)
                    notas += foliosat.ToString + ","
                End If
                mysql_execute(String.Format("UPDATE ventas SET verificada = '1' WHERE idVenta = '{0}';", idVenta), conn)
            Next
            '
            mysql_execute(String.Format("UPDATE foliossat SET foliosat = '{0}'  WHERE idTienda = '{1}';", foliosat, tiendaActual.ToString), conn)
            '
            pgf.update_status(90, "Actualizando folios")
            notas = notas.Remove(notas.Length - 1)
            mysql_execute(String.Format("UPDATE facturas SET notas = '{0}' WHERE idFactura = {1}", notas, idFactura), conn)
            'Limpiamos el grid
            gridFacturaGenerica.DataSource = Nothing
            mysql_execute(String.Format("UPDATE foliossat SET folio_factura = folio_factura + 1, folio ='{0}' WHERE idTienda = '{1}';", foliosat, tiendaActual), conn)
            mysql_execute(String.Format("UPDATE tiendas SET factura_generica_generada = 1 WHERE idTienda = '{0}';", tiendaActual), conn)
            mysql_execute("COMMIT;", conn)
            ' Imprimir factura
            pgf.update_status(95, "Subiendo factura al PAC e imprimiendo el comprobante")
            generarFactura(idFactura.ToString, 1, True)
        Catch ex As Exception
            mysql_execute("ROLLBACK;", conn)
            MsgBox(ex.Message)
        End Try
        btnFacturaGenerica.Enabled = True
        pgf.Close()
    End Sub
End Class