Imports System.ComponentModel
Imports System.Drawing
Imports System.web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Made4Net.DataAccess
Imports Made4Net.WebControls
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion.Convert

Public Class GoogleMap
    Inherits WebControl
    Implements INamingContainer
    Implements IScreenErrorEvents

#Region "Public properties"
    Public MapTable As Made4Net.WebControls.Table

    Public MapWidth As String = "500"

    Public MapHeight As String = "500"
    Public MapKey As String = "ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA"
    Public CenterLat As String = "37.4419"
    Public CenterLon As String = "-122.1419"
    Public MapZoom As String = "4"
    Public NavigationControlStyle As String = "SMALL"
    Public scaleControl As String = "true"
    Public MapType As String = "ROADMAP"
    Public MapTypeControlStyle As String = "DROPDOWN_MENU"
    Public mapTypeControl As String = "true"
    Public trafficLayer As String = "false"
    Public panControl As String = "true"
    Public streetViewControl As String = "false"
    Public overviewMapControl As String = "false"



    Public MapButtons As String = String.Empty
    Public OSM As String = "true"

    Public Routes As ArrayList = New ArrayList()
    Public Markers As ArrayList = New ArrayList()
    Public Polygons As ArrayList = New ArrayList()


    Public InitCount As Integer = 4
    Public ScriptVersion As String = String.Empty
    Public Language As String = String.Empty
    Public Sensor As String = "false"
    Public maxZoom As String = "19"


#End Region

#Region "Errors events"
    <CLSCompliant(False)> _
    Protected _HasErrors As Boolean
    Public Event StandardSuccessEvent(ByVal sender As Object, ByVal e As StandardSuccessEventArgs) Implements IScreenErrorEvents.StandardSuccessEvent
    Public Event StandardErrorEvent(ByVal sender As Object, ByVal e As StandardErrorEventArgs) Implements IScreenErrorEvents.StandardErrorEvent
    Protected Overridable Sub OnErrorEvent(ByVal e As TableEditorErrorEventArgs)
        RaiseEvent StandardErrorEvent(Me, New StandardErrorEventArgs(e.Exception.Message, e.Exception))
        _HasErrors = True
    End Sub
#End Region

