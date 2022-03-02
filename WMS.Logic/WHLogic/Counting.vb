Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

<CLSCompliant(False)> Public Class Counting

#Region "Variables"

    Protected _countid As String = String.Empty
    Protected _counttype As String = String.Empty
    Protected _status As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _sku As String = String.Empty
    Protected _loadid As String = String.Empty
    Protected _location As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _countbook As String = String.Empty
    Protected _countbookrunid As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" COUNTID = '{0}'", _countid)
        End Get
    End Property

    Public Property COUNTID() As String
        Get
            Return _countid
        End Get
        Set(ByVal Value As String)
            _countid = Value
        End Set
    End Property

    Public Property COUNTTYPE() As String
        Get
            Return _counttype
        End Get
        Set(ByVal Value As String)
            _counttype = Value
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

    Public Property LOCATION() As String
        Get
            Return _location
        End Get
        Set(ByVal Value As String)
            _location = Value
        End Set
    End Property

    Public Property WAREHOUSEAREA() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
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

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property SKU() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property COUNTBOOK() As String
        Get
            Return _countbook
        End Get
        Set(ByVal Value As String)
            _countbook = Value
        End Set
    End Property

    Public Property COUNTBOOKRUNID() As String
        Get
            Return _countbookrunid
        End Get
        Set(ByVal Value As String)
            _countbookrunid = Value
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

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName = "CreateLoadCountJobs" Then
            CreateLoadCountJobs(dr("consignee"), dr("location"), dr("warehousearea"), dr("sku"), dr("loadid"), Common.GetCurrentUser())
            Message = "Load Count Tasks Created"
        ElseIf CommandName = "CreateLocationCountJobs" Then
            Dim whArea As String
            Try
                whArea = Convert.ReplaceDBNull(dr("warehousearea"))
            Catch ex As Exception
            End Try
            CreateLocationCountJobs(whArea, Convert.ReplaceDBNull(dr("pickregion")), Convert.ReplaceDBNull(dr("location")), dr("locusagetype"), "", "", Common.GetCurrentUser())
            Message = "Location Count Tasks Created"
        End If
    End Sub

    Public Sub New(ByVal pCountid As String, Optional ByVal LoadObj As Boolean = True)
        _countid = pCountid
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal pCountType As String, ByVal pSearchKey As String, Optional ByVal LoadObj As Boolean = True)
        Dim sql As String
        Select Case pCountType
            Case WMS.Lib.TASKTYPE.LOADCOUNTING
                sql = String.Format("select countid from counting where STATUS ='PLANNED' AND counttype='{0}' AND LOADID='{1}'", _
                                pCountType, pSearchKey)
            Case Else
                sql = String.Format("select countid from counting where STATUS ='PLANNED' AND counttype='{0}' AND LOCATION='{1}'", _
                                pCountType, pSearchKey)
        End Select
        _countid = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetCount(ByVal pCountid As String) As Counting
        Return New Counting(pCountid)
    End Function

    Public Shared Function Exists(ByVal pCountid As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from conuting where conutid = '{0}'", pCountid)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function Exists(ByVal pCountType As String, ByVal pSearchKey As String) As Boolean
        Dim sql As String
        Select Case pCountType
            Case WMS.Lib.TASKTYPE.LOADCOUNTING
                sql = String.Format("select count(1) from counting where STATUS ='PLANNED' AND counttype='{0}' AND LOADID='{1}'", _
                                pCountType, pSearchKey)
            Case Else
                sql = String.Format("select count(1) from counting where STATUS ='PLANNED' AND counttype='{0}' AND LOCATION='{1}'", _
                                pCountType, pSearchKey)
        End Select
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    'Added for RWMS-2262 RWMS-2222
    Public Shared Function GetCountID(ByVal pCountType As String, ByVal pSearchKey As String) As String
        Dim sql As String
        Dim countID As String
        countID = "0"

        Try
            Select Case pCountType
                Case WMS.Lib.TASKTYPE.LOADCOUNTING
                    sql = String.Format("select top 1 IsNull(COUNTID,0) as COUNTID from counting where STATUS ='PLANNED' AND counttype='{0}' AND LOADID='{1}'", _
                                    pCountType, pSearchKey)
                Case Else
                    sql = String.Format("select top 1 IsNull(COUNTID,0) as COUNTID from counting where STATUS ='PLANNED' AND counttype='{0}' AND LOCATION='{1}'", _
                                    pCountType, pSearchKey)
            End Select
            countID = System.Convert.ToString(DataInterface.ExecuteScalar(sql))
        Catch ex As Exception

        End Try
        Return countID
    End Function
    'End Added for RWMS-2262 RWMS-2222

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM counting WHERE countid = '" & _countid & "'"
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Counting does not exists", "Counting does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("countbook") Then _countbook = dr.Item("countbook")
        If Not dr.IsNull("countbookrunid") Then _countbookrunid = dr.Item("countbookrunid")
        If Not dr.IsNull("counttype") Then _counttype = dr.Item("counttype")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("loadid") Then _loadid = dr.Item("loadid")
        If Not dr.IsNull("location") Then _location = dr.Item("location")
        If Not dr.IsNull("warehousearea") Then _warehousearea = dr.Item("warehousearea")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Private Function HasTask() As Boolean
        Dim sql As String = String.Format("Select count(1) from TASKS where countid='{0}' and status not in('COMPLETE','CANCELED')", _countid)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Function GetCountTask() As Task
        Dim sql As String = String.Format("SELECT DISTINCT TASK FROM TASKS WHERE countid='{0}' and status not in('COMPLETE','CANCELED')", _countid)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return Nothing
        Else
            Return New Task(dt.Rows(0)("TASK"))
        End If
    End Function

#End Region

#Region "Post"

    Public Sub CreateLoadCountJobs(ByVal pConsignee As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pSku As String, ByVal pLoad As String, ByVal pUser As String)
        Dim SQL As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow

        'SQL = String.Format("select * from Invload where loadid like '{0}%' and consignee like '{1}%' and sku like '{2}%' and location like '{3}%' and warehousearea like '{4}%' and (activitystatus is null or activitystatus ='') and status<>'LIMBO'", _
        '                    pLoad, pConsignee, pSku, pLocation, pWarehousearea)
        SQL = String.Format("select * from Invload where loadid like '{0}%' and consignee like '{1}%' and sku like '{2}%' and location like '{3}%' and warehousearea like '{4}%' and (activitystatus is null or activitystatus ='')", _
                            pLoad, pConsignee, pSku, pLocation, pWarehousearea)
        DataInterface.FillDataset(SQL, ds)

        For Each dr In ds.Tables(0).Rows
            If Not Exists(WMS.Lib.TASKTYPE.LOADCOUNTING, dr("loadid")) Then
                Post(WMS.Lib.TASKTYPE.LOADCOUNTING, dr("loadid"), dr("location"), dr("warehousearea"), dr("consignee"), dr("sku"), "", "", pUser)
            End If
        Next
    End Sub

    Public Function CreateLocationCountJobs(ByVal pWarehouseArea As String, ByVal pPickRegion As String, ByVal pLocation As String, ByVal pTaskType As String, ByVal pCountBook As String, ByVal pCountBookRunId As String, ByVal pUser As String) As Int32
        Dim SQL As String = String.Empty
        Dim ds As New DataSet
        Dim dr As DataRow

        'Commented for RWMS-2262 RWMS-2222
        'Dim counter As Int32 = 0
        'End Commented for RWMS-2262 RWMS-2222

        'Added for RWMS-2262 RWMS-2222
        Dim counter As Integer
        Dim COUNTID As String = ""
        'End Added for RWMS-2262 RWMS-2222

        If pLocation.Trim = String.Empty Then
            SQL = String.Format("select * from location where location like '{0}%' and isnull(pickregion,'') like '{1}%' and isnull(warehousearea,'') like '{2}%' and status=1 and inventory=1", pLocation, pPickRegion, pWarehouseArea)
        Else
            SQL = String.Format("select * from location where location = '{0}' and isnull(pickregion,'') like '{1}%' and isnull(warehousearea,'') like '{2}%' and status=1 and inventory=1", pLocation, pPickRegion, pWarehouseArea)
        End If
        DataInterface.FillDataset(SQL, ds)

        For Each dr In ds.Tables(0).Rows
            'Commented for RWMS-2262 RWMS-2222
            'If Not Exists(pTaskType, dr("location")) Then
            '    Post(pTaskType, "", dr("location"), dr("warehousearea"), "", "", pCountBook, pCountBookRunId, pUser)
            'End If
            'counter += 1
            'End Commented for RWMS-2262 RWMS-2222

            'Added for RWMS-2262 RWMS-2222
            COUNTID = GetCountID(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, dr("location"))

            If COUNTID.CompareTo("0") > 0 Then
                _countid = COUNTID
                Load()
            Else
                Post(pTaskType, "", dr("location"), dr("warehousearea"), "", "", pCountBook, pCountBookRunId, pUser)
            End If
            counter += 1
            'End Added for RWMS-2262 RWMS-2222

        Next
        Return counter
    End Function

    Public Sub Post(ByVal pCountType As String, ByVal pLoadid As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pCountBook As String, ByVal pCountBookRunId As String, ByVal pUser As String)
        _countid = Made4Net.Shared.Util.getNextCounter("COUNTING")
        _counttype = pCountType
        _consignee = pConsignee
        _sku = pSku
        _location = pLocation
        _warehousearea = pWarehousearea
        _loadid = pLoadid
        _countbook = pCountBook
        _countbookrunid = pCountBookRunId
        _status = WMS.Lib.Statuses.Counting.PLANNED
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("INSERT INTO COUNTING(COUNTID, COUNTTYPE, STATUS, CONSIGNEE, SKU, LOADID, LOCATION, WAREHOUSEAREA,COUNTBOOK, COUNTBOOKRUNID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) values (" & _
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", Made4Net.Shared.Util.FormatField(_countid), Made4Net.Shared.Util.FormatField(_counttype), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_sku), _
            Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_countbook), Made4Net.Shared.Util.FormatField(_countbookrunid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(sql)

        'And create task
        Dim tm As New TaskManager
        tm.CreateCountTask(Me)
    End Sub

#End Region

#Region "Cancel / Complete"

    Public Sub Cancel(ByVal pCountId As String, ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.Counting.PLANNED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Counting Status", "Wrong Counting Status")
        End If

        _status = WMS.Lib.Statuses.Counting.CANCELED
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update counting set status={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        If HasTask() Then
            Dim ts As Task = GetCountTask()
            ts.Cancel()
        End If
        'Send proper Event
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.CountingCancelled
        em.Add("EVENT", EventType)
        em.Add("REPLID", _countid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub Complete(ByVal pUser As String)
        _status = WMS.Lib.Statuses.Counting.COMPLETE
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update counting set status={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        If HasTask() Then
            Dim ts As Task = GetCountTask()
            ts.ExecutionLocation = _location
            ts.ExecutionWarehousearea = _warehousearea
            ts.Complete(Nothing)
        End If
        'Send proper Event
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.CountingCompleted
        em.Add("EVENT", EventType)
        em.Add("REPLID", _countid)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Count"

    Public Function Count(ByVal oCntJob As CountingJob, ByVal pUser As String) As Boolean
        Select Case COUNTTYPE
            Case WMS.Lib.TASKTYPE.LOADCOUNTING
                Dim oLoad As New WMS.Logic.Load(oCntJob.LoadId)
                If (oLoad.ShouldVerifyCounting(oCntJob.CountedQty, oCntJob.UOM) Or Not WMS.Logic.Load.ShouldVerifyCountingAttributes(oCntJob.LoadId, oCntJob.CountingAttributes)) And Not oCntJob.LoadCountVerified Then
                    Return False
                End If
                'Commented for RWMS-467
                'oLoad.Count(oCntJob.CountedQty, oCntJob.UOM, oCntJob.Location, oCntJob.Warehousearea, oCntJob.CountingAttributes, pUser)
                'End Commented for RWMS-467
                'Added for RWMS-467
                oLoad.Count(oCntJob.CountedQty, oCntJob.UOM, oCntJob.Location, oCntJob.Warehousearea, oCntJob.CountingAttributes, pUser, oCntJob.CountId)
                'End Added for RWMS-467
                '' Used for task complete, doesn't update Counting table...
                Me.LOCATION = oCntJob.Location
                Me.WAREHOUSEAREA = oCntJob.Warehousearea
            Case WMS.Lib.TASKTYPE.LOCATIONCOUNTING, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                'Send All not counted loads to limbo..
                UpdateLimboLoad(oCntJob.CountedLoads, pUser)
                'And update location
                Dim oLoc As New Location(oCntJob.Location, oCntJob.Warehousearea)
                'Commented for RWMS-467
                'oLoc.Count(oCntJob.ExpectedQty, oCntJob.CountedQty, pUser)
                'End Commented for RWMS-467
                'Added for RWMS-467
                'Pass the CountId and CountedLoads - CountedLoads contains LoadId, So from LoadId we can get the Consignee and sku
                oLoc.Count(oCntJob.ExpectedQty, oCntJob.CountedQty, pUser, oCntJob.CountId, oCntJob.CountedLoads)
                'End Added for RWMS-467
                'Check if this count assigned to a countbook, and update the countbook
                If WMS.Logic.CountBook.Exists(_countbook) Then
                    Dim oCntBook As New CountBook(_countbook)
                    oCntBook.Count(pUser)
                    'If location counting -> update audit with all loads
                    If COUNTTYPE = WMS.Lib.TASKTYPE.LOCATIONCOUNTING Or oCntJob.BulkCountLoadsCounted Then
                        PostCountBookAudit(oCntJob.CountedLoads, pUser)
                    Else
                        'Else Bulk Count Completed successfully
                        Dim fromQty, toQty As Decimal
                        CalcQty(oCntJob.CountedLoads, fromQty, toQty)
                        Dim oBLKCntBookAuditRec As New CountBookAudit
                        oBLKCntBookAuditRec.Post(_countbook, _countbookrunid, _countid, "", _location, _warehousearea, fromQty, toQty, pUser, pUser)
                    End If
                End If
        End Select
        Complete(pUser)
        Return True
    End Function

    Private Sub UpdateLimboLoad(ByVal dt As DataTable, ByVal pUser As String)
        Dim oLoad As Load
        If Not dt Is Nothing Then
            For Each dr As DataRow In dt.Rows
                If dr("counted") = 0 Then
                    oLoad = New WMS.Logic.Load(System.Convert.ToString(dr("loadid")))
                    oLoad.Count(0, oLoad.LOCATION, oLoad.WAREHOUSEAREA, Nothing, pUser)
                End If
            Next
        End If
    End Sub

    Private Sub PostCountBookAudit(ByVal dt As DataTable, ByVal pUser As String)
        Dim oCntBookAuditRec As New CountBookAudit
        If Not dt Is Nothing Then
            For Each dr As DataRow In dt.Rows
                If dr("counted") = 1 Then
                    oCntBookAuditRec.Post(_countbook, _countbookrunid, _countid, dr("loadid"), _location, _warehousearea, dr("fromqty"), dr("toqty"), pUser, pUser)
                End If
            Next
        End If
    End Sub

    Private Sub CalcQty(ByVal dt As DataTable, ByRef pFromqty As Decimal, ByRef pToQty As Decimal)
        For Each dr As DataRow In dt.Rows
            pFromqty = pFromqty + System.Convert.ToDecimal(dr("fromqty"))
            pToQty = pToQty + System.Convert.ToDecimal(dr("toqty"))
        Next
    End Sub

#End Region

#Region "Create Load"

    Public Sub CreateLoad(ByVal pLoadId As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pStatus As String, _
                ByVal pLocation As String, ByVal pWarehouseArea As String, ByVal pUOM As String, ByVal oAttributesCollection As WMS.Logic.AttributesCollection, ByVal pUserId As String, ByVal oLogger As LogHandler)

        Dim oLoad As New WMS.Logic.Load()
        Dim oConsignee As New WMS.Logic.Consignee(pConsignee)
        Dim oSKU As New WMS.Logic.SKU(pConsignee, pSku)

        pUnits = oSKU.ConvertToUnits(pUOM) * pUnits

        'Create the new load
        If pLoadId = "" AndAlso oConsignee.GENERATELOADID Then
            pLoadId = WMS.Logic.Load.GenerateLoadId()
        End If
        ValidateAttributes(oSKU.SKUClass, oAttributesCollection, oSKU.CONSIGNEE, oSKU.SKU, pStatus)
        oLoad.CreateLoad(pLoadId, pConsignee, pSku, pUOM, pLocation, pWarehouseArea, pStatus, WMS.Lib.Statuses.ActivityStatus.NONE, pUnits, "", 0, "", oAttributesCollection, WMS.Logic.GetCurrentUser, Nothing)
        If oConsignee.AUTOGENERATELOADLABELRCV Then
            oLoad.PrintLabel()
        End If
        SendLoadCreatedEvents(oLoad, pUserId)

        'Create a counter load in limbo
        Dim limboLoadObj As New WMS.Logic.Load()
        limboLoadObj.CreateLoad(WMS.Logic.Load.GenerateLoadId(), pConsignee, pSku, pUOM, "", "", WMS.Lib.Statuses.LoadStatus.LIMBO, _
            WMS.Lib.Statuses.ActivityStatus.NONE, pUnits * -1, "", 1, "", oAttributesCollection, pUserId, oLogger, "", DateTime.Now, pLocation, pStatus)
    End Sub

    Private Sub SendLoadCreatedEvents(ByVal pLoad As WMS.Logic.Load, ByVal pUserId As String)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadCount)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.COUNTLOAD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", pLoad.CONSIGNEE)
        aq.Add("DOCUMENT", pLoad.RECEIPT)
        aq.Add("DOCUMENTLINE", pLoad.RECEIPTLINE)
        aq.Add("FROMLOAD", pLoad.LOADID)
        aq.Add("FROMLOC", pLoad.LOCATION)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", pLoad.STATUS)
        aq.Add("NOTES", "")
        aq.Add("SKU", pLoad.SKU)
        aq.Add("TOLOAD", pLoad.LOADID)
        aq.Add("TOLOC", pLoad.LOCATION)
        aq.Add("TOQTY", pLoad.UNITS)
        aq.Add("TOSTATUS", pLoad.STATUS)
        aq.Add("USERID", pUserId)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUserId)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUserId)
        aq.Send(WMS.Lib.Actions.Audit.COUNTLOAD)

        'Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        'it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.CREATELOAD)
        'it.Add("CONSIGNEE", pLoad.CONSIGNEE)
        'it.Add("DOCUMENT", "")
        'it.Add("LINE", 0)
        'it.Add("LOADID", pLoad.LOADID)
        'it.Add("UOM", pLoad.LOADUOM)
        'it.Add("QTY", pLoad.UNITS)
        'it.Add("CUBE", 0)
        'it.Add("WEIGHT", 0)
        'it.Add("AMOUNT", 0)
        'it.Add("SKU", pLoad.SKU)
        'it.Add("STATUS", pLoad.STATUS)
        'it.Add("REASONCODE", "")
        'it.Add("UNITPRICE", 0)
        'InventoryTransaction.CreateAttributesRecords(it, pLoad.LoadAttributes)
        'it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("ADDUSER", pUserId)
        'it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("EDITUSER", pUserId)
        'it.Send(WMS.Lib.INVENTORY.CREATELOAD)
    End Sub

    Private Sub ValidateAttributes(ByVal oskuclass As WMS.Logic.SkuClass, ByVal oAttributesCollection As WMS.Logic.AttributesCollection, ByVal pConsignee As String, ByVal pSKU As String, ByVal pLoadStatus As String)
        'Attribute Validation
        If Not oskuclass Is Nothing Then
            For Each oLoadAtt As SkuClassLoadAttribute In oskuclass.LoadAttributes
                If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                        If oAttributesCollection Is Nothing Then Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                        If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                            Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                        End If
                    End If
                    ' Validate that the attributes supplied are valid
                    ValidateAttributeByType(oLoadAtt, oAttributesCollection(oLoadAtt.Name))
                    ' Validator
                    If Not oLoadAtt.ReceivingValidator Is Nothing Then
                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                        vals.Add(oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name))
                        vals.Add("RECEIPT", pConsignee)
                        vals.Add("LINE", CStr(pSKU))
                        vals.Add("LOADSTATUS", pLoadStatus)
                        Dim ret As String = oLoadAtt.Evaluate(SkuClassLoadAttribute.EvaluationType.Receiving, vals)
                        Dim returnedResponse() As String = ret.Split(";")
                        If returnedResponse(0) = "-1" Then
                            If returnedResponse.Length > 1 Then
                                Throw New Made4Net.Shared.M4NException(New Exception, "Invalid Attribute Value " & oLoadAtt.Name & ". " & returnedResponse(1), "Invalid Attribute Value " & oLoadAtt.Name & "." & returnedResponse(1))
                            Else
                                Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                            End If
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                        If ret = "-1" Then
                            Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Function ValidateAttributeByType(ByVal oAtt As SkuClassLoadAttribute, ByVal oAttVal As Object) As Int32
        Select Case oAtt.Type
            Case Logic.AttributeType.DateTime
                Dim Val As DateTime
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, DateTime)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.Decimal
                Dim Val As Decimal
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Decimal)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.Integer
                Dim Val As Int32
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Int32)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.String
                Try
                    If oAttVal Is Nothing Or System.Convert.ToString(oAttVal) = "" Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
        End Select
    End Function

#End Region

#End Region

End Class
