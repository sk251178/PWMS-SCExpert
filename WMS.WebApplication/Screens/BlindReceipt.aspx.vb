Public Class BlindReceipt
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterReceipt As Made4Net.WebControls.TableEditor
    Protected WithEvents TEAB As Made4Net.WebControls.TableEditorActionBar
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TECL As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TECreateLoad As Made4Net.WebControls.TableEditor


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

    Private Sub TEMasterReceipt_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterReceipt.CreatedChildControls
        With TEMasterReceipt.ActionBar
            With .Button("Save")
                '    If TEMasterReceipt.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                '        .CommandName = "New"
                '    ElseIf TEMasterReceipt.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                '        .CommandName = "Edit"
                '    End If
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.ReceiptHeader"
            End With
        End With
    End Sub

    Private Sub TECreateLoad_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECreateLoad.CreatedChildControls
        With TECreateLoad.ActionBar.Button("Save")
            .ObjectName = "WMS.Logic.Receiving"
            .ObjectDLL = "WMS.Logic.dll"
            .CommandName = "BlindReceive"
        End With
    End Sub


End Class