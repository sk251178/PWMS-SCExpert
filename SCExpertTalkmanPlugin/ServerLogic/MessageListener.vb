Imports Made4Net.AppSessionManagement
Public Class MessageListener
    Private _conn As Made4Net.Net.TCPIP.TCPConnection
    Private _innerthread As System.Threading.Thread
    Private _receivebuffer As System.IO.MemoryStream
    Private _threadenabled As Boolean = True
    Private _delimpos As Int32 = -1
    Private _delimbytes() As Byte

    Public Sub New(ByVal conn As Made4Net.Net.TCPIP.TCPConnection)
        _conn = conn
    End Sub

    Public Sub StartListening()
        _innerthread = New System.Threading.Thread(AddressOf StartListenThread)
        _innerthread.Start()
    End Sub

    Public Sub StartListenThread()
        _delimbytes = GetMessageDelimiterBytes()
        _conn.GetMessageHandler = New Made4Net.Net.TCPIP.TCPConnection.MESSAGE_HANDLER(AddressOf ReceiveMessagePart)
        _conn.ProcessConnection()
    End Sub

    Private Sub ReceiveMessagePart(ByVal _conn As Made4Net.Net.TCPIP.TCPConnection, ByVal count As Integer)
        Try
            If IsNothing(_receivebuffer) Then
                _receivebuffer = New System.IO.MemoryStream
            End If
            Dim buffer() As Byte = _conn.GetRawBuffer
            For i As Integer = 0 To count - 1
                _receivebuffer.WriteByte(buffer(i))
                If buffer(i) = 10 Or buffer(i) = 13 Then
                    '_conn.Send("Received : " & _receivebuffer.ToString & vbCrLf)
                    '_threadenabled = False
                    If buffer(i) = _delimbytes(_delimpos + 1) Then
                        _delimpos = _delimpos + 1
                    Else
                        _delimpos = -1
                    End If
                    If _delimpos = _delimbytes.Length - 1 Then
                        ProcessMessage()
                        System.Array.Clear(buffer, 0, buffer.Length - 1)
                        buffer = Nothing
                        _receivebuffer.Close()
                        _receivebuffer.Dispose()
                        _receivebuffer = Nothing
                        Exit For
                    End If
                Else
                    _delimpos = -1
                End If
            Next
        Catch ex As Exception
            ' If Made4Net.Shared.AppConfig.Get("UseLogs", 0) = "1" Then
            If Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceUseLogs) = "1" Then
                Try
                    'Dim oLogger As New WMS.Logic.LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory"), "Msg_Listener_ReceiveMessagePart_" & "_" & DateTime.Now.ToString("MMddyyyy_HHmmss") & "_" & "_" & New Random().Next() & ".txt")
                    'oLogger.WriteTimeStamp = True
                    Dim oLogger As WMS.Logic.LogHandler = CreateLogger("Msg_Listener_ReceiveMessagePart_" & "_" & DateTime.Now.ToString("MMddyyyy_HHmmss") & "_" & "_" & New Random().Next() & ".txt")
                    oLogger.StartWrite()
                    oLogger.Write("Exception on ReceiveMessagePart execution:")
                    oLogger.Write(ex.ToString())
                    oLogger.Write("Received input:")
                    _receivebuffer.Seek(0, System.IO.SeekOrigin.Begin)
                    oLogger.Write(New System.IO.StreamReader(_receivebuffer).ReadToEnd())
                    oLogger.EndWrite()
                Catch
                End Try
            End If
            _conn.Disconnect()
            _conn.Dispose()
        End Try
    End Sub

    Private Function CreateLogger(ByVal LogFileName As String) As WMS.Logic.LogHandler
        Dim strdirpath As String = Made4Net.DataAccess.Util.GetInstancePath()
        strdirpath += "\" + Made4Net.Shared.ConfigurationSettingsConsts.TalkmanServiceLogDirectory
        Dim oLogger As WMS.Logic.LogHandler = New WMS.Logic.LogHandler(strdirpath, LogFileName)
        oLogger.WriteTimeStamp = True
        Return oLogger
    End Function

    Private Sub ProcessMessage()
        Try
            Made4Net.AppSessionManagement.AppContext.Current = New Made4Net.AppSessionManagement.AppContext(New AppRequest(_receivebuffer, _conn), New Made4Net.AppSessionManagement.BaseAppResponse(_conn.GetNetworkStream))
            Dim msgproc As New MessageProcessor()
            msgproc.ProcessMessage()
            msgproc = Nothing
            Made4Net.AppSessionManagement.AppContext.Current.Release()
            Made4Net.AppSessionManagement.AppContext.Current = Nothing
        Catch ex As Exception
            ' If Made4Net.Shared.AppConfig.Get("UseLogs", 0) = "1" Then
            If Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceUseLogs) = "1" Then
                Try
                    'Dim oLogger As New WMS.Logic.LogHandler(Made4Net.Shared.AppConfig.Get("ServiceLogDirectory"), "Msg_Listener_ProcessMessage_" & "_" & DateTime.Now.ToString("MMddyyyy_HHmmss") & "_" & "_" & New Random().Next() & ".txt")
                    'oLogger.WriteTimeStamp = True
                    Dim oLogger As WMS.Logic.LogHandler = CreateLogger("Msg_Listener_ProcessMessage_" & "_" & DateTime.Now.ToString("MMddyyyy_HHmmss") & "_" & "_" & New Random().Next() & ".txt")
                    oLogger.StartWrite()
                    oLogger.Write("Exception on ProcessMessage execution:")
                    oLogger.Write(ex.ToString())
                    oLogger.Write("Received input:")
                    _receivebuffer.Seek(0, System.IO.SeekOrigin.Begin)
                    oLogger.Write(New System.IO.StreamReader(_receivebuffer).ReadToEnd())
                    oLogger.EndWrite()
                Catch
                End Try
            End If
        Finally
            _conn.GetMessageHandler = Nothing
            _conn.Disconnect()
            _conn.Dispose()
            _conn = Nothing
            System.GC.Collect()
        End Try
    End Sub

    Private Function GetMessageDelimiterBytes() As Byte()
        'Dim delim As String = "CRLFCRLF"
        Dim delim As String = MessagesInfo.EOF
        Dim delimbit As String = delim.Replace("CR", "0").Replace("LF", "1")
        Dim delimbytes() As Byte
        ReDim delimbytes(delimbit.Length - 1)
        For i As Int32 = 0 To delimbit.Length - 1
            If delimbit.Chars(i).Equals("0"c) Then
                delimbytes(i) = 13
            Else
                delimbytes(i) = 10
            End If
        Next
        Return delimbytes
    End Function

End Class