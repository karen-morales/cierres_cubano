Imports MySql.Data.MySqlClient
Imports DevExpress
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.Image
Imports System.IO
Imports tagcode.xml.CFDI
Imports tagcode.xml
Imports tagcode.xml.CFDI.Emisor
Imports tagcode.xml.CFDI.Receptor
'Imports System

Module mod_app
    ' Rol del usuario logeado
    Public rol As Integer = 0
    Public idUsuario As Integer = 0
    Public nameUsuario As String
    Public idTienda As String
    'Public admin As Boolean = False
    Public load_full_db As Boolean = True
    Public mayoreo As Integer
    Public currentSale As Integer
    Public facturaOtros As Boolean = False
    Public invoice_ok As Boolean = False

    ' Fuentes
    Private Bold10 As text.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)
    Private Normal10 As text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL)
    Private Bold8 As text.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL)
    Private Normal8 As text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)
    Private text_temp As pdf.PdfPCell

    'Obtiene el folio siguiente de las facturas
    Public Function getLastFolioFacturaR(tienda) As Integer
        Dim conn As New MySqlConnection
        Dim sql As MySqlCommand = New MySqlCommand
        ' Creamos la conexion a la db
        conn.ConnectionString = "server=" & dbhostr & ";" _
            & "port=" & dbportr & ";" _
            & "uid=" & dbuserr & ";" _
            & "pwd=" & dbpassr & ";" _
            & "database=" & dbnamer & ";"
        conn.Open()

        sql.Connection = conn
        sql.CommandText = "SELECT folio_factura FROM foliossat WHERE idTienda = '" + tienda.ToString + "';"
        Dim f As Integer = sql.ExecuteScalar

        conn.Close()
        getLastFolioFacturaR = f
    End Function

    Public Function is_it_opening()
        Dim res As Boolean = False
        If executeQueryr("SELECT id FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "'") > 0 Then
            res = True
        Else
            MsgBox("No existe una caja abierta por lo tanto no puede realizar esta acción")
        End If
        Return res
    End Function
    '
    Public Function getCFDGenGeneradaR() As Integer
        Dim temp As Integer = executeQueryr("SELECT factura_generica_generada FROM tiendas WHERE idTienda = '" + idTienda.ToString + "';")
        getCFDGenGeneradaR = temp
    End Function

    'Toma el folio SAT remoto
    Public Function getFolioSatr(tienda) As Integer
        Dim foliosat As Integer = executeQueryr(String.Format("SELECT folioSat FROM foliossat WHERE idTienda = '{0}';", tienda))
        getFolioSatr = foliosat
    End Function

    'Toma el folio de factura remoto
    Public Function getFolioFactr(tienda) As Integer
        Dim folio As Integer = executeQueryr(String.Format("SELECT folio_factura FROM foliossat WHERE idTienda = '{0}';", tienda))
        getFolioFactr = folio
    End Function

    ' Abre el dia
    Public Function abrirDia() As Integer
        My.Settings.server = executeQueryTextr("SELECT server FROM smtp")
        My.Settings.user = executeQueryTextr("SELECT user FROM smtp")
        My.Settings.pass = executeQueryTextr("SELECT pass FROM smtp")
        My.Settings.port = executeQueryTextr("SELECT port FROM smtp")
        Dim ssl As String = executeQueryTextr("SELECT secure_ssl FROM smtp")

        If ssl = 1 Then
            My.Settings.ssl = True
        Else
            My.Settings.ssl = False
        End If

        Dim saldo As String = executeQueryTextr(String.Format("SELECT round(saldo_apertura,2) FROM tiendas WHERE idTienda = {0};", idTienda))
        'Dim saldo As String = executeQueryTextr(String.Format("SELECT morralla FROM aperturas WHERE tienda = {0};", idTienda))
        executeQueryr(String.Format("INSERT INTO aperturas (id,fecha_apertura,tienda,apertura) VALUES('',now(),'{0}','{1}');", idTienda, saldo))
        Return 1
    End Function

    'Cierra el dia de ventas
    Public Function cerrarDia(ByVal saldo_cierre As Double) As Boolean
        Dim id_apertura As Integer = executeQueryr("SELECT id FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "'")
        If id_apertura > 0 Then
            executeQueryr("UPDATE aperturas SET fecha_cierre=NOW(),efectivo='" + close_day_wizard.TextBox1.Text + "',deposito='" + close_day_wizard.TextBox4.Text + "',gastos='" + close_day_wizard.TextBox5.Text + "',retira='" + close_day_wizard.TextBox7.Text + "',morralla='" + Str(CDbl(close_day_wizard.TextBox6.Text) - CDbl(close_day_wizard.TextBox7.Text)) + "',extra_iva='" + close_day_wizard.TextBox8.Text + "' WHERE id = " + Str(id_apertura))
            executeQueryr("UPDATE tiendas SET saldo_apertura = '" + Str(CDbl(close_day_wizard.TextBox6.Text) - CDbl(close_day_wizard.TextBox7.Text)) + "' WHERE idTienda = '" + idTienda + "'")

            Return True
        Else
            MsgBox("Ocurrió un error al intentar cerrar el día, inténtelo nuevamente, si el problema persiste consulte a su adminsitrador")
        End If
        Return False
    End Function

    'Generar FACTURA
    Public Function generarFactura(ByVal idFactura As String, Optional ByVal generic_invoice As Integer = 0, Optional ByVal preview As Boolean = True) As String
        Dim oDoc As New iTextSharp.text.Document(PageSize.LETTER, 40, 20, 0, 0)
        Dim pdfw As iTextSharp.text.pdf.PdfWriter
        Dim fuente As iTextSharp.text.pdf.BaseFont
        Dim folio As String

        folio = executeQueryr(String.Format("SELECT folio FROM facturas WHERE idFactura = {0} ;", idFactura))

        Dim logo, logo2 As iTextSharp.text.Image
        Dim tabladatos As New PdfPTable(5)

        Dim tbl_header_multi As New PdfPTable(3)
        Dim tbl_footer(2) As PdfPTable
        Dim tbl_address, tbl_header_single, tbl_sellos As New PdfPTable(2)
        Dim tbl_customer, tbl_cadena_original, tbl_pre_footer As New PdfPTable(1)
        Dim tablaventas As New PdfPTable(5)

        tbl_header_multi.SetWidthPercentage({90, 200, 90}, PageSize.HALFLETTER)
        tbl_header_single.SetWidthPercentage({120, 260}, PageSize.HALFLETTER)
        tbl_customer.SetWidthPercentage({380}, PageSize.HALFLETTER)
        tbl_cadena_original.SetWidthPercentage({380}, PageSize.HALFLETTER)
        tbl_pre_footer.SetWidthPercentage({380}, PageSize.HALFLETTER)
        tbl_sellos.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
        tbl_address.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
       
        'DATOS CLIENTE        
        Dim fecha As String
        Dim nombre As String = ""
        Dim direccion As String = ""
        Dim ciudad As String = ""
        Dim telefono As String = ""
        Dim rfc As String = ""
        Dim calle As String = ""
        Dim estado As String = ""
        Dim colonia As String = ""
        Dim cp As String = ""
        Dim numero As String = ""
        Dim metodopago As String = ""
        Dim aprobacion As String = ""
        Dim fechafolios As String = ""
        Dim importe As String = ""
        Dim notas As String = ""
        Dim cuenta As String = ""
        Dim datos As DataSet

        ' DATOS TIENDAS
        Dim nombrem As String = ""
        Dim rfcm As String = ""
        Dim curpm As String = ""
        Dim estadom As String = ""
        Dim ciudadm As String = ""
        Dim coloniam As String = ""
        Dim cpm As String = ""
        Dim callem As String = ""
        Dim telefonom As String = ""
        Dim nombres As String = ""
        Dim rfcs As String = ""
        Dim curps As String = ""
        Dim estados As String = ""
        Dim ciudads As String = ""
        Dim colonias As String = ""
        Dim cps As String = ""
        Dim calles As String = ""
        Dim telefonos As String = ""
        Dim prefijo As String = ""
        '
        Dim pathm As String = ""
        Dim paths As String = ""
        Dim pathcbb As String = ""
        Dim tiendam As Integer = 0
        Dim tiendas As Integer = 0
        Dim tienda_emisora As Integer = 0
        Dim idPadre As Integer = 0
        Dim idSucursal As Integer = 0
        Dim nombre_vendedor As String = ""
        Dim text_temp As pdf.PdfPCell

        Dim monto As Double = 0
        Dim expedido_matriz As Boolean = True

        Dim xml_sign As String
        Dim xml_unsign As String

        Dim testing As String
        Dim testing_cfdi As Boolean

        Dim leyenda As String = "Salida la mercancía no hacemos cambios ni devoluciones"

        'establecemos el numero de serie de la licencia del SDK para poder hacer uso de ella.
        'En caso de no asignara, el SDK funcionará con las restricciones de version DEMO,
        'las cuales permite hacer unicamente CFDI con el RFC Genérico XAXX010101000
        Dim lic As New LicenciasBuildCFDI
        Dim co As New Conceptos

        nombre_vendedor = executeQueryTextr(String.Format("SELECT b.nombre FROM facturas a INNER JOIN vendedores b ON (a.vendedor = b.id) WHERE idFactura = {0};", idFactura))
        tienda_emisora = executeQueryr(String.Format("SELECT idtienda FROM facturas WHERE idFactura = {0};", idFactura))
        lic.Licencia(executeQueryTextr(String.Format("SELECT lic FROM tiendas WHERE idTienda = {0};", tienda_emisora)))
        idPadre = executeQueryr(String.Format("SELECT padre FROM tiendas WHERE idTienda = {0};", tienda_emisora))
        testing = executeQueryTextr(String.Format("SELECT testing FROM tiendas WHERE idTienda = '{0}' LIMIT 1;", tienda_emisora)).ToString

        ' Verificamos si estamos en modo testing de facturación o no la tienda emisora
        If testing = "1" Then
            testing_cfdi = True
        Else
            testing_cfdi = False
        End If

        If idPadre = tienda_emisora Then
            expedido_matriz = True
            datos = fillDataSetByMySqlr("SELECT * FROM tiendas WHERE idTienda = " + tienda_emisora.ToString)
            For Each row As DataRow In datos.Tables(0).Rows
                nombrem = row("nombre").ToString
                rfcm = row("rfc").ToString
                estadom = row("estado").ToString
                ciudadm = row("ciudad").ToString
                coloniam = row("colonia").ToString
                curpm = row("curp").ToString
                cpm = row("cp").ToString
                callem = row("calle").ToString
                telefonom = row("telefono").ToString
                prefijo = row("prefijo").ToString
            Next

            idSucursal = executeQueryr(String.Format("SELECT idTienda FROM tiendas WHERE padre = {0} AND idTienda <> {0};", tienda_emisora))
            datos = fillDataSetByMySqlr("SELECT * FROM tiendas WHERE idTienda = " + idSucursal.ToString)

            For Each row As DataRow In datos.Tables(0).Rows
                nombres = row("nombre").ToString
                rfcs = row("rfc").ToString
                estados = row("estado").ToString
                ciudads = row("ciudad").ToString
                colonias = row("colonia").ToString
                curps = row("curp").ToString
                cps = row("cp").ToString
                calles = row("calle").ToString
                telefonos = row("telefono").ToString
                prefijo = row("prefijo").ToString
            Next
        Else
            expedido_matriz = False
            datos = fillDataSetByMySqlr("SELECT * FROM tiendas WHERE idTienda = " + idPadre.ToString)
            For Each row As DataRow In datos.Tables(0).Rows
                nombrem = row("nombre").ToString
                rfcm = row("rfc").ToString
                estadom = row("estado").ToString
                ciudadm = row("ciudad").ToString
                coloniam = row("colonia").ToString
                curpm = row("curp").ToString
                cpm = row("cp").ToString
                callem = row("calle").ToString
                telefonom = row("telefono").ToString
            Next
            datos = fillDataSetByMySqlr("SELECT * FROM tiendas WHERE idTienda = " + tienda_emisora.ToString)
            For Each row As DataRow In datos.Tables(0).Rows
                nombres = row("nombre").ToString
                rfcs = row("rfc").ToString
                estados = row("estado").ToString
                ciudads = row("ciudad").ToString
                colonias = row("colonia").ToString
                curps = row("curp").ToString
                cps = row("cp").ToString
                calles = row("calle").ToString
                telefonos = row("telefono").ToString
            Next
        End If

        'Establecemos datos del Emisor
        'esta variable con esta funcion nos dejará indicar el regimen fiscal de la empresa emisora
        Dim reg As New RegimenFiscal
        reg.AgregarRegimen("PERSONAS FISICAS CON ACTIVIDADES EMPRESARIALES Y PROFESIONALES")

        Dim df As DomicilioFiscal
        Dim NombreArchivo As String
        Dim thisRFC As String

        'con esta variable establecemos los datos del emisor
        If expedido_matriz Then
            df = New DomicilioFiscal(callem, ciudadm, estadom, "MEXICO", cpm, "", "", coloniam)
            thisRFC = rfcm
        Else
            df = New DomicilioFiscal(calles, ciudads, estados, "MEXICO", cps, "", "", colonias)
            thisRFC = rfcs
        End If

        Dim em As Emisor = New Emisor(thisRFC, reg, nombrem)

        NombreArchivo = System.IO.Path.GetTempPath + thisRFC + "_" + folio.ToString + ".pdf"
        Try
            pdfw = PdfWriter.GetInstance(oDoc, New FileStream(NombreArchivo, FileMode.Create, FileAccess.Write, FileShare.None))
        Catch
            Try
                NombreArchivo = System.IO.Path.GetTempFileName.Split(".")(0) + "_" + thisRFC + "_" + folio.ToString + ".pdf"
                pdfw = PdfWriter.GetInstance(oDoc, New FileStream(NombreArchivo, FileMode.Create, FileAccess.Write, FileShare.None))
            Catch ex As Exception
                MsgBox(ex.Message)
                Return ""
            End Try
        End Try

        'con esta funcion agregamos los datos fiscales del emisor
        em.EstablecerDomicilioFiscal(df)

        prefijo = executeQueryTextr(String.Format("SELECT prefijo FROM tiendas WHERE idTienda = {0};", tienda_emisora))

        If generic_invoice = 1 Then
            datos = fillDataSetByMySqlr(String.Format("SELECT b.*,a.folio,round(a.monto,2) AS monto,DATE_FORMAT(a.fecha, '%Y-%m-%d %H:%m:%s') AS fecha,c.description,a.notas,a.cuenta,a.idtienda FROM facturas a LEFT JOIN partners b ON (a.idCliente = b.idCliente) LEFT JOIN metodos_de_pago c ON (a.metodo = c.id)  WHERE a.idFactura = {0};", idFactura))
        Else
            datos = fillDataSetByMySqlr(String.Format("SELECT b.*,a.folio,round(a.monto,2) AS monto,DATE_FORMAT(a.fecha, '%Y-%m-%d %H:%m:%s') AS fecha,c.description,a.cuenta,a.idtienda FROM facturas a LEFT JOIN partners b ON (a.idCliente = b.idCliente) LEFT JOIN metodos_de_pago c ON (a.metodo = c.id)  WHERE a.idFactura = {0};", idFactura))
        End If

        For Each row As DataRow In datos.Tables(0).Rows
            nombre = row("nombre").ToString
            folio = row("folio").ToString
            importe = row("monto").ToString
            direccion = String.Format("{0} {1} {2} {3} {4} CP {5} ", row("calle"), row("numero"), row("colonia"), row("ciudad"), row("estado"), row("cp"))
            calle = row("calle").ToString
            numero = row("numero").ToString
            colonia = row("colonia").ToString
            estado = row("estado").ToString
            cp = row("cp").ToString
            telefono = row("telefono").ToString
            rfc = row("rfc").ToString
            fecha = row("fecha").ToString
            If generic_invoice = 1 Then
                notas = row("notas").ToString
                metodopago = "NO IDENTIFICADO"
            Else
                'If remote = 0 Then
                metodopago = row("description").ToString
                cuenta = row("cuenta").ToString
                'End If

                If Len(metodopago) = 0 Then
                    metodopago = "NO IDENTIFICADO"
                End If
            End If
            'Dim fechaFormateada As String = fecha.ToShortDateString
            'fecha = fechaFormateada
            monto = row("monto")
        Next

        'Establecemos datos del Receptor
        'esta funcion nos permite indicar la información obligatoria para el emisor
        Dim r As New Receptor(rfc, nombre)
        'con esta variable establecemos los datos del receptor
        Dim d As New Domicilio("MEXICO", calle, "", numero, colonia, , , , estado, cp)
        'con esta funcion agregamos los datos fiscales del receptor
        r.EstablecerDomicilio(d)

        'Establecemos datos del Comprobante
        'esta funcion nos da la oportunidad de asignar los valores del nodo de comprobante del CFDI

        Dim c As New Comprobante(Now, "PAGO EN UNA SOLA EXHIBICION", Comprobante.opTipoDeComprobante.ingreso, Math.Round(CDbl(monto) / 1.16, 2), Math.Round(CDbl(monto), 2), metodopago, "MEXICO", folio.ToString, prefijo, , , , "PESOS MEXICANOS", "1.0", cuenta)

        If expedido_matriz Then
            c = New Comprobante(Now, "PAGO EN UNA SOLA EXHIBICION", Comprobante.opTipoDeComprobante.ingreso, Math.Round(CDbl(monto) / 1.16, 2), Math.Round(CDbl(monto), 2), metodopago, estadom + ", MEXICO", folio.ToString, prefijo, , , , "PESOS MEXICANOS", "1.0", cuenta)
        Else
            c = New Comprobante(Now, "PAGO EN UNA SOLA EXHIBICION", Comprobante.opTipoDeComprobante.ingreso, Math.Round(CDbl(monto) / 1.16, 2), Math.Round(CDbl(monto), 2), metodopago, estados + ", MEXICO", folio.ToString, prefijo, , , , "PESOS MEXICANOS", "1.0", cuenta)
        End If

        'CONTENIDO
        '***********************************************************************************
        'Instanciamos el objeto para el tipo de letra.
        fuente = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont

        'LOGOS
        ' Primero identificamos si esta compañia es padre
        'Dim idPadre = executeQueryr(String.Format("SELECT padre FROM tiendas WHERE idTienda = {0};", tienda_emisora))
        If idPadre = tienda_emisora Then
            ' Es compañia padre
            tiendam = tienda_emisora
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", tienda_emisora))
            ' Buscamos si tiene sucursales
            tiendas = idSucursal
            ' Tomamos el logo de la sucursal
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idSucursal))
        Else
            ' Es compañia sucursal
            tiendas = tienda_emisora
            tiendam = idPadre
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idPadre))
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", tienda_emisora))
        End If

        Try
            logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + pathm) 'Dirreccion a la imagen que se hace referencia
            If Len(paths) > 0 Then
                logo2 = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + paths) 'Dirreccion a la imagen que se hace referencia
            End If
        Catch ex As Exception
            Try
                logo = iTextSharp.text.Image.GetInstance("c:\" + pathm) 'Dirreccion a la imagen que se hace referencia
                If Len(paths) > 0 Then
                    logo2 = iTextSharp.text.Image.GetInstance("c:\" + paths) 'Dirreccion a la imagen que se hace referencia
                End If
            Catch ex2 As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Error al leer las imágenes")
                generarFactura = ""
                Return False
            End Try
        End Try

        Dim x As Integer = 300
        Dim y As Integer = 730
        Dim folio_print As String = folio

        If testing_cfdi Then
            folio_print = folio + " - SIN VALIDEZ FISCAL"
        End If

        If tiendam > 0 And tiendas = 0 Then
            tbl_header_single.DefaultCell.Border = 0
            logo.ScaleAbsoluteHeight(100) 'Altura de la imagen
            tbl_header_single.AddCell(logo)
            text_temp = New pdf.PdfPCell(New Phrase(String.Format("FACTURA {1}{2}{0}www.cierreselcubano.com.mx{0}CIERRES,JARETAS,ELASTICOS,DESLIZADORES{0}LOS MEJORES PRECIOS DE MEXICO{0}{11} RFC:{8} CURP:{9}{0}{3} COL {4} {6} {5} CP: {10}{0}{7}{0} PERSONAS FISICAS CON ACTIVIDADES EMPRESARIALES Y PROFESIONALES", vbCrLf, prefijo, folio_print, callem, coloniam, estadom, ciudadm, telefonom, rfcm, curpm, cpm, nombrem), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL)))
            text_temp.Border = 0
            text_temp.HorizontalAlignment = ALIGN_LEFT
            text_temp.VerticalAlignment = ALIGN_MIDDLE
            tbl_header_single.AddCell(text_temp)
        Else
            ' Formato Sucursal
            ' ################## Cabecera #########################
            tbl_header_multi.DefaultCell.Border = 0
            ' Logo izqueirdo
            logo.ScaleAbsoluteWidth(50) 'Ancho de la imagen
            logo.ScaleAbsoluteHeight(50)
            'logo.ScaleAbsoluteWidth
            tbl_header_multi.AddCell(logo)
            ' Texto
            text_temp = New pdf.PdfPCell(New Phrase(String.Format("FACTURA {1}{2}{0}{0}www.cierreselcubano.com.mx{0}{0}CIERRES,JARETAS,ELASTICOS,DESLIZADORES{0}LOS MEJORES PRECIOS DE MEXICO{0}{3}{0}{0}RFC: {4} CURP: {5}{0} PERSONAS FISICAS CON ACTIVIDADES EMPRESARIALES Y PROFESIONALES", vbCrLf, prefijo, folio_print, nombrem, rfcm, curpm), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL)))
            text_temp.Border = 0
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_header_multi.AddCell(text_temp)
            ' Logo derecho
            logo2.ScaleAbsoluteWidth(50) 'Ancho de la imagen
            logo2.ScaleAbsoluteHeight(50) 'Altura de la imagen
            tbl_header_multi.AddCell(logo2)

            text_temp = New pdf.PdfPCell(New Phrase("MATRIZ: " + callem + "  COL. " + coloniam + " " + ciudadm + " " + estadom + " CP " + cpm + vbCrLf + telefonom, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_address.AddCell(text_temp)

            text_temp = New pdf.PdfPCell(New Phrase("SUCURSAL: " + calles + "  COL. " + colonias + " " + ciudads + " " + estados + " CP " + cps + vbCrLf + telefonos, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_address.AddCell(text_temp)
        End If

        tbl_customer.DefaultCell.Border = 0
        text_temp = New pdf.PdfPCell(New Phrase("" + fecha + vbCrLf + "NOMBRE: " + nombre + vbCrLf + "DIRECCION: " + direccion + vbCrLf + "RFC: " + rfc + " TELEFONO: " + telefono, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_LEFT
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_customer.AddCell(text_temp)

        'TABLA DE VENTAS
        Dim fuenteEncabezado As text.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)
        Dim fuenteDatos As text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)
        tablaventas.SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER) 'Ajusta el tamaño de cada columna            
        tablaventas.DefaultCell.Border = 0

        'ENCABEZADOS DE LA TABLA
        Dim piezas As New pdf.PdfPCell(New Phrase("PIEZAS", fuenteEncabezado))
        Dim um As New pdf.PdfPCell(New Phrase("UM", fuenteEncabezado))
        Dim articulo As New pdf.PdfPCell(New Phrase("ARTICULO", fuenteEncabezado))
        Dim precio As New pdf.PdfPCell(New Phrase("PRECIO", fuenteEncabezado))
        Dim importec As New pdf.PdfPCell(New Phrase("IMPORTE", fuenteEncabezado))

        piezas.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        piezas.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
        'piezas.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP

        um.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        um.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        articulo.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        articulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        precio.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        precio.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        importec.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        importec.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        'Encabezados
        tablaventas.AddCell(piezas)
        tablaventas.AddCell(um)
        tablaventas.AddCell(articulo)
        tablaventas.AddCell(precio)
        tablaventas.AddCell(importec)

        'Agregamos los productos
        Dim stotal As Double = 0
        Dim total As Double = 0
        Dim ds As DataSet = New DataSet

        If generic_invoice = 0 Then
            Dim tempistr As String
            tempistr = String.Format("SELECT CONCAT(descripcion, ' - ', nombre) AS producto, cantidad, (facturas_detalle.precio) as precio FROM facturas_detalle INNER JOIN producto_tienda ON (facturas_detalle.descripcion = producto_tienda.idproductoTienda) WHERE folio_factura = (SELECT folio FROM facturas WHERE idfactura = '{0}') AND idTienda = '{1}';", idFactura, tienda_emisora)
            ds = fillDataSetByMySqlr(tempistr)
            'TOTALES                      
            For Each row As DataRow In ds.Tables(0).Rows
                co.AgregarConcepto(CInt(row("cantidad").ToString), "PIEZA", row("producto").ToString, CDbl(row("precio")), CInt(row("cantidad").ToString) * CDbl(row("precio")))
                tablaventas.AddCell(New Paragraph(row("cantidad").ToString, FontFactory.GetFont("Arial", 8)))
                tablaventas.AddCell(New Paragraph("PZA.", FontFactory.GetFont("Arial", 8)))
                tablaventas.AddCell(New Paragraph(row("producto").ToString, FontFactory.GetFont("Arial", 8)))
                Dim p As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("precio"))), fuenteDatos))
                p.Border = 0
                p.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                tablaventas.AddCell(p)
                stotal = Math.Round(row("cantidad") * row("precio"), 2)
                Dim st As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", stotal), fuenteDatos))
                st.Border = 0
                st.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                tablaventas.AddCell(st)
                total = total + stotal
            Next
        Else
            tablaventas.AddCell(New Paragraph("1", FontFactory.GetFont("Arial", 8)))
            tablaventas.AddCell(New Paragraph("PZA", FontFactory.GetFont("Arial", 8)))
            tablaventas.AddCell(New Paragraph("ESTA FACTURA CON FOLIO " + prefijo + folio + " AMPARA LAS NOTAS DE REMISION CON FOLIOS " + notas + " QUE COMPRENDE DEL DIA " + fecha, FontFactory.GetFont("Arial", 8)))
            Dim p As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", (Math.Round(CDbl(importe) / 1.16, 2))), fuenteDatos))
            p.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            p.Border = 0
            tablaventas.AddCell(p)
            stotal = Math.Round(CDbl(importe / 1.16), 2)
            Dim st As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", stotal), fuenteDatos))
            st.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            st.Border = 0
            tablaventas.AddCell(st)
            total = total + stotal
            co.AgregarConcepto(1, "PIEZA", "ESTA FACTURA CON FOLIO " + prefijo + folio + " AMPARA LAS NOTAS DE REMISION CON FOLIOS " + notas + " QUE COMPRENDE DEL DIA " + fecha, Math.Round(CDbl(importe) / 1.16, 2), Math.Round(CDbl(importe) / 1.16, 2))
        End If

        '#########################################################################################################33
        ' FACTURA CFDI
        '#########################################################################################################33

        'Establecemos Impuestos
        'aquí declaramos un impuesto IVA trasladado con tasa de 16%
        Dim im As New Impuestos
        im.AgregarImpuestosTrasladados(Impuestos.opTraslado.IVA, 16, Math.Round(monto, 2) - Math.Round((monto / 1.16), 2))

        Dim cer_file As String = System.IO.Path.GetTempFileName + ".cer"
        Dim key_file As String = System.IO.Path.GetTempFileName + ".key"

        Dim binaryData() As Byte

        binaryData = Convert.FromBase64String(executeQueryTextr(String.Format("SELECT cer_file FROM tiendas WHERE idTienda = '{0}';", tienda_emisora)))

        Dim fs As New FileStream(cer_file, FileMode.Create)
        fs.Write(binaryData, 0, binaryData.Length)
        fs.Close()

        Dim binaryData2() As Byte

        binaryData2 = Convert.FromBase64String(executeQueryTextr(String.Format("SELECT key_file FROM tiendas WHERE idTienda = '{0}';", tienda_emisora)))

        Dim fs2 As New FileStream(key_file, FileMode.Create)
        fs2.Write(binaryData2, 0, binaryData2.Length)
        fs2.Close()

        Dim folio_fiscal_temp As String = ""
        ' Verificamos si ya se frimo esa factura
        folio_fiscal_temp = executeQueryTextr(String.Format("SELECT folio_fiscal FROM facturas WHERE idFactura = {0};", idFactura))

        ' Rutas donde se almaceran los xml
        xml_sign = System.IO.Path.GetTempPath + thisRFC + "_" + folio + "_sign.xml"
        xml_unsign = System.IO.Path.GetTempPath + thisRFC + "_" + folio + ".xml"

        ' Si la factura NO tiene folio_sat realizamos el proceso de timbrado en otro caso solo la imprimimos
        If Len(folio_fiscal_temp) = 0 Then
            ' Generamos el archivo cer y key de los almacenados en la DB
            'Creamos el XML con todos los datos llenados especificando certificado, llave privada, contraseña y ruta de destino
            Dim xml As New CFDI
            Dim pass As String
            pass = executeQueryTextr(String.Format("SELECT pass FROM tiendas WHERE idTienda = '{0}';", tienda_emisora))

            Try
                If xml.CrearXML(cer_file, key_file, pass, c, em, r, co, im, xml_unsign) = True Then
                    ' Se creo el XML sin firmar satisfactoriamente ahora se intenta firmar
                    'declaramos la variable de servicios para timbrar la factura en modo prueba
                    Dim s As New Servicios
                    If testing_cfdi Then
                        Try
                            If s.TimbrePrueba(xml_unsign, xml_sign, "altegra") = True Then
                                Dim resultados As CFDI.resCFDI = xml.ResultadosCFDI(xml_sign)
                                executeQueryr(String.Format("UPDATE facturas SET folio_fiscal = '{0}', xml_timbrado = '{1}' WHERE idFactura = {2}", resultados.UUID, My.Computer.FileSystem.ReadAllText(xml_sign), idFactura))
                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message, MsgBoxStyle.Information)
                            'Forzamos vaciamiento del buffer.
                            pdfw.Flush()
                            Return False
                        End Try
                    Else
                        Dim cuenta_nip As String
                        Dim pass_pac As String

                        cuenta_nip = executeQueryTextr(String.Format("SELECT cuenta_nip FROM tiendas WHERE idTienda = '{0}';", tienda_emisora))
                        pass_pac = executeQueryTextr(String.Format("SELECT pass_pac FROM tiendas WHERE idTienda = '{0}';", tienda_emisora))

                        Dim result_stamp As Boolean = False

                        Try
                            result_stamp = s.TimbreProduccion(xml_unsign, xml_sign, cuenta_nip, pass_pac)
                            If result_stamp Then
                                Dim resultados As CFDI.resCFDI = xml.ResultadosCFDI(xml_sign)
                                executeQueryr(String.Format("UPDATE facturas SET folio_fiscal = '{0}', xml_timbrado = '{1}' WHERE idFactura = {2}", resultados.UUID, My.Computer.FileSystem.ReadAllText(xml_sign), idFactura))
                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message, MsgBoxStyle.Information)
                            'Forzamos vaciamiento del buffer.
                            pdfw.Flush()
                            Return False
                        End Try
                    End If
                Else
                    MsgBox("Ocurrió un error al generar el XML para la factura")
                    Return False
                End If
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation)
                'Forzamos vaciamiento del buffer.
                pdfw.Flush()
                Return False
            End Try
        End If

        '#########################################################################################################
        ' END FACTURA CFDI
        '#########################################################################################################

        '#########################################################################################################
        ' FOOTER
        '#########################################################################################################
        tbl_footer(0) = New PdfPTable(3)
        tbl_footer(0).SetWidthPercentage({126.6, 126.6, 126.6}, PageSize.HALFLETTER)

        Dim xml2 As New CFDI

        Dim fs1 As FileStream = File.Create(xml_sign)
        Dim info2 As Byte() = New System.Text.UTF8Encoding(True).GetBytes(executeQueryTextr(String.Format("SELECT xml_timbrado FROM facturas WHERE idFactura = {0};", idFactura)))
        fs1.Write(info2, 0, info2.Length)
        fs1.Close()

        Dim res As CFDI.resCFDI = xml2.ResultadosCFDI(xml_sign)

        If testing_cfdi Then
            text_temp = New pdf.PdfPCell(New Phrase("IMPORTANTE: ESTA FACTURA NO TIENE VALIDEZ FISCAL POR QUE FUÉ REALIZADA EN UN SERVIDOR DE PRUEBAS CON CERTIFICADOS DE PRUEBAS, SI USTED RECBE ESTA FACTURA EXIJA OTRA QUE SI TENGA VALIDEZ FISCAL", FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
            text_temp.Border = 0
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.VerticalAlignment = ALIGN_MIDDLE
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#FE2E2E"))
            tbl_pre_footer.AddCell(text_temp)
        End If

        text_temp = New pdf.PdfPCell(New Phrase("CANTIDAD CON LETRA: " + Letras(Math.Round(monto, 2)), FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_pre_footer.AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("PAGO EN UNA SOLA EXHIBICIÓN - EFECTOS FISCALES AL PAGO" + vbCrLf + "Este documento es una representación impresa de un CFDI | CFDI, Comprobante Fiscal Digital por Internet", FontFactory.GetFont(FontFactory.HELVETICA, 6, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_pre_footer.AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("CERTIFICADO DEL SAT" + vbCrLf + res.noCertificadoSAT, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_footer(0).AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("FECHA DE TIMBRADO" + vbCrLf + res.FechaTimbrado, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_footer(0).AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("FOLIO FISCAL" + vbCrLf + res.UUID, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_footer(0).AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("CERTIFICADO DEL EMISOR" + vbCrLf + res.noCertificado, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_footer(0).AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("METODO DE PAGO" + vbCrLf + metodopago, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_footer(0).AddCell(text_temp)

        If Len(cuenta) = 0 Then
            cuenta = "NO IDENTIFICADO"
        End If
        text_temp = New pdf.PdfPCell(New Phrase("ULTIMOS 4 DIGITOS DE CUENTA" + vbCrLf + cuenta, FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_footer(0).AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("Sello digital del emisor" + vbCrLf + res.selloCFD, FontFactory.GetFont(FontFactory.HELVETICA, 5, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_sellos.AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("Sello digital SAT" + vbCrLf + res.selloSAT, FontFactory.GetFont(FontFactory.HELVETICA, 5, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_sellos.AddCell(text_temp)

        text_temp = New pdf.PdfPCell(New Phrase("Cadena original: " + res.CadenaOriginalCFDI, FontFactory.GetFont(FontFactory.HELVETICA, 5, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.VerticalAlignment = ALIGN_MIDDLE
        tbl_cadena_original.AddCell(text_temp)

        tbl_footer(1) = New PdfPTable(1)
        tbl_footer(1).SetWidthPercentage({300}, PageSize.HALFLETTER)
        text_temp = New pdf.PdfPCell(New Phrase(leyenda, FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        tbl_footer(1).AddCell(text_temp)

        '#########################################################################################################33
        ' END FOOTER
        '#########################################################################################################33

        tablaventas.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))

        'Agrgamos el subtotal
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("SUB-TOTAL", FontFactory.GetFont("Arial", 8)))
        Dim t As New pdf.PdfPCell(New Paragraph(String.Format("{0: $ #,###,###,##0.00}", Math.Round(total, 2)), fuenteDatos))
        t.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
        t.Border = 0
        tablaventas.AddCell(t)

        'Agrgamos el iva
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("IVA (16%)", FontFactory.GetFont("Arial", 8)))
        Dim iva As New pdf.PdfPCell(New Paragraph(String.Format("{0: $ #,###,###,##0.00}", Math.Round(total * 0.16, 2)), fuenteDatos))
        iva.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
        iva.Border = 0
        tablaventas.AddCell(iva)

        'Agrgamos el total
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas.AddCell(New Paragraph("TOTAL ", FontFactory.GetFont("Arial", 8)))
        Dim gt As New pdf.PdfPCell(New Paragraph(String.Format("{0: $ #,###,###,##0.00}", Math.Round(total * 1.16, 2)), fuenteDatos))
        gt.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
        gt.Border = 0
        tablaventas.AddCell(gt)

        'Apertura del documento.
        oDoc.Open()
        'Agregamos una pagina.
        oDoc.NewPage()

        oDoc.Add(New Paragraph(" "))
        oDoc.Add(New Paragraph(" "))

        If tiendam > 0 And tiendas = 0 Then
            oDoc.Add(tbl_header_single)
        Else
            oDoc.Add(tbl_header_multi)
            oDoc.Add(tbl_address)
        End If

        oDoc.Add(New Paragraph(" "))
        oDoc.Add(tbl_customer)
        oDoc.Add(New Paragraph(" "))
        oDoc.Add(tablaventas)
        oDoc.Add(New Paragraph(" "))
        oDoc.Add(tbl_pre_footer)
        oDoc.Add(tbl_footer(0))
        oDoc.Add(tbl_sellos)
        oDoc.Add(tbl_cadena_original)
        oDoc.Add(tbl_footer(1))
        '*************************************************************************************
        'Forzamos vaciamiento del buffer.
        pdfw.Flush()
        'Cerramos el documento.
        oDoc.Close()

        ' Abrimos el documento
        If preview Then
            System.Diagnostics.Process.Start(NombreArchivo)
        End If

        pdfw = Nothing
        oDoc = Nothing
        'End Try
        generarFactura = NombreArchivo
    End Function

    'GENERAR NOTA
    Public Function generarNota(ByVal idVenta As String, Optional ByVal print_fiscal As Boolean = False)
        Dim oDoc As New iTextSharp.text.Document(PageSize.LETTER, 40, 20, 0, 0)
        Dim pdfw As iTextSharp.text.pdf.PdfWriter
        Dim linea As PdfContentByte
        Dim rectangulo As PdfContentByte
        'Dim NombreArchivo As String = idVenta.ToString
        Dim folio_venta As String = idVenta
        'folio_venta = executeQueryTextr(String.Format("SELECT idVentaLocal FROM ventas WHERE idVenta = '{0}';", idVenta))
        Dim NombreArchivo = System.IO.Path.GetTempFileName + "_" + folio_venta.ToString + ".pdf"
        Dim logo, logo2 As iTextSharp.text.Image 'Declaracion de una imagen       
        Dim tablaventas(40) As PdfPTable 'declara la tabla con 5 Columnas
        Dim tbl_header_ventas As New PdfPTable(5)
        Dim tbl_header As New PdfPTable(3)
        Dim tbl_footer(2) As PdfPTable
        Dim tbl_direcciones, tbl_header_matriz As New PdfPTable(2)
        Dim prefijo As String = ""
        Dim text_temp As pdf.PdfPCell

        Dim leyenda As String = "Salida la mercancía no hacemos cambios ni devoluciones"

        'Try
        pdfw = PdfWriter.GetInstance(oDoc, New FileStream(NombreArchivo, _
        FileMode.Create, FileAccess.Write, FileShare.None))

        'Apertura del documento.
        oDoc.Open()
        linea = pdfw.DirectContent
        rectangulo = pdfw.DirectContent

        'Agregamos una pagina.
        oDoc.NewPage()

        'DATOS            
        Dim folio As String = ""
        Dim fecha As String = ""
        Dim importe As Double = 0
        Dim datos As DataSet

        ' DATOS TIENDAS
        Dim nombrem As String = ""
        Dim rfcm As String = ""
        Dim curpm As String = ""
        Dim estadom As String = ""
        Dim ciudadm As String = ""
        Dim coloniam As String = ""
        Dim cpm As String = ""
        Dim callem As String = ""
        Dim telefonom As String = ""
        Dim nombres As String = ""
        Dim rfcs As String = ""
        Dim curps As String = ""
        Dim estados As String = ""
        Dim ciudads As String = ""
        Dim colonias As String = ""
        Dim cps As String = ""
        Dim calles As String = ""
        Dim telefonos As String = ""
        Dim vendedor As String = ""
        Dim pathm As String = ""
        Dim paths As String = ""
        Dim tiendam As Integer = 0
        Dim tiendas As Integer = 0
        Dim tienda_emisora As Integer

        If Not print_fiscal Then
            folio = executeQueryr(String.Format("SELECT folio FROM ventas WHERE idVenta = '{0}';", idVenta))
        Else
            folio = executeQueryr(String.Format("SELECT foliosat FROM ventas WHERE idVenta = '{0}';", idVenta))
        End If

        tienda_emisora = executeQueryr(String.Format("SELECT idTienda FROM ventas WHERE idVenta = '{0}';", idVenta))
        importe = executeQueryr(String.Format("SELECT monto FROM ventas WHERE idVenta = '{0}';", idVenta))
        fecha = executeQueryTextr(String.Format("SELECT DATE_FORMAT(fecha, '%Y-%m-%d %H:%m:%s') FROM ventas WHERE idVenta = '{0}';", idVenta))
        vendedor = executeQueryTextr(String.Format("SELECT b.nombre FROM ventas a LEFT JOIN vendedores b ON (a.vendedor = b.id) WHERE idVenta = '{0}';", idVenta))

        '        datos = fillDataSetByMySqlr(String.Format("SELECT a.*,b.idproductoTienda,c.nombre,a.idTienda FROM ventas a LEFT JOIN vendedores c ON (c.id = a.vendedor) WHERE a.idVenta = '{0}';", idVenta))

        'For Each row As DataRow In datos.Tables(0).Rows
        'folio = row("foliosat").ToString
        'fecha = row("fecha").ToString
        'Dim fechaFormateada As String = fecha.ToShortDateString
        'fecha = fechaFormateada
        'importe = importe + (CDbl(row("precio").ToString)) * (CDbl(row("cantidad").ToString))
        'vendedor = row("nombre").ToString
        'Next

        'LOGOS
        ' Primero identificamos si esta compañia es padre
        Dim idPadre
        idPadre = executeQueryr(String.Format("SELECT padre FROM tiendas WHERE idTienda = {0};", tienda_emisora))

        If idPadre = tienda_emisora Then
            ' Es compañia padre
            tiendam = tienda_emisora
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", tienda_emisora))

            ' Buscamos si tiene sucursales
            Dim idSucursal
            idSucursal = executeQueryr(String.Format("SELECT idTienda FROM tiendas WHERE padre = {0} AND idTienda <> {0};", tienda_emisora))

            tiendas = idSucursal
            ' Tomamos el logo de la sucursal
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idSucursal))
        Else
            ' Es compañia sucursal
            tiendas = tienda_emisora
            tiendam = idPadre
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idPadre))
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", tienda_emisora))
        End If

        Try
            logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + pathm) 'Direccion a la imagen que se hace referencia
            If Len(paths) > 0 Then
                logo2 = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + paths) 'Direccion a la imagen que se hace referencia
            End If
        Catch ex As Exception
            Try
                logo = iTextSharp.text.Image.GetInstance("c:\" + pathm) 'Direccion a la imagen que se hace referencia
                If Len(paths) > 0 Then
                    logo2 = iTextSharp.text.Image.GetInstance("c:\" + paths) 'Direccion a la imagen que se hace referencia
                End If
            Catch ex2 As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Error al leer las imágenes")
                generarNota = False
                Exit Function
            End Try
        End Try

        datos = fillDataSetByMySqlr(String.Format("SELECT * FROM tiendas WHERE idTienda = {0}", tiendam))
        For Each row As DataRow In datos.Tables(0).Rows
            nombrem = row("nombre").ToString
            rfcm = row("rfc").ToString
            estadom = row("estado").ToString
            ciudadm = row("ciudad").ToString
            coloniam = row("colonia").ToString
            curpm = row("curp").ToString
            cpm = row("cp").ToString
            callem = row("calle").ToString
            telefonom = row("telefono").ToString
        Next

        datos = fillDataSetByMySqlr(String.Format("SELECT * FROM tiendas WHERE idTienda = {0}", tiendas))

        For Each row As DataRow In datos.Tables(0).Rows
            nombres = row("nombre").ToString
            rfcs = row("rfc").ToString
            estados = row("estado").ToString
            ciudads = row("ciudad").ToString
            colonias = row("colonia").ToString
            curps = row("curp").ToString
            cps = row("cp").ToString
            calles = row("calle").ToString
            telefonos = row("telefono").ToString
        Next

        prefijo = executeQueryTextr(String.Format("SELECT prefijo FROM tiendas WHERE idTienda = {0};", tienda_emisora))

        'CONTENIDO
        '***********************************************************************************
        'Instanciamos el objeto para el tipo de letra.
        'fuente = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont
        'cb.SetFontAndSize(fuente, 8) 'fuente definida en la linea anterior y tamaño

        Dim x As Integer = 300
        Dim y As Integer = 730
        'TABLA DE VENTAS
        Dim fuenteEncabezado As text.Font = Bold10
        Dim fuenteDatos As text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)

        tbl_header_ventas.SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER)

        'ENCABEZADOS DE LA TABLA
        Dim piezas As New pdf.PdfPCell(New Phrase("PIEZAS", fuenteEncabezado))
        Dim um As New pdf.PdfPCell(New Phrase("UM", fuenteEncabezado))
        Dim articulo As New pdf.PdfPCell(New Phrase("ARTICULO", fuenteEncabezado))
        Dim precio As New pdf.PdfPCell(New Phrase("PRECIO", fuenteEncabezado))
        Dim importec As New pdf.PdfPCell(New Phrase("IMPORTE", fuenteEncabezado))

        piezas.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        piezas.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
        'piezas.VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP

        um.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        um.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        articulo.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        articulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        precio.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        precio.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        importec.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        importec.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        'Encabezados
        tbl_header_ventas.AddCell(piezas)
        tbl_header_ventas.AddCell(um)
        tbl_header_ventas.AddCell(articulo)
        tbl_header_ventas.AddCell(precio)
        tbl_header_ventas.AddCell(importec)

        'Agregamos los productos
        Dim stotal As Double = 0
        Dim total As Double = 0
        Dim ds As DataSet = New DataSet

        ds = fillDataSetByMySqlr(String.Format("SELECT a.*,CONCAT(b.idproductoTienda, ' - ', d.nombre) AS producto,b.cantidad,b.precio,c.nombre,a.idTienda,a.monto FROM ventas a INNER JOIN ventas_detalle b ON (a.idVenta = b.idVenta AND b.idtienda = a.idtienda AND date(b.fecha) = (SELECT date(fecha) FROM ventas WHERE idVenta = {0})) LEFT JOIN vendedores c ON (c.id = a.vendedor) INNER JOIN producto_tienda d ON (d.idproductoTienda =  b.idproductoTienda) WHERE a.idVenta = '{0}';", idVenta, tienda_emisora))

        Dim contador_tabla, i, pagina As Integer

        contador_tabla = 0
        i = 0
        tablaventas(0) = New PdfPTable(5)
        tablaventas(0).SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER)
        'TOTALES         

        pagina = ds.Tables(0).Rows.Count / 19 - 1
        If ds.Tables(0).Rows.Count Mod 19 <> 0 Then
            pagina = ds.Tables(0).Rows.Count / 19 + 1
        End If

        For Each row As DataRow In ds.Tables(0).Rows
            tablaventas(i).DefaultCell.Border = 0
            If contador_tabla <= 16 Then
                tablaventas(i).AddCell(New Paragraph(row("cantidad").ToString, FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph("PZA.", FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(row("producto").ToString, FontFactory.GetFont("Arial", 8)))
                Dim p As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("precio"))), fuenteDatos))
                p.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                p.Border = 0
                tablaventas(i).AddCell(p)
                stotal = CDbl(row("cantidad")) * CDbl(row("precio"))
                Dim st As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("cantidad")) * CDbl(row("precio"))), fuenteDatos))
                st.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                st.Border = 0
                tablaventas(i).AddCell(st)
                total = CDbl(row("monto"))
                contador_tabla = contador_tabla + 1
            Else
                i = i + 1
                tablaventas(i) = New PdfPTable(5)
                tablaventas(i).SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER)
                tablaventas(i).DefaultCell.Border = 0

                tablaventas(i).AddCell(New Paragraph(row("cantidad").ToString, FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph("PZA.", FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(row("producto").ToString, FontFactory.GetFont("Arial", 8)))
                Dim paux As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("precio"))), fuenteDatos))
                paux.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                paux.Border = 0
                tablaventas(i).AddCell(paux)
                stotal = CDbl(row("cantidad")) * CDbl(row("precio"))
                Dim staux As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("cantidad")) * CDbl(row("precio"))), fuenteDatos))
                staux.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                staux.Border = 0
                tablaventas(i).AddCell(staux)
                total = total + stotal

                contador_tabla = 1
            End If
        Next

        While contador_tabla < 16
            tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            contador_tabla = contador_tabla + 1
        End While

        'Agrgamos el subtotal
        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        'tablaventas(i).AddCell(New Paragraph(String.Format("Página {0} de {1}", i + 1, pagina - 1), FontFactory.GetFont("Arial", 8)))
        tablaventas(i).AddCell(New Paragraph("TOTAL", FontFactory.GetFont("Arial", 8)))
        Dim tot As New pdf.PdfPCell(New Paragraph(String.Format("{0: $ #,###,###,##0.00}", Math.Round(total, 2)), fuenteDatos))
        tot.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
        tot.Border = 0
        tablaventas(i).AddCell(tot)

        Dim cadena As String
        ' Footer

        tbl_footer(0) = New PdfPTable(3)
        tbl_footer(0).SetWidthPercentage({80, 220, 80}, PageSize.HALFLETTER)
        '
        text_temp = New pdf.PdfPCell(New Phrase("" + fecha, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_footer(0).AddCell(text_temp)
        text_temp = New pdf.PdfPCell(New Phrase("IMPORTE CON LETRA: " + Letras(Math.Round(importe, 2)), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))

        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_footer(0).AddCell(text_temp)
        '
        text_temp = New pdf.PdfPCell(New Phrase("VENDEDOR: " + vendedor, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_footer(0).AddCell(text_temp)

        cadena = Len(Letras(importe)) + 17
        If cadena < 70 Then
            'tbl_footer.AddCell(New Paragraph(cadena, FontFactory.GetFont("Arial", 8)))
            tbl_footer(0).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            tbl_footer(0).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
        End If

        tbl_footer(1) = New PdfPTable(1)
        tbl_footer(1).SetWidthPercentage({300}, PageSize.HALFLETTER)
        text_temp = New pdf.PdfPCell(New Phrase(leyenda, FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        tbl_footer(1).AddCell(text_temp)

        oDoc.AddAuthor("Cierres el Cubano")
        oDoc.GetTop(50)
        oDoc.GetBottom(50)

        If tiendam > 0 And tiendas = 0 Then
            ' Formato matriz sin sucursal
            tbl_header_matriz.SetWidthPercentage({80, 300}, PageSize.HALFLETTER)
            tbl_header_matriz.DefaultCell.Border = 0
            logo.ScaleAbsoluteHeight(80) 'Altura de la imagen
            tbl_header_matriz.AddCell(logo)

            text_temp = New pdf.PdfPCell(New Phrase(String.Format("NOTA DE REMISIÓN{0} R.F.C: {7}{0} {1}{0} Folio: {9}{10}{0} {2}, COL. {3} {6} {5} CP: {4}{0}{11}", vbCrLf, nombrem, callem, coloniam, cpm, estadom, ciudadm, rfcm, curpm, prefijo, folio, telefonom), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
            'text_temp = New pdf.PdfPCell(New Phrase(String.Format("NOTA DE REMISIÓN{0}{1} - {2}, COL. {3} CP: {4} {5} {6} R.F.C: {7} C.U.R.P: {8}{0}TEL: {11} Folio: {9}{10}", vbCrLf, nombrem, callem, coloniam, cpm, estadom, ciudadm, rfcm, curpm, prefijo, folio, telefonom), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_header_matriz.AddCell(text_temp)
        Else
            ' Formato Sucursal
            ' ################## Cabecera #########################
            tbl_header.SetWidthPercentage({50, 280, 50}, PageSize.HALFLETTER)
            tbl_header.DefaultCell.Border = 0
            ' Logo izqueirdo
            logo.ScaleAbsoluteHeight(50)
            'logo.ScaleAbsoluteWidth
            tbl_header.AddCell(logo)
            ' Texto
            text_temp = New pdf.PdfPCell(New Phrase("NOTA DE REMISIÓN" + vbCrLf + nombrem + vbCrLf + "R.F.C: " + rfcm + "  C.U.R.P: " + curpm + vbCrLf + vbCrLf + "Folio: " + prefijo + folio, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_header.AddCell(text_temp)
            ' Logo derecho
            logo2.ScaleAbsoluteWidth(50) 'Ancho de la imagen
            logo2.ScaleAbsoluteHeight(50) 'Altura de la imagen
            tbl_header.AddCell(logo2)
            ' Se agrega la tabla al documento
            ' ################## Direcciones #########################
            tbl_direcciones.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
            tbl_direcciones.DefaultCell.Border = 0
            'Datos matriz
            text_temp = New pdf.PdfPCell(New Phrase("MATRIZ: " + callem + ", COL. " + coloniam + " " + ciudadm + " " + estadom + " CP: " + cpm + vbCrLf + " TEL." + telefonom, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_direcciones.AddCell(text_temp)
            ' Datos Sucursal
            text_temp = New pdf.PdfPCell(New Phrase("SUCURSAL: " + calles + ", COL. " + colonias + " " + ciudads + " " + estados + " CP: " + cps + vbCrLf + " TEL." + telefonos, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_direcciones.AddCell(text_temp)
            ' Se agrega la tabla al documento
        End If

        Dim j As Integer
        j = 0
        For j = 0 To i
            oDoc.NewPage()
            oDoc.Add(New Paragraph(" "))
            If tiendam > 0 And tiendas = 0 Then
                tbl_header_matriz.DeleteBodyRows()
                ' Formato matriz sin sucursal
                tbl_header_matriz.SetWidthPercentage({80, 300}, PageSize.HALFLETTER)
                tbl_header_matriz.DefaultCell.Border = 0
                logo.ScaleAbsoluteHeight(80) 'Altura de la imagen
                tbl_header_matriz.AddCell(logo)

                text_temp = New pdf.PdfPCell(New Phrase(String.Format("NOTA DE REMISIÓN{0} R.F.C: {7}{0} {1}{0} Folio: {9}{10}{0} {2}, COL. {3} {6} {5} CP: {4}{0}{11}", vbCrLf, nombrem, callem, coloniam, cpm, estadom, ciudadm, rfcm, curpm, prefijo, folio, telefonom), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.VerticalAlignment = ALIGN_CENTER
                tbl_header_matriz.AddCell(text_temp)
                oDoc.Add(tbl_header_matriz)
            Else
                tbl_header.DeleteBodyRows()
                ' Formato Sucursal
                ' ################## Cabecera #########################
                tbl_header.SetWidthPercentage({50, 280, 50}, PageSize.HALFLETTER)
                tbl_header.DefaultCell.Border = 0
                ' Logo izqueirdo
                logo.ScaleAbsoluteHeight(50)
                'logo.ScaleAbsoluteWidth
                tbl_header.AddCell(logo)
                ' Texto
                text_temp = New pdf.PdfPCell(New Phrase("NOTA DE REMISIÓN" + vbCrLf + nombrem + vbCrLf + "R.F.C: " + rfcm + "  C.U.R.P: " + curpm + vbCrLf + vbCrLf + "Folio: " + prefijo + folio, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.VerticalAlignment = ALIGN_CENTER
                tbl_header.AddCell(text_temp)
                ' Logo derecho
                logo2.ScaleAbsoluteWidth(50) 'Ancho de la imagen
                logo2.ScaleAbsoluteHeight(50) 'Altura de la imagen
                tbl_header.AddCell(logo2)
                ' Se agrega la tabla al documento
                ' ################## Direcciones #########################
                tbl_direcciones.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
                tbl_direcciones.DefaultCell.Border = 0
                'Datos matriz
                tbl_direcciones.DeleteBodyRows()

                text_temp = New pdf.PdfPCell(New Phrase("MATRIZ: " + callem + ", COL. " + coloniam + " " + ciudadm + " " + estadom + " CP: " + cpm + vbCrLf + " TEL:" + telefonom, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
                tbl_direcciones.AddCell(text_temp)
                ' Datos Sucursal
                text_temp = New pdf.PdfPCell(New Phrase("SUCURSAL: " + calles + ", COL. " + colonias + " " + ciudads + " " + estados + " CP: " + cps + vbCrLf + " TEL:" + telefonos, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
                tbl_direcciones.AddCell(text_temp)
                oDoc.Add(tbl_header)
                ' Se agrega la tabla al documento
            End If

            oDoc.Add(tbl_direcciones)
            oDoc.Add(tbl_header_ventas)
            oDoc.Add(tablaventas(j))

            If j = i Then
                oDoc.Add(tbl_footer(0))
                oDoc.Add(tbl_footer(1))
            End If

            oDoc.Add(New Paragraph(" "))

            If tiendam > 0 And tiendas = 0 Then
                ' Formato matriz sin sucursal
                tbl_header_matriz.DeleteBodyRows()

                tbl_header_matriz.SetWidthPercentage({80, 300}, PageSize.HALFLETTER)
                tbl_header_matriz.DefaultCell.Border = 0
                logo.ScaleAbsoluteHeight(80) 'Altura de la imagen
                tbl_header_matriz.AddCell(logo)

                text_temp = New pdf.PdfPCell(New Phrase(String.Format("NOTA DE REMISIÓN{0} R.F.C: {7}{0} {1}{0} Folio: {9}{10}{0} {2}, COL. {3} {6} {5} CP: {4}{0}{11}", vbCrLf, nombrem, callem, coloniam, cpm, estadom, ciudadm, rfcm, curpm, prefijo, folio, telefonom), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.VerticalAlignment = ALIGN_CENTER
                tbl_header_matriz.AddCell(text_temp)
                oDoc.Add(tbl_header_matriz)
            Else
                tbl_header.DeleteBodyRows()

                ' Formato Sucursal
                ' ################## Cabecera #########################
                tbl_header.SetWidthPercentage({50, 280, 50}, PageSize.HALFLETTER)
                tbl_header.DefaultCell.Border = 0
                ' Logo izqueirdo
                logo.ScaleAbsoluteHeight(50)
                'logo.ScaleAbsoluteWidth
                tbl_header.AddCell(logo)
                ' Texto
                text_temp = New pdf.PdfPCell(New Phrase("NOTA DE REMISIÓN" + vbCrLf + nombrem + vbCrLf + "R.F.C: " + rfcm + "   C.U.R.P: " + curpm + vbCrLf + vbCrLf + "Folio: " + prefijo + folio, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.VerticalAlignment = ALIGN_CENTER
                tbl_header.AddCell(text_temp)
                ' Logo derecho
                logo2.ScaleAbsoluteWidth(50) 'Ancho de la imagen
                logo2.ScaleAbsoluteHeight(50) 'Altura de la imagen
                tbl_header.AddCell(logo2)
                ' Se agrega la tabla al documento
                ' ################## Direcciones #########################
                tbl_direcciones.DeleteBodyRows()

                tbl_direcciones.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
                tbl_direcciones.DefaultCell.Border = 0
                'Datos matriz
                text_temp = New pdf.PdfPCell(New Phrase("MATRIZ: " + callem + ", COL. " + coloniam + " " + ciudadm + " " + estadom + " CP: " + cpm + vbCrLf + " TEL." + telefonom, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
                tbl_direcciones.AddCell(text_temp)
                ' Datos Sucursal
                text_temp = New pdf.PdfPCell(New Phrase("SUCURSAL: " + calles + ", COL. " + colonias + " " + ciudads + " " + estados + " CP: " + cps + vbCrLf + " TEL." + telefonos, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
                text_temp.HorizontalAlignment = ALIGN_CENTER
                text_temp.Border = 0
                text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
                tbl_direcciones.AddCell(text_temp)
                oDoc.Add(tbl_header)
                ' Se agrega la tabla al documento
            End If
            oDoc.Add(tbl_direcciones)
            oDoc.Add(tbl_header_ventas)
            oDoc.Add(tablaventas(j))

            If j = i Then
                oDoc.Add(tbl_footer(0))
                oDoc.Add(tbl_footer(1))
            End If

        Next j

        '*************************************************************************************
        'Fin del flujo de bytes.
        'cb.EndText()
        'Forzamos vaciamiento del buffer.
        pdfw.Flush()
        'Cerramos el documento.
        oDoc.Close()
        System.Diagnostics.Process.Start(NombreArchivo)
        'Catch ex As Exception
        'Si hubo una excepcion y el archivo existe …
        '   If File.Exists(NombreArchivo) Then
        'Cerramos el documento si esta abierto.
        'Y asi desbloqueamos el archivo para su eliminacion.
        'If oDoc.IsOpen Then oDoc.Close()
        '… lo eliminamos de disco.
        'File.Delete(NombreArchivo)
        'End If
        'Throw New Exception("Error al generar archivo PDF (" & ex.Message & ")")
        'Finally         
        pdfw = Nothing
        oDoc = Nothing
        'End Try
        generarNota = True
    End Function

    Public Function printNF(ByVal notas As String, ByVal id_tienda As String)
        'DATOS            
        Dim folio As String = ""
        Dim datos As DataSet

        ' DATOS TIENDAS
        Dim nombrem As String = ""
        Dim rfcm As String = ""
        Dim curpm As String = ""
        Dim estadom As String = ""
        Dim ciudadm As String = ""
        Dim coloniam As String = ""
        Dim cpm As String = ""
        Dim callem As String = ""
        Dim telefonom As String = ""
        Dim nombres As String = ""
        Dim rfcs As String = ""
        Dim curps As String = ""
        Dim estados As String = ""
        Dim ciudads As String = ""
        Dim colonias As String = ""
        Dim cps As String = ""
        Dim calles As String = ""
        Dim telefonos As String = ""
        Dim pathm As String = ""
        Dim paths As String = ""
        Dim tiendam As Integer = 0
        Dim tiendas As Integer = 0

        Dim leyenda As String = "Salida la mercancía no hacemos cambios ni devoluciones"

        Dim array_notas() As String

        'Separa cadena de notas y las coloca en una matriz
        array_notas = Split(notas, ",")

        Dim oDoc As New iTextSharp.text.Document(PageSize.LETTER, 40, 20, 0, 0)
        Dim pdfw As iTextSharp.text.pdf.PdfWriter
        Dim linea As PdfContentByte
        Dim rectangulo As PdfContentByte
        'Dim NombreArchivo As String = idVenta.ToString

        Dim NombreArchivo = System.IO.Path.GetTempFileName + ".pdf"
        Dim logo, logo2 As iTextSharp.text.Image 'Declaracion de una imagen       
        Dim tablaventas(40) As PdfPTable 'declara la tabla con 5 Columnas
        Dim tbl_header_ventas As New PdfPTable(5)
        Dim tbl_header As New PdfPTable(3)
        Dim tbl_footer(2) As PdfPTable
        Dim tbl_direcciones, tbl_header_matriz As New PdfPTable(2)
        Dim prefijo As String = ""
        Dim text_temp As pdf.PdfPCell


        'Try
        pdfw = PdfWriter.GetInstance(oDoc, New FileStream(NombreArchivo, _
        FileMode.Create, FileAccess.Write, FileShare.None))

        'Apertura del documento.
        oDoc.Open()
        linea = pdfw.DirectContent
        rectangulo = pdfw.DirectContent

        'Agregamos una pagina.
        oDoc.NewPage()

        'CONTENIDO
        '***********************************************************************************
        'Instanciamos el objeto para el tipo de letra.
        'fuente = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont
        'cb.SetFontAndSize(fuente, 8) 'fuente definida en la linea anterior y tamaño

        Dim x As Integer = 300
        Dim y As Integer = 730
        'TABLA DE VENTAS
        Dim fuenteEncabezado As text.Font = Bold10
        Dim fuenteDatos As text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)

        tbl_header_ventas.SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER)

        'ENCABEZADOS DE LA TABLA
        Dim piezas As New pdf.PdfPCell(New Phrase("PIEZAS", fuenteEncabezado))
        Dim um As New pdf.PdfPCell(New Phrase("UM", fuenteEncabezado))
        Dim articulo As New pdf.PdfPCell(New Phrase("ARTICULO", fuenteEncabezado))
        Dim precio As New pdf.PdfPCell(New Phrase("PRECIO", fuenteEncabezado))
        Dim importec As New pdf.PdfPCell(New Phrase("IMPORTE", fuenteEncabezado))

        piezas.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        piezas.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        um.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        um.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        articulo.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        articulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        precio.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        precio.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        importec.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        importec.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        'Encabezados
        tbl_header_ventas.AddCell(piezas)
        tbl_header_ventas.AddCell(um)
        tbl_header_ventas.AddCell(articulo)
        tbl_header_ventas.AddCell(precio)
        tbl_header_ventas.AddCell(importec)

        Dim con, tam, tam1 As Integer
        con = 0
        tam = array_notas.Count - 1


        Dim ids As DataSet

        ids = fillDataSetByMySqlr(String.Format("SELECT idVenta FROM ventas WHERE idTienda = " + id_tienda + " AND foliosat IN (" + notas + ");"))

        tam1 = ids.Tables(0).Rows.Count - 1
        Dim folios As String
        folios = ""
        For i As Integer = 0 To ids.Tables(0).Rows.Count - 2
            folios += ids.Tables(0).Rows(i).Item(0).ToString + ","
        Next
        folios += ids.Tables(0).Rows(tam1).Item(0).ToString


        'LOGOS
        ' Primero identificamos si esta compañia es padre
        Dim idPadre As Integer
        idPadre = executeQueryr(String.Format("SELECT padre FROM tiendas WHERE idTienda ={0};", id_tienda))

        If idPadre = id_tienda Then
            ' Es compañia padre
            tiendam = id_tienda
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda ={0};", id_tienda))

            ' Buscamos si tiene sucursales
            Dim idSucursal
            idSucursal = executeQueryr(String.Format("SELECT idTienda FROM tiendas WHERE padre = {0} AND idTienda <> {0};", id_tienda))

            tiendas = idSucursal
            ' Tomamos el logo de la sucursal
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idSucursal))

        Else
            ' Es compañia sucursal
            tiendas = id_tienda
            tiendam = idPadre
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idPadre))
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda ={0}", id_tienda))
        End If

        Try
            logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + pathm) 'Direccion a la imagen que se hace referencia
            If Len(paths) > 0 Then
                logo2 = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + paths) 'Direccion a la imagen que se hace referencia
            End If
        Catch ex As Exception
            Try
                logo = iTextSharp.text.Image.GetInstance("c:\" + pathm) 'Direccion a la imagen que se hace referencia
                If Len(paths) > 0 Then
                    logo2 = iTextSharp.text.Image.GetInstance("c:\" + paths) 'Direccion a la imagen que se hace referencia
                End If
            Catch ex2 As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Error al leer las imágenes")
                printNF = False
                Exit Function
            End Try
        End Try

        prefijo = executeQueryTextr(String.Format("SELECT prefijo FROM tiendas WHERE idTienda = {0};", id_tienda))


        datos = fillDataSetByMySqlr(String.Format("SELECT * FROM tiendas WHERE idTienda = {0}", tiendam))
        For Each row As DataRow In datos.Tables(0).Rows
            nombrem = row("nombre").ToString
            rfcm = row("rfc").ToString
            estadom = row("estado").ToString
            ciudadm = row("ciudad").ToString
            coloniam = row("colonia").ToString
            curpm = row("curp").ToString
            cpm = row("cp").ToString
            callem = row("calle").ToString
            telefonom = row("telefono").ToString
        Next

        datos = fillDataSetByMySqlr(String.Format("SELECT * FROM tiendas WHERE idTienda = {0}", tiendas))

        For Each row As DataRow In datos.Tables(0).Rows
            nombres = row("nombre").ToString
            rfcs = row("rfc").ToString
            estados = row("estado").ToString
            ciudads = row("ciudad").ToString
            colonias = row("colonia").ToString
            curps = row("curp").ToString
            cps = row("cp").ToString
            calles = row("calle").ToString
            telefonos = row("telefono").ToString
        Next

        Dim fecha, vendedor As DataSet
        'Dim ds As DataSet = New DataSet

        fecha = fillDataSetByMySqlr(String.Format("SELECT DATE_FORMAT(fecha, '%Y-%m-%d %H:%i:%s') FROM ventas WHERE idVenta IN (" + folios + ");"))
        vendedor = fillDataSetByMySqlr(String.Format("SELECT b.nombre FROM ventas a LEFT JOIN  vendedores b ON (a.vendedor = b.id) WHERE idVenta IN  (" + folios + ");"))

        'ds = fillDataSetByMySqlr(String.Format("SELECT a.*,CONCAT(b.idproductoTienda, ' - ', d.nombre) AS producto,b.cantidad,b.precio,c.nombre,a.idTienda,a.monto FROM ventas a INNER JOIN ventas_detalle b ON (a.idVenta = b.idVenta AND b.idtienda = a.idtienda AND date(b.fecha) = (SELECT date(fecha) FROM ventas WHERE idVenta IN (" + folios + "))) LEFT JOIN vendedores c ON (c.id = a.vendedor) INNER JOIN producto_tienda d ON (d.idproductoTienda =  b.idproductoTienda) WHERE a.idVenta IN (" + folios + ");", id_tienda))

        For ro As Integer = 0 To ids.Tables(0).Rows.Count - 1

            folio = array_notas(ro)

            'Agregamos los productos
            Dim stotal As Double = 0
            Dim total As Double = 0
            Dim ds As DataSet = New DataSet

            ds = fillDataSetByMySqlr(String.Format("SELECT a.*,CONCAT(b.idproductoTienda, ' - ', d.nombre) AS producto,b.cantidad,b.precio,c.nombre,a.idTienda,a.monto FROM ventas a INNER JOIN ventas_detalle b ON (a.idVenta = b.idVenta AND b.idtienda = a.idtienda AND date(b.fecha) = (SELECT date(fecha) FROM ventas WHERE idVenta = {0})) LEFT JOIN vendedores c ON (c.id = a.vendedor) INNER JOIN producto_tienda d ON (d.idproductoTienda =  b.idproductoTienda) WHERE a.idVenta = '{0}';", ids.Tables(0).Rows(ro).Item(0).ToString, id_tienda))

            Dim contador_tabla, i As Integer

            contador_tabla = 0
            i = 0
            tablaventas(0) = New PdfPTable(5)
            tablaventas(0).SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER)

            'TOTALES         
            tablaventas(i).DefaultCell.Border = 0

            For Each row As DataRow In ds.Tables(0).Rows
                tablaventas(i).DefaultCell.Border = 0
                If contador_tabla <= 16 Then
                    tablaventas(i).AddCell(New Paragraph(row("cantidad").ToString, FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("PZA.", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph(row("producto").ToString, FontFactory.GetFont("Arial", 8)))
                    Dim p As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("precio"))), fuenteDatos))
                    p.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                    p.Border = 0
                    tablaventas(i).AddCell(p)
                    stotal = CDbl(row("cantidad")) * CDbl(row("precio"))
                    Dim st As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("cantidad")) * CDbl(row("precio"))), fuenteDatos))
                    st.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                    st.Border = 0
                    tablaventas(i).AddCell(st)
                    total += stotal
                    contador_tabla = contador_tabla + 1
                Else
                    i = i + 1
                    tablaventas(i) = New PdfPTable(5)
                    tablaventas(i).SetWidthPercentage({30, 20, 230, 50, 50}, PageSize.HALFLETTER)
                    tablaventas(i).DefaultCell.Border = 0

                    tablaventas(i).AddCell(New Paragraph(row("cantidad").ToString, FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("PZA.", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph(row("producto").ToString, FontFactory.GetFont("Arial", 8)))
                    Dim paux As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("precio"))), fuenteDatos))
                    paux.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                    paux.Border = 0
                    tablaventas(i).AddCell(paux)
                    stotal = CDbl(row("cantidad")) * CDbl(row("precio"))
                    Dim staux As New pdf.PdfPCell(New Phrase(String.Format("{0: $ #,###,###,##0.00}", CDbl(row("cantidad")) * CDbl(row("precio"))), fuenteDatos))
                    staux.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
                    staux.Border = 0
                    tablaventas(i).AddCell(staux)
                    total += stotal

                    contador_tabla = 1

                End If
            Next

            While contador_tabla < 16
                tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
                contador_tabla = contador_tabla + 1
            End While

            'Agrgamos el subtotal
            tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
            tablaventas(i).AddCell(New Paragraph("TOTAL", FontFactory.GetFont("Arial", 8)))
            Dim tot As New pdf.PdfPCell(New Paragraph(String.Format("{0: $ #,###,###,##0.00}", Math.Round(total, 2)), fuenteDatos))
            tot.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT
            tot.Border = 0
            tablaventas(i).AddCell(tot)

            Dim cadena As String

            ' Footer
            tbl_footer(0) = New PdfPTable(3)

            tbl_footer(0).SetWidthPercentage({80, 220, 80}, PageSize.HALFLETTER)
            '
            text_temp = New pdf.PdfPCell(New Phrase("" + fecha.Tables(0).Rows(ro).Item(0).ToString, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_footer(0).AddCell(text_temp)
            text_temp = New pdf.PdfPCell(New Phrase("IMPORTE CON LETRA: " + Letras(Math.Round(total, 2)), FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))

            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_footer(0).AddCell(text_temp)
            '
            text_temp = New pdf.PdfPCell(New Phrase("VENDEDOR: " + vendedor.Tables(0).Rows(ro).Item(0).ToString, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_footer(0).AddCell(text_temp)

            cadena = Len(Letras(total)) + 17
            If cadena < 70 Then
                tbl_footer(0).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
                tbl_footer(0).AddCell(New Paragraph(" ", FontFactory.GetFont("Arial", 8)))
            End If

            tbl_footer(1) = New PdfPTable(1)
            tbl_footer(1).SetWidthPercentage({300}, PageSize.HALFLETTER)
            text_temp = New pdf.PdfPCell(New Phrase(leyenda, FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            tbl_footer(1).AddCell(text_temp)

            oDoc.AddAuthor("Cierres el Cubano")
            oDoc.GetTop(50)
            oDoc.GetBottom(50)


            Dim j As Integer
            j = 0
            For j = 0 To i
                oDoc.Add(New Paragraph(" "))
                If tiendam > 0 And tiendas = 0 Then
                    tbl_header_matriz.Rows.Clear()
                    tbl_header_matriz.DeleteBodyRows()
                    ' Formato matriz sin sucursal
                    tbl_header_matriz.SetWidthPercentage({80, 300}, PageSize.HALFLETTER)
                    tbl_header_matriz.DefaultCell.Border = 0
                    logo.ScaleAbsoluteHeight(80) 'Altura de la imagen
                    tbl_header_matriz.AddCell(logo)

                    text_temp = New pdf.PdfPCell(New Phrase(String.Format("NOTA DE REMISIÓN{0} R.F.C: {7}{0} {1}{0} Folio: {9}{10}{0} {2}, COL. {3} {6} {5} CP: {4}{0}{11}", vbCrLf, nombrem, callem, coloniam, cpm, estadom, ciudadm, rfcm, curpm, prefijo, folio, telefonom), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
                    text_temp.HorizontalAlignment = ALIGN_CENTER
                    text_temp.Border = 0
                    text_temp.VerticalAlignment = ALIGN_CENTER
                    tbl_header_matriz.AddCell(text_temp)
                    oDoc.Add(tbl_header_matriz)
                Else
                    tbl_header.DeleteBodyRows()
                    ' Formato Sucursal
                    ' ################## Cabecera #########################
                    tbl_header.SetWidthPercentage({50, 280, 50}, PageSize.HALFLETTER)
                    tbl_header.DefaultCell.Border = 0
                    ' Logo izqueirdo
                    logo.ScaleAbsoluteHeight(50)
                    'logo.ScaleAbsoluteWidth
                    tbl_header.AddCell(logo)
                    ' Texto
                    text_temp = New pdf.PdfPCell(New Phrase("NOTA DE REMISIÓN" + vbCrLf + nombrem + vbCrLf + "R.F.C: " + rfcm + "  C.U.R.P: " + curpm + vbCrLf + vbCrLf + "Folio: " + prefijo + folio, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
                    text_temp.HorizontalAlignment = ALIGN_CENTER
                    text_temp.Border = 0
                    text_temp.VerticalAlignment = ALIGN_CENTER
                    tbl_header.AddCell(text_temp)
                    ' Logo derecho
                    logo2.ScaleAbsoluteWidth(50) 'Ancho de la imagen
                    logo2.ScaleAbsoluteHeight(50) 'Altura de la imagen
                    tbl_header.AddCell(logo2)
                    ' Se agrega la tabla al documento
                    ' ################## Direcciones #########################
                    tbl_direcciones.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
                    tbl_direcciones.DefaultCell.Border = 0
                    'Datos matriz
                    tbl_direcciones.DeleteBodyRows()

                    text_temp = New pdf.PdfPCell(New Phrase("MATRIZ: " + callem + ", COL. " + coloniam + " " + ciudadm + " " + estadom + " CP: " + cpm + vbCrLf + " TEL:" + telefonom, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
                    text_temp.HorizontalAlignment = ALIGN_CENTER
                    text_temp.Border = 0
                    text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
                    tbl_direcciones.AddCell(text_temp)
                    ' Datos Sucursal
                    text_temp = New pdf.PdfPCell(New Phrase("SUCURSAL: " + calles + ", COL. " + colonias + " " + ciudads + " " + estados + " CP: " + cps + vbCrLf + " TEL:" + telefonos, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
                    text_temp.HorizontalAlignment = ALIGN_CENTER
                    text_temp.Border = 0
                    text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
                    tbl_direcciones.AddCell(text_temp)
                    oDoc.Add(tbl_header)
                    ' Se agrega la tabla al documento
                End If

                oDoc.Add(tbl_direcciones)
                oDoc.Add(tbl_header_ventas)
                oDoc.Add(tablaventas(j))

                If j = i Then
                    oDoc.Add(tbl_footer(0))
                    oDoc.Add(tbl_footer(1))
                End If

                con = con + 1
                If con = 2 Then
                    oDoc.NewPage()
                    con = 0
                End If

            Next j

            'Next nota
        Next ro
        '*************************************************************************************
        'Fin del flujo de bytes.
        'cb.EndText()
        'Forzamos vaciamiento del buffer.
        pdfw.Flush()
        'Cerramos el documento.
        oDoc.Close()
        System.Diagnostics.Process.Start(NombreArchivo)
        'Catch ex As Exception
        'Si hubo una excepcion y el archivo existe …
        '   If File.Exists(NombreArchivo) Then
        'Cerramos el documento si esta abierto.
        'Y asi desbloqueamos el archivo para su eliminacion.
        'If oDoc.IsOpen Then oDoc.Close()
        '… lo eliminamos de disco.
        'File.Delete(NombreArchivo)
        'End If
        'Throw New Exception("Error al generar archivo PDF (" & ex.Message & ")")
        'Finally         
        pdfw = Nothing
        oDoc = Nothing
        'End Try
        printNF = True
    End Function

    Public Function hojaSalidaBodega(ByVal grid As DevExpress.XtraGrid.Views.Grid.GridView, ByVal titulo As String, ByVal id_tienda As String, ByVal folio As String, ByVal fecha As DateTime)
        Dim datos As DataSet

        ' DATOS TIENDAS
        Dim nombrem As String = ""
        Dim rfcm As String = ""
        Dim curpm As String = ""
        Dim estadom As String = ""
        Dim ciudadm As String = ""
        Dim coloniam As String = ""
        Dim cpm As String = ""
        Dim callem As String = ""
        Dim telefonom As String = ""
        Dim nombres As String = ""
        Dim rfcs As String = ""
        Dim curps As String = ""
        Dim estados As String = ""
        Dim ciudads As String = ""
        Dim colonias As String = ""
        Dim cps As String = ""
        Dim calles As String = ""
        Dim telefonos As String = ""
        Dim pathm As String = ""
        Dim paths As String = ""
        Dim tiendam As Integer = 0
        Dim tiendas As Integer = 0

        Dim oDoc As New iTextSharp.text.Document(PageSize.LETTER, 40, 20, 0, 0)
        Dim pdfw As iTextSharp.text.pdf.PdfWriter
        Dim linea As PdfContentByte
        Dim rectangulo As PdfContentByte
        'Dim NombreArchivo As String = idVenta.ToString

        Dim NombreArchivo = System.IO.Path.GetTempFileName + ".pdf"
        Dim logo, logo2 As iTextSharp.text.Image 'Declaracion de una imagen       
        Dim tablaventas(48) As PdfPTable 'declara la tabla con 5 Columnas
        Dim tbl_header_ventas As New PdfPTable(6)
        Dim tbl_header As New PdfPTable(3)
        Dim tbl_direcciones, tbl_header_matriz As New PdfPTable(2)
        Dim prefijo As String = ""
        Dim text_temp As pdf.PdfPCell

        'Try
        pdfw = PdfWriter.GetInstance(oDoc, New FileStream(NombreArchivo, _
        FileMode.Create, FileAccess.Write, FileShare.None))

        'Apertura del documento.
        oDoc.Open()
        linea = pdfw.DirectContent
        rectangulo = pdfw.DirectContent

        'Agregamos una pagina.
        oDoc.NewPage()

        'CONTENIDO
        '***********************************************************************************
        'Instanciamos el objeto para el tipo de letra.
        'fuente = FontFactory.GetFont(FontFactory.HELVETICA, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL).BaseFont
        'cb.SetFontAndSize(fuente, 8) 'fuente definida en la linea anterior y tamaño

        Dim x As Integer = 300
        Dim y As Integer = 730
        'TABLA DE VENTAS
        Dim fuenteEncabezado As text.Font = Bold10
        Dim fuenteDatos As text.Font = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL)

        tbl_header_ventas.SetWidthPercentage({40, 40, 110, 50, 40, 100}, PageSize.HALFLETTER)

        'ENCABEZADOS DE LA TABLA
        Dim codBodega As New pdf.PdfPCell(New Phrase("Código bodega", fuenteEncabezado))
        Dim codTienda As New pdf.PdfPCell(New Phrase("Código tienda", fuenteEncabezado))
        Dim articulo As New pdf.PdfPCell(New Phrase("Producto", fuenteEncabezado))
        Dim fechaPed As New pdf.PdfPCell(New Phrase("Fecha", fuenteEncabezado))
        Dim cantidad As New pdf.PdfPCell(New Phrase("Cantidad", fuenteEncabezado))
        Dim comentario As New pdf.PdfPCell(New Phrase("Comentario", fuenteEncabezado))

        codBodega.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        codBodega.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        codTienda.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        codTienda.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        articulo.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        articulo.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        fechaPed.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        fechaPed.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        cantidad.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        cantidad.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        comentario.BackgroundColor = iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY
        comentario.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER

        'Encabezados
        tbl_header_ventas.AddCell(codBodega)
        tbl_header_ventas.AddCell(codTienda)
        tbl_header_ventas.AddCell(articulo)
        tbl_header_ventas.AddCell(fechaPed)
        tbl_header_ventas.AddCell(cantidad)
        tbl_header_ventas.AddCell(comentario)


        'LOGOS
        ' Primero identificamos si esta compañia es padre
        Dim idPadre As Integer
        idPadre = executeQueryr(String.Format("SELECT padre FROM tiendas WHERE idTienda ={0};", id_tienda))

        If idPadre = id_tienda Then
            ' Es compañia padre
            tiendam = id_tienda
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda ={0};", id_tienda))

            ' Buscamos si tiene sucursales
            Dim idSucursal
            idSucursal = executeQueryr(String.Format("SELECT idTienda FROM tiendas WHERE padre = {0} AND idTienda <> {0};", id_tienda))

            tiendas = idSucursal
            ' Tomamos el logo de la sucursal
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idSucursal))

        Else
            ' Es compañia sucursal
            tiendas = id_tienda
            tiendam = idPadre
            pathm = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda = {0};", idPadre))
            paths = executeQueryTextr(String.Format("SELECT logo FROM tiendas WHERE idTienda ={0}", id_tienda))
        End If

        Try
            logo = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + pathm) 'Direccion a la imagen que se hace referencia
            If Len(paths) > 0 Then
                logo2 = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\" + paths) 'Direccion a la imagen que se hace referencia
            End If
        Catch ex As Exception
            Try
                logo = iTextSharp.text.Image.GetInstance("c:\" + pathm) 'Direccion a la imagen que se hace referencia
                If Len(paths) > 0 Then
                    logo2 = iTextSharp.text.Image.GetInstance("c:\" + paths) 'Direccion a la imagen que se hace referencia
                End If
            Catch ex2 As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Error al leer las imágenes")

            End Try
        End Try

        prefijo = executeQueryTextr(String.Format("SELECT prefijo FROM tiendas WHERE idTienda = {0};", id_tienda))


        datos = fillDataSetByMySqlr(String.Format("SELECT * FROM tiendas WHERE idTienda = {0}", tiendam))
        For Each row As DataRow In datos.Tables(0).Rows
            nombrem = row("nombre").ToString
            rfcm = row("rfc").ToString
            estadom = row("estado").ToString
            ciudadm = row("ciudad").ToString
            coloniam = row("colonia").ToString
            curpm = row("curp").ToString
            cpm = row("cp").ToString
            callem = row("calle").ToString
            telefonom = row("telefono").ToString
        Next

        datos = fillDataSetByMySqlr(String.Format("SELECT * FROM tiendas WHERE idTienda = {0}", tiendas))

        For Each row As DataRow In datos.Tables(0).Rows
            nombres = row("nombre").ToString
            rfcs = row("rfc").ToString
            estados = row("estado").ToString
            ciudads = row("ciudad").ToString
            colonias = row("colonia").ToString
            curps = row("curp").ToString
            cps = row("cp").ToString
            calles = row("calle").ToString
            telefonos = row("telefono").ToString
        Next

        ' Footer
        Dim tbl_footer As New PdfPTable(3)
        tbl_footer.Rows.Clear()
        tbl_footer.SetWidthPercentage({80, 220, 80}, PageSize.HALFLETTER)
        '
        text_temp = New pdf.PdfPCell(New Phrase("" + fecha, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_footer.AddCell(text_temp)

        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_footer.AddCell(text_temp)
        '
        text_temp.HorizontalAlignment = ALIGN_CENTER
        text_temp.Border = 0
        text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
        tbl_footer.AddCell(text_temp)

        Dim nombreTienda = executeQueryTextr(String.Format("SELECT tienda FROM tiendas WHERE idTienda = {0};", id_tienda))

        oDoc.AddAuthor("Cierres el Cubano")
        oDoc.GetTop(50)
        oDoc.GetBottom(50)

        oDoc.Add(New Paragraph(" "))
        If tiendam > 0 And tiendas = 0 Then
            tbl_header_matriz.Rows.Clear()
            tbl_header_matriz.DeleteBodyRows()
            ' Formato matriz sin sucursal
            tbl_header_matriz.SetWidthPercentage({80, 300}, PageSize.HALFLETTER)
            tbl_header_matriz.DefaultCell.Border = 0
            logo.ScaleAbsoluteHeight(80) 'Altura de la imagen
            tbl_header_matriz.AddCell(logo)

            text_temp = New pdf.PdfPCell(New Phrase(String.Format(titulo.ToUpper + "{0}{0} R.F.C: {7}{0} {1}{0}" + "Para: {12}{0}{0} Folio: {9}{10}{0}{0} {2}, COL. {3} {6} {5} CP: {4}{0}{11}", vbCrLf, nombrem, callem, coloniam, cpm, estadom, ciudadm, rfcm, curpm, prefijo, folio, telefonom, nombreTienda.ToUpper), FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_header_matriz.AddCell(text_temp)
            tbl_header_matriz.AddCell("")
            oDoc.Add(tbl_header_matriz)
        Else
            tbl_header.DeleteBodyRows()
            ' Formato Sucursal
            ' ################## Cabecera #########################
            tbl_header.SetWidthPercentage({50, 280, 50}, PageSize.HALFLETTER)
            tbl_header.DefaultCell.Border = 0
            ' Logo izqueirdo
            logo.ScaleAbsoluteHeight(50)
            'logo.ScaleAbsoluteWidth
            tbl_header.AddCell(logo)
            ' Texto
            text_temp = New pdf.PdfPCell(New Phrase(titulo.ToUpper + vbCrLf + vbCrLf + nombrem + vbCrLf + "R.F.C: " + rfcm + "  C.U.R.P: " + curpm + vbCrLf + vbCrLf + "Para: " + nombreTienda.ToUpper + vbCrLf + vbCrLf + "Folio: " + prefijo + folio + vbCrLf + vbCrLf, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.VerticalAlignment = ALIGN_CENTER
            tbl_header.AddCell(text_temp)
            ' Logo derecho
            logo2.ScaleAbsoluteWidth(50) 'Ancho de la imagen
            logo2.ScaleAbsoluteHeight(50) 'Altura de la imagen
            tbl_header.AddCell(logo2)
            ' Se agrega la tabla al documento
            ' ################## Direcciones #########################
            tbl_direcciones.SetWidthPercentage({190, 190}, PageSize.HALFLETTER)
            tbl_direcciones.DefaultCell.Border = 0
            'Datos matriz
            tbl_direcciones.DeleteBodyRows()

            text_temp = New pdf.PdfPCell(New Phrase("MATRIZ: " + callem + ", COL. " + coloniam + " " + ciudadm + " " + estadom + " CP: " + cpm + vbCrLf + " TEL:" + telefonom, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))
            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_direcciones.AddCell(text_temp)
            ' Datos Sucursal
            text_temp = New pdf.PdfPCell(New Phrase("SUCURSAL: " + calles + ", COL. " + colonias + " " + ciudads + " " + estados + " CP: " + cps + vbCrLf + " TEL:" + telefonos + vbCrLf + vbCrLf, FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL)))

            text_temp.HorizontalAlignment = ALIGN_CENTER
            text_temp.Border = 0
            text_temp.BackgroundColor = New BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEEEEE"))
            tbl_direcciones.AddCell(text_temp)

            oDoc.Add(tbl_header)
            ' Se agrega la tabla al documento
        End If

        oDoc.Add(tbl_direcciones)
        oDoc.Add(tbl_header_ventas)

        tablaventas(0) = New PdfPTable(6)
        tablaventas(0).SetWidthPercentage({40, 40, 110, 50, 40, 100}, PageSize.HALFLETTER)
        Dim canTotal As Integer
        canTotal = 0
        'TOTALES         
        tablaventas(0).DefaultCell.Border = 0
        Dim contadorTabla As Integer = 0
        Dim i As Integer = 0
        For row As Integer = 0 To grid.RowCount - 1
            If contadorTabla <= 24 Then
                tablaventas(i).DefaultCell.Border = 0
                tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Codigo bodega").ToString(), FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Codigo tienda").ToString(), FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Producto").ToString(), FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Fecha").ToString(), FontFactory.GetFont("Arial", 8)))
                tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Cantidad").ToString(), FontFactory.GetFont("Arial", 8)))
                canTotal += grid.GetRowCellValue(row, "Cantidad")
                tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Comentario").ToString(), FontFactory.GetFont("Arial", 8)))
                contadorTabla = contadorTabla + 1

            Else
                If contadorTabla = 25 Then
                    i = i + 1
                    tablaventas(i) = New PdfPTable(6)
                    tablaventas(i).SetWidthPercentage({40, 40, 110, 50, 40, 100}, PageSize.HALFLETTER)
                    tablaventas(i).DefaultCell.Border = 0
                    tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))

                    tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Codigo bodega").ToString(), FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Codigo tienda").ToString(), FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Producto").ToString(), FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Fecha").ToString(), FontFactory.GetFont("Arial", 8)))
                    tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Cantidad").ToString(), FontFactory.GetFont("Arial", 8)))
                    canTotal += grid.GetRowCellValue(row, "Cantidad")
                    tablaventas(i).AddCell(New Paragraph(grid.GetRowCellValue(row, "Comentario").ToString(), FontFactory.GetFont("Arial", 8)))

                    contadorTabla = 0
                End If
            End If

        Next

        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))
        tablaventas(i).AddCell(New Paragraph("TOTAL: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL)))
        tablaventas(i).AddCell(New Paragraph(canTotal.ToString, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, iTextSharp.text.Font.NORMAL)))
        tablaventas(i).AddCell(New Paragraph("", FontFactory.GetFont("Arial", 8)))

        For j As Integer = 0 To i
            oDoc.Add(tablaventas(j))
            oDoc.NewPage()
            oDoc.Add(New Paragraph(" "))
        Next

        pdfw.Flush()
        oDoc.Close()
        System.Diagnostics.Process.Start(NombreArchivo)
        pdfw = Nothing
        oDoc = Nothing

        hojaSalidaBodega = True
    End Function
End Module
