Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class OutboundAgreementDetail
    Inherits AgreementDetail

    Public Sub New(ByRef oAgreement As Agreement, ByVal pLINE As Int32)
        MyBase.New(oAgreement, pLINE)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement)
        MyBase.New(oAgreement)
    End Sub

    '    Public Overrides Function Process(ByVal runToDate As Date, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection
    Public Function ProcessTotalBillingValue(ByVal orderid As String, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String) As Double
        Dim TotalBillingUnits As Double = 0, TotalBillingValue As Double = 0, LineBilling As Double, SubTotalValue As Double = 0


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

        sql = "SELECT  CONSIGNEE, ORDERID,sum(numlines) numlines,sum(numskus) numskus,sum(QTYMODIFIED) QTYMODIFIED,sum(volume) volume,sum(weight) weight,sum(numloads) numloads,sum(value) value FROM vOutBoundTotalBillingValue " & _
        "where CONSIGNEE = {0} and ORDERID = {1} "

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

        sql += " group by CONSIGNEE, orderid"

        DataInterface.FillDataset(sql, dt)
        logger.Write(sql)

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

        Dim dblOrderLineValue As Double
        For Each dr In dt.Rows
            dblOrderLineValue = 0
            'logger.Write("Order ID: " & dr("orderid"))
            Select Case _billbasis
                Case AgreementDetail.BillingBasis.HandlingUnits
                    ' get handling units by type
                    'Dim HUTSql As String = "SELECT ISNULL(SUM(HUQTY),0) HUQTY FROM HANDLINGUNITTRANSACTION WHERE CONSIGNEE = " & Made4Net.Shared.Util.FormatField(_agreement.Consignee) & " AND ORDERID = " & Made4Net.Shared.Util.FormatField(dr("orderid"))
                    'If _hutype <> "" Then
                    '    HUTSql = HUTSql & " AND HUTYPE = " & Made4Net.Shared.Util.FormatField(_hutype)
                    'End If
                    'LineBilling = DataInterface.ExecuteScalar(HUTSql)
                    LineBilling = 0
                Case AgreementDetail.BillingBasis.NumberOfDucments
                    TotalBillingUnits = dt.Rows.Count
                    'Modified for RWMS-597
                    dblOrderLineValue = Me.getChargeValue(dt.Rows.Count, "OUTBOUND", CType(dr("orderid"), String), 1, logger)
                    logger.Write("number of document: " & dt.Rows.Count)
                    logger.writeSeperator("-"c, 121)
                    logger.Write("Total Value: ".PadLeft(80) & dblOrderLineValue)
                    TotalBillingValue = MinMaxRunCheck(dblOrderLineValue, logger)
                    'End RWMS-597
                    Exit For
                Case AgreementDetail.BillingBasis.NumberOfLines
                    LineBilling = CType(dr("numlines"), Double)
                    'Dim ldSql As String = "SELECT ISNULL(SUM(ISNULL(numloads,0)),0) as numloads FROM vOutBoundNumLoads WHERE CONSIGNEE = {0} AND  [ORDERID]={1} AND ISNULL(SKUGROUP, N'') LIKE ISNULL({2}, N'%')  AND ISNULL(ORDERTYPE, N'') LIKE ISNULL({3}, N'%')"
                    'ldSql = String.Format(ldSql, Made4Net.Shared.Util.FormatField(_agreement.Consignee), Made4Net.Shared.Util.FormatField(orderid), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_documenttype))
                    'LineBilling = DataInterface.ExecuteScalar(ldSql)
                    'logger.Write(ldSql)
                Case AgreementDetail.BillingBasis.NumberOfLoads
                    LineBilling = CType(dr("numloads"), Double)
                Case AgreementDetail.BillingBasis.NumberOfSkus
                    LineBilling = CType(dr("numskus"), Double)
                Case AgreementDetail.BillingBasis.Units
                    LineBilling = CType(dr("QTYMODIFIED"), Double)
                Case AgreementDetail.BillingBasis.Value
                    LineBilling = CType(dr("value"), Double)
                Case AgreementDetail.BillingBasis.Volume
                    LineBilling = CType(dr("volume"), Double)
                Case AgreementDetail.BillingBasis.Weight
                    LineBilling = CType(dr("weight"), Double)
            End Select

            TotalBillingUnits += LineBilling

            logger.Write("Transaction Type".PadRight(15) & "|" & "Document Id".PadRight(20) & "|" & "Billing base units".PadRight(20) & "|" & "Price per unit".PadRight(20) & "|" & "Calculated Value".PadRight(20) & "|")
            logger.writeSeperator("-"c, 121)

            'Modified for RWMS-597
            dblOrderLineValue = Me.getChargeValue(CType(LineBilling, Decimal), "OUTBOUND", CType(dr("orderid"), String), 1, logger)
            logger.writeSeperator("-"c, 121)
            logger.Write("Total Value: ".PadLeft(80) & TotalBillingValue)
            TotalBillingValue += MinMaxRunCheck(dblOrderLineValue, logger)
            'End RWMS-597
        Next

        'Commented for RWMS-597
        'logger.writeSeperator("-", 121)
        'logger.Write("Total Value: ".PadLeft(80) & TotalBillingValue)
        'If (TotalBillingValue < _minperrun) Then
        '    logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
        '    TotalBillingValue = _minperrun
        'ElseIf TotalBillingValue > _maxperrun And _maxperrun > 0 Then
        '    logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
        '    TotalBillingValue = _maxperrun
        'End If
        'End RWMS-597

        Return TotalBillingValue
    End Function

    'Begin RWMS-597
    Private Function MinMaxRunCheck(ByVal dblOrderLineBillValue As Double, ByVal logger As WMS.Logic.LogHandler) As Double


        If (dblOrderLineBillValue < _minperrun) Then
            logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
            dblOrderLineBillValue = _minperrun
        ElseIf dblOrderLineBillValue > _maxperrun And _maxperrun > 0 Then
            logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
            dblOrderLineBillValue = _maxperrun
        End If
        Return dblOrderLineBillValue

    End Function
    'End RWMS-597

    Public Function ProcessBillingChargeDetailPosted(ByVal BILLFROMDATE As Date, ByVal BILLTODATE As Date, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, Optional ByVal ch As ChargeDetail = Nothing) As Int32

        Dim TotalBillingUnits As Double = 0, TotalBillingValue As Double = 0, LineBilling As Double, SubTotalValue As Double = 0
        Dim ret As Int32 = 0
        Try

            logger.Write("Processing Transaction Type: " & TransactionTypeToString(Me.TransactionType))
            'If _nextrundate.Date > runToDate Then
            '    logger.Write("Transaction Already Processed in the past for date: " & runToDate.ToString("d"))
            '    Return Nothing
            'End If

            If (Not Active) Then
                logger.Write("Skipping: Agreement Line Not Active.")
                Return 0
            End If


            TotalBillingUnits = 0
            TotalBillingValue = 0
            LineBilling = 0
            SubTotalValue = 0

            logger.Write("Processing From Date: " & LastRunDate.ToString("d") & " To Date: " & NextRunDate.ToString("d"))

            Dim sql As String
            Dim dt As New DataTable
            Dim dr As DataRow

            sql = "SELECT CONSIGNEE, ORDERID,SUM(numlines) numlines,SUM(qtyshipped) qtyshipped,SUM(numskus) as numskus , sum(numloads) as numloads,SUM(volume) volume,sum(weight) weight,SUM(value) value " & _
                    " FROM vOutBoundBillingChargeDetailPosted " & _
                    " where CONSIGNEE = {0} "

            sql = String.Format(sql, Made4Net.Shared.Util.FormatField(_agreement.Consignee))

            sql += String.Format(" and shippeddate >= {0} and shippeddate <= {1} ", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(BILLFROMDATE)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(BILLTODATE)))

            If Not _documenttype Is Nothing And Not _documenttype = "" Then
                sql += String.Format(" and ordertype = {0} ", Made4Net.Shared.Util.FormatField(_documenttype))
            End If
            If Not _skugroup Is Nothing And Not _skugroup = "" Then
                sql += String.Format(" and skugroup = {0} ", Made4Net.Shared.Util.FormatField(_skugroup))
            End If

            If Not String.IsNullOrEmpty(_picktype) Then
                sql += String.Format(" and picktype = {0} ", Made4Net.Shared.Util.FormatField(_picktype))

                'If _picktype.ToUpper = "LAYERPICK" Then
                '    sql += " and not UNITSPERLOWESTUOM is null and qtyshipped >= UNITSPERLOWESTUOM "
                'ElseIf _picktype = "PARTIAL" Then
                '    sql += " and not UNITSPERLOWESTUOM is null and qtyshipped <= UNITSPERLOWESTUOM "
                'End If
            End If

            If Not String.IsNullOrEmpty(_classname) Then
                sql += String.Format(" and CLASSNAME = {0} ", Made4Net.Shared.Util.FormatField(_classname))
            End If

            If Not String.IsNullOrEmpty(_storageclass) Then
                sql += String.Format(" and STORAGECLASS = {0} ", Made4Net.Shared.Util.FormatField(_storageclass))
            End If


            sql += " GROUP BY CONSIGNEE, ORDERID"

            DataInterface.FillDataset(sql, dt)
            logger.Write(sql)

            Dim cd As ChargeDetail
            For Each dr In dt.Rows

                cd = New ChargeDetail
                cd.CHARGEID = ch.CHARGEID

                cd.AGREEMENTNAME = _agreement.Name 'ch.BILLINGAGREEMENT.Name
                ' cd.CHARGEHEADER = ch
                cd.CHARGELINE = cd.GetNextLineId(ch.CHARGEID)

                cd.AGREEMENTLINE = _line

                cd.BILLFROMDATE = ch.BILLFROMDATE '   _lastrundate
                cd.BILLTODATE = ch.BILLTODATE ' _nextrundate
                cd.CURRENCY = ch.CURRENCY ' _currency
                cd.CHARGETEXT = ch.CHARGETEXT ' _chargedescription

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


                'logger.Write("Order ID: " & dr("orderid"))
                Select Case _billbasis
                    Case AgreementDetail.BillingBasis.HandlingUnits
                        ' get handling units by type
                        Dim HUTSql As String = "SELECT ISNULL(SUM(HUQTY),0) HUQTY FROM HANDLINGUNITTRANSACTION WHERE CONSIGNEE = " & Made4Net.Shared.Util.FormatField(_agreement.Consignee) & " AND ORDERID = " & Made4Net.Shared.Util.FormatField(dr("orderid"))
                        If _hutype <> "" Then
                            HUTSql = HUTSql & " AND HUTYPE = " & Made4Net.Shared.Util.FormatField(_hutype)
                        End If
                        LineBilling = CType(DataInterface.ExecuteScalar(HUTSql), Double)
                        logger.Write(HUTSql)
                    Case AgreementDetail.BillingBasis.NumberOfDucments
                        TotalBillingUnits = dt.Rows.Count
                        logger.Write("Transaction Type".PadRight(15) & "|" & "Document Id".PadRight(20) & "|" & "Billing base units".PadRight(20) & "|" & "Price per unit".PadRight(20) & "|" & "Calculated Value".PadRight(20) & "|")
                        logger.writeSeperator("-"c, 121)
                        TotalBillingValue = Me.getChargeValue(dt.Rows.Count, "OUTBOUND", CType(dr("orderid"), String), 1, logger)
                        logger.Write("number of document: " & dt.Rows.Count)
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
                        LineBilling = CType(dr("qtyshipped"), Double)
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

                TotalBillingValue = Me.getChargeValue(CType(LineBilling, Decimal), "OUTBOUND", CType(dr("orderid"), String), 1, logger)
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

        Return ret

    End Function

End Class