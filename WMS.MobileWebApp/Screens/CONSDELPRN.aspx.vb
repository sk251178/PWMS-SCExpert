Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class CONSDELPRN
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
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()

        If Not IsPostBack Then
            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY, rdtLogger) Then
                Session.Remove("ConsDeliveryTask")
                Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
            Else
                Dim tm As New WMS.Logic.TaskManager

                Dim condel As ConsolidationDeliveryTask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY,
                                                                             rdtLogger)
                Session("ConsDeliveryTask") = condel
                DO1.Value("TASKID") = condel.TASK
                DO1.Value("CONTAINERID") = condel.FromContainer
            End If
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TASKID")
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer")
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doSkip()
        Response.Redirect(MapVirtualPath("Screens/CONSDEL.aspx"))
    End Sub

    Private Sub doNext()
        Dim del As ConsolidationDeliveryTask = Session("ConsDeliveryTask")
        Dim prntr As LabelPrinter
        Try
            prntr = New LabelPrinter(DO1.Value("Printer"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception

        End Try
        Dim cntr As New Container(del.FromContainer, False)
        cntr.PrintShipLabel(prntr.PrinterQName)
        Response.Redirect(MapVirtualPath("Screens/CONSDEL.aspx"))
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                doMenu()
            Case "next"
                doNext()
            Case "skip"
                doSkip()
        End Select
    End Sub

End Class