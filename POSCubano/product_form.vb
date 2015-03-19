Public Class product_form

    Private Sub product_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(cmbCategoria, "SELECT id,nombre FROM categorias")
        If Me.Text.Contains("Editar") Then
            txtCosto.Text = product_tree.GridView1.GetFocusedRowCellValue("Costo").ToString
            txtMedida.Text = product_tree.GridView1.GetFocusedRowCellValue("Medida").ToString
            txtNombre.Text = product_tree.GridView1.GetFocusedRowCellValue("Nombre").ToString
            txtPrecio.Text = product_tree.GridView1.GetFocusedRowCellValue("Precio").ToString
            cmbCategoria.SelectedValue = product_tree.GridView1.GetFocusedRowCellValue("categoria_id").ToString
            txtId.Text = product_tree.GridView1.GetFocusedRowCellValue("ID").ToString

            txtId1.Text = txtId.Text

            updateGrid()
        Else
            txtId.Text = ""
            txtCosto.Text = ""
            txtMedida.Text = ""
            txtNombre.Text = ""
            txtPrecio.Text = ""
            'cmbCategoria.SelectedValue = 0
            fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, a.nombre AS Nombre, b.nombre AS Categoria, b.id AS categoria_id, c.nombre AS Color, c.id AS color_id, d.nombre AS Figura, d.id AS figura_id FROM producto a INNER JOIN categorias b ON (a.categorias_id = b.id) INNER JOIN color c ON (a.color_id = c.id) LEFT JOIN figura d ON (a.figura_id = d.id) WHERE productoTienda_idproductoTienda = '0000000'")
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAgregarCliente_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        If Me.Text.Contains("Editar") Then
            ' Actualizamos la tabla producto
            executeQueryTextr("UPDATE producto_tienda SET nombre = '" + txtNombre.Text + "',precio = '" + txtPrecio.Text + "',costo = '" + txtCosto.Text + "',medida = '" + txtMedida.Text + "',categorias_id = '" + cmbCategoria.SelectedValue.ToString + "' WHERE idproductoTienda = '" + txtId1.Text + "'")
        Else
            ' Insertamos en la tabla producto tienda
            If executeQueryTextr("SELECT idproductoTienda FROM producto_tienda WHERE idproductoTienda = '" + txtId.Text + "'") = "" Then
                executeQueryr("INSERT INTO producto_tienda (idproductoTienda,nombre,precio,costo,medida,categorias_id) VALUES ('" + txtId.Text + "','" + txtNombre.Text + "','" + txtPrecio.Text + "','" + txtCosto.Text + "','" + txtMedida.Text + "','" + cmbCategoria.SelectedValue.ToString + "')")
                ' Insertamos los colores en la tabla producto
            End If

        End If
            Me.Close()
    End Sub

    Private Sub btnAddLine_Click(sender As System.Object, e As System.EventArgs) Handles btnAddLine.Click
        If Me.Text.Contains("Crear") Then
            ' Actualizamos la tabla producto
            Try
                executeQueryr("INSERT INTO producto_tienda (idproductoTienda,nombre,precio,costo,medida,categorias_id) VALUES ('" + txtId.Text + "','" + txtNombre.Text + "','" + txtPrecio.Text + "','" + txtCosto.Text + "','" + txtMedida.Text + "','" + cmbCategoria.SelectedValue.ToString + "')")
            Catch ex As Exception
                MsgBox(ex.Message)
                Return
            End Try
        End If
        product_color_form.Text = "Crear color de producto"
        product_color_form.ShowDialog()
        updateGrid()
    End Sub

    Private Sub bntMod_Click(sender As System.Object, e As System.EventArgs) Handles bntMod.Click
        If GridView1.SelectedRowsCount > 0 Then
            product_color_form.Text = "Editar color de producto"
            product_color_form.ShowDialog()
            updateGrid()
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridControl1.DoubleClick
        product_color_form.Text = "Editar color de producto"
        product_color_form.ShowDialog()
        updateGrid()
    End Sub

    Public Function updateGrid()
        fillDataGridByMySqlrDX(GridControl1, "SELECT a.id AS ID, a.nombre AS Nombre, b.nombre AS Categoria, b.id AS categoria_id, c.nombre AS Color, c.id AS color_id, d.nombre AS Figura, d.id AS figura_id FROM producto a INNER JOIN categorias b ON (a.categorias_id = b.id) INNER JOIN color c ON (a.color_id = c.id) LEFT JOIN figura d ON (a.figura_id = d.id)  WHERE productoTienda_idproductoTienda = '" + txtId.Text + "'")
        If GridView1.RowCount > 0 Then
            GridView1.Columns("categoria_id").Visible = False
            GridView1.Columns("color_id").Visible = False
            GridView1.Columns("figura_id").Visible = False
        End If
        GridView1.BestFitColumns()
    End Function

    Private Sub cmbCategoria_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbCategoria.SelectedIndexChanged
        txtId.Text = cmbCategoria.SelectedValue.ToString + txtMedida.Text
        txtNombre.Text = cmbCategoria.Text + " " + txtMedida.Text
    End Sub

    Private Sub txtMedida_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtMedida.TextChanged
        txtId.Text = cmbCategoria.SelectedValue.ToString + txtMedida.Text
        txtNombre.Text = cmbCategoria.Text + " " + txtMedida.Text
    End Sub
End Class