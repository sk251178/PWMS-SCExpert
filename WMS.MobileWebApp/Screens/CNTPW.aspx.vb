Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class CNTPW
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
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim taskid As String = WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
                Dim oTask As WMS.Logic.PutawayTask = New WMS.Logic.PutawayTask(taskid)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                Session("CotnainerPWTask") = oTask
                Response.Redirect(MapVirtualPath("Screens/CNTPWCNF.aspx?sourcescreen=CNTPW"))
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
                Dim oTask As WMS.Logic.PutawayTask = New WMS.Logic.PutawayTask(strTask)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
            Else
                oCont.PutAway(WMS.Logic.Common.GetCurrentUser)
            End If
            strTask = isAssignedToTask(oCont.ContainerId)
            If strTask <> "" Then
                Dim oTask As WMS.Logic.PutawayTask = New WMS.Logic.PutawayTask(strTask)
                Session("CotnainerPWTask") = oTask
                Response.Redirect(MapVirtualPath("Screens/CNTPW1.aspx?sourcescreen=CNTPW"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Could Not Create PW for Container"))
                DO1.Value("ContainerId") = ""
            End If
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("ContainerId", True, "next")
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
        Dim strSql As String = "SELECT TASK FROM TASKS WHERE TASKTYPE ='CONTPW' AND (STATUS <> 'COMPLETE' AND STATUS <> 'CANCELED') AND FROMCONTAINER = '" & ContainerId & "'" ' AND USERID <> '" & WMS.Logic.Common.GetCurrentUser & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

End Class