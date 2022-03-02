Imports System.ComponentModel
Imports Made4Net.DataAccess
Imports Made4Net.WebControls
Imports WMS.Lib
Imports Telerik.Web.UI
Imports Made4Net.Shared.Web
Imports System.Web

Namespace WebCtrls

    <CLSCompliant(False)> Public Class Screen
        Inherits BasicScreenControl

#Region "Variables"
        Protected WithEvents _lblTitle As Label

        'banner
        Protected WithEvents _TopBanner As Table
        Protected WithEvents _imgBannerLeft As Image
        Protected WithEvents _imgBannerRight As Image
        Protected WithEvents _cellBannerCenter As TableCell

        Protected WithEvents _RadMenu As Telerik.Web.UI.RadMenu
        Protected WithEvents _NavBar As Table

        Protected WithEvents _lblLastScreen As Label
        Protected WithEvents _imgLastScreen As Image
        Protected WithEvents _imgHelp As Image
        Protected WithEvents _imgHome As Image

        Protected WithEvents _lblSelectScreen As Label
        Protected WithEvents _tbSelectScreen As TextBoxValidated
        Protected WithEvents _btnSelectScreen As Button

        Protected WithEvents _InfoBox As Table
        Protected WithEvents _LogOut As LinkButton
        Protected WithEvents _UserDetailsLink As Hyperlink

        'Needed in order to play with Table Visibility (N2) 
        Protected WithEvents PTitleTable As Table
#End Region

#Region "Properties"

        <DefaultValue(False)> Public Property HideBanner() As Boolean
            Get
                Return ViewState("HideBanner")
            End Get
            Set(ByVal Value As Boolean)
                ViewState("HideBanner") = Value
            End Set
        End Property

        <DefaultValue(False)> Public Property HideMenu() As Boolean
            Get
                Return ViewState("HideMenu")
            End Get
            Set(ByVal Value As Boolean)
                ViewState("HideMenu") = Value
            End Set
        End Property

        <Browsable(False)> Public Shadows Property HideStatusLineWhenEmpty() As Boolean
            Get
                Return MyBase.HideStatusLineWhenEmpty
            End Get
            Set(ByVal Value As Boolean)
                MyBase.HideStatusLineWhenEmpty = Value
            End Set
        End Property

        <DefaultValue(False)> Public Property Hidden() As Boolean
            Get
                Return ViewState("Hidden")
            End Get
            Set(ByVal Value As Boolean)
                ViewState("Hidden") = Value
            End Set
        End Property

        Private ReadOnly Property IsLisenced() As Boolean
            Get
                Return HttpContext.Current.Session("Made4NetLicensing_IsLicensedUsers")
            End Get
        End Property
#End Region

