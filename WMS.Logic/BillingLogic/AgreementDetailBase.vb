Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared.Evaluation

<CLSCompliant(False)>
Public MustInherit Class AgreementDetail

#Region "Enums"

    Public Enum TransactionTypes
        Inbound
        Outbound
        Constant
        Storage
        Additional
        ValueAdded
        woAssembly
        woDisassembly
    End Enum

    Public Enum BillingBasis
        NumberOfLines
        NumberOfSkus
        NumberOfLoads
        NumberOfDucments
        Value
        Units
        Volume
        Weight
        HandlingUnits
    End Enum

    Public Enum RunConditions
        NoCondtition
        EnterExitInLessThenXPeriods
        EnterExitInTheSamePeriod
        EnterExitInMoreThenXPeriods
    End Enum

#End Region

#Region "Variables"

#Region "Primary Keys"
    Protected _line As Int32
#End Region

#Region "Other Fields"

    Protected _agreement As Agreement
    Protected _status As String = String.Empty
    Protected _trantype As TransactionTypes
    Protected _documenttype As String = String.Empty
    Protected _uom As String = String.Empty
    Protected _billbasis As BillingBasis
    Protected _priceperunit As Double
    Protected _ispercentage As Boolean
    Protected _percentage As Double
    Protected _usepricelist As Boolean
    Protected _pricelist As PriceList = Nothing
    Protected _currency As String = String.Empty
    Protected _minpertran As Double = 0
    Protected _maxpertran As Double
    Protected _minperrun As Double = 0
    Protected _maxperrun As Double
    Protected _pricefactor As Double
    Protected _pricefactorindex As String

    Protected _skugroup As String
    Protected _periodtype As PeriodTypes
    Protected _period As String
    Protected _startdate As DateTime
    Protected _enddate As DateTime
    Protected _lastrundate As DateTime
    Protected _nextrundate As DateTime
    Protected _active As Boolean
    Protected _initialstorageline As Int32
    Protected _storageperiodtime As String
    Protected _storageperiodtype As PeriodTypes
    Protected _runcondition As String
    Protected _chargedescription As String
    Protected _hutype As String
    Protected _transportreference As String
    Protected _transporttype As String
    Protected _storagepartialperiod As Boolean

    Protected _periodtype_changed As Boolean
    Protected _period_changed As Boolean
    Protected _nextrundate_changed As Boolean

    ' v3.6.26
    Protected _isstoragerange As Boolean
    Protected _picktype As String
    Protected _classname As String
    Protected _storageclass As String
    'CLASSNAME         STORAGECLASS

    ' System fields declarations
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty


#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" AGREEMENTNAME = {0} AND CONSIGNEE = {1} And LINE = {2}", Made4Net.Shared.Util.FormatField(_agreement.Name), Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(_line))
        End Get
    End Property

    Public Property LineNumber() As Int32
        Get
            Return _line
        End Get
        Set(ByVal Value As Int32)
            _line = Value
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

    Public Property TransactionType() As TransactionTypes
        Get
            Return _trantype
        End Get
        Set(ByVal Value As TransactionTypes)
            _trantype = Value
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

    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property

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

    Public Property PriceFactorIndex() As String
        Get
            Return _pricefactorindex
        End Get
        Set(ByVal Value As String)
            _pricefactorindex = Value
        End Set
    End Property

    Public Property SkuGroup() As String
        Get
            Return _skugroup
        End Get
        Set(ByVal Value As String)
            _skugroup = Value
        End Set
    End Property

    Public Property BILLBASIS() As BillingBasis
        Get
            Return _billbasis
        End Get
        Set(ByVal Value As BillingBasis)
            _billbasis = Value
        End Set
    End Property

    Public Property PRICEPERUNIT() As Double
        Get
            Return _priceperunit
        End Get
        Set(ByVal Value As Double)
            _priceperunit = Value
        End Set
    End Property

    Public Property IsPercentage() As Boolean
        Get
            Return _ispercentage
        End Get
        Set(ByVal Value As Boolean)
            _ispercentage = Value
        End Set
    End Property

    Public Property Percentage() As Double
        Get
            Return _percentage
        End Get
        Set(ByVal Value As Double)
            _percentage = Value
        End Set
    End Property

    Public Property UsePriceList() As Boolean
        Get
            Return _usepricelist
        End Get
        Set(ByVal Value As Boolean)
            _usepricelist = Value
        End Set
    End Property

    Public Property PriceList() As PriceList
        Get
            Return _pricelist
        End Get
        Set(ByVal Value As PriceList)
            _pricelist = Value
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

    Public Property MinimumPerTran() As Double
        Get
            Return _minpertran
        End Get
        Set(ByVal Value As Double)
            _minpertran = Value
        End Set
    End Property

    Public Property MaximumPerTran() As Double
        Get
            Return _maxpertran
        End Get
        Set(ByVal Value As Double)
            _maxpertran = Value
        End Set
    End Property

    Public Property MinimumPerRun() As Double
        Get
            Return _minperrun
        End Get
        Set(ByVal Value As Double)
            _minperrun = Value
        End Set
    End Property

    Public Property MaximumPerRun() As Double
        Get
            Return _maxperrun
        End Get
        Set(ByVal Value As Double)
            _maxperrun = Value
        End Set
    End Property

    Public Property PeriodType() As PeriodTypes
        Get
            Return _periodtype
        End Get
        Set(ByVal Value As PeriodTypes)
            If _periodtype <> Value Then
                _periodtype_changed = True
            End If
            _periodtype = Value
        End Set
    End Property

    Public Property Period() As String
        Get
            Return _period
        End Get
        Set(ByVal Value As String)
            If _period <> Value Then
                _period_changed = True
            End If
            _period = Value
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

    Public Property EndDate() As DateTime
        Get
            Return _enddate
        End Get
        Set(ByVal Value As DateTime)
            _enddate = Value
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
            _nextrundate_changed = True
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

    Public Property StoragePartialPeriod() As Boolean
        Get
            Return _storagepartialperiod
        End Get
        Set(ByVal Value As Boolean)
            _storagepartialperiod = Value
        End Set
    End Property

    Public Property InitialStorageLine() As Int32
        Get
            Return _initialstorageline
        End Get
        Set(ByVal Value As Int32)
            _initialstorageline = Value
        End Set
    End Property

    Public Property StoragePeriodTime() As Int32
        Get
            Return _storageperiodtime
        End Get
        Set(ByVal Value As Int32)
            _storageperiodtime = Value
        End Set
    End Property

    Public Property StoragePeriodType() As PeriodTypes
        Get
            Return _storageperiodtype
        End Get
        Set(ByVal Value As PeriodTypes)
            _storageperiodtype = Value
        End Set
    End Property

    Public Property RunCondition() As String
        Get
            Return _runcondition
        End Get
        Set(ByVal Value As String)
            _runcondition = Value
        End Set
    End Property

    Public Property ChargeDescription() As String
        Get
            Return _chargedescription
        End Get
        Set(ByVal Value As String)
            _chargedescription = Value
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

    Public Property TransportReference() As String
        Get
            Return _transportreference
        End Get
        Set(ByVal Value As String)
            _transportreference = Value
        End Set
    End Property

    Public Property TransportType() As String
        Get
            Return _transporttype
        End Get
        Set(ByVal Value As String)
            _transporttype = Value
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
    '_picktype

    Public Property PickType() As String
        Get
            Return _picktype
        End Get
        Set(ByVal Value As String)
            _picktype = Value
        End Set
    End Property
    'CLASSNAME
    Public Property CLASSNAME() As String
        Get
            Return _classname
        End Get
        Set(ByVal Value As String)
            _classname = Value
        End Set
    End Property

    Public Property STORAGECLASS() As String
        Get
            Return _storageclass
        End Get
        Set(ByVal Value As String)
            _storageclass = Value
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

    Public Property IsStorageRange() As Boolean
        Get
            Return _isstoragerange
        End Get
        Set(ByVal Value As Boolean)
            _isstoragerange = Value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New(ByRef oAgreement As Agreement)
        _agreement = oAgreement
    End Sub

    Public Sub New(ByRef oAgreement As Agreement, ByVal iLine As Int32)
        _agreement = oAgreement
        _line = iLine
        Load()

    End Sub

