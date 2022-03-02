Imports System.Collections.Generic
Imports Made4Net.DataAccess

<System.ComponentModel.DesignerCategory("Code")>
Public Class VAllocationOnHandDataTable
    Inherits IndexedDataTable

    Protected consigneeOrderIdSkuIndex As Dictionary(Of String, List(Of DataRow)) = New Dictionary(Of String, List(Of DataRow))
    Protected consigneeSkuIndex As Dictionary(Of String, List(Of DataRow)) = New Dictionary(Of String, List(Of DataRow))
    Protected onHandDataColumn As DataColumn
    Protected consigneeColumn As DataColumn
    Protected skuColumn As DataColumn
    Protected orderIdColumn As DataColumn

    Public Function GetRowsForConsigneeSku(consignee As Object, sku As Object, sql As Object) As DataRow()
        Dim key As String = CreateKey(consignee, sku)
        If consigneeSkuIndex.ContainsKey(key) Then
            Return SelectFromRows(consigneeSkuIndex(key).ToArray(), sql)
        End If
        Return New DataRow() {}
    End Function

    Public Overrides Sub BuildRowIndex(dr As DataRow)
        Dim consignee_orderid_sku_key As String = CreateKey(dr(consigneeColumn), dr(orderIdColumn), dr(skuColumn))
        Dim consignee_sku_key As String = CreateKey(dr(consigneeColumn), dr(skuColumn))

        If Not consigneeOrderIdSkuIndex.ContainsKey(consignee_orderid_sku_key) Then
            consigneeOrderIdSkuIndex.Add(consignee_orderid_sku_key, New List(Of DataRow))
        End If
        consigneeOrderIdSkuIndex(consignee_orderid_sku_key).Add(dr)

        If Not consigneeSkuIndex.ContainsKey(consignee_sku_key) Then
            consigneeSkuIndex.Add(consignee_sku_key, New List(Of DataRow))
        End If
        consigneeSkuIndex(consignee_sku_key).Add(dr)
    End Sub

    Public Overrides Sub Initialize()
        consigneeColumn = Columns("CONSIGNEE")
        skuColumn = Columns("SKU")
        onHandDataColumn = Columns("ONHANDQTY")
        orderIdColumn = Columns("ORDERID")
    End Sub

    Public Function GetOnHandQty(consignee As String, orderID As String, sku As String) As Decimal
        Dim consignee_orderid_sku_key As String = CreateKey(consignee, orderID, sku)

        If consigneeOrderIdSkuIndex.ContainsKey(consignee_orderid_sku_key) Then
            Return consigneeOrderIdSkuIndex(consignee_orderid_sku_key).First()(onHandDataColumn)
        End If
        Return 0
    End Function
End Class