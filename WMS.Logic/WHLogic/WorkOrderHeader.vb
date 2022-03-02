Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports System.Xml
Imports Made4Net.Shared

#Region "WorkOrderHeader"

<CLSCompliant(False)> Public Class WorkOrderHeader

#Region "Variables"

    Private _consignee As String
    Private _orderid As String
    Private _ordertype As String
    Private _documenttype As String
    Private _referenceord As String
    Private _referenceordline As Int32
    Private _outboundorder As String
    Private _outboundorderline As Int32
    Private _sku As String
    Private _inventorystatus As String
    Private _originalqty As Decimal
    Private _plannedqty As Decimal
    Private _modifiedqty As Decimal
    Private _completedqty As Decimal
    Private _location As String
    Private _warehousearea As String
    Private _createdate As DateTime
    Private _releasedate As DateTime
    Private _duedate As DateTime
    Private _closedate As DateTime
    Private _notes As String
    Private _status As String
    Private _adddate As DateTime
    Private _adduser As String
    Private _editdate As DateTime
    Private _edituser As String

    Private _attributes As InventoryAttributeBase

    Private _workorderbomcollection As WorkOrderBOMCollection

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "'"
        End Get
    End Property

    Public ReadOnly Property PARTSKUCOLLECTION() As WorkOrderBOMCollection
        Get
            Return _workorderbomcollection
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

    Public Property SKU() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property ORDERID() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
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

    Public Property DOCUMENTTYPE() As WorkOrderDocumentType
        Get
            Return WorkOrderTypeFromString(_documenttype)
        End Get
        Set(ByVal Value As WorkOrderDocumentType)
            _documenttype = WorkOrderTypeToString(Value)
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

    Public Property REFERENCEORDLINE() As Int32
        Get
            Return _referenceordline
        End Get
        Set(ByVal Value As Int32)
            _referenceordline = Value
        End Set
    End Property

    Public Property OUTBOUNDORDER() As String
        Get
            Return _outboundorder
        End Get
        Set(ByVal Value As String)
            _outboundorder = Value
        End Set
    End Property

    Public Property OUTBOUNDORDERLINE() As Int32
        Get
            Return _outboundorderline
        End Get
        Set(ByVal Value As Int32)
            _outboundorderline = Value
        End Set
    End Property

    Public Property INVENTORYSTATUS() As String
        Get
            Return _inventorystatus
        End Get
        Set(ByVal Value As String)
            _inventorystatus = Value
        End Set
    End Property

    Public Property PLANNEDQTY() As Decimal
        Get
            Return _plannedqty
        End Get
        Set(ByVal Value As Decimal)
            _plannedqty = Value
        End Set
    End Property

    Public Property MODIFIEDQTY() As Decimal
        Get
            Return _modifiedqty
        End Get
        Set(ByVal Value As Decimal)
            _modifiedqty = Value
        End Set
    End Property

    Public Property ORIGINALQTY() As Decimal
        Get
            Return _originalqty
        End Get
        Set(ByVal Value As Decimal)
            _originalqty = Value
        End Set
    End Property

    Public Property COMPLETEDQTY() As Decimal
        Get
            Return _completedqty
        End Get
        Set(ByVal Value As Decimal)
            _completedqty = Value
        End Set
    End Property

    Public Property LOCATION() As String
        Get
            Return _location
        End Get
        Set(ByVal Value As String)
            _location = Value
        End Set
    End Property

    Public Property Warehousearea() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
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

    Public Property RELEASEDDATE() As DateTime
        Get
            Return _releasedate
        End Get
        Set(ByVal Value As DateTime)
            _releasedate = Value
        End Set
    End Property

    Public Property DUEDATE() As DateTime
        Get
            Return _duedate
        End Get
        Set(ByVal Value As DateTime)
            _duedate = Value
        End Set
    End Property

    Public Property CLOSEDATE() As DateTime
        Get
            Return _closedate
        End Get
        Set(ByVal Value As DateTime)
            _closedate = Value
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

    Public Property STATUS() As WorkOrderStatus
        Get
            Return WorkOrderStatusFromString(_status)
        End Get
        Set(ByVal Value As WorkOrderStatus)
            _status = WorkOrderStatusToString(Value)
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

    Public Property ATTRIBUTES() As InventoryAttributeBase
        Get
            Return _attributes
        End Get
        Set(ByVal value As InventoryAttributeBase)
            _attributes = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pConsignee As String, ByVal pOrderID As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pConsignee
        _orderid = pOrderID
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM WORKORDERHEADER WHERE" & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "WorkOrder Does not Exists", "WorkOrder Does not Exists")
        Else
            dr = dt.Rows(0)
        End If
        If Not dr.IsNull("ORDERTYPE") Then _ordertype = dr.Item("ORDERTYPE")
        If Not dr.IsNull("DOCUMENTTYPE") Then _documenttype = dr.Item("DOCUMENTTYPE")
        If Not dr.IsNull("REFERENCEORD") Then _referenceord = dr.Item("REFERENCEORD")
        If Not dr.IsNull("REFERENCEORDLINE") Then _referenceordline = dr.Item("REFERENCEORDLINE")
        If Not dr.IsNull("OUTBOUNDORDER") Then _outboundorder = dr.Item("OUTBOUNDORDER")
        If Not dr.IsNull("OUTBOUNDORDERLINE") Then _outboundorderline = dr.Item("OUTBOUNDORDERLINE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("INVENTORYSTATUS") Then _inventorystatus = dr.Item("INVENTORYSTATUS")
        If Not dr.IsNull("ORIGINALQTY") Then _originalqty = dr.Item("ORIGINALQTY")
        If Not dr.IsNull("PLANNEDQTY") Then _plannedqty = dr.Item("PLANNEDQTY")
        If Not dr.IsNull("MODIFIEDQTY") Then _modifiedqty = dr.Item("MODIFIEDQTY")
        If Not dr.IsNull("COMPLETEDQTY") Then _completedqty = dr.Item("COMPLETEDQTY")
        If Not dr.IsNull("LOCATION") Then _location = dr.Item("LOCATION")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("RELEASEDATE") Then _releasedate = dr.Item("RELEASEDATE")
        If Not dr.IsNull("DUEDATE") Then _duedate = dr.Item("DUEDATE")
        If Not dr.IsNull("CLOSEDATE") Then _closedate = dr.Item("CLOSEDATE")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        SQL = "SELECT * FROM attribute where pkeytype = '" & InventoryAttributeBase.AttributeKeyType.WorkOrder & "' and pkey1 = '" & _consignee & "' and pkey2 = '" & _orderid & "'"
        Dim attributeDataTable As New DataTable
        DataInterface.FillDataset(SQL, attributeDataTable)
        If attributeDataTable.Rows.Count = 1 Then
            _attributes = New InventoryAttributeBase(attributeDataTable.Rows(0))
        End If

        'Load PartSku Collection
        _workorderbomcollection = New WorkOrderBOMCollection(_consignee, _orderid)
    End Sub

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pOrderId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from WORKORDERHEADER where consignee = '{0}' and orderid = '{1}'", pConsignee, pOrderId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "Save / Create / Edit / Cancel"

    Public Sub Save(ByVal pUser As String)
        'Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        If WorkOrderHeader.Exists(_consignee, _orderid) Then
            Edit(_ordertype, DOCUMENTTYPE, _referenceord, _referenceordline, _location, _warehousearea, _inventorystatus, _outboundorder, _
                 _outboundorderline, _sku, _modifiedqty, _duedate, _notes, _attributes.Attributes, pUser)
        Else
            _adddate = DateTime.Now
            _adduser = pUser
            Create(_consignee, _orderid, _ordertype, _documenttype, _referenceord, _referenceordline, _outboundorder, _
             _outboundorderline, _sku, _inventorystatus, _originalqty, _duedate, _notes, _location, _warehousearea, _attributes.Attributes, pUser)
        End If
        'DataInterface.RunSQL(sql)
    End Sub

    Public Sub Create(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderType As String, ByVal pDocumentType As WorkOrderDocumentType, ByVal pRefOrder As String, ByVal pRefOrderLine As Int32, _
        ByVal pOutboundOrder As String, ByVal pOutboundOrdLine As Int32, ByVal pSku As String, ByVal pInventoryStatus As String, ByVal pQty As Decimal, ByVal pDueDate As DateTime, ByVal pNotes As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pAttributesCollection As AttributesCollection, ByVal pUser As String)

        If Not WMS.Logic.Consignee.Exists(pConsignee) Then
            Throw New M4NException(New Exception, "Consignee does not exists", "Consignee does not exists")
        End If
        If Exists(pConsignee, pOrderid) Then
            Throw New M4NException(New Exception, "WorkOrder Already Exists", "WorkOrder Already Exists")
        End If
        If Not WMS.Logic.Location.Exists(pLocation, pWarehousearea) Then
            Throw New M4NException(New Exception, "Location does not exists", "Location does not exists")
        End If
        If pOutboundOrder <> "" Then
            If Not WMS.Logic.OutboundOrderHeader.Exists(pConsignee, pOutboundOrder) Then
                Throw New M4NException(New Exception, "Outbound Order does not exists", "Outbound Order does not exists")
            End If
        End If
        _consignee = pConsignee
        _orderid = pOrderid
        If _orderid = "" Then _orderid = Made4Net.Shared.Util.getNextCounter("WORKORDER")
        _ordertype = pOrderType
        _createdate = DateTime.Now
        _duedate = pDueDate
        _modifiedqty = pQty
        _notes = pNotes
        _originalqty = pQty
        _outboundorder = pOutboundOrder
        _outboundorderline = pOutboundOrdLine
        _status = WorkOrderStatusToString(WorkOrderStatus.[New])
        _referenceord = pRefOrder
        _referenceordline = pRefOrderLine
        _location = pLocation
        _warehousearea = pWarehousearea
        _sku = pSku
        _documenttype = WorkOrderTypeToString(pDocumentType)
        _inventorystatus = pInventoryStatus
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String = String.Format("INSERT INTO WORKORDERHEADER(CONSIGNEE, ORDERID, ORDERTYPE, DOCUMENTTYPE, REFERENCEORD, REFERENCEORDLINE, " & _
                        " OUTBOUNDORDER, OUTBOUNDORDERLINE, SKU, INVENTORYSTATUS, ORIGINALQTY, PLANNEDQTY, MODIFIEDQTY, COMPLETEDQTY, LOCATION, CREATEDATE, " & _
                        " RELEASEDATE, DUEDATE, CLOSEDATE, NOTES, STATUS, ADDDATE, ADDUSER, EDITDATE, EDITUSER, WAREHOUSEAREA)" & _
                        " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25})", _
                        Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_referenceordline), Made4Net.Shared.Util.FormatField(_outboundorder), Made4Net.Shared.Util.FormatField(_outboundorderline), _
                        Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_originalqty), Made4Net.Shared.Util.FormatField(_plannedqty), Made4Net.Shared.Util.FormatField(_modifiedqty), Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_releasedate), Made4Net.Shared.Util.FormatField(_duedate), Made4Net.Shared.Util.FormatField(_closedate), _
                        Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_warehousearea))
        DataInterface.RunSQL(SQL)

        If Not pAttributesCollection Is Nothing Then
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrder, _consignee, _orderid)
            _attributes.Add(pAttributesCollection)
            _attributes.Save(pUser)
        End If

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WorkOrderCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WOINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.WOINS)

    End Sub

    Public Sub Edit(ByVal pOrderType As String, ByVal pDocumentType As WorkOrderDocumentType, ByVal pRefOrder As String, ByVal pRefOrderLine As Int32, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pInventoryStatus As String, _
            ByVal pOutboundOrder As String, ByVal pOutboundOrdLine As Int32, ByVal pSku As String, ByVal pQty As Decimal, ByVal pDueDate As DateTime, ByVal pNotes As String, ByVal pAttributesCollection As AttributesCollection, ByVal pUser As String)

        If Not WMS.Logic.Consignee.Exists(_consignee) Then
            Throw New M4NException(New Exception, "Consignee does not exists", "Consignee does not exists")
        End If
        If Not Exists(_consignee, _orderid) Then
            Throw New M4NException(New Exception, "WorkOrder does not Exists", "WorkOrder does not Exists")
        End If
        If pQty < _completedqty Then
            Throw New M4NException(New Exception, "Cannot Adjust Quantity, Units cannot be less than the completed units", "Cannot Adjust Quantity, Units cannot be less than the completed units")
        End If
        If Not WMS.Logic.Location.Exists(pLocation, pWarehousearea) Then
            Throw New M4NException(New Exception, "Location does not exists", "Location does not exists")
        End If
        If pOutboundOrder <> "" Then
            If Not WMS.Logic.OutboundOrderHeader.Exists(_consignee, pOutboundOrder) Then
                Throw New M4NException(New Exception, "Outbound Order does not exists", "Outbound Order does not exists")
            End If
        End If
        _ordertype = pOrderType
        _duedate = pDueDate
        _modifiedqty = pQty
        _notes = pNotes
        _outboundorder = pOutboundOrder
        _outboundorderline = pOutboundOrdLine
        _referenceord = pRefOrder
        _referenceordline = pRefOrderLine
        _sku = pSku
        _documenttype = WorkOrderTypeToString(pDocumentType)
        _location = pLocation
        _warehousearea = pWarehousearea
        _inventorystatus = pInventoryStatus
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String = String.Format("UPDATE WORKORDERHEADER SET ORDERTYPE ={0}, DOCUMENTTYPE ={1}, REFERENCEORD ={2}, REFERENCEORDLINE ={3}, OUTBOUNDORDER ={4}, " & _
                        "OUTBOUNDORDERLINE ={5}, SKU ={6}, INVENTORYSTATUS ={7},ORIGINALQTY={8}, PLANNEDQTY ={9}, MODIFIEDQTY ={10}, COMPLETEDQTY ={11}, LOCATION ={12}, WAREHOUSEAREA ={22}, CREATEDATE ={13}, " & _
                        "RELEASEDATE ={14}, DUEDATE ={15}, CLOSEDATE ={16}, NOTES ={17}, STATUS ={18}, EDITDATE ={19}, EDITUSER = {20} where {21}", _
                        Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_referenceordline), Made4Net.Shared.Util.FormatField(_outboundorder), Made4Net.Shared.Util.FormatField(_outboundorderline), _
                        Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_originalqty), Made4Net.Shared.Util.FormatField(_plannedqty), Made4Net.Shared.Util.FormatField(_modifiedqty), Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_releasedate), Made4Net.Shared.Util.FormatField(_duedate), Made4Net.Shared.Util.FormatField(_closedate), _
                        Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_warehousearea))
        DataInterface.RunSQL(SQL)

        If Not pAttributesCollection Is Nothing Then
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrder, _consignee, _orderid)
            _attributes.Attributes.Clear()
            _attributes.Add(pAttributesCollection)
            _attributes.Save(pUser)
        End If

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WorkOrderUpdated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WOUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.WOUPD)
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        Dim oldStatus As String = _status
        If Me.Status <> WorkOrderStatus.[New] Then
            Throw New M4NException(New Exception, "Work Order Status Incorrect", "Work Order Status Incorrect")
        End If
        If Not WMS.Logic.Consignee.Exists(_consignee) Then
            Throw New M4NException(New Exception, "Consignee does not exists", "Consignee does not exists")
        End If
        If Not Exists(_consignee, _orderid) Then
            Throw New M4NException(New Exception, "WorkOrder does not Exists", "WorkOrder does not Exists")
        End If
        Status = WorkOrderStatus.Cancelled
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String = String.Format("UPDATE WORKORDERHEADER SET STATUS ={0}, EDITDATE ={1}, EDITUSER = {2} where {3}", _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        'Update Bom Collection
        _workorderbomcollection.Cancel(pUser)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WorkOrderCancelled)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WOCNL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTTYPE", _documenttype)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.WOCNL)
    End Sub

