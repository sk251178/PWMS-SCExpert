Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Evaluation
Imports WMS.Logic

#Region "SHIPMENT"

' <summary>
' This object represents the properties and methods of a SHIPMENT.
' </summary>
Namespace AssignmentsGelsons

    Public Class ShipmentFS

#Region "Inner Classes"

#Region "Outbound Order Collection"

        '<CLSCompliant(False), Obsolete("This class is replaced by the shipment details collection (assign details by order line resolution)", True)> _
        Public Class OutboundOrderCollection
            Inherits ArrayList

#Region "Variables"

            Protected _shipment As String

#End Region

#Region "Properties"

            Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As OutboundOrderHeader
                Get
                    Return CType(MyBase.Item(index), OutboundOrderHeader)
                End Get
            End Property

#End Region

#Region "Constructors"

            Public Sub New(ByVal pShipment As String, Optional ByVal LoadAll As Boolean = True)
                _shipment = pShipment
                Dim SQL As String
                Dim dt As New DataTable
                Dim dr As DataRow
                'SQL = "Select * from OUTBOUNDORHEADER where shipment = '" & _shipment & "'"
                SQL = String.Format("select oh.* from OUTBOUNDORHEADER oh inner join (select distinct consignee, orderid from SHIPMENTDETAIL sd where SHIPMENT = '{0}') sd on sd.CONSIGNEE = oh.CONSIGNEE and sd.ORDERID = oh.ORDERID", _shipment)
                DataInterface.FillDataset(SQL, dt)
                For Each dr In dt.Rows
                    Me.add(New OutboundOrderHeader(dr))
                Next
            End Sub

#End Region

#Region "Methods"

            Public Function getOrder(ByVal Consignee As String, ByVal OrderId As String) As OutboundOrderHeader
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If Item(i).CONSIGNEE.ToUpper = Consignee.ToUpper And Item(i).ORDERID.ToUpper = OrderId.ToUpper Then
                        Return (CType(MyBase.Item(i), OutboundOrderHeader))
                    End If
                Next
                Return Nothing

            End Function

            Public Shadows Function add(ByVal pObject As OutboundOrderHeader) As Integer
                Return MyBase.Add(pObject)
            End Function

            Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As OutboundOrderHeader)
                MyBase.Insert(index, Value)
            End Sub

            Public Shadows Sub Remove(ByVal pObject As OutboundOrderHeader)
                MyBase.Remove(pObject)
            End Sub

#End Region

        End Class

#End Region

#Region "Shipment Details"

        <CLSCompliant(False)> Public Class ShipmentDetail

#Region "Variables"

            Protected _shipment As String = String.Empty
            Protected _consignee As String = String.Empty
            Protected _orderid As String = String.Empty
            Protected _orderline As Int32
            Protected _units As Decimal
            Protected _loadingseq As Int32
            Protected _adddate As DateTime
            Protected _adduser As String = String.Empty
            Protected _editdate As DateTime
            Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

            Public ReadOnly Property WhereClause() As String
                Get
                    Return String.Format("shipment = '{0}' and consignee = '{1}' and orderid = '{2}' and orderline = {3}", _shipment, _consignee, _orderid, _orderline)
                End Get
            End Property

            Public Property SHIPMENT() As String
                Set(ByVal value As String)
                    _shipment = value
                End Set
                Get
                    Return _shipment
                End Get
            End Property

            Public Property CONSIGNEE() As String
                Set(ByVal value As String)
                    _consignee = value
                End Set
                Get
                    Return _consignee
                End Get
            End Property

            Public Property ORDERID() As String
                Set(ByVal value As String)
                    _orderid = value
                End Set
                Get
                    Return _orderid
                End Get
            End Property

            Public Property ORDERLINE() As Int32
                Set(ByVal value As Int32)
                    _orderline = value
                End Set
                Get
                    Return _orderline
                End Get
            End Property

            Public Property UNITS() As Decimal
                Set(ByVal value As Decimal)
                    _units = value
                End Set
                Get
                    Return _units
                End Get
            End Property

            Public Property LOADINGSEQ() As Int32
                Set(ByVal value As Int32)
                    _loadingseq = value
                End Set
                Get
                    Return _loadingseq
                End Get
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

#Region "Ctors"

            Public Sub New()

            End Sub

            Public Sub New(ByVal pShipment As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32)
                _shipment = pShipment
                _consignee = pConsignee
                _orderid = pOrderId
                _orderline = pOrderLine
                Load()
            End Sub

            Public Sub New(ByVal dr As DataRow)
                Load(dr)
            End Sub

#End Region

#Region "Methods"

#Region "Accessors"

            Private Sub Load()
                Dim oSql As String = String.Format("select * from shipmentdetail where {0}", WhereClause)
                Dim dt As New DataTable
                DataInterface.FillDataset(oSql, dt)
                If dt.Rows.Count = 1 Then
                    Load(dt.Rows(0))
                Else
                    Throw New Made4Net.Shared.M4NException(New Exception, "Shipment detail does not exists", "Shipment detail does not exists")
                End If
            End Sub

            Private Sub Load(ByVal dr As DataRow)
                _shipment = dr("shipment")
                _consignee = dr("consignee")
                _orderid = dr("orderid")
                _orderline = dr("orderline")
                _units = dr("units")
                _loadingseq = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("loadingseq"), 0)
                _adddate = dr("adddate")
                _adduser = dr("adduser")
                _editdate = dr("editdate")
                _edituser = dr("edituser")
            End Sub

            Public Shared Function Exists(ByVal pShipmentId As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32) As Boolean
                Dim sql As String = String.Format("Select count(1) from shipmentdetail where shipment = '{0}' and consignee = '{1}' and orderid = '{2}' and orderline = {3}", pShipmentId, pConsignee, pOrderid, pOrderLine)
                Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
            End Function

            Private Shared Function LineAssignedToAnotherShipment(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32) As Boolean
                Dim sql As String = String.Format("Select count(1) from shipmentdetail where consignee = '{0}' and orderid = '{1}' and orderline = {2}", pConsignee, pOrderid, pOrderLine)
                Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
            End Function

#End Region

