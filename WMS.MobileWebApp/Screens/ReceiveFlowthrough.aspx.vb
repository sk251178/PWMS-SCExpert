Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class ReceiveFlowThrough
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
    End Sub

    Private Sub doMenu()
        'Session.Remove("FLOWTHROUGH")
        'Session.Remove("CONSIGNEE")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doRecById()
        Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("FLOWTHROUGHNUM") = DO1.Value("FLOWTHROUGH")
        'Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("FLOWTHROUGH") = "1" 'DO1.Value("FLOWTHROUGH")
        Response.Redirect(MapVirtualPath("Screens/ReceiveByID.aspx"))
    End Sub

    Private Sub doCreateLoad()
        Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("FLOWTHROUGHNUM") = DO1.Value("FLOWTHROUGH")
        Session("FLOWTHROUGH") = "1"

        Response.Redirect(MapVirtualPath("Screens/CLD1.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'DO1.AddTextboxLine("CONSINGEE")
        'DO1.AddTextboxLine("FLOWTHROUGH")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        'Session("CONSINGEE") = DO1.Value("CONSINGEE")
        'Session("FLOWTHROUGH") = DO1.Value("FLOWTHROUGH")
        Select Case e.CommandText.ToLower
            Case "creatload"
                doCreateLoad()
            Case "menu"
                doMenu()
            Case "receivebyid"
                doRecById()
        End Select
    End Sub

End Class

