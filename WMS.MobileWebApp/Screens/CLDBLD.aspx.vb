<CLSCompliant(False)> Public Class CLDBLD
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
            MobileUtils.ClearBlindReceivingSession()
            setConsigneeDropDown()
        End If
    End Sub

    Private Sub doMenu()
        MobileUtils.ClearBlindReceivingSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddDropDown("CONSIGNEE", Session("3PL"))
        DO1.AddTextboxLine("RECEIPTID", True, "next")
        DO1.AddSpacer()
    End Sub

    Private Sub setConsigneeDropDown()
        Dim dd As Made4Net.WebControls.MobileDropDown
        dd = DO1.Ctrl("CONSIGNEE")
        dd.AllOption = False
        dd.TableName = "CONSIGNEE"
        dd.ValueField = "CONSIGNEE"
        dd.TextField = "CONSIGNEENAME"
        dd.DataBind()
    End Sub

    Private Sub doNext()
        Dim RcptId As String = DO1.Value("RECEIPTID")
        Dim SQL As String = String.Format("SELECT * FROM RECEIPTHEADER WHERE RECEIPT LIKE '%{0}'", RcptId)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Receipt Not Found!"))
            Return
        ElseIf dt.Rows.Count > 1 Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("More Than One Receipt Was Found!"))
            Return
        End If
        RcptId = dt.Rows(0)("receipt")
        Dim receiptObj As New WMS.Logic.ReceiptHeader(RcptId)
        If receiptObj.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Receipt is closed."))
            Return
        End If

        Session("CreateLoadReciptId") = RcptId
        Session("CreateLoadConsignee") = DO1.Value("CONSIGNEE")
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CLDBLD1.aspx"))
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