Public Class MheEdgeType


#Region "Variables"

#Region "Primary Keys"
    Private _mhetype As String
    Private _edgetype As String
#End Region

#Region "Other Fields"

    Private _addUser As String
    Private _editUser As String
    Private _addDate As DateTime
    Private _editDate As DateTime
#End Region

#End Region

#Region "Properties"



    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("MHETYPE = '{0}' and EDGETYPE='{1}'", _mhetype, _edgetype)
        End Get
    End Property

    Public Property MHETYPE() As String
        Get
            Return _mhetype
        End Get
        Set(ByVal Value As String)
            _mhetype = Value
        End Set
    End Property

    Public Property EDGETYPE() As String
        Get
            Return _edgetype
        End Get
        Set(ByVal Value As String)
            _edgetype = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _addDate
        End Get
        Set(ByVal Value As DateTime)
            _addDate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _addUser
        End Get
        Set(ByVal Value As String)
            _addUser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editDate
        End Get
        Set(ByVal Value As DateTime)
            _editDate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _editUser
        End Get
        Set(ByVal Value As String)
            _editUser = Value
        End Set
    End Property

#End Region

    Public Shared Function Exists(ByVal pMHEType As String, ByVal pEdgeType As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from MHEEDGETYPE where MHETYPE = '{0}' and EDGETYPE='{1}'", pMHEType, pEdgeType)
        Return Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Create(ByVal pMHEType As String, ByVal pEdgeType As String, ByVal pUser As String)
        _mhetype = pMHEType
        _edgetype = pEdgeType
        _addUser = pUser
        _editUser = pUser

        If Exists(_mhetype, _edgetype) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create handling equipment edge type. It already exists.", "Can not create handling equipment edge type. It already exists.")
        End If
        Save()
    End Sub

    Public Sub Save()
        _editDate = DateTime.Now
        _addDate = DateTime.Now
        Dim sql As String

        sql = String.Format("Insert into MHEEDGETYPE values({0},{1},{2},{3},{4},{5})", Made4Net.Shared.FormatField(_mhetype), _
        Made4Net.Shared.FormatField(_edgetype), Made4Net.Shared.FormatField(_addDate), Made4Net.Shared.FormatField(_addUser), _
        Made4Net.Shared.FormatField(_editDate), Made4Net.Shared.FormatField(_editUser))
        Made4Net.DataAccess.DataInterface.RunSQL(sql)

    End Sub


End Class
