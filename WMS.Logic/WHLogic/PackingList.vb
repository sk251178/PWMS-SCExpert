Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util

#Region "PackingList"

<CLSCompliant(False)> Public Class PackingListHeader

#Region "Inner Classes"

#Region "PackingList Detail Collection"

    <CLSCompliant(False)> Public Class PackingListDetailCollection
        Inherits ArrayList

#Region "Variables"

        Protected _packinglistid As String

#End Region

#Region "Properties"

        Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As PackingListDetail
            Get
                Return CType(MyBase.Item(index), PackingListDetail)
            End Get
        End Property

        Public ReadOnly Property PackDetail(ByVal pLoadid As String) As PackingListDetail
            Get
                For Each oPackDetail As PackingListDetail In Me
                    If oPackDetail.LOADID = pLoadid Then
                        Return oPackDetail
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property PackDetailExists(ByVal pLoadid As String) As Boolean
            Get
                For Each oPackDetail As PackingListDetail In Me
                    If oPackDetail.LOADID = pLoadid Then
                        Return True
                    End If
                Next
                Return False
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByVal pPackinglistId As String, Optional ByVal LoadAll As Boolean = True)
            _packinglistid = pPackinglistId
            Dim SQL As String
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select loadid from PACKINGLISTDETAIL where PACKINGLISTID = '" & _packinglistid & "'"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                Me.add(New PackingListDetail(_packinglistid, dr("loadid")))
            Next
        End Sub

#End Region

#Region "Methods"

#Region "Pack / Unpack"

        Public Function PackLoad(ByVal pLoadid As String, ByVal pUserId As String)
            Dim oPacklistDetail As New PackingListDetail
            oPacklistDetail.Create(_packinglistid, pLoadid, pUserId)
            Me.add(oPacklistDetail)
        End Function

        Public Function UnPackLoad(ByVal pLoadid As String, ByVal pUserId As String)
            Dim oPacklistDetail As PackingListDetail = PackDetail(pLoadid)
            oPacklistDetail.Delete(pUserId)
            Me.Remove(oPacklistDetail)
        End Function

#End Region

#Region "Overrides"

        Public Shadows Function add(ByVal pObject As PackingListDetail) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As PackingListDetail)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As PackingListDetail)
            MyBase.Remove(pObject)
        End Sub

#End Region

#End Region

    End Class

#End Region

#Region "PackingList Detail"

    <CLSCompliant(False)> Public Class PackingListDetail

#Region "Variables"

        Protected _packinglistid As String
        Protected _loadid As String
        Protected _adddate As DateTime
        Protected _adduser As String = String.Empty
        Protected _editdate As DateTime
        Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return String.Format(" PACKINGLISTID = '{0}' and LOADID = '{1}'", _packinglistid, _loadid)
            End Get
        End Property

        Public Property PACKINGLISTID() As String
            Set(ByVal Value As String)
                _packinglistid = Value
            End Set
            Get
                Return _packinglistid
            End Get
        End Property

        Public Property LOADID() As String
            Get
                Return _loadid
            End Get
            Set(ByVal value As String)
                _loadid = value
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

        Public Sub New(ByVal pPackinglistId As String, ByVal pLoadid As String, Optional ByVal LoadObj As Boolean = True)
            _packinglistid = pPackinglistId
            _loadid = pLoadid
            If LoadObj Then
                Load()
            End If
        End Sub

#End Region

#Region "Methods"

#Region "Accessors"

        Public Shared Function GetPackingListDetail(ByVal pPackinglistId As String, ByVal pLoadid As String) As PackingListDetail
            Return New PackingListDetail(pPackinglistId, pLoadid)
        End Function

        Public Shared Function Exists(ByVal pPackinglistId As String, ByVal pLoadid As String) As Boolean
            Dim sql As String = String.Format("Select count(1) from PACKINGLISTDETAIL where packinglistid = '{0}' and loadid = '{1}'", pPackinglistId, pLoadid)
            Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Protected Sub Load()
            Dim SQL As String = "SELECT * FROM PACKINGLISTDETAIL WHERE " & WhereClause
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Packing List Detail does not exists", "Packing List Detail does not exists")
            End If
            dr = dt.Rows(0)
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        End Sub

#End Region

