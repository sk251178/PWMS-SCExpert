Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteStopTaskPackages

#Region "Variables"

    Protected _routeid As String
    Protected _stopnumber As Int32
    Protected _stoptaskid As Int32
    Protected _packageid As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    Protected _package As RoutePackage

#End Region

#Region "Properties"

    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" routeid = {0} and stopnumber = {1} and stoptaskid = {2} and packageid = {3}", Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(_packageid))
        End Get
    End Property

    Public Property RouteID() As String
        Set(ByVal Value As String)
            _routeid = Value
        End Set
        Get
            Return _routeid
        End Get
    End Property

    Public Property StopNumber() As Int32
        Get
            Return _stopnumber
        End Get
        Set(ByVal Value As Int32)
            _stopnumber = Value
        End Set
    End Property

    Public Property StopTaskId() As Int32
        Get
            Return _stoptaskid
        End Get
        Set(ByVal Value As Int32)
            _stoptaskid = Value
        End Set
    End Property

    Public Property PackageId() As String
        Get
            Return _packageid
        End Get
        Set(ByVal Value As String)
            _packageid = Value
        End Set
    End Property

    Public ReadOnly Property Package() As RoutePackage
        Get
            Return _package
        End Get
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

#End Region

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pRouteId As String, ByVal pStopnumber As Int32, ByVal pStopTaskId As Int32, ByVal pPackageId As String)
        _routeid = pRouteId
        _stopnumber = pStopnumber
        _stoptaskid = pStopTaskId
        _packageid = pPackageId
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal sRouteId As String, ByVal iStopNumber As Int32, ByVal iStopTaskId As Int32, ByVal sPackageId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from routestoptaskpackages where routeid = {0} and stopnumber = {1} and stoptaskid = {2} and packageid = {3}", Made4Net.Shared.Util.FormatField(sRouteId), Made4Net.Shared.Util.FormatField(iStopNumber), Made4Net.Shared.Util.FormatField(iStopTaskId), Made4Net.Shared.Util.FormatField(sPackageId))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim sql As String = String.Format("SELECT * FROM routestoptaskpackages where " & WhereClause)
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Package Not Found")
        End If
        dr = dt.Rows(0)
        If Not IsDBNull(dr("adddate")) Then _adddate = dr("adddate")
        If Not IsDBNull(dr("adduser")) Then _adduser = dr("adduser")
        If Not IsDBNull(dr("editdate")) Then _editdate = dr("editdate")
        If Not IsDBNull(dr("edituser")) Then _edituser = dr("edituser")

        'Load the package object
        _package = New RoutePackage(_packageid)
    End Sub

#End Region

#Region "Create & Delete"

    Public Sub Create(ByVal pRouteId As String, ByVal pStopNumber As Int32, ByVal pStopTaskId As Int32, ByVal pPackageId As String, ByVal pUserId As String)
        If RouteStopTaskPackages.Exists(pRouteId, pStopNumber, pStopTaskId, pPackageId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Stop Task Package - Route Stop Package already exists", "Cannot create Stop Task Package - Route Stop Package already exists")
        End If
        If Not RoutePackage.Exists(pPackageId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Stop Task Package - Package does not exists", "Cannot create Stop Task Package - Package does not exists")
        End If
        Dim SQL As String
        _routeid = pRouteId
        _stopnumber = pStopNumber
        _stoptaskid = pStopTaskId
        _packageid = pPackageId
        _adddate = DateTime.Now
        _adduser = pUserId
        _editdate = DateTime.Now
        _edituser = pUserId

        SQL = String.Format("INSERT INTO ROUTESTOPTASKPACKAGES (ROUTEID, STOPNUMBER, STOPTASKID, PACKAGEID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(_packageid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Delete(ByVal pUserId As String)
        If Not RouteStopTaskPackages.Exists(_routeid, _stopnumber, _stoptaskid, _packageid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot delete Stop Task Package - Package does not exists", "Cannot delete Stop Task Package - Package does not exists")
        End If
        If _package.Status = WMS.Lib.Statuses.RoutePackages.DELIVERED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot delete Stop Task Package - Package status Incorrect", "Cannot delete Stop Task Package - Package status Incorrect")
        End If
        _package.SetStatus(WMS.Lib.Statuses.RoutePackages.OFFLOADED, pUserId)
        'And delete the stop package object
        Dim SQL As String = String.Format("delete from ROUTESTOPTASKPACKAGES where {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#End Region

End Class
