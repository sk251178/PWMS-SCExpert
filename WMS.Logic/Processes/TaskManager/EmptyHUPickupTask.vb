Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class EmptyHUPickupTask
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

#Region "Create"

    Public Sub CreateEmptyHUPickupTask(ByVal cnt As Container)
        _fromlocation = cnt.Location
        _fromwarehousearea = cnt.Warehousearea

        Dim loc As Location = New Location(cnt.Location, cnt.Warehousearea, True)

        '_fromcontainer = cnt.ContainerId
        '_tocontainer = cnt.ContainerId
        _task_type = WMS.Lib.TASKTYPE.EMPTYHUPICKUPTASK

        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset("SELECT * FROM EMPTYHUPICKUPPOLICY WHERE HUTYPE = '" & cnt.HandlingUnitType & "' AND  PICKREGION = '" & loc.PICKREGION & "'", dt)

        ' Only if there is policy for evacuating empty container than  create task , else do not do nothing
        If dt.Rows.Count > 0 Then
            _tolocation = dt.Rows(0)("EMPTYHULOCATION")
            _towarehousearea = dt.Rows(0)("EMPTYHUWAREHOUSEAREA")
            _priority = dt.Rows(0)("TASKPRIORITY")
            MyBase.Post()
        End If

    End Sub


#End Region

    Public Sub Deliver(ByVal pUser As String)
        'Dim sql As String = "SELECT * FROM CONTAINER WHERE LOCATION = '" & _fromlocation & "'"
        'Dim dt As DataTable = New DataTable
        'DataInterface.FillDataset(sql, dt)
        'For Each cntdr As DataRow In dt.Rows
        '    Dim oCont As New Container(cntdr("CONTAINER"), True)
        '    oCont.Deliver(_tolocation, pUser, False)
        'Next
        _executionlocation = _tolocation
        Me.Complete(Nothing)
    End Sub

    Public Function getDeliveryJob()
        Dim djob As New EmptyHUPickupJob
        'djob.HUId = _fromcontainer
        djob.FromLocation = _fromlocation
        djob.ToLocation = _tolocation
        djob.FromWarehousearea = _fromwarehousearea
        djob.ToWarehousearea = _towarehousearea
        djob.TaskId = _task
        djob.IsHandOff = False
        Return djob
    End Function

#End Region

End Class

#Region "EmptyHUPickup Job"

<CLSCompliant(False)> Public Class EmptyHUPickupJob
    Public TaskId As String
    'Public HUId As String
    Public FromLocation As String
    Public ToLocation As String
    Public FromWarehousearea As String
    Public ToWarehousearea As String
    Public IsHandOff As Boolean
End Class

#End Region