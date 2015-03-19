Imports MySql.Data.MySqlClient

Public Class order_return_line_form

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        ' Buscamos si el producto en la tienda existe
        Dim producto_bodega_id = executeQueryTextr("SELECT nombre FROM producto_tienda WHERE idproductoTienda = '" + TextBox1.Text + "'")
        If producto_bodega_id = "" Then
            MsgBox("El código de producto no es válido: " + TextBox1.Text)
            Return
        End If
        ' Validamos que los datos introducidos sean números
        For i As Integer = 0 To GridView1.DataRowCount - 1
            If Not IsNumeric(GridView1.GetRowCellValue(i, "Cantidad").ToString()) And GridView1.GetRowCellValue(i, "Cantidad").ToString() <> "" Then
                MsgBox("Las cantidades a pedir deben ser númericas por favor revise los datos introducidos")
                Return
            End If
            If GridView1.GetRowCellValue(i, "Cantidad").ToString() <> "" Then
                If Me.Text = "Crear línea de Pedido" Then
                    If Integer.Parse(GridView1.GetRowCellValue(i, "Cantidad").ToString()) > Integer.Parse(GridView1.GetRowCellValue(i, "Stock bodega").ToString()) Then
                        MsgBox("Las cantidades deben ser menores al stock del producto")
                        Return
                    End If
                End If
            End If

        Next i

        Dim theconn As New MySqlConnection
        theconn = mysql_conexion_up()
        mysql_execute("START TRANSACTION", theconn)

        For i As Integer = 0 To GridView1.DataRowCount - 1
            Dim main_code As String = GridView1.GetRowCellValue(i, "Codigo").ToString()
            Dim qty As String = GridView1.GetRowCellValue(i, "Cantidad").ToString()
            If qty <> "" Then
                If Me.Text = "Crear línea de Pedido" Then
                    ' Se inserta el pedido en la nube
                    mysql_execute(String.Format("INSERT INTO tienda_pedido (id,cantidad,fecha_pedido,activo,comentario,producto_id,tienda_id,productoTienda_idproductoTienda,usuario) VALUES ('','{0}',NOW(),1,'{1}','{2}','{3}','{4}','{5}')", qty, TextBox3.Text, main_code, idTienda, TextBox1.Text, idTienda), theconn)
                    ' Se descuentan los pedidos de bodega  de la nube
                    mysql_execute("UPDATE producto SET inventarios_bodega = inventarios_bodega - " + qty + " WHERE id = '" + main_code + "'", theconn)
                    ' Se incrementa el producto en la tienda de la nube
                    mysql_execute("UPDATE producto_tienda SET tienda" + idTienda + " = tienda" + idTienda + " + " + qty + " WHERE idproductoTienda = '" + TextBox1.Text + "'", theconn)
                Else
                    ' Se inserta la devolución en la nube
                    mysql_execute(String.Format("INSERT INTO tienda_devolucion (id,cantidad,fecha_devolucion,activo,comentario,producto_id,tienda_id,productoTienda_idproductoTienda,usuario) VALUES ('','{0}',NOW(),1,'{1}','{2}','{3}','{4}','{5}')", qty, TextBox3.Text, main_code, idTienda, TextBox1.Text, idTienda), theconn)
                    ' Se suma los pedidos de bodega de la nube
                    mysql_execute("UPDATE producto SET inventarios_bodega = inventarios_bodega + " + qty + " WHERE id = '" + main_code + "'", theconn)
                    ' Se resta el producto en la tienda de la nube
                    mysql_execute("UPDATE producto_tienda SET tienda" + idTienda + " = tienda" + idTienda + " - " + qty + " WHERE idproductoTienda = '" + TextBox1.Text + "'", theconn)
                End If
            End If
        Next i

        ' Finaliza el commit
        mysql_execute("COMMIT", theconn)
        theconn.Close()
        Me.Close()
    End Sub

    Private Sub order_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
        TextBox3.Text = ""
        GridControl1.DataSource = Nothing
        GridView1.RefreshData()
    End Sub

    Private Sub GridView1_ShowingEditor(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
        If GridView1.FocusedColumn.FieldName = "Codigo" Or GridView1.FocusedColumn.FieldName = "Stock bodega" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            btnSearch_Click(sender, e)
        End If
    End Sub

    Private Sub btnSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnSearch.Click
        If Me.Text = "Crear línea de Pedido" Then
            fillDataGridByMySqlrDX(GridControl1, "SELECT id AS 'Codigo', inventarios_bodega AS 'Stock bodega', '' AS Cantidad FROM producto WHERE inventarios_bodega > 0 AND productoTienda_idproductoTienda = '" + TextBox1.Text + "'")
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT id AS 'Codigo', inventarios_bodega AS 'Stock bodega', '' AS Cantidad FROM producto WHERE productoTienda_idproductoTienda = '" + TextBox1.Text + "'")
        End If

        GridView1.BestFitColumns()
        If GridView1.Columns("Cantidad").Summary.ActiveCount = 0 Then
            GridView1.Columns("Cantidad").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Cantidad", "{0:#,##0}")
        End If
        GridView1.Columns("Cantidad").DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
        GridView1.Focus()
    End Sub
End Class