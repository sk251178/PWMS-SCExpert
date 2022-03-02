Public Class ConsolidationTasks
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEConsolidationHeader As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEConsolidationDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
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
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEConsolidationDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConsolidationDetail.CreatedChildControls
        With TEConsolidationDetail.ActionBar
            .AddSpacer()

            .AddExecButton("approveconsdetail", "Approve Consolidation Detail", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApproveConsolidationDetail"))
            With .Button("approveconsdetail")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Consolidation"
                .CommandName = "approveconsdetail"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to approve the selected consolidation detail?"
            End With

            .AddExecButton("cancelconsdetail", "Cancel Consolidation Detail", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelConsolidationDetail"))
            With .Button("cancelconsdetail")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Consolidation"
                .CommandName = "cancelconsdetail"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the selected consolidation detail?"
            End With

        End With
    End Sub

    Private Sub TEConsolidationHeader_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConsolidationHeader.CreatedChildControls
        With TEConsolidationHeader.ActionBar
            .AddSpacer()

            .AddExecButton("approvecons", "Approve Consolidation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApproveConsolidation"))
            With .Button("approvecons")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Consolidation"
                .CommandName = "approvecons"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to approve the selected consolidation?"
            End With

            .AddExecButton("cancelcons", "Cancel Consolidation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelConsolidation"))
            With .Button("cancelcons")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Consolidation"
                .CommandName = "cancelcons"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the selected consolidation?"
            End With

        End With
    End Sub

    Private Sub TEConsolidationHeader_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEConsolidationHeader.AfterItemCommand
        If e.CommandName = "cancelcons" Or e.CommandName = "approvecons" Then
            TEConsolidationHeader.RefreshData()
            TEConsolidationDetail.RefreshData()
        End If
    End Sub

    Private Sub TEConsolidationDetail_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEConsolidationDetail.AfterItemCommand
        If e.CommandName = "cancelconsdetail" Or e.CommandName = "approveconsdetail" Then
            TEConsolidationHeader.RefreshData()
            TEConsolidationDetail.RefreshData()
        End If
    End Sub

End Class
