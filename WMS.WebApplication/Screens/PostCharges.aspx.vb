Public Class PostCharges
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEBillAdditional As Made4Net.WebControls.TableEditor

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

    Private Sub TEBillAdditional_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillAdditional.CreatedChildControls
        With TEBillAdditional.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.AdditionalCharges"
            If TEBillAdditional.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "newac"
            ElseIf TEBillAdditional.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "editac"
            End If
        End With
        TEBillAdditional.ActionBar.AddExecButton("postac", "Post Additional Charges", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TEBillAdditional.ActionBar.Button("postac")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.AdditionalCharges"
            .CommandName = "postac"
        End With
    End Sub

    Private Sub TEBillAdditional_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEBillAdditional.AfterItemCommand
        TEBillAdditional.RefreshData()
    End Sub


End Class