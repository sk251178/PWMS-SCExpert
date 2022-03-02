Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "BILLINGCHARGESDETAIL"

' <summary>
' This object represents the properties and methods of a BILLINGCHARGESDETAIL.
' </summary>

Public Class ChargeDetail

#Region "Variables"

#Region "Primary Keys"

    Protected _chargeheader As ChargeHeader
    Protected _chargeline As Int32

#End Region

#Region "Other Fields"

    Protected _agreementname As String
    Protected _agreementline As Int32
    Protected _units As Double
    Protected _billtotal As Double
    Protected _currency As String = String.Empty
    Protected _billfromdate As DateTime
    Protected _billtodate As DateTime
    Protected _status As String
    Protected _chargetext As String
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" ChargeId = '{0}' and chargeLine = {1} ", _chargeheader.CHARGEID, _chargeline)
        End Get
    End Property

    Public Property ChargeHeader() As ChargeHeader
        Get
            Return _chargeheader
        End Get
        Set(ByVal Value As ChargeHeader)
            _chargeheader = Value
        End Set
    End Property

    Public Property ChargeLine() As Int32
        Get
            Return _chargeline
        End Get
        Set(ByVal Value As Int32)
            _chargeline = Value
        End Set
    End Property

    Public Property AgreementName() As String
        Get
            Return _agreementname
        End Get
        Set(ByVal Value As String)
            _agreementname = Value
        End Set
    End Property

    Public Property AgreementLine() As Int32
        Get
            Return _agreementline
        End Get
        Set(ByVal Value As Int32)
            _agreementline = Value
        End Set
    End Property

    Public Property BilledUnits() As Double
        Get
            Return _units
        End Get
        Set(ByVal Value As Double)
            _units = Value
        End Set
    End Property

    Public Property BillTotal() As Double
        Get
            Return _billtotal
        End Get
        Set(ByVal Value As Double)
            _billtotal = Value
        End Set
    End Property

    Public Property Currency() As String
        Get
            Return _currency
        End Get
        Set(ByVal Value As String)
            _currency = Value
        End Set
    End Property

    Public Property BillFromDate() As DateTime
        Get
            Return _billfromdate
        End Get
        Set(ByVal Value As DateTime)
            _billfromdate = Value
        End Set
    End Property

    Public Property BillToDate() As DateTime
        Get
            Return _billtodate
        End Get
        Set(ByVal Value As DateTime)
            _billtodate = Value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property ChargeText() As String
        Get
            Return _chargetext
        End Get
        Set(ByVal Value As String)
            _chargetext = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
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

    Public Sub New(ByRef oChargeHeader As ChargeHeader, ByVal iChargeLine As Int32)
        _chargeline = iChargeLine
        Load()
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGCHARGESDETAIL WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Charge Line does not exists", "Charge Line does not exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("AGREEMENTNAME") Then _agreementname = dr.Item("AGREEMENTNAME")
        If Not dr.IsNull("AGREEMENTLINE") Then _agreementline = dr.Item("AGREEMENTLINE")
        If Not dr.IsNull("UNITS") Then _units = dr.Item("UNITS")
        If Not dr.IsNull("BILLTOTAL") Then _billtotal = dr.Item("BILLTOTAL")
        If Not dr.IsNull("CURRENCY") Then _currency = dr.Item("CURRENCY")
        If Not dr.IsNull("BILLFROMDATE") Then _billfromdate = dr.Item("BILLFROMDATE")
        If Not dr.IsNull("BILLTODATE") Then _billtodate = dr.Item("BILLTODATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("CHARGETEXT") Then _chargetext = dr.Item("CHARGETEXT")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

    End Sub

    Public Function GetNextLineId(ByVal sChargeID As String) As Integer
        Return Convert.ToInt32(DataInterface.ExecuteScalar("SELECT ISNULL(MAX(CHARGELINE),0) + 1 FROM BILLINGCHARGESDETAIL WHERE CHARGEID='" & sChargeID & "'"))
    End Function

    Public Sub post(ByVal puser As String)
        _adddate = DateTime.Now
        _adduser = puser
        _editdate = DateTime.Now
        _edituser = puser
        Dim sql As String
        sql = String.Format("INSERT INTO BILLINGCHARGESDETAIL(CHARGEID,CHARGELINE,AGREEMENTNAME, AGREEMENTLINE," & _
            "UNITS,BILLTOTAL,CURRENCY,BILLFROMDATE,BILLTODATE, STATUS, CHARGETEXT, ADDDATE, ADDUSER,EDITDATE,EDITUSER) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9}," & _
            "{10},{11},{12},{13},{14})", Made4Net.Shared.Util.FormatField(_chargeheader.ChargeId), Made4Net.Shared.Util.FormatField(_chargeline), _
            Made4Net.Shared.Util.FormatField(_agreementname), Made4Net.Shared.Util.FormatField(_agreementline), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_billtotal), _
            Made4Net.Shared.Util.FormatField(_currency), Made4Net.Shared.Util.FormatField(_billfromdate), Made4Net.Shared.Util.FormatField(_billtodate), _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_chargetext), _
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(puser), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(puser))
        DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class

