Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared
Imports WMS.Logic

#Region "OUTBOUNDORHEADER"

<CLSCompliant(False)> Public Class OutboundOrderHeader

#Region "Inner Classes"

#Region "OutboundOrderDetail"

    <CLSCompliant(False)> Public Class OutboundOrderDetail

#Region "Variables"

#Region "Primary Keys"

        Protected _consignee As String = String.Empty
        Protected _orderid As String = String.Empty
        Protected _orderline As Int32

#End Region

#Region "Other Fields"

        Protected _referenceordline As Int32
        Protected _sku As String = String.Empty
        Protected _inventorystatus As String = String.Empty
        Protected _qtyoriginal As Decimal
        Protected _qtymodified As Decimal
        Protected _qtyallocated As Decimal
        Protected _qtypicked As Decimal
        Protected _qtystaged As Decimal
        Protected _qtypacked As Decimal
        Protected _qtyverified As Decimal
        Protected _qtyloaded As Decimal
        Protected _qtyshipped As Decimal
        Protected _qtysoftallocated As Decimal
        Protected _adddate As DateTime
        Protected _exploadedflag As Int32
        Protected _inputqty As Decimal
        Protected _inputqtyuom As String = String.Empty
        Protected _inputsku As String = String.Empty
        Protected _notes As String = String.Empty
        Protected _originalsku As String = String.Empty
        Protected _route As String
        Protected _stopnumber As Int32
        Protected _warehousearea As String

        Protected _adduser As String = String.Empty
        Protected _editdate As DateTime
        Protected _edituser As String = String.Empty
        Protected _attributes As InventoryAttributeBase

#End Region

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return " CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "' And ORDERLINE = '" & _orderline & "'"
            End Get
        End Property
        Public ReadOnly Property WhereClauseRouteStop() As String
            Get
                Return " CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "'"
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

        Public Property ORDERLINE() As Int32
            Get
                Return _orderline
            End Get
            Set(ByVal Value As Int32)
                _orderline = Value
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

        Public Property SKU() As String
            Get
                Return _sku
            End Get
            Set(ByVal Value As String)
                _sku = Value
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

        Public Property QTYORIGINAL() As Decimal
            Get
                Return _qtyoriginal
            End Get
            Set(ByVal Value As Decimal)
                _qtyoriginal = Value
            End Set
        End Property

        Public Property QTYMODIFIED() As Decimal
            Get
                Return _qtymodified
            End Get
            Set(ByVal Value As Decimal)
                _qtymodified = Value
            End Set
        End Property

        Public Property QTYALLOCATED() As Decimal
            Get
                Return _qtyallocated
            End Get
            Set(ByVal Value As Decimal)
                _qtyallocated = Value
            End Set
        End Property

        Public Property QTYPICKED() As Decimal
            Get
                Return _qtypicked
            End Get
            Set(ByVal Value As Decimal)
                _qtypicked = Value
            End Set
        End Property

        Public Property QTYVERIFIED() As Decimal
            Get
                Return _qtyverified
            End Get
            Set(ByVal Value As Decimal)
                _qtyverified = Value
            End Set
        End Property

        Public Property QTYSTAGED() As Decimal
            Get
                Return _qtystaged
            End Get
            Set(ByVal Value As Decimal)
                _qtystaged = Value
            End Set
        End Property

        Public Property QTYPACKED() As Decimal
            Get
                Return _qtypacked
            End Get
            Set(ByVal Value As Decimal)
                _qtypacked = Value
            End Set
        End Property

        Public Property QTYLOADED() As Decimal
            Get
                Return _qtyloaded
            End Get
            Set(ByVal Value As Decimal)
                _qtyloaded = Value
            End Set
        End Property

        Public Property QTYSOFTALLOCATED() As Decimal
            Get
                Return _qtysoftallocated
            End Get
            Set(ByVal Value As Decimal)
                _qtysoftallocated = Value
            End Set
        End Property

        Public Property QTYSHIPPED() As Decimal
            Get
                Return _qtyshipped
            End Get
            Set(ByVal Value As Decimal)
                _qtyshipped = Value
            End Set
        End Property

        Public ReadOnly Property QTYLEFTTOFULLFILL() As Decimal
            Get
                Return QTYMODIFIED - QTYALLOCATED - QTYPICKED - QTYSTAGED - QTYPACKED - QTYLOADED - QTYVERIFIED - QTYSHIPPED - QTYLOADED
            End Get
        End Property

        Public ReadOnly Property QTYPROCESSED() As Decimal
            Get
                Return QTYPICKED + QTYPACKED + QTYSTAGED + QTYLOADED + QTYVERIFIED + QTYSHIPPED + QTYLOADED
            End Get
        End Property

        Public ReadOnly Property FULLYPLANNED() As Boolean
            Get
                If QTYMODIFIED - QTYALLOCATED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property FULLYPICKED() As Boolean
            Get
                If QTYMODIFIED - QTYPICKED - QTYPACKED - QTYSTAGED - QTYLOADED - QTYVERIFIED - QTYSHIPPED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property FULLYLOADED() As Boolean
            Get
                If QTYMODIFIED - QTYLOADED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property FULLYSHIPPED() As Boolean
            Get
                If QTYMODIFIED - QTYSHIPPED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property FULLYPACKED() As Boolean
            Get
                If QTYMODIFIED - QTYPACKED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property FULLYSTAGED() As Boolean
            Get
                If QTYMODIFIED - QTYSTAGED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public ReadOnly Property FULLYVERIFIED()
            Get
                If QTYMODIFIED - QTYVERIFIED > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public Property EXPLOADEDFLAG() As Int32
            Get
                Return _exploadedflag
            End Get
            Set(ByVal Value As Int32)
                _exploadedflag = Value
            End Set
        End Property

        Public Property INPUTQTY() As Decimal
            Get
                Return _inputqty
            End Get
            Set(ByVal Value As Decimal)
                _inputqty = Value
            End Set
        End Property

        Public Property INPUTUOM() As String
            Get
                Return _inputqtyuom
            End Get
            Set(ByVal Value As String)
                _inputqtyuom = Value
            End Set
        End Property

        Public Property INPUTSKU() As String
            Get
                Return _inputsku
            End Get
            Set(ByVal Value As String)
                _inputsku = Value
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

        Public Property ORIGINALSKU() As String
            Get
                Return _originalsku
            End Get
            Set(ByVal Value As String)
                _originalsku = Value
            End Set
        End Property

        Public ReadOnly Property QTYFOROVERPICKICKING() As Decimal
            Get
                Dim oSku As New WMS.Logic.SKU(_consignee, _sku)
                Return oSku.OVERPICKPCT * _qtymodified
            End Get
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

        Public Property Attributes() As InventoryAttributeBase
            Get
                Return _attributes
            End Get
            Set(ByVal Value As InventoryAttributeBase)
                _attributes = Value
            End Set
        End Property

        Public Property WarehouseArea() As String
            Get
                Return _warehousearea
            End Get
            Set(ByVal value As String)
                _warehousearea = value
            End Set
        End Property

        Public ReadOnly Property ORDERSTATUS() As String
            Get
                Return DataInterface.ExecuteScalar("SELECT STATUS FROM OUTBOUNDORHEADER WHERE CONSIGNEE='" & _consignee & "' AND ORDERID='" & _orderid & "'")
            End Get
        End Property

        Public ReadOnly Property Wave() As String
            Get
                Dim sql As String = String.Format("select isnull(wave,'') from wavedetail where {0}", WhereClause)
                Return DataInterface.ExecuteScalar(sql)
            End Get
        End Property

        Public ReadOnly Property Shipment() As String
            Get
                Return ""
            End Get
        End Property

        Public ReadOnly Property ShipmentsAssignedTotalQuantity() As Decimal
            Get
                Dim sql As String = String.Format("select isnull(sum(units),0) from shipmentdetail where {0}", WhereClause)
                Return DataInterface.ExecuteScalar(sql)
            End Get
        End Property

        Public ReadOnly Property OpenQtyLeftToAssignToShipment() As Decimal
            Get
                Return QTYMODIFIED - ShipmentsAssignedTotalQuantity
            End Get
        End Property

        Public Property ROUTE() As String
            Get
                Return _route
            End Get
            Set(ByVal Value As String)
                _route = Value
            End Set
        End Property

        Public Property STOPNUMBER() As Int32
            Get
                Return _stopnumber
            End Get
            Set(ByVal Value As Int32)
                _stopnumber = Value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder)
        End Sub

        Public Sub New(ByVal pCONSIGNEE As String, ByVal pORDERID As String, ByVal pORDERLINE As Int32, Optional ByVal LoadObj As Boolean = True)
            _consignee = pCONSIGNEE
            _orderid = pORDERID
            _orderline = pORDERLINE
            If LoadObj Then
                Load()
            End If
        End Sub
        Public Sub New(ByVal pCONSIGNEE As String, ByVal pORDERID As String, ByVal RouteEdit As String, Optional ByVal LoadObj As Boolean = True)
            _consignee = pCONSIGNEE
            _orderid = pORDERID
            ' _orderline = pORDERLINE
            If LoadObj And RouteEdit = "Route" Then
                LoadRouteStop()
            End If
        End Sub

        Public Sub New(ByVal dr As DataRow, ByVal attributeDr As DataRow)
            Load(dr, attributeDr)
        End Sub

#End Region

#Region "Methods"

#Region "Accessors"

        Private Function getNextLine() As Int32
            Return DataInterface.ExecuteScalar("Select isnull(max(orderline),0) + 1 from outboundordetail where orderid = '" & _orderid & "'")
        End Function

        Public Shared Function Exists(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32) As Boolean
            Dim sql As String = String.Format("Select count(1) from OutboundOrDetail where Consignee = '{0}' and ORDERID = '{1}' and orderline = {2}", pConsignee, pOrderId, pOrderLine)
            Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Public Shared Function GetOutboundOrderDetail(ByVal pCONSIGNEE As String, ByVal pORDERID As String, ByVal pORDERLINE As Int32) As OutboundOrderDetail
            Return New OutboundOrderDetail(pCONSIGNEE, pORDERID, pORDERLINE)
        End Function

        Public Function GetOutboundOrderHeader() As OutboundOrderHeader
            Return New OutboundOrderHeader(_consignee, _orderid)
        End Function

        Public Sub Delete(ByVal pUser As String)
            _attributes.Delete()
            Dim sql As String = String.Format("delete from outboundordetail where " & WhereClause)
            DataInterface.RunSQL(sql)
            Dim wvsql As String = String.Format("delete from wavedetail where " & WhereClause)
            DataInterface.RunSQL(wvsql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderLineDeleted)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDLNDELETED)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", _orderline)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", _qtymodified)
            aq.Add("FROMSTATUS", "")
            aq.Add("NOTES", _notes)
            aq.Add("SKU", _sku)
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", _qtymodified)
            aq.Add("TOSTATUS", "")
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDLNDELETED)
        End Sub

        Protected Sub Load()
            Dim SQL As String = "SELECT * FROM OUTBOUNDORDETAIL WHERE " & WhereClause
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Throw New M4NException(New Exception, "No details exist for the specified outbound order header.", "No details exist for the specified outbound order header.")
            End If
            dr = dt.Rows(0)
            'Detail Attributes
            Dim dtAtt As New DataTable
            Dim drAtt As DataRow = Nothing
            SQL = String.Format("SELECT * from attribute where pkeytype ='OUTBOUND' and pkey1 ='{0}' AND PKEY2='{1}' and pkey3='{2}'", _consignee, _orderid, _orderline)
            DataInterface.FillDataset(SQL, dtAtt)
            If dtAtt.Rows.Count > 0 Then
                drAtt = dtAtt.Rows(0)
            End If
            Load(dr, drAtt)
        End Sub

        Protected Sub Load(ByVal dr As DataRow, ByVal attributeDr As DataRow)
            If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
            If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
            If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
            If Not dr.IsNull("REFERENCEORDLINE") Then _referenceordline = dr.Item("REFERENCEORDLINE")
            If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
            If Not dr.IsNull("INVENTORYSTATUS") Then _inventorystatus = dr.Item("INVENTORYSTATUS")
            If Not dr.IsNull("QTYORIGINAL") Then _qtyoriginal = dr.Item("QTYORIGINAL")
            If Not dr.IsNull("QTYMODIFIED") Then _qtymodified = dr.Item("QTYMODIFIED")
            If Not dr.IsNull("QTYALLOCATED") Then _qtyallocated = dr.Item("QTYALLOCATED")
            If Not dr.IsNull("QTYPICKED") Then _qtypicked = dr.Item("QTYPICKED")
            If Not dr.IsNull("QTYPACKED") Then _qtypacked = dr.Item("QTYPACKED")
            If Not dr.IsNull("QTYVERIFIED") Then _qtyverified = dr.Item("QTYVERIFIED")
            If Not dr.IsNull("QTYSTAGED") Then _qtystaged = dr.Item("QTYSTAGED")
            If Not dr.IsNull("QTYLOADED") Then _qtyloaded = dr.Item("QTYLOADED")
            If Not dr.IsNull("QTYSHIPPED") Then _qtyshipped = dr.Item("QTYSHIPPED")
            If Not dr.IsNull("QTYSOFTALLOCATED") Then _qtysoftallocated = dr.Item("QTYSOFTALLOCATED")
            If Not dr.IsNull("INPUTSKU") Then _inputsku = dr.Item("INPUTSKU")
            If Not dr.IsNull("LINEOUM") Then _inputqtyuom = dr.Item("LINEOUM")
            If Not dr.IsNull("UOMQTY") Then _inputqty = dr.Item("UOMQTY")
            If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
            If Not dr.IsNull("originalsku") Then _originalsku = dr.Item("originalsku")
            If Not dr.IsNull("STOPNUMBER") Then _stopnumber = dr.Item("STOPNUMBER")
            If Not dr.IsNull("ROUTE") Then _route = dr.Item("ROUTE")
            If Not dr.IsNull("EXPLOADEDFLAG") Then _exploadedflag = dr.Item("EXPLOADEDFLAG")
            If Not dr.IsNull("WarehouseArea") Then _warehousearea = dr.Item("WarehouseArea")
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

            If Not attributeDr Is Nothing Then
                _attributes = New InventoryAttributeBase(attributeDr)
            Else
                _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder)
                '   _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder, _consignee, _orderid, _orderline)
            End If
        End Sub

#End Region

