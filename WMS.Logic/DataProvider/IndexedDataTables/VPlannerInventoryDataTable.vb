Imports System.Collections.Generic
Imports Made4Net.DataAccess

<System.ComponentModel.DesignerCategory("Code")>
Public Class VPlannerInventoryDataTable
    Inherits IndexedDataTable

    Protected consigneeSkuStatusIndex As Dictionary(Of String, List(Of DataRow)) = New Dictionary(Of String, List(Of DataRow))

    Protected statusColumn As DataColumn
    Protected consigneeColumn As DataColumn
    Protected skuColumn As DataColumn

    Public Overrides Sub Initialize()
        statusColumn = Columns("STATUS")
        consigneeColumn = Columns("CONSIGNEE")
        skuColumn = Columns("SKU")
    End Sub

    Public Overrides Sub BuildRowIndex(dr As DataRow)

        Dim key As String = CreateKey(dr(consigneeColumn), dr(skuColumn), dr(statusColumn))

        If Not consigneeSkuStatusIndex.ContainsKey(key) Then
            consigneeSkuStatusIndex.Add(key, New List(Of DataRow))
        End If
        consigneeSkuStatusIndex(key).Add(dr)

    End Sub

    Public Function GetRowsForConsigneeSkuStatus(consignee As String, sku As String, status As String, sql As String) As DataRow()
        Dim rval As DataRow() = New DataRow() {}
        Dim key As String = CreateKey(consignee, sku, status)

        If consigneeSkuStatusIndex.ContainsKey(key) Then
            rval = SelectFromRows(consigneeSkuStatusIndex(key).ToArray(), sql)
        End If
        Return rval
    End Function

End Class