Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports Wms.Logic
Imports System.Globalization
Imports RWMS.Logic

<CLSCompliant(False)> Public Class REPL2
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
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            If Not Request.QueryString.Item("sourcescreen") Is Nothing Then
                Session("REPLSRCSCREEN") = Request.QueryString.Item("sourcescreen")
            ElseIf Not String.IsNullOrEmpty(Session("IsLoadPickUp")) Then
                Session.Remove("IsLoadPickUp")
                Session("REPLSRCSCREEN") = "rpk"
            End If
            SetScreen()
        Else
            Dim dd As Made4Net.WebControls.MobileDropDown = CType(DO1.Ctrl("UOM"), Made4Net.WebControls.MobileDropDown)
            AddHandler dd.OnNextButtonClick, AddressOf uomDropDownNextButtonClick
            AddHandler dd.OnPrevButtonClick, AddressOf uomDropDownPreviousButtonClick

        End If
    End Sub

    Private Sub SetScreen()
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        Dim replJob As ReplenishmentJob
        Dim SCREENID As String = ""
        If Not IsNothing(Session("REPLSRCSCREEN")) Then
            SCREENID = Session("REPLSRCSCREEN")
        End If
        If SCREENID = "RPKC1" Then
            replJob = ReplenishmentTask.getReplenishmentJob(ReplTaskDetail.FromLoad, WMS.Logic.GetCurrentUser)
        Else
            replJob = ReplTask.getReplenishmentJob(ReplTaskDetail)
        End If
        Session("REPLJobDetail") = replJob
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If replJob.IsHandOff Then
            DO1.setVisibility("Note", True)
            DO1.Value("Note") = trans.Translate("Task Destination Location is an Hand Off Location!")
        Else
            DO1.setVisibility("Note", False)
        End If
        DO1.Value("TASKTYPE") = replJob.TaskType

        DO1.Value("LOCATION") = replJob.toLocation
        DO1.Value("WAREHOUSEAREA") = replJob.toWarehousearea

        DO1.Value("LOADID") = replJob.fromLoad
        DO1.Value("UNITS") = Math.Round(replJob.UOMUnits, 2) 'replJob.Units
        replJob.UOMUnits = Math.Round(replJob.UOMUnits, 2)
        DO1.Value("CONSIGNEE") = replJob.Consignee
        DO1.Value("SKU") = replJob.Sku
        DO1.Value("SKUDESC") = replJob.skuDesc

        SetUOM(replJob)

        Dim ld As New WMS.Logic.Load(replJob.fromLoad)
        'INIT SESSION FOR MULTI COUNT
        Session.Remove("objMultiUOMUnits")
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)

        DO1.Value("EACHUNITS") = replJob.Units 'replJob.UOMUnits
        If ReplTaskDetail.ReplType = WMS.Logic.Replenishment.ReplenishmentTypes.FullReplenishment Then
            DO1.setVisibility("EACHUNITS", False)
        Else
            DO1.setVisibility("EACHUNITS", False) ' True)
        End If
        'Added for RWMS-2598 Start
        SetTaskProblemCode(ReplTask)
        'Added for RWMS-2598 End

        'Commented for RWMS-2598 Start
        ''Fill the problem code drop down if operation allowed
        'Dim dd1 As Made4Net.WebControls.MobileDropDown
        'dd1 = DO1.Ctrl("TaskProblemCode")
        ''dd1.AllOption = False
        'dd1.AllOption = True
        'dd1.AllOptionText = ""
        'dd1.TableName = "vTaskTypesProblemCodes"
        'dd1.ValueField = "PROBLEMCODEID"
        'dd1.TextField = "PROBLEMCODEDESC"
        'dd1.Where = "TASKTYPE = '" & ReplTask.TASKTYPE & "'"
        'dd1.DataBind()
        'Try
        '    If dd1.GetValues.Count > 0 Then
        '        DO1.setVisibility("TaskProblemCode", True)
        '    Else
        '        DO1.setVisibility("TaskProblemCode", False)
        '    End If
        'Catch ex As Exception
        '    DO1.setVisibility("TaskProblemCode", False)
        'End Try
        'Commented for RWMS-2598 End
    End Sub

    'Added for RWMS-2598 Start
    Private Sub SetTaskProblemCode(ByVal ReplTask As WMS.Logic.ReplenishmentTask)
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("TaskProblemCode")
        Dim objpriority As Prioritize = New PrioritizeReplenishments
        Dim taskPriority As Integer = Convert.ToInt32(objpriority.GetTaskPriorityImmediate())
        'dd1.AllOption = False
        dd.AllOption = True
        dd.AllOptionText = ""
        dd.TableName = "vTaskTypesProblemCodes"
        dd.ValueField = "PROBLEMCODEID"
        dd.TextField = "PROBLEMCODEDESC"
        dd.Where = "TASKTYPE = '" & ReplTask.TASKTYPE & "' AND LOCATIONFLAG = 1"
        dd.DataBind()
        Try
            If dd.GetValues.Count > 0 Then
                DO1.setVisibility("TaskProblemCode", True)
            Else
                DO1.setVisibility("TaskProblemCode", False)
                HideReportProblem()
            End If
        Catch ex As Exception
            DO1.setVisibility("TaskProblemCode", False)
            HideReportProblem()

        End Try

    End Sub

    Private Sub HideReportProblem()
        Dim R As New Made4Net.WebControls.RecursiveControlFinder
        Dim btns As Made4Net.DataAccess.Collections.GenericCollection = R.SearchByType(GetType(Made4Net.WebControls.ExecutionButton), Me.Controls)
        Dim btn As Made4Net.WebControls.ExecutionButton, n As Int32
        For n = 0 To btns.Count - 1
            btn = CType(btns(n), Made4Net.WebControls.ExecutionButton)
            If btn.Text = "ReportProblem" Then
                btn.Visible = False
            End If
        Next
    End Sub
    'Added for RWMS-2598 End

    Private Sub SetUOM(ByVal replJob As ReplenishmentJob)
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
        dd.AllOption = False
        dd.TableName = "SKUUOMDESC"
        dd.ValueField = "UOM"
        dd.TextField = "DESCRIPTION"
        dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", replJob.Consignee, replJob.Sku)
        AddHandler dd.OnPrevButtonClick, AddressOf uomDropDownPreviousButtonClick
        AddHandler dd.OnNextButtonClick, AddressOf uomDropDownNextButtonClick
        dd.DataBind()
        Try
            dd.SelectedValue = replJob.UOM
        Catch ex As Exception
        End Try
    End Sub
    'If the location is the right one then replenish the location else just move
    'the load to new location and leave the Job active
    Private Sub doOverride()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If CheckLocationOverrirde() Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Match, use Next button"))
        Else
            Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

            Try
                repl.OverrideReplenish(ReplTask, repljob, DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"), False, WMS.Logic.Common.GetCurrentUser)
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
                Return
            End Try

            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
            Session.Remove("REPLJobDetail")

            If Not ReplTask Is Nothing Then
                'If ReplTask.ASSIGNMENTTYPE = WMS.Lib.TASKASSIGNTYPE.MANUAL Then
                If Not Session("REPLSRCSCREEN") Is Nothing Then
                    Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
                Else
                    Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
                End If
            Else
                If Not Session("REPLSRCSCREEN") Is Nothing Then
                    Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
                Else
                    Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
                End If
            End If
        End If

    End Sub

    'part replanishment
    '
    Private Sub doPartRepl()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'If CheckLocationOverrirde() Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Match, use Next button"))
        'Else
        Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

        Dim pOverrideLocation As String = DO1.Value("TOLOCATION")
        Dim pOverrideWarehousearea As String = DO1.Value("WAREHOUSEAREA")
        Dim pUser As String = WMS.Logic.GetCurrentUser
        Try
            'in case when load units > repl.units and  repl.units > confirm units
            'need to split oridinal from load befor, part replanishment logic

            splitLoad(repl)

            Dim ld As New WMS.Logic.Load(repl.FromLoad)
            ' Set load location
            ld.LOCATION = pOverrideLocation
            ld.WAREHOUSEAREA = pOverrideWarehousearea
            repl.ToLoad = WMS.Logic.Load.GenerateLoadId()
            Dim sql As String = String.Format("UPDATE REPLENISHMENT SET TOLOAD='{0}' WHERE  REPLID = '{1}' ", repl.ToLoad, repl.ReplId)
            DataInterface.RunSQL(sql)

            Dim units As Decimal
            units = ManageMutliUOMUnits.GetTotalEachUnits() 'GetTotal()

            ' If repl.Units >= units Then
            ld.SplitReplenish(units, ld.LOADUOM, repl.ToLoad, pUser, False)
            ' End If


            ld.Replenish(pOverrideLocation, pOverrideWarehousearea, pUser)
            'Tolocation is not saved on the db, it will only be sent to the audit msg.
            repl.ToLocation = pOverrideLocation

            ReplTask.Complete(WMS.Logic.LogHandler.GetRDTLogger())
            'RWMS-2004 changed the last parameter Made4Net.Shared.Util.FormatField(repl.FromLocation) to Made4Net.Shared.Util.FormatField(repl.ToLocation)
            sql = String.Format("Update REPLENISHMENT set status={0},editdate={1},edituser={2},tolocation={4} where {3}", _
            Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Replenishment.COMPLETE), _
            Made4Net.Shared.Util.FormatField(DateTime.Now), _
            Made4Net.Shared.Util.FormatField(WMS.Logic.GetCurrentUser), repl.WhereClause, Made4Net.Shared.Util.FormatField(repl.ToLocation))
            DataInterface.RunSQL(sql)
            sql = String.Format("Update loads set location='{1}', activitystatus='',destinationlocation='',unitsallocated=0 where loadid='{0}'", repl.FromLoad, pOverrideLocation)
            DataInterface.RunSQL(sql)

            sql = String.Format("UPDATE TASKS SET TOLOAD='{0}' WHERE  TASK = '{1}' ", repl.ToLoad, ReplTask.TASK)
            DataInterface.RunSQL(sql)

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
            Return
        End Try
        Dim loadObj As New WMS.Logic.Load(repl.FromLoad)
        ' loadObj.RequestPickUp(WMS.Logic.Common.GetCurrentUser)
        'loadObj.PutAway(pOverrideLocation, pOverrideWarehousearea, WMS.Logic.Common.GetCurrentUser, True)
        loadObj.PutAway("", "", WMS.Logic.Common.GetCurrentUser, True)

        Dim err As String

        AppUtil.isBackLocMoveFront(repl.ToLoad, repl.ToLocation, repl.ToWarehousearea, "", err)
        If err <> "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
        End If

        Session.Remove("REPLTSKTaskId")
        Session.Remove("REPLTSKTDetail")
        Session.Remove("REPLJobDetail")
        ManageMutliUOMUnits.Clear(True)
        If Not ReplTask Is Nothing Then
            'If ReplTask.ASSIGNMENTTYPE = WMS.Lib.TASKASSIGNTYPE.MANUAL Then
            If Not Session("REPLSRCSCREEN") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
            End If
        Else
            If Not Session("REPLSRCSCREEN") Is Nothing Then
            Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
        Else
            Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
        End If
        End If
        ' End If

    End Sub

    ' <summary> RWMS-2315
    ' Steps
    ' 1. Get the picklocation
    ' 2. Check is loose id ON
    ' ---->2.1. Get the original Inventory's Load ID for Pick Location if CurrentQty > 0
    ' ---->2.2. Merge Specific Qty(Units less than LoadQty) to the original PickLoc Inventory
    ' --------->2.2.1  If currentQty = 0, Split the load
    ' --------->2.2.2 Update the Replenishment & Task with newly created load id & qty.
    ' --------->2.2.3 PutAway the original Load
    ' --------->2.2.4 If currentQty > 0, Merge the required qty to Original Pickloc Inventory
    ' --------->2.2.5 Update the Replenishment with qty & toLoad to Original Pickloc Inventory Load Id.
    ' --------->2.2.6 Decrease the corresponding qty from current fromLoad
    ' --------->2.2.7 Putaway the fromLoad
    ' 3. If Loose id is off
    ' ---->3.1 Split the load with required qty to fill in the pickloc
    ' ---->3.2 Update the Replenishment & Task with newly created load id & qty.
    ' ---->3.3 Putaway the original fromLoad
    ' </summary>
    ' <remarks></remarks>
    Private Sub doNegaReplWithPutaway(ByVal units As Decimal)
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        Dim sql As String = String.Empty

        Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

        Dim picloc As Location = New Location(repl.ToLocation, repl.ToWarehousearea)
        ' Overwrite units
        repl.Units = units
        repljob.Units = units

        Session("REPLTSKTDetail") = repl
        Session("REPLJobDetail") = repljob

        Dim pOverrideLocation As String = DO1.Value("TOLOCATION")
        Dim pOverrideWarehousearea As String = DO1.Value("WAREHOUSEAREA")
        Dim pUser As String = WMS.Logic.GetCurrentUser

        Dim orginalFromLoadId As String = ""
        Try
            If picloc.LOOSEID = True Then
                sql = String.Format("select CURRENTQTY from vcurrentqty where sku = '{0}'", repljob.Sku)
                Dim currentQty As Decimal = DataInterface.ExecuteScalar(sql)
                If currentQty > 0 Then
                    sql = String.Format("update REPLENISHMENT set units={0} where {1} ", units, repl.WhereClause)
                    Made4Net.DataAccess.DataInterface.RunSQL(sql)
                    repljob.Units = units
                    repl.Units = units
                    Session("REPLTSKTDetail") = repl
                    Session("REPLJobDetail") = repljob
                    ReplTask.Replenish(repl, repljob, WMS.Logic.Common.GetCurrentUser, True)
                Else
                    sql = String.Format("update REPLENISHMENT set units={0} where {1} ", units, repl.WhereClause)
                    Made4Net.DataAccess.DataInterface.RunSQL(sql)
                    repljob.Units = units
                    repl.Units = units
                    Session("REPLTSKTDetail") = repl
                    Session("REPLJobDetail") = repljob
                    ReplTask.Replenish(repl, repljob, WMS.Logic.Common.GetCurrentUser, True)
                    ' Update
                End If
            Else
                sql = String.Format("update REPLENISHMENT set units={0} where {1} ", units, repl.WhereClause)
                Made4Net.DataAccess.DataInterface.RunSQL(sql)
                repljob.Units = units
                repl.Units = units
                Session("REPLTSKTDetail") = repl
                Session("REPLJobDetail") = repljob
                ReplTask.Replenish(repl, repljob, WMS.Logic.Common.GetCurrentUser, True)
            End If

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
            Return
        End Try

        Dim err As String
        AppUtil.isBackLocMoveFront(repl.ToLoad, repl.ToLocation, repl.ToWarehousearea, "", err)
        If err <> "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
        End If

        Session.Remove("REPLTSKTaskId")
        Session.Remove("REPLTSKTDetail")
        Session.Remove("REPLJobDetail")

        ManageMutliUOMUnits.Clear(True)

        If Not ReplTask Is Nothing Then
            If Not Session("REPLSRCSCREEN") Is Nothing Then
                If MobileUtils.ShouldRedirectToTaskSummary Then
                    Session("TaskID") = ReplTask.TASK
                    MobileUtils.RedirectToTaskSummary(Session("REPLSRCSCREEN"))
                Else
                    Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
                End If
            Else
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session("TaskID") = ReplTask.TASK
                    MobileUtils.RedirectToTaskSummary("REPL")
                Else
                    Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
                End If
            End If

        Else
            If Not Session("REPLSRCSCREEN") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
            End If
        End If
        ' End If

    End Sub

    Private Sub doNegaRepl()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'If CheckLocationOverrirde() Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Match, use Next button"))
        'Else
        Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

        Dim pOverrideLocation As String = DO1.Value("TOLOCATION")
        Dim pOverrideWarehousearea As String = DO1.Value("WAREHOUSEAREA")
        Dim pUser As String = WMS.Logic.GetCurrentUser
        Try
            'in case when load units > repl.units and  repl.units > confirm units
            'need to split oridinal from load befor, part replanishment logic

            Dim units As Decimal
            units = ManageMutliUOMUnits.GetTotalEachUnits() 'GetTotal()

            Dim originalFromLoad As New WMS.Logic.Load(repl.FromLoad)
            If originalFromLoad.UNITS < units Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load units is less than entered units"))
                Return
            End If
            Dim sql As String = String.Format("update REPLENISHMENT set units={0} where {1} ", units, repl.WhereClause)
            Made4Net.DataAccess.DataInterface.RunSQL(sql)
            repljob.Units = units
            repl.Units = units
            Session("REPLTSKTDetail") = repl
            Session("REPLJobDetail") = repljob
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
            Return
        End Try

    End Sub



    Private Sub splitLoad(ByRef repl As WMS.Logic.Replenishment)
        Dim ld As New WMS.Logic.Load(repl.FromLoad)
        Dim newFromLoad As String
        If repl.Units < ld.UNITS Then
            newFromLoad = WMS.Logic.Load.GenerateLoadId()
            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

            ld.SplitReplenish(repl.Units, ld.LOADUOM, newFromLoad, WMS.Logic.GetCurrentUser, False)

            Dim sql As String = String.Format("UPDATE REPLENISHMENT SET FROMLOAD='{0}' WHERE  REPLID = '{1}' ", newFromLoad, repl.ReplId)
            DataInterface.RunSQL(sql)
            '
            sql = String.Format("UPDATE TASKS SET FROMLOAD='{0}' WHERE  TASK = '{1}' ", newFromLoad, ReplTask.TASK)
            DataInterface.RunSQL(sql)

            repl.FromLoad = newFromLoad
        ElseIf repl.Units > ld.UNITS Then

        End If
    End Sub


    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim flag As Boolean = True
        Dim oloc As WMS.Logic.Location
        Try
            oloc = New WMS.Logic.Location(DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"))
        Catch m4nEx As Made4Net.Shared.M4NException
            'RWMS-2757 START
            HttpContext.Current.Session.Remove("objMultiUOMUnits")
            'RWMS-2757 END
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            'RWMS-2757 START
            HttpContext.Current.Session.Remove("objMultiUOMUnits")
            'RWMS-2757 END
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            Return
        End Try
        If Not CheckLocation(flag) Then
            'RWMS-458
            DO1.Value("TOLOCATION") = ""
            DO1.Value("UNITS") = ""
            'RWMS-458
            'RWMS-2757 Commented
            'If flag Then HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Location Does Not Match"))
            'RWMS-2757 Commented END
            'RWMS-2757 START
            If flag Then
                HttpContext.Current.Session.Remove("objMultiUOMUnits")
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Location Does Not Match"))
            End If
            'RWMS-2757 END

        Else
            Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")

            If Not IsNumeric(ManageMutliUOMUnits.GetTotalEachUnits()) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Units field is mandatory")
                Return
            End If
            Dim units As Decimal
            units = ManageMutliUOMUnits.GetTotalEachUnits() 'GetTotal()

            'If ReplTask.TASKTYPE = WMS.Lib.TASKTYPE.NEGTREPL Then
            '    If repljob.Units < units Then
            '        Dim sql As String
            '        '                    update LOADS set UNITSALLOCATED = UNITSALLOCATED+{0} where LOADID='{0}'
            '        'update REPLENISHMENT set UNITS = UNITS+{0} where REPLID='{1}'

            '        sql = String.Format("update REPLENISHMENT set UNITS = {0} where REPLID='{1}'", units, ReplTask.Replenishment)
            '        DataInterface.RunSQL(sql)

            '        repljob.Units = units
            '        repl.Units = units
            '        'Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            '        Session("REPLTSKTDetail") = repl
            '        Session("REPLJobDetail") = repljob
            '        doPartRepl()
            '    End If
            'End If
            Dim isNegativeWithPutway As Boolean = False

            If (repljob.Units > units AndAlso units > 0) Then
                'RWMS-2896
                Session("SkipLoadScanScreen") = 3 'RWMS-2896
                'RWMS-2896
                Session("isNegativeWithPutway") = True
                doNegaReplWithPutaway(units)
                repl = Session("REPLTSKTDetail")
                repljob = Session("REPLJobDetail")
            ElseIf (repljob.Units < units AndAlso repl.ReplType = WMS.Logic.Replenishment.ReplenishmentTypes.NegativeReplenishment) Then
                doNegaRepl()
                repl = Session("REPLTSKTDetail")
                repljob = Session("REPLJobDetail")
            ElseIf units = 0 Or repljob.Units < units Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Error units"))
                Exit Sub
            End If



            Dim ld As New WMS.Logic.Load(repljob.fromLoad)

            Try
                If Not Session("isNegativeWithPutway") Then
                    ReplTask.Replenish(repl, repljob, WMS.Logic.Common.GetCurrentUser, True)
                End If

                Dim sql As String

                sql = String.Format("Update loads set WAREHOUSEAREA='{0}' where loadid='{1}'", ld.WAREHOUSEAREA, repl.ToLoad)
                DataInterface.RunSQL(sql)

                'repljob.toLocation
                Dim err As String

                AppUtil.isBackLocMoveFront(repl.ToLoad, repl.ToLocation, repl.ToWarehousearea, "", err)
                If err <> "" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
                End If
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
                Return
            End Try
            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
            Session.Remove("REPLJobDetail")

            ManageMutliUOMUnits.Clear(True)

            If Not ReplTask Is Nothing Then
                AssignUserToNegativeLoadTask(ReplTask)
                'If ReplTask.ASSIGNMENTTYPE = WMS.Lib.TASKASSIGNTYPE.MANUAL Then
                'Redirecting to TaskSummary page should not depend on the task assignment type, if flag is to show the task summary we should display
                If Not Session("REPLSRCSCREEN") Is Nothing Then
                    If MobileUtils.ShouldRedirectToTaskSummary Then
                        Session("TaskID") = ReplTask.TASK
                        MobileUtils.RedirectToTaskSummary(Session("REPLSRCSCREEN"))
                    Else
                        Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
                    End If
                Else
                    If MobileUtils.ShouldRedirectToTaskSummary() Then
                        Session("TaskID") = ReplTask.TASK
                        MobileUtils.RedirectToTaskSummary("REPL")
                    Else
                        Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
                    End If
                End If
            Else
                If Not Session("REPLSRCSCREEN") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
            End If
        End If
        End If
    End Sub
    Private Sub AssignUserToNegativeLoadTask(ByVal pReplTask As WMS.Logic.ReplenishmentTask)
        If pReplTask.TASKTYPE = WMS.Lib.TASKTYPE.NEGTREPL Then
            Dim sSql As String = String.Format("update TASKS set USERID = {0}, ASSIGNED=1,STATUS = {3} where FROMLOAD = {1} and task <> {2} and status = {4}", _
                        Made4Net.Shared.Util.FormatField(WMS.Logic.Common.GetCurrentUser), Made4Net.Shared.Util.FormatField(pReplTask.FROMLOAD), _
                        Made4Net.Shared.Util.FormatField(pReplTask.TASK), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Task.ASSIGNED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Task.AVAILABLE))
            Made4Net.DataAccess.DataInterface.RunSQL(sSql)
        End If
    End Sub
    Private Sub doBack()
        'Response.Redirect(MapVirtualPath("Screens/REPL1.aspx"))
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        If Not ReplTask Is Nothing Then
            If ReplTask.ASSIGNMENTTYPE = WMS.Lib.TASKASSIGNTYPE.MANUAL Then
                If Not Session("REPLSRCSCREEN") Is Nothing Then
                    Response.Redirect(MapVirtualPath("Screens/" & Session("REPLSRCSCREEN") & ".aspx"))
                Else
                    Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
                End If
            Else
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            End If
        Else
            Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
        End If
    End Sub

    Private Function CheckLocation(ByRef flag As Boolean) As Boolean
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
            'Added for RWMS-470
            Dim strInputLocation As String = DO1.Value("TOLOCATION")
            If strInputLocation.Trim.ToLower <> repljob.toLocation.Trim.ToLower Then
                Return False
            End If
            'End Added for RWMS-470
            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(repljob.toLocation, DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"))

            'Dim inpLocation As String = DO1.Value("TOLOCATION")
            Dim inpWarehousearea As String = DO1.Value("WAREHOUSEAREA")

            Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

            If strConfirmationLocation.Trim.ToLower <> repljob.toLocation.Trim.ToLower OrElse _
                inpWarehousearea.Trim.ToLower <> repljob.toWarehousearea.Trim.ToLower Then
                Try
                    Dim locDesc As New WMS.Logic.Location(strConfirmationLocation.Trim.ToLower, inpWarehousearea.Trim.ToLower)
                    Dim locTo As New WMS.Logic.Location(repljob.toLocation.Trim.ToLower, repljob.toWarehousearea.Trim.ToLower)

                    If locTo.CHECKDIGITS <> locDesc.CHECKDIGITS Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong location confirmation"))
                        flag = False
                        Return False
                    Else
                        Return True
                    End If
                Catch ex As Exception
                    Return False
                End Try
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function CheckLocationOverrirde() As Boolean
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
            Dim toLocation As String = DO1.Value("TOLOCATION")

            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(repljob.toLocation, toLocation, DO1.Value("WAREHOUSEAREA"))

            'Dim inpLocation As String = DO1.Value("TOLOCATION")
            Dim inpWarehousearea As String = DO1.Value("WAREHOUSEAREA")

            Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

            If strConfirmationLocation.Trim.ToLower <> repljob.toLocation.Trim.ToLower OrElse _
                inpWarehousearea.Trim.ToLower <> repljob.toWarehousearea.Trim.ToLower Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        If (Session("REPLTSKTDetail") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        DO1.AddLabelLine("Note")
        DO1.AddLabelLine("TASKTYPE")
        DO1.AddLabelLine("LOADID")
        'RWMS-2173 Start
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        'RWMS-2173 End
        DO1.AddLabelLine("SKU")
        DO1.setVisibility("SKU", False)
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        'DO1.AddSpacer()
        Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

        Dim ld As New WMS.Logic.Load(ReplTaskDetail.FromLoad)
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)
        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        DO1.AddTextboxLine("UNITS")
        DO1.AddTextboxLine("EACHUNITS")

        DO1.AddDropDown("UOM")
        DO1.AddTextboxLine("TOLOCATION", False, "next")
        'DO1.AddTextboxLine("TOWAREHOUSEAREA")
        DO1.AddDropDown("TaskProblemCode")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            'Case "override"
            '    doOverride()
            Case "next"

                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("UNITS"), errMessage) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    DO1.Value("UNITS") = ""
                    Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                doNext()
            Case "back/clearunits"
                'ManageMutliUOMUnits.Clear(True)
                ManageMutliUOMUnits.Clear()
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                doBack()
            Case "reportproblem"
                ReportProblem()
            Case "addunits"

                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("UNITS"), errMessage) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    DO1.Value("UNITS") = ""
                    Return
                End If
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("UNITS") = ""
                'Case "clearunits"
                '    ManageMutliUOMUnits.Clear()
                '    ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        End Select
    End Sub

    Private Sub ReportProblem()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If Not CheckLocationOverrirde() Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Does Not Match"))
                Return
            End If
            If String.IsNullOrEmpty(DO1.Value("TaskProblemCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No problem code selected"))
                Exit Sub
            End If

            Dim toLocation As String = DO1.Value("TOLOCATION")

            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")

            Dim loc As New WMS.Logic.Location(toLocation, repljob.toWarehousearea)
            If loc.PROBLEMFLAG Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location already marked as problematic", "Location already marked as problematic")
            End If

            sendProblemToAudit(repljob)
            ReplTask.ReportProblem(repl, DO1.Value("TaskProblemCode"), toLocation, repljob.toWarehousearea, WMS.Logic.Common.GetCurrentUser)

            Dim ld As New WMS.Logic.Load(repljob.fromLoad)
            If ld.IsAssignedToTask = "" Then
                ld.LOCATION = toLocation
                Dim sql As String = $"update loads set location='{toLocation}' where loadid='{ld.LOADID}'"
                DataInterface.ExecuteScalar(sql)
            End If
            'ld.RequestPickUp(UserId)
            ld.PutAway("", "", WMS.Logic.Common.GetCurrentUser, True)

            Session("REPLLDPCK") = ld.LOADID

            ManageMutliUOMUnits.Clear(True)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location problem reported"))
            If String.IsNullOrEmpty(Session("REPLSRCSCREEN")) Then
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=repl"))
            Else
                Dim srcScreen As String = Session("REPLSRCSCREEN")
                Session.Remove("REPLSRCSCREEN")
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=" & srcScreen))
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            Return
        End Try
        doBack()
    End Sub


    Private Sub sendProblemToAudit(ByVal pwjob As ReplenishmentJob)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim ld As New WMS.Logic.Load(pwjob.fromLoad)

        Dim toLocation As String = DO1.Value("TOLOCATION")

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
        aq.Add("FROMLOC", ld.LOCATION)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)

        Dim SQL As String
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

        SQL = "SELECT PROBLEMCODEDESC FROM vTaskTypesProblemCodes WHERE TASKTYPE = '" & ReplTask.TASKTYPE & "' AND PROBLEMCODEID = '" & DO1.Value("TaskProblemCode") & "'"
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        aq.Add("NOTES", SQL)

        aq.Add("SKU", ld.SKU)
        aq.Add("TOLOAD", ld.LOADID)
        aq.Add("TOLOC", toLocation)
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

    Private Sub uomDropDownPreviousButtonClick(ByVal Source As Object, ByVal e As EventArgs)
        If Not IsNumeric(DO1.Ctrl("Units")) Then Exit Sub
        Dim dd As Made4Net.WebControls.MobileDropDown = CType(DO1.Ctrl("UOM"), Made4Net.WebControls.MobileDropDown)
        Dim uom As String
        If dd.SelectedIndex = 0 Then
            uom = dd.TableData.Rows(dd.TableData.Rows.Count - 1)("Value").ToString()
        Else
            uom = dd.TableData.Rows(dd.SelectedIndex - 1)("Value").ToString()
        End If
        ViewState()("UnitsEntered") = ""
        updateUnitsDropDown(uom, dd.TableData.Rows(dd.SelectedIndex)("Value").ToString(), dd.SelectedIndex)
    End Sub

    Private Sub uomDropDownNextButtonClick(ByVal Source As Object, ByVal e As EventArgs)
        If Not IsNumeric(DO1.Ctrl("Units")) Then Exit Sub
        Dim dd As Made4Net.WebControls.MobileDropDown = CType(DO1.Ctrl("UOM"), Made4Net.WebControls.MobileDropDown)
        Dim uom As String
        If dd.SelectedIndex = dd.TableData.Rows.Count - 1 Then
            uom = dd.TableData.Rows(0)("Value").ToString()
        Else
            uom = dd.TableData.Rows(dd.SelectedIndex + 1)("Value").ToString()
        End If
        ViewState()("UnitsEntered") = ""
        updateUnitsDropDown(uom, dd.TableData.Rows(dd.SelectedIndex)("Value").ToString(), dd.SelectedIndex)
    End Sub

    Private Sub updateUnitsDropDown(ByVal pUom As String, ByVal pPrevUOM As String, ByVal indx As String)
        If Not IsNumeric(DO1.Ctrl("Units")) Then Exit Sub
        Dim replJob As ReplenishmentJob = Session("REPLJobDetail")
        Dim sSku, sCons As String
        sSku = replJob.Sku
        sCons = replJob.Consignee
        Dim UnitsEntered As String = DO1.Value("EACHUNITS")
        Dim Units As String = DO1.Value("Units")

        Dim sql As String = String.Format("Select isnull(unitsperlowestuom,1) from skuuom where sku='{0}' and consignee='{1}' and uom='{2}'", sSku, sCons, pUom)
        Dim unitsPerLowestUom As Decimal = DataInterface.ExecuteScalar(sql)
        If unitsPerLowestUom = 0 Then unitsPerLowestUom = 1

        Dim oSku As New WMS.Logic.SKU(sCons, sSku)
        If Units = 0 And indx <> 0 Then
            Units = UnitsEntered
        Else
            UnitsEntered = Units * oSku.ConvertToUnits(pPrevUOM)
        End If

        Dim value As Decimal

        value = Math.Floor(UnitsEntered / unitsPerLowestUom) 'Math.Floor(Session("CreateLoadUnits") / unitsPerLowestUom)
        DO1.Value("Units") = value

        DO1.Value("EACHUNITS") = UnitsEntered




    End Sub

End Class