Imports WMS.Lib
Imports WMS.Logic
Imports Made4Net.General
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
''' <summary>
''' Class that implements prioritization policy for Replenishments
''' </summary>
''' <remarks></remarks>
Public Class PrioritizeReplenishments
    Inherits Prioritize

    'Dictionary having Key as parameter name and value as parameter value. This is kept generic to handle multiple task interleaving
        Public Overrides Function GetTaskPriority(parameters As Generic.Dictionary(Of String, Object)) As String

        Try
            Dim sql As String
            Dim numberOfRepls As Integer
            Dim dictObject As Object

            parameters.TryGetValue(REPLENISHMENTS.PICKLOC, dictObject)

            Dim zb As PickLoc = dictObject

            Try

                sql = String.Format("SELECT SUM(UNITS) AS InventoryRepls FROM REPLENISHMENT REPL " +
                                    " INNER JOIN TASKS T ON T.REPLENISHMENT=REPL.REPLID and T.TOLOCATION=REPL.TOLOCATION " +
                                    " WHERE T.PRIORITY>=300 AND REPL.STATUS='PLANNED' AND REPL.TOLOCATION='{0}'" +
                                    " GROUP BY REPL.TOLOCATION ", zb.Location)

                numberOfRepls = System.Convert.ToInt32(DataInterface.ExecuteScalar(sql))

                If Not IsNothing(LogHandler) Then
                    LogHandler.Write(String.Format("Executing {0}", sql))
                End If
            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                    Throw
                End If
            End Try

            If parameters.TryGetValue(REPLENISHMENTS.PICKLOC, dictObject) Then

                If Not IsNothing(LogHandler) Then
                    LogHandler.Write("zb.MaximumReplQty >= (zb.CurrentQty + zb.PendingQty + numberOfRepls)")
                    LogHandler.Write(String.Format("{0} >= ({1} + {2} + {3})", _
                        zb.MaximumReplQty, zb.CurrentQty, zb.PendingQty, numberOfRepls))
                End If

                Dim priorityValue As String
                If (zb.MaximumReplQty >= (zb.CurrentQty + zb.PendingQty + numberOfRepls)) Then
                    priorityValue = GetPriorityValue(TASKPRIORITY.PRIORITY_NORMAL)
                Else
                    priorityValue = GetPriorityValue(TASKPRIORITY.PRIORITY_PENDING)
                End If
                If Not IsNothing(LogHandler) Then
                    LogHandler.Write("GetPriorityValue returns: " & priorityValue)
                End If
                Return priorityValue
            End If

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function


    Public Overrides Function GetTaskPriorityImmediate() As String

        Return GetPriorityValue(TASKPRIORITY.PRIORITY_IMMEDIATE)

    End Function

    Public Overrides Sub PickListAssignmentPrioritization(ByVal pTaskID As String, Optional ByVal oLogger As LogHandler = Nothing)
        Try
            Dim task As Task = New Task(pTaskID)
            ' Only process Assigned Tasks
            If task.STATUS = WMS.Lib.Statuses.Task.ASSIGNED Then
                ' Only process for Partial Pick
                If task.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING Then
                    Dim picklst As Picklist = New Picklist(task.Picklist)
                    For Each pckdet As PicklistDetail In picklst.Lines
                        ' Log picklist and pick line that is being processed
                        oLogger.Write("Processing PickList: " & task.Picklist & " and PickLine: " & pckdet.PickListLine)
                        If PickLoc.Exists(pckdet.Consignee, pckdet.SKU, pckdet.FromLocation, pckdet.FromWarehousearea) Then
                            oLogger.Write("PickDetail.Fromlocation is a pick location : " & pckdet.FromLocation)
                            Dim sumQty As Integer = 0
                            'RWMS-2604 Commented
                            'Dim sql As String = String.Format("select sum(pickdetail.qty) as SUMQTY from pickdetail inner join tasks on pickdetail.PICKLIST = tasks.PICKLIST  where tasks.status = 'ASSIGNED' and pickdetail.fromlocation in ('{0}') and pickdetail.pickedqty = 0 and pickdetail.STATUS in ({1})", pckdet.FromLocation, REPLENISHMENTS.StatusOfDetailsToSearch)
                            'RWMS-2604 Commented END
                            'RWMS-2604
                            Dim sql As String = String.Format("select isnull(sum(pickdetail.qty),0) as SUMQTY from pickdetail inner join tasks on pickdetail.PICKLIST = tasks.PICKLIST  where tasks.status = 'ASSIGNED' and pickdetail.fromlocation in ('{0}') and pickdetail.pickedqty = 0 and pickdetail.STATUS in ({1})", pckdet.FromLocation, REPLENISHMENTS.StatusOfDetailsToSearch)
                            'RWMS-2604 END
                            oLogger.Write("Query to Calculate Sum of qty to be picked from this location : ")
                            oLogger.Write("SQL : " & sql)
                            sumQty = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                            oLogger.Write("Sum of qty to be picked from the location : " & sumQty)

                            Dim pckloc As PickLoc = New PickLoc(pckdet.FromLocation, pckdet.FromWarehousearea, pckdet.Consignee, pckdet.SKU)
                            Dim masterDao As MasterDao = New MasterDao()

                            oLogger.Write("Trying to load PRIORITY IMMEDIATE value from Table [SystemConfig]")
                            Dim priorityImmediateValueStr As String = masterDao.GetPriorityCodeForTask(WMS.Lib.TASKPRIORITY.PRIORITY_IMMEDIATE)
                            oLogger.Write("PRIORITY_IMMEDIATE : " & priorityImmediateValueStr)

                            If Not String.IsNullOrEmpty(priorityImmediateValueStr) Then
                                Dim replenQty As String = If(String.IsNullOrEmpty(masterDao.GetSumOfQtyInReplenWithPriority(pckloc.Location, priorityImmediateValueStr)), "0", masterDao.GetSumOfQtyInReplenWithPriority(pckloc.Location, priorityImmediateValueStr))
                                oLogger.Write("Total qty in Replens at PRIORITY_IMMEDIATE : " & replenQty)

                                If pckloc.CurrentQty + replenQty < sumQty Then
                                    oLogger.Write("'qty in location + qty in Replens at PRIORITY_IMMEDIATE < SumQty to be picked' is True, raising priority...")
                                    sql = String.Format("select Top(1) TASK, Priority from Tasks inner join Replenishment on  Tasks.Replenishment = Replenishment.Replid and Tasks.Tasktype = 'NEGTREPL' And Tasks.Status= 'AVAILABLE' And Tasks.TOLOCATION = '{0}' Where Tasks.Priority < {1}  Order by Replenishment.REPLID asc", Replenishment.GetFlowRackLocation(pckloc.Location, oLogger), priorityImmediateValueStr)
                                    Dim ds As DataTable = New DataTable()
                                    Made4Net.DataAccess.DataInterface.FillDataset(sql, ds)
                                    If ds.Rows.Count = 0 Then
                                        oLogger.Write("Could not find any existing replenishment task for the picklocation, cannot raise priority, existing while loop...")
                                    Else
                                        Dim row As DataRow = ds.Rows(0)
                                        Dim priority As Integer = Convert.ToInt32(If(row("Priority"), 0))
                                        oLogger.Write("Priority of existing replenishment task " & priority)
                                        If Not String.IsNullOrEmpty(If(row("TASK"), "")) Then
                                            Dim tsk As Task = New Task(row("TASK"))
                                            masterDao.SetPriorityOfTask(row("TASK"), priorityImmediateValueStr)
                                            oLogger.Write(String.Format("Raised Priority of Replenishment with ID:{0} and TaskID:{1} to PRIORITY_IMMEDIATE", tsk.Replenishment, tsk.TASK))

                                        Else
                                            oLogger.Write("Couldnot find any valid Task to raise the priority, Task column is NULL or Empty.")
                                        End If
                                    End If
                                    replenQty = If(masterDao.GetSumOfQtyInReplenWithPriority(pckloc.Location, priorityImmediateValueStr), 0)
                                End If
                                replenQty = If(masterDao.GetSumOfQtyInReplenWithPriority(pckloc.Location, priorityImmediateValueStr), 0)
                                If pckloc.CurrentQty + replenQty < sumQty Then
                                        oLogger.Write("Could not find a replen for location " & task.FROMLOCATION & " for pickdetail " & task.Picklist & " and PickLine: " & pckdet.PickListLine)
                                    End If
                                Else
                                    oLogger.Write("Cannot fetch Priority setting for PRIORITY_IMMEDIATE from Table [SystemConfig], cannot proceed")
                            End If
                        Else
                            oLogger.Write("PickDetail.Fromlocation is not pick location, can not proceed, moving to next pickdetail.")
                        End If
                        oLogger.Write("Processing Finished for PickList: " & task.Picklist & " and PickLine: " & pckdet.PickListLine)
                    Next
                Else
                    oLogger.Write("Task type is not partial pick, can not proceed")
                End If
            Else
                oLogger.Write("Task status is still not assigned, can not proceed")
            End If
        Catch ex As Exception
            oLogger.Write("Unhandled Exception while Picklist assingment processing, couldnot raise proirity.")
        End Try
    End Sub

End Class