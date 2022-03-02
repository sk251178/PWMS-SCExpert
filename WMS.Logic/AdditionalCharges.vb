Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

<CLSCompliant(False)>
Public Class AdditionalCharges

#Region "Variables"
    Protected _tranid As String
    Protected _trantype As String
    Protected _reftranid As String
    Protected _units As Decimal
    Protected _unitstype As String
    Protected _price As Decimal
    Protected _trandate As DateTime
    Protected _status As String
    Protected _consignee As String
    Protected _currency As String
    Protected _amount As Decimal
    Protected _chargetext As String
    Protected _reference_chargeid As String


    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
#End Region

#Region "Properties"

    Public Property TransactionId() As String
        Get
            Return _tranid
        End Get
        Set(ByVal Value As String)
            _tranid = Value
        End Set
    End Property

    Public Property TransacgtionType() As String
        Get
            Return _trantype
        End Get
        Set(ByVal Value As String)
            _trantype = Value
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

    Public Property ReferenceTransactionId() As String
        Get
            Return _reftranid
        End Get
        Set(ByVal Value As String)
            _reftranid = Value
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

    Public Property UnitsType() As String
        Get
            Return _unitstype
        End Get
        Set(ByVal Value As String)
            _unitstype = Value
        End Set
    End Property

    Public Property Price() As Decimal
        Get
            Return _price
        End Get
        Set(ByVal Value As Decimal)
            _price = Value
        End Set
    End Property

    Public Property TransactionDate() As DateTime
        Get
            Return _trandate
        End Get
        Set(ByVal Value As DateTime)
            _trandate = Value
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

    Public Property Currency() As String
        Get
            Return _currency
        End Get
        Set(ByVal Value As String)
            _currency = Value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal Value As Decimal)
            _amount = Value
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

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim dr As DataRow
        Try
            dr = ds.Tables(0).Rows(0)
        Catch ex As Exception

        End Try
        Select Case CommandName.ToLower
            Case "newac"
                _tranid = Made4Net.Shared.Util.getNextCounter("BILLINGADD")
                Post(_tranid, Convert.ReplaceDBNull(dr("TRANTYPE")), Convert.ReplaceDBNull(dr("REFTRANID")), Convert.ReplaceDBNull(dr("UNITS")), Convert.ReplaceDBNull(dr("UNITSTYPE")), Convert.ReplaceDBNull(dr("PRICE")), Convert.ReplaceDBNull(dr("TRANDATE")), "NEW", Convert.ReplaceDBNull(dr("CONSIGNEE")), Convert.ReplaceDBNull(dr("CURRENCY")), Convert.ReplaceDBNull(dr("PRICE")) * Convert.ReplaceDBNull(dr("UNITS")), Convert.ReplaceDBNull(dr("ChargeText")), Convert.ReplaceDBNull(dr("REFERENCE_CHARGEID")))
                'PostCharge()
            Case "editac"
                _tranid = Convert.ReplaceDBNull(dr("TRANID"))
                Load()
                Post(_tranid, Convert.ReplaceDBNull(dr("TRANTYPE")), Convert.ReplaceDBNull(dr("REFTRANID")), Convert.ReplaceDBNull(dr("UNITS")), Convert.ReplaceDBNull(dr("UNITSTYPE")), Convert.ReplaceDBNull(dr("PRICE")), Convert.ReplaceDBNull(dr("TRANDATE")), Convert.ReplaceDBNull(dr("STATUS")), Convert.ReplaceDBNull(dr("CONSIGNEE")), Convert.ReplaceDBNull(dr("CURRENCY")), Convert.ReplaceDBNull(dr("PRICE")) * Convert.ReplaceDBNull(dr("UNITS")), Convert.ReplaceDBNull(dr("ChargeText")), Convert.ReplaceDBNull(dr("REFERENCE_CHARGEID")))
            Case "postac"
                For Each chargedr As DataRow In ds.Tables(0).Rows
                    _tranid = Convert.ReplaceDBNull(dr("TRANID"))
                    Load()
                    If _status.ToUpper = "NEW" Then
                        If Not PostCharge(Nothing) Then
                            Throw New ApplicationException(t.Translate("Can post charges, reference_chargeid is empty"))
                        End If
                        SetStatus("POST")
                    Else
                        Throw New ApplicationException(t.Translate("Can post charges only in new status"))
                    End If
                Next
                Message = "Charge/s Posted"
        End Select
    End Sub