#End Region

#Region "Methods"

#Region "Data Methods"

    Protected Function getNextLine() As Int32
        Return DataInterface.ExecuteScalar("SELECT ISNULL(MAX(LINE),0) + 1 FROM BILLINGAGREEMENTDETAIL WHERE AGREEMENTNAME = '" & _agreement.Name & "' AND CONSIGNEE='" & _agreement.Consignee & "'")
    End Function

    Public Shared Function getAgreementLine(ByRef oAgreement As Agreement, ByVal iLineNumber As Int32) As AgreementDetail
        Dim sql As String = String.Format("SELECT TRANTYPE from BILLINGAGREEMENTDETAIL where AgreementName = {0} and Consignee = {1} and Line = {2}", Made4Net.Shared.Util.FormatField(oAgreement.Name), Made4Net.Shared.Util.FormatField(oAgreement.Consignee), Made4Net.Shared.Util.FormatField(iLineNumber))
        Try
            Dim sTranType As String = DataInterface.ExecuteScalar(sql)
            Return getAgreementLine(oAgreement, iLineNumber, TransactionTypeFromString(sTranType))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Protected Overridable Sub Load()

        Dim SQL As String = "SELECT * FROM BILLINGAGREEMENTDETAIL WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Agreemen line does not exists", "Agreemen line does not exists")
            m4nEx.Params.Add("consignee", _agreement.Consignee)
            m4nEx.Params.Add("agreementname", _agreement.Name)
            m4nEx.Params.Add("line", _line)
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("TRANTYPE") Then _trantype = TransactionTypeFromString(dr.Item("TRANTYPE"))
        If Not dr.IsNull("DOCUMENTTYPE") Then _documenttype = dr.Item("DOCUMENTTYPE")
        If Not dr.IsNull("UOM") Then _uom = dr.Item("UOM")
        If Not dr.IsNull("BILLBASIS") Then _billbasis = BillingBasisFromString(dr.Item("BILLBASIS"))
        If Not dr.IsNull("PRICEPERUNIT") Then _priceperunit = dr.Item("PRICEPERUNIT")
        If Not dr.IsNull("ISPERCENTAGE") Then _ispercentage = dr.Item("ISPERCENTAGE")
        If Not dr.IsNull("PERCENTAGE") Then _percentage = dr.Item("PERCENTAGE")
        If Not dr.IsNull("USEPRICELIST") Then _usepricelist = dr.Item("USEPRICELIST")
        If _usepricelist And Not dr.IsNull("PRICELIST") Then _pricelist = New PriceList(dr("PRICELIST"))
        If Not dr.IsNull("CURRENCY") Then _currency = dr.Item("CURRENCY")
        If Not dr.IsNull("MINPERTRAN") Then _minpertran = dr.Item("MINPERTRAN")
        If Not dr.IsNull("MAXPERTRAN") Then _maxpertran = dr.Item("MAXPERTRAN")
        If Not dr.IsNull("MINPERRUN") Then _minperrun = dr.Item("MINPERRUN")
        If Not dr.IsNull("MAXPERRUN") Then _maxperrun = dr.Item("MAXPERRUN")

        ''If Not dr.IsNull("PRICEFACTOR") Then _pricefactor = dr.Item("PRICEFACTOR")
        If Not dr.IsNull("PRICEFACTORINDEX") Then _pricefactorindex = dr.Item("PRICEFACTORINDEX")
        If Not dr.IsNull("SKUGROUP") Then _skugroup = dr.Item("SKUGROUP")
        If Not dr.IsNull("ENDDATE") Then _enddate = dr.Item("ENDDATE")
        If Not dr.IsNull("RUNCONDITION") Then _runcondition = dr.Item("RUNCONDITION")
        If Not dr.IsNull("STORAGEPERIODTIME") Then _storageperiodtime = dr.Item("STORAGEPERIODTIME")
        If Not dr.IsNull("STORAGEPERIODTYPE") Then _storageperiodtype = BillingUtils.PeriodTypesFromString(dr.Item("STORAGEPERIODTYPE"))

        If Not dr.IsNull("PERIODTYPE") Then _periodtype = BillingUtils.PeriodTypesFromString(dr.Item("PERIODTYPE"))
        If Not dr.IsNull("PERIOD") Then _period = dr.Item("PERIOD")
        If Not dr.IsNull("STARTDATE") Then _startdate = dr.Item("STARTDATE")
        If Not dr.IsNull("LASTRUNDATE") Then _lastrundate = dr.Item("LASTRUNDATE")
        If Not dr.IsNull("NEXTRUNDATE") Then _nextrundate = dr.Item("NEXTRUNDATE")
        If Not dr.IsNull("ACTIVE") Then _active = dr.Item("ACTIVE")
        If Not dr.IsNull("INITIALSTORAGELINE") Then _initialstorageline = dr.Item("INITIALSTORAGELINE")

        If Not dr.IsNull("CHARGEDESCRIPTION") Then _chargedescription = dr.Item("CHARGEDESCRIPTION")
        If Not dr.IsNull("HANDLINGUNITTYPE") Then _hutype = dr.Item("HANDLINGUNITTYPE")
        Try
            If Not dr.IsNull("TRANSPORTREFERENCE") Then _transportreference = dr.Item("TRANSPORTREFERENCE")
            If Not dr.IsNull("TRANSPORTTYPE") Then _transporttype = dr.Item("TRANSPORTTYPE")
        Catch ex As Exception
        End Try
        If Not dr.IsNull("STORAGEPARTIALPERIOD") Then _storagepartialperiod = dr.Item("STORAGEPARTIALPERIOD")

        If Not dr.IsNull("PickType") Then _picktype = dr.Item("PickType")
        If Not dr.IsNull("CLASSNAME") Then _classname = dr.Item("CLASSNAME")
        If Not dr.IsNull("STORAGECLASS") Then _storageclass = dr.Item("STORAGECLASS")
        If Not dr.IsNull("ISSTORAGERANGE") Then _isstoragerange = dr.Item("ISSTORAGERANGE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")



    End Sub



    Protected Function Exists() As Boolean
        Dim sql As String
        sql = String.Format("SELECT COUNT(1) FROM BILLINGAGREEMENTDETAIL WHERE {0}", WhereClause)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    'Public Sub SaveAgreementDetail(ByVal pLine As Int32, ByVal pTrantype As TransactionTypes, ByVal pDocumenttype As String _
    '      , ByVal pUom As String, ByVal pBillBasis As BillingBasis, ByVal pUnitPrice As Double, ByVal pIsPerc As Boolean, ByVal pPerc As Double, ByVal pUsePrice As Boolean _
    '      , ByVal pPriceList As PriceList, ByVal pCurrency As String, ByVal pMinPerTran As Double, ByVal pMaxPerTran As Double, ByVal pMinPerRun As Double, ByVal pMaxPerRun As Double _
    '      , ByVal pPeriodType As String, ByVal pPeriod As String, ByVal pStartdate As DateTime, ByVal pActive As Boolean, ByVal pUser As String, ByVal pPriceFactorIndex As String, ByVal pSkuGroup As String, ByVal pEndDate As DateTime _
    '      , ByVal pStoragePeriodTime As Int32, ByVal pStoragePeriodType As String, ByVal pRunCondition As String, ByVal pChargeDescription As String, ByVal pHandligUnitType As String, ByVal pTransportReference As String, ByVal pTransportType As String, ByVal pStoragePartialPeriod As Boolean, ByVal pIsStorageRange As Boolean)

    '    _line = pLine
    '    _active = pActive
    '    _billbasis = pBillBasis
    '    _currency = pCurrency
    '    _documenttype = pDocumenttype
    '    _ispercentage = pIsPerc
    '    _maxperrun = pMaxPerRun
    '    _maxpertran = pMaxPerTran
    '    _minperrun = pMinPerRun
    '    _minpertran = pMinPerTran

    '    _pricefactorindex = pPriceFactorIndex
    '    _skugroup = pSkuGroup
    '    _enddate = pEndDate

    '    _storageperiodtime = pStoragePeriodTime
    '    _storageperiodtype = pStoragePeriodType
    '    _runcondition = pRunCondition

    '    _lastrundate = pStartdate
    '    _percentage = pPerc
    '    _period = pPeriod
    '    _periodtype = pPeriodType
    '    _pricelist = pPriceList
    '    _priceperunit = pUnitPrice
    '    _startdate = pStartdate
    '    _status = "Normal"
    '    _trantype = pTrantype
    '    _uom = pUom
    '    _usepricelist = pUsePrice
    '    _chargedescription = pChargeDescription
    '    _hutype = pHandligUnitType
    '    _transportreference = pTransportReference
    '    _transporttype = pTransportType
    '    _storagepartialperiod = pStoragePartialPeriod
    '    _isstoragerange = pIsStorageRange
    '    Save(pUser)
    'End Sub

    'Public Overridable Sub Save(ByVal sUserId As String)
    '    If Not (WMS.Logic.Consignee.Exists(_agreement.Consignee)) Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Create Agreemet detail", "Cannot Create Agreemet detail")
    '        Throw m4nEx
    '    End If

    '    _editdate = DateTime.Now
    '    _edituser = sUserId
    '    If _line = 0 Then
    '        _line = getNextLine()
    '    End If

    '    If Not Exists() Then
    '        _nextrundate = BillingUtils.getNextRunDate(_periodtype, _lastrundate, _period)
    '    Else
    '        If _periodtype_changed Or _periodtype_changed Then
    '            _nextrundate = BillingUtils.getNextRunDate(_periodtype, _lastrundate, _period)
    '        End If
    '    End If

    '    Dim plist As String = Nothing
    '    If _usepricelist And Not _pricelist Is Nothing Then plist = _pricelist.NAME

    '    Dim sql As String
    '    If Exists() Then
    '        sql = String.Format("UPDATE BILLINGAGREEMENTDETAIL SET STATUS={0},TRANTYPE={1},DOCUMENTTYPE={2}, " & _
    '            "UOM={3},BILLBASIS={4},PRICEPERUNIT={5},ISPERCENTAGE={6},PERCENTAGE={7},USEPRICELIST={8},PRICELIST={9},CURRENCY={10},MINPERTRAN={11},MAXPERTRAN={12},MINPERRUN={13}, " & _
    '            "MAXPERRUN={14},PERIODTYPE={15},PERIOD={16},STARTDATE={17},LASTRUNDATE={18},NEXTRUNDATE={19},ACTIVE={20},INITIALSTORAGELINE={21},EDITDATE={22},EDITUSER={23}, PRICEFACTORINDEX={24}, " & _
    '            "SKUGROUP={25}, ENDDATE={26}, RUNCONDITION={27}, STORAGEPERIODTIME={28}, STORAGEPERIODTYPE={29}, CHARGEDESCRIPTION={30}, HANDLINGUNITTYPE={31}, TRANSPORTREFERENCE={32}, TRANSPORTTYPE={33},STORAGEPARTIALPERIOD={34}, ISSTORAGERANGE={35} WHERE {36} ", _
    '                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(TransactionTypeToString(_trantype)), _
    '                Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(BillingBasisToString(_billbasis)), _
    '                Made4Net.Shared.Util.FormatField(_priceperunit), Made4Net.Shared.Util.FormatField(_ispercentage), Made4Net.Shared.Util.FormatField(_percentage), _
    '                Made4Net.Shared.Util.FormatField(_usepricelist), Made4Net.Shared.Util.FormatField(plist), Made4Net.Shared.Util.FormatField(_currency), _
    '                Made4Net.Shared.Util.FormatField(_minpertran), Made4Net.Shared.Util.FormatField(_maxpertran, "NULL", True), Made4Net.Shared.Util.FormatField(_minperrun), _
    '                Made4Net.Shared.Util.FormatField(_maxperrun, "NULL", True), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_periodtype)), Made4Net.Shared.Util.FormatField(_period), _
    '                Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_lastrundate), Made4Net.Shared.Util.FormatField(_nextrundate), _
    '                Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_initialstorageline), _
    '                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
    '                Made4Net.Shared.Util.FormatField(_pricefactorindex), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_enddate), _
    '                Made4Net.Shared.Util.FormatField(_runcondition), Made4Net.Shared.Util.FormatField(_storageperiodtime), _
    '                Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_storageperiodtype)), Made4Net.Shared.Util.FormatField(_chargedescription), Made4Net.Shared.Util.FormatField(_hutype), _
    '                Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_storagepartialperiod), Made4Net.Shared.Util.FormatField(_isstoragerange), WhereClause)

    '        Dim em As New EventManagerQ
    '        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.AgreementLineUpdated
    '        em.Add("EVENT", EventType)
    '        em.Add("AGREEMENT", _agreement.Name)
    '        em.Add("AGREEMENTLINE", _line)
    '        em.Add("USERID", _adduser)
    '        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '        em.Send(WMSEvents.EventDescription(EventType))

    '    Else 'insert
    '        _adddate = DateTime.Now
    '        _adduser = sUserId
    '        sql = String.Format("INSERT INTO BILLINGAGREEMENTDETAIL(AGREEMENTNAME,CONSIGNEE,LINE,STATUS,TRANTYPE,DOCUMENTTYPE, " & _
    '            "UOM,BILLBASIS,PRICEPERUNIT,ISPERCENTAGE,PERCENTAGE,USEPRICELIST,PRICELIST,CURRENCY,MINPERTRAN,MAXPERTRAN,MINPERRUN, " & _
    '            "MAXPERRUN,PERIODTYPE,PERIOD,STARTDATE,LASTRUNDATE,NEXTRUNDATE,ACTIVE,INITIALSTORAGELINE,ADDDATE,ADDUSER,EDITDATE,EDITUSER, " & _
    '            "PRICEFACTORINDEX, SKUGROUP, ENDDATE, RUNCONDITION, STORAGEPERIODTIME, STORAGEPERIODTYPE, CHARGEDESCRIPTION, HANDLINGUNITTYPE, TRANSPORTREFERENCE, TRANSPORTTYPE, STORAGEPARTIALPERIOD, ISSTORAGERANGE) " & _
    '            "VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38}, {39}, {40})", _
    '                Made4Net.Shared.Util.FormatField(_agreement.Name), Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(_line), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(TransactionTypeToString(_trantype)), _
    '                Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(BillingBasisToString(_billbasis)), _
    '                Made4Net.Shared.Util.FormatField(_priceperunit), Made4Net.Shared.Util.FormatField(_ispercentage), Made4Net.Shared.Util.FormatField(_percentage), _
    '                Made4Net.Shared.Util.FormatField(_usepricelist), Made4Net.Shared.Util.FormatField(plist), Made4Net.Shared.Util.FormatField(_currency), _
    '                Made4Net.Shared.Util.FormatField(_minpertran, "0"), Made4Net.Shared.Util.FormatField(_maxpertran, "NULL", True), Made4Net.Shared.Util.FormatField(_minperrun, "0"), _
    '                Made4Net.Shared.Util.FormatField(_maxperrun, "NULL", True), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_periodtype)), Made4Net.Shared.Util.FormatField(_period), _
    '                Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_lastrundate), Made4Net.Shared.Util.FormatField(_nextrundate), _
    '                Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_initialstorageline), _
    '                Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), _
    '                Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_pricefactorindex), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_enddate), _
    '                Made4Net.Shared.Util.FormatField(_runcondition), Made4Net.Shared.Util.FormatField(_storageperiodtime), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_storageperiodtype)), _
    '                Made4Net.Shared.Util.FormatField(_chargedescription), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_storagepartialperiod), Made4Net.Shared.Util.FormatField(_isstoragerange))

    '        Dim em As New EventManagerQ
    '        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.AgreementLineCreated
    '        em.Add("EVENT", EventType)
    '        em.Add("AGREEMENT", _agreement.Name)
    '        em.Add("AGREEMENTLINE", _line)
    '        em.Add("USERID", _adduser)
    '        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '        em.Send(WMSEvents.EventDescription(EventType))

    '    End If
    '    DataInterface.RunSQL(sql)
    'End Sub

