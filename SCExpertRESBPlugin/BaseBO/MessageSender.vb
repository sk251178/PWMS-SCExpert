Imports System
Imports System.Messaging
Imports System.Threading
Imports System.Data
Imports System.Data.Odbc
Imports Made4Net.DataAccess
Imports Made4Net.DataAccess.Schema
Imports System.Collections.Generic
Imports System.Configuration
Imports Made4Net.Shared.Logging

Public Class MessageSender

    Public Sub Send(ByVal pData As String, ByVal qName As String, ByVal oLogger As LogFile, Optional ByVal pLbl As String = "", Optional ByVal CorrelateId As String = "", _
            Optional ByVal Priority As MessagePriority = MessagePriority.Normal, Optional ByVal pIsResponseQueue As Boolean = False)

        Dim oQ As MessageQueue = Nothing
        Dim qPath As String
        Dim IsRemoteQueue As Boolean = False
        If pIsResponseQueue Then
            qPath = Made4Net.DataAccess.DataInterface.ExecuteScalar("Select RESPONSEQ from MESSAGEQUEUES where QUEUENAME = '" & qName & "'", CONSTANTS.CONNECTION_NAME)
        Else
            qPath = Made4Net.DataAccess.DataInterface.ExecuteScalar("Select QUEUEPATH from MESSAGEQUEUES where QUEUENAME = '" & qName & "'", CONSTANTS.CONNECTION_NAME)
        End If
        If qPath.ToLower.IndexOf("formatname") <> -1 Then
            IsRemoteQueue = True
        End If
        'If Not oLogger Is Nothing Then
        '    oLogger.WriteLine(String.Format("Trying to send message to {0} queue, qpath {1}", qName, qPath))
        'End If

        Try
            If Not IsRemoteQueue Then
                Try
                    If Not MessageQueue.Exists(qPath) Then
                        oQ = MessageQueue.Create(qPath)
                        oQ.SetPermissions("Administrators", MessageQueueAccessRights.FullControl)
                        oQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl)
                    End If
                Catch ex As Exception
                End Try
                oQ = New MessageQueue(qPath)
            Else
                oQ = New MessageQueue(qPath)
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.WriteLine(String.Format("Failed to init queue object. Error details: {0}", ex.ToString))
            End If
        End Try
        Send(pData, oQ, oLogger, pLbl, CorrelateId, Priority)
    End Sub

    Public Overridable Sub Send(ByVal pData As String, ByVal q As MessageQueue, ByVal oLogger As LogFile, Optional ByVal pLbl As String = "", Optional ByVal CorrelateId As String = "", Optional ByVal Priority As MessagePriority = MessagePriority.Normal)
        Dim oMsg As New Message
        Dim transaction As New MessageQueueTransaction()

        Try
            oMsg.AttachSenderId = False
            oMsg.Formatter = New XmlMessageFormatter(New String() {"System.String,mscorlib"})
            'If Not oLogger Is Nothing Then
            '    oLogger.WriteLine(String.Format("Message formatter set."))
            'End If
            transaction.Begin()
            'If Not oLogger Is Nothing Then
            '    oLogger.WriteLine(String.Format("Transaction started."))
            'End If
            oMsg.Priority = Priority
            oMsg.Body = pData
            oMsg.Label = pLbl
            oMsg.CorrelationId = CorrelateId
            oMsg.Recoverable = True
            'If Not oLogger Is Nothing Then
            '    oLogger.WriteLine(String.Format("message obj properties set."))
            'End If
            q.Send(oMsg, transaction)
            'If Not oLogger Is Nothing Then
            '    oLogger.WriteLine(String.Format("Message sent."))
            'End If
            transaction.Commit()
            'If Not oLogger Is Nothing Then
            '    oLogger.WriteLine(String.Format("Transaction comitted."))
            'End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.WriteLine(String.Format("Failed to init queue object. Error details: {0}", ex.ToString))
            End If
            Throw ex
        Finally
            If Not transaction Is Nothing Then transaction.Dispose()
            If Not oMsg Is Nothing Then oMsg.Dispose()
            If Not q Is Nothing Then q.Dispose()
        End Try
    End Sub

End Class