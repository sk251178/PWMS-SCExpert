Imports System.Data
Imports Made4Net.DataAccess

Public Class StorageBillingInventory
    Implements ICloneable

#Region "Variables"

    Protected _loadid As String
    Protected _uom As String
    Protected _consignee As String
    Protected _sku As String
    Protected _units As Decimal
    Protected _unitsperuom As Decimal
    Protected _uomweight As Decimal
    Protected _uomvolume As Decimal
    Protected _unitprice As Decimal
    Protected _transdate As DateTime

#End Region

#Region "Properties"

    Public Property LoadId() As String
        Get
            Return _loadid
        End Get
        Set(ByVal Value As String)
            _loadid = Value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property Sku() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property Units() As Decimal
        Get
            Return _units
        End Get
        Set(ByVal Value As Decimal)
            _units = Value
        End Set
    End Property

    Public Property UnitOfMeasure() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property

    Public Property UnitsPerMeasure() As Decimal
        Get
            Return _unitsperuom
        End Get
        Set(ByVal Value As Decimal)
            _unitsperuom = Value
        End Set
    End Property

    Public Property UnitPrice() As Decimal
        Get
            Return _unitprice
        End Get
        Set(ByVal Value As Decimal)
            _unitprice = Value
        End Set
    End Property

    Public Property UomVolume() As Decimal
        Get
            Return _uomvolume
        End Get
        Set(ByVal Value As Decimal)
            _uomvolume = Value
        End Set
    End Property

    Public Property UomWeight() As Decimal
        Get
            Return _uomweight
        End Get
        Set(ByVal Value As Decimal)
            _uomweight = Value
        End Set
    End Property

    Public Property TransactionDate() As DateTime
        Get
            Return _transdate
        End Get
        Set(ByVal Value As DateTime)
            _transdate = Value
        End Set
    End Property

    Public ReadOnly Property getKey() As String
        Get
            'Key property changed to suport number of loads in storage basis
            Return _loadid & "#"c & _consignee & "#"c & _sku & "#"
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New()

    End Sub
    Public Sub New(ByVal dr As DataRow)
        _loadid = CType(dr("loadid"), String)
        _uom = CType(dr("uom"), String)
        _consignee = CType(dr("consignee"), String)
        _sku = CType(dr("sku"), String)
        _units = CType(dr("units"), Decimal)
        _unitprice = CType(dr("unitprice"), Decimal)
        _uomweight = CType(dr("grossweight"), Decimal)
        _uomvolume = CType(dr("volume"), Decimal)
        '_transdate = dr("trandate")
        Dim sk As New WMS.Logic.SKU(_consignee, _sku)
        _unitsperuom = sk.ConvertToUnits(_uom)
        sk = Nothing
    End Sub
#End Region

#Region "Methods"

    Public Function getValue() As Decimal
        Return Math.Abs(_units) * _unitprice
    End Function

    Public Function getVolume() As Decimal
        'Return (Math.Ceiling(_units / _unitsperuom)) * _uomvolume
        Return (Math.Abs(_units) / _unitsperuom) * _uomvolume
    End Function

    Public Function getWeight() As Decimal
        'Return (Math.Ceiling(_units / _unitsperuom)) * _uomweight
        ' TBD : Her we nee to check if we can ovveride the weight of the calculated bill base
        Return (Math.Abs(_units) / _unitsperuom) * _uomweight
    End Function

    Public Function Clone() As Object Implements System.ICloneable.Clone
        Dim cln As New StorageBillingInventory
        cln.LoadId = Me.LoadId
        cln.Consignee = Me.Consignee
        cln.Sku = Me.Sku
        cln.UnitOfMeasure = Me.UnitOfMeasure
        cln.UnitPrice = Me.UnitPrice
        cln.Units = Me.Units
        cln.UnitsPerMeasure = Me.UnitsPerMeasure
        cln.UomVolume = Me.UomVolume
        cln.UomWeight = Me.UomWeight
        cln.TransactionDate = Me.TransactionDate
        Return cln
    End Function

#End Region

End Class

Public Class StorageBillingCollection
    Inherits System.Collections.Hashtable

