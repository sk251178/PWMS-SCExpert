Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections
Imports Made4Net.Algorithms.Interfaces
Imports DataManager.ServiceModel
Imports Made4Net.Algorithms
Imports System.Collections.Generic
Imports Made4Net.Algorithms.Scoring
Imports Made4Net.Algorithms.SortingAlgorithms

#Region "Task Manager Policies"

<CLSCompliant(False)> Public Class TaskPolicies
    Implements ICollection

#Region "Variables"

    Protected _policycol As ArrayList
    Protected oLogger As LogHandler
    Protected _userWhArea As ArrayList
    Protected _sp As IShortestPathProvider

#End Region

#Region "Constructor"

    Public Sub New(ByVal pUserId As String, Optional ByVal Logger As LogHandler = Nothing)
        _policycol = New ArrayList
        _userWhArea = New ArrayList
        oLogger = Logger
        Load(pUserId)
        _sp = ShortestPath.GetInstance()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As TaskPolicy
        Get
            Return CType(_policycol(index), TaskPolicy)
        End Get
        Set(ByVal Value As TaskPolicy)
            _policycol(index) = Value
        End Set
    End Property

    Default Public ReadOnly Property Item(ByVal PolicyName As String) As TaskPolicy
        Get
            Dim policy As TaskPolicy
            For Each policy In Me
                If policy.PolicyName.ToLower = PolicyName.ToLower Then
                    Return CType(policy, TaskPolicy)
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Methods"

    Protected Sub Load(ByVal pUserId As String)
        'Dim userSkill, userRole, userMHType As String
        'Dim dtUser As New DataTable
        'Dim SQL As String
        'SQL = String.Format("Select * from USERSKILL where userid = '{0}'", pUserId)
        'DataInterface.FillDataset(SQL, dtUser)
        'If dtUser.Rows.Count = 1 Then
        '    userSkill = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtUser.Rows(0)("skill"), "%")
        '    userRole = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtUser.Rows(0)("role"), "%")
        'End If

        'Dim dtHE As New DataTable
        'SQL = String.Format("SELECT WHACTIVITY.ACTIVITYID, WHACTIVITY.USERID, HANDLINGEQUIPMENT.HANDLINGEQUIPMENT, HANDLINGEQUIPMENT.MOBILITYCODE FROM WHACTIVITY INNER JOIN HANDLINGEQUIPMENT ON WHACTIVITY.HETYPE = HANDLINGEQUIPMENT.HANDLINGEQUIPMENT where WHACTIVITY.USERID = '{0}'", pUserId)
        'DataInterface.FillDataset(SQL, dtHE)
        'If dtHE.Rows.Count >= 1 Then
        '    userMHType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtHE.Rows(0)("MOBILITYCODE"), "%")
        'End If

        'If userRole = "" Then userRole = "%"
        'If userSkill = "" Then userSkill = "%"
        'If userMHType = "" Then userMHType = "%"
        'SQL = String.Format("Select ut.* from USERTASKASSIGNMENT ut where (isnull(ut.skill,'') like '{0}' or ut.skill is null)and (isnull(ut.role,'') like '{1}' or ut.role is null) and (isnull(ut.mobility,'') like '{2}' or ut.mobility is null) order by priority asc", userSkill, userRole, userMHType)

        'If Not oLogger Is Nothing Then
        '    oLogger.Write("Trying to get the policy lookup - User Role: " & userRole & " User Skill:" & userSkill & " ...")
        '    'oLogger.Write(SQL)
        '    oLogger.writeSeperator()
        'End If

        'Dim dt As New DataTable
        'Dim dr As DataRow
        'DataInterface.FillDataset(SQL, dt)

        ''Dim dtWHArea As New DataTable
        ''SQL = String.Format("SELECT isnull(WHAREA,'') as WHAREA FROM USERWHAREA where USERID = '{0}'", pUserId)
        ''DataInterface.FillDataset(SQL, dtWHArea)
        ''If dtWHArea.Rows.Count >= 1 Then
        ''    _userWhArea.Add(dtWHArea.Rows(0)("WHAREA"))
        ''End If

        Dim dtUserPolicies As New DataTable
        Dim dr As DataRow
        Dim DTUserParams As New DataTable()
        Made4Net.DataAccess.DataInterface.FillDataset(String.Format("select * from vUserTaskAssignment where userid = '{0}'", pUserId), DTUserParams)
        If DTUserParams.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured - could not extract user params from vUserTaskAssignment....")
            End If
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("User Params (Skill,Role,Mobility Code, etc.) extracted from vUserTaskAssignment....")
            End If
        End If
        Dim oPolicyMatcher As New PolicyMatcher("USERTASKASSIGNMENT", DTUserParams)
        dtUserPolicies = oPolicyMatcher.FindMatchingPolicies()
        Dim tp As TaskPolicy

        'Added for RWMS-1606 and RWMS-1525
        If Not dtUserPolicies Is Nothing Then
            If DTUserParams.Rows.Count >= 1 Then
                For Each dr In dtUserPolicies.Rows
                    Dim policyId As String = dr("policyid") 'dr("taskpolicyname")
                    tp = New TaskPolicy(policyId)
                    Add(tp)
                Next
            End If
        End If
        'End for RWMS-1606 and RWMS-1525
        'Commented for RWMS-1525
        'For Each dr In dtUserPolicies.Rows
        ' Dim policyId As String = dr("policyid") 'dr("taskpolicyname")
        ' tp = New TaskPolicy(policyId)
        ' Add(tp)
        'Next
        'End Commented for RWMS-1606 and  RWMS-1525

    End Sub

    Public Function FindTask(ByVal pAvailableTasks As DataTable, ByVal pUserId As String, ByVal pUserMHType As String, Optional ByVal oLogger As LogHandler = Nothing, Optional IsGetTaskFromUser As Boolean = False) As Task
        Dim SQL As String
        Dim retTask As WMS.Logic.Task
        Dim ht As Hashtable
        Dim tmpAvTasks As DataRow()
        Dim tmpAL As ArrayList
        Dim priorityAL As New ArrayList
        Dim i As Int32
        Dim tpd As TaskPolicyDetail
        Dim path As Path
        Dim operatorsEqpHeight As Double = 0

        'fetch operators equipment height
        If Not String.IsNullOrEmpty(pUserId) Then
            operatorsEqpHeight = GetOperatorsEquipmentHeight(pUserId)
        End If
        'fetch operators equipment height

        'Make rules
        Dim rulesWithHeightAndUnidirection As List(Of Rules) = New List(Of Rules)()

        Dim rulesWithHeightOnly As List(Of Rules) = New List(Of Rules)()

        Dim ruleHeight As Rules = New Rules()

        ruleHeight.Parameter = Made4Net.Algorithms.Constants.Height
        ruleHeight.Data = operatorsEqpHeight
        ruleHeight.Operator = ">"

        Dim ruleUnidirection As Rules = New Rules()

        ruleUnidirection.Parameter = Made4Net.Algorithms.Constants.Equipment
        ruleUnidirection.Operator = Made4Net.Algorithms.Constants.UniDirection

        rulesWithHeightAndUnidirection.Add(ruleHeight)
        rulesWithHeightAndUnidirection.Add(ruleUnidirection)

        rulesWithHeightOnly.Add(ruleHeight)
        'Make rules

        For Each tp As TaskPolicy In Me
            ht = New Hashtable
            'Insert all Policy Detail with the same priority to a priorities HashTable
            For Each tpd In tp.PolicyDetails
                If ht.ContainsKey(tpd.Priority) Then
                    tmpAL = ht.Item(tpd.Priority)
                    tmpAL.Add(tpd)
                    ht.Item(tpd.Priority) = tmpAL
                Else
                    Dim newAl As New ArrayList
                    newAl.Add(tpd)
                    ht.Item(tpd.Priority) = newAl
                End If
                If Not priorityAL.Contains(tpd.Priority) Then
                    priorityAL.Add(tpd.Priority)
                End If
            Next

            'sort the priorities arraylist (although there is no need... should be already sorted)
            priorityAL.Sort()

            'get all task policies from each priority and get all tasks from their types
            Dim objpriorityreset As Prioritize = New PrioritizeReplenishments
            Dim ReqTaskPriority As Integer = Convert.ToInt32(objpriorityreset.GetPriorityValue(WMS.Lib.TASKPRIORITY.PRIORITY_PENDING))
            'Get the REQSTTSKPRILEVEL from WAREHOUSEPARAMS and get the priority value from SYSTEMCONFIG table
            Dim sqlReqTaskPriorityLevel As String = "SELECT PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME = 'REQSTTSKPRILEVEL'"
            Dim paramValue As String = ""
            Dim priorityForNegtRepl As String = ""
            paramValue = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlReqTaskPriorityLevel)
            If paramValue.Length() > 0 Then
                ReqTaskPriority = Convert.ToInt32(objpriorityreset.GetPriorityValue(paramValue))
            End If

            For i = 0 To priorityAL.Count - 1
                SQL = ""
                'tpdAL holds all policydetails from each priority
                Dim tpdAL As ArrayList = ht.Item(priorityAL(i))
                For Each tpd In tpdAL

                    If (IsGetTaskFromUser = False) Then
                        priorityForNegtRepl = If(tpd.TaskType = WMS.Lib.TASKTYPE.NEGTREPL, String.Format(" AND (PRIORITY >= '{0}') ", ReqTaskPriority), "")
                    End If

                    If tpd.TaskType <> "" And tpd.TaskType <> "*" And tpd.TaskType <> "%" Then
                        If tpd.TaskSubType <> "" And tpd.TaskSubType <> "*" And tpd.TaskSubType <> "%" Then
                            SQL = SQL & String.Format(" or (tasktype = '{0}' and tasksubtype='{1}'{2})", tpd.TaskType, tpd.TaskSubType, priorityForNegtRepl)
                        Else
                            SQL = SQL & String.Format(" or (tasktype = '{0}'{1})", tpd.TaskType, priorityForNegtRepl)
                        End If
                    End If
                Next
                SQL = SQL.TrimStart(" or".ToCharArray)
                SQL = String.Format("({0})", SQL)
                'Get the MHCType filter by user mhtype
                Dim mhfilter As String = GetMHTypeFilters(pUserMHType)
                If mhfilter <> "" Then
                    SQL = SQL & " and ( " & mhfilter & " )"
                End If

                ' Check if MHType is allowing aisle share --->>> Changed by udi - this code took almost 90-100 seconds......
                Dim TakedAisleSQL As String
                If MHTypeCanShareAisle(pUserMHType) Then
                    ' if yes then check if it will not distutb other mhtypes
                    TakedAisleSQL = "SELECT LOCATION,WAREHOUSEAREA FROM LOCATION WHERE AISLE IN (SELECT DISTINCT L.AISLE FROM TASKS T INNER JOIN WHACTIVITY W ON T.USERID=W.USERID INNER JOIN HANDLINGEQUIPMENT H ON HETYPE=H.HANDLINGEQUIPMENT INNER JOIN LOCATION L ON T.FROMLOCATION=L.LOCATION and T.FROMWAREHOUSEAREA=L.WAREHOUSEAREA WHERE L.AISLE IS NOT NULL AND H.ALLOWSHAREAISLE=0 AND T.STATUS='ASSIGNED' AND T.USERID<>'" & pUserId & "')"
                Else
                    ' if no then get only location from free aisle
                    TakedAisleSQL = "SELECT LOCATION,WAREHOUSEAREA FROM LOCATION WHERE AISLE IN (SELECT DISTINCT AISLE FROM WHACTIVITY WHA INNER JOIN LOCATION L ON WHA.LOCATION=L.LOCATION and WHA.WAREHOUSEAREA=L.WAREHOUSEAREA WHERE L.AISLE IS NOT NULL AND WHA.USERID <> '" & pUserId & "')"
                End If
                If Not oLogger Is Nothing Then
                    oLogger.Write("Location's to filter by sharing aisle : " & TakedAisleSQL)
                    oLogger.writeSeperator()
                End If
                Dim badlocation As DataTable = New DataTable
                DataInterface.FillDataset(TakedAisleSQL, badlocation)

                If Not oLogger Is Nothing Then
                    oLogger.writeSeperator()
                    oLogger.Write("Location count: " & pAvailableTasks.Rows.Count)
                End If

                If SQL = "" Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Trying to get any task type by Policy: " & tp.PolicyName)
                        oLogger.writeSeperator()
                    End If
                    tmpAvTasks = pAvailableTasks.Select()
                Else

                    If Not oLogger Is Nothing Then
                        oLogger.Write("IsGetTaskRequestFromUser=" & IsGetTaskFromUser.ToString() & " ; Priority Filter: " & priorityForNegtRepl)
                        oLogger.writeSeperator()
                    End If
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Trying to get task type: " & SQL & " by Policy: " & tp.PolicyName)
                        oLogger.writeSeperator()
                    End If
                    tmpAvTasks = pAvailableTasks.Select(SQL)
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Total Available tasks after filtering the view:  " & tmpAvTasks.Length.ToString)
                        oLogger.writeSeperator()
                    End If
                End If

                ' Calculate distance between points
                ' Get Current location Coordinates
                Dim CurrentLoc As DataTable = New DataTable

                DataInterface.FillDataset("SELECT ISNULL(L.LOCATION, 'EMPTY') AS LOCATION,  ISNULL(L.WAREHOUSEAREA, 'EMPTY') as WAREHOUSEAREA, ISNULL(XCOORDINATE,0) AS X,ISNULL(YCOORDINATE,0) AS Y, ISNULL(ZCOORDINATE,0) AS Z, ISNULL(W.ACTIVITY, 'EMPTY') as ACTIVITY FROM WHACTIVITY W LEFT OUTER JOIN LOCATION L ON W.LOCATION=L.LOCATION and W.WAREHOUSEAREA=L.WAREHOUSEAREA WHERE W.USERID='" & pUserId & "'", CurrentLoc)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Current location data: " & CurrentLoc.Rows(0)("LOCATION") & " " & CurrentLoc.Rows(0)("WAREHOUSEAREA") & " " & CurrentLoc.Rows(0)("X") & " " & CurrentLoc.Rows(0)("Y") & " " & CurrentLoc.Rows(0)("Z"))
                End If
                Dim watch As Stopwatch = Stopwatch.StartNew()

                If CurrentLoc.Rows(0)("LOCATION") <> "EMPTY" Then
                    Dim dDistanceWeightageValue As Decimal
                    dDistanceWeightageValue = 0.0
                    If tp.Scoring.DoesAttributeExistsOrHasValue("Distnace") Then
                        dDistanceWeightageValue = tp.Scoring.Item("Distance")
                    End If
                    'Enable the distance calculation only when the distance has given weightage value in task policy
                    If dDistanceWeightageValue <> 0.0 Then
                        For Each rowtoupdate As DataRow In tmpAvTasks
                            'rowtoupdate("DISTANCE") = CalcDistance(CurrentLoc.Rows(0)("X"), rowtoupdate("X"), CurrentLoc.Rows(0)("Y"), rowtoupdate("Y"), CurrentLoc.Rows(0)("Z"), rowtoupdate("Z"))
                            ' Apply the actual distance
                            ' Fetch Location
                            Dim sqlLoc As [String] = "Select * from LOCATION where LOCATION = '{0}'"
                            Dim locationsFrom As New DataTable()
                            DataInterface.FillDataset([String].Format(sqlLoc, CurrentLoc.Rows(0)("LOCATION")), locationsFrom)
                            Dim from As DataRow = locationsFrom.Rows(0)
                            Dim locationsTo As New DataTable()
                            DataInterface.FillDataset([String].Format(sqlLoc, rowtoupdate("FROMLOCATION")), locationsTo)
                            Dim [to] As DataRow = locationsTo.Rows(0)
                            Try
                                ''''''''''''''''''''' RWMS-2240 : Temporary Disabling Height
                                'If rowtoupdate("TASKTYPE") = "PARPICK" Then
                                '    'Unidirection & Height
                                '    path = _sp.GetShortestPathWithContsraints(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), rowtoupdate("TASKTYPE"), False, rulesWithHeightAndUnidirection)
                                '    If path.Distance.SourceToTargetLocation > 0 Then
                                '        If oLogger IsNot Nothing Then
                                '            oLogger.Write(String.Format("Found ShortestPath for FromLocation={0} and ToLocation={1}, distance={2}", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString(), path.Distance.SourceToTargetLocation))
                                '        End If
                                '        rowtoupdate("DISTANCE") = path.Distance.SourceToTargetLocation
                                '    Else
                                '        oLogger.Write(String.Format("Could not find any valid path under the given conditions from FromLocation={0} and ToLocation={1}, using theoriginal value returned by USERAVAILABLETASKS view.", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString()))
                                '    End If
                                'Else
                                '    'Only Height
                                '    path = _sp.GetShortestPathWithContsraints(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), rowtoupdate("TASKTYPE"), False, rulesWithHeightOnly)
                                '    If path.Distance.SourceToTargetLocation > 0 Then
                                '        If oLogger IsNot Nothing Then
                                '            oLogger.Write(String.Format("Found ShortestPath for FromLocation={0} and ToLocation={1}, distance={2}", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString(), path.Distance.SourceToTargetLocation))
                                '        End If
                                '        rowtoupdate("DISTANCE") = path.Distance.SourceToTargetLocation
                                '    Else
                                '        oLogger.Write(String.Format("Could not find any valid path under the given conditions from FromLocation={0} and ToLocation={1}, using theoriginal value returned by USERAVAILABLETASKS view.", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString()))
                                '    End If
                                'End If
                                ''''''''''''''''''''' RWMS-2240 : Temporary Disabling Height

                                If rowtoupdate("TASKTYPE") = "PARPICK" Then
                                    'Unidirection
                                    Dim rulesUnidirection As List(Of Rules) = New List(Of Rules)()
                                    rulesUnidirection.Add(ruleUnidirection)
                                    path = _sp.GetShortestPathWithContsraints(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), rowtoupdate("TASKTYPE"), False, rulesUnidirection, GetShortestPathLogger(oLogger))
                                    If path.Distance.SourceToTargetLocation > 0 Then
                                        If oLogger IsNot Nothing Then
                                            oLogger.Write(String.Format("Found ShortestPath for FromLocation={0} and ToLocation={1}, distance={2}", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString(), path.Distance.SourceToTargetLocation))
                                        End If
                                        rowtoupdate("DISTANCE") = path.Distance.SourceToTargetLocation
                                    Else
                                        oLogger.Write(String.Format("Could not find any valid path under the given conditions from FromLocation={0} and ToLocation={1}, using theoriginal value returned by USERAVAILABLETASKS view.", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString()))
                                    End If
                                Else
                                    'Unonstrained
                                    path = _sp.GetShortestPath(from, [to], CurrentLoc.Rows(0)("ACTIVITY"), rowtoupdate("TASKTYPE"), False)
                                    If path.Distance.SourceToTargetLocation > 0 Then
                                        If oLogger IsNot Nothing Then
                                            oLogger.Write(String.Format("Found ShortestPath for FromLocation={0} and ToLocation={1}, distance={2}", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString(), path.Distance.SourceToTargetLocation))
                                        End If
                                        rowtoupdate("DISTANCE") = path.Distance.SourceToTargetLocation
                                    Else
                                        oLogger.Write(String.Format("Could not find any valid path under the given conditions from FromLocation={0} and ToLocation={1}, using theoriginal value returned by USERAVAILABLETASKS view.", CurrentLoc.Rows(0)("LOCATION"), rowtoupdate("FROMLOCATION").ToString()))
                                    End If
                                End If


                            Catch ex As Exception
                                watch.Stop()
                                If oLogger IsNot Nothing Then
                                    oLogger.Write("GetShortestPathWithContsraints threw exception, using the Distance value returened by USERAVAILABLETASKS view.")
                                End If
                            End Try
                            ' Apply the actual distance
                        Next
                    End If
                End If
                watch.Stop()
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Distance calculation complete, time taken {0} ", watch.Elapsed.TotalMilliseconds))
                End If

                'get all tasks in the criteria of the policy details types and score them
                Try
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Trying to Score the tasks....")
                        oLogger.writeSeperator()
                    End If
                    If Not tp.Scoring Is Nothing Then
                        tp.Scoring.Score(tmpAvTasks, oLogger)
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Scoring strategy is not set, Scoring will not be activated....")
                            oLogger.writeSeperator()
                        End If
                    End If
                Catch ex As Exception
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Exception in FinTask() scoring:" + ex.Message.ToString())
                        oLogger.writeSeperator()
                    End If
                End Try

                'Return the found task
                retTask = FindTask(tpdAL, tmpAvTasks, badlocation, pUserId)
                If Not retTask Is Nothing Then Return retTask
                oLogger.writeSeperator()
                oLogger.writeSeperator()
            Next

        Next
        Return Nothing
    End Function

    ' Distance Calculation Applied.
    Private Function CalcDistance(ByVal X1 As Integer, ByVal X2 As Integer, ByVal Y1 As Integer, ByVal Y2 As Integer, ByVal Z1 As Integer, ByVal Z2 As Integer) As Integer
        Return Math.Round(Math.Sqrt((X2 - X1) ^ 2 + (Y2 - Y1) ^ 2 + (Z2 - Z1) ^ 2))
    End Function
    ' Distance Calculation Applied.

    Private Function MHTypeCanShareAisle(ByVal pUserMHType As String) As Boolean
        Dim sql, mhtypestr As String
        Dim dt As New DataTable
        sql = String.Format("SELECT ALLOWSHAREAISLE FROM HANDLINGEQUIPMENT WHERE HANDLINGEQUIPMENT = '{0}'", pUserMHType)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return False
        Else
            If dt.Rows(0)("ALLOWSHAREAISLE") = 0 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Function GetMHTypeFilters(ByVal pUserMHType As String) As String
        Dim sql, mhtypestr As String
        Dim dt As New DataTable
        sql = String.Format("select * from hetype where access = 1 and handlingequipment = '{0}'", pUserMHType)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            mhtypestr &= "'' like locmhtype"
        Else
            For Each dr As DataRow In dt.Rows
                mhtypestr &= " or '" & dr("mhctype") & "' like locmhtype"
            Next
            mhtypestr = mhtypestr.TrimStart(" or".ToCharArray)
        End If
        Return mhtypestr
    End Function

    Private Function FindTask(ByVal pPolicyDetailAL As ArrayList, ByVal pTasks As DataRow(), ByVal pNotAccessibleLocations As DataTable, ByVal pUserId As String) As Task
        Dim dr As DataRow
        Dim dt As New DataTable
        Dim index As Int32
        Dim NewTask As Task
        Dim tasktype As String
        Dim strShiftId As String
        Dim oPolicyDetail As TaskPolicyDetail

        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Find a task - checking for old tasktype and location....")
            oLogger.writeSeperator()
        End If

        'get the previous activity of the current user requesting the task
        Dim SQL As String
        Dim prevLoc, prevWarehousearea, prevTaskType As String
        SQL = String.Format("SELECT top 1 * FROM WHACTIVITY where userid ='{0}' order by ACTIVITYTIME desc", pUserId)
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            prevLoc = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("LOCATION"), "")
            prevWarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("WAREHOUSEAREA"), "")
            prevTaskType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("ACTIVITY"), "")
            strShiftId = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("SHIFT"), "")
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("got previous location: " & prevLoc & " Warehousearea: " & prevWarehousearea & " and tasktype: " & prevTaskType)
            oLogger.writeSeperator()
        End If

        For index = 0 To pTasks.Length - 1
            dr = pTasks(index)
            NewTask = New Task(dr("task"))
            If Not oLogger Is Nothing Then
                oLogger.Write("got New Task location: " & NewTask.FROMLOCATION & " Warehousearea: " & NewTask.FROMWAREHOUSEAREA & " and new tasktype: " & NewTask.TASKTYPE)
                oLogger.writeSeperator()
            End If
            For Each tmpPD As TaskPolicyDetail In pPolicyDetailAL
                If tmpPD.TaskType.ToLower = NewTask.TASKTYPE.ToLower Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Policy Was found for the current task. Trying to validate the details to match the previous location,tasktype,etc...")
                    End If
                    oPolicyDetail = tmpPD
                    Exit For
                End If
            Next
            'Check if the task can be completed before the end of the shift
            If CanBeCompletedByEndOfShift(strShiftId, NewTask.STDTIME) Then

            End If
            'if fromlocation of task is not accessible, move on
            Dim badLoc As Int32 = 0
            badLoc = pNotAccessibleLocations.Select(String.Format("LOCATION = '{0}' and WAREHOUSEAREA = '{1}' ", NewTask.FROMLOCATION, NewTask.FROMWAREHOUSEAREA)).Length
            If Not oPolicyDetail Is Nothing And badLoc <= 0 Then
                If SameAisle(oPolicyDetail, prevLoc, prevWarehousearea, NewTask) And SameTaskType(oPolicyDetail, prevTaskType, NewTask) Then
                    Return NewTask
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Policy did not match the previous task type/aisle... will try to see if there are more available task to match...")
                    End If
                End If
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("Policy Was Not found for the current task!!! (Task Type did not match)")
                End If
            End If
        Next
        Return Nothing
    End Function

    Private Function CanBeCompletedByEndOfShift(ByVal pShiftId As String, ByVal pGoalTime As Decimal) As Boolean
        'Dim oShift As New WMS.Logic.ShiftInstance(pShiftId)
        'oShift.
        Return True
    End Function

    Private Function SameAisle(ByVal pPolicyDetail As TaskPolicyDetail, ByVal sPrevLoc As String, ByVal sPrevWarehousearea As String, ByVal newTask As Task) As Boolean
        If sPrevLoc = "" Then Return True

        Dim oOldLoc, oNewLoc As Location
        Try
            oOldLoc = New Location(sPrevLoc, sPrevWarehousearea)
        Catch ex As Exception
        End Try
        Try
            oNewLoc = New Location(newTask.FROMLOCATION, newTask.FROMWAREHOUSEAREA)
        Catch ex As Exception
        End Try
        If (oOldLoc Is Nothing Or oNewLoc Is Nothing) Then Return True

        Select Case pPolicyDetail.StayInAisle.ToUpper
            Case "SAME"
                If oOldLoc.AISLE = oNewLoc.AISLE Then
                    Return True
                Else
                    Return False
                End If
            Case "OTHER"
                If oOldLoc.AISLE <> oNewLoc.AISLE Then
                    Return True
                Else
                    Return False
                End If
            Case "ALL"
                Return True
        End Select
    End Function

    Private Function SameTaskType(ByVal pPolicyDetail As TaskPolicyDetail, ByVal sPrevTaskType As String, ByVal newTask As Task) As Boolean
        If sPrevTaskType = "" Then Return True
        If sPrevTaskType.ToUpper = "LOGIN" Then Return True

        Select Case pPolicyDetail.Interleaving.ToUpper
            Case "SAME"
                If sPrevTaskType.ToUpper = newTask.TASKTYPE.ToUpper Then
                    Return True
                Else
                    Return False
                End If
            Case "OTHER"
                If sPrevTaskType.ToUpper <> newTask.TASKTYPE.ToUpper Then
                    Return True
                Else
                    Return False
                End If
            Case "ALL"
                Return True
        End Select
    End Function

    Private Function GetOperatorsEquipmentHeight(user As [String]) As Double
        Dim sql As [String] = [String].Format("select  COALESCE(HANDLINGEQUIPMENT.HEIGHT, 0) as HEIGHT from HANDLINGEQUIPMENT inner join WHACTIVITY on HANDLINGEQUIPMENT.HANDLINGEQUIPMENT = WHACTIVITY.HETYPE where WHACTIVITY.USERID = '{0}'", user)
        Dim dt As New DataTable()
        DataInterface.FillDataset(sql, dt, False, "")
        If dt.Rows.Count > 0 Then
            Return Convert.ToDouble(dt.Rows(0)("HEIGHT"))
        End If
        Return 0
    End Function

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As TaskPolicy) As Integer
        Return _policycol.Add(value)
    End Function

    Public Sub Clear()
        _policycol.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As TaskPolicy)
        _policycol.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As TaskPolicy)
        _policycol.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _policycol.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _policycol.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _policycol.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _policycol.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _policycol.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _policycol.GetEnumerator()
    End Function

