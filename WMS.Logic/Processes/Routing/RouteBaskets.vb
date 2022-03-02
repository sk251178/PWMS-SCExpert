Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Evaluation
Imports Made4Net.GeoData
Imports Made4Net.Algorithms.GeneticAlgorithm
Imports System.Text

#Region "RouteCounters"
Public Class RouteCounters
    Public RouteNum As Integer = 0
    Public NumStopsCheck As Integer = 0
    Public NumTasksCheck As Integer = 0
    Public TotalDistanceCheck As Double = 0
    Public TotalTimeCheck As Double = 0

    Public TotalWeightCheck As Double = 0
    Public TotalVolumeCheck As Double = 0
    Public TotalPickupWeightCheck As Double = 0
    Public TotalPickupVolumeCheck As Double = 0
    Public TotalDeliveryWeightCheck As Double = 0
    Public TotalDeliveryVolumeCheck As Double = 0

    Public TotalTranClassWeightCheckHash As New Hashtable()
    Public TotalTranClassVolumeCheckHash As New Hashtable()
    Public TotalPickupTranClassWeightCheckHash As New Hashtable()
    Public TotalPickupTranClassVolumeCheckHash As New Hashtable()
    Public TotalDeliveryTranClassWeightCheckHash As New Hashtable()
    Public TotalDeliveryTranClassVolumeCheckHash As New Hashtable()



    Sub New()
    End Sub
    Public Sub Init()
        NumStopsCheck = 0
        NumTasksCheck = 0
        TotalDistanceCheck = 0
        TotalTimeCheck = 0
        TotalWeightCheck = 0
        TotalVolumeCheck = 0
        TotalPickupWeightCheck = 0
        TotalPickupVolumeCheck = 0
        TotalDeliveryWeightCheck = 0
        TotalDeliveryWeightCheck = 0
    End Sub
End Class

#End Region

#Region "RouteBaskets"

<CLSCompliant(False)> Public Class RouteBaskets

#Region "Variables"

    Protected _stops As ArrayList
    Protected _reqs As ArrayList
    Protected _numstops As Int32 = 0
    Protected _numHash As New Hashtable()
    Protected _numtasks__ As Integer = 0

    Protected _totaldistance As Double = 0
    Protected _totaltime As Double = 0
    Protected _totalweight As Double = 0
    Protected _totalvolume As Double = 0

    Protected _totalservicetime As Double = 0


    Protected _totalpickupweight As Double = 0
    Protected _totalpickupvolume As Double = 0
    Protected _totaldeliveryweight As Double = 0
    Protected _totaldeliveryvolume As Double = 0

    Protected _maxdistbetwwenstops As Double = 0
    Protected _maxtimebetwwenstops As Double = 0
    Protected _numtripsperday As Int32
    Protected _routeDate As DateTime
    Protected _routeDepoStartDate As DateTime
    Protected _routestarttime As Int32 = -1
    Protected _routelateststarttime As Int32 = -1
    Protected _vehicletype As String
    Protected _vehicletypeObj As VehicleType
    Protected _isclosed As Boolean = False
    Protected _depopointid As String = String.Empty

    Protected _isDistanceAdd As Integer = 0
    Protected _distcosttype As Integer = 0
    Protected _isdistcost As Integer = 0
    Protected ServTimeCalcParameters As Hashtable = New Hashtable()

    Protected _isOpenHoursValidation As Integer = 0

    Public oRouteCounters As New RouteCounters


    Protected _StopContactData As Hashtable = New Hashtable()
    Protected _StopData As ArrayList = New ArrayList()

    Protected _CurrentVehicle As Integer
    Protected _CurrentVehicleKey As String

    Public TripGroup As String = RoutingStrategy.DefaultVehicleGroup
    Public TripNum As Integer = 0

#End Region

#Region "Properties"
    Public Property CurrentVehicleKey() As String
        Get
            Return _CurrentVehicleKey
        End Get
        Set(ByVal Value As String)
            _CurrentVehicleKey = Value
        End Set
    End Property
    Public Property CurrentVehicle() As Integer
        Get
            Return _CurrentVehicle
        End Get
        Set(ByVal Value As Integer)
            _CurrentVehicle = Value
        End Set
    End Property

    Public ReadOnly Property RouteStartTime() As Integer
        Get
            Return _routestarttime
        End Get
    End Property
    Public ReadOnly Property RouteDepoStartDate() As DateTime
        Get
            Return _routeDepoStartDate
        End Get
    End Property

    Public ReadOnly Property Routedate() As DateTime
        Get
            Return _routeDate
        End Get
    End Property

    Public ReadOnly Property ReqCollection() As ArrayList
        Get
            Return _reqs
        End Get
    End Property

    Public ReadOnly Property DepoPointid() As String
        Get
            Return _depopointid
        End Get
    End Property

    Public ReadOnly Property StopsCollection() As ArrayList
        Get
            Return _stops
        End Get
    End Property

    Public Property NumStops() As Int32
        Get
            Return _numstops
        End Get
        Set(ByVal Value As Int32)
            _numstops = Value
        End Set
    End Property
    Public Property TotalPickupVolume() As Double
        Get
            Return _totalpickupvolume
        End Get
        Set(ByVal Value As Double)
            _totalpickupvolume = Value
        End Set
    End Property
    Public Property TotalPickupWeight() As Double
        Get
            Return _totalpickupweight
        End Get
        Set(ByVal Value As Double)
            _totalpickupweight = Value
        End Set
    End Property

    Public Property TotalDeliveryVolume() As Double
        Get
            Return _totaldeliveryvolume
        End Get
        Set(ByVal Value As Double)
            _totalpickupvolume = Value
        End Set
    End Property
    Public Property TotalDeliveryWeight() As Double
        Get
            Return _totaldeliveryweight
        End Get
        Set(ByVal Value As Double)
            _totaldeliveryweight = Value
        End Set
    End Property

    Public Property NumTripsPerDay() As Int32
        Get
            Return _numtripsperday
        End Get
        Set(ByVal Value As Int32)
            _numtripsperday = Value
        End Set
    End Property

    Public Property TotalDistance() As Double
        Get
            Return _totaldistance
        End Get
        Set(ByVal Value As Double)
            _totaldistance = Value
        End Set
    End Property

    Public Property TotalTime() As Double
        Get
            Return _totaltime
        End Get
        Set(ByVal Value As Double)
            _totaltime = Value
        End Set
    End Property

    Public Property MaxDistBetweenStops() As Double
        Get
            Return _maxdistbetwwenstops
        End Get
        Set(ByVal Value As Double)
            _maxdistbetwwenstops = Value
        End Set
    End Property

    Public Property MaxTimeBetweenStops() As Double
        Get
            Return _maxtimebetwwenstops
        End Get
        Set(ByVal Value As Double)
            _maxtimebetwwenstops = Value
        End Set
    End Property

    Public Property VehicleType() As VehicleType
        Get
            Return _vehicletypeObj
        End Get
        Set(ByVal Value As VehicleType)
            _vehicletypeObj = Value
        End Set
    End Property

    Public Property TotalWeight() As Double
        Get
            Return _totalweight
        End Get
        Set(ByVal Value As Double)
            _totalweight = Value
        End Set
    End Property

    Public Property TotalVolume() As Double
        Get
            Return _totalvolume
        End Get
        Set(ByVal Value As Double)
            _totalvolume = Value
        End Set
    End Property

    Public Property Closed() As Boolean
        Get
            Return _isclosed
        End Get
        Set(ByVal Value As Boolean)
            _isclosed = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal pRouteDate As DateTime, ByVal pVehicleType As VehicleType)
        _stops = New ArrayList
        _reqs = New ArrayList
        _routeDate = pRouteDate
        _routeDepoStartDate = pRouteDate
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)

        _isOpenHoursValidation = Made4Net.Shared.AppConfig.Get("isOpenHoursValidation", "0")

        Try
            'try to load the vehicle type for the current basket
            If Not pVehicleType Is Nothing Then
                _vehicletype = pVehicleType.VEHICLETYPEID
                _vehicletypeObj = pVehicleType
            End If
        Catch ex As Exception
            _vehicletype = Nothing
        End Try

    End Sub

#End Region

#Region "Methods"

