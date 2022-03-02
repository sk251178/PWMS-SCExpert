Imports WMS.Logic
Imports Made4Net.AppSessionManagement
Public Class AppPageTaskRelease
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Released
        NotReleased
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Task release for task: " & _msg(0)("Task ID").FieldValue)
        End If
        Dim _tsk As WMS.Logic.Task = New WMS.Logic.Task(_msg(0)("Task ID").FieldValue)
        If _tsk.ASSIGNED AndAlso _tsk.USERID.Equals(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName) Then
            Dim _tm As New TaskManager(_tsk.TASK)
            _tm.ExitTask()
            If Not oLogger Is Nothing Then
                oLogger.Write("Task Released from user...")
            End If
            _responseCode = ResponseCode.Released
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Task Is not assigned or assigned to a different user rather than the request user...")
            End If
            _responseCode = ResponseCode.NotReleased
        End If
        Me.FillSingleRecord(oLogger)
        Return _resp
    End Function

End Class
