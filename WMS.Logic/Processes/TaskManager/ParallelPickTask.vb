<CLSCompliant(False)> Public Class ParallelPickTask
    Inherits TASK

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Sub ExitTask()
        Dim pck As New ParallelPicking(_parallelpicklist)
        If Not pck.Started Then
            pck.UnAssign(_userid)
            MyBase.ExitTask()
        End If
    End Sub

    Public Overrides Sub AssignUser(ByVal pUserId As String, Optional ByVal pAssignmentType As String = WMS.Lib.TASKASSIGNTYPE.MANUAL, Optional ByVal userMHType As String = "", Optional ByVal pPriority As Int32 = -1)
        MyBase.AssignUser(pUserId, WMS.Lib.TASKASSIGNTYPE.MANUAL, userMHType)
        Dim pck As New ParallelPicking(_parallelpicklist)
        pck.AssignUser(pUserId)
    End Sub

    Public Overrides Sub Complete(ByVal logger As LogHandler, Optional ByVal pProblemRC As String = "")
        _userid = WMS.Logic.GetCurrentUser
        If _assigned Then
            Dim t As New DeliveryTask
            Dim tm As New TaskManager
            Try
                tm.CreateDeliveryTask(Me, _userid)
            Catch ex As Exception
            End Try
        End If
        MyBase.Complete(logger)
    End Sub

    Public Overrides Sub Cancel()
        Dim parPickList As New WMS.Logic.ParallelPicking(Me.ParallelPicklist)
        For Each pickListObj As WMS.Logic.Picklist In parPickList.PickLists
            pickListObj.Cancel(WMS.Lib.USERS.SYSTEMUSER, False)
        Next
        parPickList.setComplete(WMS.Lib.USERS.SYSTEMUSER, False)
        MyBase.Cancel()
    End Sub
End Class
