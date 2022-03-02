
Partial Public Class MoveStop
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim RouteID As String
        Dim TargetRouteID As String
        Dim Stopnumber As Integer
        ''http://localhost/CRTTrunk49/Screens/movestop.aspx?RouteID=R000008543&Stopnumber=1&TargetrouteID=R000008543
        Try
            RouteID = HttpContext.Current.Request.QueryString("RouteID")
            Stopnumber = Double.Parse(HttpContext.Current.Request.QueryString("Stopnumber"))
            TargetRouteID = HttpContext.Current.Request.QueryString("TargetRouteID")

            Dim oRoute As New WMS.Logic.Route(RouteID)
            If RouteID = TargetRouteID Then
                Dim Distcosto As Double = oRoute.ResequenceRoute()
            Else
                oRoute.MoveStop(Stopnumber, TargetRouteID)

                oRoute.RemoveStop(Stopnumber)
                oRoute = New WMS.Logic.Route(RouteID)
                If oRoute.Stops.Count = 0 Then
                    oRoute.SetTotalDistance(0)
                    oRoute.SetTotalTime(0)
                    oRoute.SetTotalVolume(0)
                    oRoute.SetTotalWeight(0)
                    oRoute.SetRouteCost(0)

                    oRoute.SetStatus(Logic.RouteStatus.Canceled, Date.Now(), WMS.Logic.Common.GetCurrentUser())
                Else
                    oRoute.RecalculateArrivalTime(True)
                    oRoute.SetRouteParams()
                End If

            End If

            ''check and update next routes in the same Trip Group
            Dim Message As String = String.Empty

            If Not WMS.Logic.RoutePlanner.checkAndUpdateTripGroupRoutes(oRoute, Nothing) Then
                Message = "Source Trip Group Routes don't change because feasibility reason. "
            End If

            Dim oTargetRoute As WMS.Logic.Route = New WMS.Logic.Route(TargetRouteID)
            Dim TargetMessage As String = String.Empty
            If Not WMS.Logic.RoutePlanner.checkAndUpdateTripGroupRoutes(oTargetRoute, Nothing) Then
                TargetMessage = "Source Trip Group Routes don't change because feasibility reason. "
            End If
            HttpContext.Current.Response.Write("Done. " & Message & TargetMessage)


        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString())
        End Try


    End Sub

End Class