#Region "Create / Delete"

            Public Sub Create(ByVal pShipment As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32, ByVal pUnits As Decimal, ByVal pLoadingSequence As Int32, ByVal pUser As String)
                Dim oOrdDetail As New OutboundOrderHeader.OutboundOrderDetail(pConsignee, pOrderid, pOrderLine)
                If pUnits > oOrdDetail.OpenQtyLeftToAssignToShipment Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add shipment detail, open order line quantity is less than supplied quantity", "Cannot add shipment detail, open order line quantity is less than supplied quantity")
                End If
                _shipment = pShipment
                _consignee = pConsignee
                _orderid = pOrderid
                _orderline = pOrderLine
                _units = pUnits
                _loadingseq = pLoadingSequence
                _adddate = DateTime.Now
                _editdate = DateTime.Now
                _adduser = pUser
                _edituser = pUser

                Dim oSql As String = String.Format("INSERT INTO shipmentdetail (SHIPMENT, CONSIGNEE, ORDERID, ORDERLINE, UNITS, LOADINGSEQ, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", _
                        Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
                        Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
                        Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
                DataInterface.RunSQL(oSql)
            End Sub

            Public Sub Delete()
                Dim oSql As String = String.Format("delete from shipmentdetail where {0}", WhereClause)
                DataInterface.RunSQL(oSql)
            End Sub

            Public Sub AddQauntity(ByVal pUnits As Decimal, ByVal pUser As String)
                Dim oOrdDetail As New OutboundOrderHeader.OutboundOrderDetail(_consignee, _orderid, _orderline)
                'If pUnits + _units > oOrdDetail.OpenQtyLeftToAssignToShipment Then
                If pUnits > oOrdDetail.OpenQtyLeftToAssignToShipment Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add shipment detail, open order line quantity is less than supplied quantity", "Cannot add shipment detail, open order line quantity is less than supplied quantity")
                End If
                _units += pUnits
                _editdate = DateTime.Now
                _edituser = pUser
                Dim oSql As String = String.Format("update shipmentdetail set units={0},edituser={1},editdate={2} where {3}", _
                    Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                DataInterface.RunSQL(oSql)
            End Sub

            Public Sub MoveToShipment(ByVal pNewShipmentId As String, ByVal pUser As String)
                _editdate = DateTime.Now
                _edituser = pUser
                Dim oSql As String = String.Format("update shipmentdetail set shipment={0},edituser={1},editdate={2} where {3}", _
                    Made4Net.Shared.Util.FormatField(pNewShipmentId), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                DataInterface.RunSQL(oSql)
                _shipment = pNewShipmentId
            End Sub

            Public Sub SetLoadingSequence(ByVal pLoadingSequence As String, ByVal pUser As String)
                _loadingseq = pLoadingSequence
                _editdate = DateTime.Now
                _edituser = pUser
                Dim oSql As String = String.Format("update shipmentdetail set loadingseq={0},edituser={1},editdate={2} where {3}", _
                    Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
                DataInterface.RunSQL(oSql)
            End Sub

#End Region

#End Region

        End Class

#End Region

#Region "Shipment Details Collection"

        <CLSCompliant(False)> Public Class ShipmentDetailsCollection
            Inherits ArrayList

#Region "Variables"

            Protected _shipment As String

#End Region

#Region "Properties"

            Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As ShipmentDetail
                Get
                    Return CType(MyBase.Item(index), ShipmentDetail)
                End Get
            End Property

#End Region

#Region "Constructors"

            Public Sub New()
                'Default Empty Ctor
            End Sub

            Public Sub New(ByVal pShipmentId As String, Optional ByVal LoadAll As Boolean = True)
                _shipment = pShipmentId
                Dim SQL As String
                Dim dt As New DataTable
                Dim dr As DataRow
                SQL = "Select * from shipmentdetail where shipment = '" & _shipment & "'"
                DataInterface.FillDataset(SQL, dt)
                For Each dr In dt.Rows
                    Me.add(New ShipmentDetail(dr))
                Next
            End Sub

#End Region

#Region "Methods"

            Public Function getShipmentDetail(ByVal Consignee As String, ByVal OrderId As String, ByVal pOrderLine As Int32) As ShipmentDetail
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If Item(i).CONSIGNEE.ToUpper = Consignee.ToUpper And Item(i).ORDERID.ToUpper = OrderId.ToUpper And Item(i).ORDERLINE = pOrderLine Then
                        Return (CType(MyBase.Item(i), ShipmentDetail))
                    End If
                Next
                Return Nothing
            End Function

            Public Shadows Function add(ByVal pObject As ShipmentDetail) As Integer
                Return MyBase.Add(pObject)
            End Function

            Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As ShipmentDetail)
                MyBase.Insert(index, Value)
            End Sub

            Public Shadows Sub Remove(ByVal pObject As ShipmentDetail)
                MyBase.Remove(pObject)
            End Sub

#End Region

        End Class

#End Region

#Region "Flowthrough Collection"

        <CLSCompliant(False)> Public Class FlowthroughCollection
            Inherits ArrayList

#Region "Variables"

            Protected _shipment As String

#End Region

#Region "Properties"

            Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As Flowthrough
                Get
                    Return CType(MyBase.Item(index), Flowthrough)
                End Get
            End Property

#End Region

#Region "Constructors"

            Public Sub New(ByVal pShipment As String, Optional ByVal LoadAll As Boolean = True)
                _shipment = pShipment
                Dim SQL As String
                Dim dt As New DataTable
                Dim dr As DataRow
                SQL = "SELECT * FROM FLOWTHROUGHHEADER WHERE SHIPMENT = '" & _shipment & "'"
                DataInterface.FillDataset(SQL, dt)
                For Each dr In dt.Rows
                    Me.add(New Flowthrough(dr))
                Next
            End Sub

#End Region

#Region "Methods"

            Public Function getOrder(ByVal Consignee As String, ByVal Flowthrough As String) As Flowthrough
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If Item(i).CONSIGNEE = Consignee.ToUpper And Item(i).FLOWTHROUGH = Flowthrough.ToUpper Then
                        Return (CType(MyBase.Item(i), Flowthrough))
                    End If
                Next
                Return Nothing
            End Function

            Public Shadows Function add(ByVal pObject As Flowthrough) As Integer
                Return MyBase.Add(pObject)
            End Function

            Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As Flowthrough)
                MyBase.Insert(index, Value)
            End Sub

            Public Shadows Sub Remove(ByVal pObject As Flowthrough)
                MyBase.Remove(pObject)
            End Sub

#End Region

        End Class

#End Region

#Region "TransShipment Collection"

        <CLSCompliant(False)> Public Class TransShipmentCollection
            Inherits ArrayList

#Region "Variables"

            Protected _shipment As String

#End Region

#Region "Properties"

            Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As TransShipment
                Get
                    Return CType(MyBase.Item(index), TransShipment)
                End Get
            End Property

#End Region

#Region "Constructors"

            Public Sub New(ByVal pShipment As String, Optional ByVal LoadAll As Boolean = True)
                _shipment = pShipment
                Dim SQL As String
                Dim dt As New DataTable
                Dim dr As DataRow
                SQL = "SELECT * FROM TRANSSHIPMENT WHERE SHIPMENT = '" & _shipment & "'"
                DataInterface.FillDataset(SQL, dt)
                For Each dr In dt.Rows
                    Me.add(New TransShipment(dr))
                Next
            End Sub

#End Region

#Region "Methods"

            Public Function getOrder(ByVal Consignee As String, ByVal TransShipment As String) As TransShipment
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If Item(i).CONSIGNEE = Consignee.ToUpper And Item(i).TRANSSHIPMENT.ToUpper = TransShipment.ToUpper Then
                        Return (CType(MyBase.Item(i), TransShipment))
                    End If
                Next
                Return Nothing

            End Function

            Public Shadows Function add(ByVal pObject As TransShipment) As Integer
                Return MyBase.Add(pObject)
            End Function

            Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As TransShipment)
                MyBase.Insert(index, Value)
            End Sub

            Public Shadows Sub Remove(ByVal pObject As TransShipment)
                MyBase.Remove(pObject)
            End Sub

#End Region

        End Class

#End Region

#Region "ShipmentLoadsCollection"

        <CLSCompliant(False)> Public Class ShipmentLoadsCollection
            Inherits ArrayList

#Region "Variables"

            Protected _shipment As String

#End Region

#Region "Properties"

            Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As ShipmentLoad
                Get
                    Return CType(MyBase.Item(index), ShipmentLoad)
                End Get
            End Property

            Public ReadOnly Property ShipmentLoad(ByVal pLoadid As String) As ShipmentLoad
                Get
                    Dim i As Int32
                    For i = 0 To Me.Count - 1
                        If Item(i).LoadId = pLoadid Then
                            Return (CType(MyBase.Item(i), ShipmentLoad))
                        End If
                    Next
                    Return Nothing
                End Get
            End Property

#End Region

#Region "Constructors"

            Public Sub New(ByVal pShipment As String, Optional ByVal LoadAll As Boolean = True)
                _shipment = pShipment
                Dim SQL As String
                Dim dt As New DataTable
                Dim dr As DataRow
                SQL = "Select * from SHIPMENTLOADS where shipment = '" & _shipment & "'"
                DataInterface.FillDataset(SQL, dt)
                For Each dr In dt.Rows
                    Me.add(New ShipmentLoad(dr))
                Next
            End Sub

#End Region

#Region "Methods"

            Public Shadows Function add(ByVal pObject As ShipmentLoad) As Integer
                Return MyBase.Add(pObject)
            End Function

            Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As ShipmentLoad)
                MyBase.Insert(index, Value)
            End Sub

            Public Shadows Sub Remove(ByVal pObject As ShipmentLoad)
                MyBase.Remove(pObject)
            End Sub

            Public Sub AddLoad(ByVal pLoadid As String, ByVal pUser As String)
                Dim oShipmentLoad As ShipmentLoad = ShipmentLoad(pLoadid)
                If IsNothing(oShipmentLoad) Then
                    oShipmentLoad = New ShipmentLoad
                    oShipmentLoad.Create(_shipment, pLoadid, pUser)
                    Me.add(oShipmentLoad)
                End If
            End Sub

            Public Sub RemoveLoad(ByVal pLoadid As String, ByVal pUser As String)
                Dim oShipmentLoad As ShipmentLoad = ShipmentLoad(pLoadid)
                If Not IsNothing(oShipmentLoad) Then
                    oShipmentLoad.Delete()
                    Me.Remove(oShipmentLoad)
                End If
            End Sub

#End Region

        End Class

#End Region

#Region "ShipmentLoad"

        <CLSCompliant(False)> Public Class ShipmentLoad

#Region "Variables"

            Protected _shipment As String
            Protected _loadid As String
            Protected _adddate As DateTime
            Protected _adduser As String
            Protected _editdate As DateTime
            Protected _edituser As String

#End Region

#Region "Properties"

            Public ReadOnly Property WhereClause() As String
                Get
                    Return String.Format(" Shipment = '{0}' and Loadid = '{1}'", _shipment, _loadid)
                End Get
            End Property

            Public Property Shipment() As String
                Get
                    Return _shipment
                End Get
                Set(ByVal Value As String)
                    _shipment = Value
                End Set
            End Property

            Public Property LoadId() As String
                Get
                    Return _loadid
                End Get
                Set(ByVal Value As String)
                    _loadid = Value
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

#Region "Constructor"

            Public Sub New()

            End Sub

            Public Sub New(ByVal pShipment As String, ByVal pLoadId As String)
                _shipment = pShipment
                _loadid = pLoadId
                Load()
            End Sub

            Public Sub New(ByVal dr As DataRow)
                LoadDR(dr)
            End Sub

#End Region

#Region "Methods"

#Region "Accessibility"

            Public Shared Function Exists(ByVal pShipment As String, ByVal pLoadId As String) As Boolean
                Dim sql As String = String.Format("Select count(1) from SHIPMENTLOADS where Shipment = '{0}' and Loadid = '{1}'", pShipment, pLoadId)
                Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
            End Function

            Private Sub Load()
                Dim dt As New DataTable
                Dim sql As String = String.Format("Select * from SHIPMENTLOADS where {0}", WhereClause)
                DataInterface.FillDataset(sql, dt)
                If dt.Rows.Count = 0 Then
                    Throw New Made4Net.Shared.M4NException("Shipment Loads does not exists")
                End If
                Dim dr As DataRow = dt.Rows(0)
                LoadDR(dr)
            End Sub

            Private Sub LoadDR(ByVal dr As DataRow)
                _shipment = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SHIPMENT"))
                _loadid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LOADID"))
                _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
                _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
                _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
                _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
            End Sub

#End Region

#Region "Create / Delete"

            Public Sub Create(ByVal pShipment As String, ByVal pLoadid As String, ByVal pUser As String)
                If Not WMS.Logic.Shipment.Exists(pShipment) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot insert to Shipment Loads - shipment does not exists", "Cannot insert to Shipment Loads - shipment does not exists")
                End If
                If Not WMS.Logic.Load.Exists(pLoadid) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot insert to Shipment Loads - load does not exists", "Cannot insert to Shipment Loads - load does not exists")
                End If
                If Exists(pShipment, pLoadid) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Loads already exists", "Shipment Loads already exists")
                End If
                _shipment = pShipment
                _loadid = pLoadid
                _adddate = DateTime.Now
                _editdate = DateTime.Now
                _adduser = pUser
                _edituser = pUser

                Dim Sql As String = String.Format("INSERT INTO SHIPMENTLOADS (SHIPMENT, LOADID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5})", _
                    Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
                DataInterface.RunSQL(Sql)
            End Sub

            Public Sub Delete()
                Dim Sql As String = String.Format("DELETE FROM SHIPMENTLOADS where {0}", WhereClause)
                DataInterface.RunSQL(Sql)
            End Sub

#End Region

#End Region

        End Class

#End Region

#Region "ShipmentCompartments"

        <CLSCompliant(False)> Public Class ShipmentCompartment

#Region "Variables"

            Protected _shipment As String
            Protected _vehiclelocation As String
            Protected _handlingunit As String
            Protected _compartment As String
            Protected _adddate As DateTime
            Protected _adduser As String
            Protected _editdate As DateTime
            Protected _edituser As String

#End Region

#Region "Properties"

            Public ReadOnly Property WhereClause() As String
                Get
                    Return String.Format(" Shipment = '{0}' and vehiclelocation = '{1}'", _shipment, _vehiclelocation)
                End Get
            End Property

            Public Property Shipment() As String
                Get
                    Return _shipment
                End Get
                Set(ByVal Value As String)
                    _shipment = Value
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

            Public Property VehicleLocation() As String
                Get
                    Return _vehiclelocation
                End Get
                Set(ByVal Value As String)
                    _vehiclelocation = Value
                End Set
            End Property

            Public Property Compartment() As String
                Get
                    Return _compartment
                End Get
                Set(ByVal Value As String)
                    _compartment = Value
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

#Region "Constructor"

            Public Sub New()
            End Sub

#End Region

#Region "Methods"

#Region "Create"

            Public Sub Create(ByVal pShipment As String, ByVal pVehicleLocation As String, ByVal pHandlingUnit As String, ByVal pCompartment As String, ByVal pUser As String)
                If Not WMS.Logic.Shipment.Exists(pShipment) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot insert to Shipment compartments - shipment does not exists", "Cannot insert to Shipment compartments - shipment does not exists")
                End If
                _shipment = pShipment
                _vehiclelocation = pVehicleLocation
                _compartment = pCompartment
                _handlingunit = pHandlingUnit
                _adddate = DateTime.Now
                _editdate = DateTime.Now
                _adduser = pUser
                _edituser = pUser

                Dim Sql As String = String.Format("INSERT INTO SHIPMENTCOMPARTMENT (SHIPMENT, VEHICLELOCATION, HANDLINGUNIT, COMPARTMENT, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
                    Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_vehiclelocation), Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_compartment), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
                DataInterface.RunSQL(Sql)
            End Sub

