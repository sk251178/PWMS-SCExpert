Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WMS.Lib

<CLSCompliant(False)> Public Class Nav
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents nav As Made4Net.Mobile.WebCtrls.NavigationInterface
    Protected WithEvents NavigationInterface1 As Made4Net.Mobile.WebCtrls.NavigationInterface

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            'If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '    Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            'End If
            'Start RWMS-1487


            MyBase.WriteToRDTLog(" Flow Navigated to page Main ")


            If CheckValidSetWareHouseState() Then
                Dim rdtLogger As LogHandler = WMS.Logic.LogHandler.GetRDTLogger()

                If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTPUTAWAY, rdtLogger) Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/CNTPWCNF.aspx"))
                End If
                If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, rdtLogger) Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPKC.aspx"))
                End If
                If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, rdtLogger) Then
                    Session()("TaskID") = WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, rdtLogger)
                    'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=Main"))
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                End If
                Session.Remove("MobileSourceScreen")
            Else
                'Since the warehouse is invalid do a log off
                DoLogOff()
                'End RWMS-1487
            End If
        End If
        Screen1.ShowMainMenu = True
    End Sub

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

    Private Sub NavigationInterface1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.NavigationInterface.ButtonClickEventArgs) Handles NavigationInterface1.ButtonClick
        If e.CommandText.ToLower = "logoff" Then
            DoLogOff() 'RWMS-1487
        End If
        If e.CommandText.ToLower = "requesttask" Then
            'Me.Screen1.Warn("Task Requested")
            'GetTask()
            WriteToRDTLog(" Main.ButtonClick('requesttask')")
            WriteToRDTLog("User Clicked On GetTask Button on Main Menu")
            Session("IsGetTaskRequestFromUser") = "1"
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        End If
    End Sub
    'Start RWMS-1487
    Private Sub DoLogOff()


        MyBase.WriteToRDTLog(" Main.DoLogOff - Proceeding to logoff")
        'Added for RWMS-2540 Start
        Made4Net.Shared.ContextSwitch.Current.Session("LogoutRDTUser") = Nothing
        'Added for RWMS-2540 End

        Dim url As String = GetUrlWithParams()
        MobileUtils.Session_End(Application("Made4NetLicensing_ApplicationId"), True)
        Made4Net.Shared.Authentication.User.Logout()

        'Added for RWMS-2540 Start
        Made4Net.Shared.ContextSwitch.Current.Session("LogoutRDTUser") = 1
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LOGOUT)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOGOUT)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("MHEID", Session("MHEID"))
        aq.Add("TERMINALTYPE", Session("TERMINALTYPE"))
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Send(WMS.Lib.Actions.Audit.LOGOUT)
        'Added for RWMS-2540 End

        MobileUtils.ClearBasicSessionOnLogOff()

        MyBase.WriteToRDTLog(" Main.DoLogOff - Cleared Session items")

        Response.Redirect(url)
    End Sub
    'End RWMS-1487
    Private Function GetUrlWithParams() As String
        Dim ret, urlparams As String
        Dim skin, wh, mheid, mhtype, terminaltype, wharea, locationid As String
        skin = Session("Skin")
        wh = Session("WH")
        mheid = Session("MHEID")
        mhtype = Session("MHTYPE")
        terminaltype = Session("TERMINALTYPE")
        wharea = Session("LoginWHArea")
        locationid = Session("LoginLocation")
        urlparams = "?"
        If skin <> String.Empty Then
            urlparams = urlparams & String.Format("Skin={0}&", skin)
        End If
        If wh <> String.Empty Then
            urlparams = urlparams & String.Format("WH={0}&", wh)
        End If
        If mheid <> String.Empty Then
            urlparams = urlparams & String.Format("MHEID={0}&", mheid)
        End If
        If mhtype <> String.Empty Then
            urlparams = urlparams & String.Format("MHTYPE={0}&", mhtype)
        End If
        If terminaltype <> String.Empty Then
            urlparams = urlparams & String.Format("TERMINALTYPE={0}&", terminaltype)
        End If
        If wharea <> String.Empty Then
            urlparams = urlparams & String.Format("WAREHOUSEAREA={0}&", wharea)
        End If
        If locationid <> String.Empty Then
            urlparams = urlparams & String.Format("LOCATION={0}&", locationid)
        End If
        ret = Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx") & urlparams.TrimEnd("&".ToCharArray())
        Return ret
    End Function

