Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class RouteStopCollection
    Implements ICollection

#Region "Variables"

    Protected _route As Route
    Protected _routestops As ArrayList

#End Region

#Region "Properties"

#Region "Overrides"

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _routestops.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _routestops.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _routestops.SyncRoot
        End Get
    End Property

    Default Public Property Item(ByVal index As Int32) As RouteStop
        Get
            If index = -1 Then
                Return Nothing
            Else
                Return _routestops(index)
            End If
        End Get
        Set(ByVal Value As RouteStop)
            _routestops(index) = Value
        End Set
    End Property

    Public ReadOnly Property GetStop(ByVal PointId As String) As RouteStop
        Get
            For Each oStop As WMS.Logic.RouteStop In Me
                If oStop.PointId = PointId Then
                    Return oStop
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property GetStop(ByVal pStopNumber As Int32) As RouteStop
        Get
            For Each oStop As WMS.Logic.RouteStop In Me
                If oStop.StopNumber = pStopNumber Then
                    Return oStop
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property StopExists(ByVal PointId As String) As Boolean
        Get
            For Each oStop As WMS.Logic.RouteStop In Me
                If oStop.PointId = PointId Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

#End Region

#End Region

#Region "Constructor"
    Public Sub New(ByVal oRoute As Route)
        _routestops = New ArrayList
        _route = oRoute
        Load()
    End Sub
#End Region

#Region "Methods"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _routestops.CopyTo(array, index)
    End Sub

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _routestops.GetEnumerator
    End Function

    Public Function Add(ByVal rsd As RouteStop) As Int32
        Return _routestops.Add(rsd)
    End Function

    Public Sub Remove(ByVal rsd As RouteStop)
        _routestops.Remove(rsd)
    End Sub

    Public Sub RemoveAt(ByVal Index As Int32)
        _routestops.RemoveAt(Index)
    End Sub

    Public Sub Clear()
        _routestops.Clear()
    End Sub
    Private Sub Load()
        Dim sql As String = String.Format("Select * from ROUTESTOP where routeid = {0} order by stopnumber", Made4Net.Shared.Util.FormatField(_route.RouteId))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Me.Add(New RouteStop(_route, dr))
        Next
    End Sub

#End Region

End Class
