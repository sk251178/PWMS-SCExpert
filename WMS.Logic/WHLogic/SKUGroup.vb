Public Class SKUGroup

    Public Sub New(ByVal sender As Object, ByVal commandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Dim msg As String
        Select Case commandName.ToLower
            Case "delete"
                dr = ds.Tables(0).Rows(0)
                Delete(dr("SKUGroup"))
            Case "new"
                dr = ds.Tables(0).Rows(0)
                Insert(dr("SKUGroup"), dr("SKUGroupDesc"), dr("Tobacco"))
        End Select
    End Sub

    Public Shared Sub Delete(skuGroup As String)
        If String.IsNullOrEmpty(skuGroup) Then
            Return
        End If
        If Made4Net.DataAccess.DataInterface.ExecuteScalar($"SELECT COUNT(1) FROM SKU WHERE SKUGroup = '{skuGroup}'") <> "0" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CANNOTTDELETESKUGROUP", "Cannot delete SKU Group. SKU Group entries are found in SKU table.")
        Else
            Made4Net.DataAccess.DataInterface.RunSQL($"DELETE FROM SKUGroup WHERE SKUGroup='{skuGroup}'")
        End If
    End Sub

    Public Shared Sub Insert(skuGroup As String, SKUGroupDescription As String, Tobacco As Boolean)
        If String.IsNullOrEmpty(skuGroup) Or String.IsNullOrEmpty(SKUGroupDescription) Or String.IsNullOrEmpty(Tobacco) Then
            Return
        End If

        If Exists(skuGroup) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CANNOTCREATESKUGROUP", "Cannot create SKU Group. SKU Group already exists.")
        Else
            Made4Net.DataAccess.DataInterface.RunSQL($"Insert into SKUGroup (SKUGroup, SKUGroupDesc,Tobacco) " +
                                                     $"values ('{skuGroup}','{SKUGroupDescription}','{Tobacco}')")
        End If
    End Sub

    Public Shared Function Exists(skuGroup As String) As Boolean
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar($"SELECT COUNT(1) FROM SKUGroup WHERE SKUGroup='{skuGroup}'") <> "0"
    End Function
End Class
