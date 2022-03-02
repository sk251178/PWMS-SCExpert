Public Class EventManagerQFactory
    Implements IEventManagerQFactory

    Public Function NewEventManagerQ() As IEventManagerQ Implements IEventManagerQFactory.NewEventManagerQ
        Return New EventManagerQ()
    End Function
End Class