#End Region

#Region "Accessors"

    Public Shared Function getAgreementLine(ByRef oAgreement As Agreement, ByVal iLineNumber As Int32, ByVal sTranType As String) As AgreementDetail
        Try
            Return getAgreementLine(oAgreement, iLineNumber, TransactionTypeFromString(sTranType))
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function getAgreementLine(ByRef oAgreement As Agreement, ByVal iLineNumber As Int32, ByVal eTranType As TransactionTypes) As AgreementDetail
        Select Case (eTranType)
            Case TransactionTypes.Inbound
                Return New InboundAgreementDetail(oAgreement, iLineNumber)
            Case TransactionTypes.Outbound
                Return New OutboundAgreementDetail(oAgreement, iLineNumber)
                'Case TransactionTypes.Constant
                '    Return New ConstantAgreementDetail(oAgreement, iLineNumber)
                'Case TransactionTypes.Storage
                '    Return New StorageAgreementDetail(oAgreement, iLineNumber)
                'Case TransactionTypes.ValueAdded
                '    Return New ValueAddedAgreementDetail(oAgreement, iLineNumber)
                'Case TransactionTypes.woAssembly
                '    Return New AssemblyAgreementDetail(oAgreement, iLineNumber)
                'Case TransactionTypes.woDisassembly
                '    Return New DisassemblyAgreementDetail(oAgreement, iLineNumber)
                'Case Else
                '    Return New AdditionalAgreementDetail(oAgreement, iLineNumber)
        End Select

    End Function

    Protected Sub setNextRunDate()
        Dim SQL As String
        Dim LastDate As Date = _nextrundate
        _lastrundate = _nextrundate
        _nextrundate = BillingUtils.getNextRunDate(_periodtype, LastDate, _period)
        'Save(WMS.Lib.USERS.SYSTEMUSER)
    End Sub

    Public Shared Function TransactionTypeToString(ByVal t As TransactionTypes) As String
        Select Case t
            Case TransactionTypes.Constant
                Return "CONSTANT"
            Case TransactionTypes.Inbound
                Return "INBOUND"
            Case TransactionTypes.Outbound
                Return "OUTBOUND"
            Case TransactionTypes.Storage
                Return "STORAGE"
            Case TransactionTypes.ValueAdded
                Return "VA"
            Case TransactionTypes.woAssembly
                Return "ASM"
            Case TransactionTypes.woDisassembly
                Return "DASM"
            Case Else
                Return "ADDITIONAL"
        End Select
    End Function

    Public Shared Function TransactionTypeFromString(ByVal t As String) As TransactionTypes
        Select Case t.ToUpper
            Case "CONSTANT"
                Return TransactionTypes.Constant
            Case "INBOUND"
                Return TransactionTypes.Inbound
            Case "OUTBOUND"
                Return TransactionTypes.Outbound
            Case "STORAGE"
                Return TransactionTypes.Storage
            Case "VA"
                Return TransactionTypes.ValueAdded
            Case "ASM"
                Return TransactionTypes.woAssembly
            Case "DASM"
                Return TransactionTypes.woDisassembly
            Case Else
                Return TransactionTypes.Additional
        End Select
    End Function

    Public Shared Function BillingBasisToString(ByVal bu As BillingBasis) As String
        Select Case bu
            Case BillingBasis.NumberOfLines
                Return "NUMLINES"
            Case BillingBasis.NumberOfLoads
                Return "NUMLOADS"
            Case BillingBasis.NumberOfSkus
                Return "NUMSKUS"
            Case BillingBasis.NumberOfDucments
                Return "NUMDOCS"
            Case BillingBasis.Units
                Return "UNITS"
            Case BillingBasis.Value
                Return "VALUE"
            Case BillingBasis.Volume
                Return "VOLUME"
            Case BillingBasis.Weight
                Return "WEIGHT"
            Case BillingBasis.HandlingUnits
                Return "HU"
        End Select
    End Function

    Public Shared Function BillingBasisFromString(ByVal bu As String) As BillingBasis
        Select Case bu.ToUpper
            Case "NUMLINES"
                Return BillingBasis.NumberOfLines
            Case "NUMLOADS"
                Return BillingBasis.NumberOfLoads
            Case "NUMSKUS"
                Return BillingBasis.NumberOfSkus
            Case "NUMDOCS"
                Return BillingBasis.NumberOfDucments
            Case "UNITS"
                Return BillingBasis.Units
            Case "VALUE"
                Return BillingBasis.Value
            Case "VOLUME"
                Return BillingBasis.Volume
            Case "WEIGHT"
                Return BillingBasis.Weight
            Case "HU"
                Return BillingBasis.HandlingUnits
        End Select
    End Function

