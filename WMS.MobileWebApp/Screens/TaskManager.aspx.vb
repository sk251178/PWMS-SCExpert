Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports System.Collections.Generic
Imports Made4Net.[Shared]

<CLSCompliant(False)> Public Class TaskManager
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
        If WMS.Logic.Common.GetCurrentUser Is Nothing Or WMS.Logic.Common.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            WriteToRDTLog(" TaskManager.Page_Load")
            Session.Remove("TargetScreen")
            Session.Remove("TMTask")
            Session.Remove("ShowedTaskManager")
            Session.Remove("SetScreen")
            If Not Session("TASKMANAGERSHOULDREDIRECTTOSRC") Is Nothing Then
                If Session("TASKMANAGERSHOULDREDIRECTTOSRC") = "0" Then
                    Session("TASKMANAGERSHOULDREDIRECTTOSRC") = "1"
                Else
                    If Not Session("TASKMANAGERSOURCESCREENID") Is Nothing Then
                        Dim url As String = MapVirtualPath(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select url from sys_screen where screen_id = '{0}'", Session("TASKMANAGERSOURCESCREENID")), Made4Net.Schema.CONNECTION_NAME))
                        Session.Remove("TASKMANAGERSOURCESCREENID")
                        Response.Redirect(url)
                    End If
                End If
            End If
            If Not CheckAssigned() Then
                If Not Session("IsGetTaskRequestFromUser") Is Nothing Then
                    If Session("IsGetTaskRequestFromUser") = "1" Then
                        NextClicked(True)

                    Else
                        NextClicked()
                    End If
                Else
                    NextClicked()
                End If


            End If
            'CheckAssigned()
        End If
        DO1.DefaultButton = DO1.LeftButtonText
        verifyAndUpdateWHActivityMHETypeAndId()
    End Sub

    Private Sub MenuClick()
        'Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        'If WMS.Logic.TaskManager.isAssigned(UserId) Then
        '    Dim tm As New WMS.Logic.TaskManager(UserId, "")
        '    tm.ExitTask()
        'End If
        If Not Session("TMTask") Is Nothing Then
            Dim oTask As Task = Session("TMTask")
            oTask.ExitTask()
            Session.Remove("TMTask")
        End If
        Session.Remove("MANREPLENISHMENT")
        Session.Remove("ShowedTaskManager")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked(Optional ByVal IsGetTaskFromUser As Boolean = False)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Session.Remove("MANREPLENISHMENT")
        If Not String.IsNullOrEmpty(Session("TargetScreen")) Then
            Response.Redirect(MapVirtualPath(Session("TargetScreen")))
        End If
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                'Dim tm As New WMS.Logic.TaskManager
                'tm.RequestTask(UserId)
                Dim oTask As Task = RequestTask(IsGetTaskFromUser, WMS.Logic.LogHandler.GetRDTLogger())
                If Not oTask Is Nothing Then
                    WMS.Logic.TaskManager.setStartLocation(oTask, UserId, WMS.Logic.LogHandler.GetRDTLogger())
                End If
                CheckAssigned()
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
            End Try
        Else
            Dim tm As New WMS.Logic.TaskManager
            Dim oTask As Task

            If Session("SKUMULTISEL_LOADID") IsNot Nothing Then
                Dim listPayloads As List(Of String) = Session("SKUMULTISEL_LOADID")
                oTask = tm.getMultiPutAwayFirstAssignedTask(listPayloads, UserId)
            ElseIf Not IsNothing(Session("TMTask")) Then
                oTask = Session("TMTask")
            Else
                oTask = Logic.TaskManager.getUserAssignedTask(UserId, WMS.Logic.LogHandler.GetRDTLogger())
            End If

            If Not oTask Is Nothing Then

                WMS.Logic.TaskManager.setStartLocation(oTask, UserId, WMS.Logic.LogHandler.GetRDTLogger())
                Session("TMTaskId") = oTask

                'if we come to TM screen from another screen that we wont to return to it back
                If Not Request.QueryString("sourcescreen") Is Nothing Then
                    'If "Staging" Then

                    'End If
                    Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                Else
                    Session("MobileSourceScreen") = "TaskManager"
                End If


                Session("ShowedTaskManager") = True
                Select Case oTask.TASKTYPE.ToUpper
                    Case WMS.Lib.TASKTYPE.CONSOLIDATION
                        Response.Redirect(MapVirtualPath("Screens/CONS.aspx"))
                    Case WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY
                        Response.Redirect(MapVirtualPath("Screens/CONS.aspx"))
                    Case WMS.Lib.TASKTYPE.CONTCONTDELIVERY
                        Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
                    Case WMS.Lib.TASKTYPE.CONTDELIVERY
                        If Not String.IsNullOrEmpty(Session("SkipContScanScreen")) Then
                            Session.Remove("SkipContScanScreen")
                            Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=RPKC"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
                        End If
                    Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY
                        Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
                    Case WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
                        If Not String.IsNullOrEmpty(Session("SkipContScanScreen")) Then
                            Session.Remove("SkipContScanScreen")
                            Response.Redirect(MapVirtualPath("Screens/RPKC1.aspx?sourcescreen=RPKC"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
                        End If
                    Case WMS.Lib.TASKTYPE.CONTPUTAWAY
                        If Not String.IsNullOrEmpty(Session("SkipContScanScreen")) Then
                            Session.Remove("SkipContScanScreen")
                            Response.Redirect(MapVirtualPath("Screens/CNTPW1.aspx?sourcescreen=RPKC"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/CNTPWCNF.aspx"))
                        End If
                    Case WMS.Lib.TASKTYPE.LOADCOUNTING
                        Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
                    Case WMS.Lib.TASKTYPE.DELIVERY
                        Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
                    Case WMS.Lib.TASKTYPE.FULLPICKING
                        Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                    Case WMS.Lib.TASKTYPE.PARTREPL
                        If oTask.TASKSUBTYPE = "2STEP" Then
                            Response.Redirect(MapVirtualPath("Screens/TwoStepReplenishment.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
                        End If
                    Case WMS.Lib.TASKTYPE.LOADDELIVERY
                        Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
                    Case WMS.Lib.TASKTYPE.LOADPUTAWAY
                        'Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
                        If MultiPayloadPutawayHelper.IsMultiPayLoadPutAwayTask(oTask.TASK) Then
                            Response.Redirect(MapVirtualPath("Screens/RPK2Multi.aspx?sourcescreen=MPPW"))
                        Else
                            If Not String.IsNullOrEmpty(Session("SkipLoadScanScreen")) Then

                                If Session("SkipLoadScanScreen").ToString = "2" Then
                                    Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=RPK"))
                                ElseIf Session("SkipLoadScanScreen").ToString = "3" Then 'RWMS-2896
                                    Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=TaskManager")) 'RWMS-2896
                                Else
                                    Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=LDPW")) 'RPK"))
                                End If

                                Session.Remove("SkipLoadScanScreen")

                            Else
                                Response.Redirect(MapVirtualPath("Screens/LDPWCNF.aspx"))
                            End If
                        End If

                    Case WMS.Lib.TASKTYPE.NEGTREPL
                        Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
                    Case WMS.Lib.TASKTYPE.FULLREPL
                        If oTask.TASKSUBTYPE = "2STEP" Then
                            Response.Redirect(MapVirtualPath("Screens/TwoStepReplenishment.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
                        End If

                    Case WMS.Lib.TASKTYPE.PARALLELPICKING
                        Response.Redirect(MapVirtualPath("Screens/PARPCK1.aspx"))
                    Case WMS.Lib.TASKTYPE.PARTIALPICKING
                        'Case Label Code
                        tm = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PARTIALPICKING, LogHandler.GetRDTLogger())
                        Dim pcklst As New Picklist(tm.Task.Picklist)
                        Dim sql As String = String.Format("SELECT RELEASEPOLICYID FROM PICKHEADER PH INNER JOIN PLANSTRATEGYRELEASE PS ON PH.STRATEGYID=PS.STRATEGYID WHERE PS.PICKTYPE='PARTIAL' AND PH.PICKLIST='{0}'", pcklst.PicklistID)
                        Dim _releasePolicyID As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                        Dim releaseStartegy As New ReleaseStrategyDetail(_releasePolicyID)
                        If releaseStartegy.PrintPickList Then
                            Response.Redirect(MapVirtualPath("Screens/CASELBLPRNT.aspx"))
                        Else
                            If Not Session("PCKBagOutPicking") Is Nothing AndAlso Session("PCKBagOutPicking") = 1 Then
                                Response.Redirect(MapVirtualPath("Screens/PCKBagOut.aspx"))
                            Else
                                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                            End If

                        End If
                    Case WMS.Lib.TASKTYPE.NEGPALLETPICK
                        If Not Session("PCKBagOutPicking") Is Nothing AndAlso Session("PCKBagOutPicking") = 1 Then
                            Response.Redirect(MapVirtualPath("Screens/PCKBagOut.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                        End If
                    Case WMS.Lib.TASKTYPE.PICKING
                        If Not Session("PCKBagOutPicking") Is Nothing AndAlso Session("PCKBagOutPicking") = 1 Then
                            Response.Redirect(MapVirtualPath("Screens/PCKBagOut.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                        End If
                    Case WMS.Lib.TASKTYPE.PUTAWAY
                        Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
                    Case WMS.Lib.TASKTYPE.REPLENISHMENT
                        Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
                    Case WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                        Response.Redirect(MapVirtualPath("Screens/LOCBLKCNTTASK1.aspx"))
                    Case WMS.Lib.TASKTYPE.EMPTYHUPICKUPTASK
                        Response.Redirect(MapVirtualPath("Screens/EVACHU.aspx"))
                    Case WMS.Lib.TASKTYPE.LOADCOUNTING
                        Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
                    Case WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                        Response.Redirect(MapVirtualPath("Screens/LOCBLKCNTTASK1.aspx"))
                    Case WMS.Lib.TASKTYPE.LOCATIONCOUNTING
                        Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK.aspx"))
                    Case WMS.Lib.TASKTYPE.EMPTYHUPICKUPTASK
                        Response.Redirect(MapVirtualPath("Screens/EVACHU.aspx"))
                    Case WMS.Lib.TASKTYPE.NSPICKUP
                        Response.Redirect(MapVirtualPath("Screens/NSPICKUP.aspx"))
                    Case WMS.Lib.TASKTYPE.MISC
                        If Session("SetScreen") = True Then
                            Response.Redirect(MapVirtualPath("Screens/MISCTASK.aspx"))
                        Else
                            Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
                        End If
                    Case WMS.Lib.TASKTYPE.BRLETDOWN
                        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLETDOWN))
                    Case WMS.Lib.TASKTYPE.BRUNLOAD
                        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENUNLOAD1))
                End Select
            Else
                setNotAssigned()
            End If
        End If
    End Sub

    Private Function UserIsAssignedPutawayTask() As Boolean
        Dim userHasPutawayTask As Boolean = WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser,
                                                                             WMS.Lib.TASKTYPE.PUTAWAY,
                                                                             WMS.Logic.LogHandler.GetRDTLogger())

        Dim priorScreenWasREPL As Boolean = Session("MobileSourceScreen") = "REPL"

        Return userHasPutawayTask And priorScreenWasREPL
    End Function

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        WriteToRDTLog(" TaskManager.CheckAssigned")
        If Not (Session("MANREPLENISHMENT") Is Nothing) Then
            Dim oTask As Task = Session("MANREPLENISHMENT")
            If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE And
                 oTask.STATUS <> WMS.Lib.Statuses.Task.CANCELED Then
                setAssigned(oTask)
                Session("TMTask") = oTask
                Return True
            Else
                Session.Remove("MANREPLENISHMENT")
            End If
        End If
        ' If there was a putaway task from a replenishment, don't pick up the next asigned task based on priority.
        ' The driver actually has a load on their forklift that needs to be put away first. We have to get
        ' that load!
        If UserIsAssignedPutawayTask() Then
            Dim taskID As String = Logic.TaskManager.getUserAssignedTask(
                WMS.Logic.Common.GetCurrentUser,
                WMS.Lib.TASKTYPE.PUTAWAY,
                WMS.Logic.LogHandler.GetRDTLogger())

            Session("TMTask") = Logic.TaskManager.GetTask(taskID)
        End If

        If Not Session("PCKDELTask") Is Nothing Then
            Dim oTask As Task = Session("PCKDELTask")
            If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE And oTask.STATUS <> WMS.Lib.Statuses.Task.CANCELED Then
                setAssigned(oTask)
                Session("TMTask") = oTask
                Return True
            End If
        End If

        If Not Session("PCKListToResume") Is Nothing Then
            Dim tsk As String = Made4Net.DataAccess.DataInterface.ExecuteScalar($"select top 1 task from tasks where picklist='{Session("PCKListToResume")}' and tasktype  like '%pick%'")
            Dim oTask As Task = New PickTask(tsk)
            If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE And oTask.STATUS <> WMS.Lib.Statuses.Task.CANCELED Then
                setAssigned(oTask)
                Session("TMTask") = oTask
                Return True
            End If
        End If

        If Not (Session(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK) Is Nothing) Then
            Dim oTask As Task = Session(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK)
            If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE And
                 oTask.STATUS <> WMS.Lib.Statuses.Task.CANCELED Then
                setBrUnload(oTask)
                Session("TMTask") = oTask
                Return True
            End If
        End If
        If Not (Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK) Is Nothing) Then
            Dim oTask As Task = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
            If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE And
                 oTask.STATUS <> WMS.Lib.Statuses.Task.CANCELED Then
                setBrLetDown(oTask)
                Session("TMTask") = oTask
                Return True
            End If
        End If

        If Session("TMTask") Is Nothing Then
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim oTask As New Task(Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()))
                Try
                    If oTask.STATUS = "ASSIGNED" Then
                        If oTask.TASKTYPE = WMS.Lib.TASKTYPE.CONTDELIVERY Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.LOADDELIVERY Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.CONTLOADDELIVERY Then
                            If oTask.ASSIGNMENTTYPE = "" Then

                                Dim ASSIGNMENTTYPE As String

                                Dim sql As String = String.Format("select top 1 ASSIGNMENTTYPE from tasks where PICKLIST = '{0}' and STATUS = 'COMPLETE' and TASKTYPE in ('FULLPICK','PARPICK') order by ADDDATE desc", oTask.Picklist)

                                Try
                                    ASSIGNMENTTYPE = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                                Catch ex As Exception
                                    ASSIGNMENTTYPE = ""
                                End Try
                                If ASSIGNMENTTYPE <> "" Then
                                    sql = String.Format("update tasks set ASSIGNMENTTYPE = '{1}' where task='{0}'", oTask.TASK, ASSIGNMENTTYPE)
                                    Made4Net.DataAccess.DataInterface.RunSQL(sql)
                                    oTask.ASSIGNMENTTYPE = ASSIGNMENTTYPE
                                End If
                            End If
                        End If
                        Session("TMTask") = oTask
                    End If
                Catch ex As Exception
                End Try
            End If
        End If

        If Session("TMTask") IsNot Nothing Then
            Dim oTask As Task = Session("TMTask")
            WriteToRDTLog(" TaskManager.CheckAssigned - Session('TMTask').TASK={0} .STATUS={1}", oTask.TASK, oTask.STATUS)
            If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE And oTask.STATUS <> WMS.Lib.Statuses.Task.CANCELED Then
                setAssigned(oTask)
                Return True
            Else
                WriteToRDTLog(" TaskManager.CheckAssigned - Removing 'TMTask' from session")
                Session.Remove("TMTask")
            End If
        End If

        If Logic.TaskManager.isAssigned(UserId, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim oTask As Task
            Try
                If Session("SKUMULTISEL_LOADID") IsNot Nothing Then
                    Dim listPayloads As List(Of String) = Session("SKUMULTISEL_LOADID")
                    oTask = tm.getMultiPutAwayFirstAssignedTask(listPayloads, UserId)
                Else
                    oTask = Logic.TaskManager.getUserAssignedTask(UserId, WMS.Logic.LogHandler.GetRDTLogger())
                End If
            Catch ex As Exception
            End Try
            If Not oTask Is Nothing Then
                WriteToRDTLog(" TaskManager.CheckAssigned - Setting Session('TMTask')= TASK={0} STATUS={1}", oTask.TASK, oTask.STATUS)
                Session("TMTask") = oTask
                setAssigned(oTask)
                Return True
            Else
                setNotAssigned()
                Return False
            End If
        Else
            setNotAssigned()
            Return False
        End If
    End Function

    Protected Sub setBrUnload(ByVal oTask As Task)
        DO1.setVisibility("Assigned", False)
        DO1.setVisibility("TaskId", False)
        DO1.setVisibility("Note", False)
        DO1.setVisibility("PickListId", False)
        DO1.setVisibility("TaskSubType", False)
        DO1.setVisibility("Priority", False)
        DO1.setVisibility("FromLocation", False)
        DO1.setVisibility("AssignedTime", False)
        DO1.setVisibility("STDTIME", False)
        DO1.setVisibility("TaskType", True)
        DO1.setVisibility("CONTAINER", True)
        DO1.setVisibility("PICKREGION", True)
        DO1.setVisibility("NUMLOADS", True)
        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENUNLOADHEADER)
        DO1.Value("TaskType") = oTask.TASKTYPE
        DO1.Value("CONTAINER") = brh.REPLCONTAINER
        DO1.Value("PICKREGION") = brh.PICKREGION
        DO1.Value("NUMLOADS") = CType(Session(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION), BatchReplenDetailCollection).Count
    End Sub
    Protected Sub setBrLetDown(ByVal oTask As Task)
        DO1.setVisibility("Assigned", False)
        DO1.setVisibility("TaskId", True)
        DO1.setVisibility("Note", False)
        DO1.setVisibility("PickListId", False)
        DO1.setVisibility("TaskSubType", False)
        DO1.setVisibility("Priority", False)
        DO1.setVisibility("FromLocation", False)
        DO1.setVisibility("NUMLOADS", False)
        DO1.setVisibility("STDTIME", False)
        DO1.setVisibility("TaskType", True)
        DO1.setVisibility("CONTAINER", True)
        DO1.setVisibility("PICKREGION", True)
        DO1.setVisibility("NUMLOADS", False)
        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
        DO1.Value("TaskId") = oTask.TASK
        DO1.Value("TaskType") = oTask.TASKTYPE
        DO1.Value("CONTAINER") = brh.REPLCONTAINER
        DO1.Value("PICKREGION") = brh.PICKREGION
    End Sub

    Protected Sub setNotAssigned()
        DO1.Value("Assigned") = "Not Assigned"
        DO1.setVisibility("TaskId", False)
        DO1.setVisibility("TaskType", False)
        DO1.setVisibility("Note", False)
        'DO1.setVisibility("PickListId", False)
        'DO1.RightButtonText = "RequestTask"
        DO1.LeftButtonText = "RequestTask"
        If WMS.Logic.GetSysParam("ShowGoalTimeOnTaskAssignment") = 1 Then
            DO1.setVisibility("TaskSubType", False)
            DO1.setVisibility("Priority", False)
            DO1.setVisibility("FromLocation", False)
            DO1.setVisibility("AssignedTime", False)
            DO1.setVisibility("STDTIME", False)
        End If
        DO1.setVisibility("CONTAINER", False)
        DO1.setVisibility("PICKREGION", False)
        DO1.setVisibility("NUMLOADS", False)
    End Sub

    Protected Sub setAssigned(ByVal oTask As Task)
        DO1.Value("Assigned") = "Assigned"
        DO1.setVisibility("TaskId", True)
        DO1.setVisibility("TaskType", True)
        DO1.setVisibility("Note", False)
        DO1.Value("TaskId") = oTask.TASK
        DO1.Value("TaskType") = oTask.TASKTYPE
        If oTask.GetTaskCountForPicklist(oTask.Picklist) > 2 Then
            DO1.setVisibility("Note", True)
            DO1.Value("Note") = "Multiple deliveries and multiple labels"
        End If
        If WMS.Logic.GetSysParam("ShowGoalTimeOnTaskAssignment") = 1 Then
            DO1.setVisibility("TaskSubType", True)
            DO1.Value("TaskSubType") = oTask.TASKSUBTYPE
            DO1.setVisibility("Priority", True)
            DO1.Value("Priority") = oTask.PRIORITY
            DO1.setVisibility("FromLocation", True)
            DO1.Value("FromLocation") = oTask.FROMLOCATION
            DO1.setVisibility("AssignedTime", True)
            DO1.Value("AssignedTime") = oTask.ASSIGNEDTIME
            DO1.setVisibility("STDTIME", True)
            Dim stdTimeStr As String
            Dim secondsStr As String = Math.Round(oTask.STDTIME Mod 60).ToString()
            If secondsStr.Length = 1 Then
                secondsStr = secondsStr.PadLeft(2, "0")
            End If
            stdTimeStr = (Math.Floor(oTask.STDTIME / 60)).ToString() & ":" & secondsStr
            DO1.Value("STDTIME") = stdTimeStr
        End If
        DO1.setVisibility("CONTAINER", False)
        DO1.setVisibility("PICKREGION", False)
        DO1.setVisibility("NUMLOADS", False)
        DO1.LeftButtonText = "Next"
        Session("SetScreen") = True
        Session("IsGetTaskRequestFromUser") = "0"
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Assigned")
        DO1.AddLabelLine("TaskId")
        DO1.AddLabelLine("TaskType")
        DO1.AddLabelLine("Note")
        'DO1.AddTextboxLine("PickListId")
        If WMS.Logic.GetSysParam("ShowGoalTimeOnTaskAssignment") = 1 Then
            DO1.AddLabelLine("TaskSubType")
            DO1.AddLabelLine("Priority")
            DO1.AddLabelLine("FromLocation")
            DO1.AddLabelLine("AssignedTime")
            DO1.AddLabelLine("STDTIME")
        End If
        DO1.AddLabelLine("CONTAINER")
        DO1.AddLabelLine("PICKREGION")
        DO1.AddLabelLine("NUMLOADS")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                NextClicked()
            Case "requesttask"
                NextClicked()
            Case "menu"
                MenuClick()
        End Select
    End Sub

    Private Function RequestTask(Optional ByVal IsGetTaskFromUser As Boolean = False, Optional ByVal logger As ILogHandler = Nothing) As Task
        Dim ts As New WMS.Logic.TaskManager
        Return ts.GetTaskFromTMService(WMS.Logic.Common.GetCurrentUser, IsGetTaskFromUser, logger)
    End Function
    Private Sub verifyAndUpdateWHActivityMHETypeAndId()
        Dim wh As WHActivity = New WHActivity(GetCurrentUser)
        Dim sessionMHEType As String = [Global].userSessionManager.GetMHEType(HttpContext.Current.Session.SessionID)
        Dim sessionMHEID As String = [Global].userSessionManager.GetMHEId(HttpContext.Current.Session.SessionID)
        If Not wh.HETYPE.Equals(sessionMHEType) Then
            wh.UpdateMHEType(sessionMHEType)
        End If
        If Not wh.HANDLINGEQUIPMENTID.Equals(sessionMHEID) Then
            wh.UpdateMHEID(sessionMHEID)
        End If
    End Sub

End Class