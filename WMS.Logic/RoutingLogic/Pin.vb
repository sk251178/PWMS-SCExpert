Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class Pin

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName.ToLower = "pin" Then
            Dim objType As String
            Dim newPoint As String
            Try
                objType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("OBJECTTYPE"))
                newPoint = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NEWPOINTID"))
            Catch ex As Exception
                Throw New Made4Net.Shared.M4NException(New Exception, "ObjectTypeNotSelected", "ObjectTypeNotSelected")
            End Try
            If newPoint = "" Then
                Throw New Made4Net.Shared.M4NException(New Exception, "PointNotSelected", "PointNotSelected")
            End If
            Select Case objType
                Case "DEPOT"
                    Dim dep As New Depots(ds.Tables(0).Rows(0)("DEPOTNAME"))
                    dep.SetPoint(newPoint, Common.GetCurrentUser)
                Case "COMPANY"
                    Dim comp As New Company(ds.Tables(0).Rows(0)("CONSIGNEE"), ds.Tables(0).Rows(0)("COMPANY"), ds.Tables(0).Rows(0)("COMAPNYTYPE"))
                    comp.Pin(comp.DEFAULTCONTACT, newPoint, Common.GetCurrentUser)
                Case "DRIVER"
                    Dim driv As New Driver(ds.Tables(0).Rows(0)("driverid"))
                    driv.SetStartingPoint(newPoint, Common.GetCurrentUser)
            End Select
        End If
    End Sub

End Class
