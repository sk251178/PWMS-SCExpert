Imports SCExpertTalkmanPlugin

Public Class TalkmanServer


    '' Dim a As New Made4Net.Net.TCPIP.TCPServer(System.Configuration.ConfigurationManager.AppSettings.Get("ServerPort"))
    Dim ServerPort As String = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceSection,
                                        Made4Net.Shared.ConfigurationSettingsConsts.TalkManServiceServerPort)  '' PWMS-817
    Dim a As New Made4Net.Net.TCPIP.TCPServer(ServerPort, "TalkmanService")
    Dim connections As New Hashtable
    Dim connectionthreads As New Hashtable
    Dim conncount As Integer = 0
    Public Sub Start()
        MessagesInfo.CreateConfig()
        Made4Net.AppSessionManagement.AppManager.SetListener(New AppListener)
        a.SetConnectionHandler(AddressOf Me.AcceptConnection)
        Dim tr = New Threading.Thread(AddressOf a.StartServer)
        a.StartServer()
    End Sub

    Public Sub [Stop]()
        a.StopServer()
    End Sub

    Private Sub AcceptConnection(ByVal client As Made4Net.Net.TCPIP.TCPConnection)
        'MessageBox.Show("Connected")
        Dim _ml As New MessageListener(client)
        conncount = conncount + 1
        connections.Add(conncount.ToString, _ml)
        '_ml.StartListening()
        Dim _innerthread As System.Threading.Thread = New System.Threading.Thread(AddressOf _ml.StartListenThread)
        connectionthreads.Add(conncount.ToString, _innerthread)
        _innerthread.Start()
        'client.ProcessConnection()
        'client.Send("Pizdec")
        'client.Disconnect()
    End Sub

End Class