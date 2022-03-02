Public Class PinMap
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents MPTest As Made4Net.WebControls.Map
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TECompanies As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDepots As Made4Net.WebControls.TableEditor
    'Protected WithEvents RadAjaxPanel1 As Telerik.WebControls.RadAjaxPanel
    Protected WithEvents Image1 As System.Web.UI.WebControls.Image
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents LoadingPanel1 As Telerik.WebControls.LoadingPanel

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
        'Put user code to initialize the page here
    End Sub

    Private Sub TECompanies_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompanies.CreatedGrid
        Try
            TECompanies.Grid.AddLinkColumn("geocode", "GeoCode", "GeoCode", "javascript:geoCodeObject('Contact','pointitem','{0}','null','null');ContactID", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnGeoCode"))
            TECompanies.Grid.AddLinkColumn("Center", "Center", "Center Contact On Map", "javascript:geoCodeObject('Company','gotopoint','{0}','null','null');PointId", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCenter"))

            TECompanies.Grid.AddLinkColumn("geocode2", "geocode2", "GeoCode2", "javascript:geoCodeObject('Contact','geocode2','{0}','{1}','null');ContactID;Address", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnGeoCode"))

        Catch ex As Exception
        End Try
    End Sub

    Private Sub TEDepots_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDepots.CreatedGrid
        Try
            TEDepots.Grid.AddLinkColumn("geocode", "GeoCode", "GeoCode", "javascript:geoCodeObject('Contact','pointitem','{0}','null','null');Contact", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnGeoCode"))
            TEDepots.Grid.AddLinkColumn("Center", "Center", "Center Depo On Map", "javascript:geoCodeObject('Company','gotopoint','{0}','null','null');PointId", "_self", 2, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCenter"))

        Catch ex As Exception
        End Try
    End Sub


End Class