#End Region

End Class

#End Region

#Region "Task Policy"

<CLSCompliant(False)> Public Class TaskPolicy

#Region "Variables"

    Protected _policyid As String
    'Protected _priority As Int32
    'Protected _role As String
    'Protected _skill As String
    'Protected _mobility As String
    Protected _policyname As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    Protected _policydetail As TaskPolicyDetailCollection
    Protected _scoring As TaskPolicyScoring

#End Region

#Region "Properties"

    Public ReadOnly Property PolicyId() As String
        Get
            Return _policyid
        End Get
    End Property

    Public ReadOnly Property PolicyName() As String
        Get
            Return _policyname
        End Get
    End Property

    'Public ReadOnly Property Role() As String
    '    Get
    '        Return _role
    '    End Get
    'End Property

    'Public ReadOnly Property Skill() As String
    '    Get
    '        Return _skill
    '    End Get
    'End Property

    'Public ReadOnly Property Mobility() As String
    '    Get
    '        Return _mobility
    '    End Get
    'End Property

    'Public ReadOnly Property Priority() As Int32
    '    Get
    '        Return _priority
    '    End Get
    'End Property

    Public ReadOnly Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
    End Property

    Public ReadOnly Property AddUser() As String
        Get
            Return _adduser
        End Get
    End Property

    Public ReadOnly Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
    End Property

    Public ReadOnly Property EditUser() As String
        Get
            Return _edituser
        End Get
    End Property

    Public ReadOnly Property Scoring() As TaskPolicyScoring
        Get
            Return _scoring
        End Get
    End Property

    Public ReadOnly Property PolicyDetails() As TaskPolicyDetailCollection
        Get
            Return _policydetail
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal PolicyId As String)
        _policyid = PolicyId
        Dim sql As String = String.Format("Select * from TASKASSIGNPOLICYHEADER Where policyId = '{0}'", _policyid)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 1 Then
            Dim dr As DataRow = dt.Rows(0)
            Load(dr)
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load(ByVal dr As DataRow)
        _policyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("POLICYID"))
        '_priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        '_role = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROLE"))
        '_skill = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKILL"))
        '_mobility = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MOBILITY"))
        _policyname = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("POLICYNAME"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))

        _scoring = New TaskPolicyScoring(_policyid)
        _policydetail = New TaskPolicyDetailCollection(_policyid)
    End Sub

    Public Sub Delete()
        Dim sql As String = String.Format("Delete from TASKASSIGNPOLICYHEADER where policyID={0}", Made4Net.Shared.FormatField(_policyid))
        DataInterface.RunSQL(sql)

        _policydetail.DeleteAll()
        _scoring.DeleteAll()
    End Sub

