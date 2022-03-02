Imports System.Collections.Generic
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)>
Public Class StockSelector

    Public Shared Function Allocate(oPlanStrategy As PlanStrategy,
                                    Stock As VPlannerInventoryDataTable,
                                    OnHandStock As VAllocationOnHandDataTable,
                                    reqs As PickingRequirments,
                                    pStratLineNPPVol As Decimal,
                                    pZPicking As Boolean,
                                    ByRef pStrategyAllocationSequence As Int32,
                                    PickType As String,
                                    AllocFullRequirment As Boolean,
                                    AllocByStratLineUOM As Boolean,
                                    AllocByHighestUOM As Boolean,
                                    Optional UOM As String = Nothing,
                                    Optional StrategyLineFilter As String = "",
                                    Optional AllocationUOMSize As String = "",
                                    Optional oLogger As LogHandler = Nothing,
                                    Optional pSubstitueSkuData As SKU.SkuSubstitutes = Nothing,
                                    Optional pFullPickAllocation As String = "ALWAYS") As AllocationCollection
        If PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK And reqs.RequirementsVolume < pStratLineNPPVol Then
            Return Nothing
        End If

        Dim alloc, tempalloc As AllocationCollection
        Dim sku As SKU = Planner.GetSKU(reqs.Consignee, reqs.Sku)
        Return Allocate(sku, oPlanStrategy, Stock, OnHandStock, reqs, pStratLineNPPVol,
                        pZPicking, pStrategyAllocationSequence, PickType, AllocFullRequirment,
                        AllocByStratLineUOM, AllocByHighestUOM, UOM, StrategyLineFilter,
                        AllocationUOMSize, oLogger, pSubstitueSkuData, pFullPickAllocation)
    End Function

    Private Shared Function Allocate(oSku As SKU,
                                     oPlanStrategy As PlanStrategy,
                                     Stock As VPlannerInventoryDataTable,
                                     OnHandStock As VAllocationOnHandDataTable,
                                     reqs As PickingRequirments,
                                     pStratLineNPPVol As Decimal,
                                     pZPicking As Boolean,
                                     ByRef pStrategyAllocationSequence As Int32,
                                     PickType As String,
                                     AllocFullRequirment As Boolean,
                                     AllocByStratLineUOM As Boolean,
                                     AllocByHighestUOM As Boolean,
                                     Optional UOM As String = Nothing,
                                     Optional StrategyLineFilter As String = "",
                                     Optional AllocationUOMSize As String = "",
                                     Optional oLogger As LogHandler = Nothing,
                                     Optional pSubstitueSkuData As SKU.SkuSubstitutes = Nothing,
                                     Optional pFullPickAllocation As String = "ALWAYS") As AllocationCollection
        Dim unitsFullfilled As Double = reqs.UnitsFullfilled
        Dim alloc As AllocationCollection
        Dim tempallocquantity As AllocationCollection
        Dim allocquantity As New AllocationCollection

        Select Case PickType
            Case WMS.Lib.PICKTYPE.FULLPICK
                If Not pSubstitueSkuData Is Nothing Then
                    If Not canUseSubSKU(reqs, pSubstitueSkuData) Then
                        Return Nothing
                    End If
                End If

                alloc = AllocFull(oPlanStrategy.Scoring, Stock, OnHandStock, reqs, pZPicking,
                                  pStrategyAllocationSequence, oSku, unitsFullfilled, UOM,
                                  StrategyLineFilter, AllocationUOMSize, oLogger, pFullPickAllocation)

            Case WMS.Lib.PICKTYPE.PARTIALPICK, WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK, WMS.Lib.PICKTYPE.PARALLELPICK
                For Each oPlanRequirement As Requirment In reqs
                    If Not canUseSubSKU(oPlanRequirement, pSubstitueSkuData) Then Continue For

                    tempallocquantity = AllocQty(PickType, pZPicking, pStrategyAllocationSequence,
                                                 oPlanStrategy.Scoring, Stock, OnHandStock, oPlanRequirement,
                                                 oSku, unitsFullfilled, UOM, StrategyLineFilter, AllocFullRequirment,
                                                 AllocByStratLineUOM, oPlanStrategy.PickPartialUom,
                                                 AllocByHighestUOM, pStratLineNPPVol, AllocationUOMSize, oLogger)

                    If Not tempallocquantity Is Nothing Then
                        allocquantity.AddRange(tempallocquantity)
                    End If
                Next
            Case Else
                alloc = AllocFull(oPlanStrategy.Scoring, Stock, OnHandStock, reqs, pZPicking, pStrategyAllocationSequence, oSku, unitsFullfilled, UOM, StrategyLineFilter, AllocationUOMSize, oLogger)
                For Each oPlanRequirement As Requirment In reqs
                    tempallocquantity = AllocQty(PickType, pZPicking, pStrategyAllocationSequence, oPlanStrategy.Scoring, Stock, OnHandStock, oPlanRequirement, oSku, unitsFullfilled, UOM, StrategyLineFilter, AllocFullRequirment, AllocByStratLineUOM, oPlanStrategy.PickPartialUom, AllocByHighestUOM, pStratLineNPPVol, AllocationUOMSize, oLogger)
                    If Not tempallocquantity Is Nothing Then
                        allocquantity.AddRange(tempallocquantity)
                    End If
                Next
        End Select

        If alloc Is Nothing And allocquantity.Count = 0 Then Return Nothing
        If alloc Is Nothing And allocquantity.Count > 0 Then Return allocquantity
        If Not alloc Is Nothing And allocquantity.Count = 0 Then Return alloc

        alloc.AddRange(allocquantity)
        Return alloc
    End Function

    Private Shared Function canUseSubSKU(ByVal pPickReqs As PickingRequirments, ByVal pSubSKUData As SKU.SkuSubstitutes) As Boolean
        For Each req As Requirment In pPickReqs
            If Not canUseSubSKU(req, pSubSKUData) Then Return False
        Next
        Return True
    End Function

    Private Shared Function canUseSubSKU(ByVal pReq As Requirment, ByVal pSubSKUData As SKU.SkuSubstitutes) As Boolean
        If pSubSKUData Is Nothing Then Return True

        If Not String.IsNullOrEmpty(pSubSKUData.COMPANY) Then
            If Not pReq.Company.Equals(pSubSKUData.COMPANY, StringComparison.OrdinalIgnoreCase) OrElse
                Not pReq.CompanyType.Equals(pSubSKUData.COMPANYTYPE, StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
        End If

        Return True
    End Function

    Public Shared Function PickLocAllocate(ByVal pOverAllocationAllowed As Boolean,
                                           ByVal oPlanStrategy As PlanStrategy,
                                           ByVal OnHandStock As VAllocationOnHandDataTable,
                                           ByVal reqs As PickingRequirments,
                                           ByVal pStratLineNPPVol As Decimal,
                                           ByVal pZPicking As Boolean,
                                           ByRef pStrategyAllocationSequence As Int32,
                                           ByVal PickType As String,
                                           ByVal AllocFullRequirment As Boolean,
                                           ByVal AllocByStratLineUOM As Boolean,
                                           ByVal AllocByHighestUOM As Boolean,
                                           Optional ByVal UOM As String = Nothing,
                                           Optional ByVal StrategyLineFilter As String = "",
                                           Optional ByVal AllocationUOMSize As String = "",
                                           Optional ByVal oLogger As LogHandler = Nothing,
                                           Optional ByVal pAllocateSubstituesSku As Boolean = False) As AllocationCollection
        If PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK And reqs.RequirementsVolume < pStratLineNPPVol Then
            Return Nothing
        End If
        Dim oSku As SKU
        Dim alloc, tempalloc As AllocationCollection
        oSku = Planner.GetSKU(reqs.Consignee, reqs.Sku)
        Dim unitsFullfilled As Double = reqs.UnitsFullfilled
        For Each oPlanRequirement As Requirment In reqs
            tempalloc = PickLocAllocQty(pOverAllocationAllowed, PickType, pZPicking, pStrategyAllocationSequence, OnHandStock, oPlanRequirement, oSku, unitsFullfilled, UOM, StrategyLineFilter, AllocFullRequirment, AllocByStratLineUOM, oPlanStrategy.PickPartialUom, AllocByHighestUOM, pStratLineNPPVol, AllocationUOMSize, oLogger)
            If Not tempalloc Is Nothing Then
                If alloc Is Nothing Then alloc = New AllocationCollection
                alloc.AddRange(tempalloc)
            End If
        Next
        Return alloc
    End Function

#Region "Alocate Direct Pick"

    Protected Shared Function AllocFull(ByVal Scoring As PlanStrategyScoring,
                                        ByVal Stock As VPlannerInventoryDataTable,
                                        ByVal OnHandStock As VAllocationOnHandDataTable,
                                        ByVal reqs As PickingRequirments,
                                        ByVal pZPicking As Boolean,
                                        ByRef pStrategyAllocationSequence As Int32,
                                        ByVal sku As SKU, ByRef unitsFullfilled As Double,
                                        ByVal UOM As String,
                                        ByVal StrategyLineFilter As String,
                                        Optional ByVal AllocationUOMSize As String = "",
                                        Optional ByVal oLogger As LogHandler = Nothing,
                                        Optional ByVal pFullPickAllocation As String = "ALWAYS") As AllocationCollection
        Dim ac As New AllocationCollection
        Dim dr As DataRow
        Dim selrows As DataRow()
        Dim sql, pr As String
        Dim FullUomSize As Decimal
        While True
            Dim islast As Boolean = True
            sql = CreateWhereClause(sku, reqs.InventoryStatus, reqs.Attributes, WMS.Lib.PICKTYPE.FULLPICK, 0)
            If AllocationUOMSize <> String.Empty Then
                FullUomSize = sku.ConvertToUnits(AllocationUOMSize)
                sql = sql & String.Format(" and HUNumLoads<=1 and UNITSALLOCATED=0 and UNITS = {0} and units = {1} {2}", reqs.UnitsRequired - unitsFullfilled, FullUomSize, StrategyLineFilter)
            Else
                sql = sql & String.Format(" and HUNumLoads<=1 and UNITSALLOCATED=0 and UNITS <= {0} {1}", reqs.UnitsRequired - unitsFullfilled, StrategyLineFilter)
            End If
            'Start RWMS-768
            If Not oLogger Is Nothing Then
                oLogger.Write("AllocFull SQL for Selecting the Loads: SELECT * FROM vPlannerInventory WHERE " & sql)
                oLogger.writeSeperator(" ", 20)
            End If
            'End  RWMS-768

            selrows = Stock.GetRowsForConsigneeSkuStatus(sku.CONSIGNEE, sku.SKU, reqs.InventoryStatus, sql)
            If Not Scoring Is Nothing Then Scoring.Score(selrows, oLogger) 'Score Loads
            If selrows.Length > 0 Then
                For Each dr In selrows
                    If Not allowedForFullPick(dr, pFullPickAllocation) Then
                        Continue For
                    End If
                    Dim recUom As String = dr("LOADUOM")
                    Dim load As New Load(Convert.ToString(dr("LoadID")))
                    Dim LoadUpdate As Boolean = load.SetConditionalActivityStatus([Lib].Statuses.ActivityStatus.ALLOCPENDING, [Lib].USERS.SYSTEMUSER, [Lib].Statuses.ActivityStatus.NONE, oLogger)

                    If Not LoadUpdate Then
                        Continue For
                    End If

                    If Not String.IsNullOrEmpty(UOM) And Not (recUom = UOM Or sku.isChildOf(recUom, UOM)) Then
                        Continue For
                    End If

                    ac.Add(getAllocation(Nothing, dr, dr("UNITS"), recUom, 0, unitsFullfilled, WMS.Lib.PICKTYPE.FULLPICK, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                    islast = False
                    Exit For

                Next
                If islast Then
                    Exit While
                End If
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("NO MATCHING LOADS FOUND.")
                    oLogger.writeSeperator(" ", 20)
                End If
                Exit While
            End If
        End While
        If ac.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING ALLOCATIONS FOUND.")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        Else
            Return ac
        End If
    End Function

    Private Shared Function allowedForFullPick(ByVal pLoadDataRow As DataRow, ByVal pFullPickAllocation As String) As Boolean
        If String.IsNullOrEmpty(pFullPickAllocation) OrElse pFullPickAllocation.Equals(WMS.Lib.FULLPICKALLOCATION.ALWAYS) Then Return True

        If pFullPickAllocation.Equals(WMS.Lib.FULLPICKALLOCATION.BYSKU) Then
            Dim sku As WMS.Logic.SKU = Planner.GetSKU(pLoadDataRow("CONSIGNEE"), pLoadDataRow("SKU"))
            If sku Is Nothing Then
                sku = New WMS.Logic.SKU(pLoadDataRow("CONSIGNEE"), pLoadDataRow("SKU"))
            End If
            If sku.FULLPICKALLOCATION = 0 Then Return True
            Dim palletUom As SKU.SKUUOM = sku.UOM("PALLET")
            If palletUom Is Nothing Then Return True

            Dim minQty As Decimal = palletUom.UNITSPERLOWESTUOM * sku.FULLPICKALLOCATION / 100
            If minQty <= pLoadDataRow("UNITS") Then Return True
        End If

        Return False
    End Function

#End Region

#Region "Allocate Partial Pick"

    Protected Shared Function AllocQty(ByVal pPickType As String,
                                       ByVal pZPicking As Boolean,
                                       ByRef pStrategyAllocationSequence As Int32,
                                       ByVal Scoring As PlanStrategyScoring,
                                       ByVal Stock As VPlannerInventoryDataTable,
                                       ByVal OnHandStock As DataTable,
                                       ByVal reqs As Requirment,
                                       ByVal sku As SKU,
                                       ByRef unitsFullfilled As Double,
                                       ByVal UOM As String,
                                       ByVal StrategyLineFilter As String,
                                       ByVal AllocFullRequirment As Boolean,
                                       ByVal AllocStratLineUOM As Boolean,
                                       ByVal pPickPartialUOM As Boolean,
                                       ByVal pAllocByHighestUOM As Boolean,
                                       ByVal pStratLineNPPVol As Decimal,
                                       Optional ByVal AllocationUOMSize As String = "",
                                       Optional ByVal oLogger As LogHandler = Nothing) As AllocationCollection
        Dim ac As New AllocationCollection
        Dim dr As DataRow
        Dim selrows As DataRow()
        Dim checkUOM As String = ""
        Dim unitsPerUom As Double = 0, UomAvailable As Double = 0
        Dim QtyLeftToFullFill As Double = reqs.QuantityLeftToFullfill - reqs.QuantityFullfilled 'unitsFullfilled
        Dim FullUomSize As Decimal
        'Dim sql As String = String.Format(CreateWhereClause(reqs, pPickType, pStratLineNPPVol) & " {0}", StrategyLineFilter)
        Dim sql As String = String.Format(CreateWhereClause(sku, reqs.InventoryStatus, reqs.Attributes, pPickType, pStratLineNPPVol) & " {0}", StrategyLineFilter)
        If AllocationUOMSize <> String.Empty Then
            FullUomSize = sku.ConvertToUnits(AllocationUOMSize)
            sql = sql & String.Format(" and units - unitsallocated >= {0}", FullUomSize)
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("SQL for Selecting the Loads: " & sql)
        End If
        Try
            selrows = Stock.GetRowsForConsigneeSkuStatus(sku.CONSIGNEE, sku.SKU, reqs.InventoryStatus, sql)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.WriteException(ex)
            End If
        End Try

        If Not Scoring Is Nothing Then Scoring.Score(selrows, oLogger)
        'this flag used to determine if the load has sufficient qty for the current line
        Dim CanUseLoad As Boolean = True
        If (selrows.Length > 0) Then
            For Each dr In selrows
                If Not AllowedLoad(reqs, dr("loadid")) Then
                    Continue For
                End If
                Dim availableUnits As Double = dr("units") - dr("unitsallocated")
                'check if we should allocate line qty from one load
                If AllocFullRequirment Then
                    If QtyLeftToFullFill > availableUnits Then
                        CanUseLoad = False
                    Else
                        CanUseLoad = True
                    End If
                End If
                If (availableUnits > 0) And CanUseLoad Then
                    If (Not UOM Is Nothing And Not UOM = "") Then
                        checkUOM = UOM
                        Dim UomQtyLeftToFullfill As Double
                        'should we get a child uom as well or we should take the specific uom
                        Dim UOMCond As Boolean = False
                        If AllocStratLineUOM Then
                            If checkUOM = dr("LOADUOM") Then UOMCond = True
                        Else
                            If checkUOM = dr("LOADUOM") Or sku.isChildOf(dr("LOADUOM"), checkUOM) Then UOMCond = True
                        End If
                        If (UOMCond) Then
                            unitsPerUom = sku.ConvertToUnits(checkUOM)
                            UomQtyLeftToFullfill = QtyLeftToFullFill / unitsPerUom
                            If unitsPerUom > QtyLeftToFullFill Then
                                If Not pPickPartialUOM Then
                                    UomAvailable = 0
                                Else
                                    UomAvailable = QtyLeftToFullFill
                                End If
                            Else
                                If Not pPickPartialUOM Then
                                    UomAvailable = Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                Else
                                    UomAvailable = availableUnits / unitsPerUom 'Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                End If
                            End If
                            If UomAvailable > QtyLeftToFullFill Then
                                UomAvailable = QtyLeftToFullFill
                            End If
                            If UomAvailable > 0 Then
                                If UomAvailable > UomQtyLeftToFullfill Then
                                    If Not pPickPartialUOM Then
                                        UomAvailable = Convert.ToInt32(Decimal.Floor(UomQtyLeftToFullfill))
                                    Else
                                        UomAvailable = UomQtyLeftToFullfill
                                    End If
                                End If
                                If UomAvailable > FullUomSize And AllocationUOMSize <> "" Then
                                    UomAvailable = FullUomSize
                                ElseIf UomAvailable < FullUomSize And AllocationUOMSize <> "" Then
                                    UomAvailable = 0
                                End If
                                While (True)
                                    If Not pPickPartialUOM Then
                                        If Not ContainsFractions(UomAvailable) Then
                                            UomAvailable = UomAvailable * unitsPerUom
                                            QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                                            ac.Add(getAllocation(reqs, dr, UomAvailable, checkUOM, unitsPerUom, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                                            availableUnits = availableUnits - UomAvailable
                                        ElseIf pAllocByHighestUOM Then
                                            UomAvailable = Convert.ToInt32(Math.Floor(UomAvailable))
                                            UomAvailable = UomAvailable * unitsPerUom
                                            QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                                            ac.Add(getAllocation(reqs, dr, UomAvailable, checkUOM, unitsPerUom, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                                            availableUnits = availableUnits - UomAvailable
                                        Else
                                            Exit While  'this uom is not valid if not picking with fractions...
                                        End If
                                    Else
                                        UomAvailable = UomAvailable * unitsPerUom
                                        QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                                        ac.Add(getAllocation(reqs, dr, UomAvailable, checkUOM, unitsPerUom, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                                        availableUnits = availableUnits - UomAvailable
                                    End If
                                    If QtyLeftToFullFill = 0 Or QtyLeftToFullFill < unitsPerUom Then
                                        Exit While
                                    ElseIf AllocationUOMSize <> "" AndAlso QtyLeftToFullFill < FullUomSize Then
                                        Exit While
                                    ElseIf availableUnits < unitsPerUom And Not pPickPartialUOM Then
                                        Exit While
                                    End If
                                    If ((AllocationUOMSize <> "" And availableUnits < FullUomSize) Or availableUnits = 0) Then
                                        Exit While
                                    End If
                                    If AllocationUOMSize = "" Then
                                        Exit While
                                    End If
                                    If availableUnits < QtyLeftToFullFill Then
                                        Exit While
                                    End If
                                End While
                            End If
                        End If
                    Else
                        checkUOM = dr("LOADUOM")
                        While (True)
                            unitsPerUom = sku.ConvertToUnits(checkUOM)
                            Dim UomQtyLeftToFullfill As Double
                            UomQtyLeftToFullfill = QtyLeftToFullFill / unitsPerUom 'Convert.ToInt32(Decimal.Floor(QtyLeftToFullFill / unitsPerUom))
                            If unitsPerUom > QtyLeftToFullFill Then
                                If Not pPickPartialUOM Then
                                    UomAvailable = 0
                                Else
                                    UomAvailable = QtyLeftToFullFill
                                End If
                            Else
                                If Not pPickPartialUOM Then
                                    UomAvailable = Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                Else
                                    UomAvailable = availableUnits / unitsPerUom 'Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                End If
                            End If
                            If UomAvailable > QtyLeftToFullFill Then
                                UomAvailable = QtyLeftToFullFill
                            End If
                            If (UomAvailable > 0) Then
                                If UomAvailable > UomQtyLeftToFullfill Then
                                    If Not pPickPartialUOM Then
                                        UomAvailable = Convert.ToInt32(Decimal.Floor(UomQtyLeftToFullfill))
                                    Else
                                        UomAvailable = UomQtyLeftToFullfill
                                    End If
                                End If
                                If UomAvailable > FullUomSize And AllocationUOMSize <> "" Then
                                    UomAvailable = FullUomSize
                                ElseIf UomAvailable < FullUomSize And AllocationUOMSize <> "" Then
                                    Exit While
                                End If
                                While (True)
                                    If Not pPickPartialUOM Then
                                        If Not ContainsFractions(UomAvailable) Then
                                            UomAvailable = UomAvailable * unitsPerUom
                                            ac.Add(getAllocation(reqs, dr, UomAvailable, checkUOM, unitsPerUom, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                                            QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                                            availableUnits = availableUnits - UomAvailable
                                        ElseIf pAllocByHighestUOM Then
                                            UomAvailable = Convert.ToInt32(Math.Floor(UomAvailable))
                                            UomAvailable = UomAvailable * unitsPerUom
                                            QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                                            ac.Add(getAllocation(reqs, dr, UomAvailable, checkUOM, unitsPerUom, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                                            availableUnits = availableUnits - UomAvailable
                                        Else
                                            Exit While  'this uom is not valid if not picking with fractions...
                                        End If
                                    Else
                                        UomAvailable = UomAvailable * unitsPerUom
                                        QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                                        ac.Add(getAllocation(reqs, dr, UomAvailable, checkUOM, unitsPerUom, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                                        availableUnits = availableUnits - UomAvailable
                                    End If
                                    If QtyLeftToFullFill = 0 Or QtyLeftToFullFill < unitsPerUom Then
                                        Exit While
                                    ElseIf AllocationUOMSize <> "" AndAlso QtyLeftToFullFill < FullUomSize Then
                                        Exit While
                                    ElseIf availableUnits < unitsPerUom And Not pPickPartialUOM Then
                                        Exit While
                                    End If
                                    If ((AllocationUOMSize <> "" And availableUnits < FullUomSize) Or availableUnits = 0) Then
                                        Exit While
                                    End If
                                    If AllocationUOMSize = "" Then
                                        Exit While
                                    End If
                                    If availableUnits < QtyLeftToFullFill Then
                                        Exit While
                                    End If
                                End While
                            End If
                            If QtyLeftToFullFill = 0 Or (AllocationUOMSize <> "" And QtyLeftToFullFill < FullUomSize) Then
                                Exit While
                            End If
                            If (availableUnits = 0) Or (AllocationUOMSize <> "" And QtyLeftToFullFill < FullUomSize) Then
                                Exit While
                            End If
                            checkUOM = sku.getNextUom(checkUOM)
                            If (checkUOM Is Nothing) Then
                                Exit While
                            End If
                        End While
                    End If
                    If QtyLeftToFullFill = 0 Then
                        Exit For
                    End If
                End If
            Next
            If ac.Count = 0 Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("NO MATCHING ALLOCATIONS FOUND.")
                    oLogger.writeSeperator(" ", 20)
                End If
            End If
            Return ac
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND.")
                oLogger.writeSeperator(" ", 20)
            End If
        End If
    End Function

    Private Shared Function ContainsFractions(ByVal pnum As Decimal) As Boolean
        Dim tmpNum As Int32 = Convert.ToInt32(pnum)
        Return Not tmpNum = pnum
    End Function

#End Region

#Region "Picking Location Allocation"

    Protected Shared Function PickLocAllocQty(ByVal pOverAllocationAllowed As Boolean, ByVal pPickType As String, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByVal OnHandStock As VAllocationOnHandDataTable, ByVal reqs As Requirment, ByVal sku As SKU, ByRef unitsFullfilled As Double, ByVal UOM As String, ByVal StrategyLineFilter As String, ByVal AllocFullRequirment As Boolean, ByVal AllocStratLineUOM As Boolean, ByVal pPickPartialUOM As Boolean, ByVal pAllocByHighestUOM As Boolean, ByVal pStratLineNPPVol As Decimal, Optional ByVal AllocationUOMSize As String = "", Optional ByVal oLogger As LogHandler = Nothing) As AllocationCollection
        Dim ac As New AllocationCollection
        Dim dr As DataRow
        Dim selrows As DataRow()
        Dim checkUOM As String = ""
        Dim unitsPerUom As Double = 0, UomAvailable As Double = 0
        Dim QtyLeftToFullFill As Double = reqs.QuantityLeftToFullfill - reqs.QuantityFullfilled 'unitsFullfilled
        Dim FullUomSize As Decimal = -1
        If AllocationUOMSize <> String.Empty Then
            FullUomSize = sku.ConvertToUnits(AllocationUOMSize)
        End If
        selrows = GetMatchPickingLocation(OnHandStock, pOverAllocationAllowed, StrategyLineFilter, sku.CONSIGNEE, reqs.OrderId, sku.SKU, FullUomSize, oLogger)
        'this flag used to determine if the load has sufficient qty for the current line
        Dim CanUseLoad As Boolean = True
        If (selrows.Length > 0) Then
            For Each dr In selrows
                Dim availableUnits As Double = dr("availableunits")
                'check if we should allocate line qty from one load
                If AllocFullRequirment Then
                    If QtyLeftToFullFill > availableUnits Then
                        CanUseLoad = False
                    Else
                        CanUseLoad = True
                    End If
                End If
                If (availableUnits > 0) And CanUseLoad Then
                    If (Not UOM Is Nothing And Not UOM = "") Then
                        checkUOM = UOM
                        Dim UomQtyLeftToFullfill As Double
                        Dim UOMCond As Boolean = True
                        If sku.UNITSOFMEASURE.UOM(checkUOM) Is Nothing Then
                            UOMCond = False
                        End If
                        If (UOMCond) Then
                            unitsPerUom = sku.ConvertToUnits(checkUOM)
                            UomQtyLeftToFullfill = QtyLeftToFullFill / unitsPerUom
                            If unitsPerUom > QtyLeftToFullFill Then
                                If Not pPickPartialUOM Then
                                    UomAvailable = 0
                                Else
                                    UomAvailable = QtyLeftToFullFill
                                End If
                            Else
                                If Not pPickPartialUOM Then
                                    UomAvailable = Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                Else
                                    UomAvailable = availableUnits / unitsPerUom 'Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                End If
                            End If
                            If UomAvailable > QtyLeftToFullFill Then
                                UomAvailable = QtyLeftToFullFill
                            End If
                            If UomAvailable > 0 Then
                                If UomAvailable > UomQtyLeftToFullfill Then
                                    If Not pPickPartialUOM Then
                                        UomAvailable = Convert.ToInt32(Decimal.Floor(UomQtyLeftToFullfill))
                                    Else
                                        UomAvailable = UomQtyLeftToFullfill
                                    End If
                                End If
                                If UomAvailable > FullUomSize And AllocationUOMSize <> "" Then
                                    UomAvailable = FullUomSize
                                ElseIf UomAvailable < FullUomSize And AllocationUOMSize <> "" Then
                                    UomAvailable = 0
                                End If
                                PickLocAllocQty(dr, ac, availableUnits, UomAvailable, unitsPerUom, QtyLeftToFullFill, FullUomSize, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, reqs, sku, unitsFullfilled, checkUOM, pPickPartialUOM, pAllocByHighestUOM, AllocationUOMSize, oLogger)
                            End If
                        End If
                    Else
                        checkUOM = sku.LOWESTUOM
                        While (True)
                            unitsPerUom = sku.ConvertToUnits(checkUOM)
                            Dim UomQtyLeftToFullfill As Double
                            UomQtyLeftToFullfill = QtyLeftToFullFill / unitsPerUom 'Convert.ToInt32(Decimal.Floor(QtyLeftToFullFill / unitsPerUom))
                            If unitsPerUom > QtyLeftToFullFill Then
                                If Not pPickPartialUOM Then
                                    UomAvailable = 0
                                Else
                                    UomAvailable = QtyLeftToFullFill
                                End If
                            Else
                                If Not pPickPartialUOM Then
                                    UomAvailable = Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                Else
                                    UomAvailable = availableUnits / unitsPerUom 'Convert.ToInt32(Decimal.Floor(availableUnits / unitsPerUom))
                                End If
                            End If
                            If UomAvailable > QtyLeftToFullFill Then
                                UomAvailable = QtyLeftToFullFill
                            End If
                            If (UomAvailable > 0) Then
                                If UomAvailable > UomQtyLeftToFullfill Then
                                    If Not pPickPartialUOM Then
                                        UomAvailable = Convert.ToInt32(Decimal.Floor(UomQtyLeftToFullfill))
                                    Else
                                        UomAvailable = UomQtyLeftToFullfill
                                    End If
                                End If
                                If UomAvailable > FullUomSize And AllocationUOMSize <> "" Then
                                    UomAvailable = FullUomSize
                                ElseIf UomAvailable < FullUomSize And AllocationUOMSize <> "" Then
                                    Exit While
                                End If
                                PickLocAllocQty(dr, ac, availableUnits, UomAvailable, unitsPerUom, QtyLeftToFullFill, FullUomSize, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, reqs, sku, unitsFullfilled, checkUOM, pPickPartialUOM, pAllocByHighestUOM, AllocationUOMSize, oLogger)
                            End If
                            If QtyLeftToFullFill = 0 Or (AllocationUOMSize <> "" And QtyLeftToFullFill < FullUomSize) Then
                                Exit While
                            End If
                            If (availableUnits = 0) Or (AllocationUOMSize <> "" And QtyLeftToFullFill < FullUomSize) Then
                                Exit While
                            End If
                            checkUOM = sku.getNextUom(checkUOM)
                            If (checkUOM Is Nothing) Then
                                Exit While
                            End If
                        End While
                    End If
                    If QtyLeftToFullFill = 0 Then
                        Exit For
                    End If
                End If
            Next
            If ac.Count = 0 Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("NO MATCHING ALLOCATIONS FOUND FOR PICKLOCALLOCATIONS.")
                    oLogger.writeSeperator(" ", 20)
                End If
            End If
            Return ac
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING ROWS FOUND FOUND FOR PICKLOCALLOCATIONS.")
                oLogger.writeSeperator(" ", 20)
            End If
        End If
    End Function

    Protected Shared Function PickLocAllocQty(ByVal dr As DataRow, ByRef ac As AllocationCollection, ByRef availableUnits As Double, ByRef UomAvailable As Double, ByRef unitsPerUom As Double, ByRef QtyLeftToFullFill As Double, ByVal FullUomSize As Double, ByVal pPickType As String, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByRef OnHandStock As VAllocationOnHandDataTable, ByRef reqs As Requirment, ByVal sku As SKU, ByRef unitsFullfilled As Double, ByVal checkUOM As String, ByVal pPickPartialUOM As Boolean, ByVal pAllocByHighestUOM As Boolean, ByVal AllocationUOMSize As String, Optional ByVal oLogger As LogHandler = Nothing) As AllocationCollection
        While (True)
            If Not pPickPartialUOM Then
                If Not ContainsFractions(UomAvailable) Then
                    UomAvailable = UomAvailable * unitsPerUom
                    ac.Add(getPickLocAllocation(dr, UomAvailable, checkUOM, unitsPerUom, reqs, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                    QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                    availableUnits = availableUnits - UomAvailable
                ElseIf pAllocByHighestUOM Then
                    UomAvailable = Convert.ToInt32(Math.Floor(UomAvailable))
                    UomAvailable = UomAvailable * unitsPerUom
                    QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                    ac.Add(getPickLocAllocation(dr, UomAvailable, checkUOM, unitsPerUom, reqs, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                    availableUnits = availableUnits - UomAvailable
                Else
                    Exit While  'this uom is not valid if not picking with fractions...
                End If
            Else
                UomAvailable = UomAvailable * unitsPerUom
                QtyLeftToFullFill = QtyLeftToFullFill - UomAvailable
                ac.Add(getPickLocAllocation(dr, UomAvailable, checkUOM, unitsPerUom, reqs, unitsFullfilled, pPickType, pZPicking, pStrategyAllocationSequence, OnHandStock, FullUomSize, oLogger))
                availableUnits = availableUnits - UomAvailable
            End If
            If QtyLeftToFullFill = 0 Or QtyLeftToFullFill < unitsPerUom Then
                Exit While
            ElseIf AllocationUOMSize <> "" AndAlso QtyLeftToFullFill < FullUomSize Then
                Exit While
            End If
            If ((AllocationUOMSize <> "" And availableUnits < FullUomSize) Or availableUnits = 0) Then
                Exit While
            End If
            If AllocationUOMSize = "" Then
                Exit While
            End If
        End While
    End Function

#End Region

#Region "Create Allocation"

    Protected Shared Function getAllocation(ByVal reqs As Requirment, ByVal load As DataRow, ByVal Units As Double, ByVal UOM As String, ByVal UnitsPerMeasure As Double, ByRef unitsFullfilled As Double, ByVal PickType As String, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByVal OnHandStock As VAllocationOnHandDataTable, ByVal pAllocationQuanta As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Allocate
        Dim alloc As New Allocate
        If pZPicking Then
            alloc.Location = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(load("ZPICKINGLOCATION"))
        Else
            alloc.Location = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(load("LOCATION"))
        End If
        'If the Location is picking location the allocate without load id
        If Planner.IsPickingLocation(alloc.Location, load("consignee"), load("sku")) Then
            alloc.LoadId = ""
            alloc.IsPickLocAllocation = True
        Else
            alloc.LoadId = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(load("LOADID"))
            alloc.IsPickLocAllocation = False
        End If
        alloc.PickRegion = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(load("PICKREGION"))
        alloc.PickType = PickType
        alloc.SortOrder = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(load("LOCSORTORDER")) & pStrategyAllocationSequence.ToString.PadLeft(7, "0")
        alloc.Units = Units
        alloc.WarehouseArea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(load("WAREHOUSEAREA"))
        alloc.UnitsPerMeasure = UnitsPerMeasure
        alloc.AllocationQuanta = pAllocationQuanta
        If Not reqs Is Nothing Then
            'reqs.QuantityLeftToFullfill -= Units
            reqs.QuantityFullfilled += Units
        End If
        If UnitsPerMeasure > 0 Then
            alloc.UOMUnits = Units / UnitsPerMeasure
        End If
        alloc.UOM = UOM
        load("UNITSALLOCATED") = load("UNITSALLOCATED") + Units
        unitsFullfilled = unitsFullfilled + Units
        If PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
            load("ACTIVITYSTATUS") = WMS.Lib.Statuses.ActivityStatus.PICKPEND
        End If
        'Update the total stock view
        UpdateOnHandStockView(OnHandStock, load("CONSIGNEE"), load("SKU"), Units)
        'Update the picking location view if it is a pick location...
        If Planner.IsPickingLocation(alloc.Location, load("consignee"), load("sku")) Then
            UpdatePickLocAllocQty(alloc.Location, load("UNITSALLOCATED"), oLogger)
        End If
        If Not oLogger Is Nothing Then
            'Start RWMS-768
            If (alloc.IsPickLocAllocation = True) Then
                oLogger.Write("Allocating by PickLocation : " & alloc.Location & ", " & Units & " units")
            Else
                oLogger.Write("Allocating by load : " & alloc.LoadId & ", " & Units & " units")
            End If
            'End  RWMS-768
        End If
        Return alloc
    End Function

    Protected Shared Function getPickLocAllocation(ByVal locRow As DataRow, ByVal Units As Double, ByVal UOM As String, ByVal UnitsPerMeasure As Double, ByVal req As Requirment, ByRef unitsFullfilled As Double, ByVal PickType As String, ByVal pZPicking As Boolean, ByRef pStrategyAllocationSequence As Int32, ByVal OnHandStock As DataTable, ByVal pAllocationQuanta As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Allocate
        Dim alloc As New Allocate
        alloc.IsPickLocAllocation = True
        alloc.IsOverAllocation = True
        If pZPicking Then
            alloc.Location = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(locRow("ZPICKINGLOCATION"))
        Else
            alloc.Location = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(locRow("LOCATION"))
        End If
        alloc.LoadId = ""
        alloc.PickRegion = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(locRow("PICKREGION"))
        alloc.PickType = PickType
        alloc.SortOrder = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(locRow("LOCSORTORDER")) & pStrategyAllocationSequence.ToString.PadLeft(7, "0")
        alloc.Units = Units
        If Not req Is Nothing Then
            'req.QuantityLeftToFullfill -= Units
            req.QuantityFullfilled += Units
        End If
        alloc.WarehouseArea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(locRow("WAREHOUSEAREA"))
        alloc.UnitsPerMeasure = UnitsPerMeasure
        If UnitsPerMeasure > 0 Then
            alloc.UOMUnits = Units / UnitsPerMeasure
        End If
        alloc.UOM = UOM
        alloc.AllocationQuanta = pAllocationQuanta
        unitsFullfilled = unitsFullfilled + Units
        UpdateOnHandStockView(OnHandStock, locRow("CONSIGNEE"), locRow("SKU"), Units)
        If Not oLogger Is Nothing Then
            oLogger.Write("Over Allocating Pick Location : " & alloc.Location & ", " & Units & " units")
        End If
        Return alloc
    End Function

#End Region

#Region "Misc"

    Protected Shared Function CreateWhereClause(ByVal pSKU As WMS.Logic.SKU, ByVal pInventoryStatus As String, ByVal pAttributeCollection As WMS.Logic.AttributesCollection, ByVal pPickType As String, ByVal VolumeTreshHold As Decimal) As String
        Dim sql As String = String.Format("CONSIGNEE='{0}' and SKU='{1}' and STATUS='{2}' and (activitystatus = '' or activitystatus is null)", pSKU.CONSIGNEE, pSKU.SKU, pInventoryStatus)
        For idx As Int32 = 0 To pAttributeCollection.Count - 1
            If TypeOf pAttributeCollection(idx) Is System.Boolean Then
                sql = sql & " and " & pAttributeCollection.Keys(idx) & "=" & pAttributeCollection(pAttributeCollection.Keys(idx))
            Else
                sql = sql & " and " & pAttributeCollection.Keys(idx) & "=" & Made4Net.Shared.Util.FormatField(pAttributeCollection(pAttributeCollection.Keys(idx)))
            End If
        Next
        If pPickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
            sql = sql & String.Format(" and loadsvolume >= {0} and UNITSALLOCATED=0", VolumeTreshHold)
        End If
        Return sql
    End Function


    Private Shared Function AllowedLoad(ByVal reqs As Requirment, ByVal pLoadid As String)
        Dim sLoadsFilter As String = String.Format(" consignee = '{0}' and orderid = '{1}' and loadid = '{2}'", reqs.Consignee, reqs.OrderId, pLoadid)
        If Planner.RequirmentsAllowedStock.Select(sLoadsFilter).Length > 0 Then
            Return True
        End If
        Return False
    End Function

    Private Shared Sub UpdateOnHandStockView(ByRef OnHandStock As VAllocationOnHandDataTable, ByVal pConsignee As String, ByVal pSku As String, ByVal pUnitsAllocated As Decimal)
        Dim selrows As DataRow()
        selrows = OnHandStock.GetRowsForConsigneeSku(pConsignee, pSku, String.Empty)
        If selrows.Length >= 1 Then
            For i As Int32 = 0 To selrows.Length - 1
                selrows(i)("onhandqty") = selrows(i)("onhandqty") - pUnitsAllocated
            Next
        End If
    End Sub

    Private Shared Sub UpdatePickLocAllocQty(ByVal pLocation As String, ByVal pUnitsAllocated As Decimal, oLogger As LogHandler)
        Dim selrows As DataRow()
        selrows = Planner.PickingLocationDataTable.GetRowsForLocation(pLocation)
        If selrows.Length >= 1 Then
            selrows(0)("LOADALLOCATEDQTY") += pUnitsAllocated
        End If
    End Sub

    Private Shared Function GetMatchPickingLocation(ByRef OnHandStock As VAllocationOnHandDataTable, ByVal pAllowOverAlloc As Boolean, ByVal StrategyLineFilter As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pSku As String, Optional ByVal FullUomSize As Double = -1, Optional ByVal oLogger As LogHandler = Nothing) As DataRow()
        Dim selrows As DataRow()
        Dim consigneeSkuDTRows As DataRow()
        Dim sql As String = String.Format("CONSIGNEE='{0}' and SKU='{1}' {2}", pConsignee, pSku, StrategyLineFilter)
        If FullUomSize > -1 Then
            'sql = sql & String.Format(" and currentqty >= {0}", FullUomSize)
        End If
        Try
            selrows = Planner.PickingLocationDataTable.GetRowsForConsigneeSku(pConsignee, pSku, sql)

            If selrows.Length > 0 Then
                If Not pAllowOverAlloc Then
                    For i As Int32 = 0 To selrows.Length - 1
                        selrows(i)("availableunits") = 0
                    Next
                Else
                    selrows = GetWaterLeveledPickingLocation(selrows)
                    selrows(0)("availableunits") = OnHandStock.GetOnHandQty(pConsignee, pOrderId, pSku)
                End If
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.WriteException(ex)
            End If
        End Try
        Return selrows
    End Function

    Private Shared Function GetWaterLeveledPickingLocation(ByVal selrows As DataRow(), Optional ByVal oLogger As LogHandler = Nothing) As DataRow()
        Dim bestPickLoc As DataRow()
        Dim minReplDueLevel As Decimal = Decimal.MaxValue
        Dim sBestLoc As String = String.Empty
        For i As Int32 = 0 To selrows.Length - 1
            Try
                selrows(i)("NumOfReplDue") = Math.Ceiling((selrows(i)("LOADALLOCATEDQTY") + selrows(i)("ALLOCATEDQTY") - selrows(i)("currentqty")) / (selrows(i)("maximumqty") - selrows(i)("replqty")))
                If selrows(i)("NumOfReplDue") < minReplDueLevel Then
                    minReplDueLevel = selrows(i)("NumOfReplDue")
                    sBestLoc = selrows(i)("location")
                End If
            Catch ex As Exception
            End Try
        Next
        If sBestLoc = String.Empty Then
            sBestLoc = selrows(0)("location")
        End If
        bestPickLoc = Planner.PickingLocationDataTable.GetRowsForLocation(sBestLoc)
        Return bestPickLoc
    End Function


#End Region

End Class