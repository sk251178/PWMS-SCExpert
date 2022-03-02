Imports Made4Net.WebControls

Public Class LangSkinSetup
	Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
	Protected WithEvents Label1 As Made4Net.WebControls.Label
	Protected WithEvents lang As Made4Net.WebControls.DropDownListValidated
	Protected WithEvents TEOrders As Made4Net.WebControls.TableEditor
	Protected WithEvents ddSkin As Made4Net.WebControls.DropDownListValidated
	Protected WithEvents Label2 As Made4Net.WebControls.Label

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
		If Not IsPostBack Then
			lang.DataBind()
            lang.Value = Made4Net.Shared.Translation.Translator.CurrentLanguageID

			ddSkin.DataSource = Screen1.SkinManager.GetSkinList()
			ddSkin.DataBind()
			ddSkin.Value = Screen1.SkinManager.CurrentSkin
		End If
	End Sub

	Private Sub lang_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lang.SelectedIndexChanged
        Made4Net.Shared.Translation.Translator.CurrentLanguageID = lang.Value
		Response.Redirect(Request.RawUrl)
	End Sub

	Private Sub ddSkin_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddSkin.SelectedIndexChanged
		Screen1.SkinManager.ChangeSkin(ddSkin.Value)
	End Sub
End Class
