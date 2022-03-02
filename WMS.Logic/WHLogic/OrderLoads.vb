Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class OrderLoads

#Region "Variables"

    Protected _documenttype As String
    Protected _consignee As String
    Protected _orderid As String
    Protected _loadid As String
    Protected _orderline As Int32
    Protected _picklist As String
    Protected _picklistline As Int32
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " DOCUMENTTYPE='" & _documenttype & "' AND  CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "' And LOADID = '" & _loadid & "'"
        End Get
    End Property

    Public Property DOCUMENTTYPE() As String
        Get
            Return _documenttype
        End Get
        Set(ByVal Value As String)
            _documenttype = Value
        End Set
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property ORDERID() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
        End Set
    End Property

    Public Property ORDERLINE() As Int32
        Get
            Return _orderline
        End Get
        Set(ByVal Value As Int32)
            _orderline = Value
        End Set
    End Property

    Public Property PICKLIST() As String
        Get
            Return _picklist
        End Get
        Set(ByVal Value As String)
            _picklist = Value
        End Set
    End Property

    Public Property PICKLISTLINE() As Int32
        Get
            Return _picklistline
        End Get
        Set(ByVal Value As Int32)
            _picklistline = Value
        End Set
    End Property

    Public Property LOADID() As String
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

    Public Sub New(ByVal pDocumentType As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pLoadid As String, Optional ByVal pLoadAll As Boolean = True)
        _documenttype = pDocumentType
        _consignee = pConsignee
        _orderid = pOrderid
        _loadid = pLoadid
        If pLoadAll Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        If Not dr.IsNull("DOCUMENTTYPE") Then _documenttype = dr.Item("DOCUMENTTYPE")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("LOADID") Then _loadid = dr.Item("LOADID")
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PICKLISTLINE") Then _picklistline = dr.Item("PICKLISTLINE")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal pDocumentType As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLoadid As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM ORDERLOADS WHERE DOCUMENTTYPE='{3}' AND CONSIGNEE = '{0}' AND ORDERID = '{1}' AND LOADID = '{2}'", pConsignee, pOrderId, pLoadid, pDocumentType)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function LoadExists(ByVal documentType As String, ByVal loadID As String) As Boolean
        Dim sql As String = String.Format($"SELECT COUNT(1) FROM ORDERLOADS WHERE DOCUMENTTYPE='{documentType}' AND LOADID = '{loadID}'")
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetOutboundOrderLoad(ByVal pDocumentType As String, ByVal pCONSIGNEE As String, ByVal pORDERID As String, ByVal pLoadid As String) As OrderLoads
        Return New OrderLoads(pDocumentType, pCONSIGNEE, pORDERID, pLoadid)
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM ORDERLOADS WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PICKLISTLINE") Then _picklistline = dr.Item("PICKLISTLINE")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub


#End Region