#End Region

#Region "CHARGEDETAILS COLLECTION"

Public Class ChargeDetailCollection
    Inherits ArrayList

#Region "Variables"

    Protected _chargeheader As ChargeHeader

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property item(ByVal index As Int32) As ChargeDetail
        Get
            Return CType(MyBase.Item(index), ChargeDetail)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal iLineNumber As Int32) As ChargeDetail
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If item(i).ChargeLine = iLineNumber Then
                    Return (CType(MyBase.Item(i), ChargeDetail))
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property LinesValue() As Double
        Get
            Dim cval As Double = 0
            For Each cd As ChargeDetail In Me
                cval = cval + cd.BillTotal
            Next
            Return cval
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByRef oChargeHeader As ChargeHeader)
        Dim sql As String
        Dim dt As New DataTable
        Dim dr As DataRow
        _chargeheader = oChargeHeader
        sql = "Select CHARGELINE from BILLINGCHARGESDETAIL where CHARGEID = '" & _chargeheader.CHARGEID & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New ChargeDetail(_chargeheader, dr.Item("CHARGELINE")))
        Next
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As ChargeDetail) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As ChargeDetail)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As ChargeDetail)
        MyBase.Remove(pObject)
    End Sub

    Public Sub Post(ByVal sUserId As String)
        Dim idx As Int32 = 1
        For Each cd As ChargeDetail In Me
            If cd.BillTotal > 0 Then
                cd.ChargeLine = idx
                cd.post(sUserId)
            End If
            idx = idx + 1
        Next
    End Sub

#End Region

End Class

#End Region

#Region "BILLINGCHARGESHEADER"

' <summary>
' This object represents the properties and methods of a BILLINGCHARGESHEADER.
' </summary>

Public Class ChargeHeader

#Region "Variables"

#Region "Primary Keys"

    Protected _chargeid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _billingrunid As String = String.Empty
    Protected _agreement As Agreement
    Protected _agreementname As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _billfromdate As DateTime
    Protected _billtodate As DateTime
    Protected _billtotal As Decimal
    Protected _status As String
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _chargedetails As ChargeDetailCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CHARGEID = '" & _chargeid & "' "
        End Get
    End Property

    Public ReadOnly Property ChargeDetails() As ChargeDetailCollection
        Get
            Return _chargedetails
        End Get
    End Property

    Public Property ChargeId() As String
        Get
            Return _chargeid
        End Get
        Set(ByVal Value As String)
            _chargeid = Value
        End Set
    End Property

    Public Property BillingRunId() As String
        Get
            Return _billingrunid
        End Get
        Set(ByVal Value As String)
            _billingrunid = Value
        End Set
    End Property

    Public Property BillingAgreement() As Agreement
        Get
            Return _agreement
        End Get
        Set(ByVal Value As Agreement)
            _agreement = Value
        End Set
    End Property

    Public Property AgreementName() As String
        Set(ByVal value As String)
            _agreementname = value
        End Set
        Get
            Return _agreementname
        End Get
    End Property

    Public Property Consignee() As String
        Set(ByVal value As String)
            _consignee = value
        End Set
        Get
            Return _consignee
        End Get
    End Property

    Public Property BillFromDate() As DateTime
        Get
            Return _billfromdate
        End Get
        Set(ByVal Value As DateTime)
            _billfromdate = Value
        End Set
    End Property

    Public Property BillToDate() As DateTime
        Get
            Return _billtodate
        End Get
        Set(ByVal Value As DateTime)
            _billtodate = Value
        End Set
    End Property

    Public Property BillTotal() As Decimal
        Get
            Return _billtotal
        End Get
        Set(ByVal Value As Decimal)
            _billtotal = Value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal sChargeId As String)
        _chargeid = sChargeId
        Load()
    End Sub

    Public Sub New(ByVal sBillingRunId As String, ByVal sConsignee As String, ByVal sAgreementName As String)
        _billingrunid = sBillingRunId
        _agreement = New Agreement(sAgreementName, sConsignee)
        _consignee = _agreement.Consignee
        setChargeId()
        _chargedetails = New ChargeDetailCollection(Me)
    End Sub

    Public Sub New(ByVal sBillingRunId As String, ByRef oAgreement As Agreement)
        _billingrunid = sBillingRunId
        _agreement = oAgreement
        _consignee = oAgreement.Consignee
        setChargeId()
        _chargedetails = New ChargeDetailCollection(Me)
    End Sub

    Public Sub New(ByVal sBillingRunId As String, ByVal bCreateLines As Boolean)
        _billingrunid = sBillingRunId
        setChargeId()
        If bCreateLines Then _chargedetails = New ChargeDetailCollection(Me)
    End Sub

