Imports System.Web.UI.WebControls
Imports System.EventArgs
Imports Made4Net.DataAccess

'Added for RWMS-2540 Start
Imports WMS.Logic
'Added for RWMS-2540 End

<CLSCompliant(False)> Public Class LoginHQT
    Inherits PWMSBaseLogger

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
	''Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject
	Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
	Protected WithEvents MaterialHandlingEquipmentId As System.Web.UI.WebControls.TextBox
	Protected WithEvents MaterialHandlingType As System.Web.UI.WebControls.DropDownList
	Protected WithEvents TerminalType As System.Web.UI.WebControls.DropDownList
	Protected WithEvents WarehouseArea As System.Web.UI.WebControls.DropDownList
	Protected WithEvents Location As System.Web.UI.WebControls.TextBox
	Protected WithEvents Submit As System.Web.UI.WebControls.Button

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object
	Public Property Printer As Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		If Not IsPostBack Then

			Dim selectBoxQuery As String

			''Set the MHEID drop down
			''DO1.Value("MHEID") = ""
			''Dim dd As Made4Net.WebControls.MobileDropDown
			''dd = DO1.Ctrl("MHType")
			''dd.AllOption = False
			''dd.TableName = "HANDLINGEQUIPMENT"
			''dd.ValueField = "HANDLINGEQUIPMENT"
			''dd.TextField = "HANDLINGEQUIPMENT"
			''dd.DataBind()

			'Set the MaterialHandlingType drop down
			 selectBoxQuery = "SELECT * FROM HANDLINGEQUIPMENT"
			 Dim selectBoxResults_MaterialHandlingType As New DataTable
			Made4Net.DataAccess.DataInterface.FillDataset(selectBoxQuery, selectBoxResults_MaterialHandlingType, False)
			MaterialHandlingType.DataSource = selectBoxResults_MaterialHandlingType
			 MaterialHandlingType.DataValueField = "HANDLINGEQUIPMENT"
			 MaterialHandlingType.DataTextField = "HANDLINGEQUIPMENT"
			 MaterialHandlingType.DataBind()

			''Set the Terminal Type drop down
			''Dim dd1 As Made4Net.WebControls.MobileDropDown
			''dd1 = DO1.Ctrl("TEMINALTYPE")
			''dd1.AllOption = False
			''dd1.TableName = "CODELISTDETAIL"
			''dd1.ValueField = "CODE"
			''dd1.TextField = "DESCRIPTION"
			''dd1.Where = "CODELISTCODE = 'TERMINALTYPE'"
			''dd1.DataBind()

			'Set the TerminalType drop down
			selectBoxQuery = "SELECT * FROM CODELISTDETAIL WHERE CODELISTCODE = 'TERMINALTYPE'"
			Dim selectBoxResults_TerminalType As New DataTable
			Made4Net.DataAccess.DataInterface.FillDataset(selectBoxQuery, selectBoxResults_TerminalType, False)
			TerminalType.DataSource = selectBoxResults_TerminalType
			TerminalType.DataValueField = "CODE"
			TerminalType.DataTextField = "DESCRIPTION"
			TerminalType.DataBind()

			''Set the warehouse area drop down
			''Dim dd2 As Made4Net.WebControls.MobileDropDown
			''dd2 = DO1.Ctrl("WAREHOUSEAREA")
			''dd2.AllOption = False
			''dd2.TableName = "USERWHAREA"
			''dd2.ValueField = "WHAREA"
			''dd2.TextField = "WHAREA"
			''dd2.DataBind()

			'Set the WarehouseArea drop down
            selectBoxQuery = "SELECT DISTINCT(WHAREA) FROM USERWHAREA"
			Dim selectBoxResults_WarehouseArea As New DataTable
			Made4Net.DataAccess.DataInterface.FillDataset(selectBoxQuery, selectBoxResults_WarehouseArea, False)
			WarehouseArea.DataSource = selectBoxResults_WarehouseArea
			WarehouseArea.DataValueField = "WHAREA"
			WarehouseArea.DataTextField = "WHAREA"
			WarehouseArea.DataBind()

			' RWMS-1270 : Bind should be called before setting the selected value, otherwise the wrong warehouse will remain selected.
			Try
				WarehouseArea.SelectedValue = GetUserDefaultWHArea()
				If Printer.Text = String.Empty Then
					Printer.Text = GetUserDefaultPrinter()
				End If
			Catch ex As Exception
			End Try
			' RWMS-1270 : Bind should be called before setting the selected value, otherwise the wrong warehouse will remain selected.
		End If
        'Added for RWMS-763
		''If Not DO1.Value("MHEID") = "" Then
		If Not MaterialHandlingType.SelectedValue = "" Then
			''DO1.FocusField = "Location"
			Location.Focus()
		End If
		'End Added for RWMS-763
    End Sub
    ' RWMS-1270 : Default WareHouse area should be qualified by the user selected warehouse
    Private Function GetUserDefaultWHArea() As String
        Dim Sql As String = ""
        If Session("Warehouse_CurrentWarehouseId") IsNot Nothing Then
            Sql = String.Format("select isnull(DEFAULTWHAREA,'') as DEFAULTWHAREA from userwarehouse where userid = '{0}' AND WAREHOUSE='{1}'", WMS.Logic.GetCurrentUser, Session("Warehouse_CurrentWarehouseId"))
        Else
            Sql = String.Format("select isnull(DEFAULTWHAREA,'') as DEFAULTWHAREA from userwarehouse where userid = '{0}'", WMS.Logic.GetCurrentUser)
        End If

        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql, "Made4NetSchema")
    End Function
	Private Function GetUserDefaultPrinter() As String
		Dim Sql As String = ""
		Sql = String.Format("select isnull(DEFAULTPRINTER,'') as DEFAULTPRINTER from UserProfile where userid = '{0}'", WMS.Logic.GetCurrentUser)
		Return Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql, "Made4NetSchema")
	End Function
	''Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
	''	'Commented for RWMS-763
	''	'DO1.AddTextboxLine("MHEID")
	''	'End Commented for RWMS-763
	''	DO1.AddTextboxLine("MHEID", True, "mhtypeselect")
	''	DO1.AddDropDown("MHType")
	''	DO1.AddDropDown("TEMINALTYPE")
	''	DO1.AddDropDown("WAREHOUSEAREA")
	''	DO1.AddTextboxLine("Location")
	''End Sub

	''Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
	Private Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
		'Added for RWMS-763
		''DO1.FocusField = "MHEID"
		''Select Case e.CommandText.ToLower

		'' [ ! ] Need to check with Srini where do the "scan" and mhtypeselect" values should come from
		Dim submitButton As Button = sender
		''Select Case submitButton.Text.ToLower
		''	Case "scan"
		''		doScan()
		''	Case "mhtypeselect"
				doMhtypeselect()
		''End Select

	End Sub
