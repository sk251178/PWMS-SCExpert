'*********************************************************************************************************************************************************
'   Workstation logging mechanism. It will create the Workstation error log files and folder for the logged in user.
'   RWMS-2581 RWMS-2554 - Logging for Workstation application.
'
'*********************************************************************************************************************************************************
Imports System
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Web
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports WMS.Logic
Imports System.Web
Public Class WMSLogHandler
    ''' <summary>
    ''' In the sys_param, if WMSLogging=1 then only we will creating the new logs else there will be no logs for the WMS.
    ''' </summary>
    ''' <param name="pUserid"></param>
    ''' <remarks></remarks>

    Public Sub CheckAndInitiateLogging(ByVal pUserid As String)
        Dim strWMSLogging As String = Made4Net.Shared.AppConfig.GetSystemParameter("WMSLogging", "0")
        Dim wmsLogger As ILogHandler
        Dim wmsLogHandler As New WMSLogHandler()

        If (Convert.ToInt32(strWMSLogging) = 1) Then
            wmsLogger = CreateLogFile(pUserid)
            If Not wmsLogger Is Nothing Then
                HttpContext.Current.Session("WMSLogger") = wmsLogger
            End If
        End If

        'End WMS Logging

    End Sub

    Private Function CreateLogFile(ByVal pUserid As String) As ILogHandler


        Dim oLogger As ILogHandler

        Try
            'RWMS-2456
            Dim dirPath As String = WMS.Logic.GetSysParam(Made4Net.Shared.ConfigurationSettingsConsts.WMSLogPath)
            dirPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(dirPath)
            dirPath = String.Concat(dirPath, pUserid)
            oLogger = New LogHandler(dirPath + "\", pUserid & "_WMSErrorLog_" & DateTime.Now.ToString("_ddMMyyyy_hhmmss_") & New Random().Next() & ".txt")
            oLogger.WriteTimeStamp = True
            oLogger.StartWrite()
            oLogger.Write("Initializing Workstation Log for the Userid: " & pUserid)
        Catch ex As Exception
            Try
                'Write the error to event log
                Dim evtLog As New Made4Net.General.EventLogger("WMSLogHandler", Made4Net.General.LogLevel.Information, False)
                Dim oLog As New Made4Net.General.Log("Error while trying to create loghandler for user " & pUserid & ". Error Details: " & ex.ToString())
                evtLog.Log(oLog)
            Catch innerex As Exception

            End Try
            oLogger = Nothing
        End Try
        Return oLogger
    End Function
End Class