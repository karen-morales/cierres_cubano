Public Class expense_tree

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        UpdateGrid()
    End Sub

    Private Sub gastosForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UpdateGrid()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        expense_form.ShowDialog()
        UpdateGrid()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        For Each row As DataGridViewRow In Me.grid1.SelectedRows()
            executeQueryr("DELETE FROM gastos WHERE id = " + Me.grid1(0, row.Index).Value.ToString)
            UpdateGrid()
        Next
    End Sub

    Public Function UpdateGrid() As Integer
        Dim last_opening As String = executeQueryTextr("SELECT DATE_FORMAT(fecha_apertura, '%Y-%m-%d %H:%i:%s') FROM aperturas WHERE fecha_cierre IS NULL AND tienda = '" + idTienda + "' ORDER BY fecha_apertura DESC")
        fillDataGridByMySqlr(grid1, "SELECT id AS ID, idTienda AS Tienda, descripcion AS Descripcion, monto AS Monto, fecha AS Fecha FROM gastos WHERE idTienda = " + idTienda.ToString + " AND fecha > '" + last_opening + "'")
        TextBox1.Text = String.Format("{0:n2}", executeQueryr("SELECT sum(monto) FROM gastos WHERE idTienda = " + idTienda.ToString + " AND fecha > '" + last_opening + "'"))
        UpdateGrid = 1
        grid1.AutoResizeColumns()
    End Function

    Private Sub grid1_CellContentClick(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grid1.CellContentClick

    End Sub
End Class