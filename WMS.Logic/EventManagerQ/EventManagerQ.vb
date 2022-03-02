Imports System.Messaging
Imports Made4Net.Shared
Imports System.Collections.Specialized


<Serializable()> Public Class EventManagerQ
    Inherits Made4Net.Shared.QMsgSender
    Implements IEventManagerQ

    Private Shared eventManagerQFactory As IEventManagerQFactory = Nothing
    Public Overloads Shared Property Factory() As IEventManagerQFactory
        Get
            If eventManagerQFactory Is Nothing Then
                eventManagerQFactory = New EventManagerQFactory()
            End If
            Return eventManagerQFactory
        End Get
        Set(ByVal value As IEventManagerQFactory)
            eventManagerQFactory = value
        End Set
    End Property

    Public Overrides Sub Add(key As String, value As String) Implements IEventManagerQ.Add
        MyBase.Add(key, value)
    End Sub


    Public Shadows Sub Send(ByVal pLbl As String) Implements IEventManagerQ.Send
        Dim oQ As MessageQueue
        Dim oMsg As New Message
        Dim qPath As String
        Dim IsRemoteQueue As Boolean = False

        Dim _Logger As System.IO.StreamWriter
        Try


            Try
                _Logger = PrepareLogFile()
                If IsHttpContext() Then
                    WriteToLog(_Logger, System.Web.HttpContext.Current.Request.HttpMethod + " " + System.Web.HttpContext.Current.Request.RawUrl)
                End If
                WriteToLog(_Logger, " Send EventManagerQ Request for qName:EventManager ,Label: " & pLbl)

            Catch ex As Exception

            End Try

            'qPath = Made4Net.DataAccess.DataInterface.ExecuteScalar _
            '("Select QUEUEPATH from MESSAGEQUEUES where QUEUENAME = 'EventManager'", Made4Net.Schema.CONNECTION_NAME)
            qPath = Made4Net.Shared.M4NMessageQueueCollection.GetQueue("EventManager")
            WriteToLog(_Logger, " Queue Path found :" & qPath)
            If qPath.ToLower.IndexOf("formatname") <> -1 Then
                IsRemoteQueue = True
            End If
            If Not IsRemoteQueue Then
                If Not MessageQueue.Exists(qPath) Then
                    oQ = MessageQueue.Create(qPath)
                    oQ.SetPermissions("Administrators", MessageQueueAccessRights.FullControl)
                    oQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl)
                Else
                    oQ = New MessageQueue(qPath)
                End If
            Else
                oQ = New MessageQueue(qPath)
            End If
            WriteToLog(_Logger, " Current Process Infrastructure WAREHOUSE=" & Made4Net.Shared.Warehouse.CurrentWarehouse)
            WriteToLog(_Logger, " Current Process Logic WAREHOUSE=" & WMS.Logic.Warehouse.CurrentWarehouse)
            If _cs("WAREHOUSE") Is Nothing Or _cs("WAREHOUSE") = "" Then
                WriteToLog(_Logger, " Warehouse Nothing or Empty , adding WAREHOUSE=" & Warehouse.CurrentWarehouse)
                Add("WAREHOUSE", Warehouse.CurrentWarehouse)
            Else
                WriteToLog(_Logger, " Message already contains WAREHOUSE=" & _cs("WAREHOUSE"))
            End If
            WriteToLog(_Logger, PrintMessageContent)
            If _cs("WAREHOUSE") Is Nothing Or _cs("WAREHOUSE") = "" Then
                WriteToLog(_Logger, " Warehouse is Still Empty. Exit Sender")
                Throw New Exception("Msmq Message Warehouse field cannot be empty ")
            End If

            oMsg.BodyStream = Serialize()
            If Not pLbl Is Nothing And pLbl <> "" Then
                oMsg.Label = pLbl
            End If
            oQ.Send(oMsg)
        Catch ex As Exception
            'Added for RWMS-1453 and 1451
            Try
                'Write the error to event log   
                WriteToLog(_Logger, " Error :" & ex.Message & vbCrLf & ex.StackTrace)
                Dim evtLog As New Made4Net.General.EventLogger("WMS.Logic-EventManagerQ", Made4Net.General.LogLevel.Information, False)
                Dim oLog As New Made4Net.General.Log("Error while sending message to EventManagerQ with pLbl = " & pLbl & ". Error Details: " & ex.ToString())
                evtLog.Log(oLog)
            Catch innerex As Exception
            End Try
            'Ended for RWMS-1453 and 1451
            Throw ex
        Finally
            CloseLogFile(_Logger)
            oMsg.Dispose()
            oQ.Dispose()
        End Try


    End Sub

    'Added for RWMS-2540 Start
    Public Shadows Sub Send(ByVal pLbl As String, ByVal pWarehouseId As String) Implements IEventManagerQ.Send
        Dim oQ As MessageQueue
        Dim oMsg As New Message
        Dim qPath As String
        Dim IsRemoteQueue As Boolean = False

        Dim _Logger As System.IO.StreamWriter
        Try


            Try
                _Logger = PrepareLogFile()
                If IsHttpContext() Then
                    WriteToLog(_Logger, System.Web.HttpContext.Current.Request.HttpMethod + " " + System.Web.HttpContext.Current.Request.RawUrl)
                End If
                WriteToLog(_Logger, " Send EventManagerQ Request for qName:EventManager ,Label: " & pLbl)

            Catch ex As Exception

            End Try

            'qPath = Made4Net.DataAccess.DataInterface.ExecuteScalar _
            '("Select QUEUEPATH from MESSAGEQUEUES where QUEUENAME = 'EventManager'", Made4Net.Schema.CONNECTION_NAME)
            qPath = Made4Net.Shared.M4NMessageQueueCollection.GetQueue("EventManager")
            WriteToLog(_Logger, " Queue Path found :" & qPath)
            If qPath.ToLower.IndexOf("formatname") <> -1 Then
                IsRemoteQueue = True
            End If
            If Not IsRemoteQueue Then
                If Not MessageQueue.Exists(qPath) Then
                    oQ = MessageQueue.Create(qPath)
                    oQ.SetPermissions("Administrators", MessageQueueAccessRights.FullControl)
                    oQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl)
                Else
                    oQ = New MessageQueue(qPath)
                End If
            Else
                oQ = New MessageQueue(qPath)
            End If
            WriteToLog(_Logger, " Current Process Infrastructure WAREHOUSE=" & Made4Net.Shared.Warehouse.CurrentWarehouse)
            WriteToLog(_Logger, " Current Process Logic WAREHOUSE=" & WMS.Logic.Warehouse.CurrentWarehouse)
            If _cs("WAREHOUSE") Is Nothing Or _cs("WAREHOUSE") = "" Then
                WriteToLog(_Logger, " Warehouse Nothing or Empty , adding WAREHOUSE=" & Warehouse.CurrentWarehouse)
                Add("WAREHOUSE", pWarehouseId)
            Else
                WriteToLog(_Logger, " Message already contains WAREHOUSE=" & _cs("WAREHOUSE"))
            End If
            WriteToLog(_Logger, PrintMessageContent)
            If _cs("WAREHOUSE") Is Nothing Or _cs("WAREHOUSE") = "" Then
                WriteToLog(_Logger, " Warehouse is Still Empty. Exit Sender")
                Throw New Exception("Msmq Message Warehouse field cannot be empty ")
            End If

            oMsg.BodyStream = Serialize()
            If Not pLbl Is Nothing And pLbl <> "" Then
                oMsg.Label = pLbl
            End If
            oQ.Send(oMsg)
        Catch ex As Exception
            'Added for RWMS-1453 and 1451
            Try
                'Write the error to event log   
                WriteToLog(_Logger, " Error :" & ex.Message & vbCrLf & ex.StackTrace)
                Dim evtLog As New Made4Net.General.EventLogger("WMS.Logic-EventManagerQ", Made4Net.General.LogLevel.Information, False)
                Dim oLog As New Made4Net.General.Log("Error while sending message to EventManagerQ with pLbl = " & pLbl & ". Error Details: " & ex.ToString())
                evtLog.Log(oLog)
            Catch innerex As Exception
            End Try
            'Ended for RWMS-1453 and 1451
            Throw ex
        Finally
            CloseLogFile(_Logger)
            oMsg.Dispose()
            oQ.Dispose()
        End Try


    End Sub
    'Added for RWMS-2540 End

End Class
