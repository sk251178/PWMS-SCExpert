Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class TransShipment

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _transshipment As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _ordertype As String = String.Empty
    Protected _referenceord As String = String.Empty
    Protected _sourcecompany As String = String.Empty
    Protected _sourcecompanytype As String = String.Empty
    Protected _targetcompany As String = String.Empty
    Protected _targetcompanytype As String = String.Empty
    Protected _status As String = String.Empty
    Protected _statusdate As DateTime
    Protected _notes As String = String.Empty
    Protected _pod As String = String.Empty
    Protected _createdate As DateTime
    Protected _sheduledarrivaldate As DateTime
    Protected _requestedarrivaldate As DateTime
    Protected _scheduleddeliverydate As DateTime
    Protected _shippeddate As DateTime
    Protected _staginglane As String = String.Empty
    Protected _stagingwarehousearea As String = String.Empty
    Protected _shipment As String = String.Empty
    Protected _stopnumber As String = String.Empty
    Protected _loadingseq As String = String.Empty
    Protected _routingset As String = String.Empty
    Protected _route As String = String.Empty
    Protected _deliverystatus As String = String.Empty
    Protected _saw As String = String.Empty
    Protected _orderpriority As String = String.Empty
    Protected _expectedqty As String = String.Empty
    Protected _receivedqty As String = String.Empty
    Protected _expectedweight As String = String.Empty
    Protected _receiveweight As String = String.Empty
    Protected _shipto As String = String.Empty
    Protected _receivedfrom As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty


#End Region


