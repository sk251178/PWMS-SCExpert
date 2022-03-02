Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports Made4Net.Shared
#Region "ASNCONTAINER"

<CLSCompliant(False)> Public Class AsnContainer

#Region "Variables"

    Protected _container As String
    Protected _receipt As String
    Protected _asnloads As ArrayList

#End Region

#Region "Properties"

    Public ReadOnly Property ContainerId() As String
        Get
            Return _container
        End Get
    End Property

    Public ReadOnly Property NumLoads() As Int32
        Get
            Return _asnloads.Count
        End Get
    End Property

    Public ReadOnly Property Receipt() As String
        Get
            Return _receipt
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal index As Int32) As AsnDetail
        Get
            Return CType(_asnloads.Item(index), AsnDetail)
            _asnloads.GetEnumerator()
        End Get
    End Property

    Protected Function GetEnumerator() As IEnumerator
        Return _asnloads.GetEnumerator()
    End Function

#End Region

#Region "Methods"

    Public Sub New(ByVal pContainerId As String, Optional ByVal pReceiptId As String = Nothing)
        Dim sql As String
        Dim cntRec As Int32
        _container = pContainerId
        _receipt = pReceiptId
        _asnloads = New ArrayList
        If pReceiptId Is Nothing Then
            sql = String.Format("Select count(distinct receipt) from ASNDETAIL where CONTAINER = '{0}'", _container)
            cntRec = DataInterface.ExecuteScalar(sql)
            If cntRec > 1 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "More then one Asn Container", "More then one Asn Container")
            End If
            sql = String.Format("select * from ASNDETAIL where CONTAINER = '{0}'", pContainerId)
        Else
            sql = String.Format("select * from ASNDETAIL where CONTAINER = '{0}' and RECEIPT = '{1}'", pContainerId, pReceiptId)
        End If
        Dim dt As New DataTable(sql)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Asn Container Does Not Exists", "Asn Container Does Not Exists")
        End If
        _receipt = dt.Rows(0)("RECEIPT")

        Dim dr As DataRow
        For Each dr In dt.Rows
            _asnloads.Add(New AsnDetail(dr))
        Next
        dt.Dispose()
    End Sub

    Public Sub Receive(ByVal pContainerType As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String)
        Dim asndet As AsnDetail
        Dim ld As Load
        If _asnloads.Count = 1 Then
            asndet = _asnloads.Item(0)
            Dim Rec As New ReceiptHeader(asndet.Receipt)
            ld = Rec.LINES.Line(asndet.ReceiptLine).CreateLoad(Made4Net.Shared.Util.getNextCounter("LOAD"), asndet.UOM, pLocation, pWarehousearea, asndet.Units, asndet.Status, Nothing, Nothing, pUser, Nothing)
        Else
            Dim cntr As New Container
            cntr.ContainerId = Made4Net.Shared.Util.getNextCounter("CONTAINER")
            cntr.HandlingUnitType = pContainerType
            cntr.UsageType = Container.ContainerUsageType.PutawayContainer
            cntr.Post(pUser)
            For Each asndet In Me._asnloads
                Dim RecLine As New ReceiptDetail(asndet.Receipt, asndet.ReceiptLine)
                ld = RecLine.CreateLoad(Made4Net.Shared.Util.getNextCounter("LOAD"), asndet.UOM, pLocation, pWarehousearea, asndet.Units, asndet.Status, Nothing, Nothing, pUser, Nothing)
                cntr.Place(ld, pUser)
            Next
        End If
    End Sub

    Public Shared Function getUniqueContainer(ByVal pRec As String) As String
        Dim sql As String
        Dim dt As New DataTable
        Dim cntCont As Int32
        sql = String.Format("Select distinct container from ASNDETAIL where receipt = '{0}'", pRec)
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 1 Then
            Return dt.Rows(0).Item("container")
        Else
            Return String.Empty
        End If
    End Function

#End Region

End Class

#End Region

#Region "ASNDETAIL"

' <summary>
' This object represents the properties and methods of a ASN Detail.
' </summary>

<CLSCompliant(False)> Public Class AsnDetail

#Region "Variables"

