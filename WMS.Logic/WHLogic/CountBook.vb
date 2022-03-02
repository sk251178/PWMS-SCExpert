Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared

#Region "CountBook"

<CLSCompliant(False)> Public Class CountBook

#Region "Variables"

    Protected _countbook As String = String.Empty
    Protected _countbookrunid As Int32
    Protected _status As String = String.Empty
    Protected _createdate As DateTime
    Protected _closedate As DateTime
    Protected _note As String = String.Empty
    Protected _counttype As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" COUNTBOOK = '{0}'", _countbook)
        End Get
    End Property

    Public Property COUNTBOOKID() As String
        Get
            Return _countbook
        End Get
        Set(ByVal Value As String)
            _countbook = Value
        End Set
    End Property

    Public Property COUNTBOOKRUNID() As Int32
        Get
            Return _countbookrunid
        End Get
        Set(ByVal Value As Int32)
            _countbookrunid = Value
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

    Public Property CREATEDATE() As DateTime
        Get
            Return _createdate
        End Get
        Set(ByVal Value As DateTime)
            _createdate = Value
        End Set
    End Property

    Public Property CLOSEDATE() As DateTime
        Get
            Return _closedate
        End Get
        Set(ByVal Value As DateTime)
            _closedate = Value
        End Set
    End Property

    Public Property NOTE() As String
        Get
            Return _note
        End Get
        Set(ByVal Value As String)
            _note = Value
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

    Public Sub New(ByVal pCountBook As String, Optional ByVal LoadObj As Boolean = True)
        _countbook = pCountBook
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetCountBook(ByVal pCountBook As String) As CountBook
        Return New CountBook(pCountBook)
    End Function

    Public Shared Function Exists(ByVal pCountBook As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM COUNTBOOK WHERE COUNTBOOK = '{0}'", pCountBook)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM COUNTBOOK WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "CountBook does not exists", "CountBook does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("COUNTBOOKRUNID") Then _countbookrunid = dr.Item("COUNTBOOKRUNID")
        If Not dr.IsNull("COUNTTYPE") Then _counttype = dr.Item("COUNTTYPE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("CLOSEDATE") Then _closedate = dr.Item("CLOSEDATE")
        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("NOTE") Then _note = dr.Item("NOTE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Private Function GetNextRunId() As Int32
        Dim sql As String = String.Format("SELECT  ISNULL(MAX(COUNTBOOKRUNID),0)  + 1 FROM COUNTBOOK WHERE COUNTBOOK = '{0}'", _countbook)
        Return System.Convert.ToInt32(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "Create & Update"

    Public Sub CreateNew(ByVal pCountBook As String, ByVal pNotes As String, ByVal pCountType As String, ByVal pUser As String)
        If Exists(pCountBook) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "CountBook already exists", "CountBook already exists")
            Throw m4nEx
        End If
        If pCountBook = String.Empty Then
            _countbook = Made4Net.Shared.Util.getNextCounter("COUNTBOOK")
        Else
            _countbook = pCountBook
        End If
        '_countbookrunid = GetNextRunId()
        _countbookrunid = 1
        _status = WMS.Lib.Statuses.CountBook.[NEW]
        _note = pNotes
        _counttype = pCountType
        _createdate = DateTime.Now
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("INSERT INTO COUNTBOOK (COUNTBOOK, COUNTBOOKRUNID, STATUS, CREATEDATE, CLOSEDATE, NOTE, COUNTTYPE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) values (" & _
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})", Made4Net.Shared.Util.FormatField(_countbook), Made4Net.Shared.Util.FormatField(_countbookrunid), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_closedate), _
            Made4Net.Shared.Util.FormatField(_note), Made4Net.Shared.Util.FormatField(_counttype), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Update(ByVal pNotes As String, ByVal pCountType As String, ByVal iCountbookRunId As Int32, ByVal pUser As String)
        _note = pNotes
        _counttype = pCountType
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE COUNTBOOK SET NOTE ={0}, COUNTTYPE ={1}, COUNTBOOKRUNID={5}, EDITDATE ={2}, EDITUSER ={3} WHERE {4}", Made4Net.Shared.Util.FormatField(_note), Made4Net.Shared.Util.FormatField(_counttype), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(iCountbookRunId))

        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Complete RunID Tasks"

    Private Sub CompleteRunIdTasks(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.CountBook.COMPLETE Or _status = WMS.Lib.Statuses.CountBook.CANCELLED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CountBook status incorrect", "CountBook status incorrect")
        End If
        _editdate = DateTime.Now
        _edituser = pUser
        ' Get all available tasks
        Dim AvailableTasks As DataTable = New DataTable
        Dim SqlAvailableTasks As String = "SELECT TS.TASK, TS.COUNTID FROM TASKS TS INNER JOIN COUNTING CT ON TS.COUNTID=CT.COUNTID AND CT.COUNTBOOK='" & _countbook & "' AND CT.COUNTBOOKRUNID='" & _countbookrunid & "' AND TS.STATUS='AVAILABLE'"
        DataInterface.FillDataset(SqlAvailableTasks, AvailableTasks)
        For Each AvailableTask As DataRow In AvailableTasks.Rows
            ' Complete all available tasks
            Dim CntTask As CountTask = New CountTask(AvailableTask("TASK"))
            CntTask.Cancel()
            ' Complete counting object
            Dim CountingObj As Counting = New Counting(AvailableTask("COUNTID"))
            CountingObj.Cancel(AvailableTask("COUNTID"), pUser)
        Next
    End Sub

#End Region

#Region "Complete & Cancel"

    Private Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.CountBook.COMPLETE Or _status = WMS.Lib.Statuses.CountBook.CANCELLED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CountBook status incorrect", "CountBook status incorrect")
        End If
        _status = pStatus
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE COUNTBOOK SET status ={0}, EDITDATE ={1}, EDITUSER ={2} where {3}", Made4Net.Shared.Util.FormatField(_status), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Complete(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.CountBook.COMPLETE Or _status = WMS.Lib.Statuses.CountBook.CANCELLED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CountBook status incorrect", "CountBook status incorrect")
        End If
        ' Complete all unfinished tasks
        CompleteRunIdTasks(pUser)
        SetStatus(WMS.Lib.Statuses.CountBook.COMPLETE, pUser)
        _closedate = DateTime.Now
        Dim sql As String = String.Format("update countbook set closedate={0} where {1}", _
        Made4Net.Shared.FormatField(_closedate), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Create Count Tasks & Count"

    Public Function Count(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.CountBook.[NEW] Then
            SetStatus(WMS.Lib.Statuses.CountBook.COUNTING, pUser)
        End If
    End Function

    Public Sub CreateCountTasks(ByVal pUser As String)
        Dim oCnt As New WMS.Logic.Counting
        ' Check if there are counting tasks for current run id
        If DataInterface.ExecuteScalar("SELECT COUNT(1) FROM COUNTING WHERE COUNTBOOK='" & _countbook & "' AND COUNTBOOKRUNID='" & _countbookrunid & "'") > 0 Then
            ' if yes then close current running tasks
            CompleteRunIdTasks(pUser)
            ' update runid
            _countbookrunid = GetNextRunId()
            Update(_note, _counttype, _countbookrunid, pUser)
        End If
        ' create new tasks        
        oCnt.CreateLocationCountJobs("", "", "", _counttype, _countbook, _countbookrunid, pUser)

    End Sub

    Public Sub CreateCountTaskDescripanices(ByVal pUser As String)
        ' Save old run id
        Dim LastCountRunId As String = _countbookrunid
        ' Check if there are counting tasks for current run id
        If DataInterface.ExecuteScalar("SELECT COUNT(1) FROM COUNTING WHERE COUNTBOOK='" & _countbook & "' AND COUNTBOOKRUNID='" & _countbookrunid & "'") > 0 Then
            ' if yes then close current running tasks
            CompleteRunIdTasks(pUser)
            ' update runid
            _countbookrunid = GetNextRunId()
            Update(_note, _counttype, _countbookrunid, pUser)
        Else
            ' No task , we need to create task's for all location's
            CreateCountTasks(pUser)
        End If
        ' Select all task's from count audit where we have descripancies with old run id
        Dim SqlDescripancyTasks As String = "SELECT LOCATION,WAREHOUSEAREA FROM COUNTBOOKAUDIT WHERE EXPECTEDQTY<>COUNTQTY AND COUNTBOOK='" & _countbook & "' AND COUNTBOOKRUNID='" & LastCountRunId & "'"
        Dim DescripancyTasks As DataTable = New DataTable
        DataInterface.FillDataset(SqlDescripancyTasks, DescripancyTasks)
        Dim oCnt As New WMS.Logic.Counting
        For Each DescrTask As DataRow In DescripancyTasks.Rows
            oCnt.CreateLocationCountJobs("", DescrTask("LOCATION"), DescrTask("WAREHOUSEAREA"), _counttype, _countbook, _countbookrunid, pUser)
        Next
    End Sub

#End Region

#End Region

End Class

#End Region

#Region "Count Book Audit"

Public Class CountBookAudit

#Region "Variables"

    Protected _countbook As String = String.Empty
    Protected _countbookrunid As String = String.Empty
    Protected _countid As String = String.Empty
    Protected _loadid As String = String.Empty
    Protected _location As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _expectedqty As Decimal
    Protected _countqty As Decimal
    Protected _countdate As DateTime
    Protected _userid As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" COUNTBOOK = '{0}' and COUNTBOOKRUNID='{1}' and COUNTID='{2}'", _countbook, _countbookrunid, _countid)
        End Get
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

    Public Property COUNTID() As String
        Get
            Return _countid
        End Get
        Set(ByVal Value As String)
            _countid = Value
        End Set
    End Property

    Public Property USERID() As String
        Get
            Return _userid
        End Get
        Set(ByVal Value As String)
            _userid = Value
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

    Public Property EXPECTEDQTY() As Decimal
        Get
            Return _expectedqty
        End Get
        Set(ByVal Value As Decimal)
            _expectedqty = Value
        End Set
    End Property

    Public Property COUNTQTY() As Decimal
        Get
            Return _countqty
        End Get
        Set(ByVal Value As Decimal)
            _countqty = Value
        End Set
    End Property

    Public Property COUNTDATE() As DateTime
        Get
            Return _countdate
        End Get
        Set(ByVal Value As DateTime)
            _countdate = Value
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

    Public Sub New(ByVal pCountBook As String, ByVal pCountBookRunId As String, ByVal pCountid As String, Optional ByVal LoadObj As Boolean = True)
        _countbook = pCountBook
        _countbookrunid = pCountBookRunId
        _countid = pCountid
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetCountBook(ByVal pCountBook As String, ByVal pCountBookRunId As String, ByVal pCountid As String) As CountBookAudit
        Return New CountBookAudit(pCountBook, pCountBookRunId, pCountid)
    End Function

    Public Shared Function Exists(ByVal pCountBook As String, ByVal pCountBookRunId As String, ByVal pCountid As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from countbookaudit where countbook = '{0}' and countbookrunid ='{1}' and countid='{2}'", pCountBook, pCountBookRunId, pCountid)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM countbookaudit WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "CountBookAudit line does not exists", "CountBookAudit line does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("EXPECTEDQTY") Then _expectedqty = dr.Item("EXPECTEDQTY")
        If Not dr.IsNull("COUNTQTY") Then _countqty = dr.Item("COUNTQTY")
        If Not dr.IsNull("USERID") Then _userid = dr.Item("USERID")
        If Not dr.IsNull("COUNTDATE") Then _countdate = dr.Item("COUNTDATE")
        If Not dr.IsNull("loadid") Then _loadid = dr.Item("loadid")
        If Not dr.IsNull("location") Then _location = dr.Item("location")
        If Not dr.IsNull("warehousearea") Then _location = dr.Item("warehousearea")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#End Region

#Region "Post"

    Public Sub Post(ByVal pCountBook As String, ByVal pCountBookRunId As String, ByVal pCountid As String, ByVal pLoadid As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pExpectedQty As Decimal, ByVal pCountQty As Decimal, ByVal pCountUser As String, ByVal pUser As String)
        _countbook = pCountBook
        _countbookrunid = pCountBookRunId
        _countid = pCountid
        _location = pLocation
        _warehousearea = pWarehousearea
        _loadid = pLoadid
        _expectedqty = pExpectedQty
        _countqty = pCountQty
        _userid = pCountUser
        _countdate = DateTime.Now
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format(" INSERT INTO COUNTBOOKAUDIT (COUNTBOOK, COUNTID, COUNTBOOKRUNID, " & _
                                                " LOADID, LOCATION, WAREHOUSEAREA, EXPECTEDQTY, COUNTQTY, USERID, " & _
                                                " COUNTDATE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                                          " values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}) ", _
                                        Made4Net.Shared.Util.FormatField(_countbook), Made4Net.Shared.Util.FormatField(_countid), _
                                        Made4Net.Shared.Util.FormatField(_countbookrunid), Made4Net.Shared.Util.FormatField(_loadid), _
                                        Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_warehousearea), _
                                        Made4Net.Shared.Util.FormatField(_expectedqty), Made4Net.Shared.Util.FormatField(_countqty), _
                                        Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_countdate), _
                                        Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), _
                                        Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)
    End Sub

#End Region

#End Region

End Class

#End Region