#End Region

#Region "Update Compartment"

            Public Sub UpdateCompartment(ByVal pShipment As String, ByVal pUser As String)
                If Not WMS.Logic.Shipment.Exists(pShipment) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot insert to Shipment compartments - shipment does not exists", "Cannot insert to Shipment compartments - shipment does not exists")
                End If
                _compartment = Made4Net.Shared.Util.getNextCounter("COMPARTMENT")
                Dim Sql As String = String.Format("UPDATE SHIPMENTCOMPARTMENT SET COMPARTMENT ={0}, EDITDATE ={1}, EDITUSER ={2} where shipment ={3} and (COMPARTMENT = '' or COMPARTMENT is null)", _
                    Made4Net.Shared.Util.FormatField(_compartment), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_shipment))
                DataInterface.RunSQL(Sql)
            End Sub

#End Region

#End Region

        End Class

#End Region

#End Region

#Region "Variables"

#Region "Primary Keys"

        Protected _shipment As String = String.Empty

#End Region

#Region "Other Fields"

        Protected _status As String = String.Empty
        Protected _createdate As DateTime
        Protected _scheddate As DateTime
        Protected _shippeddate As DateTime
        Protected _door As String = String.Empty
        Protected _carrier As String = String.Empty
        Protected _vehicle As String = String.Empty
        Protected _trailer As String = String.Empty
        Protected _seal1 As String = String.Empty
        Protected _seal2 As String = String.Empty
        Protected _driver1 As String = String.Empty
        Protected _driver2 As String = String.Empty
        Protected _notes As String = String.Empty
        Protected _startloadingdate As DateTime
        Protected _transportreference As String = String.Empty
        Protected _transporttype As String = String.Empty
        Protected _bol As String = String.Empty
        Protected _yardentryid As String = String.Empty
        Protected _staginglane As String = String.Empty
        Protected _stagingwarehousearea As String = String.Empty
        Protected _adddate As DateTime
        Protected _adduser As String = String.Empty
        Protected _editdate As DateTime
        Protected _edituser As String = String.Empty
        Protected _StdTimeCalcParameters As Hashtable = New Hashtable()
        Protected _sourceStdTimeEquation As String = String.Empty

#End Region

#Region "Collections"

        Protected _orders As OutboundOrderCollection
        Protected _shipmentdetails As ShipmentDetailsCollection
        Protected _flowthroughs As FlowthroughCollection
        Protected _transshipments As TransShipmentCollection
        Protected _handlingunits As HandlingUnitTransactionCollection
        Protected _shipmentloads As ShipmentLoadsCollection

