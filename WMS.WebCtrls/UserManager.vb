Imports Made4Net.WebControls
Imports Made4Net.Schema
Imports System.Web
Imports System.Web.UI

Namespace WebCtrls

    <CLSCompliant(False)> Public Class UserManager
        Inherits Made4Net.WebControls.UserManager

#Region "Variables"

        Protected WithEvents _TEUserWarehouse As TableEditor
        Protected WithEvents _UserWarehouseConnector As DataConnector

        'Protected WithEvents _TEUserSkillRole As TableEditor
        'Protected WithEvents _UserSkillRoleConnector As DataConnector

        Protected WithEvents _TEUserWHArea As TableEditor
        Protected WithEvents _UserWHAreaConnector As DataConnector

#End Region

        Public Property TEUSERWHAREA() As TableEditor
            Get
                Return _TEUserWHArea
            End Get
            Set(ByVal value As TableEditor)
                _TEUserWHArea = value
            End Set
        End Property

        Protected Overrides Sub CreateControlsUsers()
            _TEUsers = New TableEditor
            With _TEUsers
                .ID = "TEUsers"
                .DefaultDT = "UserManagement"
                .EditDT = "UserManagementEdit"
                .InsertDT = "UserManagementInsert"
                .DisableSwitches = "vrmnd"
                .ConnectionName = Made4Net.Schema.CONNECTION_NAME
                .ForbidRequiredFieldsInSearchMode = True
            End With

            _TEUserGroups = New TableEditor
            With _TEUserGroups
                .ID = "TEUserGroups"
                .DefaultDT = "UserManagementUserGroups"
                '.EditDT = "UserManagementEdit"
                '.InsertDT = "UserManagementInsert"
                .DisableSwitches = "vtmns"
                .ConnectionName = Made4Net.Schema.CONNECTION_NAME
                '.ForbidRequiredFieldsInSearchMode = True
            End With

            _UserGroupConnector = New DataConnector
            With _UserGroupConnector
                .ID = "UserGroupConnector"
                .MasterObjID = "TEUsers"
                .TargetID = "TEUserGroups"
                .ConnectTo = DataConnectorMode.DataObject
                .MasterFields = "username"
                .ChildFields = "username"
            End With

            '''''''''''''''''''''''''''''''''''''
            'user tabs

            _uDataTabControl = New DataTabControl
            With _uDataTabControl
                .ID = "uDTC"
                .ParentID = "TEUsers"
                .SyncEditMode = True
            End With

            _uDataTabOuterTable = New DataTabOuterTable
            _uDataTabControl.Controls.Add(_uDataTabOuterTable)
            With _uDataTabOuterTable
                .ID = "uDataTabOuterTable"
                .CellSpacing = 0
                .CellPadding = 0

                'create 2 rows
                .Rows.Add(New HtmlControls.HtmlTableRow)
                .Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)
                .Rows.Add(New HtmlControls.HtmlTableRow)
                .Rows(1).Cells.Add(New HtmlControls.HtmlTableCell)
            End With

            _uTabstrip = New TabStrip
            _uDataTabOuterTable.Rows(0).Cells(0).Controls.Add(_uTabstrip)
            With _uTabstrip
                .ID = "uTabStrip"
                .TargetTableID = "uDataTabInnerTable"
                .AutoPostBack = True

                Dim T As New Microsoft.Web.UI.WebControls.Tab
                T.Text = "Group Membership"
                .Items.Add(T)

                T = New Microsoft.Web.UI.WebControls.Tab
                T.Text = "User Permissions"
                .Items.Add(T)

                T = New Microsoft.Web.UI.WebControls.Tab
                T.Text = "Warehouse Membership"
                .Items.Add(T)

                T = New Microsoft.Web.UI.WebControls.Tab
                T.Text = "User Profile"
                .Items.Add(T)

                'T = New Microsoft.Web.UI.WebControls.Tab
                'T.Text = "Skill level & Role"
                '.Items.Add(T)

                T = New Microsoft.Web.UI.WebControls.Tab
                T.Text = "Warehouse Area"
                .Items.Add(T)
            End With

            _TEUserPermissions = New TableEditor
            With _TEUserPermissions
                .ID = "TEUserPermissions"
                .DefaultDT = "UserManagementPermissions"
                '.EditDT = "UserManagementEdit"
                '.InsertDT = "UserManagementInsert"
                .DisableSwitches = "vtmns"
                .ConnectionName = Made4Net.Schema.CONNECTION_NAME
                '.ForbidRequiredFieldsInSearchMode = True
                .SortExperssion = "rank"
            End With

            _UserPermissionsConnector = New DataConnector
            With _UserPermissionsConnector
                .ID = "UserPermissionsConnector"
                .MasterObjID = "TEUsers"
                .TargetID = "TEUserPermissions"
                .ConnectTo = DataConnectorMode.DataObject
                .MasterFields = "username"
                .ChildFields = "username"
            End With

            _TEUserWarehouse = New TableEditor

            With _TEUserWarehouse
                .ID = "TEUserWarehouse"
                .DefaultDT = "UserWarehouseManagement"
                .DisableSwitches = "vtmns"
                .ConnectionName = Made4Net.Schema.CONNECTION_NAME
            End With

            _UserWarehouseConnector = New DataConnector
            With _UserWarehouseConnector
                .ID = "UserWarehouseConnector"
                .MasterObjID = "TEUsers"
                .TargetID = "TEUserWarehouse"
                .ConnectTo = DataConnectorMode.DataObject
                .MasterFields = "username"
                .ChildFields = "userid"
            End With

            _TEUserProfile = New TableEditor
            With _TEUserProfile
                .ID = "TEUserProfile"
                .DefaultDT = "UserManagementUserProfile"
                '.EditDT = "UserManagementEdit"
                '.InsertDT = "UserManagementInsert"
                .DisableSwitches = "tmnsi"
                .ConnectionName = Made4Net.Schema.CONNECTION_NAME
                .DefaultMode = TableEditorMode.View
                .AfterUpdateMode = TableEditorMode.View
                '.ForbidRequiredFieldsInSearchMode = True
            End With

            _UserProfileConnector = New DataConnector
            With _UserProfileConnector
                .ID = "UserProfileConnector"
                .MasterObjID = "TEUsers"
                .TargetID = "TEUserProfile"
                .ConnectTo = DataConnectorMode.DataObject
                .MasterFields = "username"
                .ChildFields = "userid"
            End With

            '_TEUserSkillRole = New TableEditor
            'With _TEUserSkillRole
            '    .ID = "TEUserSkillRole"
            '    .DefaultDT = "DTUserSkillRole"
            '    .DisableSwitches = "vtmns"
            '    .ConnectionName = Made4Net.Schema.CONNECTION_NAME
            'End With

            '_UserSkillRoleConnector = New DataConnector
            'With _UserSkillRoleConnector
            '    .ID = "UserSkillRoleConnector"
            '    .MasterObjID = "TEUsers"
            '    .TargetID = "TEUserSkillRole"
            '    .ConnectTo = DataConnectorMode.DataObject
            '    .MasterFields = "username"
            '    .ChildFields = "userid"
            'End With

            _TEUserWHArea = New TableEditor

            With _TEUserWHArea
                .ID = "TEUserWHArea"
                .DefaultDT = "DTUSERWHAREAUM"
                '.InsertDT = "DTUSERWHAREAUMInsert"
                .DisableSwitches = "etvmns"
                .AllowDeleteInViewMode = True
            End With

            _UserWHAreaConnector = New DataConnector
            With _UserWHAreaConnector
                .ID = "UserWHAreaConnector"
                .MasterObjID = "TEUsers"
                .TargetID = "TEUserWHArea"
                .MasterFields = "username"
                .ChildFields = "userid"
            End With

            _uDataTabInnerTable = New DataTabInnerTable
            _uDataTabOuterTable.Rows(1).Cells(0).Controls.Add(_uDataTabInnerTable)
            With _uDataTabInnerTable
                .ID = "uDataTabInnerTable"
                .CellSpacing = 0
                .CellPadding = 5

                'create 5 cells
                .Rows.Add(New HtmlControls.HtmlTableRow)
                .Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)
                .Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)
                .Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)
                .Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)
                .Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)
                '.Rows(0).Cells.Add(New HtmlControls.HtmlTableCell)

                .Rows(0).Cells(0).Controls.Add(_TEUserGroups)
                .Rows(0).Cells(0).Controls.Add(_UserGroupConnector)

                .Rows(0).Cells(1).Controls.Add(_TEUserPermissions)
                .Rows(0).Cells(1).Controls.Add(_UserPermissionsConnector)

                .Rows(0).Cells(2).Controls.Add(_TEUserWarehouse)
                .Rows(0).Cells(2).Controls.Add(_UserWarehouseConnector)

                .Rows(0).Cells(3).Controls.Add(_TEUserProfile)
                .Rows(0).Cells(3).Controls.Add(_UserProfileConnector)

                '.Rows(0).Cells(3).Controls.Add(_TEUserSkillRole)
                '.Rows(0).Cells(3).Controls.Add(_UserSkillRoleConnector)

                .Rows(0).Cells(4).Controls.Add(_TEUserWHArea)
                .Rows(0).Cells(4).Controls.Add(_UserWHAreaConnector)

            End With
        End Sub

