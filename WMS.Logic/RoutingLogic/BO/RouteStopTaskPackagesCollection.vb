Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteStopTaskPackagesCollection
    Implements ICollection

#Region "Variables"

    Protected _routestoptask As RouteStopTask
    Protected _routestoptaskPackages As ArrayList

#End Region

#Region "Properties"

    Public ReadOnly Property RouteStopTask() As RouteStopTask
        Get
            Return _routestoptask
        End Get
    End Property

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _routestoptaskPackages.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _routestoptaskPackages.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _routestoptaskPackages.SyncRoot
        End Get
    End Property

    Public ReadOnly Property GetRouteStopPackage(ByVal pPackageId As String) As RouteStopTaskPackages
        Get
            For Each oStopTaskPackage As WMS.Logic.RouteStopTaskPackages In Me
                If oStopTaskPackage.PackageId = pPackageId Then
                    Return oStopTaskPackage
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property RouteStopPackageExists(ByVal pPackageId As String) As Boolean
        Get
            For Each oStopTaskPackage As WMS.Logic.RouteStopTaskPackages In Me
                If oStopTaskPackage.PackageId = pPackageId Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

#End Region

#Region "Ctor"

    Public Sub New()
        _routestoptaskPackages = New ArrayList
    End Sub

    Public Sub New(ByVal oRouteStopTask As RouteStopTask)
        _routestoptaskPackages = New ArrayList
        _routestoptask = oRouteStopTask
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _routestoptaskPackages.CopyTo(array, index)
    End Sub

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _routestoptaskPackages.GetEnumerator
    End Function

    Public Function Add(ByVal rstp As RouteStopTaskPackages) As Int32
        Return _routestoptaskPackages.Add(rstp)
    End Function

    Public Sub Remove(ByVal rstp As RouteStopTaskPackages)
        _routestoptaskPackages.Remove(rstp)
    End Sub

    Public Sub RemoveAt(ByVal Index As Int32)
        _routestoptaskPackages.RemoveAt(Index)
    End Sub

    Private Sub Load()
        Dim sql As String = String.Format("Select * from routestoptaskpackages where routeid = {0} and stopnumber = {1} and stoptaskid = {2}", Made4Net.Shared.Util.FormatField(_routestoptask.RouteID), Made4Net.Shared.Util.FormatField(_routestoptask.StopNumber), Made4Net.Shared.Util.FormatField(_routestoptask.StopTaskId))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Me.Add(New RouteStopTaskPackages(_routestoptask.RouteID, _routestoptask.StopNumber, _routestoptask.StopTaskId, dr("packageid")))
        Next
    End Sub

#End Region

#End Region

End Class
