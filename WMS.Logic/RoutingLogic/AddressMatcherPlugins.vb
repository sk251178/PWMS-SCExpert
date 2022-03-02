Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Web
Imports Made4Net.GeoData
Imports made4net.DataAccess


Public Class AddressMatcherPlugins
    Public Shared GKEY As String = "ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA"

    Sub New()
        Try
            If Not Made4Net.Shared.SysParam.Get("GoogleMapKEY") Is Nothing OrElse GKEY <> String.Empty Then
                GKEY = Made4Net.Shared.SysParam.Get("GoogleMapKEY")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Function PinContactByCoordinates(ByVal pContactid As String, _
            ByVal Latitude As Double, ByVal Longitude As Double) As String
        Try
            Dim oContact As New Contact(pContactid, True)
            Dim PointID As String
            Dim msg As String

            PointID = setPoint(GeoPoint.Deg2Int(Latitude), GeoPoint.Deg2Int(Longitude), Latitude, Longitude)
            If PointID = String.Empty Then
                msg = "Error on point create."
                Return msg
            End If
            'Dim sql As String = String.Format("update mappoints set lat='{0}', lon='{1}' where pointid='{2}'", _
            '        Latitude, Longitude, PointID)
            'DataInterface.RunSQL(sql)

            oContact.setPointId(PointID, Common.GetCurrentUser)
            oContact = Nothing

            msg = "Contact Pinned successfully."
            Return msg



        Catch ex As Exception
            Throw New ApplicationException(ex.Message.ToString())

        End Try
    End Function

    Public Shared Function PinContact(ByVal pContactid As String, _
                                      Optional ByVal address As String = "", _
                                      Optional ByVal params As ArrayList = Nothing) As String

        Try
            Dim oContact As New Contact(pContactid, True)
            Dim msg As String

            If address = "" Then
                Dim sql As String
                sql = String.Format("select dbo.getContactAddress(contactid) from CONTACT where CONTACTID={0} ", _
                    Made4Net.Shared.Util.FormatField(pContactid))
                address = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DataInterface.ExecuteScalar(sql))
                If address = String.Empty Then
                    msg = "Contact not found."
                End If
                Return msg
            End If

            Dim coordinates As String() = GoogleGeoCode(address, params).Split(","c)
            Dim PointID As String

            If coordinates.Length < 2 Then
                msg = "Address does not match. Check the address."
                Return msg
            End If

            PointID = setPoint(GeoPoint.Deg2Int(Convert.ToDouble(coordinates(1), System.Globalization.CultureInfo.InvariantCulture)), _
                                                GeoPoint.Deg2Int(Convert.ToDouble(coordinates(0), System.Globalization.CultureInfo.InvariantCulture)), _
                Convert.ToDouble(coordinates(1), System.Globalization.CultureInfo.InvariantCulture), Convert.ToDouble(coordinates(0), System.Globalization.CultureInfo.InvariantCulture))

            If PointID = String.Empty Then
                msg = "Error on point create."
                Return msg

            End If

            oContact.setPointId(PointID, Common.GetCurrentUser)
            oContact = Nothing

            msg = "Contact Pinned successfully."
            Return msg
        Catch ex As Exception
            Throw New ApplicationException(ex.Message.ToString())
        End Try


    End Function

    Public Shared Function GoogleGeoCode(ByVal address As String, _
                Optional ByVal params As ArrayList = Nothing) As String
        Dim output As String = "xml"
        Dim url As String = String.Format("http://maps.google.com/maps/geo?output={0}&q=" _
                    & HttpUtility.UrlEncode(address), output)
        Dim sr As New StreamReader(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream())

        Try
            Dim coord As Match = Regex.Match(sr.ReadToEnd(), "<coordinates>.*</coordinates>")
            If Not (coord.Success) Then
                Return ""
            Else
                Return coord.Value.Substring(13, coord.Length - 27)
            End If
        Catch ex As Exception
            Return ""
        Finally
            sr.Close()
        End Try
    End Function

    Public Shared Function setPoint(ByVal lat As Double, _
                    ByVal lon As Double, _
                Optional ByVal Latitude As Double = 0, _
                Optional ByVal Longitude As Double = 0) As String

        Dim mp As New GeoPointNode()
        Dim POINTID As String
        If GeoPoint.GeoPointExist(lon, lat) Then
            POINTID = GeoPointNode.GetGeoPointId(lon, lat)
            ''Dim ht As GeoPointNodeHashtable = GeoPointNode.GetPoints()
            ''mp = DirectCast(ht(Convert.ToInt32(tmpId)), GeoPointNode)
        Else
            GeoPointNode.GetPoints()
            mp = New Made4Net.GeoData.GeoPointNode()
            mp.LONGITUDE = lon
            mp.LATITUDE = lat
            POINTID = mp.Create(Common.GetCurrentUser, Made4Net.GeoData.PointType.ContactPin)
            Try
                mp.AddGeoPointToNetwork(mp, Common.GetCurrentUser)
            Catch ex As Exception
            End Try

            If Latitude <> 0 And Longitude <> 0 Then
                Dim sql As String = String.Format("update mappoints set lat='{0}', lon='{1}' where pointid='{2}'", _
                    Latitude, Longitude, POINTID)
                DataInterface.RunSQL(sql)
            End If
        End If
        ''HttpContext.Current.Response.Write("mp.POINTID:" & POINTID)
        Return POINTID
     
    End Function


End Class
