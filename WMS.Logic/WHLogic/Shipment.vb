Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Evaluation

#Region "SHIPMENT"

' <summary>
' This object represents the properties and methods of a SHIPMENT.
' </summary>
<CLSCompliant(False)> Public Class Shipment

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

        Protected _documenttype As String = String.Empty

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return String.Format("shipment = '{0}' and consignee = '{1}' and orderid = '{2}' and orderline = {3} and documenttype = '{4}'", _shipment, _consignee, _orderid, _orderline, _documenttype)
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

        Public Property DOCUMENTTYPE() As String
            Get
                Return _documenttype
            End Get
            Set(ByVal value As String)
                _documenttype = value
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

            _documenttype = dr("DOCUMENTTYPE")
            'Dim sh As New Shipment(_shipment, True)
            'If sh.IsLoadingCompleted Then
            '    sh.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, WMS.Logic.GetCurrentUser)
            'End If
        End Sub
        'Commented   PWMS-802 for  RWMS-847   
        'Public Shared Function Exists(ByVal pShipmentId As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32) As Boolean
        '    Dim sql As String = String.Format("Select count(1) from shipmentdetail where shipment = '{0}' and consignee = '{1}' and orderid = '{2}' and orderline = {3}", pShipmentId, pConsignee, pOrderid, pOrderLine)
        '    Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        'End Function
        'Added for PWMS-802 for RWMS-847   
        Public Shared Sub UpdateQtyIfExists(ByVal pUser As String, ByVal qtymodified As Decimal, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32)
            'RWMS-2581 RWMS-2554
            Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
            'RWMS-2581 RWMS-2554 END
            Dim sql As String = String.Format("Select count(1) from shipmentdetail where consignee = '{0}' and orderid = '{1}' and orderline = {2}", pConsignee, pOrderid, pOrderLine)
            If Convert.ToBoolean(DataInterface.ExecuteScalar(sql)) Then
                Dim shipmentsql As String
                shipmentsql = String.Format("update SHIPMENTDETAIL set UNITS = {0}, edituser = {1},editdate = {2} where consignee = '{3}' and orderid = '{4}' and orderline = {5}", _
                qtymodified, Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(DateTime.Now), pConsignee, pOrderid, pOrderLine)
                DataInterface.RunSQL(shipmentsql)

                'RWMS-2581 RWMS-2554
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" Update shipment detail SQL query:{0}", shipmentsql))
                End If
                'RWMS-2581 RWMS-2554 END
            End If
        End Sub
        'End Added for PWMS-802 for RWMS-847   


        Private Shared Function LineAssignedToAnotherShipment(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32) As Boolean
            Dim sql As String = String.Format("Select count(1) from shipmentdetail where consignee = '{0}' and orderid = '{1}' and orderline = {2}", pConsignee, pOrderid, pOrderLine)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

#End Region

#Region "Create / Delete"

        Public Sub Create(ByVal pShipment As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32, ByVal pUnits As Decimal, ByVal pLoadingSequence As Int32, ByVal pUser As String, ByVal pDocumentType As String)
            If pDocumentType.Equals(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, StringComparison.OrdinalIgnoreCase) Then
                Dim oOrdDetail As New OutboundOrderHeader.OutboundOrderDetail(pConsignee, pOrderid, pOrderLine)
                If pUnits > oOrdDetail.OpenQtyLeftToAssignToShipment Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add shipment detail, open order line quantity is less than supplied quantity", "Cannot add shipment detail, open order line quantity is less than supplied quantity")
                End If
                _documenttype = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
            ElseIf pDocumentType.Equals(WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, StringComparison.OrdinalIgnoreCase) Then
                'Commeted for RWMS-483
                'Dim ftDet As New FlowthroughDetail(pShipment, pConsignee, pOrderid, pOrderLine)
                'End Commeted for RWMS-483
                'Added for RWMS-483
                Dim ftDet As New FlowthroughDetail(pConsignee, pOrderid, pOrderLine)
                'End Added for RWMS-483
                If pUnits > ftDet.OpenQtyLeftToAssignToShipment Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add shipment detail, open order line quantity is less than supplied quantity", "Cannot add shipment detail, open order line quantity is less than supplied quantity")
                End If
                _documenttype = WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
            ElseIf pDocumentType.Equals(WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT, StringComparison.OrdinalIgnoreCase) Then
                '' Add Validation?
                _documenttype = WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
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

            Dim oSql As String = String.Format("INSERT INTO shipmentdetail (SHIPMENT, CONSIGNEE, ORDERID, ORDERLINE, UNITS, LOADINGSEQ, ADDDATE, ADDUSER, EDITDATE, EDITUSER, DOCUMENTTYPE) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _
                    Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), _
                    Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_loadingseq), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
                    Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.FormatField(_documenttype))
            DataInterface.RunSQL(oSql)
        End Sub


        Public Sub Delete()
            Dim oSql As String = String.Format("delete from shipmentdetail where {0}", WhereClause)
            DataInterface.RunSQL(oSql)
        End Sub

        Public Function CanDeleteShipment(shipId As String, ByRef Message As String) As Boolean
            Dim count As Integer
            Dim sql As String = String.Format("SELECT COUNT(*) FROM SHIPMENTDETAIL WHERE SHIPMENT='{0}'", shipId)
            count = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If count > 0 Then
                Message = "Can't delete shipment with orders assigned"
                Return True
            Else
                Return False
            End If
        End Function
        Public Sub AddQauntity(ByVal pUnits As Decimal, ByVal pUser As String)
            'RWMS-2581 RWMS-2554
            Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
            'RWMS-2581 RWMS-2554 END

            If _documenttype.Equals(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, StringComparison.OrdinalIgnoreCase) Then
                Dim oOrdDetail As New OutboundOrderHeader.OutboundOrderDetail(_consignee, _orderid, _orderline)
                'If pUnits + _units > oOrdDetail.OpenQtyLeftToAssignToShipment Then
                If pUnits > oOrdDetail.OpenQtyLeftToAssignToShipment Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add shipment detail, open order line quantity is less than supplied quantity", "Cannot add shipment detail, open order line quantity is less than supplied quantity")
                End If
            ElseIf _documenttype.Equals(WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, StringComparison.OrdinalIgnoreCase) Then
                Dim ftDet As New FlowthroughDetail(_consignee, _orderid, _orderline)
                If pUnits > ftDet.OpenQtyLeftToAssignToShipment Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add shipment detail, open order line quantity is less than supplied quantity", "Cannot add shipment detail, open order line quantity is less than supplied quantity")
                End If
            End If

            _units += pUnits
            _editdate = DateTime.Now
            _edituser = pUser
            Dim oSql As String = String.Format("update shipmentdetail set units={0},edituser={1},editdate={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(oSql)
            'RWMS-2581 RWMS-2554
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Update shipment detail SQL query:{0}", oSql))
            End If
            'RWMS-2581 RWMS-2554 END
        End Sub

        Public Sub MoveToShipment(ByVal pNewShipmentId As String, ByVal pUser As String)
            _editdate = DateTime.Now
            _edituser = pUser

            Dim oSql As String = String.Format("select COUNT (1) SHIPMENT from SHIPMENT where SHIPMENT={0} and STATUS not in({1},{2})", _
                Made4Net.Shared.Util.FormatField(pNewShipmentId), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Shipment.CANCELED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Shipment.SHIPPED))
            If DataInterface.ExecuteScalar(oSql) = 0 Then
                Throw New Made4Net.Shared.M4NException("Incorrect Shipment Status ")
            End If
            oSql = String.Format("update shipmentdetail set shipment={0},edituser={1},editdate={2} where {3}", _
                Made4Net.Shared.Util.FormatField(pNewShipmentId), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
            DataInterface.RunSQL(oSql)

            'Added for RWMS-2311
            oSql = String.Format("update SHIPMENTLOADS set shipment={0},edituser={1},editdate={2} where shipment={3}", _
                Made4Net.Shared.Util.FormatField(pNewShipmentId), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_shipment))
            DataInterface.RunSQL(oSql)
            'End Added for RWMS-2311

            oSql = String.Format("update shipment set Status={0},edituser={1},editdate={2} where shipment={3}", _
              Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Shipment.ASSIGNED), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(pNewShipmentId))
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

        'Public Function getShipmentDetail(ByVal Consignee As String, ByVal OrderId As String, ByVal pOrderLine As Int32) As ShipmentDetail
        Public Function getShipmentDetail(ByVal Consignee As String, ByVal OrderId As String, ByVal pOrderLine As Int32, ByVal pDocType As String) As ShipmentDetail
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).CONSIGNEE.ToUpper = Consignee.ToUpper And Item(i).ORDERID.ToUpper = OrderId.ToUpper And Item(i).ORDERLINE = pOrderLine AndAlso Item(i).DOCUMENTTYPE = pDocType Then
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
#Region "Delete"

        Public Sub Delete(ByVal pShipment As String, ByVal pVehicleLocation As String, ByVal pHandlingUnit As String)
          
            _shipment = pShipment
            _vehiclelocation = pVehicleLocation

            _handlingunit = pHandlingUnit
            _adddate = DateTime.Now
            _editdate = DateTime.Now
           

            Dim Sql As String = String.Format("delete from SHIPMENTCOMPARTMENT where SHIPMENT ={0} and VEHICLELOCATION ={1} and HANDLINGUNIT ={2} ", _
                Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_vehiclelocation), Made4Net.Shared.Util.FormatField(_handlingunit))
            DataInterface.RunSQL(Sql)
        End Sub

