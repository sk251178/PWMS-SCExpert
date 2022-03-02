Imports made4net.Shared.Conversion
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Evaluation
Imports Made4Net.GeoData
Imports System.Text

<CLSCompliant(False)> Public Class Route

#Region "Variables"

    Protected _routeid As String
    Protected _runid As String
    Protected _status As RouteStatus
    Protected _routeset As String
    Protected _depo As String
    Protected _routename As String
    Protected _routedate As DateTime
    Protected _startpoint As String
    Protected _endpoint As String
    Protected _vehicle As String
    Protected _vehicletype As String
    Protected _driver As String
    Protected _territory As String
    Protected _territorysetid As String
    Protected _routecost As Double
    Protected _totaldistance As Double
    Protected _totaltime As Double
    Protected _totalweight As Double
    Protected _totalvolume As Double
    'Protected _starttime As Integer
    'Protected _endtime As Integer

    Protected _startdate As DateTime
    Protected _enddate As DateTime


    Protected _actualstartdate As DateTime
    Protected _actualenddate As DateTime


    Protected _adduser As String
    Protected _adddate As DateTime
    Protected _edituser As String
    Protected _editdate As DateTime
    Protected _color As String
    Protected _oldstatus As RouteStatus
    Protected _isstatusdirty As Boolean = False
    Protected _strategyid As String
    Protected _routestops As RouteStopCollection

    Protected _DistMatrix As GeoNetworkDistanceCache
    Protected _isDistanceAdd As Integer = 0
    Protected _distcosttype As Integer = 0
    Protected _isdistcost As Integer = 0

    Protected ServTimeCalcParameters As Hashtable = New Hashtable()
    Protected _StopContactData As Hashtable = New Hashtable()
    Protected _StopData As ArrayList = New ArrayList()

    Protected _reasonunfeasibility As String
    Protected _tag As Double
    Protected _bestData As New ArrayList


    ''trips 08/09/2011
    Protected _tripnum As Integer
    Protected _tripgroup As String

    <ThreadStatic()> _
    Protected RouteCostCalcParameters As Hashtable = New Hashtable()

#End Region

#Region "Properties"
    Public Property Tag() As Double
        Get
            Return _tag
        End Get
        Set(ByVal value As Double)
            _tag = value
        End Set
    End Property

    Public Property TripNum() As Integer
        Get
            Return _tripnum
        End Get
        Set(ByVal value As Integer)
            _tripnum = value
        End Set
    End Property

    Public Property TripGroup() As String
        Get
            Return _tripgroup
        End Get
        Set(ByVal value As String)
            _tripgroup = value
        End Set
    End Property

    Public Property bestData() As ArrayList
        Get
            Return _bestData
        End Get
        Set(ByVal value As ArrayList)
            _bestData = value
        End Set
    End Property

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" RouteID = {0} ", Made4Net.Shared.Util.FormatField(_routeid))
        End Get
    End Property

    Public Property RouteId() As String
        Get
            Return _routeid
        End Get
        Set(ByVal value As String)
            _routeid = value
        End Set
    End Property

    Public Property RunId() As String
        Get
            Return _runid
        End Get
        Set(ByVal Value As String)
            _runid = Value
        End Set
    End Property

    Public Property Status() As RouteStatus
        Get
            Return _status
        End Get
        Set(ByVal Value As RouteStatus)
            If Not _status = Value Then
                _oldstatus = _status
                _status = Value
                _isstatusdirty = True
            End If
        End Set
    End Property

    Public Property Color() As String
        Get
            Return _color
        End Get
        Set(ByVal Value As String)
            _color = Value
        End Set
    End Property

    Public Property RouteSet() As String
        Get
            Return _routeset
        End Get
        Set(ByVal Value As String)
            _routeset = Value
        End Set
    End Property

    Public Property Depo() As String
        Get
            Return _depo
        End Get
        Set(ByVal Value As String)
            _depo = Value
        End Set
    End Property

    Public Property RouteName() As String
        Get
            Return _routename
        End Get
        Set(ByVal Value As String)
            _routename = Value
        End Set
    End Property

    Public Property RouteDate() As DateTime
        Get
            Return _routedate
        End Get
        Set(ByVal Value As DateTime)
            _routedate = Value
        End Set
    End Property

    Public Property StartPoint() As String
        Get
            Return _startpoint
        End Get
        Set(ByVal Value As String)
            _startpoint = Value
        End Set
    End Property

    Public Property EndPoint() As String
        Get
            Return _endpoint
        End Get
        Set(ByVal Value As String)
            _endpoint = Value
        End Set
    End Property

    Public Property Vehicle() As String
        Get
            Return _vehicle
        End Get
        Set(ByVal Value As String)
            _vehicle = Value
        End Set
    End Property

    Public Property VehicleType() As String
        Get
            Return _vehicletype
        End Get
        Set(ByVal Value As String)
            _vehicletype = Value
        End Set
    End Property

    Public Property Driver() As String
        Get
            Return _driver
        End Get
        Set(ByVal Value As String)
            _driver = Value
        End Set
    End Property
    Public Property TerritorySETID() As String
        Get
            Return _territorysetid
        End Get
        Set(ByVal Value As String)
            _territorysetid = Value
        End Set
    End Property


    Public Property Territory() As String
        Get
            Return _territory
        End Get
        Set(ByVal Value As String)
            _territory = Value
        End Set
    End Property

    Public Property RouteCost() As Double
        Get
            Return _routecost
        End Get
        Set(ByVal Value As Double)
            _routecost = Value
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

    Public Property TotalVolume() As Double
        Get
            Return _totalvolume
        End Get
        Set(ByVal Value As Double)
            _totalvolume = Value
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

    Public Property StartDate() As DateTime
        Get
            Return _startdate
        End Get
        Set(ByVal Value As DateTime)
            _startdate = Value
        End Set
    End Property

    Public Property EndDate() As DateTime
        Get
            Return _enddate
        End Get
        Set(ByVal Value As DateTime)
            _enddate = Value
        End Set
    End Property

    Public Property ActualStartDate() As DateTime
        Get
            Return _actualstartdate
        End Get
        Set(ByVal Value As DateTime)
            _actualstartdate = Value
        End Set
    End Property

    Public Property ActualEndDate() As DateTime
        Get
            Return _actualenddate
        End Get
        Set(ByVal Value As DateTime)
            _actualenddate = Value
        End Set
    End Property

    Public Property StrategyId() As String
        Get
            Return _strategyid
        End Get
        Set(ByVal Value As String)
            _strategyid = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public Property Stops() As RouteStopCollection
        Get
            Return _routestops
        End Get
        Set(ByVal Value As RouteStopCollection)
            _routestops = Value
        End Set
    End Property

    Public ReadOnly Property IsOpened() As Boolean
        Get
            If _status < RouteStatus.Loaded Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property ReasonUnFeasibility() As String
        Get
            Return _reasonunfeasibility
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _routestops = New RouteStopCollection(Me)
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)

    End Sub

    Public Sub New(ByVal RouteId As String)
        _routeid = RouteId
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)

        Load()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Dim uId As String = Common.GetCurrentUser
        Select Case CommandName.ToLower
            Case "confirm"
                For Each dr In ds.Tables(0).Rows
                    Try
                        Dim rt As New Route(dr("ROUTEID"))
                        rt.Confirm(uId)
                    Catch ex As Exception

                    End Try
                Next
        End Select
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal sRouteId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from route where routeid = {0}", Made4Net.Shared.Util.FormatField(sRouteId))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from route where routeid = {0}", Made4Net.Shared.Util.FormatField(_routeid))
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Not Found")
        End If

        dr = dt.Rows(0)
        _routeset = Convert.ReplaceDBNull(dr("routeset"))
        _runid = Convert.ReplaceDBNull(dr("runid"))
        _status = RouteStatusFromString(dr("status"))
        _oldstatus = _status
        If Not dr.IsNull("depo") Then _depo = dr("depo")
        _routename = Convert.ReplaceDBNull(dr("routename"))
        _routedate = Convert.ReplaceDBNull(dr("routedate"))
        If Not dr.IsNull("startpoint") Then _startpoint = dr("startpoint")
        If Not dr.IsNull("endpoint") Then _endpoint = dr("endpoint")
        If Not dr.IsNull("vehicleid") Then _vehicle = dr("vehicleid")
        _vehicletype = Convert.ReplaceDBNull(dr("vehicletype"))
        If Not dr.IsNull("driver") Then _driver = dr("driver")
        _territory = Convert.ReplaceDBNull(dr("territory"))
        If Not dr.IsNull("ROUTECOST") Then _routecost = dr("ROUTECOST")
        If Not dr.IsNull("TOTALDISTANCE") Then _totaldistance = dr("TOTALDISTANCE")
        If Not dr.IsNull("TOTALTIME") Then _totaltime = dr("TOTALTIME")
        If Not dr.IsNull("TOTALVOLUME") Then _totalvolume = dr("TOTALVOLUME")
        If Not dr.IsNull("TOTALWEIGHT") Then _totalweight = dr("TOTALWEIGHT")
        If Not dr.IsNull("STARTDATE") Then _startdate = dr("STARTDATE")
        If Not dr.IsNull("ENDDATE") Then _enddate = dr("ENDDATE")
        If Not dr.IsNull("ACTUALSTARTDATE") Then _actualstartdate = dr("ACTUALSTARTDATE")
        If Not dr.IsNull("ACTUALENDDATE") Then _actualenddate = dr("ACTUALENDDATE")
        If Not dr.IsNull("STRATEGYID") Then _strategyid = dr("STRATEGYID")

        If Not dr.IsNull("TripNum") Then _tripnum = dr("TripNum")
        If Not dr.IsNull("TripGroup") Then _tripgroup = dr("TripGroup")


        _adddate = Convert.ReplaceDBNull(dr("adddate"))
        _adduser = Convert.ReplaceDBNull(dr("adduser"))
        _editdate = Convert.ReplaceDBNull(dr("editdate"))
        _edituser = Convert.ReplaceDBNull(dr("edituser"))

        _routestops = New RouteStopCollection(Me)
    End Sub

#End Region

