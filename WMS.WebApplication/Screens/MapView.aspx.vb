Public Partial Class MapView
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("Made4NetLicensing_IsLicensedUsers") = True
        Session("Made4NetLoggedInUserName") = "admin"
        Session("Made4NetLoggedInUserAddress") = "127.0.0.1"
        Made4Net.DataAccess.DataInterface.ConnectionName = Made4Net.Shared.AppConfig.Get("Conn_Name", "Default")
        ''Response.Write(DataInterface.ExecuteScalar("select getdate() "))

    End Sub

End Class