Imports Made4Net.DataAccess
Imports System.Text

<CLSCompliant(False)> Public Class PicklistBreakerCollection
    Implements ICollection

#Region "Variables"
    Dim _strategybreaker As ArrayList
    Dim _planstrategyid As String
#End Region

#Region "Constructor"

    Public Sub New(ByVal PlanStrategyId As String)
        _strategybreaker = New ArrayList
        _planstrategyid = PlanStrategyId
        Load()
    End Sub

#End Region

#Region "Properties"
    Default Public Property Item(ByVal index As Int32) As PicklistBreaker
        Get
            Return CType(_strategybreaker(index), PicklistBreaker)
        End Get
        Set(ByVal Value As PicklistBreaker)
            _strategybreaker(index) = Value
        End Set
    End Property
#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from planstrategybreak Where strategyid='{0}' order by priority asc", _planstrategyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim pckBreakDetail As PicklistBreaker
        For Each dr In dt.Rows
            pckBreakDetail = New PicklistBreaker(dr)
            Add(pckBreakDetail)
        Next
    End Sub

    Public Function BreakPartial(ByVal Picks As PicksObjectCollection, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, Optional ByVal ShouldBreakForLoadingPlan As Boolean = False, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim br As New ArrayList
        Dim breakedline As ArrayList
        Dim pck As PicksObjectCollection
        Dim pckbreakline As PicklistBreaker
        Dim strLoggerBuild As New StringBuilder

        strLoggerBuild.Append(" PicklistBreakerCollection.BreakPartial - Commencing Break Partial Calculations for " + Me.Count.ToString() + " breaklines " + vbCrLf) 'RWMS-1485

        For Each pckbreakline In Me

            strLoggerBuild.Append("     PicklistBreakerCollection.BreakPartial - Begin Breakline calculations for strategy " + pckbreakline.StrategyId + vbCrLf) 'RWMS-1485

            breakedline = pckbreakline.BreakPartail(Picks, pPickPartialUOM, pBaseCube, pBaseWeight, oLogger, ShouldBreakForLoadingPlan)

            strLoggerBuild.Append("     PicklistBreakerCollection.BreakPartial - End Breakline calculations for strategy " + pckbreakline.StrategyId + vbCrLf) 'RWMS-1485

            If Not breakedline Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(" PicklistBreakerCollection.BreakPartial - Number of picks added for this break: " + breakedline.Count.ToString() + vbCrLf) 'RWMS-1485
                End If
                For Each pck In breakedline
                    br.Add(pck)
                Next
            End If
        Next
        strLoggerBuild.Append(" PicklistBreakerCollection.BreakPartial - End Of Break Partial Calculations") 'RWMS-1485
        If Not oLogger Is Nothing Then
            oLogger.Write("") 'RWMS-1485
            oLogger.Write(strLoggerBuild.ToString())
            oLogger.Write("")
        End If
        If br.Count > 0 Then
            Return br
        Else
            Return Nothing
        End If
    End Function

    Public Function BreakFull(ByVal Picks As PicksObjectCollection, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim br As ArrayList
        Dim pcks As PicksObjectCollection
        Dim pck As PicksObject
        Dim pckbreakline As PicklistBreaker
        'Get a delivery location if exists from break strategy
        Dim sDeliveryLocation As String = String.Empty
        For Each pckbreakline In Me
            If pckbreakline.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                sDeliveryLocation = pckbreakline.DeliveryLocation
                Exit For
            End If
        Next
        'Commented for PWMS-807 Start
        'Return Picks.BreakFull(sDeliveryLocation, oLogger)
        'Commented for PWMS-807 End

        'Added for PWMS-807 Start
        Return Picks.BreakFull(sDeliveryLocation)
        'Added for PWMS-807 End

    End Function

    Public Function BreakNPP(ByVal Picks As PicksObjectCollection, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim br As ArrayList
        Dim pcks As PicksObjectCollection
        Dim pck As PicksObject
        Dim pckbreakline As PicklistBreaker
        'Get a delivery location if exists from break strategy
        Dim sDeliveryLocation As String = String.Empty
        For Each pckbreakline In Me
            If pckbreakline.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
                sDeliveryLocation = pckbreakline.DeliveryLocation
                Exit For
            End If
        Next
        Return Picks.BreakNPP(sDeliveryLocation)
    End Function

    Public Function BreakParallel(ByVal Picks As PicksObjectCollection, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, Optional ByVal ShouldBreakForLoadingPlan As Boolean = False, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim br As New ArrayList
        Dim breakedline As ArrayList
        Dim pck As PicksObjectCollection
        Dim pckbreakline As PicklistBreaker
        For Each pckbreakline In Me
            breakedline = pckbreakline.BreakParallel(Picks, pPickPartialUOM, pBaseCube, pBaseWeight, oLogger, ShouldBreakForLoadingPlan)
            If Not breakedline Is Nothing Then
                For Each pck In breakedline
                    br.Add(pck)
                Next
            End If
        Next
        If br.Count > 0 Then
            Return br
        Else
            Return Nothing
        End If
    End Function

    Public Sub DeleteAll()
        Dim sql As String = String.Format("Delete from planstrategybreak where strategyid={0}", Made4Net.Shared.FormatField(_planstrategyid))
        DataInterface.RunSQL(sql)
        Me.Clear()
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As PicklistBreaker) As Integer
        Return _strategybreaker.Add(value)
    End Function

    Public Sub Clear()
        _strategybreaker.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As PicklistBreaker)
        _strategybreaker.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As PicklistBreaker)
        _strategybreaker.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _strategybreaker.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _strategybreaker.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _strategybreaker.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _strategybreaker.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _strategybreaker.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _strategybreaker.GetEnumerator()
    End Function

#End Region

End Class

<CLSCompliant(False)> Public Class PicklistBreaker

#Region "Variables"

    Protected _strategyid As String
    Protected _priority As Int32
    Protected _pickregion As String
    Protected _picktype As String
    Protected _uom As String
    Protected _container As String
    Protected _plancontainer As Boolean
    'Added for PWMS-807 Start
    Protected _splitline As Boolean
    'Added for PWMS-807 End
    Protected _packarea As String
    Protected _deliverylocation As String
    Protected _deliverywarehousearea As String
    Protected _consignee As String
    Protected _company As String
    Protected _companytype As String
    Protected _weight As Decimal
    Protected _cube As Decimal
    Protected _minplannedcube As Decimal

    Protected _ergheavyth As Decimal
    Protected _ergwgtlimit As Decimal
    Protected _ergcubelimit As Decimal
    Protected _ergwgtlimitallheavy As Decimal
    Protected _ergcubelimitallheavy As Decimal
    Protected _nosplitlinewgtpct As Decimal
    Protected _nosplitlinecubepct As Decimal
    Protected _allowmultibaseitems As Boolean
    Protected _extendwalkforfillpct As Decimal
    Protected _allowgoback As Boolean

    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"
    Public Property StrategyId() As String
        Get
            Return _strategyid
        End Get
        Set(ByVal Value As String)
            _strategyid = Value
        End Set
    End Property
    Public Property Priority() As Int32
        Get
            Return _priority
        End Get
        Set(ByVal Value As Int32)
            _priority = Value
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
    Public Property PickType() As String
        Get
            Return _picktype
        End Get
        Set(ByVal Value As String)
            _picktype = Value
        End Set
    End Property
    Public Property Uom() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property
    Public Property Container() As String
        Get
            Return _container
        End Get
        Set(ByVal Value As String)
            _container = Value
        End Set
    End Property
    Public Property PlanContainer() As Boolean
        Get
            Return _plancontainer
        End Get
        Set(ByVal Value As Boolean)
            _plancontainer = Value
        End Set
    End Property

    'Added for PWMS-807 Start
    Public Property SplitLine() As Boolean
        Get
            Return _splitline
        End Get
        Set(ByVal Value As Boolean)
            _splitline = Value
        End Set
    End Property

    'Added for PWMS-807 End
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

    Public Property Weight() As Decimal
        Get
            Return _weight
        End Get
        Set(ByVal Value As Decimal)
            _weight = Value
        End Set
    End Property

    Public Property Cube() As Decimal
        Get
            Return _cube
        End Get
        Set(ByVal Value As Decimal)
            _cube = Value
        End Set
    End Property

    Public Property MinPlannedCube() As Decimal
        Get
            Return _minplannedcube
        End Get
        Set(ByVal value As Decimal)
            _minplannedcube = value
        End Set
    End Property

    Public Property ErgoHeavyTreshHold() As Decimal
        Get
            Return _ergheavyth
        End Get
        Set(ByVal value As Decimal)
            _ergheavyth = value
        End Set
    End Property

    'the weight limit for all heavy items on a pallet contains heavy and light items.
    Public Property ErgoWeightLimit() As Decimal
        Get
            Return _ergwgtlimit
        End Get
        Set(ByVal value As Decimal)
            _ergwgtlimit = value
        End Set
    End Property

    'the cube limit for all heavy items on a pallet contains heavy and light items.
    Public Property ErgoCubeLimit() As Decimal
        Get
            Return _ergcubelimit
        End Get
        Set(ByVal value As Decimal)
            _ergcubelimit = value
        End Set
    End Property

    'the weight limit for all heavy items on a pallet contains heavy items only.
    Public Property ErgoWeightLimitAllHeavy() As Decimal
        Get
            Return _ergwgtlimitallheavy
        End Get
        Set(ByVal value As Decimal)
            _ergwgtlimitallheavy = value
        End Set
    End Property

    'the cube limit for all heavy items on a pallet contains heavy items only.
    Public Property ErgoCubeLimitAllHeavy() As Decimal
        Get
            Return _ergcubelimitallheavy
        End Get
        Set(ByVal value As Decimal)
            _ergcubelimitallheavy = value
        End Set
    End Property

    Public Property NoSplitLineWeightPCT() As Decimal
        Get
            Return _nosplitlinewgtpct
        End Get
        Set(ByVal value As Decimal)
            _nosplitlinewgtpct = value
        End Set
    End Property

    Public Property NoSplitLineCubePCT() As Decimal
        Get
            Return _nosplitlinecubepct
        End Get
        Set(ByVal value As Decimal)
            _nosplitlinecubepct = value
        End Set
    End Property

    'If not allowed then all base items will open a new container
    Public Property AllowGoBack() As Boolean
        Get
            Return _allowgoback
        End Get
        Set(ByVal value As Boolean)
            _allowgoback = value
        End Set
    End Property

    Public Property ExtendWalkForFillPCT() As Decimal
        Get
            Return _extendwalkforfillpct
        End Get
        Set(ByVal value As Decimal)
            _extendwalkforfillpct = value
        End Set
    End Property

    'Can 2 or more base items can be placed on the same container
    Public Property AllowMultiBaseItems() As Boolean
        Get
            Return _allowmultibaseitems
        End Get
        Set(ByVal value As Boolean)
            _allowmultibaseitems = value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property
    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property
    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property
    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal dr As DataRow)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        _pickregion = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKREGION"), "")).Replace("*", "%")
        _picktype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKTYPE"), "")).Replace("*", "%")
        _uom = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UOM"), "")).Replace("*", "%")
        _container = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTAINER"))
        _plancontainer = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PLANCONTAINER"), False)
        'Added for PWMS-807 Start
        _splitline = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SPLITLINE"), False)
        'Added for PWMS-807 End

        _weight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WEIGHT"), 0)
        _cube = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CUBE"), 0)
        _minplannedcube = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MINPLANNEDCUBE"), 0)

        '_packarea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PACKAREA"))
        _deliverylocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DeliveryLocation"))
        _deliverywarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DeliveryWarehousearea"))
        _ergheavyth = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ERGHEAVYTH"))
        _ergwgtlimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ERGWGTLIMIT"))
        _ergcubelimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ERGCUBELIMIT"))
        _ergwgtlimitallheavy = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ERGWGTLIMITALLHEAVY"))
        _ergcubelimitallheavy = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ERGCUBELIMITALLHEAVY"))
        _nosplitlinewgtpct = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NOSPLITLINEWGTPCT"), 0)
        _nosplitlinecubepct = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NOSPLITLINECUBEPCT"), 0)
        _extendwalkforfillpct = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXTENDWALKFORFILLPCT"), 1)
        _allowmultibaseitems = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ALLOWMULTIBASEITEMS"))
        _allowgoback = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ALLOWGOBACK"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

