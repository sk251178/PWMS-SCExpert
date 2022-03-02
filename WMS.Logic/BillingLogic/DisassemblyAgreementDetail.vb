Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class DisassemblyAgreementDetail
    Inherits AgreementDetail

    Public Sub New(ByRef oAgreement As Agreement, ByVal pLINE As Int32)
        MyBase.New(oAgreement, pLINE)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement)
        MyBase.New(oAgreement)
    End Sub

    Public Overrides Function Process(ByVal runToDate As Date, ByVal logger As LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection
        Dim TotalBillingUnits As Double = 0, TotalBillingValue As Double = 0, LineBilling As Double, SubTotalValue As Double = 0

        logger.Write("Processing Transaction Type: " & TransactionTypeToString(Me.TransactionType))
        If _nextrundate.Date > runToDate Then
            logger.Write("Transaction Already Processed in the past for date: " & runToDate.ToString("d"))
            Return Nothing
        End If

        If (Not Active) Then
            logger.Write("Skipping: Agreement Line Not Active.")
            Return Nothing
        End If

        Dim cdc As New ChargeDetailCollection
        While _nextrundate.Date <= runToDate

            TotalBillingUnits = 0
            TotalBillingValue = 0
            LineBilling = 0
            SubTotalValue = 0

            logger.Write("Processing From Date: " & LastRunDate.ToString("d") & " To Date: " & NextRunDate.ToString("d"))

            Dim sql As String
            Dim dt As New DataTable
            Dim dr As DataRow

            sql = String.Format("SELECT ORDERID, " & _
                                "COUNT(ORDERID) AS NUMLINES, " & _
                                "SUM(COMPLETEDQTY) AS COMPLETEDQTY, " & _
                                "SUM((COMPLETEDQTY / skuuom.unitspermeasure) * skuuom.volume) as VOLUME, " & _
                                "SUM((COMPLETEDQTY/ skuuom.unitspermeasure) * skuuom.grossweight) as WEIGHT, " & _
                                "SUM(COMPLETEDQTY) as NUMLOADS, " & _
                                "COUNT(DISTINCT WORKORDERHEADER.SKU) as NUMSKUS, " & _
                                "SUM(COMPLETEDQTY * sku.unitprice) as VALUE " & _
                                "FROM WORKORDERHEADER INNER JOIN SKU ON WORKORDERHEADER.CONSIGNEE=SKU.CONSIGNEE AND WORKORDERHEADER.SKU=SKU.SKU " & _
                                "INNER JOIN SKUUOM ON WORKORDERHEADER.CONSIGNEE = SKUUOM.CONSIGNEE AND WORKORDERHEADER.SKU = SKUUOM.SKU AND SKUUOM.LOWERUOM='' " & _
                                "WHERE WORKORDERHEADER.STATUS='COMPLETE' AND WORKORDERHEADER.DOCUMENTTYPE='DISASMBL' AND " & _
                                " CLOSEDATE >= {0} AND CLOSEDATE <= {1} " & _
                                " AND WORKORDERHEADER.CONSIGNEE = {2} ", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_lastrundate.Date)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(_nextrundate.AddSeconds(-1))), _
                                Made4Net.Shared.Util.FormatField(_agreement.Consignee))

            If Not _documenttype Is Nothing And Not _documenttype = "" Then
                sql += String.Format(" AND ORDERTYPE = {0} ", Made4Net.Shared.Util.FormatField(_documenttype))
            End If
            If Not _skugroup Is Nothing And Not _skugroup = "" Then
                sql += String.Format(" And sku.skugroup = {0} ", Made4Net.Shared.Util.FormatField(_skugroup))
            End If

            sql += "GROUP BY ORDERID "

            DataInterface.FillDataset(sql, dt)

            Dim cd As New ChargeDetail
            cd.ChargeHeader = ch
            cd.ChargeLine = cd.GetNextLineId(ch.ChargeId)

            cd.AgreementName = ch.BillingAgreement.Name
            cd.AgreementLine = _line
            cd.BillFromDate = _lastrundate
            cd.BillToDate = _nextrundate
            cd.Currency = _currency
            cd.ChargeText = _chargedescription

            logger.Write("Transaction Type".PadRight(15) & "|" & "Document Id".PadRight(20) & "|" & "Billing base units".PadRight(20) & "|" & "Price per unit".PadRight(20) & "|" & "Calculated Value".PadRight(20) & "|")
            logger.writeSeperator("-", 121)

            ''calc price factor
            If Not logger Is Nothing Then
                logger.Write("Calculating price factor...")
            End If
            Dim oPriceFactorCalc As New PriceFactorCalc
            _pricefactor = oPriceFactorCalc.CalculatePriceFactor(_pricefactorindex, _
                _lastrundate.ToString("dd/MM/yyyy"), Nothing).ToString()
            If Not logger Is Nothing Then
                logger.Write("Price Factor:" & _pricefactor)
                logger.Write("Start Date For Storage Range:" & _lastrundate.ToString("dd/MM/yyyy"))
            End If
            ''---

            For Each dr In dt.Rows
                'logger.Write("Order ID: " & dr("orderid"))
                Select Case _billbasis
                    Case AgreementDetail.BillingBasis.NumberOfDucments
                        TotalBillingUnits = dt.Rows.Count
                        TotalBillingValue = Me.getChargeValue(dt.Rows.Count, "OUTBOUND", dr("orderid"), 1, logger)
                        logger.Write("number of document: " & dt.Rows.Count)
                        Exit For
                    Case AgreementDetail.BillingBasis.NumberOfLines
                        LineBilling = dr("NUMLINES")
                    Case AgreementDetail.BillingBasis.NumberOfLoads
                        LineBilling = dr("NUMLOADS")
                    Case AgreementDetail.BillingBasis.NumberOfSkus
                        LineBilling = dr("NUMSKUS")
                    Case AgreementDetail.BillingBasis.Units
                        LineBilling = dr("COMPLETEDQTY")
                    Case AgreementDetail.BillingBasis.Value
                        LineBilling = dr("VALUE")
                    Case AgreementDetail.BillingBasis.Volume
                        LineBilling = dr("VOLUME")
                    Case AgreementDetail.BillingBasis.Weight
                        LineBilling = dr("WEIGHT")
                End Select

                TotalBillingUnits += LineBilling
                TotalBillingValue += Me.getChargeValue(LineBilling, "OUTBOUND", dr("orderid"), 1, logger)
            Next
            logger.writeSeperator("-", 121)
            logger.Write("Total Value: ".PadLeft(80) & TotalBillingValue)
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
        End While

        Return cdc
    End Function
End Class
