Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports Made4Net.GeoData

Partial Public Class SaveTerritory
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TERRITORYID As String
        Dim pathCoordinates As String


        ''http://localhost/CRTTrunk49/Screens/pincontact.aspx?t=Center1&p=
        Try
            TERRITORYID = HttpContext.Current.Request.QueryString("t")
            pathCoordinates = HttpContext.Current.Request.QueryString("p")
            pathCoordinates = pathCoordinates.Substring(0, pathCoordinates.Length - 2)

            HttpContext.Current.Response.Write("TERRITORYID:" & TERRITORYID)
            HttpContext.Current.Response.Write("pathCoordinates:" & pathCoordinates)

            Dim pathArray As String() = pathCoordinates.Split("Z")
            Dim sql As String = String.Format("delete MAPTERRITORYBOUNDARY where TERRITORYID={0}", _
                Made4Net.Shared.Util.FormatField(TERRITORYID))
            DataInterface.RunSQL(sql)

            Dim cnt As Integer = 1
            For Each oLatLon As String In pathArray
                Dim LATITUDE As Double = Double.Parse(oLatLon.Split(",")(0))
                Dim LONGTITUDE As Double = Double.Parse(oLatLon.Split(",")(1))
                If Not GoogleGeocoding.checkLatLng(LATITUDE, LONGTITUDE) Then Continue For

                Dim PointID As String = AddressMatcherPlugins.setPoint(GeoPoint.Deg2Int(LATITUDE), _
                    GeoPoint.Deg2Int(LONGTITUDE), _
                    LATITUDE, LONGTITUDE)
                If PointID = String.Empty Then
                    HttpContext.Current.Response.Write("*** Undefined PointID")
                    Continue For
                End If

                sql = String.Format("insert MAPTERRITORYBOUNDARY " & _
                " (TERRITORYID,POINTID,POINTNUM)" & _
                " values({0},{1},{2})", _
                    Made4Net.Shared.Util.FormatField(TERRITORYID), _
                    Made4Net.Shared.Util.FormatField(PointID), _
                    cnt.ToString)
                DataInterface.RunSQL(sql)
                cnt += 1
            Next


            HttpContext.Current.Response.Write("done")
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.Message.ToString())
        End Try
    End Sub


End Class