#End Region

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return " SHIPMENT = '" & _shipment & "'"
            End Get
        End Property

        Public ReadOnly Property CANCANCEL() As Boolean
            Get
                If _status = WMS.Lib.Statuses.Shipment.STATUSNEW Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public ReadOnly Property CanShip() As Boolean
            Get
                If _status = WMS.Lib.Statuses.Shipment.CANCELED OrElse _status = WMS.Lib.Statuses.Shipment.SHIPPING OrElse _
                _status = WMS.Lib.Statuses.Shipment.SHIPPED Then Return False
                'Dim ord As OutboundOrderHeader
                'For Each ord In Orders
                '    If Not ord.CanShip Then
                '        Return False
                '    End If
                'Next
                Return True
            End Get
        End Property

        Public ReadOnly Property IsLoadingCompleted() As Boolean
            Get
                Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits from (select sd.SHIPMENT,SUM(sd.units) as units from" & _
                " SHIPMENTDETAIL sd group by SHIPMENT) sd left outer join (select sl.SHIPMENT,sum(iv.UNITS) as unitsLoaded from SHIPMENTLOADS sl inner join INVLOAD" & _
                " iv on sl.LOADID = iv.LOADID group by sl.SHIPMENT ) sl on sl.SHIPMENT = sd.SHIPMENT where sd.{0}", Me.WhereClause)
                Dim remainingUnits As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                If remainingUnits > 0 Then
                    Return False
                Else
                    Return True
                End If
                'Dim sql As String = String.Format("SELECT STATUS FROM OUTBOUNDORHEADER WHERE SHIPMENT='{0}' GROUP BY STATUS union SELECT STATUS FROM FLOWTHROUGHHEADER WHERE SHIPMENT='{0}' GROUP BY STATUS union SELECT STATUS FROM TRANSSHIPMENT WHERE SHIPMENT='{0}' GROUP BY STATUS", _shipment)
                'Dim AllOrderStatusesDataTable As DataTable = New DataTable
                'DataInterface.FillDataset(sql, AllOrderStatusesDataTable)
                'Select Case AllOrderStatusesDataTable.Rows.Count
                '    Case 0
                '        ' No order's in this shipment
                '        Return False
                '    Case 1
                '        If AllOrderStatusesDataTable.Rows(0)("STATUS") = WMS.Lib.Statuses.OutboundOrderHeader.LOADED Then
                '            Return True
                '        Else
                '            Return False
                '        End If
                '    Case Else
                '        ' More than one status in the shipemnt so the shipment can not be loaded
                '        Return False
                'End Select
                'Return True
            End Get
        End Property

        Public Property SHIPMENT() As String
            Set(ByVal Value As String)
                _shipment = Value
            End Set
            Get
                Return _shipment
            End Get
        End Property

        Public ReadOnly Property HandlingUnits() As HandlingUnitTransactionCollection
            Get
                Return _handlingunits
            End Get
        End Property

        Public ReadOnly Property ShipmentLoads() As ShipmentLoadsCollection
            Get
                Return _shipmentloads
            End Get
        End Property

        Public Property STATUS() As String
            Get
                Return _status
            End Get
            Set(ByVal Value As String)
                _status = Value
            End Set
        End Property

        Public Property CREATEDATE() As DateTime
            Get
                Return _createdate
            End Get
            Set(ByVal Value As DateTime)
                _createdate = Value
            End Set
        End Property

        Public Property SCHEDDATE() As DateTime
            Get
                Return _scheddate
            End Get
            Set(ByVal Value As DateTime)
                _scheddate = Value
            End Set
        End Property

        Public Property SHIPPEDDATE() As DateTime
            Get
                Return _shippeddate
            End Get
            Set(ByVal Value As DateTime)
                _shippeddate = Value
            End Set
        End Property

        Public Property DOOR() As String
            Get
                Return _door
            End Get
            Set(ByVal Value As String)
                _door = Value
            End Set
        End Property

        Public Property CARRIER() As String
            Get
                Return _carrier
            End Get
            Set(ByVal Value As String)
                _carrier = Value
            End Set
        End Property

        Public Property VEHICLE() As String
            Get
                Return _vehicle
            End Get
            Set(ByVal Value As String)
                _vehicle = Value
            End Set
        End Property

        Public Property TRAILER() As String
            Get
                Return _trailer
            End Get
            Set(ByVal Value As String)
                _trailer = Value
            End Set
        End Property

        Public Property NOTES() As String
            Get
                Return _notes
            End Get
            Set(ByVal Value As String)
                _notes = Value
            End Set
        End Property

        Public Property SEAL1() As String
            Get
                Return _seal1
            End Get
            Set(ByVal Value As String)
                _seal1 = Value
            End Set
        End Property

        Public Property SEAL2() As String
            Get
                Return _seal2
            End Get
            Set(ByVal Value As String)
                _seal2 = Value
            End Set
        End Property

        Public Property DRIVER1() As String
            Get
                Return _driver1
            End Get
            Set(ByVal Value As String)
                _driver1 = Value
            End Set
        End Property

        Public Property DRIVER2() As String
            Get
                Return _driver2
            End Get
            Set(ByVal Value As String)
                _driver2 = Value
            End Set
        End Property

        Public Property STARTLOADINGTIME() As DateTime
            Get
                Return _startloadingdate
            End Get
            Set(ByVal Value As DateTime)
                _startloadingdate = Value
            End Set
        End Property

        Public Property TRANSPORTREFERENCE() As String
            Get
                Return _transportreference
            End Get
            Set(ByVal Value As String)
                _transportreference = Value
            End Set
        End Property

        Public Property TRANSPORTTYPE() As String
            Get
                Return _transporttype
            End Get
            Set(ByVal Value As String)
                _transporttype = Value
            End Set
        End Property

        Public Property BOL() As String
            Get
                Return _bol
            End Get
            Set(ByVal Value As String)
                _bol = Value
            End Set
        End Property

        Public Property YARDENTRYID() As String
            Get
                Return _yardentryid
            End Get
            Set(ByVal Value As String)
                _yardentryid = Value
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

        Public ReadOnly Property Orders() As OutboundOrderCollection
            Get
                Return _orders
            End Get
        End Property

        Public ReadOnly Property ShipmentDetails() As ShipmentDetailsCollection
            Get
                Return _shipmentdetails
            End Get
        End Property

        Public ReadOnly Property Flowthroughs() As FlowthroughCollection
            Get
                Return _flowthroughs
            End Get
        End Property

        Public ReadOnly Property Transshipments() As TransShipmentCollection
            Get
                Return _transshipments
            End Get
        End Property

        Public Property STAGINGLANE() As String
            Get
                Return _staginglane
            End Get
            Set(ByVal Value As String)
                _staginglane = Value
            End Set
        End Property

        Public Property STAGINGWAREHOUSEAREA() As String
            Get
                Return _stagingwarehousearea
            End Get
            Set(ByVal Value As String)
                _stagingwarehousearea = Value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal pSHIPMENT As String, Optional ByVal LoadObj As Boolean = True)
            _shipment = pSHIPMENT
            If LoadObj Then
                Load()
            End If
        End Sub

        Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
            Dim ds As New DataSet
            Dim dr As DataRow
            Dim dor As String = Nothing
            Dim carr As String = Nothing
            Dim vehicle As String = Nothing
            Dim trail As String = Nothing
            Dim drv1 As String = Nothing
            Dim drv2 As String = Nothing
            Dim seal1 As String = Nothing
            Dim seal2 As String = Nothing
            Dim notes As String = Nothing
            Dim bol As String = Nothing
            Dim transporttype As String = Nothing
            Dim transportreference As String = Nothing
            Dim stagingLane As String = Nothing
            Dim stagingWarehouseArea As String = Nothing
            ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
            If CommandName = "Save" Then
                Dim scdate As DateTime
                dr = ds.Tables(0).Rows(0)
                Dim startLoading As DateTime
                If Not IsDBNull(dr("SCHEDDATE")) Then scdate = dr("SCHEDDATE")
                If Not IsDBNull(dr("DOOR")) Then dor = dr("DOOR")
                If Not IsDBNull(dr("CARRIER")) Then carr = dr("CARRIER")
                If Not IsDBNull(dr("VEHICLE")) Then vehicle = dr("VEHICLE")
                If Not IsDBNull(dr("TRAILER")) Then trail = dr("TRAILER")
                If Not IsDBNull(dr("STARTLOADINGDATE")) Then startLoading = dr("STARTLOADINGDATE")
                If Not IsDBNull(dr("DRIVER1")) Then drv1 = dr("DRIVER1")
                If Not IsDBNull(dr("DRIVER2")) Then drv2 = dr("DRIVER2")
                If Not IsDBNull(dr("SEAL1")) Then seal1 = dr("SEAL1")
                If Not IsDBNull(dr("SEAL2")) Then seal2 = dr("SEAL2")
                If Not IsDBNull(dr("NOTES")) Then notes = dr("NOTES")
                If Not IsDBNull(dr("TRANSPORTREFERENCE")) Then transportreference = dr("TRANSPORTREFERENCE")
                If Not IsDBNull(dr("TRANSPORTTYPE")) Then transporttype = dr("TRANSPORTTYPE")
                If Not IsDBNull(dr("BOL")) Then bol = dr("BOL")
                If Not IsDBNull(dr("STAGINGLANE")) Then stagingLane = dr("STAGINGLANE")
                If Not IsDBNull(dr("STAGINGWAREHOUSEAREA")) Then stagingWarehouseArea = dr("STAGINGWAREHOUSEAREA")
                Create("", scdate, dor, carr, vehicle, trail, startLoading, drv1, drv2, seal1, seal2, notes, transportreference, transporttype, bol, stagingLane, stagingWarehouseArea, Common.GetCurrentUser)
                Dim col As New Made4Net.DataAccess.Collections.GenericCollection
                Dim t As New Made4Net.Shared.Translation.Translator()
                col.Add("SHIPMENT", _shipment)
                Message = t.Translate("Shipment Created #SHIPMENT#", col)
            ElseIf CommandName = "Update" Then
                Dim scdate As DateTime
                dr = ds.Tables(0).Rows(0)
                _shipment = dr("SHIPMENT")
                Load()
                Dim startLoading As DateTime
                If Not IsDBNull(dr("SCHEDDATE")) Then scdate = dr("SCHEDDATE")
                If Not IsDBNull(dr("DOOR")) Then dor = dr("DOOR")
                If Not IsDBNull(dr("CARRIER")) Then carr = dr("CARRIER")
                If Not IsDBNull(dr("VEHICLE")) Then vehicle = dr("VEHICLE")
                If Not IsDBNull(dr("TRAILER")) Then trail = dr("TRAILER")
                If Not IsDBNull(dr("STARTLOADINGDATE")) Then startLoading = dr("STARTLOADINGDATE")
                If Not IsDBNull(dr("DRIVER1")) Then drv1 = dr("DRIVER1")
                If Not IsDBNull(dr("DRIVER2")) Then drv2 = dr("DRIVER2")
                If Not IsDBNull(dr("SEAL1")) Then seal1 = dr("SEAL1")
                If Not IsDBNull(dr("SEAL2")) Then seal2 = dr("SEAL2")
                If Not IsDBNull(dr("NOTES")) Then notes = dr("NOTES")
                If Not IsDBNull(dr("TRANSPORTREFERENCE")) Then _transportreference = dr("TRANSPORTREFERENCE")
                If Not IsDBNull(dr("TRANSPORTTYPE")) Then _transporttype = dr("TRANSPORTTYPE")
                If Not IsDBNull(dr("BOL")) Then BOL = dr("BOL")
                If Not IsDBNull(dr("STAGINGLANE")) Then stagingLane = dr("STAGINGLANE")
                If Not IsDBNull(dr("STAGINGWAREHOUSEAREA")) Then stagingWarehouseArea = dr("STAGINGWAREHOUSEAREA")
                Update(scdate, dor, carr, vehicle, trail, startLoading, drv1, drv2, seal1, seal2, notes, TRANSPORTREFERENCE, TRANSPORTTYPE, BOL, stagingLane, stagingWarehouseArea, Common.GetCurrentUser)
            ElseIf CommandName.ToLower = "cancelship" Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "No Row Selected", "No Row Selected")
                    Throw m4nEx
                End If
                Try
                    For Each datar As DataRow In ds.Tables(0).Rows
                        _shipment = datar("SHIPMENT")
                        Load()
                        Cancel(Common.GetCurrentUser())
                    Next
                Catch ex As Made4Net.Shared.M4NException
                    Message = ex.Message & "Shipment ID: " & _shipment
                Catch ex As Exception
                End Try
            ElseIf CommandName = "DeAssignOrders" Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "No Row Selected", "No Row Selected")
                    Throw m4nEx
                End If
                _shipment = ds.Tables(0).Rows(0)("SHIPMENT")
                Load()
                For Each dr In ds.Tables(0).Rows
                    Dim docType As String
                    Try
                        docType = dr("DOCTYPE")
                    Catch ex As Exception
                        docType = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    End Try
                    'DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser(), docType)
                    DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Common.GetCurrentUser())
                Next
            ElseIf CommandName = "AssignOrders" Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "No Row Selected", "No Row Selected")
                    Throw m4nEx
                End If
                _shipment = ds.Tables(0).Rows(0)("SHIPID")
                Load()
                Dim docType As String

                For Each dr In ds.Tables(0).Rows
                    Try
                        docType = dr("DOCTYPE")
                    Catch ex As Exception
                        docType = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    End Try
                    'AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser(), docType)
                    'AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UNITS")), 0, Common.GetCurrentUser(), docType)
                    AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYOPEN")), 0, Common.GetCurrentUser(), docType)
                Next
            ElseIf CommandName = "CompleteShip" Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "No Row Selected", "No Row Selected")
                    Throw m4nEx
                End If
                For Each dr In ds.Tables(0).Rows
                    _shipment = ds.Tables(0).Rows(0)("SHIPID") '("SHIPMENT")
                    Load()
                    CompleteShip(Common.GetCurrentUser())
                Next
            ElseIf CommandName = "CancelShip" Then
                If ds.Tables(0).Rows.Count = 0 Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "No Row Selected", "No Row Selected")
                    Throw m4nEx
                End If
                For Each dr In ds.Tables(0).Rows
                    If CANCANCEL Then
                        _shipment = ds.Tables(0).Rows(0)("SHIPID")
                        _status = WMS.Lib.Statuses.Shipment.CANCELED
                        Load()
                        Save(Common.GetCurrentUser())
                    Else
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cant cancel Shipment. Shipment status incorrect.", "Cant cancel Shipment. Shipment status incorrect.")
                        Throw m4nEx
                    End If
                Next

            ElseIf CommandName = "Ship" Then
                For Each dr In ds.Tables(0).Rows
                    _shipment = dr("SHIPMENT")
                    Load()
                    If (CanShip) Then
                        Ship(Common.GetCurrentUser())
                    Else
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Can not ship shipment. Incorrect status.", "Can not ship shipment. Incorrect status.")
                    End If
                Next
            ElseIf CommandName = "PrintShipment" Then
                For Each dr In ds.Tables(0).Rows
                    _shipment = dr("SHIPMENT")
                    Load()
                    PrintShippingManifest(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next
            ElseIf CommandName = "PrintShipmentPackingList" Then
                For Each dr In ds.Tables(0).Rows
                    _shipment = dr("SHIPMENT")
                    Load()
                    PrintShipmentPackingList(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next
            ElseIf CommandName = "PrintLoadingWorksheet" Then
                For Each dr In ds.Tables(0).Rows
                    _shipment = dr("SHIPMENT")
                    Load()
                    PrintLoadingWorksheet(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next

            End If
        End Sub


#End Region

#Region "Methods"

#Region "Accessors"

        Public Shared Function GetSHIPMENT(ByVal pSHIPMENT As String) As Shipment
            Return New Shipment(pSHIPMENT)
        End Function

        Public Shared Function Exists(ByVal pShipmentId As String) As Boolean
            Dim sql As String = String.Format("Select count(1) from SHIPMENT where SHIPMENT = '{0}'", pShipmentId)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Protected Sub Load()

            Dim SQL As String = "SELECT * FROM SHIPMENT WHERE " & WhereClause
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Return
            End If
            dr = dt.Rows(0)

            If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
            If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
            If Not dr.IsNull("SCHEDDATE") Then _scheddate = dr.Item("SCHEDDATE")
            If Not dr.IsNull("SHIPPEDDATE") Then _shippeddate = dr.Item("SHIPPEDDATE")
            If Not dr.IsNull("DOOR") Then _door = dr.Item("DOOR")
            If Not dr.IsNull("CARRIER") Then _carrier = dr.Item("CARRIER")
            If Not dr.IsNull("VEHICLE") Then _vehicle = dr.Item("VEHICLE")
            If Not dr.IsNull("TRAILER") Then _trailer = dr.Item("TRAILER")
            If Not dr.IsNull("DRIVER1") Then _driver1 = dr.Item("DRIVER1")
            If Not dr.IsNull("DRIVER2") Then _driver2 = dr.Item("DRIVER2")
            If Not dr.IsNull("SEAL1") Then _seal1 = dr.Item("SEAL1")
            If Not dr.IsNull("SEAL2") Then _seal2 = dr.Item("SEAL2")
            If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
            If Not dr.IsNull("STARTLOADINGDATE") Then _startloadingdate = dr.Item("STARTLOADINGDATE")
            If Not IsDBNull(dr("TRANSPORTREFERENCE")) Then _transportreference = dr("TRANSPORTREFERENCE")
            If Not IsDBNull(dr("TRANSPORTTYPE")) Then _transporttype = dr("TRANSPORTTYPE")
            If Not IsDBNull(dr("BOL")) Then _bol = dr("BOL")
            If Not IsDBNull(dr("YARDENTRYID")) Then _yardentryid = dr("YARDENTRYID")
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")

            If Not dr.IsNull("STAGINGLANE") Then _staginglane = dr.Item("STAGINGLANE")
            If Not dr.IsNull("STAGINGWAREHOUSEAREA") Then _stagingwarehousearea = dr.Item("STAGINGWAREHOUSEAREA")

            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

            _orders = New OutboundOrderCollection(_shipment)
            _shipmentdetails = New ShipmentDetailsCollection(_shipment)
            _flowthroughs = New FlowthroughCollection(_shipment)
            _transshipments = New TransShipmentCollection(_shipment)
            _handlingunits = New HandlingUnitTransactionCollection(_shipment, HandlingUnitTransactionTypes.Shipment)
            _shipmentloads = New ShipmentLoadsCollection(_shipment, True)
        End Sub

        Public Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
            Dim oldStatus As String = _status
            _status = pStatus
            _edituser = pUser
            _editdate = DateTime.Now
            Dim sql As String = String.Format("Update SHIPMENT SET STATUS = {0},EditUser = {1},EditDate = {2} Where {3}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentStatusChanged)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPSTTCHNG)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _shipment)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", oldStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", pStatus)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.SHIPSTTCHNG)
        End Sub

#End Region

#Region "Create"

        Public Sub Create(ByVal pShipmentId As String, ByVal pSchedDate As DateTime, ByVal pDoor As String, ByVal pCarrier As String, ByVal pVEHICLE As String, ByVal pTrailer As String, _
                ByVal pStartLoadingDate As DateTime, ByVal pDriver1 As String, ByVal pDriver2 As String, ByVal pSeal1 As String, ByVal pSeal2 As String, ByVal pNotes As String, ByVal pTransportReference As String, ByVal pTransportType As String, ByVal pBol As String, ByVal pStagingLane As String, ByVal pStagingWarehouseArea As String, ByVal pUser As String)

            'If (Not pCarrier Is Nothing) Then
            '    If pCarrier.Trim <> "" Then
            '        If Not WMS.Logic.Carrier.Exists(pCarrier) Then
            '            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
            '            Throw m4nEx
            '        End If
            '    End If
            'End If

            If WMS.Logic.Shipment.Exists(pShipmentId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Shipment,Shipment Already Exists", "Cannot Create Shipment,Shipment Already Exists")
            End If

            'If Not String.IsNullOrEmpty(pStagingLane) AndAlso Not WMS.Logic.Location.Exists(pStagingLane, pStagingWarehouseArea) Then
            '    Throw New Made4Net.Shared.M4NException(New Exception(), "Could not create shipment. Staging Lane does not exist", "Could not create shipment. Staging Lane does not exist")
            'End If

            _carrier = pCarrier
            _createdate = DateTime.Now
            _door = pDoor
            _scheddate = pSchedDate
            _status = WMS.Lib.Statuses.Shipment.STATUSNEW
            _vehicle = pVEHICLE
            _trailer = pTrailer
            _startloadingdate = pStartLoadingDate
            _driver1 = pDriver1
            _driver2 = pDriver2
            _notes = pNotes
            _seal1 = pSeal1
            _seal2 = pSeal2
            _transportreference = pTransportReference
            _transporttype = pTransportType
            _bol = pBol

            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser

            _staginglane = pStagingLane
            _stagingwarehousearea = pStagingWarehouseArea

            If pShipmentId <> "" Then
                _shipment = pShipmentId
            Else
                _shipment = Made4Net.Shared.Util.getNextCounter("SHIPMENT")
            End If

            Dim sql As String = String.Format("INSERT INTO SHIPMENT(SHIPMENT,STATUS,CREATEDATE,SCHEDDATE,SHIPPEDDATE,DOOR," & _
                "CARRIER,VEHICLE,ADDDATE,ADDUSER,EDITDATE,EDITUSER,STARTLOADINGDATE, DRIVER1, DRIVER2, SEAL1, SEAL2, NOTES, TRAILER, TRANSPORTREFERENCE,TRANSPORTTYPE,BOL,STAGINGLANE,STAGINGWAREHOUSEAREA) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23})", _
                Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_scheddate), _
                Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_door), Made4Net.Shared.Util.FormatField(_carrier), Made4Net.Shared.Util.FormatField(_vehicle), _
                Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                Made4Net.Shared.Util.FormatField(_startloadingdate), Made4Net.Shared.Util.FormatField(_driver1), Made4Net.Shared.Util.FormatField(_driver2), Made4Net.Shared.Util.FormatField(_seal1), _
                Made4Net.Shared.Util.FormatField(_seal2), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_trailer), _
                Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_bol), _
                Made4Net.Shared.FormatField(_staginglane), Made4Net.Shared.FormatField(_stagingwarehousearea))

            DataInterface.RunSQL(sql)

            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ShipmentCreated
            'em.Add("EVENT", EventType)
            'em.Add("SHIPMENT", _shipment)
            'em.Add("USERID", _adduser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentCreated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPMENTINS)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _shipment)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", "")
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(WMS.Lib.Actions.Audit.SHIPMENTINS)

            _shipmentdetails = New ShipmentDetailsCollection(_shipment)
        End Sub

        Public Sub Update(ByVal pSchedDate As DateTime, ByVal pDoor As String, ByVal pCarrier As String, ByVal pVEHICLE As String, ByVal pTrailer As String, _
                ByVal pStartLoadingDate As DateTime, ByVal pDriver1 As String, ByVal pDriver2 As String, ByVal pSeal1 As String, ByVal pSeal2 As String, ByVal pNotes As String, ByVal pTransportReference As String, ByVal pTransportType As String, ByVal pBol As String, ByVal pStagingLane As String, ByVal pStagingWarehouseArea As String, ByVal pUser As String)

            If (Not pCarrier Is Nothing) Then
                If pCarrier.Trim <> "" Then
                    If Not WMS.Logic.Carrier.Exists(pCarrier) Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
                        Throw m4nEx
                    End If
                End If
            End If

            'If Not WMS.Logic.Location.Exists(pStagingLane, pStagingWarehouseArea) Then
            '    Throw New Made4Net.Shared.M4NException(New Exception(), "Could not update shipment. Staging Lane does not exist", "Could not update shipment. Staging Lane does not exist")
            'End If

            _scheddate = pSchedDate
            _door = pDoor
            _carrier = pCarrier
            _vehicle = pVEHICLE
            _trailer = pTrailer
            _startloadingdate = pStartLoadingDate
            _driver1 = pDriver1
            _driver2 = pDriver2
            _notes = pNotes
            _seal1 = pSeal1
            _seal2 = pSeal2
            _transportreference = pTransportReference
            _transporttype = pTransportType
            _bol = pBol

            _staginglane = pStagingLane
            _stagingwarehousearea = pStagingWarehouseArea
            _editdate = DateTime.Now
            _edituser = pUser

            Dim sql As String = String.Format("UPDATE SHIPMENT SET SCHEDDATE={0},DOOR={1},CARRIER={2},VEHICLE={3}, " & _
                "EDITDATE={4},EDITUSER={5}, STARTLOADINGDATE ={6}, DRIVER1 ={7}, DRIVER2 ={8}, SEAL1 ={9}, SEAL2 ={10}, NOTES ={11}, TRAILER ={12}, TRANSPORTREFERENCE={13},TRANSPORTTYPE={14},BOL={15},STAGINGLANE={16},STAGINGWAREHOUSEAREA={17} WHERE {18}", Made4Net.Shared.Util.FormatField(_scheddate), Made4Net.Shared.Util.FormatField(_door), Made4Net.Shared.Util.FormatField(_carrier), _
                Made4Net.Shared.Util.FormatField(_vehicle), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                Made4Net.Shared.Util.FormatField(_startloadingdate), Made4Net.Shared.Util.FormatField(_driver1), Made4Net.Shared.Util.FormatField(_driver2), Made4Net.Shared.Util.FormatField(_seal1), _
                Made4Net.Shared.Util.FormatField(_seal2), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_trailer), Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_bol), _
                Made4Net.Shared.FormatField(_staginglane), Made4Net.Shared.FormatField(_stagingwarehousearea), WhereClause)

            DataInterface.RunSQL(sql)

            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ShipmentUpdated
            'em.Add("EVENT", EventType)
            'em.Add("SHIPMENT", _shipment)
            'em.Add("USERID", _adduser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentUpdated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPMENTUPD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _shipment)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.SHIPMENTUPD)
        End Sub

        Public Sub Save(ByVal pUser As String)
            Dim sql As String
            Dim aq As EventManagerQ = New EventManagerQ
            Dim activitytype As String
            If Not Exists(_shipment) Then
                ' Insert new one
                If (Not _carrier Is Nothing) Then
                    If _carrier.Trim <> "" Then
                        If Not WMS.Logic.Carrier.Exists(_carrier) Then
                            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
                            Throw m4nEx
                        End If
                    End If
                End If

                _createdate = DateTime.Now
                _status = WMS.Lib.Statuses.Shipment.STATUSNEW
                _adddate = DateTime.Now
                _adduser = pUser
                _editdate = DateTime.Now
                _edituser = pUser

                If _shipment = "" Then
                    _shipment = Made4Net.Shared.Util.getNextCounter("SHIPMENT")
                End If

                sql = String.Format("INSERT INTO SHIPMENT(SHIPMENT,STATUS,CREATEDATE,SCHEDDATE,SHIPPEDDATE,DOOR," & _
                   "CARRIER,VEHICLE,ADDDATE,ADDUSER,EDITDATE,EDITUSER,STARTLOADINGDATE, DRIVER1, DRIVER2, SEAL1, SEAL2, NOTES, TRAILER, TRANSPORTREFERENCE,TRANSPORTTYPE,BOL,STAGINGLANE,STAGINGWAREHOUSEAREA) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23})", _
                   Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_scheddate), _
                   Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_door), Made4Net.Shared.Util.FormatField(_carrier), Made4Net.Shared.Util.FormatField(_vehicle), _
                   Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                   Made4Net.Shared.Util.FormatField(_startloadingdate), Made4Net.Shared.Util.FormatField(_driver1), Made4Net.Shared.Util.FormatField(_driver2), Made4Net.Shared.Util.FormatField(_seal1), _
                   Made4Net.Shared.Util.FormatField(_seal2), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_trailer), _
                   Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_bol), _
                   Made4Net.Shared.FormatField(_staginglane), Made4Net.Shared.FormatField(_stagingwarehousearea))

                DataInterface.RunSQL(sql)
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentCreated)
                activitytype = WMS.Lib.Actions.Audit.SHIPMENTINS

                _shipmentdetails = New ShipmentDetailsCollection(_shipment)
            Else
                ' Update old one
                If (Not _carrier Is Nothing) Then
                    If _carrier.Trim <> "" Then
                        If Not WMS.Logic.Carrier.Exists(_carrier) Then
                            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Carrier does not exists", "Carrier does not exists")
                            Throw m4nEx
                        End If
                    End If
                End If

                _editdate = DateTime.Now
                _edituser = pUser

                sql = String.Format("UPDATE SHIPMENT SET SCHEDDATE={0},DOOR={1},CARRIER={2},VEHICLE={3}, " & _
                   "EDITDATE={4},EDITUSER={5}, STARTLOADINGDATE ={6}, DRIVER1 ={7}, DRIVER2 ={8}, SEAL1 ={9}, SEAL2 ={10}, NOTES ={11}, TRAILER ={12}, TRANSPORTREFERENCE={13},TRANSPORTTYPE={14},BOL={15},STAGINGLANE={16},STAGINGWAREHOUSEAREA={17} WHERE {18}", Made4Net.Shared.Util.FormatField(_scheddate), Made4Net.Shared.Util.FormatField(_door), Made4Net.Shared.Util.FormatField(_carrier), _
                   Made4Net.Shared.Util.FormatField(_vehicle), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                   Made4Net.Shared.Util.FormatField(_startloadingdate), Made4Net.Shared.Util.FormatField(_driver1), Made4Net.Shared.Util.FormatField(_driver2), Made4Net.Shared.Util.FormatField(_seal1), _
                   Made4Net.Shared.Util.FormatField(_seal2), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_trailer), Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_bol), _
                   Made4Net.Shared.FormatField(_staginglane), Made4Net.Shared.FormatField(_stagingwarehousearea), WhereClause)

                DataInterface.RunSQL(sql)
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentUpdated)
                activitytype = WMS.Lib.Actions.Audit.SHIPMENTUPD
            End If

            aq.Add("ACTIVITYTYPE", activitytype)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _shipment)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", "")
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(activitytype)

            Load()
            '_editdate = DateTime.Now
            '_edituser = pUser
            'Dim sql As String = String.Format("Update Shipment set EDITDATE={1},EDITUSER={2}, STATUS ={3} WHERE {4}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _status, WhereClause)
            'DataInterface.RunSQL(sql)
        End Sub