#Region "Create / Delete / Ship"

        Public Sub Create(ByVal pPackinglistId As String, ByVal pLoadId As String, ByVal pUserId As String)
            If WMS.Logic.PackingListHeader.PackingListDetail.Exists(pPackinglistId, pLoadId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add line to packinglist - Already Exists", "Cannot add line to packinglist - Already Exists")
            End If
            If Not WMS.Logic.Load.Exists(pLoadId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add line to packinglist - Invalid Load", "Cannot add line to packinglist - Invalid Load")
            End If
            _packinglistid = pPackinglistId
            _loadid = pLoadId
            _adddate = DateTime.Now
            _adduser = pUserId
            _editdate = DateTime.Now
            _edituser = pUserId

            Dim sql As String = String.Format("INSERT INTO PACKINGLISTDETAIL (PACKINGLISTID, LOADID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5})", _
                       FormatField(_packinglistid), FormatField(_loadid), FormatField(_adddate), FormatField(_adduser), FormatField(_editdate), FormatField(_edituser))
            DataInterface.RunSQL(sql)

            'Update the order detail about the packed Quantities
            Dim oLoad As New WMS.Logic.Load(pLoadId)
            oLoad.Pack(pUserId)
        End Sub

        Public Sub Delete(ByVal pUserId As String)
            'Update the order detail about the packed Quantities
            Dim oLoad As New WMS.Logic.Load(_loadid)
            oLoad.UnPack(pUserId)
            Dim sql As String = String.Format("delete from PACKINGLISTDETAIL where {0}", WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub Ship(ByVal pUserId As String)
            'Update the order detail about the packed Quantities
            Dim oLoad As New WMS.Logic.Load(_loadid)
            oLoad.Ship(pUserId)
        End Sub

#End Region

#End Region

    End Class

#End Region

#End Region

#Region "Variables"

    Protected _packinglistid As String
    Protected _consignee As String
    Protected _company As String
    Protected _companytype As String
    Protected _contactid As String
    Protected _status As String
    Protected _numpackages As String
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

    'Colleactions
    Protected _lines As PackingListDetailCollection

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " PACKINGLISTID = '" & _packinglistid & "'"
        End Get
    End Property

    Public Property PACKINGLISTID() As String
        Set(ByVal Value As String)
            _packinglistid = Value
        End Set
        Get
            Return _packinglistid
        End Get
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal value As String)
            _consignee = value
        End Set
    End Property

    Public Property COMPANY() As String
        Get
            Return _company
        End Get
        Set(ByVal value As String)
            _company = value
        End Set
    End Property

    Public Property COMPANYTYPE() As String
        Get
            Return _companytype
        End Get
        Set(ByVal value As String)
            _companytype = value
        End Set
    End Property

    Public Property CONTACTID() As String
        Get
            Return _contactid
        End Get
        Set(ByVal value As String)
            _contactid = value
        End Set
    End Property

    Public Property NUMPACKAGES() As Int32
        Get
            Return _numpackages
        End Get
        Set(ByVal value As Int32)
            _numpackages = value
        End Set
    End Property

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
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

    Public ReadOnly Property Lines() As PackingListDetailCollection
        Get
            Return _lines
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _lines = New PackingListDetailCollection(_packinglistid)
    End Sub

    Public Sub New(ByVal pPackinglistId As String, Optional ByVal LoadObj As Boolean = True)
        _packinglistid = pPackinglistId
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetPackingList(ByVal pPackinglistId As String) As PackingListHeader
        Return New PackingListHeader(pPackinglistId)
    End Function

    Public Shared Function Exists(ByVal pPackinglistId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from PACKINGLISTHEADER where packinglistid = '{0}'", pPackinglistId)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM PACKINGLISTHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Packing List does not exists", "Packing List does not exists")
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("COMPANY") Then _company = dr.Item("COMPANY")
        If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")
        If Not dr.IsNull("CONTACTID") Then _contactid = dr.Item("CONTACTID")
        If Not dr.IsNull("NUMPACKAGES") Then _numpackages = dr.Item("NUMPACKAGES")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _lines = New PackingListDetailCollection(_packinglistid)
    End Sub

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
        _status = pStatus
        _edituser = pUser
        _editdate = DateTime.Now
        Dim sql As String = String.Format("Update PACKINGLISTHEADER SET STATUS = {0},EditUser = {1},EditDate = {2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Private Function CanPackLoad(ByVal pLoadid As String) As Boolean
        Dim oCons As New Consignee(_consignee)
        If oCons.PACKMULTIPLEORDERS Then
            Return True
        Else
            Dim loadOrderid As String = DataInterface.ExecuteScalar(String.Format("select orderid from orderloads where loadid = '{0}' and consignee = '{1}'", pLoadid, _consignee))
            Dim SQL As String = String.Format("select distinct orderid from vPackingLists where packinglistid = '{0}'", _packinglistid)
            Dim dt As New DataTable
            DataInterface.FillDataset(SQL, dt)
            For Each dr As DataRow In dt.Rows
                If loadOrderid.ToLower <> dr("orderid") Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

#End Region

#Region "Create / Update"

    Public Sub Create(ByVal pPackinglistId As String, ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanytype As String, ByVal pContactId As String, ByVal pNumPackages As Int32, ByVal pUser As String)
        If WMS.Logic.PackingListHeader.Exists(pPackinglistId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Packing List - Already Exists", "Cannot Create Packing List - Already Exists")
        End If
        If Not WMS.Logic.Company.Exists(pConsignee, pCompany, pCompanytype) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Packing List - Invalid Company", "Cannot Create Packing List - Invalid Company")
        End If
        If pContactId <> String.Empty Then
            If Not Contact.Exists(pContactId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Packing List - Invalid Contact", "Cannot Create Packing List - Invalid Contact")
            End If
        End If
        If pPackinglistId <> "" Then
            _packinglistid = pPackinglistId
        Else
            _packinglistid = Made4Net.Shared.Util.getNextCounter("PACKINGLIST")
        End If
        _consignee = pConsignee
        _company = pCompany
        _companytype = pCompanytype
        If pContactId = String.Empty Then
            Dim oComp As New WMS.Logic.Company(pConsignee, pCompany, pCompanytype)
            _contactid = oComp.DEFAULTCONTACT
        Else
            _contactid = pContactId
        End If
        _numpackages = pNumPackages
        _status = WMS.Lib.Statuses.PackingLists.[NEW]
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("INSERT INTO PACKINGLISTHEADER (PACKINGLISTID, CONSIGNEE, COMPANY, COMPANYTYPE, CONTACTID, STATUS, NUMPACKAGES, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", _
                   FormatField(_packinglistid), FormatField(_consignee), FormatField(_company), FormatField(_companytype), FormatField(_contactid), FormatField(_status), FormatField(_numpackages), FormatField(_adddate), FormatField(_adduser), FormatField(_editdate), FormatField(_edituser))
        DataInterface.RunSQL(sql)
        _lines = New PackingListDetailCollection(_packinglistid)

        Dim aq As EventManagerQ = New EventManagerQ
        Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.PackingListCreated)
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PackingListCreated)
        aq.Add("ACTIVITYTYPE", actType)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _packinglistid)
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
        aq.Add("USERID", _adduser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", "")
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", "")
        aq.Send(actType)
    End Sub

    Public Sub Update(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanytype As String, ByVal pContactId As String, ByVal pNumPacks As Int32, ByVal pUser As String)
        If Not WMS.Logic.PackingListHeader.Exists(_packinglistid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Update Packing List - Does Not Exists", "Cannot Update Packing List - Does Not Exists")
        End If
        If Not WMS.Logic.Company.Exists(pConsignee, pCompany, pCompanytype) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Packing List - Invalid Company", "Cannot Create Packing List - Invalid Company")
        End If
        If pContactId <> String.Empty Then
            If Not Contact.Exists(pContactId) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Packing List - Invalid Contact", "Cannot Create Packing List - Invalid Contact")
            End If
        End If
        _consignee = pConsignee
        _company = pCompany
        _companytype = pCompanytype
        If Not pContactId = String.Empty Then
            _contactid = pContactId
        End If
        _numpackages = pNumPacks
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE PACKINGLISTHEADER SET CONSIGNEE ={0}, COMPANY ={1}, COMPANYTYPE ={2}, CONTACTID ={3}, STATUS ={4}, NUMPACKAGES ={5}, EDITDATE ={6}, EDITUSER ={7} where {8}", _
                   FormatField(_consignee), FormatField(_company), FormatField(_companytype), FormatField(_contactid), FormatField(_status), FormatField(_numpackages), FormatField(_editdate), FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.PackingListUpdated)
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PackingListUpdated)
        aq.Add("ACTIVITYTYPE", actType)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _packinglistid)
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
        aq.Add("USERID", _adduser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", "")
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", "")
        aq.Send(actType)
    End Sub

#End Region

#Region "Pack / Unpack / Ship"

    Public Sub Ship(ByVal pUser As String)
        'Made4Net.DataAccess.DataInterface.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)
        Try
            For Each oDet As PackingListDetail In Me.Lines
                oDet.Ship(pUser)
            Next
            SetStatus(WMS.Lib.Statuses.PackingLists.SHIPPED, pUser)
            Dim aq As EventManagerQ = New EventManagerQ
            Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.PackingListShipped)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PackingListShipped)
            aq.Add("ACTIVITYTYPE", actType)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _packinglistid)
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
            aq.Add("USERID", _adduser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(actType)
        Catch ex As Made4Net.Shared.M4NException
            'Made4Net.DataAccess.DataInterface.RollBackTransaction()
            Throw ex
        Catch ex As Exception
            Throw ex
            'Made4Net.DataAccess.DataInterface.RollBackTransaction()
            'Throw New Made4Net.Shared.M4NException(New Exception, "Shipping failed, please ship packinglist again", "Shipping failed, please ship packinglist again")
        End Try
        'Made4Net.DataAccess.DataInterface.CommitTransaction()
    End Sub

    Public Sub PackLoad(ByVal pLoadid As String, ByVal pUser As String)
        'Made4Net.DataAccess.DataInterface.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)
        Try
            If Me.Lines.PackDetailExists(pLoadid) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Load already packed", "Load already packed")
            End If
            'First verify that the load is not packed in a different list
            Dim SQL As String = String.Format("select count(1) from PACKINGLISTDETAIL where loadid = '{0}' and packinglistid <> '{1}'", pLoadid, _packinglistid)
            If System.Convert.ToBoolean(DataInterface.ExecuteScalar(SQL)) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Load already packed to a different packing list", "Load already packed to a different packing list")
            End If
            If CanPackLoad(pLoadid) Then
                Me.Lines.PackLoad(pLoadid, pUser)
            End If
            If _status <> WMS.Lib.Statuses.PackingLists.ASSIGNED Then
                SetStatus(WMS.Lib.Statuses.PackingLists.ASSIGNED, pUser)
            End If

            Dim aq As EventManagerQ = New EventManagerQ
            Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.PackingListLoadPacked)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PackingListLoadPacked)
            aq.Add("ACTIVITYTYPE", actType)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _packinglistid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", pLoadid)
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", pLoadid)
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOLOC", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", _adduser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(actType)

        Catch ex As Made4Net.Shared.M4NException
            'Made4Net.DataAccess.DataInterface.RollBackTransaction()
            Throw ex
        Catch ex As Exception
            Throw ex
            'Made4Net.DataAccess.DataInterface.RollBackTransaction()
            'Throw New Made4Net.Shared.M4NException(New Exception, "Packing failed, please Pack Load again", "Packing failed, please Pack Load again")
        End Try
        'Made4Net.DataAccess.DataInterface.CommitTransaction()
    End Sub

    Public Sub UnPackLoad(ByVal pLoadid As String, ByVal pUser As String)
        'Made4Net.DataAccess.DataInterface.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)
        Try
            If Not Me.Lines.PackDetailExists(pLoadid) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Load does not belong to packing list", "Load does not belong to packing list")
            End If
            Me.Lines.UnPackLoad(pLoadid, pUser)
            If Lines.Count = 0 Then
                SetStatus(WMS.Lib.Statuses.PackingLists.[NEW], pUser)
            End If
            Dim aq As EventManagerQ = New EventManagerQ
            Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.PackingListLoadUnPacked)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PackingListLoadUnPacked)
            aq.Add("ACTIVITYTYPE", actType)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _packinglistid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", pLoadid)
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", pLoadid)
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", _adduser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(actType)
        Catch ex As Made4Net.Shared.M4NException
            'Made4Net.DataAccess.DataInterface.RollBackTransaction()
            Throw ex
        Catch ex As Exception
            Throw ex
            'Made4Net.DataAccess.DataInterface.RollBackTransaction()
            'Throw New Made4Net.Shared.M4NException(New Exception, "UnPacking failed, please UnPack Load again", "UnPacking failed, please UnPack Load again")
        End Try
        'Made4Net.DataAccess.DataInterface.CommitTransaction()
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        If STATUS <> WMS.Lib.Statuses.PackingLists.[NEW] Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot cancel packing list - invalid status", "Cannot cancel packing list - invalid status")
        Else
            SetStatus(WMS.Lib.Statuses.PackingLists.CANCELLED, pUser)

            Dim aq As EventManagerQ = New EventManagerQ
            Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.PackingListCancelled)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PackingListCancelled)
            aq.Add("ACTIVITYTYPE", actType)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _packinglistid)
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
            aq.Add("USERID", _adduser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", "")
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", "")
            aq.Send(actType)
        End If
    End Sub

#End Region

#Region "Printing"

    Public Sub PrintLabel(ByVal lblPrinter As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New Made4Net.Shared.QMsgSender
        qSender.Add("LABELNAME", "PACKINGLIST")
        qSender.Add("LabelType", "PACKINGLIST")
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("PACKINGLISTID", _packinglistid)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("PACKINGLISTID", _packinglistid)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Send("Label", "Packing List Label")
    End Sub


    Public Function PrintPackingList(ByVal pPrinterName As String, ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "PackingList"
        Dim Copies As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "PackingList", "Copies"))
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "PackingList")
        Dim setId As String = DataInterface.ExecuteScalar(String.Format("SELECT paramvalue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "PackingList", "DataSetName"))
        oQsender.Add("DATASETID", setId)
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", pPrinterName)
            oQsender.Add("COPIES", Copies)
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("PACKINGLISTID = '{0}'", _packinglistid))
        oQsender.Send("Report", repType)
    End Function

#End Region

#End Region

End Class

#End Region
