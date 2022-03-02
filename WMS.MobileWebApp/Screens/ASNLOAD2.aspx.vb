Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

<CLSCompliant(False)> Public Class ASNLOAD2
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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Dim oAsnCont As New WMS.Logic.AsnContainer(Session("CONTAINERID"), Session("RECEIPTID"))
            DO1.Value("RECEIPTID") = oAsnCont.Receipt
            DO1.Value("CONTAINERID") = oAsnCont.ContainerId
            DO1.Value("NUMLOADS") = oAsnCont.NumLoads
            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("USAGETYPE")
            dd.AllOption = False
            dd.TableName = "CODELISTDETAIL"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.Where = "CODELISTCODE = 'CONTUSAGE'"
            dd.DataBind()
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("CONTAINERID")
        Session.Remove("RECEIPTID")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("CONTAINERID")
        Session.Remove("RECEIPTID")
        Response.Redirect(MapVirtualPath("Screens/ASNLOAD1.aspx"))
    End Sub

    Private Sub doNext()
        Dim usageType, location, warehousearea As String
        Try
            usageType = DO1.Value("USAGETYPE")
            location = DO1.Value("LOCATION")
            warehousearea = Warehouse.getUserWarehouseArea() 'DO1.Value("WAREHOUSEAREA")
            Dim oAsnCont As New WMS.Logic.AsnContainer(Session("CONTAINERID"), Session("RECEIPTID"))
            oAsnCont.Receive(usageType, location, warehousearea, WMS.Logic.Common.GetCurrentUser)
            Session.Remove("CONTAINERID")
            Session.Remove("RECEIPTID")
            Response.Redirect(MapVirtualPath("Screens/ASNLOAD1.aspx"))
        Catch ex As Made4Net.shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("RECEIPTID")
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddLabelLine("NUMLOADS")
        DO1.AddDropDown("USAGETYPE")
        DO1.AddTextboxLine("LOCATION")
        'DO1.AddTextbox Line("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class