#End Region

#Region "Method"

#Region "Partial Breaking"


    'Added for PWMS-807 Start
    Dim _PreviousCubeCapacity As Decimal
    Dim _PreviousWeightCapacity As Decimal
    'Added for PWMS-807 End


    Public Function BreakPartail(ByVal Picks As PicksObjectCollection, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, ByVal oLogger As LogHandler, Optional ByVal ShouldBreakForLoadingPlan As Boolean = False) As ArrayList
        Dim pckcols As New ArrayList
        Dim pckColsByWhA As New System.Collections.Generic.Dictionary(Of String, PicksObjectCollection)
        Dim pickListCollection As PicksObjectCollection
        Dim pck As PicksObject
        Dim pickListCollectionArray As New ArrayList

        'Break the picks by Warehouse
        For Each pck In Picks
            If pck.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
                If pck.MatchBreaker(_uom, _pickregion, _container, _plancontainer, _packarea, _deliverylocation, _deliverywarehousearea, _picktype, oLogger) Then
                    'Vlidate Customer's mix picking flag
                    If Not pckColsByWhA.ContainsKey(pck.FromWarehousearea) Then
                        pckColsByWhA.Add(pck.FromWarehousearea, New WMS.Logic.PicksObjectCollection)
                    End If
                    If pckColsByWhA(pck.FromWarehousearea).Count > 0 AndAlso ValidateCustomerMixPicking(pck) Then
                        If pck.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then pck.ContainerType = _container
                        pckColsByWhA(pck.FromWarehousearea).Add(pck)
                    Else
                        _consignee = pck.Consignee
                        _company = pck.Company
                        _companytype = pck.CompanyType
                        If pck.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then pck.ContainerType = _container
                        pckColsByWhA(pck.FromWarehousearea).Add(pck)
                    End If
                End If
            End If
        Next

        'Remove the pick lines from Picks collection which are moved to pckColsByWhA
        For Each pckcol As WMS.Logic.PicksObjectCollection In pckColsByWhA.Values
            For Each pck In pckcol
                Picks.Remove(pck)
            Next
        Next
        Dim placed As Boolean
        If pckColsByWhA.Values.Count = 0 Then
            Return Nothing
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write(" PicklistBreaker.BreakPartail - Plan container flag set to " + _plancontainer.ToString()) 'RWMS-1485
        End If


        'Loop through each pick line and add to the picklist by utilizing the picklist break rules
        For Each pckcol As WMS.Logic.PicksObjectCollection In pckColsByWhA.Values

            ''Start Added for RWMS-1279  -Planner service not planning end of pick
            For Each pck In pckcol
                'calc for the first PL to use considering the EXTENDWALKFORFILLPCT param --> + 0.5 to round up
                Dim FirstContainerIndex As Int32 = pckcols.Count - (pckcols.Count * _extendwalkforfillpct + 0.5)
                Dim pContainerType As String = Nothing
                If (_plancontainer = True) Then
                    pContainerType = pck.ContainerType
                End If
                'go the last picklist and keep adding picks to that picklist to maintain locatio sort order
                'If (pckcols.Count > 0) Then
                '    Dim index As Integer = pckcols.Count - 1
                '    If (index >= 0) And (index >= FirstContainerIndex) Then
                '        picklistCollection = pckcols(index)
                '    End If
                'End If

                'If picklist does not exist , create a new picklist
                If (pickListCollection Is Nothing) Then
                    Dim newPickListCollection As PicksObjectCollection = New PicksObjectCollection
                    If Not oLogger Is Nothing Then
                        oLogger.Write(" PicklistBreaker.BreakPartail - Starting a new PICKLIST to start adding pick lines,pick line sku:" + pck.SKU.ToString()) 'RWMS-1485
                    End If
                    pickListCollection = newPickListCollection
                End If
                'Add pick line to the picklist
                pickListCollectionArray = AddPickLineToPickList(pckcols, pickListCollection, pck, pContainerType, oLogger)

                'Add the all the returned picklist collection to the return result
                If pickListCollectionArray.Count > 0 Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write(" PicklistBreaker.BreakPartail - Number of PICKLISTs returned =" + pickListCollectionArray.Count.ToString()) 'RWMS-1485
                    End If
                    For Each pcklist As WMS.Logic.PicksObjectCollection In pickListCollectionArray
                        If Not oLogger Is Nothing Then
                            oLogger.Write(" PicklistBreaker.BreakPartail - Adding the closed PICKLIST to picklist collection Array:" + "PICKLIST Total CubeVol=" + pcklist.CurrVolume.ToString() + ",PICKLIST Total Wegith=" + pcklist.CurrWeight.ToString()) 'RWMS-1485
                        End If
                        pckcols.Add(pcklist)
                    Next
                End If


            Next
            'Commented for RWMS-1798 Start   
            'If pickListCollectionArray.Count <= 0 Then   
            'Commented for RWMS-1798 End   

            'Added for RWMS-1899 and RWMS-1798 Start   
            If pickListCollectionArray.Count > 0 Then
                If (pickListCollection.Count > 0 And pickListCollectionArray.Contains(pickListCollection) = False) Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Adding the last PICKLIST to picklist collection Array:" + "PICKLIST Total CubeVol=" + pickListCollection.CurrVolume.ToString() + ",PICKLIST Total Wegith=" + pickListCollection.CurrWeight.ToString())
                    End If
                    pckcols.Add(pickListCollection)
                End If
            Else
                'Added for RWMS-1899 and RWMS-1798 End   

                If Not oLogger Is Nothing Then
                    oLogger.Write(" PicklistBreaker.BreakPartail - Number of picklines added to this PICKLIST =" + pickListCollection.Count.ToString()) 'RWMS-1485
                End If
                If (pickListCollection.Count > 0) Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write(" PicklistBreaker.BreakPartail - Adding the closed PICKLIST to picklist collection Array:" + "PICKLIST Total CubeVol=" + pickListCollection.CurrVolume.ToString() + ",PICKLIST Total Wegith=" + pickListCollection.CurrWeight.ToString()) 'RWMS-1485
                    End If
                    pckcols.Add(pickListCollection)
                End If

            End If
            ''End Added for RWMS-1279  -Planner service not planning end of pick
        Next
        Return pckcols

    End Function

    ''Start  RWMS-1279  -Planner service not planning end of pick
    Private Function AddPickLineToPickList(ByRef pckcols As ArrayList, ByRef picklistCollection As PicksObjectCollection, ByVal pck As PicksObject, Optional ByVal pContType As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim picklistAL As New ArrayList
        Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
        Dim skuUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
        Dim skuUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
        Dim numUnitsCanFill As Decimal
        Dim unitsToBeFullfilled As Integer
        unitsToBeFullfilled = pck.Units

        If Not oLogger Is Nothing Then
            'Start RWMS-1485
            oLogger.writeSeperator(" ", 20)
            oLogger.Write(" PicklistBreaker.AddPickLineToPickList - Pick Line in process SKU= " + pck.SKU + ",consignee=" + pck.Consignee.ToString() + ",Qty=" + pck.Units.ToString() + ",skuUnitVol=" + skuUnitVolume.ToString() + ",skuUnitWeight=" + skuUnitWeight.ToString())
            Dim strLogString As String = " PicklistBreaker.AddPickLineToPickList - Break Startegy Params - "
            strLogString = strLogString + " StrategyID = " & _strategyid.ToString()
            'End RWMS-1485
            strLogString = strLogString + " ,Cube = " & _cube.ToString()
            strLogString = strLogString + " ,Weight = " & _weight.ToString()
            strLogString = strLogString + " ,Split Line Flag = " & SplitLine.ToString()
            strLogString = strLogString + " ,No Split Line Cube Pct = " & _nosplitlinecubepct.ToString()
            strLogString = strLogString + " ,No Split Line Weight Pct = " & _nosplitlinecubepct.ToString()
            oLogger.Write(strLogString)
        End If
        Try
            While (unitsToBeFullfilled > 0)
                If Not oLogger Is Nothing Then
                    'Start RWMS-1485
                    oLogger.Write("                                          ")
                    oLogger.Write("     PicklistBreaker.AddPickLineToPickList - While condition, unitsToBeFullfilled =" + unitsToBeFullfilled.ToString())
                    'End RWMS-1485
                End If
                If (picklistCollection.Count > 0) Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Existing PickList Total CubeVol=" + picklistCollection.CurrVolume.ToString() + ",Total Weight=" + picklistCollection.CurrWeight.ToString()) 'RWMS-1485
                    End If
                    If HasExistingPickListHasRoomToFill(picklistCollection, pck, skuUnitVolume, skuUnitWeight, numUnitsCanFill, oLogger) = True Then
                        If Not oLogger Is Nothing Then
                            oLogger.Write("     PicklistBreaker.AddPickLineToPickList - HasExistingPickListHasRoomToFill returned TRUE and numUnitsCanFill =" + numUnitsCanFill.ToString()) 'RWMS-1485
                        End If
                        If (pck.Units <= numUnitsCanFill) Then
                            If Not oLogger Is Nothing Then
                                oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Pick line Units are less than estimated number of units can split") ''RWMS-1485
                            End If
                            picklistCollection.PlaceInNewPickList(pck, oLogger)
                            unitsToBeFullfilled = unitsToBeFullfilled - pck.Units
                        ElseIf (SplitLine = True) Then
                            Dim prtSplitPck1, prtSplitPck2 As PicksObject
                            prtSplitPck1 = pck.Clone
                            prtSplitPck2 = pck.Clone

                            prtSplitPck1.Units = numUnitsCanFill
                            prtSplitPck2.Units = pck.Units - prtSplitPck1.Units

                            If Not oLogger Is Nothing Then
                                oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Splitting Pick Line. First Split with Qty=" + prtSplitPck1.Units.ToString() + ",Remaing Qty=" + prtSplitPck2.Units.ToString()) ''RWMS-1485
                            End If
                            'Place the split units to picklist 
                            picklistCollection.PlaceInNewPickList(prtSplitPck1, oLogger)
                            unitsToBeFullfilled = unitsToBeFullfilled - prtSplitPck1.Units
                            pck = prtSplitPck2
                            unitsToBeFullfilled = prtSplitPck2.Units

                        Else
                            If Not oLogger Is Nothing Then
                                oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Ready to create new PICKLIST") ''RWMS-1485
                            End If
                            'Close the existing picklist and open new one 
                            If (picklistCollection.Count > 0) Then
                                picklistAL.Add(picklistCollection)
                                If Not oLogger Is Nothing Then
                                    oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Closing the existing PICKLIST and starting a new PICKLIST. " + "Picklist Total Vol=" + picklistCollection.CurrVolume.ToString() + ",Total Weight=" + picklistCollection.CurrWeight.ToString()) ''RWMS-1485
                                End If
                                Dim newPickListCollection As PicksObjectCollection = New PicksObjectCollection
                                picklistCollection = newPickListCollection
                            End If
                            SplitPickLineAndAddToPickList(picklistCollection, pck, skuUnitVolume, skuUnitWeight, unitsToBeFullfilled, oLogger)
                        End If


                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("     PicklistBreaker.AddPickLineToPickList - HasExistingPickListHasRoomToFill() returned FALSE") 'RWMS-1485
                        End If
                        'Close the existing picklist and open new one 
                        If (picklistCollection.Count > 0) Then
                            picklistAL.Add(picklistCollection)
                            If Not oLogger Is Nothing Then
                                oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Closing the existing PICKLIST and starting a new PICKLIST. " + "Picklist Total Vol=" + picklistCollection.CurrVolume.ToString() + ",Total Weight=" + picklistCollection.CurrWeight.ToString()) ''RWMS-1485
                            End If
                            Dim newPickListCollection As PicksObjectCollection = New PicksObjectCollection
                            picklistCollection = newPickListCollection
                        End If
                        SplitPickLineAndAddToPickList(picklistCollection, pck, skuUnitVolume, skuUnitWeight, unitsToBeFullfilled, oLogger)
                        'Commented for RWMS-1798 Start   
                        'If (picklistCollection.Count > 0) Then   
                        ' picklistAL.Add(picklistCollection)   

                        ' Dim newPickListCollection As PicksObjectCollection = New PicksObjectCollection   
                        ' picklistCollection = newPickListCollection   
                        'End If   

                        'Commented for RWMS-1798 End   

                    End If
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     PicklistBreaker.AddPickLineToPickList - No current PICKLIST exists. Prepared to create a new PICKLIST.")  'RWMS-1485
                    End If
                    'Close the existing picklist and open new one 
                    If (picklistCollection.Count > 0) Then
                        picklistAL.Add(picklistCollection)
                        If Not oLogger Is Nothing Then
                            oLogger.Write("     PicklistBreaker.AddPickLineToPickList - Closing the existing PICKLIST and starting a new PICKLIST." + "Picklist Total Vol=" + picklistCollection.CurrVolume.ToString() + ",Total Weight=" + picklistCollection.CurrWeight.ToString()) 'RWMS-1485
                        End If
                        Dim newPickListCollection As PicksObjectCollection = New PicksObjectCollection
                        picklistCollection = newPickListCollection
                    End If
                    'Create new picklist 
                    SplitPickLineAndAddToPickList(picklistCollection, pck, skuUnitVolume, skuUnitWeight, unitsToBeFullfilled, oLogger)

                End If

            End While
            'If (picklistCollection.Count > 0) Then
            '    picklistAL.Add(picklistCollection)
            'End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(" PicklistBreaker.AddPickLineToPickList - Exception occured,Pick Line in process sku:" + pck.SKU.ToString() + ",Error Msg=" + ex.Message.ToString()) 'RWMS-1485
            End If
        End Try


        If (Not picklistAL Is Nothing) Then
            Return picklistAL
        Else
            Return Nothing
        End If
    End Function
    ''End  RWMS-1279  -Planner service not planning end of pick

    ''Start  RWMS-1279  -Planner service not planning end of pick
    Private Function HasExistingPickListHasRoomToFill(ByVal picklistCollection As PicksObjectCollection, ByVal pck As PicksObject, ByVal skuUnitVolume As Decimal, ByVal skuUnitWeight As Decimal, ByRef numUnitsCanFill As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim cubeVolRemaining As Decimal
        Dim weightRemaining As Decimal
        Dim numUnitsByCube As Decimal
        Dim numUnitsByWeight As Decimal
        cubeVolRemaining = 0
        weightRemaining = 0

        'Determine the Cube volume remaining after deducting from the defined cube volume
        'Determine the weight remaining after deducting from the defined weight
        If (picklistCollection.Count <= 0) Then
            Return False
        End If
        If (_cube > 0) And (picklistCollection.CurrVolume > 0) Then
            If (_nosplitlinecubepct > 0) Then
                cubeVolRemaining = (_cube + ((_cube * _nosplitlinecubepct) / 100) - picklistCollection.CurrVolume)
            Else
                cubeVolRemaining = _cube - picklistCollection.CurrVolume
            End If
        End If
        If (_weight > 0) And (picklistCollection.CurrWeight > 0) Then
            If (_nosplitlinewgtpct > 0) Then
                weightRemaining = (_weight + ((_weight * _nosplitlinewgtpct) / 100) - picklistCollection.CurrWeight)
            Else
                weightRemaining = _weight - picklistCollection.CurrWeight
            End If

        End If

        'Determine how much qty of current pick line can fullfill the current picklist
        If (_cube > 0 And skuUnitVolume > 0) Then
            numUnitsByCube = Math.Floor(cubeVolRemaining / skuUnitVolume)
        End If

        If (_weight > 0 And skuUnitWeight > 0) Then
            numUnitsByWeight = Math.Floor(weightRemaining / skuUnitWeight)
        End If
        If (_cube > 0 And _weight > 0) Then
            If (numUnitsByCube < 1 Or numUnitsByWeight < 1) Then
                numUnitsCanFill = 0
                Return False
            End If
        End If
        If (_cube > 0 And _weight > 0) And (numUnitsByCube > 0 And numUnitsByWeight > 0) Then

            If (numUnitsByCube <= numUnitsByWeight) Then
                numUnitsCanFill = numUnitsByCube
            Else
                numUnitsCanFill = numUnitsByWeight
            End If
        ElseIf (_cube > 0 And numUnitsByCube > 0) Then
            numUnitsCanFill = numUnitsByCube
        ElseIf (_weight > 0 And numUnitsByWeight > 0) Then
            numUnitsCanFill = numUnitsByWeight
        Else
            numUnitsCanFill = 0
        End If

        'RWMS-2378 RWMS-2377 - If Skuunitvolume comes as zero , Allow the full quantity to add the current picklist
        If (numUnitsCanFill > 0) Then
            Return True
        ElseIf (skuUnitVolume = 0) Then
            numUnitsCanFill = pck.Units
            Return True
        Else
            Return False
        End If

    End Function
    ''End  RWMS-1279  -Planner service not planning end of pick

    ''Start  RWMS-1279  -Planner service not planning end of pick
    Private Function SplitPickLineAndAddToPickList(ByRef picklistCollection As PicksObjectCollection, ByRef pck As PicksObject, ByVal skuUnitVolume As Decimal, ByVal skuUnitWeight As Decimal, ByRef unitsToBeFullfilled As Integer, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim numUnitsCanSplit As Decimal
        If (pck.Units = 1) Then
            picklistCollection.PlaceInNewPickList(pck, oLogger)
            unitsToBeFullfilled = unitsToBeFullfilled - pck.Units
            Return True
        End If

        If IsPickLineSplitRequired(pck, skuUnitVolume, skuUnitWeight, numUnitsCanSplit, oLogger) = True Then
            Dim prtSplitPck1, prtSplitPck2 As PicksObject

            prtSplitPck1 = pck.Clone
            prtSplitPck2 = pck.Clone
            prtSplitPck1.Units = numUnitsCanSplit

            prtSplitPck2.Units = pck.Units - prtSplitPck1.Units

            If Not oLogger Is Nothing Then
                oLogger.Write(" PicklistBreaker.SplitPickLineAndAddToPickList - Splitting Pick Line. First Split with Qty=" + prtSplitPck1.Units.ToString() + ",Remaing Qty=" + prtSplitPck2.Units.ToString()) 'RWMS-1485
            End If

            'Place the split units to picklist 
            picklistCollection.PlaceInNewPickList(prtSplitPck1, oLogger)
            unitsToBeFullfilled = unitsToBeFullfilled - prtSplitPck1.Units
            pck = prtSplitPck2
            unitsToBeFullfilled = prtSplitPck2.Units

        Else
            If Not oLogger Is Nothing Then
                oLogger.Write(" PicklistBreaker.SplitPickLineAndAddToPickList - IsPickLineSplitRequired returned FALSE") 'RWMS-1485
            End If
            picklistCollection.PlaceInNewPickList(pck, oLogger)
            unitsToBeFullfilled = unitsToBeFullfilled - pck.Units

        End If
        Return True
    End Function
    ''End  RWMS-1279  -Planner service not planning end of pick
    ''Start  RWMS-1279  -Planner service not planning end of pick
    Private Function IsPickLineSplitRequired(ByVal pck As PicksObject, ByVal skuUnitVolume As Decimal, ByVal skuUnitWeight As Decimal, ByRef numUnitsCanSplit As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim cubeVolRemaining As Decimal
        Dim weightRemaining As Decimal
        Dim numUnitsByCube As Decimal
        Dim numUnitsByWeight As Decimal
        cubeVolRemaining = 0
        weightRemaining = 0

        'Determine how much qty of current line can fullfill the defined cube/weight limit
        If (pck.Units = 1) Then
            Return False
        End If

        If (_cube > 0) Then
            If (skuUnitVolume > _cube) Then
                numUnitsCanSplit = 1
                Return True
            End If
        End If

        If (_weight > 0) Then
            If (skuUnitWeight > _weight) Then
                numUnitsCanSplit = 1
                Return True
            End If
        End If

        If (_cube > 0 And skuUnitVolume > 0) Then
            If (_nosplitlinecubepct > 0) Then
                numUnitsByCube = Math.Floor((_cube + ((_cube * _nosplitlinecubepct) / 100)) / skuUnitVolume)
            Else
                numUnitsByCube = Math.Floor(_cube / skuUnitVolume)
            End If

        End If

        If (_weight > 0 And skuUnitWeight > 0) Then
            If (_nosplitlinewgtpct > 0) Then
                numUnitsByWeight = Math.Floor((_weight + ((_weight * _nosplitlinewgtpct) / 100)) / skuUnitWeight)
            Else
                numUnitsByWeight = Math.Floor(_weight / skuUnitWeight)
            End If

        End If
        If (_cube > 0 And _weight > 0) Then
            If (numUnitsByCube < 1 Or numUnitsByWeight < 1) Then
                numUnitsCanSplit = 0
                Return False
            End If

        End If
        If (_cube > 0 And _weight > 0) And (numUnitsByCube > 0 And numUnitsByWeight > 0) Then

            If (numUnitsByCube <= numUnitsByWeight) Then
                numUnitsCanSplit = numUnitsByCube
            Else
                numUnitsCanSplit = numUnitsByWeight
            End If
        ElseIf (_cube > 0 And numUnitsByCube > 0) Then
            numUnitsCanSplit = numUnitsByCube
        ElseIf (_weight > 0 And numUnitsByWeight > 0) Then
            numUnitsCanSplit = numUnitsByWeight
        Else
            numUnitsCanSplit = 0
        End If


        If (numUnitsCanSplit > 0) Then
            If (pck.Units <= numUnitsCanSplit) Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function
    ''End  RWMS-1279  -Planner service not planning end of pick
    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Private Function CreateNewPickListAndPlace(ByRef pckcols As ArrayList, ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, _
                        Optional ByVal pContType As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        If Not oLogger Is Nothing Then
            oLogger.Write("Proceeding to create container and place")
        End If
        Dim picklistAL As New ArrayList
        Dim oComp As Company
        Dim oOrd As OutboundOrderHeader
        Dim picklistCollection As PicksObjectCollection = New PicksObjectCollection
        If ((pContType Is Nothing) Or (pContType = "")) Then

            'Commented for PWMS-807 Start
            'If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing) Then
            'Commented for PWMS-807 End

            'Added for PWMS-807 Start
            Dim CubeValidation As Boolean = True
            Dim WeightValidation As Boolean = True

            'TO DO: 
            'IF splitline is false, 
            'check for cube n weight validation.
            'If validation passes, PlaceInNewPickList
            'elseif validation fails, CreateNewPickListAndPlace

            'ELSEIF splitline is true,
            'SplitPicksByCube

            If Not SplitLine Then
                If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing, SplitLine, CubeValidation, WeightValidation) Then
                    _PreviousCubeCapacity = _cube
                    _PreviousWeightCapacity = _weight
                    picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                    picklistAL.Add(picklistCollection)
                Else
                    _PreviousCubeCapacity = _cube
                    _PreviousWeightCapacity = _weight
                    CreateNewPickListAndPlaceForNoSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                End If
            Else
                Dim totalprevcubeaccumulated As Decimal
                Dim totalprevweightaccumulated As Decimal
                totalprevcubeaccumulated = 0
                totalprevweightaccumulated = 0
                If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing, SplitLine, CubeValidation, WeightValidation) Then
                    'check for the previous picklist volume and qty if exists. If possible split the current cube and qty. And assign the 1st split to previous picklist with new picklist line. And 2nd split as new picklist
                    If pckcols.Count > 0 Then
                        Dim i As Integer
                        i = pckcols.Count - 1
                        Dim temppicklistCollection As PicksObjectCollection
                        temppicklistCollection = pckcols(i)

                        totalprevcubeaccumulated = temppicklistCollection.CurrVolume
                        totalprevweightaccumulated = temppicklistCollection.CurrWeight
                    End If

                    If (totalprevcubeaccumulated > 0) And (totalprevcubeaccumulated < _PreviousCubeCapacity) Then ''RWMS-1279
                        'split the picklist
                        'split the current picklist. Find how many units can be still accomodated for the prev picklist
                        'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                        'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                        Dim prevtotalremainingcube As Decimal
                        prevtotalremainingcube = _PreviousCubeCapacity - totalprevcubeaccumulated

                        Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                        Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                        Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                        Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                        Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                        If pckcols.Count > 0 Then
                            Dim i As Integer
                            i = pckcols.Count - 1
                            Dim temppicklistCollection As PicksObjectCollection
                            temppicklistCollection = pckcols(i)

                            'split the picklist
                            '1st split
                            Dim prtPck As PicksObject
                            prtPck = pck.Clone

                            prtPck.Units = currentunitscanbeaccumulatedtoprev
                            ''eliminate adding picklines with 0 qty RWMS-1279
                            If (prtPck.Units > 0) Then
                                temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                            End If

                            '2nd split
                            pck.Units = remainingcurrentunits

                            picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            picklistAL.Add(picklistCollection)

                        End If
                    ElseIf (totalprevweightaccumulated > 0) And (totalprevweightaccumulated < _PreviousWeightCapacity) Then ''RWMS-1279
                        'split the picklist
                        'split the current picklist. Find how many units can be still accomodated for the prev picklist
                        'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                        'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                        Dim prevtotalremainingweight As Decimal
                        prevtotalremainingweight = _PreviousWeightCapacity - totalprevweightaccumulated

                        Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                        Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                        Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                        Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                        Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                        If pckcols.Count > 0 Then
                            Dim i As Integer
                            i = pckcols.Count - 1
                            Dim temppicklistCollection As PicksObjectCollection
                            temppicklistCollection = pckcols(i)

                            'split the picklist
                            '1st split
                            Dim prtPck As PicksObject
                            prtPck = pck.Clone

                            prtPck.Units = currentunitscanbeaccumulatedtoprev
                            If (prtPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                            End If

                            '2nd split
                            pck.Units = remainingcurrentunits

                            picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            picklistAL.Add(picklistCollection)

                        End If
                    Else
                        'PlaceInNewPickList
                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                        picklistAL.Add(picklistCollection)
                    End If
                    _PreviousCubeCapacity = _cube
                    _PreviousWeightCapacity = _weight
                Else
                    'check for the previous picklist volume and qty if exists. If possible split the current cube and qty. And assign the 1st split to previous picklist with new picklist line. And 2nd split as new picklist
                    If pckcols.Count > 0 Then
                        Dim i As Integer
                        i = pckcols.Count - 1
                        Dim temppicklistCollection As PicksObjectCollection
                        temppicklistCollection = pckcols(i)

                        totalprevcubeaccumulated = temppicklistCollection.CurrVolume
                        totalprevweightaccumulated = temppicklistCollection.CurrWeight
                        'Added for PWMS-853   
                    Else
                        totalprevcubeaccumulated = 0
                        totalprevweightaccumulated = 0
                        'Ended for PWMS-853   

                    End If

                    If CubeValidation = False Then
                        If (totalprevcubeaccumulated > 0) And (totalprevcubeaccumulated < _PreviousCubeCapacity) Then ''RWMS-1279
                            'split the current picklist. Find how many units can be still accomodated for the prev picklist
                            'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                            'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)

                            Dim prevtotalremainingcube As Decimal
                            prevtotalremainingcube = _PreviousCubeCapacity - totalprevcubeaccumulated

                            Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                            Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                            Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                            If pckcols.Count > 0 Then
                                Dim i As Integer
                                i = pckcols.Count - 1
                                Dim temppicklistCollection As PicksObjectCollection
                                temppicklistCollection = pckcols(i)

                                'split the picklist
                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev
                                If (prtPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                    temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits
                                'here check if these units can be accumulated in the fresh cube.
                                'if not split again and create new picklists

                                If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing, SplitLine, CubeValidation, WeightValidation) Then
                                    picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                    picklistAL.Add(picklistCollection)
                                Else
                                    Dim unitsforfreshcube As Decimal = Math.Floor(_cube / SingleUnitVolume)

                                    Dim indexUnits As Decimal = remainingcurrentunits
                                    While indexUnits >= unitsforfreshcube

                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = unitsforfreshcube

                                        If (tmpPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If

                                        indexUnits = Math.Floor(indexUnits - unitsforfreshcube)
                                    End While
                                    If indexUnits > 0 Then
                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = indexUnits
                                        If (tmpPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If
                                    End If
                                End If
                                'Ended for PWMS-853
                            End If
                        Else
                            'If the 1st picklist is cannot be accumulated in the cube
                            If totalprevcubeaccumulated = 0 And _PreviousCubeCapacity = 0 Then
                                'spit the picklist

                                Dim prevtotalremainingcube As Decimal
                                prevtotalremainingcube = _cube

                                Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                                Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                '1st split    
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev
                                If (prtPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                    CreateNewPickListAndPlaceForSplit(pckcols, prtPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                End If

                                '2nd split    
                                pck.Units = remainingcurrentunits

                                'Added for RWMS-1279
                                'check for if the splitted picklist can be accommodated in the cube
                                'else split it again in a loop
                                If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing, SplitLine, CubeValidation, WeightValidation) Then
                                    'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                    'picklistAL.Add(picklistCollection)
                                    CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                Else
                                    Dim unitsforfreshcube As Decimal = Math.Floor(_cube / SingleUnitVolume)

                                    Dim indexUnits As Decimal = remainingcurrentunits
                                    While indexUnits >= unitsforfreshcube

                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = unitsforfreshcube
                                        ''eliminate adding picklines with 0 qty RWMS-1046
                                        If (tmpPck.Units > 0) Then
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If

                                        indexUnits = Math.Floor(indexUnits - unitsforfreshcube)
                                    End While
                                    If indexUnits > 0 Then
                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = indexUnits
                                        ''eliminate adding picklines with 0 qty RWMS-1046
                                        If (tmpPck.Units > 0) Then
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If
                                    End If

                                End If
                                'End Added for RWMS-1279
                                'Commented for RWMS-1279
                                'CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                'End Commented for RWMS-1279

                            Else
                                'CreateNewPickListAndPlace    
                                CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                            End If
                            'Ended for PWMS-853  

                        End If
                    ElseIf WeightValidation = False Then
                        If (totalprevweightaccumulated > 0) And (totalprevweightaccumulated < _PreviousWeightCapacity) Then 'RWMS-1279
                            'split the current picklist. Find how many units can be still accomodated for the prev picklist
                            'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                            'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)

                            Dim prevtotalremainingweight As Decimal
                            prevtotalremainingweight = _PreviousWeightCapacity - totalprevweightaccumulated

                            Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                            Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                            Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                            If pckcols.Count > 0 Then
                                Dim i As Integer
                                i = pckcols.Count - 1
                                Dim temppicklistCollection As PicksObjectCollection
                                temppicklistCollection = pckcols(i)

                                'split the picklist
                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev

                                If (prtPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                    temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits



                                'here check if these units can be accumulated in the fresh cube.
                                'if not split again and create new picklists

                                If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing, SplitLine, CubeValidation, WeightValidation) Then
                                    picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                    picklistAL.Add(picklistCollection)
                                Else
                                    Dim unitsforfreshweight As Decimal = Math.Floor(_weight / SingleUnitWeight)

                                    Dim indexUnits As Decimal = remainingcurrentunits
                                    While indexUnits >= unitsforfreshweight

                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = unitsforfreshweight

                                        ''eliminate adding picklines with 0 qty RWMS-1279
                                        If (tmpPck.Units > 0) Then
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If

                                        indexUnits = Math.Floor(indexUnits - unitsforfreshweight)
                                    End While
                                    If indexUnits > 0 Then
                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = indexUnits

                                        If (tmpPck.Units > 0) Then ''eliminate adding picklines with 0 qty RWMS-1279
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If
                                    End If

                                End If
                                'Ended for PWMS-853
                            End If
                        Else
                            'If the 1st picklist is cannot be accumulated for the weight
                            If totalprevweightaccumulated = 0 And _PreviousWeightCapacity = 0 Then
                                'spit the picklist

                                Dim prevtotalremainingweight As Decimal
                                prevtotalremainingweight = _weight

                                Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                                Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev

                                If (prtPck.Units > 0) Then  ''eliminate adding picklines with 0 qty RWMS-1279
                                    CreateNewPickListAndPlaceForSplit(pckcols, prtPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits

                                'Added for RWMS-1279
                                'check for if the splitted picklist can be accommodated in the cube
                                'else split it again in a loop

                                If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, Nothing, SplitLine, CubeValidation, WeightValidation) Then
                                    'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                    'picklistAL.Add(picklistCollection)
                                    CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                Else
                                    Dim unitsforfreshweight As Decimal = Math.Floor(_weight / SingleUnitWeight)

                                    Dim indexUnits As Decimal = remainingcurrentunits
                                    While indexUnits >= unitsforfreshweight

                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = unitsforfreshweight

                                        ''eliminate adding picklines with 0 qty RWMS-1279
                                        If (tmpPck.Units > 0) Then
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If

                                        indexUnits = Math.Floor(indexUnits - unitsforfreshweight)
                                    End While
                                    If indexUnits > 0 Then
                                        Dim tmpPck As PicksObject
                                        tmpPck = pck.Clone

                                        tmpPck.Units = indexUnits
                                        ''eliminate adding picklines with 0 qty RWMS-1279
                                        If (tmpPck.Units > 0) Then
                                            CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                        End If
                                    End If

                                End If

                                'End Added for RWMS-1279
                                'Commented for RWMS-1279
                                'CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                'End Commented for RWMS-1279

                            Else
                                'CreateNewPickListAndPlace    
                                CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                            End If
                            'Ended for PWMS-853  
                        End If

                    End If
                    _PreviousCubeCapacity = _cube
                    _PreviousWeightCapacity = _weight
                End If
            End If

            'Added for PWMS-807 End

        Else
            oOrd = WMS.Logic.Planner.GetOutboundOrder(pck.Consignee, pck.OrderId)                           'New OutboundOrderHeader(pck.Consignee, pck.OrderId)
            oComp = WMS.Logic.Planner.GetCompany(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)      'New Company(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)

            If oComp.CONTAINER = "" Or oComp.CONTAINER Is Nothing Then
                'Commented for PWMS-807 Start
                'If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container) Then
                'End Commented for RWMS-828
                'Added for RWMS-828
                Dim CubeValidation As Boolean = True
                Dim WeightValidation As Boolean = True

                If Not SplitLine Then
                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container, SplitLine, CubeValidation, WeightValidation) Then
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                        picklistAL.Add(picklistCollection)
                    Else
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                        'CreateNewPickListAndPlaceForNoSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)

                        'TODO: BreakPicksByContainer
                        BreakPicksByContainer(picklistAL, _container, pck, pPickPartialUOM, oLogger)
                    End If
                Else
                    Dim totalprevcubeaccumulated As Decimal
                    Dim totalprevweightaccumulated As Decimal
                    'Start RWMS-1279  Planner service not planning end of pick
                    totalprevcubeaccumulated = 0
                    totalprevweightaccumulated = 0
                    'End RWMS-1279  Planner service not planning end of pick
                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container, SplitLine, CubeValidation, WeightValidation) Then
                        'check for the previous picklist volume and qty if exists. If possible split the current cube and qty. And assign the 1st split to previous picklist with new picklist line. And 2nd split as new picklist
                        If pckcols.Count > 0 Then
                            Dim i As Integer
                            i = pckcols.Count - 1
                            Dim temppicklistCollection As PicksObjectCollection
                            temppicklistCollection = pckcols(i)

                            totalprevcubeaccumulated = temppicklistCollection.CurrVolume
                            totalprevweightaccumulated = temppicklistCollection.CurrWeight
                        End If

                        If (totalprevcubeaccumulated > 0) And (totalprevcubeaccumulated < _PreviousCubeCapacity) Then 'RWMS-1279
                            'split the picklist
                            'split the current picklist. Find how many units can be still accomodated for the prev picklist
                            'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                            'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            Dim prevtotalremainingcube As Decimal
                            prevtotalremainingcube = _PreviousCubeCapacity - totalprevcubeaccumulated

                            Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                            Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                            Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                            If pckcols.Count > 0 Then
                                Dim i As Integer
                                i = pckcols.Count - 1
                                Dim temppicklistCollection As PicksObjectCollection
                                temppicklistCollection = pckcols(i)

                                'split the picklist
                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev

                                ''eliminate adding picklines with 0 qty RWMS-1279
                                If (prtPck.Units > 0) Then
                                    temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits

                                picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                picklistAL.Add(picklistCollection)

                            End If
                        ElseIf (totalprevweightaccumulated > 0) And (totalprevweightaccumulated < _PreviousWeightCapacity) Then 'RWMS-1279
                            'split the picklist
                            'split the current picklist. Find how many units can be still accomodated for the prev picklist
                            'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                            'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            Dim prevtotalremainingweight As Decimal
                            prevtotalremainingweight = _PreviousWeightCapacity - totalprevweightaccumulated

                            Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                            Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                            Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                            If pckcols.Count > 0 Then
                                Dim i As Integer
                                i = pckcols.Count - 1
                                Dim temppicklistCollection As PicksObjectCollection
                                temppicklistCollection = pckcols(i)

                                'split the picklist
                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev

                                ''eliminate adding picklines with 0 qty RWMS-1046(RWMS-1279)
                                If (prtPck.Units > 0) Then
                                    temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits

                                picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                picklistAL.Add(picklistCollection)

                            End If
                        Else
                            'PlaceInNewPickList
                            picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            picklistAL.Add(picklistCollection)
                        End If
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                    Else
                        'check for the previous picklist volume and qty if exists. If possible split the current cube and qty. And assign the 1st split to previous picklist with new picklist line. And 2nd split as new picklist
                        If pckcols.Count > 0 Then
                            Dim i As Integer
                            i = pckcols.Count - 1
                            Dim temppicklistCollection As PicksObjectCollection
                            temppicklistCollection = pckcols(i)

                            totalprevcubeaccumulated = temppicklistCollection.CurrVolume
                            totalprevweightaccumulated = temppicklistCollection.CurrWeight
                            'Added for PWMS-853   
                        Else
                            totalprevcubeaccumulated = 0
                            totalprevweightaccumulated = 0
                            'Ended for PWMS-853   

                        End If

                        If CubeValidation = False Then
                            If (totalprevcubeaccumulated > 0) And (totalprevcubeaccumulated < _PreviousCubeCapacity) Then 'RWMS-1279
                                'split the current picklist. Find how many units can be still accomodated for the prev picklist
                                'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                                'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)

                                Dim prevtotalremainingcube As Decimal
                                prevtotalremainingcube = _PreviousCubeCapacity - totalprevcubeaccumulated

                                Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                                Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                If pckcols.Count > 0 Then
                                    Dim i As Integer
                                    i = pckcols.Count - 1
                                    Dim temppicklistCollection As PicksObjectCollection
                                    temppicklistCollection = pckcols(i)

                                    'split the picklist
                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046 (added with RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits



                                    'here check if these units can be accumulated in the fresh cube.
                                    'if not split again and create new picklists

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container, SplitLine, CubeValidation, WeightValidation) Then
                                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        picklistAL.Add(picklistCollection)
                                    Else
                                        Dim unitsforfreshcube As Decimal = Math.Floor(_cube / SingleUnitVolume)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshcube

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshcube

                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshcube)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If
                                    'Ended for PWMS-853 


                                End If
                            Else
                                'If the 1st picklist is cannot be accumulated in the cube
                                If totalprevcubeaccumulated = 0 And _PreviousCubeCapacity = 0 Then
                                    'spit the picklist

                                    Dim prevtotalremainingcube As Decimal
                                    prevtotalremainingcube = _cube

                                    Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                    Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                                    Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev
                                    ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        CreateNewPickListAndPlaceForSplit(pckcols, prtPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits

                                    'Added for RWMS-1279

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container, SplitLine, CubeValidation, WeightValidation) Then
                                        'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        'picklistAL.Add(picklistCollection)
                                        CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    Else
                                        Dim unitsforfreshcube As Decimal = Math.Floor(_cube / SingleUnitVolume)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshcube

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshcube
                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshcube)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If
                                    'End Added for RWMS-1279

                                    'Commented for RWMS-1279
                                    'CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    'Commented for RWMS-1279

                                Else
                                    'CreateNewPickListAndPlace    
                                    CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                End If

                            End If
                            'Ended for PWMS-853   
                        ElseIf WeightValidation = False Then
                            If (totalprevweightaccumulated > 0) And (totalprevweightaccumulated < _PreviousWeightCapacity) Then 'RWMS-1279
                                'split the current picklist. Find how many units can be still accomodated for the prev picklist
                                'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                                'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)

                                Dim prevtotalremainingweight As Decimal
                                prevtotalremainingweight = _PreviousWeightCapacity - totalprevweightaccumulated

                                Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                                Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                If pckcols.Count > 0 Then
                                    Dim i As Integer
                                    i = pckcols.Count - 1
                                    Dim temppicklistCollection As PicksObjectCollection
                                    temppicklistCollection = pckcols(i)

                                    'split the picklist
                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits


                                    'here check if these units can be accumulated in the fresh cube.
                                    'if not split again and create new picklists

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container, SplitLine, CubeValidation, WeightValidation) Then
                                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        picklistAL.Add(picklistCollection)
                                    Else
                                        Dim unitsforfreshweight As Decimal = Math.Floor(_weight / SingleUnitWeight)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshweight

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshweight

                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshweight)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If
                                    'ended for  PWMS-853 

                                    'Commented for RWMS-2072 RWMS-1899 RWMS-1798 End
                                    'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                    'picklistAL.Add(picklistCollection)
                                    'Commented for RWMS-2072 RWMS-1899 RWMS-1798 End

                                End If
                            Else
                                'If the 1st picklist is cannot be accumulated for the weight
                                If totalprevweightaccumulated = 0 And _PreviousWeightCapacity = 0 Then
                                    'spit the picklist

                                    Dim prevtotalremainingweight As Decimal
                                    prevtotalremainingweight = _weight

                                    Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                    Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                                    Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046
                                    If (prtPck.Units > 0) Then
                                        CreateNewPickListAndPlaceForSplit(pckcols, prtPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits

                                    'Added for RWMS-1279

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container, SplitLine, CubeValidation, WeightValidation) Then
                                        'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        'picklistAL.Add(picklistCollection)
                                        CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    Else
                                        Dim unitsforfreshweight As Decimal = Math.Floor(_weight / SingleUnitWeight)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshweight

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshweight

                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshweight)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If
                                    'End Added for RWMS-1279

                                    'Commented for RWMS-1279
                                    'CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    'End Commented for RWMS-1279

                                Else
                                    'CreateNewPickListAndPlace
                                    CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                End If
                            End If
                        End If
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                    End If

                End If

            Else

                Dim CubeValidation As Boolean = True
                Dim WeightValidation As Boolean = True

                If Not SplitLine Then
                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, oComp.CONTAINER, SplitLine, CubeValidation, WeightValidation) Then
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                        picklistAL.Add(picklistCollection)
                    Else
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                        'CreateNewPickListAndPlaceForNoSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)

                        'TODO: BreakPicksByContainer
                        BreakPicksByContainer(picklistAL, oComp.CONTAINER, pck, pPickPartialUOM, oLogger)
                    End If
                Else
                    Dim totalprevcubeaccumulated As Decimal
                    Dim totalprevweightaccumulated As Decimal
                    'Start RWMS-1279  Planner service not planning end of pick
                    totalprevcubeaccumulated = 0
                    totalprevweightaccumulated = 0
                    'End RWMS-1279  Planner service not planning end of pick
                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, oComp.CONTAINER, SplitLine, CubeValidation, WeightValidation) Then
                        'check for the previous picklist volume and qty if exists. If possible split the current cube and qty. And assign the 1st split to previous picklist with new picklist line. And 2nd split as new picklist
                        If pckcols.Count > 0 Then
                            Dim i As Integer
                            i = pckcols.Count - 1
                            Dim temppicklistCollection As PicksObjectCollection
                            temppicklistCollection = pckcols(i)

                            totalprevcubeaccumulated = temppicklistCollection.CurrVolume
                            totalprevweightaccumulated = temppicklistCollection.CurrWeight
                        End If

                        If (totalprevcubeaccumulated > 0) And (totalprevcubeaccumulated < _PreviousCubeCapacity) Then 'RWMS-1279
                            'split the picklist
                            'split the current picklist. Find how many units can be still accomodated for the prev picklist
                            'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                            'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            Dim prevtotalremainingcube As Decimal
                            prevtotalremainingcube = _PreviousCubeCapacity - totalprevcubeaccumulated

                            Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                            Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                            Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                            If pckcols.Count > 0 Then
                                Dim i As Integer
                                i = pckcols.Count - 1
                                Dim temppicklistCollection As PicksObjectCollection
                                temppicklistCollection = pckcols(i)

                                'split the picklist
                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev

                                ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                If (prtPck.Units > 0) Then
                                    temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits

                                picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                picklistAL.Add(picklistCollection)

                            End If
                        ElseIf (totalprevweightaccumulated > 0) And (totalprevweightaccumulated < _PreviousWeightCapacity) Then 'RWMS-1279
                            'split the picklist
                            'split the current picklist. Find how many units can be still accomodated for the prev picklist
                            'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                            'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            Dim prevtotalremainingweight As Decimal
                            prevtotalremainingweight = _PreviousWeightCapacity - totalprevweightaccumulated

                            Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                            Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                            Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                            Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                            If pckcols.Count > 0 Then
                                Dim i As Integer
                                i = pckcols.Count - 1
                                Dim temppicklistCollection As PicksObjectCollection
                                temppicklistCollection = pckcols(i)

                                'split the picklist
                                '1st split
                                Dim prtPck As PicksObject
                                prtPck = pck.Clone

                                prtPck.Units = currentunitscanbeaccumulatedtoprev

                                ''eliminate adding picklines with 0 qty RWMS-1046 (RWMS-1279)
                                If (prtPck.Units > 0) Then
                                    temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                End If

                                '2nd split
                                pck.Units = remainingcurrentunits

                                picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                picklistAL.Add(picklistCollection)

                            End If
                        Else
                            'PlaceInNewPickList
                            picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                            picklistAL.Add(picklistCollection)
                        End If
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                    Else
                        'check for the previous picklist volume and qty if exists. If possible split the current cube and qty. And assign the 1st split to previous picklist with new picklist line. And 2nd split as new picklist
                        If pckcols.Count > 0 Then
                            Dim i As Integer
                            i = pckcols.Count - 1
                            Dim temppicklistCollection As PicksObjectCollection
                            temppicklistCollection = pckcols(i)

                            totalprevcubeaccumulated = temppicklistCollection.CurrVolume
                            totalprevweightaccumulated = temppicklistCollection.CurrWeight
                        Else
                            'Added for PWMS-853
                            totalprevcubeaccumulated = 0
                            totalprevweightaccumulated = 0
                            'Ended for PWMS-853
                        End If

                        If CubeValidation = False Then
                            If (totalprevcubeaccumulated > 0) And (totalprevcubeaccumulated < _PreviousCubeCapacity) Then 'RWMS-1279
                                'split the current picklist. Find how many units can be still accomodated for the prev picklist
                                'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                                'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)

                                Dim prevtotalremainingcube As Decimal
                                prevtotalremainingcube = _PreviousCubeCapacity - totalprevcubeaccumulated

                                Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                                Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                If pckcols.Count > 0 Then
                                    Dim i As Integer
                                    i = pckcols.Count - 1
                                    Dim temppicklistCollection As PicksObjectCollection
                                    temppicklistCollection = pckcols(i)

                                    'split the picklist
                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits



                                    'here check if these units can be accumulated in the fresh cube.
                                    'if not split again and create new picklists

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, oComp.CONTAINER, SplitLine, CubeValidation, WeightValidation) Then
                                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        picklistAL.Add(picklistCollection)
                                    Else
                                        Dim unitsforfreshcube As Decimal = Math.Floor(_cube / SingleUnitVolume)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshcube

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshcube

                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshcube)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits

                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If





                                End If
                            Else
                                'If the 1st picklist is cannot be accumulated in the cube
                                If totalprevcubeaccumulated = 0 And _PreviousCubeCapacity = 0 Then
                                    'spit the picklist

                                    Dim prevtotalremainingcube As Decimal
                                    prevtotalremainingcube = _cube

                                    Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                    Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingcube / SingleUnitVolume)
                                    Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        CreateNewPickListAndPlaceForSplit(pckcols, prtPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits

                                    'Added for RWMS-1279

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, oComp.CONTAINER, SplitLine, CubeValidation, WeightValidation) Then
                                        'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        'picklistAL.Add(picklistCollection)
                                        CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    Else
                                        Dim unitsforfreshcube As Decimal = Math.Floor(_cube / SingleUnitVolume)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshcube

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshcube
                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshcube)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If
                                    'End Added for RWMS-1279

                                    'Commented for RWMS-1279
                                    'CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    'End Commented for RWMS-1279

                                Else
                                    'CreateNewPickListAndPlace
                                    CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                End If

                            End If
                        ElseIf WeightValidation = False Then
                            If (totalprevweightaccumulated > 0) And (totalprevweightaccumulated < _PreviousWeightCapacity) Then 'RWMS-1279
                                'split the current picklist. Find how many units can be still accomodated for the prev picklist
                                'assign the 1st split units to the previous picklist as new picklist line. i.e call picklistCollection.PlaceInNewPickList(pck, _ergheavyth)
                                'assign the 2nd split units to the current picklist i.e. new picklist. i.e call picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)

                                Dim prevtotalremainingweight As Decimal
                                prevtotalremainingweight = _PreviousWeightCapacity - totalprevweightaccumulated

                                Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                                Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                If pckcols.Count > 0 Then
                                    Dim i As Integer
                                    i = pckcols.Count - 1
                                    Dim temppicklistCollection As PicksObjectCollection
                                    temppicklistCollection = pckcols(i)

                                    'split the picklist
                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        temppicklistCollection.PlaceInNewPickList(prtPck, _ergheavyth)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits



                                    'here check if these units can be accumulated in the fresh cube.
                                    'if not split again and create new picklists

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, oComp.CONTAINER, SplitLine, CubeValidation, WeightValidation) Then
                                        picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        picklistAL.Add(picklistCollection)
                                    Else
                                        Dim unitsforfreshweight As Decimal = Math.Floor(_weight / SingleUnitWeight)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshweight

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshweight

                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshweight)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If

                                    'Commented for RWMS-1899 AND RWMS-1798 start
                                    'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                    'picklistAL.Add(picklistCollection)
                                    'Commented for RWMS-1899 AND RWMS-1798 end
                                End If
                            Else
                                'If the 1st picklist is cannot be accumulated for the weight
                                If (totalprevweightaccumulated = 0 And _PreviousWeightCapacity = 0) Then
                                    'spit the picklist

                                    Dim prevtotalremainingweight As Decimal
                                    prevtotalremainingweight = _weight

                                    Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
                                    Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
                                    Dim currentunitscanbeaccumulatedtoprev As Decimal = Math.Floor(prevtotalremainingweight / SingleUnitWeight)
                                    Dim remainingcurrentunits As Decimal = Math.Floor(pck.Units - currentunitscanbeaccumulatedtoprev)

                                    '1st split
                                    Dim prtPck As PicksObject
                                    prtPck = pck.Clone

                                    prtPck.Units = currentunitscanbeaccumulatedtoprev

                                    ''eliminate adding picklines with 0 qty RWMS-1046 (Added on RWMS-1279)
                                    If (prtPck.Units > 0) Then
                                        CreateNewPickListAndPlaceForSplit(pckcols, prtPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    End If

                                    '2nd split
                                    pck.Units = remainingcurrentunits

                                    'Added for RWMS-1279

                                    If picklistCollection.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, oComp.CONTAINER, SplitLine, CubeValidation, WeightValidation) Then
                                        'picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
                                        'picklistAL.Add(picklistCollection)
                                        CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    Else
                                        Dim unitsforfreshweight As Decimal = Math.Floor(_weight / SingleUnitWeight)

                                        Dim indexUnits As Decimal = remainingcurrentunits
                                        While indexUnits >= unitsforfreshweight

                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = unitsforfreshweight

                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If

                                            indexUnits = Math.Floor(indexUnits - unitsforfreshweight)
                                        End While
                                        If indexUnits > 0 Then
                                            Dim tmpPck As PicksObject
                                            tmpPck = pck.Clone

                                            tmpPck.Units = indexUnits
                                            ''eliminate adding picklines with 0 qty RWMS-1046
                                            If (tmpPck.Units > 0) Then
                                                CreateNewPickListAndPlaceForSplit(pckcols, tmpPck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                            End If
                                        End If

                                    End If

                                    'End Added for RWMS-1279

                                    'Commented for RWMS-1279
                                    'CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                    'End Commented for RWMS-1279

                                Else
                                    'CreateNewPickListAndPlace    
                                    CreateNewPickListAndPlaceForSplit(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, pContType, oLogger)
                                End If

                            End If
                        End If
                        _PreviousCubeCapacity = _cube
                        _PreviousWeightCapacity = _weight
                    End If

                End If

            End If
        End If

        For Each picklistobj As PicksObjectCollection In picklistAL
            pckcols.Add(picklistobj)
        Next
    End Function


    'Added for PWMS-807 Start
    Private Function CreateNewPickListAndPlaceForNoSplit(ByRef pckcols As ArrayList, ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, _
                        Optional ByVal pContType As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        If Not oLogger Is Nothing Then
            oLogger.Write("Proceeding to create container and place")
        End If
        Dim picklistAL As New ArrayList
        Dim oComp As Company
        Dim oOrd As OutboundOrderHeader
        Dim picklistCollection As PicksObjectCollection = New PicksObjectCollection
        If ((pContType Is Nothing) Or (pContType = "")) Then
            picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
            picklistAL.Add(picklistCollection)
        Else
            oOrd = WMS.Logic.Planner.GetOutboundOrder(pck.Consignee, pck.OrderId)                           'New OutboundOrderHeader(pck.Consignee, pck.OrderId)
            oComp = WMS.Logic.Planner.GetCompany(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)      'New Company(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)

            If oComp.CONTAINER = "" Or oComp.CONTAINER Is Nothing Then
                'we have to break the PCK obj to multiple picks objects and place them in multiple strategy containers
                'Commented for RWMS-828
                BreakPicksByContainer(picklistAL, _container, pck, pPickPartialUOM, oLogger)
                'End Commented for RWMS-828
            Else
                BreakPicksByContainer(picklistAL, oComp.CONTAINER, pck, pPickPartialUOM, oLogger)
            End If
        End If

        For Each picklistobj As PicksObjectCollection In picklistAL
            pckcols.Add(picklistobj)
        Next
    End Function

    Private Function CreateNewPickListAndPlaceForSplit(ByRef pckcols As ArrayList, ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, _
                        Optional ByVal pContType As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        If Not oLogger Is Nothing Then
            oLogger.Write("Proceeding to create container and place")
        End If
        Dim picklistAL As New ArrayList
        Dim oComp As Company
        Dim oOrd As OutboundOrderHeader
        Dim picklistCollection As PicksObjectCollection = New PicksObjectCollection
        If ((pContType Is Nothing) Or (pContType = "")) Then
            picklistCollection.PlaceInNewPickList(_container, pck, _ergheavyth)
            picklistAL.Add(picklistCollection)
        Else
            oOrd = WMS.Logic.Planner.GetOutboundOrder(pck.Consignee, pck.OrderId)                           'New OutboundOrderHeader(pck.Consignee, pck.OrderId)
            oComp = WMS.Logic.Planner.GetCompany(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)      'New Company(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)

            If oComp.CONTAINER = "" Or oComp.CONTAINER Is Nothing Then
                'we have to break the PCK obj to multiple picks objects and place them in multiple strategy containers
                'Commented for RWMS-828
                BreakPicksByContainer(picklistAL, _container, pck, pPickPartialUOM, oLogger)
                'End Commented for RWMS-828
            Else
                BreakPicksByContainer(picklistAL, oComp.CONTAINER, pck, pPickPartialUOM, oLogger)
            End If
        End If

        For Each picklistobj As PicksObjectCollection In picklistAL
            pckcols.Add(picklistobj)
        Next
    End Function


    'Added for PWMS-807 End

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
    Private Function BreakPicksByContainer(ByRef ContAL As ArrayList, ByVal ContainerType As String, ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, ByVal oLogger As LogHandler) As ArrayList
        Dim prtPck As PicksObject
        Dim numUnitsPerCont As Decimal = GetNumberUnitsPerContainer(ContainerType, pck, pPickPartialUOM, 0, 0, oLogger)
        If numUnitsPerCont = 0 Then
            'If Not oLogger Is Nothing Then
            '    oLogger.Write("Container [" & ContainerType & "] is not suitable to contain any units of [" & pck.SKU & "] sku,please check container , sku units of measure and plan break strategy definitions")
            'End If
            Dim oSku As SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
            numUnitsPerCont = oSku.ConvertToUnits(pck.UOM)
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("UnitsPerContainer for breaking " + numUnitsPerCont.ToString())
        End If
        While pck.Units > 0
            Dim pckCollectionObj As New PicksObjectCollection
            prtPck = pck.Clone

            If numUnitsPerCont < pck.Units Then
                prtPck.Units = numUnitsPerCont
                pck.Units -= numUnitsPerCont
            Else
                prtPck.Units = pck.Units
                pck.Units = 0
            End If
            pckCollectionObj.PlaceInNewPickList(ContainerType, prtPck, _ergheavyth)
            ContAL.Add(pckCollectionObj)
        End While
    End Function

    Private Function GetNumberUnitsPerContainer(ByVal ContainerType As String, ByVal pck As PicksObject, ByVal pPickPartialUOM As Boolean, _
                                                ByVal pUsedWeight As Decimal, ByVal pUsedCube As Decimal, ByVal oLogger As LogHandler) As Decimal

        Dim NumUnits As Decimal
        Dim oCont As New HandelingUnit(ContainerType)

        Dim oSku As WMS.Logic.SKU = Planner.GetSKU(pck.Consignee, pck.SKU)
        Dim SingleUnitWeight As Decimal = Inventory.CalculateWeight(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)
        Dim SingleUnitVolume As Decimal = Inventory.CalculateVolume(pck.Consignee, pck.SKU, 1, pck.UOM, oSku)

        If Not oLogger Is Nothing Then
            oLogger.Write("")
            oLogger.Write("Calculating number of units per container for SKU " + pck.SKU)
            oLogger.Write("Calculated single unit weight " + SingleUnitWeight.ToString())
            oLogger.Write("Calculated single unit volume " + SingleUnitVolume.ToString())
        End If

        Dim dContAvailableCude As Decimal = oCont.CubeCapacity - pUsedCube
        Dim dContAvailableWeight As Decimal = oCont.WeightCapacity - pUsedWeight

        If Not oLogger Is Nothing Then
            oLogger.Write("Available Cube Capacity " + dContAvailableCude.ToString())
            oLogger.Write("Available Weight Capacity " + dContAvailableWeight.ToString())
        End If

        If pPickPartialUOM Then
            NumUnits = dContAvailableCude / SingleUnitVolume
        Else
            'NumUnits = Math.Floor(Math.Floor(dContAvailableCude / oSku.ConvertToUnits(pck.UOM)) * oSku.ConvertToUnits(pck.UOM) / SingleUnitVolume)
            NumUnits = Math.Floor(dContAvailableCude / (oSku.ConvertToUnits(pck.UOM) * SingleUnitVolume)) * oSku.ConvertToUnits(pck.UOM)
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("Number of units after applying cube " + NumUnits.ToString())
        End If

        If NumUnits > Math.Floor(dContAvailableWeight / SingleUnitWeight) Then
            If pPickPartialUOM Then
                NumUnits = dContAvailableWeight / SingleUnitWeight
            Else
                'NumUnits = Math.Floor(Math.Floor(dContAvailableWeight / oSku.ConvertToUnits(pck.UOM)) * oSku.ConvertToUnits(pck.UOM) / SingleUnitWeight)
                NumUnits = Math.Floor(dContAvailableWeight / (oSku.ConvertToUnits(pck.UOM) * SingleUnitWeight)) * oSku.ConvertToUnits(pck.UOM)
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Number of units after applying weight " + NumUnits.ToString())
            End If
            Return NumUnits
        Else
            Return NumUnits
        End If
    End Function


#End Region

#Region "Parallel Breaking"

    Public Function BreakParallel(ByVal Picks As PicksObjectCollection, ByVal pPickPartialUOM As Boolean, ByVal pBaseCube As Decimal, ByVal pBaseWeight As Decimal, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal ShouldBreakForLoadingPlan As Boolean = False) As ArrayList
        Dim pckcols As New ArrayList
        Dim pckcol As New PicksObjectCollection
        Dim pckcolcontainer As PicksObjectCollection
        Dim pck As PicksObject
        For Each pck In Picks
            If pck.PickType = WMS.Lib.PICKTYPE.PARALLELPICK Then
                If pck.MatchBreaker(_uom, _pickregion, _container, _plancontainer, _packarea, _deliverylocation, "", _picktype, oLogger) Then
                    'Vlidate Customer's mix picking flag
                    pck.ContainerType = _container
                    If pckcol.Count > 0 AndAlso ValidateCustomerMixPicking(pck) Then
                        pckcol.Add(pck)
                    ElseIf pckcol.Count = 0 Then
                        _consignee = pck.Consignee
                        _company = pck.Company
                        _companytype = pck.CompanyType
                        pckcol.Add(pck)
                    End If
                End If
            End If
        Next
        For Each pck In pckcol
            Picks.Remove(pck)
        Next
        Dim placed As Boolean
        If pckcol.Count > 0 Then
            If _plancontainer Then
                For Each pck In pckcol
                    placed = False
                    For Each pckcolcontainer In pckcols
                        'If pckcolcontainer.CanPlaceInContainer(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, False, False, _container) Then
                        If pckcolcontainer.CanPlaceInNewPickList(pck, _weight, _cube, _minplannedcube, pBaseCube, pBaseWeight, _ergheavyth, _ergwgtlimit, _ergcubelimit, _ergwgtlimitallheavy, _ergcubelimitallheavy, _nosplitlinewgtpct, _nosplitlinecubepct, _allowgoback, _extendwalkforfillpct, _allowmultibaseitems, pPickPartialUOM, oLogger, False, _container) Then
                            If ShouldBreakForLoadingPlan Then
                                ''check if we can place the pick on the vehicle
                                'If LoadingPlanner.CanPlaceContainer(pckcolcontainer, pck) Then
                                '    'place the pick and assigne it to the current picklist
                                '    pckcolcontainer.PlaceInContainer(pck, _ergheavyth)
                                '    placed = True
                                '    Exit For
                                'End If
                            Else
                                'place the pick and assigne it to the current picklist without consideration of the vehicle limitation
                                pckcolcontainer.PlaceInNewPickList(pck, _ergheavyth)
                                placed = True
                                Exit For
                            End If
                        End If
                    Next
                    If Not placed Then
                        CreateNewPickListAndPlace(pckcols, pck, pPickPartialUOM, pBaseCube, pBaseWeight, _container, oLogger)
                    End If
                Next
            Else
                pckcols.Add(pckcol)
            End If
            Return pckcols
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Customer Breaking"

    Private Function ValidateCustomerMixPicking(ByVal pck As PicksObject) As Boolean
        Dim oComp As Company = WMS.Logic.Planner.GetCompany(pck.Consignee, pck.Company, pck.CompanyType)
        If Not oComp.MIXPICKING Then
            If _consignee.ToLower <> pck.Consignee.ToLower Or _company.ToLower <> pck.Company.ToLower Or _companytype.ToLower <> pck.CompanyType.ToLower Then
                Return False
            End If
        End If
        Return True
    End Function

#End Region

#Region "Loading Plans Beaker"

    '****************************************************************************************************
    'New Code for the loading plan changes
    '****************************************************************************************************
    'Public Function BreakPartail(ByVal Picks As PicksObjectCollection, Optional ByVal ShouldBreakForLoadingPlan As Boolean = False) As ArrayList
    '    Dim pckcols As New ArrayList
    '    Dim retpckcols As New ArrayList
    '    Dim pckcol As New PicksObjectCollection
    '    Dim pckcolTemp As New ArrayList
    '    Dim pckcolcontainer As PicksObjectCollection
    '    Dim pck As PicksObject
    '    For Each pck In Picks
    '        If pck.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
    '            If pck.MatchBreaker(_uom, _pickregion, _container, _plancontainer, _packarea, _prestaginglocation) Then
    '                pckcol.Add(pck)
    '            End If
    '        End If
    '    Next

    '    For Each pck In pckcol
    '        Picks.Remove(pck)
    '    Next

    '    Dim placed As Boolean
    '    If pckcol.Count > 0 Then
    '        If _plancontainer Then
    '            While pckcol.Count > 0
    '                pckcolcontainer = New PicksObjectCollection
    '                pckcolcontainer.PlaceInContainer(_container, pckcol(0))
    '                pckcol.Remove(pckcol(0))
    '                pckcols.Add(pckcolcontainer)
    '                retpckcols.Add(pckcolcontainer)
    '                While pckcols.Count > 0
    '                    For Each pck In pckcol
    '                        If Not pckcolTemp.Contains(pck) Then
    '                            placed = False
    '                            If pckcolcontainer.CanPlaceInContainer(pck) Then
    '                                '------------------------------------------------------------------------------
    '                                'changed on 09-02-2006 by udi - for the Loading plan breakdown
    '                                'Description: The old code checked if we can place the pick obj on the cont,
    '                                'if we could we moved on. Now we have to check if we can load the container and 
    '                                'can place it on the truck.
    '                                '------------------------------------------------------------------------------
    '                                If ShouldBreakForLoadingPlan Then
    '                                    'check if we can place the pick on the vehicle
    '                                    If LoadingPlanner.CanPlaceContainer(pckcolcontainer, pck) Then
    '                                        'Old code - place the pick and assign it to the current picklist
    '                                        pckcolcontainer.PlaceInContainer(pck)
    '                                        placed = True
    '                                        'pckcol.Remove(pck)
    '                                        'Exit For
    '                                        pckcolTemp.Add(pck)
    '                                    End If
    '                                Else
    '                                    'Old code - place the pick and assigne it to the current picklist
    '                                    'without consideration of the vehicle limitation
    '                                    pckcolcontainer.PlaceInContainer(pck)
    '                                    placed = True
    '                                    'pckcol.Remove(pck)
    '                                    'Exit For
    '                                    pckcolTemp.Add(pck)
    '                                End If
    '                            End If
    '                        End If
    '                    Next

    '                    Dim tempPCK As PicksObject
    '                    'now should remove the picks in pckcolTemp from pckcol

    '                    'and create the loading plan
    '                    'If ShouldBreakForLoadingPlan Then
    '                    '    'Load the current Container(/PickList) on the vehicle
    '                    '    Dim lp As New LoadingPlanner
    '                    '    lp.Post(pckcolcontainer)
    '                    'End If

    '                    pckcols.Remove(pckcolcontainer)

    '                    'and create another one to move on the remain picks
    '                    If pckcol.Count > 0 Then
    '                        pckcolcontainer = New PicksObjectCollection
    '                        pckcolcontainer.PlaceInContainer(_container, pckcol(0))
    '                        pckcol.Remove(pckcol(0))
    '                        pckcols.Add(pckcolcontainer)
    '                        retpckcols.Add(pckcolcontainer)
    '                    End If
    '                End While
    '            End While
    '        Else
    '            'pckcols.Add(pckcol)
    '            retpckcols.Add(pckcol)
    '        End If
    '        Return retpckcols
    '    Else
    '        Return Nothing
    '    End If
    'End Function

    'This Function breaks a picks for a specific customer by the mixPicking/self hnadelingunit Parameters
    'Public Function BreakCustomer(ByVal Picks As PicksObjectCollection, Optional ByVal ShouldBreakForLoadingPlan As Boolean = False) As ArrayList
    '    Dim pckcols As New ArrayList
    '    Dim retpckcols As New ArrayList
    '    Dim pckcol As New PicksObjectCollection
    '    Dim pckcolcontainer As PicksObjectCollection
    '    Dim pck As PicksObject
    '    Dim oComp As Company
    '    Dim oOrd As OutboundOrderHeader
    '    For Each pck In Picks
    '        oOrd = New OutboundOrderHeader(pck.Consignee, pck.OrderId)
    '        oComp = New Company(oOrd.CONSIGNEE, oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE)
    '        If Not oComp.MIXPICKING Then
    '            pckcol.Add(pck)
    '        End If
    '    Next

    '    For Each pck In pckcol
    '        Picks.Remove(pck)
    '    Next

    '    Dim placed As Boolean
    '    If pckcol.Count > 0 Then
    '        If _plancontainer Then
    '            While pckcol.Count > 0
    '                pckcolcontainer = New PicksObjectCollection
    '                pckcolcontainer.PlaceInContainer(_container, pckcol(0))
    '                pckcol.Remove(pckcol(0))
    '                pckcols.Add(pckcolcontainer)
    '                retpckcols.Add(pckcolcontainer)
    '                While pckcols.Count > 0
    '                    For Each pck In pckcol
    '                        placed = False
    '                        If pckcolcontainer.CanPlaceInContainer(pck, True) Then
    '                            '------------------------------------------------------------------------------
    '                            'changed on 09-02-2006 by udi - for the Loading plan breakdown
    '                            'Description: The old code checked if we can place the pick obj on the cont,
    '                            'if we could we moved on. Now we have to check if we can load the container and 
    '                            'can place it on the truck.
    '                            '------------------------------------------------------------------------------
    '                            If ShouldBreakForLoadingPlan Then
    '                                'check if we can place the pick on the vehicle
    '                                If LoadingPlanner.CanPlaceContainer(pckcolcontainer, pck) Then
    '                                    'Old code - place the pick and assigne it to the current picklist
    '                                    pckcolcontainer.PlaceInContainer(pck)
    '                                    placed = True
    '                                    pckcol.Remove(pck)
    '                                    Exit For
    '                                End If
    '                            Else
    '                                'Old code - place the pick and assigne it to the current picklist
    '                                'without consideration of the vehicle limitation
    '                                pckcolcontainer.PlaceInContainer(pck)
    '                                placed = True
    '                                pckcol.Remove(pck)
    '                                Exit For
    '                            End If
    '                        End If
    '                    Next

    '                    'If ShouldBreakForLoadingPlan Then
    '                    '    'Load the current Container(/PickList) on the vehicle
    '                    '    Dim lp As New LoadingPlanner
    '                    '    lp.Post(pckcolcontainer)
    '                    'End If

    '                    pckcols.Remove(pckcolcontainer)

    '                    'and create another one to move on the remain picks
    '                    If pckcol.Count > 0 Then
    '                        pckcolcontainer = New PicksObjectCollection
    '                        pckcolcontainer.PlaceInContainer(_container, pckcol(0))
    '                        pckcol.Remove(pckcol(0))
    '                        pckcols.Add(pckcolcontainer)
    '                        retpckcols.Add(pckcolcontainer)
    '                    End If
    '                End While
    '            End While
    '        Else
    '            'pckcols.Add(pckcol)
    '            retpckcols.Add(pckcol)
    '        End If
    '        Return retpckcols
    '    Else
    '        Return Nothing
    '    End If
    'End Function

#End Region

#End Region

End Class
