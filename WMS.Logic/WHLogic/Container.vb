Imports Made4Net.DataAccess
Imports Made4Net.Shared

#Region "Container"

<CLSCompliant(False)> Public Class Container
    Implements IContainerContentList
    <CLSCompliant(False)> Public Class ContainerUsageType
        Public Const PickingContainer As String = "PICKCONT"
        Public Const ShippingContainer As String = "SHIPCONT"
        Public Const PutawayContainer As String = "PWCONT"
    End Class

#Region "Variables"

    Protected _container As String
    Protected _hutype As String
    Protected _destinationlocation As String
    Protected _destinationwarehousearea As String
    Protected _oncontainer As String
    Protected _createdate As DateTime
    Protected _usagetype As String
    Protected _serial As String
    Protected _status As String
    Protected _activitystatus As String
    Protected _location As String
    Protected _warehousearea As String
    Protected _laststatusdate As DateTime
    'Protected _inhandoff As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    Protected _containerloads As ContainerLoads

    Protected _weight As Decimal = 0
    Protected _volume As Decimal = 0

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" CONTAINER = '{0}' ", _container)
        End Get
    End Property

    Public Property ContainerId() As String
        Get
            Return _container
        End Get
        Set(ByVal Value As String)
            _container = Value
        End Set
    End Property

    Public Property DestinationLocation() As String
        Get
            Return _destinationlocation
        End Get
        Set(ByVal Value As String)
            _destinationlocation = Value
        End Set
    End Property

    Public Property DestinationWarehousearea() As String
        Get
            Return _destinationwarehousearea
        End Get
        Set(ByVal Value As String)
            _destinationwarehousearea = Value
        End Set
    End Property

    Public Property HandlingUnitType() As String
        Get
            Return _hutype
        End Get
        Set(ByVal Value As String)
            _hutype = Value
        End Set
    End Property

    Public Property OnContainer() As String
        Get
            Return _oncontainer
        End Get
        Set(ByVal Value As String)
            _oncontainer = Value
        End Set
    End Property

    Public ReadOnly Property CreateDate() As DateTime
        Get
            Return _createdate
        End Get
    End Property

    Public Property UsageType() As String
        Get
            Return _usagetype
        End Get
        Set(ByVal Value As String)
            _usagetype = Value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal Value As String)
            _serial = Value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property ActivityStatus() As String
        Get
            Return _activitystatus
        End Get
        Set(ByVal Value As String)
            _activitystatus = Value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return _location
        End Get
        Set(ByVal Value As String)
            _location = Value
        End Set
    End Property

    Public Property Warehousearea() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
        End Set
    End Property

    Public Property LastStatusDate() As DateTime
        Get
            Return _laststatusdate
        End Get
        Set(ByVal Value As DateTime)
            _laststatusdate = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public ReadOnly Property Loads() As ContainerLoads
        Get
            Return _containerloads
        End Get
    End Property

    Public ReadOnly Property Volume() As Decimal
        Get
            Return _volume
        End Get
    End Property

    Public ReadOnly Property Weight() As Decimal
        Get
            Return _weight
        End Get
    End Property

    Public ReadOnly Property HasAllocatedLoads() As Boolean
        Get
            For Each oLoad As WMS.Logic.Load In _containerloads
                If oLoad.UNITSALLOCATED > 0 Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        _containerloads = New ContainerLoads
    End Sub

    Public Sub New(ByVal ContainerId As String, ByVal LoadAll As Boolean)
        _container = ContainerId
        Load(LoadAll)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal ContainerId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from CONTAINER where CONTAINER = '{0}'", ContainerId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj.GetType Is GetType(Container) Then
            If CType(obj, Container).ContainerId = Me.ContainerId Then
                Return True
            Else
                Return False
            End If
        End If
        Return False
    End Function

    Public Sub Load(ByVal LoadAll As Boolean)
        Dim dt As New DataTable
        Dim sql As String = String.Format("Select * from CONTAINER where {0}", WhereClause)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException("Container does not exists")
        End If
        Dim dr As DataRow = dt.Rows(0)
        _container = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTAINER"))
        _hutype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HUTYPE"))
        _destinationlocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DESTINATIONLOCATION"))
        _destinationwarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DESTINATIONWAREHOUSEAREA"))
        _oncontainer = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ONCONTAINER"))
        _createdate = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CREATEDATE"), DateTime.Now))
        _usagetype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("USAGETYPE")))
        _serial = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SERIAL")))
        _status = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATUS")))
        _location = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LOCATION")))
        _warehousearea = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA")))
        _laststatusdate = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LASTSTATUSDATE"), DateTime.Now))
        _activitystatus = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ActivityStatus"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))

        If LoadAll Then
            _containerloads = New ContainerLoads(_container, LoadAll)
            Try
                _volume = _containerloads.Volume
            Catch ex As Exception
            End Try
            Try
                _weight = _containerloads.Weight
            Catch ex As Exception
            End Try
        Else
            _containerloads = New ContainerLoads()
        End If
    End Sub

    ' Returns TaskID that this container is assigned to
    Public Function isAssignedToTask() As String
        Dim strSql As String = "SELECT * FROM TASKS WHERE status not in ('COMPLETE','CANCELED') and FROMCONTAINER = '" & _container & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            'If Convert.ToString(dt.Rows(0)("STATUS")).ToLower = "assigned" And Convert.ToString(dt.Rows(0)("USERID")).ToLower <> GetCurrentUser().ToLower Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Container Task assigned to another user", "Container Task assigned to another user")
            'End If
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

    ' Returns true if the container has several other containers on it (Parallel Picking)
    Public Function IsExternalContainer() As Boolean
        Dim strSql As String = String.Format("SELECT count(1) FROM container WHERE ONCONTAINER = '{0}'", _container)
        Dim cnt As Int64 = DataInterface.ExecuteScalar(strSql)
        If cnt > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#Region "Activity Statuses"

    Public Sub SetActivityStatus(ByVal pStatus As String, ByVal pUser As String)
        _activitystatus = pStatus
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update CONTAINER set ACTIVITYSTATUS = {0}, EditDate = {1}, EditUser = {2} Where {3}", Made4Net.Shared.Util.FormatField(_activitystatus), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Post & Place & Deliver & Move"

    Public Sub Move(ByVal pLoc As String, ByVal pSubLocation As String, ByVal pWarehousearea As String, ByVal pUser As String)
        _location = pLoc
        _edituser = pUser
        _editdate = DateTime.Now
        _warehousearea = pWarehousearea
        Dim ld As Load
        For Each ld In _containerloads
            ld.Move(pLoc, pWarehousearea, pSubLocation, pUser)
        Next

        Dim sql As String = String.Format("UPDATE CONTAINER SET LOCATION={0},WAREHOUSEAREA={1},EDITUSER={2},EDITDATE={3} WHERE {4}", _
         Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_warehousearea), _
         Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub SetDestinationLocation(ByVal pTargetLoc As String, ByVal pTargetWarehousearea As String, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser
        _destinationlocation = pTargetLoc
        _destinationwarehousearea = pTargetWarehousearea
        If _activitystatus Is Nothing Or _activitystatus = "" Or _activitystatus = WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND Then
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.PUTAWAYPEND
        End If
        For Each ld As Load In _containerloads
            ld.SetDestinationLocation(pTargetLoc, pTargetWarehousearea, pUser)
        Next
        Dim sql As String = String.Format("update container set destinationlocation = {0},destinationwarehousearea = {5},activitystatus = {1}, editdate = {2},edituser = {3} where {4}", _
                Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_destinationwarehousearea))
        DataInterface.ExecuteScalar(sql)
    End Sub

    Public Sub UpdateLastPickLocation(ByVal pLoc As String, ByVal pWarehousearea As String, ByVal pUser As String)
        _location = pLoc
        _warehousearea = pWarehousearea
        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String = String.Format("UPDATE CONTAINER SET location = {0}, warehousearea = {4}, EDITUSER={1},EDITDATE={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(_warehousearea))
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Place(ByVal pLoad As Load, ByVal pUser As String, Optional ByVal pPickedLoad As Boolean = False)
        'Dim sql As String = String.Format("INSERT INTO CONTAINERLOADS(CONTAINERID,LOADID,ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES ({0},{1},{2},{3},{4},{5})", _
        '    Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(pLoad.LOADID), Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(pUser))
        If pPickedLoad Then
            pLoad.PutOnContainer(_container, "", "", pUser)
        Else
            pLoad.PutOnContainer(_container, _location, _warehousearea, pUser)
        End If
        'DataInterface.RunSQL(sql)
        _containerloads.Add(pLoad)
    End Sub

    Public Sub Load(ByVal pConfirmation As String, ByVal pConfirmationWarehousearea As String, ByVal pUser As String, Optional ByVal pShipment As WMS.Logic.Shipment = Nothing)
        'Build orders cache so the load will not build the order each time in order to update its quantities
        Dim oOrderHT As Hashtable = BuildOrdersCache(pShipment)

        _status = WMS.Lib.Statuses.Container.LOADED
        _edituser = pUser
        _editdate = DateTime.Now
        For Each oLoad As WMS.Logic.Load In Me.Loads
            If oLoad.ACTIVITYSTATUS <> WMS.Lib.Statuses.ActivityStatus.LOADED Then
                Dim tempOutboundOrder As OutboundOrderHeader
                Dim tempOutboundOrderLine As OutboundOrderHeader.OutboundOrderDetail
                Dim tempFlowthroughOrder As Flowthrough
                Dim tempFlowthroughOrderLine As FlowthroughDetail

                If oOrderHT.ContainsKey(oLoad.LOADID.ToString()) Then
                    Dim tempOrderLoad As OrderLoadStruct = oOrderHT.Item(oLoad.LOADID.ToString())
                    If tempOrderLoad.oOrderLoad.DOCUMENTTYPE = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER Then
                        tempOutboundOrder = CType(tempOrderLoad.oOrder, WMS.Logic.OutboundOrderHeader)
                        tempOutboundOrderLine = CType(tempOrderLoad.oOrder, WMS.Logic.OutboundOrderHeader).Lines.Line(tempOrderLoad.oOrderLoad.ORDERLINE)
                    ElseIf tempOrderLoad.oOrderLoad.DOCUMENTTYPE = WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH Then
                        tempFlowthroughOrder = CType(tempOrderLoad.oOrder, WMS.Logic.Flowthrough)
                        tempFlowthroughOrderLine = CType(tempOrderLoad.oOrder, WMS.Logic.Flowthrough).LINES.Line(tempOrderLoad.oOrderLoad.ORDERLINE)
                    End If
                End If

                oLoad.Load(pConfirmation, pConfirmationWarehousearea, pUser, tempOutboundOrderLine, tempOutboundOrder, tempFlowthroughOrderLine, tempFlowthroughOrder, False)
            End If
        Next
        Dim sql As String = String.Format("UPDATE CONTAINER SET status = {0},EDITUSER={1},EDITDATE={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub
    Public Function AllLoadsAreLoaded() As Boolean
        Dim sql As String = String.Format("select count(1) from INVLOAD where HANDLINGUNIT='{0}' and ACTIVITYSTATUS<>'LOADED'", Me.ContainerId)
        Return Not Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    Public Function HasCaseDetails() As Boolean
        Dim sql As String = String.Format("select count(1) from casedetail where tocontainer='{0}'", Me.ContainerId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    Public Sub Create(ByVal pUser As String)
        If Not WMS.Logic.Location.Exists(_location, _warehousearea) Then
            'Throw New M4NException(New Exception, "Can not create load. Location does not exist.", "Can not create load. Location does not exist.")
            Throw New M4NException(New Exception, "Can not create container. Location does not exist.", "Can not create container. Location does not exist.")
        End If

        Post(pUser)
    End Sub
    'RWMS-2074 and RWMS-1726 Added Start   
    Public Sub Post(ByVal pUser As String, ByVal pck As PicklistDetail)
        _laststatusdate = DateTime.Now
        _status = WMS.Lib.Statuses.Container.STATUSNEW
        _createdate = DateTime.Now
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        If _container Is Nothing OrElse _container.Trim() = "" Then
            _container = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        End If

        Dim sql As String = String.Format("INSERT INTO CONTAINER(CONTAINER,HUTYPE,DESTINATIONLOCATION,DESTINATIONWAREHOUSEAREA,ONCONTAINER,CREATEDATE,USAGETYPE,SERIAL,STATUS,LASTSTATUSDATE,LOCATION, WAREHOUSEAREA, ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES (" & _
        "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_oncontainer), Made4Net.Shared.Util.FormatField(_createdate), _
        Made4Net.Shared.Util.FormatField(_usagetype), Made4Net.Shared.Util.FormatField(_serial), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
        Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)

        Dim em As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContainerCreated
        em.Add("EVENT", EventType)
        em.Add("DOCUMENT", pck.PickList)
        em.Add("DOCUMENTLINE", pck.PickListLine)
        em.Add("TOCONTAINER", _container)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYTYPE", "CONTINS")
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))

    End Sub
    'RWMS-2074 and RWMS-1726 Added End  

    Public Sub Post(ByVal pUser As String)
        _laststatusdate = DateTime.Now
        _status = WMS.Lib.Statuses.Container.STATUSNEW
        _createdate = DateTime.Now
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        If _container Is Nothing OrElse _container.Trim() = "" Then
            _container = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        End If

        Dim sql As String = String.Format("INSERT INTO CONTAINER(CONTAINER,HUTYPE,DESTINATIONLOCATION,DESTINATIONWAREHOUSEAREA,ONCONTAINER,CREATEDATE,USAGETYPE,SERIAL,STATUS,LASTSTATUSDATE,LOCATION, WAREHOUSEAREA, ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES (" & _
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_oncontainer), Made4Net.Shared.Util.FormatField(_createdate), _
            Made4Net.Shared.Util.FormatField(_usagetype), Made4Net.Shared.Util.FormatField(_serial), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContainerCreated
        em.Add("EVENT", EventType)
        'RWMS-2074 and RWMS-1726 Commented Start   
        'em.Add("CONTAINER", _container)   
        'RWMS-2074 and RWMS-1726 Commented End   

        'RWMS-2074 and RWMS-1726 Added Start   
        em.Add("TOCONTAINER", _container)
        'RWMS-2074 and RWMS-1726 Added End  

        em.Add("USERID", _adduser)
        em.Add("ACTIVITYTYPE", "CONTINS")
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))

    End Sub


    Public Sub Save(ByVal pUser As String)
        If Container.Exists(_container) Then
            '_laststatusdate = DateTime.Now
            _editdate = DateTime.Now
            _edituser = pUser

            Dim sql As String = String.Format("UPDATE CONTAINER SET HUTYPE ={0}, ONCONTAINER ={1}, DESTINATIONLOCATION ={2}, DESTINATIONWAREHOUSEAREA ={12}, USAGETYPE ={3}, SERIAL ={4}, STATUS ={5}, LASTSTATUSDATE ={6}, LOCATION ={7}, WAREHOUSEAREA ={13}, ACTIVITYSTATUS ={8}, EDITDATE ={9}, EDITUSER ={10} WHERE {11}", _
                Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_oncontainer), Made4Net.Shared.Util.FormatField(_destinationlocation), _
                Made4Net.Shared.Util.FormatField(_usagetype), Made4Net.Shared.Util.FormatField(_serial), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location), _
                Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_warehousearea))

            DataInterface.RunSQL(sql)

            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContainerUpdated
            em.Add("EVENT", EventType)
            em.Add("CONTAINER", _container)
            em.Add("USERID", _adduser)
            em.Add("ACTIVITYTYPE", "CONTUPD")
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))

        Else
            _laststatusdate = DateTime.Now
            _status = WMS.Lib.Statuses.Container.STATUSNEW
            _createdate = DateTime.Now
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser

            If _container Is Nothing OrElse _container.Trim() = "" Then
                _container = Made4Net.Shared.Util.getNextCounter("CONTAINER")
            End If

            Dim sql As String = String.Format("INSERT INTO CONTAINER(CONTAINER,HUTYPE,DESTINATIONLOCATION,ONCONTAINER,CREATEDATE,USAGETYPE,SERIAL,STATUS,LASTSTATUSDATE,LOCATION, ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES (" & _
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_oncontainer), Made4Net.Shared.Util.FormatField(_createdate), _
                Made4Net.Shared.Util.FormatField(_usagetype), Made4Net.Shared.Util.FormatField(_serial), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

            DataInterface.RunSQL(sql)

            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContainerCreated
            em.Add("EVENT", EventType)
            em.Add("CONTAINER", _container)
            em.Add("USERID", _adduser)
            em.Add("ACTIVITYTYPE", "CONTINS")
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))
        End If

    End Sub

    Public Sub Deliver(ByVal toLocation As String, ByVal toWarehousearea As String, ByVal pUser As String, Optional ByVal IsHandOff As Boolean = False, Optional ByVal pRemoveFromOnContainer As Boolean = True)
        Dim ld As Load
        Dim shouldStage As Boolean

        'Build orders cache so the load will not build the order each time in order to update its quantities
        Dim oOrderHT As Hashtable = BuildOrdersCache()
        For Each ld In _containerloads
            Try
                Dim tempOutboundOrder As OutboundOrderHeader
                Dim tempOutboundOrderLine As OutboundOrderHeader.OutboundOrderDetail
                Dim tempFlowthroughOrder As Flowthrough
                Dim tempFlowthroughOrderLine As FlowthroughDetail

                If oOrderHT.ContainsKey(ld.LOADID.ToString()) Then
                    Dim tempOrderLoad As OrderLoadStruct = oOrderHT.Item(ld.LOADID.ToString())
                    If tempOrderLoad.oOrderLoad.DOCUMENTTYPE = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER Then
                        tempOutboundOrder = CType(tempOrderLoad.oOrder, WMS.Logic.OutboundOrderHeader)
                        tempOutboundOrderLine = CType(tempOrderLoad.oOrder, WMS.Logic.OutboundOrderHeader).Lines.Line(tempOrderLoad.oOrderLoad.ORDERLINE)
                    ElseIf tempOrderLoad.oOrderLoad.DOCUMENTTYPE = WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH Then
                        tempFlowthroughOrder = CType(tempOrderLoad.oOrder, WMS.Logic.Flowthrough)
                        tempFlowthroughOrderLine = CType(tempOrderLoad.oOrder, WMS.Logic.Flowthrough).LINES.Line(tempOrderLoad.oOrderLoad.ORDERLINE)
                    End If
                End If
                ld.Deliver(toLocation, toWarehousearea, False, pUser, Not IsHandOff, IsHandOff, True, tempOutboundOrderLine, tempOutboundOrder, tempFlowthroughOrderLine, tempFlowthroughOrder)
                If ld.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.STAGED Then
                    shouldStage = True
                End If
            Catch ex As Exception
            End Try
        Next
        If shouldStage Then
            _status = WMS.Lib.Statuses.Container.STAGED
            _destinationlocation = Nothing
            _destinationwarehousearea = Nothing
            If pRemoveFromOnContainer Then
                _oncontainer = Nothing
            End If
            _activitystatus = Nothing
        Else
            If Not IsHandOff Then
                _status = WMS.Lib.Statuses.Container.DELIVERED
                'its not an handoff location - the loads were delivered but not to staging lane...
                If Not _destinationlocation Is Nothing AndAlso _destinationlocation.Equals(toLocation, StringComparison.OrdinalIgnoreCase) Then
                    _destinationlocation = Nothing
                    _activitystatus = Nothing
                End If
            End If
        End If
        _location = toLocation
        _warehousearea = toWarehousearea
        _laststatusdate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update Container set status={0},destinationlocation={1},destinationwarehousearea={8},oncontainer={2},editdate={3},edituser={4},location={5},warehousearea={9},activitystatus={6} where {7}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_oncontainer), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_activitystatus), WhereClause, Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_warehousearea))
        DataInterface.RunSQL(sql)

    End Sub

    Private Function BuildOrdersCache(Optional ByVal pShipment As WMS.Logic.Shipment = Nothing) As Hashtable
        Dim OrdersLoadsHT As New Hashtable
        Dim oOrdersHt As New Hashtable
        Dim sSql As String = String.Format("SELECT INVLOAD.HANDLINGUNIT as CONTAINERID, ORDERLOADS.DOCUMENTTYPE, ORDERLOADS.CONSIGNEE, ORDERLOADS.ORDERID, ORDERLOADS.ORDERLINE, ORDERLOADS.LOADID, ORDERLOADS.PICKLIST, ORDERLOADS.PICKLISTLINE, ORDERLOADS.ADDDATE, ORDERLOADS.ADDUSER, ORDERLOADS.EDITDATE, ORDERLOADS.EDITUSER FROM ORDERLOADS INNER JOIN INVLOAD ON ORDERLOADS.LOADID = INVLOAD.LOADID where INVLOAD.HANDLINGUNIT = '{0}'", _container)
        Dim dt As New DataTable
        DataInterface.FillDataset(sSql, dt)
        For Each dr As DataRow In dt.Rows
            Dim oOrder As Object = Nothing
            If Not oOrdersHt.ContainsKey(dr("DOCUMENTTYPE").ToString & "#" & dr("CONSIGNEE").ToString & "#" & dr("ORDERID").ToString) Then
                Select Case dr("DOCUMENTTYPE").ToString
                    Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                        If Not pShipment Is Nothing Then
                            oOrder = pShipment.Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID"))
                        End If
                        If oOrder Is Nothing Then
                            oOrder = New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        End If
                    Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                        oOrder = New WMS.Logic.Flowthrough(dr("CONSIGNEE"), dr("ORDERID"))
                End Select
                oOrdersHt.Add(dr("DOCUMENTTYPE").ToString & "#" & dr("CONSIGNEE").ToString & "#" & dr("ORDERID").ToString, oOrder)
            Else
                oOrder = oOrdersHt.Item(dr("DOCUMENTTYPE").ToString & "#" & dr("CONSIGNEE").ToString & "#" & dr("ORDERID").ToString)
            End If
            Dim oOrderLoad As New OrderLoads(dr)
            Dim tempOrdLoadStruct As OrderLoadStruct
            tempOrdLoadStruct.oOrder = oOrder
            tempOrdLoadStruct.oOrderLoad = oOrderLoad
            OrdersLoadsHT.Add(dr("LOADID").ToString, tempOrdLoadStruct)
        Next
        Return OrdersLoadsHT
    End Function

    Private Structure OrderLoadStruct
        Public oOrderLoad As OrderLoads
        Public oOrder As Object
    End Structure

    Public Sub DeliverPend(ByVal pUser As String)
        _status = WMS.Lib.Statuses.Container.DELIVERYPEND
        _destinationlocation = Nothing
        _destinationwarehousearea = Nothing
        _oncontainer = Nothing
        _laststatusdate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("Update Container set status={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Put(ByVal toLocation As String, ByVal toWarehousearea As String, ByVal pUser As String, Optional ByVal isHandOff As Boolean = False)
        Dim oldLoc As String = _location
        Dim oldWarehousearea As String = _warehousearea
        _laststatusdate = DateTime.Now
        _location = toLocation
        _warehousearea = toWarehousearea
        _editdate = DateTime.Now
        _edituser = pUser
        If Not isHandOff Then
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.NONE
            _destinationlocation = ""
            _destinationwarehousearea = ""
        End If

        Dim ld As Load
        For Each ld In _containerloads
            ld.Deliver(toLocation, toWarehousearea, False, pUser, False, isHandOff, True)
        Next

        Dim sql As String = String.Format("Update Container set location={0},warehousearea={6}, activitystatus={1},destinationlocation={2}, destinationwarehousearea={7},editdate={3},edituser={4} where {5}", _
            Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_destinationwarehousearea))
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ContainerPutaway)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CONTAINERPUTAWAY)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMCONTAINER", _container)
        aq.Add("FROMLOC", oldLoc)
        aq.Add("FROMWAREHOUSEAREA", oldWarehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "PROC")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOCONTAINER", _container)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Audit.CONTAINERPUTAWAY)
    End Sub

    Public Sub Remove(ByVal ld As Load)
        _containerloads.Remove(ld)
    End Sub

    Public Sub RequestPickUp(ByVal pUser As String)
        Dim strTask As String
        strTask = isAssignedToTask()
        If strTask <> "" Then
            Return
        End If
        Dim ld As Load
        For Each ld In _containerloads
            Try
                If ld.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.PICKED And ld.DESTINATIONLOCATION <> "" And ld.DESTINATIONWAREHOUSEAREA <> "" Then
                    'Do nothing - will get the cont load pw task....
                    'CreateLDPWTask(ld, pUser)
                Else
                    ld.RequestPickUp(pUser, "", False, True) ''RWMS-1277
                End If
            Catch ex As Exception
            End Try
        Next
        If _containerloads.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Container has no loads", "Container has no loads")
        Else
            Dim tm As New TaskManager
            tm.CreateContainerLoadPutAwayTask(Me, pUser)
        End If
    End Sub

    Private Sub CreateLDPWTask(ByVal ld As Load, ByVal pUser As String)
        Dim SQL As String = String.Format("select count(1) from tasks where fromload = '{0}' and status not in('COMPLETE','CANCELED')", ld.LOADID)
        Dim exists As Boolean = Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
        If Not exists Then
            Dim tm As New TaskManager
            tm.CreatePutAwayTask(ld, pUser)
        End If
    End Sub

    Public Sub PutAwayLoads(ByVal pUser As String)
        Dim strTask As String
        strTask = isAssignedToTask()
        If strTask <> "" Then
            Return
        End If
        Dim ld As Load
        For Each ld In _containerloads
            Try
                ld.PutAway(ld.DESTINATIONLOCATION, ld.DESTINATIONWAREHOUSEAREA, pUser, False)
            Catch ex As Exception
            End Try
        Next
        If _containerloads.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Container has no loads", "Container has no loads")
        Else
            Dim tm As New TaskManager
            tm.CreateContainerLoadPutAwayTask(Me, pUser)
        End If
    End Sub

    Public Sub PutAway(ByVal pUser As String, Optional ByVal CreateTask As Boolean = True)
        If _activitystatus <> WMS.Lib.Statuses.ActivityStatus.NONE Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Container is Assigned to another activity", "Container is Assigned to another activity")
        End If
        For Each oLoad As WMS.Logic.Load In _containerloads
            If oLoad.hasActivity() Or oLoad.UNITSALLOCATED > 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "One or more loads on container has activity", "One or more loads on container has activity")
            Else
                oLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, pUser)
            End If
        Next
        Dim destLoc, destWarehousearea, strTask As String
        strTask = isAssignedToTask()
        If strTask <> "" Then
            Return
        End If
        Dim pw As New Putaway
        pw.RequestDestinationForContainer(destLoc, destWarehousearea, _container, pUser, CreateTask)
    End Sub