#Region "Task Manager Methods"

    Private Function RequestTask() As Task
        Dim ts As New Logic.TaskManager
        Return ts.GetTaskFromTMService(GetCurrentUser, True, LogHandler.GetRDTLogger())
    End Function

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        WriteToRDTLog(" Main.CheckAssigned")

        If Logic.TaskManager.isAssigned(UserId, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim oTask As Task
            Try
                oTask = Logic.TaskManager.getUserAssignedTask(UserId, WMS.Logic.LogHandler.GetRDTLogger())
            Catch ex As Exception
            End Try
            If Not oTask Is Nothing Then
                Session("TMTask") = oTask
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Private Sub GetTask()
        'Start RWMS-1487
          MyBase.WriteToRDTLog(" Main.GetTask - Proceeding to get task for user")

        'End RWMS-1487
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                'Dim tm As New WMS.Logic.TaskManager
                'tm.RequestTask(UserId)
                Dim oTask As Task = RequestTask()
                WMS.Logic.Picking.BagOutProcess = False
                Session.Remove("PCKBagOutPicking")
                'CheckAssigned()
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Catch ex As Made4Net.Shared.M4NException
                'Start RWMS-1487

                MyBase.WriteToRDTLog(" Main.GetTask - A shared exception occured while getting task - " + ex.ToString())

                'End  RWMS-1487
                'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                'Start RWMS-1487

                MyBase.WriteToRDTLog(" Main.GetTask - An exception occured while getting task - " + ex.ToString())

                'End RWMS-1487
                'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            End Try
        Else
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            'Dim tm As New WMS.Logic.TaskManager
            'Dim oTask As Task = tm.getUserAssignedTask(UserId)
            'If Not oTask Is Nothing Then
            '    Session("TMTaskId") = oTask
            '    Select Case oTask.TASKTYPE.ToUpper
            '        Case WMS.Lib.TASKTYPE.CONSOLIDATION
            '            Response.Redirect(MapVirtualPath("Screens/CONS.aspx"))
            '        Case WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY
            '            Response.Redirect(MapVirtualPath("Screens/CONS.aspx"))
            '        Case WMS.Lib.TASKTYPE.CONTCONTDELIVERY
            '            Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            '        Case WMS.Lib.TASKTYPE.CONTDELIVERY
            '            Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            '        Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY
            '            Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            '        Case WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
            '            Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
            '        Case WMS.Lib.TASKTYPE.CONTPUTAWAY
            '            Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
            '        Case WMS.Lib.TASKTYPE.COUNTING
            '            Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
            '        Case WMS.Lib.TASKTYPE.DELIVERY
            '            Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            '        Case WMS.Lib.TASKTYPE.FULLPICKING
            '            Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            '        Case WMS.Lib.TASKTYPE.HOTREPL
            '            Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
            '        Case WMS.Lib.TASKTYPE.LOADDELIVERY
            '            Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            '        Case WMS.Lib.TASKTYPE.LOADPUTAWAY
            '            Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
            '        Case WMS.Lib.TASKTYPE.NORMALREPL
            '            Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
            '        Case WMS.Lib.TASKTYPE.PARALLELPICKING
            '            Response.Redirect(MapVirtualPath("Screens/PARPCK1.aspx"))
            '        Case WMS.Lib.TASKTYPE.PARTIALPICKING
            '            Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            '        Case WMS.Lib.TASKTYPE.PICKING
            '            Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            '        Case WMS.Lib.TASKTYPE.PUTAWAY
            '            Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
            '        Case WMS.Lib.TASKTYPE.REPLENISHMENT
            '            Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
            '    End Select
            'End If
        End If
    End Sub

#End Region

    'Start RWMS-1487/RWMS-1444 Making check for no warehouse set and logging that in the event log
    Private Function CheckValidSetWareHouseState() As Boolean
        Dim blnResult As Boolean = True
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        MyBase.WriteToRDTLog(" Main.CheckTheSetWareHouseState - Current set ware-house " + Warehouse.CurrentWarehouse)

        Dim strCurrentWarehouse As String = Warehouse.CurrentWarehouse
        If strCurrentWarehouse Is Nothing Then
            blnResult = False
            Try
                Dim strUser As String = Logic.GetCurrentUser
                Dim evtLog As New Made4Net.General.EventLogger("RDT-Main.aspx", Made4Net.General.LogLevel.Warning, False)
                Dim oLog As New Made4Net.General.Log("Dangerous condition with no warehouse set for user " + strUser)
                evtLog.Log(oLog)
            Catch ex As Exception
            End Try
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid warehouse selection. Please re-login to proceed."))
        End If
        Return blnResult
    End Function
    'End RWMS-1487
End Class