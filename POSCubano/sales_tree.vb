Public Class sales_tree

    Private Sub ventas_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UpdateGrid()
    End Sub
    'imprimir
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridVentas.SelectedRows()

        For Each row As DataGridViewRow In nots
            ' Es factura
            If Me.gridVentas(4, row.Index).Value = "Facturado" Then
                ' Debemos de buscar el ID de la factura
                Dim id As Integer = executeQueryr("SELECT idFactura FROM facturas WHERE folio = " + Me.gridVentas(5, row.Index).Value.ToString + " AND idtienda = " + idTienda.ToString)
                Dim folio_fiscal As String = executeQueryTextr("SELECT folio_fiscal FROM facturas WHERE folio = " + Me.gridVentas(5, row.Index).Value.ToString + " AND idtienda = " + idTienda.ToString)
                If Len(folio_fiscal) < 1 Then
                    MsgBox("Al imprimir una factura que aún no ha sido firmada por el PAC se intentará firmar, este proceso puede demorar un poco por favor sea paciente", MsgBoxStyle.Information)
                End If
                generarFactura(id)
            ElseIf Me.gridVentas(4, row.Index).Value = "Cancelado" Then
                MsgBox("Ha seleccionado un folio cancelado")
                Exit Sub
            Else
                ' Es nota
                generarNota(row.Cells(0).Value.ToString)
            End If
        Next
        UpdateGrid()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult

        Dim nots As DataGridViewSelectedRowCollection = Me.gridVentas.SelectedRows()
        Dim seemOK As Boolean = True
        For Each row As DataGridViewRow In nots
            If Me.gridVentas(4, row.Index).Value = "Facturado" Then
                MsgBox("No puede cancelar una venta facturada.")
                seemOK = False
            End If
        Next
        If seemOK Then
            style = MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information Or MsgBoxStyle.YesNo
            ' Display message.
            response = MsgBox("Esta acción cancelará las ventas seleccionadas. Presione Si para confirmar.", style, "Cancelar ventas")
            If response = MsgBoxResult.Yes Then   ' User chose Yes.
                For Each row As DataGridViewRow In gridVentas.Rows
                    If row.Selected Then
                        executeQueryr(String.Format("UPDATE ventas SET idEstatus = '2' WHERE idVenta = '{0}';", row.Cells(0).Value))
                        UpdateGrid()
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        UpdateGrid()
    End Sub


    Public Function UpdateGrid()
        ' Tomamos la fecha de la ultima apertura
        Dim last_opening As String = executeQueryTextr("SELECT DATE_FORMAT(fecha_apertura, '%Y-%m-%d %H:%i:%s') FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "'")
        ' Llenamos el GRID
        fillDataGridByMySqlr(gridVentas, String.Format("SELECT a.idventa AS ID, a.fecha AS Fecha, a.monto AS Total, a.folio AS 'Folio de venta', b.estatus AS Estado, a.folio_factura AS 'Folio factura', c.folio_fiscal AS 'Folio Fiscal' FROM ventas a INNER JOIN estatus b ON (a.idEstatus = b.idEstatus) LEFT JOIN facturas c ON (c.folio = a.folio_factura AND c.idTienda = '" + idTienda + "') WHERE a.idTienda = " + idTienda + " AND a.fecha > '" + last_opening + "'"))
        ' IVAS
        TextBox7.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(monto) FROM extra_iva WHERE tienda_id = " + idTienda + " AND fecha > '" + last_opening + "'"))
        ' Venta EFE
        TextBox1.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(A.monto) FROM ventas A INNER JOIN estatus B ON (A.idEstatus = B.idEstatus) WHERE (A.idEstatus != '2' AND metodo = 1) AND (A.idTienda = " + idTienda + " AND A.fecha > '" + last_opening + "')"))
        ' Venta DEP
        TextBox4.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(A.monto) FROM ventas A INNER JOIN estatus B ON (A.idEstatus = B.idEstatus) WHERE (A.idEstatus != '2' AND metodo != 1) AND (A.idTienda = " + idTienda + " AND fecha > '" + last_opening + "')"))
        ' Apertura
        TextBox2.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(apertura) FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "' ORDER BY fecha_apertura DESC"))
        ' Gastos
        TextBox5.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(monto) FROM gastos WHERE idTienda = " + idTienda + " AND fecha > '" + last_opening + "'"))
        ' Total
        TextBox3.Text = String.Format("{0:n2}", CDbl(TextBox1.Text) + CDbl(TextBox2.Text) + CDbl(TextBox4.Text) + CDbl(TextBox7.Text))
        ' Efectivo
        TextBox6.Text = String.Format("{0:n2}", CDbl(TextBox1.Text) + CDbl(TextBox2.Text) + CDbl(TextBox7.Text) - CDbl(TextBox5.Text))
        gridVentas.AutoResizeColumns()
        UpdateGrid = 1
    End Function

    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        UpdateGrid()
    End Sub

    Private Sub gridVentas_CellDoubleClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridVentas.CellDoubleClick
        Button2_Click(sender, e)
        UpdateGrid()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim nots As DataGridViewSelectedRowCollection = Me.gridVentas.SelectedRows()
        For Each row As DataGridViewRow In nots
            If Me.gridVentas(4, row.Index).Value = "Facturado" Then
                Dim mailf As mail_form = New mail_form
                mailf.folio = gridVentas.Item(5, gridVentas.CurrentRow.Index).Value.ToString
                mailf.ShowDialog()
            Else
                MsgBox("Solo se pueden enviar por correo facturas no las notas de venta", MsgBoxStyle.Information)
            End If
        Next
    End Sub


End Class