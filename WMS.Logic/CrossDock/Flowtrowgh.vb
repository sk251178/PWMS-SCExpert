Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared


<CLSCompliant(False)> Public Class FlowthroughLoadCollection
    Inherits ArrayList

#Region "Variables"

    Protected _consignee As String
    Protected _flowthrough As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As OrderLoads
        Get
            Return CType(MyBase.Item(index), OrderLoads)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal LineNumber As Int32) As OrderLoads
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).ORDERLINE = LineNumber Then
                    Return (CType(MyBase.Item(i), OrderLoads))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pConsignee As String, ByVal pFlowthrough As String, Optional ByVal LoadAll As Boolean = True)
        _consignee = pConsignee
        _flowthrough = pFlowthrough
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "SELECT * FROM ORDERLOADS WHERE CONSIGNEE = '" & _consignee & "' AND ORDERID = '" & _flowthrough & "' AND DOCUMENTTYPE='" & WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New OrderLoads(dr))
        Next
    End Sub

    Public Sub New()

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

    Public Sub AddLine(ByVal pLine As Int32, ByVal pLoad As String)
        Dim oLine As OrderLoads = Line(pLine)
        If IsNothing(oLine) Then
            oLine = New OrderLoads
            oLine.CreateOrderLoad(WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, _consignee, _flowthrough, pLine, pLoad, Nothing, Nothing, WMS.Logic.Common.GetCurrentUser)
            Me.add(oLine)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot add line to Flowthrough", "Cannot add line to Flowthrough")
            m4nEx.Params.Add("pLine", pLine)
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("Flowthrough", _flowthrough)
            Throw m4nEx
        End If
    End Sub

    Public Sub CreateNewLine(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As Integer, ByVal pLoad As String)
        Me.add(New OrderLoads().CreateOrderLoad(WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, pConsignee, pFlowthrough, pFlowthroughLine, pLoad, Nothing, Nothing, WMS.Logic.Common.GetCurrentUser))
    End Sub

#End Region

End Class

<CLSCompliant(False)> Public Class FlowthroughDetail

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _flowthrough As String = String.Empty
    Protected _flowthroughline As Int32

#End Region

#Region "Other Fields"

    Protected _sku As String = String.Empty
    Protected _qtyoriginal As Decimal
    Protected _qtyreceived As Decimal
    Protected _qtymodified As Decimal

    Protected _qtypicked As Decimal
    Protected _qtystaged As Decimal
    Protected _qtypacked As Decimal
    Protected _qtyverified As Decimal
    Protected _qtyloaded As Decimal
    Protected _qtyshipped As Decimal

    Protected _inputqty As Decimal
    Protected _inputsku As String = String.Empty
    Protected _inputuom As String = String.Empty
    Protected _inventorystatus As String = String.Empty

    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

    Protected _attributes As InventoryAttributeBase

    Protected _route As String
    Protected _stopnumber As Integer

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' AND FLOWTHROUGH = '" & _flowthrough & "' AND FLOWTHROUGHLINE = '" & _flowthroughline & "'"
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

    Public Property FLOWTHROUGH() As String
        Get
            Return _flowthrough
        End Get
        Set(ByVal Value As String)
            _flowthrough = Value
        End Set
    End Property

    Public Property FLOWTHROUGHLINE() As Int32
        Get
            Return _flowthroughline
        End Get
        Set(ByVal Value As Int32)
            _flowthroughline = Value
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

    Public Property QTYORIGINAL() As Decimal
        Get
            Return _qtyoriginal
        End Get
        Set(ByVal Value As Decimal)
            _qtyoriginal = Value
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

    Public Property QTYMODIFIED() As Decimal
        Get
            Return _qtymodified
        End Get
        Set(ByVal Value As Decimal)
            _qtymodified = Value
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

    Public Property QTYSHIPPED() As Decimal
        Get
            Return _qtyshipped
        End Get
        Set(ByVal Value As Decimal)
            _qtyshipped = Value
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

    Public ReadOnly Property Attributes() As InventoryAttributeBase
        Get
            Return _attributes
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

    Public Property ROUTE() As String
        Get
            Return _route
        End Get
        Set(ByVal value As String)
            _route = value
        End Set
    End Property

    Public Property STOPNUMBER() As Integer
        Get
            Return _stopnumber
        End Get
        Set(ByVal value As Integer)
            _stopnumber = value
        End Set
    End Property

    Public ReadOnly Property ShipmentsAssignedTotalQuantity() As Decimal
        Get
            Dim sql As String = String.Format("select isnull(sum(units),0) from shipmentdetail where CONSIGNEE={0} AND ORDERID={1} AND ORDERLINE={2}", _
            Made4Net.Shared.FormatField(_consignee), Made4Net.Shared.FormatField(_flowthrough), Made4Net.Shared.FormatField(_flowthroughline))
            Return DataInterface.ExecuteScalar(sql)
        End Get
    End Property

    Public ReadOnly Property OpenQtyLeftToAssignToShipment() As Decimal
        Get
            Return QTYMODIFIED - ShipmentsAssignedTotalQuantity
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InboundOrder)
    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pFLOWTHROUGH As String, ByVal pFLOWTHROUGHLINE As Int32, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _flowthrough = pFLOWTHROUGH
        _flowthroughline = pFLOWTHROUGHLINE
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "General"

    Private Function getNextLine() As Int32
        Return DataInterface.ExecuteScalar("SELECT ISNULL(MAX(FLOWTHROUGHLINE),0) + 1 FROM FLOWTHROUGHDETAIL WHERE FLOWTHROUGH = '" & _flowthrough & "' AND CONSIGNEE='" & _consignee & "'")
    End Function

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As Int32) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '{0}' AND FLOWTHROUGH = '{1}' AND FLOWTHROUGHLINE = {2}", pConsignee, pFlowthrough, pFlowthroughLine)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetFlowthroughDetail(ByVal pCONSIGNEE As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As Int32) As FlowthroughDetail
        Return New FlowthroughDetail(pCONSIGNEE, pFlowthrough, pFlowthroughLine)
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM FLOWTHROUGHDETAIL WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

        End If
        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("FLOWTHROUGH") Then _flowthrough = dr.Item("FLOWTHROUGH")
        If Not dr.IsNull("FLOWTHROUGHLINE") Then _flowthroughline = dr.Item("FLOWTHROUGHLINE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("QTYORIGINAL") Then _qtyoriginal = dr.Item("QTYORIGINAL")
        If Not dr.IsNull("QTYMODIFIED") Then _qtymodified = dr.Item("QTYMODIFIED")
        If Not dr.IsNull("QTYRECEIVED") Then _qtyreceived = dr.Item("QTYRECEIVED")
        If Not dr.IsNull("QTYPICKED") Then _qtypicked = dr.Item("QTYPICKED")
        If Not dr.IsNull("QTYPACKED") Then _qtypacked = dr.Item("QTYPACKED")
        If Not dr.IsNull("QTYVERIFIED") Then _qtyverified = dr.Item("QTYVERIFIED")
        If Not dr.IsNull("QTYSTAGED") Then _qtystaged = dr.Item("QTYSTAGED")
        If Not dr.IsNull("QTYLOADED") Then _qtyloaded = dr.Item("QTYLOADED")
        If Not dr.IsNull("QTYSHIPPED") Then _qtyshipped = dr.Item("QTYSHIPPED")
        If Not dr.IsNull("INPUTQTY") Then _inputqty = dr.Item("INPUTQTY")
        If Not dr.IsNull("INPUTSKU") Then _inputsku = dr.Item("INPUTSKU")
        If Not dr.IsNull("INPUTUOM") Then _inputuom = dr.Item("INPUTUOM")
        If Not dr.IsNull("INVENTORYSTATUS") Then _inventorystatus = dr.Item("INVENTORYSTATUS")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        If Not dr.IsNull("ROUTE") Then _route = dr.Item("ROUTE")
        If Not dr.IsNull("STOPNUMBER") Then _stopnumber = dr.Item("STOPNUMBER")

        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.FLOWTHROUGH, _consignee, _flowthrough, _flowthroughline)
    End Sub

    Public Function GeFlowthrough() As Flowthrough
        Return New Flowthrough(_consignee, _flowthrough)
    End Function

#End Region

