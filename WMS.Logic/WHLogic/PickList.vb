Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections.Generic
Imports WMS.Logic
Imports System.Web
Imports Made4Net.General.Helpers


'Public Module Extension
'    <Extension()>
'    Public Function HasValue(ByVal dataRow As DataRow, ByVal columnName As String) As Boolean
'        Return IsDBNull(dataRow(columnName)) OrElse String.IsNullOrEmpty(dataRow(columnName))
'    End Function
'End Module


#Region "PickList"

<CLSCompliant(False)> Public Class Picklist

#Region "Variables"
    Protected _picklist As String
    Protected _picktype As String
    Protected _pickmethod As String
    Protected _strategyid As String
    Protected _createdate As DateTime
    Protected _plandate As DateTime
    Protected _releasedate As DateTime
    Protected _assigneddate As DateTime
    Protected _completeddate As DateTime
    Protected _status As String
    Protected _wave As String
    Protected _handelingunittype As String
    'Protected _deliverylocation As String
    'Protected _packarea As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
	Protected _pickorderstatus As String

    'Collections
    Protected _pickdetails As PicklistDetailCollection
    Protected _containercoll As ArrayList

    Protected _planStrategy As WMS.Logic.PlanStrategy
    Protected _releaseStrategy As WMS.Logic.ReleaseStrategyDetail
    Protected _pickUnalloc As New Dictionary(Of String, Integer)
    Protected _raiseError As Boolean = False
    Protected _defaultprinter As String       'RWMS-3822
#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" PICKLIST = {0} ", Made4Net.Shared.Util.FormatField(_picklist))
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal index As Int32) As PicklistDetail
        Get
            Return _pickdetails.PicklistLine(index)
        End Get
    End Property

    Public ReadOnly Property PicklistLine(ByVal pLineNumber As Int32) As PicklistDetail
        Get
            For Each pckdet As PicklistDetail In _pickdetails
                If pckdet.PickListLine = pLineNumber Then
                    Return pckdet
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property Containers() As ArrayList
        Get
            Return _containercoll
        End Get
    End Property

    Public ReadOnly Property ActiveContainer() As String
        Get
            If Not _containercoll Is Nothing Then
                For Each oCont As Container In _containercoll
                    If oCont.Status = WMS.Lib.Statuses.Container.STATUSNEW Then
                        Return oCont.ContainerId
                    End If
                Next
            End If
            Return ""
        End Get
    End Property

    Public ReadOnly Property ContainerExists(ByVal pContainerId As String) As Boolean
        Get
            For Each oCont As Container In _containercoll
                If oCont.ContainerId = pContainerId Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public Property PicklistID() As String
        Get
            Return _picklist
        End Get
        Set(ByVal Value As String)
            _picklist = Value
        End Set
    End Property

    Public Property PickType() As String
        Get
            Return _picktype
        End Get
        Set(ByVal Value As String)
            _picktype = Value
        End Set
    End Property

    Public Property PickMethod() As String
        Get
            Return _pickmethod
        End Get
        Set(ByVal Value As String)
            _pickmethod = Value
        End Set
    End Property

    Public Property StrategyId() As String
        Get
            Return _strategyid
        End Get
        Set(ByVal Value As String)
            _strategyid = Value
        End Set
    End Property
    Public Property DefaultPrinter() As String
        Get
            Return _defaultprinter
        End Get
        Set(ByVal value As String)
            _defaultprinter = value
        End Set
    End Property

    Public Property CreateDate() As DateTime
        Get
            Return _createdate
        End Get
        Set(ByVal Value As DateTime)
            _createdate = Value
        End Set
    End Property

    Public Property PlanDate() As DateTime
        Get
            Return _plandate
        End Get
        Set(ByVal Value As DateTime)
            _plandate = Value
        End Set
    End Property

    Public Property ReleasedDate() As DateTime
        Get
            Return _releasedate
        End Get
        Set(ByVal Value As DateTime)
            _releasedate = Value
        End Set
    End Property

    Public Property AssignedDate() As DateTime
        Get
            Return _assigneddate
        End Get
        Set(ByVal Value As DateTime)
            _assigneddate = Value
        End Set
    End Property

    Public Property CompletedDate() As DateTime
        Get
            Return _completeddate
        End Get
        Set(ByVal Value As DateTime)
            _completeddate = Value
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

    Public Property Wave() As String
        Get
            Return _wave
        End Get
        Set(ByVal Value As String)
            _wave = Value
        End Set
    End Property

    Public Property HandelingUnitType() As String
        Get
            Return _handelingunittype
        End Get
        Set(ByVal Value As String)
            _handelingunittype = Value
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

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
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

    Public ReadOnly Property Lines() As PicklistDetailCollection
        Get
            Return _pickdetails
        End Get
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property
	Public Property PICKORDERSTATUS() As String
        Get
            If Not _pickorderstatus Is Nothing Then
                Return _pickorderstatus
            End If
            Return ""
        End Get
        Set(ByVal Value As String)
            _pickorderstatus = Value
        End Set
    End Property
    Public ReadOnly Property isCompleted() As Boolean
        Get
            Dim pcklst As PicklistDetail
            Dim hasComp As Boolean = False
            For Each pcklst In _pickdetails
                If pcklst.Status = WMS.Lib.Statuses.Picklist.COMPLETE Then hasComp = True
                If pcklst.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And pcklst.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                    Return False
                End If
            Next
            Return hasComp
        End Get
    End Property

    Public ReadOnly Property isCanceled() As Boolean
        Get
            Dim pcklst As PicklistDetail
            For Each pcklst In _pickdetails
                If pcklst.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property
    Public ReadOnly Property GetTotalPickedQty() As Decimal
        Get
            Dim TotalPickedQty As Decimal
            For Each pcklst As PicklistDetail In _pickdetails
                TotalPickedQty = TotalPickedQty + pcklst.PickedQuantity
            Next
            Return TotalPickedQty
        End Get
    End Property
    Public ReadOnly Property IsFirstPick() As Boolean
        Get
            Dim bIsFirstPick As Boolean = True
            For Each pcklst As PicklistDetail In _pickdetails
                If pcklst.Status.ToUpper().CompareTo("COMPLETE") = 0 Then
                    bIsFirstPick = False
                    Exit For
                End If
            Next
            Return bIsFirstPick
        End Get
    End Property

    Public ReadOnly Property Started() As Boolean
        Get
            Dim pcklst As PicklistDetail
            For Each pcklst In _pickdetails
                If pcklst.Status = WMS.Lib.Statuses.Picklist.COMPLETE Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property ShouldPrintPickLabelOnPicking() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            Return _planStrategy.ShouldPrintPickLabelOnPicking(Me)
        End Get
    End Property

    Public ReadOnly Property ShouldPrintBagOutReportOnStart() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            Return _planStrategy.ShouldPrintBagOutReportOnStart(Me)
        End Get
    End Property

    Public ReadOnly Property ShouldPrintBagOutReportOnComplete() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            Return _planStrategy.ShouldPrintBagOutReportOnComplete(Me)
        End Get
    End Property
    Public ReadOnly Property ShouldPrintCaseLabel() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            Return _planStrategy.ShouldPrintCaseLabel(Me)
        End Get
    End Property
    Public ReadOnly Property ShouldPrintPickLabel() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            Return _planStrategy.ShouldPrintPickLabel(Me)
        End Get
    End Property

    Public ReadOnly Property ShouldPrintShipLabelOnPickLineCompleted() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            Return _planStrategy.ShouldPrintPickLabelOnPickLineCompleted(Me)
        End Get
    End Property

    Public ReadOnly Property ShouldPrintShipLabel() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            If _planStrategy.ShouldPrintShipLabel(Me) Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property ShouldPrintContentList() As Boolean
        Get
            If _planStrategy Is Nothing Then
                _planStrategy = New PlanStrategy(_strategyid)
            End If
            If _planStrategy.ShouldPrintContentList(Me) Then
                Return True
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property hasContainer() As Boolean
        Get
            For Each cont As Container In _containercoll
                If cont.Status <> WMS.Lib.Statuses.Container.DELIVERED Then
                    Return True
                End If
            Next
            Return False
            'If Me.Lines.Count > 1 Then
            '    Return True
            'Else
            '    If Me.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            '        Return False
            '    Else
            '        Return True
            '    End If
            'End If
        End Get
    End Property

    Public ReadOnly Property ParallelPickList() As String
        Get
            Return DataInterface.ExecuteScalar(String.Format("select parallelpickid from parallelpickdetail where picklist={0}", Made4Net.Shared.FormatField(_picklist)))
        End Get
    End Property

    Public ReadOnly Property getReleaseStrategy() As ReleaseStrategyDetail
        Get
            If _releaseStrategy Is Nothing Then
                If _planStrategy Is Nothing Then
                    _planStrategy = New PlanStrategy(_strategyid)
                End If
                Dim releasestrat As ReleaseStrategyDetail
                For Each releasestrat In _planStrategy.ReleaseStrategyDetails
                    If (releasestrat.PickType = _picktype Or releasestrat.PickType = "") Then
                        _releaseStrategy = releasestrat
                        Exit For
                    End If
                Next
            End If
            Return _releaseStrategy
        End Get
    End Property

    Public ReadOnly Property NoOfToLocations() As Integer
        Get
            Dim sql As String = String.Format("select count(distinct TOLOCATION) from PICKDETAIL where PICKLIST='{0}' and status in ('COMPLETE','RELEASED')", PicklistID)
            Return Convert.ToInt32(DataInterface.ExecuteScalar(sql))
        End Get
    End Property

    Public Shared Function Exists(ByVal picklistID As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from PICKHEADER where PICKLIST = '{0}'", picklistID)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function IsPickTaskAssigendToOtherUser(ByVal picklistID As String, ByVal userID As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from TASKS where PICKLIST = '{0}' AND USERID <>'{1}' AND ASSIGNED=1", picklistID, userID)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
	
#End Region

#Region "Constructor"
    Public Sub New(ByVal PicklistId As String, Optional ByVal LoadObject As Boolean = True)
        _picklist = PicklistId
        If LoadObject Then
            Load()
        Else
            _pickdetails = New PicklistDetailCollection(_picklist)
        End If

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim pk As String
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        Select Case CommandName.ToLower
            Case "pick"
                For Each dr In ds.Tables(0).Rows
                    _picklist = dr("PICKLIST")
                    Load()
                    Me.Pick(UserId)
                Next
            Case "cancelpicklist"
                'RWMS-2823
                For Each pk In Picking.DistinctPicks(ds.Tables(0))
                    _picklist = pk
                    Load()
                    Me.Cancel(UserId)
                Next
            Case "unallocatepicklist"
                'RWMS-2762
                For Each pk In Picking.DistinctPicks(ds.Tables(0))
                    _picklist = pk
                    Load()
                    Me.unAllocate(UserId)
                Next

            Case "cancelpicks"
                For Each dr In ds.Tables(0).Rows
                    _picklist = dr("PICKLIST")
                    Load()
                    Me.CancelLine(dr("PICKLISTLINE"), UserId)
                Next
            Case "unallocatepicks"

                For Each dr In ds.Tables(0).Rows
                    _picklist = dr("PICKLIST")
                    Load()
                    Me.unAllocateLine(dr("PICKLISTLINE"), UserId)
                Next

            Case "unpick"
                For Each dr In ds.Tables(0).Rows
                    _picklist = dr("PICKLIST")
                    Load()
                    Me.UnPickLine(dr("PICKLISTLINE"), UserId)
                Next
            Case "printlabels"
                If ds.Tables(0).Rows.Count > 0 Then
                    _picklist = ds.Tables(0).Rows(0)("PICKLIST")
                    Load()
                    PrintPickLabels("")
                End If
            Case "printshiplabels"
                If ds.Tables(0).Rows.Count > 0 Then
                    _picklist = ds.Tables(0).Rows(0)("PICKLIST")
                    Load()
                    PrintShipLabels("", True)
                    'PrintPickLabels("")
                End If
            Case "printpicklist"
                Dim ph As Picklist
                For Each dr In ds.Tables(0).Rows
                    ph = New Picklist(dr("PICKLIST"))
                    ph.Print("", Common.GetCurrentUser)
                Next
            Case "releaseorder"
                For Each dr In ds.Tables(0).Rows
                    _picklist = dr("PICKLIST")
                    Load()
                    ReleasePicklist(Common.GetCurrentUser)
                Next
        End Select
    End Sub
#End Region

#Region "Methods"

