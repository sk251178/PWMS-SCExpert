Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Util

<CLSCompliant(False)> Public Class YardEntry

#Region "Variables"

#Region "Primary Keys"

    Protected _yardentryid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _carrier As String = String.Empty
    Protected _vehicle As String = String.Empty
    Protected _trailer As String = String.Empty
    Protected _yardlocation As String = String.Empty
    Protected _scheduledate As DateTime
    Protected _checkoutdate As DateTime
    Protected _checkindate As DateTime
    Protected _status As String = String.Empty
    Protected _statusdate As DateTime
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where YardEntryId = '" & _yardentryid & "'"
        End Get
    End Property

    Public Property YARDENTRYID() As String
        Get
            Return _yardentryid
        End Get
        Set(ByVal Value As String)
            _yardentryid = Value
        End Set
    End Property

    Public Property CARRIER() As String
        Get
            Return _carrier
        End Get
        Set(ByVal Value As String)
            _carrier = Value
        End Set
    End Property

    Public Property VEHICLE() As String
        Get
            Return _vehicle
        End Get
        Set(ByVal Value As String)
            _vehicle = Value
        End Set
    End Property

    Public Property TRAILER() As String
        Get
            Return _trailer
        End Get
        Set(ByVal Value As String)
            _trailer = Value
        End Set
    End Property

    Public Property YARDLOCATION() As String
        Get
            Return _yardlocation
        End Get
        Set(ByVal Value As String)
            _yardlocation = Value
        End Set
    End Property

    Public Property CHECKINDATE() As DateTime
        Get
            Return _checkindate
        End Get
        Set(ByVal Value As DateTime)
            _checkindate = Value
        End Set
    End Property

    Public Property CHECKOUTDATE() As DateTime
        Get
            Return _checkoutdate
        End Get
        Set(ByVal Value As DateTime)
            _checkoutdate = Value
        End Set
    End Property

    Public Property SCHEDULEDATE() As DateTime
        Get
            Return _scheduledate
        End Get
        Set(ByVal Value As DateTime)
            _scheduledate = Value
        End Set
    End Property

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property STATUSDATE() As DateTime
        Get
            Return _statusdate
        End Get
        Set(ByVal Value As DateTime)
            _statusdate = Value
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

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pYardEntryId As String, Optional ByVal LoadObj As Boolean = True)
        _yardentryid = pYardEntryId
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal pYardEntry As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from yardentry where yardentryid = '{0}'", pYardEntry)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetYardEntry(ByVal pYardEntryId As String) As YardEntry
        Return New YardEntry(pYardEntryId)
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM yardentry " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Yard Entry does not exists", "Yard Entry does not exists")
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("CARRIER") Then _carrier = dr.Item("CARRIER")
        If Not dr.IsNull("VEHICLE") Then _vehicle = dr.Item("VEHICLE")
        If Not dr.IsNull("TRAILER") Then _trailer = dr.Item("TRAILER")
        If Not dr.IsNull("YARDLOCATION") Then _yardlocation = dr.Item("YARDLOCATION")
        If Not dr.IsNull("CHECKINDATE") Then _checkindate = dr.Item("CHECKINDATE")
        If Not dr.IsNull("CHECKOUTDATE") Then _checkoutdate = dr.Item("CHECKOUTDATE")
        If Not dr.IsNull("SCHEDULEDATE") Then _scheduledate = dr.Item("SCHEDULEDATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("STATUSDATE") Then _statusdate = dr.Item("STATUSDATE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

#Region "Create / Update"

    Public Sub Create(ByVal pYardEntryId As String, ByVal pCarrier As String, ByVal pVehicle As String, ByVal pTrailer As String, ByVal pYardLocation As String, _
                 ByVal pScheduleDate As DateTime, ByVal pUser As String)

        If WMS.Logic.YardEntry.Exists(pYardEntryId) Then
            Throw New M4NException(New Exception, "Yard Entry Already Exists", "Yard Entry Already Exists")
        End If
        If Not WMS.Logic.Carrier.Exists(pCarrier) Then
            Throw New M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
        End If
        If pYardEntryId = "" Then
            _yardentryid = Made4Net.Shared.Util.getNextCounter("YARDENTRY")
        Else
            _yardentryid = pYardEntryId
        End If
        _carrier = pCarrier
        _scheduledate = pScheduleDate
        _trailer = pTrailer
        _vehicle = pVehicle
        _yardlocation = pYardLocation
        _status = WMS.Lib.Statuses.YardEntry.STATUSNEW
        _statusdate = DateTime.Now
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        SQL = String.Format("INSERT INTO YARDENTRY (YARDENTRYID, CARRIER, VEHICLE, TRAILER, YARDLOCATION, CHECKINDATE, CHECKOUTDATE, SCHEDULEDATE, STATUS, STATUSDATE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", FormatField(_yardentryid), FormatField(_carrier), FormatField(_vehicle), FormatField(_trailer), FormatField(_yardlocation), _
                FormatField(_checkindate), FormatField(_checkoutdate), FormatField(_scheduledate), FormatField(_status), FormatField(_statusdate), FormatField(_adddate), FormatField(_adduser), FormatField(_editdate), FormatField(_edituser))
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryCreated
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub Update(ByVal pCarrier As String, ByVal pVehicle As String, ByVal pTrailer As String, ByVal pYardLocation As String, _
                 ByVal pCheckInDate As DateTime, ByVal pCheckOutDate As DateTime, ByVal pScheduleDate As DateTime, ByVal pUser As String)

        If Not WMS.Logic.Carrier.Exists(pCarrier) Then
            Throw New M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
        End If
        _carrier = pCarrier
        _checkindate = pCheckInDate
        _checkoutdate = pCheckOutDate
        _scheduledate = pScheduleDate
        _trailer = pTrailer
        _vehicle = pVehicle
        _yardlocation = pYardLocation
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        SQL = String.Format("UPDATE YARDENTRY SET CARRIER ={0}, VEHICLE ={1}, TRAILER ={2}, YARDLOCATION ={3}, CHECKINDATE ={4}, CHECKOUTDATE ={5}, " & _
                " SCHEDULEDATE ={6}, EDITDATE ={7}, EDITUSER ={8} {9} ", FormatField(_carrier), FormatField(_vehicle), FormatField(_trailer), FormatField(_yardlocation), _
                FormatField(_checkindate), FormatField(_checkoutdate), FormatField(_scheduledate), FormatField(_editdate), FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryUpdated
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Location"

    Public Sub SetLocation(ByVal pYardLocation As String, ByVal pUser As String)
        '' Yard location = warehouse location?! if yes - what about warehouse area?
        'If Not WMS.Logic.Location.Exists(pYardLocation) Then
        '    Throw New M4NException(New Exception, "Location does not exists", "Location does not exists")
        'End If
        Dim oldLocation As String = _yardlocation
        _yardlocation = pYardLocation
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        SQL = String.Format("UPDATE YARDENTRY SET YARDLOCATION ={0} " & _
                " , EDITDATE ={1}, EDITUSER ={2} where {3} ", FormatField(_yardlocation), _
                FormatField(_editdate), FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryLocationChanged
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("FROMLOCATION", oldLocation)
        em.Add("TOLOCATION", _yardlocation)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Statuses / CheckIn / CheckOut"

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.YardEntry.CANCELED Then
            Throw New M4NException(New Exception, "Wrong Yard Entry Status - Cannot change status", "Wrong Yard Entry Status - Cannot change status")
        End If
        Dim OldStatus As String = _status
        _status = pStatus
        _statusdate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        SQL = String.Format("UPDATE YARDENTRY SET status={0},statusdate={1} " & _
                " , EDITDATE ={2}, EDITUSER ={3} {4} ", FormatField(_status), FormatField(_statusdate), _
                FormatField(_editdate), FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryStatusChanged
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("FROMSTATUS", OldStatus)
        em.Add("TOSTATUS", _status)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub Schedule(ByVal pUser As String)
        SetStatus(WMS.Lib.Statuses.YardEntry.SCHEDULED, pUser)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryScheduled
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub CheckIn(ByVal pYardLocation As String, ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.YardEntry.SCHEDULED Or _status <> WMS.Lib.Statuses.YardEntry.STATUSNEW Then
            Throw New M4NException(New Exception, "Wrong Yard Entry Status - Cannot change status", "Wrong Yard Entry Status - Cannot change status")
        End If

        'Add validation for yard location....

        _status = WMS.Lib.Statuses.YardEntry.INYARD
        _statusdate = DateTime.Now
        _checkindate = DateTime.Now
        _yardlocation = pYardLocation
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        SQL = String.Format("UPDATE YARDENTRY SET status={0},statusdate={1}, checkindate={2}, yardlocation={3} " & _
                " , EDITDATE ={4}, EDITUSER ={5} {6} ", FormatField(_status), FormatField(_statusdate), FormatField(_checkindate), _
                FormatField(_yardlocation), FormatField(_editdate), FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryCheckIn
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub CheckOut(ByVal pYardLocation As String, ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.YardEntry.INYARD Or _status <> WMS.Lib.Statuses.YardEntry.SCHEDULED Or _status <> WMS.Lib.Statuses.YardEntry.STATUSNEW Then
            Throw New M4NException(New Exception, "Wrong Yard Entry Status - Cannot change status", "Wrong Yard Entry Status - Cannot change status")
        End If

        'Add validation for yard location....

        _status = WMS.Lib.Statuses.YardEntry.DEPARTED
        _statusdate = DateTime.Now
        _checkoutdate = DateTime.Now
        _yardlocation = pYardLocation
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        SQL = String.Format("UPDATE YARDENTRY SET status={0},statusdate={1}, checkoutdate={2}, yardlocation={3} " & _
                " , EDITDATE ={4}, EDITUSER ={5} {6} ", FormatField(_status), FormatField(_statusdate), FormatField(_checkindate), _
                FormatField(_yardlocation), FormatField(_editdate), FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.YardEntryCheckOut
        em.Add("EVENT", EventType)
        em.Add("YARDENTRYID", _yardentryid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#End Region

End Class