#Region "Method overrides"
    Protected Overrides Sub CreateChildControls()
        Dim Lat As Double
        Try
            Lat = Double.Parse(CenterLat)
        Catch ex As Exception
            Lat = 37.4419
        End Try
        Dim Lon As Double
        Try
            Lon = Double.Parse(CenterLon)
        Catch ex As Exception
            Lon = -122.1419
        End Try

        Dim MWidth As Integer
        Try
            MWidth = Integer.Parse(MapWidth)
        Catch ex As Exception
            MWidth = 40
        End Try

        Dim MHeight As Integer
        Try
            MHeight = Integer.Parse(MapHeight)
        Catch ex As Exception
            MHeight = 40
        End Try

        Dim MZoom As Integer
        Try
            MZoom = Integer.Parse(MapZoom)
        Catch ex As Exception
            MHeight = 4
        End Try

        Dim jscript As String
        If (Not Page.ClientScript.IsClientScriptIncludeRegistered("googleInclude")) Then
            ''&language=he
            ''Dim language As String = Made4Net.Shared.Translation.Translator.CurrentLanguageID

            Dim baseUrl As String = "http://maps.google.com/maps/api/js?"
            Dim addUrl As String
            If Sensor <> String.Empty Then
                addUrl = "sensor=" & Sensor
            Else
                addUrl = "sensor=false"
            End If
            ''addUrl &= "&callback=initialize"


            If ScriptVersion <> String.Empty Then addUrl &= "&v=" & ScriptVersion
            If Language <> String.Empty Then addUrl &= "&language=" & Language
            Page.ClientScript.RegisterClientScriptInclude("googleInclude", baseUrl & addUrl)
        End If

        If (Not Page.ClientScript.IsClientScriptIncludeRegistered("googledragInclude")) Then
            Page.ClientScript.RegisterClientScriptInclude("googledragInclude", "../m4nClientScripts\keydragzoom.js")
        End If

        If (Not Page.ClientScript.IsClientScriptIncludeRegistered("googleKey")) Then
            Page.ClientScript.RegisterClientScriptInclude("googleKey", "http://www.google.com/jsapi?key=" & MapKey)
        End If

        If (Not Page.ClientScript.IsClientScriptIncludeRegistered("m4ninclude")) Then
            Page.ClientScript.RegisterClientScriptInclude("m4ninclude", "../m4nClientScripts/m4nGoglemap.js")
        End If


        If (Not Page.ClientScript.IsClientScriptBlockRegistered("clientScript")) Then
            jscript = vbCrLf & "<script language=""JavaScript"">" & vbCrLf
            jscript &= "var map = null;" & vbCrLf

            jscript &= "var CenterPoint = new google.maps.LatLng(" & Lat.ToString() & ", " & Lon.ToString() & ");" & vbCrLf


            jscript &= "var osmMapType = new google.maps.ImageMapType({ " & _
                 " getTileUrl: function(coord, zoom) { " & _
                 " return ""http://tile.openstreetmap.org/"" + " & _
                 " zoom + ""/"" + coord.x + ""/"" + coord.y + "".png""; " & _
                 " }, " & _
                 " tileSize: new google.maps.Size(256, 256), " & _
                 " isPng: true, " & _
                 " alt: ""Open Street Map"", " & _
                 " name: ""OS Map"", " & _
                 " maxZoom: " & maxZoom & _
                 " }); " & vbCrLf



            jscript &= "function initialize() {" & vbCrLf
            jscript &= "    var myOptions = {" & vbCrLf
            jscript &= "      panControl: " & panControl.ToString() & "," & vbCrLf
            jscript &= "      streetViewControl: " & streetViewControl.ToString() & "," & vbCrLf
            jscript &= "      overviewMapControl: " & overviewMapControl.ToString() & "," & vbCrLf

            jscript &= "      zoom: " & MZoom.ToString() & "," & vbCrLf
            jscript &= "      center: new google.maps.LatLng(" & Lat.ToString() & ", " & Lon.ToString() & ")," & vbCrLf
            jscript &= "      scaleControl: " & scaleControl & "," & vbCrLf
            jscript &= "      mapTypeControl: " & mapTypeControl & "," & vbCrLf


            jscript &= "        mapTypeControlOptions: {" & vbCrLf
            If OSM = "true" Then
                jscript &= "        mapTypeIds: ['OSM', google.maps.MapTypeId.ROADMAP,google.maps.MapTypeId.HYBRID,google.maps.MapTypeId.TERRAIN,google.maps.MapTypeId.SATELLITE]," & vbCrLf
            Else
                jscript &= "        mapTypeIds: [google.maps.MapTypeId.ROADMAP,google.maps.MapTypeId.HYBRID,google.maps.MapTypeId.TERRAIN,google.maps.MapTypeId.SATELLITE]," & vbCrLf
            End If

            jscript &= "mapTypeControlOptions: {style: google.maps.MapTypeControlStyle." & MapTypeControlStyle & "}" & vbCrLf

            ''jscript &= "    style:      google.maps.MapTypeControlStyle." & MapTypeControlStyle & vbCrLf

            jscript &= "    }," & vbCrLf


            jscript &= "      navigationControl: true," & vbCrLf
            jscript &= "      navigationControlOptions: {style: google.maps.NavigationControlStyle." & NavigationControlStyle & "}"

            If MapType <> "OSM" Then
                jscript &= ", " & vbCrLf
                jscript &= "      mapTypeId: google.maps.MapTypeId." & MapType & vbCrLf
            End If


            jscript &= "    }" & vbCrLf
            jscript &= "    map = new google.maps.Map(document.getElementById(""map_canvas"")," & vbCrLf
            jscript &= "                                  myOptions);" & vbCrLf


            If OSM = "true" Then
                jscript &= "map.mapTypes.set('OSM',osmMapType);" & vbCrLf
            End If
            If MapType = "OSM" Then
                jscript &= "map.setMapTypeId('OSM');" & vbCrLf
            End If



            If trafficLayer = "true" Then
                jscript &= "    var trafficLayer = new google.maps.TrafficLayer();trafficLayer.setMap(map);" & vbCrLf
            End If

            jscript &= "    map.enableKeyDragZoom({key: ""shift"",boxStyle: {border: ""1px dashed red"",backgroundColor: ""transparent"",opacity: 1.0 },veilStyle:{backgroundColor: ""grey"",opacity: 0.35,cursor: ""crosshair""}}); " & vbCrLf


            If MapButtons <> String.Empty Then
                Dim ButtonsArr As String() = MapButtons.Split("#")
                Dim t As New Translation.Translator()
                For Each btn As String In ButtonsArr
                    If btn.Split(",").Length = 2 Then
                        Dim pCaption As String = btn.Split(",")(0)
                        Dim LCaption As String = t.Translate(pCaption)
                        Dim pCallbackfunction As String = btn.Split(",")(1)
                        Dim LMessage As String = t.Translate("Click to set the map to")
                        jscript &= getBtnString(pCaption, LCaption, pCallbackfunction, LMessage)
                    End If
                Next
            End If
            'Home,setCenterPoint#Erase,eraseMarkers#

            Dim j As Integer
            For j = 1 To InitCount
                jscript &= "if (String(typeof(initialize" & j.ToString & "))!='undefined') initialize" & j.ToString & "();" & vbCrLf
            Next j


            'jscript &= "        if (String(typeof(initialize2))!='undefined') initialize2();" & vbCrLf
            'jscript &= "        if (String(typeof(initialize3))!='undefined') initialize3();" & vbCrLf
            'jscript &= "        if (String(typeof(initialize4))!='undefined') initialize4();" & vbCrLf
            jscript &= "  }" & vbCrLf




            ''jscript &= "window.attachEvent(""onload"", initialize)" & vbCrLf
            jscript &= "function doLoad(){initialize();}" & vbCrLf
            jscript &= "if(window.addEventListener ){window.addEventListener( ""load"", doLoad, false );}else if ( window.attachEvent ){window.attachEvent( ""onload"", doLoad );} else if ( window.onLoad ) { window.onload = doLoad;}" & vbCrLf

            jscript &= "</script>" & vbCrLf
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript", jscript)


        End If

        MapTable = New Made4Net.WebControls.Table()
        MapTable.ID = "map_canvas_table"

        With MapTable
            .AddRow()
            .AddCell("<div id=""map_canvas"" style=""width: " _
                & MWidth.ToString() & "px; height: " & MHeight.ToString() & "px""></div>")

        End With

        Me.Controls.Add(MapTable)
    End Sub


    Private Function getBtnString(ByVal pCaption As String, _
            ByVal pLCaption As String, _
            ByVal pcallbackfunction As String, _
    ByVal LMessage As String) As String
        Dim res As String
        res = String.Format("var {0}ControlDiv = document.createElement('DIV');var {0}Control = new btnControl({0}ControlDiv, map," & "'{3} {2}'" & ",'{2}',{1});{0}ControlDiv.index = 1;map.controls[google.maps.ControlPosition.TOP_RIGHT].push({0}ControlDiv);" & vbCrLf, _
            pCaption, pcallbackfunction, pLCaption, LMessage)
        Return res
    End Function

