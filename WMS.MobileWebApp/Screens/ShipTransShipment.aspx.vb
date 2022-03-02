Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class ShipTransShipment
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
        Session.Remove("REFERENCEORD")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("CONSINGEE")
        Session.Remove("TRANSSHIPMENT")
        Session.Remove("REFERENCEORD")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Session("CONSINGEE") <> "" Then
            Try
                Dim oConsignee As New WMS.Logic.Consignee(Session("CONSINGEE"))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Consignee not found"))
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Consignee not found"))
            Exit Sub
        End If
        Try
            Dim oTrans As WMS.Logic.TransShipment
            If Session("TRANSSHIPMENT") <> "" Then
                oTrans = New WMS.Logic.TransShipment(Session("CONSINGEE"), Session("TRANSSHIPMENT"))
            ElseIf Session("REFERENCEORD") <> "" Then
                oTrans = New WMS.Logic.TransShipment(Session("CONSINGEE"), Logic.TransShipment.getTranshipmentIDbyRefOrd(Session("CONSINGEE"), Session("REFERENCEORD")))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Transshipment not found"))
                Exit Sub
            End If
            Session.Remove("CONSINGEE")
            Session.Remove("TRANSSHIPMENT")
            Session.Remove("REFERENCEORD")
            If oTrans.CANSHIP Then
                Session("TRANSHIPMENTOBJ") = oTrans
                Response.Redirect(MapVirtualPath("Screens/ShipTransShipment1.aspx"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Transshipment status inncorrect"))
                Exit Sub
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONSINGEE")
        DO1.AddTextboxLine("TRANSSHIPMENT")
        DO1.AddTextboxLine("REFERENCEORD")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("TRANSSHIPMENT") = DO1.Value("TRANSSHIPMENT")
        Session("REFERENCEORD") = DO1.Value("REFERENCEORD")

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

