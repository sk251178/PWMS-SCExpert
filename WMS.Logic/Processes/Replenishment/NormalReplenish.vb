Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class NormalReplenish
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Protected _all As String
    Protected _location As String
    Protected _warehousearea As String
    Protected _pickregion As String
    Protected _sku As String

    Public Sub New()
    End Sub

    Public Sub New(ByVal sLocation As String, ByVal sWarehousearea As String, ByVal sPickRegion As String, ByVal sSku As String)
        MyBase.New()
        Try
            If Not sLocation Is Nothing Then _location = sLocation Else _location = ""
            If Not sWarehousearea Is Nothing Then _warehousearea = sWarehousearea Else _warehousearea = ""
        Catch ex As Exception
            _warehousearea = ""
            _location = ""
        End Try
        Try
            If Not sPickRegion Is Nothing Then _pickregion = sPickRegion Else _pickregion = ""
        Catch ex As Exception
            _pickregion = ""
        End Try
        Try
            If Not sSku Is Nothing Then _sku = sSku Else _sku = ""
        Catch ex As Exception
            _sku = ""
        End Try
        Try
            _all = "1"
        Catch ex As Exception
        End Try
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Replenish" Then
            For Each dr In ds.Tables(0).Rows
                NormalReplenish(dr("Location"), dr("Warehousearea"), dr("consignee"), dr("sku"), "")
            Next
        End If
    End Sub
    'Added for RWMS-1840   
    Dim ReplPolicyMethod As String = WMS.Logic.Replenishment.GetPolicyID()
    'Ended for RWMS-1840  

    Public Sub NormalReplenish(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pPickRegion As String, Optional ByVal pReplenishAll As Boolean = False, Optional ByVal pMinQtyOverride As Decimal = -1, Optional ByVal pTaskTimeLimit As Decimal = -1)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.NormalReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", ReplPolicyMethod)
        em.Add("LOCATION", pLocation)
        em.Add("WAREHOUSEAREA", pWarehousearea)
        em.Add("PICKREGION", pPickRegion)
        If Not pReplenishAll Then
            em.Add("ALL", "0")
        Else
            em.Add("ALL", "1")
        End If
        em.Add("SKU", pSku)
        em.Add("CONSIGNEE", pConsignee)
        em.Add("MINQTYOVERRIDE", pMinQtyOverride)
        em.Add("TASKSTIMELIMIT", pTaskTimeLimit)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Overrides Sub Execute()
        'New Code to replenish with the service - added by Udi
        Try
            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.NormalReplenishment
            em.Add("EVENT", EventType)
            em.Add("REPLMETHOD", ReplPolicyMethod)
            em.Add("LOCATION", _location)
            em.Add("WAREHOUSEAREA", _warehousearea)
            em.Add("PICKREGION", _pickregion)
            em.Add("ALL", "1")
            em.Add("SKU", _sku)
            em.Send(WMSEvents.EventDescription(EventType))
        Catch ex As Exception
        End Try
    End Sub

End Class
