Imports System.Web
Imports System.Web.SessionState
Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.General

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Northwoods.GoWeb.GoView.VersionName = "vQjAzWP55EE9y3uJj+68eMwG0d9HEOnC18sFWO6VodMCHNux0hCoi3g7C4W5ndTr1NtVQKucnqfeBupr4SuqS2wnGAbzXcbfAzOIE6Y5jdRCsOTrfkC7pbPJXSecDHED"
        '''''''''''''''''''''''''''''''
        ' Handles the Job Scheduler   '
        '''''''''''''''''''''''''''''''
        'Dim vDir As String = HttpContext.Current.Server.MapPath("")
        'Dim Dir As String = WMS.Logic.GetSysParam("ApplicationLogDirectory")
        'Application("JobScheduler") = New Made4Net.Shared.jobScheduling.JobScheduler(TimeSpan.FromMinutes(1), Dir, vDir)
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''
        Dim appId As String = System.Configuration.ConfigurationManager.AppSettings().Get("Made4NetLicensing_ApplicationId")
        'Dim SQL As String = "select param_value from sys_param where param_code = 'ApplicationId'"
        'Dim appId As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        Application("Made4NetLicensing_ApplicationId") = appId
        Dim logPath As String = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.ApplicationLogDirectory)
        logPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(logPath)
        Dim configPath As String = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.XmlConfigPath)
        configPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(configPath)
        LogConfiguration.InitializeDiagnostics(configPath)
        ExceptionConfiguration.BuildExceptionConfiguration(logPath, Nothing, Nothing, Nothing)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        'If TypeOf Server.GetLastError.InnerException Is Made4Net.DataAccess.InvalidLicenseException Then
        '    Context.ClearError()
        '    Common.GotoLogin()
        'End If
        'System.Diagnostics.Debugger.Break()
        If TypeOf Server.GetLastError.InnerException Is Made4Net.Shared.Authentication.UserNotLoggedInException Then
            Context.ClearError()
            Common.GotoLogin()
        End If
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''
        'remove the current user + session from license server
        'userid = Made4Net.Shared.Web.User.GetCurrentUser.UserName
        'sessionid = HttpContext.Current.Session.SessionID
        'ipAddress = HttpContext.Current.Request.UserHostAddress

        Dim userid, appId, sessionid, ipAddress, conn As String
        appId = Application("Made4NetLicensing_ApplicationId")
        userid = Me.Session().Item("Made4NetLoggedInUserName")

        'System.Diagnostics.Debugger.Break()

        'Added for RWMS-2540 Start
        Dim warehouseId, logoutuser As String
        warehouseId = Me.Session().Item("Warehouse_CurrentWarehouseId")
        logoutuser = Me.Session().Item("LogoutUser")
        'Added for RWMS-2540 End

        'warehousename = Me.Session().Item("Warehouse_CurrentWarehouseName")
        'System.Diagnostics.Debugger.Break()
        'Dim sessionvariable As System.Web.SessionState.HttpSessionState = Me.Session()
        'Dim sessionvariable2 As System.Web.SessionState.HttpSessionState = HttpContext.Current.Session
        sessionid = Me.Session().SessionID
        ipAddress = Me.Session().Item("Made4NetLoggedInUserAddress")
        conn = Made4Net.DataAccess.DataInterface.ConnectionName
        'System.Diagnostics.Debugger.Break()
        'Added for RWMS-2540 Start
        If logoutuser = String.Empty Then
            Dim strTimeout As String = "TIMEOUT"
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LOGOUT)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOGOUT)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("USERID", userid)
            aq.Add("FROMWAREHOUSEAREA", warehouseId)
            aq.Add("TOWAREHOUSEAREA", warehouseId)
            aq.Add("NOTES", strTimeout)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
            aq.Send(WMS.Lib.Actions.Audit.LOGOUT, warehouseId)
            'System.Diagnostics.Debugger.Break()
        End If
        'Added for RWMS-2540 End
        Try
            Dim Session_Id As String = ipAddress & "_" & sessionid
            Dim key As String = "DisConnect" & "@" & userid & "@" & Session_Id & "@" & appId & "@" & conn
            Dim SQL As String = "select param_value from sys_param where param_code = 'LicenseServer'"
            Dim server As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
            SQL = "select param_value from sys_param where param_code = 'LicenseServerPort'"
            Dim port As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
            Dim tcpClient As New Made4Net.Net.TCPIP.Client(server, port)
            Dim ret As Boolean = Convert.ToBoolean(tcpClient.SendRequest(key))
        Catch ex As Exception
        End Try
        'close all connections to the DB and release memory
        Try
            Made4Net.DataAccess.DataInterface.CloseAllConnections(Me.Session)
        Catch ex As Exception
        End Try
        'Made4Net.Shared.Web.User.Logout()
        'Common.GotoLogin()
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class