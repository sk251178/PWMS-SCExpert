Public Class CustomViews
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
	Protected WithEvents CustomViewsManager1 As Made4Net.WebControls.CustomViewsManager

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region


    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Made4Net.WebControls.ViewStateHandler.SaveToPersistenceMedium(Me, viewState)
    End Sub

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Made4Net.WebControls.ViewStateHandler.LoadFromPersistenceMedium(Me)
    End Function

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

End Class
