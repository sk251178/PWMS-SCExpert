Public Partial Class LogOffSession
    Inherits PWMSBaseLogger

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim currentURL As Uri
        currentURL = Request.Url
        Dim browserClosed As String
        If Not Request.QueryString("closeBrowserWindow") Is Nothing Then
            browserClosed = HttpUtility.ParseQueryString(currentURL.Query).Get("closeBrowserWindow")
            HttpContext.Current.Session().Item("BrowserClose") = browserClosed
        End If
        MobileUtils.Session_End(Application("Made4NetLicensing_ApplicationId"), True)
    End Sub


End Class