#End Region
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

            'Commented for RWMS-2343 RWMS-2314 RWMS-2348
            'Dim Sql As String = String.Format("INSERT INTO SHIPMENTCOMPARTMENT (SHIPMENT, VEHICLELOCATION, HANDLINGUNIT, COMPARTMENT, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
            '    Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_vehiclelocation), Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_compartment), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
            'End Commented for RWMS-2343 RWMS-2314 RWMS-2348

            ''Added for RWMS-2343 RWMS-2314 RWMS-2348 if SHIPMENTCOMPARTMENT already has record for SHIPMENT,VEHICLELOCATION,HANDLINGUNIT update else insert
            Dim sqlCompartment = String.Format("select count(1) from SHIPMENTCOMPARTMENT where SHIPMENT={0} and VEHICLELOCATION={1} and HANDLINGUNIT={2}", _
        Made4Net.Shared.FormatField(_shipment), Made4Net.Shared.FormatField(_vehiclelocation), Made4Net.Shared.FormatField(_handlingunit))

            Dim exists As Boolean = Convert.ToBoolean(DataInterface.ExecuteScalar(sqlCompartment))
            Dim Sql As String
            If Not exists Then
                Sql = String.Format("INSERT INTO SHIPMENTCOMPARTMENT (SHIPMENT, VEHICLELOCATION, HANDLINGUNIT, COMPARTMENT, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
                Made4Net.Shared.Util.FormatField(_shipment), Made4Net.Shared.Util.FormatField(_vehiclelocation), Made4Net.Shared.Util.FormatField(_handlingunit), Made4Net.Shared.Util.FormatField(_compartment), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
                DataInterface.RunSQL(Sql)
            Else
                '_compartment = Made4Net.Shared.Util.getNextCounter("COMPARTMENT")
                'Sql = String.Format("UPDATE SHIPMENTCOMPARTMENT SET COMPARTMENT ={0}, EDITDATE ={1}, EDITUSER ={2} where shipment ={3} and (COMPARTMENT = '' or COMPARTMENT is null)", _
                'Made4Net.Shared.Util.FormatField(_compartment), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_shipment))
                'DataInterface.RunSQL(Sql)
            End If
            'End Added for RWMS-2343 RWMS-2314 RWMS-2348

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
            'Commented for RWMS-2447
            ''RWMS-1710 - In the sql joined the outboundordetail table to filter the records having qtymodified=0   
            'Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits from (select sd.SHIPMENT,SUM(sd.units) as units from" & _
            '" SHIPMENTDETAIL sd inner join outboundordetail od on sd.ORDERID=od.ORDERID and sd.ORDERLINE=od.ORDERLINE and sd.CONSIGNEE=od.CONSIGNEE and od.QTYMODIFIED<>0 group by SHIPMENT) sd left outer join (select sl.SHIPMENT,sum(iv.UNITS) as unitsLoaded from SHIPMENTLOADS sl inner join LOADS" & _
            '" iv on sl.LOADID = iv.LOADID group by sl.SHIPMENT ) sl on sl.SHIPMENT = sd.SHIPMENT where sd.{0}", Me.WhereClause)
            'Dim remainingUnits As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            'If remainingUnits > 0 Then
            '    Return False
            'Else
            '    Return True

            'End If

            'End Commented for RWMS-2447
            'Added for RWMS-2447
            Dim sqlUnits As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits,sd.units as shipmentunits from (select sd.SHIPMENT,SUM(sd.units) as units from" & _
            " SHIPMENTDETAIL sd inner join outboundordetail od on sd.ORDERID=od.ORDERID and sd.ORDERLINE=od.ORDERLINE and sd.CONSIGNEE=od.CONSIGNEE and od.QTYMODIFIED<>0 group by SHIPMENT) sd left outer join (select sl.SHIPMENT,sum(iv.UNITS) as unitsLoaded from SHIPMENTLOADS sl inner join LOADS" & _
            " iv on sl.LOADID = iv.LOADID group by sl.SHIPMENT ) sl on sl.SHIPMENT = sd.SHIPMENT where sd.{0}", Me.WhereClause)
            Dim dtUnits As New DataTable
            DataInterface.FillDataset(sqlUnits, dtUnits)
            If dtUnits.Rows.Count > 0 Then
                Dim remainingUnits As Decimal = dtUnits.Rows(0)("RemainingUnits")
                Dim Units As Decimal = dtUnits.Rows(0)("shipmentunits")
                If Units = 0 Then
                    Return False
                End If
                If remainingUnits > 0 Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
            'End for RWMS-2447

            'End for PWMS-806 for RWMS-862 and RWMS-791

            ' old code   
            'Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits from (select sd.SHIPMENT,SUM(sd.units) as units from" & _   
            '" SHIPMENTDETAIL sd group by SHIPMENT) sd left outer join (select sl.SHIPMENT,sum(iv.UNITS) as unitsLoaded from SHIPMENTLOADS sl inner join LOADS" & _   
            '" iv on sl.LOADID = iv.LOADID group by sl.SHIPMENT ) sl on sl.SHIPMENT = sd.SHIPMENT where sd.{0}", Me.WhereClause)   
            'Dim remainingUnits As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)   
            'If remainingUnits > 0 Then   
            ' Return False   
            'Else   
            ' Return True   
            'End If   

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
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Save" Then
            Dim scdate As DateTime
            dr = ds.Tables(0).Rows(0)
            Dim dor, carr, vehicle, trail, drv1, drv2, seal1, seal2, notes, bol, transporttype, transportreference, stagingLane, stagingWarehouseArea As String
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
            Dim dor, carr, vehicle, trail, drv1, drv2, seal1, seal2, notes, stagingLane, stagingWarehouseArea As String
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

            'RWMS-2824
            Dim ldmsg As String = "Cannot unassign order. Shipment payloads needs to be unloaded - "
            Dim shiploadid As String
            'RWMS-2824 END
            For Each dr In ds.Tables(0).Rows
                Dim docType As String
                Try
                    'docType = dr("DOCTYPE")
                    docType = dr("DOCUMENTTYPE")
                Catch ex As Exception
                    docType = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                End Try
                'DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser(), docType)

                'Added for RWMS-2342 RWMS-2334 - check whether the LOAD is in ShipmentLoads
                Dim dtvShipmentLoads As New DataTable
                Dim drvShipmentLoads As DataRow
                Dim shipmentOrder As String = dr("ORDERID")
                Dim shipmentOrderline As String = dr("ORDERLINE")
                'RWMS-2824
                'Dim SQLvShipLoad As String = String.Format("SELECT * FROM vshipmentloads WHERE SHIPMENT='{0}' and ORDERID='{1}' and ORDERLINE='{2}'", _shipment, shipmentOrder, shipmentOrderline)
                'RWMS-2824
                'RWMS-2824
                Dim SQLvShipLoad As String = String.Format("SELECT * FROM vshipmentloads WHERE SHIPMENT='{0}' and ORDERID='{1}' and ORDERLINE='{2}' and LOADED=1", _shipment, shipmentOrder, shipmentOrderline)
                'RWMS-2824 END
                DataInterface.FillDataset(SQLvShipLoad, dtvShipmentLoads)

                'RWMS-2824
                If dtvShipmentLoads.Rows.Count > 0 Then
                    For i As Integer = 0 To 4
                        shiploadid += dtvShipmentLoads.Rows(i)("LOADID") + ","
                        If i = dtvShipmentLoads.Rows.Count - 1 Then
                            Exit For
                        End If
                    Next
                    'For Each paramLoad As DataRow In dtvShipmentLoads.Rows
                    '    shiploadid += paramLoad("LOADID") + ","
                    'Next
                    ldmsg = ldmsg + shiploadid.TrimEnd(",")
                End If
                'RWMS-2824 END

                'Added for RWMS-2370 RWMS-2365 - check for 'UnAssignLoadedOrder'. 
                Dim dtUnAssignLoadedOrder As New DataTable
                Dim drUnAssignLoadedOrder As DataRow
                Dim SQLUnAssignLoadedOrder As String = "SELECT * FROM warehouseparams WHERE PARAMNAME='UnAssignLoadedOrder'"
                DataInterface.FillDataset(SQLUnAssignLoadedOrder, dtUnAssignLoadedOrder)
                If dtUnAssignLoadedOrder.Rows.Count > 0 Then
                    Dim unAssignLoadedOrder As String = dtUnAssignLoadedOrder.Rows(0)("PARAMVALUE")
                    If Not String.IsNullOrEmpty(unAssignLoadedOrder) Then
                        If Not unAssignLoadedOrder = "1" Then
                            If dtvShipmentLoads.Rows.Count > 0 Then
                                'Commented for RWMS-2824
                                'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                                'Commented for RWMS-2824 END
                                'RWMS-2824
                                Throw New Made4Net.Shared.M4NException(New Exception, ldmsg, ldmsg)
                                'RWMS-2824 END
                            End If
                        End If
                    Else
                        If dtvShipmentLoads.Rows.Count > 0 Then
                            'Commented for RWMS-2824
                            'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                            'Commented for RWMS-2824 END
                            'RWMS -2824
                            Throw New Made4Net.Shared.M4NException(New Exception, ldmsg, ldmsg)
                            'RWMS -2824 END
                        End If
                    End If
                Else
                    If dtvShipmentLoads.Rows.Count > 0 Then
                        'Commented for RWMS-2824
                        'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                        'Commented for RWMS-2824 END
                        'RWMS -2824
                        Throw New Made4Net.Shared.M4NException(New Exception, ldmsg, ldmsg)
                        'RWMS -2824 END
                    End If
                End If
                'End Added for RWMS-2370 RWMS-2365

                'Commented for for RWMS-2370 RWMS-2365
                'If dtvShipmentLoads.Rows.Count > 0 Then
                '    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                'End If
                'End Commented for RWMS-2370 RWMS-2365

                'End Added for RWMS-2342 RWMS-2334

                DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Common.GetCurrentUser(), docType)
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
                    'docType = dr("DOCTYPE")
                    docType = dr("DOCUMENTTYPE")
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
        ElseIf CommandName = "PrintLoadingPlan" Then
            For Each dr In ds.Tables(0).Rows
                _shipment = dr("SHIPMENT")
                Load()
                PrintLoadingDiagram(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
            Next

        End If
    End Sub


#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetSHIPMENT(ByVal pSHIPMENT As String) As SHIPMENT
        Return New SHIPMENT(pSHIPMENT)
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

    Public Shared Function IsShipmentFullyLoaded(ByVal ShipmentId As String) As Boolean

        Dim sql = String.Format("select count(1) SHIPMENT from SHIPMENT where SHIPMENT={0} and STATUS={1}", _
        Made4Net.Shared.FormatField(ShipmentId), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Shipment.LOADED))

        If DataInterface.ExecuteScalar(sql) < 1 Then
            Return False
        End If
        Return True
    End Function

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

        'Added for PWMS-805 and RWMS-857 Start   
        Dim blnNewShipmentDetail As Boolean
        If ShipmentDetails.Count = 0 Then
            blnNewShipmentDetail = True
        End If
        'Ended for PWMS-805 and RWMS-857 End 

        Dim oShipDetail As ShipmentDetail
        oShipDetail = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine, pDocumentType)
        If oShipDetail Is Nothing Then
            oShipDetail = New ShipmentDetail
            Select Case pDocumentType
                Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    oShipDetail.Create(_shipment, Consignee, OrdId, pOrderLine, pUnits, pLoadingSequence, pUser, WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER)
                    _shipmentdetails.add(oShipDetail)
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    oShipDetail.Create(_shipment, Consignee, OrdId, pOrderLine, pUnits, pLoadingSequence, pUser, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH)
                    _shipmentdetails.add(oShipDetail)
                Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
                    Dim t1 As New TransShipment(Consignee, OrdId)
                    t1.AssignToShipment(_shipment, pUser)
                    Transshipments.add(t1)
            End Select
        Else
            oShipDetail.AddQauntity(pUnits, pUser)
        End If

        CalculateStdtime()
        _editdate = DateTime.Now
        _edituser = pUser
        If Me.STATUS = WMS.Lib.Statuses.Shipment.STATUSNEW Then
            _status = WMS.Lib.Statuses.Wave.ASSIGNED
            DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        End If

        'Added for PWMS-805 and RWMS-857 Start   
        If blnNewShipmentDetail Then
            AssignOrderScheduleDate(Consignee, OrdId, pDocumentType)
        End If
        'Ended for PWMS-805 and RWMS-857 Start 

    End Sub
    'Added for PWMS-805 and RWMS-857 Start   
    Private Sub AssignOrderScheduleDate(ByVal Consignee As String, ByVal OrdId As String, ByVal pDocumentType As String)
        Try
            Select Case pDocumentType
                Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    Dim oOutboundOrder As OutboundOrderHeader = New OutboundOrderHeader(Consignee, OrdId, True)
                    _scheddate = oOutboundOrder.SCHEDULEDDATE
                    DataInterface.RunSQL(String.Format("Update shipment set SCHEDDATE = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_scheddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim oFlowthroughOrder As Flowthrough = New Flowthrough(Consignee, OrdId, True)
                    _scheddate = oFlowthroughOrder.SCHEDULEDDELIVERYDATE
                    DataInterface.RunSQL(String.Format("Update shipment set SCHEDDATE = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_scheddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
                Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
                    'Do nothing   
            End Select
        Catch ex As Exception
        End Try

    End Sub

    'Ended for PWMS-805 and RWMS-857 Start
    Public Sub DeAssignOrder(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pUser As String, Optional ByVal pDocumentType As String = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER)
        If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "INCORRECT SHIPMENT STATUS", "INCORRECT SHIPMENT STATUS")
        End If
        Select Case pDocumentType
            Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                Dim sd As ShipmentDetail
                sd = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine, pDocumentType)
                sd.Delete()
                Me.ShipmentDetails.Remove(sd)
                'Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                'Dim fl As Flowthrough
                'fl = Me.Flowthroughs.getOrder(Consignee, OrdId)
                'Me.Flowthroughs.Remove(fl)
                'fl.DeAssignFromShipment(pUser)
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

    'Public Sub MoveDetailToShipment(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pNewShipmentId As String, ByVal pUser As String)
    'Commented for RWMS-791  
    'Public Sub MoveDetailToShipment(ByVal Consignee As String, ByVal pDocumentType As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pNewShipmentId As String, ByVal pUser As String)
    '    If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "INCORRECT SHIPMENT STATUS", "INCORRECT SHIPMENT STATUS")
    '    End If
    '    If String.IsNullOrEmpty(pNewShipmentId) Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Target Shipment field cannot be blank", "Target Shipment field cannot be blank")
    '    End If
    '    If Not Exists(pNewShipmentId) Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "New shipment does not exists - Cannot Move detail to new shipment", "New shipment does not exists - Cannot Move detail to new shipment")
    '    End If
    '    Dim sd As ShipmentDetail
    '    sd = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine, pDocumentType)
    '    sd.MoveToShipment(pNewShipmentId, pUser)
    '    Me.ShipmentDetails.Remove(sd)

    '    _editdate = DateTime.Now
    '    _edituser = pUser
    '    If ShipmentDetails.Count = 0 Then
    '        _status = WMS.Lib.Statuses.Shipment.STATUSNEW
    '        DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
    '    Else
    '        DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
    '    End If
    'End Sub
    'End Commented for RWMS-791  
    'Added  for PWMS-810 and RWMS-791  
    Public Sub MoveDetailToShipment(ByVal Consignee As String, ByVal pDocumentType As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pNewShipmentId As String, ByVal pUser As String)
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
        sd = Me.ShipmentDetails.getShipmentDetail(Consignee, OrdId, pOrderLine, pDocumentType)
        Dim fromShipmentId As String = sd.SHIPMENT

        Dim SQL As String = String.Format("SELECT LOADID,HANDLINGUNIT FROM VSHIPMENTLOADS WHERE SHIPMENT={0} AND ORDERID={1} AND ORDERLINE={2} AND CONSIGNEE={3}", Made4Net.Shared.Util.FormatField(fromShipmentId), Made4Net.Shared.Util.FormatField(OrdId), Made4Net.Shared.Util.FormatField(pOrderLine), Made4Net.Shared.Util.FormatField(Consignee))
        Dim ds As New DataSet
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, ds)

        If ds.Tables(0).Rows.Count > 0 Then

            For Each dr In ds.Tables(0).Rows
                Dim oLoad As New WMS.Logic.Load(Convert.ToString(dr("LOADID")))
                Dim fromContainer As String

                If Not String.IsNullOrEmpty(dr("LOADID")) Then
                    Dim loadid = dr("LOADID")
                    fromContainer = Convert.ToString(dr("HANDLINGUNIT"))
                    'check for single container load   
                    If isMultiLoadContainer(loadid, fromContainer, Consignee) Then

                        'get fromcontainer details   
                        Dim oldcnt As New WMS.Logic.Container(fromContainer, True)
                        'create new container id   
                        Dim containerID As String = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                        If Not Container.Exists(containerID) Then
                            Dim cnt As New Container
                            cnt.ContainerId = containerID
                            cnt.HandlingUnitType = oldcnt.HandlingUnitType
                            cnt.UsageType = oldcnt.UsageType
                            cnt.Location = oldcnt.Location
                            cnt.Warehousearea = oldcnt.Warehousearea
                            cnt.Post(pUser)
                                'move the old to new shipment and assign the new container to the new shipment   
                                sd.MoveToShipment(pNewShipmentId, pUser)

                                'write shipmentmoved to audit   
                                Dim sq As EventManagerQ = New EventManagerQ
                                sq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SHIPMENTMOVED)
                                sq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPMENTMOVED)
                                sq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                                sq.Add("ACTIVITYTIME", "0")
                                sq.Add("CONSIGNEE", Consignee)
                                sq.Add("DOCUMENT", pNewShipmentId)
                                sq.Add("DOCUMENTLINE", 0)
                                sq.Add("FROMLOAD", loadid)
                                sq.Add("FROMLOC", "")
                                sq.Add("FROMWAREHOUSEAREA", "")
                                sq.Add("FROMQTY", 0)
                                sq.Add("FROMSTATUS", "")
                                sq.Add("FROMCONTAINER", fromContainer)
                                sq.Add("NOTES", "")
                                sq.Add("SKU", "")
                                sq.Add("TOLOAD", loadid)
                                sq.Add("TOLOC", "")
                                sq.Add("TOWAREHOUSEAREA", "")
                                sq.Add("TOQTY", 0)
                                sq.Add("TOSTATUS", "")
                                sq.Add("TOCONTAINER", containerID)
                                sq.Add("USERID", pUser)
                                sq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                                sq.Add("ADDUSER", pUser)
                                sq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                                sq.Add("EDITUSER", pUser)
                                sq.Send(WMS.Lib.Actions.Audit.SHIPMENTMOVED)

                                'write orderlinemoved to audit   
                                Dim oq As EventManagerQ = New EventManagerQ
                                oq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ORDERLINEMOVED)
                                oq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.ORDERLINEMOVED)
                                oq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                                oq.Add("ACTIVITYTIME", "0")
                                oq.Add("CONSIGNEE", Consignee)
                                oq.Add("DOCUMENT", OrdId)
                                oq.Add("DOCUMENTLINE", pOrderLine)
                                oq.Add("FROMLOAD", loadid)
                                oq.Add("FROMLOC", "")
                                oq.Add("FROMWAREHOUSEAREA", "")
                                oq.Add("FROMQTY", 0)
                                oq.Add("FROMSTATUS", "")
                                oq.Add("FROMCONTAINER", fromContainer)
                                oq.Add("NOTES", "")
                                oq.Add("SKU", "")
                                oq.Add("TOLOAD", loadid)
                                oq.Add("TOLOC", "")
                                oq.Add("TOWAREHOUSEAREA", "")
                                oq.Add("TOQTY", 0)
                                oq.Add("TOSTATUS", "")
                                oq.Add("TOCONTAINER", containerID)
                                oq.Add("USERID", pUser)
                                oq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                                oq.Add("ADDUSER", pUser)
                                oq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                                oq.Add("EDITUSER", pUser)
                                oq.Send(WMS.Lib.Actions.Audit.ORDERLINEMOVED)

                                'update the new cntainer as handlingunit to the load   
                            'UpdateHandlingUnit(pNewShipmentId, OrdId, pOrderLine, Consignee, containerID)
                            UpdateHandlingUnit(containerID, loadid)
                                'print the new container label   
                                cnt.PrintContainerLabel()
                                cnt = Nothing

                                'remove the old ShipmentDetails   
                                Me.ShipmentDetails.Remove(sd)

                                'print the old container label   
                                oldcnt.PrintContainerLabel()
                                oldcnt = Nothing
                            End If
                        Else


                            'move the old to new shipment and assign the new container to the new shipment   
                            sd.MoveToShipment(pNewShipmentId, pUser)

                            'write shipmentmoved to audit   
                            Dim sq As EventManagerQ = New EventManagerQ
                            sq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SHIPMENTMOVED)
                            sq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPMENTMOVED)
                            sq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                            sq.Add("ACTIVITYTIME", "0")
                            sq.Add("CONSIGNEE", Consignee)
                            sq.Add("DOCUMENT", pNewShipmentId)
                            sq.Add("DOCUMENTLINE", 0)
                            sq.Add("FROMLOAD", loadid)
                            sq.Add("FROMLOC", "")
                            sq.Add("FROMWAREHOUSEAREA", "")
                            sq.Add("FROMQTY", 0)
                            sq.Add("FROMSTATUS", "")
                            sq.Add("FROMCONTAINER", fromContainer)
                            sq.Add("NOTES", "")
                            sq.Add("SKU", "")
                            sq.Add("TOLOAD", loadid)
                            sq.Add("TOLOC", "")
                            sq.Add("TOWAREHOUSEAREA", "")
                            sq.Add("TOQTY", 0)
                            sq.Add("TOSTATUS", "")
                            sq.Add("TOCONTAINER", fromContainer)
                            sq.Add("USERID", pUser)
                            sq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                            sq.Add("ADDUSER", pUser)
                            sq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                            sq.Add("EDITUSER", pUser)
                            sq.Send(WMS.Lib.Actions.Audit.SHIPMENTMOVED)

                            'write orderlinemoved to audit   
                            Dim oq As EventManagerQ = New EventManagerQ
                            oq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ORDERLINEMOVED)
                            oq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.ORDERLINEMOVED)
                            oq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                            oq.Add("ACTIVITYTIME", "0")
                            oq.Add("CONSIGNEE", Consignee)
                            oq.Add("DOCUMENT", OrdId)
                            oq.Add("DOCUMENTLINE", pOrderLine)
                            oq.Add("FROMLOAD", loadid)
                            oq.Add("FROMLOC", "")
                            oq.Add("FROMWAREHOUSEAREA", "")
                            oq.Add("FROMQTY", 0)
                            oq.Add("FROMSTATUS", "")
                            oq.Add("FROMCONTAINER", fromContainer)
                            oq.Add("NOTES", "")
                            oq.Add("SKU", "")
                            oq.Add("TOLOAD", loadid)
                            oq.Add("TOLOC", "")
                            oq.Add("TOWAREHOUSEAREA", "")
                            oq.Add("TOQTY", 0)
                            oq.Add("TOSTATUS", "")
                        oq.Add("TOCONTAINER", fromContainer)
                            oq.Add("USERID", pUser)
                            oq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                            oq.Add("ADDUSER", pUser)
                            oq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                            oq.Add("EDITUSER", pUser)
                            oq.Send(WMS.Lib.Actions.Audit.ORDERLINEMOVED)
                            Me.ShipmentDetails.Remove(sd)
                        End If
                    Else
                        fromContainer = ""

                    End If
               Next
        End If
        _editdate = DateTime.Now
        _edituser = pUser
        If ShipmentDetails.Count = 0 Then
            _status = WMS.Lib.Statuses.Shipment.STATUSNEW
            DataInterface.RunSQL(String.Format("Update shipment set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            DataInterface.RunSQL(String.Format("Update shipment set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        End If

    End Sub
    Public Shared Function isMultiLoadContainer(ByVal pLoadId As String, ByVal pContainerId As String, ByVal pConsignee As String) As Boolean
        If Not String.IsNullOrEmpty(pContainerId) Then
            Dim Sql As String = String.Format("SELECT count(1) FROM VSHIPMENTLOADS WHERE LOADID<>'{0}' AND HANDLINGUNIT='{1}' AND CONSIGNEE='{2}'", pLoadId, pContainerId, pConsignee)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(Sql))
        Else
            Return False
        End If

    End Function

    ' Public Sub UpdateHandlingUnit(ByVal pShipmentId As String, ByVal pOrderId As String, ByVal pOrderline As Int32, ByVal pConsignee As String, ByVal pContainerId As String)
    Public Sub UpdateHandlingUnit(ByVal pContainerId As String, ByVal pLoadId As String)
        'Dim SQL As String = String.Format("SELECT LOADID FROM VSHIPMENTLOADS WHERE SHIPMENT={0} AND ORDERID={1} AND ORDERLINE={2} AND CONSIGNEE={3}", Made4Net.Shared.Util.FormatField(pShipmentId), Made4Net.Shared.Util.FormatField(pOrderId), Made4Net.Shared.Util.FormatField(pOrderline), Made4Net.Shared.Util.FormatField(pConsignee))
        'Dim dt As New DataTable
        'Dim dr As DataRow

        'DataInterface.FillDataset(SQL, dt)

        'If dt.Rows.Count > 0 Then

        '    Dim oLoad As New WMS.Logic.Load(Convert.ToString(dt.Rows(0).Item("LOADID")))
        '    oLoad.UpdateContainer(pContainerId)

        'End If

        If Not String.IsNullOrEmpty(pContainerId) Then
            Dim oLoad As New WMS.Logic.Load(pLoadId)
            oLoad.UpdateContainer(pContainerId)

        End If

    End Sub

    'End Added for PWMS-810 and RWMS-791   
    'RWMS-791 - Remove Handling Unit from Loads table   
    Public Sub UpdateHandlingUnit(ByVal pShipmentId As String, ByVal pOrderId As String, ByVal pOrderline As Int32, ByVal pConsignee As String)

        Dim SQL As String = String.Format("SELECT LOADID,HANDLINGUNIT FROM VSHIPMENTLOADS WHERE SHIPMENT={0} AND ORDERID={1} AND ORDERLINE={2} AND CONSIGNEE={3}", Made4Net.Shared.Util.FormatField(pShipmentId), Made4Net.Shared.Util.FormatField(pOrderId), Made4Net.Shared.Util.FormatField(pOrderline), Made4Net.Shared.Util.FormatField(pConsignee))
        Dim dt As New DataTable
        Dim dr As DataRow

        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count > 0 Then

            Dim oLoad As New WMS.Logic.Load(Convert.ToString(dt.Rows(0).Item("LOADID")))
            Dim fromContainer As String = Convert.ToString(dt.Rows(0).Item("HANDLINGUNIT"))
            oLoad.UpdateContainer(fromContainer)

        End If

    End Sub
    'END RWMS-791   


#End Region

#Region "Standart Loading Time"

    Private Function CalculateStdtime() As Double
        LoadCalcParameters()
        getEquation()
        Dim StdTime As Double = CalcStdTimeEquation(_sourceStdTimeEquation)
        updateStdTime(StdTime)
    End Function

    Private Function updateStdTime(ByVal pStdTime As Double)
        DataInterface.RunSQL(String.Format("update SHIPMENT set ESTLOADINGTIME='{0}' where shipment='{1}'", Math.Round(pStdTime, 2).ToString(), _shipment))
    End Function

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
        Dim targetEquation As String
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
        Dim ord As OutboundOrderHeader
        'For Each ord In Me.Orders
        '    ord.Complete(pUser)
        'Next

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
            Case WMS.Lib.Statuses.Shipment.SHIPPED, WMS.Lib.Statuses.Shipment.SHIPPING
                Throw New Made4Net.Shared.M4NException(New Exception(), "Can not cancel a shipped shipment.", "Can not cancel a shipped shipment.")
            Case WMS.Lib.Statuses.Shipment.STATUSNEW
                SetStatus(WMS.Lib.Statuses.Shipment.CANCELED, pUser)

                rq.Add("ACTION", action)
                rq.Add("USERID", pUser)
                rq.Add("DATE", DateTime.Now.ToString())
                rq.Add("WAVE", _shipment)
                rq.Send(WMS.Lib.Actions.Audit.CANSHIP)

            Case Else ' WMS.Lib.Statuses.Shipment.ASSIGNED

                'Added code for (RWMS-2261)RWMS-2205 START
                For Each ShipmentPayloads As ShipmentLoad In ShipmentLoads
                    If ShipmentLoads.Count > 0 Then
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Cannot cancel shipment. Shipment needs to be unloaded  ", "Cannot cancel shipment. Shipment needs to be unloaded  ")
                    End If
                Next
                'Added code for (RWMS-2261)RWMS-2205 END
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

    'Public Sub UnLoad(ByVal pLoadID As String, ByVal pLocation As String, ByVal pUser As String)
    '    Dim loadObj As New WMS.Logic.Load(pLoadID)
    '    Me.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
    '    loadObj.UnLoad(pLocation, pUser)

    '    Me.ShipmentLoads.ShipmentLoad(pLoadID).Delete()

    'End Sub

    Public Sub UnLoad(ByVal pLoadID As String, ByVal pLocation As String, ByVal pWarehouseArea As String, ByVal pUser As String)
        If Not WMS.Logic.Location.Exists(pLocation, pWarehouseArea) Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not unload - location does not exist", "Can not unload - location does not exist")
        End If
        Dim dt As New DataTable()
        Dim sql As String = String.Format("select loadid from shipmentloads where shipment={0}", _
        Made4Net.Shared.FormatField(_shipment))

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count = 0 OrElse dt.Select(String.Format("LOADID={0}", Made4Net.Shared.FormatField(pLoadID))).Length = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Load does not belong to shipment", "Load does not belong to shipment")
        End If

        Dim loadObj As New WMS.Logic.Load(pLoadID)
        loadObj.UnLoad(pLocation, pWarehouseArea, pUser)
        If Not String.IsNullOrEmpty(loadObj.ContainerId) Then
            Dim contObj As New WMS.Logic.Container(loadObj.ContainerId, True)
            'RWMS-2343 RWMS-2314 - checking the container Loads.Count >=1 instead of Loads.Count = 1
            If contObj.Loads.Count >= 1 Then
                contObj.Location = pLocation
                contObj.Warehousearea = pWarehouseArea
                If loadObj.ACTIVITYSTATUS.Equals(WMS.Lib.Statuses.ActivityStatus.STAGED, StringComparison.OrdinalIgnoreCase) Then
                    contObj.Status = WMS.Lib.Statuses.Container.STAGED
                Else
                    contObj.Status = WMS.Lib.Statuses.Container.DELIVERED
                End If
                'contObj.Status = loadObj.ACTIVITYSTATUS
                contObj.Save(pUser)
            Else
                loadObj.RemoveFromContainer()
            End If
        End If

        sql = String.Format("delete from shipmentloads where shipment={0} and loadid={1}", Made4Net.Shared.FormatField(_shipment), Made4Net.Shared.FormatField(pLoadID))
        Made4Net.DataAccess.DataInterface.RunSQL(sql)

        If dt.Rows.Count > 1 Then
            'Me.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
            setLoading(pUser)
        Else
            Me.ShipmentLoads.Clear()
            SetAtDock(pUser)
            'Me.SetStatus(WMS.Lib.Statuses.Shipment.ATDOCK, pUser)
        End If
    End Sub

    Public Sub UnloadContainer(ByVal pContainer As String, ByVal pLocation As String, ByVal pWarehouseArea As String, ByVal pUser As String)
        If Not WMS.Logic.Location.Exists(pLocation, pWarehouseArea) Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not unload - location does not exist", "Can not unload - location does not exist")
        End If
        Dim dt As New DataTable()
        Dim sql As String = String.Format("select sl.loadid from shipmentloads sl inner join contloads cl on sl.loadid = cl.loadid where shipment={0} and containerid={1}", _
        Made4Net.Shared.FormatField(_shipment), Made4Net.Shared.FormatField(pContainer))

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "None of the loads on the container belong to the shipment", "None of the loads on the container belong to the shipment")
        End If
        Dim contObj As New WMS.Logic.Container(pContainer, True)
        Dim nextContStatus As String = WMS.Lib.Statuses.Container.DELIVERED
        For Each dr As DataRow In dt.Rows

            Dim loadObj As New WMS.Logic.Load(dr("LOADID").ToString())
            loadObj.UnLoad(pLocation, pWarehouseArea, pUser)
            If loadObj.ACTIVITYSTATUS.Equals(WMS.Lib.Statuses.ActivityStatus.STAGED, StringComparison.OrdinalIgnoreCase) Then
                nextContStatus = WMS.Lib.Statuses.Container.STAGED
            End If
            sql = String.Format("delete from shipmentloads where shipment={0} and loadid={1}", Made4Net.Shared.FormatField(_shipment), Made4Net.Shared.FormatField(loadObj.LOADID))
            Made4Net.DataAccess.DataInterface.RunSQL(sql)
        Next

        contObj.Location = pLocation
        contObj.Warehousearea = pWarehouseArea
        contObj.Status = nextContStatus
        contObj.Save(pUser)

        dt = New DataTable()
        sql = String.Format("select loadid from shipmentloads where shipment={0}", Made4Net.Shared.FormatField(_shipment))

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count > 0 Then
            setLoading(pUser)
        Else
            Me.ShipmentLoads.Clear()
            SetAtDock(pUser)
            'Me.SetStatus(WMS.Lib.Statuses.Shipment.ATDOCK, pUser)
        End If
    End Sub

    Private Sub setLoading(ByVal pUser As String)
        Dim oldStat As String = _status
        Me.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentLoading)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPMENTLOADING)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _shipment)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldStat)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.SHIPMENTLOADING)
    End Sub


