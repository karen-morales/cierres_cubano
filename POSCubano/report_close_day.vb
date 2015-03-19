Public Class report_close_day

    Private Sub reporteFinesdeDia_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        fillComboByMySqlr(tiendas, "SELECT idTienda,tienda FROM tiendas;")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        fillDataGridByMySqlr(Me.DataGridView1, String.Format("SELECT ID, fecha_apertura AS 'Fecha de apertura', fecha_cierre AS 'Fecha de cierre',apertura AS 'Saldo inicial',efectivo AS 'Entrada Efectivo', deposito AS 'Entrada Deposito',extra_iva AS 'IVA Cobrado',gastos AS 'Salida Gastos',retira AS 'Retiro de caja',morralla AS 'Cambio en caja'  FROM aperturas WHERE tienda = '{0}' ORDER BY id DESC;", tiendas.SelectedValue))
        DataGridView1.AutoResizeColumns()
    End Sub
End Class