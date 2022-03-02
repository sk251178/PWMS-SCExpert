Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class Merge

    Public Shared Function Put(ByVal SourceLoad As Load, ByVal pLocation As String, ByVal pWarehousearea As String) As Load
        Dim loc As New Location(pLocation, pWarehousearea)
        Dim objLoadReturn As Load = Nothing
        If loc.LOOSEID Then
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(String.Format("Select loadid from invload where location='{0}' and warehousearea='{4}' and loadid <> '{1}' and sku='{2}' and consignee = '{3}' and isnull(ACTIVITYSTATUS,'') = ''", pLocation, SourceLoad.LOADID, SourceLoad.SKU, SourceLoad.CONSIGNEE, pWarehousearea), dt)
            For Each dr In dt.Rows
                Dim ld As New Load(Convert.ToString(dr("LOADID")))
                If CanMerge(SourceLoad, ld) Then
                    ld.Merge(SourceLoad, True)
                    objLoadReturn = ld
                    Exit For

                End If
            Next
        End If
        Return objLoadReturn
    End Function

    Public Shared Function CanMerge(ByVal SourceLoad As Load, ByVal DestLoad As Load) As Boolean
        'If SourceLoad.CONSIGNEE.Equals(DestLoad.CONSIGNEE, StringComparison.OrdinalIgnoreCase) And SourceLoad.SKU.Equals(DestLoad.SKU, StringComparison.OrdinalIgnoreCase) _
        '    And SourceLoad.STATUS.Equals(DestLoad.STATUS, StringComparison.OrdinalIgnoreCase) And SourceLoad.ACTIVITYSTATUS.equals(WMS.Lib.Statuses.ActivityStatus.NONE,StringComparison.OrdinalIgnoreCase) Then
        If SourceLoad.CONSIGNEE.Equals(DestLoad.CONSIGNEE, StringComparison.OrdinalIgnoreCase) And _
            SourceLoad.SKU.Equals(DestLoad.SKU, StringComparison.OrdinalIgnoreCase) And _
            SourceLoad.STATUS.Equals(DestLoad.STATUS, StringComparison.OrdinalIgnoreCase) And _
            String.IsNullOrEmpty(SourceLoad.ACTIVITYSTATUS) Then

            'Check for merge expressions
            Dim oSku As New WMS.Logic.SKU(SourceLoad.CONSIGNEE, SourceLoad.SKU)
            If Not oSku.SKUClass Is Nothing Then
                Dim expression As String = oSku.SKUClass.MergeValidationExpression
                If expression <> String.Empty Then
                    Dim eval As New Made4Net.Shared.Evaluation.ExpressionEvaluator()
                    Dim objects As New Made4Net.DataAccess.Collections.GenericCollection
                    objects.Add("currentload", New WMS.Logic.Load(DestLoad.LOADID))
                    objects.Add("incomingload", New WMS.Logic.Load(SourceLoad.LOADID))
                    eval.ObjectsCollection = objects
                    Try
                        Return Convert.ToBoolean(Convert.ToInt32(eval.Evaluate(expression)))
                    Catch ex As Exception
                        Return False
                    End Try
                End If
            End If
            'if there isnt any sku class
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function CanMergeLimbo(ByVal SourceLoad As Load, ByVal DestLoad As Load) As Boolean
        If SourceLoad.CONSIGNEE = DestLoad.CONSIGNEE And SourceLoad.SKU = DestLoad.SKU _
            And ((SourceLoad.STATUS = WMS.Lib.Statuses.LoadStatus.LIMBO AndAlso DestLoad.STATUS = WMS.Lib.Statuses.LoadStatus.NONE) _
            OrElse (DestLoad.STATUS = WMS.Lib.Statuses.LoadStatus.LIMBO AndAlso SourceLoad.STATUS = WMS.Lib.Statuses.LoadStatus.NONE) _
            OrElse (DestLoad.STATUS = WMS.Lib.Statuses.LoadStatus.LIMBO AndAlso SourceLoad.STATUS = WMS.Lib.Statuses.LoadStatus.LIMBO)) _
            And DestLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.NONE _
            And SourceLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.NONE Then

            'Check for merge expressions
            Dim oSku As New WMS.Logic.SKU(SourceLoad.CONSIGNEE, SourceLoad.SKU)
            If Not oSku.SKUClass Is Nothing Then
                Dim expression As String = oSku.SKUClass.MergeValidationExpression
                If expression <> String.Empty Then
                    Dim eval As New Made4Net.Shared.Evaluation.ExpressionEvaluator()
                    Dim objects As New Made4Net.DataAccess.Collections.GenericCollection
                    objects.Add("currentload", New WMS.Logic.Load(DestLoad.LOADID))
                    objects.Add("incomingload", New WMS.Logic.Load(SourceLoad.LOADID))
                    eval.ObjectsCollection = objects
                    Try
                        Return Convert.ToBoolean(Convert.ToInt32(eval.Evaluate(expression)))
                    Catch ex As Exception
                        Return False
                    End Try
                End If
            End If
            'if there isnt any sku class
            Return True
        Else
            Return False
        End If
    End Function

End Class
