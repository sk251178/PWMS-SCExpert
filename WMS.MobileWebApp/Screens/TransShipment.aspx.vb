Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class TransShipment
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
        Session.Remove("CONSINGEE")
        Session.Remove("TRANSSHIPMENT")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("CONSINGEE")
        Session.Remove("TRANSSHIPMENT")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If Not WMS.Logic.TransShipment.Exists(Session("CONSINGEE"), Session("TRANSSHIPMENT")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Transshipment does not exist"))
                Return
            End If
            Dim oTrans As New WMS.Logic.TransShipment(Session("CONSINGEE"), Session("TRANSSHIPMENT"))
            oTrans.Receive(WMS.Logic.Common.GetCurrentUser)
            Session.Remove("CONSINGEE")
            Session.Remove("TRANSSHIPMENT")
            Made4Net.Mobile.Common.GoToMenu()
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddDropDown("CONSINGEE", Session("3PL"))
        DO1.AddTextboxLine("TRANSSHIPMENT", True, "receive")

        Dim dd As Made4Net.WebControls.MobileDropDown
        dd = DO1.Ctrl("CONSINGEE")
        dd.AllOption = False
        dd.TableName = "CONSIGNEE"
        dd.ValueField = "CONSIGNEE"
        dd.TextField = "CONSIGNEENAME"
        dd.DataBind()

        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("TRANSSHIPMENT") = DO1.Value("TRANSSHIPMENT")

        Select Case e.CommandText.ToLower
            Case "receive"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class

