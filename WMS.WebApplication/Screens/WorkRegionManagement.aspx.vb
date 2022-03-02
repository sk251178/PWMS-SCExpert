Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

Partial Public Class WorkRegionManagement
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "AssignUser"
                Dim oUserWorkRegion As New WMS.Logic.UserWorkRegion()
                For Each dr In ds.Tables(0).Rows
                    oUserWorkRegion.WorkRegionId = dr("WorkRegionId")
                    oUserWorkRegion.WarehouseArea = dr("WarehouseArea")
                    oUserWorkRegion.UserId = dr("UserId")
                    oUserWorkRegion.Save(UserId)
                Next
            Case "UnAssignUser"
                For Each dr In ds.Tables(0).Rows
                    Dim oUserWorkRegion As New WMS.Logic.UserWorkRegion(dr("UserId"), dr("WorkRegionId"), dr("WarehouseArea"))
                    oUserWorkRegion.Delete()
                Next
            Case "SetPriority"
                For Each dr In ds.Tables(0).Rows
                    Dim oUserWorkRegion As New WMS.Logic.UserWorkRegion(dr("UserId"), dr("WorkRegionId"), dr("WarehouseArea"))
                    oUserWorkRegion.Priority = dr("Priority")
                    oUserWorkRegion.Save(UserId)
                Next
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Build the screen query according to the search filters
        Dim workRegionType As String
        'Dim UserRole As String
        Dim ctrl As Made4Net.WebControls.DisplayTypes.DropDownList
        If Not TEWORKREGION.SearchForm Is Nothing Then
            Try
                ctrl = TEWORKREGION.SearchForm.Controls(0).FindControl("Field_WORKREGIONTYPE")
                workRegionType = ctrl.Value.Replace(" ", "")
                'ctrl.SetValues("")
            Catch ex As Exception
                workRegionType = ""
            End Try
            'Try
            '    ctrl = TEWORKREGION.SearchForm.Controls(0).FindControl("Field_ActiveUserRole")
            '    UserRole = ctrl.GetValues()(0).Replace(" ", "")
            '    ViewState()("UserRoles") = ctrl.GetValues()
            '    ctrl.SetValues("")
            'Catch ex As Exception
            '    UserRole = ""
            'End Try

            TEWORKREGION.SQL = BuildQuery(workRegionType)
            TEWORKREGION.FilterExpression = GetLikeCluase("WORKREGIONTYPE", workRegionType)
        Else
            TEWORKREGION.SQL = BuildQuery("")
            TEWORKREGION.FilterExpression = String.Empty
            ViewState()("UserRoles") = Nothing
        End If
    End Sub

    Private Sub TEWORKREGION_AfterDataLoaded(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWORKREGION.AfterDataLoaded
        'Dim ctrl As Made4Net.WebControls.DisplayTypes.MultiSelect
        'If Not TEWORKREGION.SearchForm Is Nothing Then
        '    'Try
        '    '    ctrl = TEWORKREGION.SearchForm.Controls(0).FindControl("Field_WORKREGIONTYPE")
        '    '    ctrl.SetValues(ViewState()("WorkRegions"))
        '    'Catch ex As Exception
        '    'End Try
        '    Try
        '        CType(TEWORKREGION.SearchForm.Controls(0).FindControl("Field_ActiveUserRole"), Made4Net.WebControls.DisplayTypes.MultiSelect).SetValues(ViewState()("UserRoles"))
        '        'ctrl.SetValues(ViewState()("UserRoles"))

        '    Catch ex As Exception
        '    End Try
        'End If
    End Sub

    Private Function BuildQuery(ByVal pUserRole As String) As String
        Dim selectSql As String
        Dim userRoleClause As String
        userRoleClause = GetLikeCluase("dbo.USERSKILL.ROLE", pUserRole)
        selectSql = String.Format("SELECT WORKREGIONID, WAREHOUSEAREA, WORKREGIONTYPE, WORKREGIONDESC, MAXRESOURCES, FullRepl, PartRepl, FullPick, " & _
            " PartPick, Putaways, Counts, Other, ExpectedHotReplenishments, " & _
            " (SELECT count(dbo.USERSKILL.USERID) as numUsers FROM dbo.USERSKILL INNER JOIN dbo.USERWORKREGION ON dbo.USERSKILL.USERID = dbo.USERWORKREGION.USERID " & _
            " where {0} and dbo.USERWORKREGION.WORKREGIONID = vWorkRegionsManagement.workregionid  " & _
            " and USERWORKREGION.WAREHOUSEAREA = vWorkRegionsManagement.WAREHOUSEAREA)  AS AssignedUsers, " & _
            " (SELECT count(dbo.USERSKILL.USERID) as numUsers " & _
            " FROM dbo.USERWORKREGION AS uwr INNER JOIN dbo.WHACTIVITY AS wa ON uwr.USERID = wa.USERID inner join " & _
            " dbo.USERSKILL ON uwr.USERID = USERSKILL.USERID INNER JOIN dbo.USERWORKREGION ON dbo.USERSKILL.USERID = dbo.USERWORKREGION.USERID " & _
            " where {0} and dbo.USERWORKREGION.WORKREGIONID = vWorkRegionsManagement.workregionid " & _
            " and USERWORKREGION.WAREHOUSEAREA = vWorkRegionsManagement.WAREHOUSEAREA) AS ActiveUsers, " & _
            " (SELECT count(dbo.USERSKILL.USERID) as numUsers " & _
            " FROM dbo.WHACTIVITY inner join dbo.USERSKILL ON dbo.USERSKILL.USERID = dbo.WHACTIVITY.USERID inner join " & _
            " vworkregionlocations on vworkregionlocations.location = WHACTIVITY.location and vworkregionlocations.warehousearea " & _
            " = WHACTIVITY.warehousearea where {0} and vworkregionlocations.WORKREGIONID = " & _
            " vWorkRegionsManagement.workregionid and vworkregionlocations.WAREHOUSEAREA = vWorkRegionsManagement.WAREHOUSEAREA) AS InRegionUsers" & _
            " FROM vWorkRegionsManagement ", userRoleClause)
        Return selectSql
    End Function

    Private Function GetLikeCluase(ByVal pFieldName As String, ByVal pValue As String) As String
        Dim ret As String
        Dim arr() As String = pValue.Split(",")
        For i As Int32 = 0 To arr.Length - 1
            ret = ret & String.Format(" {0} like '{1}%' or", pFieldName, arr(i))
        Next
        ret = ret.TrimEnd("or".ToCharArray())
        Return String.Format("({0})", ret)
    End Function

    Private Sub TEAssignUsers_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignUsers.CreatedChildControls
        With TEAssignUsers
            With .ActionBar
                .AddSpacer()
                .AddExecButton("AssignUser", "Assign User", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("AssignUser")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.WorkRegionManagement"
                    .CommandName = "AssignUser"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to assign the selected users to work region?"
                End With
            End With
        End With
    End Sub

    Private Sub TEUnAssignUser_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUnAssignUser.CreatedChildControls
        With TEUnAssignUser
            With .ActionBar
                .AddSpacer()
                .AddExecButton("UnAssignUser", "UnAssign User", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("UnAssignUser")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.WorkRegionManagement"
                    .CommandName = "UnAssignUser"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unassign the selected users from work region?"
                End With
                .AddSpacer()
                .AddExecButton("SetPriority", "Set Priority", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("SetPriority")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.WorkRegionManagement"
                    .CommandName = "SetPriority"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to change the selected users work region priority?"
                End With
            End With
        End With
    End Sub

    Private Sub TEWORKREGION_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEWORKREGION.RecordSelected
        Dim tds As DataTable = TEWORKREGION.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Add("WorkRegionId", tds.Rows(0)("WorkRegionId"))
        vals.Add("WarehouseArea", tds.Rows(0)("WarehouseArea"))
        TEAssignUsers.PreDefinedValues = vals
        TEUnAssignUser.PreDefinedValues = vals
    End Sub

End Class