#Region "Insert/Update"

        Public Sub Create(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLineNumber As Int32, ByVal pRefOrdLine As Int32, ByVal pSku As String, ByVal pStatus As String, ByVal pQty As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExpolodedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing)

            Dim oSku As WMS.Logic.SKU
            If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
                If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                    'Added for RWMS-1509 and RWMS-1502
                    Dim strErrDesc As String = "Cannot create order detail.Sku " + pInputSku + " does not Exist for consignee " + pConsignee
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order detail.Sku not Exist", strErrDesc)
                    'Ended for RWMS-1509 and RWMS-1502
                    Throw m4nEx
                Else
                    oSku = New WMS.Logic.SKU(pConsignee, pInputSku)
                    _inputsku = pInputSku
                    _sku = oSku.SKU
                End If
            Else
                oSku = New WMS.Logic.SKU(pConsignee, pSku)
                _sku = pSku
            End If
            If pQty < 0 Then
                If pInpupUOM = String.Empty Or pInpupUOM Is Nothing Or pInputQTY < 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity entered for order line", "Invalid quantity entered for order line")
                Else
                    If pInpupUOM <> "" And Not pInpupUOM Is Nothing Then
                        If Not WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pInpupUOM) Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
                        End If
                    End If
                    pQty = oSku.ConvertToUnits(pInpupUOM) * pInputQTY
                    _inputqty = pInputQTY
                    _inputqtyuom = pInpupUOM
                End If
            Else
                'Do nothing, leave supplied quantity as received...
            End If

            _consignee = pConsignee
            _orderid = pOrderId
            _orderline = pLineNumber
            _referenceordline = pRefOrdLine
            _inventorystatus = pStatus
            _qtyoriginal = pQty
            _qtymodified = pQty
            _qtyallocated = 0
            _qtyloaded = 0
            _qtypicked = 0
            _qtyshipped = 0
            _qtyverified = 0
            _qtystaged = 0
            _qtypacked = 0
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser
            _notes = pNotes
            _exploadedflag = pExpolodedFlag
            _route = pRoute
            _stopnumber = pStopNumber
            _warehousearea = pWarehouseArea

            Dim sql As String = String.Format("Insert into OutboundOrdetail(CONSIGNEE,ORDERID,ORDERLINE,REFERENCEORDLINE,SKU,INVENTORYSTATUS,QTYORIGINAL,QTYMODIFIED,QTYALLOCATED,QTYPICKED,QTYSTAGED,QTYVERIFIED,QTYPACKED,QTYLOADED,QTYSHIPPED,ADDDATE,ADDUSER,EDITDATE,EDITUSER,EXPLOADEDFLAG,LINEOUM,UOMQTY, INPUTSKU, NOTES, ROUTE, STOPNUMBER,WAREHOUSEAREA) values " & _
                            "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26})", Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_referenceordline, , True), _
                            Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_qtyoriginal), Made4Net.Shared.Util.FormatField(_qtymodified), Made4Net.Shared.Util.FormatField(_qtyallocated), Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtyverified), Made4Net.Shared.Util.FormatField(_qtypacked), _
                            Made4Net.Shared.Util.FormatField(_qtyloaded), Made4Net.Shared.Util.FormatField(_qtyshipped), _
                            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_exploadedflag), Made4Net.Shared.Util.FormatField(pInpupUOM), Made4Net.Shared.Util.FormatField(pInputQTY), Made4Net.Shared.Util.FormatField(pInputSku), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_warehousearea))

            DataInterface.RunSQL(sql)
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder, _consignee, _orderid, _orderline)
            _attributes.Add(oAttributeCollection)
            _attributes.Save(pUser)

            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OutboundOrderLineCreated
            'em.Add("EVENT", EventType)
            'em.Add("ORDERID", _orderid)
            'em.Add("CONSIGNEE", _consignee)
            'em.Add("ORDERLINE", _orderline)
            'em.Add("USERID", pUser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderLineCreated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDLNINS)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", _orderline)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", pQty)
            aq.Add("FROMSTATUS", "")
            aq.Add("NOTES", pNotes)
            aq.Add("SKU", _sku)
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", pQty)
            aq.Add("TOSTATUS", "")
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDLNINS)
        End Sub

        Public Function InsertDetail(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExplodedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing) As OutboundOrderDetail

            _orderid = pOrderId

            Dim orderLine As Int32 = getNextLine()
            Create(pConsignee, pOrderId, orderLine, pRefOrdLine, pSku, pInvStat, pQty, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExplodedFlag, oAttributeCollection, pWarehouseArea, pInputQTY, pInpupUOM)
            Return Me
        End Function

        Public Function InsertDetail(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrdLine As Int32, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExplodedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing) As OutboundOrderDetail

            _orderid = pOrderId
            If pOrdLine = 0 Then
                Dim orderLine As Int32 = getNextLine()
            Else
                ORDERLINE = pOrdLine
            End If
            Create(pConsignee, pOrderId, ORDERLINE, pRefOrdLine, pSku, pInvStat, pQty, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExplodedFlag, oAttributeCollection, pWarehouseArea, pInputQTY, pInpupUOM)
            Return Me
        End Function

        Public Function EditDetail(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLineNumber As String, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExplodedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, ByVal pInputSku As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInputUOM As String = Nothing) As OutboundOrderDetail

            Dim oSku As WMS.Logic.SKU
            If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
                If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot edit order detail.Sku not Exist", "Cannot edit order detail.Sku not Exist")
                    Throw m4nEx
                Else
                    oSku = New WMS.Logic.SKU(pConsignee, pInputSku)
                    _inputsku = pInputSku
                    _sku = oSku.SKU
                End If
            Else
                oSku = New WMS.Logic.SKU(pConsignee, pSku)
                _sku = pSku
            End If

            If pQty < 0 Then
                If pInputUOM = String.Empty Or pInputUOM Is Nothing Or pInputQTY < 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity entered for order line", "Invalid quantity entered for order line")
                Else
                    If pInputUOM <> "" And Not pInputUOM Is Nothing Then
                        If Not WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pInputUOM) Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
                        End If
                    End If
                    pQty = oSku.ConvertToUnits(pInputUOM) * pInputQTY
                    _inputqty = pInputQTY
                    _inputqtyuom = pInputUOM
                End If
            Else
                'Do nothing, leave supplied quantity as received...
            End If

            _consignee = pConsignee
            _orderid = pOrderId
            _orderline = pLineNumber
            _referenceordline = pRefOrdLine
            _sku = pSku
            _inventorystatus = pInvStat
            _notes = pNotes
            _qtymodified = pQty
            _editdate = DateTime.Now
            _edituser = pUser
            _exploadedflag = pExplodedFlag
            _route = pRoute
            _stopnumber = pStopNumber
            _warehousearea = pWarehouseArea

            Dim sql As String = String.Format("Update OutboundOrdetail set REFERENCEORDLINE={0},SKU={1},INVENTORYSTATUS={2},QTYMODIFIED={3},EDITDATE={4},EDITUSER={5}, EXPLOADEDFLAG={6}, NOTES={7},INPUTQTY={8},INPUTQTYUOM={9}, ROUTE={10},STOPNUMBER={11}, WAREHOUSEAREA={12} where {13}" _
                , Made4Net.Shared.Util.FormatField(_referenceordline, , True), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_qtymodified), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_exploadedflag), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(pInputQTY), Made4Net.Shared.Util.FormatField(pInputUOM), Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_stopnumber), _
                Made4Net.Shared.Util.FormatField(_warehousearea), WhereClause)

            DataInterface.RunSQL(sql)
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder, _consignee, _orderid, _orderline)
            _attributes.PrimaryKey1 = _consignee
            _attributes.PrimaryKey2 = _orderid
            _attributes.PrimaryKey3 = _orderline
            _attributes.Add(oAttributeCollection)
            _attributes.Save(pUser)

            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OutboundOrderLineUpdated
            'em.Add("EVENT", EventType)
            'em.Add("ORDERID", _orderid)
            'em.Add("CONSIGNEE", _consignee)
            'em.Add("ORDERLINE", _orderline)
            'em.Add("USERID", pUser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderLineUpdated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDLNUPD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", _orderline)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", pQty)
            aq.Add("FROMSTATUS", "")
            aq.Add("NOTES", pNotes)
            aq.Add("SKU", _sku)
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", pQty)
            aq.Add("TOSTATUS", "")
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDLNUPD)
            Return Me
        End Function



        Protected Sub LoadRouteStop()
            Dim SQL As String = "SELECT * FROM OUTBOUNDORDETAIL WHERE " & WhereClauseRouteStop
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Throw New M4NException(New Exception, "No details exist for the specified outbound order header.", "No details exist for the specified outbound order header.")
            End If
            dr = dt.Rows(0)

            If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
            If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
            'If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
            If Not dr.IsNull("STOPNUMBER") Then _stopnumber = dr.Item("STOPNUMBER")
            If Not dr.IsNull("ROUTE") Then _route = dr.Item("ROUTE")
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        End Sub

        Public Sub EditRouteStop(ByVal pConsignee As String, ByVal pOrderId As String, _
                 ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String)

            _consignee = pConsignee
            _orderid = pOrderId
            ' _orderline = pLineNumber
            _editdate = DateTime.Now
            _edituser = pUser
            _route = pRoute
            _stopnumber = pStopNumber

            Dim sql As String = String.Format("Update OutboundOrdetail set ROUTE={0},STOPNUMBER={1}, EDITDATE={2},EDITUSER={3}  where {4}" _
                , Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClauseRouteStop)

            DataInterface.RunSQL(sql)
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.OutboundOrder, _consignee, _orderid)
            _attributes.PrimaryKey1 = _consignee
            _attributes.PrimaryKey2 = _orderid
            ' _attributes.PrimaryKey3 = _orderline
            _attributes.Save(pUser)

        End Sub
#End Region

