Imports MySql.Data.MySqlClient
Imports System.Drawing
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraGrid.Columns

Public Class inventory_shop_tree
    Dim tienda_selected As String
    Dim tienda_selectedn As String

    Private Sub inventory_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If rol <> 3 Then
            fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas WHERE idTienda = " + idTienda + ";")
            Button2.Hide()
        Else
            fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
            Button2.Show()
        End If
        Button4_Click(sender, e)
    End Sub

    Public Function UpdateGrid()
        fillDataGridByMySqlrDX(GridControl1, "SELECT idproductoTienda AS Codigo,producto_tienda.nombre AS Nombre,categorias.nombre AS Categoria,costo AS Costo,costo*tienda" + tiendas.SelectedValue.ToString + " AS 'Costo Total',  precio AS Precio,'' AS 'Nuevo',tienda" + tiendas.SelectedValue.ToString + " AS 'Stock','' AS Inventario,'' AS Diferencia, precio*tienda" + tiendas.SelectedValue.ToString + " AS 'Valor' FROM producto_tienda LEFT JOIN categorias ON (categorias.id = producto_tienda.categorias_id)")
        GridView1.OptionsView.ShowFooter = True

        If GridView1.Columns("Codigo").Summary.ActiveCount = 0 Then
            GridView1.Columns("Valor").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Valor", "${0:#,##0.00}")
            GridView1.Columns("Costo Total").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Valor", "${0:#,##0.00}")
            GridView1.Columns("Codigo").Summary.Add(DevExpress.Data.SummaryItemType.Count, "Codigo", "Qty: {0:#,##0}")
        End If

        GridView1.Columns("Valor").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        GridView1.Columns("Valor").DisplayFormat.FormatString = "#,##0.00"
        GridView1.Columns("Stock").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        GridView1.Columns("Stock").DisplayFormat.FormatString = "#,##0"

        GridView1.OptionsBehavior.CopyToClipboardWithColumnHeaders = True
        GridView1.OptionsFind.AlwaysVisible = True

        If rol <> 3 Then
            GridView1.Columns("Nuevo").Visible = False
            GridView1.Columns("Inventario").Visible = False
            GridView1.Columns("Valor").Visible = False
            GridView1.Columns("Diferencia").Visible = False
            GridView1.OptionsView.ShowFooter = False
        End If

        GridView1.BestFitColumns()
        Return True
    End Function

    Private Sub GridView1_RowUpdated(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowObjectEventArgs) Handles GridView1.RowUpdated
        Try
            e.Row("Diferencia") = e.Row("Inventario") - e.Row("Stock")
        Catch ex As Exception
            e.Row("Diferencia") = ""
            e.Row("Inventario") = ""
        End Try
    End Sub

    Private Sub GridView1_ShowingEditor(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
        If rol <> 3 Then
            e.Cancel = True
        End If
        If GridView1.FocusedColumn.FieldName <> "Inventario" And GridView1.FocusedColumn.FieldName <> "Nuevo" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        UpdateGrid()
        tienda_selected = tiendas.SelectedValue.ToString
        tienda_selectedn = tiendas.Text.ToString
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim NombreArchivo As String = System.IO.Path.GetTempFileName + ".xls"
        GridView1.OptionsPrint.AutoWidth = True
        GridControl1.ExportToXls(NombreArchivo)
        System.Diagnostics.Process.Start(NombreArchivo)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        ' Aplicamos el filtro para solo exportar los productos con modificaciones
        'GridView1.ActiveFilterString = "[Diferencia] > 0"
        ' Muestra el PDF con los cambios a realizar
        export2PDF(GridControl1, "Actualización de precio / inventario [borrador]", tienda_selectedn + " | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
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
                    Dim nprecio As String = GridView1.GetRowCellValue(i, "Nuevo").ToString()
                    If qty <> "" Then
                        mysql_execute(String.Format("INSERT INTO listaMovimientosTiendas (productoTiendaId,cantidad,fecha,tienda,usuario) VALUES('{0}','{1}',NOW(),'{2}','{3}')", code, qty, tienda_selected, idUsuario), theconn)
                        mysql_execute("UPDATE producto_tienda SET tienda" + tienda_selected + " = " + qty + " WHERE idproductoTienda = '" + code + "'", theconn)
                    End If
                    If nprecio <> "" Then
                        mysql_execute("UPDATE producto_tienda SET precio = " + nprecio + " WHERE idproductoTienda = '" + code + "'", theconn)
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
            export2PDF(GridControl1, "Actualización de precio / inventario", tienda_selectedn + " | " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
            UpdateGrid()
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridControl1.DoubleClick
        'If GridView1.GetSelectedRows.Length > 0 Then
        'inventory_shop_form.ShowDialog()
        'End If
    End Sub

    Private Sub GridControl1_Click(sender As System.Object, e As System.EventArgs) Handles GridControl1.Click

    End Sub
End Class
