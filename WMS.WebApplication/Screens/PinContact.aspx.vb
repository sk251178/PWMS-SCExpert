Imports WMS.Logic
Partial Public Class PinContact
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim contactID As String
        Dim Latitude As Double
        Dim Londitude As Double
        ''http://localhost/CRTTrunk49/Screens/pincontact.aspx?contact=6&lat=32.19695&lon=34.88
        Try
            contactID = HttpContext.Current.Request.QueryString("contact")
            Latitude = Double.Parse(HttpContext.Current.Request.QueryString("lat"))
            Londitude = Double.Parse(HttpContext.Current.Request.QueryString("lon"))
            AddressMatcherPlugins.PinContactByCoordinates(contactID, Latitude, Londitude)
            HttpContext.Current.Response.Write("done")
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString())
            HttpContext.Current.Response.Write("contactID:" & contactID)
            HttpContext.Current.Response.Write("Latitude:" & Latitude)
            HttpContext.Current.Response.Write("Londitude:" & Londitude)
        End Try
    End Sub
End Class