#End Region

#Region "Assemble / Disassemble"

    Public Function Assemble(ByVal pQtyToProduce As Decimal, ByVal pLoadsCollection As WMS.Logic.LoadsCollection, ByVal pLoadAttributes As AttributesCollection, ByVal pNewLoadUOM As String, ByVal pNewLoadStatus As String, ByVal pUser As String) As String
        Dim oldStat As String = _status
        If STATUS = WorkOrderStatus.Cancelled Or STATUS = WorkOrderStatus.Complete Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Work Order Status incorrect", "Work Order Status incorrect")
        End If
        If _completedqty + pQtyToProduce > _modifiedqty Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot produce quantity greater than the work order modified quantity", "Cannot produce quantity greater than the work order modified quantity")
        End If
        'Verify input
        VerifyLoadsInput(pQtyToProduce, pLoadsCollection)

        Dim newLoadKit As New Load
        newLoadKit.ProduceForWorkOrder(_consignee, _orderid, _sku, _location, _warehousearea, pQtyToProduce, "", pNewLoadUOM, pNewLoadStatus, pLoadAttributes, pUser)
        'Produce the needed kits
        For Each wobom As WorkOrderBOM In _workorderbomcollection
            For Each ld As Load In pLoadsCollection
                If ld.CONSIGNEE = Me.CONSIGNEE And ld.SKU = wobom.PARTSKU Then
                    ld.ConsumeForWorkOrder(_orderid, wobom.PARTQTY * pQtyToProduce, pUser)
                End If
            Next
        Next
        'Update the completed qty and edituser
        _editdate = DateTime.Now
        _edituser = pUser
        _completedqty = _completedqty + pQtyToProduce
        Dim SQL As String
        If STATUS < WorkOrderStatus.Released And _completedqty < _modifiedqty Then
            STATUS = WorkOrderStatus.Released
            _releasedate = DateTime.Now
            SQL = String.Format("UPDATE WORKORDERHEADER SET COMPLETEDQTY ={0}, EDITDATE ={1}, EDITUSER ={2}, STATUS={3},RELEASEDATE={4} WHERE {5}", _
                       Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_releasedate), WhereClause)
        ElseIf _completedqty = _modifiedqty Then
            STATUS = WorkOrderStatus.Complete
            _closedate = DateTime.Now
            SQL = String.Format("UPDATE WORKORDERHEADER SET COMPLETEDQTY ={0}, EDITDATE ={1}, EDITUSER ={2}, STATUS={3},CLOSEDATE={4} WHERE {5}", _
           Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_closedate), WhereClause)
        Else
            SQL = String.Format("UPDATE WORKORDERHEADER SET COMPLETEDQTY ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", _
                       Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        End If
        DataInterface.RunSQL(SQL)
        'Raise WorkOrderExecuted Event
        RaiseWorkOrderEvent(oldStat, WMSEvents.EventType.WorkOrderExecuted, pUser)
        Return newLoadKit.LOADID
    End Function

    Private Sub VerifyLoadsInput(ByVal pQtyToProduce As Int32, ByVal pLoadsCollection As WMS.Logic.LoadsCollection)
        'Verify first that each sku does not exist in more than one load
        If Not VerifySkuDuplicate(pLoadsCollection) Then
            Throw New M4NException(New Exception, "Sku cannot apear in more than one load", "Sku cannot apear in more than one load")
        End If
        'Verify first that each sku is sufficient for the production proccess
        If Not VerifyAssmblSufficeQty(pQtyToProduce, pLoadsCollection) Then
            Throw New M4NException(New Exception, "Quantity in Loads is less than the required quantity", "Quantity in Loads is less than the required quantity")
        End If
    End Sub

    Private Function VerifySkuDuplicate(ByVal pLoadsCollection As WMS.Logic.LoadsCollection) As Boolean
        Dim tmpHT As New Hashtable
        For Each ld As Load In pLoadsCollection
            If tmpHT.ContainsKey(ld.CONSIGNEE & "@" & ld.SKU) Then
                Throw New M4NException(New Exception, "", "")
            End If
            tmpHT.Add(ld.CONSIGNEE & "@" & ld.SKU, ld)
        Next
        tmpHT.Clear()
        tmpHT = Nothing
        Return True
    End Function

    Private Function VerifyAssmblSufficeQty(ByVal pQtyToProduce As Int32, ByVal pLoadsCollection As WMS.Logic.LoadsCollection) As Boolean
        Dim qtyToFullfill As Decimal
        For Each wobom As WorkOrderBOM In _workorderbomcollection
            qtyToFullfill = pQtyToProduce * wobom.PARTQTY
            For Each ld As Load In pLoadsCollection
                If ld.CONSIGNEE = Me.Consignee And ld.SKU = wobom.PartSku Then
                    If ld.AvailableUnits < qtyToFullfill Then
                        Return False
                    Else
                        qtyToFullfill = qtyToFullfill - ld.AvailableUnits
                    End If
                End If
            Next
            If qtyToFullfill > 0 Then Return False
        Next
        Return True
    End Function

    Public Sub DisAssemble(ByVal pKitLoadId As String, ByVal pQtyToExplode As Decimal, ByVal pBomLoadsDS As DataSet, ByVal pUser As String)
        Dim oldStat As String = _status
        If pQtyToExplode <= 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Quantity must be greater than zero.", "Quantity must be greater than zero.")
        End If
        If STATUS = WorkOrderStatus.Cancelled Or STATUS = WorkOrderStatus.Complete Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Work Order Status incorrect", "Work Order Status incorrect")
        End If
        If _completedqty + pQtyToExplode > _modifiedqty Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot produce quantity greater than the work order modified quantity", "Cannot produce quantity greater than the work order modified quantity")
        End If
        If Not VerifyBomSkuDS(pBomLoadsDS) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Not All BOM SKU found", "Not All BOM SKU found")
        End If
        Dim parentload As Load
        If Not WMS.Logic.Load.Exists(pKitLoadId) Then
            Throw New M4NException(New Exception, "LoadId does not exists", "LoadId does not exists")
        Else
            parentload = New Load(pKitLoadId)
            If parentload.ACTIVITYSTATUS <> WMS.Lib.Statuses.ActivityStatus.NONE OrElse Not String.IsNullOrEmpty(parentload.ACTIVITYSTATUS) Then
                Throw New M4NException(New Exception(), "Can not use load. It is assigned to another activity.", "Can not use load. It is assigned to another activity.")
            End If
        End If
        If pQtyToExplode > parentload.UNITS Then
            Throw New M4NException(New Exception, "Quantity to expload greater than the load units", "Quantity to expload greater than the load units")
        End If
        'Explode the returned kits - create loads of BOM first
        For Each dr As DataRow In pBomLoadsDS.Tables(0).Rows
            Dim woBom As WorkOrderBOM = _workorderbomcollection.PartSku(dr("PARTSKU"))
            Dim bomLoad As New Load
            dr("SKU") = woBom.PARTSKU
            Dim oAtt As AttributesCollection = SkuClass.ExtractReceivingAttributes(dr)
            bomLoad.ProduceForWorkOrder(_consignee, _orderid, woBom.PARTSKU, _location, _warehousearea, (woBom.PARTQTY * pQtyToExplode), "", _
                dr("LOADUOM"), dr("STATUS"), oAtt, pUser)
        Next
        'reduce qty from parent load
        parentload.ConsumeForWorkOrder(_orderid, pQtyToExplode, pUser)
        'Update the completed qty and edituser
        _editdate = DateTime.Now
        _edituser = pUser
        _completedqty = _completedqty + pQtyToExplode
        If STATUS < WorkOrderStatus.Released And _completedqty < _modifiedqty Then
            STATUS = WorkOrderStatus.Released
        ElseIf _completedqty = _modifiedqty Then
            STATUS = WorkOrderStatus.Complete
        End If
        Dim SQL As String = String.Format("UPDATE WORKORDERHEADER SET COMPLETEDQTY ={0}, EDITDATE ={1}, EDITUSER ={2}, STATUS={3} WHERE {4}", _
           Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_status), WhereClause)
        DataInterface.RunSQL(SQL)
        'Raise WorkOrderExecuted Event
        RaiseWorkOrderEvent(oldStat, WMSEvents.EventType.WorkOrderExecuted, pUser)
    End Sub

    Private Function VerifyBomSkuDS(ByVal pBomSkuDS As DataSet) As Boolean
        For Each wobom As WorkOrderBOM In _workorderbomcollection
            Dim found As Boolean = False
            For Each dr As DataRow In pBomSkuDS.Tables(0).Rows
                If dr("CONSIGNEE") = wobom.Consignee And dr("PARTSKU") = wobom.PartSku Then
                    found = True
                End If
            Next
            If Not found Then Return False
        Next
        Return True
    End Function

    Private Sub RaiseWorkOrderEvent(ByVal pOldStatus As String, ByVal WOEventType As WMS.Logic.WMSEvents.EventType, ByVal pUser As String)
        Dim aq As EventManagerQ = New EventManagerQ
        Dim EventType As Int32 = WOEventType
        aq.Add("EVENT", WOEventType)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WOEXEC)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTTYPE", _documenttype)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", _location)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", pOldStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", _location)
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Add("UNITPRICE", 0)
        aq.Add("UOM", "")
        'aq.Add(_loadattributes.Attributes.ToNameValueCollection())
        aq.Send(WMS.Lib.Actions.Audit.WOEXEC)

    End Sub

    Public Sub ExecuteValueAddedLoads(ByVal pLoadid As String, ByVal pNewLoadStatus As String, ByVal pQty As Decimal, ByVal pLoadAttributes As AttributesCollection, ByVal pUser As String)
        Dim oldStat As String = _status
        Dim oLoad As Load = New Load(pLoadid)
        Dim oldLoadStatus As String = oLoad.STATUS
        If oLoad.UNITS = pQty Then
            oLoad.setStatus(pNewLoadStatus, "", pUser)
            oLoad.setAttributes(pLoadAttributes, pUser)
        ElseIf oLoad.UNITS > pQty Then
            oLoad.Split(oLoad.LOCATION, oLoad.WAREHOUSEAREA, pQty, "", pUser)
            oLoad.setStatus(pNewLoadStatus, "", pUser)
            oLoad.setAttributes(pLoadAttributes, pUser)
        Else
            Throw New M4NException(New Exception, "Quantity cannot be greater than the original quantity", "Quantity cannot be greater than the original quantity")
        End If

        _completedqty = _completedqty + pQty
        'Update the completed qty and edituser
        _editdate = DateTime.Now
        _edituser = pUser
        If Status < WorkOrderStatus.Released And _completedqty < _modifiedqty Then
            Status = WorkOrderStatus.Released
        ElseIf _completedqty = _modifiedqty Then
            Status = WorkOrderStatus.Complete
        End If
        Dim SQL As String = String.Format("UPDATE WORKORDERHEADER SET COMPLETEDQTY ={0}, EDITDATE ={1}, EDITUSER ={2}, STATUS={3} WHERE {4}", _
           Made4Net.Shared.Util.FormatField(_completedqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_status), WhereClause)
        DataInterface.RunSQL(SQL)
        'Raise WorkOrderExecuted Event
        RaiseWorkOrderEvent(oldStat, WMSEvents.EventType.WorkOrderExecuted, pUser)
    End Sub