#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And TRANSSHIPMENT = '" & _transshipment & "'"
        End Get
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property TRANSSHIPMENT() As String
        Get
            Return _transshipment
        End Get
        Set(ByVal Value As String)
            _transshipment = Value
        End Set
    End Property

    Public Property ORDERTYPE() As String
        Get
            Return _ordertype
        End Get
        Set(ByVal Value As String)
            _ordertype = Value
        End Set
    End Property

    Public Property REFERENCEORD() As String
        Get
            Return _referenceord
        End Get
        Set(ByVal Value As String)
            _referenceord = Value
        End Set
    End Property

    Public Property SOURCECOMPANY() As String
        Get
            Return _sourcecompany
        End Get
        Set(ByVal Value As String)
            _sourcecompany = Value
        End Set
    End Property

    Public Property SOURCECOMPANYTYPE() As String
        Get
            Return _sourcecompanytype
        End Get
        Set(ByVal Value As String)
            _sourcecompanytype = Value
        End Set
    End Property

    Public Property TARGETCOMPANY() As String
        Get
            Return _targetcompany
        End Get
        Set(ByVal Value As String)
            _targetcompany = Value
        End Set
    End Property

    Public Property TARGETCOMPANYTYPE() As String
        Get
            Return _targetcompanytype
        End Get
        Set(ByVal Value As String)
            _targetcompanytype = Value
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

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal Value As String)
            _notes = Value
        End Set
    End Property

    Public Property CREATEDATE() As DateTime
        Get
            Return _createdate
        End Get
        Set(ByVal Value As DateTime)
            _createdate = Value
        End Set
    End Property

    Public Property SCHEDULEDARRIVALDATE() As DateTime
        Get
            Return _sheduledarrivaldate
        End Get
        Set(ByVal Value As DateTime)
            _sheduledarrivaldate = Value
        End Set
    End Property

    Public Property REQUESTEDDELIVERYDATE() As DateTime
        Get
            Return _requestedarrivaldate
        End Get
        Set(ByVal Value As DateTime)
            _requestedarrivaldate = Value
        End Set
    End Property

    Public Property SCHEDULEDDELIVERYDATE() As DateTime
        Get
            Return _scheduleddeliverydate
        End Get
        Set(ByVal Value As DateTime)
            _scheduleddeliverydate = Value
        End Set
    End Property

    Public Property SHIPPEDDATE() As DateTime
        Get
            Return _shippeddate
        End Get
        Set(ByVal Value As DateTime)
            _shippeddate = Value
        End Set
    End Property

    Public Property STAGINGLANE() As String
        Get
            Return _staginglane
        End Get
        Set(ByVal Value As String)
            _staginglane = Value
        End Set
    End Property

    Public Property STAGINGWAREHOUSEAREA() As String
        Get
            Return _stagingwarehousearea
        End Get
        Set(ByVal Value As String)
            _stagingwarehousearea = Value
        End Set
    End Property

    Public Property SHIPMENT() As String
        Get
            Return _shipment
        End Get
        Set(ByVal Value As String)
            _shipment = Value
        End Set
    End Property

    Public Property STOPNUMBER() As String
        Get
            Return _stopnumber
        End Get
        Set(ByVal Value As String)
            _stopnumber = Value
        End Set
    End Property

    Public Property LOADINGSEQ() As String
        Get
            Return _loadingseq
        End Get
        Set(ByVal Value As String)
            _loadingseq = Value
        End Set
    End Property

    Public Property ROUTINGSET() As String
        Get
            Return _routingset
        End Get
        Set(ByVal Value As String)
            _routingset = Value
        End Set
    End Property

    Public Property ROUTE() As String
        Get
            Return _route
        End Get
        Set(ByVal Value As String)
            _route = Value
        End Set
    End Property

    Public Property DELIVERYSTATUS() As String
        Get
            Return _deliverystatus
        End Get
        Set(ByVal Value As String)
            _deliverystatus = Value
        End Set
    End Property

    Public Property POD() As String
        Get
            Return _pod
        End Get
        Set(ByVal Value As String)
            _pod = Value
        End Set
    End Property

    Public Property ORDERPRIORITY() As String
        Get
            Return _orderpriority
        End Get
        Set(ByVal Value As String)
            _orderpriority = Value
        End Set
    End Property

    Public Property EXPECTEDQTY() As String
        Get
            Return _expectedqty
        End Get
        Set(ByVal Value As String)
            _expectedqty = Value
        End Set
    End Property

    Public Property RECEIVEDQTY() As String
        Get
            Return _receivedqty
        End Get
        Set(ByVal Value As String)
            _receivedqty = Value
        End Set
    End Property

    Public Property EXPECTEDWEIGHT() As String
        Get
            Return _expectedweight
        End Get
        Set(ByVal Value As String)
            _expectedweight = Value
        End Set
    End Property

    Public Property RECEIVEWEIGHT() As String
        Get
            Return _receiveweight
        End Get
        Set(ByVal Value As String)
            _receiveweight = Value
        End Set
    End Property

    Public Property ShipTo() As String
        Get
            Return _shipto
        End Get
        Set(ByVal Value As String)
            _shipto = Value
        End Set
    End Property

    Public Property ReceivedFrom() As String
        Get
            Return _receivedfrom
        End Get
        Set(ByVal Value As String)
            _receivedfrom = Value
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

    Public ReadOnly Property CANSHIP() As Boolean
        Get
            If _status = WMS.Lib.Statuses.TransShipment.RECEIVED Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CANCANCEL() As Boolean
        Get
            If _status = WMS.Lib.Statuses.TransShipment.STATUSNEW Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CANRECEIVE() As Boolean
        Get
            If _status = WMS.Lib.Statuses.TransShipment.STATUSNEW Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pTRANSSHIPMENT As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _transshipment = pTRANSSHIPMENT
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim outorheader As OutboundOrderHeader
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)

        Select Case CommandName.ToUpper
            Case "CREATE"
                CreateNew(dr("CONSIGNEE"), dr("TRANSSHIPMENT"), Convert.ReplaceDBNull(dr("ORDERTYPE")), Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                          dr("SOURCECOMPANY"), Convert.ReplaceDBNull(dr("SOURCECOMPANYTYPE")), Convert.ReplaceDBNull(dr("TARGETCOMPANY")), Convert.ReplaceDBNull(dr("TARGETCOMPANYTYPE")), Nothing, _
                          Convert.ReplaceDBNull(dr("NOTES")), DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                          Convert.ReplaceDBNull(dr("REQUESTEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), Nothing, _
                          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("SHIPMENT")), Nothing, Convert.ReplaceDBNull(dr("LOADINGSEQ")), Nothing, _
                          Nothing, Nothing, Nothing, Nothing, _
                          Convert.ReplaceDBNull(dr("EXPECTEDQTY")), Nothing, Convert.ReplaceDBNull(dr("EXPECTEDWEIGHT")), Nothing, Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                          DateTime.Now, Common.GetCurrentUser, DateTime.Now, Common.GetCurrentUser)
            Case "UPDATE"
                _consignee = dr("CONSIGNEE")
                _transshipment = dr("TRANSSHIPMENT")
                Edit(dr("CONSIGNEE"), dr("TRANSSHIPMENT"), Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                          Convert.ReplaceDBNull(dr("SOURCECOMPANY")), Convert.ReplaceDBNull(dr("SOURCECOMPANYTYPE")), Convert.ReplaceDBNull(dr("TARGETCOMPANY")), Convert.ReplaceDBNull(dr("TARGETCOMPANYTYPE")), _
                          Convert.ReplaceDBNull(dr("NOTES")), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                          Convert.ReplaceDBNull(dr("REQUESTEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), _
                          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("SHIPMENT")), Convert.ReplaceDBNull(dr("STOPNUMBER")), Convert.ReplaceDBNull(dr("LOADINGSEQ")), Convert.ReplaceDBNull(dr("ROUTINGSET")), _
                          Convert.ReplaceDBNull(dr("ROUTE")), Convert.ReplaceDBNull(dr("ORDERPRIORITY")), _
                          Convert.ReplaceDBNull(dr("EXPECTEDQTY")), Convert.ReplaceDBNull(dr("EXPECTEDWEIGHT")), Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                          DateTime.Now, Common.GetCurrentUser)
            Case "CANCEL"
                For Each dr In ds.Tables(0).Rows
                    _consignee = dr("CONSIGNEE")
                    _transshipment = dr("TRANSSHIPMENT")
                    Load()
                    If CANCANCEL Then
                        _status = WMS.Lib.Statuses.TransShipment.CANCELED
                        Save(Common.GetCurrentUser)
                    Else
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot cancel Transshipment. Transshipment status incorrect.", "Cannot cancel Transshipment. Transshipment status incorrect.")
                        Throw m4nEx
                    End If
                Next

            Case "SHIP"
                For Each dr In ds.Tables(0).Rows
                    _consignee = dr("CONSIGNEE")
                    _transshipment = dr("TRANSSHIPMENT")
                    Load()
                    Ship(Common.GetCurrentUser)
                Next
            Case "RECEIVE"
                For Each dr In ds.Tables(0).Rows
                    _consignee = dr("CONSIGNEE")
                    _transshipment = dr("TRANSSHIPMENT")
                    Load()
                    Receive(Common.GetCurrentUser)
                    'Edit(dr("CONSIGNEE"), dr("TRANSSHIPMENT"), Convert.ReplaceDBNull(dr("ORDERTYPE")), Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                    '          Convert.ReplaceDBNull(dr("SOURCECOMPANY")), Convert.ReplaceDBNull(dr("SOURCECOMPANYTYPE")), Convert.ReplaceDBNull(dr("TARGETCOMPANY")), Convert.ReplaceDBNull(dr("TARGETCOMPANYTYPE")), _status, DateTime.Now, _
                    '          Convert.ReplaceDBNull(dr("NOTES")), DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                    '          Convert.ReplaceDBNull(dr("requesteddeliverydate")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SHIPPEDDATE")), _
                    '          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("SHIPMENT")), Convert.ReplaceDBNull(dr("STOPNUMBER")), Convert.ReplaceDBNull(dr("LOADINGSEQ")), Convert.ReplaceDBNull(dr("ROUTINGSET")), _
                    '          Convert.ReplaceDBNull(dr("ROUTE")), Convert.ReplaceDBNull(dr("DELIVERYSTATUS")), Convert.ReplaceDBNull(dr("POD")), Convert.ReplaceDBNull(dr("ORDERPRIORITY")), _
                    '          Convert.ReplaceDBNull(dr("EXPECTEDQTY")), Convert.ReplaceDBNull(dr("RECEIVEDQTY")), Convert.ReplaceDBNull(dr("EXPECTEDWEIGHT")), Convert.ReplaceDBNull(dr("RECEIVEWEIGHT")), _
                    '          DateTime.Now, Common.GetCurrentUser)
                Next

            Case "PRINTSH"
                For Each dr In ds.Tables(0).Rows
                    Dim trn As New TRANSSHIPMENT(dr("CONSIGNEE"), dr("TRANSSHIPMENT"))
                    trn.PrintShippingManifest(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next
            Case "PRINTLABELS"
                If ds.Tables(0).Rows.Count > 0 Then
                    _consignee = dr("CONSIGNEE")
                    _transshipment = ds.Tables(0).Rows(0)("TRANSSHIPMENT")
                    Load()
                    PrintTransshipmentLabels("")
                End If


        End Select

    End Sub

#End Region

#Region "Methods"

#Region "General"

    Public Shared Function Exists(ByVal pCONSIGNEE As String, ByVal pTRANSSHIPMENT As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM TRANSSHIPMENT WHERE CONSIGNEE = '{0}' AND TRANSSHIPMENT = '{1}'", pCONSIGNEE, pTRANSSHIPMENT)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM TRANSSHIPMENT WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            Load(dr)
        End If
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("TRANSSHIPMENT") Then _transshipment = dr.Item("TRANSSHIPMENT")
        If Not dr.IsNull("ORDERTYPE") Then _ordertype = dr.Item("ORDERTYPE")
        If Not dr.IsNull("REFERENCEORD") Then _referenceord = dr.Item("REFERENCEORD")
        If Not dr.IsNull("SOURCECOMPANY") Then _sourcecompany = dr.Item("SOURCECOMPANY")
        If Not dr.IsNull("SOURCECOMPANYTYPE") Then _sourcecompanytype = dr.Item("SOURCECOMPANYTYPE")
        If Not dr.IsNull("TARGETCOMPANY") Then _targetcompany = dr.Item("TARGETCOMPANY")
        If Not dr.IsNull("TARGETCOMPANYTYPE") Then _targetcompanytype = dr.Item("TARGETCOMPANYTYPE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("STATUSDATE") Then _statusdate = dr.Item("STATUSDATE")
        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("SCHEDULEDARRIVALDATE") Then _sheduledarrivaldate = dr.Item("SCHEDULEDARRIVALDATE")
        If Not dr.IsNull("REQUESTEDDELIVERYDATE") Then _requestedarrivaldate = dr.Item("REQUESTEDDELIVERYDATE")
        If Not dr.IsNull("SCHEDULEDDELIVERYDATE") Then _scheduleddeliverydate = dr.Item("SCHEDULEDDELIVERYDATE")
        If Not dr.IsNull("SHIPPEDDATE") Then _shippeddate = dr.Item("SHIPPEDDATE")
        If Not dr.IsNull("STAGINGLANE") Then _staginglane = dr.Item("STAGINGLANE")
        If Not dr.IsNull("STAGINGWAREHOUSEAREA") Then _stagingwarehousearea = dr.Item("STAGINGWAREHOUSEAREA")
        If Not dr.IsNull("SHIPMENT") Then _shipment = dr.Item("SHIPMENT")
        If Not dr.IsNull("STOPNUMBER") Then _stopnumber = dr.Item("STOPNUMBER")
        If Not dr.IsNull("LOADINGSEQ") Then _loadingseq = dr.Item("LOADINGSEQ")
        If Not dr.IsNull("ROUTINGSET") Then _routingset = dr.Item("ROUTINGSET")
        If Not dr.IsNull("ROUTE") Then _route = dr.Item("ROUTE")
        If Not dr.IsNull("DELIVERYSTATUS") Then _deliverystatus = dr.Item("DELIVERYSTATUS")
        If Not dr.IsNull("POD") Then _pod = dr.Item("POD")
        If Not dr.IsNull("ORDERPRIORITY") Then _orderpriority = dr.Item("ORDERPRIORITY")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("EXPECTEDQTY") Then _expectedqty = dr.Item("EXPECTEDQTY")
        If Not dr.IsNull("RECEIVEDQTY") Then _receivedqty = dr.Item("RECEIVEDQTY")
        If Not dr.IsNull("EXPECTEDWEIGHT") Then _expectedweight = dr.Item("EXPECTEDWEIGHT")
        If Not dr.IsNull("RECEIVEWEIGHT") Then _receiveweight = dr.Item("RECEIVEWEIGHT")
        If Not dr.IsNull("SHIPTO") Then _shipto = dr.Item("SHIPTO")
        If Not dr.IsNull("RECEIVEDFROM") Then _receivedfrom = dr.Item("RECEIVEDFROM")
    End Sub

    Public Shared Function getTranshipmentIDbyRefOrd(ByVal pCONSIGNEE As String, ByVal pREFERENCEORD As String) As String
        Dim sql As String = String.Format("SELECT TRANSSHIPMENT FROM TRANSSHIPMENT WHERE CONSIGNEE = '{0}' AND REFERENCEORD = '{1}'", pCONSIGNEE, pREFERENCEORD)
        Return System.Convert.ToString(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "DB Functions"

    Public Sub CreateNew(ByVal pConsignee As String, ByVal pTransshipment As String, ByVal pOrdertype As String, ByVal pReferenceord As String, _
                         ByVal pSourcecompany As String, ByVal pSourcecompanytype As String, ByVal pTargetcompany As String, ByVal pTargetcompanytype As String, _
                         ByVal pStatusdate As DateTime, ByVal pNotes As String, _
                         ByVal pCreatedate As DateTime, ByVal pScheduledarrivaldate As DateTime, ByVal pRequesteddeliverydate As DateTime, _
                         ByVal pScheduleddeliverydate As DateTime, ByVal pShippeddate As DateTime, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, _
                         ByVal pShipment As String, ByVal pStopnumber As String, ByVal pLoadingseq As String, ByVal pRoutingset As String, _
                         ByVal pRoute As String, ByVal pDeliverystatus As String, ByVal pPod As String, ByVal pOrderpriority As String, _
                         ByVal pExpectedqty As Double, ByVal pReceivedqty As Double, ByVal pExpectedweight As Double, ByVal pReceiveweight As Double, _
                         ByVal pShipTo As String, ByVal pReceivedFrom As String, _
                         ByVal pAdddate As DateTime, ByVal pAdduser As String, ByVal pEditdate As DateTime, ByVal pEdituser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        exist = WMS.Logic.Consignee.Exists(pConsignee)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        Else
            _consignee = pConsignee
        End If

        exist = WMS.Logic.TransShipment.Exists(pConsignee, pTransshipment)
        If exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.OrderId already Exist", "Can't create order.OrderId already Exist")
            Throw m4nEx
        Else
            _transshipment = pTransshipment
        End If

        exist = WMS.Logic.Company.Exists(_consignee, pSourcecompany, pSourcecompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
            Throw m4nEx
        Else
            If pReceivedFrom Is Nothing Then pReceivedFrom = ""
            If pReceivedFrom.Length = 0 Then
                _sourcecompanytype = pSourcecompanytype
                _sourcecompany = pSourcecompany
                Dim oComp As New Company(pConsignee, pSourcecompany, pSourcecompanytype)
                _receivedfrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pReceivedFrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _receivedfrom = pReceivedFrom
                _sourcecompanytype = pSourcecompanytype
                _sourcecompany = pSourcecompany
            End If
        End If

        exist = WMS.Logic.Company.Exists(_consignee, pTargetcompany, pTargetcompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If pShipTo Is Nothing Then pShipTo = ""
            If pShipTo.Length = 0 Then
                _targetcompanytype = pTargetcompanytype
                _targetcompany = pTargetcompany
                Dim oComp As New Company(pConsignee, pTargetcompany, pTargetcompanytype)
                _shipto = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pShipTo) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _shipto = pShipTo
                _targetcompanytype = pTargetcompanytype
                _targetcompany = pTargetcompany
            End If
        End If

        If pStaginglane = "" And pStagingwarehousearea = "" Then
            _staginglane = pStaginglane
            _stagingwarehousearea = pStagingwarehousearea
        Else
            exist = WMS.Logic.Location.Exists(pStaginglane, pStagingwarehousearea)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            Else
                _staginglane = pStaginglane
                _stagingwarehousearea = pStagingwarehousearea
            End If
        End If

        _referenceord = pReferenceord
        _status = WMS.Lib.Statuses.TransShipment.STATUSNEW
        _statusdate = pStatusdate
        _notes = pNotes
        _createdate = DateTime.Now()
        _sheduledarrivaldate = pScheduledarrivaldate
        _requestedarrivaldate = pRequesteddeliverydate
        _scheduleddeliverydate = pScheduleddeliverydate
        _shippeddate = pShippeddate
        _shipment = pShipment
        _stopnumber = pStopnumber
        _loadingseq = pLoadingseq
        _routingset = pRoutingset
        _route = pRoute
        _deliverystatus = pDeliverystatus
        _pod = pPod
        _orderpriority = pOrderpriority
        _expectedqty = pExpectedqty
        _expectedweight = pExpectedweight
        _receivedqty = pReceivedqty
        _receiveweight = pReceiveweight
        _adddate = DateTime.Now
        _adduser = pAdduser
        _editdate = DateTime.Now
        _edituser = pEdituser

        SQL = "INSERT INTO TRANSSHIPMENT(CONSIGNEE, TRANSSHIPMENT, ORDERTYPE, REFERENCEORD, SOURCECOMPANY, SOURCECOMPANYTYPE, TARGETCOMPANY, " & _
                                            "TARGETCOMPANYTYPE, STATUS, STATUSDATE, NOTES, CREATEDATE, SCHEDULEDARRIVALDATE, REQUESTEDDELIVERYDATE, " & _
                                            "SCHEDULEDDELIVERYDATE, SHIPPEDDATE, STAGINGLANE,STAGINGWAREHOUSEAREA, SHIPMENT, STOPNUMBER, LOADINGSEQ, ROUTINGSET, ROUTE, DELIVERYSTATUS, " & _
                                            "EXPECTEDQTY, EXPECTEDWEIGHT, RECEIVEDQTY, RECEIVEWEIGHT, " & _
                                            "POD, ORDERPRIORITY, RECEIVEDFROM, SHIPTO, ADDDATE, ADDUSER, EDITDATE, EDITUSER) "
        SQL += "VALUES(" & Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_transshipment) & "," & Made4Net.Shared.Util.FormatField(_ordertype) & "," & Made4Net.Shared.Util.FormatField(_referenceord) & "," & Made4Net.Shared.Util.FormatField(_sourcecompany) & "," & Made4Net.Shared.Util.FormatField(_sourcecompanytype) & "," & Made4Net.Shared.Util.FormatField(_targetcompany) & "," & _
                           Made4Net.Shared.Util.FormatField(_targetcompanytype) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_statusdate) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & Made4Net.Shared.Util.FormatField(_createdate) & "," & Made4Net.Shared.Util.FormatField(_sheduledarrivaldate) & "," & Made4Net.Shared.Util.FormatField(_requestedarrivaldate) & "," & _
                           Made4Net.Shared.Util.FormatField(_scheduleddeliverydate) & "," & Made4Net.Shared.Util.FormatField(_shippeddate) & "," & Made4Net.Shared.Util.FormatField(_staginglane) & "," & Made4Net.Shared.Util.FormatField(_stagingwarehousearea) & "," & Made4Net.Shared.Util.FormatField(_shipment) & "," & Made4Net.Shared.Util.FormatField(_stopnumber) & "," & Made4Net.Shared.Util.FormatField(_loadingseq) & "," & Made4Net.Shared.Util.FormatField(_routingset) & "," & Made4Net.Shared.Util.FormatField(_route) & "," & Made4Net.Shared.Util.FormatField(_deliverystatus) & "," & _
                           Made4Net.Shared.Util.FormatField(_expectedqty) & "," & Made4Net.Shared.Util.FormatField(_expectedweight) & "," & Made4Net.Shared.Util.FormatField(_receivedqty) & "," & Made4Net.Shared.Util.FormatField(_receiveweight) & "," & _
                           Made4Net.Shared.Util.FormatField(_pod) & "," & Made4Net.Shared.Util.FormatField(_orderpriority) & "," & Made4Net.Shared.Util.FormatField(_receivedfrom) & "," & Made4Net.Shared.Util.FormatField(_shipto) & "," & Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Save(ByVal pUser As String)

        If Not WMS.Logic.Consignee.Exists(_consignee) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        End If

        'If Not WMS.Logic.Company.Exists(_consignee, _sourcecompany, _sourcecompanytype) Then
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
        '    Throw m4nEx
        'End If


        'If Not WMS.Logic.Company.Exists(_consignee, _targetcompany, _targetcompanytype) Then
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
        '    Throw m4nEx
        'End If

        If _staginglane <> "" And _stagingwarehousearea <> "" Then
            If Not WMS.Logic.Location.Exists(_staginglane, _stagingwarehousearea) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            End If
        End If

        Dim SQL As String
        If Not Exists(_consignee, _transshipment) Then
            _status = WMS.Lib.Statuses.TransShipment.STATUSNEW
            _createdate = DateTime.Now
            _editdate = DateTime.Now
            _edituser = pUser
            _adduser = pUser
            _adddate = DateTime.Now
            _statusdate = DateTime.Now

            SQL = "INSERT INTO TRANSSHIPMENT(CONSIGNEE, TRANSSHIPMENT, ORDERTYPE, REFERENCEORD, SOURCECOMPANY, SOURCECOMPANYTYPE, TARGETCOMPANY, " & _
                                                "TARGETCOMPANYTYPE, STATUS, STATUSDATE, NOTES, CREATEDATE, SCHEDULEDARRIVALDATE, REQUESTEDDELIVERYDATE, " & _
                                                "SCHEDULEDDELIVERYDATE, SHIPPEDDATE, STAGINGLANE, STAGINGWAREHOUSEAREA, SHIPMENT, STOPNUMBER, LOADINGSEQ, ROUTINGSET, ROUTE, DELIVERYSTATUS, " & _
                                                "EXPECTEDQTY, EXPECTEDWEIGHT, RECEIVEDQTY, RECEIVEWEIGHT, " & _
                                                "POD, ORDERPRIORITY, RECEIVEDFROM, SHIPTO, ADDDATE, ADDUSER, EDITDATE, EDITUSER) "
            SQL += "VALUES(" & Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_transshipment) & "," & Made4Net.Shared.Util.FormatField(_ordertype) & "," & Made4Net.Shared.Util.FormatField(_referenceord) & "," & Made4Net.Shared.Util.FormatField(_sourcecompany) & "," & Made4Net.Shared.Util.FormatField(_sourcecompanytype) & "," & Made4Net.Shared.Util.FormatField(_targetcompany) & "," & _
                               Made4Net.Shared.Util.FormatField(_targetcompanytype) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_statusdate) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & Made4Net.Shared.Util.FormatField(_createdate) & "," & Made4Net.Shared.Util.FormatField(_sheduledarrivaldate) & "," & Made4Net.Shared.Util.FormatField(_requestedarrivaldate) & "," & _
                               Made4Net.Shared.Util.FormatField(_scheduleddeliverydate) & "," & Made4Net.Shared.Util.FormatField(_shippeddate) & "," & Made4Net.Shared.Util.FormatField(_staginglane) & "," & Made4Net.Shared.Util.FormatField(_stagingwarehousearea) & "," & Made4Net.Shared.Util.FormatField(_shipment) & "," & Made4Net.Shared.Util.FormatField(_stopnumber) & "," & Made4Net.Shared.Util.FormatField(_loadingseq) & "," & Made4Net.Shared.Util.FormatField(_routingset) & "," & Made4Net.Shared.Util.FormatField(_route) & "," & Made4Net.Shared.Util.FormatField(_deliverystatus) & "," & _
                               Made4Net.Shared.Util.FormatField(_expectedqty) & "," & Made4Net.Shared.Util.FormatField(_expectedweight) & "," & Made4Net.Shared.Util.FormatField(_receivedqty) & "," & Made4Net.Shared.Util.FormatField(_receiveweight) & "," & _
                               Made4Net.Shared.Util.FormatField(_pod) & "," & Made4Net.Shared.Util.FormatField(_orderpriority) & "," & Made4Net.Shared.Util.FormatField(_receivedfrom) & "," & Made4Net.Shared.Util.FormatField(_shipto) & "," & Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"


        Else
            _editdate = DateTime.Now
            _edituser = pUser
            SQL = String.Format("UPDATE TRANSSHIPMENT " & _
                                "SET ORDERTYPE ={0}, REFERENCEORD ={1}, SOURCECOMPANY ={2}, SOURCECOMPANYTYPE ={3}, TARGETCOMPANY ={4}, " & _
                                    " TARGETCOMPANYTYPE ={5}, STATUS ={6}, STATUSDATE ={7}, NOTES ={8}, CREATEDATE ={9}, SCHEDULEDARRIVALDATE ={10}, REQUESTEDDELIVERYDATE ={11}, " & _
                                    " SCHEDULEDDELIVERYDATE ={12}, SHIPPEDDATE ={13}, STAGINGLANE ={14}, STAGINGWAREHOUSEAREA ={33}, SHIPMENT ={15}, STOPNUMBER ={16}, LOADINGSEQ ={17}, ROUTINGSET ={18}, ROUTE ={19}, " & _
                                    " DELIVERYSTATUS ={20}, POD ={21}, ORDERPRIORITY ={22}, EDITDATE ={23}, EDITUSER = {24}," & _
                                    " EXPECTEDQTY ={25}, EXPECTEDWEIGHT ={26}, RECEIVEDQTY ={27}, RECEIVEWEIGHT ={28}, RECEIVEDFROM={31}, SHIPTO={32} " & _
                                " WHERE CONSIGNEE ={29} AND  TRANSSHIPMENT ={30}", _
                                Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_sourcecompany), Made4Net.Shared.Util.FormatField(_sourcecompanytype), Made4Net.Shared.Util.FormatField(_targetcompany), Made4Net.Shared.Util.FormatField(_targetcompanytype), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), _
                                Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_sheduledarrivaldate), Made4Net.Shared.Util.FormatField(_requestedarrivaldate), Made4Net.Shared.Util.FormatField(_scheduleddeliverydate), Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_shipment), _
                                Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_deliverystatus), Made4Net.Shared.Util.FormatField(_pod), Made4Net.Shared.Util.FormatField(_orderpriority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                                Made4Net.Shared.Util.FormatField(_expectedqty), Made4Net.Shared.Util.FormatField(_expectedweight), Made4Net.Shared.Util.FormatField(_receivedqty), Made4Net.Shared.Util.FormatField(_receiveweight), _
                                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_transshipment), Made4Net.Shared.Util.FormatField(_receivedfrom), Made4Net.Shared.Util.FormatField(_shipto), Made4Net.Shared.Util.FormatField(_stagingwarehousearea))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Edit(ByVal pConsignee As String, ByVal pTransshipment As String, ByVal pReferenceord As String, _
                         ByVal pSourcecompany As String, ByVal pSourcecompanytype As String, ByVal pTargetcompany As String, ByVal pTargetcompanytype As String, _
                         ByVal pNotes As String, _
                         ByVal pScheduledarrivaldate As DateTime, ByVal pRequesteddeliverydate As DateTime, _
                         ByVal pScheduleddeliverydate As DateTime, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, _
                         ByVal pShipment As String, ByVal pStopnumber As String, ByVal pLoadingseq As String, ByVal pRoutingset As String, _
                         ByVal pRoute As String, ByVal pOrderpriority As String, _
                         ByVal pExpectedqty As String, ByVal pExpectedweight As String, _
                         ByVal pShipTo As String, ByVal pReceivedFrom As String, _
                         ByVal pEditdate As DateTime, ByVal pEdituser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        _consignee = pConsignee
        _transshipment = pTransshipment

        exist = WMS.Logic.Company.Exists(_consignee, pSourcecompany, pSourcecompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
            Throw m4nEx
        Else
            If pReceivedFrom.Length = 0 Then
                _sourcecompanytype = pSourcecompanytype
                _sourcecompany = pSourcecompany
                Dim oComp As New Company(pConsignee, pSourcecompany, pSourcecompanytype)
                _receivedfrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pReceivedFrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _receivedfrom = pReceivedFrom
                _sourcecompanytype = pSourcecompanytype
                _sourcecompany = pSourcecompany
            End If
        End If

        exist = WMS.Logic.Company.Exists(_consignee, pTargetcompany, pTargetcompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If pShipTo Is Nothing Then pShipTo = ""
            If pShipTo.Length = 0 Then
                _targetcompanytype = pTargetcompanytype
                _targetcompany = pTargetcompany
                Dim oComp As New Company(pConsignee, pTargetcompany, pTargetcompanytype)
                _shipto = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pShipTo) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _shipto = pShipTo
                _targetcompanytype = pTargetcompanytype
                _targetcompany = pTargetcompany
            End If
        End If

        exist = WMS.Logic.Location.Exists(pStaginglane, pStagingwarehousearea)
        If pStaginglane = "" And pStagingwarehousearea = "" Then
            _staginglane = pStaginglane
            _stagingwarehousearea = pStagingwarehousearea
        Else
            exist = WMS.Logic.Location.Exists(pStaginglane, pStagingwarehousearea)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            Else
                _staginglane = pStaginglane
                _stagingwarehousearea = pStagingwarehousearea
            End If
        End If

        _referenceord = pReferenceord
        _notes = pNotes
        _sheduledarrivaldate = pScheduledarrivaldate
        _requestedarrivaldate = pRequesteddeliverydate
        _scheduleddeliverydate = pScheduleddeliverydate
        _shipment = pShipment
        _stopnumber = pStopnumber
        _loadingseq = pLoadingseq
        _routingset = pRoutingset
        _route = pRoute
        _orderpriority = pOrderpriority
        _expectedqty = pExpectedqty
        _expectedweight = pExpectedweight
        _editdate = DateTime.Now
        _edituser = Common.GetCurrentUser

        SQL = String.Format("UPDATE TRANSSHIPMENT " & _
                            "SET  REFERENCEORD ={0}, SOURCECOMPANY ={1}, SOURCECOMPANYTYPE ={2}, TARGETCOMPANY ={3}, " & _
                                " TARGETCOMPANYTYPE ={4}, NOTES ={5}, SCHEDULEDARRIVALDATE ={6}, REQUESTEDDELIVERYDATE ={7}, " & _
                                " SCHEDULEDDELIVERYDATE ={8}, STAGINGLANE ={9}, STAGINGWAREHOUSEAREA={24}, SHIPMENT ={10}, STOPNUMBER ={11}, LOADINGSEQ ={12}, ROUTINGSET ={13}, ROUTE ={14}, " & _
                                " ORDERPRIORITY ={15}, EDITDATE ={16}, EDITUSER = {17}," & _
                                " EXPECTEDQTY ={18}, EXPECTEDWEIGHT ={19} , RECEIVEDFROM={22}, SHIPTO={23} " & _
                            " WHERE CONSIGNEE ={20} AND  TRANSSHIPMENT ={21}", _
                            Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_sourcecompany), Made4Net.Shared.Util.FormatField(_sourcecompanytype), Made4Net.Shared.Util.FormatField(_targetcompany), Made4Net.Shared.Util.FormatField(_targetcompanytype), _
                            Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_sheduledarrivaldate), Made4Net.Shared.Util.FormatField(_requestedarrivaldate), Made4Net.Shared.Util.FormatField(_scheduleddeliverydate), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_shipment), _
                            Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_orderpriority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                            Made4Net.Shared.Util.FormatField(_expectedqty), Made4Net.Shared.Util.FormatField(_expectedweight), _
                            Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_transshipment), Made4Net.Shared.Util.FormatField(_receivedfrom), Made4Net.Shared.Util.FormatField(_shipto), Made4Net.Shared.Util.FormatField(_stagingwarehousearea))
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Print Label"

    Public Sub PrintLabel(ByVal prtrName As String)
        PrintTransshipmentLabels(prtrName)
    End Sub

