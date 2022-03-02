Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Util
Imports System.Web
Imports System.Web.UI
Imports system.xml
Imports system.Text
Imports Made4Net.Shared.Conversion.Convert
Imports System.IO
Imports Made4Net.Schema
Imports Made4Net.Shared.Logging
Imports Made4Net.WebControls
Imports Made4Net.GeoData

Public Class SGLogic
    Public Shared Sub TEShowCompContacts_ShowrContacts(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return
        Dim oPage As System.Web.UI.Page = oTE.Page

        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")

        oMap.Markers.Clear()
        For Each oDR As DataRow In oDT.Rows

            Dim LATITUDE, LONGTITUDE As String
            LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LAT"))
            LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LON"))
            If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
            If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Continue For

            Dim infowindow As String = oDR("infowindow")
            Dim tooltip As String = oDR("tooltip")

            oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}", _
                LATITUDE, LONGTITUDE, tooltip, infowindow))

        Next
        oMap.ShowMarkers("M123", True)

        If Not oTE.SelectedRecordPrimaryKeyValues Is Nothing Then
            Dim contactid As String = oTE.SelectedRecordPrimaryKeyValues("contactid")

            Dim jscript As String
            If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
                jscript = "<script language=""JavaScript"">" & vbCrLf
                jscript &= "function initialize5(){" & vbCrLf
                jscript &= "Tag='" & contactid & "';" & vbCrLf
                jscript &= "}" & vbCrLf
                jscript &= "</script>" & vbCrLf
                oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
            End If
        End If

    End Sub

    Public Shared Sub TERoutes_Showroutes(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return

        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
        Dim sql As String


        Dim colors As New ArrayList
        colors.AddRange(New String() {"#FFCC00", "#FF0000", "#FFCCDD", "#EECCDD", "#CCFF00", "#CCDD00", "#CCEE00", "#DD1122"})
        sql = "select distinct '#'+color_rgb color_rgb from routecolors"
        Dim DTColors As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTColors)

        If DTColors.Rows.Count <> 0 Then
            colors.Clear()
            For Each drcolor As DataRow In DTColors.Rows
                colors.Add(drcolor("color_rgb"))
            Next
        End If



        Dim colorcnt As Integer = 0
        Dim sroutes As String = String.Empty
        For Each oDR As DataRow In oDT.Rows
            Dim RouteID As String = ReplaceDBNull(oDR("routeid"))

            sql = String.Format("select * from vRoutes " & _
                    " where  routeid='{0}'" & _
                    " order by routeid,stopnumber ", RouteID)
            Dim DTvRoutesData As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTvRoutesData)
            If DTvRoutesData.Rows.Count = 0 Then Continue For

            oMap.BuildRoute(DTvRoutesData, oDR("routeid"), _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopShadowImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopShadowImage"), _
            "tooltip", "infowindow", colors(colorcnt), "3", False, True)

            If RouteID <> String.Empty AndAlso Not oMap.Routes.Contains(RouteID) Then
                oMap.Routes.Add(RouteID)
                sroutes += ",'" & RouteID & "'"
            End If


            If colorcnt + 1 < colors.Count - 1 Then
                colorcnt += 1
            Else
                colorcnt = 0
            End If
        Next

        ''show routes from array
        If sroutes.Length > 0 Then
            sql = String.Format("select distinct LATITUDE,LONGTITUDE from vRoutes " & _
                  " where  routeid in ({0})", sroutes.Substring(1))
            Dim DTzoom As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTzoom)
            oMap.ShowRoutes(DTzoom)

        End If



    End Sub

    Public Shared Sub TERoutes_ShowrouteswithUnrouted(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return

        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
        Dim sql As String


        Dim colors As New ArrayList ''{"#FFCC00", "#FF0000", "#FFCCDD", "#EECCDD", "#CCFF00", "#CCDD00", "#CCEE00", "#DD1122"}
        sql = "select distinct '#'+color_rgb color_rgb from routecolors"
        Dim DTColors As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTColors)

        Dim RUNID As String
        Dim ROUTESET As String
        If (oDT.Rows.Count > 0) Then
            RUNID = oDT.Rows(0)("RUNID")
            ROUTESET = oDT.Rows(0)("ROUTESET")
        End If

        For Each drcolor As DataRow In DTColors.Rows
            colors.Add(drcolor("color_rgb"))
        Next



        Dim colorcnt As Integer = 0
        Dim sroutes As String = String.Empty
        For Each oDR As DataRow In oDT.Rows
            Dim RouteID As String = ReplaceDBNull(oDR("routeid"))

            sql = String.Format("select * from vRoutes " & _
                    " where  routeid='{0}'" & _
                    " order by routeid,stopnumber ", RouteID)
            Dim DTvRoutesData As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTvRoutesData)
            oMap.BuildRoute(DTvRoutesData, oDR("routeid"), _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopShadowImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopShadowImage"), _
            "tooltip", "infowindow", colors(colorcnt), "3", False, True)

            If RouteID <> String.Empty AndAlso Not oMap.Routes.Contains(RouteID) Then
                oMap.Routes.Add(RouteID)
                sroutes += ",'" & RouteID & "'"
            End If


            If colorcnt + 1 < colors.Count - 1 Then
                colorcnt += 1
            Else
                colorcnt = 0
            End If
        Next



        ''''''''''''''

        If (oDT.Rows.Count > 0) Then

            sql = String.Format("select * from vShowUnrouted where ROUTINGSET='{0}'", _
                ROUTESET)
            Dim oDTUnrouted As DataTable = New DataTable()
            DataInterface.FillDataset(sql, oDTUnrouted)

            For Each oDRUnrouted As DataRow In oDTUnrouted.Rows

                Dim LATITUDE, LONGTITUDE As String
                LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDRUnrouted.Item("LAT"))
                LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDRUnrouted.Item("LON"))
                If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
                If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Continue For

                Dim infowindow As String = oDRUnrouted("infowindow")
                Dim tooltip As String = oDRUnrouted("tooltip")

                oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}", _
                    LATITUDE, LONGTITUDE, tooltip, infowindow))

            Next
            oMap.ShowMarkers("M12345", True)

        End If
        ''''''''''''''''''''

        ''show routes from array
        sql = String.Format("select distinct LATITUDE,LONGTITUDE from vRoutes " & _
              " where  routeid in ({0}) " & _
              " union select distinct lat LATITUDE,lon LONGTITUDE from vShowUnrouted " & _
              " where routingset='{1}'", sroutes.Substring(1), ROUTESET)
        Dim DTzoom As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTzoom)
        oMap.ShowRoutes(DTzoom)


    End Sub


    Public Shared Sub TEShowRoutingSet_ShowRoutingSet(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return

        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
        Dim sql As String


        Dim colors As New ArrayList ''{"#FFCC00", "#FF0000", "#FFCCDD", "#EECCDD", "#CCFF00", "#CCDD00", "#CCEE00", "#DD1122"}
        sql = "select distinct '#'+color_rgb color_rgb from routecolors"
        Dim DTColors As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTColors)

        For Each drcolor As DataRow In DTColors.Rows
            colors.Add(drcolor("color_rgb"))
        Next

        Dim RoutesArray As New ArrayList()
        For Each oDR As DataRow In oDT.Rows
            Dim SETID As String = ReplaceDBNull(oDR("SETID"))
            Dim RUNID As String = ReplaceDBNull(oDR("RUNID"))

            sql = String.Format("select distinct ROUTEID from ROUTE where ROUTESET='{0}' and RUNID='{1}' order by ROUTEID", SETID, RUNID)
            Dim DTRoutes As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTRoutes)

            For Each oDRroutes As DataRow In DTRoutes.Rows
                RoutesArray.Add(oDRroutes("ROUTEID"))
            Next
        Next

        Dim colorcnt As Integer = 0
        Dim sroutes As String = String.Empty
        For Each RouteID As String In RoutesArray

            sql = String.Format("select * from vRoutes " & _
                    " where  routeid='{0}'  " & _
                    " order by routeid,stopnumber ", RouteID)
            Dim DTvRoutesData As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTvRoutesData)
            oMap.BuildRoute(DTvRoutesData, RouteID, _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopShadowImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopShadowImage"), _
            "tooltip", "infowindow", colors(colorcnt), "3", False, True)

            If RouteID <> String.Empty AndAlso Not oMap.Routes.Contains(RouteID) Then
                oMap.Routes.Add(RouteID)
                sroutes += ",'" & RouteID & "'"
            End If


            If colorcnt + 1 < colors.Count - 1 Then
                colorcnt += 1
            Else
                colorcnt = 0
            End If
        Next

        ''show routes from array
        sql = String.Format("select distinct LATITUDE,LONGTITUDE from vRoutes " & _
              " where  routeid in ({0})", sroutes.Substring(Math.Max(0, 1)))
        Dim DTzoom As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTzoom)
        oMap.ShowRoutes(DTzoom)



    End Sub

    Public Shared Sub TEShowCompContact_RecordSelected(ByVal parameters As ExecutionParams)
        Dim paramTable As Hashtable = parameters.Arguments(0)
        Dim oTE As TableEditor = paramTable("TableEditor")
        If oTE.SelectedRecordPrimaryKeyValues Is Nothing Then Return

        Dim oPage As Page = parameters.Arguments(0)("Page")
        Dim contactid As String = oTE.SelectedRecordPrimaryKeyValues("contactid")

        Dim jscript As String
        If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
            jscript = "<script language=""JavaScript"">" & vbCrLf
            jscript &= "function initialize5(){" & vbCrLf
            jscript &= "Tag='" & contactid & "';" & vbCrLf
            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
        End If

    End Sub


    Public Shared Sub TEShowVehiclePosition_ShowVehiclePosition(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return
        Dim oPage As System.Web.UI.Page = oTE.Page

        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")

        oMap.Markers.Clear()
        For Each oDR As DataRow In oDT.Rows

            Dim LATITUDE, LONGTITUDE As String
            LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LATITUDE"))
            LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LONGITUDE"))
            If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
            If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Continue For

            Dim infowindow As String = oDR("infowindow")
            Dim tooltip As String = oDR("tooltip")

            oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}#" & _
            """" & Made4Net.WebControls.SkinManager.GetImageURL("TruckImage") & """#" & _
            """" & Made4Net.WebControls.SkinManager.GetImageURL("ShadowTruckImage") & """", _
            LATITUDE, LONGTITUDE, tooltip, infowindow))

        Next
        oMap.ShowMarkers("M123", True)

        If Not oTE.SelectedRecordPrimaryKeyValues Is Nothing Then
            Dim VEHICLEID As String = oTE.SelectedRecordPrimaryKeyValues("VEHICLEID")

            Dim jscript As String
            If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
                jscript = "<script language=""JavaScript"">" & vbCrLf
                jscript &= "function initialize5(){" & vbCrLf
                jscript &= "Tag='" & VEHICLEID & "';" & vbCrLf
                jscript &= "}" & vbCrLf
                jscript &= "</script>" & vbCrLf
                oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
            End If
        End If

    End Sub


    Public Shared Sub TEShowRoutingSet_BtnShowClick(ByVal parameters As ExecutionParams)

        Dim oTE As TableEditor = parameters.Arguments(2)
        If oTE.ActionRecordPrimaryKeyValues Is Nothing Then
            Throw New ApplicationException("Select record first.")
            Return
        End If
        Dim SETID As String = oTE.ActionRecordPrimaryKeyValues("SETID")
        Dim RUNID As String = oTE.ActionRecordPrimaryKeyValues("RUNID")

        If SETID <> String.Empty Then
            Dim oPage As System.Web.UI.Page = oTE.Page
            Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
            Dim sql As String


            Dim colors As New ArrayList ''{"#FFCC00", "#FF0000", "#FFCCDD", "#EECCDD", "#CCFF00", "#CCDD00", "#CCEE00", "#DD1122"}
            sql = "select distinct '#'+color_rgb color_rgb from routecolors"
            Dim DTColors As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTColors)

            For Each drcolor As DataRow In DTColors.Rows
                colors.Add(drcolor("color_rgb"))
            Next

            Dim RoutesArray As New ArrayList()

            sql = String.Format("select distinct ROUTEID from ROUTE where ROUTESET='{0}' and RUNID='{1}' order by ROUTEID", SETID, RUNID)
            Dim DTRoutes As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTRoutes)

            For Each oDRroutes As DataRow In DTRoutes.Rows
                RoutesArray.Add(oDRroutes("ROUTEID"))
            Next

            Dim colorcnt As Integer = 0
            Dim sroutes As String = String.Empty
            For Each RouteID As String In RoutesArray

                sql = String.Format("select * from vRoutes " & _
                        " where  routeid='{0}'" & _
                        " order by routeid,stopnumber ", RouteID)
                Dim DTvRoutesData As DataTable = New DataTable()
                DataInterface.FillDataset(sql, DTvRoutesData)
                oMap.BuildRoute(DTvRoutesData, RouteID, _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopShadowImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopImage"), _
            Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopShadowImage"), _
                "tooltip", "infowindow", colors(colorcnt), "3", False, True)

                If RouteID <> String.Empty AndAlso Not oMap.Routes.Contains(RouteID) Then
                    oMap.Routes.Add(RouteID)
                    sroutes += ",'" & RouteID & "'"
                End If


                If colorcnt + 1 < colors.Count - 1 Then
                    colorcnt += 1
                Else
                    colorcnt = 0
                End If
            Next

            ''show routes from array
            sql = String.Format("select distinct LATITUDE,LONGTITUDE from vRoutes " & _
                  " where  routeid in ({0})", sroutes.Substring(Math.Max(0, 1)))
            Dim DTzoom As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTzoom)
            oMap.ShowRoutes(DTzoom)

        End If

     
    End Sub

    Public Shared Sub TEShowCompContact_BtnShowClick(ByVal parameters As ExecutionParams)

        Dim oTE As TableEditor = parameters.Arguments(2)
        If oTE.ActionRecordPrimaryKeyValues Is Nothing Then
            Throw New ApplicationException("Select record first.")
            Return
        End If
        Dim CONTACTID As String = oTE.ActionRecordPrimaryKeyValues("CONTACTID")

        Dim oDT As DataTable = parameters.GetDataTable()
        oDT.Select("CONTACTID='" & CONTACTID & "'")
        If oDT.Rows.Count = 0 Then Exit Sub

        Dim oDR As DataRow = oDT.Rows(0)

        Dim oPage As System.Web.UI.Page = oTE.Page

        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")

        oMap.Markers.Clear()

        Dim LATITUDE, LONGTITUDE As String
        LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LAT"))
        LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LON"))
        If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Exit Sub
        If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Exit Sub

        Dim infowindow As String = oDR("infowindow")
        Dim tooltip As String = oDR("tooltip")

        oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}", _
            LATITUDE, LONGTITUDE, tooltip, infowindow))

        oMap.ShowMarkers("M123", True)


        Dim jscript As String
        If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
            jscript = "<script language=""JavaScript"">" & vbCrLf
            jscript &= "function initialize5(){" & vbCrLf
            jscript &= "Tag='" & CONTACTID & "';" & vbCrLf
            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
        End If

    End Sub
    Public Shared Sub TEShowVehiclePosition_BtnShowClick(ByVal parameters As ExecutionParams)

        Dim oTE As TableEditor = parameters.Arguments(2)
        If oTE.ActionRecordPrimaryKeyValues Is Nothing Then
            Throw New ApplicationException("Select record first.")
            Return
        End If
        Dim VEHICLEID As String = oTE.ActionRecordPrimaryKeyValues("VEHICLEID")
        Dim RUNID As String = oTE.ActionRecordPrimaryKeyValues("RUNID")

        Dim oDT As DataTable = parameters.GetDataTable()
        oDT.Select(String.Format("VEHICLEID='{0}' and RUNID='{1}'", VEHICLEID, RUNID))
        If oDT.Rows.Count = 0 Then Exit Sub

        Dim oDR As DataRow = oDT.Rows(0)

        Dim oPage As System.Web.UI.Page = oTE.Page

        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")

        oMap.Markers.Clear()

        Dim LATITUDE, LONGTITUDE As String
        LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LATITUDE"))
        LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR.Item("LONGITUDE"))
        If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Exit Sub
        If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Exit Sub

        Dim infowindow As String = oDR("infowindow")
        Dim tooltip As String = oDR("tooltip")

        oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}#" & _
        """" & Made4Net.WebControls.SkinManager.GetImageURL("TruckImage") & """#" & _
        """" & Made4Net.WebControls.SkinManager.GetImageURL("ShadowTruckImage") & """", _
        LATITUDE, LONGTITUDE, tooltip, infowindow))

        oMap.ShowMarkers("M123", True)


        Dim jscript As String
        If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
            jscript = "<script language=""JavaScript"">" & vbCrLf
            jscript &= "function initialize5(){" & vbCrLf
            jscript &= "Tag='" & VEHICLEID & "';" & vbCrLf
            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
        End If

    End Sub

    Public Shared Sub TERoute_BtnShowUnRouted(ByVal parameters As ExecutionParams)

        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return

        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")

        Dim sql As String

        Dim oDR As DataRow = oDT.Rows(0)
        Dim RUNID As String = oDR("RUNID")
        Dim ROUTESET As String = oDR("ROUTESET")

        sql = String.Format("select * from vShowUnrouted where ROUTINGSET='{0}'", _
            ROUTESET)
        Dim oDTUnrouted As DataTable = New DataTable()
        DataInterface.FillDataset(sql, oDTUnrouted)

        oMap.Markers.Clear()

        For Each oDRUnrouted As DataRow In oDTUnrouted.Rows

            Dim LATITUDE, LONGTITUDE As String
            LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDRUnrouted.Item("LAT"))
            LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDRUnrouted.Item("LON"))
            If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
            If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Continue For

            Dim infowindow As String = oDRUnrouted("infowindow")
            Dim tooltip As String = oDRUnrouted("tooltip")

            oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}", _
                LATITUDE, LONGTITUDE, tooltip, infowindow))

        Next
        oMap.ShowMarkers("M123", True)


        Dim jscript As String
        If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
            jscript = "<script language=""JavaScript"">" & vbCrLf
            jscript &= "function initialize5(){" & vbCrLf
            jscript &= "Tag='" & RUNID & "';" & vbCrLf
            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
        End If

    End Sub



    Public Shared Sub TEShowTerritory_BtnShowClick(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        If oTE.ActionRecordPrimaryKeyValues Is Nothing Then
            Throw New ApplicationException("Select record first.")
            Return
        End If

        Dim TERRITORYID As String = oTE.ActionRecordPrimaryKeyValues("TERRITORYID")
        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
        Dim sql As String


        oMap.Polygons.Clear()
        Dim cntPolygons As Integer = 0
        Dim colors As New GoogleMap.PolygonColors()
        Dim polygoncolor As System.Drawing.Color = colors.PolygonColors(cntPolygons Mod colors.PolygonColors.Count)

        sql = String.Format("select * from vMAPTERRITORYBOUNDARY " & _
            " where TERRITORYID='{0}'", _
        TERRITORYID)

        Dim DTTerritoryBoundary As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTTerritoryBoundary)

        Dim BoundaryString As String = String.Empty
        Dim cnt As Integer = 1
        ''oMap.Polygons.Add("32,34,A,true#32,34.5,C,true#31,34.5,B,true#31,34,D,true")

        For Each drTerritoryBoundary As DataRow In DTTerritoryBoundary.Rows
            BoundaryString &= _
            drTerritoryBoundary("lat") & ":" & _
            drTerritoryBoundary("lon") & ":" & _
            cnt & ":" & _
            drTerritoryBoundary("pointid") & ":" & _
            "true" & ":" & _
            System.Drawing.ColorTranslator.ToHtml(polygoncolor) & "@"
            cnt += 1

        Next

        If BoundaryString <> String.Empty Then
            oMap.Polygons.Add(BoundaryString)
            cntPolygons += 1
            oMap.ShowPolygon("PolygonfromData", True, True)
        End If

        If Not oTE.SelectedRecordPrimaryKeyValues Is Nothing Then
            Dim jscript As String
            If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript3")) Then
                jscript = "<script language=""JavaScript"">" & vbCrLf
                jscript &= "function initialize3(){" & vbCrLf
                jscript &= "Tag='" & TERRITORYID & "';" & vbCrLf
                jscript &= "}" & vbCrLf
                jscript &= "</script>" & vbCrLf
                oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript3", jscript)
            End If
        End If


    End Sub

    Public Shared Sub TERoute_onLoad(ByVal parameters As ExecutionParams)
        'Dim paramTable As Hashtable = parameters.Arguments(0)
        'Dim oTE As TableEditor = paramTable("TableEditor")

        'Dim sql As String
        'sql = "select top 1 RUNID,ROUTESET from route order by adddate desc"

        'Dim DTData As DataTable = New DataTable()
        'DataInterface.FillDataset(sql, DTData)

        'If DTData.Rows.Count > 0 Then
        '    Dim RUNID As String = DTData.Rows(0)("RUNID")
        '    Dim ROUTESET As String = DTData.Rows(0)("ROUTESET")

        '    oTE.FilterExpression = String.Format("ROUTESET='{0}' and RUNID='{1}'", ROUTESET, RUNID)
        'End If

    End Sub



    Public Shared Sub TERoute_BtnShowClick(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        If oTE.ActionRecordPrimaryKeyValues Is Nothing Then
            Throw New ApplicationException("Select record first.")
            Return
        End If
        Dim ROUTEID As String = oTE.ActionRecordPrimaryKeyValues("ROUTEID")


        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
        Dim sql As String


        Dim colors As New ArrayList ''{"#FFCC00", "#FF0000", "#FFCCDD", "#EECCDD", "#CCFF00", "#CCDD00", "#CCEE00", "#DD1122"}
        sql = "select distinct '#'+color_rgb color_rgb from routecolors"
        Dim DTColors As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTColors)

        For Each drcolor As DataRow In DTColors.Rows
            colors.Add(drcolor("color_rgb"))
        Next

        Dim colorcnt As Integer = 0
        Dim sroutes As String = String.Empty

        sql = String.Format("select * from vRoutes " & _
                " where  routeid='{0}'" & _
                " order by routeid,stopnumber ", ROUTEID)
        Dim DTvRoutesData As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTvRoutesData)
        oMap.BuildRoute(DTvRoutesData, ROUTEID, _
        Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopImage"), _
        Made4Net.WebControls.SkinManager.GetImageURL("CompletedStopShadowImage"), _
        Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopImage"), _
        Made4Net.WebControls.SkinManager.GetImageURL("NonCompletedStopShadowImage"), _
        "tooltip", "infowindow", colors(colorcnt), "3", False, True)

        If ROUTEID <> String.Empty AndAlso Not oMap.Routes.Contains(ROUTEID) Then
            oMap.Routes.Add(ROUTEID)
            sroutes += ",'" & ROUTEID & "'"
        End If

        If colorcnt + 1 < colors.Count - 1 Then
            colorcnt += 1
        Else
            colorcnt = 0
        End If


        ''show vehicle
        Dim vehicleid As String = String.Empty
        If DTvRoutesData.Rows.Count > 0 Then
            vehicleid = ReplaceDBNull(DTvRoutesData.Rows(0)("vehicleid"))
            If vehicleid <> String.Empty Then
                sql = String.Format("select  * from vShowVehicle where vehicleid='{0}' and routeid='{1}' order by runid desc", _
                            vehicleid, ROUTEID)
                Dim oDTvehicle As DataTable = New DataTable()
                DataInterface.FillDataset(sql, oDTvehicle)

                If oDTvehicle.Rows.Count > 0 Then
                    Dim LATITUDE, LONGITUDE As String
                    For Each oDRVehicle As DataRow In oDTvehicle.Rows
                        LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDRVehicle.Item("LATITUDE"))
                        LONGITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDRVehicle.Item("LONGITUDE"))
                        If Not (LONGITUDE Is Nothing) And Not (LATITUDE Is Nothing) And _
                            GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGITUDE)) Then

                            Dim infowindow As String = oDRVehicle("infowindow")
                            Dim tooltip As String = oDRVehicle("tooltip")

                            Dim imagename As String = oDRVehicle("TruckImage")
                            Dim shadowimagename As String = oDRVehicle("ShadowTruckImage")

                            oMap.Markers.Add(String.Format("{0}#{1}#{2}#false#{3}#" & _
                            """" & Made4Net.WebControls.SkinManager.GetImageURL(imagename) & """#" & _
                            """" & Made4Net.WebControls.SkinManager.GetImageURL(shadowimagename) & """", _
                            LATITUDE, LONGITUDE, tooltip, infowindow))
                        End If

                    Next
                    oMap.ShowMarkers("Mv123", True)

                End If

            End If

        End If
        ''


        ''show routes from array
        sql = String.Format("select distinct LATITUDE,LONGTITUDE from vRoutes " & _
              " where  routeid in ({0})" & _
              " UNION select distinct LATITUDE,LONGITUDE LONGTITUDE from VEHICLEPOSITION where  vehicleid='{1}'", _
              sroutes.Substring(Math.Max(0, 1)), vehicleid)
        Dim DTzoom As DataTable = New DataTable()
        DataInterface.FillDataset(sql, DTzoom)
        oMap.ShowRoutes(DTzoom)






    End Sub


