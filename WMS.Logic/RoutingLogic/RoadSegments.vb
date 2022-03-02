<CLSCompliant(False)> Public Class RoadSegments
    Implements ICollection

    Protected _segs As ArrayList

    Public Sub New(ByVal dt As DataTable)
        _segs = New ArrayList
        For Each dr As DataRow In dt.Rows
            Me.Add(New MapPoint(dr("xpos"), dr("ypos")))
        Next
    End Sub

    Default Public Property Item(ByVal index As Int32) As MapPoint
        Get
            Return _segs(index)
        End Get
        Set(ByVal Value As MapPoint)
            _segs(index) = Value
        End Set
    End Property

    Public Function Add(ByVal mp As MapPoint) As Int32
        Return _segs.Add(mp)
    End Function

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _segs.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _segs.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _segs.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _segs.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _segs.GetEnumerator
    End Function
End Class
