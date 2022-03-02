Imports Made4Net.DataAccess
Imports WMS.Lib

<CLSCompliant(False)> Public Class Task

#Region "Variables"

#Region "Primary Keys"

    Protected _task As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _task_type As String = String.Empty
    Protected _task_sub_type As String = String.Empty
    Protected _status As String = String.Empty
    Protected _consignee As String
    Protected _sku As String
    Protected _fromlocation As String = String.Empty
    Protected _tolocation As String = String.Empty
    Protected _fromwarehousearea As String = String.Empty
    Protected _towarehousearea As String = String.Empty
    Protected _fromload As String = String.Empty
    Protected _caseid As String = String.Empty
    Protected _toload As String = String.Empty
    Protected _fromcontainer As String = String.Empty
    Protected _tocontainer As String = String.Empty
    Protected _picklist As String = String.Empty
    Protected _parallelpicklist As String = String.Empty
    Protected _consolidation As String = String.Empty
    Protected _replenishment As String = String.Empty
    Protected _countid As String = String.Empty
    Protected _priority As Int32
    Protected _assigned As Boolean
    Protected _userid As String = String.Empty
    Protected _document As String = String.Empty
    Protected _documentline As Int32
    Protected _starttime As DateTime
    Protected _endtime As DateTime
    Protected _assignedtime As DateTime
    Protected _executiontime As Int32
    Protected _startlocation As String = String.Empty
    Protected _startwarehousearea As String = String.Empty
    Protected _assignmenttype As String = String.Empty
    Protected _executionlocation As String = String.Empty
    Protected _executionwarehousearea As String = String.Empty
    Protected _stdtime As Double = 0D
    Protected _problemflag As Boolean
    Protected _problemrc As String = String.Empty
    Protected _problemcodecompletetask As Boolean = False
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _prepopulateLocation As Boolean ''RWMS-1277
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " TASK = '" & _task & "'"
        End Get
    End Property

    Public ReadOnly Property WhereClauseForSettingStartTime() As String
        Get
            Return " TASK = '" & _task & "' AND STARTTIME is null"
        End Get
    End Property

    Public ReadOnly Property TASK() As String
        Get
            Return _task
        End Get
    End Property

    Public Property TASKTYPE() As String
        Get
            Return _task_type
        End Get
        Set(ByVal Value As String)
            _task_type = Value
        End Set
    End Property

    Public Property TASKSUBTYPE() As String
        Get
            Return _task_sub_type
        End Get
        Set(ByVal Value As String)
            _task_sub_type = Value
        End Set
    End Property

    Public ReadOnly Property STATUS() As String
        Get
            Return _status
        End Get
    End Property

    Public Property Picklist() As String
        Get
            Return _picklist
        End Get
        Set(ByVal Value As String)
            _picklist = Value
        End Set
    End Property

    Public Property ParallelPicklist() As String
        Get
            Return _parallelpicklist
        End Get
        Set(ByVal Value As String)
            _parallelpicklist = Value
        End Set
    End Property

    Public Property Consolidation() As String
        Get
            Return _consolidation
        End Get
        Set(ByVal Value As String)
            _consolidation = Value
        End Set
    End Property

    Public Property Replenishment() As String
        Get
            Return _replenishment
        End Get
        Set(ByVal Value As String)
            _replenishment = Value
        End Set
    End Property

    Public Property COUNTID() As String
        Get
            Return _countid
        End Get
        Set(ByVal Value As String)
            _countid = Value
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

    Public Property FROMLOCATION() As String
        Get
            Return _fromlocation
        End Get
        Set(ByVal Value As String)
            _fromlocation = Value
        End Set
    End Property

    Public Property FROMWAREHOUSEAREA() As String
        Get
            Return _fromwarehousearea
        End Get
        Set(ByVal Value As String)
            _fromwarehousearea = Value
        End Set
    End Property

    Public Property TOLOCATION() As String
        Set(ByVal Value As String)
            _tolocation = Value
        End Set
        Get
            Return _tolocation
        End Get
    End Property

    Public Property TOWAREHOUSEAREA() As String
        Set(ByVal Value As String)
            _towarehousearea = Value
        End Set
        Get
            Return _towarehousearea
        End Get
    End Property
    Public Property CASEID() As String
        Get
            Return _caseid
        End Get
        Set(ByVal Value As String)
            _caseid = Value
        End Set
    End Property
    Public Property FROMLOAD() As String
        Get
            Return _fromload
        End Get
        Set(ByVal Value As String)
            _fromload = Value
        End Set
    End Property

    Public Property TOLOAD() As String
        Get
            Return _toload
        End Get
        Set(ByVal Value As String)
            _toload = Value
        End Set
    End Property

    Public Property FromContainer() As String
        Get
            Return _fromcontainer
        End Get
        Set(ByVal Value As String)
            _fromcontainer = Value
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

    Public Property PRIORITY() As Int32
        Get
            Return _priority
        End Get
        Set(ByVal Value As Int32)
            _priority = Value
        End Set
    End Property

    Public Property ASSIGNED() As Boolean
        Get
            Return _assigned
        End Get
        Set(ByVal Value As Boolean)
            _assigned = Value
        End Set
    End Property

    Public Property USERID() As String
        Set(ByVal Value As String)
            _userid = Value
        End Set
        Get
            Return _userid
        End Get
    End Property

    Public Property DOCUMENT() As String
        Get
            Return _document
        End Get
        Set(ByVal Value As String)
            _document = Value
        End Set
    End Property

    Public Property DOCUMENTLINE() As Int32
        Get
            Return _documentline
        End Get
        Set(ByVal Value As Int32)
            _documentline = Value
        End Set
    End Property
    Public Property STDTIME() As Double
        Get
            Return _stdtime
        End Get
        Set(ByVal Value As Double)
            _stdtime = Value
        End Set
    End Property

    Public Property STARTTIME() As DateTime
        Get
            Return _starttime
        End Get
        Set(ByVal Value As DateTime)
            _starttime = Value
        End Set
    End Property

    Public Property ENDTIME() As DateTime
        Get
            Return _endtime
        End Get
        Set(ByVal Value As DateTime)
            _endtime = Value
        End Set
    End Property

    Public Property ASSIGNEDTIME() As DateTime
        Get
            Return _assignedtime
        End Get
        Set(ByVal Value As DateTime)
            _assignedtime = Value
        End Set
    End Property

    Public Property EXECUTIONTIME() As Int32
        Get
            Return _executiontime
        End Get
        Set(ByVal Value As Int32)
            _executiontime = Value
        End Set
    End Property

    Public Property STARTLOCATION() As String
        Get
            Return _startlocation
        End Get
        Set(ByVal Value As String)
            _startlocation = Value
        End Set
    End Property

    Public Property STARTWAREHOUSEAREA() As String
        Get
            Return _startwarehousearea
        End Get
        Set(ByVal Value As String)
            _startwarehousearea = Value
        End Set
    End Property

    Public Property ExecutionLocation() As String
        Get
            Return _executionlocation
        End Get
        Set(ByVal Value As String)
            _executionlocation = Value
        End Set
    End Property

    Public Property ExecutionWarehousearea() As String
        Get
            Return _executionwarehousearea
        End Get
        Set(ByVal Value As String)
            _executionwarehousearea = Value
        End Set
    End Property

    Public Property ASSIGNMENTTYPE() As String
        Get
            Return _assignmenttype
        End Get
        Set(ByVal Value As String)
            _assignmenttype = Value
        End Set
    End Property

    Public Property PROBLEMFLAG() As Boolean
        Get
            Return _problemflag
        End Get
        Set(ByVal Value As Boolean)
            _problemflag = Value
        End Set
    End Property

    Public Property PROBLEMRC() As String
        Get
            Return _problemrc
        End Get
        Set(ByVal Value As String)
            _problemrc = Value
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

    Public ReadOnly Property CompleteTaskOnProblemCode() As Boolean
        Get
            Return _problemcodecompletetask
        End Get
    End Property
    'Start RWMS-1277
    Public Property PREPOPULATELOCATION() As Boolean
        Get
            Return _prepopulateLocation
        End Get
        Set(ByVal Value As Boolean)
            _prepopulateLocation = Value
        End Set
    End Property
    'End RWMS-1277
