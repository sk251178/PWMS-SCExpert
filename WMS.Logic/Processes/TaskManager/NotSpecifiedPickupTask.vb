Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class NotSpecifiedPickupTask
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

    Public Overloads Sub Create(ByVal FromLocation As String, ByVal FromWarehousearea As String)
        _fromlocation = FromLocation
        _fromwarehousearea = FromWarehousearea
        _task_type = WMS.Lib.TASKTYPE.NSPICKUP
        MyBase.Post()
    End Sub

#End Region

End Class