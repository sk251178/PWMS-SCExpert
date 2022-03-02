Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteGeneralTask

#Region "Variables"

    Protected _taskid As String
    Protected _tasktype As String
    Protected _status As String
    Protected _statusdate As DateTime
    Protected _scheduledate As DateTime
    Protected _consignee As String
    Protected _company As String
    Protected _companytype As String
    Protected _contactid As String
    Protected _contact As Contact
    Protected _notes As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"

    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" TASKID = {0}", Made4Net.Shared.Util.FormatField(_taskid))
        End Get
    End Property

    Public Property TaskId() As String
        Get
            Return _taskid
        End Get
        Set(ByVal Value As String)
            _taskid = Value
        End Set
    End Property

    Public Property TaskType() As String
        Get
            Return _tasktype
        End Get
        Set(ByVal Value As String)
            _tasktype = Value
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

    Public Property Company() As String
        Get
            Return _company
        End Get
        Set(ByVal Value As String)
            _company = Value
        End Set
    End Property

    Public Property CompanyType() As String
        Get
            Return _companytype
        End Get
        Set(ByVal Value As String)
            _companytype = Value
        End Set
    End Property

    Public Property ContactId() As String
        Get
            Return _contactid
        End Get
        Set(ByVal Value As String)
            _contactid = Value
        End Set
    End Property

    Public ReadOnly Property Contact() As Contact
        Get
            If _contact Is Nothing Then
                _contact = New Contact(_contactid)
            End If
            Return _contact
        End Get
    End Property

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property StatusDate() As DateTime
        Get
            Return _statusdate
        End Get
        Set(ByVal Value As DateTime)
            _statusdate = Value
        End Set
    End Property

    Public Property ScheduleDate() As DateTime
        Get
            Return _scheduledate
        End Get
        Set(ByVal Value As DateTime)
            _scheduledate = Value
        End Set
    End Property

    Public Property Notes() As String
        Get
            Return _notes
        End Get
        Set(ByVal Value As String)
            _notes = Value
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

    Public Sub New(ByVal pTaskId As String)
        _taskid = pTaskId
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function Exists(ByVal pTaskId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from routetasks where taskid = {0}", Made4Net.Shared.Util.FormatField(pTaskId))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim sql As String = String.Format("SELECT * FROM ROUTETASKS where " & WhereClause)
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Task Not Found")
        End If
        dr = dt.Rows(0)
        If Not IsDBNull(dr("tasktype")) Then _tasktype = dr("tasktype")
        If Not IsDBNull(dr("consignee")) Then _consignee = dr("consignee")
        If Not IsDBNull(dr("company")) Then _company = dr("company")
        If Not IsDBNull(dr("companytype")) Then _companytype = dr("companytype")
        If Not IsDBNull(dr("notes")) Then _notes = dr("notes")
        If Not IsDBNull(dr("status")) Then _status = dr("status")
        If Not IsDBNull(dr("statusdate")) Then _statusdate = dr("statusdate")
        If Not IsDBNull(dr("scheduledate")) Then _scheduledate = dr("scheduledate")
        If Not IsDBNull(dr("contactid")) Then _contactid = dr("contactid")
        If Not IsDBNull(dr("adddate")) Then _adddate = dr("adddate")
        If Not IsDBNull(dr("adduser")) Then _adduser = dr("adduser")
        If Not IsDBNull(dr("editdate")) Then _editdate = dr("editdate")
        If Not IsDBNull(dr("edituser")) Then _edituser = dr("edituser")
    End Sub

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUserId As String)
        Dim SQL As String
        _edituser = pUserId
        _editdate = DateTime.Now
        _status = pStatus
        _statusdate = DateTime.Now
        SQL = String.Format("update routetasks set status={0}, statusdate={1}, editdate={2}, edituser={3} where {4}", Made4Net.Shared.Util.FormatField(_status), _
                Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Create / Update / Delete"

    Public Sub Create(ByVal pTaskId As String, ByVal pTaskType As String, ByVal pScheduleDate As DateTime, ByVal pNotes As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pConsignee As String, ByVal pContactId As String, ByVal pUserId As String)
        If RoutePackage.Exists(pTaskId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Route Task - Task Already exists", "Cannot create Route Task - Task Already exists")
        End If
        If Not WMS.Logic.Company.Exists(pConsignee, pCompany, pCompanyType) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create Route Task - Target Company does not exists", "Cannot create Route Task - Target Company does not exists")
        End If
        If pContactId <> String.Empty AndAlso WMS.Logic.Contact.Exists(pContactId) Then
            _contactid = pContactId
        Else
            Dim oComp As New Company(pConsignee, pCompany, pCompanyType)
            _contactid = oComp.DEFAULTCONTACT
        End If
        Dim SQL As String
        If pTaskId = String.Empty Then
            _taskid = Made4Net.Shared.Util.getNextCounter("ROUTETASK")
        Else
            _taskid = pTaskId
        End If
        _tasktype = pTaskType
        _scheduledate = pScheduleDate
        _notes = pNotes
        _company = pCompany
        _companytype = pCompanyType
        _consignee = pConsignee
        _status = WMS.Lib.Statuses.RouteTasks.[NEW]
        _statusdate = DateTime.Now
        _adddate = DateTime.Now
        _adduser = pUserId
        _editdate = DateTime.Now
        _edituser = pUserId

        SQL = String.Format("INSERT INTO ROUTETASKS (TASKID, TASKTYPE, SCHEDULEDATE, STATUS, STATUSDATE, NOTES, CONSIGNEE, COMPANY, COMPANYTYPE, CONTACTID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _
            Made4Net.Shared.Util.FormatField(_taskid), Made4Net.Shared.Util.FormatField(_tasktype), Made4Net.Shared.Util.FormatField(_scheduledate), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_notes), _
            Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Update(ByVal pTaskType As String, ByVal pScheduleDate As DateTime, ByVal pNotes As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pConsignee As String, ByVal pStatus As String, ByVal pContactId As String, ByVal pUserId As String)
        If Not Exists(_taskid) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot edit Route Task - Task does not exists", "Cannot edit Route Task - Task does not exists")
        End If
        If Not WMS.Logic.Company.Exists(pConsignee, pCompany, pCompanyType) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot edit Route Task - Target Company does not exists", "Cannot edit Route Task - Target Company does not exists")
        End If
        If pContactId <> String.Empty AndAlso WMS.Logic.Contact.Exists(pContactId) Then
            _contactid = pContactId
        Else
            Dim oComp As New Company(pConsignee, pCompany, pCompanyType)
            _contactid = oComp.DEFAULTCONTACT
        End If
        Dim SQL As String
        _tasktype = pTaskType
        _notes = pNotes
        _scheduledate = pScheduleDate
        _company = pCompany
        _companytype = pCompanyType
        _consignee = pConsignee
        _status = pStatus
        _statusdate = DateTime.Now
        _editdate = DateTime.Now
        _edituser = pUserId

        SQL = String.Format("UPDATE ROUTETASKS SET TASKTYPE ={0}, SCHEDULEDATE={1}, STATUS ={2}, STATUSDATE ={3}, NOTES ={4}, CONSIGNEE ={5}, COMPANY ={6}, COMPANYTYPE ={7}, CONTACTID ={8}, EDITDATE ={9}, EDITUSER ={10} where {11}", _
            Made4Net.Shared.Util.FormatField(_tasktype), Made4Net.Shared.Util.FormatField(_scheduledate), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_notes), _
            Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub Delete()
        Dim sSql As String
        sSql = String.Format("select count(1) from ROUTESTOPTASK where STOPTASKTYPE='General' and DOCUMENTID = {0}", Made4Net.Shared.Util.FormatField(_taskid))
        If DataInterface.ExecuteScalar(sSql) > 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot delete route task - Task assigned to route", "Cannot delete route task - Task assigned to route")
        End If
        sSql = String.Format("delete from ROUTETASKS where {0}", WhereClause)
        DataInterface.RunSQL(sSql)
    End Sub

#End Region

#Region "Complete / InComplete"

    Public Sub Complete(ByVal pUserId As String)
        SetStatus(WMS.Lib.Statuses.RouteTasks.COMPLETED, pUserId)
    End Sub

    Public Sub InComplete(ByVal pReasonCode As String, ByVal pUserId As String)
        SetStatus(WMS.Lib.Statuses.RouteTasks.INCOMPLETE, pUserId)
    End Sub

    Public Sub Cancel(ByVal pReasonCode As String, ByVal pUserId As String)
        SetStatus(WMS.Lib.Statuses.RouteTasks.CANCELLED, pUserId)
    End Sub

#End Region

#End Region

End Class
