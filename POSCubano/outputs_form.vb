Imports MySql.Data.MySqlClient
Public Class outputs_form

    Private Sub outputs_form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Me.Text = "Detalle de salidas" Then
            TextBox1.Text = outputs_tree.GridView1.GetFocusedRowCellValue("ID").ToString()
            TextBox2.Text = outputs_tree.GridView1.GetFocusedRowCellValue("Fecha").ToString()
            TextBox3.Text = outputs_tree.GridView1.GetFocusedRowCellValue("Responsable").ToString()
            TextBox5.Text = outputs_tree.GridView1.GetFocusedRowCellValue("Total").ToString()
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.producto_id AS Codigo, b.nombre AS Producto, a.cantidad AS Cantidad, a.comentario AS Comentario FROM salidas_producto_has_producto a INNER JOIN producto b ON (a.producto_id = b.id) WHERE salidas_producto_idSalida = " + TextBox1.Text + " ORDER BY b.nombre")

            GridView1.OptionsBehavior.Editable = False
            btnOK.Text = "Aceptar"
            btnCancel.Visible = False
            Button1.Enabled = False
            Button2.Enabled = False
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT producto_id AS Codigo, '' AS Producto, cantidad AS Cantidad, comentario AS Comentario FROM salidas_producto_has_producto WHERE salidas_producto_idSalida = '000000'")
            GridView1.OptionsBehavior.Editable = True
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox5.Text = ""
            btnOK.Text = "Agregar"
            btnCancel.Visible = True
            Button1.Enabled = True
            Button2.Enabled = True
        End If

        If rol = 3 Then
            TextBox5.Show()
            Label5.Show()
        Else
            TextBox5.Show()
            Label5.Show()
        End If
        GridView1.BestFitColumns()

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If btnOK.Text = "Aceptar" Then
            Me.Close()
        ElseIf GridView1.DataRowCount > 0 Then
            Dim conn As New MySqlConnection
            Dim total As Double
            Dim productos As New List(Of String)

            conn = mysql_conexion_up()
            ' Validamos que los productos instroducidos existan
            For i As Integer = 0 To GridView1.DataRowCount - 1
                Dim x = mysql_execute("SELECT id FROM producto WHERE id = '" + GridView1.GetRowCellValue(i, "Codigo").ToString() + "'", conn)
                If x.Count = 0 Then
                    MsgBox("El codigo de producto [" + GridView1.GetRowCellValue(i, "Producto").ToString() + "] no es correcto, por favor verifique.")
                    conn.Close()
                    Return
                End If

                ' Validamos que exista cantidad y costo
                If IsDBNull(GridView1.GetRowCellValue(i, "Cantidad")) Then
                    MsgBox("La cantidad de productos a sacar debe ser numérica, por favor revise los datos introducidos")
                    conn.Close()
                    Return
                End If

                Dim ca = mysql_execute("SELECT inventarios_bodega FROM producto WHERE id = '" + GridView1.GetRowCellValue(i, "Codigo").ToString() + "'", conn)(0)("inventarios_bodega")
                If ca > 0 Then
                    Dim cant = CInt(GridView1.GetRowCellValue(i, "Cantidad"))
                    If ca >= cant Then
                        total += CInt(GridView1.GetRowCellValue(i, "Cantidad"))
                    Else
                        MsgBox("El producto [" + GridView1.GetRowCellValue(i, "Producto").ToString + "] sólo tiene en stock " + ca + ". Verifique la cantidad que solicita.")
                        conn.Close()
                        Return
                    End If
                Else
                    MsgBox("El producto [" + GridView1.GetRowCellValue(i, "Producto").ToString + "] tiene stock 0, no puede hacer salidas.")
                    conn.Close()
                    Return
                End If


                If productos.Contains(GridView1.GetRowCellValue(i, "Producto").ToString) Then
                    MsgBox("El producto [" + GridView1.GetRowCellValue(i, "Producto").ToString + "] está repetido por favor corrija.")
                    conn.Close()
                    Return
                Else
                    productos.Add(GridView1.GetRowCellValue(i, "Producto").ToString)
                End If
            Next i

            mysql_execute("START TRANSACTION", conn)
            ' Guardamos las líneas de la compra
            Dim id = mysql_execute("INSERT INTO salidas_producto (fecha_salida,total_productos,usuarios_idusuarios) VALUES(NOW(),'" + total.ToString + "','" + idUsuario.ToString + "'); SELECT LAST_INSERT_ID() AS id", conn)(0)("id")
            For i As Integer = 0 To GridView1.DataRowCount - 1
                mysql_execute("INSERT INTO salidas_producto_has_producto (salidas_producto_idSalida,producto_id,cantidad,comentario) VALUES('" + id.ToString + "','" + GridView1.GetRowCellValue(i, "Codigo").ToString() + "','" + GridView1.GetRowCellValue(i, "Cantidad").ToString() + "','" + GridView1.GetRowCellValue(i, "Comentario").ToString() + "')", conn)
                mysql_execute("UPDATE producto SET inventarios_bodega = inventarios_bodega - " + GridView1.GetRowCellValue(i, "Cantidad").ToString() + " WHERE id = '" + GridView1.GetRowCellValue(i, "Codigo").ToString() + "'", conn)
            Next i
            mysql_execute("COMMIT", conn)
            outputs_tree.updateGrid()
            Me.Close()
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        export2PDF(GridControl1, "Salida realizada", "Responsable: " + TextBox3.Text + ", | Total: " + TextBox5.Text + " | Fecha: " + TextBox2.Text)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        GridView1.AddNewRow()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        GridView1.DeleteSelectedRows()
    End Sub

    Private Sub GridView1_ShowingEditor(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
        If GridView1.FocusedColumn.FieldName = "Subtotal" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub GridView1_RowUpdated(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowObjectEventArgs) Handles GridView1.RowUpdated
        Try
            ' Mostramos el nombre del producto
            Dim ban = False
            Dim cod As String = ""
            Dim pos As Integer = 0

            Dim conn As New MySqlConnection
            conn = mysql_conexion_up()
            Dim name = mysql_execute("SELECT nombre FROM producto WHERE id = '" + e.Row("Codigo") + "'", conn)(0)("nombre")

            If GridView1.RowCount = 1 Then
                e.Row("Producto") = name
            Else
                For i As Integer = 0 To GridView1.DataRowCount - 2
                    If e.Row("Codigo") = GridView1.GetRowCellValue(i, "Codigo") Then
                        ban = True
                        cod = GridView1.GetRowCellValue(i, "Codigo")
                        pos = i
                        i = GridView1.DataRowCount
                    End If
                Next

                If ban Then
                    Dim cantidad As Integer = GridView1.GetRowCellValue(pos, "Cantidad") + e.Row("Cantidad")
                    GridView1.SetRowCellValue(pos, "Cantidad", cantidad)
                    GridView1.DeleteSelectedRows()
                Else
                    e.Row("Producto") = name
                End If
            End If
        Catch ex As Exception
            MsgBox("El codigo [" + e.Row("Codigo") + "] no es correcto, por favor verifique.")
            Return
        End Try
    End Sub
End Class