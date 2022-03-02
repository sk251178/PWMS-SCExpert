Imports Made4Net.DataAccess

Public Class ManualReplenishment

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Replenish" Then
            For Each dr In ds.Tables(0).Rows
                ManualLocationReplenish(dr("Location"), dr("warehousearea"), dr("Consignee"), dr("SKU"), "")
            Next
        ElseIf CommandName = "ZoneReplenish" Then
            For Each dr In ds.Tables(0).Rows
                ManualZoneReplenish(dr("PUTREGION"), dr("SKU"))
            Next
        End If
    End Sub
    Private Sub ManualLocationReplenish(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pPickRegion As String)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ManualReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", Replenishment.ReplenishmentMethods.ManualReplenishment)
        em.Add("LOCATION", pLocation)
        em.Add("WAREHOUSEAREA", pWarehousearea)
        em.Add("PICKREGION", pPickRegion)
        em.Add("ALL", "0")
        em.Add("SKU", pSku)
        em.Add("CONSIGNEE", pConsignee)

        em.Send(WMSEvents.EventDescription(EventType))
    End Sub
    'PWMS-639/332 added private method to Public 
    Public Sub ManualLocationReplenish(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pPickRegion As String, Optional ByVal plogInUserId As String = "")
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ManualReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", Replenishment.ReplenishmentMethods.ManualReplenishment)
        em.Add("LOCATION", pLocation)
        em.Add("WAREHOUSEAREA", pWarehousearea)
        em.Add("PICKREGION", pPickRegion)
        em.Add("ALL", "0")
        em.Add("SKU", pSku)
        em.Add("CONSIGNEE", pConsignee)
        'Jira 332- added loged in user id for assigning manual replenish task to self
        em.Add("LOGINUSERID", plogInUserId)

        em.Send(WMSEvents.EventDescription(EventType))
    End Sub
    'Ended PWMS-639 

    Private Sub ManualZoneReplenish(ByVal pPutRegion As String, ByVal pSku As String)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ManualReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", Replenishment.ReplenishmentMethods.ManualReplenishment)
        em.Add("LOCATION", "")
        em.Add("WAREHOUSEAREA", "")
        em.Add("PICKREGION", pPutRegion)
        em.Add("ALL", "1")
        em.Add("SKU", pSku)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

End Class
