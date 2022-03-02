Imports Made4Net.Shared.Web
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class CLDInfo
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            DO1.Value("CONSIGNEE") = Session("CreateLoadConsignee")
            DO1.Value("SKUINF") = Session("CreateLoadSKU")
            DO1.Value("RECEIPT") = Session("CreateLoadRCN")
            DO1.Value("RECEIPTLINE") = Session("CreateLoadRCNLine")
            Dim oSku As New Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
            DO1.Value("SKUDESC") = oSku.SKUDESC
        End If
    End Sub

    Private Sub doNext()
        'Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1'", "Made4NetSchema")
        Response.Redirect(MobileUtils.GetURLByScreenCode("RDTCLD1"))
    End Sub

    Private Sub doMenu()
        'Session.Remove("CreateLoadRCN")
        'Session.Remove("CreateLoadRCNLine")
        'Session.Remove("CreateLoadConsignee")
        'Session.Remove("CreateLoadSKU")
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("RECEIPT")
        DO1.AddTextboxLine("RECEIPTLINE")
        DO1.AddTextboxLine("CONSIGNEE")
        DO1.AddTextboxLine("SKUINF")
        DO1.AddTextboxLine("SKUDESC")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class
