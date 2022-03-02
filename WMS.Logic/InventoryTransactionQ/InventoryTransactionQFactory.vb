
Public Class InventoryTransactionQFactory
    Implements IInventoryTransactionQFactory

    Public Function NewInventoryTransactionQ() As IInventoryTransactionQ Implements IInventoryTransactionQFactory.NewInventoryTransactionQ
        Return New InventoryTransactionQ()
    End Function
End Class