Private Function ValidateWHAreaLocation() As Boolean
		''If DO1.Value("WAREHOUSEAREA") = String.Empty Then
		If WarehouseArea.Text = String.Empty Then
			Return False
		End If
		''If DO1.Value("Location") <> String.Empty Then
		If Location.Text <> String.Empty Then
			'Validate that the location is a valid location in selected wharea
			''Dim Sql As String = String.Format("select isnull(warehousearea,'') from location where location = '{0}'", DO1.Value("Location"))
			Dim Sql As String = String.Format("select isnull(warehousearea,'') from location where location = '{0}'", Location.Text)
			Dim wharea As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
			If wharea Is Nothing Then
				Return False
			End If
			If wharea.ToLower <> WarehouseArea.Text.ToLower Then
				Return False
			End If
		End If
		Return True
	End Function
	Private Function ValidateDefaultPrinter() As Boolean
		''If DO1.Value("WAREHOUSEAREA") = String.Empty Then
		If Printer.Text = String.Empty Then
			Return True
		End If

		If Printer.Text <> String.Empty Then
			'Validate that the location is a valid location in selected wharea
			''Dim Sql As String = String.Format("select isnull(warehousearea,'') from location where location = '{0}'", DO1.Value("Location"))
			Dim Sql As String = String.Format("select isnull(PrintQName,'') from LabelPrinters where PrintQName = '{0}'", Printer.Text)
			Dim labelPrinter As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
			If labelPrinter Is Nothing Then
				Return False
			End If
			If labelPrinter.ToLower <> Printer.Text.ToLower Then
				Return False
			End If
		End If
		Return True
	End Function
	'Added for RWMS-763
