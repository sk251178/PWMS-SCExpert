<CLSCompliant(False)> Public Class PicksObjectCollection
    Implements ICollection

#Region "Variables"

    Protected _pck As ArrayList
    Protected _containertype As String
    Protected _container As HandelingUnit
    Protected _currvolume As Double
    Protected _currweight As Double
    Protected _ContainsErgoHeavyItems As Boolean
    Protected _ContainsErgoLightItems As Boolean
    Protected _ErgoCurrWeight As Double
    Protected _ErgoCurrCube As Double

    Protected _closeCont As Boolean

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As PicksObject
        Get
            Return CType(_pck(index), PicksObject)
        End Get
        Set(ByVal Value As PicksObject)
            _pck(index) = Value
        End Set
    End Property

    Public ReadOnly Property Container() As HandelingUnit
        Get
            Return _container
        End Get
    End Property

    Public ReadOnly Property CurrVolume() As Double
        Get
            Return _currvolume
        End Get
    End Property

    Public ReadOnly Property CurrWeight() As Double
        Get
            Return _currweight
        End Get
    End Property

    Public Property CloseCont() As Boolean
        Get
            Return _closeCont
        End Get
        Set(ByVal value As Boolean)
            _closeCont = value
        End Set
    End Property

#End Region

#Region "Constructor"
    Public Sub New()
        _pck = New ArrayList
    End Sub
#End Region

