Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "ReceiptDetail"

' <summary>
' This object represents the properties and methods of a ReceiptDetail.
' </summary>

<CLSCompliant(False)> Public Class ReceiptDetail

#Region "Variables"

#Region "Primary Keys"

    Protected _receipt As String = String.Empty
    Protected _receiptline As Int32

#End Region

#Region "Other Fields"

    Protected _consignee As String = String.Empty
    Protected _sku As String = String.Empty
    Protected _orderid As String = String.Empty
    Protected _orderline As Int32
    Protected _qtyoriginal As Decimal
    Protected _qtyexpected As Decimal
    Protected _qtyreceived As Decimal

    Protected _inputsku As String = String.Empty
    Protected _inputuom As String = String.Empty
    Protected _inputqty As Decimal
    Protected _reford As String = String.Empty
    Protected _refordline As String = String.Empty
    Protected _inventorystatus As String = String.Empty
    Protected _company As String = String.Empty
    Protected _companytype As String = String.Empty
    Protected _originalsku As String = String.Empty
    Protected _avgweight As Decimal
    Protected _avgweightuom As String = String.Empty
    Protected _receivedweight As Decimal

    Protected _documenttype As String = String.Empty
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
            Return " RECEIPT = '" & _receipt & "' And RECEIPTLINE = '" & _receiptline & "'"
        End Get
    End Property

    Public ReadOnly Property IsInboundOrderLine() As Boolean
        Get
            If IsNothing(_orderid) Then
                Return False
            End If
            If (_orderid.Trim() <> "") AndAlso (_documenttype = WMS.Lib.DOCUMENTTYPES.INBOUNDORDER OrElse _
                    _documenttype = "") Then
                Return True
            Else
                Return False
            End If
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

    Public ReadOnly Property RECEIPT() As String
        Get
            Return _receipt
        End Get
     
    End Property

    Public Property RECEIPTLINE() As Int32
        Get
            Return _receiptline
        End Get
        Set(ByVal value As Int32)
            _receiptline = value
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

    Public Property ORDERLINE() As Int32
        Get
            Return _orderline
        End Get
        Set(ByVal Value As Int32)
            _orderline = Value
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

    Public Property QTYEXPECTED() As Decimal
        Get
            Return _qtyexpected
        End Get
        Set(ByVal Value As Decimal)
            _qtyexpected = Value
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

    Public Property DOCUMENTTYPE() As String
        Get
            Return _documenttype
        End Get
        Set(ByVal Value As String)
            _documenttype = Value
        End Set
    End Property

    Public Property REFERENCEORDER() As String
        Get
            Return _reford
        End Get
        Set(ByVal Value As String)
            _reford = Value
        End Set
    End Property

    Public Property REFERENCEORDERLINE() As String
        Get
            Return _refordline
        End Get
        Set(ByVal Value As String)
            _refordline = Value
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

    Public Property COMPANY() As String
        Get
            Return _company
        End Get
        Set(ByVal Value As String)
            _company = Value
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

    Public Property ORIGINALSKU() As String
        Get
            Return _originalsku
        End Get
        Set(ByVal Value As String)
            _originalsku = Value
        End Set
    End Property

    Public ReadOnly Property OPENQTY() As Decimal
        Get
            Return _qtyexpected - _qtyreceived
        End Get
    End Property

    Public Property AVGWEIGHT() As Decimal
        Get
            Return _avgweight
        End Get
        Set(ByVal Value As Decimal)
            _avgweight = Value
        End Set
    End Property

    Public Property RECEIVEDWEIGHT() As Decimal
        Get
            Return _receivedweight
        End Get
        Set(ByVal Value As Decimal)
            _receivedweight = Value
        End Set
    End Property

    Public Property AVGWEIGHTUOM() As String
        Get
            Return _avgweightuom
        End Get
        Set(ByVal Value As String)
            _avgweightuom = Value
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
        Set(ByVal value As InventoryAttributeBase)
            _attributes = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt)
    End Sub

    Public Sub New(ByVal pRECEIPT As String, ByVal pRECEIPTLINE As Int32, Optional ByVal LoadObj As Boolean = True)
        _receipt = pRECEIPT
        _receiptline = pRECEIPTLINE
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetReceiptDetail(ByVal pRECEIPT As String, ByVal pRECEIPTLINE As Int32) As ReceiptDetail
        Return New ReceiptDetail(pRECEIPT, pRECEIPTLINE)
    End Function

    Public Shared Function HasOpenReceiptLineForInboundLine(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderline As Int32) As Boolean
        Dim sSql As String = String.Format("SELECT COUNT(1) FROM RECEIPTDETAIL rd inner join RECEIPTHEADER rh on rd.RECEIPT = rh.RECEIPT Where QTYEXPECTED - QTYRECEIVED > 0 and STATUS <> '{0}'and STATUS <> '{1}' and CONSIGNEE = '{2}' and ORDERID = '{3}' and ORDERLINE = '{4}'", _
                 WMS.Lib.Statuses.Receipt.CLOSED, WMS.Lib.Statuses.Receipt.CANCELLED, pConsignee, pOrderid, pOrderline)
        Dim iNumberOfLines As Int32 = DataInterface.ExecuteScalar(sSql)
        If iNumberOfLines <= 0 Then
            Return False
        End If
        Return True
    End Function
    Public Shared Function GetSkuByConsigneeOrderIdAndLine(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderline As Int32) As String
        Dim sSql As String = String.Format("SELECT sku FROM RECEIPTDETAIL rd inner join RECEIPTHEADER rh on rd.RECEIPT = rh.RECEIPT Where QTYEXPECTED - QTYRECEIVED > 0 and STATUS <> '{0}'and STATUS <> '{1}' and CONSIGNEE = '{2}' and ORDERID = '{3}' and ORDERLINE = '{4}'", _
                 WMS.Lib.Statuses.Receipt.CLOSED, WMS.Lib.Statuses.Receipt.CANCELLED, pConsignee, pOrderid, pOrderline)
        Dim sSku As String = DataInterface.ExecuteScalar(sSql)

        Return sSku
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM ReceiptDetail Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Receipt Line Does Not Exists", "Receipt Line Does Not Exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Public Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("RECEIPT") Then _receipt = dr.Item("RECEIPT")
        If Not dr.IsNull("RECEIPTLINE") Then _receiptline = dr.Item("RECEIPTLINE")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("QTYORIGINAL") Then _qtyoriginal = dr.Item("QTYORIGINAL")
        If Not dr.IsNull("QTYEXPECTED") Then _qtyexpected = dr.Item("QTYEXPECTED")
        If Not dr.IsNull("QTYRECEIVED") Then _qtyreceived = dr.Item("QTYRECEIVED")
        If Not dr.IsNull("INPUTQTY") Then _inputqty = dr.Item("INPUTQTY")
        If Not dr.IsNull("INPUTSKU") Then _inputsku = dr.Item("INPUTSKU")
        If Not dr.IsNull("INPUTUOM") Then _inputuom = dr.Item("INPUTUOM")
        If Not dr.IsNull("REFORD") Then _reford = dr.Item("REFORD")
        If Not dr.IsNull("REFORDLINE") Then _refordline = dr.Item("REFORDLINE")
        If Not dr.IsNull("INVENTORYSTATUS") Then _inventorystatus = dr.Item("INVENTORYSTATUS")
        If Not dr.IsNull("COMPANY") Then _company = dr.Item("COMPANY")
        If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")
        If Not dr.IsNull("originalsku") Then _originalsku = dr.Item("originalsku")
        If Not dr.IsNull("DOCUMENTTYPE") Then _documenttype = dr.Item("DOCUMENTTYPE")
        If Not dr.IsNull("AVGWEIGHT") Then _avgweight = dr.Item("AVGWEIGHT")
        If Not dr.IsNull("AVGWEIGHTUOM") Then _avgweightuom = dr.Item("AVGWEIGHTUOM")
        If Not dr.IsNull("RECEIVEDWEIGHT") Then _receivedweight = dr.Item("RECEIVEDWEIGHT")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt, _consignee, _receipt, _receiptline)
    End Sub

    Private Function getNextLine() As Int32
        Return DataInterface.ExecuteScalar("Select isnull(max(receiptline),0) + 1 from receiptdetail where receipt = '" & _receipt & "'")
    End Function

    Public Shared Function LineExist(ByVal pReceipt As String, ByVal pConsignee As String, ByVal pSku As String) As Int32
        Dim sql As String
        sql = String.Format("Select receiptline from receiptdetail where receipt = '{0}' and consignee = '{1}' and sku = '{2}'", _
                pReceipt, pConsignee, pSku)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return -1
        Else
            Return dt.Rows(0)("receiptline")
        End If
    End Function

    Public Shared Function LineExist(ByVal pReceipt As String, ByVal pReceiptLine As Int32) As Boolean
        Dim sql As String
        sql = String.Format("Select count(1) from receiptdetail where receipt = '{0}' and receiptline = {1}", _
                pReceipt, pReceiptLine)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    'Public Function Exists(ByVal pSku As String, ByVal pConsignee As String, ByVal pOrderID As String, ByVal pOrderLine As String) As Boolean
    '    Dim sql As String = String.Format("select COUNT(1) from RECEIPTDETAIL where SKU = '{0}' and CONSIGNEE = '{1}'  and ORDERID ='{2}'  and ORDERLINE='{3}'", pSku, pConsignee, pOrderID, pOrderLine)

    '    Dim Exist As Boolean
    '    If (DataInterface.ExecuteScalar(sql) > 0) Then
    '        Return True
    '    End If
    '    Return False
    'End Function
    'Public Shared Function LineExist(ByVal pReceipt As String, ByVal pReceiptLine As Int32, ByVal pSku As String) As Boolean
    '    Dim sql As String
    '    sql = String.Format("Select count(1) from receiptdetail where receipt = '{0}' and receiptline = {1} and sku = {2}", _
    '            pReceipt, pReceiptLine, pSku)
    '    Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    'End Function
    ''' <summary>
    ''' checks if there is only on occurance of sku on a receipt
    ''' if true - returns the orderline 
    ''' </summary>
    ''' <param name="pReceipt"></param>
    ''' <param name="pSku"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetOrderlineByReceiptAndSku(ByVal pReceipt As String, ByVal pSku As String) As Int32
        Dim sql As String
        sql = String.Format("Select count(1) from receiptdetail where receipt = '{0}' and sku = {1}", _
                pReceipt, pSku)
        If ((DataInterface.ExecuteScalar(sql) > 1)) Then
            'throw exception - Multiple orderlines were found for sku [#param0#] at receipt[#param1#]
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception(), "multiple ORDlns sku [#param0#] receipt [#param1#]", "multiple ORDlns sku [#param0#] receipt [#param1#]")
            m4nEx.Params.Add("sku", pSku)
            m4nEx.Params.Add("receipt", pReceipt)
            Throw m4nEx
        Else
            If ((DataInterface.ExecuteScalar(sql) < 1)) Then
                'throw exception - No orderline was found for sku [#param0#] at receipt[#param1#]
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception(), "no ORDlns sku [#param0#] receipt [#param1#]", "no ORDlns sku [#param0#] receipt [#param1#]")
                m4nEx.Params.Add("sku", pSku)
                m4nEx.Params.Add("receipt", pReceipt)
                Throw m4nEx
            End If
        End If
        sql = String.Format("Select receiptline from receiptdetail where receipt = '{0}' and sku = {1}", pReceipt, pSku)
        Return Convert.ToInt32(DataInterface.ExecuteScalar(sql))


    End Function

    Private Sub ValidateAvgWeight(ByVal oSku As WMS.Logic.SKU, ByVal pUnitsToReceivie As Decimal, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String)
        Dim valid As Boolean = True
        Dim grosweight As Decimal

        Select Case oSku.RECEIVINGWEIGHTCAPTUREMETHOD
            Case WMS.Lib.RECEIVINGWEIGHTCAPTUREMETHODS.NOCAPTURE
            Case Else
                If pAvgWeightUOM = String.Empty Then
                    grosweight = oSku.UOM(oSku.LOWESTUOM).GROSSWEIGHT
                    If (Math.Abs(pAvgWeight / pUnitsToReceivie - grosweight) > grosweight * oSku.RECEIVINGWEIGHTTOLERANCE) Then
                        valid = False
                    End If
                Else
                    grosweight = oSku.UOM(pAvgWeightUOM).GROSSWEIGHT / oSku.UOM(pAvgWeightUOM).UNITSPERLOWESTUOM
                    If (Math.Abs(pAvgWeight / oSku.UOM(pAvgWeightUOM).UNITSPERLOWESTUOM) > grosweight * oSku.RECEIVINGWEIGHTTOLERANCE) Then
                        valid = False
                    End If
                End If
        End Select

        If Not valid Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Supplied average weight is not valid", "Supplied average weight is not valid")
        End If
    End Sub