#Region "DB Functions"

    Public Sub Create(ByVal pFlowthrough As String, ByVal pConsignee As String, ByVal pFlowthroughLine As Int32, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0)

        If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
            If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                Throw New ApplicationException("SKU Does not Exists")
            Else
                Dim oSku As SKU = New SKU(pConsignee, pInputSku)
                _sku = oSku.SKU
            End If
        Else
            _sku = pSku
        End If

        _consignee = pConsignee
        _flowthrough = pFlowthrough
        _flowthroughline = pFlowthroughLine
        _qtymodified = pQty
        _qtyreceived = 0
        _qtyoriginal = pQty

        _qtyloaded = 0
        _qtypicked = 0
        _qtyshipped = 0
        _qtyverified = 0
        _qtystaged = 0
        _qtypacked = 0

        _inventorystatus = pInventoryStatus

        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        _route = pRoute
        _stopnumber = pStopNumber

        Dim SQL As String
        SQL = "INSERT INTO FLOWTHROUGHDETAIL(CONSIGNEE, FLOWTHROUGH, FLOWTHROUGHLINE, SKU, QTYORIGINAL, QTYRECEIVED, QTYMODIFIED,INVENTORYSTATUS, ADDUSER, ADDDATE, EDITUSER, EDITDATE,INPUTUOM,INPUTSKU,INPUTQTY,QTYPICKED,QTYSTAGED,QTYVERIFIED,QTYPACKED,QTYLOADED,QTYSHIPPED,ROUTE,STOPNUMBER)" & _
              " VALUES(" & Made4Net.Shared.Util.FormatField(_consignee) & "," & _
                          Made4Net.Shared.Util.FormatField(_flowthrough) & "," & _
                          Made4Net.Shared.Util.FormatField(_flowthroughline) & "," & _
                          Made4Net.Shared.Util.FormatField(_sku) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtyoriginal) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtyreceived) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtymodified) & "," & _
                          Made4Net.Shared.Util.FormatField(_inventorystatus) & "," & _
                          Made4Net.Shared.Util.FormatField(_adduser) & "," & _
                          Made4Net.Shared.Util.FormatField(_adddate) & "," & _
                          Made4Net.Shared.Util.FormatField(_edituser) & "," & _
                          Made4Net.Shared.Util.FormatField(_editdate) & "," & _
                          Made4Net.Shared.Util.FormatField(pInpupUOM) & "," & _
                          Made4Net.Shared.Util.FormatField(pInputSku) & "," & _
                          Made4Net.Shared.Util.FormatField(pInputQTY) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtypicked) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtystaged) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtyverified) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtypacked) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtyloaded) & "," & _
                          Made4Net.Shared.Util.FormatField(_qtyshipped) & "," & _
                          Made4Net.Shared.Util.FormatField(_route) & "," & _
                          Made4Net.Shared.Util.FormatField(_stopnumber) & _
                        ")"

        DataInterface.RunSQL(SQL)
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.FLOWTHROUGH, _consignee, _flowthrough, _flowthroughline)
        _attributes.Add(oAttributeCollection)
        _attributes.Save(pUser)

    End Sub

    Public Sub Delete(ByVal pUser As String)
        Dim sql As String = String.Format("delete from FLOWTHROUGHDETAIL where " & WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.FlowThroughLineDeleted)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.FLWTHLNDEL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _flowthrough)
        aq.Add("DOCUMENTLINE", _flowthroughline)
        aq.Add("SKU", _sku)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.INBOUNDLNDEL)
    End Sub

    Public Function InsertDetail(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0) As FlowthroughDetail
        _flowthrough = pFlowthrough
        _consignee = pConsignee
        Dim pFlowthroughLine As Int32 = getNextLine()
        If Not (WMS.Logic.SKU.Exists(pConsignee, pSku)) Then
            If Not (WMS.Logic.SKU.Exists(pConsignee, pInputSku)) Then
                Throw New ApplicationException("SKU Does not Exists")
            Else
                Dim oSku As SKU = New SKU(pConsignee, pInputSku)
                _sku = oSku.SKU
            End If
        Else
            _sku = pSku
        End If

        Create(pFlowthrough, pConsignee, pFlowthroughLine, _sku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pRoute, pStopNumber)
        Return Me
    End Function

    Public Function EditDetail(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As Int32, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0) As FlowthroughDetail
        If Not WMS.Logic.SKU.Exists(pConsignee, pSku) Then
            'Added for RWMS-1509 and RWMS-1502
            Dim strErrDesc As String = "Cannot create order detail.Sku " + pSku + " does not Exist for consignee " + pConsignee
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order detail.Sku not Exist", strErrDesc)
            'Ended for RWMS-1509 and RWMS-1502
            Throw m4nEx
        End If
        _consignee = pConsignee
        _flowthrough = pFlowthrough
        _flowthroughline = pFlowthroughLine
        _inventorystatus = pInventoryStatus
        _sku = pSku
        _qtymodified = pQty
        _editdate = DateTime.Now
        _edituser = pUser

        _route = pRoute
        _stopnumber = pStopNumber

        Dim SQL As String
        SQL = "UPDATE FLOWTHROUGHDETAIL " & _
              "SET SKU =" & Made4Net.Shared.Util.FormatField(_sku) & ", QTYMODIFIED =" & Made4Net.Shared.Util.FormatField(_qtymodified) & ", INVENTORYSTATUS =" & Made4Net.Shared.Util.FormatField(_inventorystatus) & ", ROUTE =" & Made4Net.Shared.FormatField(_route) & ", STOPNUMBER = " & Made4Net.Shared.FormatField(_stopnumber) & ", EDITUSER =" & Made4Net.Shared.Util.FormatField(_edituser) & ", EDITDATE =" & Made4Net.Shared.Util.FormatField(_editdate) & _
              " WHERE CONSIGNEE =" & Made4Net.Shared.Util.FormatField(_consignee) & " AND FLOWTHROUGH =" & Made4Net.Shared.Util.FormatField(_flowthrough) & " AND FLOWTHROUGHLINE =" & Made4Net.Shared.Util.FormatField(_flowthroughline) & ""
        DataInterface.RunSQL(SQL)

        'Me.Attributes.SetAttributes(oAttributeCollection, _flowthrough)
        'Me.Attributes.Save(pUser)
        _attributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.FLOWTHROUGH, _consignee, _flowthrough, _flowthroughline)
        _attributes.Add(oAttributeCollection)
        _attributes.Save(pUser)


        Return Me
    End Function

#End Region

#Region "Recieve Flowthrough Line"

    Public Sub Receive(ByVal pQty As Decimal, ByVal pUser As String)

        Dim uColl As New System.Collections.Specialized.NameValueCollection
        _qtyreceived = _qtyreceived + pQty
        _qtypicked = _qtypicked + pQty
        _editdate = DateTime.Now
        _edituser = pUser

        uColl.Add("QTYRECEIVED", Made4Net.Shared.Util.FormatField(_qtyreceived))
        uColl.Add("QTYPICKED", Made4Net.Shared.Util.FormatField(_qtypicked))
        uColl.Add("EDITDATE", Made4Net.Shared.Util.FormatField(_editdate))
        uColl.Add("EDITUSER", Made4Net.Shared.Util.FormatField(_edituser))

        Update(uColl)
    End Sub

    Public Sub CancelReceive(ByVal pQty As Decimal, ByVal pUser As String)

        Dim uColl As New System.Collections.Specialized.NameValueCollection
        _qtyreceived = _qtyreceived - pQty
        _qtypicked = _qtypicked - pQty
        _editdate = DateTime.Now
        _edituser = pUser

        uColl.Add("QTYRECEIVED", Made4Net.Shared.Util.FormatField(_qtyreceived))
        uColl.Add("QTYPICKED", Made4Net.Shared.Util.FormatField(_qtypicked))
        uColl.Add("EDITDATE", Made4Net.Shared.Util.FormatField(_editdate))
        uColl.Add("EDITUSER", Made4Net.Shared.Util.FormatField(_edituser))

        Update(uColl)
    End Sub

    Public Sub Update(ByVal updateFields As System.Collections.Specialized.NameValueCollection)
        Dim SqlUpdateStatement As String = Made4Net.DataAccess.SQLStatement.CreateUpdateStatement("FlowthroughDetail", updateFields, WhereClause)
        DataInterface.RunSQL(SqlUpdateStatement)
    End Sub

#End Region

