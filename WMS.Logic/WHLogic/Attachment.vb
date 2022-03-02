Imports System.Data
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.IO
Imports System.Drawing

<CLSCompliant(False)> Public Class Attachment

#Region "Variables"

    Protected _documentid As String = String.Empty
    Protected _documenttype As String
    Protected _documentdata() As Byte
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"

    Public Property DOCUMENTID() As String
        Get
            Return _documentid
        End Get
        Set(ByVal Value As String)
            _documentid = Value
        End Set
    End Property

    Public Property DOCUMENTTYPE() As String
        Get
            Return _documenttype
        End Get
        Set(ByVal Value As String)
            _documenttype = Value
        End Set
    End Property

    Public Property DOCUMENTDATA() As Byte()
        Get
            Return _documentdata
        End Get
        Set(ByVal Value As Byte())
            _documentdata = Value
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

#End Region

#Region "Methods"

    Public Sub Create(ByVal pDocId As String, ByVal pDocType As String, ByVal pDocData() As Byte, ByVal pUserId As String)
        _documentid = Made4Net.Shared.Util.getNextCounter("ATTACHMENT")
        _documenttype = pDocType
        _documentdata = pDocData
        _adddate = DateTime.Now
        _adduser = pUserId
        _editdate = DateTime.Now
        _edituser = pUserId
        InsertData()
    End Sub

    Private Sub InsertData()
        Dim oConn As System.Data.IDbConnection = Made4Net.DataAccess.DataInterface.GetConnection()
        Dim sql As String = String.Format("INSERT INTO ATTACHMENT (DOCUMENTID, DOCUMENTTYPE, DOCUMENTDATA, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},@DOCUMENTDATA,{2},{3},{4},{5})", _
                FormatField(_documentid), FormatField(_documenttype), FormatField(_adddate), FormatField(_adduser), FormatField(_editdate), FormatField(_edituser))
        Dim oCom As System.Data.IDbCommand = oConn.CreateCommand
        oCom.Connection = oConn
        oCom.CommandText = sql
        AddParameter(_documentdata, "@DOCUMENTDATA", oCom)
        'oCom.Parameters.Add("@DOCUMENTDATA").Value = _documentdata
        oCom.ExecuteNonQuery()
    End Sub
    Private Sub AddParameter(ByVal data As Byte(), ByVal name As String, ByVal comm As IDbCommand)

        If TypeOf comm Is System.Data.SqlClient.SqlCommand Then

            AddParameterSQL(data, name, CType(comm, System.Data.SqlClient.SqlCommand))

        Else

            AddParameterODBC(data, name, CType(comm, System.Data.Odbc.OdbcCommand))

        End If
    End Sub
    Private Sub AddParameterODBC(ByVal data As Byte(), ByVal name As String, ByVal comm As System.Data.Odbc.OdbcCommand)

        comm.Parameters.Add(name, System.Data.Odbc.OdbcType.Image).Value = data
    End Sub
    Private Sub AddParameterSQL(ByVal data As Byte(), ByVal name As String, ByVal comm As System.Data.SqlClient.SqlCommand)

        comm.Parameters.Add(name, System.Data.SqlDbType.Image).Value = data
    End Sub
#End Region

End Class