#End Region

#Region "Printing"

    Public Sub PrintShipLabel(Optional ByVal lblPrinter As String = "", Optional ByVal pLabelFormat As String = "")
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        If pLabelFormat Is Nothing OrElse pLabelFormat = String.Empty Then
            pLabelFormat = "CONTSHIPLBL"
        End If
        If Not Made4Net.Label.LabelHandler.Factory.GetNewLableHandler().ValidateLabel(pLabelFormat) Then
            Throw New M4NException(New Exception(), "'" + pLabelFormat + "' Label Not Configured.", "'" + pLabelFormat + "' Label Not Configured.")
        Else
            Dim qSender As IQMsgSender = QMsgSender.Factory.Create()
            qSender.Add("LABELNAME", "CONTSHIPLBL")
            qSender.Add("LabelType", pLabelFormat)
            qSender.Add("PRINTER", lblPrinter)
            qSender.Add("FORMATFILE", pLabelFormat)
            qSender.Add("CONTAINERID", _container)
            Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
            ht.Hash.Add("CONTAINERID", _container)
            qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
            qSender.Send("Label", String.Format("Container Shipping Label ({0})", _container))
        End If
    End Sub

    Public Sub PrintContainerLabel(Optional ByVal lblPrinter As String = "")
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As IQMsgSender = QMsgSender.Factory.Create()
        qSender.Add("LABELNAME", "CONTLBL")
        qSender.Add("LabelType", "CONTLBL")
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        qSender.Add("CONTAINERID", _container)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("CONTAINERID", _container)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        'Commented for RWMS-780
        'qSender.Send("LABEL", String.Format("Container Label ({0})", _container))
        'End Commented for RWMS-780
        'Added for RWMS-780
        qSender.Send("Label", String.Format("Container Label ({0})", _container))
        'End Added for RWMS-780
    End Sub

    Public Sub PrintContainerContentList(ByVal lblPrinter As String, ByVal Language As Int32, ByVal pPicklistID As String, ByVal pUser As String, Optional ByVal pReportName As String = "ContentList")
        Dim oQsender As IQMsgSender = QMsgSender.Factory.Create()
        Dim repType As String
        Dim dt As New DataTable
        repType = pReportName
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}'", repType), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        Try
            oQsender.Add("DATASETID", dt.Select("ParamName='DATASETNAME'")(0)("ParamValue")) ' "repContentList")
        Catch ex As Exception
            oQsender.Add("DATASETID", "repContentList")
        End Try

        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", lblPrinter)
            Try
                oQsender.Add("COPIES", dt.Select("ParamName='Copies'")(0)("ParamValue"))
            Catch ex As Exception
                oQsender.Add("COPIES", 1)
            End Try

            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONTAINERID = '{0}' AND PICKLIST = '{1}'", _container, pPicklistID))
        oQsender.Send("Report", repType)
    End Sub

    Public Sub PrintContentList(ByVal lblPrinter As String, ByVal Language As Int32, ByVal pUser As String, Optional ByVal pReportName As String = "ContentList")
        Dim oQsender As IQMsgSender = QMsgSender.Factory.Create()
        Dim repType As String
        Dim dt As New DataTable
        repType = pReportName
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}'", repType), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        Try
            oQsender.Add("DATASETID", dt.Select("ParamName='DATASETNAME'")(0)("ParamValue")) ' "repContentList")
        Catch ex As Exception
            oQsender.Add("DATASETID", "repContentList")
        End Try

        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", lblPrinter)
            Try
                oQsender.Add("COPIES", dt.Select("ParamName='Copies'")(0)("ParamValue"))
            Catch ex As Exception
                oQsender.Add("COPIES", 1)
            End Try

            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONTAINERID = '{0}'", _container))
        oQsender.Send("Report", repType)
    End Sub

    Public Function ContentContainerWhereClause(ByVal pickListID As String) As String Implements IContainerContentList.ContentContainerWhereClause
        Return String.Format("CONTAINERID = '{0}' AND PICKLIST = '{1}'", _container, pickListID)
    End Function
