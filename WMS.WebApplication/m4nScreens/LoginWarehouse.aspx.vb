Imports WMS.Logic
Imports WMS.Lib
Imports Made4Net.Shared.Web
Imports Made4Net.WebControls

Public Class LoginWarehouse
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
            btnGo.ImageUrl = MapVirtualPath("Images/Go_Button.jpg")
            ShowWHList()
        End If
        SetAppInfo()
        SetSplashImages()
    End Sub

    Private Sub ShowWHList()
        Dim User As String = WMS.Logic.Common.GetCurrentUser
        '' Start Commented for labor phase 2 - RWMS-1667 since the clock in and clock out newly implemented in future phase
        'If Not ShiftInstance.checkUserClockinShift(User) And WMS.Logic.GetSysParam("AutoClockinonLogin") = "1" Then
        '    ShiftInstance.ClockUser(User, WMS.Lib.Shift.ClockStatus.IN, "")
        'End If
        '' End Commented for labor phase 2 - RWMS-1667 since the clock in and clock out newly implemented in future phase

        Dim SQL As String = String.Format("select su.username , uw.warehouse , isnull(uw.DEFAULTWHAREA,'') as DEFAULTWHAREA, wa.warehousename " & _
            " from sys_users su inner join userwarehouse uw on su.username = uw.userid " & _
            " inner join warehouse wa on wa.warehouseid = uw.warehouse where su.username = '{0}' ", User)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt, False, "Made4NetSchema")
        Dim dr As DataRow
        If dt.Rows.Count = 0 Then
            Response.Redirect(MapVirtualPath(SCREENS.LOGIN))
        ElseIf dt.Rows.Count = 1 Then
            Warehouse.setCurrentWarehouse(dt.Rows(0)("warehouse"))

            Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection

            If dt.Rows(0)("DEFAULTWHAREA") <> String.Empty Then
                Warehouse.setUserWarehouseArea(dt.Rows(0)("DEFAULTWHAREA"))
            Else
                SetUserWarehouseArea()
            End If
            'Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
            'Added for RWMS-2540 Start
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LOGIN)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOGIN)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("USERID", WMS.Logic.GetCurrentUser)
            aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
            aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
            aq.Send(WMS.Lib.Actions.Audit.LOGIN)
            'Added for RWMS-2540 End
            RedirectToUserScreen(User)
        Else
            For Each dr In dt.Rows
                lstWareHouse.Items.Add(New ListItem(dr("warehousename"), dr("warehouse")))
            Next
        End If
        dt.Dispose()
    End Sub

    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGo.Click
        Warehouse.setCurrentWarehouse(lstWareHouse.SelectedValue())
        Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
        'Set the user default wh area
        If Warehouse.CurrentWarehouse = "" Then

            System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=""JavaScript"">alert('" + WMS.Logic.Utils.TranslateMessage("Please choose warehouse", Nothing) + "')</SCRIPT>")

        Else
            SetUserWarehouseArea()
            RedirectToUserScreen(WMS.Logic.Common.GetCurrentUser)
        End If
    End Sub

    Private Sub SetUserWarehouseArea()
        Dim userid As String = WMS.Logic.Common.GetCurrentUser
        Dim SQL As String = String.Format("select isnull(defaultwharea,'') as defaultwharea from USERWAREHOUSE where userid = '{0}' and warehouse = '{1}'", userid, Warehouse.CurrentWarehouse)
        Dim wharea As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        If String.IsNullOrEmpty(wharea) Then
            Dim dt As New DataTable
            SQL = String.Format("select * from USERWHAREA where userid = '{0}'", userid)
            Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Response.Redirect(MapVirtualPath(SCREENS.LOGIN))
            Else
                wharea = dt.Rows(0)("wharea")
                Warehouse.setUserWarehouseArea(wharea)
            End If
        Else
            Warehouse.setUserWarehouseArea(wharea)
        End If
    End Sub

    Private Sub RedirectToUserScreen(ByVal pUser As String)
        Dim SQL As String = String.Format("SELECT isnull(DEFAULTSCREEN,'') FROM USERPROFILE WHERE (USERID = '{0}')", pUser)
        Dim screenId As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME)
        If screenId = "" Then
            Response.Redirect(MapVirtualPath(SCREENS.MAIN))
        Else
            SQL = String.Format("select isnull(url,'') from sys_screen where screen_id = '{0}'", screenId)
            Dim url As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME)
            If url <> "" Then
                Response.Redirect(MapVirtualPath(url))
            Else
                Response.Redirect(MapVirtualPath(SCREENS.MAIN))
            End If
        End If
    End Sub

    Private Sub SetSplashImages()
        Dim imgL As New Image
        imgL.ImageUrl = SkinManager.GetImageURL("SplashLeft")
        Dim imgM As New Image
        imgM.ImageUrl = SkinManager.GetImageURL("SplashMiddle")
        Dim imgR As New Image
        imgR.ImageUrl = SkinManager.GetImageURL("SplashRight")

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

End Class