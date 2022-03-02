Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

<CLSCompliant(False)> Public Class DEL
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
            'start for 1420 and RWMS-1412
            MyBase.WriteToRDTLog(" Flow Navigated to page DEL ")
            'end for 1420 and RWMS-1412

            If Not Request.QueryString("sourcescreen") Is Nothing Then
                Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
            End If

            If Request.QueryString("printed") = 1 Then
                Session("printed") = 1
                MyBase.WriteToRDTLog(" Printted= " & "1")
            End If

            Dim sMobileSourceScreen As String = ""
            If Not IsNothing(Session("MobileSourceScreen")) Then
                sMobileSourceScreen = Session("MobileSourceScreen").ToString()
            End If
            MyBase.WriteToRDTLog(" DEL.aspx.vb: MobileSourceScreen= " & sMobileSourceScreen)
            'For partial and full picks , after completion of the pick task ,system should navigate to task summary to show labor performance stats only if the pick task is completed
            If (sMobileSourceScreen = "PCK" Or sMobileSourceScreen = "SPECPCK" Or sMobileSourceScreen = "PCKBagOut") Then
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session.Remove("PCKPicklistActiveContainerID")
                    MyBase.WriteToRDTLog(" Redirecting to Task Summmary: ShowPerformanceOnTaskComplete is 1")
                    If Not Session("PCKPicklist") Is Nothing Then
                        Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                        MyBase.WriteToRDTLog("Pick Task picklistID:" & oPicklist.PicklistID.ToString() & ",Pick type=" & oPicklist.PickType.ToString())
                        Dim tm As WMS.Logic.TaskManager = WMS.Logic.TaskManager.NewTaskManagerForPicklist(oPicklist.PicklistID)
                        MyBase.WriteToRDTLog("Pick Task ID:" & tm.Task.TASK.ToString() & ",Task Status=" & tm.Task.STATUS.ToString())
                        Session("MobileSourceScreen") = "DEL"
                        If tm.Task.STATUS = "COMPLETE" Then
                            MyBase.WriteToRDTLog("Navigation page rediecting to Task Summary page for TaskID:" & tm.Task.TASK.ToString())
                            MobileUtils.RedirectToTaskSummary(Session("MobileSourceScreen"), tm.Task.TASK)
                        End If
                    End If
                End If
            End If

            'RWMS-2646 RWMS-2645
            MyBase.WriteToRDTLog(" Checking if Delevery task assigned to the user.. ")
            'RWMS-2646 RWMS-2645 END

            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog(" Delevery task not assigned to the user ")
                'RWMS-2646 RWMS-2645 END

                doBack()
            End If

            'RWMS-2646 RWMS-2645
            MyBase.WriteToRDTLog(" Delevery task assigned to the user ")
            'RWMS-2646 RWMS-2645 END

            Dim delobj As DeliveryJob
            Try
                Dim tm As New WMS.Logic.TaskManager
                Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog(" Retreving the delivery task assigned to the user.. ")
                'RWMS-2646 RWMS-2645 END
                Dim deltsk As DeliveryTask
                If Not Session("pckpicklist") Is Nothing Then
                    Dim pcklst As Picklist = Session("pckpicklist")
                    deltsk = tm.getPicklistDeliveryTask(pcklst.PicklistID, WMS.Logic.Common.GetCurrentUser)
                    Session("PCKDELTask") = deltsk
                ElseIf Not Session("TMTask") Is Nothing Then
                    Dim oTask As WMS.Logic.Task
                    oTask = Session("TMTask")
                    deltsk = New DeliveryTask(oTask.TASK)
                Else
                    deltsk = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, rdtLogger)
                End If
                If deltsk Is Nothing Then

                    'RWMS-2646 RWMS-2645
                    MyBase.WriteToRDTLog(" delivery task not found for the user ")
                    'RWMS-2646 RWMS-2645 END

                    doBack()
                Else

                    'RWMS-2646 RWMS-2645
                    MyBase.WriteToRDTLog(" delivery task found for the user ")
                    'RWMS-2646 RWMS-2645 END


                    'If deltsk.ShouldPrintShipLabelOnPicking And deltsk.TASKTYPE = WMS.Lib.TASKTYPE.CONTCONTDELIVERY And Session("printed") = 0 Then
                    '    Session("PCKDeliveryTask") = deltsk
                    '    Session("printed") = 1
                    '    Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    'End If

                    'RWMS-2646 RWMS-2645
                    MyBase.WriteToRDTLog(" Checking ShowGoalTimeOnTaskAssignment.. ")
                    'RWMS-2646 RWMS-2645 END

                    If MobileUtils.ShowGoalTimeOnTaskAssignment() Then

                        'RWMS-2646 RWMS-2645
                        MyBase.WriteToRDTLog(" ShowGoalTimeOnTaskAssignment : TRUE ")
                        'RWMS-2646 RWMS-2645 END

                        If Session("ShowedTaskManager") Is Nothing Then
                            Session("ShowedTaskManager") = True
                            Session("TMTask") = deltsk
                            If Not Session("MobileSourceScreen") Is Nothing Then
                                Session("TargetScreen") = "screens/del.aspx?sourcescreen=" & Session("MobileSourceScreen")
                                'If Not Session("PCKBagOutPicking") Is Nothing Then
                                '    Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                                'Else
                                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER & "?sourcescreen=" & Session("MobileSourceScreen")))
                                'End If

                            Else
                                Session("TargetScreen") = "screens/del.aspx"
                                If Not Session("PCKBagOutPicking") Is Nothing Then
                                    Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                                Else
                                    Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                                End If
                            End If

                            'RWMS-2646 RWMS-2645
                        Else
                            MyBase.WriteToRDTLog(" ShowGoalTimeOnTaskAssignment : FALSE ")
                            'RWMS-2646 RWMS-2645 END

                        End If
                    End If
                End If

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog(" Checking the task for CONTAINERDELIVERY/CONTAINERLOADDELIVERY.. ")
                'RWMS-2646 RWMS-2645 END

                If deltsk.TASKTYPE = WMS.Lib.TASKTYPE.CONTDELIVERY Or deltsk.TASKTYPE = WMS.Lib.TASKTYPE.CONTLOADDELIVERY Or deltsk.TASKTYPE = WMS.Lib.TASKTYPE.LOADDELIVERY Then

                    'RWMS-2646 RWMS-2645
                    MyBase.WriteToRDTLog("Task is : " & deltsk.TASKTYPE)

                    MyBase.WriteToRDTLog("Checking if the picklist exist for the task")
                    'RWMS-2646 RWMS-2645 END

                    If deltsk.Picklist = "" Then

                        'RWMS-2646 RWMS-2645
                        MyBase.WriteToRDTLog("Picklist not exist for the task")
                        'RWMS-2646 RWMS-2645 END

                        Dim sqlUpdate As String
                        If deltsk.TASKTYPE = WMS.Lib.TASKTYPE.CONTDELIVERY Then
                            sqlUpdate = "UPDATE TASKS SET PICKLIST=(SELECT TOP 1 PICKLIST FROM PICKDETAIL WHERE TOCONTAINER='{0}') WHERE TASK='{1}'"
                            sqlUpdate = String.Format(sqlUpdate, deltsk.ToContainer, deltsk.TASK)
                        Else
                            sqlUpdate = "UPDATE TASKS SET PICKLIST=(SELECT TOP 1 PICKLIST FROM PICKDETAIL WHERE TOLOAD='{0}') WHERE TASK='{1}'"
                            sqlUpdate = String.Format(sqlUpdate, deltsk.TOLOAD, deltsk.TASK)
                        End If
                        Made4Net.DataAccess.DataInterface.RunSQL(sqlUpdate)

                        'RWMS-2646 RWMS-2645
                        MyBase.WriteToRDTLog("Updated the Task : SQL : " & sqlUpdate)
                        'RWMS-2646 RWMS-2645 END

                        deltsk.Picklist = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("SELECT TOP 1 PICKLIST FROM TASKS WHERE TASK='{0}'", deltsk.TASK)).ToString()

                        'RWMS-2646 RWMS-2645
                        MyBase.WriteToRDTLog("Retreived the PICKLIST for the Task : PICKLIST : " & deltsk.Picklist)
                        'RWMS-2646 RWMS-2645 END

                    End If
                End If
                Session("PCKDeliveryTask") = deltsk

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog("Retreiving the DeliveryJob..")
                'RWMS-2646 RWMS-2645 END

                delobj = deltsk.getDeliveryJob()

                'RWMS-2646 RWMS-2645
                If delobj Is Nothing Then
                    MyBase.WriteToRDTLog("Retreived : DeliveryJob not Found ")
                Else
                    MyBase.WriteToRDTLog("Retreived : DeliveryJob Found ")
                End If
                'RWMS-2646 RWMS-2645 END

                Session("printed") = 0

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog("Started binding delivery screen..")
                'RWMS-2646 RWMS-2645 END

                setDelivery(delobj)

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog("Finished binding delivery screen..")
                'RWMS-2646 RWMS-2645 END

                'Added for RWMS-527
                DO1.FocusField = "CONFIRMLOCATION"
                'End Added for RWMS-527
                'Commented the code for delivery consolidation
                'If Request.QueryString("delconsolidation") Is Nothing Then

                '    Dim taskid As String
                '    Dim cont As New RWMS.Logic.Container()


                '    taskid = cont.getContainerConsContainer(deltsk.ToContainer, deltsk.TASK)

                '    If Not IsNothing(taskid) And WMS.Logic.Task.Exists(taskid) Then
                '        'go to consolidation delivery screen
                '        Session("RWMSContainer") = cont

                '        Response.Redirect(MapVirtualPath("screens/DELConsolidation.aspx"))
                '        'Session("PCKDeliveryTask") = deltsk
                '    End If
                'End If

            Catch ex As Exception
                'If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                '    Throw
                'End If

                'RWMS-2646 RWMS-2645
                MyBase.WriteToRDTLog(ex.ToString())
                'RWMS-2646 RWMS-2645 END

            End Try
        End If
    End Sub

    Public Sub setDelivery(ByVal delobj As DeliveryJob)
        'Added for RWMS-1542 and RWMS-1442

        'Ended for RWMS-1542 and RWMS-1442

        'RWMS-2646 RWMS-2645
        MyBase.WriteToRDTLog("Checking for Delivery Job..")
        'RWMS-2646 RWMS-2645 END

        If delobj Is Nothing Then

            'Added for RWMS-1542 and RWMS-1442

            MyBase.WriteToRDTLog("Delivery Job not found.")

            'Ended for RWMS-1542 and RWMS-1442
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim deltask As Logic.DeliveryTask
                Dim tm As New WMS.Logic.TaskManager
                deltask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger())
                deltask.Complete(WMS.Logic.LogHandler.GetRDTLogger)
            End If
            doBack()
        End If
        'Added for RWMS-1542 and RWMS-1442

        MyBase.WriteToRDTLog("Delivery Job found.")

        'Ended for RWMS-1542 and RWMS-1442
        Session.Item("PCKDeliveryJob") = delobj
        Dim deltsk As DeliveryTask = Session("PCKDeliveryTask")
        If delobj.isContainer Then
            DO1.setVisibility("SKU", False)
            DO1.setVisibility("SKUDESC", False)
            DO1.setVisibility("UOM", False)
            'DO1.setVisibility("UOMUNITS", False)
            DO1.setVisibility("QTY", False)
            'RWMS-383 : BEGIN
            'DO1.setVisibility("HANDLINGUNIT", True)
            DO1.setVisibility("CONTAINERPAYLOADID", False)
            'RWMS-383 END
            DO1.setVisibility("HANDLINGUNITTYPE", False)
        Else
            DO1.setVisibility("SKU", True)
            DO1.setVisibility("SKUDESC", True)
            DO1.setVisibility("UOM", True)
            'DO1.setVisibility("UOMUNITS", True)
            DO1.setVisibility("QTY", True)

            DO1.Value("SKU") = delobj.Sku
            DO1.Value("SKUDESC") = delobj.skuDesc
            DO1.Value("UOM") = delobj.UOM
            'DO1.Value("UOMUNITS") = delobj.UOMUnits
            DO1.Value("QTY") = delobj.UOMUnits

            'RWMS-383 : BEGIN
            'DO1.setVisibility("HANDLINGUNIT", True)
            DO1.setVisibility("CONTAINERPAYLOADID", False) 'update 1/13/15
            'RWMS-383 END
            DO1.setVisibility("HANDLINGUNITTYPE", False)
            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("HANDLINGUNITTYPE")
            dd.AllOption = False
            dd.TableName = "handelingunittype"
            dd.ValueField = "container"
            dd.TextField = "containerdesc"
            dd.DataBind()
        End If

        If deltsk.TASKTYPE = WMS.Lib.TASKTYPE.CONTCONTDELIVERY Then
            DO1.setVisibility("SEQ", True)
            DO1.Value("SEQ") = delobj.OrderSeq
        Else
            DO1.setVisibility("SEQ", False)
        End If
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If delobj.IsHandOff Then
            DO1.setVisibility("Note", True)
            DO1.Value("Note") = trans.Translate("Task Destination Location is an Hand Off Location!")
        Else
            DO1.setVisibility("Note", False)
        End If
        DO1.Value("LOADID") = delobj.LoadId
        If delobj.toLocation <> "" Then
            DO1.Value("LOCATION") = delobj.toLocation
            'Start RWMS-1888 and RWMS-1819: On RDT RDTAutoPopDeliLoc prepopulated
            Dim whpSQL As String
            whpSQL = "select PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME='RDTAutoPopDeliLoc'"
            Dim paramval As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(whpSQL)
            If paramval = "1" Then
                DO1.Value("CONFIRMLOCATION") = delobj.toLocation
            Else
                DO1.Value("CONFIRMLOCATION") = ""
            End If
            'End RWMS-1888 and RWMS-1819: On RDT RDTAutoPopDeliLoc prepopulated

        Else
            DO1.Value("LOCATION") = deltsk.TOLOCATION
            'Start RWMS-1888 and RWMS-1819: On RDT RDTAutoPopDeliLoc prepopulated
            Dim whpSQL As String
            whpSQL = "select PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME='RDTAutoPopDeliLoc'"
            Dim paramval As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(whpSQL)
            If paramval = "1" Then
                DO1.Value("CONFIRMLOCATION") = deltsk.TOLOCATION
            Else
                DO1.Value("CONFIRMLOCATION") = ""
            End If
            'End RWMS-1888 and RWMS-1819: On RDT RDTAutoPopDeliLoc prepopulated

        End If
        If delobj.toWarehousearea <> "" Then
            DO1.Value("WAREHOUSEAREA") = delobj.toWarehousearea
        Else
            DO1.Value("WAREHOUSEAREA") = deltsk.TOWAREHOUSEAREA
        End If
        'RWMS-346 Start
        DO1.setVisibility("WAREHOUSEAREA", False)
        'RWMS-346 End
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Dim srcScreen As String
        Try
            srcScreen = Session("MobileSourceScreen")
        Catch ex As Exception

        End Try
        Session.Remove("PCKPicklist")
        Dim deltsk As DeliveryTask = Session("PCKDeliveryTask")

        If srcScreen = "" Or srcScreen Is Nothing Then
            Response.Redirect(MapVirtualPath("screens/Main.aspx"))
        Else
            If Session("PCKBagOutPicking") Is Nothing Then
                Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/PCKBagOut.aspx"))
            End If
        End If
    End Sub

    Private Sub doNext(Optional ByVal pDoOverride As Boolean = False)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If (Not Session("PCKDELTask") Is Nothing) Then
            Session.Remove("PCKDELTask")
        End If
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim del As DeliveryJob = Session.Item("PCKDeliveryJob")

        If (Session("PCKDeliveryJob") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        Dim pcklst As New Picklist(del.Picklist)
        If pcklst.isCompleted = False Then
            Session("PCKListToResume") = pcklst.PicklistID
        Else
            Session.Remove("PCKListToResume")
        End If

        Dim strdoWarehousearea As String = DO1.Value("WAREHOUSEAREA")

        Dim oCont As WMS.Logic.Container
        Dim ConfirmedLocation As String = String.Empty
        Try
            If pDoOverride Then
                If String.Equals(DO1.Value("CONFIRMLOCATION"), del.toLocation, StringComparison.OrdinalIgnoreCase) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Confirmation location equals to job target location, use next button instead"))
                    Return
                Else
                    ConfirmedLocation = WMS.Logic.Location.CheckLocationConfirmation(del.toLocation, DO1.Value("CONFIRMLOCATION"), strdoWarehousearea)
                    If String.Equals(ConfirmedLocation, del.toLocation, StringComparison.OrdinalIgnoreCase) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Confirmation location equals to job target location, use next button instead"))
                        Return
                    Else
                        ConfirmedLocation = DO1.Value("CONFIRMLOCATION")
                        'Added for RWMS-1542 and RWMS-1442

                        MyBase.WriteToRDTLog("Confirmed Location : " & ConfirmedLocation)

                        'Ended for RWMS-1542 and RWMS-1442
                    End If
                End If
            Else
                If Not String.Equals(DO1.Value("CONFIRMLOCATION"), del.toLocation, StringComparison.OrdinalIgnoreCase) Then
                    ConfirmedLocation = WMS.Logic.Location.CheckLocationConfirmation(del.toLocation, DO1.Value("CONFIRMLOCATION"), strdoWarehousearea)
                    If Not String.Equals(ConfirmedLocation, del.toLocation, StringComparison.OrdinalIgnoreCase) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Confirmation location not equals to job target location, use override button instead"))
                        Return
                    End If
                Else
                    ConfirmedLocation = DO1.Value("CONFIRMLOCATION")
                End If
            End If
            'RWMS-383 : "HANDLINGUNIT" replaced with "CONTAINERPAYLOADID"
            DO1.Value("CONTAINERPAYLOADID") = DO1.Value("CONTAINERPAYLOADID").Trim
            If pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
                If DO1.Value("CONTAINERPAYLOADID") <> "" Then
                    If DO1.Value("CONTAINERPAYLOADID") = DO1.Value("LOADID") Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Illegal handling unit"))
                        Return
                    End If

                    If WMS.Logic.Container.Exists(DO1.Value("CONTAINERPAYLOADID")) Then
                        oCont = New WMS.Logic.Container(DO1.Value("CONTAINERPAYLOADID"), True)
                        'Added for RWMS-1542 and RWMS-1442

                        MyBase.WriteToRDTLog("Container exist")

                        'Ended for RWMS-1542 and RWMS-1442
                        If oCont.Location <> ConfirmedLocation Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Handling unit location is different"))
                            Return
                        End If

                    Else
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Illegal handling unit"))
                        'Added for RWMS-1542 and RWMS-1442

                        MyBase.WriteToRDTLog("Container not exist")

                        'Ended for RWMS-1542 and RWMS-1442
                        Return
                    End If
                End If
            Else
                'RWMS-383 : "HANDLINGUNIT" replaced with "CONTAINERPAYLOADID"
                If WMS.Logic.Container.Exists(DO1.Value("CONTAINERPAYLOADID")) Then
                    oCont = New WMS.Logic.Container(DO1.Value("CONTAINERPAYLOADID"), True)
                    MyBase.WriteToRDTLog("Container exist")
                Else
                    oCont = Nothing
                    MyBase.WriteToRDTLog("Container not exist")
                End If
            End If
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForTask(del.TaskId)
            Session("TaskID") = tm.Task.TASK
            MyBase.WriteToRDTLog("Container Delivery Started..")
            CType(tm.Task, WMS.Logic.DeliveryTask).Deliver(ConfirmedLocation, strdoWarehousearea, del.IsHandOff, oCont, WMS.Logic.LogHandler.GetRDTLogger, pcklst.PicklistID)
            MyBase.WriteToRDTLog("Container Delivery Completed.")
            Session.Remove("PCKPicklistActiveContainerID")
            'go to task summary after task completion
            If (tm.Task.STATUS <> "COMPLETE") Then
                Dim sMobileSourceScreen As String = ""
                If Not IsNothing(Session("MobileSourceScreen")) Then
                    sMobileSourceScreen = Session("MobileSourceScreen").ToString()
                End If
                If sMobileSourceScreen <> "Staging" Then
                    DO1.Value("CONFIRMLOCATION") = ""
                    If pcklst.isCompleted = False Then
                        Session("PCKListToResume") = pcklst.PicklistID
                    Else
                        Session.Remove("PCKListToResume")
                    End If
                    If Not Session("PCKBagOutPicking") Is Nothing Then
                        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                    Else
                        Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                    End If
                End If
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            MyBase.WriteToRDTLog(ex.ToString())
            Return
        Catch ex As Exception
            MyBase.WriteToRDTLog(ex.ToString())
            Return
        End Try

        DO1.Value("CONFIRMLOCATION") = ""
        DO1.Value("CONTAINERPAYLOADID") = ""
        Dim deliveryobj As DeliveryJob
        Try
            Dim tm As New WMS.Logic.TaskManager
            Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
            Dim deltsk As DeliveryTask
            If Session("PCKListToResume") Is Nothing Then
                deltsk = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, rdtLogger)
            End If
            If deltsk Is Nothing Then
                MyBase.WriteToRDTLog("There are no delivery tasks assigned to the user.")
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session.Remove("PCKPicklistActiveContainerID")
                    MyBase.WriteToRDTLog(" Delivery screen. Container session Session.Remove(PCKPicklistActiveContainerID) has been removed and Redirect to Task Manager page. ")
                    If pcklst.isCompleted = False Then
                        Session("PCKListToResume") = pcklst.PicklistID
                    Else
                        Session.Remove("PCKListToResume")
                    End If
                    If Not Session("PCKBagOutPicking") Is Nothing Then
                        Session.Remove("ShowedTaskManager")
                        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                    Else
                        Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                    End If

                Else
                    Try
                        If pcklst.PickMethod = "PBI" Or pcklst.PickMethod = "PBW" Then
                            If pcklst.isCompleted = False Then
                                Session("PCKListToResume") = pcklst.PicklistID
                            Else
                                Session.Remove("PCKListToResume")
                            End If
                            If Not Session("PCKBagOutPicking") Is Nothing Then
                                Session.Remove("ShowedTaskManager")
                                Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                            Else
                                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                            End If
                        Else
                            doBack()
                        End If
                    Catch ex As Exception
                        MyBase.WriteToRDTLog(ex.ToString())
                    End Try
                End If
                Exit Sub
            End If
            MyBase.WriteToRDTLog("Found delivery tasks assigned to the user.")
            Session("PCKDeliveryTask") = deltsk
            deliveryobj = deltsk.getDeliveryJob()
            setDelivery(deliveryobj)
        Catch ex As Exception
            MyBase.WriteToRDTLog(ex.ToString)
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls

        DO1.AddLabelLine("Note")
        DO1.AddLabelLine("SEQ")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOADID", "CONTAINERPAYLOADID")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")

        DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("QTY")
        'DO1.AddSpacer()
        DO1.AddTextboxLine("CONTAINERPAYLOADID") 'Display label after translation will be "Payload/Container ID"

        DO1.AddDropDown("HANDLINGUNITTYPE")
        DO1.AddTextboxLine("CONFIRMLOCATION", True, "next")

        MyBase.WriteToRDTLog("Child control creation has completed in DEL.aspx")


    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "override"
                doNext(True)
            Case "back"
                doBack()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class