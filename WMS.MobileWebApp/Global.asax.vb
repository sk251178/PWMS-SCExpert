Imports System.Web
Imports System.Web.SessionState
Imports Made4Net.Shared
Imports Made4Net.General
Imports WMS.Logic

<CLSCompliant(False)> Public Class [Global]
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

    Public Shared userSessionManager As UserSessionManager
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''
        Dim appId As String = System.Configuration.ConfigurationManager.AppSettings().Get("Made4NetLicensing_ApplicationId")
        'Dim SQL As String = "select param_value from sys_param where param_code = 'ApplicationId'"
        'Dim appId As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        Dim InstancePath As String = Made4Net.DataAccess.Util.GetInstancePath()
        Application("Made4NetLicensing_ApplicationId") = appId
        Dim logPath As String = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.ApplicationLogDirectory)
        logPath = InstancePath + "\\" + logPath
        Dim configPath As String = AppConfig.GetSystemParameter("XmlConfigPath")
        configPath = InstancePath + "\\" + configPath
        LogConfiguration.InitializeDiagnostics(configPath)
        ExceptionConfiguration.BuildExceptionConfiguration(logPath, Nothing, Nothing, Nothing)
        userSessionManager = New UserSessionManager()
        Application("UserSessionManager") = userSessionManager
        userSessionManager.UserHasBeenRemoved = AddressOf WHActivity.Delete
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        userSessionManager = Application("UserSessionManager")
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
        System.Diagnostics.Debug.Print("Global end " + Date.Now.ToString())
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        '' Fires when an error occurs
        If TypeOf Server.GetLastError.InnerException Is Made4Net.DataAccess.InvalidLicenseException Then
            Context.ClearError()

            'remove  user WHACTIVITY
            Try
                userSessionManager.RemoveSession(HttpContext.Current.Session().Item("Made4NetLoggedInUserName"), HttpContext.Current.Session.SessionID, HttpContext.Current.Request.UserHostAddress, LogHandler.GetRDTLogger())
                Made4Net.DataAccess.DataInterface.CloseAllConnections()
                Made4Net.Shared.Authentication.User.Logout()
            Catch ex As Exception
            End Try

            HandheldPopupNAlertMessageHandler.DisplayMessage(New Object(), "No Licence found!")
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        Else

        End If
    End Sub
    'Added for RWMS-2009 and RWMS-2005 Start
    Protected Sub Application_AcquireRequestState(sender As Object, e As EventArgs)

        If Not HttpContext.Current.Session Is Nothing Then
            If (Session.Count = 0) AndAlso Not (Request.Url.AbsolutePath.EndsWith("login.aspx", StringComparison.InvariantCultureIgnoreCase)) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
            End If
        End If

    End Sub

    'Added for RWMS-2009 and RWMS-2005 End

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''


        'System.Diagnostics.Debugger.Break()
        'remove the current user + session from license server
        'userid = Made4Net.Shared.Web.User.GetCurrentUser.UserName
        'sessionid = HttpContext.Current.Session.SessionID
        'ipAddress = HttpContext.Current.Request.UserHostAddress
        Dim userid, appId, sessionid, ipAddress, conn, browserClosed As String
        appId = Application("Made4NetLicensing_ApplicationId")
        userid = Me.Session().Item("Made4NetLoggedInUserName")
        browserClosed = Me.Session().Item("BrowserClose")

        'System.Diagnostics.Debugger.Break()

        'Added for RWMS-2540 Start
        Dim warehouseId, mheId, terminalType, logoutRdtUser As String
        warehouseId = Me.Session().Item("Warehouse_CurrentWarehouseId")
        mheId = Me.Session().Item("MHEID")
        terminalType = Me.Session().Item("TERMINALTYPE")
        logoutRdtUser = Me.Session().Item("LogoutRDTUser")
        'Added for RWMS-2540 End

        'System.Diagnostics.Debugger.Break()

        sessionid = Me.Session().SessionID
        ipAddress = Me.Session().Item("Made4NetLoggedInUserAddress")
        conn = Made4Net.DataAccess.DataInterface.ConnectionName


        Dim rdtLogger As WMS.Logic.LogHandler = Nothing
        If (Me.Session() Is Nothing) Then
            rdtLogger = Nothing
        ElseIf Not (Me.Session().Item("RDTLogger") Is Nothing) Then
            rdtLogger = Me.Session().Item("RDTLogger")
        End If

        If Not rdtLogger Is Nothing Then
            If browserClosed = "true" Then
                rdtLogger.Write("Session cancelled by user " & userid)
            Else
                rdtLogger.Write("Session ended for user " & userid)
            End If
        End If

        'Added for RWMS-2540 Start
        If logoutRdtUser = String.Empty Then
            Dim note As String = IIf(Not String.IsNullOrEmpty(browserClosed), "Browser Closed", "TIMEOUT")
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LOGOUT)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOGOUT)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("USERID", userid)
            aq.Add("MHEID", mheId)
            aq.Add("TERMINALTYPE", terminalType)
            aq.Add("FROMWAREHOUSEAREA", warehouseId)
            aq.Add("TOWAREHOUSEAREA", warehouseId)
            aq.Add("NOTES", note)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
            aq.Send(WMS.Lib.Actions.Audit.LOGOUT, warehouseId)
            If Not browserClosed Then
                userSessionManager.RemoveSession(userid, sessionid, ipAddress, LogHandler.GetRDTLogger())
            End If
        End If
        'Added for RWMS-2540 End

        Dim Session_Id As String = ipAddress & "_" & sessionid
        Dim key As String = "DisConnect" & "@" & userid & "@" & Session_Id & "@" & appId & "@" & conn
        Dim SQL As String = "select param_value from sys_param where param_code = 'LicenseServer'"
        Dim server As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        SQL = "select param_value from sys_param where param_code = 'LicenseServerPort'"
        Dim port As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        Dim tcpClient As New Made4Net.Net.TCPIP.Client(server, port)
        Dim ret As Boolean = Convert.ToBoolean(tcpClient.SendRequest(key))

        ''close all connections to the DB and release memory
        'Made4Net.DataAccess.DataInterface.CloseAllConnections()
        'Made4Net.Shared.Web.User.Logout()
        Try
            Made4Net.DataAccess.DataInterface.CloseAllConnections(Me.Session)
        Catch ex As Exception
        End Try
        ' Made4Net.Mobile.Common.GoToMenu()
        ' Made4Net.Shared.Web.User.Logout()
        ' WMS.Logic.Common.GotoLogin()
        'Response.Redirect("m4nScreens/Login.aspx")
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class