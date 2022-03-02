Public Class RoutingSetDisplay
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TERoutingSet As Made4Net.WebControls.TableEditor
    Protected WithEvents MPVehiclePos As Made4Net.WebControls.Map

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
        'If Not IsPostBack Then
        '    ddRS.DataBind()
        '    ddRS_SelectedIndexChanged(Nothing, Nothing)
        'End If
    End Sub

    Private Sub ddRS_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Try
        '    TERoutingSet.FilterExpression = "setid ='" & ddRS.SelectedValue & "'"
        'Catch ex As Exception
        'End Try
    End Sub

    Private Sub TERoutingSet_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutingSet.CreatedGrid
        Try
            TERoutingSet.Grid.AddLinkColumn("ShowRouteSet", "Show Route Set", "Click To View Routes", "javascript:geoCodeObject('RouteSet','showroute','{0}','null','null');SetId", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
        Catch ex As Exception
        End Try
    End Sub

End Class