#End Region

End Class

#End Region

#Region "Task Manager Scoring"

<CLSCompliant(False)> Public Class TaskPolicyScoring

#Region "Variables"
    Protected _policyid As String
    Protected _attributesscoring As Made4Net.DataAccess.Collections.GenericCollection
#End Region

#Region "Properties"
    Public ReadOnly Property PolicyId() As String
        Get
            Return _policyid
        End Get
    End Property

    Public ReadOnly Property Key(ByVal idx As Int32) As String
        Get
            Return _attributesscoring.Keys(idx)
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal idx As Int32) As Decimal
        Get
            Return _attributesscoring(idx)
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal sKey As String) As Decimal
        Get
            Return _attributesscoring(sKey)
        End Get
    End Property
    Public ReadOnly Property DoesAttributeExistsOrHasValue(ByVal sKey As String) As Boolean
        Get
            If _attributesscoring.ContainsKey(sKey) Then
                If Not Object.ReferenceEquals(_attributesscoring.Item(sKey), System.DBNull.Value) And _attributesscoring.Item(sKey) IsNot Nothing Then
                    Dim result As Decimal = 0
                    If Decimal.TryParse(_attributesscoring.Item(sKey), result) Then
                        If result <> 0 Then
                            Return True
                        End If
                    End If
                End If
            End If
            Return False
        End Get
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal sPolicyId As String)
        _policyid = sPolicyId
        Load()
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from TASKPOLICYSCORING where policyid = {0}", Made4Net.Shared.Util.FormatField(_policyid))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            _attributesscoring = New Made4Net.DataAccess.Collections.GenericCollection
            For Each oCol As DataColumn In dt.Columns
                If Not oCol.ColumnName.ToLower = "policyid" Then
                    _attributesscoring.Add(oCol.ColumnName, Convert.ToDecimal(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr(oCol.ColumnName), "0")))
                End If
            Next
        End If
        dt.Dispose()
    End Sub

    Public Sub Score(ByRef cTasksCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
        Try
            oLogger.Write("Start Scoring Tasks...")
            Dim oScoreSetter As ScoreSetter = _
                New ScoreSetter.ScoreSetterBuilder() _
                .Sorter(New DataTableSort()) _
                .Logger(oLogger) _
                .Build()

            oScoreSetter.Score(cTasksCollection, _attributesscoring)
            Dim first As Boolean = True
            oLogger.Write("Scoring Results:")
            oLogger.writeSeperator("-", 80)
            For Each LoadRow As DataRow In cTasksCollection

                'RWMS-2604 - get the replenishment duedatetime

                Dim logDate As String = Convert.ToString(LoadRow("DUEDATE"))
                Dim logDateHeader As String = "CREATIONDATE"
                Dim ptasktype As String = Convert.ToString(LoadRow("TASKTYPE"))
                If ptasktype = WMS.Lib.TASKTYPE.NEGTREPL Then
                    logDateHeader = "DUEDATE"
                End If

                Dim DataRowStr, CaptionRowStr As String
                If first Then
                    oLogger.Write("TASK".PadRight(23) & "|" & "PRIORITY".PadRight(10) & "|" & logDateHeader.PadRight(20) & "|" & "DISTANCE".PadRight(11) & "|" & "SCORE".PadRight(10))
                    oLogger.writeSeperator("-", 80)
                    oLogger.Write(Convert.ToString(LoadRow("TASK")).PadRight(23) & "|" & Convert.ToString(LoadRow("PRIORITY")).PadRight(10) & "|" & logDate.PadRight(20) & "|" & Convert.ToString(LoadRow("DISTANCE")).PadRight(11) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    first = False
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write(Convert.ToString(LoadRow("TASK")).PadRight(23) & "|" & Convert.ToString(LoadRow("PRIORITY")).PadRight(10) & "|" & logDate.PadRight(20) & "|" & Convert.ToString(LoadRow("DISTANCE")).PadRight(11) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    End If
                End If
            Next
            oLogger.writeSeperator("=", 80)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.WriteException(ex)
            End If
        End Try
    End Sub

    Public Sub DeleteAll()
        Dim sql As String = String.Format("Delete from TASKPOLICYSCORING where policyid={0}", Made4Net.Shared.FormatField(_policyid))
        DataInterface.RunSQL(sql)
        If Not _attributesscoring Is Nothing Then
            _attributesscoring.Clear()
        End If
    End Sub

#Region "Old Scoring"

    'Public Sub Score(ByRef cTasksCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
    '    If cTasksCollection.Length = 0 Then Return
    '    ClearScore(cTasksCollection)
    '    For idx As Int32 = 0 To _attributesscoring.Count - 1
    '        If _attributesscoring(idx) Is DBNull.Value Or _attributesscoring(idx) Is Nothing Or _attributesscoring(idx) = 0 Then
    '            'Do Nothing
    '        Else
    '            If _attributesscoring(idx) < 0 Then
    '                Sort(cTasksCollection, _attributesscoring.Keys(idx), SortOrder.Descending)
    '            Else
    '                Sort(cTasksCollection, _attributesscoring.Keys(idx), SortOrder.Ascending)
    '            End If
    '            setScore(cTasksCollection, _attributesscoring.Keys(idx), Math.Abs(_attributesscoring(idx)))
    '        End If
    '    Next
    '    Sort(cTasksCollection, "Score", SortOrder.Descending)
    '    Try
    '        Dim first As Boolean = True
    '        oLogger.writeSeperator("=", 80)
    '        For Each LoadRow As DataRow In cTasksCollection
    '            Dim DataRowStr, CaptionRowStr As String
    '            If first Then
    '                oLogger.Write("TASK".PadRight(23) & "|" & "PRIORITY".PadRight(10) & "|" & "CREATIONDATE".PadRight(10) & "|" & "DISTANCE".PadRight(11) & "|" & "SCORE".PadRight(10))
    '                oLogger.writeSeperator("-", 80)
    '                oLogger.Write(Convert.ToString(LoadRow("TASK")).PadRight(22) & "|" & Convert.ToString(LoadRow("PRIORITY")).PadRight(10) & "|" & Convert.ToString(LoadRow("CREATIONDATE")).PadRight(10) & "|" & Convert.ToString(LoadRow("DISTANCE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
    '                first = False
    '            Else
    '                If Not oLogger Is Nothing Then
    '                    oLogger.Write(Convert.ToString(LoadRow("TASK")).PadRight(22) & "|" & Convert.ToString(LoadRow("PRIORITY")).PadRight(10) & "|" & Convert.ToString(LoadRow("CREATIONDATE")).PadRight(10) & "|" & Convert.ToString(LoadRow("DISTANCE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
    '                End If
    '            End If
    '        Next
    '        oLogger.writeSeperator("=", 80)
    '    Catch ex As Exception
    '        If Not oLogger Is Nothing Then
    '            oLogger.WriteException(ex)
    '        End If
    '    End Try
    'End Sub

    'Protected Sub setScore(ByRef cTasksCollection As DataRow(), ByVal sFieldName As String, ByVal dAttributeScore As Decimal)
    '    Dim iNumValues As Int32
    '    Dim iValueIdx As Int32 = 0
    '    Dim oVal As Object = cTasksCollection(0)(sFieldName)
    '    iNumValues = getNumValues(cTasksCollection, sFieldName)
    '    For idx As Int32 = 0 To cTasksCollection.Length - 1
    '        If (oVal Is Nothing Or oVal Is DBNull.Value) And (cTasksCollection(idx)(sFieldName) Is Nothing Or cTasksCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            ' Do Nothing - Same Values
    '        ElseIf (cTasksCollection(idx)(sFieldName) Is Nothing Or cTasksCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            oVal = cTasksCollection(idx)(sFieldName)
    '            iValueIdx = iValueIdx + 1
    '        ElseIf oVal <> cTasksCollection(idx)(sFieldName) Then
    '            oVal = cTasksCollection(idx)(sFieldName)
    '            iValueIdx = iValueIdx + 1
    '        End If
    '        cTasksCollection(idx)("score") = cTasksCollection(idx)("score") + (dAttributeScore - (iValueIdx * dAttributeScore / iNumValues))
    '    Next
    'End Sub

    'Protected Function getNumValues(ByRef cTasksCollection As DataRow(), ByVal sFieldName As String) As Integer
    '    Dim iNumValues As Int32 = 1
    '    Dim val As Object = cTasksCollection(0)(sFieldName)
    '    For idx As Int32 = 0 To cTasksCollection.Length - 1
    '        If (val Is Nothing Or val Is DBNull.Value) And (cTasksCollection(idx)(sFieldName) Is Nothing Or cTasksCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            ' Do Nothing - Same Values
    '        ElseIf (cTasksCollection(idx)(sFieldName) Is Nothing Or cTasksCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            val = cTasksCollection(idx)(sFieldName)
    '            iNumValues = iNumValues + 1
    '        ElseIf cTasksCollection(idx)(sFieldName) <> val Then
    '            val = cTasksCollection(idx)(sFieldName)
    '            iNumValues = iNumValues + 1
    '        End If
    '    Next
    '    Return iNumValues
    'End Function

    'Protected Sub ClearScore(ByRef cTasksCollection As DataRow())
    '    For Each oTaskRecord As DataRow In cTasksCollection
    '        oTaskRecord("Score") = 0
    '    Next
    'End Sub

    'Protected Sub Sort(ByRef cTasksCollection As DataRow(), ByVal sFieldName As String, ByVal eSortOrder As SortOrder)
    '    Dim bSorted As Boolean = False
    '    For idx As Int32 = 1 To cTasksCollection.Length - 1
    '        bSorted = True
    '        For jdx As Int32 = cTasksCollection.Length - 1 To idx Step -1
    '            Select Case eSortOrder
    '                Case SortOrder.Ascending
    '                    If (Not cTasksCollection(jdx).IsNull(sFieldName)) Then
    '                        If cTasksCollection(jdx - 1).IsNull(sFieldName) Then
    '                            Swap(cTasksCollection(jdx), cTasksCollection(jdx - 1))
    '                            bSorted = False
    '                        ElseIf (cTasksCollection(jdx)(sFieldName) < cTasksCollection(jdx - 1)(sFieldName)) Then
    '                            Swap(cTasksCollection(jdx), cTasksCollection(jdx - 1))
    '                            bSorted = False
    '                        End If
    '                    End If
    '                Case SortOrder.Descending
    '                    If (Not cTasksCollection(jdx).IsNull(sFieldName)) Then
    '                        If cTasksCollection(jdx - 1).IsNull(sFieldName) Then
    '                            Swap(cTasksCollection(jdx), cTasksCollection(jdx - 1))
    '                            bSorted = False
    '                        ElseIf (cTasksCollection(jdx)(sFieldName) > cTasksCollection(jdx - 1)(sFieldName)) Then
    '                            Swap(cTasksCollection(jdx), cTasksCollection(jdx - 1))
    '                            bSorted = False
    '                        End If
    '                    End If
    '            End Select
    '        Next
    '        If bSorted Then Exit For
    '    Next
    'End Sub

    'Protected Enum SortOrder
    '    Ascending
    '    Descending
    'End Enum

    'Protected Sub Swap(ByRef oDataRow1 As DataRow, ByRef oDataRow2 As DataRow)
    '    Dim tempDataRow As DataRow = oDataRow1
    '    oDataRow1 = oDataRow2
    '    oDataRow2 = tempDataRow
    'End Sub

#End Region

#End Region

End Class

#End Region

#Region "Task Policy Detail"

<CLSCompliant(False)> Public Class TaskPolicyDetail

#Region "Variables"

    Protected _policyid As String
    Protected _priority As Int32
    Protected _tasktype As String
    Protected _stayinaisle As String
    Protected _interleaving As String
    Protected _tasksubtype As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"

    Public ReadOnly Property PolicyId() As String
        Get
            Return _policyid
        End Get
    End Property

    Public ReadOnly Property Priority() As Int32
        Get
            Return _priority
        End Get
    End Property

    Public ReadOnly Property TaskType() As String
        Get
            Return _tasktype
        End Get
    End Property

    Public ReadOnly Property TaskSubType() As String
        Get
            Return _tasksubtype
        End Get
    End Property

    Public ReadOnly Property StayInAisle() As String
        Get
            Return _stayinaisle
        End Get
    End Property

    Public ReadOnly Property Interleaving() As String
        Get
            Return _interleaving
        End Get
    End Property

    Public ReadOnly Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
    End Property

    Public ReadOnly Property AddUser() As String
        Get
            Return _adduser
        End Get
    End Property

    Public ReadOnly Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
    End Property

    Public ReadOnly Property EditUser() As String
        Get
            Return _edituser
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal dr As DataRow)
        _policyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("POLICYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        _tasktype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TASKTYPE"), "")).Replace("*", "%")
        _tasksubtype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TASKSUBTYPE"), "")).Replace("*", "%")
        _stayinaisle = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STAYINAISLE"))
        _interleaving = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INTERLEAVING"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

