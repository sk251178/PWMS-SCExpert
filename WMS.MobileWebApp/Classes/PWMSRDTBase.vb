Imports Made4Net.Shared.Web

Public Class PWMSRDTBase
    Inherits PWMSBaseLogger


    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        '... add custom logic here ...
        DoPageSessionValidation()
        'Be sure to call the base class's OnLoad method!
        MyBase.OnLoad(e)
    End Sub

    Private Sub DoPageSessionValidation()
        Try
            If Not HttpContext.Current.Session Is Nothing Then
                If (HttpContext.Current.Session.Count = 0) AndAlso Not (HttpContext.Current.Request.Url.AbsolutePath.EndsWith("login.aspx", StringComparison.InvariantCultureIgnoreCase)) Then
                    HttpContext.Current.Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class
