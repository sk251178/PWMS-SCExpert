Imports WMS.Logic
Imports WMS.Lib
Imports Made4Net.AppSessionManagement
Public Class AppPageTaskAssignment
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Assigned
        NotAssigned
        NoUserLoggedIn = 99
    End Enum
    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
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
        Dim userid As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Task Request for user: " & userid)
        End If
        Dim _tsk As WMS.Logic.Task = Nothing
        If _msg(0)("Task ID").FieldValue.Equals("") Then
            Dim _tm As New WMS.Logic.TaskManager
            _tsk = WMS.Logic.TaskManager.getUserAssignedTask(userid, oLogger)
            If _tsk Is Nothing Then
                If _msg(0)("Task Type").FieldValue.Equals("") Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Task Id is blank, trying to get task from task manager...")
                    End If
                    _tsk = _tm.GetTaskFromTMService(userid, False, oLogger)
                    If _tsk Is Nothing Then
                        oLogger.Write("Task manager did not return a task. Trying one more time...")
                        _tsk = WMS.Logic.TaskManager.getUserAssignedTask(userid, oLogger)
                    End If
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Task Id is blank, trying to get task according to tasktype: " & _msg(0)("Task Type").FieldValue)
                    End If
                    _tsk = _tm.RequestTask(userid, _msg(0)("Task Type").FieldValue, oLogger)
                End If
            End If
            If Not _tsk Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Task Id: " & _msg(0)("Task Type").FieldValue & " Assigned successfully...")
                End If
                Me.FillRecordsFromView(String.Format("task='{0}'", _tsk.TASK), oLogger)
            Else
                _responseCode = ResponseCode.NotAssigned
                If Not oLogger Is Nothing Then
                    oLogger.Write("No Available task found...")
                End If
                Me.FillSingleRecord(oLogger)
            End If
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Task Id In Message: " & _msg(0)("Task ID").FieldValue & ". Will try to assign user...")
            End If
            If Not WMS.Logic.Task.Exists(_msg(0)("Task ID").FieldValue) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Task Does not exist!")
                End If
            Else
                _tsk = New WMS.Logic.Task(_msg(0)("Task ID").FieldValue)
                If _tsk.ASSIGNED Then
                    If _tsk.USERID.Equals(userid) Then
                        Me.FillRecordsFromView(String.Format("task='{0}'", _tsk.TASK), oLogger)
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Task Already assigned for another user...")
                        End If
                        _responseCode = ResponseCode.NotAssigned
                        Me.FillSingleRecord(oLogger)
                    End If
                Else
                    _tsk.AssignUser(userid)
                    If Not oLogger Is Nothing Then
                        oLogger.Write("User assigned to task...")
                    End If
                    Me.FillRecordsFromView(String.Format("task='{0}'", _tsk.TASK), oLogger)
                End If
            End If
        End If
        If Not IsNothing(_tsk) Then
            If _tsk.TASKTYPE = WMS.Lib.TASKTYPE.PARALLELPICKING Then

            End If
        End If
        Return _resp
    End Function

End Class