#End Region

End Class

#End Region

#Region "Task Policy Detail Collection"

<CLSCompliant(False)> Public Class TaskPolicyDetailCollection
    Implements ICollection

#Region "Variables"

    Protected _policylines As ArrayList
    Protected _policyid As String

#End Region

#Region "Constructor"

    Public Sub New(ByVal TaskPolicyId As String)
        _policylines = New ArrayList
        _policyid = TaskPolicyId
        Load()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As TaskPolicyDetail
        Get
            Return CType(_policylines(index), TaskPolicyDetail)
        End Get
        Set(ByVal Value As TaskPolicyDetail)
            _policylines(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from TASKASSIGNPOLICYDETAIL Where policyid='{0}' order by priority asc", _policyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim tpDetail As TaskPolicyDetail
        For Each dr In dt.Rows
            tpDetail = New TaskPolicyDetail(dr)
            Add(tpDetail)
        Next

    End Sub

    Public Sub DeleteAll()
        Dim sql As String = String.Format("Delete from TASKASSIGNPOLICYDETAIL where policyid={0}", Made4Net.Shared.FormatField(_policyid))
        DataInterface.RunSQL(sql)
        Me.Clear()
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As TaskPolicyDetail) As Integer
        Return _policylines.Add(value)
    End Function

    Public Sub Clear()
        _policylines.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As TaskPolicyDetail)
        _policylines.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As TaskPolicyDetail)
        _policylines.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _policylines.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _policylines.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _policylines.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _policylines.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _policylines.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _policylines.GetEnumerator()
    End Function