#End Region

#Region "Orders Assignment"

        Public Sub AssignOrder(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pUnits As Decimal, ByVal pLoadingSequence As Int32, ByVal pUser As String, Optional ByVal pDocumentType As String = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER)
            If _status = WMS.Lib.Statuses.Shipment.SHIPPED Or _status = WMS.Lib.Statuses.Shipment.CANCELED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "INCORRECT SHIPMENT STATUS", "INCORRECT SHIPMENT STATUS")
            End If
            If pUnits <= 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Quantity must be greater than 0.", "Quantity must be greater than 0.")
            End If
            Select Case pDocumentType
                Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    Dim oShipDetail As ShipmentDetail
                    oShipDetail = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine)
                    If Not oShipDetail Is Nothing Then
                        oShipDetail.AddQauntity(pUnits, pUser)
                    Else
                        ''check if detail is assigned to another shipment
                        'If pAssignedShipment <> String.Empty Then
                        '    oShipDetail = New ShipmentDetail(pAssignedShipment, Consignee, OrdId, pOrderLine)
                        '    oShipDetail.MoveToShipment(_shipment, pUser)
                        '    oShipDetail.AddQauntity(pUnits, pUser)
                        'Else
                        oShipDetail = New ShipmentDetail
                        oShipDetail.Create(_shipment, Consignee, OrdId, pOrderLine, pUnits, pLoadingSequence, pUser)
                        _shipmentdetails.add(oShipDetail)
                        'End If
                    End If
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim fl As New Flowthrough(Consignee, OrdId)
                    fl.AssignToShipment(_shipment, pUser)
                    Flowthroughs.add(fl)
                Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
                    Dim t1 As New TransShipment(Consignee, OrdId)
                    t1.AssignToShipment(_shipment, pUser)
                    Transshipments.add(t1)
            End Select
            ' CalculateStdtime()
            _editdate = DateTime.Now
            _edituser = pUser
            'If Me.STATUS = WMS.Lib.Statuses.Shipment.STATUSNEW Then
            '    _status = WMS.Lib.Statuses.Wave.ASSIGNED
            '    DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            'Else
            '    DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            'End If
        End Sub

        Public Sub UpdateShipmentStatus()
            If Me.STATUS = WMS.Lib.Statuses.Shipment.STATUSNEW Then
                _status = WMS.Lib.Statuses.Wave.ASSIGNED
                DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            Else
                DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            End If
        End Sub

        Public Sub DeAssignOrder(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pUser As String, Optional ByVal pDocumentType As String = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER)
            If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "INCORRECT SHIPMENT STATUS", "INCORRECT SHIPMENT STATUS")
            End If
            Select Case pDocumentType
                Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    Dim sd As ShipmentDetail
                    sd = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine)
                    sd.Delete()
                    Me.ShipmentDetails.Remove(sd)
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim fl As Flowthrough
                    fl = Me.Flowthroughs.getOrder(Consignee, OrdId)
                    Me.Flowthroughs.Remove(fl)
                    fl.DeAssignFromShipment(pUser)
                Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
                    Dim ts As TransShipment
                    ts = Me.Transshipments.getOrder(Consignee, OrdId)
                    Me.Transshipments.Remove(ts)
                    ts.DeAssignFromShipment(pUser)
            End Select

            _editdate = DateTime.Now
            _edituser = pUser
            If ShipmentDetails.Count = 0 Then
                _status = WMS.Lib.Statuses.Shipment.STATUSNEW
                DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            Else
                DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            End If
        End Sub

        Public Sub MoveDetailToShipment(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pNewShipmentId As String, ByVal pUser As String)
            If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "INCORRECT SHIPMENT STATUS", "INCORRECT SHIPMENT STATUS")
            End If
            If String.IsNullOrEmpty(pNewShipmentId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Target Shipment field cannot be blank", "Target Shipment field cannot be blank")
            End If
            If Not Exists(pNewShipmentId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "New shipment does not exists - Cannot Move detail to new shipment", "New shipment does not exists - Cannot Move detail to new shipment")
            End If
            Dim sd As ShipmentDetail
            sd = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine)
            sd.MoveToShipment(pNewShipmentId, pUser)
            Me.ShipmentDetails.Remove(sd)

            _editdate = DateTime.Now
            _edituser = pUser
            If ShipmentDetails.Count = 0 Then
                _status = WMS.Lib.Statuses.Shipment.STATUSNEW
                DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            Else
                DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            End If
        End Sub

