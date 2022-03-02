Imports System.Collections.Generic
Imports Made4Net.DataAccess

<System.ComponentModel.DesignerCategory("Code")>
Public Class PlanStrategyDetailDataTable
    Inherits IndexedDataTable

    Protected strategyPriorityPickTypeIndex As Dictionary(Of String, List(Of DataRow)) = New Dictionary(Of String, List(Of DataRow))
    Protected strategyIdColumn As DataColumn
    Protected priorityColumn As DataColumn
    Protected pickTypeColumn As DataColumn

    Public Function GetRowsForStrategyIdPriorityPickType(strategyId As Object, priority As Object, pickType As Object) As DataRow()
        Dim key As String = CreateKey(strategyId, priority, pickType)

        If strategyPriorityPickTypeIndex.ContainsKey(key) Then
            Return strategyPriorityPickTypeIndex(key).ToArray()
        End If
        Return New DataRow() {}
    End Function
    Public Overrides Sub BuildRowIndex(dr As DataRow)
        Dim key As String = CreateKey(dr(strategyIdColumn), dr(priorityColumn), dr(pickTypeColumn))

        If Not strategyPriorityPickTypeIndex.ContainsKey(key) Then
            strategyPriorityPickTypeIndex.Add(key, New List(Of DataRow))
        End If
        strategyPriorityPickTypeIndex(key).Add(dr)
    End Sub

    Public Overrides Sub Initialize()
        strategyIdColumn = Columns("STRATEGYID")
        priorityColumn = Columns("PRIORITY")
        pickTypeColumn = Columns("PICKTYPE")
    End Sub
End Class