#Region "Primary Keys"

    Protected _asnid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _receipt As String = String.Empty
    Protected _receiptline As Int32
    Protected _container As String
    Protected _uom As String = String.Empty
    Protected _units As Decimal
    Protected _status As String
    Protected _loadid As String
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

    Protected _asnattributes As InventoryAttributeBase

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" ASNID = '{0}'", _asnid)
        End Get
    End Property

    Public Property AsnId() As String
        Get
            Return _asnid
        End Get
        Set(ByVal Value As String)
            _asnid = Value
        End Set
    End Property

    Public Property Receipt() As String
        Get
            Return _receipt
        End Get
        Set(ByVal Value As String)
            _receipt = Value
        End Set
    End Property

    Public Property ReceiptLine() As Int32
        Get
            Return _receiptline
        End Get
        Set(ByVal Value As Int32)
            _receiptline = Value
        End Set
    End Property

    Public Property Container() As String
        Get
            Return _container
        End Get
        Set(ByVal Value As String)
            _container = Value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
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

    Public Property Units() As Decimal
        Get
            Return _units
        End Get
        Set(ByVal Value As Decimal)
            _units = Value
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

    Public ReadOnly Property ASNAttributes() As InventoryAttributeBase
        Get
            Return _asnattributes
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pAsnId As String)
        _asnid = pAsnId

        Dim SQL As String = "SELECT * FROM ASNDETAIL Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "ASNDetail does not exists", "ASNDetail does not exists")
        End If

        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "ReceiveASNLoad" Then
            For Each dr In ds.Tables(0).Rows
                _asnid = dr("ASNID")
                Load()
                ReceiveByLoadId("", "", Common.GetCurrentUser())
            Next
            'RWMS-1345 - Printing ASN Labels
        ElseIf CommandName.ToLower = "printlabels" Then
            For Each dr In ds.Tables(0).Rows
                PrintLabel(dr("ASNID"), "")
            Next
            Message = "Labels sent to printing service"
        End If
        'End RWMS-1345
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal pASNID As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM ASNDETAIL WHERE ASNID = '{0}'", pASNID)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("ASNID") Then _asnid = dr.Item("ASNID")
        If Not dr.IsNull("RECEIPT") Then _receipt = dr.Item("RECEIPT")
        If Not dr.IsNull("RECEIPTLINE") Then _receiptline = dr.Item("RECEIPTLINE")
        If Not dr.IsNull("CONTAINER") Then _container = dr.Item("CONTAINER")
        If Not dr.IsNull("UOM") Then _uom = dr.Item("UOM")
        If Not dr.IsNull("UNITS") Then _units = dr.Item("UNITS")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("LOADID") Then _loadid = dr.Item("LOADID")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _asnattributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.ASN, _asnid)

    End Sub

    Protected Sub Load()
        Dim SQL As String = "Select * from ASNDETAIL where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "ASN does not exists", "ASN does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("ASNID") Then _asnid = dr.Item("ASNID")
        If Not dr.IsNull("RECEIPT") Then _receipt = dr.Item("RECEIPT")
        If Not dr.IsNull("RECEIPTLINE") Then _receiptline = dr.Item("RECEIPTLINE")
        If Not dr.IsNull("CONTAINER") Then _container = dr.Item("CONTAINER")
        If Not dr.IsNull("UOM") Then _uom = dr.Item("UOM")
        If Not dr.IsNull("UNITS") Then _units = dr.Item("UNITS")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("LOADID") Then _loadid = dr.Item("LOADID")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _asnattributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.ASN, _asnid)
    End Sub

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
        _status = pStatus
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String
        SQL = String.Format("Update ASNDETAIL set status={0},edituser={1},editdate={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)

        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Create"

    Public Sub Save()
        Dim SQL As String
        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String
        If Exists(_asnid) Then
            SQL = String.Format("Update ANSDETAIL set UOM = {0}, UNITS = {1}, STATUS = {2},LOADID = {3}, editdate = {4}, edituser = {5} where {6}", _
                 Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ASNDetailUpdated
            'em.Add("EVENT", EventType)
            'em.Add("ASN", _asnid)
            'em.Add("USERID", _adduser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ASNDetailUpdated)
            activitytype = WMS.Lib.Actions.Audit.ASNUPD
        Else
            _asnid = Made4Net.Shared.Util.getNextCounter("ASN")
            SQL = String.Format("INSERT INTO ASNDETAIL(ASNID,RECEIPT,RECEIPTLINE,CONTAINER,UOM,UNITS,STATUS,LOADID,ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _
               Made4Net.Shared.Util.FormatField(_asnid), Made4Net.Shared.Util.FormatField(_receipt), Made4Net.Shared.Util.FormatField(_receiptline), Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_units), _
               Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

            activitytype = WMS.Lib.Actions.Audit.ASNINS
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ASNDetailCreated)
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ASNDetailCreated
            'em.Add("EVENT", EventType)
            'em.Add("ASN", _asnid)
            'em.Add("USERID", _adduser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
        End If

        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _asnid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(activitytype)

        DataInterface.RunSQL(SQL)

        If Not _asnattributes Is Nothing Then
            _asnattributes.Save(_edituser)
        End If
    End Sub

    Public Sub Create(ByVal pAsnId As String, ByVal pReceipt As String, ByVal pReceiptLine As Int32, ByVal pContainer As String, ByVal pUOM As String, ByVal pUnits As Decimal, ByVal pLoadid As String, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String)
        If pAsnId = "" Then
            _asnid = Made4Net.Shared.Util.getNextCounter("ASN")
        Else
            _asnid = pAsnId
        End If
        _receipt = pReceipt
        _receiptline = pReceiptLine
        _container = pContainer
        _uom = pUOM
        _units = pUnits
        _loadid = pLoadid
        _status = WMS.Lib.Statuses.ASN.EXPECTED
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String
        SQL = String.Format("INSERT INTO ASNDETAIL(ASNID,RECEIPT,RECEIPTLINE,CONTAINER,UOM,UNITS,STATUS,LOADID,ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11})", _
               Made4Net.Shared.Util.FormatField(_asnid), Made4Net.Shared.Util.FormatField(_receipt), Made4Net.Shared.Util.FormatField(_receiptline), Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_units), _
               Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(SQL)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ASNDetailCreated
        'em.Add("EVENT", EventType)
        'em.Add("ASN", _asnid)
        'em.Add("USERID", _adduser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))

        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String
        activitytype = WMS.Lib.Actions.Audit.ASNINS
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ASNDetailCreated)

        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _asnid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(activitytype)

        'Create the ASN attributes
        'If Not oAttributeCollection Is Nothing Then
        '    For idx As Int32 = 0 To oAttributeCollection.Count - 1
        '        _asnattributes.Attribute(oAttributeCollection.Keys(idx)) = oAttributeCollection(idx)
        '    Next
        '    _asnattributes.Save(pUser)
        'End If
        _asnattributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.ASN, _asnid)
        If Not oAttributeCollection Is Nothing Then
            setAttributes(oAttributeCollection, pUser)
        End If

        Load()
    End Sub

    Public Sub Edit(ByVal pUOM As String, ByVal pUnits As Decimal, ByVal pLoadid As String, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String)
        _uom = pUOM
        _units = pUnits
        _loadid = pLoadid
        _edituser = pUser
        _editdate = DateTime.Now

        Dim SQL As String
        SQL = String.Format("Update ASNDETAIL set UOM = {0}, UNITS = {1}, STATUS = {2},LOADID = {3}, editdate = {4}, edituser = {5} where {6}", _
             Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(SQL)

        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String
        activitytype = WMS.Lib.Actions.Audit.ASNUPD
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ASNDetailUpdated)

        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _asnid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(activitytype)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ASNDetailUpdated
        'em.Add("EVENT", EventType)
        'em.Add("ASN", _asnid)
        'em.Add("USERID", _adduser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))

        If Not oAttributeCollection Is Nothing Then
            setAttributes(oAttributeCollection, pUser)
        End If
    End Sub

    Public Sub setAttributes(ByVal oAttributesCollection As AttributesCollection, ByVal sUserId As String)
        _asnattributes.Attributes.Clear()
        _asnattributes.Add(oAttributesCollection)
        _asnattributes.Save(sUserId)
    End Sub

