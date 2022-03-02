Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

Public Class AdditionalAgreementDetail
    Inherits AgreementDetail

    Protected _trntype As String

    Public Property AdditionalTransactionType() As String
        Get
            Return _trntype
        End Get
        Set(ByVal Value As String)
            _trntype = Value
        End Set
    End Property

    Protected Overrides Sub Load()

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
        _trntype = dr.Item("TRANTYPE")
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
        If Not dr.IsNull("PERIODTYPE") Then _periodtype = BillingUtils.PeriodTypesFromString(dr.Item("PERIODTYPE"))
        If Not dr.IsNull("PERIOD") Then _period = dr.Item("PERIOD")
        If Not dr.IsNull("STARTDATE") Then _startdate = dr.Item("STARTDATE")
        If Not dr.IsNull("LASTRUNDATE") Then _lastrundate = dr.Item("LASTRUNDATE")
        If Not dr.IsNull("NEXTRUNDATE") Then _nextrundate = dr.Item("NEXTRUNDATE")
        If Not dr.IsNull("ACTIVE") Then _active = dr.Item("ACTIVE")
        If Not dr.IsNull("INITIALSTORAGELINE") Then _initialstorageline = dr.Item("INITIALSTORAGELINE")

        ''If Not dr.IsNull("PRICEFACTOR") Then _pricefactor = dr.Item("PRICEFACTOR")
        If Not dr.IsNull("PRICEFACTORINDEX") Then _pricefactorindex = dr.Item("PRICEFACTORINDEX")

        If Not dr.IsNull("SKUGROUP") Then _skugroup = dr.Item("SKUGROUP")
        If Not dr.IsNull("ENDDATE") Then _enddate = dr.Item("ENDDATE")
        If Not dr.IsNull("RUNCONDITION") Then _runcondition = dr.Item("RUNCONDITION")
        If Not dr.IsNull("STORAGEPERIODTIME") Then _storageperiodtime = dr.Item("STORAGEPERIODTIME")
        If Not dr.IsNull("STORAGEPERIODTYPE") Then _storageperiodtype = BillingUtils.PeriodTypesFromString(dr.Item("STORAGEPERIODTYPE"))
        If Not dr.IsNull("CHARGEDESCRIPTION") Then _chargedescription = dr.Item("CHARGEDESCRIPTION")

        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

    End Sub

    Public Overrides Sub Save(ByVal sUserId As String)
        If Not (WMS.Logic.Consignee.Exists(_agreement.Consignee)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Create Agreemet detail", "Cannot Create Agreemet detail")
            Throw m4nEx
        End If

        _editdate = DateTime.Now
        _edituser = sUserId
        If _line = 0 Then
            _line = getNextLine()
        End If

        _nextrundate = BillingUtils.getNextRunDate(_periodtype, _lastrundate, _period)

        Dim plist As String = Nothing
        If _usepricelist And Not _pricelist Is Nothing Then plist = _pricelist.NAME

        Dim sql As String
        If Exists() Then
            sql = String.Format("UPDATE BILLINGAGREEMENTDETAIL SET STATUS={0},TRANTYPE={1},DOCUMENTTYPE={2}, " & _
                "UOM={3},BILLBASIS={4},PRICEPERUNIT={5},ISPERCENTAGE={6},PERCENTAGE={7},USEPRICELIST={8},PRICELIST={9},CURRENCY={10},MINPERTRAN={11},MAXPERTRAN={12},MINPERRUN={13}, " & _
                "MAXPERRUN={14},PERIODTYPE={15},PERIOD={16},STARTDATE={17},LASTRUNDATE={18},NEXTRUNDATE={19},ACTIVE={20},INITIALSTORAGELINE={21},EDITDATE={22},EDITUSER={23}, PRICEFACTORINDEX={24}, SKUGROUP={25}, ENDDATE={26}, RUNCONDITION={27}, STORAGEPERIODTIME={28}, STORAGEPERIODTYPE={29}, CHARGEDESCRIPTION={30}, HANDLINGUNITTYPE={31}, TRANSPORTREFERENCE={32}, TRANSPORTTYPE={33},STORAGEPARTIALPERIOD={34},ISSTORAGERANGE={35} WHERE {36} ", _
                    Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(TransactionTypeToString(_trantype)), _
                    Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_uom, ""), Made4Net.Shared.Util.FormatField(BillingBasisToString(_billbasis)), _
                    Made4Net.Shared.Util.FormatField(_priceperunit), Made4Net.Shared.Util.FormatField(_ispercentage), Made4Net.Shared.Util.FormatField(_percentage), _
                    Made4Net.Shared.Util.FormatField(_usepricelist), Made4Net.Shared.Util.FormatField(plist), Made4Net.Shared.Util.FormatField(_currency), _
                    Made4Net.Shared.Util.FormatField(_minpertran), Made4Net.Shared.Util.FormatField(_maxpertran, "NULL", True), Made4Net.Shared.Util.FormatField(_minperrun), _
                    Made4Net.Shared.Util.FormatField(_maxperrun, "NULL", True), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_periodtype)), Made4Net.Shared.Util.FormatField(_period), _
                    Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_lastrundate), Made4Net.Shared.Util.FormatField(_nextrundate), _
                    Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_initialstorageline), _
                    Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                    Made4Net.Shared.Util.FormatField(_pricefactorindex), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_enddate), _
                    Made4Net.Shared.Util.FormatField(_runcondition), Made4Net.Shared.Util.FormatField(_storageperiodtime), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_storageperiodtype)), Made4Net.Shared.Util.FormatField(_chargedescription), Made4Net.Shared.Util.FormatField(_hutype), _
                    Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_storagepartialperiod), Made4Net.Shared.Util.FormatField(_isstoragerange), WhereClause)
        Else 'insert
            _adddate = DateTime.Now
            _adduser = sUserId
            sql = String.Format("INSERT INTO BILLINGAGREEMENTDETAIL(AGREEMENTNAME,CONSIGNEE,LINE,STATUS,TRANTYPE,DOCUMENTTYPE, " & _
                "UOM,BILLBASIS,PRICEPERUNIT,ISPERCENTAGE,PERCENTAGE,USEPRICELIST,PRICELIST,CURRENCY,MINPERTRAN,MAXPERTRAN,MINPERRUN, " & _
                "MAXPERRUN,PERIODTYPE,PERIOD,STARTDATE,LASTRUNDATE,NEXTRUNDATE,ACTIVE,INITIALSTORAGELINE,ADDDATE,ADDUSER,EDITDATE,EDITUSER, PRICEFACTORINDEX, SKUGROUP, ENDDATE, RUNCONDITION, STORAGEPERIODTIME, STORAGEPERIODTYPE , CHARGEDESCRIPTION, HANDLINGUNITTYPE, TRANSPORTREFERENCE, TRANSPORTTYPE,STORAGEPARTIALPERIOD,ISSTORAGERANGE) " & _
                "VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28}, {29}, {30}, {31}, {32}, {33}, {34}, {35}, {36}, {37}, {38},{39},{40})", _
                    Made4Net.Shared.Util.FormatField(_agreement.Name), Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(_line), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(TransactionTypeToString(_trantype)), _
                    Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(BillingBasisToString(_billbasis)), _
                    Made4Net.Shared.Util.FormatField(_priceperunit), Made4Net.Shared.Util.FormatField(_ispercentage), Made4Net.Shared.Util.FormatField(_percentage), _
                    Made4Net.Shared.Util.FormatField(_usepricelist), Made4Net.Shared.Util.FormatField(plist), Made4Net.Shared.Util.FormatField(_currency), _
                    Made4Net.Shared.Util.FormatField(_minpertran, "0"), Made4Net.Shared.Util.FormatField(_maxpertran, "NULL", True), Made4Net.Shared.Util.FormatField(_minperrun, "0"), _
                    Made4Net.Shared.Util.FormatField(_maxperrun, "NULL", True), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_periodtype)), Made4Net.Shared.Util.FormatField(_period), _
                    Made4Net.Shared.Util.FormatField(_startdate), Made4Net.Shared.Util.FormatField(_lastrundate), Made4Net.Shared.Util.FormatField(_nextrundate), _
                    Made4Net.Shared.Util.FormatField(_active), Made4Net.Shared.Util.FormatField(_initialstorageline), _
                    Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), _
                    Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_pricefactorindex), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_enddate), _
                    Made4Net.Shared.Util.FormatField(_runcondition), Made4Net.Shared.Util.FormatField(_storageperiodtime), Made4Net.Shared.Util.FormatField(BillingUtils.PeriodTypeToString(_storageperiodtype)), Made4Net.Shared.Util.FormatField(_chargedescription), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_storagepartialperiod), Made4Net.Shared.Util.FormatField(_isstoragerange))
        End If
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement, ByVal pLINE As Int32)
        MyBase.New(oAgreement, pLINE)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement)
        MyBase.New(oAgreement)
    End Sub

    Public Overrides Function Process(ByVal runToDate As DateTime, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection
        logger.Write("Processing Transaction Type: " & Me.AdditionalTransactionType)
        Dim SubTotalValue As Decimal = 0
        logger.Write("Processing From Date: " & LastRunDate.ToString("d") & " To Date: " + NextRunDate.ToString("d"))
        Dim SQL As String
        Dim TotalBillingUnits As Double = 0, TotalBillingValue As Double = 0, LineBilling As Double = 0
        Dim NumTransactions As Int32 = 0

        If Not _active Then
            logger.Write("Skipping: Agreement Line Not Active.")
            Return Nothing
        End If

        Dim cdc As New ChargeDetailCollection
        While _nextrundate.Date <= runToDate
            Dim dt As New DataTable
            Dim dr As DataRow
            Dim cd As New ChargeDetail

            cd.AgreementLine = _line
            cd.BillFromDate = _lastrundate
            cd.BillToDate = _nextrundate
            cd.Currency = _currency

            SQL = String.Format("Select TRANID,CONSIGNEE,UNITS,UNITSTYPE,ISTOTALAMOUNT,PRICE from BILLINGADDITIONAL WHERE CONSIGNEE = {0} and TRANTYPE = {1} and UNITSTYPE = {2} AND " & _
               "TRANDATE >= {3} and TRANDATE < {4}", Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(_trntype), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_lastrundate)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_nextrundate.AddDays(1).AddSeconds(-1))))

            logger.Write(SQL)

            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                NumTransactions = NumTransactions + 1
                logger.Write("Action ID: " & dr("TRANID"))
                If (dr("ISTOTALAMOUNT")) Then
                    logger.Write("Action Is Total Amount ")
                    LineBilling = 1
                    logger.Write("Line Price: " & dr("PRICE"))
                    SubTotalValue = dr("PRICE")
                Else
                    LineBilling = dr("Units")
                    logger.Write("Action Billing Units: " & LineBilling)
                    If _usepricelist Then
                        logger.Write("Use Pricelist: " & _pricelist.NAME)
                        SubTotalValue = _pricelist.CalculateValue(LineBilling, -1)
                    Else
                        logger.Write("Action Price Per Unit: " & _priceperunit)
                        logger.Write("Action Billing Value: " & _priceperunit * LineBilling)
                        SubTotalValue = _priceperunit * LineBilling
                        TotalBillingUnits += LineBilling
                    End If
                    If (_ispercentage) Then
                        SubTotalValue = (SubTotalValue * _percentage) / 100
                    End If
                End If
                logger.Write("Subtotal value: " & SubTotalValue)

                If (SubTotalValue < _minpertran) Then
                    logger.Write("Line Value is Less then Minimum value: " & _minpertran & "... New Line Value Updated to Minimum")
                    TotalBillingValue += _minpertran
                ElseIf SubTotalValue > _maxpertran Then
                    logger.Write("Line Value is Greater then Maximum value: " & _maxpertran & "... New Line Value Updated to Maximum")
                    TotalBillingValue += _maxpertran
                Else
                    TotalBillingValue += SubTotalValue
                End If
            Next

            logger.Write("Total Value: " & TotalBillingValue)
            If (TotalBillingValue < _minperrun) Then
                logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
                TotalBillingValue = _minperrun
            Else
                If TotalBillingValue > _maxperrun Then
                    logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
                    TotalBillingValue = _maxperrun
                End If
            End If

            cd.BillTotal = TotalBillingValue
            cd.BilledUnits = TotalBillingUnits
            cdc.add(cd)
            setNextRunDate()
        End While

        Return cdc
    End Function
End Class
