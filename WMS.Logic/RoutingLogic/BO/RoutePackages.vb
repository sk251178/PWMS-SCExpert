Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RoutePackage

#Region "Variables"

    Protected _packageid As String
    Protected _packagetype As String
    Protected _documentid As String
    Protected _documenttype As String
    Protected _consignee As String
    Protected _status As String
    Protected _statusdate As DateTime
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    'Protected _document As Object
    'Protected _contact As Contact

#End Region

#Region "Properties"

    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" packageid = {0}", Made4Net.Shared.Util.FormatField(_packageid))
        End Get
    End Property

    Public Property PackageId() As String
        Get
            Return _packageid
        End Get
        Set(ByVal Value As String)
            _packageid = Value
        End Set
    End Property

    Public Property PackageType() As String
        Get
            Return _packagetype
        End Get
        Set(ByVal Value As String)
            _packagetype = Value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property DocumentId() As String
        Get
            Return _documentid
        End Get
        Set(ByVal Value As String)
            _documentid = Value
        End Set
    End Property

    Public Property DocumentType() As String
        Get
            Return _documenttype
        End Get
        Set(ByVal Value As String)
            _documenttype = Value
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

    Public Property StatusDate() As DateTime
        Get
            Return _statusdate
        End Get
        Set(ByVal Value As DateTime)
            _statusdate = Value
        End Set
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

    'Public ReadOnly Property DocumentProperty(ByVal pPropertyName As String) As Object
    '    Get
    '        Try
    '            Return CallByName(_document, pPropertyName, CallType.Get, Nothing)
    '        Catch ex As Exception
    '        End Try
    '        Return Nothing
    '    End Get
    'End Property

    'Public ReadOnly Property Contact() As Contact
    '    Get
    '        Try
    '            If _contact Is Nothing Then
    '                Select Case _documenttype
    '                    Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
    '                        _contact = DocumentProperty("SHIPTO")
    '                    Case WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
    '                        _contact = DocumentProperty("RECEIVEDFROM")
    '                    Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
    '                        _contact = DocumentProperty("SHIPTO")
    '                    Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
    '                        _contact = DocumentProperty("SHIPTO")
    '                End Select
    '            End If
    '            Return _contact
    '        Catch ex As Exception
    '            Return Nothing
    '        End Try
    '    End Get
    'End Property

