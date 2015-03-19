Imports MySql.Data.MySqlClient

Public Class order_special_form

    Private Sub order_special_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If Me.Text = "Crear pedido especial" Then
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox3.Text = ""
            fillDataGridByMySqlrDX(GridControl1, "SELECT productoEspecial AS Descripcion, cantidad AS Cantidad, comentario AS Comentario FROM productos_pedidos_especiales WHERE pedidosEspeciales_id = -1 ORDER BY productoEspecial")
            TextBox3.Enabled = True
            GridView1.OptionsBehavior.Editable = True
            btnOK.Text = "Agregar"
            btnCancel.Visible = True
            Button1.Enabled = True
            Button2.Enabled = True
        Else
            TextBox1.Text = order_special_tree.GridView1.GetFocusedRowCellValue("ID").ToString()
            TextBox2.Text = order_special_tree.GridView1.GetFocusedRowCellValue("Fecha").ToString()
            TextBox3.Text = order_special_tree.GridView1.GetFocusedRowCellValue("Comentario").ToString()
            fillDataGridByMySqlrDX(GridControl1, "SELECT productoEspecial AS Descripcion, cantidad AS Cantidad, comentario AS Comentario FROM productos_pedidos_especiales WHERE pedidosEspeciales_id = " + TextBox1.Text + " ORDER BY productoEspecial")
            TextBox3.Enabled = False
            GridView1.OptionsBehavior.Editable = False
            btnOK.Text = "Aceptar"
            btnCancel.Visible = False
            Button1.Enabled = False
            Button2.Enabled = False
        End If
    End Sub

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        If btnOK.Text = "Aceptar" Then
            Me.Close()
        Else
            Dim conn As MySqlConnection
            conn = mysql_conexion_up()
            mysql_execute("START TRANSACTION", conn)
            ' Creamos el pedido
            Dim id = mysql_execute("INSERT INTO pedidos_especiales (fecha_pedidoEspecial,usuario,tienda,comentario,activo) VALUES (NOW(),'" + idUsuario.ToString + "','" + idTienda.ToString + "','" + TextBox3.Text + "','1'); SELECT LAST_INSERT_ID() AS id;", conn)(0)("id")
            For i As Integer = 0 To GridView1.DataRowCount - 1
                mysql_execute("INSERT INTO productos_pedidos_especiales (productoEspecial,pedidosEspeciales_id,cantidad,comentario) VALUES('" + GridView1.GetRowCellValue(i, "Descripcion").ToString() + "','" + id + "','" + GridView1.GetRowCellValue(i, "Cantidad").ToString() + "','" + GridView1.GetRowCellValue(i, "Comentario").ToString() + "')", conn)
            Next i
            mysql_execute("COMMIT", conn)
            order_special_tree.updateGrid()
            Me.Close()
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        GridView1.AddNewRow()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        GridView1.DeleteSelectedRows()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        export2PDF(GridControl1, "Pedido especial", "Número: " + TextBox1.Text + " | Tienda: " + order_special_tree.GridView1.GetFocusedRowCellValue("Tienda").ToString() + " | Fecha: " + TextBox2.Text + " | Comentario: " + TextBox3.Text)
    End Sub
End Class