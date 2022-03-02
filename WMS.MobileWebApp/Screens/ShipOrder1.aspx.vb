Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
<CLSCompliant(False)> Public Class ShipOrder1
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
        If Not IsPostBack Then
            If Not Session("OUTORDSHPOBJ") Is Nothing Then
                SetScreen()
            Else
                doBack()
            End If
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("OUTORDSHPOBJ")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("Screens/ShipOrder.aspx"))
    End Sub

    Private Sub SetScreen()
        Dim oOrd As OutboundOrderHeader = Session("OUTORDSHPOBJ")
        DO1.Value("CONSIGNEE") = oOrd.CONSIGNEE
        DO1.Value("ORDERID") = oOrd.ORDERID
        DO1.Value("COMPANY") = DataInterface.ExecuteScalar(String.Format("select companyname from company where consignee = '{0}' and company = '{1}' and companytype = '{2}' ", oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE))
        DO1.Value("STATUS") = DataInterface.ExecuteScalar(String.Format("select [description] from codelistdetail where codelistcode = 'outordstat' and code = '{0}'", oOrd.STATUS))
        DO1.Value("REQUESTEDDATE") = oOrd.REQUESTEDDATE
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oOrd As OutboundOrderHeader = Session("OUTORDSHPOBJ")
            oOrd.Ship(WMS.Logic.Common.GetCurrentUser)
            'Go Back To Scan another orderid
            doBack()
        Catch ex As System.Threading.ThreadAbortException
            'Do nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("ORDERID")
        DO1.AddLabelLine("COMPANY")
        DO1.AddLabelLine("STATUS")
        DO1.AddLabelLine("REQUESTEDDATE")
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
