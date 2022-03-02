Imports Made4Net.Shared.Web

<CLSCompliant(False)> Public Class _Default
    Inherits System.Web.UI.Page

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Request.QueryString("MHTYPE") = "" And Request.QueryString("WH") = "" Then
            Response.Redirect(MapVirtualPath("m4nScreens/Login.aspx"))
        Else
            Response.Redirect(MapVirtualPath("m4nScreens/Login.aspx?MHTYPE=" & Request.QueryString("MHTYPE") & "&WH=" & Request.QueryString("WH")))
        End If
    End Sub

End Class
