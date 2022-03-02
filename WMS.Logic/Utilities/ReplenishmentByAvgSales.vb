Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ReplenishmentByAvgSales
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Protected _daysrange As Int32
    Protected _consignee As String
    Protected _ordertype As String
    Protected _maxqtymultipl As Int32

    'Public Sub New(ByVal iDaysRange As String, ByVal sConsignee As String, ByVal sOrderType As String)
    '    MyBase.New()
    '    _consignee = sConsignee
    '    _daysrange = iDaysRange
    '    _ordertype = sOrderType        
    'End Sub

    Public Sub New()
        MyBase.New()
        Dim dt As DataTable = New DataTable
        Try
            DataInterface.FillDataset("SELECT * FROM JOB_SCHEDULE WHERE class_name='WMS.Logic.ReplanishmentByAvgSales'", dt)
            If dt.Rows.Count <> 0 Then
                Dim str() As String = Convert.ToString(dt.Rows(0)("arguments")).Split(",")
                _consignee = str(1)
                _daysrange = str(0)
                _ordertype = str(2)
                _maxqtymultipl = str(3)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Overrides Sub Execute()
        Dim dt As New DataTable

        Dim sql As String = String.Format("SELECT SKU.CONSIGNEE, SKU.SKU, ISNULL(NUMLINES/CHECKDATE,0) AVGNUMLINES, ISNULL(UNITSORDERED/CHECKDATE,0) AVGUNITS " & _
                "FROM SKU LEFT OUTER JOIN " & _
                "(SELECT OD.CONSIGNEE, OD.SKU, COUNT(1) AS NUMLINES, SUM(QTYORIGINAL) AS UNITSORDERED, {0} AS CHECKDATE " & _
                "FROM OUTBOUNDORDETAIL OD JOIN SKU ON OD.CONSIGNEE = SKU.CONSIGNEE AND OD.SKU = SKU.SKU " & _
                "JOIN OUTBOUNDORHEADER OH ON OD.CONSIGNEE = OH.CONSIGNEE AND OD.ORDERID = OH.ORDERID " & _
                "WHERE(SKU.ADDDATE <= DateAdd(DD, - {0}, GETDATE()) AND OH.ORDERTYPE='{2}' And OH.CREATEDATE > DateAdd(DD, - {0}, GETDATE())) " & _
                "GROUP BY OD.CONSIGNEE, OD.SKU " & _
                "UNION " & _
                "SELECT OD.CONSIGNEE, OD.SKU, COUNT(1) AS NUMLINES, SUM(QTYORIGINAL) AS UNITSORDERED, DATEDIFF(DD,SKU.ADDDATE,GETDATE()) AS CHECKDATE " & _
                "FROM OUTBOUNDORDETAIL OD JOIN SKU ON OD.CONSIGNEE = SKU.CONSIGNEE AND OD.SKU = SKU.SKU " & _
                "JOIN OUTBOUNDORHEADER OH ON OD.CONSIGNEE = OH.CONSIGNEE AND OD.ORDERID = OH.ORDERID " & _
                "WHERE(SKU.ADDDATE > DateAdd(DD, - {0}, GETDATE()) AND OH.ORDERTYPE='{2}' And OH.CREATEDATE >= SKU.ADDDATE)" & _
                "GROUP BY OD.CONSIGNEE, OD.SKU, SKU.ADDDATE) AS UNI " & _
                "ON SKU.CONSIGNEE = UNI.CONSIGNEE AND SKU.SKU = UNI.SKU " & _
                "WHERE UNI.CONSIGNEE='{1}' " & _
                "ORDER BY AVGUNITS DESC", _daysrange, _consignee, _ordertype)

        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            DataInterface.RunSQL("UPDATE PICKLOC SET NORMALREPLQTY = '" & dr("AVGUNITS") & "',MAXIMUMQTY = '" & dr("AVGUNITS") * _maxqtymultipl & "' WHERE CONSIGNEE='" & dr("CONSIGNEE") & "' AND SKU='" & dr("SKU") & "'")
        Next

    End Sub

End Class
