Public Class RouteStop
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TERouteStops As Made4Net.WebControls.TableEditor
    Protected WithEvents hdVal1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdValCommand As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdPointId As System.Web.UI.WebControls.TextBox

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
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TERouteStops.Width = New System.Web.UI.WebControls.Unit(200, UnitType.Pixel)
        If hdValCommand.Text = "hide" Then
            TERouteStops.Visible = False
        ElseIf hdValCommand.Text = "show" Then
            TERouteStops.Visible = True
            showRouteStopDetail()
        End If
    End Sub

    Private Sub showRouteStopDetail()
        TERouteStops.Visible = True
        TERouteStops.FilterExpression = "ROUTEID='" & hdVal1.Text & "'"
        TERouteStops.Restart()
    End Sub
End Class