#End Region

#Region "Plan / Release / Complete"

#End Region

#Region "Work Order BOM Collection"

    Public Sub GenerateBomBySku(ByVal pUser As String)
        If Me.DOCUMENTTYPE = WorkOrderDocumentType.ValueAdded Then
            Throw New M4NException(New Exception, String.Format("Can not populate BOM for value added work orders [Order ID:{0}].", Me.ORDERID), String.Format("Can not populate bom for value added work orders.Order ID:{0}", Me.ORDERID))
        End If
        Dim oSku As New SKU(_consignee, _sku)
        For Each tmpBOM As WMS.Logic.SKU.SKUBOM In oSku.BOM
            AddPartSku(tmpBOM.PARTSKU, tmpBOM.PARTQTY, "", Nothing, pUser)
        Next
    End Sub

    Public Sub AddPartSku(ByVal pPartSku As String, ByVal pPartQty As Decimal, ByVal pInventoryStatus As String, ByVal pAttributesCollection As AttributesCollection, ByVal pUser As String)
        If Me.STATUS < WorkOrderStatus.Released Then
            If Me.PARTSKUCOLLECTION Is Nothing Then
                _workorderbomcollection = New WorkOrderBOMCollection(_consignee, _orderid)
            End If
            Me.PARTSKUCOLLECTION.CreatePartSku(pPartSku, pPartQty, pInventoryStatus, pAttributesCollection, pUser)
        Else
            Throw New M4NException(New Exception, "Work Order Status Incorrect", "Work Order Status Incorrect")
        End If
    End Sub

    Public Sub EditPartSku(ByVal pPartSku As String, ByVal pPartQty As Decimal, ByVal pInventoryStatus As String, ByVal pAttributesCollection As AttributesCollection, ByVal pUser As String)
        If Me.STATUS < WorkOrderStatus.Released Then
            Me.PARTSKUCOLLECTION.EditPartSku(pPartSku, pPartQty, pInventoryStatus, pAttributesCollection, pUser)
        Else
            Throw New M4NException(New Exception, "Work Order Status Incorrect", "Work Order Status Incorrect")
        End If
    End Sub

    Public Sub DeletePartSku(ByVal pPartSku As String, ByVal pUser As String)
        If Me.Status < WorkOrderStatus.Released Then
            Me.PartSkuCollection.DeletePartSku(pPartSku, pUser)
        Else
            Throw New M4NException(New Exception, "Work Order Status Incorrect", "Work Order Status Incorrect")
        End If
    End Sub


