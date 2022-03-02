Public Class VehiclePosition
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents MPVehiclePos As Made4Net.WebControls.Map
    Protected WithEvents TEVehiclePosition As Made4Net.WebControls.TableEditor
    Protected WithEvents TEVehicleRoutes As Made4Net.WebControls.TableEditor
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable

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

    End Sub

    Private Sub TEVehiclePosition_CreatedGrid(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEVehiclePosition.CreatedGrid
        If Not TEVehiclePosition.Grid Is Nothing Then
            TEVehiclePosition.Grid.AddLinkColumn("ShowVehicle", "Show Vehicle", "Click To View Vehicle Location", "javascript:geoCodeObject('Vehicle','showroute','{0}','{1}','{2}');LATITUDE;LONGITUDE;VEHICLEID", "_self", 0, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
        End If
    End Sub

    Private Sub TEVehicleRoutes_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEVehicleRoutes.CreatedGrid
        AddHandler TEVehicleRoutes.Grid.DataCreated, AddressOf TEVehicleRoutesDataCreated
        If Not TEVehicleRoutes.Grid Is Nothing Then
            TEVehicleRoutes.Grid.AddLinkColumn("ShowVehicleRoute", "Show Vehicle Route", "Click To View Vehicle Route", "javascript:geoCodeObject('VehicleRoute','showroute','{0}','{1}','{2}');VEHICLEID;MinTimeStamp;MaxTimeStamp", "_self", 0, Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
        End If
    End Sub


    Sub TEVehicleRoutesDataCreated(ByVal source As Object, ByVal e As Made4Net.WebControls.DataGridDataCreatedEventArgs)
        Try
            If Not TEVehicleRoutes.SearchForm Is Nothing Then
                Dim fromdate As DateTime
                Dim fromTime, toTime, strFromDate, strToDate, vehicleId As String
                Dim ctrl1 As Made4Net.WebControls.TextBoxValidated
                Dim ctrl As Made4Net.WebControls.DisplayTypes.TextBox
                Dim datectrl As Made4Net.WebControls.DisplayTypes.DateTimeBox
                Try
                    ctrl1 = TEVehicleRoutes.SearchForm.Controls(0).FindControl("Field_ACTIVITYTIME").Controls(0)
                    fromTime = ctrl1.Value
                    ctrl1.Value = ""
                Catch ex As Exception
                    fromTime = ""
                End Try
                Try
                    ctrl1 = TEVehicleRoutes.SearchForm.Controls(0).FindControl("Field_ACTIVITYTIME").Controls(2)
                    toTime = ctrl1.Value
                    toTime = toTime.Split(":")(0) & ":" & (Convert.ToInt32(toTime.Split(":")(1)) + 1).ToString.PadLeft(2, "0")
                    ctrl1.Value = ""
                Catch ex As Exception
                    toTime = "23:59"
                End Try
                Try
                    datectrl = TEVehicleRoutes.SearchForm.Controls(0).FindControl("Field_TIMESTAMP")
                    fromdate = Convert.ToDateTime(datectrl.Value)
                    datectrl.Value = ""
                Catch ex As Exception
                    fromdate = DateTime.MinValue
                End Try
                Try
                    ctrl = TEVehicleRoutes.SearchForm.Controls(0).FindControl("Field_VEHICLEID")
                    vehicleId = ctrl.Value
                    ctrl.Value = ""
                Catch ex As Exception
                    fromTime = ""
                End Try

                If fromdate <> DateTime.MinValue Then
                    strFromDate = fromdate.ToString("yyyy-MM-dd") & " " & fromTime
                    strFromDate = strFromDate.Trim
                    strToDate = fromdate.ToString("yyyy-MM-dd") & " " & toTime
                    strToDate = strToDate.Trim
                    Dim SQL As String = String.Format("SELECT dbo.VEHICLEPOSITION.VEHICLEID, dbo.VEHICLEPOSITION.LONGITUDE as MinLong, dbo.VEHICLEPOSITION.LATITUDE as MinLat," & _
                        " dbo.VEHICLEPOSITION.[TIMESTAMP] as MinTimeStamp, VEHICLEPOSITION_1.LONGITUDE AS MaxLong, VEHICLEPOSITION_1.LATITUDE AS MaxLat, " & _
                        " VEHICLEPOSITION_1.[TIMESTAMP] AS MaxTimeStamp" & _
                        " FROM  (SELECT VEHICLEID, MIN(RUNID) AS RUNID" & _
                        "        FROM dbo.VEHICLEPOSITION where '{0}' <= [TimeStamp]  " & _
                        "       GROUP BY VEHICLEID) VEHICLEPOSITIONMIN LEFT OUTER JOIN" & _
                        " dbo.VEHICLEPOSITION ON VEHICLEPOSITIONMIN.RUNID = dbo.VEHICLEPOSITION.RUNID AND " & _
                        " VEHICLEPOSITIONMIN.VEHICLEID  = dbo.VEHICLEPOSITION.VEHICLEID LEFT OUTER JOIN" & _
                        " (SELECT VEHICLEID, MAX(RUNID) AS RUNID" & _
                        "   FROM dbo.VEHICLEPOSITION  where '{1}' >= [TimeStamp]" & _
                        "   GROUP BY VEHICLEID) VEHICLEPOSITIONMAX LEFT OUTER JOIN" & _
                        " dbo.VEHICLEPOSITION VEHICLEPOSITION_1 ON VEHICLEPOSITIONMAX.RUNID = VEHICLEPOSITION_1.RUNID AND " & _
                        " VEHICLEPOSITIONMAX.VEHICLEID  = VEHICLEPOSITION_1.VEHICLEID ON " & _
                        " dbo.VEHICLEPOSITION.VEHICLEID = VEHICLEPOSITION_1.VEHICLEID where VEHICLEPOSITION.VEHICLEID = '{2}'", strFromDate, strToDate, vehicleId)
                    'TEVehicleRoutes.SQL = SQL
                    Dim m As New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(SQL, m)
                    e.Data = m
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

End Class
