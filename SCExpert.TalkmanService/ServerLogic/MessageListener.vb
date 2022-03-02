Imports Made4Net.AppSessionManagement
Public Class MessageListener
    Private _conn As Made4Net.Net.TCPIP.TCPConnection
    Private _innerthread As System.Threading.Thread
    Private _receivebuffer As New System.IO.MemoryStream
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
        '_conn.Send("Welcome" & vbCrLf)
        'While _threadenabled
        '    System.Threading.Thread.Sleep(100)
        'End While

    End Sub
    Private Sub ReceiveMessagePart(ByVal _conn As Made4Net.Net.TCPIP.TCPConnection, ByVal count As Integer)
        Try


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
                        Exit For
                    End If
                Else
                    _delimpos = -1
                End If
            Next
        Catch ex As Exception
            _conn.Disconnect()
            _conn.Dispose()
        End Try

    End Sub
    Private Sub ProcessMessage()
        Try
            Made4Net.AppSessionManagement.AppContext.Current = New Made4Net.AppSessionManagement.AppContext(New AppRequest(_receivebuffer, _conn), New Made4Net.AppSessionManagement.BaseAppResponse(_conn.GetNetworkStream))
            Dim msgproc As New MessageProcessor()
            msgproc.ProcessMessage()
            Made4Net.AppSessionManagement.AppContext.Current.Release()
            Made4Net.AppSessionManagement.AppContext.Current = Nothing
        Catch ex As Exception
        Finally
            _conn.Disconnect()
            _conn.Dispose()
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