#End Region

#Region "Ship Transshipment"

    Public Sub Ship(ByVal puser As String)
        If CANSHIP Then
            If _status = WMS.Lib.Statuses.TransShipment.SHIPPED Then
                Return
            Else
                Dim sql As String
                _editdate = DateTime.Now
                _edituser = puser
                _status = WMS.Lib.Statuses.TransShipment.SHIPPED
                _shippeddate = DateTime.Now
                _statusdate = DateTime.Now



                sql = String.Format("UPDATE TRANSSHIPMENT SET SHIPPEDDATE = {0},STATUS = {1},STATUSDATE = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_shippeddate), _
                             Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(sql)
            End If
        Else
            Throw New ApplicationException(String.Format("TransShipment {0} status is incorrect, can't ship Transshipment", _transshipment))
        End If
    End Sub


    Public Sub AssignToShipment(ByVal pShipmentId As String, ByVal pUser As String)
        If (_status <> WMS.Lib.Statuses.TransShipment.CANCELED) Then
            _shipment = pShipmentId
            _editdate = DateTime.Now
            _edituser = pUser
            DataInterface.RunSQL(String.Format("Update TRANSSHIPMENT SET SHIPMENT = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "TransShipment status is incorrect for ship assignment", "TransShipment status is incorrect for canceling")
            Throw m4nEx
        End If
    End Sub

    Public Sub DeAssignFromShipment(ByVal pUser As String)
        If (_status <> WMS.Lib.Statuses.TransShipment.CANCELED) Then
            _shipment = ""
            _editdate = DateTime.Now
            _edituser = pUser
            DataInterface.RunSQL(String.Format("Update TRANSSHIPMENT SET SHIPMENT = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "TransShipment status is incorrect for ship assignment", "TransShipment status is incorrect for canceling")
            Throw m4nEx
        End If
    End Sub

#End Region

#Region "Recieve Transshipment"

    Public Sub SetReceivedQty(ByVal pQty As Integer, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE TRANSSHIPMENT SET RECEIVEDQTY = '{0}',EDITDATE = {1},EDITUSER = {2} Where {3}", pQty, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub SetReceivedWeight(ByVal pWeght As Double, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE TRANSSHIPMENT SET RECEIVEWEIGHT = '{0}',EDITDATE = {1},EDITUSER = {2} Where {3}", pWeght, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Receive(ByVal pUser As String)

        If CANRECEIVE Then
            If _status = WMS.Lib.Statuses.TransShipment.RECEIVED Then
                Throw New ApplicationException(String.Format("TransShipment {0} was already received, can't receive Transshipment", _transshipment))
                Return
            Else
                _editdate = DateTime.Now
                _edituser = pUser


                If _status = WMS.Lib.Statuses.TransShipment.STATUSNEW Then
                    _status = WMS.Lib.Statuses.TransShipment.RECEIVED
                End If

                _receivedqty = _expectedqty
                _receiveweight = _expectedweight
                Dim sql As String = String.Format("UPDATE TRANSSHIPMENT SET STATUS = '{0}', RECEIVEWEIGHT='{1}', RECEIVEDQTY='{2}',EDITDATE = {3},EDITUSER = {4} Where {5}", _status, _receiveweight, _receivedqty, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(sql)
            End If
        Else
            Throw New ApplicationException(String.Format("TransShipment {0} status is incorrect, can't receive Transshipment", _transshipment))
        End If

    End Sub

#End Region

#Region "Print Label"

    Public Sub PrintTransshipmentLabels(ByVal lblPrinter As String)
        Dim lbltype As String
        Try
            lbltype = WMS.Lib.TRANSSHIPMENTLABELTYPE.TRANSSHIPMENT
        Catch ex As Exception

        End Try
        If Not lbltype = WMS.Lib.TRANSSHIPMENTLABELTYPE.NONE Then
            PrintTransshipmentLabels(lbltype, lblPrinter)
        End If
    End Sub

    Public Sub PrintTransshipmentLabels(ByVal LabelType As String, ByVal lblPrinter As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "Transshipmentlabel")
        qSender.Add("LABELTYPE", LabelType)
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("TRANSSHIPMENT", _transshipment)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("CONSIGNEE", _consignee)
        ht.Hash.Add("TRANSSHIPMENT", _transshipment)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Send("Label", "Transshipment Label")
    End Sub

#End Region

#Region "SL Assign"

    Public Sub SetStagingLane(ByVal pStagingLane As String, ByVal pStagingwarehousearea As String, ByVal pUser As String)
        Dim SQL As String = String.Empty
        Dim exist As Boolean = True

        If _status = WMS.Lib.Statuses.TransShipment.STATUSNEW Then
            If pStagingLane = "" And pStagingwarehousearea = "" Then
                _staginglane = pStagingLane
                _stagingwarehousearea = pStagingwarehousearea
            Else
                exist = WMS.Logic.Location.Exists(pStagingLane, pStagingwarehousearea)
                If Not exist Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                    Throw m4nEx
                Else
                    _staginglane = pStagingLane
                    _stagingwarehousearea = pStagingwarehousearea
                End If
            End If

            _editdate = DateTime.Now
            _edituser = pUser

            SQL = String.Format("UPDATE TRANSSHIPMENT SET staginglane = {0},STAGINGWAREHOUSEAREA={4}, editdate = {1},edituser={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_stagingwarehousearea))

            DataInterface.RunSQL(SQL)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot update order.Invalid Status", "Can't update order.Invalid Status")
            Throw m4nEx
        End If
    End Sub

#End Region

#Region "Set Contacts"

    Public Sub SetReceivedFrom(ByVal pContactID As String)
        Dim SQL As String = "UPDATE TRANSSHIPMENT SET RECEIVEDFROM = '" & pContactID & "' WHERE " & WhereClause
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub SetShipTo(ByVal pContactID As String)
        Dim SQL As String = "UPDATE TRANSSHIPMENT SET SHIPTO = '" & pContactID & "' WHERE " & WhereClause
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#End Region

#Region "Reports"

    Public Sub PrintShippingManifest(ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "TranshipmentDelNote"
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "TranshipmentDelNote", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "TranshipmentDelNote")
        oQsender.Add("DATASETID", "repTranshipmentDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONSIGNEE = '{0}' and TRANSSHIPMENT = '{1}'", _consignee, _transshipment))
        oQsender.Send("Report", repType)
    End Sub

#End Region

End Class


<CLSCompliant(False)> Public Class TransShipmentDetail
#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _transshipment As String = String.Empty
    Protected _handlingunitid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _handlingunittype As String = String.Empty
    Protected _weight As Decimal
    Protected _volume As Decimal
    Protected _notes As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty


#End Region

#End Region

#Region "Properties"
    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And TRANSSHIPMENT = '" & _transshipment & "' And HandlingUnitID='" & _handlingunitid & "'"
        End Get
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal value As String)
            _consignee = value
        End Set
    End Property

    Public Property TRANSSHIPMENT() As String
        Get
            Return _transshipment
        End Get
        Set(ByVal value As String)
            _transshipment = value
        End Set
    End Property

    Public Property HANDLINGUNITID() As String
        Get
            Return _handlingunitid
        End Get
        Set(ByVal value As String)
            _handlingunitid = value
        End Set
    End Property

    Public Property HANDLINGUNITTYPE() As String
        Get
            Return _handlingunittype
        End Get
        Set(ByVal value As String)
            _handlingunittype = value
        End Set
    End Property

    Public Property WEIGHT() As Decimal
        Get
            Return _weight
        End Get
        Set(ByVal value As Decimal)
            _weight = value
        End Set
    End Property

    Public Property VOLUME() As Decimal
        Get
            Return _volume
        End Get
        Set(ByVal value As Decimal)
            _volume = value
        End Set
    End Property

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal value As String)
            _notes = value
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

    Public Sub New(ByVal pConsignee As String, ByVal pTransshipment As String, ByVal pHandlingUnitID As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pConsignee
        _transshipment = pTransshipment
        _handlingunitid = pHandlingUnitID
        If LoadObj Then
            Load()
        End If
    End Sub
#End Region

#Region "Methods"

    Public Sub Load()
        Dim SQL As String = "SELECT * FROM TRANSSHIPMENTDetails WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New M4NException(New Exception, "Can not load transshipment Detail. It does not exist.", "Can not load transshipment Detail. It does not exist.")
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("HANDLINGUNITTYPE") Then _handlingunittype = dr.Item("HANDLINGUNITTYPE")
        If Not dr.IsNull("WEIGHT") Then _weight = dr.Item("WEIGHT")
        If Not dr.IsNull("VOLUME") Then _volume = dr.Item("VOLUME")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Public Sub Create(ByVal pConsignee As String, ByVal pTransshipment As String, ByVal pHandlingUnitID As String, _
                      ByVal pHandlingUnitType As String, ByVal pWeight As Decimal, ByVal pVolume As Decimal, ByVal pNotes As String, _
                      ByVal pUser As String)

        _consignee = pConsignee
        _transshipment = pTransshipment
        _handlingunitid = pHandlingUnitID
        _handlingunittype = pHandlingUnitType
        _weight = pWeight
        _volume = pVolume
        _notes = pNotes
        _adduser = pUser
        _edituser = pUser
        validate(False)
        _adddate = DateTime.Now
        _editdate = DateTime.Now
        Save()
    End Sub

    Public Sub Update(ByVal pHandlingUnitType As String, ByVal pWeight As Decimal, ByVal pVolume As Decimal, ByVal pNotes As String, _
    ByVal pUser As String)
        _handlingunittype = pHandlingUnitType
        _weight = pWeight
        _volume = pVolume
        _notes = pNotes
        _adduser = pUser
        _edituser = pUser
        validate(True)
        _adddate = DateTime.Now
        _editdate = DateTime.Now
        Save()
    End Sub

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pTransshipment As String, ByVal pHandlingUnitID As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM TransshipmentDetails WHERE CONSIGNEE = '{0}' AND TRANSSHIPMENT = '{1}' and handlingunitid='{2}'", pConsignee, pTransshipment, pHandlingUnitID)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Save()
        If Exists(_consignee, _transshipment, _handlingunitid) Then
            Dim sql As String = String.Format("Update TransshipmentDetails set HandlingUnitType={0},Weight={1},Volume={2},Notes={3},EditUser={4}," & _
            "EditDate={5} where {6}", FormatField(_handlingunittype), FormatField(_weight), FormatField(_volume), FormatField(_notes), FormatField(_edituser), _
            FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
        Else
            Dim sql As String = String.Format("Insert into transshipmentdetails (Consignee,Transshipment,HandlingUnitID,HandlingUnitType,Weight," & _
            "Volume,Notes,AddUser,AddDate,EditUser,EditDate) Values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{7},{8})", _
            FormatField(_consignee), FormatField(_transshipment), FormatField(_handlingunitid), FormatField(_handlingunittype), FormatField(_weight), _
            FormatField(_volume), FormatField(_notes), FormatField(_adduser), FormatField(_adddate))

            DataInterface.RunSQL(sql)
        End If
    End Sub

    Private Sub validate(ByVal pEdit As Boolean)
        If pEdit Then
            If Not Exists(_consignee, _transshipment, _handlingunitid) Then
                Throw New M4NException(New Exception, "Transshipment detail does not exist.", "Transshipment detail does not exist.")
            End If
        Else
            If Exists(_consignee, _transshipment, _handlingunitid) Then
                Throw New M4NException(New Exception, "Transshipment detail already exists.", "Transshipment detail already exists.")
            End If
        End If

        If Not WMS.Logic.TransShipment.Exists(_consignee, _transshipment) Then
            Throw New M4NException(New Exception, "Transshipment does not exist.", "Transshipment does not exist.")
        End If

        Dim ts As New WMS.Logic.TransShipment(_consignee, _transshipment)
        If ts.STATUS = WMS.Lib.Statuses.TransShipment.SHIPPED Then
            Throw New M4NException(New Exception, "Transshipment was already shipped.", "Transshipment was already shipped.")
        End If
        If ts.STATUS = WMS.Lib.Statuses.TransShipment.CANCELED Then
            Throw New M4NException(New Exception, "Transshipment was cancelled.", "Transshipment was cancelled.")
        End If

        If _weight < 0 Then
            Throw New M4NException(New Exception, "Weight can not be negative.", "Weight can not be negative.")
        End If

        If _volume < 0 Then
            Throw New M4NException(New Exception, "Volume can not be negative.", "Volume can not be negative.")
        End If
    End Sub

#End Region

End Class