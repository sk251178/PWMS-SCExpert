Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports WMS.Lib

Partial Public Class RouteGeneralTasks
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "Ctors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "addtask"
                dr = ds.Tables(0).Rows(0)
                Dim oGenTask As New WMS.Logic.RouteGeneralTask
                oGenTask.Create(ReplaceDBNull(dr("taskid")), ReplaceDBNull(dr("tasktype")), ReplaceDBNull(dr("scheduledate")), ReplaceDBNull(dr("notes")), ReplaceDBNull(dr("company")), _
                    ReplaceDBNull(dr("companytype")), ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("contactid")), UserId)
            Case "edittask"
                dr = ds.Tables(0).Rows(0)
                Dim oGenTask As New WMS.Logic.RouteGeneralTask(ReplaceDBNull(dr("taskid")))
                oGenTask.Update(ReplaceDBNull(dr("tasktype")), ReplaceDBNull(dr("scheduledate")), ReplaceDBNull(dr("notes")), ReplaceDBNull(dr("company")), _
                    ReplaceDBNull(dr("companytype")), ReplaceDBNull(dr("consignee")), oGenTask.Status, ReplaceDBNull(dr("contactid")), UserId)
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TERoutesTasks_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutesTasks.CreatedChildControls
        With TERoutesTasks
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteGeneralTasks"
                If TERoutesTasks.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addtask"
                ElseIf TERoutesTasks.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "edittask"
                End If
            End With
        End With
    End Sub

End Class