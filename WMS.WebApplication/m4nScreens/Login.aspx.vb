Imports WMS.Logic
Imports WMS.Lib
Imports Made4Net.Shared.Web
Imports Made4Net.WebControls

Public Class Login
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents pnlImages As Made4Net.WebControls.Panel
    Protected WithEvents pnlAppInfo As Made4Net.WebControls.Panel
    Protected WithEvents LoginBox1 As Made4Net.WebControls.LoginBox
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen

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
            Session.Clear()
        End If
        Dim lang As Int32 = WMS.Logic.GetSysParam("DEFAULTLANGUAGE")
        Made4Net.Shared.Translation.Translator.CurrentLanguageID = lang
        Try
            Dim skin As String = WMS.Logic.GetSysParam("DEFAULTSKIN")
            Made4Net.WebControls.SkinManager.ChangeSkin(skin)
            Screen1.SkinManager.ChangeSkin(skin)
        Catch ex As Exception
        End Try
        LoginBox1.LoginButtonImageURL = MapVirtualPath("Images/Go_Button.jpg")
        LoginBox1.Focus()
        'SetAppInfo()
        SetSplashImages()
    End Sub

    Private Sub SetAppInfo()
        Dim Tbl As New Table
        pnlAppInfo.Controls.Add(Tbl)
        With Tbl
            .CssClass = "AppInfo"
            .HorizontalAlign = HorizontalAlign.Left

            .AddRow()

            Dim lblVer As New Label("Version")
            .AddCell(lblVer)
            .AddedCell.HorizontalAlign = HorizontalAlign.Left
            .Add(":&nbsp;")

            Dim Ver As New Label(WMS.Logic.GetSysParam("VERSION"))
            .Add(Ver)

            .AddRow()
            Dim lblLic As New Label("Licensed To")
            .AddCell(lblLic)
            .AddedCell.HorizontalAlign = HorizontalAlign.Left
            .Add(":&nbsp;")

            Dim Lic As New Label(WMS.Logic.GetSysParam("LICENSEDTO"))
            .Add(Lic)

            .AddRow()
            Dim copyrt As New Label(WMS.Logic.GetCopyrightText())
            .AddCell(copyrt)
            .AddedCell.HorizontalAlign = HorizontalAlign.Left

        End With

    End Sub

    Private Sub SetSplashImages()
        Dim imgL As New Image
        imgL.ImageUrl = Screen1.SkinManager.GetImageURL("SplashLeft") 'SkinManager.GetImageURL("SplashLeft")
        Dim imgM As New Image
        imgM.ImageUrl = Screen1.SkinManager.GetImageURL("SplashMiddle") 'SkinManager.GetImageURL("SplashMiddle")
        Dim imgR As New Image
        imgR.ImageUrl = Screen1.SkinManager.GetImageURL("SplashRight") 'SkinManager.GetImageURL("SplashRight")

        Dim Tbl As New Table
        With Tbl
            .CellPadding = 0
            .CellSpacing = 0
            .BorderWidth = Web.UI.WebControls.Unit.Pixel(0)
            .AddCell(imgL)
            .AddCell(imgM)
            .AddCell(imgR)
        End With

        pnlImages.Controls.Add(Tbl)
    End Sub

    Private _user As WMS.Logic.User

    Private Sub SetUser()
        _user = New WMS.Logic.User(WMS.Logic.GetCurrentUser())
        If Not _user.IsWarehouseAssigned() Then
            'If Made4Net.Shared.AppConfig.GetSystemParameter("AUTHLOGINPROVIDER", "LOCAL").Equals("ACTIVEDIRECTORY") Then
            '    _user.ApplyDefaultWarehouse()
            '    If Not _user.IsWarehouseAssigned() Then
            '        Made4Net.Shared.Web.User.Logout()
            '        Throw New ApplicationException("User is not assigned to any warehouse")
            '    End If
            'Else
            Made4Net.Shared.Authentication.User.Logout()
            Throw New ApplicationException("User is not assigned to any warehouse")
            'End If
        End If
    End Sub

    Private Sub GoToMain()
        Try
            SetUser()
        Catch ex As Made4Net.Shared.M4NException
            Screen1.Warn(ex.GetErrMessage())
            Return
        End Try


        Try
            Session.Timeout = Convert.ToInt32(WMS.Logic.GetSysParam("CRTLOGOFFTIME"))
        Catch ex As Exception

        End Try

        Dim lang As Int32
        Try
            lang = Int32.Parse(_user.LANGUAGE)
        Catch ex As Exception
        End Try

        Made4Net.Shared.Translation.Translator.CurrentLanguageID = lang
        If lang = 2 Then
            Try
                SkinManager.ChangeSkin(_user.SKIN)
                Screen1.SkinManager.ChangeSkin(_user.SKIN)
            Catch ex As Exception
                SkinManager.ChangeSkin("DefaultRTL")
                Screen1.SkinManager.ChangeSkin("DefaultRTL")
            End Try
        Else
            Try
                SkinManager.ChangeSkin(_user.SKIN)
                Screen1.SkinManager.ChangeSkin(_user.SKIN)
            Catch ex As Exception
                SkinManager.ChangeSkin("Default")
                Screen1.SkinManager.ChangeSkin("Default")
            End Try
        End If
        Try
            'RWMS-2581 RWMS-2554 - Logging Initializing- if session(WMSAppLogging) is true then we will create logs for the Workstation application
            Dim wmsLogHandler As New WMSLogHandler()
            wmsLogHandler.CheckAndInitiateLogging(Logic.GetCurrentUser)
            'RWMS-2581 RWMS-2554 END
            Response.Redirect(MapVirtualPath(SCREENS.WAREHOUSELOGIN))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoginBox1_LogIn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoginBox1.LogIn
        GoToMain()
    End Sub

End Class