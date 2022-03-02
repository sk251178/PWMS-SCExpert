Imports Made4Net.Shared

<CLSCompliant(False)> Public Class Shipping
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim ordheader As OutboundOrderHeader
        Dim qs As New QMsgSender
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Ship" Then
            qs.Add("ACTION", "SHIP")
            qs.Add("USER", Common.GetCurrentUser)
            Dim shp As Boolean = False
            For Each dr In ds.Tables(0).Rows
                ordheader = New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                Try
                    ordheader.Ship(Common.GetCurrentUser)
                    If ordheader.CONSIGNEE.ToUpper = "COOP" Or ordheader.CONSIGNEE.ToUpper = "ZARA" Then
                        qs.Add("CONSIGNEE", ordheader.CONSIGNEE)
                        qs.Add("ORDERID", ordheader.ORDERID)
                        shp = True
                    End If
                Catch ex As Exception
                    Message = "Not all orders where Shipped, Bad Status"
                End Try
            Next
            If shp Then
                qs.Send("Uploader", "Shipping")
            End If
        End If
    End Sub
End Class