#Region "PICK & PLAN"

        Public Sub SoftAllocate(ByVal units As Decimal, ByVal puser As String)
            Dim sql As String
            _qtysoftallocated += units
            _edituser = puser
            _editdate = DateTime.Now
            sql = String.Format("UPDATE OUTBOUNDORDETAIL SET QTYSOFTALLOCATED = {0},EDITUSER = {1},EDITDATE = {2} WHERE {3}", _
                Made4Net.Shared.Util.FormatField(_qtysoftallocated), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub Allocate(ByVal units As Decimal, ByVal puser As String)
            Dim sql As String
            _qtyallocated += units
            _edituser = puser
            _editdate = DateTime.Now
            sql = String.Format("UPDATE OUTBOUNDORDETAIL SET QTYALLOCATED = {0},EDITUSER = {1},EDITDATE = {2} WHERE {3}", _
                Made4Net.Shared.Util.FormatField(_qtyallocated), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
            SoftAllocate(units, puser)
        End Sub

        Public Function CanPick(ByVal pUnits As Decimal, ByVal oSku As WMS.Logic.SKU) As Boolean
            If QTYMODIFIED * oSku.OVERPICKPCT < QTYPROCESSED + pUnits Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Sub Pick(ByVal units As Decimal, ByVal puser As String)
            Dim oSKu As New WMS.Logic.SKU(_consignee, _sku)
            If _qtymodified * oSKu.OVERPICKPCT < QTYPROCESSED + units Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Order Detail Processed Units cannot exceed the modified units", "Order Detail Processed Units cannot exceed the modified units")
            End If
            Dim sql As String
            _qtyallocated -= units
            If _qtyallocated < 0 Then
                _qtyallocated = 0
            End If
            _qtypicked += units
            _edituser = puser
            _editdate = DateTime.Now

            ' Check if picked quantity is exeptable , in other case throw exception
            If _qtypicked - (_qtymodified * oSKu.OVERPICKPCT) > _qtymodified - _qtystaged - _qtyshipped Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Picked Quantity is greater than open quantity", "Picked Quantity is greater than open quantity")
                Throw m4nEx
            ElseIf _qtypicked < 0 Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Picked Quantity Cannot become negative", "Picked Quantity Cannot become negative")
                Throw m4nEx
            End If
            sql = String.Format("UPDATE OUTBOUNDORDETAIL SET QTYALLOCATED = {0},QTYPICKED = {1},EDITUSER = {2},EDITDATE = {3} WHERE {4}", _
                Made4Net.Shared.Util.FormatField(_qtyallocated), Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub UnPick(ByVal pLoadId As String, ByVal units As Decimal, ByVal puser As String)
            Dim sql As String = String.Empty
            _edituser = puser
            _editdate = DateTime.Now
            ' Get Load activity status
            Dim pLoadActivityStatus As String = DataInterface.ExecuteScalar("SELECT ACTIVITYSTATUS FROM LOADS WHERE LOADID='" & pLoadId & "'")
            ' Un pick can be made when order is in status PICKED, STAGED, Packed ,Loading
            ' we need to check quantities accoring to status of the order
            Select Case pLoadActivityStatus 'ORDERSTATUS
                Case WMS.Lib.Statuses.ActivityStatus.PICKED 'WMS.Lib.Statuses.OutboundOrderHeader.PICKED, WMS.Lib.Statuses.OutboundOrderHeader.PICKING
                    _qtypicked -= units
                    If _qtypicked < 0 Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Picked Quantity Cannot become negative", "Picked Quantity Cannot become negative")
                        Throw m4nEx
                    End If
                    sql = String.Format("update outboundordetail set qtypicked = {0},edituser = {1},editdate = {2} where {3}", _
                        Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                Case WMS.Lib.Statuses.ActivityStatus.STAGED 'WMS.Lib.Statuses.OutboundOrderHeader.STAGED
                    _qtystaged -= units
                    If _qtystaged < 0 Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Staged Quantity Cannot become negative", "Staged Quantity Cannot become negative")
                        Throw m4nEx
                    End If
                    sql = String.Format("update outboundordetail set qtystaged = {0},edituser = {1},editdate = {2} where {3}", _
                        Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                Case WMS.Lib.Statuses.ActivityStatus.PACKED 'WMS.Lib.Statuses.OutboundOrderHeader.PACKED
                    _qtypacked -= units
                    If _qtypacked < 0 Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Packed Quantity Cannot become negative", "Staged Quantity Cannot become negative")
                        Throw m4nEx
                    End If
                    sql = String.Format("update outboundordetail set qtypacked = {0},edituser = {1},editdate = {2} where {3}", _
                        Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                Case WMS.Lib.Statuses.ActivityStatus.LOADED 'WMS.Lib.Statuses.OutboundOrderHeader.LOADING
                    _qtyloaded -= units
                    If _qtyloaded < 0 Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Loaded Quantity Cannot become negative", "Loaded Quantity Cannot become negative")
                        Throw m4nEx
                    End If
                    sql = String.Format("update outboundordetail set qtyloaded = {0},edituser = {1},editdate = {2} where {3}", _
                        Made4Net.Shared.Util.FormatField(_qtyloaded), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                Case WMS.Lib.Statuses.ActivityStatus.VERIFIED 'WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED
                    _qtyverified -= units
                    If _qtyverified < 0 Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Verified Quantity Cannot become negative", "Loaded Quantity Cannot become negative")
                        Throw m4nEx
                    End If
                    sql = String.Format("update outboundordetail set qtyverified = {0},edituser = {1},editdate = {2} where {3}", _
                        Made4Net.Shared.Util.FormatField(_qtyverified), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            End Select

            DataInterface.RunSQL(sql)
        End Sub

        Public Sub Adjust(ByVal adjustedunits As Decimal, ByVal puser As String)
            Dim sql As String
            If adjustedunits > _qtymodified - _qtypicked Then
                Throw New ApplicationException("Modified units is less than Adjusted units")
            End If
            _qtyallocated -= adjustedunits
            _qtymodified -= adjustedunits
            If _qtyallocated < 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity - qty cannot become negative", "Invalid quantity - cannot become negative")
            End If
            If _qtymodified < 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity - qty cannot become negative", "Invalid quantity - cannot become negative")
            End If
            If _qtyallocated < 0 Then
                _qtyallocated = 0
            End If
            _edituser = puser
            _editdate = DateTime.Now
            sql = String.Format("update outboundordetail set qtyallocated = {0},qtymodified = {1},edituser = {2},editdate = {3} where {4}", _
                Made4Net.Shared.Util.FormatField(_qtyallocated), Made4Net.Shared.Util.FormatField(_qtymodified), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub unAllocate(ByVal unallocatedunits As Decimal, ByVal puser As String)
            Dim sql As String
            _qtyallocated -= unallocatedunits
            If _qtyallocated < 0 Then
                _qtyallocated = 0
            End If
            _edituser = puser
            _editdate = DateTime.Now
            sql = String.Format("update outboundordetail set qtyallocated = {0},edituser = {1},editdate = {2} where {3}", _
                Made4Net.Shared.Util.FormatField(_qtyallocated), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub Complete(ByVal puser As String)
            Dim sql As String
            If QTYLEFTTOFULLFILL > 0 Then
                _qtymodified = _qtymodified - QTYLEFTTOFULLFILL
                _edituser = puser
                _editdate = DateTime.Now
                sql = String.Format("update outboundordetail set qtymodified = {0},edituser = {1},editdate = {2} where {3}", _
                    Made4Net.Shared.Util.FormatField(_qtymodified), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                DataInterface.RunSQL(sql)
            End If
        End Sub

        Public Sub CancelExceptions(ByVal pUser As String, Optional ByVal pExceptionQty As Decimal = -1, Optional ByVal pOrdHeader As OutboundOrderHeader = Nothing)
            Dim sql As String
            Dim header As OutboundOrderHeader = pOrdHeader
            If header Is Nothing Then
                header = Me.GetOutboundOrderHeader()
            End If
            If header.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.PLANNING Then
                Throw New M4NException(New Exception(), "Incorrect order status for canceling exceptions.", "Incorrect order status for canceling exceptions.")
            End If
            If QTYLEFTTOFULLFILL > 0 Then
                Dim fromQTY As Decimal = _qtymodified
                If pExceptionQty > 0 Then
                    _qtymodified = _qtymodified - pExceptionQty
                Else
                    _qtymodified = _qtymodified - QTYLEFTTOFULLFILL
                End If
                _edituser = pUser
                _editdate = DateTime.Now
                sql = String.Format("update outboundordetail set qtymodified = {0},edituser = {1},editdate = {2} where {3}", _
                    Made4Net.Shared.Util.FormatField(_qtymodified), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                DataInterface.RunSQL(sql)


                'header.CancelExceptionsSetStatus(pUser)

                cancelLineExcetpionsSendMsg(pUser, fromQTY)
            End If
        End Sub

        Private Sub cancelLineExcetpionsSendMsg(ByVal pUser As String, ByVal pFromQTY As Decimal)
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderLineCancelExcept)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDLNCNCLEXC)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", _orderline)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", pFromQTY)
            aq.Add("FROMSTATUS", Me.ORDERSTATUS)
            aq.Add("NOTES", "")
            aq.Add("SKU", _sku)
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", _qtymodified)
            aq.Add("TOSTATUS", Me.ORDERSTATUS)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDLNCNCLEXC)
        End Sub

#End Region

#Region "Stage/Pack/Load/Ship"

        Public Sub Ship(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String, Optional ByVal oOH As OutboundOrderHeader = Nothing)
            _qtyshipped += pUnits '_qtypicked + _qtystaged + _qtyloaded
            UpdateLastActivityQuantities(pPreviousActivty, pUnits)
            _edituser = puser
            _editdate = DateTime.Now
            'sql = String.Format("Update OUTBOUNDORDETAIL SET QTYPICKED = {0},QTYSTAGED = {1}, QTYPACKED = {2},QTYLOADED = {3},QTYSHIPPED = {4},QTYVERIFIED={5}, EDITDATE = {6},EDITUSER = {7} WHERE {8}", Made4Net.Shared.Util.FormatField(_qtypicked), _
            '    Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_qtyloaded), Made4Net.Shared.Util.FormatField(_qtyshipped), Made4Net.Shared.Util.FormatField(_qtyverified), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            'DataInterface.RunSQL(sql)
            updateQuantitiesInDB()
            'And Update the Header if needed
            Dim oOrdHeader As OutboundOrderHeader
            If oOH Is Nothing Then
                oOrdHeader = GetOutboundOrderHeader()
            Else
                oOrdHeader = oOH
            End If
            oOrdHeader.Lines.Line(_orderline).QTYSHIPPED = _qtyshipped
            For Each oOrdDet As OutboundOrderDetail In oOrdHeader.Lines
                If Not oOrdDet.FULLYSHIPPED Then
                    Return
                End If
            Next
            oOrdHeader.UpdateShipStatus(puser)
        End Sub

        Public Sub Pack(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String)
            _qtypacked += pUnits '_qtypicked + _qtystaged
            UpdateLastActivityQuantities(pPreviousActivty, pUnits)
            'If _qtypicked - pUnits > 0 Then _qtypicked -= pUnits Else _qtypicked = 0
            'If _qtystaged - pUnits > 0 Then _qtystaged -= pUnits Else _qtystaged = 0
            _edituser = puser
            _editdate = DateTime.Now
            'sql = String.Format("Update OUTBOUNDORDETAIL SET QTYPICKED = {0},QTYSTAGED = {1},QTYPACKED = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_qtypicked), _
            '    Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            'DataInterface.RunSQL(sql)
            updateQuantitiesInDB()
            'And Update the Header if needed
            Dim oOrdHeader As OutboundOrderHeader = GetOutboundOrderHeader()
            For Each oOrdDet As OutboundOrderDetail In oOrdHeader.Lines
                If Not oOrdDet.FULLYPACKED Then
                    Return
                End If
            Next
            oOrdHeader.Pack(puser)
        End Sub

        Public Sub UnPack(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String)
            _qtypacked -= pUnits
            If pPreviousActivty = WMS.Lib.LoadActivityTypes.STAGING Then
                _qtystaged += pUnits
            Else
                _qtypicked += pUnits
            End If
            _edituser = puser
            _editdate = DateTime.Now
            'sql = String.Format("Update OUTBOUNDORDETAIL SET QTYPICKED = {0},QTYSTAGED = {1},QTYPACKED = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_qtypicked), _
            '    Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            'DataInterface.RunSQL(sql)
            updateQuantitiesInDB()
        End Sub

        Public Sub Stage(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String, Optional ByVal oOH As OutboundOrderHeader = Nothing)
            'If _qtystaged + pUnits <= _qtymodified Then _qtystaged += pUnits Else _qtystaged = _qtymodified
            'Was changed because of overpick.
            _qtystaged += pUnits
            UpdateLastActivityQuantities(pPreviousActivty, pUnits)
            _edituser = puser
            _editdate = DateTime.Now
            updateQuantitiesInDB()
            'And Update the Header if needed
            Dim oOrdHeader As OutboundOrderHeader
            If oOH Is Nothing Then
                oOrdHeader = GetOutboundOrderHeader()
            Else
                oOrdHeader = oOH
            End If
            For Each oOrdDet As OutboundOrderDetail In oOrdHeader.Lines
                If Not oOrdDet.FULLYSTAGED Then
                    Return
                End If
            Next
            oOrdHeader.Stage(puser)
        End Sub

        Public Sub Load(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String, Optional ByVal oOH As OutboundOrderHeader = Nothing)
            'If _qtyloaded + pUnits <= _qtymodified Then _qtyloaded += pUnits Else _qtyloaded = _qtymodified
            _qtyloaded += pUnits
            UpdateLastActivityQuantities(pPreviousActivty, pUnits)
            _edituser = puser
            _editdate = DateTime.Now
            updateQuantitiesInDB()
            'And Update the Header if needed
            Dim oOrdHeader As OutboundOrderHeader
            If oOH Is Nothing Then
                oOrdHeader = GetOutboundOrderHeader()
            Else
                oOrdHeader = oOH
            End If
            Dim bFullyLoaded As Boolean = True
            For Each oOrdDet As OutboundOrderDetail In oOrdHeader.Lines
                If Not oOrdDet.FULLYLOADED Then
                    bFullyLoaded = False
                    Exit For
                End If
            Next
            If bFullyLoaded Then
                oOrdHeader.UpdateLoadStatus(puser)
            Else
                oOrdHeader.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.LOADING, puser)
            End If
        End Sub

        Public Sub Verify(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal pUser As String)
            'If _qtyverified + pUnits <= _qtymodified Then _qtyverified += pUnits Else _qtyverified = _qtymodified
            _qtyverified += pUnits
            UpdateLastActivityQuantities(pPreviousActivty, pUnits)
            _editdate = DateTime.Now
            _edituser = pUser
            updateQuantitiesInDB()
            Dim oOrdHeader As OutboundOrderHeader = GetOutboundOrderHeader()
            For Each oOrdDet As OutboundOrderDetail In oOrdHeader.Lines
                If Not oOrdDet.FULLYVERIFIED Then
                    Return
                End If
            Next
            oOrdHeader.Verify(pUser)
        End Sub

        Private Sub updateQuantitiesInDB()
            Dim sql As String = String.Format("Update OUTBOUNDORDETAIL SET QTYORIGINAL={0},QTYMODIFIED={1},QTYALLOCATED={2}," & _
            "QTYSOFTALLOCATED={3},QTYPICKED={4},QTYSTAGED={5},QTYPACKED={6},QTYLOADED={7},QTYSHIPPED={8},QTYVERIFIED={9},EDITUSER={11},EDITDATE={12} Where {10}", _
            Made4Net.Shared.Util.FormatField(_qtyoriginal), Made4Net.Shared.Util.FormatField(_qtymodified), _
            Made4Net.Shared.Util.FormatField(_qtyallocated), Made4Net.Shared.Util.FormatField(_qtysoftallocated), _
            Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_qtystaged), _
            Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_qtyloaded), _
            Made4Net.Shared.Util.FormatField(_qtyshipped), Made4Net.Shared.Util.FormatField(_qtyverified), WhereClause, _
            Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate))

            DataInterface.RunSQL(sql)
        End Sub

        Private Sub UpdateLastActivityQuantities(ByVal pPrevActivtyType As String, ByVal pUnits As Decimal)
            Select Case pPrevActivtyType
                Case WMS.Lib.LoadActivityTypes.PICKING
                    If _qtypicked - pUnits > 0 Then _qtypicked -= pUnits Else _qtypicked = 0
                Case WMS.Lib.LoadActivityTypes.STAGING
                    If _qtystaged - pUnits > 0 Then _qtystaged -= pUnits Else _qtystaged = 0
                Case WMS.Lib.LoadActivityTypes.PACKING
                    If _qtypacked - pUnits > 0 Then _qtypacked -= pUnits Else _qtypacked = 0
                Case WMS.Lib.LoadActivityTypes.LOADING
                    If _qtyloaded - pUnits > 0 Then _qtyloaded -= pUnits Else _qtyloaded = 0
                Case WMS.Lib.LoadActivityTypes.VERIFYING
                    If _qtyverified - pUnits > 0 Then _qtyverified -= pUnits Else _qtyverified = 0
            End Select
        End Sub

        Public Sub UnLoad(ByVal pQty As Decimal, ByVal UpdateStagedQty As Boolean, ByVal pUser As String)
            Dim sql As String
            Dim header As New OutboundOrderHeader(_consignee, _orderid)
            If Me.QTYLOADED < pQty Then
                Throw New M4NException(New Exception, "Can not unload.Loaded quantity is less the quantity requested to unload.", "Can not unload.Loaded quantity is less the quantity requested to unload.")
            End If

            Dim editDate As DateTime = DateTime.Now
            If UpdateStagedQty Then
                Me.QTYSTAGED += pQty
                Me.QTYLOADED -= pQty
                sql = String.Format("Update outboundordetail set qtyStaged={0},qtyloaded={1},edituser={2},editdate={3} where {4}", _
                Made4Net.Shared.FormatField(_qtystaged), Made4Net.Shared.FormatField(_qtyloaded), _
                Made4Net.Shared.FormatField(pUser), Made4Net.Shared.FormatField(editDate), WhereClause)
            Else
                Me.QTYPICKED += pQty
                Me.QTYLOADED -= pQty
                sql = String.Format("Update outboundordetail set qtyPicked={0},qtyloaded={1},edituser={2},editdate={3} where {4}", _
                Made4Net.Shared.FormatField(_qtypicked), Made4Net.Shared.FormatField(_qtyloaded), _
                Made4Net.Shared.FormatField(pUser), Made4Net.Shared.FormatField(editDate), WhereClause)
            End If

            DataInterface.RunSQL(sql)

            header.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.LOADING, pUser)
        End Sub



#End Region

#Region "Work Order"

        Public Function GenerateWorkOrder(ByVal pOrderType As String, ByVal pRefOrd As String, ByVal pRequestedDate As DateTime, ByVal pNotes As String, ByVal pUser As String) As WorkOrderHeader
            Dim oWorkOrd As New WorkOrderHeader
            oWorkOrd.Create(_consignee, "", pOrderType, WorkOrderDocumentType.Assembly, pRefOrd, _referenceordline, _
                 _orderid, _orderline, _sku, _inventorystatus, _qtymodified, pRequestedDate, pNotes, "", "", Nothing, pUser)

            _exploadedflag = 1
            _editdate = DateTime.Now
            _edituser = pUser
            Dim sql As String = String.Format("Update OutboundOrdetail set EDITDATE={0},EDITUSER={1}, EXPLOADEDFLAG={2} where {3}" _
                , Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_exploadedflag), WhereClause)
            DataInterface.RunSQL(sql)

            Return oWorkOrd
        End Function

#End Region

#Region "Substitute Sku"

        Public Sub UpdateOriginalSku(ByVal pSubSku As String, ByVal pUser As String)
            If Not (WMS.Logic.SKU.Exists(_consignee, pSubSku)) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Substitute SKU does not exist", "Substitute SKU does not exist")
            End If
            _originalsku = _sku
            _sku = pSubSku
            _editdate = DateTime.Now
            _edituser = pUser
            Dim sql As String = String.Format("update outboundordetail set originalsku = '{0}',sku = '{1}', editdate = {2}, edituser = '{3}' where {4}", _
                    _originalsku, _sku, Made4Net.Shared.Util.FormatField(_editdate), _edituser, WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

#End Region

#End Region

    End Class

#End Region

#Region "Outbound Order Details Collection"

    <CLSCompliant(False)> Public Class OutboundOrderDetailsCollection
        Inherits ArrayList

#Region "Variables"

        Protected _consignee As String
        Protected _orderid As String

#End Region

#Region "Properties"

        Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As OutboundOrderDetail
            Get
                Return CType(MyBase.Item(index), OutboundOrderDetail)
            End Get
        End Property

        Public ReadOnly Property Line(ByVal LineNumber As Int32) As OutboundOrderDetail
            Get
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If Item(i).ORDERLINE = LineNumber Then
                        Return (CType(MyBase.Item(i), OutboundOrderDetail))
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
            Dim dtLines As New DataTable
            Dim drLines As DataRow
            SQL = "SELECT * FROM OUTBOUNDORDETAIL where consignee = '" & _consignee & "' and ORDERID = '" & _orderid & "'"
            DataInterface.FillDataset(SQL, dtLines)
            Dim dtAttributes As New DataTable
            Dim drAttribute As DataRow
            SQL = "SELECT * FROM attribute where pkeytype = 'OUTBOUND' and pkey1 = '" & _consignee & "' and pkey2 = '" & _orderid & "'"
            DataInterface.FillDataset(SQL, dtAttributes)
            For Each drLines In dtLines.Rows
                Try
                    drAttribute = dtAttributes.Select(String.Format("pkey3 = '{0}'", drLines("orderline")))(0)
                Catch ex As Exception
                    drAttribute = Nothing
                End Try
                Me.add(New OutboundOrderDetail(drLines, drAttribute))
            Next
        End Sub

#End Region

#Region "Methods"

        Public Function GetLine(ByVal orderline As String) As OutboundOrderDetail
            For index As Integer = 0 To Me.Count
                If Me(index).ORDERLINE = orderline Then
                    Return Me(index)
                End If
            Next
            Return Nothing
        End Function

        Public Shadows Function add(ByVal pObject As OutboundOrderDetail) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As OutboundOrderDetail)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As OutboundOrderDetail)
            MyBase.Remove(pObject)
        End Sub

        Public Sub AddLine(ByVal pLine As Int32, ByVal pRefLine As Int32, ByVal pSku As String, ByVal pStatus As String, ByVal pUnits As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String)
            Dim oLine As OutboundOrderDetail = Line(pLine)
            If IsNothing(oLine) Then
                oLine = New OutboundOrderDetail
                oLine.Create(_consignee, _orderid, pLine, pRefLine, pSku, pStatus, pUnits, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea)
                Me.add(oLine)
            Else
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot add line to OutboundOrder", "Cannot add line to OutboundOrder")
                m4nEx.Params.Add("pLine", pLine)
                m4nEx.Params.Add("consignee", _consignee)
                m4nEx.Params.Add("orderid", _orderid)
                Throw m4nEx
            End If
        End Sub

        Public Sub AddLine(ByVal pRefLine As Int32, ByVal pSku As String, ByVal pStatus As String, ByVal pUnits As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing)
            Dim pLineNumber As Int32 = System.Convert.ToInt32(DataInterface.ExecuteScalar(String.Format("Select isnull(max(orderline),0) + 1 from outboundordetail where consignee = '{0}' and orderid = '{1}'", _consignee, _orderid)))
            AddLine(pLineNumber, pRefLine, pSku, pStatus, pUnits, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea)
        End Sub

        Public Sub CreateNewLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing)
            Me.add(New OutboundOrderDetail().InsertDetail(pConsignee, pOrderId, pRefOrdLine, pSku, pInvStat, pQty, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea, pInputQTY, pInpupUOM))
        End Sub

        Public Sub CreateNewLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrdLine As Int32, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing)
            Me.add(New OutboundOrderDetail().InsertDetail(pConsignee, pOrderId, pOrdLine, pRefOrdLine, pSku, pInvStat, pQty, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea, pInputQTY, pInpupUOM))
        End Sub

        Public Sub EditLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLineNumber As String, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputSku As String = Nothing, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInputUOM As String = Nothing)
            Me.add(New OutboundOrderDetail().EditDetail(pConsignee, pOrderId, pLineNumber, pRefOrdLine, pSku, pInvStat, pQty, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea, pInputSku, pInputQTY, pInputUOM))
        End Sub

#End Region

    End Class

#End Region