#End Region

#Region "Standart Loading Time"

        Public Function CalculateStdtime() As Double
            LoadCalcParameters()
            getEquation()
            Dim StdTime As Double = CalcStdTimeEquation(_sourceStdTimeEquation)
            updateStdTime(StdTime)
        End Function

        Private Sub updateStdTime(ByVal pStdTime As Double)
            DataInterface.RunSQL(String.Format("update SHIPMENT set ESTLOADINGTIME='{0}' where shipment='{1}'", Math.Round(pStdTime, 2).ToString(), _shipment))
        End Sub

        Private Function LoadCalcParameters() As Integer
            Dim sql As String = String.Format("SELECT * FROM vCalculateShipmentStdtime  where shipment='{0}' ", _shipment)
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            Dim dr As DataRow
            If dt.Rows.Count = 0 Then Return 0
            dr = dt.Rows(0)
            For Each column As DataColumn In dt.Columns
                addStdTimeCalcParameters(column.ColumnName, dr(column.ColumnName).ToString())
            Next
        End Function

        Private Function CalcStdTimeEquation(ByVal sourceEquation As String) As Double
            Dim targetEquation As String = Nothing
            Dim res As Double
            Try
                Dim oEvalEquation As EvalEquation = New EvalEquation(Nothing, _StdTimeCalcParameters)
                res = oEvalEquation.EvalEquation(sourceEquation, targetEquation)
                Return res
            Catch ex As Exception
                Return 0D
            End Try
        End Function

        Private Sub getEquation()
            _sourceStdTimeEquation = DataInterface.ExecuteScalar(String.Format("select isnull(SERVICETIMEEQUATION,'') SERVICETIMEEQUATION from DOCUMENTTIMECALCULATIONMETHODS where DOCTYPE='{0}'", _
                    WMS.Lib.DOCUMENTTYPES.SHIPMENT))
        End Sub

        Private Sub addStdTimeCalcParameters(ByVal key As String, ByVal value As String)
            If Not _StdTimeCalcParameters.Contains(key) Then
                _StdTimeCalcParameters.Add(key, value)
            Else
                _StdTimeCalcParameters(key) = value
            End If
        End Sub

#End Region

#Region "Complete Ship"

        Public Sub CompleteShip(ByVal pUser As String)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentComplete)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.COMPLETESHIP)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _shipment)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", "")
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(WMS.Lib.Actions.Audit.COMPLETESHIP)
        End Sub

#End Region