#End Region

#Region "Add/Create New"

    Public Function CreateNew(ByVal pReceipt As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pReceiptLine As Int32 = -1, Optional ByVal pDocumentType As String = "", Optional ByVal pOrderID As String = "", Optional ByVal pOrderLine As Integer = 0) As ReceiptDetail
        _receipt = pReceipt
        If pReceiptLine <= 0 Then
            _receiptline = getNextLine()
        Else
            _receiptline = pReceiptLine
        End If
        _consignee = pConsignee
        Dim oSku As SKU

        If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
            If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                Throw New ApplicationException("SKU Does not Exists")
            Else
                oSku = New SKU(pConsignee, pInputSku)
                _sku = oSku.SKU
            End If
        Else
            oSku = New SKU(pConsignee, pSku)
            _sku = pSku
        End If

        _orderid = Nothing
        _orderline = Nothing
        If pInputQTY > 0 Then
            If Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pInpupUOM) Then
                Dim qty As Decimal = oSku.ConvertToUnits(pInpupUOM) * pInputQTY
                _qtyexpected = qty
                _inputqty = pInputQTY
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
            End If
        Else
            _inputqty = pInputQTY
            _qtyexpected = pQtyExpected
        End If
        If pAvgWeight > 0 Then
            ValidateAvgWeight(oSku, _qtyexpected, pAvgWeight, pAvgWeightUOM)
        End If

        _qtyoriginal = _qtyexpected
        _qtyreceived = 0
        _reford = pRefOrd
        _refordline = pRefOrdLine
        _inventorystatus = pInventoryStatus
        _company = pCompany
        _companytype = pCompanyType
        _inputuom = pInpupUOM
        _receivedweight = pReceivedWeight
        If _receivedweight > 0 AndAlso _qtyreceived > 0 Then
            _avgweight = _receivedweight / _qtyreceived
        Else
            _avgweight = pAvgWeight
        End If
        _avgweightuom = pAvgWeightUOM
        _documenttype = pDocumentType
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt, _consignee, _receipt, _receiptline)
        _attributes.Add(oAttributeCollection)

        _orderid = pOrderID
        _orderline = pOrderLine

        Create()
        Return Me
    End Function

    Public Function CreateNew(ByVal pReceipt As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pDocumentType As String = "") As ReceiptDetail
        Dim oLine
        Select Case pDocumentType
            Case "", WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
                oLine = New InboundOrderHeader(pConsignee, pOrderId).Lines.Line(pOrderLine)
            Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                oLine = New Flowthrough(pConsignee, pOrderId).LINES.Line(pOrderLine)
        End Select

        _receipt = pReceipt
        _receiptline = getNextLine()
        _consignee = oLine.CONSIGNEE
        _sku = oLine.SKU
        _orderid = pOrderId
        _orderline = pOrderLine
        _qtyexpected = pQtyExpected
        _reford = pRefOrd
        _refordline = pRefOrdLine
        _inventorystatus = pInventoryStatus
        _company = pCompany
        _companytype = pCompanyType

        If pInputQTY > 0 AndAlso pQtyExpected < 0 Then
            _qtyexpected = pInputQTY
            _inputqty = pInputQTY
        Else
            _inputqty = pInputQTY
            _qtyexpected = pQtyExpected
        End If

        _inputuom = pInpupUOM
        _inputsku = pInputSku

        _receivedweight = pReceivedWeight
        If _receivedweight > 0 AndAlso _qtyreceived > 0 Then
            _avgweight = _receivedweight / _qtyreceived
        Else
            _avgweight = pAvgWeight
        End If
        _avgweightuom = pAvgWeightUOM

        _qtyoriginal = _qtyexpected
        _qtyreceived = 0
        _documenttype = pDocumentType
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt, _consignee, _receipt, _receiptline)
        _attributes.Add(oAttributeCollection)

        If pAvgWeight > 0 Then
            Dim oSku As New SKU(_consignee, _sku)
            ValidateAvgWeight(oSku, _qtyexpected, pAvgWeight, pAvgWeightUOM)
        End If

        Create()
        Return Me
    End Function

    Public Function CreateNew(ByVal pReceipt As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal fromOrder As Boolean, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pDocumentType As String = "") As ReceiptDetail
        Select Case pDocumentType.ToUpper
            Case "", WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
                If Not InboundOrderHeader.LineExists(pConsignee, pOrderId, pOrderLine) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Order Line Does Not Exists", "Order Line Does Not Exists")
                    Throw m4nEx
                End If
                Dim ibnoLine As InboundOrderDetail = InboundOrderDetail.GetInboundOrderDetail(pConsignee, pOrderId, pOrderLine)
                Return Me.CreateNew(pReceipt, pConsignee, pOrderId, pOrderLine, ibnoLine.QTYADJUSTED - ibnoLine.QTYRECEIVED, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pReceivedWeight, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, WMS.Lib.DOCUMENTTYPES.INBOUNDORDER)
            Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                If Not Flowthrough.LineExists(pConsignee, pOrderId, pOrderLine) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Order Line Does Not Exists", "Order Line Does Not Exists")
                    Throw m4nEx
                End If
                Dim foLine As FlowthroughDetail = FlowthroughDetail.GetFlowthroughDetail(pConsignee, pOrderId, pOrderLine)
                Return Me.CreateNew(pReceipt, pConsignee, pOrderId, pOrderLine, foLine.QTYMODIFIED, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pReceivedWeight, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pDocumentType)
        End Select

    End Function
    '------------------ New Field Add to DB DOCUMENTTYPE
    Private Function Create()
        If Not WMS.Logic.SKU.Exists(CONSIGNEE, SKU) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Consignee/Sku Does Not Exists", "Consignee/Sku Does Not Exists")
            Throw m4nEx
        End If
        _adduser = WMS.Logic.GetCurrentUser
        _edituser = WMS.Logic.GetCurrentUser
        Dim SQL As String = ""
        SQL = "INSERT INTO RECEIPTDETAIL(RECEIPT,RECEIPTLINE,CONSIGNEE,SKU,ORDERID,ORDERLINE, QTYORIGINAL, QTYEXPECTED, QTYRECEIVED, REFORD,REFORDLINE,INVENTORYSTATUS, COMPANY, COMPANYTYPE, AVGWEIGHT, AVGWEIGHTUOM, RECEIVEDWEIGHT, DOCUMENTTYPE," & _
            "ADDDATE,ADDUSER,EDITDATE,EDITUSER,INPUTQTY,INPUTSKU,INPUTUOM) Values ("
        SQL += Made4Net.Shared.Util.FormatField(_receipt) & "," & Made4Net.Shared.Util.FormatField(_receiptline) & "," & _
            Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_sku) & "," & Made4Net.Shared.Util.FormatField(_orderid) & "," & Made4Net.Shared.Util.FormatField(_orderline) & "," & _
            Made4Net.Shared.Util.FormatField(_qtyoriginal) & "," & Made4Net.Shared.Util.FormatField(_qtyexpected) & "," & Made4Net.Shared.Util.FormatField(_qtyreceived) & "," & Made4Net.Shared.Util.FormatField(_reford) & "," & Made4Net.Shared.Util.FormatField(_refordline) & "," & _
            Made4Net.Shared.Util.FormatField(_inventorystatus) & "," & Made4Net.Shared.Util.FormatField(_company) & "," & Made4Net.Shared.Util.FormatField(_companytype) & "," & Made4Net.Shared.Util.FormatField(_avgweight) & "," & Made4Net.Shared.Util.FormatField(_avgweightuom) & "," & Made4Net.Shared.Util.FormatField(_receivedweight) & "," & Made4Net.Shared.Util.FormatField(_documenttype) & "," & Made4Net.Shared.Util.FormatField(_adddate) & "," & _
            Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & "," & _
            Made4Net.Shared.Util.FormatField(_inputqty) & "," & Made4Net.Shared.Util.FormatField(_inputsku) & "," & Made4Net.Shared.Util.FormatField(_inputuom) & ")"
        DataInterface.RunSQL(SQL)

        _attributes.Save(_adduser)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptLineCreated
        'em.Add("EVENT", EventType)
        'em.Add("RECEIPT", _receipt)
        'em.Add("RECEIPTLINE", _receiptline)
        'em.Add("USERID", _adduser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptLineCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTLNINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", _qtyexpected)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _qtyexpected)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", _adduser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _adduser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _adduser)
        aq.Send(WMS.Lib.Actions.Audit.RECEIPTLNINS)
    End Function

    Public Function Update(ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pDocumentType As String = "", Optional ByVal pOrderID As String = "", Optional ByVal pOrderLine As Integer = 0)
        Dim oSku As WMS.Logic.SKU
        If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
            If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Sku does not exists, cannot update receipt line", "Sku does not exists, cannot update receipt line")
                m4nEx.Params.Add("pConsignee", pConsignee)
                m4nEx.Params.Add("pSku", pSku)
                m4nEx.Params.Add("receipt", _receipt)
                m4nEx.Params.Add("receiptline", _receiptline)
                Throw m4nEx
            Else
                oSku = New SKU(pConsignee, pInputSku)
                _sku = oSku.SKU
            End If
        Else
            oSku = New SKU(pConsignee, pSku)
            _sku = pSku
        End If

        If _qtyreceived > 0 Then
            If pConsignee.ToUpper <> _consignee Or pSku.ToUpper <> _sku Then
                'Cannot change receipt line sku
                Throw New ApplicationException(String.Format("Cannot change receipt {0} line {1} sku, quantity already been received", _receipt, _receiptline))
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot change receipt line sku", "Cannot change receipt line sku")
                m4nEx.Params.Add("receipt", _receipt)
                m4nEx.Params.Add("receiptline", _receiptline)
                Throw m4nEx
            End If
            If pQtyExpected < _qtyreceived Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Expected units is less than received units", "Expected units is less than received units")
                Throw m4nEx
            End If
        End If
        If pInputQTY > 0 Then
            If Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pInpupUOM) Then
                Dim qty As Decimal = oSku.ConvertToUnits(pInpupUOM) * pInputQTY
                _qtyexpected = qty
                _inputqty = pInputQTY
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
            End If
        Else
            _inputqty = pInputQTY
            _qtyexpected = pQtyExpected
        End If
        If pAvgWeight > 0 Then
            ValidateAvgWeight(oSku, _qtyexpected, pAvgWeight, pAvgWeightUOM)
        End If
        _consignee = pConsignee
        _sku = pSku
        _inventorystatus = pInventoryStatus
        _reford = pRefOrd
        _refordline = pRefOrdLine
        _company = pCompany
        _companytype = pCompanyType
        _inputuom = pInpupUOM
        _editdate = DateTime.Now
        _edituser = pUser
        _documenttype = pDocumentType
        _receivedweight = pReceivedWeight
        If _receivedweight > 0 AndAlso _qtyreceived > 0 Then
            _avgweight = _receivedweight / _qtyreceived
        Else
            _avgweight = pAvgWeight
        End If
        _avgweightuom = pAvgWeightUOM
        _orderid = pOrderID
        _orderline = pOrderLine

        Dim sql As String = String.Format("UPDATE RECEIPTDETAIL SET CONSIGNEE = {0}, SKU = {1},QTYEXPECTED = {2}, REFORD={3}, REFORDLINE={4}, INVENTORYSTATUS={5}, COMPANY={6}, COMPANYTYPE={7}, EDITUSER = {8}, EDITDATE = {9}, INPUTUOM ={10}, INPUTQTY ={11}, DOCUMENTTYPE={12},ORDERID={13},ORDERLINE={14},AVGWEIGHT={15}, AVGWEIGHTUOM={16},RECEIVEDWEIGHT={17} WHERE {18}", Made4Net.Shared.Util.FormatField(_consignee), _
            Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_qtyexpected), Made4Net.Shared.Util.FormatField(_reford), Made4Net.Shared.Util.FormatField(_refordline), Made4Net.Shared.Util.FormatField(_inventorystatus), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), _
            Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_inputuom), Made4Net.Shared.Util.FormatField(_inputqty), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_avgweight), Made4Net.Shared.Util.FormatField(_avgweightuom), Made4Net.Shared.Util.FormatField(_receivedweight), WhereClause)


        DataInterface.RunSQL(sql)
        Try
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt, _consignee, _receipt, _receiptline)
            _attributes.Add(oAttributeCollection)
            _attributes.Save(pUser)
        Catch ex As Exception
        End Try

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptLineUpdated
        'em.Add("EVENT", EventType)
        'em.Add("RECEIPT", _receipt)
        'em.Add("RECEIPTLINE", _receiptline)
        'em.Add("USERID", _adduser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptLineUpdated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTLNUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", _qtyexpected)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _qtyexpected)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", _adduser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _adduser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _adduser)
        aq.Send(WMS.Lib.Actions.Audit.RECEIPTLNUPD)
    End Function

    Public Sub SetInboundDocument()
        Dim sql As String = String.Format("UPDATE RECEIPTDETAIL SET ORDERID = {0}, ORDERLINE = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Function Delete()
        If _qtyreceived > 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot delete receipt line, quantity received is greater than 0", "Cannot delete receipt line, quantity received is greater than 0")
            Throw m4nEx
        End If

        'Remove att
        Try
            _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Receipt, _consignee, _receipt, _receiptline)
            _attributes.Delete()
        Catch : End Try

        'Delete line
        Dim sql As String = String.Format("DELETE from RECEIPTDETAIL WHERE {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Function

#End Region

#Region "Create Load"

    Public Function CreateLoad(ByVal pLoadId As String, ByVal pLoadUom As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUnits As Decimal, ByVal pStatus As String, ByVal pRecCode As String, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String, ByVal oLogger As LogHandler, Optional ByVal pDocumentType As String = "", Optional ByVal pIsUOMQty As Boolean = False) As Load
        Dim oSku As New SKU(_consignee, _sku)
        If Not pIsUOMQty Then
            pUnits = oSku.ConvertToUnits(pLoadUom) * pUnits
        End If

        If pUnits > (_qtyexpected * oSku.OVERRECEIVEPCT - _qtyreceived) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Receipt Line Open Quantity is less then receiving quantity", "Receipt Line Open Quantity is less then receiving quantity")
            Throw m4nEx
        End If

        Select Case pDocumentType
            Case "", WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
                If IsInboundOrderLine Then
                    Dim inbordline As New InboundOrderDetail(_consignee, _orderid, _orderline, True)
                    'If inbordline.QTYRECEIVED + pUnits > oSku.ConvertToUnits(pLoadUom) * inbordline.QTYADJUSTED * oSku.OVERRECEIVEPCT Then
                    '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Order Line Open Quantity is less then received quantity", "Order Line Open Quantity is less then received quantity")
                    '    Throw m4nEx
                    'End If
                    If inbordline.QTYRECEIVED + pUnits > inbordline.QTYADJUSTED * oSku.OVERRECEIVEPCT Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Order Line Open Quantity is less then received quantity", "Order Line Open Quantity is less then received quantity")
                        Throw m4nEx
                    End If
                End If
            Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                Dim flLine As New FlowthroughDetail(_consignee, _orderid, _orderline)
                If flLine.QTYRECEIVED + pUnits > flLine.QTYMODIFIED * oSku.OVERRECEIVEPCT Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Order Line Open Quantity is less then received quantity", "Order Line Open Quantity is less then received quantity")
                    Throw m4nEx
                End If
        End Select

        Dim oLoad As New Load
        oLoad.CreateLoad(pLoadId, _consignee, _sku, pLoadUom, pLocation, pWarehousearea, pStatus, "", pUnits, _receipt, _
            _receiptline, pRecCode, oAttributeCollection, pUser, oLogger)

        If pDocumentType = WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH Then
            oLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.PICKED, pUser)
            Dim oFLoads As New OrderLoads
            oFLoads.CreateOrderLoad(WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, _consignee, _orderid, _orderline, oLoad.LOADID, Nothing, Nothing, pUser)
        End If

        _qtyreceived += pUnits
        If Not oAttributeCollection Is Nothing AndAlso Not oAttributeCollection.Item("weight") Is Nothing AndAlso oAttributeCollection.Item("weight").ToString <> "" AndAlso oAttributeCollection.Item("weight") > 0 Then
            _receivedweight += oAttributeCollection.Item("weight")
        End If
        DataInterface.RunSQL("UPDATE RECEIPTDETAIL SET QTYRECEIVED = " & Made4Net.Shared.Util.FormatField(_qtyreceived) & ",ReceivedWeight=" & Made4Net.Shared.Util.FormatField(_receivedweight) & ",EditUser=" & Made4Net.Shared.Util.FormatField(pUser) & ",EditDate = " & Made4Net.Shared.Util.FormatField(DateTime.Now) & " Where " & WhereClause)

        Select Case pDocumentType
            Case "", WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
                If IsInboundOrderLine Then
                    Dim inbord As New InboundOrderHeader(_consignee, _orderid)
                    inbord.Receive(_orderline, pUnits, pUser)
                End If
            Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                Dim fl As New Flowthrough(_consignee, _orderid)
                fl.Receive(_orderline, pUnits, pUser)
        End Select

        Return oLoad

    End Function

    Public Sub CalculateAverageWeight()
        Dim sSql As String = String.Format("select isnull(sum(isnull(att.WEIGHT,0)) / isnull(count(il.loadid),1),0) from INVLOAD il left outer join ATTRIBUTE att on il.LOADID = att.PKEY1 and att.PKEYTYPE = 'LOAD' where il.RECEIPT = '{0}' and il.RECEIPTLINE = {1} ", _
                            _receipt, _receiptline)
        
        _avgweight = Made4Net.DataAccess.DataInterface.ExecuteScalar(sSql)
        sSql = String.Format("UPDATE RECEIPTDETAIL SET AVGWEIGHT={0}, EDITUSER = {1}, EDITDATE = {2} WHERE {3}", _
            Made4Net.Shared.Util.FormatField(_avgweight), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sSql)
    End Sub

