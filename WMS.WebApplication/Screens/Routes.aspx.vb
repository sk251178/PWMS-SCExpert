Public Class Routes
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TERouteList As Made4Net.WebControls.TableEditor
    Protected WithEvents MPTest As Made4Net.WebControls.Map
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable

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
        'Put user code to initialize the page here
    End Sub



    Private Sub TERouteList_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERouteList.CreatedGrid
        TERouteList.Grid.AddLinkColumn("ShowRoute", "Show Route", "Click To View Route", "javascript:geoCodeObject('Route','showroute','{0}','null','null');RouteId", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
    End Sub
End Class
