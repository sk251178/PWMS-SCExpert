Public Class AdjustmentDocument
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEADJUSTMENT As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEADJUSTMENTDETAIL As Made4Net.WebControls.TableEditor
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

    Private Sub TEADJUSTMENT_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEADJUSTMENT.CreatedChildControls
        With TEADJUSTMENT
            With .ActionBar
                .AddSpacer()

                .AddExecButton("commit", "Commit", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("commit")
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.AdjustmentDocument"
                    .CommandName = "Commit"
                End With

                .AddExecButton("CancelDoc", "Cancel", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelDoc")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.AdjustmentDocument"
                    .CommandName = "CancelException"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel this adjustment document?"
                End With

            End With

        End With
    End Sub

End Class