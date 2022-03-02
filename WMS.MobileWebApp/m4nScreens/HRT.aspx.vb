<CLSCompliant(False)> Public Class HRT
    Inherits PWMSBaseLogger

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
        If Request.QueryString.Count = 0 Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx?Skin=HRT"))
        Else
            If Request.QueryString("WH") <> "" Then Session("WH") = Request.QueryString("WH")
            If Request.QueryString("MHTYPE") <> "" Then Session("MHTYPE") = Request.QueryString("MHTYPE")
            If Request.QueryString("MHEID") <> "" Then Session("MHEID") = Request.QueryString("MHEID")
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx?Skin=HRT"))
        End If
    End Sub

End Class