#End Region

#Region "Receive By Id"

    Public Function ReceiveByLoadId(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String, Optional ByVal pDocType As String = "") As Load
        If _status <> WMS.Lib.Statuses.ASN.EXPECTED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "ASNDETAIL already received", "ASNDETAIL already received")
        End If
        Dim oRec As ReceiptHeader
        Dim oRecDetail As ReceiptDetail
        Dim oLoad() As Load
        'oRec = New ReceiptHeader(_receipt)
        oRecDetail = New ReceiptDetail(_receipt, _receiptline)
        'Validate the company flag for ASN receiving
        If oRecDetail.COMPANY <> String.Empty Then
            Dim oComp As New Company(oRecDetail.CONSIGNEE, oRecDetail.COMPANY, oRecDetail.COMPANYTYPE)
            If Not oComp.ALLOWASNREC Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot receive ASN for current company", "Cannot receive ASN for current company")
            End If
        End If
        If _loadid = "" Then
            _loadid = Made4Net.Shared.Util.getNextCounter("LOAD")
        End If
        Dim ocons As New Consignee(oRecDetail.CONSIGNEE)
        If pLocation = "" Then
            pLocation = ocons.DEFAULTRECEIVINGLOCATION
        End If
        If pWarehousearea = "" Then
            pWarehousearea = ocons.DEFAULTRECEIVINGWAREHOUSEAREA
        End If
        Dim oReceiving As New Receiving
        oLoad = oReceiving.CreateLoad(_receipt, _receiptline, oRecDetail.SKU, _loadid, _uom, pLocation, pWarehousearea, _units, "AVAILABLE", "", 1, pUser, Nothing, _asnattributes.Attributes, Nothing, pDocType, Nothing, Nothing)
        SetStatus(WMS.Lib.Statuses.ASN.RECEIVED, pUser)
        Return oLoad(0)
    End Function

    Public Shared Function ReceiveByContId(ByVal pContainerId As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String, Optional ByVal pDocType As String = "")
        Dim SQL As String
        Dim oASN As AsnDetail
        SQL = String.Format("select distinct asnid from asndetail where container='{0}'", pContainerId)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        Dim oCont As New Container
        If dt.Rows.Count > 0 Then
            oCont.ContainerId = pContainerId
            oCont.Post(pUser)
        End If
        Dim oLoad As Load
        For Each dr As DataRow In dt.Rows
            oASN = New AsnDetail(Convert.ToString(dr("asnid")))
            oLoad = oASN.ReceiveByLoadId(pLocation, pWarehousearea, pUser, pDocType)
            oCont.Place(oLoad, pUser)
        Next
    End Function

