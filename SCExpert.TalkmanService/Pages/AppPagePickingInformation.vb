Imports WMS.Logic
Imports Made4Net.AppSessionManagement

Public Class AppPagePickingInformation
    Inherits AppPageProcessor

    Private Enum ResponseCode
        NoError
        [Error]
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Picking Information request according to task: " & _msg(0)("Task ID").FieldValue)
        End If
        Dim _tsk As WMS.Logic.Task = New WMS.Logic.Task(_msg(0)("Task ID").FieldValue)
        If _tsk.ASSIGNED AndAlso _tsk.USERID.Equals(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName) Then
            _responseCode = ResponseCode.NoError
            Me.FillRecordsFromView(String.Format("task='{0}'", _msg(0)("Task ID").FieldValue), oLogger)
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Task Is not assigned to user...")
            End If
            _responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
        End If
        Return _resp
    End Function

End Class




