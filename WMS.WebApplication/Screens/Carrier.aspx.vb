Public Class Carrier
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterCarrier As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TECarrierContact As Made4Net.WebControls.TableEditor
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

    Private Sub TECarrierContact_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECarrierContact.CreatedChildControls
        With TECarrierContact.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Carrier"
            If TECarrierContact.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "setContactInsert"
            Else
                .CommandName = "setContactUpdate"
            End If
        End With
    End Sub

    Private Sub TECarrierContact_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECarrierContact.CreatedGrid
        'With TECarrierContact.ActionBar.Button("Save")
        '    .ObjectDLL = "WMS.Logic.dll"
        '    .ObjectName = "WMS.Logic.Carrier"
        '    .CommandName = "setContact" 
        'End With
    End Sub

    Private Sub TEMasterCarrier_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterCarrier.CreatedChildControls
        Try
            With TEMasterCarrier.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Carrier"
                If TEMasterCarrier.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "createnew"
                ElseIf TEMasterCarrier.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "update"
                End If
            End With
        Catch ex As Exception
        End Try
    End Sub


End Class
