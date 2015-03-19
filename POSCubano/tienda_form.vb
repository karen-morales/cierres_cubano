Imports System
Imports System.IO
Imports System.Text

Public Class tienda_form
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "(Archivos de cetificados)|*.cer"
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then          
            executeQueryr(String.Format("UPDATE tiendas SET cer_file = '{0}' WHERE idTienda = '{1}';", Convert.ToBase64String(System.IO.File.ReadAllBytes(OpenFileDialog1.FileName)), tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))
            TextBox11.Text = "Certificado cargado satisfactoriamente"
            TextBox11.BackColor = Color.LightGreen
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        OpenFileDialog1.Filter = "(Archivos de llaves)|*.key"
        OpenFileDialog1.FileName = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            executeQueryr(String.Format("UPDATE tiendas SET key_file = '{0}' WHERE idTienda = '{1}';", Convert.ToBase64String(System.IO.File.ReadAllBytes(OpenFileDialog1.FileName)), tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))
            TextBox12.Text = "Llave cargada satisfactoriamente"
            TextBox12.BackColor = Color.LightGreen
        End If
    End Sub

    Private Sub tienda_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Tienda
        TextBox1.Text = tienda_tree.gridFoliosCbb(1, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Nombre
        TextBox2.Text = tienda_tree.gridFoliosCbb(3, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Calle
        TextBox10.Text = tienda_tree.gridFoliosCbb(10, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' RFC
        TextBox3.Text = tienda_tree.gridFoliosCbb(4, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Estado
        TextBox4.Text = tienda_tree.gridFoliosCbb(5, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Colonia
        TextBox5.Text = tienda_tree.gridFoliosCbb(7, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Ciudad
        TextBox6.Text = tienda_tree.gridFoliosCbb(6, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' CURP
        TextBox8.Text = tienda_tree.gridFoliosCbb(8, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' CP
        TextBox7.Text = tienda_tree.gridFoliosCbb(9, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Telefonos
        TextBox9.Text = tienda_tree.gridFoliosCbb(11, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Password certificados
        TextBox13.Text = tienda_tree.gridFoliosCbb(20, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Licencia del PAC
        TextBox14.Text = tienda_tree.gridFoliosCbb(21, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Cuenta NIP
        TextBox15.Text = tienda_tree.gridFoliosCbb(22, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Password de la cuenta del PAC
        TextBox16.Text = tienda_tree.gridFoliosCbb(23, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        'Cantidad de piezas por mayoreo'
        TxtPiezas.Text = tienda_tree.gridFoliosCbb(2, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value
        ' Modo testing de la tienda 
        CheckBox1.Checked = tienda_tree.gridFoliosCbb(24, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value

        ' Leer el .cer
        If Len(executeQueryTextr(String.Format("SELECT cer_file FROM tiendas WHERE idTienda = '{0}';", tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))) > 0 Then
            TextBox11.Text = "Certificado cargado"
            TextBox11.BackColor = Color.LightGreen
        Else
            TextBox11.Text = "Cargar ..."
            TextBox11.BackColor = Color.OrangeRed
        End If
        ' Leer el .key
        If Len(executeQueryTextr(String.Format("SELECT key_file FROM tiendas WHERE idTienda = '{0}';", tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))) > 0 Then
            TextBox12.Text = "Certificado cargado"
            TextBox12.BackColor = Color.LightGreen
        Else
            TextBox12.Text = "Cargar ..."
            TextBox12.BackColor = Color.OrangeRed
        End If

        If CheckBox1.Checked Then
            Timer1.Start()
        End If

        ToolTip1.SetToolTip(Label11, "El certificado de prueba se encuentra en: " + Application.StartupPath + "\cer_test.cer")
        ToolTip1.SetToolTip(Label12, "La llave de pruebas se encuentra en: " + Application.StartupPath + "\key_test.cer")
        ToolTip1.SetToolTip(TextBox11, "El certificado de prueba se encuentra en: " + Application.StartupPath + "\cer_test.cer")
        ToolTip1.SetToolTip(TextBox12, "La llave de pruebas se encuentra en: " + Application.StartupPath + "\key_test.cer")
        'TextBox11.Text = executeQueryTextr(String.Format("SELECT cer_file FROM tiendas WHERE idTienda = '{0}';", tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))
        'TextBox12.Text = executeQueryTextr(String.Format("SELECT key_file FROM tiendas WHERE idTienda = '{0}';", tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        Dim test As Integer = 0

        If CheckBox1.Checked Then
            test = 1
        End If

        Dim num = IsNumeric(TxtPiezas.Text)
        If num Then
            executeQueryr(String.Format("UPDATE tiendas SET tienda='{0}',piezas_mayoreo='{1}',nombre='{2}',calle='{3}',rfc='{4}',estado='{5}',ciudad='{6}',colonia='{7}',curp='{8}',cp='{9}',telefono='{10}',pass='{11}',lic='{12}',cuenta_nip='{13}',pass_pac='{14}',testing={15} WHERE idTienda = '{16}';",
               TextBox1.Text, TxtPiezas.Text, TextBox2.Text, TextBox10.Text, TextBox3.Text.Trim, TextBox4.Text, TextBox6.Text, TextBox5.Text, TextBox8.Text, TextBox7.Text, TextBox9.Text, TextBox13.Text, TextBox14.Text.Trim, TextBox15.Text.Trim, TextBox16.Text, test, tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))
            executeQueryr(String.Format("UPDATE tiendas SET tienda='{0}',piezas_mayoreo='{1}',nombre='{2}',calle='{3}',rfc='{4}',estado='{5}',ciudad='{6}',colonia='{7}',curp='{8}',cp='{9}',telefono='{10}',pass='{11}',lic='{12}',cuenta_nip='{13}',pass_pac='{14}',testing={15} WHERE idTienda = '{16}';",
               TextBox1.Text, TxtPiezas.Text, TextBox2.Text, TextBox10.Text, TextBox3.Text.Trim, TextBox4.Text, TextBox6.Text, TextBox5.Text, TextBox8.Text, TextBox7.Text, TextBox9.Text, TextBox13.Text, TextBox14.Text.Trim, TextBox15.Text.Trim, TextBox16.Text, test, tienda_tree.gridFoliosCbb(0, tienda_tree.gridFoliosCbb.CurrentRow.Index).Value))
            fillDataGridByMySqlr(tienda_tree.gridFoliosCbb, "SELECT * FROM tiendas ORDER BY tienda ASC;")
            Me.Close()
        Else
            MsgBox("Ingrese un valor numérico en piezas por mayoreo")
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        MsgBox("Esta tienda esta configurada para trabajar en modo de PRUEBAS, los comprobantes fiscales que genere no tienen validez para el SAT.", MsgBoxStyle.Information)
    End Sub
End Class