#Region "Methods"

        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            If Not Me.Hidden Then
                If Not _lblTitle Is Nothing Then
                    _lblTitle.Value = Made4Net.WebControls.TranslationManager.Translate(Me.Title)
                End If
                MyBase.Render(writer)
                writer.Write("<div style=""margin-left:10px; margin-right:10px; margin-bottom:20;"">")
            End If
        End Sub

        Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
            EnsureChildControls()
            MyBase.OnPreRender(e)
            'Moved SetImage to the CreateNavbar section - Problem with Item counts caused problems with Events (N2)
            'SetImages()
        End Sub

        Protected Overrides Sub CreateChildControls()
            Try
                MyBase.CreateChildControls()
                If Not Me.HideBanner Then
                    Controls.Add(CreateTopBanner)
                End If
                If Not Me.HideMenu Then
                    Controls.Add(CreateNavBar)
                End If
                Controls.Add(CreateTitle)
            Catch ex As Exception
                Me.Warn(ex.Message)
            End Try
        End Sub

        Private Function CreateNavBar() As Table
            'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5
            Dim LeftDirection As UI.WebControls.HorizontalAlign
            Dim RightDirection As UI.WebControls.HorizontalAlign
            If TranslationManager.RTL Then
                LeftDirection = UI.WebControls.HorizontalAlign.Right
                RightDirection = UI.WebControls.HorizontalAlign.Left
            Else
                LeftDirection = UI.WebControls.HorizontalAlign.Left
                RightDirection = UI.WebControls.HorizontalAlign.Right
            End If

            'Dim RightDirection As Web.UI.WebControls.HorizontalAlign
            'If TranslationManager.RTL Then
            '    LeftDirection = Web.UI.WebControls.HorizontalAlign.Right
            '    RightDirection = Web.UI.WebControls.HorizontalAlign.Left
            'Else
            '    LeftDirection = Web.UI.WebControls.HorizontalAlign.Left
            '    RightDirection = Web.UI.WebControls.HorizontalAlign.Right
            'End If


            _NavBar = New Table
            _NavBar.ID = "Navbar"
            _NavBar.Attributes("Style") = "Visibility:Hidden;"

            With _NavBar
                .CellPadding = 0
                .CellSpacing = 0

                .AddCell(CreateMenu())
                .AddedCell.HorizontalAlign = LeftDirection
                'If Made4Net.WebControls.TranslationManager.RTL Then
                'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5,
                .AddedCell.Width = UI.WebControls.Unit.Percentage(100)
                'End If
                .AddCell()
                'If Not Made4Net.WebControls.TranslationManager.RTL Then
                .AddedCell.Width = UI.WebControls.Unit.Percentage(100)
                'End If
                .AddedCell.HorizontalAlign = RightDirection
                .AddedCell.VerticalAlign = UI.WebControls.VerticalAlign.Top

                .Add("<table border=0 cellpadding=0 cellspacing=0><tr><td nowrap>")

                .Add("&nbsp;&nbsp;&nbsp;&nbsp;")


                .Add("<a href=LastScreen>")
                _imgLastScreen = New Image
                _imgLastScreen.Attributes.Add("align", "center")
                .Add(_imgLastScreen)
                .Add("</a>")

                .Add("</td><td nowrap>")
                .Add("<a href='" & Made4Net.Shared.Web.MapVirtualPath("screens/main.aspx?") & "'>&nbsp;")
                _imgHome = New Image
                _imgHome.Attributes.Add("align", "center")
                .Add(_imgHome)
                .Add("</a>")

                .Add("<a href='" & Made4Net.Shared.Web.MapVirtualPath("m4nscreens/HelpScreen.aspx?sc=" & _ScreenID) & "' target=_blank class=LastScreen>&nbsp;")
                _imgHelp = New Image
                _imgHelp.Attributes.Add("align", "center")
                .Add(_imgHelp)
                .Add("</a>")

                .Add("</td><td nowrap>")
                .Add("&nbsp;&nbsp;&nbsp;&nbsp;")

                _lblSelectScreen = New Label("Go To Screen")
                .Add(_lblSelectScreen)
                .Add(":&nbsp;")
                _tbSelectScreen = New TextBoxValidated
                With _tbSelectScreen
                    .ID = "GotoScreen"
                    .Columns = 4
                    .CssClass = "GotoScreenTextBox"
                End With
                .Add(_tbSelectScreen)

                _btnSelectScreen = New Button
                With _btnSelectScreen
                    .ID = "btnGotoScreen"
                    .Style.Add("display", "none")
                    .CausesValidation = False
                End With
                .Add(_btnSelectScreen)


                .Add("&nbsp;")
                .Add("</td></tr></table>")
            End With
            ' Called here instead of in PreRender (see explanation in that function) (N2)
            SetImages()
            'ReBuildMenu()
            Return _NavBar
        End Function

        Private Function CreateMenu() As Telerik.Web.UI.RadMenu
            Dim XML As String = Made4Net.WebControls.RadMenuUtil.CreateMenuXML()

            _RadMenu = New Telerik.Web.UI.RadMenu
            _RadMenu.Skin = Made4Net.WebControls.Skins.SkinManager.GetInstance(Page).CurrentSkin()
            _RadMenu.ClickToOpen = True
            _RadMenu.CausesValidation = False
            'If Made4Net.WebControls.TranslationManager.RTL Then
            '    _RadMenu.Attributes("dir") = "rtl"
            'Else
            '    _RadMenu.Attributes("dir") = "ltr"
            'End If
            XML = XML.Replace(" Label=", " Text=")
            'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5,
            '_RadMenu.LoadXmlString(XML)
            _RadMenu.LoadXml(XML)
            'For Each menuItem As WebControls.RadMenuItem In _RadMenu.Items
            '    menuItem.Value = menuItem.Text
            '    'Removed - Style is managed through the styles.css (N2)
            '    ' menuItem.CssClassOver = "MenuItemOverRoot"
            '    If menuItem.Items.Count > 0 Then
            '        menuItem.GroupSettings.OffsetY = 5
            '    End If
            'Next
            ' Add the first and last RadMenuItem's that will contains the images
            'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5,
            Dim menuItemFirstImage As New Telerik.Web.UI.RadMenuItem()
            Dim menuItemLastImage As New Telerik.Web.UI.RadMenuItem()
            menuItemFirstImage.PostBack = False
            menuItemLastImage.PostBack = False
            _RadMenu.Items.Insert(0, menuItemFirstImage)
            _RadMenu.Items.Add(menuItemLastImage)
            Return _RadMenu
        End Function

        Private Function CreateTitle() As Table
            'Hide the title - It will be made visible in OnLoad() (N2)
            PTitleTable = New Table
            PTitleTable.ID = "TitleTable"
            PTitleTable.Attributes("style") = "Visibility:Hidden"
            With PTitleTable
                _lblTitle = New Label
                If Me.Title Is Nothing OrElse Me.Title.Length = 0 Then
                Else
                    _lblTitle.Value = Me.Title
                    .AddCell("<h1>")
                    .Add(_lblTitle)
                    .Add("</h1>")
                End If
            End With
            Return PTitleTable
        End Function

        Private Function CreateTopBanner() As Table

            _TopBanner = New Table
            _imgBannerLeft = New Image
            _imgBannerRight = New Image
            _cellBannerCenter = New TableCell

            With _TopBanner
                .CellPadding = 0
                .CellSpacing = 0
                'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5 
                .Width = UI.WebControls.Unit.Percentage(100)

                If TranslationManager.RTL Then
                    .AddCell()
                    InsertInfoBox()
                    .Add(_imgBannerRight)

                    .AddedRow.Cells.Add(_cellBannerCenter)
                    .AddedCell.Width = UI.WebControls.Unit.Percentage(100)

                    .AddCell(_imgBannerLeft)
                Else
                    .AddCell(_imgBannerLeft)

                    .AddedRow.Cells.Add(_cellBannerCenter)
                    'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5 -Pending
                    .AddedCell.Width = UI.WebControls.Unit.Percentage(100)

                    .AddCell()
                    InsertInfoBox()
                    .Add(_imgBannerRight)
                End If

            End With
            Return _TopBanner
        End Function

        Protected Sub InsertInfoBox()
            If Not _NoLoginRequired Then
                CreateInfoBox()
                With _TopBanner
                    .AddedCell.Wrap = False
                    .Add("<div class='info_box'>")
                    .Add(_InfoBox)
                    .Add("</div>")
                End With
            End If

        End Sub

        Protected Sub CreateInfoBox()
            _InfoBox = New Table
            With _InfoBox

                Dim user As String = WMS.Logic.GetCurrentUserDisplayName()
                Dim wh As String = WMS.Logic.Warehouse.WarehouseName()
                Dim wharea As String = WMS.Logic.Warehouse.getUserWarehouseAreaDescription()

                .AddCell(String.Format("<div class=TopInfo><nowrap><span class=UserName>{0}</span>&nbsp;- {1}&nbsp;/ {2}<br>", user, wh, wharea))

                '.Add(_UserDetailsLink)
                Dim UserDetailStr As String = String.Format("<a class=""TopLinks"" onmouseover=""this.style.cursor='hand'"" onclick=window.open(""{0}"",target=""_blank"",""status=yes,toolbar=no,menubar=no,location=no,resizable=1,width=750,height=300"")>User Details</a>", MapVirtualPath(SCREENS.USER_INFO))
                .Add(UserDetailStr)

                _LogOut = New LinkButton
                With _LogOut
                    .Text = "Log Off"
                    .CausesValidation = False
                    .CssClass = "TopLinks"
                End With
                .Add("&nbsp;&nbsp;&nbsp;")
                .Add(_LogOut)

                .Add("</nowrap></div>")

            End With
        End Sub

        Protected Sub SetTableEditorLines()
            Dim R As New RecursiveControlFinder
            Dim TEs As Made4Net.DataAccess.Collections.GenericCollection = _
                         R.SearchByType(GetType(TableEditor), Page.Controls)
            Dim LinesTab As Int32 = 15
            Try
                LinesTab = Int32.Parse(WMS.Logic.GetSysParam("DOLINESTAB"))
            Catch ex As Exception
            End Try

            Dim LinesMaster As Int32 = 10
            Try
                LinesMaster = Int32.Parse(WMS.Logic.GetSysParam("DOLINESMASTER"))
            Catch ex As Exception
            End Try

            Dim LinesSingle As Int32 = 25
            Try
                LinesSingle = Int32.Parse(WMS.Logic.GetSysParam("DOLINESSINGLE"))
            Catch ex As Exception
            End Try

            Dim n As Int32, te As TableEditor, DTab As DataTabControl
            For n = 0 To TEs.Count - 1
                te = TEs(n)
                DTab = RecursiveControlFinder.SearchForParentByType(GetType(DataTabControl), te)    'R.SearchForParentByType(GetType(DataTabControl), te)
                If Not DTab Is Nothing Then
                    'In a tab
                    te.GridPageSize = LinesTab
                ElseIf TEs.Count = 1 Then
                    'single on a page
                    te.GridPageSize = LinesSingle
                Else
                    'not single
                    te.GridPageSize = LinesMaster
                End If
            Next
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)
            Me.HideStatusLineWhenEmpty = True
            SetTableEditorLines()

            ' This is a series of patche due to teh fact that the RadMenu Wraps around when innside a Table (N2)
            If Not Me.HideMenu Then
                ' Fix the width of the Menu Table (N2)
                ' Page.ClientScript.RegisterStartupScript(Page.GetType, "Menu", String.Format("{0}.FixListWidth({0});", _RadMenu.ClientID), True)
                ' Make the Navbar Visible - It is first set to not be visible in order to avoid showing the Menu wrapping and then quickly being stretched.
                ' This whole thing should hopefully be fixed in a later version (N2)
                'Upgrade bug found 

                _NavBar.ID = "Navbar"
                Page.ClientScript.RegisterStartupScript(Page.GetType, "Table", String.Format("{0}.style.visibility = ""visible"";", _NavBar.ID), True)
                ' Same as the line above for the screen title (N2)
                Page.ClientScript.RegisterStartupScript(Page.GetType, "Table2", String.Format("{0}.style.visibility = ""visible"";", PTitleTable.ID), True)
            End If
        End Sub

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
            If Not _NoLoginRequired Then
                WMS.Logic.EnsureUser()
            End If
        End Sub

