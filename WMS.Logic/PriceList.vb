Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

#Region "BILLINGPRICELISTDETAIL"

' <summary>
' This object represents the properties and methods of a BILLINGPRICELISTDETAIL.
' </summary>

Public Class PriceListDetail

#Region "Variables"

#Region "Primary Keys"

    Protected _pricelistname As String = String.Empty
    Protected _line As Int32

#End Region

#Region "Other Fields"

    Protected _minunits As Double = 0
    Protected _maxunits As Double = Double.MaxValue
    Protected _price As Double
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " PRICELISTNAME = '" & _pricelistname & "' And LINE = " & _line
        End Get
    End Property

    Public ReadOnly Property PRICELISTNAME() As String
        Get
            Return _pricelistname
        End Get
    End Property

    Public ReadOnly Property LINE() As Int32
        Get
            Return _line
        End Get
    End Property

    Public Property MINUNITS() As Double
        Get
            Return _minunits
        End Get
        Set(ByVal Value As Double)
            _minunits = Value
        End Set
    End Property

    Public Property MAXUNITS() As Double
        Get
            Return _maxunits
        End Get
        Set(ByVal Value As Double)
            _maxunits = Value
        End Set
    End Property

    Public Property PRICE() As Double
        Get
            Return _price
        End Get
        Set(ByVal Value As Double)
            _price = Value
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

    Public Sub New(ByVal pPRICELISTNAME As String, ByVal pLINE As Int32, Optional ByVal LoadObj As Boolean = True)
        _pricelistname = pPRICELISTNAME
        _line = pLINE
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetBILLINGPRICELISTDETAIL(ByVal pPRICELISTNAME As String, ByVal pLINE As Int32) As PriceListDetail
        Return New PriceListDetail(pPRICELISTNAME, pLINE)
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGPRICELISTDETAIL WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Price list line does not exists", "Price list line does not exists")
            m4nEx.Params.Add("pricelistname", _pricelistname)
            m4nEx.Params.Add("line", _line)
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("LINE") Then _line = dr.Item("LINE")
        If Not dr.IsNull("MINUNITS") Then _minunits = dr.Item("MINUNITS")
        If Not dr.IsNull("MAXUNITS") Then _maxunits = dr.Item("MAXUNITS")
        If Not dr.IsNull("PRICE") Then _price = dr.Item("PRICE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

    End Sub

#End Region

#End Region

End Class

#End Region

#Region "PRICELISTDETAIL COLLECTION"

Public Class PricelistDetailCollection
    Inherits ArrayList

#Region "Variables"

    Protected _pricelistname As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property item(ByVal index As Int32) As PriceListDetail
        Get
            Return CType(MyBase.Item(index), PriceListDetail)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal pLine As Int32) As PriceListDetail
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If item(i).LINE = pLine Then
                    Return (CType(MyBase.Item(i), PriceListDetail))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pPricelistName As String, Optional ByVal LoadAll As Boolean = True)
        _pricelistname = pPricelistName
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select line from BILLINGPRICELISTDETAIL where PRICELISTNAME = '" & _pricelistname & "' order by line"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New PriceListDetail(_pricelistname, dr.Item("LINE"), LoadAll))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As PriceListDetail) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As PriceListDetail)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As PriceListDetail)
        MyBase.Remove(pObject)
    End Sub

#End Region

End Class

#End Region

#Region "PRICELISTHEADER"

' <summary>
' This object represents the properties and methods of a BILLINGPRICELISTHEADER.
' </summary>

Public Class PriceList

#Region "Consts"

    Public Class PricelistTypes
        Public Const TOTAL As String = "TOTAL"
        Public Const [STEP] As String = "STEP"
        Public Const LINEAR As String = "LINEAR"
    End Class

#End Region

#Region "Variables"

