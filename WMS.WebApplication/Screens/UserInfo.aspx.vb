Imports Made4Net.DataAccess

Public Class UserInfo
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEUserInfo As Made4Net.WebControls.TableEditor
    Protected WithEvents UserID As Made4Net.WebControls.Label
    Protected WithEvents FirstName As Made4Net.WebControls.Label
    Protected WithEvents LastName As Made4Net.WebControls.Label
    Protected WithEvents FullName As Made4Net.WebControls.Label
    Protected WithEvents DefaultLanguage As Made4Net.WebControls.Label
    Protected WithEvents DefaultSkin As Made4Net.WebControls.Label
    Protected WithEvents DefaultConsignee As Made4Net.WebControls.Label
    Protected WithEvents txtFirstName As Made4Net.WebControls.TextBox
    Protected WithEvents txtLastName As Made4Net.WebControls.TextBox
    Protected WithEvents txtFullName As Made4Net.WebControls.TextBox
    Protected WithEvents ddLanguage As Made4Net.WebControls.DropDownList
    Protected WithEvents ddSkin As Made4Net.WebControls.DropDownList
    Protected WithEvents txtUserID As Made4Net.WebControls.TextBox
    Protected WithEvents Label1 As Made4Net.WebControls.Label
    Protected WithEvents noUserdiv As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents noUserlbl As System.Web.UI.WebControls.Label
    Protected WithEvents userDetail As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents txtConsignee As Made4Net.WebControls.TextBox
    Protected WithEvents Label2 As Made4Net.WebControls.Label
    Protected WithEvents Label4 As Made4Net.WebControls.Label
    Protected WithEvents tbNewPass1 As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents Label3 As Made4Net.WebControls.Label
    Protected WithEvents tbNewPass2 As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents Button1 As Made4Net.WebControls.Button
    Protected WithEvents lblNewPassMsg As System.Web.UI.WebControls.Label
    Protected WithEvents btnSave As Made4Net.WebControls.Button
    Protected WithEvents DefaultScreen As Made4Net.WebControls.Label
    Protected WithEvents txtScreenid As Made4Net.WebControls.TextBox

    Protected WithEvents ddUserWHArea As Made4Net.WebControls.DropDownList
    Protected WithEvents btnWHAreaChange As Made4Net.WebControls.Button
    Protected WithEvents btnUserParamSave As Made4Net.WebControls.Button

    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip

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


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            noUserdiv.Visible = False
            userDetail.Visible = True
            txtUserID.Readonly = True
            If String.IsNullOrEmpty(WMS.Logic.GetCurrentUser()) Then
                Response.Write("<script language='javascript'>")
                Response.Write("window.close();")
                'Response.Write(String.Format("parent.location.href = '../{0}';", Made4Net.Shared.Web.M4N_SCREENS.LOGIN))
                Response.Write(String.Format("opener.location.href = '../{0}';", Made4Net.Shared.Web.M4N_SCREENS.LOGIN))
                Response.Write("<" + "/script>")
            End If
            ViewState.Add("activeuserid", WMS.Logic.GetCurrentUser())
            lblNewPassMsg.Visible = False
            lblNewPassMsg.Text = ""
            SetData()
        End If
    End Sub

    Private Sub SetData()
        SetUserDetailData()
        SetUserParamData()
    End Sub

    Private Sub SetUserDetailData()
        Me.TEUserInfo.FilterExpression = String.Format("UserID = '{0}'", ViewState.Item("activeuserid"))
    End Sub

    Private Sub SetUserParamData()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("Select * from userwarehouse where UserID = '{0}' and warehouse='{1}'", ViewState.Item("activeuserid"), WMS.Logic.Warehouse.CurrentWarehouse)
        DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.CONNECTION_NAME)
        If dt.Rows.Count = 0 Then
            'noUserdiv.Visible = True
            'userDetail.Visible = False
            'noUserlbl.Text = "User " & ViewState.Item("activeuserid") & " Not Found"
        Else
            dr = dt.Rows(0)
            Me.txtConsignee.Text = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DEFAULTCONSIGNEE"))

            ddUserWHArea.AllOption = False
            ddUserWHArea.TableName = "vuserwharea"
            ddUserWHArea.ValueField = "wharea"
            ddUserWHArea.TextField = "warehouseareadescription"
            ddUserWHArea.Where = String.Format("userid='{0}'", ViewState.Item("activeuserid"))
            'ddUserWHArea.ConnectionName
            ddUserWHArea.DataBind()
            If Not IsNothing(WMS.Logic.Warehouse.getUserWarehouseArea) Then
                ddUserWHArea.SelectedValue = WMS.Logic.Warehouse.getUserWarehouseArea
            ElseIf Not IsDBNull(dr("DEFAULTWHAREA")) Then
                ddUserWHArea.SelectedValue = dr("DEFAULTWHAREA")
            End If

        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If tbNewPass1.Value = "" Then
            lblNewPassMsg.Visible = True
            lblNewPassMsg.Text = "Enter a new password"
            Exit Sub
        End If

        If tbNewPass2.Value = "" Then
            lblNewPassMsg.Visible = True
            lblNewPassMsg.Text = "Enter the password again to confirm"
            Exit Sub
        End If

        If Not tbNewPass1.Value = tbNewPass2.Value Then
            lblNewPassMsg.Visible = True
            lblNewPassMsg.Text = "The passwords do not match"
            Exit Sub
        End If

        lblNewPassMsg.Visible = False
        Try

            'Made4Net.Shared.Web.User.Update(WMS.Logic.GetCurrentUser(), tbNewPass1.Value, True)
            Made4Net.Shared.Authentication.User.Update(WMS.Logic.GetCurrentUser(), tbNewPass1.Value, False)
        Catch ex As Exception
            lblNewPassMsg.Visible = True
            lblNewPassMsg.Text = ex.Message
        End Try

    End Sub

    Private Sub ddLanguage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddLanguage.SelectedIndexChanged
        bindSkins()
    End Sub

    Private Sub bindSkins()
        Dim sql As String = "Select * from SKINS where SKINNAME in (Select SKINNAME from SKINLANGUAGE where LANGUAGEID = '" & ddLanguage.SelectedValue & "')"
        Dim dt As New DataTable
        Dim dr As DataRow
        ddSkin.Items.Clear()
        ddSkin.AllOption = True
        DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.CONNECTION_NAME)
        For Each dr In dt.Rows
            ddSkin.Items.Add(New ListItem(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKINNAME")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKINNAME"))))
        Next
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim oUser As New WMS.Logic.User(ViewState.Item("activeuserid"))
        oUser.Update(txtFirstName.Text, txtLastName.Text, txtFullName.Text, "", txtScreenid.Text, ddLanguage.SelectedValue, ddSkin.SelectedValue, ViewState.Item("activeuserid"))

        Made4Net.WebControls.SkinManager.ChangeSkin(oUser.SKIN)
        Screen1.SkinManager.ChangeSkin(oUser.SKIN)
        Made4Net.Shared.Translation.Translator.CurrentLanguageID = oUser.LANGUAGE
        RefreshParent()
    End Sub

    Private Sub btnWHAreaChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnWHAreaChange.Click
        WMS.Logic.Warehouse.setUserWarehouseArea(Me.ddUserWHArea.Value)
        RefreshParent()
    End Sub

    Private Sub btnUserParamSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUserParamSave.Click
        Dim SQL As String = String.Format("Update userwarehouse set defaultconsignee='{0}',defaultwharea='{1}' where userid='{2}' and warehouse='{3}'", Me.txtConsignee.Text, Me.ddUserWHArea.Value, ViewState.Item("activeuserid"), WMS.Logic.Warehouse.CurrentWarehouse)
        DataInterface.RunSQL(SQL, Made4Net.Schema.CONNECTION_NAME)
        RefreshParent()
    End Sub

    Private Sub RefreshParent()
        Response.Write("<script language=""javascript"">" & vbCrLf)
        Response.Write("var doBlur=1;")
        Response.Write("window.opener.location = window.opener.location;" & vbCrLf)
        'Response.Write("this.window.focus();" & vbCrLf)
        'Response.Write("window.location = window.location;" & vbCrLf)
        'Response.Write("window.toFront();" & vbCrLf)

        Response.Write("</script>" & vbCrLf)
    End Sub

    Private Sub TEUserInfo_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEUserInfo.AfterItemCommand
        If e.CommandName.ToLower.Equals("save") Then
            Dim oUser As New WMS.Logic.User(ViewState.Item("activeuserid"))
            Dim oldSkin As String = Made4Net.WebControls.SkinManager.CurrentSkin
            Dim oldlang As Integer = Made4Net.Shared.Translation.Translator.CurrentLanguageID
            If (Not oldSkin.Equals(oUser.SKIN)) Or oldlang <> oUser.LANGUAGE Then
                Made4Net.WebControls.SkinManager.ChangeSkin(oUser.SKIN)
                Screen1.SkinManager.ChangeSkin(oUser.SKIN)
                Made4Net.Shared.Translation.Translator.CurrentLanguageID = oUser.LANGUAGE
                RefreshParent()
            End If
        End If
    End Sub

    Private Sub TS_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TS.SelectedIndexChange

    End Sub
End Class