#Region "Accessors"

    Public Sub Load()
        Dim sql As String = String.Format("SELECT * FROM PICKHEADER WHERE PICKLIST='{0}'", _picklist)
        Dim data As New DataTable
        Dim dr As DataRow
        _pickdetails = New PicklistDetailCollection(_picklist)
        DataInterface.FillDataset(sql, data)
        '' Start PWMS-1238
        If data.Rows.Count = 0 Then
            Return
        End If
        dr = data.Rows(0)
        '' End PWMS-1238
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PICKTYPE") Then _picktype = dr.Item("PICKTYPE")
        If Not dr.IsNull("PICKMETHOD") Then _pickmethod = dr.Item("PICKMETHOD")
        If Not dr.IsNull("STRATEGYID") Then _strategyid = dr.Item("STRATEGYID")
        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("PLANDATE") Then _plandate = dr.Item("PLANDATE")
        If Not dr.IsNull("RELEASEDATE") Then _releasedate = dr.Item("RELEASEDATE")
        If Not dr.IsNull("ASSIGNEDDATE") Then _assigneddate = dr.Item("ASSIGNEDDATE")
        If Not dr.IsNull("COMPLETEDDATE") Then _completeddate = dr.Item("COMPLETEDDATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("WAVE") Then _wave = dr.Item("WAVE")
        If Not dr.IsNull("HANDELINGUNITTYPE") Then _handelingunittype = dr.Item("HANDELINGUNITTYPE")
        'If Not dr.IsNull("PACKAREA") Then _packarea = dr.Item("PACKAREA")
        'If Not dr.IsNull("DELIVERYLOCATION") Then _deliverylocation = dr.Item("DELIVERYLOCATION")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
		 If Not dr.IsNull("PICKORDERSTATUS") Then _pickorderstatus = dr.Item("PICKORDERSTATUS") 'RWMS-3736
        LoadContainers()
        data.Dispose()
    End Sub

    Public Sub Post(ByVal picks As PicksObjectCollection)
        Try


            _createdate = DateTime.Now

            _adddate = DateTime.Now
            _editdate = DateTime.Now
            _adduser = WMS.Lib.USERS.SYSTEMUSER
            _edituser = WMS.Lib.USERS.SYSTEMUSER
            _plandate = DateTime.Now

            _status = WMS.Lib.Statuses.Picklist.PLANNED

            _wave = picks(0).Wave
            _picktype = picks(0).PickType
            _pickmethod = picks(0).PickMethod
            _strategyid = picks(0).StrategyId

            Dim contid As String

            If picks(0).PickType = WMS.Lib.PICKTYPE.PARTIALPICK Or picks(0).PickType = WMS.Lib.PICKTYPE.PARALLELPICK Or picks(0).PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
                _handelingunittype = picks(0).ContainerType
            End If

            Dim sql As String = String.Format("Insert into pickheader (picklist,picktype,pickmethod,strategyid,createdate,plandate,status,wave,handelingunittype,adddate,adduser,editdate,edituser) values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})", Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picktype), Made4Net.Shared.Util.FormatField(_pickmethod), Made4Net.Shared.Util.FormatField(_strategyid),
                Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_plandate), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_wave), Made4Net.Shared.Util.FormatField(_handelingunittype),
                Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PicklistCreated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PCKINS)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _picklist)
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
            aq.Add("USERID", _edituser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", _edituser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", _edituser)
            aq.Send(WMS.Lib.Actions.Audit.PCKINS)

            DataInterface.RunSQL(sql)

            _pickdetails.post(picks)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub LoadContainers()
        If _containercoll Is Nothing Then _containercoll = New ArrayList
        Dim SQL As String = String.Format("select distinct tocontainer from pickdetail where picklist = '{0}' and tocontainer<>''", _picklist)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        Try
            For Each dr As DataRow In dt.Rows
                If WMS.Logic.Container.Exists(Convert.ToString(dr("tocontainer"))) Then
                    _containercoll.Add(New WMS.Logic.Container(Convert.ToString(dr("tocontainer")), False))
                End If
            Next
        Catch ex As Exception
        End Try
        dt.Dispose()
    End Sub

    Public Sub setStatus(ByVal pStatus As String, ByVal puser As String)
        Dim sql As String
        _status = pStatus
        _editdate = DateTime.Now
        _edituser = puser
        sql = String.Format("update PICKHEADER set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Pick & Cancel"

    Public Sub CompleteList(ByVal pUser As String, ByVal logger As LogHandler, Optional ByVal pCompleteTask As Boolean = True)
        If isCanceled Then
            _status = WMS.Lib.Statuses.Picklist.CANCELED
        Else
            _status = WMS.Lib.Statuses.Picklist.COMPLETE
        End If

        _completeddate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("Update PICKHEADER set status={0},completeddate={1},editdate={2},edituser={3} where {4}", Made4Net.Shared.Util.FormatField(_status),
            Made4Net.Shared.Util.FormatField(_completeddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(sql)

        If pCompleteTask Then
            If TaskManager.ExistPickTask(_picklist) Then
                Dim tm As TaskManager = TaskManager.NewTaskManagerForPicklist(_picklist)
                UpdateContainers()
                tm.Complete(logger)
                If ShouldPrintCaseLabel Then
                    Dim deltask As DeliveryTask
                    Do
                        deltask = tm.getPicklistDeliveryTask(_picklist, pUser)
                        If deltask Is Nothing Then
                            deltask = tm.getPicklistDeliveryTask(_picklist, Common.GetCurrentUser())
                        End If
                        If deltask Is Nothing Then
                            Exit Do
                        End If
                        Dim cont As Container = Nothing

                        If Not String.IsNullOrEmpty(deltask.ToContainer) Then
                            cont = New Container(deltask.ToContainer, True)
                        End If

                        deltask.Deliver(deltask.TOLOCATION, deltask.TOWAREHOUSEAREA, False, cont, logger)
                        stageCases(pUser)
                    Loop
                End If
            End If

            If _status = WMS.Lib.Statuses.Picklist.RELEASED AndAlso _picktype = WMS.Lib.PICKTYPE.PARALLELPICK Then 'And Not _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
                Dim parpcklst As New ParallelPicking(Me)
                parpcklst.setComplete(pUser)
            End If


            Dim pickListIDStr As String = ParallelPickList
            If Not String.IsNullOrEmpty(pickListIDStr) Then
                If WMS.Logic.ParallelPickList.ShouldComplete(pickListIDStr) Then
                    Dim parPick As New WMS.Logic.ParallelPicking(pickListIDStr)
                    parPick.setComplete(pUser)
                End If
            End If
        End If

        Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListCompleted)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PCKCOMPL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _picklist)
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
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(WMS.Lib.Actions.Audit.PCKCOMPL)
    End Sub

    Private Sub stageCases(ByVal pUser As String)
        If ShouldPrintCaseLabel Then
            CaseDetail.StageCases(PicklistID, pUser)
        End If
    End Sub

    Public Sub Cancel(ByVal pUser As String, Optional ByVal pCompleteTask As Boolean = True)
        Dim orgStatus As String = _status

        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot cancel picklist, incorrect status", "Cannot cancel picklist, incorrect status")
            Throw m4nEx
            ' Retrofit of PWMS-479
            ' RWMS-945 : Check is it assigned to som other user
        ElseIf CheckIfPickTaskIsAssignedToSomeOtherUser(pUser) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot cancel picklist " + _picklist + " as task is assigned to user", "Cannot cancel picklist " + _picklist + " as task is assigned to user")
            Throw m4nEx
        End If
        ' Retrofit of PWMS-479

        _pickdetails.Cancel(pUser)

        CompleteList(pUser, Nothing, pCompleteTask)

        'Call Event 52 of unallocate picklist to cancel related replenishments on the cancellation of picklists
        Dim eq As EventManagerQ = New EventManagerQ
        eq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListUnAlloc)
        eq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST)
        eq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        eq.Add("ACTIVITYTIME", "0")
        eq.Add("CONSIGNEE", "")
        eq.Add("DOCUMENT", _picklist)
        eq.Add("DOCUMENTLINE", 0)
        eq.Add("FROMLOAD", "")
        eq.Add("FROMLOC", "")
        eq.Add("FROMWAREHOUSEAREA", "")
        eq.Add("FROMQTY", 0)
        eq.Add("FROMSTATUS", orgStatus)
        eq.Add("NOTES", "")
        eq.Add("SKU", "")
        eq.Add("TOLOAD", "")
        eq.Add("TOLOC", "")
        eq.Add("TOWAREHOUSEAREA", "")
        eq.Add("TOQTY", 0)
        eq.Add("TOSTATUS", _status)
        eq.Add("USERID", _edituser)
        eq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        eq.Add("ADDUSER", _edituser)
        eq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        eq.Add("EDITUSER", _edituser)
        eq.Send(WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST)

        'Dim ad As New EventManagerQ
        'ad.Add("ACTION", WMS.Lib.Actions.Audit.CANPCKLIST)
        'ad.Add("USERID", pUser)
        'ad.Add("PICKLIST", _picklist)
        'ad.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'ad.Add("FROMSTATUS", orgStatus)
        'ad.Add("TOSTATUS", _status)
        'ad.Send(WMS.Lib.Actions.Audit.CANPCKLIST)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListCompleted)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CANPCKLIST)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _picklist)
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
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(WMS.Lib.Actions.Audit.CANPCKLIST)

    End Sub

    Public Sub CancelLine(ByVal LineNumber As Int32, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust pickdetail, incorrect status", "Cannot adjust pickdetail, incorrect status")
            Throw m4nEx
            'Retrofit PWMS-479
            ' RWMS-945 : Check is it assigned to som other user
        ElseIf CheckIfPickTaskIsAssignedToSomeOtherUser(pUser) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot cancel pickdetail " + _picklist + " as task is assigned to user", "Cannot cancel pickdetail " + _picklist + " as task is assigned to user")
            Throw m4nEx
        End If
        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot cancel full pick", "Cannot cancel full pick")
            Throw m4nEx
        End If
        Me(LineNumber).Cancel(pUser)

        If isCompleted Or isCanceled Then
            CompleteList(pUser, Nothing)
        End If
    End Sub

    Public Sub AdjustLine(ByVal LineNumber As Int32, ByVal adjusted As Decimal, ByVal puser As String)
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust pickdetail, incorrect status", "Cannot adjust pickdetail, incorrect status")
            Throw m4nEx
        End If
        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust full pick", "Cannot adjust full pick")
            Throw m4nEx
        End If
        Me(LineNumber).Adjust(adjusted, puser)

        If isCompleted Or isCanceled Then
            CompleteList(puser, Nothing)
        End If

    End Sub

    Public Sub unAllocate(ByVal puser As String)

        'Retrofit of PWMS-479
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unallocate picklist, incorrect status", "Cannot unallocate picklist, incorrect status")
            Throw m4nEx
            ' RWMS-945 : Check is it assigned to som other user
        ElseIf CheckIfPickTaskIsAssignedToSomeOtherUser(puser) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unallocate pickdetail " + _picklist + " as task is assigned to user", "Cannot unallocate pickdetail " + _picklist + " as task is assigned to user")
            Throw m4nEx
        End If
        'Retrofit of PWMS-479

        Dim orgStatus As String = _status

        _pickdetails.UnAllocate(puser)

        CompleteList(puser, Nothing)

        'Dim ad As New EventManagerQ
        'ad.Add("ACTION", WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST)
        'ad.Add("USERID", puser)
        'ad.Add("PICKLIST", _picklist)
        'ad.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'ad.Add("FROMSTATUS", orgStatus)
        'ad.Add("TOSTATUS", _status)
        'ad.Send(WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListUnAlloc)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", orgStatus)
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
        aq.Send(WMS.Lib.Actions.Audit.UNALLOCATEPICKLIST)
    End Sub

    'Retrofit of PWMS-479
    ' RWMS-945 : Check is it assigned to som other user
    Private Function CheckIfPickTaskIsAssignedToSomeOtherUser(currentUser As String) As Boolean
        Dim blnRet As Boolean
        Dim sql As String = "Select * from vAssignedPickTasks where picklist='{0}' and userId <> '{1}'"
        sql = String.Format(sql, _picklist, currentUser)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            blnRet = True
        End If
        Return blnRet
    End Function


    Public Sub unAllocateLine(ByVal LineNumber As Int32, ByVal puser As String)
        'Retrofit of PWMS-479
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust pickdetail, incorrect status", "Cannot adjust pickdetail, incorrect status")
            Throw m4nEx
            ' RWMS-945 : Check is it assigned to som other user
        ElseIf CheckIfPickTaskIsAssignedToSomeOtherUser(puser) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unallocate pickdetail " + _picklist + " as task is assigned to user", "Cannot unallocate pickdetail " + _picklist + " as task is assigned to user")
            Throw m4nEx
        End If
        'Retrofit of PWMS-479
        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unallocate full pick", "Cannot unallocate full pick")
            Throw m4nEx
        End If
        Me(LineNumber).unAllocate(puser)

        If isCompleted Or isCanceled Then
            CompleteList(puser, Nothing)
        End If

    End Sub

    Public Sub UnPickLine(ByVal LineNumber As Int32, ByVal pUser As String)
        Dim oPd As PicklistDetail = Me.Lines.PicklistLine(LineNumber)
        oPd.UnPick(pUser)
        If isCanceled Then
            CompleteList(pUser, Nothing)
        End If
    End Sub

    Public Sub Pick(ByVal puser As String)
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot pick picklist, incorrect status", "Cannot pick picklist, incorrect status")
            Throw m4nEx
        End If

        Dim orgStatus As String = _status

        Dim pck As PicklistDetail

        Dim contId As String
        If ActiveContainer = String.Empty Then
            contId = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        Else
            contId = ActiveContainer
        End If
        For Each pck In _pickdetails
            If pck.Status = WMS.Lib.Statuses.Picklist.RELEASED Or pck.Status = WMS.Lib.Statuses.Picklist.PLANNED Then
                setContainer(pck, contId, puser)
            End If
        Next

        If Me.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            _pickdetails.Pick(puser, Me.getReleaseStrategy(), True)
        Else
            _pickdetails.Pick(puser, Me.getReleaseStrategy(), False)
        End If

        If isCompleted Then
            CompleteList(puser, Nothing)
        End If

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListPick)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PCKPCKLIST)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", orgStatus)
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
        aq.Send(WMS.Lib.Actions.Audit.PCKPCKLIST)
    End Sub

    Public Sub PickLine(ByVal LineNumber As Int32, ByVal PickQty As Decimal, ByVal PickUOM As String, ByVal puser As String, ByVal oAttributeCollection As AttributesCollection)
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot pick pickdetail, incorrect status", "Cannot pick pickdetail, incorrect status")
            Throw m4nEx
        End If

        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust full pick", "Cannot adjust full pick")
            Throw m4nEx
        End If

        Dim contId As String = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        setContainer(Me(LineNumber), contId, puser)

        Me(LineNumber).Pick(PickQty, PickUOM, puser, oAttributeCollection)

        If isCompleted Or isCanceled Then
            CompleteList(puser, Nothing)
        End If
    End Sub

    Public Sub PickLine(ByVal LineNumber As Int32, ByVal PickQty As Decimal, ByVal puser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pUserPickShortType As String = "", Optional ByVal pContainerID As String = "")
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot pick pickdetail, incorrect status", "Cannot pick pickdetail, incorrect status")
            Throw m4nEx
        End If

        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust full pick", "Cannot adjust full pick")
            Throw m4nEx
        End If

        Dim contId As String
        If Not String.IsNullOrEmpty(pContainerID) Then
            contId = pContainerID
        Else
            contId = Me.ActiveContainer
            If String.IsNullOrEmpty(contId) Then
                contId = Made4Net.Shared.Util.getNextCounter("CONTAINER")
            End If
        End If

        setContainer(Me(LineNumber), contId, puser)

        If pUserPickShortType = "" Then pUserPickShortType = WMS.Logic.UserPickShort.PickPartialCreateException

        Dim hasFromLoad As Boolean = True
        Dim numOfNewLines As Integer = 0
        Dim AdjQtyBeforeAlloc As Decimal = Me(LineNumber).AdjustedQuantity
        Dim oRelStrat As WMS.Logic.ReleaseStrategyDetail = Me.getReleaseStrategy
        If String.IsNullOrEmpty(Me(LineNumber).FromLoad) Then
            If Not Me.Lines.AllocateLines(Me(LineNumber), PickQty, oRelStrat, False, numOfNewLines) Then
                hasFromLoad = False
            End If
        End If

        If Not hasFromLoad Then
            'Me(LineNumber).unAllocate(puser)
            'Dim m4nExc As New Made4Net.Shared.M4NException(New Exception(), "Line #linenumber# has insufficent inventory.", "Line #linenumber# has insufficent inventory.")
            'm4nExc.Level = M4NException.ErrorLevel.UserLevel
            'm4nExc.Params.Add("linenumber", Me(LineNumber).PickListLine)
            'Throw m4nExc
            Select Case oRelStrat.SystemPickShort
                Case WMS.Logic.SystemPickShort.PickPartialCancelException, WMS.Logic.SystemPickShort.PickZeroCancelException
                    Me(LineNumber).Cancel(WMS.Lib.USERS.SYSTEMUSER)
                Case WMS.Logic.SystemPickShort.PickPartialCreateException, WMS.Logic.SystemPickShort.PickZeroCreateException
                    Me(LineNumber).unAllocate(WMS.Lib.USERS.SYSTEMUSER)
            End Select
        Else
            Dim isFirst As Boolean = True
            Select Case pUserPickShortType
                Case WMS.Logic.UserPickShort.PickPartialLeaveOpen
                    For i As Integer = LineNumber To LineNumber + numOfNewLines
                        'the second condition of checks for picking location allocation since the adj qty changed in the allocation function...
                        If (PickQty < Me(i).AdjustedQuantity) OrElse (isFirst And PickQty < AdjQtyBeforeAlloc) Then
                            If isFirst And PickQty < AdjQtyBeforeAlloc Then
                                Me(i).Pick(PickQty, puser, oAttributeCollection, WMS.Logic.UserPickShort.PickPartialLeaveOpen, False, Nothing, "", AdjQtyBeforeAlloc, True, False)
                            Else
                                Me(i).Pick(PickQty, puser, oAttributeCollection, WMS.Logic.UserPickShort.PickPartialLeaveOpen, False, Nothing, "", Me(i).AdjustedQuantity, True, False)
                            End If
                        Else
                            Me(i).Pick(PickQty, puser, oAttributeCollection, WMS.Logic.UserPickShort.PickPartialCreateException)
                        End If
                        isFirst = False
                    Next
                Case Else
                    For i As Integer = LineNumber To LineNumber + numOfNewLines
                        'the second condition of checks for picking location allocation since the adj qty changed in the allocation function...
                        If (PickQty < Me(i).AdjustedQuantity) OrElse (isFirst And PickQty < AdjQtyBeforeAlloc) Then
                            If isFirst And PickQty < AdjQtyBeforeAlloc Then
                                Me(i).Pick(PickQty, puser, oAttributeCollection, pUserPickShortType, False, Nothing, "", AdjQtyBeforeAlloc, True)
                            Else
                                Me(i).Pick(PickQty, puser, oAttributeCollection, pUserPickShortType, False, Nothing, "", Me(i).AdjustedQuantity, True)
                            End If
                        Else
                            Me(i).Pick(PickQty, puser, oAttributeCollection, pUserPickShortType)
                        End If
                        isFirst = False
                    Next
            End Select
        End If


        If isCompleted Or isCanceled Then
            CompleteList(puser, Nothing)
        End If
    End Sub

    Public Sub Pick(ByVal pickj As PickJob, ByVal puser As String, ByVal logger As LogHandler)
        Dim unitspicked As Decimal = pickj.pickedqty
        Dim UserPickShort As Boolean = False
        If pickj.pickedqty < pickj.units Then
            UserPickShort = True
        End If
        Dim pckdet As PicklistDetail
        Dim relStrat As WMS.Logic.ReleaseStrategyDetail = Me.getReleaseStrategy()
        Dim skuObj As WMS.Logic.SKU
        If pickj.oSku Is Nothing Then
            skuObj = New WMS.Logic.SKU(pickj.consingee, pickj.sku)
            pickj.oSku = skuObj
        Else
            skuObj = pickj.oSku
        End If
        If Not logger Is Nothing Then
            Try
                logger.Write("Started Picklist.Pick ")
                logger.Write("Picklist.Pick : Consignee : " & pickj.consingee & " SKU : " & pickj.sku)
                logger.Write("Picklist.Pick : Picklist : " & pickj.picklist & " Picklistlines : " & pickj.PickDetLines.Count)
                logger.Write("Picklist.Pick : Pick Units : " & pickj.units & " Picked Qty " & pickj.pickedqty)
                logger.Write("Picklist.Pick : Picktype : " & Me.PickType)
            Catch ex As Exception
            End Try
        End If
        If skuObj.OVERPICKPCT > 0 AndAlso skuObj.OVERPICKPCT * pickj.units < pickj.pickedqty Then
            If Not logger Is Nothing Then
                logger.Write("Picklist.Pick : SKU OVERPICKPCT : " & skuObj.OVERPICKPCT)
            End If
            Throw New M4NException(New Exception(), "Cannot pick line, Invalid quantities in pick line or order line", "Cannot pick line, Invalid quantities in pick line or order line")
        End If
        Dim pickedLinesCounter As Integer = 1
        Dim _totalweight As Decimal = 0
        Dim pckTotalWeight As Decimal = 0
        'End Added for PWMS-840
        If (pickj.oAttributeCollection IsNot Nothing) AndAlso (pickj.oAttributeCollection.Count > 0) Then
            pckTotalWeight = Convert.ToDecimal(pickj.oAttributeCollection("WEIGHT"))
        End If
        For Each iLineNumber As Integer In pickj.PickDetLines 'For Each pckdet In Lines
            pckdet = PicklistLine(iLineNumber)
            If Not logger Is Nothing Then
                Try
                    logger.Write("Picklist.Pick : Now working on line number : " & pckdet.PickListLine)
                Catch ex As Exception
                End Try
            End If
            If pckdet.Status = "COMPLETE" Then
                Continue For
            End If
            If (Not pickj.oAttributeCollection Is Nothing) AndAlso (pickj.oAttributeCollection.Count > 0) AndAlso (Not String.IsNullOrEmpty(pckdet.FromLoad)) Then
                If pickj.PickDetLines.Count > 1 Then
                    For idx As Int32 = 0 To pickj.oAttributeCollection.Count - 1
                        If pickj.oAttributeCollection.Keys(idx).ToUpper = "WEIGHT" Then
                            'calculate the weight per qty here and again assign that to attriute
                            Dim ld As New Load(pckdet.FromLoad)
                            Dim totalloadweight As Decimal
                            If Math.Round(ld.GetAttribute("WEIGHT"), 2) > 0 Then
                                totalloadweight = Convert.ToDecimal(ld.GetAttribute("WEIGHT"))
                                _totalweight = totalloadweight
                                If Not pickj.oAttributeCollection(idx) Is Nothing Then
                                    Dim totalqty As Decimal = pickj.pickedqty
                                    Dim lineqty As Decimal = pckdet.Quantity
                                    Dim lineweight As Decimal = (pckTotalWeight * lineqty) / totalqty
                                    pickj.oAttributeCollection(idx) = lineweight.ToString()
                                End If
                            Else
                                If Not pickj.oAttributeCollection(idx) Is Nothing Then
                                    If _totalweight > 0 Then
                                        totalloadweight = _totalweight
                                        Dim totalqty As Decimal = pickj.pickedqty
                                        Dim lineqty As Decimal = pckdet.Quantity
                                        Dim lineweight As Decimal = (pckTotalWeight * lineqty) / totalqty
                                        pickj.oAttributeCollection(idx) = lineweight.ToString()
                                    Else
                                        Dim TWeight As String = pickj.oAttributeCollection(idx)
                                        totalloadweight = Convert.ToDecimal(TWeight)
                                        Dim totalqty As Decimal = pickj.pickedqty
                                        Dim lineqty As Decimal = pckdet.Quantity
                                        Dim lineweight As Decimal = (totalloadweight * lineqty) / totalqty
                                        pickj.oAttributeCollection(idx) = lineweight.ToString()
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            If pickj.pickedqty > 0 AndAlso Me.PickType <> WMS.Lib.PICKTYPE.FULLPICK Then
                setContainer(pckdet, pickj.container, puser)
                If WMS.Logic.Container.Exists(pickj.container) Then
                    Dim cont As New WMS.Logic.Container(pickj.container, False)
                    If Not Me.Containers.Contains(cont) Then
                        Me.Containers.Add(cont)
                    End If
                End If
            End If
            If pickj.PickDetLines.Count > 1 Or relStrat.GroupPickDetails Or Me.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                If unitspicked = 0 Then
                    pckdet.Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, relStrat)
                ElseIf pckdet.AdjustedQuantity - pckdet.PickedQuantity <= unitspicked Then
                    If Not logger Is Nothing Then
                        logger.Write("Adjusted Quantity : " & pckdet.AdjustedQuantity & " Picked Quantity : " & pckdet.PickedQuantity & " Units picked : " & unitspicked)
                    End If
                    If pickedLinesCounter < pickj.PickDetLines.Count Then
                        pckdet.Pick(pckdet.AdjustedQuantity - pckdet.PickedQuantity, puser, pickj.oAttributeCollection, "", pickj.SystemPickShort, relStrat, pickj.LabelPrinterName, -1, False, True, skuObj, pickj.BasedOnPartPickedLine)
                    Else
                        pckdet.Pick(unitspicked, puser, pickj.oAttributeCollection, "", pickj.SystemPickShort, relStrat, pickj.LabelPrinterName, -1, False, True, skuObj, pickj.BasedOnPartPickedLine)
                    End If
                    unitspicked = unitspicked - pckdet.PickedQuantity
                    If Not logger Is Nothing Then
                        logger.Write("Picklist.Pick : Total units picked : " & unitspicked)
                    End If
                    If (pckdet.PickedQuantity > 0) Then
                        pickedLinesCounter = pickedLinesCounter + 1
                    End If
                Else
                    If pickj.SystemPickShort Then
                        Dim exceptionqty As Decimal = pickj.adjustedunits - pickj.units
                        If exceptionqty > 0 Then
                            Dim ExceptionId As String = Made4Net.Shared.Util.getNextCounter("ExceptionId")
                            Dim addDate As Date = DateAndTime.Now
                            Dim code As String = String.Empty
                            Dim strSQL As String = DataInterface.ExecuteScalar(String.Format("Select count(1) from TASKS where TASKTYPE in ('{0}','{1}','{2}') and sku='{3}' and STATUS not in ('CANCELED','COMPLETE')", WMS.Lib.TASKTYPE.NEGTREPL, WMS.Lib.TASKTYPE.FULLREPL, WMS.Lib.TASKTYPE.PARTREPL, pickj.sku))
                            If strSQL = 0 Then
                                code = "S"
                            Else
                                code = "R"
                            End If

                            Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
                            If Not logger Is Nothing Then
                                logger.Write(" Proceeding to write for picklist exception history for orderID : " & pckdet.OrderId & " orderline : " & pckdet.OrderLine.ToString() & " Exception qty = " & exceptionqty.ToString())
                            End If
                            Dim sql As String = String.Format("INSERT INTO ExceptionHistory(ExceptionId,ActivityDate,Consignee,Orderid,OrderLine,Code,ExceptionQty,AddUser) values ({0},{1},{2},{3},{4},{5},{6},{7})",
                                                            Made4Net.Shared.Util.FormatField(ExceptionId), Made4Net.Shared.Util.FormatField(addDate), Made4Net.Shared.Util.FormatField(pickj.consingee),
                                                            Made4Net.Shared.Util.FormatField(pckdet.OrderId), Made4Net.Shared.Util.FormatField(pckdet.OrderLine),
                                                            Made4Net.Shared.Util.FormatField(code), Made4Net.Shared.Util.FormatField(exceptionqty), Made4Net.Shared.Util.FormatField(UserId))
                            DataInterface.RunSQL(sql)
                        End If
                    End If
                    pckdet.Pick(unitspicked, puser, pickj.oAttributeCollection, "", pickj.SystemPickShort, relStrat, pickj.LabelPrinterName, pckdet.AdjustedQuantity - pckdet.PickedQuantity, True, True, skuObj, pickj.BasedOnPartPickedLine)
                    unitspicked = unitspicked - pckdet.PickedQuantity
                    If Not logger Is Nothing Then
                        logger.Write("Picklist.Pick : Total units picked : " & unitspicked)
                    End If
                    If (pckdet.PickedQuantity > 0) Then
                        pickedLinesCounter = pickedLinesCounter + 1
                    End If
                End If
            Else
                pckdet.Pick(unitspicked, puser, pickj.oAttributeCollection, "", pickj.SystemPickShort, relStrat, pickj.LabelPrinterName, pickj.units, UserPickShort, True, skuObj, pickj.BasedOnPartPickedLine)
                unitspicked = 0
                Exit For
            End If
            'End If
            If unitspicked <= 0 AndAlso Not relStrat.GroupPickDetails Then
                Exit For
            End If

        Next
        If isCompleted Or isCanceled Then
            Dim sqlUpdatetask As String = String.Format("UPDATE TASKS SET ExecutionWareHouseArea={0} where picklist={1}", Made4Net.Shared.Util.FormatField(pickj.fromwarehousearea), Made4Net.Shared.Util.FormatField(pckdet.PickList))
            DataInterface.RunSQL(sqlUpdatetask)
            CompleteList(puser, logger)
        End If
    End Sub

    Dim sessionContainer As String = String.Empty
    Private Sub UpdateContainers()

        Dim container As String = String.Empty
        Dim pckdet As PicklistDetail
        sessionContainer = getSessionContainer()
        Dim toLocationGroups As Dictionary(Of String, List(Of PicklistDetail)) = Me.Lines _
                             .Cast(Of PicklistDetail) _
                             .Where(Function(pd) ContinerFilter(pd)) _
                             .GroupBy(Function(s) s.ToLocation) _
                             .ToDictionary(Function(x) x.Key, Function(x) x.ToList())
        If PickType = WMS.Lib.PICKTYPE.FULLPICK And toLocationGroups.Count = 1 Then
            Dim picksGroup As List(Of PicklistDetail) = toLocationGroups.FirstOrDefault().Value
            If picksGroup.Count > 1 Then
                UpdateContainerForToLocationGroups(picksGroup(0).FromLoad, picksGroup)
            End If
            Return
        End If
        Dim item As List(Of PicklistDetail)
        For Each item In toLocationGroups.Values
            If item.Count > 1 Or PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
                UpdateContainerForToLocationGroups(getContainerID(), item)
            End If
        Next
    End Sub

    Private Function getSessionContainer() As String
        Return Me.Lines _
                 .Cast(Of PicklistDetail) _
                 .Where(Function(pd) ContinerFilter(pd)) _
                 .Select(Function(det) det.ToContainer).FirstOrDefault()
    End Function

    Private Function getContainerID() As String
        If String.IsNullOrEmpty(sessionContainer) Then
            Return getNextCounter("CONTAINER")
        Else
            getContainerID = sessionContainer
            sessionContainer = String.Empty
            Return getContainerID
        End If
    End Function
	
    Private Shared Function ContinerFilter(pd As PicklistDetail) As Boolean

        If String.IsNullOrEmpty(pd.ToContainer) Then
            Return True
        Else
            Dim cont As New Container(pd.ToContainer, True)
            Return Not cont.Status.Equals("staged", StringComparison.InvariantCultureIgnoreCase)
        End If
    End Function

    Private Sub UpdateContainerForToLocationGroups(ByRef container As String, pickLinesGroup As List(Of PicklistDetail))

        Dim pckdet As PicklistDetail
        Dim load As Load

        For Each pckdet In pickLinesGroup
            If pckdet.PickedQuantity > 0 Then
                pckdet.UpdateToContainer(container)
                CreateContainer(pckdet, container, Nothing)
                load = New Load(pckdet.ToLoad)
                Dim cont As Container = New Container(pckdet.ToContainer, False)
                cont.Place(load, Common.GetCurrentUser(), True)
            End If
        Next

        If Not Me.Containers.Contains(container) Then
            Me.Containers.Add(container)
        End If
    End Sub
    Protected Sub setContainer(ByVal pck As PicklistDetail, ByVal pContainerId As String, ByVal pUser As String)
        If pck.Status = WMS.Lib.Statuses.Picklist.RELEASED Or pck.Status = WMS.Lib.Statuses.Picklist.PLANNED Or pck.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
            If Me.PickType <> WMS.Lib.PICKTYPE.FULLPICK Then
                CreateContainer(pck, pContainerId, pUser)
            End If
        End If
    End Sub

    Public Sub CreateContainer(ByVal pck As PicklistDetail, ByVal pContainerId As String, ByVal pUser As String)
        If pContainerId = "" Then pContainerId = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        pck.SetContainer(pContainerId, pUser)
        If Not Container.Exists(pck.ToContainer) Then
            Dim cnt As New Container
            cnt.ContainerId = pck.ToContainer
            cnt.HandlingUnitType = _handelingunittype
            cnt.UsageType = Container.ContainerUsageType.PickingContainer
            If _picktype = WMS.Lib.PICKTYPE.PARALLELPICK Then
                Try
                    Dim dt As New DataTable
                    Dim dr As DataRow
                    Dim sql As String = String.Format("Select top 1 parallelpickid from PARALLELPICKDETAIL where picklist = '{0}'", _picklist)
                    DataInterface.FillDataset(sql, dt)
                    dr = dt.Rows(0)
                    Dim para As New ParallelPicking(Convert.ToString(dr("parallelpickid")))
                    If Not para.ToContainer Is Nothing And Not para.ToContainer = String.Empty Then
                        If Not Container.Exists(para.ToContainer) Then
                            Dim paracnt As New Container
                            paracnt.ContainerId = para.ToContainer
                            paracnt.HandlingUnitType = para.HandlingUnitType
                            paracnt.UsageType = Container.ContainerUsageType.PickingContainer
                            paracnt.Post(pUser)
                        End If
                        cnt.OnContainer = para.ToContainer
                    End If
                Catch ex As Exception

                End Try
            End If
            cnt.Location = pck.FromLocation
            cnt.Warehousearea = pck.FromWarehousearea
            'RWMS-2075 and RWMS-1726 Commented Start
            'cnt.Post(pUser)
            'RWMS-2075 and RWMS-1726 Commented End

            'RWMS-2075 and RWMS-1726 Added Start
            cnt.Post(pUser, pck)
            'RWMS-2075 and RWMS-1726 Added End

            cnt = Nothing
        Else
            Dim cnt As New Container(pck.ToContainer, True)
            cnt.UpdateLastPickLocation(pck.FromLocation, pck.FromWarehousearea, pUser)
        End If
    End Sub

    Public Sub CloseContainer(ByVal pContainerId As String, ByVal pShouldDeliver As Boolean, ByVal pUser As String)
        If Not WMS.Logic.Container.Exists(pContainerId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Close Container - Container does not exists", "Cannot Close Container - Container does not exists")
        End If
        'Create a closed pick line with the last pick line picked qty and reopen the current line
        Dim dtOpenLines As New DataTable
        Dim destLoc As String = String.Empty
        Dim destWarehousearea As String = String.Empty
        'RWMS-2075 and RWMS-1725 Added Start
        Dim pPicklistline As Int32
        'RWMS-2075 and RWMS-1725 Added End

        Dim SQL As String = String.Format("select * from pickdetail where (status = '{0}' or status = '{1}') and tocontainer = '{2}' and picklist = '{3}'",
            WMS.Lib.Statuses.Picklist.PARTPICKED, WMS.Lib.Statuses.Picklist.RELEASED, pContainerId, _picklist)
        DataInterface.FillDataset(SQL, dtOpenLines)
        For Each dr As DataRow In dtOpenLines.Rows
            Dim oPickDet As New PicklistDetail(dr("picklist"), dr("picklistline"), True)
            oPickDet.CloseContainer(pUser)
            destLoc = dr("tolocation")
            destWarehousearea = dr("towarehousearea")
        Next
        'Closing container while picking - Should we deliver it now or at the end of the whole list
        If Not pShouldDeliver Then
            Dim oCont As New Container(pContainerId, True)
            oCont.DeliverPend(WMS.Logic.GetCurrentUser)
        Else
            If destLoc = String.Empty Then
                For Each pckdet As PicklistDetail In Me.Lines
                    If pckdet.ToContainer = pContainerId Then
                        'RWMS-2075 and RWMS-1725 Added Start
                        pPicklistline = pckdet.PickListLine
                        'RWMS-2075 and RWMS-1725 Added End

                        destLoc = pckdet.ToLocation
                        destWarehousearea = pckdet.ToWarehousearea
                        Exit For
                    End If
                Next
            End If
            'RWMS-2075 and RWMS-1725 Added Start
            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContainerClosed
            em.Add("EVENT", EventType)
            em.Add("DOCUMENT", _picklist)
            em.Add("DOCUMENTLINE", pPicklistline)
            em.Add("TOCONTAINER", pContainerId)
            em.Add("USERID", WMS.Logic.GetCurrentUser)
            em.Add("ACTIVITYTYPE", "CONTCLOSE")
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))
            'RWMS-2075 and RWMS-1725 Added End
            'If Not WMS.Logic.Picking.BagOutProcess Then
            Dim oDelTask As New DeliveryTask
                Dim oCont As New Container(pContainerId, True)
                oCont.SetDestinationLocation(destLoc, destWarehousearea, pUser)
                'Begin for RWMS-1294 and RWMS-1222
                ' oDelTask.CreateContainerDeliveryTask(pContainerId, destLoc, destWarehousearea, pUser, _picklist)
                oDelTask.CreateContainerDeliveryTask(pContainerId, destLoc, destWarehousearea, pUser, Nothing, _picklist)
            'End for RWMS-1294 and RWMS-1222
            'End If

        End If
    End Sub

#End Region

#Region "Complete Order"
    Public Sub CompleteOrder(ByVal Consignee As String, ByVal OrderId As String, ByVal UserId As String)
        If _status = WMS.Lib.Statuses.Picklist.CANCELED Or _status = WMS.Lib.Statuses.Picklist.COMPLETE Then
            Return
        Else
            Dim pck As PicklistDetail
            For Each pck In _pickdetails
                If pck.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Or pck.Status = WMS.Lib.Statuses.Picklist.PLANNED Or pck.Status = WMS.Lib.Statuses.Picklist.RELEASED Then
                    If pck.Consignee = Consignee And pck.OrderId = OrderId Then
                        pck.Cancel(UserId)
                    Else
                        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
                            pck.unAllocate(UserId)
                        End If
                    End If
                End If
            Next
            If isCompleted Or isCanceled Then
                CompleteList(UserId, Nothing)
            End If
        End If
    End Sub
#End Region

#Region "Assign & Unassign"

    Public Sub UnAssign(ByVal puser As String)
        If Not Started Then
            _assigneddate = Nothing
            _editdate = DateTime.Now
            _edituser = puser
            Dim sql As String = String.Format("Update pickheader set assigneddate={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_assigneddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(sql)
        End If
    End Sub

    Public Sub AssignUser(ByVal puser As String)
        _assigneddate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = puser
        Dim sql As String = String.Format("Update pickheader set assigneddate={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_assigneddate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Release"

    Public Sub Release()
        ' Check if tasks created , if yes release else do not change status to released
        _status = WMS.Lib.Statuses.Picklist.RELEASED
        _releasedate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = WMS.Lib.USERS.SYSTEMUSER

        Dim sql As String = String.Format("Update pickheader set status = {0},releasedate = {1},editdate = {2}, edituser = {3} where {4}",
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_releasedate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(sql)

        _pickdetails.Release()
    End Sub

    'Public Function getReleaseStrategy() As ReleaseStrategyDetail
    '    Dim strat As PlanStrategy
    '    strat = New PlanStrategy(_strategyid)
    '    Dim releasestrat As ReleaseStrategyDetail
    '    For Each releasestrat In strat.ReleaseStrategyDetails
    '        If (releasestrat.PickType = _picktype Or releasestrat.PickType = "") Then
    '            Return releasestrat
    '        End If
    '    Next
    '    Return Nothing
    'End Function

    Public Function GetMatchingStrategyDetail(ByVal pRegion As String, ByVal pUOM As String) As PlanStrategyDetail
        Dim strat As PlanStrategy
        strat = New PlanStrategy(_strategyid)
        Dim stratDetail As PlanStrategyDetail
        For Each stratDetail In strat.PlanDetails
            If (stratDetail.PickType = _picktype Or stratDetail.PickType = "") And
                        (stratDetail.PickRegion.Equals(pRegion, StringComparison.OrdinalIgnoreCase) Or stratDetail.PickRegion = "") And
                        (stratDetail.UOM.Equals(pUOM, StringComparison.OrdinalIgnoreCase) Or stratDetail.UOM = "") Then
                Return stratDetail
            End If
        Next
        Return Nothing
    End Function

    Public Function CanPickPartialUOM() As Boolean
        Dim strat As PlanStrategy
        strat = New PlanStrategy(_strategyid)
        If Not strat Is Nothing Then
            Return strat.PickPartialUom
        End If
        Return False
    End Function

    Public Sub ReleasePicklist(ByVal pUser As String)
        'If _status = WMS.Lib.Statuses.Picklist.PLANNED Then

        If _status <> WMS.Lib.Statuses.Picklist.PLANNED Then
            Throw New M4NException(New Exception(), "Can not release picklist. Incorrect status.", "Can not release picklist. Incorrect status.")
        End If
        _status = WMS.Lib.Statuses.Picklist.RELEASING
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now

        'setStatus(WMS.Lib.Statuses.Picklist.RELEASING, WMS.Lib.USERS.SYSTEMUSER)
        Dim qh As New Made4Net.Shared.QMsgSender
        qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.RELEASEPICKLIST)
        qh.Values.Add("PICKLISTID", _picklist)
        qh.Values.Add("USER", pUser)
        qh.Send("Releasor", _picklist)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListReleased)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RELEASEPICKLIST)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _picklist)
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
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(WMS.Lib.Actions.Audit.RELEASEPICKLIST)
    End Sub

    Public Sub ReleaseComplete()
        _status = WMS.Lib.Statuses.Picklist.RELEASED
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now

        setStatus(WMS.Lib.Statuses.Picklist.RELEASED, WMS.Lib.USERS.SYSTEMUSER)
    End Sub

#End Region

