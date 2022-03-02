Imports Made4Net.Algorithms
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data
<CLSCompliant(False)> Public Class SchedExecuter

    'Start RWMS-1754
#Region "Variables"
    Protected _scheduleID As String
    Protected _applicationId As String
    Protected _dbConnectionName As String
#End Region
#Region "Properties"

    Public Property ScheduleID() As String
        Get
            Return _scheduleID
        End Get
        Set(ByVal value As String)
            _scheduleID = value
        End Set
    End Property

    Public Property ApplicationId() As String
        Get
            Return _applicationId
        End Get
        Set(ByVal value As String)
            _applicationId = value
        End Set
    End Property

    Public Property DbConnectionName() As String
        Get
            Return _dbConnectionName
        End Get
        Set(ByVal value As String)
            _dbConnectionName = value
        End Set
    End Property
    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("ScheduleID = '{0}'", _scheduleID)
        End Get
    End Property
#End Region
    'End RWMS-1754
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Run" Then
            For Each dr In ds.Tables(0).Rows
                'Dim oSchedJob As New SchedJobBase(dr("APPLICATIONNAME"))
                'Dim Arguments As String() = CType(dr("arguments"), String).Split(",")
                'Dim obj As Made4Net.Shared.JobScheduling.ScheduledJobBase
                'obj = Made4Net.Shared.Reflection.CreateInstance(oSchedJob.AssemblyDll, oSchedJob.ClassName, Arguments)
                'obj.Execute()
            Next
        End If
    End Sub
    Public Sub New()
    End Sub
    Public Sub New(ByVal pScheduleId As Int32)
        ScheduleID = pScheduleId
        Load()
    End Sub
#Region "Create / Update"
    Public Sub CreateSchedule(ByVal pScheduleId As String, ByVal pApplicationId As String, ByVal pDbConnectionName As String)
        If CheckScheduleExist(pScheduleId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Schedule  - Already Exists", "Cannot Create Schedule - Already Exists")
        End If
       
        ScheduleID = pScheduleId
        ApplicationId = pApplicationId
        DbConnectionName = pDbConnectionName
       
        Dim sql As String = String.Format("INSERT INTO SchedulerApplications (ScheduleID,ApplicationID,DBConnectionName) VALUES ({0},{1},{2})", _
                   FormatField(ScheduleID), FormatField(ApplicationId), FormatField(DbConnectionName))
        DataInterface.RunSQL(sql, Made4Net.Schema.CONNECTION_NAME)

    End Sub
    Public Sub UpdateSchedule(ByVal pApplicationId As String, ByVal pDbConnectionName As String)

        ApplicationId = pApplicationId
        DbConnectionName = pDbConnectionName

        Dim sql As String = String.Format("UPDATE SchedulerApplications SET ApplicationId={0},DbConnectionName={1} WHERE {2}", _
                   FormatField(ApplicationId), FormatField(DbConnectionName), WhereClause)
        DataInterface.RunSQL(sql, Made4Net.Schema.CONNECTION_NAME)

    End Sub
#End Region

    Private Function CheckScheduleExist(pScheduleId As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(*) FROM SchedulerApplications WHERE ScheduleID={0}", FormatField(pScheduleId))
        Return DataInterface.ExecuteScalar(sql, Made4Net.Schema.CONNECTION_NAME)
    End Function
    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM SchedulerApplications WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt, False, Made4Net.Schema.CONNECTION_NAME)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Schedule ID does not exists", "Schedule ID does not exists")
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("ApplicationID") Then ApplicationId = dr.Item("ApplicationID")
        If Not dr.IsNull("DBConnectionName") Then DbConnectionName = dr.Item("DBConnectionName")
    End Sub
    'End RWMS-1754
End Class
