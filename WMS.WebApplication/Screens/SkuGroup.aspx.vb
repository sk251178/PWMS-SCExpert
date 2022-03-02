Public Class SkuGroup
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TESKUGroup As Made4Net.WebControls.TableEditor
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public Sub New()

    End Sub
    

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


    Private Sub TESKUClass_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESKUGroup.CreatedChildControls
        If TESKUGroup.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            With TESKUGroup.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.SKUGroup"
                .CommandName = "new"
            End With
        End If
        With TESKUGroup.ActionBar.Button("Delete")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.SKUGroup"
            .CommandName = "Delete"
        End With

    End Sub

End Class
