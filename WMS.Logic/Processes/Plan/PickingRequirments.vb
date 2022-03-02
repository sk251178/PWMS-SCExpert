Imports Made4Net.DataAccess

#Region "Picking Requirments"

<CLSCompliant(False)> Public Class PickingRequirments
    Inherits Requirments

#Region "Variables"

    Protected _consignee As String
    Protected _sku As String
    Protected _inventorystatus As String
    Protected _reqattributes As AttributesCollection
    Protected _allocs As AllocationCollection
    Protected _unitsreq As Double = 0
    Protected _unitsfullfilled As Double = 0
    Protected _reqvolume As Double = 0
    Protected _warehousearea As String = ""
#End Region

#Region "Properties"

    Public ReadOnly Property Consignee() As String
        Get
            Return _consignee
        End Get
    End Property

    Public Property Sku() As String
        Set(ByVal Value As String)
            _sku = Value
        End Set
        Get
            Return _sku
        End Get
    End Property

    Public ReadOnly Property InventoryStatus() As String
        Get
            Return _inventorystatus
        End Get
    End Property

    Public ReadOnly Property UnitsRequired() As Double
        Get
            Return _unitsreq
        End Get
    End Property

    Public ReadOnly Property UnitsFullfilled() As Double
        Get
            Return _unitsfullfilled
        End Get
    End Property

    Public ReadOnly Property UnitsLeftToFullfill() As Double
        Get
            Return _unitsreq - _unitsfullfilled
        End Get
    End Property

    Public ReadOnly Property RequirementsVolume() As Double
        Get
            Return _reqvolume
        End Get
    End Property

    Public ReadOnly Property Allocated() As AllocationCollection
        Get
            Return _allocs
        End Get
    End Property

    Public ReadOnly Property Attributes() As AttributesCollection
        Get
            Return _reqattributes
        End Get
    End Property

    Public ReadOnly Property WarehouseArea() As String
        Get
            Return _warehousearea
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        MyBase.New()
        _allocs = New AllocationCollection
    End Sub

#End Region

