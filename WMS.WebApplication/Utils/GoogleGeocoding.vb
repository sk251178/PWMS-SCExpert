Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web
Imports Made4Net.GeoData
Imports System.Xml.XPath
Imports System.Xml

Public Class GoogleGeocoding

    Public GKEY As String = String.Empty

    Sub New()
        GKEY = "ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA"
        Try
            If Not Made4Net.Shared.Util.GetSystemParameterValue("GoogleMapKEY") Is Nothing OrElse GKEY <> String.Empty Then
                GKEY = Made4Net.Shared.Util.GetSystemParameterValue("GoogleMapKEY")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Function GoogleGeoCode(ByVal address As String, _
                    Optional ByVal params As ArrayList = Nothing) As String
        Try
            Dim output As String = "xml"
            Dim url As String = String.Format("http://maps.google.com/maps/geo?output={0}&q=" _
                        & HttpUtility.UrlEncode(address), output)
            Dim sr As New StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream())


            Dim coord As Match = Regex.Match(sr.ReadToEnd(), "<coordinates>.*</coordinates>")
            If Not (coord.Success) Then
                Return "N/A"
            Else
                Return coord.Value.Substring(13, coord.Length - 27)
            End If
            sr.Close()
        Catch ex As Exception
            Return "NA"
        End Try
    End Function

    Public Function GoogleReverseGeoCode(ByVal lat As String, _
                        ByVal lng As String, _
                    Optional ByVal params As ArrayList = Nothing) As ArrayList

        ''http://maps.google.com/maps/api/geocode/json?latlng=40.714224,-73.961452&sensor=true
        Dim address As String = String.Empty
        Dim addressList As New ArrayList
        Try

            Dim output As String = "xml"
            Dim url As String = String.Format("http://maps.google.com/maps/api/geocode/{2}?latlng={0},{1}&sensor=true", _
                           lat, lng, output)
            Dim sr As New StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream())
            Dim xpathDoc As XPathDocument
            Dim xmlNav As XPathNavigator

            Dim xmlNI As XPathNodeIterator
            xpathDoc = New XPathDocument(sr)
            xmlNav = xpathDoc.CreateNavigator()


            xmlNI = xmlNav.Select("//formatted_address")
            While (xmlNI.MoveNext())
                addressList.Add(xmlNI.Current.Value)
            End While

            ''see all http://code.google.com/apis/maps/documentation/geocoding/
            ''"/GeocodeResponse/result[address_component[type/text() = 'country']/short_name/text() = 'US']/address_component[type/text() = 'administrative_area_level_1']/long_name/text()"
            Return addressList

        Catch ex As Exception
            addressList.Add("NA")
            Return addressList

        End Try

    End Function

    Public Shared Function checkLatLng(ByVal pLATITUDE As Double, ByVal pLONGTITUDE As Double) As Boolean
        If (pLATITUDE = 0) And (pLONGTITUDE = 0) Then Return False
        If (pLATITUDE > 180) Or (pLATITUDE < -180) Then Return False
        If (pLONGTITUDE > 180) Or (pLONGTITUDE < -180) Then Return False
        Return True
    End Function


    ''http://maps.googleapis.com/maps/api/distancematrix/xml?origins=Berlin+Germany|riga+Latvia|Jurmala+Latvia&destinations=dfdfdfdf|Jurmala+Latvia&mode=driving&language=EN-US&sensor=false
    ''http://code.google.com/apis/maps/documentation/distancematrix/#DistanceMatrixRequests
    ''http://maps.googleapis.com/maps/api/distancematrix/xml?origins=Latvia,Jurmala&destinations=5%20D%C4%81rza%20iela,R%C4%ABga,Latvia&mode=driving&language=EN-US&sensor=false
    ''<DistanceMatrixResponse>
    '<status>OK</status>
    '<origin_address>Jurmala, Latvia</origin_address>
    '<destination_address>Dārza iela 5, Riga, LV-1007, Latvia</destination_address>
    '<row>
    '<element>
    '<status>OK</status>
    '<duration>
    '<value>1743</value>
    '<text>29 mins</text>
    '</duration>
    '<distance>
    '<value>21254</value>
    '<text>21.3 km</text>
    '</distance>
    '</element>
    '</row>
    '</DistanceMatrixResponse>
    Public Shared Function getGoogleDitanceMatrix(ByVal pOrigins As ArrayList, _
                                        ByVal pDestinations As ArrayList, _
                                          ByVal mode As String, _
                            ByVal language As String) As ArrayList


        '' address format <strret>+<city>+<state>|
        '' address format <lat>+<lon>|

        Dim resArr As New ArrayList()
        If pOrigins.Count = 0 Or pDestinations.Count = 0 Then Return resArr

        If mode = String.Empty Then mode = "driving" ''driving,walking,bicycling
        If language = String.Empty Then language = "en"

        Dim url As String
        Dim sOrigins As String
        For Each s As String In pOrigins
            sOrigins &= s & "|"
        Next
        sOrigins = sOrigins.Substring(0, sOrigins.Length - 1)

        Dim sDestinations As String
        For Each s As String In pDestinations
            sDestinations &= s & "|"
        Next
        sDestinations = sDestinations.Substring(0, sDestinations.Length - 1)

        url = String.Format("http://maps.googleapis.com/maps/api/distancematrix/xml?origins={0}&destinations={1}&mode={2}&language={3}&sensor=false", _
                    sOrigins, sDestinations, mode, language)


        Dim sr As New StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream())
        Dim xpathDoc As XPathDocument
        xpathDoc = New XPathDocument(sr)

        Dim xmlNav As XPathNavigator
        xmlNav = xpathDoc.CreateNavigator()

        Dim rows As XPathNodeIterator
        rows = xmlNav.Select("/DistanceMatrixResponse/row")
        Dim originscnt As Integer = 0
        Dim destionationcnt As Integer = 0
        While rows.MoveNext()

            Dim rowsnavigator As XPathNavigator = rows.Current
            Dim elementText As XPathNodeIterator = rowsnavigator.SelectDescendants(XPathNodeType.Text, False)

            While elementText.MoveNext()
                If elementText.Current.OuterXml = "OK" Then
                    elementText.MoveNext()
                    Dim duration As String = elementText.Current.OuterXml
                    elementText.MoveNext()
                    elementText.MoveNext()
                    Dim distance As String = elementText.Current.OuterXml
                    resArr.Add(pOrigins(originscnt) & "#" & _
                               pDestinations(destionationcnt) & "#" & _
                            duration & "#" & _
                            distance)
                    elementText.MoveNext()
                Else
                    resArr.Add(pOrigins(originscnt) & "#" & _
                               pDestinations(destionationcnt) & "#" & _
                            "-1" & "#" & _
                            "-1")
                End If

                destionationcnt += 1
            End While
            destionationcnt = 0
            originscnt += 1
        End While

        ''see all http://code.google.com/apis/maps/documentation/geocoding/
        ''"/GeocodeResponse/result[address_component[type/text() = 'country']/short_name/text() = 'US']/address_component[type/text() = 'administrative_area_level_1']/long_name/text()"
        Return resArr

    End Function




    Public Shared Function getGoogleDitance(ByVal pOrigin As String, _
                                        ByVal pDestination As String, _
                                          ByVal mode As String, _
                            ByVal language As String, _
                            ByRef distance As Double, _
                            ByRef duration As Double) As String


        '' address format <strret>+<city>+<state>|
        '' address format <lat>+<lon>|
        ''http://maps.googleapis.com/maps/api/distancematrix/xml?origins=Latvia,Jurmala&destinations=5%20D%C4%81rza%20iela,R%C4%ABga,Latvia&mode=driving&language=EN-US&sensor=false

        distance = -1
        duration = -1
        If pOrigin = String.Empty Then Return "Undefined Origin"
        If pDestination = String.Empty Then Return "Undefined Destination"

        If mode = String.Empty Then mode = "driving" ''driving,walking,bicycling
        If language = String.Empty Then language = "en"

        Dim url As String


        url = String.Format("http://maps.googleapis.com/maps/api/distancematrix/xml?origins={0}&destinations={1}&mode={2}&language={3}&sensor=false", _
                    pOrigin, pDestination, mode, language)

        Dim sr As New StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream())
        Dim xpathDoc As XPathDocument
        xpathDoc = New XPathDocument(sr)

        Dim xmlNav As XPathNavigator
        xmlNav = xpathDoc.CreateNavigator()

        Dim rows As XPathNodeIterator
        rows = xmlNav.Select("/DistanceMatrixResponse/row")
        Dim originscnt As Integer = 0
        Dim destionationcnt As Integer = 0
        Dim status As String

        rows.MoveNext()
        Dim rowsnavigator As XPathNavigator = rows.Current
        Dim elementText As XPathNodeIterator = rowsnavigator.SelectDescendants(XPathNodeType.Text, False)

        elementText.MoveNext()
        status = elementText.Current.OuterXml
        If status = "OK" Then
            elementText.MoveNext()
            duration = CDbl(elementText.Current.OuterXml)
            elementText.MoveNext()
            elementText.MoveNext()
            distance = CDbl(elementText.Current.OuterXml)
        End If
        Return status

    End Function



