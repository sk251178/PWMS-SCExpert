Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports System.Text

'#Region "VEHICLEROUTE"

'<CLSCompliant(False)>  Public Class VehicleRoute

'#Region "Variables"

'#Region "Primary Keys"

'    Protected _vehiclerouteid As Int32 = -1

'#End Region

'#Region "Other Fields"

'    Protected _routestart As DateTime
'    Protected _routeend As DateTime
'    Protected _vehicleid As String
'    Protected _comment As String = String.Empty
'    Protected _predefined As Boolean
'    Protected _monday As Boolean
'    Protected _tuesday As Boolean
'    Protected _wednesday As Boolean
'    Protected _thursday As Boolean
'    Protected _friday As Boolean
'    Protected _saturday As Boolean
'    Protected _sunday As Boolean
'    Protected _adddate As DateTime
'    Protected _adduser As String = String.Empty
'    Protected _editdate As DateTime
'    Protected _edituser As String = String.Empty

'    ' the mappoints array representing the route
'    Dim _mappoints() As Integer

'#End Region

'#End Region

'#Region "Properties"

'    Public ReadOnly Property MapPoints() As Integer()
'        Get
'            Return _mappoints
'        End Get
'    End Property


'    Public ReadOnly Property WhereClause() As String
'        Get
'            Return " Where VEHICLEROUTEID = '" & _vehiclerouteid & "'"
'        End Get
'    End Property

'    Public ReadOnly Property VEHICLEROUTEID() As Int32
'        Get
'            Return _vehiclerouteid
'        End Get
'    End Property

'    Public Property ROUTESTART() As DateTime
'        Get
'            Return _routestart
'        End Get
'        Set(ByVal Value As DateTime)
'            _routestart = Value
'        End Set
'    End Property

'    Public Property ROUTEEND() As DateTime
'        Get
'            Return _routeend
'        End Get
'        Set(ByVal Value As DateTime)
'            _routeend = Value
'        End Set
'    End Property

'    Public Property VEHICLEID() As String
'        Get
'            Return _vehicleid
'        End Get
'        Set(ByVal Value As String)
'            _vehicleid = Value
'        End Set
'    End Property

'    Public Property COMMENT() As String
'        Get
'            Return _comment
'        End Get
'        Set(ByVal Value As String)
'            _comment = Value
'        End Set
'    End Property

'    Public Property PREDEFINED() As Boolean
'        Get
'            Return _predefined
'        End Get
'        Set(ByVal Value As Boolean)
'            _predefined = Value
'        End Set
'    End Property

'    Public Property MONDAY() As Boolean
'        Get
'            Return _monday
'        End Get
'        Set(ByVal Value As Boolean)
'            _monday = Value
'        End Set
'    End Property

'    Public Property TUESDAY() As Boolean
'        Get
'            Return _tuesday
'        End Get
'        Set(ByVal Value As Boolean)
'            _tuesday = Value
'        End Set
'    End Property

'    Public Property WEDNESDAY() As Boolean
'        Get
'            Return _wednesday
'        End Get
'        Set(ByVal Value As Boolean)
'            _wednesday = Value
'        End Set
'    End Property

'    Public Property THURSDAY() As Boolean
'        Get
'            Return _thursday
'        End Get
'        Set(ByVal Value As Boolean)
'            _thursday = Value
'        End Set
'    End Property

'    Public Property FRIDAY() As Boolean
'        Get
'            Return _friday
'        End Get
'        Set(ByVal Value As Boolean)
'            _friday = Value
'        End Set
'    End Property

'    Public Property SATURDAY() As Boolean
'        Get
'            Return _saturday
'        End Get
'        Set(ByVal Value As Boolean)
'            _saturday = Value
'        End Set
'    End Property

'    Public Property SUNDAY() As Boolean
'        Get
'            Return _sunday
'        End Get
'        Set(ByVal Value As Boolean)
'            _sunday = Value
'        End Set
'    End Property

