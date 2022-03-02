Imports Made4Net.Shared.Web

<CLSCompliant(False)> Public Class CreateReceiptLoad
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DOCreateLoad As Made4Net.Mobile.WebCtrls.DataObject
    Protected WithEvents btnRCN As Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If Session("CreateLoadRCN") Is Nothing Or Session("CreateLoadRCNLine") Then
                Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLines.aspx"))
            End If
        End If
    End Sub

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region


    Private Sub DOCreateLoad_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DOCreateLoad.ButtonClick
        Select Case e.ButtonIndex
            Case 0
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
        btnRCN = New Made4Net.WebControls.Button
        btnRCN.Text = "Select Receipt Line..."
        DOCreateLoad.Content.Controls.AddAt(0, btnRCN)

        Dim r As String = Session("CreateLoadCRN")
        Dim lbl As New Made4Net.WebControls.Label(r)
        Dim lbl2 As New Made4Net.WebControls.Label("Receipt")

        DOCreateLoad.Content.Controls.AddAt(0, lbl)
        DOCreateLoad.Content.Controls.AddAt(0, New LiteralControl(" "))
        DOCreateLoad.Content.Controls.AddAt(0, lbl2)

    End Sub

    Private Sub btnRCN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRCN.Click
        Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLines.aspx"))
    End Sub


End Class
