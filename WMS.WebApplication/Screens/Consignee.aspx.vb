Public Class Consignee
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TEOrders As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEConsigneeShipping As Made4Net.WebControls.TableEditor
    Protected WithEvents TEConsignee As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEConsigneeInventory As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEConsigneeReceiving As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEConsigneeBilling As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEConsigneeContact As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEPW As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEPicking As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector4 As Made4Net.WebControls.DataConnector

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

    Private Sub TEConsigneeContact_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConsigneeContact.CreatedGrid

    End Sub

    Private Sub TEConsigneeContact_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConsigneeContact.CreatedChildControls
        'Dim vals As New Specialized.NameValueCollection
        'vals.Add("CONTACTTYPE", "CONSIGNEE")
        'TEConsigneeContact.PreDefinedValues = vals
        With TEConsigneeContact.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Consignee"
            .CommandName = "SetContact"
        End With
    End Sub

    Private Sub TEConsignee_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConsignee.CreatedChildControls
        If TEConsignee.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            With TEConsignee.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Consignee"
                .CommandName = "createnew"
            End With
        ElseIf TEConsignee.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEConsignee.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Consignee"
                .CommandName = "update"
            End With
        End If
    End Sub
End Class