#End Region

End Class

#End Region

#Region "Work Region"

<CLSCompliant(False)> Public Class WorkRegion

#Region "Variables"

    Protected _workregionid As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _workregiontype As String = String.Empty
    Protected _workregiondesc As String = String.Empty
    Protected _workregionboundarytype As String = String.Empty
    Protected _boundaryfrom As String = String.Empty
    Protected _boundaryto As String = String.Empty
    Protected _maxresources As Int32
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty

#End Region

#Region "Properties"

    Public Property WorkRegionId() As String
        Get
            Return _workregionid
        End Get
        Set(ByVal Value As String)
            _workregionid = Value
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

    Public Property WorkRegionType() As String
        Get
            Return _workregiontype
        End Get
        Set(ByVal Value As String)
            _workregiontype = Value
        End Set
    End Property

    Public Property WorkRegionDescription() As String
        Get
            Return _workregiondesc
        End Get
        Set(ByVal Value As String)
            _workregiondesc = Value
        End Set
    End Property

    Public Property WorkRegionBoundaryType() As String
        Get
            Return _workregionboundarytype
        End Get
        Set(ByVal Value As String)
            _workregionboundarytype = Value
        End Set
    End Property

    Public Property BoundaryFrom() As String
        Get
            Return _boundaryfrom
        End Get
        Set(ByVal Value As String)
            _boundaryfrom = Value
        End Set
    End Property

    Public Property BoundaryTo() As String
        Get
            Return _boundaryto
        End Get
        Set(ByVal Value As String)
            _boundaryto = Value
        End Set
    End Property

    Public Property MaxResources() As Int32
        Get
            Return _maxresources
        End Get
        Set(ByVal Value As Int32)
            _maxresources = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal pWorkRegionId As String, ByVal pWarehouseArea As String)
        _workregionid = pWorkRegionId
        _warehousearea = pWarehouseArea
        Dim sql As String = String.Format("SELECT * FROM WORKREGION where WAREHOUSEAREA = '{0}' and WORKREGIONID = '{1}'", _warehousearea, _workregionid)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 1 Then
            Dim dr As DataRow = dt.Rows(0)
            Load(dr)
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load(ByVal dr As DataRow)
        _workregionid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WORKREGIONID"))
        _warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))
        _workregiontype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WORKREGIONTYPE"))
        _workregiondesc = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WORKREGIONDESC"))
        _workregionboundarytype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WORKREGIONBOUNDARYTYPE"))
        _boundaryfrom = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("BOUNDARYFROM"))
        _boundaryto = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("BOUNDARYTO"))
        _maxresources = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXRESOURCES"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

    Public Shared Function Exists(ByVal pWorkRegionId As String, ByVal pWarehouseArea As String) As Boolean
        Dim sql As String = String.Format("SELECT count(1) FROM WORKREGION where WAREHOUSEAREA = '{0}' and WORKREGIONID = '{1}'", pWarehouseArea, pWorkRegionId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

End Class

#End Region

#Region "MHE Work Region"

<CLSCompliant(False)> Public Class MHEWorkRegion

#Region "Variables"

    Protected _mhe As String = String.Empty
    Protected _workregionid As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _maxresources As Int32
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty

#End Region

#Region "Properties"

    Public Property MHE() As String
        Get
            Return _mhe
        End Get
        Set(ByVal Value As String)
            _mhe = Value
        End Set
    End Property

    Public Property WorkRegionId() As String
        Get
            Return _workregionid
        End Get
        Set(ByVal Value As String)
            _workregionid = Value
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

    Public Property MaxResources() As Int32
        Get
            Return _maxresources
        End Get
        Set(ByVal Value As Int32)
            _maxresources = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal pMHEId As String, ByVal pWorkRegionId As String, ByVal pWarehouseArea As String)
        _workregionid = pWorkRegionId
        _mhe = pMHEId
        _warehousearea = pWarehouseArea
        Dim sql As String = String.Format("SELECT * FROM MHEWORKREGION where MHE = '{0}' and WORKREGIONID = '{1}' and WAREHOUSEAREA = '{2}'", _mhe, _workregionid, _warehousearea)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 1 Then
            Dim dr As DataRow = dt.Rows(0)
            Load(dr)
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load(ByVal dr As DataRow)
        _workregionid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WORKREGIONID"))
        _mhe = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MHE"))
        _warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))
        _maxresources = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXRESOURCES"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

