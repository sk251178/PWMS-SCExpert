Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class BillingChargesLoad

#Region "Variables"

    Protected _chargeid As String
    Protected _chargeline As String
    Protected _transactiondate As DateTime
    Protected _transactiontype As String
    Protected _loadid As String
    Protected _uom As String
    Protected _consignee As String
    Protected _sku As String
    Protected _units As Decimal
    Protected _unitsperuom As Decimal
    Protected _uomweight As Decimal
    Protected _uomvolume As Decimal
    Protected _unitprice As Decimal

#End Region

#Region "Properties"

    Public Property ChargeId() As String
        Get
            Return _chargeid
        End Get
        Set(ByVal Value As String)
            _chargeid = Value
        End Set
    End Property

    Public Property ChargeLine() As String
        Get
            Return _chargeline
        End Get
        Set(ByVal Value As String)
            _chargeline = Value
        End Set
    End Property

    Public Property TransactionDate() As datetime
        Get
            Return _transactiondate
        End Get
        Set(ByVal Value As datetime)
            _transactiondate = Value
        End Set
    End Property

    Public Property LoadId() As String
        Get
            Return _loadid
        End Get
        Set(ByVal Value As String)
            _loadid = Value
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

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property Sku() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property Units() As Decimal
        Get
            Return _units
        End Get
        Set(ByVal Value As Decimal)
            _units = Value
        End Set
    End Property

    Public Property UnitOfMeasure() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property

    Public Property UnitsPerMeasure() As Decimal
        Get
            Return _unitsperuom
        End Get
        Set(ByVal Value As Decimal)
            _unitsperuom = Value
        End Set
    End Property

    Public Property UnitPrice() As Decimal
        Get
            Return _unitprice
        End Get
        Set(ByVal Value As Decimal)
            _unitprice = Value
        End Set
    End Property

    Public Property UomVolume() As Decimal
        Get
            Return _uomvolume
        End Get
        Set(ByVal Value As Decimal)
            _uomvolume = Value
        End Set
    End Property

    Public Property UomWeight() As Decimal
        Get
            Return _uomweight
        End Get
        Set(ByVal Value As Decimal)
            _uomweight = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal ChrageId As String, ByVal ChargeLine As String, ByVal TransDate As datetime, ByVal loadid As String, Optional ByVal loadobj As Boolean = True)
        _chargeid = ChargeId
        _chargeline = ChargeLine
        _transactiondate = TransDate
        _loadid = loadid
        If loadobj Then Load()
    End Sub

#End Region

#Region "Methods"

#Region "Data Base Methods"

    Public Sub Load()
        Dim SQL As String = String.Format("SELECT * FROM BILLINGCHARGESLOADS WHERE CHARGEID={0} AND CHARGELINE={1} AND TRANSACTIONDATE={2} AND LOADID={3}", _
                    Made4Net.Shared.Util.FormatField(_chargeid), Made4Net.Shared.Util.FormatField(_chargeline), _
                    Made4Net.Shared.Util.FormatField(_transactiondate), Made4Net.Shared.Util.FormatField(_loadid))
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Agreemen line does not exists", "Agreemen line does not exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("CHARGEID") Then _chargeid = dr.Item("CHARGEID")
        If Not dr.IsNull("CHARGELINE") Then _chargeline = dr.Item("CHARGELINE")
        If Not dr.IsNull("TRANSACTIONDATE") Then _transactiondate = dr.Item("TRANSACTIONDATE")
        If Not dr.IsNull("TRANSACTIONTYPE") Then _transactiontype = dr.Item("TRANSACTIONTYPE")
        If Not dr.IsNull("LOADID") Then _loadid = dr.Item("LOADID")

        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("UOM") Then _uom = dr.Item("UOM")
        If Not dr.IsNull("UNITPRICE") Then _unitprice = dr.Item("UNITPRICE")
        If Not dr.IsNull("UNITSPERUOM") Then _unitsperuom = dr.Item("UNITSPERUOM")
        If Not dr.IsNull("UOMVOLUME") Then _uomvolume = dr.Item("UOMVOLUME")
        If Not dr.IsNull("UOMWEIGHT") Then _uomweight = dr.Item("UOMWEIGHT")
    End Sub

    Public Function Exists(ByVal ChrageId As String, ByVal ChargeLine As String, ByVal TransDate As DateTime, ByVal loadid As String) As Boolean
        Dim sql As String
        sql = String.Format("SELECT COUNT(1) FROM BILLINGCHARGESLOADS WHERE CHARGEID={0} AND CHARGELINE={1} AND TRANSACTIONDATE={2} AND LOADID={3}", _
                    Made4Net.Shared.Util.FormatField(ChrageId), Made4Net.Shared.Util.FormatField(ChargeLine), _
                    Made4Net.Shared.Util.FormatField(TransDate), Made4Net.Shared.Util.FormatField(loadid))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Save()
        Dim SQL As String
        'Insert
        SQL = String.Format("INSERT INTO BILLINGCHARGESLOADS(CHARGEID, CHARGELINE, TRANSACTIONDATE, LOADID, CONSIGNEE, SKU, UOM, UNITPRICE, UNITS, UNITSPERUOM, UOMVOLUME, UOMWEIGHT, TRANSACTIONTYPE) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}, {12}) ", _
                            Made4Net.Shared.Util.FormatField(_chargeid), Made4Net.Shared.Util.FormatField(_chargeline), Made4Net.Shared.Util.FormatField(_transactiondate), Made4Net.Shared.Util.FormatField(_loadid), _
                            Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_unitprice), _
                            Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_unitsperuom), Made4Net.Shared.Util.FormatField(_uomvolume), Made4Net.Shared.Util.FormatField(_uomweight), Made4Net.Shared.Util.FormatField(_transactiontype))
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#End Region

End Class

Public Class BillingChargesLoads

#Region "Variables"

#End Region

#Region "Properties"

#End Region

#Region "Constructors"

#End Region

#Region "Methods"

#End Region

End Class