#End Region

#End Region

End Class

#End Region

#Region "Container Loads"

<CLSCompliant(False)> Public Class ContainerLoads
    Implements ICollection

#Region "Variables"

    Protected _container As String
    Protected _loads As ArrayList

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As Load
        Get
            Return _loads(index)
        End Get
        Set(ByVal Value As Load)
            _loads(index) = Value
        End Set
    End Property

    Default Public ReadOnly Property Item(ByVal LoadId As String) As Load
        Get
            Dim ld As Load
            For Each ld In Me
                If ld.LOADID = LoadId Then
                    Return ld
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Volume() As Decimal
        Get
            Dim vol As Decimal
            For Each ld As Load In Me
                vol += ld.Volume 'ld.CalculateVolume()
            Next
            Return vol
        End Get
    End Property

    Public ReadOnly Property Weight() As Decimal
        Get
            Dim wgt As Decimal
            For Each ld As Load In Me
                wgt += ld.Weight 'ld.CalculateWeight()
            Next
            Return wgt
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _loads = New ArrayList
    End Sub

    Public Sub New(ByVal ContainerId As String, ByVal LoadAll As Boolean)
        _loads = New ArrayList
        _container = ContainerId
        If LoadAll Then
            Load()
        End If
    End Sub

    Protected Sub Load()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("select invload.handlingunit as containerid, container.hutype, invload.*, attribute.*, skuuom.netweight, skuuom.volume from invload inner join CONTAINER ON CONTAINER.CONTAINER = invload.handlingunit left outer join attribute on invload.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' inner join skuuom on invload.consignee = skuuom.consignee and invload.sku = skuuom.sku and (skuuom.loweruom = '' or skuuom.loweruom is null) where container ='{0}' order by invload.LASTMOVEDATE desc", _container)
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            Add(New Load(dr))
        Next
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As Load) As Integer
        For Each ld As WMS.Logic.Load In _loads
            If ld.LOADID = value.LOADID Then
                Return -1
            End If
        Next
        Return _loads.Add(value)
    End Function

    Public Sub Clear()
        _loads.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As Load)
        _loads.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As Load)
        _loads.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _loads.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _loads.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _loads.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _loads.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _loads.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _loads.GetEnumerator()
    End Function

#End Region

End Class

#End Region