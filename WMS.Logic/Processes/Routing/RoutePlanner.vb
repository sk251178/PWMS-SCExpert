Imports Made4Net.GeoData
Imports Made4Net.Algorithms
Imports Made4Net.Algorithms.GeneticAlgorithm
Imports Made4Net.Algorithms.Interfaces
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert

<CLSCompliant(False)> Public Class RoutePlanner

#Region "Variables"

    Protected _RoutingSet As RoutingSet                     'Main Object Container to be routed
    Protected _RoutingStrat As RoutingStrategies            'Routing Startegies Collection
    Protected _RoutingReqs As RoutingRequirements           'The required orders to the routing proccess
    ''Protected _GenerationCount As Int32
    ''Protected _PopulationSize As Int32
    Protected _RunId As String

    Protected _PreDefinedRunId As String = String.Empty
    Protected _vRoutingRequirementsName As String = String.Empty

    Protected _DistMatrix As GeoNetworkDistanceCache
    ''Protected Shared _DistMatrix As GeoNetworkDistanceCache

    Protected oLogger As LogHandler
    Protected Shared _VehicleTypeCache As Hashtable

    Protected _territorycoll As New ArrayList

    Protected _isDistanceAdd As Integer = 0
    Protected _distcosttype As Integer = 0
    Protected _isdistcost As Integer = 0
    Protected _UserId As String = "RoutingPlannner"

    Protected _runCounter As Integer = 0

    ''_replantype   0 - replan
    ''              1 - backhaul
    Protected _replantype As Integer = 0

    Protected _TerritoryPointsHash As Hashtable = New Hashtable()
    Protected _GeoTerritoryCollectionHash As Hashtable = New Hashtable()
#End Region

#Region "Constructors"

    Public Sub New(Optional ByVal pLogger As LogHandler = Nothing)
        If Not pLogger Is Nothing Then
            oLogger = pLogger
        End If
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)
    End Sub

#End Region

#Region "Methods"


