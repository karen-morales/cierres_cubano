Imports MySql.Data.MySqlClient

Public Class loginForm
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
        Dim theconn As New MySqlConnection
        My.Settings.user_login = TextBox1.Text
        My.Settings.Save()
        Try
            theconn = mysql_conexion_up()
            rol = CInt(mysql_execute("SELECT nivel FROM usuarios WHERE nombre = '" + TextBox1.Text + "' AND password = '" + TextBox2.Text + "'", theconn)(0).Item("nivel"))
            If rol > 0 Then
                'idTienda = mysql_execute("SELECT tienda_id FROM usuarios WHERE nombre = '" + TextBox1.Text + "' AND password = '" + TextBox2.Text + "'", theconn)(0).Item("tienda_id")
                'idUsuario = mysql_execute("SELECT idusuarios FROM usuarios WHERE nombre = '" + TextBox1.Text + "' AND password = '" + TextBox2.Text + "'", theconn)(0).Item("idusuarios")
                Dim res = mysql_execute("SELECT tienda_id,idusuarios,nombre FROM usuarios WHERE nombre = '" + TextBox1.Text + "' AND password = '" + TextBox2.Text + "'", theconn)(0)
                idTienda = res.Item("tienda_id")
                idUsuario = res.Item("idusuarios")
                nameUsuario = res.Item("nombre")
                mayoreo = executeQueryr(String.Format("SELECT piezas_mayoreo FROM tiendas WHERE idTienda = '{0}' LIMIT 1;", idTienda)).ToString
                main_window.Show()
                Me.Hide()
            Else
                MsgBox("El usuario o contraseña introducidos son inválidos por favor revise los datos de acceso y vuelva a intentarlo")
            End If
        Catch ex As Exception
            MsgBox("El usuario o contraseña introducidos son inválidos por favor revise los datos de acceso y vuelva a intentarlo")
        End Try
        theconn.Close()
    End Sub

    Private Sub loginForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'createXML()
        ' Si estamos en modo debug debemos utilizar las conexiones locales
        If Debugger.IsAttached Then
            'dbhostr = "127.0.0.1"
            'dbnamer = "cierres_demo"
            'dbuserr = "root"
        End If
        'Dim theconn As New MySqlConnection
        'theconn = mysql_conexion_up()
        'MsgBox(mysql_execute("SELECT LAST_INSERT_ID()  AS id", theconn)(0).Item("id"))
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            Button1_Click(sender, e)
        End If
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            TextBox2.Focus()
        End If
    End Sub

    Private Sub loginForm_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        TextBox1.Text = My.Settings.user_login
        If TextBox1.Text <> "" Then
            TextBox2.Focus()
        End If
    End Sub

    Public Function createXML()
        Dim doc As New System.Xml.XmlDocument
        doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", Nothing))
        Dim cfdi = doc.CreateElement("cfdi", "comprobante", "http://www.sat.gob.mx/cfd/3")
        Dim attr = doc.CreateAttribute("xsi")
        attr.Value = "valores"
        cfdi.Attributes.Append(attr)
        'cfdi.AppendChild(doc.CreateElement("Order"))
        doc.AppendChild(cfdi)
        doc.Save("d:\aaaa_testing.xml")
    End Function

End Class