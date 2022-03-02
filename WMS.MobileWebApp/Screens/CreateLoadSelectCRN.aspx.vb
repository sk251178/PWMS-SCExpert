Imports Made4Net.Shared.Web

<CLSCompliant(False)> Public Class CreateLoadSelectCRN
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents CRN As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnGo As System.Web.UI.WebControls.Button
    Protected WithEvents lbl As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

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
        If Not IsPostBack Then
            lbl.Visible = False
            CRN.Text = "0000000005"
        End If
    End Sub

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        Dim R As String = Made4Net.DataAccess.DataInterface.DataLookup("RECEIPTDETAIL", "RECEIPT", String.Format("RECEIPT='{0}'", CRN.Text))
        If R Is Nothing Then
            lbl.Visible = True
        Else
            Session("CreateLoadCRN") = CRN.Text
            Response.Redirect(MapVirtualPath("Screens/CreateLoad.aspx"))
        End If
    End Sub
End Class
