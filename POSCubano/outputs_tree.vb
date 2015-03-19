Public Class outputs_tree

    Private Sub outputs_tree_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        updateGrid()

        If rol = 3 Then
            btnAdd.Show()
            GridView1.Columns("Total").Visible = True
        Else
            btnAdd.Hide()
            GridView1.Columns("Total").Visible = True
        End If
    End Sub

    Public Function updateGrid() As Integer
        fillDataGridByMySqlrDX(GridControl1, "SELECT a.idSalida AS ID, a.fecha_salida AS Fecha, a.total_productos AS Total, b.nombre AS Responsable FROM salidas_producto a INNER JOIN usuarios b ON (a.usuarios_idusuarios = b.idusuarios) ORDER BY idSalida DESC LIMIT 100")
        'GridView1.Columns.Item("proveedor_id").Visible = False
        updateGrid = 1
    End Function

    Private Sub btnDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click
        If GridView1.GetSelectedRows.Length > 0 Then
            outputs_form.Text = "Detalle de salidas"
            outputs_form.ShowDialog()
        End If
    End Sub

    Private Sub GridControl1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridControl1.DoubleClick
        If GridView1.GetSelectedRows.Length > 0 Then
            outputs_form.Text = "Detalle de salidas"
            outputs_form.ShowDialog()
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        outputs_form.Text = "Registrar salida"
        outputs_form.ShowDialog()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        updateGrid()
    End Sub
End Class