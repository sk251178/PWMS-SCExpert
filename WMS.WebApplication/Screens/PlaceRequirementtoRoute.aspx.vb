Imports made4net.DataAccess

Partial Public Class PlaceRequirementtoRoute
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TargetRouteID As String
        Dim ReqID As String
        ''http://localhost/CRTTrunk49/Screens/PlaceRequirementtoRoute.aspx?ReqID=XXX&TargetrouteID=R000008543
        Try
            ReqID = HttpContext.Current.Request.QueryString("ReqID")
            TargetRouteID = HttpContext.Current.Request.QueryString("TargetRouteID")

            Dim oTargetRoute As New WMS.Logic.Route(TargetRouteID)

            ''load oreq
            Dim sql As String = String.Format("select * from vROUTINGREQUIREMENTSPlaceUnrouted where ReqID='{0}'", _
                ReqID)
            Dim dt As DataTable = New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                Throw New ApplicationException("REQUIREMENT Not Found")
                Exit Sub
            End If

            dr = dt.Rows(0)
            Dim oReq As New WMS.Logic.RoutingRequirement(dr)
            oTargetRoute.PlaceRequirement(oReq, TargetRouteID)

            oTargetRoute = New WMS.Logic.Route(TargetRouteID)
            oTargetRoute.RecalculateArrivalTime(True)
            oTargetRoute.SetRouteParams()

            ''check and update next routes in the same Trip Group
            Dim Message As String = String.Empty
            If Not WMS.Logic.RoutePlanner.checkAndUpdateTripGroupRoutes(oTargetRoute, Nothing) Then
                Message = "Target Trip Group Routes don't change because feasibility reason. "
            End If
            HttpContext.Current.Response.Write("Done. " & Message)

            ''''''''''''''''''''

        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString())
        End Try
    End Sub

End Class