#End Region

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pPackageId As String)
        _packageid = pPackageId
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal pPackageId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from routepackages where packageid = {0}", Made4Net.Shared.Util.FormatField(pPackageId))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim sql As String = String.Format("SELECT * FROM ROUTEPACKAGES where " & WhereClause)
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Package Not Found")
        End If
        dr = dt.Rows(0)
        If Not IsDBNull(dr("consignee")) Then _consignee = dr("consignee")
        If Not IsDBNull(dr("documentid")) Then _documentid = dr("documentid")
        If Not IsDBNull(dr("documenttype")) Then _documenttype = dr("documenttype")
        If Not IsDBNull(dr("packagetype")) Then _packagetype = dr("packagetype")
        If Not IsDBNull(dr("status")) Then _status = dr("status")
        If Not IsDBNull(dr("statusdate")) Then _statusdate = dr("statusdate")
        If Not IsDBNull(dr("adddate")) Then _adddate = dr("adddate")
        If Not IsDBNull(dr("adduser")) Then _adduser = dr("adduser")
        If Not IsDBNull(dr("editdate")) Then _editdate = dr("editdate")
        If Not IsDBNull(dr("edituser")) Then _edituser = dr("edituser")

        'Try
        '    LoadDocument()
        'Catch ex As Exception
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Package Document does not exist", "Package Document does not exist")
        'End Try
    End Sub

    'Protected Sub LoadDocument()
    '    Select Case _documenttype
    '        Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
    '            _document = New OutboundOrderHeader(_consignee, _documentid)
    '        Case WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
    '            _document = New InboundOrderHeader(_consignee, _documentid)
    '        Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
    '            _document = New Flowthrough(_consignee, _documentid)
    '        Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
    '            _document = New TransShipment(_consignee, _documentid)
    '    End Select
    'End Sub

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUserId As String)
        Dim SQL As String
        _edituser = pUserId
        _editdate = DateTime.Now
        _status = pStatus
        _statusdate = DateTime.Now
        SQL = String.Format("update routepackages set status={0}, statusdate={1}, editdate={2}, edituser={3} where {4}", Made4Net.Shared.Util.FormatField(_status), _
                Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Create / Update"

    Public Sub Create(ByVal pPackageId As String, ByVal pPackageType As String, ByVal pDocumentType As String, ByVal pDocumentId As String, ByVal pConsignee As String, ByVal pUserId As String)
        If RoutePackage.Exists(pPackageId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Route Package - Package Already exists", "Cannot create Route Package - Package Already exists")
        End If
        Dim SQL As String
        If pPackageId = String.Empty Then
            _packageid = Made4Net.Shared.Util.getNextCounter("ROUTEPACKAGE")
        Else
            _packageid = pPackageId
        End If
        _packagetype = pPackageType
        _documenttype = pDocumentType
        _documentid = pDocumentId
        _consignee = pConsignee
        _status = WMS.Lib.Statuses.RoutePackages.[NEW]
        _statusdate = DateTime.Now
        _adddate = DateTime.Now
        _adduser = pUserId
        _editdate = DateTime.Now
        _edituser = pUserId

        SQL = String.Format("INSERT INTO ROUTEPACKAGES(PACKAGEID, PACKAGETYPE, DOCUMENTTYPE, CONSIGNEE, DOCUMENTID, STATUS, STATUSDATE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _
            Made4Net.Shared.Util.FormatField(_packageid), Made4Net.Shared.Util.FormatField(_packagetype), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_documentid), _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Update(ByVal pPackageType As String, ByVal pDocumentType As String, ByVal pDocumentId As String, ByVal pConsignee As String, ByVal pStatus As String, ByVal pUserId As String)
        If Not RoutePackage.Exists(_packageid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot update Route Package - Package does not exists", "Cannot update Route Package - Package does not exists")
        End If
        Dim SQL As String
        _packagetype = pPackageType
        _documenttype = pDocumentType
        _documentid = pDocumentId
        _consignee = pConsignee
        _status = pStatus
        _statusdate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = pUserId

        SQL = String.Format("UPDATE ROUTEPACKAGES SET PACKAGEID={0}, PACKAGETYPE={1}, DOCUMENTTYPE={2}, CONSIGNEE={3}, DOCUMENTID={4}, STATUS={5}, STATUSDATE={6}, EDITDATE={7}, EDITUSER={8} where {9}", _
            Made4Net.Shared.Util.FormatField(_packageid), Made4Net.Shared.Util.FormatField(_packagetype), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_documentid), _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Deliver / Return"

    Public Sub Deliver(ByVal pUserId)
        SetStatus(WMS.Lib.Statuses.RoutePackages.DELIVERED, pUserId)
    End Sub
    Public Sub DeliverPartial(ByVal pUserId)
        SetStatus(WMS.Lib.Statuses.RoutePackages.DELIVEREDPARTIAL, pUserId)
    End Sub
    Public Sub OffLoad(ByVal pUserId)
        SetStatus(WMS.Lib.Statuses.RoutePackages.OFFLOADED, pUserId)
    End Sub

    Public Sub Load(ByVal pUserId)
        SetStatus(WMS.Lib.Statuses.RoutePackages.LOADED, pUserId)
    End Sub

    Public Sub PickUp(ByVal pUserId)
        SetStatus(WMS.Lib.Statuses.RoutePackages.PICKEDUP, pUserId)
    End Sub

#End Region

#End Region

End Class
