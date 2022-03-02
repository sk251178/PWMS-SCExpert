Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
<CLSCompliant(False)> Public Class RPKC
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
            If Not Request.QueryString.Item("CntrId") Is Nothing And Request.QueryString.Item("CntrId") <> "" Then
                DO1.Value("CONTAINERID") = Request.QueryString.Item("CntrId")
                doNext(False)
            Else
                checkPutaway()
            End If
        End If
    End Sub

    Private Sub checkPutaway()
        If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim pwtask As PutawayTask
            Dim tm As New WMS.Logic.TaskManager
            pwtask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
            DO1.Value("CONTAINERID") = pwtask.FromContainer
            doNext(True)
        End If
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext(ByVal pSkipTaskManager As Boolean)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oCont As New WMS.Logic.Container(DO1.Value("ContainerId"), True)
            'Cheking if the Container is already assigned to delivery/putaway taks for another user
            'If its not assigned we will try to assignee it else we will give the task to this(requesting) user
            Dim strTask As String
            oCont.RequestPickUp(WMS.Logic.Common.GetCurrentUser)
            'Now need to check which task type we have to perform - is it a ld putaway or opp replenishment
            strTask = oCont.isAssignedToTask()
            If strTask <> "" Then
                Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                leaveScreen(oTask, pSkipTaskManager)
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not assigned to any task"))
            End If
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As WMS.Logic.LogicException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub leaveScreen(ByVal pTask As WMS.Logic.Task, ByVal pSkipTaskManager As Boolean)
        If pTask.TASKTYPE = WMS.Lib.TASKTYPE.CONTLOADPUTAWAY Then
            Session("CONTAINERID") = DO1.Value("CONTAINERID")
            Session("CONTTASKID") = pTask.TASK
            If Not pSkipTaskManager AndAlso MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Session("TMTask") = pTask
                Session("TargetScreen") = "Screens/RPKC1.aspx?sourcescreen=RPKC"
                Session("MobileSourceScreen") = "RPKC"
                Session("SkipContScanScreen") = "1"
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/RPKC1.aspx?sourcescreen=RPKC"))
            End If
        ElseIf pTask.TASKTYPE = WMS.Lib.TASKTYPE.CONTPUTAWAY Then
            Dim oCNTPWTask As WMS.Logic.PutawayTask = New WMS.Logic.PutawayTask(pTask.TASK)
            Session("CotnainerPWTask") = oCNTPWTask
            Session("MobileSourceScreen") = "RPKC"
            If Not pSkipTaskManager AndAlso MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Session("TMTask") = pTask
                Session("TargetScreen") = "Screens/CNTPW1.aspx?sourcescreen=RPKC"
                Session("MobileSourceScreen") = "RPKC"
                Session("SkipContScanScreen") = "1"
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/CNTPW1.aspx?sourcescreen=RPKC"))
            End If
        ElseIf pTask.TASKTYPE = WMS.Lib.TASKTYPE.CONTDELIVERY Then
            Session("MobileSourceScreen") = "RPKC"
            If Not pSkipTaskManager AndAlso MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Session("TMTask") = pTask
                Session("TargetScreen") = "Screens/DEL.aspx?sourcescreen=RPKC"
                Session("SkipContScanScreen") = "1"
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=RPKC"))
            End If
        Else
            Session("TASKMANAGERSOURCESCREENID") = "rpkc"
            Session("MobileSourceScreen") = "rpkc"
            Session("TASKMANAGERSHOULDREDIRECTTOSRC") = "0"
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("ContainerId", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext(False)
            Case "menu"
                doMenu()
        End Select
    End Sub

    ' Returns TaskID that this container is assigned to
    Private Function isAssignedToTask(ByVal ContainerId As String) As String
        Dim strSql As String = "SELECT * FROM TASKS WHERE FROMCONTAINER = '" & ContainerId & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            If Convert.ToString(dt.Rows(0)("STATUS")).ToLower = "assigned" And Convert.ToString(dt.Rows(0)("USERID")) <> GetCurrentUser() Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Container Task assigned to another user", "Container Task assigned to another user")
            End If
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

End Class