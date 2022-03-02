Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Partial Public Class CNTPWCNF
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
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim taskid As String = WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
                Dim oTask As WMS.Logic.PutawayTask = New WMS.Logic.PutawayTask(taskid)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                Session("CotnainerPWTask") = oTask
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
            Dim oTask As WMS.Logic.Task = Session("CotnainerPWTask")
            If DO1.Value("Confirm") <> String.Empty AndAlso String.Equals(DO1.Value("Confirm"), oTask.FromContainer, StringComparison.OrdinalIgnoreCase) Then
                Response.Redirect(MapVirtualPath("Screens/CNTPW1.aspx?sourcescreen=CNTPWCNF"))
            Else
                DO1.Value("Confirm") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Wrong Container Confirmation"))
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
        If Not Session("CotnainerPWTask") Is Nothing Then
            Dim oTask As WMS.Logic.Task = Session("CotnainerPWTask")
            DO1.Value("TaskId") = oTask.TASK
            DO1.Value("ContainerId") = oTask.FromContainer
            DO1.Value("Location") = oTask.FROMLOCATION
            DO1.Value("Warehousearea") = oTask.FROMWAREHOUSEAREA
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TaskId")
        DO1.AddLabelLine("ContainerId")
        DO1.AddLabelLine("Location")
        DO1.AddLabelLine("Warehousearea")
        DO1.AddSpacer()
        DO1.AddTextboxLine("Confirm")
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