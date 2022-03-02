'Begin RWMS-1294 and RWMS-1222
Imports System.Web
'End RWMS-1294 and RWMS-1222
Imports Made4Net.DataAccess
Imports System.Collections.Generic 'RWMS-1306/RWMS-971
<CLSCompliant(False)> Public Class PickTask
    Inherits Task

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Sub ExitTask()
        Dim pck As New Picklist(_picklist)
        pck.UnAssign(_userid)
        MyBase.ExitTask()
    End Sub

    Public Overrides Sub AssignUser(ByVal pUserId As String, Optional ByVal pAssignmentType As String = WMS.Lib.TASKASSIGNTYPE.MANUAL, Optional ByVal userMHType As String = "", Optional ByVal pPriority As Int32 = -1)
        MyBase.AssignUser(pUserId, WMS.Lib.TASKASSIGNTYPE.MANUAL, userMHType)
        Dim pcklst As New Picklist(_picklist)
        pcklst.AssignUser(pUserId)
    End Sub

    Public Overrides Sub Cancel()
        Dim pickListObj As New WMS.Logic.Picklist(Me.Picklist)
        pickListObj.Cancel(WMS.Lib.USERS.SYSTEMUSER, False)
        MyBase.Cancel()
    End Sub

    Public Overrides Sub Complete(ByVal logger As LogHandler, Optional ByVal pProblemRC As String = "")
        'Begin for RWMS-1294 and RWMS-1222

        If Not logger Is Nothing Then
            logger.Write("... Pick Task got Completed......")
        End If
        'End  for RWMS-1294 and RWMS-1222

        If _assigned Then
            'Begin for RWMS-1294 and RWMS-1222
            If Not logger Is Nothing Then
                logger.Write("...Checking whether to create the Delivery Task...")
            End If
            'End for RWMS-1294 and RWMS-1222
            'check if we should create the delivery job at all - if we have pack area or staging lane
            If ShouldCreateDeliveryTask() AndAlso Not Picking.BagOutProcess Then
                'Begin for RWMS-1294 and RWMS-1222
                If Not logger Is Nothing Then
                    logger.Write("...Started Creating Delivery Task...")
                End If
                'End  for RWMS-1294 and RWMS-1222
                ' DO we need to save here
                Dim t As New DeliveryTask
                Dim tm As New TaskManager
                'tm.CreateDeliveryTask(Me, _userid)
                tm.CreateDeliveryTask(Me, _userid, Nothing)
            End If
        End If
        'RWMS-2828
        Dim Sql = String.Format("SELECT LOCATION FROM WHACTIVITY WHERE USERID='{0}'", GetCurrentUser())
        Dim lastActivityLocation As String = DataInterface.ExecuteScalar(Sql)
        ExecutionLocation = lastActivityLocation

        '' If Execution location is not set and remain empty in WHActivity table. Must be set.
        'If MyBase.ExecutionLocation = "" Then
        '    If MyBase.TOLOCATION IsNot Nothing And MyBase.TOLOCATION <> "" Then
        '        MyBase.ExecutionLocation = MyBase.TOLOCATION
        '    Else
        '        MyBase.ExecutionLocation = MyBase.FROMLOCATION
        '    End If
        'End If
        'RWMS-2828
        ' WareHouse
        If MyBase.TOWAREHOUSEAREA IsNot Nothing And MyBase.TOWAREHOUSEAREA <> "" Then
            MyBase.ExecutionWarehousearea = MyBase.TOWAREHOUSEAREA
        Else
            MyBase.ExecutionWarehousearea = MyBase.FROMWAREHOUSEAREA
        End If
        MyBase.Complete(logger)

    End Sub

    Public Shadows Sub ReportProblem(ByVal pPicklist As Picklist, ByVal pProblemCodeId As String, ByVal pLocation As String, ByVal pWHArea As String, ByVal pUserId As String)
        MyBase.ReportProblem(pProblemCodeId, pLocation, pWHArea, pUserId)
        If CompleteTaskOnProblemCode Then
            pPicklist.CompleteList(pUserId, Nothing)
        End If
    End Sub

    Private Function ShouldCreateDeliveryTask() As Boolean
        Dim pcklist As New Picklist(_picklist)
        Return pcklist.GetTotalPickedQty > 0
        
    End Function

    ' Add extra optional out parameter
    Public Shared Function getNextPick(ByRef pcklst As Picklist, ByRef ValidationStatus As Boolean) As PickJob
        Dim pckJob As New PickJob
        Dim jobFound As Boolean = False

        Dim matchRelStrat As ReleaseStrategyDetail
        Dim grpPickDetails As Boolean = False
        'Dim pStrat As New PlanStrategy(pcklst.StrategyId)
        'For Each oRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
        '    If oRelStrat.PickType = pcklst.PickType OrElse oRelStrat.PickType = String.Empty OrElse oRelStrat.PickType Is Nothing Then
        '        grpPickDetails = oRelStrat.GroupPickDetails
        '        matchRelStrat = oRelStrat
        '        Exit For
        '    End If
        'Next

        matchRelStrat = pcklst.getReleaseStrategy
        grpPickDetails = matchRelStrat.GroupPickDetails
        'start for 1420 and RWMS-1412
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not rdtLogger Is Nothing Then
            rdtLogger.Write(" Started PickTask.getNextPick ")
            Try
                rdtLogger.Write(" PickTask.getNextPick : Group Pick Flag - " & grpPickDetails.ToString() & " Release strategy - " & matchRelStrat.StrategyId & " Picklist - " & pcklst.PicklistID & " PickType - " & pcklst.PickType.ToString())
            Catch ex As Exception

            End Try
        End If
        'end for 1420 and RWMS-1412

        While Not jobFound
            'start for 1420 and RWMS-1412
            If Not rdtLogger Is Nothing Then
                rdtLogger.Write(" PickTask.getNextPick : Proceeding to get next job ")
            End If
            'end for 1420 and RWMS-1412
            If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                pckJob = getFullPickJob(pcklst)
            Else
                pckJob = getPartialPickJob(pcklst, grpPickDetails, matchRelStrat.SystemPickShort)
            End If
            'Update the FromLocation of the task
            If Not pckJob Is Nothing Then
                ' Labor calc changes RWMS-952 -> PWMS-903
                ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User only if this is the first pick line
                If pckJob.PickDetLines.Count = 1 AndAlso pckJob.PickDetLines(0) = 1 Then
                    Dim oWHActivity As New WHActivity
                    oWHActivity.USERID = WMS.Logic.GetCurrentUser()
                    oWHActivity.SaveLastLocation()
                End If
                ' Labor calc changes RWMS-952 -> PWMS-903
                ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User only if this is the first pick line
                UpdateTaskFromLocation(pckJob)
            Else
                'start for 1420 and RWMS-1412
                If Not rdtLogger Is Nothing Then
                    rdtLogger.Write(" PickTask.getNextPick : No next job found ")
                End If
                'end for 1420 and RWMS-1412
                ''No partial pick job can be continued, complete all remain details...
                For Each pckdet As PicklistDetail In pcklst.Lines
                    If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                            pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER, False)
                        Else
                            pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER)
                        End If
                    End If
                Next
                pcklst.CompleteList(WMS.Lib.USERS.SYSTEMUSER, rdtLogger)
                Exit While
            End If
            'start for 1420 and RWMS-1412
            If Not rdtLogger Is Nothing Then
                rdtLogger.Write(" PickTask.getNextPick : Proceeding to validate pick job ")
            End If
            'end for 1420 and RWMS-1412

            ' Add Extra Validation Method here.
            If ValidatePickJob(pcklst, pckJob, matchRelStrat) Then
                getTaskConfirmation(pcklst.getReleaseStrategy().ConfirmationType, pckJob)
                jobFound = True
                ValidationStatus = True
            Else
                If Not doesLoadExists(pckJob) Then
                    ValidationStatus = False
                    getTaskConfirmation(pcklst.getReleaseStrategy().ConfirmationType, pckJob)
                    jobFound = True
                End If
                If pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
                    If Not isPickListFromLocationPicLoc(pckJob) Then
                        ValidationStatus = False
                        getTaskConfirmation(pcklst.getReleaseStrategy().ConfirmationType, pckJob)
                        jobFound = True
                    End If
                End If
            End If
        End While

        If Not pckJob Is Nothing AndAlso pckJob.units > pckJob.adjustedunits Then
            pckJob.units = pckJob.adjustedunits
            'Dim pckSku As New SKU(pckJob.consingee, pckJob.sku)
            Dim pckSku As SKU
            If pckJob.oSku Is Nothing Then
                pckSku = New SKU(pckJob.consingee, pckJob.sku)
            Else
                pckSku = pckJob.oSku
            End If
            pckJob.uomunits = pckSku.ConvertUnitsToUom(pckJob.uom, pckJob.units)
        End If
        Return pckJob
    End Function

    Public Shared Function doesLoadExists(ByVal pPickJob As PickJob) As Boolean
        Dim AvailableLoads As New DataTable()
        Dim sql As String = String.Format("select distinct loadid,availableunits from vPickLocAllocationInventory where picklist = {0} and " & _
        "picklistline = {1}", Made4Net.Shared.Util.FormatField(pPickJob.picklist), Made4Net.Shared.Util.FormatField(pPickJob.PickDetLines(0)))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, AvailableLoads)
        Return AvailableLoads.Rows.Count > 0
    End Function

    Public Shared Function isPickListFromLocationPicLoc(ByVal pPickJob As PickJob) As Boolean
        Dim pickLocDs As New DataTable()
        Dim sql As String = String.Format("select LOCATION from PICKLOC where SKU = {0}", Made4Net.Shared.Util.FormatField(pPickJob.sku))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, pickLocDs)
        If pickLocDs.Rows.Count > 0 Then
            Dim pickLoc = pickLocDs.Rows(0)("LOCATION")
            If pickLoc.Equals(pPickJob.fromlocation) Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function getNextPick(ByRef pcklst As Picklist) As PickJob
        Dim pckJob As New PickJob
        Dim jobFound As Boolean = False

        Dim matchRelStrat As ReleaseStrategyDetail
        Dim grpPickDetails As Boolean = False
        'Dim pStrat As New PlanStrategy(pcklst.StrategyId)
        'For Each oRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
        '    If oRelStrat.PickType = pcklst.PickType OrElse oRelStrat.PickType = String.Empty OrElse oRelStrat.PickType Is Nothing Then
        '        grpPickDetails = oRelStrat.GroupPickDetails
        '        matchRelStrat = oRelStrat
        '        Exit For
        '    End If
        'Next

        matchRelStrat = pcklst.getReleaseStrategy
        grpPickDetails = matchRelStrat.GroupPickDetails

        While Not jobFound
            If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                pckJob = getFullPickJob(pcklst)
            Else
                pckJob = getPartialPickJob(pcklst, grpPickDetails, matchRelStrat.SystemPickShort)
            End If
            'Update the FromLocation of the task
            If Not pckJob Is Nothing Then
                ' Labor calc changes RWMS-952 -> PWMS-903
                ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User only if this is the first pick line
                If pckJob.PickDetLines.Count = 1 AndAlso pckJob.PickDetLines(0) = 1 Then
                    Dim oWHActivity As New WHActivity
                    oWHActivity.USERID = WMS.Logic.GetCurrentUser()
                    oWHActivity.SaveLastLocation()
                End If
                ' Labor calc changes RWMS-952 -> PWMS-903
                ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User only if this is the first pick line
                UpdateTaskFromLocation(pckJob)
            Else
                ''No partial pick job can be continued, complete all remain details...
                For Each pckdet As PicklistDetail In pcklst.Lines
                    If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                            pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER, False)
                        Else
                            pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER)
                        End If
                    End If
                Next
                pcklst.CompleteList(WMS.Lib.USERS.SYSTEMUSER, Nothing)
                Exit While
            End If
            ' Add Extra Validation Method here.
            If ValidatePickJob(pcklst, pckJob, matchRelStrat) Then
                getTaskConfirmation(pcklst.getReleaseStrategy().ConfirmationType, pckJob)
                jobFound = True
            End If

        End While

        If Not pckJob Is Nothing AndAlso pckJob.units > pckJob.adjustedunits Then
            pckJob.units = pckJob.adjustedunits
            'Dim pckSku As New SKU(pckJob.consingee, pckJob.sku)
            Dim pckSku As SKU
            If pckJob.oSku Is Nothing Then
                pckSku = New SKU(pckJob.consingee, pckJob.sku)
            Else
                pckSku = pckJob.oSku
            End If
            pckJob.uomunits = pckSku.ConvertUnitsToUom(pckJob.uom, pckJob.units)
        End If
        Return pckJob
    End Function

    Private Shared Function ValidatePickJob(ByVal pPickList As Picklist, ByRef pPickJob As PickJob, ByVal pReleaseStrategy As ReleaseStrategyDetail) As Boolean
        Dim usedLoadsList As New List(Of String) 'RWMS-1306/RWMS-971

        If String.IsNullOrEmpty(pPickJob.fromload) Then
            Dim isFullPick As Boolean = False
            If pPickList.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                isFullPick = True
            End If
            If Not pPickList.Lines.AllocateLines(pPickJob, pReleaseStrategy, isFullPick, usedLoadsList) Then 'RWMS-1306/RWMS-971
                For Each i As Integer In pPickJob.PickDetLines
                    Dim oOrder As New OutboundOrderHeader(pPickList.Lines.PicklistLine(i).Consignee, pPickList.Lines.PicklistLine(i).OrderId)
                    If pPickJob.BasedOnPartPickedLine Then
                        pPickList.Lines.PicklistLine(i).Status = WMS.Lib.Statuses.Picklist.PARTPICKED
                        pPickList.Lines.PicklistLine(i).SystemPickShort(pReleaseStrategy.SystemPickShort, pPickList.Lines.PicklistLine(i).AdjustedQuantity, oOrder, Nothing, WMS.Lib.USERS.SYSTEMUSER)
                        pPickList.Lines.PicklistLine(i).Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                        pPickList.Lines.PicklistLine(pPickList.Lines.PicklistLine(i).PickListLine) = pPickList.Lines.PicklistLine(i)
                    Else
                        Dim adjQty As Decimal = pPickList.Lines.PicklistLine(i).AdjustedQuantity
                        pPickList.Lines.PicklistLine(i).SystemPickShort(pReleaseStrategy.SystemPickShort, adjQty, oOrder, Nothing, WMS.Lib.USERS.SYSTEMUSER)
                        pPickList.Lines.PicklistLine(i).Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                    End If
                Next
                Return False
            End If
        Else
            If WMS.Logic.Load.Exists(pPickJob.fromload) Then
                '    'Allocation is done , need to get pickjob populated with right value start the picking
                Dim pickjobunits As Integer = 0
                For Each i As Integer In pPickJob.PickDetLines
                    If (pPickList.Lines.PicklistLine(i).FromLoad.Length > 0) Then
                        pickjobunits += pPickList.Lines.PicklistLine(i).AdjustedQuantity
                    End If
                Next
                pPickJob.units = pickjobunits
                Dim oSku As WMS.Logic.SKU
                If oSku Is Nothing Then
                    oSku = New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)
                    pPickJob.uomunits = oSku.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
                End If

                Dim oLoad As New Load(pPickJob.fromload)
                    If Not oLoad.ValidatePick(pPickJob.units, pPickJob.fromlocation, pPickJob.fromwarehousearea) OrElse pPickJob.SystemPickShort Then
                        Dim canPick As Boolean = True
                        Dim pickDet As PicklistDetail
                        Dim remainingUnits As Decimal = pPickJob.units
                        For Each i As Integer In pPickJob.PickDetLines
                            pickDet = pPickList.PicklistLine(i)
                            If pickDet.AdjustedQuantity > remainingUnits OrElse oLoad.LOCATION = String.Empty OrElse oLoad.LOCATION <> pickDet.FromLocation Then
                                Dim oOrder As New OutboundOrderHeader(pickDet.Consignee, pickDet.OrderId)
                                'if the status is partpicked it means it is the 2nd try from fullpick, which means we need to system pick short.
                                If pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCancelException OrElse
                                        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCreateException OrElse
                                        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroLeaveOpen Then
                                    If pPickJob.BasedOnPartPickedLine Then
                                        pickDet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED
                                    End If
                                    pickDet.SystemPickShort(pReleaseStrategy.SystemPickShort, pPickList.Lines.PicklistLine(i).Quantity, oOrder, oLoad, WMS.Lib.USERS.SYSTEMUSER)
                                    pickDet.Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                                    canPick = False
                                    'Return False
                                ElseIf oLoad.UNITS = 0 OrElse oLoad.UNITSALLOCATED = 0 OrElse oLoad.IsLimbo OrElse oLoad.LOCATION = String.Empty _
                                    OrElse Not pickDet.FromLocation.Equals(oLoad.LOCATION, StringComparison.OrdinalIgnoreCase) _
                                    OrElse pickDet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then

                                    If pPickJob.BasedOnPartPickedLine Then
                                        pickDet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED
                                    End If

                                    If Not pickDet.FromLocation.Equals(oLoad.LOCATION, StringComparison.OrdinalIgnoreCase) AndAlso
                                                oLoad.UNITSALLOCATED >= pickDet.Quantity - pickDet.PickedQuantity Then
                                        oLoad.unAllocate(pickDet.Quantity - pickDet.PickedQuantity, WMS.Lib.USERS.SYSTEMUSER)
                                    End If

                                    pickDet.SystemPickShort(pReleaseStrategy.SystemPickShort, pickDet.Quantity - pickDet.PickedQuantity, oOrder, oLoad, WMS.Lib.USERS.SYSTEMUSER)
                                    pickDet.Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                                    pPickList.Lines.PicklistLine(pickDet.PickListLine) = pickDet
                                    canPick = False
                                    'Return False
                                Else
                                    Dim qtyToUnallocate As Decimal
                                    Dim pckSKU As WMS.Logic.SKU
                                    If pPickJob.units > oLoad.UNITS Then
                                        pPickJob.units = oLoad.UNITS
                                        Try
                                            If pPickJob.units > 0 Then
                                                If pPickJob.oSku Is Nothing Then
                                                    pckSKU = New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)
                                                    pPickJob.oSku = pckSKU
                                                Else
                                                    pckSKU = pPickJob.oSku
                                                End If
                                                If pickDet.UOM <> pPickJob.uom Then
                                                    pPickJob.uom = pickDet.UOM
                                                End If
                                                pPickJob.uomunits = pckSKU.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
                                                While pPickJob.uomunits = 0
                                                    pPickJob.uom = pckSKU.getNextUom(pPickJob.uom)
                                                    pPickJob.uomunits = pckSKU.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
                                                End While
                                            End If
                                        Catch ex As Exception
                                        End Try
                                    End If
                                    If pPickJob.units > oLoad.UNITSALLOCATED Then
                                        pPickJob.units = oLoad.UNITSALLOCATED
                                        Try
                                            If pPickJob.units > 0 Then
                                                If pPickJob.oSku Is Nothing Then
                                                    pckSKU = New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)
                                                    pPickJob.oSku = pckSKU
                                                Else
                                                    pckSKU = pPickJob.oSku
                                                End If
                                                If pickDet.UOM <> pPickJob.uom Then
                                                    pPickJob.uom = pickDet.UOM
                                                End If
                                                pPickJob.uomunits = pckSKU.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
                                                While pPickJob.uomunits = 0
                                                    pPickJob.uom = pckSKU.getNextUom(pPickJob.uom)
                                                    pPickJob.uomunits = pckSKU.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
                                                End While
                                            End If
                                        Catch ex As Exception
                                        End Try
                                    End If
                                    If pReleaseStrategy.SystemPickShort = WMS.Logic.SystemPickShort.PickPartialLeaveOpen Then
                                        If pickDet.AdjustedQuantity - pickDet.PickedQuantity > remainingUnits Then
                                            qtyToUnallocate = pickDet.AdjustedQuantity - pickDet.PickedQuantity - remainingUnits
                                        End If
                                    Else
                                        If pickDet.Quantity - pickDet.AdjustedQuantity - pickDet.PickedQuantity > 0 Then
                                            qtyToUnallocate = pickDet.Quantity - pickDet.AdjustedQuantity - pickDet.PickedQuantity
                                        End If
                                    End If
                                    pPickJob.SystemPickShort = True
                                    pickDet.SystemPickShort(pReleaseStrategy.SystemPickShort, qtyToUnallocate, oOrder, oLoad, WMS.Lib.USERS.SYSTEMUSER)
                                    If pReleaseStrategy.SystemPickShort <> SystemPickShort.PickPartialLeaveOpen Then
                                        If pickDet.AdjustedQuantity >= remainingUnits Then pickDet.AdjustedQuantity = Math.Min(remainingUnits, pickDet.AdjustedQuantity)
                                        pPickList.Lines.PicklistLine(pickDet.PickListLine) = pickDet
                                    End If
                                    'Return True
                                End If
                            End If
                            remainingUnits -= pickDet.AdjustedQuantity
                        Next
                        Return canPick
                    End If
                End If
            End If
        Return True
    End Function

    Private Shared Sub GetConfirmation(ByVal pcklst As Picklist, ByVal pckJob As PickJob, ByRef pConfirm1 As String, ByRef pConfirm2 As String)
        Dim oRelStrat As ReleaseStrategyDetail = pcklst.getReleaseStrategy()
        Select Case oRelStrat.ConfirmationType
            Case WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
                pConfirm1 = pckJob.fromload
            Case WMS.Lib.Release.CONFIRMATIONTYPE.LOCATION
                pConfirm1 = pckJob.fromlocation
            Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU
                pConfirm1 = pckJob.sku
            Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION
                pConfirm1 = pckJob.sku
                pConfirm2 = pckJob.fromlocation
            Case WMS.Lib.Release.CONFIRMATIONTYPE.SKUUOM
                pConfirm1 = pckJob.sku
                pConfirm2 = pckJob.uom
        End Select
    End Sub

    Private Shared Sub getTaskConfirmation(ByVal pConfType As String, ByVal oPckJob As PickJob)
        Select Case pConfType
            Case WMS.Lib.CONFIRMATIONTYPE.LOAD
                oPckJob.TaskConfirmation = New TaskConfirmationLoad(oPckJob.fromload)
            Case WMS.Lib.CONFIRMATIONTYPE.LOCATION
                oPckJob.TaskConfirmation = New TaskConfirmationLocation(oPckJob.fromlocation, oPckJob.fromwarehousearea)
            Case WMS.Lib.CONFIRMATIONTYPE.NONE
                oPckJob.TaskConfirmation = New TaskConfirmationNone()
            Case WMS.Lib.CONFIRMATIONTYPE.SKU
                oPckJob.TaskConfirmation = New TaskConfirmationSKU(oPckJob.sku)
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATION
                oPckJob.TaskConfirmation = New TaskConfirmationSKULocation(oPckJob.sku, oPckJob.fromlocation, oPckJob.fromwarehousearea)
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATIONUOM
                oPckJob.TaskConfirmation = New TaskConfirmationSKULocationUOM(oPckJob.sku, oPckJob.fromlocation, oPckJob.fromwarehousearea, oPckJob.uom)
            Case WMS.Lib.CONFIRMATIONTYPE.SKUUOM
                oPckJob.TaskConfirmation = New TaskConfirmationSKUUOM(oPckJob.sku, oPckJob.uom)
            Case WMS.Lib.CONFIRMATIONTYPE.UPC
                Dim oSKU As New SKU(oPckJob.consingee, oPckJob.sku)
                oPckJob.TaskConfirmation = New TaskConfirmationUPC(oSKU.UOM(oPckJob.uom).EANUPC)
            Case Else 'Using Load Confirmation as default
                oPckJob.TaskConfirmation = New TaskConfirmationLoad(oPckJob.fromload)
        End Select
    End Sub

    Private Shared Sub UpdateTaskFromLocation(ByVal pckJob As PickJob)
        Dim sql As String = String.Format("select task from tasks where picklist = '{0}'", pckJob.picklist)
        Dim strTaskId As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If WMS.Logic.Task.Exists(strTaskId) Then
            Dim oTask As New Task(strTaskId)
            'Rwms-2828
            If oTask.FROMLOCATION = String.Empty Then
                oTask.FROMLOCATION = pckJob.fromlocation
            End If
            'Rwms-2828 End
            If oTask.STARTTIME = DateTime.MinValue Then
                oTask.STARTTIME = DateTime.Now
            End If
            'Rwms-2828
            If oTask.STARTLOCATION = String.Empty Then
                sql = String.Format("SELECT LOCATION FROM WHACTIVITY WHERE USERID='{0}'", GetCurrentUser())
                Dim taskStartLocation As String = DataInterface.ExecuteScalar(sql)
                oTask.STARTLOCATION = taskStartLocation
            End If
            'oTask.ExecutionLocation = pckJob.fromlocation
            'Rwms-2828 End
            oTask.Save()

            Dim oWHActivity As New WHActivity
            oWHActivity.ACTIVITY = oTask.TASKTYPE
            oWHActivity.LOCATION = pckJob.fromlocation
            'Added for RWMS-1692 and RWMS-1391
            oWHActivity.WAREHOUSEAREA = pckJob.fromwarehousearea
            'Ended for RWMS-1692 and RWMS-1391
            oWHActivity.USERID = WMS.Logic.GetCurrentUser()
            oWHActivity.ACTIVITYTIME = DateTime.Now
            oWHActivity.ADDDATE = DateTime.Now
            oWHActivity.EDITDATE = DateTime.Now
            oWHActivity.ADDUSER = oTask.USERID
            oWHActivity.EDITUSER = oTask.USERID
            oWHActivity.Post()
        End If
    End Sub

    Public Shared Function getNextPickBySku(ByRef pcklst As Picklist, ByVal pSku As String) As PickJob
        '' OLD CODE
        'Dim pckJob As New PickJob
        'Dim matchRelStrat As ReleaseStrategyDetail
        'Dim grpPickDetails As Boolean = False
        'Dim pStrat As New PlanStrategy(pcklst.StrategyId)
        'For Each oRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
        '    If oRelStrat.PickType = pcklst.PickType Or oRelStrat.PickType Is Nothing Then
        '        grpPickDetails = oRelStrat.GroupPickDetails
        '        matchRelStrat = oRelStrat
        '        Exit For
        '    End If
        'Next
        'pckJob = getPartialPickJob(pcklst, grpPickDetails, matchRelStrat.SystemPickShort, pSku)
        ''Update the FromLocation of the task
        'UpdateTaskFromLocation(pckJob)
        'Return pckJob

        Dim pckJob As New PickJob
        Dim jobFound As Boolean = False
        Dim matchRelStrat As ReleaseStrategyDetail
        Dim grpPickDetails As Boolean = False
        Dim pStrat As New PlanStrategy(pcklst.StrategyId)
        For Each oRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
            If oRelStrat.PickType = pcklst.PickType Or oRelStrat.PickType Is Nothing Then
                grpPickDetails = oRelStrat.GroupPickDetails
                matchRelStrat = oRelStrat
                Exit For
            End If
        Next

        While Not jobFound
            ' If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            '   pckJob = getFullPickJob(pcklst)
            'Else
            pckJob = getPartialPickJob(pcklst, grpPickDetails, matchRelStrat.SystemPickShort, pSku)
            ' End If
            'Update the FromLocation of the task
            If Not pckJob Is Nothing Then
                UpdateTaskFromLocation(pckJob)
            Else
                ''No partial pick job can be continued, complete all remain details...
                For Each pckdet As PicklistDetail In pcklst.Lines
                    If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        'If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                        'pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER, False)
                        'Else
                        pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER)
                        'End If
                    End If
                Next
                pcklst.CompleteList(WMS.Lib.USERS.SYSTEMUSER, Nothing)
                Exit While
            End If
            If ValidatePickJob(pcklst, pckJob, matchRelStrat) Then
                getTaskConfirmation(pcklst.getReleaseStrategy().ConfirmationType, pckJob)
                jobFound = True
            End If
        End While

        If Not pckJob Is Nothing AndAlso pckJob.units > pckJob.adjustedunits Then
            pckJob.units = pckJob.adjustedunits
            Dim pckSku As New SKU(pckJob.consingee, pckJob.sku)
            pckJob.uomunits = pckSku.ConvertUnitsToUom(pckJob.uom, pckJob.units)
        End If
        Return pckJob

    End Function

    Public Shared Function getNextPickByLoad(ByRef pckList As Picklist, ByVal fromload As Load) As PickJob
        Dim pckJob As New PickJob
        pckJob = getPartialPickJob(pckList, fromload)
        'Update the FromLocation of the task
        UpdateTaskFromLocation(pckJob)
        Return pckJob
    End Function

    Protected Shared Function getFullPickJob(ByRef pcklst As Picklist) As PickJob
        Dim pckdet As PicklistDetail
        Dim pckjb As New PickJob
        Dim isFirst As Boolean = True
        'Dim oRelStrat As ReleaseStrategyDetail = pcklst.getReleaseStrategy
        For Each pckdet In pcklst.Lines
            'If pckdet.FromLoad = String.Empty Then
            '    pckdet.AllocateLoadFromPickLoc(oRelStrat)
            'End If

            If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                Return Nothing
            End If

            If isFirst Then
                pckjb.consingee = pckdet.Consignee
                pckjb.sku = pckdet.SKU
                pckjb.fromload = pckdet.FromLoad
                pckjb.fromlocation = pckdet.FromLocation
                pckjb.fromwarehousearea = pckdet.FromWarehousearea
                pckjb.uom = pckdet.UOM
                pckjb.originaluom = pckdet.UOM
                pckjb.picklist = pcklst.PicklistID
                pckjb.SystemPickShort = False
                isFirst = False
            End If
            pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
            pckjb.PickDetLines.Add(pckdet.PickListLine)
        Next
        If pckjb.units = 0 Then
            Return Nothing
        End If
        pckjb.adjustedunits = pckjb.units
        Dim pckSku As New SKU(pckjb.consingee, pckjb.sku)
        pckjb.skudesc = pckSku.SKUDESC
        pckjb.oAttributeCollection = getPickAttributes(pckSku)
        Try
            pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
        Catch ex As Exception
        End Try

        Return pckjb
    End Function

    Protected Shared Function getPartialPickJob(ByRef pcklst As Picklist, ByVal pFromLoad As Load) As PickJob
        Dim pckdet As PicklistDetail
        Dim pckjb As New PickJob
        Dim isFirst As String = True
        Dim grpPickDetails As Boolean = False
        Dim pStrat As New PlanStrategy(pcklst.StrategyId)
        For Each oRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
            If oRelStrat.PickType = pcklst.PickType Or oRelStrat.PickType Is Nothing Then
                grpPickDetails = oRelStrat.GroupPickDetails
                Exit For
            End If
        Next

        For Each pckdet In pcklst.Lines
            If pckdet.Status = WMS.Lib.Statuses.Picklist.PLANNED Or pckdet.Status = WMS.Lib.Statuses.Picklist.RELEASED Then
                If pFromLoad.LOADID = pckdet.FromLoad Then
                    pckjb.consingee = pckdet.Consignee
                    pckjb.sku = pckdet.SKU
                    pckjb.fromload = pckdet.FromLoad
                    pckjb.fromlocation = pckdet.FromLocation
                    pckjb.fromwarehousearea = pckdet.FromWarehousearea
                    pckjb.originaluom = pckdet.UOM
                    pckjb.uom = pckdet.UOM
                    pckjb.picklist = pcklst.PicklistID
                    pckjb.SystemPickShort = False
                    isFirst = False
                    pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                    Exit For
                End If
            End If
        Next

        If pckjb.units = 0 Then
            'Old Code
            'Return Nothing
            isFirst = True
            'Find Partial Picked Line to Continue with...
            For Each pckdet In pcklst.Lines
                If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    If pckdet.CanBePartialPicked() Then
                        If isFirst Then
                            pckjb.consingee = pckdet.Consignee
                            pckjb.sku = pckdet.SKU
                            pckjb.fromload = pckdet.FromLoad
                            pckjb.fromlocation = pckdet.FromLocation
                            pckjb.fromwarehousearea = pckdet.FromWarehousearea
                            pckjb.originaluom = pckdet.UOM
                            pckjb.uom = pckdet.UOM
                            pckjb.picklist = pcklst.PicklistID
                            pckjb.SystemPickShort = False
                            isFirst = False
                            pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                            If Not grpPickDetails Then Exit For
                        Else
                            If pckdet.FromLoad = pckjb.fromload And pckdet.UOM = pckjb.uom Then
                                pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                            End If
                        End If
                    End If
                End If
            Next
            If pckjb.units = 0 Then
                'No partial pick job can be continued, complete all remain details...
                For Each pckdet In pcklst.Lines
                    If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        pckdet.CompleteDetail(WMS.Lib.USERS.SYSTEMUSER)
                    End If
                Next
                pcklst.CompleteList(WMS.Lib.USERS.SYSTEMUSER, Nothing)
                Return Nothing
            End If
        End If
        Dim pckSku As New SKU(pckjb.consingee, pckjb.sku)
        pckjb.skudesc = pckSku.SKUDESC
        pckjb.oAttributeCollection = getPickAttributes(pckSku)
        Try
            pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
            While pckjb.uomunits = 0
                pckjb.uom = pckSku.getNextUom(pckjb.uom)
                pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
            End While
        Catch ex As Exception

        End Try
        'Validate System Pick Short
        Dim oLoad As New Load(pckjb.fromload)
        If Not oLoad.ValidatePick(pckjb.units, pckjb.fromlocation, pckjb.fromwarehousearea) Then
            If pckjb.units > oLoad.UNITS Then
                pckjb.units = oLoad.UNITS
            End If
            pckjb.SystemPickShort = True
        End If
        Return pckjb
    End Function

    Protected Shared Function getPartialPickJob(ByRef pcklst As Picklist, ByVal pGroupPickDetail As Boolean, ByVal pSystemPickShortType As String, Optional ByVal pSku As String = Nothing) As PickJob
        Dim pckdet As PicklistDetail
        Dim pckjb As New PickJob
        Dim isFirst As String = True
        Dim lineIdx As Int32 = 0
        Dim pickstatchk As String 'RWMS-3736
        Dim picklstlnidx As Int32 = 0 'RWMS-3736
        'pckdet = pcklst.Lines(0)

        'RWMS-3736
        'check if pickorder status is reverse...
        pickstatchk = pcklst.PICKORDERSTATUS
        If pickstatchk.ToUpper = "REVERSE" Then              'if yes order pickline in desc  ....
            picklstlnidx = pcklst.Lines.Count - 1
            pckdet = pcklst.Lines(picklstlnidx)
        Else
            pckdet = pcklst.Lines(0)
        End If
        'RWMS-3736

        While Not pckdet Is Nothing
            If pckdet.Status = WMS.Lib.Statuses.Picklist.PLANNED Or pckdet.Status = WMS.Lib.Statuses.Picklist.RELEASED Then
                ''Check for the status after over allocation - the line may have been cancelled
                'If pckdet.Status = WMS.Lib.Statuses.Picklist.PLANNED Or pckdet.Status = WMS.Lib.Statuses.Picklist.RELEASED Then
                If Not pSku Is Nothing Then  'Should get a specific sku to pick
                    If pSku = pckdet.SKU Then
                        pckjb.consingee = pckdet.Consignee
                        pckjb.sku = pckdet.SKU
                        pckjb.fromload = pckdet.FromLoad
                        pckjb.fromlocation = pckdet.FromLocation
                        pckjb.fromwarehousearea = pckdet.FromWarehousearea
                        pckjb.originaluom = pckdet.UOM
                        pckjb.uom = pckdet.UOM
                        pckjb.picklist = pcklst.PicklistID
                        pckjb.SystemPickShort = False
                        isFirst = False
                        pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                        If Not pGroupPickDetail Then Exit While
                    End If
                Else   'act normally and return the next pick
                    If isFirst Then
                        pckjb.consingee = pckdet.Consignee
                        pckjb.sku = pckdet.SKU
                        pckjb.fromload = pckdet.FromLoad
                        pckjb.fromlocation = pckdet.FromLocation
                        pckjb.fromwarehousearea = pckdet.FromWarehousearea
                        pckjb.originaluom = pckdet.UOM
                        pckjb.uom = pckdet.UOM
                        pckjb.picklist = pcklst.PicklistID
                        pckjb.SystemPickShort = False
                        isFirst = False
                        pckjb.PickDetLines.Add(pckdet.PickListLine)
                        pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                        If Not pGroupPickDetail Then Exit While
                    Else
                        If Not String.IsNullOrEmpty(pckdet.FromLoad) Then
                            If pckdet.UOM = pckjb.uom AndAlso pckdet.SKU = pckjb.sku AndAlso pckdet.Consignee = pckjb.consingee Then
                                pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                                pckjb.PickDetLines.Add(pckdet.PickListLine)
                            End If
                        Else
                            If pckdet.FromLocation = pckjb.fromlocation AndAlso pckdet.UOM = pckjb.uom AndAlso
                            pckdet.SKU = pckjb.sku AndAlso pckdet.Consignee = pckjb.consingee Then
                                pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                                pckjb.PickDetLines.Add(pckdet.PickListLine)
                            End If
                        End If
                    End If
                End If
                'End If
            End If
            If pickstatchk.ToUpper = "REVERSE" Then                         'RWMS-3736
                pcklst.Lines(picklstlnidx) = pckdet
                picklstlnidx -= 1
                If picklstlnidx <= pcklst.Lines.Count - 1 Then
                    If picklstlnidx < 0 Then
                        pckdet = Nothing
                    Else
                        pckdet = pcklst.Lines(picklstlnidx)
                    End If
                End If
            Else
                pcklst.Lines(lineIdx) = pckdet
                lineIdx += 1
                If lineIdx <= pcklst.Lines.Count - 1 Then
                    pckdet = pcklst.Lines(lineIdx)
                Else
                    pckdet = Nothing
                End If
            End If
        End While


        If pckjb.units = 0 Then
            isFirst = True
            'Find Partial Picked Line to Continue with...
            lineIdx = 0
            pckdet = pcklst.Lines(0)
            While Not pckdet Is Nothing
                If pckdet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    'If pckdet.CanBePartialPicked() Then
                    If isFirst Then
                        'Start RWMS-1306/RWMS-971 - if it is a last picklist line, then complete the picklist, but donot create the new picklist line
                        If pcklst.Lines.Count = pckdet.PickListLine Then
                            pckdet.NoSplitPartPickedLine()
                            Exit While
                        End If
                        'End RWMS-1306/RWMS-971
                        Dim newPickDetail As PicklistDetail = pckdet.SplitPartPickedLine()
                        pcklst.Lines.Add(newPickDetail)
                        pckdet = newPickDetail
                        pckjb.consingee = pckdet.Consignee
                        pckjb.sku = pckdet.SKU
                        pckjb.fromload = pckdet.FromLoad
                        pckjb.fromlocation = pckdet.FromLocation
                        pckjb.fromwarehousearea = pckdet.FromWarehousearea
                        pckjb.originaluom = pckdet.UOM
                        pckjb.uom = pckdet.UOM
                        pckjb.picklist = pcklst.PicklistID
                        pckjb.SystemPickShort = False
                        pckjb.BasedOnPartPickedLine = True
                        isFirst = False
                        pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                        pckjb.PickDetLines.Add(pckdet.PickListLine)
                        If Not pGroupPickDetail Then Exit While
                    Else
                        If Not String.IsNullOrEmpty(pckdet.FromLoad) Then
                            If pckdet.UOM = pckjb.uom AndAlso pckdet.SKU = pckjb.sku AndAlso pckdet.Consignee = pckjb.consingee Then
                                Dim newPickDetail As PicklistDetail = pckdet.SplitPartPickedLine()
                                pcklst.Lines.Add(newPickDetail)
                                pckdet = newPickDetail

                                pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                                pckjb.PickDetLines.Add(pckdet.PickListLine)
                            End If
                        Else
                            If pckdet.FromLocation = pckjb.fromlocation AndAlso pckdet.UOM = pckjb.uom AndAlso _
                            pckdet.Consignee = pckjb.consingee AndAlso pckdet.SKU = pckjb.sku Then
                                Dim newPickDetail As PicklistDetail = pckdet.SplitPartPickedLine()
                                pcklst.Lines.Add(newPickDetail)
                                pckdet = newPickDetail

                                pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                                pckjb.PickDetLines.Add(pckdet.PickListLine)
                            End If
                        End If
                    End If
                    'End If
                    'End If

                    'pcklst.Lines(lineIdx) = pckdet
                    pcklst.Lines.PicklistLine(pckdet.PickListLine) = pckdet
                End If
                lineIdx += 1
                If lineIdx <= pcklst.Lines.Count - 1 Then
                    pckdet = pcklst.Lines(lineIdx)
                Else
                    pckdet = Nothing
                End If
            End While
            If pckjb.units = 0 Then
                Return Nothing
            End If
        End If
        pckjb.adjustedunits = pckjb.units
        Dim pckSku As New SKU(pckjb.consingee, pckjb.sku)
        pckjb.skudesc = pckSku.SKUDESC
        pckjb.oSku = pckSku
        pckjb.oAttributeCollection = getPickAttributes(pckSku)
        Try
            pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
            While pckjb.uomunits = 0
                pckjb.uom = pckSku.getNextUom(pckjb.uom)
                pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
            End While
        Catch ex As Exception
        End Try

        Return pckjb
    End Function


    Protected Shared Sub AllocateFromPickLocation(ByRef pcklst As Picklist, ByVal pckdet As PicklistDetail, ByVal oReleaseStratDetail As ReleaseStrategyDetail)
        Dim newPicks As ArrayList
        newPicks = pckdet.AllocateLoadFromPickLoc(oReleaseStratDetail)
        'If we have new lines after allocating than add it to picklist lines collection
        'If Not newPicks Is Nothing Then
        'For i As Int32 = 0 To newPicks.Count - 1
        '    Dim tempDet As PicklistDetail = newPicks(i)
        '    pcklst.Lines.Insert(tempDet.PickListLine - 1, tempDet)
        'Next
        pcklst = New Picklist(pcklst.PicklistID)
        'End If
    End Sub

    Protected Shared Function getPickAttributes(ByVal oSku As SKU) As AttributesCollection
        Dim oAttributeCol As AttributesCollection
        Dim objSkuClass As WMS.Logic.SkuClass = oSku.SKUClass
        If Not objSkuClass Is Nothing Then
            If objSkuClass.CaptureAtPickingLoadAttributesCount > 0 Then
                oAttributeCol = New AttributesCollection
                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    If oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                        oAttributeCol.Add(oAtt.Name, Nothing)
                    End If
                Next
            End If
        End If
        Return oAttributeCol
    End Function
    Public Shared Function Pick(ByRef pcklst As Picklist, ByVal pck As PickJob, ByVal pUser As String, ByVal logger As LogHandler, Optional ByVal ShouldReturnNextPick As Boolean = True, Optional ByVal SecondConfirmation As String = Nothing, Optional ByVal shouldConfirm As Boolean = True) As PickJob

        If Not logger Is Nothing Then
            logger.Write(" Started PickTask.Pick for user " & pUser)
        End If
        If Not (Math.Round(pck.pickedqty) = pck.pickedqty) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "only full units can be confirmed.", "only full units can be confirmed.")
        End If
        Dim oWHActivity As New WHActivity()
        oWHActivity.LOCATION = pck.fromlocation
        oWHActivity.WAREHOUSEAREA = pck.fromwarehousearea
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.EDITDATE = DateTime.Now
        oWHActivity.EDITUSER = pUser
        oWHActivity.Post()
        If pck.uomunits < 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Quantity", "Wrong Quantity")
        End If
        If pck.uomunits >= 0 And shouldConfirm Then
            If Not logger Is Nothing Then
                logger.Write(" PickTask.Pick : Proceeding to do task confirmation ")
            End If
            pck.TaskConfirmation.Confirm()
        End If
        Dim oPicking As New Picking
        oPicking.Pick(pcklst, pck, pUser, logger)
        EvacuateEmptyContainer(pck, pUser)
        If Not logger Is Nothing Then
            logger.Write(" PickTask.Pick : Picklist Status " & pcklst.Status)
        End If
        If pcklst.Status = WMS.Lib.Statuses.Picklist.COMPLETE Or pcklst.Status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Return Nothing
        Else
            If ShouldReturnNextPick Then
                If Not logger Is Nothing Then
                    logger.Write(" PickTask.Pick : Proceeding to return next pick ")
                End If
                Return getNextPick(pcklst)
            Else
                Return Nothing
            End If
        End If
    End Function
    Private Shared Sub EvacuateEmptyContainer(ByVal pck As PickJob, ByVal pUser As String)
        Dim dt As DataTable = New DataTable
        Try
            Dim oLoad As New Load(pck.fromload)
            If oLoad.UNITS = 0 Then
                oLoad.EvacuateEmptyContainer(pUser)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CreateZPickingReturnPWTasks(ByVal pck As PickJob, ByVal pUser As String)
        Dim dt As DataTable = New DataTable
        Try
            Dim oLoad As New Load(pck.fromload)
            If oLoad.UNITS > 0 Then
                If oLoad.ContainerId = "" Then
                    'Need to create load pw task
                    If oLoad.UNITSALLOCATED = 0 Then

                    End If
                Else
                    'Need to create container pw task
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Shared Function UpdateSubstituteLoad(ByVal newLoadid As String, ByVal pcklst As Picklist, ByRef pck As PickJob, ByVal pUser As String, ByVal pLogDir As String, ByRef errmsg As String) As Boolean

        Dim ld As Load
        Dim tmpload As Load
        If WMS.Logic.Load.ValidLoadExists(newLoadid) Then
            ld = New Load(newLoadid)
        Else
            errmsg = "Load does not exist/criteria"
            Return False
            ''Throw New Made4Net.Shared.M4NException(New Exception, "Load does not exist/criteria", "Load does not exist/criteria")
        End If

        Dim oOrder As New OutboundOrderHeader(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId)
        If Not ld.ValidShelfLife(oOrder.TARGETCOMPANY, LogHandler.GetRDTLogger()) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Shelf life validation failed", "Shelf life validation failed for the selected load.")
        End If
        tmpload = New Load(pck.fromload)
        Dim assigned As Boolean = False
        Dim assignedUserName As String = String.Empty
        Dim sqlAssigned As String = String.Format("SELECT ASSIGNED,ISNULL(USERID,'') as AssignedUserName FROM TASKS WHERE STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND FROMLOAD = '{0}' AND FROMLOCATION='{1}'", ld.LOADID, ld.LOCATION)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sqlAssigned, dt)
        If (dt.Rows.Count > 0) Then
            dr = dt.Rows(0)
            assigned = dr.Item("ASSIGNED")
            assignedUserName = dr.Item("AssignedUserName")
            If assigned = True AndAlso (String.Compare(pUser.ToUpper(), assignedUserName.ToUpper()) <> 0) Then
                errmsg = "Substitue load associated Task is assigned to User - " & assignedUserName
                Return False
            End If
        End If

        If ((ld.UNITSALLOCATED >= 0) AndAlso String.IsNullOrWhiteSpace(ld.ACTIVITYSTATUS)) OrElse
             ((ld.UNITSALLOCATED > 0) AndAlso (ld.ACTIVITYSTATUS.ToUpper = "REPLPEND")) Then

            Dim pckdet As PicklistDetail
            'check for the sku of the load and the attributes and available units for the pick job
            If pck.consingee <> ld.CONSIGNEE Or ld.SKU <> pck.sku Or pck.uom <> ld.LOADUOM Then
                errmsg = "Sku does not match"
                Return False
            End If
            If pck.units > ld.UNITS Then
                errmsg = "Load Units short"
                Return False
            End If
            If tmpload.UNITS <> ld.UNITS Then
                errmsg = "Loads Units Does Not Match"
                Return False
            End If

            If (ld.UNITSALLOCATED = 0) AndAlso String.IsNullOrWhiteSpace(ld.ACTIVITYSTATUS) Then
                'if we can pick the quantity from the current load then allocate the new load with the pck qty
                ld.Allocate(pck.units, WMS.Lib.USERS.SYSTEMUSER)
                tmpload.unAllocate(pck.units, WMS.Lib.USERS.SYSTEMUSER)
            End If
            'update the pickdetail
            For Each pckdet In pcklst.Lines
                If pckdet.FromLoad = tmpload.LOADID And pckdet.UOM = pck.originaluom And pckdet.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And pckdet.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                    If (ld.UNITSALLOCATED = 0) AndAlso String.IsNullOrWhiteSpace(ld.ACTIVITYSTATUS) Then
                        pckdet.ChangeLoad(ld, WMS.Lib.USERS.SYSTEMUSER)
                    Else
                        pckdet.ChangeLoad(ld, tmpload, WMS.Lib.USERS.SYSTEMUSER, pLogDir, pckdet.PickListLine)
                    End If
                End If
            Next

            'update pick job and continue with the regular pick method
            pck.fromload = ld.LOADID
            pck.fromlocation = ld.LOCATION
            pck.fromwarehousearea = ld.WAREHOUSEAREA
            pck.TaskConfirmation = New TaskConfirmationLoad(pck.fromload)
        End If
        Return True
    End Function

    'End Added for RWMS-167

    ' Update End Time : RWMS-1497
    Public Shared Sub UpdateCompletionTime(ByVal pickLst As WMS.Logic.Picklist)
        Dim now As DateTime = DateTime.Now
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not rdtLogger Is Nothing Then
            rdtLogger.Write("Updating the Pick Task Completion time(After label printing) to : " & now.ToString())
        End If
        'RWMS 2869
        'Dim sql = String.Format("select TASK from tasks where PICKLIST = '{0}'", pickLst.PicklistID)
        Dim sql = String.Format("select TASK from tasks where PICKLIST = '{0}' And STATUS='COMPLETE'", pickLst.PicklistID)
        Dim taskId = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
		If String.IsNullOrEmpty(taskId) Then
            Return
        End If
        Dim task As Task = New Task(taskId, True)

        sql = String.Format("Update PICKHEADER Set COMPLETEDDATE = '{0}', EDITDATE = '{0}' where PICKLIST = '{1}'", now.ToString(), pickLst.PicklistID)
        Made4Net.DataAccess.DataInterface.RunSQL(sql)

        sql = String.Format("update WHACTIVITY set ACTIVITYTIME = '{0}', PREVIOUSACTIVITYTIME='{0}',  EDITDATE='{0}' where USERID = '{1}'", now.ToString(), WMS.Logic.GetCurrentUser)
        Made4Net.DataAccess.DataInterface.RunSQL(sql)

        'RWMS 2869
        If task.STARTTIME = DateTime.MinValue Then
            If Not rdtLogger Is Nothing Then
                rdtLogger.Write("Task starttime not set using now as starttime.")
            End If
            task.STARTTIME = DateTime.Now
        End If


        Dim execTime As Long = DateDiff(DateInterval.Second, task.STARTTIME, now)

        sql = String.Format("Update TASKS Set ENDTIME = '{0}', EDITDATE='{0}', EXECUTIONTIME = '{1}' where TASK = '{2}'", now.ToString(), execTime, taskId)
        Made4Net.DataAccess.DataInterface.RunSQL(sql)


        ' Need to Raise Event to Labor to update laborperformanceaudit record

        Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformanceOnComplete")
        'A  task is completeted -> Labor performance audit entry will be available to be updated.
        If ShowLaborPerformance = "1" Then
            Dim oQ As New Made4Net.Shared.SyncQMsgSender
            Dim oMsg As System.Messaging.Message
            oQ.Add("TASK", taskId)
            oQ.Add("USERID", WMS.Logic.GetCurrentUser)
            oQ.Add("EVENT", WMS.Logic.WMSEvents.EventType.LaborTaskUpdated)
            oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
            oQ.Add("UPDATEDENDDATE", now.ToString())
            oQ.Add("EXECUTIONTIME", execTime.ToString())

            oMsg = oQ.Send(WMS.Lib.MSMQUEUES.LABORSYNC)
            Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)

            If Not String.IsNullOrEmpty(qm.Values("ERROR")) Then
                Throw New ApplicationException(qm.Values("ERROR"))
            End If
        End If


    End Sub
    ' Update End Time : RWMS-1497
End Class