#End Region

    '#Region "Price Calculations"

    Protected Function getBillLoadValueByDateRange(ByVal sLoadID As String, ByVal units As Decimal, ByVal dStartBillingDate As DateTime, ByVal dEndBillingDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
        'Dim dStartBillingDate As DateTime
        Dim DaysCalculated As Decimal = 0

        Dim obl As BillingLoad = New BillingLoad(sLoadID)
        ' Start storage date is greater then last run date

        ' Get days to bill for the load
        DaysCalculated = dEndBillingDate.Subtract(dStartBillingDate).Days
        ' Get Unit's to bill
        Dim total As Decimal

        If _pricelist Is Nothing Then
            total = units * DaysCalculated
            If Not logger Is Nothing Then
                ' logger.Write(sLoadID.PadRight(20) & "|" & dStartBillingDate.ToString("yyyy-MM-dd").PadRight(10) & "|" & dEndBillingDate.ToString("yyyy-MM-dd").PadRight(10) & "|" & units.ToString().PadRight(10) & "|" & DaysCalculated.ToString().PadRight(20) & "|" & total.ToString().PadRight(15) & "|")
            End If
        Else
            total = _pricelist.CalculateValue(DaysCalculated, -1, logger)
            If Not logger Is Nothing Then
                ' logger.Write(sLoadID.PadRight(20) & "|" & dStartBillingDate.ToString("yyyy-MM-dd").PadRight(10) & "|" & dEndBillingDate.ToString("yyyy-MM-dd").PadRight(10) & "|" & DaysCalculated.ToString().PadRight(10) & "|" & DaysCalculated.ToString().PadRight(20) & "|" & total.ToString().PadRight(15) & "|")
            End If
            'total = _pricelist.CalculateValue(DaysCalculated * units, logger)
        End If
        Return total
    End Function

    '#Region "Price calculations for run conditions"

    '    Protected Function CalculateStorageLineValue(ByVal DaysInPeriod As Int32, ByVal sbic As StorageBillingCollection, ByVal dStartBillingPeriod As Date, ByVal dEndBillingPeriod As Date, ByVal IsDateRange As Boolean, ByVal RunCondition As Integer, Optional ByVal DocType As String = "", Optional ByVal DocID As String = "", Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal

    '        Dim LineValue As Decimal = 0
    '        If RunCondition = 0 Or RunCondition = 7 Then
    '            ' Calculate value with no condition
    '            LineValue = getChargeValue(DaysInPeriod, dStartBillingPeriod, dEndBillingPeriod, sbic, IsDateRange, PriceConvertion, logger)
    '        ElseIf RunCondition = 2 Or RunCondition = 5 Then
    '            ' Calculate value with shipped condition
    '            LineValue = getChargeValueShipped(DaysInPeriod, sbic, dStartBillingPeriod, IsDateRange, DocType, DocID, PriceConvertion, logger)
    '        ElseIf RunCondition = 1 Or RunCondition = 3 Or RunCondition = 4 Or RunCondition = 6 Then
    '            ' Calculate value with not shipped condition
    '            LineValue = getChargeValueNotShipped(DaysInPeriod, sbic, dStartBillingPeriod, dEndBillingPeriod, IsDateRange, DocType, DocID, PriceConvertion, logger)
    '        End If
    '        Return LineValue
    '    End Function

    '   Calculate line value for days range price list
    Private Function getChargeValue(ByVal DaysInPeriod As Int32, ByVal dStartBillingPeriod As Date, ByVal dEndBillingPeriod As Date, ByVal sbic As StorageBillingCollection, ByVal IsDateRange As Boolean, Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
        '  Calculate initial value for the storage
        Dim total As Decimal = 0
        logger.Write("No condition calculating")
        For Each sbi As StorageBillingInventory In sbic.Values
            Dim obl As BillingLoad = New BillingLoad(sbi.LoadId)
            Dim dStartDate, dEndDate As Date
            Dim sKeyDate As String = obl.StartDate.ToString("yyyy-MM-dd")
            Dim sHashKey As String
            If dStartBillingPeriod.Date.Subtract(obl.StartDate.Date).Days > 0 Then
                dStartDate = dStartBillingPeriod.Date
            Else
                dStartDate = obl.StartDate.Date
            End If
            If obl.EndDate <> Nothing Then
                If dEndBillingPeriod.Date.Subtract(obl.EndDate.Date).Days > 0 Then
                    dEndDate = obl.EndDate.Date
                Else
                    dEndDate = dEndBillingPeriod.Date
                End If
            Else
                dEndDate = dEndBillingPeriod.Date
            End If


            'calc price factor
            If Not logger Is Nothing Then
                logger.Write("Calculating price factor...")
            End If
            Dim oPriceFactorCalc As New PriceFactorCalc
            _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
                dStartDate.ToString("dd/MM/yyyy"), Nothing).ToString()
            If Not logger Is Nothing Then
                logger.Write("Price Factor:" & _pricefactor.ToString)
                logger.Write("Start Date:" & dStartDate.ToString("dd/MM/yyyy"))
            End If
            '---

            Dim iNumOfDays As Integer = dEndDate.Date.Subtract(dStartDate.Date).Days + 1
            ' Check if partian Storage Partial Period is checked
            Dim StoragePartialPeriodCoop As Decimal = 1
            If _storagepartialperiod Then
                StoragePartialPeriodCoop = iNumOfDays / DaysInPeriod
            End If
            Dim units As Decimal = GetUnitsForBillBase(sbi)
            If IsDateRange Then
                total = total + getBillLoadValueByDateRange(sbi.LoadId, 1, dStartDate, dEndDate, logger) * units * StoragePartialPeriodCoop
            Else
                total = total + getChargeValue(units * StoragePartialPeriodCoop, "", "", PriceConvertion, logger)
            End If

        Next
        Return total
    End Function

    '    Private Function getChargeValueNotShipped(ByVal DaysInPeriod As Int32, ByVal sbic As StorageBillingCollection, ByVal dStartBillingPeriod As Date, ByVal dEndBillingPeriod As Date, ByVal IsDateRange As Boolean, Optional ByVal DocType As String = "", Optional ByVal DocID As String = "", Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
    '        ' Calculate initial value for the storage
    '        Dim total As Decimal = 0
    '        Dim ohblprices As Hashtable = New Hashtable
    '        Dim ohblpricefactor As Hashtable = New Hashtable
    '        Dim ohblunits As Hashtable = New Hashtable
    '        Dim dStartDate, dEndDate As Date
    '        Dim isChargable As Boolean = True
    '        Dim sHashKey As String
    '        logger.Write("Not shipped condition " & RunCondition & " calculating , with using date range " & IsDateRange)
    '        If IsDateRange Then
    '            For Each sbi As StorageBillingInventory In sbic.Values
    '                Dim obl As BillingLoad = New BillingLoad(sbi.LoadId)
    '                Dim sKeyDate As String = obl.StartDate.ToString("yyyy-MM-dd")
    '                ' Get The end and start date for the calculating
    '                If dStartBillingPeriod.Date.Subtract(obl.StartDate.Date).Days > 0 Then
    '                    dStartDate = dStartBillingPeriod.Date
    '                Else
    '                    dStartDate = obl.StartDate.Date
    '                End If
    '                dEndDate = dEndBillingPeriod.Date

    '                ''calc price factor
    '                If Not logger Is Nothing Then
    '                    logger.Write("Calculating price factor...")
    '                End If
    '                Dim oPriceFactorCalc As New PriceFactorCalc
    '                Dim CurrentPriceFactor As Double = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
    '                    dStartBillingPeriod.ToString("dd/MM/yyyy"), Nothing).ToString()
    '                If Not logger Is Nothing Then
    '                    logger.Write("Price Factor:" & _pricefactor)
    '                    logger.Write("Start Billing Period:" & dStartBillingPeriod.ToString("dd/MM/yyyy"))
    '                End If
    '                ''---

    '                Dim iNumOfDays As Integer = dEndDate.Date.Subtract(dStartDate.Date).Days
    '                ' Check if partian Storage Partial Period is checked
    '                Dim StoragePartialPeriodCoop As Decimal = 1
    '                If _storagepartialperiod Then
    '                    StoragePartialPeriodCoop = iNumOfDays / DaysInPeriod
    '                End If
    '                sHashKey = GetHashKeyForDateRange(sbi, iNumOfDays)
    '                ' After seting the hash key we can manipulate the hash table
    '                Dim units As Decimal = GetUnitsForBillBase(sbi)
    '                If Not ohblprices.ContainsKey(sHashKey) Then
    '                    ohblprices.Add(sHashKey, (getBillLoadValueByDateRange(sbi.LoadId, 1, dStartDate, dEndDate, logger) * StoragePartialPeriodCoop * units)) ' / dEndBillingPeriod.Date.Subtract(dStartBillingPeriod.Date).Days))
    '                Else
    '                    ohblprices.Item(sHashKey) = ohblprices.Item(sHashKey) + getBillLoadValueByDateRange(sbi.LoadId, 1, dStartDate, dEndDate, logger) * StoragePartialPeriodCoop * units
    '                End If

    '                If Not ohblpricefactor.ContainsKey(sHashKey) Then
    '                    ohblpricefactor.Add(sHashKey, CurrentPriceFactor)
    '                Else
    '                    ohblpricefactor.Item(sHashKey) = CurrentPriceFactor
    '                End If



    '            Next

    '            Dim id As IDictionaryEnumerator = ohblprices.GetEnumerator()
    '            While id.MoveNext()
    '                _pricefactor = ohblpricefactor(ohblprices.Item(id.Key))
    '                total = total + getChargeValueIsDateRange(id.Value, ohblprices.Item(id.Key), DocType, DocID, PriceConvertion, logger)
    '            End While
    '        Else
    '            Dim tmptotal As Decimal = 0
    '            For Each sbi As StorageBillingInventory In sbic.Values
    '                Dim obl As BillingLoad = New BillingLoad(sbi.LoadId)
    '                Dim sKeyDate As String = obl.StartDate.ToString("yyyy-MM-dd")
    '                If dStartBillingPeriod.Date.Subtract(obl.StartDate.Date).Days > 0 Then
    '                    dStartDate = dStartBillingPeriod.Date
    '                Else
    '                    dStartDate = obl.StartDate.Date
    '                End If
    '                dEndDate = dEndBillingPeriod.Date
    '                Dim iNumOfDays As Integer = dEndDate.Date.Subtract(dStartDate.Date).Days
    '                ' Check if partian Storage Partial Period is checked
    '                Dim StoragePartialPeriodCoop As Decimal = 1
    '                If _storagepartialperiod Then
    '                    StoragePartialPeriodCoop = iNumOfDays / DaysInPeriod
    '                End If
    '                tmptotal = tmptotal + GetUnitsForBillBase(sbi) * StoragePartialPeriodCoop
    '            Next

    '            ''calc price factor
    '            If Not logger Is Nothing Then
    '                logger.Write("Calculating price factor...")
    '            End If
    '            Dim oPriceFactorCalc As New PriceFactorCalc
    '            _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
    '                dStartBillingPeriod.ToString("dd/MM/yyyy"), Nothing).ToString()
    '            If Not logger Is Nothing Then
    '                logger.Write("Price Factor Detail:" & _pricefactor)
    '                logger.Write("Start Billing Period:" & dStartBillingPeriod.ToString("dd/MM/yyyy"))
    '            End If

    '            total = total + getChargeValue(tmptotal, "INVENTORY", "", 1, logger)
    '        End If
    '        Return total
    '    End Function

    '    Private Function getChargeValueShipped(ByVal DaysInPeriod As Int32, ByVal sbic As StorageBillingCollection, ByVal dStartBillingPeriod As Date, ByVal IsDateRange As Boolean, Optional ByVal DocType As String = "", Optional ByVal DocID As String = "", Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
    '        ' Calculate initial value for the storage
    '        Dim total As Decimal = 0
    '        Dim ohblprices As Hashtable = New Hashtable
    '        Dim ohblunits As Hashtable = New Hashtable
    '        Dim ohblpricefactor As Hashtable = New Hashtable
    '        Dim isChargable As Boolean = True
    '        Dim dStartDate, dEndDate As Date
    '        Dim sHashKey As String
    '        logger.Write("Shipped condition " & RunCondition & " calculating , with using date range " & IsDateRange)
    '        If IsDateRange Then
    '            For Each sbi As StorageBillingInventory In sbic.Values
    '                Dim obl As BillingLoad = New BillingLoad(sbi.LoadId)
    '                Dim sKeyDate As String = obl.StartDate.ToString("yyyy-MM-dd")
    '                ' Get The end and start date for the calculating
    '                If dStartBillingPeriod.Date.Subtract(obl.StartDate.Date).Days > 0 Then
    '                    dStartDate = dStartBillingPeriod.Date
    '                Else
    '                    dStartDate = obl.StartDate.Date
    '                End If


    '                ''calc price factor
    '                If Not logger Is Nothing Then
    '                    logger.Write("Calculating price factor...")
    '                End If
    '                Dim oPriceFactorCalc As New PriceFactorCalc
    '                Dim CurrentPriceFactor As Double = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
    '                    dStartBillingPeriod.ToString("dd/MM/yyyy"), Nothing).ToString()
    '                If Not logger Is Nothing Then
    '                    logger.Write("Price Factor:" & _pricefactor)
    '                    logger.Write("Start Billing Period:" & dStartBillingPeriod.ToString("dd/MM/yyyy"))
    '                End If
    '                ''---

    '                Dim iNumOfDays As Integer = obl.EndDate.Date.Subtract(dStartDate.Date).Days + 1
    '                ' The load is no more so let not charge for it
    '                If obl.EndDate.Date.Subtract(dStartDate.Date).Days >= 0 Then
    '                    isChargable = True
    '                Else
    '                    isChargable = False
    '                End If
    '                If isChargable Then
    '                    ' Check if partian Storage Partial Period is checked
    '                    Dim StoragePartialPeriodCoop As Decimal = 1
    '                    If _storagepartialperiod Then
    '                        StoragePartialPeriodCoop = iNumOfDays / DaysInPeriod
    '                    End If
    '                    sHashKey = GetHashKeyForDateRange(sbi, iNumOfDays)
    '                    ' After seting the hash key we can manipulate the hash table
    '                    Dim units As Decimal = GetUnitsForBillBase(sbi)
    '                    If Not ohblprices.ContainsKey(sHashKey) Then
    '                        ohblprices.Add(sHashKey, getBillLoadValueByDateRange(sbi.LoadId, 1, dStartDate, obl.EndDate, logger) * StoragePartialPeriodCoop * units)
    '                    Else
    '                        ohblprices.Item(sHashKey) = ohblprices.Item(sHashKey) + getBillLoadValueByDateRange(sbi.LoadId, 1, dStartDate, obl.EndDate, logger) * StoragePartialPeriodCoop * units
    '                    End If

    '                    If Not ohblpricefactor.ContainsKey(sHashKey) Then
    '                        ohblpricefactor.Add(sHashKey, CurrentPriceFactor)
    '                    Else
    '                        ohblpricefactor.Item(sHashKey) = CurrentPriceFactor
    '                    End If


    '                End If
    '            Next
    '            Dim id As IDictionaryEnumerator = ohblprices.GetEnumerator()
    '            While id.MoveNext()
    '                _pricefactor = ohblpricefactor(ohblprices.Item(id.Key))
    '                total = total + getChargeValueIsDateRange(id.Value, ohblprices.Item(id.Key), DocType, DocID, PriceConvertion, logger)
    '            End While
    '        Else
    '            Dim tmptotal As Decimal = 0
    '            For Each sbi As StorageBillingInventory In sbic.Values
    '                Dim obl As BillingLoad = New BillingLoad(sbi.LoadId)
    '                Dim sKeyDate As String = obl.StartDate.ToString("yyyy-MM-dd")
    '                If dStartBillingPeriod.Date.Subtract(obl.StartDate.Date).Days > 0 Then
    '                    dStartDate = dStartBillingPeriod.Date
    '                Else
    '                    dStartDate = obl.StartDate.Date
    '                End If
    '                If obl.EndDate.Date.Subtract(dStartDate.Date).Days >= 0 Then
    '                    isChargable = True
    '                Else
    '                    isChargable = False
    '                End If
    '                If isChargable Then
    '                    Dim iNumOfDays As Integer = obl.EndDate.Date.Subtract(dStartDate.Date).Days + 1
    '                    ' Check if partian Storage Partial Period is checked
    '                    Dim StoragePartialPeriodCoop As Decimal = 1
    '                    If _storagepartialperiod Then
    '                        StoragePartialPeriodCoop = iNumOfDays / DaysInPeriod
    '                    End If
    '                    tmptotal = tmptotal + GetUnitsForBillBase(sbi) * StoragePartialPeriodCoop
    '                End If
    '            Next

    '            ''calc price factor
    '            If Not logger Is Nothing Then
    '                logger.Write("Calculating price factor...")
    '            End If
    '            Dim oPriceFactorCalc As New PriceFactorCalc
    '            Dim CurrentPriceFactor As Double = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
    '                dStartDate.ToString("dd/MM/yyyy"), Nothing).ToString()
    '            If Not logger Is Nothing Then
    '                logger.Write("Price Factor:" & _pricefactor)
    '                logger.Write("Start Date:" & dStartDate.ToString("dd/MM/yyyy"))
    '            End If
    '            ''---

    '            _pricefactor = CurrentPriceFactor
    '            total = total + getChargeValue(tmptotal, "INVENTORY", "", 1, logger)
    '        End If
    '        Return total
    '    End Function

    Private Function GetHashKeyForDateRange(ByVal sbi As StorageBillingInventory, ByVal iNumOfDays As Integer) As String
        Dim sHashKey As String = ""
        Select Case _billbasis
            Case AgreementDetail.BillingBasis.NumberOfSkus
                sHashKey = sbi.Consignee & "#"c & sbi.Sku & "#"c & iNumOfDays
            Case AgreementDetail.BillingBasis.Units
                sHashKey = sbi.Units & "#"c & iNumOfDays
            Case AgreementDetail.BillingBasis.Value
                sHashKey = sbi.getValue & "#"c & iNumOfDays
            Case AgreementDetail.BillingBasis.Volume
                sHashKey = sbi.getVolume & "#"c & iNumOfDays
            Case AgreementDetail.BillingBasis.Weight
                sHashKey = sbi.getWeight & "#"c & iNumOfDays
            Case AgreementDetail.BillingBasis.NumberOfLoads
                sHashKey = iNumOfDays
        End Select
        Return sHashKey
    End Function

    Private Function GetUnitsForBillBase(ByVal sbi As StorageBillingInventory) As Decimal
        Dim units As Decimal = 0
        Select Case _billbasis
            Case AgreementDetail.BillingBasis.NumberOfSkus
                units = 1
            Case AgreementDetail.BillingBasis.Units
                units = sbi.Units
            Case AgreementDetail.BillingBasis.Value
                units = sbi.getValue
            Case AgreementDetail.BillingBasis.Volume
                units = sbi.getVolume
            Case AgreementDetail.BillingBasis.Weight
                units = sbi.getWeight
            Case AgreementDetail.BillingBasis.NumberOfLoads
                units = 1
        End Select
        Return units
    End Function

    '#End Region

    '    ' Function for calculating Charge Value without running condition
    '    Protected Function getChargeValueIsDateRange(ByVal chargeUnits As Decimal, ByVal mNewPrice As Decimal, Optional ByVal DocType As String = "", Optional ByVal DocID As String = "", Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal

    '        ' Calculate initial value for the storage
    '        Dim calcValue As Double = 0
    '        calcValue = chargeUnits * PriceConvertion * Me.PriceFactor
    '        ' If percentage is updated then calculate the percantage
    '        If (_ispercentage) Then
    '            calcValue = ((calcValue * _percentage) / 100) * Me.PriceFactor
    '        End If
    '        ' Take max and min run to percentage calculations
    '        If calcValue < _minpertran Then
    '            calcValue = _minpertran * Me.PriceFactor
    '        ElseIf calcValue > _maxpertran And _maxpertran > 0 Then
    '            calcValue = _maxpertran * Me.PriceFactor
    '        End If

    '        If Not logger Is Nothing Then
    '            logger.Write(DocType.PadRight(16) & "|" & DocID.PadRight(20) & "|" & chargeUnits.ToString().PadRight(20) & "|" & _priceperunit.ToString().PadRight(20) & "|" & calcValue.ToString().PadRight(20) & "|")
    '        End If

    '        Return calcValue
    '    End Function

    '    ' Function for calculating Charge Value without running condition
    '    Protected Function getChargeValue(ByVal chargeUnits As Decimal, ByVal mNewPrice As Decimal, Optional ByVal DocType As String = "", Optional ByVal DocID As String = "", Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal

    '        ' Calculate initial value for the storage
    '        Dim calcValue As Double = 0
    '        If (_usepricelist) Then
    '            calcValue = _pricelist.CalculateValue(chargeUnits, -1, logger) * PriceConvertion * Me.PriceFactor
    '        Else
    '            calcValue = chargeUnits * mNewPrice * PriceConvertion * Me.PriceFactor
    '        End If
    '        ' If percentage is updated then calculate the percantage
    '        If (_ispercentage) Then
    '            calcValue = ((calcValue * _percentage) / 100) * Me.PriceFactor
    '        End If
    '        ' Take max and min run to percentage calculations
    '        If calcValue < _minpertran Then
    '            calcValue = _minpertran * Me.PriceFactor
    '        ElseIf calcValue > _maxpertran And _maxpertran > 0 Then
    '            calcValue = _maxpertran * Me.PriceFactor
    '        End If

    '        If Not logger Is Nothing Then
    '            logger.Write(DocType.PadRight(16) & "|" & DocID.PadRight(20) & "|" & chargeUnits.ToString().PadRight(20) & "|" & _priceperunit.ToString().PadRight(20) & "|" & calcValue.ToString().PadRight(20) & "|")
    '        End If

    '        Return calcValue
    '    End Function

    ' Function for calculating Charge Value without running condition
    Protected Function getChargeValue(ByVal chargeUnits As Decimal, Optional ByVal DocType As String = "", Optional ByVal DocID As String = "", Optional ByVal PriceConvertion As Decimal = 1, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal ', Optional ByVal PriceListChargeUnits As Decimal = -1, Optional ByVal pUsePriceListChargeUnits As Boolean = True) As Decimal
        ' Calculate initial value for the storage
        Dim calcValue As Double = 0
        logger.Write("Using pricelist " & _usepricelist & " is percentage " & _ispercentage)
        If (_usepricelist) Then
            calcValue = _pricelist.CalculateValue(chargeUnits, -1, logger) * PriceConvertion * Me.PriceFactor
        Else
            calcValue = chargeUnits * _priceperunit * PriceConvertion * Me.PriceFactor
        End If
        ' If percentage is updated then calculate the percantage
        If (_ispercentage) Then
            calcValue = ((calcValue * _percentage) / 100) * Me.PriceFactor
        End If
        ' Take max and min run to percentage calculations
        If calcValue < _minpertran Then
            calcValue = _minpertran * Me.PriceFactor
        ElseIf calcValue > _maxpertran And _maxpertran > 0 Then
            calcValue = _maxpertran * Me.PriceFactor
        End If

        If Not logger Is Nothing Then
            logger.Write(DocType.PadRight(16) & "|" & DocID.PadRight(20) & "|" & chargeUnits.ToString().PadRight(20) & "|" & _priceperunit.ToString().PadRight(20) & "|" & calcValue.ToString().PadRight(20) & "|")
        End If

        Return calcValue
    End Function
    '#End Region

    '#Region "Proccess Methods"

    '    'Public MustOverride Function Process(ByVal runToDate As DateTime, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection

    '#End Region

#End Region

End Class