Public Class product_tree

    Private Sub product_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        updateGrid()
    End Sub

    Private Sub btnMod_Click(sender As System.Object, e As System.EventArgs) Handles btnMod.Click
        product_form.Text = "Editar Producto"
        product_form.ShowDialog()
        updateGrid()
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridControl1.DoubleClick
        product_form.Text = "Editar Producto"
        product_form.ShowDialog()
        updateGrid()
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        product_form.Text = "Crear Producto"
        product_form.ShowDialog()
        updateGrid()
    End Sub

    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click
        updateGrid()
    End Sub

    Public Function updateGrid()
        fillDataGridByMySqlrDX(GridControl1, "SELECT idproductoTienda AS ID, a.nombre AS Nombre, precio AS Precio, costo AS Costo, medida AS Medida, b.nombre AS Categoria, b.id AS categoria_id FROM producto_tienda a INNER JOIN categorias b ON (a.categorias_id = b.id) WHERE activo = 1")
        GridView1.Columns("categoria_id").Visible = False
        GridView1.BestFitColumns()
    End Function
End Class