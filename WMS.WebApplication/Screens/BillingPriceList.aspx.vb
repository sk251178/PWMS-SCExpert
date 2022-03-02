Public Class BillingPriceList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEBillingPriceMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingPriceDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable

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

    Private Sub TEBillingPriceMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingPriceMaster.CreatedChildControls

        With TEBillingPriceMaster.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.PriceList"
            If TEBillingPriceMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "createnew"
            ElseIf TEBillingPriceMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "update"
            End If
        End With
    End Sub

    Private Sub TEBillingPriceMaster_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEBillingPriceMaster.AfterItemCommand
        TEBillingPriceMaster.RefreshData()
    End Sub

End Class