#Region "Cancel"

        'Added by priel
        Public Sub Cancel(ByVal pUser As String)
            Dim rq As New EventManagerQ
            Dim action As String = WMS.Lib.Actions.Audit.CANSHIP

            Select Case _status
                Case WMS.Lib.Statuses.Shipment.CANCELED
                    Throw New Made4Net.Shared.M4NException(New Exception(), "Shipment already canceled.", "Shipment already canceled.")
                Case WMS.Lib.Statuses.Shipment.STATUSNEW
                    SetStatus(WMS.Lib.Statuses.Shipment.CANCELED, pUser)

                    rq.Add("ACTION", action)
                    rq.Add("USERID", pUser)
                    rq.Add("DATE", DateTime.Now.ToString())
                    rq.Add("WAVE", _shipment)
                    rq.Send(WMS.Lib.Actions.Audit.CANSHIP)

                Case Else ' WMS.Lib.Statuses.Shipment.ASSIGNED

                    For Each shipdet As ShipmentDetail In ShipmentDetails
                        shipdet.Delete()
                    Next

                    For Each trns As WMS.Logic.TransShipment In Transshipments
                        If trns.STATUS <> WMS.Lib.Statuses.TransShipment.CANCELED Then
                            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not cancel shipment with transshipments which are not canceled.", "Can not cancel shipment with transshipments which are not canceled.")
                        End If
                    Next

                    For Each flt As WMS.Logic.Flowthrough In Flowthroughs
                        If flt.STATUS <> WMS.Lib.Statuses.Flowthrough.CANCELED Then
                            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not cancel shipment with flowthroughs which are not canceled.", "Can not cancel shipment with transshipments which are not canceled.")
                        End If
                    Next

                    SetStatus(WMS.Lib.Statuses.Shipment.CANCELED, pUser)

                    rq.Add("ACTION", action)
                    rq.Add("USERID", pUser)
                    rq.Add("DATE", DateTime.Now.ToString())
                    rq.Add("WAVE", _shipment)
                    rq.Send(WMS.Lib.Actions.Audit.CANSHIP)

                    'Case Else
                    '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Shimpent status is incorrect for canceling", "Shipment status is incorrect for canceling")
                    '    Throw m4nEx
            End Select
        End Sub

#End Region

#Region "Loading"

        'This Function checks for the Shipment Status - if all orders assigned are Loaded then status should be Loaded
        Public Sub LoadPallet(ByVal pUser As String)
            If _status = WMS.Lib.Statuses.Shipment.ASSIGNED Or _status = WMS.Lib.Statuses.Shipment.ATDOCK Then
                If IsLoadingCompleted Then
                    SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)
                Else
                    UpdateStartLoadingDate(pUser)
                    SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
                End If
            ElseIf _status = WMS.Lib.Statuses.Shipment.LOADING Then
                If IsLoadingCompleted Then
                    SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)
                End If
            End If
        End Sub

        Private Sub UpdateStartLoadingDate(ByVal pUser As String)
            _startloadingdate = DateTime.Now
            _edituser = pUser
            _editdate = DateTime.Now

            Dim sql As String = String.Format("Update SHIPMENT SET STARTLOADINGDATE = {0},EditUser = {1},EditDate = {2} Where {3}", _
                Made4Net.Shared.Util.FormatField(_startloadingdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub UnLoad(ByVal pLoadID As String, ByVal pLocation As String, ByVal pUser As String)
            Dim loadObj As New WMS.Logic.Load(pLoadID)
            Me.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)

            'loadObj.UnLoad(pLocation, pUser)

            Me.ShipmentLoads.ShipmentLoad(pLoadID).Delete()

        End Sub

#End Region

#Region "Yard"

        Public Sub AssignToYardEntry(ByVal pYardEntryId As String, ByVal pUserId As String)
            'If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Status Incorrect", "Shipment Status Incorrect")
            'End If
            'If Not YardEntry.Exists(pYardEntryId) Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Yard Entry does not exists", "Yard Entry does not exists")
            'End If
            'Dim SQL As String
            '_edituser = pUserId
            '_editdate = DateTime.Now
            '_yardentryid = pYardEntryId
            'SQL = String.Format("UPDATE SHIPMENT SET YARDENTRYID ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_yardentryid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            'DataInterface.RunSQL(SQL)
        End Sub

        Public Sub UnAssignFromYardEntry(ByVal pUserId As String)
            If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Status Incorrect", "Shipment Status Incorrect")
            End If
            Dim SQL As String
            _edituser = pUserId
            _editdate = DateTime.Now
            _yardentryid = ""
            SQL = String.Format("UPDATE SHIPMENT SET YARDENTRYID ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_yardentryid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(SQL)
        End Sub

        Public Sub Schedule(ByVal pUserId As String)
            If _status = WMS.Lib.Statuses.Shipment.STATUSNEW Or _status = WMS.Lib.Statuses.Shipment.ASSIGNED Then
                Dim SQL As String
                _edituser = pUserId
                _editdate = DateTime.Now
                _status = WMS.Lib.Statuses.Shipment.SHEDULED
                SQL = String.Format("UPDATE SHIPMENT SET STATUS ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(SQL)
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Status Incorrect", "Shipment Status Incorrect")
            End If
        End Sub

        Public Sub SetAtDock(ByVal pUserId As String)
            If _status = WMS.Lib.Statuses.Shipment.STATUSNEW Or _status = WMS.Lib.Statuses.Shipment.ASSIGNED Or _status = WMS.Lib.Statuses.Shipment.SHEDULED Then
                Dim oldStatus, SQL As String
                oldStatus = _status
                _edituser = pUserId
                _editdate = DateTime.Now
                _status = WMS.Lib.Statuses.Shipment.ATDOCK
                SQL = String.Format("UPDATE SHIPMENT SET STATUS ='{0}', EDITDATE ={1}, EDITUSER ={2} WHERE {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(SQL)

                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentAtDock)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPATDOCK)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", "")
                aq.Add("DOCUMENT", _shipment)
                aq.Add("DOCUMENTLINE", 0)
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMWAREHOUSEAREA", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", oldStatus)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOWAREHOUSEAREA", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", pUserId)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", pUserId)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", pUserId)
                aq.Send(WMS.Lib.Actions.Audit.SHIPATDOCK)
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Status Incorrect", "Shipment Status Incorrect")
            End If
        End Sub

#End Region