#End Region

#Region "Images"

        Protected ReadOnly Property I(ByVal pImageName As String) As String
            Get
                ' Not Changed but should be fixed in Library so that there is an option to return a Relative path. 
                ' if you look at calls to I() in here, all calls used to do I(<string>).Substring(1)  (N2)
                Return SkinManager.GetImageURL(pImageName)
            End Get
        End Property
        Protected ReadOnly Property W(ByVal pImageName As String) As String
            Get
                ' Not Changed but should be fixed in Library so that there is an option to return a Relative path. 
                ' if you look at calls to I() in here, all calls used to do I(<string>).Substring(1)  (N2)
                Return SkinManager.GetImageWidth(pImageName)
            End Get
        End Property

        Public Sub SetImages()
            Try
                _imgBannerLeft.ImageUrl = I("LeftBanner")
                _imgBannerRight.ImageUrl = I("RightBanner")
                _cellBannerCenter.Attributes.Add("background", I("MiddleBanner"))
                _imgLastScreen.ImageUrl = I("LastScreenArrow")
                'RWMS-1773 - Added tooltip for Home,back and help start
                _imgLastScreen.ToolTip = "Back"
                _imgHelp.ImageUrl = I("ActionBarBtnHelp")
                _imgHelp.ToolTip = "Help"
                _imgHome.ImageUrl = I("BtnHome")
                _imgHome.ToolTip = "Home"
                'RWMS-1773 - Ended tooltip for Home,back and help start
                ' Changed to to set BG for all cells (N2)
                'With _NavBar
                '    .Rows(0).Cells(1).Style.Add("background-image", String.Format("url({0})", I("NavBg")))
                'End With
                With _NavBar
                    For ncells As Int32 = 0 To .Rows(0).Cells.Count - 1
                        .Rows(0).Cells(ncells).Style.Add("background-image", String.Format("url({0})", I("NavBg")))
                    Next
                End With

                With _RadMenu
                    ' This property is no longer used - I general the Telerik directory structure needs to be used (N2)
                    '.ImagesBaseDir = "/"
                    If Made4Net.WebControls.TranslationManager.RTL Then
                        .Style.Add("background-image", String.Format("url({0})", I("NavBg")))
                    Else
                        .Style.Add("background-image", String.Format("url({0})", I("MenuBg")))
                    End If
                    ' Modify the first and last menu items
                    Me.SetUpRadMenuItem(.Items(0), I("MenuLeftCorner"), String.Empty, String.Empty, String.Empty, W("MenuLeftCorner"))
                    Me.SetUpRadMenuItem(.Items(.Items.Count - 1), I("MenuRightCorner"), String.Empty, String.Empty, String.Empty, W("MenuRightCorner"))

                    ' Removed the check below for Postback - Problem with Item counts caused problems with Events (N2)  
                    'If Not Page.IsPostBack Then
                    'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5, 
                    Dim objMenuItem As RadMenuItem = Nothing

                    For n As Int32 = (.Items.Count - 2) To 2 Step -1
                        ' Create a new RadMenuItem and set it up
                        'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5, 
                        objMenuItem = New RadMenuItem()
                        objMenuItem.PostBack = False
                        'objMenuItem.IsSeparator = True
                        Me.SetUpRadMenuItem(objMenuItem, I("MenuSeparator"), String.Empty, String.Empty, String.Empty, W("MenuSeparator"))

                        ' Add the RadMenuItem to the RadMenu
                        .Items.Insert(n, objMenuItem)
                    Next
                    If Made4Net.WebControls.TranslationManager.RTL Then
                        For n As Int32 = 0 To .Items.Count - 1
                            .Items(n).Style.Add("background-image", String.Format("url({0})", I("MenuBg")))
                            .Items(n).Style.Add("background-repeat", "repeat-x")
                        Next
                    End If
                    'End If
                End With
            Catch ex As Exception
            End Try
        End Sub
        Private Sub ReBuildMenu()
            'Dim _mobj(_RadMenu.Items.Count - 1) As WebControls.RadMenuItem
            'With _RadMenu
            '    For n As Int32 = 0 To .Items.Count - 1
            '        _mobj(n) = .Items(n)
            '    Next
            '    .Items.Clear()
            'End With
            '_RadMenu.Dispose()
            '_RadMenu = New RadMenu
            '_RadMenu.Skin = Made4Net.WebControls.Skins.SkinManager.GetInstance(Page).CurrentSkin()
            '_RadMenu.ClickToOpen = True
            '_RadMenu.CausesValidation = False
            'If Made4Net.WebControls.TranslationManager.RTL Then
            '    _RadMenu.Attributes("dir") = "rtl"
            'Else
            '    _RadMenu.Attributes("dir") = "ltr"
            'End If

            'For n As Int32 = 0 To _mobj.Length - 1
            '    _RadMenu.Items.Add(_mobj(n).Clone)
            'Next
            'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5,
            Dim xml As String = _RadMenu.GetXml
            _RadMenu.Items.Clear()
            _RadMenu.LoadXml(xml)


        End Sub
        'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5, 
        Private Sub SetUpRadMenuItem(ByRef objMenuItem As RadMenuItem, _
                                            ByVal strImageUrl As String, _
                                            ByVal strCssClass As String, _
                                            ByVal strClickedCssClass As String, _
                                            ByVal strOverCssClass As String, _
                                            ByVal strImageWidth As String)

            ' Set the image URL
            Try
                objMenuItem.ImageUrl = strImageUrl
            Catch
            End Try
            If Not IsNothing(strImageWidth) Then
                Try
                    objMenuItem.Width = Web.UI.WebControls.Unit.Pixel(strImageWidth)
                    If Made4Net.WebControls.TranslationManager.RTL Then
                        'objMenuItem.Style.Add("width", strImageWidth & "px")
                    End If
                Catch
                End Try
            End If
            ' Set the CSS classes
            If Made4Net.WebControls.TranslationManager.RTL Then
                'objMenuItem.Style.Add("background-repeat", "repeat-x")
            End If
            objMenuItem.CssClass = strCssClass
            objMenuItem.ClickedCssClass = strClickedCssClass

            ' TODO: Must look into how to add the CssClassOver string
        End Sub
        'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5, 
        Private Sub SetUpRadMenuItemOLD(ByRef objMenuItem As RadMenuItem, _
                                    ByVal strImageUrl As String, _
                                    ByVal strCssClass As String, _
                                    ByVal strClickedCssClass As String, _
                                    ByVal strOverCssClass As String)
            ' Set the image URL
            Try
                objMenuItem.ImageUrl = strImageUrl
            Catch
            End Try

            ' Set the CSS classes
            objMenuItem.CssClass = strCssClass
            objMenuItem.ClickedCssClass = strClickedCssClass

            ' TODO: Must look into how to add the CssClassOver string
        End Sub

