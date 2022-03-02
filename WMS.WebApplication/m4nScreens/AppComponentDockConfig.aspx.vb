Public Class AppComponentDockConfig
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents ph As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents btnOK As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents btnCancel As System.Web.UI.HtmlControls.HtmlInputButton

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "handler"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            btnCancel.Value = Made4Net.WebControls.TranslationManager.Translate(btnCancel.Value)
            btnOK.Value = Made4Net.WebControls.TranslationManager.Translate(btnOK.Value)
        End If
    End Sub

#End Region

#Region "property"

    Public ReadOnly Property DockCode() As String
        Get
            Return Request.QueryString("DockCode")
        End Get
    End Property

    Public ReadOnly Property DockScreenID() As String
        Get
            Return Request.QueryString("DockScId")
        End Get
    End Property

#End Region

#Region "method"

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()

        Me.ValidateScreenUsage()
        Me.BuildControls()
    End Sub

    Sub ValidateScreenUsage()
        If Not Made4Net.Shared.Authentication.User.IsLoggedIn Then
            Throw New ApplicationException("Cannot use this screen because you are not logged in.")
        End If

        If Me.DockCode Is Nothing OrElse Me.DockCode = String.Empty Then
            Throw New ApplicationException("DockCode was not specified.")
        End If

        If Me.DockScreenID Is Nothing OrElse Me.DockScreenID = String.Empty Then
            Throw New ApplicationException("DockScreenID was not specified.")
        End If
    End Sub

    Sub BuildControls()
        Dim user As String = Made4Net.Shared.Authentication.User.GetCurrentUser().UserName
        Dim list As New Made4Net.Schema.AppComponents.Docking.DockList(Me.DockCode, user, Me.DockScreenID)

        Dim builder As New Made4Net.WebControls.AppComponents.DockConfigTreeBuiler(list)
        Dim t As Made4Net.WebControls.TreeViewControl = builder.Build()
        t.ID = "Tree"
        ph.Controls.Add(t)
    End Sub

#End Region

End Class