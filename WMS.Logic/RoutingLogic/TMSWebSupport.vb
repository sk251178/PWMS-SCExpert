<CLSCompliant(False)> Public Class TMSWebSupport

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Try
            If System.Web.HttpContext.Current Is Nothing Then
                Return
            End If
        Catch ex As Exception
            Return
        End Try
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName.ToLower = "center" Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            Select Case Convert.ToString(dr("OBJECTTYPE"))
                Case "COMP"
                    CenterComp(dr("CONSIGNEE"), dr("COMPANY"), dr("COMPANYTYPE"))
                Case "DRIVERS"
                    Dim oDriver As New Driver(dr("DRIVERID"))
                    CenterPoint(oDriver.POINTID)
                    'Case "DEPOT"
                    '    Dim oDepot As New Depots(dr("DEPOTNAME"))
                    '    CenterPoint(oDepot.POINTID)
            End Select
        ElseIf CommandName.ToLower = "centerdriver" Then
            Dim dr As DataRow = ds.Tables(0).Rows(0)
            If dr("STARTINGPOINT") Is DBNull.Value Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Driver has no starting point", "Driver has no starting point")
            ElseIf dr("STARTINGPOINT") = "" Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Driver has no starting point", "Driver has no starting point")
            Else
                CenterPoint(dr("STARTINGPOINT"))
            End If
        ElseIf CommandName.ToLower = "save" Then

            Dim dr As DataRow = ds.Tables(0).Rows(0)
            Dim am As New AddressMatcher
            Dim mp As MapPoint
            Dim sStreet, sCity, sHouseNumber As String

            If Not dr.IsNull("city") Then sCity = dr("city")
            If Not dr.IsNull("street1") Then sStreet = dr("street1")
            If Not dr.IsNull("housenumber1") Then sHouseNumber = dr("housenumber1")

            mp = am.MatchAddress(sCity, sStreet, sHouseNumber)
            Dim tmsMapPoint As New Made4Net.GeoData.GeoPoint
            'tmsMapPoint.CITY = sCity
            tmsMapPoint.LATITUDE = mp.LatitudeDec
            tmsMapPoint.LONGITUDE = mp.LongitudeDec
            tmsMapPoint.POINTTYPEID = 1
            'tmsMapPoint.STREET = sStreet
            tmsMapPoint.Create(Common.GetCurrentUser)

            Dim cntct As New Contact(dr("contactid"), Contact.ContactTypeFromString(dr("contacttype")))
            cntct.STREET1 = sStreet
            'cntct.HouseNumber1 = sHouseNumber
            cntct.CITY = sCity
            cntct.POINTID = tmsMapPoint.POINTID
            cntct.Save(Common.GetCurrentUser)
            Message = "Customer Pointed"
            'cntct.Save(WMS.Lib.Common.GetCurrentUser)

        End If
    End Sub

    Protected Sub CenterComp(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String)
        Dim oComp As New Company(pConsignee, pCompany, pCompanyType)
        If oComp.DEFAULTCONTACTOBJ.POINTID Is Nothing Or oComp.DEFAULTCONTACTOBJ.POINTID = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Company has no point", "Company has no point")
        End If

        CenterPoint(oComp.DEFAULTCONTACTOBJ.POINTID)
    End Sub

    Protected Sub CenterDriver(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String)
        Dim oComp As New Company(pConsignee, pCompany, pCompanyType)
        If oComp.DEFAULTCONTACTOBJ.POINTID Is Nothing Or oComp.DEFAULTCONTACTOBJ.POINTID = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Company has no point", "Company has no point")
        End If

        CenterPoint(oComp.DEFAULTCONTACTOBJ.POINTID)
    End Sub

    Protected Sub CenterPoint(ByVal PointId As String)
        If PointId = "" Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Object is not pinned", "Object is not pinned")
        End If
        With System.Web.HttpContext.Current.Response
            .Write("<Script language=""javascript"">" & vbCrLf)
            .Write("try {" & vbCrLf)
            .Write("window.parent.args.value='" & PointId & "';" & vbCrLf)
            .Write("window.parent.command.value=""gotopoint"";" & vbCrLf)
            .Write("window.parent.btnAct.click();" & vbCrLf)
            .Write("} catch (e){}" & vbCrLf)
            .Write("</Script>" & vbCrLf)
        End With
    End Sub

End Class