#Region "Create / Update"

    Private Sub Create(ByVal pDocumentType As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLoadid As String, ByVal pOrderLine As Int32, ByVal pPicklist As String, ByVal pPickLine As Int32, ByVal pUser As String)
        _adddate = DateTime.Now
        _adduser = pUser
        _edituser = pUser
        _editdate = DateTime.Now
        If Not WMS.Logic.Consignee.Exists(pConsignee) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Consignee does not exists", "Consignee does not exists")
        End If
        'If Not WMS.Logic.OutboundOrderHeader.OutboundOrderDetail.Exists(pConsignee, pOrderId, pOrderLine) Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Order Line does not exists", "Order Line does not exists")
        'End If
        If Not WMS.Logic.Load.Exists(pLoadid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
        End If
        'If Not WMS.Logic.PicklistDetail.Exists(pPicklist, pPickLine) Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Pick Line does not exists", "Pick Line does not exists")
        'End If
        _documenttype = pDocumentType
        _consignee = pConsignee
        _orderid = pOrderId
        _orderline = pOrderLine
        _loadid = pLoadid
        _picklist = pPicklist
        _picklistline = pPickLine
        Dim SQL As String = String.Format("INSERT INTO ORDERLOADS (DOCUMENTTYPE, CONSIGNEE, ORDERID, LOADID, ORDERLINE, PICKLIST, PICKLISTLINE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_loadid), _
                Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)
    End Sub

    Private Sub Update(ByVal pOrderLine As Int32, ByVal pPicklist As String, ByVal pPickLine As Int32, ByVal pUser As String)
        _edituser = pUser
        _editdate = DateTime.Now
        'If Not WMS.Logic.OutboundOrderHeader.OutboundOrderDetail.Exists(_consignee, _orderid, pOrderLine) Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Order Line does not exists", "Order Line does not exists")
        'End If
        'If Not WMS.Logic.PicklistDetail.Exists(pPicklist, pPickLine) Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Pick Line does not exists", "Pick Line does not exists")
        'End If
        _picklist = pPicklist
        _picklistline = pPickLine
        Dim SQL As String = String.Format("UPDATE ORDERLOADS SET ORDERLINE ={0}, PICKLIST ={1}, PICKLISTLINE ={2}, EDITDATE ={3}, EDITUSER ={4} where {5}", _
                Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Private Sub Delete()
        Dim SQL As String = String.Format("DELETE FROM ORDERLOADS WHERE {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Create Load / Delete Load"

    Public Function CreateOrderLoad(ByVal pDocumentType As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pLoadid As String, ByVal pPicklist As String, ByVal pPickLine As Int32, ByVal pUser As String) As OrderLoads
        Create(pDocumentType, pConsignee, pOrderId, pLoadid, pOrderLine, pPicklist, pPickLine, pUser)
        Return Me
    End Function

    Public Sub DeleteOrderLoad(ByVal pDocumentType As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLoadid As String, ByVal pUser As String)
        _documenttype = pDocumentType
        _consignee = pConsignee
        _orderid = pOrderId
        _loadid = pLoadid
        Delete()
    End Sub

#End Region

#Region "Undo Pick"

    Public Sub UndoPick(ByVal pUser As String)
        Dim oLoad As WMS.Logic.Load
        If Not WMS.Logic.Load.Exists(_loadid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Unpick, Invalid Load", "Cannot Unpick, Invalid Load")
        Else
            oLoad = New Load(_loadid)
        End If
        If oLoad.LOCATION = String.Empty Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Unpick Load. Load must have a location", "Cannot Unpick Load. Load must have a location")
        End If
        oLoad.SetActivityStatus("", pUser)
        If String.IsNullOrEmpty(oLoad.STATUS) Then
            oLoad.setStatus(WMS.Lib.Statuses.LoadStatus.AVAILABLE, "", pUser)
        End If
        If oLoad.ContainerId <> String.Empty Then
            oLoad.RemoveFromContainer()
        End If
        'Fill the message
        Dim em As EventManagerQ = New EventManagerQ
        em.Add("EVENT", WMS.Logic.WMSEvents.EventType.UnpickLoad)
        em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNPICK)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ACTIVITYTIME", "0")
        em.Add("CONSIGNEE", _consignee)
        em.Add("DOCUMENT", _orderid)
        em.Add("DOCUMENTLINE", _orderline)
        em.Add("FROMLOAD", _loadid)
        em.Add("FROMLOC", oLoad.LOCATION)
        em.Add("FROMQTY", oLoad.UNITS)
        em.Add("FROMSTATUS", oLoad.STATUS)
        em.Add("NOTES", "")
        em.Add("SKU", oLoad.SKU)
        em.Add("TOLOAD", _loadid)
        em.Add("TOLOC", oLoad.LOCATION)
        em.Add("TOQTY", oLoad.UNITS)
        em.Add("TOSTATUS", oLoad.STATUS)
        em.Add("USERID", pUser)
        em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ADDUSER", pUser)
        em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("EDITUSER", pUser)
        'Delete the order load line from table
        Delete()
        'And send the message
        em.Send(WMS.Lib.Actions.Audit.UNPICK)
    End Sub

#End Region

#Region "Pack / UnPack"

    '    Public Sub Pack1(ByVal pToContainerId As String, ByVal pUnits As Decimal, ByVal pUser As String)
    '        Dim origLoad As New WMS.Logic.Load(_loadid)
    '        Dim tempLoad As New WMS.Logic.Load(_loadid)
    '        If pUnits = 0 Then
    '            Throw New Made4Net.Shared.M4NException(New Exception, "Packing Units must be greater than zero", "Packing Units must be greater than zero")
    '        ElseIf origLoad.UNITS < pUnits Then
    '            Throw New Made4Net.Shared.M4NException(New Exception, "Packing Units cannot exceed Load units", "Packing Units cannot exceed Load units")
    '        End If
    '        Dim oCont As Container
    '        If WMS.Logic.Container.Exists(pToContainerId) Then
    '            oCont = New Container(pToContainerId, True)
    '        Else
    '            oCont = New Container
    '            Dim oPickDet As New WMS.Logic.PicklistDetail(_picklist, _picklistline)
    '            If WMS.Logic.Container.Exists(oPickDet.ToContainer) Then 'Handles the Full Picks
    '                Dim origCont As New WMS.Logic.Container(oPickDet.ToContainer, True)
    '                oCont.HandlingUnitType = origCont.HandlingUnitType
    '                oCont.Location = origCont.Location
    '                oCont.DestinationLocation = origCont.DestinationLocation
    '            End If
    '            oCont.UsageType = WMS.Logic.Container.ContainerUsageType.ShippingContainer
    '            oCont.ContainerId = pToContainerId
    '            oCont.Post(pUser)
    '        End If
    '        If origLoad.UNITS = pUnits Then
    '            oCont.Place(origLoad, pUser)
    '        Else
    '            origLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, pUser)
    '            origLoad.Split(origLoad.LOCATION, pUnits, "", pUser)
    '            origLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.PICKED, pUser)
    '            tempLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.PICKED, pUser)
    '            SplitPackLoad(origLoad.LOADID, pUser)
    '            oCont.Place(origLoad, pUser)
    '        End If
    '    End Sub

    '    Private Sub SplitPackLoad(ByVal pNewLoadid As String, ByVal pUser As String)
    '        Dim oOrderLoad As New OrderLoads
    '        oOrderLoad.Create(_documenttype, _consignee, _orderid, pNewLoadid, _orderline, _picklist, _picklistline, pUser)
    '        Me.Update(_orderline, _picklist, _picklistline, pUser)
    '    End Sub

    '    Public Sub UnPack1(ByVal pContId As String, ByVal pUser As String)
    '        Dim oLoad As New WMS.Logic.Load(_loadid)
    '        oLoad.RemoveFromContainer()
    '        Dim oPickDet As New WMS.Logic.PicklistDetail(_picklist, _picklistline)
    '        Dim origCont As New WMS.Logic.Container(oPickDet.ToContainer, True)
    '        origCont.Place(oLoad, pUser)
    '    End Sub

#End Region

#End Region

End Class
