Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ABC
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Protected _dayscutoff As Int32
    Protected _checktype As String
    Protected _aper As Int32
    Protected _bper As Int32

    Public Sub New(ByVal iDaysCutOff As String, ByVal sCheckType As String, ByVal aPercent As String, ByVal bPercent As String)
        MyBase.New()
        Try
            _dayscutoff = iDaysCutOff
        Catch ex As Exception
            _dayscutoff = 30
        End Try
        _checktype = sCheckType
        If _checktype Is Nothing Or _checktype = String.Empty Then
            _checktype = "LINES"
        End If
        Try
            _aper = aPercent
        Catch ex As Exception
            _aper = 10
        End Try
        Try
            _bper = bPercent
        Catch ex As Exception
            _bper = 80
        End Try
    End Sub

    Public Sub New()
        MyBase.New()
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
            "on sku.consignee = uni.consignee and sku.sku = uni.sku", _dayscutoff)
        If _checktype = "LINES" Then
            sql = sql & " ORDER BY avgNumLines desc"
        Else
            sql = sql & " ORDER BY AvgUnits desc"
        End If

        DataInterface.FillDataset(sql, dt)
        Dim cntrec As Int32 = dt.Rows.Count
        Dim idx, lastidx As Int32
        Dim sk As SKU

        For idx = 0 To cntrec * _aper / 100
            dr = dt.Rows(idx)
            sk = New SKU(dr("CONSIGNEE"), dr("SKU"))
            sk.VELOCITY = "A"
            sk.Save()
        Next
        lastidx = idx
        For idx = lastidx To lastidx + (cntrec * _bper / 100)
            dr = dt.Rows(idx)
            sk = New SKU(dr("CONSIGNEE"), dr("SKU"))
            sk.VELOCITY = "B"
            sk.Save()
        Next
        lastidx = idx
        For idx = lastidx To cntrec - 1
            dr = dt.Rows(idx)
            sk = New SKU(dr("CONSIGNEE"), dr("SKU"))
            sk.VELOCITY = "C"
            sk.Save()
        Next

        dt.Dispose()
    End Sub
End Class
