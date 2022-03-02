Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class SpecificPickupTask
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

    Public Sub CreateSpecificLoadPickup(ByVal FromLocation As String, ByVal pFromLoad As String)
        _fromlocation = FromLocation
        _fromload = pFromLoad
        _task_type = WMS.Lib.TASKTYPE.SPICKUP
        MyBase.Post()
    End Sub

    Public Sub CreateSpecificContainerPickup(ByVal FromLocation As String, ByVal pFromContainer As String)
        _fromlocation = FromLocation
        _fromcontainer = pFromContainer
        _task_type = WMS.Lib.TASKTYPE.SPICKUP
        MyBase.Post()
    End Sub

#End Region

End Class