#End Region

#Region "Methods"

    Public Shared Function Exists(ByVal sTransactionId As String)
        Dim sql As String = "SELECT COUNT(1) FROM BILLINGADDITIONAL WHERE TRANID = '" & sTransactionId & "'"
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function PostCharge(ByRef logger As WMS.Logic.LogHandler) As Boolean
        Dim ch As ChargeHeader
        Dim user As String = WMS.Logic.GetCurrentUser
        If String.IsNullOrEmpty(_reference_chargeid) Then Return False

        If Not ChargeHeader.Exist(_reference_chargeid) Then
            ch = New ChargeHeader(Made4Net.Shared.Util.getNextCounter("BILLINGPROC"), True)
            Dim cdc As New ChargeDetailCollection
            ch.CHARGEID = _reference_chargeid
            ch.BILLFROMDATE = _trandate
            ch.BILLTODATE = _trandate
            ch.CONSIGNEE = _consignee
            ch.AGREEMENTNAME = ""
            ch.STATUS = "NEW"
            Dim cd As ChargeDetail = New ChargeDetail
            fillChargeDetail(cd)

            cdc.add(cd)

            ch.AddCharges(cdc)
            ch.BILLTOTAL = ch.CHARGEDETAILS.LinesValue
            ch.Post(user, logger)
        Else
            Dim cd As ChargeDetail = New ChargeDetail

            fillChargeDetail(cd)

            cd.post(user, _reference_chargeid, logger)
            ChargeHeader.SetBillTotal(_reference_chargeid)
        End If

        Return True
    End Function

    Public Sub fillChargeDetail(ByRef cd As ChargeDetail)
        cd.CHARGEID = _reference_chargeid
        cd.AGREEMENTLINE = 1
        cd.BILLFROMDATE = _trandate
        cd.BILLTODATE = _trandate
        cd.CURRENCY = _currency
        cd.TRANSACTIONID = _tranid
        If String.IsNullOrEmpty(_chargetext) Then
            cd.CHARGETEXT = "Additional Charge"
        Else
            cd.CHARGETEXT = _chargetext
        End If

        cd.BILLTOTAL = _amount
        cd.BILLEDUNITS = _units
        cd.STATUS = "NEW"
    End Sub

    Public Sub Post(ByVal stranid As String, _
                            ByVal strantype As String, _
                            ByVal sreftranid As String, _
                            ByVal sunits As Decimal, _
                            ByVal sunitstype As String, _
                            ByVal sprice As Decimal, _
                            ByVal strandate As DateTime, _
                            ByVal sstatus As String, _
                            ByVal sconsignee As String, _
                            ByVal scurrency As String, _
                            ByVal samount As Decimal, _
                            ByVal schargetext As String, _
                            ByVal sreference_chargeid As String)

        _tranid = stranid
        _trantype = strantype
        _reftranid = sreftranid
        _units = sunits
        _unitstype = sunitstype
        _price = sprice
        _trandate = strandate
        _status = sstatus
        _consignee = sconsignee
        _currency = scurrency
        _amount = samount
        _chargetext = schargetext
        _reference_chargeid = sreference_chargeid
        'CHARGETEXT,REFERENCE_CHARGEID
        Post()
    End Sub


    Public Sub SetStatus(ByVal STATUS As String)
        Dim SQL As String
        ' If (Exists(_tranid)) Then
        'Update the old transaction}
        SQL = String.Format("UPDATE BILLINGADDITIONAL SET STATUS ={0}, EDITDATE ={1}, EDITUSER = {2} WHERE TRANID ={3}", _
                             Made4Net.Shared.Util.FormatField(STATUS), _
                             Made4Net.Shared.Util.FormatField(DateTime.Now()), _
                             Made4Net.Shared.Util.FormatField(WMS.Logic.GetCurrentUser), _
                             Made4Net.Shared.Util.FormatField(_tranid))
        DataInterface.RunSQL(SQL)

    End Sub

    Public Sub Post()
        Dim SQL As String
        If (Exists(_tranid)) Then
            'Update the old transaction}
            SQL = String.Format("UPDATE BILLINGADDITIONAL SET TRANTYPE ={0}, REFTRANID ={1}, UNITS ={2}, UNITSTYPE ={3}, PRICE ={4}, TRANDATE ={5}, STATUS ={6}, CONSIGNEE ={7}, CURRENCY ={8}, AMOUNT ={9}, EDITDATE ={10}, EDITUSER = {11},CHARGETEXT = {13}, REFERENCE_CHARGEID = {14} WHERE TRANID ={12}", _
            Made4Net.Shared.Util.FormatField(_trantype), _
            Made4Net.Shared.Util.FormatField(_reftranid), _
            Made4Net.Shared.Util.FormatField(_units), _
            Made4Net.Shared.Util.FormatField(_unitstype), _
            Made4Net.Shared.Util.FormatField(_price), _
            Made4Net.Shared.Util.FormatField(_trandate), _
            Made4Net.Shared.Util.FormatField(_status), _
            Made4Net.Shared.Util.FormatField(_consignee), _
            Made4Net.Shared.Util.FormatField(_currency), _
            Made4Net.Shared.Util.FormatField(_amount), _
            Made4Net.Shared.Util.FormatField(DateTime.Now()), _
            Made4Net.Shared.Util.FormatField(""), _
            Made4Net.Shared.Util.FormatField(_tranid), _
            Made4Net.Shared.Util.FormatField(_chargetext), _
            Made4Net.Shared.Util.FormatField(_reference_chargeid))
        Else
            'Insert new transaction
            SQL = String.Format("INSERT INTO BILLINGADDITIONAL(TRANID, TRANTYPE, REFTRANID, UNITS, UNITSTYPE, PRICE, TRANDATE, STATUS, CONSIGNEE, CURRENCY, AMOUNT, ADDDATE, ADDUSER, EDITDATE, EDITUSER,CHARGETEXT,REFERENCE_CHARGEID) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16})", _
            Made4Net.Shared.Util.FormatField(_tranid), _
            Made4Net.Shared.Util.FormatField(_trantype), _
            Made4Net.Shared.Util.FormatField(_reftranid), _
            Made4Net.Shared.Util.FormatField(_units), _
            Made4Net.Shared.Util.FormatField(_unitstype), _
            Made4Net.Shared.Util.FormatField(_price), _
            Made4Net.Shared.Util.FormatField(_trandate), _
            Made4Net.Shared.Util.FormatField(_status), _
            Made4Net.Shared.Util.FormatField(_consignee), _
            Made4Net.Shared.Util.FormatField(_currency), _
            Made4Net.Shared.Util.FormatField(_amount), _
            Made4Net.Shared.Util.FormatField(DateTime.Now()), _
            Made4Net.Shared.Util.FormatField(""), _
            Made4Net.Shared.Util.FormatField(DateTime.Now()), _
            Made4Net.Shared.Util.FormatField(""), _
            Made4Net.Shared.Util.FormatField(_chargetext), _
            Made4Net.Shared.Util.FormatField(_reference_chargeid))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

    Private Sub Load()
        Dim SQL As String = "SELECT * FROM BILLINGADDITIONAL WHERE TRANID = '" & _tranid & "'"
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        dr = dt.Rows(0)

        If Not dr.IsNull("TRANID") Then _tranid = dr.Item("TRANID")
        If Not dr.IsNull("TRANTYPE") Then _trantype = dr.Item("TRANTYPE")
        If Not dr.IsNull("REFTRANID") Then _reftranid = dr.Item("REFTRANID")
        If Not dr.IsNull("UNITS") Then _units = dr.Item("UNITS")
        If Not dr.IsNull("UNITSTYPE") Then _unitstype = dr.Item("UNITSTYPE")
        If Not dr.IsNull("PRICE") Then _price = dr.Item("PRICE")
        If Not dr.IsNull("TRANDATE") Then _trandate = dr.Item("TRANDATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("CURRENCY") Then _currency = dr.Item("CURRENCY")
        If Not dr.IsNull("AMOUNT") Then _amount = dr.Item("AMOUNT")
        If Not dr.IsNull("CHARGETEXT") Then _chargetext = dr.Item("CHARGETEXT")
        If Not dr.IsNull("REFERENCE_CHARGEID") Then _reference_chargeid = dr.Item("REFERENCE_CHARGEID")

        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

End Class