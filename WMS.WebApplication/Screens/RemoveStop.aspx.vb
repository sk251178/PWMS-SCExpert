Imports WMS.Logic
Imports Made4Net.DataAccess

Partial Public Class RemoveStop
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RouteID As String
        Dim Stopnumber As Integer
        ''http://localhost/CRTTrunk49/Screens/removestop.aspx?RouteID=R000008543&Stopnumber=1
        Try
            RouteID = HttpContext.Current.Request.QueryString("RouteID")
            Stopnumber = Double.Parse(HttpContext.Current.Request.QueryString("Stopnumber"))

            Dim oRoute As New WMS.Logic.Route(RouteID)
            oRoute.RemoveStop(Stopnumber)
            oRoute = New WMS.Logic.Route(RouteID)
            oRoute.RecalculateArrivalTime(True)
            oRoute.SetRouteParams()

            ''check and update next routes in the same Trip Group
            Dim Message As String = String.Empty
            If Not WMS.Logic.RoutePlanner.checkAndUpdateTripGroupRoutes(oRoute, Nothing) Then
                Message = "Source Trip Group Routes don't change because feasibility reason."
            End If
            HttpContext.Current.Response.Write("Done. " & Message)


        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString())
        End Try

    End Sub

End Class