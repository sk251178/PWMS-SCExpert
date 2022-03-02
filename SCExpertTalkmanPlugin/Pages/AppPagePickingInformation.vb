Imports WMS.Logic
Imports Made4Net.AppSessionManagement

Public Class AppPagePickingInformation
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
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Picking Information request according to task: " & _msg(0)("Task ID").FieldValue)
        End If
        Dim _tsk As WMS.Logic.Task = New WMS.Logic.Task(_msg(0)("Task ID").FieldValue)
        If _tsk.ASSIGNED AndAlso _tsk.USERID.Equals(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, StringComparison.OrdinalIgnoreCase) Then
            'Begin RWMS-512
            ClearWeightCapture(_msg(0)("Task ID").FieldValue, _msg(0)("Device ID").FieldValue, oLogger)
            'End RWMS-512
            _responseCode = ResponseCode.NoError
            Me.FillRecordsFromView(String.Format("task='{0}'", _msg(0)("Task ID").FieldValue), oLogger)
            Session("UserAssignedTask") = _tsk
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Task Is not assigned to user...")
            End If
            _responseCode = ResponseCode.Error
            _responseCode = String.Format("Task Is not assigned to user")
            Me.FillSingleRecord(oLogger)
        End If
        Return _resp
    End Function

    'Begin RWMS-512
    Private Sub ClearWeightCapture(ByVal strTaskID As String, ByVal strDeviceID As String, ByVal oLogger As WMS.Logic.LogHandler)

        Try
            If Not oLogger Is Nothing Then
                oLogger.Write("Proceeding to clear the case weights for the device..." + strDeviceID)
            End If
            Dim sql As String = "DELETE FROM PS_VOICEWEIGHTCAPTURE WHERE VOICEDEVICEID = '{0}'"
            sql = String.Format(sql, strDeviceID)
            If Not oLogger Is Nothing Then
                oLogger.Write("SQL to clear voice capture details for device....... " + sql)
            End If
            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If Not oLogger Is Nothing Then
                oLogger.Write("Caseweight records deleted for device....... " + strDeviceID)
            End If

            'If Not oLogger Is Nothing Then
            '    oLogger.Write("Proceeding to fetch the pickinginfo for task ID..." + strTaskID)
            'End If
            'sql = "select PICKLIST from Talkman_PickingInfo where TASK = '{0}'"
            'sql = String.Format(sql, strTaskID)
            'If Not oLogger Is Nothing Then
            '    oLogger.Write("SQL to fetch the pickinginfo..." + sql)
            'End If
            'Dim dtPickingTask As New DataTable
            'Made4Net.DataAccess.DataInterface.FillDataset(sql, dtPickingTask)
            'If dtPickingTask.Rows.Count > 0 Then

            '    Dim strPickListID As String = dtPickingTask.Rows(0).Item("PICKLIST")
            '    If Not oLogger Is Nothing Then
            '        oLogger.Write("Proceeding to clear the case weights for picklist..." + strPickListID)
            '    End If

            '    sql = "DELETE FROM PS_VOICEWEIGHTCAPTURE WHERE PICKLIST = '{0}'"
            '    sql = String.Format(sql, strPickListID)
            '    If Not oLogger Is Nothing Then
            '        oLogger.Write("SQL to clear voice capture details for picklist....... " + sql)
            '    End If
            '    Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            '    If Not oLogger Is Nothing Then
            '        oLogger.Write("Received pickjob. Caseweight records deleted for picklist....... " + strPickListID)
            '    End If
            'Else
            '    If Not oLogger Is Nothing Then
            '        oLogger.Write("No pick records found in the task.......")
            '    End If
            'End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error clearing the case weights: " & ex.ToString())
            End If
        End Try

    End Sub
    'End RWMS-512

End Class




