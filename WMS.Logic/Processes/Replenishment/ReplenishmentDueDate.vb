Imports System.Collections.Generic
Imports Made4Net.DataAccess
'
'
' Implementation of requirement of RWMS-2604
'
'
'

''' <summary>
''' Implementation of RWMS-2604
''' </summary>
''' <remarks></remarks>
Public Class ReplenishmentDueDate

    ' Implementation of RWMS-2604
    ' OrderPalnned
    ' WavePlaned
    ' WaveRelease
    Public Shared Sub EstimateDueDateOnCreate(ByVal orderId As String, ByVal replid As String, Optional ByVal oLogger As LogHandler = Nothing)
        Try
            Dim replen As Replenishment = New Replenishment(replid)
            Dim dueDate As DateTime = DateTime.Now
            WriteLog("Estimating due date on create of replen", oLogger)
            dueDate = DeriveDueDateForReplen(orderId, oLogger)
            WriteLog(String.Format("Estimated Due Date: {0}", dueDate.ToString()), oLogger)
            Replenishment.SetReplenDueDateTime(replid, dueDate, oLogger)
            WriteLog("Estimation of due date on create of replen", oLogger)
        Catch ex As Exception
            WriteLog("ReplenishmentDueDate.EstimateDueDateOnCreate threw exception : " + ex.StackTrace, oLogger)
        End Try

    End Sub

    ' Implementation of RWMS-2604
    ' Sets the duedate to current date, to be used for:
    ' If a replenishment is created outside of the planner service (example due to an inventory adjustment then use
    ' current date for the replenishment.duedatetime
    Public Shared Sub EstimateDueDate(ByVal replid As String, Optional ByVal oLogger As LogHandler = Nothing)
        Try
            Dim replen As Replenishment = New Replenishment(replid)
            Dim dueDate As DateTime = DateTime.Now
            WriteLog("Estimation of due date on creation outside of the planner service", oLogger)
            Replenishment.SetReplenDueDateTime(replid, dueDate, oLogger)
        Catch ex As Exception
            WriteLog("ReplenishmentDueDate.EstimateDueDate threw exception : " + ex.StackTrace, oLogger)
        End Try
    End Sub

    ' PickListAssign
    ''' <summary>
    ''' If labor standards are being used, compute a case per hour (CPH) as (goal time / total units on picklist)
    '''
    ''' If labor standards are not being used, use a case per hour (CPH) value that was be added to the picking policy planstrategyheader.CasesPerHour.
    '''
    ''' Calculate a time to pick a case in minutes: (60 / CPH)
    ''' To calculate in seconds: (60 / CPH) * 60
    ''' Example:
    ''' If CPH is 240, time to pick a case = (60 / 240) = .25 minutes
    ''' 		.25 minutes = 15 seconds (.25 * 60)
    ''' If CPH is 250, time to pick a case = (60 / 250) = .24 minutes
    ''' .24 minutes = 14.4 seconds (.24 * 60)
    ''' If CPH is 200, time to pick a case = (60 / 200) = .30 minutes
    ''' .30 minutes = 18 seconds (.30 * 60)
    '''
    ''' For each SKU on the picklist where the pickloc quantity is less than the quantity to be picked, compute the estimated time the selector will arrive at the pick location.
    ''' 1.	Take the start date/time of the PARPICK task
    ''' 2.	Compute the previous cases in the picklist
    ''' 3.	Multiple the previous cases (from #2) by the # seconds to pick each case
    ''' 4.	Add the # of seconds from #3 to the start date/time from #1
    ''' 5.	This is the estimated date/time the selector will reach the pick location
    '''
    ''' If the estimated date/time is less than the current due date/time on the task (replenishment.duedatetime), update the due date/time on the replenishment task.
    '''
    ''' </summary>
    ''' <param name="pTaskID"></param>
    ''' <remarks></remarks>
    Public Shared Sub ReEstimateDueDateOnPickListAssign(ByVal pTaskID As String, Optional ByVal oLogger As LogHandler = Nothing)
        Try
            Dim cph As Decimal = 0.0
            Dim task As Task = New Task(pTaskID)
            WriteLog("Reestimation of due date on PickList assignment for Task :" & pTaskID, oLogger)
            WriteLog("PickList ID:" & task.Picklist, oLogger)
            Dim timeToPickACaseInSecs As Decimal = 0.0
            If Not String.IsNullOrEmpty(task.Picklist) AndAlso task.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING Then
                Dim pcklist As Picklist = New Picklist(task.Picklist)
                Dim pickdetailcol As PicklistDetailCollection = New PicklistDetailCollection(pcklist.PicklistID)
                Dim totalUnitsOnPicklist As Integer = 0
                Dim skus As List(Of String) = New List(Of String)
                For i As Integer = 1 To pickdetailcol.Count
                    Dim oPickListDetail As PicklistDetail = pcklist.Lines.PicklistLine(i)
                    totalUnitsOnPicklist = totalUnitsOnPicklist + oPickListDetail.Quantity
                    If Not skus.Contains(oPickListDetail.SKU) Then
                        skus.Add(oPickListDetail.SKU)
                    End If
                Next
                WriteLog("Total Units on PickList:" & totalUnitsOnPicklist, oLogger)
                'determine if labor standards is being used
                If IfLaborTurnedOn(oLogger) Then
                    ' Goal time is task STDTIME
                    WriteLog("Labor is turned on, Task std time: " & task.STDTIME, oLogger)
                    cph = task.STDTIME / totalUnitsOnPicklist
                    WriteLog("Labor is turned on, Calculated Case Per Hour: " & cph, oLogger)
                Else
                    WriteLog("Labor is turned off, fetching from picking policy", oLogger)
                    Dim planstrategyheader As PlanStrategy = New PlanStrategy(pcklist.StrategyId)
                    cph = planstrategyheader.CASEPERHOUR
                    WriteLog("Labor is turned  off, Calculated Case Per Hour: " & cph, oLogger)
                End If
                'Calculate time to pick a case in minutes
                If cph > 0 Then
                    timeToPickACaseInSecs = (60 / cph) * 60
                Else
                    timeToPickACaseInSecs = 0

                    WriteLog("Calculate Time to Pick a case per sec: " & timeToPickACaseInSecs, oLogger)
                    WriteLog("Try to update replen duedate as currentdate ", oLogger)
                    For Each sku As String In skus
                        Dim oPickListDetail As PicklistDetail = FindPickDetailForSKU(sku, pcklist)
                        Dim pickloc As PickLoc = New PickLoc(oPickListDetail.FromLocation, oPickListDetail.FromWarehousearea, oPickListDetail.Consignee, oPickListDetail.SKU)
                        WriteLog("Pickloc" & pickloc.Location, oLogger)

                        Dim priority As PrioritizeReplenishments = New PrioritizeReplenishments()
                        Dim priorityImmediate As String = priority.GetTaskPriorityImmediate()
                        'RWMS-2604 Commented
                        'Dim sql As String = String.Format("select Top(1) TASK from Tasks inner join Replenishment on Tasks.REPLENISHMENT = REPLENISHMENT.REPLID  where Tasks.TASKTYPE  = 'NEGTREPL' AND Tasks.TOLOCATION = '{0}' AND Tasks.PRIORITY = '{1}' order by REPLENISHMENT.DUEDATETIME desc", pickloc.Location, priorityImmediate)
                        'RWMS-2604 Commented END
                        'RWMS-2604
                        Dim sql As String = String.Format("select Top(1) TASK from Tasks inner join Replenishment on Tasks.REPLENISHMENT = REPLENISHMENT.REPLID  where Tasks.TASKTYPE  = 'NEGTREPL' AND Tasks.TOLOCATION = '{0}' AND Tasks.PRIORITY = '{1}' AND Tasks.STATUS not in ('COMPLETE','CANCELED') order by REPLENISHMENT.DUEDATETIME desc", pickloc.Location, priorityImmediate)
                        'RWMS-2604 END
                        Dim replenTask = DataInterface.ExecuteScalar(sql)
                        WriteLog("Found replenishment, considering to update due date: taskid : " & replenTask, oLogger)

                        If Not String.IsNullOrEmpty(replenTask) Then
                            Dim replTask As Task = New Task(replenTask)
                            Dim estimateddate As DateTime = DateTime.Now
                            Replenishment.SetReplenDueDateTime(replTask.Replenishment, estimateddate, oLogger)
                            WriteLog("updated replenishment duedate as currentdate : " & estimateddate, oLogger)
                        End If
                    Next
                    Exit Sub

                End If
                WriteLog("Calculate Time to Pick a case per sec: " & timeToPickACaseInSecs, oLogger)

                ' Compute estimated time selector will arrive at pick location
                For Each sku As String In skus
                    Dim oPickListDetail As PicklistDetail = FindPickDetailForSKU(sku, pcklist)
                    Dim pickloc As PickLoc = New PickLoc(oPickListDetail.FromLocation, oPickListDetail.FromWarehousearea, oPickListDetail.Consignee, oPickListDetail.SKU)
                    WriteLog("Compute estimated time selector will arrive at pick location for Pickloc" & pickloc.Location, oLogger)
                    ' where the pickloc quantity is less than the quantity to be picked
                    Dim prevcases As Int32 = 0
                    If pickloc.CurrentQty < oPickListDetail.Quantity Then
                        prevcases = FindPreviousCaseCountForSKU(pcklist)
                    Else
                        WriteLog("Pickloc quantity is not less than the quantity to be pick, PreviousCaseCount: " & prevcases, oLogger)
                    End If

                    'Dim estimatedSelectorArrivalTime As DateTime = task.STARTTIME.AddSeconds(prevcases * timeToPickACaseInSecs)
                    Dim estimatedSelectorArrivalTime As DateTime
                    If task.STARTTIME = DateTime.MinValue Then
                        WriteLog("Pick Task start time : " & task.STARTTIME.ToString(), oLogger)
                        estimatedSelectorArrivalTime = DateTime.Now
                        WriteLog("Pick Task start time updated to: " & estimatedSelectorArrivalTime.ToString(), oLogger)
                    Else
                        estimatedSelectorArrivalTime = task.STARTTIME.Date
                        estimatedSelectorArrivalTime.AddSeconds(prevcases * timeToPickACaseInSecs)
                        WriteLog(String.Format("estimatedSelectorArrivalTime={0}.AddSeconds({1} * {2})", task.STARTTIME.ToString(), prevcases.ToString(), timeToPickACaseInSecs.ToString()), oLogger)
                    End If

                    WriteLog("Calculated estimated time selector will arrive at pick location: " & estimatedSelectorArrivalTime, oLogger)

                    Dim priority As PrioritizeReplenishments = New PrioritizeReplenishments()
                    Dim priorityImmediate As String = priority.GetTaskPriorityImmediate()
                    ''RWMS-2604 Commented
                    'Dim sql As String = String.Format("select Top(1) TASK from Tasks inner join Replenishment on Tasks.REPLENISHMENT = REPLENISHMENT.REPLID  where Tasks.TASKTYPE  = 'NEGTREPL' AND Tasks.TOLOCATION = '{0}' AND Tasks.PRIORITY = '{1}' order by REPLENISHMENT.DUEDATETIME desc", pickloc.Location, priorityImmediate)
                    'RWMS-2604 Commented END
                    'RWMS-2604
                    Dim sql As String = String.Format("select Top(1) TASK from Tasks inner join Replenishment on Tasks.REPLENISHMENT = REPLENISHMENT.REPLID  where Tasks.TASKTYPE  = 'NEGTREPL' AND Tasks.TOLOCATION = '{0}' AND Tasks.PRIORITY = '{1}' AND Tasks.STATUS not in ('COMPLETE','CANCELED') order by REPLENISHMENT.DUEDATETIME desc", pickloc.Location, priorityImmediate)
                    'RWMS-2604 END
                    Dim replenTask = DataInterface.ExecuteScalar(sql)
                    WriteLog("Found replenishment, considering to update due date: taskid : " & replenTask, oLogger)
                    If Not String.IsNullOrEmpty(replenTask) Then
                        Dim replTask As Task = New Task(replenTask)

                        'Dim replDueDate As DateTime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(String.Format("select DuedateTime from replenishment where replid = '{0}'", replTask.Replenishment), DateTime.MaxValue)

                        Dim sqlDueDate As String = "select DuedateTime from replenishment where replid = '{0}'"
                        Dim replDueDate As DateTime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format(sqlDueDate, replTask.Replenishment)), DateTime.MaxValue)

                        ' If the estimated date/time is less than the current due date/time on the task (replenishment.duedatetime), update the due date/time on the replenishment task.
                        If estimatedSelectorArrivalTime < replDueDate Then
                            Replenishment.SetReplenDueDateTime(replTask.Replenishment, estimatedSelectorArrivalTime, oLogger)
                            WriteLog("Estimated date/time is less than the current due date/time on the task (replenishment.duedatetime), due date updated to : " & estimatedSelectorArrivalTime, oLogger)
                        Else
                            WriteLog("Estimated date/time is more than the current due date/time on the task (replenishment.duedatetime), not update required.", oLogger)
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            WriteLog("ReplenishmentDueDate.ReEstimateDueDateOnPickListAssign threw exception : " + ex.StackTrace, oLogger)
        End Try
    End Sub

    Private Shared Function DeriveDueDateForReplen(porderID As String, Optional ByVal oLogger As LogHandler = Nothing) As DateTime
        Dim dueDate As DateTime = DateTime.Now
        Dim orderDueDate As DateTime = DateTime.Now

        Dim sql As String = "select SCHEDULEDDATE from OUTBOUNDORHEADER where Orderid = '{0}'"
        orderDueDate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format(sql, porderID)), DateTime.MinValue)

        If Not oLogger Is Nothing Then
            WriteLog(String.Format("SQL for order Schedule Date: {0}", sql), oLogger)
            WriteLog(String.Format("order Schedule Date: {0}", orderDueDate.ToString()), oLogger)
        End If

        ' Check if shipment is there
        sql = "SELECT A.SHIPMENT, B.ORDERID, B.ORDERLINE, A.SCHEDDATE FROM SHIPMENT A (NOLOCK) INNER JOIN SHIPMENTDETAIL B (NOLOCK) ON A.SHIPMENT = B.SHIPMENT  AND B.ORDERID = '{0}'"
        Dim dt As DataTable = New DataTable()
        Made4Net.DataAccess.DataInterface.FillDataset(String.Format(sql, porderID), dt)


        If Not oLogger Is Nothing Then
            WriteLog("SQL for shipment Schedule Date:: " & sql, oLogger)
        End If


        If dt.Rows.Count > 0 Then
            Dim dueDateOnShipment As DateTime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("SCHEDDATE"), DateTime.MinValue)
            If Not oLogger Is Nothing Then
                WriteLog(String.Format("shipment Schedule Date: {0}", dueDateOnShipment.ToString()), oLogger)
            End If
            If dueDateOnShipment <> DateTime.MinValue Then
                dueDate = dueDateOnShipment
            Else
                If orderDueDate <> DateTime.MinValue Then
                    dueDate = orderDueDate
                End If
            End If
        Else
            If orderDueDate <> DateTime.MinValue Then
                dueDate = orderDueDate
            End If
        End If
        Return dueDate
    End Function

    Private Shared Function IfLaborTurnedOn(Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim sql As String = "Select count(*) from LABORTASKCALCULATION"
        Dim count As Integer = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If count > 0 Then
            Dim showlabor As Int32 = WMS.Logic.GetSysParam("ShowLaborPerformance")
            Dim showLaborOnAssigne As Int32 = WMS.Logic.GetSysParam("ShowLaborPerformanceOnAssign")
            If showlabor & showLaborOnAssigne Then
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Private Shared Sub WriteLog(ByVal msg As String, Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write(msg)
        End If
    End Sub

    Private Shared Function FindPickDetailForSKU(sku As String, pcklist As Picklist) As PicklistDetail
        Dim pickdetailcol As PicklistDetailCollection = New PicklistDetailCollection(pcklist.PicklistID)

        For i As Integer = 1 To pickdetailcol.Count
            Dim oPickListDetail As PicklistDetail = pcklist.Lines.PicklistLine(i)
            If oPickListDetail.SKU.Equals(sku) Then
                Return oPickListDetail
            End If
        Next
    End Function

    Private Shared Function FindPreviousCaseCountForSKU(pcklist As Picklist) As Integer
        Dim prevCount As Integer = 0
        Dim pickdetailcol As PicklistDetailCollection = New PicklistDetailCollection(pcklist.PicklistID)
        For i As Integer = 1 To pickdetailcol.Count
            Dim oPickListDetail As PicklistDetail = pcklist.Lines.PicklistLine(i)
            If oPickListDetail.Status = "COMPLETE" Then
                prevCount = prevCount + oPickListDetail.Quantity
            ElseIf oPickListDetail.Status <> "CANCELED" Then
                Exit For
            End If
        Next
        Return prevCount
    End Function
End Class