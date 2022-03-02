Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteStopTaskItems

#Region "Variables"

    Protected _routeid As String
    Protected _stopnumber As Int32
    Protected _stoptaskid As Int32
    Protected _item As String
    Protected _itemdesc As String
    Protected _qty As Decimal
    Protected _qtyconfirmed As Decimal
    Protected _uom As String
    Protected _barcode As String
    Protected _reasoncode As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String


#End Region

#Region "Properties"

    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" routeid = {0} and stopnumber = {1} and stoptaskid = {2} and item = {3} and uom= {4}", Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(_item), Made4Net.Shared.Util.FormatField(_uom))
        End Get
    End Property

    Public Property RouteID() As String
        Set(ByVal Value As String)
            _routeid = Value
        End Set
        Get
            Return _routeid
        End Get
    End Property

    Public Property StopNumber() As Int32
        Get
            Return _stopnumber
        End Get
        Set(ByVal Value As Int32)
            _stopnumber = Value
        End Set
    End Property

    Public Property StopTaskId() As Int32
        Get
            Return _stoptaskid
        End Get
        Set(ByVal Value As Int32)
            _stoptaskid = Value
        End Set
    End Property

    Public Property Item() As String
        Get
            Return _item
        End Get
        Set(ByVal Value As String)
            _item = Value
        End Set
    End Property
    Public Property ItemDesc() As String
        Get
            Return _itemdesc
        End Get
        Set(ByVal Value As String)
            _itemdesc = Value
        End Set
    End Property
    
    Public Property QTY() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal Value As Decimal)
            _qty = Value
        End Set
    End Property
    Public Property QTYConfirmed() As Decimal
        Get
            Return _qtyconfirmed
        End Get
        Set(ByVal Value As Decimal)
            _qtyconfirmed = Value
        End Set
    End Property
    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property
    Public Property Barcode() As String
        Get
            Return _barcode
        End Get
        Set(ByVal Value As String)
            _barcode = Value
        End Set
    End Property
    Public Property ReasonCode() As String
        Get
            Return _reasoncode
        End Get
        Set(ByVal Value As String)
            _reasoncode = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pRouteId As String, ByVal pStopnumber As Int32, ByVal pStopTaskId As Int32, ByVal pItem As String, ByVal pUom As String)
        _routeid = pRouteId
        _stopnumber = pStopnumber
        _stoptaskid = pStopTaskId
        _item = pItem
        _uom = pUom
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal sRouteId As String, ByVal iStopNumber As Int32, ByVal iStopTaskId As Int32, ByVal sItem As String, ByVal sUom As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from routestoptaskitems where routeid = {0} and stopnumber = {1} and stoptaskid = {2} and item = {3} and uom={4}", Made4Net.Shared.Util.FormatField(sRouteId), Made4Net.Shared.Util.FormatField(iStopNumber), Made4Net.Shared.Util.FormatField(iStopTaskId), Made4Net.Shared.Util.FormatField(sItem), Made4Net.Shared.Util.FormatField(sUom))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim sql As String = String.Format("SELECT * FROM routestoptaskitems where " & WhereClause)
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Item Not Found")
        End If
        dr = dt.Rows(0)
        If Not IsDBNull(dr("qty")) Then _qty = dr("qty")
        If Not IsDBNull(dr("qtyconfirmed")) Then _qtyconfirmed = dr("qtyconfirmed")
        If Not IsDBNull(dr("itemdesc")) Then _itemdesc = dr("itemdesc")
        If Not IsDBNull(dr("barcode")) Then _barcode = dr("barcode")
        If Not IsDBNull(dr("reasoncode")) Then _reasoncode = dr("reasoncode")
        If Not IsDBNull(dr("adddate")) Then _adddate = dr("adddate")
        If Not IsDBNull(dr("adduser")) Then _adduser = dr("adduser")
        If Not IsDBNull(dr("editdate")) Then _editdate = dr("editdate")
        If Not IsDBNull(dr("edituser")) Then _edituser = dr("edituser")
    End Sub

#End Region

