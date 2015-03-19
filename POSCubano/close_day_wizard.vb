Public Class close_day_wizard

    Private Sub closeDay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Tomamos la fecha de la ultima apertura
        Dim last_opening As String = executeQueryTextr("SELECT DATE_FORMAT(fecha_apertura, '%Y-%m-%d %H:%i:%s') FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "'")
        'Venta EFE
        TextBox1.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(A.monto) FROM ventas A INNER JOIN estatus B ON (A.idEstatus = B.idEstatus) WHERE (A.idEstatus != '2' AND metodo = 1) AND (A.idTienda = " + idTienda + " AND A.fecha > '" + last_opening + "')"))
        ' Venta DEP
        TextBox4.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(A.monto) FROM ventas A INNER JOIN estatus B ON (A.idEstatus = B.idEstatus) WHERE (A.idEstatus != '2' AND metodo != 1) AND (A.idTienda = " + idTienda + " AND A.fecha > '" + last_opening + "')"))
        ' Iva extra
        TextBox8.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(monto) FROM extra_iva WHERE tienda_id = " + idTienda + " AND fecha > '" + last_opening + "'"))
        ' Apertura
        TextBox2.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(apertura) FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "' ORDER BY fecha_apertura DESC"))
        ' Gastos
        TextBox5.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(monto) FROM gastos WHERE idTienda = " + idTienda + " AND fecha > '" + last_opening + "'"))
        ' Total
        TextBox3.Text = String.Format("{0:n2}", CDbl(TextBox1.Text) + CDbl(TextBox2.Text) + CDbl(TextBox4.Text))
        ' Efectivo
        TextBox6.Text = String.Format("{0:n2}", CDbl(TextBox1.Text) + CDbl(TextBox2.Text) + CDbl(TextBox8.Text) - CDbl(TextBox5.Text))
        Panel1.Visible = True
        Panel2.Visible = False
        TextBox7.Text = "0"
        Me.ControlBox = True
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        'Panel1.Visible = False
        'Panel2.Visible = True
        Me.ControlBox = False

        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult

        Button2.Enabled = False
        style = MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information Or MsgBoxStyle.YesNo

        If IsNumeric(TextBox6.Text) Then
            If TextBox6.Text - TextBox7.Text < 0 Then
                MsgBox("No puedes retirar mas efectivo del que tienes actualmente en caja. Inténtalo de nuevo")
                Exit Sub
            End If

            response = MsgBox(String.Format("IMPORTANTE: Esta acción no se puede deshacer, está a punto de cerrar el día retirando {0} de efectivo y dejando {1} para la apertura del día siguiente. Presione Si para confirmar.", TextBox7.Text, TextBox6.Text - TextBox7.Text), style, "Cerrar dia")
            If response = MsgBoxResult.Yes Then   ' User chose Yes.
                If cerrarDia(CDbl(TextBox6.Text - TextBox7.Text)) Then
                    'Cerramos todos los forms
                    For Each f As Form In main_window.MdiChildren
                        f.Close()
                    Next
                    Me.Close()
                Else
                    MsgBox("Ocurrió un error al intentar cerra caja, inténtelo de nuevo y si el problema persite consulte a su administrador")
                End If
                Button2.Enabled = True
            End If
        Else
            MsgBox("Debe introducir un valor númerico para retirar de la caja. Inténtelo de nuevo.")
        End If
        Me.ControlBox = True
    End Sub

    Private Sub Button2_Click_1(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class