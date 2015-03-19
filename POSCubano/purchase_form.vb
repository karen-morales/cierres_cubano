Imports MySql.Data.MySqlClient

Public Class purchase_form
    Private last_code As String = ""
    Private Sub purchase_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(ComboBox1, "SELECT idCliente,nombre FROM partners WHERE tipo = 1")
        If Me.Text = "Detalle de compra" Then
            TextBox1.Text = purchase_tree.GridView1.GetFocusedRowCellValue("ID").ToString()
            TextBox2.Text = purchase_tree.GridView1.GetFocusedRowCellValue("Fecha").ToString()
            TextBox3.Text = purchase_tree.GridView1.GetFocusedRowCellValue("Usuario").ToString()
            TextBox5.Text = purchase_tree.GridView1.GetFocusedRowCellValue("Total").ToString()
            TextBox6.Text = purchase_tree.GridView1.GetFocusedRowCellValue("Factura").ToString()
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.producto_id AS Codigo, b.nombre AS Producto, a.cantidad AS Cantidad, a.costo AS Costo, a.subtotal AS Subtotal FROM entrada_producto_has_producto a INNER JOIN producto b ON (a.producto_id = b.id) WHERE entrada_producto_identrada_producto = " + TextBox1.Text + " ORDER BY b.nombre")
            ComboBox1.SelectedValue = purchase_tree.GridView1.GetFocusedRowCellValue("proveedor_id").ToString()

            TextBox6.Enabled = False
            ComboBox1.Enabled = False

            GridView1.OptionsBehavior.Editable = False
            btnOK.Text = "Aceptar"
            btnCancel.Visible = False
            Button1.Enabled = False
            Button2.Enabled = False
        Else
            fillDataGridByMySqlrDX(GridControl1, "SELECT producto_id AS Codigo, '' AS Producto, cantidad AS Cantidad, costo AS Costo, subtotal AS Subtotal FROM entrada_producto_has_producto WHERE entrada_producto_identrada_producto = '000000'")
            GridView1.OptionsBehavior.Editable = True
            TextBox6.Enabled = True
            ComboBox1.Enabled = True
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = ""
            btnOK.Text = "Agregar"
            btnCancel.Visible = True
            Button1.Enabled = True
            Button2.Enabled = True
        End If
        If GridView1.Columns("Subtotal").Summary.ActiveCount = 0 Then
            GridView1.Columns("Subtotal").Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Subtotal", "{0:#,##0.00}")
        End If

        If rol = 3 Then
            GridView1.Columns("Subtotal").Visible = True
            GridView1.Columns("Costo").Visible = True
            TextBox5.Show()
            Label5.Show()
        Else
            GridView1.Columns("Subtotal").Visible = False
            GridView1.Columns("Costo").Visible = False
            TextBox5.Show()
            TextBox5.Hide()
            Label5.Hide()
        End If
        GridView1.BestFitColumns()

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If btnOK.Text = "Aceptar" Then
            Me.Close()
        ElseIf GridView1.DataRowCount > 0 Then
            If Not TextBox6.Text = "" Then
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
                    If IsDBNull(GridView1.GetRowCellValue(i, "Cantidad")) Or IsDBNull(GridView1.GetRowCellValue(i, "Costo")) Then
                        MsgBox("Las cantidades a comprar y el costo deben ser númericos por favor revise los datos introducidos")
                        conn.Close()
                        Return
                    End If

                    ' Calculamos el total
                    total += CInt(GridView1.GetRowCellValue(i, "Cantidad")) * CDbl(GridView1.GetRowCellValue(i, "Costo"))

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
                Dim id = mysql_execute("INSERT INTO entrada_producto (proveedores_id,numero_factura,fecha_entrada,total,usuarios_idusuarios) VALUES('" + ComboBox1.SelectedValue.ToString + "','" + TextBox6.Text + "',NOW(),'" + total.ToString + "','" + idUsuario.ToString + "'); SELECT LAST_INSERT_ID() AS id", conn)(0)("id")
                For i As Integer = 0 To GridView1.DataRowCount - 1
                    mysql_execute("INSERT INTO entrada_producto_has_producto (entrada_producto_identrada_producto,producto_id,cantidad,costo,subtotal) VALUES('" + id.ToString + "','" + GridView1.GetRowCellValue(i, "Codigo").ToString() + "','" + GridView1.GetRowCellValue(i, "Cantidad").ToString() + "','" + GridView1.GetRowCellValue(i, "Costo").ToString() + "','" + Str(CDbl(GridView1.GetRowCellValue(i, "Cantidad").ToString()) * CDbl(GridView1.GetRowCellValue(i, "Costo").ToString())) + "')", conn)
                    mysql_execute("UPDATE producto SET inventarios_bodega = inventarios_bodega + " + GridView1.GetRowCellValue(i, "Cantidad").ToString() + " WHERE id = '" + GridView1.GetRowCellValue(i, "Codigo").ToString() + "'", conn)
                Next i
                mysql_execute("COMMIT", conn)
                purchase_tree.updateGrid()
                Me.Close()
            Else
                MsgBox("Ingrese número de factura. ")
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
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
            'If last_code <> e.Row("Codigo") Then
            Dim conn As New MySqlConnection
            conn = mysql_conexion_up()
            Dim name = mysql_execute("SELECT nombre FROM producto WHERE id = '" + e.Row("Codigo") + "'", conn)(0)("nombre")

            If GridView1.RowCount = 1 Then
                e.Row("Producto") = name
                e.Row("Subtotal") = e.Row("Cantidad") * e.Row("Costo")
                'last_code = e.Row("Codigo")
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
                    Dim costo As Double = GridView1.GetRowCellValue(pos, "Costo")
                    GridView1.SetRowCellValue(pos, "Cantidad", cantidad)
                    GridView1.SetRowCellValue(pos, "Subtotal", (cantidad * costo))
                    GridView1.DeleteSelectedRows()
                Else
                    e.Row("Producto") = name
                    e.Row("Subtotal") = e.Row("Cantidad") * e.Row("Costo")
                End If

            End If
        Catch ex As Exception
            e.Row("Subtotal") = 0
        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        export2PDF(GridControl1, "Compra realizada", "Comprador " + TextBox3.Text + ", Proveeedor: " + ComboBox1.Text + ", Total: " + TextBox5.Text + " | Fecha: " + TextBox2.Text)
    End Sub
End Class