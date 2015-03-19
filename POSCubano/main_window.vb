Imports System.Threading

Public Class main_window
    Private Sub VenderProductosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VenderProductosToolStripMenuItem.Click
        If is_it_opening() Then
            pos_form.MdiParent = Me
            pos_form.Show()
        End If
    End Sub

    Private Sub GenerarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub FoliosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FoliosToolStripMenuItem.Click
        tienda_tree.MdiParent = Me
        tienda_tree.Show()
    End Sub

    Private Sub GenerarReporteDeVentasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub main_window_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
        loginForm.TextBox2.Text = ""
        loginForm.TextBox2.Focus()
        loginForm.Show()
    End Sub

    Private Sub main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Establemos los menues dependiendo del rol del usuario
        sale_menu.Visible = False
        box_menu.Visible = False
        expense_menu.Visible = False
        invoice_menu.Visible = False
        order_menu.Visible = False
        warehouse_menu.Visible = False
        purchase_menu.Visible = False
        outputs_menu.Visible = False
        config_menu.Visible = False
        generic_invoice_menu.Visible = False
        stock_menu.Visible = False
        report_menu.Visible = False
        purchase_menu.Visible = False
        change_password_menu.Visible = False

        If rol = 1 Then
            sale_menu.Visible = True
            box_menu.Visible = True
            expense_menu.Visible = True
            invoice_menu.Visible = True
            order_menu.Visible = True
            ' Titulo de la ventana
            Me.Text = "Sistema de administración 'El Cubano' v" + My.Application.Info.Version.ToString + " [Tienda " + idTienda.ToString + "]"
        ElseIf rol = 2 Then
            warehouse_menu.Visible = True
            purchase_menu.Visible = True
            outputs_menu.Visible = True
            ' Titulo de la ventana
            Me.Text = "Sistema de administración 'El Cubano' v" + My.Application.Info.Version.ToString + " [ALMACÉN]"
        ElseIf rol = 3 Then
            config_menu.Visible = True
            generic_invoice_menu.Visible = True
            stock_menu.Visible = True
            report_menu.Visible = True
            purchase_menu.Visible = True
            outputs_menu.Visible = True
            change_password_menu.Visible = True
            ' Titulo de la ventana
            Me.Text = "Sistema de administración 'El Cubano' v" + My.Application.Info.Version.ToString + " [ADMINISTRADOR]"
            ' 
        ElseIf rol = 4 Then
            generic_invoice_menu.Visible = True
            report_menu.Visible = True
            report_menu.DropDownItems.Item(0).Visible = False
            report_menu.DropDownItems.Item(1).Visible = False
            report_menu.DropDownItems.Item(3).Visible = False
            report_menu.DropDownItems.Item(4).Visible = False
            report_menu.DropDownItems.Item(5).Visible = False
            report_menu.DropDownItems.Item(6).Visible = False
            report_menu.DropDownItems.Item(7).Visible = False

            Me.Text = "Sistema de administración 'El Cubano' v" + My.Application.Info.Version.ToString + " [FACTURACIÓN]"
        Else
            MsgBox("Ocurrió un error al leer los permisos del usuario, consulte con su administrador del sistema")
        End If
        ' Leemos las piezas de mayoreo de esta tienda
    End Sub

    Private Sub CerrarDíaToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CerrarDíaToolStripMenuItem1.Click
        Dim id_apertura As Integer = executeQueryr("SELECT id FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "'")
        If id_apertura > 0 Then
            'If executeQueryr("SELECT id FROM aperturas;") > 0 Then
            close_day_wizard.ShowDialog()
        Else
            MsgBox("No existe una apertura de caja, ni existen ventas que se pueden enviar.")
        End If
    End Sub

    Private Sub VentasRealizdasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VentasRealizdasToolStripMenuItem.Click
        If is_it_opening() Then
            sales_tree.MdiParent = Me
            sales_tree.Show()
        End If
    End Sub

    Private Sub AbrirCajaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AbrirCajaToolStripMenuItem.Click
        If executeQueryTextr("SELECT id FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "'") = "" Then
            abrirDia()
        Else
            MsgBox("Ya existe una apertura de caja para esta tienda, no es necesario volver a abrirla.")
        End If
    End Sub

    Private Sub SincronizarClientesYVendedoresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SincronizarClientesYVendedoresToolStripMenuItem.Click
        salesman_tree.MdiParent = Me
        salesman_tree.Show()
    End Sub

    Private Sub GastosDelDíaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GastosDelDíaToolStripMenuItem.Click
        If is_it_opening() Then
            expense_tree.MdiParent = Me
            expense_tree.Show()
        End If
    End Sub

    Private Sub AdministrarClientesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdministrarClientesToolStripMenuItem.Click
        If is_it_opening() Then
            partner_tree.MdiParent = Me
            partner_tree.Text = "Administrar clientes"
            partner_tree.Show()
        End If
    End Sub

    Private Sub RealizarFacturasDeDíasAnterioesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RealizarFacturasDeDíasAnterioesToolStripMenuItem.Click
        If is_it_opening() Then
            invoice_ticket_wizard.ShowDialog()
        End If
    End Sub

    Private Sub RealizarVentaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sale_menu.Click
        If is_it_opening() Then
            pos_form.MdiParent = Me
            pos_form.Show()
        End If
    End Sub

    Private Sub ConfigurarEnvíoDeCorreosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigurarEnvíoDeCorreosToolStripMenuItem.Click
        mail_config.ShowDialog()
    End Sub

    Private Sub RealizarPedidoABodegaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RealizarPedidoABodegaToolStripMenuItem.Click
        If is_it_opening() Then
            Dim order As order_return_process_tree = New order_return_process_tree
            order.MdiParent = Me
            order.Text = "Pedidos a bodega"
            order.Show()
        End If
    End Sub

    Private Sub DevolverProductosABodegaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DevolverProductosABodegaToolStripMenuItem.Click
        If is_it_opening() Then
            Dim returns As order_return_process_tree = New order_return_process_tree
            returns.MdiParent = Me
            returns.Text = "Devoluciones a bodega"
            returns.Show()
        End If
    End Sub

    Private Sub ReporteDeProductosVendidosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDeProductosVendidosToolStripMenuItem.Click
        report_products.MdiParent = Me
        report_products.Show()
    End Sub

    Private Sub ReporteDeFoliosDeVentasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDeFoliosDeVentasToolStripMenuItem.Click
        report_sales.MdiParent = Me
        report_sales.Show()
    End Sub

    Private Sub ReporteDeFacturasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDeFacturasToolStripMenuItem.Click
        report_invoice.MdiParent = Me
        report_invoice.Show()
    End Sub

    Private Sub ReporteDeFinesDeDíaToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDeFinesDeDíaToolStripMenuItem1.Click
        report_close_day.MdiParent = Me
        report_close_day.Show()
    End Sub

    Private Sub GenerarFacturaGenéricaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles generic_invoice_menu.Click
        generic_invoice_wizard.MdiParent = Me
        generic_invoice_wizard.Show()
    End Sub

    Private Sub InventarioDeLasTiendasToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InventarioDeLasTiendasToolStripMenuItem1.Click
        inventory_shop_tree.MdiParent = Me
        inventory_shop_tree.Show()
    End Sub

    Private Sub InventarioDeBodegaToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InventarioDeBodegaToolStripMenuItem1.Click
        inventory_main_warehouse_tree.MdiParent = Me
        inventory_main_warehouse_tree.Show()
    End Sub

    Private Sub pedidos_especialesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pedidos_especialesToolStripMenuItem.Click
        order_special_tree.MdiParent = Me
        order_special_tree.Show()
    End Sub

    Private Sub InventarioDeBodegaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InventarioDeBodegaToolStripMenuItem.Click
        inventory_main_warehouse_tree.MdiParent = Me
        inventory_main_warehouse_tree.Show()
    End Sub

    Private Sub EnviarPedidosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnviarPedidosToolStripMenuItem.Click
        Dim order As order_return_process_tree = New order_return_process_tree
        order.MdiParent = Me
        order.Text = "Pedidos a bodega"
        order.Show()
    End Sub

    Private Sub ProcesarDevolucionesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcesarDevolucionesToolStripMenuItem.Click
        Dim returns As order_return_process_tree = New order_return_process_tree
        returns.MdiParent = Me
        returns.Text = "Devoluciones a bodega"
        returns.Show()
    End Sub

    Private Sub ComprasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles purchase_menu.Click
        purchase_tree.MdiParent = Me
        purchase_tree.Show()
    End Sub
    Private Sub SalidasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles outputs_menu.Click
        outputs_tree.MdiParent = Me
        outputs_tree.Show()
    End Sub

    Private Sub ReporteDepedidos_especialesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDepedidos_especialesToolStripMenuItem.Click
        order_special_tree.MdiParent = Me
        order_special_tree.Show()
    End Sub

    Private Sub PedidosEspecialesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PedidosEspecialesToolStripMenuItem.Click
        order_special_tree.MdiParent = Me
        order_special_tree.Show()
    End Sub

    Private Sub VerInventarioDeBodegaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerInventarioDeBodegaToolStripMenuItem.Click
        inventory_main_warehouse_tree.MdiParent = Me
        inventory_main_warehouse_tree.Show()
    End Sub

    Private Sub VerInventarioDeMiTiendaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerInventarioDeMiTiendaToolStripMenuItem.Click
        inventory_shop_tree.MdiParent = Me
        inventory_shop_tree.Show()
    End Sub

    Private Sub change_password_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles change_password_menu.Click
    End Sub

    Private Sub ReporteDePedidosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDePedidosToolStripMenuItem.Click
        Dim orders As order_return_tree = New order_return_tree
        orders.MdiParent = Me
        orders.Text = "Pedidos a bodega"
        orders.Show()
    End Sub

    Private Sub ReporteDeDevolucionesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReporteDeDevolucionesToolStripMenuItem.Click
        Dim returns As order_return_tree = New order_return_tree
        returns.MdiParent = Me
        returns.Text = "Devoluciones a bodega"
        returns.Show()
    End Sub

    Private Sub HistorialDeProductoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistorialDeProductoToolStripMenuItem.Click
        product_history_tree.MdiParent = Me
        product_history_tree.Show()
    End Sub

    Private Sub AdmnistrarProveedoresToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdmnistrarProveedoresToolStripMenuItem.Click
        partner_tree.MdiParent = Me
        partner_tree.Text = "Administrar proveedores"
        partner_tree.tipo = 1
        partner_tree.Show()
    End Sub

    Private Sub AdministrarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdministrarToolStripMenuItem.Click
        Dim frmCat As New catalogo_tree
        frmCat.MdiParent = Me
        frmCat.Text = "Administrar Categorías"
        frmCat.table = "categorias"
        frmCat.Show()
    End Sub

    Private Sub AdmnistrarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdmnistrarToolStripMenuItem.Click
        Dim frmColor As New catalogo_tree
        frmColor.MdiParent = Me
        frmColor.Text = "Administrar Colores"
        frmColor.table = "color"
        frmColor.Show()
    End Sub

    Private Sub AdministrarProductosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdministrarProductosToolStripMenuItem.Click
        product_tree.MdiParent = Me
        product_tree.Show()
    End Sub

    Private Sub AdminsitrarFigurasToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AdminsitrarFigurasToolStripMenuItem.Click
        Dim frmFigura As New catalogo_tree
        frmFigura.MdiParent = Me
        frmFigura.Text = "Administrar Figuras"
        frmFigura.table = "figura"
        frmFigura.Show()
    End Sub

    Private Sub CambiarMiContraseñaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CambiarMiContraseñaToolStripMenuItem.Click
        change_password_form.my_pass = True
        change_password_form.ShowDialog()
    End Sub

    Private Sub CamiarContraseñaDeOtroUsuarioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CamiarContraseñaDeOtroUsuarioToolStripMenuItem.Click
        change_password_form.my_pass = False
        change_password_form.ShowDialog()
    End Sub
End Class
