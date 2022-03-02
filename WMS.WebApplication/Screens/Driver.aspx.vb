Public Class Driver
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEDriver As Made4Net.WebControls.TableEditor
    Protected WithEvents hdPointId As System.Web.UI.WebControls.TextBox

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
        'If IsPostBack Then
        '    Dim tds As DataTable = TEDriver.CreateDataTableForSelectedRecord()
        '    Dim vals As New Specialized.NameValueCollection
        '    vals.Add("newstartpointid", hdPointId.Text)
        '    TEDriver.PreDefinedValues = vals
        'End If
    End Sub

    Private Sub TEDriver_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDriver.CreatedChildControls
        'Dim li As New Made4Net.WebControls.LinkImage
        'li.ImageUrl = Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit")
        'li.OnClick = "ShowDriver()"
        'li.ToolTip = "Locate"
        'li.Style.Add("cursor", "hand")

        'Dim li2 As New Made4Net.WebControls.LinkImage
        'li2.ImageUrl = Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit")
        'li2.OnClick = "ClearDriver()"
        'li2.ToolTip = "Clear"
        'li2.Style.Add("cursor", "hand")

        'With TEDriver.ActionBar
        '    .AddSpacer()
        '    .AddControl(li)
        '    .AddControl(li2)
        '    .Button("cancel").Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        'End With
    End Sub

    Private Sub TEDriver_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDriver.CreatedGrid
        'TEDriver.Grid.AddExecButton("CenterDriver", "CenterDriver", "WMS.Logic.dll", "WMS.Logic.TMSWebSupport", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        'TEDriver.Grid.AddExecButton("setpoint", "Point Driver", "WMS.Logic.dll", "WMS.Logic.Driver", 3, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
    End Sub

    Private Sub TEDriver_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEDriver.AfterItemCommand
        If e.CommandName = "setpoint" Then
            TEDriver.RefreshData()
        End If
    End Sub
End Class