#End Region

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _orderid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _ordertype As String = String.Empty
    Protected _referenceord As String = String.Empty
    Protected _targetcompany As String = String.Empty
    Protected _companytype As String = String.Empty
    Protected _status As String = String.Empty
    Protected _createdate As DateTime
    Protected _notes As String = String.Empty
    Protected _staginglane As String = String.Empty
    Protected _stagingwarehousearea As String = String.Empty
    Protected _requesteddate As DateTime
    Protected _scheduleddate As DateTime
    Protected _shippeddate As DateTime
    'Protected _shipment As String = String.Empty
    'Protected _stopnumber As Int32
    Protected _orderpriority As Int32
    'Protected _route As String
    Protected _routingset As String = String.Empty
    Protected _loadingseq As String
    'Protected _wave As String = String.Empty
    Protected _statusdate As DateTime
    Protected _hostorderid As String = String.Empty '--- 17/10/2005
    '---- Stores CONTACTID of Customer
    Protected _shipto As String = String.Empty
    Protected _deliverystatus As String = String.Empty
    Protected _pod As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Collections"

    Protected _Lines As OutboundOrderDetailsCollection
    Protected _Loads As OutboundOrderLoadsCollection
    Protected _handlingunits As HandlingUnitTransactionCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "'"
        End Get
    End Property

    Public ReadOnly Property CanShip() As Boolean
        Get
            If _status = WMS.Lib.Statuses.OutboundOrderHeader.PICKED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED _
                Or _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.PACKED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED Or _
                _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPING Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CanStage() As Boolean
        Get
            If _status = WMS.Lib.Statuses.OutboundOrderHeader.PICKED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CanLoad() As Boolean
        Get
            If _status = WMS.Lib.Statuses.OutboundOrderHeader.PICKED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED _
                    Or _status = WMS.Lib.Statuses.OutboundOrderHeader.PACKED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADING Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property FULLYPLANNED() As Boolean
        Get
            For Each oDet As OutboundOrderDetail In Me.Lines
                If Not oDet.FULLYPLANNED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property

    Public ReadOnly Property FULLYPICKED() As Boolean
        Get
            For Each oDet As OutboundOrderDetail In Me.Lines
                If Not oDet.FULLYPICKED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property

    Public ReadOnly Property FULLYLOADED() As Boolean
        Get
            For Each oDet As OutboundOrderDetail In Me.Lines
                If Not oDet.FULLYLOADED Then
                    Return False
                End If
            Next
            Return True
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

    Public ReadOnly Property Lines() As OutboundOrderDetailsCollection
        Get
            Return _Lines
        End Get
    End Property

    Public ReadOnly Property Loads() As OutboundOrderLoadsCollection
        Get
            Return _Loads
        End Get
    End Property

    Public ReadOnly Property HandlingUnits() As HandlingUnitTransactionCollection
        Get
            Return _handlingunits
        End Get
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

    Public Property REFERENCEORD() As String
        Get
            Return _referenceord
        End Get
        Set(ByVal Value As String)
            _referenceord = Value
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

    Public Property COMPANYTYPE() As String
        Get
            Return _companytype
        End Get
        Set(ByVal Value As String)
            _companytype = Value
        End Set
    End Property

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)

            'Added for RWMS-2014(RWMS-1710 Reverted Code Changes) Start
            _status = Value
            'Added for RWMS-2014(RWMS-1710 Reverted Code Changes) End

            'Commented for RWMS-2014(RWMS-1710 Reverted Code Changes) Start
            ''Added for RWMS-2014 Start
            'If Exists(_consignee, _orderid) Then
            '    Dim StatusOutboundOrder As String = DataInterface.ExecuteScalar(String.Format("SELECT STATUS FROM vOutboundOrderStatus WHERE orderid='{0}'", _orderid))
            '    _status = StatusOutboundOrder
            'Else
            '    _status = Value
            'End If
            ''Added for RWMS-2014 End
            'Commented for RWMS-2014(RWMS-1710 Reverted Code Changes) End

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

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal Value As String)
            _notes = Value
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

    Public Property REQUESTEDDATE() As DateTime
        Get
            Return _requesteddate
        End Get
        Set(ByVal Value As DateTime)
            _requesteddate = Value
        End Set
    End Property

    Public Property SCHEDULEDDATE() As DateTime
        Get
            Return _scheduleddate
        End Get
        Set(ByVal Value As DateTime)
            _scheduleddate = Value
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

    'Public Property SHIPMENT() As String
    '    Get
    '        Return _shipment
    '    End Get
    '    Set(ByVal Value As String)
    '        _shipment = Value
    '    End Set
    'End Property

    'Public Property STOPNUMBER() As Int32
    '    Get
    '        Return _stopnumber
    '    End Get
    '    Set(ByVal Value As Int32)
    '        _stopnumber = Value
    '    End Set
    'End Property

    Public Property ORDERPRIORITY() As Int32
        Get
            Return _orderpriority
        End Get
        Set(ByVal Value As Int32)
            _orderpriority = Value
        End Set
    End Property

    'Public Property ROUTE() As String
    '    Get
    '        Return _route
    '    End Get
    '    Set(ByVal Value As String)
    '        _route = Value
    '    End Set
    'End Property

    Public Property LOADINGSEQ() As String
        Get
            Return _loadingseq
        End Get
        Set(ByVal Value As String)
            _loadingseq = Value
        End Set
    End Property

    'Public Property WAVE() As String
    '    Get
    '        Return _wave
    '    End Get
    '    Set(ByVal Value As String)
    '        _wave = Value

    '    End Set
    'End Property

    Public Property ROUTINGSET() As String
        Get
            Return _routingset
        End Get
        Set(ByVal Value As String)
            _routingset = Value
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
    '--- 17/10/2005
    Public Property HOSTORDERID() As String
        Get
            Return _hostorderid
        End Get
        Set(ByVal Value As String)
            _hostorderid = Value
        End Set
    End Property
    '---- Stores CONTACT Object of the Customer
    Public ReadOnly Property SHIPTO() As Contact
        Get
            Try
                Return New Contact(_shipto, ContactReference.CompanyContact)
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public Property SHIPTOID() As String
        Get
            Return _shipto
        End Get
        Set(ByVal value As String)
            _shipto = value
        End Set
    End Property

    Public Property DELIVERYSTATUS() As String
        Get
            Return _deliverystatus
        End Get
        Set(ByVal value As String)
            _deliverystatus = value
        End Set
    End Property

    Public Property POD() As String
        Get
            Return _pod
        End Get
        Set(ByVal value As String)
            _pod = value
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

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim outorheader As OutboundOrderHeader
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName.ToLower = "printsh" Then
            For Each dr In ds.Tables(0).Rows
                outorheader = New OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                outorheader.PrintShippingManifest(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
            Next
        ElseIf CommandName.ToLower = "neworder" Then
            Dim hID As String = ""
            Try
                hID = Convert.ReplaceDBNull(dr("hostorderid"))
            Catch ex As Exception
            End Try
            Dim delStatus As String = ""
            If dr.Table.Columns.Contains("DELIVERYSTATUS") Then
                delStatus = Convert.ReplaceDBNull(dr("DELIVERYSTATUS"))
            End If

            'CreateNew(dr("consignee"), dr("orderid"), dr("ordertype"), dr("targetcompany"), dr("companytype"), _
            '    Convert.ReplaceDBNull(dr("notes")), Convert.ReplaceDBNull(dr("requesteddate")), Convert.ReplaceDBNull(dr("scheduleddate")), Convert.ReplaceDBNull(dr("stopnumber")), Convert.ReplaceDBNull(dr("orderpriority")), Convert.ReplaceDBNull(dr("route")), hID, _
            '    Convert.ReplaceDBNull(dr("loadingseq")), Convert.ReplaceDBNull(dr("referenceord")), Convert.ReplaceDBNull(dr("staginglane")), Convert.ReplaceDBNull(dr("shipto")), delStatus, Common.GetCurrentUser)
            CreateNew(dr("consignee"), dr("orderid"), dr("ordertype"), dr("targetcompany"), dr("companytype"), _
                            Convert.ReplaceDBNull(dr("notes")), Convert.ReplaceDBNull(dr("requesteddate")), Convert.ReplaceDBNull(dr("scheduleddate")), Convert.ReplaceDBNull(dr("orderpriority")), HOSTORDERID, _
                            Convert.ReplaceDBNull(dr("loadingseq")), Convert.ReplaceDBNull(dr("referenceord")), Convert.ReplaceDBNull(dr("staginglane")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("shipto")), delStatus, Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "editorder" Then
            _consignee = dr("consignee")
            _orderid = dr("orderid")
            Load()
            Dim hID As String = ""
            Try
                hID = Convert.ReplaceDBNull(dr("hostorderid"))
            Catch ex As Exception
            End Try

            Dim delStatus As String = ""
            If dr.Table.Columns.Contains("DELIVERYSTATUS") Then
                delStatus = Convert.ReplaceDBNull(dr("DELIVERYSTATUS"))
            End If
            'EditOrder(dr("consignee"), dr("orderid"), dr("ordertype"), dr("targetcompany"), dr("companytype"), _
            'Convert.ReplaceDBNull(dr("notes")), Convert.ReplaceDBNull(dr("requesteddate")), Convert.ReplaceDBNull(dr("scheduleddate")), Convert.ReplaceDBNull(dr("stopnumber")), _
            'Convert.ReplaceDBNull(dr("route")), hID, Convert.ReplaceDBNull(dr("loadingseq")), _
            'Convert.ReplaceDBNull(dr("referenceord")), Convert.ReplaceDBNull(dr("staginglane")), Convert.ReplaceDBNull(dr("shipto")), delStatus, Common.GetCurrentUser)
            EditOrder(dr("consignee"), dr("orderid"), dr("ordertype"), dr("targetcompany"), dr("companytype"), _
            Convert.ReplaceDBNull(dr("notes")), Convert.ReplaceDBNull(dr("requesteddate")), Convert.ReplaceDBNull(dr("scheduleddate")), _
            HOSTORDERID, Convert.ReplaceDBNull(dr("loadingseq")), _
            Convert.ReplaceDBNull(dr("referenceord")), Convert.ReplaceDBNull(dr("staginglane")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("shipto")), delStatus, Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "neworderdetail" Then
            _consignee = dr("consignee")
            _orderid = dr("orderid")
            Load()
            Dim refordline As Int32
            Dim exploadedflag As Int32
            Try
                refordline = dr("referenceordline")
            Catch ex As Exception
                refordline = 0
            End Try
            Try
                exploadedflag = dr("exploadedflag")
            Catch ex As Exception
                exploadedflag = 0
            End Try
            Dim units As Decimal
            If IsDBNull(dr("skuuom")) Or dr("skuuom") = "" Then
                units = dr("qtyoriginal")
            Else
                units = CalcUnits(Convert.ReplaceDBNull(dr("sku")), dr("qtyoriginal"), dr("skuuom")) * dr("qtyoriginal")
            End If
            Dim oAttCol As AttributesCollection '= SkuClass.ExtractLoadAttributes(dr)
            If IsDBNull(dr("SKU")) Then
                oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(dr, True)
            Else
                oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(dr)
            End If
            AddOrderLine(_consignee, _orderid, refordline, _
               Convert.ReplaceDBNull(dr("sku")), dr("inventorystatus"), units, Convert.ReplaceDBNull(dr("inputsku")), Convert.ReplaceDBNull(dr("notes")), Convert.ReplaceDBNull(dr("route")), Convert.ReplaceDBNull(dr("stopnumber")), Common.GetCurrentUser(), exploadedflag, oAttCol, dr("Warehousearea"), dr("qtyoriginal"), dr("skuuom"))
        ElseIf CommandName.ToLower = "editorderdetail" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                Dim refordline As Int32
                Dim exploadedflag As Int32
                Try
                    refordline = dr("referenceordline")
                Catch ex As Exception
                    refordline = 0
                End Try
                Try
                    exploadedflag = dr("exploadedflag")
                Catch ex As Exception
                    exploadedflag = 0
                End Try
                Dim oAttCol As AttributesCollection = WMS.Logic.SkuClass.ExtractLoadAttributes(dr)
                EditOrderLine(_consignee, _orderid, dr("orderline"), refordline, _
                   dr("sku"), dr("inventorystatus"), dr("QTYMODIFIED"), Convert.ReplaceDBNull(dr("notes")), Convert.ReplaceDBNull(dr("route")), Convert.ReplaceDBNull(dr("stopnumber")), Common.GetCurrentUser(), exploadedflag, oAttCol, Convert.ReplaceDBNull(dr("warehousearea")))
            Next
        ElseIf CommandName.ToLower = "completeorder" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                Complete(Common.GetCurrentUser())
            Next
        ElseIf CommandName.ToLower = "setstaginglane" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                Dim reqDate As DateTime
                Dim stagingLane As String = String.Empty
                Dim stagingwarehousearea As String = String.Empty
                Try
                    If Not IsDBNull(dr("REQUESTEDDATE")) Then reqDate = dr("REQUESTEDDATE")
                    If Not IsDBNull(dr("STAGINGLANE")) Then stagingLane = dr("STAGINGLANE")
                    If Not IsDBNull(dr("STAGINGWAREHOUSEAREA")) Then stagingwarehousearea = dr("STAGINGWAREHOUSEAREA")
                Catch ex As Exception

                End Try
                SetStagingLane(stagingLane, stagingwarehousearea, reqDate, Common.GetCurrentUser())
            Next
        ElseIf CommandName.ToLower = "printlabels" Then
            _consignee = dr("consignee")
            _orderid = dr("orderid")
            Load()
            PrintLabel(Nothing)
            'ElseIf CommandName.ToLower = "verifycont" Then
            '    Dim contId As String = dr("loadid")
            '    Verify(contId)
        ElseIf CommandName.ToLower = "ship" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                Ship(Common.GetCurrentUser)
            Next
        ElseIf CommandName.ToLower = "deleteheader" Then
            _consignee = dr("consignee")
            _orderid = dr("orderid")
            Load()
            Delete(Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "deleteline" Then
            _consignee = dr("consignee")
            _orderid = dr("orderid")
            Load()
            DeleteLine(dr("orderline"), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "canceloutbound" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                CancelOutbound(Common.GetCurrentUser)
            Next
        ElseIf CommandName.ToLower = "planorder" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                CalibrateSoftAlloc()
                Plan(False, Common.GetCurrentUser)
            Next
            Message = "Planning Order"
        ElseIf CommandName.ToLower = "softpaln" Then
            For Each dr In ds.Tables(0).Rows
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                Load()
                CalibrateSoftAlloc()
                Plan(False, Common.GetCurrentUser, True)
            Next
        End If
    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pORDERID As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _orderid = pORDERID
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessories"

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pOrderId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from OutboundOrHeader where Consignee = '{0}' and ORDERID = '{1}'", pConsignee, pOrderId)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetOutboundOrderHeader(ByVal pCONSIGNEE As String, ByVal pORDERID As String) As OutboundOrderHeader
        Return New OutboundOrderHeader(pCONSIGNEE, pORDERID)
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM OUTBOUNDORHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New M4NException(New Exception, "Outbound Order does not exist", "Outbound Order does not exist")
        End If
        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERTYPE") Then _ordertype = dr.Item("ORDERTYPE")
        If Not dr.IsNull("REFERENCEORD") Then _referenceord = dr.Item("REFERENCEORD")
        If Not dr.IsNull("TARGETCOMPANY") Then _targetcompany = dr.Item("TARGETCOMPANY")
        If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")

        'UnCommented for RWMS-2014(RWMS-1710 Reverted Code Changes) Start
        'RWMS-2014
        'Commented for RWMS-1710 Start
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        'Commented for RWMS-1710 End
        'UnCommented for RWMS-2014(RWMS-1710 Reverted Code Changes) End

        'Commented for RWMS-2014(RWMS-1710 Reverted Code Changes) Start
        ''Added for RWMS-2014 Start
        'If Not dr.IsNull("STATUS") Then STATUS = dr.Item("STATUS")
        ''Added for RWMS-2014 End
        'Commented for RWMS-2014(RWMS-1710 Reverted Code Changes) End

        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("STAGINGLANE") Then _staginglane = dr.Item("STAGINGLANE")
        'If Not dr.IsNull("STAGINGWAREHOUSEAREA") Then _staginglane = dr.Item("STAGINGWAREHOUSEAREA")
        If Not dr.IsNull("STAGINGWAREHOUSEAREA") Then _stagingwarehousearea = dr.Item("STAGINGWAREHOUSEAREA")
        If Not dr.IsNull("REQUESTEDDATE") Then _requesteddate = dr.Item("REQUESTEDDATE")
        If Not dr.IsNull("SCHEDULEDDATE") Then _scheduleddate = dr.Item("SCHEDULEDDATE")
        If Not dr.IsNull("SHIPPEDDATE") Then _shippeddate = dr.Item("SHIPPEDDATE")
        'If Not dr.IsNull("SHIPMENT") Then _shipment = dr.Item("SHIPMENT")
        'If Not dr.IsNull("STOPNUMBER") Then _stopnumber = dr.Item("STOPNUMBER")
        'If Not dr.IsNull("ROUTE") Then _route = dr.Item("ROUTE")
        If Not dr.IsNull("ROUTINGSET") Then _routingset = dr.Item("ROUTINGSET")
        If Not dr.IsNull("HOSTORDERID") Then _hostorderid = dr.Item("HOSTORDERID")
        If Not dr.IsNull("LOADINGSEQ") Then _loadingseq = dr.Item("LOADINGSEQ")
        If Not dr.IsNull("ORDERPRIORITY") Then _orderpriority = dr.Item("ORDERPRIORITY")
        'If Not dr.IsNull("WAVE") Then _wave = dr.Item("WAVE")
        If Not dr.IsNull("STATUSDATE") Then _statusdate = dr.Item("STATUSDATE")
        If Not dr.IsNull("SHIPTO") Then _shipto = dr.Item("SHIPTO")
        If Not dr.IsNull("DELIVERYSTATUS") Then _deliverystatus = dr.Item("DELIVERYSTATUS")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _Lines = New OutboundOrderDetailsCollection(_consignee, _orderid)
        _Loads = New OutboundOrderLoadsCollection(_consignee, _orderid)
        _handlingunits = New HandlingUnitTransactionCollection(_consignee, _orderid, HandlingUnitTransactionTypes.OutboundOrder)

    End Sub

    Public Sub Delete(ByVal sUserId As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED _
            Or _status = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED Then
            For Each oLine As OutboundOrderDetail In _Lines
                oLine.Delete(sUserId)
            Next
            Dim sql As String = String.Format("delete from outboundorheader where " & WhereClause)
            DataInterface.RunSQL(sql)
            Dim wvsql As String = String.Format("delete from wavedetail where " & WhereClause)
            DataInterface.RunSQL(wvsql)
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OutboundOrderDeleted
            'em.Add("EVENT", EventType)
            'em.Add("ORDERID", _orderid)
            'em.Add("CONSIGNEE", _consignee)
            'em.Add("USERID", sUserId)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderDeleted)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHDEL)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", sUserId)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", sUserId)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", sUserId)
            aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHDEL)
        Else
            Throw New ApplicationException("Incorrect Order Status")
        End If
    End Sub

    Public Sub DeleteLine(ByVal iLineNumber As Int32, ByVal sUserId As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED _
            Or _status = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED Then
            Dim oLine As OutboundOrderDetail = _Lines.Line(iLineNumber)
            oLine.Delete(sUserId)
        Else
            Throw New ApplicationException("Incorrect Order Status")
        End If
    End Sub

    Private Function CalcUnits(ByVal pSku As String, ByVal pUnits As Decimal, ByVal pUom As String) As Decimal
        Dim oSku As New WMS.Logic.SKU(_consignee, pSku)
        If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pUom) Then
            Return oSku.ConvertToUnits(pUom)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
        End If
        'Return oSku.ConvertToUnits(pUom)
    End Function

    Private Function getOrderStatusByActivity() As String
        Dim NextOrderStatus As String = ""
        Dim QuantityCheckDataTable As DataTable = New DataTable
        Dim QuantityCheckSql As String = "SELECT SUM(QTYMODIFIED) - SUM(QTYPICKED) QTYPICKED, SUM(QTYMODIFIED) - SUM(QTYPACKED) QTYPACKED, SUM(QTYMODIFIED) - SUM(QTYSTAGED) QTYSTAGED,  SUM(QTYMODIFIED) - SUM(QTYLOADED) QTYLOADED,  SUM(QTYMODIFIED) - SUM(QTYVERIFIED) QTYVERIFIED,  SUM(QTYMODIFIED) - SUM(QTYSHIPPED) QTYSHIPPED FROM OUTBOUNDORDETAIL WHERE CONSIGNEE='" & _consignee & "' AND ORDERID='" & _orderid & "' GROUP BY  CONSIGNEE,ORDERID"
        DataInterface.FillDataset(QuantityCheckSql, QuantityCheckDataTable)

        If QuantityCheckDataTable.Rows.Count > 0 Then
            If QuantityCheckDataTable.Rows(0)("QTYSHIPPED") = 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED
            If QuantityCheckDataTable.Rows(0)("QTYLOADED") = 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.LOADED
            If QuantityCheckDataTable.Rows(0)("QTYPACKED") = 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.PACKED
            If QuantityCheckDataTable.Rows(0)("QTYVERIFIED") = 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED
            If QuantityCheckDataTable.Rows(0)("QTYSTAGED") = 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.STAGED
            If QuantityCheckDataTable.Rows(0)("QTYPICKED") = 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.PICKED
        End If

        Return NextOrderStatus
    End Function

#End Region

#Region "Lines"

    Public Sub AddLine(ByVal oLine As OutboundOrderDetail)
        AddOrderLine(oLine)
    End Sub

    Public Sub AddLine(ByVal pLine As Int32, ByVal prefLine As Int32, ByVal pSku As String, ByVal pStatus As String, ByVal pUnits As Decimal, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String)
        _Lines.AddLine(pLine, prefLine, pSku, pStatus, pUnits, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea)
    End Sub

    Public Sub AddLine(ByVal prefLine As Int32, ByVal pSku As String, ByVal pStatus As String, ByVal pUnits As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String)
        _Lines.AddLine(prefLine, pSku, pStatus, pUnits, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea)
    End Sub

#End Region

#Region "Shipments"

    <Obsolete("This method is not in use any more. Shipment was moved to the order detail level", True)> _
    Public Function AssignToShipment(ByVal pShipmentId As String, ByVal pUser As String)
        'If (_status <> WMS.Lib.Statuses.OutboundOrderHeader.CANCELED And STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED) Then
        '    _shipment = pShipmentId
        '    _editdate = DateTime.Now
        '    _edituser = pUser
        '    ' Check if current status is new than set status to SASSIGNED else dont change status
        '    If _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW Then
        '        If _wave Is Nothing Then
        '            _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED
        '        ElseIf _wave = "" Then
        '            _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED
        '        End If
        '        DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET SHIPMENT = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        '    Else
        '        DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET SHIPMENT = {0}, EDITDATE = {1}, EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        '    End If
        'Else
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "OutboundOrder status is incorrect for ship assignment", "OutboundOrder status is incorrect for canceling")
        '    Throw m4nEx
        'End If
        Return Nothing
    End Function

    <Obsolete("This method is not in use any more. Shipment was moved to the order detail level", True)> _
    Public Function DeAssignShipment(ByVal pUser As String)
        '_shipment = ""
        '_editdate = DateTime.Now
        '_edituser = pUser

        'If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED Then
        '    If _wave Is Nothing Then
        '        _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        '    ElseIf _wave = "" Then
        '        _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        '    End If
        '    DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET SHIPMENT = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        'Else
        '    DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET SHIPMENT = {0}, EDITDATE = {1}, EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        'End If
        Return Nothing
    End Function

#End Region

#Region "Wave"

    <Obsolete("This method is not in use any more. Wave was moved to the order detail level", True)> _
    Public Function AssignToWave(ByVal pWaveId As String, ByVal pUser As String)
        'If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW Then
        '    _wave = pWaveId
        '    _status = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED
        '    _editdate = DateTime.Now
        '    _edituser = pUser

        '    DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET WAVE = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_wave), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        'Else
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Assign Order to Wave", "Cannot Assign Order to Wave")
        '    Throw m4nEx
        'End If
        Return Nothing
    End Function

    <Obsolete("This method is not in use any more. Wave was moved to the order detail level", True)> _
    Public Function DeAssignWave(ByVal pUser As String)
        'If _status = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED Then
        '    _wave = ""
        '    If _shipment Is Nothing Then
        '        _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        '    ElseIf _shipment = "" Then
        '        _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        '    Else
        '        _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED
        '    End If

        '    _editdate = DateTime.Now
        '    _edituser = pUser

        '    DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET WAVE = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_wave), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        'Else
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot deAssign Order from Wave", "Cannot deAssign Order from Wave")
        '    Throw m4nEx
        'End If
        Return Nothing
    End Function

#End Region

#Region "Routing"

    <Obsolete("This method is not in use any more. Route was moved to the order detail level", True)> _
    Public Sub DeAssignFromRoute(ByVal pUser As String)
        'If _route = "" Or _route Is Nothing Then
        '    Exit Sub
        'End If
        'If WMS.Logic.Route.Exists(_route) Then
        '    Dim oRoute As New WMS.Logic.Route(_route)
        '    oRoute.CancelStopDetail(_consignee, _orderid, "", "", pUser)
        'End If
        'SetRoute("", 0, pUser)
    End Sub

    <Obsolete("This method is not in use any more. Route was moved to the order detail level", True)> _
    Public Sub SetRoute(ByVal pRouteId As String, ByVal pStopNumber As Int32, ByVal pUser As String)
        'Dim SQL As String
        'If _status <> WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
        '    _edituser = pUser
        '    _editdate = DateTime.Now
        '    _stopnumber = pStopNumber
        '    _route = pRouteId
        '    SQL = String.Format("Update outboundorheader set route = {0}, stopnumber = {1},editdate = {2}, edituser = {3} where {4}", Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        '    DataInterface.RunSQL(SQL)
        'End If
    End Sub

    Public Sub AssignToRoutingSet(ByVal pRoutingSetId As String, ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
            _routingset = pRoutingSetId
            _status = WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED
            _editdate = DateTime.Now
            _edituser = pUser

            DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET ROUTINGSET = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Assign Order to Route", "Cannot Assign Order to Route")
            Throw m4nEx
        End If
    End Sub

    Public Sub DeAssignRoutingSet(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED Then
            _routingset = ""
            Dim sShipment As String
            Dim sql As String = String.Format("select * from shipmentdetail where consignee = '{0}' and orderid = '{1}'", _consignee, _orderid)
            sShipment = DataInterface.ExecuteScalar(sql)
            If sShipment = String.Empty Then
                _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
            Else
                _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED
            End If

            _editdate = DateTime.Now
            _edituser = pUser

            DataInterface.RunSQL(String.Format("Update OUTBOUNDORHEADER SET ROUTINGSET = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot deAssign Order from Wave", "Cannot deAssign Order from Wave")
            Throw m4nEx
        End If
    End Sub

#End Region

#Region "Pack"

    Public Sub Pack(ByVal puser As String)
        Dim oldstat As String = _status

        setStatus(WMS.Lib.Statuses.OutboundOrderHeader.PACKED, puser)
        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OutboundOrderPacked
        'em.Add("EVENT", EventType)
        'em.Add("ORDERID", _orderid)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("FROMSTATUS", oldstat)
        'em.Add("TOSTATUS", _status)
        'em.Add("USERID", puser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderPacked)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHPAK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldstat)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHPAK)
    End Sub

#End Region

#Region "Plan & Pick & Release"

    Public Sub setStatus(ByVal pStatus As String, ByVal puser As String)
        Dim sql As String
        Dim oldstat As String = _status
        _status = pStatus
        _editdate = DateTime.Now
        _edituser = puser
        _statusdate = DateTime.Now
        sql = String.Format("update OUTBOUNDORHEADER set STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)


        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OrderStatusChanged
        'em.Add("EVENT", EventType)
        'em.Add("ORDERID", _orderid)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("FROMSTATUS", oldstat)
        'em.Add("TOSTATUS", _status)
        'em.Add("USERID", puser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderStatusChanged)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.ORDSTUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldstat)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", pStatus)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.ORDSTUPD)
    End Sub

    Public Sub Allocate(ByVal LineNumber As Int32, ByVal Units As Decimal, ByVal pUser As String)
        Dim ordline As OutboundOrderDetail = Me.Lines.Line(LineNumber)
        If ordline Is Nothing Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot allocate line in outbound order", "Cannot allocate line in outbound order")
            m4nEx.Params.Add("LineNumber", LineNumber)
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("orderid", _orderid)
            Throw m4nEx
        End If
        ordline.Allocate(Units, pUser)
    End Sub

    Public Sub SoftAllocate(ByVal LineNumber As Int32, ByVal Units As Decimal, ByVal pUser As String)
        Dim ordline As OutboundOrderDetail = Me.Lines.Line(LineNumber)
        If ordline Is Nothing Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot allocate line in outbound order", "Cannot allocate line in outbound order")
            m4nEx.Params.Add("LineNumber", LineNumber)
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("orderid", _orderid)
            Throw m4nEx
        End If
        ordline.SoftAllocate(Units, pUser)
    End Sub

    Public Sub Pick(ByVal pOrderLine As Int32, ByVal units As Decimal, ByVal pLoadId As String, ByVal pPickList As String, ByVal pPickLine As Int32, ByVal puser As String)
        Dim oldStatus As String = _status
        Me.Lines.Line(pOrderLine).Pick(units, puser)
        Me.Loads.AddLoad(pLoadId, pOrderLine, pPickList, pPickLine, puser)
        Dim ordetail As OutboundOrderDetail
        For Each ordetail In Me.Lines
            If Not ordetail.FULLYPICKED Then
                If _status = WMS.Lib.Statuses.OutboundOrderHeader.PLANNED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.RELEASED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED Then
                    setStatus(WMS.Lib.Statuses.OutboundOrderHeader.PICKING, puser)
                End If
                Exit Sub
            End If
        Next
        setStatus(WMS.Lib.Statuses.OutboundOrderHeader.PICKED, puser)
        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OutboundOrderPicked
        'em.Add("EVENT", EventType)
        'em.Add("ORDERID", _orderid)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("FROMSTATUS", oldStatus)
        'em.Add("TOSTATUS", _status)
        'em.Add("USERID", puser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderPicked)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHPCK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHPCK)
    End Sub

    Public Sub UnPick(ByVal pOrderLine As Int32, ByVal pLoadId As String, ByVal pUnits As Decimal, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Or _
                _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Or _
                _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Unpick Load. Incorrect Order Status", "Cannot Unpick Load. Incorrect Order Status")
        End If
        Me.Lines.Line(pOrderLine).UnPick(pLoadId, pUnits, pUser)
        Me.Loads.UnpickLoad(pLoadId, pUser)
        Select Case _status
            Case WMS.Lib.Statuses.OutboundOrderHeader.PICKED, WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED, WMS.Lib.Statuses.OutboundOrderHeader.STAGED, WMS.Lib.Statuses.OutboundOrderHeader.PACKED
                setStatus(WMS.Lib.Statuses.OutboundOrderHeader.PICKING, pUser)
        End Select
    End Sub

    Public Sub AdjustLine(ByVal pOrderLine As Int32, ByVal units As Decimal, ByVal puser As String)
        Me.Lines.Line(pOrderLine).Adjust(units, puser)
        Dim sPrevStat As String = _status
        Dim ordetail As OutboundOrderDetail
        Dim dTotalModifiedQty As Decimal = 0
        For Each ordetail In Me.Lines
            If ordetail.QTYMODIFIED - ordetail.QTYPICKED <> 0 Then
                Exit Sub
            End If
            dTotalModifiedQty += ordetail.QTYMODIFIED
        Next

        Dim NextOrderStatus As String = ""
        Dim QuantityCheckDataTable As DataTable = New DataTable
        Dim QuantityCheckSql As String = "SELECT SUM(QTYPICKED) QTYPICKED, SUM(QTYPACKED) QTYPACKED, SUM(QTYSTAGED) QTYSTAGED, SUM(QTYLOADED) QTYLOADED, SUM(QTYVERIFIED) QTYVERIFIED,SUM(QTYSHIPPED) QTYSHIPPED FROM OUTBOUNDORDETAIL WHERE CONSIGNEE='" & _consignee & "' AND ORDERID='" & _orderid & "' GROUP BY  CONSIGNEE,ORDERID"
        DataInterface.FillDataset(QuantityCheckSql, QuantityCheckDataTable)

        If QuantityCheckDataTable.Rows.Count > 0 Then
            If QuantityCheckDataTable.Rows(0)("QTYSHIPPED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED
            If QuantityCheckDataTable.Rows(0)("QTYLOADED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.LOADED
            If QuantityCheckDataTable.Rows(0)("QTYPACKED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.PACKED
            If QuantityCheckDataTable.Rows(0)("QTYVERIFIED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED
            If QuantityCheckDataTable.Rows(0)("QTYSTAGED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.STAGED
            If QuantityCheckDataTable.Rows(0)("QTYPICKED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.PICKED
        End If

        If dTotalModifiedQty > 0 Then
            _status = NextOrderStatus
        Else
            _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED
        End If
        _editdate = DateTime.Now
        _edituser = puser

        Dim sql As String = String.Format("update OUTBOUNDORHEADER set STATUS = {0},EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        If _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
            SendCancellationEvent(sPrevStat, puser)
        End If
    End Sub

    Public Sub unAllocateLine(ByVal pOrderLine As Int32, ByVal units As Decimal, ByVal puser As String)
        Me.Lines.Line(pOrderLine).unAllocate(units, puser)
    End Sub

    Public Sub Complete(ByVal puser As String)
        Dim orline As OutboundOrderDetail
        Dim pckqty As Decimal = 0
        Dim oldstatus As String = _status
        'RWMS-2247 RWMS-2014 - Checking the status OutboundOrderHeader.LOADED
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADING Or _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.PICKING Or _status = WMS.Lib.Statuses.OutboundOrderHeader.RELEASED Or _
                    _status = WMS.Lib.Statuses.OutboundOrderHeader.PLANNED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED _
                    Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED Then

            Dim pl As WMS.Logic.Picklist
            Dim sql As String
            sql = String.Format("select distinct picklist from pickdetail where (status = {0} or status = {1} or status = {2}) and orderid = {3}", Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.PARTPICKED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.PLANNED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.RELEASED), Made4Net.Shared.Util.FormatField(_orderid))
            Dim dt As New DataTable
            Dim dr As DataRow
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            For Each dr In dt.Rows
                pl = New WMS.Logic.Picklist(dr("PICKLIST"))
                pl.CompleteOrder(_consignee, _orderid, puser)
            Next
            If dt.Rows.Count > 0 Then
                _Lines = New OutboundOrderDetailsCollection(_consignee, _orderid)
            End If

            ' Lets go over all lines and set orders next status
            For Each orline In Me.Lines
                orline.Complete(puser)
                pckqty += orline.QTYPROCESSED
            Next

            Dim NextOrderStatus As String = ""
            Dim QuantityCheckDataTable As DataTable = New DataTable
            Dim QuantityCheckSql As String = "SELECT SUM(QTYPICKED) QTYPICKED, SUM(QTYPACKED) QTYPACKED, SUM(QTYSTAGED) QTYSTAGED, SUM(QTYLOADED) QTYLOADED, SUM(QTYVERIFIED) QTYVERIFIED,SUM(QTYSHIPPED) QTYSHIPPED FROM OUTBOUNDORDETAIL WHERE CONSIGNEE='" & _consignee & "' AND ORDERID='" & _orderid & "' GROUP BY  CONSIGNEE,ORDERID"
            DataInterface.FillDataset(QuantityCheckSql, QuantityCheckDataTable)

            If QuantityCheckDataTable.Rows.Count > 0 Then
                If QuantityCheckDataTable.Rows(0)("QTYSHIPPED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED
                If QuantityCheckDataTable.Rows(0)("QTYLOADED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.LOADED
                If QuantityCheckDataTable.Rows(0)("QTYPACKED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.PACKED
                If QuantityCheckDataTable.Rows(0)("QTYVERIFIED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED
                If QuantityCheckDataTable.Rows(0)("QTYSTAGED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.STAGED
                If QuantityCheckDataTable.Rows(0)("QTYPICKED") > 0 Then NextOrderStatus = WMS.Lib.Statuses.OutboundOrderHeader.PICKED
            End If

            If pckqty = 0 Then
                Cancel(WMS.Lib.USERS.SYSTEMUSER)
            Else
                setStatus(NextOrderStatus, WMS.Lib.USERS.SYSTEMUSER)
            End If

            Dim aq As EventManagerQ = New EventManagerQ
            Dim evtType As WMS.Logic.WMSEvents.EventType
            Dim actType As String = Nothing
            If _status = WMS.Lib.Statuses.OutboundOrderHeader.PICKED Then
                evtType = WMS.Logic.WMSEvents.EventType.OutboundOrderPicked
                actType = WMS.Lib.Actions.Audit.OUTBOUNDHPCK
            ElseIf _status = WMS.Lib.Statuses.OutboundOrderHeader.PACKED Then
                evtType = WMS.Logic.WMSEvents.EventType.OutboundOrderPacked
                actType = WMS.Lib.Actions.Audit.OUTBOUNDHPAK
            ElseIf _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED Then
                evtType = WMS.Logic.WMSEvents.EventType.OrderStaged
                actType = WMS.Lib.Actions.Audit.OUTBOUNDHSTG
            ElseIf _status = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED Then
                evtType = WMS.Logic.WMSEvents.EventType.OutBoundOrderVerified
                actType = WMS.Lib.Actions.Audit.OUTBOUNDHVRF
            ElseIf _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED Then
                evtType = WMS.Logic.WMSEvents.EventType.OutboundOrderLoaded
                actType = WMS.Lib.Actions.Audit.OUTBOUNDHLD
            End If

            'Added for RWMS-1519 and RWMS-1516 - Checking the actType to avoid the message going to dead letter queue
            If Not actType Is Nothing Then
                aq.Add("EVENT", evtType)
                aq.Add("ACTIVITYTYPE", actType)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", _consignee)
                aq.Add("DOCUMENT", _orderid)
                aq.Add("DOCUMENTLINE", 0)
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", oldstatus)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", puser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", puser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", puser)
                aq.Send(actType)
            End If
            'Ended for RWMS-1519 and RWMS-1516 - Checking the actType to avoid the message going to dead letter queue

            aq = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CompleteOrder)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.COMPLETEORDER)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", puser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", puser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", puser)
            aq.Send(WMS.Lib.Actions.Audit.COMPLETEORDER)

        End If
    End Sub

    Public Sub CalibrateSoftAlloc()
        Dim SQL As String = String.Format("UPDATE OUTBOUNDORDETAIL SET QTYSOFTALLOCATED = 0 WHERE {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    ' Added doSoftPlan for checking if the planer should realy plan the order or shoul it run the proccess without planning
    Public Sub Plan(ByVal doRelease As Boolean, ByVal pUser As String, Optional ByVal doSoftPlan As Boolean = False)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADING Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.PACKED Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.PICKED Or _
            _status = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Order Status Incorrect", "Order Status Incorrect")
        End If
        If Not doSoftPlan Then
            setStatus(WMS.Lib.Statuses.OutboundOrderHeader.PLANNING, pUser)
        End If

        Dim qh As New Made4Net.Shared.QMsgSender
        qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.PLANORDER)
        qh.Values.Add("DORELEASE", doRelease.ToString())
        qh.Values.Add("CONSIGNEE", _consignee)
        qh.Values.Add("SOFTPALN", doSoftPlan)
        qh.Values.Add("ORDERID", _orderid)
        qh.Values.Add("USER", pUser)
        qh.Send("Planner", _orderid)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderPalnned)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PLANORDER)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.PLANORDER)
    End Sub

    Public Sub PlanComplete(Optional ByVal oLogger As LogHandler = Nothing)
        _status = WMS.Lib.Statuses.OutboundOrderHeader.PLANNED
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now

        setStatus(_status, WMS.Lib.USERS.SYSTEMUSER)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderPalnned)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", "ORDPLANED")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", "")
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("LASTSTATUSUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("LASTCOUNTUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("REASONCODE", "")
        aq.Add("ADDUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Send("ORDPLANED")
    End Sub

    Public Sub Release(ByVal pUser As String)
        Dim SQL As String = String.Format("Select distinct picklist from pickheader where orderid = '{0}' and consignee = '{1}'", _orderid, _consignee)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim oPcks As New WMS.Logic.Picklist(dr("picklist"))
            oPcks.ReleasePicklist(pUser)
        Next
    End Sub

    Public Sub CancelExceptions(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Order status Incorrect", "Order status Incorrect")
        End If
        Dim ordetail As OutboundOrderDetail
        For Each ordetail In Me.Lines
            If ordetail.QTYLEFTTOFULLFILL > 0 Then
                ordetail.CancelExceptions(pUser, -1, Me)
            End If
        Next
    End Sub

    Public Sub CancelExceptionsSetStatus(ByVal pUser As String)
        Dim nextStatus As String = Me.getOrderStatusByActivity()
        If String.IsNullOrEmpty(nextStatus) Then
            Return
        End If

        Me.setStatus(nextStatus, pUser)

    End Sub

#End Region

#Region "Load /Stage /Ship"

    Public Sub SetShipTo(ByVal pContactID As String)
        Dim SQL As String = "UPDATE OUTBOUNDORHEADER SET SHIPTO = '" & pContactID & "' WHERE " & WhereClause
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Ship(ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED AndAlso CanShip Then
            setStatus(WMS.Lib.Statuses.OutboundOrderHeader.SHIPPING, pUser)
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderShipped)
            qh.Values.Add("CONSIGNEE", _consignee)
            qh.Values.Add("DOCUMENT", _orderid)
            qh.Values.Add("USER", pUser)
            qh.Send("Shipper", _consignee & "_" & _orderid)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cant Ship order, incorrect status", "Cant Ship order, incorrect status")
        End If
    End Sub

    'Public Sub ShipOrder(ByVal puser As String)
    '    If CanShip Then
    '        If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
    '            Return
    '        Else
    '            Dim sql As String
    '            Dim dt As New DataTable
    '            Dim dr As DataRow
    '            Dim shpld As Load

    '            If _Loads.Count = 0 Then
    '                Me.UpdateShipStatus(puser)
    '                Return
    '            End If

    '            For Each orderload As OrderLoads In _Loads
    '                Try
    '                    shpld = New WMS.Logic.Load(orderload.LOADID)
    '                    shpld.Ship(puser, orderload.ORDERID, orderload.ORDERLINE)
    '                Catch ex As Exception
    '                End Try
    '            Next
    '        End If
    '    Else
    '        Throw New ApplicationException(String.Format("Order {0} status is incorrect, can't ship order", _orderid))
    '    End If
    'End Sub

    Private Sub UpdateShipStatus(ByVal pUser)
        Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        Dim oldStatus As String = _status
        _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED
        _shippeddate = DateTime.Now
        _statusdate = DateTime.Now

        sql = String.Format("Update OUTBOUNDORHEADER SET SHIPPEDDATE = {0},STATUS = {1},STATUSDATE = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_shippeddate), _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderShipped)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHSHP)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHSHP)
    End Sub

    Private Sub UpdateLoadStatus(ByVal pUser)
        Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        Dim oldStatus As String = _status
        _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED
        _statusdate = DateTime.Now

        sql = String.Format("Update OUTBOUNDORHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderLoaded)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHLD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHLD)
    End Sub

    Public Sub Stage(ByVal pUser As String)
        If CanStage Then
            If _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED Then
                Return
            Else
                Dim sql As String
                'Dim outordetail As OutboundOrderDetail
                'For Each outordetail In Lines
                '    outordetail.Stage(puser)
                'Next
                _editdate = DateTime.Now
                _edituser = pUser
                Dim oldStatus As String = _status
                _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED
                _statusdate = DateTime.Now

                sql = String.Format("Update OUTBOUNDORHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                    Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(sql)

                'Dim em As New EventManagerQ
                'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OrderStaged
                'em.Add("EVENT", EventType)
                'em.Add("ORDERID", _orderid)
                'em.Add("CONSIGNEE", _consignee)
                'em.Add("USERID", pUser)
                'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                'em.Send(WMSEvents.EventDescription(EventType))
                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderStaged)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHSTG)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", _consignee)
                aq.Add("DOCUMENT", _orderid)
                aq.Add("DOCUMENTLINE", "")
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", oldStatus)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", pUser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", pUser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", pUser)
                aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHSTG)
            End If
        Else
            Throw New ApplicationException(String.Format("Order {0} status is incorrect, can't ship order", _orderid))
        End If
    End Sub

#End Region

#Region "Cancel"

    Public Sub CancelOutbound(ByVal puser As String)
        'Select Case _status
        '    Case WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        '        Cancel(puser)
        '    Case WMS.Lib.Statuses.OutboundOrderHeader.PLANNED, WMS.Lib.Statuses.OutboundOrderHeader.RELEASED, _
        '        WMS.Lib.Statuses.OutboundOrderHeader.RELEASING, WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED, _
        '        WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED, WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED
        '        For Each orderLine As WMS.Logic.OutboundOrderHeader.OutboundOrderDetail In Me.Lines
        '            If orderLine.QTYPROCESSED > 0 Then
        '                Throw New M4NException(New Exception(), "Can not cancel order with quantity being processed.", "Can not cancel order with quantity being processed.")
        '            End If
        '        Next
        '    Case Else
        '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "OutboundOrder status is incorrect for canceling", "OutboundOrder status is incorrect for canceling")
        '        Throw m4nEx
        'End Select
        If STATUS = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED OrElse _
        (STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.RELEASED AndAlso _
        STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED AndAlso _
        STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED AndAlso _
        STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW) Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Incorrect status for canceling.", "Incorrect status for canceling.")
        End If

        For Each orderLine As WMS.Logic.OutboundOrderHeader.OutboundOrderDetail In Lines
            If orderLine.QTYALLOCATED > 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception(), "Can not cancel outbound order with allocated quantity.", "Can not cancel outbound order with allocated quantity.")
            End If
        Next

        Cancel(puser)
    End Sub

    Private Sub Cancel(ByVal puser As String)
        Dim outordetail As OutboundOrderDetail
        Dim oldStatus As String = _status

        For Each outordetail In Lines
            outordetail.QTYMODIFIED = 0
            outordetail.EditDetail(outordetail.CONSIGNEE, outordetail.ORDERID, outordetail.ORDERLINE, _
                outordetail.REFERENCEORDLINE, outordetail.SKU, outordetail.INVENTORYSTATUS, outordetail.QTYMODIFIED, _
                outordetail.NOTES, outordetail.ROUTE, outordetail.STOPNUMBER, puser, outordetail.EXPLOADEDFLAG, Nothing, "", "")
        Next
        setStatus(WMS.Lib.Statuses.OutboundOrderHeader.CANCELED, puser)
        SendCancellationEvent(oldStatus, puser)
        ''try to deassign from route if assigned
        'If _route <> "" And Not _route Is Nothing Then
        '    DeAssignFromRoute(puser)
        'End If
    End Sub

    Private Sub SendCancellationEvent(ByVal pFromStatus As String, ByVal pUser As String)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderCancelled)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDCANC)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", pFromStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDCANC)
    End Sub
#End Region

#Region "Reports"

    Public Sub PrintShippingManifest(ByVal Language As Int32, ByVal pUser As String)
        Dim repType As String
        ' Deside wich Delivery Note we need to print
        ' If TargetCompany is not empty then print Company delivery note
        Dim oComp As WMS.Logic.Company = New WMS.Logic.Company(_consignee, _targetcompany, _companytype)
        Dim oCons As WMS.Logic.Consignee = New WMS.Logic.Consignee(_consignee)

        If oComp.DELIVERYNOTELAYOUT.Length > 0 Then
            repType = oComp.DELIVERYNOTELAYOUT
        ElseIf oCons.SHIPPINGMANIFEST.Length > 0 Then
            repType = oCons.SHIPPINGMANIFEST
        Else
            repType = "OutboundDelNote"
        End If

        Dim oQsender As New Made4Net.Shared.QMsgSender

        Dim dt As New DataTable

        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False)
        'Dim rep As Made4Net.Reporting.Common.Report
        'rep = Made4Net.Reporting.Common.Report.getReportInstance("ShipMan")
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        oQsender.Add("DATASETID", DataInterface.ExecuteScalar(String.Format("SELECT ParamValue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = 'DataSetName'", repType))) '"repOutboundDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
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
        oQsender.Add("WHERE", String.Format("CONSIGNEE = '{0}' and ORDERID = '{1}'", _consignee, _orderid))
        oQsender.Send("Report", repType)
    End Sub
#End Region

#Region "Insert/Update"

    Public Sub Save(ByVal pUser As String)
        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitystatus As String

        If Not WMS.Logic.Consignee.Exists(_consignee) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        End If

        If (Not WMS.Logic.Company.Exists(_consignee, _targetcompany, _companytype) And Not WMS.Logic.Contact.Exists(_shipto)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company or customer contact", "Can't create order.Invalid target company or customer contact")
            Throw m4nEx
        End If

        If _staginglane <> "" And _stagingwarehousearea <> "" Then
            If Not WMS.Logic.Location.Exists(_staginglane, _stagingwarehousearea) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            End If
        End If

        Dim sql As String
        If Exists(_consignee, _orderid) Then
            ' Checking fields before saving

            Dim exist As Boolean = True

            '---- Deside if the order is to bychance customer or there is a Company
            exist = WMS.Logic.Company.Exists(_consignee, _targetcompany, _companytype)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
                Throw m4nEx
            Else
                If _shipto Is Nothing Then _shipto = ""
                If _shipto.Length = 0 Then
                    Dim oComp As New WMS.Logic.Company(_consignee, _targetcompany, _companytype)
                    _shipto = oComp.DEFAULTCONTACT
                Else
                    If Not WMS.Logic.Contact.Exists(_shipto) Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                        Throw m4nEx
                    End If
                End If
            End If
            '------------ Company/Customer region END
            Dim oContact As WMS.Logic.Contact = Nothing
            If WMS.Logic.Contact.Exists(_shipto) Then
                oContact = New WMS.Logic.Contact(_shipto)
            End If

            If _staginglane = "" And _stagingwarehousearea = "" Then
                'Try to set the SL according to the contact SL
                If Not oContact Is Nothing Then
                    If oContact.STAGINGLANE <> String.Empty Then
                        _staginglane = oContact.STAGINGLANE
                    End If
                    If oContact.STAGINGWAREHOUSEAREA <> String.Empty Then
                        _stagingwarehousearea = oContact.STAGINGWAREHOUSEAREA
                    End If
                End If
            Else
                exist = WMS.Logic.Location.Exists(_staginglane, _stagingwarehousearea)
                If Not exist Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                    Throw m4nEx
                End If
            End If
            'If _route = "" Or _route = String.Empty Then
            '    'Try to set the SL according to the contact SL
            '    If Not oContact Is Nothing Then
            '        If oContact.ROUTE <> String.Empty Then
            '            _route = oContact.ROUTE
            '        End If
            '    End If
            'End If

            If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Order status incorrect", "Order status incorrect")
            End If
            _editdate = DateTime.Now
            _edituser = pUser
            sql = String.Format("UPDATE OUTBOUNDORHEADER SET ORDERTYPE={0},REFERENCEORD={1},TARGETCOMPANY={2},COMPANYTYPE={3},NOTES={4},STAGINGLANE={5},STAGINGWAREHOUSEAREA={16},REQUESTEDDATE={6},SCHEDULEDDATE={7}" & _
                ",LOADINGSEQ={8},HOSTORDERID={9},EDITDATE={10},EDITUSER={11},ORDERPRIORITY={12}, SHIPTO={13}, ROUTINGSET={14},DELIVERYSTATUS={17},POD={18} WHERE {15}", Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_targetcompany), _
                Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_requesteddate), Made4Net.Shared.Util.FormatField(_scheduleddate), _
                 Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_hostorderid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_orderpriority), Made4Net.Shared.Util.FormatField(_shipto), Made4Net.Shared.Util.FormatField(_routingset), WhereClause, Made4Net.Shared.Util.FormatField(_stagingwarehousearea), Made4Net.Shared.Util.FormatField(_deliverystatus), Made4Net.Shared.Util.FormatField(_pod))

            DataInterface.RunSQL(sql)

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderUpdated)
            activitystatus = WMS.Lib.Actions.Audit.OUTBOUNDHUPD

        Else
            _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
            Dim exist As Boolean = True


            exist = WMS.Logic.Company.Exists(_consignee, _targetcompany, _companytype)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
                Throw m4nEx
            Else
                If _shipto Is Nothing Then _shipto = ""
                If _shipto.Length = 0 Then
                    _targetcompany = _targetcompany
                    Dim oComp As New WMS.Logic.Company(_consignee, _targetcompany, _companytype)
                    _shipto = oComp.DEFAULTCONTACT
                Else
                    If Not WMS.Logic.Contact.Exists(_shipto) Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                        Throw m4nEx
                    End If
                End If
            End If
            '------------ Company/Customer region END
            Dim oContact As WMS.Logic.Contact = Nothing
            If WMS.Logic.Contact.Exists(_shipto) Then
                oContact = New WMS.Logic.Contact(_shipto)
            End If

            If _staginglane = "" And _stagingwarehousearea = "" Then
                'Try to set the SL according to the contact SL
                If Not oContact Is Nothing Then
                    If oContact.STAGINGLANE <> String.Empty Then
                        _staginglane = oContact.STAGINGLANE
                    End If
                    If oContact.STAGINGWAREHOUSEAREA <> String.Empty Then
                        _stagingwarehousearea = oContact.STAGINGWAREHOUSEAREA
                    End If
                End If
            Else
                exist = WMS.Logic.Location.Exists(_staginglane, _stagingwarehousearea)
                If Not exist Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                    Throw m4nEx
                End If
            End If
            'If _route = "" Or _route = String.Empty Then
            '    'Try to set the SL according to the contact SL
            '    If Not oContact Is Nothing Then
            '        If oContact.ROUTE <> String.Empty Then
            '            _route = oContact.ROUTE
            '        End If
            '    End If
            'End If

            _createdate = DateTime.Now
            _editdate = DateTime.Now
            _edituser = pUser
            _adduser = pUser
            _adddate = DateTime.Now
            _statusdate = DateTime.Now
            sql = String.Format("INSERT INTO OUTBOUNDORHEADER(CONSIGNEE,ORDERID,ORDERTYPE,REFERENCEORD,TARGETCOMPANY,COMPANYTYPE,STATUS,CREATEDATE,NOTES,STAGINGLANE,STAGINGWAREHOUSEAREA, REQUESTEDDATE,SCHEDULEDDATE," & _
               "STATUSDATE,LOADINGSEQ,HOSTORDERID,ORDERPRIORITY, SHIPTO, ROUTINGSET,DELIVERYSTATUS, POD, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24})", Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
               Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_targetcompany), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_notes), _
               Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_stagingwarehousearea), Made4Net.Shared.Util.FormatField(_requesteddate), Made4Net.Shared.Util.FormatField(_scheduleddate), Made4Net.Shared.Util.FormatField(_statusdate), _
               Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_hostorderid), Made4Net.Shared.Util.FormatField(_orderpriority), Made4Net.Shared.Util.FormatField(_shipto), Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_deliverystatus), Made4Net.Shared.Util.FormatField(_pod), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
            DataInterface.RunSQL(sql)
            _Lines = New OutboundOrderDetailsCollection(_consignee, _orderid)

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderCreated)
            activitystatus = WMS.Lib.Actions.Audit.OUTBOUNDHINS

        End If

        aq.Add("ACTIVITYTYPE", activitystatus)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(activitystatus)
    End Sub


    Public Sub CreateNew(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrdType As String, ByVal pTargetCompany As String, _
            ByVal pCompType As String, ByVal pNotes As String, ByVal pExpDate As DateTime, ByVal pSchedDate As DateTime, ByVal pOrderPriority As Integer, _
            ByVal pHostOrderId As String, ByVal pLoadingSeq As String, ByVal pRefOrd As String, ByVal pStagingLane As String, ByVal pStagingwarehousearea As String, ByVal pShipTo As String, ByVal pDeliveryStatus As String, ByVal pUser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        exist = WMS.Logic.Consignee.Exists(pConsignee)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        Else
            _consignee = pConsignee
        End If
        exist = WMS.Logic.OutboundOrderHeader.Exists(pConsignee, pOrderId)
        If exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.OrderId already Exist", "Can't create order.OrderId already Exist")
            Throw m4nEx
        Else
            _orderid = pOrderId
        End If

        exist = WMS.Logic.Company.Exists(_consignee, pTargetCompany, pCompType)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If pShipTo Is Nothing Then pShipTo = ""
            If pShipTo.Length = 0 Then
                _targetcompany = pTargetCompany
                Dim oComp As New WMS.Logic.Company(pConsignee, pTargetCompany, pCompType)
                _shipto = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pShipTo) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _shipto = pShipTo
                _targetcompany = pTargetCompany
            End If
        End If
        '------------ Company/Customer region END
        Dim oContact As WMS.Logic.Contact = Nothing
        If WMS.Logic.Contact.Exists(_shipto) Then
            oContact = New WMS.Logic.Contact(_shipto)
        End If

        If pStagingLane = "" And pStagingwarehousearea = "" Then
            'Try to set the SL according to the contact SL
            If Not oContact Is Nothing Then
                If oContact.STAGINGLANE <> String.Empty Then
                    _staginglane = oContact.STAGINGLANE
                End If
                If oContact.STAGINGWAREHOUSEAREA <> String.Empty Then
                    _stagingwarehousearea = oContact.STAGINGWAREHOUSEAREA
                End If
            End If
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
        'If _route = "" Or _route = String.Empty Then
        '    'Try to set the SL according to the contact SL
        '    If Not oContact Is Nothing Then
        '        If oContact.ROUTE <> String.Empty Then
        '            _route = oContact.ROUTE
        '        End If
        '    End If
        'Else
        '    _route = pRoute
        'End If

        _ordertype = pOrdType
        _companytype = pCompType
        _referenceord = pRefOrd
        _notes = pNotes
        _scheduleddate = pSchedDate
        _requesteddate = pExpDate
        '_stopnumber = pStopNum
        _hostorderid = pHostOrderId
        _loadingseq = pLoadingSeq
        _orderpriority = pOrderPriority
        _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        _editdate = DateTime.Now
        _edituser = pUser
        _adddate = DateTime.Now
        _adduser = pUser

        SQL = "INSERT INTO OUTBOUNDORHEADER (CONSIGNEE,ORDERID ,ORDERTYPE ,REFERENCEORD ,TARGETCOMPANY ,COMPANYTYPE ,STATUS, CREATEDATE,  NOTES " & _
              " ,STAGINGLANE ,STAGINGWAREHOUSEAREA, REQUESTEDDATE ,SCHEDULEDDATE, HOSTORDERID, LOADINGSEQ, STATUSDATE , SHIPTO, ORDERPRIORITY,ADDDATE ,ADDUSER , EDITDATE ,EDITUSER) values( "
        SQL += Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_orderid) & "," & _
            Made4Net.Shared.Util.FormatField(_ordertype) & "," & Made4Net.Shared.Util.FormatField(_referenceord) & "," & Made4Net.Shared.Util.FormatField(_targetcompany) & "," & _
            Made4Net.Shared.Util.FormatField(_companytype) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(DateTime.Now) & "," & _
            Made4Net.Shared.Util.FormatField(_notes) & "," & Made4Net.Shared.Util.FormatField(_staginglane) & "," & Made4Net.Shared.Util.FormatField(_stagingwarehousearea) & "," & Made4Net.Shared.Util.FormatField(_requesteddate) & "," & _
            Made4Net.Shared.Util.FormatField(_scheduleddate) & "," & _
            Made4Net.Shared.Util.FormatField(_hostorderid) & "," & Made4Net.Shared.Util.FormatField(_loadingseq) & "," & _
            Made4Net.Shared.Util.FormatField(DateTime.Now) & "," & Made4Net.Shared.Util.FormatField(_shipto) & "," & Made4Net.Shared.Util.FormatField(_orderpriority) & "," & Made4Net.Shared.Util.FormatField(DateTime.Now) & "," & Made4Net.Shared.Util.FormatField(pUser) & "," & Made4Net.Shared.Util.FormatField(DateTime.Now) & "," & _
            Made4Net.Shared.Util.FormatField(pUser) & ")"

        DataInterface.RunSQL(SQL)
        _Lines = New OutboundOrderDetailsCollection(_consignee, _orderid)

        'Raise Order Created Event
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHINS)
    End Sub

    Public Sub SetStagingLane(ByVal pStagingLane As String, ByVal pStagingwarehousearea As String, ByVal pRequestedDate As DateTime, ByVal pUser As String)
        Dim SQL As String = String.Empty
        Dim exist As Boolean = True

        If _status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW Or _status = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPMENTASSIGNED Then
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
            Try
                _requesteddate = pRequestedDate
            Catch ex As Exception

            End Try
            _editdate = DateTime.Now
            _edituser = pUser
            _adddate = DateTime.Now
            _adduser = pUser

            SQL = String.Format("update outboundorheader set CONSIGNEE= {0} ,ORDERID = {1} ,ORDERTYPE= {2}  ,REFERENCEORD = {3} ,TARGETCOMPANY = {4} ,COMPANYTYPE= {5}  ,STATUS= {6} ,  NOTES= {7} , " & _
                  " STAGINGLANE= {8},STAGINGWAREHOUSEAREA={15}, REQUESTEDDATE= {9},SCHEDULEDDATE= {10}, STATUSDATE= {11}, EDITDATE= {12},EDITUSER= {13} where {14}", _
                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
                Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_targetcompany), _
                Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_status), _
                Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_requesteddate), _
                Made4Net.Shared.Util.FormatField(_scheduleddate), _
                Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(DateTime.Now), _
                Made4Net.Shared.Util.FormatField(pUser), WhereClause, Made4Net.Shared.Util.FormatField(_stagingwarehousearea))

            DataInterface.RunSQL(SQL)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot update order.Invalid Status", "Can't update order.Invalid Status")
            Throw m4nEx
        End If

    End Sub

    Public Sub EditOrder(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrdType As String, ByVal pTargetCompany As String, _
            ByVal pCompType As String, ByVal pNotes As String, ByVal pExpDate As DateTime, ByVal pSchedDate As DateTime, _
            ByVal pHostOrderId As String, ByVal pLoadingSeq As String, _
            ByVal pRefOrd As String, ByVal pStagingLane As String, ByVal pStagingwarehousearea As String, ByVal pShipTo As String, ByVal pDeliveryStatus As String, ByVal pUser As String)


        If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Order status incorrect", "Order status incorrect")
        End If
        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        _consignee = pConsignee
        _orderid = pOrderId
        _ordertype = pOrdType
        _companytype = pCompType
        _referenceord = pRefOrd
        _notes = pNotes
        _scheduleddate = pSchedDate
        _requesteddate = pExpDate
        '_stopnumber = pStopNum
        '_route = pRoute
        _hostorderid = pHostOrderId
        _loadingseq = pLoadingSeq
        If pShipTo Is Nothing Then pShipTo = ""
        '---- Deside if the order is to bychance customer or there is a Company
        exist = WMS.Logic.Company.Exists(_consignee, pTargetCompany, pCompType)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If pShipTo.Length = 0 Then
                _targetcompany = pTargetCompany
                Dim oComp As New WMS.Logic.Company(pConsignee, pTargetCompany, pCompType)
                _shipto = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pShipTo) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _shipto = pShipTo
                _targetcompany = pTargetCompany
            End If
        End If
        '------------ Company/Customer region END

        exist = WMS.Logic.Location.Exists(pStagingLane, pStagingwarehousearea)
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

        '_status = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW
        _editdate = DateTime.Now
        _edituser = pUser
        _adddate = DateTime.Now
        _adduser = pUser

        SQL = String.Format("UPDATE OUTBOUNDORHEADER SET CONSIGNEE= {0} ,ORDERID = {1} ,ORDERTYPE= {2}  ,REFERENCEORD = {3} ," & _
        "TARGETCOMPANY = {4} ,COMPANYTYPE= {5}  ,STATUS= {6} ,  NOTES= {7} , STAGINGLANE= {8},STAGINGWAREHOUSEAREA={9}, REQUESTEDDATE= {10}" & _
        ",SCHEDULEDDATE= {11},STATUSDATE= {12}, EDITDATE= {13},EDITUSER= {14}, HOSTORDERID={15}, LOADINGSEQ={16}, SHIPTO={17} where {18}", _
            Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
            Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), _
            Made4Net.Shared.Util.FormatField(_targetcompany), Made4Net.Shared.Util.FormatField(_companytype), _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_notes), _
            Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_stagingwarehousearea), _
            Made4Net.Shared.Util.FormatField(_requesteddate), Made4Net.Shared.Util.FormatField(_scheduleddate), _
            Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(DateTime.Now), _
            Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(_hostorderid), _
            Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_shipto), WhereClause)

        DataInterface.RunSQL(SQL)
        'Raise Order Created Event
        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OutboundOrderUpdated
        'em.Add("EVENT", EventType)
        'em.Add("ORDERID", _orderid)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("USERID", pUser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutboundOrderUpdated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHUPD)
    End Sub

