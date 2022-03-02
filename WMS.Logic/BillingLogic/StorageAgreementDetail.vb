Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class StorageAgreementDetail
    Inherits AgreementDetail

    Public Sub New(ByRef oAgreement As Agreement, ByVal pLINE As Int32)
        MyBase.New(oAgreement, pLINE)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement)
        MyBase.New(oAgreement)
    End Sub

    Public Overrides Function Process(ByVal runToDate As Date, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection
        logger.Write("Processing Transaction Type: " & TransactionTypeToString(Me.TransactionType))
        If _nextrundate.Date > runToDate Then
            logger.Write("Skiping : Transaction Already Processed in the past for date: " & runToDate.ToString("d"))
            Return Nothing
        End If
        If (Not Active) Then
            logger.Write("Skipping : Agreement Line Not Active.")
            Return Nothing
        End If
        Dim TotalBillingUnits As Decimal = 0, TotalBillingValue As Decimal = 0, SubTotalValue As Decimal = 0
        Dim sbi As New StorageBillingProvider(_agreement.Consignee, _uom, _skugroup)
        Dim cdc As New ChargeDetailCollection
        Dim storagePeriodStartDate, storagePeriodEndDate As Date
        Dim CalcByStoragePeriods As Boolean = False
        If _storageperiodtype > _periodtype Then
            storagePeriodStartDate = _lastrundate
            storagePeriodEndDate = BillingUtils.getNextRunDate(_storageperiodtype, storagePeriodStartDate, _storageperiodtime)
            CalcByStoragePeriods = True
        Else
            storagePeriodStartDate = DateTime.MinValue
            storagePeriodEndDate = DateTime.MinValue
        End If
        ' Should not run till the ending period exactly , will be calculated in the next run
        Dim ChargeDetailCounter As Int32 = 1
        Dim ItemsCache As New Hashtable
        While _nextrundate.Date <= runToDate
            If Not CalcByStoragePeriods Then
                storagePeriodStartDate = _lastrundate.Date
                storagePeriodEndDate = _nextrundate.Date
            End If
            sbi.FillBillingLoads(_lastrundate, _nextrundate, _runcondition, logger)

            TotalBillingUnits = 0
            TotalBillingValue = 0
            'LineBilling = 0
            SubTotalValue = 0

            logger.Write("Processing From Date: " & _lastrundate.ToString("dd-MM-yyyy") & " To Date: " & _nextrundate.ToString("dd-MM-yyyy"))
            logger.Write("Storage Period From Date: " & storagePeriodStartDate.ToString("dd-MM-yyyy") & " Storage Period To Date: " & storagePeriodEndDate.ToString("dd-MM-yyyy") & " (For checking run conditions..)")

            'DataInterface.RunSQL("select 'this is the begining of the balagan queries'")

            Dim cd As New ChargeDetail
            cd.AgreementName = ch.BillingAgreement.Name
            cd.ChargeHeader = ch
            cd.ChargeLine = cd.GetNextLineId(ch.ChargeId)
            cd.AgreementLine = _line
            cd.BillFromDate = _lastrundate
            cd.BillToDate = _nextrundate
            cd.Currency = _currency
            cd.ChargeText = _chargedescription
            ' After all the data from Inventory Transactions and InvLoad is loaded get stock from last run date
            'logger.Write("Start get stock for date..")
            Dim sbic As StorageBillingCollection = sbi.getStockForDate(_lastrundate, ItemsCache, logger)
            'logger.Write("End get stock for date..")

            ' we need to check id there is runcondition for each one of the rows and update the StorageBillingCollection accordingly
            ' !!!no need - did it when loading all loads.... !!!
            'logger.Write("Start Update run condition..")
            'sbic.UpdateRunConditionForCollection(_runcondition, BillingUtils.getNextRunDate(_storageperiodtype, _storageperiodtime), storagePeriodStartDate, storagePeriodEndDate, logger)
            'logger.Write("End Update run condition..")

            ' After Load Collection is created for billing calculations we will insert the data to SQL Table
            If SaveStorageTransactions = 1 Then
                sbic.UpdateBillingChargesLoads(ch.ChargeId, cd.ChargeLine)
            End If
            '----------------------------------
            '----------------------------------

            'DataInterface.RunSQL("select 'this is the end of the balagan queries'")

            ''calc price factor
            If Not logger Is Nothing Then
                logger.Write("Calculating price factor...")
            End If
            Dim oPriceFactorCalc As New PriceFactorCalc
            _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
                _lastrundate.ToString("dd/MM/yyyy"), logger).ToString()
            If Not logger Is Nothing Then
                logger.Write("Price Factor:" & _pricefactor)
                logger.Write("Start Date For Storage Range:" & _lastrundate.ToString("dd/MM/yyyy"))
            End If
            ''---

            TotalBillingValue = GetTotalBillingValue(cd, sbic, TotalBillingUnits, logger)
            '----------------------------------
            '----------------------------------
            logger.Write("Total Value : " & TotalBillingValue)
            If (TotalBillingValue < _minperrun) Then
                logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
                TotalBillingValue = _minperrun
            ElseIf TotalBillingValue > _maxperrun And _maxperrun > 0 Then
                logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
                TotalBillingValue = _maxperrun
            End If

            cd.BillTotal = TotalBillingValue
            cd.BilledUnits = TotalBillingUnits
            If cd.BillTotal > 0 Then
                cd.post(pUser)
                cdc.add(cd)
            End If
            setNextRunDate()
            If CalcByStoragePeriods AndAlso _lastrundate.Date >= storagePeriodEndDate Then
                storagePeriodStartDate = _lastrundate.Date
                storagePeriodEndDate = BillingUtils.getNextRunDate(_storageperiodtype, storagePeriodStartDate, _storageperiodtime)
            End If
            logger.writeSeperator("-", 120)
        End While

        Return cdc
    End Function

    Private Function GetTotalBillingValue(ByVal cd As ChargeDetail, ByVal sbic As StorageBillingCollection, ByRef pTotalBillingUnits As Decimal, Optional ByVal logger As WMS.Logic.LogHandler = Nothing)
        Dim IsPriceList As Boolean = False
        Dim IsDateRange As Boolean = False
        Dim LineBilling As Decimal
        Dim TotalBillingValue As Decimal = 0

        If _usepricelist Then
            IsPriceList = True
        End If
        If _usepricelist AndAlso _pricelist.ISDATERANGE Then
            IsDateRange = True
        End If
        ' Get period days
        Dim DaysInPeriod As Int32 = cd.BillToDate.Subtract(cd.BillFromDate).Days
        logger.Write("Days In Period : " & DaysInPeriod)
        '--------------------------------------------------
        '--------------------------------------------------
        Dim TotalBillingUnits As Decimal = 0
        If _isstoragerange Then
            ' In this case we should go over all loads and get the pricelist per units (Run condition already calculated)
            Dim hs As Hashtable = New Hashtable
            logger.Write("Runnig over all Storage Billing Inventory collection for price evaluation")
            For Each sbi As StorageBillingInventory In sbic.Values
                ' The key will be the date and the value according to PriceList + calculated billbase units
                AddStorageBillingValue(hs, sbi)
            Next
            TotalBillingValue = CalculateStorageTotalForStorageRange(hs, DaysInPeriod, logger)
            logger.Write("Charge value calculated " & TotalBillingValue)
        Else
            LineBilling = sbic.getBilledUnits(BillBasis)
            logger.Write("Units to bill : " & LineBilling)
            logger.Write("Load Id".PadRight(20) & "|" & "Start Date".PadRight(10) & "|" & "End Date".PadRight(10) & "|" & "Units".PadRight(10) & "|" & "Days in storage".PadRight(20) & "|" & "Total value".PadRight(15) & "|")
            logger.writeSeperator("-", 91)
            TotalBillingUnits += LineBilling

            If Me.RunCondition <> RunConditions.NoCondtition Then
                logger.Write("Run condition is : " & Me.RunCondition)
                If IsDateRange Then
                    logger.Write("Charge value calculated with Date Range")
                    TotalBillingValue = TotalBillingValue + Me.CalculateStorageLineValue(DaysInPeriod, sbic, _lastrundate.Date, _nextrundate.Date, IsDateRange, Me.RunCondition, "INVENTORY", "", 1, logger)
                ElseIf _storageperiodtype = _periodtype Then
                    ' OK - DaysInPeriod
                    logger.Write("Charge value calculated by Not Shipped calculation")
                    TotalBillingValue = TotalBillingValue + Me.CalculateStorageLineValue(DaysInPeriod, sbic, _lastrundate.Date, _nextrundate.Date, IsDateRange, Me.RunCondition, "INVENTORY", "", 1, logger)
                Else
                    TotalBillingValue = TotalBillingValue + Me.getChargeValue(LineBilling, "INVENTORY", "", 1, logger)
                End If
            Else
                logger.Write("No running condition")
                ' 0 - No condition
                If IsDateRange Then
                    logger.Write("Charge value calculated with Date Range")
                    TotalBillingValue = TotalBillingValue + Me.CalculateStorageLineValue(DaysInPeriod, sbic, _lastrundate.Date, _nextrundate.Date, IsDateRange, Me.RunCondition, "INVENTORY", "", 1, logger)
                Else
                    logger.Write("Charge value calculated regular")
                    TotalBillingValue = TotalBillingValue + Me.getChargeValue(LineBilling, "INVENTORY", "", 1, logger)
                End If
            End If
            logger.writeSeperator("-", 121)
        End If
        pTotalBillingUnits = TotalBillingUnits
        Return TotalBillingValue
    End Function

    Private Sub AddStorageBillingValue(ByRef pHT As Hashtable, ByVal sbi As StorageBillingInventory)
        Dim LoadRecivedDate As Date = DataInterface.ExecuteScalar("SELECT TOP 1 RECEIVEDATE FROM vBILLINGLOADS WHERE LOADID='" & sbi.LoadId & "'")
        Dim strDate As String = Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(LoadRecivedDate).Split(" ")(0))
        Dim sqlQty As String

        Dim dUnitsValueToCharge As Decimal
        Select Case _billbasis
            Case AgreementDetail.BillingBasis.NumberOfSkus
                sqlQty = String.Format("SELECT count(distinct sku) as currentqty FROM vBILLINGLOADS WHERE BILLINGLOADID IN " & _
                        "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0}" & _
                        " AND todate >= {1} AND CONSIGNEE = '{2}' group by loadid)", strDate, strDate, sbi.Consignee)
                dUnitsValueToCharge = 1
            Case AgreementDetail.BillingBasis.Units
                sqlQty = String.Format("SELECT SUM(currentqty) as currentqty FROM vBILLINGLOADS WHERE BILLINGLOADID IN " & _
                        "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0}" & _
                        " AND todate >= {1} AND CONSIGNEE = '{2}' group by loadid)", strDate, strDate, sbi.Consignee)
                dUnitsValueToCharge = sbi.Units
            Case AgreementDetail.BillingBasis.Value
                sqlQty = String.Format("SELECT sum(currentqty * isnull(sku.unitprice,0)) as currentqty FROM vBILLINGLOADS left outer join sku on vBILLINGLOADS.consignee = sku.consignee and vBILLINGLOADS.sku= sku.sku WHERE BILLINGLOADID IN " & _
                        "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0}" & _
                        " AND todate >= {1} AND CONSIGNEE = '{2}' group by loadid)", strDate, strDate, sbi.Consignee)
                dUnitsValueToCharge = sbi.UnitPrice * sbi.Units
            Case AgreementDetail.BillingBasis.Volume
                sqlQty = String.Format("SELECT sum(currentqty * isnull(skuuom.volume,0)) as currentqty FROM vBILLINGLOADS left outer join skuuom on vBILLINGLOADS.consignee = skuuom.consignee and vBILLINGLOADS.sku= skuuom.sku WHERE BILLINGLOADID IN " & _
                        "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0}" & _
                        " AND todate >= {1} AND CONSIGNEE = '{2}' group by loadid)", strDate, strDate, sbi.Consignee)
                dUnitsValueToCharge = sbi.UomVolume * sbi.Units
            Case AgreementDetail.BillingBasis.Weight
                sqlQty = String.Format("SELECT sum(currentqty * isnull(skuuom.netweight,0)) as currentqty FROM vBILLINGLOADS left outer join skuuom on vBILLINGLOADS.consignee = skuuom.consignee and vBILLINGLOADS.sku= skuuom.sku WHERE BILLINGLOADID IN " & _
                        "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0}" & _
                        " AND todate >= {1} AND CONSIGNEE = '{2}' group by loadid)", strDate, strDate, sbi.Consignee)
                dUnitsValueToCharge = sbi.UomWeight * sbi.Units
            Case AgreementDetail.BillingBasis.NumberOfLoads
                sqlQty = String.Format("SELECT count(distinct loadid) as currentqty FROM vBILLINGLOADS WHERE BILLINGLOADID IN " & _
                        "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0}" & _
                        " AND todate >= {1} AND CONSIGNEE = '{2}' group by loadid)", strDate, strDate, sbi.Consignee)
                dUnitsValueToCharge = 1
        End Select
        Dim dtInventoryUnits As DataTable = New DataTable
        DataInterface.FillDataset(sqlQty, dtInventoryUnits)
        Dim dCurrentQtyLevel As Decimal
        If dtInventoryUnits.Rows.Count > 0 Then
            dCurrentQtyLevel = System.Convert.ToDecimal(dtInventoryUnits.Rows(0)("currentqty"))
        End If
        If dtInventoryUnits.Rows.Count > 0 Then
            UpdateStorageBillingValue(pHT, sbi.LoadId, dUnitsValueToCharge, dCurrentQtyLevel, sbi)
        End If
    End Sub

    Private Sub UpdateStorageBillingValue(ByRef pHT As Hashtable, ByVal pKey As String, ByVal pUnitsValueToCharge As Decimal, ByVal pInventoryLevelForPriceListCalculation As Decimal, ByVal sbi As StorageBillingInventory)
        Dim oBillingStorageChargeUnit As BillingStorageChargeUnit
        If pHT.ContainsKey(pKey) Then
            oBillingStorageChargeUnit = pHT.Item(pKey)
            oBillingStorageChargeUnit.LoadUnitsToCharge += pUnitsValueToCharge
            oBillingStorageChargeUnit.SBI = sbi
        Else
            oBillingStorageChargeUnit.LoadUnitsToCharge = pUnitsValueToCharge
            oBillingStorageChargeUnit.InventoryLevelForPriceListCalculation = pInventoryLevelForPriceListCalculation
            oBillingStorageChargeUnit.SBI = sbi
            pHT.Add(pKey, oBillingStorageChargeUnit)
        End If
    End Sub

    Private Function CalculateStorageTotalForStorageRange(ByRef pHT As Hashtable, ByVal DaysInPeriod As Int32, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Decimal
        Dim TotalValue As Decimal = 0
        Dim LineValue As Decimal = 0
        Dim isChargable As Boolean = True
        Dim dStartDate, dEndDate As Date
        Dim iNumOfDays As Integer

        For Each oBillingStorageChargeUnit As BillingStorageChargeUnit In pHT.Values
            Dim obl As BillingLoad = New BillingLoad(oBillingStorageChargeUnit.SBI.LoadId)
            Select Case Me.RunCondition
                Case 0      ' Calculate value with no condition
                    If _lastrundate.Date.Subtract(obl.StartDate.Date).Days > 0 Then
                        dStartDate = _lastrundate.Date
                    Else
                        dStartDate = obl.StartDate.Date
                    End If
                    If obl.EndDate <> Nothing Then
                        If _nextrundate.Date.Subtract(obl.EndDate.Date).Days > 0 Then
                            dEndDate = obl.EndDate.Date
                        Else
                            dEndDate = _nextrundate.Date
                        End If
                    Else
                        dEndDate = _nextrundate.Date
                    End If
                    isChargable = True
                    iNumOfDays = dEndDate.Date.Subtract(dStartDate.Date).Days
                Case 2, 5
                    ' Calculate value with shipped condition
                    If _lastrundate.Date.Subtract(obl.StartDate.Date).Days > 0 Then
                        dStartDate = _lastrundate.Date
                    Else
                        dStartDate = obl.StartDate.Date
                    End If
                    If obl.EndDate.Date.Subtract(dStartDate.Date).Days >= 0 Then
                        isChargable = True
                    Else
                        isChargable = False
                    End If
                    iNumOfDays = dEndDate.Date.Subtract(dStartDate.Date).Days + 1
                Case 1, 3, 4, 6
                    ' Calculate value with not shipped condition
                    If _lastrundate.Date.Subtract(obl.StartDate.Date).Days > 0 Then
                        dStartDate = _lastrundate.Date
                    Else
                        dStartDate = obl.StartDate.Date
                    End If
                    dEndDate = _nextrundate.Date
                    iNumOfDays = dEndDate.Date.Subtract(dStartDate.Date).Days
                    isChargable = True
            End Select
            ' Check if partian Storage Partial Period is checked
            Dim StoragePartialPeriodCoop As Decimal = 1
            If _storagepartialperiod Then
                StoragePartialPeriodCoop = iNumOfDays / DaysInPeriod
            End If

            ''calc price factor
            If Not logger Is Nothing Then
                logger.Write("Calculating price factor...")
            End If
            Dim oPriceFactorCalc As New PriceFactorCalc
            _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
                dStartDate.ToString("dd/MM/yyyy"), Nothing).ToString()
            If Not logger Is Nothing Then
                logger.Write("Price Factor:" & _pricefactor)
                logger.Write("Start Date For Storage Range:" & dStartDate.ToString("dd/MM/yyyy"))
            End If
            ''---


            If (_usepricelist) Then
                LineValue = _pricelist.CalculateValue(oBillingStorageChargeUnit.LoadUnitsToCharge, oBillingStorageChargeUnit.InventoryLevelForPriceListCalculation, logger) * 1 * Me.PriceFactor * StoragePartialPeriodCoop
            Else
                LineValue = oBillingStorageChargeUnit.LoadUnitsToCharge * _priceperunit * 1 * Me.PriceFactor * StoragePartialPeriodCoop
            End If
            ' If percentage is updated then calculate the percantage
            If (_ispercentage) Then
                LineValue = ((LineValue * _percentage) / 100) * Me.PriceFactor
            End If
            ' Take max and min run to percentage calculations
            If LineValue < _minpertran Then
                LineValue = _minpertran * Me.PriceFactor
            ElseIf LineValue > _maxpertran And _maxpertran > 0 Then
                LineValue = _maxpertran * Me.PriceFactor
            End If

            If Not logger Is Nothing Then
                logger.Write("INVENTORY".PadRight(16) & "|" & "".PadRight(20) & "|" & oBillingStorageChargeUnit.LoadUnitsToCharge.ToString().PadRight(20) & "|" & _priceperunit.ToString().PadRight(20) & "|" & LineValue.ToString().PadRight(20) & "|")
            End If
            TotalValue += LineValue
        Next

        Return TotalValue
    End Function

    Private Structure BillingStorageChargeUnit
        Dim LoadUnitsToCharge As Decimal
        Dim InventoryLevelForPriceListCalculation As Decimal
        Dim SBI As StorageBillingInventory
    End Structure


End Class
