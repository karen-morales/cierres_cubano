<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mail_config
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mail_config))
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.checkboxSSL = New System.Windows.Forms.CheckBox()
        Me.txtPassCorreo = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtUsuarioCorreo = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtPuertoCorreo = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtServidorCorreo = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.checkboxSSL)
        Me.GroupBox4.Controls.Add(Me.txtPassCorreo)
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Controls.Add(Me.txtUsuarioCorreo)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Controls.Add(Me.txtPuertoCorreo)
        Me.GroupBox4.Controls.Add(Me.Label10)
        Me.GroupBox4.Controls.Add(Me.txtServidorCorreo)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(361, 110)
        Me.GroupBox4.TabIndex = 9
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Envió de correos"
        '
        'checkboxSSL
        '
        Me.checkboxSSL.AutoSize = True
        Me.checkboxSSL.Location = New System.Drawing.Point(187, 78)
        Me.checkboxSSL.Name = "checkboxSSL"
        Me.checkboxSSL.Size = New System.Drawing.Size(87, 17)
        Me.checkboxSSL.TabIndex = 12
        Me.checkboxSSL.Text = "Habilitar SSL"
        Me.checkboxSSL.UseVisualStyleBackColor = True
        '
        'txtPassCorreo
        '
        Me.txtPassCorreo.Location = New System.Drawing.Point(246, 51)
        Me.txtPassCorreo.Name = "txtPassCorreo"
        Me.txtPassCorreo.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassCorreo.Size = New System.Drawing.Size(102, 20)
        Me.txtPassCorreo.TabIndex = 11
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(184, 54)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(56, 13)
        Me.Label13.TabIndex = 10
        Me.Label13.Text = "Password:"
        '
        'txtUsuarioCorreo
        '
        Me.txtUsuarioCorreo.Location = New System.Drawing.Point(62, 51)
        Me.txtUsuarioCorreo.Name = "txtUsuarioCorreo"
        Me.txtUsuarioCorreo.Size = New System.Drawing.Size(110, 20)
        Me.txtUsuarioCorreo.TabIndex = 9
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(15, 54)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(46, 13)
        Me.Label12.TabIndex = 8
        Me.Label12.Text = "Usuario:"
        '
        'txtPuertoCorreo
        '
        Me.txtPuertoCorreo.Location = New System.Drawing.Point(62, 78)
        Me.txtPuertoCorreo.Name = "txtPuertoCorreo"
        Me.txtPuertoCorreo.Size = New System.Drawing.Size(110, 20)
        Me.txtPuertoCorreo.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(15, 81)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(41, 13)
        Me.Label10.TabIndex = 6
        Me.Label10.Text = "Puerto:"
        '
        'txtServidorCorreo
        '
        Me.txtServidorCorreo.Location = New System.Drawing.Point(142, 24)
        Me.txtServidorCorreo.Name = "txtServidorCorreo"
        Me.txtServidorCorreo.Size = New System.Drawing.Size(206, 20)
        Me.txtServidorCorreo.TabIndex = 4
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(15, 27)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(121, 13)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "Servidor correo saliente:"
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.IndianRed
        Me.Button5.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button5.ForeColor = System.Drawing.Color.White
        Me.Button5.Location = New System.Drawing.Point(162, 137)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(94, 36)
        Me.Button5.TabIndex = 11
        Me.Button5.Text = "Cancelar"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.OliveDrab
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(262, 137)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(111, 36)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Guardar"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'mail_config
        '
        Me.AcceptButton = Me.Button2
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Button5
        Me.ClientSize = New System.Drawing.Size(387, 188)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.GroupBox4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "mail_config"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Configurar correo saliente"
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents checkboxSSL As System.Windows.Forms.CheckBox
    Friend WithEvents txtPassCorreo As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents txtUsuarioCorreo As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtPuertoCorreo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtServidorCorreo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class