#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pTaskId As String, Optional ByVal LoadObj As Boolean = True)
        _task = pTaskId
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function Equals(ByVal obj As Object) As Boolean
        If Me.TASK = CType(obj, Task).TASK Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function Exists(ByVal pTaskId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from TASKS where TASK = '{0}'", pTaskId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM TASKS Where " & WhereClause
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Task does not exists", "Task does not exists")
        End If
        Load(dt.Rows(0))
    End Sub

    Public Sub Post(Optional ByRef taskId As String = Nothing)
        _status = WMS.Lib.Statuses.Task.AVAILABLE
        _assigned = False
        _userid = ""
        If _adduser = "" Then _adduser = WMS.Lib.USERS.SYSTEMUSER
        If _adduser = "" Then _edituser = WMS.Lib.USERS.SYSTEMUSER
        Save(taskId)

    End Sub

    Public Sub Post(ByVal pUser As String, Optional ByRef taskId As String = Nothing)
        _status = WMS.Lib.Statuses.Task.ASSIGNED
        _assigned = True
        _userid = pUser

        ' RWMS-1497, Get the date from WHActivity
        Dim whPreviousActivityTime As DateTime?
        Dim whActivity As New DataTable
        Dim query As String
        query = String.Format("Select * from WHACTIVITY where userid = '{0}'", USERID)
        DataInterface.FillDataset(query, whActivity)

        'Commented for RWMS-2003   
        'If whActivity.Rows.Count >= 1 And Not whActivity.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then   
        ' whPreviousActivityTime = whActivity.Rows(0)("PREVIOUSACTIVITYTIME")   
        'End Commented for RWMS-2003   
        'End If   
        'Added for RWMS-2014   

        If whActivity.Rows.Count >= 1 Then
            If Not whActivity.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
                whPreviousActivityTime = whActivity.Rows(0)("PREVIOUSACTIVITYTIME")
            End If
        End If
        'End Added for RWMS-2014  
        If whPreviousActivityTime.HasValue Then
            _starttime = whPreviousActivityTime
        Else
            _starttime = DateTime.Now
        End If
        ' RWMS-1497, Get the date from WHActivity

        _assignedtime = DateTime.Now
        _adduser = pUser
        _edituser = pUser
        Save(taskId)

        Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskAssigned)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKASGN)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _task)
        aq.Add("TASK", _task)
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
        'Added for RWMS-1971 start   
        aq.Add("TOQTY", 0)
        'Added for RWMS-1971 end 
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.TASKASGN)

    End Sub

    Public Overridable Sub ExitTask()
        _status = DataInterface.ExecuteScalar(String.Format("select status from tasks where {0}", WhereClause))
        If _status = WMS.Lib.Statuses.Task.CANCELED OrElse _status = WMS.Lib.Statuses.Task.COMPLETE Then
            Return
        End If
        Dim sql As String
        Dim fromStatus = _status
        _assigned = False
        _status = WMS.Lib.Statuses.Task.AVAILABLE
        _editdate = DateTime.Now
        _edituser = _userid
        _userid = Nothing
        ' RWMS-1877 -> RWMS-1839
        ' Set starttime as Nothing on existing the task   
        _starttime = Nothing

        sql = String.Format("update tasks set status={0},assigned={1},userid={2},editdate={3},edituser={4},ASSIGNEDTIME={5}, STARTTIME={6} where {7}", Made4Net.Shared.Util.FormatField(_status), _
       Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_assignedtime), Made4Net.Shared.Util.FormatField(_starttime), WhereClause)
        ' RWMS-1877 -> RWMS-1839 
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskReleased)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKREL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _task)
        aq.Add("TASK", _task)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", fromStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        'Added for RWMS-1971 start   
        aq.Add("TOQTY", 0)
        'Added for RWMS-1971 end 
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _userid)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _userid)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _userid)
        aq.Send(WMS.Lib.Actions.Audit.TASKREL)
    End Sub

    Public Sub SetPriority(ByVal pPriority As Int32)
        Dim sql As String
        _priority = pPriority
        _editdate = DateTime.Now
        _edituser = _userid
        sql = String.Format("update tasks set priority={0}, editdate={1},edituser={2} where {3}", _
                    Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Save(Optional ByRef taskId As String = Nothing)
        Dim SQL As String
        Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        Dim activitystatus As String
        If Exists(_task) Then
            _editdate = DateTime.Now
            ' SQL = String.Format("UPDATE TASKS SET TASKTYPE ={0}, STATUS ={1}, PICKLIST ={2}, PARALLELPICKLIST ={3}, REPLENISHMENT ={4}, CONSOLIDATION ={5}, CONSIGNEE ={6}, SKU ={7}, FROMLOCATION ={8}, FROMWAREHOUSEAREA ={29}, FROMLOAD ={9}, FROMCONTAINER ={10}, TOLOCATION ={11}, TOWAREHOUSEAREA ={30}, TOLOAD ={12}, TOCONTAINER ={13}, PRIORITY ={14}, ASSIGNED ={15}, USERID ={16}, DOCUMENT ={17}, DOCUMENTLINE ={18}, STARTTIME ={19}, ENDTIME ={20}, ASSIGNEDTIME ={21}, EXECUTIONTIME ={22}, STARTLOCATION ={23}, STARTWAREHOUSEAREA ={31}, ASSIGNMENTTYPE ={24},EXECUTIONLOCATION={25}, EXECUTIONWAREHOUSEAREA={32}, EDITDATE ={26} , COUNTID ={27},TaskSubType={33},STDTIME={34},PROBLEMFLAG={35},PROBLEMRC={36}, TASKSUBTYPE={37} WHERE {28}", _
            SQL = String.Format("UPDATE TASKS SET TASKTYPE ={0}, STATUS ={1}, PICKLIST ={2}, PARALLELPICKLIST ={3}, REPLENISHMENT ={4}, CONSOLIDATION ={5}, CONSIGNEE ={6}, SKU ={7}, FROMLOCATION ={8}, FROMWAREHOUSEAREA ={29}, CASEID ={37}, FROMLOAD ={9}, FROMCONTAINER ={10}, TOLOCATION ={11}, TOWAREHOUSEAREA ={30}, TOLOAD ={12}, TOCONTAINER ={13}, PRIORITY ={14}, ASSIGNED ={15}, USERID ={16}, DOCUMENT ={17}, DOCUMENTLINE ={18}, STARTTIME ={19}, ENDTIME ={20}, ASSIGNEDTIME ={21}, EXECUTIONTIME ={22}, STARTLOCATION ={23}, STARTWAREHOUSEAREA ={31}, ASSIGNMENTTYPE ={24},EXECUTIONLOCATION={25}, EXECUTIONWAREHOUSEAREA={32}, EDITDATE ={26} , COUNTID ={27},TaskSubType={33},STDTIME={34},PROBLEMFLAG={35},PROBLEMRC={36} WHERE {28}",
                                Made4Net.Shared.Util.FormatField(_task_type), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_parallelpicklist), Made4Net.Shared.Util.FormatField(_replenishment), Made4Net.Shared.Util.FormatField(_consolidation), Made4Net.Shared.Util.FormatField(_consignee),
                                Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromcontainer), Made4Net.Shared.Util.FormatField(_tolocation), Made4Net.Shared.Util.FormatField(_toload),
                                Made4Net.Shared.Util.FormatField(_tocontainer), Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_userid),
                                Made4Net.Shared.Util.FormatField(_document), Made4Net.Shared.Util.FormatField(_documentline), Made4Net.Shared.Util.FormatField(_starttime), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_assignedtime), Made4Net.Shared.Util.FormatField(_executiontime), Made4Net.Shared.Util.FormatField(_startlocation), Made4Net.Shared.Util.FormatField(_assignmenttype), Made4Net.Shared.Util.FormatField(_executionlocation), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_countid), WhereClause,
                                Made4Net.Shared.Util.FormatField(_fromwarehousearea),
                                Made4Net.Shared.Util.FormatField(_towarehousearea),
                                Made4Net.Shared.Util.FormatField(_startwarehousearea),
                                Made4Net.Shared.Util.FormatField(_executionwarehousearea), Made4Net.Shared.Util.FormatField(_task_sub_type), Made4Net.Shared.Util.FormatField(_stdtime), Made4Net.Shared.Util.FormatField(_problemflag), Made4Net.Shared.Util.FormatField(_problemrc), Made4Net.Shared.Util.FormatField(_caseid))

            ' Made4Net.Shared.Util.FormatField(_executionwarehousearea), Made4Net.Shared.Util.FormatField(_task_sub_type), Made4Net.Shared.Util.FormatField(_stdtime), Made4Net.Shared.Util.FormatField(_problemflag), Made4Net.Shared.Util.FormatField(_problemrc), Made4Net.Shared.Util.FormatField(_task_sub_type))

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskUpdated)
            activitystatus = WMS.Lib.Actions.Audit.TASKUPD
        Else
            _task = Made4Net.Shared.Util.getNextCounter("TASK")
            'Start PWMS-398
            taskId = _task
            'End PWMS-398
            _adddate = DateTime.Now
            _editdate = DateTime.Now
            'RWMS-1277 added PREPOPULATELOCATION column
            SQL = String.Format("Insert into TASKS (TASK,TASKTYPE,STATUS,PICKLIST,PARALLELPICKLIST,CONSOLIDATION,REPLENISHMENT, COUNTID, CONSIGNEE,SKU,FROMLOCATION,FROMWAREHOUSEAREA,FROMLOAD," &
                        "FROMCONTAINER,TOLOCATION,TOWAREHOUSEAREA, TOLOAD,TOCONTAINER,PRIORITY,ASSIGNED,USERID,DOCUMENT,DOCUMENTLINE,STARTTIME,ADDDATE,ADDUSER,EDITDATE,EDITUSER,ASSIGNEDTIME,STARTLOCATION,STARTWAREHOUSEAREA, ASSIGNMENTTYPE,EXECUTIONLOCATION, EXECUTIONWAREHOUSEAREA,TASKSubTYPE,STDTIME,PROBLEMFLAG,PROBLEMRC,PREPOPULATELOCATION,CASEID) Values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39})",
                        Made4Net.Shared.Util.FormatField(_task), Made4Net.Shared.Util.FormatField(_task_type), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_picklist), Made4Net.Shared.Util.FormatField(_parallelpicklist), Made4Net.Shared.Util.FormatField(_consolidation), Made4Net.Shared.Util.FormatField(_replenishment), Made4Net.Shared.Util.FormatField(_countid), Made4Net.Shared.Util.FormatField(_consignee),
                        Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_fromwarehousearea),
                        Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromcontainer), Made4Net.Shared.Util.FormatField(_tolocation),
                        Made4Net.Shared.Util.FormatField(_towarehousearea), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_tocontainer),
                        Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_userid),
                        Made4Net.Shared.Util.FormatField(_document), Made4Net.Shared.Util.FormatField(_documentline), Made4Net.Shared.Util.FormatField(_starttime), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_assignedtime),
                        Made4Net.Shared.Util.FormatField(_startlocation), Made4Net.Shared.Util.FormatField(_startwarehousearea), Made4Net.Shared.Util.FormatField(_assignmenttype),
                        Made4Net.Shared.Util.FormatField(_executionlocation), Made4Net.Shared.Util.FormatField(_executionwarehousearea), Made4Net.Shared.Util.FormatField(_task_sub_type), Made4Net.Shared.Util.FormatField(_stdtime), Made4Net.Shared.Util.FormatField(_problemflag), Made4Net.Shared.Util.FormatField(_problemrc), Made4Net.Shared.Util.FormatField(_prepopulateLocation), Made4Net.Shared.Util.FormatField(_caseid))

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskCreated)
            activitystatus = WMS.Lib.Actions.Audit.TASKINS
        End If
        DataInterface.RunSQL(SQL)

        aq.Add("ACTIVITYTYPE", activitystatus)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _task)
        aq.Add("TASK", _task)
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
        'Added for RWMS-1971 start   
        aq.Add("TOQTY", 0)
        'Added for RWMS-1971 end
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _userid)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _userid)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _userid)
        aq.Send(activitystatus)

        'Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformance")
        Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformanceOnCreate")
        'A new task is created -> send a message to the labor service to get the standart time for the task
        If ShowLaborPerformance = "1" AndAlso activitystatus = WMS.Lib.Actions.Audit.TASKINS Then
            Dim oQ As Made4Net.Shared.ISyncQMsgSender = Made4Net.Shared.SyncQMsgSender.Factory.Create()
            Dim oMsg As System.Messaging.Message
            oQ.Add("TASK", _task)
            oQ.Add("USERID", _edituser)
            oQ.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskCreated)
            oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse)

            'oMsg = oQ.Send("LaborPerformanceProc")
            oMsg = oQ.Send(WMS.Lib.MSMQUEUES.LABORSYNC)
            Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
            If Not String.IsNullOrEmpty(qm.Values("ERROR")) Then
                Throw New ApplicationException(qm.Values("ERROR"))
            End If

        End If


    End Sub

    Public Overridable Sub DeAssignUser()
        _status = WMS.Lib.Statuses.Task.AVAILABLE
        _assigned = False
        _assignedtime = Nothing
        _editdate = DateTime.Now
        _userid = ""
        ' RWMS-1877 -> RWMS-1839
        ' Set starttime as Nothing on existing the task   
        _starttime = Nothing

        Dim sql As String = String.Format("UPDATE TASKS SET STATUS={0},ASSIGNED={1},ASSIGNEDTIME={2},USERID={3},editdate={4}, STARTTIME={5} WHERE {6}", Made4Net.Shared.Util.FormatField(_status), _
        Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_assignedtime), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_starttime), WhereClause)
        ' RWMS-1877 -> RWMS-1839 

        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskReleased)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKREL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _task)
        aq.Add("TASK", _task)
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
        'Added for RWMS-1971 start   
        aq.Add("TOQTY", 0)
        'Added for RWMS-1971 end
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _userid)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _userid)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _userid)
        aq.Send(WMS.Lib.Actions.Audit.TASKREL)
    End Sub

    Public Overridable Sub AssignUser(ByVal pUserId As String, Optional ByVal pAssignmentType As String = WMS.Lib.TASKASSIGNTYPE.MANUAL, Optional ByVal userMHType As String = "", Optional ByVal pPriority As Int32 = -1)

        If (String.IsNullOrEmpty(pUserId)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "User Id cannot be blank", "User Id cannot be blank")
            Throw m4nEx
        End If

        If (Not String.IsNullOrEmpty(_userid)) And (Not String.Equals(pUserId, _userid, StringComparison.CurrentCultureIgnoreCase)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "there is already user assigned to task [#param0#]", "there is already user assigned to task [#param0#]")
            m4nEx.Params.Add("task", _task)
            Throw m4nEx
        End If
        Dim fromStatus = _status
        _status = WMS.Lib.Statuses.Task.ASSIGNED
        _assigned = True
        _userid = pUserId
        If pPriority > -1 Then
            _priority = pPriority
        End If
        _editdate = DateTime.Now
        _edituser = pUserId
        _starttime = DateTime.Now
        _assignedtime = DateTime.Now
        _assignmenttype = pAssignmentType

        'Determine task start location
        Dim previousTaskEndLocation As String = WHActivity.GetPreviousActivityLocation(pUserId)
        Dim whactivityLocation As String = WHActivity.GetUserCurrentLocation(pUserId)

        Dim UpdatedStartlocation As String = String.Empty

        If Not String.IsNullOrEmpty(previousTaskEndLocation) Then
            UpdatedStartlocation = previousTaskEndLocation
        Else
            If Not String.IsNullOrEmpty(whactivityLocation) Then
                UpdatedStartlocation = whactivityLocation
            Else
                UpdatedStartlocation = FROMLOCATION
            End If
        End If
        STARTLOCATION = UpdatedStartlocation

        Dim sql As String = String.Format("update tasks set status={0},assigned={1},userid={2},assignedtime={3},assignmenttype={4}," &
                    "priority={5},editdate={6},edituser={7},startlocation={8} where {9}",
                    Made4Net.Shared.Util.FormatField(_status),
                    Made4Net.Shared.Util.FormatField(_assigned),
                    Made4Net.Shared.Util.FormatField(_userid),
                    Made4Net.Shared.Util.FormatField(_assignedtime),
                    Made4Net.Shared.Util.FormatField(_assignmenttype),
                    Made4Net.Shared.Util.FormatField(_priority),
                    Made4Net.Shared.Util.FormatField(_editdate),
                    Made4Net.Shared.Util.FormatField(_edituser),
                    Made4Net.Shared.Util.FormatField(STARTLOCATION),
                    WhereClause & " and ASSIGNED = 0 ")

        Dim result As Int32 = DataInterface.RunSQL(sql)
        If result > 0 Then
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskAssigned)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKASGN)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _task)
            aq.Add("TASK", _task)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", fromStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            'Added for RWMS-1971 start   
            aq.Add("TOQTY", 0)
            'Added for RWMS-1971 end
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", _userid)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", _userid)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", _userid)
            aq.Send(WMS.Lib.Actions.Audit.TASKASGN)


            'cad
            If (_task_type = WMS.Lib.PICKTYPE.FULLPICK Or _task_type = WMS.Lib.PICKTYPE.PARTIALPICK Or _task_type = WMS.Lib.PICKTYPE.PARALLELPICK Or _task_type = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK) Then

                Dim pq As EventManagerQ = New EventManagerQ
                pq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PickListAssign)
                pq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKASGN)
                pq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                pq.Add("ACTIVITYTIME", "0")
                pq.Add("CONSIGNEE", _consignee)
                pq.Add("DOCUMENT", _task)
                pq.Add("TASK", _task)
                pq.Add("DOCUMENTLINE", 0)
                pq.Add("FROMLOAD", "")
                pq.Add("FROMLOC", "")
                pq.Add("FROMWAREHOUSEAREA", "")
                pq.Add("FROMQTY", 0)
                pq.Add("FROMSTATUS", fromStatus)
                aq.Add("NOTES", "")
                pq.Add("SKU", "")
                pq.Add("TOLOAD", "")
                pq.Add("TOLOC", "")
                pq.Add("TOWAREHOUSEAREA", "")
                'Added for RWMS-1971 start   
                pq.Add("TOQTY", 0)
                'Added for RWMS-1971 end
                pq.Add("TOSTATUS", _status)
                pq.Add("USERID", _userid)
                pq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                pq.Add("ADDUSER", _userid)
                pq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                pq.Add("EDITUSER", _userid)
                pq.Send(WMS.Lib.Actions.Audit.PICKLISTASSIGN)


            End If


            'Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformance")
            Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformanceOnAssign")
            ' task is assigned -> send a message to the labor service to get the standart time for the task
            If ShowLaborPerformance = "1" Then
                Dim oQ As New Made4Net.Shared.SyncQMsgSender
                Dim oMsg As System.Messaging.Message
                oQ.Add("TASK", _task)
                oQ.Add("USERID", _edituser)
                oQ.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskAssigned)
                oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse)

                'oMsg = oQ.Send("LaborPerformanceProc")
                oMsg = oQ.Send(WMS.Lib.MSMQUEUES.LABORSYNC)
                Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
                If Not qm.Values("ERROR") Is Nothing Then
                    Throw New ApplicationException(qm.Values("ERROR"))
                End If

            End If

        End If
    End Sub

    'Added for RWMS-1867 Start
    Public Overridable Sub AssignUserManualReplenish(ByVal pUserId As String, Optional ByVal pAssignmentType As String = WMS.Lib.TASKASSIGNTYPE.MANUAL, Optional ByVal userMHType As String = "", Optional ByVal pPriority As Int32 = -1)

        If (String.IsNullOrEmpty(pUserId)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "User Id cannot be blank", "User Id cannot be blank")
            Throw m4nEx
        End If

        If (Not String.IsNullOrEmpty(_userid)) And (Not String.Equals(pUserId, _userid, StringComparison.CurrentCultureIgnoreCase)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "there is already user assigned to task [#param0#]", "there is already user assigned to task [#param0#]")
            m4nEx.Params.Add("task", _task)
            Throw m4nEx
        End If
        'Jira-332 Assigned to logged in user
        _status = WMS.Lib.Statuses.Task.ASSIGNED
        _assigned = True
        _userid = pUserId
        If pPriority > -1 Then
            _priority = pPriority
        End If
        _editdate = DateTime.Now
        _edituser = pUserId
        _starttime = DateTime.Now
        _assignedtime = DateTime.Now
        _assignmenttype = pAssignmentType

        Dim userstartlocation As String = DataInterface.ExecuteScalar(String.Format("select isnull(LOCATION,'') LOCATION from WHACTIVITY where USERID='{0}'", _userid))

        Dim sql As String = String.Format("update tasks set status={0},assigned={1},userid={2},assignedtime={3},assignmenttype={4}, priority={5},editdate={6},edituser={7} where {8}", Made4Net.Shared.Util.FormatField(_status),
                    Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_assignedtime), Made4Net.Shared.Util.FormatField(_assignmenttype), Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause & " and ASSIGNED = 0 and isnull(USERID,'') ='' ")

        Dim result As Int32 = DataInterface.RunSQL(sql)
        If result > 0 Then
            Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskAssigned)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKASGN)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _task)
            aq.Add("TASK", _task)
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
            aq.Add("USERID", _userid)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", _userid)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", _userid)
            aq.Send(WMS.Lib.Actions.Audit.TASKASGN)

            'Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformance")
            Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformanceOnAssign")
            ' task is assigned -> send a message to the labor service to get the standart time for the task
            If ShowLaborPerformance = "1" Then
                Dim oQ As Made4Net.Shared.ISyncQMsgSender = Made4Net.Shared.SyncQMsgSender.Factory.Create()
                Dim oMsg As System.Messaging.Message
                oQ.Add("TASK", _task)
                oQ.Add("USERID", _edituser)
                oQ.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskAssigned)
                oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse)

                'oMsg = oQ.Send("LaborPerformanceProc")
                oMsg = oQ.Send(WMS.Lib.MSMQUEUES.LABORSYNC)
                Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
                If Not qm.Values("ERROR") Is Nothing Then
                    Throw New ApplicationException(qm.Values("ERROR"))
                End If

            End If

        End If
    End Sub
    'Added for RWMS-1867 End

    Public Overridable Sub Complete(ByVal logger As LogHandler, Optional ByVal pProblemRC As String = "")

        If _status = Statuses.Task.COMPLETE Then
            Return
        End If

        Dim sql As String
        Dim fromStatus = _status
        _status = WMS.Lib.Statuses.Task.COMPLETE
        _assigned = False
        _edituser = _userid
        _editdate = DateTime.Now
        _endtime = DateTime.Now
        Try
            '_executiontime = DateDiff(DateInterval.Second, _assignedtime, _endtime)
            _executiontime = DateDiff(DateInterval.Second, _starttime, _endtime)
        Catch ex As Exception
            _executiontime = -1
        End Try
        If pProblemRC <> String.Empty Then
            _problemflag = True
            _problemrc = pProblemRC
        End If
        sql = String.Format("update tasks set status={0},assigned={1},userid={2},editdate={3},edituser={4},endtime={5},executiontime={6},executionlocation={7},executionwarehousearea={8},PROBLEMFLAG={9},PROBLEMRC={10} where {11}", Made4Net.Shared.Util.FormatField(_status),
                    Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_endtime), Made4Net.Shared.Util.FormatField(_executiontime), Made4Net.Shared.Util.FormatField(_executionlocation), Made4Net.Shared.Util.FormatField(_executionwarehousearea), Made4Net.Shared.Util.FormatField(_problemflag), Made4Net.Shared.Util.FormatField(_problemrc), WhereClause)
        DataInterface.RunSQL(sql)

        Dim oWHActivity As New WHActivity
        oWHActivity.ACTIVITY = _task_type
        oWHActivity.LOCATION = _executionlocation
        oWHActivity.WAREHOUSEAREA = _executionwarehousearea
        oWHActivity.USERID = _edituser
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.ADDDATE = DateTime.Now
        oWHActivity.EDITDATE = DateTime.Now
        oWHActivity.ADDUSER = _edituser
        oWHActivity.EDITUSER = _edituser
        'Adding for RWMS-1497
        oWHActivity.PREVIOUSACTIVITYTIME = _endtime
        oWHActivity.Post()

        Dim aq As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskCompleted)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKCOMP)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _task)
        aq.Add("TASK", _task)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", fromStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        'Added for RWMS-1971 start   
        aq.Add("TOQTY", 0)
        'Added for RWMS-1971 end
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _userid)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(WMS.Lib.Actions.Audit.TASKCOMP)


        'Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformance")
        Dim ShowLaborPerformance As String = Made4Net.Shared.Util.GetSystemParameterValue("ShowLaborPerformanceOnComplete")
        'A  task is completeted -> send a message to the labor service to recalc the time for the task
        If ShowLaborPerformance = "1" Then
            Dim oQ As Made4Net.Shared.ISyncQMsgSender = Made4Net.Shared.SyncQMsgSender.Factory.Create()
            Dim oMsg As System.Messaging.Message
            oQ.Add("TASK", _task)
            oQ.Add("USERID", _edituser)
            oQ.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskCompleted)
            oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse)

            'oMsg = oQ.Send("LaborPerformanceProc")
            oMsg = oQ.Send(WMS.Lib.MSMQUEUES.LABORSYNC)
            Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
            '-- oded changes--

            If Not String.IsNullOrEmpty(qm.Values("ERROR")) Then
                Throw New ApplicationException(qm.Values("ERROR"))
            End If
            'If Not qm.Values("ERROR") Is Nothing Then
            '    Throw New ApplicationException(qm.Values("ERROR"))
            'End If
        Else
            'Else save the last location here
            ' PWMS-909
            oWHActivity.SaveLastLocation()
        End If



    End Sub

    Public Function UpdateStdTime(ByVal stdTaskTime As Double) As Boolean
        Try
            Dim sql As String = String.Format("update TASKS set STDTIME='{0}' where TASK='{1}'", stdTaskTime.ToString(), _task)
            DataInterface.RunSQL(sql)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetTaskPerformance() As Decimal
        Dim sql As String = String.Format("Select TaskPerformance from vTaskPerformanceByTaskID where taskid={0}", Made4Net.Shared.FormatField(_task))
        Try
            Return CType(DataInterface.ExecuteScalar(sql), Decimal)
        Catch ex As Exception
            Return 0
        End Try
    End Function


    Public Overridable Sub Cancel()
        Dim sql As String
        _status = WMS.Lib.Statuses.Task.CANCELED
        _assigned = False
        _edituser = WMS.Logic.GetCurrentUser
        _userid = Nothing
        _editdate = DateTime.Now
        _endtime = Nothing
        _starttime = Nothing
        sql = String.Format("update tasks set status={0},assigned={1},userid={2},editdate={3},edituser={4},endtime={5},starttime={6} where {7}", Made4Net.Shared.Util.FormatField(_status), _
                    Made4Net.Shared.Util.FormatField(_assigned), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_endtime), Made4Net.Shared.Util.FormatField(_starttime), WhereClause)
        DataInterface.RunSQL(sql)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.TaskCancelled)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.TASKCNL)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _task)
        aq.Add("TASK", _task)
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
        'Added for RWMS-1971 start   
        aq.Add("TOQTY", 0)
        'Added for RWMS-1971 end
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Send(WMS.Lib.Actions.Audit.TASKCNL)
    End Sub

    Public Overridable Sub SetStartTime()
        Dim sql As String
        _edituser = WMS.Logic.GetCurrentUser
        _editdate = DateTime.Now
        'RWMS-2919
        If _starttime = DateTime.MinValue Then _starttime = DateTime.Now
        'RWMS-2828
        If String.IsNullOrEmpty(_startlocation) Then
            updateStartLocation()
        End If
        'RWMS-2828 End
        sql = String.Format("update tasks set starttime={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_starttime),
                    Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClauseForSettingStartTime)
        DataInterface.RunSQL(sql)
    End Sub
    'RWMS-2828
    Private Function updateStartLocation()
        Dim sql As String
        Dim currentlocation As String
        Try
            currentlocation = WHActivity.GetUserCurrentLocation(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
        Catch
            currentlocation = String.Empty
        End Try

        If String.IsNullOrEmpty(currentlocation) Then
            _startlocation = _fromlocation
        Else
            _startlocation = currentlocation
        End If
        sql = String.Format("Update tasks set startlocation='{0}' where task='{1}'", _startlocation, TASK)
        DataInterface.RunSQL(sql)
    End Function
    'RWMS-2828 End

    'Added for RWMS-1497
    Public Overridable Sub SetStartTime(userPreviousActivityTime As DateTime)
        Dim sql As String
        sql = String.Format("update tasks set starttime={0}, editdate={0}, edituser='{2}' where {1}", Made4Net.Shared.Util.FormatField(userPreviousActivityTime), WhereClauseForSettingStartTime, WMS.Lib.USERS.SYSTEMUSER)
        DataInterface.RunSQL(sql)
        'RWMS-2828
        updateStartLocation()
        'RWMS-2828 End
    End Sub
    'End RWMS-1497

    Public Sub Create()
        If _status = "" Or _status Is Nothing Then
            _status = "Available"
        End If
        Save()
    End Sub

    Public Sub Update()
        If Not Exists(_task) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Task does not exist.", "Task does not exist.")
        End If

        Save()
    End Sub

    'Gets the count of tasks having provided picklist id
    Public Function GetTaskCountForPicklist(ByVal picklistId As String) As Integer
        If Not String.IsNullOrEmpty(picklistId) Then
            Dim sql As String = String.Format("SELECT COUNT(*) FROM TASKS WHERE PICKLIST={0}", Made4Net.Shared.Util.FormatField(picklistId))
            Return DataInterface.ExecuteScalar(sql)
        End If
        Return 0
    End Function

    Public Function GetAvailableTaskByReplenishmentType(ByVal taskType As String) As Task
        Dim SQL As String = String.Format("SELECT * FROM TASKS WHERE STATUS='AVAILABLE' AND TASKTYPE='{0}'", taskType)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Task does not exists", "Task does not exists")
        End If
        Load(dt.Rows(0))
        Return Me
    End Function

    Private Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("TASK") Then _task = dr.Item("TASK")
        If Not dr.IsNull("TASKTYPE") Then _task_type = dr.Item("TASKTYPE")
        If Not dr.IsNull("TASKSUBTYPE") Then _task_sub_type = dr.Item("TASKSUBTYPE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("PICKLIST") Then _picklist = dr.Item("PICKLIST")
        If Not dr.IsNull("PARALLELPICKLIST") Then _parallelpicklist = dr.Item("PARALLELPICKLIST")
        If Not dr.IsNull("REPLENISHMENT") Then _replenishment = dr.Item("REPLENISHMENT")
        If Not dr.IsNull("COUNTID") Then _countid = dr.Item("COUNTID")
        If Not dr.IsNull("CONSOLIDATION") Then _consolidation = dr.Item("CONSOLIDATION")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("FROMLOCATION") Then _fromlocation = dr.Item("FROMLOCATION")
        If Not dr.IsNull("FROMWAREHOUSEAREA") Then _fromwarehousearea = dr.Item("FROMWAREHOUSEAREA")
        If Not dr.IsNull("CASEID") Then _caseid = dr.Item("CASEID")
        If Not dr.IsNull("FROMLOAD") Then _fromload = dr.Item("FROMLOAD")
        If Not dr.IsNull("FROMCONTAINER") Then _fromcontainer = dr.Item("FROMCONTAINER")
        If Not dr.IsNull("TOLOCATION") Then _tolocation = dr.Item("TOLOCATION")
        If Not dr.IsNull("TOWAREHOUSEAREA") Then _towarehousearea = dr.Item("TOWAREHOUSEAREA")
        If Not dr.IsNull("TOLOAD") Then _toload = dr.Item("TOLOAD")
        If Not dr.IsNull("TOCONTAINER") Then _tocontainer = dr.Item("TOCONTAINER")
        If Not dr.IsNull("PRIORITY") Then _priority = dr.Item("PRIORITY")
        If Not dr.IsNull("ASSIGNED") Then _assigned = dr.Item("ASSIGNED")
        If Not dr.IsNull("USERID") Then _userid = dr.Item("USERID")
        If Not dr.IsNull("DOCUMENT") Then _document = dr.Item("DOCUMENT")
        If Not dr.IsNull("DOCUMENTLINE") Then _documentline = dr.Item("DOCUMENTLINE")
        If Not dr.IsNull("STARTTIME") Then _starttime = dr.Item("STARTTIME")
        If Not dr.IsNull("ENDTIME") Then _endtime = dr.Item("ENDTIME")
        If Not dr.IsNull("ASSIGNEDTIME") Then _assignedtime = dr.Item("ASSIGNEDTIME")
        If Not dr.IsNull("EXECUTIONTIME") Then _executiontime = dr.Item("EXECUTIONTIME")
        If Not dr.IsNull("STARTLOCATION") Then _startlocation = dr.Item("STARTLOCATION")
        If Not dr.IsNull("STARTWAREHOUSEAREA") Then _startwarehousearea = dr.Item("STARTWAREHOUSEAREA")
        If Not dr.IsNull("ASSIGNMENTTYPE") Then _assignmenttype = dr.Item("ASSIGNMENTTYPE")
        If Not dr.IsNull("EXECUTIONLOCATION") Then _executionlocation = dr.Item("EXECUTIONLOCATION")
        If Not dr.IsNull("EXECUTIONWAREHOUSEAREA") Then _executionwarehousearea = dr.Item("EXECUTIONWAREHOUSEAREA")
        If Not dr.IsNull("STDTIME") Then _stdtime = dr.Item("STDTIME")
        If Not dr.IsNull("PROBLEMFLAG") Then _problemflag = dr.Item("PROBLEMFLAG")
        If Not dr.IsNull("PROBLEMRC") Then _problemrc = dr.Item("PROBLEMRC")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

#Region "Problem Codes"

    Public Overridable Sub ReportProblem(ByVal pProblemCodeId As String, ByVal pLocation As String, ByVal pWHArea As String, ByVal pUserId As String)
        Dim dr As DataRow = GetProblemCodesSetupRow(pProblemCodeId, _task_type)
        Dim lc As WMS.Logic.Location
        Dim taskProblem As String
        If Convert.ToString(dr("LOCATIONPROBLEMRC")) <> String.Empty Then
            taskProblem = dr("LOCATIONPROBLEMRC")
        Else
            taskProblem = dr("PROBLEMCODEDESC")
        End If
        If Convert.ToBoolean(dr("locationproblem")) Then
            lc = New WMS.Logic.Location(pLocation, pWHArea)
            lc.SetProblemFlag(True, taskProblem, pUserId)
        End If
        If Convert.ToBoolean(dr("completetask")) Then
            _problemcodecompletetask = True
            Complete(Nothing, taskProblem)
        Else
            _problemcodecompletetask = False
            DeAssignUser()
        End If
        If Convert.ToString(dr("counttype")) <> String.Empty Then
            Dim oCnt As New WMS.Logic.Counting()
            oCnt.CreateLocationCountJobs(pWHArea, "", pLocation, Convert.ToString(dr("counttype")), "", "", pUserId)
        End If
    End Sub

    Private Function GetProblemCodesSetupRow(ByVal pProblemCodeId As String, ByVal pTaskType As String) As DataRow
        Dim dt As New DataTable
        Dim sql As String = String.Format("select * from vTaskTypesProblemCodes WHERE PROBLEMCODEID = '{0}' and TASKTYPE = '{1}'", pProblemCodeId, pTaskType)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Operation not allowed or configuration is missing for current task type", "Operation not allowed or configuration is missing for current task type")
        Else
            Return dt.Rows(0)
        End If
    End Function

#End Region

End Class
'Start RWMS-1754
<CLSCompliant(False)> Public Class TaskProblemCodes
#Region "Variables"
    Protected _problemCodeID As String = String.Empty
    Protected _problemCodeDesc As String = String.Empty
    Protected _locationProblem As Boolean
    Protected _completeTask As Boolean
    Protected _countType As String
    Protected _addDate As DateTime
    Protected _addUser As String
    Protected _editDate As DateTime
    Protected _editUser As String
    Protected _locationProblemRC As String
#End Region
#Region "Properties"
    Public ReadOnly Property WhereClause() As String
        Get
            Return " PROBLEMCODEID = '" & _problemCodeID & "'"
        End Get
    End Property
    Public Property PROBLEMCODEID() As String
        Get
            Return _problemCodeID
        End Get
        Set(ByVal Value As String)
            _problemCodeID = Value
        End Set
    End Property
    Public Property PROBLEMCODEDESC() As String
        Get
            Return _problemCodeDesc
        End Get
        Set(ByVal Value As String)
            _problemCodeDesc = Value
        End Set
    End Property
    Public Property LOCATIONPROBLEM() As Boolean
        Get
            Return _locationProblem
        End Get
        Set(ByVal Value As Boolean)
            _locationProblem = Value
        End Set
    End Property
    Public Property COMPLETETASK() As Boolean
        Get
            Return _completeTask
        End Get
        Set(ByVal Value As Boolean)
            _completeTask = Value
        End Set
    End Property
    Public Property COUNTTYPE() As String
        Get
            Return _countType
        End Get
        Set(ByVal Value As String)
            _countType = Value
        End Set
    End Property
    Public Property ADDUSER() As String
        Get
            Return _addUser
        End Get
        Set(ByVal Value As String)
            _addUser = Value
        End Set
    End Property
    Public Property ADDDATE() As DateTime
        Get
            Return _addDate
        End Get
        Set(ByVal Value As DateTime)
            _addDate = Value
        End Set
    End Property
    Public Property EDITDATE() As DateTime
        Get
            Return _editDate
        End Get
        Set(ByVal Value As DateTime)
            _editDate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _editUser
        End Get
        Set(ByVal Value As String)
            _editUser = Value
        End Set
    End Property
    Public Property LOCATIONPROBLEMRC() As String
        Get
            Return _locationProblemRC
        End Get
        Set(ByVal Value As String)
            _locationProblemRC = Value
        End Set
    End Property
#End Region
    Public Sub New()
    End Sub
    Public Sub New(ByVal pProblemCodeID As String)
        PROBLEMCODEID = pProblemCodeID
        Load()
    End Sub
#Region "Create / Update"
    Public Sub CreateTaskProblemCode(ByVal pProblemCodeID As String, ByVal pProblemCodeDesc As String, ByVal pLocationProblem As Boolean, ByVal pCompleteTask As Boolean, ByVal pCountType As String, ByVal pLocationProblemRC As String)
        If CheckTaskProblemCodeExist(pProblemCodeID) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Problem Code  - Already Exists", "Cannot Create Problem Code - Already Exists")
        End If

        PROBLEMCODEID = pProblemCodeID
        PROBLEMCODEDESC = pProblemCodeDesc
        LOCATIONPROBLEM = pLocationProblem
        COMPLETETASK = pCompleteTask
        COUNTTYPE = pCountType
        ADDUSER = WMS.Logic.Common.GetCurrentUser
        ADDDATE = Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now)
        LOCATIONPROBLEMRC = pLocationProblemRC

        Dim sql As String = String.Format("INSERT INTO TASKPROBLEMCODE(PROBLEMCODEID,PROBLEMCODEDESC,LOCATIONPROBLEM,COMPLETETASK,COUNTTYPE,ADDUSER,ADDDATE,LOCATIONPROBLEMRC) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})", _
                   Made4Net.Shared.Util.FormatField(PROBLEMCODEID), Made4Net.Shared.Util.FormatField(PROBLEMCODEDESC), Made4Net.Shared.Util.FormatField(LOCATIONPROBLEM), Made4Net.Shared.Util.FormatField(COMPLETETASK), Made4Net.Shared.Util.FormatField(COUNTTYPE), Made4Net.Shared.Util.FormatField(ADDUSER), Made4Net.Shared.Util.FormatField(ADDDATE), Made4Net.Shared.Util.FormatField(LOCATIONPROBLEMRC))
        DataInterface.RunSQL(sql)

    End Sub
    Public Sub UpdateTaskProblemCode(ByVal pProblemCodeID As String, ByVal pProblemCodeDesc As String, ByVal pLocationProblem As Boolean, ByVal pCompleteTask As Boolean, ByVal pCountType As String, ByVal pLocationProblemRC As String)

        PROBLEMCODEID = pProblemCodeID
        PROBLEMCODEDESC = pProblemCodeDesc
        LOCATIONPROBLEM = pLocationProblem
        COMPLETETASK = pCompleteTask
        COUNTTYPE = pCountType
        EDITUSER = WMS.Logic.Common.GetCurrentUser
        EDITDATE = Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now)
        LOCATIONPROBLEMRC = pLocationProblemRC

        Dim sql As String = String.Format("UPDATE TASKPROBLEMCODE SET PROBLEMCODEDESC={0},LOCATIONPROBLEM={1},COMPLETETASK={2},COUNTTYPE={3},EDITUSER={4},EDITDATE={5},LOCATIONPROBLEMRC={6} WHERE {7}", _
                   Made4Net.Shared.Util.FormatField(PROBLEMCODEDESC), Made4Net.Shared.Util.FormatField(LOCATIONPROBLEM), Made4Net.Shared.Util.FormatField(COMPLETETASK), Made4Net.Shared.Util.FormatField(COUNTTYPE), Made4Net.Shared.Util.FormatField(EDITUSER), Made4Net.Shared.Util.FormatField(EDITDATE), Made4Net.Shared.Util.FormatField(LOCATIONPROBLEMRC), WhereClause)
        DataInterface.RunSQL(sql)

    End Sub