#End Region

#Region "Substitute Sku"

    Public Sub UpdateOriginalSku(ByVal pSubSku As String, ByVal pUser As String)
        If Not (WMS.Logic.SKU.Exists(_consignee, pSubSku)) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Substitute SKU does not exists", "Substitute SKU does not exists")
        End If
        _originalsku = _sku
        _sku = pSubSku
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("update receiptdetail set originalsku = '{0}',sku = '{1}', editdate = {2}, edituser = '{3}' where {4}", _
                _originalsku, _sku, Made4Net.Shared.Util.FormatField(_editdate), _edituser, WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Cancel Receive"

    Public Sub CancelReceive(ByVal pLoadid As String, ByVal pReasonCode As String, ByVal pUser As String)

        'RWMS-2684
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2684 END

        If Not WMS.Logic.Load.Exists(pLoadid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Load Does Not exists", "Load Does Not exists")
        Else
            'RWMS-2684
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Load:{0}", pLoadid))
            End If
            'RWMS-2684 END

            Dim oLoad As New Load(pLoadid)
            Dim dLoadQty As Decimal = oLoad.UNITS
            Dim sLoadStatus As String = oLoad.STATUS
            Dim sLoadLocation As String = oLoad.LOCATION
            oLoad.CancelReceive(pReasonCode, pUser)
            _qtyreceived -= oLoad.UNITS
            'If updated qtyreceived is less than zero, set the value 0
            If (_qtyreceived < 0) Then
                _qtyreceived = 0
            End If

            'update the receipt and receiptline to current load receipt and receiptline
            _receipt = oLoad.RECEIPT
            _receiptline = oLoad.RECEIPTLINE

            DataInterface.RunSQL("UPDATE RECEIPTDETAIL SET QTYRECEIVED = " & Made4Net.Shared.Util.FormatField(_qtyreceived) & ",EditUser=" & Made4Net.Shared.Util.FormatField(pUser) & ",EditDate = " & Made4Net.Shared.Util.FormatField(DateTime.Now) & " Where " & WhereClause)
            Select Case _documenttype
                Case "", WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
                    If IsInboundOrderLine Then
                        Dim inbord As New InboundOrderHeader(_consignee, _orderid)

                        'RWMS-2684
                        If Not wmsrdtLogger Is Nothing Then
                            wmsrdtLogger.Write(String.Format(" Cancel Orderid:{0}, Orderline:{1}", _orderid, _orderline))
                            wmsrdtLogger.Write(String.Format(" Load.Units:{0}", oLoad.UNITS))
                        End If
                        'RWMS-2684 END

                        inbord.CancelReceive(_orderline, oLoad.UNITS, pUser)
                    End If
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim fl As New Flowthrough(_consignee, _orderid)
                    fl.CancelReceive(_orderline, oLoad.UNITS, pUser)
            End Select
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.UnReceiveLoad)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNRECEIVELOAD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", oLoad.CONSIGNEE)
            aq.Add("DOCUMENT", oLoad.RECEIPT)
            aq.Add("DOCUMENTLINE", oLoad.RECEIPTLINE)
            aq.Add("FROMLOAD", oLoad.LOADID)
            aq.Add("FROMLOC", sLoadLocation)
            aq.Add("FROMQTY", dLoadQty)
            aq.Add("FROMSTATUS", sLoadStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", oLoad.SKU)
            aq.Add("TOLOAD", oLoad.LOADID)
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", "")
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Add("UOM", oLoad.LOADUOM)
            aq.Send(WMS.Lib.Actions.Audit.UNRECEIVELOAD)

            'RWMS-2684
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Message sent to Audit. {0}.", WMS.Lib.Actions.Audit.UNRECEIVELOAD))
                wmsrdtLogger.Write(String.Format(" Receipt:{0}, Receiptline:{1}, Load:{2}, FromQty:{3}", oLoad.RECEIPT, oLoad.RECEIPTLINE, oLoad.LOADID, dLoadQty))
            End If
            'RWMS-2684 END

            Dim it As IInventoryTransactionQ = InventoryTransactionQ.Factory.NewInventoryTransactionQ()
            it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.UNRECEIVELOAD)
            it.Add("CONSIGNEE", oLoad.CONSIGNEE)
            it.Add("DOCUMENT", oLoad.RECEIPT)
            it.Add("LINE", oLoad.RECEIPTLINE)
            it.Add("LOADID", oLoad.LOADID)
            it.Add("UOM", oLoad.LOADUOM)
            it.Add("QTY", dLoadQty)
            it.Add("CUBE", oLoad.Volume)
            it.Add("LOADWEIGHT", oLoad.Weight)
            it.Add("AMOUNT", 0)
            it.Add("SKU", oLoad.SKU)
            it.Add("STATUS", sLoadStatus)
            it.Add("REASONCODE", pReasonCode)
            it.Add("UNITPRICE", 0)
            InventoryTransaction.CreateAttributesRecords(it, oLoad.LoadAttributes)
            it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            it.Add("ADDUSER", pUser)
            it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            it.Add("EDITUSER", pUser)
            it.Send(WMS.Lib.INVENTORY.UNRECEIVELOAD)

            'RWMS-2684
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Message sent to Inventory Trannsaction. {0}.", WMS.Lib.INVENTORY.UNRECEIVELOAD))
                wmsrdtLogger.Write(String.Format(" Receipt:{0}, Receiptline:{1}, Load:{2}, Qty:{3}, REASONCODE:{4}", oLoad.RECEIPT, oLoad.RECEIPTLINE, oLoad.LOADID, dLoadQty, pReasonCode))
            End If
            'RWMS-2684 END

        End If
    End Sub

#End Region

    Public Sub CreateASNDetails(ByVal pUser As String)
        Dim lineConsignee As New WMS.Logic.Consignee(Me.CONSIGNEE)
        If Not lineConsignee.PROMPTFORRCVQTY Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create asn details for the seleced line. It is disabled for the skus client.", "Can not create asn details for the seleced line. It is disabled for the sku's client.")
        End If
        Dim lineSKU As New WMS.Logic.SKU(Me.CONSIGNEE, Me.SKU)
        Dim quantity = Me.QTYEXPECTED - Me.QTYRECEIVED
        If quantity > 0 Then
            Dim remainder As Decimal = quantity Mod lineSKU.UNITSOFMEASURE.UOM(lineSKU.DEFAULTRECLOADUOM).UNITSPERLOWESTUOM
            Dim numOfLoads As Int32 = Math.Floor(quantity / lineSKU.UNITSOFMEASURE.UOM(lineSKU.DEFAULTRECLOADUOM).UNITSPERLOWESTUOM)
            Dim asnDet As AsnDetail = New AsnDetail()
            For i As Integer = 1 To numOfLoads
                asnDet.Create("", Me.RECEIPT, Me.RECEIPTLINE, "", lineSKU.DEFAULTRECUOM, _
                lineSKU.UNITSOFMEASURE.UOM(lineSKU.DEFAULTRECLOADUOM).UNITSPERLOWESTUOM, WMS.Logic.Load.GenerateLoadId(), Me.Attributes.Attributes, pUser)
                asnDet.PrintLabel()
            Next
            If remainder > 0 Then
                asnDet.Create("", Me.RECEIPT, Me.RECEIPTLINE, "", lineSKU.LOWESTUOM, remainder, WMS.Logic.Load.GenerateLoadId(), _
                                Me.Attributes.Attributes, pUser)
                asnDet.PrintLabel()
            End If
        End If
    End Sub

#End Region

End Class

#End Region