#Region "Placing Validation"


    Public Function CanPlace(ByRef oStrategy As RoutingStrategy, _
            ByVal lastPlacedReq As RoutingRequirement, _
            ByVal prevReq As RoutingRequirement, _
        ByVal Req As RoutingRequirement, _
        ByRef TempDistance As Double, _
        ByRef TempDrivingTime As Double, _
        ByVal isSamePoint As Boolean, _
        ByVal islaststop As Boolean, _
        ByRef isExcludeReq As Boolean, _
        ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        ''Return True

        Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)

        ''If Not isSamePoint AndAlso (_numstops + 1) > oStratDet.MaxStopsPerRoute Then
        If Not isSamePoint AndAlso (_numHash.Count + 1) > oStratDet.MaxStopsPerRoute Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Max Stops (" & (_numstops + 1).ToString _
                  & ">" & oStratDet.MaxStopsPerRoute & ") Per Route Exceeded.")
            End If
            Return False
        End If


        Dim maxServiceTime As Integer = 0
        If oStratDet.ServicetimeEquation = String.Empty Then
            If Not isSamePoint Then
                If Not Req.ContactObj Is Nothing Then
                    maxServiceTime = Req.ContactObj.FixedServiceTime
                Else
                    maxServiceTime = oStratDet.MinServiceTime
                End If
            End If
        Else

            If Not isSamePoint Then
                InitServTimeCalcParameters(oStratDet)
            End If
            SetReqServTimeCalcParameters(oStratDet, Req, isSamePoint, 0)
            maxServiceTime = CalcServTimeEquation(oStratDet.ServicetimeEquation, oLogger)
        End If

        Dim lasttoDepoDrvTime As Double = 0D
        Dim lasttoDepoDistance As Double = 0D
        '' calc ret to Depo
        If oStratDet.CalcRetTimeToDepot Then
            If Not oStratDet.Depot.POINTID Is Nothing AndAlso oStratDet.Depot.POINTID <> "" Then
                Dim DepoStop As GeoPointNode = New GeoPointNode(oStratDet.Depot.POINTID)
                ''                        Dim LastStop As GeoPointNode = New GeoPointNode(Req.TargetGeoPointID)
                If Not DepoStop Is Nothing Then
                    _depopointid = DepoStop.POINTID
                    GeoNetworkItem.GetDistance(Req.TargetGeoPoint, DepoStop, lasttoDepoDistance, lasttoDepoDrvTime, Common.GetCurrentUser, _isDistanceAdd, _
                        _isdistcost, _distcosttype)
                End If
            Else
                oLogger.Write("Error: Undefined return Depot PointID")
            End If

        End If


        'check if it is the first stop -> if its not check distance and time constraints 
        ''add to trunk
        If _numstops = 0 Then
            If _reqs.Count = 0 Then
                If oStratDet.CalcRetTimeToFirstStop Then
                    If Not oStratDet.Depot.POINTID Is Nothing AndAlso oStratDet.Depot.POINTID <> "" Then
                        Dim DepoStop As GeoPointNode = New GeoPointNode(oStratDet.Depot.POINTID)
                        If Not DepoStop Is Nothing Then
                            _depopointid = DepoStop.POINTID
                            GeoNetworkItem.GetDistance(DepoStop, Req.TargetGeoPoint, TempDistance, TempDrivingTime, Common.GetCurrentUser, _isDistanceAdd, _
                                _isdistcost, _distcosttype)

                        Else
                            _depopointid = String.Empty
                        End If
                    Else
                        oLogger.Write("Error: Undefined Depot PointID")
                    End If
                End If


                ''If Not ValidateDepotOpenHours(Req, TempDrivingTime, Req.ContactObj, oStratDet, oLogger) Then Return False
                Dim addArrivalDays As Integer = 0
                If checkDepotStopTimeType(oStrategy, Req, Req.ContactObj, TempDrivingTime, addArrivalDays, oLogger) = 0 Then Return False

                Dim EstDepTime, EstArrivalTime, addDepDays As Integer
                calcDepotStopTime(oStrategy, Req, Req.ContactObj, TempDrivingTime, _
                        Req.EstArrivalHour, Req.EstDepartureHour, Req.addDepDays, addArrivalDays, oLogger)

            End If

            Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
            Dim TripNumber As Integer = oVehicleDataArray(2)
            Dim TotalTripsTime As Integer = oVehicleDataArray(5)
            If TripNumber > 0 Then
                If Not ValidateTimeConstraints(oStrategy, Req, TempDrivingTime, maxServiceTime, lasttoDepoDrvTime, oLogger) Then Return False
                If Not ValidateDistanceConstraints(Req, oStrategy, TempDistance, lasttoDepoDistance, oLogger) Then Return False
            End If

        ElseIf _numstops > 0 Then


            If Not checkSameStop(prevReq, Req) Then
                GeoNetworkItem.GetDistance(prevReq.TargetGeoPoint, Req.TargetGeoPoint, TempDistance, TempDrivingTime, Common.GetCurrentUser, _isDistanceAdd, _
                                _isdistcost, _distcosttype)
                'Validate the distance params
                If Not ValidateDistanceConstraints(Req, oStrategy, TempDistance, lasttoDepoDistance, oLogger) Then Return False
            End If


            ''If Not isSamePoint AndAlso (TempDrivingTime <> 0) Then
            'Validate the time params
            ''End If

        End If

        ''check target capacity for exclude for exclude
        Dim isTargetFillCapacity As Boolean = _
            (oRouteCounters.TotalWeightCheck >= oStratDet.TargetFillWeight * _vehicletypeObj.TOTALWEIGHT) Or _
            (oRouteCounters.TotalVolumeCheck >= oStratDet.TargetFillVolume * _vehicletypeObj.TOTALVOLUME)

        'If _isOpenHoursValidation = 1 And oStratDet.CalcRetTimeToDepot Then
        '    If Not ValidateReturnDepotOpenHours(oStratDet, Req, TempDrivingTime, maxServiceTime, lasttoDepoDrvTime, oLogger) Then Return False

        'End If


        If _numstops > 0 And Not Req.ContactObj Is Nothing Then
            If Not prevReq Is Nothing Then
                If checkStoptoStopStartTimeType(oStratDet, Req, Req.ContactObj, _
                lastPlacedReq.EstDepartureHour, lastPlacedReq.addDepDays, TempDrivingTime, oLogger) = 0 Then
                    If Not isTargetFillCapacity Then isExcludeReq = True
                    Return False
                End If
                calcStoptoStopArrivalDepartureTime(oStratDet, Req, Req.ContactObj, _
                    lastPlacedReq.EstDepartureHour, Req.EstArrivalHour, Req.EstDepartureHour, _
                    lastPlacedReq.addDepDays, Req.addDepDays, _
                    TempDrivingTime, oLogger)

                If Not checkSameStop(prevReq, Req) Then
                    If Not ValidateTimeConstraints(oStrategy, Req, TempDrivingTime, maxServiceTime, lasttoDepoDrvTime, oLogger) Then Return False
                End If
            End If

        End If

        If _vehicletypeObj Is Nothing Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Undefined vehicle.")
            End If
            Return False
        End If

        If Not Validate2WeightConstraints(Req, oStratDet, islaststop, oLogger) Then
            If Not isTargetFillCapacity Then isExcludeReq = True
            Return False
        End If

        Dim oTransportationClass As TransportationClass = _
            _vehicletypeObj.VehicleTypeTransportationClass.TransportationClassCollection(Req.TransportationClass)

        If Req.TransportationClass <> String.Empty AndAlso Not oTransportationClass Is Nothing Then
            If Not ValidateWeightTransClassConstraints(Req, oStratDet, oTransportationClass, oLogger) Then
                If Not isTargetFillCapacity Then isExcludeReq = True
                Return False
            End If

            If Not ValidateVolumeTransClassConstraints(Req, oStratDet, oTransportationClass, oLogger) Then
                If Not isTargetFillCapacity Then isExcludeReq = True
                Return False
            End If
        End If

        If Not Validate2VolumeConstraints(Req, oStratDet, islaststop, oLogger) Then
            If Not isTargetFillCapacity Then isExcludeReq = True
            Return False
        End If


        If Not ValidateVolumePickupafterDeliveryConstraints(Req, oStratDet, islaststop, oLogger) Then
            If Not isTargetFillCapacity Then isExcludeReq = True
            Return False
        End If

        If Not ValidateWeightPickupafterDeliveryConstraints(Req, oStratDet, islaststop, oLogger) Then
            If Not isTargetFillCapacity Then isExcludeReq = True
            Return False
        End If


        '' counters
        If Req.PDType = PDType.Pickup Then
            oRouteCounters.TotalPickupWeightCheck += Req.OrderWeight
            oRouteCounters.TotalPickupVolumeCheck += Req.OrderVolume
        ElseIf Req.PDType = PDType.Delivery Then
            oRouteCounters.TotalDeliveryWeightCheck += Req.OrderWeight
            oRouteCounters.TotalDeliveryVolumeCheck += Req.OrderVolume
        End If
        oRouteCounters.TotalWeightCheck += Req.OrderWeight
        oRouteCounters.TotalVolumeCheck += Req.OrderVolume

        Return True
    End Function

    private Function checkSameStop(ByVal prevReq As RoutingRequirement, _
                                      ByVal currentReq As RoutingRequirement) As Boolean
        If prevReq Is Nothing OrElse currentReq Is Nothing Then Return False
        If currentReq.PointID = prevReq.PointID Then Return True
        Return False
    End Function


    Private Sub addServTimeCalcParameters(ByVal key As String, ByVal value As String)
        If Not ServTimeCalcParameters.Contains(key) Then
            ServTimeCalcParameters.Add(key, value)
        Else
            ServTimeCalcParameters(key) = value
        End If
    End Sub


    Public Sub InitServTimeCalcParameters(ByVal oStratDet As RoutingStrategyDetail)
        _StopContactData.Clear()
        _StopData.Clear()

        addServTimeCalcParameters("ContactTasks", "1")
        addServTimeCalcParameters("ContactFixedServiceTime", "0")
        addServTimeCalcParameters("ContactSumFixedServiceTime", "0")
        addServTimeCalcParameters("ContactTotalVolume", "0")
        addServTimeCalcParameters("ContactTotalWeight", "0")

        addServTimeCalcParameters("StrategyServiceTime", oStratDet.MinServiceTime)


        addServTimeCalcParameters("TotalTasks", "1")
        addServTimeCalcParameters("StopTotalVolume", "0")
        addServTimeCalcParameters("StopTotalWeight", "0")
        ''max fixed serv time
        ''avg fixed servicetime
    End Sub

    Private Sub SetReqServTimeCalcParameters(ByVal stratDet As RoutingStrategyDetail, _
                                ByVal Req As RoutingRequirement, ByVal isSamePoint As Boolean, _
    ByVal res As Integer)

        ''ContactData
        Dim ContactData As New ArrayList
        If _StopContactData.Contains(Req.PointID & "#" & Req.ContactId) Then
            ContactData = _StopContactData(Req.PointID & "#" & Req.ContactId)
            ContactData(0) = ContactData(0) + 1
            ContactData(1) = Req.ContactObj.FixedServiceTime.ToString
            If Not Req.ContactObj Is Nothing Then
                ContactData(2) = ContactData(2) + Req.ContactObj.FixedServiceTime
            End If
            ContactData(3) = ContactData(3) + Req.OrderVolume
            ContactData(4) = ContactData(4) + Req.OrderWeight
            _StopContactData(Req.PointID & "#" & Req.ContactId) = ContactData
        Else
            ContactData.Add(1) ''1?
            If Not Req.ContactObj Is Nothing Then
                ContactData.Add(Req.ContactObj.FixedServiceTime.ToString)
                ContactData.Add(Req.ContactObj.FixedServiceTime.ToString)
            Else
                ContactData.Add(0)
                ContactData.Add(0)
            End If
            ContactData.Add(Req.OrderVolume.ToString())
            ContactData.Add(Req.OrderWeight.ToString())
            _StopContactData.Add(Req.PointID & "#" & Req.ContactId, ContactData)
        End If

        addServTimeCalcParameters("ContactTasks", ContactData(0).ToString)
        addServTimeCalcParameters("ContactFixedServiceTime", ContactData(1).ToString)
        addServTimeCalcParameters("ContactSumFixedServiceTime", ContactData(2).ToString())
        addServTimeCalcParameters("ContactTotalVolume", ContactData(3).ToString())
        addServTimeCalcParameters("ContactTotalWeight", ContactData(4).ToString())


        ''StopData
        If Not isSamePoint And res = 0 Then
            _StopData.Clear()
        End If

        If _StopData.Count > 0 Then
            addServTimeCalcParameters("TotalTasks", (_StopData(0) + 1).ToString())
            addServTimeCalcParameters("StopTotalVolume", (_StopData(1) + Req.OrderVolume).ToString())
            addServTimeCalcParameters("StopTotalWeight", (_StopData(2) + Req.OrderWeight).ToString())
            ''max fixed serv time
            ''avg fixed servicetime
            _StopData(0) = _StopData(0) + 1
            _StopData(1) = _StopData(1) + Req.OrderVolume
            _StopData(2) = _StopData(2) + Req.OrderWeight
        Else
            addServTimeCalcParameters("TotalTasks", "1")
            addServTimeCalcParameters("StopTotalVolume", Req.OrderVolume.ToString())
            addServTimeCalcParameters("StopTotalWeight", Req.OrderWeight.ToString())
            _StopData.Add(1)
            _StopData.Add(Req.OrderVolume)
            _StopData.Add(Req.OrderWeight)
            ''max fixed serv time
            ''avg fixed servicetime

        End If

        ''general and task data
        addServTimeCalcParameters("StrategyServiceTime", stratDet.MinServiceTime.ToString())
        addServTimeCalcParameters("TaskVolume", Req.OrderVolume.ToString())
        addServTimeCalcParameters("TaskWeight", Req.OrderWeight.ToString())
        addServTimeCalcParameters("TaskType", Req.OrderType.ToString())
        addServTimeCalcParameters("TaskValue", Req.OrderValue.ToString())

        addServTimeCalcParameters("VehicleType", Req.VehicleType)
        addServTimeCalcParameters("UnloadingType", Req.UnloadingType)
        addServTimeCalcParameters("Carrier", Req.Carrier)

    End Sub


    Private Function CalcServTimeEquation(ByVal sourceEquation As String, _
                                Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Double
        Dim targetEquation As String
        Dim res As Double
        Try
            Dim UseEvalLogs As String = Made4Net.Shared.AppConfig.Get("UseEvalLogs", String.Empty)
            Dim dirpath As String
            If UseEvalLogs = "1" Then
                dirpath = Made4Net.Shared.AppConfig.Get("ServiceEvalLogDirectory", Nothing)
            End If

            Dim oEvalEquation As EvalEquation = New EvalEquation(Nothing, ServTimeCalcParameters, dirpath)
            res = oEvalEquation.EvalEquation(sourceEquation, targetEquation)
            Return res
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("CalcServEquation Error:" & ex.ToString())
            End If
            Return 0D

        End Try
    End Function


    Private Function SameTargetPoint(ByVal Req As RoutingRequirement) As Boolean
        Dim i As Int32
        Dim curStop As String
        If _numstops > 0 Then
            For i = 0 To _stops.Count - 1
                curStop = CType(_reqs(i), RoutingRequirement).TargetGeoPoint.POINTID
                If curStop = Req.TargetGeoPoint.POINTID Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function ValidateDistanceConstraints(ByVal Req As RoutingRequirement, _
        ByRef oStrategy As RoutingStrategy, _
        ByRef TempDistance As Double, _
        ByVal lasttoDepoDistance As Double, _
        ByVal oLogger As WMS.Logic.LogHandler) As Boolean

        Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)



        If TempDistance = -1 Or TempDistance = Double.MaxValue Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Distance between points could not be calculated.")
            End If
            Return False
        End If
        Dim tdist As Double = 0

        Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
        Dim TripNumber As Integer = oVehicleDataArray(2)
        Dim TotalTripsDistance As Integer = oVehicleDataArray(4)
        If TripNumber > 0 Then
            tdist = TempDistance + _totaldistance + lasttoDepoDistance + TotalTripsDistance
            If tdist > oStratDet.TotalTripsAllocationDistance Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Could not Place Requirement " & _
                                    "  point - " & Req.PointID & _
                                    "  contact - " & Req.ContactId & _
                                    "  order - " & Req.OrderId & _
                                    "  #Total Distance Per Trip Route Exceeded (" & tdist.ToString & ">" & oStratDet.TotalTripsAllocationDistance.ToString & ".)")

                End If
                Return False
            End If
        End If


        tdist = TempDistance + _totaldistance + lasttoDepoDistance
        If tdist > oStratDet.MaxDistancePerRoute Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Total Distance Per Route Exceeded (" & tdist.ToString & ">" & oStratDet.MaxDistancePerRoute.ToString & ".)")

            End If
            Return False
        End If
        If TempDistance > oStratDet.MaxDistanceBetweenStops Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Max Distance Between Stops Exceeded(" & TempDistance.ToString & ">" & _
                                oStratDet.MaxDistanceBetweenStops & ".)")
            End If
            Return False
        End If
        Return True
    End Function

    Private Function ValidateTimeConstraints(ByRef oStrategy As RoutingStrategy, _
    ByVal Req As RoutingRequirement, _
    ByRef TempDrivingTime As Double, _
    ByVal maxServiceTime As Double, _
    ByVal lasttoDepoDrvTime As Double, _
    ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        ''temporary not avlaiable
        ''Return True
        Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)

        If TempDrivingTime = -1 Or TempDrivingTime = Double.MaxValue Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Driving time between points could not be calculated.")
            End If
            Return False

        End If

        Dim ttime As Double = 0

        Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
        Dim TripNumber As Integer = oVehicleDataArray(2)
        Dim TotalTripsTime As Integer = oVehicleDataArray(5)
        ttime = TotalTripsTime + TempDrivingTime + _totaltime + lasttoDepoDrvTime + maxServiceTime
        If TripNumber > 0 Then
            If ttime + maxServiceTime > oStratDet.TotalTripsAllocationTime Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Could not Place Requirement " & _
                                    "  point - " & Req.PointID & _
                                    "  contact - " & Req.ContactId & _
                                    "  order - " & Req.OrderId & _
                                    "  #Total Time Per Trip Route Exceeded (" & ttime.ToString & ">" & oStratDet.TotalTripsAllocationTime.ToString & ".)")

                End If
                Return False
            End If
        End If


        ttime = TempDrivingTime + _totaltime + lasttoDepoDrvTime + maxServiceTime
        If ttime > oStratDet.MaxTimePerRoute Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Total Driving time Per Route Exceeded. ( " & ttime.ToString & " > " & oStratDet.MaxTimePerRoute & ".)")
            End If
            Return False
        End If
        If TempDrivingTime > oStratDet.MaxTimeBetweenStops Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Total Driving time Between Stops Exceeded.( " & _
                                TempDrivingTime & " > " & oStratDet.MaxTimeBetweenStops & ".)")
            End If
            Return False
        End If

        Return True
    End Function


    Public Shared Function CalcEstimateArrivalTime(ByVal tempDrivingTime As Double, ByVal pStartTime As Int32, _
                            ByRef AddDays As Integer, _
                                 ByVal oLogger As WMS.Logic.LogHandler) As Int32
        Try
            Dim itotalTime As Double = Math.Round(Convert.ToDouble(tempDrivingTime)) ''+ _totaltime
            itotalTime = Math.Round(itotalTime, 3)
            Dim sec, res As Integer
            sec = itotalTime + Math.Floor(pStartTime / 100) * 3600 + Math.Floor(pStartTime Mod 100) * 60
            res = Math.Floor(sec / 3600) * 100 + Math.Floor((sec Mod 3600) / 60)
            AddDays = Route.get24AddDays(res)
            res = Route.get24Hour(res)

            Return res

        Catch ex As Exception
            oLogger.Write("CalcEstimateArrivalTime Error: " & ex.ToString())
            Return -1
        End Try

    End Function

    <Obsolete()> _
    Private Function ValidateVehicleWeightConstraints(ByVal Req As RoutingRequirement, _
                                 ByVal oStratDet As RoutingStrategyDetail, _
                                 ByVal islaststop As Boolean, _
                                 ByVal oLogger As WMS.Logic.LogHandler) As Boolean

        Dim weightLimit As Double = _vehicletypeObj.TOTALWEIGHT
        If oStratDet.AllowedOverWeightforLastStop <> 0 AndAlso islaststop Then weightLimit = _
            _vehicletypeObj.TOTALWEIGHT * oStratDet.AllowedOverWeightforLastStop

        If _totalweight + Req.OrderWeight > weightLimit Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & Req.PointID & _
                                "  contact - " & Req.ContactId & _
                                "  order - " & Req.OrderId & _
                                "  #Vehicle volume exceeded." & " total current volume: " & (_totalweight + Req.OrderWeight).ToString() & " Order volume: " & Req.OrderVolume.ToString() & " Vehicle volume: " & _vehicletypeObj.TOTALVOLUME.ToString())
            End If
            Return False
        End If
        Return True
    End Function

    <Obsolete()> _
    Private Function ValidateVehicleVolumeConstraints(ByVal tmpReq As RoutingRequirement, _
         ByVal oStratDet As RoutingStrategyDetail, _
                                 ByVal islaststop As Boolean, _
                                 ByVal oLogger As WMS.Logic.LogHandler) As Boolean

        Dim volumeLimit As Double = _vehicletypeObj.TOTALVOLUME
        If oStratDet.AllowedOverVolumeforLastStop <> 0 AndAlso islaststop Then volumeLimit = _
            _vehicletypeObj.TOTALVOLUME * oStratDet.AllowedOverVolumeforLastStop
        If _totalvolume + tmpReq.OrderVolume > volumeLimit Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  #Vehicle volume exceeded. Total current volume: " & (_totalvolume + tmpReq.OrderVolume).ToString() & " Order volume: " & tmpReq.OrderVolume.ToString() & " Vehicle volume: " & _vehicletypeObj.TOTALVOLUME.ToString())
            End If
            Return False
        End If
        Return True
    End Function


    Public Function ValidateWeightTransClassConstraints(ByVal tmpReq As RoutingRequirement, _
                                        ByVal oStratDet As RoutingStrategyDetail, _
                                        ByVal oTransportationClass As TransportationClass, _
                                        ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        If tmpReq.PDType = PDType.General Then Return True
        Dim weightLimit As Double = oTransportationClass.MaxTotalWeight

        '' check tmpReq.PDType
        Dim isfeasible As Boolean = True
        Dim currentWeight As Double = 0

        ''init counters
        If Not oRouteCounters.TotalTranClassWeightCheckHash.Contains(oTransportationClass.TransportationClass) Then
            oRouteCounters.TotalTranClassWeightCheckHash.Add(oTransportationClass.TransportationClass, 0)
        End If
        If Not oRouteCounters.TotalPickupTranClassWeightCheckHash.Contains(oTransportationClass.TransportationClass) Then
            oRouteCounters.TotalPickupTranClassWeightCheckHash.Add(oTransportationClass.TransportationClass, 0)
        End If
        If Not oRouteCounters.TotalDeliveryTranClassWeightCheckHash.Contains(oTransportationClass.TransportationClass) Then
            oRouteCounters.TotalDeliveryTranClassWeightCheckHash.Add(oTransportationClass.TransportationClass, 0)
        End If


        currentWeight = oRouteCounters.TotalPickupTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) _
            + tmpReq.OrderWeight _
            - oRouteCounters.TotalDeliveryTranClassWeightCheckHash.Contains(oTransportationClass.TransportationClass)
        If currentWeight > weightLimit Then isfeasible = False


        currentWeight = oRouteCounters.TotalTranClassWeightCheckHash(oTransportationClass.TransportationClass) _
            + tmpReq.OrderWeight

        If tmpReq.PDType = PDType.Delivery AndAlso currentWeight > weightLimit Then isfeasible = False

        If Not isfeasible Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  #For transportation class: " & oTransportationClass.TransportationClass & " - vehicle weight exceeded. Transportation class: " & oTransportationClass.TransportationClass & "  Total current weight: " & (oRouteCounters.TotalTranClassWeightCheckHash(oTransportationClass.TransportationClass) + tmpReq.OrderWeight).ToString() & " Order weight: " & tmpReq.OrderWeight.ToString() & " Vehicle weight: " & oTransportationClass.MaxTotalWeight.ToString() & " - " & currentWeight.ToString & ">" & oTransportationClass.MaxTotalWeight.ToString)
            End If
            Return False
        End If

        Return True
    End Function

    Public Function ValidateVolumeTransClassConstraints(ByVal tmpReq As RoutingRequirement, _
                                    ByVal oStratDet As RoutingStrategyDetail, _
                                    ByVal oTransportationClass As TransportationClass, _
                                    ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        If tmpReq.PDType = PDType.General Then Return True
        Dim VolumeLimit As Double = oTransportationClass.MaxTotalVolume

        '' check tmpReq.PDType
        Dim isfeasible As Boolean = True
        Dim currentVolume As Double = 0

        ''init counters
        If Not oRouteCounters.TotalTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) Then
            oRouteCounters.TotalTranClassVolumeCheckHash.Add(oTransportationClass.TransportationClass, 0)
        End If
        If Not oRouteCounters.TotalPickupTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) Then
            oRouteCounters.TotalPickupTranClassVolumeCheckHash.Add(oTransportationClass.TransportationClass, 0)
        End If
        If Not oRouteCounters.TotalDeliveryTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) Then
            oRouteCounters.TotalDeliveryTranClassVolumeCheckHash.Add(oTransportationClass.TransportationClass, 0)
        End If


        currentVolume = oRouteCounters.TotalPickupTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) _
            + tmpReq.OrderVolume _
            - oRouteCounters.TotalDeliveryTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass)
        If currentVolume > VolumeLimit Then isfeasible = False


        currentVolume = oRouteCounters.TotalTranClassVolumeCheckHash(oTransportationClass.TransportationClass) _
            + tmpReq.OrderVolume

        If tmpReq.PDType = PDType.Delivery AndAlso currentVolume > VolumeLimit Then isfeasible = False

        If Not isfeasible Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  #For transportation class: " & oTransportationClass.TransportationClass & " - vehicle Volume exceeded. Transportation class: " & oTransportationClass.TransportationClass & "  Total current Volume: " & (oRouteCounters.TotalTranClassVolumeCheckHash(oTransportationClass.TransportationClass) + tmpReq.OrderVolume).ToString() & " Order Volume: " & tmpReq.OrderVolume.ToString() & " Vehicle Volume: " & oTransportationClass.MaxTotalVolume.ToString() & " - " & currentVolume.ToString & ">" & oTransportationClass.MaxTotalVolume.ToString)
            End If
            Return False
        End If

        Return True
    End Function


    Public Function Validate2WeightConstraints(ByVal tmpReq As RoutingRequirement, _
                                    ByVal oStratDet As RoutingStrategyDetail, _
                                    ByVal islaststop As Boolean, _
                                    ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        If tmpReq.PDType = PDType.General Then Return True

        Dim weightLimit As Double = _vehicletypeObj.TOTALWEIGHT
        If oStratDet.AllowedOverWeightforLastStop <> 0 AndAlso islaststop Then weightLimit = _
            _vehicletypeObj.TOTALWEIGHT * oStratDet.AllowedOverWeightforLastStop

        '' check tmpReq.PDType
        Dim isfeasible As Boolean = True
        Dim currentWeight As Double = 0

        currentWeight = oRouteCounters.TotalPickupWeightCheck + tmpReq.OrderWeight - oRouteCounters.TotalDeliveryWeightCheck
        If currentWeight > weightLimit Then isfeasible = False


        currentWeight = oRouteCounters.TotalWeightCheck + tmpReq.OrderWeight
        If tmpReq.PDType = PDType.Delivery AndAlso currentWeight > weightLimit Then isfeasible = False

        If Not isfeasible Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  #Vehicle weight exceeded. Total current weight: " & (oRouteCounters.TotalWeightCheck + tmpReq.OrderWeight).ToString() & " Order weight: " & tmpReq.OrderWeight.ToString() & " Vehicle weight: " & _vehicletypeObj.TOTALWEIGHT.ToString() & " - " & currentWeight.ToString & ">" & weightLimit.ToString)
            End If
            Return False
        End If

        Return True
    End Function

    Public Function ValidateVolumePickupafterDeliveryConstraints(ByVal tmpReq As RoutingRequirement, _
         ByVal oStratDet As RoutingStrategyDetail, _
        ByVal islaststop As Boolean, _
         ByVal oLogger As WMS.Logic.LogHandler) As Boolean

        If islaststop OrElse tmpReq.PDType <> PDType.Pickup Then Return True
        Dim volumeLimit As Double = _vehicletypeObj.TOTALVOLUME * oStratDet.AllowPickupBeforeDelivery
        If oRouteCounters.TotalDeliveryVolumeCheck < volumeLimit Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement. Not enough delivery volume before pickup." & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  order - " & tmpReq.OrderId & _
                                "  total delivery volume - " & oRouteCounters.TotalDeliveryVolumeCheck & _
                                "  required volume - " & volumeLimit)
            End If
            Return False
        End If
        Return True
    End Function

    Public Function ValidateWeightPickupafterDeliveryConstraints(ByVal tmpReq As RoutingRequirement, _
         ByVal oStratDet As RoutingStrategyDetail, _
        ByVal islaststop As Boolean, _
         ByVal oLogger As WMS.Logic.LogHandler) As Boolean

        If islaststop OrElse tmpReq.PDType <> PDType.Pickup Then Return True
        Dim weightLimit As Double = _vehicletypeObj.TOTALWEIGHT * oStratDet.AllowPickupBeforeDelivery
        If oRouteCounters.TotalDeliveryWeightCheck < weightLimit Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement. Not enough delivery weight before pickup." & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  order - " & tmpReq.OrderId & _
                                "  total delivery volume - " & oRouteCounters.TotalDeliveryWeightCheck & _
                                "  required volume - " & weightLimit)
            End If
            Return False
        End If
        Return True
    End Function


    Public Function Validate2VolumeConstraints(ByVal tmpReq As RoutingRequirement, _
         ByVal oStratDet As RoutingStrategyDetail, _
                                 ByVal islaststop As Boolean, _
                                 ByVal oLogger As WMS.Logic.LogHandler) As Boolean

        If tmpReq.PDType = PDType.General Then Return True

        Dim volumeLimit As Double = _vehicletypeObj.TOTALVOLUME
        If oStratDet.AllowedOverVolumeforLastStop <> 0 AndAlso islaststop Then volumeLimit = _
            _vehicletypeObj.TOTALVOLUME * oStratDet.AllowedOverVolumeforLastStop

        Dim isfeasible As Boolean = True
        If oRouteCounters.TotalPickupVolumeCheck + tmpReq.OrderVolume - _
                oRouteCounters.TotalDeliveryVolumeCheck > volumeLimit Then isfeasible = False
        If tmpReq.PDType = PDType.Delivery AndAlso oRouteCounters.TotalVolumeCheck + tmpReq.OrderVolume > volumeLimit Then isfeasible = False

        If Not isfeasible Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement " & _
                                "  point - " & tmpReq.PointID & _
                                "  contact - " & tmpReq.ContactId & _
                                "  order - " & tmpReq.OrderId & _
                                "  #Vehicle volume exceeded. Total current volume: " & (oRouteCounters.TotalVolumeCheck + tmpReq.OrderVolume).ToString() & " Order volume: " & tmpReq.OrderVolume.ToString() & " Vehicle volume: " & _vehicletypeObj.TOTALVOLUME.ToString())
            End If
            Return False
        End If
        Return True
    End Function


