<CLSCompliant(False)> Public Class ConsolidationDeliveryTask
    Inherits Task

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub Deliver(ByVal toLocation As String, ByVal toWarehousearea As String)
        Dim cntr As New Container(_fromcontainer, True)
        cntr.Deliver(toLocation, toWarehousearea, USERID)
        Dim pCurrentUser As String = _userid
        _executionlocation = toLocation
        Me.Complete(Nothing)
    End Sub

    Public Function getDeliveryJob() As DeliveryJob
        Dim djob As New DeliveryJob
        djob.isContainer = True
        djob.LoadId = _fromcontainer
        djob.TaskId = _task
        djob.toLocation = _tolocation
        djob.toWarehousearea = _towarehousearea

        Return djob
    End Function

End Class