#Region "Plan RoutingSet"

    Public Sub PlanRoutesWithRunID(ByVal pRunID As String, _
                    ByVal pRoutingSetId As String, _
                            ByVal sWriteStats As Boolean, _
                            ByVal pUser As String, _
                            Optional ByVal Network As GeoNetworkHashtable = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write("")
            oLogger.writeSeperator("+", 60)
            oLogger.Write("Start Plan with RUNID " & pRunID & "...")
        End If

        _PreDefinedRunId = pRunID
        _vRoutingRequirementsName = "vAdditionalRoutingRequirements"
        PlanRoutes(pRoutingSetId, sWriteStats, pUser, Network)
    End Sub


    Public Sub RePlanRoutes(ByVal pRunID As String, _
                    ByVal pUser As String, _
                     ByVal pReplanType As Integer, _
                     ByVal sWriteStats As Boolean)
        Try
            _replantype = pReplanType


            If _replantype = 1 Then

            End If
            _runCounter += 1
            If Not oLogger Is Nothing Then
                oLogger.Write(" User: " & pUser & " RunIDs: " & pRunID & " start replanning for type: " & _replantype & "...")
            End If
            _UserId = pUser

            Dim sql As String

            If _replantype = 0 Then
                sql = String.Format("select top 1 routeset  from  dbo.vReplanRoutes where runid={0}", pRunID)
            ElseIf _replantype = 1 Then
                sql = String.Format("select top 1 routeset  from  dbo.vReplanBackHaulRoutes where runid={0}", pRunID)
            End If

            Dim dtRoutingSetId As New DataTable
            DataInterface.FillDataset(sql, dtRoutingSetId)
            If dtRoutingSetId.Rows.Count = 0 Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Routing set not found for RunID: " & pRunID)
                End If
                Return
            End If
            Dim pRoutingSetId As String = dtRoutingSetId.Rows(0)("routeset")

            _RoutingSet = New RoutingSet(pRoutingSetId, False)
            If Not oLogger Is Nothing Then
                oLogger.Write("Routing Set Created... ")
            End If


            If Not oLogger Is Nothing Then
                oLogger.Write("Loading Distances Matrix for Orders Assigned to RoutingSet...")
            End If

            Dim DistSQL As String = BuildDistMatrixQuery(pRoutingSetId)
            If _DistMatrix Is Nothing Then
                _DistMatrix = GeoNetworkItem.GetDistancesCache(DistSQL)
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write("Distance Matrix " & _DistMatrix.Count & " Edges Loaded...")
            End If

            _RunId = pRunID
            If Not oLogger Is Nothing Then
                oLogger.Write("The RunId For replanning: " & _RunId)
                oLogger.Write("Loading Routing Plan Policies and new Requirements...")
            End If


            ''''''''''''''''''''''''
            _RoutingStrat = New RoutingStrategies(oLogger)                                   ' Create Planning Strategies
            If _replantype = 0 Then
                _RoutingReqs = New RoutingRequirements(pRoutingSetId, pRunID, _replantype, oLogger)
            ElseIf _replantype = 1 Then
                _RoutingReqs = New RoutingRequirements(pRoutingSetId, pRunID, _replantype, oLogger)
            End If


            If _RoutingReqs.Count > 0 Then
                If _RoutingStrat(_RoutingReqs(0).Strategy).StrategyDetails(0).DefaultVehicleType = String.Empty Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("***Undefined default strategy vehicle for replan.")
                    End If
                    Return
                End If
                AssignToTerritories(oLogger)

                tryPlacetoExistsRoute()
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("There are no matching requirements.")
                End If
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write("RePlanned. Saving results...")
                If sWriteStats AndAlso _replantype = 0 Then
                    WriteStatistics("replan")
                ElseIf sWriteStats AndAlso _replantype = 1 Then
                    WriteStatistics("replanbackhaul")
                End If
            End If

        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("RePlanRoutes Error: Complete with error: " & ex.ToString())
            End If

        Finally
            Try
                If Not _RoutingSet Is Nothing Then
                    _RoutingSet.PlanComplete(_UserId)
                End If
                If Not oLogger Is Nothing Then
                    oLogger.Write("Replan Complete.")
                End If
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write("***Complete Error:" & ex.ToString())
                End If
            End Try
        End Try

    End Sub


    Protected Function findBestReqPos(ByVal req As RoutingRequirement, ByVal pRoute As Route, _
            ByRef bestpos As Integer, ByRef bestDistance As Double, ByRef bestTime As Double) As Double


        Dim addDistance As Double = 0D
        Dim addTime As Double = 0D

        Dim NewReq As GeoPointNode = req.TargetGeoPoint

        Dim oStratDet As RoutingStrategyDetail = _RoutingStrat(req.Strategy).StrategyDetails(0)


        'find first apropriate position fro pickup
        Dim startPos As Integer = 0

        If oStratDet.DefaultVehicleType = String.Empty Then
            Return Integer.MaxValue
        End If
        Dim oVehicleType As VehicleType = New VehicleType(oStratDet.DefaultVehicleType, True)

        Dim volumeLimit As Double = oVehicleType.TOTALVOLUME * oStratDet.AllowPickupBeforeDelivery
        Dim WeightLimit As Double = oVehicleType.TOTALWEIGHT * oStratDet.AllowPickupBeforeDelivery
        Dim currentVolume As Double = 0
        Dim currentWeight As Double = 0

        If _replantype = 1 Then ''backhaul orders
            startPos = pRoute.Stops.Count
        Else
            If req.PDType = PDType.Pickup Then

                If oStratDet.AllowPickupBeforeDelivery = 1 Then
                    startPos = pRoute.Stops.Count
                ElseIf oStratDet.AllowPickupBeforeDelivery > 0 Then

                    Dim isExit As Boolean = False
                    For Each oRouteSop As RouteStop In pRoute.Stops
                        startPos += 1
                        For Each oStopTask As RouteStopTask In oRouteSop.RouteStopTask
                            currentVolume += oStopTask.StopDetailVolume
                            currentWeight += oStopTask.StopDetailWeight
                            If currentVolume > volumeLimit And currentWeight > WeightLimit Then
                                isExit = True
                                Exit For
                            End If
                        Next
                        If isExit = True Then Exit For
                    Next
                    If isExit = False Then startPos = pRoute.Stops.Count
                End If
            End If
        End If


        ''
        For i As Integer = startPos To pRoute.Stops.Count
            Dim tempDist As Double = Integer.MaxValue
            Dim tmpDrivingTime As Double = Double.MaxValue

            If i = 0 Then
                Dim Target As GeoPointNode = New GeoPointNode(pRoute.Stops(i).PointId)
                GeoNetworkItem.GetDistance(NewReq, Target, addDistance, addTime, _UserId, _
                    _isDistanceAdd, _isdistcost, _distcosttype)

                If (req.PDType = PDType.Delivery And (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops)) Or _
                    (req.PDType = PDType.Pickup And (addDistance > oStratDet.MaxdistanceBetweenDeliveryAndPickup)) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If
            ElseIf i < pRoute.Stops.Count Then
                Dim Source As GeoPointNode = New GeoPointNode(pRoute.Stops(i - 1).PointId)
                Dim Target As GeoPointNode = New GeoPointNode(pRoute.Stops(i).PointId)

                GeoNetworkItem.GetDistance(Source, NewReq, tempDist, tmpDrivingTime, _UserId, _
                    _isDistanceAdd, _isdistcost, _distcosttype)
                If (req.PDType = PDType.Delivery And (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops)) Or _
                    (req.PDType = PDType.Pickup And (addDistance > oStratDet.MaxdistanceBetweenDeliveryAndPickup)) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If

                GeoNetworkItem.GetDistance(NewReq, Target, addDistance, addTime, _UserId, _
                    _isDistanceAdd, _isdistcost, _distcosttype)
                If (req.PDType = PDType.Delivery And (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops)) Or _
                    (req.PDType = PDType.Pickup And (addDistance > oStratDet.MaxdistanceBetweenDeliveryAndPickup)) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If


                Dim currDist, currTime As Double
                GeoNetworkItem.GetDistance(Source, NewReq, currDist, currTime, _UserId, _
                    _isDistanceAdd, _isdistcost, _distcosttype)


                addDistance += tempDist - currDist
                addTime += tmpDrivingTime - currTime

            ElseIf i = pRoute.Stops.Count Then
                Dim Source As GeoPointNode = New GeoPointNode(pRoute.Stops(i - 1).PointId)
                GeoNetworkItem.GetDistance(Source, NewReq, addDistance, addTime, _UserId, _
                    _isDistanceAdd, _isdistcost, _distcosttype)
                If (req.PDType = PDType.Delivery And (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops)) Or _
                    (req.PDType = PDType.Pickup And (addDistance > oStratDet.MaxdistanceBetweenDeliveryAndPickup)) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If

            End If
            If bestDistance > addDistance Then
                bestpos = i
                bestDistance = addDistance
                bestTime = addTime
            End If
        Next

        Return addDistance
    End Function

    Protected Function PlaceReqtoRoute(ByVal pReq As RoutingRequirement, _
            ByRef pRoute As Route, _
            ByVal theBestPos As Integer, _
            ByVal adddistance As Double, _
            ByVal addtime As Double) As Boolean
        If Not oLogger Is Nothing Then
            oLogger.Write("Try Place Req to Route: " & pRoute.RouteId & " OrderId: " & pReq.OrderId & "....")
        End If


        '' add stop
        Dim NewStopNumber As Integer
        'If theBestPos = 0 Then
        '    NewStopNumber = 1
        'ElseIf theBestPos = pRoute.Stops.Count Then
        '    NewStopNumber = theBestPos + 1
        'Else
        '    NewStopNumber = theBestPos + 2
        'End If
        NewStopNumber = theBestPos + 1


        Dim oContact As Contact = New Contact(pReq.ContactId, True)

        Dim oStopTaskType As String
        Select Case pReq.PDType
            Case PDType.Delivery
                oStopTaskType = StopTaskType.Delivery
            Case PDType.Pickup
                oStopTaskType = StopTaskType.PickUp
            Case PDType.General
                oStopTaskType = StopTaskType.General
        End Select

        Dim oRouteStop As RouteStop = New RouteStop()
        oRouteStop.RouteID = pRoute.RouteId
        oRouteStop.StopNumber = NewStopNumber
        oRouteStop.PointId = oContact.POINTID


        Dim oRouteStopTask As RouteStopTask = New RouteStopTask()
        oRouteStopTask.RouteID = pRoute.RouteId
        oRouteStopTask.StopNumber = NewStopNumber


        oRouteStopTask.Contact = oContact
        oRouteStopTask.ContactId = pReq.ContactId
        oRouteStopTask.StopDetailVolume = pReq.OrderVolume
        oRouteStopTask.StopDetailWeight = pReq.OrderWeight
        oRouteStopTask.StopDetailValue = pReq.OrderValue
        oRouteStopTask.StopTaskType = oStopTaskType
        oRouteStopTask.TransportationClass = pReq.TransportationClass

        oRouteStop.RouteStopTask.Add(oRouteStopTask)


        Dim oNewRouteStopCollection As ArrayList = New ArrayList()
        For Each oRStop As RouteStop In pRoute.Stops
            If oRStop.StopNumber = NewStopNumber Then
                oNewRouteStopCollection.Add(oRouteStop)
            End If
            If oRStop.StopNumber >= NewStopNumber Then
                oRStop.StopNumber += 1
            End If
            oNewRouteStopCollection.Add(oRStop)
        Next


        pRoute.Stops.Clear()
        For Each oRStop As RouteStop In oNewRouteStopCollection
            pRoute.Stops.Add(oRStop)
        Next


        Dim msg As String

        pRoute.RecalculateArrivalTime(False)

        Dim isFeasibile As Boolean = pRoute.CheckFeasibility(msg, oLogger)
        If Not isFeasibile Then
            If Not oLogger Is Nothing Then
                oLogger.Write(pRoute.RouteId & ": " & msg)
            End If
            Return False
        End If


        '' 22/11/2011 
        ''check and update next routes in the same Trip Group
        If Not checkAndUpdateTripGroupRoutes(pRoute, oLogger) Then
            Return False
        End If
        ''''''''''''''''''''


        ''delete from obj
        pRoute.Stops.Remove(oRouteStop)
        For Each oRStop As RouteStop In pRoute.Stops
            If oRStop.StopNumber > NewStopNumber Then
                oRStop.StopNumber -= 1
            End If
        Next

        AddRouteStop(pRoute, pReq, 0, 0, NewStopNumber)


        Dim oRecalcRoute As New Route(pRoute.RouteId)

        oRecalcRoute.RecalculateArrivalTime()
        oRecalcRoute.SetTotalDistance(pRoute.TotalDistance + adddistance)
        oRecalcRoute.SetTotalTime(pRoute.TotalTime + addtime)
        oRecalcRoute.SetTotalWeight(pRoute.TotalWeight + pReq.OrderWeight)
        oRecalcRoute.SetTotalVolume(pRoute.TotalVolume + pReq.OrderVolume)
        Dim Routecost As Decimal = oRecalcRoute.CalculateRouteCost()
        oRecalcRoute.SetRouteCost(Routecost)

        If Not oLogger Is Nothing Then
            oLogger.Write("New Distance: " & oRecalcRoute.TotalDistance)
            oLogger.Write("New Time: " & oRecalcRoute.TotalTime)
            oLogger.Write("New Weight: " & oRecalcRoute.TotalWeight)
            oLogger.Write("New Volume: " & oRecalcRoute.TotalVolume)
            oLogger.Write("New Cost: " & oRecalcRoute.RouteCost)
        End If

        'Dim oOrd As OutboundOrderHeader
        'oOrd = New OutboundOrderHeader(pReq.Consignee, pReq.OrderId, False)
        'oOrd.SetRoute(oRecalcRoute.RouteId, NewStopNumber, "RePlan")

        oRecalcRoute.SetStatus(RouteStatus.Assigned, DateTime.Now, "RePlan")
        pRoute = oRecalcRoute
        If Not oLogger Is Nothing Then
            oLogger.Write("Route: " & oRecalcRoute.RouteId & " status changed to Assigned")

        End If


        If Not oLogger Is Nothing Then
            oLogger.Write("Route " & pRoute.RouteId & ". Placed new order " & pReq.OrderId)
        End If
        Return True

    End Function


    Public Shared Function checkAndUpdateTripGroupRoutes(ByVal pRoute As Route, _
                ByVal oLogger As LogHandler) As Boolean
        Dim msg As String = String.Empty

        If pRoute.TripNum > 0 Then
            Dim oStrategyDetail As RoutingStrategyDetailCollection = _
                New RoutingStrategy(pRoute.StrategyId).StrategyDetails
            Dim TripRoutesArr As New ArrayList()
            getTripGroupRoutesArr(TripRoutesArr, pRoute)

            Dim currentRoutendDate As DateTime
            currentRoutendDate = pRoute.EndDate
            Dim isTripGroupFeasibile As Boolean

            Dim TripGroupTotalTime As Double = 0
            Dim TripGroupTotalDistance As Double = 0

            For Each tripRoute As Route In TripRoutesArr
                If tripRoute.TripNum > pRoute.TripNum Then
                    tripRoute.StartDate = currentRoutendDate.AddSeconds(oStrategyDetail(0).DepotReloadTime)
                    tripRoute.RecalculateArrivalTime(False)
                    currentRoutendDate = tripRoute.EndDate
                    isTripGroupFeasibile = tripRoute.CheckFeasibility(msg, oLogger)
                    If Not isTripGroupFeasibile Then Return False
                End If

                TripGroupTotalTime += tripRoute.TotalTime
                If TripGroupTotalTime > oStrategyDetail(0).TotalTripsAllocationTime Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("*** Total Trips Allocation Time exceeded: " & TripGroupTotalTime & ">" & _
                            oStrategyDetail(0).TotalTripsAllocationTime)
                    End If
                    Return False
                End If

                TripGroupTotalDistance += tripRoute.TotalDistance
                If TripGroupTotalDistance > oStrategyDetail(0).TotalTripsAllocationDistance Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("*** Total Trips Allocation Distance exceeded: " & TripGroupTotalDistance & ">" & _
                            oStrategyDetail(0).TotalTripsAllocationDistance)
                    End If
                    Return False
                End If
            Next

            ''if all trips feasible update arrival time
            currentRoutendDate = pRoute.EndDate
            For Each tripRoute As Route In TripRoutesArr
                If tripRoute.TripNum > pRoute.TripNum Then
                    tripRoute.SetStartDate(tripRoute.StartDate)
                    tripRoute.RecalculateArrivalTime(True)
                    currentRoutendDate = tripRoute.EndDate
                End If
            Next

        End If
        Return True
        ''''''''''''''''''''

    End Function


    Public Shared Sub getTripGroupRoutesArr(ByRef TripRoutesArr As ArrayList, _
            ByVal pRoute As Route)

        Dim sql As String
        sql = String.Format("select routeid  from  ROUTE " & _
                " where TripGroup ='{0}'  and ROUTESET ='{1}' and runid='{2}'  " & _
                " order by TripNum", _
            pRoute.TripGroup, pRoute.RouteSet, pRoute.RunId)

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows()
            Dim tripRoute As New Route(dr("routeid"))
            TripRoutesArr.Add(tripRoute)
        Next
    End Sub

    Protected Sub AddRouteStop(ByRef oRoute As Route, ByVal oReq As RoutingRequirement, _
    ByVal estArrvHour As Int32, ByVal estDepartureHour As Int32, _
        ByVal theBestPos As Integer)
        Try
            Dim oRouteStop As RouteStop
            If Not oRoute.Stops.StopExists(oReq.PointID) Then
                oRouteStop = oRoute.AddStop("", oReq.PointID, estArrvHour, estDepartureHour, _UserId, theBestPos)
            Else
                oRouteStop = oRoute.Stops.GetStop(oReq.PointID)
            End If


            Dim oStopTaskType As String
            Select Case oReq.PDType
                Case PDType.Delivery
                    oStopTaskType = StopTaskType.Delivery
                Case PDType.Pickup
                    oStopTaskType = StopTaskType.PickUp
                Case PDType.General
                    oStopTaskType = StopTaskType.General
            End Select

            oRouteStop.AddStopDetail(oStopTaskType, oRoute.RouteDate, oReq.Consignee, oReq.OrderId, _
                oReq.OrderType, oReq.TargetCompany, oReq.CompanyType, oReq.ContactObj.CONTACTID, oReq.ChkPnt, "", 0, oReq.TransportationClass, oReq.OrderVolume, oReq.OrderWeight, 0, "", StopTaskConfirmationType.None, _UserId)
        Catch ex As Exception
            oLogger.Write("AddRouteStop error: " & ex.Message)
        End Try
    End Sub

    Protected Sub tryPlacetoExistsRoute()
        If Not oLogger Is Nothing Then
            oLogger.Write("Tryinng to place to exists route... ")
        End If

        Dim RoutesByStrategy As Hashtable = LoadRoutesByStrategyanTerritory()
        Dim RoutingReqsClone As New ArrayList

        For Each reqtmp As RoutingRequirement In _RoutingReqs
            RoutingReqsClone.Add(reqtmp)
        Next

        For Each req As RoutingRequirement In RoutingReqsClone
            If req.Strategy Is Nothing OrElse req.Strategy = String.Empty Then Continue For

            Dim TerritorySetID As String = CType(_RoutingStrat(req.Strategy), RoutingStrategy).StrategyDetails(0).TerritorySetID

            ''Dim hashKey As String = TerritorySetID & "#" & req.Strategy.ToString()

            Dim territorykey As String = String.Empty
            If Not TerritorySetID Is Nothing AndAlso TerritorySetID <> String.Empty Then
                Try
                    Dim TerritoryID As String = String.Empty
                    Dim oGeoTerritoryCollection As GeoTerritoryCollection = _
                       GeoTerritory.GetTerritoriesByPoint(CType(req.TargetGeoPoint, GeoPoint), TerritorySetID)
                    If Not oGeoTerritoryCollection Is Nothing AndAlso oGeoTerritoryCollection.Count > 0 Then
                        TerritoryID = CType(oGeoTerritoryCollection(0), GeoTerritory).TERRITORYID
                    End If

                    If Not TerritoryID Is Nothing AndAlso TerritoryID <> String.Empty Then
                        territorykey = TerritoryID

                        req.TerritoryKey = territorykey
                        If _territorycoll.IndexOf(territorykey) = -1 Then
                            _territorycoll.Add(territorykey)
                        End If
                    End If
                Catch ex As Exception
                End Try
            End If



            Dim oRoutes As ArrayList = CType(RoutesByStrategy(territorykey), ArrayList)
            If oRoutes Is Nothing Then Continue For

            Dim theBestCost As Double = Integer.MaxValue
            Dim theBetsPos As Integer = 0
            Dim theBestRoute As Route
            Dim adddistance = Integer.MaxValue
            Dim addtime = Integer.MaxValue

            If Not oLogger Is Nothing Then
                oLogger.Write("Searching the best position for requirement in " & oRoutes.Count.ToString & _
                                " routes for " & _
                                "point - " & req.PointID & _
                                " contact - " & req.ContactId & _
                                " order - " & req.OrderId)
            End If

            Dim oRoutesObj As New ArrayList
            For Each CurrentRouteID As String In oRoutes

                Dim oCurrentRoute As Route = New Route(CurrentRouteID)
                Dim findPos As Integer = 0
                Dim cost As Double = findBestReqPos(req, oCurrentRoute, findPos, adddistance, addtime)
                If cost < Integer.MaxValue Then
                    oCurrentRoute.Tag = cost
                    oCurrentRoute.bestData.Clear()
                    oCurrentRoute.bestData.Add(findPos)
                    oCurrentRoute.bestData.Add(adddistance)
                    oCurrentRoute.bestData.Add(addtime)

                    oRoutesObj.Add(oCurrentRoute)
                End If

                ''comment
                If cost >= 0 AndAlso cost < theBestCost Then
                    theBestCost = cost
                    theBestRoute = oCurrentRoute
                    theBetsPos = findPos
                End If
            Next

            Dim myComparer = New RouteCostCompareClass()
            oRoutesObj.Sort(myComparer)

            For Each oRoute As Route In oRoutesObj
                If PlaceReqtoRoute(req, oRoute, oRoute.bestData(0), oRoute.bestData(1), oRoute.bestData(2)) Then
                    _RoutingReqs.Remove(req)
                    Exit For
                End If
            Next

        Next
    End Sub

    Protected Function LoadRoutesByStrategyanTerritory() As Hashtable
        Dim RoutesByStategy As New Hashtable
        If Not oLogger Is Nothing Then
            oLogger.Write("Load Routes By Strategy... ")
        End If

        For Each req As RoutingRequirement In _RoutingReqs
            If req.Strategy Is Nothing OrElse req.Strategy = String.Empty Then Continue For

            Dim TerritorySetID As String = CType(_RoutingStrat(req.Strategy), RoutingStrategy).StrategyDetails(0).TerritorySetID
            ''Dim hashKey As String = TerritorySetID & "#" & req.Strategy.ToString()
            Dim territorykey As String = String.Empty
            If Not TerritorySetID Is Nothing AndAlso TerritorySetID <> String.Empty Then
                Try
                    Dim TerritoryID As String = String.Empty
                    Dim oGeoTerritoryCollection As GeoTerritoryCollection = _
                       GeoTerritory.GetTerritoriesByPoint(CType(req.TargetGeoPoint, GeoPoint), TerritorySetID)
                    If Not oGeoTerritoryCollection Is Nothing AndAlso oGeoTerritoryCollection.Count > 0 Then
                        TerritoryID = CType(oGeoTerritoryCollection(0), GeoTerritory).TERRITORYID
                    End If

                    If Not TerritoryID Is Nothing AndAlso TerritoryID <> String.Empty Then
                        territorykey = TerritoryID ''TerritorySetID & "#" & TerritoryID

                        req.TerritoryKey = territorykey
                        If _territorycoll.IndexOf(territorykey) = -1 Then
                            _territorycoll.Add(territorykey)
                        End If
                    End If
                Catch ex As Exception
                End Try
            End If


            If Not RoutesByStategy.Contains(territorykey) Then
                Dim sql As String = String.Format("select routeid from vReplanRoutes where strategyid ={0} and runid in ({1}) and isnull(TERRITORYSET,'') =isnull({2},'') ", _
                        Made4Net.Shared.Util.FormatField(req.Strategy), _
                        _RunId, _
                        Made4Net.Shared.Util.FormatField(TerritorySetID))
                Dim dt As New DataTable
                DataInterface.FillDataset(sql, dt)
                Dim oRoutes As New ArrayList
                For Each dr As DataRow In dt.Rows()
                    oRoutes.Add(dr("routeid"))
                Next
                RoutesByStategy.Add(territorykey, oRoutes)
            End If
        Next

        Return RoutesByStategy
    End Function

    Public Sub PlanRoutes(ByVal pRoutingSetId As String, _
                            ByVal sWriteStats As Boolean, _
                            ByVal pUser As String, _
                            Optional ByVal Network As GeoNetworkHashtable = Nothing)

        Try
            _runCounter += 1

            If Not oLogger Is Nothing Then
                oLogger.Write("Start Planning...")
                oLogger.Write("Routingset: " & pRoutingSetId & " User: " & pUser & " start planning...")
            End If
            _RoutingSet = New RoutingSet(pRoutingSetId, False)
            If Not oLogger Is Nothing Then
                oLogger.Write("Routing Set Created... ")
            End If

            _UserId = pUser
            If Not oLogger Is Nothing Then
                oLogger.Write("Loading Distances Matrix for Orders Assigned to RoutingSet...")
            End If

            Dim DistSQL As String = BuildDistMatrixQuery(pRoutingSetId)
            If _DistMatrix Is Nothing Then
                _DistMatrix = GeoNetworkItem.GetDistancesCache(DistSQL)
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write("Distance Matrix " & _DistMatrix.Count & " Edges Loaded...")
            End If

            If _PreDefinedRunId = String.Empty Then
                _RunId = Made4Net.Shared.Util.getNextCounter("RoutePlanRunId")
            Else
                _RunId = _PreDefinedRunId
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write("The Run Id For the Current Run: " & _RunId)
                oLogger.Write("Loading Routing Plan Policies and Requirements...")
            End If


            If _vRoutingRequirementsName = String.Empty Then
                _vRoutingRequirementsName = "ROUTINGREQUIREMENTS"
                _RoutingReqs = New RoutingRequirements(pRoutingSetId, oLogger)
            Else
                _RoutingReqs = New RoutingRequirements(pRoutingSetId, _vRoutingRequirementsName, oLogger)
            End If

            If _RoutingReqs.Count = 0 Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("There are no matching requirements.")
                End If
            Else
                _RoutingStrat = New RoutingStrategies(oLogger)                                   ' Create Planning Strategies

                AssignToTerritories(oLogger)

                If AssignToStrategies(String.Empty, oLogger) Then
                    LoadVehiclesCache(oLogger)
                    _RoutingStrat.PlanRoutes("", _RoutingSet, _RunId, oLogger)
                End If
                ' Assign Requirments to strategies for territories
                For Each territorykey As String In _territorycoll
                    If AssignToStrategies(territorykey, oLogger) Then
                        LoadVehiclesCache(oLogger)
                        _RoutingStrat.PlanRoutes(territorykey, _RoutingSet, _RunId, oLogger)
                    End If
                Next
            End If

            Dim cntPointed As Integer
            If sWriteStats AndAlso Not oLogger Is Nothing Then
                oLogger.Write("Write statistic results...")
                cntPointed = WriteStatistics("plan")
            End If

            ''add check the flag
            'If _runCounter < 2 And cntPointed > 0 Then
            '    If Not oLogger Is Nothing Then
            '        oLogger.Write("Start Replan...")
            '        WriteStatistics()
            '    End If
            '    RePlanRoutes(pRoutingSetId, _RunId, pUser)
            'End If

        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("PlanRoutes Error: Complete with error: " & ex.ToString())
            End If
        Finally
            Try
                If Not oLogger Is Nothing Then
                    oLogger.Write("Planned. Saving results...")
                End If

                _RoutingSet.PlanComplete(_UserId)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Plan Complete.")
                End If
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write("***Complete Error:" & ex.ToString())
                End If
            End Try
        End Try
    End Sub

    Protected Function WriteStatistics(ByVal action As String) As Integer
        ''Return 0

        Dim ROUTINGREQUIREMENTS, ROUTE, ROUTESTOP, ROUTESTOPTASK As String
        If action = "plan" Then
            ROUTINGREQUIREMENTS = _vRoutingRequirementsName
            ROUTE = "ROUTE"
            ROUTESTOP = "ROUTESTOP"
            ROUTESTOPTASK = "ROUTESTOPTASK"
        ElseIf action = "replan" Then
            ROUTINGREQUIREMENTS = "vSourceROUTINGREQUIREMENTS"
            ROUTE = "vreplanroutes"
        ElseIf action = "replanbackhaul" Then
            ROUTINGREQUIREMENTS = "vSourceBackHaulROUTINGREQUIREMENTS"
            ROUTE = "vReplanBackHaulRoutes"

        End If
        Dim sql As String
        Dim cntPointed As Integer

        'sql = String.Format("with NotOnRouteTask as " & _
        '                 " ( " & _
        '                 " select orderid from {2} where routingset={0}  EXCEPT " & _
        '                 " select documentid from ROUTESTOPTASK rst join {3} r on rst.routeid=r.routeid and r.runid in ({1}) " & _
        '                 " ) " & _
        '                 " select CONTACTID, ORDERID, isnull(POINTID,'Undefined') POINTID  from  {2} " & _
        '                 " where routingset={0} and orderid in (select orderid from NotOnRouteTask) ", _
        '                    Made4Net.Shared.Util.FormatField(_RoutingSet.SetID), _
        '                    _RunId, _
        '                    ROUTINGREQUIREMENTS, ROUTE)

        sql = String.Format("select CONTACTID, ORDERID, isnull(POINTID,'Undefined') POINTID  from {2} " & _
                                 " where routingset={0} " & _
                                 " and orderid not in   (select documentid from ROUTESTOPTASK rst" & _
                                 " join {3} r on rst.routeid=r.routeid and r.ROUTESET ={0} and  r.runid in ({1})) order by POINTID,CONTACTID,ORDERID ", _
                            Made4Net.Shared.Util.FormatField(_RoutingSet.SetID), _
                            _RunId, _
                            ROUTINGREQUIREMENTS, ROUTE)

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        oLogger.Write("Total unplanned orders: " & dt.Rows().Count)
        cntPointed = 0
        For Each dr As DataRow In dt.Rows()
            If dr.Item("POINTID").ToString <> "Undefined" Then
                cntPointed += 1
            End If
            oLogger.Write("point - " & dr.Item("POINTID").ToString & _
                "  contact - " & dr.Item("CONTACTID").ToString() & _
                "  order - " & dr.Item("ORDERID").ToString())
        Next

        ''routes info
        sql = String.Format("select count(routeid) as cntroute, sum(routecost) as sumroutecost from {1} where runid in ({0})", _
           _RunId, ROUTE)
        Dim dt2 As New DataTable
        DataInterface.FillDataset(sql, dt2)
        If dt2.Rows.Count > 0 Then
            oLogger.Write("Total Planned Routes: " & dt2.Rows(0).Item("cntroute").ToString())
            oLogger.Write("Total Planned Routes Cost: " & dt2.Rows(0).Item("sumroutecost").ToString())
        End If


        sql = String.Format("select count(distinct rs.pointid) cntpointid, count(distinct rst.documentid) cntorderid " & _
                    " from {2} r " & _
                    " join ROUTESTOP rs on rs.routeid=r.routeid " & _
                    " join ROUTESTOPTASK rst on rst.routeid=r.routeid  " & _
                    " where r.runid in ({1}) and r.routeset={0} ", _
                               Made4Net.Shared.Util.FormatField(_RoutingSet.SetID), _
                               _RunId, ROUTE)

        Dim dt1 As New DataTable
        DataInterface.FillDataset(sql, dt1)
        If dt1.Rows.Count > 0 Then
            oLogger.Write("Total Planned Stops: " & dt1.Rows(0).Item("cntpointid").ToString())
            oLogger.Write("Total Planned Tasks: " & dt1.Rows(0).Item("cntorderid").ToString())
        End If

        Return cntPointed
    End Function

#End Region

#Region "Resequence RoutingSet"
    <Obsolete()> _
    Public Sub ResequenceRoute(ByVal pPointsArray As ArrayList)
        Dim oResequenceRouting As ResequenceRouting = New ResequenceRouting(pPointsArray, oLogger)
        oResequenceRouting.runResequenceRoutes()

    End Sub

#End Region

#Region "Accessors"

    Private Function BuildDistMatrixQuery(ByVal pRoutingSet As String) As String
        ''OUTBOUNDORHEADER->[ROUTINGREQUIREMENTS]
        Dim sql As String

        'sql = String.Format("select {0} routingset , mn.source, mn.destination, mn.distance,mn.drivingtime " & _
        '            " FROM MAPNETWORKDISTANCES mn " & _
        '            " WHERE SOURCE IN (SELECT POINTID FROM ROUTINGREQUIREMENTS where  routingset = {0}  " & _
        '            " union select pointid from VDEPOT) AND " & _
        '            " DESTINATION IN (SELECT POINTID FROM ROUTINGREQUIREMENTS where  routingset = {0}  " & _
        '            " union select pointid from VDEPOT)", _
        '        Made4Net.Shared.Util.FormatField(pRoutingSet))

        sql = String.Format("select {0} routingset , mn.source, mn.destination, mn.distance,mn.drivingtime " & _
            " FROM MAPNETWORKDISTANCES mn ", _
            Made4Net.Shared.Util.FormatField(pRoutingSet))


        Return sql
    End Function

    Protected Sub AssignToTerritories(Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write("Assigning to Territories...")
        End If


        For Each req As RoutingRequirement In _RoutingReqs
            If req.Strategy Is Nothing OrElse req.Strategy = String.Empty OrElse _
                    req.planned = 1 Then Continue For

            Dim oRoutingStrategy As RoutingStrategy = DirectCast(_RoutingStrat(req.Strategy), RoutingStrategy)
            If oRoutingStrategy Is Nothing _
                OrElse oRoutingStrategy.StrategyDetails.Count = 0 Then
                Continue For
            End If

            Dim TerritorySetID As String = oRoutingStrategy.StrategyDetails(0).TerritorySetID
            Dim territorykey As String = String.Empty

            Dim oGeoTerritoryCollection As GeoTerritoryCollection
            If _GeoTerritoryCollectionHash.ContainsKey(TerritorySetID) Then
                oGeoTerritoryCollection = _GeoTerritoryCollectionHash(TerritorySetID)
            Else
                oGeoTerritoryCollection = New GeoTerritoryCollection(TerritorySetID)
                _GeoTerritoryCollectionHash.Add(TerritorySetID, oGeoTerritoryCollection)
            End If

            If Not TerritorySetID Is Nothing AndAlso TerritorySetID <> String.Empty Then
                Try

                    Dim TerritoryID As String = String.Empty

                    If _TerritoryPointsHash.ContainsKey(req.TargetGeoPoint.POINTID) Then
                        req.TerritoryKey = _TerritoryPointsHash(territorykey)
                    Else

                        Dim oGeoTerritory As GeoTerritory
                        oGeoTerritory = oGeoTerritoryCollection.GetFromPoint(req.TargetGeoPoint.LATITUDE, req.TargetGeoPoint.LONGITUDE)
                        If Not oGeoTerritory Is Nothing Then
                            TerritoryID = oGeoTerritory.TERRITORYID
                        End If

                        'Dim oGeoTerritoryCollection As GeoTerritoryCollection = _
                        '   GeoTerritory.GetTerritoriesByPoint(CType(req.TargetGeoPoint, GeoPoint), TerritorySetID)
                        'If Not oGeoTerritoryCollection Is Nothing AndAlso oGeoTerritoryCollection.Count > 0 Then
                        '    TerritoryID = CType(oGeoTerritoryCollection(0), GeoTerritory).TERRITORYID
                        'End If


                        If Not TerritoryID Is Nothing AndAlso TerritoryID <> String.Empty Then
                            territorykey = TerritoryID ''TerritorySetID & "#" & TerritoryID

                            req.TerritoryKey = territorykey
                            If Not oLogger Is Nothing Then
                                oLogger.Write("Point: " & req.PointID & " assigned to territory: " & territorykey)
                            End If

                            If _territorycoll.IndexOf(territorykey) = -1 Then
                                _territorycoll.Add(territorykey)
                            End If

                            _TerritoryPointsHash.Add(req.TargetGeoPoint.POINTID, territorykey)
                        End If

                    End If
                Catch ex As Exception
                End Try
            End If
        Next

        If Not oLogger Is Nothing Then
            oLogger.Write("Total Territories Assigned: " & _territorycoll.Count.ToString())
        End If
    End Sub

    Protected Function AssignToStrategies(ByVal territorykey As String, _
        Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim req As RoutingRequirement
        Dim strat As RoutingStrategy


        If Not oLogger Is Nothing Then
            oLogger.Write("Assigning To Strategies for TeritoryID: " & _
                territorykey.ToString() & " ...")
        End If
        For Each req In _RoutingReqs
            If req.Strategy Is Nothing OrElse req.Strategy = String.Empty OrElse req.planned = 1 Then Continue For

            strat = _RoutingStrat(req.Strategy)
            If strat Is Nothing Then Continue For

            strat.Clear()
        Next


        For Each req In _RoutingReqs
            Dim reqCH As Integer = req.ContactObj.CloseHour(_RoutingSet.DistributionDate)
            Dim reqOH As Integer = req.ContactObj.OpeningHour(_RoutingSet.DistributionDate)
            If reqCH <= reqOH Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Could not assigned requirment contact close. Point - " & req.PointID & _
                     "  contact - " & req.ContactId & _
                     "  order - " & req.OrderId & _
                     " day - " & _RoutingSet.DistributionDate.DayOfWeek.ToString())
                End If
                Continue For
            End If


            If req.TerritoryKey <> territorykey OrElse req.planned = 1 Then Continue For

            If Not req.Strategy Is Nothing AndAlso req.Strategy <> String.Empty Then
                strat = _RoutingStrat(req.Strategy)
                If strat Is Nothing Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("There no matching strategies for requirment Order: " & req.OrderId & " for StrategyID: " & _
                            req.Strategy & " ...")
                    End If
                    Continue For
                End If

                If strat.StrategyDetails.Count = 0 Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("***Undefined details for strategy: " & req.Strategy & " Order: " & req.OrderId)
                    End If
                    Continue For
                End If

                Dim ret As Integer = strat.Add(req)
                If (ret < 0) Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("*** Point not attached to network - contact:" & req.ContactObj.CONTACTID.ToString() & _
                        " Order: " & req.OrderId & _
                        " Point:" & req.PointID)
                    End If
                End If
            End If
        Next


        If Not oLogger Is Nothing Then
            oLogger.Write("Assigned  to strategies for TeritoryID: " & _
                territorykey.ToString() & " Produced " & _RoutingReqs.Count.ToString() & " requirements.")
        End If

        Return True


    End Function

    Protected Shared Sub LoadVehiclesCache(Optional ByVal oLogger As LogHandler = Nothing)
        If _VehicleTypeCache Is Nothing Then _VehicleTypeCache = New Hashtable
        _VehicleTypeCache.Clear()
        Dim SQL As String = "SELECT distinct vehicletypename FROM VEHICLETYPE"
        If Not oLogger Is Nothing Then
            oLogger.Write("Start Loading Vehicle Types cache...")
        End If
        Dim dt As New DataTable
        Dim dr As DataRow
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            _VehicleTypeCache.Add(dr("vehicletypename"), VehicleType.GetVEHICLETYPE(dr("vehicletypename")))
        Next
        If Not oLogger Is Nothing Then
            oLogger.Write("Vehicle Types cache loaded...")
        End If
    End Sub

    Public Shared Function GetVehicle(ByVal pVehicleTypeId As String) As VehicleType
        If _VehicleTypeCache.ContainsKey(pVehicleTypeId) Then
            Return _VehicleTypeCache(pVehicleTypeId)
        Else
            Return Nothing
        End If
    End Function

#End Region

#End Region

End Class

Public Class RouteCostCompareClass
    Implements IComparer

    Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer _
       Implements IComparer.Compare
        Return New CaseInsensitiveComparer().Compare(CType(x, Route).Tag, CType(y, Route).Tag)
    End Function

End Class



