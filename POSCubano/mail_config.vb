Public Class mail_config

    Private Sub mail_config_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        txtServidorCorreo.Text = executeQueryTextr("SELECT server FROM smtp")
        txtUsuarioCorreo.Text = executeQueryTextr("SELECT user FROM smtp")
        txtPassCorreo.Text = executeQueryTextr("SELECT pass FROM smtp")
        txtPuertoCorreo.Text = executeQueryTextr("SELECT port FROM smtp")
        Dim ssl As String = executeQueryTextr("SELECT secure_ssl FROM smtp")

        If ssl = 1 Then
            checkboxSSL.Checked = True
        Else
            checkboxSSL.Checked = False
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        Dim ssl As String

        If checkboxSSL.Checked Then
            ssl = "1"
        Else
            ssl = "0"
        End If
        executeQueryr("UPDATE smtp SET server = '" + txtServidorCorreo.Text + "', user = '" + txtUsuarioCorreo.Text + "', pass = '" + txtPassCorreo.Text + "', port = '" + txtPuertoCorreo.Text + "', secure_ssl = '" + ssl + "'")
        ' Actualizamos los datos en esta maquina
        My.Settings.server = txtServidorCorreo.Text
        My.Settings.user = txtUsuarioCorreo.Text
        My.Settings.pass = txtPassCorreo.Text
        My.Settings.port = txtPuertoCorreo.Text
        My.Settings.ssl = checkboxSSL.Checked
        Me.Close()
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        Me.Close()
    End Sub
End Class