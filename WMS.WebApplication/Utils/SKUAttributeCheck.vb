Public Class SKUAttributeCheck

    Public Shared Function CheckAttribute(ByVal pConsignee As String, ByVal pSKU As String, ByVal pAttributeName As String) As String
        Try
            Dim sql As String
            sql = String.Format("select count(1) from SKUCLSATT sa inner join sku on sa.CLASSNAME = SKU.CLASSNAME where " & _
            "CONSIGNEE ={0} and SKU={1} and ATTRIBUTENAME={2}", Made4Net.Shared.FormatField(pConsignee), Made4Net.Shared.FormatField(pSKU), _
            Made4Net.Shared.FormatField(pAttributeName))
            Dim numOfRows As Integer = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If numOfRows = 0 Then
                Return "-1"
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function
End Class


