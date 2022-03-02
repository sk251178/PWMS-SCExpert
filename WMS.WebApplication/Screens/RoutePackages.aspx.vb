Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports WMS.Lib

Partial Public Class RoutePackages
    Inherits System.Web.UI.Page

#Region "Ctors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "addpackage"
                dr = ds.Tables(0).Rows(0)
                Dim oPackage As New WMS.Logic.RoutePackage
                oPackage.Create(ReplaceDBNull(dr("packageid")), ReplaceDBNull(dr("packagetype")), ReplaceDBNull(dr("documenttype")), ReplaceDBNull(dr("documentid")), ReplaceDBNull(dr("consignee")), UserId)
            Case "editpackage"
                dr = ds.Tables(0).Rows(0)
                Dim oPackage As New WMS.Logic.RoutePackage(ReplaceDBNull(dr("packageid")))
                oPackage.Update(ReplaceDBNull(dr("packagetype")), ReplaceDBNull(dr("documenttype")), ReplaceDBNull(dr("documentid")), ReplaceDBNull(dr("consignee")), oPackage.Status, UserId)
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TERoutesPackages_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutesPackages.CreatedChildControls
        With TERoutesPackages
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RoutePackages"
                If TERoutesPackages.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addPackage"
                ElseIf TERoutesPackages.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "editpackage"
                End If
            End With
        End With
    End Sub

End Class