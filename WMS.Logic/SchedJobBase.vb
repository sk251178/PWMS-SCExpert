Imports Made4Net.Shared
Imports Made4Net.DataAccess

#Region "SchedJobBase"

<CLSCompliant(False)> Public Class SchedJobBase

#Region "Variables"

#Region "Primary Keys"

    Protected _name As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _assembly_dll As String = String.Empty
    Protected _class_name As String = String.Empty
    Protected _arguments As String
    Protected _description As String
    Protected _interval As Int32
    Protected _last_run As DateTime
    Protected _last_status As String
    Protected _last_log As String
    Protected _running As Boolean
    Protected _enabled As Boolean

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " name = '" & _name & "'"
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return _name
        End Get
    End Property

    Public Property AssemblyDll() As String
        Get
            Return _assembly_dll
        End Get
        Set(ByVal Value As String)
            _assembly_dll = Value
        End Set
    End Property

    Public Property ClassName() As String
        Get
            Return _class_name
        End Get
        Set(ByVal Value As String)
            _class_name = Value
        End Set
    End Property

    Public Property Arguments() As String
        Get
            Return _arguments
        End Get
        Set(ByVal Value As String)
            _arguments = Value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal Value As String)
            _description = Value
        End Set
    End Property

    Public Property Interval() As Int32
        Get
            Return _interval
        End Get
        Set(ByVal Value As Int32)
            _interval = Value
        End Set
    End Property

    Public Property LastRun() As DateTime
        Get
            Return _last_run
        End Get
        Set(ByVal Value As DateTime)
            _last_run = Value
        End Set
    End Property

    Public Property LastStatus() As String
        Get
            Return _last_status
        End Get
        Set(ByVal Value As String)
            _last_status = Value
        End Set
    End Property

    Public Property LastLog() As String
        Get
            Return _last_log
        End Get
        Set(ByVal Value As String)
            _last_log = Value
        End Set
    End Property

    Public Property Running() As Boolean
        Get
            Return _running
        End Get
        Set(ByVal Value As Boolean)
            _running = Value
        End Set
    End Property

    Public Property Enabled() As Boolean
        Get
            Return _enabled
        End Get
        Set(ByVal Value As Boolean)
            _enabled = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pName As String, Optional ByVal LoadObj As Boolean = True)
        _name = pName
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetSched(ByVal pName As String) As SchedJobBase
        Return New SchedJobBase(pName)
    End Function

    Public Shared Function Exists(ByVal pName As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from JOB_SCHEDULE where Name = '{0}'", pName)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM JOB_SCHEDULE Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("NAME") Then _name = dr.Item("NAME")
        If Not dr.IsNull("ASSEMBLY_DLL") Then _assembly_dll = dr.Item("ASSEMBLY_DLL")
        If Not dr.IsNull("CLASS_NAME") Then _class_name = dr.Item("CLASS_NAME")
        If Not dr.IsNull("ARGUMENTS") Then _arguments = dr.Item("ARGUMENTS")
        If Not dr.IsNull("DESCRIPTION") Then _description = dr.Item("DESCRIPTION")
        If Not dr.IsNull("INTERVAL") Then _interval = dr.Item("INTERVAL")
        If Not dr.IsNull("LAST_RUN") Then _last_run = dr.Item("LAST_RUN")
        If Not dr.IsNull("LAST_STATUS") Then _last_status = dr.Item("LAST_STATUS")
        If Not dr.IsNull("LAST_LOG") Then _last_log = dr.Item("LAST_LOG")
        If Not dr.IsNull("RUNNING") Then _running = dr.Item("RUNNING")
        If Not dr.IsNull("ENABLED") Then _enabled = dr.Item("ENABLED")

    End Sub

#End Region

End Class

#End Region