#Region "Ctor"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

        Private Sub _TEUserProfile_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles _TEUserProfile.CreatedChildControls
            With _TEUserProfile.ActionBar.Button("Save")
                If _TEUserProfile.CurrentMode = TableEditorMode.Edit Or _TEUserProfile.CurrentMode = TableEditorMode.Insert Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.UserManagement"
                    .CommandName = "saveuserprofile"
                End If
            End With
        End Sub

        Private Sub _TEUserWHArea_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles _TEUserWHArea.CreatedChildControls
            With _TEUserWHArea.ActionBar.Button("Save")
                If _TEUserWHArea.CurrentMode = TableEditorMode.Insert Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.UserManagement"
                    .CommandName = "assignusers"
                End If
            End With

            If _TEUserWHArea.CurrentMode = TableEditorMode.Grid Then
                _TEUserWHArea.ActionBar.AddExecButton("unassignusers", "Unassign Users", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCloseReceipt"))
                With _TEUserWHArea.ActionBar.Button("unassignusers")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.UserManagement"
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .CommandName = "unassignusers"
                    .ConfirmRequired = False
                End With
            End If

            'Dim vals As New Specialized.NameValueCollection
            'vals.Add("UserID", Me.SelectedUser)
            '_TEUserWHArea.PreDefinedValues = vals
            'RaiseEvent CreatedChildControlsForTEUSWHAREA()
        End Sub

        Public Event CreatedChildControlsForTEUSWHAREA()

        Public ReadOnly Property SelectedUser() As String
            Get
                Return _TEUsers.SelectedRecordPrimaryKeyValues("USERNAME")
            End Get
        End Property

    End Class

End Namespace