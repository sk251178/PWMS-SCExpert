
#Region "Requirments Baskets"

<CLSCompliant(False)> Public Class RequirmentsBaskets
    Implements ICollection

#Region "Variables"
    Protected _reqs As ArrayList
    Protected _pickmethod As String
#End Region

#Region "Properties"
    Default Public Property Item(ByVal index As Int32) As PickBaskets
        Get
            Return CType(_reqs(index), PickBaskets)
        End Get
        Set(ByVal Value As PickBaskets)
            _reqs(index) = Value
        End Set
    End Property
    Public ReadOnly Property PickMethod() As String
        Get
            Return _pickmethod
        End Get
    End Property
#End Region

#Region "Ctor"
    Public Sub New()
        _reqs = New ArrayList
    End Sub
    Public Sub New(ByVal PckMethod As String)
        _reqs = New ArrayList
        _pickmethod = PckMethod
    End Sub
#End Region

#Region "Methods"
    Public Sub Place(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing)
        Dim pckbasket As PickBaskets
        For Each pckbasket In _reqs

            If pckbasket.CanPlace(req, oLogger) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("CanPlace returned TRUE, pickmethod=" & pckbasket.PickMethod & ",req SKU=" & req.Sku & ",req inventoryStatus=" & req.InventoryStatus)
                End If
                pckbasket.Place(req, oLogger)
                Exit Sub
            End If
        Next
        If Not oLogger Is Nothing Then
            oLogger.Write("Creating new Requirment basket for pick method " & _pickmethod)
        End If
        Dim newbasket As PickBaskets
        Select Case _pickmethod
            Case WMS.Lib.PickMethods.PickMethod.PICKBYORDER
                newbasket = New PickByOrderStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYITEM
                newbasket = New PickByItemStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYCUSTOMER
                newbasket = New PickByCustomerStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYCOMPANYGROUP
                newbasket = New PickByCompanyGroupStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYSHIPTO
                newbasket = New PickByShipToStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYROUTE
                newbasket = New PickByRouteStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYTRUCK
                newbasket = New PickByTruckStrategy
            Case WMS.Lib.PickMethods.PickMethod.PICKBYWAVE
                newbasket = New PickByWaveStrategy
            Case WMS.Lib.PickMethods.PickMethod.DISCREETPICK
                newbasket = New PickDiscreet
                'Case WMS.Lib.PickMethods.PickMethod.PARALELORDERPICKING
                'newbasket = New PickParalelStrategy
        End Select

        newbasket.Place(req, oLogger)
        Add(newbasket)
    End Sub
#End Region

#Region "Overrides"
    Public Function Add(ByVal value As PickBaskets) As Integer
        Return _reqs.Add(value)
    End Function
    Public Sub Clear()
        _reqs.Clear()
    End Sub
    Public Sub Insert(ByVal index As Integer, ByVal value As PickBaskets)
        _reqs.Insert(index, value)
    End Sub
    Public Sub Remove(ByVal value As PickBaskets)
        _reqs.Remove(value)
    End Sub
    Public Sub RemoveAt(ByVal index As Integer)
        _reqs.RemoveAt(index)
    End Sub
    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _reqs.CopyTo(array, index)
    End Sub
    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _reqs.Count
        End Get
    End Property
    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _reqs.IsSynchronized
        End Get
    End Property
    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _reqs.SyncRoot
        End Get
    End Property
    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _reqs.GetEnumerator()
    End Function
#End Region

End Class

#End Region