#Region "Methods"

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Sub ExportToLog(ByVal oLogger As LogHandler)
        If Not oLogger Is Nothing Then
            Try
                oLogger.writeSeperator(" ", 20)
                oLogger.Write("Exporting PickObjectsCollection")
                Dim lgpckobj As PicksObject
                For Each lgpckobj In Me
                    lgpckobj.ExportToLog(oLogger)
                Next
            Catch ex As Exception
                oLogger.Write(ex.ToString())
            End Try
        End If
    End Sub

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
    Public Sub CreateLoadingPlan(ByVal pPickListId As String)
        'If Me.Count > 0 Then
        '    Dim pckbr As PicksObject
        '    Dim ldplan As New LoadingPlanner
        '    ldplan.Post(Me, pPickListId)
        'End If
    End Sub

    Public Function Post() As String
        Try


        If Me.Count > 0 Then
            Dim pckid As String = Made4Net.Shared.Util.getNextCounter("PICKLIST")
            Dim pckbr As PicksObject
            Dim pcklist As New Picklist(pckid, False)
            pcklist.Post(Me)
            Return pckid
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return String.Empty
    End Function
    ''Start RWMS-1279 Planner service not planning end of pick

    Public Sub PlaceInNewPickList(ByVal Pck As PicksObject, Optional ByVal oLogger As LogHandler = Nothing)
        Dim pckvol, pckweight As Double
        Try
            pckvol = Inventory.CalculateVolume(Pck.Consignee, Pck.SKU, Pck.Units, Pck.UOM)
            pckweight = Inventory.CalculateWeight(Pck.Consignee, Pck.SKU, Pck.Units, Pck.UOM)
            _currvolume = _currvolume + pckvol
            _currweight = _currweight + pckweight
            If Not oLogger Is Nothing Then
                oLogger.Write("Pick line added to current PICKLIST. sku=" + Pck.SKU.ToString() + ",Qty=" + Pck.Units.ToString() + ",pckVol=" + pckvol.ToString() + ",pckWeight=" + pckweight.ToString() + ",PickList Total CubeVol=" + _currvolume.ToString() + ",PickList Total Weight=" + _currweight.ToString())
            End If
            Me.Add(Pck)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Exception found in PlaceInNewPickList(). Error Msg=" + ex.Message.ToString())
            End If
        End Try
    End Sub
    ''End RWMS-1279 Planner service not planning end of pick
    Public Sub PlaceInNewPickList(ByVal Pck As PicksObject, ByVal pErgoHeavyTreshHold As Decimal)
        Dim pckvol, pckweight As Double
        pckvol = Inventory.CalculateVolume(Pck.Consignee, Pck.SKU, Pck.Units, Pck.UOM)
        pckweight = Inventory.CalculateWeight(Pck.Consignee, Pck.SKU, Pck.Units, Pck.UOM)
        _currvolume = _currvolume + pckvol
        _currweight = _currweight + pckweight
        If pckweight >= pErgoHeavyTreshHold Then
            _ErgoCurrCube = _ErgoCurrCube + pckvol
            _ErgoCurrWeight = _ErgoCurrWeight + pckweight
            _ContainsErgoHeavyItems = True
        Else
            _ContainsErgoLightItems = True
        End If
        Me.Add(Pck)
    End Sub

    Public Sub PlaceInNewPickList(ByVal ContainerType As String, ByVal Pck As PicksObject, ByVal pErgoHeavyTreshHold As Decimal)
        If ((ContainerType = Nothing) Or (ContainerType = "")) Then
            _containertype = Nothing
        Else
            _containertype = ContainerType
            Try
                _container = New HandelingUnit(_containertype)
            Catch ex As Exception
                _container = Nothing
            End Try
            Pck.ContainerType = _containertype
        End If


        Dim pckvol, pckweight As Double
        pckvol = Inventory.CalculateVolume(Pck.Consignee, Pck.SKU, Pck.Units, Pck.UOM)
        pckweight = Inventory.CalculateWeight(Pck.Consignee, Pck.SKU, Pck.Units, Pck.UOM)
        _currvolume = _currvolume + pckvol
        _currweight = _currweight + pckweight
        If pckweight >= pErgoHeavyTreshHold Then
            _ErgoCurrCube = _ErgoCurrCube + pckvol
            _ErgoCurrWeight = _ErgoCurrWeight + pckweight
            _ContainsErgoHeavyItems = True
        Else
            _ContainsErgoLightItems = True
        End If
        Me.Add(Pck)
    End Sub

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End

    'Public Function CanPlaceInContainer(ByVal pck As PicksObject, ByVal Weight As Decimal, ByVal Cube As Decimal, ByVal MinPlannedCube As Decimal, _
    '            ByVal BaseCube As Decimal, ByVal BaseWeight As Decimal, ByVal pErgoHeavyTreshHold As Decimal, ByVal pErgoWeightLimit As Decimal, _
    '            ByVal pErgoCubeLimit As Decimal, ByVal pErgoWeightLimitAllHeavy As Decimal, ByVal pErgoCubeLimitAllHeavy As Decimal, _
    '            ByVal pNoSplitLineWeightPCT As Decimal, ByVal pNoSplitLineCubePCT As Decimal, ByVal pAllowGoBack As Boolean, _
    '            ByVal pExtendWalkForFillPCT As Decimal, ByVal pAllowMultiBaseItems As Boolean, ByVal pPickPartialUOM As Boolean, _
    '            Optional ByVal pMatchCustomer As Boolean = False, Optional ByVal pContType As String = Nothing) As Boolean

    '    Dim pckvol, pckweight As Double
    '    ' Lets validate by SKU master data
    '    Dim sk As SKU '= New SKU(pck.Consignee, pck.SKU)
    '    If Planner.GetSKU(pck.Consignee, pck.SKU) Is Nothing Then
    '        sk = New SKU(pck.Consignee, pck.SKU)
    '        Planner.SetSKU(sk)
    '    Else
    '        sk = Planner.GetSKU(pck.Consignee, pck.SKU)
    '    End If
    '    pckvol = Inventory.CalculateVolume(pck.Consignee, pck.SKU, pck.Units, pck.UOM)
    '    pckweight = Inventory.CalculateWeight(pck.Consignee, pck.SKU, pck.Units, pck.UOM)

    '    'Check if the current pick goes under the no-split tresh hold
    '    If _currvolume > 0 AndAlso pckvol / _currvolume <= pNoSplitLineWeightPCT Then Return True
    '    If _currweight > 0 AndAlso pckweight / _currweight <= pNoSplitLineCubePCT Then Return True
    '    If _ErgoCurrCube > 0 AndAlso pckvol / _ErgoCurrCube <= pNoSplitLineWeightPCT Then Return True
    '    If _ErgoCurrWeight > 0 AndAlso pckweight / _ErgoCurrWeight <= pNoSplitLineCubePCT Then Return True

    '    Select Case sk.BASEITEM
    '        Case "NEVER"
    '            Return True
    '        Case "ALWAYS"
    '            If Not pAllowMultiBaseItems Or Not pAllowGoBack Then
    '                Return False
    '            End If
    '        Case Else
    '            ' Check  Base Cube and Base Weight
    '            If BaseCube > 0 And BaseWeight > 0 Then
    '                If pckvol > BaseCube Or pckweight > BaseWeight Then
    '                    Return False
    '                End If
    '            ElseIf BaseCube > 0 Then
    '                If pckvol > BaseCube Then
    '                    Return False
    '                End If
    '            ElseIf BaseWeight > 0 Then
    '                If pckweight > BaseWeight Then
    '                    Return False
    '                End If
    '            End If
    '    End Select

    '    'Validate Ergo weight & cube limit
    '    If pckweight >= pErgoHeavyTreshHold Then
    '        If _ContainsErgoLightItems Then
    '            If _ErgoCurrWeight + pckweight > pErgoWeightLimit Then Return False
    '            If _ErgoCurrCube + pckvol > pErgoCubeLimit Then Return False
    '        ElseIf _ContainsErgoHeavyItems Then
    '            If _ErgoCurrWeight + pckweight > pErgoWeightLimitAllHeavy Then Return False
    '            If _ErgoCurrCube + pckvol > pErgoCubeLimitAllHeavy Then Return False
    '        End If
    '    End If

    '    'Validate Regular weight and cube limit
    '    If _container Is Nothing And pContType Is Nothing Then Return True
    '    If Not pContType Is Nothing And _container Is Nothing Then
    '        _container = New HandelingUnit(pContType)
    '    End If

    '    ' Lets see if Weight and Cube is greater than zero and if yes override container data
    '    Dim ContainerCubeCapacity As Decimal
    '    Dim ContainerWeightCapacity As Decimal
    '    If Weight > 0 Then
    '        ContainerWeightCapacity = Weight
    '    Else
    '        ContainerWeightCapacity = _container.WeightCapacity
    '    End If
    '    If Cube > 0 Then
    '        ContainerCubeCapacity = Cube
    '    Else
    '        ContainerCubeCapacity = _container.CubeCapacity
    '    End If
    '    ' After setting weight and cube we will check if we can add another load with less then MinPlannedCube cube
    '    If _currvolume + pckvol > ContainerCubeCapacity + MinPlannedCube Then Return False
    '    If _currweight + pckweight > ContainerWeightCapacity Then Return False
    '    If pMatchCustomer Then
    '        If Me(0).Consignee.ToUpper <> pck.Consignee.ToUpper Or Me(0).OrderId.ToUpper <> pck.OrderId.ToUpper Then
    '            Return False
    '        End If
    '    End If
    '    'Check that if partial uom is set to false - we cannot add the whole qty to container - should split line
    '    If Not ValidatePartialUOMS(pck, pPickPartialUOM, pContType) Then Return False
    '    'Everything is ok - return true.
    '    Return True
    'End Function

    'Commented for RWMS-828
    'Public Function CanPlaceInNewPickList(ByVal pck As PicksObject, ByVal Weight As Decimal, ByVal Cube As Decimal, ByVal MinPlannedCube As Decimal, _
    '            ByVal BaseCube As Decimal, ByVal BaseWeight As Decimal, ByVal pErgoHeavyTreshHold As Decimal, ByVal pErgoWeightLimit As Decimal, _
    '            ByVal pErgoCubeLimit As Decimal, ByVal pErgoWeightLimitAllHeavy As Decimal, ByVal pErgoCubeLimitAllHeavy As Decimal, _
    '            ByVal pNoSplitLineWeightPCT As Decimal, ByVal pNoSplitLineCubePCT As Decimal, ByVal pAllowGoBack As Boolean, _
    '            ByVal pExtendWalkForFillPCT As Decimal, ByVal pAllowMultiBaseItems As Boolean, ByVal pPickPartialUOM As Boolean, ByVal oLogger As LogHandler, _
    '            Optional ByVal pMatchCustomer As Boolean = False, Optional ByVal pContType As String = Nothing) As Boolean
    'End Commented for RWMS-828
    'Added for RWMS-828
    Public Function CanPlaceInNewPickList(ByVal pck As PicksObject, ByVal Weight As Decimal, ByVal Cube As Decimal, ByVal MinPlannedCube As Decimal, _
                ByVal BaseCube As Decimal, ByVal BaseWeight As Decimal, ByVal pErgoHeavyTreshHold As Decimal, ByVal pErgoWeightLimit As Decimal, _
                ByVal pErgoCubeLimit As Decimal, ByVal pErgoWeightLimitAllHeavy As Decimal, ByVal pErgoCubeLimitAllHeavy As Decimal, _
                ByVal pNoSplitLineWeightPCT As Decimal, ByVal pNoSplitLineCubePCT As Decimal, ByVal pAllowGoBack As Boolean, _
                ByVal pExtendWalkForFillPCT As Decimal, ByVal pAllowMultiBaseItems As Boolean, ByVal pPickPartialUOM As Boolean, ByVal oLogger As LogHandler, _
                Optional ByVal pMatchCustomer As Boolean = False, Optional ByVal pContType As String = Nothing, Optional ByVal IsSplitLineSet As Boolean = False, Optional ByRef CubeValidation As Boolean = True, Optional ByRef WeightValidation As Boolean = True) As Boolean
        'Ended for RWMS-909(RWMS-866)

        'Added for PWMS-807 Start
        'Added for RWMS-866 (Srini Suggested Change) Start

        'Added for PWMS-807 Start
        Dim SQL1 As String
        Dim SQL2 As String
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim Cubepck As Decimal
        Dim Weightpck As Decimal


        SQL1 = String.Format("SELECT TOP 1 STRATEGYID,PICKREGION,CUBE , WEIGHT ,PRIORITY from PLANSTRATEGYBREAK WHERE PICKTYPE ='PARTIAL' AND STRATEGYID='{0}' AND PICKREGION ='{1}'", pck.StrategyId, pck.PickRegion)

        Made4Net.DataAccess.DataInterface.FillDataset(SQL1, dt1)


        If dt1.Rows.Count = 1 Then
            'Commented for PWMS-807 Start
            'Cubepck = dt1.Rows(0)("CUBE")
            'Weightpck = dt1.Rows(0)("WEIGHT")
            'Commented for PWMS-807 End

            'Commented for RWMS-1899 and RWMS-1798 Start
            ''Added for PWMS-807 Start
            'If Convert.ToString(dt1.Rows(0)("CUBE")) = "" Or IsDBNull(dt1.Rows(0)("CUBE")) Then
            ' Cubepck = 0
            'Else
            ' Cubepck = dt1.Rows(0)("CUBE")
            'End If

            'If Convert.ToString(dt1.Rows(0)("WEIGHT")) = "" Or IsDBNull(dt1.Rows(0)("WEIGHT")) Then
            ' Weightpck = 0
            'Else
            ' Weightpck = dt1.Rows(0)("WEIGHT")
            'End If

            ''Added for PWMS-807 End

            'Commented for RWMS-1899 and RWMS-1798 End


            'Added for RWMS-1899 and RWMS-1798 Start
            If String.IsNullOrEmpty(dt1.Rows(0)("CUBE")) Then

                Cubepck = 0
            Else
                Cubepck = dt1.Rows(0)("CUBE")
            End If

            If String.IsNullOrEmpty(dt1.Rows(0)("WEIGHT")) Then
                Weightpck = 0
            Else
                Weightpck = dt1.Rows(0)("WEIGHT")
            End If

            'Added for RWMS-1899 and RWMS-1798 End

        Else
            SQL2 = String.Format("SELECT TOP 1 STRATEGYID,PICKREGION,CUBE , WEIGHT ,PRIORITY from PLANSTRATEGYBREAK WHERE PICKTYPE ='PARTIAL' AND STRATEGYID='{0}' AND PICKREGION IS NULL", pck.StrategyId)

            Made4Net.DataAccess.DataInterface.FillDataset(SQL2, dt2)


            If dt2.Rows.Count = 1 Then
                'Commented for PWMS-807 Start
                'Cubepck = dt2.Rows(0)("CUBE")
                'Weightpck = dt2.Rows(0)("WEIGHT")
                'Commented for PWMS-807 End
                'Commented for RWMS-1798 Start
                ''Added for PWMS-807 Start
                'If Convert.ToString(dt1.Rows(0)("CUBE")) = "" Or IsDBNull(dt1.Rows(0)("CUBE")) Then
                ' Cubepck = 0
                'Else
                ' Cubepck = dt1.Rows(0)("CUBE")
                'End If

                'If Convert.ToString(dt1.Rows(0)("WEIGHT")) = "" Or IsDBNull(dt1.Rows(0)("WEIGHT")) Then
                ' Weightpck = 0
                'Else
                ' Weightpck = dt1.Rows(0)("WEIGHT")
                'End If

                ''Added for PWMS-807 End

                'Commented for RWMS-1798 End

                'Added for RWMS-1899 and RWMS-1798 Start
                If String.IsNullOrEmpty(dt2.Rows(0)("CUBE")) Then

                    Cubepck = 0
                Else
                    Cubepck = dt2.Rows(0)("CUBE")
                End If

                If String.IsNullOrEmpty(dt2.Rows(0)("WEIGHT")) Then
                    Weightpck = 0
                Else
                    Weightpck = dt2.Rows(0)("WEIGHT")
                End If

                'Added for RWMS-1899 and RWMS-1798 End

            Else

                Cubepck = 0
                Weightpck = 0

            End If

        End If

        Cube = Cubepck
        Weight = Weightpck

        'Added for PWMS-807 End

        If Not oLogger Is Nothing Then
            Try
                oLogger.Write("Checking for picklist placement for SKU " + pck.SKU + " for consignee " + pck.Consignee)
                Dim strLogString As String = "Break Startegy Params - "
                strLogString = strLogString + " Weight - " & Weight.ToString()
                strLogString = strLogString + " Cube - " & Cube.ToString()
                strLogString = strLogString + " MinPlannedCube - " & MinPlannedCube.ToString()
                oLogger.Write(strLogString)
            Catch ex As Exception
                'Do nothing in case of exception
            End Try
        End If

        'If _closePicklist Then Return False

        Dim pckvol, pckweight As Double
        Dim tempTotalVolume As Double 'RWMS-1279
        Dim tempTotalWeight As Double 'RWMS-1279
        pckvol = Inventory.CalculateVolume(pck.Consignee, pck.SKU, pck.Units, pck.UOM)
        pckweight = Inventory.CalculateWeight(pck.Consignee, pck.SKU, pck.Units, pck.UOM)

        If Not oLogger Is Nothing Then
            oLogger.Write("Calculated inventory volume for units " + pck.Units.ToString() + " for UOM " + pck.UOM + " = " + pckvol.ToString())
            oLogger.Write("Calculated inventory weight for units " + pck.Units.ToString() + " for UOM " + pck.UOM + " = " + pckweight.ToString())
            'Start RWMS-1279  Planner service not planning end of pick
            tempTotalVolume = Me.CurrVolume + pckvol
            tempTotalWeight = Me.CurrVolume + pckweight
            oLogger.Write("Calculated picklist TOTAL CUBE VOLUME will be after adding SKU:" + pck.SKU.ToString() + "=" + tempTotalVolume.ToString())
            oLogger.Write("Calculated picklist TOTAL WEIGHT will be after adding SKU :" + pck.SKU.ToString() + "=" + tempTotalWeight.ToString())
            'End RWMS-1279  Planner service not planning end of pick
        End If

        If ((pContType <> "") And (pContType <> Nothing) AndAlso (_container Is Nothing)) Then
            _container = New HandelingUnit(pContType)
            If Not oLogger Is Nothing Then
                oLogger.Write("Created HandlingUnit of type " + pContType)
            End If
        End If

        ' Lets see if Weight and Cube is greater than zero and if yes override picklist data
        Dim CubeCapacity As Decimal = 0.0
        Dim WeightCapacity As Decimal = 0.0
        If ((pContType = Nothing) Or (pContType = "")) Then
            If (Cube > 0) Then
                CubeCapacity = Cube
            End If
            If (Weight > 0) Then
                WeightCapacity = Weight
            End If
        Else
            If _container Is Nothing Then
                CubeCapacity = 999999
            Else
                CubeCapacity = _container.CubeCapacity
            End If

            If _container Is Nothing Then
                WeightCapacity = 9999999
            Else
                WeightCapacity = _container.WeightCapacity
            End If
        End If

        'Added for RWMS-1899 and RWMS-1798 Start
        If Not oLogger Is Nothing Then
            oLogger.Write("CanPlaceInNewPickList - Defined cube capacity " + CubeCapacity.ToString())
            oLogger.Write("CanPlaceInNewPickList - Defiend weight capacity " + WeightCapacity.ToString())
        End If
        'Added for RWMS-1899 and RWMS-1798 End

        If (CubeCapacity > 0) Then

            If validateCube(CubeCapacity, pckvol, MinPlannedCube, pNoSplitLineCubePCT) = False Then
                'Added for PWMS-807 Start
                CubeValidation = False
                'Added for PWMS-807 End
                Return False
            End If
        End If
        If (WeightCapacity > 0) Then
            If validateWeight(WeightCapacity, pckweight, pNoSplitLineWeightPCT) = False Then
                'Added for PWMS-807 Start
                WeightValidation = False
                'Added for PWMS-807 End
                Return False
            End If
        End If

        'Validate Ergo weight & cube limit
        If (pErgoHeavyTreshHold > 0) Then
            If pckweight >= pErgoHeavyTreshHold Then
                If _ContainsErgoLightItems Then
                    If _ErgoCurrWeight + pckweight > pErgoWeightLimit Then Return False
                    If _ErgoCurrCube + pckvol > pErgoCubeLimit Then Return False
                ElseIf _ContainsErgoHeavyItems Then
                    If _ErgoCurrWeight + pckweight > pErgoWeightLimitAllHeavy Then Return False
                    If _ErgoCurrCube + pckvol > pErgoCubeLimitAllHeavy Then Return False
                End If
            End If
        End If

        If Not validateBaseItem(pck, pckvol, pckweight, pAllowMultiBaseItems, pAllowGoBack, BaseCube, BaseWeight) Then
            Return False
        End If


        If pMatchCustomer Then
            If Me(0).Consignee.ToUpper <> pck.Consignee.ToUpper Or Me(0).OrderId.ToUpper <> pck.OrderId.ToUpper Then
                Return False
            End If
        End If

        'Check that if partial uom is set to false - we cannot add the whole qty to container - should split line
        If (((pContType <> Nothing) And (pContType <> ""))) Then
            If Not ValidatePartialUOMS(pck, pPickPartialUOM, pContType, oLogger) Then Return False
        End If
        'Everything is ok - return true.
        Return True
    End Function

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End



    Private Function getWeightCapacity(ByVal pWeight As Decimal) As Decimal
        If pWeight > 0 Then
            Return pWeight
        End If

        If _container Is Nothing Then
            Return 9999999
        Else
            Return _container.WeightCapacity
        End If

    End Function

    Private Function getCubeCapacity(ByVal pCube As Decimal) As Decimal
        If pCube > 0 Then
            Return pCube
        Else
            If _container Is Nothing Then
                Return 999999
            Else
                Return _container.CubeCapacity
            End If
        End If
    End Function

    Private Function validateCube(ByVal pCubeCapacity As Decimal, ByVal pPickVol As Decimal, ByVal pMinCube As Decimal, ByVal pNoSplitLineCubePCT As Decimal) As Boolean

        If ((_currvolume + pPickVol) <= (pCubeCapacity + pMinCube)) _
            Or ((_currvolume > 0) AndAlso ((pPickVol / _currvolume) <= pNoSplitLineCubePCT)) _
            Or ((_ErgoCurrCube > 0) AndAlso ((pPickVol / _ErgoCurrCube) <= pNoSplitLineCubePCT)) Then

            Return True
        Else
            Return False
        End If
    End Function



    Private Function validateWeight(ByVal pWeightCapacity As Decimal, ByVal pPickWeight As Decimal, ByVal pNoSplitLineWeightPCT As Decimal) As Boolean
        If ((_currweight + pPickWeight) <= pWeightCapacity) _
            Or ((_currweight > 0) AndAlso ((pPickWeight / _currweight) <= pNoSplitLineWeightPCT)) _
            Or ((_ErgoCurrWeight > 0) AndAlso ((pPickWeight / _ErgoCurrWeight) <= pNoSplitLineWeightPCT)) Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function validateBaseItem(ByVal pck As PicksObject, ByVal pPckVol As Decimal, ByVal pPckWeight As Decimal, ByVal pAllowMultiBaseItems As Boolean, ByVal pAllowGoBack As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal) As Boolean
        ' Lets validate by SKU master data
        Dim sk As SKU
        If Planner.GetSKU(pck.Consignee, pck.SKU) Is Nothing Then
            sk = New SKU(pck.Consignee, pck.SKU)
            Planner.SetSKU(sk)
        Else
            sk = Planner.GetSKU(pck.Consignee, pck.SKU)
        End If
        Select Case sk.BASEITEM
            Case "NEVER"
                Return True
            Case "ALWAYS"

                If Me.Count = 0 Then
                    Return True
                Else
                    If pAllowMultiBaseItems Then
                        Return True
                    End If
                End If

                Return False

                'If Not pAllowMultiBaseItems Or Not pAllowGoBack Then
                'Return False
                'Else
                'Return True
                'End If
            Case Else
                If pBaseCube > 0 Then
                    If pPckVol > pBaseCube Then
                        Return False
                    End If
                End If

                If pBaseWeight > 0 Then
                    If pPckWeight > pBaseWeight Then
                        Return False
                    End If
                End If
                Return True
        End Select
    End Function

    Private Function ValidatePartialUOMS(ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, ByVal pContType As String, ByVal oLogger As LogHandler) As Boolean
        If pPickPartialUOM Then
            Return True
        Else
            Dim numUnitsPerCont As Decimal = GetNumberUnitsPerContainer(pContType, pck, pPickPartialUOM, oLogger)
            If numUnitsPerCont = 0 Then
                Return Nothing
            End If
            Return pck.Units <= numUnitsPerCont
        End If
        Return False
    End Function

    Private Function GetNumberUnitsPerContainer(ByVal ContainerType As String, ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, ByVal oLogger As LogHandler) As Decimal

        If Not oLogger Is Nothing Then
            oLogger.Write("Calculating NumberUnitsPerContainer")
        End If
        Dim NumUnits As Decimal
        Dim oCont As HandelingUnit
        If Not _container Is Nothing Then
            oCont = _container
        Else
            oCont = New HandelingUnit(ContainerType)
        End If
        Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
        Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
        Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)

        Dim dContAvailableCude As Decimal = oCont.CubeCapacity - _currvolume
        Dim dContAvailableWeight As Decimal = oCont.WeightCapacity - _currweight

        If Not oLogger Is Nothing Then
            oLogger.Write("Inventory cube  - " + _currvolume.ToString() + " Inventory weight - " + _currweight.ToString())
            oLogger.Write("Available cube  - " + dContAvailableCude.ToString() + " Available weight - " + dContAvailableWeight.ToString())
        End If

        If pPickPartialUOM Then
            NumUnits = dContAvailableCude / SingleUnitVolume
        Else
            NumUnits = Math.Floor(Math.Floor(dContAvailableCude / oSku.ConvertToUnits(pck.UOM)) * oSku.ConvertToUnits(pck.UOM) / SingleUnitVolume)
        End If
        If NumUnits > Math.Floor(dContAvailableWeight / SingleUnitWeight) Then
            If pPickPartialUOM Then
                NumUnits = dContAvailableWeight / SingleUnitWeight
            Else
                NumUnits = Math.Floor(Math.Floor(dContAvailableWeight / oSku.ConvertToUnits(pck.UOM)) * oSku.ConvertToUnits(pck.UOM) / SingleUnitWeight)
            End If
            Return NumUnits
        Else
            Return NumUnits
        End If
    End Function


    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End


    Public Function CanPlaceDirect(ByVal pck As PicksObject) As Boolean
        If Me.Count = 0 Then Return True
        If Me(0).LoadId = pck.LoadId Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function BreakFull(ByVal pDeliveryLocation As String, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim brfull As New ArrayList
        'Dim fullpck As PicksObjectCollection
        Dim fullPckByWhA As New System.Collections.Generic.Dictionary(Of String, System.Collections.Generic.List(Of WMS.Logic.PicksObjectCollection))
        Dim pck As PicksObject
        Dim Placed As Boolean
        For Each pck In Me
            Placed = False
            If pck.PickType = WMS.Lib.PICKTYPE.FULLPICK Or pck.PickMethod = WMS.Lib.PickMethods.PickMethod.PICKBYITEM Then
                pck.DeliveryLocation = pDeliveryLocation
                'Start RWMS-768
                If Not oLogger Is Nothing Then
                    oLogger.Write("BreakFull- DeliveryLocation: " & pck.DeliveryLocation)
                End If
                'End  RWMS-768
                If Not fullPckByWhA.ContainsKey(pck.FromWarehousearea) Then
                    fullPckByWhA.Add(pck.FromWarehousearea, New System.Collections.Generic.List(Of WMS.Logic.PicksObjectCollection))
                End If
                'For Each fullpck In brfull
                For Each fullpck As PicksObjectCollection In fullPckByWhA(pck.FromWarehousearea)
                    If fullpck.CanPlaceDirect(pck) Then
                        fullpck.Add(pck)
                        Placed = True
                        'Start RWMS-768
                        If Not oLogger Is Nothing Then
                            oLogger.Write("BreakFull-CanPlaceDirect pck - SKU:" & pck.SKU & ",Units:" & pck.Units.ToString())
                        End If
                        'End  RWMS-768
                        Exit For
                    End If
                Next
                If Not Placed Then
                    'fullpck = New PicksObjectCollection
                    Dim fullpck As New PicksObjectCollection
                    fullpck.Add(pck)
                    fullPckByWhA(pck.FromWarehousearea).Add(fullpck)
                    'Start RWMS-768
                    If Not oLogger Is Nothing Then
                        oLogger.Write("BreakFull-Fullpick by Warehouse, pck - SKU:" & pck.SKU & ",Units:" & pck.Units.ToString())
                    End If
                    'End  RWMS-768
                    'brfull.Add(fullpck)
                End If
            End If
        Next

        For Each list As System.Collections.Generic.List(Of WMS.Logic.PicksObjectCollection) In fullPckByWhA.Values
            For Each coll As WMS.Logic.PicksObjectCollection In list
                brfull.Add(coll)
            Next
        Next

        For Each fullpck As WMS.Logic.PicksObjectCollection In brfull
            For Each pck In fullpck
                Me.Remove(pck)
            Next
        Next

        If brfull.Count > 0 Then
            Return brfull
        Else
            Return Nothing
        End If
    End Function

    Public Function BreakNPP(ByVal pDeliveryLocation As String, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim brNPP As New ArrayList
        Dim NPPpck As PicksObjectCollection
        Dim pck As PicksObject
        Dim Placed As Boolean
        For Each pck In Me
            Placed = False
            If pck.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
                pck.DeliveryLocation = pDeliveryLocation
                For Each NPPpck In brNPP
                    If NPPpck.CanPlaceDirect(pck) Then
                        NPPpck.Add(pck)
                        Placed = True
                        Exit For
                    End If
                Next
                If Not Placed Then
                    NPPpck = New PicksObjectCollection
                    NPPpck.Add(pck)
                    brNPP.Add(NPPpck)
                End If
            End If
        Next

        For Each NPPpck In brNPP
            For Each pck In NPPpck
                Me.Remove(pck)
            Next
        Next

        If brNPP.Count > 0 Then
            Return brNPP
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Overrides"
    Public Function Add(ByVal value As PicksObject) As Integer
        Return _pck.Add(value)
    End Function

    Public Sub AddRange(ByVal picks As PicksObjectCollection)
        If Not picks Is Nothing Then
            If picks.Count > 0 Then
                _pck.AddRange(picks)
            End If
        End If
    End Sub

    Public Sub Clear()
        _pck.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As PicksObject)
        _pck.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As PicksObject)
        _pck.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _pck.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _pck.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _pck.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _pck.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _pck.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _pck.GetEnumerator()
    End Function

    Public Sub Sort(ByVal pPickListBaseVolume As Decimal, ByVal pPickListBaseWeight As Decimal)
        Dim tmpAl As New ArrayList, tmp2Al As New ArrayList
        _pck.Sort(New ClassSorter("SortOrder", SortDirection.Ascending))
        If pPickListBaseVolume <= 0 And pPickListBaseWeight <= 0 Then
            Exit Sub
        End If
        For Each oPicksObj As PicksObject In _pck
            Dim oSku As SKU = Planner.GetSKU(oPicksObj.Consignee, oPicksObj.SKU)
            If Not oSku Is Nothing Then
                Select Case oSku.BASEITEM
                    Case SKU.BaseItemTypes.ALWAYS
                        tmpAl.Add(oPicksObj)
                    Case SKU.BaseItemTypes.NEVER
                        tmp2Al.Add(oPicksObj)
                    Case Else
                        If oPicksObj.Volume >= pPickListBaseVolume Or oPicksObj.Weight >= pPickListBaseWeight Then
                            tmpAl.Add(oPicksObj)
                        Else
                            tmp2Al.Add(oPicksObj)
                        End If
                End Select
            End If
        Next
        tmpAl.Sort(New ClassSorter("Volume", SortDirection.Descending))
        tmp2Al.Sort(New ClassSorter("SortOrder", SortDirection.Ascending))
        _pck.Clear()
        _pck.AddRange(tmpAl)
        _pck.AddRange(tmp2Al)
    End Sub


