Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

<CLSCompliant(False)> Public Class Loading

#Region "Ctor"

    Public Sub New()

    End Sub

#End Region

#Region "Methods"

#Region "PickUp"

    Public Function PickupLoad(ByVal pLoadId As String, ByVal pCeateTask As Boolean, ByVal pUser As String) As LoadingJob
        Dim ret As Boolean = False

        'Commented for RWMS-2048 RWMS-1716
        'Dim SQL As String = String.Format("select * from vloadingloads where toload = '{0}'", pLoadId)
        'End Commented for RWMS-2048 RWMS-1716
        'Added for RWMS-2048 RWMS-1716
        Dim SQL As String = String.Format("select * from vloadingloads where toload = '{0}'  AND ISNULL(SHIPMENTSTATUS, N'') <> 'SHIPPED'", pLoadId)
        'End Added for RWMS-2048 RWMS-1716

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Load not found", "Load not found")
        Else
            Dim ship, ord As String
            ship = dt.Rows(0)("shipment")
            If ship = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Load must be assigned to a shipment", "Load must be assigned to a shipment")
            End If
            ord = dt.Rows(0)("orderid")
            If ord = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Load must be assigned to an order", "Load must be assigned to an order")
            End If
            Dim ldJob As New LoadingJob
            ldJob.Carrier = dt.Rows(0)("carrier")
            ldJob.CarrierName = dt.Rows(0)("carriername")
            ldJob.Comapny = dt.Rows(0)("targetcompany")
            ldJob.ComapnyName = dt.Rows(0)("companyname")
            ldJob.Consignee = dt.Rows(0)("consignee")
            ldJob.IsContainer = False
            ldJob.IsCase = False
            ldJob.LoadId = dt.Rows(0)("toload")
            ldJob.Picklist = dt.Rows(0)("picklist")
            ldJob.RequestedDate = dt.Rows(0)("requesteddate")
            ldJob.Shipment = ship
            ldJob.Sku = dt.Rows(0)("sku")
            ldJob.SkuDesc = dt.Rows(0)("skudesc")

            'RWMS-2238
            If Not IsDBNull(dt.Rows(0)("tolocation")) Then
                If Not String.IsNullOrEmpty(dt.Rows(0)("tolocation")) Then
                    ldJob.FromLocation = dt.Rows(0)("tolocation")
                Else
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet", "Cannot Load Pallet - Location is missing" + ldJob.LoadId)
                End If
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet", "Cannot Load Pallet - Location is missing" + ldJob.LoadId)
            End If
            'RWMS-2238

            ldJob.FromWarehousearea = dt.Rows(0)("towarehousearea")
            ldJob.ToWarehousearea = dt.Rows(0)("towarehousearea")
            ldJob.Units = dt.Rows(0)("units")
            ldJob.UOM = dt.Rows(0)("loaduom")
            If Not IsDBNull(dt.Rows(0)("Door")) Then
                ldJob.Door = dt.Rows(0)("Door")
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Door is missing", "Cannot Load Pallet - Door is missing")
            End If
            If Not IsDBNull(dt.Rows(0)("Driver1")) Then ldJob.Driver = dt.Rows(0)("Driver1")
            If Not IsDBNull(dt.Rows(0)("Seal1")) Then ldJob.Seal1 = dt.Rows(0)("Seal1")
            If Not IsDBNull(dt.Rows(0)("Seal2")) Then ldJob.Seal2 = dt.Rows(0)("Seal2")

            'RWMS-2674 - removing the Trailer validation which was included for RWMS-2499
            If Not IsDBNull(dt.Rows(0)("Trailer")) Then
                ldJob.Trailer = dt.Rows(0)("Trailer")
                'Else
                'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Trailer is missing", "Cannot Load Pallet - Trailer is missing")
            End If
            If Not IsDBNull(dt.Rows(0)("TransportReference")) Then ldJob.TransportReference = dt.Rows(0)("TransportReference")
            If Not IsDBNull(dt.Rows(0)("TransportType")) Then ldJob.TransportType = dt.Rows(0)("TransportType")

            'RWMS-2674 - removing the Vehicle validation which was included for RWMS-2499
            If Not IsDBNull(dt.Rows(0)("Vehicle")) Then
                ldJob.Vehicle = dt.Rows(0)("Vehicle")
                'Else
                'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Vehicle is missing", "Cannot Load Pallet - Vehicle is missing")
            End If
            ldJob.OrderId = ord
            ldJob.OrderLine = dt.Rows(0)("orderline")
            If pCeateTask Then
                Dim oLoadingTask As New LoadingTask
                oLoadingTask.CreateLoadLoadingTask(ldJob, pUser)
            End If
            Return ldJob
        End If
    End Function

    Public Function PickupContainer(ByVal pContId As String, ByVal pCeateTask As Boolean, ByVal pUser As String) As LoadingJob
        Dim ret As Boolean = False

        'Commented for RWMS-2048 RWMS-1716
        'Dim SQL As String = String.Format("select * from vLoadingContainer where containerid = '{0}'", pContId)
        'End Commented for RWMS-2048 RWMS-1716
        'Added for RWMS-2048 RWMS-1716
        Dim SQL As String = String.Format("select * from vLoadingContainer where containerid = '{0}' AND ISNULL(SHIPMENTSTATUS, N'') <> 'SHIPPED'", pContId)
        'End Added for RWMS-2048 RWMS-1716

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Container not found", "Container not found")
        Else
            Dim ship, ord As String
            ship = dt.Rows(0)("shipment")
            If ship = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Container must be assigned to a shipment", "Container must be assigned to a shipment")
            End If
            ord = dt.Rows(0)("orderid")
            If ord = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Container must be assigned to an order", "Container must be assigned to an order")
            End If
            Dim ldJob As New LoadingJob
            ldJob.Carrier = dt.Rows(0)("carrier")
            ldJob.CarrierName = dt.Rows(0)("carriername")
            ldJob.Comapny = dt.Rows(0)("targetcompany")
            ldJob.ComapnyName = dt.Rows(0)("companyname")
            ldJob.Consignee = dt.Rows(0)("consignee")
            ldJob.IsContainer = True
            ldJob.IsCase = False
            ldJob.LoadId = dt.Rows(0)("containerid")
            'ldJob.Picklist = dt.Rows(0)("picklist")
            ldJob.RequestedDate = dt.Rows(0)("requesteddate")
            ldJob.FromLocation = dt.Rows(0)("fromlocation")
            ldJob.FromWarehousearea = dt.Rows(0)("fromwarehousearea")
            ldJob.ToWarehousearea = dt.Rows(0)("towarehousearea")
            ldJob.Shipment = ship
            ldJob.OrderId = ord
            If Not IsDBNull(dt.Rows(0)("Door")) Then
                ldJob.Door = dt.Rows(0)("Door")
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Door is missing", "Cannot Load Pallet - Door is missing")
            End If
            If Not IsDBNull(dt.Rows(0)("Driver1")) Then ldJob.Driver = dt.Rows(0)("Driver1")
            If Not IsDBNull(dt.Rows(0)("Seal1")) Then ldJob.Seal1 = dt.Rows(0)("Seal1")
            If Not IsDBNull(dt.Rows(0)("Seal2")) Then ldJob.Seal2 = dt.Rows(0)("Seal2")

            'RWMS-2674 - removing the Trailer validation which was included for RWMS-2499
            If Not IsDBNull(dt.Rows(0)("Trailer")) Then
                ldJob.Trailer = dt.Rows(0)("Trailer")
                'Else
                'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Trailer is missing", "Cannot Load Pallet - Trailer is missing")
            End If
            If Not IsDBNull(dt.Rows(0)("TransportReference")) Then ldJob.TransportReference = dt.Rows(0)("TransportReference")
            If Not IsDBNull(dt.Rows(0)("TransportType")) Then ldJob.TransportType = dt.Rows(0)("TransportType")

            'RWMS-2674 - removing the Vehicle validation which was included for RWMS-2499
            If Not IsDBNull(dt.Rows(0)("Vehicle")) Then
                ldJob.Vehicle = dt.Rows(0)("Vehicle")
                'Else
                'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Vehicle is missing", "Cannot Load Pallet - Vehicle is missing")
            End If
            If pCeateTask Then
                Dim oLoadingTask As New LoadingTask
                oLoadingTask.CreateContainerLoadingTask(ldJob, pUser)
            End If
            Return ldJob
        End If
    End Function
    Public Function PickupCase(ByVal pCaseId As String, ByVal pCreateTask As Boolean, ByVal pUser As String) As LoadingJob
        Dim oCase As New WMS.Logic.CaseDetail(pCaseId)
        Dim SQL As String = String.Format("select * from vloadingloads where toload = '{0}'  AND ISNULL(SHIPMENTSTATUS, N'') <> 'SHIPPED'", oCase.ToLoad)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "No Payload found for the Case", "No Payload found for the Case")
        Else
            Dim ship, ord As String
            ship = dt.Rows(0)("shipment")
            If ship = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Case Payload must be assigned to a shipment", "Case Payload must be assigned to a shipment")
            End If
            ord = dt.Rows(0)("orderid")
            If ord = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Case Payload  must be assigned to an order", "Case Payload must be assigned to an order")
            End If
            Dim ldJob As New LoadingJob
            ldJob.Carrier = dt.Rows(0)("carrier")
            ldJob.CarrierName = dt.Rows(0)("carriername")
            ldJob.Comapny = dt.Rows(0)("targetcompany")
            ldJob.ComapnyName = dt.Rows(0)("companyname")
            ldJob.Consignee = dt.Rows(0)("consignee")
            ldJob.IsContainer = False
            ldJob.IsCase = True
            ldJob.LoadId = pCaseId
            ldJob.Picklist = dt.Rows(0)("picklist")
            ldJob.RequestedDate = dt.Rows(0)("requesteddate")
            ldJob.Shipment = ship
            ldJob.Sku = dt.Rows(0)("sku")
            ldJob.SkuDesc = dt.Rows(0)("skudesc")
            If Not IsDBNull(dt.Rows(0)("tolocation")) AndAlso Not String.IsNullOrEmpty(dt.Rows(0)("tolocation")) Then
                ldJob.FromLocation = dt.Rows(0)("tolocation")
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Location is missing for Case Payload", "Cannot Load Pallet - Location is missing for Case Payload " + oCase.ToLoad)
            End If
            ldJob.FromWarehousearea = dt.Rows(0)("towarehousearea")
            ldJob.ToWarehousearea = dt.Rows(0)("towarehousearea")
            If Not IsDBNull(dt.Rows(0)("Door")) Then
                ldJob.Door = dt.Rows(0)("Door")
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet - Door is missing", "Cannot Load Pallet - Door is missing")
            End If
            If Not IsDBNull(dt.Rows(0)("Driver1")) Then ldJob.Driver = dt.Rows(0)("Driver1")
            If Not IsDBNull(dt.Rows(0)("Seal1")) Then ldJob.Seal1 = dt.Rows(0)("Seal1")
            If Not IsDBNull(dt.Rows(0)("Seal2")) Then ldJob.Seal2 = dt.Rows(0)("Seal2")
            If Not IsDBNull(dt.Rows(0)("Trailer")) Then
                ldJob.Trailer = dt.Rows(0)("Trailer")
            End If
            If Not IsDBNull(dt.Rows(0)("TransportReference")) Then ldJob.TransportReference = dt.Rows(0)("TransportReference")
            If Not IsDBNull(dt.Rows(0)("TransportType")) Then ldJob.TransportType = dt.Rows(0)("TransportType")
            If Not IsDBNull(dt.Rows(0)("Vehicle")) Then
                ldJob.Vehicle = dt.Rows(0)("Vehicle")
            End If
            ldJob.OrderId = ord
            ldJob.OrderLine = dt.Rows(0)("orderline")
            If pCreateTask Then
                Dim oLoadingTask As New LoadingTask
                oLoadingTask.CreateCaseLoadingTask(ldJob, pUser)
            End If
            Return ldJob
        End If
    End Function

