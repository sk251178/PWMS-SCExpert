Imports System
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
'Upgrade 2.0 to 4.5 for telerik
'Imports Telerik.WebControls
Imports Telerik.Web.UI
Imports WMS.Logic
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class LoadingPlan
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEShipments As Made4Net.WebControls.TableEditor
    Protected WithEvents RadTreeView1 As Telerik.Web.UI.RadTreeView
    Protected WithEvents rt As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Radtreeview2 As Telerik.Web.UI.RadTreeView
    Protected WithEvents truck As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tdLeftTruck As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents tdRightTruck As System.Web.UI.HtmlControls.HtmlTableCell
    Protected WithEvents LeftVehicle As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents RightVehicle As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected RightVehicleTable As Table
    Protected LeftVehicleTable As Table
    Protected _Shipment As String
    Protected _VehicleTypeName As String
    Protected WithEvents DropLocation As System.Web.UI.HtmlControls.HtmlInputHidden

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

    Private Sub TEShipments_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEShipments.RecordSelected

        _VehicleTypeName = TEShipments.CurrentRecordDataTable.Rows(0)("VEHICLETYPENAME")
        _Shipment = TEShipments.CurrentRecordDataTable.Rows(0)("shipment")
        HttpContext.Current.Session("VehicleTypeName") = _VehicleTypeName

        'create the trees
        GenerateTreeView(_Shipment, _VehicleTypeName, Radtreeview2, "RIGHT")
        GenerateTreeView(_Shipment, _VehicleTypeName, RadTreeView1, "LEFT")

        'create the vehicle schemas
        RightVehicleTable = GenerateVehicleSchema(TEShipments.CurrentRecordDataTable.Rows(0)("VEHICLETYPEName"), "RIGHT")
        LeftVehicleTable = GenerateVehicleSchema(TEShipments.CurrentRecordDataTable.Rows(0)("VEHICLETYPEName"), "LEFT")
        RightVehicleTable.ID = "RightVehicleTable"
        LeftVehicleTable.ID = "LeftVehicleTable"

        'show trees
        RadTreeView1.Visible = True
        Radtreeview2.Visible = True
        rt.Visible = True

        'show vehicles
        LeftVehicle.Controls.Clear()
        RightVehicle.Controls.Clear()
        LeftVehicle.Controls.Add(LeftVehicleTable)
        RightVehicle.Controls.Add(RightVehicleTable)
        LeftVehicle.Visible = True
        RightVehicle.Visible = True

    End Sub

    Private Sub GenerateTreeView(ByVal pShipmentID As String, ByVal pVehicleTypeName As String, ByRef pTree As Telerik.Web.UI.RadTreeView, ByVal pSide As String)
        'Dim Shipment As New RadTreeNode("Shipment: " & dr("shipment"))
        'pTree.AddNode(Shipment)
        'Dim currentLoc As New RadTreeNode("Vehicle Side: " & pSide)
        'Shipment.AddNode(currentLoc)
        'currentLoc.Visible = False
        'Shipment.Visible = False
        'Dim currentBay As New RadTreeNode("Bays")

        GenerateVehicleSideBays(pTree, pVehicleTypeName, pSide)
    End Sub

    Private Sub GenerateVehicleSideBays(ByRef pTree As Telerik.Web.UI.RadTreeView, ByVal pVehicletype As String, ByVal pSide As String)
        Dim dt As New DataTable
        Dim SQL As String
        SQL = String.Format("select distinct bay from vehiclelocations where vehicletype = {0} and side = {1} and status='active'", _
            Made4Net.Shared.Util.FormatField(pVehicletype), Made4Net.Shared.Util.FormatField(pSide))
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim currentBay As New RadTreeNode("Bay: " & dr("bay"))
            ' SIMON pTree.AddNode(currentBay)
            pTree.Nodes.Add(currentBay)
            GenerateBayLocations(currentBay, pVehicletype, pSide, dr("bay"))
        Next
    End Sub

    Private Sub GenerateBayLocations(ByRef currentNode As RadTreeNode, ByVal pVehicletype As String, ByVal pSide As String, ByVal pBay As String)
        Dim dt As New DataTable
        Dim SQL As String
        SQL = String.Format("select distinct location from vehiclelocations where vehicletype = {0} and side = {1} and bay = {2} and status='active'", _
            Made4Net.Shared.Util.FormatField(pVehicletype), Made4Net.Shared.Util.FormatField(pSide), Made4Net.Shared.Util.FormatField(pBay))
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim currentLoc As New RadTreeNode("Location: " & dr("location"))
            currentLoc.Value = "LOC_" & dr("location")
            'SIMON currentNode.AddNode(currentLoc)
            currentNode.Nodes.Add(currentLoc)
            GenerateCustNodes(currentLoc, _Shipment, dr("location"))
        Next
    End Sub

    Private Sub TEShipments_RecordUnSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEShipments.RecordUnSelected
        RadTreeView1.Nodes.Clear()
        Radtreeview2.Nodes.Clear()
        rt.Visible = False
        LeftVehicle.Visible = False
        RightVehicle.Visible = False
        HttpContext.Current.Items.Remove("VehicleTypeName")
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

    Private Sub GenerateOrderLinesView(ByRef currentNode As RadTreeNode, ByVal pShipmentId As String, ByVal pVehicleLocation As String, ByVal pCustomer As String)
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select * from LOADINGPLANVIEW where shipment = '{0}' and vehiclelocation = '{1}' and targetcompany = '{2}'" _
                , pShipmentId, pVehicleLocation, pCustomer)
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim currentOrder As New RadTreeNode("OrderId: " & dr("orderid") & " Line: " & dr("orderline") & " Item: " & dr("skudesc") & " QTY: " & dr("qty"))
            currentOrder.Value = "PLANID_" & dr("PLANID")
            'SIMON currentNode.AddNode(currentOrder)
            currentNode.Nodes.Add(currentOrder)
        Next
    End Sub

    Private Sub GenerateCustNodes(ByRef currentNode As RadTreeNode, ByVal pShipmentId As String, ByVal pVehicleLocation As String)
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select distinct targetcompany,companyname from LOADINGPLANVIEW where shipment = '{0}' and vehiclelocation = '{1}'", pShipmentId, pVehicleLocation)
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim currentCust As New RadTreeNode("Customer: " & dr("companyname"))
            currentCust.Value = "CUST_" & dr("targetcompany")
            'SIMON currentNode.AddNode(currentCust)
            currentNode.Nodes.Add(currentCust)
            GenerateOrderLinesView(currentCust, pShipmentId, pVehicleLocation, dr("targetcompany"))
        Next
    End Sub

    Public Sub HandleDrop(ByVal o As Object, ByVal e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs) 'Handles RadTreeView1.NodeDrop, Radtreeview2.NodeDrop
        ' Fetch event data
        Dim sourceNode As RadTreeNode = e.SourceDragNode
        Dim destNode As RadTreeNode = e.DestDragNode
        Dim parentNode As RadTreeNode
        parentNode = sourceNode.Parent

        If DropLocation.Value <> "NONE" Then
            If Not sourceNode.ID.StartsWith("PLANID") Then
                Exit Sub
            End If
            HandleHTMLDrop(sourceNode, DropLocation.Value)
            Exit Sub
        End If

        If Not destNode.Value.StartsWith("LOC") Then
            Exit Sub
        End If
        If Not sourceNode.Value.StartsWith("PLANID") Then
            Exit Sub
        End If
        If sourceNode.Value.StartsWith("PLANID") And Not destNode.Value.StartsWith("LOC") Then
            Exit Sub
        End If
        'if the drop is on another tree element
        ' Swap nodes in tree.
        'SIMON Dim nodeCollection As RadTreeNodeCollection = IIf(Not (sourceNode.Parent Is Nothing), sourceNode.Parent.Nodes, sourceNode.TreeViewParent.Nodes)

        'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5
        'Dim nodeCollection As RadTreeNodeCollection = IIf(Not (sourceNode.Parent Is Nothing), sourceNode.Parent.Nodes, sourceNode.TreeView.Nodes)
        Dim nodeCollection As RadTreeNodeCollection = IIf(Not (sourceNode.Parent Is Nothing), sourceNode.ParentNode.Nodes, sourceNode.TreeView.Nodes)

        nodeCollection.Remove(sourceNode)
        If TreeContainsNode(destNode, sourceNode.Parent) <> -1 Then
            Dim index As Int32 = TreeContainsNode(destNode, sourceNode.Parent)
            'SIMON destNode.Nodes(index).AddNode(sourceNode)
            destNode.Nodes(index).Nodes.Add(sourceNode)
        Else
            Dim newNode As New RadTreeNode
            'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5
            newNode.Text = sourceNode.ParentNode.Text
            newNode.Value = sourceNode.ParentNode.Value
            'newNode.Text = sourceNode.Parent.Text
            'newNode.Value = sourceNode.Parent.Value
            'SIMON newNode.AddNode(sourceNode)
            newNode.Nodes.Add(sourceNode)
            'SIMON destNode.AddNode(newNode)
            destNode.Nodes.Add(newNode)
        End If
        If parentNode.Nodes.Count = 0 Then
            'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5
            'parentNode.Parent.Nodes.Remove(parentNode)
            parentNode.ParentNode.Nodes.Remove(parentNode)

        End If
        ' Expand destination node to see result of swap immediately.
        destNode.Expanded = True
        ' Clear last active selection.
        RadTreeView1.UncheckAllNodes()

        'and update the load plan table
        Dim SQL As String
        Dim planId As String = sourceNode.Value.Replace("PLANID_", "")
        Dim CurrLocationStatus As String
        _VehicleTypeName = TEShipments.CurrentRecordDataTable.Rows(0)("VEHICLETYPENAME")
        _Shipment = TEShipments.CurrentRecordDataTable.Rows(0)("shipment")
        'check if the status of the destination location is active
        SQL = String.Format("select status from VEHICLElocations where vehicletype = {0} and location = {1}", _
            Made4Net.Shared.Util.FormatField(_VehicleTypeName), Made4Net.Shared.Util.FormatField(destNode.ID.Replace("LOC_", "")))
        CurrLocationStatus = DataInterface.ExecuteScalar(SQL)
        If CurrLocationStatus.ToLower = "active" Then
            'update the loading plans table
            SQL = String.Format("UPDATE LOADINGPLAN SET VEHICLELOCATION = {0} where PLANID = {1}", _
            Made4Net.Shared.Util.FormatField(destNode.ID.Replace("LOC_", "")), planId)
            DataInterface.RunSQL(SQL)
        End If

        'create the vehicle schemas
        _VehicleTypeName = HttpContext.Current.Session("VehicleTypeName")
        If _VehicleTypeName Is Nothing Or _VehicleTypeName = "" Then
            Exit Sub
        End If
        RightVehicleTable = GenerateVehicleSchema(_VehicleTypeName, "RIGHT")
        LeftVehicleTable = GenerateVehicleSchema(_VehicleTypeName, "LEFT")
        RightVehicleTable.ID = "RightVehicleTable"
        LeftVehicleTable.ID = "LeftVehicleTable"

        LeftVehicle.Controls.Clear()
        RightVehicle.Controls.Clear()
        LeftVehicle.Controls.Add(LeftVehicleTable)
        RightVehicle.Controls.Add(RightVehicleTable)
    End Sub

    Private Sub HandleHTMLDrop(ByVal sourceNode As RadTreeNode, ByVal pTargetLocationId As String)
        _VehicleTypeName = TEShipments.CurrentRecordDataTable.Rows(0)("VEHICLETYPENAME")
        _Shipment = TEShipments.CurrentRecordDataTable.Rows(0)("shipment")

        Dim SQL As String
        Dim planId As String = sourceNode.ID.Replace("PLANID_", "")
        Dim CurrLocationStatus As String
        'to implement the Expand Nodes after the drop
        Dim tmpNode As RadTreeNode = sourceNode

        'check if the status of the destination location is active
        pTargetLocationId = pTargetLocationId.ToUpper.Replace("RIGHT", "").Replace("LEFT", "")
        SQL = String.Format("select status from VEHICLElocations where vehicletype = {0} and location = {1}", _
            Made4Net.Shared.Util.FormatField(_VehicleTypeName), Made4Net.Shared.Util.FormatField(pTargetLocationId))
        CurrLocationStatus = DataInterface.ExecuteScalar(SQL)
        If CurrLocationStatus.ToLower = "active" Then
            'update the loading plans table
            SQL = String.Format("UPDATE LOADINGPLAN SET VEHICLELOCATION = {0} where PLANID = {1}", _
            Made4Net.Shared.Util.FormatField(pTargetLocationId), planId)
            DataInterface.RunSQL(SQL)
            'create the trees
            RadTreeView1.Nodes.Clear()
            Radtreeview2.Nodes.Clear()
            GenerateTreeView(_Shipment, _VehicleTypeName, Radtreeview2, "RIGHT")
            GenerateTreeView(_Shipment, _VehicleTypeName, RadTreeView1, "LEFT")
        End If
        'create the vehicle schemas
        _VehicleTypeName = HttpContext.Current.Session("VehicleTypeName")
        If _VehicleTypeName Is Nothing Or _VehicleTypeName = "" Then
            Exit Sub
        End If
        RightVehicleTable = GenerateVehicleSchema(_VehicleTypeName, "RIGHT")
        LeftVehicleTable = GenerateVehicleSchema(_VehicleTypeName, "LEFT")
        RightVehicleTable.ID = "RightVehicleTable"
        LeftVehicleTable.ID = "LeftVehicleTable"

        LeftVehicle.Controls.Clear()
        RightVehicle.Controls.Clear()
        LeftVehicle.Controls.Add(LeftVehicleTable)
        RightVehicle.Controls.Add(RightVehicleTable)
    End Sub

    Private Function TreeContainsNode(ByVal oDestNode As RadTreeNode, ByVal oNodeToFind As RadTreeNode) As Int32
        For Each oNode As RadTreeNode In oDestNode.Nodes
            If oNode.ID = oNodeToFind.ID Then
                Return oDestNode.Nodes.IndexOf(oNode)
            End If
        Next
        Return -1
    End Function

End Class