#Region "Save/Update/Create"

    Public Sub Create(ByVal pRouteId As String, ByVal pRouteSet As String, ByVal pDepo As String, _
                ByVal pRouteName As String, ByVal pRouteDate As DateTime, _
                ByVal pStartPoint As String, ByVal pEndPoint As String, _
                ByVal pVehicleId As String, ByVal pVehicleType As String, ByVal pDriver As String, _
                ByVal pTerritorySetID As String, ByVal pTerritoryID As String, _
                ByVal pRunId As String, ByVal pRouteCost As Double, _
                ByVal pStartTime As Integer, ByVal pEndTime As Integer, ByVal pNumDays As Integer, _
                ByVal pTotalDist As Double, ByVal pTotalTime As Double, _
                ByVal pTotalWeight As Double, ByVal pTotalVolume As Double, ByVal pStrategyId As String, _
                ByVal pUser As String, _
                Optional ByVal pTripNum As Integer = 0, _
                Optional ByVal pTripGroup As String = "")

        Dim sql As String
        If _routeid Is Nothing Or _routeid = String.Empty Then
            _routeid = Made4Net.Shared.Util.getNextCounter("ROUTE")
        End If
        _runid = pRunId
        _routeset = pRouteSet
        _routename = pRouteName
        _routedate = pRouteDate
        _depo = pDepo
        _startpoint = pStartPoint
        _endpoint = pEndPoint
        _vehicle = pVehicleId
        _vehicletype = pVehicleType
        _driver = pDriver
        _territory = pTerritoryID
        _territorysetid = pTerritorySetID

        _status = RouteStatus.[New]
        _totaldistance = pTotalDist
        _totaltime = pTotalTime
        _totalweight = pTotalWeight
        _totalvolume = pTotalVolume
        _strategyid = pStrategyId

        _tripnum = pTripNum
        _tripgroup = pTripGroup

        'If pTotalDist > 0 Or pTotalTime > 0 Then
        '    _routecost = CalculateRouteCost()
        'Else
        '    _routecost = pRouteCost
        'End If

        ''        Try
        Dim adddays As Integer = 0
        _startdate = RouteStop.getDateTime(pRouteDate, pStartTime, adddays)

        ''        Catch ex As Exception
        ''        End Try

        _enddate = DateAdd(DateInterval.Second, _totaltime, _startdate)

        If Not Exists(_routeid) Then
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser
            sql = String.Format("Insert into route(ROUTEID, STATUS, ROUTESET, DEPO, ROUTENAME, ROUTEDATE, STARTPOINT, ENDPOINT, VEHICLEID, VEHICLETYPE, " & _
            "DRIVER,TERRITORY, ADDDATE, ADDUSER, EDITDATE, EDITUSER,RUNID ,ROUTECOST ," & _
            "STARTDATE ,ENDDATE, " & _
            "TOTALDISTANCE, TOTALTIME, TOTALVOLUME, TOTALWEIGHT, STRATEGYID, TERRITORYSET,TRIPNUM,TRIPGROUP) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27})", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(RouteStatusToString(_status)), Made4Net.Shared.Util.FormatField(_routeset), Made4Net.Shared.Util.FormatField(Depo), Made4Net.Shared.Util.FormatField(_routename), Made4Net.Shared.Util.FormatField(_routedate), _
            Made4Net.Shared.Util.FormatField(_startpoint), Made4Net.Shared.Util.FormatField(_endpoint), Made4Net.Shared.Util.FormatField(_vehicle), Made4Net.Shared.Util.FormatField(_vehicletype), _
            Made4Net.Shared.Util.FormatField(_driver), _
            Made4Net.Shared.Util.FormatField(_territory), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_runid), _
            Made4Net.Shared.Util.FormatField(_routecost), Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_enddate), Made4Net.Shared.Util.FormatField(_totaldistance), Made4Net.Shared.Util.FormatField(_totaltime), Made4Net.Shared.Util.FormatField(_totalvolume), Made4Net.Shared.Util.FormatField(_totalweight), Made4Net.Shared.Util.FormatField(_strategyid), _
            Made4Net.Shared.Util.FormatField(_territorysetid), _
            Made4Net.Shared.Util.FormatField(_tripnum), _
            Made4Net.Shared.Util.FormatField(_tripgroup))
            _routestops = New RouteStopCollection(Me)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Route Already Exists", "Route Already Exists")
        End If
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Update(ByVal pDepo As String, ByVal pRouteName As String, ByVal pRouteDate As DateTime, ByVal pStatus As RouteStatus, _
                ByVal pStartPoint As String, ByVal pEndPoint As String, ByVal pVehicleId As String, ByVal pVehicleType As String, _
                ByVal pRouteCost As Double, _
                ByVal pStartDate As DateTime, ByVal pEndDate As DateTime, _
                ByVal pDriver As String, _
                ByVal pTerritorySet As String, ByVal pTerritory As String, ByVal pTotalDist As Double, ByVal pTotalTime As Double, ByVal pTotalWeight As Double, ByVal pTotalVolume As Double, ByVal pUser As String, _
                Optional ByVal pTripNum As Integer = 0, _
                Optional ByVal pTripGroup As String = "")
        Dim sql As String
        If Not Exists(_routeid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Route does not Exists", "Route does not Exists")
        End If
        _routename = pRouteName
        _routedate = pRouteDate
        _depo = pDepo
        _startpoint = pStartPoint
        _endpoint = pEndPoint
        _vehicle = pVehicleId
        _vehicletype = pVehicleType
        _driver = pDriver
        _territory = pTerritory
        _territorysetid = pTerritorySet
        _oldstatus = _status
        _status = pStatus
        _routecost = pRouteCost

        _startdate = pStartDate
        _enddate = pEndDate

        _totaldistance = pTotalDist
        _totaltime = pTotalTime
        _totalweight = pTotalWeight
        _totalvolume = pTotalVolume

        _tripnum = pTripNum
        _tripgroup = pTripGroup

        sql = String.Format("update route set ROUTEID={0}, STATUS={1}, ROUTESET={2}, DEPO={3}, ROUTENAME={4}, ROUTEDATE={5}, STARTPOINT={6}, ENDPOINT={7}, VEHICLEID={8}, VEHICLETYPE={9}, " & _
                    "DRIVER={10}, TERRITORYSET={24}, TERRITORY={11}, ADDDATE={12}, ADDUSER={13}, EDITDATE={14}, EDITUSER={15}, ROUTECOST={16}, STARTDATE={17},ENDDATE={18}, TOTALDISTANCE={19}, TOTALTIME={20}, TOTALVOLUME={21}, TOTALWEIGHT={22}, TRIPNUM={25}, TRIPGROUP ={26} Where {23}", _
                    Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(RouteStatusToString(_status)), Made4Net.Shared.Util.FormatField(_routeset), Made4Net.Shared.Util.FormatField(Depo), Made4Net.Shared.Util.FormatField(_routename), Made4Net.Shared.Util.FormatField(_routedate), _
                    Made4Net.Shared.Util.FormatField(_startpoint), Made4Net.Shared.Util.FormatField(_endpoint), Made4Net.Shared.Util.FormatField(_vehicle), Made4Net.Shared.Util.FormatField(_vehicletype), _
                    Made4Net.Shared.Util.FormatField(_driver), Made4Net.Shared.Util.FormatField(_territory), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                    Made4Net.Shared.Util.FormatField(_routecost), Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_enddate), Made4Net.Shared.Util.FormatField(_totaldistance), Made4Net.Shared.Util.FormatField(_totaltime), Made4Net.Shared.Util.FormatField(_totalvolume), Made4Net.Shared.Util.FormatField(_totalweight), WhereClause, _
                    Made4Net.Shared.Util.FormatField(_territorysetid), _
            Made4Net.Shared.Util.FormatField(_tripnum), _
            Made4Net.Shared.Util.FormatField(_tripgroup))
        DataInterface.RunSQL(sql)
        If _isstatusdirty Or _oldstatus <> _status Then
            RouteStatusChanged(New RouteStatusChangedEventArgs(_routeid, _oldstatus, _status))
            _oldstatus = _status
            _isstatusdirty = False
        End If
    End Sub


    Public Sub Save(ByVal sUserId As String)
        Dim sql As String
        If _routeid Is Nothing Or _routeid = String.Empty Then
            _routeid = Made4Net.Shared.Util.getNextCounter("ROUTE")
        End If
        If Not Exists(_routeid) Then
            If _runid = String.Empty Then
                _runid = Made4Net.Shared.Util.getNextCounter("RoutePlanRunId")
            End If
            _adddate = DateTime.Now
            _adduser = sUserId
            _editdate = DateTime.Now
            _edituser = sUserId
            sql = String.Format("Insert into route(ROUTEID, STATUS, ROUTESET, DEPO, ROUTENAME, ROUTEDATE, STARTPOINT, ENDPOINT, VEHICLEID, VEHICLETYPE, " & _
            "DRIVER, TERRITORY, ADDDATE, ADDUSER, EDITDATE, EDITUSER,RUNID, TERRITORYSET,TRIPNUM,TRIPGROUP) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19})", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(RouteStatusToString(_status)), Made4Net.Shared.Util.FormatField(_routeset), Made4Net.Shared.Util.FormatField(Depo), Made4Net.Shared.Util.FormatField(_routename), Made4Net.Shared.Util.FormatField(_routedate), _
            Made4Net.Shared.Util.FormatField(_startpoint), Made4Net.Shared.Util.FormatField(_endpoint), Made4Net.Shared.Util.FormatField(_vehicle), Made4Net.Shared.Util.FormatField(_vehicletype), _
            Made4Net.Shared.Util.FormatField(_driver), Made4Net.Shared.Util.FormatField(_territory), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_runid), _
            Made4Net.Shared.Util.FormatField(_territorysetid), _
            Made4Net.Shared.Util.FormatField(_tripnum), _
            Made4Net.Shared.Util.FormatField(_tripgroup))
            _routestops = New RouteStopCollection(Me)
        Else
            _editdate = DateTime.Now
            _edituser = sUserId
            sql = String.Format("update route set ROUTEID={0}, STATUS={1}, ROUTESET={2}, DEPO={3}, ROUTENAME={4}, ROUTEDATE={5}, STARTPOINT={6}, ENDPOINT={7}, VEHICLEID={8}, VEHICLETYPE={9}, " & _
            "DRIVER={10}, TERRITORYSET={17}, TERRITORY={11}, ADDDATE={12}, ADDUSER={13}, EDITDATE={14}, EDITUSER={15}, TRIPNUM={18}, TRIPGROUP ={19} Where {16}", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(RouteStatusToString(_status)), Made4Net.Shared.Util.FormatField(_routeset), Made4Net.Shared.Util.FormatField(Depo), Made4Net.Shared.Util.FormatField(_routename), Made4Net.Shared.Util.FormatField(_routedate), _
            Made4Net.Shared.Util.FormatField(_startpoint), Made4Net.Shared.Util.FormatField(_endpoint), Made4Net.Shared.Util.FormatField(_vehicle), Made4Net.Shared.Util.FormatField(_vehicletype), _
            Made4Net.Shared.Util.FormatField(_driver), Made4Net.Shared.Util.FormatField(_territory), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
            WhereClause, Made4Net.Shared.Util.FormatField(_territorysetid), _
            Made4Net.Shared.Util.FormatField(_tripnum), _
            Made4Net.Shared.Util.FormatField(_tripgroup))
        End If
        DataInterface.RunSQL(sql)
        If _isstatusdirty Or _oldstatus <> _status Then
            RouteStatusChanged(New RouteStatusChangedEventArgs(_routeid, _oldstatus, _status))
            _oldstatus = _status
            _isstatusdirty = False
        End If
    End Sub

#End Region

