Imports System.IO
Public Class AppRequest
    Inherits Made4Net.AppSessionManagement.BaseAppRequest
    Private _msg As ClientMessage
    Public Sub New(ByVal RequestStream As Stream, ByVal Conn As Made4Net.Net.TCPIP.TCPConnection)
        MyBase.New(RequestStream)
        Me._remoteAddress = Conn.GetIpAddress
    End Sub
    Public Overrides Function CreateSessionID() As String
        Dim msg As ClientMessage = GetClientMessage()
        Return msg.DeviceID
    End Function

    Public Function GetClientMessage() As ClientMessage
        If IsNothing(_msg) Then
            Dim msg As String = MyBase.GetMessage
            _msg = MessagesInfo.GetMessageInstance(msg)
        End If
        Return _msg
    End Function
End Class