#Region "Constructor"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Properties"
    Default Public Overloads Property Item(ByVal key As String) As StorageBillingInventory
        Get
            Return CType(MyBase.Item(key), StorageBillingInventory)
        End Get
        Set(ByVal Value As StorageBillingInventory)
            MyBase.Item(key) = Value
        End Set
    End Property
#End Region

#Region "Overrides"

    Public Overloads Sub Add(ByVal sbi As StorageBillingInventory)
        Dim tempsbi As StorageBillingInventory = Me(sbi.getKey)
        If tempsbi Is Nothing Then
            MyBase.Add(sbi.getKey, sbi)
        Else
            tempsbi.Units = tempsbi.Units + sbi.Units
        End If
    End Sub

    Public Overloads Sub Add(ByVal TranType As String, ByVal sUom As String, ByVal sbi As StorageBillingInventory, ByVal oSku As WMS.Logic.SKU) ', ByVal dtSkuParams As DataTable)
        Dim tempsbi As StorageBillingInventory = Me(sbi.getKey)
        'Dim drParams As DataRow
        'Dim filter As String
        If tempsbi Is Nothing Then
            tempsbi = New StorageBillingInventory
            'To do the storage billing with this number of loads billing basis
            tempsbi.LoadId = sbi.LoadId

            tempsbi.Consignee = sbi.Consignee
            tempsbi.Sku = sbi.Sku
            tempsbi.Units = sbi.Units
            If sUom Is Nothing Or sUom = String.Empty Then
                tempsbi.UnitOfMeasure = oSku.LOWESTUOM 'oSku.getLowestUom() 'DataInterface.ExecuteScalar(String.Format("select top 1 uom from skuuom where (loweruom is null or loweruom = '') and consignee = {0} and sku = {1}", Made4Net.Shared.Util.FormatField(tempsbi.Consignee), Made4Net.Shared.Util.FormatField(tempsbi.Sku)))
                'filter = String.Format("(loweruom is null or loweruom = '') and consignee = '{0}' and sku = '{1}'", tempsbi.Consignee, tempsbi.Sku)
                'drParams = dtSkuParams.Select(filter)(0)
                'tempsbi.UnitOfMeasure = drParams("uom")
            Else
                'filter = String.Format("consignee = '{0}' and sku = '{1}' and uom='{2}'", tempsbi.Consignee, tempsbi.Sku, sUom)
                'drParams = dtSkuParams.Select(filter)(0)
                tempsbi.UnitOfMeasure = sUom
            End If
            'Dim sk As New SKU(tempsbi.Consignee, tempsbi.Sku)
            tempsbi.UnitsPerMeasure = oSku.ConvertToUnits(tempsbi.UnitOfMeasure)
            tempsbi.UomVolume = oSku.UOM(tempsbi.UnitOfMeasure).VOLUME 'drParams("VOLUME")
            tempsbi.UomWeight = oSku.UOM(tempsbi.UnitOfMeasure).GROSSWEIGHT 'drParams("GROSSWEIGHT")
            tempsbi.UnitPrice = oSku.UNITPRICE 'drParams("UNITPRICE")
            tempsbi.TransactionDate = sbi.TransactionDate
            Me.Add(tempsbi)
        Else
            tempsbi.Units = tempsbi.Units + sbi.Units
        End If
    End Sub

#End Region

