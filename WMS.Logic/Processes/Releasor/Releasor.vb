<CLSCompliant(False)> Public Class Releasor

#Region "Release Wave"

    Public Function Release(ByVal WaveId As String, Optional ByVal pUser As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing)
        Dim wv As Wave
        Try
            wv = New Wave(WaveId)
            Release(wv, pUser, oLogger)
        Catch ex As Exception
            wv.ReleaseComplete()
        Finally
            wv = Nothing
        End Try
    End Function

    Public Function Release(ByVal Wave As Wave, Optional ByVal pUser As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing)
        If pUser Is Nothing Then pUser = WMS.Lib.USERS.SYSTEMUSER
        Dim pls As New PicklistList(Wave, oLogger)
        Dim pcklst As Picklist
        Dim strat As New PlanStrategies
        Dim parpls As New ParallelPicksBasket(strat)
        Dim strategy As PlanStrategy
        Dim releasestrat As ReleaseStrategyDetail

        For Each pcklst In pls
            If pcklst.PickType = WMS.Lib.PICKTYPE.PARALLELPICK Then
                parpls.Add(pcklst)
            End If
            ' First release and after it change picklist status
            'pcklst.Release()
            'pcklst.ReleaseComplete()
            strategy = strat(pcklst.StrategyId)
            For Each releasestrat In strategy.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    releasestrat.Release(pcklst, oLogger)
                    Exit For
                End If
            Next
        Next

        If parpls.PickLists.Count > 0 Then
            parpls.Release()
        End If

        Wave.ReleaseComplete()
    End Function

#End Region

#Region "Release Picklist"

    Public Function ReleasePickList(ByVal pPickList As String, Optional ByVal pUser As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing)
        Dim oPickList As New Picklist(pPickList)
        If oPickList.Status = WMS.Lib.Statuses.Picklist.PLANNED Or oPickList.Status = WMS.Lib.Statuses.Picklist.RELEASING Then
            ReleasePickList(oPickList, pUser)
        End If
    End Function

    Private Function ReleasePickList(ByVal oPickList As Picklist, Optional ByVal pUser As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing)
        If pUser Is Nothing Then pUser = WMS.Lib.USERS.SYSTEMUSER
        Dim pls As New PicklistList(oPickList, oLogger)
        Dim pcklst As Picklist
        Dim strat As New PlanStrategies
        Dim parpls As New ParallelPicksBasket(strat)
        Dim strategy As PlanStrategy
        Dim releasestrat As ReleaseStrategyDetail

        For Each pcklst In pls
            If pcklst.PickType = WMS.Lib.PICKTYPE.PARALLELPICK Then
                parpls.Add(pcklst)
            End If
            ' First release and after it change picklist status
            'pcklst.Release()
            'pcklst.ReleaseComplete()
            strategy = strat(pcklst.StrategyId)
            For Each releasestrat In strategy.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    releasestrat.Release(pcklst, oLogger)
                    Exit For
                End If
            Next
        Next
        If parpls.PickLists.Count > 0 Then
            parpls.Release()
        End If
    End Function

#End Region

End Class