#Region "Print"

	
    Public Sub Print(Optional ByVal Printer As String = Nothing, Optional ByVal pUser As String = Nothing)
        '''''''''''''''Optional ByVal Language As Integer = 0,
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        If pUser Is Nothing Then pUser = WMS.Lib.USERS.SYSTEMUSER
        repType = WMS.Lib.REPORTS.PICKLISTS
        'DataInterface.FillDataset(String.Format("Select * from reports where reportname = '{0}'", repType), dt)
        'RWMS-2609 - replaced the db connection from sys to app
        DataInterface.FillDataset(String.Format("Select * from sys_reportparams where reportid = '{0}' and paramname = '{1}'", "PickList", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "PickList")
        oQsender.Add("DATASETID", "repPickList")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        'oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("PICKLIST = '{0}'", _picklist))
        oQsender.Send("Report", repType)
    End Sub

    Public Sub PrintContentList(ByVal prntr As String, ByVal pUser As String, Optional ByVal pLang As Int32 = 0)
        Dim ReprtName As String = GetContentListReoprtName()
        If ReprtName = "" Then
            ReprtName = "ContentList"
        End If
        Select Case _picktype
            Case WMS.Lib.PICKTYPE.PARTIALPICK, WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK, WMS.Lib.PICKTYPE.PARALLELPICK, WMS.Lib.PICKTYPE.FULLPICK
                Dim dt As DataTable = New DataTable
                Dim sSql As String = String.Format("SELECT DISTINCT cn.CONTAINER FROM PICKDETAIL pd inner join CONTAINER cn on cn.CONTAINER = pd.TOCONTAINER WHERE TOCONTAINER<>'' and PICKLIST ='{0}' and cn.STATUS not in ('{1}','{2}')",
                    _picklist, WMS.Lib.Statuses.Container.STAGED, WMS.Lib.Statuses.Container.DELIVERED)
                DataInterface.FillDataset(sSql, dt)
                For Each dr As DataRow In dt.Rows
                    Dim cnt As Container = New Container(dr("CONTAINER"), True)
                    ContentListPrinter.PrintContainerContentList(cnt, prntr, pLang, _picklist, pUser, ReprtName)

                Next
        End Select
    End Sub
    Public Sub PrintContentListReport(ByVal pPrinter As String, ByVal pcontentReportName As String, ByVal oLogger As WMS.Logic.LogHandler)
        Dim SQL As String
        Select Case _picktype
            Case WMS.Lib.PICKTYPE.PARTIALPICK, WMS.Lib.PICKTYPE.FULLPICK

                SQL = String.Format("SELECT DISTINCT pd.TOCONTAINER as CONTAINER,pd.TOLOCATION FROM PICKDETAIL pd  WHERE LEN(pd.TOCONTAINER) >0 and pd.PICKLIST ='{0}' GROUP BY PD.PICKLIST,PD.TOCONTAINER,PD.TOLOCATION ",
                    _picklist)

                Dim dt As New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
                Dim contid As String = String.Empty
                For Each dr As DataRow In dt.Rows
                    Try
                        contid = dr("CONTAINER")
                        If WMS.Logic.Container.Exists(contid) Then
                            Dim cnt As Container = New Container(dr("CONTAINER"), True)
                            If cnt.Status.Equals("STAGED", StringComparison.InvariantCultureIgnoreCase) Then
                                Continue For
                            End If
                            ContentListPrinter.PrintContainerContentList(cnt, pPrinter, 0, _picklist,
                                                                         Made4Net.Shared.Authentication.User.GetCurrentUser.UserName,
                                                                         pcontentReportName)
                        Else
                            oLogger.SafeWrite(String.Format("Container does not exists containerID: {0}", contid))
                        End If
                    Catch ex As Exception
                        oLogger.SafeWrite(String.Format("Error while Printing content list for container id: {0}", contid))
                        oLogger.SafeWrite(String.Format("Error Details: {0}", ex.ToString))
                    End Try
                Next

                SQL = String.Format("SELECT DISTINCT TOLOAD,TOLOCATION FROM PICKDETAIL WHERE PICKLIST ='{0}' AND (TOCONTAINER is null or TOCONTAINER ='') GROUP BY PICKLIST,TOLOAD,TOLOCATION",
                    _picklist)

                Dim dtLoads As New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset(SQL, dtLoads)
                Dim loadid As String = String.Empty
                For Each dr As DataRow In dtLoads.Rows
                    Try
                        loadid = dr("TOLOAD")
                        If WMS.Logic.Load.Exists(loadid) Then
                            Dim oLoad As New WMS.Logic.Load(loadid, True)
                            ContentListPrinter.PrintContainerContentList(oLoad, pPrinter, 0, _picklist,
                                                                         Made4Net.Shared.Authentication.User.GetCurrentUser.UserName,
                                                                         pcontentReportName)
                        Else
                            oLogger.SafeWrite(String.Format("Container does not exists containerID: {0}", loadid))
                        End If
                    Catch ex As Exception
                        oLogger.SafeWrite(String.Format("Error while Printing content list for container id: {0}", loadid))
                        oLogger.SafeWrite(String.Format("Error Details: {0}", ex.ToString))
                    End Try
                Next
        End Select
    End Sub

    Public Sub PrintContLabel(ByVal cont As String, Optional ByVal lblPrinter As String = "", Optional ByVal pIgnoreContainerStatus As Boolean = False)
        Dim lblFormat As String = GetShipLabelFormat()
        Dim cnt As Container = New Container(cont, True)
        If pIgnoreContainerStatus Or cnt.Status = WMS.Lib.Statuses.Container.DELIVERYPEND Or cnt.Status = WMS.Lib.Statuses.Container.STATUSNEW Then
            cnt.PrintShipLabel(lblPrinter, lblFormat)
        End If
    End Sub
    Public Sub PrintShipLabels(Optional ByVal lblPrinter As String = "", Optional ByVal pIgnoreContainerStatus As Boolean = False)
        Dim lblFormat As String = GetShipLabelFormat()
        Select Case _picktype
            Case WMS.Lib.PICKTYPE.FULLPICK, WMS.Lib.PICKTYPE.PARTIALPICK
                Dim dt As DataTable = New DataTable
                DataInterface.FillDataset("SELECT * FROM PICKDETAIL WHERE PICKLIST ='" & _picklist & "'", dt)
                Dim contArrList As New ArrayList
                For Each dr As DataRow In dt.Rows
                    If dr.IsDBNullOrEmpty("TOCONTAINER") Then
                        'Print Load labels for picklines with load delivery
                        If Not dr.IsDBNullOrEmpty("TOLOAD") Then
                            Dim ld As Load = New Load(dr("TOLOAD").ToString())
                            ld.PrintShippingLabelFullPicks(dr("PICKLIST"), dr("PICKLISTLINE"), lblPrinter, lblFormat)
                        End If
                    ElseIf Not contArrList.Contains(dr("TOCONTAINER")) AndAlso WMS.Logic.Container.Exists(dr("TOCONTAINER")) Then
                        'Print container labels for picklines with container delivery
                        Dim cnt As Container = New Container(dr("TOCONTAINER"), True)
                        If pIgnoreContainerStatus Or cnt.Status = WMS.Lib.Statuses.Container.DELIVERYPEND Or cnt.Status = WMS.Lib.Statuses.Container.STATUSNEW Then
                            cnt.PrintShipLabel(lblPrinter, lblFormat)
                        End If
                        contArrList.Add(cnt.ContainerId)
                    End If
                Next
            Case WMS.Lib.PICKTYPE.PARALLELPICK
                Dim dt As DataTable = New DataTable
                DataInterface.FillDataset("SELECT DISTINCT TOCONTAINER FROM PICKDETAIL WHERE TOCONTAINER<>'' and  PICKLIST ='" & _picklist & "'", dt)
                For Each dr As DataRow In dt.Rows
                    If WMS.Logic.Container.Exists(dr("TOCONTAINER")) Then
                        Dim cnt As Container = New Container(dr("TOCONTAINER"), True)
                        If pIgnoreContainerStatus Or cnt.Status = WMS.Lib.Statuses.Container.DELIVERYPEND Or cnt.Status = WMS.Lib.Statuses.Container.STATUSNEW Then
                            cnt.PrintShipLabel(lblPrinter, lblFormat)
                        End If
                    End If
                Next

        End Select
    End Sub

    Public Sub PrintPickLabels(ByVal lblPrinter As String)
        Dim lbltype As String
        Try
            lbltype = getPickLabelType()
        Catch ex As Exception

        End Try
        If Not lbltype = WMS.Lib.Release.PICKLABELTYPE.NONE Then
            PrintPickLabels(lbltype, lblPrinter)
        End If
    End Sub

    Public Sub PrintBagOutReport(ByVal reportPrinter As String, ByVal Language As Int32, ByVal pUser As String)
        Dim reptype As String
        Try
            reptype = GetBagOutReportName()
        Catch ex As Exception

        End Try
        If Not reptype = WMS.Lib.Release.PICKLABELTYPE.NONE Then
            PrintBagOutReport(reportPrinter, Language, pUser, reptype)
        End If
    End Sub

    Public Function GetPickCube() As String
        Try
            Return CType(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select cube from vPICKLISTCUBE WHERE PICKLIST = '{0}' ", _picklist)), Double).ToString("0.00")
        Catch ex As Exception
            Return "0"
        End Try
    End Function

    Public Function GetMinimumQty() As Int32
        Dim ret As Int32 = -1
        Dim sql As String = String.Format("select case when (select cmp.LARGEQUANTITY from COMPANY as cmp inner join OUTBOUNDORHEADER as obh on cmp.COMPANY = obh.TARGETCOMPANY inner join PICKDETAIL as pckd on pckd.ORDERID = obh.ORDERID where pckd.PICKLIST = '{0}') IS null then (select plnstrg.LARGEQUANTITY  from PLANSTRATEGYRELEASE plnstrg inner join PICKHEADER pckhrd on pckhrd.STRATEGYID =  plnstrg.STRATEGYID AND plnstrg.PICKTYPE = 'PARTIAL' where pckhrd.PICKLIST = '{0}') else (select cmp.LARGEQUANTITY from COMPANY as cmp inner join OUTBOUNDORHEADER as obh on cmp.COMPANY = obh.TARGETCOMPANY inner join PICKDETAIL as pckd on pckd.ORDERID = obh.ORDERID where pckd.PICKLIST = '{0}') end", _picklist)

        If Not IsDBNull(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)) Then
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        End If
        Return ret
    End Function

    Public Sub PrintPickLabels(ByVal LabelType As String, ByVal lblPrinter As String)

        'Added for RWMS-2047 and RWMS-1637
        If LabelType Is Nothing OrElse LabelType = "" Then LabelType = getPickLabelType()
        If LabelType Is Nothing OrElse LabelType = "" Then
            Throw New M4NException(New Exception(), "PICKLIST Label Not Configured.", "PICKLIST Label Not Configured.")
        Else
            If Not Made4Net.Label.LabelHandler.Factory.GetNewLableHandler().ValidateLabel(LabelType) Then
                Throw New M4NException(New Exception(), "'" + LabelType + "' Label Not Configured.", "'" + LabelType + "' Label Not Configured.")
            Else
                If lblPrinter Is Nothing Then
                    lblPrinter = ""
                End If
                'If LabelType = "" Then LabelType = getPickLabelType()
                Dim qSender As IQMsgSender = QMsgSender.Factory.Create()
                qSender.Add("LABELNAME", LabelType)
                qSender.Add("LabelType", LabelType)
                qSender.Add("PRINTER", lblPrinter)
                qSender.Add("PICKLIST", _picklist)
                Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
                ht.Hash.Add("PICKLIST", _picklist)
                qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
                qSender.Send("Label", String.Format("Picklist Pick Label ({0})", _picklist))

            End If
        End If
        'Ended for RWMS-2047 and RWMS-1637

        'If lblPrinter Is Nothing Then
        '    lblPrinter = ""
        'End If
        'If LabelType = "" Then LabelType = getPickLabelType()
        'Dim qSender As New QMsgSender
        'qSender.Add("LABELNAME", LabelType)
        'qSender.Add("LabelType", LabelType)
        'qSender.Add("PRINTER", lblPrinter)
        'qSender.Add("PICKLIST", _picklist)
        'Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        'ht.Hash.Add("PICKLIST", _picklist)
        'qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        'qSender.Send("Label", "Picklist Pick Label")
    End Sub
    Public Sub PrintBagOutReport(ByVal reportPrinter As String, ByVal Language As Int32, ByVal pUser As String, Optional ByVal pReportName As String = "BagOutSlip")
        Dim oQsender As IQMsgSender = QMsgSender.Factory.Create()
        Dim repType As String
        Dim dt As New DataTable
        repType = pReportName
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}'", repType), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        Try
            oQsender.Add("DATASETID", dt.Select("ParamName='DATASETNAME'")(0)("ParamValue"))
        Catch ex As Exception
            oQsender.Add("DATASETID", "repBagOutSlip")
        End Try

        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", reportPrinter)
            Try
                oQsender.Add("COPIES", dt.Select("ParamName='Copies'")(0)("ParamValue"))
            Catch ex As Exception
                oQsender.Add("COPIES", 1)
            End Try

            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("PICKLIST = '{0}'", _picklist))
        oQsender.Send("Report", repType)
    End Sub
    Protected Function getPickLabelType() As String
        Dim strat As New PlanStrategy(_strategyid)
        'Return strat.PickLabelType(Me)
        Return strat.PickLabelFormat(Me)
    End Function

    Public Function GetShipLabelFormat() As String
        Dim strat As New PlanStrategy(_strategyid)
        Return strat.ShipLabelFormat(Me)

    End Function

    'Gets ship label format for partial pick release strategy for the current plan strategy
    Public Function GetPartialPickShipLabelFormat() As String
        Dim strat As New PlanStrategy(_strategyid)
        Return strat.GetPartialPickLabelFormat()

    End Function

    Public Function GetContentListReoprtName() As String
        Dim strat As New PlanStrategy(_strategyid)
        Return strat.ContentListDocumentName(Me)
    End Function

    Public Function GetBagOutReportName() As String
        Dim strat As New PlanStrategy(_strategyid)
        Return strat.GetBagOutDocumentName(Me)
    End Function
    Public Function GetCaseLabel() As String
        Dim strat As New PlanStrategy(_strategyid)
        Return strat.GetCaseLabelName(Me)
    End Function

    'Begin RWMS-239
    Public Function GetPrintContentList() As Boolean
        Dim strat As New PlanStrategy(_strategyid)
        Return strat.ShouldPrintContentList(Me)
    End Function

    'End RWMS-239



#End Region

#Region "Confirmation"

    Public Function Confirmed(ByVal Confirmation As String, ByVal WareHoseuAreaConfirmation As String, ByVal PickJob As PickJob, Optional ByVal SecondConfirmation As String = Nothing) As Boolean
        Dim relStrat As ReleaseStrategyDetail
        relStrat = getReleaseStrategy()
        If Not relStrat Is Nothing Then
            Select Case relStrat.ConfirmationType
                Case WMS.Lib.Release.CONFIRMATIONTYPE.NONE
                    Return True
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOCATION
                    Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(PickJob.fromlocation, Confirmation, WareHoseuAreaConfirmation)

                    Return strConfirmationLocation.ToLower = PickJob.fromlocation.ToLower
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
                    Return Confirmation.ToLower = PickJob.fromload.ToLower
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU
                    Dim SQLSku As String = "SELECT * FROM vSKUCODE WHERE SKUCODE='" & Confirmation & "' OR SKU='" & Confirmation & "'"
                    Dim dtSkus As DataTable = New DataTable
                    Dim SkuExist As Boolean = False
                    DataInterface.FillDataset(SQLSku, dtSkus)
                    For Each dr As DataRow In dtSkus.Rows
                        If Convert.ToString(dr("SKU")).ToLower = PickJob.sku.ToLower Then
                            SkuExist = True
                        End If
                    Next
                    'Dim oSku As New SKU(PickJob.consingee, Confirmation)
                    'If oSku Is Nothing Then Return False
                    Return SkuExist 'oSku.SKU.ToLower = PickJob.sku.ToLower
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION
                    ' Check location confirmation
                    Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(PickJob.fromlocation, Confirmation, PickJob.fromwarehousearea)
                    ' Check SKU confirmation with vSKUCODE
                    Dim SQLSku As String = "SELECT * FROM vSKUCODE WHERE SKUCODE='" & SecondConfirmation & "' OR SKU='" & SecondConfirmation & "'"
                    Dim dtSkus As DataTable = New DataTable
                    Dim SkuExist As Boolean = False
                    DataInterface.FillDataset(SQLSku, dtSkus)
                    For Each dr As DataRow In dtSkus.Rows
                        If Convert.ToString(dr("SKU")).ToLower = PickJob.sku.ToLower Then
                            SkuExist = True
                        End If
                    Next
                    'Dim oSku As New SKU(PickJob.consingee, SecondConfirmation)
                    'If oSku Is Nothing Then Return False
                    Return (strConfirmationLocation.ToLower = PickJob.fromlocation.ToLower And SkuExist)
                Case WMS.Lib.Release.CONFIRMATIONTYPE.UPC
                    Try
                        Dim skuom As New SKU.SKUUOM(PickJob.consingee, PickJob.sku, PickJob.uom)
                        Return Confirmation.ToLower = skuom.EANUPC.ToLower
                    Catch ex As Exception
                        Return False
                    End Try
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKUUOM
                    Dim oSku As New SKU(PickJob.consingee, Confirmation)
                    If oSku Is Nothing Then Return False
                    Return oSku.SKU.ToLower = PickJob.sku.ToLower
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATIONUOM
                    Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(PickJob.fromlocation, Confirmation, WareHoseuAreaConfirmation)
                    Dim oSku As New SKU(PickJob.consingee, SecondConfirmation)
                    If oSku Is Nothing Then Return False
                    Return (strConfirmationLocation.ToLower = PickJob.fromlocation.ToLower And oSku.SKU.ToLower = PickJob.sku.ToLower)
            End Select
        Else
            Return True
        End If

    End Function

#End Region

#Region "Split Lists"

#Region "By Cube"

    Public Function SplitListByCube(ByVal pCube As Decimal, ByVal pUser As String) As ArrayList
        If _status = WMS.Lib.Statuses.Picklist.CANCELED Or _status = WMS.Lib.Statuses.Picklist.COMPLETE Then
            Throw New M4NException(New Exception, "Invalid picklist status - Cannot split list", "Invalid picklist status - Cannot split list")
        End If
        If _picktype = WMS.Lib.PICKTYPE.PARALLELPICK Then
            Throw New M4NException(New Exception, "Invalid Pick type- Cannot split list from parallel picklist", "Invalid Pick type- Cannot split list from parallel picklist")
        End If
        Dim PicklistIdList As New ArrayList
        Dim htPickLinesLists As Hashtable = GetDividedLinesByCube(pCube)
        'If no breaks should be made according to the cube passed as parameter then do nothing and return the current picklist
        If htPickLinesLists.Count = 1 Then
            PicklistIdList.Add(_picklist)
            Return PicklistIdList
        Else
            Return SplitList(htPickLinesLists, pUser)
        End If
    End Function

    Private Function GetDividedLinesByCube(ByVal pCube As Decimal) As Hashtable
        Dim LineListsHT As New Hashtable    'Hash table to hold all lists
        Dim tmpAL As New ArrayList          'Array list to hold the lines
        Dim iTotalCube As Decimal = 0
        Dim iCurrCube As Decimal = 0
        Dim htIndex As Int32 = 0
        For Each oLine As PicklistDetail In Lines
            If oLine.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And oLine.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                iCurrCube = Inventory.CalculateVolume(oLine.Consignee, oLine.SKU, oLine.AdjustedQuantity, oLine.UOM)
                If iTotalCube + iCurrCube < pCube Then
                    iTotalCube += iCurrCube
                    tmpAL.Add(oLine)
                Else
                    tmpAL = New ArrayList
                    iTotalCube = iCurrCube
                    htIndex += 1
                    tmpAL.Add(oLine)
                End If
                If Not LineListsHT.ContainsValue(tmpAL) Then
                    LineListsHT.Add(htIndex, tmpAL)
                End If
            End If
        Next
        Return LineListsHT
    End Function

#End Region

#Region "By UOM Quantity"

    Public Function SplitListByUOM(ByVal pUOMQty As Decimal, ByVal pUser As String) As ArrayList
        If _status = WMS.Lib.Statuses.Picklist.CANCELED Or _status = WMS.Lib.Statuses.Picklist.COMPLETE Then
            Throw New M4NException(New Exception, "Invalid picklist status - Cannot split list", "Invalid picklist status - Cannot split list")
        End If
        If _picktype = WMS.Lib.PICKTYPE.PARALLELPICK Then
            Throw New M4NException(New Exception, "Invalid Pick type- Cannot split list from parallel picklist", "Invalid Pick type- Cannot split list from parallel picklist")
        End If
        Dim PicklistIdList As New ArrayList
        Dim htPickLinesLists As Hashtable = GetDividedLinesByUOM(pUOMQty)
        'If no breaks should be made according to the cube passed as parameter then do nothing and return the current picklist
        If htPickLinesLists.Count = 1 Then
            PicklistIdList.Add(_picklist)
            Return PicklistIdList
        Else
            Return SplitList(htPickLinesLists, pUser)
        End If
    End Function

    Private Function GetDividedLinesByUOM(ByVal pUOMQty As Decimal) As Hashtable
        Dim LineListsHT As New Hashtable    'Hash table to hold all lists
        Dim tmpAL As New ArrayList          'Array list to hold the lines
        Dim iTotalQty As Decimal = 0
        Dim iCurrQty As Decimal = 0
        Dim htIndex As Int32 = 0
        Dim oSku As SKU
        For Each oLine As PicklistDetail In Lines
            If oLine.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And oLine.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                oSku = New SKU(oLine.Consignee, oLine.SKU)
                iCurrQty = oSku.ConvertUnitsToUom(oLine.UOM, oLine.AdjustedQuantity)
                If iTotalQty + iCurrQty < pUOMQty Then
                    iTotalQty += iCurrQty
                    tmpAL.Add(oLine)
                Else
                    tmpAL = New ArrayList
                    iTotalQty = iCurrQty
                    htIndex += 1
                    tmpAL.Add(oLine)
                End If
                If Not LineListsHT.ContainsValue(tmpAL) Then
                    LineListsHT.Add(htIndex, tmpAL)
                End If
            End If
        Next
        Return LineListsHT
    End Function

#End Region

#Region "By Lines"

    Public Function SplitList(ByVal pToLine As Int32, ByVal pUser As String) As String
        If _status = WMS.Lib.Statuses.Picklist.CANCELED Or _status = WMS.Lib.Statuses.Picklist.COMPLETE Then
            Throw New M4NException(New Exception, "Invalid picklist status - Cannot split list", "Invalid picklist status - Cannot split list")
        End If
        If _picktype = WMS.Lib.PICKTYPE.PARALLELPICK Then
            Throw New M4NException(New Exception, "Invalid Pick type- Cannot split list from parallel picklist", "Invalid Pick type- Cannot split list from parallel picklist")
        End If
        Dim shouldCreateTask As Boolean = False
        If _status = WMS.Lib.Statuses.Picklist.RELEASED Then
            shouldCreateTask = True
        End If
        Dim picklistId As String
        Dim oPicksCollection As New PicksObjectCollection
        Dim tmpLine As PicklistDetail
        Dim oPickObj As PicksObject
        For i As Int32 = pToLine - 1 To Lines.Count - 1
            tmpLine = Lines(i)
            If tmpLine.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And tmpLine.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                oPickObj = GetPickObjectFromLine(tmpLine)
                oPicksCollection.Add(oPickObj)
                tmpLine.unAllocate(pUser)
            End If
        Next
        picklistId = oPicksCollection.Post()
        If shouldCreateTask Then
            SendReleaseEvents(picklistId, pUser)
        End If
        Return picklistId
    End Function

    Private Shared Sub SendReleaseEvents(ByVal pPicklistId As String, ByVal pUser As String)
        Dim qh As New Made4Net.Shared.QMsgSender
        qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.RELEASEPICKLIST)
        qh.Values.Add("PICKLISTID", pPicklistId)
        qh.Values.Add("USER", pUser)
        qh.Send("Releasor", pPicklistId)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListReleased)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RELEASEPICKLIST)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", pPicklistId)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", WMS.Lib.Statuses.Picklist.PLANNED)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", WMS.Lib.Statuses.Picklist.RELEASED)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.RELEASEPICKLIST)
    End Sub

#End Region

#Region "Accessors"

    Private Function GetPickObjectFromLine(ByVal oLine As PicklistDetail) As PicksObject
        Dim oPickObj As New PicksObject
        oPickObj.PickType = _picktype
        oPickObj.PickMethod = _pickmethod
        oPickObj.StrategyId = _strategyid
        oPickObj.ContainerType = _handelingunittype
        oPickObj.Wave = _wave
        oPickObj.Consignee = oLine.Consignee
        oPickObj.SKU = oLine.SKU
        oPickObj.DeliveryLocation = oLine.ToLocation
        oPickObj.DeliveryWarehousearea = oLine.ToWarehousearea
        oPickObj.FromLocation = oLine.FromLocation
        oPickObj.FromWarehousearea = oLine.FromWarehousearea
        oPickObj.LoadId = oLine.FromLoad
        'oPickObj.LocationSortOrder = oLine.locsortorder
        oPickObj.OrderId = oLine.OrderId
        oPickObj.OrderLine = oLine.OrderLine
        oPickObj.PickRegion = oLine.PickRegion
        'oPickObj.SkuSortOrder = oLine.skusortorder
        oPickObj.Units = oLine.AdjustedQuantity
        oPickObj.UOM = oLine.UOM
        Return oPickObj
    End Function

    Private Function SplitList(ByVal htPickLinesLists As Hashtable, ByVal pUser As String) As ArrayList
        Dim isfirst As Boolean = True
        Dim shouldCreateTask As Boolean = False
        If _status = WMS.Lib.Statuses.Picklist.RELEASED Then
            shouldCreateTask = True
        End If
        Dim PicklistIdList As New ArrayList
        Dim oPicksCollection As New PicksObjectCollection
        Dim tmpLine As PicklistDetail
        Dim oPickObj As PicksObject
        Dim LinesArr As ArrayList
        Dim picklistId As String
        Dim linesEnumerator As System.Collections.IDictionaryEnumerator = htPickLinesLists.GetEnumerator()
        While linesEnumerator.MoveNext()
            'At First the original picklist was kept as the first picklist
            If Not isfirst Then
                LinesArr = linesEnumerator.Value
                For i As Int32 = 0 To LinesArr.Count - 1
                    tmpLine = LinesArr(i)
                    oPickObj = GetPickObjectFromLine(tmpLine)
                    oPicksCollection.Add(oPickObj)
                    tmpLine.unAllocate(pUser)
                Next
                picklistId = oPicksCollection.Post()
                PicklistIdList.Add(picklistId)
                If shouldCreateTask Then
                    SendReleaseEvents(picklistId, pUser)
                End If
            Else
                PicklistIdList.Add(_picklist)
                isfirst = False
            End If
        End While
        Return PicklistIdList
    End Function

#End Region

#End Region

#Region "Merge Lists"

    Public Shared Function MergeLists(ByVal pPicklistIdArray As ArrayList, ByVal pUser As String) As String
        If pPicklistIdArray.Count = 0 Then Return String.Empty
        Dim strPiclist As String
        'Load Picklists objects
        Dim PicklistArray As New ArrayList
        Dim oPickList As Picklist
        For i As Int32 = 0 To pPicklistIdArray.Count - 1
            oPickList = New Picklist(pPicklistIdArray(i))
            PicklistArray.Add(oPickList)
        Next
        ValidateMerge(PicklistArray)

        'Added for RWMS-2400 RWMS-2399
        'validate shipment
        ValidateMergeForShipment(PicklistArray)
        'validate outboundorheader.targetcompany
        ValidateMergeForTargetCompany(PicklistArray)
        'End Added RWMS-2400 RWMS-2399

        'Merge the lists
        Dim oPicksCollection As PicksObjectCollection = GetPicksCollection(pPicklistIdArray)
        'Unallocate the picklists
        UnallocateListBeforMerge(PicklistArray, pUser)
        'And post the new merged list
        If Not oPicksCollection.Count = 0 Then
            oPicksCollection.Sort(0, 0)
            strPiclist = oPicksCollection.Post()
        End If
        Return strPiclist
    End Function

    Private Shared Sub ValidateMerge(ByVal pPicklistObjectArray As ArrayList)
        Dim picktype, pickmethod, stratid As String
        picktype = CType(pPicklistObjectArray(0), Picklist).PickType
        pickmethod = CType(pPicklistObjectArray(0), Picklist).PickMethod
        stratid = CType(pPicklistObjectArray(0), Picklist).StrategyId
        For i As Int32 = 0 To pPicklistObjectArray.Count - 1
            If CType(pPicklistObjectArray(i), Picklist).StrategyId <> stratid Then
                Throw New M4NException(New Exception, "Cannot merge lists from different strategies", "Cannot merge lists from different strategies")
            End If
            If CType(pPicklistObjectArray(i), Picklist).PickMethod <> pickmethod Then
                Throw New M4NException(New Exception, "Cannot merge lists from different methods", "Cannot merge lists from different methods")
            End If
            If CType(pPicklistObjectArray(i), Picklist).PickType <> picktype Then
                Throw New M4NException(New Exception, "Cannot merge lists from different types", "Cannot merge lists from different types")
            End If
        Next
    End Sub

    'Added for RWMS-2400 RWMS-2399
    Private Shared Sub ValidateMergeForShipment(ByVal pPicklistObjectArray As ArrayList)
        Dim picklistid As String
        Dim picklistshipment As String = String.Empty
        picklistid = CType(pPicklistObjectArray(0), Picklist).PicklistID
        Dim dtShipment As New DataTable
        Dim drShipment As DataRow
        Dim SqlShipment As String = String.Format("select distinct s.SHIPMENT from SHIPMENT s inner join SHIPMENTDETAIL sd on s.SHIPMENT=sd.SHIPMENT inner join PICKDETAIL pd on sd.ORDERID=pd.ORDERID and sd.ORDERLINE=pd.ORDERLINE where pd.PICKLIST='{0}'", picklistid)
        DataInterface.FillDataset(SqlShipment, dtShipment)
        If dtShipment.Rows.Count > 1 Then
            Throw New M4NException(New Exception, "Cannot merge lists from different shipments", "Cannot merge lists from different shipments")
        ElseIf dtShipment.Rows.Count = 1 Then
            drShipment = dtShipment.Rows(0)
            picklistshipment = drShipment("SHIPMENT")
        End If

        For i As Int32 = 0 To pPicklistObjectArray.Count - 1
            Dim pcklistid As String
            Dim pcklistshipment As String = String.Empty
            pcklistid = CType(pPicklistObjectArray(i), Picklist).PicklistID
            Dim dtShip As New DataTable
            Dim drShip As DataRow
            Dim SqlShip As String = String.Format("select distinct s.SHIPMENT from SHIPMENT s inner join SHIPMENTDETAIL sd on s.SHIPMENT=sd.SHIPMENT inner join PICKDETAIL pd on sd.ORDERID=pd.ORDERID and sd.ORDERLINE=pd.ORDERLINE where pd.PICKLIST='{0}'", pcklistid)
            DataInterface.FillDataset(SqlShip, dtShip)
            If dtShip.Rows.Count > 1 Then
                Throw New M4NException(New Exception, "Cannot merge lists from different shipments", "Cannot merge lists from different shipments")
            ElseIf dtShip.Rows.Count = 1 Then
                drShip = dtShip.Rows(0)
                pcklistshipment = drShip("SHIPMENT")
            End If

            If picklistshipment <> pcklistshipment Then
                Throw New M4NException(New Exception, "Cannot merge lists from different shipments", "Cannot merge lists from different shipments")
            End If
        Next
    End Sub

    Private Shared Sub ValidateMergeForTargetCompany(ByVal pPicklistObjectArray As ArrayList)
        Dim picklistid As String
        Dim picklisttargetcompany As String = String.Empty
        picklistid = CType(pPicklistObjectArray(0), Picklist).PicklistID
        Dim dtTargetCompany As New DataTable
        Dim drTargetCompany As DataRow
        Dim SqlTargetCompany As String = String.Format("select distinct oh.TARGETCOMPANY from PICKDETAIL pd inner join outboundordetail od on pd.ORDERID=od.ORDERID and pd.ORDERLINE=od.ORDERLINE inner join OUTBOUNDORHEADER oh on od.ORDERID=oh.ORDERID where pd.PICKLIST='{0}'", picklistid)
        DataInterface.FillDataset(SqlTargetCompany, dtTargetCompany)
        If dtTargetCompany.Rows.Count > 1 Then
            Throw New M4NException(New Exception, "Cannot merge lists from different OUTBOUNDORHEADER.TARGETCOMPANY", "Cannot merge lists from different OUTBOUNDORHEADER.TARGETCOMPANY")
        ElseIf dtTargetCompany.Rows.Count = 1 Then
            drTargetCompany = dtTargetCompany.Rows(0)
            picklisttargetcompany = drTargetCompany("TARGETCOMPANY")
        End If

        For i As Int32 = 0 To pPicklistObjectArray.Count - 1
            Dim pcklistid As String
            Dim pcklisttargetcompany As String = String.Empty
            pcklistid = CType(pPicklistObjectArray(i), Picklist).PicklistID
            Dim dtTargetComp As New DataTable
            Dim drTargetComp As DataRow
            Dim SqlTargetComp As String = String.Format("select distinct oh.TARGETCOMPANY from PICKDETAIL pd inner join outboundordetail od on pd.ORDERID=od.ORDERID and pd.ORDERLINE=od.ORDERLINE inner join OUTBOUNDORHEADER oh on od.ORDERID=oh.ORDERID where pd.PICKLIST='{0}'", pcklistid)
            DataInterface.FillDataset(SqlTargetComp, dtTargetComp)
            If dtTargetComp.Rows.Count > 1 Then
                Throw New M4NException(New Exception, "Cannot merge lists from different OUTBOUNDORHEADER.TARGETCOMPANY", "Cannot merge lists from different OUTBOUNDORHEADER.TARGETCOMPANY")
            ElseIf dtTargetComp.Rows.Count = 1 Then
                drTargetComp = dtTargetComp.Rows(0)
                pcklisttargetcompany = drTargetComp("TARGETCOMPANY")
            End If

            If picklisttargetcompany <> pcklisttargetcompany Then
                Throw New M4NException(New Exception, "Cannot merge lists from different OUTBOUNDORHEADER.TARGETCOMPANY", "Cannot merge lists from different OUTBOUNDORHEADER.TARGETCOMPANY")
            End If
        Next
    End Sub
    'End Added RWMS-2400 RWMS-2399

    Private Shared Sub UnallocateListBeforMerge(ByVal pPicklistObjectArray As ArrayList, ByVal pUser As String)
        Dim oPickList As Picklist
        For i As Int32 = 0 To pPicklistObjectArray.Count - 1
            oPickList = pPicklistObjectArray(i)
            oPickList.unAllocate(pUser)
        Next
    End Sub

    Private Shared Function GetPicklistWhereCondition(ByVal pPicklistIdArray As ArrayList) As String
        Dim strLists As String = String.Empty
        For i As Int32 = 0 To pPicklistIdArray.Count - 1
            strLists &= "'" & pPicklistIdArray(i) & "',"
        Next
        strLists = strLists.TrimEnd(",".ToCharArray())
        Return strLists
    End Function

    Private Shared Function GetPicksCollection(ByVal pPicklistIdArray As ArrayList) As PicksObjectCollection
        'Get all rows in the selected picklists
        Dim dtDetails As New DataTable
        Dim strLists As String = GetPicklistWhereCondition(pPicklistIdArray)
        Dim Sql As String = String.Empty
        Sql = String.Format("select ph.pickmethod,ph.picktype,ph.strategyid,ph.wave,isnull(ph.handelingunittype,'') as handelingunittype, pd.*,loc.locsortorder,sku.picksortorder as skusortorder from pickdetail pd inner join location loc on " &
        "loc.location = pd.fromlocation and loc.warehousearea = pd.fromwarehousearea inner join pickheader ph on ph.picklist = pd.picklist " &
        "inner join sku on sku.sku = pd.sku and sku.consignee = pd.consignee " &
        "where pd.status not in('{0}','{1}') and pd.picklist in ({2})", WMS.Lib.Statuses.Picklist.CANCELED, WMS.Lib.Statuses.Picklist.COMPLETE, strLists)
        DataInterface.FillDataset(Sql, dtDetails)
        'Build the objects collection
        Dim oPicksCollection As New PicksObjectCollection()
        Dim oPickObj As PicksObject
        For Each dr As DataRow In dtDetails.Rows
            oPickObj = GetPickObjectFromRow(dr)
            If Not oPickObj Is Nothing Then
                oPicksCollection.Add(oPickObj)
            End If
        Next
        Return oPicksCollection
    End Function

    Private Shared Function GetPickObjectFromRow(ByVal dr As DataRow) As PicksObject
        Dim oPickObj As PicksObject
        If Not dr Is Nothing Then
            oPickObj = New PicksObject
            If Not IsDBNull(dr("PickType")) Then oPickObj.PickType = dr("PickType")
            If Not IsDBNull(dr("PickMethod")) Then oPickObj.PickMethod = dr("PickMethod")
            If Not IsDBNull(dr("StrategyId")) Then oPickObj.StrategyId = dr("StrategyId")
            If Not IsDBNull(dr("handelingunittype")) Then oPickObj.ContainerType = dr("handelingunittype")
            If Not IsDBNull(dr("consignee")) Then oPickObj.Consignee = dr("consignee")
            If Not IsDBNull(dr("sku")) Then oPickObj.SKU = dr("sku")
            If Not IsDBNull(dr("tolocation")) Then oPickObj.DeliveryLocation = dr("tolocation")
            If Not IsDBNull(dr("towarehousearea")) Then oPickObj.DeliveryWarehousearea = dr("towarehousearea")
            If Not IsDBNull(dr("fromlocation")) Then oPickObj.FromLocation = dr("fromlocation")
            If Not IsDBNull(dr("FromWarehousearea")) Then oPickObj.FromWarehousearea = dr("FromWarehousearea")
            If Not IsDBNull(dr("fromload")) Then oPickObj.LoadId = dr("fromload")
            If Not IsDBNull(dr("locsortorder")) Then oPickObj.LocationSortOrder = dr("locsortorder")
            If Not IsDBNull(dr("orderid")) Then oPickObj.OrderId = dr("orderid")
            If Not IsDBNull(dr("orderline")) Then oPickObj.OrderLine = dr("orderline")
            If Not IsDBNull(dr("PickRegion")) Then oPickObj.PickRegion = dr("PickRegion")
            If Not IsDBNull(dr("skusortorder")) Then oPickObj.SkuSortOrder = dr("skusortorder")
            If Not IsDBNull(dr("adjqty")) Then oPickObj.Units = dr("adjqty")
            If Not IsDBNull(dr("uom")) Then oPickObj.UOM = dr("uom")
        End If
        Return oPickObj
    End Function

#End Region

#End Region

End Class
#End Region

#Region "PicklistCollection"
<CLSCompliant(False)> Public Class PicklistDetailCollection
    Implements ICollection

#Region "Variables"

    Protected _picklist As String
    Protected _pcks As ArrayList

#End Region

#Region "Properties"

    Public ReadOnly Property PicklistId() As String
        Get
            Return _picklist
        End Get
    End Property

    Default Public Property Item(ByVal index As Int32) As PicklistDetail
        Get
            Return CType(_pcks(index), PicklistDetail)
        End Get
        Set(ByVal Value As PicklistDetail)
            _pcks(index) = Value
        End Set
    End Property

    Public Property PicklistLine(ByVal index As Int32) As PicklistDetail
        Get
            Dim pck As PicklistDetail
            For Each pck In Me
                If pck.PickListLine = index Then
                    Return pck
                End If
            Next
            Return Nothing
        End Get
        Set(ByVal value As PicklistDetail)
            Dim pck As PicklistDetail
            For Each pck In Me
                If pck.PickListLine = index Then
                    pck = value
                End If
            Next
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal Picklist As String)
        _picklist = Picklist
        _pcks = New ArrayList
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("Select * from pickdetail where picklist = '{0}' order by picklistline", _picklist)
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            _pcks.Add(New PicklistDetail(dr))
        Next
        dt.Dispose()
    End Sub

#End Region

#Region "Methods"

    Public Sub post(ByVal picks As PicksObjectCollection)
        Me.Add(picks)
        Post()
    End Sub

    Public Sub Post()
        Dim pick As PicklistDetail
        Dim idx As Int32 = 1
        For Each pick In Me
            pick.Post(idx)
            idx = idx + 1
        Next
    End Sub

    Public Sub Cancel(ByVal puser As String)
        Dim pcklistdet As PicklistDetail
        For Each pcklistdet In Me
            Try
                pcklistdet.Cancel(puser)
            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Sub UnAllocate(ByVal puser As String)
        Dim pcklistdet As PicklistDetail
        Dim dt As New DataTable
        Dim sql As String = String.Format("select oo.* from outboundorheader oo inner join pickdetail pd on oo.consignee = pd.consignee and oo.orderid = pd.orderid where picklist={0} ",
        Made4Net.Shared.FormatField(_picklist))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        Dim ordDict As New Dictionary(Of String, OutboundOrderHeader)
        Dim ordHeader As OutboundOrderHeader
        For Each pcklistdet In Me
            Try
                If Not ordDict.ContainsKey(pcklistdet.Consignee & pcklistdet.OrderId) Then
                    ordDict.Add(pcklistdet.Consignee & pcklistdet.OrderId, New OutboundOrderHeader(dt.Select(String.Format("CONSIGNEE={0} and ORDERID={1}", Made4Net.Shared.FormatField(pcklistdet.Consignee), Made4Net.Shared.FormatField(pcklistdet.OrderId)))(0)))
                End If
                ordHeader = ordDict(pcklistdet.Consignee & pcklistdet.OrderId)
                pcklistdet.unAllocate(puser, ordHeader)
                ordDict(pcklistdet.Consignee & pcklistdet.OrderId) = ordHeader
            Catch ex As Exception

            End Try
        Next

        'For Each pcklistdet In Me
        '    Try
        '        pcklistdet.unAllocate(puser)
        '    Catch ex As Exception

        '    End Try
        'Next
    End Sub

    Public Sub Pick(ByVal puser As String, ByVal pReleaseStrategy As ReleaseStrategyDetail, ByVal pIsFullPick As Boolean)
        Dim pcklistdet As PicklistDetail
        Dim currentLine As Integer = 1
        Dim unallocatedLinesList As New System.Collections.Generic.List(Of Integer)
        While currentLine <= Me.Count
            For i As Integer = currentLine To Me.Count
                pcklistdet = Me.PicklistLine(i)
                If Not String.IsNullOrEmpty(pcklistdet.FromLoad) OrElse pcklistdet.Status = WMS.Lib.Statuses.Picklist.COMPLETE OrElse
                                pcklistdet.Status = WMS.Lib.Statuses.Picklist.CANCELED Then
                    Continue For
                End If
                Exit For
            Next
            Dim linesAdded As Integer = 0
            If String.IsNullOrEmpty(pcklistdet.FromLoad) AndAlso Not AllocateLines(pcklistdet, pcklistdet.AdjustedQuantity, pReleaseStrategy, pIsFullPick, linesAdded) Then
                Select Case pReleaseStrategy.SystemPickShort
                    Case WMS.Logic.SystemPickShort.PickPartialCancelException, WMS.Logic.SystemPickShort.PickZeroCancelException
                        pcklistdet.Cancel(WMS.Lib.USERS.SYSTEMUSER)
                    Case WMS.Logic.SystemPickShort.PickPartialCreateException, WMS.Logic.SystemPickShort.PickZeroCreateException
                        pcklistdet.unAllocate(WMS.Lib.USERS.SYSTEMUSER)
                End Select
                unallocatedLinesList.Add(pcklistdet.PickListLine)
            Else
                If InventoryAttributeBase.LineExists(InventoryAttributeBase.AttributeKeyType.Load, pcklistdet.FromLoad, "", "") Then
                    Picking.ValidateLine(pcklistdet.Consignee, pcklistdet.SKU, pcklistdet.AdjustedQuantity - pcklistdet.PickedQuantity, puser, New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Load, pcklistdet.FromLoad).Attributes)
                Else
                    Picking.ValidateLine(pcklistdet.Consignee, pcklistdet.SKU, pcklistdet.AdjustedQuantity - pcklistdet.PickedQuantity, puser, Nothing)
                End If
            End If
            currentLine = pcklistdet.PickListLine + linesAdded + 1
        End While

        For Each picklistdet As PicklistDetail In Me
            If Not unallocatedLinesList.Contains(picklistdet.PickListLine) Then
                If picklistdet.AdjustedQuantity < picklistdet.Quantity Then
                    picklistdet.Pick(puser, Nothing, True)
                Else
                    picklistdet.Pick(puser, Nothing, False)
                End If
            End If
        Next
    End Sub

    Public Sub Release()
        Dim pcklistdet As PicklistDetail
        For Each pcklistdet In Me
            Try
                pcklistdet.Release()
            Catch ex As Exception

            End Try
        Next
    End Sub

#Region "Overrides"

    Public Function Add(ByVal value As PicklistDetail) As Integer
        Return _pcks.Add(value)
    End Function

    Public Function Add(ByVal value As PicksObjectCollection) As Integer
        Dim pck As PicksObject
        For Each pck In value
            Add(pck)
        Next
        Return 0
    End Function

    Public Function Add(ByVal value As PicksObject) As Integer
        Dim pck As New PicklistDetail(_picklist, value)
        pck.SubSKUConversionUnits = value.SubSKUConversionUnits
        Return _pcks.Add(pck)
    End Function

    Public Sub Clear()
        _pcks.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As PicklistDetail)
        _pcks.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As PicklistDetail)
        _pcks.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _pcks.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _pcks.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _pcks.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _pcks.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _pcks.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _pcks.GetEnumerator
    End Function

#End Region

    Public Function AllocateLines(ByVal pPickJob As PickJob, ByVal pReleaseStrategy As ReleaseStrategyDetail, ByVal pIsFullPick As Boolean) As Boolean

        Dim loadsAvailableUnitsDictionary As System.Collections.Generic.Dictionary(Of String, Decimal)
        If pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCancelException OrElse
        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCreateException OrElse
        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroLeaveOpen OrElse pIsFullPick Then
            loadsAvailableUnitsDictionary = findLoadsForAllocation(pPickJob.units, pPickJob.PickDetLines(0), False)
        Else
            loadsAvailableUnitsDictionary = findLoadsForAllocation(pPickJob.units, pPickJob.PickDetLines(0), True)
        End If

        Dim pckDet As PicklistDetail
        Dim oSku As WMS.Logic.SKU
        Dim unitsSum As Decimal = 0
        Dim dTotalAllocationQty As Decimal = 0
        Dim usedLoadsDictionary As New System.Collections.Generic.Dictionary(Of String, Decimal)
        Dim usedLoadsList As New List(Of String)
        Dim newPickDetailsByPreviousDetailLine As New Dictionary(Of Integer, List(Of PicklistDetail))
        For i As Integer = 0 To pPickJob.PickDetLines.Count - 1
            pckDet = Me.PicklistLine(pPickJob.PickDetLines(i))
            Dim oOldDetailUOM As String = pckDet.UOM
            Dim dOrigUnits As Decimal = pckDet.Quantity
            For Each pair As System.Collections.Generic.KeyValuePair(Of String, Decimal) In loadsAvailableUnitsDictionary
                If pair.Value > 0 Then
                    If pair.Value <= pckDet.AdjustedQuantity - unitsSum Then
                        unitsSum = unitsSum + pair.Value
                        usedLoadsDictionary.Add(pair.Key, pair.Value)
                        usedLoadsList.Add(pair.Key)
                    Else
                        usedLoadsDictionary.Add(pair.Key, pckDet.AdjustedQuantity - unitsSum)
                        unitsSum = unitsSum + (pckDet.AdjustedQuantity - unitsSum)
                        usedLoadsList.Add(pair.Key)
                    End If
                    If unitsSum = pckDet.AdjustedQuantity Then
                        If pPickJob.fromload <> pair.Key Then
                            pPickJob.fromload = ""
                        End If
                        Exit For
                    End If
                Else
                    pPickJob.fromload = ""
                End If
            Next
            Dim newDetailsList As New List(Of PicklistDetail)
            If usedLoadsList.Count > 0 AndAlso (pPickJob.fromload = String.Empty OrElse pPickJob.fromload = usedLoadsList(0)) Then
                If pPickJob.fromload = String.Empty Then pPickJob.fromload = usedLoadsList(0)
                allocateLoadsToLine(usedLoadsDictionary, usedLoadsList, newDetailsList, pckDet, False, pReleaseStrategy.SystemPickShort)
            Else ' 0309 Changes
                Select Case pReleaseStrategy.SystemPickShort.ToLower()
                    Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower(), WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()
                        '' don't do anything.
                        'PickLoc.GetPickLoc(pPickJob.fromlocation, pPickJob.consingee, pPickJob.sku).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pLoadsDictionary(pLoadsList(0)), WMS.Lib.USERS.SYSTEMUSER)
                    Case Else
                        PickLoc.GetPickLoc(pPickJob.fromlocation, pPickJob.fromwarehousearea, pPickJob.consingee, pPickJob.sku).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pckDet.Quantity, WMS.Lib.USERS.SYSTEMUSER)
                End Select
            End If

            'Dim oSku As WMS.Logic.SKU
            If Not oOldDetailUOM.Equals(pckDet.UOM, StringComparison.OrdinalIgnoreCase) OrElse dOrigUnits <> pckDet.AdjustedQuantity Then
                If pPickJob.oSku Is Nothing Then
                    oSku = New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)
                    pPickJob.oSku = oSku
                Else
                    oSku = pPickJob.oSku
                End If
                pPickJob.adjustedunits -= (dOrigUnits - pckDet.AdjustedQuantity)
                pPickJob.uom = pckDet.UOM
                pPickJob.uomunits = oSku.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
            End If
            If newDetailsList.Count > 0 Then
                newPickDetailsByPreviousDetailLine.Add(pckDet.PickListLine, newDetailsList)
            End If
            For Each pair As KeyValuePair(Of String, Decimal) In usedLoadsDictionary
                loadsAvailableUnitsDictionary(pair.Key) -= pair.Value
            Next
            usedLoadsDictionary.Clear()
            usedLoadsList.Clear()
            dTotalAllocationQty += unitsSum
            unitsSum = 0
        Next

        If dTotalAllocationQty = 0 Then
            Return False
        End If

        If pPickJob.units > dTotalAllocationQty Then
            If oSku Is Nothing Then  ' Could have initialized in the previous validation section
                oSku = New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)
            End If
            pPickJob.units = dTotalAllocationQty
            'pPickJob.adjustedunits = dTotalAllocationQty
            pPickJob.uomunits = oSku.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
            pPickJob.SystemPickShort = True
        End If
        For Each pair As KeyValuePair(Of Integer, List(Of PicklistDetail)) In newPickDetailsByPreviousDetailLine
            insertLinesToCollection(pair.Value, pair.Key)
        Next
        Return True
    End Function
    Public Function AllocateLines(ByVal pPickJob As PickJob, ByVal pReleaseStrategy As ReleaseStrategyDetail, ByVal pIsFullPick As Boolean, ByRef pLoads As List(Of String)) As Boolean
        Dim loadsAvailableUnitsDictionary As System.Collections.Generic.Dictionary(Of String, Decimal)
        If pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCancelException OrElse
        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCreateException OrElse
        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroLeaveOpen OrElse pIsFullPick Then
            loadsAvailableUnitsDictionary = findLoadsForAllocation(pPickJob.units, pPickJob.PickDetLines(0), False)
        Else
            loadsAvailableUnitsDictionary = findLoadsForAllocation(pPickJob.units, pPickJob.PickDetLines(0), True)
        End If
        Dim pckDet As PicklistDetail
        Dim oSku As WMS.Logic.SKU
        Dim unitsSum As Decimal = 0
        Dim dTotalAllocationQty As Decimal = 0
        Dim usedLoadsDictionary As New System.Collections.Generic.Dictionary(Of String, Decimal)
        Dim usedLoadsList As New List(Of String)
        Dim newPickDetailsByPreviousDetailLine As New Dictionary(Of Integer, List(Of PicklistDetail))
        Dim updatePckJobDetails As New Dictionary(Of Integer, PicklistDetail)
        For i As Integer = 0 To pPickJob.PickDetLines.Count - 1
            pckDet = Me.PicklistLine(pPickJob.PickDetLines(i))
            Dim oOldDetailUOM As String = pckDet.UOM
            Dim dOrigUnits As Decimal = pckDet.Quantity
            For Each pair As System.Collections.Generic.KeyValuePair(Of String, Decimal) In loadsAvailableUnitsDictionary
                If pair.Value > 0 Then
                    If pair.Value <= pckDet.AdjustedQuantity - unitsSum Then
                        unitsSum = unitsSum + pair.Value
                        usedLoadsDictionary.Add(pair.Key, pair.Value)
                        usedLoadsList.Add(pair.Key)
                    Else
                        usedLoadsDictionary.Add(pair.Key, pckDet.AdjustedQuantity - unitsSum)
                        unitsSum = unitsSum + (pckDet.AdjustedQuantity - unitsSum)
                        usedLoadsList.Add(pair.Key)
                    End If
                    If unitsSum >= pckDet.AdjustedQuantity Then
                        If pPickJob.fromload <> pair.Key Then
                            pPickJob.fromload = ""
                        End If
                        Exit For
                    End If
                Else
                    pPickJob.fromload = ""
                End If
            Next
            Dim newDetailsList As New List(Of PicklistDetail)
            If usedLoadsList.Count > 0 AndAlso (pPickJob.fromload = String.Empty OrElse pPickJob.fromload = usedLoadsList(0)) Then
                If pPickJob.fromload = String.Empty Then pPickJob.fromload = usedLoadsList(0)
                allocateLoadsToLine(usedLoadsDictionary, usedLoadsList, newDetailsList, pckDet, False, pReleaseStrategy.SystemPickShort)
            Else
                Select Case pReleaseStrategy.SystemPickShort.ToLower()
                    Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower(), WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()
                    Case Else
                        PickLoc.GetPickLoc(pPickJob.fromlocation, pPickJob.fromwarehousearea, pPickJob.consingee, pPickJob.sku).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pckDet.Quantity, WMS.Lib.USERS.SYSTEMUSER)
                End Select
            End If
            If newDetailsList.Count > 0 Then
                newPickDetailsByPreviousDetailLine.Add(pckDet.PickListLine, newDetailsList)
            End If
            For Each pair As KeyValuePair(Of String, Decimal) In usedLoadsDictionary
                loadsAvailableUnitsDictionary(pair.Key) -= pair.Value
            Next
            usedLoadsDictionary.Clear()
            pLoads = usedLoadsList
            usedLoadsList.Clear()
            dTotalAllocationQty += unitsSum
            unitsSum = 0
            updatePckJobDetails.Add(pckDet.PickListLine, pckDet)
        Next
        'Save the updated pickdetail in picklist
        updateLinesToCollection(updatePckJobDetails)
        If dTotalAllocationQty = 0 Then
            Return False
        End If
        If pPickJob.units > dTotalAllocationQty Then
            If oSku Is Nothing Then
                oSku = New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)
            End If
            pPickJob.units = dTotalAllocationQty
            'pPickJob.adjustedunits = dTotalAllocationQty
            pPickJob.uomunits = oSku.ConvertUnitsToUom(pPickJob.uom, pPickJob.units)
            pPickJob.SystemPickShort = True
        End If
        Dim prevadjqty As Decimal = 0
        Dim totalprevadjqty As Decimal = 0
        Dim newpicklinescount As Integer = 0
        For Each pair As KeyValuePair(Of Integer, List(Of PicklistDetail)) In newPickDetailsByPreviousDetailLine
            insertLinesToCollection(pair.Value, pair.Key + newpicklinescount)
            newpicklinescount += 1

            AdjustPickJobLines(pPickJob, pair.Key + newpicklinescount, pair.Value.Count)
        Next
        'Reset pick job fromload 
        If String.IsNullOrEmpty(pPickJob.fromload) Then
            For Each iLineNumber As Integer In pPickJob.PickDetLines
                pckDet = PicklistLine(iLineNumber)
                If (Not String.IsNullOrEmpty(pckDet.FromLoad)) Then
                    pPickJob.fromload = pckDet.FromLoad
                    Exit For
                End If
            Next
        End If
        Return True
    End Function
    Sub AdjustPickJobLines(ByRef pPickJob As PickJob, ByVal newLineAdded As Integer, ByVal numoflinesAdded As Integer)
        Dim newPickJobLines As New System.Collections.Generic.List(Of Integer)
        For index As Integer = newLineAdded To (newLineAdded + numoflinesAdded - 1)
            newPickJobLines.Add(index)
        Next
        For i As Integer = 0 To pPickJob.PickDetLines.Count - 1
            If pPickJob.PickDetLines(i) < newLineAdded Then
                newPickJobLines.Add(pPickJob.PickDetLines(i))
            ElseIf pPickJob.PickDetLines(i) >= newLineAdded Then
                newPickJobLines.Add(pPickJob.PickDetLines(i) + numoflinesAdded)
            End If
        Next

        pPickJob.PickDetLines.Clear()
        For i As Integer = 0 To newPickJobLines.Count - 1
            pPickJob.PickDetLines.Add(newPickJobLines(i))
        Next
    End Sub
    Public Function AllocateLines(ByVal pPickDetail As PicklistDetail, ByVal pNeededQty As Decimal,
    ByVal pReleaseStrategy As ReleaseStrategyDetail, ByVal pIsFullPick As Boolean, ByRef pNumOfNewLines As Integer) As Boolean
        Dim loadsAvailableUnitsDictionary As System.Collections.Generic.Dictionary(Of String, Decimal)
        If pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCancelException OrElse
                    pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCreateException OrElse
                    pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroLeaveOpen OrElse pIsFullPick Then
            loadsAvailableUnitsDictionary = findLoadsForAllocation(pNeededQty, pPickDetail.PickListLine, False)
        Else
            loadsAvailableUnitsDictionary = findLoadsForAllocation(pNeededQty, pPickDetail.PickListLine, True)
        End If

        'Means we can not pick anything for this pickjob and we need a new one.
        If loadsAvailableUnitsDictionary.Count = 0 Then
            Return False
        End If

        'Dim pckDet As PicklistDetail
        Dim unitsSum As Decimal = 0
        Dim usedLoadsDictionary As New System.Collections.Generic.Dictionary(Of String, Decimal)
        Dim usedLoadsList As New List(Of String)

        'In tihs method this dictionary will only have one line because we have only one pickdetail.
        Dim newPickDetailsByPreviousDetailLine As New Dictionary(Of Integer, List(Of PicklistDetail))

        For Each pair As System.Collections.Generic.KeyValuePair(Of String, Decimal) In loadsAvailableUnitsDictionary
            If pair.Value > 0 Then
                If pair.Value <= pNeededQty - unitsSum Then
                    unitsSum = unitsSum + pair.Value
                    usedLoadsDictionary.Add(pair.Key, pair.Value)
                    usedLoadsList.Add(pair.Key)
                Else
                    usedLoadsDictionary.Add(pair.Key, pNeededQty - unitsSum)
                    unitsSum = unitsSum + (pNeededQty - unitsSum)
                    usedLoadsList.Add(pair.Key)
                End If

                If unitsSum = pNeededQty Then
                    Exit For
                End If
            End If
        Next

        Dim newDetailsList As New List(Of PicklistDetail)
        allocateLoadsToLine(usedLoadsDictionary, usedLoadsList, newDetailsList, pPickDetail, False, pReleaseStrategy.SystemPickShort)
        If newDetailsList.Count > 0 Then
            newPickDetailsByPreviousDetailLine.Add(pPickDetail.PickListLine, newDetailsList)
        End If
        For Each pair As KeyValuePair(Of String, Decimal) In usedLoadsDictionary
            loadsAvailableUnitsDictionary(pair.Key) -= pair.Value
        Next
        usedLoadsDictionary.Clear()
        usedLoadsList.Clear()
        unitsSum = 0

        'Only one pickdetail so we can use its value.count method to get how many new lines were added.
        For Each pair As KeyValuePair(Of Integer, List(Of PicklistDetail)) In newPickDetailsByPreviousDetailLine
            insertLinesToCollection(pair.Value, pair.Key)
            pNumOfNewLines = pair.Value.Count
        Next
        Return True
    End Function

    Private Sub allocateLoadsToLine(ByVal pLoadsDictionary As Dictionary(Of String, Decimal), ByVal pLoadsList As List(Of String),
                ByRef pNewPickDetails As List(Of PicklistDetail), ByVal pCurrentPickDetail As PicklistDetail, ByVal pPickPartialUom As Boolean,
                ByVal pSystemPickShort As String)

        Dim dTotalAllocatedQty As Decimal = -1
        Dim oLoad As New WMS.Logic.Load(pLoadsList(0), True)
        If oLoad.LOADUOM = pCurrentPickDetail.UOM AndAlso oLoad.UNITS - oLoad.UNITSALLOCATED >= pCurrentPickDetail.Quantity Then
            Select Case pSystemPickShort.ToLower()
                Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower(), WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()
                    PickLoc.GetPickLoc(oLoad.LOCATION, oLoad.WAREHOUSEAREA, oLoad.CONSIGNEE, oLoad.SKU).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pLoadsDictionary(pLoadsList(0)), WMS.Lib.USERS.SYSTEMUSER)
                Case Else
                    PickLoc.GetPickLoc(oLoad.LOCATION, oLoad.WAREHOUSEAREA, oLoad.CONSIGNEE, oLoad.SKU).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pCurrentPickDetail.Quantity, WMS.Lib.USERS.SYSTEMUSER)
            End Select
            pCurrentPickDetail.AllocateLoad(oLoad, oLoad.LOADUOM, pLoadsDictionary(pLoadsList(0)), WMS.Lib.USERS.SYSTEMUSER)
        Else
            If pPickPartialUom Then
                Select Case pSystemPickShort.ToLower()
                    Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower(), WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()
                        PickLoc.GetPickLoc(oLoad.LOCATION, oLoad.WAREHOUSEAREA, oLoad.CONSIGNEE, oLoad.SKU).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pLoadsDictionary(pLoadsList(0)), WMS.Lib.USERS.SYSTEMUSER)
                    Case Else
                        PickLoc.GetPickLoc(oLoad.LOCATION, oLoad.WAREHOUSEAREA, oLoad.CONSIGNEE, oLoad.SKU).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pCurrentPickDetail.Quantity, WMS.Lib.USERS.SYSTEMUSER)
                End Select
                pCurrentPickDetail.AllocateLoad(oLoad, oLoad.LOADUOM, pLoadsDictionary(pLoadsList(0)), WMS.Lib.USERS.SYSTEMUSER)
            Else
                Dim oSKU As New SKU(oLoad.CONSIGNEE, oLoad.SKU)
                Dim currentUom As String
                If Not oLoad.LOADUOM.Equals(pCurrentPickDetail.UOM) Then
                    If oSKU.isChildOf(pCurrentPickDetail.UOM, oLoad.LOADUOM) Then
                        currentUom = oLoad.LOADUOM
                    Else
                        currentUom = pCurrentPickDetail.UOM
                    End If
                Else
                    currentUom = oLoad.LOADUOM
                End If
                Dim neededQty As Decimal = pLoadsDictionary(oLoad.LOADID)
                Dim pickDetail As PicklistDetail = pCurrentPickDetail
                While (True)
                    Dim unitsperUom As Decimal = oSKU.ConvertToUnits(currentUom)
                    Dim currentUomQuantity = Math.Floor(neededQty / unitsperUom) * unitsperUom
                    If currentUomQuantity > 0 AndAlso currentUomQuantity <= neededQty Then
                        Select Case pSystemPickShort.ToLower()
                            Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower(), WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()
                                PickLoc.GetPickLoc(oLoad.LOCATION, oLoad.WAREHOUSEAREA, oLoad.CONSIGNEE, oLoad.SKU).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, currentUomQuantity, WMS.Lib.USERS.SYSTEMUSER)
                            Case Else
                                PickLoc.GetPickLoc(oLoad.LOCATION, oLoad.WAREHOUSEAREA, oLoad.CONSIGNEE, oLoad.SKU).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pCurrentPickDetail.Quantity, WMS.Lib.USERS.SYSTEMUSER)
                        End Select
                        pickDetail.AllocateLoad(oLoad, currentUom, currentUomQuantity, WMS.Lib.USERS.SYSTEMUSER)
                        neededQty -= currentUomQuantity
                        If dTotalAllocatedQty < 0 Then
                            dTotalAllocatedQty = currentUomQuantity
                        Else
                            dTotalAllocatedQty += currentUomQuantity
                        End If
                        If (dTotalAllocatedQty > 0) Then
                            pickDetail.AdjustedQuantity = currentUomQuantity
                        End If

                        If neededQty = 0 Then
                            Exit While
                        Else
                            pickDetail.AdjustedQuantity = currentUomQuantity

                            Dim newPickLine As PicklistDetail = pickDetail.SplitLine(pickDetail.Quantity - pickDetail.AdjustedQuantity)
                            newPickLine.UOM = oSKU.getNextUom(currentUom)
                            pNewPickDetails.Add(newPickLine)
                            pickDetail = newPickLine
                        End If
                    End If
                    currentUom = oSKU.getNextUom(currentUom)
                End While
            End If
        End If
        pLoadsList.RemoveAt(0)
        If (pCurrentPickDetail.Quantity - pCurrentPickDetail.AdjustedQuantity) > 0 Then
            Dim newPickLine As PicklistDetail
            If pCurrentPickDetail.Quantity - pCurrentPickDetail.AdjustedQuantity > 0 Then
                newPickLine = pCurrentPickDetail.SplitLine(pCurrentPickDetail.Quantity - pCurrentPickDetail.AdjustedQuantity)
            End If
            If pLoadsList.Count > 0 Then
                pNewPickDetails.Add(newPickLine)
                allocateLoadsToLine(pLoadsDictionary, pLoadsList, pNewPickDetails, newPickLine, pPickPartialUom, pSystemPickShort)
            End If

        End If
    End Sub
    Private Sub insertLinesToCollection(ByVal pNewLinesToAdd As List(Of PicklistDetail), ByVal pPreviousLine As Integer)
        Dim currentLineNumber As Integer = pPreviousLine
        For i As Integer = 0 To pNewLinesToAdd.Count - 1
            currentLineNumber += 1
            pNewLinesToAdd(i).PickListLine = currentLineNumber
            _pcks.Insert(currentLineNumber - 1, pNewLinesToAdd(i))
        Next
        For i As Integer = currentLineNumber To _pcks.Count - 1
            currentLineNumber += 1
            CType(_pcks(i), PicklistDetail).PickListLine = currentLineNumber
        Next
        For i As Integer = 0 To _pcks.Count - 1
            CType(_pcks(i), PicklistDetail).Save(WMS.Lib.USERS.SYSTEMUSER)
        Next
    End Sub
    Private Sub updateLinesToCollection(ByVal updatedPickDetails As Dictionary(Of Integer, PicklistDetail))

        For Each pair As KeyValuePair(Of Integer, PicklistDetail) In updatedPickDetails
            For i As Integer = 0 To _pcks.Count - 1
                If CType(_pcks(i), PicklistDetail).PickListLine = pair.Key Then
                    _pcks(i) = pair.Value
                    Exit For
                End If
            Next
        Next
        For i As Integer = 0 To _pcks.Count - 1
            CType(_pcks(i), PicklistDetail).Save(WMS.Lib.USERS.SYSTEMUSER)
        Next
    End Sub
    Private Function findLoadsForAllocation(ByVal pUnits As Decimal, ByVal pPickListLine As Integer, ByVal pManyLoadsAllowed As Boolean) As System.Collections.Generic.Dictionary(Of String, Decimal)
        Dim loadsAvailableUnitsDictionary As New System.Collections.Generic.Dictionary(Of String, Decimal)
        Dim AvailableLoads As New DataTable()
        'Added Order by availableunits desc for PWMS-846
        Dim sql As String = String.Format("select distinct loadid,availableunits ,LASTMOVEDATE from vPickLocAllocationInventory where picklist = {0} and " &
        "picklistline = {1} Order by LASTMOVEDATE ASC,availableunits desc", Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(pPickListLine))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, AvailableLoads)
        'Ended Order by availableunits desc for PWMS-846
        Dim sumOfUnitsInLoads As Decimal = 0
        Dim picklist As Picklist = New Picklist(_picklist)
        Dim pickDetail As PicklistDetail = picklist.Lines.PicklistLine(pPickListLine)
        Dim oOrder As New OutboundOrderHeader(pickDetail.Consignee, pickDetail.OrderId)
        Dim addLoadsToDictionary As Boolean = True
        For Each dr As DataRow In AvailableLoads.Rows
            Dim availUnits As Decimal = Decimal.Parse(dr("availableUnits"))
            Dim loadId As String = (dr("Loadid"))
            Dim oLoad As New Load(loadId)
            If Not oLoad.ValidShelfLife(oOrder.TARGETCOMPANY) Then
                Continue For
            End If
            If addLoadsToDictionary Then
                sumOfUnitsInLoads += availUnits
                loadsAvailableUnitsDictionary.Add(dr("Loadid"), dr("AvailableUnits"))
                If sumOfUnitsInLoads >= pUnits Then
                    If pManyLoadsAllowed Then
                        Return loadsAvailableUnitsDictionary
                    End If
                    addLoadsToDictionary = False
                End If
            End If

        Next
        If pManyLoadsAllowed Then
            Return loadsAvailableUnitsDictionary
        Else
            loadsAvailableUnitsDictionary.Clear()
            Return loadsAvailableUnitsDictionary
        End If
    End Function

#End Region

End Class
#End Region

#Region "PicklistDetail"

<CLSCompliant(False)> Public Class PicklistDetail

#Region "Variables"

#Region "Primary Keys"
    Protected _picklist As String
    Protected _picklistline As Int32
#End Region

#Region "Other Fields"
    Protected _pickregion As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _orderid As String = String.Empty
    Protected _orderline As Int32
    Protected _status As String = String.Empty
    Protected _sku As String = String.Empty
    Protected _uom As String = String.Empty
    Protected _qty As Decimal
    Protected _adjqty As Decimal
    Protected _pickedqty As Decimal
    Protected _fromlocation As String = String.Empty
    Protected _fromwarehousearea As String = String.Empty
    Protected _fromload As String = String.Empty
    Protected _toload As String = String.Empty
    Protected _towarehousearea As String = String.Empty
    Protected _tocontainer As String = String.Empty
    Protected _tolocation As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    '' Used when using substitute SKU
    Protected _subSKUConversionUnits As Decimal = 1
    Protected _picktype As String = String.Empty 'Property added instead of getting it from db each time.
    Protected _caseIDs As List(Of String) = New List(Of String)
    Public Shared CaseLabelPrinter As String

#End Region
#End Region

#Region "Properties"
    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" PICKLIST = '{0}' And PICKLISTLINE = {1}", _picklist, _picklistline)
        End Get
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
    Public Property PickRegion() As String
        Get
            Return _pickregion
        End Get
        Set(ByVal Value As String)
            _pickregion = Value
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
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
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
    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property
    Public Property Quantity() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal Value As Decimal)
            _qty = Value
        End Set
    End Property
    Public Property AdjustedQuantity() As Decimal
        Get
            Return _adjqty
        End Get
        Set(ByVal Value As Decimal)
            _adjqty = Value
        End Set
    End Property
    Public Property PickedQuantity() As Decimal
        Get
            Return _pickedqty
        End Get
        Set(ByVal Value As Decimal)
            _pickedqty = Value
        End Set
    End Property
    Public Property FromLocation() As String
        Get
            Return _fromlocation
        End Get
        Set(ByVal Value As String)
            _fromlocation = Value
        End Set
    End Property
    Public Property FromWarehousearea() As String
        Get
            Return _fromwarehousearea
        End Get
        Set(ByVal Value As String)
            _fromwarehousearea = Value
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
    Public Property ToLoad() As String
        Get
            Return _toload
        End Get
        Set(ByVal Value As String)
            _toload = Value
        End Set
    End Property
    Public Property ToContainer() As String
        Get
            Return _tocontainer
        End Get
        Set(ByVal Value As String)
            _tocontainer = Value
        End Set
    End Property
    Public Property ToLocation() As String
        Get
            Return _tolocation
        End Get
        Set(ByVal Value As String)
            _tolocation = Value
        End Set
    End Property
    Public Property ToWarehousearea() As String
        Get
            Return _towarehousearea
        End Get
        Set(ByVal Value As String)
            _towarehousearea = Value
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
    Public ReadOnly Property PickType() As String
        Get
            If _picktype = "" Then
                Dim SQL As String = String.Format("SELECT isnull(picktype,'') FROM PICKHEADER WHERE (PICKLIST = '{0}')", _picklist)
                _picktype = DataInterface.ExecuteScalar(SQL)
            End If
            Return _picktype
        End Get
    End Property

    Public Property SubSKUConversionUnits() As Decimal
        Get
            Return _subSKUConversionUnits
        End Get
        Set(ByVal value As Decimal)
            _subSKUConversionUnits = value
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New()

    End Sub

    Public Sub New(ByVal Picklistid As String, ByVal pck As PicksObject)
        _picklist = Picklistid
        _consignee = pck.Consignee
        _sku = pck.SKU
        _orderid = pck.OrderId
        _orderline = pck.OrderLine
        _qty = pck.Units
        _uom = pck.UOM
        _fromload = pck.LoadId
        _fromlocation = pck.FromLocation
        'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
        '_tolocation = pck.DeliveryLocation
        'Commented for Retrofit Item PWMS-748 (RWMS-439) End

        'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
        _tolocation = pck.ToLocation
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        _fromwarehousearea = pck.FromWarehousearea
        _towarehousearea = pck.DeliveryWarehousearea
        If _tolocation = "" Then
            'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
            '_tolocation = pck.ToLocation
            'Commented for Retrofit Item PWMS-748 (RWMS-439) End

            'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
            _tolocation = pck.DeliveryLocation
            'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        End If
        _pickregion = pck.PickRegion
        If _towarehousearea = "" Then
            _towarehousearea = pck.ToWarehousearea
        End If
        'Check if the current pick is over allocated from pickloc, and update the pickloc over allocated field as required.
        If pck.IsPickLocAllocation Then
            'Dim sSql As String = String.Format("select * from vpickloc where consignee = '{0}' and sku = '{1}' and location = '{2}'", _consignee, _sku, _fromlocation)
            'DataInterface.RunSQL(sSql)
            Dim oPickloc As PickLoc = Planner.GetPickingLocation(_fromlocation, _fromwarehousearea, _consignee, _sku)
            oPickloc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.ADDQTY, _qty, WMS.Lib.USERS.SYSTEMUSER)
            Planner.SetPickingLocation(oPickloc)
        End If
    End Sub

    Public Sub New(ByVal PicklistID As String, ByVal Picklistline As Int32, Optional ByVal LoadObject As Boolean = True)
        _picklist = PicklistID
        _picklistline = Picklistline
        If LoadObject Then load()
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub load()
        Dim SQL As String = "SELECT * FROM PICKDETAIL WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            'Exception
        End If

        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PICKLISTLINE") Then _picklistline = dr.Item("PICKLISTLINE")
        If Not dr.IsNull("PICKREGION") Then _pickregion = dr.Item("PICKREGION")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERLINE") Then _orderline = dr.Item("ORDERLINE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("UOM") Then _uom = dr.Item("UOM")
        If Not dr.IsNull("QTY") Then _qty = dr.Item("QTY")
        If Not dr.IsNull("ADJQTY") Then _adjqty = dr.Item("ADJQTY")
        If Not dr.IsNull("PICKEDQTY") Then _pickedqty = dr.Item("PICKEDQTY")
        If Not dr.IsNull("FROMLOCATION") Then _fromlocation = dr.Item("FROMLOCATION")
        If Not dr.IsNull("FROMWAREHOUSEAREA") Then _fromwarehousearea = dr.Item("FROMWAREHOUSEAREA")
        If Not dr.IsNull("FROMLOAD") Then _fromload = dr.Item("FROMLOAD")
        If Not dr.IsNull("TOLOAD") Then _toload = dr.Item("TOLOAD")
        If Not dr.IsNull("TOCONTAINER") Then _tocontainer = dr.Item("TOCONTAINER")
        If Not dr.IsNull("TOLOCATION") Then _tolocation = dr.Item("TOLOCATION")
        If Not dr.IsNull("TOWAREHOUSEAREA") Then _towarehousearea = dr.Item("TOWAREHOUSEAREA")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Public Sub Post(ByVal idx As Int32)

        _editdate = DateTime.Now
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _adddate = DateTime.Now
        _adduser = WMS.Lib.USERS.SYSTEMUSER

        _picklistline = idx
        _adjqty = _qty
        _pickedqty = 0
        _status = WMS.Lib.Statuses.Picklist.PLANNED
        _tocontainer = ""
        _toload = ""

        Dim sql As String = String.Format("Insert into pickdetail(picklist,picklistline,pickregion,consignee,orderid,orderline,status," &
            "sku,uom,qty,adjqty,pickedqty,fromlocation,fromwarehousearea, fromload,toload,tocontainer,tolocation,towarehousearea,adddate,adduser,editdate,edituser) values (" &
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22})", Made4Net.Shared.Util.FormatField(_picklist),
            Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_pickregion), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline),
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_pickedqty),
            Made4Net.Shared.Util.FormatField(_fromlocation),
            Made4Net.Shared.Util.FormatField(_fromwarehousearea),
            Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_tocontainer),
            Made4Net.Shared.Util.FormatField(_tolocation),
            Made4Net.Shared.Util.FormatField(_towarehousearea),
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PicklistLineCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PCKLNINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", _fromlocation)
        aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
        aq.Add("FROMQTY", _qty)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _toload)
        aq.Add("TOLOC", _tolocation)
        aq.Add("TOWAREHOUSEAREA", _towarehousearea)
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        DataInterface.RunSQL(sql)
        aq.Send(WMS.Lib.Actions.Audit.PCKLNINS)

        If _fromload <> String.Empty Then       'From Load can be blank if over allocation has taken...
            Dim load As New WMS.Logic.Load(_fromload)
            load.Allocate(_qty, WMS.Lib.USERS.SYSTEMUSER)
        End If
        'Added for RWMS-1645 and RWMS-1508
        'Dim ordHeader As OutboundOrderHeader = Planner.GetOutboundOrder(_consignee, _orderid)
        Dim ordHeader As OutboundOrderHeader = Nothing
        'Ended for RWMS-1645 and RWMS-1508
        If ordHeader Is Nothing Then
            ordHeader = New OutboundOrderHeader(_consignee, _orderid)
        End If
        allocateOrderLine(ordHeader)
        'Dim ordetail As New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(_consignee, _orderid, _orderline)
        'ordetail.Allocate(_qty, WMS.Lib.USERS.SYSTEMUSER)
    End Sub

    Private Sub allocateOrderLine(ByVal pOrdHeader As OutboundOrderHeader)
        Dim origLine As OutboundOrderHeader.OutboundOrderDetail = pOrdHeader.Lines.Line(_orderline)
        If origLine.SKU.Equals(_sku, StringComparison.OrdinalIgnoreCase) Then
            origLine.Allocate(_qty, WMS.Lib.USERS.SYSTEMUSER)
            Return
        End If



        Dim od As OutboundOrderHeader.OutboundOrderDetail = getAllocationOrderLine(pOrdHeader, _sku, origLine)

        If od Is Nothing Then
            pOrdHeader.Lines.CreateNewLine(_consignee, _orderid, origLine.REFERENCEORDLINE, _sku, origLine.INVENTORYSTATUS, _qty, origLine.INPUTSKU, origLine.NOTES,
            origLine.ROUTE, origLine.STOPNUMBER, WMS.Lib.USERS.PLANNER, origLine.EXPLOADEDFLAG, origLine.Attributes.Attributes, origLine.WAREHOUSEAREA, origLine.INPUTQTY, origLine.INPUTUOM)

            od = pOrdHeader.Lines.Item(pOrdHeader.Lines.Count - 1)
            od.UpdateOriginalSku(origLine.SKU, WMS.Lib.USERS.PLANNER)

            If Not String.IsNullOrEmpty(origLine.Wave) Then
                Dim wv As Wave = Planner.GetWave()
                If wv Is Nothing Then
                    wv = New Wave(origLine.Wave)
                End If
                wv.AssignOrderLine(pOrdHeader.CONSIGNEE, pOrdHeader.ORDERID, od.ORDERLINE, True, WMS.Lib.USERS.SYSTEMUSER)
                od.Wave = wv.WAVE
            End If
        Else
            od.EditDetail(od.CONSIGNEE, od.ORDERID, od.ORDERLINE, od.REFERENCEORDLINE, od.SKU, od.INVENTORYSTATUS, od.QTYMODIFIED + _qty,
            od.NOTES, od.ROUTE, od.STOPNUMBER, WMS.Lib.USERS.PLANNER, od.EXPLOADEDFLAG, od.Attributes.Attributes, od.WAREHOUSEAREA, od.INPUTSKU, od.INPUTQTY,
            od.INPUTUOM)
        End If

        If Not String.IsNullOrEmpty(origLine.Shipment) Then
            Dim ship As New Shipment(origLine.Shipment)
            Dim origShipDet As Shipment.ShipmentDetail = ship.ShipmentDetails.getShipmentDetail(origLine.CONSIGNEE, origLine.ORDERID, origLine.ORDERLINE, WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER)
            ship.AssignOrder(pOrdHeader.CONSIGNEE, pOrdHeader.ORDERID, od.ORDERLINE, _qty, origShipDet.LOADINGSEQ, WMS.Lib.USERS.SYSTEMUSER)
            od.Shipment = ship.SHIPMENT
            '' Reduce the quantity that was moved to the new line
            origShipDet.AddQauntity(-_qty / _subSKUConversionUnits, WMS.Lib.USERS.SYSTEMUSER)
        End If


        od.Allocate(_qty, WMS.Lib.USERS.PLANNER)

        Dim updatedQty As Decimal = origLine.QTYMODIFIED - _qty / _subSKUConversionUnits

        pOrdHeader.Lines.Line(origLine.ORDERLINE).EditDetail(origLine.CONSIGNEE, origLine.ORDERID, origLine.ORDERLINE, origLine.REFERENCEORDLINE, origLine.SKU, origLine.INVENTORYSTATUS, updatedQty, origLine.NOTES, origLine.ROUTE, origLine.STOPNUMBER,
        WMS.Lib.USERS.SYSTEMUSER, origLine.EXPLOADEDFLAG, origLine.Attributes.Attributes, origLine.WAREHOUSEAREA, origLine.INPUTSKU, origLine.INPUTQTY, origLine.INPUTUOM)

        _orderline = od.ORDERLINE
        Me.Save(WMS.Lib.USERS.SYSTEMUSER)
    End Sub

    '' If it returns nothing, it means we need to create a new line.
    ''Private Function getAllocationOrderLine(ByVal pOrdHeader As OutboundOrderHeader, ByVal pSKU As String, ByVal pOriginalSKU As String, ByVal pReferenceOrderLine As Integer, ByVal pOrigLine As OutboundOrderHeader.OutboundOrderDetail)
    'For Each det As OutboundOrderHeader.OutboundOrderDetail In pOrdHeader.Lines
    '    If det.SKU.Equals(pSKU, StringComparison.OrdinalIgnoreCase) AndAlso _
    '    Not String.IsNullOrEmpty(det.ORIGINALSKU) AndAlso det.ORIGINALSKU.Equals(pOriginalSKU, StringComparison.OrdinalIgnoreCase) _
    '    AndAlso det.ADDUSER.Equals(WMS.Lib.USERS.PLANNER, StringComparison.OrdinalIgnoreCase) AndAlso det.REFERENCEORDLINE = pReferenceOrderLine AndAlso _
    '    det.QTYLEFTTOFULLFILL + det.QTYALLOCATED > 0 Then
    '        Return det
    '    End If
    'Next
    'Return Nothing
    'End Function
    '' If it returns nothing, it means we need to create a new line.
    Private Function getAllocationOrderLine(ByVal pOrdHeader As OutboundOrderHeader, ByVal pSKU As String, ByVal pOrigLine As OutboundOrderHeader.OutboundOrderDetail)
        For Each det As OutboundOrderHeader.OutboundOrderDetail In pOrdHeader.Lines
            If det.SKU.Equals(pSKU, StringComparison.OrdinalIgnoreCase) AndAlso
            Not String.IsNullOrEmpty(det.ORIGINALSKU) AndAlso det.ORIGINALSKU.Equals(pOrigLine.SKU, StringComparison.OrdinalIgnoreCase) _
            AndAlso det.ADDUSER.Equals(WMS.Lib.USERS.PLANNER, StringComparison.OrdinalIgnoreCase) AndAlso det.REFERENCEORDLINE = pOrigLine.REFERENCEORDLINE AndAlso
            det.QTYLEFTTOFULLFILL + det.QTYALLOCATED > 0 AndAlso det.Shipment.Equals(pOrigLine.Shipment, StringComparison.OrdinalIgnoreCase) AndAlso
            det.Wave.Equals(pOrigLine.Wave, StringComparison.OrdinalIgnoreCase) Then
                Return det
            End If
        Next
        Return Nothing
    End Function

    Public Sub Save(ByVal pUser As String)
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String

        Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()

        Dim activitystatus As String
        'PWMS-800(RWMS-824) - added the QTY coulmn in the update Statement.
        If WMS.Logic.PicklistDetail.Exists(_picklist, _picklistline) Then
            'Update
            SQL = String.Format("UPDATE PICKDETAIL SET  PICKREGION ={0}, CONSIGNEE ={1}, ORDERID ={2}, ORDERLINE ={3}, STATUS ={4}, SKU ={5}, UOM ={6}, ADJQTY ={7}, " &
                "PICKEDQTY ={8}, FROMLOCATION ={9}, FROMWAREHOUSEAREA ={17}, FROMLOAD ={10}, TOLOAD ={11}, TOCONTAINER ={12}, TOLOCATION ={13}, TOWAREHOUSEAREA ={18}, EDITDATE ={14}, EDITUSER ={15},QTY={19} where {16}", Made4Net.Shared.Util.FormatField(_pickregion), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline),
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_pickedqty),
            Made4Net.Shared.Util.FormatField(_fromlocation),
            Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_tocontainer),
            Made4Net.Shared.Util.FormatField(_tolocation), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause,
             Made4Net.Shared.Util.FormatField(_fromwarehousearea), Made4Net.Shared.Util.FormatField(_towarehousearea), Made4Net.Shared.Util.FormatField(_qty))
            DataInterface.RunSQL(SQL)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PicklistLineUpdated)
            activitystatus = WMS.Lib.Actions.Audit.PCKLNUPD
        Else
            'Create
            _adduser = pUser
            _adddate = DateTime.Now
            SQL = String.Format("Insert into pickdetail(picklist,picklistline,pickregion,consignee,orderid,orderline,status," &
            "sku,uom,qty,adjqty,pickedqty,fromlocation,fromwarehousearea,fromload,toload,tocontainer,tolocation,towarehousearea,adddate,adduser,editdate,edituser) values (" &
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{21}, {13},{14},{15},{16},{22},{17},{18},{19},{20})", Made4Net.Shared.Util.FormatField(_picklist),
            Made4Net.Shared.Util.FormatField(_picklistline), Made4Net.Shared.Util.FormatField(_pickregion), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_orderline),
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_pickedqty),
            Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_tocontainer),
            Made4Net.Shared.Util.FormatField(_tolocation), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser),
            Made4Net.Shared.Util.FormatField(_fromwarehousearea), Made4Net.Shared.Util.FormatField(_towarehousearea))
            DataInterface.RunSQL(SQL)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PicklistLineCreated)
            activitystatus = WMS.Lib.Actions.Audit.PCKLNINS
        End If
        aq.Add("ACTIVITYTYPE", activitystatus)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", _fromlocation)
        aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)

        aq.Add("FROMQTY", _qty)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _toload)
        aq.Add("TOLOC", _tolocation)
        aq.Add("TOWAREHOUSEAREA", _towarehousearea)

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(activitystatus)

        'DataInterface.RunSQL(SQL)
    End Sub

    Public Shared Function Exists(ByVal pPicklistId As String, ByVal pPickLine As Int32) As Boolean
        Dim sql As String = String.Format("select count(1) from pickdetail where picklist = '{0}' and picklistline = {1}", pPicklistId, pPickLine)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Function GetNextLineId() As Int32
        Dim sql As String = String.Format("select max(picklistline) + 1 from pickdetail where picklist = '{0}'", _picklist)
        Return Convert.ToInt32(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "Pick & Cancel"

    'Added a New Method For RWMS-167
    'Added For RWMS-1532 and RWMS-990
    Public Sub ChangeLoad(ByVal oLoad As Load, ByVal tmpLoad As Load, ByVal pUser As String, ByVal pLogDir As String, ByVal pPickListLine As Integer)

        Dim oLogger As LogHandler
        oLogger = New LogHandler(pLogDir, "SubStituteLoad_" & DateTime.Now.ToString("_ddMMyyyy_hhmmss_") & New Random().Next() & ".txt")
        oLogger.StartWrite()

        _fromload = oLoad.LOADID
        _fromlocation = oLoad.LOCATION
        _fromwarehousearea = oLoad.WAREHOUSEAREA
        _edituser = pUser
        _editdate = DateTime.Now

        If (pPickListLine = 1) Then
            If oLoad.ACTIVITYSTATUS = String.Empty Or oLoad.ACTIVITYSTATUS = "" Then

                Dim sqlPick As String = String.Format("SELECT * FROM PICKDETAIL where STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND FROMLOAD ='{0}' AND FROMLOCATION='{1}' ", _fromload, _fromlocation)
                If Not oLogger Is Nothing Then
                    oLogger.Write("SQL statement : " & sqlPick)
                    oLogger.writeSeperator("-", 100)
                End If
                Dim dt As New DataTable
                Dim dr As DataRow
                DataInterface.FillDataset(sqlPick, dt)
                If dt.Rows.Count > 0 Then
                    dr = dt.Rows(0)

                    Dim subsPicklistId As String = String.Empty
                    'Dim subspicklistline As String = String.Empty
                    Dim WhereClausesubsPicklistId As String = String.Empty
                    subsPicklistId = dr.Item("PICKLIST")
                    'subspicklistline = dr.Item("PICKLISTLINE")
                    WhereClausesubsPicklistId = String.Format(" PICKLIST = '{0}'", subsPicklistId)


                    If Not oLogger Is Nothing Then
                        oLogger.Write("WhereClausesubsPicklistId : " & WhereClausesubsPicklistId)
                        oLogger.writeSeperator("-", 100)
                    End If



                    Dim orginalFromload As String = String.Empty
                    Dim orginalFromlocation As String = String.Empty
                    Dim orginalFromWarehousearea As String = String.Empty

                    orginalFromload = tmpLoad.LOADID
                    orginalFromlocation = tmpLoad.LOCATION
                    orginalFromWarehousearea = tmpLoad.WAREHOUSEAREA

                    Dim sqlSubstiPickDtlUpdate As String
                    sqlSubstiPickDtlUpdate = String.Format("UPDATE pickdetail SET fromload = {0}, fromlocation = {1}, fromwarehousearea = {5}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(orginalFromload),
                    Made4Net.Shared.Util.FormatField(orginalFromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClausesubsPicklistId, Made4Net.Shared.Util.FormatField(orginalFromWarehousearea))
                    DataInterface.RunSQL(sqlSubstiPickDtlUpdate)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("Sql Statement : " & sqlSubstiPickDtlUpdate)
                        oLogger.writeSeperator("-", 100)
                    End If

                    Dim substituteTaskId As String = String.Empty
                    substituteTaskId = DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(TASK,'') AS TASK FROM TASKS WHERE STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND PICKLIST='{0}' AND TASKTYPE='FULLPICK'", subsPicklistId))
                    Dim whereClausesubstituteTaskId As String = String.Empty
                    whereClausesubstituteTaskId = String.Format(" TASK = '{0}'", substituteTaskId)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("SQL statement: " & substituteTaskId)
                        oLogger.writeSeperator("-", 100)
                    End If

                    Dim sqlsubstituteTaskIdUpdate As String
                    sqlsubstituteTaskIdUpdate = String.Format("UPDATE TASKS SET FROMLOAD = {0}, FROMLOCATION = {1}, FROMWAREHOUSEAREA = {5}, EDITUSER = {2},EDITDATE = {3} where {4}", Made4Net.Shared.Util.FormatField(orginalFromload),
                    Made4Net.Shared.Util.FormatField(orginalFromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClausesubstituteTaskId, Made4Net.Shared.Util.FormatField(orginalFromWarehousearea))
                    DataInterface.RunSQL(sqlsubstituteTaskIdUpdate)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("Sql Statement : " & sqlsubstituteTaskIdUpdate)
                        oLogger.writeSeperator("-", 100)
                    End If
                End If


            ElseIf oLoad.ACTIVITYSTATUS = "REPLPEND" Then

                Dim originalFromload As String = String.Empty
                Dim originalFromlocation As String = String.Empty
                Dim originalFromwarehousearea As String = String.Empty
                originalFromload = tmpLoad.LOADID
                originalFromlocation = tmpLoad.LOCATION
                originalFromwarehousearea = tmpLoad.WAREHOUSEAREA
                Dim replActivityStatus = String.Empty
                Dim replDestinationLocation = String.Empty
                Dim replDestinationWarehouse = String.Empty
                Dim WhereClausesubstitutePayloadid = String.Empty
                WhereClausesubstitutePayloadid = String.Format("LOADID = '{0}'", originalFromload)
                replActivityStatus = oLoad.ACTIVITYSTATUS
                replDestinationLocation = oLoad.DESTINATIONLOCATION
                replDestinationWarehouse = oLoad.DESTINATIONWAREHOUSEAREA


                'RWMS-852 - Added the UNITSALLOCTED=0 in Update script
                Dim sqlOriginalLoadUpdate As String
                sqlOriginalLoadUpdate = String.Format("UPDATE LOADS SET UNITSALLOCATED=0, ACTIVITYSTATUS = {0}, DESTINATIONLOCATION = {1}, DESTINATIONWAREHOUSEAREA = {2}, edituser = {3},editdate = {4} where {5}", Made4Net.Shared.Util.FormatField(replActivityStatus),
                 Made4Net.Shared.Util.FormatField(replDestinationLocation), Made4Net.Shared.Util.FormatField(replDestinationWarehouse), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClausesubstitutePayloadid)
                DataInterface.RunSQL(sqlOriginalLoadUpdate)

                If Not oLogger Is Nothing Then
                    oLogger.Write("Sql Statement : " & sqlOriginalLoadUpdate)
                    oLogger.writeSeperator("-", 100)
                End If

                Dim sqlRepl As String = String.Format("SELECT * FROM REPLENISHMENT where STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND FROMLOAD ='{0}' AND FROMLOCATION='{1}' ", _fromload, _fromlocation)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Sql Statement : " & sqlRepl)
                    oLogger.writeSeperator("-", 100)
                End If

                Dim dt As New DataTable
                Dim dr As DataRow
                DataInterface.FillDataset(sqlRepl, dt)
                If dt.Rows.Count > 0 Then
                    dr = dt.Rows(0)

                    Dim substituteReplIdId As String = String.Empty
                    Dim whereClausesubstituteRepl As String = String.Empty
                    substituteReplIdId = dr.Item("REPLID")
                    whereClausesubstituteRepl = String.Format("REPLID = '{0}'", substituteReplIdId)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("Sql Statement : " & substituteReplIdId & " - " & " whereClausesubstituteRepl : " & whereClausesubstituteRepl)
                        oLogger.writeSeperator("-", 100)
                    End If



                    Dim sqlSubstiReplenishUpdate As String
                    sqlSubstiReplenishUpdate = String.Format("UPDATE REPLENISHMENT SET TOLOAD = {0},fromload = {1}, fromlocation = {2}, fromwarehousearea = {6}, edituser = {3},editdate = {4} where {5}", Made4Net.Shared.Util.FormatField(originalFromload), Made4Net.Shared.Util.FormatField(originalFromload),
                Made4Net.Shared.Util.FormatField(originalFromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClausesubstituteRepl, Made4Net.Shared.Util.FormatField(originalFromwarehousearea))
                    DataInterface.RunSQL(sqlSubstiReplenishUpdate)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("Sql Statement : " & sqlSubstiReplenishUpdate)
                        oLogger.writeSeperator("-", 100)
                    End If

                    Dim substituteTaskId As String = String.Empty
                    substituteTaskId = DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(TASK,'') AS TASK FROM TASKS WHERE STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND REPLENISHMENT ='{0}'", substituteReplIdId))
                    Dim whereClause3 As String = String.Empty
                    whereClause3 = String.Format(" TASK = '{0}'", substituteTaskId)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("Sql Statement : " & substituteTaskId)
                        oLogger.writeSeperator("-", 100)
                    End If


                    Dim sqlsubstituteTaskIdUpdate As String
                    'sqlsubstituteTaskIdUpdate = String.Format("UPDATE TASKS SET TOLOAD = {0}, FROMLOAD = {1}, FROMLOCATION = {2}, FROMWAREHOUSEAREA = {5}, EDITUSER = {2},EDITDATE = {3} where {4}", Made4Net.Shared.Util.FormatField(originalFromload), Made4Net.Shared.Util.FormatField(originalFromload), _
                    'Made4Net.Shared.Util.FormatField(originalFromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClause3, Made4Net.Shared.Util.FormatField(originalFromwarehousearea))
                    'DataInterface.RunSQL(sqlsubstituteTaskIdUpdate)

                    sqlsubstituteTaskIdUpdate = String.Format("UPDATE TASKS SET TOLOAD = {0}, FROMLOAD = {1}, FROMLOCATION = {2}, EDITUSER = {3},EDITDATE = {4}, FROMWAREHOUSEAREA = {5} where {6} ", Made4Net.Shared.Util.FormatField(originalFromload), Made4Net.Shared.Util.FormatField(originalFromload),
                Made4Net.Shared.Util.FormatField(originalFromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(originalFromwarehousearea), whereClause3)
                    DataInterface.RunSQL(sqlsubstituteTaskIdUpdate)

                    If Not oLogger Is Nothing Then
                        oLogger.Write("Sql Statement : " & sqlsubstituteTaskIdUpdate)
                        oLogger.writeSeperator("-", 100)
                    End If
                End If

            End If
            Dim WhereClauseCurrPicklistId As String = String.Empty
            WhereClauseCurrPicklistId = String.Format(" PICKLIST = '{0}'", _picklist)

            Dim sqlCurrPickDtlUpdate As String
            sqlCurrPickDtlUpdate = String.Format("UPDATE pickdetail SET fromload = {0}, fromlocation = {1}, fromwarehousearea = {5}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(_fromload),
            Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClauseCurrPicklistId, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
            DataInterface.RunSQL(sqlCurrPickDtlUpdate)


            If Not oLogger Is Nothing Then
                oLogger.Write("Sql Statement : " & sqlCurrPickDtlUpdate)
                oLogger.writeSeperator("-", 100)
            End If

            Dim currTaskId As String = String.Empty
            currTaskId = DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(TASK,'') AS TASK FROM TASKS where STATUS <> 'CANCELED' AND STATUS <> 'COMPLETE' AND PICKLIST='{0}' AND TASKTYPE='FULLPICK'", _picklist))
            Dim whereClausecurrTaskId = String.Empty
            whereClausecurrTaskId = String.Format(" TASK = '{0}'", currTaskId)


            If Not oLogger Is Nothing Then
                oLogger.Write("Sql Statement : " & currTaskId)
                oLogger.writeSeperator("-", 100)
            End If

            Dim sqlCurrTaskUpdate As String
            sqlCurrTaskUpdate = String.Format("update TASKS set FROMLOAD = {0}, FROMLOCATION = {1}, FROMWAREHOUSEAREA = {5}, EDITUSER = {2},EDITDATE = {3} where {4}", Made4Net.Shared.Util.FormatField(_fromload),
            Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClausecurrTaskId, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
            DataInterface.RunSQL(sqlCurrTaskUpdate)


            If Not oLogger Is Nothing Then
                oLogger.Write("Sql Statement : " & sqlCurrTaskUpdate)
                oLogger.writeSeperator("-", 100)
            End If
        End If
    End Sub

    'End Added a New Method For RWMS-167
    'Ended  For RWMS-1532 and RWMS-990
    Public Sub ChangeLoad(ByVal oLoad As Load, ByVal pUser As String)
        _fromload = oLoad.LOADID
        _fromlocation = oLoad.LOCATION
        _fromwarehousearea = oLoad.WAREHOUSEAREA

        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String
        sql = String.Format("update pickdetail set fromload = {0}, fromlocation = {1}, fromwarehousearea = {5}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(_fromload),
        Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
        DataInterface.RunSQL(sql)

        'Added for RWMS-167
        Dim currTaskId As String = String.Empty
        currTaskId = DataInterface.ExecuteScalar(String.Format("select isnull(TASK,'') as Task from TASKS where PICKLIST='{0}' and TASKTYPE='FULLPICK'", _picklist))
        Dim whereClause2 As String = String.Empty
        whereClause2 = String.Format(" TASK = '{0}'", currTaskId)

        Dim sql2 As String
        sql2 = String.Format("update TASKS set FROMLOAD = {0}, FROMLOCATION = {1}, FROMWAREHOUSEAREA = {5}, EDITUSER = {2},EDITDATE = {3} where {4}", Made4Net.Shared.Util.FormatField(_fromload),
        Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClause2, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
        DataInterface.RunSQL(sql2)
        'End Added For RWMS-167
    End Sub

    Friend Sub Cancel(ByVal pUser As String)
        Dim oldStat As String = _status
        Dim ord As New OutboundOrderHeader(_consignee, _orderid)
        ord.AdjustLine(_orderline, _adjqty - _pickedqty, pUser)
        If _fromload <> "" AndAlso WMS.Logic.Load.Exists(_fromload) Then
            Dim ld As New Load(_fromload)
            ld.unAllocate(_adjqty - _pickedqty, pUser)
        ElseIf WMS.Logic.PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
            Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
            oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, _adjqty - _pickedqty, pUser)
        End If
        _adjqty = _pickedqty
        _edituser = pUser
        _editdate = DateTime.Now
        'Added for PWMS-802 for RWMS-847
        Dim qtymodified As Decimal
        Dim sqlqtymodofied As String = String.Format("Select QTYMODIFIED from OUTBOUNDORDETAIL where consignee = '{0}' and orderid = '{1}' and orderline = {2}", _consignee, _orderid, _orderline)
        qtymodified = Convert.ToDecimal(DataInterface.ExecuteScalar(sqlqtymodofied))
        WMS.Logic.Shipment.ShipmentDetail.UpdateQtyIfExists(pUser, qtymodified, _consignee, _orderid, _orderline)
        'End Added PWMS-802 for RWMS-847
        Dim sql As String
        If _adjqty = 0 Then
            _status = WMS.Lib.Statuses.Picklist.CANCELED
        Else
            _status = WMS.Lib.Statuses.Picklist.COMPLETE
        End If
        sql = String.Format("update pickdetail set ADJQTY = {0}, STATUS = {1}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListLineCancel)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CANCELPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", _fromlocation)
        aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldStat)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _adjqty)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.CANCELPICK)
    End Sub

    Public Sub Adjust(ByVal adjusted As Decimal, ByVal puser As String)
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust pickdetail, incorrect status", "Cannot adjust pickdetail, incorrect status")
            Throw m4nEx
        End If
        If (adjusted > _adjqty) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Adjusted units can't be more than original", "Adjusted units can't be more than original")
            Throw m4nEx
        End If
        Dim ord As New OutboundOrderHeader(_consignee, _orderid)
        ord.AdjustLine(_orderline, _adjqty - adjusted, puser)
        If _fromload <> "" AndAlso WMS.Logic.Load.Exists(_fromload) Then
            Dim ld As New Load(_fromload)
            ld.unAllocate(_adjqty - adjusted, puser)
        ElseIf WMS.Logic.PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
            Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
            oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, _adjqty - adjusted, puser)
        End If
        _adjqty = adjusted
        _edituser = puser
        _editdate = DateTime.Now
        If _adjqty = 0 Then
            _status = WMS.Lib.Statuses.Picklist.CANCELED
        ElseIf _adjqty = _pickedqty Then
            _status = WMS.Lib.Statuses.Picklist.COMPLETE
        End If
        Dim sql As String = String.Format("update pickdetail set ADJQTY = {0}, status = {1},edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub unAllocate(ByVal puser As String, Optional ByRef pOrdHeader As OutboundOrderHeader = Nothing)
        unAllocate(_adjqty - _pickedqty, puser, pOrdHeader)
    End Sub

    Public Sub unAllocate(ByVal UnAllocUnits As Decimal, ByVal puser As String, Optional ByRef pOrdHeader As OutboundOrderHeader = Nothing)
        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unallocate pickdetail, incorrect status", "Cannot unallocate pickdetail, incorrect status")
            Throw m4nEx
        End If
        Dim orgStatus As String = _status
        If pOrdHeader Is Nothing Then
            Dim ord As New OutboundOrderHeader(_consignee, _orderid)
            ord.unAllocateLine(_orderline, UnAllocUnits, puser)
        Else
            pOrdHeader.unAllocateLine(_orderline, UnAllocUnits, puser)
        End If

        If _fromload <> "" AndAlso WMS.Logic.Load.Exists(_fromload) Then
            Dim ld As New Load(_fromload)
            If UnAllocUnits > ld.UNITSALLOCATED Then
                ld.unAllocate(ld.UNITSALLOCATED, puser)
            Else
                ld.unAllocate(UnAllocUnits, puser)
            End If
        ElseIf WMS.Logic.PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
            'Calling WMS.Logic.PickLoc, with additional parameter UNALLOCATION-ONLY, this object gets only pickloc table record instead of vPickLoc
            Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku, "UNALLOCATION-ONLY")
            oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, UnAllocUnits, puser)
        End If
        _adjqty = _adjqty - UnAllocUnits
        _edituser = puser
        _editdate = DateTime.Now
        If _adjqty = 0 Then
            _status = WMS.Lib.Statuses.Picklist.CANCELED
        ElseIf _adjqty = _pickedqty Then
            _status = WMS.Lib.Statuses.Picklist.COMPLETE
        End If
        Dim sql As String = String.Format("update pickdetail set ADJQTY = {0}, status = {1}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListLineUnAlloc)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNALLOCATEPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", _fromlocation)
        aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
        aq.Add("FROMQTY", _adjqty + UnAllocUnits)
        aq.Add("FROMSTATUS", orgStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _adjqty)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.UNALLOCATEPICK)
    End Sub

    Public Sub Pick(ByVal PickQty As Decimal, ByVal PickUOM As String, ByVal puser As String, ByVal oAttributeCollection As AttributesCollection)
        If _uom.ToUpper <> PickUOM.ToUpper Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Pick UOM does not match", "Pick quantity less than quantity left to pick")
            Throw m4nEx
        End If
        Dim oSku As New SKU(_consignee, _sku)
        PickQty = PickQty * oSku.ConvertToUnits(PickUOM)
        Pick(PickQty, puser, oAttributeCollection)
    End Sub

    Public Sub Pick(ByVal puser As String, ByVal oAttributeCollection As AttributesCollection, ByVal pSystemPickShort As Boolean)
        Pick(_adjqty - _pickedqty, puser, oAttributeCollection, "", pSystemPickShort)
    End Sub

    Public Sub Pick(ByVal PickQty As Decimal, ByVal puser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pUserPickShortType As String = "", Optional ByVal pSystemPickShort As Boolean = False, Optional ByVal pRelStrat As WMS.Logic.ReleaseStrategyDetail = Nothing, Optional ByVal pLabelPrinter As String = "", Optional ByVal pPickJobQtyToPick As Decimal = -1, Optional ByVal pUserPickShort As Boolean = False, Optional ByVal pUnallocLoadOnUserPickShort As Boolean = True, Optional ByVal pSkuObj As WMS.Logic.SKU = Nothing, Optional ByVal pBasedOnPartialPickedLine As Boolean = False)

        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()

        'Start RWMS-1367 and RWMS-1318
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(" Starting PicklistDetail Pick operation for user : " & puser & " Consignee : " & _consignee & " Orderid : " & _orderid & " Orderline : " & _orderline & " PickQty : " & PickQty)
        End If
        'end RWMS-1367 and RWMS-1318

        Dim bUserPickShort As Boolean = False

        Dim ord As New OutboundOrderHeader(_consignee, _orderid)
        Dim orl As OutboundOrderHeader.OutboundOrderDetail = ord.Lines.GetLine(_orderline)
        Dim oSku As WMS.Logic.SKU
        If pSkuObj Is Nothing Then
            oSku = New SKU(_consignee, _sku)
        Else
            oSku = pSkuObj
        End If




        'Added for RWMS-1478 and RWMS-1408 - checking the status of the pickdetail before validating the CanPick
        Dim pckdetStatus As String = Me.Status
        If pckdetStatus <> "COMPLETE" Then
            If Not CanPick(PickQty, oSku) Or Not orl.CanPick(PickQty, oSku) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot pick line, Invalid quantities in pick line or order line", "Cannot pick line, Invalid quantities in pick line or order line")
            End If
        End If
        'Ended for RWMS-1478 and RWMS-1408 -


        'If pPickJobQtyToPick <> -1 AndAlso PickQty < pPickJobQtyToPick AndAlso (PickQty < _adjqty - _pickedqty) Then
        '    bUserPickShort = True
        'End If
        bUserPickShort = pUserPickShort

        Dim ld As Load
        Dim sLoadStatus As String
        If _fromload <> String.Empty Then
            ld = New Load(_fromload)
            sLoadStatus = ld.STATUS
        End If
        Dim oRelStrat As ReleaseStrategyDetail
        If Not pRelStrat Is Nothing Then
            oRelStrat = pRelStrat
        Else
            oRelStrat = GetReleaseStrategy()
        End If

        If PickQty >= 0 Then
            'Open a new Transaction to encapsulate the picking operations
            Made4Net.DataAccess.DataInterface.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)
            Try

                If (PickQty > 0) Then

                    'Load units less than the pickQty , set pickQty to load units , systempickshort to true
                    If (ld.UNITS < PickQty) Then
                        PickQty = ld.UNITS
                        pSystemPickShort = True
                    End If
                    PickLoad(ld, PickQty, ord, oAttributeCollection, puser, oSku)
                End If


                _pickedqty = _pickedqty + PickQty
                'Update picked qty and check for pick short
                'If _pickedqty + PickQty < _adjqty Then    'User Pick Short - Act According to the strategies
                'This case will handle both the system pick short (if it's true and

                'Start RWMS-1367 and RWMS-1318
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(" PicklistDetail.Pick : PickedQty : " & _pickedqty)
                End If
                'End RWMS-1367 and RWMS-1318

                Dim userPickShortStr As String
                If bUserPickShort Then

                    If Not String.IsNullOrEmpty(pUserPickShortType) Then
                        userPickShortStr = pUserPickShortType.ToLower()
                    Else
                        userPickShortStr = oRelStrat.UserPickShort.ToLower()
                    End If

                    If userPickShortStr <> WMS.Logic.UserPickShort.PickPartialLeaveOpen.ToLower() Then
                        If pSystemPickShort AndAlso (oRelStrat.SystemPickShort.ToLower() = WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower() OrElse
                                    oRelStrat.SystemPickShort.ToLower() = WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()) AndAlso
                                    _status <> WMS.Lib.Statuses.Picklist.PARTPICKED Then
                            UserPickShort(userPickShortStr, _adjqty - _pickedqty, ord, ld, oAttributeCollection, puser, pUnallocLoadOnUserPickShort)
                        Else
                            UserPickShort(userPickShortStr, pPickJobQtyToPick - PickQty, ord, ld, oAttributeCollection, puser, pUnallocLoadOnUserPickShort)
                        End If
                        'Will complete the line.
                        _adjqty = _pickedqty
                    Else
                        'If _status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        If _status = WMS.Lib.Statuses.Picklist.PARTPICKED OrElse pBasedOnPartialPickedLine Then
                            If pBasedOnPartialPickedLine Then _status = WMS.Lib.Statuses.Picklist.PARTPICKED
                            If pSystemPickShort AndAlso oRelStrat.SystemPickShort.ToLower() = WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower() OrElse
                             oRelStrat.SystemPickShort.ToLower() = WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower() Then
                                UserPickShort(userPickShortStr, _adjqty - _pickedqty, ord, ld, oAttributeCollection, puser, pUnallocLoadOnUserPickShort)
                            Else
                                UserPickShort(userPickShortStr, pPickJobQtyToPick - PickQty, ord, ld, oAttributeCollection, puser, pUnallocLoadOnUserPickShort)
                            End If
                            'Will complete the line.
                            _adjqty = _pickedqty
                        End If
                    End If
                ElseIf pSystemPickShort Then
                    Select Case oRelStrat.SystemPickShort.ToLower
                        Case WMS.Logic.SystemPickShort.PickPartialCancelException.ToLower(), WMS.Logic.SystemPickShort.PickPartialCreateException.ToLower(),
                        WMS.Logic.SystemPickShort.PickZeroCancelException.ToLower(), WMS.Logic.SystemPickShort.PickZeroCreateException.ToLower()
                            '' Shouldnt unallocate at this stage for system pick short, happened at the get pick phase ?!
                            'SystemPickShort(oRelStrat.SystemPickShort, _qty - _pickedqty, ord, Nothing, puser)

                            'RWMS-2599
                            If (PickQty = 0) Then
                                orl.CheckException(puser, wmsrdtLogger, _adjqty)
                            End If
                            'RWMS-2599 END


                            _adjqty = _pickedqty
                        Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower(), WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower()
                            If _status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                                _adjqty = _pickedqty
                            End If
                    End Select
                End If
                'Old Comment
                ''RWMS-727 Check if userpickshort or systempickshort enabled for create exception when pick 0, unallocate orderline to create exception
                'New Comment
                ''RWMS-727 Check if userpickshort enabled for create exception when pick 0, unallocate orderline to create exception
                If (PickQty = 0) Then
                    If bUserPickShort Then
                        If userPickShortStr = WMS.Logic.UserPickShort.PickPartialCreateException.ToLower() Then
                            ord.unAllocateLine(_orderline, _qty, puser)

                        End If
                        'Commented for RWMS-2313(RWMS-2253) Start
                        'SystemPickshort() method does decrement the QtyAllocation of the orderline , this is duplicate unallocation of orderline when systempickshort
                        'ElseIf pSystemPickShort Then
                        '    If oRelStrat.SystemPickShort.ToLower = WMS.Logic.SystemPickShort.PickPartialCreateException.ToLower() Then
                        '        ord.unAllocateLine(_orderline, _qty, puser)
                        '    End If
                        'Commented for RWMS-2313(RWMS-2253) End
                    End If
                End If
                'Else
                ''Pick Regular - Full Qty
                ''create the new load and pick it
                'PickLoad(ld, PickQty, ord, oAttributeCollection, puser)
                '_pickedqty = _pickedqty + PickQty
                'End If
                'Update the pickdetail
                _edituser = puser
                _editdate = DateTime.Now
                If _adjqty = 0 And PickQty > 0 Then
                    _status = WMS.Lib.Statuses.Picklist.CANCELED
                    'Start RWMS-1367 and RWMS-1318
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(" PicklistDetail.Pick : _adjqty : " & _adjqty & " PickQty : " & PickQty & " PICK DETAIL status : " & _status)
                    End If
                    'End RWMS-1367 and RWMS-1318

                ElseIf _adjqty = 0 And PickQty = 0 Then
                    _status = WMS.Lib.Statuses.Picklist.COMPLETE
                    'Start RWMS-1367 and RWMS-1318
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(" PicklistDetail.Pick : _adjqty : " & _adjqty & " PickQty : " & PickQty & " PICK DETAIL status : " & _status)
                    End If
                    'End RWMS-1367 and RWMS-1318

                ElseIf _adjqty <= _pickedqty Then
                    _status = WMS.Lib.Statuses.Picklist.COMPLETE
                    'Start RWMS-1367 and RWMS-1318
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(" PicklistDetail.Pick : _adjqty : " & _adjqty & " PickQty : " & PickQty & " PICK DETAIL status : " & _status)
                    End If
                    'End RWMS-1367 and RWMS-1318
                ElseIf _adjqty > _pickedqty Then
                    _status = WMS.Lib.Statuses.Picklist.PARTPICKED
                    'Start RWMS-1367 and RWMS-1318
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(" PicklistDetail.Pick : _adjqty : " & _adjqty & " PickQty : " & PickQty & " PICK DETAIL status : " & _status)
                    End If
                    'End RWMS-1367 and RWMS-1318
                End If
                'Start RWMS-1367 and RWMS-1318
                ' DataInterface.RunSQL(String.Format("Update PICKDETAIL WITH (UPDLOCK) SET ADJQTY = {0},PICKEDQTY = {1},STATUS = {2},TOLOAD = {3},EDITUSER = {4}, EDITDATE = {5}, TOCONTAINER={6} WHERE {7}", Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_pickedqty), _
                Dim updSQL As String = String.Format("Update PICKDETAIL WITH (UPDLOCK) SET ADJQTY = {0},PICKEDQTY = {1},STATUS = {2},TOLOAD = {3},EDITUSER = {4}, EDITDATE = {5}, TOCONTAINER={6} WHERE {7}", Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_pickedqty),
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate),
                Made4Net.Shared.Util.FormatField(_tocontainer), WhereClause)
                DataInterface.RunSQL(updSQL)

                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(" PicklistDetail.Pick : Updated PICKDETAIL with SQL " & updSQL)
                End If
                'End RWMS-1367 and RWMS-1318

            Catch ex As Made4Net.Shared.M4NException
                Made4Net.DataAccess.DataInterface.RollBackTransaction()
                'Start RWMS-1367 and RWMS-1318
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(ex.ToString)
                End If
                'End RWMS-1367 and RWMS-1318

                Throw ex
            Catch ex As Exception
                Made4Net.DataAccess.DataInterface.RollBackTransaction()
                'Start RWMS-1367 and RWMS-1318
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(ex.ToString)
                End If
                'End RWMS-1367 and RWMS-1318
                Throw New Made4Net.Shared.M4NException(New Exception, "Picking failed, please confirm pick line again." + ex.ToString(), "Picking failed, please confirm pick line again." + ex.ToString())

            End Try
            Made4Net.DataAccess.DataInterface.CommitTransaction()
            Dim pck As Picklist = New Picklist(PickList)

            If pck.ShouldPrintCaseLabel Then
                createCaseEntries(puser)
                For Each caseid As String In _caseIDs
                    printCaseLabels(pck.GetCaseLabel, pLabelPrinter, caseid)
                Next
            End If
            'Check wethear should print pick label on complete...
            If Not oRelStrat Is Nothing Then
                If _pickedqty > 0 And oRelStrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKDETAILCOMPLETE And oRelStrat.LabelFormat <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                    PrintPickLabels(oRelStrat.LabelFormat, pLabelPrinter)
                End If
            End If
            'Raise PickLoad Event
            If PickQty > 0 Then
                RaisePickLoadEvent(sLoadStatus, puser)
                'Start RWMS-1367 and RWMS-1318
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(" PicklistDetail.Pick : Load Status : " & sLoadStatus & " for user : " & puser)
                End If
                'End RWMS-1367 and RWMS-1318

            End If
            CreateSpecificPickupTasks(ld)
        End If
    End Sub
    Private Sub printCaseLabels(LabelType As String, lblPrinter As String, CaseID As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", LabelType)
        qSender.Add("LabelType", LabelType)
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("CASEID", CaseID)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("CASEID", CaseID)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Send("Label", "Picklist CaseLabel Label")
    End Sub
    Private Sub createCaseEntries(user As String)
        For Ind As Integer = 1 To _pickedqty
            Dim cs As CaseDetail = WMS.Logic.CaseDetail.Create(Consignee, PickList, PickListLine, OrderId, OrderLine, FromLoad, ToLoad, ToContainer, "PICKED", user)
            _caseIDs.Add(cs.CaseID)
        Next
    End Sub
    Private Sub CreateSpecificPickupTasks(ByVal oLoad As WMS.Logic.Load)
        Dim sSql As String
        If oLoad Is Nothing Then Return
        'Check if the are open picks left for the load
        'sSql = String.Format("select COUNT(1) from PICKDETAIL where STATUS in ('{0}','{1}') and FROMLOAD = '{2}'", _
        '    WMS.Lib.Statuses.Picklist.PARTPICKED, WMS.Lib.Statuses.Picklist.RELEASED, _fromload)
        'If Convert.ToInt32(DataInterface.ExecuteScalar(sSql)) > 0 Then
        '    Return
        'End If
        If oLoad.UNITSALLOCATED > 0 OrElse oLoad.UNITS = 0 Then Return

        'Check if current location is a Z-Picking location
        sSql = String.Format("select COUNT(1) from LOCATION where isnull(ZPICKINGLOCATION,'') = '{0}'", _fromlocation)
        If Convert.ToInt32(DataInterface.ExecuteScalar(sSql)) = 0 Then
            Return
        End If

        'If load is placed on an handling unit - validate that there isnt any other load required from its HU
        If oLoad.ContainerId <> String.Empty Then
            sSql = String.Format("select COUNT(1) from PICKDETAIL pd inner join INVLOAD il on il.LOADID = pd.FROMLOAD where pd.STATUS in ('{0}','{1}') and il.HANDLINGUNIT = '{2}'",
                WMS.Lib.Statuses.Picklist.PARTPICKED, WMS.Lib.Statuses.Picklist.RELEASED, oLoad.ContainerId)
            If Convert.ToInt32(DataInterface.ExecuteScalar(sSql)) = 0 Then
                Return
            End If
        End If

        'Otherwise create a load specific task
        Dim oSpecPickUpTask As New SpecificPickupTask
        If oLoad.ContainerId <> String.Empty Then
            'Check if there is already an open task for the specific load already
            sSql = String.Format("select count(1) from TASKS where FROMLOCATION = '{0}' and FROMCONTAINER = '{1}' and TASKTYPE ='{2}' and STATUS not in ('{3}','{4}')", _fromlocation, oLoad.ContainerId, WMS.Lib.TASKTYPE.SPICKUP, WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED)
            If Convert.ToInt32(DataInterface.ExecuteScalar(sSql)) > 0 Then
                Return
            End If
            oSpecPickUpTask.CreateSpecificContainerPickup(_fromlocation, oLoad.ContainerId)
        Else
            'Check if there is already an open task for the specific load already
            sSql = String.Format("select count(1) from TASKS where FROMLOCATION = '{0}' and FROMLOAD = '{1}' and TASKTYPE ='{2}' and STATUS not in ('{3}','{4}')", _fromlocation, oLoad.LOADID, WMS.Lib.TASKTYPE.SPICKUP, WMS.Lib.Statuses.Task.COMPLETE, WMS.Lib.Statuses.Task.CANCELED)
            If Convert.ToInt32(DataInterface.ExecuteScalar(sSql)) > 0 Then
                Return
            End If
            oSpecPickUpTask.CreateSpecificLoadPickup(_fromlocation, _fromload)
        End If
    End Sub

    Private Sub PickLoad(ByVal oFromLoad As WMS.Logic.Load, ByVal PickQty As Decimal, ByVal oOrder As WMS.Logic.OutboundOrderHeader, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String, Optional ByVal oSku As WMS.Logic.SKU = Nothing)

        If PickQty = 0 Then
            Return
        ElseIf PickQty > _adjqty - _pickedqty Then
            'We have to over pick from load - check if there is enough units to allocate
            Dim dOverPickQty As Decimal = PickQty - _adjqty + _pickedqty
            If oFromLoad.UNITS - oFromLoad.UNITSALLOCATED >= dOverPickQty Then
                'There is enough to allocate from load
                oFromLoad.Allocate(dOverPickQty, WMS.Lib.USERS.SYSTEMUSER)
                oOrder.Lines.Line(_orderline).Allocate(dOverPickQty, WMS.Lib.USERS.SYSTEMUSER)
            Else
                'Load units shortage
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot pick more than required - load units short", "Cannot pick more than required - load units short")
            End If
        End If
        Dim newLoadLoc As String
        Dim newld As Load

        ''Keep on the toload only when we have a part picked line, and we should add the quantities to the first picked load
        '' not valid - if line is part picked then it will be splitted into a new line in the picklist....
        'If _status <> WMS.Lib.Statuses.Picklist.PARTPICKED AndAlso Not WMS.Logic.Load.Exists(_toload) Then
        _toload = ""
        'End If

        Dim pickedWeight As Decimal = -1

        If (Me.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Or Me.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Or Me.PickType = WMS.Lib.PICKTYPE.PARALLELPICK Or (Me.PickType = WMS.Lib.PICKTYPE.FULLPICK AndAlso MultiLinesFullPickLoad(True))) Then
            newld = New Load
            newld.LOCATION = ""
            If _toload = "" Then
                Dim newLoadAttribute As New AttributesCollection
                newLoadAttribute.Add(oFromLoad.LoadAttributes.Attributes)
                newLoadAttribute.Add(oAttributeCollection)
                _toload = WMS.Logic.Load.GenerateLoadId()

                newld.CreateLoad(_toload, _consignee, _sku, _uom, "", "", oFromLoad.STATUS, WMS.Lib.Statuses.ActivityStatus.PICKED, PickQty, Nothing, Nothing, Nothing, newLoadAttribute, pUser, Nothing, "", DateTime.MinValue, Nothing, Nothing, oSku)

                Dim pcklst As New Picklist(Me.PickList)
                If Me.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    If String.IsNullOrEmpty(_tocontainer) Then
                        _tocontainer = oFromLoad.ContainerId
                    End If
                End If

                If oAttributeCollection Is Nothing OrElse String.IsNullOrEmpty(oAttributeCollection("WEIGHT")) Then
                    newld.AdjustLoadWeightAttribute(oFromLoad.UNITS, PickQty, oFromLoad.WeightBeforeAdjusting, pUser)
                Else
                    'Added for PWMS-840
                    'pickedWeight = oAttributeCollection("WEIGHT")
                    If Me.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                        Dim ld As New Load(oFromLoad.LOADID)
                        If Math.Round(ld.GetAttribute("WEIGHT"), 2) > 0 Then
                            pickedWeight = Convert.ToDecimal(ld.GetAttribute("WEIGHT"))
                        Else
                            pickedWeight = oAttributeCollection("WEIGHT")
                        End If
                    Else
                        pickedWeight = oAttributeCollection("WEIGHT")
                    End If
                    'End Added for PWMS-840

                End If

                If Not _tocontainer Is Nothing AndAlso Not _tocontainer = "" Then
                    Dim cnt As New Container(_tocontainer, False)
                    cnt.Place(newld, pUser, True)
                End If
            Else
                newld = New WMS.Logic.Load(_toload)
                newld.UpdatePickQty(PickQty, pUser)
            End If
            'Set the destination location of the load to the to location of the pickdetail
            newld.SetDestinationLocation(_tolocation, _towarehousearea, pUser)

            'If PickQty > _adjqty Then
            '    oFromLoad.Pick(PickQty, pUser, _adjqty)
            'Else

            Dim isweightcaptureneeded As Boolean = WMS.Logic.SKU.weightNeeded(oSku)
            oFromLoad.Pick(PickQty, pUser, 0, pickedWeight, isweightcaptureneeded)
            'End If
        Else
            _toload = _fromload
            newld = New Load(_toload)
            'If MultiLinesFullPickLoad(True) Then
            If Not String.IsNullOrEmpty(newld.ContainerId) Then
                _tocontainer = newld.ContainerId
            End If
            'End If
            newld.PickFull(_tolocation, _towarehousearea, pUser)
            newld.setAttributes(oAttributeCollection, pUser)
        End If
        'RWMS-665 and RWMS-790- Passing the _qty parameter
        oOrder.Pick(_orderline, PickQty, _toload, _picklist, _picklistline, pUser, oSku, _qty)
    End Sub

    Private Function MultiLinesFullPickLoad(ByVal pIgnoreStatus As Boolean) As Boolean
        Dim sql As String = String.Format("select count(1) from pickdetail where fromload = '{0}' and picklist = '{1}' and picklistline <> {2}",
                    _fromload, _picklist, _picklistline)
        If Not pIgnoreStatus Then
            sql = String.Format("{0} and status not in ('COMPLETE','CANCELED') ", sql)
        End If
        Dim cnt As Int32 = DataInterface.ExecuteScalar(sql)
        If cnt > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function CanPick(ByVal pUnits As Decimal, ByVal oSku As WMS.Logic.SKU) As Boolean
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()

        'Start RWMS-1367 and RWMS-1318
        If Not rdtLogger Is Nothing Then
            rdtLogger.Write(" PicklistDetail CanPick Assessment - AdjustedQuantity : " & AdjustedQuantity & " oSku.OVERPICKPCT : " & oSku.OVERPICKPCT & " PickedQuantity : " & PickedQuantity & " pUnits : " & pUnits)
        End If
        'End RWMS-1367 and RWMS-1318

        If AdjustedQuantity * oSku.OVERPICKPCT < PickedQuantity + pUnits Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function CanBePartialPicked() As Boolean
        'For now always return False - need to think of validators and stuff.
        'If we Have more loads, replenishments,counting etc Affected the original load....
        If _fromload = "" Then
            Return False
        End If
        Dim oLoad As New WMS.Logic.Load(_fromload)
        If oLoad.UNITSALLOCATED >= _adjqty - _pickedqty Then
            Return True
        End If
        Return False
    End Function

    Public Sub CompleteDetail(ByVal pUser As String, Optional ByVal pUnAllocateLoad As Boolean = True)
        Dim oOrder As New OutboundOrderHeader(_consignee, _orderid)
        oOrder.unAllocateLine(_orderline, _adjqty - _pickedqty, pUser)
        If pUnAllocateLoad AndAlso _fromload <> "" Then
            Dim oFromLoad As New Load(_fromload)
            oFromLoad.unAllocate(_adjqty - _pickedqty, pUser)
        End If
        _edituser = pUser
        _editdate = DateTime.Now
        _adjqty = _pickedqty
        If _adjqty = 0 Then
            _status = WMS.Lib.Statuses.Picklist.CANCELED
        Else
            _status = WMS.Lib.Statuses.Picklist.COMPLETE
        End If
        DataInterface.RunSQL(String.Format("Update PICKDETAIL SET ADJQTY = {0},STATUS = {1},EDITUSER = {2}, EDITDATE = {3} WHERE {4}", Made4Net.Shared.Util.FormatField(_adjqty),
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
    End Sub

    'RWMS-2100
    Public Sub CompleteDetailPicking(ByVal LineNumber As Int32, ByVal puser As String, Optional ByVal pUnAllocateLoad As Boolean = True)

        If _status = WMS.Lib.Statuses.Picklist.COMPLETE Or _status = WMS.Lib.Statuses.Picklist.CANCELED Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust pickdetail, incorrect status", "Cannot adjust pickdetail, incorrect status")
            Throw m4nEx
        End If
        If _picktype = WMS.Lib.PICKTYPE.FULLPICK Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot unallocate full pick", "Cannot unallocate full pick")
            Throw m4nEx
        End If
        Dim UnAllocUnits As Decimal = _adjqty - _pickedqty
        Dim orgStatus As String = _status

        Dim ord As New OutboundOrderHeader(_consignee, _orderid)
        ord.unAllocateLine(_orderline, UnAllocUnits, puser)

        If _fromload <> "" AndAlso WMS.Logic.Load.Exists(_fromload) Then
            Dim ld As New Load(_fromload)
            If UnAllocUnits > ld.UNITSALLOCATED Then
                ld.unAllocate(ld.UNITSALLOCATED, puser)
            Else
                ld.unAllocate(UnAllocUnits, puser)
            End If
        ElseIf WMS.Logic.PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
            Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
            oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, UnAllocUnits, puser)
        End If
        _adjqty = _adjqty - UnAllocUnits
        _edituser = puser
        _editdate = DateTime.Now

        _status = WMS.Lib.Statuses.Picklist.COMPLETE

        Dim sql As String = String.Format("update pickdetail set ADJQTY = {0}, status = {1}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListLineUnAlloc)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNALLOCATEPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", _fromlocation)
        aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
        aq.Add("FROMQTY", _adjqty + UnAllocUnits)
        aq.Add("FROMSTATUS", orgStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _adjqty)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.UNALLOCATEPICK)

    End Sub
    'End RWMS-2100


    Public Sub SetContainer(ByVal pContainerid As String, ByVal pUser As String)
        _edituser = pUser
        _editdate = DateTime.Now
        _tocontainer = pContainerid
        'DataInterface.RunSQL(String.Format("Update PICKDETAIL SET TOCONTAINER = {0}, EDITUSER = {1}, EDITDATE = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_tocontainer), _
        '        Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
    End Sub

    Public Sub CloseContainer(ByVal pUser As String)
        'Create a new line with the same params
        Dim oPickDet As PicklistDetail = Clone(pUser)
        oPickDet.Quantity = _adjqty - _pickedqty '_pickedqty
        oPickDet.AdjustedQuantity = _adjqty - _pickedqty '_pickedqty
        oPickDet.PickedQuantity = 0 '_pickedqty
        'oPickDet.Status = WMS.Lib.Statuses.Picklist.COMPLETE
        oPickDet.Status = WMS.Lib.Statuses.Picklist.RELEASED

        Me._adjqty = Me._pickedqty
        Me.Status = WMS.Lib.Statuses.Picklist.COMPLETE
        Save(pUser)

        'Check if we need to update the fromload field. If we allocate from pickloc and the difference between the fromload allocatedqty
        'to the sum of the units needed from this load (according to the other picklists in pickdetail table) is equal to what we need to pick
        'in this line, only then keep the fromload in this new detail line. Otherwise remove it(so a new load will be allocated from the
        'pickloc.
        If PickLoc.Exists(oPickDet.Consignee, oPickDet.SKU, oPickDet.FromLocation, oPickDet.FromWarehousearea) Then
            Dim sql As String = String.Format("Select isnull(sum(adjqty-pickedqty),0) from pickdetail where (status<>{0} and status<>{1}) and fromload={2}",
            Made4Net.Shared.FormatField(WMS.Lib.Statuses.Picklist.CANCELED), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Picklist.COMPLETE),
            Made4Net.Shared.FormatField(oPickDet.FromLoad))
            Dim qtyAllocatedAccordingToPickDetails As Decimal = Convert.ToDecimal(DataInterface.ExecuteScalar(sql))
            Dim loadObj As New WMS.Logic.Load(oPickDet.FromLoad)
            If loadObj.UNITSALLOCATED - qtyAllocatedAccordingToPickDetails <> oPickDet.Quantity Then
                oPickDet.FromLoad = ""
            End If
        End If
        oPickDet.ToContainer = ""
        oPickDet.ToLoad = ""
        oPickDet.Save(pUser)

        'And Change the Original Pick Line
        '_tocontainer = ""
        '_toload = ""
        '_adjqty = _adjqty - _pickedqty
        '_pickedqty = 0
        'Me._adjqty = Me._pickedqty
        'Me.Status = WMS.Lib.Statuses.Picklist.COMPLETE
        'Save(pUser)
    End Sub

    Public Sub UpdateToContainer(ByVal containerID As String)
        Dim sql As String = String.Format("Update PickDetail WITH (UPDLOCK) SET TOCONTAINER = '{0}' where PICKLIST = '{1}' And PICKLISTLINE = {2}", containerID, Me.PickList, Me.PickListLine)
        DataInterface.RunSQL(sql)
    End Sub

    Public Function Clone(ByVal pUser As String) As PicklistDetail
        Dim oNewPickDet As New PicklistDetail
        oNewPickDet.AddDate = DateTime.Now
        oNewPickDet.AddUser = pUser
        oNewPickDet.AdjustedQuantity = _adjqty
        oNewPickDet.Consignee = _consignee
        oNewPickDet.EditDate = DateTime.Now
        oNewPickDet.EditUser = pUser
        oNewPickDet.FromLoad = _fromload
        oNewPickDet.FromLocation = _fromlocation
        oNewPickDet.FromWarehousearea = _fromwarehousearea
        oNewPickDet.OrderId = _orderid
        oNewPickDet.OrderLine = _orderline
        oNewPickDet.PickedQuantity = _pickedqty
        oNewPickDet.PickList = _picklist
        oNewPickDet.PickListLine = GetNextLineId()
        oNewPickDet.PickRegion = _pickregion
        oNewPickDet.Quantity = _qty
        oNewPickDet.SKU = _sku
        oNewPickDet.Status = _status
        oNewPickDet.ToContainer = _tocontainer
        oNewPickDet.ToLoad = _toload
        oNewPickDet.ToLocation = _tolocation
        oNewPickDet.ToWarehousearea = _towarehousearea
        oNewPickDet.UOM = _uom
        Return oNewPickDet
    End Function

    Private Sub RaisePickLoadEvent(ByVal pLoadStatus As String, ByVal pUser As String)
        Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickLoad)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PICKLOAD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", _fromlocation)
        aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
        aq.Add("FROMQTY", _pickedqty)
        aq.Add("TOCONTAINER", _tocontainer)
        aq.Add("FROMSTATUS", pLoadStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _toload)
        aq.Add("TOLOC", _fromlocation)
        aq.Add("TOWAREHOUSEAREA", _towarehousearea)
        aq.Add("TOQTY", _pickedqty)
        aq.Add("TOSTATUS", pLoadStatus)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.PICKLOAD)
    End Sub

    Public Sub UnPick(ByVal puser As String)
        Dim ord As New OutboundOrderHeader(_consignee, _orderid)
        Dim ld As Load
        If _toload = "" Or _toload Is Nothing Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Unpick Line. Invalid target Load", "Cannot Unpick Line. Invalid target Load")
        Else
            ld = New Load(_toload)
        End If
        'check for the picklist and the order status
        If _status = WMS.Lib.Statuses.Picklist.CANCELED Or _status = WMS.Lib.Statuses.Picklist.PLANNED Or _status = WMS.Lib.Statuses.Picklist.RELEASED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Unpick Line.Bad Status", "Cannot Unpick Line.Bad Status")
        End If
        If ord.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Unpick Line.Bad Order Status", "Cannot Unpick Line.Bad Status")
        End If

        'Check if the Location of TOLOAD is not Empty
        If ld.LOCATION <> "" Then
            'change the activity status of the new load created and remove all containers
            ld.SetActivityStatus("", puser)
            ld.Move(ld.LOCATION, ld.WAREHOUSEAREA, ld.SUBLOCATION, puser)
        Else
            'change the activity status of the new load created and remove all containers
            ld.SetActivityStatus("", puser)
            ld.Move(_fromlocation, ld.WAREHOUSEAREA, ld.SUBLOCATION, puser)
        End If

        ''DO NOT REMOVE THE LOAD FROM CONTAINER IN ANY CASE!!!!!!!!!
        ''WE WILL NOT BE ABLE TO IDENTIFY THE LOAD WITHOUT ITS CINTAINER UIN THIS STAGE OF PROCESS,
        ''SO WE WILL NOT BE ABLE TO PUT IT AWAY BACK TO WAREHOUSE....

        ''If ld.ContainerId <> String.Empty Then
        ''    ld.RemoveFromContainer()
        ''End If

        'update the picked qty of the order line
        ord.UnPick(_orderline, ld.UNITS, _toload, puser)

        Dim oldStat As String = _status
        _status = WMS.Lib.Statuses.Picklist.CANCELED
        _edituser = puser
        _editdate = DateTime.Now
        DataInterface.RunSQL(String.Format("UPDATE PICKDETAIL SET STATUS = {0},EDITUSER = {1}, EDITDATE = {2} WHERE {3}",
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListLineUnPick)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _picklist)
        aq.Add("DOCUMENTLINE", _picklistline)
        aq.Add("FROMLOAD", _fromload)
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", _pickedqty)
        aq.Add("FROMSTATUS", oldStat)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _toload)
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", _pickedqty)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.UNPICK)
    End Sub

#Region "Pick Short"

    Public Sub SystemPickShort(ByVal pSystemPickShortType As String, ByVal pQtyToAdjust As Decimal, ByVal oOrder As OutboundOrderHeader, ByVal oFromLoad As Load, ByVal pUser As String)
        'And update the Order and pickdetail object according to the release strategy
        'The Load should not be updated since if we got to this step - the load itself caused the pickshort, so in this case the qty
        'and the allocated qty of the load object is less than required - dont reduce it and make it worse.
        Select Case pSystemPickShortType.ToLower

            Case WMS.Logic.SystemPickShort.PickPartialCancelException.ToLower
                oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                oOrder.Lines.Line(_orderline).CancelExceptions(pUser, pQtyToAdjust)

            Case WMS.Logic.SystemPickShort.PickPartialCreateException.ToLower
                oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)

            Case WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower
                If _status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                    Dim qtyToUnalloc As Decimal = pQtyToAdjust
                    If oFromLoad Is Nothing AndAlso qtyToUnalloc > 0 Then
                        Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                        oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, qtyToUnalloc, pUser)
                    End If
                End If
            Case WMS.Logic.SystemPickShort.PickZeroCancelException.ToLower
                'do not pick anything
                oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                oOrder.Lines.Line(_orderline).CancelExceptions(pUser, pQtyToAdjust)
                If Not oFromLoad Is Nothing Then
                    oFromLoad.unAllocate(Math.Min(pQtyToAdjust, oFromLoad.UNITSALLOCATED), pUser)
                End If

            Case WMS.Logic.SystemPickShort.PickZeroCreateException.ToLower
                oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                If Not oFromLoad Is Nothing Then
                    oFromLoad.unAllocate(Math.Min(pQtyToAdjust, oFromLoad.UNITSALLOCATED), pUser)
                End If

            Case WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower
                'Do not update anything - pick zero!
                'If it's the 2nd try to pick, we pick 0 and create exception.
                If _status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                    Dim qtyToUnalloc As Decimal = pQtyToAdjust
                    If Not oFromLoad Is Nothing Then
                        If qtyToUnalloc > oFromLoad.UNITSALLOCATED Then
                            oFromLoad.unAllocate(oFromLoad.UNITSALLOCATED, pUser)
                            qtyToUnalloc -= oFromLoad.UNITSALLOCATED
                        Else
                            oFromLoad.unAllocate(qtyToUnalloc, pUser)
                            qtyToUnalloc = 0
                        End If
                    End If
                    If qtyToUnalloc > 0 Then
                        Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                        oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, qtyToUnalloc, pUser)
                    End If
                End If
        End Select
    End Sub

    Private Sub UserPickShort(ByVal pUserPickShortType As String, ByVal pQtyToAdjust As Decimal, ByVal oOrder As OutboundOrderHeader, ByVal oFromLoad As Load, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String, ByVal pUnallocLoadOnUserPickShort As Boolean)
        Select Case pUserPickShortType.ToLower
            Case WMS.Logic.UserPickShort.PickPartialCreateException.ToLower
                'Same as we did till now, always unallocated and create the exception (Old Code)
                'RWMS-665 and RWMS-790 - Commented
                'Uncommented and added a condition for for PWMS-801(RWMS-865) Start
                'RWMS-1806(RWMS-1893) - Commented the Condition If (_pickedqty = 0) Then
                'Added for RWMS-2153 and RWMS-2145 Start
                'Commented for RWMS-2346 Start
                'If (_pickedqty > 0) Then
                '    oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                'End If
                'Commented for RWMS-2346 End
                'Added for RWMS-2153 and RWMS-2145 End

                'Uncommented and added a condition for for PWMS-801(RWMS-865) End

                If Not oFromLoad Is Nothing AndAlso pQtyToAdjust > oFromLoad.UNITSALLOCATED Then
                    Dim unitsAlloc As Decimal = oFromLoad.UNITSALLOCATED
                    If pUnallocLoadOnUserPickShort Then
                        oFromLoad.unAllocate(oFromLoad.UNITSALLOCATED, pUser)
                        pQtyToAdjust -= unitsAlloc
                    End If
                Else
                    If pUnallocLoadOnUserPickShort Then
                        If Not oFromLoad Is Nothing Then
                            oFromLoad.unAllocate(pQtyToAdjust, pUser)
                        End If
                        'RWMS-1806(RWMS-1893) - Commented the code pQtyToAdjust = 0
                        'Uncomented for RWMS-2346 Start
                        pQtyToAdjust = 0 ' oFromLoad.UNITSALLOCATED
                        'Uncomented for RWMS-2346 End
                    End If
                End If
                'RWMS-2387 RWMS-2335 There is no need to unallocated from pickloc , we are substracting from picloc when the pick presented to user
                'If pQtyToAdjust > 0 AndAlso PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
                '    Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                '    oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pQtyToAdjust, pUser)
                'End If
            Case WMS.Logic.UserPickShort.PickPartialCancelException.ToLower
                '_pickedqty = _pickedqty + pQtyToAdjust
                oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                oOrder.Lines.Line(_orderline).CancelExceptions(pUser, pQtyToAdjust)
                If Not oFromLoad Is Nothing AndAlso pQtyToAdjust > oFromLoad.UNITSALLOCATED Then
                    Dim unitsAlloc As Decimal = oFromLoad.UNITSALLOCATED
                    If pUnallocLoadOnUserPickShort Then
                        If Not oFromLoad Is Nothing Then
                            oFromLoad.unAllocate(oFromLoad.UNITSALLOCATED, pUser)
                        End If
                        pQtyToAdjust -= unitsAlloc
                    End If
                Else
                    If pUnallocLoadOnUserPickShort Then
                        If Not oFromLoad Is Nothing Then
                            oFromLoad.unAllocate(pQtyToAdjust, pUser)
                        End If
                        pQtyToAdjust = 0 ' oFromLoad.UNITSALLOCATED
                    End If
                End If

                If pQtyToAdjust > 0 AndAlso PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
                    Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                    oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pQtyToAdjust, pUser)
                End If
                '_adjqty = _pickedqty
            Case WMS.Logic.UserPickShort.PickPartialLeaveOpen.ToLower
                If _status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    'At least second time there
                    '_pickedqty = _pickedqty + pQtyToAdjust
                    'Same as we did till now, always unallocated and create the exception (Old Code)
                    oOrder.unAllocateLine(_orderline, pQtyToAdjust, pUser)
                    If Not oFromLoad Is Nothing AndAlso pQtyToAdjust > oFromLoad.UNITSALLOCATED Then
                        Dim unitsAlloc As Decimal = oFromLoad.UNITSALLOCATED
                        If pUnallocLoadOnUserPickShort Then
                            oFromLoad.unAllocate(oFromLoad.UNITSALLOCATED, pUser)
                            pQtyToAdjust -= unitsAlloc
                        End If
                    Else
                        If pUnallocLoadOnUserPickShort Then
                            If Not oFromLoad Is Nothing Then
                                oFromLoad.unAllocate(pQtyToAdjust, pUser)
                            End If
                            pQtyToAdjust = 0 ' oFromLoad.UNITSALLOCATED
                        End If
                    End If

                    If pQtyToAdjust > 0 AndAlso PickLoc.Exists(_consignee, _sku, _fromlocation, _fromwarehousearea) Then
                        Dim oPickLoc As New WMS.Logic.PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                        oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pQtyToAdjust, pUser)
                    End If
                End If
        End Select
        oOrder.UpdateHeaderAfterPick(pUser)
    End Sub

    Public Function GetReleaseStrategy() As ReleaseStrategyDetail
        Dim SQL As String = String.Format("SELECT * FROM PICKHEADER WHERE (PICKLIST = '{0}')", _picklist)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        Dim dr As DataRow = dt.Rows(0)
        Dim oPlanStart As New PlanStrategy(Convert.ToString(dr("strategyid")))
        For Each relStrat As ReleaseStrategyDetail In oPlanStart.ReleaseStrategyDetails
            If relStrat.PickType.ToLower = Convert.ToString(dr("picktype")).ToLower Then
                Return relStrat
            End If
        Next
        Return oPlanStart.ReleaseStrategyDetails(0)
    End Function

    'RWMS-1306/RWMS-971 - complete the picklistline but donot create the new picklistline
    Public Sub NoSplitPartPickedLine()
        _adjqty = _pickedqty
        _status = WMS.Lib.Statuses.Picklist.COMPLETE
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now
        Save(WMS.Lib.USERS.SYSTEMUSER)
    End Sub
    'End RWMS-1306/RWMS-971

    Friend Function SplitPartPickedLine() As PicklistDetail
        Dim newPickDetail As PicklistDetail = Clone(WMS.Lib.USERS.SYSTEMUSER)
        newPickDetail.PickedQuantity = 0
        newPickDetail.Quantity = AdjustedQuantity - PickedQuantity
        newPickDetail.AdjustedQuantity = newPickDetail.Quantity
        newPickDetail.Status = WMS.Lib.Statuses.Picklist.RELEASED
        newPickDetail.ToLoad = ""
        newPickDetail.ToContainer = ""
        If WMS.Logic.Load.Exists(newPickDetail.FromLoad) Then
            Dim oLoad As New WMS.Logic.Load(newPickDetail.FromLoad)
            If (oLoad.UNITS = 0 OrElse oLoad.UNITSALLOCATED = 0) AndAlso
                        WMS.Logic.PickLoc.Exists(newPickDetail.Consignee, newPickDetail.SKU, newPickDetail.FromLocation, newPickDetail.FromWarehousearea) Then
                newPickDetail.FromLoad = ""
            End If
        Else
            newPickDetail.FromLoad = ""
        End If
        'newPickDetail.ToLocation = ""
        newPickDetail.Save(WMS.Lib.USERS.SYSTEMUSER)
        'Close the line, but leave all loads allocation and order allocation as is for the next line based on this one...
        _adjqty = _pickedqty
        _status = WMS.Lib.Statuses.Picklist.COMPLETE
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now
        Save(WMS.Lib.USERS.SYSTEMUSER)
        Return newPickDetail
    End Function

#End Region

#Region "Picking Location Allocation"

    Public Function AllocateLoadFromPickLoc(ByVal oReleaseStratDetail As ReleaseStrategyDetail) As ArrayList
        Dim NewPickDetails As New ArrayList
        If _fromload <> String.Empty Then
            Return Nothing
        End If
        Dim Sql As String
        Dim QtyFullfilled As Decimal = 0
        Dim AvailableLoads As New DataTable
        Sql = String.Format("select distinct loadid from vPickLocAllocationInventory where picklist = {0} and picklistline = {1}", Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_picklistline))
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, AvailableLoads)
        If AvailableLoads.Rows.Count > 0 Then
            For Each drLoad As DataRow In AvailableLoads.Rows
                QtyFullfilled += AllocateLoad(drLoad("loadid"), NewPickDetails, oReleaseStratDetail)
                'If QtyFullfilled = _adjqty - _pickedqty Then
                Exit For
                'End If
            Next
        Else
            Select Case oReleaseStratDetail.SystemPickShort
                Case WMS.Logic.SystemPickShort.PickZeroCancelException, WMS.Logic.SystemPickShort.PickPartialCancelException _
                        , WMS.Logic.SystemPickShort.PickZeroCreateException, WMS.Logic.SystemPickShort.PickPartialCreateException

                    Dim oPickLoc As New PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                    oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, _adjqty, WMS.Lib.USERS.SYSTEMUSER)

                Case WMS.Logic.SystemPickShort.PickPartialLeaveOpen, WMS.Logic.SystemPickShort.PickZeroLeaveOpen
                    If _status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        Dim oPickLoc As New PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)
                        oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, _adjqty, WMS.Lib.USERS.SYSTEMUSER)
                    End If
            End Select
        End If
        If NewPickDetails.Count = 0 Then
            Return Nothing
        Else
            Return NewPickDetails
        End If
    End Function

    <Obsolete("This function is not in use any more. please use new method", False)> Private Function AllocateLoad(ByVal pLoadId As String, ByRef pNewPickDetails As ArrayList, ByVal oReleaseStratDetail As ReleaseStrategyDetail) As Decimal
        Dim oSku As New SKU(_consignee, _sku)
        Dim oOrder As New OutboundOrderHeader(_consignee, _orderid)
        Dim oPickLoc As New PickLoc(_fromlocation, _fromwarehousearea, _consignee, _sku)

        Dim oRemainPickDet As PicklistDetail
        Dim QtySuffice As Boolean = True
        Dim openQty As Decimal = _adjqty - _pickedqty
        Dim oLoad As New WMS.Logic.Load(pLoadId)
        Dim dAvailableLoadUnits As Decimal = oLoad.UNITS - oLoad.UNITSALLOCATED
        Dim unitsPerUom As Decimal = 0
        Dim CurrentUOMQty As Decimal = 0
        Dim checkUOM As String = _uom
        If dAvailableLoadUnits < openQty Then
            If oReleaseStratDetail.SystemPickShort.Equals(WMS.Logic.SystemPickShort.PickZeroLeaveOpen, StringComparison.OrdinalIgnoreCase) _
                OrElse oReleaseStratDetail.SystemPickShort.Equals(WMS.Logic.SystemPickShort.PickPartialLeaveOpen, StringComparison.OrdinalIgnoreCase) Then

                If _status <> WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    QtySuffice = False
                    Dim newUom As String = checkUOM
                    oRemainPickDet = Clone(WMS.Lib.USERS.SYSTEMUSER)
                    oRemainPickDet.AdjustedQuantity = openQty - dAvailableLoadUnits
                    unitsPerUom = oSku.ConvertToUnits(checkUOM)
                    While oRemainPickDet.AdjustedQuantity < unitsPerUom
                        newUom = oSku.getNextUom(checkUOM)
                        If newUom Is Nothing Then
                            Exit While
                        End If
                        unitsPerUom = oSku.ConvertToUnits(newUom)
                    End While
                    oRemainPickDet.UOM = newUom
                    oRemainPickDet.Save(WMS.Lib.USERS.SYSTEMUSER)
                    pNewPickDetails.Add(oRemainPickDet)
                Else
                    oOrder.Lines.Line(_orderline).unAllocate(openQty - dAvailableLoadUnits, WMS.Lib.USERS.SYSTEMUSER)
                    oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, dAvailableLoadUnits, WMS.Lib.USERS.SYSTEMUSER)
                End If
            Else
                Select Case oReleaseStratDetail.SystemPickShort
                    Case WMS.Logic.SystemPickShort.PickZeroCancelException, WMS.Logic.SystemPickShort.PickPartialCancelException
                        oOrder.Lines.Line(_orderline).CancelExceptions(WMS.Lib.USERS.SYSTEMUSER, openQty - dAvailableLoadUnits)
                        oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, dAvailableLoadUnits, WMS.Lib.USERS.SYSTEMUSER)
                    Case WMS.Logic.SystemPickShort.PickZeroCreateException, WMS.Logic.SystemPickShort.PickPartialCreateException
                        oOrder.unAllocateLine(_orderline, openQty - dAvailableLoadUnits, WMS.Lib.USERS.SYSTEMUSER)
                        oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, dAvailableLoadUnits, WMS.Lib.USERS.SYSTEMUSER)
                End Select
            End If
        Else
            dAvailableLoadUnits = openQty
            oPickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, dAvailableLoadUnits, WMS.Lib.USERS.SYSTEMUSER)
        End If
        While openQty > 0 And dAvailableLoadUnits > 0
            unitsPerUom = oSku.ConvertToUnits(checkUOM)
            If Not CanPickPartialUOM() Then
                CurrentUOMQty = (Math.Floor(dAvailableLoadUnits / unitsPerUom)) * unitsPerUom
            Else
                CurrentUOMQty = dAvailableLoadUnits
            End If
            If CurrentUOMQty > 0 Then
                oLoad.Allocate(CurrentUOMQty, WMS.Lib.USERS.SYSTEMUSER)
            End If
            If _uom = checkUOM Then
                _fromload = pLoadId
                _adjqty = CurrentUOMQty
                _uom = checkUOM
                If _adjqty = 0 Then
                    _status = WMS.Lib.Statuses.Picklist.CANCELED
                End If
                _editdate = DateTime.Now
                _edituser = WMS.Lib.USERS.SYSTEMUSER
                Dim Sql As String = String.Format("update pickdetail set fromload = {0},adjqty={1},status={2},editdate={3},edituser={4} where {5}",
                    Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
                DataInterface.RunSQL(Sql)
            Else
                If CurrentUOMQty > 0 Then
                    Dim oPickDet As PicklistDetail = Clone(WMS.Lib.USERS.SYSTEMUSER)
                    oPickDet.PickListLine = _picklistline + 1
                    oPickDet.FromLoad = pLoadId
                    oPickDet.AdjustedQuantity = CurrentUOMQty
                    If oPickDet.AdjustedQuantity > 0 Then
                        oPickDet.Status = WMS.Lib.Statuses.Picklist.RELEASED
                    End If
                    oPickDet.UOM = checkUOM
                    oPickDet.EditDate = DateTime.Now
                    oPickDet.EditUser = WMS.Lib.USERS.SYSTEMUSER
                    AddLineAfter(oPickDet)
                    pNewPickDetails.Add(oPickDet)
                End If
            End If
            dAvailableLoadUnits -= CurrentUOMQty
            openQty -= CurrentUOMQty
            checkUOM = oSku.getNextUom(checkUOM)
            If (checkUOM Is Nothing) Then
                Exit While
            End If
        End While
        Return dAvailableLoadUnits
    End Function

    Private Function CanPickPartialUOM() As Boolean
        Dim ret As Boolean = False
        Dim oPicklist As New Picklist(_picklist)
        Return oPicklist.CanPickPartialUOM
    End Function

    Private Sub AddLineAfter(ByVal oPickDet As PicklistDetail)
        Dim oSql As String = String.Format("update pickdetail set picklistline=picklistline+1 where picklist ='{0}' and picklistline > {1}", _picklist, _picklistline)
        DataInterface.RunSQL(oSql)
        oPickDet.Save(WMS.Lib.USERS.SYSTEMUSER)
    End Sub

    Public Sub AllocateLoad(ByVal pLoad As Load, ByVal pUOM As String, ByVal pUnitsToAllocate As Decimal, ByVal pUser As String)
        'If PickLoc.Exists(pLoad.LOCATION) Then
        '    PickLoc.GetPickLoc(pLoad.LOCATION).AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pUnitsToAllocate, WMS.Lib.USERS.SYSTEMUSER)
        'End If
        pLoad.Allocate(pUnitsToAllocate, WMS.Lib.USERS.SYSTEMUSER)

        '_adjqty = pUnitsToAllocate
        _fromload = pLoad.LOADID
        _uom = pUOM
        _editdate = DateTime.Now
        _edituser = pUser
        Dim Sql As String
        'Sql = String.Format("update pickdetail set fromload={0},adjqty={1},uom={2},status={3},editdate={4},edituser={5} where {6}", _
        '        Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_adjqty), Made4Net.Shared.Util.FormatField(pUOM), _
        '        Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
        '        WhereClause)

        Sql = String.Format("update pickdetail set fromload={0},uom={1},editdate={2},edituser={3} where {4}",
                Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(pUOM),
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser),
                WhereClause)
        DataInterface.RunSQL(Sql)

    End Sub

    Public Function SplitLine(ByVal pQuantity As Decimal) As PicklistDetail
        Dim pckListDet As PicklistDetail = Clone(WMS.Lib.USERS.SYSTEMUSER)
        pckListDet.FromLoad = ""
        pckListDet.AdjustedQuantity = pQuantity
        pckListDet.Quantity = pQuantity
        pckListDet.PickedQuantity = 0
        Return pckListDet
    End Function

#End Region

#End Region

#Region "Release"

    Public Sub Release()
        If _status = WMS.Lib.Statuses.Picklist.PLANNED Then
            _status = WMS.Lib.Statuses.Picklist.RELEASED
            _editdate = DateTime.Now
            _edituser = WMS.Lib.USERS.SYSTEMUSER
            Dim sql As String = String.Format("Update pickdetail set status = {0},editdate = {1},edituser = {2} where {3}",
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(sql)
        End If
    End Sub

#End Region

#Region "Print Pick Labels"

    Public Sub PrintPickLabels(ByVal LabelType As String, ByVal lblPrinter As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", LabelType)
        qSender.Add("LabelType", LabelType)
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("PICKLIST", _picklist)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("PICKLIST", _picklist)
        ht.Hash.Add("PICKLISTLINE", _picklistline)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Send("Label", "Picklist Pick Label")
    End Sub

#End Region

#End Region
#End Region
End Class
'Public Class CaseId

'    Private _caseID As String
'    Public Property CaseID() As String
'        Get
'            Return _caseID
'        End Get
'        Set(ByVal value As String)
'            _caseID = value
'        End Set
'    End Property

'    Private _consignee As String
'    Public Property Consignee() As String
'        Get
'            Return _consignee
'        End Get
'        Set(ByVal value As String)
'            _consignee = value
'        End Set
'    End Property

'    Private _picklist As String
'    Public Property PickList() As String
'        Get
'            Return _picklist
'        End Get
'        Set(ByVal value As String)
'            _picklist = value
'        End Set
'    End Property

'    Private _PickListLine As Integer
'    Public Property PickListLine() As Integer
'        Get
'            Return _PickListLine
'        End Get
'        Set(ByVal value As Integer)
'            _PickListLine = value
'        End Set
'    End Property
'    Private _OrderId As String
'    Public Property OrderId() As String
'        Get
'            Return _OrderId
'        End Get
'        Set(ByVal value As String)
'            _OrderId = value
'        End Set
'    End Property

'    Private _OrderLine As Integer
'    Public Property OrderLine() As Integer
'        Get
'            Return _OrderLine
'        End Get
'        Set(ByVal value As Integer)
'            _OrderLine = value
'        End Set
'    End Property

'    Private _FromLoad As String
'    Public Property FromLoad() As String
'        Get
'            Return _FromLoad
'        End Get
'        Set(ByVal value As String)
'            _FromLoad = value
'        End Set
'    End Property
'    Private _ToLoad As String
'    Public Property ToLoad() As String
'        Get
'            Return _ToLoad
'        End Get
'        Set(ByVal value As String)
'            _ToLoad = value
'        End Set
'    End Property
'    Private _ToContainer As String
'    Public Property ToContainer() As String
'        Get
'            Return _ToContainer
'        End Get
'        Set(ByVal value As String)
'            _ToContainer = value
'        End Set
'    End Property

'    Private _status As String
'    Public Property Status() As String
'        Get
'            Return _status
'        End Get
'        Set(ByVal value As String)
'            _status = value
'        End Set
'    End Property
'    Public ReadOnly Property WhereClause() As String
'        Get
'            Return String.Format(" CASEID = '{0}' ", _caseID)
'        End Get
'    End Property

'    Private _addDate As DateTime
'    Public Property AddDate() As DateTime
'        Get
'            Return _addDate
'        End Get
'        Set(ByVal value As DateTime)
'            _addDate = value
'        End Set
'    End Property

'    Private _addUser As String
'    Public Property AddUser() As String
'        Get
'            Return _addUser
'        End Get
'        Set(ByVal value As String)
'            _addUser = value
'        End Set
'    End Property

'    Private _editDate As DateTime
'    Public Property EditDate() As DateTime
'        Get
'            Return _editDate
'        End Get
'        Set(ByVal value As DateTime)
'            _editDate = value
'        End Set
'    End Property
'    Private _editUser As String
'    Public Property EditUser() As String
'        Get
'            Return _editUser
'        End Get
'        Set(ByVal value As String)
'            _editUser = value
'        End Set
'    End Property

'    Public Sub New(ByVal CaseId As String)
'        _caseID = CaseId
'        Load()
'    End Sub

'    Public Sub New()

'    End Sub


'    Private Sub Load()
'        Dim dt As New DataTable
'        Dim sql As String = String.Format("Select * from CaseID where {0}", WhereClause)
'        DataInterface.FillDataset(sql, dt)
'        If dt.Rows.Count = 0 Then
'            Throw New Made4Net.Shared.M4NException("CaseID does not exists")
'        End If
'        Dim dr As DataRow = dt.Rows(0)
'        _caseID = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CASEID"))
'        _consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE"))
'        _picklist = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKLIST"))
'        _PickListLine = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKLINE"))
'        _OrderId = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERID"))
'        _OrderLine = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERLINE"))
'        _FromLoad = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMLOAD"))
'        _ToLoad = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOLOAD"))
'        _ToContainer = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOCONTAINER"))
'        _status = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATUS"))
'        _addDate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
'        _addUser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
'        _editDate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
'        _editUser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
'    End Sub

'    Private Sub Save(caseID As String, user As String)
'        Dim Sql As String
'        If WMS.Logic.CaseId.Exists(caseID) Then
'            Sql = $"Update caseid set consignee='{Consignee}', picklist='{PickList}',pickline={PickListLine},orderid='{OrderId}',orderline={OrderLine},fromload='{FromLoad}',toload='{ToLoad}',tocontainer='{ToContainer}',status='{Status}',editdate='{DateTime.Now}',edituser='{user}'"
'        Else
'            Sql = $" INSERT INTO CASEID ([CASEID],[CONSIGNEE],[PICKLIST],[PICKLINE],[ORDERID],[ORDERLINE],[FROMLOAD],[TOLOAD],[TOCONTAINER],[STATUS], ADDDATE,ADDUSER,EDITDATE,EDITUSER) values ('{caseID}','{Consignee}','{PickList}',{PickListLine},'{OrderId}',{OrderLine},'{FromLoad}','{ToLoad}','{ToContainer}','{Status}',{Made4Net.Shared.Util.FormatField(_addDate)},'{user}',{Made4Net.Shared.Util.FormatField(_editDate)},'{user}')"
'        End If
'        DataInterface.RunSQL(Sql)
'    End Sub

'    Public Sub UpdateStatus(caseId As String, status As String, user As String)
'        _status = status
'        Save(_caseID, user)
'    End Sub

'    Public Shared Function Create(Consignee As String, PickLIst As String, PickListLine As Integer, OrderId As String, OrderLine As Integer, FromLoad As String, ToLoad As String, ToContainer As String, Status As String, User As String) As CaseId
'        Dim counter As String = getNextCounter("CASEID")
'        Dim caseId As CaseId = New CaseId()
'        caseId.CaseID = counter
'        caseId.Consignee = Consignee
'        caseId.PickList = PickLIst
'        caseId.PickListLine = PickListLine
'        caseId.OrderId = OrderId
'        caseId.OrderLine = OrderLine
'        caseId.FromLoad = FromLoad
'        caseId.ToLoad = ToLoad
'        caseId.ToContainer = ToContainer
'        caseId.Status = Status
'        caseId.AddDate = Date.Now
'        caseId.EditDate = DateTime.Now
'        caseId.AddUser = User
'        caseId.EditUser = User
'        caseId.Save(counter, User)
'        Return New CaseId(counter)
'    End Function



'    Public Shared Function Exists(ByVal CaseID As String) As Boolean
'        Dim sql As String = String.Format("Select count(1) from CASEID where CASEID = '{0}'", CaseID)
'        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
'    End Function

'    Public Shared Sub StageCaseIDs(container As String, User As String)
'        Dim sql As String = $"select caseid from caseid where tocontainer='{container}'"
'        Dim dt As DataTable = New DataTable()
'        DataInterface.FillDataset(sql, dt)
'        For Each dr As DataRow In dt.Rows
'            Dim caseid As CaseId = New CaseId(Convert.ToString(dr("caseid")))
'            caseid._status = "STAGED"
'            caseid.Save(caseid.CaseID, User)
'        Next
'    End Sub

'End Class

