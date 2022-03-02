Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteStopTaskItemsCollection
    Implements ICollection

#Region "Variables"

    Protected _routestoptask As RouteStopTask
    Protected _routestoptaskItems As ArrayList

#End Region

#Region "Properties"

    Public ReadOnly Property RouteStopTask() As RouteStopTask
        Get
            Return _routestoptask
        End Get
    End Property

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _routestoptaskItems.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _routestoptaskItems.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _routestoptaskItems.SyncRoot
        End Get
    End Property

    Public ReadOnly Property GetRouteStopItem(ByVal pItem As String) As RouteStopTaskItems
        Get
            For Each oStopTaskItem As WMS.Logic.RouteStopTaskItems In Me
                If oStopTaskItem.Item = pItem Then
                    Return oStopTaskItem
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property RouteStopPackageExists(ByVal pItem As String) As Boolean
        Get
            For Each oStopTaskItem As WMS.Logic.RouteStopTaskItems In Me
                If oStopTaskItem.Item = pItem Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

#End Region

#Region "Ctor"

    Public Sub New()
        _routestoptaskItems = New ArrayList
    End Sub

    Public Sub New(ByVal oRouteStopTask As RouteStopTask)
        _routestoptaskItems = New ArrayList
        _routestoptask = oRouteStopTask
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _routestoptaskItems.CopyTo(array, index)
    End Sub

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _routestoptaskItems.GetEnumerator
    End Function

    Public Function Add(ByVal rstp As RouteStopTaskItems) As Int32
        Return _routestoptaskItems.Add(rstp)
    End Function

    Public Sub Remove(ByVal rstp As RouteStopTaskItems)
        _routestoptaskItems.Remove(rstp)
    End Sub

    Public Sub RemoveAt(ByVal Index As Int32)
        _routestoptaskItems.RemoveAt(Index)
    End Sub

    Private Sub Load()
        Dim sql As String = String.Format("Select * from routestoptaskitems where routeid = {0} and stopnumber = {1} and stoptaskid = {2}", Made4Net.Shared.Util.FormatField(_routestoptask.RouteID), Made4Net.Shared.Util.FormatField(_routestoptask.StopNumber), Made4Net.Shared.Util.FormatField(_routestoptask.StopTaskId))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Me.Add(New RouteStopTaskItems(_routestoptask.RouteID, _routestoptask.StopNumber, _routestoptask.StopTaskId, dr("item"), dr("uom")))
        Next
    End Sub

#End Region

#End Region

End Class