#End Region

#Region "Add/Update Lines"

    Private Sub AddOrderLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pInputSku As String, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing)

        If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPING OrElse _
        _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED OrElse _
        _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
            Throw New M4NException(New Exception, "Can not add order line. Incorrect status.", "Can not add order line. Incorrect status.")
        End If
        _Lines.CreateNewLine(pConsignee, pOrderId, pRefOrdLine, pSku, pInvStat, pQty, pInputSku, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea, pInputQTY, pInpupUOM)
    End Sub

    Private Sub AddOrderLine(ByVal oLine As OutboundOrderDetail)
        'Check If we got an input UOM to calculate the ordered qty --> moved to BO
        'Dim units As Decimal
        'If IsDBNull(oLine.INPUTUOM) Or oLine.INPUTUOM = "" Then
        '    units = oLine.QTYORIGINAL
        'Else
        '    units = CalcUnits(oLine.SKU, oLine.INPUTQTY, oLine.INPUTUOM) * oLine.INPUTQTY
        'End If

        If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPING OrElse _
           _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED OrElse _
           _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
            Throw New M4NException(New Exception, "Can not add order line. Incorrect status.", "Can not add order line. Incorrect status.")
        End If

        If _Lines Is Nothing Then
            _Lines = New OutboundOrderDetailsCollection(oLine.CONSIGNEE, oLine.ORDERID, True)
        End If
        If OutboundOrderDetail.Exists(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE) Then
            _Lines.EditLine(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE, oLine.REFERENCEORDLINE, oLine.SKU, oLine.INVENTORYSTATUS, oLine.QTYMODIFIED, oLine.NOTES, oLine.ROUTE, oLine.STOPNUMBER, oLine.EDITUSER, oLine.EXPLOADEDFLAG, oLine.Attributes.Attributes, oLine.WarehouseArea, oLine.INPUTSKU, oLine.INPUTQTY, oLine.INPUTUOM)
        Else
            _Lines.CreateNewLine(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE, oLine.REFERENCEORDLINE, oLine.SKU, oLine.INVENTORYSTATUS, oLine.QTYORIGINAL, oLine.INPUTSKU, oLine.NOTES, oLine.ROUTE, oLine.STOPNUMBER, oLine.ADDUSER, oLine.EXPLOADEDFLAG, oLine.Attributes.Attributes, oLine.WarehouseArea, oLine.INPUTQTY, oLine.INPUTUOM)
        End If
    End Sub

    Private Sub EditOrderLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLineNumber As String, ByVal pRefOrdLine As Int32, ByVal pSku As String, _
                ByVal pInvStat As String, ByVal pQty As Decimal, ByVal pNotes As String, ByVal pRoute As String, ByVal pStopNumber As Int32, ByVal pUser As String, ByVal pExploadedFlag As Int32, ByVal oAttributeCollection As AttributesCollection, ByVal pWarehouseArea As String, Optional ByVal pInputSku As String = Nothing, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInputUOM As String = Nothing)

        If _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPING OrElse _
        _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED OrElse _
        _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
            Throw New M4NException(New Exception, "Can not edit order line. Incorrect status.", "Can not edit order line. Incorrect status.")
        End If
        _Lines.EditLine(pConsignee, pOrderId, pLineNumber, pRefOrdLine, pSku, pInvStat, pQty, pNotes, pRoute, pStopNumber, pUser, pExploadedFlag, oAttributeCollection, pWarehouseArea)
    End Sub

