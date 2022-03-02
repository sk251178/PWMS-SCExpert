Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Text

#Region "Planner"

<CLSCompliant(False)> Public Class Planner

#Region "Varibales"

    Protected _reqs As PlanRequirments
    Protected _strat As PlanStrategies
    Protected _stock As VPlannerInventoryDataTable
    Protected Shared _RequirmentsAllowedStock As DataTable
    Protected _onhandstock As VAllocationOnHandDataTable
    Protected Shared _softplan As Boolean = False
    Protected Shared _orders As Hashtable
    Protected Shared _companies As Hashtable
    Protected Shared _sku As Hashtable
    'Holds the table of the picklocs
    Protected Shared _picklocdt As VPickLocDataTable
    'Holds the PickingLocation Objects - will not be initialized as the begining - only if pick loc is required in create pick detail
    'when updating the overallocation field in pickloc will be required (which will be filled from _picklocdt
    Protected Shared _picklocs As Hashtable

    Protected Shared _wave As Wave

#End Region

#Region "Properties"

    Public Shared ReadOnly Property SOFTPLAN() As Boolean
        Get
            Return _softplan
        End Get
    End Property

    Public Shared ReadOnly Property GetOutboundOrder(ByVal pConsignee As String, ByVal pOrderid As String) As OutboundOrderHeader
        Get
            If _orders.Contains(pConsignee & pOrderid) Then
                Return _orders.Item(pConsignee & pOrderid)
            End If
        End Get
    End Property

    Public Shared ReadOnly Property GetCompany(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String) As Company
        Get
            If _companies.Contains(pConsignee & pCompany & pCompanyType) Then
                Return _companies.Item(pConsignee & pCompany & pCompanyType)
            End If
        End Get
    End Property

    Public Shared ReadOnly Property GetSKU(ByVal pConsignee As String, ByVal pSku As String) As SKU
        Get
            If _sku.Contains(pConsignee & pSku) Then
                Return _sku.Item(pConsignee & pSku)
            End If
            Return Nothing
        End Get
    End Property

    Public Shared Sub SetSKU(ByVal oSku As SKU)
        If Not _sku.Contains(oSku.CONSIGNEE & oSku.SKU) Then
            _sku.Add(oSku.CONSIGNEE & oSku.SKU, oSku)
        End If
    End Sub

    Public Shared ReadOnly Property PickingLocationDataTable() As VPickLocDataTable
        Get
            If _picklocdt Is Nothing Then
                LoadPickingLocationTable()
            End If
            Return _picklocdt
        End Get
    End Property

    Public Shared ReadOnly Property IsPickingLocation(ByVal pLocationId As String, ByVal pConsignee As String, ByVal pSKU As String) As Boolean
        Get
            If _picklocdt Is Nothing Then
                LoadPickingLocationTable()
            End If
            Dim selrows As DataRow()
            selrows = _picklocdt.Select(String.Format("Location = '{0}' and Consignee='{1}' and sku='{2}'", pLocationId, pConsignee, pSKU))
            If selrows.Length > 0 Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public Shared ReadOnly Property GetPickingLocation(ByVal pLocationId As String, ByVal pWarehouseareaId As String, ByVal pConsignee As String, ByVal pSKU As String) As PickLoc
        Get
            If _picklocdt Is Nothing Then
                LoadPickingLocationTable()
            End If
            Dim selrows As DataRow()
            selrows = _picklocdt.Select(String.Format("Location = '{0}' and Consignee='{1}' and sku='{2}'", pLocationId, pConsignee, pSKU))
            If selrows.Length = 1 Then
                If _picklocs Is Nothing Then
                    _picklocs = New Hashtable
                End If
                If _picklocs.ContainsKey(pLocationId & "@" & pWarehouseareaId & "@" & pConsignee & "@" & pSKU) Then
                    Return _picklocs.Item(pLocationId & "@" & pWarehouseareaId & "@" & pConsignee & "@" & pSKU)
                Else
                    Dim tmpPickLoc As New PickLoc(selrows(0))
                    _picklocs.Item(pLocationId & "@" & pWarehouseareaId & "@" & pConsignee & "@" & pSKU) = tmpPickLoc
                    Return tmpPickLoc
                End If
            End If
            Return Nothing
        End Get
    End Property

    Public Shared Sub SetPickingLocation(ByVal PickLocation As PickLoc)
        If _picklocdt Is Nothing Then
            Return
        End If
        If _picklocs.ContainsKey(PickLocation.Location & "@" & PickLocation.Warehousearea & "@" & PickLocation.Consignee & "@" & PickLocation.SKU) Then
            _picklocs.Item(PickLocation.Location & "@" & PickLocation.Warehousearea & "@" & PickLocation.Consignee & "@" & PickLocation.SKU) = PickLocation
        End If
    End Sub

    Public Shared ReadOnly Property RequirmentsAllowedStock() As DataTable
        Get
            Return _RequirmentsAllowedStock
        End Get
    End Property

    Public Shared ReadOnly Property GetWave()
        Get
            Return _wave
        End Get
    End Property

    Public Shared Sub SetWave(ByVal pWave As Wave)
        _wave = pWave
    End Sub

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal SoftPlanning As Boolean)
        _softplan = SoftPlanning
        _orders = New Hashtable
        _companies = New Hashtable
        _sku = New Hashtable(StringComparer.InvariantCultureIgnoreCase)
    End Sub

