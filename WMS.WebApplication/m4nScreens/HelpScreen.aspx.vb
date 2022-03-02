Imports Made4Net.DataAccess
Imports Made4Net.Schema
Imports Made4Net.Shared

Public Class HelpScreen
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
        ' Get HTML for screen
        Dim sScreenHelpHTMLPath As String = DataInterface.ExecuteScalar("SELECT param_value FROM sys_param WHERE param_code='SystemScreenHelp'", "Made4NetSchema")
        Dim sScreenHelpHTML As String = DataInterface.ExecuteScalar("SELECT ISNULL(helptopic,'webblank.html') FROM sys_screen WHERE screen_id='" & Request.QueryString("sc") & "'", "Made4NetSchema")
        Response.Write("<script lang=""javascript"">var defaultTopic=""" & sScreenHelpHTMLPath & sScreenHelpHTML & """;</script>")
        ''Response.Write("<script lang=""javascript"">window.frames['webcontent'].location = """ & sScreenHelpHTMLPath & sScreenHelpHTML & """;</script>")
    End Sub

End Class
