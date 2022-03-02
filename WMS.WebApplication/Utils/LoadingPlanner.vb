Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports WMS.Logic

Public Class LoadingPlanner

    Private _ShipReq As DataTable
    Private _VehicleLoc As DataTable
 
    Public Sub New()
        _ShipReq = New DataTable
        _VehicleLoc = New DataTable
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        _ShipReq = New DataTable
        _VehicleLoc = New DataTable
        If CommandName = "PlanShipments" Then
            For Each dr In ds.Tables(0).Rows
                Plan(dr("SHIPMENT"), dr("vehicletypename"), dr("DOOR"))
            Next
        End If
    End Sub

    Public Sub Plan(ByVal pShipmentId As String, ByVal pVehicleType As String, ByVal pSide As String)
        DeleteAll(pShipmentId)
        InitShipReq(pShipmentId)
        InitVehicleLoc(pVehicleType)
        PlanShipment()
    End Sub

    Private Sub DeleteAll(ByVal pShipmentId As String)
        Dim SQL As String
        SQL = String.Format("delete from loadingplan where shipment = {0} ", _
            Made4Net.Shared.Util.FormatField(pShipmentId))
        DataInterface.RunSQL(SQL)
    End Sub

    Private Sub InitShipReq(ByVal pShipmentId As String)
        Dim SQL As String
        SQL = String.Format("SELECT * from shipmentrequirements where shipment = {0} order by qtymodified desc", _
            Made4Net.Shared.Util.FormatField(pShipmentId))
        DataInterface.FillDataset(SQL, _ShipReq)
    End Sub

    Private Sub InitVehicleLoc(ByVal pVehicleType As String)
        Dim SQL As String
        SQL = String.Format("select * from vehiclelocations where vehicletype = {0} and status = 'active'", _
          Made4Net.Shared.Util.FormatField(pVehicleType))
        DataInterface.FillDataset(SQL, _VehicleLoc)
    End Sub

    Private Sub PlanShipment()
        Dim drShipReq As DataRow
        Dim drLoc As DataRow
        Dim found As Boolean = False
        For Each drShipReq In _ShipReq.Rows
            Dim shipment As String, cons As String, sk As String, qty As Int32
            shipment = drShipReq("shipment")
            cons = drShipReq("consignee")
            sk = drShipReq("sku")
            qty = drShipReq("remainQty")
            If qty > 0 Then
                For Each drLoc In _VehicleLoc.Rows
                    If CanPlace(shipment, drLoc("location"), cons, sk, qty) Then
                        PutItemInLoc(drShipReq, drLoc("location"))
                        found = True
                        Exit For
                    End If
                Next
            End If
            If Not found Then
                PutItemInLoc(drShipReq)
            End If
            found = False
        Next
    End Sub

    Private Function CanPlace(ByVal pShipment As String, ByVal pLocation As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pQty As Int32) As Boolean
        Dim SQl As String
        Dim oSku As WMS.Logic.SKU = New WMS.Logic.SKU(pConsignee, pSku)
        Dim dt As New DataTable
        Dim dr As DataRow() = _VehicleLoc.Select("location = '" & pLocation & "'")
        Dim MaxLayers As Int32 = dr(0).Item("height") \ oSku.UOM("LAYER").HEIGHT
        SQl = String.Format("select isnull(sum(qty),0) as qty from loadingplanview where shipment = {0} and vehiclelocation = {1} ", _
            Made4Net.Shared.Util.FormatField(pShipment), Made4Net.Shared.Util.FormatField(pLocation))
        Dim CurrQty As Int32 = DataInterface.ExecuteScalar(SQl)
        Dim numLayers As Int32 = Convert.ToInt32(CurrQty / oSku.ConvertToUnits("LAYER") + 0.5)
        'Dim numLayers As Int32 = oSku.ConvertUnitsToUom("LAYER", CurrQty)
        If MaxLayers > numLayers And pQty < (oSku.ConvertToUnits("LAYER") * (MaxLayers - numLayers)) Then
            Return True
        End If
        Return False
    End Function

    Private Sub PutItemInLoc(ByRef dr As DataRow, Optional ByVal loc As String = "")
        If loc = "" Then loc = "000"
        Dim SQL As String = String.Format("INSERT INTO LOADINGPLAN  (SHIPMENT, VEHICLELOCATION, CONSIGNEE, ORDERID, ORDERLINE, QTY, ADDUSER, ADDDATE, EDITUSER, EDITDATE) " & _
            "VALUES ('{0}','{1}','{2}','{3}',{4},{5},'{6}',{7},'{8}',{9}) ", _
            dr("SHIPMENT"), loc, dr("consignee"), dr("orderid"), dr("orderline"), dr("remainqty"), "SYSTEM", Made4Net.Shared.Util.FormatField(DateTime.Now()), "SYSTEM", Made4Net.Shared.Util.FormatField(DateTime.Now()))
        DataInterface.RunSQL(SQL)
        dr("remainQty") = 0
    End Sub

End Class