#End Region

End Class

<CLSCompliant(False)> Public Class PicksObject

#Region "Variables"
    Protected _orderid As String
    Protected _consignee As String
    Protected _company As String
    Protected _companytype As String
    Protected _loadid As String
    Protected _sku As String
    Protected _warehousearea As String
    Protected _pickregion As String
    Protected _units As Double
    Protected _uom As String
    Protected _fromlocation As String
    Protected _fromwarehousearea As String
    Protected _tolocation As String
    Protected _towarehousearea As String
    Protected _wave As String
    Protected _pickmethod As String
    Protected _orderline As Int32
    Protected _uomqty As Double
    Protected _locationsortorder As String
    Protected _skusortorder As String
    Protected _tocontainer As String
    Protected _containertype As String
    Protected _picktype As String
    Protected _packarea As String
    'Protected _prestaginglocation As String
    Protected _deliverylocation As String
    Protected _deliverywarehousearea As String
    Protected _strategyid As String
    Protected _volume As Decimal
    Protected _weight As Decimal
    Protected _ispicklocalloc As Boolean
    Protected _isoveralloc As Boolean
    '' Used later in pickdetail creation and order detail allocation
    Protected _subSKUconversionUnits As Decimal = 1

#End Region

#Region "Constructor"
    Public Sub New()
    End Sub
