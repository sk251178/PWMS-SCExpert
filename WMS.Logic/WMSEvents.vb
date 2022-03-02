<CLSCompliant(False)> Public Class WMSEvents

    Public Shared ReadOnly Property EventDescription(ByVal eventid As Int32) As String
        Get
            Try
                Return Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("SELECT DESCRIPTION FROM EVENTS WHERE EVENTID = {0}", eventid.GetHashCode))
            Catch ex As Exception
                Return String.Empty
            End Try
        End Get
    End Property

    Public Shared Function getRegisteredProcesses(ByVal eventid As Int32) As Made4Net.DataAccess.Collections.GenericCollection
        Dim regs As New Made4Net.DataAccess.Collections.GenericCollection
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim evreg As EVENTREG
        Dim sql As String = String.Format("SELECT EVENTID, QUEUENAME, ISNULL(PRIORITY, 0) AS PRIORITY, ISNULL(SYNCHRONIZE, 0) AS SYNCHRONIZE FROM EVENTSREGISTRATION WHERE EVENTID = {0} ORDER BY PRIORITY", Made4Net.Shared.Util.FormatField(eventid))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            evreg = New EVENTREG
            evreg.EVENTID = eventid
            evreg.REGQUEUENAME = Convert.ToString(dr("QUEUENAME"))
            evreg.PRIORITY = Convert.ToInt32(dr("PRIORITY"))
            evreg.SYNCHRONIZEDQUEUE = Convert.ToBoolean(dr("SYNCHRONIZE"))
            regs.Add(evreg)
        Next
        Return regs
    End Function

    Public Shared Function GetEventTransactionType(ByVal eventid As Int32) As String
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT isnull(EVENTTRANSTYPE,eventtype) FROM EVENTS WHERE EVENTID = '" & eventid & "'")
    End Function

    Public Enum EventType
        SkuCreated = 10
        SkuUpdated = 11
        SkuUOMSettled = 12
        OrderPalnned = 19
        OrderStatusChanged = 20
        OrderShipped = 21
        OrderStaged = 22
        OutboundOrderCreated = 23
        OutboundOrderUpdated = 24
        OutboundOrderPicked = 25
        OutboundOrderDeleted = 26
        OutboundOrderLineCreated = 27
        OutboundOrderLineUpdated = 28
        OutboundOrderLineDeleted = 29
        LoadStatusChanged = 30
        LoadUnitsChanged = 31
        LoadLocationChanged = 32
        LoadUomChanged = 33
        LoadDelete = 34
        LoadAttributeChanged = 35
        SnapShot = 36
        LoadCount = 37
        LoadMerge = 38
        LoadSplit = 39
        ReceiptClose = 40
        CreateLoad = 41
        ShipLoad = 42
        PickLoad = 43
        LoadUpdated = 44
        LoadPutaway = 45
        LoadLoaded = 46
        LoadUnloaded = 461
        LoadPacked = 47
        LoadUnPacked = 48
        LoadReplenish = 49
        PickListCompleted = 50
        'PWMS-756 - Not Used start
        'PickListCanceled = 51
        'PWMS-756 - Not Used end 
        PickListUnAlloc = 52
        PickListPick = 53
        PickListReleased = 54
        PickListLineCancel = 55
        PickListLineUnAlloc = 56
        PickListLineUnPick = 57
        ShipmentShip = 60
        ShipmentLoaded = 61
        ShipmentStatusChanged = 62
        ShipmentAtDock = 63
        ShipmentLoading = 64
        NormalReplenishment = 70
        Replenishment = 71
        ZoneReplenishment = 72
        FullReplenishment = 73
        PartialReplenishment = 74
        OpportunityReplenishment = 75
        ReplenishmentCompleted = 76
        ManualReplenishment = 77
        BatchReplenishment = 78
        RoutingSetStatusChanged = 80
        BillingChargePosted = 90
        WorkOrderPlanned = 100
        WorkOrderPicked = 101
        WorkOrderExecuted = 102
        WorkOrderCompleted = 103
        WorkOrderCancelled = 104
        WorkOrderLoadProduced = 105
        WorkOrderLoadConsumed = 106
        WorkOrderCreated = 107
        WorkOrderUpdated = 108
        ConsolidationRequested = 120
        InboundCreated = 130
        InboundUpdated = 131
        InboundLineCreated = 132
        InboundLineUpdated = 133
        InboundLineDeleted = 134
        ReceiptCreated = 140
        ReceiptUpdated = 141
        ReceiptLineCreated = 142
        ReceiptLineUpdated = 143
        ReceiptCancelled = 144
        ReceiptAtDock = 145
        'PWMS-756 - Not Used start
        'ReceiptLineDeleted = 146
        'PWMS-756 - Not Used end 
        ConigneeCreated = 150
        ConsigneeUpdated = 151
        CompanyCreated = 160
        CompanyUpdated = 161
        AgreementCreated = 170
        AgreementUpdated = 171
        AgreementLineCreated = 172
        AgreementLineUpdated = 173
        PicklistCreated = 180
        PicklistUpdated = 181
        PicklistLineCreated = 182
        PicklistLineUpdated = 183
        'CAD- Added eventtype for pick list assignement
        PickListAssign = 187
        LocationCreated = 190
        LocationUpdated = 191

        'Added for RWMS-2510 Start
        LocationDeleted = 198
        'Added for RWMS-2510 End

        PickLocationCreated = 192
        PickLocationUpdated = 193

        'Added for RWMS-2510 Start
        PickLocationDeleted = 197
        'Added for RWMS-2510 End

        LocationCount = 194
        LocationProblem = 195
        'Added for RWMS-1554 and RWMS-1506 Start   
        CancelLocationProblem = 196
        'Added for RWMS-1554 and RWMS-1506 End 
        ContactCreated = 200
        ContactUpdated = 201
        TaskCreated = 210
        TaskUpdated = 211
        TaskAssigned = 212
        TaskReleased = 213
        TaskCancelled = 214
        TaskCompleted = 215
        WaveCreated = 220
        WaveUpdated = 221
        WaveCompleted = 222
        WaveCanceled = 223
        WaveAssigned = 224
        WavePlan = 225
        WaveRelease = 226
        WavePlaned = 227
        WaveCancelException = 228
        ContainerCreated = 230
        ContainerUpdated = 231
        ContainerPutaway = 232
        'RWMS-2075 and RWMS-1725 Commented Start   
        ''RWMS-745   
        'ContainerOpenClose = 233   
        ''RWMS-745   
        'RWMS-2075 and RWMS-1725 Commented End   

        'RWMS-2075 and RWMS-1725 Added Start   
        ContainerClosed = 234
        'RWMS-2075 and RWMS-1725 Added End 

        ShipmentCreated = 240
        ShipmentUpdated = 241
        CarrierCreated = 250
        CarrierUpdated = 251
        VehicleCreated = 260
        VehicleUpdated = 261
        ASNDetailCreated = 270
        ASNDetailUpdated = 271
        OutboundOrderPacked = 280
        OutboundOrderLoaded = 281
        AssignOutboundOrderToWave = 282
        AssignOutboundOrderToShipment = 283
        OutboundOrderCancelled = 284
        OutBoundOrderVerified = 285
        'PWMS-756 - Not Used start
        'DeAssignOutboundOrderFromWave = 286
        'PWMS-756 - Not Used end 
        LocationCycleCounts = 290
        LoadCycleCounts = 291
        CountingCompleted = 292
        CountingCancelled = 293
        FlowThroughStaged = 297
        FlowThroughPacked = 298
        FlowThroughLoaded = 299
        FlowThroughShipped = 300
        'PWMS-756 - Not Used start
        'FlowThroughLineCreated = 301
        'FlowThroughLineUpdated = 302
        'PWMS-756 - Not Used end 
        FlowThroughLineDeleted = 303
        FlowThroughCreated = 304
        FlowThroughUpdated = 305
        'PWMS-756 - Not Used start Added for PWMS-816
        YardEntryCreated = 310
        YardEntryUpdated = 311
        YardEntryStatusChanged = 312
        YardEntryLocationChanged = 313
        YardEntryScheduled = 314
        YardEntryCheckIn = 315
        YardEntryCheckOut = 316
        'PWMS-756 - Not Used end  Ended for PWMS-816
        RequestPickup = 340
        CompleteOrder = 350
        ' Uncommented for RWMS-2233 : Otherwise the OrderPlanned(19) is fired multiple times.
        'PWMS-756 - Not Used start
        PlanOrder = 351
        'PWMS-756 - Not Used end 
        ' Uncommented for RWMS-2233 : Otherwise the OrderPlanned(19) is fired multiple times.
        ShipmentComplete = 361
        SendLocToLocAssign = 370
        'PWMS-756 - Not Used start
        'RouteCreated = 400
        'RouteUpdated = 401
        'RouteStatusChanged = 402
        'RouteLoaded = 404
        'RouteDeparted = 404
        'RouteReturned = 405
        'RouteConfirmed = 406
        'RouteClosed = 407
        'RoutePlanned = 408
        'RouteStopTaskCompleted = 410
        'RouteStopTaskCancelled = 411
        'RouteStopTaskStatusChanged = 412
        'PWMS-756 - Not Used end 
        PackingListCreated = 420
        PackingListUpdated = 421
        PackingListCancelled = 422
        PackingListShipped = 423
        PackingListLoadPacked = 424
        PackingListLoadUnPacked = 425
        PutawayOverried = 440
        UnpickLoad = 450
        LoadVerified = 451
        LoadStaged = 452
        UnReceiveLoad = 453
        ShiftInstanceCreated = 460 'status new
        ShiftInstanceStarted = 470
        ShiftInstanceClosed = 480
        ShiftInstanceCanceled = 490
        OutboundOrderLineCancelExcept = 521
        TransShipmentCreated = 522
        TransShipmentUpdated = 523
        RECOVRRD = 2001
        WGTOVRRD = 2002
        LOCASGNPRB = 2004
        SHORTPICK = 2005
        SKUVERIFICATION = 2006
        REPORTPROBLEM = 2007
        'Added for PWMS-810 and RWMS-791   
        SHIPMENTMOVED = 2012
        ORDERLINEMOVED = 2013
        'End Added for PWMS-810 and RWMS-791  
        REVIEW_REPLEN = 51

        ' RWMS-1497 : New event for labor the update the laboraudit
        LaborTaskUpdated = 2014
        ' RWMS-1497 : New event for labor the update the laboraudits

        'Added for RWMS-2540 Start
        LOGIN = 2015
        LOGOUT = 2016
        'Added for RWMS-2540 End
        BReplenPlanned = 3000
        BReplenReleased = 3001
        BReplenLetdown = 3002
        BReplenUnload = 3003
        BReplenLineLetdown = 3004
        BReplenLineUnload = 3005
        BReplenScheduled = 3006
        BReplenCanceled = 3007
        BReplenUnAlloc = 3008
        BReplenCompleted = 3009
    End Enum

End Class

<CLSCompliant(False)> Public Class EVENTREG

    Public EVENTID As Int32
    Public REGQUEUENAME As String
    Public PRIORITY As Int32
    Public SYNCHRONIZEDQUEUE As Boolean

End Class