#End Region

#Region "Yard"

    Public Sub AssignToYardEntry(ByVal pYardEntryId As String, ByVal pUserId As String)
        If _status = WMS.Lib.Statuses.Shipment.CANCELED Or _status = WMS.Lib.Statuses.Shipment.SHIPPED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Status Incorrect", "Shipment Status Incorrect")
        End If
        If Not YardEntry.Exists(pYardEntryId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Yard Entry does not exists", "Yard Entry does not exists")
        End If
        Dim SQL As String
        _edituser = pUserId
        _editdate = DateTime.Now
        _yardentryid = pYardEntryId
        SQL = String.Format("UPDATE SHIPMENT SET YARDENTRYID ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_yardentryid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
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
        If _status = WMS.Lib.Statuses.Shipment.STATUSNEW OrElse _status = WMS.Lib.Statuses.Shipment.ASSIGNED OrElse _status = WMS.Lib.Statuses.Shipment.SHEDULED OrElse _
        _status = WMS.Lib.Statuses.Shipment.LOADED OrElse _status = WMS.Lib.Statuses.Shipment.LOADING Then
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

    Public Function Ship(ByVal pUser As String)
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
    End Function

    Public Function ShipShipment(ByVal pUser As String, ByVal oLogger As LogHandler)
        If _status <> WMS.Lib.Statuses.Shipment.CANCELED AndAlso _status <> WMS.Lib.Statuses.Shipment.SHIPPED Then
            '-------- Try to create handling unit transaction for this shipment loads
            CreateHuTransactions()
            '-------- Try to ship all outboundorders
            'RWMS-726
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Proceeding to ship all the outbound orders for shipment {0} with status {1}", _shipment, _status))
            ShipLoads(pUser, oLogger)
            'RWMS-726
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Finished shipping all the outbound orders for shipment {0}", _shipment))

            _editdate = DateTime.Now
            _edituser = pUser
            _status = WMS.Lib.Statuses.Shipment.SHIPPED
            _shippeddate = DateTime.Now
            CaseDetail.SetStatusByShipment(_shipment, _status, _edituser)
            Dim sql As String = String.Format("Update Shipment set status={0},shippeddate={1},editdate={2},edituser={3} where {4}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_shippeddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(sql)
            'RWMS-2014 commented Start  
            'Added for RWMS-1293 and RWMS-705 - Modifying the status of the Container to SHIPPED.   

            'Dim SqlLoads As String = String.Format("select Distinct CT.CONTAINER AS CONTAINERID, CT.STATUS AS CONTAINERSTATUS from LOADS LD INNER JOIN SHIPMENTLOADS SHL ON LD.LOADID=SHL.LOADID LEFT OUTER JOIN CONTAINER CT ON CT.CONTAINER=LD.HANDLINGUNIT WHERE  CT.CONTAINER<>'' AND CT.CONTAINER IS NOT NULL  and SHL.SHIPMENT='{0}'", _shipment)
            'Dim dtLoads As New DataTable
            'DataInterface.FillDataset(SqlLoads, dtLoads)

            'If dtLoads.Rows.Count > 0 Then

            '    For Each drLoads As DataRow In dtLoads.Rows

            '        If Not String.IsNullOrEmpty(drLoads("CONTAINERID")) And drLoads("CONTAINERSTATUS") = "LOADED" Then

            '            Dim sqlContainer As String = String.Format("Update CONTAINER set status={0},editdate={1},edituser={2} where CONTAINER={3}", _
            '            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(drLoads("CONTAINERID")))
            '            DataInterface.RunSQL(sqlContainer)

            '        End If
            '    Next
            'End If
            'END for RWMS-1293 and  RWMS-705   
            'RWMS-2014 commented end  

            'RWMS-1710 Added Start
            Dim SqlContainers As String = String.Format("select Distinct CT.CONTAINER AS CONTAINERID, CT.STATUS AS CONTAINERSTATUS from LOADS LD INNER JOIN SHIPMENTLOADS SHL ON LD.LOADID=SHL.LOADID INNER JOIN CONTAINER CT ON CT.CONTAINER=LD.HANDLINGUNIT WHERE SHL.SHIPMENT='{0}'", _shipment)
            Dim dtContainers As New DataTable
            DataInterface.FillDataset(SqlContainers, dtContainers)

            For Each drContainers As DataRow In dtContainers.Rows
                'If Not String.IsNullOrEmpty(drContainers("CONTAINERID")) And drContainers("CONTAINERSTATUS") = "LOADED" Then   
                If Not String.IsNullOrEmpty(drContainers("CONTAINERID")) Then


                    If drContainers("CONTAINERSTATUS") = "LOADED" Then
                        Dim sqlContainer As String = String.Format("Update CONTAINER set status={0},editdate={1},edituser={2} where CONTAINER={3}", _
                    Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(drContainers("CONTAINERID")))
                        DataInterface.RunSQL(sqlContainer)
                    End If
                End If
            Next
            'RWMS-1710 Added End   

            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Updated shipment status ", sql))

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
    End Function

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
                        "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & fh.CONSIGNEE & "','" & fh.FLOWTHROUGH & "','" & fh.ORDERTYPE & "','" & fh.TARGETCOMPANY & "','" & fh.TARGETCOMPANYTYPE & "','" & Cnt.HandlingUnitType & "',1,GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                        DataInterface.RunSQL(strHUTransIns)
                    Else
                        ' It's outboubd order
                        Dim strHUTransIns As String
                        Dim orderid As String = DataInterface.ExecuteScalar("SELECT ORDERID FROM orderloads WHERE  DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                        Dim consignee As String = DataInterface.ExecuteScalar("SELECT CONSIGNEE FROM orderloads WHERE DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                        Dim oo As OutboundOrderHeader = New OutboundOrderHeader 'Orders.getOrder(consignee, orderid) 'New OutboundOrderHeader(consignee, orderid)
                        If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SHIPTO) FROM INVLOAD CNL INNER JOIN ORDERLOADS ODL ON CNL.LOADID=ODL.LOADID INNER JOIN OUTBOUNDORHEADER OH ON ODL.CONSIGNEE=OH.CONSIGNEE AND ODL.ORDERID=OH.ORDERID WHERE CNL.HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") = 1 Then
                            strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                            "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & oo.CONSIGNEE & "','" & oo.ORDERID & "','" & oo.ORDERTYPE & "','" & oo.TARGETCOMPANY & "','" & oo.COMPANYTYPE & "','" & Cnt.HandlingUnitType & "',1,GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                            DataInterface.RunSQL(strHUTransIns)
                        Else
                            strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                            "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(), NULL , NULL, NULL,'" & oo.TARGETCOMPANY & "','" & oo.COMPANYTYPE & "','" & Cnt.HandlingUnitType & "',1,GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
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
                            "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & fh.CONSIGNEE & "','" & fh.FLOWTHROUGH & "','" & fh.ORDERTYPE & "','" & fh.TARGETCOMPANY & "','" & fh.TARGETCOMPANYTYPE & "','" & OnCnt.HandlingUnitType & "',1,GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                            DataInterface.RunSQL(strHUTransIns)
                        Else
                            ' It's outboubd order
                            Dim orderid As String = DataInterface.ExecuteScalar("SELECT ORDERID FROM orderloads WHERE DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                            Dim consignee As String = DataInterface.ExecuteScalar("SELECT CONSIGNEE FROM orderloads WHERE  DOCUMENTTYPE='OUTBOUND' AND LOADID ='" & DataInterface.ExecuteScalar("SELECT TOP 1 LOADID FROM INVLOAD WHERE HANDLINGUNIT='" & ShipmentContainerDr("CONTAINERID") & "'") & "'")
                            Dim oo As OutboundOrderHeader = New OutboundOrderHeader 'Orders.getOrder(consignee, orderid) 'New OutboundOrderHeader(consignee, orderid)
                            Dim strHUTransIns As String = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                            "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','SHIPMENT','" & _shipment & "',GETDATE(),'" & oo.CONSIGNEE & "','" & oo.ORDERID & "','" & oo.ORDERTYPE & "','" & oo.TARGETCOMPANY & "','" & oo.COMPANYTYPE & "','" & OnCnt.HandlingUnitType & "',1,GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                            DataInterface.RunSQL(strHUTransIns)
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        Next
    End Sub

    Private Sub ShipLoads(ByVal pUser As String, ByVal oLogger As LogHandler)
        ' Select all loads
        Dim Sql As String = String.Format("SELECT LOADS.*, CONTAINER.CONTAINER as CONTAINERID, HUTYPE, ORDERID, ORDERLINE, DOCUMENTTYPE, attribute.*, skuuom.netweight,skuuom.volume  FROM SHIPMENTLOADS INNER JOIN LOADS ON SHIPMENTLOADS.LOADID=LOADS.LOADID INNER JOIN ORDERLOADS ON SHIPMENTLOADS.LOADID=ORDERLOADS.LOADID left outer JOIN CONTAINER ON dbo.CONTAINER.CONTAINER = loads.HANDLINGUNIT left outer join attribute on loads.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' left outer join skuuom on loads.consignee = skuuom.consignee and loads.sku = skuuom.sku and (skuuom.loweruom = '' or skuuom.loweruom is null) WHERE SHIPMENT = '{0}'", _shipment)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)

        'RWMS-726
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Fetching all the loads of shipment {0}", Sql))
            oLogger.writeSeperator(" ", 100)
        End If


        ' Select all load attributes
        Sql = String.Format("SELECT attribute.* FROM SHIPMENTLOADS left outer join attribute on SHIPMENTLOADS.loadid = attribute.pkey1 and attribute.pkeytype = 'LOAD' WHERE SHIPMENT = '{0}'", _shipment)
        Dim dtAtt As New DataTable
        DataInterface.FillDataset(Sql, dtAtt)

        'RWMS-726
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Fetching all the load attributes of shipment {0}", Sql))
            oLogger.writeSeperator(" ", 100)
        End If


        Dim docType As String
        For Each dr As DataRow In dt.Rows
            Try
                docType = dr("DOCUMENTTYPE")
                Dim loadatt() As DataRow
                loadatt = dtAtt.Select(String.Format("pkey1 = '{0}'", dr("loadid")))

                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("LoadAttribute {0}", loadatt.Length.ToString()))
                    oLogger.writeSeperator(" ", 100)
                End If
                Dim dratt As DataRow = Nothing
                If (loadatt.Length > 0) Then

                    If Not oLogger Is Nothing Then
                        oLogger.Write(String.Format("Load attribute of Loadid  {0}", loadatt(0).Item("PKEY1").ToString()))
                        oLogger.writeSeperator(" ", 100)
                    End If
                    dratt = loadatt(0)
                End If
                Dim oLoad As New WMS.Logic.Load(dr, dratt)
                Select Case docType
                    Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH

                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Select Case Flowthrough statement {0}", docType.ToString()))
                            oLogger.writeSeperator(" ", 100)
                        End If
                        'oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE"))) ', Nothing, Nothing, Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")).LINES.Line(dr("ORDERLINE")), Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")))
                        'oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE")), Nothing, Nothing, Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")).LINES.Line(dr("ORDERLINE")), Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")), docType)
                        Dim ft As New Flowthrough(dr("CONSIGNEE"), dr("ORDERID"))
                        oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE")), Nothing, Nothing, ft.LINES.Line(dr("ORDERLINE")), ft, docType, oLogger)
                    Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT

                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Select Case TRANSHIPMENT statement {0}", docType.ToString()))
                            oLogger.writeSeperator(" ", 100)
                        End If
                        oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE")), Nothing, Nothing, Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")).LINES.Line(dr("ORDERLINE")), Flowthroughs.getOrder(dr("CONSIGNEE"), dr("ORDERID")), docType, oLogger)
                    Case Else

                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Select Case Else statement {0}", docType.ToString()))
                            oLogger.writeSeperator(" ", 100)
                        End If
                        Dim ord As New OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        Dim orl As OutboundOrderHeader.OutboundOrderDetail = ord.Lines.GetLine(dr("ORDERLINE"))

                        'oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE"))) ', Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID")).Lines.GetLine(dr("ORDERLINE")), Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID")))
                        'oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE")), Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID")).Lines.GetLine(dr("ORDERLINE")), Orders.getOrder(dr("CONSIGNEE"), dr("ORDERID")), Nothing, Nothing, docType, oLogger)
                        oLoad.Ship(pUser, Convert.ToString(dr("ORDERID")), Convert.ToInt32(dr("ORDERLINE")), orl, ord, Nothing, Nothing, docType, oLogger)
                End Select
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Message {0}", ex.Message.ToString()))
                    oLogger.Write(String.Format("Loadid: {0}", dr("loadid")))
                    oLogger.writeSeperator(" ", 100)
                End If
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

    Public Function PrintLoadingWorksheet(ByVal Language As Int32, ByVal pUser As String)
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
    End Function

    Public Function PrintShippingManifest(ByVal Language As Int32, ByVal pUser As String)
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
    End Function

    Public Function PrintShipmentPackingList(ByVal Language As Int32, ByVal pUser As String)
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
    End Function
    Public Function PrintLoadingDiagram(ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "ShipMan"
        Dim Copies As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "repLoadingDiagram", "Copies"))
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "repLoadingDiagram")
        Dim setId As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "repLoadingDiagram", "DataSetName"))
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
    End Function

#End Region

#End Region

End Class

#End Region

