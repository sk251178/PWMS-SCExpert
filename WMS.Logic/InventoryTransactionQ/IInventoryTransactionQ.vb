Public Interface IInventoryTransactionQ
    Sub Add(key As String, value As String)
    Sub Send(label As String)

End Interface