#Region "Territory"
    Public Shared Sub TEShowCompTerritoty_RecordSelected(ByVal parameters As ExecutionParams)
        Dim paramTable As Hashtable = parameters.Arguments(0)
        Dim oTE As TableEditor = paramTable("TableEditor")
        If oTE.SelectedRecordPrimaryKeyValues Is Nothing Then Return

        Dim oPage As Page = parameters.Arguments(0)("Page")
        Dim TERRITORYID As String = oTE.SelectedRecordPrimaryKeyValues("TERRITORYID")

        Dim jscript As String
        If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript5")) Then
            jscript = "<script language=""JavaScript"">" & vbCrLf
            jscript &= "function initialize5(){" & vbCrLf
            jscript &= "Tag='" & TERRITORYID & "';" & vbCrLf
            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript5", jscript)
        End If
    End Sub

    Public Shared Sub TEShowTerritory_ShowTerritory(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return

        Dim oPage As System.Web.UI.Page = oTE.Page
        Dim oMap As GoogleMap = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oPage, "GmapTest")
        Dim sql As String


        oMap.Polygons.Clear()
        Dim cntPolygons As Integer = 0
        For Each oDR As DataRow In oDT.Rows
            Dim colors As New GoogleMap.PolygonColors()
            Dim polygoncolor As System.Drawing.Color = colors.PolygonColors(cntPolygons Mod colors.PolygonColors.Count)
            Dim TERRITORYID As String = ReplaceDBNull(oDR("TERRITORYID"))

            sql = String.Format("select * from vMAPTERRITORYBOUNDARY " & _
                " where TERRITORYID='{0}'", _
            TERRITORYID)

            Dim DTTerritoryBoundary As DataTable = New DataTable()
            DataInterface.FillDataset(sql, DTTerritoryBoundary)

            Dim BoundaryString As String = String.Empty
            Dim cnt As Integer = 1
            ''oMap.Polygons.Add("32,34,A,true#32,34.5,C,true#31,34.5,B,true#31,34,D,true")

            For Each drTerritoryBoundary As DataRow In DTTerritoryBoundary.Rows
                BoundaryString &= _
                drTerritoryBoundary("lat") & ":" & _
                drTerritoryBoundary("lon") & ":" & _
                cnt & ":" & _
                drTerritoryBoundary("pointid") & ":" & _
                "true" & ":" & _
                System.Drawing.ColorTranslator.ToHtml(polygoncolor) & "@"
                cnt += 1
            Next
            If BoundaryString <> String.Empty Then
                oMap.Polygons.Add(BoundaryString)
            End If
            cntPolygons += 1
        Next

        oMap.ShowPolygon("PolygonfromData", True, True)

        If Not oTE.SelectedRecordPrimaryKeyValues Is Nothing Then
            Dim TERRITORYID As String = oTE.SelectedRecordPrimaryKeyValues("TERRITORYID")
            Dim jscript As String
            If (Not oPage.ClientScript.IsClientScriptBlockRegistered("clientScript3")) Then
                jscript = "<script language=""JavaScript"">" & vbCrLf
                jscript &= "function initialize3(){" & vbCrLf
                jscript &= "Tag='" & TERRITORYID & "';" & vbCrLf
                jscript &= "}" & vbCrLf
                jscript &= "</script>" & vbCrLf
                oPage.ClientScript.RegisterClientScriptBlock(oPage.GetType(), "clientScript3", jscript)
            End If
        End If

    End Sub
    Public Shared Sub TEShowCompContact_PincontactbyGoogle(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return
        Try
            For Each oDR As DataRow In oDT.Rows
                Dim Address As String = ReplaceDBNull(oDR("ADDRESS"))
                Dim ContactID As String = ReplaceDBNull(oDR("CONTACTID"))
                If Address <> String.Empty Then
                    Dim Message As String = WMS.Logic.AddressMatcherPlugins.PinContact(ContactID, Address)
                    If Message = "Contact Pinned successfully." Then
                        Dim oScreen As WMS.WebCtrls.WebCtrls.Screen = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oTE.Page, "oScreen")
                        oScreen.Notify(Message, StatusLine.StatusLineMessageType.Success)
                    End If

                Else
                    Throw New ApplicationException("Empty Address")
                End If
            Next

        Catch ex As Exception
            Throw New ApplicationException(ex.ToString)

        End Try

    End Sub

    Public Shared Sub TEShowCompContact_Pincontact(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT Is Nothing Then Return

        Try
            Dim oScreen As WMS.WebCtrls.WebCtrls.Screen = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oTE.Page, "oScreen")

            For Each oDR As DataRow In oDT.Rows
                Dim CITY As String = ReplaceDBNull(oDR("CITY"))
                Dim STREET1 As String = ReplaceDBNull(oDR("STREET1"))
                Dim STREET2 As Integer
                STREET2 = ReplaceDBNull(oDR("STREET2"))

                Dim ContactID As String = ReplaceDBNull(oDR("CONTACTID"))

                If CITY <> String.Empty Then
                    Dim oMatchAddress As New WMS.Logic.AddressMatcher()
                    Dim pnt As WMS.Logic.MapPoint = oMatchAddress.MatchAddress(CITY, STREET1, STREET2)
                    Dim SQL As String = String.Format("select top 1 POINTID from mappoints where LATITUDE='{0}' and  LONGITUDE='{1}' ", pnt.LatitudeDec.ToString(), pnt.LongitudeDec.ToString())
                    Dim cntr As Integer = Convert.ToInt32(Made4Net.[Shared].Conversion.Convert.ReplaceDBNull(DataInterface.ExecuteScalar(SQL)))

                    If cntr = 0 Then
                        SQL = "select isnull(max(pointid),0) + 1 from mappoints "
                        cntr = Convert.ToInt32(DataInterface.ExecuteScalar(SQL))
                        SQL = String.Format("INSERT INTO MAPPOINTS([POINTID], [SHORTNAME], [POINTTYPEID], [LATITUDE], [LONGITUDE],[LAT],[LON]) " & _
                            " VALUES('{0}', '{1}', '{4}' , {2}, {3},dbo.InttoDeg({2}),dbo.InttoDeg({3}))", _
                            cntr, "PNT" & cntr, pnt.LatitudeDec, pnt.LongitudeDec, PointType.ContactPin)
                        DataInterface.RunSQL(SQL)

                        Try
                            Dim gpNew As New GeoPoint(cntr, True)
                            GeoPoint.AddGeoPointToNetwork(gpNew, "WebApp", "")
                        Catch ex As Exception
                            oScreen.Notify(ex.ToString, StatusLine.StatusLineMessageType.Success)

                        End Try
                    End If

                    SQL = String.Format("Update contact set pointid = '{0}', EDITDATE=getdate() where contactid = '{1}'", _
                    cntr, ContactID)
                    DataInterface.RunSQL(SQL)
                    oScreen.Notify("Contact pinned successfully.", StatusLine.StatusLineMessageType.Success)

                Else
                    oScreen.Notify("City empty", StatusLine.StatusLineMessageType.Success)
                End If
            Next

        Catch ex As Exception
            Dim oScreen As WMS.WebCtrls.WebCtrls.Screen = Made4Net.WebControls.RecursiveControlFinder.FindControlByID(oTE.Page, "oScreen")
            oScreen.Notify(ex.Message, StatusLine.StatusLineMessageType.Success)

        End Try

    End Sub


    Public Shared Sub TERoute_InsertRoute(ByVal parameters As ExecutionParams)
        Dim dr As DataRow = parameters.GetFirstDataRow()
        Dim sRouteID As String
        sRouteID = dr("ROUTEID")
        If sRouteID = "" Then
            sRouteID = Made4Net.Shared.Util.getNextCounter("ROUTE")
        End If
        Dim oRoute As New WMS.Logic.Route
        Dim oRouteSet As New WMS.Logic.RoutingSet(dr("ROUTESET"), True)
        oRoute.Create(sRouteID, oRouteSet.SetID, ReplaceDBNull(dr("DEPO")), "", oRouteSet.DistributionDate, ReplaceDBNull(dr("STARTPOINT")), ReplaceDBNull(dr("ENDPOINT")), "", ReplaceDBNull(dr("VEHICLETYPE")), "", "", "", ReplaceDBNull(dr("RUNID")), 0, 0, 0, 0, 0, 0, 0, 0, "", WMS.Logic.GetCurrentUser)
        oRoute.Status = Logic.RouteStatus.Assigned
        oRoute.Save(WMS.Logic.GetCurrentUser)

    End Sub

    Public Shared Sub TERoute_ConfirmRunID(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)
        Dim oDT As DataTable = parameters.GetDataTable()
        If oDT.Rows.Count = 0 Then
            Throw New ApplicationException("Select record first.")
            Return
        End If
        Dim ROUTESET As String = oDT.Rows(0)("routeset")
        Dim RUNID As String = oDT.Rows(0)("RUNID") 'oTE.ActionRecordPrimaryKeyValues("RUNID")

        Dim oRoutingSet As New WMS.Logic.RoutingSet(ROUTESET, False)


        Dim sSql As String
        sSql = String.Format("select count(*) from route where status='CONFIRMED' and routeset='{0}' and RUNID='{1}'", ROUTESET, oRoutingSet.ActiveRunID)
        If oRoutingSet.ActiveRunID = String.Empty Or CInt(DataInterface.ExecuteScalar(sSql)) = 0 Then
            oRoutingSet.SetActiveRrunID(RUNID)
        Else
            Throw New ApplicationException("This RoutingSet has CONFIRMED routes")
            Return
        End If
    End Sub

    Public Shared Sub TERoute_CreatedChildControls(ByVal parameters As ExecutionParams)
        Dim paramTable As Hashtable = parameters.Arguments(0)
        Dim oTE As TableEditor = paramTable("TableEditor")
        With oTE
            With .ActionBar
                With .Button("Save")
                    If oTE.CurrentMode = TableEditorMode.Insert Then
                        .ObjectDLL = "WMS.WebApp.dll"
                        .ObjectName = "WMS.WebApp.SGLogic"
                        .MethodName = "TERoute_InsertRoute"
                        .CommandName = "Save"
                    End If
                End With
            End With
        End With


    End Sub
    Public Shared Sub TEShowRouteStatistic_CreatedChildControls(ByVal parameters As ExecutionParams)
        Dim paramTable As Hashtable = parameters.Arguments(0)
        Dim oTE As TableEditor = paramTable("TableEditor")
        Dim oUrlAttr As New Made4Net.Shared.Web.OpenURLAttributes("ScreenGenerator.aspx?sc=ruplreq")
        With oUrlAttr
            .MenuBar = False
            .Location = False
            .PixelHeight = 500
            .PixelWidth = 500
            .Status = False
            .ToolBar = False
            '.Center = True
        End With


        With oTE
            With .ActionBar

                'Dim _exBtn As New ExecutionButton()
                Dim _attr As New ExecutionButtonAttributes()
                With _attr
                    .Name = "ImportReq"
                    .Text = "Import Req"
                    .Behavior = ExecutionButtonBehavior.OpenURL
                    .OpenURLAttributes = oUrlAttr
                    .CausesValidation = False
                End With
                .AddExecButton(_attr)
                With .Button("ImportReq")
                    .CommandName = "ImportReq"
                End With

            End With
        End With


    End Sub
