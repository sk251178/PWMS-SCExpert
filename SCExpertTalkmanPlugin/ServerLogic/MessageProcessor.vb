Public Class MessageProcessor
    Inherits Made4Net.AppSessionManagement.AppClient

    Public Sub New()
        MyBase.New()
    End Sub

    Private Function CreateLogger(ByVal LogFileName As String) As WMS.Logic.LogHandler
        Dim strdirpath As String = Made4Net.DataAccess.Util.GetInstancePath()
        strdirpath += "\" + Made4Net.Shared.ConfigurationSettingsConsts.TalkmanServiceLogDirectory
        Dim oLogger As WMS.Logic.LogHandler = New WMS.Logic.LogHandler(strdirpath, LogFileName)
        oLogger.WriteTimeStamp = True
        Return oLogger
    End Function

    Public Overrides Sub ProcessMessage()
        Dim ClientMsg As ClientMessage = CType(Me.Request, AppRequest).GetClientMessage
        Dim ClientMsgConfig As MessagesInfo.MessageInfo = MessagesInfo.GetMessageConfig(ClientMsg.Name, Nothing)
        Dim RespMsg As ClientMessage
        Dim Page As AppPageProcessor
        Dim oLogger As WMS.Logic.LogHandler = Nothing

        'If Made4Net.Shared.AppConfig.Get("UseLogs", 0) = "1" Then
        If Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceUseLogs) = "1" Then
            Try
                'oLogger = New WMS.Logic.LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory"), ClientMsg.DeviceID & "_" & DateTime.Now.ToString("MMddyyyy_HHmmss") & "_" & ClientMsg.Name & "_" & New Random().Next() & ".txt")
                oLogger = CreateLogger(ClientMsg.DeviceID & "_" & DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") & "_" & ClientMsg.Name & "_" & New Random().Next() & ".txt")
                oLogger.StartWrite()
            Catch ex As Exception
            End Try
        End If
        Try

            Dim _handler As Made4Net.Shared.Evaluation.EvalFunction = New Made4Net.Shared.Evaluation.EvalFunction(ClientMsgConfig.MessageHandler)
            Page = Made4Net.Shared.Reflection.CreateInstance(_handler.AssemblyDll, _handler.ClassName)
            RespMsg = Page.Process(oLogger)

            Page = Nothing
            If ClientMsgConfig.Type.Equals(MessagesInfo.MessageType.LUT) Then
                If Not IsNothing(RespMsg) Then Response.write(RespMsg.Export)
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("ODR Message processed response code to be sent as per confirmation flag - " + ClientMsgConfig.Confirmation)
                    oLogger.Write("Instead sending response code Y")
                End If
                'Response.write(ClientMsgConfig.Confirmation)
                Response.write("Y")
            End If
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured:" & ex.ToString())
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured:" & ex.ToString())
            End If
        End Try
        If Not oLogger Is Nothing Then
            oLogger.EndWrite()
            oLogger = Nothing
        End If
        Response.Flush()
        System.GC.Collect()
    End Sub

End Class