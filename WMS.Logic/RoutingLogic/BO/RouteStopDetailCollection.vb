Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class RouteStopTaskCollection
    Implements ICollection

#Region "Variables"

    Protected _routestop As RouteStop
    Protected _routestoptasks As ArrayList

#End Region

#Region "Properties"

    Public ReadOnly Property RouteStop() As RouteStop
        Get
            Return _routestop
        End Get
    End Property

#Region "Overrides"

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _routestoptasks.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _routestoptasks.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _routestoptasks.SyncRoot
        End Get
    End Property

    Default Public Property Item(ByVal index As Int32) As RouteStopTask
        Get
            Return _routestoptasks(index)
        End Get
        Set(ByVal Value As RouteStopTask)
            _routestoptasks(index) = Value
        End Set
    End Property

    Public ReadOnly Property GetStopDetail(ByVal pConsignee As String, ByVal pOrderId As String) As RouteStopTask
        Get
            For Each oStopDet As WMS.Logic.RouteStopTask In Me
                If oStopDet.Consignee = pConsignee And oStopDet.DocumentId = pOrderId Then
                    Return oStopDet
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property GetStopDetail(ByVal pStopTaskId As Int32) As RouteStopTask
        Get
            For Each oStopDet As WMS.Logic.RouteStopTask In Me
                If oStopDet.StopTaskId = pStopTaskId Then
                    Return oStopDet
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property StopDetailExists(ByVal pConsignee As String, ByVal pOrderId As String) As Boolean
        Get
            For Each oStopDet As WMS.Logic.RouteStopTask In Me
                If oStopDet.Consignee = pConsignee And oStopDet.DocumentId = pOrderId Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property RouteStopGeneralTaskExists(ByVal pTaskId As String) As Boolean
        Get
            For Each oStopDet As WMS.Logic.RouteStopTask In Me
                If oStopDet.DocumentId = pTaskId And oStopDet.StopTaskType = StopTaskType.General Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property GetGeneralTask(ByVal pTaskId As String) As RouteStopTask
        Get
            For Each oStopDet As WMS.Logic.RouteStopTask In Me
                If oStopDet.DocumentId = pTaskId And oStopDet.StopTaskType = StopTaskType.General Then
                    Return oStopDet
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#End Region

#Region "Constructor"

    Public Sub New()
        _routestoptasks = New ArrayList
    End Sub

    Public Sub New(ByVal oRouteStop As RouteStop)
        _routestoptasks = New ArrayList
        _routestop = oRouteStop
        Load()
    End Sub

#End Region

#Region "Methods"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _routestoptasks.CopyTo(array, index)
    End Sub

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _routestoptasks.GetEnumerator
    End Function

    Public Function Add(ByVal rsd As RouteStopTask) As Int32
        Return _routestoptasks.Add(rsd)
    End Function

    Public Sub Remove(ByVal rsd As RouteStopTask)
        _routestoptasks.Remove(rsd)
    End Sub

    Public Sub RemoveAt(ByVal Index As Int32)
        _routestoptasks.RemoveAt(Index)
    End Sub

    Private Sub Load()
        Dim sql As String = String.Format("Select * from routestoptask where routeid = {0} and stopnumber = {1}", Made4Net.Shared.Util.FormatField(_routestop.RouteID), Made4Net.Shared.Util.FormatField(_routestop.StopNumber))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Me.Add(New RouteStopTask(_routestop.RouteID, _routestop.StopNumber, Convert.ToInt32(dr("stoptaskid"))))
        Next
    End Sub

#End Region

End Class
