Public Class product_color_form
    Private Sub producto_colores_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(cmbCategoria, "SELECT id,nombre FROM categorias")
        fillComboByMySqlr(cmbColor, "SELECT id,nombre FROM color")
        fillComboByMySqlr(cmbPtienda, "SELECT idproductoTienda,nombre FROM producto_tienda")
        fillComboByMySqlr(cmbFigura, "SELECT id,nombre FROM figura")



        cmbCategoria.SelectedValue = product_form.cmbCategoria.SelectedValue.ToString
        txtMedida.Text = product_form.txtMedida.Text

        If Me.Text.Contains("Editar") Then
            txtId.Text = product_form.GridView1.GetFocusedRowCellValue("ID").ToString

            txtId1.Text = txtId.Text

            txtNombre.Text = product_form.GridView1.GetFocusedRowCellValue("Nombre").ToString
            cmbColor.SelectedValue = product_form.GridView1.GetFocusedRowCellValue("color_id").ToString
            cmbFigura.SelectedValue = product_form.GridView1.GetFocusedRowCellValue("figura_id").ToString
            cmbPtienda.SelectedValue = product_form.txtId.Text
        Else
            txtId.Text = product_form.txtId.Text
            txtNombre.Text = product_form.txtNombre.Text
            cmbColor.SelectedIndex = 0
            cmbFigura.SelectedItem = "00"
            cmbPtienda.SelectedValue = product_form.txtId.Text
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        generateName()
        ' Verificamos que no existe ese id
        If Me.Text.Contains("Editar") Then
            executeQueryTextr("UPDATE producto SET nombre = '" + txtNombre.Text + "',categorias_id = '" + cmbCategoria.SelectedValue.ToString + "',figura_id = '" + cmbFigura.SelectedValue.ToString + "',color_id = '" + cmbColor.SelectedValue.ToString + "', medida = '" + txtMedida.Text + "' WHERE id = '" + txtId1.Text + "'")

            Me.Close()
        Else
            If executeQueryTextr("SELECT id FROM producto WHERE id = '" + txtId.Text + "'") = "" Then
                executeQueryTextr("INSERT INTO producto (id,nombre,categorias_id,color_id,medida,productoTienda_idproductoTienda,figura_id) VALUES ('" + txtId.Text + "','" + txtNombre.Text + "','" + cmbCategoria.SelectedValue.ToString + "','" + cmbColor.SelectedValue.ToString + "','" + txtMedida.Text + "','" + cmbPtienda.SelectedValue.ToString + "','" + cmbFigura.SelectedValue.ToString + "')")
                Me.Close()
            Else
                MsgBox("Este color ya esta agregado")
            End If
        End If
    End Sub

    Private Sub cmbColor_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbColor.SelectedIndexChanged
        generateName()
    End Sub

    Private Sub cmbFigura_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cmbFigura.SelectedIndexChanged
        generateName()
    End Sub

    Public Function generateName()
        Try
            If cmbFigura.SelectedValue.ToString <> "00" Then
                txtNombre.Text = product_form.txtNombre.Text + " " + cmbColor.Text + " " + cmbFigura.Text
                txtId.Text = product_form.txtId.Text + cmbColor.SelectedValue.ToString + cmbFigura.SelectedValue.ToString
            Else
                txtNombre.Text = product_form.txtNombre.Text + " " + cmbColor.Text
                txtId.Text = product_form.txtId.Text + cmbColor.SelectedValue.ToString
            End If
        Catch ex As Exception
        End Try
        generateName = True
    End Function
End Class