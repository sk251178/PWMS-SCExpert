Imports System
Imports System.Messaging
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports System.Data
Imports Made4Net.Shared

Public Class TaskManager
    Inherits Made4Net.Shared.QHandler

    Public Sub New()
        MyBase.New("TaskManager", False)
    End Sub

    Protected Overrides Sub ProcessQueue(ByVal qMsg As System.Messaging.Message, ByVal qSender As Made4Net.Shared.QMsgSender, ByVal e As System.Messaging.PeekCompletedEventArgs)
        Try
            Dim oLogger As WMS.Logic.LogHandler
            '   Dim useLogs As String = System.Configuration.ConfigurationManager.AppSettings.Item("UseLogs")
            Dim useLogs As String = Made4Net.Shared.Util.GetAppConfigNameValue(Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceSection,
                                                    Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceUseLogs)
            If useLogs = "1" Then
                ' Dim dirpath As String = System.Configuration.ConfigurationManager.AppSettings.Item("ServiceLogDirectory")
                Dim dirpath As String = Made4Net.DataAccess.Util.GetInstancePath()
                dirpath += "\\" + Made4Net.Shared.ConfigurationSettingsConsts.TaskManagerServiceLogDirectory
                oLogger = New LogHandler(dirpath, qSender.Values("UserId") & DateTime.Now.ToString("_yyyyMMdd_HHmmss_fff_") & New Random().Next() & ".txt")
            End If
            Dim oTM As New WMS.Logic.TaskManager
            Dim oTask As Task
            Dim userId As String = qSender.Values("UserId")
            Dim Simulate As Integer = Convert.ToInt32(qSender.Values("SIMULATE"))
            Dim IsGetTaskFromUser As Boolean = False
            If Not qSender.Values("IsGetTaskFromUser") Is Nothing Then
                If qSender.Values("IsGetTaskFromUser") = "1" Then
                    IsGetTaskFromUser = True
                End If
            End If
            oTask = oTM.RequestTask(userId, Simulate, True, oLogger, IsGetTaskFromUser)
            If Not oTask Is Nothing Then
                ResponseOnSuccess(oTask.TASK, qMsg)
            Else
                ResponseOnSuccess("", qMsg)
            End If
        Catch ex As Exception
            ResponseOnSuccess("", qMsg)
            'Added for Sending the Message to deadletterQueue Start
            Throw
            'Added for Sending the Message to deadletterQueue End

        End Try
    End Sub

    Private Sub ResponseOnSuccess(ByVal pTaskId As String, ByVal qMsg As System.Messaging.Message)
        Try
            If Not qMsg.ResponseQueue Is Nothing Then
                Dim newmq As MessageQueue = qMsg.ResponseQueue
                Dim qs As Made4Net.Shared.QMsgSender = New Made4Net.Shared.QMsgSender
                qs.Add("TASKID", pTaskId)
                qs.Send(newmq, pTaskId, qMsg.Id, System.Messaging.MessagePriority.Normal)
            End If
        Catch ex As Exception
        End Try
    End Sub

End Class