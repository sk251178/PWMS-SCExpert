Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class Vehicles

#Region "Variables"

#Region "Primary Keys"

    Protected _carrier As String = String.Empty
    Protected _truck As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _status As Boolean = True
    Protected _activitystatus As String = String.Empty
    Protected _yardlocation As String = String.Empty
    Protected _trucktype As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CARRIER = '" & _carrier & "' and VEHICLEID = '" & _truck & "'"
        End Get
    End Property

    Public ReadOnly Property WhereClauseWithoutCarrier() As String
        Get
            Return " VEHICLEID = '" & _truck & "'"
        End Get
    End Property

    Public Property YARDLOCATION() As String
        Get
            Return _yardlocation
        End Get
        Set(ByVal Value As String)
            _yardlocation = Value
        End Set
    End Property


    Public Property STATUS() As Boolean
        Get
            Return _status
        End Get
        Set(ByVal Value As Boolean)
            _status = Value
        End Set
    End Property

    Public Property ACTIVITYSTATUS() As String
        Get
            Return _activitystatus
        End Get
        Set(ByVal Value As String)
            _status = Value
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

    Public Property TRUCK() As String
        Get
            Return _truck
        End Get
        Set(ByVal Value As String)
            _truck = Value
        End Set
    End Property

    Public Property TRUCKTYPE() As String
        Get
            Return _trucktype
        End Get
        Set(ByVal Value As String)
            _trucktype = Value
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

    Public Sub New(ByVal pCARRIER As String, ByVal pTRUCK As String, Optional ByVal LoadObj As Boolean = True)
        _carrier = pCARRIER
        _truck = pTRUCK
        If Contact.Exists(pCARRIER) And LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal pTRUCK As String, Optional ByVal LoadObj As Boolean = True)
        '  _carrier = pCARRIER
        _truck = pTRUCK
        If LoadObj Then
            LoadWithoutCarrier()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetVehicle(ByVal pCarrier As String, ByVal pTruck As String) As Vehicles
        Return New Vehicles(pCarrier, pTruck)
    End Function

    Public Shared Function Exists(ByVal pCarrier As String, ByVal pTruck As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from VEHICLES where Carrier = '{0}' and truck = '{1}'", pCarrier, pTruck)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM VEHICLE Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Vehicle does not exists", "Vehicle does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("carrier") Then _carrier = dr.Item("carrier")
        If Not dr.IsNull("vehicleid") Then _truck = dr.Item("vehicleid")
        If Not dr.IsNull("vehicletype") Then _trucktype = dr.Item("vehicletype")
    End Sub

    Protected Sub LoadWithoutCarrier()
        Dim SQL As String = "SELECT * FROM VEHICLE Where " & WhereClauseWithoutCarrier
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Vehicle does not exists", "Vehicle does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("carrier") Then _carrier = dr.Item("carrier")
        If Not dr.IsNull("vehicleid") Then _truck = dr.Item("vehicleid")
        If Not dr.IsNull("vehicletype") Then _trucktype = dr.Item("vehicletype")
    End Sub

#End Region

    Public Sub Save()
        If Not WMS.Logic.Carrier.Exists(_carrier) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
            Throw m4nEx
        End If
        Dim sql As String
        If Vehicles.Exists(_carrier, _truck) Then
            sql = String.Format("Update VEHICLE set carrier = {0},vehicleid = {1},trucktype = {2}, editdate = {3}, edituser = {4} where " & WhereClause, _
                Made4Net.Shared.Util.FormatField(_carrier), Made4Net.Shared.Util.FormatField(_truck), Made4Net.Shared.Util.FormatField(_trucktype), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.VehicleUpdated
            em.Add("EVENT", EventType)
            em.Add("CARRIER", _carrier)
            em.Add("VEHICLE", _truck)
            em.Add("USERID", _adduser)
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))

        Else
            sql = "Insert into VEHICLES(carrier,truck,trucktype,ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ("
            sql += Made4Net.Shared.Util.FormatField(_carrier) & "," & _
               Made4Net.Shared.Util.FormatField(_truck) & "," & Made4Net.Shared.Util.FormatField(_trucktype) & "," & _
               Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
               Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.VehicleCreated
            em.Add("EVENT", EventType)
            em.Add("CARRIER", _carrier)
            em.Add("VEHICLE", _truck)
            em.Add("USERID", _adduser)
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))

        End If
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Move(ByVal location As String, ByVal pUser As String)
        _yardlocation = location
        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String = String.Format("Update VEHICLES SET yardlocation = {0},EditUser = {1},EditDate = {2} Where {3}", _
        Made4Net.Shared.Util.FormatField(_yardlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClauseWithoutCarrier)

        DataInterface.RunSQL(sql)

    End Sub

    'Added by priel
    Public Sub SetActivityStatus(ByVal paramStatus As String, ByVal pUser As String)
        _activitystatus = paramStatus
        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String = String.Format("Update VEHICLES SET ACTIVITYSTATUS = {0},EditUser = {1},EditDate = {2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class
