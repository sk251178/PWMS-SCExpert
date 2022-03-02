<CLSCompliant(False)> Public Class ParallelPicksBasket
    Implements ICollection

#Region "Variables"

    Protected _pcklsts As PicklistList
    Protected _pcklstscol As ArrayList
    Protected _strats As PlanStrategies

#End Region

#Region "Constructor"

    Public Sub New()
        _strats = New PlanStrategies
        _pcklsts = New PicklistList
        _pcklstscol = New ArrayList
    End Sub

    Public Sub New(ByVal starts As PlanStrategies)
        _strats = starts
        _pcklsts = New PicklistList
        _pcklstscol = New ArrayList
    End Sub

#End Region

#Region "Properties"

    Default Public ReadOnly Property Item(ByVal index As Int32) As ReleasePickListCollection
        Get
            Return _pcklstscol(index)
        End Get
    End Property

    Public ReadOnly Property PickLists() As PicklistList
        Get
            Return _pcklsts
        End Get
    End Property

#End Region

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _pcklstscol.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _pcklstscol.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _pcklstscol.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _pcklstscol.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _pcklstscol.GetEnumerator
    End Function

#End Region

#Region "Methods"

    Public Sub Add(ByVal pcklst As Picklist)
        _pcklsts.Add(pcklst)
    End Sub

    Public Sub AddBasket(ByVal pckbasket As ReleasePickListCollection)
        _pcklstscol.Add(pckbasket)
    End Sub

    Public Sub Release()
        setPickBaskets()
        Dim parbasket As ReleasePickListCollection
        For Each parbasket In Me
            parbasket.Release()
        Next
    End Sub

    Protected Function setPickBaskets()
        Dim pck As Picklist
        Dim strat As PlanStrategy
        Dim relpcklsts As ReleasePickListCollection
        Dim parallelrelstrat As ParallelReleaseStrategyDetail
        Dim placed As Boolean
        Dim sTaskSubType As String
        For Each pck In _pcklsts
            placed = False
            strat = _strats(pck.StrategyId)
            sTaskSubType = GetTaskSubType(pck)
            For Each parallelrelstrat In strat.ParallelReleaseStrategyDetails
                If (parallelrelstrat.HandelingUnitType Is Nothing Or parallelrelstrat.HandelingUnitType = String.Empty Or parallelrelstrat.HandelingUnitType = pck.HandelingUnitType) Then
                    For Each relpcklsts In Me
                        If relpcklsts.CanPlace(pck, parallelrelstrat) Then
                            relpcklsts.Add(pck)
                            placed = True
                            Exit For
                        End If
                    Next
                    If Not placed Then
                        Dim relcol As New ReleasePickListCollection(pck, parallelrelstrat, sTaskSubType)
                        Me.AddBasket(relcol)
                    End If
                    Exit For
                ElseIf (parallelrelstrat.HandelingUnitType Is Nothing Or parallelrelstrat.HandelingUnitType = String.Empty) Then
                    Dim relcol As New ReleasePickListCollection(pck, parallelrelstrat, sTaskSubType)
                    Me.AddBasket(relcol)
                End If
            Next
        Next
    End Function

    Private Function GetTaskSubType(ByVal oPicklist As Picklist) As String
        Dim oRelStratDetail As ReleaseStrategyDetail = oPicklist.getReleaseStrategy()
        If Not oRelStratDetail Is Nothing Then
            Return oRelStratDetail.TaskSubType
        End If
        Return String.Empty
    End Function

#End Region

End Class
