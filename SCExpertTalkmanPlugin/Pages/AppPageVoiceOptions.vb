Imports WMS.Logic
Imports WMS.Lib
Imports Made4Net.Shared

Public Class AppPageVoiceOptions
    Inherits AppPageProcessor

    Private Enum ResponseCode
        OK
        UnknownError
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        PrintMessageContent(oLogger)
        _messageDate = Now.ToString("yyyy-MM-dd HH:mm:ss")

        If Not oLogger Is Nothing Then
            oLogger.Write("Processing voice options request...")
        End If

        Try
            _responseCode = ResponseCode.OK
            _responseText = ""
            Me.FillSingleRecord(oLogger)
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Values in response message:"))
                oLogger.Write(String.Format("PasswordRequired: {0}", Util.GetSystemParameterValue("TalkMan_RequirePWD")))
                oLogger.Write(String.Format("GoalTime: {0}", Util.GetSystemParameterValue("ShowGoalTimeOnTaskAssignment")))
                oLogger.Write(String.Format("Performance: {0}", Util.GetSystemParameterValue("ShowPerformanceOnTaskComplete")))
                oLogger.Write(String.Format("AllowCountOnPicking: {0}", Util.GetSystemParameterValue("TalkMan_AllowCountOnPicking")))
                oLogger.Write(String.Format("AllowCloseContainer: {0}", Util.GetSystemParameterValue("TalkMan_AllowCloseContainer")))
                oLogger.Write(String.Format("MHELogin: {0}", Util.GetSystemParameterValue("TalkMan_MHELogin")))
                oLogger.Write(String.Format("Login location (required): {0}", Util.GetSystemParameterValue("TalkMan_LoginLocation")))
                oLogger.Write(String.Format("Capture handling unit id on picking: {0}", Util.GetSystemParameterValue("TalkMan_HUIdRequired")))
            End If

            _resp(0)("PasswordRequired").FieldValue = Util.GetSystemParameterValue("TalkMan_RequirePWD")
            _resp(0)("GoalTime").FieldValue = Util.GetSystemParameterValue("ShowGoalTimeOnTaskAssignment")
            _resp(0)("Performance").FieldValue = Util.GetSystemParameterValue("ShowPerformanceOnTaskComplete")
            _resp(0)("AllowCountOnPicking").FieldValue = Util.GetSystemParameterValue("TalkMan_AllowCountOnPicking")
            _resp(0)("AllowCloseContainer").FieldValue = Util.GetSystemParameterValue("TalkMan_AllowCloseContainer")
            _resp(0)("MHELogin").FieldValue = Util.GetSystemParameterValue("TalkMan_MHELogin")
            _resp(0)("LoginLocation").FieldValue = Util.GetSystemParameterValue("TalkMan_LoginLocationReq")
            _resp(0)("HUIdRequired").FieldValue = Util.GetSystemParameterValue("TalkMan_HUIdRequired")
        Catch ex As Made4Net.Shared.M4NException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Description
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.GetErrMessage(0))
            End If
            Me.FillSingleRecord(oLogger)
        Catch ex As ApplicationException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
        Catch ex As Exception
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
        End Try
        Return _resp
    End Function

End Class