#Region "Methods"

    'Public Sub Allocate(ByVal oPlanStrategy As PlanStrategy, ByVal Stock As DataTable, ByVal OnHandStock As DataTable, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByVal PickType As String, ByVal AllocFullRequirment As Boolean, ByVal AllocByStratLineUOM As Boolean, ByVal pStratLineNPPVol As Decimal, ByVal AllocByHighestUOM As Boolean, Optional ByVal UOM As String = Nothing, Optional ByVal StrategyLineFilter As String = "", Optional ByVal AllocationUOMSize As String = "", Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pAllocateSubstituesSku As Boolean = False)
    '    Dim newallocs As AllocationCollection = StockSelector.Allocate(oPlanStrategy, Stock, OnHandStock, Me, pStratLineNPPVol, pZPicking, pStrategyAllocationSequence, PickType, AllocFullRequirment, AllocByStratLineUOM, AllocByHighestUOM, UOM, StrategyLineFilter, AllocationUOMSize, oLogger, pAllocateSubstituesSku)
    '    If Not newallocs Is Nothing Then
    '        _allocs.AddRange(newallocs)
    '        _unitsfullfilled = _unitsfullfilled + newallocs.AllocatedUnits
    '    End If
    'End Sub

    Public Sub Allocate(ByVal oPlanStrategy As PlanStrategy, ByVal Stock As VPlannerInventoryDataTable, ByVal OnHandStock As VAllocationOnHandDataTable, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByVal PickType As String, ByVal AllocFullRequirment As Boolean, ByVal AllocByStratLineUOM As Boolean, ByVal pStratLineNPPVol As Decimal, ByVal AllocByHighestUOM As Boolean, Optional ByVal UOM As String = Nothing, Optional ByVal StrategyLineFilter As String = "", Optional ByVal AllocationUOMSize As String = "", Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pSubstituesSkuData As SKU.SkuSubstitutes = Nothing, Optional ByVal pFullPickAllocation As String = "ALWAYS")
        Dim newallocs As AllocationCollection = StockSelector.Allocate(oPlanStrategy, Stock, OnHandStock, Me, pStratLineNPPVol, pZPicking, pStrategyAllocationSequence, PickType, AllocFullRequirment, AllocByStratLineUOM, AllocByHighestUOM, UOM, StrategyLineFilter, AllocationUOMSize, oLogger, pSubstituesSkuData, pFullPickAllocation)
        If Not newallocs Is Nothing Then
            _allocs.AddRange(newallocs)
            _unitsfullfilled = _unitsfullfilled + newallocs.AllocatedUnits
        End If
    End Sub

    Public Sub PickLocAllocate(ByVal pOverAllocationAllowed As Boolean, ByVal oPlanStrategy As PlanStrategy, ByVal OnHandStock As VAllocationOnHandDataTable, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByVal PickType As String, ByVal AllocFullRequirment As Boolean, ByVal AllocByStratLineUOM As Boolean, ByVal pStratLineNPPVol As Decimal, ByVal AllocByHighestUOM As Boolean, Optional ByVal UOM As String = Nothing, Optional ByVal StrategyLineFilter As String = "", Optional ByVal AllocationUOMSize As String = "", Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pAllocateSubstituesSku As Boolean = False)
        Dim newOverAllocs As AllocationCollection = StockSelector.PickLocAllocate(pOverAllocationAllowed, oPlanStrategy, OnHandStock, Me, pStratLineNPPVol, pZPicking, pStrategyAllocationSequence, PickType, AllocFullRequirment, AllocByStratLineUOM, AllocByHighestUOM, UOM, StrategyLineFilter, AllocationUOMSize, oLogger, pAllocateSubstituesSku)
        If Not newOverAllocs Is Nothing Then
            _allocs.AddRange(newOverAllocs)
            _unitsfullfilled = _unitsfullfilled + newOverAllocs.AllocatedUnits

        End If
    End Sub

    Public Overrides Function CanPlace(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing) As Boolean


        If _consignee = req.Consignee And _sku = req.Sku And _inventorystatus = req.InventoryStatus _
          And AttributesCollection.Equal(_reqattributes, req.Attributes) AndAlso _warehousearea = req.WarehouseArea Then

            If Not oLogger Is Nothing Then
                oLogger.Write("Picking Requirements CanPlace() requirement Consignee=" & req.Consignee & ", Requirement WarehouseArea=" & req.WarehouseArea)
            End If
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Sub Place(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing)
        If Count = 0 Then
            _consignee = req.Consignee
            _sku = req.Sku
            _inventorystatus = req.InventoryStatus
            _reqattributes = req.Attributes
            _warehousearea = req.WarehouseArea
        End If
        MyBase.Place(req, oLogger)
        _unitsreq = _unitsreq + req.QuantityLeftToFullfill
        _reqvolume += req.RequirementVolume
    End Sub

    Public Function CloneForSubSKU(ByVal pSubSKU As SKU, ByVal pConversionUnits As Decimal)
        Dim tmpPickingRequirments As New PickingRequirments()

        For Each req As Requirment In Me._req
            tmpPickingRequirments.Add(req.CloneForSubSKU(pSubSKU, pConversionUnits))
        Next
        tmpPickingRequirments._consignee = pSubSKU.CONSIGNEE
        tmpPickingRequirments._inventorystatus = Me.InventoryStatus
        tmpPickingRequirments._reqattributes = Me.Attributes
        'Udi tmpPickingRequirments._reqvolume =
        tmpPickingRequirments._sku = pSubSKU.SKU
        tmpPickingRequirments._unitsfullfilled = 0 'Me.UnitsFullfilled * pConversionUnits
        tmpPickingRequirments._unitsreq = Me.UnitsRequired * pConversionUnits
        tmpPickingRequirments._warehousearea = Me.WarehouseArea
        Return tmpPickingRequirments

    End Function

