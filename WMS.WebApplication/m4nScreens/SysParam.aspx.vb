Public Class SysParam
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Protected WithEvents TE As Made4Net.WebControls.TableEditor
    ' Simon Protected WithEvents Screen As App.WebApp.Screen
    <CLSCompliant(False)> Protected WithEvents Screen As WMS.WebCtrls.WebCtrls.Screen

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Made4Net.WebControls.ViewStateHandler.SaveToPersistenceMedium(Me, viewState)
    End Sub

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Made4Net.WebControls.ViewStateHandler.LoadFromPersistenceMedium(Me)
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub TE_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TE.AfterItemCommand
        Made4Net.Shared.SysParam.Reload()
    End Sub
End Class
