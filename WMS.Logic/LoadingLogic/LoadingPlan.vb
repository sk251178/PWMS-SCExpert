Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class LoadingPlan

#Region "Variables"

#Region "Primary Keys"

    Protected _planid As String = String.Empty
    Protected _shipment As String = String.Empty
    Protected _vehiclelocation As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _handlingunit As String
    Protected _consignee As String
    Protected _orderid As String
    Protected _orderline As Int32
    Protected _qty As String
    Protected _picklistline As Int32
    Protected _picklist As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" PLANID = {0} and SHIPMENT = {1} and VEHICLELOCATION = {2}", _planid, _shipment, _vehiclelocation)
        End Get
    End Property

    Public Property PlanId() As String
        Get
            Return _planid
        End Get
        Set(ByVal Value As String)
            _planid = Value
        End Set
    End Property

    Public Property Shipment() As String
        Get
            Return _shipment
        End Get
        Set(ByVal Value As String)
            _shipment = Value
        End Set
    End Property

    Public Property Vehiclelocation() As Int32
        Get
            Return _vehiclelocation
        End Get
        Set(ByVal Value As Int32)
            _vehiclelocation = Value
        End Set
    End Property

    Public Property HandlingUnit() As String
        Get
            Return _handlingunit
        End Get
        Set(ByVal Value As String)
            _handlingunit = Value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property OrderId() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
        End Set
    End Property

    Public Property OrderLine() As Int32
        Get
            Return _orderline
        End Get
        Set(ByVal Value As Int32)
            _orderline = Value
        End Set
    End Property

    Public Property QTY() As Double
        Get
            Return _qty
        End Get
        Set(ByVal Value As Double)
            _qty = Value
        End Set
    End Property

    Public Property PickList() As String
        Get
            Return _picklist
        End Get
        Set(ByVal Value As String)
            _picklist = Value
        End Set
    End Property

    Public Property PickListLine() As Int32
        Get
            Return _picklistline
        End Get
        Set(ByVal Value As Int32)
            _picklistline = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pPlanId As String, ByVal pShipmentId As String, ByVal pVehicleLocation As String)
        _planid = pPlanId
        _shipment = pShipmentId
        _vehiclelocation = pVehicleLocation

        Dim SQL As String = "SELECT * FROM loadingplan Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Loadingplan does not exists", "ASNDetail does not exists")
        End If

        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal pPlanId As String, ByVal pShipmentId As String, ByVal pVehicleLocation As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from loadingplan where PLANID = {0} and SHIPMENT = {1} and VEHICLELOCATION = {2}", pPlanId, pShipmentId, pVehicleLocation)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("PLANID") Then _planid = dr.Item("PLANID")
        If Not dr.IsNull("SHIPMENT") Then _shipment = dr.Item("SHIPMENT")
        If Not dr.IsNull("VEHICLELOCATION") Then _vehiclelocation = dr.Item("VEHICLELOCATION")
        If Not dr.IsNull("HANDLINGUNIT") Then _handlingunit = dr.Item("HANDLINGUNIT")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("QTY") Then _qty = dr.Item("QTY")
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PICKLISTLINE") Then _picklistline = dr.Item("PICKLISTLINE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM loadingplan Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Loadingplan does not exists", "ASN does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("PLANID") Then _planid = dr.Item("PLANID")
        If Not dr.IsNull("SHIPMENT") Then _shipment = dr.Item("SHIPMENT")
        If Not dr.IsNull("VEHICLELOCATION") Then _vehiclelocation = dr.Item("VEHICLELOCATION")
        If Not dr.IsNull("HANDLINGUNIT") Then _handlingunit = dr.Item("HANDLINGUNIT")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("QTY") Then _qty = dr.Item("QTY")
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PICKLISTLINE") Then _picklistline = dr.Item("PICKLISTLINE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

#Region "Create"

    Public Sub Save()
        Dim SQL As String
        If Exists(_planid, _shipment, _vehiclelocation) Then
            SQL = String.Format("UPDATE LOADINGPLAN SET HANDLINGUNIT = {0}, CONSIGNEE = {1}, ORDERID = {2}, " & _
                " ORDERLINE = {3}, QTY = {4}, PICKLIST = {5},PICKLISTLINE = {6},EDITUSER = {7}, EDITDATE = {8} where {9}", _
                 Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(OrderId), Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        Else
            _planid = Made4Net.Shared.Util.getNextCounter("LOADINGPLAN")
            SQL = String.Format("INSERT INTO LOADINGPLAN (PLANID, SHIPMENT, VEHICLELOCATION, HANDLINGUNIT, CONSIGNEE, ORDERID, ORDERLINE, QTY, PICKLIST ,PICKLISTLINE, ADDDATE, ADDUSER, EDITDATE, EDITUSER ) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _
               Made4Net.Shared.Util.FormatField(_planid), Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_vehiclelocation), Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
               Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Create(ByVal pPlanId As String, ByVal pShipment As String, ByVal pVehicleLocation As String, ByVal pHandlingUnit As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pQTY As Double, ByVal pPickListId As String, ByVal pPickListLine As Int32, ByVal pUser As String)
        If pPlanId = "" Then
            _planid = Made4Net.Shared.Util.getNextCounter("LOADINGPLAN")
        Else
            _planid = pPlanId
        End If
        _shipment = pShipment
        _vehiclelocation = pVehicleLocation
        _handlingunit = pHandlingUnit
        _consignee = pConsignee
        _orderid = pOrderId
        _orderline = pOrderLine
        _qty = pQTY
        _picklist = pPickListId
        _picklistline = pPickListLine
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String
        SQL = String.Format("INSERT INTO LOADINGPLAN (PLANID, SHIPMENT, VEHICLELOCATION, HANDLINGUNIT, CONSIGNEE, ORDERID, ORDERLINE, QTY, PICKLIST ,PICKLISTLINE, ADDDATE, ADDUSER, EDITDATE, EDITUSER ) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _
               Made4Net.Shared.Util.FormatField(_planid), Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_vehiclelocation), Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
               Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)

    End Sub

    Public Sub Edit(ByVal pHandlingUnit As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pQTY As Double, ByVal pPickListId As String, ByVal pPickListLine As Int32, ByVal pUser As String)
        _handlingunit = pHandlingUnit
        _consignee = pConsignee
        _orderid = pOrderId
        _orderline = pOrderLine
        _qty = pQTY
        _picklist = pPickListId
        _picklistline = pPickListLine
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String
        SQL = String.Format("UPDATE LOADINGPLAN SET HANDLINGUNIT = {0}, CONSIGNEE = {1}, ORDERID = {2}, " & _
                " ORDERLINE = {3}, QTY = {4}, PICKLIST = {5},PICKLISTLINE = {6},EDITUSER = {7}, EDITDATE = {8} where {9}", _
                 Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(OrderId), Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#End Region

End Class