#End Region

#Region "Enum Conversions"

    Public Shared Function WorkOrderTypeToString(ByVal pOrderCategory As WorkOrderDocumentType) As String
        Select Case pOrderCategory
            Case WMS.Logic.WorkOrderDocumentType.Assembly
                Return "ASMBL"
            Case WMS.Logic.WorkOrderDocumentType.DisAssembly
                Return "DISASMBL"
            Case WMS.Logic.WorkOrderDocumentType.ValueAdded
                Return "VALUEADDED"
        End Select
    End Function

    Public Shared Function WorkOrderTypeFromString(ByVal pOrderCategory As String) As WorkOrderDocumentType
        Select Case pOrderCategory.ToUpper
            Case "ASMBL"
                Return WMS.Logic.WorkOrderDocumentType.Assembly
            Case "DISASMBL"
                Return WMS.Logic.WorkOrderDocumentType.DisAssembly
            Case "VALUEADDED"
                Return WMS.Logic.WorkOrderDocumentType.ValueAdded
        End Select
    End Function

    Public Shared Function WorkOrderStatusToString(ByVal pOrderStatus As WorkOrderStatus) As String
        Select Case pOrderStatus
            Case WMS.Logic.WorkOrderStatus.Cancelled
                Return "CANCELLED"
            Case WMS.Logic.WorkOrderStatus.Complete
                Return "COMPLETE"
            Case WMS.Logic.WorkOrderStatus.[New]
                Return "NEW"
            Case WMS.Logic.WorkOrderStatus.Planned
                Return "PLANNED"
            Case WMS.Logic.WorkOrderStatus.Released
                Return "RELEASED"
        End Select
        Return String.Empty
    End Function

    Public Shared Function WorkOrderStatusFromString(ByVal pOrderStatus As String) As WorkOrderStatus
        Select Case pOrderStatus.ToUpper
            Case "CANCELLED"
                Return WMS.Logic.WorkOrderStatus.Cancelled
            Case "COMPLETE"
                Return WMS.Logic.WorkOrderStatus.Complete
            Case "NEW"
                Return WMS.Logic.WorkOrderStatus.[New]
            Case "PLANNED"
                Return WMS.Logic.WorkOrderStatus.Planned
            Case "RELEASED"
                Return WMS.Logic.WorkOrderStatus.Released
        End Select
        Return String.Empty
    End Function

