Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports RWMS.Logic
Imports System.Linq
Imports System.Collections.Generic

<CLSCompliant(False)> Public Class RPK2
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            Try
                Session.Remove("pwjob")
                Session.Remove("pwtask")
                Session.Remove("MobileSourceScreen")
                Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                checkPutaway()

            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
                Return
            End Try
        End If
    End Sub

    Private Sub checkPutaway()

        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, rdtLogger) Then
            goBack()
        End If
        Dim pwtask As PutawayTask
        Dim tm As New WMS.Logic.TaskManager
        If Session("SKUMULTISEL_LOADID") IsNot Nothing Then
            Dim listPayloads As List(Of String) = Session("SKUMULTISEL_LOADID")
            pwtask = tm.getMultiPutAwayFirstAssignedTask(listPayloads, WMS.Logic.Common.GetCurrentUser)
        Else
            pwtask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, rdtLogger)
        End If
        ' Check if the containes Putaway jobs, if no then close task and go bak
        If Session("LOADIDONCONTAINER") Is Nothing Then
            If pwtask.getPutawayJob Is Nothing Then
                pwtask.Complete(WMS.Logic.LogHandler.GetRDTLogger)
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session("TaskID") = pwtask.TASK
                    MobileUtils.RedirectToTaskSummary(Session("MobileSourceScreen"))
                Else
                    goBack()
                End If
            End If
        Else
            If pwtask.getPutawayJob(Session("LOADIDONCONTAINER")) Is Nothing Then
                pwtask.Complete(WMS.Logic.LogHandler.GetRDTLogger)
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session("TaskID") = pwtask.TASK
                    MobileUtils.RedirectToTaskSummary(Session("MobileSourceScreen"))
                Else
                    goBack()
                End If

            End If
        End If

        If Not pwtask Is Nothing Then
            'if we came from RPK1 - check if we have the multiple loads collection and remove current load from it..
            Dim oLoadCollection As ArrayList = Session("RPKLoadsCollection")
            If Not oLoadCollection Is Nothing Then
                If oLoadCollection.Count > 0 Then
                    Dim strLoadid As String = pwtask.FROMLOAD
                    oLoadCollection.Remove(strLoadid)
                    Session("RPKLoadsCollection") = oLoadCollection
                End If
            End If
            Session("pwtask") = pwtask
            If Session("LOADIDONCONTAINER") Is Nothing Then
                setPutaway(pwtask.getPutawayJob)
            Else
                setPutaway(pwtask.getPutawayJob(Session("LOADIDONCONTAINER")))
            End If
        End If
    End Sub

    Public Sub setPutaway(ByVal pwjob As PutawayJob)
        Session("pwjob") = pwjob
        'Added for RWMS-1561 and RWMS-851 Start
        Try
            If pwjob.IsDestinationLocationNotAccesibleByHeType = True Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Current equipment will not reach suggested putaway location, returning payload to starting location", "Current equipment will not reach suggested putaway location, returning payload to starting location")
            End If
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
        'Added for RWMS-1561 and RWMS-851 End

        If pwjob.isContainer Then
            DO1.setVisibility("CONSIGNEE", False, Session("3PL"))
            DO1.setVisibility("SKU", False)
            DO1.setVisibility("SKUDESC", False)
            DO1.setVisibility("UOM", False)
            DO1.setVisibility("UOMUNITS", False)
            DO1.setVisibility("UNITS", False)
        Else
            DO1.setVisibility("CONSIGNEE", True, Session("3PL"))
            DO1.setVisibility("SKU", True)
            DO1.setVisibility("SKUDESC", True)
            DO1.setVisibility("UOM", True)
            DO1.setVisibility("UOMUNITS", True)
            DO1.setVisibility("UNITS", True)
            DO1.setVisibility("HANDLINGUNIT", False)
            DO1.setVisibility("HANDLINGUNITTYPE", False)

            DO1.Value("CONSIGNEE") = pwjob.Consignee
            DO1.Value("SKU") = pwjob.Sku
            DO1.Value("SKUDESC") = pwjob.skuDesc
            DO1.Value("UOM") = pwjob.UOM
            DO1.Value("UOMUNITS") = pwjob.UOMUnits
            DO1.Value("UNITS") = Math.Round(pwjob.UOMUnits, 2) 'pwjob.Units
            'Dim dd As Made4Net.WebControls.MobileDropDown
            'dd = DO1.Ctrl("HANDLINGUNITTYPE")
            'dd.AllOption = False
            'dd.TableName = "handelingunittype"
            'dd.ValueField = "container"
            'dd.TextField = "containerdesc"
            'dd.DataBind()
        End If
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Commented for RWMS-1561 and RWMS-851 Start
        'If pwjob.IsHandOff Then
        '    'DO1.setVisibility("Note", True)
        '    'DO1.Value("Note") = trans.Translate("Task Destination Location is an Hand Off Location!")
        '    'Instead - go to the container pw screen and deliver the whole container

        'Else
        '    DO1.setVisibility("Note", False)
        'End If
        'End Commented for  RWMS-1561 and RWMS-851 Start
        'Added for  RWMS-1561 and RWMS-851 Start
        If pwjob.IsDestinationLocationNotAccesibleByHeType Then
            DO1.setVisibility("Note", True)
            DO1.Value("Note") = trans.Translate("Current equipment will not reach suggested putaway location, returning payload to starting location")
        Else
            DO1.setVisibility("Note", False)
        End If
        'Added for  RWMS-1561 and RWMS-851 End

        DO1.Value("LOADID") = pwjob.LoadId
        DO1.Value("LOCATION") = pwjob.toLocation
        DO1.Value("WAREHOUSEAREA") = pwjob.toWarehousearea
        'Added for RWMS-1242/RWMS-1277 - Payload Putaway - prepopulate location
        Dim IsPrepolulateLocation As Boolean
        Dim putaway As New WMS.Logic.Putaway()
        IsPrepolulateLocation = putaway.CheckPrepolateLocationFromTask(pwjob.TaskId)
        If (IsPrepolulateLocation) Then
            DO1.Value("CONFIRM") = pwjob.toLocation
        End If
        'End for RWMS-1242/RWMS-1277 - Payload Putaway - prepopulate location
        'check if loacation where found
        CheckPWLocation(pwjob)


        'Fill the problem code drop down if operation allowed
        Dim pwtask As PutawayTask = Session("pwtask")
        Dim dd1 As Made4Net.WebControls.MobileDropDown
        dd1 = DO1.Ctrl("TaskProblemCode")
        'dd1.AllOption = False
        dd1.AllOption = True
        dd1.AllOptionText = ""
        dd1.TableName = "vTaskTypesProblemCodes"
        dd1.ValueField = "PROBLEMCODEID"
        dd1.TextField = "PROBLEMCODEDESC"
        dd1.Where = "TASKTYPE = '" & pwtask.TASKTYPE & "'"
        dd1.DataBind()
        Try
            If dd1.GetValues.Count > 0 Then
                DO1.setVisibility("TaskProblemCode", True)
            Else
                DO1.setVisibility("TaskProblemCode", False)
            End If
        Catch ex As Exception
            DO1.setVisibility("TaskProblemCode", False)
        End Try
    End Sub

    Private Sub CheckPWLocation(ByVal pwjob As PutawayJob)
        Dim ld As New WMS.Logic.Load(pwjob.LoadId)

        If pwjob.toLocation = ld.LOCATION And pwjob.toWarehousearea = ld.WAREHOUSEAREA Then
            If ld.DESTINATIONLOCATION = ld.LOCATION And ld.DESTINATIONWAREHOUSEAREA = ld.WAREHOUSEAREA Then
                sendLOCASGNPRB(pwjob, ld)
            End If
        End If

    End Sub

    Private Sub sendLOCASGNPRB(ByVal pwjob As PutawayJob, ByVal ld As WMS.Logic.Load)
        Dim MSG As String = "LOCASGNPRB"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", "2004")
        aq.Add("ACTIVITYTYPE", MSG)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", pwjob.Consignee)
        aq.Add("DOCUMENT", ld.RECEIPT)
        aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)

        aq.Add("FROMLOAD", ld.LOADID)
        Try
            aq.Add("FROMCONTAINER", ld.ContainerId)
        Catch ex As Exception

        End Try
        aq.Add("FROMLOC", pwjob.toLocation)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)

        'aq.Add("NOTES", Session("ERROROVERRIDE"))
        aq.Add("SKU", ld.SKU)
        'aq.Add("TOLOAD", Session("CreateLoadLoadId"))
        'aq.Add("TOLOC", Session("CreateLoadLocation"))
        'aq.Add("TOQTY", Session("CreateLoadUnits"))
        'aq.Add("TOSTATUS", Session("CreateLoadStatus"))
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", ld.WAREHOUSEAREA)
        aq.Add("TOWAREHOUSEAREA", pwjob.toWarehousearea)

        aq.Send(MSG)

    End Sub

    Private Sub goBack()
        Dim srcScreen As String
        Try
            srcScreen = Session("MobileSourceScreen")
            Dim pwtask As PutawayTask
            Dim tm As New WMS.Logic.TaskManager
            pwtask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
            If Not pwtask Is Nothing Then
                If Session("MobileSourceScreen") <> "RPKC1" Then
                    pwtask.ExitTask()
                End If
            End If
        Catch ex As Exception

        End Try
        If Session("PayLoadId") IsNot Nothing Then
            Session.Remove("PayLoadId")
        End If
        If srcScreen = "" Or srcScreen Is Nothing Then
            Response.Redirect(MapVirtualPath("screens/RPK.aspx"))
        Else
            Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx")) '
        End If
        'Response.Redirect(MapVirtualPath("screens/tasksummary.aspx"))
    End Sub

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim pwjob As PutawayJob = Session.Item("pwjob")
        Dim oCont As WMS.Logic.Container
        Dim taskID As String = ""
        Try
            'Added for RWMS-1561 and RWMS-851 Start
            If pwjob.IsDestinationLocationNotAccesibleByHeType = True Then
                pwjob.IsHandOff = False
            End If
            'Added for RWMS-1561 and RWMS-851 End

            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(pwjob.toLocation, DO1.Value("CONFIRM"), pwjob.toWarehousearea)

            If strConfirmationLocation.ToLower <> pwjob.toLocation.ToLower Or _
                                DO1.Value("WAREHOUSEAREA").ToLower <> pwjob.toWarehousearea.ToLower Then

                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Location entered does not match to original location, please use the ""Override"" button")
                Return
            End If


            If Not pwjob.isContainer Then
                If WMS.Logic.Container.Exists(DO1.Value("HANDLINGUNIT")) Then
                    oCont = New WMS.Logic.Container(DO1.Value("HANDLINGUNIT"), True)
                Else
                    If DO1.Value("HANDLINGUNIT") <> "" Then
                        oCont = New WMS.Logic.Container
                        oCont.ContainerId = DO1.Value("HANDLINGUNIT")
                        oCont.HandlingUnitType = DO1.Value("HANDLINGUNITTYPE")
                        oCont.Location = pwjob.toLocation
                        oCont.Warehousearea = pwjob.toWarehousearea

                        oCont.Post(WMS.Logic.Common.GetCurrentUser)
                    End If
                End If
            End If


            'Check if the location is correct , if not print an error
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PUTAWAY, LogHandler.GetRDTLogger())
            taskID = pwjob.TaskId

            Dim putAwayTask As PutawayTask = New PutawayTask(taskID)

            putAwayTask.Put(pwjob, strConfirmationLocation, "", DO1.Value("WAREHOUSEAREA"), oCont)
            DO1.Value("CONFIRM") = ""
            Session.Remove("PUTAWAYTASK")
            If Session("SKUMULTISEL_LOADID") IsNot Nothing Then

                Dim loadIdList As List(Of String) = Session("SKUMULTISEL_LOADID")
                loadIdList.Remove(pwjob.LoadId)
                If loadIdList.Count = 0 Then
                    Session.Remove("SKUMULTISEL_LOADID")
                Else
                    Session("SKUMULTISEL_LOADID") = loadIdList
                    Session("ScreenAfterTaskSummary") = "LDPWCNF"
                End If
                'Check and remove
                Session.Remove("PayLoadId")
                Session.Remove("Sequence")
            End If

            'Dim sql As String
            'sql = String.Format("Update loads set WAREHOUSEAREA='{0}' where loadid='{1}'", DO1.Value("WAREHOUSEAREA"), pwjob.LoadId)
            'Made4Net.DataAccess.DataInterface.RunSQL(sql)

            Dim err As String
            AppUtil.isBackLocMoveFront(pwjob.LoadId, strConfirmationLocation, DO1.Value("WAREHOUSEAREA"), "", err)
            If err <> "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
            End If

            'DO1.Value("CONFIRMWarehousearea") = ""
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
            Return
        End Try
        If Session("MobileSourceScreen") = "RPKC1" Then
            goBack()
        End If
        If MobileUtils.ShouldRedirectToTaskSummary() Then
            Session("TaskID") = taskID
            If Session("ScreenAfterTaskSummary") = "LDPWCNF" Then
                MobileUtils.RedirectToTaskSummary(Session("ScreenAfterTaskSummary"))
            Else
                MobileUtils.RedirectToTaskSummary(Session("MobileSourceScreen"))
            End If
        End If
        checkPutaway()

    End Sub

    Private Sub doOverride()
        Session("Mode") = "Override"
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim pwjob As PutawayJob = Session.Item("pwjob")
        Try
            'Check if the location is correct , if not print an error
            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(pwjob.toLocation, DO1.Value("CONFIRM"), pwjob.toWarehousearea)
            'Added for RWMS-1561 and RWMS-851 Start
            If pwjob.IsDestinationLocationNotAccesibleByHeType = True Then
                pwjob.IsHandOff = False
            End If
            'Added for RWMS-1561 and RWMS-851 End

            If strConfirmationLocation.ToLower <> pwjob.toLocation.ToLower Or _
                    DO1.Value("WAREHOUSEAREA").ToLower <> pwjob.toWarehousearea.ToLower Then
                Dim ld As New WMS.Logic.Load(pwjob.LoadId)

                Dim err As String
                If Not AppUtil.ChangeLocationValidation(strConfirmationLocation, DO1.Value("WAREHOUSEAREA"), ld.CONSIGNEE, ld.SKU, err) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
                    DO1.Value("CONFIRM") = ""
                    Return
                End If

                sendOverrideToAudit(pwjob)

                Dim lc As New WMS.Logic.Location(pwjob.toLocation, pwjob.toWarehousearea)
                lc.CancelPut(ld, WMS.Logic.Common.GetCurrentUser)

                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PUTAWAY, Nothing)

                'Start  RWMS-1317 System is creating a new putaway task when the user overides a payload putaway
                Dim sql As String
                sql = String.Format("update TASKS set TOLOCATION='{0}' where TASK='{1}'", strConfirmationLocation, pwjob.TaskId)
                Made4Net.DataAccess.DataInterface.RunSQL(sql)
                'End  RWMS-1317 System is creating a new putaway task when the user overides a payload putaway

                CType(tm.Task, WMS.Logic.PutawayTask).Put(pwjob, strConfirmationLocation, ld.SUBLOCATION, DO1.Value("WAREHOUSEAREA"))
                DO1.Value("CONFIRM") = ""
                'DO1.Value("CONFIRMWarehousearea") = ""

                'Dim sql As String
                'sql = String.Format("Update loads set WAREHOUSEAREA='{0}' where loadid='{1}'", DO1.Value("WAREHOUSEAREA"), pwjob.LoadId)
                'Made4Net.DataAccess.DataInterface.RunSQL(sql)

                AppUtil.isBackLocMoveFront(pwjob.LoadId, strConfirmationLocation, DO1.Value("WAREHOUSEAREA"), "", err)
                If err <> "" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
                End If

            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Location entered match to original location, please use the ""Next"" button")
                Return
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            Return
        End Try
        If Session("MobileSourceScreen") = "RPKC1" Then
            goBack()
        End If
        checkPutaway()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Note")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("UNITS")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddTextboxLine("HANDLINGUNIT")
        DO1.AddDropDown("HANDLINGUNITTYPE")
        DO1.AddTextboxLine("CONFIRM", True, "next")
        'DO1.AddTextboxLine("CONFIRMWarehousearea")
        DO1.AddDropDown("TaskProblemCode")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "override"
                doOverride()
            Case "back"
                goBack()
            Case "reportproblem"
                ReportProblem()
        End Select
    End Sub

    Private Sub ReportProblem()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If String.IsNullOrEmpty(DO1.Value("TaskProblemCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No problem code selected"))
                Exit Sub
            End If

            Dim pwjob As PutawayJob = Session.Item("pwjob")

            If Not WMS.Logic.Location.Exists(DO1.Value("CONFIRM"), pwjob.toWarehousearea) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location does not exist", "Location does not exist")
            End If

            Dim loc As New WMS.Logic.Location(DO1.Value("CONFIRM"), pwjob.toWarehousearea)
            If loc.PROBLEMFLAG Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location already marked as problematic", "Location already marked as problematic")
            End If


            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(pwjob.toLocation, DO1.Value("CONFIRM"), pwjob.toWarehousearea)
            If strConfirmationLocation.ToLower <> pwjob.toLocation.ToLower Or _
                    DO1.Value("WAREHOUSEAREA").ToLower <> pwjob.toWarehousearea.ToLower Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Location entered is not valid")
                Return
            End If
            Dim loc1 As New WMS.Logic.Location(strConfirmationLocation, pwjob.toWarehousearea)
            If loc1.PROBLEMFLAG Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location already marked as problematic", "Location already marked as problematic")
            End If

            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            Dim ld As New WMS.Logic.Load(pwjob.LoadId)

            sendProblemToAudit(pwjob)


            Dim sql As String
            sql = String.Format("update LOCATION set PROBLEMFLAGRC=(SELECT LOCATIONPROBLEMRC FROM TASKPROBLEMCODE where PROBLEMCODEID = '{0}') where LOCATION = '{1}' and WAREHOUSEAREA='{2}'", DO1.Value("TaskProblemCode"), strConfirmationLocation, pwjob.toWarehousearea)
            Made4Net.DataAccess.DataInterface.RunSQL(sql)

            sql = String.Format("Update loads set location='{1}', activitystatus='',destinationlocation='',unitsallocated=0 where loadid='{0}'", ld.LOADID, strConfirmationLocation)
            Made4Net.DataAccess.DataInterface.RunSQL(sql)

            Dim tsk As New WMS.Logic.Task(pwjob.TaskId)

            tsk.ReportProblem(DO1.Value("TaskProblemCode"), strConfirmationLocation, pwjob.toWarehousearea, WMS.Logic.Common.GetCurrentUser)

            'Dim tm As New WMS.Logic.TaskManager(UserId, WMS.Lib.TASKTYPE.PUTAWAY)
            'Dim tm As New WMS.Logic.TaskManager(pwjob.TaskId, False) 'UserId, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
            'CType(tm.Task, WMS.Logic.PutawayTask).ReportProblem(ld, DO1.Value("TaskProblemCode"), strConfirmationLocation, pwjob.toWarehousearea, UserId) 'pwjob.toLocation
            'tm.Complete()


            ld.PutAway("", "", UserId, True)

            ' adjust next tasks from location in case of multi putaway.
            If Session("SKUMULTISEL_LOADID") IsNot Nothing Then
                Dim tm As New WMS.Logic.TaskManager
                Dim loadIdList As List(Of String) = Session("SKUMULTISEL_LOADID")
                Dim pwtask As PutawayTask = tm.getMultiPutAwayFirstAssignedTask(loadIdList, WMS.Logic.Common.GetCurrentUser)
                Dim toLocation As String = pwtask.TOLOCATION
                ' Update this in the next tasks from location
                Dim nextLoadId As String = getNextLoadId(loadIdList, pwtask.FROMLOAD)
                If Not String.IsNullOrEmpty(nextLoadId) Then
                    tm.adjustMultiPutawaySingleTaskFromLocaion(nextLoadId, toLocation, WMS.Logic.Common.GetCurrentUser)
                End If
            End If

            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location problem reported"))
            DO1.Value("CONFIRM") = ""
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
            Return
        End Try
        If Session("MobileSourceScreen") = "RPKC1" Then
            goBack()
        End If
        checkPutaway()
    End Sub

    Private Sub sendProblemToAudit(ByVal pwjob As PutawayJob)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim ld As New WMS.Logic.Load(pwjob.LoadId)
        Dim SQL As String
        Dim pwtask As PutawayTask = Session("pwtask")

        Dim MSG As String = "REPORTPROBLEM"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.REPORTPROBLEM)
        aq.Add("ACTIVITYTYPE", MSG)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", ld.CONSIGNEE)
        aq.Add("DOCUMENT", ld.RECEIPT)
        aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)
        aq.Add("FROMLOAD", ld.LOADID)
        aq.Add("FROMLOC", pwtask.TOLOCATION)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)


        SQL = "SELECT PROBLEMCODEDESC FROM vTaskTypesProblemCodes WHERE TASKTYPE = '" & pwtask.TASKTYPE & "' AND PROBLEMCODEID = '" & DO1.Value("TaskProblemCode") & "'"
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        aq.Add("NOTES", SQL)

        aq.Add("SKU", ld.SKU)
        aq.Add("TOLOAD", ld.LOADID)
        aq.Add("TOLOC", DO1.Value("CONFIRM"))
        aq.Add("TOQTY", pwjob.Units)
        aq.Add("TOSTATUS", "PROBLEM")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(MSG)

    End Sub


    Private Sub sendOverrideToAudit(ByVal pwjob As PutawayJob)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim ld As New WMS.Logic.Load(pwjob.LoadId)

        Dim MSG As String = "OVERRIDE"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", "2008")
        aq.Add("ACTIVITYTYPE", MSG)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", ld.CONSIGNEE)
        aq.Add("DOCUMENT", ld.RECEIPT)
        aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)
        aq.Add("FROMLOAD", ld.LOADID)
        aq.Add("FROMLOC", pwjob.toLocation)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)
        aq.Add("NOTES", pwjob.TaskId)
        aq.Add("SKU", ld.SKU)
        aq.Add("TOLOAD", ld.LOADID)
        aq.Add("TOLOC", DO1.Value("CONFIRM"))
        aq.Add("TOQTY", pwjob.Units)
        aq.Add("TOSTATUS", ld.STATUS)
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(MSG)

    End Sub

    Private Function getNextLoadId(loadIdList As List(Of String), currLoad As String) As String
        Dim found As Boolean = False

        For Each loadid As String In loadIdList
            If found Then
                Return loadid
            End If
            If currLoad = loadid Then
                found = True
            End If
        Next
        Return ""
    End Function
End Class