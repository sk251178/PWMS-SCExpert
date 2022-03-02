Imports Made4Net.DataAccess
Imports Made4Net.Shared.Web
Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.General.Helpers
Imports System.Data
Imports System.Collections
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports System.Web
Imports System.Collections.Generic

<CLSCompliant(False)> Public Class BatchReplenishment
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Protected _all As String
    Protected _consignee As String
    Protected _putregion As String
    Protected _sku As String
    Protected _status As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _replid As String
    Protected _REPLENPOLICY As String
    Protected _PICKREGION As String
    Protected _REPLCONTAINER As String
    Protected _WAREHOUSEAREA As String
    Protected _BReplPolicy As WMS.Logic.BatchReplenHeader
    Protected _fromload As String
    Protected _TOLOCATION As String
    Protected _FROMQTY As String
    Protected _brdstatus As String

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
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

    Public Property ReplId() As String
        Get
            Return _replid
        End Get
        Set(ByVal Value As String)
            _replid = Value
        End Set
    End Property

    Public Property FromLoad() As String
        Get
            Return _fromload
        End Get
        Set(ByVal Value As String)
            _fromload = Value
        End Set
    End Property

    Public Property TOLOCATION() As String
        Get
            Return _TOLOCATION
        End Get
        Set(ByVal Value As String)
            _TOLOCATION = Value
        End Set
    End Property

    Public Property FROMQTY() As String
        Get
            Return _FROMQTY
        End Get
        Set(ByVal Value As String)
            _FROMQTY = Value
        End Set
    End Property

    Public Property BRDStatus() As String
        Get
            Return _brdstatus
        End Get
        Set(ByVal Value As String)
            _brdstatus = Value
        End Set
    End Property

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" BATCHREPLID = {0} ", Made4Net.Shared.Util.FormatField(_replid))
        End Get
    End Property

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pReplId As String)
        _replid = pReplId
        LoadBatchReplenishTask()
    End Sub

    Public ReadOnly Property hasTask() As String
        Get
            Dim sql As String = String.Format("Select count(1) from TASKS where tasktype in ('{0}','{1}') and replenishment='{2}'", WMS.Lib.TASKTYPE.BRLETDOWN, WMS.Lib.TASKTYPE.BRUNLOAD, _replid)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property

    Public Sub New(ByVal sPutRegion As String, ByVal sSku As String, ByVal pConsignee As String)
        MyBase.New()
        Try
            If Not sPutRegion Is Nothing Then _putregion = sPutRegion Else _putregion = ""
        Catch ex As Exception
            _putregion = ""
        End Try
        Try
            If Not sSku Is Nothing Then _sku = sSku Else _sku = ""
        Catch ex As Exception
            _sku = ""
        End Try
        Try
            _all = "1"
        Catch ex As Exception
        End Try

        Try
            If Not pConsignee Is Nothing Then _consignee = pConsignee Else _consignee = ""
        Catch ex As Exception
            _consignee = ""
        End Try

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Schedule" Then
            For Each dr In ds.Tables(0).Rows
                ScheduleBatchReplenishment(dr("Pickregion"), dr("Warehousearea"), dr("consignee"), dr("replpolicy"), UserId)
            Next
        End If
        If CommandName = "CancelBatchReplenish" Then
            For Each dr In ds.Tables(0).Rows
                _replid = dr("BATCHREPLID")
                _status = dr("STATUS")
                LoadBatchReplenishTask()
                CancelBatchReplenish(UserId, dr("Pickregion"), dr("Warehousearea"), dr("consignee"), dr("REPLENPOLICY"), dr("Status"), dr("BATCHREPLID"))
            Next
        End If

    End Sub

#End Region
#Region "Methods"

#Region "Job Scheduler"

    Public Overrides Sub Execute()
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.BatchReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", _REPLENPOLICY)
        em.Add("LOCATION", "")
        em.Add("WAREHOUSEAREA", "")
        em.Add("PICKREGION", _putregion)
        em.Add("ALL", "1")
        em.Add("SKU", _sku)
        em.Add("CONSIGNEE", _consignee)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub ScheduleBatchReplenishment(ByVal pPickRegion As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pReplpolicy As String, ByVal pUserId As String)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.BReplenScheduled
        em.Add("EVENT", EventType)
        em.Add("PICKREGION", pPickRegion)
        em.Add("REPLPOLICY", pReplpolicy)
        em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLSCHEDULED)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ACTIVITYTIME", "0")
        em.Add("CONSIGNEE", pConsignee)
        em.Add("DOCUMENT", "")
        em.Add("DOCUMENTLINE", 0)
        em.Add("FROMLOAD", "")
        em.Add("FROMLOC", "")
        em.Add("FROMWAREHOUSEAREA", "")
        em.Add("FROMQTY", 0)
        em.Add("FROMSTATUS", "")
        em.Add("NOTES", "")
        em.Add("SKU", "")
        em.Add("TOLOAD", "")
        em.Add("TOLOC", "")
        em.Add("TOWAREHOUSEAREA", pWarehousearea)
        em.Add("TOQTY", 0)
        em.Add("TOSTATUS", "")
        em.Add("USERID", pUserId)
        em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ADDUSER", pUserId)
        em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("EDITUSER", pUserId)
        em.Send(WMS.Lib.Actions.Audit.BATCHREPLSCHEDULED)
    End Sub

