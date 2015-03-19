Imports MySql.Data.MySqlClient
Imports System.Threading

Public Class pos_form
    Dim celda As Integer
    Dim search As String

    Private Sub DataGridView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.DoubleClick
        If CInt(DataGridView1.Item(3, DataGridView1.CurrentRow.Index).Value.ToString) > 0 Then
            DataGridView2.Rows.Add(New String() {1, DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value.ToString, DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value.ToString, DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value.ToString, DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString})
            Label2.Text = FormatNumber(UpdateTotal, 2, , , TriState.True)
            DataGridView2.CurrentCell = DataGridView2.Item(0, DataGridView2.Rows.Count - 1)
            DataGridView2.Focus()
        End If
    End Sub

    Private Sub DataGridView2_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellEndEdit
        Dim temp As Double
        Dim str As String
        Dim can = String.Format("SELECT tienda{1} FROM producto_tienda WHERE nombre ='{0}'", DataGridView2.CurrentRow.Cells(1).Value.ToString, idTienda)
        temp = executeQueryr(can)
        If DataGridView2.CurrentRow.Cells(0).Value > temp Then
            MsgBox("No puede vender más cantidad de lo que tienen en inventario.")
        Else
            If DataGridView2.CurrentRow.Cells(0).Value >= mayoreo Then
                str = String.Format("SELECT precio FROM producto_tienda WHERE idproductoTienda = '{0}' LIMIT 1;", DataGridView2.CurrentRow.Cells(4).Value.ToString)
                temp = executeQueryr(str)
                DataGridView2.CurrentRow.Cells(2).Value = temp
                '            DataGridView2.CurrentRow.Cells(2).Style.BackColor = Color.Red
                '           Timer1.Enabled = True
            End If
        End If
    End Sub

    Private Sub DataGridView2_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellValueChanged
        Try
            DataGridView2.Rows(DataGridView2.CurrentRow.Index).Cells(3).Value = Double.Parse((DataGridView2.CurrentRow.Cells(0).Value * DataGridView2.CurrentRow.Cells(2).Value))
            Label2.Text = FormatNumber(UpdateTotal, 2, , , TriState.True)
        Catch ex As Exception
        End Try
    End Sub

    Private Function UpdateTotal() As Double
        Dim total As Double = 0
        'Definimos la variable i para controlar el ciclo for
        Dim i As Integer
        'Definimos del ciclo que va desde que i vale cero hasta que i valga itotal menos uno, osea el penultimo regsitro de la tabla
        For i = 0 To DataGridView2.Rows.Count - 1
            'Double.parse()<---es para convertir a double el valor que se encuentre entre lso parentesis
            'me.datagridview1(4,i).value <----toma todos los valores de la columna 4... 4 es el numero de columna y i es el numero de fila asi toma todos los 
            'valores de esa columna, recuerda que las columnas inician en 0... asi que la 4 enrealidad sera la 5 visualmente
            total = total + Double.Parse(Me.DataGridView2(3, i).Value)
        Next
        If CheckBox2.Checked = True Then
            total *= 1.16
        End If
        UpdateTotal = total
    End Function

    Private Sub DataGridView2_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView2.RowsRemoved
        Label2.Text = FormatNumber(UpdateTotal, 2, , , TriState.True)
    End Sub
    'Vender
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim myconn As MySqlConnection
        myconn = mysql_conexion_up()
        If DataGridView2.Rows.Count > 0 Then

            'Actualizamos y verificamos stock
            Dim vende As Boolean = True
            Dim temp As Integer
            For i = 0 To DataGridView2.Rows.Count - 1
                Dim stock = String.Format("SELECT tienda{1} AS Qty FROM producto_tienda WHERE nombre = '{0}';", DataGridView2(1, i).Value.ToString, idTienda.ToString)
                temp = executeQueryr(stock)
                Dim can As Double = Double.Parse(DataGridView2(0, i).Value.ToString)
                If can > temp Then
                    vende = False
                    MsgBox("Intenta vender una cantida mayor a la que tiene en inventario del producto " + DataGridView2(1, i).Value.ToString + "")
                    i = DataGridView2.Rows.Count
                End If
            Next

            If vende = True Then
                mysql_execute("START TRANSACTION", myconn)
                Try
                    Dim tienda As String = "tienda" + idTienda.ToString
                    Dim monto As Double = 0
                    'Definimos la variable i para controlar el ciclo for
                    Dim i As Integer
                    Dim folio As Integer = mysql_execute(String.Format("SELECT folio FROM foliossat WHERE idTienda = '{0}';", idTienda.ToString), myconn)(0).Item("folio") + 1
                    'Obtenemos monto de venta
                    For i = 0 To DataGridView2.Rows.Count - 1
                        monto = monto + (CInt(DataGridView2(0, i).Value) * CDbl(DataGridView2(2, i).Value))
                    Next
                    ' Creamos la cabecera de la venta
                    Dim idventa As Integer
                    If CheckBox2.Checked = True Then
                        idventa = mysql_execute(String.Format("INSERT INTO ventas (fecha,monto,idEstatus,idTienda,folio,metodo,vendedor) VALUES (NOW(),round({0},2),'1','{1}','{2}','{3}','{4}'); SELECT LAST_INSERT_ID() AS id;", monto * 1.16, idTienda.ToString, folio, cmbPaymentMethod.SelectedValue, vendedorCmb.SelectedValue), myconn)(0).Item("id")
                    Else
                        idventa = mysql_execute(String.Format("INSERT INTO ventas (fecha,monto,idEstatus,idTienda,folio,metodo,vendedor) VALUES (NOW(),round({0},2),'1','{1}','{2}','{3}','{4}'); SELECT LAST_INSERT_ID() AS id;", monto, idTienda.ToString, folio, cmbPaymentMethod.SelectedValue, vendedorCmb.SelectedValue), myconn)(0).Item("id")
                    End If
                    ' Insertamos los detalles de la venta
                    For i = 0 To DataGridView2.Rows.Count - 1
                        If CheckBox2.Checked = True Then
                            mysql_execute(String.Format("INSERT INTO ventas_detalle (idVenta,idproductoTienda,cantidad,precio,fecha,idTienda) VALUES ({0},'{1}',{2},round({3},2),now(),{4}); SELECT LAST_INSERT_ID();", idventa, DataGridView2(4, i).Value, DataGridView2(0, i).Value, DataGridView2(2, i).Value * 1.16, idTienda), myconn)
                        Else
                            mysql_execute(String.Format("INSERT INTO ventas_detalle (idVenta,idproductoTienda,cantidad,precio,fecha,idTienda) VALUES ({0},'{1}',{2},round({3},2),now(),{4}); SELECT LAST_INSERT_ID();", idventa, DataGridView2(4, i).Value, DataGridView2(0, i).Value, DataGridView2(2, i).Value, idTienda), myconn)
                        End If
                        ' Descontamos los productos del stock
                        mysql_execute(String.Format("UPDATE producto_tienda SET " + tienda + " = " + tienda + " - {0} WHERE idproductoTienda = '{1}';", DataGridView2(0, i).Value, DataGridView2(4, i).Value), myconn)
                    Next
                    'Actualizamos el folio de la tabla foliossat
                    If CheckBox2.Checked = False Then
                        mysql_execute(String.Format("UPDATE foliossat SET folio = folio + 1 WHERE idTienda = '{0}';", idTienda.ToString), myconn)
                    End If

                    mysql_execute("COMMIT;", myconn)
                    myconn.Close()
                    invoice_ok = True

                    If CheckBox2.Checked = True Then
                        currentSale = idventa
                        invoice_sale_wizard.ShowDialog()
                    Else
                        generarNota(idventa.ToString)
                    End If
                    '
                    If CheckBox1.Checked Then
                        fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%'", search, idTienda))
                    Else
                        fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE (nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%') AND tienda{1} > 0", search, idTienda))
                    End If
                    ' Limpiamos la venta

                    If invoice_ok Then
                        DataGridView2.Rows.Clear()
                        CheckBox2.Checked = False
                    End If
                    '
                Catch ex As Exception
                    mysql_execute("ROLLBACK;", myconn)
                    myconn.Close()
                End Try
            Else
                MsgBox("Verifique el stock de inventario.")
                If CheckBox1.Checked Then
                    fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%'", search, idTienda))
                Else
                    fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE (nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%') AND tienda{1} > 0", search, idTienda))
                End If
            End If
        End If
    End Sub

    Private Sub DataGridView1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles DataGridView1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            If CInt(DataGridView1.Item(3, DataGridView1.CurrentRow.Index).Value.ToString) > 0 Then
                DataGridView2.Rows.Add(New String() {1, DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value.ToString, DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value.ToString, DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value.ToString, DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString})
                Label2.Text = FormatNumber(UpdateTotal, 2, , , TriState.True)
                DataGridView2.CurrentCell = DataGridView2.Item(0, DataGridView2.Rows.Count - 1)
                DataGridView2.Focus()
            End If
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        DataGridView2.CurrentRow.Cells(2).Style.BackColor = Color.White
        Timer1.Enabled = False
    End Sub

    Private Sub Pos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(cmbPaymentMethod, "SELECT id,description FROM metodos_de_pago;")
        fillComboByMySqlr(vendedorCmb, "SELECT id,nombre FROM vendedores WHERE tienda = " + idTienda)
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            If TextBox1.Text <> "" Then
                search = TextBox1.Text
                If CheckBox1.Checked Then
                    fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%'", search, idTienda))
                Else
                    fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE (nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%') AND tienda{1} > 0", search, idTienda))
                End If
                TextBox1.Text = ""
            End If
            DataGridView1.Focus()
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        Label2.Text = FormatNumber(UpdateTotal, 2, , , TriState.True)
    End Sub

    Private Sub DataGridView2_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles DataGridView2.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            TextBox1.Focus()
        End If
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        DataGridView2.Rows.Clear()
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Try
            DataGridView2.Rows.Remove(DataGridView2.CurrentRow)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button3_Click_1(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If TextBox1.Text <> "" Then
            search = TextBox1.Text
            If CheckBox1.Checked Then
                fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%'", search, idTienda))
            Else
                fillDataGridByMySqlr(DataGridView1, String.Format("SELECT idproductoTienda AS REF, nombre AS Nombre, precio AS Precio, tienda{1} AS Qty FROM producto_tienda WHERE (nombre LIKE '%{0}%' OR idproductoTienda LIKE '%{0}%') AND tienda{1} > 0", search, idTienda))
            End If
            TextBox1.Text = ""
        End If
        DataGridView1.Focus()
    End Sub
End Class