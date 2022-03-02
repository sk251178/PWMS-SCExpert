Public Class AppPageUserLogout
    Inherits AppPageProcessor

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Logout For user " & Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
        End If
        Made4Net.Shared.Authentication.User.Logout()
        'Session.Abandon()
        If Not oLogger Is Nothing Then
            oLogger.Write("User Logout Finished successfully.")
        End If
        Return Nothing
    End Function

End Class