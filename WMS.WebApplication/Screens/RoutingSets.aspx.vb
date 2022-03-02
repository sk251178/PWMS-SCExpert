Public Class RoutingSets
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterRoutingSet As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TERoutingSetRoutes As Made4Net.WebControls.TableEditor
    Protected WithEvents TEUnRoutedOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents RouteSetDetailsDiv As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector

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
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    'Private Sub TEAssignOrders_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEAssignOrders.CreatedChildControls
    '    TEAssignOrders.ActionBar.AddSpacer()
    '    TEAssignOrders.ActionBar.AddExecButton("AssignOrders", "Assign Orders To Routing Set", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
    '    With TEAssignOrders.ActionBar.Button("AssignOrders")
    '        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
    '        .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
    '        .ObjectDLL = "WMS.Logic.dll"
    '        .ObjectName = "WMS.Logic.RoutingSet"
    '    End With
    'End Sub

    'Private Sub TERoutingSetOrders_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TERoutingSetOrders.CreatedChildControls
    '    With TERoutingSetOrders.ActionBar.Button("Save")
    '        .ObjectDLL = "WMS.Logic.dll"
    '        .ObjectName = "WMS.Logic.OutboundOrderHeader"
    '        .CommandName = "SetStagingLane"
    '    End With
    '    TERoutingSetOrders.ActionBar.AddSpacer()
    '    TERoutingSetOrders.ActionBar.AddExecButton("DeAssignOrders", "DeAssign Orders From Routing Set", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
    '    With TERoutingSetOrders.ActionBar.Button("DeAssignOrders")
    '        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
    '        .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
    '        .ObjectDLL = "WMS.Logic.dll"
    '        .ObjectName = "WMS.Logic.RoutingSet"
    '    End With
    'End Sub

    'Private Sub TEMasterRoutingSet_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterRoutingSet.RecordSelected
    '    Dim tds As DataTable = TEMasterRoutingSet.CreateDataTableForSelectedRecord()
    '    Dim vals As New Specialized.NameValueCollection
    '    vals.Add("SETID", tds.Rows(0)("SETID"))
    '    TEAssignOrders.PreDefinedValues = vals
    'End Sub

    'Private Sub TERoutingSetOrders_AfterItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TERoutingSetOrders.BeforeItemCommand
    '    If e.CommandName = "DeAssignOrders" Then
    '        TERoutingSetOrders.RefreshData()
    '        TEMasterRoutingSet.RefreshData()
    '    End If
    'End Sub

    'Private Sub TEAssignOrders_AfterItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAssignOrders.AfterItemCommand
    '    If e.CommandName = "AssignOrders" Then
    '        TEAssignOrders.RefreshData()
    '        TEMasterRoutingSet.RefreshData()
    '    End If
    'End Sub

    Private Sub TERoutingSetRoutes_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutingSetRoutes.CreatedChildControls
        With TERoutingSetRoutes.ActionBar
            .AddExecButton("Plan", "Plan Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanRoute"))
            With .Button("Plan")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.RoutingSet"
            End With
        End With
    End Sub

    Private Sub TEUnRoutedOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUnRoutedOrders.CreatedChildControls
        With TEUnRoutedOrders.ActionBar
            .AddExecButton("SetRoute", "Set Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnApprovePicks"))
            With .Button("SetRoute")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.RoutingSet"
            End With
        End With
    End Sub

    Private Sub TEMasterRoutingSet_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterRoutingSet.CreatedChildControls
        With TEMasterRoutingSet.ActionBar
            .AddExecButton("Plan", "Plan Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanRoute"))
            With .Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.RoutingSet"
            End With
            With .Button("Plan")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.RoutingSet"
            End With
        End With
    End Sub

    Private Sub TEMasterRoutingSet_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterRoutingSet.AfterItemCommand
        If e.CommandName = "plan" Then
            TEMasterRoutingSet.RefreshData()
        End If
    End Sub

End Class
