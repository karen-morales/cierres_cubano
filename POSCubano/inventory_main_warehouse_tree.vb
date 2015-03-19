Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Grid
Imports MySql.Data.MySqlClient

Public Class inventory_main_warehouse_tree
    Private Sub inventory_main_warehouse_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        refresh_Click(sender, e)
        If rol = 3 Then
            updateStock.Visible = True
            aplicar.Visible = True
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnExportXLS.Click
        Dim NombreArchivo As String = System.IO.Path.GetTempFileName + ".xls"
        GridView1.OptionsPrint.AutoWidth = True
        GridControl1.ExportToXls(NombreArchivo)
        System.Diagnostics.Process.Start(NombreArchivo)
    End Sub

    Private Sub refresh_Click(sender As System.Object, e As System.EventArgs) Handles refresh.Click
        If rol = 3 Then
            fillDataGridByMySqlrDX(GridControl1, "SELECT producto.id AS Codigo, producto.nombre AS Nombre,categorias.nombre AS Categoria,color.nombre AS Color, producto.inventarios_bodega AS Existencia, '' AS Inventario,'' AS Diferencia,producto_tienda.precio AS Precio, inventarios_bodega*producto_tienda.precio AS Valor, productoTienda_idproductoTienda AS 'Codigo tienda' FROM producto LEFT JOIN categorias ON (producto.categorias_id = categorias.id) LEFT JOIN color ON (color.id = producto.color_id) INNER JOIN producto_tienda ON (producto.productoTienda_idproductoTienda=producto_tienda.idproductoTienda)")
            GridView1.OptionsView.ShowFooter = True

            If GridView1.Columns("Codigo").Summary.ActiveCount = 0 Then
                GridView1.Columns("Valor").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Valor", "${0:#,##0.00}")
                GridView1.Columns("Codigo").Summary.Add(DevExpress.Data.SummaryItemType.Count, "Codigo", "Qty: {0:#,##0}")
            End If
        ElseIf rol = 2 Then
            fillDataGridByMySqlrDX(GridControl1, "SELECT producto.id AS Codigo,producto.nombre AS Nombre,categorias.nombre AS Categoria,color.nombre AS Color,inventarios_bodega AS Existencia, productoTienda_idproductoTienda AS 'Codigo tienda' FROM producto LEFT JOIN categorias ON (producto.categorias_id = categorias.id) LEFT JOIN color ON (color.id = producto.color_id)")
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT producto.id AS Codigo,producto.nombre AS Nombre,categorias.nombre AS Categoria,color.nombre AS Color,inventarios_bodega AS Existencia,producto_tienda.precio AS Precio, productoTienda_idproductoTienda AS 'Codigo tienda' FROM producto LEFT JOIN categorias ON (producto.categorias_id = categorias.id) LEFT JOIN color ON (color.id = producto.color_id) INNER JOIN producto_tienda ON (producto.productoTienda_idproductoTienda=producto_tienda.idproductoTienda)")
        End If

        'GridView1.GroupFooterShowMode = GroupFooterShowMode.VisibleIfExpanded
        'GridView1.OptionsBehavior.Editable = False
        'GridView1.OptionsFind.AlwaysVisible = True
        'GridView1.OptionsView.ShowFooter = False
        'GridView1.OptionsView.ShowGroupPanel = True
        GridView1.BestFitColumns()

        
        If GridView1.Columns("Existencia").Summary.ActiveCount = 0 Then
            GridView1.Columns("Existencia").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Existencia", "{0:#,##0.00}")
        End If

    End Sub

    Private Sub GridView1_RowUpdated(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowObjectEventArgs) Handles GridView1.RowUpdated
        Try
            e.Row("Diferencia") = e.Row("Inventario") - e.Row("Existencia")
        Catch ex As Exception
            e.Row("Diferencia") = ""
            e.Row("Inventario") = ""
        End Try
    End Sub

    Private Sub GridView1_ShowingEditor(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
        If rol <> 3 Then
            e.Cancel = True
        End If
        If GridView1.FocusedColumn.FieldName <> "Inventario" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs)
        'If GridView1.GetSelectedRows.Length > 0 And rol = 3 Then
        'inventory_shop_form.ShowDialog()
        'End If
    End Sub

    Private Sub updateStock_Click(sender As System.Object, e As System.EventArgs) Handles updateStock.Click
        ' Muestra el PDF con los cambios a realizar
        export2PDF(GridControl1, "Actualización de inventario [borrador]", "Bodega | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
        GridView1.ActiveFilterString = ""
        If MsgBox("¿Esta seguro de aplicar el inventario, esta acción no se puede deshacer?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim theconn As New MySqlConnection
            theconn = mysql_conexion_up()
            mysql_execute("START TRANSACTION", theconn)
            Try
                ' Buscamos los cambios en el inventario
                For i As Integer = 0 To GridView1.DataRowCount - 1
                    Dim code As String = GridView1.GetRowCellValue(i, "Codigo").ToString()
                    Dim qty As String = GridView1.GetRowCellValue(i, "Inventario").ToString()
                    If qty <> "" Then
                        mysql_execute("UPDATE producto SET inventarios_bodega = " + qty + " WHERE id = '" + code + "'", theconn)
                        mysql_execute(String.Format("INSERT INTO listaMovimientosBodega (productoId,cantidad,fecha,usuario) VALUES('{0}','{1}',NOW(),'{2}')", code, qty, idUsuario), theconn)
                    End If
                Next i
                ' Finaliza el commit
                mysql_execute("COMMIT", theconn)
                'MsgBox("Proceso realizado satisfactoriamente")
            Catch ex As Exception
                mysql_execute("ROLLBACK", theconn)
                MsgBox(ex, MsgBoxStyle.Information)
            End Try
            theconn.Close()
            export2PDF(GridControl1, "Actualización de inventario", "Bodega | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
            refresh_Click(sender, e)
        End If
    End Sub

    Private Sub aplicar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles aplicar.Click
        GridView1.ActiveFilterString = ""
        Dim theconn As New MySqlConnection
        theconn = mysql_conexion_up()
        mysql_execute("START TRANSACTION", theconn)
        Dim porcen As Double = porcentaje.Text
        Try
            ' Buscamos los cambios en el inventario
            For i As Integer = 0 To GridView1.SelectedRowsCount - 1
                Dim code As String = GridView1.GetRowCellValue(i, "Codigo").ToString()
                Dim precio As String = GridView1.GetRowCellValue(i, "Precio").ToString()
                Dim porcentaje As Double = ((precio * porcen) / 100) + precio
                mysql_execute("UPDATE producto_tienda SET precio = " + precio + " WHERE idproductoTienda = '" + code + "'", theconn)
            Next i
            ' Finaliza el commit
            mysql_execute("COMMIT", theconn)
            MsgBox("Proceso realizado satisfactoriamente")
        Catch ex As Exception
            mysql_execute("ROLLBACK", theconn)
            MsgBox(ex, MsgBoxStyle.Information)
        End Try
            theconn.Close()
            refresh_Click(sender, e)
    End Sub
End Class