#End Region

#Region "Methods"

    Protected Sub setChargeId()
        _chargeid = Made4Net.Shared.Util.getNextCounter("CHARGES")
    End Sub

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGCHARGESHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Charge does not exists", "Charge does not exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("BILLINGRUNID") Then _billingrunid = dr.Item("BILLINGRUNID")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")

        'If Not dr.IsNull("AGREEMENTNAME") Then _agreement = New Agreement(dr.Item("AGREEMENTNAME"), _consignee)
        If Not dr.IsNull("BILLFROMDATE") Then _billfromdate = dr.Item("BILLFROMDATE")
        If Not dr.IsNull("BILLTODATE") Then _billtodate = dr.Item("BILLTODATE")
        If Not dr.IsNull("BILLTOTAL") Then _billtotal = dr.Item("BILLTOTAL")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _chargedetails = New ChargeDetailCollection(Me)

    End Sub

    Public Sub AddCharges(ByVal cdc As ChargeDetailCollection)
        If Not cdc Is Nothing Then
            If cdc.Count > 0 Then
                For Each cd As ChargeDetail In cdc

                    cd.ChargeHeader = Me
                Next
                _chargedetails.AddRange(cdc)
            End If
        End If
    End Sub

    Public Function Post(ByVal pUser As String)
        Dim SQL As String
        Dim cd As ChargeDetail
        _adddate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = pUser
        _adduser = pUser
        'SQL = String.Format("INSERT INTO BILLINGCHARGESHEADER(CHARGEID,BILLINGRUNID,AGREEMENTNAME,CONSIGNEE,BILLFROMDATE,BILLTODATE,BILLTOTAL, STATUS, ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _
        '    Made4Net.Shared.Util.FormatField(_chargeid), Made4Net.Shared.Util.FormatField(_billingrunid), Made4Net.Shared.Util.FormatField(_agreement.Name), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_billfromdate), _
        '    Made4Net.Shared.Util.FormatField(_billtodate), Made4Net.Shared.Util.FormatField(_billtotal), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        SQL = String.Format("INSERT INTO BILLINGCHARGESHEADER(CHARGEID,BILLINGRUNID,AGREEMENTNAME,CONSIGNEE,BILLFROMDATE,BILLTODATE,BILLTOTAL, STATUS, ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _
            Made4Net.Shared.Util.FormatField(_chargeid), Made4Net.Shared.Util.FormatField(_billingrunid), Made4Net.Shared.Util.FormatField(_agreementname), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_billfromdate), _
            Made4Net.Shared.Util.FormatField(_billtodate), Made4Net.Shared.Util.FormatField(_billtotal), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)
        '_chargedetails.Post(pUser)

        Dim aq As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.BillingChargePosted
        aq.Add("EVENT", EventType)
        aq.Add("USERID", pUser)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("CHARGEID", _chargeid)
        'aq.Add("AGREEMENT", _agreement.Name)
        aq.Add("BILLFROMDATE", _billfromdate)
        aq.Add("BILLTODATE", _billtodate)
        aq.Add("BILLINGRUNID", _billingrunid)
        aq.Add("BILLTOTAL", _billtotal)
        aq.Add("CONSIGNEE", _consignee)
        aq.Send("BillingChargePosted")
    End Function

#End Region

End Class

#End Region