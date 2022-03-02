Imports System.Web.UI.WebControls
Imports System.EventArgs

Imports Made4Net.WebControls
Imports WMS.Logic

<CLSCompliant(False)> Public Class Login
    Inherits PWMSRDTBase


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ''<CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
    ''Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    Protected WithEvents UserName As System.Web.UI.WebControls.TextBox
    Protected WithEvents Password As System.Web.UI.WebControls.TextBox
    Protected WithEvents Submit As System.Web.UI.WebControls.Button

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
            Made4Net.Mobile.MessageQue.Clear()
            Dim lang As Int32 = WMS.Logic.GetSysParam("DEFAULTLANGUAGE")
            Made4Net.Shared.Translation.Translator.CurrentLanguageID = lang
            ''ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:UserIdval:tb"))

            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            If Not IsNothing(HttpContext.Current.Session("ErrMultiLogIn")) Then
                If HttpContext.Current.Session("ErrMultiLogIn") = 1 Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("User already logged in on another device"))
                End If
                HttpContext.Current.Session.Remove("ErrMultiLogIn")
            End If
            'If Request.QueryString("ErrMultiLogIn") <> "" Then

            '     HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("User already logged in on another device"))
            '    'Response.Redirect(MobileUtils.GetUrlWithParams(False))

            'End If
        End If
        If Request.QueryString("Skin") <> "" Then
            Made4Net.Mobile.MobileSkinManager.ChangeSkin(Request.QueryString("Skin"))
            Session("Skin") = Request.QueryString("Skin")
        ElseIf Session("Skin") <> "" Then
            Made4Net.Mobile.MobileSkinManager.ChangeSkin(Session("Skin"))
        Else
            Try
                Dim skin As String = WMS.Logic.GetSysParam("DEFAULTSKIN")
                Made4Net.Mobile.MobileSkinManager.ChangeSkin(skin)
            Catch ex As Exception
            End Try
        End If
    End Sub

    'Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
    '    DO1.AddTextboxLine("UserId", True, "login")
    '    DO1.AddTextboxLine("Password", True, "login", "Password", "", True)
    'End Sub


    'Private Sub Submit_Click(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles Submit.Click
    '    Dim MoveToMain As Boolean = False
    '    Dim MoveToWH As Boolean = False
    '    Dim MoveToMHType As Boolean = False
    '    Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


    '    If e.CommandText.ToLower = "login" Then
    '        Try

    '            Try
    '                Made4Net.Shared.Web.User.Login(UserName.Text, Password.Text)
    '            Catch ex As Exception
    '                 HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
    '                Return
    '            End Try

    '            Dim whSQL As String
    '            'Code for 3PL sesion
    '            Dim sql3PL As String
    '            Dim consigneeSQL As String
    '            sql3PL = "SELECT PARAM_VALUE FROM sys_param WHERE param_code='3PL'"
    '            Session("3PL") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql3PL, "Made4NetSchema")
    '            If Session("3PL") = 0 Then
    '                consigneeSQL = "SELECT TOP 1 CONSIGNEE FROM CONSIGNEE"
    '                Session("consigneeSession") = Made4Net.DataAccess.DataInterface.ExecuteScalar(consigneeSQL)
    '            End If
    '            'End 3PL Session

    '            'RWMS-1025 RDT Logging Initializing- if session(RDTAppLogging) is true then we will create logs for the RDT application
    '            Dim rdtLogHandler As New RDTLogHandler()
    '            rdtLogHandler.CheckAndInitiateLogging(Logic.GetCurrentUser)
    '            'End RDT Logging

    '            'Made4Net.Shared.Web.User.Login(DO1.Value("UserId"), DO1.Value("Password"))
    '            whSQL = "SELECT * FROM USERWAREHOUSE WHERE USERID='" & Logic.GetCurrentUser & "' AND WAREHOUSE='" & Request.QueryString("WH") & "'"
    '            Dim dt As New DataTable
    '            Made4Net.DataAccess.DataInterface.FillDataset(whSQL, dt, False, "Made4NetSchema")
    '            'Dim dr As DataRow
    '            If Session("WH") = "" Then
    '                If dt.Rows.Count = 0 Then
    '                    Session("WH") = ""
    '                    MoveToWH = True
    '                Else
    '                    Session("WH") = Request.QueryString("WH")
    '                    MoveToWH = False
    '                End If
    '            Else
    '                MoveToWH = False
    '            End If
    '            MoveToMHType = SetMHEID()
    '            If MoveToMHType Or MoveToWH Then
    '                MoveToMain = False
    '            Else
    '                MoveToMain = True
    '            End If
    '        Catch ex As Made4Net.Shared.M4NException
    '             HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
    '            MoveToMHType = True
    '        Catch ex As ApplicationException
    '             HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
    '            MoveToMHType = True
    '        End Try
    '        Dim _user As New WMS.Logic.User(UserName.Text)
    '        Try
    '            Session.Timeout = Convert.ToInt32(WMS.Logic.GetSysParam("RDTLOGOFFTIME"))
    '        Catch ex As Exception
    '        End Try
    '        Dim lang As Int32
    '        Try
    '            lang = Int32.Parse(_user.LANGUAGE)
    '        Catch ex As Exception
    '        End Try
    '        Made4Net.Shared.Translation.Translator.CurrentLanguageID = lang
    '        If MoveToWH Then
    '            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginWH.aspx"))
    '        End If
    '        If MoveToMHType Then
    '            Warehouse.setCurrentWarehouse(Session("WH"))
    '            Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
    '            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
    '        End If

    '        If MoveToMain Then
    '            Warehouse.setCurrentWarehouse(Session("WH"))
    '            Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
    '            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nscreens/LoginSaCh.aspx"))
    '            'Add the Login as an activity of the user
    '            Dim oWHActivity As New WMS.Logic.WHActivity
    '            oWHActivity.ACTIVITY = "Login"
    '            oWHActivity.LOCATION = Session("LoginLocation")
    '            oWHActivity.WAREHOUSEAREA = Session("LoginWHArea")
    '            Warehouse.setUserWarehouseArea(Session("LoginWHArea"))
    '            oWHActivity.USERID = UserName.Text
    '            oWHActivity.HETYPE = Session("MHType")
    '            oWHActivity.ACTIVITYTIME = DateTime.Now
    '            oWHActivity.ADDDATE = DateTime.Now
    '            oWHActivity.EDITDATE = DateTime.Now
    '            oWHActivity.ADDUSER = UserName.Text
    '            oWHActivity.EDITUSER = UserName.Text
    '            oWHActivity.HANDLINGEQUIPMENTID = Session("MHEID")
    '            oWHActivity.TERMINALTYPE = Session("TERMINALTYPE")
    '            Session("ActivityID") = oWHActivity.Post()
    '            'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Main.aspx"))
    '        Else
    '            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginWH.aspx"))
    '        End If
    '    Else
    '        Response.Redirect(RemoveSessionValues())
    '    End If
    '    Dim userid, appId, sessionid, ipAddress, conn As String
    '    appId = Application("Made4NetLicensing_ApplicationId")

    'End Sub

    Private Function RemoveSessionValues() As String
        Dim ret, urlparams As String
        Dim skin, wh As String ', mheid, mhtype, terminaltype As String
        skin = Session("Skin")
        wh = Session("WH")
        'mheid = Session("MHEID")
        'mhtype = Session("MHTYPE")
        'terminaltype = Session("TERMINALTYPE")
        Session.Remove("MHEID")
        Session.Remove("MHTYPE")
        Session.Remove("TERMINALTYPE")
        Session.Remove("RDTLogger")
        urlparams = "?"
        If skin <> String.Empty Then
            urlparams = urlparams & String.Format("Skin={0}&", skin)
        End If
        If wh <> String.Empty Then
            urlparams = urlparams & String.Format("WH={0}&", wh)
        End If
        'If mheid <> String.Empty Then
        '    urlparams = urlparams & String.Format("MHEID={0}&", mheid)
        'End If
        'If mhtype <> String.Empty Then
        '    urlparams = urlparams & String.Format("MHTYPE={0}&", mhtype)
        'End If
        'If terminaltype <> String.Empty Then
        '    urlparams = urlparams & String.Format("TERMINALTYPE={0}&", terminaltype)
        'End If
        ret = Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx") & urlparams.TrimEnd("&".ToCharArray())
        Return ret
    End Function

    Private Function SetMHEID() As Boolean
        If Session("TERMINALTYPE") = "" Then
            If Request.QueryString("TERMINALTYPE") = "" Then
                Session("TERMINALTYPE") = ""
            Else
                Session("TERMINALTYPE") = Request.QueryString("TERMINALTYPE")
            End If
        End If
        Dim MoveToMHType As Boolean
        If Session("MHEID") = "" Then
            If Request.QueryString("MHEID") = "" Then
                Session("MHEID") = ""
                MoveToMHType = True
            Else
                Session("MHEID") = Request.QueryString("MHEID")
                MoveToMHType = False
                SetMHType(Session("MHEID"))
                Return MoveToMHType
            End If
        Else
            MoveToMHType = False
            SetMHType(Session("MHEID"))
            Return MoveToMHType
        End If
        If Session("MHType") = "" Then
            If Request.QueryString("MHTYPE") = "" Then
                Session("MHType") = ""
                MoveToMHType = True
            Else
                Session("MHType") = Request.QueryString("MHTYPE")
                MoveToMHType = False
            End If
        Else
            MoveToMHType = False
        End If
        'Try
        SetLocationAndWarehouseArea()
        'Catch ex As Exception
        ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.ToString)
        'MoveToMHType = True
        'End Try
        If Session("LoginWHArea") = String.Empty Then
            MoveToMHType = True
        End If
        Return MoveToMHType
    End Function

    Private Sub SetMHType(ByVal MHEID As String)
        Dim mhSQL As String = "SELECT isnull(LABELPRINTER,'') as LABELPRINTER, isnull(MHETYPE,'') as MHETYPE  FROM MHE WHERE MHEID='" & MHEID & "'"
        Dim dtMH As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(mhSQL, dtMH)
        If dtMH.Rows.Count = 1 Then
            Session("MHType") = dtMH.Rows(0)("MHETYPE")
            Session("MHEIDLabelPrinter") = dtMH.Rows(0)("LABELPRINTER")
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "MHE ID not found", "MHE ID not found")
        End If
    End Sub

    Private Sub SetLocationAndWarehouseArea()
        If Session("LoginWHArea") = "" Then
            If Request.QueryString("WarehouseArea") = "" Then
                Session("LoginWHArea") = ""
            Else
                Session("LoginWHArea") = Request.QueryString("WarehouseArea")

            End If
        End If
        'If Session("LoginWHArea") <> "" Then
        If Session("LoginLocation") = "" Then
            If Request.QueryString("Location") = "" Then
                Session("LoginLocation") = ""
            Else
                'Validate that the location is a valid location in selected wharea
                Dim Sql As String = String.Format("select isnull(warehousearea,'') from location where location = '{0}'", Request.QueryString("Location"))
                Dim wharea As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
                If wharea Is Nothing Then
                    Session("LoginWHArea") = ""
                    Throw New Made4Net.Shared.M4NException(New Exception, "Location is not a valid location in selected warehouse area", "Location is not a valid location in selected warehouse area")
                End If
                If wharea.Equals(Session("LoginWHArea").ToString, StringComparison.OrdinalIgnoreCase) Then
                    Session("LoginLocation") = Request.QueryString("Location")
                    Session("UserSelectedLoginLocation") = Request.QueryString("Location")
                Else
                    Session("LoginWHArea") = ""
                    Throw New Made4Net.Shared.M4NException(New Exception, "Location is not a valid location in selected warehouse area", "Location is not a valid location in selected warehouse area")
                End If
            End If
        End If
        'End If
    End Sub



    Private Sub Submit_Click(sender As Object, e As EventArgs) Handles Submit.Click
        Dim MoveToMain As Boolean = False
        Dim MoveToWH As Boolean = False
        Dim MoveToMHType As Boolean = False
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        Try

            Try
                Made4Net.Shared.Authentication.User.Login(UserName.Text, Password.Text)

                'RWMS-1025 RDT Logging Initializing- if session(RDTAppLogging) is true then we will create logs for the RDT application
                Dim rdtLogHandler As New RDTLogHandler()
                rdtLogHandler.CheckAndInitiateLogging(Logic.GetCurrentUser)
                'End RDT Logging
                MyBase.WriteToRDTLog(String.Format("User {0} is Authenticated.", UserName.Text))
                ' RWMS-2124
                Dim IsUserShiftPerformanceFlagOn As Integer = WMS.Logic.GetSysParam("ShowPerformanceOnTaskComplete")
                If IsUserShiftPerformanceFlagOn And ifLaborTurnedOn() Then
                    If Not UserInShift(UserName.Text) Then
                        MyBase.WriteToRDTLog(String.Format("User {0} Cannot Login, User has not been assigned to a shift.", UserName.Text))
                        Throw New Exception("Cannot Login, User has not been assigned to a shift")

                    End If
                End If
                ' RWMS-2124
            Catch ex As Exception
                'RWMS-1833 Start
                ' ScriptManager.RegisterStartupScript(Me, [GetType](), "showalert", "alert('" & t.Translate(ex.Message) & "');", True)
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
                ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
                'RWMS-1833 End
                Return
            End Try

            Dim whSQL As String
            'Code for 3PL sesion
            Dim sql3PL As String
            Dim consigneeSQL As String
            sql3PL = "SELECT PARAM_VALUE FROM sys_param WHERE param_code='3PL'"
            Session("3PL") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql3PL, "Made4NetSchema")
            If Session("3PL") = 0 Then
                consigneeSQL = "SELECT TOP 1 CONSIGNEE FROM CONSIGNEE"
                Session("consigneeSession") = Made4Net.DataAccess.DataInterface.ExecuteScalar(consigneeSQL)
            End If
            'End 3PL Session
            'RWMS-1834 Start
            Session("RDTDateFormat") = WMS.Logic.GetSysParam("RDTDateFormat")
            'RWMS-1834 End

            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Testing logging of Popup message")

            'Made4Net.Shared.Web.User.Login(DO1.Value("UserId"), DO1.Value("Password"))
            whSQL = "SELECT * FROM USERWAREHOUSE WHERE USERID='" & Logic.GetCurrentUser & "' AND WAREHOUSE='" & Request.QueryString("WH") & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(whSQL, dt, False, "Made4NetSchema")
            'Dim dr As DataRow
            If Session("WH") = "" Then
                If dt.Rows.Count = 0 Then
                    Session("WH") = ""
                    MoveToWH = True
                Else
                    Session("WH") = Request.QueryString("WH")
                    MoveToWH = False
                End If
            Else
                MoveToWH = False
            End If
            MoveToMHType = SetMHEID()
            If MoveToMHType Or MoveToWH Then
                MoveToMain = False
            Else
                MoveToMain = True
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            MoveToMHType = True
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
            MoveToMHType = True
        End Try
        Dim _user As New WMS.Logic.User(UserName.Text)
        Try
            Session.Timeout = Convert.ToInt32(WMS.Logic.GetSysParam("RDTLOGOFFTIME"))
        Catch ex As Exception
        End Try
        Dim lang As Int32
        Try
            lang = Int32.Parse(_user.LANGUAGE)
        Catch ex As Exception
        End Try
        Made4Net.Shared.Translation.Translator.CurrentLanguageID = lang
        MyBase.WriteToRDTLog(String.Format("LoginWH.aspx:MoveToWH={0}, LoginHQT.aspx:MoveToMHType={1},LoginSaCh.aspx:MoveToMain={2} ", MoveToWH, MoveToMHType, MoveToMain))
        If MoveToWH Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginWH.aspx"))
        End If
        If MoveToMHType Then
            Warehouse.setCurrentWarehouse(Session("WH"))
            Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginHQT.aspx"))
        End If


        If MoveToMain Then
            MyBase.WriteToRDTLog(String.Format("LoginLocation={0}, LoginWHArea={1}, MHType={2},TERMINALTYPE={3} ", Session("LoginLocation"), Session("LoginWHArea"), Session("MHType"), Session("MHEID")))
            Warehouse.setCurrentWarehouse(Session("WH"))
            Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nscreens/LoginSaCh.aspx"))
            'Add the Login as an activity of the user
            Dim oWHActivity As New WMS.Logic.WHActivity
            oWHActivity.ACTIVITY = "Login"
            oWHActivity.LOCATION = Session("LoginLocation")
            oWHActivity.WAREHOUSEAREA = Session("LoginWHArea")
            Warehouse.setUserWarehouseArea(Session("LoginWHArea"))
            oWHActivity.USERID = UserName.Text
            oWHActivity.HETYPE = Session("MHType")
            oWHActivity.ACTIVITYTIME = DateTime.Now
            oWHActivity.ADDDATE = DateTime.Now
            oWHActivity.EDITDATE = DateTime.Now
            oWHActivity.ADDUSER = UserName.Text
            oWHActivity.EDITUSER = UserName.Text
            oWHActivity.HANDLINGEQUIPMENTID = Session("MHEID")
            oWHActivity.TERMINALTYPE = Session("TERMINALTYPE")
            Session("ActivityID") = oWHActivity.Post()
            [Global].userSessionManager.AddSession(UserName.Text, HttpContext.Current.Session.SessionID, HttpContext.Current.Request.UserHostAddress, Session("MHType"), Session("MHEID"), LogHandler.GetRDTLogger())
            'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Main.aspx"))
        Else
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/LoginWH.aspx"))
        End If

        Dim appId As String
        appId = Application("Made4NetLicensing_ApplicationId")


    End Sub
    ' RWMS-2124
    Private Function IfLaborTurnedOn() As Boolean
        Dim sql As String = "Select count(*) from LABORTASKCALCULATION"
        Dim count As Integer = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    ' RWMS-2124
    Private Function UserInShift(ByVal userName As String) As Boolean
        Dim SQL As String = String.Format("select isnull(DEFAULTSHIFT,'') DEFAULTSHIFT from USERWAREHOUSE where USERID='{0}' ", userName)
        Dim shift As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME)
        If String.IsNullOrEmpty(shift) Then
            Return False
        Else
            Return True
        End If
    End Function
    ' RWMS-2124
End Class