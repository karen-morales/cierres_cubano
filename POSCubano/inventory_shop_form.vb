Public Class inventory_shop_form

    Private Sub inventory_shop_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        code.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Codigo").ToString()
        TextBox11.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Codigo").ToString() + " - " + inventory_shop_tree.GridView1.GetFocusedRowCellValue("Nombre").ToString()
        TextBox1.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 1").ToString()
        TextBox2.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 2").ToString()
        TextBox3.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 3").ToString()
        TextBox4.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 4").ToString()
        TextBox5.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 5").ToString()
        TextBox6.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 1").ToString()
        TextBox7.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 2").ToString()
        TextBox8.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 3").ToString()
        TextBox9.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 4").ToString()
        TextBox10.Text = inventory_shop_tree.GridView1.GetFocusedRowCellValue("Tienda 5").ToString()
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        If TextBox6.Text <> "" And TextBox7.Text <> "" And TextBox8.Text <> "" And TextBox9.Text <> "" And TextBox10.Text <> "" Then
            If IsNumeric(TextBox6.Text) And IsNumeric(TextBox7.Text) And IsNumeric(TextBox8.Text) And IsNumeric(TextBox9.Text) And IsNumeric(TextBox10.Text) Then
                executeQueryr(String.Format("UPDATE producto_tienda SET tienda1 = '{0}',tienda2 = '{1}',tienda3 = '{2}',tienda4 = '{3}',tienda5 = '{4}' WHERE idproductoTienda = '{5}'", TextBox6.Text, TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox10.Text, code.Text))
                If TextBox1.Text <> TextBox6.Text Then
                    Dim temp As Integer = CInt(TextBox6.Text) - CInt(TextBox1.Text)
                    executeQueryr(String.Format("INSERT INTO listaMovimientosTiendas (producto_tiendaId,cantidad,fecha,tienda) VALUES('{0}','{1}',NOW(),1)", code.Text, temp.ToString))
                End If
                If TextBox2.Text <> TextBox7.Text Then
                    Dim temp As Integer = CInt(TextBox7.Text) - CInt(TextBox2.Text)
                    executeQueryr(String.Format("INSERT INTO listaMovimientosTiendas (producto_tiendaId,cantidad,fecha,tienda) VALUES('{0}','{1}',NOW(),2)", code.Text, temp.ToString))
                End If
                If TextBox3.Text <> TextBox8.Text Then
                    Dim temp As Integer = CInt(TextBox8.Text) - CInt(TextBox3.Text)
                    executeQueryr(String.Format("INSERT INTO listaMovimientosTiendas (producto_tiendaId,cantidad,fecha,tienda) VALUES('{0}','{1}',NOW(),3)", code.Text, temp.ToString))
                End If
                If TextBox4.Text <> TextBox9.Text Then
                    Dim temp As Integer = CInt(TextBox9.Text) - CInt(TextBox4.Text)
                    executeQueryr(String.Format("INSERT INTO listaMovimientosTiendas (producto_tiendaId,cantidad,fecha,tienda) VALUES('{0}','{1}',NOW(),4)", code.Text, temp.ToString))
                End If
                If TextBox5.Text <> TextBox10.Text Then
                    Dim temp As Integer = CInt(TextBox10.Text) - CInt(TextBox5.Text)
                    executeQueryr(String.Format("INSERT INTO listaMovimientosTiendas (producto_tiendaId,cantidad,fecha,tienda) VALUES('{0}','{1}',NOW(),5)", code.Text, temp.ToString))
                End If
                inventory_shop_tree.UpdateGrid()
                Me.Close()
            Else
                MsgBox("Alguno de los valores introducidos no es un número válido por favor verifique")
            End If
        End If
    End Sub
End Class