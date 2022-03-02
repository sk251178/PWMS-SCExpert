Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic



<CLSCompliant(False)> Public Class PCKCOMMENTS
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            If Not Session("PICKINGCOMMENTS") Is Nothing Then
                DO1.Value("PICKINGCOMMENTS") = Session("PICKINGCOMMENTS")
            End If
        End If
    End Sub


    Private Sub doNext()
        Dim pcklst As Picklist = Session("PCKPicklist")
        If Not pcklst Is Nothing Then
            Session("COMMENTSSHOWN") = 1
            If Not Session("PCKBagOutPicking") Is Nothing AndAlso Session("PCKBagOutPicking") = 1 Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKBagOut.aspx"))
            Else
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCK.aspx"))
            End If
        End If

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddLabelLine("PICKINGCOMMENTS")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()

        End Select
    End Sub
End Class