'    Public Property ADDDATE() As DateTime
'        Get
'            Return _adddate
'        End Get
'        Set(ByVal Value As DateTime)
'            _adddate = Value
'        End Set
'    End Property

'    Public Property ADDUSER() As String
'        Get
'            Return _adduser
'        End Get
'        Set(ByVal Value As String)
'            _adduser = Value
'        End Set
'    End Property

'    Public Property EDITDATE() As DateTime
'        Get
'            Return _editdate
'        End Get
'        Set(ByVal Value As DateTime)
'            _editdate = Value
'        End Set
'    End Property

'    Public Property EDITUSER() As String
'        Get
'            Return _edituser
'        End Get
'        Set(ByVal Value As String)
'            _edituser = Value
'        End Set
'    End Property



'#End Region

'#Region "Constructors"

'    Public Sub New()
'    End Sub

'    Public Sub New(ByVal dr As DataRow)
'        SetObj(dr)
'    End Sub

'    Public Sub New(ByVal pVEHICLEROUTEID As Int32, Optional ByVal LoadObj As Boolean = True)
'        _vehiclerouteid = pVEHICLEROUTEID
'        If LoadObj Then
'            Load()
'        End If
'    End Sub

'#End Region

'#Region "Methods"

'    Public Shared Function GetVEHICLEROUTE(ByVal pVEHICLEROUTEID As Int32) As VehicleRoute
'        Return New VehicleRoute(pVEHICLEROUTEID)
'    End Function

'    Protected Sub Load()

'        Dim SQL As String = "SELECT * FROM VEHICLEROUTE " & WhereClause
'        Dim dt As New DataTable
'        Dim dr As DataRow
'        DataInterface.FillDataset(SQL, dt)
'        If dt.Rows.Count = 0 Then

'        End If

'        dr = dt.Rows(0)
'        SetObj(dr)

'        LoadMapPoints()

'    End Sub

'    Private Sub SetObj(ByRef dr As DataRow)
'        _vehiclerouteid = dr.Item("VEHICLEROUTEID")
'        If Not dr.IsNull("ROUTESTART") Then _routestart = dr.Item("ROUTESTART")
'        If Not dr.IsNull("ROUTEEND") Then _routeend = dr.Item("ROUTEEND")
'        If Not dr.IsNull("VEHICLEID") Then _vehicleid = dr.Item("VEHICLEID")
'        If Not dr.IsNull("COMMENT") Then _comment = dr.Item("COMMENT")
'        If Not dr.IsNull("PREDEFINED") Then _predefined = dr.Item("PREDEFINED")
'        If Not dr.IsNull("MONDAY") Then _monday = dr.Item("MONDAY")
'        If Not dr.IsNull("TUESDAY") Then _tuesday = dr.Item("TUESDAY")
'        If Not dr.IsNull("WEDNESDAY") Then _wednesday = dr.Item("WEDNESDAY")
'        If Not dr.IsNull("THURSDAY") Then _thursday = dr.Item("THURSDAY")
'        If Not dr.IsNull("FRIDAY") Then _friday = dr.Item("FRIDAY")
'        If Not dr.IsNull("SATURDAY") Then _saturday = dr.Item("SATURDAY")
'        If Not dr.IsNull("SUNDAY") Then _sunday = dr.Item("SUNDAY")
'        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
'        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
'        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
'        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
'    End Sub

'    Private Sub LoadMapPoints()
'        Dim sql As String = "SELECT COUNT(*) FROM VEHICLEROUTEPOINTS WHERE VEHICLEROUTEID=" + _vehiclerouteid.ToString()

'        Dim count As Integer = DataInterface.ExecuteScalar(sql)
'        If count > 0 Then
'            Dim dt As New DataTable
'            Dim i As Integer
'            Dim dr As DataRow

'            _mappoints = New Integer(count - 1) {}
'            sql = "SELECT * FROM VEHICLEROUTEPOINTS WHERE VEHICLEROUTEID=" + _vehiclerouteid.ToString() + " ORDER BY [DATE]"
'            DataInterface.FillDataset(sql, dt)

