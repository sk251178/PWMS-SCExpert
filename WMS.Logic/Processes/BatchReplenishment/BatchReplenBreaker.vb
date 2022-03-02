Imports Made4Net.DataAccess
Imports System.Text

<CLSCompliant(False)> Public Class BatchReplenBreaker

#Region "Variables"

    Protected _cube As Decimal


#End Region

#Region "Properties"

    Public Property Cube() As Decimal
        Get
            Return _cube
        End Get
        Set(ByVal Value As Decimal)
            _cube = Value
        End Set
    End Property
#End Region
#Region "Constructor"
    Public Sub New(ByVal batchheader As BatchReplenHeader)
        Dim batchreplpolicy = New BatchReplenishmentPolicy
        _cube = batchreplpolicy.GetCubeLimit(batchheader.REPLENPOLICY)
    End Sub

#End Region

#Region "Method"

#Region "Partial Breaking"
    Dim _PreviousCubeCapacity As Decimal
    Private Function ExistingBatchReplenHasRoomToFill(ByVal breplencollection As BatchReplenDetailCollection, ByVal brdetail As BatchReplenDetail, ByVal skuUnitVolume As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim cubeVolRemaining As Decimal
        Dim currentReplenLineVol As Decimal

        currentReplenLineVol = brdetail.FROMSKUBASEUOMQTY * skuUnitVolume

        If (_cube > 0) And (breplencollection.CurrVolume > 0) Then
            cubeVolRemaining = _cube - breplencollection.CurrVolume
        End If
        If currentReplenLineVol <= cubeVolRemaining Then
            Return True
        Else
            Return False
        End If

    End Function

    Private Function HasExistingBatchReplenHasRoomToFill(ByVal breplencollection As BatchReplenDetailCollection, ByVal brdetail As BatchReplenDetail, ByVal skuUnitVolume As Decimal, ByRef numUnitsCanFill As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim cubeVolRemaining As Decimal
        Dim numUnitsByCube As Decimal
        cubeVolRemaining = 0
        'Determine the Cube volume remaining after deducting from the defined cube volume
        'Determine the weight remaining after deducting from the defined weight
        If (breplencollection.Count <= 0) Then
            Return False
        End If
        If (_cube > 0) And (breplencollection.CurrVolume > 0) Then
            cubeVolRemaining = _cube - breplencollection.CurrVolume
        End If
        'Determine how much qty of current pick line can fullfill the current picklist
        If (_cube > 0 And skuUnitVolume > 0) Then
            numUnitsByCube = Math.Floor(Math.Round(cubeVolRemaining / skuUnitVolume, 2))
        End If
        If (_cube > 0 And numUnitsByCube > 0) Then
            numUnitsCanFill = numUnitsByCube

        End If
        If numUnitsByCube = 0 Then
            numUnitsCanFill = 0
        End If
        'If Skuunitvolume comes as zero , Allow the full quantity to add the current picklist
        If (numUnitsCanFill >= 1) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function BreakBatchReplens(ByVal batchreplens As BatchReplenDetailCollection, ByVal oLogger As LogHandler) As ArrayList
        Dim breplencols As New ArrayList
        Dim breplencollection As New BatchReplenDetailCollection
        Dim brdetail As BatchReplenDetail
        Dim breplenCollectionArray As New ArrayList

        For Each brdetail In batchreplens
            'If batchreplen does not exist , create a new batchreplen
            If (breplencollection.Count = 0) Then
                Dim newbreplenCollection As BatchReplenDetailCollection = New BatchReplenDetailCollection
                If Not oLogger Is Nothing Then
                    oLogger.Write(" BatchReplenBreaker.BreakPartail - Starting a new BatchReplen to start adding replen lines,replen line sku:" + brdetail.FROMSKU.ToString())
                End If
                breplencollection = newbreplenCollection
            End If
            'Add Replen line to the BatchReplen
            breplenCollectionArray = AddReplenLineToBatchReplen(breplencols, breplencollection, brdetail, oLogger)
            'Add the all the returned BatchReplen collection to the return result
            If breplenCollectionArray.Count > 0 Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(" BatchReplentBreaker.BreakPartail - Number of BatchReplens returned =" + breplenCollectionArray.Count.ToString())
                End If
                For Each batchreplen As WMS.Logic.BatchReplenDetailCollection In breplenCollectionArray
                    If Not oLogger Is Nothing Then
                        oLogger.Write(" BatchReplenBreaker.BreakPartail - Adding the closed BatchReplen to replen collection Array:" + "BatchReplen Total CubeVol=" + breplencollection.CurrVolume.ToString()) 'RWMS-1485
                    End If
                    breplencols.Add(batchreplen)
                Next
            End If
        Next
        If breplenCollectionArray.Count > 0 Then
            If (breplencollection.Count > 0 And breplenCollectionArray.Contains(breplencollection) = False) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Adding the last BatchReplen to BatchReplen collection Array:" + "BatchReplen Total CubeVol=" + breplencollection.CurrVolume.ToString())
                End If
                breplencols.Add(breplencollection)
            End If
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write(" BatchReplenBreaker.BreakPartail - Number of replenlines added to this BatchReplen =" + breplencollection.Count.ToString())
            End If
            If Not breplencollection Is Nothing AndAlso (breplencollection.Count > 0) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("BatchReplenBreaker.BreakPartail - Adding the closed BatchReplen to BatchReplen collection Array:" + "PICKLIST Total CubeVol=" + breplencollection.CurrVolume.ToString())
                End If
                breplencols.Add(breplencollection)
            End If

        End If
        Return breplencols
    End Function

    Private Function AddReplenLineToBatchReplen(ByRef breplencols As ArrayList, ByRef breplenCollection As BatchReplenDetailCollection, ByVal brdetail As BatchReplenDetail, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim batchreplenAL As New ArrayList
        Dim oSku As New WMS.Logic.SKU(brdetail.CONSIGNEE, brdetail.FROMSKU)
        Dim skuUnitVolume As Decimal = Inventory.CalculateVolumeForGivenUOM(brdetail.CONSIGNEE, brdetail.FROMSKU, 1, brdetail.FROMSKUBASEUOM, oSku)
        Dim numUnitsCanFill As Decimal
        Dim currentReplenCubVol As Decimal

        currentReplenCubVol = brdetail.FROMSKUBASEUOMQTY * skuUnitVolume

        If Not oLogger Is Nothing Then
            'Start RWMS-1485
            oLogger.writeSeperator(" ", 20)
            oLogger.Write(" BatchReplenBreaker.AddReplenLineToBatchReplen - Replen Line in process SKU= " + brdetail.FROMSKU + ",FromLoad=" + brdetail.FROMLOAD.ToString() + ",FromSkuQty=" + brdetail.FROMQTY.ToString() + ",BASE UOM=" + brdetail.FROMSKUBASEUOM.ToString() + ",BASE UOM Qty=" + brdetail.FROMSKUBASEUOMQTY.ToString() + ",skuUnitVol=" + skuUnitVolume.ToString())
            Dim strLogString As String = " BatchReplenBreaker.AddReplenLineToBatchReplen - Break Startegy Params - "
            strLogString = strLogString + " ,Cube = " & _cube.ToString()
            oLogger.Write(strLogString)
        End If
        Try
            If (breplenCollection.Count > 0) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("     BatchReplenBreaker.AddReplenLineToBatchReplen - Existing BatchReplen Total CubeVol=" + breplenCollection.CurrVolume.ToString())
                End If
                Dim bReturnValue As Boolean = ExistingBatchReplenHasRoomToFill(breplenCollection, brdetail, skuUnitVolume, oLogger)
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("BatchReplenBreaker.AddReplenLineToBatchReplen - ExistingBatchReplenHasRoomToFill:{0},fromSKU={1},fromQty={2},skuUnitVolume={3},FROMSKUBASEUOMQTY={4},Current Replen Volme={5}", bReturnValue.ToString(), brdetail.FROMSKU, brdetail.FROMQTY.ToString(), skuUnitVolume.ToString(), brdetail.FROMSKUBASEUOMQTY.ToString(), currentReplenCubVol.ToString()))
                End If

                If bReturnValue = True Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     PicklistBreaker.AddPickLineToPickList - HasExistingPickListHasRoomToFill returned TRUE and currentReplenCubVol =" + currentReplenCubVol.ToString() + ",BatchReplen Total cube Volume=" + breplenCollection.CurrVolume.ToString())
                    End If
                    breplenCollection.PlaceInNewBatchReplen(brdetail, oLogger)
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     BatchReplenBreaker.AddReplenLineToBatchReplen - HasExistingBatchReplenHasRoomToFill() returned FALSE")
                    End If
                    'Close the existing BatchReplen and open new one 
                    If (breplenCollection.Count > 0) Then
                        batchreplenAL.Add(breplenCollection)
                        If Not oLogger Is Nothing Then
                            oLogger.Write("     BatchReplenBreaker.AddReplenLineToBatchReplen - Closing the existing BatchReplen and starting a new BatchReplen. " + "BatchReplen Total Vol=" + breplenCollection.CurrVolume.ToString())
                        End If
                        Dim newBatchReplenCollection As BatchReplenDetailCollection = New BatchReplenDetailCollection
                        breplenCollection = newBatchReplenCollection
                    End If
                    breplenCollection.PlaceInNewBatchReplen(brdetail, oLogger)
                    If (_cube > 0) Then
                        If currentReplenCubVol > _cube Then
                            If Not oLogger Is Nothing Then
                                oLogger.Write("     BatchReplenBreaker.AddReplenLineToBatchReplen - Current replen line Volume exceeds the defined cube limit" + "Define Cube Volume=" + _cube.ToString() + ",current replen line cube volume=" + currentReplenCubVol.ToString())
                            End If
                        End If
                    End If

                End If
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("    BatchReplenBreaker.AddReplenLineToBatchReplen - No current BatchReplen exists. Prepared to create a new BatchReplen.")
                End If
                'Close the existing picklist and open new one 
                If (breplenCollection.Count > 0) Then
                    batchreplenAL.Add(breplenCollection)
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     BatchReplenBreaker.AddReplenLineToBatchReplen - Closing the existing BatchReplen and starting a new BatchReplen." + "BatchReplen Total Vol=" + breplenCollection.CurrVolume.ToString())
                    End If
                    Dim newbreplenCollection As BatchReplenDetailCollection = New BatchReplenDetailCollection
                    breplenCollection = newbreplenCollection
                End If
                breplenCollection.PlaceInNewBatchReplen(brdetail, oLogger)
                If (_cube > 0) Then
                    If currentReplenCubVol > _cube Then
                        If Not oLogger Is Nothing Then
                            oLogger.Write("     BatchReplenBreaker.AddReplenLineToBatchReplen - Current replen line Volume exceeds the defined cube limit" + "Define Cube Volume=" + _cube.ToString() + ",current replen line cube volume=" + currentReplenCubVol.ToString())
                        End If
                    End If
                End If

            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(" BatchReplenBreaker.AddReplenLineToBatchReplen - Exception occured,Replen Line in process sku:" + brdetail.FROMSKU.ToString() + ",Error Msg=" + ex.Message.ToString())
            End If
        End Try
        If (Not batchreplenAL Is Nothing) Then
            Return batchreplenAL
        Else
            Return Nothing
        End If
    End Function


    Private Function SplitReplenLineAndAddToBatchReplen(ByRef batchreplenAL As ArrayList, ByRef breplencollection As BatchReplenDetailCollection, ByRef brdetail As BatchReplenDetail, ByVal skuUnitVolume As Decimal, ByRef unitsToBeFullfilled As Integer, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim numUnitsCanSplit As Decimal
        If IsReplenLineSplitRequired(brdetail, skuUnitVolume, numUnitsCanSplit, oLogger) = True Then
            Dim prtSplitPck1, prtSplitPck2 As BatchReplenDetail
            prtSplitPck1 = brdetail.Clone
            prtSplitPck2 = brdetail.Clone
            prtSplitPck1.FROMSKUBASEUOMQTY = numUnitsCanSplit
            Dim oFromSku As New SKU(prtSplitPck1.CONSIGNEE, prtSplitPck1.FROMSKU)
            Dim oToSku As New SKU(prtSplitPck1.CONSIGNEE, prtSplitPck1.TOSKU)
            If prtSplitPck1.FROMSKUUOM = prtSplitPck1.FROMSKUBASEUOM Then
                prtSplitPck1.FROMQTY = prtSplitPck1.FROMSKUBASEUOMQTY
            Else
                prtSplitPck1.FROMQTY = oFromSku.ConvertUnitsToUom(prtSplitPck1.FROMSKUBASEUOM, prtSplitPck1.FROMSKUBASEUOMQTY)
            End If

            If prtSplitPck1.FROMSKU <> prtSplitPck1.TOSKU Then
                prtSplitPck1.TOQTY = oToSku.ConvertParentLowestUomUnitsToMyLowestUomUnits(prtSplitPck1.FROMQTY)
            Else
                prtSplitPck1.TOQTY = oToSku.ConvertUnitsToUom(oFromSku.LOWESTUOM, prtSplitPck1.FROMQTY)
            End If
            prtSplitPck2.FROMQTY = brdetail.FROMQTY - prtSplitPck1.FROMQTY

            prtSplitPck2.TOQTY = brdetail.TOQTY - prtSplitPck1.TOQTY
            prtSplitPck2.FROMSKUBASEUOMQTY = brdetail.FROMSKUBASEUOMQTY - prtSplitPck1.FROMSKUBASEUOMQTY

            If Not oLogger Is Nothing Then
                oLogger.Write(" BatchReplenBreaker.SplitReplenLineAndAddToBatchReplen - Splitting Replen Line. First Split with BASEUOMQty=" + prtSplitPck1.FROMSKUBASEUOMQTY.ToString() + ",Remaing BASEUomQty=" + prtSplitPck2.FROMSKUBASEUOMQTY.ToString()) 'RWMS-1485
            End If

            Dim bReturnValue As Boolean = HasExistingBatchReplenHasRoomToFill(breplencollection, prtSplitPck1, skuUnitVolume, prtSplitPck1.FROMSKUBASEUOMQTY, oLogger)
            If bReturnValue = False Then
                If (breplencollection.Count > 0) Then
                    batchreplenAL.Add(breplencollection)
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     PicklistBreaker.AddReplenLineToBatchReplen - Closing the existing BatchReplen and starting a new BatchReplen. " + "BatchReplen Total Vol=" + breplencollection.CurrVolume.ToString())
                    End If
                    Dim newbreplenCollection As BatchReplenDetailCollection = New BatchReplenDetailCollection
                    breplencollection = newbreplenCollection
                End If

            End If
            'Place the split units in batch replen container 
            breplencollection.PlaceInNewBatchReplen(prtSplitPck1, oLogger)
            unitsToBeFullfilled = unitsToBeFullfilled - prtSplitPck1.FROMSKUBASEUOMQTY
            brdetail = prtSplitPck2
            unitsToBeFullfilled = prtSplitPck2.FROMSKUBASEUOMQTY

        Else
            If Not oLogger Is Nothing Then
                oLogger.Write(" BatchReplenBreaker.SplitReplenLineAndAddToBatchReplen - IsReplenLineSplitRequired returned FALSE")
            End If
            Dim bReturnValue As Boolean = HasExistingBatchReplenHasRoomToFill(breplencollection, brdetail, skuUnitVolume, brdetail.FROMSKUBASEUOMQTY, oLogger)
            If bReturnValue = False Then
                If (breplencollection.Count > 0) Then
                    batchreplenAL.Add(breplencollection)
                    If Not oLogger Is Nothing Then
                        oLogger.Write("     PicklistBreaker.AddReplenLineToBatchReplen - Closing the existing BatchReplen and starting a new BatchReplen. " + "BatchReplen Total Vol=" + breplencollection.CurrVolume.ToString())
                    End If
                    Dim newbreplenCollection As BatchReplenDetailCollection = New BatchReplenDetailCollection
                    breplencollection = newbreplenCollection
                End If

            End If
            breplencollection.PlaceInNewBatchReplen(brdetail, oLogger)
            unitsToBeFullfilled = unitsToBeFullfilled - brdetail.FROMSKUBASEUOMQTY

        End If
        Return True
    End Function


    Private Function IsReplenLineSplitRequired(ByVal brdetail As BatchReplenDetail, ByVal skuUnitVolume As Decimal, ByRef numUnitsCanSplit As Decimal, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim cubeVolRemaining As Decimal
        Dim numUnitsByCube As Decimal
        cubeVolRemaining = 0

        'Determine how much qty of current line can fullfill the defined cube/weight limit
        If (brdetail.FROMSKUBASEUOMQTY = 1) Then
            Return False
        End If

        If (_cube > 0) Then
            If (skuUnitVolume > _cube) Then
                numUnitsCanSplit = 1
                Return True
            End If
        End If



        If (_cube > 0 And skuUnitVolume > 0) Then
            numUnitsByCube = Math.Floor(Math.Floor(_cube / skuUnitVolume))
        End If

        If (_cube > 0) Then
            If (numUnitsByCube < 1) Then
                numUnitsCanSplit = 0
                Return False
            End If

        End If
        If (_cube > 0 And numUnitsByCube > 0) Then
            numUnitsCanSplit = numUnitsByCube
        Else
            numUnitsCanSplit = 0
        End If


        If (numUnitsCanSplit >= 1) Then
            If (brdetail.FROMSKUBASEUOMQTY <= numUnitsCanSplit) Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If

    End Function

#End Region


#End Region
End Class
