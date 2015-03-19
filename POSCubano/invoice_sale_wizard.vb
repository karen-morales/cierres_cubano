Imports System.Text.RegularExpressions
Imports tagcode.xml.CFDI
Imports tagcode.xml
Imports tagcode.xml.CFDI.Emisor
Imports tagcode.xml.CFDI.Receptor
Imports MySql.Data.MySqlClient

Public Class invoice_sale_wizard
    Private closeok As Boolean = False

    Private Sub cmbClientes_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbClientes.KeyUp
        'Cuando se carga el form
        If e.KeyCode = 13 Then
            fillComboByMySqlr(cmbClientes, String.Format("SELECT idCliente,CONCAT(nombre,' ',rfc) FROM partners WHERE nombre LIKE '%{0}%' OR rfc LIKE '%{0}%';", cmbClientes.Text))
        End If
    End Sub

    Private Sub factura_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not closeok Then
            e.Cancel = True
            Return
        End If
    End Sub

    Private Sub factura_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim nots As DataGridViewSelectedRowCollection = facturacion.gridFacturacion.SelectedRows()
        'Dim notass As String = ""
        'Seteamos la cadena que informa de las notas seleccionadas a facturar
        'For Each row As DataGridViewRow In nots
        'notass += facturacion.gridFacturacion(0, row.Index).Value.ToString & ","
        'Next
        'notass = notass.Remove(notass.Length - 1)
        'notas.Text = notass
        ' Cargamos el grid de metodo de pago
        fillComboByMySqlr(cmbPaymentMethod, "SELECT id,description FROM metodos_de_pago;")
        'Dim nots As DataGridViewSelectedRowCollection = ventas.gridVentas.SelectedRows()
        'For Each row As DataGridViewRow In nots
        'Dim idPago As Integer = executeQueryr(String.Format("SELECT id FROM metodos_de_pago WHERE description LIKE '{0}';", ventas.gridVentas(5, row.Index).Value.ToString))
        Dim idpago As Integer = pos_form.cmbPaymentMethod.SelectedValue
        cmbPaymentMethod.SelectedValue = idpago
        If idpago = 1 Or idpago = 6 Then
            TextBox1.Enabled = False
            Label5.Enabled = False
        Else
            TextBox1.Enabled = True
            Label5.Enabled = True
        End If
        closeok = False
        ComboBox1.SelectedValue = 0
        cmbClientes.SelectedValue = 0
        ComboBox1.SelectedValue = 0
        cmbPaymentMethod.SelectedValue = 0
        TextBox1.Text = ""

        fillComboByMySqlr(ComboBox1, "SELECT id,nombre FROM vendedores;")
        Try
            ComboBox1.SelectedValue = pos_form.vendedorCmb.SelectedValue
        Catch ex As Exception
            ComboBox1.SelectedValue = 0
        End Try
        'Exit For
        'Next
    End Sub
    'Facturar
    Private Sub btnFacturar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFacturar.Click
        Dim theconn As New MySqlConnection
        If cmbClientes.SelectedValue Then
            GroupBox1.Enabled = False
            Label3.Text = "Por favor sea paciente se está generando y enviando la factura al PAC"
            My.Application.DoEvents()
            Try
                Dim folio As Integer = getLastFolioFacturaR(idTienda) + 1
                Dim monto As Double
                Dim num_cuenta As String = String.Format("'{0}'", TextBox1.Text)
                Dim idCliente As String = cmbClientes.SelectedValue.ToString
                Dim idFactura As Integer
                Dim ds As DataSet = New DataSet

                theconn = mysql_conexion_up()
                mysql_execute("START TRANSACTION", theconn)

                If facturaOtros Then
                    monto = invoice_ticket_wizard.Label4.Text
                    'Insertamos la factura
                    idFactura = mysql_execute(String.Format("INSERT INTO facturas (idCliente,fecha,idEstatus,folio,idTienda,monto,metodo,cuenta,vendedor) VALUES ({0},NOW(),'1',{1},{2},round({3},2),{4},{5},{6}); SELECT LAST_INSERT_ID() AS id;", idCliente, folio, idTienda, monto * 1.16, cmbPaymentMethod.SelectedIndex + 1, num_cuenta, ComboBox1.SelectedValue), theconn)(0).Item("id")

                    For i = 0 To invoice_ticket_wizard.DataGridView1.Rows.Count - 1
                        'foliosNotas += GenerarFacturaOtros.DataGridView1(0, i).Value.ToString + ","
                        'executeQueryr(String.Format("INSERT INTO facturas_detalle (id_factura,descripcion,cantidad,precio,fecha) VALUES ({0},'{1}',{2},{3},now()); SELECT LAST_INSERT_ID();", idFactura, Pos.DataGridView2(4, i).Value, Pos.DataGridView2(0, i).Value, Pos.DataGridView2(2, i).Value))
                        ' foliosNotas = foliosNotas.Substring(0, foliosNotas.Length - 1)
                        Dim fecha As Date = invoice_ticket_wizard.DataGridView1(2, i).Value
                        Dim fechaFormateada As String = fecha.ToString("yyyy-MM-dd")
                        ds = fillDataSetByMySqlr(String.Format("SELECT * FROM ventas_detalle WHERE idventa = '{0}' AND date(fecha) = '{1}' AND idTienda = '{2}';", invoice_ticket_wizard.DataGridView1(0, i).Value, fechaFormateada, idTienda))
                        'TOTALES                      
                        For Each row As DataRow In ds.Tables(0).Rows
                            mysql_execute(String.Format("INSERT INTO facturas_detalle (folio_factura,descripcion,cantidad,precio,fecha,idtienda) VALUES ({0},'{1}',{2},round({3},2),now(),'{4}'); SELECT LAST_INSERT_ID();", folio, row("idproductoTienda"), row("cantidad"), row("precio"), idTienda), theconn)
                        Next
                        'Se guarda el iva
                        mysql_execute(String.Format("INSERT INTO extra_iva (monto,tienda_id,fecha) VALUES (round({0},2),'{1}',now());", invoice_ticket_wizard.DataGridView1(3, i).Value * 0.16, idTienda), theconn)
                        ' Se marca esa venta como facturada
                        mysql_execute(String.Format("UPDATE ventas SET idEstatus='3',facturada = '1',folio_factura = '{0}',folio=NULL WHERE idVenta = '{1}';", folio, currentSale), theconn)
                    Next
                Else
                    'Obtenemos el monto total
                    monto = mysql_execute(String.Format("SELECT monto FROM ventas WHERE idVenta = {0};", currentSale), theconn)(0).Item("monto")
                    'Insertamos la factura
                    idFactura = mysql_execute(String.Format("INSERT INTO facturas (idCliente,fecha,idEstatus,folio,idTienda,monto,metodo,cuenta,vendedor) VALUES ({0},NOW(),'1',{1},{2},round({3},2),{4},{5},{6}); SELECT LAST_INSERT_ID() AS id;", idCliente, folio, idTienda, monto, cmbPaymentMethod.SelectedIndex + 1, num_cuenta, ComboBox1.SelectedValue), theconn)(0).Item("id")

                    ds = fillDataSetByMySqlr(String.Format("SELECT * FROM ventas_detalle WHERE idVenta = '{0}';", currentSale))
                    For Each row As DataRow In ds.Tables(0).Rows
                        mysql_execute(String.Format("INSERT INTO facturas_detalle (folio_factura,descripcion,cantidad,precio,fecha,idtienda) VALUES ({0},'{1}',{2},round({3},2),now(),'{4}'); SELECT LAST_INSERT_ID();", folio, row("idproductoTienda"), row("cantidad"), row("precio") / 1.16, idTienda), theconn)
                    Next
                    mysql_execute(String.Format("UPDATE ventas SET idEstatus='3',facturada = '1',folio_factura = '{0}',folio=NULL WHERE idVenta = '{1}';", folio, currentSale), theconn)
                    'Next
                End If

                ' Se actualiza el folio de la factura
                mysql_execute(String.Format("UPDATE foliossat SET folio_factura = '{0}' WHERE idTienda = '{1}';", folio, idTienda), theconn)

                mysql_execute("COMMIT", theconn)

                generarFactura(idFactura.ToString)
                'Cargamos el grid de ventas
                sales_tree.UpdateGrid()
                closeok = True
                Me.Close()
            Catch ex As Exception
                mysql_execute("ROLLBACK", theconn)
                MsgBox(ex.Message)
                GroupBox1.Enabled = True
                facturaOtros = False
                Label3.Text = "Escriba el nombre del cliente y presione Enter para buscarlo"
                closeok = True
                invoice_ok = False
                Me.Close()
                'MsgBox("Seleccione un cliente por favor.")
            End Try
            GroupBox1.Enabled = True
            theconn.Close()
        Else
            MsgBox("Seleccione un cliente para poder facturar")
        End If
        facturaOtros = False
        Label3.Text = "Escriba el nombre del cliente y presione Enter para buscarlo"
    End Sub

    Private Sub cmbPaymentMethod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbPaymentMethod.SelectedIndexChanged
        'Console.WriteLine(cmbPaymentMethod.SelectedIndex)
        'Dim resultIndex As Integer = cmbPaymentMethod.FindStringExact(cmbPaymentMethod.SelectedValue)
        If cmbPaymentMethod.SelectedIndex = 0 Or cmbPaymentMethod.SelectedIndex = 5 Then
            TextBox1.Enabled = False
            Label5.Enabled = False
        Else
            TextBox1.Enabled = True
            Label5.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        partner_form.ShowDialog()
    End Sub

    Private Sub cmbClientes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbClientes.SelectedIndexChanged

    End Sub
End Class