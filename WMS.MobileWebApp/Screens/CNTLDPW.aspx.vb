Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Partial Public Class CNTLDPW
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
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            If Not Request.QueryString.Item("CntrId") Is Nothing And Request.QueryString.Item("CntrId") <> "" Then
                DO1.Value("CONTAINERID") = Request.QueryString.Item("CntrId")
                doNext()
            End If
        End If
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oCont As New WMS.Logic.Container(DO1.Value("ContainerId"), True)
            'Cheking if the Container is already assigned to delivery/putaway taks for another user
            'If its not assigned we will try to assignee it else we will give the task to this(requesting) user
            Dim strTask As String = isAssignedToTask(oCont.ContainerId)
            If strTask <> "" Then
                Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
            Else
                oCont.PutAwayLoads(WMS.Logic.Common.GetCurrentUser)
            End If
            Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=CNTLDPW"))
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As WMS.Logic.LogicException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("ContainerId")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    ' Returns TaskID that this load is assigned to
    Private Function isAssignedToTask(ByVal ContainerId As String) As String
        Dim strSql As String = "SELECT TASK FROM TASKS WHERE (STATUS = 'ASSIGNED') AND FROMCONTAINER = '" & ContainerId & "' AND USERID <> '" & WMS.Logic.Common.GetCurrentUser & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

End Class
