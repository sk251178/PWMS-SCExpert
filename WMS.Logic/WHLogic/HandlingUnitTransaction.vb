Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports System.Xml
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class HandlingUnitTransactionCollection
    Inherits ArrayList

#Region "Variables"

    Protected _shipment As String
    Protected _receipt As String
    Protected _consignee As String
    Protected _orderid As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As HandlingUnitTransaction
        Get
            Return CType(MyBase.Item(index), HandlingUnitTransaction)
        End Get
    End Property

    Public ReadOnly Property HandlingUnitTransaction(ByVal pHUTransId As String) As HandlingUnitTransaction
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).TransactionId = pHUTransId Then
                    Return (CType(MyBase.Item(i), HandlingUnitTransaction))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pTransactionType As String, Optional ByVal LoadAll As Boolean = True)
        _consignee = pConsignee
        _orderid = pOrderId
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select * from HANDLINGUNITTRANSACTION where consignee = '" & _consignee & "' and ORDERID = '" & _orderid & "' and transactiontype = '" & pTransactionType & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.Add(New HandlingUnitTransaction(dr))
        Next
    End Sub

    Public Sub New(ByVal pTransactionTypeId As String, ByVal pTransactionType As String, Optional ByVal LoadAll As Boolean = True)
        Select Case pTransactionType
            Case HandlingUnitTransactionTypes.Receipt
                _receipt = pTransactionTypeId
            Case HandlingUnitTransactionTypes.Shipment
                _shipment = pTransactionTypeId
        End Select
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select * from HANDLINGUNITTRANSACTION where transactiontypeid = '" & pTransactionTypeId & "' and transactiontype = '" & pTransactionType & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.Add(New HandlingUnitTransaction(dr))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function Add(ByVal pObject As HandlingUnitTransaction) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Remove(ByVal pObject As HandlingUnitTransaction)
        MyBase.Remove(pObject)
    End Sub

#End Region

End Class

<CLSCompliant(False)> Public Class HandlingUnitTransaction

#Region "Variables"

    Private _transactionid As String
    Private _transactiontype As String     ' Shipment / Receipt
    Private _transactiontypeid As String     ' Shipment / Receipt ID
    Private _transactiondate As DateTime
    Private _consignee As String
    Private _documenttype As String
    Private _orderid As String
    Private _companytype As String
    Private _company As String
    Private _hutype As String
    Private _huqty As Decimal
    Private _adduser As String
    Private _adddate As DateTime
    Private _edituser As String
    Private _editdate As DateTime

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " TRANSACTIONID = '" & _transactionid & "'"
        End Get
    End Property

    Public Property TransactionId() As String
        Get
            Return _transactionid
        End Get
        Set(ByVal Value As String)
            _transactionid = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return _transactiontype
        End Get
        Set(ByVal Value As String)
            _transactiontype = Value
        End Set
    End Property

    Public Property TransactionTypeId() As String
        Get
            Return _transactiontypeid
        End Get
        Set(ByVal Value As String)
            _transactiontypeid = Value
        End Set
    End Property

    Public Property TransactionDate() As DateTime
        Get
            Return _transactiondate
        End Get
        Set(ByVal Value As DateTime)
            _transactiondate = Value
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

    Public Property DocumentType() As String
        Get
            Return _documenttype
        End Get
        Set(ByVal Value As String)
            _documenttype = Value
        End Set
    End Property

    Public Property OrderId() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
        End Set
    End Property

    Public Property CompanyType() As String
        Get
            Return _companytype
        End Get
        Set(ByVal Value As String)
            _companytype = Value
        End Set
    End Property

    Public Property Company() As String
        Get
            Return _company
        End Get
        Set(ByVal Value As String)
            _company = Value
        End Set
    End Property

    Public Property HandlingUnitType() As String
        Get
            Return _hutype
        End Get
        Set(ByVal Value As String)
            _hutype = Value
        End Set
    End Property

    Public Property HandlingUnitQuantity() As Decimal
        Get
            Return _huqty
        End Get
        Set(ByVal Value As Decimal)
            _huqty = Value
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

    Public Sub New(ByVal pTransactionId As String, Optional ByVal LoadObj As Boolean = True)
        _transactionid = pTransactionId
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

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM HANDLINGUNITTRANSACTION WHERE" & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "HUTransaction Does not Exists", "HUTransaction Does not Exists")
        Else
            dr = dt.Rows(0)
        End If
        Load(dr)
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("TRANSACTIONID") Then _transactionid = dr.Item("TRANSACTIONID")
        If Not dr.IsNull("TRANSACTIONTYPE") Then _transactiontype = dr.Item("TRANSACTIONTYPE")
        If Not dr.IsNull("TRANSACTIONTYPEID") Then _transactiontypeid = dr.Item("TRANSACTIONTYPEID")
        If Not dr.IsNull("TRANSACTIONDATE") Then _transactiondate = dr.Item("TRANSACTIONDATE")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("DOCUMENTTYPE") Then _documenttype = dr.Item("DOCUMENTTYPE")
        If Not dr.IsNull("COMPANY") Then _company = dr.Item("COMPANY")
        If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")
        If Not dr.IsNull("HUTYPE") Then _hutype = dr.Item("HUTYPE")
        If Not dr.IsNull("HUQTY") Then _huqty = dr.Item("HUQTY")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

