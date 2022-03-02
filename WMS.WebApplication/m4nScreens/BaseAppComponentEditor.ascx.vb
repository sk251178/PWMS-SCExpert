Public Class BaseAppComponentEditor
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TE As Made4Net.WebControls.TableEditor

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Const BASE_APP_COMPONENT_DLL As String = "Made4Net.Schema.dll"
    Private Const BASE_APP_COMPONENT_CLASS As String = "Made4Net.Schema.AppComponents.AppComponentBase"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TE_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TE.CreatedChildControls
        With TE.ActionBar
            If TE.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                With .Button("Save")
                    .ObjectDLL = BASE_APP_COMPONENT_DLL
                    .ObjectName = BASE_APP_COMPONENT_CLASS
                    .CommandName = "Save"

                    .MethodName = "Exec_CreateNew"
                End With
            End If

            With .Button("Delete")
                .ObjectDLL = BASE_APP_COMPONENT_DLL
                .ObjectName = BASE_APP_COMPONENT_CLASS
                .CommandName = "Delete"
                .MethodName = "Exec_Delete"
            End With
        End With
    End Sub

    'Private Sub TE_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TE.CreatedGrid
    '    With TE.Grid
    '        Dim url As String = Made4Net.WebControls.ScreenList.GetScreenURL("-dt")
    '        url &= "&DTName=ACBase_{0};code"
    '        url = Made4Net.Shared.Web.MapVirtualPath(url)
    '        .AddLinkColumn("EditDT", "Edit DT", "Edit Data Template", url, "_blank", 0, Nothing)

    '        .AddExecButton("Exec_Refresh", "Refresh", BASE_APP_COMPONENT_DLL, BASE_APP_COMPONENT_CLASS, 0)
    '    End With
    'End Sub
End Class