#Region "Methods"

    Private Function getSkuInInventory() As Integer
        Dim h As Hashtable = New Hashtable
        For Each sbi As StorageBillingInventory In Me.Values
            If Not h.ContainsKey(sbi.Consignee & "#"c & sbi.Sku) Then h.Add(sbi.Consignee & "#"c & sbi.Sku, "1")
        Next
        Return h.Count
    End Function

    Public Function getBilledUnits(ByVal bb As AgreementDetail.BillingBasis) As Decimal
        Dim total As Decimal = 0
        For Each sbi As StorageBillingInventory In Me.Values
            Select Case bb
                Case AgreementDetail.BillingBasis.NumberOfSkus
                    Return Me.getSkuInInventory
                Case AgreementDetail.BillingBasis.Units
                    total = total + sbi.Units
                Case AgreementDetail.BillingBasis.Value
                    total = total + sbi.getValue()
                Case AgreementDetail.BillingBasis.Volume
                    total = total + sbi.getVolume()
                Case AgreementDetail.BillingBasis.Weight
                    ' TBD : Her we nee to check if we can ovveride the weight of the calculated bill base
                    total = total + sbi.getWeight()
                Case AgreementDetail.BillingBasis.NumberOfLoads
                    Return Me.Count
                Case Else
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Selected billing basis is not supported for this trunsaction", "Selected billing basis is not supported for this trunsaction")
                    Throw m4nEx
            End Select
        Next
        Return total
    End Function

    Public Sub UpdateRunConditionForCollection(ByVal vRunCondtition As Int32, ByVal vStoragePeriodTime As Int32, ByVal StartPeriodDate As DateTime, ByVal LastPeriodDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing)
        Dim arr As ArrayList = New ArrayList
        For Each sbi As StorageBillingInventory In Me.Values
            Select Case vRunCondtition
                Case 0 'No Condition
                    If Not BillingUtils.getInStockBeforeStartPeriodDate(sbi.LoadId, StartPeriodDate, logger) And Not BillingUtils.getEnterInSamePeriod(sbi.LoadId, StartPeriodDate, LastPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
                Case 1 ' Loads that where in warehouse before the billing period
                    If Not BillingUtils.getInStockBeforeStartPeriodDate(sbi.LoadId, StartPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
                Case 2 ' Loads that where in warehouse before the billing period and where shipped
                    If Not BillingUtils.getInStockBeforeStartPeriodDateAndShipped(sbi.LoadId, StartPeriodDate, LastPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
                Case 3 ' Loads that where in warehouse before the billing period and where not shipped
                    If Not BillingUtils.getInStockBeforeStartPeriodDateAndNotShipped(sbi.LoadId, StartPeriodDate, LastPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
                Case 4 ' Loads The where received in warehouse is the billing period
                    If Not BillingUtils.getEnterInSamePeriod(sbi.LoadId, StartPeriodDate, LastPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
                Case 5 ' Loads The where received in warehouse is the billing period and where shipped
                    If Not BillingUtils.getEnterAndExitInSamePeriod(sbi.LoadId, StartPeriodDate, LastPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
                Case 6 ' Loads The where received in warehouse is the billing period and where not shipped
                    If Not BillingUtils.getEnterInSamePeriodAndNotExit(sbi.LoadId, StartPeriodDate, LastPeriodDate, logger) Then
                        arr.Add(sbi.getKey)
                    End If
            End Select
        Next

        For Each key As Object In arr
            Me.Remove(key)
        Next
    End Sub

    'Public Sub UpdateBillingChargesLoads(ByVal ChargeId As String, ByVal ChargeLine As String)
    '    For Each sbi As StorageBillingInventory In Me.Values
    '        Dim bcl As BillingChargesLoad = New BillingChargesLoad
    '        bcl.ChargeId = ChargeId
    '        bcl.ChargeLine = ChargeLine
    '        bcl.TransactionDate = sbi.TransactionDate 'ld.RECEIVEDATE
    '        bcl.TransactionType = "STORAGE"
    '        bcl.LoadId = sbi.LoadId
    '        bcl.Consignee = sbi.Consignee
    '        bcl.Sku = sbi.Sku
    '        bcl.UnitOfMeasure = sbi.UnitOfMeasure
    '        bcl.UnitPrice = sbi.UnitPrice
    '        bcl.Units = sbi.Units
    '        bcl.UnitsPerMeasure = sbi.UnitsPerMeasure
    '        bcl.UomVolume = sbi.UomVolume
    '        bcl.UomWeight = sbi.UomWeight
    '        'Dim ld As WMS.Logic.Load = New WMS.Logic.Load(bcl.LoadId)
    '        bcl.Save()
    '    Next
    'End Sub

#End Region

End Class

Public Class StorageBillingProvider

    Protected _uom As String
    Protected _consignee As String
    Protected _skugroup As String
    Protected _trans As DataTable
    Protected _stb As StorageBillingCollection

    Public Sub New(ByVal sConsignee As String, ByVal sUom As String)
        _stb = New StorageBillingCollection
        _uom = sUom
        _consignee = sConsignee
    End Sub

    Public Sub New(ByVal sConsignee As String, ByVal sUom As String, ByVal sSkuGroup As String)
        _stb = New StorageBillingCollection
        _uom = sUom
        _consignee = sConsignee
        _skugroup = sSkuGroup
    End Sub

    Public Sub FillCurrentStorage1(Optional ByVal logger As WMS.Logic.LogHandler = Nothing)
        ' Check if the SKUGROUP filter is full and add it if neccesary
        Dim strSkuGrp As String = ""
        If Not _skugroup Is Nothing And Not _skugroup = String.Empty Then
            strSkuGrp = String.Format(" AND SKUGROUP = {0}", Made4Net.Shared.Util.FormatField(_skugroup))
            logger.Write("SkuGroup filter is : " & strSkuGrp & " : ")
        End If
        Dim sql As String = String.Format("SELECT STORAGEBILLINGBASE.BILLINGLOADID LOADID,STORAGEBILLINGBASE.CONSIGNEE, STORAGEBILLINGBASE.SKU, ISNULL(SKU.UNITPRICE,0) AS UNITPRICE, UOM.UOM, ISNULL(UOM.GROSSWEIGHT,0) AS GROSSWEIGHT, ISNULL(UOM.VOLUME,0) AS VOLUME, SUM(ISNULL(UNITS,0)) AS UNITS, ISNULL(SKUGROUP,'') AS SKUGROUP " & _
            "FROM STORAGEBILLINGBASE JOIN SKU ON STORAGEBILLINGBASE.CONSIGNEE = SKU.CONSIGNEE AND STORAGEBILLINGBASE.SKU = SKU.SKU {1} " & _
            "JOIN (SELECT CONSIGNEE, SKU, UOM, GROSSWEIGHT, VOLUME FROM SKUUOM WHERE CONSIGNEE = '{0}' ", _consignee, strSkuGrp)
        ' Check if the UOM filter is full and add it if neccesary
        If Not _uom Is Nothing And Not _uom = String.Empty Then
            sql = sql & String.Format(" AND UOM = {0}", Made4Net.Shared.Util.FormatField(_uom))
        Else
            sql = sql & String.Format(" AND (LOWERUOM IS NULL OR LOWERUOM = '')")
        End If
        sql = sql & String.Format(") AS UOM ON STORAGEBILLINGBASE.CONSIGNEE = UOM.CONSIGNEE AND STORAGEBILLINGBASE.SKU = UOM.SKU " & _
                        "WHERE STORAGEBILLINGBASE.CONSIGNEE = '{0}' AND ENDDATE IS NULL " & _
                        " GROUP BY STORAGEBILLINGBASE.BILLINGLOADID, STORAGEBILLINGBASE.CONSIGNEE, STORAGEBILLINGBASE.SKU, SKU.UNITPRICE, SKU.SKUGROUP, UOM.GROSSWEIGHT, UOM.VOLUME, UOM.UOM ", _consignee)

        If Not logger Is Nothing Then logger.Write(sql)

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Try
                _stb.Add(New StorageBillingInventory(dr))
            Catch ex As Exception
                If Not logger Is Nothing Then
                    logger.writeSeperator()
                    logger.Write(String.Format("Error adding storage when etracting current inventory: {0} {1}", dr("CONSIGNEE"), dr("SKU")))
                    logger.writeSeperator()
                End If
            End Try
        Next
    End Sub

    Public Sub FillStockTransactions1(ByVal FromDate As DateTime, Optional ByVal logger As WMS.Logic.LogHandler = Nothing)
        Dim sql As String = String.Format("SELECT BILLINGLOADID AS LOADID,LOADS.CONSIGNEE,LOADS.SKU,SKUGROUP,SUM(CURRENTQTY) UNITS, startdate, lastdate FROM BILLINGLOADS LEFT OUTER JOIN LOADS ON BILLINGLOADS.BILLINGLOADID=LOADS.LOADID LEFT OUTER JOIN SKU ON LOADS.SKU=SKU.SKU AND LOADS.CONSIGNEE=SKU.CONSIGNEE " & _
            " WHERE LASTDATE >= {0} AND LOADS.CONSIGNEE = '{1}' ", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(FromDate)), _consignee)
        ' Check if the UOM filter is full and add it if neccesary
        If Not _uom Is Nothing And Not _uom = String.Empty Then
            sql = sql & String.Format(" AND LOADS.SKU IN (SELECT SKU FROM SKUUOM WHERE CONSIGNEE = '{0}' AND UOM = '{1}') ", _consignee, _uom)
        End If
        ' Check if the SKUGROUP filter is full and add it if neccesary
        If Not _skugroup Is Nothing And Not _skugroup = String.Empty Then
            sql = sql & String.Format("AND SKUGROUP = {0}", Made4Net.Shared.Util.FormatField(_skugroup))
        End If

        sql = sql & String.Format(" GROUP BY BILLINGLOADID,LOADS.CONSIGNEE,LOADS.SKU,SKUGROUP, startdate, lastdate  ORDER BY BILLINGLOADID ")

        If Not logger Is Nothing Then logger.Write(sql)

        _trans = New DataTable
        DataInterface.FillDataset(sql, _trans)
    End Sub

    Public Sub FillBillingLoads(ByVal StartPeriod As DateTime, ByVal EndPeriod As DateTime, ByVal vRunCondtition As Int32, ByVal oPeriodType As PeriodTypes, ByVal iPeriod As Int32, Optional ByVal logger As WMS.Logic.LogHandler = Nothing)
        Dim sql As String = String.Format("Select * from vBILLINGLOADS where billingloadid in" & _
            "(SELECT min(billingloadid) as billingloadid FROM vBILLINGLOADS WHERE fromdate <= {0} and (todate is null or todate >= {1}) AND CONSIGNEE = '{2}'", Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(EndPeriod)), Made4Net.Shared.Util.FormatField(Made4Net.Shared.Util.DateTimeToDbString(StartPeriod)), _consignee)
        If Not _uom Is Nothing And Not _uom = String.Empty Then
            sql = sql & String.Format(" AND UOM = '{0}'", _uom)
        End If
        If Not _skugroup Is Nothing And Not _skugroup = String.Empty Then
            sql = sql & String.Format(" AND SKUGROUP = {0}", Made4Net.Shared.Util.FormatField(_skugroup))
        End If
        sql = sql & GetRunConditionWhereClause(vRunCondtition, StartPeriod, EndPeriod, oPeriodType, iPeriod)
        sql = sql & String.Format("  group by loadid) ORDER BY BILLINGLOADID")

        If Not logger Is Nothing Then logger.Write(sql)

        _trans = New DataTable
        DataInterface.FillDataset(sql, _trans)
    End Sub

    Private Function GetRunConditionWhereClause(ByVal vRunCondtition As Int32, ByVal StartPeriodDate As DateTime, _
                                                ByVal LastPeriodDate As DateTime, ByVal oPeriodType As PeriodTypes, ByVal iPeriod As Int32) As String
        Dim whereClause As String = String.Empty
        Select Case vRunCondtition
            Case 0 'No Condition
                whereClause = whereClause & String.Format("({0} or ({1}))", getInStockBeforeStartPeriodDateClause(StartPeriodDate), _
                        getEnterInSamePeriodClause(StartPeriodDate, LastPeriodDate))
            Case 1 ' Loads that were in warehouse before the billing period
                whereClause = whereClause & String.Format("({0})", getInStockBeforeStartPeriodDateClause(StartPeriodDate))
            Case 2 ' Loads that were in warehouse before the billing period and where shipped
                whereClause = whereClause & String.Format("({0})", getInStockBeforeStartPeriodDateAndShippedClause(StartPeriodDate, LastPeriodDate))
            Case 3 ' Loads that were in warehouse before the billing period and where not shipped
                whereClause = whereClause & String.Format("({0})", getInStockBeforeStartPeriodDateAndNotShippedClause(StartPeriodDate, LastPeriodDate))
            Case 4 ' Loads that were received in warehouse is the billing period
                whereClause = whereClause & String.Format("({0})", getEnterInSamePeriodClause(StartPeriodDate, LastPeriodDate))
            Case 5 ' Loads that were received in warehouse within the billing period and where shipped
                whereClause = whereClause & String.Format("({0})", getEnterAndExitInSamePeriodClause(StartPeriodDate, LastPeriodDate))
            Case 6 ' Loads that were received in warehouse is the billing period and where not shipped
                whereClause = whereClause & String.Format("({0})", getEnterInSamePeriodAndNotExitClause(StartPeriodDate, LastPeriodDate))
            Case 7 ' Load's birthday
                Dim sClause As String = getLoadsBirthdayClause(StartPeriodDate, LastPeriodDate, oPeriodType, iPeriod)
                If sClause <> String.Empty Then
                    whereClause = whereClause & String.Format("({0})", sClause)
                Else
                    Return String.Empty
                End If
        End Select
        Return String.Format(" And {0} ", whereClause)
    End Function

    Private Function getLoadsBirthdayClause(ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime, _
                                            ByVal oPeriodType As PeriodTypes, ByVal iPeriod As Int32) As String
        Dim sWhereClause As String
        Select Case oPeriodType
            Case PeriodTypes.Dates
                Throw New Made4Net.Shared.M4NException(New Exception, "Configuration of #Load receive date# is not supported for period type #Dates#", "Configuration of #Load receive date# is not supported for period type #Dates#")
            Case PeriodTypes.Day
                Return String.Empty
                'Case PeriodTypes.Week
                '    sWhereClause = String.Format("(DATEPART(WEEKDAY,receivedate) <= DATEPART(WEEKDAY,shipdate) or todate is null)")
                'Case PeriodTypes.Month
                '    sWhereClause = String.Format("(day(receivedate) <= day(shipdate) or todate is null)")
            Case PeriodTypes.Week
                sWhereClause = String.Format("(((case when DATEPART(WEEKDAY,receivedate)<DATEPART(WEEKDAY,'{0}')then DATEPART(WEEKDAY,receivedate)+7 else DATEPART(WEEKDAY,receivedate) end <= case when DATEPART(WEEKDAY,todate) <  DATEPART(WEEKDAY,'{0}')then DATEPART(WEEKDAY,todate)+7 else DATEPART(WEEKDAY,todate) end) or todate>'{1}'))", StartPeriodDate, EndPeriodDate)
            Case PeriodTypes.Month
                sWhereClause = String.Format("(((case when day(receivedate) < day('{0}')then day(receivedate)+31 else day(receivedate) end <= case when day(TODATE) <  day('{0}') then day(todate)+31 else day(todate) end) or todate>'{1}'))", StartPeriodDate, EndPeriodDate)
            Case Else
                Return String.Empty
        End Select
        Return sWhereClause
    End Function

    Private Function getInStockBeforeStartPeriodDateClause(ByVal StartPeriodDate As DateTime) As String
        Return String.Format("CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) < '{0}'", StartPeriodDate.ToString("yyyy-MM-dd"))
    End Function

    Private Function getEnterInSamePeriodClause(ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime) As String
        Return String.Format("CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) >= '{0}' AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) < '{1}'", StartPeriodDate.ToString("yyyy-MM-dd"), EndPeriodDate.ToString("yyyy-MM-dd"))
    End Function

    Private Function getInStockBeforeStartPeriodDateAndShippedClause(ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime) As String
        Return String.Format("(shipdate IS NOT NULL) AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) < '{0}' AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),shipdate,101)) < '{1}' AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),shipdate,101)) >= '{0}'", StartPeriodDate.ToString("yyyy-MM-dd"), EndPeriodDate.ToString("yyyy-MM-dd"))
    End Function

    Private Function getInStockBeforeStartPeriodDateAndNotShippedClause(ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime) As String
        Return String.Format("CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) < '{0}' AND (CONVERT(DATETIME,CONVERT(NVARCHAR(20),shipdate,101)) > '{1}' or shipdate is null)", StartPeriodDate.ToString("yyyy-MM-dd"), EndPeriodDate.ToString("yyyy-MM-dd"))
    End Function

    Private Function getEnterAndExitInSamePeriodClause(ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime) As String
        Return String.Format("CONVERT(DATETIME,CONVERT(NVARCHAR(20),shipdate,101)) < '{1}' AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),shipdate,101)) >= '{0}' AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) >= '{0}' AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) < '{1}'", StartPeriodDate.ToString("yyyy-MM-dd"), EndPeriodDate.ToString("yyyy-MM-dd"))
    End Function

    Private Function getEnterInSamePeriodAndNotExitClause(ByVal StartPeriodDate As DateTime, ByVal EndPeriodDate As DateTime) As String
        Return String.Format("(shipdate IS NOT NULL OR CONVERT(DATETIME,CONVERT(NVARCHAR(20),shipdate,101)) > '{1}') AND CONVERT(DATETIME,CONVERT(NVARCHAR(20),receivedate,101)) >= '{0}'", StartPeriodDate.ToString("yyyy-MM-dd"), EndPeriodDate.ToString("yyyy-MM-dd"))
    End Function

    Public Function getStockForDate(ByVal StockDate As DateTime, ByRef ItemsCache As Hashtable, Optional ByVal logger As WMS.Logic.LogHandler = Nothing) As StorageBillingCollection
        Dim stb As New StorageBillingCollection

        For Each sbi As StorageBillingInventory In _stb.Values
            stb.Add(CType(sbi.Clone(), StorageBillingInventory))
        Next
        ' Commented : The filter is done when filling transaction
        'Dim drs As DataRow() = _trans.Select(String.Format(" startdate >= {0} and lastdate <= {0}", Made4Net.Shared.Util.FormatField(StockDate)))

        'Dim dtSkuParams As New DataTable
        'Dim sSql As String = String.Format("select sku.CONSIGNEE,sku.SKU,UNITPRICE,UOM,GROSSWEIGHT,VOLUME,LOWERUOM from SKU inner join SKUUOM on SKU.CONSIGNEE = SKUUOM.CONSIGNEE and SKU.SKU = SKUUOM.SKU inner join (select distinct consignee,sku from vBillingLoads) as bl on sku.CONSIGNEE = bl.consignee and bl.sku = sku.SKU ")
        'DataInterface.FillDataset(sSql, dtSkuParams)
        Dim oSku As WMS.Logic.SKU
        For Each dr As DataRow In _trans.Rows
            Dim oStorageBillingTransactionLine As New StorageBillingInventory
            oStorageBillingTransactionLine.LoadId = CType(dr("LOADID"), String)
            oStorageBillingTransactionLine.Consignee = _consignee
            oStorageBillingTransactionLine.Sku = CType(dr("SKU"), String)
            oStorageBillingTransactionLine.Units = CType(dr("currentqty"), Decimal)
            oStorageBillingTransactionLine.TransactionDate = StockDate 'dr("TRANDATE") 'StockDate 'dr("units")
            Try
                Dim key As String = _consignee & "~" & CType(dr("SKU"), String)
                If ItemsCache.Contains(key) Then
                    oSku = CType(ItemsCache(key), WMS.Logic.SKU)
                Else
                    oSku = New WMS.Logic.SKU(_consignee, CType(dr("SKU"), String))
                    ItemsCache.Add(key, oSku)
                End If
                'stb.Add(dr("invtrntype"), _uom, oStorageBillingTransactionLine)
                stb.Add("", _uom, oStorageBillingTransactionLine, oSku)
            Catch ex As Exception
                If Not logger Is Nothing Then
                    logger.Write("skipping row in inventory")
                    'logger.Write(dr("invtrntype").PadRight(25) & "|" & oStorageBillingTransactionLine.Sku.PadRight(25) & "|" & oStorageBillingTransactionLine.Units.ToString.PadRight(10))
                    logger.Write(oStorageBillingTransactionLine.Sku.PadRight(25) & "|" & oStorageBillingTransactionLine.Units.ToString.PadRight(10))
                    logger.writeSeperator()
                End If
            End Try
        Next
        Return stb
    End Function

End Class