#Region "Create / Update"

    Public Sub Create(ByVal pTransId As String, ByVal pTransType As String, ByVal pTransTypeId As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pDocType As String, _
        ByVal pCompany As String, ByVal pCompanyType As String, ByVal pHUType As String, ByVal pHUQty As Decimal, ByVal pUser As String)


        If pTransId = "" Then
            _transactionid = Made4Net.Shared.Util.getNextCounter("HUTRANSID")
        Else
            _transactionid = pTransId
        End If
        _transactiontype = pTransType
        _transactiontypeid = pTransTypeId
        _transactiondate = DateTime.Now
        _consignee = pConsignee
        _orderid = pOrderid
        _documenttype = pDocType
        _company = pCompany
        _companytype = pCompanyType
        _hutype = pHUType
        _huqty = pHUQty
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String = String.Format("INSERT INTO HANDLINGUNITTRANSACTION (TRANSACTIONID, TRANSACTIONTYPE, TRANSACTIONTYPEID, TRANSACTIONDATE, CONSIGNEE, ORDERID, DOCUMENTTYPE, COMPANY, COMPANYTYPE, HUTYPE, HUQTY, ADDDATE, ADDUSER, EDITDATE, EDITUSER)" & _
                        " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})", _
                        Made4Net.Shared.Util.FormatField(_transactionid), Made4Net.Shared.Util.FormatField(_transactiontype), Made4Net.Shared.Util.FormatField(_transactiontypeid), Made4Net.Shared.Util.FormatField(_transactiondate), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_documenttype), _
                        Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_huqty), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub UpdateHUQty(ByVal pQty As Decimal, ByVal pUser As String)
        _huqty = pQty
        _edituser = pUser
        _editdate = DateTime.Now
        _transactiondate = DateTime.Now

        Dim SQL As String = String.Format("UPDATE HANDLINGUNITTRANSACTION SET HUQTY ={0}, TRANSACTIONDATE ={1}, EDITDATE ={2}, EDITUSER ={3} where {4}", _
                        Made4Net.Shared.Util.FormatField(_huqty), Made4Net.Shared.Util.FormatField(_transactiondate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#End Region

End Class

<CLSCompliant(False)> Public Class HandlingUnitTransactionTypes
    Public Const Shipment = "SHIPMENT"
    Public Const Receipt = "RECEIPT"
    Public Const InboundOrder = "INORD"
    Public Const OutboundOrder = "OUTORD"
End Class