#Region "doScan()"
	Private Sub doScan()
		If Not ValidateMHEID() Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Invalid MHEID")
            Return
        End If
        ''Dim MHType As String = GetMHEMHType(DO1.Value("MHEID"))
        Dim MHType As String = GetMHEMHType(MaterialHandlingEquipmentId.Text)
        ''DO1.Value("MHType") = MHType
        '' [ ! ] Need to check why the following is needed (it's casuing a multiple results error)
        ''MaterialHandlingType.Items.FindByValue(MHType).Selected = True
        '' [ ! ] Need to check if the default-button here has any significance
        ''DO1.DefaultButton = "mhtypeselect"

        ''If Not DO1.Value("MHEID") = "" Then
        ''	DO1.FocusField = "Location"
        ''End If
        If Not MaterialHandlingEquipmentId.Text = "" Then
            Location.Focus()
        End If


    End Sub
#End Region

#Region "doMhtypeselect()"
    Private Sub doMhtypeselect()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        ' Make location mandatory - Depends on the Settings
        ' Labor calc changes RWMS-952
        If WMS.Logic.WarehouseParams.GetWarehouseParam("LocMandatoryOnLogin") = "1" Then
            ''If String.IsNullOrEmpty(DO1.Value("Location").Trim()) Then
            If String.IsNullOrEmpty(Location.Text.Trim()) Then
                'RWMS-1937 Start
                ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Please provide Location since it is mandatory while login."))
                ' ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('Please provide Location since it is mandatory while login.');", True)
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Please provide Location since it is mandatory while login.;")
                'RWMS-1937 End
                Return
            End If
        End If
        ' Labor calc changes RWMS-952

        'Added for RWMS-763
        If Not ValidateMHEID() Then
            'RWMS-1937 Start
            '  HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Invalid MHEID")
            ' ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('Invalid MHEID');", True)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Invalid MHEID")
            'RWMS-1937 End
            Return
        End If
        If Not ValidateMHEType() Then
            Return
        End If
        'End Added for RWMS-763
        If Not ValidateWHAreaLocation() Then
            'RWMS-1937 Start
            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "User must be assigned to warehouse area / Location must be a valid warehouse area location")
            ' ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('User must be assigned to warehouse area / Location must be a valid warehouse area location');", True)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "User must be assigned to warehouse area / Location must be a valid warehouse area location")
            'RWMS-1937 End
            Return
        End If
		If Not ValidateDefaultPrinter() Then
			HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "User must be assigned to Default Printer / Default Printer must be a valid Printer Name")
			'RWMS-1937 End
			Return
		End If

		Session("MHEID") = MaterialHandlingEquipmentId.Text ''DO1.Value("MHEID")
        Session("MHType") = MaterialHandlingType.Text ''DO1.Value("MHType")
        Session("MHEIDLabelPrinter") = WMS.MobileWebApp.MobileUtils.GetMHEDefaultPrinter(MaterialHandlingEquipmentId.Text) ''(DO1.Value("MHEID"))
        Session("LoginWHArea") = WarehouseArea.Text ''DO1.Value("WAREHOUSEAREA")
        WMS.Logic.Warehouse.setUserWarehouseArea(WarehouseArea.Text) ''(DO1.Value("WAREHOUSEAREA"))
        Session("UserSelectedLoginLocation") = Location.Text ''DO1.Value("Location")
		Session("TERMINALTYPE") = TerminalType.Text ''DO1.Value("TEMINALTYPE")
		Session("DefaultPrinter") = Printer.Text

		Dim oWHActivity As New WMS.Logic.WHActivity
        oWHActivity.ACTIVITY = "Login"
        'RWMS-2742 Commented
        'oWHActivity.LOCATION = Session("LoginLocation")
        'RWMS-2742 Commented END
        'RWMS-2742 START
        If Session("LoginLocation") <> "" Then
            oWHActivity.LOCATION = Session("LoginLocation")
        Else
            oWHActivity.LOCATION = Session("UserSelectedLoginLocation")
        End If
        'RWMS-2742 END
        oWHActivity.WAREHOUSEAREA = WarehouseArea.Text ''DO1.Value("WAREHOUSEAREA")
        WMS.Logic.Warehouse.setUserWarehouseArea(WarehouseArea.Text) ''(DO1.Value("WAREHOUSEAREA"))
        oWHActivity.USERID = Logic.GetCurrentUser
        oWHActivity.HETYPE = MaterialHandlingType.Text ''DO1.Value("MHType")
        oWHActivity.HANDLINGEQUIPMENTID = MaterialHandlingEquipmentId.Text
        oWHActivity.TERMINALTYPE = TerminalType.Text
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.ADDDATE = DateTime.Now
        oWHActivity.EDITDATE = DateTime.Now
        oWHActivity.ADDUSER = Logic.GetCurrentUser
		oWHActivity.EDITUSER = Logic.GetCurrentUser
		oWHActivity.PRINTER = Session("DefaultPrinter")
		Session("ActivityID") = oWHActivity.Post()
		MobileUtils.UpdateDPrinterInUserProfile(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
		Session("MHEID") = MaterialHandlingEquipmentId.Text
        Session("MHType") = MaterialHandlingType.Text
        Session("MHEIDLabelPrinter") = WMS.MobileWebApp.MobileUtils.GetMHEDefaultPrinter(MaterialHandlingEquipmentId.Text)

        'Added for RWMS-2540 Start
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LOGIN)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOGIN)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("MHEID", Session("MHEID"))
        aq.Add("TERMINALTYPE", Session("TERMINALTYPE"))
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Send(WMS.Lib.Actions.Audit.LOGIN)
        'Added for RWMS-2540 End
        [Global].userSessionManager.AddSession(GetCurrentUser, HttpContext.Current.Session.SessionID, HttpContext.Current.Request.UserHostAddress, MaterialHandlingType.Text, Session("MHEID"), LogHandler.GetRDTLogger())
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginSaCH.aspx"))
    End Sub