#End Region

#Region "Properties"
    Public Property OrderId() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
        End Set
    End Property
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
    Public Property Company() As String
        Get
            Return _company
        End Get
        Set(ByVal Value As String)
            _company = Value
        End Set
    End Property
    Public Property CompanyType() As String
        Get
            Return _companytype
        End Get
        Set(ByVal Value As String)
            _companytype = Value
        End Set
    End Property
    Public Property SKU() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property
    Public Property WarehouseArea() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
        End Set
    End Property
    Public Property PickRegion() As String
        Get
            Return _pickregion
        End Get
        Set(ByVal Value As String)
            _pickregion = Value
        End Set
    End Property
    Public Property Units() As Double
        Get
            Return _units
        End Get
        Set(ByVal Value As Double)
            _units = Value
        End Set
    End Property
    Public Property Volume() As Decimal
        Get
            Return _volume
        End Get
        Set(ByVal Value As Decimal)
            _volume = Value
        End Set
    End Property
    Public Property Weight() As Decimal
        Get
            Return _weight
        End Get
        Set(ByVal Value As Decimal)
            _weight = Value
        End Set
    End Property
    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property
    Public Property FromLocation() As String
        Get
            Return _fromlocation
        End Get
        Set(ByVal Value As String)
            _fromlocation = Value
        End Set
    End Property
    Public Property ToLocation() As String
        Get
            Return _tolocation
        End Get
        Set(ByVal Value As String)
            _tolocation = Value
        End Set
    End Property
    Public Property FromWarehousearea() As String
        Get
            Return _fromwarehousearea
        End Get
        Set(ByVal Value As String)
            _fromwarehousearea = Value
        End Set
    End Property
    Public Property ToWarehousearea() As String
        Get
            Return _towarehousearea
        End Get
        Set(ByVal Value As String)
            _towarehousearea = Value
        End Set
    End Property
    Public Property Wave() As String
        Get
            Return _wave
        End Get
        Set(ByVal Value As String)
            _wave = Value
        End Set
    End Property
    Public Property PickMethod() As String
        Get
            Return _pickmethod
        End Get
        Set(ByVal Value As String)
            _pickmethod = Value
        End Set
    End Property
    Public Property OrderLine() As Int32
        Get
            Return _orderline
        End Get
        Set(ByVal Value As Int32)
            _orderline = Value
        End Set
    End Property
    Public Property LocationSortOrder() As String
        Get
            Return _locationsortorder
        End Get
        Set(ByVal Value As String)
            _locationsortorder = Value
        End Set
    End Property
    Public Property SkuSortOrder() As String
        Get
            Return _skusortorder
        End Get
        Set(ByVal Value As String)
            _skusortorder = Value
        End Set
    End Property
    Public Property ToContainer() As String
        Get
            Return _tocontainer
        End Get
        Set(ByVal Value As String)
            _tocontainer = Value
        End Set
    End Property
    Public Property ContainerType() As String
        Get
            Return _containertype
        End Get
        Set(ByVal Value As String)
            _containertype = Value
        End Set
    End Property

    Public Property PickType() As String
        Get
            Return _picktype
        End Get
        Set(ByVal Value As String)
            _picktype = Value
        End Set
    End Property

    Public ReadOnly Property SortOrder() As String
        Get
            Dim so As String = _skusortorder & _locationsortorder & _consignee & _sku & _orderid & _orderline
            If _isoveralloc Then
                so = so & "ZZZZZZZ"
            End If
            Return so
        End Get
    End Property

    Public Property PackArea() As String
        Get
            Return _packarea
        End Get
        Set(ByVal Value As String)
            _packarea = Value
        End Set
    End Property

    Public Property DeliveryLocation() As String
        Get
            Return _deliverylocation
        End Get
        Set(ByVal Value As String)
            _deliverylocation = Value
        End Set
    End Property

    Public Property DeliveryWarehousearea() As String
        Get
            Return _deliverywarehousearea
        End Get
        Set(ByVal Value As String)
            _deliverywarehousearea = Value
        End Set
    End Property

    Public Property StrategyId() As String
        Get
            Return _strategyid
        End Get
        Set(ByVal Value As String)
            _strategyid = Value
        End Set
    End Property

    Public Property IsPickLocAllocation() As Boolean
        Get
            Return _ispicklocalloc
        End Get
        Set(ByVal Value As Boolean)
            _ispicklocalloc = Value
        End Set
    End Property

    Public Property IsOverAllocation() As Boolean
        Get
            Return _isoveralloc
        End Get
        Set(ByVal Value As Boolean)
            _isoveralloc = Value
        End Set
    End Property

    Public Property SubSKUConversionUnits() As Decimal
        Get
            Return _subSKUconversionUnits
        End Get
        Set(ByVal value As Decimal)
            _subSKUconversionUnits = value
        End Set
    End Property

