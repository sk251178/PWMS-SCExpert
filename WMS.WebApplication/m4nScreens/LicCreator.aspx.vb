Public Class LicCreator
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents pnlCld As System.Web.UI.WebControls.Panel
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEAB As Made4Net.WebControls.TableEditorActionBar
    Protected WithEvents lblLoadId As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtLoadId As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblLoc As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtLocation As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblUnits As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtUnits As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblNumOfLoads As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtNumLoads As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblUOM As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddUOM As Made4Net.WebControls.DropDownList
    Protected WithEvents lblStatus As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddStatus As Made4Net.WebControls.DropDownList
    Protected WithEvents lblHoldRc As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddHold As Made4Net.WebControls.DropDownList
    Protected WithEvents AttribTbl As Made4Net.WebControls.Table
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

End Class