#Region "Stage/Pack/Load/Ship"

    Public Sub Ship(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String, Optional ByVal oFlowthrough As Flowthrough = Nothing, Optional ByVal oLogger As LogHandler = Nothing)
        Dim sql As String
        _qtyshipped += pUnits
        UpdateLastActivityQuantities(pPreviousActivty, pUnits)
        _edituser = puser
        _editdate = DateTime.Now
        sql = String.Format("UPDATE FLOWTHROUGHDETAIL SET QTYPICKED = {0},QTYSTAGED = {1}, QTYPACKED = {2},QTYLOADED = {3},QTYSHIPPED = {4},QTYVERIFIED = {5},EDITDATE = {6},EDITUSER = {7} WHERE {8}", Made4Net.Shared.Util.FormatField(_qtypicked), _
            Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_qtyloaded), Made4Net.Shared.Util.FormatField(_qtyshipped), Made4Net.Shared.Util.FormatField(_qtyverified), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        'RWMS-726
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Updated flowthroughdetail shipment status {0}", sql))
            oLogger.writeSeperator(" ", 100)
        End If


        Dim oOrdHeader As Flowthrough
        'And Update the Header if needed
        If oFlowthrough Is Nothing Then
            oOrdHeader = GeFlowthrough()
        Else
            oOrdHeader = oFlowthrough
        End If
        oOrdHeader.LINES.Line(_flowthroughline).QTYSHIPPED = _qtyshipped
        For Each oOrdDet As FlowthroughDetail In oOrdHeader.LINES
            If Not oOrdDet.FULLYSHIPPED Then
                Return
            End If
        Next
        oOrdHeader.UpdateShipStatus(puser, oLogger)
    End Sub

    Public Sub Pack(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String)
        Dim sql As String
        _qtypacked += pUnits '_qtypicked + _qtystaged
        UpdateLastActivityQuantities(pPreviousActivty, pUnits)
        _edituser = puser
        _editdate = DateTime.Now
        sql = String.Format("UPDATE FLOWTHROUGHDETAIL SET QTYPICKED = {0},QTYSTAGED = {1},QTYPACKED = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_qtypicked), _
            Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        'And Update the Header if needed
        Dim oOrdHeader As Flowthrough = GeFlowthrough()
        For Each oOrdDet As FlowthroughDetail In oOrdHeader.LINES
            If Not oOrdDet.FULLYPACKED Then
                Return
            End If
        Next
        oOrdHeader.Pack(puser)
    End Sub

    Public Sub UnPack(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String)
        Dim sql As String
        _qtypacked -= pUnits
        If pPreviousActivty = WMS.Lib.LoadActivityTypes.STAGING Then
            _qtystaged += pUnits
        Else
            _qtypicked += pUnits
        End If
        _edituser = puser
        _editdate = DateTime.Now
        sql = String.Format("UPDATE FLOWTHROUGHDETAIL SET QTYPICKED = {0},QTYSTAGED = {1},QTYPACKED = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_qtypicked), _
            Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Stage(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String)
        Dim sql As String
        If _qtystaged + pUnits <= _qtymodified Then _qtystaged += pUnits Else _qtystaged = _qtymodified
        UpdateLastActivityQuantities(pPreviousActivty, pUnits)
        'If _qtypicked - pUnits > 0 Then _qtypicked -= pUnits Else _qtypicked = 0
        _edituser = puser
        _editdate = DateTime.Now
        sql = String.Format("UPDATE FLOWTHROUGHDETAIL SET QTYSTAGED = {0},QTYPICKED = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_qtystaged), _
            Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        'And Update the Header if needed
        Dim oOrdHeader As Flowthrough = GeFlowthrough()
        For Each oOrdDet As FlowthroughDetail In oOrdHeader.LINES
            If Not oOrdDet.FULLYSTAGED Then
                Return
            End If
        Next
        oOrdHeader.Stage(puser)
    End Sub

    Public Sub Load(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal puser As String)
        Dim sql As String
        If _qtyloaded + pUnits <= _qtymodified Then _qtyloaded += pUnits Else _qtyloaded = _qtymodified
        UpdateLastActivityQuantities(pPreviousActivty, pUnits)
        _edituser = puser
        _editdate = DateTime.Now
        sql = String.Format("UPDATE FLOWTHROUGHDETAIL SET QTYLOADED = {0},QTYPICKED = {1}, QTYSTAGED = {2},QTYPACKED = {3}, QTYVERIFIED = {4}, EDITDATE = {5},EDITUSER = {6} WHERE {7}", Made4Net.Shared.Util.FormatField(_qtyloaded), _
            Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_qtyverified), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        'And Update the Header if needed
        Dim oOrdHeader As Flowthrough = GeFlowthrough()
        For Each oOrdDet As FlowthroughDetail In oOrdHeader.LINES
            If Not oOrdDet.FULLYLOADED Then
                If oOrdHeader.STATUS <> WMS.Lib.Statuses.Flowthrough.LOADING Then
                    oOrdHeader.SetStatus(WMS.Lib.Statuses.Flowthrough.LOADING, puser)
                End If
                Return
            End If
        Next
        oOrdHeader.UpdateLoadStatus(puser)
    End Sub

    Public Sub Verify(ByVal pUnits As Decimal, ByVal pPreviousActivty As String, ByVal pUser As String)
        Dim sql As String
        If _qtyverified + pUnits <= _qtymodified Then _qtyverified += pUnits Else _qtyverified = _qtymodified
        UpdateLastActivityQuantities(pPreviousActivty, pUnits)
        _editdate = DateTime.Now
        _edituser = pUser

        sql = String.Format("UPDATE FLOWTHROUGHDETAIL SET QTYLOADED = {0},QTYPICKED = {1}, QTYSTAGED = {2},QTYPACKED = {3}, QTYVERIFIED={4}, EDITDATE = {5},EDITUSER = {6} WHERE {7}", Made4Net.Shared.Util.FormatField(_qtyloaded), _
            Made4Net.Shared.Util.FormatField(_qtypicked), Made4Net.Shared.Util.FormatField(_qtystaged), Made4Net.Shared.Util.FormatField(_qtypacked), Made4Net.Shared.Util.FormatField(_qtyverified), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)


        Dim oOrdHeader As Flowthrough = GeFlowthrough()
        For Each oOrdDet As FlowthroughDetail In oOrdHeader.LINES
            If Not oOrdDet.FULLYVERIFIED Then
                Return
            End If
        Next
        oOrdHeader.Verify(pUser)

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

#End Region

#End Region

End Class

<CLSCompliant(False)> Public Class FlowthroughDetailCollection
    Inherits ArrayList

#Region "Variables"

    Protected _consignee As String
    Protected _flowthrough As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As FlowthroughDetail
        Get
            Return CType(MyBase.Item(index), FlowthroughDetail)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal LineNumber As Int32) As FlowthroughDetail
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).FLOWTHROUGHLINE = LineNumber Then
                    Return (CType(MyBase.Item(i), FlowthroughDetail))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pConsignee As String, ByVal pFlowthrough As String, Optional ByVal LoadAll As Boolean = True)
        _consignee = pConsignee
        _flowthrough = pFlowthrough
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "SELECT * FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '" & _consignee & "' AND FLOWTHROUGH = '" & _flowthrough & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New FlowthroughDetail(dr))
        Next
    End Sub

    Public Sub New()

    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As FlowthroughDetail) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As FlowthroughDetail)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As FlowthroughDetail)
        MyBase.Remove(pObject)
    End Sub

    Public Sub AddLine(ByVal pFlowthrough As String, ByVal pConsignee As String, ByVal pLine As Int32, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0)
        Dim oLine As FlowthroughDetail = Line(pLine)
        If IsNothing(oLine) Then
            oLine = New FlowthroughDetail
            oLine.Create(pFlowthrough, pConsignee, pLine, pSku, pUnits, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
            Me.add(oLine)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot add line to Flowthrough", "Cannot add line to Flowthrough")
            m4nEx.Params.Add("pLine", pLine)
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("Flowthrough", _flowthrough)
            Throw m4nEx
        End If
    End Sub

    Public Sub AddLine(ByVal pFlowthrough As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing)
        Dim pLineNumber As Int32 = System.Convert.ToInt32(DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(MAX(FLOWTHROUGHLINE),0) + 1 FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '{0}' and FLOWTHROUGH = '{1}'", _consignee, _flowthrough)))
        AddLine(pFlowthrough, pConsignee, pLineNumber, pSku, pUnits, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
    End Sub

    Public Sub CreateNewLine(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pSku As String, _
             ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0)
        Me.add(New FlowthroughDetail().InsertDetail(pConsignee, pFlowthrough, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pRoute, pStopNumber))
    End Sub

    Public Sub EditLine(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As String, ByVal pSku As String, _
            ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0)
        Me.add(New FlowthroughDetail().EditDetail(pConsignee, pFlowthrough, pFlowthroughLine, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pRoute, pStopNumber))
    End Sub

#End Region

End Class

<CLSCompliant(False)> Public Class Flowthrough

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _flowthrough As String = String.Empty

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
    Protected _createdate As DateTime
    Protected _scheduledarrivaldate As DateTime
    Protected _requesteddeliverydate As DateTime
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
    Protected _pod As String = String.Empty
    Protected _orderpriority As String = String.Empty
    Protected _shipto As String = String.Empty
    Protected _receivedfrom As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

    Protected _exptecteddate As DateTime
    Protected _appointmentid As String = String.Empty
    Protected _carrierid As String = String.Empty

#End Region

#Region "Collections"

    Protected _Lines As FlowthroughDetailCollection
    Protected _Loads As FlowthroughLoadCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And FLOWTHROUGH = '" & _flowthrough & "'"
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

    Public Property FLOWTHROUGH() As String
        Get
            Return _flowthrough
        End Get
        Set(ByVal Value As String)
            _flowthrough = Value
        End Set
    End Property

    Public ReadOnly Property LINES() As FlowthroughDetailCollection
        Get
            Return _Lines
        End Get
    End Property

    Public ReadOnly Property LOADS() As FlowthroughLoadCollection
        Get
            Return _Loads
        End Get
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
            Return _scheduledarrivaldate
        End Get
        Set(ByVal Value As DateTime)
            _scheduledarrivaldate = Value
        End Set
    End Property

    Public Property REQUESTEDDELIVERYDATE() As DateTime
        Get
            Return _requesteddeliverydate
        End Get
        Set(ByVal Value As DateTime)
            _requesteddeliverydate = Value
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
            If _status = WMS.Lib.Statuses.Flowthrough.RECEIVED Or _status = WMS.Lib.Statuses.Flowthrough.LOADED Or _status = WMS.Lib.Statuses.Flowthrough.VERIFIED Or _status = WMS.Lib.Statuses.Flowthrough.SHIPPING Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CANCREATERECEIPT() As Boolean
        Get
            'Commnented and Added for PWMS-599
            'If _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW Then
            If _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW Or _status = WMS.Lib.Statuses.Flowthrough.RECEIVING Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property CANCANCEL() As Boolean
        Get
            If _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public Property EXPECTEDDATE() As DateTime
        Get
            Return _exptecteddate
        End Get
        Set(ByVal value As DateTime)
            _exptecteddate = value
        End Set
    End Property

    Public Property APPOINTMENTID() As String
        Get
            Return _appointmentid
        End Get
        Set(ByVal value As String)
            _appointmentid = value
        End Set
    End Property

    Public Property CARRIERID() As String
        Get
            Return _carrierid
        End Get
        Set(ByVal value As String)
            _carrierid = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _Lines = New FlowthroughDetailCollection()
        _Loads = New FlowthroughLoadCollection()
    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pFLOWTHROUGH As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _flowthrough = pFLOWTHROUGH
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
                CreateNew(dr("CONSIGNEE"), dr("FLOWTHROUGH"), Convert.ReplaceDBNull(dr("ORDERTYPE")), Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                          dr("SOURCECOMPANY"), dr("SOURCECOMPANYTYPE"), dr("TARGETCOMPANY"), dr("TARGETCOMPANYTYPE"), Nothing, _
                          Convert.ReplaceDBNull(dr("NOTES")), WMS.Logic.Warehouse.CurrentWarehouseArea, DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                          Convert.ReplaceDBNull(dr("REQUESTEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), Nothing, _
                          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("SHIPMENT")), Nothing, Convert.ReplaceDBNull(dr("LOADINGSEQ")), Nothing, _
                          Nothing, Nothing, Nothing, Nothing, Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                          DateTime.Now, Common.GetCurrentUser, DateTime.Now, Common.GetCurrentUser)
            Case "UPDATE"
                _consignee = dr("CONSIGNEE")
                _flowthrough = dr("FLOWTHROUGH")
                'Edit(dr("CONSIGNEE"), dr("FLOWTHROUGH"), Nothing, Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                '          dr("SOURCECOMPANY"), dr("SOURCECOMPANYTYPE"), dr("TARGETCOMPANY"), dr("TARGETCOMPANYTYPE"), Convert.ReplaceDBNull(dr("STATUS")), Convert.ReplaceDBNull(dr("STATUSDATE")), _
                '          Convert.ReplaceDBNull(dr("NOTES")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                '          Convert.ReplaceDBNull(dr("requesteddeliverydate")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SHIPPEDDATE")), _
                '          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("SHIPMENT")), Nothing, Convert.ReplaceDBNull(dr("LOADINGSEQ")), Nothing, _
                '          Nothing, Nothing, Nothing, Nothing, Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                '          DateTime.Now, Common.GetCurrentUser)

                Edit(dr("CONSIGNEE"), dr("FLOWTHROUGH"), _
          Nothing, Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                      dr("SOURCECOMPANY"), dr("SOURCECOMPANYTYPE"), _
          dr("TARGETCOMPANY"), dr("TARGETCOMPANYTYPE"), _
          Convert.ReplaceDBNull(dr("STATUS")), Convert.ReplaceDBNull(dr("STATUSDATE")), _
                      Convert.ReplaceDBNull(dr("NOTES")), Me.CREATEDATE, _
          DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                      Convert.ReplaceDBNull(dr("requesteddeliverydate")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), _
          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), _
          Convert.ReplaceDBNull(dr("SHIPMENT")), Nothing, _
          Convert.ReplaceDBNull(dr("LOADINGSEQ")), Nothing, _
                      Nothing, Nothing, _
          Nothing, Nothing, _
          Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                      Convert.ReplaceDBNull(dr("EXPECTEDDATE")), Convert.ReplaceDBNull(dr("APPOINTMENTID")), _
                      Convert.ReplaceDBNull(dr("CARRIERID")), DateTime.Now, Common.GetCurrentUser)
            Case "CANCEL"
                For Each dr In ds.Tables(0).Rows
                    _consignee = dr("CONSIGNEE")
                    _flowthrough = dr("FLOWTHROUGH")
                    Load()
                    If CANCANCEL Then
                        _status = WMS.Lib.Statuses.Flowthrough.CANCELED
                        Save(Common.GetCurrentUser)
                    Else
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cant cancel Flowthrough. Flowthrough status incorrect.", "Cant cancel Flowthrough. Flowthrough status incorrect.")
                        Throw m4nEx
                    End If
                Next
            Case "CREATERECEIPT"
                Dim oRct As String
                'Added For RWMS-484   
                Dim IsExport As Boolean
                Dim FlowthroughExported As Boolean
                Dim Msg As String = "Some flowthrough lines selected have already been exported. "
                'Ended For RWMS-484 
                For Each dr In ds.Tables(0).Rows
                    _consignee = dr("CONSIGNEE")
                    _flowthrough = dr("FLOWTHROUGH")
                    Load()
                    If CANCREATERECEIPT Then
                        'oRct = oRct & CreateReceipt(Common.GetCurrentUser) & ","
                        'Added For RWMS-484   
                        oRct = oRct & CreateReceipt(Common.GetCurrentUser, IsExport, FlowthroughExported) & ","
                        'Ended For RWMS-484
                        Message = "RECEIPT CREATED : "
                        '_status = WMS.Lib.Statuses.Flowthrough.RECEIVING
                        'Save(Common.GetCurrentUser)
                    Else
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cant create Receipt. Flowthrough status incorrect.", "Cant create Receipt. Flowthrough status incorrect.")
                        Throw m4nEx
                    End If
                Next
                'Added For RWMS-484   
                If IsExport = False Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Flowthrough lines selected have already been exported.", "Flowthrough lines selected have already been exported")
                    Throw m4nEx
                End If

                If oRct.Length > 0 Then
                    If FlowthroughExported Then
                        Message = Msg & Message & oRct.Trim(",")
                    Else
                        Message = Message & oRct.Trim(",")
                    End If
                End If
                'Ended For RWMS-484  

            Case "SHIP"
                For Each dr In ds.Tables(0).Rows
                    _consignee = dr("CONSIGNEE")
                    _flowthrough = dr("FLOWTHROUGH")
                    Load()
                    Dim units As Decimal
                    Dim oAttCol As AttributesCollection
                    If IsDBNull(dr("SKU")) Then
                        oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                    Else
                        oAttCol = SkuClass.ExtractLoadAttributes(dr)
                    End If
                    AddFlowthroughLine(_consignee, _flowthrough, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKU")), dr("QTYORIGINAL"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser(), oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTQTY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTSKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STOPNUMBER")))

                    Ship(Common.GetCurrentUser)
                Next
            Case "PRINTSH"
                Dim flt As Flowthrough
                For Each dr In ds.Tables(0).Rows
                    flt = New Flowthrough(dr("CONSIGNEE"), dr("FLOWTHROUGH"))
                    flt.PrintShippingManifest(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next
                'Uncommented for PWMS-745
                'Added For RWMS-484   
            Case "INSERTLINE"
                _consignee = dr("CONSIGNEE")
                _flowthrough = dr("FLOWTHROUGH")
                Load()

                Dim units As Decimal
                Dim oAttCol As AttributesCollection
                If IsDBNull(dr("SKU")) Then
                    oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                Else
                    oAttCol = SkuClass.ExtractLoadAttributes(dr)
                End If
                'Added PWMS-768 ReplaceDBNull to dr("QTYORIGINAL") 
                AddFlowthroughLine(_consignee, _flowthrough, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYORIGINAL")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser(), oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTQTY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTSKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STOPNUMBER")))
                'Ended For RWMS-484  
                'End Uncommented for PWMS-745
            Case "EDITLINE"
                'Added For RWMS-484   
                '_consignee = dr("CONSIGNEE")
                '_flowthrough = dr("FLOWTHROUGH")
                'Load()
                'Dim oAttCol As AttributesCollection
                'If IsDBNull(dr("SKU")) Then
                '    oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                'Else
                '    oAttCol = SkuClass.ExtractLoadAttributes(dr)
                'End If
                'EditFlowthroughLine(_consignee, _flowthrough, dr("FLOWTHROUGHLINE"), dr("SKU"), dr("QTYMODIFIED"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser(), oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STOPNUMBER")))
                _consignee = dr("CONSIGNEE")
                _flowthrough = dr("FLOWTHROUGH")
                Load()
                Dim oAttCol As AttributesCollection
                If IsDBNull(dr("SKU")) Then
                    oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                Else
                    oAttCol = SkuClass.ExtractLoadAttributes(dr)
                End If
                EditFlowthroughLine(_consignee, _flowthrough, dr("FLOWTHROUGHLINE"), dr("SKU"), dr("QTYMODIFIED"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser(), oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STOPNUMBER")))
                'Ended For RWMS-484  
            Case "DELETELINE"
                'Added For RWMS-484   
                '_consignee = dr("CONSIGNEE")
                '_flowthrough = dr("FLOWTHROUGH")
                'Load()
                'DeleteFlowthroughLine(_consignee, _flowthrough, dr("FLOWTHROUGHLINE"))
                _consignee = dr("CONSIGNEE")
                _flowthrough = dr("FLOWTHROUGH")
                Load()
                DeleteFlowthroughLine(_consignee, _flowthrough, dr("FLOWTHROUGHLINE"))
                'Ended For RWMS-484  
            Case "PRINTLABELS"
                'Added For RWMS-484   
                'If ds.Tables(0).Rows.Count > 0 Then
                '    _consignee = dr("CONSIGNEE")
                '    _flowthrough = ds.Tables(0).Rows(0)("FLOWTHROUGH")
                '    Load()
                '    PrintFlowthroughLabels("")
                'End If
                If ds.Tables(0).Rows.Count > 0 Then
                    _consignee = dr("CONSIGNEE")
                    _flowthrough = ds.Tables(0).Rows(0)("FLOWTHROUGH")
                    Load()
                    PrintFlowthroughLabels("")
                End If
                'Ended For RWMS-484  
        End Select

    End Sub

#End Region

#Region "Methods"

#Region "General"

    Public Shared Function Exists(ByVal pCONSIGNEE As String, ByVal pFLOWTHROUGH As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM FLOWTHROUGHHEADER WHERE CONSIGNEE = '{0}' AND FLOWTHROUGH = '{1}'", pCONSIGNEE, pFLOWTHROUGH)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM FLOWTHROUGHHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count <> 0 Then
            dr = dt.Rows(0)
            Load(dr)
        End If
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("FLOWTHROUGH") Then _flowthrough = dr.Item("FLOWTHROUGH")
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
        If Not dr.IsNull("SCHEDULEDARRIVALDATE") Then _scheduledarrivaldate = dr.Item("SCHEDULEDARRIVALDATE")
        If Not dr.IsNull("REQUESTEDDELIVERYDATE") Then _requesteddeliverydate = dr.Item("REQUESTEDDELIVERYDATE")
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
        If Not dr.IsNull("SHIPTO") Then _shipto = dr.Item("SHIPTO")
        If Not dr.IsNull("RECEIVEDFROM") Then _receivedfrom = dr.Item("RECEIVEDFROM")

        If Not dr.IsNull("EXPECTEDDATE") Then _exptecteddate = dr.Item("EXPECTEDDATE")
        If Not dr.IsNull("APPOINTMENTID") Then _appointmentid = dr.Item("APPOINTMENTID")
        If Not dr.IsNull("CARRIERID") Then _carrierid = dr.Item("CARRIERID")

        _Lines = New FlowthroughDetailCollection(_consignee, _flowthrough)
        _Loads = New FlowthroughLoadCollection(_consignee, _flowthrough)

    End Sub

    Public Shared Function getFlowthroughIDbyRefOrd(ByVal pCONSIGNEE As String, ByVal pREFERENCEORD As String) As String
        Dim sql As String = String.Format("SELECT FLOWTHROUGH FROM FLOWTHROUGHHEADER WHERE CONSIGNEE = '{0}' AND REFERENCEORD = '{1}'", pCONSIGNEE, pREFERENCEORD)
        Return System.Convert.ToString(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "DB Functions"

    Public Sub CreateNew(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pOrdertype As String, ByVal pReferenceord As String, _
                         ByVal pSourcecompany As String, ByVal pSourcecompanytype As String, ByVal pTargetcompany As String, ByVal pTargetcompanytype As String, _
                         ByVal pStatusdate As DateTime, ByVal pNotes As String, ByVal pStagingWarehousearea As String, _
                         ByVal pCreatedate As DateTime, ByVal pScheduledarrivaldate As DateTime, ByVal pRequesteddeliverydate As DateTime, _
                         ByVal pScheduleddeliverydate As DateTime, ByVal pShippeddate As DateTime, ByVal pStaginglane As String, _
                         ByVal pShipment As String, ByVal pStopnumber As String, ByVal pLoadingseq As String, ByVal pRoutingset As String, _
                         ByVal pRoute As String, ByVal pDeliverystatus As String, ByVal pPod As String, ByVal pOrderpriority As String, _
                         ByVal pShipTo As String, ByVal pReceivedFrom As String, ByVal pAdddate As DateTime, ByVal pAdduser As String, ByVal pEditdate As DateTime, ByVal pEdituser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        exist = WMS.Logic.Consignee.Exists(pConsignee)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        Else
            _consignee = pConsignee
        End If

        exist = WMS.Logic.Flowthrough.Exists(pConsignee, pFlowthrough)
        If exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.OrderId already Exist", "Can't create order.OrderId already Exist")
            Throw m4nEx
        Else
            _flowthrough = pFlowthrough
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

        If pStaginglane = "" And pStagingWarehousearea = "" Then
            _staginglane = pStaginglane
            _stagingwarehousearea = pStagingWarehousearea
        Else
            exist = WMS.Logic.Location.Exists(pStaginglane, pStagingWarehousearea)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            Else
                _staginglane = pStaginglane
                _stagingwarehousearea = pStagingWarehousearea
            End If
        End If

        _referenceord = pReferenceord
        _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW
        _statusdate = pStatusdate
        _notes = pNotes
        _createdate = DateTime.Now()
        _scheduledarrivaldate = pScheduledarrivaldate
        _requesteddeliverydate = pRequesteddeliverydate
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

        _adddate = DateTime.Now
        _adduser = pAdduser
        _editdate = DateTime.Now
        _edituser = pEdituser

        SQL = "INSERT INTO FLOWTHROUGHHEADER(CONSIGNEE, FLOWTHROUGH, ORDERTYPE, REFERENCEORD, SOURCECOMPANY, SOURCECOMPANYTYPE, TARGETCOMPANY, " & _
                                            "TARGETCOMPANYTYPE, STATUS, STATUSDATE, NOTES, CREATEDATE, SCHEDULEDARRIVALDATE, REQUESTEDDELIVERYDATE, " & _
                                            "SCHEDULEDDELIVERYDATE, SHIPPEDDATE, STAGINGLANE, STAGINGWAREHOUSEAREA, SHIPMENT, STOPNUMBER, LOADINGSEQ, ROUTINGSET, ROUTE, DELIVERYSTATUS, " & _
                                            "POD, ORDERPRIORITY, RECEIVEDFROM, SHIPTO, ADDDATE, ADDUSER, EDITDATE, EDITUSER) "
        SQL += "VALUES(" & Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_flowthrough) & "," & Made4Net.Shared.Util.FormatField(_ordertype) & "," & Made4Net.Shared.Util.FormatField(_referenceord) & "," & Made4Net.Shared.Util.FormatField(_sourcecompany) & "," & Made4Net.Shared.Util.FormatField(_sourcecompanytype) & "," & Made4Net.Shared.Util.FormatField(_targetcompany) & "," & _
                           Made4Net.Shared.Util.FormatField(_targetcompanytype) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_statusdate) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & Made4Net.Shared.Util.FormatField(_createdate) & "," & Made4Net.Shared.Util.FormatField(_scheduledarrivaldate) & "," & Made4Net.Shared.Util.FormatField(_requesteddeliverydate) & "," & _
                           Made4Net.Shared.Util.FormatField(_scheduleddeliverydate) & "," & Made4Net.Shared.Util.FormatField(_shippeddate) & "," & Made4Net.Shared.Util.FormatField(_staginglane) & "," & Made4Net.Shared.Util.FormatField(_stagingwarehousearea) & "," & Made4Net.Shared.Util.FormatField(_shipment) & "," & Made4Net.Shared.Util.FormatField(_stopnumber) & "," & Made4Net.Shared.Util.FormatField(_loadingseq) & "," & Made4Net.Shared.Util.FormatField(_routingset) & "," & Made4Net.Shared.Util.FormatField(_route) & "," & Made4Net.Shared.Util.FormatField(_deliverystatus) & "," & _
                           Made4Net.Shared.Util.FormatField(_pod) & "," & Made4Net.Shared.Util.FormatField(_orderpriority) & "," & Made4Net.Shared.Util.FormatField(_receivedfrom) & "," & Made4Net.Shared.Util.FormatField(_shipto) & "," & Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

        DataInterface.RunSQL(SQL)

    End Sub

    Public Sub Save(ByVal pUser As String)

        If Not WMS.Logic.Consignee.Exists(_consignee) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        End If

        If Not WMS.Logic.Company.Exists(_consignee, _sourcecompany, _sourcecompanytype) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
            Throw m4nEx
        End If

        If Not WMS.Logic.Company.Exists(_consignee, _targetcompany, _targetcompanytype) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        End If

        Dim exist As Boolean = True

        exist = WMS.Logic.Company.Exists(_consignee, _sourcecompany, _sourcecompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
            Throw m4nEx
        Else
            If _receivedfrom.Length = 0 Then
                Dim oComp As New Company(_consignee, _sourcecompany, _sourcecompanytype)
                _receivedfrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(_receivedfrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
            End If
        End If

        exist = WMS.Logic.Company.Exists(_consignee, _targetcompany, _targetcompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If _shipto.Length = 0 Then
                Dim oComp As New Company(_consignee, _targetcompany, _targetcompanytype)
                _shipto = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(_shipto) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
            End If
        End If


        Dim oContact As WMS.Logic.Contact
        If WMS.Logic.Contact.Exists(_shipto) Then
            oContact = New WMS.Logic.Contact(_shipto)
        End If
        If _staginglane = "" Then
            'Try to set the SL according to the contact SL
            If Not oContact Is Nothing Then
                If oContact.STAGINGLANE <> String.Empty Then
                    _staginglane = oContact.STAGINGLANE
                End If
            End If
        Else
            exist = WMS.Logic.Location.Exists(_staginglane, _stagingwarehousearea)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            End If
        End If


        Dim SQL As String
        If Not Exists(_consignee, _flowthrough) Then
            _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW
            _createdate = DateTime.Now
            _editdate = DateTime.Now
            _edituser = pUser
            _adduser = pUser
            _adddate = DateTime.Now
            _statusdate = DateTime.Now
            SQL = "INSERT INTO FLOWTHROUGHHEADER(CONSIGNEE, FLOWTHROUGH, ORDERTYPE, REFERENCEORD, SOURCECOMPANY, SOURCECOMPANYTYPE, TARGETCOMPANY, " & _
                                                "TARGETCOMPANYTYPE, STATUS, STATUSDATE, NOTES, CREATEDATE, SCHEDULEDARRIVALDATE, REQUESTEDDELIVERYDATE, " & _
                                                "SCHEDULEDDELIVERYDATE, SHIPPEDDATE, STAGINGLANE, STAGINGWAREHOUSEAREA, SHIPMENT, STOPNUMBER, LOADINGSEQ, ROUTINGSET, ROUTE, DELIVERYSTATUS, " & _
                                                "POD, ORDERPRIORITY, RECEIVEDFROM, SHIPTO, EXPECTEDDATE, APPOINTMENTID, CARRIERID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) "
            SQL += "VALUES(" & Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_flowthrough) & "," & Made4Net.Shared.Util.FormatField(_ordertype) & "," & Made4Net.Shared.Util.FormatField(_referenceord) & "," & Made4Net.Shared.Util.FormatField(_sourcecompany) & "," & Made4Net.Shared.Util.FormatField(_sourcecompanytype) & "," & Made4Net.Shared.Util.FormatField(_targetcompany) & "," & _
                               Made4Net.Shared.Util.FormatField(_targetcompanytype) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_statusdate) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & Made4Net.Shared.Util.FormatField(_createdate) & "," & Made4Net.Shared.Util.FormatField(_scheduledarrivaldate) & "," & Made4Net.Shared.Util.FormatField(_requesteddeliverydate) & "," & _
                               Made4Net.Shared.Util.FormatField(_scheduleddeliverydate) & "," & Made4Net.Shared.Util.FormatField(_shippeddate) & "," & Made4Net.Shared.Util.FormatField(_staginglane) & "," & Made4Net.Shared.Util.FormatField(_stagingwarehousearea) & "," & Made4Net.Shared.Util.FormatField(_shipment) & "," & Made4Net.Shared.Util.FormatField(_stopnumber) & "," & Made4Net.Shared.Util.FormatField(_loadingseq) & "," & Made4Net.Shared.Util.FormatField(_routingset) & "," & Made4Net.Shared.Util.FormatField(_route) & "," & Made4Net.Shared.Util.FormatField(_deliverystatus) & "," & _
                               Made4Net.Shared.Util.FormatField(_pod) & "," & Made4Net.Shared.Util.FormatField(_orderpriority) & "," & Made4Net.Shared.Util.FormatField(_receivedfrom) & "," & Made4Net.Shared.Util.FormatField(_shipto) & "," & _
                               Made4Net.Shared.Util.FormatField(_exptecteddate) & "," & Made4Net.Shared.Util.FormatField(_appointmentid) & "," & Made4Net.Shared.Util.FormatField(_carrierid) & "," & Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

        Else
            _editdate = DateTime.Now
            _edituser = pUser
            SQL = String.Format("UPDATE FLOWTHROUGHHEADER " & _
                                "SET ORDERTYPE ={0}, REFERENCEORD ={1}, SOURCECOMPANY ={2}, SOURCECOMPANYTYPE ={3}, TARGETCOMPANY ={4}, " & _
                                    " TARGETCOMPANYTYPE ={5}, STATUS ={6}, STATUSDATE ={7}, NOTES ={8}, CREATEDATE ={9}, SCHEDULEDARRIVALDATE ={10}, REQUESTEDDELIVERYDATE ={11}, " & _
                                    " SCHEDULEDDELIVERYDATE ={12}, SHIPPEDDATE ={13}, STAGINGLANE ={14}, STAGINGWAREHOUSEAREA={29}, SHIPMENT ={15}, STOPNUMBER ={16}, LOADINGSEQ ={17}, ROUTINGSET ={18}, ROUTE ={19}, " & _
                                    " DELIVERYSTATUS ={20}, POD ={21}, ORDERPRIORITY ={22}, RECEIVEDFROM={27}, SHIPTO={28}, EDITDATE ={23}, EDITUSER = {24}, EXPECTEDDATE={30}, APPOINTMENTID={31}, CARRIERID={32} " & _
                                " WHERE CONSIGNEE ={25} AND  FLOWTHROUGH ={26}", _
                                Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_sourcecompany), Made4Net.Shared.Util.FormatField(_sourcecompanytype), Made4Net.Shared.Util.FormatField(_targetcompany), Made4Net.Shared.Util.FormatField(_targetcompanytype), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), _
                                Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_scheduledarrivaldate), Made4Net.Shared.Util.FormatField(_requesteddeliverydate), Made4Net.Shared.Util.FormatField(_scheduleddeliverydate), Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_shipment), _
                                Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_deliverystatus), Made4Net.Shared.Util.FormatField(_pod), Made4Net.Shared.Util.FormatField(_orderpriority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_flowthrough), Made4Net.Shared.Util.FormatField(_receivedfrom), Made4Net.Shared.Util.FormatField(_shipto), Made4Net.Shared.Util.FormatField(_stagingwarehousearea), _
                                Made4Net.Shared.FormatField(_exptecteddate), Made4Net.Shared.FormatField(_appointmentid), Made4Net.Shared.FormatField(_carrierid))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Edit(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pOrdertype As String, ByVal pReferenceord As String, _
                         ByVal pSourcecompany As String, ByVal pSourcecompanytype As String, ByVal pTargetcompany As String, ByVal pTargetcompanytype As String, _
                         ByVal pStatus As String, ByVal pStatusdate As DateTime, ByVal pNotes As String, _
                         ByVal pCreatedate As DateTime, ByVal pScheduledarrivaldate As DateTime, ByVal pRequesteddeliverydate As DateTime, _
                         ByVal pScheduleddeliverydate As DateTime, ByVal pShippeddate As DateTime, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, _
                         ByVal pShipment As String, ByVal pStopnumber As String, ByVal pLoadingseq As String, ByVal pRoutingset As String, _
                         ByVal pRoute As String, ByVal pDeliverystatus As String, ByVal pPod As String, ByVal pOrderpriority As String, _
                         ByVal pShipTo As String, ByVal pReceivedFrom As String, ByVal pExpectedDate As DateTime, ByVal pAppointmentID As String, ByVal pCarrierID As String, ByVal pEditdate As DateTime, ByVal pEdituser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        _consignee = pConsignee
        _flowthrough = pFlowthrough
        _ordertype = "CUST"

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
        _status = pStatus
        _statusdate = pStatusdate
        _notes = pNotes
        _createdate = DateTime.Now()
        _scheduledarrivaldate = pScheduledarrivaldate
        _requesteddeliverydate = pRequesteddeliverydate
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

        _exptecteddate = pExpectedDate
        _appointmentid = pAppointmentID
        _carrierid = pCarrierID


        _editdate = DateTime.Now
        _edituser = Common.GetCurrentUser

        SQL = String.Format("UPDATE FLOWTHROUGHHEADER " & _
        "SET ORDERTYPE ={0}, REFERENCEORD ={1}, SOURCECOMPANY ={2}, SOURCECOMPANYTYPE ={3}, TARGETCOMPANY ={4}, " & _
            " TARGETCOMPANYTYPE ={5}, STATUS ={6}, STATUSDATE ={7}, NOTES ={8}, CREATEDATE ={9}, SCHEDULEDARRIVALDATE ={10}, REQUESTEDDELIVERYDATE ={11}, " & _
            " SCHEDULEDDELIVERYDATE ={12}, SHIPPEDDATE ={13}, STAGINGLANE ={14},STAGINGWAREHOUSEAREA={29}, SHIPMENT ={15}, STOPNUMBER ={16}, LOADINGSEQ ={17}, ROUTINGSET ={18}, ROUTE ={19}, " & _
            " DELIVERYSTATUS ={20}, POD ={21}, ORDERPRIORITY ={22}, RECEIVEDFROM={27}, SHIPTO={28}, EXPECTEDDATE={30}, APPOINTMENTID={31},CARRIERID={32}, EDITDATE ={23}, EDITUSER = {24}" & _
        " WHERE CONSIGNEE ={25} AND FLOWTHROUGH ={26}", _
        Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_sourcecompany), Made4Net.Shared.Util.FormatField(_sourcecompanytype), _
        Made4Net.Shared.Util.FormatField(_targetcompany), Made4Net.Shared.Util.FormatField(_targetcompanytype), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), _
        Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_scheduledarrivaldate), Made4Net.Shared.Util.FormatField(_requesteddeliverydate), Made4Net.Shared.Util.FormatField(_scheduleddeliverydate), Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_shipment), _
        Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_routingset), Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_deliverystatus), Made4Net.Shared.Util.FormatField(_pod), Made4Net.Shared.Util.FormatField(_orderpriority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
        Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_flowthrough), Made4Net.Shared.Util.FormatField(_receivedfrom), Made4Net.Shared.Util.FormatField(_shipto), Made4Net.Shared.Util.FormatField(_stagingwarehousearea), _
        Made4Net.Shared.Util.FormatField(_exptecteddate), Made4Net.Shared.Util.FormatField(_appointmentid), Made4Net.Shared.Util.FormatField(_carrierid))

        DataInterface.RunSQL(SQL)

    End Sub

