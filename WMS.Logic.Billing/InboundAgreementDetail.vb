Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class InboundAgreementDetail
    Inherits AgreementDetail

    Public Sub New(ByRef oAgreement As Agreement, ByVal pLINE As Int32)
        MyBase.New(oAgreement, pLINE)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement)
        MyBase.New(oAgreement)
    End Sub

    'Public Overrides Function Process(ByVal runToDate As Date, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection
    Public Function ProcessTotalBillingValue(ByVal orderid As String, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String) As Double
        Dim TotalBillingUnits As Double = 0, TotalBillingValue As Double = 0, LineBilling As Double, SubTotalValue As Double = 0

        logger.Write("Processing Transaction Type: " & TransactionTypeToString(Me.TransactionType))

        If (Not Active) Then
            logger.Write("Skipping: Agreement Line Not Active.")
            Return Nothing
        End If

        TotalBillingUnits = 0
        TotalBillingValue = 0
        LineBilling = 0
        SubTotalValue = 0

        Dim sql As String
        Dim dt As New DataTable
        Dim dr As DataRow

        sql = "select CONSIGNEE, ORDERID,ISNULL(sum(numskus),0) numskus,ISNULL(sum(numlines),0) numlines,ISNULL(sum(numloads),0) numloads,ISNULL(sum(qtyreceived),0) qtyreceived,ISNULL(sum(value),0) value,ISNULL(sum(weight),0) weight,ISNULL(sum(volume),0) volume " & _
              "FROM vInboundTotalBillingValue where CONSIGNEE={0} and ORDERID={1}"

        sql = String.Format(sql, Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(orderid))

        If Not _documenttype Is Nothing And Not _documenttype = "" Then
            sql += String.Format(" and ordertype = {0} ", Made4Net.Shared.Util.FormatField(_documenttype))
        End If
        If Not _skugroup Is Nothing And Not _skugroup = "" Then
            sql += String.Format(" and skugroup = {0} ", Made4Net.Shared.Util.FormatField(_skugroup))
        End If

        If Not String.IsNullOrEmpty(_classname) Then
            sql += String.Format(" and CLASSNAME = {0} ", Made4Net.Shared.Util.FormatField(_classname))
        End If

        If Not String.IsNullOrEmpty(_storageclass) Then
            sql += String.Format(" and STORAGECLASS = {0} ", Made4Net.Shared.Util.FormatField(_storageclass))
        End If

        sql = sql & " group by CONSIGNEE, ORDERID"
        sql = String.Format(sql, _agreement.Consignee, orderid)
        logger.Write("SQL: " & sql)

        DataInterface.FillDataset(sql, dt)


        ''calc price factor
        If Not logger Is Nothing Then
            logger.Write("Calculating price factor...")
        End If
        Dim oPriceFactorCalc As New PriceFactorCalc
        _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
            CType(_lastrundate.ToString("dd/MM/yyyy"), String), Nothing)
        If Not logger Is Nothing Then
            logger.Write("Price Factor:" & _pricefactor)
            logger.Write("Start Date For Storage Range:" & _lastrundate.ToString("dd/MM/yyyy"))
        End If
        ''---
        logger.Write("Transaction Type".PadRight(15) & "|" & "Document Id".PadRight(20) & "|" & "Billing base units".PadRight(20) & "|" & "Price per unit".PadRight(20) & "|" & "Calculated Value".PadRight(20) & "|")
        logger.writeSeperator("-"c, 121)

        For Each dr In dt.Rows
            ' logger.Write("Receipt ID: " & dr("receipt"))
            Select Case _billbasis
                Case AgreementDetail.BillingBasis.HandlingUnits
                    'Dim HUTSql As String = "SELECT ISNULL(SUM(HUQTY),0) HUQTY FROM HANDLINGUNITTRANSACTION WHERE TRANSACTIONTYPE='RECEIPT' AND CONSIGNEE = " & Made4Net.Shared.Util.FormatField(_agreement.Consignee) & " AND TRANSACTIONTYPEID = " & Made4Net.Shared.Util.FormatField(dr("receipt"))
                    'If _hutype <> "" Then
                    '    HUTSql = HUTSql & " AND HUTYPE = " & Made4Net.Shared.Util.FormatField(_hutype)
                    'End If
                    'LineBilling = DataInterface.ExecuteScalar(HUTSql)
                    LineBilling = 0
                Case AgreementDetail.BillingBasis.NumberOfDucments
                    TotalBillingUnits = dt.Rows.Count
                    TotalBillingValue = Me.getChargeValue(dt.Rows.Count, "ORDERID", CType(dr("ORDERID"), String), 1, logger)
                    logger.Write("Number of document: " & dt.Rows.Count)
                    Exit For
                Case AgreementDetail.BillingBasis.NumberOfLines
                    LineBilling = CType(dr("numlines"), Double)
                Case AgreementDetail.BillingBasis.NumberOfLoads
                    LineBilling = CType(dr("numloads"), Double)
                Case AgreementDetail.BillingBasis.NumberOfSkus
                    LineBilling = CType(dr("numskus"), Double)
                Case AgreementDetail.BillingBasis.Units
                    LineBilling = CType(dr("qtyreceived"), Double)
                Case AgreementDetail.BillingBasis.Value
                    LineBilling = CType(dr("value"), Double)
                Case AgreementDetail.BillingBasis.Volume
                    LineBilling = CType(dr("volume"), Double)
                Case AgreementDetail.BillingBasis.Weight
                    LineBilling = CType(dr("weight"), Double)
            End Select

            TotalBillingUnits += LineBilling
            TotalBillingValue += Me.getChargeValue(CType(LineBilling, Decimal), "ORDERID", CType(dr("ORDERID"), String), 1, logger)
        Next

        logger.writeSeperator("-"c, 121)
        logger.Write("Total Value: ".PadLeft(80) & TotalBillingValue)

        If (TotalBillingValue < _minperrun) Then
            logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
            TotalBillingValue = _minperrun
        ElseIf TotalBillingValue > _maxperrun And _maxperrun > 0 Then
            logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
            TotalBillingValue = _maxperrun
        End If


        Return TotalBillingValue

    End Function


    Public Function ProcessBillingChargeDetailPosted(ByVal BILLFROMDATE As Date, ByVal BILLTODATE As Date, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, Optional ByVal ch As ChargeDetail = Nothing) As Int32
        Dim TotalBillingUnits As Double = 0, TotalBillingValue As Double = 0, LineBilling As Double, SubTotalValue As Double = 0
        Dim ret As Int32 = 0

        logger.Write("Processing Transaction Type: " & TransactionTypeToString(Me.TransactionType))
        'If _nextrundate.Date > runToDate Then
        '    logger.Write("Transaction Already Processed in the past for date: " & runToDate.ToString("d"))
        '    Return Nothing
        'End If

        Try

            If (Not Active) Then
                logger.Write("Skipping: Agreement Line Not Active.")
                Return 0
            End If

            'Dim cdc As New ChargeDetailCollection

            '' Should not run till the ending period exactly , will be calculated in the next run
            'While _nextrundate.Date <= runToDate

            TotalBillingUnits = 0
            TotalBillingValue = 0
            LineBilling = 0
            SubTotalValue = 0

            'logger.Write("Last Run Date: " & LastRunDate.ToString("d"))
            'logger.Write("Next Run Date: " & NextRunDate.ToString("d"))
            'logger.Write("Run To Date: " & runToDate.ToString("d"))

            logger.Write("Processing From Date: " & LastRunDate.ToString("d") & " To Date: " & NextRunDate.ToString("d"))

            Dim sql As String
            Dim dt As New DataTable
            Dim dr As DataRow

            If Not _skugroup Is Nothing And Not _skugroup = "" Then
                sql = String.Format("select rd.consignee, rd.orderid, count(distinct rd.sku) as numskus, count(distinct rd.receiptline) as numlines, " &
                    "(select count(distinct loadid) from inventorytrans left outer join sku on sku.consignee = inventorytrans.consignee and inventorytrans.sku = sku.sku where invtrntype={0} and inventorytrans.consignee={2} and IsNull(sku.STORAGECLASS,{5}) = {5}  " &
                    " and document in (select RECEIPT from RECEIPTDETAIL r1 where r1.CONSIGNEE =  rd.CONSIGNEE and r1.ORDERID = rd.ORDERID) and isnull(sku.skugroup,'')={3}) - " &
                    "(select isnull(count(distinct loadid),0) from inventorytrans left outer join sku on sku.consignee = inventorytrans.consignee and inventorytrans.sku = sku.sku where invtrntype={4} and inventorytrans.consignee={2} and IsNull(sku.STORAGECLASS,{5}) = {5} " &
                    " and document in (select RECEIPT from RECEIPTDETAIL r1 where r1.CONSIGNEE =  rd.CONSIGNEE and r1.ORDERID = rd.ORDERID) and isnull(sku.skugroup,'')={3}) as numloads,  " &
                    "(SELECT SUM(qtyreceived) FROM RECEIPTDETAIL left outer join sku on sku.consignee = RECEIPTDETAIL.consignee and RECEIPTDETAIL.sku = sku.sku WHERE RECEIPT in (select RECEIPT from RECEIPTDETAIL r1 where r1.CONSIGNEE =  rd.CONSIGNEE and r1.ORDERID = rd.ORDERID) AND qtyreceived > 0 and isnull(sku.skugroup,'')={3}) as qtyreceived,  " &
                    "sum(rd.qtyreceived * isnull(sku.unitprice,0)) as value, rhweight.weight as weight,  rhvol.volume as volume  " &
                    "from receiptheader rh join receiptdetail rd on rh.receipt = rd.receipt join sku on rd.consignee = sku.consignee and rd.sku = sku.sku " &
                    "left outer join (SELECT rd.CONSIGNEE, rd.ORDERID, sum(ISNULL(SKUUOM.VOLUME, 0) * qtyreceived) AS volume FROM receiptdetail AS rd INNER JOIN " &
                    "SKUUOM ON rd.CONSIGNEE = SKUUOM.CONSIGNEE AND rd.SKU = SKUUOM.SKU WHERE (SKUUOM.LOWERUOM IS NULL) OR (SKUUOM.LOWERUOM = '') group by rd.CONSIGNEE, rd.ORDERID) AS rhvol ON rhvol.CONSIGNEE = rd.CONSIGNEE and  rhvol.ORDERID = rd.ORDERID " &
                    "left outer join (SELECT rd.CONSIGNEE, rd.ORDERID, sum(ISNULL(SKUUOM.netweight, 0) * qtyreceived) AS weight FROM receiptdetail AS rd INNER JOIN " &
                    "SKUUOM ON rd.CONSIGNEE = SKUUOM.CONSIGNEE AND rd.SKU = SKUUOM.SKU WHERE (SKUUOM.LOWERUOM IS NULL) OR (SKUUOM.LOWERUOM = '') group by rd.CONSIGNEE, rd.ORDERID) AS rhweight ON rhweight.CONSIGNEE = rd.CONSIGNEE and  rhweight.ORDERID = rd.ORDERID " &
                    "left outer join inboundordheader id on id.consignee = rd.consignee and id.orderid = rd.orderid  " &
                    "where rh.status = {1} and rd.consignee = {2} and rd.qtyreceived > 0 ",
                    Made4Net.Shared.Util.FormatField(WMS.Lib.Actions.Audit.CREATELOAD),
                    Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Receipt.CLOSED),
                    Made4Net.Shared.Util.FormatField(_agreement.Consignee),
                    Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(WMS.Lib.Actions.Audit.UNRECEIVELOAD),
                    Made4Net.Shared.Util.FormatField(_storageclass))
            Else
                sql = String.Format("select rd.consignee, rd.orderid, count(distinct rd.sku) as numskus, count(distinct rd.receiptline) as numlines, " &
                    "(select count(distinct loadid) from inventorytrans left outer join sku on sku.consignee = inventorytrans.consignee and inventorytrans.sku = sku.sku where invtrntype={0} and inventorytrans.consignee={2} and  IsNull(sku.STORAGECLASS,{4}) = {4} and document  in (select RECEIPT from RECEIPTDETAIL r1 where r1.CONSIGNEE =  rd.CONSIGNEE and r1.ORDERID = rd.ORDERID)) - " &
                    "(select isnull(count(distinct loadid),0) from inventorytrans left outer join sku on sku.consignee = inventorytrans.consignee and inventorytrans.sku = sku.sku where invtrntype={3} and inventorytrans.consignee={2}  and IsNull(sku.STORAGECLASS,{4}) = {4} and document  in (select RECEIPT from RECEIPTDETAIL r1 where r1.CONSIGNEE =  rd.CONSIGNEE and r1.ORDERID = rd.ORDERID)) as numloads,  " &
                    "(SELECT SUM(qtyreceived) FROM RECEIPTDETAIL left outer join sku on sku.consignee = RECEIPTDETAIL.consignee and RECEIPTDETAIL.sku = sku.sku WHERE RECEIPT  in (select RECEIPT from RECEIPTDETAIL r1 where r1.CONSIGNEE =  rd.CONSIGNEE and r1.ORDERID = rd.ORDERID) AND qtyreceived > 0) as qtyreceived,  " &
                    "sum(rd.qtyreceived * isnull(sku.unitprice,0)) as value, rhweight.weight as weight,  rhvol.volume as volume  " &
                    "from receiptheader rh join receiptdetail rd on rh.receipt = rd.receipt join sku on rd.consignee = sku.consignee and rd.sku = sku.sku " &
                    "left outer join (SELECT rd.CONSIGNEE, rd.ORDERID, sum(ISNULL(SKUUOM.VOLUME, 0) * qtyreceived) AS volume FROM receiptdetail AS rd INNER JOIN " &
                    "SKUUOM ON rd.CONSIGNEE = SKUUOM.CONSIGNEE AND rd.SKU = SKUUOM.SKU WHERE (SKUUOM.LOWERUOM IS NULL) OR (SKUUOM.LOWERUOM = '') group by rd.CONSIGNEE, rd.ORDERID) AS rhvol ON rhvol.CONSIGNEE = rd.CONSIGNEE and  rhvol.ORDERID = rd.ORDERID " &
                    "left outer join (SELECT rd.CONSIGNEE, rd.ORDERID, sum(ISNULL(SKUUOM.netweight, 0) * qtyreceived) AS weight FROM receiptdetail AS rd INNER JOIN " &
                    "SKUUOM ON rd.CONSIGNEE = SKUUOM.CONSIGNEE AND rd.SKU = SKUUOM.SKU WHERE (SKUUOM.LOWERUOM IS NULL) OR (SKUUOM.LOWERUOM = '') group by rd.CONSIGNEE, rd.ORDERID) AS rhweight ON rhweight.CONSIGNEE = rd.CONSIGNEE and  rhweight.ORDERID = rd.ORDERID " &
                    "left outer join inboundordheader id on id.consignee = rd.consignee and id.orderid = rd.orderid  " &
                    "where rh.status = {1} and rd.consignee = {2} and rd.qtyreceived > 0 ",
                    Made4Net.Shared.Util.FormatField(WMS.Lib.Actions.Audit.CREATELOAD),
                    Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Receipt.CLOSED),
                    Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(WMS.Lib.Actions.Audit.UNRECEIVELOAD),
                    Made4Net.Shared.Util.FormatField(_storageclass))
                '" SUM(rd.QTYRECEIVED) as qtyreceived, " & _
                '"(SELECT SUM(qtyreceived) FROM RECEIPTDETAIL left outer join sku on sku.consignee = RECEIPTDETAIL.consignee and RECEIPTDETAIL.sku = sku.sku WHERE RECEIPT = rh.receipt AND qtyreceived > 0) as qtyreceived,  " & _

            End If
            'Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_nextrundate.AddDays(1).AddSeconds(-1))))

            'If _periodtype = PeriodTypes.Day Then
            '    sql += String.Format(" and rh.editdate >= {0} and rh.editdate <= {1} ", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_lastrundate)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_lastrundate.AddDays(1).AddSeconds(-1))))
            'Else
            '    sql += String.Format(" and rh.editdate >= {0} and rh.editdate <= {1} ", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_lastrundate)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_nextrundate.AddSeconds(-1))))
            'End If

            sql += String.Format(" and rh.editdate >= {0} and rh.editdate <= {1} ", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(BILLFROMDATE)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(BILLTODATE)))


            If Not _documenttype Is Nothing And Not _documenttype = "" Then
                sql += String.Format(" and id.ordertype = {0} ", Made4Net.Shared.Util.FormatField(_documenttype))
            End If

            If Not _skugroup Is Nothing And Not _skugroup = "" Then
                sql += String.Format(" and sku.skugroup = {0} ", Made4Net.Shared.Util.FormatField(_skugroup))
            End If

            If Not String.IsNullOrEmpty(_classname) Then
                sql += String.Format(" and sku.CLASSNAME = {0} ", Made4Net.Shared.Util.FormatField(_classname))
            End If

            If Not String.IsNullOrEmpty(_storageclass) Then
                sql += String.Format(" and sku.STORAGECLASS = {0} ", Made4Net.Shared.Util.FormatField(_storageclass))
            End If

            sql = sql & " group by  rd.consignee, rd.orderid,rhvol.volume,rhweight.weight"
            logger.Write("SQL: " & sql)

            DataInterface.FillDataset(sql, dt)

            Dim cd As ChargeDetail

            For Each dr In dt.Rows
                cd = New ChargeDetail
                cd.CHARGEID = ch.CHARGEID

                ' cd.CHARGEHEADER = ch
                cd.CHARGELINE = cd.GetNextLineId(ch.CHARGEID)

                cd.AGREEMENTNAME = _agreement.Name ' ch.BILLINGAGREEMENT.Name
                cd.AGREEMENTLINE = _line
                cd.BILLFROMDATE = ch.BILLFROMDATE ' BILLFROMDATE ' _lastrundate
                cd.BILLTODATE = ch.BILLTODATE ' BILLTODATE '_nextrundate
                cd.CURRENCY = ch.CURRENCY ' _currency
                cd.CHARGETEXT = ch.CHARGETEXT ' _chargedescription
                ' cd.ToStrin = _chargedescription


                ''calc price factor
                If Not logger Is Nothing Then
                    logger.Write("Calculating price factor...")
                End If
                Dim oPriceFactorCalc As New PriceFactorCalc
                _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
                    _lastrundate.ToString("dd/MM/yyyy"), Nothing)
                If Not logger Is Nothing Then
                    logger.Write("Price Factor:" & _pricefactor)
                    logger.Write("Start Date For Storage Range:" & _lastrundate.ToString("dd/MM/yyyy"))
                End If
                ''---

                ' logger.Write("Receipt ID: " & dr("receipt"))
                Select Case _billbasis
                    Case AgreementDetail.BillingBasis.HandlingUnits
                        'Dim HUTSql As String = "SELECT ISNULL(SUM(HUQTY),0) HUQTY FROM HANDLINGUNITTRANSACTION WHERE TRANSACTIONTYPE='RECEIPT' AND CONSIGNEE = " & Made4Net.Shared.Util.FormatField(_agreement.Consignee) & " AND TRANSACTIONTYPEID = " & Made4Net.Shared.Util.FormatField(dr("receipt"))
                        Dim HUTSql As String = "SELECT ISNULL(SUM(HUQTY),0) HUQTY FROM HANDLINGUNITTRANSACTION WHERE TRANSACTIONTYPE='RECEIPT' AND CONSIGNEE = {0} AND TRANSACTIONTYPEID IN (SELECT RECEIPT FROM RECEIPTDETAIL WHERE CONSIGNEE={0} AND ORDERID = {1}) "
                        HUTSql = String.Format(HUTSql, Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(dr("ORDERID")))
                        If _hutype <> "" Then
                            HUTSql = HUTSql & " AND HUTYPE = " & Made4Net.Shared.Util.FormatField(_hutype)
                        End If
                        LineBilling = CType(DataInterface.ExecuteScalar(HUTSql), Double)
                    Case AgreementDetail.BillingBasis.NumberOfDucments
                        TotalBillingUnits = dt.Rows.Count
                        logger.Write("Transaction Type".PadRight(15) & "|" & "Document Id".PadRight(20) & "|" & "Billing base units".PadRight(20) & "|" & "Price per unit".PadRight(20) & "|" & "Calculated Value".PadRight(20) & "|")
                        logger.writeSeperator("-"c, 121)
                        TotalBillingValue = Me.getChargeValue(dt.Rows.Count, "INBOUND", CType(dr("orderid"), String), 1, logger)
                        logger.Write("Number of document: " & dt.Rows.Count)
                        'Begin RWMS-809
                        logger.Write("Total Value: ".PadLeft(80) & TotalBillingValue)

                        If (TotalBillingValue < _minperrun) Then
                            logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
                            TotalBillingValue = _minperrun
                        ElseIf TotalBillingValue > _maxperrun And _maxperrun > 0 Then
                            logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
                            TotalBillingValue = _maxperrun
                        End If

                        cd.BILLTOTAL = TotalBillingValue
                        cd.BILLEDUNITS = TotalBillingUnits
                        cd.TRANSACTIONID = ""
                        cd.STATUS = "NEW"

                        logger.Write("TotalBillingValue =" & TotalBillingValue.ToString())

                        If cd.BILLTOTAL > 0 Then
                            cd.post(pUser, logger)
                        Else
                            logger.Write("can not add new line, TotalBillingValue is empty")
                        End If
                        'Begin RWMS-809
                        Exit For
                    Case AgreementDetail.BillingBasis.NumberOfLines
                        LineBilling = CType(dr("numlines"), Double)
                    Case AgreementDetail.BillingBasis.NumberOfLoads
                        LineBilling = CType(dr("numloads"), Double)
                    Case AgreementDetail.BillingBasis.NumberOfSkus
                        LineBilling = CType(dr("numskus"), Double)
                    Case AgreementDetail.BillingBasis.Units
                        LineBilling = CType(dr("qtyreceived"), Double)
                    Case AgreementDetail.BillingBasis.Value
                        LineBilling = CType(dr("value"), Double)
                    Case AgreementDetail.BillingBasis.Volume
                        LineBilling = CType(dr("volume"), Double)
                    Case AgreementDetail.BillingBasis.Weight
                        LineBilling = CType(dr("weight"), Double)
                End Select

                TotalBillingUnits = LineBilling
                logger.Write("Transaction Type".PadRight(15) & "|" & "Document Id".PadRight(20) & "|" & "Billing base units".PadRight(20) & "|" & "Price per unit".PadRight(20) & "|" & "Calculated Value".PadRight(20) & "|")
                logger.writeSeperator("-"c, 121)
                TotalBillingValue = Me.getChargeValue(CType(LineBilling, Decimal), "INBOUND", CType(dr("orderid"), String), 1, logger)

                logger.writeSeperator("-"c, 121)
                logger.Write("Total Value: ".PadLeft(80) & TotalBillingValue)

                If (TotalBillingValue < _minperrun) Then
                    logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
                    TotalBillingValue = _minperrun
                ElseIf TotalBillingValue > _maxperrun And _maxperrun > 0 Then
                    logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
                    TotalBillingValue = _maxperrun
                End If

                cd.BILLTOTAL = TotalBillingValue
                cd.BILLEDUNITS = TotalBillingUnits
                cd.TRANSACTIONID = CType(dr("ORDERID"), String)
                cd.STATUS = "NEW"

                logger.Write("TotalBillingValue =" & TotalBillingValue.ToString())

                If cd.BILLTOTAL > 0 Then
                    cd.post(pUser, logger)
                Else
                    logger.Write("can not add new line, TotalBillingValue is empty")
                End If
                ret = ret + 1
            Next

        Catch ex As Exception
            logger.Write(ex.Message)
        End Try
        'setNextRunDate()
        'End While

        ' Return cdc
        Return ret
    End Function

End Class