#End Region

	Private Function ValidateMHEID() As Boolean

		If MaterialHandlingEquipmentId.Text = String.Empty Then
			MaterialHandlingEquipmentId.Focus()	''DO1.FocusField = "MHEID"
			Return False
		Else
			Dim Sql As String = String.Format("select isnull(MHEID,'') from MHE where MHEID = '{0}'", MaterialHandlingEquipmentId.Text)
			Dim MHEID As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
			If MHEID Is Nothing Then
				Return False
			End If
		End If

		Return True
	End Function

	Private Function ValidateMHEType() As Boolean

		If MaterialHandlingEquipmentId.Text = String.Empty Then
			MaterialHandlingEquipmentId.Focus()
			Return False
		Else
			Dim MHType As String = GetMHEMHType(MaterialHandlingEquipmentId.Text)
			If Not MaterialHandlingType.Text = "" Then
				If Not MHType = MaterialHandlingType.Text Then
					'' [ ! ] Need to check if the Default Button is necessary here
					''DO1.DefaultButton = "mhtypeselect"
					MaterialHandlingType.Text = MHType
					Location.Focus()
					Return False
				End If
			End If
		End If

		Return True
	End Function

	Public Function GetMHEMHType(Optional ByVal pMHEId As String = "") As String
		Dim MHType As String = ""

		MHType = DataInterface.ExecuteScalar(String.Format("SELECT isnull(MHETYPE,'') as MHETYPE FROM MHE WHERE MHEID = '{0}'", pMHEId))

		Return MHType
	End Function
	'End Added for RWMS-763

End Class