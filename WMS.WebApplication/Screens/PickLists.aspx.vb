Public Class PickLists
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterPick As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEPickDetails As Made4Net.WebControls.TableEditor
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

    End Sub

    Private Sub TEMasterPick_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterPick.CreatedChildControls
        With TEMasterPick
            With .ActionBar
                .AddSpacer()

                .AddExecButton("ApprovePicks", "Approve Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("ApprovePicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "ApprovePickList"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to approve the selected Pick Lists?"
                End With

                .AddExecButton("CancelPicks", "Cancel Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelPicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "CancelPickList"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected Pick Lists?"
                End With

                .AddExecButton("unallocatePicks", "Unallocate Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCancel"))
                With .Button("unallocatePicks")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "unallocatePickList"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unallocated the selected Pick Lists?"
                End With

                .AddExecButton("PrintPickList", "Print Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintPickList")
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Pick"
                    .CommandName = "PrintPickList"
                End With

            End With
        End With
    End Sub

    Private Sub TEMasterPick_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterPick.AfterItemCommand
        If e.CommandName = "ApprovePickList" Or e.CommandName = "CancelPickList" Or e.CommandName = "unallocatePickList" Then
            TEMasterPick.RefreshData()
            TEPickDetails.RefreshData()
        End If
    End Sub

End Class
