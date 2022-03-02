Public Class UserPickRegion
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEUserRole As Made4Net.WebControls.TableEditor
    Protected WithEvents TEUSERWHAREA As Made4Net.WebControls.TableEditor
    Protected WithEvents TEUSER As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEUserWorkRegion As Made4Net.WebControls.TableEditor
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents rblWHAREAUSERS As RadioButtonList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region "Ctors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        Select Case CommandName.ToLower
            Case "assign"
                Dim oUserWorkRegion As New WMS.Logic.UserWorkRegion()
                Dim dr As DataRow
                dr = ds.Tables(0).Rows(0)
                oUserWorkRegion.Create(dr("UserID"), dr("workregionid"), dr("warehousearea"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("priority")), UserId)
            Case "unassign"
                Dim oUserWorkRegion As WMS.Logic.UserWorkRegion
                For Each dr As DataRow In ds.Tables(0).Rows
                    oUserWorkRegion = New WMS.Logic.UserWorkRegion(dr("userid"), dr("workregionid"), dr("warehousearea"))
                    oUserWorkRegion.Delete()
                Next
            Case "setpriority"
                Dim oUserWorkRegion As WMS.Logic.UserWorkRegion
                For Each dr As DataRow In ds.Tables(0).Rows
                    oUserWorkRegion = New WMS.Logic.UserWorkRegion(dr("userid"), dr("workregionid"), dr("warehousearea"))
                    oUserWorkRegion.Update(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("priority")), UserId)
                Next
            Case "assigntowharea"
                Dim oUser As WMS.Logic.User
                For Each dr As DataRow In ds.Tables(0).Rows
                    oUser = New WMS.Logic.User(dr("username"), False)
                    oUser.AssignToWarehouseArea(WMS.Logic.Warehouse.getUserWarehouseArea())
                Next
            Case "unassignfromwharea"
                Dim oUser As WMS.Logic.User
                For Each dr As DataRow In ds.Tables(0).Rows
                    oUser = New WMS.Logic.User(dr("username"), False)
                    oUser.UnAssignFromWarehouseArea(WMS.Logic.Warehouse.getUserWarehouseArea())
                Next
            Case "setskillrole"
                'check if user has a role ,if not its an insert mode (not update)
                For Each dr As DataRow In ds.Tables(0).Rows
                    'oUser = New WMS.Logic.User(dr("USERID"), False)
                    'If oUser.HasRoleDefined(dr("USERID"), dr("WHAREA")) Then
                    '    oUser.UpdateSkillRole(dr("SKILL"), dr("ROLE"), dr("WHAREA"))
                    'Else
                    '    oUser.InsertSkillRole(dr("SKILL"), dr("ROLE"), dr("WHAREA"))
                    'End If
                    If HasRoleDefined(dr("USERID"), dr("WHAREA")) Then
                        UpdateSkillRole(dr("SKILL"), dr("ROLE"), dr("WHAREA"), dr("USERID"))
                    Else
                        InsertSkillRole(dr("SKILL"), dr("ROLE"), dr("WHAREA"), dr("USERID"))
                    End If

                Next
            Case "insertskillrole"
                For Each dr As DataRow In ds.Tables(0).Rows
                    InsertSkillRole(dr("SKILL"), dr("ROLE"), dr("WHAREA"), dr("USERID"))
                Next

        End Select
    End Sub

    Public Function HasRoleDefined(ByVal pUSERID As String, ByVal pWHArea As String) As Boolean
        Dim SQL As String
        SQL = String.Format("select count(1) from USERSKILL where userid = '{0}' and WHAREA = '{1}'", WMS.Logic.GetCurrentUser, pWHArea)
        If Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)) > 0 Then
            Return True
        End If
        Return False
    End Function
    Public Sub UpdateSkillRole(ByVal pSkill As String, ByVal pRole As String, ByVal pWHArea As String, ByVal user As String)
        Dim SQL As String
        SQL = String.Format("update USERSKILL SET ROLE = '{0}', SKILL = '{1}' where userid = '{2}' and WHAREA = '{3}'", pRole, pSkill, user, pWHArea) ' WMS.Logic.GetCurrentUser, pWHArea)
        Made4Net.DataAccess.DataInterface.RunSQL(SQL)
    End Sub

    Public Sub InsertSkillRole(ByVal pSkill As String, ByVal pRole As String, ByVal pWHArea As String, ByVal user As String)
        Dim SQL As String
        Dim exist As Boolean

        SQL = String.Format("select count(1) from USERSKILL where userid = '{0}' and WHAREA = '{1}'", user, pWHArea) ' WMS.Logic.GetCurrentUser, pWHArea)
        exist = Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL))
        If exist Then
            Throw New Made4Net.Shared.M4NException(New Exception, "User Already has skill and role", "User Already has skill and role")
        Else
            SQL = String.Format("INSERT INTO USERSKILL (USERID, WHAREA, ROLE, SKILL) VALUES ('{0}','{1}','{2}','{3}') ", user, pWHArea, pRole, pSkill) ' WMS.Logic.GetCurrentUser, pWHArea, pRole, pSkill)
            Made4Net.DataAccess.DataInterface.RunSQL(SQL)
        End If
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        rblWHAREAUSERS_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Private Sub TEUserRole_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUserRole.CreatedChildControls
        With TEUserRole
            With .ActionBar
                With .Button("Save")
                    If TEUserRole.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        '.ObjectDLL = "WMS.Logic.dll"
                        '.ObjectName = "WMS.Logic.User"
                        .ObjectDLL = "WMS.WebApp.dll"
                        .ObjectName = "WMS.WebApp.UserPickRegion"
                        .CommandName = "setskillrole"
                    ElseIf TEUserRole.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        '.ObjectDLL = "WMS.Logic.dll"
                        '.ObjectName = "WMS.Logic.User"
                        .ObjectDLL = "WMS.WebApp.dll"
                        .ObjectName = "WMS.WebApp.UserPickRegion"
                        .CommandName = "insertskillrole"
                    End If
                End With
            End With
        End With
    End Sub

    Private Sub TEUserRole_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEUserRole.AfterItemCommand
        TEUserRole.RefreshData()
    End Sub

    Protected Sub TEUserWorkRegion_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEUserWorkRegion.CreatedChildControls

        TEUserWorkRegion.ActionBar.AddSpacer()
        TEUserWorkRegion.ActionBar.AddExecButton("SetPriority", "Set Priority", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        TEUserWorkRegion.ActionBar.AddExecButton("UnAssign", "Unassign", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))

        With TEUserWorkRegion.ActionBar.Button("SetPriority")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.All
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.UserPickRegion"
            .CommandName = "setpriority"
        End With

        With TEUserWorkRegion.ActionBar.Button("UnAssign")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.All
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.UserPickRegion"
            .CommandName = "unassign"
            .ConfirmRequired = True
            .ConfirmMessage = "Are you sure you want to unassign the selected work regions?"
        End With

        With TEUserWorkRegion.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.UserPickRegion"
            .CommandName = "assign"
        End With
    End Sub

    Private Sub TEUSER_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEUSER.AfterItemCommand
        rblWHAREAUSERS_SelectedIndexChanged(Nothing, Nothing)
    End Sub


    Private Sub TEUSER_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUSER.CreatedChildControls

        TEUSER.ActionBar.AddSpacer()
        If rblWHAREAUSERS.SelectedValue = "ShowNotAssigned" Then
            TEUSER.ActionBar.AddExecButton("AssignToWHArea", "Assign To Warehouse Area", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With TEUSER.ActionBar.Button("AssignToWHArea")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UserPickRegion"
                .CommandName = "AssignToWHArea"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to assign the selected users to current warehouse area?"
            End With
            'TEUSER.DisableSwitches = TEUSER.DisableSwitches & "t"
            'TEUSER.Restart()
        Else
            TEUSER.ActionBar.AddExecButton("UnAssignFromWHArea", "UnAssign From Warehouse Area", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
            With TEUSER.ActionBar.Button("UnAssignFromWHArea")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UserPickRegion"
                .CommandName = "UnAssignFromWHArea"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to unassign the selected users from current warehouse area?"
            End With
            'TEUSER.DisableSwitches = TEUSER.DisableSwitches.Replace("t", "")
        End If
    End Sub

    Private Sub rblWHAREAUSERS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblWHAREAUSERS.SelectedIndexChanged
        Try
            If rblWHAREAUSERS.SelectedValue = "ShowNotAssigned" Then
                TEUSER.FilterExpression = BuildUserWHFilter(False)
            Else
                TEUSER.FilterExpression = BuildUserWHFilter(True)
            End If
            TEUSER.RefreshData()
        Catch ex As Exception
        End Try
    End Sub

    Private Function BuildUserWHFilter(ByVal pIsAssigned As Boolean) As String
        Dim res As String = String.Empty
        Dim sql As String
        If pIsAssigned Then
            sql = String.Format("select distinct userid from userwharea where wharea = '{0}'", WMS.Logic.Warehouse.getUserWarehouseArea())
        Else
            sql = String.Format("select distinct userid from userwharea where wharea <> '{0}' and userid not in(select distinct userid from userwharea where wharea = '{0}' )", WMS.Logic.Warehouse.getUserWarehouseArea())
        End If
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return ""
        End If
        For Each dr As DataRow In dt.Rows
            res = res & "'" & dr("userid") & "',"
        Next
        res = res.TrimEnd(",".ToCharArray)
        res = String.Format(" username in ({0})", res)
        Return res
    End Function

End Class