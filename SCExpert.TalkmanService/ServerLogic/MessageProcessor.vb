Public Class MessageProcessor
    Inherits Made4Net.AppSessionManagement.AppClient

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Sub ProcessMessage()
        Dim ClientMsg As ClientMessage = CType(Me.Request, AppRequest).GetClientMessage
        Dim ClientMsgConfig As MessagesInfo.MessageInfo = MessagesInfo.GetMessageConfig(ClientMsg.Name)
        Dim RespMsg As ClientMessage
        Dim Page As AppPageProcessor
        Dim oLogger As WMS.Logic.LogHandler = Nothing
        If Made4Net.Shared.AppConfig.Get("UseLogs", 0) = "1" Then
            Try
                oLogger = New WMS.Logic.LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory"), ClientMsg.Name & "_" & DateTime.Now.ToString("MMddyyyy_HHmmss") & New Random().Next() & ".txt")
                oLogger.WriteTimeStamp = True
                oLogger.StartWrite()
            Catch ex As Exception
            End Try
        End If
        Try

            Dim _handler As Made4Net.Shared.Evaluation.EvalFunction = New Made4Net.Shared.Evaluation.EvalFunction(ClientMsgConfig.MessageHandler)
            Page = Made4Net.Shared.Reflection.CreateInstance(_handler.AssemblyDll, _handler.ClassName)
            RespMsg = Page.Process(oLogger)
            'Select Case ClientMsg.Name
            '    Case "LOGINREQ"
            '        Page = New AppPageUserLogin()
            '        RespMsg = Page.Process(oLogger)
            '    Case "LOGOFFREQ"
            '        Page = New AppPageUserLogout()
            '        RespMsg = Page.Process(oLogger)
            '    Case "TASKASSIGNREQ"
            '        Page = New AppPageTaskAssignment()
            '        RespMsg = Page.Process(oLogger)
            '    Case "TASKRELEASEREQ"
            '        Page = New AppPageTaskRelease()
            '        RespMsg = Page.Process(oLogger)
            '    Case "CLOSECONTREQ" 'add logs
            '        Page = New AppPageCloseContainer
            '        RespMsg = Page.Process(oLogger)
            '    Case "PICKINGLISTREQ"
            '        Page = New AppPageGetPick()
            '        RespMsg = Page.Process(oLogger)
            '    Case "PICKREQ" 'add logs
            '        Page = New AppPagePick()
            '        RespMsg = Page.Process(oLogger)
            '    Case "PRINTREQ"
            '        Page = New AppPagePrint()
            '        RespMsg = Page.Process(oLogger)
            '    Case "PICKINGINFOREQ"
            '        Page = New AppPagePickingInformation()
            '        RespMsg = Page.Process(oLogger)
            '    Case "DELIVERYREQ" 'add logs
            '        Page = New AppPageDelivery
            '        RespMsg = Page.Process(oLogger)
            '    Case "COUNTREQ" 'add logs
            '        Page = New AppPageCount
            '        RespMsg = Page.Process(oLogger)
            'End Select
            Page = Nothing
            If ClientMsgConfig.Type.Equals(MessagesInfo.MessageType.LUT) Then
                If Not IsNothing(RespMsg) Then Response.write(RespMsg.Export)
            Else
                Response.write(ClientMsgConfig.Confirmation)
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
    End Sub

End Class