#Region "Ship"

        Public Sub Ship(ByVal pUser As String)
            If _status <> WMS.Lib.Statuses.Shipment.SHIPPED AndAlso CanShip Then
                'For Each oOrder As OutboundOrderHeader In Orders
                '    If Not oOrder.CanShip Then
                '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot ship shipment, orders have invalid status", "Cannot ship shipment, orders have invalid status")
                '    End If
                'Next
                SetStatus(WMS.Lib.Statuses.Shipment.SHIPPING, pUser)
                Dim qh As New Made4Net.Shared.QMsgSender
                qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentShip)
                qh.Values.Add("DOCUMENT", _shipment)
                qh.Values.Add("USER", pUser)
                qh.Send("Shipper", _shipment)
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cant Ship Shipment, incorrect status", "Cant Ship Shipment, incorrect status")
            End If
        End Sub

        Public Sub ShipShipment(ByVal pUser As String)
            If _status <> WMS.Lib.Statuses.Shipment.CANCELED AndAlso _status <> WMS.Lib.Statuses.Shipment.SHIPPED Then
                '-------- Try to create handling unit transaction for this shipment loads
                CreateHuTransactions()
                '-------- Try to ship all outboundorders
                ShipLoads(pUser)

                _editdate = DateTime.Now
                _edituser = pUser
                _status = WMS.Lib.Statuses.Shipment.SHIPPED
                _shippeddate = DateTime.Now
                CaseDetail.SetStatusByShipment(_shipment, _status, _edituser)
                Dim sql As String = String.Format("Update Shipment set status={0},shippeddate={1},editdate={2},edituser={3} where {4}", _
                    Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(sql)
                '----------------- Added By Lev ------------------------
                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentShip)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHPSHIP)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", "")
                aq.Add("DOCUMENT", _shipment)
                aq.Add("DOCUMENTLINE", 0)
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMWAREHOUSEAREA", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", _status)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOWAREHOUSEAREA", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", pUser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", pUser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", pUser)
                aq.Send(WMS.Lib.Actions.Audit.SHPSHIP)

            Else
                Throw New Made4Net.Shared.M4NException(New Exception, "Cant Ship Shipment", "Cant Ship Shipment")
            End If
        End Sub

        Private Sub CreateHuTransactions()
            ' Get all containers of this shipment
            Dim ShipmentContainersSql As String = "SELECT DISTINCT HANDLINGUNIT as CONTAINERID FROM SHIPMENTLOADS SL INNER JOIN INVLOAD IL ON SL.LOADID=IL.LOADID WHERE SL.SHIPMENT='" & _shipment & "'"
            Dim ShipmentContainerDt As DataTable = New DataTable
            DataInterface.FillDataset(ShipmentContainersSql, ShipmentContainerDt)
            For Each ShipmentContainerDr As DataRow In ShipmentContainerDt.Rows
                Try
                    Dim Cnt As Container = New WMS.Logic.Container(ShipmentContainerDr("CONTAINERID"), False)
                    ' Load can be from Outbound Order or from Flowthrows, so lest look in both tables
                    If Cnt.HandlingUnitType <> "" Then
                        If DataInterface.ExecuteScalar("SELECT COUNT(1) FROM ORDERLOADS WHERE DOCUMENTTYPE='FLWTH' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'") > 0 Then
                            ' It's flowthrogh
                            Dim flowthroghid As String = DataInterface.ExecuteScalar("SELECT ORDERID FROM ORDERLOADS WHERE DOCUMENTTYPE='FLWTH' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                            Dim consignee As String = DataInterface.ExecuteScalar("SELECT CONSIGNEE FROM ORDERLOADS WHERE DOCUMENTTYPE='FLWTH' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                            Dim fh As Flowthrough = Flowthroughs.getOrder(consignee, flowthroghid) ' New Flowthrough(consignee, flowthroghid)
                            Dim strHUTransIns As String = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                            "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & fh.CONSIGNEE & "','" & fh.FLOWTHROUGH & "','" & fh.ORDERTYPE & "','" & fh.TARGETCOMPANY & "','" & fh.TARGETCOMPANYTYPE & "','" & Cnt.HandlingUnitType & "',1,GETDATE(),'" & WMS.Logic.GetCurrentUser & "',GETDATE(),'" & WMS.Logic.GetCurrentUser & "')"
                            DataInterface.RunSQL(strHUTransIns)
                        Else
                            ' It's outboubd order
                            Dim strHUTransIns As String
                            Dim orderid As String = DataInterface.ExecuteScalar("SELECT ORDERID FROM orderloads WHERE  DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                            Dim consignee As String = DataInterface.ExecuteScalar("SELECT CONSIGNEE FROM orderloads WHERE DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                            Dim oo As OutboundOrderHeader = New OutboundOrderHeader 'Orders.getOrder(consignee, orderid) 'New OutboundOrderHeader(consignee, orderid)
                            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SHIPTO) FROM INVLOAD CNL INNER JOIN ORDERLOADS ODL ON CNL.LOADID=ODL.LOADID INNER JOIN OUTBOUNDORHEADER OH ON ODL.CONSIGNEE=OH.CONSIGNEE AND ODL.ORDERID=OH.ORDERID WHERE CNL.HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") = 1 Then
                                strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & oo.CONSIGNEE & "','" & oo.ORDERID & "','" & oo.ORDERTYPE & "','" & oo.TARGETCOMPANY & "','" & oo.COMPANYTYPE & "','" & Cnt.HandlingUnitType & "',1,GETDATE(),'" & WMS.Logic.GetCurrentUser & "',GETDATE(),'" & WMS.Logic.GetCurrentUser & "')"
                                DataInterface.RunSQL(strHUTransIns)
                            Else
                                strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(), NULL , NULL, NULL,'" & oo.TARGETCOMPANY & "','" & oo.COMPANYTYPE & "','" & Cnt.HandlingUnitType & "',1,GETDATE(),'" & WMS.Logic.GetCurrentUser & "',GETDATE(),'" & WMS.Logic.GetCurrentUser & "')"
                                DataInterface.RunSQL(strHUTransIns)
                            End If
                        End If
                    End If
                    ' Check if there is onther container on this container, if yes create hu transaction for it
                    Dim OnContainerSql As String = "SELECT ISNULL(ONCONTAINER,'') FROM CONTAINER WHERE CONTAINER='" & ShipmentContainerDr("CONTAINERID") & "'"
                    Dim OnContainerId As String = DataInterface.ExecuteScalar(OnContainerSql)
                    If OnContainerId <> "" Then
                        Dim OnCnt As Container = New Container(OnContainerId, False)
                        If OnCnt.HandlingUnitType <> "" Then
                            If DataInterface.ExecuteScalar("SELECT COUNT(1) FROM ORDERLOADS WHERE  DOCUMENTTYPE='FLWTH' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'") > 0 Then
                                ' It's flowthrogh
                                Dim flowthroghid As String = DataInterface.ExecuteScalar("SELECT ORDERID FROM ORDERLOADS WHERE DOCUMENTTYPE='FLWTH' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                                Dim consignee As String = DataInterface.ExecuteScalar("SELECT CONSIGNEE FROM ORDERLOADS WHERE DOCUMENTTYPE='FLWTH' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                                Dim fh As Flowthrough = Flowthroughs.getOrder(consignee, flowthroghid) ' New Flowthrough(consignee, flowthroghid)
                                Dim strHUTransIns As String = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & fh.CONSIGNEE & "','" & fh.FLOWTHROUGH & "','" & fh.ORDERTYPE & "','" & fh.TARGETCOMPANY & "','" & fh.TARGETCOMPANYTYPE & "','" & OnCnt.HandlingUnitType & "',1,GETDATE(),'" & WMS.Logic.GetCurrentUser & "',GETDATE(),'" & WMS.Logic.GetCurrentUser & "')"
                                DataInterface.RunSQL(strHUTransIns)
                            Else
                                ' It's outboubd order
                                Dim orderid As String = DataInterface.ExecuteScalar("SELECT ORDERID FROM orderloads WHERE DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                                Dim consignee As String = DataInterface.ExecuteScalar("SELECT CONSIGNEE FROM orderloads WHERE  DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                                Dim oo As OutboundOrderHeader = New OutboundOrderHeader 'Orders.getOrder(consignee, orderid) 'New OutboundOrderHeader(consignee, orderid)
                                Dim strHUTransIns As String = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & oo.CONSIGNEE & "','" & oo.ORDERID & "','" & oo.ORDERTYPE & "','" & oo.TARGETCOMPANY & "','" & oo.COMPANYTYPE & "','" & OnCnt.HandlingUnitType & "',1,GETDATE(),'" & WMS.Logic.GetCurrentUser & "',GETDATE(),'" & WMS.Logic.GetCurrentUser & "')"
                                DataInterface.RunSQL(strHUTransIns)
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try
            Next
        End Sub

        Private Sub ShipLoads(ByVal pUser As String)
            ' Select all loads
            Dim Sql As String = String.Format("SELECT LOADS.*, CONTAINER.CONTAINER as CONTAINERID, HUTYPE, ORDERID, ORDERLINE, DOCUMENTTYPE, attribute.*, skuuom.netweight,skuuom.volume  FROM SHIPMENTLOADS INNER JOIN LOADS ON SHIPMENTLOADS.LOADID=LOADS.LOADID INNER JOIN ORDERLOADS ON SHIPMENTLOADS.LOADID=ORDERLOADS.LOADID left outer JOIN CONTAINER ON dbo.CONTAINER.CONTAINER = loads.HANDLINGUNIT left outer join attribute on loads.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' left outer join skuuom on loads.consignee = skuuom.consignee and loads.sku = skuuom.sku and (skuuom.loweruom = '' or skuuom.loweruom is null) WHERE SHIPMENT = '{0}'", _shipment)
            Dim dt As New DataTable
            DataInterface.FillDataset(Sql, dt)

            ' Select all load attributes
            Sql = String.Format("SELECT attribute.* FROM SHIPMENTLOADS left outer join attribute on SHIPMENTLOADS.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' WHERE SHIPMENT = '{0}'", _shipment)
            Dim dtAtt As New DataTable
            DataInterface.FillDataset(Sql, dtAtt)

            Dim docType As String
            For Each dr As DataRow In dt.Rows
                Try
                    docType = dr("DOCUMENTTYPE")
                    Dim loadatt() As DataRow
                    loadatt = dtAtt.Select(String.Format("pkey1 = '{0}'", dr("loadid")))
                    Dim dratt As DataRow = Nothing
                    If (loadatt.Length > 0) Then
                        dratt = loadatt(0)
                    End If
                    Dim oLoad As New WMS.Logic.Load(dr, dratt)
                    Select Case docType
                        Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
                            oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE"))) ', Nothing, Nothing, Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")).LINES.Line(dr("ORDERLINE")), Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")))
                        Case Else
                            oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE"))) ', Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID")).Lines.GetLine(dr("ORDERLINE")), Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID")))
                    End Select
                Catch ex As Exception
                End Try
            Next
        End Sub

#End Region

#Region "Handling Units"

        Public Sub AddHandlingUnit(ByVal pHandlingUnitType As String, ByVal pQty As Decimal, ByVal pUser As String)
            If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Shipment Status", "Incorrect Shipment Status")
            End If
            Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction
            oHUTrans.Create(Nothing, WMS.Logic.HandlingUnitTransactionTypes.Shipment, _shipment, Nothing, _
                        Nothing, Nothing, Nothing, Nothing, pHandlingUnitType, pQty, pUser)
            _handlingunits.Add(oHUTrans)
        End Sub

        Public Sub UpdateHandlingUnit(ByVal pHUTransId As String, ByVal pQty As Decimal, ByVal pUser As String)
            If _status = WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Order Status", "Incorrect Order Status")
            End If
            Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction(pHUTransId)
            oHUTrans.UpdateHUQty(pQty, pUser)
            _handlingunits.Remove(_handlingunits.HandlingUnitTransaction(pHUTransId))
            _handlingunits.Add(oHUTrans)
        End Sub

#End Region

#Region "Printing"

        Public Sub PrintLoadingWorksheet(ByVal Language As Int32, ByVal pUser As String)
            Dim oQsender As New Made4Net.Shared.QMsgSender
            Dim repType As String
            Dim dt As New DataTable
            repType = "ShipMan"
            Dim Copies As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "repLoadingWorksheet", "Copies"))
            oQsender.Add("REPORTNAME", repType)
            oQsender.Add("REPORTID", "repLoadingWorksheet")
            Dim setId As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "repLoadingWorksheet", "DataSetName"))
            oQsender.Add("DATASETID", setId)
            oQsender.Add("FORMAT", "PDF")
            oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
            oQsender.Add("USERID", pUser)
            oQsender.Add("LANGUAGE", Language)
            Try
                oQsender.Add("PRINTER", "")
                oQsender.Add("COPIES", Copies)

                If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                    oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
                Else
                    oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
                End If
            Catch ex As Exception
            End Try
            oQsender.Add("WHERE", String.Format("SHIPMENT = '{0}'", _shipment))
            oQsender.Send("Report", repType)
        End Sub

        Public Sub PrintShippingManifest(ByVal Language As Int32, ByVal pUser As String)
            Dim oQsender As New Made4Net.Shared.QMsgSender
            Dim repType As String
            Dim dt As New DataTable
            repType = "ShipMan"
            Dim Copies As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "ShipMan", "Copies"))
            oQsender.Add("REPORTNAME", repType)
            oQsender.Add("REPORTID", "ShipMan")
            Dim setId As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "ShipMan", "DataSetName"))
            oQsender.Add("DATASETID", setId)
            oQsender.Add("FORMAT", "PDF")
            oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
            oQsender.Add("USERID", pUser)
            oQsender.Add("LANGUAGE", Language)
            Try
                oQsender.Add("PRINTER", "")
                oQsender.Add("COPIES", Copies)
                If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                    oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
                Else
                    oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
                End If
            Catch ex As Exception
            End Try
            oQsender.Add("WHERE", String.Format("SHIPMENT = '{0}'", _shipment))
            oQsender.Send("Report", repType)
        End Sub

        Public Sub PrintShipmentPackingList(ByVal Language As Int32, ByVal pUser As String)
            Try
                Dim DocumentsInShipment As DataTable = New DataTable
                DataInterface.FillDataset("SELECT * FROM vShipingManifestHeader WHERE SHIPMENT='" & _shipment & "'", DocumentsInShipment)
                For Each doc As DataRow In DocumentsInShipment.Rows
                    Select Case Convert.ToString(doc("DOCTYPE"))
                        Case "OUTBOUND"
                            Dim oDoc As New OutboundOrderHeader(doc("CONSIGNEE"), doc("ORDERID"))
                            oDoc.PrintShippingManifest(Language, pUser)
                        Case "FLOWTROUGH"
                            Dim oDoc As New Flowthrough(doc("CONSIGNEE"), doc("ORDERID"))
                            oDoc.PrintShippingManifest(Language, pUser)
                        Case "TRANSSHIPEMNT"
                            Dim oDoc As New TransShipment(doc("CONSIGNEE"), doc("ORDERID"))
                            oDoc.PrintShippingManifest(Language, pUser)
                    End Select
                Next
            Catch ex As Exception

            End Try
        End Sub

#End Region

#End Region

    End Class
End Namespace

#End Region