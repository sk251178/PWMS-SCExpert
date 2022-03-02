<CLSCompliant(False)> Public Class ConsolidationTask
    Inherits TASK

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Sub ExitTask()
        Dim cons As New Consolidation(_consolidation)
        cons.UnAssignUser(USERID)
        MyBase.ExitTask()
    End Sub

    Public Overrides Sub AssignUser(ByVal pUserId As String, Optional ByVal pAssignmentType As String = WMS.Lib.TASKASSIGNTYPE.MANUAL, Optional ByVal userMHType As String = "", Optional ByVal pPriority As Int32 = -1)
        Dim cons As New Consolidation(_consolidation)
        cons.AssignUser(pUserId)
        MyBase.AssignUser(pUserId, WMS.Lib.TASKASSIGNTYPE.MANUAL, userMHType)
    End Sub

    Public ReadOnly Property ShouldCreateContainer() As Boolean
        Get
            Dim cons As New Consolidation(_consolidation)
            If cons.Started Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property ConsolidationObject() As Consolidation
        Get
            Return New Consolidation(_consolidation)
        End Get
    End Property

    Public Function getConsolidationJob() As ConsolidationJob
        Dim cj As New ConsolidationJob
        Dim cons As New Consolidation(_consolidation)
        Dim condet As ConsolidationTaskDetail
        If cons.Status = WMS.Lib.Statuses.Consolidation.COMPLETE Or cons.Status = WMS.Lib.Statuses.Consolidation.CANCELED Then
            Return Nothing
        End If
        For Each condet In cons
            If condet.Status <> WMS.Lib.Statuses.Consolidation.COMPLETE And condet.Status <> WMS.Lib.Statuses.Consolidation.CANCELED Then
                Dim conld As New Load(condet.FromLoad)
                cj.consignee = conld.CONSIGNEE
                cj.ConsolidationId = condet.ConsolidateId
                cj.ConsolidationLine = condet.ConsolidateLine
                cj.FromLoad = conld.LOADID
                cj.FromLocation = conld.LOCATION
                cj.FromWarehousearea = conld.WAREHOUSEAREA
                cj.FromContainer = conld.ContainerId
                cj.isContainer = False
                cj.Sku = conld.SKU
                cj.TaskId = _task
                cj.tocontainer = cons.ToContainer
                cj.Units = conld.UNITS
                cj.UOM = conld.LOADUOM
                Dim sku As New sku(conld.CONSIGNEE, conld.SKU)
                cj.skuDesc = sku.SKUDESC
                cj.UOMUnits = sku.ConvertUnitsToUom(conld.LOADUOM, conld.UNITS)
                Return cj
            End If
        Next
        Return Nothing
    End Function

    Public Function ConsolidateLoad(ByVal ConJob As ConsolidationJob, ByVal pConfirmLoadId As String, ByVal pUser As String) As ConsolidationJob
        Dim cons As New Consolidation(_consolidation)
        cons.Consolidate(ConJob, New Load(pConfirmLoadId), pUser)
        Return getConsolidationJob()
    End Function

    Public Function ConsolidateContainer(ByVal ConJob As ConsolidationJob, ByVal pConfirmContainerId As String, ByVal pUser As String) As ConsolidationJob
        Dim cons As New Consolidation(_consolidation)
        Dim cn As New Container(pConfirmContainerId, True)
        cons.Consolidate(ConJob, cn, pUser)
        Return getConsolidationJob()
    End Function

    Public Overrides Sub Complete(ByVal logger As LogHandler, Optional ByVal pProblemRC As String = "")
        Dim cons As Consolidation = Me.ConsolidationObject
        If cons.Status = WMS.Lib.Statuses.Consolidation.COMPLETE Then
            If _assigned Then
                Dim t As New ConsolidationDeliveryTask
                t.TASKTYPE = WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY
                t.Consolidation = cons.ConsolidateId
                t.FromContainer = cons.ToContainer
                t.TOLOCATION = cons.DestinationLocation
                t.TOWAREHOUSEAREA = cons.DestinationWarehousearea

                t.Post(_userid)
            Else
                Dim cntr As New Container(cons.ToContainer, True)
                cntr.Deliver(cons.DestinationLocation, cons.DestinationWarehousearea, USERID)
            End If
        End If
        MyBase._edituser = _edituser
        _executionlocation = cons.DestinationLocation

        MyBase.Complete(logger)
    End Sub
End Class