#End Region

#Region "Route Methods"
    Private Function getRoundCoord(ByVal d As Double, ByVal zoom As Integer) As Double
        ''Dim r As String = Made4Net.Shared.AppConfig.GetSystemParameter("ROUNDLATLON")
        Dim sql As String = String.Format("select top 1 r from vZoomround where zoom<={0} order by zoom desc", zoom.ToString)
        Dim r As String = ReplaceDBNull(DataInterface.ExecuteScalar(sql), 2)
        Return Math.Round(d, Convert.ToInt32(r))
    End Function

    Public Function SetBoundsScript(ByVal odt As DataTable) As String
        If odt.Rows.Count = 0 Then Return String.Empty
        Dim jscript As String = String.Empty
        ''jscript = "var bounds = new google.maps.LatLngBounds();" & vbCrLf
        For Each oDr As DataRow In odt.Rows
            Dim LATITUDE, LONGTITUDE As String
            LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDr("LATITUDE"))
            LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDr("LONGTITUDE"))
            If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
            If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Continue For
            jscript &= "bounds.extend(new google.maps.LatLng(" & LATITUDE & ", " & LONGTITUDE & "));" & vbCrLf
        Next

        jscript &= "if(!bounds.isEmpty())map.fitBounds(bounds);" & vbCrLf



        Return jscript

    End Function


    Public Shared Function SetBoundsScriptbyCollection(ByVal pLatLonCollection As LatLonCollection) As String
        If pLatLonCollection.PointsArrayList.Count = 0 Then Return String.Empty


        Dim jscript As String = String.Empty

        If pLatLonCollection.PointsArrayList.Count < 2 Then
            jscript = "map.setZoom(" & LatLonCollection.AlonePointZoom & ");" & vbCrLf
            Return jscript
        End If

        For Each oLatlon As LatLon In pLatLonCollection.PointsArrayList
            Dim LATITUDE, LONGTITUDE As String
            LATITUDE = oLatlon.Lat
            LONGTITUDE = oLatlon.Lon
            If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
            If Not GoogleGeocoding.checkLatLng(LATITUDE, LONGTITUDE) Then Continue For
            jscript &= "bounds.extend(new google.maps.LatLng(" & LATITUDE & ", " & LONGTITUDE & "));" & vbCrLf
        Next
        jscript &= "if(!bounds.isEmpty()) map.fitBounds(bounds);" & vbCrLf
        Return jscript
    End Function

    Private Function getZoombyData(ByVal dt As DataTable) As Integer
        Dim oLatLonCollection As New LatLonCollection
        For Each oDR As DataRow In dt.Rows
            Dim LATITUDE, LONGTITUDE As String
            LATITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR("LATITUDE"))
            LONGTITUDE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(oDR("LONGTITUDE"))
            If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For

            Dim dLATITUDE, dLONGTITUDE As Double

            If Double.TryParse(LATITUDE, dLATITUDE) And Double.TryParse(LONGTITUDE, dLONGTITUDE) Then
                If Not GoogleGeocoding.checkLatLng(dLATITUDE, dLONGTITUDE) Then Continue For
                oLatLonCollection.Add(New LatLon(dLATITUDE, dLONGTITUDE))
            End If



        Next
        Dim zoom As Integer = oLatLonCollection.getZoom(Convert.ToInt32(MapHeight), Convert.ToInt32(MapWidth))

        Return zoom
    End Function

    Public Sub ShowRoutes(Optional ByVal dtZoom As DataTable = Nothing)
        Dim jscript As String

        If (Not Page.ClientScript.IsClientScriptBlockRegistered("ShowRoutes")) Then
            jscript = vbCrLf & "<script language=""JavaScript"">" & vbCrLf
            jscript &= "function initialize5() {" & vbCrLf
            For Each RouteID As String In Routes
                jscript &= "  if (String(typeof(getRoute_" & RouteID & "() ))!='undefined') getRoute_" & RouteID & "();" & vbCrLf
            Next

            If Not dtZoom Is Nothing AndAlso dtZoom.Rows.Count > 0 Then
                jscript &= SetBoundsScript(dtZoom)
                'Dim zoom As Integer = getZoombyData(dtZoom)
                'jscript &= "map.setZoom(" & zoom.ToString & ");" & vbCrLf
            End If



            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "ShowRoutes", jscript)
        End If

    End Sub


    Public Sub BuildRoute(ByVal oDT As DataTable, _
    ByVal RouteID As String, _
    ByVal ImageName As String, _
    ByVal ShadowImageName As String, _
    ByVal WhiteImageName As String, _
    ByVal WhiteShadowImageName As String, _
    ByVal ToolTipFileldName As String, _
    ByVal HTMLInfoWindowFieldName As String, _
    ByVal strokeColor As String, _
    ByVal strokeWeight As String, _
    ByVal isRoundCoord As Boolean, _
    ByVal isShowLine As Boolean)
        Try
            If oDT Is Nothing OrElse oDT.Rows.Count = 0 Then Return

            Dim arrayLoc As String = String.Empty
            Dim arrayPolyLine As String = String.Empty

            Dim oLatLonCollection As New LatLonCollection

            Dim LatLonHash As New Hashtable()

            Dim maxSEVERITY As Integer = 0
            Dim LATITUDE, LONGTITUDE As String

            Dim sImage As String = String.Empty

            Dim sImageStart As String = String.Empty
            Dim sImageEnd As String = String.Empty


            Dim sShadowImage As String

            Dim sToolTip As String = String.Empty
            Dim sHTMLInfoWindow As String = String.Empty

            Dim zoom As Integer = getZoombyData(oDT)


            ''''routeinfowindow
            Dim sql As String
            Dim RouteInfoWindowText As String
            'sql = String.Format("select infowindow from vShowRouteInfo where routeid='{0}'", RouteID)
            ' RouteInfoWindowText= DataInterface.ExecuteScalar(sql)

            sql = String.Format("select * from [vShowRouteInfoDetail] where routeid='{0}'", RouteID)

            Dim dtShowRouteInfoDetail As New DataTable
            DataInterface.FillDataset(sql, dtShowRouteInfoDetail)
            If dtShowRouteInfoDetail.Rows.Count > 0 Then
                RouteInfoWindowText &= getHTMLDataFromDataRow(dtShowRouteInfoDetail.Rows(0))
            End If



            Dim RouteRows() As DataRow = oDT.Select(String.Format("ROUTEID='{0}'", RouteID))




            Dim i As Integer = 0
            For Each oDR As DataRow In RouteRows
                LATITUDE = ReplaceDBNull(oDR("LATITUDE"))
                LONGTITUDE = ReplaceDBNull(oDR("LONGTITUDE"))
                Dim stopstatus As String = ReplaceDBNull(oDR("stopstatus")).ToString.ToUpper


                Dim StopImage As String = Made4Net.WebControls.SkinManager.GetImageURL(oDR("StopImage"))
                Dim ShadowStopImage As String = Made4Net.WebControls.SkinManager.GetImageURL(oDR("ShadowStopImage"))

                If (LONGTITUDE Is Nothing) Or (LATITUDE Is Nothing) Then Continue For
                If Not GoogleGeocoding.checkLatLng(Double.Parse(LATITUDE), Double.Parse(LONGTITUDE)) Then Continue For


                If isRoundCoord Then
                    LATITUDE = getRoundCoord(LATITUDE, zoom)
                    LONGTITUDE = getRoundCoord(LONGTITUDE, zoom)
                End If

                Dim oLatLon As New LatLon(Double.Parse(LATITUDE), _
                        Double.Parse(LONGTITUDE))
                If oLatLon.Lon = 0 Or oLatLon.Lat = 0 Then Continue For

                sImage = ImageName
                sShadowImage = ShadowImageName

                sImage = StopImage
                sShadowImage = ShadowStopImage


                'If stopstatus = "COMPLETED" Then
                '    sImage = ImageName
                '    sShadowImage = ShadowImageName
                'Else
                '    sImage = WhiteImageName
                '    sShadowImage = WhiteShadowImageName
                'End If

                sToolTip = ReplaceDBNull(oDR(ToolTipFileldName))
                sHTMLInfoWindow = ReplaceDBNull(oDR(HTMLInfoWindowFieldName))


                'Select Case i
                '    Case 0
                '        If oDR("stopstatus").ToString.ToUpper = "COMPLETED" Then
                '            sImageStart = "../images/dd_start.png"
                '        Else
                '            sImageStart = "../images/dd_white_start.png"
                '        End If
                '        sImage = sImageStart
                '    Case RouteRows.Length - 1
                '        If oDR("stopstatus").ToString.ToUpper = "COMPLETED" Then
                '            sImageEnd = "../images/dd_end.png"
                '        Else
                '            sImageEnd = "../images/dd_white_end.png"
                '        End If
                '        sImage = sImageEnd
                'End Select

                Dim oDataArrayList As New ArrayList()
                ''add round if need 
                Dim key As String = oLatLon.Lat.ToString & "#" & oLatLon.Lon.ToString
                If Not LatLonHash.ContainsKey(key) Then
                    oDataArrayList.Add(oLatLon)
                    oDataArrayList.Add(sToolTip)
                    oDataArrayList.Add(sHTMLInfoWindow)
                    oDataArrayList.Add(sImage)
                    LatLonHash.Add(key, oDataArrayList)
                Else
                    oDataArrayList = LatLonHash(key)
                    oDataArrayList(1) &= sToolTip
                    oDataArrayList(2) &= sHTMLInfoWindow
                    oDataArrayList(3) = sImage
                End If
                arrayPolyLine &= ",new google.maps.LatLng(" & oLatLon.Lat.ToString & "," & oLatLon.Lon.ToString & ")"
                i += 1
            Next


            Dim de As IEnumerator = LatLonHash.GetEnumerator
            i = 1
            While (de.MoveNext())
                Dim oDataArrayList As ArrayList = CType(de.Current.Value, ArrayList)
                Dim oLatLon As LatLon = CType(de.Current.Value(0), LatLon)

                arrayLoc &= ",['" & oDataArrayList(1) & "', " & _
                   oLatLon.Lat.ToString & ", " & _
                   oLatLon.Lon.ToString & ", " & i.ToString() & ",'" & _
                   oDataArrayList(2) & "','" & _
                   oDataArrayList(3) & "']"
                oLatLonCollection.Add(oLatLon)
                i += 1
            End While


            If arrayLoc Is Nothing OrElse arrayLoc.Length = 0 Then Exit Sub
            arrayLoc = "[" & arrayLoc.Substring(1) & "];" & vbCrLf

            If arrayPolyLine.Length > 0 Then arrayPolyLine = "[" & arrayPolyLine.Substring(1) & "];" & vbCrLf


            Dim jscript As String

            If (Not Page.ClientScript.IsClientScriptBlockRegistered(RouteID & "_clientScript")) Then
                jscript = vbCrLf & "<script ID='Route_" & RouteID & "' language=""JavaScript"">" & vbCrLf

                jscript &= "var TargetRouteID='';" & vbCrLf
                jscript &= "var TargetRouteColor='';" & vbCrLf
                jscript &= "var " & RouteID & "marker=new Array();" & vbCrLf
                jscript &= "var " & RouteID & "infowindow=new Array();" & vbCrLf
                jscript &= "var " & RouteID & "flightPlanCoordinates;" & vbCrLf
                jscript &= "var " & RouteID & "flightPath;" & vbCrLf


                jscript &= "function getRoute_" & RouteID & "() {" & vbCrLf
                jscript &= "map.setZoom(" & zoom.ToString & ");" & vbCrLf

                jscript &= "map.setCenter(new google.maps.LatLng(" & _
                    oLatLonCollection.CenterLatLon.Lat.ToString & ", " & _
                    oLatLonCollection.CenterLatLon.Lon.ToString & "), 13);" & vbCrLf

                jscript &= "var " & RouteID & "locations = " & arrayLoc & vbCrLf
                jscript &= "var " & RouteID & "shadow = new google.maps.MarkerImage('" & sShadowImage & _
                    "',new google.maps.Size(22, 20),new google.maps.Point(0,0),new google.maps.Point(0, 20));" & vbCrLf


                jscript &= "var " & RouteID & "shape = {coord: [1, 1, 1, 20, 18, 20, 18 , 1],type: 'poly'};" & vbCrLf

                jscript &= "for (var i = 0; i < " & RouteID & "locations.length; i++) " & vbCrLf
                jscript &= "{var " & RouteID & "trip = " & RouteID & "locations[i];" & vbCrLf

                jscript &= "var " & RouteID & "image = new google.maps.MarkerImage(" & RouteID & "trip[5],new google.maps.Size(12, 20), new google.maps.Point(0,0),new google.maps.Point(0, 20));" & vbCrLf
                jscript &= "var " & RouteID & "myLatLng = new google.maps.LatLng(" & RouteID & "trip[1], " & RouteID & "trip[2]); " & vbCrLf
                jscript &= RouteID & "marker[i] = new google.maps.Marker({position: " & RouteID & "myLatLng,  map: map, draggable: true, shadow: " & RouteID & "shadow, icon: " & RouteID & "image,shape: " & RouteID & "shape,title: " & RouteID & "trip[0],zIndex: i});" & vbCrLf
                jscript &= RouteID & "infowindow[i] = new google.maps.InfoWindow({content: " & RouteID & "trip[4]}); " & vbCrLf

                ''

                jscript &= "google.maps.event.addListener(" & RouteID & "marker[i], 'click', function() {" & vbCrLf

                For Each oDRClear As DataRow In RouteRows
                    Dim RouteIDClear As String = oDRClear("ROUTEID")
                    jscript &= "for(var k=0;k<" & RouteIDClear & "infowindow.length;k++)if(" & RouteIDClear & "infowindow[k]!=null)" & _
                        RouteIDClear & "infowindow[k].close();" & vbCrLf
                Next

                jscript &= RouteID & "infowindow[this.zIndex].open(map, this); " & vbCrLf
                jscript &= "});" & vbCrLf

                If isShowLine Then
                    If arrayPolyLine.Length > 0 Then
                        jscript &= RouteID & "flightPlanCoordinates = " & arrayPolyLine & vbCrLf
                        If strokeColor = String.Empty Then strokeColor = "#FF0000"
                        If strokeWeight = String.Empty Then strokeWeight = "2"

                        jscript &= RouteID & "flightPath = new google.maps.Polyline({path: " & RouteID & "flightPlanCoordinates,strokeColor: '" & _
                            strokeColor & "',strokeOpacity: 1.0,strokeWeight: " & _
                            strokeWeight & "}); "


                        jscript &= "google.maps.event.addListener(" & RouteID & "flightPath, 'click', function(event) " & vbCrLf
                        jscript &= "{var " & RouteID & "flightPathinfowindow = new google.maps.InfoWindow();" & vbCrLf
                        jscript &= RouteID & "flightPathinfowindow.setContent('" & RouteInfoWindowText & "');" & vbCrLf
                        jscript &= RouteID & "flightPathinfowindow.setPosition(event.latLng);" & vbCrLf
                        jscript &= RouteID & "flightPathinfowindow.open(map);" & vbCrLf
                        jscript &= "this.setMap(map);});" & vbCrLf


                        jscript &= "google.maps.event.addListener(" & RouteID & "flightPath, 'rightclick', function(event) {" & _
                                " if (TargetRouteID!=''){eval(TargetRouteID+'flightPath').strokeWeight=3;eval(TargetRouteID+'flightPath').strokeColor=TargetRouteColor;eval(TargetRouteID+'flightPath').setMap(map)};TargetRouteColor=" & RouteID & "flightPath.strokeColor;" & RouteID & "flightPath.strokeWeight=5;" & RouteID & "flightPath.strokeColor='#ff0000';" & RouteID & "flightPath.setMap(map); TargetRouteID='" & RouteID & "';alert('Target RouteID for Move Stop: '+TargetRouteID); " & _
                    "});" & vbCrLf

                        jscript &= RouteID & "flightPath.setMap(map);" & vbCrLf


                    End If
                End If


                jscript &= " }" & vbCrLf

                jscript &= "for (var i = 0; i < " & RouteID & "locations.length; i++) " & vbCrLf
                jscript &= "google.maps.event.addListener(" & RouteID & "marker[i], 'rightclick', function(event) {" & _
                                " if (TargetRouteID!=''){eval(TargetRouteID+'flightPath').strokeWeight=3;eval(TargetRouteID+'flightPath').strokeColor=TargetRouteColor;eval(TargetRouteID+'flightPath').setMap(map)};TargetRouteColor=" & RouteID & "flightPath.strokeColor;" & RouteID & "flightPath.strokeWeight=5;" & RouteID & "flightPath.strokeColor='#ff0000';" & RouteID & "flightPath.setMap(map); TargetRouteID='" & RouteID & "';alert('Target RouteID for Move Stop: '+TargetRouteID); " & _
                    "});" & vbCrLf


                jscript &= "}" & vbCrLf

                jscript &= "</script>" & vbCrLf
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), RouteID & "_clientScript", jscript)
            End If

            'Dim jscripttab As String
            'If (Not Page.ClientScript.IsClientScriptBlockRegistered("clientScripttab")) Then
            '    jscripttab = vbCrLf & "<script language=""JavaScript"">" & vbCrLf
            '    jscripttab &= "function initialize4(){" & vbCrLf
            '    jscripttab &= "var d1; d1 =document.getElementById(""oSG_MainTable___ho"");" & vbCrLf
            '    jscripttab &= "if (d1==null) d1 =document.getElementById(""oSG_MainTable___HO"");" & vbCrLf
            '    jscripttab &= "var d2 =document.getElementById(""map_canvas"");" & vbCrLf
            '    jscripttab &= "var d3 =document.getElementById(""oSG_MAPHome_map_canvas_table"");" & vbCrLf
            '    jscripttab &= "if(d1!=null)d1.style.width=""100%"";" & vbCrLf
            '    jscripttab &= "if(d2!=null)d2.style.width=""100%"";" & vbCrLf
            '    jscripttab &= "if(d3!=null)d3.style.width=""100%"";" & vbCrLf
            '    jscripttab &= "}" & vbCrLf
            '    jscripttab &= "</script>" & vbCrLf
            '    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScripttab", jscripttab)
            'End If

        Catch ex As Exception
            Throw New ApplicationException(ex.ToString())

        End Try



    End Sub


    Public Sub ShowMarkers(ByVal ArrayID As String, _
                ByVal isFit As Boolean)
        Dim jscript As String


        If (Not Page.ClientScript.IsClientScriptBlockRegistered("clientScript4")) Then
            jscript = "<script language=""JavaScript"">" & vbCrLf
            jscript &= " var  markerArray" & ArrayID & " = [];" & vbCrLf
            jscript &= "function initialize4(){" & vbCrLf




            Dim i As Integer = 0
            For Each MarkerData As String In Markers
                Dim ArrayMarkerData As String()
                ArrayMarkerData = MarkerData.Split("#")
                Dim lat As String = ArrayMarkerData(0)
                Dim lon As String = ArrayMarkerData(1)
                Dim title As String = ArrayMarkerData(2).Replace("'", "\'")
                Dim draggable As String = ArrayMarkerData(3)


                If ArrayMarkerData.Length > 5 Then
                    jscript &= "var image" & i & " = new google.maps.MarkerImage(" & ArrayMarkerData(5) & ",new google.maps.Size(32, 32), new google.maps.Point(0,0),new google.maps.Point(0, 32));" & vbCrLf
                    jscript &= "var imageshadow" & i & " = new google.maps.MarkerImage(" & ArrayMarkerData(6) & ",new google.maps.Size(59, 32), new google.maps.Point(0,0),new google.maps.Point(0, 32));" & vbCrLf
                Else
                    jscript &= "var image" & i & " = new google.maps.MarkerImage(""../images/marker.png"",new google.maps.Size(20, 34), new google.maps.Point(0,0),new google.maps.Point(0, 20));" & vbCrLf
                    jscript &= "var imageshadow" & i & " = new google.maps.MarkerImage(""../images/markershadow.png"",new google.maps.Size(37, 34), new google.maps.Point(0,0),new google.maps.Point(0, 20));" & vbCrLf

                End If

                jscript &= "markerArray" & ArrayID & "[" & i & _
                    "]=new google.maps.Marker({map: map,position: new google.maps.LatLng(" & lat & _
                        ", " & lon & "), draggable: " & draggable & ",shadow:  imageshadow" & i & ", icon: image" & i & ",title: '" & title & _
                            "'});" & vbCrLf
                If ArrayMarkerData.Length > 4 Then
                    Dim infowindowText As String = ArrayMarkerData(4).Replace("'", "\'")
                    jscript &= " var infowindow" & i & " = new google.maps.InfoWindow({content: '" & infowindowText & "'});" & vbCrLf
                    jscript &= " google.maps.event.addListener(markerArray" & ArrayID & "[" & i & _
                    "], 'click', function() {infowindow" & i & ".open(map, this);}); " & vbCrLf
                End If

                jscript &= " markerArray" & ArrayID & "[" & i & "].setMap(map);" & vbCrLf

                If isFit Then
                    jscript &= "bounds.extend(new google.maps.LatLng(" & lat & ", " & lon & "));" & vbCrLf
                End If

                i += 1
            Next
            If isFit Then
                If Markers.Count = 1 Then
                    jscript &= "map.setCenter(new google.maps.LatLng(" & _
                        Markers(0).Split("#")(0) & ", " & _
                        Markers(0).Split("#")(1) & "));" & vbCrLf
                    jscript &= "map.setZoom(" & LatLonCollection.AlonePointZoom & ");" & vbCrLf
                Else
                    jscript &= "if(!bounds.isEmpty()) map.fitBounds(bounds);" & vbCrLf
                    ''jscript &= "if(!bounds.isEmpty()) map.panToBounds(bounds);" & vbCrLf

                End If

            End If




            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript4", jscript)
        End If


    End Sub


    Public Sub ShowPolygon(ByVal PolygonID As String, _
            ByVal isMarkers As Boolean, _
            ByVal isFit As Boolean)
        Dim jscript As String


        If (Not Page.ClientScript.IsClientScriptBlockRegistered("clientScript4")) Then

            jscript = "<script language=""JavaScript"">" & vbCrLf

            jscript &= "function initialize4(){" & vbCrLf

            Dim i As Integer = 0
            Dim j As Integer = 0
            Dim FillColor As String

            For Each Polygon As String In Polygons

                If isMarkers Then jscript &= " var markerArray" & PolygonID & "_" & j.ToString & " = [];" & vbCrLf
                jscript &= " var PolygonArray" & PolygonID & "_" & j.ToString & " = new google.maps.MVCArray();" & vbCrLf
                For Each PolygonData As String In Polygon.Split("@")
                    ''jscript &= "var aaaa_" & j.ToString & "='" & PolygonData & "';" & vbCrLf

                    If PolygonData = String.Empty Then Continue For

                    Dim lat As String = PolygonData.Split(":")(0)
                    Dim lon As String = PolygonData.Split(":")(1)
                    jscript &= "PolygonArray" & PolygonID & "_" & j.ToString & ".push(new google.maps.LatLng(" & lat & ", " & lon & "));" & vbCrLf

                    If isMarkers Then
                        Dim title As String = PolygonData.Split(":")(2)
                        title = title.Replace("'", "\'")
                        jscript &= "markerArray" & PolygonID & "_" & j.ToString & "[" & i & _
                                            "]=new google.maps.Marker({map: map,position: new google.maps.LatLng(" & lat & _
                                                ", " & lon & "), draggable: false ,title: '" & title & _
                                                    "'}); markerArray" & PolygonID & "_" & j.ToString & "[" & i & "].setMap(map);" & vbCrLf
                    End If
                    If isFit Then
                        jscript &= "bounds.extend(new google.maps.LatLng(" & lat & ", " & lon & "));" & vbCrLf
                    End If
                    FillColor = PolygonData.Split(":")(5)
                    i += 1
                Next
                jscript &= "if (PolygonArray" & PolygonID & "_" & j.ToString & ".length>0) DrawPreparedPoligon(map,PolygonArray" & _
                PolygonID & "_" & j.ToString & ",'" & FillColor & "', 0.1,'#FF0000',0.5,2);" & vbCrLf

                j += 1
            Next

            If isFit Then
                jscript &= "if(!bounds.isEmpty())map.fitBounds(bounds);" & vbCrLf
            End If


            jscript &= "}" & vbCrLf
            jscript &= "</script>" & vbCrLf
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "clientScript4", jscript)
        End If


    End Sub


    Public Shared Function getHTMLDataFromDataRow(ByVal odr As DataRow) As String
        Dim res As String = String.Empty
        For Each dc As DataColumn In odr.Table.Columns
            res &= "<tr><td><b>" & _
            TranslationManager.Translate(dc.ColumnName).ToString().Replace("'", "\'") & ":</b></td>" & _
            "<td>" & ReplaceDBNull(odr(dc.ColumnName)).ToString().Replace("'", "\'") & "</td></tr>"
        Next
        res = "<table>" & res & "</table>"
        Return res
    End Function

#End Region

    Public Class PolygonColors
        <CLSCompliant(False)> Protected _Colors As New ArrayList()
        ''        Public Shared PolygonColorsArray As String() = New String() {"#78FFF000", "#78e0e0e0", "#78ff0000", "#78ffff00", "#7800ff00", "#7800ffff", "#780000ff", "#78ff00ff", "#78804000", "#78ff8040", "#78808000", "#788080ff", "#78ff0080"}
        Sub New()
            SkinManager.LoadColorsPalette(_Colors, "PolygonColors")
        End Sub
        Public Property PolygonColors() As ArrayList
            Get
                Return _Colors
            End Get
            Set(ByVal Value As ArrayList)
                _Colors = Value
            End Set
        End Property
    End Class

End Class

