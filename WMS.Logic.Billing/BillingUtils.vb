Imports Made4Net.DataAccess

Public Class BillingUtils

#Region "Run Conditions"

    Public Shared Function getInStockBeforeStartPeriodDate(ByVal vLoadId As String, ByVal StartPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim SQL As String = "SELECT COUNT(1) COND FROM BILLINGLOADS "
        SQL = SQL & " WHERE LOADID = '" & vLoadId & "' "
        'SQL = SQL & " AND (ENDDATE IS NULL OR CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) >= '" & StartPeriodDate.ToString("yyyy-MM-dd") & "')"
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) < '" & StartPeriodDate.ToString("yyyy-MM-dd") & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQL, dt)
        If CType(dt.Rows(0)("COND"), Integer) <> 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Shared Function getInStockBeforeStartPeriodDateAndShipped(ByVal vLoadId As String, ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim SQL As String = "SELECT COUNT(1) COND FROM BILLINGLOADS "
        SQL = SQL & " WHERE LOADID = '" & vLoadId & "' "
        SQL = SQL & " AND ENDDATE IS NOT NULL "
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) < '" & StartPeriodDate.ToString("yyyy-MM-dd") & "'"
        SQL = SQL & " AND (CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) < '" & EndPeriodDate.ToString("yyyy-MM-dd") & "')"
        SQL = SQL & " AND (CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) >= '" & StartPeriodDate.ToString("yyyy-MM-dd") & "')"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQL, dt)
        If CType(dt.Rows(0)("COND"), Integer) <> 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Shared Function getInStockBeforeStartPeriodDateAndNotShipped(ByVal vLoadId As String, ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim SQL As String = "SELECT COUNT(1) COND FROM BILLINGLOADS "
        SQL = SQL & " WHERE LOADID = '" & vLoadId & "' "
        SQL = SQL & " AND (CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) >= '" & StartPeriodDate.ToString("yyyy-MM-dd") & "'"
        SQL = SQL & " OR (CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) <= '" & EndPeriodDate.ToString("yyyy-MM-dd") & "' and ENDDATE is not null))"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQL, dt)
        If CType(dt.Rows(0)("COND"), Integer) = 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Shared Function getEnterInSamePeriod(ByVal vLoadId As String, ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim SQL As String = "SELECT COUNT(1) COND FROM BILLINGLOADS "
        SQL = SQL & " WHERE LOADID = '" & vLoadId & "' "
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) >= '" & StartPeriodDate.ToString("yyyy-MM-dd") & "'"
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) < '" & EndPeriodDate.ToString("yyyy-MM-dd") & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQL, dt)
        If CType(dt.Rows(0)("COND"), Integer) <> 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Shared Function getEnterAndExitInSamePeriod(ByVal vLoadId As String, ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim SQL As String = "SELECT COUNT(1) COND FROM BILLINGLOADS "
        SQL = SQL & " WHERE LOADID = '" & vLoadId & "' "
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) < '" & EndPeriodDate.ToString("yyyy-MM-dd") & "'"
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) >= '" & StartPeriodDate.ToString("yyyy-MM-dd") & "'"
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) >= '" & StartPeriodDate.ToString("yyyy-MM-dd") & "'"
        SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) < '" & EndPeriodDate.ToString("yyyy-MM-dd") & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQL, dt)
        If CType(dt.Rows(0)("COND"), Integer) <> 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Shared Function getEnterInSamePeriodAndNotExit(ByVal vLoadId As String, ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim SQL As String = "SELECT COUNT(1) COND FROM BILLINGLOADS "
        SQL = SQL & " WHERE LOADID = '" & vLoadId & "' "
        SQL = SQL & " AND (( ENDDATE IS NOT NULL AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),ENDDATE,101)) < '" & EndPeriodDate.ToString("yyyy-MM-dd") & "') "
        SQL = SQL & " OR CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) < '" & StartPeriodDate.ToString("yyyy-MM-dd") & "')"
        'SQL = SQL & " AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),STARTDATE,101)) < '" & EndPeriodDate.ToString("yyyy-MM-dd") & "')"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQL, dt)
        If CType(dt.Rows(0)("COND"), Integer) = 0 Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

#End Region

    Public Shared Function getBilledUnit(ByVal BillBasis As AgreementDetail.BillingBasis, ByVal dr As DataRow) As Double
        Dim _bUnit As Decimal = 0
        Select Case BillBasis
            Case AgreementDetail.BillingBasis.Units
                _bUnit = CType(dr("UNITS"), Decimal)
            Case AgreementDetail.BillingBasis.NumberOfLines
                _bUnit = CType(dr("NUMLINES"), Decimal)
            Case AgreementDetail.BillingBasis.Volume
                _bUnit = CType(dr("CUBE"), Decimal)
            Case AgreementDetail.BillingBasis.Weight
                _bUnit = CType(dr("WEIGHT"), Decimal)
            Case AgreementDetail.BillingBasis.Value
                _bUnit = CType(dr("VALUE"), Decimal)
            Case AgreementDetail.BillingBasis.NumberOfSkus
                _bUnit = CType(dr("NUMSKUS"), Decimal)
            Case AgreementDetail.BillingBasis.NumberOfLoads
                _bUnit = CType(dr("LOADS"), Decimal)
        End Select
        Return _bUnit
    End Function

    Public Shared Function PeriodTypeToString(ByVal pt As PeriodTypes) As String
        Select Case pt
            Case PeriodTypes.Day
                Return "DAY"
            Case PeriodTypes.Month
                Return "MONTH"
            Case PeriodTypes.Week
                Return "WEEK"
            Case PeriodTypes.Year
                Return "YEAR"
            Case PeriodTypes.Dates
                Return "DATES"
            Case Else
                Return ""
        End Select
    End Function

    Public Shared Function PeriodTypesFromString(ByVal pt As String) As PeriodTypes
        If pt Is Nothing Then Return PeriodTypes.None
        Select Case pt.ToUpper
            Case "DAY"
                Return PeriodTypes.Day
            Case "MONTH"
                Return PeriodTypes.Month
            Case "WEEK"
                Return PeriodTypes.Week
            Case "YEAR"
                Return PeriodTypes.Year
            Case "DATES"
                Return PeriodTypes.Dates
            Case Else
                Return PeriodTypes.None
        End Select
    End Function

    Public Shared Function getBilledUnit(ByVal BillBasis As AgreementDetail.BillingBasis, ByVal consignee As String, ByVal sku As String, ByVal units As Double, ByVal uom As String, ByVal val As Decimal) As Double
        Dim _bUnit As Double = 0
        Try
            Select Case BillBasis
                Case AgreementDetail.BillingBasis.Units
                    _bUnit = WMS.Logic.Inventory.ConvertUom(consignee, sku, units, Nothing, uom)
                Case AgreementDetail.BillingBasis.Value
                    _bUnit = units * val
                Case AgreementDetail.BillingBasis.Volume
                    _bUnit = WMS.Logic.Inventory.CalculateVolume(consignee, sku, units, uom)
                Case AgreementDetail.BillingBasis.Weight
                    _bUnit = WMS.Logic.Inventory.CalculateWeight(consignee, sku, units, uom)
            End Select
        Catch ex As Exception

        End Try
        Return _bUnit
    End Function

    Public Shared Function getNextRunDate(ByVal pt As PeriodTypes, ByVal DatePeriod As String) As Int32
        Dim dp As Int32 = CType(DatePeriod, Int32)
        Select Case pt
            Case PeriodTypes.Day
                Return dp
            Case PeriodTypes.Week
                Return dp * 7
            Case PeriodTypes.Month
                Return dp * 30
            Case PeriodTypes.Year
                Return dp * 365
        End Select
        Return 0
    End Function

    Public Shared Function getNextRunDate(ByVal pt As PeriodTypes, ByVal LastRunDate As Date, ByVal DatePeriod As String) As Date
        Dim dp As Int32 = CType(DatePeriod, Int32)
        Select Case pt
            Case PeriodTypes.Day
                Return LastRunDate.AddDays(dp)
            Case PeriodTypes.Week
                Return LastRunDate.AddDays(dp * 7)
            Case PeriodTypes.Month
                Return LastRunDate.AddMonths(dp)
            Case PeriodTypes.Year
                Return LastRunDate.AddYears(dp)
            Case PeriodTypes.Dates
                Return getNextRunDateForDates(LastRunDate, DatePeriod)
        End Select
        Return Nothing
    End Function

    Public Shared Function getNextRunDateForDates(ByVal LastRunDate As Date, ByVal DatePeriod As String) As Date
        Dim arr() As String = DatePeriod.Split(",".ToCharArray())
        Dim nextrun As DateTime
        Dim dateSet As Boolean = False
        Dim day As Int32 = LastRunDate.Day
        For i As Int32 = 0 To arr.Length - 1
            If Convert.ToInt32(arr(i)) > day Then
                nextrun = LastRunDate.AddDays(Convert.ToInt32(arr(i)) - day)
                dateSet = True
                Exit For
            End If
        Next
        If Not dateSet Then 'This can only happen when we are @ the end of the period array and the next run is the next month
            nextrun = LastRunDate.AddDays(Convert.ToInt32(arr(0)) - day).AddMonths(1)
        End If
        Return nextrun
    End Function

    Public Shared Function getLastRunDate(ByVal pt As PeriodTypes, ByVal LastRunDate As Date, ByVal DatePeriod As Int32) As Date
        Select Case pt
            Case PeriodTypes.Day
                Return LastRunDate.AddDays(DatePeriod)
            Case PeriodTypes.Week
                Return LastRunDate.AddDays(DatePeriod * 7)
            Case PeriodTypes.Month
                Return LastRunDate.AddMonths(DatePeriod)
            Case PeriodTypes.Year
                Return LastRunDate.AddYears(DatePeriod)
        End Select
        Return Nothing
    End Function

End Class