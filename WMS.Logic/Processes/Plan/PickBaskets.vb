Imports System.Collections.Generic

<CLSCompliant(False)> Public MustInherit Class PickBaskets
    Implements ICollection

#Region "Variables"
    Protected _pickmethod As String
    Protected _baskets As ArrayList
#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As PickingRequirments
        Get
            Return CType(_baskets(index), PickingRequirments)
        End Get
        Set(ByVal Value As PickingRequirments)
            _baskets(index) = Value
        End Set
    End Property

    Public ReadOnly Property PickMethod() As String
        Get
            Return _pickmethod
        End Get
    End Property

#End Region

#Region "Constructor"
    Public Sub New(ByVal oPickMethod As String)
        _pickmethod = oPickMethod
        _baskets = New ArrayList
    End Sub
#End Region

#Region "Methods"

    'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
    'Public MustOverride Function CanPlace(ByVal req As Requirment) As Boolean
    'Commented for Retrofit Item PWMS-748 (RWMS-439) End
    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public MustOverride Function CanPlace(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End

    'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
    'Public Overridable Sub Place(ByVal req As Requirment)
    'Commented for Retrofit Item PWMS-748 (RWMS-439) End
    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Overridable Sub Place(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing)
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End

        Dim preq As PickingRequirments
        For Each preq In Me
            'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
            'If preq.CanPlace(req) Then
            'Commented for Retrofit Item PWMS-748 (RWMS-439) End
            'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
            If preq.CanPlace(req, oLogger) Then
                'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
                preq.Place(req)
                Exit Sub
            End If
        Next
        preq = New PickingRequirments
        'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
        'preq.Place(req)
        'Commented for Retrofit Item PWMS-748 (RWMS-439) End
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
        preq.Place(req, oLogger)
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        Me.Add(preq)
    End Sub

    Public Function IsFullyAllocated() As Boolean
        Dim preq As PickingRequirments
        For Each preq In Me
            If preq.UnitsLeftToFullfill > 0 Then
                Return False
            End If
        Next
        Return True
    End Function

    'Public Sub Allocate(ByVal Stock As DataTable, ByVal OnHandStock As DataTable, ByVal oPlanStrategy As PlanStrategy, ByRef pStrategyAllocationSequence As Int32, Optional ByVal oLogger As LogHandler = Nothing)
    '    Dim StrategyLines As PlanStrategyDetailCollection = oPlanStrategy.PlanDetails
    '    Dim reqs As PickingRequirments
    '    Dim stratline, stratline1 As PlanStrategyDetail
    '    Dim i As Int32 = 0
    '    Dim orders, lines As String
    '    ' Requerments loop
    '    For Each reqs In Me
    '        ' For each requermets work on all its strategy lines
    '        For Each stratline In StrategyLines
    '            If Not oLogger Is Nothing Then
    '                Try
    '                    oLogger.writeSeperator(" ", 20)
    '                    oLogger.Write("Allocating Requirement")
    '                    oLogger.writeSeperator("-", 20)
    '                    oLogger.writeSeperator(" ", 20)
    '                    oLogger.writeSeperator(" ", 20)
    '                    orders = GetOrdersFromRequirements(reqs)
    '                    lines = GetLinesFromRequirements(reqs)
    '                    oLogger.Write("Consignee: " & reqs.Consignee.PadRight(20) & " Order: " & orders.PadRight(20) & " Order Line: " & lines.PadRight(10) & " SKU: " & reqs.Sku.PadRight(20) & " QTY: " & reqs.UnitsLeftToFullfill.ToString.PadRight(10))
    '                    oLogger.writeSeperator(" ", 20)
    '                    oLogger.writeSeperator(" ", 20)
    '                    oLogger.Write("Priority " & stratline.Priority & ", Required " & reqs.UnitsRequired)
    '                    oLogger.writeSeperator(" ", 20)
    '                Catch ex As Exception
    '                    oLogger.Write("No more data for this Requirement")
    '                    oLogger.writeSeperator(" ", 20)
    '                End Try
    '            End If
    '            reqs.Allocate(oPlanStrategy, Stock, OnHandStock, stratline.ZPicking, pStrategyAllocationSequence, stratline.PickType, stratline.AllocFullRequirment, stratline.AllocStratLineUOM, stratline.NegativePalletPickVol, stratline.AllocByHighestUOM, stratline.UOM, stratline.getDetailFilterByPriority(), stratline.AllocationUOMQTY, oLogger)
    '            If reqs.UnitsLeftToFullfill = 0 Then
    '                Exit For
    '            Else
    '                'Try to allocate from pick locations as well
    '                reqs.PickLocAllocate(stratline.OverAllocatePickLocations, oPlanStrategy, OnHandStock, stratline.ZPicking, pStrategyAllocationSequence, stratline.PickType, stratline.AllocFullRequirment, stratline.AllocStratLineUOM, stratline.NegativePalletPickVol, stratline.AllocByHighestUOM, stratline.UOM, stratline.getDetailFilterByPriority(True), stratline.AllocationUOMQTY, oLogger)
    '            End If
    '            If reqs.UnitsLeftToFullfill = 0 Then
    '                Exit For
    '            End If
    '        Next
    '        'i += 1
    '    Next
    '    'Check weather should allocate subs skus....
    '    If oPlanStrategy.SubstituteSkuMode = WMS.Lib.Plan.SUBSTITUTESKUMODE.ONSHORT Then
    '        For Each reqs In Me
    '            ' For each requermets work on all its strategy lines
    '            For Each stratline In StrategyLines
    '                If Not oLogger Is Nothing Then
    '                    Try
    '                        oLogger.writeSeperator(" ", 20)
    '                        oLogger.Write("Allocating Requirement According to substitues SKUs...")
    '                        oLogger.writeSeperator("-", 20)
    '                        oLogger.writeSeperator(" ", 20)
    '                        oLogger.writeSeperator(" ", 20)
    '                        orders = GetOrdersFromRequirements(reqs)
    '                        lines = GetLinesFromRequirements(reqs)
    '                        oLogger.Write("Consignee: " & reqs.Consignee.PadRight(20) & " Order: " & orders.PadRight(20) & " Order Line: " & lines.PadRight(10) & " SKU: " & reqs.Sku.PadRight(20) & " QTY: " & reqs.UnitsLeftToFullfill.ToString.PadRight(10))
    '                        oLogger.writeSeperator(" ", 20)
    '                        oLogger.writeSeperator(" ", 20)
    '                    Catch ex As Exception
    '                        oLogger.Write("No more data for this Requirement")
    '                        oLogger.writeSeperator(" ", 20)
    '                    End Try
    '                End If

    '                reqs.Allocate(oPlanStrategy, Stock, OnHandStock, stratline.ZPicking, pStrategyAllocationSequence, stratline.PickType, stratline.AllocFullRequirment, stratline.AllocStratLineUOM, stratline.NegativePalletPickVol, stratline.AllocByHighestUOM, stratline.UOM, stratline.getDetailFilterByPriority(), stratline.AllocationUOMQTY, oLogger, True)

    '                If reqs.UnitsLeftToFullfill = 0 Then
    '                    Exit For
    '                End If
    '            Next
    '        Next
    '    End If
    'End Sub


    Public Sub Allocate(ByVal Stock As VPlannerInventoryDataTable, ByVal OnHandStock As VAllocationOnHandDataTable, ByVal oPlanStrategy As PlanStrategy, ByRef pStrategyAllocationSequence As Int32, Optional ByVal oLogger As LogHandler = Nothing)
        Dim StrategyLines As PlanStrategyDetailCollection = oPlanStrategy.PlanDetails
        Dim reqs As PickingRequirments
        Dim stratline, stratline1 As PlanStrategyDetail
        Dim i As Int32 = 0
        Dim orders, lines As String
        ' Requerments loop
        For Each reqs In Me
            ' For each requermets work on all its strategy lines
            For Each stratline In StrategyLines
                If Not oLogger Is Nothing Then
                    Try
                        oLogger.writeSeperator(" ", 20)
                        oLogger.Write("Allocating Requirement")
                        oLogger.writeSeperator("-", 20)
                        oLogger.writeSeperator(" ", 20)
                        oLogger.writeSeperator(" ", 20)
                        orders = GetOrdersFromRequirements(reqs)
                        lines = GetLinesFromRequirements(reqs)
                        'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
                        'oLogger.Write("Consignee: " & reqs.Consignee.PadRight(20) & " Order: " & orders.PadRight(20) & " Order Line: " & lines.PadRight(10) & " SKU: " & reqs.Sku.PadRight(20) & " QTY: " & reqs.UnitsLeftToFullfill.ToString.PadRight(10))
                        'Commented for Retrofit Item PWMS-748 (RWMS-439) End
                        'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
                        oLogger.Write("Consignee: " & reqs.Consignee.PadRight(20) & " Order: " & orders.PadRight(20) & " Order Line: " & lines.PadRight(10) & " QTY: " & reqs.UnitsLeftToFullfill.ToString.PadRight(10))
                        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
                        oLogger.writeSeperator(" ", 20)
                        oLogger.writeSeperator(" ", 20)
                        oLogger.Write("Priority " & stratline.Priority & ", Required " & reqs.UnitsRequired)
                        oLogger.writeSeperator(" ", 20)
                        'Start RWMS-768
                        oLogger.Write("Using Strategy " & stratline.StrategyId.ToString() & " PickType " & stratline.PickType)
                        oLogger.writeSeperator(" ", 20)
                        'End  RWMS-768

                    Catch ex As Exception
                        oLogger.Write("No more data for this Requirement")
                        oLogger.writeSeperator(" ", 20)
                    End Try
                End If
                reqs.Allocate(oPlanStrategy, Stock, OnHandStock, stratline.ZPicking, pStrategyAllocationSequence, stratline.PickType, stratline.AllocFullRequirment, stratline.AllocStratLineUOM, stratline.NegativePalletPickVol, stratline.AllocByHighestUOM, stratline.UOM, stratline.getDetailFilterByPriority(), stratline.AllocationUOMQTY, oLogger, Nothing, stratline.FullPickAllocation)
                If reqs.UnitsLeftToFullfill = 0 Then
                    Exit For
                Else
                    'Try to allocate from pick locations as well
                    reqs.PickLocAllocate(stratline.OverAllocatePickLocations, oPlanStrategy, OnHandStock, stratline.ZPicking, pStrategyAllocationSequence, stratline.PickType, stratline.AllocFullRequirment, stratline.AllocStratLineUOM, stratline.NegativePalletPickVol, stratline.AllocByHighestUOM, stratline.UOM, stratline.getDetailFilterByPriority(True), stratline.AllocationUOMQTY, oLogger)
                End If
                If reqs.UnitsLeftToFullfill = 0 Then
                    Exit For
                End If
            Next
            'i += 1
        Next
        'Check weather should allocate subs skus....
        'If oPlanStrategy.SubstituteSkuMode = WMS.Lib.Plan.SUBSTITUTESKUMODE.ONSHORT Then

        Dim tmpReq As PickingRequirments
        Dim tmpSKU As SKU
        Dim subSKUsList As List(Of SubSKU)
        Dim pickReqList As New System.Collections.Generic.List(Of PickingRequirments)
        For Each reqs In Me
            ' For each requermets work on all its strategy lines
            If Not subSKUNeeded(reqs, oPlanStrategy.SubstituteSkuMode) Then Continue For

            tmpSKU = Planner.GetSKU(reqs.Consignee, reqs.Sku)
            subSKUsList = New List(Of SubSKU)
            'subSKUsList.Add(New SubSKU(tmpSKU, 1, 0, -1))
            subSKUsList.Add(New SubSKU(Nothing, tmpSKU, Nothing, 0))
            getSubstitueSKUsList(reqs, subSKUsList(0), subSKUsList)
            ''Remove the original SKU
            subSKUsList.Sort()
            subSKUsList.RemoveAt(0)


            For Each tmpSubSKU As SubSKU In subSKUsList
                'tmpReq = reqs.CloneForSubSKU(tmpSubSKU.SKU, tmpSubSKU.ConversionUnits)
                tmpReq = reqs.CloneForSubSKU(tmpSubSKU.SubSKUObj, tmpSubSKU.ConversionUnits)

                For Each stratline In StrategyLines
                    writeAllocationDataRequirementsToLog(oLogger, tmpReq, stratline)
                    tmpReq.Allocate(oPlanStrategy, Stock, OnHandStock, stratline.ZPicking, pStrategyAllocationSequence, stratline.PickType, stratline.AllocFullRequirment, stratline.AllocStratLineUOM, stratline.NegativePalletPickVol, stratline.AllocByHighestUOM, stratline.UOM, stratline.getDetailFilterByPriority(), stratline.AllocationUOMQTY, oLogger, tmpSubSKU.SubSKUData, stratline.FullPickAllocation)
                    '' Adding the conversion rate for later use in picklist creation and orderline allocation.
                    For Each all As Allocate In tmpReq.Allocated
                        all.SubSKUConversionUnits = tmpSubSKU.ConversionUnits
                    Next
                    If tmpReq.UnitsLeftToFullfill = 0 Then
                        Exit For
                    End If
                Next
                If tmpReq.UnitsFullfilled > 0 Then
                    pickReqList.Add(tmpReq)
                    If tmpReq.UnitsLeftToFullfill = 0 Then
                        Exit For
                    End If
                End If
            Next
        Next
        For Each pickReq As PickingRequirments In pickReqList
            Me.Add(pickReq)
        Next
    End Sub

    Private Sub writeAllocationDataRequirementsToLog(ByVal oLogger As WMS.Logic.LogHandler, ByVal pReqs As PickingRequirments, ByVal pStrategyLine As PlanStrategyDetail)
        If oLogger Is Nothing Then Return
        Try
            oLogger.writeSeperator(" ", 20)
            oLogger.Write("Allocating Requirement According to substitues SKUs...")
            oLogger.writeSeperator("-", 20)
            oLogger.writeSeperator(" ", 20)
            oLogger.writeSeperator(" ", 20)
            Dim orders, lines As String
            orders = GetOrdersFromRequirements(pReqs)
            lines = GetLinesFromRequirements(pReqs)
            oLogger.Write("Consignee: " & pReqs.Consignee.PadRight(20) & " Order: " & orders.PadRight(20) & " Order Line: " & lines.PadRight(10) & " SKU: " & pReqs.Sku.PadRight(20) & " QTY: " & pReqs.UnitsLeftToFullfill.ToString.PadRight(10))
            oLogger.writeSeperator(" ", 20)
            oLogger.writeSeperator(" ", 20)
            oLogger.Write("Priority " & pStrategyLine.Priority & ", Required " & pReqs.UnitsRequired)
            oLogger.writeSeperator(" ", 20)
        Catch ex As Exception
            oLogger.Write("No more data for this Requirement")
            oLogger.writeSeperator(" ", 20)
        End Try
    End Sub

    Private Function subSKUNeeded(ByVal pReqs As PickingRequirments, ByVal pSubSKUMode As String) As Boolean
        'If pSubSKUMode.Equals(WMS.Lib.Plan.SUBSTITUTESKUMODE.ONSHORT, StringComparison.OrdinalIgnoreCase) Then
        '    If pReqs.UnitsFullfilled > 0 Then Return False
        'End If
        If pSubSKUMode.Equals(WMS.Lib.Plan.SUBSTITUTESKUMODE.NEVER, StringComparison.OrdinalIgnoreCase) Then
            Return False
        End If
        Return True
    End Function

    'Private Function canUseSubSKU(ByVal pReqs As PickingRequirments, ByVal pSubSKUData As SKU.SkuSubstitutes) As Boolean
    '    If Not String.IsNullOrEmpty(pSubSKUData.COMPANY) Then
    '        For Each req As Requirment In pReqs
    '            If Not req.Company.Equals(pSubSKUData.COMPANY, StringComparison.OrdinalIgnoreCase) OrElse _
    '            Not req.CompanyType.Equals(pSubSKUData.COMPANYTYPE, StringComparison.OrdinalIgnoreCase) Then
    '                Return False
    '            End If
    '        Next
    '    End If
    '    If Not pSubSKUData.FROMDATE.Subtract(DateTime.Now).Ticks < 0 OrElse Not pSubSKUData.TODATE.Subtract(DateTime.Now).Ticks > 0 Then
    '        Return False
    '    End If
    '    '' Do something with pSubSKU.SUBSTITUTIONTYPE
    '    Return True
    'End Function


    '' When used the pSubsSKUList need to contain the original SKU
    Private Sub getSubstitueSKUsList(ByVal pReqs As PickingRequirments, ByVal pSubSKU As SubSKU, ByRef pSubsSKUList As List(Of SubSKU))
        Dim tmpList As New List(Of SubSKU)

        Dim tmpSubSKU As SubSKU
        Dim subSKUObj As SKU

        'For Each subSKU As SKU.SkuSubstitutes In pSKU.SKU.SUBSTITUTESSKU
        For Each subSKUData As SKU.SkuSubstitutes In pSubSKU.SubSKUObj.SUBSTITUTESSKU
            subSKUObj = Planner.GetSKU(subSKUData.CONSIGNEE, subSKUData.SUBSTITUTESKU)
            If subSKUObj Is Nothing Then
                subSKUObj = New SKU(subSKUData.CONSIGNEE, subSKUData.SUBSTITUTESKU)
                Planner.SetSKU(subSKUObj)
            End If
            tmpSubSKU = New SubSKU(subSKUData, subSKUObj, pSubSKU, pSubSKU.Level + 1)
            If Not canUseSubSKU(tmpSubSKU) Then Continue For
            If Not pSubsSKUList.Contains(tmpSubSKU) Then
                pSubsSKUList.Add(tmpSubSKU)
                tmpList.Add(tmpSubSKU)
            End If
        Next

        For Each zb As SubSKU In tmpList
            If zb.SubSKUData.MULTILEVEL Then
                getSubstitueSKUsList(pReqs, zb, pSubsSKUList)
            End If
        Next
    End Sub

    Private Class SubSKU
        Implements IComparable

        Private _subSKUData As WMS.Logic.SKU.SkuSubstitutes
        Private _subSKUObj As WMS.Logic.SKU
        Private _parentSubSKU As SubSKU
        Private _level As Integer

        Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            Dim tmpSKU As SubSKU = CType(obj, SubSKU)
            If tmpSKU.Level > Me.Level Then
                Return -1
            ElseIf tmpSKU.Level = Me.Level Then
                If tmpSKU.SubSKUData.PRIORITY > Me.SubSKUData.PRIORITY Then
                    Return -1
                ElseIf tmpSKU.SubSKUData.PRIORITY < Me.SubSKUData.PRIORITY Then
                    Return 1
                End If
                Return 0
            Else
                Return 1
            End If
        End Function

        Public Sub New(ByVal pSubSKUData As SKU.SkuSubstitutes, ByVal pSubSKUObj As SKU, ByVal pParentSubSKU As SubSKU, ByVal pLevel As Integer)
            _subSKUData = pSubSKUData
            _subSKUObj = pSubSKUObj
            _level = pLevel
            _parentSubSKU = pParentSubSKU
        End Sub

        Public Property SubSKUData() As WMS.Logic.SKU.SkuSubstitutes
            Get
                Return _subSKUData
            End Get
            Set(ByVal value As WMS.Logic.SKU.SkuSubstitutes)
                _subSKUData = value
            End Set
        End Property

        Public Property Level() As Integer
            Get
                Return _level
            End Get
            Set(ByVal value As Integer)
                _level = value
            End Set
        End Property

        Public Property SubSKUObj() As SKU
            Get
                Return _subSKUObj
            End Get
            Set(ByVal value As SKU)
                _subSKUObj = value
            End Set
        End Property

        Public Property ParentSubSKU() As SubSKU
            Get
                Return _parentSubSKU
            End Get
            Set(ByVal value As SubSKU)
                _parentSubSKU = value
            End Set
        End Property

        Public ReadOnly Property ConversionUnits() As Decimal
            Get
                If _parentSubSKU Is Nothing Then
                    Return 1
                Else
                    Return _parentSubSKU.ConversionUnits * _subSKUData.SUBSTITUTESKUQTY / _subSKUData.SKUQTY
                End If
            End Get
        End Property

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj Is Nothing Then Return False
            If Not obj.GetType().Equals(GetType(SubSKU)) Then Return False
            Dim tmpSKU As SubSKU = CType(obj, SubSKU)
            If tmpSKU.SubSKUObj.CONSIGNEE.Equals(Me.SubSKUObj.CONSIGNEE, StringComparison.OrdinalIgnoreCase) AndAlso _
           tmpSKU.SubSKUObj.SKU.Equals(Me.SubSKUObj.SKU, StringComparison.OrdinalIgnoreCase) Then
                Return True
            Else
                Return False
            End If
        End Function



    End Class

    Private Shared Function canUseSubSKU(ByVal pSubSKU As SubSKU) As Boolean
        If Not pSubSKU.SubSKUData.FROMDATE.Subtract(DateTime.Now).Ticks < 0 OrElse Not pSubSKU.SubSKUData.TODATE.Subtract(DateTime.Now).Ticks > 0 Then
            Return False
        End If
        If pSubSKU.SubSKUObj.NEWSKU Then
            Return False
        End If
        If Not pSubSKU.SubSKUObj.STATUS Then
            Return False
        End If
        '' Do something with pSubSKU.SubSKUData.SUBSTITUTIONTYPE
        Return True
    End Function

    'Private Class SubSKU
    '    Implements IComparable

    '    Private _sku As WMS.Logic.SKU
    '    Private _level As Integer
    '    Private _priority As Integer
    '    Private _conversionUnits As Decimal
    '    Private _multiLevel As Boolean

    '    Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
    '        Dim tmpSKU As SubSKU = CType(obj, SubSKU)
    '        If tmpSKU.Level > Me.Level Then
    '            Return -1
    '        ElseIf tmpSKU.Level = Me.Level Then
    '            If tmpSKU.Priority > Me.Priority Then
    '                Return -1
    '            ElseIf tmpSKU.Priority < Me.Priority Then
    '                Return 1
    '            End If
    '            Return 0
    '        Else
    '            Return 1
    '        End If
    '    End Function

    '    Public Sub New(ByVal pSKu As SKU, ByVal pConversionUnits As Decimal, ByVal pLevel As Integer, ByVal pPriority As Integer, ByVal pMulti)
    '        _level = pLevel
    '        _sku = pSKu
    '        _priority = pPriority
    '        _conversionUnits = pConversionUnits
    '    End Sub

    '    Public Property SKU() As WMS.Logic.SKU
    '        Get
    '            Return _sku
    '        End Get
    '        Set(ByVal value As WMS.Logic.SKU)
    '            _sku = value
    '        End Set
    '    End Property

    '    Public Property Level() As Integer
    '        Get
    '            Return _level
    '        End Get
    '        Set(ByVal value As Integer)
    '            _level = value
    '        End Set
    '    End Property

    '    Public Property Priority() As Integer
    '        Get
    '            Return _priority
    '        End Get
    '        Set(ByVal value As Integer)
    '            _priority = value
    '        End Set
    '    End Property

    '    Public Property ConversionUnits() As Decimal
    '        Get
    '            Return _conversionUnits
    '        End Get
    '        Set(ByVal value As Decimal)
    '            _conversionUnits = value
    '        End Set
    '    End Property

    '    Public Overrides Function Equals(ByVal obj As Object) As Boolean
    '        If obj Is Nothing Then Return False
    '        If Not obj.GetType() Is SKU.GetType() Then Return False
    '        Dim tmpSKu As SKU = CType(obj, SKU)
    '        If tmpSKu.CONSIGNEE.Equals(Me._sku.CONSIGNEE, StringComparison.OrdinalIgnoreCase) AndAlso tmpSKu.SKU.Equals(Me._sku.SKU, StringComparison.OrdinalIgnoreCase) Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    End Function

    'End Class






    Private Function GetOrdersFromRequirements(ByVal reqs As PickingRequirments) As String
        Dim ord As String = ""
        For Each req As Requirment In reqs
            If ord.IndexOf(req.OrderId) = -1 Then
                ord = ord & "," & req.OrderId
            End If
        Next
        ord = ord.TrimStart(",".ToCharArray())
        Return ord
    End Function

    Private Function GetLinesFromRequirements(ByVal reqs As PickingRequirments) As String
        Dim lns As String = ""
        For Each req As Requirment In reqs
            If lns.IndexOf(req.OrderLine) = -1 Then
                lns = lns & "," & req.OrderLine
            End If
        Next
        lns = lns.TrimStart(",".ToCharArray())
        Return lns
    End Function

    Public Function CreatePickLists(ByVal StrategyId As String, Optional ByVal oLogger As LogHandler = Nothing) As PicksObjectCollection
        Dim picks As New PicksObjectCollection
        Dim reqs As PickingRequirments
        Dim idx As Int32

        If Not oLogger Is Nothing Then
            oLogger.writeSeperator(" ", 20)
            oLogger.Write("Creating picklists for pickbasket")
        End If

        For Each reqs In Me
            'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
            'picks.AddRange(CreateMatchPicks(reqs, StrategyId))
            'picks.AddRange(CreateLeftPicks(reqs, StrategyId))
            'Commented for Retrofit Item PWMS-748 (RWMS-439) End
            'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
            picks.AddRange(CreateMatchPicks(reqs, StrategyId, oLogger))
            picks.AddRange(CreateLeftPicks(reqs, StrategyId, oLogger))
            'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        Next

        If Not oLogger Is Nothing Then oLogger.writeSeperator("-", 60)

        If picks.Count = 0 Then
            Return Nothing
        Else
            Return picks
        End If
    End Function

    Protected Function CreatePickObject(ByVal req As Requirment, ByVal allocs As Allocate, ByVal Units As Double, ByVal StrategyId As String) As PicksObject
        Dim opick As New PicksObject
        opick.Consignee = req.Consignee
        opick.LoadId = allocs.LoadId
        opick.FromLocation = allocs.Location
        opick.LocationSortOrder = allocs.SortOrder
        opick.OrderId = req.OrderId
        opick.Company = req.Company
        opick.CompanyType = req.CompanyType
        opick.OrderLine = req.OrderLine
        opick.PickMethod = _pickmethod
        opick.PickRegion = allocs.PickRegion
        opick.WarehouseArea = allocs.WarehouseArea
        opick.FromWarehousearea = allocs.WarehouseArea
        opick.PickType = allocs.PickType
        opick.SKU = req.Sku
        opick.SkuSortOrder = req.PickSortOrder
        opick.ToLocation = req.StagingLane
        opick.ToWarehousearea = req.StaginglaneWHArea
        opick.Units = Units
        opick.Volume = req.RequirementVolume
        opick.Weight = req.RequirementWeight
        opick.UOM = allocs.UOM
        opick.Wave = req.Wave
        opick.StrategyId = StrategyId
        req.QuantityLeftToFullfill = req.QuantityLeftToFullfill - Units
        allocs.Units = allocs.Units - Units
        opick.IsPickLocAllocation = allocs.IsPickLocAllocation
        opick.IsOverAllocation = allocs.IsOverAllocation

        opick.SubSKUConversionUnits = allocs.SubSKUConversionUnits
        Return opick
    End Function

    Protected Function CreateMatchPicks(ByVal reqs As PickingRequirments, ByVal StrategyId As String, Optional ByVal oLogger As LogHandler = Nothing) As PicksObjectCollection
        Dim picks As New PicksObjectCollection
        Dim allocs As Allocate
        Dim req As Requirment
        Dim Placed As Boolean = True

        If Not oLogger Is Nothing Then
            oLogger.writeSeperator(" ", 20)
            oLogger.Write("Creating match picks")
        End If

        While Placed
            Placed = False
            For Each req In reqs
                If req.QuantityLeftToFullfill > 0 Then
                    If Not oLogger Is Nothing Then oLogger.Write("Requirement quantity left to fullfill.... : " + req.QuantityLeftToFullfill.ToString())
                    For Each allocs In reqs.Allocated
                        If allocs.Units > 0 Then
                            If allocs.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                                If Not oLogger Is Nothing Then oLogger.Write("Allocation for FULLPICK.... : " + allocs.Units.ToString())
                                If allocs.Units > req.QuantityLeftToFullfill Then
                                    Dim oPicksObj As PicksObject = CreatePickObject(req, allocs, req.QuantityLeftToFullfill, StrategyId)
                                    If oPicksObj.Units > 0 Then
                                        picks.Add(oPicksObj)
                                    End If
                                    Placed = True
                                Else
                                    picks.Add(CreatePickObject(req, allocs, allocs.Units, StrategyId))
                                    Placed = True
                                End If
                            Else
                                If Not oLogger Is Nothing Then oLogger.Write("Allocation for PARTIALPICK.... : " + allocs.UnitsPerMeasure.ToString())
                                If allocs.UnitsPerMeasure <= req.QuantityLeftToFullfill Then
                                    Dim uomleftto As Double = req.QuantityLeftToFullfill / allocs.UnitsPerMeasure 'Convert.ToInt32(Decimal.Floor(req.QuantityLeftToFullfill / allocs.UnitsPerMeasure))
                                    Dim AllocatedUomLeft As Double = allocs.Units / allocs.UnitsPerMeasure
                                    If Not oLogger Is Nothing Then oLogger.Write("UOMLeftTo .... : " + uomleftto.ToString() + "   " + "Allocated Uom Left .... : " + AllocatedUomLeft.ToString())
                                    If AllocatedUomLeft > uomleftto Then
                                        If allocs.AllocationQuanta < uomleftto Then
                                            picks.Add(CreatePickObject(req, allocs, uomleftto * allocs.UnitsPerMeasure, StrategyId))
                                            Placed = True
                                        End If
                                    Else
                                        picks.Add(CreatePickObject(req, allocs, AllocatedUomLeft * allocs.UnitsPerMeasure, StrategyId))
                                        Placed = True
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Next
        End While

        If Not oLogger Is Nothing Then
            oLogger.Write("Finished creating match picks")
            oLogger.writeSeperator(" ", 20)
        End If

        Return picks
    End Function

    Protected Function CreateLeftPicks(ByVal reqs As PickingRequirments, ByVal StrategyId As String, Optional ByVal oLogger As LogHandler = Nothing) As PicksObjectCollection
        Dim picks As New PicksObjectCollection
        Dim allocs As Allocate
        Dim req As Requirment

        If Not oLogger Is Nothing Then
            oLogger.writeSeperator(" ", 20)
            oLogger.Write("Creating left picks")
            oLogger.writeSeperator("-", 21)
        End If

        For Each req In reqs
            If req.QuantityLeftToFullfill > 0 Then
                If Not oLogger Is Nothing Then oLogger.Write("Requirement quantity left to fullfill.... : " + req.QuantityLeftToFullfill.ToString())
                For Each allocs In reqs.Allocated
                    If allocs.Units > 0 Then
                        If Not oLogger Is Nothing Then oLogger.Write("Allocation units available.... : " + allocs.Units.ToString())
                        If allocs.Units >= req.QuantityLeftToFullfill Then
                            picks.Add(CreatePickObject(req, allocs, req.QuantityLeftToFullfill, StrategyId))
                            Exit For
                        Else
                            picks.Add(CreatePickObject(req, allocs, allocs.Units, StrategyId))
                        End If
                    End If
                Next
            End If
        Next

        If Not oLogger Is Nothing Then
            oLogger.Write("Finished creating left picks")
            oLogger.writeSeperator(" ", 20)
        End If

        Return picks
    End Function

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As PickingRequirments) As Integer
        Return _baskets.Add(value)
    End Function

    Public Sub Clear()
        _baskets.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As PickingRequirments)
        _baskets.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As PickingRequirments)
        _baskets.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _baskets.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _baskets.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _baskets.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _baskets.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _baskets.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _baskets.GetEnumerator()
    End Function

#End Region

End Class