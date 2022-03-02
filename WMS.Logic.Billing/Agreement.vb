Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

#Region "AGREEMENT"

' <summary>
' This object represents the properties and methods of a BILLINGAGREEMENTHEADER.
' </summary>

Public Class Agreement

#Region "Variables"

#Region "Primary Keys"

    Protected _name As String = String.Empty
    Protected _consignee As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _description As String = String.Empty
    Protected _status As String = String.Empty
    Protected _minvalue As Double = 0
    Protected _maxvalue As Double
    Protected _periodtype As PeriodTypes
    Protected _pricefactor As Double
    Protected _pricefactorindex As String
    Protected _period As String
    Protected _startdate As DateTime
    Protected _enddate As DateTime
    Protected _lastrundate As DateTime
    Protected _nextrundate As DateTime
    Protected _active As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _agreementdetails As AgreementDetailCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" NAME = {0} And Consignee = {1} ", Made4Net.Shared.Util.FormatField(_name), Made4Net.Shared.Util.FormatField(_consignee))
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property Consignee() As String
        Get
            Return _consignee
        End Get
    End Property

    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal Value As String)
            _description = Value
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
    'Price Index for converting the total price of the agreement
    Public Property PriceFactor() As Double
        Get
            If _pricefactor = 0 Then Return 1
            Try
                Return _pricefactor
            Catch ex As Exception
                Return 1
            End Try
        End Get
        Set(ByVal Value As Double)
            _pricefactor = Value
        End Set
    End Property
    Public Property PriceFactorIndex() As Double
        Get
            Return CType(_pricefactorindex, Double)
        End Get
        Set(ByVal Value As Double)
            _pricefactorindex = CType(Value, String)
        End Set
    End Property


    Public Property MinimumValue() As Double
        Get
            Return _minvalue
        End Get
        Set(ByVal Value As Double)
            _minvalue = Value
        End Set
    End Property

    Public Property MaximumValue() As Double
        Get
            Return _maxvalue
        End Get
        Set(ByVal Value As Double)
            _maxvalue = Value
        End Set
    End Property

    Public Property PeriodType() As PeriodTypes
        Get
            Return _periodtype
        End Get
        Set(ByVal Value As PeriodTypes)
            _periodtype = Value
        End Set
    End Property

    Public Property Period() As String
        Get
            Return _period
        End Get
        Set(ByVal Value As String)
            _period = Value
        End Set
    End Property

    Public Property EndDate() As DateTime
        Get
            Return _enddate
        End Get
        Set(ByVal Value As DateTime)
            _enddate = Value
        End Set
    End Property

    Public Property StartDate() As DateTime
        Get
            Return _startdate
        End Get
        Set(ByVal Value As DateTime)
            _startdate = Value
        End Set
    End Property

    Public Property LastRunDate() As DateTime
        Get
            Return _lastrundate
        End Get
        Set(ByVal Value As DateTime)
            _lastrundate = Value
        End Set
    End Property

    Public Property NextRunDate() As DateTime
        Get
            Return _nextrundate
        End Get
        Set(ByVal Value As DateTime)
            _nextrundate = Value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _active
        End Get
        Set(ByVal Value As Boolean)
            _active = Value
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

    Public ReadOnly Property Lines() As AgreementDetailCollection
        Get
            Return _agreementdetails
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pConsignee As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pConsignee
        Dim pName As String
        'choose agreement
        Dim sql As String
        sql = String.Format("SELECT TOP (1) NAME FROM dbo.BILLINGAGREEMENTHEADER WHERE CONSIGNEE='{0}' and ACTIVE = 1 ORDER BY NAME", pConsignee)
        Try
            pName = CType(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql), String)
            If Not String.IsNullOrEmpty(pName) Then
                _name = pName
                _consignee = pConsignee
                If LoadObj Then
                    Load()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub New(ByVal pName As String, ByVal pConsignee As String, Optional ByVal LoadObj As Boolean = True)
        _name = pName
        _consignee = pConsignee
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Try
            dr = ds.Tables(0).Rows(0)
        Catch ex As Exception

        End Try
        Select Case CommandName.ToLower
            '        Case "insertagreementheader"
            '            Save(dr("consignee"), dr("name"), Convert.ReplaceDBNull(dr("description"), ""), Convert.ReplaceDBNull(dr("minvalue")), Convert.ReplaceDBNull(dr("maxvalue")), _
            '                BillingUtils.PeriodTypesFromString(Convert.ReplaceDBNull(dr("periodtype"))), Convert.ReplaceDBNull(dr("period")), Convert.ReplaceDBNull(dr("startdate")), _
            '                Convert.ReplaceDBNull(dr("active")), Common.GetCurrentUser(), Convert.ReplaceDBNull(dr("pricefactorindex")), Convert.ReplaceDBNull(dr("enddate")))
            '        Case "updateagreementheader"
            '            _consignee = dr("consignee")
            '            _name = dr("name")
            '            Load()
            '            Save(dr("consignee"), dr("name"), Convert.ReplaceDBNull(dr("description"), ""), Convert.ReplaceDBNull(dr("minvalue")), Convert.ReplaceDBNull(dr("maxvalue")), _
            '                BillingUtils.PeriodTypesFromString(Convert.ReplaceDBNull(dr("periodtype"))), Convert.ReplaceDBNull(dr("period")), Convert.ReplaceDBNull(dr("startdate")), _
            '                Convert.ReplaceDBNull(dr("active")), Common.GetCurrentUser(), Convert.ReplaceDBNull(dr("pricefactorindex")), Convert.ReplaceDBNull(dr("enddate")))
            '        Case "addagreementline"
            '            _name = dr("agreementname")
            '            _consignee = dr("consignee")
            '            Load()
            '            AddAgreementLine(dr("line"), AgreementDetail.TransactionTypeFromString(Convert.ReplaceDBNull(dr("trantype"))), Convert.ReplaceDBNull(dr("documenttype")), Convert.ReplaceDBNull(dr("uom")), _
            '                AgreementDetail.BillingBasisFromString(Convert.ReplaceDBNull(dr("billbasis"))), Convert.ReplaceDBNull(dr("priceperunit")), Convert.ReplaceDBNull(dr("ispercentage")), Convert.ReplaceDBNull(dr("percentage")), Convert.ReplaceDBNull(dr("usepricelist")), _
            '                Convert.ReplaceDBNull(dr("pricelist")), Convert.ReplaceDBNull(dr("currency")), Convert.ReplaceDBNull(dr("minpertran")), _
            '                Convert.ReplaceDBNull(dr("maxpertran")), Convert.ReplaceDBNull(dr("minperrun")), Convert.ReplaceDBNull(dr("maxperrun")), _
            '                BillingUtils.PeriodTypesFromString(Convert.ReplaceDBNull(dr("periodtype"))), Convert.ReplaceDBNull(dr("period")), Convert.ReplaceDBNull(dr("startdate")), Convert.ReplaceDBNull(dr("active")), Common.GetCurrentUser(), _
            '                Convert.ReplaceDBNull(dr("pricefactorindex")), Convert.ReplaceDBNull(dr("skugroup")), Convert.ReplaceDBNull(dr("enddate")), _
            '                Convert.ReplaceDBNull(dr("storageperiodtime")), BillingUtils.PeriodTypesFromString(Convert.ReplaceDBNull(dr("storageperiodtype"))), Convert.ReplaceDBNull(dr("runcondition")), _
            '                Convert.ReplaceDBNull(dr("chargedescription")), "", Convert.ReplaceDBNull(dr("STORAGEPARTIALPERIOD")), Convert.ReplaceDBNull(dr("ISSTORAGERANGE")), Convert.ReplaceDBNull(dr("trantype")))
            Case "bill"
                For Each dr In ds.Tables(0).Rows
                    _name = CType(dr("name"), String)
                    _consignee = CType(dr("consignee"), String)
                    ' Load()
                    PostToBillQ(WMS.Logic.Common.GetCurrentUser())
                Next
                Message = WMS.Logic.Utils.TranslateMessage("Processing Agreements", Nothing)
        End Select
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Protected Function Exists() As Boolean
        Dim sql As String
        sql = String.Format("SELECT COUNT(1) FROM BILLINGAGREEMENTHEADER WHERE {0}", WhereClause)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGAGREEMENTHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Agreement does not exists", "Agreement does not exists")
            m4nEx.Params.Add("consignee", _consignee)
            m4nEx.Params.Add("name", _name)
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("DESCRIPTION") Then _description = CType(dr.Item("DESCRIPTION"), String)
        If Not dr.IsNull("STATUS") Then _status = CType(dr.Item("STATUS"), String)
        If Not dr.IsNull("MINVALUE") Then _minvalue = CType(dr.Item("MINVALUE"), Double)
        If Not dr.IsNull("MAXVALUE") Then _maxvalue = CType(dr.Item("MAXVALUE"), Double)
        ''        If Not dr.IsNull("PRICEFACTOR") Then _pricefactor = dr.Item("PRICEFACTOR")
        If Not dr.IsNull("PRICEFACTORINDEX") Then _pricefactorindex = CType(dr.Item("PRICEFACTORINDEX"), String)

        If Not dr.IsNull("PERIODTYPE") Then _periodtype = BillingUtils.PeriodTypesFromString(CType(dr.Item("PERIODTYPE"), String))
        If Not dr.IsNull("PERIOD") Then _period = CType(dr.Item("PERIOD"), String)
        If Not dr.IsNull("STARTDATE") Then _startdate = CType(dr.Item("STARTDATE"), Date)
        If Not dr.IsNull("LASTRUNDATE") Then _lastrundate = CType(dr.Item("LASTRUNDATE"), Date)
        If Not dr.IsNull("NEXTRUNDATE") Then _nextrundate = CType(dr.Item("NEXTRUNDATE"), Date)
        If Not dr.IsNull("ENDDATE") Then _enddate = CType(dr.Item("ENDDATE"), Date)
        If Not dr.IsNull("ACTIVE") Then _active = CType(dr.Item("ACTIVE"), Boolean)
        If Not dr.IsNull("ADDDATE") Then _adddate = CType(dr.Item("ADDDATE"), Date)
        If Not dr.IsNull("ADDUSER") Then _adduser = CType(dr.Item("ADDUSER"), String)
        If Not dr.IsNull("EDITDATE") Then _editdate = CType(dr.Item("EDITDATE"), Date)
        If Not dr.IsNull("EDITUSER") Then _edituser = CType(dr.Item("EDITUSER"), String)

        '_agreementdetails = New AgreementDetailCollection(Me)

    End Sub

    Protected Sub setNextRunDate()
        Dim LastDate As Date = _nextrundate
        _lastrundate = _nextrundate
        _nextrundate = BillingUtils.getNextRunDate(_periodtype, LastDate, _period)
    End Sub

