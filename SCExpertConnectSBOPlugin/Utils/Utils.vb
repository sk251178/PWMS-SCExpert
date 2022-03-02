Public Class Utils

#Region "Documents"

    Public Enum DocumentType
        Invoice = 13
        CreditNotes = 14
        OutboundDeliveryNote = 15
        OutboundReturn = 16
        OutboundOrder = 17
        PurchaseInvoice = 18
        PurchaseCreditNotes = 19
        PurchaseDeliveryNote = 20
        PurchaseReturn = 21
        PurchaseOrder = 22
        Quotations = 23
        InventoryAdjustmentGenEtry = 59
        InventoryAdjustmentGenExit = 60
        WHTransfer = 67
        WorkOrder = 68
        Draft = 112
        ProductionOrder = 202
        InventoryAdjustment = 300
    End Enum

#End Region

#Region "Pick Lists Statuses"

    Public Class SBOPickListStatuses
        Public Const Closed = "Closed"
        Public Const Released = "Released"
        Public Const Picked = "Picked"
        Public Const PartiallyPicked = "PartiallyPicked"
        Public Const PartiallyDelivered = "PartiallyDelivered"
    End Class

#End Region
    
End Class