#End Region

#Region "Methods"

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Sub ExportToLog(ByVal oLogger As LogHandler)
        If Not oLogger Is Nothing Then
            Try
                oLogger.Write("Exporting PicksObject - OrderId = " + Me.OrderId + "OrderLine = " + Me.OrderLine.ToString() + " SKU = " + Me.SKU + " Company = " + Me.Company + " Consignee = " + Me.Consignee)
            Catch ex As Exception
                oLogger.Write(ex.ToString())
            End Try
        End If
    End Sub

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
    Public Function MatchBreaker(ByVal uom As String, ByVal pickregion As String, ByVal containertype As String, ByVal plancontainer As Boolean, ByVal packarea As String, ByVal deliverylocation As String, ByVal deliverywarehousearea As String, ByVal pPicktype As String, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        If Not uom = "*" And Not uom = "%" And Not uom Is Nothing And Not uom = "" And Not uom.ToUpper = Me.UOM.ToUpper Then
            If Not oLogger Is Nothing Then
                oLogger.Write(" PicksObject.MatchBreaker - Pick line UOM does not match  UOM defined in policy. (policy UOM = " & Me.UOM & " ,pick line UOM = " & uom & ".") ''RWMS-1485
            End If
            Return False
        End If

        If Not pickregion = "" And Not pickregion Is Nothing And Not pickregion = "*" And Not pickregion = "%" And Not pickregion.ToUpper = Me.PickRegion.ToUpper Then
            If Not oLogger Is Nothing Then
                oLogger.Write(" PicksObject.MatchBreaker - Pick line pickregion does not match PickRegion defined in policy. (policy PickRegion = " & Me.PickRegion & " ,pick line PickRegion=" & pickregion) ''RWMS-1485
            End If
            Return False
        End If

        If Not pPicktype = "" And Not pPicktype Is Nothing And Not pPicktype = "*" And Not pPicktype = "%" And Not pPicktype.ToUpper = Me.PickType.ToUpper Then
            If Not oLogger Is Nothing Then
                oLogger.Write(" PicksObject.MatchBreaker - Pick line PickType does not match with PickType defined in policy. (Policy PickType = " & Me.PickType & ",pick line PickType=" & pPicktype)  'RWMS-1485
            End If
            Return False
        End If
        'Added for RWMS-1899 and RWMS-1798 Start
        If Not oLogger Is Nothing Then
            oLogger.Write("PicksObject.MatchBreaker - PickList Breaker matched the cuurent line. will set delivery location to - " & deliverylocation)
            oLogger.Write("PicksObject.MatchBreaker - PickList Breaker matched the cuurent line. will set delivery warehousearea to - " & deliverywarehousearea)
        End If
        'Added for RWMS-1899 and RWMS-1798 End

        Me.PackArea = packarea
        Me.DeliveryLocation = deliverylocation
        Me.DeliveryWarehousearea = deliverywarehousearea
        Return True
    End Function

    Public Function Clone() As PicksObject
        Dim retPck As New PicksObject
        retPck.Consignee = _consignee
        retPck.Company = _company
        retPck.CompanyType = _companytype
        retPck.ContainerType = _containertype
        retPck.DeliveryLocation = _deliverylocation
        retPck.FromLocation = _fromlocation
        retPck.DeliveryWarehousearea = _deliverywarehousearea
        retPck.FromWarehousearea = _fromwarehousearea
        retPck.LoadId = _loadid
        retPck.LocationSortOrder = _locationsortorder
        retPck.OrderId = _orderid
        retPck.OrderLine = _orderline
        retPck.PackArea = _packarea
        retPck.PickMethod = _pickmethod
        retPck.PickRegion = _pickregion
        retPck.PickType = _picktype
        retPck.SKU = _sku
        retPck.SkuSortOrder = _skusortorder
        retPck.StrategyId = _strategyid
        retPck.ToContainer = _tocontainer
        retPck.ToLocation = _tolocation
        retPck.ToWarehousearea = _towarehousearea
        retPck.Units = _units
        retPck.UOM = _uom
        retPck.WarehouseArea = _warehousearea
        retPck.Wave = _wave
        retPck.IsPickLocAllocation = _ispicklocalloc
        Return retPck
    End Function
#End Region

End Class

#Region "Comparer"

<CLSCompliant(False)> Public Class ClassSorter
    Implements IComparer

    Protected _sortBy As String
    Protected _sortDirection As SortDirection

    Public Sub New(ByVal sortBy As String, ByVal sortDirection As SortDirection)
        _sortBy = sortBy
        _sortDirection = sortDirection
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object, ByVal Comparer As String) As Integer
        Dim icx, icy As IComparable
        icx = x.GetType().GetProperty(Comparer).GetValue(x, Nothing)
        icy = y.GetType().GetProperty(Comparer).GetValue(y, Nothing)
        If (_sortDirection = SortDirection.Descending) Then
            Return icy.CompareTo(icx)
        Else
            Return icx.CompareTo(icy)
        End If
    End Function

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Int32 Implements System.Collections.IComparer.Compare
        Return Compare(x, y, _sortBy)
    End Function

End Class

#End Region

Public Enum SortDirection
    Ascending = 0
    Descending = 1
End Enum