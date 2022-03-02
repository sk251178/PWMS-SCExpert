Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports System.Collections.Generic

Partial Public Class LDPWCNF
    Inherits PWMSRDTBase

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
            Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
            Session.Remove("LoadPWTask")
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, rdtLogger) Then
                Dim tm As New WMS.Logic.TaskManager
                Dim pwtask As WMS.Logic.PutawayTask

                If Session("SKUMULTISEL_LOADID") IsNot Nothing Then
                    Dim listPayloads As List(Of String) = Session("SKUMULTISEL_LOADID")
                    pwtask = tm.getMultiPutAwayFirstAssignedTask(listPayloads, WMS.Logic.Common.GetCurrentUser)
                Else
                    pwtask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, rdtLogger)
                End If

                pwtask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                Session("LoadPWTask") = pwtask
                setScreen()
            End If
        End If
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oTask As WMS.Logic.Task = Session("LoadPWTask")
            'If DO1.Value("Confirm") <> String.Empty AndAlso String.Equals(DO1.Value("Confirm"), oTask.FromContainer, StringComparison.OrdinalIgnoreCase) Then
            If DO1.Value("Confirm") <> String.Empty AndAlso String.Equals(DO1.Value("Confirm"), oTask.FROMLOAD, StringComparison.OrdinalIgnoreCase) Then
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=LDPWCNF"))
            Else
                DO1.Value("Confirm") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Wrong Load Confirmation"))
                Return
            End If
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            DO1.Value("Confirm") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            DO1.Value("Confirm") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
    End Sub

    Private Sub setScreen()
        If Not Session("LoadPWTask") Is Nothing Then
            Dim oTask As WMS.Logic.Task = Session("LoadPWTask")
            DO1.Value("TaskId") = oTask.TASK
            DO1.Value("LoadId") = oTask.FROMLOAD
            DO1.Value("Location") = oTask.FROMLOCATION
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TaskId")
        DO1.AddLabelLine("LoadId")
        DO1.AddLabelLine("Location")
        DO1.AddSpacer()
        DO1.AddTextboxLine("Confirm")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "reportproblem"
                ReportProblem()
        End Select
    End Sub

    Private Sub ReportProblem()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If String.IsNullOrEmpty(DO1.Value("TaskProblemCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No problem code selected"))
                Exit Sub
            End If
            Dim oTask As WMS.Logic.Task = Session("LoadPWTask")
            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(oTask.TOLOCATION, DO1.Value("Confirm"), oTask.TOWAREHOUSEAREA)
            If strConfirmationLocation.ToLower <> oTask.TOLOCATION.ToLower Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Location entered is not valid")
                Return
            End If
            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            Dim ld As New WMS.Logic.Load(oTask.FROMLOAD)
            CType(oTask, WMS.Logic.PutawayTask).ReportProblemOnRetrieval(ld, DO1.Value("TaskProblemCode"), oTask.TOLOCATION, oTask.TOWAREHOUSEAREA, UserId)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location problem reported"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
            Return
        End Try
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER & "?"))
    End Sub

End Class