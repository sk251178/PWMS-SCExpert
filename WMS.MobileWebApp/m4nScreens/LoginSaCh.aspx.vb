Imports System.Web.UI.WebControls
Imports System.EventArgs

Imports Made4Net.WebControls
Imports WMS.Logic

Partial Public Class LoginSaCh
    Inherits PWMSBaseLogger

    ''Protected WithEvents DO1 As Global.Made4Net.Mobile.WebCtrls.DataObject

    Protected WithEvents SafetyCheck As System.Web.UI.WebControls.DropDownList
    Protected WithEvents SubmitNext As System.Web.UI.WebControls.Button
    Protected WithEvents SubmitPrev As System.Web.UI.WebControls.Button

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            If Session("MHType") = "" Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
            End If
            ViewState.Add("shiftSet", True)

            '' [ ! ] Temporary disabled so we may proceed coding
            ''Try
            ''	If Not MobileUtils.IsSafetyCheckRequiredForMHE(Session("MHType")) Then
            ''		RedirectToMain()
            ''	End If
            ''Catch ex As Exception
            ''	ViewState.Add("shiftSet", False)
            ''End Try


            ''Dim dd As Made4Net.WebControls.MobileDropDown
            ''dd = DO1.Ctrl("SafetyChecked")
            ''dd.ConnectionName = "Default"
            ''dd.AllOption = False
            ''dd.TableName = "CodeListDetail"
            ''dd.ValueField = "Code"
            ''dd.TextField = "Description"
            ''dd.Where = "codelistcode='yesno'"
            ''dd.DataBind()

            'Set the SafetyCheck drop down
            Dim selectBoxQuery As String

            selectBoxQuery = "SELECT * FROM CodeListDetail WHERE codelistcode='yesno'"
            Dim selectBoxResults As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(selectBoxQuery, selectBoxResults, False)
            SafetyCheck.DataSource = selectBoxResults
            SafetyCheck.DataValueField = "Code"
            SafetyCheck.DataTextField = "Description"
            SafetyCheck.DataBind()

        End If

    End Sub

    ''Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
    ''    DO1.AddDropDown("SafetyChecked")
    ''End Sub

    Private Sub RedirectToMain()
        Dim oWHActivity As New WMS.Logic.WHActivity
        oWHActivity.ACTIVITY = "Login"
        oWHActivity.LOCATION = Session("UserSelectedLoginLocation")
        oWHActivity.WAREHOUSEAREA = Session("LoginWHArea")
        WMS.Logic.Warehouse.setUserWarehouseArea(Session("LoginWHArea"))
        oWHActivity.USERID = Logic.GetCurrentUser
        oWHActivity.HETYPE = Session("MHType")
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.ADDDATE = DateTime.Now
        oWHActivity.EDITDATE = DateTime.Now
        oWHActivity.ADDUSER = Logic.GetCurrentUser
        oWHActivity.EDITUSER = Logic.GetCurrentUser
        oWHActivity.HANDLINGEQUIPMENTID = Session("MHEID")
        oWHActivity.TERMINALTYPE = Session("TERMINALTYPE")

        Session("ActivityID") = oWHActivity.Post()
        '' [ ! ] Temporary disabled just so we may proceed coding
        ''MobileUtils.UpdateUserProfilesShiftIDMHE(Session("MHType"))
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Main.aspx"))
    End Sub

    '' Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
    Private Sub SubmitNext_Click(sender As Object, e As EventArgs) Handles SubmitNext.Click
        '' [ ! ] Need to check if shiftset is required.
        ''If ViewState("shiftSet") Then
        If SafetyCheck.Text.ToLower = "yes" Then
            RedirectToMain()
            'Added for RWMS-2287 Start
        Else
            ' ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('You must make sure your handling Equipment is safe in order to continue');", True)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "You must make sure your handling Equipment is safe in order to continue")
            Return
            'Added for RWMS-2287 End
        End If
        'Commented for RWMS-2287 Start
        ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "You must make sure your handling Equipment is safe in order to continue.")
        'Commented for RWMS-2287 End
        ''Else
        ''	 HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Shift is not set. Can not login.")
        ''End If
    End Sub

    Private Sub SubmitPrev_Click(sender As Object, e As EventArgs) Handles SubmitPrev.Click
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/loginhqt.aspx"))
    End Sub
End Class