#Region "Primary Keys"

    Protected _name As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _description As String = String.Empty
    Protected _pricelisttype As String = String.Empty
    Protected _isDateRange As Boolean
    Protected _active As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _pricelistlines As PricelistDetailCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " NAME = '" & _name & "'"
        End Get
    End Property

    Public ReadOnly Property Lines() As PricelistDetailCollection
        Get
            Return _pricelistlines
        End Get
    End Property

    Public ReadOnly Property NAME() As String
        Get
            Return _name
        End Get
    End Property

    Public Property DESCRIPTION() As String
        Get
            Return _description
        End Get
        Set(ByVal Value As String)
            _description = Value
        End Set
    End Property

    Public Property PRICELISTTYPE() As String
        Get
            Return _pricelisttype
        End Get
        Set(ByVal Value As String)
            _pricelisttype = Value
        End Set
    End Property

    Public Property ACTIVE() As Boolean
        Get
            Return _active
        End Get
        Set(ByVal Value As Boolean)
            _active = Value
        End Set
    End Property

    Public Property ISDATERANGE() As Boolean
        Get
            Return _isDateRange
        End Get
        Set(ByVal Value As Boolean)
            _isDateRange = Value
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

    Public Sub New(ByVal pNAME As String, Optional ByVal LoadObj As Boolean = True)
        _name = pNAME
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        If CommandName.ToLower = "createnew" Then
            dr = ds.Tables(0).Rows(0)
            Create(dr("name"), Convert.ReplaceDBNull(dr("DESCRIPTION")), Convert.ReplaceDBNull(dr("PRICELISTTYPE")), dr("ACTIVE"), dr("ISDATERANGE"), WMS.Logic.Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "update" Then
            dr = ds.Tables(0).Rows(0)
            _name = dr("name")
            update(Convert.ReplaceDBNull(dr("DESCRIPTION")), Convert.ReplaceDBNull(dr("PRICELISTTYPE")), dr("ACTIVE"), dr("ISDATERANGE"), WMS.Logic.Common.GetCurrentUser)
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetBILLINGPRICELISTHEADER(ByVal pNAME As String) As PriceList
        Return New PriceList(pNAME)
    End Function

    Public Shared Function Exists(ByVal pName As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from BILLINGPRICELISTHEADER where name = '{0}'", pName)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGPRICELISTHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Price list does not exists", "Price list does not exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("DESCRIPTION") Then _description = dr.Item("DESCRIPTION")
        If Not dr.IsNull("PRICELISTTYPE") Then _pricelisttype = dr.Item("PRICELISTTYPE")
        If Not dr.IsNull("ACTIVE") Then _active = dr.Item("ACTIVE")
        If Not dr.IsNull("ISDATERANGE") Then _isDateRange = dr.Item("ISDATERANGE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _pricelistlines = New PricelistDetailCollection(_name)

    End Sub

#End Region

#Region "Calculations"

    Public Function CalculateValue(ByVal units As Decimal, ByVal PriceListChargeUnits As Decimal, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
        Dim bpld As PriceListDetail
        Try
            If PriceListChargeUnits <= 0 Then PriceListChargeUnits = units
            For Each bpld In _pricelistlines
                If bpld.MINUNITS <= PriceListChargeUnits And bpld.MAXUNITS >= PriceListChargeUnits Then
                    Select Case _pricelisttype
                        Case PricelistTypes.LINEAR
                            If Not logger Is Nothing Then
                                logger.Write("Calculated : " & units * bpld.PRICE)
                            End If
                            Return units * bpld.PRICE
                        Case PricelistTypes.TOTAL
                            If Not logger Is Nothing Then
                                logger.Write("Calculated : " & bpld.PRICE)
                            End If
                            Return bpld.PRICE
                        Case PricelistTypes.STEP
                            'Return units * bpld.PRICE
                            If Not logger Is Nothing Then
                                logger.Write("Calculating STEP")
                            End If
                            Return CalcSteps(units, logger)
                        Case Else
                            Return bpld.PRICE
                    End Select
                End If
            Next
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Function CalcSteps(ByVal pUnits As Decimal, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
        Dim bpld As PriceListDetail
        Dim val As Decimal = 0
        Dim calUnits As Decimal = pUnits
        Dim line As Int32
        For Each bpld In _pricelistlines
            If bpld.MINUNITS <= calUnits And bpld.MAXUNITS >= calUnits Then
                line = bpld.LINE
                Exit For
            End If
        Next
        For Each bpld In _pricelistlines
            If bpld.LINE <= line Then

                If calUnits < bpld.MAXUNITS - bpld.MINUNITS + 1 Then
                    val = val + bpld.PRICE * (calUnits)
                    If Not logger Is Nothing Then
                        logger.Write("Line " & bpld.LINE & " Calculated : " & calUnits)
                    End If
                Else
                    val = val + bpld.PRICE * (bpld.MAXUNITS - bpld.MINUNITS + 1)
                    If Not logger Is Nothing Then
                        logger.Write("Line " & bpld.LINE & " Calculated : " & (bpld.MAXUNITS - bpld.MINUNITS + 1))
                    End If
                End If

                calUnits = calUnits - (bpld.MAXUNITS - bpld.MINUNITS + 1)
                'If calUnits >= bpld.MAXUNITS Then
                '    val = val + bpld.PRICE * (bpld.MAXUNITS - bpld.MINUNITS + 1)
                '    calUnits = calUnits - (bpld.MAXUNITS - bpld.MINUNITS + 1)
                'Else
                '    val = val + bpld.PRICE * calUnits
                '    calUnits = 0
                'End If
            End If

            'If calUnits = 0 Then Exit For
        Next
        Return val
    End Function

#End Region

#Region "Create / Update"

    Public Sub Create(ByVal pName As String, ByVal pDescription As String, ByVal pType As String, ByVal pActive As Boolean, ByVal pIsDateRange As Boolean, ByVal pUser As String)
        Dim SQL As String

        If PriceList.Exists(pName) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "PriceList Already Exist", "PriceList Already Exist")
            Throw m4nEx
        End If

        _name = pName
        _description = pDescription
        _pricelisttype = pType
        _active = pActive
        _isDateRange = pIsDateRange
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        SQL = String.Format("INSERT INTO BILLINGPRICELISTHEADER (NAME, DESCRIPTION, PRICELISTTYPE, ISDATERANGE, ACTIVE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                " VALUES( {0},{1},{2},{3},{4},{5},{6},{7},{8})", Made4Net.Shared.Util.FormatField(_name), Made4Net.Shared.Util.FormatField(_description), Made4Net.Shared.Util.FormatField(_pricelisttype), _
                Made4Net.Shared.Util.FormatField(_isDateRange), Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub update(ByVal pDescription As String, ByVal pType As String, ByVal pActive As Boolean, ByVal pIsDateRange As Boolean, ByVal pUser As String)
        Dim SQL As String

        _description = pDescription
        _pricelisttype = pType
        _active = pActive
        _isDateRange = pIsDateRange
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        SQL = String.Format("UPDATE BILLINGPRICELISTHEADER SET DESCRIPTION ={0}, PRICELISTTYPE ={1},ISDATERANGE={2}, ACTIVE ={3}, EDITDATE ={4}, EDITUSER ={5} where {6}", _
                Made4Net.Shared.Util.FormatField(_description), Made4Net.Shared.Util.FormatField(_pricelisttype), _
                Made4Net.Shared.Util.FormatField(_isDateRange), Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#End Region

End Class

#End Region

