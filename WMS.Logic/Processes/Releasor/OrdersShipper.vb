Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class OrdersShipper
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Public Overrides Sub Execute()
        Dim dt As New DataTable
        Dim oOrder As OutboundOrderHeader
        Dim dr As DataRow
        Dim sql As String = String.Format("select distinct consignee,orderid from outboundorheader where status = '{0}'", _
            WMS.Lib.Statuses.OutboundOrderHeader.PICKED)

        DataInterface.FillDataset(sql, dt)
        For i As Int32 = 0 To dt.Rows.Count - 1
            Try
                dr = dt.Rows(i)
                oOrder = New OutboundOrderHeader(dr("consignee"), dr("orderid"))
                If oOrder.FULLYPICKED Then
                    oOrder.Ship("SYSTEM")
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub

End Class