#End Region

End Class

#End Region

#Region "Requirment"

<CLSCompliant(False)> Public Class Requirment

#Region "Variables"

    'Shipment/Wave
    Protected _wave As String
    Protected _shipment As String

    'Order Header Information
    Protected _consignee As String
    Protected _orderid As String
    Protected _ordertype As String
    Protected _targetcompany As String
    Protected _companytype As String
    Protected _companygroup As String
    Protected _staginglane As String
    Protected _staginglanewharea As String
    Protected _route As String
    Protected _ordervalue As Decimal
    Protected _ordervolume As Decimal
    Protected _shipto As String
    Protected _orderpriority As Int32
    Protected _orderunits As Decimal

    'Order Line Information
    Protected _orderline As Int32
    Protected _sku As String
    Protected _invstat As String
    Protected _qtylefttofullfill As Double
    Protected _qtyfullfilled As Double
    Protected _warehousearea As String
    Protected _linevalue As Decimal
    Protected _linevolume As Decimal
    Protected _orderlines As Decimal
    Protected _lineunits As Decimal

    'Sku information
    Protected _velocity As String
    Protected _skugroup As String
    Protected _classname As String
    Protected _picksortorder As String

    'Strategy Selection
    Protected _strategy As String

    'Order Line Attributes Collection
    Protected _attributecollection As AttributesCollection

    'Loading Inforamtion params
    Protected _carrier As String
    Protected _unloadingtype As String
    Protected _vehicletype As String

    Protected _reqvolume As Decimal
    Protected _reqweight As Decimal
    Protected _hazclass As String

    'Substitute Sku Collection
    Protected _substitutescollection As SKU.SkuSubstitutesCollection

#End Region