#End Region

#Region "Process"

    'Public Sub process(ByVal ProcKey As String, ByVal Logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String)
    '    Logger.Write("Processing Agreement For Consignee: " & _consignee)
    '    Logger.Write("Agreement Name: " & _name & ", Agreement Desc: " & _description)
    '    Dim EndRunDate As DateTime

    '    If _enddate <> DateTime.MinValue Then
    '        EndRunDate = _enddate
    '    Else
    '        EndRunDate = DateTime.Now.Date
    '    End If

    '    If (Not _active) Then
    '        Logger.Write("Skipping Agreement - Agreement Not Active")
    '        Logger.writeSeperator()
    '        Logger.writeSeperator()
    '        Return
    '    End If
    '    If (_nextrundate.Date > EndRunDate) Then
    '        Logger.Write("Skipping Agreement - Next Run Date is: " & _nextrundate.ToString("dd-MM-yyyy"))
    '        Logger.writeSeperator()
    '        Logger.writeSeperator()
    '        Return
    '    End If
    '    Dim inv As DataTable
    '    While (_nextrundate.Date <= EndRunDate)
    '        Logger.writeSeperator()
    '        Logger.Write("Processing Agreement From Date: " & _lastrundate.ToString("dd-MM-yyyy") & " To Date: " & _nextrundate.ToString("dd-MM-yyyy"))
    '        Dim AggValue As Decimal = 0
    '        Logger.Write("Creating New Charge Header...")
    '        Dim ch As New ChargeHeader(ProcKey, Me)
    '        ch.BillFromDate = _lastrundate
    '        ch.BillToDate = _nextrundate
    '        Logger.Write("Processing Agreement Details")
    '        Logger.Write("Number Of Details: " & _agreementdetails.Count)
    '        Dim adb As AgreementDetail
    '        For Each adb In _agreementdetails
    '            Try
    '                Logger.writeSeperator()
    '                Logger.Write("Agreement Detail Line: " & adb.LineNumber)
    '                ch.AddCharges(adb.Process(_nextrundate.Date, Logger, pUser, SaveStorageTransactions, ch))
    '            Catch ex As Made4Net.Shared.M4NException
    '                Logger.writeSeperator()
    '                Logger.Write("Error Occured: " & ex.GetErrMessage(0))
    '            Catch ex As Exception
    '                Logger.writeSeperator()
    '                Logger.Write("Error Occured: " & ex.ToString())
    '            End Try
    '        Next
    '        Logger.Write("Agreement Value: " & ch.ChargeDetails.LinesValue.ToString)
    '        Logger.Write("Agreement Minimum Value: " & _minvalue)
    '        Logger.Write("Agreement Maximum Value: " & _maxvalue)


    '        ''calc price factor
    '        If Not Logger Is Nothing Then
    '            Logger.Write("Calculating price factor...")
    '        End If
    '        Dim oPriceFactorCalc As New PriceFactorCalc
    '        _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
    '            _lastrundate.ToString("dd/MM/yyyy"), Logger).ToString()
    '        If Not Logger Is Nothing Then
    '            Logger.Write("Price Factor:" & _pricefactor)
    '            Logger.Write("Last run date:" & _lastrundate.ToString("dd/MM/yyyy"))
    '        End If
    '        ''---

    '        If (ch.ChargeDetails.LinesValue < _minvalue) Then
    '            ch.BillTotal = _minvalue * Me.PriceFactor
    '        ElseIf (ch.ChargeDetails.LinesValue > _maxvalue And _maxvalue > 0) Then
    '            ch.BillTotal = _maxvalue * Me.PriceFactor
    '        Else
    '            ch.BillTotal = ch.ChargeDetails.LinesValue * Me.PriceFactor
    '        End If


    '        Logger.Write("LinesValue: " & ch.ChargeDetails.LinesValue.ToString())
    '        Logger.Write("BillTotal: " & ch.BillTotal.ToString())


    '        Logger.Write("Agreement New Value: " & AggValue)
    '        Logger.Write("Start Posting Charge")
    '        ch.AgreementName = Me.Name
    '        ch.Post(pUser)
    '        Logger.Write("Finished Posting Charge")
    '        setNextRunDate()


    '        Save(pUser)

    '        Logger.Write("Finished Processing Agreement")
    '        Logger.writeSeperator()
    '    End While

    '    For Each adb As AgreementDetail In _agreementdetails
    '        'adb.NextRunDate = _nextrundate
    '        'adb.LastRunDate = _lastrundate '.AddDays(-1)
    '        adb.Save(adb.AddUser)
    '    Next
    '    Logger.Write("Finished Agreement Run!")
    '    Logger.writeSeperator()
    'End Sub
