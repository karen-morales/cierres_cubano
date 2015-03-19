Public Class invoice_ticket_wizard

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim id As Integer = executeQueryr(String.Format("SELECT idVenta FROM ventas WHERE folio = '{0}' AND DATE(fecha) = '{1}' AND idTienda = {2};", TextBox1.Text, DateTimePicker1.Value.ToString("yyyy-MM-dd"), idTienda))
        If Not TextBox1.Text = "" Then
            If id = 0 Or CInt(TextBox1.Text) = 0 Then
                MsgBox("Este folio no existe en esa fecha, por favor verifiquelo.")
                Exit Sub

            Else
                Dim monto As Double = executeQueryr(String.Format("SELECT monto FROM ventas WHERE folio = {0} AND date(fecha) = '{1}' AND idTienda = '{2}';", TextBox1.Text, DateTimePicker1.Value.ToString("yyyy-MM-dd"), idTienda))
                ' Verificamos si no se ha agregado con anteioridad este folio
                For i = 0 To DataGridView1.Rows.Count - 1
                    If id = Me.DataGridView1(0, i).Value And Me.DataGridView1(3, i).Value = monto And Me.DataGridView1(2, i).Value = DateTimePicker1.Value.ToString("yyyy-MM-dd") Then
                        MsgBox("Este folio ya fue agregado con anterioridad.")
                        Exit Sub
                    End If
                Next
                'Dim monto As Double = executeQueryr(String.Format("SELECT monto FROM ventas WHERE folio = {0} AND date(fecha) = '{1}' AND idTienda = '{2}';", TextBox1.Text, DateTimePicker1.Value.ToString("yyyy-MM-dd"), idTienda))
                DataGridView1.Rows.Add(New String() {id, TextBox1.Text, DateTimePicker1.Value.ToString("yyyy-MM-dd"), monto})
                'Label4.Text = FormatNumber(UpdateTotal, 2, , , TriState.True)
                Label4.Text = UpdateTotal()

            End If
        ElseIf TextBox1.Text = "" Then
            MessageBox.Show("Ingrese folio.")
        End If



    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        DataGridView1.Rows.Clear()
        Label4.Text = "0.0"
    End Sub

    Private Function UpdateTotal() As Double
        Dim total As Double = 0
        'Definimos la variable i para controlar el ciclo for
        Dim i As Integer
        'Definimos del ciclo que va desde que i vale cero hasta que i valga itotal menos uno, osea el penultimo regsitro de la tabla
        For i = 0 To DataGridView1.Rows.Count - 1
            'Double.parse()<---es para convertir a double el valor que se encuentre entre lso parentesis
            'me.datagridview1(4,i).value <----toma todos los valores de la columna 4... 4 es el numero de columna y i es el numero de fila asi toma todos los 
            'valores de esa columna, recuerda que las columnas inician en 0... asi que la 4 enrealidad sera la 5 visualmente
            total = total + Double.Parse(Me.DataGridView1(3, i).Value)
        Next
        UpdateTotal = total
    End Function

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        facturaOtros = True
        invoice_sale_wizard.ShowDialog()
        Me.Close()
    End Sub

    Private Sub GenerarFacturaOtros_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
        DataGridView1.Rows.Clear()
        Label4.Text = "0.0"
    End Sub
End Class