Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class DaysOfInventory
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Protected _consignee As String
    Protected _adays As Int32
    Protected _bdays As Int32
    Protected _cdays As Int32
    Protected _checkdays As Int32

    Public Sub New(ByVal sConsignee As String, ByVal iAcutOff As String, ByVal iBcutOff As String, ByVal iCcutOff As String, ByVal iDaysCheck As String)
        MyBase.New()
        Try
            _adays = iAcutOff
        Catch ex As Exception
            _adays = 0
        End Try
        Try
            _bdays = iBcutOff
        Catch ex As Exception
            _bdays = 0
        End Try
        Try
            _cdays = iCcutOff
        Catch ex As Exception
            _cdays = 0
        End Try
        _consignee = sConsignee
        _checkdays = iDaysCheck
    End Sub

    Public Overrides Sub Execute()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("select sku.consignee, sku.sku, isnull(numlines/checkdate,0) AvgNumLines, isnull(unitsordered/checkdate,0) AvgUnits from sku left outer join " & _
            "(select od.consignee, od.sku, count(1) as numlines, sum(qtyoriginal) as unitsordered, {0} as Checkdate " & _
            "from outboundordetail od join sku on od.consignee = sku.consignee and od.sku = sku.sku " & _
            "join outboundorheader oh on od.consignee = oh.consignee and od.orderid = oh.orderid " & _
            "where(SKU.ADDDATE <= DateAdd(dd, -{0}, getdate()) And oh.createdate > DateAdd(dd, -{0}, getdate())) " & _
            "group by od.consignee, od.sku " & _
            "union " & _
            "select od.consignee, od.sku, count(1) as numlines, sum(qtyoriginal) as unitsordered, datediff(dd,sku.adddate,getdate()) as Checkdate " & _
            "from outboundordetail od join sku on od.consignee = sku.consignee and od.sku = sku.sku " & _
            "join outboundorheader oh on od.consignee = oh.consignee and od.orderid = oh.orderid " & _
            "where sku.adddate > dateadd(dd,-{0},getdate()) and oh.createdate >= sku.adddate " & _
            "group by od.consignee, od.sku, sku.adddate) as Uni " & _
            "on sku.consignee = uni.consignee and sku.sku = uni.sku where sku.consignee like '{1}'", _checkdays, _consignee)

        sql = sql & " ORDER BY AvgUnits desc"

        DataInterface.FillDataset(sql, dt)
        Dim sk As SKU

        For Each dr In dt.Rows
            sk = New SKU(dr("CONSIGNEE"), dr("SKU"))
            If sk.VELOCITY = "A" Then
                sk.ONSITEMIN = dr("avgUnits") * _adays
            ElseIf sk.VELOCITY = "B" Then
                sk.ONSITEMIN = dr("avgUnits") * _bdays
            ElseIf sk.VELOCITY = "C" Then
                sk.ONSITEMIN = dr("avgUnits") * _cdays
            End If
            sk.Save()
        Next

        dt.Dispose()
    End Sub

End Class