#End Region

#Region "Print Label"

    Public Sub PrintLabel(ByVal prtrName As String)
        If prtrName Is Nothing Then
            prtrName = ""
        End If
        Dim qSender As New Made4Net.Shared.QMsgSender
        qSender.Add("LABELNAME", "OUTBOUNDORDER")
        qSender.Add("LabelType", "OUTBOUNDORDER")
        qSender.Add("PRINTER", prtrName)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("CONSIGNEE", _consignee)
        ht.Hash.Add("ORDERID", _orderid)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Add("CONSIGNEE", _consignee)
        qSender.Add("ORDERID", _orderid)
        qSender.Send("Label", "Outbound Order Label")
    End Sub

    Public Sub PrintShipLabels(Optional ByVal pPrinter As String = "")
        PrintContShipLabels("", pPrinter)
        PrintLoadShipLabels("", pPrinter)
    End Sub

    'For Partial Picks and Packing
    Public Sub PrintContShipLabels(Optional ByVal pContainerId As String = "", Optional ByVal pPrinter As String = "")
        Dim SQL As String = String.Format("SELECT distinct pickcontainer,picklist FROM vOutboundorderLoads where orderid = '{0}' and consignee = '{1}' and pickcontainer <> '' and pickcontainer is not null and pickcontainer like '%{2}'", _
                        _orderid, _consignee, pContainerId)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return
        Else
            For Each dr As DataRow In dt.Rows
                Dim oPck As New WMS.Logic.Picklist(dr("picklist"))
                Dim lblFormat As String = oPck.GetShipLabelFormat()
                Dim cnt As New WMS.Logic.Container(dr("pickcontainer"), True)
                cnt.PrintShipLabel(pPrinter, lblFormat)
            Next
        End If
    End Sub

    'For Full Picks
    Public Sub PrintLoadShipLabels(Optional ByVal pLoadid As String = "", Optional ByVal pPrinter As String = "")
        Dim SQL As String = String.Format("SELECT distinct loadid,picklist FROM vOutboundorderLoads where orderid = '{0}' and consignee = '{1}' and (pickcontainer = '' or pickcontainer is null) and loadid like '%{2}'", _
                        _orderid, _consignee, pLoadid)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return
        Else
            For Each dr As DataRow In dt.Rows
                Dim oPck As New WMS.Logic.Picklist(dr("picklist"))
                Dim lblFormat As String = oPck.GetShipLabelFormat()
                Dim ld As WMS.Logic.Load = New WMS.Logic.Load(System.Convert.ToString(dr("loadid")))
                ld.PrintShippingLabel("", lblFormat)
            Next
        End If
    End Sub


