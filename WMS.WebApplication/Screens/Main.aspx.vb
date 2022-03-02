Imports Made4Net.WebControls.AppComponents

Public Class Main
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents ph As System.Web.UI.WebControls.PlaceHolder

    Private WithEvents _ACControl As AppComponentControl
    'Private WithEvents _ACControl2 As AppComponentControl

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
        'Put user code to initialize the page here
    End Sub

    Private Sub BuildAC()
        _ACControl = New AppComponentControl
        _ACControl.AppComponentCode = "MyDock"
        _ACControl.Width = System.Web.UI.WebControls.Unit.Percentage(80)
        _ACControl.Height = System.Web.UI.WebControls.Unit.Pixel(350)
        _ACControl.ID = "ACCONTROL1"
        ph.Controls.Add(_ACControl)

        '_ACControl2 = New AppComponentControl
        '_ACControl2.AppComponentCode = "PDock"
        '_ACControl2.Width = System.Web.UI.WebControls.Unit.Pixel(500)
        '_ACControl2.Height = System.Web.UI.WebControls.Unit.Pixel(300)
        'ph.Controls.Add(_ACControl2)
    End Sub

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()

        Me.BuildAC()
    End Sub

End Class