#Region "Properties"

    Public ReadOnly Property Wave() As String
        Get
            Return _wave
        End Get
    End Property

    Public ReadOnly Property Shipment() As String
        Get
            Return _shipment
        End Get
    End Property

    Public ReadOnly Property OrderId() As String
        Get
            Return _orderid
        End Get
    End Property

    Public ReadOnly Property Consignee() As String
        Get
            Return _consignee
        End Get
    End Property

    Public ReadOnly Property OrderType() As String
        Get
            Return _ordertype
        End Get
    End Property

    Public ReadOnly Property ShipTo() As String
        Get
            Return _shipto
        End Get
    End Property

    Public ReadOnly Property Company() As String
        Get
            Return _targetcompany
        End Get
    End Property

    Public ReadOnly Property CompanyType() As String
        Get
            Return _companytype
        End Get
    End Property

    Public ReadOnly Property CompanyGroup() As String
        Get
            Return _companygroup
        End Get
    End Property

    Public ReadOnly Property StagingLane() As String
        Get
            Return _staginglane
        End Get
    End Property

    Public ReadOnly Property Route() As String
        Get
            Return _route
        End Get
    End Property

    Public ReadOnly Property OrderValue() As Decimal
        Get
            Return _ordervalue
        End Get
    End Property

    Public ReadOnly Property OrderVolume() As Decimal
        Get
            Return _ordervolume
        End Get
    End Property

    Public ReadOnly Property OrderLine() As Int32
        Get
            Return _orderline
        End Get
    End Property

    Public ReadOnly Property LineValue() As Decimal
        Get
            Return _linevalue
        End Get
    End Property

    Public ReadOnly Property LineVolume() As Decimal
        Get
            Return _linevolume
        End Get
    End Property

    Public ReadOnly Property OrderLines() As Int32
        Get
            Return _orderlines
        End Get
    End Property

    Public ReadOnly Property LineUnits() As Decimal
        Get
            Return _lineunits
        End Get
    End Property

    Public ReadOnly Property OrderPriority() As Int32
        Get
            Return _orderpriority
        End Get
    End Property

    Public ReadOnly Property Sku() As String
        Get
            Return _sku
        End Get
    End Property

    Public ReadOnly Property InventoryStatus() As String
        Get
            Return _invstat
        End Get
    End Property

    Public Property QuantityLeftToFullfill() As Double
        Get
            Return _qtylefttofullfill
        End Get
        Set(ByVal Value As Double)
            _qtylefttofullfill = Value
        End Set
    End Property

    Public Property QuantityFullfilled() As Double
        Get
            Return _qtyfullfilled
        End Get
        Set(ByVal Value As Double)
            _qtyfullfilled = Value
        End Set
    End Property

    Public ReadOnly Property Velocity() As String
        Get
            Return _velocity
        End Get
    End Property

    Public ReadOnly Property SkuClass() As String
        Get
            Return _classname
        End Get
    End Property

    Public ReadOnly Property SkuGroup() As String
        Get
            Return _skugroup
        End Get
    End Property

    Public ReadOnly Property PickSortOrder() As String
        Get
            Return _picksortorder
        End Get
    End Property

    Public Property Strategy() As String
        Get
            Return _strategy
        End Get
        Set(ByVal Value As String)
            _strategy = Value
        End Set
    End Property

    Public Property OrderUnits() As Double
        Get
            Return _orderunits
        End Get
        Set(ByVal Value As Double)
            _orderunits = Value
        End Set
    End Property

    Public ReadOnly Property Attributes() As AttributesCollection
        Get
            Return _attributecollection
        End Get
    End Property

    Public ReadOnly Property SKUSubstitutes() As ArrayList
        Get
            Return _substitutescollection
        End Get
    End Property

    Public ReadOnly Property Carrier() As String
        Get
            Return _carrier
        End Get
    End Property

    Public ReadOnly Property UnloadingType() As String
        Get
            Return _unloadingtype
        End Get
    End Property

    Public ReadOnly Property VehicleType() As String
        Get
            Return _vehicletype
        End Get
    End Property

    Public Property RequirementVolume() As Double
        Get
            Return _reqvolume
        End Get
        Set(ByVal Value As Double)
            _reqvolume = Value
        End Set
    End Property

    Public Property RequirementWeight() As Double
        Get
            Return _reqweight
        End Get
        Set(ByVal Value As Double)
            _reqweight = Value
        End Set
    End Property

    Public ReadOnly Property HazardClass() As String
        Get
            Return _hazclass
        End Get
    End Property

    Public ReadOnly Property StaginglaneWHArea() As String
        Get
            Return _staginglanewharea
        End Get
    End Property

    Public Property WarehouseArea() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal value As String)
            _warehousearea = value
        End Set
    End Property

#End Region

