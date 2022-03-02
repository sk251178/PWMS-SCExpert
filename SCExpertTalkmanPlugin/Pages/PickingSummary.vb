Imports WMS.Logic
Imports Made4Net.AppSessionManagement

Public Class PickingSummary
    Inherits AppPageProcessor

    Private Enum ResponseCode
        NoError
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Try
            If Not oLogger Is Nothing Then
                oLogger.Write("Processing Picking Information Summary request according to task: " & _msg(0)("Task ID").FieldValue)
            End If
            PrintMessageContent(oLogger)
            If Not ValidateUserLoggedIn() Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("User is not logged in. Please sign in again."))
                End If
                Me._responseCode = ResponseCode.NoUserLoggedIn
                Me._responseText = "User is not logged in"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            Dim sPicklist As String = ""
            Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(_msg(0)("Task ID").FieldValue)
            If Not oLogger Is Nothing Then
                oLogger.Write("Task object loaded.")
            End If
            If oTask.TASKTYPE.Equals(WMS.Lib.TASKTYPE.PARALLELPICKING, StringComparison.OrdinalIgnoreCase) Then
                sPicklist = oTask.ParallelPicklist
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Trying to get summary details for parallel pick list {0}...", oTask.ParallelPicklist))
                End If
                _responseCode = ResponseCode.NoError
                _responseText = ""
                Me.FillRecordsFromView(String.Format("task='{0}'", _msg(0)("Task ID").FieldValue), oLogger)
            Else
                sPicklist = oTask.Picklist
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Trying to get summary details for pick list {0}...", oTask.Picklist))
                End If
                _responseCode = ResponseCode.NoError
                _responseText = ""
                Me.FillRecordsFromView(String.Format("task='{0}'", _msg(0)("Task ID").FieldValue), oLogger)
            End If
            
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.ToString)
            End If
            _responseCode = ResponseCode.Error
            _responseText = ex.Message
            Me.FillSingleRecord(oLogger)
        End Try
        Return _resp
    End Function

End Class