#End Region

#Region "Placing In Baskets Methods"

    Public Sub Place(ByVal isSamePoint As Boolean, _
                     ByRef oStrategy As RoutingStrategy, _
                     ByVal iTargetStop As Int32, _
                     ByVal Req As RoutingRequirement, _
                     ByVal TempDistance As Double, _
                     ByVal TempDrivingTime As Double, _
                                  ByVal oLogger As WMS.Logic.LogHandler)

        Try
            Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)

            Dim tmpsb As New StringBuilder
            ''Dim EstArrivalHour, EstDepartureHour As Int32

            Dim maxServiceTime As Integer = 0
            If oStratDet.ServicetimeEquation = String.Empty Then
                If Not isSamePoint Then
                    If Not Req.ContactObj Is Nothing Then
                        maxServiceTime = Req.ContactObj.FixedServiceTime
                    Else
                        maxServiceTime = oStratDet.MinServiceTime
                    End If
                End If
            Else
                SetReqServTimeCalcParameters(oStratDet, Req, isSamePoint, 1)
                maxServiceTime = CalcServTimeEquation(oStratDet.ServicetimeEquation, oLogger)
            End If
            _totalservicetime += maxServiceTime

            Dim AddDays, AddDepdDays As Integer
            If isSamePoint Then
                Dim prevStop As String() = CType(_stops.Item(_stops.Count - 1), String).Split("#")
                ''EstArrivalHour = CalcEstimateArrivalTime(TempDrivingTime, _routestarttime, AddDays, oLogger)
                ''EstDepartureHour = CalcEstimateDepartureTime(EstArrivalHour, maxServiceTime, AddDepdDays)
                _totaltime += maxServiceTime

                Req.EstDepartureHour = CalcEstimateArrivalTime(maxServiceTime, Req.EstDepartureHour, AddDays, oLogger)
                Req.addDepDays += AddDays

                ''adddays

                Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
                _stops.Add(iTargetStop.ToString() & "#" & _totaldistance & "#" & _totaltime & _
                    "#" & Req.EstArrivalHour & "#" & Req.EstDepartureHour & "#" & Req.addDepDays)

            Else
                If _numstops = 0 Then
                    _totalweight = 0
                    _totalvolume = 0
                End If


                _totaldistance += TempDistance
                _totaltime += TempDrivingTime + maxServiceTime
                ''_totaldistance+??to_depotdistance

                Req.EstDepartureHour = CalcEstimateArrivalTime(maxServiceTime, Req.EstDepartureHour, AddDays, oLogger)
                Req.addDepDays += AddDays

                _stops.Add(iTargetStop.ToString() & "#" & _totaldistance & "#" & _totaltime & "#" & _
                    Req.EstArrivalHour & "#" & Req.EstDepartureHour & "#" & Req.addDepDays)
                _numstops += 1
                If Not _numHash.ContainsKey(Req.PointID) Then
                    _numHash.Add(Req.PointID, Req.PointID)
                End If
                addServTimeCalcParameters("NumStops", _numstops.ToString())

            End If

            'If Not oLogger Is Nothing And tmpsb.Length > 0 Then
            '    tmpsb.AppendLine()
            '    oLogger.Write(tmpsb.ToString())
            'End If

            _reqs.Add(Req)
            _totalvolume += Req.OrderVolume
            _totalweight += Req.OrderWeight

            If Req.PDType = PDType.Pickup Then
                _totalpickupweight += Req.OrderWeight
                _totalpickupvolume += Req.OrderVolume
            ElseIf Req.PDType = PDType.Delivery Then
                _totaldeliveryweight += Req.OrderWeight
                _totaldeliveryvolume += Req.OrderVolume
            End If


        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Place Error: " & ex.ToString())
            End If

        End Try
    End Sub

    Public Shared Function CalcEstimateDepartureTime(ByVal estArrivalTime As Integer, ByVal servicetime As Double, _
        ByRef AddDays As Integer) As Int32

        Dim itotalTime As Double = Math.Round(servicetime, 0)
        Dim sec, res As Integer
        sec = itotalTime + Math.Floor(estArrivalTime / 100) * 3600 + Math.Floor(estArrivalTime Mod 100) * 60
        res = Math.Floor(sec / 3600) * 100 + Math.Floor((sec Mod 3600) / 60)
        AddDays = Route.get24AddDays(res)
        res = Route.get24Hour(res)

        Return res


    End Function
    Public Shared Function DifTimeSec(ByVal pStartTime As Double, ByVal pEndTime As Integer) As Integer
        Dim min, res As Integer
        res = Math.Floor(pEndTime / 100) * 3600 + Math.Floor(pEndTime Mod 100) * 60 - _
            (Math.Floor(pStartTime / 100) * 3600 + Math.Floor(pStartTime Mod 100) * 60)
        Return res
    End Function

    Public Shared Function DifTimeSecwithDays(ByVal pStartTime As Double, ByVal pEndTime As Integer) As Integer


        If pStartTime = pEndTime Then Return 0
        Dim min, res As Integer
        If pEndTime > pStartTime Then
            res = Math.Floor(pEndTime / 100) * 3600 + Math.Floor(pEndTime Mod 100) * 60 - _
                (Math.Floor(pStartTime / 100) * 3600 + Math.Floor(pStartTime Mod 100) * 60)
        Else
            res = 86400 - ((Math.Floor(pStartTime / 100) * 3600 + Math.Floor(pStartTime Mod 100) * 60) - _
            Math.Floor(pEndTime / 100) * 3600 + Math.Floor(pEndTime Mod 100) * 60)
        End If

        Return res
    End Function


    ''DEPOT->STOP1
    Public Function checkDepotStopTimeType(ByVal oStrategy As RoutingStrategy, _
        ByVal req As RoutingRequirement, _
        ByVal oContact As Contact, _
        ByVal TempDrivingTime As Integer, _
        ByRef addDays As Integer, _
        ByVal oLogger As WMS.Logic.LogHandler) As Integer

        Dim StartimeOh, StartimeCh, StartimeWt As Integer
        Dim res As Integer
        Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)


        Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
        Dim TripNumber As Integer = oVehicleDataArray(2)
        Dim EndDateTime As DateTime = oVehicleDataArray(3)

        If TripNumber = 0 Then
            Dim ContactOH = oContact.OpeningHour(_routeDate) ''*-1??
            If ContactOH = 0 Then Return True

            StartimeOh = CalcEstimateArrivalTime(-1 * TempDrivingTime, ContactOH, addDays, oLogger)
            Dim ContactCH = oContact.CloseHour(_routeDate) ''_routeDate.AddDays(addDays * -1)

            If ContactCH <= ContactOH Then
                res = 0
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Could not Place Requirement to Trip. Contact Close  point - " & req.PointID & _
                     "  contact - " & req.ContactId & _
                     "  order - " & req.OrderId & _
                     " Day of week - " & _routeDate.DayOfWeek.ToString())
                End If
                ''If addDays = 0 Or ContactCH < ContactOH Then req.planned = 1
                Return res
            End If

            _routeDepoStartDate = _routeDate.AddDays(addDays)

            If ContactCH = 0 Then Return True

            Dim addDepDays As Integer = 0
            Dim addWtDays As Integer = 0
            StartimeCh = CalcEstimateArrivalTime(-1 * TempDrivingTime, ContactCH, addDepDays, oLogger)
            StartimeWt = CalcEstimateArrivalTime(-1 * (TempDrivingTime + oStratDet.AllowedTimeBeforeOpen), oContact.OpeningHour(_routeDate), addWtDays, oLogger)


            If StartimeOh <= oStratDet.DepoLatestStartTime And StartimeOh >= oStratDet.DepoStartTime Then
                res = 1
            ElseIf StartimeOh >= oStratDet.DepoLatestStartTime And StartimeWt <= oStratDet.DepoLatestStartTime Then
                res = 2
            ElseIf StartimeOh <= oStratDet.DepoStartTime And (StartimeCh >= oStratDet.DepoStartTime OrElse ContactCH = 0) Then
                res = 3
            Else
                res = 0
            End If

            If Not oLogger Is Nothing AndAlso res = 0 Then
                oLogger.Write("*** Check. #Contact Open Hours does not match. Could not Place Requirement res:" & res)
                oLogger.Write("  point - " & req.PointID & _
                 "  contact - " & req.ContactId & _
                 "  order - " & req.OrderId & _
                 " StartimeOh: " & StartimeOh & _
                 " StartimeCh: " & StartimeCh & _
                 " StartimeWt: " & StartimeWt & _
                 " DrivingTime: " & TempDrivingTime & _
                 " ContactOH:" & ContactOH & _
                 " ContactOCH:" & ContactCH & _
                 " Day Of Week:" & _routeDate.DayOfWeek & _
                 " add days:" & addDays & _
                 " Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
            End If
        Else
            ''trip
            Dim StartTripTime As DateTime
            Try
                StartTripTime = EndDateTime.AddSeconds(oStratDet.DepotReloadTime)
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** checkDepotStopTimeType error: " & ex.ToString())
                End If

            End Try

            Dim ContactOH = oContact.OpeningHour(StartTripTime)
            Dim ContactCH = oContact.CloseHour(StartTripTime)

            ''If ContactOH = 0 Then Return True

            Dim DepotExitHour As Integer = StartTripTime.Hour * 100 + StartTripTime.Minute ''depot exit hour
            Dim ContactEnterHour As Integer = CalcEstimateArrivalTime(TempDrivingTime, DepotExitHour, addDays, oLogger)

            Dim ContactWTEnterHour As Integer = CalcEstimateArrivalTime(TempDrivingTime + oStratDet.AllowedTimeBeforeOpen, DepotExitHour, addDays, oLogger)

            If (DepotExitHour <= oStratDet.DepoLatestStartTime And DepotExitHour >= oStratDet.DepoStartTime) _
                AndAlso (ContactEnterHour >= ContactOH And ContactEnterHour <= ContactCH) Then
                res = 4
            ElseIf (DepotExitHour <= oStratDet.DepoLatestStartTime And DepotExitHour >= oStratDet.DepoStartTime) _
                AndAlso (ContactWTEnterHour >= ContactOH And ContactEnterHour <= ContactCH) Then
                res = 5
            Else
                res = 0
            End If

            If Not oLogger Is Nothing AndAlso res = 0 Then
                oLogger.Write("*** Check. #Contact Open Hours does not match. Could not Place Requirement to Trip res:" & res)
                oLogger.Write("  point - " & req.PointID & _
                 "  contact - " & req.ContactId & _
                 "  order - " & req.OrderId & _
                 " DepotExitHour: " & DepotExitHour & _
                 " ContactEnterHour: " & ContactEnterHour & _
                 " ContactWTEnterHour: " & ContactWTEnterHour & _
                 " DrivingTime: " & TempDrivingTime & _
                 " ContactOH:" & ContactOH & _
                 " ContactOCH:" & ContactCH & _
                 " Day Of Week:" & StartTripTime.DayOfWeek & _
                 " Depot Reload Time:" & oStratDet.DepotReloadTime & _
                 " Previous Trip End Date Time:" & EndDateTime.ToString() & _
                 " Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
            End If

        End If

        Return res
    End Function

    Public Function calcDepotStopTime(ByVal oStrategy As RoutingStrategy, _
        ByVal req As RoutingRequirement, _
            ByVal oContact As Contact, _
            ByRef TempDrivingTime As Integer, _
            ByRef EstArrivalTime As Integer, _
            ByRef EstDepartureTime As Integer, _
            ByRef addDepDays As Integer, _
            ByRef addArrivalDays As Integer, _
            ByVal oLogger As WMS.Logic.LogHandler) As Integer

        Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)

        Dim StartimeOh, StartimeCh, StartimeWt As Integer
        Dim addDays, addDaysWt, addCloseDays As Integer
        Dim res As Integer = 0


        Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
        Dim TripNumber As Integer = oVehicleDataArray(2)
        Dim EndDateTime As DateTime = oVehicleDataArray(3)

        If TripNumber = 0 Then
            Dim ContactOH As Integer = oContact.OpeningHour(_routeDate)
            Dim ContactCH As Integer = oContact.CloseHour(_routeDate)

            If ContactOH = 0 Then
                StartimeOh = oStratDet.DepoStartTime
                ContactOH = CalcEstimateArrivalTime(TempDrivingTime, StartimeOh, addDays, oLogger)
            Else
                StartimeOh = CalcEstimateArrivalTime(-1 * TempDrivingTime, ContactOH, addDays, oLogger)
                StartimeCh = CalcEstimateArrivalTime(-1 * TempDrivingTime, ContactCH, addCloseDays, oLogger)
                StartimeWt = CalcEstimateArrivalTime(-1 * (TempDrivingTime + oStratDet.AllowedTimeBeforeOpen), ContactOH, addDaysWt, oLogger)
            End If

            If StartimeOh <= oStratDet.DepoLatestStartTime And StartimeOh >= oStratDet.DepoStartTime Then
                res = 1
                EstArrivalTime = ContactOH
                EstDepartureTime = EstArrivalTime
                req.addDepDays = addDays * (-1)
                _routestarttime = StartimeOh
            ElseIf StartimeOh >= oStratDet.DepoLatestStartTime And StartimeWt <= oStratDet.DepoLatestStartTime Then
                res = 2
                EstArrivalTime = ContactOH
                EstDepartureTime = EstArrivalTime ''needs add serv time
                TempDrivingTime = DifTimeSecwithDays(EstDepartureTime, EstArrivalTime)
                EstDepartureTime = EstArrivalTime

                addDaysWt = TempDrivingTime \ 86400
                'If addDaysWt > 0 Then
                '    ''addDaysWt = -1
                '    _routeDate = _routeDate.AddDays(addDaysWt)
                'End If
                req.addDepDays = addDays * (-1)
                _routestarttime = oStratDet.DepoLatestStartTime
            ElseIf StartimeOh <= oStratDet.DepoStartTime And (StartimeCh >= oStratDet.DepoStartTime OrElse ContactCH = 0) Then
                res = 3
                EstArrivalTime = CalcEstimateArrivalTime(TempDrivingTime, oStratDet.DepoStartTime, addDays, oLogger)
                EstDepartureTime = EstArrivalTime
                req.addDepDays = addDays * (-1)
                _routestarttime = oStratDet.DepoStartTime
            End If

            'If Not oLogger Is Nothing Then
            '    If res = 0 Then
            '        oLogger.Write("*** Place. #Contact Open Hours does not match. Could not Place Requirement resStartTime:" & res)
            '        oLogger.Write("  point - " & req.PointID & _
            '         "  contact - " & req.ContactId & _
            '         "  order - " & req.OrderId & _
            '         " Depot EstDepTime: " & EstDepartureTime & _
            '         " Stop EstArrivalTime: " & EstArrivalTime & _
            '         " StartimeOh: " & StartimeOh & _
            '         " StartimeCh: " & StartimeCh & _
            '         " StartimeWt: " & StartimeWt & _
            '         " DrivingTime: " & TempDrivingTime & _
            '         " Day Of Week:" & _routeDate.DayOfWeek & _
            '         " addArrivalDays:" & addArrivalDays & _
            '         " addDepDays:" & addDepDays & _
            '         " Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
            '    End If
            'End If

        Else
            Dim StartTripTime As DateTime = EndDateTime.AddSeconds(oStratDet.DepotReloadTime)

            Dim ContactOH = oContact.OpeningHour(StartTripTime)
            Dim ContactCH = oContact.CloseHour(StartTripTime)

            Dim DepotExitHour As Integer = StartTripTime.Hour * 100 + StartTripTime.Minute ''depot exit hour
            Dim ContactEnterHour As Integer = CalcEstimateArrivalTime(TempDrivingTime, DepotExitHour, addDays, oLogger)
            Dim ContactWTEnterHour As Integer = CalcEstimateArrivalTime(TempDrivingTime + oStratDet.AllowedTimeBeforeOpen, DepotExitHour, addDays, oLogger)

            StartimeOh = DepotExitHour
            ContactOH = CalcEstimateArrivalTime(TempDrivingTime, StartimeOh, addDays, oLogger)
            StartimeWt = CalcEstimateArrivalTime((TempDrivingTime + oStratDet.AllowedTimeBeforeOpen), ContactOH, addDaysWt, oLogger)


            If (DepotExitHour <= oStratDet.DepoLatestStartTime And DepotExitHour >= oStratDet.DepoStartTime) _
                AndAlso (ContactEnterHour >= ContactOH And ContactEnterHour <= ContactCH) Then
                res = 4

                EstArrivalTime = CalcEstimateArrivalTime(TempDrivingTime, DepotExitHour, addDays, oLogger)
                EstDepartureTime = EstArrivalTime
                req.addDepDays = addDays * (-1)
                _routestarttime = DepotExitHour
            ElseIf (DepotExitHour <= oStratDet.DepoLatestStartTime And DepotExitHour >= oStratDet.DepoStartTime) _
                AndAlso (ContactWTEnterHour >= ContactOH And ContactEnterHour <= ContactCH) Then
                EstArrivalTime = ContactOH
                TempDrivingTime = DifTimeSecwithDays(DepotExitHour, EstArrivalTime)
                EstDepartureTime = EstArrivalTime

                addDaysWt = TempDrivingTime \ 86400

                EstDepartureTime = EstArrivalTime
                req.addDepDays = addDays * (-1)
                _routestarttime = DepotExitHour
                res = 5
            Else
                res = 0
            End If

        End If


        ''''''''''''''''





        Return res
    End Function

    ''STOP1->STOP2
    Public Function checkStoptoStopStartTimeType(ByVal oStratDet As RoutingStrategyDetail, _
        ByVal req As RoutingRequirement, _
        ByVal oContact As Contact, _
        ByVal prevStopDepTime As Integer, _
        ByVal addArrivalDays As Integer, _
        ByVal TempDrivingTime As Integer, _
        ByVal oLogger As WMS.Logic.LogHandler) As Integer

        Dim EstArrivalTime, EstArrivalTimeWt As Integer
        Dim addDays, addDaysWt As Integer
        Dim res As Integer

        EstArrivalTime = CalcEstimateArrivalTime(TempDrivingTime, prevStopDepTime, addDays, oLogger)
        EstArrivalTimeWt = CalcEstimateArrivalTime(TempDrivingTime + oStratDet.AllowedTimeBeforeOpen, prevStopDepTime, addDaysWt, oLogger)

        Dim ContactOH As Integer = oContact.OpeningHour(_routeDepoStartDate.AddDays(addArrivalDays))
        Dim ContactCH As Integer = oContact.CloseHour(_routeDepoStartDate.AddDays(addArrivalDays + addDays))

        If ContactCH <= ContactOH Then
            res = 0
            If Not oLogger Is Nothing Then
                oLogger.Write("*** Could not Place Requirement to Trip. Contact Close  point - " & req.PointID & _
                 "  contact - " & req.ContactId & _
                 "  order - " & req.OrderId & _
                 " Day of week - " & _routeDate.DayOfWeek.ToString())
            End If
            ''If addDays = 0 Or ContactCH < ContactOH Then req.planned = 1
            Return res
        End If


        If ContactOH = 0 Then Return True
        If ContactCH = 0 Then Return True

        If EstArrivalTime >= ContactOH And _
            EstArrivalTime <= ContactCH Then
            res = 1
        ElseIf ((EstArrivalTimeWt >= oContact.OpeningHour(_routeDate.AddDays(addArrivalDays + addDaysWt)) _
                    And (EstArrivalTime <= ContactCH)) OrElse _
                    (((addDaysWt - addArrivalDays) > 0) And (EstArrivalTime <= ContactCH))) Then
            res = 2
        Else
            res = 0
        End If


        If Not oLogger Is Nothing AndAlso res = 0 Then
            oLogger.Write("*** Could not Place Requirement #Contact Open Hours does not match. res:" & res)
            oLogger.Write("  point - " & req.PointID & _
             "  contact - " & req.ContactId & _
             "  order - " & req.OrderId & _
             " prevStopDepTime: " & prevStopDepTime & _
             " EstArrivalTime: " & EstArrivalTime & _
             " EstArrivalTimeWt: " & EstArrivalTimeWt & _
             " DrivingTime: " & TempDrivingTime & _
             " Day Of Week:" & _routeDate.DayOfWeek & _
             " ContactOH:" & ContactOH & _
             " ContactCH:" & ContactCH & _
             " addArrivalDays:" & addArrivalDays & _
             " add days:" & addDays & _
             " addDaysWt:" & addDaysWt & _
             " Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
        End If

        Return res
    End Function

    Public Function calcStoptoStopArrivalDepartureTime(ByVal oStratDet As RoutingStrategyDetail, _
        ByVal req As RoutingRequirement, _
        ByVal oContact As Contact, _
        ByVal prevStopDepTime As Integer, _
            ByRef EstArrivalTime As Integer, _
            ByRef EstDepartureTime As Integer, _
            ByRef addDepDays As Integer, _
            ByRef addArrivalDays As Integer, _
        ByRef TempDrivingTime As Integer, _
        ByVal oLogger As WMS.Logic.LogHandler) As Integer

        Dim EstArrivalTimeWt As Integer
        Dim addDays, addDaysWt As Integer
        Dim res As Integer

        EstArrivalTime = CalcEstimateArrivalTime(TempDrivingTime, prevStopDepTime, addDays, oLogger)
        EstArrivalTimeWt = CalcEstimateArrivalTime(TempDrivingTime + oStratDet.AllowedTimeBeforeOpen, prevStopDepTime, addDaysWt, oLogger)

        Dim ContactOH As Integer = oContact.OpeningHour(_routeDepoStartDate.AddDays(addDepDays))
        If ContactOH = 0 Then ContactOH = oStratDet.DepoStartTime
        Dim ContactCH As Integer = oContact.CloseHour(_routeDepoStartDate.AddDays(addDepDays + addDays))

        If (EstArrivalTime >= ContactOH OrElse ContactOH = 0) And _
               (EstArrivalTime <= ContactCH OrElse ContactCH = 0) Then
            EstDepartureTime = EstArrivalTime
            addDays = TempDrivingTime \ 86400
            req.addDepDays = addDepDays + addDays

            res = 1
        ElseIf ((EstArrivalTimeWt >= oContact.OpeningHour(_routeDate.AddDays(addArrivalDays + addDaysWt)) OrElse _
                ((EstArrivalTime <= ContactCH) And (addDaysWt - addArrivalDays) > 0)) _
                OrElse ContactOH = 0) Then
            EstArrivalTime = Math.Max(EstArrivalTime, ContactOH)
            TempDrivingTime = DifTimeSecwithDays(prevStopDepTime, EstArrivalTime)
            EstDepartureTime = EstArrivalTime
            If prevStopDepTime > EstArrivalTime Then
                addDaysWt = 1 + TempDrivingTime \ 86400
            Else
                addDaysWt = TempDrivingTime \ 86400
            End If

            req.addDepDays = addDepDays + addDaysWt
            res = 2
        Else
            res = 0
        End If


        'If Not oLogger Is Nothing Then
        '    If res = 0 Then
        '        oLogger.Write("*** Could not Place Requirement res:" & res)
        '    Else
        '        oLogger.Write("#Place Requirement:")
        '    End If
        '    oLogger.Write("  point - " & req.PointID & _
        '     "  contact - " & req.ContactId & _
        '     "  order - " & req.OrderId & _
        '     "  #Contact Open Hours does not match. " & _
        '     " prevStopDepTime: " & prevStopDepTime & _
        '     " EstArrivalTime: " & EstArrivalTime & _
        '     " EstArrivalTimeWt: " & EstArrivalTimeWt & _
        '     " EstDepartureTime: " & EstDepartureTime & _
        '     " DrivingTime: " & TempDrivingTime & _
        '     " Day Of Week:" & _routeDate.DayOfWeek & _
        '     " addArrivalDays:" & addArrivalDays & _
        '     " add days:" & addDays & _
        '     " addDaysWt:" & addDaysWt & _
        '     " Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
        'End If

        Return res
    End Function


    Public Sub SetRouteStartTime(ByRef oStratDet As RoutingStrategyDetail, ByVal oContact As Contact)
        Try
            'Check if we have a starttime to the route
            If _routestarttime = -1 Then

                If oStratDet.CalcRetTimeToFirstStop Then
                    _routestarttime = oStratDet.DepoStartTime
                    _routelateststarttime = oStratDet.DepoLatestStartTime

                    If _routestarttime = -1 Then
                        _routestarttime = Made4Net.Shared.SysParam.Get("RoutingDefOpenHours")
                        _routelateststarttime = Made4Net.Shared.SysParam.Get("RoutingDefCloseHours")
                    End If
                Else
                    If oContact.OpeningHour(_routeDate) <> -1 Then
                        _routestarttime = oContact.OpeningHour(_routeDate)
                        _routelateststarttime = oContact.CloseHour(_routeDate)
                    Else
                        _routestarttime = Made4Net.Shared.SysParam.Get("RoutingDefOpenHours")
                        _routelateststarttime = Made4Net.Shared.SysParam.Get("RoutingDefCloseHours")
                    End If

                End If
            End If

        Catch ex As Exception
            _routestarttime = Made4Net.Shared.SysParam.Get("RoutingDefOpenHours")
            _routelateststarttime = Made4Net.Shared.SysParam.Get("RoutingDefCloseHours")

        End Try

    End Sub



#End Region

#Region "Accessors"

    Public Function ConvertBasketToArrayList(ByVal oStrategy As RoutingStrategy) As ArrayList
        Dim newAL As New ArrayList
        Dim i As Int32
        newAL.Add("@")
        Dim firstStop As RoutingRequirement = CType(ReqCollection(0), RoutingRequirement)
        Dim lastStop As RoutingRequirement = CType(ReqCollection(ReqCollection.Count - 1), RoutingRequirement)

        Dim routeendtime As Integer = lastStop.EstDepartureHour

        Dim oStratDet As RoutingStrategyDetail = oStrategy.StrategyDetails(0)

        Dim TerritorySetID As String = String.Empty
        Dim TerritoryID As String = String.Empty
        If firstStop.TerritoryKey <> String.Empty Then
            TerritorySetID = ""
            TerritoryID = firstStop.TerritoryKey
        End If


        Dim isTargetFillCapacity As Boolean = (TotalVolume >= oStratDet.TargetFillVolume * _vehicletypeObj.TOTALVOLUME) Or (TotalWeight >= oStratDet.TargetFillWeight * _vehicletypeObj.TOTALWEIGHT)

        Dim oVehicleDataArray As ArrayList = oStrategy.VechicleInTripPool(CurrentVehicleKey)
        Dim TripNumber As Integer = Me.TripNum
        Dim TripGroup As String = Me.TripGroup


        newAL.Add(_totaldistance.ToString() & _
            "#" & TotalTime.ToString() & _
            "#" & Math.Round(_totalvolume, 4).ToString() & _
            "#" & Math.Round(_totalweight, 4).ToString() & _
            "#" & _vehicletype & _
            "#" & _routestarttime.ToString() & _
            "#" & _stops(_stops.Count - 1).split("#")(4).ToString() & _
            "#" & firstStop.PointID.ToString() & _
            "#" & lastStop.PointID.ToString() & _
            "#" & _depopointid.ToString() & _
            "#" & TerritorySetID & _
            "#" & TerritoryID & _
            "#" & Math.Round(_totalpickupvolume, 4).ToString() & _
            "#" & Math.Round(_totalpickupweight, 4).ToString() & _
            "#" & Math.Round(_totaldeliveryvolume, 4).ToString() & _
            "#" & Math.Round(_totaldeliveryweight, 4).ToString() & _
            "#" & Me.NumStops.ToString() & _
            "#" & ReqCollection.Count.ToString() & _
            "#" & isTargetFillCapacity.ToString() & _
            "#" & _stops(_stops.Count - 1).split("#")(5) & _
            "#" & lastStop.addDepDays & _
            "#" & _routeDepoStartDate.ToString() & _
            "#" & _CurrentVehicle.ToString() & _
            "#" & TripNumber.ToString() & _
            "#" & TripGroup.ToString())





        For i = 0 To _stops.Count - 1
            newAL.Add("-")
            newAL.Add(_stops(i))
        Next
        Return newAL
    End Function

#End Region

#End Region

End Class

#End Region

#Region "RouteBasketsCollection"

<CLSCompliant(False)> Public Class RouteBasketsCollection
    Implements ICollection

#Region "Variables"

    Protected _strat As RoutingStrategy
    Protected _routebaskets As ArrayList
    Protected _routingSetDate As DateTime
    Protected _OrdersRoutingParamTable As DataTable
    Protected _isDistanceAdd As Integer = 0
    Protected _distcosttype As Integer = 0
    Protected _isdistcost As Integer = 0

#End Region

#Region "Constructor"

    Public Sub New(ByVal oRoutingStrategy As RoutingStrategy, ByVal RoutingDate As DateTime, ByVal OrdersParam As DataTable)
        _strat = oRoutingStrategy
        ''        _strat.InitAvailableVehicle()
        _routebaskets = New ArrayList
        _routingSetDate = RoutingDate
        _OrdersRoutingParamTable = OrdersParam
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)

    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As RouteBaskets
        Get
            Return CType(_routebaskets(index), RouteBaskets)
        End Get
        Set(ByVal Value As RouteBaskets)
            _routebaskets(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"
    Protected Function CheckisTheLastTask(ByVal i As Integer, ByVal chrom As Chromosome) As Boolean
        Return False
        If i = chrom.GetSize() - 1 Then Return True
        If i <= chrom.GetSize() Then
            Dim tpmReq As RoutingRequirement = CType(_strat.Item(chrom.GetGene(i)), RoutingRequirement)
            Dim nextReq As RoutingRequirement = CType(_strat.Item(chrom.GetGene(i + 1)), RoutingRequirement)
            If tpmReq.PointID <> nextReq.PointID Then Return True
        End If
        Return False
    End Function
    Protected Function isPlaceroute(ByVal pRoutingBasket As RouteBaskets, _
                                    ByVal numExit As Integer, _
                                    ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        Dim cntFalse As Integer = 0
        Dim messageWeight As String = String.Empty
        Dim messageVolume As String = String.Empty
        Dim oStratDet As RoutingStrategyDetail = _strat.StrategyDetails(0)
        If (oStratDet.MinRouteWeight > 0 And pRoutingBasket.TotalWeight < oStratDet.MinRouteWeight) Then
            If Not oLogger Is Nothing Then
                messageWeight = "### Could not place Route. Reqs:" & pRoutingBasket.ReqCollection.Count & " #Weight not enough:" & pRoutingBasket.TotalWeight & " min weight:" & oStratDet.MinRouteWeight
            End If
            cntFalse += 1
        End If
        If (oStratDet.MinRouteVolume > 0 And pRoutingBasket.TotalVolume < oStratDet.MinRouteVolume) Then
            messageVolume = "### Could not place Route. Reqs:" & pRoutingBasket.ReqCollection.Count & " #Volume not enough:" & pRoutingBasket.TotalVolume & " min volume:" & oStratDet.MinRouteVolume
            cntFalse += 1
        End If
        If cntFalse = 2 Then
            If Not oLogger Is Nothing Then
                If messageWeight <> String.Empty Then oLogger.Write(messageWeight)
                If messageVolume <> String.Empty Then oLogger.Write(messageVolume)
            End If
            If numExit > RoutingStrategies.maxexit Then
                For Each oReq As RoutingRequirement In pRoutingBasket.ReqCollection
                    oReq.planned = 2
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Temporary exclude from planing:" & _
                            " point - " & oReq.PointID & _
                            " contact - " & oReq.ContactId & _
                            " order - " & oReq.OrderId)
                    End If
                Next

            End If
            Return False
        Else
            Return True
        End If
    End Function


    Public Function decodeChromosome2(ByRef chrom As Chromosome, ByRef sb As StringBuilder, _
               ByVal oLogger As WMS.Logic.LogHandler, _
               ByVal numExit As Integer, _
                ByVal prevcount As Integer) As System.Collections.ArrayList
        Try
            sb.AppendLine()
            Dim oRoutingBasket As RouteBaskets
            Dim tmpReq, lastPlacedReq As RoutingRequirement
            Dim i As Int32
            Dim lastChromIndex As Int32 = 0
            Dim stratDet As RoutingStrategyDetail = _strat.StrategyDetails(0)

            Dim oVehicle As VehicleType = _strat.GetNextAvailableVehicleWithTrips(numExit, prevcount)
            If oVehicle Is Nothing Then
                numExit = RoutingStrategies.maxexit + 1
                Return New ArrayList()
            End If

            oRoutingBasket = New RouteBaskets(_routingSetDate, oVehicle)
            oRoutingBasket.CurrentVehicle = _strat.VehicleCounterFromPool
            oRoutingBasket.CurrentVehicleKey = _strat.CurrentVehicleKey


            oRoutingBasket.InitServTimeCalcParameters(stratDet)

            Dim oContact As Contact

            Dim TempDistance As Double = 0D, TempDrivingTime As Double = 0D
            Dim routenum As Integer = 1
            Dim oStratDet As RoutingStrategyDetail = _strat.StrategyDetails(0)
            Dim isCanPlace As Boolean
            Dim isExcludeReq As Boolean = False ''???

            Dim prevReq As RoutingRequirement
            For i = 0 To chrom.GetSize() - 1
                tmpReq = _strat.Item(chrom.GetGene(i))
                If tmpReq.planned = 1 Then Continue For

                If numExit > RoutingStrategies.maxexit And tmpReq.planned = 2 Then
                    Continue For
                End If


                Dim islaststop As Boolean = CheckisTheLastTask(i, chrom)
                isExcludeReq = False

                isCanPlace = True
                Dim i1, i2, i3 As Integer

                i1 = i
                If i1 <> chrom.GetSize() - 1 Then
                    i2 = i + 1
                Else
                    i2 = i1
                End If

                Dim checktmpReq As RoutingRequirement = tmpReq
                Dim checknextReq As RoutingRequirement = tmpReq
                While i2 < chrom.GetSize() - 1
                    checktmpReq = checknextReq
                    checknextReq = _strat.Item(chrom.GetGene(i2))

                    If Not checkTheSame(checktmpReq, checknextReq, numExit) Then
                        i2 -= 1
                        Exit While
                    End If
                    i2 += 1
                End While

                Dim checkprevReq As RoutingRequirement = prevReq
                checktmpReq = tmpReq
                For j As Integer = i1 To i2
                    checktmpReq = _strat.Item(chrom.GetGene(j))

                    Dim samePoint As Boolean = checkTheSamePoint(checkprevReq, checktmpReq, j, i1, i2)
                    If Not samePoint Then
                        oRoutingBasket.InitServTimeCalcParameters(stratDet)
                    End If

                    isCanPlace = oRoutingBasket.CanPlace(_strat, lastPlacedReq, checkprevReq, checktmpReq, TempDistance, TempDrivingTime, samePoint, islaststop, isExcludeReq, oLogger)
                    checkprevReq = checktmpReq

                    If Not isCanPlace Then
                        Dim checktmpReq1, checktmpReq2 As RoutingRequirement

                        checktmpReq1 = _strat.Item(chrom.GetGene(i1))
                        checktmpReq2 = _strat.Item(chrom.GetGene(j))
                        ''checkTheSame(checktmpReq1, checktmpReq2, numExit)
                        If i1 = 0 And checkTheSamePoint(checktmpReq1, checktmpReq2, j, i1, i2) Then
                            i3 = j - 1
                        Else
                            i3 = i1 - 1
                        End If

                        Exit For
                    End If
                Next

                If Not isCanPlace And i1 > 0 Then
                    If i2 = chrom.GetSize() - 1 Then
                        Exit For
                    Else
                        i = i2
                    End If
                End If

                ''''place first req and place all same req
                ''debug

                If (Not isCanPlace And i1 = 0 And i3 >= 0) Then
                    Dim tmpReqlog3, tmpReqlog1 As RoutingRequirement
                    tmpReqlog3 = _strat.Item(chrom.GetGene(i3))
                    tmpReqlog1 = _strat.Item(chrom.GetGene(i1))
                    sb.AppendLine("*** Split the first stop in route: " & _
                                        "  point - " & tmpReqlog3.PointID & _
                                        "  contact - " & tmpReqlog3.ContactId & _
                                        " order - " & tmpReqlog3.OrderId)
                End If


                If (Not isCanPlace And i1 = 0 And i3 >= 0) Or isCanPlace Then
                    Dim imax As Integer = i3
                    If isCanPlace Then imax = i2

                    lastPlacedReq = prevReq
                    checktmpReq = tmpReq
                    For j As Integer = i1 To imax
                        chrom.SetRouteGeneNum(j, routenum)
                        checktmpReq = _strat.Item(chrom.GetGene(j))

                        Dim samePoint As Boolean = checkTheSamePoint(lastPlacedReq, checktmpReq, j, i1, i2)
                        If Not samePoint Then
                            oRoutingBasket.InitServTimeCalcParameters(stratDet)
                        End If

                        If Not lastPlacedReq Is Nothing Then
                            If samePoint Then
                                checktmpReq.EstArrivalHour = lastPlacedReq.EstDepartureHour
                                checktmpReq.EstDepartureHour = lastPlacedReq.EstDepartureHour
                                checktmpReq.addDepDays = lastPlacedReq.addDepDays
                            Else
                                Dim addDays As Integer = 0
                                GeoNetworkItem.GetDistance(lastPlacedReq.TargetGeoPoint, checktmpReq.TargetGeoPoint, TempDistance, TempDrivingTime, Common.GetCurrentUser, _isDistanceAdd, _
                                    _isdistcost, _distcosttype)
                                checktmpReq.EstArrivalHour = RouteBaskets.CalcEstimateArrivalTime(TempDrivingTime, lastPlacedReq.EstDepartureHour, addDays, oLogger)
                                checktmpReq.addDepDays += addDays
                                checktmpReq.EstDepartureHour = checktmpReq.EstArrivalHour
                            End If

                        End If

                        If samePoint Then
                            oRoutingBasket.Place(samePoint, _strat, chrom.GetGene(j), checktmpReq, 0, 0, oLogger)
                        Else
                            oRoutingBasket.Place(samePoint, _strat, chrom.GetGene(j), checktmpReq, TempDistance, TempDrivingTime, oLogger)
                        End If

                        prevReq = _strat.Item(chrom.GetGene(j))
                        lastPlacedReq = checktmpReq
                    Next

                    i = imax
                End If


                'If i > 0 And oStratDet.ContinueRouteWithReq = 0 And Not isCanPlace Then
                '    Exit For
                'End If
                If i > 0 And oStratDet.ContinueRouteWithReq = 0 And Not isCanPlace And _
                    ((oRoutingBasket.TotalWeight >= oStratDet.TargetFillWeight * oVehicle.TOTALWEIGHT) Or _
                    (oRoutingBasket.TotalVolume >= oStratDet.TargetFillVolume * oVehicle.TOTALVOLUME)) Then
                    Exit For
                End If



            Next

            ''try to place to new route
            If Not Contains(oRoutingBasket) And oRoutingBasket.StopsCollection.Count > 0 _
                AndAlso isPlaceroute(oRoutingBasket, numExit, oLogger) Then

                addReturnRouteData(stratDet, oRoutingBasket, _
                    oRoutingBasket.ReqCollection(oRoutingBasket.ReqCollection.Count - 1), sb)

                Me.Add(oRoutingBasket)

                routenum += 1
                sb.AppendLine()

                Dim StartDateTime As DateTime = New DateTime(oRoutingBasket.RouteDepoStartDate.Year, _
                    oRoutingBasket.RouteDepoStartDate.Month, _
                    oRoutingBasket.RouteDepoStartDate.Day, _
                    oRoutingBasket.RouteStartTime \ 100, _
                    oRoutingBasket.RouteStartTime Mod 100, 0)
                Dim EndDateTime As DateTime = StartDateTime.AddSeconds(oRoutingBasket.TotalTime)
                _strat.SetTripVehicleData(EndDateTime, oRoutingBasket.TotalDistance, _
                    oRoutingBasket.TotalTime, oRoutingBasket.CurrentVehicleKey, oRoutingBasket.TripGroup, oRoutingBasket.TripNum, prevcount)
            Else

                Dim oVehicleDataArray As ArrayList = _strat.VechicleInTripPool(_strat.CurrentVehicleKey)
                Dim TripNumber As Integer = oVehicleDataArray(2)
                If TripNumber = 0 Or numExit < 2 Then
                    _strat.StrategyVehicleAllocation.RestoreAvailableVehicle(oVehicle.VEHICLETYPEID)
                End If
            End If

            If Not oLogger Is Nothing Then
                sb.AppendLine("Total Routes for decoding " & Me.Count)
            End If


            Return ConvertBasketsToArrayList(_strat)

        Catch ex As Exception
            sb.AppendLine("decodeChromosome error: " & ex.ToString())
            Return Nothing
        End Try

    End Function

    Public Shared Function checkInit(ByVal prevReq As RoutingRequirement, _
                                      ByVal currentReq As RoutingRequirement) As Boolean
        If prevReq Is Nothing OrElse currentReq Is Nothing Then Return False
        If currentReq.PointID = prevReq.PointID Then Return True
        If currentReq.ContactId = prevReq.ContactId Then Return True
        If currentReq.OrderId = prevReq.OrderId Then Return True
        Return False
    End Function



    Protected Sub addReturnRouteData(ByVal stratDet As RoutingStrategyDetail, _
                ByRef oRoutingBasket As RouteBaskets, _
        ByVal tmpReq As RoutingRequirement, _
        ByRef sb As StringBuilder)

        If stratDet.CalcRetTimeToDepot Then
            If Not stratDet.Depot.POINTID Is Nothing AndAlso stratDet.Depot.POINTID <> "" Then
                ''Dim DepoStop As GeoPointNode = GeoPointNode.GetPointNode(oStratDet.Depot.POINTID)
                Dim DepoStop As GeoPointNode = New GeoPointNode(stratDet.Depot.POINTID)

                Dim LastStop As GeoPointNode = New GeoPointNode(tmpReq.PointID)
                Dim lasttoDepoDistance As Double = 0D
                Dim lasttoDepoDrvTime As Double = 0D

                If Not DepoStop Is Nothing Then
                    GeoNetworkItem.GetDistance(LastStop, DepoStop, lasttoDepoDistance, lasttoDepoDrvTime, Common.GetCurrentUser, _isDistanceAdd, _
                            _isdistcost, _distcosttype)
                    oRoutingBasket.TotalDistance += lasttoDepoDistance
                    oRoutingBasket.TotalTime += lasttoDepoDrvTime
                End If
            Else
                sb.AppendLine("Error: Undefined Depot PointID")
            End If

        End If
    End Sub
    ''prevent split close points
    Protected Function checkTheSame(ByVal prevReq As RoutingRequirement, _
                                    ByVal currentReq As RoutingRequirement, _
    ByVal numexit As Integer) As Boolean
        If prevReq Is Nothing OrElse currentReq Is Nothing Then Return False
        Dim oStratDet As RoutingStrategyDetail = _strat.StrategyDetails(0)


        If oStratDet.AllowSplitStop = "0" AndAlso currentReq.PointID = prevReq.PointID Then Return True
        If oStratDet.AllowSplitContact = "0" AndAlso currentReq.ContactId = prevReq.ContactId Then Return True
        If oStratDet.AllowSplitOrder = "0" AndAlso currentReq.OrderId = prevReq.OrderId Then Return True

        Dim TempDistance, TempDrivingTime As Double
        GeoNetworkItem.GetDistance(prevReq.TargetGeoPoint, currentReq.TargetGeoPoint, TempDistance, TempDrivingTime, Common.GetCurrentUser, _isDistanceAdd, _
            _isdistcost, _distcosttype)
        If numexit < 4 And oStratDet.AllowSplitStop = "0" AndAlso oStratDet.DistanceForSplitStop > 0 Then
            If TempDistance <= oStratDet.DistanceForSplitStop Then Return True
        End If


        Return False
    End Function

    Protected Function checkTheSame_old(ByVal prevReq As RoutingRequirement, _
                                    ByVal currentReq As RoutingRequirement) As Boolean
        If prevReq Is Nothing OrElse currentReq Is Nothing Then Return False
        Dim oStratDet As RoutingStrategyDetail = _strat.StrategyDetails(0)
        If oStratDet.AllowSplitStop = "0" AndAlso currentReq.PointID = prevReq.PointID Then Return True
        If oStratDet.AllowSplitContact = "0" AndAlso currentReq.ContactId = prevReq.ContactId Then Return True
        If oStratDet.AllowSplitOrder = "0" AndAlso currentReq.OrderId = prevReq.OrderId Then Return True
        Return False
    End Function

    Protected Function checkTheSamePoint(ByVal prevReq As RoutingRequirement, _
                                    ByVal currentReq As RoutingRequirement, _
                                    ByVal j As Integer, ByVal i1 As Integer, ByVal i2 As Integer) As Boolean

        If j = i1 And i1 <> i2 Then Return False
        If prevReq Is Nothing And i1 = i2 Then Return False
        If prevReq Is Nothing And i1 <> i2 Then Return True
        If prevReq.PointID <> currentReq.PointID Or i1 = i2 Then Return False
        Return True
    End Function


    Private Function ConvertBasketsToArrayList(ByVal strat As RoutingStrategy) As ArrayList
        Dim newAL As New ArrayList
        For Each oRoutingBasket As RouteBaskets In Me
            If oRoutingBasket.NumStops > 0 Then
                newAL.AddRange(oRoutingBasket.ConvertBasketToArrayList(strat))
            End If
        Next
        Return newAL
    End Function


#End Region

#Region "Overrides"

    Public Function Add(ByVal value As RouteBaskets) As Integer
        Return _routebaskets.Add(value)
    End Function

    Public Sub Clear()
        _routebaskets.Clear()
    End Sub

    Public Function Contains(ByVal value As RouteBaskets) As Boolean
        _routebaskets.Contains(value)
    End Function

    Public Sub Insert(ByVal index As Integer, ByVal value As RouteBaskets)
        _routebaskets.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As RouteBaskets)
        _routebaskets.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _routebaskets.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _routebaskets.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _routebaskets.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _routebaskets.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _routebaskets.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _routebaskets.GetEnumerator()
    End Function

#End Region

End Class

#End Region

Public Class PDType
    Public Shared Delivery As String = "D"
    Public Shared Pickup As String = "P"
    Public Shared General As String = "G"

End Class



