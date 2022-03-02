Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "InboundOrderDetail"

' <summary>
' This object represents the properties and methods of a InboundOrderDetail.
' </summary>

<CLSCompliant(False)> Public Class InboundOrderDetail

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _orderid As String = String.Empty
    Protected _orderline As Int32

#End Region

#Region "Other Fields"

    Protected _referenceordline As String = String.Empty
    Protected _expecteddate As DateTime
    Protected _lastreceiptdate As DateTime
    Protected _sku As String = String.Empty
    Protected _qtyordered As Decimal
    Protected _qtyadjusted As Decimal
    Protected _qtyreceived As Decimal

    Protected _inputqty As Decimal
    Protected _inputsku As String = String.Empty
    Protected _inputuom As String = String.Empty
    Protected _inventorystatus As String = String.Empty

    Protected _adddate As DateTime
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

    Public ReadOnly Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
    End Property

    Public ReadOnly Property ORDERID() As String
        Get
            Return _orderid
        End Get
    End Property

    Public Property ORDERLINE() As Int32
        Get
            Return _orderline
        End Get
        Set(ByVal Value As Int32)
            _orderline = Value
        End Set
    End Property

    Public Property REFERENCEORDLINE() As String
        Get
            Return _referenceordline
        End Get
        Set(ByVal Value As String)
            _referenceordline = Value
        End Set
    End Property

    Public Property EXPECTEDDATE() As DateTime
        Get
            Return _expecteddate
        End Get
        Set(ByVal Value As DateTime)
            _expecteddate = Value
        End Set
    End Property

    Public Property LASTRECEIPTDATE() As DateTime
        Get
            Return _lastreceiptdate
        End Get
        Set(ByVal Value As DateTime)
            _lastreceiptdate = Value
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

    Public Property QTYORDERED() As Decimal
        Get
            Return _qtyordered
        End Get
        Set(ByVal Value As Decimal)
            _qtyordered = Value
        End Set
    End Property

    Public Property QTYADJUSTED() As Decimal
        Get
            Return _qtyadjusted
        End Get
        Set(ByVal Value As Decimal)
            _qtyadjusted = Value
        End Set
    End Property

    Public Property QTYRECEIVED() As Decimal
        Get
            Return _qtyreceived
        End Get
        Set(ByVal Value As Decimal)
            _qtyreceived = Value
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

    Public Property INPUTSKU() As String
        Get
            Return _inputsku
        End Get
        Set(ByVal Value As String)
            _inputsku = Value
        End Set
    End Property

    Public Property INPUTUOM() As String
        Get
            Return _inputuom
        End Get
        Set(ByVal Value As String)
            _inputuom = Value
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

    Public Property Attributes() As InventoryAttributeBase
        Get
            Return _attributes
        End Get
        Set(ByVal Value As InventoryAttributeBase)
            _attributes = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InboundOrder)
    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pORDERID As String, ByVal pORDERLINE As Int32, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _orderid = pORDERID
        _orderline = pORDERLINE
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetInboundOrderDetail(ByVal pCONSIGNEE As String, ByVal pORDERID As String, ByVal pORDERLINE As Int32) As InboundOrderDetail
        Return New InboundOrderDetail(pCONSIGNEE, pORDERID, pORDERLINE)
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM InboundOrdDetail where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Inbound Order Detail Does Not Exists", "Inbound Order Detail Does Not Exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("REFERENCEORDLINE") Then _referenceordline = dr.Item("REFERENCEORDLINE")
        If Not dr.IsNull("EXPECTEDDATE") Then _expecteddate = dr.Item("EXPECTEDDATE")
        If Not dr.IsNull("LASTRECEIPTDATE") Then _lastreceiptdate = dr.Item("LASTRECEIPTDATE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("QTYORDERED") Then _qtyordered = dr.Item("QTYORDERED")
        If Not dr.IsNull("QTYADJUSTED") Then _qtyadjusted = dr.Item("QTYADJUSTED")
        If Not dr.IsNull("QTYRECEIVED") Then _qtyreceived = dr.Item("QTYRECEIVED")

        If Not dr.IsNull("inputqty") Then _inputqty = dr.Item("inputqty")
        If Not dr.IsNull("inputuom") Then _inputuom = dr.Item("inputuom")
        If Not dr.IsNull("INVENTORYSTATUS") Then _inventorystatus = dr.Item("INVENTORYSTATUS")
        If Not dr.IsNull("INPUTSKU") Then _inputsku = dr.Item("INPUTSKU")

        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InboundOrder, _consignee, _orderid, _orderline)
    End Sub

    Public Sub Receive(ByVal pQty As Decimal, ByVal pUser As String)

        Dim uColl As New System.Collections.Specialized.NameValueCollection
        _qtyreceived = _qtyreceived + pQty
        If (_qtyreceived < 0) Then
            _qtyreceived = 0
        End If
        _editdate = DateTime.Now
        _edituser = pUser
        _lastreceiptdate = DateTime.Now

        uColl.Add("QTYRECEIVED", Made4Net.Shared.Util.FormatField(_qtyreceived))
        uColl.Add("EDITDATE", Made4Net.Shared.Util.FormatField(_editdate))
        uColl.Add("EDITUSER", Made4Net.Shared.Util.FormatField(_edituser))
        uColl.Add("LASTRECEIPTDATE", Made4Net.Shared.Util.FormatField(_lastreceiptdate))

        Update(uColl)
    End Sub

    Public Sub CancelReceive(ByVal pQty As Decimal, ByVal pUser As String)

        Dim uColl As New System.Collections.Specialized.NameValueCollection
        _qtyreceived = _qtyreceived - pQty
        _editdate = DateTime.Now
        _edituser = pUser
        _lastreceiptdate = DateTime.Now

        uColl.Add("QTYRECEIVED", Made4Net.Shared.Util.FormatField(_qtyreceived))
        uColl.Add("EDITDATE", Made4Net.Shared.Util.FormatField(_editdate))
        uColl.Add("EDITUSER", Made4Net.Shared.Util.FormatField(_edituser))
        uColl.Add("LASTRECEIPTDATE", Made4Net.Shared.Util.FormatField(_lastreceiptdate))

        Update(uColl)
    End Sub

    Public Sub Update(ByVal updateFields As System.Collections.Specialized.NameValueCollection)
        Dim SqlUpdateStatement As String = Made4Net.DataAccess.SQLStatement.CreateUpdateStatement("InboundOrdDetail", updateFields, WhereClause)
        DataInterface.RunSQL(SqlUpdateStatement)
    End Sub

    Public Sub CreateLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLineNumber As Int32, ByVal pRefOrd As String, ByVal pExpDate As DateTime, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing)

        Dim oSku As WMS.Logic.SKU
        If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
            If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                'Added for RWMS-1509 and RWMS-1502
                Dim strErrDesc As String = "Cannot create order detail.Sku " + pInputSku + " does not Exist for consignee " + pConsignee
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order detail.Sku not Exist", strErrDesc)
                'Ended for RWMS-1509 and RWMS-1502
                Throw m4nEx
            Else
                oSku = New SKU(pConsignee, pInputSku)
                _inputsku = pInputSku
                _sku = oSku.SKU
            End If
        Else
            oSku = New SKU(pConsignee, pSku)
            _sku = pSku
        End If
        If pQty < 0 Then
            If pInpupUOM = String.Empty Or pInpupUOM Is Nothing Or pInputQTY < 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity entered for order line", "Invalid quantity entered for order line")
            Else
                If pInpupUOM <> "" And Not pInpupUOM Is Nothing Then
                    If Not Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pInpupUOM) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
                    End If
                End If
                pQty = oSku.ConvertToUnits(pInpupUOM) * pInputQTY
                _inputqty = pInputQTY
                _inputuom = pInpupUOM
            End If
        Else
            'Do nothing, leave supplied quantity as received...
        End If

        _consignee = pConsignee
        _orderid = pOrderId
        _orderline = pLineNumber
        _referenceordline = pRefOrd
        _expecteddate = pExpDate
        _qtyordered = pQty
        _qtyadjusted = pQty
        _qtyreceived = 0
        _inventorystatus = pInventoryStatus

        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("INSERT INTO INBOUNDORDDETAIL(CONSIGNEE,ORDERID,ORDERLINE,REFERENCEORDLINE,EXPECTEDDATE,LASTRECEIPTDATE,SKU,QTYORDERED,QTYADJUSTED,QTYRECEIVED,INVENTORYSTATUS, ADDDATE,ADDUSER,EDITDATE,EDITUSER,INPUTUOM,INPUTQTY,INPUTSKU) VALUES " & _
            "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}, {16}, {17})", Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_referenceordline), _
            Made4Net.Shared.Util.FormatField(_expecteddate), Made4Net.Shared.Util.FormatField(_lastreceiptdate), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_qtyordered), Made4Net.Shared.Util.FormatField(_qtyadjusted), Made4Net.Shared.Util.FormatField(_qtyreceived), Made4Net.Shared.Util.FormatField(_inventorystatus), _
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(pInpupUOM), Made4Net.Shared.Util.FormatField(pInputQTY), Made4Net.Shared.Util.FormatField(pInputSku))

        DataInterface.RunSQL(sql)
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InboundOrder, _consignee, _orderid, _orderline)
        _attributes.Add(oAttributeCollection)
        _attributes.Save(pUser)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.InboundLineCreated
        'em.Add("EVENT", EventType)
        'em.Add("ORDERID", _orderid)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("ORDERLINE", _orderline)
        'em.Add("USERID", pUser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundLineCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDLNINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", _orderline)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", pQty)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", pQty)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.INBOUNDLNINS)

    End Sub

    Public Sub UpdateLine(ByVal pRefOrd As String, ByVal pExpDate As DateTime, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing)
        Dim _skuCheck As String
        Dim oSku As WMS.Logic.SKU
        If Not (WMS.Logic.SKU.Exists(_consignee, pSku)) Then
            If Not (WMS.Logic.SKU.Exists(_consignee, pInputSku)) Then
                'Added for RWMS-1509 and RWMS-1502
                Dim strErrDesc As String = "Cannot create order detail.Sku " + pInputSku + " does not Exist for consignee " + _consignee
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order detail.Sku not Exist", strErrDesc)
                'Ended for RWMS-1509 and RWMS-1502
                Throw m4nEx
            Else
                oSku = New SKU(_consignee, pInputSku)
                _inputsku = pInputSku
                _skuCheck = _sku
                _sku = oSku.SKU
            End If
        Else
            oSku = New SKU(_consignee, pSku)
            _skuCheck = _sku
            _sku = pSku
        End If
        If pQty < 0 Then
            If pInpupUOM = String.Empty Or pInpupUOM Is Nothing Or pInputQTY < 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity entered for order line", "Invalid quantity entered for order line")
            Else
                If pInpupUOM <> "" And Not pInpupUOM Is Nothing Then
                    If Not Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pInpupUOM) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
                    End If
                End If
                pQty = oSku.ConvertToUnits(pInpupUOM) * pInputQTY
                _inputqty = pInputQTY
                _inputuom = pInpupUOM
            End If
        Else
            'Do nothing, leave supplied quantity as received...
        End If

        ' Block user  from  editing the line if there is already an open receipt line based on this  order line.
        If ReceiptDetail.HasOpenReceiptLineForInboundLine(_consignee, _orderid, _orderline) Then
            If Not String.Equals(Me.SKU, ReceiptDetail.GetSkuByConsigneeOrderIdAndLine(_consignee, _orderid, _orderline), StringComparison.CurrentCultureIgnoreCase) Then

                'Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Edit Line - open receipt lines exists for inbound line", "Cannot Edit Line - open receipt lines exists for inbound line")
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot change SKU, an open receipt line exists", "Cannot change SKU, an open receipt line exists")
                Throw m4nEx
            End If
        End If
        If (_qtyreceived > 0 And pSku <> _skuCheck) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot change SKU, line partially received", "Cannot change SKU, line partially received")
            Throw m4nEx
        End If
        If (pQty < _qtyreceived) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Quantity is less than quantity received", "Quantity is less than quantity received")
            Throw m4nEx
        End If

        _referenceordline = pRefOrd
        _expecteddate = pExpDate
        _sku = pSku
        _qtyadjusted = pQty
        _inventorystatus = pInventoryStatus

        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE INBOUNDORDDETAIL SET REFERENCEORDLINE = {0},EXPECTEDDATE = {1},SKU = {2},QTYADJUSTED = {3}, INVENTORYSTATUS={4}, EDITDATE = {5},EDITUSER = {6},INPUTUOM={7},INPUTQTY={8},INPUTSKU={9} Where {10}", _
            Made4Net.Shared.Util.FormatField(_referenceordline), Made4Net.Shared.Util.FormatField(_expecteddate), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_qtyadjusted), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_editdate), _
            Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(pInpupUOM), Made4Net.Shared.Util.FormatField(pInputQTY), Made4Net.Shared.Util.FormatField(pInputSku), WhereClause)
        DataInterface.RunSQL(sql)

        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InboundOrder, _consignee, _orderid, _orderline)
        _attributes.PrimaryKey1 = _consignee
        _attributes.PrimaryKey2 = _orderid
        _attributes.PrimaryKey3 = _orderline
        _attributes.Add(oAttributeCollection)
        _attributes.Save(pUser)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundLineUpdated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDLNUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", _orderline)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", pQty)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", pQty)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.INBOUNDLNUPD)
    End Sub

    Public Sub Delete(ByVal pUser As String)
        If _qtyreceived > 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot delete line, quantity received greater than zero", "Cannot delete line, quantity received greater than zero")
        End If
        _attributes.Delete()
        Dim sql As String = String.Format("delete from INBOUNDORDDETAIL where " & WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundLineDeleted)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDLNDEL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", _orderline)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", _qtyadjusted)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _qtyadjusted)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.INBOUNDLNDEL)
    End Sub
  
#End Region

End Class

#End Region


