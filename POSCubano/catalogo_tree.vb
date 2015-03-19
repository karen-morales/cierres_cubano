Public Class catalogo_tree
    Public table As String

    Public Function updateGrid()
        fillDataGridByMySqlrDX(GridControl1, "SELECT id AS ID, nombre AS Nombre FROM " + table)
        GridView1.BestFitColumns()
    End Function

    Private Sub catalogo_tree_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        updateGrid()
    End Sub

    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click
        updateGrid()
    End Sub

    Private Sub btnModify_Click(sender As System.Object, e As System.EventArgs) Handles btnModify.Click
        If GridView1.SelectedRowsCount > 0 Then
            catalogo_form.Text = "Editar datos."
            catalogo_form.table = table
            catalogo_form.id = GridView1.GetFocusedRowCellValue("ID").ToString
            catalogo_form.nombre = GridView1.GetFocusedRowCellValue("Nombre").ToString
            catalogo_form.ShowDialog()
            updateGrid()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        catalogo_form.Text = "Crear registro."
        catalogo_form.table = table
        catalogo_form.ShowDialog()
        updateGrid()
    End Sub

    Private Sub GridControl1_DoubleClick(sender As Object, e As System.EventArgs) Handles GridControl1.DoubleClick
        If GridView1.SelectedRowsCount > 0 Then
            catalogo_form.Text = "Editar datos."
            catalogo_form.table = table
            catalogo_form.id = GridView1.GetFocusedRowCellValue("ID").ToString
            catalogo_form.nombre = GridView1.GetFocusedRowCellValue("Nombre").ToString
            catalogo_form.ShowDialog()
            updateGrid()
        End If
    End Sub
End Class