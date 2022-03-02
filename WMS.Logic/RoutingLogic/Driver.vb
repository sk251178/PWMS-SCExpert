Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess


<CLSCompliant(False)> Public Class Driver

#Region "Variables"

#Region "Primary Keys"

    Protected _driverid As String

#End Region

#Region "Other Fields"

    Protected _firstname As String = String.Empty
    Protected _lastname As String = String.Empty
    Protected _fullname As String = String.Empty
    Protected _birthdate As DateTime
    Protected _territoryid As String = String.Empty
    Protected _comments As String = String.Empty
    Protected _pointid As String = String.Empty
    Protected _adduser As String = String.Empty
    Protected _adddate As DateTime
    Protected _edituser As String = String.Empty
    Protected _editdate As DateTime

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where DRIVERID = '" & _driverid & "'"
        End Get
    End Property

    Public Property DRIVERID() As String
        Get
            Return _driverid
        End Get
        Set(ByVal Value As String)
            _driverid = Value
        End Set
    End Property

    Public Property FIRSTNAME() As String
        Get
            Return _firstname
        End Get
        Set(ByVal Value As String)
            _firstname = Value
        End Set
    End Property

    Public Property LASTNAME() As String
        Get
            Return _lastname
        End Get
        Set(ByVal Value As String)
            _lastname = Value
        End Set
    End Property

    Public Property FULLNAME() As String
        Get
            Return _fullname
        End Get
        Set(ByVal Value As String)
            _fullname = Value
        End Set
    End Property

    Public Property BIRTHDATE() As DateTime
        Get
            Return _birthdate
        End Get
        Set(ByVal Value As DateTime)
            _birthdate = Value
        End Set
    End Property

    Public Property TERRITORYID() As String
        Get
            Return _territoryid
        End Get
        Set(ByVal Value As String)
            _territoryid = Value
        End Set
    End Property

    Public Property COMMENTS() As String
        Get
            Return _comments
        End Get
        Set(ByVal Value As String)
            _comments = Value
        End Set
    End Property

    Public Property POINTID() As String
        Get
            Return _pointid
        End Get
        Set(ByVal Value As String)
            _pointid = Value
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

    Public Sub New(ByVal pDriverId As String, Optional ByVal LoadObj As Boolean = True)
        _driverid = pDriverId
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName.ToLower = "setpoint" Then
            _driverid = ds.Tables(0).Rows(0)("driverid")
            Load()
            If ds.Tables(0).Rows(0)("newstartpointid") Is DBNull.Value Then
                Throw New Made4Net.Shared.M4NException
            End If
            SetStartingPoint(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("newstartpointid")), Common.GetCurrentUser)
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exist(ByVal pDriverId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from VEHICLEDRIVER where driverid = '{0}'", pDriverId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetDriver(ByVal pDriverId As String) As Driver
        Return New Driver(pDriverId)
    End Function

    Private Sub SetObj(ByRef dr As DataRow)
        _driverid = dr.Item("DRIVERID")
        If Not dr.IsNull("FIRSTNAME") Then _firstname = dr.Item("FIRSTNAME")
        If Not dr.IsNull("LASTNAME") Then _lastname = dr.Item("LASTNAME")
        If Not dr.IsNull("FULLNAME") Then _fullname = dr.Item("FULLNAME")
        If Not dr.IsNull("BIRTHDATE") Then _birthdate = dr.Item("BIRTHDATE")
        If Not dr.IsNull("TERRITORYID") Then _territoryid = dr.Item("TERRITORYID")
        If Not dr.IsNull("POINTID") Then _pointid = dr.Item("POINTID")
        If Not dr.IsNull("COMMENTS") Then _comments = dr.Item("COMMENTS")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
    End Sub

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM VEHICLEDRIVER " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New LogicException("Driver Does Not Exists", Me, "Load", 1001)
        End If

        dr = dt.Rows(0)
        SetObj(dr)
    End Sub

#End Region

#Region "Save"

    Public Sub Save()
        Dim SQL As String
        If Exist(_driverid) Then
            SQL = String.Format("UPDATE VEHICLEDRIVER SET FIRSTNAME ={0}, LASTNAME ={1}, FULLNAME ={2}, BIRTHDATE ={3}, TERRITORYID ={4}, COMMENTS ={5}, POINTID ={6}, EDITDATE ={7}, EDITUSER ={8} {9}", _
                 Made4Net.Shared.Util.FormatField(_firstname), Made4Net.Shared.Util.FormatField(_lastname), Made4Net.Shared.Util.FormatField(_fullname), Made4Net.Shared.Util.FormatField(_birthdate), _
                Made4Net.Shared.Util.FormatField(_territoryid), Made4Net.Shared.Util.FormatField(_comments), Made4Net.Shared.Util.FormatField(_pointid), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        Else
            SQL = String.Format("INSERT INTO VEHICLEDRIVER (DRIVERID, FIRSTNAME, LASTNAME, FULLNAME, BIRTHDATE, TERRITORYID, COMMENTS, POINTID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) Values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _
               Made4Net.Shared.Util.FormatField(_driverid), Made4Net.Shared.Util.FormatField(_firstname), Made4Net.Shared.Util.FormatField(_lastname), Made4Net.Shared.Util.FormatField(_fullname), Made4Net.Shared.Util.FormatField(_birthdate), Made4Net.Shared.Util.FormatField(_territoryid), _
               Made4Net.Shared.Util.FormatField(_comments), Made4Net.Shared.Util.FormatField(_pointid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

    Public Sub SetStartingPoint(ByVal PointId As String, ByVal puser As String)
        If PointId Is Nothing Or PointId = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Map Point does not exists", "Map Point does not exists")
        End If
        _pointid = PointId
        _edituser = puser
        _editdate = DateTime.Now
        Dim sql As String = String.Format("Update VehicleDriver set pointid={0},edituser={1},editdate={2} {3}", Made4Net.Shared.Util.FormatField(_pointid), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class