#End Region

#Region "Lines"

    Public Sub AddLine(ByVal oLine As FlowthroughDetail)
        AddFlowthroughLine(oLine)
    End Sub

    Public Shared Function LineExists(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As Int32) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '{0}' AND FLOWTHROUGH = '{1}' AND FLOWTHROUGHLINE = {2}", pConsignee, pFlowthrough, pFlowthroughLine)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub AddLine(ByVal pFlowthrough As String, ByVal pConsignee As String, ByVal pLine As Int32, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing)
        _Lines.AddLine(pFlowthrough, pConsignee, pLine, pSku, pUnits, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
    End Sub

#End Region

#Region "Add/Update Lines"

    Private Sub AddFlowthroughLine(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pSku As String, _
                            ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0)
        _Lines.CreateNewLine(pConsignee, pFlowthrough, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pRoute, pStopNumber)
    End Sub

    Private Sub AddFlowthroughLine(ByVal oLine As FlowthroughDetail)
        If FlowthroughDetail.Exists(oLine.CONSIGNEE, oLine.FLOWTHROUGH, oLine.FLOWTHROUGHLINE) Then
            _Lines.EditLine(oLine.CONSIGNEE, oLine.FLOWTHROUGH, oLine.FLOWTHROUGHLINE, oLine.SKU, oLine.QTYMODIFIED, oLine.INVENTORYSTATUS, oLine.EDITUSER, oLine.Attributes.Attributes, oLine.ROUTE, oLine.STOPNUMBER)
        Else
            _Lines.AddLine(oLine.FLOWTHROUGH, oLine.CONSIGNEE, oLine.FLOWTHROUGHLINE, oLine.SKU, oLine.QTYORIGINAL, oLine.INVENTORYSTATUS, oLine.ADDUSER, oLine.Attributes.Attributes, 0, Nothing, Nothing, oLine.ROUTE, oLine.STOPNUMBER)
        End If
    End Sub

    Private Sub EditFlowthroughLine(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As String, ByVal pSku As String, _
                ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pRoute As String = "", Optional ByVal pStopNumber As Integer = 0)
        _Lines.EditLine(pConsignee, pFlowthrough, pFlowthroughLine, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pRoute, pStopNumber)
    End Sub
    Private Sub DeleteFlowthroughLine(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pFlowthroughLine As String)
        Dim ftLine As New FlowthroughDetail(pConsignee, pFlowthrough, pFlowthroughLine)
        ftLine.Delete(Common.GetCurrentUser)
    End Sub


#End Region

#Region "Print Label"

    Public Sub PrintFlowthroughLabels(ByVal lblPrinter As String)
        Dim lbltype As String
        Try
            lbltype = WMS.Lib.FLOWTHROUGHLABELTYPE.FLOWTHROUGH
        Catch ex As Exception

        End Try
        If Not lbltype = WMS.Lib.FLOWTHROUGHLABELTYPE.NONE Then
            PrintFlowthroughLabels(lbltype, lblPrinter)
        End If
    End Sub

    Public Sub PrintFlowthroughLabels(ByVal LabelType As String, ByVal lblPrinter As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "Flowthroughlabel")
        qSender.Add("LABELTYPE", LabelType)
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("FLOWTHROUGH", _flowthrough)
        qSender.Send("Label", "Flowthrough Label")
    End Sub


    Public Sub PrintLabel(ByVal prtrName As String)
        If prtrName Is Nothing Then
            prtrName = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "Flowthroughlabel")
        qSender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        qSender.Add("PRINTER", prtrName)
        qSender.Add("CONSIGNEE", _consignee)
        qSender.Add("ORDERID", _flowthrough)
        qSender.Send("Label", "Flowthrough Label")
    End Sub

