Public Class DocViewer
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
		Dim ContentType As String = Request.QueryString("ct")
		Dim Path As String = Request.QueryString("path")

		If IO.File.Exists(Path) Then

			Response.ClearContent()
			Response.ClearHeaders()
			Response.ContentType = ContentType
            Response.AddHeader("Content-Disposition", "attachment;filename=results.xls")
			Response.WriteFile(Path)
            Response.Flush()
            'Commented for PWMS-889(RWMS-1002) Start
            'Response.Close()
            'Commented for PWMS-889(RWMS-1002) End
	    'RWMS-2115 Start
	    HttpContext.Current.Response.SuppressContent = True
	    'RWMS-2115 End

            'Added for PWMS-889(RWMS-1002) Start
            HttpContext.Current.ApplicationInstance.CompleteRequest()
            'Added for PWMS-889(RWMS-1002) End

			IO.File.Delete(Path)

		End If
	End Sub

End Class
