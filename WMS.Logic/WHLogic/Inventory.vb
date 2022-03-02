Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class Inventory
    Public Shared Function GetSkuInventoryForReceiptDate(ByVal consignee As String, ByVal sku As String, ByVal FromReceiptDate As DateTime, ByVal ToReceiptDate As DateTime, ByVal computeDate As DateTime) As ArrayList
        Dim invlist As New ArrayList
        Dim totalUnits As Double
        Dim chkdate As DateTime = DateTime.Now.Date
        computeDate = computeDate.Date.AddDays(1).AddSeconds(-1)
        FromReceiptDate = FromReceiptDate.Date
        ToReceiptDate = ToReceiptDate.Date.AddDays(1).AddSeconds(-1)
        Dim sql As String = String.Format("Select sum(units) as TotalUnits from invload where consignee = '{0}' and sku = '{1}' and receivedate >= '{2}' and receivedate <= '{3}'", consignee, sku, Made4Net.Shared.Util.DateTimeToDbString(FromReceiptDate), Made4Net.Shared.Util.DateTimeToDbString(ToReceiptDate))
        totalUnits = DataInterface.ExecuteScalar(sql)
        System.Web.HttpContext.Current.Response.Write("Current Units: " & totalUnits & "<BR>")
        Dim dt As New DataTable
        Dim dr As DataRow
        totalUnits = totalUnits + DataInterface.ExecuteScalar(String.Format("select isnull(sum(inventorytrans.qty),0) as QTY from pickdetail join loads on pickdetail.fromload = loads.loadid join inventorytrans on pickdetail.toload = inventorytrans.loadid " & _
                "where loads.receivedate >= '{0}' and loads.receivedate <= '{1}' and loads.consignee = '{2}' and loads.sku = '{3}' and trandate >= '{4}'", Made4Net.Shared.Util.DateTimeToDbString(FromReceiptDate), Made4Net.Shared.Util.DateTimeToDbString(ToReceiptDate), consignee, _
                sku, Made4Net.Shared.Util.DateTimeToDbString(computeDate)))
        sql = String.Format("select invtrntype,isnull(sum(qty),0) as qty from inventorytrans join loads on inventorytrans.loadid = loads.loadid " & _
            "where loads.receivedate >= '{0}' and loads.receivedate <= '{1}' and loads.consignee = '{2}' and loads.sku = '{3}' and trandate >= '{4}' and invtrntype <> '{5}' group by invtrntype", Made4Net.Shared.Util.DateTimeToDbString(FromReceiptDate), Made4Net.Shared.Util.DateTimeToDbString(ToReceiptDate), consignee, _
            sku, Made4Net.Shared.Util.DateTimeToDbString(computeDate), WMS.Lib.Actions.Audit.SHIPLOAD)
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            Select Case Convert.ToString(dr("INVTRNTYPE")).ToUpper
                Case WMS.Lib.Actions.Audit.SUBQTY
                    totalUnits = totalUnits + dr("QTY")
                Case WMS.Lib.Actions.Audit.ADDQTY
                    totalUnits = totalUnits - dr("QTY")
                Case WMS.Lib.Actions.Audit.CREATELOAD
                    totalUnits = totalUnits - dr("QTY")
            End Select
        Next
        Return Nothing
    End Function

    Public Shared Sub FillSkuInventoryForDate(ByVal consignee As String, ByVal computeDate As DateTime, ByVal inv As DataTable)
        Dim dt As New DataTable
        Dim dr, newrec As DataRow
        Dim sql As String = String.Format("Select * from sku where consignee = '{0}'", consignee)
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            FillSkuInventoryForDate(consignee, dr("SKU"), computeDate, inv)
        Next
    End Sub

    Private Shared Sub FillSkuInventoryForDate(ByVal consignee As String, ByVal sku As String, ByVal computeDate As DateTime, ByVal inv As DataTable)
        Dim invlist As New ArrayList
        Dim InventoryTrans As New DataTable
        Dim sql As String = String.Format("Select * from InventoryTrans Where Consignee = '{0}' and TranDate >= '{1}'", consignee, Made4Net.Shared.Util.DateTimeToDbString(computeDate))
        DataInterface.FillDataset(sql, InventoryTrans)
        Dim totalUnits As Double
        Dim chkdate As DateTime = computeDate.Date.AddDays(1)
        'Dim sql As String = String.Format("Select isnull(sum(units),0) as TotalUnits from invload where consignee = '{0}' and sku = '{1}'", consignee, sku)
        totalUnits = DataInterface.ExecuteScalar(sql)
        Dim dr As DataRow
        Dim newdr As DataRow
        newdr = inv.NewRow()
        newdr("CONSIGNEE") = consignee
        newdr("SKU") = sku
        newdr("TRANDATE") = DateTime.Now.Date()
        newdr("UNITS") = totalUnits
        inv.Rows.Add(newdr)
        Dim currdate As DateTime = DateTime.Now.Date
        While currdate > computeDate
            Dim dt As New DataTable
            sql = String.Format("Select invtrntype,sum(qty) as qty from inventorytrans where consignee = '{0}' and sku = '{1}' and trandate >= '{2}' and trandate <= '{3}' group by invtrntype", consignee, sku, Made4Net.Shared.Util.DateTimeToDbString(currdate), Made4Net.Shared.Util.DateTimeToDbString(currdate.Date.AddDays(1).AddSeconds(-1)))
            DataInterface.FillDataset(sql, dt)
            newdr = inv.NewRow()
            newdr("CONSIGNEE") = consignee
            newdr("SKU") = sku
            newdr("TRANDATE") = currdate.Date()
            For Each dr In dt.Rows
                Select Case Convert.ToString(dr("INVTRNTYPE"))
                    Case WMS.Lib.Actions.Audit.SUBQTY
                        totalUnits = totalUnits + dr("QTY")
                    Case WMS.Lib.Actions.Audit.ADDQTY
                        totalUnits = totalUnits - dr("QTY")
                    Case WMS.Lib.Actions.Audit.CREATELOAD
                        totalUnits = totalUnits - dr("QTY")
                End Select
            Next
            newdr("UNITS") = totalUnits
            inv.Rows.Add(newdr)
            currdate = currdate.AddDays(-1)
        End While
    End Sub

    Public Shared Function GetSkuInventoryForDate(ByVal consignee As String, ByVal sku As String, ByVal computeDate As DateTime) As Double
        Dim invlist As New ArrayList
        Dim totalUnits As Double
        Dim chkdate As DateTime = computeDate.Date.AddDays(1)
        Dim sql As String = String.Format("Select isnull(sum(units),0) as TotalUnits from invload where consignee = '{0}' and sku = '{1}'", consignee, sku)
        totalUnits = DataInterface.ExecuteScalar(sql)
        Dim dt As New DataTable
        Dim dr As DataRow
        sql = String.Format("Select invtrntype,isnull(sum(qty),0) as qty from inventorytrans where consignee = '{0}' and sku = '{1}' and trandate >= '{2}' and trandate <= '{3}' group by invtrntype", consignee, sku, Made4Net.Shared.Util.DateTimeToDbString(chkdate), Made4Net.Shared.Util.DateTimeToDbString(DateTime.Now))
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            Select Case Convert.ToString(dr("INVTRNTYPE"))
                Case WMS.Lib.Actions.Audit.CREATELOAD
                    totalUnits = totalUnits - dr("QTY")
                Case WMS.Lib.Actions.Audit.SHIPLOAD
                    totalUnits = totalUnits + dr("QTY")
                Case WMS.Lib.Actions.Audit.SUBQTY
                    totalUnits = totalUnits + dr("QTY")
                Case WMS.Lib.Actions.Audit.ADDQTY
                    totalUnits = totalUnits - dr("QTY")
            End Select
        Next
        Return totalUnits
    End Function

    Public Shared Function CalculateWeight(ByVal paramConsignee As String, ByVal paramSKU As String, ByVal paramUnits As Double, ByVal paramUOM As String, Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Double
        Dim loadsku As SKU = Nothing
        Dim _leftQty As Double = paramUnits
        Dim _currQty As Double = paramUnits
        Dim _calcWeight As Double = 0
        Dim _curUom As SKU.SKUUOM = Nothing
        Dim _nextUom As SKU.SKUUOM = Nothing
        Try
            If oSku Is Nothing Then
                loadsku = New SKU(paramConsignee, paramSKU)
            Else
                loadsku = oSku
            End If
            _curUom = loadsku.UNITSOFMEASURE.UOM(loadsku.LOWESTUOM)
            _nextUom = _curUom
            While _leftQty > 0
                _nextUom = loadsku.UNITSOFMEASURE.UOM(loadsku.UNITSOFMEASURE.UOM(_curUom.UOM).LOWERUOM)
                If _nextUom Is Nothing OrElse _curUom.UOM = _nextUom.UOM Then
                    _calcWeight += _leftQty * _curUom.GROSSWEIGHT
                    _leftQty = 0
                Else
                    _currQty = _leftQty / _nextUom.UNITSPERMEASURE
                    _calcWeight += _currQty * _nextUom.GROSSWEIGHT
                    _leftQty = _leftQty Mod _nextUom.UNITSPERMEASURE
                    _curUom = _nextUom
                End If
            End While
            Return _calcWeight
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Shared Function ConvertUom(ByVal paramConsignee As String, ByVal paramSku As String, ByVal paramUnits As Double, ByVal paramFromUom As String, ByVal paramToUom As String) As Double
        Dim loadsku As SKU = Nothing
        Dim _calcUnits As Double = 1
        Dim _curUom As SKU.SKUUOM = Nothing
        Dim _nextUom As SKU.SKUUOM = Nothing
        If paramFromUom Is Nothing Or paramFromUom = "" Then
            Try
                Dim sql As String = String.Format("Select top 1 uom from skuuom where consignee = '{0}' and sku = '{1}' and (loweruom is null or loweruom = '')", paramConsignee, paramSku)
                paramFromUom = DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
                Return 0
            End Try
        End If
        Try
            loadsku = New SKU(paramConsignee, paramSku)
            _curUom = loadsku.UNITSOFMEASURE.UOM(paramToUom)
            _nextUom = _curUom

            While _nextUom.UOM <> paramFromUom
                _nextUom = loadsku.UNITSOFMEASURE.UOM(loadsku.UNITSOFMEASURE.UOM(_curUom.UOM).LOWERUOM)
                If _nextUom Is Nothing OrElse _curUom.UOM = _nextUom.UOM Then
                    Exit While
                Else
                    _calcUnits = _calcUnits * _curUom.UNITSPERMEASURE
                    _curUom = _nextUom
                End If
            End While

            Return System.Math.Ceiling(paramUnits / _calcUnits)


        Catch ex As Exception
            Return 1
        End Try
    End Function


    Public Shared Function CalculateVolume(ByVal paramConsignee As String, ByVal paramSKU As String, ByVal paramUnits As Double, ByVal paramUOM As String, Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Double
        Dim loadsku As SKU = Nothing
        Dim _leftQty As Double = paramUnits
        Dim _currQty As Double = paramUnits
        Dim _calcVolume As Double = 0
        Dim _curUom As SKU.SKUUOM = Nothing
        Dim _nextUom As SKU.SKUUOM = Nothing

        Try
            If oSku Is Nothing Then
                loadsku = New SKU(paramConsignee, paramSKU)
            Else
                loadsku = oSku
            End If
            _curUom = loadsku.UNITSOFMEASURE.UOM(loadsku.LOWESTUOM) 'paramUOM)
            _nextUom = _curUom
            While _leftQty > 0
                _nextUom = loadsku.UNITSOFMEASURE.UOM(loadsku.UNITSOFMEASURE.UOM(_curUom.UOM).LOWERUOM)
                If _nextUom Is Nothing OrElse _curUom.UOM = _nextUom.UOM Then
                    _calcVolume += _leftQty * _curUom.VOLUME
                    _leftQty = 0
                Else
                    _currQty = _leftQty / _nextUom.UNITSPERMEASURE
                    _calcVolume += _currQty * _nextUom.VOLUME
                    _leftQty = _leftQty Mod _nextUom.UNITSPERMEASURE
                    _curUom = _nextUom
                End If
            End While
            Return _calcVolume
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Shared Function CalculateVolumeForGivenUOM(ByVal paramConsignee As String, ByVal paramSKU As String, ByVal paramUnits As Double, ByVal paramUOM As String, Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Double
        Dim loadsku As SKU = Nothing
        Dim _leftQty As Double = paramUnits
        Dim _calcVolume As Double = 0
        Dim _curUom As SKU.SKUUOM = Nothing
        Try
            If oSku Is Nothing Then
                loadsku = New SKU(paramConsignee, paramSKU)
            Else
                loadsku = oSku
            End If
            _curUom = loadsku.UNITSOFMEASURE.UOM(paramUOM)
            _calcVolume = _leftQty * _curUom.VOLUME
            _leftQty = 0

            Return _calcVolume
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Shared Function CalculateAndUpdateRunningAvgWgt(ByVal rcptID As String)
        Dim sSqlFlag As String = "SELECT PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME = 'CalcRuningAvgWt'"
        Dim paramValue As String
        Dim avgWeight As Decimal = 0
        Dim totalInventory As Integer = 0
        Dim currentInventory As Integer = 0
        Dim AvgRunningVal As Decimal = 0.0


        paramValue = Made4Net.DataAccess.DataInterface.ExecuteScalar(sSqlFlag)
        If (paramValue = "1") Then

            Dim strClassUnits As String = "select rcpt.SKU,rcpt.RECEIVEDWEIGHT,rcpt.QTYRECEIVED,sum(units) as UNITS,attb.WGT,sku.CLASSNAME from loads ld " & _
                    " inner join SKUATTRIBUTE attb on attb.SKU = ld.SKU " & _
                    " inner join SKU sku on sku.sku = ld.sku " & _
                    " inner join (select sum(receivedweight) as RECEIVEDWEIGHT,sum(QTYRECEIVED) as QTYRECEIVED, SKU from receiptdetail " & _
                    " where RECEIPT = '" & rcptID & "' group by sku) as rcpt " & _
                    " on rcpt.SKU = ld.SKU where ld.STATUS != '' and ld.LOCATION !='' group by ld.sku, rcpt.RECEIVEDWEIGHT,rcpt.SKU, attb.WGT, sku.CLASSNAME, rcpt.QTYRECEIVED"
            Dim dtSkuDetails As New DataTable
            Dim drSKU As DataRow
            Made4Net.DataAccess.DataInterface.FillDataset(strClassUnits, dtSkuDetails)
            For Each drSKU In dtSkuDetails.Rows
                If Not IsDBNull(drSKU("CLASSNAME")) And drSKU("CLASSNAME") = "WGT" Or drSKU("CLASSNAME") = "EXPW" Or drSKU("CLASSNAME") = "MANW" Then
                    avgWeight = drSKU("WGT")
                    totalInventory = drSKU("UNITS")
                    currentInventory = totalInventory - drSKU("QTYRECEIVED")
                    'AvgRunningVal = ((currentInventory * avgWeight) + Decimal.Parse(drSKU("RECEIVEDWEIGHT"))) / totalInventory
                    AvgRunningVal = CalculateRunningAverageWeight(currentInventory, avgWeight, totalInventory, Decimal.Parse(drSKU("RECEIVEDWEIGHT")))
                    Dim sqlUpdate As String = String.Format("Update SKUATTRIBUTE set WGT = " & Decimal.Parse(AvgRunningVal) & " where SKU='" & drSKU("SKU") & "'")
                    DataInterface.RunSQL(sqlUpdate)
                End If
            Next
        End If
    End Function



    Public Shared Function CalculateRunningAverageWeight(ByVal currentReceivedInventory As Decimal, ByVal averageWeight As Decimal, ByVal totalAvailableInventory As Decimal, ByVal receivedWeight As Decimal)
        Dim AvgRunningWeightVal As Decimal = 0.0
        AvgRunningWeightVal = ((currentReceivedInventory * averageWeight) + receivedWeight) / totalAvailableInventory
        Return AvgRunningWeightVal

    End Function
End Class
