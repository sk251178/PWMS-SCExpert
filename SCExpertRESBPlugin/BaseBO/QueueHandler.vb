Imports System
Imports System.Messaging
Imports System.Threading
Imports System.Data
Imports System.Data.Odbc
Imports Made4Net.DataAccess
Imports Made4Net.DataAccess.Schema
Imports System.Collections.Generic
Imports System.Configuration

Public Class QueueHandler
    Inherits MessageQueue

#Region "Members"

    Protected mPeekCompletedEventArgs As PeekCompletedEventArgs
    Protected mMessageQueuePath As String
    Protected mMessageQueueName As String
    Protected mEnableLogging As Boolean
    Protected mQueueLogPath As Boolean
    Protected mMessage As Message
    Protected oMessageProcessor As IMessageProcessor

#End Region

#Region "Properties"

    Public ReadOnly Property MessageQueuePath() As String
        Get
            Return mMessageQueuePath
        End Get
    End Property

    Public ReadOnly Property MessageQueueName() As String
        Get
            Return mMessageQueueName
        End Get
    End Property

#End Region

#Region "Ctor"

    Public Sub New(ByVal pMessageQueueName As String, ByVal pMessageProcessor As IMessageProcessor)
        mMessageQueueName = pMessageQueueName
        If mMessageQueueName Is Nothing OrElse mMessageQueueName = String.Empty Then
            Throw New ApplicationException("Message queue name is not defined.")
        End If

        Dim ci As Made4Net.DataAccess.ODBCConnectionInfo = New Made4Net.DataAccess.ODBCConnectionInfo(CONSTANTS.CONNECTION_NAME)
        Dim sql As String = String.Format("Select QUEUEPath from MESSAGEQUEUES Where QUEUENAME = '{0}'", Made4Net.Shared.Strings.PSQ(pMessageQueueName))
        Dim d As New Made4Net.DataAccess.Data(sql, ci)
        Dim dt As DataTable = d.CreateDataTable(False, False)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException(String.Format("Could not find message queue '{0}' in MESSAGEQUEUES table.", pMessageQueueName))
        End If

        mMessageQueuePath = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)(0))
        If mMessageQueuePath Is Nothing OrElse mMessageQueuePath = String.Empty Then
            Throw New ApplicationException(String.Format("Message queue path for queue '{0}' cannot be blank.", mMessageQueuePath))
        End If

        Me.Path = mMessageQueuePath
        Try
            'Convert the network path to local path
            Dim LocalPath As String
            Dim localPathArr() As String
            If mMessageQueuePath.ToLower.StartsWith("formatname:direct") Then
                localPathArr = mMessageQueuePath.Split("\")
                LocalPath = "." & "\" & localPathArr(1) & "\" & localPathArr(2)
            Else
                LocalPath = mMessageQueuePath
            End If
            'Put code in try block to handle the remote queue exception
            If Not MessageQueue.Exists(LocalPath) Then
                MessageQueue.Create(LocalPath)
                Me.SetPermissions("Administrators", MessageQueueAccessRights.FullControl)
                Me.SetPermissions("Everyone", MessageQueueAccessRights.FullControl)
            Else
            End If
        Catch ex As Exception
        End Try
        oMessageProcessor = pMessageProcessor
    End Sub

#End Region

#Region "Methods"

    Public Sub StartQueue()
        AddHandler PeekCompleted, AddressOf _q_PeekCompleted
        BeginPeek()
    End Sub

    Public Sub StopQueue()
        RemoveHandler PeekCompleted, AddressOf _q_PeekCompleted
        Close()
    End Sub

    Public Function ClearQueue() As Int32
        Dim msgCntr As Int32 = GetAllMessages().Length
        Dim i As Int32
        For i = 0 To msgCntr - 1
            Receive()
        Next
        Return msgCntr
    End Function

    Private Sub _q_PeekCompleted(ByVal sender As Object, ByVal e As PeekCompletedEventArgs)
        Try
            If IsNothing(sender) Then
                Return
            End If
            Dim mq As MessageQueue = CType(sender, MessageQueue)
            Dim m As Message = mq.EndPeek(e.AsyncResult)
            oMessageProcessor.ProcessQueue(m, e)
            Receive()
            BeginPeek()
        Catch ex As Made4Net.Shared.M4NQHandleException
            If ex.ShouldReceiveMessage Then
                Receive()
            End If
            BeginPeek()
        Catch ex As Exception
            Receive()
            BeginPeek()
        End Try
    End Sub

#End Region

#Region "IDisposable"

    Public Overridable Overloads Sub Dispose()
        RemoveHandler PeekCompleted, AddressOf _q_PeekCompleted
        MyBase.Dispose()
    End Sub

#End Region

End Class