#End Region

#Region "Save"

    'Private Sub Save(ByVal pConsignee As String, ByVal pAgreementName As String, ByVal pDescription As String, ByVal pMinvalue As Double, ByVal pMaxvalue As Double, _
    '        ByVal pPeriodtype As PeriodTypes, ByVal pPeriod As String, ByVal pStartdate As DateTime, _
    '        ByVal pActive As Boolean, ByVal pUser As String, ByVal pPriceFactorIndex As String, _
    '        ByVal pEndDate As DateTime)

    '    'Dim agreementHeader As Agreement
    '    Dim SQL As String = String.Empty

    '    If Not WMS.Logic.Consignee.Exists(pConsignee) Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create Agreement", "Can't create Agreement")
    '        Throw m4nEx
    '    Else
    '        _consignee = pConsignee
    '    End If
    '    _name = pAgreementName
    '    _description = pDescription
    '    _minvalue = pMinvalue
    '    _maxvalue = pMaxvalue
    '    _pricefactorindex = pPriceFactorIndex
    '    _enddate = pEndDate
    '    _startdate = pStartdate
    '    Dim periodchanged As Boolean = False

    '    If _periodtype <> pPeriodtype Or _period <> pPeriod Then
    '        periodchanged = True
    '    End If
    '    _periodtype = pPeriodtype
    '    _period = pPeriod

    '    If Not Exists() Then
    '        _lastrundate = pStartdate
    '        _nextrundate = BillingUtils.getNextRunDate(_periodtype, _lastrundate, _period)
    '    Else
    '        ' Agreement header exist then change NextRanDate only if period, periodtype was changed
    '        If periodchanged Then
    '            _nextrundate = BillingUtils.getNextRunDate(_periodtype, _lastrundate, _period)
    '        End If
    '    End If

    '    _status = "Normal"
    '    _active = pActive
    '    Save(pUser)
    'End Sub

    'Public Sub Save(ByVal sUserId As String)
    '    Dim sql As String
    '    _editdate = DateTime.Now
    '    _edituser = sUserId
    '    If Exists() Then
    '        sql = String.Format("UPDATE BILLINGAGREEMENTHEADER SET DESCRIPTION={0}, STATUS={1} ,MINVALUE={2}, MAXVALUE={3}, PERIODTYPE={4} ,PERIOD={5} ,STARTDATE={6}, " & _
    '            "LASTRUNDATE={7} ,NEXTRUNDATE ={8},ACTIVE={9},EDITDATE={10} ,EDITUSER={11}, PRICEFACTORINDEX={12}, ENDDATE={13} where {14}", _
    '            Made4Net.Shared.Util.FormatField(_description), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_minvalue), _
    '            Made4Net.Shared.Util.FormatField(_maxvalue), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_periodtype)), _
    '            Made4Net.Shared.Util.FormatField(_period), Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_lastrundate), _
    '            Made4Net.Shared.Util.FormatField(_nextrundate), Made4Net.Shared.Util.FormatField(_active), _
    '            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_pricefactorindex), Made4Net.Shared.Util.FormatField(_enddate), WhereClause)
    '        DataInterface.RunSQL(sql)

    '        Dim em As New EventManagerQ
    '        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.AgreementCreated
    '        em.Add("EVENT", EventType)
    '        em.Add("AGREEMENT", _name)
    '        em.Add("CONSIGNEE", _consignee)
    '        em.Add("USERID", _adduser)
    '        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '        em.Send(WMSEvents.EventDescription(EventType))
    '    Else
    '        _adddate = DateTime.Now
    '        _adduser = sUserId
    '        sql = String.Format("INSERT INTO BILLINGAGREEMENTHEADER(NAME ,CONSIGNEE ,DESCRIPTION, STATUS ,MINVALUE, MAXVALUE, PERIODTYPE ,PERIOD ,STARTDATE, " & _
    '            "LASTRUNDATE ,NEXTRUNDATE ,ACTIVE ,ADDDATE ,ADDUSER ,EDITDATE ,EDITUSER, PRICEFACTORINDEX, ENDDATE) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17})", _
    '            Made4Net.Shared.Util.FormatField(_name), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_description), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_minvalue), _
    '            Made4Net.Shared.Util.FormatField(_maxvalue), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_periodtype)), Made4Net.Shared.Util.FormatField(_period), _
    '            Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_lastrundate), Made4Net.Shared.Util.FormatField(_nextrundate), _
    '            Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), _
    '            Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_pricefactorindex), Made4Net.Shared.Util.FormatField(_enddate))
    '        DataInterface.RunSQL(sql)

    '        Dim em As New EventManagerQ
    '        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.AgreementUpdated
    '        em.Add("EVENT", EventType)
    '        em.Add("AGREEMENT", _name)
    '        em.Add("CONSIGNEE", _consignee)
    '        em.Add("USERID", _adduser)
    '        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '        em.Send(WMSEvents.EventDescription(EventType))

    '        _agreementdetails = New AgreementDetailCollection(Me)
    '    End If

    '    _agreementdetails.Save(sUserId)
    'End Sub

