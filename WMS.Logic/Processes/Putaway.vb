Imports Made4Net.DataAccess
Imports System.Collections.Generic
Imports Made4Net.Algorithms
Imports Made4Net.Algorithms.DTO
Imports Made4Net.Algorithms.Scoring
Imports Made4Net.Algorithms.SortingAlgorithms


<CLSCompliant(False)> Public Class Putaway

    Public Sub New()
    End Sub

#Region "Load Putaway"

    ''' <summary>
    ''' When Simulation flag is set then we will only find location and not assign load to any activity
    ''' </summary>
    Public Sub RequestDestinationForLoad(ByVal pLoadId As String, ByRef destLocation As String, ByRef destWarehousearea As String, ByVal Simulation As Integer, ByRef prePopulateLocation As String, Optional ByVal CreateTask As Boolean = True, Optional ByVal onContainer As Boolean = False) 'RWMS1277
        If Not Load.Exists(pLoadId) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot assign Location to load", "Cannot assign Location to load - Load does not exist")
            m4nEx.Params.Add("Loadid", pLoadId)
            Throw m4nEx
        End If
        If Simulation = 1 Then
            Dim oLoad As New Load(pLoadId)
            RequestDestination(oLoad, destLocation, destWarehousearea, Common.GetCurrentUser, prePopulateLocation) 'RWMS-1277
            ' Release activity status
            Dim sql As String = String.Format("UPDATE LOADS SET activitystatus = '',DestinationLocation = '',DestinationWarehousearea = '', editdate = {0},edituser = {1} where loadid = '{2}'", _
                    Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(Common.GetCurrentUser), pLoadId)
            DataInterface.ExecuteScalar(sql)
            ' Return found location for future use
        Else
            RequestDestinationForLoad(pLoadId, destLocation, destWarehousearea, prePopulateLocation, CreateTask, onContainer) ''RWMS-1277
        End If
    End Sub

    Private Sub RequestDestinationForLoad(ByVal pLoadId As String, ByRef destLocation As String, ByRef destWarehousearea As String, ByRef prePopulateLocation As String, Optional ByVal CreateTask As Boolean = True, Optional ByVal onContainer As Boolean = False) 'RWMS-1277
        Dim oLoad As New Load(pLoadId)
        If oLoad.UNITSALLOCATED > 0 Then
            If Not String.Equals(oLoad.ACTIVITYSTATUS, WMS.Lib.Statuses.ActivityStatus.REPLPENDING, StringComparison.CurrentCultureIgnoreCase) Then

                Throw New Made4Net.Shared.M4NException(New Exception(), "Can not putaway load - units allocated", "Can not putaway load - units allocated")
            End If
        End If
        RequestDestination(oLoad, destLocation, destWarehousearea, Common.GetCurrentUser, prePopulateLocation) 'RWMS-1277
        'if there is no location found -> leave it in the same location
        If destLocation = "" Then
            destLocation = oLoad.LOCATION
        End If
        If destWarehousearea = "" Then
            destWarehousearea = oLoad.WAREHOUSEAREA
        End If
        oLoad.SetDestinationLocation(destLocation, destWarehousearea, Common.GetCurrentUser)
        If CreateTask And Not onContainer Then
            Dim tm As New TaskManager
            tm.CreatePutAwayTask(oLoad, Common.GetCurrentUser, True, 200, "", "", Nothing, prePopulateLocation) 'RWMS-1277

            ' 'RWMS-1497
            Dim whPreviousActivityTime As DateTime?
            Dim whActivity As New DataTable
            Dim query As String
            query = String.Format("Select * from WHACTIVITY where userid = '{0}'", Common.GetCurrentUser)
            DataInterface.FillDataset(query, whActivity)
            If whActivity.Rows.Count >= 1 And Not whActivity.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
                whPreviousActivityTime = whActivity.Rows(0)("PREVIOUSACTIVITYTIME")
            End If
            If whPreviousActivityTime.HasValue Then
                query = String.Format("update tasks set starttime={0}, editdate={0},edituser='{1}' where TASKTYPE='LOADPW' AND ASSIGNED = 1 AND USERID='{1}'  AND STARTTIME is null", Made4Net.Shared.Util.FormatField(whPreviousActivityTime), Common.GetCurrentUser)
                DataInterface.RunSQL(query)
            End If
            'RWMS-1497
        End If
    End Sub

    Protected Sub RequestDestination(ByVal oLoad As WMS.Logic.Load, _
            ByRef destLocation As String, ByRef destWarehousearea As String, ByVal pUser As String, ByRef prePopulateLocation As String) 'RWMS-1277
        oLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, pUser)
        SendToLocassign(destLocation, destWarehousearea, oLoad.LOADID, pUser, prePopulateLocation) 'RWMS-1277
    End Sub

    Protected Sub SendToLocassign(ByRef destLocation As String, ByRef destWarehousearea As String, _
       ByVal pLoadId As String, ByVal pUser As String, ByRef prePopulateLocation As String) 'RWMS-1277
        Dim ld As New Load(pLoadId)
        RequestDest(destLocation, destWarehousearea, pLoadId, prePopulateLocation) 'RWMS-1277
        ld.SetDestinationLocation(destLocation, destWarehousearea, pUser)
    End Sub

    'this function gets load id and return a destination for load
    Protected Sub RequestDest(ByRef destLocation As String, ByRef destWarehousearea As String, _
        ByVal pLoadId As String, ByRef prePopulateLocation As String)  'RWMS-1277
        Dim ld As Load
        Try
            ld = New Load(pLoadId)
            'ld.DESTINATIONLOCATION = destLocation
            'ld.DESTINATIONWAREHOUSEAREA = destWarehousearea
            Dim co As New Consignee(ld.CONSIGNEE)
            Dim sk As New SKU(ld.CONSIGNEE, ld.SKU)

            If destLocation = String.Empty AndAlso sk.PREFLOCATION <> "" And Not sk.PREFLOCATION Is Nothing _
                And sk.PREFWAREHOUSEAREA <> "" And Not sk.PREFWAREHOUSEAREA Is Nothing Then
                SendLocToLocAssign(destLocation, destWarehousearea, pLoadId, sk.PREFLOCATION, sk.PREFWAREHOUSEAREA, "", "%", 1, 1, 1, 1)
            End If

            If destLocation = String.Empty AndAlso sk.PREFPUTREGION <> "" And Not sk.PREFPUTREGION Is Nothing Then
                SendRegToLocAssign(destLocation, destWarehousearea, "", pLoadId, String.Format("{0}", sk.PREFPUTREGION), "", "%", 1, 1, 1, 1)
            End If

            If destLocation = String.Empty Then
                'else - we need to check the putaway policies -> send the load to loc assign service
                SendLoadToLocAssign(destLocation, destWarehousearea, ld.LOADID, prePopulateLocation) 'RWMS-1277
            End If

            'else we didnt find any location, return load's current location
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SendLocToLocAssign(ByRef destLocation As String, ByRef destWarehousearea As String, _
        ByVal pLoadId As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pSTORAGETYPE As String, _
        ByVal pCONTENT As String, ByVal pFitByVolume As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByPalletType As Boolean)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SendLocToLocAssign)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Services.REQUESTLOCATIONBYLOC)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", pLoadId)

        aq.Add("FROMLOC", pLocation)
        aq.Add("FROMWAREHOUSEAREA", pWarehousearea)

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", pLoadId)

        aq.Add("TOLOC", pLocation)
        aq.Add("TOWAREHOUSEAREA", pWarehousearea)

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Services.REQUESTLOCATIONBYLOC)


        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        oQ.Add("LOADID", pLoadId)

        oQ.Add("LOCATION", pLocation)
        oQ.Add("WAREHOUSEAREA", pWarehousearea)

        oQ.Add("ACTION", WMS.Lib.Actions.Services.REQUESTLOCATIONBYLOC)
        oQ.Add("USERID", Common.GetCurrentUser())
        oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse())
        oQ.Add("STORAGETYPE", pSTORAGETYPE)
        oQ.Add("FITBYVOLUME", pFitByVolume)
        oQ.Add("FITBYHEIGHT", pFitByHeight)
        oQ.Add("FITBYWEIGHT", pFitByWeight)
        oQ.Add("FITBYPALLETTYPE", pFitByPalletType)
        oQ.Add("CONTENT", pCONTENT)
        oMsg = oQ.Send("LocAssign")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        If Not qm.Values("ERROR") Is Nothing Then
            Throw New ApplicationException(qm.Values("ERROR"))
        End If

        destLocation = qm.Values("LOCATION")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
    End Sub

    Private Sub SendRegToLocAssign(ByRef destLocation As String, ByRef destWarehousearea As String, _
        ByVal pPWPolicy As String, ByVal pLoadId As String, ByVal pRegion As String, ByVal pSTORAGETYPE As String, _
        ByVal pCONTENT As String, ByVal pFitByVolume As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByPalletType As Boolean)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SendLocToLocAssign)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Services.REQUESTLOCATIONBYREGION)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", pLoadId)

        aq.Add("FROMLOC", "")
        aq.Add("WAREHOUSEAREA", "")

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", pLoadId)


        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Services.REQUESTLOCATIONBYREGION)

        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        oQ.Add("LOADID", pLoadId)
        oQ.Add("POLICYID", pPWPolicy)
        oQ.Add("REGION", pRegion)
        oQ.Add("STORAGETYPE", pSTORAGETYPE)
        oQ.Add("FITBYVOLUME", pFitByVolume)
        oQ.Add("FITBYHEIGHT", pFitByHeight)
        oQ.Add("FITBYWEIGHT", pFitByWeight)
        oQ.Add("FITBYPALLETTYPE", pFitByPalletType)
        oQ.Add("CONTENT", pCONTENT)
        oQ.Add("ACTION", WMS.Lib.Actions.Services.REQUESTLOCATIONBYREGION)
        oQ.Add("USERID", Common.GetCurrentUser())
        oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse())
        oMsg = oQ.Send("LocAssign")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        If Not qm.Values("ERROR") Is Nothing Then
            Throw New ApplicationException(qm.Values("ERROR"))
        End If

        destLocation = qm.Values("LOCATION")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
    End Sub

    Public Sub SendLoadToLocAssign(ByRef destLocation As String, ByRef destWarehousearea As String, _
        ByVal pLoadId As String, ByRef prePopulateLocation As String) ''RWMS-1277
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SendLocToLocAssign)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Services.REQUESTLOCATIONFORLOAD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", pLoadId)
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", pLoadId)
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Services.REQUESTLOCATIONFORLOAD)

        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        oQ.Add("LOADID", pLoadId)
        oQ.Add("ACTION", WMS.Lib.Actions.Services.REQUESTLOCATIONFORLOAD)
        oQ.Add("USERID", Common.GetCurrentUser())
        oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse())
        oMsg = oQ.Send("LocAssign")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        If Not qm.Values("ERROR") Is Nothing Then
            Throw New ApplicationException(qm.Values("ERROR"))
        End If

        destLocation = qm.Values("LOCATION")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
        prePopulateLocation = qm.Values("PREPOPULATELOCATION") 'RWMS-1277
    End Sub

    Private Sub setDestination(ByVal pLocationName As String, ByVal pWarehouseareaName As String, ByVal ld As Load)
        Dim PendingWeight, PendingCube As Double
        Dim sql As String
        Try
            PendingWeight = ld.CalculateWeight()
        Catch ex As Exception
            PendingWeight = 0
        End Try

        Try
            PendingCube = ld.Volume
        Catch ex As Exception
            PendingCube = 0
        End Try
        sql = String.Format("Update Loads set DestinationLocation = '{0}',DestinationWarehousearea = '{4}', EditDate = '{1}',EditUser = '{2}' Where LoadId = '{3}'", pLocationName, Made4Net.Shared.Util.DateTimeToDbString(DateTime.Now), Common.GetCurrentUser(), ld.LOADID, pWarehouseareaName)
        DataInterface.RunSQL(sql)
    End Sub

    'Public Sub Put(ByVal pLoadId As String, ByVal pSubLocation As String, ByVal pLocation As String, _
    '                ByVal pSubWarehousearea As String, ByVal pWarehousearea As String, ByVal pUser As String, Optional ByVal pIsHandOff As Boolean = False)
    Public Sub Put(ByVal pLoadId As String, ByVal pSubLocation As String, ByVal pLocation As String, _
                    ByVal pWarehousearea As String, ByVal pUser As String, Optional ByVal pIsHandOff As Boolean = False)
        Dim oLoad As New Load(pLoadId)
        Dim FromLocation As String = oLoad.LOCATION
        Dim FromWarehousearea As String = oLoad.WAREHOUSEAREA
        ' oLoad.Put(pLocation, pWarehousearea, pSubLocation, pUser)
        oLoad.Put(pLocation, pWarehousearea, pSubLocation, pUser, pIsHandOff)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadPutaway)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOADPUTAWAY)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", oLoad.CONSIGNEE)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", pLoadId)

        aq.Add("FROMLOC", FromLocation)
        aq.Add("FROMWAREHOUSEAREA ", FromWarehousearea)

        aq.Add("FROMQTY", oLoad.UNITS)
        aq.Add("FROMSTATUS", oLoad.STATUS)
        aq.Add("NOTES", "PROC")
        aq.Add("SKU", oLoad.SKU)
        aq.Add("TOLOAD", pLoadId)

        aq.Add("TOLOC", pLocation)
        aq.Add("TOWAREHOUSEAREA", pWarehousearea)

        aq.Add("TOQTY", oLoad.UNITS)
        aq.Add("TOSTATUS", oLoad.STATUS)
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Audit.LOADPUTAWAY)
    End Sub
    'Start RWMS-1277
    Public Function CheckPrepolateLocationFromTask(TaskId As String) As Boolean
        Dim sql As String
        sql = String.Format("select ISNULL(PREPOPULATELOCATION,0) PREPOPULATELOCATION from TASKS where TASK='{0}'", TaskId)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    End Function
    'End RWMS-1277
#End Region

#Region "Container Putaway"

    Public Sub RequestDestinationForContainer(ByRef destLocation As String, ByRef destWarehousearea As String, _
                    ByVal pContainerId As String, ByVal pUser As String, Optional ByVal CreateTask As Boolean = True)

        If Not Container.Exists(pContainerId) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot assign Location to container", "Cannot assign Location to container - Container does not exist")
            m4nEx.Params.Add("Container", pContainerId)
            Throw m4nEx
        End If
        Dim oCont As New Container(pContainerId, True)
        oCont.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, pUser)
        SendContainerToLocassign(destLocation, destWarehousearea, oCont, pUser)
        'if there is no location found -> leave it in the same location
        If destLocation = "" Then
            destLocation = oCont.Location
        End If
        If destWarehousearea = "" Then
            destLocation = oCont.Warehousearea
        End If
        oCont.SetDestinationLocation(destLocation, destWarehousearea, pUser)
        'And remove the activity statuses of the loads
        For Each oLoad As WMS.Logic.Load In oCont.Loads
            If oLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND Then
                oLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, pUser)
            End If
        Next
        If CreateTask Then
            Dim tm As New TaskManager
            tm.CreateContainerPutAwayTask(oCont, pUser)
        End If

    End Sub

    Protected Sub SendContainerToLocassign(ByRef destLocation As String, ByRef destWarehousearea As String, _
                    ByVal oCont As Container, ByVal pUser As String)
        SendContainerToLocassign(destLocation, destWarehousearea, oCont.ContainerId)

    End Sub

    Public Sub SendContainerToLocAssign(ByRef destLocation As String, ByRef destWarehousearea As String, _
                    ByVal pContainerId As String)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ContainerPutaway)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Services.REQUESTLOCATIONFORCONTAINER)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMCONTAINER", pContainerId)

        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA ", "")

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOCONTAINER", pContainerId)

        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA ", "")

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Services.REQUESTLOCATIONFORCONTAINER)

        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        oQ.Add("CONTAINERID", pContainerId)
        oQ.Add("ACTION", WMS.Lib.Actions.Services.REQUESTLOCATIONFORCONTAINER)
        oQ.Add("USERID", Common.GetCurrentUser())
        oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse())
        oMsg = oQ.Send("LocAssign")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        If Not qm.Values("ERROR") Is Nothing Then
            Throw New ApplicationException(qm.Values("ERROR"))
        End If

        destLocation = qm.Values("LOCATION")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
    End Sub

    Public Sub PutContainer(ByVal pContid As String, ByVal pLocation As String, ByVal pWarehousearea As String, _
            ByVal pUser As String)
        Dim oCont As New Container(pContid, True)
        Dim FromLocation As String = oCont.Location
        Dim FromWarehousearea As String = oCont.Warehousearea

        oCont.Put(pLocation, pWarehousearea, pUser)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ContainerPutaway)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CONTAINERPUTAWAY)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMCONTAINER", pContid)

        aq.Add("FROMLOC", FromLocation)
        aq.Add("FROMWAREHOUSEAREA", FromWarehousearea)

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oCont.Status)
        aq.Add("NOTES", "PROC")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOCONTAINER", pContid)

        aq.Add("TOLOC", pLocation)
        aq.Add("TOWAREHOUSEAREA", pWarehousearea)

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", oCont.Status)
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Audit.CONTAINERPUTAWAY)
    End Sub

#End Region

#Region "PutAway Scoring"

    <CLSCompliant(False)> Public Class PutAwayScoring

#Region "Variables"
        Protected _strategyid As String
        Protected _attributesscoring As Made4Net.DataAccess.Collections.GenericCollection
#End Region

#Region "Properties"
        Public ReadOnly Property StrategyId() As String
            Get
                Return _strategyid
            End Get
        End Property

        Public ReadOnly Property Key(ByVal idx As Int32) As String
            Get
                Return _attributesscoring.Keys(idx)
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal idx As Int32) As Decimal
            Get
                Return _attributesscoring(idx)
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal sKey As String) As Decimal
            Get
                Return _attributesscoring(sKey)
            End Get
        End Property

        Public ReadOnly Property DoesAttributeExistsOrHasValue(ByVal sKey As String) As Boolean
            Get
                If _attributesscoring.ContainsKey(sKey) Then
                    If Not Object.ReferenceEquals(_attributesscoring.Item(sKey), System.DBNull.Value) And _attributesscoring.Item(sKey) IsNot Nothing Then
                        Dim result As Decimal = 0
                        If Decimal.TryParse(_attributesscoring.Item(sKey), result) Then
                            If result <> 0 Then
                                Return True
                            End If
                        End If
                    End If
                End If
                Return False
            End Get
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal pPutawayPolicyId As String)
            _strategyid = pPutawayPolicyId
            Load()
        End Sub
#End Region

#Region "Methods"

        Protected Sub Load()
            Dim sql As String = String.Format("Select * from PUTAWAYPOLICYSCORING where STRATEGYID = {0}", Made4Net.Shared.Util.FormatField(_strategyid))
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                _attributesscoring = New Made4Net.DataAccess.Collections.GenericCollection
                For Each oCol As DataColumn In dt.Columns
                    If Not oCol.ColumnName.ToLower = "strategyid" Then
                        _attributesscoring.Add(oCol.ColumnName, Convert.ToDecimal(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr(oCol.ColumnName), "0")))
                    End If
                Next
            End If
            dt.Dispose()
        End Sub

        Public Sub Score(ByRef cLocationCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
            If Not oLogger Is Nothing Then
                oLogger.Write("Start scoring the location collection...")
            End If
            If cLocationCollection.Length = 0 Then Return
            Dim scoringStats As String = String.Empty

            Dim oScoreSetter As ScoreSetter = _
                New ScoreSetter.ScoreSetterBuilder() _
                    .Logger(oLogger) _
                    .Build()

            oScoreSetter.Score(cLocationCollection, _attributesscoring)

            Try
                Dim first As Boolean = True
                oLogger.Write("Scoring Results:")
                oLogger.writeSeperator("=", 80)
                For Each LoadRow As DataRow In cLocationCollection
                    Dim DataRowStr, CaptionRowStr As String
                    If first Then
                        oLogger.Write("LOCATION".PadRight(23) & "|" & "WAREHOUSEAREA".PadRight(23) & "|" & "SCORE".PadRight(10))
                        oLogger.writeSeperator("-", 80)
                        oLogger.Write(Convert.ToString(LoadRow("LOCATION")).PadRight(23) & "|" & Convert.ToString(LoadRow("WAREHOUSEAREA")).PadRight(23) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                        first = False
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write(Convert.ToString(LoadRow("LOCATION")).PadRight(22) & "|" & Convert.ToString(LoadRow("WAREHOUSEAREA")).PadRight(22) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                        End If
                    End If
                Next

            Catch ex As Exception
                oLogger.Write("Exception in writing loc assign scoring " & ex.Message)
            End Try


            If Not oLogger Is Nothing Then
                oLogger.Write("Finished scoring the location collection...")
            End If
        End Sub

        Public Sub Score(ByRef cLocationCollection As List(Of LocationDTO), Optional ByVal oLogger As LogHandler = Nothing)
            If Not oLogger Is Nothing Then
                oLogger.Write("Start scoring the location collection[New Scoring Algorithm Implementation]...")
            End If
            If cLocationCollection.Count = 0 Then Return

            Dim scoringStats As String = String.Empty

            Dim oScoreSetter As New Made4Net.Algorithms.Scoring.PutAwayScoreSetter()

            oScoreSetter.Score(cLocationCollection, _attributesscoring, scoringStats)

            Try
                Dim first As Boolean = True
                oLogger.Write("Scoring Results:")
                oLogger.writeSeperator("=", 80)
                For Each LoadRow As LocationDTO In cLocationCollection
                    Dim DataRowStr, CaptionRowStr As String
                    If first Then
                        oLogger.Write("LOCATION".PadRight(23) & "|" & "WAREHOUSEAREA".PadRight(23) & "|" & "SCORE".PadRight(10))
                        oLogger.writeSeperator("-", 80)
                        oLogger.Write(LoadRow.LOCATION.PadRight(23) & "|" & LoadRow.WAREHOUSEAREA.PadRight(23) & "|" & Convert.ToString(LoadRow.SCORE).PadRight(10))
                        first = False
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write(LoadRow.LOCATION.PadRight(22) & "|" & LoadRow.WAREHOUSEAREA.PadRight(22) & "|" & Convert.ToString(LoadRow.SCORE).PadRight(10))
                        End If
                    End If
                Next
                oLogger.writeSeperator("=", 80)
                oLogger.Write("Scoring Statistics")
                oLogger.Write(scoringStats)

            Catch ex As Exception
                oLogger.Write("Exception in writing loc assign scoring " & ex.Message)
            End Try


            If Not oLogger Is Nothing Then
                oLogger.Write("Finished scoring the location collection[New Scoring Algorithm Implementation]...")
            End If
        End Sub

        Public Sub DeleteAll()
            Dim sql As String = String.Format("Delete from PUTAWAYPOLICYSCORING where STRATEGYID ={0}", Made4Net.Shared.FormatField(_strategyid))
            DataInterface.RunSQL(sql)
            _attributesscoring.Clear()
        End Sub

#Region "Old Scoring"

        'Public Sub Score(ByRef cLocationCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
        '    If cLocationCollection.Length = 0 Then Return
        '    ClearScore(cLocationCollection)
        '    For idx As Int32 = 0 To _attributesscoring.Count - 1
        '        If _attributesscoring(idx) Is DBNull.Value Or _attributesscoring(idx) Is Nothing Or _attributesscoring(idx) = 0 Then
        '            'Do Nothing
        '        Else
        '            If _attributesscoring(idx) < 0 Then
        '                Sort(cLocationCollection, _attributesscoring.Keys(idx), SortOrder.Descending)
        '            Else
        '                Sort(cLocationCollection, _attributesscoring.Keys(idx), SortOrder.Ascending)
        '            End If
        '            setScore(cLocationCollection, _attributesscoring.Keys(idx), Math.Abs(_attributesscoring(idx)))
        '        End If
        '    Next
        '    Sort(cLocationCollection, "Score", SortOrder.Descending)
        'End Sub

        'Protected Sub setScore(ByRef cLocationCollection As DataRow(), ByVal sFieldName As String, ByVal dAttributeScore As Decimal)
        '    Dim iNumValues As Int32
        '    Dim iValueIdx As Int32 = 0
        '    Dim oVal As Object = cLocationCollection(0)(sFieldName)
        '    iNumValues = getNumValues(cLocationCollection, sFieldName)
        '    For idx As Int32 = 0 To cLocationCollection.Length - 1
        '        If (oVal Is Nothing Or oVal Is DBNull.Value) And (cLocationCollection(idx)(sFieldName) Is Nothing Or cLocationCollection(idx)(sFieldName) Is DBNull.Value) Then
        '            ' Do Nothing - Same Values
        '        ElseIf (cLocationCollection(idx)(sFieldName) Is Nothing Or cLocationCollection(idx)(sFieldName) Is DBNull.Value) Then
        '            oVal = cLocationCollection(idx)(sFieldName)
        '            iValueIdx = iValueIdx + 1
        '        ElseIf oVal <> cLocationCollection(idx)(sFieldName) Then
        '            oVal = cLocationCollection(idx)(sFieldName)
        '            iValueIdx = iValueIdx + 1
        '        End If
        '        cLocationCollection(idx)("score") = cLocationCollection(idx)("score") + (dAttributeScore - (iValueIdx * dAttributeScore / iNumValues))
        '    Next
        'End Sub

        'Protected Function getNumValues(ByRef cLocationCollection As DataRow(), ByVal sFieldName As String)
        '    Dim iNumValues As Int32 = 1
        '    Dim val As Object = cLocationCollection(0)(sFieldName)
        '    For idx As Int32 = 0 To cLocationCollection.Length - 1
        '        If (val Is Nothing Or val Is DBNull.Value) And (cLocationCollection(idx)(sFieldName) Is Nothing Or cLocationCollection(idx)(sFieldName) Is DBNull.Value) Then
        '            ' Do Nothing - Same Values
        '        ElseIf (cLocationCollection(idx)(sFieldName) Is Nothing Or cLocationCollection(idx)(sFieldName) Is DBNull.Value) Then
        '            val = cLocationCollection(idx)(sFieldName)
        '            iNumValues = iNumValues + 1
        '        ElseIf cLocationCollection(idx)(sFieldName) <> val Then
        '            val = cLocationCollection(idx)(sFieldName)
        '            iNumValues = iNumValues + 1
        '        End If
        '    Next
        '    Return iNumValues
        'End Function

        'Protected Sub ClearScore(ByRef cLocationCollection As DataRow())
        '    For Each oLocationRecord As DataRow In cLocationCollection
        '        oLocationRecord("Score") = 0
        '    Next
        'End Sub

        'Protected Sub Sort(ByRef cLocationCollection As DataRow(), ByVal sFieldName As String, ByVal eSortOrder As SortOrder)
        '    Dim bSorted As Boolean = False
        '    For idx As Int32 = 1 To cLocationCollection.Length - 1
        '        bSorted = True
        '        For jdx As Int32 = cLocationCollection.Length - 1 To idx Step -1
        '            Select Case eSortOrder
        '                Case SortOrder.Ascending
        '                    If (Not cLocationCollection(jdx).IsNull(sFieldName)) Then
        '                        If cLocationCollection(jdx - 1).IsNull(sFieldName) Then
        '                            Swap(cLocationCollection(jdx), cLocationCollection(jdx - 1))
        '                            bSorted = False
        '                        ElseIf (cLocationCollection(jdx)(sFieldName) < cLocationCollection(jdx - 1)(sFieldName)) Then
        '                            Swap(cLocationCollection(jdx), cLocationCollection(jdx - 1))
        '                            bSorted = False
        '                        End If
        '                    End If
        '                Case SortOrder.Descending
        '                    If (Not cLocationCollection(jdx).IsNull(sFieldName)) Then
        '                        If cLocationCollection(jdx - 1).IsNull(sFieldName) Then
        '                            Swap(cLocationCollection(jdx), cLocationCollection(jdx - 1))
        '                            bSorted = False
        '                        ElseIf (cLocationCollection(jdx)(sFieldName) > cLocationCollection(jdx - 1)(sFieldName)) Then
        '                            Swap(cLocationCollection(jdx), cLocationCollection(jdx - 1))
        '                            bSorted = False
        '                        End If
        '                    End If
        '            End Select
        '        Next
        '        If bSorted Then Exit For
        '    Next
        'End Sub

        'Protected Enum SortOrder
        '    Ascending
        '    Descending
        'End Enum

        'Protected Sub Swap(ByRef oDataRow1 As DataRow, ByRef oDataRow2 As DataRow)
        '    Dim tempDataRow As DataRow = oDataRow1
        '    oDataRow1 = oDataRow2
        '    oDataRow2 = tempDataRow
        'End Sub

#End Region

#End Region

    End Class

#End Region

End Class

#Region "Putaway Policy"

<CLSCompliant(False)> Public Class PutawayPolicy

#Region "Variables"

#Region "Primary Keys"
    Dim _putawaypolicy As String
#End Region

#Region "Other Fields"
    Dim _policyname As String
    Dim _adddate As DateTime
    Dim _adduser As String
    Dim _editdate As DateTime
    Dim _edituser As String
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("PutawayPolicy={0}", Made4Net.Shared.FormatField(_putawaypolicy))
        End Get
    End Property

    Public Property PutawayPolicy() As String
        Get
            Return _putawaypolicy
        End Get
        Set(ByVal value As String)
            _putawaypolicy = value
        End Set
    End Property

    Public Property PolicyName() As String
        Get
            Return _policyname
        End Get
        Set(ByVal value As String)
            _policyname = value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal value As DateTime)
            _adddate = value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal value As String)
            _adduser = value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal value As DateTime)
            _editdate = value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal value As String)
            _edituser = value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pPutawayPolicy As String)
        _putawaypolicy = pPutawayPolicy
    End Sub

#End Region

#Region "Methods"

    Public Sub Delete()
        Dim sql As String = String.Format("Delete from PUTAWAYPOLICY where {0}", WhereClause)
        DataInterface.RunSQL(sql)

        sql = String.Format("Delete from PUTAWAYPOLICYDETAIL where {0}", WhereClause)
        DataInterface.RunSQL(sql)

        sql = String.Format("Delete from PUTAWAYPOLICYScoring where strategyid ={0}", Made4Net.Shared.FormatField(_putawaypolicy))
        DataInterface.RunSQL(sql)
    End Sub

#End Region


End Class

#End Region