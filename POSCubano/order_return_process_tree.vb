Imports MySql.Data.MySqlClient

Public Class order_return_process_tree
    Dim tienda_selected As String
    Dim tienda_selectedn As String

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles btnRefresh.Click
        If rol = 1 Then
            btnAdd.Visible = True
            tiendas.Enabled = False
            btnExportPdf.Visible = True
            tiendas.SelectedIndex = idTienda - 1
            'GridControl1.Location = New System.Drawing.Point(10, 9)
            'GridControl1.Size = New System.Drawing.Point(730, 345)
            fillDataGridByMySqlrDX(GridControl2, "SELECT '' AS ID, '' AS Tienda, '' AS 'Codigo bodega','' AS 'Codigo tienda','' AS Producto,'' AS 'Fecha','' AS 'Cantidad','' AS 'Pedido' FROM tienda_pedido a INNER JOIN tiendas b ON (idTienda = tienda_id) INNER JOIN producto c ON (c.id = producto_id) WHERE a.activo = 1 AND tienda_id = '" + idTienda + "'")
            If Me.Text = "Pedidos a bodega" Then
                fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, b.tienda AS Tienda, producto_id AS 'Codigo bodega',a.productoTienda_idproductoTienda AS 'Codigo tienda',c.nombre AS Producto, fecha_pedido AS 'Fecha',cantidad AS 'Cantidad',listaPedidos_id AS 'Pedido',comentario AS Comentario FROM tienda_pedido a INNER JOIN tiendas b ON (idTienda = tienda_id) INNER JOIN producto c ON (c.id = producto_id) WHERE a.activo = 1 AND tienda_id = '" + idTienda + "' ORDER BY c.nombre")
            Else
                fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, b.tienda AS Tienda, producto_id AS 'Codigo bodega',a.productoTienda_idproductoTienda AS 'Codigo tienda',c.nombre AS Producto, fecha_devolucion AS 'Fecha',cantidad AS 'Cantidad',listaDevoluciones_id AS 'Devolucion',comentario AS Comentario FROM tienda_devolucion a INNER JOIN tiendas b ON (idTienda = tienda_id) INNER JOIN producto c ON (c.id = producto_id) WHERE a.activo = 1 AND tienda_id = '" + idTienda + "' ORDER BY c.nombre")
            End If
        ElseIf rol = 2 Then
            tiendas.Visible = True
            Label1.Visible = True
            btnProcess.Visible = True
            If Me.Text = "Pedidos a bodega" Then
                fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID,a.fecha_pedido AS 'Fecha',a.producto_id AS 'Codigo bodega',a.productoTienda_idproductoTienda AS 'Codigo tienda',c.nombre AS Producto,a.cantidad AS 'Cantidad',comentario AS Comentario FROM tienda_pedido a INNER JOIN tiendas b ON (idTienda = tienda_id) INNER JOIN producto c ON (c.id = a.producto_id) WHERE a.activo = 1 AND a.tienda_id = '" + Str(tiendas.SelectedIndex + 1) + "' ORDER BY c.nombre")
            Else
                fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, a.fecha_devolucion AS 'Fecha',a.producto_id AS 'Codigo bodega',a.productoTienda_idproductoTienda AS 'Codigo tienda',c.nombre AS Producto, a.cantidad AS 'Cantidad',comentario AS Comentario  FROM tienda_devolucion a INNER JOIN tiendas b ON (idTienda = tienda_id) INNER JOIN producto c ON (c.id = producto_id) WHERE a.activo = 1 AND a.tienda_id = '" + Str(tiendas.SelectedIndex + 1) + "' ORDER BY c.nombre")
            End If
        End If
        GridView1.OptionsView.ShowFooter = True
        If GridView1.Columns("Cantidad").Summary.ActiveCount = 0 Then
            GridView1.Columns("Cantidad").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Cantidad", "{0:#,##0.00}")
        End If
        GridView1.Columns("Fecha").DisplayFormat.FormatString = "YYYY-MM-DD HH:mm:ss"
        GridView1.Columns("ID").Visible = False

        GridView1.BestFitColumns()
        GridView2.BestFitColumns()

        tienda_selected = tiendas.SelectedValue.ToString
        tienda_selectedn = tiendas.Text.ToString
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
        Button3_Click(sender, e)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        If Me.Text = "Pedidos a bodega" Then
            order_return_line_form.Text = "Crear línea de Pedido"
        Else
            order_return_line_form.Text = "Crear línea de Devolución"
        End If
        order_return_line_form.ShowDialog()
        Button3_Click(sender, e)
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnExportPdf.Click
        Dim NombreArchivo As String = System.IO.Path.GetTempFileName + ".pdf"
        GridView1.OptionsPrint.PrintHeader = True
        GridView1.OptionsPrint.PrintFooter = True
        Me.GridControl1.ExportToPdf(NombreArchivo)
        System.Diagnostics.Process.Start(NombreArchivo)
    End Sub

    Private Sub btnProcess_Click(sender As System.Object, e As System.EventArgs) Handles btnProcess.Click
        Dim conn As New MySqlConnection
        Dim id As String = ""
        Try
            If GridView1.SelectedRowsCount > 0 Then
                fillDataGridByMySqlrDX(GridControl2, "SELECT a.producto_id AS 'Codigo bodega',a.productoTienda_idproductoTienda AS 'Codigo tienda','' AS Producto, a.fecha_pedido AS 'Fecha',a.cantidad AS 'Cantidad',comentario AS Comentario FROM tienda_pedido a WHERE a.activo = 1234")

                conn = mysql_conexion_up()
                mysql_execute("START TRANSACTION", conn)
                If Me.Text = "Pedidos a bodega" Then
                    id = mysql_execute("INSERT INTO lista_pedidos (fecha,activo) VALUES (NOW(),0); SELECT LAST_INSERT_ID() AS id;", conn)(0)("id")
                Else
                    id = mysql_execute("INSERT INTO lista_devoluciones (fecha,activo) VALUES (NOW(),0); SELECT LAST_INSERT_ID() AS id;", conn)(0)("id")
                End If

                Dim i As Integer = 0

                If Me.Text = "Pedidos a bodega" Then
                    For a As Integer = 0 To GridView1.RowCount - 1
                        GridView2.AddNewRow()
                        'GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "ID", GridView1.GetRowCellValue(row_id, "ID").ToString())
                        'GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Tienda", GridView1.GetRowCellValue(row_id, "Tienda").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Codigo bodega", GridView1.GetRowCellValue(a, "Codigo bodega").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Fecha", GridView1.GetRowCellValue(a, "Fecha").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Codigo tienda", GridView1.GetRowCellValue(a, "Codigo tienda").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Producto", GridView1.GetRowCellValue(a, "Producto").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Cantidad", GridView1.GetRowCellValue(a, "Cantidad").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Comentario", GridView1.GetRowCellValue(a, "Comentario").ToString())
                        mysql_execute("UPDATE tienda_pedido SET activo = 0, listaPedidos_id = '" + id.ToString + "' WHERE id = '" + GridView1.GetRowCellValue(a, "ID").ToString() + "'", conn)
                    Next a

                Else

                    For Each row_id In GridView1.GetSelectedRows
                        GridView2.AddNewRow()
                        'GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "ID", GridView1.GetRowCellValue(row_id, "ID").ToString())
                        'GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Tienda", GridView1.GetRowCellValue(row_id, "Tienda").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Codigo bodega", GridView1.GetRowCellValue(row_id, "Codigo bodega").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Fecha", GridView1.GetRowCellValue(row_id, "Fecha").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Codigo tienda", GridView1.GetRowCellValue(row_id, "Codigo tienda").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Producto", GridView1.GetRowCellValue(row_id, "Producto").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Cantidad", GridView1.GetRowCellValue(row_id, "Cantidad").ToString())
                        GridView2.SetRowCellValue(GridView2.FocusedRowHandle, "Comentario", GridView1.GetRowCellValue(row_id, "Comentario").ToString())
                        'If Me.Text = "Pedidos a bodega" Then
                        'mysql_execute("UPDATE tienda_pedido SET activo = 0, listaPedidos_id = '" + id.ToString + "' WHERE id = '" + GridView1.GetRowCellValue(row_id, "ID").ToString() + "'", conn)
                        'Else
                        mysql_execute("UPDATE tienda_devolucion SET activo = 0, listaDevoluciones_id = '" + id.ToString + "' WHERE id = '" + GridView1.GetRowCellValue(row_id, "ID").ToString() + "'", conn)
                        'End If
                        'i += 1
                    Next
                End If
                GridView2.AddNewRow()
                GridView2.BestFitColumns()

                mysql_execute("COMMIT", conn)
                'Dim idTienda = executeQueryTextr(String.Format("SELECT idTienda FROM tiendas WHERE tienda = {0};", tienda_selected))
                If Me.Text = "Pedidos a bodega" Then
                    hojaSalidaBodega(GridView1, "Pedido de bodega procesado", tienda_selected, id.ToString(), DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                    'export2PDF(GridControl2, "Pedido de bodega procesado", "Tienda " + tienda_selected + ", " + tienda_selectedn + " | Folio: " + id.ToString + " | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                Else
                    export2PDF(GridControl2, "Devolución de bodega procesada", "Tienda " + tienda_selected + ", " + tienda_selectedn + " | Folio: " + id.ToString + " | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                End If
                Button3_Click(sender, e)
            End If

        Catch ex As Exception
            mysql_execute("ROLLBACK", conn)
            MsgBox("Error al procesar: " + ex.ToString)
        End Try
        conn.Close()
    End Sub

   

End Class