#End Region

#Region "Load"

    Public Sub LoadPallet(ByVal pLoadingJob As LoadingJob, ByVal pPosition As String, ByVal pConfirmation As String, ByVal pConfirmationWarehousearea As String, ByVal pUser As String, Optional ByVal pShipment As WMS.Logic.Shipment = Nothing)

        Dim oShip As Shipment
        If pShipment Is Nothing Then
            oShip = New Shipment(pLoadingJob.Shipment)
        Else
            oShip = pShipment
        End If
        'First verify that the location exists and that the location confirmed is the Door from the shipment Object
        'If Not Location.Exists(pConfirmation) Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet, Location does not exists", "Cannot Load Pallet, Location does not exists")
        'End If
        pConfirmation = Location.CheckLocationConfirmation(oShip.DOOR.ToLower, pConfirmation, pConfirmationWarehousearea)
        If oShip.DOOR.ToLower <> pConfirmation.ToLower Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Load Pallet, Wrong Confirmation", "Cannot Load Pallet, Confirmation Location does not match Shipment Door")
        End If
        If oShip.STATUS <> WMS.Lib.Statuses.Shipment.ATDOCK And oShip.STATUS <> WMS.Lib.Statuses.Shipment.LOADING Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Shipment status incorrect", "Shipment status incorrect")
        End If
        ''Verify Vehicle Position 
        If Not pPosition Is Nothing Then
            VerifyPosition(pLoadingJob.Vehicle, pPosition)
        End If
        'Then Start Loading
        If pLoadingJob.IsCase Then
            Dim oCase As CaseDetail = New CaseDetail(pLoadingJob.LoadId)
            If oCase.IsLastOneToBeLoadedInThePayload() Then
                oShip.ShipmentLoads.AddLoad(oCase.ToLoad, pUser)
            End If
            LoadCase(pLoadingJob, pConfirmation, pConfirmationWarehousearea, pPosition, pUser, oShip)
        ElseIf pLoadingJob.IsContainer Then
            ' Add all loads on container to shipment loads
            Dim ContainerLoadsSql As String = "SELECT LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & pLoadingJob.LoadId & "'"
            Dim ContainerLoadsDt As DataTable = New DataTable
            DataInterface.FillDataset(ContainerLoadsSql, ContainerLoadsDt)
            For Each ContainerLoadDr As DataRow In ContainerLoadsDt.Rows
                oShip.ShipmentLoads.AddLoad(ContainerLoadDr("LOADID"), pUser)
            Next
            ' Load All Container's Loads
            LoadContainer(pLoadingJob, pConfirmation, pConfirmationWarehousearea, pPosition, pUser, oShip)
        Else
            ' Add to shipment loads
            oShip.ShipmentLoads.AddLoad(pLoadingJob.LoadId, pUser)
            'Load Load
            LoadLoad(pLoadingJob, pConfirmation, pConfirmationWarehousearea, pPosition, pUser)
        End If

        oShip.LoadPallet(pUser)
    End Sub

    Private Sub LoadLoad(ByVal pLoadingJob As LoadingJob, ByVal pConfirmation As String, ByVal pConfirmationWarehousearea As String, ByVal pVehiclePosition As String, ByVal pUser As String)
        Dim oLoad As New WMS.Logic.Load(pLoadingJob.LoadId)
        oLoad.Load(pConfirmation, pConfirmationWarehousearea, pUser, Nothing, Nothing, Nothing, Nothing, True)
        'Add to shipment compartments
        If oLoad.ContainerId = String.Empty Then
            AddToShipmentCompartments(pLoadingJob.Shipment, pVehiclePosition, oLoad.LOADID, pUser)
        Else
            AddToShipmentCompartments(pLoadingJob.Shipment, pVehiclePosition, oLoad.ContainerId, pUser)
        End If
        'If there is a task - complete it
        Dim oLoadindTask As New LoadingTask
        oLoadindTask.CompleteTask(pLoadingJob, pUser)
    End Sub
    Private Sub LoadCase(ByVal pLoadingJob As LoadingJob, ByVal pConfirmation As String, ByVal pConfirmationWarehousearea As String, ByVal pVehiclePosition As String, ByVal pUser As String, Optional ByVal pShipment As WMS.Logic.Shipment = Nothing)
        Dim oCase As New WMS.Logic.CaseDetail(pLoadingJob.LoadId)
        oCase.Load(pConfirmation, pConfirmationWarehousearea, pUser, pShipment)
        AddToShipmentCompartments(pLoadingJob.Shipment, pVehiclePosition, oCase.ToContainer, pUser)
        Dim oLoadindTask As New LoadingTask
        oLoadindTask.CompleteTask(pLoadingJob, pUser)
    End Sub
    Private Sub LoadContainer(ByVal pLoadingJob As LoadingJob, ByVal pConfirmation As String, ByVal pConfirmationWarehousearea As String, ByVal pVehiclePosition As String, ByVal pUser As String, Optional ByVal pShipment As WMS.Logic.Shipment = Nothing)
        Dim oCont As New WMS.Logic.Container(pLoadingJob.LoadId, True)
        oCont.Load(pConfirmation, pConfirmationWarehousearea, pUser, pShipment)
        AddToShipmentCompartments(pLoadingJob.Shipment, pVehiclePosition, pLoadingJob.LoadId, pUser)
        'If there is a task - complete it
        Dim oLoadindTask As New LoadingTask
        oLoadindTask.CompleteTask(pLoadingJob, pUser)
    End Sub
    Private Sub VerifyPosition(ByVal pVehicleId As String, ByVal pPosition As String)
        Dim sql, vehicleType As String
        Dim dt As New DataTable
        If WarehouseParams.GetWarehouseParam("UseDefaultVehicleType") Then
            sql = "Select Location from vehiclelocations where vehicletype = 'default'"
        Else
            sql = String.Format("SELECT VEHICLETYPENAME FROM VEHICLE where VEHICLEID = {0}", Made4Net.Shared.FormatField(pVehicleId))
            vehicleType = DataInterface.ExecuteScalar(sql)
            sql = String.Format("Select Location from vehiclelocations where vehicletype={0}", Made4Net.Shared.FormatField(vehicleType))
        End If
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New M4NException(New Exception, "No Locations exists for the given vehicle type", "No Locations exists for the given vehicle type")
        End If
        For Each dr As DataRow In dt.Rows
            If pPosition.Equals(Convert.ToString(dr("Location")), StringComparison.InvariantCultureIgnoreCase) Then
                Return
            End If
        Next
        Throw New M4NException(New Exception, "Position does not exist.", "Position does not exist")
    End Sub

    Private Sub AddToShipmentCompartments(ByVal pShipmentId As String, ByVal pVehiclePosition As String, ByVal HandlingUnitId As String, ByVal pUser As String)
        If pVehiclePosition <> String.Empty Then
            Dim oCompartments As New Shipment.ShipmentCompartment
            oCompartments.Create(pShipmentId, pVehiclePosition, HandlingUnitId, "", pUser)
        End If
    End Sub

#End Region

#End Region

End Class