Imports WMS.Logic
Imports Made4Net.Mobile

Public Class HandheldPopupNAlertMessageHandler

    Public Shared Sub DisplayMessage(ByVal page As System.Web.UI.Page, ByVal pText As String)
        Try
            If (HttpContext.Current Is Nothing) Then
                Return
            ElseIf Not (HttpContext.Current.Request Is Nothing) Then
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                If HttpContext.Current.Request.Url.ToString().ToLower().Contains("/m4nscreens/") Then
                    ' Add the message as Client Script
                    If Not String.IsNullOrEmpty(pText) Then
                        ScriptManager.RegisterStartupScript(page, page.[GetType](), "showalert", "alert('" & t.Translate(pText) & "');", True)
                        WriteToRDTLog("Popup : " + pText)
                    End If
                ElseIf HttpContext.Current.Request.Url.ToString().ToLower().Contains("/screens/") Then
                    MessageQue.Enqueue(pText)
                    WriteToRDTLog("Popup : " + pText)
                End If
            End If
        Catch ex As Exception
            WriteToRDTLog("######################## - Exception occured in method HandheldPopupNAlertMessageHandler.DisplayMessage ######################## " + ex.ToString())
        End Try
    End Sub

    Private Shared Sub WriteToRDTLog(ByVal Data As String)
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not rdtLogger Is Nothing Then
            Dim strRDTLogging As String = Made4Net.Shared.AppConfig.GetSystemParameter("RDTExtendedLogging", "0")
            If (Convert.ToInt32(strRDTLogging) = 1) Then
                rdtLogger.Write(Data)
            End If
        End If
    End Sub

End Class