#Region "Add/Remove Stops"

    Public Sub UpdateRouteStopNumbers(ByVal pNewStopNumber As Int32, ByVal pActionType As String, ByVal pUser As String)
        Dim shouldUpdate As Boolean = False
        For Each oRouteStop As RouteStop In Me.Stops
            If oRouteStop.StopNumber = pNewStopNumber Then
                shouldUpdate = True
            End If
        Next
        If shouldUpdate Then
            If pActionType = ActionTypes.Increment Then
                Dim i As Int32
                For i = Me.Stops.Count - 1 To 0 Step -1
                    If Me.Stops(i).StopNumber >= pNewStopNumber Then
                        Me.Stops(i).UpdateStopNumber(Me.Stops(i).StopNumber + 1, pUser)
                    End If
                Next
            Else
                Dim i As Int32
                For i = 0 To Me.Stops.Count - 1
                    If Me.Stops(i).StopNumber >= pNewStopNumber Then
                        Me.Stops(i).UpdateStopNumber(Me.Stops(i).StopNumber - 1, pUser)
                    End If
                Next
            End If
        End If
    End Sub

    Public Function AddStop(ByVal pStopName As String, ByVal pPointId As String, ByVal pArrivalTime As Int32, _
    ByVal pDepartureTime As Int32, ByVal pUser As String, _
        Optional ByVal adddays As Integer = 0, _
        Optional ByVal pStopNumber As Int32 = -1) As RouteStop
        If _status = RouteStatus.Canceled Or Status = RouteStatus.Confirmed Then
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
        If pStopNumber <> -1 Then
            UpdateRouteStopNumbers(pStopNumber, ActionTypes.Increment, pUser)
        End If
        Dim oRouteStop As RouteStop
        If Me.Stops.StopExists(pPointId) Then
            oRouteStop = Me.Stops.GetStop(pPointId)
            oRouteStop.UpdateDepartureTime(pDepartureTime, pUser)

        Else
            oRouteStop = New RouteStop
            oRouteStop.Create(_routeid, pStopName, pPointId, pArrivalTime, pDepartureTime, pUser, _
                adddays, RouteDate, pStopNumber)
            Me.Stops.Add(oRouteStop)
        End If
        If _status = RouteStatus.[New] Then
            SetStatus(RouteStatus.Assigned, DateTime.Now, pUser)
        End If
        Return oRouteStop
    End Function

    Public Sub SetTotalVolume(ByVal pNewTotalVolume As Double)
        _totalvolume = pNewTotalVolume
        Dim SQL As String = String.Format("update route set TOTALVOLUME={0} Where {1} ", _
                    Made4Net.Shared.Util.FormatField(_totalvolume), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub


    Public Sub AddStop(ByVal oRouteStop As RouteStop)
        If _status = RouteStatus.Canceled Or Status = RouteStatus.Confirmed Then
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
        Me.Stops.Add(oRouteStop)
    End Sub

    ''main
    Public Sub addRouteChkPnt(Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing)
        Dim currentTotalTime = TotalTime
        Dim ChkPntHash As New Hashtable()
        For Each oRouteStop As RouteStop In Stops
            For Each oRouteStopTask As RouteStopTask In oRouteStop.RouteStopTask
                If oRouteStopTask.ChkPnt <> String.Empty Then
                    If Not ChkPntHash.ContainsKey(oRouteStopTask.ChkPnt) Then
                        ChkPntHash.Add(oRouteStopTask.ChkPnt, oRouteStopTask)
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Find Check Ppoint:" & oRouteStopTask.ChkPnt & " for documentid:" & oRouteStopTask.DocumentId & " stop numbber: " & oRouteStopTask.StopNumber)
                        End If
                    End If
                End If
            Next
        Next

        For Each Item As DictionaryEntry In ChkPntHash
            Dim oRouteStopTask As RouteStopTask = Item.Value()
            Dim pRouteStopTask As RouteStopTask = getRouteStopTask(oRouteStopTask.DocumentId, oRouteStopTask.StopTaskType)
            If Not oLogger Is Nothing Then
                oLogger.Write("Add Check Ppoint:" & pRouteStopTask.ChkPnt & " for documentid:" & pRouteStopTask.DocumentId & " stop numbber: " & pRouteStopTask.StopNumber)
            End If
            addChkPnt(pRouteStopTask, oLogger)
        Next


        Dim oStrat As RoutingStrategy = New RoutingStrategy(Me.StrategyId)
        Dim oStratDet As RoutingStrategyDetail = oStrat.StrategyDetails(0)

        Dim newStartDate As DateTime = StartDate.AddSeconds(currentTotalTime - TotalTime)
        Dim newStartime As Integer = DateTimetoIntTime(newStartDate)

        If newStartime > oStratDet.DepoStartTime And newStartime < oStratDet.DepoLatestStartTime Then
            StartDate = StartDate.AddSeconds(currentTotalTime - TotalTime)
        Else

            Dim difftime As Integer = RouteBaskets.DifTimeSec(newStartime, oStratDet.DepoStartTime)
            StartDate.AddSeconds(currentTotalTime - TotalTime + difftime)

        End If
        RecalculateArrivalTime(True)

    End Sub

    Protected Function getRouteStopTask(ByVal pDocumentID As String, _
        ByVal pStopTaskType As StopTaskType) As RouteStopTask
        For Each oRouteStop As RouteStop In Stops
            For Each oRouteStopTask As RouteStopTask In oRouteStop.RouteStopTask
                If oRouteStopTask.DocumentId = pDocumentID And oRouteStopTask.StopTaskType = pStopTaskType Then
                    Return oRouteStopTask
                End If
            Next
        Next
        Return Nothing
    End Function



    Public Function AssignStopDetailToRoute(ByVal pReq As RoutingRequirement, _
                Optional ByVal stopNum As Integer = 0) As String
        Dim oStopTaskType As StopTaskType
        Select Case pReq.PDType
            Case PDType.Delivery
                oStopTaskType = StopTaskType.Delivery
            Case PDType.Pickup
                oStopTaskType = StopTaskType.PickUp
            Case PDType.General
                oStopTaskType = StopTaskType.General
        End Select

        Dim oRouteStopTask As RouteStopTask
        oRouteStopTask = AddStopDetail(oStopTaskType, Me.StartDate, pReq.Consignee, pReq.OrderId, pReq.OrderType, _
            pReq.TargetCompany, pReq.CompanyType, pReq.ContactId, pReq.ChkPnt, "", 0, "", _
                pReq.OrderVolume, pReq.OrderWeight, _
            0, "", StopTaskConfirmationType.None, Common.GetCurrentUser, stopNum)
        Me.RefreshRouteData()


        ''
        If pReq.ChkPnt <> String.Empty Then
            addChkPnt(oRouteStopTask)
        End If


        ''check and update next routes in the same Trip Group
        Dim Message As String = String.Empty
        If Not WMS.Logic.RoutePlanner.checkAndUpdateTripGroupRoutes(Me, Nothing) Then
            Message &= "Impossible update trip group routes - feasibility reason."
        End If

        Return (Message)
    End Function

    Public Sub addChkPnt(ByVal pRouteStopTask As RouteStopTask, _
            Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing)

        Dim stopNum As Integer = pRouteStopTask.StopNumber

        Dim thebestpos As Integer = 0, adddistance As Double = 0, addtime As Double = 0

        Dim oChkPntContact As Contact = New Contact(pRouteStopTask.ChkPnt, True)
        If Me.StrategyId <> String.Empty Then
            Dim oStrat As RoutingStrategy = New RoutingStrategy(Me.StrategyId)
            Dim oStratDet As RoutingStrategyDetail = oStrat.StrategyDetails(0)

            Dim CHKPNTPOSITIONTYPE As String = oStrat.StrategyDetails(0).ChkPntPositionType
            Dim existsChkPntStopNumber As Integer = Me.getExistsChkPntStopNumber(oChkPntContact.POINTID)
            If Not oLogger Is Nothing Then
                oLogger.Write("CHKPNT POSITION TYPE: " & CHKPNTPOSITIONTYPE)
            End If

            If existsChkPntStopNumber = -1 Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Check point:" & pRouteStopTask.ChkPnt & " doesn't exists. Add..")
                End If

                If CHKPNTPOSITIONTYPE = 0 Then
                    thebestpos = 0
                ElseIf CHKPNTPOSITIONTYPE = "1" Then
                    thebestpos = stopNum - 1
                ElseIf CHKPNTPOSITIONTYPE = "2" Then
                    Dim bestdistance As Double = 0, besttime As Double = 0
                    Dim chkPntStop As GeoPointNode = New GeoPointNode(oChkPntContact.POINTID)
                    Me.findBestchkPntPos(chkPntStop, stopNum, thebestpos, bestdistance, besttime)
                End If
                ''If thebestpos = 0 Then thebestpos = 1
                Me.AddChkPntFromRequirementToRoute(pRouteStopTask, oChkPntContact, thebestpos, adddistance, addtime, oLogger)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Check point:" & pRouteStopTask.ChkPnt & " added pos:" & thebestpos)
                End If
                Me.RefreshRouteData()

            Else

                If CHKPNTPOSITIONTYPE = "1" Then
                    thebestpos = stopNum - 1
                    If thebestpos < existsChkPntStopNumber Then
                        Me.UpdateStopNumber(thebestpos, existsChkPntStopNumber)
                        Me.RefreshRouteData()
                    End If
                ElseIf CHKPNTPOSITIONTYPE = "2" Then
                    Dim bestdistance As Double = 0, besttime As Double = 0
                    Dim chkPntStop As GeoPointNode = New GeoPointNode(oChkPntContact.POINTID)
                    Me.findBestchkPntPos(chkPntStop, stopNum, thebestpos, bestdistance, besttime)
                    If thebestpos + 1 < existsChkPntStopNumber Then
                        Me.UpdateStopNumber(thebestpos + 1, existsChkPntStopNumber)
                        Me.RefreshRouteData()
                    End If
                End If

            End If

        End If

    End Sub


    Public Function checkAssignTasktoAnotherRoute(ByVal pOrderID As String) As Boolean
        Dim sql As String = String.Format("select count(*) from ROUTESTOPTASK where STATUS not in ('{1}','{2}') and DOCUMENTID='{0}' " & _
        " and routeid in (select routeid from route where runid in (select runid from route where routeid='{3}'))", _
            pOrderID, RouteStatus.Canceled, RouteStatus.Closed, Me.RouteId)
        Return DataInterface.ExecuteScalar(sql)
    End Function

    Public Function AddStopDetail(ByVal pStopDetailType As StopTaskType, ByVal pScheduleDate As DateTime, _
                ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderType As String, ByVal pCompany As String, ByVal pCompType As String, ByVal pContactId As String, _
                ByVal pChkPnt As String, _
                ByVal pPackType As String, ByVal pNumPacks As Int32, ByVal pTransClass As String, ByVal pVolume As Double, ByVal pWeight As Double, ByVal pValue As Double, _
                ByVal pComments As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pUserId As String, Optional ByVal pStopNumber As Int32 = -1, Optional ByVal pStopTaskId As Int32 = -1) As RouteStopTask

        If _status >= RouteStatus.Returned Then
            Throw New M4NException(New Exception, "Route status incorrect", "Route status incorrect")
        End If
        If _status = RouteStatus.Departed And pStopDetailType = StopTaskType.Delivery Then
            Throw New M4NException(New Exception, "Cannot add new delivery to a departed route", "Cannot add new delivery to a departed route")
        End If
        Dim oRouteStopDet As RouteStopTask
        Dim oRouteStop As RouteStop
        Dim oComp As Company
        Dim shipTo As String
        If pContactId = "" Then
            If Company.Exists(pConsignee, pCompany, pCompType) Then
                oComp = New Company(pConsignee, pCompany, pCompType)
                shipTo = oComp.DEFAULTCONTACTOBJ.CONTACTID
            End If
        Else
            shipTo = pContactId
        End If
        oRouteStop = GetRouteStopByStopDetail(shipTo, pConsignee, pCompany, pCompType, pUserId, pStopNumber)
        oRouteStopDet = oRouteStop.AddStopDetail(pStopDetailType, pScheduleDate, _
          pConsignee, pOrderid, pOrderType, pCompany, pCompType, shipTo, pChkPnt, pPackType, pNumPacks, pTransClass, pVolume, pWeight, pValue, pComments, pConfirmationType, pUserId, pStopTaskId)

        Dim sql As String = String.Format("select packageid from routepackages where  consignee={0} and documentid={1}", _
        Made4Net.Shared.FormatField(oRouteStopDet.Consignee), Made4Net.Shared.FormatField(oRouteStopDet.DocumentId))
        Dim dt As New DataTable()
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        For Each packageDR As DataRow In dt.Rows
            oRouteStopDet.AddStopTaskPackage(packageDR("PACKAGEID"), WMS.Logic.GetCurrentUser)
        Next

        Return oRouteStopDet
    End Function

    ''????
    Public Function AddChkPntFromRequirementToRoute(ByVal pRouteStopTask As RouteStopTask, _
            ByVal pChkPntContact As Contact, _
              ByVal theBestPos As Integer, _
              ByVal adddistance As Double, _
              ByVal addtime As Double, _
            Optional ByVal oLogger As LogHandler = Nothing) As Boolean


        '' add stop
        Dim NewStopNumber As Integer
        NewStopNumber = theBestPos + 1
        AddRouteChkPntStop(pRouteStopTask, pChkPntContact, 0, 0, NewStopNumber, oLogger)
        '' 22/11/2011 
        ''check and update next routes in the same Trip Group
        'If Not checkAndUpdateTripGroupRoutes(pRoute, oLogger) Then
        '    Return False
        'End If
        ''''''''''''''''''''
        Return True

    End Function
    Public Function getExistsChkPntStopNumber(ByVal pChkPntPointID As String) As Integer
        Dim SQL As String = String.Format("select isnull(min(stopnumber),-1) from ROUTESTOP " & _
                    " where pointid='{1}' and routeid='{0}' ", _
                    Me.RouteId, pChkPntPointID)

        Dim ExistsChkPntStopNumber As Integer = DataInterface.ExecuteScalar(SQL)
        Return ExistsChkPntStopNumber

    End Function

    Protected Sub AddRouteChkPntStop(ByVal pRouteStopTask As RouteStopTask, _
            ByVal pChkPntContact As Contact, _
            ByVal estArrvHour As Int32, ByVal estDepartureHour As Int32, _
            ByVal theBestPos As Integer, _
            Optional ByVal oLogger As LogHandler = Nothing)

        Dim oRouteStop As RouteStop
        If Not Stops.StopExists(pChkPntContact.POINTID) Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Add stop with Pointid:" & pChkPntContact.POINTID)
            End If
            oRouteStop = AddStop("", pChkPntContact.POINTID, estArrvHour, estDepartureHour, Common.GetCurrentUser(), 0, theBestPos)
        Else
            oRouteStop = Stops.GetStop(pChkPntContact.POINTID)
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("Add chkPntTask with documentid:" & pRouteStopTask.DocumentId)
        End If
        oRouteStop.AddStopDetail(StopTaskType.ChkPnt, RouteDate, pRouteStopTask.Consignee, pRouteStopTask.DocumentId, _
            pRouteStopTask.DocumentType, pRouteStopTask.Company, pRouteStopTask.CompanyType, _
            pRouteStopTask.ChkPnt, "", "", 0, pRouteStopTask.TransportationClass, _
            0, 0, 0, "", StopTaskConfirmationType.None, Common.GetCurrentUser())
    End Sub


    Public Function RemoveStopTask(ByVal pStopNumber As Int32, ByVal pStopTaskId As Int32, ByVal pUser As String)
        If _status >= RouteStatus.Departed Then
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
        Dim oRouteStopTask As New RouteStopTask(_routeid, pStopNumber, pStopTaskId)
        If oRouteStopTask.Status = StopTaskStatus.Incomplete Or oRouteStopTask.Status = StopTaskStatus.Completed Or _
                        oRouteStopTask.Status = StopTaskStatus.Canceled Then
            Throw New M4NException(New Exception, "Task Status Incorrect , Cannot Unassign Task", "Task Status Incorrect , Cannot Unassign Task")
        End If
        Dim oRouteStop As New RouteStop(oRouteStopTask.RouteID, oRouteStopTask.StopNumber)
        oRouteStop.RemoveStopTask(oRouteStopTask, pUser)
    End Function

    Public Function CancelStopTask(ByVal pStopNumber As Int32, ByVal pStopTaskId As Int32, ByVal pUser As String)
        Dim oRouteStopTask As New RouteStopTask(_routeid, pStopNumber, pStopTaskId)
        CancelStopTask(oRouteStopTask, pUser)
    End Function

    Public Function CancelStopTask(ByVal oStopTask As RouteStopTask, ByVal pUser As String)
        If _status = RouteStatus.Canceled Or Status = RouteStatus.Confirmed Then
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
        Dim oRouteStop As New RouteStop(oStopTask.RouteID, oStopTask.StopNumber)
        oRouteStop.CancelStopDetail(oStopTask, pUser)
        For Each tmpRouteStop As RouteStop In Me.Stops
            If tmpRouteStop.Status <> StopStatus.Canceled Then
                Exit Function
            End If
        Next
        SetStatus(RouteStatus.Canceled, DateTime.Now, pUser)
    End Function

    Public Function CancelStopDetail(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pCompany As String, ByVal pCompType As String, ByVal pUser As String)
        If _status = RouteStatus.Canceled Or Status = RouteStatus.Confirmed Then
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
        Dim oRouteStopDet As RouteStopTask
        Dim oRouteStop As RouteStop
        oRouteStop = GetRouteStopByStopDetail(pConsignee, pOrderId, pCompany, pCompType, pUser)
        oRouteStopDet = RouteStopTask.GetRouteStopTask(oRouteStop.RouteID, pConsignee, pOrderId)
        oRouteStop.CancelStopDetail(oRouteStopDet, pUser)
        Me.Stops = New RouteStopCollection(Me)
        For Each tmpRouteStop As RouteStop In Me.Stops
            If tmpRouteStop.Status <> StopStatus.Canceled Then
                Exit Function
            End If
        Next
        SetStatus(RouteStatus.Canceled, DateTime.Now, pUser)
    End Function

    Public Function GetRouteStopByStopDetail(ByVal pContactId As String, ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompType As String, ByVal pUser As String, Optional ByVal pStopNumber As Int32 = -1) As RouteStop
        Dim oOrd As OutboundOrderHeader
        Dim oComp As Company
        Dim shipTo As String
        If pContactId = "" Then
            If Company.Exists(pConsignee, pCompany, pCompType) Then
                oComp = New Company(pConsignee, pCompany, pCompType)
                For Each tmpRouteStop As RouteStop In Me.Stops
                    If tmpRouteStop.PointId = oComp.DEFAULTCONTACTOBJ.POINTID Then
                        Return tmpRouteStop
                    End If
                Next
                shipTo = oComp.DEFAULTCONTACTOBJ.POINTID
            End If
        Else
            Dim ocontact As WMS.Logic.Contact = New WMS.Logic.Contact(pContactId, True)
            shipTo = ocontact.POINTID
        End If
        Dim oRouteStop As RouteStop = AddStop("", shipTo, 0, 0, pUser, 0, pStopNumber)
        Return oRouteStop
    End Function

#End Region

#Region "Add/Remove Packages"

    'Public Function AddRouteStopTaskPackage(ByVal pPackageId As String, ByVal pUserId As String) As RouteStopTaskPackages
    '    Dim oRouteStopTask As WMS.Logic.RouteStopTask
    '    Dim oRouteStopTaskPackages As RouteStopTaskPackages
    '    Dim oRoutePackage As RoutePackage
    '    If Not RoutePackage.Exists(pPackageId) Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add package to route - package does not exists", "Cannot add package to route - package does not exists")
    '    End If
    '    oRoutePackage = New RoutePackage(pPackageId)
    '    Dim schedDate As DateTime
    '    Dim sCompany, sCompanyType, sNotes As String
    '    FillStopTaskProperties(oRoutePackage, sCompany, sCompanyType, sNotes, schedDate)
    '    oRouteStopTask = AddStopDetail(StopTaskType.Delivery, schedDate, oRoutePackage.Consignee, oRoutePackage.DocumentId, oRoutePackage.DocumentType, _
    '        sCompany, sCompanyType, "", 0, "", 0, 0, _
    '        0, sNotes, Common.GetCurrentUser)

    '    oRouteStopTaskPackages = oRouteStopTask.AddStopTaskPackage(pPackageId, pUserId)
    '    Return oRouteStopTaskPackages
    'End Function

    'Public Function RemoveRouteStopTaskPackage(ByVal pPackageId As String, ByVal pUserId As String) As RouteStopTaskPackages
    '    Dim oRoutePackage As RoutePackage
    '    Dim removed As Boolean = False
    '    If Not RoutePackage.Exists(pPackageId) Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot remove package from route - package does not exists", "Cannot remove package from route - package does not exists")
    '    Else
    '        oRoutePackage = New RoutePackage(pPackageId)
    '    End If
    '    Dim oRouteStop As RouteStop
    '    Dim stopNumber As Int32
    '    If RouteStop.ExistInRoute(_routeid, oRoutePackage.Contact.POINTID, stopNumber) Then
    '        oRouteStop = Me.Stops.GetStop(oRoutePackage.Contact.POINTID)
    '    Else
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot remove package from route - stop does not exists", "Cannot remove package from route - stop does not exists")
    '    End If
    '    'Find the stop task that the package is assigned to
    '    For Each stopTask As RouteStopTask In oRouteStop.RouteStopTask
    '        If stopTask.StopPackages.RouteStopPackageExists(pPackageId) Then
    '            stopTask.RemoveStopTaskPackage(pPackageId, pUserId)
    '            removed = True
    '        End If
    '    Next
    '    If Not removed Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot remove package from route - package does not exists", "Cannot remove package from route - package does not exists")
    '    End If
    'End Function

    'Private Sub FillStopTaskProperties(ByVal oRoutePackage As RoutePackage, ByRef sCompany As String, ByRef sCompanyType As String, ByRef sNotes As String, ByRef schedDate As DateTime)
    '    Select Case oRoutePackage.DocumentType
    '        Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
    '            sCompany = oRoutePackage.DocumentProperty("TARGETCOMPANY")
    '            sCompanyType = oRoutePackage.DocumentProperty("COMPANYTYPE")
    '            sNotes = oRoutePackage.DocumentProperty("NOTES")
    '            schedDate = oRoutePackage.DocumentProperty("SCHEDULEDDATE")
    '        Case WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
    '            sCompany = oRoutePackage.DocumentProperty("SOURCECOMPANY")
    '            sCompanyType = oRoutePackage.DocumentProperty("COMPANYTYPE")
    '            sNotes = oRoutePackage.DocumentProperty("NOTES")
    '            schedDate = oRoutePackage.DocumentProperty("EXPECTEDDATE")
    '        Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
    '            sCompany = oRoutePackage.DocumentProperty("SOURCECOMPANY")
    '            sCompanyType = oRoutePackage.DocumentProperty("SOURCECOMPANYTYPE")
    '            sNotes = oRoutePackage.DocumentProperty("NOTES")
    '            schedDate = oRoutePackage.DocumentProperty("SCHEDULEDARRIVALDATE")
    '        Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
    '            sCompany = oRoutePackage.DocumentProperty("SOURCECOMPANY")
    '            sCompanyType = oRoutePackage.DocumentProperty("SOURCECOMPANYTYPE")
    '            sNotes = oRoutePackage.DocumentProperty("NOTES")
    '            schedDate = oRoutePackage.DocumentProperty("SCHEDULEDARRIVALDATE")
    '    End Select
    'End Sub

#End Region

#Region "Add/Remove General Tasks"

    Public Sub AddGeneralTask(ByVal pTaskId As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pUserId As String)
        Dim oRouteStopTask As WMS.Logic.RouteStopTask
        Dim oRouteTask As RouteGeneralTask
        If Not RouteGeneralTask.Exists(pTaskId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add task to route - task does not exists", "Cannot add task to route - task does not exists")
        Else
            oRouteTask = New RouteGeneralTask(pTaskId)
        End If
        Dim oRouteStop As RouteStop
        Dim stopNumber As Int32
        If RouteStop.ExistInRoute(_routeid, oRouteTask.Contact.POINTID, stopNumber) Then
            oRouteStop = Me.Stops.GetStop(oRouteTask.Contact.POINTID)
        Else
            oRouteStop = AddStop("", oRouteTask.Contact.POINTID, 0, 0, pUserId, 0, -1)
        End If
        oRouteStop.AddGeneralTask(oRouteTask.TaskId, pConfirmationType, pUserId)
    End Sub

    Public Sub RemoveGeneralTask(ByVal pTaskId As String, ByVal pUserId As String)
        Dim oRouteTask As RouteGeneralTask
        If Not RouteGeneralTask.Exists(pTaskId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot remove task to route - task does not exists", "Cannot remove task to route - task does not exists")
        Else
            oRouteTask = New RouteGeneralTask(pTaskId)
        End If
        Dim oRouteStop As RouteStop
        Dim stopNumber As Int32
        If RouteStop.ExistInRoute(_routeid, oRouteTask.Contact.POINTID, stopNumber) Then
            oRouteStop = Me.Stops.GetStop(oRouteTask.Contact.POINTID)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot remove task to route - stop does not exists", "Cannot remove task to route - stop does not exists")
        End If
        oRouteStop.RemoveGeneralTask(oRouteTask.TaskId, pUserId)
    End Sub

#End Region

#Region "Route Activities"

    Public Sub Cancel(ByVal pUser As String)
        Dim sql As String
        If _status <> RouteStatus.Confirmed Then
            Dim oStop As RouteStop
            For Each oStop In Me.Stops
                oStop.Cancel(WMS.Lib.USERS.SYSTEMUSER)
            Next
            SetStatus(RouteStatus.Canceled, DateTime.Now, pUser)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Route Status", "Incorrect Route Status")
            Throw m4nEx
        End If
    End Sub

    Public Sub Confirm(ByVal pUser As String)
        Dim sql As String
        If _status <> RouteStatus.Canceled Then
            Dim oStop As RouteStop
            For Each oStop In Me.Stops
                oStop.Confirm(WMS.Lib.USERS.SYSTEMUSER)
            Next
            SetStatus(RouteStatus.Confirmed, DateTime.Now, pUser)

            Dim aq As EventManagerQ = New EventManagerQ
            Dim EventType As Int32
            EventType = WMS.Logic.WMSEvents.EventType.RouteConfirmed

            Dim actType As String = WMS.Logic.TMSEvents.EventToString(WMS.Logic.TMSEvents.Event.RouteStatusChanged)
            aq.Add("EVENT", EventType.ToString())
            aq.Add("ACTION", "RTSETSTAT")
            aq.Add("ACTIVITYTYPE", actType)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _routeid)
            aq.Add("DOCUMENTLINE", "")
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _oldstatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(actType)


        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Route Status", "Incorrect Route Status")
            Throw m4nEx
        End If
    End Sub

    Public Sub Depart(ByVal pUser As String, ByVal pActivityDate As DateTime)
        If _status = RouteStatus.Loaded Then
            SetStatus(RouteStatus.Departed, pActivityDate, pUser)
            SetActualStartDate(pActivityDate, pUser)
        ElseIf Status < RouteStatus.Departed Then
            For Each oStop As RouteStop In Me.Stops
                For Each oStopTask As RouteStopTask In oStop.RouteStopTask
                    If Not oStopTask.Loaded Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Depart Route - not all packages were loaded", "Cannot Depart Route - not all packages were loaded")
                    End If
                Next
            Next
            SetStatus(RouteStatus.Departed, pActivityDate, pUser)
            SetActualStartDate(pActivityDate, pUser)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Route Status", "Incorrect Route Status")
            Throw m4nEx
        End If

        Dim aq As EventManagerQ = New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.RouteDeparted
        Dim actType As String = WMS.Logic.TMSEvents.EventToString(WMS.Logic.WMSEvents.EventType.RouteDeparted)
        aq.Add("EVENT", EventType)
        aq.Add("ACTIVITYTYPE", actType)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(pActivityDate))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _routeid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _oldstatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send("RouteDeparted")
    End Sub

    Public Sub ReturnRoute(ByVal pUser As String, ByVal pActivityDate As DateTime)
        'If _status <> RouteStatus.Canceled AndAlso _status <> RouteStatus.Returned Then
        If _status = RouteStatus.Departed Then
            PrepareRouteTasksForReturn()
            SetStatus(RouteStatus.Returned, pActivityDate, pUser)
            SetActualEndTime(pActivityDate, pUser)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Route Status", "Incorrect Route Status")
            Throw m4nEx
        End If
    End Sub
    Public Sub CloseRoute(ByVal pUser As String, ByVal pActivityDate As DateTime)
        'If _status <> RouteStatus.Canceled AndAlso _status <> RouteStatus.Returned Then
        If _status = RouteStatus.Returned Then
            'PrepareRouteTasksForReturn()
            SetStatus(RouteStatus.Closed, pActivityDate, pUser)
            'SetActualEndTime(pActivityDate, pUser)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Route Status", "Incorrect Route Status")
            Throw m4nEx
        End If
    End Sub


    Public Sub Load(ByVal pUser As String, ByVal pActivityDate As DateTime)
        If _status < RouteStatus.Loaded Then
            For Each oStop As RouteStop In Me.Stops
                For Each oStopTask As RouteStopTask In oStop.RouteStopTask
                    If Not oStopTask.Loaded Then
                        'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot set route loaded - not all packages were loaded", "Cannot set route loaded - not all packages were loaded")
                        For Each rstpack As RouteStopTaskPackages In oStopTask.StopPackages
                            rstpack.Package.Load(pUser)
                        Next
                    End If
                Next
            Next
            SetStatus(RouteStatus.Loaded, pActivityDate, pUser)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Route Status", "Incorrect Route Status")
            Throw m4nEx
        End If
    End Sub

    Public Sub SetActualStartDate(ByVal pActivityDate As DateTime, ByVal pUserId As String)
        _actualstartdate = pActivityDate

        _editdate = DateTime.Now
        _edituser = pUserId
        Dim SQL As String = String.Format("Update route set ACTUALSTARTDATE={0}, EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_actualstartdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub SetActualEndTime(ByVal pActivityDate As DateTime, ByVal pUserId As String)
        _actualenddate = pActivityDate
        _editdate = DateTime.Now
        _edituser = pUserId
        Dim SQL As String = String.Format("Update route set ACTUALENDDATE={0}, EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_actualenddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub
    Private Sub PrepareRouteTasksForReturn()
        For Each _rs As RouteStop In Me.Stops
            For Each _rst As RouteStopTask In _rs.RouteStopTask
                If _rst.Status <> StopTaskStatus.Completed And _rst.Status <> StopTaskStatus.Canceled Then
                    _rst.InComplete(_rst.ReasonCode, "System")
                End If
            Next
        Next
    End Sub
#End Region

#Region "Statuses"

    Public Sub SetStatus(ByVal pNewStatus As RouteStatus, ByVal pActivityDate As DateTime, ByVal pUserId As String)
        _oldstatus = _status
        _status = pNewStatus
        _editdate = DateTime.Now
        _edituser = pUserId
        Dim SQL As String = String.Format("Update route set STATUS={0}, EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(RouteStatusToString(_status)), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim aq As EventManagerQ = New EventManagerQ
        Dim EventType As Int32
        EventType = WMS.Logic.WMSEvents.EventType.RouteStatusChanged

        Dim actType As String = WMS.Logic.TMSEvents.EventToString(WMS.Logic.TMSEvents.Event.RouteStatusChanged)
        aq.Add("EVENT", EventType.ToString())
        aq.Add("ACTION", "RTSETSTAT")
        aq.Add("ACTIVITYTYPE", actType)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(pActivityDate))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _routeid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _oldstatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUserId)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUserId)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUserId)
        aq.Send(actType)
    End Sub

