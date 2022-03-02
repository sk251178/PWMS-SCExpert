Public Class AppListener
    Implements Made4Net.AppSessionManagement.IAppSessionListener

    Public Sub onApplicationStart(ByVal Application As Made4Net.AppSessionManagement.AppState) Implements Made4Net.AppSessionManagement.IAppSessionListener.onApplicationStart
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''
        Dim appId As String = System.Configuration.ConfigurationManager.AppSettings().Get("Made4NetLicensing_ApplicationId")
        'Dim SQL As String = "select param_value from sys_param where param_code = 'ApplicationId'"
        'Dim appId As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        Application("Made4NetLicensing_ApplicationId") = appId
    End Sub

    Public Sub onSessionEnd(ByVal Session As Made4Net.AppSessionManagement.AppSessionState) Implements Made4Net.AppSessionManagement.IAppSessionListener.onSessionEnd
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''
        'remove the current user + session from license server
        'userid = Made4Net.Shared.Web.User.GetCurrentUser.UserName
        'sessionid = HttpContext.Current.Session.SessionID
        'ipAddress = HttpContext.Current.Request.UserHostAddress
        Try
            WMS.Logic.WHActivity.Delete(Session("Made4NetLoggedInUserName"))
        Catch ex As Made4Net.DataAccess.InvalidLicenseException
            '' don't write to log - it  could be because user got here from AppPageUserLogoff screen 
            ''which calls User.Logout (in Made4Net.Shared.Authentication) that calls session.Abandon 
            ''-that calls OnSessionEnd but now the user is already removed from the license server

        End Try
        ''  WMS.Logic.WHActivity.Delete(Session.Item("Made4NetLoggedInUserName"))
        Dim userid, appId, sessionid, ipAddress, conn As String
        appId = Made4Net.AppSessionManagement.AppManager.Application("Made4NetLicensing_ApplicationId")
        userid = Session.Item("Made4NetLoggedInUserName")
        sessionid = Session.SessionID
        ipAddress = Session.Item("Made4NetLoggedInUserAddress")

        conn = Made4Net.DataAccess.DataInterface.ConnectionName
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
            Made4Net.DataAccess.DataInterface.CloseAllAppSessionConnections(Session)
        Catch ex As Exception
        End Try
        'Made4Net.Shared.Web.User.Logout()
        'Common.GotoLogin()
    End Sub

    Public Sub onSessionStart(ByVal Session As Made4Net.AppSessionManagement.AppSessionState) Implements Made4Net.AppSessionManagement.IAppSessionListener.onSessionStart

    End Sub
End Class