'            For i = 0 To dt.Rows.Count - 1
'                dr = dt.Rows(i)
'                _mappoints(i) = dr.Item("MAPPOINTID")
'            Next
'        End If
'    End Sub

'    Public Function ToXml(ByRef sb As StringBuilder) As StringBuilder
'        sb.Append("<route routeid=""" & VEHICLEROUTEID.ToString() & """ vehicleid=""" & VEHICLEID.ToString() & """")
'        sb.Append(" start=""" & ROUTESTART.ToString() & """ end=""" & ROUTEEND.ToString() & """")
'        sb.Append(" comment=""" & COMMENT.ToString() & """>")
'        sb.Append("<routepoints>")
'        ' lets add the points and coordinates
'        Dim i As Integer
'        For i = 0 To _mappoints.Length - 1
'            Dim mp As MapPoint = MapPoint.GetPoint(_mappoints(i))
'            sb.Append("<point id=""" & mp.POINTID.ToString() & """ latitude=""" & mp.LATITUDE.ToString() & """ longitude=""" & mp.LONGITUDE.ToString() & """/>")
'            sb.Append(ControlChars.Lf)
'        Next
'        sb.Append("</routepoints>")
'        sb.Append("</route>")
'        sb.Append(ControlChars.Lf)
'        Return sb
'    End Function


'#End Region
'End Class
'#End Region

'#Region "VEHICLEROUTECOLLECTION"
'<CLSCompliant(False)>  Public Class VehicleRouteCollection
'    Inherits ArrayList
'#Region "Properties"
'    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As VehicleRoute
'        Get
'            Return CType(MyBase.Item(index), VehicleRoute)
'        End Get
'    End Property
'#End Region
'#Region "Constructors"
'    Public Sub New()
'    End Sub
'#End Region

'#Region "Methods"
'    Public Sub LoadPredefinedRoutes(ByVal vehicleid As Int32)
'        Dim sql As String = "SELECT * FROM VEHICLEROUTE WHERE PREDEFINED=1 AND VEHICLEID = " + vehicleid.ToString()
'        Dim dt As New DataTable
'        Dim dr As DataRow
'        DataInterface.FillDataset(sql, dt)
'        For Each dr In dt.Rows
'            Me.Add(New VehicleRoute(dr))
'        Next
'    End Sub
'    Public Function GetPredefinedForDay(ByVal forday As DateTime) As VehicleRoute
'        Dim vsp As VehicleRoute
'        Dim weekday As DayOfWeek = forday.DayOfWeek
'        For Each vsp In Me
'            If vsp.PREDEFINED Then
'                Dim found = False

'                Select Case weekday
'                    Case DayOfWeek.Sunday
'                        found = vsp.SUNDAY
'                    Case DayOfWeek.Monday
'                        found = vsp.MONDAY
'                    Case DayOfWeek.Tuesday
'                        found = vsp.TUESDAY
'                    Case DayOfWeek.Wednesday
'                        found = vsp.WEDNESDAY
'                    Case DayOfWeek.Thursday
'                        found = vsp.THURSDAY
'                    Case DayOfWeek.Friday
'                        found = vsp.FRIDAY
'                    Case DayOfWeek.Saturday
'                        found = vsp.SATURDAY
'                End Select

'                If found Then
'                    Return vsp
'                End If
'            End If
'        Next
'        Return Nothing
'    End Function
'#End Region
'End Class
'#End Region

'#Region "VEHICLEROUTECACHE"
'<CLSCompliant(False)>  Public Class VehicleRouteHashtable
'    Inherits Hashtable

'#Region "PROPERTIES"
'    Default Public Shadows ReadOnly Property Item(ByVal key As Object) As VehicleRoute
'        Get
'            Return CType(MyBase.Item(key), VehicleRoute)
'        End Get
'    End Property
'#End Region

'#Region "METHODS"
'    Public Overloads Sub Add(ByVal obj As VehicleRoute)
'        MyBase.Add(obj.VEHICLEROUTEID, obj)
'    End Sub
'#End Region
'End Class

'#End Region