#End Region

#Region "Method"

#Region "Plan Wave"

    Public Sub Plan(ByVal Wave As String, Optional ByVal doRelease As Boolean = False, Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        Dim wv As Wave
        Try
            wv = New Wave(Wave)
            Planner.SetWave(wv)
            LoadOrderCompaniesCache(wv)
            Plan(wv, doRelease, UserId, oLogger)
        Catch ex As Made4Net.Shared.M4NException
            wv.PlanComplete(oLogger)
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.Description)
                oLogger.Write(ex.ToString)
            End If
        Catch ex As Exception
            wv.PlanComplete(oLogger)
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.ToString)
            End If
        Finally
            wv = Nothing
        End Try
    End Sub

    Public Sub Plan(ByVal Wave As Wave, Optional ByVal doRelease As Boolean = False, Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.StartWrite()
            'Start RWMS-768
            oLogger.WriteTimeStamp = True
            'End  RWMS-768
            oLogger.Write("Planing Wave: " & Wave.WAVE)
            oLogger.writeSeperator(" ", 100)
        End If
        _reqs = New PlanRequirments(Wave, oLogger)                      ' Create Wave Requirments
        _strat = New PlanStrategies(oLogger)                            ' Create Planning Strategies
        _stock = getWaveStock(Wave, oLogger)                            ' Get Current Warehouse Stock
        _RequirmentsAllowedStock = GetRequirementsAllowedStock(Wave)
        _onhandstock = GetWaveOnHandStock(Wave)                         ' Get On Hand Warehouse Stock (depends on incoming inventory)
        LoadPickingLocationTable()
        AssignToStrategies(oLogger)                                     ' Assign Requirments to strategies
        _strat.Plan(_stock, _onhandstock, oLogger)                       ' Plan Strategies
        _strat.CreatePickList(oLogger)                                  ' Create Picklists
        resetActivityStatus(_stock, oLogger)

        If Not SOFTPLAN Then
            Wave.PlanComplete(oLogger)

            If doRelease Or _strat.ShouldRelease(oLogger) Then 'RWMS-1279
                Wave.Release(UserId)
            Else
                _strat.ReleasePickLists(oLogger)
            End If

            'Begin RWMS-2599
            Wave.CheckException(UserId, oLogger)
            'End RWMS-2599
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("Planing wave : " & Wave.WAVE & " finished.")
            oLogger.EndWrite()
        End If
    End Sub

    Private Sub resetActivityStatus(stock As VPlannerInventoryDataTable, Optional ByVal oLogger As LogHandler = Nothing)
        Dim load As Load
        For Each dr As DataRow In stock.Rows
            load = New Load(dr("LOADID"), True)
            load.SetConditionalActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Lib.USERS.SYSTEMUSER, WMS.Lib.Statuses.ActivityStatus.ALLOCPENDING, oLogger)
        Next
    End Sub

    Protected Function getWaveStock(ByVal Wave As Wave, Optional ByVal oLogger As LogHandler = Nothing) As VPlannerInventoryDataTable
        Dim dt As VPlannerInventoryDataTable = New VPlannerInventoryDataTable
        Dim sql As String = BuildStockSQL(String.Format(" wave = '{0}'", Wave.WAVE))  'String.Format("select * from vPlannerInventory where wave = '{0}'", Wave.WAVE)
        'Start RWMS-768
        If Not oLogger Is Nothing Then
            oLogger.Write("Get Wave stock : " & sql)

        End If
        'End  RWMS-768
        DataInterface.FillDataset(sql, dt)
        Return dt
    End Function

    Protected Function GetRequirementsAllowedStock(ByVal Wave As Wave, Optional ByVal oLogger As LogHandler = Nothing) As DataTable
        Dim dt As DataTable = New DataTable
        Dim sql As String = String.Format("select * from vPlannerInventory where wave = '{0}'", Wave.WAVE)
        DataInterface.FillDataset(sql, dt)
        Return dt
    End Function

    Protected Function GetWaveOnHandStock(ByVal Wave As Wave, Optional ByVal oLogger As LogHandler = Nothing) As VAllocationOnHandDataTable
        Dim dt As New VAllocationOnHandDataTable
        Dim sql As String = String.Format("select wave, consignee, orderid , sku, onhandqty from vAllocationOnHand where wave = '{0}'", Wave.WAVE)
        If Not oLogger Is Nothing Then
            oLogger.Write("GetWaveOnHandStock : " & sql)
        End If
        DataInterface.FillDataset(sql, dt)
        Return dt
    End Function