#End Region
#Region "Upload Requirements"
    Public Shared Sub TEReqUpload_CreatedChildControls(ByVal parameters As ExecutionParams)
        Dim paramTable As Hashtable = parameters.Arguments(0)
        Dim oTE As TableEditor = paramTable("TableEditor")
        'oTE.RecordEditor.EnableViewState = False
        Dim oScreen As WMS.WebCtrls.WebCtrls.Screen = CType(oTE.ScreenObject, WMS.WebCtrls.WebCtrls.Screen)
        'oScreen.HideBanner = True
        'oScreen.HideMenu = True
        'oScreen.Hidden = True
        'oTE.Page.EnableViewState = False
        With (oTE)
            With .ActionBar
                '.DisableAllButtons()
                'With .Button("Help")
                '.Visible = False
                'End With
                'With .Button("Save")
                '.Visible = False
                'End With
                .AddExecButton("Upload", "Send")
                With .Button("Upload")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.SGLogic"
                    .MethodName = "TEReqUpload_Upload"
                    .CommandName = "Upload"
                End With

            End With
        End With
    End Sub
    Public Shared Sub TEReqUpload_Upload(ByVal parameters As ExecutionParams)
        Dim oTE As TableEditor = parameters.Arguments(2)

        
        Try


            Dim PolicyName As String = oTE.RecordEditor.Form.Field("POLICYNAME").Value
            Dim DataFile As String = ""
            If HttpContext.Current.Request.Files.Count > 0 AndAlso HttpContext.Current.Request.Files(0).ContentLength > 0 Then
                Dim sr As New System.IO.StreamReader(HttpContext.Current.Request.Files(0).InputStream, System.Text.Encoding.GetEncoding("windows-1255"))
                DataFile = sr.ReadToEnd()
                Dim ws As New Made4Net.Shared.WebServices.WebServiceProxy("http://localhost/SAASWS/SaasRouteReqWS.asmx?WSDL")
                Dim args As Object() = {DataFile, PolicyName}
                oTE.RecordEditor.Form.Field("RESULTS").Value = Made4Net.Shared.Reflection.InvokeMethod(ws.CreateInstance, "CreateRequirements", args)
                parameters.ReturnParams.Message = "Import Finished"
            Else
                parameters.ReturnParams.Message = "Error: File Data not found"
            End If
        Catch ex As Exception
            parameters.ReturnParams.Message = "Error: " & ex.ToString
        End Try
        'oTE.Restart()
        
    End Sub
#End Region
End Class
