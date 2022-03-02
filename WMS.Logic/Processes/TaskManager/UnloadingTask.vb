Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class UnloadingTask
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

    Public Function AssignTask(ByVal pUser As String)
        Dim sql As String
        Dim dt As New DataTable
        sql = String.Format("SELECT TOP 1 TASK FROM TASKS WHERE USERID = '{0}' AND ASSIGNED = 1 AND STATUS = '{1}' AND TASKTYPE = '{2}'", pUser, WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.UNLOADING)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            _task = dt.Rows(0)("TASK")
            Load()
            Return Me
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Create"

    Public Overloads Sub Create(ByVal Door As String, ByVal puser As String)
        If _task = "" Then
            _task = Made4Net.Shared.Util.getNextCounter("TASK")
        End If
        _fromlocation = Door
        '' !!! Warehousearea - add paramByVal Warehousearea As String,
        _fromwarehousearea = ""
        _priority = 200
        _task_type = WMS.Lib.TASKTYPE.UNLOADING
        MyBase.Post(puser)
    End Sub

#End Region

#Region "Complete & Exit"

    Public Sub CompleteTask()
        _executionlocation = _fromlocation
        _executionwarehousearea = _fromwarehousearea
        Me.Complete(Nothing)
    End Sub

    Public Sub ReleaseTask(ByVal puser As String)
        Dim sql As String
        Dim dt As New DataTable
        sql = String.Format("SELECT TOP 1 TASK FROM TASKS WHERE USERID = '{0}' AND ASSIGNED = 1 AND STATUS = '{1}' AND TASKTYPE = '{2}'", puser, WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.UNLOADING)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            _task = dt.Rows(0)("TASK")
            Load()
            ExitTask()
        End If
    End Sub

#End Region

#End Region

End Class

#Region "Unloading Job"

<CLSCompliant(False)> Public Class UnloadingJob
    Public TaskId As String
    Public ToLocation As String
    Public Receipt As String
    Public Door As String
End Class

#End Region