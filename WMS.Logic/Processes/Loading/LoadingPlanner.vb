Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class LoadingPlanner

#Region "Variables"

    Private _VehicleLoc As DataTable
    Private _pcks As PicksObjectCollection
    Private _shipment As String

#End Region

#Region "Properties"

    Public ReadOnly Property VehicleLocation() As DataTable
        Get
            Return _VehicleLoc
        End Get
    End Property

    Public ReadOnly Property PickingCollection() As PicksObjectCollection
        Get
            Return _pcks
        End Get
    End Property

#End Region

#Region "Ctors"

    Public Sub New()
        _VehicleLoc = New DataTable
    End Sub

#End Region

#Region "Methods"

    Public Sub Post(ByVal pcks As PicksObjectCollection, ByVal pPickListId As String)
        PlanPicks(pcks, pPickListId)
    End Sub

    Public Sub PlanPicks(ByVal pcks As PicksObjectCollection, ByVal pPickListId As String)
        Dim drLoc As DataRow
        Dim found As Boolean = False
        Dim pck As PicksObject
        Dim loc As String, hutype As String
        'get the order and shipment from the pick object
        Dim oOrd As New OutboundOrderHeader(pcks(0).Consignee, pcks(0).OrderId)
        _shipment = "" 'oOrd.SHIPMENT
        If Not _shipment = "" Then
            'init the vehicle locations for the shipment - the shipments holds the vehicle type
            InitVehicleLoc(_shipment)
            For Each drLoc In _VehicleLoc.Rows
                If CanPlace(drLoc, pcks) Then
                    PlacePicks(pcks, _shipment, pPickListId, drLoc("location"))
                    found = True
                    Exit For
                End If
            Next
            If Not found Then
                PlacePicks(pcks, _shipment, pPickListId)
            End If
        End If
    End Sub

    Public Sub InitVehicleLoc(ByVal pShipment As String)
        _shipment = pShipment
        Dim SQL As String
        SQL = String.Format("select * from LoadingPlanVehicleLocations where shipment = {0} and status = 'active'", _
          Made4Net.Shared.Util.FormatField(pShipment))
        DataInterface.FillDataset(SQL, _VehicleLoc)
    End Sub

    Private Sub PlacePicks(ByVal pcks As PicksObjectCollection, ByVal pShipment As String, ByVal pPickListId As String, Optional ByVal pLocation As String = "000")
        Dim i As Int32 = 1
        For Each pick As PicksObject In pcks
            Dim lp As New LoadingPlan
            lp.Create("", pShipment, pLocation, pcks(0).ContainerType, pick.Consignee, pick.OrderId, pick.OrderLine, pick.Units, pPickListId, i, "SYSTEM")
            i += 1
        Next
    End Sub

    Public Function CanPlace(ByVal drLoc As DataRow, ByVal pcks As PicksObjectCollection, Optional ByVal pck As PicksObject = Nothing) As Boolean
        'calc the cont height
        Dim ContHeight As Double = CalcContHeight(pcks)

        Dim pContWeight As Double = pcks.CurrWeight
        Dim pContVolume As Double = pcks.CurrVolume

        'If we get additional pick -> calc the new dimensions of the container
        If Not pck Is Nothing Then
            Dim pckvol, pckweight As Double
            pckvol = Inventory.CalculateVolume(pck.Consignee, pck.SKU, pck.Units, pck.UOM)
            pckweight = Inventory.CalculateWeight(pck.Consignee, pck.SKU, pck.Units, pck.UOM)
            pContWeight += pckweight
            pContVolume += pckvol
        End If

        'now check the limitations
        Dim LocWeightLimit, LocHeightLimit, LocVolumeLimit As Double
        If IsDBNull(drLoc("WEIGHTLIMIT")) Then LocWeightLimit = 0 Else LocWeightLimit = drLoc("WEIGHTLIMIT")
        If IsDBNull(drLoc("TOTALVOLUME")) Then LocVolumeLimit = 0 Else LocVolumeLimit = drLoc("TOTALVOLUME")
        If IsDBNull(drLoc("HEIGHT")) Then LocHeightLimit = 0 Else LocHeightLimit = drLoc("HEIGHT")
        If ContHeight > LocHeightLimit Then
            Return False
        End If

        'check the cont height vs the remaining height of the loc
        Dim inUse As Boolean = LocInUse(drLoc("Location"))
        If inUse Then Return False

        'Finally
        Return True
    End Function

    Private Function CalcContHeight(ByVal pcks As PicksObjectCollection) As Double
        Dim height As Double
        Dim hu As HandelingUnit = pcks.Container
        Dim HUheight, HUwidth As Double
        If hu.Height = 0 Then HUheight = 1 Else HUheight = hu.Height
        If hu.Width = 0 Then HUwidth = 1 Else HUwidth = hu.Width
        height = pcks.CurrVolume / (HUheight * HUwidth)
        Return height
    End Function

    Private Function LocInUse(ByVal pLoc As String) As Boolean
        Dim SQL As String
        SQL = String.Format("select count(1) from loadingplanview where shipment = {0} and vehiclelocation = {1} ", _
            Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(pLoc))

        Return Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
    End Function

    Public Shared Function CanPlaceContainer(ByVal pcks As PicksObjectCollection, ByVal pck As PicksObject) As Boolean
        'Get the shipment of the current order/PickList so we can Load the vehicle parameters
        Dim oOrd As New OutboundOrderHeader(pcks(0).Consignee, pcks(0).OrderId)
        Dim shipment As String = "" ' oOrd.SHIPMENT

        If pcks.Container Is Nothing Then Return True

        'now search for an available location on the vehicle according to the new dimensions
        Dim lp As New LoadingPlanner
        lp.InitVehicleLoc(shipment)
        For Each drLoc As DataRow In lp.VehicleLocation.Rows
            If lp.CanPlace(drLoc, pcks) Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

End Class
