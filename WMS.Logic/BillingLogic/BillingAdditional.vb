Imports System.Data
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class BillingAdditional

#Region "Vars"

    Protected _tranid As String
    Protected _trantype As String
    Protected _reftranid As String
    Protected _consignee As String
    Protected _units As Double
    Protected _unitstype As String
    Protected _istotalamount As Boolean
    Protected _price As Double
    Protected _trandate As DateTime
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " TRANID = '" & _tranid & "'"
        End Get
    End Property

    Public ReadOnly Property TRANID() As String
        Get
            Return _tranid
        End Get
    End Property

    Public Property TRANTYPE() As String
        Get
            Return _trantype
        End Get
        Set(ByVal Value As String)
            _trantype = Value
        End Set
    End Property

    Public Property REFTRANID() As String
        Get
            Return _reftranid
        End Get
        Set(ByVal Value As String)
            _reftranid = Value
        End Set
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property UNITS() As Double
        Get
            Return _units
        End Get
        Set(ByVal Value As Double)
            _units = Value
        End Set
    End Property

    Public Property UNITSTYPE() As String
        Get
            Return _unitstype
        End Get
        Set(ByVal Value As String)
            _unitstype = Value
        End Set
    End Property

    Public Property ISTOTALAMOUNT() As Boolean
        Get
            Return _istotalamount
        End Get
        Set(ByVal Value As Boolean)
            _istotalamount = Value
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

    Public Property TRANDATE() As DateTime
        Get
            Return _trandate
        End Get
        Set(ByVal Value As DateTime)
            _trandate = Value
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

#Region "Ctors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pTranId As String, Optional ByVal LoadObj As Boolean = True)
        _tranid = pTranId
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        If CommandName.ToLower = "saveadditionalagreement" Then
            dr = ds.Tables(0).Rows(0)
            Dim units, price As Double
            Dim tranId As String
            If dr.IsNull("UNITS") Then units = 0 Else units = dr("UNITS")
            If dr.IsNull("PRICE") Then price = 0 Else price = dr("PRICE")
            If dr.IsNull("TRANID") Then price = Made4Net.Shared.Util.getNextCounter("BILLINGADD") Else tranId = dr("TRANID")
            Save(tranId, Convert.ReplaceDBNull(dr("TRANTYPE")), Convert.ReplaceDBNull(dr("REFTRANID")), Convert.ReplaceDBNull(dr("CONSIGNEE")), units, Convert.ReplaceDBNull(dr("UNITSTYPE")), Convert.ReplaceDBNull(dr("ISTOTALAMOUNT")), price, Convert.ReplaceDBNull(dr("TRANDATE")), Common.GetCurrentUser)
        End If
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetBillingAdditional(ByVal pTranId As String) As BillingAdditional
        Return New BillingAdditional(pTranId)
    End Function

    Public Shared Function Exists(ByVal pTranId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from BILLINGADDITIONAL where TranId = '{0}'", pTranId)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGADDITIONAL Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Exit Sub
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("TRANTYPE") Then _trantype = dr.Item("TRANTYPE")
        If Not dr.IsNull("REFTRANID") Then _reftranid = dr.Item("REFTRANID")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("UNITS") Then _units = dr.Item("UNITS")
        If Not dr.IsNull("UNITSTYPE") Then _unitstype = dr.Item("UNITSTYPE")
        If Not dr.IsNull("ISTOTALAMOUNT") Then _istotalamount = dr.Item("ISTOTALAMOUNT")
        If Not dr.IsNull("PRICE") Then _price = dr.Item("PRICE")
        If Not dr.IsNull("TRANDATE") Then _trandate = dr.Item("TRANDATE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

    End Sub

    Public Sub Save(ByVal pTranId As String, ByVal pTranType As String, ByVal pRefTranId As String, ByVal pConsignee As String, ByVal pUnits As Double, ByVal pUnitsType As String, ByVal pIsTotalAmount As Boolean, ByVal pPrice As Double, ByVal pTranDate As DateTime, ByVal pUser As String)
        Dim SQL As String

        If Not WMS.Logic.Consignee.Exists(pConsignee) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid Consignee", "Invalid Consignee")
            Throw m4nEx
        End If

        _tranid = pTranId
        _trantype = pTranType
        _reftranid = pRefTranId
        _consignee = pConsignee
        _units = pUnits
        _unitstype = pUnitsType
        _istotalamount = pIsTotalAmount
        _price = pPrice
        _trandate = pTranDate
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        If WMS.Logic.BillingAdditional.Exists(_tranid) Then

            SQL = String.Format("UPDATE BILLINGADDITIONAL SET TRANTYPE ={0}, REFTRANID ={1}, CONSIGNEE ={2}, UNITS ={3}, UNITSTYPE ={4}, ISTOTALAMOUNT ={5}, PRICE ={6}, TRANDATE ={7}, EDITDATE ={8}, EDITUSER ={9}  where {10}", _
                   Made4Net.Shared.Util.FormatField(_trantype), Made4Net.Shared.Util.FormatField(_reftranid), Made4Net.Shared.Util.FormatField(_consignee), _
                    Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_unitstype), Made4Net.Shared.Util.FormatField(_istotalamount), Made4Net.Shared.Util.FormatField(_price), _
                    Made4Net.Shared.Util.FormatField(_trandate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        Else

            SQL = String.Format("INSERT INTO BILLINGADDITIONAL (TRANID, TRANTYPE, REFTRANID, CONSIGNEE, UNITS, UNITSTYPE, ISTOTALAMOUNT, PRICE, TRANDATE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                    "VALUES({0},{1} ,{2} ,{3} ,{4} ,{5} ,{6} ,{7} ,{8} ,{9} ,{10} ,{11} ,{12} )", _
                    Made4Net.Shared.Util.FormatField(_tranid), Made4Net.Shared.Util.FormatField(_trantype), Made4Net.Shared.Util.FormatField(_reftranid), Made4Net.Shared.Util.FormatField(_consignee), _
                    Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_unitstype), Made4Net.Shared.Util.FormatField(_istotalamount), Made4Net.Shared.Util.FormatField(_price), _
                    Made4Net.Shared.Util.FormatField(_trandate), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

End Class