#End Region

#Region "Handlers"

        Private Sub _LogOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _LogOut.Click
            WMS.Logic.LogOut()
        End Sub

        Protected Overrides Sub HandleErrorEvent(ByVal sender As Object, ByVal e As StandardErrorEventArgs)
            Dim msg As String
            Dim m4nEx As Made4Net.Shared.M4NException
            Dim lang As Integer = Made4Net.Shared.Translation.Translator.CurrentLanguageID
            'By Simon. If It's Thread Exception - do not fire any event.
            If TypeOf e.Exception Is System.Threading.ThreadAbortException Then
                Exit Sub
            End If
            '-----------------------------------------------------------------------------------------
            If e.IsMade4NetException() Then
                m4nEx = e.Exception
                msg = m4nEx.GetErrMessage(lang)
                If m4nEx.Output = Made4Net.[Shared].M4NException.ErrorOutput.Screen Then
                    Me.Warn(msg)
                End If
            Else
                Me.Warn(e.Message)
            End If
            If Made4Net.Shared.AppConfig.EnableErrorLog Then
                If e.IsMade4NetException() Then
                    If m4nEx.Output = Made4Net.[Shared].M4NException.ErrorOutput.ApplicationLog Then
                        m4nEx.WriteToLog(Made4Net.[Shared].M4NException.ErrorOutput.ApplicationLog)
                    Else
                        m4nEx.WriteToLog(Made4Net.[Shared].M4NException.ErrorOutput.SystemLog)
                    End If
                    'm4nEx.WriteToSystemEventViewer("WMSLOG")
                End If
                Try
                    Made4Net.Shared.Web.ErrorLog.Write(e.Exception)
                Catch ex As Exception
                    Throw New ApplicationException("An error occured while trying to write an exception to the error log. You can turn the error log on and off by setting the EnableErrorLog directive in the web.config file.", ex)
                End Try
            End If
        End Sub

        Protected Overrides Sub HandleSuccessEvent(ByVal sender As Object, ByVal e As StandardSuccessEventArgs)
            Me.Notify(e.Message, StatusLine.StatusLineMessageType.Success)
        End Sub

        Private Sub _btnSelectScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _btnSelectScreen.Click
            Me.GoToScreen(_tbSelectScreen.Value)
        End Sub

#End Region

    End Class

End Namespace
