Public Interface IEventManagerQ
    Sub Add(key As String, value As String)
    Sub Send(label As String)
    Sub Send(label As String, warehouseId As String)

End Interface