#End Region

#Region "Recieve Flowthrough"

    Public Sub Receive(ByVal pLine As Int32, ByVal pQuantity As Decimal, ByVal pUser As String)
        Dim tLine As FlowthroughDetail = _Lines.Line(pLine)
        If IsNothing(tLine) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Flowthrough Order Line Does Not Exists", "Flowthrough Order Line Does Not Exists")
            Throw m4nEx
        End If
        tLine.Receive(pQuantity, pUser)
        _editdate = DateTime.Now
        _edituser = pUser

        If _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW Then
            _status = WMS.Lib.Statuses.Flowthrough.RECEIVING
        End If

        Dim sql As String = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = '{0}',EDITDATE = {1},EDITUSER = {2} WHERE {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub SetReceived(ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser

        If _status = WMS.Lib.Statuses.Flowthrough.RECEIVING Then
            _status = WMS.Lib.Statuses.Flowthrough.RECEIVED
        End If

        Dim sql As String = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = '{0}',EDITDATE = {1},EDITUSER = {2} WHERE {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub CancelReceive(ByVal pLine As Int32, ByVal pQuantity As Decimal, ByVal pUser As String)
        Dim tLine As FlowthroughDetail = _Lines.Line(pLine)
        If IsNothing(tLine) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Flowthrough Order Line Does Not Exists", "Flowthrough Order Line Does Not Exists")
            Throw m4nEx
        End If
        tLine.CancelReceive(pQuantity, pUser)
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = '{0}',EDITDATE = {1},EDITUSER = {2} WHERE {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Receipt Methods"
    'Added For RWMS-494
    'Public Function CreateReceipt(ByVal pUser As String) As String
    Public Function CreateReceipt(ByVal pUser As String, ByRef IsExport As Boolean, ByRef FlowthroughExported As Boolean) As String
        'Ended For RWMS-484  
        Dim oReceipt As New ReceiptHeader
        Dim dr As DataRow
        Dim ReceiptId As String = String.Empty
        'Added For RWMS-494
        Dim cntReceipt As Boolean
        'End RWMS-494
        'Added For RWMS-484   
        Dim CntExport As Integer
        Dim cnt As Boolean
        'Ended For RWMS-484  

        'Added For RWMS-484   
        'oReceipt.CreateNew("", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, pUser)
        'oReceipt.CreateNew("", Me.SCHEDULEDARRIVALDATE, Nothing, Nothing, Me.CARRIERID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, pUser)
        ' ReceiptId = oReceipt.RECEIPT
        'Ended For RWMS-484  
        If _Lines.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cant create Receipt. Flowthrough has no lines.", "Cant create Receipt. Flowthrough has no lines.")
            Throw m4nEx
        End If
        'Added For RWMS-484 
        For Each Line As FlowthroughDetail In _Lines
            ' oReceipt.addLineFromOrder(_consignee, _flowthrough, Line.FLOWTHROUGHLINE, Nothing, Nothing, Line.INVENTORYSTATUS, _sourcecompany, _sourcecompanytype, -1, "", pUser, Line.Attributes.Attributes, Line.INPUTQTY, Line.INPUTUOM, Line.INPUTSKU, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH)
            'Checking the flowthrough lines exported or not   
            If Not (CheckOrderLineForReceipt(Line.FLOWTHROUGHLINE, Line.FLOWTHROUGH)) Then
                'Create one Receipt ID for Flowthrough   
                If cnt = False Then
                    oReceipt.CreateNew("", Me.SCHEDULEDARRIVALDATE, Nothing, Nothing, Me.CARRIERID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, pUser)
                    cnt = True
                End If
                IsExport = True
                CntExport = CntExport + 1
                ReceiptId = oReceipt.RECEIPT
                'Create Receipt line for the receipt   
                oReceipt.addLineFromOrder(_consignee, _flowthrough, Line.FLOWTHROUGHLINE, Nothing, Nothing, Line.INVENTORYSTATUS, _sourcecompany, _sourcecompanytype, -1, "", pUser, Line.Attributes.Attributes, Line.INPUTQTY, Line.INPUTUOM, Line.INPUTSKU, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH)

                'Added For RWMS-494
                If cntReceipt = False Then
                    Dim orec As New WMS.Logic.ReceiptHeader(ReceiptId)
                    ReceiptHeader.PrintReceivingDoc(ReceiptId, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, False)
                    cntReceipt = True
                End If
                'End RWMS-494

            End If
            'Ended For RWMS-484  

            ''Added For RWMS-494
            'If cntReceipt = False Then
            '    Dim orec As New WMS.Logic.ReceiptHeader(ReceiptId)
            '    ReceiptHeader.PrintReceivingDoc(ReceiptId, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, False)
            '    cntReceipt = True
            'End If
            ''End RWMS-494

        Next
        'Added For RWMS-484 
        'Checking whether some of lines exported or not   
        If CntExport <> 0 Then
            If CntExport < _Lines.Count Then
                FlowthroughExported = True
            End If
        End If
        'Ended For RWMS-484  

        Return ReceiptId
    End Function
    'Added For RWMS-484   
    'Funtion for checking flowthrough exported or not   
    Public Function CheckOrderLineForReceipt(ByVal lines As Integer, ByVal OrderId As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(*) FROM RECEIPTDETAIL WHERE ORDERLINE='{0}' AND ORDERID ='{1}'", lines, OrderId)
        Dim Exist As Boolean
        Exist = System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        Return Exist
    End Function
    'Ended For RWMS-484  

#End Region

#Region "Set Contacts"

    Public Sub SetReceivedFrom(ByVal pContactID As String)
        Dim SQL As String = "UPDATE FLOWTHROUGHHEADER SET RECEIVEDFROM = '" & pContactID & "' WHERE " & WhereClause
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub SetShipTo(ByVal pContactID As String)
        Dim SQL As String = "UPDATE FLOWTHROUGHHEADER SET SHIPTO = '" & pContactID & "' WHERE " & WhereClause
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Set status"

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
        Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        Dim oldStatus As String = _status
        _status = pStatus
        _statusdate = DateTime.Now

        sql = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Pack / Load /Stage /Ship status change"

    Public Function Ship(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Flowthrough.SHIPPED OrElse _
           _status = WMS.Lib.Statuses.Flowthrough.CANCELED OrElse _
            _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cant Ship flowthrough, incorrect status", "Cant Ship flowthrough, incorrect status")
        End If

        SetStatus(WMS.Lib.Statuses.Flowthrough.SHIPPING, pUser)
        Dim qh As New Made4Net.Shared.QMsgSender
        qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.FlowThroughShipped)
        qh.Values.Add("CONSIGNEE", _consignee)
        qh.Values.Add("DOCUMENT", _flowthrough)
        qh.Values.Add("USER", pUser)
        qh.Send("Shipper", _consignee & "_" & _flowthrough)
    End Function

    Public Sub ShipFlowThrough(ByVal puser As String)
        If CANSHIP Then
            If _status = WMS.Lib.Statuses.Flowthrough.SHIPPED Then
                Return
            Else
                Dim sql As String
                _editdate = DateTime.Now
                _edituser = puser
                _status = WMS.Lib.Statuses.Flowthrough.SHIPPED
                _shippeddate = DateTime.Now
                _statusdate = DateTime.Now

                If _Loads.Count = 0 Then
                    Me.UpdateShipStatus(puser)
                    Return
                End If

                Dim shpld As Load
                For Each oLoad As OrderLoads In _Loads
                    Try
                        shpld = New Load(oLoad.LOADID)
                        shpld.Ship(puser, oLoad.ORDERID, oLoad.ORDERLINE)
                    Catch ex As Exception

                    End Try
                Next

                'sql = String.Format("UPDATE FLOWTHROUGHHEADER SET SHIPPEDDATE = {0},STATUS = {1},STATUSDATE = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_shippeddate), _
                '             Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                'DataInterface.RunSQL(sql)
            End If
        Else
            Throw New ApplicationException(String.Format("Order {0} status is incorrect, can't ship Flowthrough", _flowthrough))
        End If
    End Sub

    Public Sub AssignToShipment(ByVal pShipmentId As String, ByVal pUser As String)
        If (_status <> WMS.Lib.Statuses.Flowthrough.CANCELED) Then
            _shipment = pShipmentId
            _editdate = DateTime.Now
            _edituser = pUser
            DataInterface.RunSQL(String.Format("Update FLOWTHROUGHHEADER SET SHIPMENT = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Flowthrough status is incorrect for ship assignment", "Flowthrough status is incorrect for canceling")
            Throw m4nEx
        End If
    End Sub

    Public Sub DeAssignFromShipment(ByVal pUser As String)
        If (_status <> WMS.Lib.Statuses.Flowthrough.CANCELED) Then
            _shipment = ""
            _editdate = DateTime.Now
            _edituser = pUser
            DataInterface.RunSQL(String.Format("Update FLOWTHROUGHHEADER SET SHIPMENT = {0},STATUS = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Flowthrough status is incorrect for ship assignment", "Flowthrough status is incorrect for canceling")
            Throw m4nEx
        End If
    End Sub

    Public Sub UpdateShipStatus(ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        Dim oldStatus As String = _status
        _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED
        _shippeddate = DateTime.Now
        _statusdate = DateTime.Now

        sql = String.Format("UPDATE FLOWTHROUGHHEADER SET SHIPPEDDATE = {0},STATUS = {1},STATUSDATE = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_shippeddate), _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        'RWMS-726
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Updated flowthroughheader shipment status {0}", sql))
            oLogger.writeSeperator(" ", 100)
        End If


        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.FlowThroughShipped)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.FLOWTHSHP)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _flowthrough)
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
        aq.Send(WMS.Lib.Actions.Audit.FLOWTHSHP)
    End Sub

    Public Sub Pack(ByVal puser As String)
        Dim oldstat As String = _status
        _statusdate = DateTime.Now
        _status = WMS.Lib.Statuses.OutboundOrderHeader.PACKED
        Dim sql As String = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.FlowThroughPacked)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.FLOWTHPAK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _flowthrough)
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
        aq.Send(WMS.Lib.Actions.Audit.FLOWTHPAK)
    End Sub

    Public Sub UpdateLoadStatus(ByVal pUser)
        Dim sql As String
        _editdate = DateTime.Now
        _edituser = pUser
        Dim oldStatus As String = _status
        _status = WMS.Lib.Statuses.OutboundOrderHeader.LOADED
        _statusdate = DateTime.Now

        sql = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.FlowThroughLoaded)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.FLOWTHLD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _flowthrough)
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
        aq.Send(WMS.Lib.Actions.Audit.FLOWTHLD)
    End Sub

    Public Sub Stage(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED Then
            Return
        Else
            Dim sql As String

            _editdate = DateTime.Now
            _edituser = pUser
            _status = WMS.Lib.Statuses.OutboundOrderHeader.STAGED
            _statusdate = DateTime.Now

            sql = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(sql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.FlowThroughStaged)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.FLOWTHSTG)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _flowthrough)
            aq.Add("DOCUMENTLINE", "")
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
            aq.Send(WMS.Lib.Actions.Audit.FLOWTHSTG)
        End If
    End Sub

#End Region

#Region "SL Assign"

    Public Sub SetStagingLane(ByVal pStagingLane As String, ByVal pStagingwarehousearea As String, ByVal pUser As String)
        Dim SQL As String = String.Empty
        Dim exist As Boolean = True
        ''Added for PWMS-799, RWMS-864 Added RECEIVED status also  
        If _status = WMS.Lib.Statuses.Flowthrough.STATUSNEW Or _status = WMS.Lib.Statuses.Flowthrough.RECEIVING Or _status = WMS.Lib.Statuses.Flowthrough.RECEIVED Then
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

            SQL = String.Format("UPDATE FLOWTHROUGHHEADER SET staginglane = {0},STAGINGWAREHOUSEAREA={4}, editdate = {1},edituser={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_stagingwarehousearea))

            DataInterface.RunSQL(SQL)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot update order.Invalid Status", "Can't update order.Invalid Status")
            Throw m4nEx
        End If
    End Sub

#End Region

    Public Sub Verify(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Flowthrough.VERIFIED Then
            Return
        Else
            Dim sql As String
            _editdate = DateTime.Now
            _edituser = pUser
            Dim oldStatus As String = _status
            _status = WMS.Lib.Statuses.Flowthrough.VERIFIED
            _statusdate = DateTime.Now

            sql = String.Format("UPDATE FLOWTHROUGHHEADER SET STATUS = {0},STATUSDATE = {1},EDITDATE = {2},EDITUSER = {3} WHERE {4}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(sql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OutBoundOrderVerified)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.OUTBOUNDHVRF)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _flowthrough)
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

#Region "Reports"

    Public Sub PrintShippingManifest(ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "FlowthrowghDelNote"
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "FlowthrowghDelNote", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "FlowthrowghDelNote")
        oQsender.Add("DATASETID", "repFlowthrowghDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "") 'dt.Rows(0)("DEFAULTPRINTER"))
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONSIGNEE='{0}' and FLOWTHROUGH='{1}'", _consignee, _flowthrough))
        oQsender.Send("Report", repType)
    End Sub

#End Region


End Class
