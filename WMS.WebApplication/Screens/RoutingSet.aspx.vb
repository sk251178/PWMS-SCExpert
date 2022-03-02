Public Class RoutingSet
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TERoutingSet As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TERoutingSetOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEAssignOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable

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

    Private Sub TERoutingSet_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutingSet.CreatedChildControls
        With TERoutingSet.ActionBar
            .AddSpacer()
            .AddExecButton("plan", "Plan Routing Set", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("plan")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.RoutingSet"
                .CommandName = "plan"
            End With
            With .Button("Save")
                If TERoutingSet.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.RoutingSet"
                    .CommandName = "save"
                End If
            End With
        End With
    End Sub

    Private Sub TERoutingSetOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutingSetOrders.CreatedChildControls
        Dim nvcol As New Specialized.NameValueCollection
        nvcol.Add("SETID", TERoutingSet.SelectedRecordPrimaryKeyValues.Get("SETID"))
        TERoutingSetOrders.PreDefinedValues = nvcol
        TERoutingSetOrders.ActionBar.AddSpacer()
        TERoutingSetOrders.ActionBar.AddExecButton("DeAssignOrders", "DeAssign Orders From Routing Set", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TERoutingSetOrders.ActionBar.Button("DeAssignOrders")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.RoutingSet"
        End With
    End Sub

    Private Sub TEAssignOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignOrders.CreatedChildControls
        Dim nvcol As New Specialized.NameValueCollection
        nvcol.Add("SETID", TERoutingSet.SelectedRecordPrimaryKeyValues.Get("SETID"))
        TEAssignOrders.ActionBar.AddSpacer()
        TEAssignOrders.ActionBar.AddExecButton("AssignOrders", "Assign Orders To Routing Set", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        TEAssignOrders.PreDefinedValues = nvcol
        With TEAssignOrders.ActionBar.Button("AssignOrders")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.RoutingSet"
        End With
    End Sub

End Class
