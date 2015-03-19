Public Class change_password_form
    Public my_pass As Boolean = True

    Private Sub btnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        Dim id As String = "Any Value"
        If my_pass Then
            ' Verificamos que la contraseña anterior sea la correcta
            id = executeQueryTextr("SELECT idusuarios FROM usuarios WHERE idusuarios = '" + idUsuario.ToString + "' AND password = '" + TextBox1.Text + "'")
        End If
        If Len(id) > 0 Then
            ' Validamos que la ccontraseña y la confirmación sean las mismas
            If TextBox2.Text <> TextBox3.Text Then
                MsgBox("La contraseña nueva y la confirmación no coinciden, por favor inténtelo nuevamente.", MsgBoxStyle.Exclamation)
                TextBox2.Text = ""
                TextBox1.Text = ""
            Else
                ' Validamos la seguridad de la nueva contraseña
                If ValidatePassword(TextBox2.Text) Then
                    If my_pass Then
                        executeQueryTextr("UPDATE usuarios SET password = '" + TextBox2.Text + "' WHERE idusuarios = '" + idUsuario.ToString + "'")
                    Else
                        executeQueryTextr("UPDATE usuarios SET password = '" + TextBox2.Text + "' WHERE idusuarios = '" + ComboBox1.SelectedValue.ToString + "'")
                    End If
                    MsgBox("La contraseña se ha cambiado satisfactoriamente")
                    Me.Close()
                Else
                    MsgBox("La contraseña no cumple con los requisitos mínimos de seguridad, debe tener mínimo una letra mayúscula, un número y ser de mínimo 6 caracteres.", MsgBoxStyle.Critical)
                End If
            End If
        Else
            MsgBox("La contraseña anterior no coincide", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub change_password_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If my_pass Then
            Label1.Visible = True
            TextBox1.Visible = True
            Label5.Visible = False
            ComboBox1.Visible = False
        Else
            Label1.Visible = False
            TextBox1.Visible = False
            Label5.Visible = True
            ComboBox1.Visible = True
        End If

        fillComboByMySqlr(ComboBox1, "SELECT idusuarios,nombre FROM usuarios WHERE idusuarios <> " + idUsuario.ToString)
    End Sub
End Class