#End Region

#Region "Print"

#Region "Label Printing"

    Public Sub PrintLabel(Optional ByVal lblPrinter As String = "")
        WMS.Logic.AsnDetail.PrintLabel(_asnid, lblPrinter)
    End Sub

    Public Shared Sub PrintLabel(ByVal AsnId As String, ByVal lblPrinter As String)
        'Added for RWMS-2047 and RWMS-1637     
        If Not Made4Net.Label.LabelHandler.Factory.GetNewLableHandler().ValidateLabel("ASNDETAIL") Then
            Throw New M4NException(New Exception(), "ASNDETAIL Label Not Configured.", "ASNDETAIL Label Not Configured.")
        Else
            If lblPrinter Is Nothing Then
                lblPrinter = ""
            End If
            Dim qSender As New Made4Net.Shared.QMsgSender
            qSender.Add("LABELNAME", "ASNDETAIL")
            qSender.Add("PRINTER", lblPrinter)
            qSender.Add("ASNID", AsnId)
            'Start RWMS-1048   
            qSender.Add("LabelType", "ASNDETAIL")
            Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
            ht.Hash.Add("ASNID", AsnId)
            qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
            'End RWMS-1048   
            qSender.Send("Label", "ASN Label")
        End If
        'Ended for RWMS-2047 and RWMS-1637  

        'If lblPrinter Is Nothing Then
        '    lblPrinter = ""
        'End If
        'Dim qSender As New Made4Net.Shared.QMsgSender
        'qSender.Add("LABELNAME", "ASNDETAIL")
        'qSender.Add("PRINTER", lblPrinter)
        'qSender.Add("ASNID", AsnId)
        ''Start RWMS-1048
        'qSender.Add("LabelType", "ASNDETAIL")
        'Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        'ht.Hash.Add("ASNID", AsnId)
        'qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        ''End RWMS-1048
        'qSender.Send("Label", "ASN Label")

    End Sub

#End Region

#End Region

#End Region

End Class

#End Region