#End Region

#Region "Verification"

    'Public Sub Verify(ByVal pContId As String)
    '    'cont exist?
    '    Dim SQL As String
    '    Dim user As String = Common.GetCurrentUser
    '    Dim count As Int32
    '    SQL = String.Format("select count(*) from pickdetail " & _
    '            " where tocontainer = '{0}'", pContId)
    '    count = DataInterface.ExecuteScalar(SQL)
    '    If count < 1 Then
    '        Throw New Made4Net.Shared.M4NException(New Exception("Container not exist"), "Container not exist", "Container not exist")
    '    End If

    '    'check that all activities are correct
    '    SQL = String.Format("select distinct toload,status from pickdetail " & _
    '            " where tocontainer = '{0}'", pContId)
    '    Dim AllPicked As Boolean = True
    '    Dim dsLoads As New DataSet
    '    Dim dsOrders As New DataSet
    '    Dim dr As DataRow
    '    Dim oLoad As Load
    '    DataInterface.FillDataset(SQL, dsLoads)
    '    For Each dr In dsLoads.Tables(0).Rows
    '        If dr("status") = WMS.Lib.Statuses.Picklist.COMPLETE Or dr("status") = WMS.Lib.Statuses.Picklist.CANCELED Then
    '            oLoad = New Load(dr("toload"), True)
    '            If oLoad.ACTIVITYSTATUS <> WMS.Lib.Statuses.ActivityStatus.PICKED Then
    '                AllPicked = False
    '                Exit For
    '            End If
    '            oLoad = Nothing
    '        Else
    '            AllPicked = False
    '            Exit For
    '        End If
    '    Next
    '    SQL = String.Format("select distinct orderid,consignee from pickdetail " & _
    '     " where tocontainer = '{0}'", pContId)
    '    DataInterface.FillDataset(SQL, dsOrders)
    '    For Each dr In dsOrders.Tables(0).Rows
    '        Dim oOrder As New OutboundOrderHeader(dr("consignee"), dr("orderid"))
    '        If oOrder.STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.PICKED Then
    '            AllPicked = False
    '            Exit For
    '        End If
    '    Next
    '    If Not AllPicked Then
    '        Throw New Made4Net.Shared.M4NException(New Exception("Not all Loads were picked"), "Container not verified", "Container not verified")
    '    End If

    '    'else start change statuses
    '    For Each dr In dsLoads.Tables(0).Rows
    '        oLoad = New Load(dr("toload"), True)
    '        oLoad.Verify()
    '        oLoad = Nothing
    '    Next

    '    For Each dr In dsOrders.Tables(0).Rows
    '        Dim oOrder As New OutboundOrderHeader(dr("consignee"), dr("orderid"))
    '        Dim oldStat As String = oOrder.STATUS
    '        oOrder.setStatus(WMS.Lib.Statuses.ActivityStatus.VERIFIED, user)
    '        ''send to audit
    '        'Dim redQ As New EventManagerQ
    '        'redQ.Add("ACTION", WMS.Lib.Actions.Audit.SETORDSTAT)
    '        'redQ.Add("USERID", user)
    '        'redQ.Add("ORDERID", oOrder.ORDERID)
    '        'redQ.Add("FROMSTATUS", oldStat)
    '        'redQ.Add("TOSTATUS", WMS.Lib.Statuses.ActivityStatus.VERIFIED)
    '        'redQ.Add("CONSIGNEE", oOrder.CONSIGNEE)
    '        'redQ.Add("DOCUMENT", oOrder.ORDERID)
    '        'redQ.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '        'redQ.Send(WMS.Lib.Actions.Audit.SETORDSTAT)

    '        Dim aq As EventManagerQ = New EventManagerQ
    '        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadAttributeChanged)
    '        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SETORDSTAT)
    '        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '        aq.Add("ACTIVITYTIME", "0")
    '        aq.Add("CONSIGNEE", oOrder.CONSIGNEE)
    '        aq.Add("DOCUMENT", oOrder.ORDERID)
    '        aq.Add("DOCUMENTLINE", 0)
    '        aq.Add("FROMLOAD", "")
    '        aq.Add("FROMLOC", "")
    '        aq.Add("FROMQTY", 0)
    '        aq.Add("FROMSTATUS", oldStat)
    '        aq.Add("NOTES", "")
    '        aq.Add("SKU", "")
    '        aq.Add("TOLOAD", "")
    '        aq.Add("TOLOC", "")
    '        aq.Add("TOQTY", 0)
    '        aq.Add("TOSTATUS", WMS.Lib.Statuses.ActivityStatus.VERIFIED)
    '        aq.Add("USERID", user)
    '        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '        aq.Add("ADDUSER", user)
    '        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '        aq.Add("EDITUSER", user)
    '        aq.Send(WMS.Lib.Actions.Audit.SETORDSTAT)
    '    Next
    'End Sub

    Public Sub Verify(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED Then
            Return
        Else
            Dim sql As String
            _editdate = DateTime.Now
            _edituser = pUser
            Dim oldStatus As String = _status
            _status = WMS.Lib.Statuses.OutboundOrderHeader.VERIFIED
            _statusdate = DateTime.Now

            sql = String.Format("Update OUTBOUNDORHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(sql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutBoundOrderVerified)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHVRF)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _orderid)
            aq.Add("DOCUMENTLINE", "")
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", oldStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.OUTBOUNDHVRF)
        End If
    End Sub



#End Region

#Region "Work Order"

    Public Function GenerateWorkOrderFromLine(ByVal pOrderLine As Int32, ByVal pUser As String)
        Return Me.Lines().Line(pOrderLine).GenerateWorkOrder(_ordertype, _referenceord, _requesteddate, _notes, pUser)
    End Function

#End Region

#Region "Handling Units"

    Public Sub AddHandlingUnit(ByVal pHandlingUnitType As String, ByVal pQty As Decimal, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Order Status", "Incorrect Order Status")
        End If
        Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction
        oHUTrans.Create(Nothing, WMS.Logic.HandlingUnitTransactionTypes.OutboundOrder, "", _consignee, _
                   _orderid, _ordertype, _targetcompany, _companytype, pHandlingUnitType, pQty, pUser)
        _handlingunits.Add(oHUTrans)
    End Sub

    Public Sub UpdateHandlingUnit(ByVal pHUTransId As String, ByVal pQty As Decimal, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Order Status", "Incorrect Order Status")
        End If
        Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction(pHUTransId)
        oHUTrans.UpdateHUQty(pQty, pUser)
        _handlingunits.Remove(_handlingunits.HandlingUnitTransaction(pHUTransId))
        _handlingunits.Add(oHUTrans)
    End Sub

#End Region

#End Region

End Class

#End Region

#Region "OutbooundOrderLoadsCollection"

<CLSCompliant(False)> Public Class OutboundOrderLoadsCollection
    Inherits ArrayList

#Region "Variables"

    Protected _consignee As String
    Protected _orderid As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As OrderLoads
        Get
            Return CType(MyBase.Item(index), OrderLoads)
        End Get
    End Property

    Public ReadOnly Property OrderLoad(ByVal pLoadid As String) As OrderLoads
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).LOADID = pLoadid Then
                    Return (CType(MyBase.Item(i), OrderLoads))
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
        SQL = "SELECT * FROM ORDERLOADS WHERE DOCUMENTTYPE='" + WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER + "' AND CONSIGNEE = '" & _consignee & "' AND ORDERID = '" & _orderid & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            'Me.add(New OrderLoads(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, _consignee, _orderid, dr.Item("LOADID"), LoadAll))
            Me.add(New OrderLoads(dr))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As OrderLoads) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As OrderLoads)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As OrderLoads)
        MyBase.Remove(pObject)
    End Sub

    Public Sub AddLoad(ByVal pLoadid As String, ByVal pOrderLine As Int32, ByVal pPickList As String, ByVal pPickListLine As Int32, ByVal pUser As String)
        Dim oOrderLoad As OrderLoads = OrderLoad(pLoadid)
        If IsNothing(oOrderLoad) Then
            oOrderLoad = New OrderLoads
            oOrderLoad.CreateOrderLoad(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, _consignee, _orderid, pOrderLine, pLoadid, pPickList, pPickListLine, pUser)
            Me.add(oOrderLoad)
            'Else
            '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot add load to OutboundOrder", "Cannot add load to OutboundOrder")
            '    m4nEx.Params.Add("pLoadid", pLoadid)
            '    m4nEx.Params.Add("consignee", _consignee)
            '    m4nEx.Params.Add("orderid", _orderid)
            '    Throw m4nEx
        End If
    End Sub

    Public Sub RemoveLoad(ByVal pLoadid As String, ByVal pUser As String)
        Dim oOrderLoad As OrderLoads = OrderLoad(pLoadid)
        If Not IsNothing(oOrderLoad) Then
            oOrderLoad.DeleteOrderLoad(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, _consignee, _orderid, pLoadid, pUser)
            Me.Remove(oOrderLoad)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot remove load to OutboundOrder", "Cannot remove load to OutboundOrder")
            m4nEx.Params.Add("pLoadid", pLoadid)
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("orderid", _orderid)
            Throw m4nEx
        End If
    End Sub

    Public Sub UnpickLoad(ByVal pLoadid As String, ByVal pUser As String)
        Dim oOrderLoad As OrderLoads = OrderLoad(pLoadid)
        If Not IsNothing(oOrderLoad) Then
            oOrderLoad.UndoPick(pUser)
            Me.Remove(oOrderLoad)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unpick load to OutboundOrder", "Cannot unpick load to OutboundOrder")
            m4nEx.Params.Add("pLoadid", pLoadid)
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("orderid", _orderid)
            Throw m4nEx
        End If
    End Sub

#End Region

End Class

#End Region