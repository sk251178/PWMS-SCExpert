Imports Made4Net.Shared.Web
Imports Made4Net.WebControls

<CLSCompliant(False)> Public Class CLD
    Inherits PWMSRDTBase

    Dim WithEvents btnRCN As Button

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents lblReceipt As Made4Net.WebControls.Label
    Protected WithEvents lblLine As Made4Net.WebControls.Label
    Protected WithEvents lblReceiptVal As Made4Net.WebControls.Label
    Protected WithEvents lblLineVal As Made4Net.WebControls.Label
    Protected WithEvents lblConsignee As Made4Net.WebControls.Label
    Protected WithEvents lblConsigneeVal As Made4Net.WebControls.Label
    Protected WithEvents lblSKU As Made4Net.WebControls.Label
    Protected WithEvents lblSKUVal As Made4Net.WebControls.Label
    Protected WithEvents lblLoadID As Made4Net.WebControls.Label
    Protected WithEvents txtLoadID As Made4Net.WebControls.TextBox
    Protected WithEvents lblLocation As Made4Net.WebControls.Label
    Protected WithEvents txtLocation As Made4Net.WebControls.TextBox
    Protected WithEvents lblMsg As Made4Net.WebControls.Label
    Protected WithEvents lblUnits As Made4Net.WebControls.Label
    Protected WithEvents txtUnits As Made4Net.WebControls.TextBox
    Protected WithEvents lblNumLoads As Made4Net.WebControls.Label
    Protected WithEvents txtNumLoads As Made4Net.WebControls.TextBox
    Protected WithEvents btnReceiptChange As Made4Net.WebControls.Button
    Protected WithEvents DOCreateLoad As Made4Net.Mobile.WebCtrls.DataObject

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
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If Session("CreateLoadRCN") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/CLDSearch.aspx"))
            End If
        End If
    End Sub

    Private Sub DOCreateLoad_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DOCreateLoad.ButtonClick
        Select Case e.ButtonIndex
            Case 0
                Session.Remove("CreateLoadRCN")
                Session.Remove("CreateLoadRCNLine")
                Session.Remove("CreateLoadConsignee")
                Session.Remove("CreateLoadSKU")
                HttpContext.Current.Session.Remove("CreateLoadAdditionalAttributesCode128")
                HttpContext.Current.Session.Remove("CreateLoadDictSKUCode128")
                Made4Net.Mobile.Common.GoToMenu()
            Case 1
                If Page.IsValid Then
                    Made4Net.Mobile.StepManager.Flush("CreateLoad")
                    Made4Net.Mobile.StepManager.Save("CreateLoad", DOCreateLoad)
                    Response.Redirect(MapVirtualPath("Screens/CreateLoad1.aspx"))
                End If
        End Select
    End Sub

    Private Sub DOCreateLoad_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DOCreateLoad.CreatedChildControls
        'Dim tb As Made4Net.WebControls.TextBoxValidated
        'Dim lb As Made4Net.WebControls.FieldLabel
        'If ViewState.Item("CLDGenerateLoadID") Then
        '    tb = DOCreateLoad.Content.Controls(0).Controls(0).FindControl("field_LoadID")
        '    tb.Visible = False
        '    lb = DOCreateLoad.Content.Controls(0).Controls(0).FindControl("label_LoadID")
        '    lb.Visible = False
        'End If
        btnRCN = New Made4Net.WebControls.Button
        btnRCN.Text = "Change Receipt Line..."
        DOCreateLoad.Content.Controls.AddAt(0, btnRCN)
    End Sub

    Private Sub btnRCN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRCN.Click
        Response.Redirect(MapVirtualPath("Screens/CLDSearch.aspx"))
    End Sub

End Class
