<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class main_window
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(main_window))
        Me.DocumentManager1 = New DevExpress.XtraBars.Docking2010.DocumentManager(Me.components)
        Me.TabbedView1 = New DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.sale_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.box_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.VenderProductosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VentasRealizdasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AbrirCajaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CerrarDíaToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.expense_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.GastosDelDíaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.invoice_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdministrarClientesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.order_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerInventarioDeMiTiendaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerInventarioDeBodegaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.RealizarPedidoABodegaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DevolverProductosABodegaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pedidos_especialesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.warehouse_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.InventarioDeBodegaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnviarPedidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProcesarDevolucionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PedidosEspecialesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.generic_invoice_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.purchase_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.outputs_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.stock_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.InventarioDeLasTiendasToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.InventarioDeBodegaToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.report_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDeProductosVendidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDeFoliosDeVentasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDeFacturasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDeFinesDeDíaToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDepedidos_especialesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDePedidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReporteDeDevolucionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HistorialDeProductoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.config_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.FoliosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SincronizarClientesYVendedoresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.AdministrarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdmnistrarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdminsitrarFigurasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdministrarProductosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdmnistrarProveedoresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.change_password_menu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CambiarMiContraseñaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CamiarContraseñaDeOtroUsuarioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DocumentManager1
        '
        Me.DocumentManager1.MdiParent = Me
        Me.DocumentManager1.View = Me.TabbedView1
        Me.DocumentManager1.ViewCollection.AddRange(New DevExpress.XtraBars.Docking2010.Views.BaseView() {Me.TabbedView1})
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sale_menu, Me.box_menu, Me.expense_menu, Me.invoice_menu, Me.order_menu, Me.warehouse_menu, Me.generic_invoice_menu, Me.purchase_menu, Me.outputs_menu, Me.stock_menu, Me.report_menu, Me.config_menu, Me.change_password_menu})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.MenuStrip1.Size = New System.Drawing.Size(889, 24)
        Me.MenuStrip1.TabIndex = 4
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'sale_menu
        '
        Me.sale_menu.BackColor = System.Drawing.Color.White
        Me.sale_menu.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sale_menu.ForeColor = System.Drawing.Color.Black
        Me.sale_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_4_Shopping_cart_add
        Me.sale_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.sale_menu.Name = "sale_menu"
        Me.sale_menu.Size = New System.Drawing.Size(138, 36)
        Me.sale_menu.Text = "Realizar venta"
        Me.sale_menu.Visible = False
        '
        'box_menu
        '
        Me.box_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VenderProductosToolStripMenuItem, Me.VentasRealizdasToolStripMenuItem, Me.ToolStripSeparator1, Me.AbrirCajaToolStripMenuItem, Me.CerrarDíaToolStripMenuItem1})
        Me.box_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_3_Product_sales_report1
        Me.box_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.box_menu.Name = "box_menu"
        Me.box_menu.Size = New System.Drawing.Size(131, 36)
        Me.box_menu.Text = "Control de caja"
        Me.box_menu.Visible = False
        '
        'VenderProductosToolStripMenuItem
        '
        Me.VenderProductosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_4_Shopping_cart_add
        Me.VenderProductosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.VenderProductosToolStripMenuItem.Name = "VenderProductosToolStripMenuItem"
        Me.VenderProductosToolStripMenuItem.Size = New System.Drawing.Size(217, 38)
        Me.VenderProductosToolStripMenuItem.Text = "Vender productos"
        '
        'VentasRealizdasToolStripMenuItem
        '
        Me.VentasRealizdasToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Creative_Freedom_Free_Vibrant_Line_Graph
        Me.VentasRealizdasToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.VentasRealizdasToolStripMenuItem.Name = "VentasRealizdasToolStripMenuItem"
        Me.VentasRealizdasToolStripMenuItem.Size = New System.Drawing.Size(217, 38)
        Me.VentasRealizdasToolStripMenuItem.Text = "Analisis diario de Ventas"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(214, 6)
        '
        'AbrirCajaToolStripMenuItem
        '
        Me.AbrirCajaToolStripMenuItem.Name = "AbrirCajaToolStripMenuItem"
        Me.AbrirCajaToolStripMenuItem.Size = New System.Drawing.Size(217, 38)
        Me.AbrirCajaToolStripMenuItem.Text = "Abrir Caja"
        '
        'CerrarDíaToolStripMenuItem1
        '
        Me.CerrarDíaToolStripMenuItem1.Name = "CerrarDíaToolStripMenuItem1"
        Me.CerrarDíaToolStripMenuItem1.Size = New System.Drawing.Size(217, 38)
        Me.CerrarDíaToolStripMenuItem1.Text = "Cerrar Caja"
        '
        'expense_menu
        '
        Me.expense_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GastosDelDíaToolStripMenuItem})
        Me.expense_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Webiconset_E_Commerce_Wallet
        Me.expense_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.expense_menu.Name = "expense_menu"
        Me.expense_menu.Size = New System.Drawing.Size(86, 36)
        Me.expense_menu.Text = "Gastos"
        Me.expense_menu.Visible = False
        '
        'GastosDelDíaToolStripMenuItem
        '
        Me.GastosDelDíaToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Webiconset_E_Commerce_Wallet
        Me.GastosDelDíaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.GastosDelDíaToolStripMenuItem.Name = "GastosDelDíaToolStripMenuItem"
        Me.GastosDelDíaToolStripMenuItem.Size = New System.Drawing.Size(163, 38)
        Me.GastosDelDíaToolStripMenuItem.Text = "Gastos del día"
        '
        'invoice_menu
        '
        Me.invoice_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdministrarClientesToolStripMenuItem, Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem})
        Me.invoice_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Finance_Invoice
        Me.invoice_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.invoice_menu.Name = "invoice_menu"
        Me.invoice_menu.Size = New System.Drawing.Size(94, 36)
        Me.invoice_menu.Text = "Facturar"
        Me.invoice_menu.Visible = False
        '
        'AdministrarClientesToolStripMenuItem
        '
        Me.AdministrarClientesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_8_Users
        Me.AdministrarClientesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AdministrarClientesToolStripMenuItem.Name = "AdministrarClientesToolStripMenuItem"
        Me.AdministrarClientesToolStripMenuItem.Size = New System.Drawing.Size(270, 38)
        Me.AdministrarClientesToolStripMenuItem.Text = "Administrar Clientes"
        '
        'RealizarFacturasDeDíasAnterioesToolStripMenuItem
        '
        Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Finance_Invoice
        Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem.Name = "RealizarFacturasDeDíasAnterioesToolStripMenuItem"
        Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem.Size = New System.Drawing.Size(270, 38)
        Me.RealizarFacturasDeDíasAnterioesToolStripMenuItem.Text = "Realizar facturas de días anteriores"
        '
        'order_menu
        '
        Me.order_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VerInventarioDeMiTiendaToolStripMenuItem, Me.VerInventarioDeBodegaToolStripMenuItem, Me.ToolStripSeparator2, Me.RealizarPedidoABodegaToolStripMenuItem, Me.DevolverProductosABodegaToolStripMenuItem, Me.pedidos_especialesToolStripMenuItem})
        Me.order_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Fatcow_Farm_Fresh_Chart_stock
        Me.order_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.order_menu.Name = "order_menu"
        Me.order_menu.Size = New System.Drawing.Size(163, 36)
        Me.order_menu.Text = "Almacenes y pedidos"
        Me.order_menu.ToolTipText = "Inventarios, pedidos y devoluciones"
        Me.order_menu.Visible = False
        '
        'VerInventarioDeMiTiendaToolStripMenuItem
        '
        Me.VerInventarioDeMiTiendaToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Double_J_Design_Apple_Festival_App_stock
        Me.VerInventarioDeMiTiendaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.VerInventarioDeMiTiendaToolStripMenuItem.Name = "VerInventarioDeMiTiendaToolStripMenuItem"
        Me.VerInventarioDeMiTiendaToolStripMenuItem.Size = New System.Drawing.Size(232, 38)
        Me.VerInventarioDeMiTiendaToolStripMenuItem.Text = "Ver inventario de mi tienda"
        '
        'VerInventarioDeBodegaToolStripMenuItem
        '
        Me.VerInventarioDeBodegaToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Hamzasaleem_Iwork_Style_2_Numbers
        Me.VerInventarioDeBodegaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.VerInventarioDeBodegaToolStripMenuItem.Name = "VerInventarioDeBodegaToolStripMenuItem"
        Me.VerInventarioDeBodegaToolStripMenuItem.Size = New System.Drawing.Size(232, 38)
        Me.VerInventarioDeBodegaToolStripMenuItem.Text = "Ver inventario de Bodega"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(229, 6)
        '
        'RealizarPedidoABodegaToolStripMenuItem
        '
        Me.RealizarPedidoABodegaToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Office_Add_2
        Me.RealizarPedidoABodegaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.RealizarPedidoABodegaToolStripMenuItem.Name = "RealizarPedidoABodegaToolStripMenuItem"
        Me.RealizarPedidoABodegaToolStripMenuItem.Size = New System.Drawing.Size(232, 38)
        Me.RealizarPedidoABodegaToolStripMenuItem.Text = "Pedidos a Bodega"
        '
        'DevolverProductosABodegaToolStripMenuItem
        '
        Me.DevolverProductosABodegaToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Icojam_Onebit_Minus
        Me.DevolverProductosABodegaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.DevolverProductosABodegaToolStripMenuItem.Name = "DevolverProductosABodegaToolStripMenuItem"
        Me.DevolverProductosABodegaToolStripMenuItem.Size = New System.Drawing.Size(232, 38)
        Me.DevolverProductosABodegaToolStripMenuItem.Text = "Devoluciones a Bodega"
        '
        'pedidos_especialesToolStripMenuItem
        '
        Me.pedidos_especialesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Designbolts_Free_Valentine_Heart_Heart_Doodle
        Me.pedidos_especialesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.pedidos_especialesToolStripMenuItem.Name = "pedidos_especialesToolStripMenuItem"
        Me.pedidos_especialesToolStripMenuItem.Size = New System.Drawing.Size(232, 38)
        Me.pedidos_especialesToolStripMenuItem.Text = "Pedidos especiales"
        '
        'warehouse_menu
        '
        Me.warehouse_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InventarioDeBodegaToolStripMenuItem, Me.EnviarPedidosToolStripMenuItem, Me.ProcesarDevolucionesToolStripMenuItem, Me.PedidosEspecialesToolStripMenuItem})
        Me.warehouse_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Icons_Land_Gis_Gps_Map_ContainerRed
        Me.warehouse_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.warehouse_menu.Name = "warehouse_menu"
        Me.warehouse_menu.Size = New System.Drawing.Size(98, 36)
        Me.warehouse_menu.Text = "Almacén"
        Me.warehouse_menu.Visible = False
        '
        'InventarioDeBodegaToolStripMenuItem
        '
        Me.InventarioDeBodegaToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Hamzasaleem_Iwork_Style_2_Numbers
        Me.InventarioDeBodegaToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.InventarioDeBodegaToolStripMenuItem.Name = "InventarioDeBodegaToolStripMenuItem"
        Me.InventarioDeBodegaToolStripMenuItem.Size = New System.Drawing.Size(208, 38)
        Me.InventarioDeBodegaToolStripMenuItem.Text = "Inventario de bodega"
        '
        'EnviarPedidosToolStripMenuItem
        '
        Me.EnviarPedidosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Petalart_Business_Box_download2
        Me.EnviarPedidosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.EnviarPedidosToolStripMenuItem.Name = "EnviarPedidosToolStripMenuItem"
        Me.EnviarPedidosToolStripMenuItem.Size = New System.Drawing.Size(208, 38)
        Me.EnviarPedidosToolStripMenuItem.Text = "Procesar pedidos"
        '
        'ProcesarDevolucionesToolStripMenuItem
        '
        Me.ProcesarDevolucionesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Petalart_Business_Box_download_red2
        Me.ProcesarDevolucionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ProcesarDevolucionesToolStripMenuItem.Name = "ProcesarDevolucionesToolStripMenuItem"
        Me.ProcesarDevolucionesToolStripMenuItem.Size = New System.Drawing.Size(208, 38)
        Me.ProcesarDevolucionesToolStripMenuItem.Text = "Procesar devoluciones"
        '
        'PedidosEspecialesToolStripMenuItem
        '
        Me.PedidosEspecialesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Designbolts_Free_Valentine_Heart_Heart_Doodle
        Me.PedidosEspecialesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.PedidosEspecialesToolStripMenuItem.Name = "PedidosEspecialesToolStripMenuItem"
        Me.PedidosEspecialesToolStripMenuItem.Size = New System.Drawing.Size(208, 38)
        Me.PedidosEspecialesToolStripMenuItem.Text = "Pedidos especiales"
        '
        'generic_invoice_menu
        '
        Me.generic_invoice_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Finance_Invoice
        Me.generic_invoice_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.generic_invoice_menu.Name = "generic_invoice_menu"
        Me.generic_invoice_menu.Size = New System.Drawing.Size(180, 36)
        Me.generic_invoice_menu.Text = "Generar factura genérica"
        Me.generic_invoice_menu.Visible = False
        '
        'purchase_menu
        '
        Me.purchase_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Ebiene_E_Commerce_Shopping_bags
        Me.purchase_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.purchase_menu.Name = "purchase_menu"
        Me.purchase_menu.Size = New System.Drawing.Size(99, 36)
        Me.purchase_menu.Text = "Compras"
        Me.purchase_menu.Visible = False
        '
        'outputs_menu
        '
        Me.outputs_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Icojam_Onebit_Minus
        Me.outputs_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.outputs_menu.Name = "outputs_menu"
        Me.outputs_menu.Size = New System.Drawing.Size(87, 36)
        Me.outputs_menu.Text = "Salidas"
        Me.outputs_menu.Visible = False
        '
        'stock_menu
        '
        Me.stock_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InventarioDeLasTiendasToolStripMenuItem1, Me.InventarioDeBodegaToolStripMenuItem1})
        Me.stock_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Fatcow_Farm_Fresh_Chart_curve_add
        Me.stock_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.stock_menu.Name = "stock_menu"
        Me.stock_menu.Size = New System.Drawing.Size(109, 36)
        Me.stock_menu.Text = "Inventarios"
        Me.stock_menu.Visible = False
        '
        'InventarioDeLasTiendasToolStripMenuItem1
        '
        Me.InventarioDeLasTiendasToolStripMenuItem1.Image = Global.CubanoAdmin.My.Resources.Resources.Creative_Freedom_Free_Vibrant_Line_Graph
        Me.InventarioDeLasTiendasToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.InventarioDeLasTiendasToolStripMenuItem1.Name = "InventarioDeLasTiendasToolStripMenuItem1"
        Me.InventarioDeLasTiendasToolStripMenuItem1.Size = New System.Drawing.Size(217, 38)
        Me.InventarioDeLasTiendasToolStripMenuItem1.Text = "Inventario de las tiendas"
        '
        'InventarioDeBodegaToolStripMenuItem1
        '
        Me.InventarioDeBodegaToolStripMenuItem1.Image = Global.CubanoAdmin.My.Resources.Resources.Hamzasaleem_Iwork_Style_2_Numbers
        Me.InventarioDeBodegaToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.InventarioDeBodegaToolStripMenuItem1.Name = "InventarioDeBodegaToolStripMenuItem1"
        Me.InventarioDeBodegaToolStripMenuItem1.Size = New System.Drawing.Size(217, 38)
        Me.InventarioDeBodegaToolStripMenuItem1.Text = "Inventario de bodega"
        '
        'report_menu
        '
        Me.report_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ReporteDeProductosVendidosToolStripMenuItem, Me.ReporteDeFoliosDeVentasToolStripMenuItem, Me.ReporteDeFacturasToolStripMenuItem, Me.ReporteDeFinesDeDíaToolStripMenuItem1, Me.ReporteDepedidos_especialesToolStripMenuItem, Me.ReporteDePedidosToolStripMenuItem, Me.ReporteDeDevolucionesToolStripMenuItem, Me.HistorialDeProductoToolStripMenuItem})
        Me.report_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Double_J_Design_Apple_Festival_App_stock
        Me.report_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.report_menu.Name = "report_menu"
        Me.report_menu.Size = New System.Drawing.Size(97, 36)
        Me.report_menu.Text = "Reportes"
        Me.report_menu.Visible = False
        '
        'ReporteDeProductosVendidosToolStripMenuItem
        '
        Me.ReporteDeProductosVendidosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_1
        Me.ReporteDeProductosVendidosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDeProductosVendidosToolStripMenuItem.Name = "ReporteDeProductosVendidosToolStripMenuItem"
        Me.ReporteDeProductosVendidosToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDeProductosVendidosToolStripMenuItem.Text = "Reporte de productos vendidos"
        '
        'ReporteDeFoliosDeVentasToolStripMenuItem
        '
        Me.ReporteDeFoliosDeVentasToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_2
        Me.ReporteDeFoliosDeVentasToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDeFoliosDeVentasToolStripMenuItem.Name = "ReporteDeFoliosDeVentasToolStripMenuItem"
        Me.ReporteDeFoliosDeVentasToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDeFoliosDeVentasToolStripMenuItem.Text = "Reporte de folios de ventas"
        '
        'ReporteDeFacturasToolStripMenuItem
        '
        Me.ReporteDeFacturasToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_3
        Me.ReporteDeFacturasToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDeFacturasToolStripMenuItem.Name = "ReporteDeFacturasToolStripMenuItem"
        Me.ReporteDeFacturasToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDeFacturasToolStripMenuItem.Text = "Reporte de facturas"
        '
        'ReporteDeFinesDeDíaToolStripMenuItem1
        '
        Me.ReporteDeFinesDeDíaToolStripMenuItem1.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_4
        Me.ReporteDeFinesDeDíaToolStripMenuItem1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDeFinesDeDíaToolStripMenuItem1.Name = "ReporteDeFinesDeDíaToolStripMenuItem1"
        Me.ReporteDeFinesDeDíaToolStripMenuItem1.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDeFinesDeDíaToolStripMenuItem1.Text = "Reporte de fines de día"
        '
        'ReporteDepedidos_especialesToolStripMenuItem
        '
        Me.ReporteDepedidos_especialesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_5
        Me.ReporteDepedidos_especialesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDepedidos_especialesToolStripMenuItem.Name = "ReporteDepedidos_especialesToolStripMenuItem"
        Me.ReporteDepedidos_especialesToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDepedidos_especialesToolStripMenuItem.Text = "Reporte de pedidos especiales"
        '
        'ReporteDePedidosToolStripMenuItem
        '
        Me.ReporteDePedidosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_6
        Me.ReporteDePedidosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDePedidosToolStripMenuItem.Name = "ReporteDePedidosToolStripMenuItem"
        Me.ReporteDePedidosToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDePedidosToolStripMenuItem.Text = "Reporte de pedidos"
        '
        'ReporteDeDevolucionesToolStripMenuItem
        '
        Me.ReporteDeDevolucionesToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_7
        Me.ReporteDeDevolucionesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ReporteDeDevolucionesToolStripMenuItem.Name = "ReporteDeDevolucionesToolStripMenuItem"
        Me.ReporteDeDevolucionesToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.ReporteDeDevolucionesToolStripMenuItem.Text = "Reporte de devoluciones"
        '
        'HistorialDeProductoToolStripMenuItem
        '
        Me.HistorialDeProductoToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Visualpharm_Icons8_Metro_Style_Numbers_8__1_
        Me.HistorialDeProductoToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.HistorialDeProductoToolStripMenuItem.Name = "HistorialDeProductoToolStripMenuItem"
        Me.HistorialDeProductoToolStripMenuItem.Size = New System.Drawing.Size(255, 38)
        Me.HistorialDeProductoToolStripMenuItem.Text = "Historial de producto"
        '
        'config_menu
        '
        Me.config_menu.BackColor = System.Drawing.SystemColors.Control
        Me.config_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator7, Me.FoliosToolStripMenuItem, Me.SincronizarClientesYVendedoresToolStripMenuItem, Me.ToolStripSeparator5, Me.ConfigurarEnvíoDeCorreosToolStripMenuItem, Me.ToolStripSeparator3, Me.AdministrarToolStripMenuItem, Me.AdmnistrarToolStripMenuItem, Me.AdminsitrarFigurasToolStripMenuItem, Me.AdministrarProductosToolStripMenuItem, Me.AdmnistrarProveedoresToolStripMenuItem})
        Me.config_menu.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.config_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Franksouza183_Fs_Categories_preferences_system
        Me.config_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.config_menu.Name = "config_menu"
        Me.config_menu.Size = New System.Drawing.Size(126, 36)
        Me.config_menu.Text = "Configuración"
        Me.config_menu.Visible = False
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(233, 6)
        '
        'FoliosToolStripMenuItem
        '
        Me.FoliosToolStripMenuItem.BackColor = System.Drawing.Color.White
        Me.FoliosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Webiconset_E_Commerce_Empty_shopping_cart
        Me.FoliosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.FoliosToolStripMenuItem.Name = "FoliosToolStripMenuItem"
        Me.FoliosToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.FoliosToolStripMenuItem.Text = "Configurar tiendas"
        '
        'SincronizarClientesYVendedoresToolStripMenuItem
        '
        Me.SincronizarClientesYVendedoresToolStripMenuItem.BackColor = System.Drawing.Color.White
        Me.SincronizarClientesYVendedoresToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_8_Users
        Me.SincronizarClientesYVendedoresToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.SincronizarClientesYVendedoresToolStripMenuItem.Name = "SincronizarClientesYVendedoresToolStripMenuItem"
        Me.SincronizarClientesYVendedoresToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.SincronizarClientesYVendedoresToolStripMenuItem.Text = "Administrar vendedores"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(233, 6)
        '
        'ConfigurarEnvíoDeCorreosToolStripMenuItem
        '
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem.BackColor = System.Drawing.Color.White
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Petalart_Business_Mail_envelope
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem.Name = "ConfigurarEnvíoDeCorreosToolStripMenuItem"
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.ConfigurarEnvíoDeCorreosToolStripMenuItem.Text = "Configurar envío de correos"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(233, 6)
        '
        'AdministrarToolStripMenuItem
        '
        Me.AdministrarToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_5_Catalog
        Me.AdministrarToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AdministrarToolStripMenuItem.Name = "AdministrarToolStripMenuItem"
        Me.AdministrarToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.AdministrarToolStripMenuItem.Text = "Administrar Categorías"
        '
        'AdmnistrarToolStripMenuItem
        '
        Me.AdmnistrarToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_5_Catalog
        Me.AdmnistrarToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AdmnistrarToolStripMenuItem.Name = "AdmnistrarToolStripMenuItem"
        Me.AdmnistrarToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.AdmnistrarToolStripMenuItem.Text = "Administrar Colores"
        '
        'AdminsitrarFigurasToolStripMenuItem
        '
        Me.AdminsitrarFigurasToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_5_Catalog
        Me.AdminsitrarFigurasToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AdminsitrarFigurasToolStripMenuItem.Name = "AdminsitrarFigurasToolStripMenuItem"
        Me.AdminsitrarFigurasToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.AdminsitrarFigurasToolStripMenuItem.Text = "Administrar Figuras"
        '
        'AdministrarProductosToolStripMenuItem
        '
        Me.AdministrarProductosToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_5_Catalog
        Me.AdministrarProductosToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AdministrarProductosToolStripMenuItem.Name = "AdministrarProductosToolStripMenuItem"
        Me.AdministrarProductosToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.AdministrarProductosToolStripMenuItem.Text = "Administrar Productos"
        '
        'AdmnistrarProveedoresToolStripMenuItem
        '
        Me.AdmnistrarProveedoresToolStripMenuItem.Image = Global.CubanoAdmin.My.Resources.Resources.Custom_Icon_Design_Pretty_Office_5_Catalog
        Me.AdmnistrarProveedoresToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.AdmnistrarProveedoresToolStripMenuItem.Name = "AdmnistrarProveedoresToolStripMenuItem"
        Me.AdmnistrarProveedoresToolStripMenuItem.Size = New System.Drawing.Size(236, 38)
        Me.AdmnistrarProveedoresToolStripMenuItem.Text = "Admnistrar Proveedores"
        '
        'change_password_menu
        '
        Me.change_password_menu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.change_password_menu.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        Me.change_password_menu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CambiarMiContraseñaToolStripMenuItem, Me.CamiarContraseñaDeOtroUsuarioToolStripMenuItem})
        Me.change_password_menu.Image = Global.CubanoAdmin.My.Resources.Resources.Aha_Soft_Security_Secrecy
        Me.change_password_menu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.change_password_menu.Name = "change_password_menu"
        Me.change_password_menu.Size = New System.Drawing.Size(183, 36)
        Me.change_password_menu.Text = "Reestablecer contraseñas"
        Me.change_password_menu.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.change_password_menu.Visible = False
        '
        'CambiarMiContraseñaToolStripMenuItem
        '
        Me.CambiarMiContraseñaToolStripMenuItem.Name = "CambiarMiContraseñaToolStripMenuItem"
        Me.CambiarMiContraseñaToolStripMenuItem.Size = New System.Drawing.Size(263, 22)
        Me.CambiarMiContraseñaToolStripMenuItem.Text = "Cambiar mi contraseña"
        '
        'CamiarContraseñaDeOtroUsuarioToolStripMenuItem
        '
        Me.CamiarContraseñaDeOtroUsuarioToolStripMenuItem.Name = "CamiarContraseñaDeOtroUsuarioToolStripMenuItem"
        Me.CamiarContraseñaDeOtroUsuarioToolStripMenuItem.Size = New System.Drawing.Size(263, 22)
        Me.CamiarContraseñaDeOtroUsuarioToolStripMenuItem.Text = "Cambiar contraseña de otro usuario"
        '
        'main_window
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(889, 482)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(900, 520)
        Me.Name = "main_window"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Punto de venta ""El Cubano"""
        CType(Me.DocumentManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TabbedView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DocumentManager1 As DevExpress.XtraBars.Docking2010.DocumentManager
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents box_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VenderProductosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TabbedView1 As DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView
    Friend WithEvents config_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FoliosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CerrarDíaToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VentasRealizdasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AbrirCajaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SincronizarClientesYVendedoresToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents expense_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GastosDelDíaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents invoice_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdministrarClientesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RealizarFacturasDeDíasAnterioesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents sale_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConfigurarEnvíoDeCorreosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents order_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RealizarPedidoABodegaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DevolverProductosABodegaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents warehouse_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents report_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDeProductosVendidosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDeFoliosDeVentasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDeFacturasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDeFinesDeDíaToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents generic_invoice_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents stock_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InventarioDeLasTiendasToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InventarioDeBodegaToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pedidos_especialesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDepedidos_especialesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InventarioDeBodegaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnviarPedidosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProcesarDevolucionesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents purchase_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PedidosEspecialesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerInventarioDeMiTiendaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VerInventarioDeBodegaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents change_password_menu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDePedidosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReporteDeDevolucionesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HistorialDeProductoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdministrarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdmnistrarToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdministrarProductosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AdmnistrarProveedoresToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AdminsitrarFigurasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CambiarMiContraseñaToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CamiarContraseñaDeOtroUsuarioToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents outputs_menu As System.Windows.Forms.ToolStripMenuItem

End Class
