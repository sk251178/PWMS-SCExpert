Imports System.Web

Partial Public Class CheckDistance
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetDistance()

    End Sub


    Protected Sub GetDistance()
        ''http://maps.googleapis.com/maps/api/distancematrix/xml?origins=Latvia,Jurmala&destinations=5%20D%C4%81rza%20iela,R%C4%ABga,Latvia
        Dim Origin As String = "Dārza iela 5, Riga, LV-1007, Latvia"
        Dim Destination As String = "Turaidas iela 2, Riga, LV-1039, Latvia"

        ''Origin = Server.UrlEncode(HttpContext.Current.Request("origin"))
        ''Destination = Server.UrlEncode(HttpContext.Current.Request("destination"))

        Dim status As String
        Dim distance, duration As Double
        status = GoogleGeocoding.getGoogleDitance(Origin, Destination, "", "", distance, duration)
        Response.Write("distance: " & distance & "<br>")
        Response.Write("duration: " & duration & "<br>")
        Response.Write("status: " & status & "<br>")

    End Sub

    Protected Sub GetMatrixDistance()
        Dim resarr As New ArrayList()

        Dim oOrigins As New ArrayList()
        ''Berlin+Germany|riga+Latvia|Jurmala+Latvia
        oOrigins.Add("Berlin+Germany")
        oOrigins.Add("Riga+Latvia")
        oOrigins.Add("Jurmala+Latvia")

        ''dfdfdfdf|Jurmala+Latvia|Dresden+Germany
        Dim oDestinations As New ArrayList()
        oDestinations.Add("dfdfdfdf")
        oDestinations.Add("Jurmala+Latvia")
        oDestinations.Add("Kleine+Alexanderstraße+28+10178+Berlin+Germany")
        oDestinations.Add("Theaterplatz+1+01067+Dresden+Germany")

        Dim mode As String = Request("mode")
        Dim language As String = Request("language")

        resarr = GoogleGeocoding.getGoogleDitanceMatrix(oOrigins, oDestinations, mode, language)

        For Each s As String In resarr
            Response.Write(s & "<br>")
        Next
    End Sub

End Class
