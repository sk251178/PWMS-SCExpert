Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class ConstantAgreementDetail
    Inherits AgreementDetail

    Public Sub New(ByRef oAgreement As Agreement, ByVal pLINE As Int32)
        MyBase.New(oAgreement, pLINE)
    End Sub

    Public Sub New(ByRef oAgreement As Agreement)
        MyBase.New(oAgreement)
    End Sub

    Public Overrides Function Process(ByVal runToDate As DateTime, ByVal logger As WMS.Logic.LogHandler, ByVal pUser As String, ByVal SaveStorageTransactions As String, Optional ByVal ch As ChargeHeader = Nothing) As ChargeDetailCollection
        logger.Write("Processing Transaction Type: " & TransactionTypeToString(Me.TransactionType))
        logger.Write("Processing From Date: " & LastRunDate.ToString("d") & " To Date: " & NextRunDate.ToString("d"))
        Dim TotalBillingValue As Double = 0

        If (Not Active) Then
            logger.Write("Skipping: Agreement Line Not Active.")
            Return Nothing
        End If


        If _nextrundate.Date > runToDate Then
            logger.Write("Transaction Already Processed in the past for date: " & runToDate.ToString("d"))
            Return Nothing
        End If


        Dim cdc As New ChargeDetailCollection
        While _nextrundate.Date <= runToDate

            TotalBillingValue = 0
            logger.Write("Processing From Date: " & LastRunDate.ToString("d") & " To Date: " & NextRunDate.ToString("d"))

            Dim cd As New ChargeDetail
            cd.ChargeHeader = ch
            cd.ChargeLine = cd.GetNextLineId(ch.ChargeId)

            cd.AgreementName = ch.BillingAgreement.Name
            cd.AgreementLine = _line
            cd.BillFromDate = _lastrundate
            cd.BillToDate = _nextrundate
            cd.Currency = _currency
            cd.ChargeText = _chargedescription

            TotalBillingValue = _priceperunit
            logger.Write("Total Value: " & _priceperunit)
            If _priceperunit < _minperrun Then
                logger.Write("Total Value is Less then Mimimum for run: " & _minperrun & " Total Value Updated to Minimum")
                TotalBillingValue = _minperrun
            ElseIf (TotalBillingValue > _maxperrun) Then
                logger.Write("Total Value is Greater then Maximum for run: " & _maxperrun & " Total Value Updated to Maximum")
                TotalBillingValue = _maxperrun
            End If

            cd.BillTotal = TotalBillingValue
            cd.BilledUnits = 1
            If cd.BillTotal > 0 Then
                cd.post(pUser)
                cdc.add(cd)
            End If

            setNextRunDate()
        End While
        Return cdc
    End Function
End Class
