Public Class tienda_tree
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        tienda_form.ShowDialog()
    End Sub

    Private Sub gridFoliosCbb_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridFoliosCbb.CellDoubleClick
        tienda_form.ShowDialog()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    End Sub

    Private Sub tienda_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        fillDataGridByMySqlr(gridFoliosCbb, "SELECT * FROM tiendas ORDER BY tienda ASC;")
        gridFoliosCbb.AutoResizeColumns()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        fillDataGridByMySqlr(gridFoliosCbb, "SELECT * FROM tiendas ORDER BY tienda ASC;")
        gridFoliosCbb.AutoResizeColumns()
    End Sub
End Class