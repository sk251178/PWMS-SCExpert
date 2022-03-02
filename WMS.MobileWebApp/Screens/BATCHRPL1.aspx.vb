Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class BATCHRPL1
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            SetScreen()
        End If
    End Sub

    Private Sub SetScreen()
        Dim task As Task = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
        DO1.Value("Container") = brh.REPLCONTAINER
        DO1.Value("ConfirmationLocation") = brh.TARGETLOCATION
        DO1.Value("TargetLocation") = String.Empty
    End Sub

    Private Sub doNext()
        If IsValidated() Then
            Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
            brh.UpdateStatus(WMS.Lib.Statuses.BatchReplenHeader.LETDOWN, "Admin")
            Dim task As Task = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
            task.USERID = WMS.Logic.Common.GetCurrentUser()
            task.Complete(WMS.Logic.LogHandler.GetRDTLogger)
            brh.Letdown()
            Session.Remove(WMS.Lib.SESSION.SHOWTASKMANAGER)
            
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLETDOWN))
        End If
    End Sub

    Private Function IsValidated() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            ' Get and validate the Batch Replenishment Header
            Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
            If brh.STATUS <> Statuses.BatchReplenHeader.RELEASED Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid batch status"))
                Return False
            End If
            If DO1.Value("TargetLocation").Trim() <> brh.TARGETLOCATION Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Location Confirmation"))
                Return False
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return False
        End Try
        Return True
    End Function


    Private Sub doMenu()
        Dim task As Task = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
        If Not IsNothing(task) Then
            task.ExitTask()
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Container")
        DO1.AddLabelLine("ConfirmationLocation", "Staging Location")
        DO1.AddSpacer()
        DO1.AddTextboxLine("TargetLocation", True, "next", "Confirm Location")
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