#End Region

#End Region

End Class

#Region "WorkOrderCategory"

Public Enum WorkOrderDocumentType
    [Assembly]
    DisAssembly
    ValueAdded
End Enum

Public Enum WorkOrderStatus
    [New]
    Planned
    Released
    Complete
    Cancelled
End Enum

#End Region

#End Region

#Region "WorkOrderBOM Collection"

<CLSCompliant(False)> Public Class WorkOrderBOMCollection
    Inherits ArrayList

#Region "Variables"

    Protected _consignee As String
    Protected _orderid As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As WorkOrderBOM
        Get
            Return CType(MyBase.Item(index), WorkOrderBOM)
        End Get
    End Property

    Public ReadOnly Property PartSku(ByVal pPartSku As String) As WorkOrderBOM
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).PartSku = pPartSku Then
                    Return (CType(MyBase.Item(i), WorkOrderBOM))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pConsignee As String, ByVal pOrderId As String, Optional ByVal LoadAll As Boolean = True)
        _consignee = pConsignee
        _orderid = pOrderId
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select * from WorkOrderBOM where consignee = '" & _consignee & "' and ORDERID = '" & _orderid & "'"
        DataInterface.FillDataset(SQL, dt)
        'For Each dr In dt.Rows
        'Me.Add(New WorkOrderBOM(_consignee, _orderid, dr.Item("PARTSKU"), LoadAll))
        'Next
        Dim dtAttributes As New DataTable
        Dim drAttribute As DataRow
        SQL = "SELECT * FROM attribute where pkeytype = '" & InventoryAttributeBase.AttributeKeyType.WorkOrder.ToString() & "' and pkey1 = '" & _consignee & "' and pkey2 = '" & _orderid & "'"
        DataInterface.FillDataset(SQL, dtAttributes)
        For Each dr In dt.Rows
            Try
                drAttribute = dtAttributes.Select(String.Format("pkey3 = '{0}'", dr("partsku")))(0)
            Catch ex As Exception
                drAttribute = Nothing
            End Try
            Me.Add(New WorkOrderBOM(dr, drAttribute))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function Add(ByVal pObject As WorkOrderBOM) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Remove(ByVal pObject As WorkOrderBOM)
        MyBase.Remove(pObject)
    End Sub

    Public Sub CreatePartSku(ByVal pPartSku As String, ByVal pPartQty As Double, ByVal pInventoryStatus As String, ByVal pAttributeCollection As AttributesCollection, ByVal pUser As String)
        Dim woBom As New WorkOrderBOM
        woBom.Create(_consignee, _orderid, pPartSku, pPartQty, pInventoryStatus, pAttributeCollection, pUser)
        Add(woBom)
    End Sub

    Public Sub EditPartSku(ByVal pPartSku As String, ByVal pPartQty As Double, ByVal pInventoryStatus As String, ByVal pAttributeCollection As AttributesCollection, ByVal pUser As String)
        If Me.PartSku(pPartSku) Is Nothing Then
            Throw New M4NException(New Exception, "WorkOrder BOM does not Exists", "WorkOrder BOM does not Exists")
        End If
        Me.PartSku(pPartSku).Edit(pPartQty, pInventoryStatus, pAttributeCollection, pUser)
    End Sub

    Public Sub DeletePartSku(ByVal pPartSku As String, ByVal pUser As String)
        If Me.PartSku(pPartSku) Is Nothing Then
            Throw New M4NException(New Exception, "WorkOrder BOM does not Exists", "WorkOrder BOM does not Exists")
        End If
        Me.PartSku(pPartSku).Delete(pUser)
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        For Each tmpBom As WorkOrderBOM In Me
            tmpBom.Cancel(pUser)
        Next
    End Sub