#Region "Create & Delete"

    Public Sub Create(ByVal pRouteId As String, ByVal pStopNumber As Int32, ByVal pStopTaskId As Int32, ByVal pItem As String, ByVal pItemDescription As String, ByVal pUom As String, _
             ByVal pQty As Decimal, ByVal pQtyConfirmed As Decimal, ByVal pBarcode As String, ByVal pReasonCode As String, ByVal pUserId As String)

        If RouteStopTaskItems.Exists(pRouteId, pStopNumber, pStopTaskId, pItem, pUom) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Stop Task Item - Route Stop Item already exists", "Cannot create Stop Task Item - Route Stop Item already exists")
        End If
        If Not WMS.Logic.RouteStopTask.Exists(pRouteId, pStopNumber, pStopTaskId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Stop Task Item - Route Stop task does not exist", "Cannot create Stop Task Item - Route Stop task does not exist")
        End If
        Dim oRoute As New WMS.Logic.Route(pRouteId)
        If oRoute.Status >= RouteStatus.Departed Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create item - Route status incorrect", "Cannot create item - Route status incorrect")
        End If
        If oRoute.Stops.GetStop(pStopNumber).RouteStopTask.GetStopDetail(pStopTaskId).StopTaskType <> StopTaskType.Delivery Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create item - Route stop task type incorrect", "Cannot create item - Route stop task type incorrect")
        End If

        Dim sSql As String
        _routeid = pRouteId
        _stopnumber = pStopNumber
        _stoptaskid = pStopTaskId
        _item = pItem
        _itemdesc = pItemDescription
        _uom = pUom
        _qty = pQty
        _qtyconfirmed = pQtyConfirmed
        _barcode = pBarcode
        _reasoncode = pReasonCode
        _adddate = DateTime.Now
        _adduser = pUserId
        _editdate = DateTime.Now
        _edituser = pUserId

        sSql = String.Format("INSERT INTO ROUTESTOPTASKITEMS(ROUTEID, STOPNUMBER, STOPTASKID, ITEM, ITEMDESC, QTY, QTYCONFIRMED, UOM, BARCODE, REASONCODE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_stoptaskid), _
            Made4Net.Shared.Util.FormatField(_item), Made4Net.Shared.Util.FormatField(_itemdesc), Made4Net.Shared.Util.FormatField(_qty), _
            Made4Net.Shared.Util.FormatField(_qtyconfirmed), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_barcode), _
            Made4Net.Shared.Util.FormatField(_reasoncode), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(sSql)
    End Sub

    Public Sub Update(ByVal pItemDescription As String, ByVal pQty As Decimal, ByVal pQtyConfirmed As Decimal, ByVal pBarcode As String, ByVal pReasonCode As String, ByVal pUserId As String)
        If Not RouteStopTaskItems.Exists(_routeid, _stopnumber, _stoptaskid, _item, _uom) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot update stop task item - item does not exist", "Cannot update stop task item - item does not exist")
        End If

        Dim oRoute As New WMS.Logic.Route(_routeid)
        If oRoute.Status >= RouteStatus.Departed Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add item to route", "Cannot add item to route - Route status incorrect")
        End If

        Dim sSql As String
        _itemdesc = pItemDescription
        _qty = pQty
        _qtyconfirmed = pQtyConfirmed
        _barcode = pBarcode
        _reasoncode = pReasonCode
        _editdate = DateTime.Now
        _edituser = pUserId

        sSql = String.Format("UPDATE ROUTESTOPTASKITEMS SET QTY = {0}, QTYCONFIRMED ={1}, BARCODE ={2}, REASONCODE ={3}, EDITDATE ={4}, EDITUSER ={5}, ITEMDESC ={6} where {7}", _
            Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_qtyconfirmed), Made4Net.Shared.Util.FormatField(_barcode), Made4Net.Shared.Util.FormatField(_reasoncode), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_itemdesc), WhereClause)
        DataInterface.RunSQL(sSql)
    End Sub

    Public Sub Save(ByVal pUserId As String)
        Dim sql As String
        If RouteStopTaskItems.Exists(_routeid, _stopnumber, _stoptaskid, _item, _uom) Then
            Update(_itemdesc, _qty, _qtyconfirmed, _barcode, _reasoncode, pUserId)
        Else
            Create(_routeid, _stopnumber, _stoptaskid, _item, _itemdesc, _uom, _qty, _qtyconfirmed, _barcode, _reasoncode, pUserId)
        End If
    End Sub

    Public Sub Delete(ByVal pUserId As String)
        'And delete the stop item object
        Dim SQL As String = String.Format("delete from ROUTESTOPTASKITEMS where {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Quantity Confirmation"

    Public Sub UpdateConfirmedQuantity(ByVal pQtyConfirmed As Decimal, ByVal pReasonCode As String, ByVal pUserId As String)
        _qtyconfirmed = pQtyConfirmed
        _reasoncode = pReasonCode
        _editdate = DateTime.Now
        _edituser = pUserId

        Dim sSql As String = String.Format("update ROUTESTOPTASKITEMS set QTYCONFIRMED={0}, REASONCODE={1}, EDITDATE={2}, EDITUSER={3} where {4}", _
            Made4Net.Shared.Util.FormatField(_qtyconfirmed), Made4Net.Shared.Util.FormatField(_reasoncode), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sSql)
    End Sub

#End Region

#End Region

End Class
