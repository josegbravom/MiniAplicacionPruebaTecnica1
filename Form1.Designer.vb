<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMiniAppPdfRest
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
        Me.bIniciarGeneracion = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'bIniciarGeneracion
        '
        Me.bIniciarGeneracion.Location = New System.Drawing.Point(175, 44)
        Me.bIniciarGeneracion.Name = "bIniciarGeneracion"
        Me.bIniciarGeneracion.Size = New System.Drawing.Size(136, 37)
        Me.bIniciarGeneracion.TabIndex = 0
        Me.bIniciarGeneracion.Text = "Iniciar Generación"
        Me.bIniciarGeneracion.UseVisualStyleBackColor = True
        '
        'frmMiniAppPdfRest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 126)
        Me.Controls.Add(Me.bIniciarGeneracion)
        Me.MaximizeBox = False
        Me.Name = "frmMiniAppPdfRest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Aplicación que genera PDF desde un REST"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bIniciarGeneracion As System.Windows.Forms.Button

End Class