#End Region

End Class

#End Region

#Region "WorkOrderBOM"

<CLSCompliant(False)> Public Class WorkOrderBOM

#Region "Variables"

    Private _consignee As String
    Private _orderid As String
    Private _partsku As String
    Private _partqty As Decimal
    Private _inventorystatus As String
    Private _adddate As DateTime
    Private _adduser As String
    Private _editdate As DateTime
    Private _edituser As String
    Private _attributes As InventoryAttributeBase

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "' AND PARTSKU = '" & _partsku & "'"
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

    Public Property ORDERID() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
        End Set
    End Property

    Public Property PARTQTY() As Decimal
        Get
            Return _partqty
        End Get
        Set(ByVal Value As Decimal)
            _partqty = Value
        End Set
    End Property

    Public Property PARTSKU() As String
        Get
            Return _partsku
        End Get
        Set(ByVal Value As String)
            _partsku = Value
        End Set
    End Property

    Public Property INVENTORYSTATUS() As String
        Get
            Return _inventorystatus
        End Get
        Set(ByVal Value As String)
            _inventorystatus = Value
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

    Public Property ATTRIBUTES() As InventoryAttributeBase
        Get
            Return _attributes
        End Get
        Set(ByVal value As InventoryAttributeBase)
            _attributes = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pConsignee As String, ByVal pOrderID As String, ByVal pPartSku As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pConsignee
        _orderid = pOrderID
        _partsku = pPartSku
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal pObjectDr As DataRow, ByVal pAttributeDr As DataRow)
        Load(pObjectDr, pAttributeDr)
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM WORKORDERBOM WHERE" & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "WorkOrderBOM Does not Exists", "WorkOrderBOM Does not Exists")
        Else
            dr = dt.Rows(0)
        End If
        If Not dr.IsNull("INVENTORYSTATUS") Then _inventorystatus = dr.Item("INVENTORYSTATUS")
        If Not dr.IsNull("PARTQTY") Then _partqty = dr.Item("PARTQTY")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        SQL = "SELECT * FROM attribute where pkeytype = '" & InventoryAttributeBase.AttributeKeyType.WorkOrderBOM & "' and pkey1 = '" & _consignee & "' and pkey2 = '" & _orderid & "' and pkey3='" & _partsku & "'"
        Dim attributeDataTable As New DataTable
        DataInterface.FillDataset(SQL, attributeDataTable)
        If attributeDataTable.Rows.Count = 1 Then
            _attributes = New InventoryAttributeBase(attributeDataTable.Rows(0))
        End If

    End Sub

    Protected Sub Load(ByVal pObjectDr As DataRow, ByVal pAttributeDr As DataRow)
        If Not pObjectDr.IsNull("ORDERID") Then _orderid = pObjectDr.Item("ORDERID")
        If Not pObjectDr.IsNull("CONSIGNEE") Then _consignee = pObjectDr.Item("CONSIGNEE")
        If Not pObjectDr.IsNull("PARTSKU") Then _partsku = pObjectDr.Item("PARTSKU")
        If Not pObjectDr.IsNull("PARTQTY") Then _partqty = pObjectDr.Item("PARTQTY")
        If Not pObjectDr.IsNull("INVENTORYSTATUS") Then _inventorystatus = pObjectDr.Item("INVENTORYSTATUS")
        If Not pObjectDr.IsNull("ADDDATE") Then _adddate = pObjectDr.Item("ADDDATE")
        If Not pObjectDr.IsNull("ADDUSER") Then _adduser = pObjectDr.Item("ADDUSER")
        If Not pObjectDr.IsNull("EDITDATE") Then _editdate = pObjectDr.Item("EDITDATE")
        If Not pObjectDr.IsNull("EDITUSER") Then _edituser = pObjectDr.Item("EDITUSER")

        If Not pAttributeDr Is Nothing Then
            _attributes = New InventoryAttributeBase(pAttributeDr)
        End If
    End Sub

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pPartSku As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from WORKORDERBOM where consignee = '{0}' and orderid = '{1}' and partsku='{2}'", pConsignee, pOrderId, pPartSku)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "Save / Create / Edit"

    Public Sub Save(ByVal pUser As String)
        Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        If WorkOrderHeader.Exists(_consignee, _orderid) Then
            sql = String.Format("UPDATE WORKORDERBOM SET PARTQTY={0},INVENTORYSTATUS={1}, EDITDATE ={2}, EDITUSER = {3} where {4}", _
                Made4Net.Shared.Util.FormatField(_partqty), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        Else
            _adddate = DateTime.Now
            _adduser = pUser
            sql = String.Format("INSERT INTO WORKORDERBOM(CONSIGNEE, ORDERID, PARTSKU, PARTQTY, INVENTORYSTATUS, ADDDATE, ADDUSER, EDITDATE, EDITUSER)" & _
                " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})", _
                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_partsku), _
                Made4Net.Shared.Util.FormatField(_partqty), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        End If
        DataInterface.RunSQL(sql)

        If Not _attributes Is Nothing Then
            _attributes.Save(pUser)
        End If
    End Sub

    Public Sub Create(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pPartSku As String, ByVal pPartQty As Decimal, _
                ByVal pInventoryStatus As String, ByVal pAttributeCollection As AttributesCollection, ByVal pUser As String)
        If Not WMS.Logic.Consignee.Exists(pConsignee) Then
            Throw New M4NException(New Exception, "Consignee does not exists", "Consignee does not exists")
        End If
        If Not WorkOrderHeader.Exists(pConsignee, pOrderid) Then
            Throw New M4NException(New Exception, "WorkOrder does not Exists", "WorkOrder does not Exists")
        End If
        If Not SKU.Exists(pConsignee, pPartSku) Then
            Throw New M4NException(New Exception, "SKU does not Exists", "SKU does not Exists")
        End If
        If WorkOrderBOM.Exists(pConsignee, pOrderid, pPartSku) Then
            Throw New M4NException(New Exception, "WorkOrder BOM Already Exists", "WorkOrder BOM Already Exists")
        End If
        _consignee = pConsignee
        _orderid = pOrderid
        _partsku = pPartSku
        _partqty = pPartQty
        _inventorystatus = pInventoryStatus
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        validate(False)

        Dim SQL As String = String.Format("INSERT INTO WORKORDERBOM(CONSIGNEE, ORDERID, PARTSKU, PARTQTY, INVENTORYSTATUS, ADDDATE, ADDUSER, EDITDATE, EDITUSER)" & _
                " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})", _
                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_partsku), _
                Made4Net.Shared.Util.FormatField(_partqty), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)

        If Not pAttributeCollection Is Nothing Then
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrderBOM, _consignee, _orderid, _partsku)
            _attributes.Add(pAttributeCollection)
            _attributes.Save(pUser)
        End If

    End Sub

    Public Sub Edit(ByVal pPartQty As Decimal, ByVal pInventoryStatus As String, ByVal pAttributesCollection As AttributesCollection, ByVal pUser As String)
        If Not WorkOrderBOM.Exists(_consignee, _orderid, _partsku) Then
            Throw New M4NException(New Exception, "WorkOrder BOM does not Exists", "WorkOrder BOM does not Exists")
        End If

        _partqty = pPartQty
        _inventorystatus = pInventoryStatus
        _edituser = pUser
        _editdate = DateTime.Now

        validate(True)

        Dim SQL As String = String.Format("UPDATE WORKORDERBOM SET PARTQTY={0}, INVENTORYSTATUS={1}, EDITDATE ={2}, EDITUSER = {3} where {4}", _
                Made4Net.Shared.Util.FormatField(_partqty), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        If Not pAttributesCollection Is Nothing Then
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.WorkOrderBOM, _consignee, _orderid, _partsku)
            _attributes.Attributes.Clear()
            _attributes.Add(pAttributesCollection)
            _attributes.Save(pUser)
        End If
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        If Not WorkOrderBOM.Exists(_consignee, _orderid, _partsku) Then
            Throw New M4NException(New Exception, "WorkOrder BOM does not Exists", "WorkOrder BOM does not Exists")
        End If

        _partqty = 0
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String = String.Format("UPDATE WORKORDERBOM SET PARTQTY={0}, EDITDATE ={1}, EDITUSER = {2} where {3}", _
                Made4Net.Shared.Util.FormatField(_partqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Delete(ByVal pUser As String)
        If Not WorkOrderBOM.Exists(_consignee, _orderid, _partsku) Then
            Throw New M4NException(New Exception, "WorkOrder BOM does not Exists", "WorkOrder BOM does not Exists")
        End If

        Dim SQL As String = String.Format("delete from WORKORDERBOM where {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Private Sub validate(ByVal pIsEdit As Boolean)
        If pIsEdit Then
            If Not WorkOrderBOM.Exists(_consignee, _orderid, _partsku) Then
                Throw New M4NException(New Exception, "WorkOrder BOM does not Exists.", "WorkOrder BOM does not Exists.")
            End If
        Else
            If Not WMS.Logic.Consignee.Exists(_consignee) Then
                Throw New M4NException(New Exception, "Consignee does not exists.", "Consignee does not exists.")
            End If
            If Not WorkOrderHeader.Exists(_consignee, _orderid) Then
                Throw New M4NException(New Exception, "WorkOrder does not Exists.", "WorkOrder does not Exists.")
            End If
            If Not SKU.Exists(_consignee, _partsku) Then
                Throw New M4NException(New Exception, "SKU does not Exists.", "SKU does not Exists.")
            End If
            If WorkOrderBOM.Exists(_consignee, _orderid, _partsku) Then
                Throw New M4NException(New Exception, "WorkOrder BOM Already Exists.", "WorkOrder BOM Already Exists.")
            End If
        End If
        If _partqty <= 0 Then
            Throw New M4NException(New Exception, "Part quantity must be bigger than 0.", "Part quantity must be bigger than 0.")
        End If
    End Sub

#End Region

#End Region

End Class

#End Region

