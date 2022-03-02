'*********************************************************************************************************************************************************
'   RDT logging mechanism. It will create the RDT error log files and folder for the logged in user.
'   RWMS-1238- Logging for RDT application.
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
Public Class RDTLogHandler
    ''' <summary>
    ''' In the sys_param, if RDTLogging=1 then only we will creating the new logs else there will be no logs for the RDT.
    ''' </summary>
    ''' <param name="pUserid"></param>
    ''' <remarks></remarks>

    Public Sub CheckAndInitiateLogging(ByVal pUserid As String)
        Dim strRDTLogging As String = Made4Net.Shared.AppConfig.GetSystemParameter("RDTLogging", "0")
        Dim rdtLogger As WMS.Logic.LogHandler
        Dim rdtLogHandler As New RDTLogHandler()

        If (Convert.ToInt32(strRDTLogging) = 1) Then
            rdtLogger = CreateLogFile(pUserid)
            If Not rdtLogger Is Nothing Then
                HttpContext.Current.Session("RDTLogger") = rdtLogger
            End If
        End If

        'End RDT Logging

    End Sub

    Private Function CreateLogFile(ByVal pUserid As String) As LogHandler


        Dim oLogger As WMS.Logic.LogHandler

        Try
            'RWMS-2456
            Dim dirPath As String = WMS.Logic.GetSysParam(Made4Net.Shared.ConfigurationSettingsConsts.RDTLogPath)
            dirPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(dirPath)
            dirPath = String.Concat(dirPath, pUserid)
            'RWMS-2635 Commented
            'oLogger = New LogHandler(dirPath + "\", pUserid & "_RDTErrorLog_" & DateTime.Now.ToString("_ddMMyyyy_hhmmss_") & New Random().Next() & ".txt")
            'RWMS-2635 Commented
            'RWMS-2635 START
            oLogger = New LogHandler(dirPath + "\", pUserid & "RDT" & DateTime.Now.ToString("ddMMyyyy_hhmmss") & New Random().Next(100, 999) & ".txt")
            'RWMS-2635 END
            oLogger.WriteTimeStamp = True
            oLogger.StartWrite()
            oLogger.Write("Initializing RDT Log for the Userid: " & pUserid)
        Catch ex As Exception
            oLogger = Nothing
        End Try
        Return oLogger
    End Function
End Class