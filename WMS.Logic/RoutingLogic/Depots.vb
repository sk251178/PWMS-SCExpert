Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util

<CLSCompliant(False)> Public Class Depots

#Region "Variables"

#Region "Primary Keys"

    Protected _depotname As String

#End Region

#Region "Other Fields"

    Protected _description As String = String.Empty
    Protected _depottype As String = String.Empty
    Protected _contactid As String = String.Empty
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
            Return " DEPOTNAME = '" & _depotname & "'"
        End Get
    End Property

    Public ReadOnly Property DEPOTNAME() As String
        Get
            Return _depotname
        End Get
    End Property

    Public ReadOnly Property POINTID() As String
        Get
            Return _pointid
        End Get
    End Property

    Public Property DESCRIPTION() As String
        Get
            Return _description
        End Get
        Set(ByVal Value As String)
            _description = Value
        End Set
    End Property

    Public Property DEPOTTYPE() As String
        Get
            Return _depottype
        End Get
        Set(ByVal Value As String)
            _depottype = Value
        End Set
    End Property

    Public Property CONTACTID() As String
        Get
            Return _contactid
        End Get
        Set(ByVal Value As String)
            _contactid = Value
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

    Public Sub New(ByVal pDepotName As String, Optional ByVal LoadObj As Boolean = True)
        _depotname = pDepotName
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

    Public Sub Insert(ByVal pDepotName As String, ByVal pDepoType As String, ByVal pDescription As String, ByVal pUser As String)
        If Depots.Exists(pDepotName) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "DEPOT Already Exists", "DEPOT Already Exists")
        End If
        _depotname = pDepotName
        _depottype = pDepoType
        _description = pDescription
        _contactid = Made4Net.Shared.Util.getNextCounter("CONTACTID")
        _editdate = DateTime.Now
        _edituser = pUser
        _adddate = DateTime.Now
        _adduser = pUser

        Dim sql As String = String.Format("INSERT INTO DEPOT (DEPOTNAME, DESCRIPTION, DEPOTTYPE, CONTACT, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                    "VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", Made4Net.Shared.Util.FormatField(_depotname), Made4Net.Shared.Util.FormatField(_description), Made4Net.Shared.Util.FormatField(_depottype), _
                    Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Edit(ByVal pDepoType As String, ByVal pDescription As String, ByVal pUser As String)
        If Not Depots.Exists(_depotname) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "DEPOT Already Exists", "DEPOT Already Exists")
        End If
        _depottype = pDepoType
        _description = pDescription
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE DEPOT SET DESCRIPTION ={0}, DEPOTTYPE ={1}, EDITDATE ={2}, EDITUSER ={3} where {4}", Made4Net.Shared.Util.FormatField(_description), Made4Net.Shared.Util.FormatField(_depottype), _
                    Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(sql)
    End Sub

    Public Shared Function GetDepot(ByVal pDepotName As String) As Depots
        Return New Depots(pDepotName)
    End Function

    Public Shared Function Exists(ByVal pDepotName As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM DEPOT WHERE DEPOTNAME = '{0}'", pDepotName)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Sub SetObj(ByRef dr As DataRow)
        _depotname = dr.Item("DEPOTNAME")
        If Not dr.IsNull("DESCRIPTION") Then _description = dr.Item("DESCRIPTION")
        If Not dr.IsNull("DEPOTTYPE") Then _depottype = dr.Item("DEPOTTYPE")
        If Not dr.IsNull("CONTACT") Then _contactid = dr.Item("CONTACT")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If _contactid <> "" Then
            _pointid = DataInterface.ExecuteScalar(String.Format("select isNull(pointid,'')  from contact where contactid = '{0}'", _contactid))
        End If
    End Sub

    Protected Sub Load()
        Dim SQL As String = "SELECT * from DEPOT WHERE" & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "DEPOT Does Not Exists", "DEPOT Does Not Exists")
        End If
        dr = dt.Rows(0)
        SetObj(dr)
    End Sub

    Public Sub SetPoint(ByVal PointId As String, ByVal puser As String)
        If PointId Is Nothing Or PointId = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Map Point does not exists", "Map Point does not exists")
        End If
        If Not Contact.Exists(_contactid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Contact does not exists", "Contact does not exists")
        End If
        Dim oCont As New Contact(_contactid)
        oCont.setPointId(PointId, puser)
    End Sub

#End Region

End Class