#End Region

#Region "Route Cost"

    Private Sub InitRouteCostCalcParameters()
        RouteCostCalcParameters.Clear()
        RouteCostCalcParameters.Add("VehType", "0")
        RouteCostCalcParameters.Add("DefCostPerDay", "0")
        RouteCostCalcParameters.Add("DefCostPerHour", "0")
        RouteCostCalcParameters.Add("DefCostPerDistUnit", "0")
        RouteCostCalcParameters.Add("TotalDist", "0")
        RouteCostCalcParameters.Add("TotalDrvTime", "0")
        RouteCostCalcParameters.Add("VehCostPerDay", "0")
        RouteCostCalcParameters.Add("VehCostPerHour", "0")
        RouteCostCalcParameters.Add("VehCostPerDistanceUnit", "0")
        RouteCostCalcParameters.Add("RouteCost", "0")

        RouteCostCalcParameters.Add("TotalStopNumber", getTotalStopNumber())
        RouteCostCalcParameters.Add("TotalTaskNumber", getTotalTaskNumber())
        RouteCostCalcParameters.Add("RouteStartDayofWeek", StartDate.DayOfWeek.ToString)
        RouteCostCalcParameters.Add("RouteEndtDayofWeek", EndDate.DayOfWeek.ToString)


    End Sub
    Public Function getTotalTaskNumber() As Integer
        Dim cntTasks As Integer = 0
        For Each oRSt As RouteStop In Me.Stops
            cntTasks += oRSt.RouteStopTask.Count
        Next
        Return cntTasks
    End Function

    Public Function getTotalStopNumber() As Integer
        Return Me.Stops.Count
    End Function

    Private Function CalcRouteCostEquation(ByVal sourceEquation As String, _
                                Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Double
        Dim targetEquation As String
        Dim res As Double
        Try
            Dim UseEvalLogs As String = Made4Net.Shared.AppConfig.Get("UseEvalLogs", String.Empty)
            Dim dirpath As String
            If UseEvalLogs = "1" Then
                dirpath = Made4Net.Shared.AppConfig.Get("ServiceEvalLogDirectory", Nothing)
            End If

            Dim oEvalEquation As EvalEquation = New EvalEquation(Nothing, RouteCostCalcParameters, dirpath)
            res = oEvalEquation.EvalEquation(sourceEquation, targetEquation)
            Return res
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("CalcServEquation Error:" & ex.Message)
            End If
            Return 0D
        Finally
            ''return +
            'If Not oLogger Is Nothing Then
            '    oLogger.Write("Source Equation:" & sourceEquation & "  Target Equation Equation: " & targetEquation & "  Result: Equation: " & res.ToString())
            'End If
        End Try
    End Function

    Public Function CalculateRouteCost() As Decimal
        Try
            Dim oStrat As RoutingStrategy = New RoutingStrategy(_strategyid)
            Dim oStratDet As RoutingStrategyDetail = oStrat.StrategyDetails(0)
            Dim oVehicle As WMS.Logic.VehicleType
            If _vehicletype = "" Or _vehicletype Is Nothing Then
                oVehicle = oStratDet.DefaultVehicle
            Else
                oVehicle = New VehicleType(_vehicletype)
            End If

            InitRouteCostCalcParameters()

            RouteCostCalcParameters("VehType") = System.Convert.ToInt16(_vehicletype Is Nothing Or _vehicletype = "")

            RouteCostCalcParameters("DefCostPerDay") = oStrat.DefCostPerDay.ToString()
            RouteCostCalcParameters("DefCostPerHour") = oStrat.DefCostPerHour.ToString()
            RouteCostCalcParameters("DefCostPerDistUnit") = oStrat.DefCostPerDistUnit.ToString()

            RouteCostCalcParameters("VehCostPerDay") = oVehicle.COSTPERDAY.ToString()
            RouteCostCalcParameters("VehCostPerHour") = oVehicle.COSTPERHOUR.ToString()
            RouteCostCalcParameters("VehCostPerDistanceUnit") = oVehicle.COSTPERDISTANCEUINIT.ToString()

            RouteCostCalcParameters("TotalDist") = TotalDistance.ToString()
            RouteCostCalcParameters("TotalDrvTime") = TotalTime.ToString()

            Dim RouteCost As Double = CalcRouteCostEquation(oStratDet.RouteCostEquation)
            Return RouteCost

            ' Should calculate the following Sigma: 
            ' Vehicle cost per day + totaltime*vehicle cost per hour + totaldistance*vehicle cost per KM
            'Dim costperday, costperhour, costperdist As Decimal
            'costperday = oVehicle.COSTPERDAY
            'costperdist = oVehicle.COSTPERDISTANCEUINIT * pTotalDist / 1000
            'Dim hr, min As Int32
            'hr = pTotalTime / 60
            'min = pTotalTime Mod 60
            'costperhour = oVehicle.COSTPERHOUR * hr + (min / 60) * oVehicle.COSTPERHOUR
            'Return costperday + costperdist + costperhour
        Catch ex As Exception
            Return -1
        End Try
    End Function


    Public Function SetRouteParams() As Boolean
        Dim Routecost As Double = 0D
        Dim stopDist As Double = 0
        Dim stopDrivingTime As Double = 0
        Dim oStrat As RoutingStrategy = New RoutingStrategy(_strategyid)
        Dim oStratDet As RoutingStrategyDetail = oStrat.StrategyDetails(0)

        TotalDistance = 0
        TotalTime = 0
        TotalWeight = 0
        TotalVolume = 0
        If oStratDet.CalcRetTimeToFirstStop AndAlso Stops.Count > 0 Then
            GetCost(oStratDet.Depot.POINTID, Stops(0).PointId, stopDist, stopDrivingTime)
            TotalDistance += stopDist
            TotalTime += stopDrivingTime
        End If

        Dim prevRouteStop As RouteStop
        Dim numStop As Integer = 1
        InitServTimeCalcParameters(oStratDet)
        For Each oStop As RouteStop In Stops

            If oStop.StopNumber > 1 And numStop > 1 Then
                stopDist = 0D
                stopDrivingTime = 0D
                GetCost(oStop.PointId, prevRouteStop.PointId, stopDist, stopDrivingTime)
                TotalDistance += stopDist
                TotalTime += stopDrivingTime
            End If

            Dim numTask As Integer = 1
            For Each oStopTask As RouteStopTask In oStop.RouteStopTask
                ''servicetime add
                Dim maxServiceTime As Integer = 0

                SetRouteReqServTimeCalcParameters(oStratDet, oStopTask, (numTask <> 1))
                maxServiceTime = CalcServTimeEquation(oStratDet.ServicetimeEquation)
                TotalTime += maxServiceTime

                TotalWeight += oStopTask.StopDetailWeight
                TotalVolume += oStopTask.StopDetailVolume
                numTask += 1
            Next
            prevRouteStop = oStop
            numStop += 1
        Next

        If oStratDet.CalcRetTimeToDepot AndAlso Stops.Count > 0 Then
            stopDist = 0D
            stopDrivingTime = 0D
            GetCost(oStratDet.Depot.POINTID, Stops(Stops.Count - 1).PointId, stopDist, stopDrivingTime)
            TotalDistance += stopDist
            TotalTime += stopDrivingTime
        End If
        ''''''''''''''''

        SetTotalDistance(TotalDistance)
        SetTotalTime(TotalTime)
        SetTotalWeight(TotalWeight)
        SetTotalVolume(TotalVolume)

        Routecost = CalculateRouteCost()
        SetRouteCost(Routecost)

        Return True
    End Function

    Public Function RemoveStop(ByVal StopNumber As Integer)
        Dim oStop As New WMS.Logic.RouteStop(Me, StopNumber)

        Dim i As Integer = 1
        For Each oStopTask As RouteStopTask In oStop.RouteStopTask
            ''RemoveStopTask(StopNumber, i, WMS.Logic.GetCurrentUser)
            CompleteDeleteRouteStopTask(StopNumber, i)
            i += 1
        Next

        oStop.Delete()
        Dim sql As String
        sql = String.Format("update routestop set stopnumber=stopnumber-1 where stopnumber>{0} and routeid='{1}'", _
            StopNumber, RouteId)
        DataInterface.RunSQL(sql)
        sql = String.Format("update routestoptask set stopnumber=stopnumber-1 where stopnumber>{0} and routeid='{1}'", _
            StopNumber, RouteId)
        DataInterface.RunSQL(sql)


    End Function

    Public Function ResequenceRoute(Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Double
        Try

            DeleteChkPnts()

            Dim sourceArrayList As ArrayList = New ArrayList()
            Dim targetArrayList As ArrayList = New ArrayList()
            Dim Distcost As Double = 0D
            Dim Timecost As Double = 0D
            Dim Routecost As Double = 0D

            Dim oStrat As RoutingStrategy = New RoutingStrategy(_strategyid)
            Dim oStratDet As RoutingStrategyDetail = oStrat.StrategyDetails(0)


            For Each oRouteStop As RouteStop In _routestops
                sourceArrayList.Add(oRouteStop.PointId.ToString())
            Next

            Dim startDepot, returnDepot As String
            If oStratDet.CalcRetTimeToFirstStop Then startDepot = oStratDet.Depot.POINTID
            If oStratDet.CalcRetTimeToDepot Then returnDepot = oStratDet.Depot.POINTID

            Dim oResequenceRouting As ResequenceRouting = New ResequenceRouting(sourceArrayList, startDepot, returnDepot, oLogger)
            targetArrayList = oResequenceRouting.runResequenceRoutes()
            Distcost = oResequenceRouting.DistCost
            Timecost = oResequenceRouting.TimeCost


            SetTotalDistance(Distcost)
            Routecost = CalculateRouteCost()

            SetRouteCost(Routecost)
            SetStatus(RouteStatus.Assigned, DateTime.Now, "RePlan")

            Dim targetHash As Hashtable = New Hashtable()
            Dim stopnumber As Integer = 1
            For Each gp As String In targetArrayList
                If Not targetHash.Contains(gp) Then
                    targetHash.Add(gp, stopnumber)
                    stopnumber += 1
                End If
            Next

            For Each oRouteStop As RouteStop In _routestops
                oRouteStop.UpdateStopNumber(oRouteStop.StopNumber * 1000, Common.GetCurrentUser())
            Next
            For Each oRouteStop As RouteStop In _routestops
                Dim newStopnumber As Integer = targetHash(oRouteStop.PointId)
                oRouteStop.UpdateStopNumber(newStopnumber, Common.GetCurrentUser())
            Next

            '''?? check for update
            ''Load()
            ''RecalculateArrivalTime(True)
            Me.RefreshRouteData()

            Me.addRouteChkPnt()


            Return Distcost

        Catch ex As Exception
            Return 0D
        End Try

    End Function

    Private Sub addServTimeCalcParameters(ByVal key As String, ByVal value As String)
        If Not ServTimeCalcParameters.Contains(key) Then
            ServTimeCalcParameters.Add(key, value)
        Else
            ServTimeCalcParameters(key) = value
        End If
    End Sub

    Private Sub InitServTimeCalcParameters(ByVal oStratDet As RoutingStrategyDetail)
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

    Private Sub SetRouteReqServTimeCalcParameters(ByVal stratDet As RoutingStrategyDetail, _
                    ByVal oStopDetail As RouteStopTask, ByVal isSamePoint As Boolean)

        ''ContactData
        Dim ContactData As New ArrayList
        If _StopContactData.Contains(oStopDetail.Contact.POINTID & "#" & oStopDetail.Contact.CONTACTID) Then
            ContactData = _StopContactData(oStopDetail.Contact.POINTID & "#" & oStopDetail.Contact.CONTACTID)
            ContactData(0) = ContactData(0) + 1
            ContactData(1) = oStopDetail.Contact.FixedServiceTime.ToString
            ContactData(2) = ContactData(2) + oStopDetail.Contact.FixedServiceTime
            ContactData(3) = ContactData(3) + oStopDetail.StopDetailVolume
            ContactData(4) = ContactData(4) + oStopDetail.StopDetailWeight
            _StopContactData(oStopDetail.Contact.POINTID & "#" & oStopDetail.Contact.CONTACTID) = ContactData
        Else
            ContactData.Add(1) ''1?
            ContactData.Add(oStopDetail.Contact.FixedServiceTime)
            ContactData.Add(0)
            ContactData.Add(0)
            ContactData.Add(0)
            _StopContactData.Add(oStopDetail.Contact.POINTID & "#" & oStopDetail.Contact.CONTACTID, ContactData)
        End If

        addServTimeCalcParameters("ContactTasks", ContactData(0).ToString)
        addServTimeCalcParameters("ContactFixedServiceTime", ContactData(1).ToString)
        addServTimeCalcParameters("ContactSumFixedServiceTime", ContactData(2).ToString())
        addServTimeCalcParameters("ContactTotalVolume", ContactData(3).ToString())
        addServTimeCalcParameters("ContactTotalWeight", ContactData(4).ToString())


        ''StopData
        If Not isSamePoint Then
            _StopData.Clear()
        End If

        If _StopData.Count > 0 Then
            addServTimeCalcParameters("TotalTasks", (_StopData(0) + 1).ToString())
            addServTimeCalcParameters("StopTotalVolume", (_StopData(1) + oStopDetail.StopDetailVolume).ToString())
            addServTimeCalcParameters("StopTotalWeight", (_StopData(2) + oStopDetail.StopDetailWeight).ToString())
            ''max fixed serv time
            ''avg fixed servicetime
            _StopData(0) = _StopData(0) + 1
            _StopData(1) = _StopData(1) + oStopDetail.StopDetailVolume
            _StopData(2) = _StopData(2) + oStopDetail.StopDetailWeight
        Else
            addServTimeCalcParameters("TotalTasks", "1")
            addServTimeCalcParameters("StopTotalVolume", oStopDetail.StopDetailVolume.ToString())
            addServTimeCalcParameters("StopTotalWeight", oStopDetail.StopDetailWeight.ToString())
            _StopData.Add(1)
            _StopData.Add(oStopDetail.StopDetailVolume)
            _StopData.Add(oStopDetail.StopDetailWeight)
            ''max fixed serv time
            ''avg fixed servicetime

        End If


        Dim oPDType As String
        Select Case oStopDetail.StopTaskType
            Case StopTaskType.Delivery
                oPDType = PDType.Delivery
            Case StopTaskType.PickUp
                oPDType = PDType.Pickup
            Case StopTaskType.General
                oPDType = PDType.General
        End Select

        ''general and task data
        addServTimeCalcParameters("StrategyServiceTime", stratDet.MinServiceTime.ToString())
        addServTimeCalcParameters("TaskVolume", oStopDetail.StopDetailVolume.ToString())
        addServTimeCalcParameters("TaskWeight", oStopDetail.StopDetailWeight.ToString())
        addServTimeCalcParameters("TaskType", oPDType)
        addServTimeCalcParameters("TaskValue", oStopDetail.StopDetailValue.ToString())

        addServTimeCalcParameters("VehicleType", Me.VehicleType)

        ''???
        'addServTimeCalcParameters("UnloadingType", Req.UnloadingType)
        'addServTimeCalcParameters("Carrier", Req.Carrier)

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
    Public Sub RefreshRouteData()
        _routestops = New RouteStopCollection(Me)
        RecalculateArrivalTime(True)
        Dim msg As String
        Dim isFeasibile As Boolean = CheckFeasibility(msg, Nothing)
        SetTotalDistance(TotalDistance)
        SetTotalTime(TotalTime)
        SetTotalVolume(TotalVolume)
        SetTotalWeight(TotalWeight)
        Dim Routecost As Decimal = CalculateRouteCost()
        SetRouteCost(Routecost)
    End Sub

    Public Sub UpdateStopNumber(ByVal newStopNumber As Integer, ByVal oldStopNumber As Integer)
        Dim whc As String = String.Format(" routeid = {0} and stopnumber = {1} ", _
            Made4Net.Shared.Util.FormatField(RouteId), _
            Made4Net.Shared.Util.FormatField(newStopNumber))
        Dim SQL As String = String.Format("select count(*) from routestop where {0} ", whc)
        Dim isExistsNewStop As Integer = DataInterface.ExecuteScalar(SQL)
        Dim tempStopNumber As Int32

        If isExistsNewStop > 0 Then
            tempStopNumber = (New Random).Next(9000000, 9999999)
            Dim oNewRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(RouteId, newStopNumber)
            oNewRouteStop.UpdateStopNumber(tempStopNumber, Common.GetCurrentUser)
        End If

        Dim oRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(RouteId, oldStopNumber)
        oRouteStop.UpdateStopNumber(newStopNumber, Common.GetCurrentUser)

        If isExistsNewStop > 0 Then
            Dim otempRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(RouteId, tempStopNumber)
            otempRouteStop.UpdateStopNumber(oldStopNumber, Common.GetCurrentUser)
        End If
    End Sub

    Public Sub CompleteDeleteRouteStopTask(ByVal pStopNumber As Integer, _
                    ByVal pStopTaskID As Integer)
        If Me.Status = RouteStatus.Confirmed Then
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
            Exit Sub
        End If

        Dim oRouteStop As New WMS.Logic.RouteStop(Me.RouteId, pStopNumber)
        If oRouteStop.Status = StopStatus.Completed Then
            Throw New M4NException(New Exception, "Route Stop Status Incorrect", "Route Stop Status Incorrect")
            Exit Sub
        End If

        Dim oRouteStopTask As New WMS.Logic.RouteStopTask(Me.RouteId, pStopNumber, pStopTaskID)
        If oRouteStopTask.Status = StopTaskStatus.Completed Then
            Throw New M4NException(New Exception, "Route Stop Task Status Incorrect", "Route Stop Task Status Incorrect")
            Exit Sub
        End If

        Dim sql As String = String.Format("delete routestoptask " & _
                " where routeid='{0}' and stopnumber='{1}'  and stoptaskid='{2}'", Me.RouteId, pStopNumber, pStopTaskID)
        DataInterface.RunSQL(sql)

        sql = String.Format("delete routestop" & _
                " from routestop rs" & _
                " where routeid='{0}'  and stopnumber='{1}' and " & _
                " (select count(*) from routestoptask rst " & _
                " where rst.routeid=rs.routeid and rst.stopnumber=rs.stopnumber)" & _
                " =0", Me.RouteId, pStopNumber)
        DataInterface.RunSQL(sql)
        Me.RefreshRouteData()

        If oRouteStopTask.ChkPnt <> String.Empty Then
            If getCntChkPnt(oRouteStopTask.ChkPnt) = 0 Then
                Dim oContact As New Contact(oRouteStopTask.ChkPnt, True)
                Dim chkPntStop As WMS.Logic.RouteStop = Me.Stops.GetStop(oContact.POINTID)

                sql = String.Format("delete routestoptask " & _
                              " where routeid='{0}' and stopnumber='{1}' ", _
                                Me.RouteId, chkPntStop.StopNumber)
                DataInterface.RunSQL(sql)

                sql = String.Format("delete routestop" & _
                        " from routestop rs" & _
                        " where routeid='{0}'  and stopnumber='{1}' ", _
                        Me.RouteId, chkPntStop.StopNumber)
                DataInterface.RunSQL(sql)

                Me.RefreshRouteData()
            End If
        End If
    End Sub

    Public Sub DeleteChkPnts()
        Dim sql As String
        sql = String.Format("delete routestop " & _
                        " where stopnumber in " & _
                        " (select stopnumber from routestoptask " & _
                        " where routeid='{0}'  and stoptasktype='ChkPnt') ", _
                        Me.RouteId)
        DataInterface.RunSQL(sql)

        sql = String.Format("delete routestoptask " & _
                      " where routeid='{0}' and stoptasktype='ChkPnt'", _
                        Me.RouteId)
        DataInterface.RunSQL(sql)
        Me.RefreshRouteData()

    End Sub

    Public Function getCntChkPnt(ByVal chkPnt As String) As Integer
        Dim sql As String = String.Format("select count(*) from routestoptask " & _
            " where chkpnt='{0}' and routeid='{1}'", _
        chkPnt, Me.RouteId)
        Return CInt(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub RecalculateArrivalTime(Optional ByVal isupdate As Boolean = True)
        Dim oStrategyDetail As RoutingStrategyDetailCollection = _
            New RoutingStrategy(_strategyid).StrategyDetails
        LoadDistanceCache(oStrategyDetail)

        Dim STARTTIMEDEPO As Integer '' = Integer.Parse(oStrategyDetail(0).DepoStartTime)
        STARTTIMEDEPO = DateTimetoIntTime(StartDate)


        'Dim STARTTIMEDEPO As Integer
        'If StartDate.ToString = "01/01/0001 00:00:00" Then
        '    STARTTIMEDEPO = Integer.Parse(oStrategyDetail(0).DepoStartTime)
        'Else
        '    STARTTIMEDEPO = DateTimetoIntTime(StartDate)
        'End If

        Dim TotalArrivalTime As Integer = 0
        Dim TotalDepartureTime As Integer = 0
        Dim PrevPointID As String = String.Empty
        Dim isSamePoint As Boolean = False

        Dim totalstopservicetime As Double = 0

        TotalTime = 0D

        InitServTimeCalcParameters(oStrategyDetail(0))
        Dim allAddDays, AddDays, alladdDepDays, addDepDays As Integer
        For j As Integer = 0 To _routestops.Count - 1
            Dim oRouteStop As RouteStop = _routestops(j)

            totalstopservicetime = 0
            isSamePoint = (PrevPointID = oRouteStop.PointId)
            For Each oStopDetail As RouteStopTask In oRouteStop.RouteStopTask
                Dim maxServiceTime As Integer = 0

                SetRouteReqServTimeCalcParameters(oStrategyDetail(0), oStopDetail, isSamePoint)
                maxServiceTime = CalcServTimeEquation(oStrategyDetail(0).ServicetimeEquation, Nothing)
                totalstopservicetime += maxServiceTime
                isSamePoint = True
            Next



            PrevPointID = oRouteStop.PointId

            Dim DrivingTimefromDepo As Double = 0D
            Dim DrivingTimefromPrev As Double = 0D
            If oRouteStop.StopNumber = 1 Or j = 0 Then

                If oStrategyDetail(0).CalcRetTimeToFirstStop Then
                    If Not oStrategyDetail(0).Depot.POINTID Is Nothing AndAlso oStrategyDetail(0).Depot.POINTID <> "" Then
                        Dim DepoStop As GeoPointNode = New GeoPointNode(oStrategyDetail(0).Depot.POINTID)
                        If Not DepoStop Is Nothing Then
                            DrivingTimefromDepo = GetCost(DepoStop.POINTID, oRouteStop.PointId)
                            TotalTime += DrivingTimefromDepo
                        End If
                    End If
                End If
                TotalArrivalTime = CalcEstimateArrivalTime(DrivingTimefromDepo, STARTTIMEDEPO, AddDays)
                allAddDays += AddDays
            Else
                Dim prevRouteStop As RouteStop = _routestops(j - 1)
                DrivingTimefromPrev = GetCost(prevRouteStop.PointId, oRouteStop.PointId)
                TotalTime += DrivingTimefromPrev
                TotalArrivalTime = CalcEstimateArrivalTime(DrivingTimefromPrev, TotalDepartureTime, AddDays)
                allAddDays += AddDays
            End If

            TotalDepartureTime = CalcEstimateArrivalTime(totalstopservicetime, TotalArrivalTime, addDepDays)  ''SERVICETIME
            alladdDepDays += addDepDays
            TotalTime += totalstopservicetime
            oRouteStop.ArrivalTime = TotalArrivalTime
            oRouteStop.DepartureTime = TotalDepartureTime
            oRouteStop.AddDays = allAddDays

            If isupdate Then
                oRouteStop.Update(oRouteStop.StopName, TotalArrivalTime, TotalDepartureTime, Common.GetCurrentUser(), _
                    allAddDays + alladdDepDays, RouteDate)
            End If
        Next

        Dim DrivingTimetoDepo As Double
        If oStrategyDetail(0).CalcRetTimeToDepot Then
            If Not oStrategyDetail(0).Depot.POINTID Is Nothing AndAlso oStrategyDetail(0).Depot.POINTID <> "" Then
                Dim DepoStop As GeoPointNode = New GeoPointNode(oStrategyDetail(0).Depot.POINTID)
                If Not DepoStop Is Nothing Then
                    Dim oRouteStop As RouteStop = _routestops(_routestops.Count - 1)
                    DrivingTimetoDepo = GetCost(oRouteStop.PointId, DepoStop.POINTID)
                    TotalTime += DrivingTimetoDepo
                End If
            End If
        End If

        SetTotalTime(TotalTime)

        EndDate = StartDate.AddSeconds(TotalTime)
        If isupdate Then
            Dim sql As String = String.Format("update route " & _
                    " set ENDDATE=dateadd(ss," & TotalTime.ToString() & ",STARTDATE) " & _
                    " where routeid='{0}' ", RouteId)
            DataInterface.RunSQL(sql)
        End If


    End Sub

    Private Function CalcEstimateArrivalTime(ByVal tempDrivingTime As Double, ByVal pStartTime As Int32, _
                          ByRef AddDays As Integer) As Int32
        Try
            Dim itotalTime As Double = Math.Round(System.Convert.ToDouble(tempDrivingTime)) ''+ _totaltime
            itotalTime = Math.Round(itotalTime, 3)
            Dim sec, res As Integer
            sec = itotalTime + Math.Floor(pStartTime / 100) * 3600 + Math.Floor(pStartTime Mod 100) * 60
            res = Math.Floor(sec / 3600) * 100 + Math.Floor((sec Mod 3600) / 60)
            AddDays = Route.get24AddDays(res)
            res = Route.get24Hour(res)

            Return res

        Catch ex As Exception
            Return -1
        End Try

    End Function

    Public Shared Function get24Hour(ByVal wholehours As Integer) As Integer
        Dim res As Integer
        If wholehours < 0 Then
            res = 2400 + (wholehours \ 100) * 100 + 60 + (wholehours Mod 100)
        Else
            res = (wholehours + 2400 * 10) Mod 2400
        End If
        Return res
    End Function
    Public Shared Function get24AddDays(ByVal wholehours As Integer) As Integer
        Dim res As Integer
        If wholehours < 0 Then
            res = wholehours \ 2400 - 1
        Else
            res = wholehours \ 2400
        End If
        Return res
    End Function

    Public Sub LoadDistanceCache(ByVal oStrategyDetail As RoutingStrategyDetailCollection)
        Dim WhereClause As String = String.Empty
        For Each oRouteStop As RouteStop In _routestops
            WhereClause &= ",'" & oRouteStop.PointId.ToString() & "'"
        Next
        If Not oStrategyDetail(0).Depot.POINTID Is Nothing AndAlso oStrategyDetail(0).Depot.POINTID <> "" Then
            WhereClause &= ",'" & oStrategyDetail(0).Depot.POINTID.ToString() & "'"
        End If

        If WhereClause <> "" Then WhereClause = WhereClause.Substring(1, WhereClause.Length - 1)

        Dim DistSQL As String = String.Format("select * from  MAPNETWORKDISTANCES where SOURCE in ({0}) and  DESTINATION in ({0})", _
        WhereClause)
        _DistMatrix = GeoNetworkItem.GetDistancesCache(DistSQL)

    End Sub

    Private Function GetCost(ByVal FromPoint As Int32, _
                             ByVal ToPoint As Int32, _
                             Optional ByRef tempDist As Double = 0D, _
                             Optional ByRef tmpDrivingTime As Double = 0D) As Double
        Try
            If FromPoint = ToPoint Then Return 0

            Dim Source As GeoPointNode = New GeoPointNode(FromPoint)
            Dim Target As GeoPointNode = New GeoPointNode(ToPoint)
            If Not (Source Is Nothing) And Not (Target Is Nothing) Then

                GeoNetworkItem.GetDistance(Source, Target, tempDist, tmpDrivingTime, "CheckFeasibility", _isDistanceAdd, _
                    _isdistcost, _distcosttype)

            End If
            Return tmpDrivingTime
        Catch ex As Exception
            Return 0D
        End Try
    End Function


    Public Sub SetTotalDistance(ByVal pNewTotalDistance As Double)
        _totaldistance = pNewTotalDistance
        Dim SQL As String = String.Format("update route set TOTALDISTANCE={0} Where {1} ", _
                    Made4Net.Shared.Util.FormatField(_totaldistance), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub
    Public Sub SetStartDate(ByVal pStartDate As DateTime)
        _startdate = pStartDate
        Dim SQL As String = String.Format("update route set STARTDATE={0} Where {1} ", _
                    Made4Net.Shared.Util.FormatField(_startdate), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub


    Public Sub SetTotalTime(ByVal pNewTotalTime As Double)
        _totaltime = pNewTotalTime
        Dim SQL As String = String.Format("update route set TOTALTIME={0} Where {1} ", _
                    Made4Net.Shared.Util.FormatField(_totaltime), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub SetTotalWeight(ByVal pNewTotalWeight As Double)
        _totalweight = pNewTotalWeight
        Dim SQL As String = String.Format("update route set TOTALWEIGHT={0} Where {1} ", _
                    Made4Net.Shared.Util.FormatField(_totalweight), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub SetRouteCost(ByVal pSetRouteCost As Double)
        _routecost = pSetRouteCost
        Dim SQL As String = String.Format("update route set ROUTECOST={0} Where {1} ", _
                    Made4Net.Shared.Util.FormatField(_routecost), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub


#End Region

#Region "Reports"

    Public Function PrintRouteDriverManifest(ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "DriverManifest"
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "DriverManifest", "Copies"), dt, False, "Made4NetSchema")
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "DriverManifest")
        oQsender.Add("DATASETID", "repDriverManifest")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            'try to get the User Default Printer
            Try
                oQsender.Add("PRINTER", "")
            Catch ex As Exception
                oQsender.Add("PRINTER", "")
            End Try
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.SysParam.Get("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("ROUTEID = '{0}'", _routeid))
        oQsender.Send("Report", repType)
    End Function

    Public Shared Function PrintDriversDeliverySheet(ByVal pRouteID As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType, loadIds As String
        Dim i As Int32
        Dim dt As New DataTable
        repType = "DriversDeliverySheet"
        '-----------------------------------------------------------------------
        'DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "PutAwayWorkSheet", "Copies"), dt, False, "Made4NetSchema")
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        oQsender.Add("DATASETID", "repDriversDeliverySheet")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", WMS.Logic.GetCurrentUser)
        oQsender.Add("LANGUAGE", Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", 1) 'dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.SysParam.Get("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("routeid = {0}", Made4Net.Shared.Util.FormatField(pRouteID)))
        oQsender.Send("Report", repType)
    End Function

#End Region

#Region "Enum Conversion"

    Protected Function RouteStatusToString(ByVal rs As RouteStatus) As String
        Select Case rs
            Case RouteStatus.None
                Return "None"
            Case RouteStatus.Assigned
                Return "Assigned"
            Case RouteStatus.Canceled
                Return "Canceled"
            Case RouteStatus.Confirmed
                Return "Confirmed"
            Case RouteStatus.[New]
                Return "New"
            Case RouteStatus.Planned
                Return "Planned"
            Case RouteStatus.Planning
                Return "Planning"
            Case RouteStatus.Departed
                Return "Departed"
            Case RouteStatus.Returned
                Return "Returned"
            Case RouteStatus.Loaded
                Return "Loaded"
            Case RouteStatus.Closed
                Return "Closed"
        End Select
    End Function

    Protected Shared Function RouteStatusFromString(ByVal sRouteStatus As String) As RouteStatus
        Select Case sRouteStatus.ToLower
            Case "none"
                Return RouteStatus.None
            Case "assigned"
                Return RouteStatus.Assigned
            Case "canceled"
                Return RouteStatus.Canceled
            Case "confirmed"
                Return RouteStatus.Confirmed
            Case "new"
                Return RouteStatus.[New]
            Case "planned"
                Return RouteStatus.Planned
            Case "planning"
                Return RouteStatus.Planning
            Case "returned"
                Return RouteStatus.Returned
            Case "completed"
                Return RouteStatus.Returned
            Case "departed"
                Return RouteStatus.Departed
            Case "loaded"
                Return RouteStatus.Loaded
            Case "closed"
                Return RouteStatus.Closed
        End Select
    End Function

    Protected Sub RouteStatusChanged(ByVal e As RouteStatusChangedEventArgs)
        Dim evnt As New EventManagerQ
        evnt.Add("ROUTEID", e.RouteId)
        evnt.Add("FROMSTATUS", RouteStatusToString(e.FromStatus))
        evnt.Add("TOSTATUS", RouteStatusToString(e.ToStatus))
        evnt.Add("TIMESTAMP", e.TimeStamp)
        evnt.Send(TMSEvents.[Event].RouteStatusChanged)
    End Sub

#End Region
#Region "Shared"
    Public Shared Function GetRouteStatus(ByVal RouteID As String) As RouteStatus
        Try
            Return Route.RouteStatusFromString(System.Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT isnull(STATUS,'None') FROM ROUTE WHERE ROUTEID='" & RouteID & "'")))
        Catch ex As Exception
            Return Route.RouteStatusFromString("None")
        End Try
    End Function

#End Region
#Region "ActionType"

    <CLSCompliant(False)> Public Class ActionTypes
        Public Const Increment As String = "AddStop"
        Public Const Decrement As String = "RemoveStop"
    End Class

#End Region



#End Region

#Region "Route Feasibility"
    Public Function CheckFeasibility(ByRef msg As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim isFeasibily As Boolean = True
        Try
            Dim oStrategyDetail As RoutingStrategyDetailCollection = _
                New RoutingStrategy(_strategyid).StrategyDetails

            LoadDistanceCache(oStrategyDetail)

            Dim oStratDet As RoutingStrategyDetail = CType(oStrategyDetail(0), RoutingStrategyDetail)

            Dim oRouteCounters As New RouteCounters

            TotalWeight = 0D
            TotalVolume = 0D

            Dim currentWeight As Double = 0D
            Dim currentVolume As Double = 0D


            TotalDistance = 0D
            TotalTime = 0D
            Dim stopDrivingTime As Double = 0D
            Dim stopDist As Double = 0D

            Dim defaultOpenHour As Integer = Made4Net.Shared.SysParam.Get("RoutingDefOpenHours")
            Dim defaultCloseHour As Integer = Made4Net.Shared.SysParam.Get("RoutingDefCloseHours")

            Dim sVehicleType As String = VehicleType
            If sVehicleType = String.Empty Then sVehicleType = oStratDet.DefaultVehicleType
            Dim oVehicleType As VehicleType = New VehicleType(sVehicleType, True)

            Dim weightLimit As Double = oVehicleType.TOTALWEIGHT
            Dim volumeLimit As Double = oVehicleType.TOTALVOLUME

            InitServTimeCalcParameters(oStrategyDetail(0))


            Dim SB As New StringBuilder()

            If oStratDet.CalcRetTimeToFirstStop AndAlso Stops.Count > 0 Then
                GetCost(oStratDet.Depot.POINTID, Stops(0).PointId, stopDist, stopDrivingTime)
                TotalDistance += stopDist
                TotalTime += stopDrivingTime
            End If


            If Stops.Count > oStratDet.MaxStopsPerRoute Then
                SB.AppendLine("Max Stops (" & Stops.Count.ToString _
                  & ">" & oStratDet.MaxStopsPerRoute & ") Per Route Exceeded")
            End If


            Dim prevRouteStop As RouteStop
            Dim numStop As Integer = 1
            For Each oStop As RouteStop In Stops
                If oStratDet.AllowedOverWeightforLastStop <> 0 AndAlso (numStop = Stops.Count + 1) Then
                    weightLimit = oVehicleType.TOTALWEIGHT * oStratDet.AllowedOverWeightforLastStop
                    volumeLimit = oVehicleType.TOTALVOLUME * oStratDet.AllowedOverVolumeforLastStop
                End If

                If oStop.StopNumber > 1 And numStop > 1 Then
                    stopDist = 0D
                    stopDrivingTime = 0D
                    GetCost(oStop.PointId, prevRouteStop.PointId, stopDist, stopDrivingTime)
                    TotalDistance += stopDist
                    TotalTime += stopDrivingTime

                    If oStratDet.MaxTimeBetweenStops < stopDrivingTime Then
                        SB.AppendLine("Max Time Between Stops Exceeded:" & prevRouteStop.PointId & _
                        "-" & oStop.PointId & " " & oStratDet.MaxTimeBetweenStops.ToString() & _
                        "<" & stopDrivingTime.ToString())
                    End If

                    If oStratDet.MaxDistanceBetweenStops < stopDist Then
                        SB.AppendLine("Max Distance Between Stops Exceeded:" & prevRouteStop.PointId & _
                        "-" & oStop.PointId & " " & oStratDet.MaxDistanceBetweenStops.ToString() & _
                        "<" & stopDist.ToString())
                    End If


                End If

                Dim numTask As Integer = 1
                For Each oStopTask As RouteStopTask In oStop.RouteStopTask
                    ''servicetime add
                    Dim maxServiceTime As Integer = 0

                    SetRouteReqServTimeCalcParameters(oStratDet, oStopTask, (numTask <> 1))
                    maxServiceTime = CalcServTimeEquation(oStratDet.ServicetimeEquation, oLogger)
                    TotalTime += maxServiceTime


                    ''weight volume check
                    currentWeight = oRouteCounters.TotalPickupWeightCheck + oStopTask.StopDetailWeight - oRouteCounters.TotalDeliveryWeightCheck
                    currentVolume = oRouteCounters.TotalPickupVolumeCheck + oStopTask.StopDetailVolume - oRouteCounters.TotalDeliveryVolumeCheck

                    TotalWeight += oStopTask.StopDetailWeight
                    TotalVolume += oStopTask.StopDetailVolume

                    If oStopTask.StopTaskType = StopTaskType.Delivery Then
                        oRouteCounters.TotalDeliveryWeightCheck += oStopTask.StopDetailWeight
                        oRouteCounters.TotalDeliveryVolumeCheck += oStopTask.StopDetailVolume
                    ElseIf oStopTask.StopTaskType = StopTaskType.PickUp Then
                        oRouteCounters.TotalPickupWeightCheck += oStopTask.StopDetailWeight
                        oRouteCounters.TotalPickupVolumeCheck += oStopTask.StopDetailVolume
                    End If

                    oRouteCounters.TotalWeightCheck += oStopTask.StopDetailWeight
                    oRouteCounters.TotalVolumeCheck += oStopTask.StopDetailVolume

                    If currentWeight > weightLimit Or _
                             oRouteCounters.TotalDeliveryWeightCheck > weightLimit Or _
                             oRouteCounters.TotalPickupWeightCheck > weightLimit Then
                        SB.AppendLine("Vehicle Weight exceeded: num task#" & numTask.ToString & _
                                " num stop#" & numStop.ToString & " " & _
                                " Total Delivery Weight:" & oRouteCounters.TotalDeliveryWeightCheck.ToString() & " " & _
                                " Total Pickup Weight:" & oRouteCounters.TotalDeliveryWeightCheck.ToString() & " " & _
                            currentWeight.ToString() & ">" & weightLimit.ToString())
                    End If
                    If currentVolume > volumeLimit Or _
                             oRouteCounters.TotalDeliveryVolumeCheck > volumeLimit Or _
                             oRouteCounters.TotalPickupVolumeCheck > volumeLimit Then
                        SB.AppendLine("Vehicle Volume exceeded: num task#" & numTask.ToString & _
                                " num stop#" & numStop.ToString & " " & _
                                " Total Delivery Volume:" & oRouteCounters.TotalDeliveryVolumeCheck.ToString() & " " & _
                                " Total Pickup Volume:" & oRouteCounters.TotalDeliveryVolumeCheck.ToString() & " " & _
                            currentVolume.ToString() & ">" & volumeLimit.ToString())
                    End If

                    ''weight volume check for class transportation check
                    If oStopTask.TransportationClass <> String.Empty Then
                        Dim oTransportationClass As TransportationClass = _
                            oVehicleType.VehicleTypeTransportationClass.TransportationClassCollection(oStopTask.TransportationClass)
                        If Not oTransportationClass Is Nothing Then
                            Dim weightLimitTransClass As Double = oTransportationClass.MaxTotalWeight
                            Dim volumeLimitTransClass As Double = oTransportationClass.MaxTotalVolume

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

                            If Not oRouteCounters.TotalTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) Then
                                oRouteCounters.TotalTranClassVolumeCheckHash.Add(oTransportationClass.TransportationClass, 0)
                            End If
                            If Not oRouteCounters.TotalPickupTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) Then
                                oRouteCounters.TotalPickupTranClassVolumeCheckHash.Add(oTransportationClass.TransportationClass, 0)
                            End If
                            If Not oRouteCounters.TotalDeliveryTranClassVolumeCheckHash.Contains(oTransportationClass.TransportationClass) Then
                                oRouteCounters.TotalDeliveryTranClassVolumeCheckHash.Add(oTransportationClass.TransportationClass, 0)
                            End If


                            If oStopTask.StopTaskType = StopTaskType.Delivery Then
                                oRouteCounters.TotalDeliveryTranClassWeightCheckHash(oTransportationClass.TransportationClass) += oStopTask.StopDetailWeight
                                oRouteCounters.TotalDeliveryTranClassVolumeCheckHash(oTransportationClass.TransportationClass) += oStopTask.StopDetailVolume
                            ElseIf oStopTask.StopTaskType = StopTaskType.PickUp Then
                                oRouteCounters.TotalPickupTranClassWeightCheckHash(oTransportationClass.TransportationClass) += oStopTask.StopDetailWeight
                                oRouteCounters.TotalPickupTranClassVolumeCheckHash(oTransportationClass.TransportationClass) += oStopTask.StopDetailVolume
                            End If
                            oRouteCounters.TotalPickupTranClassWeightCheckHash(oTransportationClass.TransportationClass) += oStopTask.StopDetailWeight
                            oRouteCounters.TotalPickupTranClassVolumeCheckHash(oTransportationClass.TransportationClass) += oStopTask.StopDetailVolume


                            If oRouteCounters.TotalPickupTranClassWeightCheckHash(oTransportationClass.TransportationClass) > oTransportationClass.MaxTotalWeight Or _
                                     oRouteCounters.TotalDeliveryTranClassWeightCheckHash(oTransportationClass.TransportationClass) > oTransportationClass.MaxTotalWeight Or _
                                     oRouteCounters.TotalPickupTranClassWeightCheckHash(oTransportationClass.TransportationClass) > oTransportationClass.MaxTotalWeight Then
                                SB.AppendLine("Transportation class " & oTransportationClass.TransportationClass & " Vehicle Weight exceeded: num task#" & numTask.ToString & _
                                        " num stop#" & numStop.ToString & " " & _
                                        " Total Delivery Weight:" & oRouteCounters.TotalDeliveryTranClassWeightCheckHash(oTransportationClass.TransportationClass) & " " & _
                                        " Total Pickup Weight:" & oRouteCounters.TotalPickupTranClassWeightCheckHash(oTransportationClass.TransportationClass) & " " & _
                                    oRouteCounters.TotalPickupTranClassWeightCheckHash(oTransportationClass.TransportationClass).ToString() & ">" & oTransportationClass.MaxTotalWeight.ToString())
                            End If

                            If oRouteCounters.TotalPickupTranClassVolumeCheckHash(oTransportationClass.TransportationClass) > oTransportationClass.MaxTotalVolume Or _
                                     oRouteCounters.TotalDeliveryTranClassVolumeCheckHash(oTransportationClass.TransportationClass) > oTransportationClass.MaxTotalVolume Or _
                                     oRouteCounters.TotalPickupTranClassVolumeCheckHash(oTransportationClass.TransportationClass) > oTransportationClass.MaxTotalVolume Then
                                SB.AppendLine("Transportation class " & oTransportationClass.TransportationClass & " Vehicle Volume exceeded: num task#" & numTask.ToString & _
                                        " num stop#" & numStop.ToString & " " & _
                                        " Total Delivery Volume:" & oRouteCounters.TotalDeliveryTranClassVolumeCheckHash(oTransportationClass.TransportationClass) & " " & _
                                        " Total Pickup Volume:" & oRouteCounters.TotalPickupTranClassVolumeCheckHash(oTransportationClass.TransportationClass) & " " & _
                                    oRouteCounters.TotalPickupTranClassVolumeCheckHash(oTransportationClass.TransportationClass).ToString() & ">" & oTransportationClass.MaxTotalVolume.ToString())
                            End If
                        End If
                    End If

                    ''''''''''''

                    Dim oContact As Contact = oStopTask.Contact
                    Dim OpenHour As Integer = 0
                    Dim CloseHour As Integer = 0

                    Dim ArrTime As Integer = DateTimetoIntTime(oStop.ArrivalDate)
                    If Not oContact.CanDeliver(oStop.ArrivalDate, ArrTime, defaultOpenHour, defaultCloseHour, oStratDet.AllowedTimeBeforeOpen, OpenHour, CloseHour) Then
                        SB.AppendLine("Could not Place Requirement - Company Open Hours does not match.")
                        SB.AppendLine("Estimated Arrival Hour: " & ArrTime)
                        SB.AppendLine("Open Hour: " & OpenHour)
                        SB.AppendLine("Close Hour: " & CloseHour)
                        SB.AppendLine("Day Of Week:" & oStop.ArrivalDate.DayOfWeek)
                        SB.AppendLine("Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
                    End If

                    numTask += 1
                Next
                prevRouteStop = oStop
                numStop += 1
            Next

            If oStratDet.CalcRetTimeToDepot AndAlso Stops.Count > 0 Then
                stopDist = 0D
                stopDrivingTime = 0D
                GetCost(oStratDet.Depot.POINTID, Stops(Stops.Count - 1).PointId, stopDist, stopDrivingTime)
                TotalDistance += stopDist
                TotalTime += stopDrivingTime
            End If

            If oStratDet.MaxStopsPerRoute < Stops.Count Then
                SB.AppendLine("Max Number Stops per Route Exceeded:" & oStratDet.MaxStopsPerRoute.ToString() & _
                    "<" & Stops.Count.ToString())
            End If
            If oStratDet.MaxTimePerRoute < TotalTime Then
                SB.AppendLine("Max Route Time Exceeded:" & oStratDet.MaxTimePerRoute.ToString() & _
                    "<" & TotalTime.ToString())
            End If
            If oStratDet.MaxDistancePerRoute + oStratDet.MaxAdditionalDistancePerRoute < TotalDistance Then
                SB.AppendLine("Max Route Distance Exceeded:" & oStratDet.MaxDistancePerRoute.ToString() & _
                    "<" & TotalDistance.ToString())
            End If


            _reasonunfeasibility = SB.ToString()
            If _reasonunfeasibility.Length > 0 Then isFeasibily = False

            If Not oLogger Is Nothing Then
                oLogger.Write(_reasonunfeasibility)
            End If
            msg = _reasonunfeasibility
            If msg <> String.Empty Then
                Dim SQL As String = String.Format("update [route] set FEASIBILITY={0} where routeid={1}", _
                        FormatField(msg), FormatField(RouteId))
                DataInterface.RunSQL(SQL)
            End If

            Return isFeasibily

        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("CheckFeasibility Error: " & ex.ToString())
            End If
            msg = ex.ToString()
            Return False

        End Try


    End Function

    'Public Function getMinPosforChkPnt(ByVal pChkPnt As String) As Integer
    '    Dim SQL As String = String.Format("SELECT isnull(min(stopnumber),9999) FROM ROUTESTOP WHERE routeid='{0}' and POINTID in " & _
    '                        "(select  distinct pointid FROM ROUTINGREQUIREMENTS where chkpnt='1000' and routingset='{1}') ", _
    '                        Me.RouteId, Me.RouteSet, pChkPnt)

    '    Dim minstopnumber As Integer = DataInterface.ExecuteScalar(SQL)
    '    Return minstopnumber
    'End Function


    Public Function CheckOpenHoursFeasibility(ByRef msg As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim isFeasibily As Boolean = True
        Try
            ''RecalculateArrivalTimeEquation()
            Dim oStrategyDetail As RoutingStrategyDetailCollection = _
                New RoutingStrategy(_strategyid).StrategyDetails

            LoadDistanceCache(oStrategyDetail)
            Dim oStratDet As RoutingStrategyDetail = CType(oStrategyDetail(0), RoutingStrategyDetail)

            Dim defaultOpenHour As Integer = Made4Net.Shared.SysParam.Get("RoutingDefOpenHours")
            Dim defaultCloseHour As Integer = Made4Net.Shared.SysParam.Get("RoutingDefCloseHours")

            Dim SB As New StringBuilder()
            Dim prevRouteStop As RouteStop
            For Each oStop As RouteStop In Stops
                For Each oStopTask As RouteStopTask In oStop.RouteStopTask
                    Dim oContact As Contact = oStopTask.Contact
                    Dim OpenHour As Integer = 0
                    Dim CloseHour As Integer = 0

                    Dim ArrTime As Integer = DateTimetoIntTime(oStop.ArrivalDate)
                    If Not oContact.CanDeliver(oStop.ArrivalDate, ArrTime, defaultOpenHour, defaultCloseHour, oStratDet.AllowedTimeBeforeOpen, OpenHour, CloseHour) Then
                        SB.AppendLine("Could not Place Requirement - Company Open Hours does not match.")
                        SB.AppendLine("Estimated Arrival Hour: " & ArrTime)
                        SB.AppendLine("Open Hour: " & OpenHour)
                        SB.AppendLine("Close Hour: " & CloseHour)
                        SB.AppendLine("Day Of Week:" & oStop.ArrivalDate.DayOfWeek)
                        SB.AppendLine("Allowed TimeBefore Open: " & oStratDet.AllowedTimeBeforeOpen)
                    End If

                Next
                prevRouteStop = oStop
            Next

            _reasonunfeasibility = SB.ToString()
            If _reasonunfeasibility.Length > 0 Then isFeasibily = False
            If Not oLogger Is Nothing Then
                oLogger.Write(_reasonunfeasibility)
            End If
            msg = _reasonunfeasibility
            Return isFeasibily

        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("CheckFeasibility Error: " & ex.ToString())
            End If
            msg = ex.ToString()
            Return False

        End Try


    End Function

    Public Shared Function DateTimetoIntTime(ByVal dt As DateTime) As Integer
        Dim res As Integer
        Try
            Return Integer.Parse(Format(dt.Hour, "00").ToString + Format(dt.Minute, "00").ToString)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Sub MoveStop(ByVal StopNumber As Integer, ByVal TargetRouteID As String, _
                    Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing)
        Dim oSourceStop As New WMS.Logic.RouteStop(Me, StopNumber)
        Dim oTargetRoute As New Route(TargetRouteID)

        Dim oTargetRouteStop As RouteStop = oTargetRoute.AddStop("", oSourceStop.PointId, 0, 0, _
            WMS.Logic.GetCurrentUser, 0)

        Dim i As Integer = 1
        For Each oSourcetStopTask As RouteStopTask In oSourceStop.RouteStopTask
            oTargetRouteStop.AddStopDetail(oSourcetStopTask.StopTaskType, oTargetRoute.RouteDate, _
                oSourcetStopTask.Consignee, oSourcetStopTask.DocumentId, _
                        oSourcetStopTask.DocumentType, oSourcetStopTask.Company, oSourcetStopTask.CompanyType, _
                            oSourcetStopTask.ContactId, oSourcetStopTask.ChkPnt, _
                            "", 0, oSourcetStopTask.TransportationClass, _
                            oSourcetStopTask.StopDetailVolume, oSourcetStopTask.StopDetailWeight, oSourcetStopTask.StopDetailValue, _
                            "Moved from route:" & Me.RouteId, StopTaskConfirmationType.None, WMS.Logic.GetCurrentUser)
            i += 1
        Next

        oTargetRoute = New Route(TargetRouteID)

        oTargetRoute.ResequenceRoute(oLogger)

        oTargetRoute.RefreshRouteData()


        ''add checkpoints for all task

    End Sub


    Public Sub PlaceRequirement(ByVal oReq As RoutingRequirement, _
                    ByVal TargetRouteID As String, _
                    Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing)
        Dim oTargetRoute As New Route(TargetRouteID)

        Dim oTargetRouteStop As RouteStop = oTargetRoute.AddStop("", oReq.PointID, 0, 0, _
            WMS.Logic.GetCurrentUser, 0)

        Dim oStopTaskType As StopTaskType
        Select Case oReq.PDType
            Case PDType.Delivery
                oStopTaskType = StopTaskType.Delivery
            Case PDType.Pickup
                oStopTaskType = StopTaskType.PickUp
            Case PDType.General
                oStopTaskType = StopTaskType.General
        End Select

        oTargetRouteStop.AddStopDetail(oStopTaskType, oTargetRoute.RouteDate, _
                oReq.Consignee, oReq.OrderId, _
                        oReq.OrderType, oReq.TargetCompany, oReq.CompanyType, _
                            oReq.ContactId, oReq.ChkPnt, "", 0, oReq.TransportationClass, _
                            oReq.OrderVolume, oReq.OrderWeight, oReq.OrderValue, _
                            "Placed from unrouted", StopTaskConfirmationType.None, WMS.Logic.GetCurrentUser)

        oTargetRoute = New Route(TargetRouteID)
        oTargetRoute.ResequenceRoute(oLogger)

    End Sub



    Public Function findBestchkPntPos(ByVal pChkPntGeoPointNode As GeoPointNode, _
        ByVal stopNum As Integer, _
        ByRef bestpos As Integer, _
        ByRef bestDistance As Double, ByRef bestTime As Double) As Double

        Dim addDistance As Double = 0D
        Dim addTime As Double = 0D

        bestDistance = Integer.MaxValue
        bestTime = Integer.MaxValue


        Dim oStrat As RoutingStrategy = New RoutingStrategy(_strategyid)
        Dim oStratDet As RoutingStrategyDetail = oStrat.StrategyDetails(0)


        'find first apropriate position fro pickup
        Dim startPos As Integer = 0

        For i As Integer = startPos To stopNum - 1
            Dim tempDist As Double = Integer.MaxValue
            Dim tmpDrivingTime As Double = Double.MaxValue

            If i = 0 Then
                Dim Target As GeoPointNode = New GeoPointNode(Stops(i).PointId)
                GeoNetworkItem.GetDistance(pChkPntGeoPointNode, Target, addDistance, addTime, Common.GetCurrentUser(), _
                    _isDistanceAdd, _isdistcost, _distcosttype)

                If (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If
            ElseIf i < Stops.Count Then
                Dim Source As GeoPointNode = New GeoPointNode(Stops(i - 1).PointId)
                Dim Target As GeoPointNode = New GeoPointNode(Stops(i).PointId)

                GeoNetworkItem.GetDistance(Source, pChkPntGeoPointNode, tempDist, tmpDrivingTime, Common.GetCurrentUser(), _
                    _isDistanceAdd, _isdistcost, _distcosttype)
                If (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If

                GeoNetworkItem.GetDistance(pChkPntGeoPointNode, Target, addDistance, addTime, Common.GetCurrentUser(), _
                    _isDistanceAdd, _isdistcost, _distcosttype)
                If (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops) Then
                    addDistance = Integer.MaxValue
                    addTime = Integer.MaxValue
                    Continue For
                End If


                Dim currDist, currTime As Double
                GeoNetworkItem.GetDistance(Source, pChkPntGeoPointNode, currDist, currTime, Common.GetCurrentUser(), _
                    _isDistanceAdd, _isdistcost, _distcosttype)


                addDistance += tempDist - currDist
                addTime += tmpDrivingTime - currTime

            ElseIf i = Stops.Count Then
                Dim Source As GeoPointNode = New GeoPointNode(Stops(i - 1).PointId)
                GeoNetworkItem.GetDistance(Source, pChkPntGeoPointNode, addDistance, addTime, Common.GetCurrentUser(), _
                    _isDistanceAdd, _isdistcost, _distcosttype)
                If (addDistance > oStratDet.MaxDistanceBetweenStops Or addTime > oStratDet.MaxTimeBetweenStops) Then
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


#End Region


End Class




#Region "Route Statuses"

Public Enum RouteStatus
    None = 0
    [New] = 1
    Assigned = 2
    Planning = 3
    Planned = 4
    Loaded = 5
    Confirmed = 6
    Departed = 7
    Returned = 8
    Canceled = 9
    Closed = 10
End Enum

#End Region

#Region "Route Status Changed Event Args"

<CLSCompliant(False)> Public Class RouteStatusChangedEventArgs

    Protected _routeid As String
    Protected _fromstatus As RouteStatus
    Protected _tostatus As RouteStatus
    Protected _timestamp As DateTime

    Public Sub New(ByVal sRouteId As String, ByVal eFromStatus As RouteStatus, ByVal eToStatus As RouteStatus)
        _routeid = sRouteId
        _fromstatus = eFromStatus
        _tostatus = eToStatus
        _timestamp = DateTime.Now
    End Sub

    Public ReadOnly Property RouteId() As String
        Get
            Return _routeid
        End Get
    End Property

    Public ReadOnly Property FromStatus() As RouteStatus
        Get
            Return _fromstatus
        End Get
    End Property

    Public ReadOnly Property ToStatus() As RouteStatus
        Get
            Return _tostatus
        End Get
    End Property

    Public ReadOnly Property TimeStamp() As DateTime
        Get
            Return _timestamp
        End Get
    End Property
End Class

#End Region

