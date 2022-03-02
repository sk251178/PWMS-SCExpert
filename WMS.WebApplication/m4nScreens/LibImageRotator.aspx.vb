Public Class LibImageRotator
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TE As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents Screen As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm

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
        Dim vals As New Specialized.NameValueCollection
        vals.Add("ItemID", Request.QueryString("ItemID"))
        TE.PreDefinedValues = vals
    End Sub

End Class
