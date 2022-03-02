Imports System.Web.UI.WebControls
Imports System.EventArgs

Imports Made4Net.WebControls
Imports WMS.Logic

'Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class LoginWH
    Inherits PWMSBaseLogger

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents WarehouseSelectBox As System.Web.UI.WebControls.DropDownList
    Protected WithEvents Submit As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim SQL As String = "SELECT * FROM USERWAREHOUSE WHERE USERID='" & Logic.GetCurrentUser & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt, False, "Made4NetSchema")
            '' start Commented for labor phase 2 - RWMS-1667 since the clock in and clock out newly implemented in future phase
            'If Not ShiftInstance.checkUserClockinShift(Logic.GetCurrentUser) And WMS.Logic.GetSysParam("AutoClockinonLogin") = "1" Then
            '	ShiftInstance.ClockUser(Logic.GetCurrentUser, WMS.Lib.Shift.ClockStatus.IN, "")
            'End If
            ''End Commented for labor phase 2 - RWMS-1667 since the clock in and clock out newly implemented in future phase
            If dt.Rows.Count = 0 Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreen/Login.aspx"))
            ElseIf dt.Rows.Count = 1 Then
                Warehouse.setCurrentWarehouse(dt.Rows(0)("warehouse"))
                Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
                If Not MobileUtils.MultiLogIn(Application("Made4NetLicensing_ApplicationId")) Then
                    Response.Redirect(MobileUtils.GetUrlWithParams(True))
                    Exit Sub
                End If
                If Session("MHType") = "" Or Session("LoginWHArea") = "" Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
                End If
            Else

                Dim sqlWarehousesNames As String = "SELECT * FROM vUSERWAREHOUSE WHERE USERID='" & Logic.GetCurrentUser & "'"
                Dim warehousesDataTables As New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset(sqlWarehousesNames, warehousesDataTables, False, "Made4NetSchema")

                WarehouseSelectBox.DataSource = warehousesDataTables
                WarehouseSelectBox.DataValueField = "WAREHOUSE"
                WarehouseSelectBox.DataTextField = "WAREHOUSENAME"
                WarehouseSelectBox.DataBind()
            End If

            dt.Dispose()

        End If
    End Sub

    'Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
    '	DO1.AddDropDown("WHLIST")
    'End Sub

    'Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
    '	If e.CommandText.ToLower = "warehouseselect" Then
    '		Warehouse.setCurrentWarehouse(DO1.Value("WHLIST"))
    '		Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection


    '		':2013-04-28
    '		'if MultiDeviceLogin = allow or block
    '		'If it is set to “Allow”, login process will continue normally.
    '		'if whSQL = "0" then MultiDeviceLogin =  block
    '		'If it is set to “Block”, the system will check if the user already exists in the warehouse activity table (WHACTIVITY).
    '		'If true, the system will display and error message saying “user already logged in on another device” and return to the login screen.
    '		If Not MobileUtils.MultiLogIn(Application("Made4NetLicensing_ApplicationId")) Then
    '			'  HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("User already logged in on another device"))
    '			'  HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "User already logged in on another device")
    '			Response.Redirect(MobileUtils.GetUrlWithParams(True))

    '			Exit Sub
    '		End If

    '		If Session("MHType") = "" Or Session("LoginWHArea") = "" Then
    '			Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
    '		Else
    '			Dim oWHActivity As New WMS.Logic.WHActivity
    '			oWHActivity.ACTIVITY = "Login"
    '			oWHActivity.LOCATION = Session("LoginLocation")
    '			oWHActivity.WAREHOUSEAREA = Session("LoginWHArea")
    '			Warehouse.setUserWarehouseArea(Session("LoginWHArea"))
    '			oWHActivity.USERID = Logic.GetCurrentUser
    '			oWHActivity.HETYPE = Session("MHType")
    '			oWHActivity.ACTIVITYTIME = DateTime.Now
    '			oWHActivity.ADDDATE = DateTime.Now
    '			oWHActivity.EDITDATE = DateTime.Now
    '			oWHActivity.ADDUSER = Logic.GetCurrentUser
    '			oWHActivity.EDITUSER = Logic.GetCurrentUser
    '			oWHActivity.HANDLINGEQUIPMENTID = Session("MHEID")
    '			oWHActivity.TERMINALTYPE = Session("TERMINALTYPE")
    '			Session("ActivityID") = oWHActivity.Post()

    '			'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Main.aspx"))
    '			Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("M4NScreens/LoginSaCh.aspx"))
    '		End If
    '		'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
    '	End If
    'End Sub

    Private Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        Warehouse.setCurrentWarehouse(WarehouseSelectBox.SelectedItem.Text)
        Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection

        If Not MobileUtils.MultiLogIn(Application("Made4NetLicensing_ApplicationId")) Then
            Response.Redirect(MobileUtils.GetUrlWithParams(True))
            Exit Sub
        End If

        If Session("MHType") = "" Or Session("LoginWHArea") = "" Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
        Else
            Dim oWHActivity As New WMS.Logic.WHActivity
            oWHActivity.ACTIVITY = "Login"
            oWHActivity.LOCATION = Session("LoginLocation")
            oWHActivity.WAREHOUSEAREA = Session("LoginWHArea")
            Warehouse.setUserWarehouseArea(Session("LoginWHArea"))
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
            [Global].userSessionManager.AddSession(GetCurrentUser, HttpContext.Current.Session.SessionID, HttpContext.Current.Request.UserHostAddress, Session("MHType"), Session("MHEID"), LogHandler.GetRDTLogger())
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("M4NScreens/LoginSaCh.aspx"))
        End If
    End Sub

End Class