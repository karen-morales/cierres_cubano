<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class progress_form
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
        Me.lblText = New System.Windows.Forms.Label()
        Me.pg = New DevExpress.XtraEditors.ProgressBarControl()
        CType(Me.pg.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblText
        '
        Me.lblText.AutoSize = True
        Me.lblText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblText.Location = New System.Drawing.Point(14, 13)
        Me.lblText.Name = "lblText"
        Me.lblText.Size = New System.Drawing.Size(55, 16)
        Me.lblText.TabIndex = 0
        Me.lblText.Text = "Label1"
        '
        'pg
        '
        Me.pg.Location = New System.Drawing.Point(12, 44)
        Me.pg.Name = "pg"
        Me.pg.Properties.LookAndFeel.SkinName = "Seven Classic"
        Me.pg.Size = New System.Drawing.Size(382, 39)
        Me.pg.TabIndex = 1
        '
        'progress_form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(406, 101)
        Me.Controls.Add(Me.pg)
        Me.Controls.Add(Me.lblText)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "progress_form"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Progeso"
        CType(Me.pg.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblText As System.Windows.Forms.Label
    Friend WithEvents pg As DevExpress.XtraEditors.ProgressBarControl
End Class