#End Region

#Region "Plan Order"

    Public Function PlanOrder(ByVal pConsignee As String, ByVal pOrderid As String, Optional ByVal doRelease As Boolean = False, Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        Dim oOrder As OutboundOrderHeader
        Try
            oOrder = New OutboundOrderHeader(pConsignee, pOrderid)
            LoadOrderCompaniesCache(oOrder)
            Return PlanOrder(oOrder, doRelease, UserId, oLogger)
        Catch ex As Made4Net.Shared.M4NException
            If Not oOrder Is Nothing Then
                oOrder.PlanComplete(oLogger)
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.Description)
                oLogger.Write(ex.ToString)
            End If
            'Added for Sending the Message to deadletterQueue Start
            Throw
            'Added for Sending the Message to deadletterQueue End
        Catch ex As Exception
            If Not oOrder Is Nothing Then
                oOrder.PlanComplete(oLogger)
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.ToString)
            End If
            'Added for Sending the Message to deadletterQueue Start
            Throw
            'Added for Sending the Message to deadletterQueue Start
        Finally
            oOrder = Nothing
        End Try
    End Function

    Public Function PlanOrder(ByVal oOrder As OutboundOrderHeader, Optional ByVal doRelease As Boolean = False, Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.StartWrite()
            oLogger.Write("Planing order: " & oOrder.ORDERID & " for Consignee : " & oOrder.CONSIGNEE)
            oLogger.writeSeperator(" ", 100)
        End If
        _reqs = New PlanRequirments(oOrder, oLogger)                    ' Create Orders Requirments
        LoadItemsCache()
        _strat = New PlanStrategies(oLogger)                            ' Create Planning Strategies
        _stock = getOrdersStock(oOrder, oLogger)                        ' Get Current Warehouse Stock
        _RequirmentsAllowedStock = GetRequirementsAllowedStock(oOrder)
        _onhandstock = GetOrderOnHandStock(oOrder, oLogger)             ' Get On Hand Warehouse Stock (depends on incoming inventory)
        LoadPickingLocationTable()
        AssignToStrategies(oLogger)                                     ' Assign Requirments to strategies
        _strat.Plan(_stock, _onhandstock, oLogger)                      ' Plan Strategies
        _strat.CreatePickList(oLogger)                                  ' Create Picklists
        resetActivityStatus(_stock, oLogger)

        If Not SOFTPLAN Then
            oOrder.PlanComplete(oLogger)
            'Begin RWMS-2599
            oOrder.CheckException(UserId, oLogger)
            'End RWMS-2599
            _strat.ReleasePickLists(oLogger)
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("Planing order " & oOrder.ORDERID & " finished.")
            oLogger.EndWrite()
        End If
    End Function

    Protected Function getOrdersStock(ByVal oOrder As OutboundOrderHeader, Optional ByVal oLogger As LogHandler = Nothing) As VPlannerInventoryDataTable
        Dim dt As DataTable = New VPlannerInventoryDataTable
        Dim sql As String = BuildStockSQL(String.Format(" consignee = '{0}' and orderid = '{1}'", oOrder.CONSIGNEE, oOrder.ORDERID))
        DataInterface.FillDataset(sql, dt)
        Return dt
    End Function

    Protected Function GetRequirementsAllowedStock(ByVal oOrder As OutboundOrderHeader, Optional ByVal oLogger As LogHandler = Nothing) As DataTable
        Dim dt As DataTable = New DataTable
        Dim sql As String = String.Format("select * from vPlannerInventory where consignee = '{0}' and orderid = '{1}'", oOrder.CONSIGNEE, oOrder.ORDERID)
        DataInterface.FillDataset(sql, dt)
        Return dt
    End Function

    Protected Function GetOrderOnHandStock(ByVal oOrder As OutboundOrderHeader, Optional ByVal oLogger As LogHandler = Nothing) As VAllocationOnHandDataTable
        Dim dt As New VAllocationOnHandDataTable
        Dim sql As String = String.Format("select consignee,orderid,sku,sum(onhandqty) as onhandqty from vAllocationOnHand where vAllocationOnHand.consignee = '{0}' and orderid = '{1}' group by consignee, orderid, sku", oOrder.CONSIGNEE, oOrder.ORDERID)
        DataInterface.FillDataset(sql, dt)
        Return dt
    End Function

#End Region

#Region "Accessors"

    Private Function BuildStockSQL(ByVal pWhereCluse As String) As String


        'Commented for RWMS-1185 Start
        '    Dim queryFields As String = String.Empty
        '    Dim objectId As String = "PlannerSchema"
        '    'commented temporraily, enum def not found in members, not defined by previous programmer
        '    'If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.PlannerInventorySchema, objectId) Then
        '    If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AttributeTableSchema, objectId) Then
        '        queryFields = BuildStockQueryFields()
        '    Else
        '        'queryFields = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.PlannerInventorySchema, objectId)
        '        queryFields = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AttributeTableSchema, objectId)
        '        If IsNothing(queryFields) Then
        '            queryFields = BuildStockQueryFields()
        '            'GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.PlannerInventorySchema, objectId, queryFields)
        '            GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AttributeTableSchema, objectId, queryFields)
        '        End If
        '    End If

        '    Return String.Format("Select distinct {0} from vPlannerInventory where {1}", queryFields, pWhereCluse)
        'End Function
        'Private Function BuildStockQueryFields() As String

        'Commented for RWMS-1185 End

        Dim dataTable As New DataTable

        Dim sSql As String = String.Format("select column_name from information_schema.columns where table_name = 'vPlannerInventory' and column_name not in ('WAVE','ORDERID')")
        DataInterface.FillDataset(sSql, dataTable)

        Dim queryFields As String
        For Each dr As DataRow In dataTable.Rows
            queryFields = queryFields & dr("column_name") & ","
        Next

        queryFields = queryFields.Remove(queryFields.Length - 1, 1)
        dataTable.Dispose()
        Return String.Format("Select distinct {0} from vPlannerInventory where {1}", queryFields, pWhereCluse)
    End Function

    Protected Shared Sub LoadPickingLocationTable()
        _picklocdt = New VPickLocDataTable
        _picklocs = New Hashtable
        Dim sql As String = "select *,0 as AvailableUnits,0 as NumOfReplDue from vpickloc order by locsortorder"

        DataInterface.FillDataset(sql, _picklocdt)
    End Sub

    Protected Sub AssignToStrategies(Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.writeSeperator()
            oLogger.Write("Getting Requirements : ")
            oLogger.writeSeperator("-", 20)
            oLogger.writeSeperator(" ", 20)
        End If
        Dim req As Requirment
        Dim strat As PlanStrategy

        If Not oLogger Is Nothing Then
            oLogger.Write("Consignee".PadRight(20) & "Order".PadRight(20) & "Line".PadRight(10) & "SKU".PadRight(20) & "Policy".PadRight(10))
            oLogger.writeSeperator("-", 80)
        End If

        For Each req In _reqs
            If Not req.Strategy Is Nothing Then
                strat = _strat(req.Strategy)
                If Not strat Is Nothing Then
                    If Not oLogger Is Nothing Then
                        'Uncommented for Retrofit Item PWMS-748 (RWMS-439) Start
                        oLogger.Write("Found plan strategy to assign , strategyID is : " & strat.StrategyId)
                        'Uncommented for Retrofit Item PWMS-748 (RWMS-439) End
                        req.WriteToLog(oLogger)
                    End If
                    strat.addRequirment(req, oLogger)
                End If
            End If
        Next
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
        _strat.ExportToLog(oLogger)
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        If Not oLogger Is Nothing Then
            'Uncommented for Retrofit Item PWMS-748 (RWMS-439) Start
            oLogger.Write("Finished assigning Requirments to strategies...")
            'Uncommented for Retrofit Item PWMS-748 (RWMS-439) End
            oLogger.writeSeperator(" ", 20)
        End If
    End Sub

    Private Sub LoadOrderCompaniesCache(ByVal pWave As Wave)
        Dim tmpOrder As OutboundOrderHeader
        Dim sConsignee, sOrderID, sCompany, sCompanyType As String
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select oh.* from wavedetail orq inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = orq.CONSIGNEE and orq.ORDERID = oh.ORDERID where WAVE = '" & pWave.WAVE & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            sConsignee = dr("consignee")
            sCompany = dr("targetcompany")
            sCompanyType = dr("companytype")
            sOrderID = dr("orderid")
            If Not _orders.Contains(sConsignee & sOrderID) Then
                tmpOrder = New OutboundOrderHeader(dr) 'New OutboundOrderHeader(dr("consignee"), dr("orderid"))
                _orders.Add(tmpOrder.CONSIGNEE & tmpOrder.ORDERID, tmpOrder)
            Else
                tmpOrder = _orders(sConsignee & sOrderID)
            End If
            If Not _companies.Contains(sConsignee & sCompany & sCompanyType) Then
                _companies.Add(sConsignee & sCompany & sCompanyType, New Company(sConsignee, sCompany, sCompanyType))
            End If
        Next
    End Sub

    Private Sub LoadOrderCompaniesCache(ByVal pOrder As OutboundOrderHeader)
        If Not _orders.Contains(pOrder.CONSIGNEE & pOrder.ORDERID) Then
            _orders.Add(pOrder.CONSIGNEE & pOrder.ORDERID, pOrder)
        End If
        If Not _companies.Contains(pOrder.CONSIGNEE & pOrder.TARGETCOMPANY & pOrder.COMPANYTYPE) Then
            _companies.Add(pOrder.CONSIGNEE & pOrder.TARGETCOMPANY & pOrder.COMPANYTYPE, New Company(pOrder.CONSIGNEE, pOrder.TARGETCOMPANY, pOrder.COMPANYTYPE))
        End If
    End Sub

    Private Sub LoadItemsCache()
        For Each req As Requirment In _reqs
            If Not _sku.Contains(req.Consignee & req.Sku) Then
                _sku.Add(req.Consignee & req.Sku, New SKU(req.Consignee, req.Sku))
            End If
        Next
    End Sub

#End Region

#End Region

End Class

#End Region