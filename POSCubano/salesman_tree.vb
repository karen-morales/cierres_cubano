Public Class salesman_tree
    Public Function UpdateGrid() As Integer
        fillDataGridByMySqlr(grid1, "SELECT * FROM vendedores")
        UpdateGrid = 1
    End Function

    Private Sub sellersForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UpdateGrid()
    End Sub

    Private Sub btnAgregarCliente_Click(sender As System.Object, e As System.EventArgs) Handles btnAgregarCliente.Click
        salesman_form.Text = "Agregar vendedor"
        salesman_form.ShowDialog()
        UpdateGrid()
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        UpdateGrid()
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        salesman_form.Text = "Modificar vendedor"
        salesman_form.ShowDialog()
        UpdateGrid()
    End Sub

    Private Sub grid1_DoubleClick(sender As Object, e As System.EventArgs) Handles grid1.DoubleClick
        salesman_form.Text = "Modificar vendedor"
        salesman_form.ShowDialog()
        UpdateGrid()
    End Sub
End Class