End Class



Public Class LatLon
    Public Lat As Double
    Public Lon As Double
    Sub New(ByVal pLat As Double, ByVal pLon As Double)
        Lat = pLat
        Lon = pLon
    End Sub
End Class

Public Class LatLonCollection
    Public PointsArrayList As New ArrayList
    Public CenterLatLon As LatLon
    Public North As LatLon
    Public South As LatLon
    Public East As LatLon
    Public West As LatLon



    Sub New()
    End Sub
    Public Const AlonePointZoom As String = "15"

    Public Function Add(ByVal pLatLon As LatLon) As LatLon
        PointsArrayList.Add(pLatLon)
        RecalcCenter()
        Return CenterLatLon
    End Function

    Protected Sub SetBounds(ByVal pLatLon As LatLon)
        If South Is Nothing OrElse South.Lat > pLatLon.Lat Then South = pLatLon
        If North Is Nothing OrElse North.Lat < pLatLon.Lat Then North = pLatLon

        If West Is Nothing OrElse West.Lon > pLatLon.Lon Then West = pLatLon
        If East Is Nothing OrElse East.Lon < pLatLon.Lon Then East = pLatLon

    End Sub

    Protected Sub RecalcCenter()
        Dim SumLat, SumLon As Double
        For Each oLatLon As LatLon In PointsArrayList
            SumLat += oLatLon.Lat
            SumLon += oLatLon.Lon
        Next
        CenterLatLon = New LatLon(SumLat / PointsArrayList.Count, SumLon / PointsArrayList.Count)
    End Sub
    Public Function getMaxDistance() As Double
        Dim maxd As Double = 0D
        Dim maxlat As Double = Integer.MinValue
        Dim maxlon As Double = Integer.MinValue
        Dim minlat As Double = Integer.MaxValue
        Dim minlon As Double = Integer.MaxValue

        For Each oLatLon As LatLon In PointsArrayList
            If oLatLon.Lat > maxlat Then maxlat = oLatLon.Lat
            If oLatLon.Lat < minlat Then minlat = oLatLon.Lat

            If oLatLon.Lon > maxlon Then maxlon = oLatLon.Lon
            If oLatLon.Lon < minlon Then minlon = oLatLon.Lon
        Next
        maxd = GeoPoint.CalculateDistance(GeoPoint.Deg2Int(minlat), GeoPoint.Deg2Int(minlon), GeoPoint.Deg2Int(maxlat), GeoPoint.Deg2Int(maxlon))
        Return maxd / 1000
    End Function

    Public Function getZoom(ByVal mapheight As Integer, ByVal mapwidth As Integer) As Integer
        Dim dist As Double = getMaxDistance()
        If dist = 0 Then Return AlonePointZoom

        Dim zoom As Integer
        zoom = Math.Floor(6 - Math.Log(1.6446 * dist / Math.Sqrt(2 * (mapheight * mapwidth))) / Math.Log(2))
        If zoom <= 1 And zoom > 15 Then zoom = AlonePointZoom
        If zoom <= 1 Then zoom = 2
        If zoom > AlonePointZoom Then zoom = AlonePointZoom

        Return zoom

    End Function


    Public Shared Function Int2Deg(ByVal x As Integer) As Double
        x = Math.Floor(x / 100)
        Dim secs, minutes As Integer
        Dim deg As Integer = Math.DivRem(x, 3600, minutes)
        minutes = Math.DivRem(minutes, 60, secs)
        Return deg + (minutes + (secs / 60.0)) / 60.0
    End Function

    Public Shared Function Deg2Int(ByVal d As Double) As Double
        Dim deg As Double = Math.Floor(d)
        Dim minutes As Double = Math.Floor((d - deg) * 60)
        Dim secs As Double = Math.Floor(((d - deg) * 60 - minutes) * 60)
        Return Math.Floor((deg * 3600 + minutes * 60 + secs) * 100)
    End Function

End Class