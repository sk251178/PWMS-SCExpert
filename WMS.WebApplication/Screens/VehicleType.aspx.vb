Imports WMS.Logic
Imports Made4Net.DataAccess
Imports System
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls


Public Class VehicleType
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEVehicleType As Made4Net.WebControls.TableEditor
    Protected WithEvents TEVehicleTypeClass As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEVehicleLocations As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    'div to display the vehicle schema
    Protected WithEvents LeftVehicle As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents RightVehicle As System.Web.UI.HtmlControls.HtmlGenericControl
    'tables of schema
    Protected RightVehicleTable As Table
    Protected LeftVehicleTable As Table
    <CLSCompliant(False)> Protected _VehicleTypeName As String

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
        If Page.IsPostBack Then
            Try
                _VehicleTypeName = HttpContext.Current.Session("VehicleTypeName")
                If _VehicleTypeName Is Nothing Or _VehicleTypeName = "" Then
                    Exit Sub
                End If
                'create the vehicle schemas
                RightVehicleTable = GenerateVehicleSchema(_VehicleTypeName, "RIGHT")
                LeftVehicleTable = GenerateVehicleSchema(_VehicleTypeName, "LEFT")
                RightVehicleTable.ID = "RightVehicleTable"
                LeftVehicleTable.ID = "LeftVehicleTable"

                LeftVehicle.Controls.Clear()
                RightVehicle.Controls.Clear()
                LeftVehicle.Controls.Add(LeftVehicleTable)
                RightVehicle.Controls.Add(RightVehicleTable)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Function GenerateVehicleSchema(ByVal pVehicletype As String, ByVal pSide As String) As Table
        Dim oVehicleDrawer As New VehicleDrawer
        oVehicleDrawer.TruckType = pVehicletype
        oVehicleDrawer.TruckSide = pSide
        If pSide.ToLower = "left" Then
            oVehicleDrawer.DriverCabImageUrl = "LeftDriverCabImageUrl"
            oVehicleDrawer.TruckWheelsImageUrl = "LeftTruckWheelsImageUrl"
        Else
            oVehicleDrawer.DriverCabImageUrl = "RightDriverCabImageUrl"
            oVehicleDrawer.TruckWheelsImageUrl = "RightTruckWheelsImageUrl"
        End If
        oVehicleDrawer.CreateTruck()
        Return oVehicleDrawer.Truck
    End Function

    Private Sub TEVehicleType_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEVehicleType.RecordSelected
        _VehicleTypeName = TEVehicleType.CurrentRecordDataTable.Rows(0)("VEHICLETYPENAME")
        HttpContext.Current.Session("VehicleTypeName") = _VehicleTypeName
    End Sub

    Private Sub TEVehicleType_RecordUnSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEVehicleType.RecordUnSelected
        HttpContext.Current.Items.Remove("VehicleTypeName")
    End Sub
End Class