<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.txtReceipt1 = New System.Windows.Forms.TextBox()
        Me.lblReceipt1 = New System.Windows.Forms.Label()
        Me.btnCloseReceipt = New System.Windows.Forms.Button()
        Me.btnExportOutbound = New System.Windows.Forms.Button()
        Me.txtOrder1 = New System.Windows.Forms.TextBox()
        Me.lblOrder1 = New System.Windows.Forms.Label()
        Me.cbConsignee1 = New System.Windows.Forms.ComboBox()
        Me.lblConsignee1 = New System.Windows.Forms.Label()
        Me.txtWarehouse1 = New System.Windows.Forms.TextBox()
        Me.lblWarehouse1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnNext2 = New System.Windows.Forms.Button()
        Me.lbWarehouse2 = New System.Windows.Forms.ListBox()
        Me.lblWarehouse2 = New System.Windows.Forms.Label()
        Me.tbExportOutbound = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnExportCompany = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCompanyType = New System.Windows.Forms.TextBox()
        Me.txtCompany = New System.Windows.Forms.TextBox()
        Me.txtConsinee = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.tbExportOutbound.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtReceipt1)
        Me.Panel1.Controls.Add(Me.lblReceipt1)
        Me.Panel1.Controls.Add(Me.btnCloseReceipt)
        Me.Panel1.Controls.Add(Me.btnExportOutbound)
        Me.Panel1.Controls.Add(Me.txtOrder1)
        Me.Panel1.Controls.Add(Me.lblOrder1)
        Me.Panel1.Controls.Add(Me.cbConsignee1)
        Me.Panel1.Controls.Add(Me.lblConsignee1)
        Me.Panel1.Controls.Add(Me.txtWarehouse1)
        Me.Panel1.Controls.Add(Me.lblWarehouse1)
        Me.Panel1.Location = New System.Drawing.Point(20, 21)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(583, 310)
        Me.Panel1.TabIndex = 0
        '
        'txtReceipt1
        '
        Me.txtReceipt1.Location = New System.Drawing.Point(189, 158)
        Me.txtReceipt1.Margin = New System.Windows.Forms.Padding(4)
        Me.txtReceipt1.Name = "txtReceipt1"
        Me.txtReceipt1.Size = New System.Drawing.Size(132, 22)
        Me.txtReceipt1.TabIndex = 9
        '
        'lblReceipt1
        '
        Me.lblReceipt1.AutoSize = True
        Me.lblReceipt1.Location = New System.Drawing.Point(77, 158)
        Me.lblReceipt1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblReceipt1.Name = "lblReceipt1"
        Me.lblReceipt1.Size = New System.Drawing.Size(71, 17)
        Me.lblReceipt1.TabIndex = 8
        Me.lblReceipt1.Text = "Receipt Id"
        '
        'btnCloseReceipt
        '
        Me.btnCloseReceipt.Location = New System.Drawing.Point(279, 229)
        Me.btnCloseReceipt.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCloseReceipt.Name = "btnCloseReceipt"
        Me.btnCloseReceipt.Size = New System.Drawing.Size(244, 28)
        Me.btnCloseReceipt.TabIndex = 7
        Me.btnCloseReceipt.Text = "Generate Close Receipt Event"
        Me.btnCloseReceipt.UseVisualStyleBackColor = True
        '
        'btnExportOutbound
        '
        Me.btnExportOutbound.Location = New System.Drawing.Point(59, 229)
        Me.btnExportOutbound.Margin = New System.Windows.Forms.Padding(4)
        Me.btnExportOutbound.Name = "btnExportOutbound"
        Me.btnExportOutbound.Size = New System.Drawing.Size(177, 28)
        Me.btnExportOutbound.TabIndex = 6
        Me.btnExportOutbound.Text = "Export Outbound"
        Me.btnExportOutbound.UseVisualStyleBackColor = True
        '
        'txtOrder1
        '
        Me.txtOrder1.Location = New System.Drawing.Point(189, 117)
        Me.txtOrder1.Margin = New System.Windows.Forms.Padding(4)
        Me.txtOrder1.Name = "txtOrder1"
        Me.txtOrder1.Size = New System.Drawing.Size(132, 22)
        Me.txtOrder1.TabIndex = 5
        '
        'lblOrder1
        '
        Me.lblOrder1.AutoSize = True
        Me.lblOrder1.Location = New System.Drawing.Point(77, 117)
        Me.lblOrder1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblOrder1.Name = "lblOrder1"
        Me.lblOrder1.Size = New System.Drawing.Size(60, 17)
        Me.lblOrder1.TabIndex = 4
        Me.lblOrder1.Text = "Order Id"
        '
        'cbConsignee1
        '
        Me.cbConsignee1.FormattingEnabled = True
        Me.cbConsignee1.Location = New System.Drawing.Point(189, 70)
        Me.cbConsignee1.Margin = New System.Windows.Forms.Padding(4)
        Me.cbConsignee1.Name = "cbConsignee1"
        Me.cbConsignee1.Size = New System.Drawing.Size(160, 24)
        Me.cbConsignee1.TabIndex = 3
        '
        'lblConsignee1
        '
        Me.lblConsignee1.AutoSize = True
        Me.lblConsignee1.Location = New System.Drawing.Point(77, 70)
        Me.lblConsignee1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblConsignee1.Name = "lblConsignee1"
        Me.lblConsignee1.Size = New System.Drawing.Size(75, 17)
        Me.lblConsignee1.TabIndex = 2
        Me.lblConsignee1.Text = "Consignee"
        '
        'txtWarehouse1
        '
        Me.txtWarehouse1.Location = New System.Drawing.Point(189, 25)
        Me.txtWarehouse1.Margin = New System.Windows.Forms.Padding(4)
        Me.txtWarehouse1.Name = "txtWarehouse1"
        Me.txtWarehouse1.ReadOnly = True
        Me.txtWarehouse1.Size = New System.Drawing.Size(132, 22)
        Me.txtWarehouse1.TabIndex = 1
        '
        'lblWarehouse1
        '
        Me.lblWarehouse1.AutoSize = True
        Me.lblWarehouse1.Location = New System.Drawing.Point(77, 25)
        Me.lblWarehouse1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblWarehouse1.Name = "lblWarehouse1"
        Me.lblWarehouse1.Size = New System.Drawing.Size(81, 17)
        Me.lblWarehouse1.TabIndex = 0
        Me.lblWarehouse1.Text = "Warehouse"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.btnNext2)
        Me.Panel2.Controls.Add(Me.lbWarehouse2)
        Me.Panel2.Controls.Add(Me.lblWarehouse2)
        Me.Panel2.Location = New System.Drawing.Point(20, 21)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(583, 204)
        Me.Panel2.TabIndex = 1
        '
        'btnNext2
        '
        Me.btnNext2.Location = New System.Drawing.Point(85, 155)
        Me.btnNext2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNext2.Name = "btnNext2"
        Me.btnNext2.Size = New System.Drawing.Size(100, 28)
        Me.btnNext2.TabIndex = 2
        Me.btnNext2.Text = "Next"
        Me.btnNext2.UseVisualStyleBackColor = True
        '
        'lbWarehouse2
        '
        Me.lbWarehouse2.FormattingEnabled = True
        Me.lbWarehouse2.ItemHeight = 16
        Me.lbWarehouse2.Location = New System.Drawing.Point(189, 26)
        Me.lbWarehouse2.Margin = New System.Windows.Forms.Padding(4)
        Me.lbWarehouse2.Name = "lbWarehouse2"
        Me.lbWarehouse2.Size = New System.Drawing.Size(233, 116)
        Me.lbWarehouse2.TabIndex = 1
        '
        'lblWarehouse2
        '
        Me.lblWarehouse2.AutoSize = True
        Me.lblWarehouse2.Location = New System.Drawing.Point(81, 26)
        Me.lblWarehouse2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblWarehouse2.Name = "lblWarehouse2"
        Me.lblWarehouse2.Size = New System.Drawing.Size(81, 17)
        Me.lblWarehouse2.TabIndex = 0
        Me.lblWarehouse2.Text = "Warehouse"
        '
        'tbExportOutbound
        '
        Me.tbExportOutbound.Controls.Add(Me.TabPage1)
        Me.tbExportOutbound.Controls.Add(Me.TabPage2)
        Me.tbExportOutbound.Location = New System.Drawing.Point(16, 15)
        Me.tbExportOutbound.Margin = New System.Windows.Forms.Padding(4)
        Me.tbExportOutbound.Name = "tbExportOutbound"
        Me.tbExportOutbound.SelectedIndex = 0
        Me.tbExportOutbound.Size = New System.Drawing.Size(637, 385)
        Me.tbExportOutbound.TabIndex = 2
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Panel1)
        Me.TabPage1.Controls.Add(Me.Panel2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage1.Size = New System.Drawing.Size(629, 356)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Export Outbound / Close Receipt"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Size = New System.Drawing.Size(629, 356)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Company"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnExportCompany)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.txtCompanyType)
        Me.Panel3.Controls.Add(Me.txtCompany)
        Me.Panel3.Controls.Add(Me.txtConsinee)
        Me.Panel3.Location = New System.Drawing.Point(150, 82)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(339, 204)
        Me.Panel3.TabIndex = 6
        '
        'btnExportCompany
        '
        Me.btnExportCompany.Location = New System.Drawing.Point(77, 155)
        Me.btnExportCompany.Name = "btnExportCompany"
        Me.btnExportCompany.Size = New System.Drawing.Size(178, 35)
        Me.btnExportCompany.TabIndex = 6
        Me.btnExportCompany.Text = "Export"
        Me.btnExportCompany.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(103, 17)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Company Type"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Company"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Consignee"
        '
        'txtCompanyType
        '
        Me.txtCompanyType.Location = New System.Drawing.Point(136, 108)
        Me.txtCompanyType.Name = "txtCompanyType"
        Me.txtCompanyType.Size = New System.Drawing.Size(146, 22)
        Me.txtCompanyType.TabIndex = 2
        '
        'txtCompany
        '
        Me.txtCompany.Location = New System.Drawing.Point(135, 64)
        Me.txtCompany.Name = "txtCompany"
        Me.txtCompany.Size = New System.Drawing.Size(146, 22)
        Me.txtCompany.TabIndex = 1
        '
        'txtConsinee
        '
        Me.txtConsinee.Location = New System.Drawing.Point(136, 18)
        Me.txtConsinee.Name = "txtConsinee"
        Me.txtConsinee.Size = New System.Drawing.Size(146, 22)
        Me.txtConsinee.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 454)
        Me.Controls.Add(Me.tbExportOutbound)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.Text = "SCExpertConnect Utility"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.tbExportOutbound.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtWarehouse1 As System.Windows.Forms.TextBox
    Friend WithEvents lblWarehouse1 As System.Windows.Forms.Label
    Friend WithEvents txtOrder1 As System.Windows.Forms.TextBox
    Friend WithEvents lblOrder1 As System.Windows.Forms.Label
    Friend WithEvents cbConsignee1 As System.Windows.Forms.ComboBox
    Friend WithEvents lblConsignee1 As System.Windows.Forms.Label
    Friend WithEvents btnExportOutbound As System.Windows.Forms.Button
    Friend WithEvents btnNext2 As System.Windows.Forms.Button
    Friend WithEvents lbWarehouse2 As System.Windows.Forms.ListBox
    Friend WithEvents lblWarehouse2 As System.Windows.Forms.Label
    Friend WithEvents tbExportOutbound As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents btnCloseReceipt As System.Windows.Forms.Button
    Friend WithEvents txtReceipt1 As System.Windows.Forms.TextBox
    Friend WithEvents lblReceipt1 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnExportCompany As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCompanyType As TextBox
    Friend WithEvents txtCompany As TextBox
    Friend WithEvents txtConsinee As TextBox
End Class
