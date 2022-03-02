Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class PutawayTask
    Inherits Task

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function getPutawayJob(Optional ByVal pLoadId As String = Nothing) As PutawayJob
        Dim isLocationAssignmentUnableToFindDestinationLocation = False

        Dim pwjob As New PutawayJob
        pwjob.IsHandOff = False
        'RWMS-1972 -> Added for RWMS-1561 and RWMS-851 Start
        pwjob.IsDestinationLocationNotAccesibleByHeType = False
        'RWMS-1972 -> Added for RWMS-1561 and RWMS-851 End

        If _task_type = WMS.Lib.TASKTYPE.CONTPUTAWAY Then
            pwjob.isContainer = True
            pwjob.LoadId = _fromcontainer
            pwjob.ContainerId = _fromcontainer
            pwjob.toLocation = _tolocation
            pwjob.toWarehousearea = _towarehousearea
            Dim oCont As New WMS.Logic.Container(_fromcontainer, True)
            pwjob.HandlingUnitType = oCont.HandlingUnitType
        ElseIf _task_type = WMS.Lib.TASKTYPE.LOADPUTAWAY Then
            Dim ld As New Load(_toload)
            pwjob.isContainer = False
            pwjob.Consignee = ld.CONSIGNEE
            pwjob.Sku = ld.SKU
            pwjob.LoadId = ld.LOADID
            pwjob.Units = ld.UNITS
            pwjob.UOM = ld.LOADUOM
            ' When Pick & Drop is preformed then
            If ld.DESTINATIONLOCATION <> "" Then
                pwjob.toLocation = ld.DESTINATIONLOCATION
            Else
                pwjob.toLocation = Me.TOLOCATION
            End If
            If ld.DESTINATIONWAREHOUSEAREA <> "" Then
                pwjob.toWarehousearea = ld.DESTINATIONWAREHOUSEAREA
            Else
                pwjob.toWarehousearea = Me.TOWAREHOUSEAREA
            End If

            Try
                Dim sku As New SKU(ld.CONSIGNEE, ld.SKU)
                pwjob.skuDesc = sku.SKUDESC
                pwjob.UOMUnits = sku.ConvertUnitsToUom(ld.LOADUOM, ld.UNITS)
            Catch ex As Exception

            End Try
            Dim Sql As String = String.Format("select isnull(hutype,'') as handlingunittype  from invload left outer join container on container.CONTAINER = isnull(INVLOAD.HANDLINGUNIT,'') where loadid = '{0}'", ld.LOADID)  'String.Format("select handlingunittype from invload where loadid = '{0}'", ld.LOADID)
            pwjob.HandlingUnitType = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTLOADPUTAWAY Then
            Dim cnt As New Container(_fromcontainer, True)
            Dim ld As Load
            If pLoadId Is Nothing Then
                Try
                    ld = cnt.Loads(0)
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                ld = New Load(pLoadId)
            End If
            pwjob.isContainer = False
            pwjob.Consignee = ld.CONSIGNEE
            pwjob.Sku = ld.SKU
            pwjob.LoadId = ld.LOADID
            pwjob.Units = ld.UNITS
            pwjob.UOM = ld.LOADUOM
            ' When Pick & Drop is preformed then
            If ld.DESTINATIONLOCATION <> "" Then
                pwjob.toLocation = ld.DESTINATIONLOCATION
            Else
                pwjob.toLocation = Me.TOLOCATION
            End If
            If ld.DESTINATIONWAREHOUSEAREA <> "" Then
                pwjob.toWarehousearea = ld.DESTINATIONWAREHOUSEAREA
            Else
                pwjob.toWarehousearea = Me.TOWAREHOUSEAREA
            End If

            Try
                Dim sku As New SKU(ld.CONSIGNEE, ld.SKU)
                pwjob.skuDesc = sku.SKUDESC
                pwjob.UOMUnits = sku.ConvertUnitsToUom(ld.LOADUOM, ld.UNITS)
            Catch ex As Exception
            End Try
            pwjob.HandlingUnitType = cnt.HandlingUnitType
            pwjob.ContainerId = cnt.ContainerId
        End If
        'Now check if the user has the access to the destination location of the replenishment job
        Dim origDestLocation As String = pwjob.toLocation
        Dim origDestWarehousearea As String = pwjob.toWarehousearea

        ' RWMS-1972 -> RWMS-1823
        ' The fix done for 'RWMS-1561 and RWMS-851'  causes issues, when the FromLocation = ToLocation as returned by RequestDestinationForLoad from LocationAssignment Service,
        ' RequestDestinationForLoad will set the ToLocation same as FromLocation if it was not able to find any destintion location for th load in question, but that doesnot means
        ' that the location is innaccessible, the code for 'RWMS-1561 and RWMS-851' below causes this issue.
        ' Introduced a special flag which checks if the FromLocation = ToLocation before calling GetFinalDestinationLocation
        If _fromlocation = pwjob.toLocation Then
            isLocationAssignmentUnableToFindDestinationLocation = True
        End If
        ' RWMS-1972 -> RWMS-1823

        TaskManager.GetFinalDestinationLocation(USERID, _fromlocation, pwjob.toLocation, _
         _fromwarehousearea, pwjob.toWarehousearea, pwjob.HandlingUnitType)
        'RWMS-1972 -> Added for RWMS-1561 and RWMS-851 Start  ' RWMS-1823 Modifications see above comments.
        If _fromlocation = pwjob.toLocation And Not isLocationAssignmentUnableToFindDestinationLocation Then
            pwjob.IsDestinationLocationNotAccesibleByHeType = True
        End If
        'RWMS-1972 -> Added for RWMS-1561 and RWMS-851 End

        If origDestLocation <> pwjob.toLocation Or origDestWarehousearea <> pwjob.toWarehousearea Then
            pwjob.IsHandOff = True
        End If

        pwjob.TaskId = _task
        ' And set the task start time
        MyBase.SetStartTime()
        Return pwjob
    End Function

    Public Sub Put(ByVal pwjob As PutawayJob, ByVal toLocation As String, ByVal pSubLocation As String, ByVal toWarehousearea As String, Optional ByVal oCont As Container = Nothing)
        Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(pwjob.toLocation, toLocation, toWarehousearea)
        Dim oPW As New WMS.Logic.Putaway()

        If _task_type = WMS.Lib.TASKTYPE.CONTPUTAWAY Then
            Dim cntr As New Container(_fromcontainer, True)
            cntr.Put(strConfirmationLocation, toWarehousearea, USERID, pwjob.IsHandOff)
            _executionlocation = strConfirmationLocation
            _executionwarehousearea = toWarehousearea

            Me.Complete(Nothing)
            If (pwjob.toLocation.ToLower <> cntr.DestinationLocation.ToLower Or _
                pwjob.toWarehousearea.ToLower <> cntr.DestinationWarehousearea.ToLower) And _
                pwjob.IsHandOff Then
                'This is an handoff situation - need to create a new task from the handoff to the original destination
                _task = ""
                _fromlocation = strConfirmationLocation 'pwjob.toLocation
                _executionlocation = ""
                _fromwarehousearea = toWarehousearea
                _executionwarehousearea = ""

                Me.Post()
            End If
        ElseIf _task_type = WMS.Lib.TASKTYPE.LOADPUTAWAY Then
            Dim ld As New Load(_fromload)
            If pwjob.toLocation <> strConfirmationLocation And pwjob.toWarehousearea <> toWarehousearea Then
                ' override message will be send
                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PutawayOverried)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("ACTIVITYTYPE", "PWTOVRD")
                aq.Add("CONSIGNEE", ld.CONSIGNEE)
                aq.Add("DOCUMENT", ld.RECEIPT)
                aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)
                aq.Add("FROMLOAD", ld.LOADID)
                aq.Add("FROMLOC", ld.LOCATION)
                aq.Add("FROMWAREHOUSEAREA", ld.WAREHOUSEAREA)
                aq.Add("FROMQTY", ld.UNITS)
                aq.Add("FROMSTATUS", ld.STATUS)
                aq.Add("NOTES", "")
                aq.Add("SKU", ld.SKU)
                aq.Add("TOLOAD", ld.LOADID)
                aq.Add("TOLOC", strConfirmationLocation)
                aq.Add("TOWAREHOUSEAREA", toWarehousearea)
                aq.Add("TOQTY", ld.UNITS)
                aq.Add("TOSTATUS", ld.STATUS)
                aq.Add("USERID", _userid)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("LASTMOVEUSER", _userid)
                aq.Add("LASTSTATUSUSER", _userid)
                aq.Add("LASTCOUNTUSER", _userid)
                aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("REASONCODE", "")
                aq.Add("ADDUSER", _userid)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", _userid)
                aq.Send("PWTOVRD")
            End If

            If pSubLocation = "" Then pSubLocation = ld.SUBLOCATION
            If toLocation.ToLower <> ld.DESTINATIONLOCATION.ToLower And pwjob.IsHandOff Then
                'ld.Put(strConfirmationLocation, pSubLocation, _userid, True)
                'oPW.Put(ld.LOADID, "", strConfirmationLocation, pSubLocation, toWarehousearea, _userid, True)
                oPW.Put(ld.LOADID, "", strConfirmationLocation, toWarehousearea, _userid, True)
            Else
                'ld.Put(strConfirmationLocation, pSubLocation, _userid, False)
                'oPW.Put(ld.LOADID, "", strConfirmationLocation, pSubLocation, toWarehousearea, _userid, False)
                ' Do take care of Multi Payload Putaway
                If MultiPayloadPutawayHelper.IsMultiPayLoadPutAwayTask(pwjob.TaskId) Then
                    ' Do For each
                    Dim dt As DataTable = MultiPayloadPutawayHelper.MultiPayloadPutAwayLoads(pwjob.TaskId)
                    For Each load As DataRow In dt.Rows
                        oPW.Put(load("FROMLOAD").ToString(), "", strConfirmationLocation, toWarehousearea, _userid, False)
                    Next
                Else
                    oPW.Put(ld.LOADID, "", strConfirmationLocation, toWarehousearea, _userid, False)
                End If

            End If
            If Not oCont Is Nothing Then
                oCont.Place(ld, _userid)
            End If
            _executionlocation = strConfirmationLocation
            _executionwarehousearea = pwjob.toWarehousearea
            ' Labor calc changes RWMS-952 -> PWMS-903
            ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User
            Dim oWHActivity As New WHActivity
            oWHActivity.USERID = _userid
            oWHActivity.SaveLastLocation()
            ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User
            ' Labor calc changes RWMS-952 -> PWMS-903

            Me.Complete(Nothing)
            'This is an handoff situation - need to create a new task from the handoff to the original destination
            If ld.DESTINATIONLOCATION <> "" And toLocation.ToLower <> ld.DESTINATIONLOCATION.ToLower And pwjob.IsHandOff Then
                _task = ""
                _fromlocation = strConfirmationLocation 'pwjob.toLocation
                _executionlocation = ""
                _fromwarehousearea = pwjob.toWarehousearea
                _executionwarehousearea = ""
                Me.Post()
            End If
        Else
            Dim ld As New Load(pwjob.LoadId)
            If pSubLocation = "" Then pSubLocation = ld.SUBLOCATION
            Dim cntr As New Container(_fromcontainer, True)
            If pwjob.toLocation.ToLower <> ld.DESTINATIONLOCATION.ToLower And pwjob.IsHandOff Then
                ' Handoff location lets put all the loads in this handoff location
                cntr.Put(toLocation, _userid, True)
            Else
                'ld.Put(strConfirmationLocation, pSubLocation, _userid, False)
                'oPW.Put(ld.LOADID, strConfirmationLocation, pSubLocation, toWarehousearea, _userid, False)
                oPW.Put(ld.LOADID, pSubLocation, strConfirmationLocation, toWarehousearea, _userid, False)
                ' Remove this load from container
                ld.RemoveFromContainer()
            End If

            If Not oCont Is Nothing Then
                oCont.Place(ld, _userid)
            End If
            Dim pCurrentUser As String = _userid
            If cntr.Loads.Count = 0 Then
                _executionlocation = strConfirmationLocation
                _executionwarehousearea = pwjob.toWarehousearea
                Me.Complete(Nothing)
            End If
            If pwjob.toLocation.ToLower <> _tolocation.ToLower And pwjob.IsHandOff Then
                _executionlocation = strConfirmationLocation
                Me.Complete(Nothing)
                'This is an handoff situation - need to create a new task from the handoff to the original destination
                _task = ""
                _fromlocation = strConfirmationLocation 'pwjob.toLocation
                _executionlocation = ""
                _fromwarehousearea = pwjob.toWarehousearea
                _executionwarehousearea = ""
                Me.Post()
            End If
        End If
    End Sub

    Public Overrides Sub Cancel()
        If _task_type = WMS.Lib.TASKTYPE.LOADPUTAWAY Then
            Dim loadObj As New WMS.Logic.Load(_fromload)
            loadObj.SetDestinationLocation("", "", Nothing)
            loadObj.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, "")
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTLOADPUTAWAY Then
            Dim contObj As New Container(_fromcontainer, True)
            For Each loadObj As Load In contObj.Loads
                loadObj.SetDestinationLocation("", "", Nothing)
                loadObj.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, "")
            Next
            contObj.SetDestinationLocation("", "", Nothing)
            contObj.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, "")
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTPUTAWAY Then
            Dim contObj As New Container(_fromcontainer, True)
            contObj.SetDestinationLocation("", "", Nothing)
            contObj.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, "")
        End If

        MyBase.Cancel()
    End Sub

    Public Shadows Sub ReportProblem(ByVal pLoad As Load, ByVal pProblemCodeId As String, ByVal pLocation As String, ByVal pWHArea As String, ByVal pUserId As String)
        MyBase.ReportProblem(pProblemCodeId, pLocation, pWHArea, pUserId)
        If CompleteTaskOnProblemCode Then
            pLoad.Put(pLocation, pWHArea, "", pUserId)
        End If
        Dim sNewLoc, sNewWHArea As String
        pLoad.PutAway(sNewLoc, sNewWHArea, pUserId, True)
    End Sub

    Public Sub ReportProblemOnRetrieval(ByVal pLoad As Load, ByVal pProblemCodeId As String, ByVal pLocation As String, ByVal pWHArea As String, ByVal pUserId As String)
        MyBase.ReportProblem(pProblemCodeId, pLocation, pWHArea, pUserId)
        pLoad.Count(0, pLoad.LOADUOM, pLocation, pWHArea, pLoad.LoadAttributes.Attributes, pUserId)
        MyBase.Cancel()
    End Sub

End Class

#Region "Putaway Job"

<CLSCompliant(False)> Public Class PutawayJob
    Public isContainer As Boolean
    Public TaskId As String
    Public LoadId As String
    Public ContainerId As String
    Public Consignee As String
    Public Sku As String
    Public skuDesc As String
    Public toLocation As String
    Public toWarehousearea As String
    Public Units As Double
    Public UOM As String
    Public UOMUnits As Double
    Public IsHandOff As Boolean
    Public HandlingUnitType As String
    'Added for RWMS-1561 and RWMS-851 Start
    Public IsDestinationLocationNotAccesibleByHeType As Boolean
    'Added for RWMS-1561 and RWMS-851 End

End Class

#End Region