#Region "Ctor"

    Public Sub New(ByVal requirment As DataRow, ByVal oAttributeDataTables As DataTable, Optional ByVal oLogger As LogHandler = Nothing)
        _wave = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("wave"))
        _shipment = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("shipment"))
        _consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("consignee"))
        _orderid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("orderid"))
        _ordertype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("ordertype"))
        _targetcompany = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("targetcompany"), "%")
        _companytype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("companytype"))
        _companygroup = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("companygroup"))
        _staginglane = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("staginglane"))
        _staginglanewharea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("stagingwarehousearea"))
        _route = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("route"))
        _ordervalue = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("ordervalue"))
        _ordervolume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("ordervolume"))
        _orderline = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("orderline"))
        _sku = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("sku"))
        _invstat = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("inventorystatus"))
        _qtylefttofullfill = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("qtylefttofullfill"))
        _velocity = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("velocity"))
        _skugroup = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("skugroup"))
        _classname = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("classname"))
        _picksortorder = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("picksortorder"))
        _linevalue = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("linevalue"))
        _linevolume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("linevolume"))
        _orderlines = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("orderlines"))
        _lineunits = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("lineunits"))
        _orderpriority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("orderpriority"))
        _orderunits = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("orderunits"))
        _vehicletype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("vehicletype"))
        _unloadingtype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("unloadingtype"))
        _carrier = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("carrier"))
        _reqvolume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("leftqtyvolume"))
        _reqweight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("leftqtyweight"))
        _hazclass = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("hazclass"))
        _shipto = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("shipto"))
        _warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment("warehousearea"))

        _attributecollection = New AttributesCollection
        For Each drAttribute As DataRow In oAttributeDataTables.Rows
            Dim val As Object = requirment(drAttribute("Name"))
            If Not val Is Nothing And Not val Is DBNull.Value Then
                _attributecollection.Add(drAttribute("Name"), val)
            End If
        Next

        _qtyfullfilled = 0

        'Build the subs collection, and add the sku to the items cache of the planner...
        If Planner.GetSKU(_consignee, _sku) Is Nothing Then
            Dim oTempSku As SKU
            oTempSku = New SKU(_consignee, _sku)
            Planner.SetSKU(oTempSku)
            _substitutescollection = oTempSku.SUBSTITUTESSKU
            ' And add the subs skus to the cache as well
            For Each subSku As SKU.SkuSubstitutes In _substitutescollection
                oTempSku = New SKU(subSku.CONSIGNEE, subSku.SUBSTITUTESKU)
                Planner.SetSKU(oTempSku)
            Next
        End If
    End Sub

    Public Sub New()

    End Sub

#End Region