#End Region

#Region "BatchReplenish"

    Protected Function getReplTask(ByVal pIgnoreStatus As Boolean) As ReplenishmentTask


        Dim sql As String
        If pIgnoreStatus Then
            sql = String.Format("SELECT DISTINCT TASK FROM TASKS WHERE REPLENISHMENT='{0}'", _replid)
        Else
            sql = String.Format("SELECT DISTINCT TASK FROM TASKS WHERE STATUS<>'COMPLETE' AND STATUS<>'CANCELED' AND REPLENISHMENT='{0}'", _replid)
        End If
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return Nothing
        Else
            Return New ReplenishmentTask(dt.Rows(0)("TASK"))
        End If
    End Function

    Public Sub LoadBatchReplenishTask()
        Dim sql As String = String.Format("SELECT * FROM BATCHREPLENHEADER WHERE BATCHREPLID='{0}'", _replid)
        Dim data As New DataTable
        Dim dr As DataRow

        DataInterface.FillDataset(sql, data)

        If data.Rows.Count = 0 Then
            Return
        End If

        dr = data.Rows(0)

        If Not dr.IsNull("BATCHREPLID") Then _replid = dr.Item("BATCHREPLID")
        If Not dr.IsNull("Status") Then _status = dr.Item("Status")
        If Not dr.IsNull("REPLENPOLICY") Then _REPLENPOLICY = dr.Item("REPLENPOLICY")
        If Not dr.IsNull("PICKREGION") Then _PICKREGION = dr.Item("PICKREGION")
        If Not dr.IsNull("WAREHOUSEAREA") Then _WAREHOUSEAREA = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        data.Dispose()

    End Sub

    Public Sub UpdateStatusHeader(ByVal status As String, ByVal user As String, ByVal pBatchReplID As String)
        Dim Sql = String.Format("UPDATE BATCHREPLENHEADER SET STATUS={0}, EDITDATE={1}, EDITUSER={2} Where BATCHREPLID={3} ",
        Made4Net.Shared.Util.FormatField(status),
        Made4Net.Shared.Util.FormatField(DateTime.Now),
        Made4Net.Shared.Util.FormatField(user),
        Made4Net.Shared.Util.FormatField(pBatchReplID))
        DataInterface.RunSQL(Sql)
    End Sub

    Public Sub UpdateStatusDetail(ByVal status As String, ByVal user As String, ByVal pBatchReplID As String)
        Dim Sql = String.Format("UPDATE BATCHREPLENDETAIL SET STATUS={0}, EDITDATE={1}, EDITUSER={2} Where BATCHREPLID={3} ",
        Made4Net.Shared.Util.FormatField(status),
        Made4Net.Shared.Util.FormatField(DateTime.Now),
        Made4Net.Shared.Util.FormatField(user),
        Made4Net.Shared.Util.FormatField(pBatchReplID))
        DataInterface.RunSQL(Sql)
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        Dim ts As Task = getReplTask(False)
        Dim brh As New WMS.Logic.BatchReplenHeader(_replid)
        Dim brdetails As New BatchReplenDetailCollection(_replid)
        If Not (ts.STATUS = WMS.Lib.Statuses.Task.ASSIGNED Or ts.STATUS = WMS.Lib.Statuses.Task.AVAILABLE) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect TASK Status", "Incorrect TASK Status")
        End If

        If brdetails.IsAnyBatchReplenLETDOWNStatus Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot cancel Batch Replenishment, Incorrect Status", "Cannot cancel Batch Replenishment, Incorrect Status")
        Else
            CancelBatchReplenish(pUser, brh.PICKREGION, brh.WAREHOUSEAREA, brh.CONSIGNEE, brh.REPLENPOLICY, brh.STATUS, brh.BATCHREPLID)
        End If
    End Sub


    Public Sub CancelBatchReplenish(ByVal pUserId As String, ByVal pPickRegion As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pReplpolicy As String, ByVal pStatus As String, ByVal pBatchReplID As String, Optional ByVal pCompleteTask As Boolean = True)
        Dim brdetails As New BatchReplenDetailCollection(_replid)

        If _status = WMS.Lib.Statuses.BatchReplensishment.COMPLETE Or _status = WMS.Lib.Statuses.BatchReplensishment.CANCELED Or _status = WMS.Lib.Statuses.BatchReplensishment.LETDOWN Or _status = WMS.Lib.Statuses.BatchReplensishment.UNLOAD Then

            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot cancel Replenishment, Incorrect Status", "Cannot cancel Replenishment, Incorrect Status")
        ElseIf brdetails.IsAnyBatchReplenLETDOWNStatus Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot cancel Batch Replenishment, Incorrect Status", "Cannot cancel Batch Replenishment, Incorrect Status")
        End If

        _status = WMS.Lib.Statuses.BatchReplensishment.CANCELED

        CancelBatchReplenishUnAlloc(pPickRegion, pWarehousearea, pConsignee, pReplpolicy, _status, pBatchReplID)

        UpdateStatusHeader(_status, WMS.Lib.USERS.SYSTEMUSER, pBatchReplID)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.BReplenCanceled
        em.Add("Event", EventType)
        em.Add("PICKREGION", pPickRegion)
        em.Add("WAREHOUSEAREA", pWarehousearea)
        em.Add("CONSIGNEE", pConsignee)
        em.Add("REPLPOLICY", pReplpolicy)
        em.Add("Status", pStatus)
        em.Add("BATCHREPLID", pBatchReplID)
        em.Send(WMSEvents.EventDescription(EventType))

        CancelBatchReplenishTask(Nothing, pUserId, pPickRegion, pWarehousearea, pConsignee, pReplpolicy, _status, pBatchReplID)

    End Sub

    Public Sub CancelBatchReplenishUnAlloc(ByVal pPickRegion As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pReplpolicy As String, ByVal pStatus As String, ByVal pBatchReplID As String, Optional ByVal pCancelTask As Boolean = True)
        Dim sql As String

        Dim breplendetails As New BatchReplenDetailCollection(pBatchReplID)

        Dim brd As New WMS.Logic.BatchReplenDetail

        For Each brd In breplendetails
            Try
                Dim fl As String = brd.FROMLOAD
                Dim fQty As String = brd.FROMQTY
                If fl <> "" AndAlso WMS.Logic.Load.Exists(fl) Then
                    Dim ld As New Load(fl)
                    ld.unAllocate(fQty, _edituser)
                End If
            Catch ex As Exception

            End Try
        Next

        _status = WMS.Lib.Statuses.BatchReplensishment.CANCELED

        UpdateStatusDetail(_status, WMS.Lib.USERS.SYSTEMUSER, pBatchReplID)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.BReplenUnAlloc
        em.Add("Event", EventType)
        em.Add("PICKREGION", pPickRegion)
        em.Add("WAREHOUSEAREA", pWarehousearea)
        em.Add("CONSIGNEE", pConsignee)
        em.Add("REPLPOLICY", pReplpolicy)
        em.Add("Status", pStatus)
        em.Add("BATCHREPLID", pBatchReplID)
        em.Send(WMSEvents.EventDescription(EventType))

    End Sub

    Public Sub CancelBatchReplenishTask(ByVal logger As LogHandler, ByVal pUserId As String, ByVal pPickRegion As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pReplpolicy As String, ByVal pStatus As String, ByVal pBatchReplID As String, Optional ByVal pCompleteTask As Boolean = True)
        Try

            _status = WMS.Lib.Statuses.BatchReplensishment.CANCELED

            If pCompleteTask Then
                If TaskManager.ExistBReplenTask(pBatchReplID) Then
                    Dim ts As Task = getReplTask(True)
                    If ts.STATUS <> WMS.Lib.Statuses.Task.CANCELED And ts.STATUS <> WMS.Lib.Statuses.Task.COMPLETE Then
                        ts.Cancel()
                    End If
                End If
            End If

            Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenCompleted)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLCOMPLETED)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", pBatchReplID)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", pWarehousearea)
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", _edituser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", _edituser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", _edituser)
            aq.Send(WMS.Lib.Actions.Audit.BATCHREPLCOMPLETED)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub




#End Region


#End Region

End Class