#End Region

    Private Function CheckTaskProblemCodeExist(pProblemCodeID As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(*) FROM TASKPROBLEMCODE WHERE PROBLEMCODEID={0}", Made4Net.Shared.Util.FormatField(pProblemCodeID))
        Return DataInterface.ExecuteScalar(sql)
    End Function
    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM TASKPROBLEMCODE WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Problem Code does not exists", "Problem Code does not exists")
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("PROBLEMCODEID") Then PROBLEMCODEID = dr.Item("PROBLEMCODEID")
        If Not dr.IsNull("PROBLEMCODEDESC") Then PROBLEMCODEDESC = dr.Item("PROBLEMCODEDESC")
        If Not dr.IsNull("LOCATIONPROBLEM") Then LOCATIONPROBLEM = dr.Item("LOCATIONPROBLEM")
        If Not dr.IsNull("COMPLETETASK") Then COMPLETETASK = dr.Item("COMPLETETASK")
        If Not dr.IsNull("COUNTTYPE") Then COUNTTYPE = dr.Item("COUNTTYPE")
        If Not dr.IsNull("ADDUSER") Then ADDUSER = dr.Item("ADDUSER")
        If Not dr.IsNull("ADDDATE") Then ADDDATE = dr.Item("ADDDATE")
        If Not dr.IsNull("EDITUSER") Then EDITUSER = dr.Item("EDITUSER")
        If Not dr.IsNull("EDITDATE") Then EDITDATE = dr.Item("EDITDATE")
        If Not dr.IsNull("LOCATIONPROBLEMRC") Then LOCATIONPROBLEMRC = dr.Item("LOCATIONPROBLEMRC")

    End Sub
End Class
'End RWMS-1754
Public Enum REPLTASKTYPE
    ZONEREPLANISHMENT = 100
    NORMALREPLANISHMENT = 200
    HOTREPLANISHMENT = 800
End Enum
