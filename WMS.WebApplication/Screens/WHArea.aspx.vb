Imports WMS.Logic
Imports made4net.Shared
Imports made4net.DataAccess

Partial Public Class WHArea
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
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "save"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oUser As New WMS.Logic.User(dr("Userid"))
                    oUser.AssignToWarehouseArea(Session("TEAssignUsersCurrentWHAreaCode"))
                Next
            Case "unassign"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oUser As New WMS.Logic.User(dr("Userid"))
                    oUser.UnAssignFromWarehouseArea(dr("WHAREA"))
                Next
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TEUSERWHAREA_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEUSERWHAREA.AfterItemCommand
        TEUSERWHAREA.RefreshData()
    End Sub

    Private Sub TEUSERWHAREA_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUSERWHAREA.CreatedChildControls

        TEUSERWHAREA.ActionBar.AddExecButton("unassign", "UnAssign", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEUSERWHAREA.ActionBar.Button("unassign")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.WHArea"
            .CommandName = "unassign"
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .ConfirmRequired = False
        End With

        Dim vals As New Specialized.NameValueCollection
        vals.Add("WHAREA", TEWHAREA.SelectedRecordPrimaryKeyValues("WarehouseAreacode"))
        TEUSERWHAREA.PreDefinedValues = vals

    End Sub

    Private Sub TEAssignUsers_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAssignUsers.AfterItemCommand
        TEAssignUsers.Restart()
    End Sub

    Private Sub TEAssignUsers_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignUsers.CreatedChildControls
        TEAssignUsers.ActionBar.AddExecButton("assignusers", "Assign Users", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignUsers.ActionBar.Button("assignusers")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.WHArea"
            .CommandName = "save"
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .ConfirmRequired = False
        End With

        Dim sql As String = String.Format("select distinct userid from userwharea where wharea ='{0}'", _
            TEWHAREA.SelectedRecordPrimaryKeyValues("WarehouseAreacode"))
        Dim dt As New DataTable()
        DataInterface.FillDataset(sql, dt)
        Dim userList As String = ""
        For Each dr As DataRow In dt.Rows
            userList += "'" + dr("UserID").ToString() + "',"
        Next
        userList = userList.TrimEnd(",")

        If userList = "" Then userList = "''"
        TEAssignUsers.FilterExpression = String.Format("userid not in ({0})", userList)
        'Dim vals As New Specialized.NameValueCollection
        'vals.Add("WHAREA", TEWHAREA.SelectedRecordPrimaryKeyValues("WarehouseAreacode"))
        'TEAssignUsers.PreDefinedValues = vals
        Session("TEAssignUsersCurrentWHAreaCode") = TEWHAREA.SelectedRecordPrimaryKeyValues("WarehouseAreacode")
    End Sub
End Class