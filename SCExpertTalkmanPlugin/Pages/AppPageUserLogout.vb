Public Class AppPageUserLogout
    Inherits AppPageProcessor

    Private Enum ResponseCode
        LogoffOK
        LogoffError
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        PrintMessageContent(oLogger)
        Dim sUserName As String = ""
        Dim sDevice As String = _msg(0)("Device ID").FieldValue
        Try
            sUserName = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        Catch ex As Made4Net.Shared.Authentication.UserNotLoggedInException
            If Not oLogger Is Nothing Then
                oLogger.Write("User is not logged in.")
            End If
            _responseCode = ResponseCode.LogoffError
            _responseText = "User is not logged in"
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Logout For user " & sUserName)
        End If
        Try
            Dim bClockOff As Boolean = Convert.ToBoolean(Convert.ToInt32(_msg(0)("ClockOff").FieldValue))
            If bClockOff Then
               ' WMS.Logic.ShiftInstance.ClockUser(sUserName, WMS.Lib.Shift.ClockStatus.OUT, "", "") ''  Commented for labor phase 2 - RWMS-1667 since the clock in and clock out newly implemented in future phase
                _responseCode = ResponseCode.LogoffOK
                If Not oLogger Is Nothing Then
                    oLogger.Write("User Clock Off Finished successfully.")
                End If
            Else
                '---
                'remove  user WHACTIVITY
                WMS.Logic.WHActivity.Delete(Me.Session().Item("Made4NetLoggedInUserName"))

                'remove the current user + session from license server
                'userid = Made4Net.Shared.Web.User.GetCurrentUser.UserName
                'sessionid = HttpContext.Current.Session.SessionID
                'ipAddress = HttpContext.Current.Request.UserHostAddress
                Dim userid, appId, sessionid, ipAddress, conn As String
                appId = Made4Net.AppSessionManagement.AppManager.Application("Made4NetLicensing_ApplicationId")
                userid = Session.Item("Made4NetLoggedInUserName")
                sessionid = Session.SessionID
                ipAddress = Session.Item("Made4NetLoggedInUserAddress")
                conn = String.Empty
                '  conn = Made4Net.DataAccess.DataInterface.ConnectionName
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
                    Made4Net.DataAccess.DataInterface.CloseAllAppSessionConnections(Me.Session())
                Catch ex As Exception
                End Try
                ''--
                Made4Net.Shared.Authentication.User.Logout()
                _responseCode = ResponseCode.LogoffOK
                If Not oLogger Is Nothing Then
                    oLogger.Write("User Logout Finished successfully.")
                End If
            End If
            Me.FillSingleRecord(oLogger, False)
            _resp.Record(0)("Device ID").FieldValue = sDevice
            _resp.Record(0)("Message Date").FieldValue = DateTime.Now
            Return _resp
        Catch ex As Exception
            _responseCode = ResponseCode.LogoffError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
    End Function

End Class