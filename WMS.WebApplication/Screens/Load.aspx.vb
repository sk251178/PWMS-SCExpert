Public Class Load
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterLoad As Made4Net.WebControls.TableEditor

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

    Private Sub TEMasterLoad_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterLoad.CreatedGrid
        'TEMasterLoad.Grid.AddExecButton("printlabels", "Print Label", "WMS.Logic.dll", "WMS.Logic.Load", 3, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
    End Sub

    Private Sub TEMasterLoad_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMasterLoad.CreatedChildControls
        With TEMasterLoad.ActionBar
            .AddSpacer()
            .AddExecButton("printlabels", "Print Label", Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
            With .Button("printlabels")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

End Class