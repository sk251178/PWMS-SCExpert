Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections.Generic
Imports DataManager.ServiceModel
Imports Made4Net.Algorithms.Interfaces
Imports Made4Net.Algorithms
Imports WMS.Lib

<CLSCompliant(False)> Public Class TaskManager

#Region "Variables"

    Protected _task As Task
    Protected _taskPolicies As TaskPolicies

#End Region

#Region "Properties"

    Public ReadOnly Property Task() As Task
        Get
            Return _task
        End Get
    End Property

    Public ReadOnly Property TaskPolicies() As TaskPolicies
        Get
            Return _taskPolicies
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Shared Function NewTaskManagerForTask(taskId As String)
        Dim tm As New TaskManager()
        tm.Load(taskId)
        Return tm
    End Function

    Public Shared Function NewTaskManagerForPicklist(picklistId As String) As TaskManager
        Dim sql As String = String.Format("select task from tasks where picklist = '{0}' and tasktype in ('{1}','{2}','{3}')",
                                          picklistId,
                                          WMS.Lib.TASKTYPE.FULLPICKING,
                                          WMS.Lib.TASKTYPE.PARTIALPICKING,
                                          WMS.Lib.TASKTYPE.NEGPALLETPICK)
        Return NewTaskManagerForTask(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function NewTaskManagerForBatchReplen(pTASKTYPE As String, pBatchReplID As String) As TaskManager
        Dim sql As String = String.Format("SELECT * FROM TASKS WHERE REPLENISHMENT='{0}' AND TASKTYPE='{1}'", pBatchReplID, pTASKTYPE)

        Return NewTaskManagerForTask(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function NewTaskManagerForUserAndTaskType(userId As String, taskType As String, logger As ILogHandler) As TaskManager
        Dim tm As TaskManager = New TaskManager()
        If logger IsNot Nothing Then
            logger.Write(String.Format("TaskManager::ctor(UserId={0}, TaskType={1})", userId, taskType))
        End If
        If taskType = "" Then
            Dim oTask As Task = getUserAssignedTask(userId, logger)
            tm._task = oTask
        Else
            tm.getAssignedTask(userId, taskType, logger)
        End If
        Return tm
    End Function

    Public Sub New()
    End Sub

#End Region

#Region "Properties"

    Public Shared ReadOnly Property Exists(ByVal TaskId As String)
        Get
            Return WMS.Logic.Task.Exists(TaskId)
        End Get
    End Property

    Public Shared ReadOnly Property ExistPickTask(ByVal PicklistId As String)
        Get
            Dim sql As String = String.Format("Select count(1) from TASKS where PICKLIST = '{0}' and TASKTYPE in ('{1}','{2}','{3}')", PicklistId, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.NEGPALLETPICK, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.PARALLELPICKING)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property

    Public Shared ReadOnly Property ExistBReplenTask(ByVal pBatchReplID As String)
        Get
            Dim sql As String = String.Format("Select count(1) from TASKS where REPLENISHMENT = '{0}' and TASKTYPE = '{1}'", pBatchReplID, WMS.Lib.TASKTYPE.BRLETDOWN)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property

    Public Shared ReadOnly Property ExistConsolidationTask(ByVal consid As String)
        Get
            Dim sql As String = String.Format("Select count(1) from TASKS where CONSOLIDATION = '{0}'", consid)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property

#End Region

#Region "Methods"

#Region "Accessors"
    Protected Sub Load(ByVal TaskId As String)

        Dim sql As String = String.Format("select tasktype from tasks where task = '{0}'", TaskId)
        Dim tsktype As String = DataInterface.ExecuteScalar(sql)
        Select Case tsktype
            Case WMS.Lib.TASKTYPE.FULLPICKING
                _task = New PickTask(TaskId)
            Case WMS.Lib.TASKTYPE.PARTIALPICKING
                _task = New PickTask(TaskId)
            Case WMS.Lib.TASKTYPE.NEGPALLETPICK
                _task = New PickTask(TaskId)
            Case WMS.Lib.TASKTYPE.PARALLELPICKING
                _task = New ParallelPickTask(TaskId)
            Case WMS.Lib.TASKTYPE.NEGTREPL
                _task = New ReplenishmentTask(TaskId)
            Case WMS.Lib.TASKTYPE.FULLREPL
                _task = New ReplenishmentTask(TaskId)
            Case WMS.Lib.TASKTYPE.PARTREPL
                _task = New ReplenishmentTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONTDELIVERY
                _task = New DeliveryTask(TaskId)
            Case WMS.Lib.TASKTYPE.LOADDELIVERY
                _task = New DeliveryTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY
                _task = New DeliveryTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONTCONTDELIVERY
                _task = New DeliveryTask(TaskId)
            Case WMS.Lib.TASKTYPE.LOADPUTAWAY
                _task = New PutawayTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
                _task = New PutawayTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONTPUTAWAY
                _task = New PutawayTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONSOLIDATION
                _task = New ConsolidationTask(TaskId)
            Case WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY
                _task = New ConsolidationDeliveryTask(TaskId)
            Case WMS.Lib.TASKTYPE.EMPTYHUPICKUPTASK
                _task = New EmptyHUPickupTask(TaskId)
            Case WMS.Lib.TASKTYPE.LOADCOUNTING
                _task = New CountTask(TaskId)
            Case WMS.Lib.TASKTYPE.LOCATIONCOUNTING
                _task = New CountTask(TaskId)
            Case WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                _task = New CountTask(TaskId)
                'Case WMS.Lib.TASKTYPE.BRLETDOWN
                '    _task = New CountTask(TaskId)
            Case Else
                _task = New Task(TaskId)
        End Select
    End Sub
    Public Function getPicklistDeliveryTask(picklistID As String, UserId As String) As DeliveryTask
        Dim dt As New DataTable
        Dim SQL As String = $"select top 1 task from tasks " +
            $" where userid = '{UserId}' And assigned = 1" +
            $" And tasktype in ('{WMS.Lib.TASKTYPE.LOADDELIVERY}','{WMS.Lib.TASKTYPE.CONTDELIVERY}','{WMS.Lib.TASKTYPE.CONTLOADDELIVERY}','{WMS.Lib.TASKTYPE.CONTCONTDELIVERY}')" +
            $" And status = '{WMS.Lib.Statuses.Task.ASSIGNED}'" +
            $" And picklist = '{picklistID}'"
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return Nothing
        Else
            Return New DeliveryTask(Convert.ToString(dt.Rows(0)(0)))
        End If
    End Function
    Public Function getAssignedTask(ByVal UserId As String, ByVal pTaskType As String, logger As LogHandler, Optional ByVal batchReplId As String = Nothing) As Task
        Dim sql As String
        Dim dt As New DataTable
        If pTaskType = WMS.Lib.TASKTYPE.PARTIALPICKING Then
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype in ('{1}') and status = '{2}'", UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.Statuses.Task.ASSIGNED)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.PICKING Then
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype in ('{1}','{2}','{4}') and status = '{3}'", UserId, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.NEGPALLETPICK)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.REPLENISHMENT Then
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype in ('{1}','{2}','{3}') and status = '{4}'", UserId, WMS.Lib.TASKTYPE.PARTREPL, WMS.Lib.TASKTYPE.FULLREPL, WMS.Lib.TASKTYPE.NEGTREPL, WMS.Lib.Statuses.Task.ASSIGNED)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.PUTAWAY Then
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype in ('{1}','{2}','{3}') and status = '{4}' order by task desc", UserId, WMS.Lib.TASKTYPE.LOADPUTAWAY, WMS.Lib.TASKTYPE.CONTPUTAWAY, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, WMS.Lib.Statuses.Task.ASSIGNED)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.DELIVERY Then
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype in ('{1}','{2}','{3}','{4}') and status = '{5}'", UserId, WMS.Lib.TASKTYPE.LOADDELIVERY, WMS.Lib.TASKTYPE.CONTDELIVERY, WMS.Lib.TASKTYPE.CONTLOADDELIVERY, WMS.Lib.TASKTYPE.CONTCONTDELIVERY, WMS.Lib.Statuses.Task.ASSIGNED)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.BRUNLOAD And batchReplId IsNot Nothing Then
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype = '{1}' and REPLENISHMENT='{2}' and status = '{3}'", UserId, pTaskType, batchReplId, WMS.Lib.Statuses.Task.ASSIGNED)
        Else
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype = '{1}' and status = '{2}'", UserId, pTaskType, WMS.Lib.Statuses.Task.ASSIGNED)
        End If
        If logger IsNot Nothing Then
            logger.Write(String.Format("getAssignedTask: pTaskType={0} sql={1}", pTaskType, sql))
        End If
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return Nothing
        Else
            If logger IsNot Nothing Then
                logger.Write(String.Format("returned task: {0}", dt.Rows(0)("task")))
            End If
            Load(dt.Rows(0)("task"))
            SetStartTimeFromWHActivity(_task, UserId)
            Load(_task.TASK)
            Return _task
        End If
    End Function

    Public Function getMultiPutAwayFirstAssignedTask(ByVal listPayloads As List(Of String), ByVal UserId As String) As Task

        Dim loadList As String = String.Join(",", listPayloads.ToArray())

        Dim strSql As String = String.Format("SELECT top(1) task from tasks inner join  dbo.Split('{0}', ',') as loadIds on tasks.FROMLOAD = loadIds.Data where (STATUS <> 'COMPLETE' AND STATUS <> 'CANCELED') AND userid = '{1}'  order by loadIds.id asc", loadList, UserId)
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Load(Convert.ToString(dt.Rows(0)("TASK")))
            ' RWMS-1497 : Set Start time from whactivity
            SetStartTimeFromWHActivity(_task, UserId)
            ' RWMS-1497 : Set Start time from whactivity
            Return _task
        Else
            Return Nothing
        End If
    End Function

    Public Sub adjustMultiPutawaySingleTaskFromLocaion(ByVal loadid As String, ByVal newFromLoc As String, ByVal UserId As String)
        Dim strSql As String = String.Format("update tasks set FROMLOCATION = '{0}' where FROMLOAD = '{1}' AND userid = '{2}' and (STATUS <> 'COMPLETE' AND STATUS <> 'CANCELED')", newFromLoc, loadid, UserId)
        DataInterface.RunSQL(strSql)
    End Sub


    Public Sub getParallelPickTask(ByVal parallelPickId As String)
        Dim sql As String = String.Format("Select top 1 Task from TASKS where ParallelPicklist = '{0}'", parallelPickId)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Load(dt.Rows(0)("task"))
        End If
    End Sub

    Public Shared Function GetTask(ByVal pTaskID As String) As Task
        Dim sql As String = String.Format("Select tasktype from tasks where task='{0}'", pTaskID)
        Dim type As String = DataInterface.ExecuteScalar(sql)

        If String.IsNullOrEmpty(type) Then
            Throw New M4NException(New Exception, "Task does not exist.", "Task does not exist.")
        End If

        Select Case type.ToUpper()
            Case WMS.Lib.TASKTYPE.CONSOLIDATION
                Return New WMS.Logic.ConsolidationTask(pTaskID)

            Case WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY
                Return New WMS.Logic.ConsolidationDeliveryTask(pTaskID)

            Case WMS.Lib.TASKTYPE.CONTDELIVERY, WMS.Lib.TASKTYPE.CONTCONTDELIVERY, WMS.Lib.TASKTYPE.CONTLOADDELIVERY, WMS.Lib.TASKTYPE.LOADDELIVERY
                Return New WMS.Logic.DeliveryTask(pTaskID)

            Case WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, WMS.Lib.TASKTYPE.CONTPUTAWAY, WMS.Lib.TASKTYPE.LOADPUTAWAY
                Return New WMS.Logic.PutawayTask(pTaskID)

            Case WMS.Lib.TASKTYPE.LOADCOUNTING, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                Return New WMS.Logic.CountTask(pTaskID)

            Case WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.NEGPALLETPICK
                Return New WMS.Logic.PickTask(pTaskID)

            Case WMS.Lib.TASKTYPE.PARTREPL, WMS.Lib.TASKTYPE.NEGTREPL, WMS.Lib.TASKTYPE.FULLREPL
                Return New WMS.Logic.ReplenishmentTask(pTaskID)

            Case WMS.Lib.TASKTYPE.PARALLELPICKING
                Return New WMS.Logic.ParallelPickTask(pTaskID)

            Case WMS.Lib.TASKTYPE.BRUNLOAD
                Return New WMS.Logic.ReplenishmentTask(pTaskID)

            Case WMS.Lib.TASKTYPE.BRLETDOWN
                Return New WMS.Logic.ReplenishmentTask(pTaskID)

            Case Else
                Return New WMS.Logic.Task(pTaskID)
        End Select

    End Function

#End Region

#Region "Create"

    Public Sub CreatePickTask(ByVal Pcklist As Picklist, ByVal TaskPriority As Int32, ByVal pTaskSubType As String)
        _task = New PickTask
        _task.ASSIGNED = False
        If Pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            _task.CONSIGNEE = Pcklist(1).Consignee
            _task.SKU = Pcklist(1).SKU
            _task.FROMLOAD = Pcklist(1).FromLoad
            _task.FROMLOCATION = Pcklist(1).FromLocation
            _task.FROMWAREHOUSEAREA = Pcklist(1).FromWarehousearea

            _task.TASKTYPE = WMS.Lib.TASKTYPE.FULLPICKING
            If Pcklist.hasContainer Then
                _task.ToContainer = Pcklist(1).ToContainer
            Else
                _task.TOLOCATION = Pcklist(1).ToLocation
                _task.TOWAREHOUSEAREA = Pcklist(1).ToWarehousearea
                _task.TOLOAD = Pcklist(1).ToLoad
            End If
        Else
            If Pcklist.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
                _task.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING
                'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
                _task.TOLOCATION = Pcklist(1).ToLocation
                'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
            ElseIf Pcklist.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
                _task.TASKTYPE = WMS.Lib.TASKTYPE.NEGPALLETPICK
            End If
            _task.FROMLOCATION = Pcklist(1).FromLocation
            _task.FROMWAREHOUSEAREA = Pcklist(1).FromWarehousearea
        End If
        _task.TASKSUBTYPE = pTaskSubType
        _task.Picklist = Pcklist.PicklistID
        _task.PRIORITY = TaskPriority
        _task.Post()
    End Sub

    Public Sub CreateParallelPickTask(ByVal ParallelList As ParallelPicking, ByVal TaskPriority As Int32, ByVal pTaskSubType As String)
        _task = New PickTask
        _task.ASSIGNED = False

        _task.ToContainer = ParallelList.ToContainer
        _task.TASKTYPE = WMS.Lib.TASKTYPE.PARALLELPICKING
        _task.TASKSUBTYPE = pTaskSubType
        _task.ParallelPicklist = ParallelList.ParallelPickId
        _task.PRIORITY = TaskPriority
        _task.FROMLOCATION = ParallelList.PickLists(0).Lines(0).FromLocation
        _task.FROMWAREHOUSEAREA = ParallelList.PickLists(0).Lines(0).FromWarehousearea
        _task.Post()
    End Sub

    'Public Sub CreateReplenishmentTask(ByVal Repl As Replenishment, ByVal ld As Load, Optional ByVal ReplPriority As Int32 = 200)
    Public Sub CreateReplenishmentTask(ByVal Repl As Replenishment, ByVal ld As Load, Optional ByVal ReplPriority As Int32 = 100, Optional ByVal TaskSubType As String = "")
        _task = New ReplenishmentTask
        _task.TASKSUBTYPE = TaskSubType
        _task.TASKTYPE = Repl.ReplType
        _task.CONSIGNEE = ld.CONSIGNEE
        _task.SKU = ld.SKU
        _task.Replenishment = Repl.ReplId
        _task.FROMLOCATION = Repl.FromLocation
        _task.FROMWAREHOUSEAREA = Repl.FromWarehousearea
        _task.FROMLOAD = Repl.FromLoad
        _task.TOLOCATION = Repl.ToLocation
        _task.TOWAREHOUSEAREA = Repl.ToWarehousearea
        _task.TOLOAD = Repl.ToLoad
        _task.PRIORITY = ReplPriority

        _task.Post()
    End Sub
    Public Sub CreateBatchReplenishmentTask(ByVal batchreplen As BatchReplenHeader, Optional ByVal ReplPriority As Int32 = 100, Optional ByVal TaskSubType As String = "")
        _task = New ReplenishmentTask
        _task.TASKSUBTYPE = TaskSubType
        _task.TASKTYPE = WMS.Lib.TASKTYPE.BRLETDOWN
        _task.CONSIGNEE = batchreplen.CONSIGNEE
        _task.Replenishment = batchreplen.BATCHREPLID
        _task.FromContainer = batchreplen.REPLCONTAINER
        _task.ToContainer = batchreplen.REPLCONTAINER
        _task.FROMLOCATION = batchreplen.FROMLOCATION
        _task.FROMWAREHOUSEAREA = batchreplen.WAREHOUSEAREA
        _task.TOLOCATION = batchreplen.TARGETLOCATION
        _task.PRIORITY = ReplPriority
        _task.Post()
    End Sub
    Public Sub CreateBatchReplenUnloadTask(brh As BatchReplenHeader)
        Dim BRUnloadTask As BatchReplenUnloadTask = New BatchReplenUnloadTask()
        Dim drpolicydetail As DataRow
        GetBatchReplenPolicyDetail(brh.REPLENPOLICY, drpolicydetail, WMS.Logic.LogHandler.GetRDTLogger)
        BRUnloadTask.Create(brh.BATCHREPLID, "Admin")
        BRUnloadTask.FROMLOCATION = brh.TARGETLOCATION
        BRUnloadTask.STARTLOCATION = brh.TARGETLOCATION
        BRUnloadTask.FromContainer = brh.REPLCONTAINER
        BRUnloadTask.ExecutionWarehousearea = brh.WAREHOUSEAREA
        BRUnloadTask.TOWAREHOUSEAREA = brh.WAREHOUSEAREA
        BRUnloadTask.STARTWAREHOUSEAREA = brh.WAREHOUSEAREA
        BRUnloadTask.PRIORITY = drpolicydetail("TASKPRIORITY")
        BRUnloadTask.Save(BRUnloadTask.TASK)
    End Sub
    Public Sub CreateCountTask(ByVal Cnt As Counting, Optional ByVal CntPriority As Int32 = 100)
        _task = New CountTask
        _task.TASKTYPE = Cnt.COUNTTYPE
        _task.CONSIGNEE = Cnt.CONSIGNEE
        _task.SKU = Cnt.SKU
        _task.COUNTID = Cnt.COUNTID
        _task.FROMLOCATION = Cnt.LOCATION
        _task.FROMWAREHOUSEAREA = Cnt.WAREHOUSEAREA
        _task.FROMLOAD = Cnt.LOADID
        _task.PRIORITY = CntPriority
        _task.ADDUSER = Cnt.ADDUSER
        _task.EDITUSER = Cnt.EDITUSER
        _task.Post()
    End Sub

    Public Sub CreateConsolidationTask(ByVal Cons As Consolidation, Optional ByVal priorty As Int32 = 200)
        _task = New ConsolidationTask

        _task.TASKTYPE = WMS.Lib.TASKTYPE.CONSOLIDATION
        _task.Consolidation = Cons.ConsolidateId
        _task.TOLOCATION = Cons.DestinationLocation
        _task.TOWAREHOUSEAREA = Cons.DestinationWarehousearea
        _task.PRIORITY = priorty
        _task.ToContainer = Cons.ToContainer

        _task.Post()
    End Sub

    Public Sub CreatePutAwayTask(ByVal ld As Load, ByVal pUser As String, Optional ByVal AssignUser As Boolean = True, Optional ByVal pTaskPriorty As Int32 = 200, Optional ByVal pDestinationLocation As String = "", Optional ByVal pDestinationWarehousearea As String = "", Optional ByRef taskId As String = Nothing, Optional ByVal prePopulateLocation As String = "") ' RWMS-1277
        _task = New PutawayTask

        _task.TASKTYPE = WMS.Lib.TASKTYPE.LOADPUTAWAY
        _task.CONSIGNEE = ld.CONSIGNEE
        _task.SKU = ld.SKU
        _task.FROMLOCATION = ld.LOCATION
        _task.FROMWAREHOUSEAREA = ld.WAREHOUSEAREA
        _task.FROMLOAD = ld.LOADID
        If pDestinationLocation <> "" Then
            _task.TOLOCATION = pDestinationLocation
        Else
            _task.TOLOCATION = ld.DESTINATIONLOCATION
        End If
        If pDestinationWarehousearea <> "" Then
            _task.TOWAREHOUSEAREA = pDestinationWarehousearea
        Else
            _task.TOWAREHOUSEAREA = ld.DESTINATIONWAREHOUSEAREA
        End If
        _task.TOLOAD = ld.LOADID
        _task.PRIORITY = pTaskPriorty
        'Start RWMS-1277
        If prePopulateLocation = "" Then
            _task.PREPOPULATELOCATION = False
        Else
            _task.PREPOPULATELOCATION = prePopulateLocation
        End If
        'End RWMS-1277
        If AssignUser Then
            _task.Post(pUser, taskId)
        Else
            _task.Post(taskId)
        End If
        System.Web.HttpContext.Current.Session("PUTAWAYTASK") = _task.TASK
    End Sub

    Public Sub CreateContainerLoadPutAwayTask(ByVal Cont As Container, ByVal pUser As String, Optional ByVal pTaskPriorty As Int32 = 200)
        _task = New PutawayTask
        _task.TASKTYPE = WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
        _task.FromContainer = Cont.ContainerId
        _task.PRIORITY = pTaskPriorty
        _task.FROMLOCATION = Cont.Loads(0).LOCATION
        _task.FROMWAREHOUSEAREA = Cont.Loads(0).WAREHOUSEAREA
        If pUser = "" Then
            _task.Post()
        Else
            _task.Post(pUser)
        End If
    End Sub

    Public Sub CreateContainerPutAwayTask(ByVal oCont As Container, ByVal pUser As String, Optional ByVal pTaskPriorty As Int32 = 200, Optional ByVal pDestinationLocation As String = "", Optional ByVal pDestinationWarehousearea As String = "", Optional ByVal pAssignUser As Boolean = True)
        _task = New PutawayTask
        _task.TASKTYPE = WMS.Lib.TASKTYPE.CONTPUTAWAY
        _task.FromContainer = oCont.ContainerId
        _task.FROMLOCATION = oCont.Location
        _task.FROMWAREHOUSEAREA = oCont.Warehousearea
        If pDestinationLocation <> "" Then
            _task.TOLOCATION = pDestinationLocation
        Else
            _task.TOLOCATION = oCont.DestinationLocation
        End If
        If pDestinationWarehousearea <> "" Then
            _task.TOWAREHOUSEAREA = pDestinationWarehousearea
        Else
            _task.TOWAREHOUSEAREA = oCont.DestinationWarehousearea
        End If
        _task.ToContainer = oCont.ContainerId
        _task.PRIORITY = pTaskPriorty
        If pUser <> "" And pAssignUser Then
            _task.Post(pUser)
        Else
            _task.Post()
        End If
    End Sub
    'Begin for RWMS-1294 and RWMS-1222
    'Public Sub CreateDeliveryTask(ByVal pckTask As PickTask, ByVal pUser As String)
    Public Sub CreateDeliveryTask(ByVal pckTask As PickTask, ByVal pUser As String, ByVal logger As LogHandler)

        'End for RWMS-1294 and RWMS-1222
        Dim pcklist As New Picklist(pckTask.Picklist)
        Dim picklinesToSameSL As New Dictionary(Of String, List(Of WMS.Logic.PicklistDetail))

        For Each line As WMS.Logic.PicklistDetail In pcklist.Lines
            If Not picklinesToSameSL.ContainsKey(line.ToLocation) Then
                picklinesToSameSL.Add(line.ToLocation, New List(Of WMS.Logic.PicklistDetail))
            End If
            picklinesToSameSL(line.ToLocation).Add(line)
        Next

        Dim tmpCont As WMS.Logic.Container
        Dim containers As New HashSet(Of String)
        Dim toLocations As New HashSet(Of String)
        For Each pckDet As PicklistDetail In pcklist.Lines
            If String.IsNullOrEmpty(pckDet.ToContainer) Then
                If Not pckDet.ToLoad.Equals(String.Empty, StringComparison.OrdinalIgnoreCase) AndAlso Not toLocations.Contains(pckDet.ToLocation) Then
                    _task = New DeliveryTask
                    CType(_task, DeliveryTask).CreateLoadDeliveryTask(pckDet.ToLoad, pckDet.ToLocation, pckDet.ToWarehousearea, pUser, pckTask.Picklist) ', pckTask.ASSIGNMENTTYPE)
                    toLocations.Add(pckDet.ToLocation)
                End If
            Else
                If Not containers.Contains(pckDet.ToContainer) Then
                    _task = New DeliveryTask
                    If WMS.Logic.Container.Exists(pckDet.ToContainer) Then
                        tmpCont = New WMS.Logic.Container(pckDet.ToContainer, True)
                        Dim sql As String
                        sql = String.Format("select COUNT(1) from LOADS where HANDLINGUNIT = '{0}' AND LOADS.ACTIVITYSTATUS = 'PICKED'", pckDet.ToContainer)
                        Dim numberOfLoads = System.Convert.ToInt32(DataInterface.ExecuteScalar(sql))
                        If numberOfLoads > 0 Then
                            ' PWMS-560, Only is Staning lane exists.
                            sql = String.Format("select OUTBOUNDORHEADER.STAGINGLANE from OUTBOUNDORHEADER inner join PICKDETAIL ON OUTBOUNDORHEADER.ORDERID = PICKDETAIL.ORDERID Where PICKDETAIL.PICKLIST = '{0}' AND PICKDETAIL.PICKLISTLINE = '{1}'", pckDet.PickList, pckDet.PickListLine)
                            Dim stangingLane = DataInterface.ExecuteScalar(sql).ToString()
                            If Not String.IsNullOrEmpty(stangingLane) Then
                                ' PWMS-560, Only is Staging lane exists.
                                If (tmpCont.Status = WMS.Lib.Statuses.Container.STATUSNEW Or tmpCont.Status = WMS.Lib.Statuses.Container.DELIVERYPEND) And (tmpCont.ActivityStatus = String.Empty) Then
                                    'Begin for RWMS-1294 and RWMS-1222
                                    If Not logger Is Nothing Then
                                        logger.Write("...Started creating Container Delivery Task...")
                                    End If
                                    'End   for RWMS-1294 and RWMS-1222
                                    CType(_task, DeliveryTask).CreateContainerDeliveryTask(tmpCont.ContainerId, pckDet.ToLocation, pckDet.ToWarehousearea, pUser, Nothing, pckTask.Picklist)
                                End If
                            End If
                        End If
                        containers.Add(tmpCont.ContainerId)
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub CreateDeliveryTask(ByVal ParpckTask As ParallelPickTask, ByVal pUser As String)

        Dim contArrList As New ArrayList
        Dim parpcklst As New ParallelPicking(ParpckTask.ParallelPicklist)
        Dim toloc, toWHArea As String
        Dim parpck As ParallelPickList
        For Each parpck In parpcklst
            For Each oLine As PicklistDetail In parpck.Lines
                If Not contArrList.Contains(oLine.ToContainer) Then
                    toloc = oLine.ToLocation
                    toWHArea = oLine.ToWarehousearea
                    If toloc <> String.Empty AndAlso WMS.Logic.Container.Exists(oLine.ToContainer) Then
                        Dim cnt As New Container(oLine.ToContainer, True)
                        If cnt.Loads.Count > 0 Then
                            cnt.SetDestinationLocation(toloc, toWHArea, pUser)
                            contArrList.Add(cnt.ContainerId)
                        End If
                    End If
                End If
            Next
        Next
        If toloc <> String.Empty AndAlso parpcklst.ToContainer <> String.Empty AndAlso WMS.Logic.Container.Exists(parpcklst.ToContainer) Then
            _task = New DeliveryTask
            'Begin for RWMS-1294 and RWMS-1222
            CType(_task, DeliveryTask).CreateContainerDeliveryTask(parpcklst.ToContainer, toloc, toWHArea, pUser, Nothing, ParpckTask.ParallelPicklist, True)
            ' CType(_task, DeliveryTask).CreateContainerDeliveryTask(parpcklst.ToContainer, toloc, toWHArea, pUser, ParpckTask.ParallelPicklist, True)
            'End for RWMS-1294 and RWMS-1222
        ElseIf toloc <> String.Empty Then
            For i As Int32 = 0 To contArrList.Count - 1
                _task = New DeliveryTask
                'Begin for RWMS-1294 and RWMS-1222
                CType(_task, DeliveryTask).CreateContainerDeliveryTask(contArrList(i), toloc, toWHArea, pUser, Nothing, ParpckTask.ParallelPicklist, True)
                ' CType(_task, DeliveryTask).CreateContainerDeliveryTask(contArrList(i), toloc, toWHArea, pUser, ParpckTask.ParallelPicklist, True)
                'End for RWMS-1294 and RWMS-1222
            Next
        End If
    End Sub
    Sub GetBatchReplenPolicyDetail(ByVal strBatchReplPolicy As String, ByRef drpolicydetail As DataRow, Optional ByVal oLogger As LogHandler = Nothing)
        Dim sql As String = String.Format("SELECT * FROM REPLPOLICYDETAIL where POLICYID='{0}' ", strBatchReplPolicy)
        If Not oLogger Is Nothing Then
            oLogger.Write("SQL statement : " & sql)
            oLogger.writeSeperator("-", 100)
        End If
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            drpolicydetail = dr
        Else
            dr = Nothing
        End If
    End Sub

#End Region

#Region "Complete, Assign"

    Public Shared Function isAssigned(ByVal puser As String, ByVal pTaskType As String, oLogger As LogHandler, Optional ByVal batchReplId As String = Nothing) As Boolean
        Dim sql As String
        If pTaskType = WMS.Lib.TASKTYPE.PICKING Then
            Return isAssigned(puser, WMS.Lib.TASKTYPE.PARTIALPICKING, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.FULLPICKING, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.NEGPALLETPICK, oLogger)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.REPLENISHMENT Then
            Return isAssigned(puser, WMS.Lib.TASKTYPE.FULLREPL, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.PARTREPL, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.NEGTREPL, oLogger)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.PUTAWAY Then
            Return isAssigned(puser, WMS.Lib.TASKTYPE.LOADPUTAWAY, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.CONTPUTAWAY, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, oLogger)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.DELIVERY Then
            Return isAssigned(puser, WMS.Lib.TASKTYPE.LOADDELIVERY, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.CONTDELIVERY, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.CONTLOADDELIVERY, oLogger) _
                Or isAssigned(puser, WMS.Lib.TASKTYPE.CONTCONTDELIVERY, oLogger)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.BRUNLOAD And batchReplId IsNot Nothing Then
            sql = String.Format("Select count(1) from TASKS where USERID = '{0}' and ASSIGNED = 1 and TASKTYPE = '{1}' and REPLENISHMENT = '{2}' and STATUS = 'ASSIGNED'", puser, pTaskType, batchReplId)
        Else
            sql = String.Format("Select count(1) from TASKS where userid = '{0}' and assigned = 1 and tasktype = '{1}' and status = 'ASSIGNED'", puser, pTaskType)
        End If
        Dim result As Boolean = System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        If oLogger IsNot Nothing Then
            oLogger.Write(String.Format("isAssigned({0},{1}): sql={2} returns={3}", puser, pTaskType, sql, result))
        End If
        Return result
    End Function

    Public Shared Function isAssigned(ByVal pUser As String, oLogger As LogHandler) As Boolean
        Dim sql As String
        sql = String.Format("Select count(1) from TASKS where userid = '{0}' and assigned = 1 and status = 'ASSIGNED'", pUser)
        Dim result As Boolean = System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        If oLogger IsNot Nothing Then
            oLogger.Write(String.Format("isAssigned({0}): sql={1} returns={2}", pUser, sql, result))
        End If
        Return result
    End Function

    Public Shared Function getUserAssignedTask(ByVal UserId As String, oLogger As LogHandler) As Task
        Dim sql As String
        Dim dt As New DataTable
        'Added for RWMS-1481 and RWMS-1424 added ORDER BY TASK to the sql
        'sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and status = '{1}'", UserId, WMS.Lib.Statuses.Task.ASSIGNED)
        sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and status = '{1}' ORDER BY TASK", UserId, WMS.Lib.Statuses.Task.ASSIGNED)

        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            If oLogger IsNot Nothing Then
                oLogger.Write("getUserAssignedTask({0}): returns nothing", UserId)
            End If
            Return Nothing
        Else
            Dim t As String = dt.Rows(0)("task")
            If oLogger IsNot Nothing Then
                oLogger.Write("getUserAssignedTask({0}): returns {1}", UserId, t)
            End If

            Dim whPreviousActivityTime As DateTime?
            whPreviousActivityTime = WHActivity.GetPreviousActivityTime(whPreviousActivityTime, UserId)

            UpdateTaskStartTime(whPreviousActivityTime, t)
            Dim ts As New Task(t)
            Return ts
        End If
    End Function

    Private Shared Sub UpdateTaskStartTime(ByVal startTime As DateTime?, ByVal taskID As String)
        Dim query As String
        query = String.Format("update tasks set starttime={0} where TASK = {1}", Made4Net.Shared.Util.FormatField(startTime),
                        Made4Net.Shared.Util.FormatField(taskID))
        DataInterface.RunSQL(query)
    End Sub
    ''' <summary>
    ''' If the user started a task and leaves it before comleting StartLocation of that task is not cleared.
    ''' This method will clear the StartLocation.
    ''' </summary>
    ''' <param name="task"></param>
    ''' <param name="UserId"></param>
    Public Shared Sub setStartLocation(ByRef task As Task, ByVal UserId As String, logger As ILogHandler)

        Dim previousTaskEndLocation As String = WHActivity.GetPreviousActivityLocation(UserId)
        Dim whactivityLocation As String = WHActivity.GetUserCurrentLocation(UserId)

        Dim startlocation As String = String.Empty

        If Not String.IsNullOrEmpty(previousTaskEndLocation) Then
            startlocation = previousTaskEndLocation
        Else
            If Not String.IsNullOrEmpty(whactivityLocation) Then
                startlocation = whactivityLocation
            Else
                startlocation = task.FROMLOCATION
            End If
        End If

        If Not task.STARTLOCATION.Equals(startlocation, StringComparison.OrdinalIgnoreCase) Then
            Dim sql As String = String.Format("Update Tasks set StartLocation='{0}' where Task={1}", startlocation, Made4Net.Shared.Util.FormatField(task.TASK))
            logger.SafeWrite("Clearing previous StartLocation '{0}' of task. Sql: {1}", task.STARTLOCATION, sql)
            DataInterface.RunSQL(sql)
            task.STARTLOCATION = startlocation
        End If
    End Sub

    'Added for RWMS-2262 RWMS-2222
    Public Shared Function IsUserAssignedForPickList(ByVal UserId As String, ByVal PickListID As String) As Boolean
        Dim sql As String
        Dim dt As New DataTable
        sql = String.Format("select Count(1) TS.TASK FROM PICKHEADER PH INNER JOIN TASKS TS on PH.PICKLIST = TS.PICKLIST WHERE TS.USERID ='{0}' AND PH.PICKLIST ='{1}'  AND PH.STATUS NOT IN ('{2}','{3}') AND TS.STATUS ='{4}' AND TS.ASSIGNED = 1 ", UserId, PickListID, WMS.Lib.Statuses.Picklist.CANCELED, WMS.Lib.Statuses.Picklist.COMPLETE, WMS.Lib.Statuses.Task.ASSIGNED)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    Public Shared Function isUserAssignedForDeliverTask(ByVal puser As String, ByVal pTaskType As String, ByVal PickListID As String) As Boolean
        Dim sql As String
        If pTaskType = WMS.Lib.TASKTYPE.DELIVERY Then
            Return isUserAssignedForDeliverTask(puser, WMS.Lib.TASKTYPE.LOADDELIVERY, PickListID) Or isUserAssignedForDeliverTask(puser, WMS.Lib.TASKTYPE.CONTDELIVERY, PickListID) Or isUserAssignedForDeliverTask(puser, WMS.Lib.TASKTYPE.CONTLOADDELIVERY, PickListID) Or isUserAssignedForDeliverTask(puser, WMS.Lib.TASKTYPE.CONTCONTDELIVERY, PickListID)
        Else
            sql = String.Format("select Count(1)  FROM PICKHEADER PH INNER JOIN TASKS TS on PH.PICKLIST = TS.PICKLIST WHERE TS.USERID ='{0}' AND PH.PICKLIST ='{1}'  AND PH.STATUS NOT IN ('{2}','{3}') AND TS.STATUS ='{4}' AND TS.ASSIGNED = 1 AND TS.TASKTYPE ='{5}' ", puser, PickListID, WMS.Lib.Statuses.Picklist.CANCELED, WMS.Lib.Statuses.Picklist.COMPLETE, WMS.Lib.Statuses.Task.ASSIGNED, pTaskType)

            Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End If
    End Function
    Public Shared Function isUserAssignedPickListTask(ByVal puser As String, ByVal pTaskType As String, ByVal PickListID As String) As Boolean
        Dim sql As String
        If pTaskType = WMS.Lib.TASKTYPE.PICKING Then
            Return isUserAssignedPickListTask(puser, WMS.Lib.TASKTYPE.PARTIALPICKING, PickListID) Or isUserAssignedPickListTask(puser, WMS.Lib.TASKTYPE.FULLPICKING, PickListID) Or isUserAssignedPickListTask(puser, WMS.Lib.TASKTYPE.NEGPALLETPICK, PickListID)
        Else
            sql = String.Format("select Count(1)  FROM PICKDETAIL PD INNER JOIN TASKS TS on PD.PICKLIST = TS.PICKLIST WHERE TS.USERID ='{0}' AND PD.PICKLIST ='{1}'  AND PD.STATUS NOT IN ('{2}','{3}') AND TS.STATUS ='{4}' AND TS.ASSIGNED = 1 AND TS.TASKTYPE ='{5}' ", puser, PickListID, WMS.Lib.Statuses.Picklist.CANCELED, WMS.Lib.Statuses.Picklist.COMPLETE, WMS.Lib.Statuses.Task.ASSIGNED, pTaskType)

            Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End If
    End Function
    'End Added for RWMS-2262 RWMS-2222

    Public Shared Function getUserAssignedTask(ByVal puser As String, ByVal pTaskType As String, logger As LogHandler) As String
        Dim sql As String
        Dim task As String
        If pTaskType = WMS.Lib.TASKTYPE.PICKING Then
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.PARTIALPICKING, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.FULLPICKING, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.NEGPALLETPICK, logger)
            If task <> String.Empty Then
                Return task
            End If
        ElseIf pTaskType = WMS.Lib.TASKTYPE.REPLENISHMENT Then
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.FULLREPL, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.PARTREPL, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.NEGTREPL, logger)
            If task <> String.Empty Then
                Return task
            End If
        ElseIf pTaskType = WMS.Lib.TASKTYPE.PUTAWAY Then
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.LOADPUTAWAY, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.CONTPUTAWAY, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, logger)
            If task <> String.Empty Then
                Return task
            End If
        ElseIf pTaskType = WMS.Lib.TASKTYPE.DELIVERY Then
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.LOADDELIVERY, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.CONTDELIVERY, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.CONTLOADDELIVERY, logger)
            If task <> String.Empty Then
                Return task
            End If
            task = getUserAssignedTask(puser, WMS.Lib.TASKTYPE.CONTCONTDELIVERY, logger)
            If task <> String.Empty Then
                Return task
            End If
        Else
            Dim prevActivity As New WHActivity(puser)
            'Assign the newly created Putaway Task for the previous Partial Negative Replen Task
            If pTaskType = WMS.Lib.TASKTYPE.LOADPUTAWAY AndAlso prevActivity.ACTIVITY = WMS.Lib.TASKTYPE.NEGTREPL AndAlso System.Web.HttpContext.Current.Session("isNegativeWithPutway") Then
                task = System.Web.HttpContext.Current.Session("PUTAWAYTASK")
                If logger IsNot Nothing Then
                    logger.Write("getUserAssignedTask({0}, {1}): returns {2}", puser, pTaskType, task)
                End If
            Else
                sql = String.Format("Select task from TASKS where userid = '{0}' and assigned = 1 and tasktype = '{1}'", puser, pTaskType)
                task = Convert.ToString(DataInterface.ExecuteScalar(sql))
                If logger IsNot Nothing Then
                    logger.Write("getUserAssignedTask({0}, {1}): sql={2}: returns {3}", puser, pTaskType, sql, task)
                End If
            End If
            Return task
        End If
    End Function

    ''' <summary>
    ''' Assign Load delivery Task - PWMS-520/560
    ''' </summary>
    ''' <param name="UserId">User to whome to assign</param>
    ''' <param name="Pcklist">Load delivery task of this picklist</param>
    ''' <remarks></remarks>
    Public Sub AssignDeleveryTask(ByVal UserId As String, ByVal Pcklist As Picklist, ByVal TaskType As String)
        Dim sql As String
        sql = String.Format("select Top(1) TASK from TASKS where PICKLIST = '{0}' AND TASKTYPE = '{1}' AND ASSIGNED  = 0 ", Pcklist.PicklistID, TaskType)
        Dim taskId As String = DataInterface.ExecuteScalar(sql)
        If Not String.IsNullOrEmpty(taskId) Then
            Dim t As WMS.Logic.DeliveryTask = New DeliveryTask(taskId)
            t.AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.AUTOMATIC, Pcklist.HandelingUnitType)
            ' 'RWMS-1497
            Dim whPreviousActivityTime As DateTime?
            Dim whActivity As New DataTable
            Dim query As String
            query = String.Format("Select * from WHACTIVITY where userid = '{0}'", UserId)
            DataInterface.FillDataset(query, whActivity)
            If whActivity.Rows.Count >= 1 And Not whActivity.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
                whPreviousActivityTime = whActivity.Rows(0)("PREVIOUSACTIVITYTIME")
            End If
            If whPreviousActivityTime.HasValue Then
                t.SetStartTime(whPreviousActivityTime)
            End If
            'RWMS-1497
        End If
    End Sub
    'Begin for RWMS-1294 and RWMS-1222
    'Public Sub Complete()
    Public Sub Complete(ByVal logger As LogHandler)
        Try
            _task.Complete(logger)
        Catch ex As Exception
            If Not logger Is Nothing Then
                logger.Write("Eror Occured while completing task : " & ex.ToString)
            End If
            Throw ex
        End Try
        'End for RWMS-1294 and RWMS-1222
    End Sub
    ''' <summary>
    ''' RWMS-1497
    ''' </summary>
    ''' <param name="t"></param>
    ''' <param name="UserId"></param>
    ''' <remarks></remarks>
    Protected Sub SetStartTimeFromWHActivity(ByVal t As Task, ByVal UserId As String)
        ' 'RWMS-1497
        Dim whPreviousActivityTime As DateTime?
        Dim whActivity As New DataTable
        Dim query As String
        query = String.Format("Select * from WHACTIVITY where userid = '{0}'", UserId)
        DataInterface.FillDataset(query, whActivity)
        If whActivity.Rows.Count >= 1 And Not whActivity.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
            whPreviousActivityTime = whActivity.Rows(0)("PREVIOUSACTIVITYTIME")
        End If
        If whPreviousActivityTime.HasValue Then
            t.SetStartTime(whPreviousActivityTime)
            query = String.Format("update tasks set starttime={0}, editdate={0}, edituser='{2}' where TASK = '{1}'", Made4Net.Shared.Util.FormatField(whPreviousActivityTime), t.TASK, WMS.Lib.USERS.SYSTEMUSER)
            DataInterface.RunSQL(query)
        End If
        'RWMS-1497
    End Sub

    Public Sub Cancel()
        _task.Cancel()
    End Sub

    Public Sub ExitTask()
        _task.ExitTask()
    End Sub

    Public Function RequestTask(ByVal UserId As String, ByVal pTaskType As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal batchReplId As String = Nothing) As Task
        If isAssigned(UserId, pTaskType, oLogger, batchReplId) Then
            Return getAssignedTask(UserId, pTaskType, oLogger, batchReplId)
        End If
        ' All tasks should be assigned according to the task assignment policy

        If oLogger Is Nothing Then
            Dim useLogs As String = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.CountingServiceConfigSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.CountingServiceUseLogs) ''PWMS-887
            If useLogs Is Nothing Or useLogs = "1" Then
                Dim dirpath As String = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.ApplicationLogDirectory)
                dirpath = Made4Net.DataAccess.Util.BuildAndGetFilePath(dirpath)
                oLogger = New LogHandler(dirpath, UserId & DateTime.Now.ToString("_ddMMyyyy_hhmmss_") & New Random().Next() & ".txt")
            End If

        End If

        Dim sql As String
        If pTaskType = WMS.Lib.TASKTYPE.PICKING Then
            'Commented for RWMS-734
            'Commented for RWMS -486
            'sql = String.Format("select top 1 TASK from TASKS where assigned = 0 and status = '{0}' and (TASKTYPE = '{1}' or TASKTYPE = '{2}' or TASKTYPE = '{3}') Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.NEGPALLETPICK)
            'End Commented for RWMS -486
            'Added for RWMS -486
            'sql = String.Format("SELECT TOP 1 T.TASK FROM (SELECT T.TASK, TASKTYPE,PRIORITY, (CASE WHEN TASKTYPE IN ('FULLPICK','PARPICK') AND ( SELECT ROLE FROM USERSKILL WHERE USERID = '{0}' ) IN ('PP', 'F')THEN FROMLOCATION " & _
            '"WHEN TASKTYPE IN ('FULLREPL','NEGTREPL') AND ( SELECT ROLE FROM USERSKILL WHERE USERID = '{0}' ) = 'NPP' THEN TOLOCATION END ) AS LOCATION " & _
            '"FROM TASKS T WHERE assigned = 0 and status = 'AVAILABLE' ) AS T JOIN LOCATION L ON L.LOCATION = T.LOCATION INNER JOIN " & _
            '"(SELECT C.WORKREGIONID, BOUNDARYFROM, BOUNDARYTO, A.USERID, A.ROLE FROM USERSKILL A JOIN USERWORKREGION B ON A.USERID = B.USERID " & _
            '"JOIN WORKREGION C ON C.WORKREGIONID = B.WORKREGIONID WHERE A.USERID = '{0}' ) AS B ON L.AISLE >= B.BOUNDARYFROM AND L.AISLE <= B.BOUNDARYTO " & _
            '"WHERE T.TASKTYPE = ( SELECT ( CASE WHEN ROLE = 'PP' THEN 'PARPICK' WHEN ROLE = 'F' THEN 'FULLPICK' WHEN ROLE = 'NPP' THEN 'NPP' END )FROM USERSKILL WHERE USERID = '{0}') " & _
            '"ORDER BY T.PRIORITY DESC, T.TASK ", UserId)
            'End Added for RWMS -486
            'End Commented for RWMS-734

            'Added for RWMS-734, RWMS-736, RWMS-497
            sql = String.Format("SELECT TOP 1 T.TASK FROM (SELECT T.TASK, TASKTYPE,PRIORITY, (CASE WHEN TASKTYPE IN ('FULLPICK','PARPICK') AND ( SELECT ROLE FROM USERSKILL WHERE USERID = '{0}' ) IN ('PP', 'F')THEN FROMLOCATION " &
                    "WHEN TASKTYPE IN ('FULLREPL','NEGTREPL') AND ( SELECT ROLE FROM USERSKILL WHERE USERID = '{0}' ) = 'NPP' THEN TOLOCATION END ) AS LOCATION " &
                     "FROM TASKS T WHERE assigned = 0 and status = 'AVAILABLE' ) AS T JOIN LOCATION L ON L.LOCATION = T.LOCATION INNER JOIN " &
                     "(SELECT C.WORKREGIONID, BOUNDARYFROM, BOUNDARYTO, A.USERID, A.ROLE, C.workregionboundarytype FROM USERSKILL A JOIN USERWORKREGION B ON A.USERID = B.USERID " &
                     "JOIN WORKREGION C ON C.WORKREGIONID = B.WORKREGIONID WHERE A.USERID = '{0}' ) AS B ON ( CASE WHEN B.workregionboundarytype = 'PICKREGION' AND ( L.pickregion >= B.BOUNDARYFROM AND L.pickregion <= B.BOUNDARYTO ) THEN 1 WHEN B.workregionboundarytype ='PUTREGION' AND ( L.putregion >= B.BOUNDARYFROM AND L.putregion <= B.BOUNDARYTO ) THEN 1 WHEN B.workregionboundarytype ='AISLES' AND ( L.aisle >= B.BOUNDARYFROM AND L.aisle <= B.BOUNDARYTO ) THEN 1 ELSE 0 END ) = 1 " &
                     "WHERE T.TASKTYPE = ( SELECT ( CASE WHEN ROLE = 'PP' THEN 'PARPICK' WHEN ROLE = 'F' THEN 'FULLPICK' WHEN ROLE = 'NPP' THEN 'NPP' END )FROM USERSKILL WHERE USERID = '{0}') " &
                     "ORDER BY T.PRIORITY DESC, T.TASK ", UserId)
            'End Added for RWMS-734, RWMS-736, RWMS-497
        ElseIf pTaskType = WMS.Lib.TASKTYPE.PARTIALPICKING Then
            sql = String.Format("select top 1 TASK from TASKS where assigned = 0 and status = '{0}' and TASKTYPE = '{1}' Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.PARTIALPICKING)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.FULLPICKING Then
            sql = String.Format("select top 1 TASK from TASKS where assigned = 0 and status = '{0}' and TASKTYPE = '{1}' Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.FULLPICKING)
        ElseIf pTaskType = WMS.Lib.TASKTYPE.REPLENISHMENT Then

            Dim objpriorityreset As Prioritize = New PrioritizeReplenishments
            Dim ReqTaskPriority As Integer = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_PENDING))

            'Added for RWMS-2598 Start Restrict 100 priority task to be assigned to the user
            sql = String.Format("select TOP 1 Tasks.TASK from (SELECT T.TASK,T.PRIORITY FROM (SELECT T.TASK, TASKTYPE,PRIORITY,(CASE WHEN TASKTYPE IN ('FULLREPL','PARTREPL','NEGTREPL') " &
            " AND (SELECT ROLE FROM USERSKILL WHERE USERID = '{0}' ) IN ('FR', 'FRP', 'R', 'RP')THEN FROMLOCATION END) AS LOCATION FROM TASKS T WHERE assigned = 0 and status = 'AVAILABLE') " &
            " AS T JOIN LOCATION L ON L.LOCATION = T.LOCATION INNER JOIN (SELECT C.WORKREGIONID, BOUNDARYFROM, BOUNDARYTO, A.USERID, A.ROLE, C.workregionboundarytype FROM USERSKILL A " &
            " JOIN USERWORKREGION B ON A.USERID = B.USERID JOIN WORKREGION C ON C.WORKREGIONID = B.WORKREGIONID WHERE A.USERID = '{0}') AS B " &
            " ON  ( CASE WHEN B.workregionboundarytype = 'PICKREGION' AND ( L.pickregion >= B.BOUNDARYFROM AND L.pickregion <= B.BOUNDARYTO )  THEN 1 WHEN B.workregionboundarytype ='PUTREGION' " &
            " AND ( L.putregion >= B.BOUNDARYFROM AND L.putregion <= B.BOUNDARYTO )  THEN 1 WHEN B.workregionboundarytype ='AISLES' AND ( L.aisle >= B.BOUNDARYFROM AND L.aisle <= B.BOUNDARYTO ) THEN 1 ELSE 0 END ) = 1 " &
            " WHERE T.TASKTYPE IN ('FULLREPL','PARTREPL','NEGTREPL') AND ROLE IN ('FR', 'FRP', 'R', 'RP') ) as Tasks where Tasks.PRIORITY > {1} ORDER BY Tasks.PRIORITY DESC, Tasks.TASK ", UserId, ReqTaskPriority.ToString())
            'Added for RWMS-2598 End

        ElseIf pTaskType = WMS.Lib.TASKTYPE.BRLETDOWN Then
            sql = String.Format("SELECT TOP 1 T.TASK FROM (SELECT T.TASK, TASKTYPE,PRIORITY,(CASE WHEN TASKTYPE ='BRLETDOWN' THEN 'BRPL' Else '' END) AS [ROLE] FROM TASKS T " &
            "WHERE assigned = 0 and status = 'AVAILABLE')  AS T join USERSKILL sk on t.ROLE=sk.ROLE where sk.USERID='{0}'", UserId)

        ElseIf pTaskType = WMS.Lib.TASKTYPE.BRUNLOAD And batchReplId IsNot Nothing Then
            sql = String.Format("SELECT T.TASK,T.TASKTYPE, T.PRIORITY,t.ROLE,sk.USERID FROM (SELECT T.TASK, TASKTYPE,PRIORITY,(CASE WHEN TASKTYPE ='BRUNLOAD' AND REPLENISHMENT='{0}' THEN 'BRPU' Else '' END) AS [ROLE] FROM TASKS T " &
            "WHERE assigned = 0 and status = 'AVAILABLE')  AS T join USERSKILL sk on t.ROLE=sk.ROLE where sk.USERID='{1}'", batchReplId, UserId)

        Else
            sql = String.Format("select top 1 TASK from TASKS where assigned = 0 and status = '{0}' and TASKTYPE = '{1}' Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, pTaskType)
        End If

        Dim task As String = ""
        Try
            task = DataInterface.ExecuteScalar(sql)
        Catch ex As Exception

        End Try
        If task = "" Or task Is Nothing Then
            Return Nothing
        Else
            Load(task)
            Dim dtHE As New DataTable
            Dim query, userMHType As String
            Dim whPreviousActivityTime As DateTime?
            query = String.Format("Select * from WHACTIVITY where userid = '{0}'", UserId)
            DataInterface.FillDataset(query, dtHE)
            If dtHE.Rows.Count >= 1 Then
                userMHType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtHE.Rows(0)("hetype"), "%")
                'RWMS-1497
                '' Discovered in RWMS-1729 : Need to null check
                If Not dtHE.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
                    whPreviousActivityTime = dtHE.Rows(0)("PREVIOUSACTIVITYTIME")
                End If
                '' Discovered in RWMS-1729 : Need to null check
                'End RWMS-1497
            End If
            AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.MANUAL, userMHType)
            'Added for RWMS-1497
            If whPreviousActivityTime.HasValue Then
                _task.SetStartTime(whPreviousActivityTime)
            End If
            'End RWMS-1497
            Return _task
        End If
    End Function

    Public Function RequestPickTaskByPicklistId(ByVal UserId As String, ByVal pTaskType As String, ByVal pPickListId As String, logger As LogHandler) As Task
        If isAssigned(UserId, pTaskType, logger) Then
            Return getAssignedTask(UserId, pTaskType, logger)
        End If
        Dim sql As String
        If pTaskType = WMS.Lib.TASKTYPE.PICKING Then
            sql = String.Format("select top 1 TASK from TASKS where assigned = 0 and status = '{0}' and (TASKTYPE = '{1}' or TASKTYPE = '{2}' or TASKTYPE = '{4}') and picklist = '{3}' Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.FULLPICKING, pPickListId, WMS.Lib.TASKTYPE.NEGPALLETPICK)
        End If

        Dim task As String = ""
        Try
            task = DataInterface.ExecuteScalar(sql)
        Catch ex As Exception

        End Try
        If task = "" Or task Is Nothing Then
            Return Nothing
        Else
            Load(task)
            AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.MANUAL)
            Return _task
        End If
    End Function
    Public Function RequestPartialPickTaskByPicklistId(ByVal UserId As String, ByVal pTaskType As String, ByVal pPickListId As String, logger As LogHandler) As Task
        Dim sql As String
        If pTaskType = WMS.Lib.TASKTYPE.PARTIALPICKING Then
            sql = String.Format("select top 1 TASK from TASKS where  status IN ('{0}','{1}') and TASKTYPE = '{2}' and picklist = '{3}' Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.PARTIALPICKING, pPickListId)
        End If

        Dim task As String = ""
        Try
            task = DataInterface.ExecuteScalar(sql)
        Catch ex As Exception

        End Try
        If task = "" Or task Is Nothing Then
            Return Nothing
        Else
            Load(task)
            AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.MANUAL)
            Return _task
        End If
    End Function
    Public Function RequestPickTaskByOrderId(ByVal UserId As String, ByVal pTaskType As String, ByVal pConsignee As String, ByVal pOrderId As String, logger As LogHandler) As Task
        If isAssigned(UserId, pTaskType, logger) Then
            Return getAssignedTask(UserId, pTaskType, logger)
        End If
        Dim sql As String
        'get the only picklist created on the order
        Dim picklistId As String
        sql = String.Format("select distinct picklist from pickdetail where consignee like '%{0}' and orderid = '{1}' and status in ('PLANNED','RELEASED')", pConsignee, pOrderId)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 1 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "More Than 1 Picklist", "More Than 1 Picklist")
        ElseIf dt.Rows.Count = 1 Then
            picklistId = dt.Rows(0)("picklist")
        ElseIf dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "No Picklist Was Found for order", "No Picklist Was Found for order")
        End If

        sql = String.Format("select top 1 TASK from TASKS where assigned = 0 and status = '{0}' and (TASKTYPE = '{1}' or TASKTYPE = '{2}') and picklist = '{3}' Order By Priority desc, task", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.FULLPICKING, picklistId)

        Dim task As String = ""
        Try
            task = DataInterface.ExecuteScalar(sql)
        Catch ex As Exception

        End Try
        If task = "" Or task Is Nothing Then
            Return Nothing
        Else
            Load(task)
            AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.MANUAL)
            Return _task
        End If
    End Function

    Public Function RequestDeliveryTask(ByVal UserId As String, ByVal pTaskType As String, ByVal pObjId As String, Optional ByVal pAssignType As String = WMS.Lib.TASKASSIGNTYPE.MANUAL) As Task
        Dim sql As String

        If pTaskType = "LDDEL" Then
            sql = String.Format("select top 1 TASK from TASKS inner join INVLOAD on tasks.fromcontainer = INVLOAD.HANDLINGUNIT where TASKS.status <> '{0}' and TASKS.status <> '{1}' and loadid = '{2}' Order By Priority desc, task", WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED, pObjId)
        Else
            sql = String.Format("select top 1 task from TASKS inner join container on tasks.fromcontainer = container.oncontainer " & _
                "where TASKS.status <> '{0}' and TASKS.status <> '{1}' and  (oncontainer = '{2}' or container = '{2}' ) " & _
                "union " & _
                "select top 1 task from TASKS inner join INVLOAD on tasks.fromcontainer = INVLOAD.HANDLINGUNIT " & _
                "where TASKS.status <> '{0}' and TASKS.status <> '{1}' and HANDLINGUNIT = '{2}' ", WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED, pObjId)
        End If

        Dim task As String = ""
        Try
            task = DataInterface.ExecuteScalar(sql)
        Catch ex As Exception

        End Try
        If task = "" Or task Is Nothing Then
            Return Nothing
        Else
            Load(task)
            Dim dtHE As New DataTable
            Dim query, userMHType As String
            Dim whPreviousActivityTime As DateTime?
            query = String.Format("Select * from WHACTIVITY where userid = '{0}'", UserId)
            DataInterface.FillDataset(query, dtHE)
            If dtHE.Rows.Count >= 1 Then
                userMHType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtHE.Rows(0)("hetype"), "%")
                '' Discovered in RWMS-1729 : Need to null check
                If Not dtHE.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
                    whPreviousActivityTime = dtHE.Rows(0)("PREVIOUSACTIVITYTIME")
                End If
                '' Discovered in RWMS-1729 : Need to null check
            End If
            AssignUser(UserId, pAssignType, userMHType)

            ' 'RWMS-1497

            If whPreviousActivityTime.HasValue Then
                _task.SetStartTime(whPreviousActivityTime)
            End If
            'RWMS-1497
            Load(task)

            Return _task
        End If
    End Function


    Public Sub AssignUser(ByVal UserId As String, ByVal pAssignmentType As String, Optional ByVal userMHType As String = "")
        _task.AssignUser(UserId, pAssignmentType, userMHType)
    End Sub

    Public Sub getConsolidationTask(ByVal ConsId As String)
        Dim sql As String
        sql = String.Format("select top 1 TASK from TASKS where consolidation = '{0}'", ConsId)

        Dim task As String = ""
        Try
            task = DataInterface.ExecuteScalar(sql)
        Catch ex As Exception

        End Try
        If task = "" Or task Is Nothing Then
            Return
        Else
            Load(task)
        End If
    End Sub

    Public Function GetCountingTask(ByVal CountId As String, ByVal pUserId As String) As WMS.Logic.Task
        Dim sql As String
        sql = String.Format("select top 1 TASK from TASKS where countid = '{0}'", CountId)
        Dim task As String = ""
        task = DataInterface.ExecuteScalar(sql)
        If task = "" Or task Is Nothing Then
            Return Nothing
        Else
            Load(task)
            AssignUser(pUserId, WMS.Lib.TASKASSIGNTYPE.MANUAL)
            Return _task
        End If
    End Function

    Public Function RequestTask(ByVal UserId As String, ByVal Simulate As Integer, ByVal TaskAssignmentTypeAuto As Boolean, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal IsGetTaskFromUser As Boolean = False) As Task
        If Not oLogger Is Nothing Then
            oLogger.StartWrite()
            oLogger.Write("Begining task request for User: " & UserId)
            If Simulate = 1 Then
                oLogger.Write("Simulating request task " & UserId)
            End If
            oLogger.writeSeperator()
        End If
        Dim dtHE As New DataTable

        Dim SQL, userMHType As String
        'RWMS-1497 - Get PreviousActivityTime
        Dim whPreviousActivityTime As DateTime?
        'End RWMS-1497
        SQL = String.Format("Select * from WHACTIVITY where userid = '{0}'", UserId)
        DataInterface.FillDataset(SQL, dtHE)
        If dtHE.Rows.Count >= 1 Then
            userMHType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtHE.Rows(0)("hetype"), "%")
            'RWMS-1497
            If Not dtHE.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
                whPreviousActivityTime = dtHE.Rows(0)("PREVIOUSACTIVITYTIME")
            End If
            'End RWMS-1497
        End If
        _taskPolicies = New TaskPolicies(UserId, oLogger)
        Dim dtAvailTasks As DataTable = GetUserAvailableTasks(UserId, oLogger)
        If Not oLogger Is Nothing Then
            oLogger.Write(_taskPolicies.Count & " Task Policies were Loaded successfuly...")
            oLogger.Write(dtAvailTasks.Rows.Count & " Available Tasks were Loaded successfuly from USERAVAILABLETASKS View...")
            oLogger.Write("Trying to find a task by policies and user skill...")
            oLogger.writeSeperator()
        End If
        Try
            _task = _taskPolicies.FindTask(dtAvailTasks, UserId, userMHType, oLogger, IsGetTaskFromUser)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator()
                oLogger.Write("Error ocuured: " & ex.ToString)
                oLogger.writeSeperator()
            End If
        End Try
        If Simulate = 1 Then
            If Not _task Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(_task.TASK & " was found.Assigning to user...")
                    oLogger.writeSeperator()
                End If
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("No Task was found for user!")
                    oLogger.writeSeperator()
                End If
            End If
            Return Nothing
        Else
            If Not _task Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(_task.TASK & " was found.Assigning to user...")
                    oLogger.writeSeperator()
                End If
                If TaskAssignmentTypeAuto Then
                    AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.AUTOMATIC, userMHType)
                Else
                    AssignUser(UserId, WMS.Lib.TASKASSIGNTYPE.MANUAL, userMHType)
                End If


                'Added for RWMS-1497
                If whPreviousActivityTime.HasValue Then
                    _task.SetStartTime(whPreviousActivityTime)
                End If

                'End RWMS-1497
                Dim CurrentLoc As DataTable = New DataTable
                DataInterface.FillDataset("SELECT ISNULL(L.LOCATION, 'EMPTY') AS LOCATION,  ISNULL(L.WAREHOUSEAREA, 'EMPTY') as WAREHOUSEAREA, ISNULL(XCOORDINATE,0) AS X,ISNULL(YCOORDINATE,0) AS Y, ISNULL(ZCOORDINATE,0) AS Z, ISNULL(W.ACTIVITY, 'EMPTY') as ACTIVITY FROM WHACTIVITY W LEFT OUTER JOIN LOCATION L ON W.LOCATION=L.LOCATION and W.WAREHOUSEAREA=L.WAREHOUSEAREA WHERE W.USERID='" & UserId & "'", CurrentLoc)
                If CurrentLoc.Rows(0)("LOCATION") <> "EMPTY" Then
                    ' Fetch Location
                    Try
                        Dim sqlLoc As [String] = "Select * from LOCATION where LOCATION = '{0}'"
                        Dim locationsFrom As New DataTable()
                        DataInterface.FillDataset([String].Format(sqlLoc, CurrentLoc.Rows(0)("LOCATION")), locationsFrom)
                        Dim from As DataRow = locationsFrom.Rows(0)
                        Dim locationsTo As New DataTable()
                        DataInterface.FillDataset([String].Format(sqlLoc, _task.FROMLOCATION), locationsTo)
                        Dim [to] As DataRow = locationsTo.Rows(0)

                        Dim operatorsEqpHeight As Double = 0
                        Dim path As Path
                        Dim _sp As IShortestPathProvider = ShortestPath.GetInstance()

                        'fetch operators equipment height
                        If Not String.IsNullOrEmpty(UserId) Then
                            operatorsEqpHeight = GetOperatorsEquipmentHeight(UserId)
                        End If
                        'fetch operators equipment height

                        'Make rules
                        Dim rulesWithHeightAndUnidirection As List(Of Rules) = New List(Of Rules)()

                        Dim rulesWithHeightOnly As List(Of Rules) = New List(Of Rules)()

                        Dim ruleHeight As Rules = New Rules()

                        ruleHeight.Parameter = Made4Net.Algorithms.Constants.Height
                        ruleHeight.Data = operatorsEqpHeight
                        ruleHeight.Operator = ">"

                        Dim ruleUnidirection As Rules = New Rules()

                        ruleUnidirection.Parameter = Made4Net.Algorithms.Constants.Equipment
                        ruleUnidirection.Operator = Made4Net.Algorithms.Constants.UniDirection

                        rulesWithHeightAndUnidirection.Add(ruleHeight)
                        rulesWithHeightAndUnidirection.Add(ruleUnidirection)

                        rulesWithHeightOnly.Add(ruleHeight)
                        'Make rules

                        ''''''''''''''''''''' RWMS-2240 : Temporary Disabling Height
                        'If _task.TASKTYPE = "PARPICK" Then
                        '    'Unidirection & Height
                        '    path = _sp.GetShortestPathWithContsraints(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), _task.TASKTYPE, True, rulesWithHeightAndUnidirection)
                        'Else
                        '    'Only Height
                        '    path = _sp.GetShortestPathWithContsraints(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), _task.TASKTYPE, True, rulesWithHeightOnly)
                        'End If
                        If _task.TASKTYPE = "PARPICK" Then
                            'Unidirection
                            Dim rulesUnidirection As List(Of Rules) = New List(Of Rules)()
                            rulesUnidirection.Add(ruleUnidirection)
                            path = _sp.GetShortestPathWithContsraints(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), _task.TASKTYPE, True, rulesUnidirection, GetShortestPathLogger(oLogger))
                        Else
                            'Unconstrained
                            path = _sp.GetShortestPath(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), _task.TASKTYPE, True)
                        End If

                        ''''''''''''''''' RWMS-2240 : Temporary Disabling Height

                        ' Print the path
                        If path.Distance.SourceToTargetLocation > 0 Then
                            oLogger.Write("Node to Node Traversed Distance")
                            oLogger.writeSeperator("-", 80)

                            For Each item As KeyValuePair(Of String, Double) In path.TraversedNodes
                                oLogger.Write([String].Format("Node : {0} -- Distance : {1}", item.Key, item.Value))
                            Next
                            oLogger.writeSeperator("-", 80)
                        End If
                    Catch ex As Exception
                        If oLogger IsNot Nothing Then
                            oLogger.Write("GetShortestPathWithContsraints threw exception, using the values returened by USERAVAILABLETASKS view. Cannot display the walk nodes.")
                        End If
                    End Try
                End If
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("No Task was found for user!")
                    oLogger.writeSeperator()
                End If
            End If
            Return _task
        End If
    End Function

    Public Function GetUserAvailableTasks(ByVal UserId As String, Optional ByVal oLogger As LogHandler = Nothing) As DataTable
        Dim dtTasks As New DataTable
        'Commented For RWMS-295
        'Dim SQL As String = String.Format("select * from USERAVAILABLETASKS where username = '{0}'", UserId)
        'End Commented For RWMS-295
        'Added For RWMS-295
        Dim objpriorityreset As Prioritize = New PrioritizeReplenishments
        Dim ReqTaskPriority As Integer = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_PENDING))

        Dim SQL As String = String.Format("SELECT * from USERAVAILABLETASKS WITH(NOLOCK) Where username = '{0}' " +
                                          " AND (TaskType <>'NEGTREPL' OR ( TaskType = 'NEGTREPL' AND Priority NOT IN ({1},0))) ORDER BY PRIORITY DESC ", UserId, ReqTaskPriority.ToString())


        ' End Added For RWMS-295
        If oLogger IsNot Nothing Then
            oLogger.Write("GetUserAvailableTasks: sql={0}", SQL)
        End If
        DataInterface.FillDataset(SQL, dtTasks)
        Return dtTasks
    End Function

    Private Function GetOperatorsEquipmentHeight(user As [String]) As Double
        Dim sql As [String] = [String].Format("select  COALESCE(HANDLINGEQUIPMENT.HEIGHT, 0) as HEIGHT from HANDLINGEQUIPMENT inner join WHACTIVITY on HANDLINGEQUIPMENT.HANDLINGEQUIPMENT = WHACTIVITY.HETYPE where WHACTIVITY.USERID = '{0}'", user)
        Dim dt As New DataTable()
        DataInterface.FillDataset(sql, dt, False, "")
        If dt.Rows.Count > 0 Then
            Return Convert.ToDouble(dt.Rows(0)("HEIGHT"))
        End If
        Return 0
    End Function

    Public Function GetTaskFromTMService(ByVal UserId As String, Optional ByVal IsGetTaskFromUser As Boolean = False, Optional ByVal logger As ILogHandler = Nothing) As Task
        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        oQ.Add("UserId", UserId)
        'oQ.Add("PickList", PickListId)
        If (IsGetTaskFromUser = True) Then
            oQ.Add("IsGetTaskFromUser", "1")
        Else
            oQ.Add("IsGetTaskFromUser", "0")
        End If

        oQ.Add("SIMULATE", "0")
        oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse())

        logger.SafeWrite("Sending message to task manager...")
        oQ.Values.WriteToLog(logger, "   ")

        oMsg = oQ.Send("TaskManager")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)

        logger.SafeWrite("Response received from TaskManager...")
        qm.Values.WriteToLog(logger, "   ")

        If qm.Values("TASKID") <> "" Then
            _task = New Task(qm.Values("TASKID"))
            logger.SafeWrite("Task ID found: {0}", qm.Values("TASKID"))
            Return _task
        Else
            logger.SafeWrite("No task found")
            Return Nothing
        End If
    End Function


    Public Shared Sub GetFinalDestinationLocation(ByVal pUserId As String, _
         ByRef pSourceLocation As String, ByRef pDestinationLocation As String, _
         ByRef pSourceWarehousearea As String, ByRef pDestinationWarehousearea As String, _
         Optional ByVal pHUType As String = "")

        'RWMS-2646 RWMS-2645
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2646 RWMS-2645 END

        Dim SQL, userMHType As String
        If pDestinationLocation = "" Or pDestinationWarehousearea = "" Then Return

        'Get the Current user MHType
        Dim dtHE As New DataTable
        SQL = String.Format("Select * from WHACTIVITY where userid = '{0}' ", pUserId)

        'RWMS-2646 RWMS-2645
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" sql:{0}", SQL))
        End If
        'RWMS-2646 RWMS-2645 END

        DataInterface.FillDataset(SQL, dtHE)
        If dtHE.Rows.Count >= 1 Then
            userMHType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtHE.Rows(0)("hetype"), "%")

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" userMHType:{0}", userMHType))
            End If
            'RWMS-2646 RWMS-2645 END

        Else
            userMHType = ""

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" userMHType:{0}", userMHType))
            End If
            'RWMS-2646 RWMS-2645 END

        End If
        dtHE.Dispose()
        'Check If we can access to destination Location

        'RWMS-2646 RWMS-2645
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" Check If we can access to Location "))
            wmsrdtLogger.Write(String.Format(" userMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, pDestinationLocation, pDestinationWarehousearea))
        End If
        'RWMS-2646 RWMS-2645 END

        If CanAccessByMHType(userMHType, pDestinationLocation, pDestinationWarehousearea) Then

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" CanAccessByMHType: YES. userMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, pDestinationLocation, pDestinationWarehousearea))
            End If
            'RWMS-2646 RWMS-2645 END

            Return
        Else
            'start searching for a handoff location

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" CanAccessByMHType: NO. UserMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, pDestinationLocation, pDestinationWarehousearea))
                wmsrdtLogger.Write(String.Format(" start searching for a handoff location"))
                wmsrdtLogger.Write(String.Format(" SourceLocation {0},SourceWarehousearea {1}", pSourceLocation, pSourceWarehousearea))
            End If
            'RWMS-2646 RWMS-2645 END

            'Commented for RWMS-2645
            'Dim oSrcLoc As New Location(pSourceLocation, pSourceWarehousearea)
            'Dim oDestLoc As New Location(pDestinationLocation, pDestinationWarehousearea)
            'Commented for RWMS-2645 END

            'RWMS-2646 RWMS-2645 START
            Dim oSrcLoc As New Location
            Dim oDestLoc As New Location

            If pSourceWarehousearea Is Nothing Or pSourceWarehousearea = "" Then
                oSrcLoc = New Location(pSourceLocation)
                oDestLoc = New Location(pDestinationLocation)
            Else
                oSrcLoc = New Location(pSourceLocation, pSourceWarehousearea)
                oDestLoc = New Location(pDestinationLocation, pDestinationWarehousearea)
            End If
            'RWMS-2646 RWMS-2645 END

            Dim dtHandOffs As New DataTable
            SQL = String.Format("select * from vHandOff where '{0}' like FROMHANDOFFREGION  and '{1}' like TOHANDOFFREGION and status = 1 and (loadscapacity-numloads) >= 1  order by priority", _
                oSrcLoc.OUTHANDOFF, oDestLoc.INHANDOFF)

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" sql:{0}", SQL))
            End If
            'RWMS-2646 RWMS-2645 END

            DataInterface.FillDataset(SQL, dtHandOffs)
            'If dtHandOffs.Rows.Count = 0 Then
            pDestinationLocation = pSourceLocation
            pDestinationWarehousearea = pSourceWarehousearea

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Location : {0}, Warehousearea : {1}", pDestinationLocation, pDestinationWarehousearea))
            End If
            'RWMS-2646 RWMS-2645 END

            'End If
            For Each dr As DataRow In dtHandOffs.Rows
                Dim tmpLocation As String = dr("handofflocation")
                Dim tmpWarehousearea As String = dr("handoffwarehousearea")

                'RWMS-2646 RWMS-2645
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" HandoffLocation Found: {0}, HandoffWarehousearea Found: {1}", tmpLocation, tmpWarehousearea))
                End If
                'RWMS-2646 RWMS-2645 END

                'Commented for RWMS-2645
                'Dim oLoc As New Location(tmpLocation, tmpWarehousearea)
                'Commented for RWMS-2645 END

                'RWMS-2646 RWMS-2645 START
                Dim oLoc As New Location

                If tmpWarehousearea Is Nothing Or tmpWarehousearea = "" Then
                    oLoc = New Location(tmpLocation)
                Else
                    oLoc = New Location(tmpLocation, tmpWarehousearea)
                End If
                'RWMS-2646 RWMS-2645 END

                'RWMS-2646 RWMS-2645
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" Check If we can access to Location "))
                    wmsrdtLogger.Write(String.Format(" userMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, tmpLocation, tmpWarehousearea))
                End If
                'RWMS-2646 RWMS-2645 END

                If CanAccessByMHType(userMHType, tmpLocation, tmpWarehousearea) Then

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" CanAccessByMHType: YES. userMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, tmpLocation, tmpWarehousearea))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    If pHUType = String.Empty Then
                        pDestinationLocation = tmpLocation
                        pDestinationWarehousearea = tmpWarehousearea
                        Exit For
                    ElseIf oLoc.HUSTORAGETEMPLATE = String.Empty Then
                        pDestinationLocation = tmpLocation
                        pDestinationWarehousearea = tmpWarehousearea
                        Exit For
                    ElseIf WMS.Logic.Location.ValidateLocationHU(oLoc.HUSTORAGETEMPLATE, oLoc.Location, oLoc.WAREHOUSEAREA, pHUType, Nothing, Nothing) Then
                        pDestinationLocation = tmpLocation
                        pDestinationWarehousearea = tmpWarehousearea
                        Exit For
                    End If

                    'RWMS-2646 RWMS-2645
                Else
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" CanAccessByMHType: NO. UserMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, tmpLocation, tmpWarehousearea))
                    End If
                    'RWMS-2646 RWMS-2645 END
                End If
            Next
        End If

    End Sub

    Public Shared Function CanAccessByMHType(ByVal userMHType As String, ByVal pLocation As String, ByVal pWarehousearea As String) As Boolean

        'RWMS-2646 RWMS-2645
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2646 RWMS-2645 END

        Dim CanGetJob As Boolean = False
        Dim query As String
        If pLocation = "" Or pWarehousearea = "" Then Return False
        Dim oLoc As New Location(pLocation, pWarehousearea)
        If oLoc.LOCMHTYPE = "" Then Return True
        Dim sql As String = " select MHCTYPE from hetype where HANDLINGEQUIPMENT like '" & userMHType & "' and ACCESS = 1"
        Dim dt As New DataTable

        'RWMS-2646 RWMS-2645
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" sql:{0}", sql))
        End If
        'RWMS-2646 RWMS-2645 END

        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            If Convert.ToString(dr("MHCTYPE")).ToLower.Trim() = oLoc.LOCMHTYPE.ToLower.Trim() Then
                CanGetJob = True
                Exit For
            End If
        Next
        dt.Dispose()

        'RWMS-2645
        If Not wmsrdtLogger Is Nothing Then
            If CanGetJob Then
                wmsrdtLogger.Write(String.Format(" CanAccessByMHType: True. Can get the job"))
                wmsrdtLogger.Write(String.Format(" UserMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, pLocation, pWarehousearea))
            Else
                wmsrdtLogger.Write(String.Format(" CanAccessByMHType: False. Can't get the job"))
                wmsrdtLogger.Write(String.Format(" UserMHType: {0}, Location: {1}, Warehousearea: {2}", userMHType, pLocation, pWarehousearea))
            End If

        End If
        'RWMS-2645 END

        Return CanGetJob
    End Function

#End Region

#End Region

End Class