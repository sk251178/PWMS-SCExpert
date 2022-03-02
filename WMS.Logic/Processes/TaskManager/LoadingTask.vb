Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class LoadingTask
    Inherits Task

#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

#End Region

#Region "Create"

    Public Sub CreateLoadLoadingTask(ByVal pLoadingJob As LoadingJob, ByVal puser As String)
        Dim sql As String
        Dim taskid As String
        sql = String.Format("Select task from TASKS where fromload = '{0}' and status not in('{1}','{2}') and tasktype = '{3}'", pLoadingJob.LoadId, WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED, WMS.Lib.TASKTYPE.LOADLOADING)
        taskid = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If taskid <> "" Then
            'Assign user
            _task = taskid
            MyBase.AssignUser(puser)
            ' And set the task start time
            MyBase.SetStartTime()
            Return
        End If
        _consignee = pLoadingJob.Consignee
        _fromload = pLoadingJob.LoadId
        _fromlocation = pLoadingJob.FromLocation
        _fromwarehousearea = pLoadingJob.FromWarehousearea
        _priority = 200
        _sku = pLoadingJob.Sku
        _task_type = WMS.Lib.TASKTYPE.LOADLOADING
        _toload = pLoadingJob.LoadId
        _tolocation = pLoadingJob.Door
        _towarehousearea = pLoadingJob.DoorWarehousearea
        _assignedtime = DateTime.Now
        MyBase.Post(puser, Nothing)
    End Sub

    Public Sub CreateContainerLoadingTask(ByVal pLoadingJob As LoadingJob, ByVal puser As String)
        Dim sql As String
        Dim taskid As String
        sql = String.Format("Select task from TASKS where fromcontainer = '{0}' and status not in('{1}','{2}') and tasktype = '{3}'", pLoadingJob.LoadId, WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED, WMS.Lib.TASKTYPE.CONTLOADING)
        taskid = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If taskid <> "" Then
            'Assign user
            _task = taskid
            MyBase.AssignUser(puser)
            ' And set the task start time
            MyBase.SetStartTime()
            Return
        End If
        _fromcontainer = pLoadingJob.LoadId
        'Added for RWMS-1462 and RWMS-1194   
        _fromlocation = pLoadingJob.FromLocation
        'Ended for RWMS-1462 and RWMS-1194  
        _priority = 200
        _task_type = WMS.Lib.TASKTYPE.CONTLOADING
        _tocontainer = pLoadingJob.LoadId
        _tolocation = pLoadingJob.Door
        _towarehousearea = pLoadingJob.DoorWarehousearea
        _assignedtime = DateTime.Now
        MyBase.Post(puser, Nothing)
    End Sub
    Public Sub CreateCaseLoadingTask(ByVal pLoadingJob As LoadingJob, ByVal puser As String)
        Dim sql As String
        Dim taskid As String
        sql = String.Format("Select task from TASKS where caseid = '{0}' and status not in('{1}','{2}') and tasktype = '{3}'", pLoadingJob.LoadId, WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED, WMS.Lib.TASKTYPE.CASELOADING)
        taskid = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If taskid <> "" Then
            _task = taskid
            MyBase.AssignUser(puser)
            MyBase.SetStartTime()
            Return
        End If
        Dim oCase As New WMS.Logic.CaseDetail(pLoadingJob.LoadId)
        _consignee = pLoadingJob.Consignee
        _caseid = oCase.CaseID
        _fromload = oCase.ToLoad
        _fromcontainer = oCase.ToContainer
        _fromlocation = pLoadingJob.FromLocation
        _fromwarehousearea = pLoadingJob.FromWarehousearea
        _priority = 200
        _sku = pLoadingJob.Sku
        _task_type = WMS.Lib.TASKTYPE.CASELOADING
        _toload = oCase.ToLoad
        _tocontainer = oCase.ToContainer
        _tolocation = pLoadingJob.Door
        _towarehousearea = pLoadingJob.DoorWarehousearea
        _assignedtime = DateTime.Now
        MyBase.Post(puser, Nothing)
    End Sub

#End Region

#Region "Complete & Exit"

    Public Sub CompleteTask(ByVal pLoadingJob As LoadingJob, ByVal pUser As String)
        Dim sql As String
        Dim dt As New DataTable
        sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and status = '{1}' and (tasktype = '{2}' or tasktype = '{3}' or tasktype = '{4}')", pUser, WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.CONTLOADING, WMS.Lib.TASKTYPE.LOADLOADING, WMS.Lib.TASKTYPE.CASELOADING)
        If pLoadingJob.IsCase Then
            sql = sql + String.Format(" And caseid = '{0}'", pLoadingJob.LoadId)
        ElseIf pLoadingJob.IsContainer Then
            sql = sql + String.Format(" And fromcontainer = '{0}'", pLoadingJob.LoadId)
        Else
            sql = sql + String.Format(" And fromload = '{0}'", pLoadingJob.LoadId)
        End If
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            _task = dt.Rows(0)("task")
            Load()
            _executionlocation = pLoadingJob.ToLocation
            ' RWMS-1697
            Dim oWHActivity As New WHActivity
            oWHActivity.USERID = pUser
            oWHActivity.SaveLastLocation()
            ' RWMS-1697
            Complete(Nothing)
        End If
    End Sub

    Public Sub ReleaseTask(ByVal pLoadingJob As LoadingJob, ByVal pUser As String)
        Dim sql As String
        Dim dt As New DataTable
        sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and status = '{1}' and (tasktype = '{2}' or tasktype = '{3}' or tasktype = '{4}')", pUser, WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.CONTLOADING, WMS.Lib.TASKTYPE.LOADLOADING, WMS.Lib.TASKTYPE.CASELOADING)
        If pLoadingJob.IsCase Then
            sql = sql + String.Format(" And caseid = '{0}'", pLoadingJob.LoadId)
        ElseIf pLoadingJob.IsContainer Then
            sql = sql + String.Format(" And fromcontainer = '{0}'", pLoadingJob.LoadId)
        Else
            sql = sql + String.Format(" And fromload = '{0}'", pLoadingJob.LoadId)
        End If
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            _task = dt.Rows(0)("task")
            Load()
            ExitTask()
        End If
    End Sub

#End Region

#End Region

End Class

#Region "Loading Job"

<CLSCompliant(False)> Public Class LoadingJob
    Public IsContainer As String
    Public TaskId As String
    Public LoadId As String
    Public IsCase As String
    Public Consignee As String
    Public Sku As String
    Public SkuDesc As String
    Public Picklist As String
    Public OrderId As String
    Public OrderLine As String
    Public FromLocation As String
    Public ToLocation As String
    Public FromWarehousearea As String
    Public ToWarehousearea As String
    Public Units As Double
    Public UOM As String
    Public UOMUnits As Double
    Public OrderSeq As Int32
    Public Comapny As String
    Public ComapnyName As String
    Public Shipment As String
    Public Carrier As String
    Public CarrierName As String
    Public RequestedDate As DateTime
    Public Door As String
    Public DoorWarehousearea As String
    Public Vehicle As String
    Public Trailer As String
    Public Seal1 As String
    Public Seal2 As String
    Public Driver As String
    Public Bol As String
    Public TransportType As String
    Public TransportReference As String
End Class

#End Region