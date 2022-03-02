Imports System.Collections.Generic
Imports Made4Net.DataAccess

<System.ComponentModel.DesignerCategory("Code")>
Public Class VPickLocDataTable
    Inherits IndexedDataTable

    Protected locationIndex As Dictionary(Of String, List(Of DataRow)) = New Dictionary(Of String, List(Of DataRow))
    Protected consigneeSkuIndex As Dictionary(Of String, List(Of DataRow)) = New Dictionary(Of String, List(Of DataRow))
    Protected locationColumn As DataColumn
    Protected consigneeColumn As DataColumn
    Protected skuColumn As DataColumn

    Public Overrides Sub Initialize()
        locationColumn = Columns("LOCATION")
        consigneeColumn = Columns("CONSIGNEE")
        skuColumn = Columns("SKU")
    End Sub
    Public Overrides Sub BuildRowIndex(dr As DataRow)

        Dim location_key As String = dr(locationColumn)
        Dim consignee_sku_key As String = CreateKey(dr(consigneeColumn), dr(skuColumn))

        If Not locationIndex.ContainsKey(location_key) Then
            locationIndex.Add(location_key, New List(Of DataRow))
        End If
        locationIndex(location_key).Add(dr)

        If Not consigneeSkuIndex.ContainsKey(consignee_sku_key) Then
            consigneeSkuIndex.Add(consignee_sku_key, New List(Of DataRow))
        End If
        consigneeSkuIndex(consignee_sku_key).Add(dr)
    End Sub

    Public Function GetRowsForLocation(location As String) As DataRow()
        Dim rval As DataRow() = New DataRow() {}
        If locationIndex.ContainsKey(location) Then
            rval = locationIndex(location).ToArray()
        End If
        Return rval
    End Function

    Public Function GetRowsForConsigneeSku(consignee As String, sku As String, sql As String) As DataRow()
        Dim key As String = CreateKey(consignee, sku)
        If consigneeSkuIndex.ContainsKey(key) Then
            Return SelectFromRows(consigneeSkuIndex(key).ToArray(), sql)
        End If
        Return New DataRow() {}
    End Function

End Class