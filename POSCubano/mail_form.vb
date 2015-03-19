Imports tagcode.xml.CFDI
Imports tagcode.xml
Imports tagcode.xml.CFDI.Emisor
Imports tagcode.xml.CFDI.Receptor
Imports tagcode.view
Imports System.Net.Mail
Imports System.Threading

Public Class mail_form
    Dim xml_sign As String
    Dim xml_pdf As String
    Public folio As String
    Public generic As Integer = 0
    Public tienda As String

    ' folio: recibe folio de la factura cuando se esta trabajando con la base de datos local (remote=0)
    '        y recibo el ID de la factura cuando se trabaja con la base remota (remote = 1)

    Private Sub mail_form_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Dim folio As String = sales_tree.gridVentas.Item(5, sales_tree.gridVentas.CurrentRow.Index).Value.ToString
        Dim idfactura As String
        Dim idshop As String = 0

        If tienda <> "" Then
            idfactura = executeQueryr("SELECT idFactura FROM facturas WHERE folio = " + folio + " AND idTienda = " + tienda)
            idshop = executeQueryr("SELECT idTienda FROM facturas WHERE idFactura = " + idfactura)
        Else
            idfactura = executeQueryr("SELECT idFactura FROM facturas WHERE folio = " + folio + " AND idTienda = " + idTienda)
            'idfactura = folio
        End If

        ' Generamos el PDF
        xml_pdf = generarFactura(idfactura, generic, False)
        ' Generamos el XML
        xml_sign = xml_pdf.Split(".")(0) + ".xml"

        My.Computer.FileSystem.WriteAllText(xml_sign, executeQueryTextr("SELECT xml_timbrado FROM facturas WHERE idFactura = " + idfactura), False)

        ' Creamos el asunto del mensaje
        TextBox5.Text = "Factura CFDI recibida, folio: " + folio
        ' Creamos el cuerpo del mensaje
        TextBox4.Text = "Estimado cliente," + vbCrLf + vbCrLf + "Le enviamos la factura CFDI solicitada." + vbCrLf + vbCrLf + "Cierres el Cubano, agradece su preferencia."
        ' Email del cliente
        TextBox1.Text = executeQueryTextr("SELECT email FROM partners INNER JOIN facturas ON (partners.idCliente = facturas.idCliente) WHERE facturas.idFactura = " + idfactura)

        ' Mostramos en el list view los archivos adjunto
        ListView1.Items.Clear()
        ' Agregar datos del archivo xml
        Dim fichero As System.IO.FileInfo
        Dim Str(2) As String

        fichero = New System.IO.FileInfo(xml_sign)
        Str(0) = fichero.Name
        Str(1) = Math.Round(fichero.Length / 1024, 2).ToString + " kb"
        ListView1.Items.Add(New ListViewItem(Str))
        ' Agregar datos del archivo pdf
        fichero = New System.IO.FileInfo(xml_pdf)
        Str(0) = fichero.Name
        Str(1) = Math.Round(fichero.Length / 1024, 2).ToString + " kb"
        ListView1.Items.Add(New ListViewItem(Str))

    End Sub

    Private Sub cancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub send_email_Click(sender As System.Object, e As System.EventArgs) Handles btnSend.Click
        If Len(TextBox1.Text) = 0 Then
            MessageBox.Show("Debe existir por lo menos un destinatario para poder enviar el correo.", "Factusol CFDI", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        'Instanciar el hilo
        Dim oHilo As Thread
        Dim omail As New send_mail()
        'crear objeto y asignarlo al sub que queremos ejecutar
        oHilo = New Thread(AddressOf omail.send)
        ' Ponemos las variables en el objeto
        omail.dest = TextBox1.Text
        omail.copia = TextBox2.Text
        omail.copia_oculta = TextBox3.Text
        omail.subject = TextBox5.Text
        omail.body = TextBox4.Text
        omail.adj1 = xml_sign
        omail.adj2 = xml_pdf
        'lanzar el hilo
        oHilo.Start()
        ' Cerramos el form
        Me.Close()
    End Sub

    Public Class send_mail
        Public dest As String
        Public copia As String
        Public copia_oculta As String
        Public subject As String
        Public body As String
        Public adj1 As String
        Public adj2 As String

        Public Function send() As Boolean
            Dim smtp As New System.Net.Mail.SmtpClient
            Dim correo As New System.Net.Mail.MailMessage
            Dim adjunto1 As System.Net.Mail.Attachment
            Dim adjunto2 As System.Net.Mail.Attachment

            Try
                With smtp
                    .Port = My.Settings.port
                    .Host = My.Settings.server
                    .Credentials = New System.Net.NetworkCredential(My.Settings.user, My.Settings.pass)
                    .EnableSsl = My.Settings.ssl
                End With
                adjunto1 = New System.Net.Mail.Attachment(adj1)
                adjunto2 = New System.Net.Mail.Attachment(adj2)
                With correo
                    .From = New System.Net.Mail.MailAddress(My.Settings.user)
                    .To.Add(dest)
                    If copia <> "" Then
                        .CC.Add(copia)
                    End If
                    If copia_oculta <> "" Then
                        .Bcc.Add(copia_oculta)
                    End If
                    .Subject = subject
                    .Body = body
                    .IsBodyHtml = False
                    .Priority = System.Net.Mail.MailPriority.Normal
                    .Attachments.Add(adjunto1)
                    .Attachments.Add(adjunto2)
                End With

                smtp.Send(correo)
                'MessageBox.Show("Su mensaje de correo ha sido enviado.", _
                '"Correo enviado", _
                'MessageBoxButtons.OK)
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, _
                                "Error al enviar correo", _
                                MessageBoxButtons.OK)
            End Try
            send = True
        End Function
    End Class

    Private Sub ListView1_DoubleClick(sender As Object, e As System.EventArgs) Handles ListView1.DoubleClick
        If ListView1.SelectedItems(0).SubItems(0).Text.Contains("pdf") Then
            System.Diagnostics.Process.Start(xml_pdf)
        ElseIf ListView1.SelectedItems(0).SubItems(0).Text.Contains("xml") Then
            System.Diagnostics.Process.Start(xml_sign)
        End If
    End Sub
End Class