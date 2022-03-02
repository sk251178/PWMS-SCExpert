Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

<CLSCompliant(False)> Public Class Staging

#Region "Ctor"

    Public Sub New()
        'Empty Ctor
    End Sub

#End Region

#Region "Methods"

#Region "Load"

    Public Function StageLoad(ByVal pLoadId As String, ByVal pUser As String) As Task
        If Not WMS.Logic.Load.Exists(pLoadId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot stage load, Load does not exists", "Cannot stage load, Load does not exists")
        End If
        Dim oDelTask As New WMS.Logic.DeliveryTask()
        Dim oLoad As New WMS.Logic.Load(pLoadId)
        If oLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.STAGED Then
            Throw New M4NException(New Exception, "Load is already staged.", "Load is already staged.")
        End If
        If oLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.LOADED Then
            Throw New M4NException(New Exception, "Load is already loaded.", "Load is already loaded.")
        End If

        ' '' '' First get orderload type
        '' ''Dim docType As String = DataInterface.ExecuteScalar("SELECT DOCUMENTTYPE FROM ORDERLOADS WHERE LOADID = '" & pLoadId & "'")

        '' ''Select Case docType
        '' ''    Case "FLWTH"
        '' ''        Dim oFlt As WMS.Logic.Flowthrough = oLoad.GetFlowThroughOrder()
        '' ''        If oFlt Is Nothing Then
        '' ''            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot stage load, Load does not belong to flowthrough order", "Cannot stage load, Load does not belong to flowthrough order")
        '' ''        End If
        '' ''        'oLoad.SetDestinationLocation(oFlt.STAGINGLANE, "", pUser)
        '' ''        'oDelTask.CreateLoadDeliveryTask(oLoad.LOADID, oFlt.STAGINGLANE, "", pUser)
        '' ''        oLoad.SetDestinationLocation(oFlt.STAGINGLANE, oFlt.STAGINGWAREHOUSEAREA, pUser)
        '' ''        oDelTask.CreateLoadDeliveryTask(oLoad.LOADID, oFlt.STAGINGLANE, oFlt.STAGINGWAREHOUSEAREA, pUser)
        '' ''    Case "OUTBOUND"
        '' ''        Dim oOrd As WMS.Logic.OutboundOrderHeader = oLoad.GetOutboundOrder()
        '' ''        If oOrd Is Nothing Then
        '' ''            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot stage load, Load does not belong to outbound order", "Cannot stage load, Load does not belong to outbound order")
        '' ''        End If
        '' ''        'oLoad.SetDestinationLocation(oOrd.STAGINGLANE, "", pUser)
        '' ''        oLoad.SetDestinationLocation(oOrd.STAGINGLANE, oOrd.STAGINGWAREHOUSEAREA, pUser)
        '' ''        'oDelTask.CreateLoadDeliveryTask(oLoad.LOADID, oOrd.STAGINGLANE, "", pUser)
        '' ''        oDelTask.CreateLoadDeliveryTask(oLoad.LOADID, oOrd.STAGINGLANE, oOrd.STAGINGWAREHOUSEAREA, pUser)
        '' ''End Select

        '' ''Return oDelTask


        Dim stagingLane As String = ""
        Dim stagingWarehouseArea As String = ""
        oLoad.GetLoadFinalDestination(stagingLane, stagingWarehouseArea)
        If String.IsNullOrEmpty(stagingLane) Then
            Throw New M4NException(New Exception(), "Can not stage load. No staging lane found", "Can not stage load. No staging lane found")
        End If
        oLoad.SetDestinationLocation(stagingLane, stagingWarehouseArea, pUser)
        oDelTask.CreateLoadDeliveryTask(oLoad.LOADID, stagingLane, stagingWarehouseArea, pUser)
        Return oDelTask
    End Function

#End Region

#Region "Containers"

    Public Function StageContainer(ByVal pContainerId As String, ByVal pUser As String, Optional ByVal pPicklistID As String = "") As Task
        If Not WMS.Logic.Container.Exists(pContainerId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot stage Container, Container does not exists", "Cannot stage Container, Container does not exists")
        End If
        Dim sameDest As Boolean = True
        Dim oDelTask As New WMS.Logic.DeliveryTask

        'Dim Sql As String = String.Format("select ol.consignee, ol.orderid, ol.loadid, inv.handlingunit as containerid , inv.* from invload inv inner join orderloads ol on ol.loadid = inv.loadid where inv.handlingunit = '{0}'", pContainerId)
        Dim Sql As String = String.Format("select ol.consignee, ol.orderid, ol.loadid, inv.handlingunit as containerid , inv.* from invload inv inner join orderloads ol on ol.loadid = inv.loadid where inv.handlingunit = '{0}' union select ol.consignee, ol.orderid, ol.loadid, CONTAINER.ONCONTAINER as containerid , inv.* from invload inv inner join orderloads ol on ol.loadid = inv.loadid inner join CONTAINER on container.CONTAINER = inv.HANDLINGUNIT where CONTAINER.ONCONTAINER = '{0}'", pContainerId)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        Dim sLoc As String
        Dim sWarehouseArea As String
        If dt.Rows.Count > 0 Then
            'sLoc = GetStagingLane(dt.Rows(0)("consignee"), dt.Rows(0)("orderid"))
            For Each dr As DataRow In dt.Rows
                Dim tmpLoad As New WMS.Logic.Load(dr) '(Convert.ToString(dr("loadid")))
                'Dim tmpSl As String = tmpLoad.GetLoadFinalDestination() 'GetStagingLane(dr("loadid"))
                Dim tmpSl As String = ""
                Dim tmpWarehouseArea As String = ""
                tmpLoad.GetLoadFinalDestination(tmpSl, tmpWarehouseArea)
                If tmpSl <> String.Empty Then
                    'tmpLoad.SetDestinationLocation(tmpSl, "", pUser)
                    tmpLoad.SetDestinationLocation(tmpSl, tmpWarehouseArea, pUser)
                End If
                sLoc = tmpSl
                sWarehouseArea = tmpWarehouseArea
            Next
        End If
        'oDelTask.CreateContainerDeliveryTask(pContainerId, sLoc, "", pUser)
        ' oDelTask.CreateContainerDeliveryTask(pContainerId, sLoc, sWarehouseArea, pUser)
        'Begin for RWMS-1294 and RWMS-1222
        oDelTask.CreateContainerDeliveryTask(pContainerId, sLoc, sWarehouseArea, pUser, Nothing, pPicklistID)
        'End for RWMS-1294 and RWMS-1222
        Return oDelTask
    End Function

    'Private Function GetStagingLane(ByVal pLoadID As String) As String
    '    Dim sl As String
    '    Dim Sql As String = String.Format("select isnull(staginglane,'') as staginglane from vLoadsStagingLanes where loadid = '{0}' ", pLoadID)
    '    sl = DataInterface.ExecuteScalar(Sql)
    '    Return sl
    'End Function

    'Private Function GetStagingLane(ByVal pLoadID As String) As String

    '    '    Private Function GetStagingLane(ByVal pConsignee As String, ByVal pOrderid As String) As String
    '    Dim sl As String
    '    'Sql = String.Format("select isnull(staginglane,'') as staginglane from outboundorheader where consignee = '{0}' and orderid = '{1}'", pConsignee, pOrderid)
    '    'sl = DataInterface.ExecuteScalar(Sql)
    '    Dim docType As String = DataInterface.ExecuteScalar("SELECT DOCUMENTTYPE FROM ORDERLOADS WHERE LOADID = '" & pLoadId & "'")
    '    Dim oLoad As New WMS.Logic.Load(pLoadID)
    '    Select Case docType
    '        Case "FLWTH"
    '            Dim oFlt As WMS.Logic.Flowthrough = oLoad.GetFlowThroughOrder()
    '            If oFlt Is Nothing Then
    '                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot stage load, Load does not belong to flowthrough order", "Cannot stage load, Load does not belong to flowthrough order")
    '            End If
    '            sl = oFlt.STAGINGLANE
    '        Case "OUTBOUND"
    '            Dim oOrd As WMS.Logic.OutboundOrderHeader = oLoad.GetOutboundOrder()
    '            If oOrd Is Nothing Then
    '                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot stage load, Load does not belong to outbound order", "Cannot stage load, Load does not belong to outbound order")
    '            End If
    '            sl = oOrd.STAGINGLANE
    '    End Select
    '    Return sl
    'End Function

#End Region

#End Region

End Class
