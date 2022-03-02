Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "VEHICLE"

<CLSCompliant(False)> Public Class Vehicle

#Region "Variables"

#Region "Primary Keys"

    Protected _vehicleid As Int32

#End Region

#Region "Other Fields"

    Protected _vehicletypeid As String
    Protected _driverid As Int32 = -1
    Protected _name As String = String.Empty
    Protected _comment As String = String.Empty
    Protected _adduser As String = String.Empty
    Protected _adddate As DateTime
    Protected _edituser As String = String.Empty
    Protected _editdate As DateTime

    ' the vehicle type object - representing various parameteres of the vehicle
    Protected _type As VehicleType
    ' the teritoryid to which the vehicle can go 
    Protected _territoryid As Int32 = -1
    '
    'Protected _predefnedRoutes As VehicleRouteCollection
    'Protected _predefnedRoutes As RouteCollection
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property TerritoryID() As Int32
        Get
            Return _territoryid
        End Get
    End Property

    'Public ReadOnly Property PredefinedRoutes() As RouteCollection
    '    Get
    '        Return _predefnedRoutes
    '    End Get
    'End Property


    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where VEHICLEID = '" & _vehicleid & "'"
        End Get
    End Property

    Public ReadOnly Property VEHICLEID() As Int32
        Get
            Return _vehicleid
        End Get
    End Property

    Public Property VEHICLETYPEID() As Int32
        Get
            Return _vehicletypeid
        End Get
        Set(ByVal Value As Int32)
            _vehicletypeid = Value
        End Set
    End Property

    Public Property DRIVERID() As Int32
        Get
            Return _driverid
        End Get
        Set(ByVal Value As Int32)
            _driverid = Value
        End Set
    End Property

    Public Property NAME() As String
        Get
            Return _name
        End Get
        Set(ByVal Value As String)
            _name = Value
        End Set
    End Property

    Public Property COMMENT() As String
        Get
            Return _comment
        End Get
        Set(ByVal Value As String)
            _comment = Value
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

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
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

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property


#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pVEHICLEID As Int32, Optional ByVal LoadObj As Boolean = True)
        _vehicleid = pVEHICLEID
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByRef dr As DataRow)
        SetObj(dr)
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetVEHICLE(ByVal pVEHICLEID As Int32) As Vehicle
        Return New Vehicle(pVEHICLEID)
    End Function


    Private Sub SetObj(ByRef dr As DataRow)
        _vehicleid = dr.Item("VEHICLEID")
        If Not dr.IsNull("VEHICLETYPENAME") Then _vehicletypeid = dr.Item("VEHICLETYPENAME")
        If Not dr.IsNull("DRIVERID") Then _driverid = dr.Item("DRIVERID")
        If Not dr.IsNull("NAME") Then _name = dr.Item("NAME")
        If Not dr.IsNull("COMMENT") Then _comment = dr.Item("COMMENT")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")

        _type = New VehicleType(_vehicletypeid)

        ' if we have a driver - this may restruct the teritories to which the truck can go
        Try
            If _driverid <> -1 Then
                Dim SQL = "SELECT * FROM VEHICLEDRIVER WHERE DRIVERID = " + _driverid.ToString()
                Dim dt As DataTable = New DataTable
                Dim drLocal As DataRow
                DataInterface.FillDataset(SQL, dt)
                If dt.Rows.Count = 0 Then
                    Throw New LogicException("Unknown DRIVER. Check the referential integruty of the database", Me, "Load", 1001)
                End If
                drLocal = dt.Rows(0)
                If Not drLocal.IsNull("TERRITORYID") Then _territoryid = drLocal.Item("TERRITORYID")
            End If
        Catch ex As Exception
        End Try
    End Sub


    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM VEHICLE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New LogicException("Vehicle Item Does Not Exists", Me, "Load", 1001)
        End If

        dr = dt.Rows(0)

        SetObj(dr)
    End Sub

#End Region

End Class

#End Region

#Region "VEHICLECOLLECTION"
<CLSCompliant(False)> Public Class VehicleCollection
    Inherits ArrayList

#Region "PROPERTIES"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As Vehicle
        Get
            Return CType(MyBase.Item(index), Vehicle)
        End Get
    End Property

#End Region

#Region "CONSTRUCTORS"
    Sub New(Optional ByVal toLoad As Boolean = True)
        If toLoad Then LoadAll()
    End Sub
#End Region

#Region "METHODS"
    Private Sub LoadAll()
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        Dim sql As String = "SELECT * FROM VEHICLES"

        DataInterface.FillDataset(sql, dt)

        For Each dr In dt.Rows
            Me.Add(New Vehicle(dr))
        Next
    End Sub
    Public Sub LoadFromList(ByVal ids As ArrayList)
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        Dim sql As String = "SELECT * FROM VEHICLES WHERE VEHICLEID IN("

        Dim o As Object
        For Each o In ids
            sql += o.ToString() + ","
        Next

        Dim ncom = sql.LastIndexOf(",")
        If ncom > 0 Then
            sql = sql.Remove(sql.Length - 1, 1)
        End If

        sql += ")"

        DataInterface.FillDataset(sql, dt)

        For Each dr In dt.Rows
            Me.Add(New Vehicle(dr))
        Next
    End Sub
#End Region

End Class


#End Region