#Region "Methods"

    Public Sub SetPlanStrategy(ByVal pDtPolicy As DataTable)
        If pDtPolicy.Rows.Count > 0 Then
            _strategy = pDtPolicy.Rows(0)("policyid")
        End If
    End Sub

    Private Sub setStrategy(ByVal policies As DataTable, Optional ByVal oLogger As LogHandler = Nothing)
        Dim dr() As DataRow
        Dim sql As String = "consignee = '" & _consignee & "' and '" & _sku & "' like sku and '" & _ordertype & "' like ordertype and '" & _classname & "' like classname " &
            "and '" & _velocity & "' like velocity and '" & _skugroup & "' like skugroup and '" & _companytype & "' like companytype and '" & _route & "' like route and ordervalue <= " & _ordervalue.ToString().Replace(",", ".") & " and ordervolume <= " & Convert.ToString(_ordervolume).Replace(",", ".") &
            " and " & _orderlines & " >= orderlines and " & Convert.ToString(_lineunits).Replace(",", ".") & " >= linesunits and " & Convert.ToString(_linevolume).Replace(",", ".") & " >= linevolume and " & _linevalue.ToString().Replace(",", ".") & " >= linevalue and '" & _targetcompany & "' like targetcompany and '" & _companygroup & "' like companygroup and " & _orderpriority & " = orderpriority and '" & _invstat & "' like invstatus and " & _orderunits.ToString().Replace(",", ".") & " >= orderunits and '" & _carrier & "' like carrier  and  '" & _vehicletype & "' like vehicletype  and '" & _unloadingtype & "' like unloadingtype and '" & _hazclass & "' like hazclass "

        dr = policies.Select(sql, "priority asc")

        If Not dr Is Nothing And dr.Length > 0 Then
            _strategy = dr(0)("STRATEGYID")
            If Not oLogger Is Nothing Then
                oLogger.Write("Selected Strategy : " & _strategy & " for SKU :" & _sku)
                oLogger.writeSeperator()
                oLogger.Write("SQL Query used for selecting the Strategy : " & sql)
            End If
        End If
    End Sub

    Public Sub WriteToLog(Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write(Me.Consignee.PadRight(20) & Me.OrderId.PadRight(20) & Me.OrderLine.ToString.PadRight(10) & Me.Sku.PadRight(20) & Me.Strategy.PadRight(10))
        End If
    End Sub

    Public Function CloneForSubSKU(ByVal pSubSKU As SKU, ByVal pConversionUnits As Decimal) As Requirment
        Dim tmpReq As New Requirment()

        tmpReq._wave = Me.Wave
        'tmpReq._shipment = Me.Shipment

        tmpReq._orderid = Me.OrderId
        tmpReq._ordertype = Me.OrderType
        tmpReq._targetcompany = Me.Company
        tmpReq._companytype = Me.CompanyType
        ' tmpReq._companygroup = Me.CompanyGroup
        tmpReq._staginglane = Me.StagingLane
        tmpReq._staginglanewharea = Me.StaginglaneWHArea
        'tmpReq._route = Me.Route

        tmpReq._invstat = Me.InventoryStatus

        'tmpReq._ordervalue = Me.OrderValue
        'tmpReq._ordervolume = Me.OrderVolume
        tmpReq._orderline = Me.OrderLine

        tmpReq._consignee = pSubSKU.CONSIGNEE
        tmpReq._sku = pSubSKU.SKU

        tmpReq._qtylefttofullfill = pConversionUnits * Me.QuantityLeftToFullfill
        tmpReq._qtyfullfilled = pConversionUnits * Me.QuantityFullfilled


        tmpReq._orderunits = Me.OrderUnits
        'tmpReq._velocity = pSKU.VELOCITY
        'tmpReq._skugroup = pSKU.SKUGROUP
        'tmpReq._classname = pSKU.SKUClassName
        'tmpReq._picksortorder = pSKU.PICKSORTORDER
        '_linevalue = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("linevalue"))
        '_linevolume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("linevolume"))
        '_orderlines = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("orderlines"))
        '_lineunits = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("lineunits"))
        '_orderpriority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("orderpriority"))

        '_vehicletype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("vehicletype"))
        '_unloadingtype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("unloadingtype"))
        '_carrier = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("carrier"))
        '_reqvolume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("leftqtyvolume"))
        '_reqweight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("leftqtyweight"))
        '_hazclass = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("hazclass"))
        '_shipto = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("shipto"))
        '_warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Requirment("warehousearea"))

        tmpReq._attributecollection = Me.Attributes


        'Build the subs collection, and add the sku to the items cache of the planner...
        'If Planner.GetSKU(_consignee, _sku) Is Nothing Then
        ' Dim oTempSku As SKU
        'oTempSku = New SKU(_consignee, _sku)
        'Planner.SetSKU(oTempSku)
        '_substitutescollection = oTempSku.SUBSTITUTESSKU
        ' And add the subs skus to the cache as well
        'For Each subSku As SKU.SkuSubstitutes In _substitutescollection
        'oTempSku = New SKU(subSku.CONSIGNEE, subSku.SUBSTITUTESKU)
        'Planner.SetSKU(oTempSku)
        'Next
        'End If

        Return tmpReq
    End Function

#End Region

End Class

#End Region

#Region "Requirment Collection"

<CLSCompliant(False)> Public MustInherit Class Requirments
    Implements ICollection

#Region "Variables"
    Protected _req As ArrayList
#End Region

#Region "Constructor"
    Public Sub New()
        _req = New ArrayList
    End Sub
#End Region

#Region "Methods"
    Public MustOverride Function CanPlace(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing) As Boolean

    Public Overridable Sub Place(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing)
        Add(req)
    End Sub
#End Region

#Region "Properties"
    Default Public Property Item(ByVal index As Int32) As Requirment
        Get
            Return CType(_req(index), Requirment)
        End Get
        Set(ByVal Value As Requirment)
            _req(index) = Value
        End Set
    End Property
#End Region

#Region "Overrides"

    Public Function Add(ByVal value As Requirment) As Integer
        Return _req.Add(value)
    End Function

    Public Sub Clear()
        _req.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As Requirment)
        _req.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As Requirment)
        _req.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _req.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _req.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _req.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _req.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _req.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _req.GetEnumerator()
    End Function

#End Region

End Class

#End Region