#End Region

End Class

#End Region

#Region "User Work Region"

<CLSCompliant(False)> Public Class UserWorkRegion

#Region "Variables"

    Protected _userid As String = String.Empty
    Protected _workregionid As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _priority As Int32
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " USERID = '" & _userid & "' And WORKREGIONID = '" & _workregionid & "' And WAREHOUSEAREA = '" & _warehousearea & "'"
        End Get
    End Property

    Public Property UserId() As String
        Get
            Return _userid
        End Get
        Set(ByVal Value As String)
            _userid = Value
        End Set
    End Property

    Public Property WorkRegionId() As String
        Get
            Return _workregionid
        End Get
        Set(ByVal Value As String)
            _workregionid = Value
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

    Public Property Priority() As Int32
        Get
            Return _priority
        End Get
        Set(ByVal Value As Int32)
            _priority = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pUserId As String, ByVal pWorkRegionId As String, ByVal pWarehouseArea As String)
        _workregionid = pWorkRegionId
        _userid = pUserId
        _warehousearea = pWarehouseArea
        Dim sql As String = String.Format("SELECT * FROM USERWORKREGION where USERID = '{0}' and WORKREGIONID = '{1}' and WAREHOUSEAREA = '{2}'", _userid, _workregionid, _warehousearea)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 1 Then
            Dim dr As DataRow = dt.Rows(0)
            Load(dr)
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Public Shared Function Exists(ByVal pUserId As String, ByVal pWorkRegionId As String, ByVal pWarehouseArea As String) As Boolean
        Dim sql As String = String.Format("SELECT count(1) FROM USERWORKREGION where USERID = '{0}' and WORKREGIONID = '{1}' and WAREHOUSEAREA = '{2}'", pUserId, pWorkRegionId, pWarehouseArea)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load(ByVal dr As DataRow)
        _workregionid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WORKREGIONID"))
        _userid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("USERID"))
        _warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("priority"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

    Public Sub Save(ByVal pUserId As String)
        Dim Sql As String
        If UserWorkRegion.Exists(_userid, _workregionid, _warehousearea) Then
            _editdate = DateTime.Now
            _edituser = pUserId
            Sql = String.Format("UPDATE USERWORKREGION SET PRIORITY={0}, EDITDATE={1}, EDITUSER={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        Else
            _editdate = DateTime.Now
            _edituser = pUserId
            _adddate = DateTime.Now
            _adduser = pUserId
            Sql = String.Format("INSERT INTO USERWORKREGION(USERID, WORKREGIONID, WAREHOUSEAREA, PRIORITY, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
                            Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_workregionid), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        End If
        DataInterface.RunSQL(Sql)
    End Sub

    Public Sub Delete()
        Dim Sql As String
        If UserWorkRegion.Exists(_userid, _workregionid, _warehousearea) Then
            Sql = String.Format("Delete from USERWORKREGION where {0}", WhereClause)
            DataInterface.RunSQL(Sql)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "User is not assigned to the pickregion", "User is not assigned to the pickregion")
        End If
    End Sub

    Public Sub Create(ByVal pUserID As String, ByVal pWorkRegionId As String, ByVal pWarehouseArea As String, ByVal pPriority As Int16, ByVal pAddingUser As String)
        _userid = pUserID
        _workregionid = pWorkRegionId
        _warehousearea = pWarehouseArea
        _priority = pPriority

        validate(False)

        _editdate = DateTime.Now
        _edituser = pAddingUser
        _adddate = DateTime.Now
        _adduser = pAddingUser
        Dim sql As String = String.Format("INSERT INTO USERWORKREGION(USERID, WORKREGIONID, WAREHOUSEAREA, PRIORITY, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
                        Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_workregionid), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Update(ByVal pPriority As Int16, ByVal pEditingUser As String)
        _priority = pPriority
        validate(True)
        _editdate = DateTime.Now
        _edituser = pEditingUser
        Dim sql As String = String.Format("UPDATE USERWORKREGION SET PRIORITY={0}, EDITDATE={1}, EDITUSER={2} where {3}", _
            Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(sql)
    End Sub

    Private Sub validate(ByVal pEdit As Boolean)
        If pEdit Then
            If Not Exists(_userid, _workregionid, _warehousearea) Then
                Throw New M4NException(New Exception, "Can not edit user work region. It does not exist.", "Can not edit user work region. It does not exist.")
            End If
        Else
            If Exists(_userid, _workregionid, _warehousearea) Then
                Throw New M4NException(New Exception, "Can not add user work region. It already exists.", "Can not add user work region. It already exists.")
            End If
        End If

        If Not WorkRegion.Exists(_workregionid, _warehousearea) Then
            Throw New M4NException(New Exception, "Work region does not exist.", "Work region does not exist.")
        End If
        If Not Warehouse.ValidateWareHouseArea(_warehousearea) Then
            Throw New M4NException(New Exception, "Warehouse Area does not exist.", "Warehouse Area does not exist.")
        End If

        If _priority <= 0 Then
            _priority = 1
        End If
    End Sub

#End Region

End Class

#End Region