#End Region

#Region "Add/Update Lines"

    Private Sub AddAgreementLine(ByVal pLine As Int32, ByVal pTrantype As AgreementDetail.TransactionTypes, ByVal pDocumenttype As String _
            , ByVal pUom As String, ByVal pBillBasis As AgreementDetail.BillingBasis, ByVal pUnitPrice As Double, ByVal pIsPerc As Boolean, ByVal pPerc As Double, ByVal pUserPrice As Boolean _
            , ByVal pPriceList As String, ByVal pCurrency As String, ByVal pMinPerTran As Double, ByVal pMaxPerTran As Double, ByVal pMinPerRun As Double, ByVal pMaxPerRun As Double _
            , ByVal pPeriodType As PeriodTypes, ByVal pPeriod As String, ByVal pStartdate As DateTime, ByVal pActive As Boolean, ByVal pUser As String, ByVal pPriceFactorindex As String, ByVal pSkuGroup As String, ByVal pEndDate As DateTime _
            , ByVal pStoragePeriodTime As Int32, ByVal pStoragePeriodType As PeriodTypes, ByVal pRunCondition As String, ByVal pChargeDescription As String _
            , ByVal pHandlingUnitType As String, ByVal pStoragePartialPeriod As Boolean, ByVal pIsStorageRange As Boolean, Optional ByVal AdditionalTranType As String = Nothing)

        _agreementdetails.SaveLine(pLine, pTrantype, pDocumenttype, pUom, pBillBasis, pUnitPrice, _
                pIsPerc, pPerc, pUserPrice, pPriceList, pCurrency, pMinPerTran, _
                pMaxPerTran, pMinPerRun, pMaxPerRun, pPeriodType, pPeriod, pStartdate, pActive, pUser, _
                pPriceFactorindex, pSkuGroup, pEndDate, pStoragePeriodTime, pStoragePeriodType, pRunCondition, pChargeDescription, _
                pHandlingUnitType, pStoragePartialPeriod, pIsStorageRange, AdditionalTranType)

    End Sub
#End Region

    Public Sub PostToBillQ(ByVal pUser As String)
        Dim a As New Made4Net.Shared.QMsgSender
        a.Add("USERID", pUser)
        a.Add("CONSIGNEE", _consignee)
        a.Add("AGREEMENT", _name)
        a.Send("BillingProc")

        'a.Send("BILLINGPROC")

    End Sub

#End Region

End Class

#End Region

#Region "Enums"

Public Enum PeriodTypes
    None
    Day
    Week
    Month
    Year
    Dates
End Enum

#End Region