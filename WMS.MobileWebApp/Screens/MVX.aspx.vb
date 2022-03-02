<CLSCompliant(False)> Public Class MVX
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
            DO1.Value("CONTAINERID") = Request.QueryString.Item("CntrId")
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONTAINERID", True, "nextload")
        DO1.AddTextboxLine("LOADID", True, "nextload")
        DO1.AddSpacer()
    End Sub

    Private Sub doNextLoad()
        Try
            Dim cntr As New WMS.Logic.Container(DO1.Value("CONTAINERID"), True)
            Dim ld As New WMS.Logic.Load(DO1.Value("LOADID"))
            cntr.Place(ld, WMS.Logic.Common.GetCurrentUser)
            DO1.Value("LOADID") = ""
            DO1.FocusField = "LOADID"
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            Return
        End Try
    End Sub

    Private Sub doPickUpContainer()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/RPKC.aspx?CntrId=" + DO1.Value("CONTAINERID")))
    End Sub

    Private Sub doCloseContainer()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "nextload"
                doNextLoad()
            Case "closecontainer"
                doCloseContainer()
            Case "pickupcontainer"
                doPickUpContainer()
        End Select
    End Sub
End Class
