Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ReplenishmentTask
    Inherits Task

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub Replenish(ByVal repl As WMS.Logic.Replenishment, ByVal oReplJob As ReplenishmentJob, ByVal pUser As String, Optional ByVal pCreatePWTask As Boolean = True)
        repl.Replenish(oReplJob.toLocation, oReplJob.toWarehousearea, pUser, pCreatePWTask)
        If oReplJob.OnContainer Then
            Dim oLoad As New Load(oReplJob.fromLoad)
            If Not oReplJob.IsHandOff Then
                ' Lets get loads container
                Dim ContainerSql As String = "SELECT HANDLINGUNIT FROM INVLOAD WHERE LOADID='" & oReplJob.fromLoad & "'"
                Dim CntId As String = Convert.ToString(DataInterface.ExecuteScalar(ContainerSql))
                oLoad.RemoveFromContainer()
                If DataInterface.ExecuteScalar("SELECT COUNT(1) FROM INVLOAD WHERE HANDLINGUNIT='" & CntId & "'") = 0 Then
                    ' This is the last load on this container , lets finish the task of the container
                    Dim ContainerTaskIdSQL As String = "SELECT TASK FROM TASKS WHERE STATUS='ASSIGNED' AND FROMCONTAINER='" & CntId & "' AND USERID='" & pUser & "'"
                    Dim dt As DataTable = New DataTable
                    DataInterface.FillDataset(ContainerTaskIdSQL, dt)
                    If dt.Rows.Count > 0 Then
                        Dim ContTask As Task = New Task(Convert.ToString(dt.Rows(0)("TASK")))
                        ContTask.Complete(Nothing)
                    End If
                End If

            End If
        End If
        _executionlocation = oReplJob.toLocation
        _executionwarehousearea = oReplJob.toWarehousearea
        'Commented for RWMS-469
        'Me.Complete()
        'This is an handoff situation - need to create a new task from the handoff to the original destination
        'If oReplJob.toLocation <> _tolocation And oReplJob.toWarehousearea <> _towarehousearea And Not oReplJob.OnContainer Then
        If (oReplJob.toLocation <> _tolocation OrElse oReplJob.toWarehousearea <> _towarehousearea) And Not oReplJob.OnContainer Then
            _task = ""
            If repl.ReplType = Logic.Replenishment.ReplenishmentTypes.PartialReplenishment Then
                _fromload = repl.ToLoad
            End If
            _executionlocation = String.Empty
            _executionwarehousearea = String.Empty
            _fromlocation = oReplJob.toLocation
            _fromwarehousearea = oReplJob.toWarehousearea
            Me.Post()
        End If
    End Sub

    Public Sub SubtitueLoad(ByVal newLoadid As String, ByRef repl As WMS.Logic.Replenishment, ByVal oReplJob As ReplenishmentJob, ByVal pUser As String)
        If oReplJob.TaskType <> WMS.Lib.TASKTYPE.FULLREPL Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Replenishment Type - Cannot subtitute Load", "Incorrect Replenishment Type - Cannot subtitute Load")
        End If
        Dim ld As Load
        Dim oldLoad As Load
        If WMS.Logic.Load.Exists(newLoadid) Then
            ld = New Load(newLoadid)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
        End If

        oldLoad = New Load(oReplJob.fromLoad)

        'Added for PWMS-302
        Dim assigned As Boolean = False
        Dim assignedUserName As String = String.Empty
        Dim sqlAssigned As String = String.Format("SELECT ASSIGNED,ISNULL(USERID,'') as AssignedUserName FROM TASKS WHERE STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND FROMLOAD = '{0}' AND FROMLOCATION='{1}'", ld.LOADID, ld.LOCATION)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sqlAssigned, dt)

        If (dt.Rows.Count > 0) Then

            dr = dt.Rows(0)
            assigned = dr.Item("ASSIGNED")
            assignedUserName = dr.Item("AssignedUserName")

            If assigned = True Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Substitue load associated Task is assigned to User - " & assignedUserName, "Substitue load associated Task is assigned to User - " & assignedUserName)
            End If
        End If
        'End added for PWMS-302

        If oReplJob.Consignee <> ld.CONSIGNEE Or ld.SKU <> oReplJob.Sku Or oReplJob.UOM <> ld.LOADUOM Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Sku does not match", "Sku does not match")
        End If

        'Commented for RWMS-1989 and RWMS-1946 Start
        ''Added for PWMS-289
        'If Not ld.ACTIVITYSTATUS = "" Then
        ' 'End Added For PWMS-289
        'Commented for RWMS-1989 and RWMS-1946 End


        'Commented for RWMS-1989 and RWMS-1946 Start
        ''Added for PWMS-289
        'End If
        ''End Added For PWMS-289
        'Commented for RWMS-1989 and RWMS-1946 End
        'Added for RWMS-1989 and RWMS-1946 Start
        If oReplJob.Units > ld.UNITS Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Load Units short", "Load Units short")
        End If
        If oldLoad.UNITS <> ld.UNITS Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Loads Units Does Not Match", "Loads Units Does Not Match")
        End If
        'Commented for RWMS-1946 Start
        ''Added for PWMS-289
        'End If
        ''End Added For PWMS-289
        'Ended for RWMS-1989 and RWMS-1946 Start


        If ld.UNITSALLOCATED > 0 And ld.ACTIVITYSTATUS.ToUpper = "REPLPEND" Then

            '1) allocate the new load with the repl qty

            'ld.Allocate(oReplJob.Units, WMS.Lib.USERS.SYSTEMUSER)
            'ld.SetDestinationLocation(oldLoad.DESTINATIONLOCATION, oldLoad.DESTINATIONWAREHOUSEAREA, WMS.Lib.USERS.SYSTEMUSER)
            'ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.REPLPENDING, WMS.Lib.USERS.SYSTEMUSER)

            'and unallocate the old load
            'oldLoad.unAllocate(oReplJob.Units, WMS.Lib.USERS.SYSTEMUSER)
            'oldLoad.SetDestinationLocation("", "", WMS.Lib.USERS.SYSTEMUSER)
            'oldLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Lib.USERS.SYSTEMUSER)

            '2) update the Replenishment
            repl.ChangeLoad(ld, oldLoad, WMS.Lib.USERS.SYSTEMUSER)
            '3) update repl job and repltask
            oReplJob.fromLoad = ld.LOADID
            oReplJob.fromLocation = ld.LOCATION
            oReplJob.fromWarehousearea = ld.WAREHOUSEAREA
            _fromload = ld.LOADID
            'Added for RWMS-167
            _fromlocation = ld.LOCATION
            'End Added for RWMS-167
            If _task_type = WMS.Lib.TASKTYPE.FULLREPL Then
                _toload = ld.LOADID
            End If

        ElseIf ld.UNITSALLOCATED > 0 And ld.ACTIVITYSTATUS = String.Empty Then
            '1) allocate the new load with the repl qty

            'ld.Allocate(oReplJob.Units, WMS.Lib.USERS.SYSTEMUSER)
            'ld.SetDestinationLocation(oldLoad.DESTINATIONLOCATION, oldLoad.DESTINATIONWAREHOUSEAREA, WMS.Lib.USERS.SYSTEMUSER)
            'ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.REPLPENDING, WMS.Lib.USERS.SYSTEMUSER)

            'and unallocate the old load
            'oldLoad.unAllocate(oReplJob.Units, WMS.Lib.USERS.SYSTEMUSER)
            'oldLoad.SetDestinationLocation("", "", WMS.Lib.USERS.SYSTEMUSER)
            'oldLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Lib.USERS.SYSTEMUSER)

            '2) update the Replenishment
            repl.ChangeLoad(ld, oldLoad, WMS.Lib.USERS.SYSTEMUSER)
            '3) update repl job and repltask
            oReplJob.fromLoad = ld.LOADID
            oReplJob.fromLocation = ld.LOCATION
            oReplJob.fromWarehousearea = ld.WAREHOUSEAREA
            _fromload = ld.LOADID

            'Added for RWMS-167
            _fromlocation = ld.LOCATION
            'End Added for RWMS-167
            If _task_type = WMS.Lib.TASKTYPE.FULLREPL Then
                _toload = ld.LOADID
            End If


        ElseIf ld.ACTIVITYSTATUS = String.Empty Or ld.ACTIVITYSTATUS = "" Then

            '1) allocate the new load with the repl qty
            ld.Allocate(oReplJob.Units, WMS.Lib.USERS.SYSTEMUSER)
            ld.SetDestinationLocation(oldLoad.DESTINATIONLOCATION, oldLoad.DESTINATIONWAREHOUSEAREA, WMS.Lib.USERS.SYSTEMUSER)
            ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.REPLPENDING, WMS.Lib.USERS.SYSTEMUSER)
            'and unallocate the old load
            oldLoad.unAllocate(oReplJob.Units, WMS.Lib.USERS.SYSTEMUSER)
            oldLoad.SetDestinationLocation("", "", WMS.Lib.USERS.SYSTEMUSER)
            oldLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Lib.USERS.SYSTEMUSER)
            '2) update the Replenishment
            repl.ChangeLoad(ld, WMS.Lib.USERS.SYSTEMUSER)
            '3) update repl job and repltask
            oReplJob.fromLoad = ld.LOADID
            oReplJob.fromLocation = ld.LOCATION
            oReplJob.fromWarehousearea = ld.WAREHOUSEAREA
            _fromload = ld.LOADID

            'Added for RWMS-167
            _fromlocation = ld.LOCATION
            'End Added for RWMS-167

            If _task_type = WMS.Lib.TASKTYPE.FULLREPL Then
                _toload = ld.LOADID
            End If


        End If
        Save()
    End Sub

    'End Added For RWMS-167

    Public Function getReplenishmentJob(ByVal oRepl As WMS.Logic.Replenishment) As ReplenishmentJob
        Dim rjob As New ReplenishmentJob
        rjob.IsHandOff = False
        rjob.Consignee = _consignee
        rjob.Sku = _sku
        rjob.fromLoad = _fromload
        rjob.Units = oRepl.Units
        rjob.UOM = oRepl.UOM
        rjob.Replenishment = oRepl.ReplId
        Try
            Dim sku As New SKU(_consignee, _sku)
            rjob.skuDesc = sku.SKUDESC
            rjob.UOMUnits = sku.ConvertUnitsToUom(oRepl.UOM, oRepl.Units)
        Catch ex As Exception
            rjob.UOMUnits = rjob.Units
        End Try
        rjob.toLocation = _tolocation
        rjob.fromLocation = _fromlocation
        rjob.toWarehousearea = _towarehousearea
        rjob.fromWarehousearea = _fromwarehousearea
        rjob.TaskId = _task
        rjob.TaskType = _task_type
        'Now check if the user has the access to the destination location of the replenishment job
        Dim origDestLocation As String = rjob.toLocation
        Dim origDestWarehousearea As String = rjob.toWarehousearea
        TaskManager.GetFinalDestinationLocation(USERID, _fromlocation, rjob.toLocation, _fromwarehousearea, rjob.toWarehousearea)
        If origDestLocation <> rjob.toLocation Then
            rjob.IsHandOff = True
        End If
        ' And set the task start time

        'RWMS-2926
        MyBase.SetStartTime()

        'RWMS-2932
        Dim oWHActivity As New WHActivity
        oWHActivity.ACTIVITY = _task_type
        oWHActivity.LOCATION = _fromlocation
        oWHActivity.WAREHOUSEAREA = _executionwarehousearea
        oWHActivity.USERID = _edituser
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.ADDDATE = DateTime.Now
        oWHActivity.EDITDATE = DateTime.Now
        oWHActivity.ADDUSER = _edituser
        oWHActivity.EDITUSER = _edituser
        oWHActivity.Post()
        'RWMS-2932


        Return rjob
    End Function

    'This function will get a replenishment job which is not based on a task, but on a regular replenishment
    Public Shared Function getReplenishmentJob(ByVal pLoadid As String, ByVal pUserId As String)
        Dim SQL As String = String.Format("select * from replenishment where fromload = '{0}' and status = '{1}'", pLoadid, WMS.Lib.Statuses.Replenishment.PLANNED)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "No Replenishment was found", "No Replenishment was found")
        End If
        Dim oLoad As New Load(pLoadid)
        Dim oRepl As New Replenishment(dt.Rows(0)("replid"))
        Dim rjob As New ReplenishmentJob
        rjob.OnContainer = True
        rjob.IsHandOff = False
        rjob.Consignee = oLoad.CONSIGNEE
        rjob.Sku = oLoad.SKU
        rjob.fromLoad = pLoadid
        rjob.Units = oRepl.Units
        rjob.UOM = oRepl.UOM
        rjob.Replenishment = oRepl.ReplId
        Try
            Dim sku As New SKU(oLoad.CONSIGNEE, oLoad.SKU)
            rjob.skuDesc = sku.SKUDESC
            rjob.UOMUnits = sku.ConvertUnitsToUom(oRepl.UOM, oRepl.Units)
        Catch ex As Exception
            rjob.UOMUnits = rjob.Units
        End Try
        rjob.toLocation = oRepl.ToLocation
        rjob.fromLocation = oRepl.FromLocation
        rjob.toWarehousearea = oRepl.ToWarehousearea
        rjob.fromWarehousearea = oRepl.FromWarehousearea
        rjob.TaskId = ""
        rjob.TaskType = WMS.Lib.TASKTYPE.FULLREPL
        'Now check if the user has the access to the destination location of the replenishment job
        Dim origDestLocation As String = rjob.toLocation
        TaskManager.GetFinalDestinationLocation(pUserId, oRepl.FromLocation, rjob.toLocation, oRepl.FromWarehousearea, rjob.toWarehousearea)
        If origDestLocation <> rjob.toLocation Then
            rjob.IsHandOff = True
        End If
        Return rjob
    End Function

    Public Overrides Sub Cancel()
        Dim oRepl As New Replenishment(_replenishment)
        If oRepl.Status = WMS.Lib.Statuses.Replenishment.PLANNED Then
            oRepl.Cancel(WMS.Logic.GetCurrentUser)
        End If

        Dim oBRepl As New BatchReplenishment(_replenishment)
        If oBRepl.Status = WMS.Lib.Statuses.BatchReplensishment.RELEASED Then
            oBRepl.Cancel(WMS.Logic.GetCurrentUser)
        ElseIf oBRepl.Status = WMS.Lib.Statuses.BatchReplensishment.LETDOWN Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot cancel Batch Replenishment, Incorrect Status", "Cannot cancel Batch Replenishment, Incorrect Status")
        ElseIf oBRepl.Status = WMS.Lib.Statuses.BatchReplensishment.UNLOAD Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot cancel Batch Replenishment, Incorrect Status", "Cannot cancel Batch Replenishment, Incorrect Status")
        End If

        MyBase.Cancel()
    End Sub

    Public Shadows Sub ReportProblem(ByVal pRepl As Replenishment, ByVal pProblemCodeId As String, ByVal pLocation As String, ByVal pWHArea As String, ByVal pUserId As String)
        MyBase.ReportProblem(pProblemCodeId, pLocation, pWHArea, pUserId)
        If CompleteTaskOnProblemCode Then
            pRepl.Cancel(pUserId)
        End If
    End Sub

End Class

#Region "Replenishment Job"

<CLSCompliant(False)> Public Class ReplenishmentJob
    Public TaskId As String
    Public TaskType As String
    Public Replenishment As String
    Public Consignee As String
    Public Sku As String
    Public skuDesc As String
    Public fromLocation As String
    Public fromWarehousearea As String
    Public fromLoad As String
    Public toLocation As String
    Public toWarehousearea As String
    Public Units As Double
    Public UOM As String
    Public UOMUnits As Double
    Public IsHandOff As Boolean
    Public OnContainer As Boolean
End Class

#End Region