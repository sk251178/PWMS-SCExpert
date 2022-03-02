Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert

Partial Public Class PinMapSave
    Inherits System.Web.UI.Page
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Protected contactid As String
    Protected lon As Double
    Protected lat As Double


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            contactid = ReplaceDBNull(Convert.ToString(HttpContext.Current.Request.Params("contact")))
            lon = ReplaceDBNull(Double.Parse(HttpContext.Current.Request.Params("x")))
            lat = ReplaceDBNull(Double.Parse(HttpContext.Current.Request.Params("y")))
            If contactid <> String.Empty And lon <> 0 And lat <> 0 Then PinContact()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub PinContact()
        Dim mp As New Made4Net.GeoData.GeoPointNode
        If Made4Net.GeoData.GeoPoint.GeoPointExist(lon, lat) Then
            Dim tmpId As String = Made4Net.GeoData.GeoPointNode.GetGeoPointId(lon, lat)
            mp = Made4Net.GeoData.GeoPointNode.GetPoints(Convert.ToInt32(tmpId))
        Else
            Made4Net.GeoData.GeoPointNode.GetPoints()
            mp = New Made4Net.GeoData.GeoPointNode
            mp.LONGITUDE = lon
            mp.LATITUDE = lat
            mp.POINTTYPEID = "contact"
            Try
                mp.Create(Common.GetCurrentUser)
            Catch ex As Exception
            End Try
        End If

        Dim oContact As New WMS.Logic.Contact(contactID)
        oContact.setPointId(mp.POINTID, WMS.Logic.Common.GetCurrentUser)
        oContact = Nothing


    End Sub


  
End Class