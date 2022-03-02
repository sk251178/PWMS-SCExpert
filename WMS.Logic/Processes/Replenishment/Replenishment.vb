Imports Made4Net.DataAccess
Imports WMS.Logic
Imports Made4Net.General
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports WMS.Lib
Imports Made4Net.Algorithms.Scoring
Imports Made4Net.Algorithms.SortingAlgorithms
Imports Made4Net.Shared

#Region "Replenishment"

<CLSCompliant(False)> Public Class Replenishment

    Public Enum DueDateEstimateType
        OrderPlanner
        Normal
    End Enum
#Region "Types"

    <CLSCompliant(False)> Public Class ReplenishmentTypes
        Public Const FullReplenishment As String = "FULLREPL"
        Public Const NegativeReplenishment As String = "NEGTREPL"
        Public Const PartialReplenishment As String = "PARTREPL"
    End Class

    Public Class ReplenishmentMethods
        'Public Const Replenishments As String = "REPLENISHMENT"
        'Public Const NormalReplenishment As String = "REPLENISHMENT"
        'Public Const OpporunityReplenishment As String = "OPRNTREPL"
        Public Const ManualReplenishment As String = "MANUALREPL"

        Public Sub New()
        End Sub
    End Class

#End Region

#Region "Variables"

    Protected _replid As String
    Protected _repltype As String
    Protected _strategyid As String
    Protected _replmethod As String
    Protected _fromload As String
    Protected _fromlocation As String
    Protected _fromwarehousearea As String
    Protected _toload As String
    Protected _tolocation As String
    Protected _towarehousearea As String
    Protected _units As Double
    Protected _uom As String
    Protected _status As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _currentOrderQty As Double
    Protected _dueDate As DateTime
    Protected _taskPriority As Integer

    'Added for RWMS-1867 Start
    Protected _userid As String
    'Added for RWMS-1867 End

    ' RWMS-2604
    Protected _estimateDueDateOnCreateType As DueDateEstimateType
    Protected _estimateDueDateOnCreateOrderId As String
    ' RWMS-2604


#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" REPLID = '{0}' ", _replid)
        End Get
    End Property

    Public ReadOnly Property ReplId() As String
        Get
            Return _replid
        End Get
    End Property
    Public Property STRATEGYID() As String
        Get
            Return _strategyid
        End Get
        Set(ByVal Value As String)
            _strategyid = Value
        End Set
    End Property
    Public Property ReplType() As String
        Get
            Return _repltype
        End Get
        Set(ByVal Value As String)
            _repltype = Value
        End Set
    End Property

    Public Property ReplMethod() As String
        Get
            Return _replmethod
        End Get
        Set(ByVal Value As String)
            _replmethod = Value
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

    Public Property ToLoad() As String
        Get
            Return _toload
        End Get
        Set(ByVal Value As String)
            _toload = Value
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

    Public Property Units() As Double
        Get
            Return _units
        End Get
        Set(ByVal Value As Double)
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

    Public Property Status() As String
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

    'Added for RWMS-1867 Start
    Public Property USERID() As String
        Get
            Return _userid
        End Get
        Set(ByVal Value As String)
            _userid = Value
        End Set
    End Property
    'Added for RWMS-1867 End

    Public Property REPLDATETIME() As DateTime
        Get
            Return _dueDate
        End Get
        Set(ByVal Value As DateTime)
            _dueDate = Value
        End Set
    End Property

    Public ReadOnly Property hasTask() As String
        Get
            Dim sql As String = String.Format("Select count(1) from TASKS where tasktype in ('{0}','{1}','{2}') and replenishment='{3}'", WMS.Lib.TASKTYPE.PARTREPL, WMS.Lib.TASKTYPE.FULLREPL, WMS.Lib.TASKTYPE.NEGTREPL, _replid)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property

    ' RWMS-2604
    Public WriteOnly Property EstimateDueDateOnCreateType() As DueDateEstimateType
        Set(value As DueDateEstimateType)
            Me._estimateDueDateOnCreateType = value
        End Set
    End Property

    Public WriteOnly Property EstimateDueDateOnCreateOrderId() As String
        Set(value As String)
            Me._estimateDueDateOnCreateOrderId = value
        End Set
    End Property
    ' RWMS-2604

#End Region

#Region "Constructor"

    Public Sub New()
        ' RWMS-2604
        Me._estimateDueDateOnCreateType = DueDateEstimateType.Normal
        Me._estimateDueDateOnCreateOrderId = String.Empty
        ' RWMS-2604
    End Sub

    Public Sub New(ByVal pReplId As String)
        _replid = pReplId
        Load()
        ' RWMS-2604
        Me._estimateDueDateOnCreateType = DueDateEstimateType.Normal
        Me._estimateDueDateOnCreateOrderId = String.Empty
        ' RWMS-2604
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "replenish"
                For Each dr In ds.Tables(0).Rows
                    _replid = dr("REPLID")
                    Load()
                    Try
                        Me.Replenish(_tolocation, _towarehousearea, UserId, False)
                    Catch m4nex As Made4Net.Shared.M4NException
                        Throw m4nex
                    Catch ex As Exception

                    End Try
                Next
            Case "cancelreplenish"
                For Each dr In ds.Tables(0).Rows
                    _replid = dr("REPLID")
                    Load()
                    Try
                        Me.Cancel(UserId)
                    Catch m4nex As Made4Net.Shared.M4NException
                        Throw m4nex
                    Catch ex As Exception

                    End Try
                Next
            Case "printworksheet"
                PrintWorksheet(Made4Net.Shared.Translation.Translator.CurrentLanguageID, UserId)
        End Select
        ' RWMS-2604
        Me._estimateDueDateOnCreateType = DueDateEstimateType.Normal
        Me._estimateDueDateOnCreateOrderId = String.Empty
        ' RWMS-2604
    End Sub

#End Region

#Region "Methods"

#Region "Accessibility"

    'RWMS-2604 - Added logger
    Public Shared Sub SetReplenDueDateTime(ByVal replid As String, ByVal duedatetime As DateTime, Optional ByVal oLogger As LogHandler = Nothing)

        Dim sql As String = $"UPDATE REPLENISHMENT SET DUEDATETIME = '{duedatetime}' WHERE REPLID='{replid}'"

        'RWMS-2604
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("SQL: {0}", sql))
        End If
        'RWMS-2604 END

        DataInterface.RunSQL(sql)

    End Sub

    Public Shared Function Exists(ByVal ReplenishmentId As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM REPLENISHMENT WHERE REPLID = '{0}'", ReplenishmentId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim dt As New DataTable
        Dim sql As String = String.Format("SELECT * FROM REPLENISHMENT WHERE {0}", WhereClause)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            'ERRROR NO CONTAINER
            Return
        End If

        Dim dr As DataRow = dt.Rows(0)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _replid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REPLID"))
        _repltype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REPLTYPE"))
        _replmethod = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REPLMETHOD"))
        _fromload = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMLOAD"))
        _fromlocation = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMLOCATION")))
        _fromwarehousearea = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMWAREHOUSEAREA")))
        _toload = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOLOAD")))
        _tolocation = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOLOCATION")))
        _towarehousearea = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOWAREHOUSEAREA")))
        _uom = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UOM")))
        _status = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATUS")))
        _units = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UNITS")))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

    'This function will get a replenishment job which is not based on a task, but on a regular replenishment
    Public Shared Function getReplenishment(ByVal pLoadid As String) As Replenishment
        Dim SQL As String = String.Format("select * from replenishment where fromload = '{0}' and status = '{1}'", pLoadid, WMS.Lib.Statuses.Replenishment.PLANNED)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "No Replenishment was found", "No Replenishment was found")
        End If
        Dim oRepl As New Replenishment(dt.Rows(0)("replid"))
        Return oRepl
    End Function

#End Region

#Region "Post"

    Public Sub Post(ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing)
        _replid = Made4Net.Shared.Util.getNextCounter("REPLENISHMENT")

        _status = WMS.Lib.Statuses.Replenishment.PLANNED
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("INSERT INTO REPLENISHMENT(REPLID,REPLTYPE,REPLMETHOD,FROMLOAD,FROMLOCATION,FROMWAREHOUSEAREA,TOLOAD,TOLOCATION,TOWAREHOUSEAREA,UNITS,UOM,STATUS,ADDDATE,ADDUSER,EDITDATE,EDITUSER,STRATEGYID,REPLDATETIME) values (" &
                                        "{0},{1},{2},{3},{4},{14},{5},{6},{15}, {7},{8},{9},{10},{11},{12},{13},{16},{17})",
            Made4Net.Shared.Util.FormatField(_replid), Made4Net.Shared.Util.FormatField(_repltype), Made4Net.Shared.Util.FormatField(_replmethod),
            Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_toload),
            Made4Net.Shared.Util.FormatField(_tolocation), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_uom),
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser),
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_fromwarehousearea),
            Made4Net.Shared.Util.FormatField(_towarehousearea), Made4Net.Shared.Util.FormatField(_strategyid), Made4Net.Shared.Util.FormatField(_dueDate))
        If Not oLogger Is Nothing Then
            oLogger.Write("About to insert Replenishment in to the Database, SQL: " & sql)
        End If
        DataInterface.RunSQL(sql)
        If Not oLogger Is Nothing Then
            oLogger.Write("Insertion done.")
        End If
    End Sub

#End Region

#Region "Replenish"

#Region "Accessors"


    'Added for RWMS-1867 Start
    Private Function AssignedToTask(ByVal ReplTaskId As String) As String
        Dim strSql As String
        If ReplTaskId <> "" Then
            strSql = "SELECT TASK FROM TASKS WHERE (STATUS = 'AVAILABLE') AND REPLENISHMENT ='" & ReplTaskId & "'"
        End If
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function
    'Added for RWMS-1867 End

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
    'Added a New method For RWMS-167
    Public Sub ChangeLoad(ByVal oLoad As Load, ByVal tempLoad As Load, ByVal pUser As String)
        _fromload = oLoad.LOADID
        _fromlocation = oLoad.LOCATION
        _fromwarehousearea = oLoad.WAREHOUSEAREA
        If _repltype = WMS.Lib.TASKTYPE.FULLREPL Then
            _toload = oLoad.LOADID
        End If
        _edituser = pUser
        _editdate = DateTime.Now


        If oLoad.ACTIVITYSTATUS = "REPLPEND" Then
            Dim sqlRepl As String = String.Format("SELECT * FROM REPLENISHMENT where  STATUS <> 'CANCELED' AND  STATUS <> 'COMPLETE' AND FROMLOAD ='{0}' AND FROMLOCATION='{1}' ", _fromload, _fromlocation)

            Dim dt As New DataTable
            Dim dr As DataRow
            If dt.Rows.Count > 1 Then
                'oLogger.Write(" dt.Rows.Count = : " & dt.Rows.Count)
                'oLogger.writeSeperator("-", 100)
            End If
            DataInterface.FillDataset(sqlRepl, dt)
            If dt.Rows.Count = 0 Then
                'Exception
            End If

            dr = dt.Rows(0)

            Dim subsReplId As String = String.Empty
            subsReplId = dr.Item("REPLID")
            Dim WhereClausesubsReplId As String = String.Empty
            WhereClausesubsReplId = String.Format(" REPLID = '{0}' ", subsReplId)


            Dim orginalFromload As String = String.Empty
            Dim orginalFromlocation As String = String.Empty
            Dim orginalFromwarehousearea As String = String.Empty
            Dim orginalToload As String = String.Empty

            orginalFromload = tempLoad.LOADID
            orginalFromlocation = tempLoad.LOCATION
            orginalFromwarehousearea = tempLoad.WAREHOUSEAREA
            If _repltype = WMS.Lib.TASKTYPE.FULLREPL Then
                orginalToload = tempLoad.LOADID
            End If

            Dim sqlSubstiReplenishUpdate As String
            sqlSubstiReplenishUpdate = String.Format("UPDATE REPLENISHMENT set fromload = {0}, fromlocation = {1}, toload = {2}, edituser = {3},editdate = {4} where {5}", Made4Net.Shared.Util.FormatField(orginalFromload), Made4Net.Shared.Util.FormatField(orginalFromlocation), Made4Net.Shared.Util.FormatField(orginalToload), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClausesubsReplId)
            DataInterface.RunSQL(sqlSubstiReplenishUpdate)

            Dim substituteTaskId As String = String.Empty
            substituteTaskId = DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(TASK,'') AS TASK FROM TASKS WHERE STATUS <> 'CANCELED' AND  STATUS <> 'COMPLETE' AND REPLENISHMENT='{0}' ", subsReplId))
            Dim whereClausesubstituteTaskId As String = String.Empty
            whereClausesubstituteTaskId = String.Format(" TASK = '{0}'", substituteTaskId)

            Dim sqlsubstituteTaskIdUpdate As String
            sqlsubstituteTaskIdUpdate = String.Format("UPDATE TASKS SET FROMLOAD = {0}, FROMLOCATION = {1}, TOLOAD={2}, FROMWAREHOUSEAREA = {3}, EDITUSER = {4}, EDITDATE = {5} where {6}", Made4Net.Shared.Util.FormatField(orginalFromload),
            Made4Net.Shared.Util.FormatField(orginalFromlocation), Made4Net.Shared.Util.FormatField(orginalToload), Made4Net.Shared.Util.FormatField(orginalFromwarehousearea), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClausesubstituteTaskId)
            DataInterface.RunSQL(sqlsubstituteTaskIdUpdate)

        ElseIf oLoad.ACTIVITYSTATUS = String.Empty Or oLoad.ACTIVITYSTATUS = "" Then

            Dim orginalFromload As String = String.Empty
            Dim orginalfromlocation As String = String.Empty
            Dim orginalfromwarehousearea As String = String.Empty


            orginalFromload = tempLoad.LOADID
            orginalfromlocation = tempLoad.LOCATION
            orginalfromwarehousearea = tempLoad.WAREHOUSEAREA

            Dim pickActivityStatus = String.Empty
            Dim pickDestinationLocation = String.Empty
            Dim pickDestinationWarehouse = String.Empty
            Dim WhereClausesubstitutePayloadid = String.Empty
            WhereClausesubstitutePayloadid = String.Format("LOADID = '{0}'", orginalFromload)

            pickActivityStatus = oLoad.ACTIVITYSTATUS
            pickDestinationLocation = oLoad.DESTINATIONLOCATION
            pickDestinationWarehouse = oLoad.DESTINATIONWAREHOUSEAREA

            Dim sqlOriginalLoadUpdate As String
            sqlOriginalLoadUpdate = String.Format("UPDATE LOADS SET ACTIVITYSTATUS = {0}, DESTINATIONLOCATION = {1}, DESTINATIONWAREHOUSEAREA = {2}, edituser = {3},editdate = {4} where {5}", Made4Net.Shared.Util.FormatField(pickActivityStatus),
            Made4Net.Shared.Util.FormatField(pickDestinationLocation), Made4Net.Shared.Util.FormatField(pickDestinationWarehouse), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClausesubstitutePayloadid)
            DataInterface.RunSQL(sqlOriginalLoadUpdate)

            Dim sqlPick As String = String.Format("SELECT * FROM PICKDETAIL where STATUS <> 'CANCELED' AND  STATUS <> 'COMPLETE' AND FROMLOAD ='{0}' AND FROMLOCATION='{1}' ", _fromload, _fromlocation)

            Dim dt As New DataTable
            Dim dr As DataRow
            If dt.Rows.Count > 1 Then
                'oLogger.Write(" dt.Rows.Count = : " & dt.Rows.Count)
                'oLogger.writeSeperator("-", 100)
            End If
            DataInterface.FillDataset(sqlPick, dt)
            If dt.Rows.Count = 0 Then
                'Exception
            End If

            dr = dt.Rows(0)

            Dim subsPicklistId As String = String.Empty
            Dim subspicklistline As String = String.Empty
            Dim WhereClausesubsPicklistId As String = String.Empty
            subsPicklistId = dr.Item("PICKLIST")
            subspicklistline = dr.Item("PICKLISTLINE")
            WhereClausesubsPicklistId = String.Format(" PICKLIST = '{0}' And PICKLISTLINE = {1}", subsPicklistId, subspicklistline)

            Dim sqlSubstiPickDtlUpdate As String
            sqlSubstiPickDtlUpdate = String.Format("UPDATE pickdetail SET fromload = {0}, fromlocation = {1}, fromwarehousearea = {5}, edituser = {2},editdate = {3} where {4}", Made4Net.Shared.Util.FormatField(orginalFromload),
            Made4Net.Shared.Util.FormatField(orginalfromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClausesubsPicklistId, Made4Net.Shared.Util.FormatField(orginalfromwarehousearea))
            DataInterface.RunSQL(sqlSubstiPickDtlUpdate)


            Dim substituteTaskId As String = String.Empty
            substituteTaskId = DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(TASK,'') AS TASK FROM TASKS WHERE STATUS <> 'CANCELED' AND  STATUS <> 'COMPLETE' AND PICKLIST='{0}' and TASKTYPE='FULLPICK'", subsPicklistId))
            Dim whereClausesubstituteTaskId As String = String.Empty
            whereClausesubstituteTaskId = String.Format(" TASK = '{0}'", substituteTaskId)

            Dim sqlsubstituteTaskIdUpdate As String
            sqlsubstituteTaskIdUpdate = String.Format("UPDATE TASKS SET FROMLOAD = {0}, FROMLOCATION = {1}, FROMWAREHOUSEAREA = {5}, EDITUSER = {2},EDITDATE = {3} where {4}", Made4Net.Shared.Util.FormatField(orginalFromload),
            Made4Net.Shared.Util.FormatField(orginalfromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClausesubstituteTaskId, Made4Net.Shared.Util.FormatField(orginalfromwarehousearea))
            DataInterface.RunSQL(sqlsubstituteTaskIdUpdate)

        End If

        Dim sqlCurrReplUpdate As String
        sqlCurrReplUpdate = String.Format("UPDATE REPLENISHMENT SET fromload = {0}, fromlocation = {1}, toload = {2}, edituser = {3},editdate = {4} where {5}", Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sqlCurrReplUpdate)

        Dim currTaskId As String = String.Empty
        currTaskId = DataInterface.ExecuteScalar(String.Format("SELECT ISNULL(TASK,'') AS TASK FROM TASKS WHERE STATUS <> 'CANCELED' AND  STATUS <> 'COMPLETE' AND REPLENISHMENT='{0}' ", _replid))
        Dim whereClausecurrTaskId = String.Empty
        whereClausecurrTaskId = String.Format(" TASK = '{0}'", currTaskId)

        Dim sqlCurrTaskUpdate As String
        sqlCurrTaskUpdate = String.Format("update TASKS set FROMLOAD = {0}, FROMLOCATION = {1}, TOLOAD={2}, FROMWAREHOUSEAREA = {3}, EDITUSER = {4}, EDITDATE = {5} where {6}", Made4Net.Shared.Util.FormatField(_fromload),
        Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_fromwarehousearea), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClausecurrTaskId)
        DataInterface.RunSQL(sqlCurrTaskUpdate)



    End Sub
    'End Added a New method For RWMS-167
    Public Sub ChangeLoad(ByVal oLoad As Load, ByVal pUser As String)
        _fromload = oLoad.LOADID
        _fromlocation = oLoad.LOCATION
        _fromwarehousearea = oLoad.WAREHOUSEAREA
        If _repltype = WMS.Lib.TASKTYPE.FULLREPL Then
            _toload = oLoad.LOADID
        End If
        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String
        sql = String.Format("update REPLENISHMENT set fromload = {0}, fromlocation = {1}, toload = {2}, edituser = {3},editdate = {4} where {5}", Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)

        'Added For RWMS-167
        Dim currTaskId As String = String.Empty
        currTaskId = DataInterface.ExecuteScalar(String.Format("select isnull(TASK,'') as Task from TASKS where REPLENISHMENT='{0}' ", _replid))
        Dim whereClause2 As String = String.Empty
        whereClause2 = String.Format(" TASK = '{0}'", currTaskId)

        Dim sql2 As String
        sql2 = String.Format("update TASKS set FROMLOAD = {0}, FROMLOCATION = {1}, TOLOAD={2}, FROMWAREHOUSEAREA = {3}, EDITUSER = {4}, EDITDATE = {5} where {6}", Made4Net.Shared.Util.FormatField(_fromload),
        Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_toload), Made4Net.Shared.Util.FormatField(_fromwarehousearea), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), whereClause2)
        DataInterface.RunSQL(sql2)
        'End Added For RWMS-167

    End Sub

    Protected Sub UpdateFromLocation(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String)
        _fromlocation = pLocation
        _fromwarehousearea = pWarehousearea
        _edituser = pUser
        _editdate = DateTime.Now
        Dim sql As String = String.Format("UPDATE REPLENISHMENT SET FROMLOCATION={0},FROMWAREHOUSEAREA={4}, EDITUSER={1}, EDITDATE = {2} WHERE {3}",
            Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
        DataInterface.RunSQL(sql)
    End Sub
    Protected Sub UpdateExecutionLocation(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String)
        _fromlocation = pLocation
        _fromwarehousearea = pWarehousearea
        _edituser = pUser
        _editdate = DateTime.Now
        Dim sql As String = String.Format("UPDATE REPLENISHMENT SET FROMLOCATION={0},FROMWAREHOUSEAREA={4}, EDITUSER={1}, EDITDATE = {2} WHERE {3}",
            Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
        DataInterface.RunSQL(sql)
    End Sub

    Protected Sub UpdateLoadLocation(ByVal pLoadid As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String)
        Dim edituser As String = pUser
        Dim editdate As DateTime = DateTime.Now
        Dim sql As String = String.Format("UPDATE LOADS SET LOCATION={0},WAREHOUSEAREA={4}, EDITUSER={1}, EDITDATE = {2} WHERE loadid = {3}",
            Made4Net.Shared.Util.FormatField(pLocation), Made4Net.Shared.Util.FormatField(edituser), Made4Net.Shared.Util.FormatField(editdate), Made4Net.Shared.Util.FormatField(pLoadid), Made4Net.Shared.Util.FormatField(pWarehousearea))
        DataInterface.RunSQL(sql)
    End Sub
    Private Function ShouldCreateLoadPickUpTask() As Boolean
        Dim sql As String
        sql = String.Format("SELECT count(1)  FROM  dbo.REPLPOLICY where POLICYID={0} and PICKUPLOADAFTERNEGTREPL= 1", Made4Net.Shared.FormatField(Me.STRATEGYID))
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    Public Sub Replenish(ByVal pReplLocation As String, ByVal pReplWarehousearea As String, ByVal pUser As String, ByVal CreatePutawayJob As Boolean, Optional ByVal oLogger As LogHandler = Nothing)
        If _status <> WMS.Lib.Statuses.Replenishment.PLANNED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Replenishment Status", "Wrong Replenishment Status")
        End If
        ' Labor calc changes RWMS-952 -> PWMS-903
        ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User
        Dim oWHActivity As New WHActivity
        oWHActivity.USERID = pUser
        oWHActivity.SaveLastLocation()
        ' Fill StartLocation & Previous Activity as WHActivity's Location & Activity for Current User
        ' Labor calc changes RWMS-952 -> PWMS-903

        If _tolocation = pReplLocation And _towarehousearea = pReplWarehousearea Then
            If _fromload = _toload Then
                Dim ld As New Load(_fromload)
                ld.Replenish(_tolocation, _towarehousearea, pUser)
                Complete(pUser)
            Else
                'new code of creating the loadid
                If _repltype = ReplenishmentTypes.NegativeReplenishment Then
                    Dim ld As New Load(_fromload)
                    _toload = WMS.Logic.Load.GenerateLoadId()
                    Dim sql As String = String.Format("UPDATE REPLENISHMENT SET TOLOAD={0} WHERE {1}", Made4Net.Shared.Util.FormatField(_toload), WhereClause)
                    DataInterface.RunSQL(sql)
                    'PWMS-832 Not Able to complete Replenishment Task by RDT as gives error
                    ' _uom is passed as '%' which is not a valid UOM hence the error.
                    _uom = ld.LOADUOM
                    'PWMS-832Not Able to complete Replenishment Task by RDT as gives error
                    Dim toSKU As SKU = Nothing
                    sql = $"select sku from vpickloc where location='{_tolocation}'"
                    toSKU = New SKU(ld.CONSIGNEE, Convert.ToString(DataInterface.ExecuteScalar(sql)))
                    If Not (String.IsNullOrEmpty(toSKU.PARENTSKU)) Then
                        ChildReplenish(pUser, ld, toSKU)
                    Else
                        ld.SplitReplenish(_units, _uom, _toload, pUser, False)
                        ld.Replenish(_tolocation, _towarehousearea, pUser)
                    End If
                    Complete(pUser)

                    ld = New Load(_fromload)
                    If ld.STATUS <> String.Empty Then
                        sql = String.Format("update loads set location ={0},warehousearea={1} where loadid={2}",
                    Made4Net.Shared.FormatField(_tolocation), Made4Net.Shared.FormatField(_towarehousearea),
                    Made4Net.Shared.FormatField(_fromload))
                        DataInterface.RunSQL(sql)

                        If CreatePutawayJob Then
                            Dim putAwayLoad = New Load(_fromload)
                            putAwayLoad.PutAway(String.Empty, String.Empty, pUser, True)
                        End If
                    End If
                Else 'ReplenishmentTypes.PartialReplenishment
                    If WMS.Logic.Load.Exists(_toload) Then
                        Dim ld As New Load(_toload)
                        ld.Replenish(_tolocation, _towarehousearea, pUser)
                    Else
                        Dim ld As New Load(_fromload)
                        _toload = WMS.Logic.Load.GenerateLoadId()
                        Dim sql As String = String.Format("UPDATE REPLENISHMENT SET TOLOAD={0} WHERE {1}", Made4Net.Shared.Util.FormatField(_toload), WhereClause)
                        DataInterface.RunSQL(sql)
                        ld.SplitReplenish(_units, _uom, _toload, pUser, False)
                        ld.Replenish(_tolocation, _towarehousearea, pUser)
                    End If
                    Complete(pUser)
                End If
            End If
        Else 'We have a HandOff Location
            Dim previousFromLocation As String = _fromlocation
            UpdateFromLocation(pReplLocation, pReplWarehousearea, pUser)
            If _repltype = ReplenishmentTypes.PartialReplenishment Then
                Dim ld As New Load(_fromload)
                _toload = WMS.Logic.Load.GenerateLoadId()
                Dim sql As String = String.Format("UPDATE REPLENISHMENT SET FROMLOAD={0} WHERE {1}", Made4Net.Shared.Util.FormatField(_toload), WhereClause)
                DataInterface.RunSQL(sql)
                ld.SplitReplenish(_units, _uom, _toload, pUser, True)
                UpdateLoadLocation(_toload, pReplLocation, pReplWarehousearea, pUser)
            Else
                UpdateLoadLocation(_fromload, pReplLocation, pReplWarehousearea, pUser)
            End If
            sendReplAuditMsg(_status, pReplLocation, _fromload, previousFromLocation)
        End If
    End Sub

    Private Function ChildReplenish(pUser As String, ByRef ld As Load, toSKU As SKU) As Load
        Dim newLoad As Load = Nothing
        Dim oldQty As Decimal
        Dim newQty As Decimal
        Dim convQuantity = toSKU.ConvertParentLowestUomUnitsToMyLowestUomUnits(_units)
        ld.SplitReplenish(_units, _uom, _toload, pUser, False, toSKU, convQuantity)
        ld.GenerateInvAdj(WMS.Lib.INVENTORY.SUBQTY, ld.UNITS + _units, _units, "ChildReplenishment", pUser)
        newLoad = ld.Replenish(_tolocation, _towarehousearea, pUser)
        If newLoad Is Nothing Then
            oldQty = 0
            newQty = convQuantity
        Else
            oldQty = newLoad.UNITS - convQuantity
            newQty = newLoad.UNITS
        End If
        newLoad.GenerateInvAdj(WMS.Lib.INVENTORY.ADDQTY, oldQty, newQty, "ChildReplenishment", pUser)
        Return newLoad
    End Function

    Public Sub OverrideReplenish(ByVal ReplTask As ReplenishmentTask, ByVal oReplJob As ReplenishmentJob, ByVal pOverrideLocation As String, ByVal pOverrideWarehousearea As String, ByVal CreatePutawayJob As Boolean, ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.Replenishment.PLANNED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Replenishment Status", "Wrong Replenishment Status")
        End If
        If ReplTask.TOLOCATION.Equals(pOverrideLocation, StringComparison.OrdinalIgnoreCase) Then
            Dim bShouldCreatePW As Boolean = False
            If _repltype = ReplenishmentTypes.NegativeReplenishment Then
                bShouldCreatePW = True
            End If
            oReplJob.IsHandOff = False
            oReplJob.toLocation = ReplTask.TOLOCATION
            ReplTask.Replenish(Me, oReplJob, WMS.Logic.Common.GetCurrentUser, bShouldCreatePW)
            Return
        End If
        If _repltype = ReplenishmentTypes.NegativeReplenishment Then
            Dim ld As New Load(_fromload)
            ' Set load location
            ld.LOCATION = pOverrideLocation
            ld.WAREHOUSEAREA = pOverrideWarehousearea
            _toload = WMS.Logic.Load.GenerateLoadId()
            Dim sql As String = String.Format("UPDATE REPLENISHMENT SET TOLOAD={0} WHERE {1}", Made4Net.Shared.Util.FormatField(_toload), WhereClause)
            DataInterface.RunSQL(sql)
            ld.SplitReplenish(_units, _uom, _toload, pUser, False)

            ld.Replenish(pOverrideLocation, pOverrideWarehousearea, pUser)
            'Tolocation is not saved on the db, it will only be sent to the audit msg.
            _tolocation = pOverrideLocation
            Complete(pUser)
            If CreatePutawayJob Then
                ld = New Load(_fromload)
                If ld.UNITS > 0 AndAlso ld.STATUS <> String.Empty Then
                    If ShouldCreateLoadPickUpTask() Then
                        ld.RequestPickUp(pUser, "", True) ' RWMS-1277

                    Else
                        ld.PutAway(String.Empty, String.Empty, pUser, True)
                    End If
                End If
            End If
        Else
            ' Move Load to new Location
            Dim ld As WMS.Logic.Load = New WMS.Logic.Load(ReplTask.FROMLOAD)
            Dim unitsAllocated As Double = ld.UNITSALLOCATED
            ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Lib.USERS.SYSTEMUSER)
            Dim destloc As String = ld.DESTINATIONLOCATION
            Dim destwarehousearea As String = ld.DESTINATIONWAREHOUSEAREA
            ld.unAllocate(unitsAllocated, WMS.Lib.USERS.SYSTEMUSER)
            ld.Move(pOverrideLocation, pOverrideWarehousearea, "", WMS.Logic.Common.GetCurrentUser)
            ld.Allocate(unitsAllocated, WMS.Lib.USERS.SYSTEMUSER)
            ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.REPLPENDING, WMS.Lib.USERS.SYSTEMUSER)
            ld.SetDestinationLocation(destloc, destwarehousearea, WMS.Lib.USERS.SYSTEMUSER)
            'complete the original task and open a new one from the override location to the destination location of the replenishment.
            ReplTask.ExecutionWarehousearea = pOverrideWarehousearea
            ReplTask.ExecutionLocation = pOverrideLocation
            ReplTask.Complete(Nothing) ' complete the original task
            'create a new task from exec location to destination
            Dim oNewReplTask As New ReplenishmentTask()
            oNewReplTask.FROMLOCATION = pOverrideLocation
            oNewReplTask.FROMWAREHOUSEAREA = pOverrideWarehousearea
            oNewReplTask.FROMLOAD = ReplTask.FROMLOAD
            oNewReplTask.FromContainer = ReplTask.FromContainer

            oNewReplTask.TOLOCATION = ReplTask.TOLOCATION
            oNewReplTask.TOWAREHOUSEAREA = ReplTask.TOWAREHOUSEAREA
            oNewReplTask.TOLOAD = ReplTask.TOLOAD
            oNewReplTask.ToContainer = ReplTask.ToContainer

            oNewReplTask.CONSIGNEE = ReplTask.CONSIGNEE
            oNewReplTask.Replenishment = ReplTask.Replenishment
            oNewReplTask.TASKTYPE = ReplTask.TASKTYPE
            'oNewReplTask.STATUS = WMS.Lib.Statuses.Task.AVAILABLE
            oNewReplTask.PRIORITY = ReplTask.PRIORITY
            oNewReplTask.DOCUMENT = ReplTask.DOCUMENT
            oNewReplTask.DOCUMENTLINE = ReplTask.DOCUMENTLINE
            oNewReplTask.SKU = ReplTask.SKU

            oNewReplTask.STARTWAREHOUSEAREA = pOverrideWarehousearea
            oNewReplTask.Post()
            UpdateFromLocation(pOverrideLocation, pOverrideWarehousearea, pUser)
            '        UpdateExecutionLocation(pOverrideLocation, pOverrideWarehousearea, pUser)
        End If
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        If _status <> WMS.Lib.Statuses.Replenishment.PLANNED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Replenishment Status", "Wrong Replenishment Status")
        End If

        Dim ld As New Load(_fromload)
        ld.CancelReplenish(_tolocation, _towarehousearea, _units, pUser)

        _status = WMS.Lib.Statuses.Replenishment.CANCELED
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update REPLENISHMENT set status={0},editdate={1},edituser={2} where {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        If hasTask Then
            Dim ts As Task = getReplTask(True)
            If ts.STATUS <> WMS.Lib.Statuses.Task.CANCELED And ts.STATUS <> WMS.Lib.Statuses.Task.COMPLETE Then
                ts.Cancel()
            End If
        End If
    End Sub

    Protected Sub Complete(ByVal pUser As String)
        Dim fromStatus As String = _status
        _status = WMS.Lib.Statuses.Replenishment.COMPLETE
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update REPLENISHMENT set status={0},editdate={1},edituser={2},tolocation={4},towarehousearea={5} where {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_tolocation), Made4Net.Shared.Util.FormatField(_towarehousearea))
        DataInterface.RunSQL(sql)
        If hasTask Then
            'added for RWMS-1692 and RWMS-1391 Start
            Dim sqlUpdatetask As String = String.Format("UPDATE TASKS SET ExecutionWareHouseArea={0} where REPLENISHMENT ={1}", Made4Net.Shared.Util.FormatField(_towarehousearea), Made4Net.Shared.Util.FormatField(_replid))
            DataInterface.RunSQL(sqlUpdatetask)
            'added for RWMS-1692 and RWMS-1391 End

            Dim ts As ReplenishmentTask = getReplTask(False)
            ' If Execution location is not set and remain empty in WHActivity table. Must be set.
            If ts.ExecutionLocation = "" Then
                If ts.TOLOCATION IsNot Nothing And ts.TOLOCATION <> "" Then
                    ts.ExecutionLocation = ts.TOLOCATION
                Else
                    ts.ExecutionLocation = ts.FROMLOCATION
                End If
            End If
            ' WareHouse
            If ts.TOWAREHOUSEAREA IsNot Nothing And ts.TOWAREHOUSEAREA <> "" Then
                ts.ExecutionWarehousearea = ts.TOWAREHOUSEAREA
            Else
                ts.ExecutionWarehousearea = ts.FROMWAREHOUSEAREA
            End If
            ts.Complete(Nothing)
        End If
        sendReplAuditMsg(fromStatus, _tolocation, _toload, _fromlocation)
    End Sub

    Private Sub sendReplAuditMsg(ByVal pFromStatus As String, ByVal pToLocation As String, ByVal pToLoad As String, ByVal pFromLocation As String)
        'Send proper Event
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format("Select consignee,sku from loads where loadid = '{0}'", _fromload), dt)
        Dim sSku As String = dt.Rows(0)("SKU")
        Dim sConsignee As String = dt.Rows(0)("CONSIGNEE")

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReplenishmentCompleted
        em.Add("ACTIVITYTIME", "0")
        'em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.REPLCOMPL)
        em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.REPLENISHMENT)
        em.Add("DOCUMENT", _replid)
        em.Add("FROMLOAD", _fromload)
        em.Add("CONSIGNEE", sConsignee)
        em.Add("SKU", sSku)
        em.Add("FROMLOC", pFromLocation)
        em.Add("FROMQTY", _units)
        em.Add("FROMSTATUS", pFromStatus)
        em.Add("NOTES", "")
        em.Add("TOLOAD", pToLoad)
        em.Add("TOLOC", pToLocation)
        em.Add("TOQTY", _units)
        em.Add("TOSTATUS", _status)
        em.Add("USERID", Common.GetCurrentUser)
        em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("LASTMOVEUSER", Common.GetCurrentUser)
        em.Add("LASTSTATUSUSER", Common.GetCurrentUser)
        em.Add("LASTCOUNTUSER", Common.GetCurrentUser)
        em.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ADDUSER", Common.GetCurrentUser)
        em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("EDITUSER", Common.GetCurrentUser)
        em.Add("EVENT", EventType)
        em.Add("REPLID", _replid)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Function CreatePolicyDetailTable() As DataTable
        Dim dt As New DataTable
        Dim dc As DataColumn = New DataColumn
        Try
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "PICKREGION"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "UOM"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "REPLTYPE"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.Int32")
            dc.ColumnName = "CREATETASK"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.Int32")
            dc.ColumnName = "TASKPRIORITY"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "ALLOCUOMQTY"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "TASKSUBTYPE"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "POLICYID"
            dt.Columns.Add(dc)
        Catch ex As Exception

        End Try
        Return dt
    End Function

    Public Function CreatePolicyDetailRow(ByVal oPolicyDetail As DataTable, ByVal sPolicyID As String, ByVal sPriority As String) As DataRow
        Dim rpLn As DataTable = New DataTable
        'Commented for RWMS-2615 RWMS-1867 Start
        'DataInterface.FillDataset("SELECT ISNULL(POLICYID,'')as POLICYID,REPLACE(ISNULL(FROMPICKREGION,'%'),'*','%') AS FROMPICKREGION, REPLACE(ISNULL(UOM ,'%'),'*','%') AS UOM, ISNULL(REPLTYPE,'') AS REPLTYPE,ISNULL(CREATETASK,0) AS CREATETASK,ISNULL(TASKPRIORITY,200) AS TASKPRIORITY, ISNULL(ALLOCUOMQTY,'') as ALLOCUOMQTY,ISNULL(TASKSUBTYPE,'') as TASKSUBTYPE FROM REPLPOLICYDETAIL WHERE POLICYID = '" & sPolicyID & "' AND PRIORITY = '" & sPriority & "'", rpLn)
        'Commented for RWMS-2615 RWMS-1867 End

        'Added for RWMS-2615 RWMS-1867 Start
        Dim masterDao As MasterDao = New MasterDao()
        Dim priorityNormalValueStr As String = masterDao.GetPriorityCodeForTask(WMS.Lib.TASKPRIORITY.PRIORITY_NORMAL)
        DataInterface.FillDataset("SELECT ISNULL(POLICYID,'')as POLICYID,REPLACE(ISNULL(FROMPICKREGION,'%'),'*','%') AS FROMPICKREGION, REPLACE(ISNULL(UOM ,'%'),'*','%') AS UOM, ISNULL(REPLTYPE,'') AS REPLTYPE,ISNULL(CREATETASK,0) AS CREATETASK,ISNULL(TASKPRIORITY," & priorityNormalValueStr & ") AS TASKPRIORITY, ISNULL(ALLOCUOMQTY,'') as ALLOCUOMQTY,ISNULL(TASKSUBTYPE,'') as TASKSUBTYPE FROM REPLPOLICYDETAIL WHERE POLICYID = '" & sPolicyID & "' AND PRIORITY = '" & sPriority & "'", rpLn)
        'Added for RWMS-2615 RWMS-1867 End

        Dim lnPickRegion As String
        Dim lnUom As String
        Dim lnReplType As String
        Dim lnCreateTask As Integer
        Dim lnTaskPriorety As String
        Dim lnAllocUOMQty As String
        Dim lnTaskSubType As String
        Dim lnPolicyID As String
        For Each ln As DataRow In rpLn.Rows
            If ln("FROMPICKREGION") = "%" Then
                lnPickRegion = ln("FROMPICKREGION")
            End If
            If lnPickRegion <> "%" Then
                lnPickRegion = lnPickRegion & ln("FROMPICKREGION") & ","
            End If
            If ln("UOM") = "%" Then
                lnUom = ln("UOM")
            End If
            If lnUom <> "%" Then
                lnUom = lnUom & ln("UOM") & ","
            End If
            lnReplType = ln("REPLTYPE")
            If ln("CREATETASK") = "1" Then
                lnCreateTask = 1
            End If
            If lnCreateTask <> 1 Then
                lnCreateTask = ln("CREATETASK")
            End If
            lnTaskPriorety = ln("TASKPRIORITY")
            lnAllocUOMQty = ln("ALLOCUOMQTY")
            lnTaskSubType = ln("TASKSUBTYPE")
            lnPolicyID = ln("POLICYID")
        Next
        Dim tmpLn As DataRow = oPolicyDetail.NewRow

        tmpLn("PICKREGION") = lnPickRegion.TrimEnd(",")
        tmpLn("UOM") = lnUom.TrimEnd(",")
        tmpLn("REPLTYPE") = lnReplType
        tmpLn("CREATETASK") = lnCreateTask
        tmpLn("TASKPRIORITY") = lnTaskPriorety
        tmpLn("ALLOCUOMQTY") = lnAllocUOMQty
        tmpLn("TASKSUBTYPE") = lnTaskSubType
        tmpLn("POLICYID") = lnPolicyID
        Return tmpLn
    End Function

    Public Function getStockFilterByPriority(ByVal sPickRegions As String, ByVal sUoms As String) As String
        Dim aPickRegions As String() = sPickRegions.Split(",")
        Dim aUoms As String() = sUoms.Split(",")

        If aPickRegions.Length = 1 Then
            Return " (PICKREGION LIKE '" & sPickRegions & "' AND LOADUOM LIKE '" & sUoms & "') "
        End If
        If aUoms.Length >= 1 Then
            sUoms = aUoms(0)
        End If
        Dim SQLFilter As String = "LOADUOM LIKE '" & sUoms & "' AND ("
        For i As Int32 = 0 To aPickRegions.Length - 1
            SQLFilter = SQLFilter & " (PICKREGION LIKE '" & aPickRegions(i) & "') OR"
        Next
        SQLFilter = " (" & SQLFilter.TrimEnd("OR".ToCharArray) & ")) "
        Return SQLFilter
    End Function

#Region "Location replanishment methods"

    Public Function LocReplenish(ByVal PickLocDataRow As DataRow, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0, Optional ByVal processUserPickShort As Boolean = False) As Boolean
        ' No need to chech if location exists because we selected it and passed as datarow
        Dim zb As New PickLoc(PickLocDataRow)
        LocReplenish(zb, pReplMethod, pUser, oLogger, Simulation)
    End Function
    'Added for RWMS-2598 Start
    Public Function LocReplenishReportProblem(ByVal Loc As String, ByVal Warehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal Simulation As Integer = 0, Optional ByVal PickedQty As Integer = 0) As Boolean

        Dim oLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        Dim zb As New PickLoc(Loc, Warehousearea, pConsignee, pSku)
        Dim locObj As New WMS.Logic.Location(zb.Location, zb.Warehousearea)

        If Not oLogger Is Nothing Then
            oLogger.Write("Replenishment method " & pReplMethod)
        End If

        If locObj.STATUS = False Then
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Can not replenish Location {0}. Location status is false.", locObj.Location))
            Return False
        End If
        If locObj.PROBLEMFLAG Then
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Can not replenish Location {0}. Location problem flag is true.", locObj.Location))
            Return False
        End If

        Dim sql As String
        ' Get policy details , if there is same priority then group them by this priority
        ' Group all Priorities
        Dim pReplPolicy As String
        '[Get PolicyID by type]

        'CAD 235- commented
        'If pReplMethod = ReplenishmentMethods.NormalReplenishment Or pReplMethod = ReplenishmentMethods.ManualReplenishment Then
        '    pReplPolicy = PickLocation.NormalReplPolicy
        'Else
        '    pReplPolicy = PickLocation.HotReplPolicy
        'End If

        'started for PWMS-756
        pReplPolicy = zb.ReplPolicy
        'Ended for PWMS-756

        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Using {0} as replenishment policy.", pReplPolicy))

        Dim prTbl As DataTable = New DataTable
        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY", prTbl)
        'For each priority found build policy detail table group by priority
        Dim dt As New DataTable
        dt = CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            dt.Rows.Add(CreatePolicyDetailRow(dt, pReplPolicy, pr("PRIORITY")))
        Next
        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        'Return CreateNormalReplTasksReportProblem(dt, zb, pReplMethod, oLogger, Simulation)
        Dim currentqty As Double = zb.CurrentQty + zb.PendingQty - zb.AllocatedQty

        Dim dr As DataRow

        'Dim objpriority As Prioritize = New PrioritizeReplenishments
        For Each dr In dt.Rows

            While True

                If Not CreateReplTasksInRegionReportProblem(dr, zb, currentqty, pReplMethod, oLogger, "", False, Simulation) Then
                    Exit While
                End If
            End While
        Next
    End Function
    'Added for RWMS-2598 End
    'Public Function LocReplenish(ByVal Loc As String, ByVal Warehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0) As Boolean
    'Added an optional parameter "logInUserId" to capture the Logged in User for RWMS-1867
    Public Function LocReplenish(ByVal Loc As String, ByVal Warehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0, Optional ByVal PickedQty As Integer = 0, Optional ByVal processUserPickShort As Boolean = False, Optional ByVal logInUserId As String = "") As Boolean
        Dim zb As New PickLoc(Loc, Warehousearea, pConsignee, pSku)
        LocReplenish(zb, pReplMethod, pUser, oLogger, Simulation, PickedQty, processUserPickShort, logInUserId)
    End Function

    'Private Function LocReplenish(ByVal PickLocation As PickLoc, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0) As Boolean
    'Added an optional parameter "logInUserId" to capture the Logged in User for RWMS-1867
    Private Function LocReplenish(ByVal PickLocation As PickLoc, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0, Optional ByVal PickedQty As Integer = 0, Optional ByVal processUserPickShort As Boolean = False, Optional ByVal logInUserId As String = "") As Boolean
        Dim locObj As New WMS.Logic.Location(PickLocation.Location, PickLocation.Warehousearea)

        If Not oLogger Is Nothing Then
            oLogger.Write("Replenishment method " & pReplMethod)
        End If

        If locObj.STATUS = False Then
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Can not replenish Location {0}. Location status is false.", locObj.Location))
            Return False
        End If
        If locObj.PROBLEMFLAG Then
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Can not replenish Location {0}. Location problem flag is true.", locObj.Location))
            Return False
        End If

        Dim sql As String
        ' Get policy details , if there is same priority then group them by this priority
        ' Group all Priorities
        Dim pReplPolicy As String
        '[Get PolicyID by type]

        'CAD 235- commented
        'If pReplMethod = ReplenishmentMethods.NormalReplenishment Or pReplMethod = ReplenishmentMethods.ManualReplenishment Then
        '    pReplPolicy = PickLocation.NormalReplPolicy
        'Else
        '    pReplPolicy = PickLocation.HotReplPolicy
        'End If

        'started for PWMS-756
        pReplPolicy = PickLocation.ReplPolicy
        'Ended for PWMS-756

        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Using {0} as replenishment policy.", pReplPolicy))

        Dim prTbl As DataTable = New DataTable
        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY", prTbl)
        'For each priority found build policy detail table group by priority
        Dim dt As New DataTable
        dt = CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            dt.Rows.Add(CreatePolicyDetailRow(dt, pReplPolicy, pr("PRIORITY")))
        Next
        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        'Added for RWMS-1867 Start
        If pReplMethod = ReplenishmentMethods.ManualReplenishment Then
            USERID = logInUserId
        End If
        oLogger.SafeWrite("processUserPickShort {0}", processUserPickShort)
        oLogger.SafeWrite("Using {0} as replenishment method.", pReplMethod)
        If processUserPickShort Then
            CreateReplPickShortTasks(dt, PickLocation, oLogger)
            UpdateprioritizationforImmediate(pReplMethod, PickLocation, oLogger)
        Else
            If pReplMethod = ReplenishmentMethods.ManualReplenishment Then
                Return CreateNormalReplTasks(dt, PickLocation, pReplMethod, oLogger, Simulation)
            Else
                CreateReplTasks(dt, PickLocation, oLogger)
                CheckPickLocToUpdatepriority(PickLocation, oLogger)
            End If
        End If
    End Function
    ' Added new method for PWMS-860 for short Pick
    Public Function Updateprioritization(pReplMethod As String, PickLocation As PickLoc, Optional logHandler As LogHandler = Nothing)
        Try
            'Added for RWMS-1840
            Dim ReplPolicyMethod As String = GetPolicyID()
            'Ended for RWMS-1840

            'Needs to call Logic here for prioritization
            Dim objpriority As Prioritize = New PrioritizeReplenishments
            Dim AssignedPriorityType As Integer
            Dim parameters As New Generic.Dictionary(Of String, Object)
            objpriority.LogHandler = logHandler
            parameters.Add(ReplPolicyMethod, pReplMethod)
            parameters.Add(REPLENISHMENTS.PICKLOC, PickLocation)
            _taskPriority = Convert.ToInt32(objpriority.GetTaskPriority(parameters))
            'RWMS-2714 Commented
            'AssignedPriorityType = objpriority.SetPriority(_taskPriority, PickLocation)
            'RWMS-2714 Commented END
            'RWMS-2714
            Dim objpriorityreset As Prioritize = New PrioritizeReplenishments
            Dim PriorityReset As Integer = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_NORMAL))
            If Not logHandler Is Nothing Then
                logHandler.Write("Updateprioritization: ResetPriroity({0}, {1}, {2})",
                                 _taskPriority, PickLocation, PriorityReset)
            End If
            AssignedPriorityType = objpriority.ReSetPriority(_taskPriority, PickLocation, PriorityReset, logHandler)
            'RWMS-2714 END
        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function
    ' Ended new method for PWMS-860 for short Pick
    ' Added new method for PWMS-860 for short Pick
    Public Function UpdateprioritizationforImmediate(pReplMethod As String, PickLocation As PickLoc, Optional logHandler As LogHandler = Nothing)
        Try

            'Needs to call Logic here for prioritization
            Dim objpriority As Prioritize = New PrioritizeReplenishments
            objpriority.LogHandler = logHandler
            Dim AssignedPriorityType As Integer
            _taskPriority = Convert.ToInt32(objpriority.GetTaskPriorityImmediate())
            AssignedPriorityType = objpriority.SetPriority(_taskPriority, PickLocation)
        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function
    ' Ended new method for PWMS-860 for short Pick
#End Region



    Protected Function FindBestLoadForPickLoc(ByVal zb As PickLoc) As Load
        If zb.ReplQty < zb.CurrentQty Then Return Nothing
        Dim dt As New DataTable
        Dim sql As String = String.Format("SELECT LOADID FROM INVLOAD WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE > 0 AND UNITSAVAILABLE <= {2} AND UNITSALLOCATED = 0 AND STATUS = '{3}' AND (ACTIVITYSTATUS IS NULL OR ACTIVITYSTATUS = '{4}') AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC", zb.Consignee, zb.SKU, zb.MaximumReplQty - zb.CurrentQty - zb.PendingQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Dim ld As New Load(Convert.ToString(dt.Rows(0)("LOADID")))
            Return ld
        Else
            Return Nothing
        End If
    End Function

    'Added for RWMS-2598 Start
    Protected Function CreateReplTasksInRegionReportProblem(ByVal PolicyDetail As DataRow, ByVal zb As PickLoc, ByRef CurrentQty As Double,
                                                            ByVal pReplMethod As String, Optional ByVal oLogger As LogHandler = Nothing,
                                                            Optional ByVal pLoadId As String = "", Optional ByVal pOnContainer As Boolean = False,
                                                            Optional ByVal Simulation As Integer = 0) As Boolean
        Dim dt As New DataTable
        Dim sql As String
        Dim repltype As String
        Dim ld As Load
        If pOnContainer Then
            repltype = ReplenishmentTypes.FullReplenishment
        Else
            repltype = PolicyDetail("REPLTYPE")
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Replenishment type used:{0}", repltype))
        End If


        Dim AllocatedQty As Double = 0

        Dim pReplPolicy As String

        'started for PWMS-756
        pReplPolicy = zb.ReplPolicy
        'Ended for PWMS-756

        Select Case repltype
            Case ReplenishmentTypes.FullReplenishment
                ld = getFullReplLoadInRegion(PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                If ld Is Nothing Then
                    If Not oLogger Is Nothing Then oLogger.Write("Could not find load for full replenishment")
                    Return False
                Else
                    If Not oLogger Is Nothing Then oLogger.Write("Found load for full replenishment. Load id: " + ld.LOADID)
                    CreateReplTask(ld, zb, PolicyDetail, pReplMethod, ld.UNITS, Nothing, oLogger, pOnContainer)
                    CurrentQty = CurrentQty + ld.UNITS
                    If Simulation Then Return False
                    Return True
                End If
            Case ReplenishmentTypes.NegativeReplenishment, ReplenishmentTypes.PartialReplenishment
                ld = getPartialReplLoadInRegionReportProblem(PolicyDetail, pReplMethod, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, AllocatedQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                If Not ld Is Nothing AndAlso AllocatedQty > 0 Then
                    Return True
                Else
                    If Not oLogger Is Nothing Then oLogger.Write("Allocated qty = " + AllocatedQty.ToString + " or Could not find load for " + repltype + " replenishment")
                    Return False
                End If
            Case Else
                ld = getFullReplLoadInRegion(PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                If Not ld Is Nothing Then
                    If Not oLogger Is Nothing Then oLogger.Write(String.Format("Found load for {0}. Load id: {1} ", repltype, ld.LOADID))
                    CreateReplTask(ld, zb, PolicyDetail, pReplMethod, ld.UNITS, Nothing, oLogger, pOnContainer)
                    CurrentQty = CurrentQty + ld.UNITS
                    If Simulation Then Return False
                    Return True
                Else
                    ld = getPartialReplLoadInRegion(PolicyDetail, pReplMethod, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy,
                                                    zb, CurrentQty, AllocatedQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                    If Not ld Is Nothing AndAlso AllocatedQty > 0 Then
                        'CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, PolicyDetail("UOM"), oLogger, pOnContainer)
                        CurrentQty = CurrentQty + AllocatedQty
                        If Simulation Then Return False
                        Return True
                    Else
                        If Not oLogger Is Nothing Then oLogger.Write("Allocated qty = " + AllocatedQty.ToString + " or Could not find load for " + repltype + " replenishment")
                        Return False
                    End If
                End If
        End Select
    End Function
    'Added for RWMS-2598 End
    'RWMS-2628 - passing parameter for Opportunity replen
    Protected Function CreateReplTasksInRegion(ByVal PolicyDetail As DataRow, ByVal zb As PickLoc, ByRef CurrentQty As Double, ByVal pReplMethod As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pOnContainer As Boolean = False, Optional ByVal Simulation As Integer = 0, Optional ByVal OpportunityReplen As Boolean = False) As Boolean
        Dim dt As New DataTable
        Dim sql As String
        Dim repltype As String
        Dim ld As Load
        If pOnContainer Then
            repltype = ReplenishmentTypes.FullReplenishment
        Else
            repltype = PolicyDetail("REPLTYPE")
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Replenishment type used:{0}", repltype))
        End If

        Dim AllocatedQty As Double = 0

        Dim pReplPolicy As String

        'CAD- Jira 241
        'If pReplMethod = ReplenishmentMethods.NormalReplenishment Or pReplMethod = ReplenishmentMethods.ManualReplenishment Then
        '    pReplPolicy = zb.NormalReplPolicy
        'Else
        '    pReplPolicy = zb.HotReplPolicy
        'End If

        'started for PWMS-756
        pReplPolicy = zb.ReplPolicy
        'Ended for PWMS-756

        ''If we are at opportunity replenishment lets verify that the original location of the load consist with the policy from pick region
        'If pLoadId <> "" Then
        '    sql = String.Format("select count(1) from invload inner join location on invload.location = location.location and location.pickregion = '{0}' and invload.loadid = '{1}'", PolicyDetail("PICKREGION"), pLoadId)
        '    If Not Convert.ToBoolean(DataInterface.ExecuteScalar(sql)) Then
        '        Return False
        '    End If
        'End If

        Select Case repltype
            Case ReplenishmentTypes.FullReplenishment
                ld = getFullReplLoadInRegion(PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                If ld Is Nothing Then
                    If Not oLogger Is Nothing Then oLogger.Write("Could not find load for full replenishment")
                    Return False
                Else
                    If Not oLogger Is Nothing Then oLogger.Write("Found load for full replenishment. Load id: " + ld.LOADID)
                    CreateReplTask(ld, zb, PolicyDetail, pReplMethod, ld.UNITS, Nothing, oLogger, pOnContainer)
                    CurrentQty = CurrentQty + ld.UNITS
                    If Simulation Then Return False
                    Return True
                End If
            Case ReplenishmentTypes.NegativeReplenishment, ReplenishmentTypes.PartialReplenishment
                'RWMS-2628 - passing parameter for Opportunity replen
                ld = getPartialReplLoadInRegion(PolicyDetail, pReplMethod, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, AllocatedQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"), Simulation, OpportunityReplen)
                If Not ld Is Nothing AndAlso AllocatedQty > 0 Then
                    'CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, PolicyDetail("UOM"), oLogger, pOnContainer)
                    ' CurrentQty = CurrentQty + AllocatedQty
                    Return True
                Else
                    If Not oLogger Is Nothing Then oLogger.Write("Allocated qty = " + AllocatedQty.ToString + " or Could not find load for " + repltype + " replenishment")
                    Return False
                End If
            Case Else
                ld = getFullReplLoadInRegion(PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                If Not ld Is Nothing Then
                    If Not oLogger Is Nothing Then oLogger.Write(String.Format("Found load for {0}. Load id: {1} ", repltype, ld.LOADID))
                    CreateReplTask(ld, zb, PolicyDetail, pReplMethod, ld.UNITS, Nothing, oLogger, pOnContainer)
                    CurrentQty = CurrentQty + ld.UNITS
                    If Simulation Then Return False
                    Return True
                Else
                    ld = getPartialReplLoadInRegion(PolicyDetail, pReplMethod, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pReplPolicy, zb, CurrentQty, AllocatedQty, oLogger, pLoadId, PolicyDetail("ALLOCUOMQTY"))
                    If Not ld Is Nothing AndAlso AllocatedQty > 0 Then
                        'CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, PolicyDetail("UOM"), oLogger, pOnContainer)
                        CurrentQty = CurrentQty + AllocatedQty
                        If Simulation Then Return False
                        Return True
                    Else
                        If Not oLogger Is Nothing Then oLogger.Write("Allocated qty = " + AllocatedQty.ToString + " or Could not find load for " + repltype + " replenishment")
                        Return False
                    End If
                End If
        End Select
    End Function

    Protected Function CreateReplTask(ByVal ld As Load, ByVal zb As PickLoc, ByVal PolicyDetail As DataRow, ByVal ReplMethod As String, ByVal UnitsAllocated As Double, Optional ByVal ReplUom As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal OnContainer As Boolean = False, Optional ByVal Simulation As Integer = 0) As Boolean
        If Simulation = 1 Then Return True
        Dim Repl As New Replenishment
        Dim pUser As String = WMS.Lib.USERS.SYSTEMUSER
        If Not oLogger Is Nothing Then
            oLogger.Write("Creating replenishment task")
            oLogger.writeSeperator(" ", 20)
        End If
        Repl.FromLoad = ld.LOADID
        Repl.FromLocation = ld.LOCATION
        Repl.FromWarehousearea = ld.WAREHOUSEAREA
        Repl.ReplMethod = ReplMethod
        If ld.UNITS = UnitsAllocated AndAlso PolicyDetail("REPLTYPE") = ReplenishmentTypes.FullReplenishment Then
            Repl.ReplType = ReplenishmentTypes.FullReplenishment
            Repl.ToLoad = ld.LOADID
            Repl.UOM = ld.LOADUOM
        Else
            Repl.ReplType = PolicyDetail("REPLTYPE")
            Repl.ToLoad = ""
            Repl.UOM = ReplUom
        End If
        Repl.ToLocation = zb.Location
        Repl.ToWarehousearea = zb.Warehousearea

        'Commented for RWMS-2438 Start
        'Repl.Units = UnitsAllocated
        'Commented for RWMS-2438 End

        'Added for RWMS-2438 Start
        Repl.Units = ld.UNITS
        'Added for RWMS-2438 End

        Repl.STRATEGYID = PolicyDetail("POLICYID")
        Repl.REPLDATETIME = _dueDate

        '_taskPriority = PolicyDetail("TASKPRIORITY")


        If Not oLogger Is Nothing Then
            oLogger.Write("From LoadId " & Repl.FromLoad & " with qty " & UnitsAllocated &
                " in Location " & Repl.FromLocation &
                " in Warehousearea " & Repl.FromWarehousearea &
                " with replenishment methods :" & Repl.ReplMethod &
                " with taskpriority " & _taskPriority)
            oLogger.Write("To LoadId " & Repl.ToLoad & " with uom " & Repl.UOM)
        End If
        If ld.SetReplenishmentJob(Repl, pUser, oLogger) Then
            Repl.Post(pUser, oLogger)

            ' RWMS-2604
            ' Find the created replen id
            Try
                Dim sql As String = String.Format("select top(1) REPLID from replenishment where Fromload = '{0}' and Fromlocation = '{1}' and FROMWAREHOUSEAREA = '{2}' and REPLMETHOD = '{3}' and TOLOCATION = '{4}' and TOWAREHOUSEAREA = '{5}' and STRATEGYID = '{6}' And Units = {7} and Status = 'PLANNED'",
                                    Repl.FromLoad,
                                    Repl.FromLocation,
                                    Repl.FromWarehousearea,
                                    Repl.ReplMethod,
                                    Repl.ToLocation,
                                    Repl.ToWarehousearea,
                                    Repl.STRATEGYID,
                                    Repl.Units)
                Dim replid As String = DataInterface.ExecuteScalar(sql)
                If Not String.IsNullOrEmpty(replid) Then
                    If Me._estimateDueDateOnCreateType = DueDateEstimateType.OrderPlanner Then
                        If Not String.IsNullOrEmpty(Me._estimateDueDateOnCreateOrderId) Then
                            ReplenishmentDueDate.EstimateDueDateOnCreate(Me._estimateDueDateOnCreateOrderId, replid, oLogger)
                        End If
                    ElseIf Me._estimateDueDateOnCreateType = DueDateEstimateType.Normal Then
                        ReplenishmentDueDate.EstimateDueDate(replid, oLogger)
                    End If
                End If
            Catch ex As Exception
                oLogger.Write("Error while estimating due date on create of replen task. Exception : " + ex.StackTrace)
            End Try


            ' RWMS-2604


            If PolicyDetail("CREATETASK") And Not OnContainer Then
                Dim tm As New TaskManager
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("PolicyDetail('CREATETASK') - _taskPriority={0}", _taskPriority))
                End If
                tm.CreateReplenishmentTask(Repl, ld, _taskPriority, PolicyDetail("TASKSUBTYPE"))
                'Added for RWMS-1867 Start
                If (Repl.ReplMethod = ReplenishmentMethods.ManualReplenishment) Then
                    'If Not oLogger Is Nothing Then
                    '    oLogger.Write("Assign user new logic----" & Repl.ReplMethod)
                    'End If
                    UpdateprioritizationforImmediate(ReplMethod, zb, oLogger)
                    Dim strTask As String = AssignedToTask(Repl.ReplId)

                    If strTask <> "" Then
                        Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                        oTask.AssignUserManualReplenish(USERID)
                        oLogger.SafeWrite("Task Found- assigned user:" & USERID)
                    Else
                        oLogger.SafeWrite("No Available Task Found")
                    End If

                End If
                'Added for RWMS-1867 End
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write("Replenishment task created")
                oLogger.writeSeperator()
            End If
            Return True
        Else
            Return False
        End If
    End Function

#Region "Full & Partial replenishment load selecting procedures"

    Public Function getFullReplLoadInRegion(ByVal PickRegion As String, ByVal pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByVal CurrentQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "") As Load

        If WMS.Logic.PickLoc.IsBatchPickLocation(zb.Consignee, zb.Location, oLogger) Then
            Return Nothing
        End If

        If (Not String.IsNullOrEmpty(zb.PARENTSKU)) Then
            If oLogger IsNot Nothing Then oLogger.Write("getFullReplLoadInRegion() is using the parent sku for vReplenishmentInventory SKU {0}, PARENTSKU {1}", zb.SKU, zb.PARENTSKU)
            zb.SKU = zb.PARENTSKU
        End If

        Dim sql As String
        Dim dt As New DataTable
        'Get Loads for scoring procedure
        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE <= {2} AND LOADID like '%{6}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {5} ", zb.Consignee, zb.SKU, zb.MaximumReplQty - CurrentQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE <= {2} AND LOADID like '%{5}' ", zb.Consignee, zb.SKU, zb.MaximumReplQty - CurrentQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If
        Dim FullUomSize As Decimal
        If pAllocUOMQty <> "" Then
            Dim oSku As New SKU(zb.Consignee, zb.SKU)
            FullUomSize = oSku.ConvertToUnits(pAllocUOMQty)
            sql = sql & " and UNITSAVAILABLE = " & FullUomSize
        End If
        sql = sql & " ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC"
        DataInterface.FillDataset(sql, dt)

        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to get loads according to query:")
            oLogger.Write(sql.ToString)
        End If

        If dt.Rows.Count > 0 Then
            ' Loads found for scoring
            Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
            Dim arrLoadsToCheck As DataRow()
            arrLoadsToCheck = dt.Select()
            'Run scoring procedure
            rps.Score(arrLoadsToCheck, oLogger)
            'if there is more than one load then return the best load from scoring run
            Dim ld As New Load(Convert.ToString(arrLoadsToCheck(0)("LOADID")))
            Return ld
        Else
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND.")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        End If
    End Function

    Public Function getPartialReplLoadInRegionReportProblem(ByVal PolicyDetail As DataRow, ByVal pReplMethod As String, ByVal PickRegion As String, ByRef pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByRef CurrentQty As Double, ByRef AllocatedQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "", Optional ByVal pSimulation As Integer = 0) As Load

        If (Not String.IsNullOrEmpty(zb.PARENTSKU)) Then
            If oLogger IsNot Nothing Then oLogger.Write("getPartialReplLoadInRegionReportProblem() is using the parent sku for vReplenishmentInventory SKU {0}, PARENTSKU {1}", zb.SKU, zb.PARENTSKU)
            zb.SKU = zb.PARENTSKU
        End If

        Dim sql As String
        Dim dt As New DataTable
        Dim dr As DataRow
        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND LOADID like '%{5}' AND {4} ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else

            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{4}' ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If
        If PolicyDetail("REPLTYPE").ToString.Equals(WMS.Logic.Replenishment.ReplenishmentTypes.NegativeReplenishment, StringComparison.OrdinalIgnoreCase) Then
            sql = sql & " and UNITSALLOCATED = 0"
        End If

        Dim FullUomSize As Decimal
        Dim oSku As New SKU(zb.Consignee, zb.SKU)
        If pAllocUOMQty <> "" Then
            FullUomSize = oSku.ConvertToUnits(pAllocUOMQty)
            sql = sql & " and UNITSAVAILABLE >= " & FullUomSize
        End If
        sql = sql & " ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC"
        If Not oLogger Is Nothing Then oLogger.Write("Selecting loads" & vbNewLine & sql)
        DataInterface.FillDataset(sql, dt)
        ' Loads found for scoring
        Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
        Dim arrLoadsToCheck As DataRow()
        arrLoadsToCheck = dt.Select()
        'Run scoring procedure
        If Not oLogger Is Nothing Then
            oLogger.Write("Filling loads table for scoring procedure, found " & arrLoadsToCheck.Length)
        End If
        rps.Score(arrLoadsToCheck, oLogger)
        'if there is more than one load then return the best load from scoring run
        If dt.Rows.Count > 0 Then

            ' for each load in after scoring procedure select from the best available
            If Not oLogger Is Nothing Then
                oLogger.Write("Running on loads array after scoring.")
            End If
            ' If  replenishment than check the quntity in location and
            ' if  maximum is less than quntity in location take quantity in location
            ' otherwise take Maximum qty as limit

            Dim UpperQtyLimit As Decimal

            'Started new Logic for PWMS-756
            UpperQtyLimit = zb.MaximumReplQty
            Dim isAnyFound As Boolean = False
            Dim foundLoad As Load = Nothing

            For Each dr In arrLoadsToCheck 'dt.Rows
                Dim ld As New Load(Convert.ToString(dr("LOADID")))

                If Not oLogger Is Nothing Then oLogger.Write(String.Format("Checking Load {0}.", ld.LOADID))
                Dim CheckedUom As String
                CheckedUom = ld.LOADUOM
                Dim unitsperuom As Double = oSku.ConvertToUnits(CheckedUom)

                'If unitsperuom <= UpperQtyLimit - CurrentQty Then
                Dim uomunits As Double '= oSku.ConvertUnitsToUom(CheckedUom, UpperQtyLimit - CurrentQty)
                'If UpperQtyLimit - CurrentQty < ld.UNITS - ld.UNITSALLOCATED Then
                '    uomunits = oSku.ConvertUnitsToUom(CheckedUom, UpperQtyLimit - CurrentQty)
                'Else
                uomunits = oSku.ConvertUnitsToUom(CheckedUom, ld.UNITS - ld.UNITSALLOCATED)
                'End If

                If Not oLogger Is Nothing Then oLogger.Write(String.Format("UOM {0}.", ld.LOADID))

                _taskPriority = PolicyDetail("TASKPRIORITY")

                ' RWMS-2403 Begin
                'uomunits = ld.UNITS - ld.UNITSALLOCATED
                If pUom Is Nothing Or pUom = "" Or pUom = "%" Then
                    CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, CheckedUom, oLogger)
                Else
                    CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, pUom, oLogger)
                End If
                ' RWMS-2403 End
                AllocatedQty = (uomunits) * unitsperuom
                If Not oLogger Is Nothing Then oLogger.Write("uomunits :" & (ld.UNITS - ld.UNITSALLOCATED))
                If Not oLogger Is Nothing Then oLogger.Write("unitsperuom:" & unitsperuom)
                If Not oLogger Is Nothing Then oLogger.Write("allocated qty_2= uomunits * unitsperuom : " & AllocatedQty)
                CurrentQty = CurrentQty + AllocatedQty
                AllocatedQty = 0

                Dim masterDao As MasterDao = New MasterDao()
                Dim strPriorityPending As String = masterDao.GetPriorityCodeForTask(WMS.Lib.TASKPRIORITY.PRIORITY_PENDING)
                Dim sql1 As String = String.Format("UPDATE TASKS SET PRIORITY = '{0}' WHERE TASK = (SELECT TOP 1  TASK FROM TASKS  WHERE CONSIGNEE = '{1}' AND SKU = '{2}' AND STATUS='AVAILABLE' AND  REPLENISHMENT IS NOT NULL AND REPLENISHMENT<>'' ORDER BY REPLENISHMENT DESC)", strPriorityPending, zb.Consignee, zb.SKU)
                DataInterface.RunSQL(sql1)

                'Updateprioritization(pReplMethod, zb)
                'UpdateprioritizationforImmediate(pReplMethod, zb)
                isAnyFound = True
                foundLoad = ld
                Exit For
                'End If
            Next

            ' No loads found for scoring
            If isAnyFound = True Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Found load : " & foundLoad.LOADID)
                    oLogger.writeSeperator(" ", 20)
                End If
                Return foundLoad
            Else
                ' No loads found for scoring
                If Not oLogger Is Nothing Then
                    oLogger.Write("NO MATCHING LOADS FOUND.")
                    oLogger.writeSeperator(" ", 20)
                End If
                Return Nothing
            End If
        Else
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND[BEFORE SCORING PROCEDURE, SQL RESULT 0 ROWS].")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        End If

    End Function
    'Added for RWMS-2598 End

    'Added for RWMS-1867 Start
    'RWMS-2628 - passing the optional parameter for opportunity replen
    Public Function getPartialReplLoadInRegion(ByVal PolicyDetail As DataRow, ByVal pReplMethod As String, ByVal PickRegion As String, ByRef pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByRef CurrentQty As Double, ByRef AllocatedQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "", Optional ByVal pSimulation As Integer = 0, Optional ByVal OpportunityReplen As Boolean = False) As Load

        If WMS.Logic.PickLoc.IsBatchPickLocation(zb.Consignee, zb.Location, oLogger) Then
            Return Nothing
        End If

        Dim sql As String
        Dim dt As New DataTable
        Dim dr As DataRow

        Dim PendingQty As Double
        Dim objpriorityreset As Prioritize = New PrioritizeReplenishments
        Dim pendingpriority As Integer = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_PENDING))

        If pReplMethod <> ReplenishmentMethods.ManualReplenishment AndAlso (CurrentQty > zb.ReplQty) Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Calculated Quantity is ABOVE Replen point.Replenishment will not be created.[CurrentQty+PendingQty-AllocatedQty({0}) > ReplQty({1})]", CurrentQty, zb.ReplQty))
            End If
            Return Nothing
        End If

        If (Not String.IsNullOrEmpty(zb.PARENTSKU)) Then
            If oLogger IsNot Nothing Then oLogger.Write("getPartialReplLoadInRegion() is using the parent sku for vReplenishmentInventory SKU {0}, PARENTSKU {1}", zb.SKU, zb.PARENTSKU)
            zb.SKU = zb.PARENTSKU
        End If

        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND LOADID like '%{5}' AND {4} ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{4}' ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If
        If PolicyDetail("REPLTYPE").ToString.Equals(WMS.Logic.Replenishment.ReplenishmentTypes.NegativeReplenishment, StringComparison.OrdinalIgnoreCase) Then
            sql = sql & " and UNITSALLOCATED = 0"
        End If

        Dim FullUomSize As Decimal
        Dim oSku As New SKU(zb.Consignee, zb.SKU)
        If pAllocUOMQty <> "" Then
            FullUomSize = oSku.ConvertToUnits(pAllocUOMQty)
            sql = sql & " and UNITSAVAILABLE >= " & FullUomSize
        End If
        sql = sql & " ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC"
        If Not oLogger Is Nothing Then oLogger.Write("Selecting loads" & vbNewLine & sql)
        DataInterface.FillDataset(sql, dt)
        ' Loads found for scoring
        Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
        Dim arrLoadsToCheck As DataRow()
        arrLoadsToCheck = dt.Select()
        'Run scoring procedure
        If Not oLogger Is Nothing Then
            oLogger.Write("Filling loads table for scoring procedure, found " & arrLoadsToCheck.Length)
        End If
        rps.Score(arrLoadsToCheck, oLogger)
        'if there is more than one load then return the best load from scoring run
        If dt.Rows.Count > 0 Then

            ' for each load in after scoring procedure select from the best available
            If Not oLogger Is Nothing Then
                oLogger.Write("Running on loads array after scoring.")
            End If
            ' If  replenishment than check the quntity in location and
            ' if  maximum is less than quntity in location take quantity in location
            ' otherwise take Maximum qty as limit

            Dim UpperQtyLimit As Decimal

            'Started new Logic for PWMS-756
            UpperQtyLimit = zb.MaximumReplQty
            Dim isAnyFound As Boolean = False
            Dim foundLoad As Load = Nothing

            For Each dr In arrLoadsToCheck 'dt.Rows
                If pReplMethod = ReplenishmentMethods.ManualReplenishment Then
                    Dim taskid As String = String.Empty
                    If Not ReplenishmentExists(zb.SKU, zb.Location, zb.Warehousearea, zb.Consignee, taskid) Then
                        Dim ld As New Load(Convert.ToString(dr("LOADID")))

                        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Checking Load {0}.", ld.LOADID))
                        Dim CheckedUom As String
                        CheckedUom = ld.LOADUOM
                        Dim unitsperuom As Double = oSku.ConvertToUnits(CheckedUom)

                        'If unitsperuom <= UpperQtyLimit - CurrentQty Then
                        Dim uomunits As Double '= oSku.ConvertUnitsToUom(CheckedUom, UpperQtyLimit - CurrentQty)
                        'If UpperQtyLimit - CurrentQty < ld.UNITS - ld.UNITSALLOCATED Then
                        '    uomunits = oSku.ConvertUnitsToUom(CheckedUom, UpperQtyLimit - CurrentQty)
                        'Else
                        uomunits = oSku.ConvertUnitsToUom(CheckedUom, ld.UNITS - ld.UNITSALLOCATED)
                        'End If

                        If Not oLogger Is Nothing Then oLogger.Write(String.Format("UOM {0}.", ld.LOADID))

                        _taskPriority = pendingpriority

                        ' RWMS-2403 Begin
                        'uomunits = ld.UNITS - ld.UNITSALLOCATED
                        If pUom Is Nothing Or pUom = "" Or pUom = "%" Then
                            CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, CheckedUom, oLogger)
                        Else
                            CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, pUom, oLogger)
                        End If
                        ' RWMS-2403 End
                        AllocatedQty = (uomunits) * unitsperuom
                        If Not oLogger Is Nothing Then oLogger.Write(String.Format("uomunits={0},unitsperuom={1},allocated qty_2= uomunits * unitsperuom={2}:", (ld.UNITS - ld.UNITSALLOCATED), unitsperuom, AllocatedQty))
                        CurrentQty = CurrentQty + AllocatedQty
                        AllocatedQty = 0
                        'Updateprioritization(pReplMethod, zb)
                        'UpdateprioritizationforImmediate(pReplMethod, zb)
                        isAnyFound = True
                        foundLoad = ld
                        Exit For
                    Else
                        Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(taskid)
                        oTask.AssignUserManualReplenish(USERID)
                    End If
                Else
                    Dim ld As New Load(Convert.ToString(dr("LOADID")))

                    If Not oLogger Is Nothing Then oLogger.Write(String.Format("Checking Load {0}.", ld.LOADID))
                    Dim CheckedUom As String


                    'If pUom Is Nothing Or pUom = "" Or pUom = "%" Then
                    CheckedUom = ld.LOADUOM

                    Dim unitsperuom As Double = oSku.ConvertToUnits(CheckedUom)
                    If Not oLogger Is Nothing Then
                        oLogger.Write(String.Format("unitsperuom of LOADUOM ={0},UpperQtyLimit={1},Calculated Qty={2}.", unitsperuom.ToString(), UpperQtyLimit.ToString(), CurrentQty.ToString()))
                    End If

                    If unitsperuom <= UpperQtyLimit - CurrentQty Then
                        Dim uomunits As Double '= oSku.ConvertUnitsToUom(CheckedUom, UpperQtyLimit - CurrentQty)
                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Stepped In: unitsperuom <= UpperQtyLimit - CurrentQty"))
                        End If
                        uomunits = oSku.ConvertUnitsToUom(CheckedUom, ld.UNITS - ld.UNITSALLOCATED)

                        If Not oLogger Is Nothing Then oLogger.Write(String.Format("UOM={0},uomunits={1}.", ld.LOADID, uomunits))

                        'If ld.UNITS - ld.UNITSALLOCATED - AllocatedQty <= 0 Then
                        '    If Not oLogger Is Nothing Then oLogger.Write(String.Format("Load does not fit. Not enough units."))
                        '    'Exit While
                        'End If
                        'Commented for RWMS-22249 Start
                        'If (CurrentQty + ld.UNITS - ld.UNITSALLOCATED < zb.MaximumReplQty) Then
                        'Commented for RWMS-22249 End

                        ' RWMS-2315 : Removed the check as Rick suggested -> Rolled Back, since removing this condition reslted in creation of multiple replenishment tasks.
                        'Added for RWMS-2249 Start

                        'Org condition:(CurrentQty + ld.UNITS - ld.UNITSALLOCATED <= zb.MaximumReplQty) '
                        If (CurrentQty < zb.ReplQty) Then
                            'Added for RWMS-2249 End
                            ' RWMS-2315 : Removed the check as Rick suggested -> Rolled Back, since removing this condition reslted in creation of multiple replenishment tasks.
                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("Calcualted Quantity is BELOW Replen point.[CurrentQty+PendingQty-AllocatedQty({0}) < ReplQty({1})]", CurrentQty, zb.ReplQty))
                            End If


                            '_taskPriority = PolicyDetail("TASKPRIORITY")
                            'Determining the task priority for new replen task
                            'Calculating the exisitng replens qty with task priority > PENDING priority
                            'Dim sqlpendingqty As String = String.Format("select isnull(sum(r.units),0) from REPLENISHMENT r inner join TASKS t on r.REPLID=t.REPLENISHMENT where t.PRIORITY>{0} and r.status not in ('COMPLETE','CANCELED') and t.TOLOCATION='{1}'", pendingpriority, zb.Location)
                            'PendingQty = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlpendingqty)
                            'uomunits = ld.UNITS - ld.UNITSALLOCATED

                            'If Not oLogger Is Nothing Then
                            '    oLogger.Write(String.Format("LoadID={0}, Calcuated Qty={1}, Existing Replens Qty(Priority > PENDING)={2}, Load units ={3}, Load units UNITSALLOCATED={4},ReplQty={5}, MaximumReplQty={6}.", ld.LOADID, CurrentQty, PendingQty.ToString(), ld.UNITS, ld.UNITSALLOCATED, zb.ReplQty, zb.MaximumReplQty))
                            'End If

                            'If (zb.CurrentQty + PendingQty < zb.ReplQty) Or ((zb.CurrentQty + PendingQty + uomunits) < zb.MaximumReplQty) Then

                            '    _taskPriority = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_NORMAL))
                            '    If Not oLogger Is Nothing Then
                            '        oLogger.Write(String.Format("One of the following 2 conditions is TRUE. Assigning the task priority to NORMAL"))
                            '        oLogger.Write(String.Format("[CurrentQty + Existing Replens Qty(Priority > PENDING):({0}) < ReplQty:({1})]", zb.CurrentQty + PendingQty, zb.ReplQty))
                            '        oLogger.Write(String.Format("[CurrentQty + Existing Replens Qty(Priority > PENDING) + Load available units:({0}) < MaximumReplQty:({1})]", zb.CurrentQty + PendingQty + uomunits, zb.MaximumReplQty))
                            '    End If
                            'Else

                            '    _taskPriority = pendingpriority
                            '    If Not oLogger Is Nothing Then
                            '        oLogger.Write(String.Format("Both of the following 2 conditions FAILED. Assigning the task priority to PENDING"))
                            '        oLogger.Write(String.Format("[CurrentQty + Existing Replens Qty(Priority > PENDING):({0}) < ReplQty:({1})]", zb.CurrentQty + PendingQty, zb.ReplQty))
                            '        oLogger.Write(String.Format("[CurrentQty + Existing Replens Qty(Priority > PENDING) + Load available units:({0}) < MaximumReplQty:({1})]", zb.CurrentQty + PendingQty + uomunits, zb.MaximumReplQty))
                            '    End If
                            'End If


                            ' RWMS-2403 Begin
                            _taskPriority = pendingpriority
                            If pUom Is Nothing Or pUom = "" Or pUom = "%" Then
                                If Not CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, CheckedUom, oLogger) Then
                                    Continue For
                                End If
                            Else
                                If Not CreateReplTask(ld, zb, PolicyDetail, pReplMethod, AllocatedQty, pUom, oLogger) Then
                                    Continue For
                                End If
                            End If
                            ' RWMS-2403 End
                            AllocatedQty = uomunits * unitsperuom
                            If Not oLogger Is Nothing Then oLogger.Write(String.Format("Load Allocated. LoadID={0},Allocated Qty(uomunits * unitsperuom) ={1}", ld.LOADID, AllocatedQty))
                            CurrentQty = CurrentQty + AllocatedQty
                            'Updateprioritization(pReplMethod, zb, oLogger)
                            isAnyFound = True
                            foundLoad = ld
                        Else

                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("No more replenishments required. Calcualted quantity is fullfilled.No other loads to go through. [Calculated Qty[CurrentQty + PendingQty - AllocatedQty]({0}) > zb.ReplQty({1})]", CurrentQty, zb.ReplQty))
                            End If

                            Exit For
                        End If
                    End If
                End If
            Next



            'Ended  new Logic for PWMS-756
            '---------------------------------------------------------------------------------------------------------------

            ' No loads found for scoring
            If isAnyFound = True Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Found load : " & foundLoad.LOADID)
                    oLogger.writeSeperator(" ", 20)
                End If
                Return foundLoad
            Else
                ' No loads found for scoring
                If Not oLogger Is Nothing Then
                    oLogger.Write("NO MATCHING LOADS FOUND.")
                    oLogger.writeSeperator(" ", 20)
                End If
                Return Nothing
            End If
        Else
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND[BEFORE SCORING PROCEDURE, SQL RESULT 0 ROWS].")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        End If

    End Function
    'Added for RWMS-1867 End

    ' Added new Method for PWMS-860 for short Pick
    ' RWMS-2315, rectified the code to atleast create one task if loads are available.
    Public Function getUserPickShort(ByVal PolicyDetail As DataRow, ByVal pReplMethod As String, ByVal PickRegion As String, ByRef pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByVal CurrentQty As Double, ByRef AllocatedQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "", Optional ByVal pSimulation As Integer = 0) As Load

        If (Not String.IsNullOrEmpty(zb.PARENTSKU)) Then
            If oLogger IsNot Nothing Then oLogger.Write("getUserPickShort() is using the parent sku for vReplenishmentInventory SKU {0}, PARENTSKU {1}", zb.SKU, zb.PARENTSKU)
            zb.SKU = zb.PARENTSKU
        End If

        Dim sql As String
        Dim dt As New DataTable
        'Dim dr As DataRow
        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND LOADID like '%{5}' AND {4} ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else

            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{4}' ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If
        If Not oLogger Is Nothing Then oLogger.Write(sql)
        DataInterface.FillDataset(sql, dt)
        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Query returened {0} loads.", dt.Rows.Count))

        Dim existingReplCount As Integer = DataInterface.ExecuteScalar(String.Format("select count(*) as REPLCOUNT from REPLENISHMENT where TOLOCATION = '{0}' and STATUS not in ('CANCELED','COMPLETE')", GetFlowRackLocation(zb.Location, oLogger)))

        If dt.Rows.Count > 0 Then

            If existingReplCount = 0 Then

                Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
                Dim arrLoadsToCheck As DataRow()
                arrLoadsToCheck = dt.Select()
                'Run scoring procedure
                If Not oLogger Is Nothing Then
                    oLogger.Write("Filling loads table for scoring procedure, found " & arrLoadsToCheck.Length)
                End If
                rps.Score(arrLoadsToCheck, oLogger)

                Dim dr As DataRow = arrLoadsToCheck(0)

                Dim ld As New Load(Convert.ToString(dr("LoadID")))

                If Not oLogger Is Nothing Then oLogger.Write(String.Format("Checking Load {0}.", ld.LOADID))
                Dim CheckedUom As String
                CheckedUom = ld.LOADUOM

                If Not oLogger Is Nothing Then oLogger.Write(String.Format("UOM {0}.", CheckedUom))
                ' No need to check any condition, the first loads should be used to create a single replenishment task.
                CreateReplTask(ld, zb, PolicyDetail, pReplMethod, 0, pUom, oLogger)
                UpdateprioritizationforImmediate(pReplMethod, zb, oLogger)
            Else
                If Not oLogger Is Nothing Then oLogger.Write(String.Format("No Replenishment created since there exists already {0} replenishment task(s)", existingReplCount))
            End If
        Else
            If Not oLogger Is Nothing Then oLogger.Write(String.Format("No Matching Loads found for SKU : {0} for replenishment task creation", zb.SKU))
        End If

    End Function
    ' Ended new Method for PWMS-860 for short Pick
#End Region

#End Region

#Region "Opportunity Replenishment"

    Public Function OpportunityReplenish(ByRef sLocation As String, ByRef sWarehousearea As String, ByVal pLoadId As String, ByVal pOnContainer As Boolean, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing) As String
        PickLocOpportunityReplenish(sLocation, sWarehousearea, pLoadId, pOnContainer, pUser, oLogger)
        If sLocation = "" Then
            sLocation = ZoneOpportunityReplenish(sLocation, sWarehousearea, pLoadId, pOnContainer, pUser, oLogger)
        End If
        Return sLocation
    End Function

    Private Function PickLocOpportunityReplenish(ByRef sLocation As String, ByRef sWarehousearea As String, ByVal pLoadId As String, ByVal pOnContainer As Boolean, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing) As String
        Dim dtPickLocs As New DataTable
        Dim sqlPickLocs As String
        Dim oLoad As New WMS.Logic.Load(pLoadId)
        Dim oSku As New SKU(oLoad.CONSIGNEE, oLoad.SKU)
        If Not oSku.OPORTUNITYRELPFLAG Then
            Return String.Empty
        End If
        'sqlPickLocs = String.Format("SELECT distinct vPICKLOC.Location, vPICKLOC.WAREHOUSEAREA FROM vPICKLOC INNER JOIN invload ON invload.CONSIGNEE = vPICKLOC.CONSIGNEE AND invload.sku = vPICKLOC.sku inner join location on location.location = vpickloc.location and location.warehousearea = vPICKLOC.warehousearea and location.status = 1 WHERE vPICKLOC.CONSIGNEE = '{0}' AND vPICKLOC.sku LIKE '{1}%'", oLoad.CONSIGNEE, oLoad.SKU)
        sqlPickLocs = String.Format("SELECT distinct vPICKLOC.Location, vPICKLOC.WAREHOUSEAREA FROM vPICKLOC INNER JOIN invload ON invload.CONSIGNEE = vPICKLOC.CONSIGNEE AND invload.sku = vPICKLOC.sku inner join location on location.location = vpickloc.location and location.warehousearea = vPICKLOC.warehousearea and isnull(location.status,0) = 1 and isnull(location.problemflag,0) = 0 WHERE vPICKLOC.CONSIGNEE = '{0}' AND vPICKLOC.sku LIKE '{1}%' AND (vPICKLOC.BATCHPICKLOCATION is null or vPICKLOC.BATCHPICKLOCATION = '0')", oLoad.CONSIGNEE, oLoad.SKU)
        DataInterface.FillDataset(sqlPickLocs, dtPickLocs)
        Dim found As Boolean = False
        For Each dr As DataRow In dtPickLocs.Rows
            sLocation = dr("location")
            sWarehousearea = dr("WAREHOUSEAREA")
            Dim zb As New PickLoc(sLocation, sWarehousearea, oLoad.CONSIGNEE, oLoad.SKU)
            Dim sql As String
            ' Get policy details , if there is same priority then group them by this priority
            Dim pReplPolicy As String = zb.ReplPolicy
            Dim prTbl As DataTable = New DataTable
            DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY", prTbl)
            Dim dt As New DataTable
            dt = CreatePolicyDetailTable()
            For Each pr As DataRow In prTbl.Rows
                dt.Rows.Add(CreatePolicyDetailRow(dt, pReplPolicy, pr("PRIORITY")))
            Next
            found = CreateOpportunityReplTasks(dt, zb, pLoadId, pOnContainer, oLogger)
            If Not found Then
                sLocation = String.Empty
                sWarehousearea = String.Empty
            Else
                Return String.Empty 'check with udi
            End If
        Next
        Return String.Empty
    End Function

    Private Function ZoneOpportunityReplenish(ByRef sLocation As String, ByRef sWarehousearea As String, ByVal pLoadId As String, ByVal pOnContainer As Boolean, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing) As String
        Dim SQL As String = String.Empty
        Dim pr As String
        Dim zrepl As New ZoneReplenish
        Dim qtyUpdated As Decimal
        Dim ReplPolicyMethod As String = GetPolicyID()
        Dim dt As New DataTable, dr As DataRow
        If WMS.Logic.Load.Exists(pLoadId) Then
            Dim oLoad As New Load(pLoadId)
            Dim oSku As New SKU(oLoad.CONSIGNEE, oLoad.SKU)
            If Not oSku.OPORTUNITYRELPFLAG Then
                Return String.Empty
            End If
            SQL = String.Format("SELECT DISTINCT PUTREGION FROM vZONEREPLINVENTORYLEVEL WHERE SKU = '{0}' and consignee = '{1}'", oLoad.SKU, oLoad.CONSIGNEE)
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                pr = dr("PUTREGION")
                qtyUpdated = zrepl.ZoneReplenishment(pr, oLoad.SKU, ReplPolicyMethod, pUser, oLogger, oLoad, pOnContainer)
                If qtyUpdated > 0 Then
                    oLoad = New Load(pLoadId)
                    sLocation = oLoad.DESTINATIONLOCATION
                    sWarehousearea = oLoad.DESTINATIONWAREHOUSEAREA
                Else
                    sLocation = String.Empty
                    sWarehousearea = String.Empty
                End If
            Next
        End If
        Return String.Empty
    End Function

    Protected Function CreateOpportunityReplTasks(ByVal PolicyDetails As DataTable, ByVal zb As PickLoc, ByVal pLoadId As String, ByVal pOnContainer As Boolean, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim currentqty As Double = zb.CurrentQty + zb.PendingQty
        Dim dr As DataRow
        Dim ReplPolicyMethod As String = GetPolicyID()
        'we have to replenish according to the policy detail
        ' Check in sku definition if we can do opurtunity replanishment
        Dim aSku As SKU = New SKU(zb.Consignee, zb.SKU)
        If Not aSku.OPORTUNITYRELPFLAG Then Return False

        For Each dr In PolicyDetails.Rows
            While True
                'RWMS-2628 - passing the optional parameter for opportunity replen
                If Not CreateReplTasksInRegion(dr, zb, currentqty, ReplPolicyMethod, oLogger, pLoadId, pOnContainer, 0, True) Then
                    Exit While
                Else
                    Return True
                End If
            End While
        Next
    End Function


#End Region

#Region "Normal Replenishment"


    Public Sub NormalReplenish(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pPickRegion As String, ByVal pSku As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0)

        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("SELECT * FROM vPickLoc inner join location on vpickloc.location = location.location and vpickloc.warehousearea = location.warehousearea WHERE vPickLoc.LOCATION LIKE '{0}%' and vPickLoc.WAREHOUSEAREA LIKE '{3}%' AND vPickLoc.PICKREGION LIKE '{1}%' AND SKU LIKE '{2}%' and isnull(location.status,0) = 1 and isnull(location.problemflag,0) = 0 and (vpickloc.BATCHPICKLOCATION is null or vpickloc.BATCHPICKLOCATION='0') ",
        pLocation, pPickRegion, pSku, pWarehousearea)
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            Try
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Trying picking location {0} in warehouse area {1}", dr("Location"), dr("WarehouseArea")))
                End If
                LocReplenish(dr, ReplPolicyMethod, WMS.Lib.USERS.SYSTEMUSER, oLogger)
            Catch ex As Made4Net.Shared.M4NException
                If Not oLogger Is Nothing Then
                    oLogger.writeSeperator()
                    oLogger.Write("Error while replenish location " & dr("Location"))
                    oLogger.Write(ex.GetErrMessage(0))
                    oLogger.writeSeperator()
                End If
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.writeSeperator()
                    oLogger.Write("Error while replenish location " & dr("Location"))
                    oLogger.Write(ex.ToString)
                    oLogger.writeSeperator()
                End If
            End Try
        Next
    End Sub

    Protected Function CreateNormalReplTasks(ByVal PolicyDetails As DataTable, ByVal zb As PickLoc, ByVal pReplMethod As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal Simulation As Integer = 0) As Boolean

        'Jira 241. deducted zb.AllocatedQty from below expression which dint exists before. The allocated qty is returned from vpicloc view
        ' which returns alloacted qty due to order or wave planing. This value is got from pickloc table.

        Dim currentqty As Double = zb.CurrentQty + zb.PendingQty - zb.AllocatedQty

        Dim dr As DataRow
        If zb.ReplQty < currentqty And Not pReplMethod = ReplenishmentMethods.ManualReplenishment Then
            If Not oLogger Is Nothing Then
                oLogger.Write("The current quantity is greater than the normal replenishment quantity.")
            End If
            Return False
        End If
        'we have to replenish according to the policy detail
        If zb.ReplQty > currentqty Or pReplMethod = ReplenishmentMethods.ManualReplenishment Or currentqty < zb.MaximumReplQty Then
            'Dim objpriority As Prioritize = New PrioritizeReplenishments
            For Each dr In PolicyDetails.Rows
                'Start Commented for PWMS-756
                'Dim counter As Integer = 0
                'End Commented for PWMS-756
                While True
                    'Start Commented for PWMS-756
                    'counter = counter + 1
                    'If pReplMethod = ReplenishmentMethods.ManualReplenishment Then
                    '    _taskPriority = Convert.ToInt32(objpriority.GetPriorityValue(TASKPRIORITY.HIGHEST))
                    '    _dueDate = DateAndTime.Now
                    'ElseIf (counter = 1) Then
                    '    _taskPriority = Convert.ToInt32(objpriority.GetPriorityValue(TASKPRIORITY.HIGH))
                    'Else
                    '    _taskPriority = Convert.ToInt32(objpriority.GetPriorityValue(TASKPRIORITY.LOWEST))
                    'End If
                    'End Commented for PWMS-756

                    If Not CreateReplTasksInRegion(dr, zb, currentqty, pReplMethod, oLogger, "", False, Simulation) Then
                        Exit While
                    End If
                End While
            Next
        End If
        'RWMS-2716
        CheckPickLocToUpdatepriority(zb, oLogger, pReplMethod)
        'RWMS-2716 END
    End Function
    Public Function ReplenishmentExists(ByVal SKU As String, Location As String, Consignee As String, WAREHOUSEAREA As String,
                                               Optional ByRef taskID As String = "", Optional ByRef replID As String = "", Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim sql As String = $"SELECT TASK,REPLENISHMENT FROM TASKS WHERE  REPLENISHMENT IN(select REPLID from vReplenishment where SKU='{SKU}'" &
                            $"and TOLOCATION='{GetFlowRackLocation(Location, oLogger)}' and CONSIGNEE='{Consignee}' and  status='PLANNED')"
        Dim dt As DataTable = New DataTable()
        Try
            oLogger.SafeWrite("Checking if there is any pending replenishment task exists for SKU: " + SKU)
            oLogger.SafeWrite("sql: " + sql)
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                replID = dt.Rows(0)("REPLENISHMENT")
                taskID = dt.Rows(0)("TASK")
                oLogger.SafeWrite("Pending replenishment task " + taskID + " found for SKU:" + SKU + " ; ReplenID:" + replID)
                ReplenishmentExists = True
            Else
                ReplenishmentExists = False
            End If
        Catch ex As Exception
            oLogger.SafeWrite("Exception raised while checking pending Replenishment Task. Exception details: " + ex.Message)
            ReplenishmentExists = False
        End Try
        Return ReplenishmentExists

    End Function
    Public Shared Function GetFlowRackLocation(location As String, oLogger As LogHandler)
        Dim dtLoc As DataTable = New DataTable
        Dim sql As String = String.Format("select LOCATION from LOCATION where FRONTRACKLOCATION='{0}'", location)
        If oLogger IsNot Nothing Then
            oLogger.Write("Attempting to get location where frontracklocation is {0}", location)
            oLogger.Write("sql: {0}", sql)
        End If
        Try
            DataInterface.FillDataset(sql, dtLoc)
            If dtLoc.Rows.Count > 0 Then
                Dim frontrackLoc As String = dtLoc.Rows(0).Item("LOCATION")
                If oLogger IsNot Nothing Then
                    oLogger.Write("location={0}", frontrackLoc)
                End If
                If Not String.IsNullOrEmpty(frontrackLoc) Then
                    If oLogger IsNot Nothing Then
                        oLogger.Write("Substituting FlowRack location: {0}", frontrackLoc)
                    End If
                    Return frontrackLoc
                End If
            End If
        Catch ex As Exception
            If oLogger IsNot Nothing Then
                oLogger.WriteException(ex)
            End If
        End Try
        Return location
    End Function

    'RWMS-2716
    Public Function CheckPickLocToUpdatepriority(PickLocation As PickLoc, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pReplMethod As String = Nothing)
        Try
            Dim dtPickloc As New DataTable
            Dim drPickloc As DataRow
            Dim sql As String = String.Format("select LOCATION,SKU,CURRENTQTY,PENDINGQTY,ALLOCATEDQTY,REPLQTY,MAXREPLQTY from vPickLoc where LOCATION = '{0}'", PickLocation.Location, oLogger)
            DataInterface.FillDataset(sql, dtPickloc)

            Dim CurrentQty As Double
            Dim PendingQty As Double
            Dim AllocaedQty As Double
            Dim ReplQty As Double
            Dim replenPriority As Integer
            Dim PickLocLocation As String
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Checking to see Replenishment Task Priority update is required. PickLoc:({0}), SKU:({1})", PickLocation.Location, PickLocation.SKU))
            End If

            If dtPickloc.Rows.Count > 0 Then
                'get the pickloc quantities
                drPickloc = dtPickloc.Rows(0)
                ReplQty = drPickloc.Item("REPLQTY")
                CurrentQty = drPickloc.Item("CURRENTQTY")
                AllocaedQty = drPickloc.Item("ALLOCATEDQTY")
                PickLocLocation = drPickloc.Item("LOCATION")
                'PendingQty - get from the sql
                Dim objpriorityreset As Prioritize = New PrioritizeReplenishments
                Dim pendingpriority As Integer = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_PENDING))
                replenPriority = pendingpriority
                'Dim sqlpendingqty As String = String.Format("select isnull(sum(r.units),0) from REPLENISHMENT r inner join TASKS t on r.REPLID=t.REPLENISHMENT where t.PRIORITY>{0} and r.status not in ('COMPLETE','CANCELED') and t.TOLOCATION='{1}'", pendingpriority, PickLocation.Location)
                'PendingQty = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlpendingqty)
                'If Not oLogger Is Nothing Then
                '    oLogger.Write(String.Format("PickLoc CurrentQty={0},  Existing Replens Qty(Priority > PENDING):{1},ReplQty={2}", CurrentQty, PendingQty.ToString(), ReplQty))
                'End If


                'get the oldest replenishment task
                Dim sqlrepln As String = String.Format("select TOP 1 REPLENISHMENT,FROMLOAD,PRIORITY,TASK from TASKS where tolocation = '{0}'  and STATUS not in ('CANCELED','COMPLETE') ORDER BY REPLENISHMENT ASC", GetFlowRackLocation(PickLocation.Location, oLogger))
                Dim dtreplen As New DataTable
                DataInterface.FillDataset(sqlrepln, dtreplen)
                Dim Oldestreplid As String
                Dim OldestReplenLoadID As String
                Dim OldestReplenTaskPriority As String
                Dim OldestRepnTaskID As String
                Dim ImmediatePriority As Integer
                Dim loadObj As Load
                Dim locObj As Location = New Location()

                ImmediatePriority = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_IMMEDIATE))
                If dtreplen.Rows.Count > 0 Then
                    Dim rowreplen As DataRow = dtreplen.Rows(0)

                    If Not String.IsNullOrEmpty(rowreplen("REPLENISHMENT")) Then
                        Oldestreplid = rowreplen("REPLENISHMENT")
                    End If
                    If Not String.IsNullOrEmpty(rowreplen("FROMLOAD")) Then
                        OldestReplenLoadID = rowreplen("FROMLOAD")
                        loadObj = New Load(OldestReplenLoadID)
                    End If

                    If Not String.IsNullOrEmpty(rowreplen("PRIORITY")) Then
                        OldestReplenTaskPriority = rowreplen("PRIORITY")
                    End If

                    If Not String.IsNullOrEmpty(rowreplen("TASK")) Then
                        OldestRepnTaskID = rowreplen("TASK")
                    End If

                    If Not oLogger Is Nothing Then
                        oLogger.Write(String.Format("Oldest Replenishmment = ({0}), LoadId = ({1},Priority = ({2}),TaskID = ({3}))", Oldestreplid, OldestReplenLoadID, OldestReplenTaskPriority, OldestRepnTaskID))
                    End If

                    If Convert.ToInt32(OldestReplenTaskPriority) = ImmediatePriority Then
                        oLogger.SafeWrite("Oldest Replenishmment({0}) Task has priority IMMEDIDATE", Oldestreplid)
                        If pReplMethod <> ReplenishmentMethods.ManualReplenishment AndAlso ((CurrentQty > ReplQty) And (AllocaedQty < CurrentQty)) Then

                            oLogger.SafeWrite("[(CurrentQty({0}) > ReplQty({1})) AND (AllocaedQty({2})< CurrentQty({0}))]", CurrentQty, ReplQty, AllocaedQty)
                            Dim sqlCancelTasks As String = String.Format("UPDATE TASKS SET STATUS='CANCELED' WHERE REPLENISHMENT IN (SELECT REPLID  FROM REPLENISHMENT   WHERE TOLOCATION ='{0}' and STATUS ='PLANNED') AND STATUS ='AVAILABLE'", GetFlowRackLocation(PickLocation.Location, oLogger))
                            DataInterface.RunSQL(sqlCancelTasks)
                            oLogger.SafeWrite("Canceled all the tasks for tolocation={0},sku:{1}", PickLocation.Location, PickLocation.SKU)
                            locObj.CancelReplenishByLocation(GetFlowRackLocation(PickLocation.Location, oLogger), Common.GetCurrentUser(), PickLocation.SKU)

                            Dim sqlCancelReplenishments As String = String.Format("UPDATE REPLENISHMENT   SET STATUS ='CANCELED' WHERE TOLOCATION ='{0}' and STATUS ='PLANNED'", GetFlowRackLocation(PickLocation.Location, oLogger))
                            DataInterface.RunSQL(sqlCancelReplenishments)

                            oLogger.SafeWrite("Canceled all the replenisments for tolocation={0},sku:{1}", PickLocation.Location, PickLocation.SKU)
                            oLogger.SafeWrite("Update SQL:" & sqlCancelReplenishments)
                        End If

                    Else

                        'Calculating the exisitng replens qty with task priority > PENDING priority
                        'Dim sqlpendingqty As String = String.Format("select isnull(sum(r.units),0) from REPLENISHMENT r inner join TASKS t on r.REPLID=t.REPLENISHMENT where t.PRIORITY>{0} and r.status not in ('COMPLETE','CANCELED') and t.TOLOCATION='{1}'", pendingpriority, PickLocLocation)
                        'PendingQty = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlpendingqty)


                        'If Not oLogger Is Nothing Then
                        '    oLogger.Write(String.Format("LoadID={0}, Calcuated Qty={1}, Existing Replens Qty(Priority > PENDING)={2}, Load units ={3}, Load units UNITSALLOCATED={4},ReplQty={5}, MaximumReplQty={6}.", loadObj.LOADID, CurrentQty, PendingQty.ToString(), loadObj.UNITS.ToString(), loadObj.UNITSALLOCATED.ToString(), ReplQty, PickLocation.MaximumReplQty))
                        'End If

                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Oldest Replenishmment Task Priority({0})", OldestReplenTaskPriority))
                        End If

                        If Not oLogger Is Nothing Then
                            oLogger.Write(String.Format("Ready to evaluate conditions for reprioritizing oldest Replen Task"))
                        End If

                        'Normal Priority 300
                        If (CurrentQty < ReplQty) And (CurrentQty + loadObj.UNITS < PickLocation.MaximumReplQty) Then
                            'Set the priority to NORMAL
                            replenPriority = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_NORMAL))
                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("[CurrentQty({0}) < ReplQty({1})] and [CurrentQty({0}) + loadObj.UNITS({2}) < MaximumReplQty({3})]", CurrentQty, ReplQty, loadObj.UNITS, PickLocation.MaximumReplQty))
                            End If
                        End If

                        'Order Demand 500
                        If (CurrentQty < ReplQty) And (AllocaedQty > CurrentQty) Then
                            'Set the priority to ORDER_DEMAND
                            replenPriority = Convert.ToInt32(objpriorityreset.GetPriorityValue(TASKPRIORITY.PRIORITY_ORDER_DEMAND))
                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("[CurrentQty({0}) < ReplQty({1})] AND [AllocatedQty({2}) > CurrentQty({0})]", CurrentQty, ReplQty, AllocaedQty))
                            End If
                        End If
                        If pReplMethod <> ReplenishmentMethods.ManualReplenishment AndAlso ((CurrentQty > ReplQty) And (AllocaedQty < 1)) Then
                            'Cancel all the existing replen tasks and replens of the SKU

                            If Not oLogger Is Nothing Then
                                oLogger.Write($"[(CurrentQty({CurrentQty}) > ReplQty({ReplQty})) AND (AllocaedQty({AllocaedQty})< 1]")

                            End If
                            Dim sqlCancelTasks As String = String.Format("UPDATE TASKS SET STATUS='CANCELED' WHERE REPLENISHMENT IN (SELECT REPLID  FROM REPLENISHMENT   WHERE TOLOCATION ='{0}' and STATUS ='PLANNED') AND STATUS ='AVAILABLE'", GetFlowRackLocation(PickLocation.Location, oLogger))
                            DataInterface.RunSQL(sqlCancelTasks)


                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("Canceled all the tasks for tolocation={0},sku:{1}", PickLocation.Location, PickLocation.SKU))
                                oLogger.Write("Update SQL:" & sqlCancelTasks)
                            End If

                            'loadObj.CancelReplenish(PickLocation.Location, PickLocation.Warehousearea, CurrentQty, Common.GetCurrentUser())
                            locObj.CancelReplenishByLocation(GetFlowRackLocation(PickLocation.Location, oLogger), Common.GetCurrentUser(), PickLocation.SKU)

                            Dim sqlCancelReplenishments As String = String.Format("UPDATE REPLENISHMENT   SET STATUS ='CANCELED' WHERE TOLOCATION ='{0}' and STATUS ='PLANNED'", GetFlowRackLocation(PickLocation.Location, oLogger))
                            DataInterface.RunSQL(sqlCancelReplenishments)

                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("Canceled all the replenisments for tolocation={0},sku:{1}", PickLocation.Location, PickLocation.SKU))
                                oLogger.Write("Update SQL:" & sqlCancelReplenishments)
                            End If
                        Else
                            'update task priority
                            Dim updatesqlPriority As String = String.Format("UPDATE TASKS SET PRIORITY={0} WHERE REPLENISHMENT = '{1}'", replenPriority, Oldestreplid)
                            DataInterface.RunSQL(updatesqlPriority)

                            If Not oLogger Is Nothing Then
                                oLogger.Write(String.Format("Replenishment Task(ReplID)={0}, Priority Updated to:{1}", Oldestreplid.ToString(), replenPriority.ToString()))
                                oLogger.Write("Update SQL:" & updatesqlPriority)
                            End If

                        End If

                    End If

                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("SQL:" & sqlrepln)
                        oLogger.Write("No Qualified Replenishment Task found to update priority")
                    End If
                End If



            End If

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function
    'RWMS-2716 END

#End Region

#Region " Replenishment"


    ' PWMS-756 - Event ID 31 and 194 for Location count ,Load Changes this method will trigger from Replenishment Service
    Public Sub Replenishment(ByVal oLoc As Location, ByVal pConsignee As String, ByVal pSKU As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal processCancellation As Boolean = False)
        Dim dtPickloks As New DataTable

        Dim sql As String = String.Format("SELECT LOCATION,warehousearea,CONSIGNEE,SKU FROM vPICKLOC WHERE LOCATION = '{0}' and consignee ='{1}' and SKU='{2}' and warehousearea='{3}'",
           oLoc.Location, pConsignee, pSKU, oLoc.WAREHOUSEAREA)
        If Not oLogger Is Nothing Then
            oLogger.Write(sql)
        End If
        DataInterface.FillDataset(sql, dtPickloks)
        ReplenishLocations(dtPickloks, oLogger)
        If processCancellation Then
            Dim manager As New ReplenishmentManager
            manager.CancelReplenishments(pSKU, pConsignee, oLoc.Location, oLogger)
        End If

    End Sub
    ' PWMS-756 - Event ID 19 for Order Planned this method will trigger from Replenishment Service


    Public Sub Replenishment(ByVal pConsignee As String, ByVal pOrderId As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim dtPickloks As New DataTable


        Dim sql As String = String.Format("select pd.fromlocation as location, pd.FROMWAREHOUSEAREA as Warehousearea,pd.consignee,pd.sku from pickdetail as pd left join pickloc as pl on pd.fromlocation=pl.LOCATION where pd.consignee = '{0}' and pd.orderid = '{1}' and pd.fromload = ''  and pd.status in('RELEASED','PLANNED') and (pl.BATCHPICKLOCATION is null or pl.BATCHPICKLOCATION = '0')", pConsignee, pOrderId)

        If Not oLogger Is Nothing Then
            oLogger.Write(sql)
        End If
        DataInterface.FillDataset(sql, dtPickloks)
        Dim manager As New ReplenishmentManager
        _dueDate = manager.GetOutBoundDepartureDate(pOrderId)
        ReplenishLocations(dtPickloks, oLogger)

    End Sub
    ' PWMS-756 - Event ID 187 for Pick List Assignment this method will trigger from Replenishment Service
    Public Sub ProcessPickListAssignment(ByVal pTaskID As String, Optional ByVal oLogger As LogHandler = Nothing)
        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        Dim dtPickloks As New DataTable
        Dim sql As String = String.Format("select PICKLOC.LOCATION as location, PICKDETAIL.FROMWAREHOUSEAREA as Warehousearea, PICKDETAIL.CONSIGNEE, TASKS.SKU, QTY, PICKDETAIL.PICKLIST , TASKS.USERID from pickdetail Join TASKS on tasks.PICKLIST = PICKDETAIL.PICKLIST and TASKS.SKU = PICKDETAIL.SKU join PICKLOC on PICKDETAIL.SKU=PICKLOC.SKU where TASK='{0}'", pTaskID)
        DataInterface.FillDataset(sql, dtPickloks)
        If dtPickloks.Rows.Count > 0 Then
            Dim dataRow As DataRow = dtPickloks.Rows(0)
            Dim manager As New ReplenishmentManager
            _dueDate = manager.CalculateReplenishmentTime(dataRow.Item("location").ToString(), dataRow.Item("USERID").ToString())
            LocReplenish(dataRow.Item("location").ToString(), dataRow.Item("Warehousearea").ToString(), dataRow.Item("CONSIGNEE").ToString(), dataRow.Item("SKU").ToString(), ReplPolicyMethod, dataRow.Item("USERID").ToString(), oLogger, 0, dataRow.Item("QTY").ToString())
        End If


    End Sub
    'PWMS-756 -  Added for Pick list Unallocate Event ID : 52
    Public Sub ProcessPickListCancel(ByVal picklistId As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal processCancellation As Boolean = False)

        Dim dtPicklist As New DataTable
        Dim sql As String = String.Format("select distinct fromlocation as location, FROMWAREHOUSEAREA as Warehousearea,consignee,sku from pickdetail pd inner join pickheader ph on pd.picklist = ph.picklist where  pd.picklist= '{0}' and fromload = ''  and pd.STATUS='CANCELED' and fromlocation in (select location from pickloc)  ", picklistId)
        DataInterface.FillDataset(sql, dtPicklist)
        If Not oLogger Is Nothing Then
            oLogger.Write(sql)
        End If
        If dtPicklist.Rows.Count > 0 Then
            Dim dataRow As DataRow
            If processCancellation Then
                'Loop through all the pick locations of the given picklist and cancel the replens 
                For Each dataRow In dtPicklist.Rows
                    Dim pickloc As New PickLoc(dataRow("location"), dataRow("Warehousearea"), dataRow("consignee"), dataRow("sku"), True)
                    CheckPickLocToUpdatepriority(pickloc, oLogger)
                Next

            End If
        End If


    End Sub
    ' Added new method for PWMS-860 for short Pick
    'RWMS-2606 - Added parameter picklistline
    Public Sub ProcessUserPickShort(ByVal picklistId As String, ByVal picklistline As String, ByVal oLogger As LogHandler, ByVal processUserPickShort As Boolean)
        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        Dim dtPicklist As New DataTable

        'RWMS-2606 - Commented
        'Dim sql As String = String.Format("select pd.PICKLIST,pd.PICKEDQTY,pd.ADJQTY,pd.QTY,fromlocation as location, FROMWAREHOUSEAREA as Warehousearea,consignee,sku,pd.ADDUSER as UserID from pickdetail pd inner join pickheader ph on pd.picklist = ph.picklist where pd.picklist= '{0}' and fromlocation in (select location from pickloc) ", picklistId)
        'RWMS-2606 - Commented END
        'RWMS-2606 START
        Dim sql As String = String.Format("select pd.PICKLIST,pd.PICKEDQTY,pd.ADJQTY,pd.QTY,fromlocation as location, FROMWAREHOUSEAREA as Warehousearea,consignee,sku,pd.ADDUSER as UserID from pickdetail pd inner join pickheader ph on pd.picklist = ph.picklist where pd.picklist= '{0}' and pd.picklistline= '{1}' and fromlocation in (select location from pickloc) ", picklistId, picklistline)
        'RWMS-2606 END

        DataInterface.FillDataset(sql, dtPicklist)
        If Not oLogger Is Nothing Then
            oLogger.Write(sql)
        End If
        If dtPicklist.Rows.Count > 0 Then
            'Commented for RWMS-2490
            'Dim dataRow As DataRow = dtPicklist.Rows(0)

            'If dataRow.Item("PICKEDQTY") < dataRow.Item("QTY") Then
            '    LocReplenish(dataRow.Item("location").ToString(), dataRow.Item("Warehousearea").ToString(), dataRow.Item("CONSIGNEE").ToString(), dataRow.Item("SKU").ToString(), ReplPolicyMethod, dataRow.Item("USERID").ToString(), oLogger, 0, dataRow.Item("PICKEDQTY").ToString(), True)
            'End If
            'End Commented for RWMS-2490

            'RWMS-2606 - Commented
            ''Added for RWMS-2490 - Added the foreach loop to consider all the rows of the picklist i.e. all the rows of the datatable dtPicklist for processing replenishment.
            'Dim dtRow As DataRow
            'For Each dtRow In dtPicklist.Rows
            '    If dtRow("PICKEDQTY") < dtRow("QTY") Then
            '        LocReplenish(dtRow("location").ToString(), dtRow("Warehousearea").ToString(), dtRow("CONSIGNEE").ToString(), dtRow("SKU").ToString(), ReplPolicyMethod, dtRow("USERID").ToString(), oLogger, 0, dtRow("PICKEDQTY").ToString(), True)
            '    End If
            'Next
            ''End Added for RWMS-2490
            'RWMS-2606 - Commented END
            'RWMS-2606 START
            Dim dtRow As DataRow = dtPicklist.Rows(0)

            If dtRow("PICKEDQTY") < dtRow("QTY") Then
                LocReplenish(dtRow("location").ToString(), dtRow("Warehousearea").ToString(), dtRow("CONSIGNEE").ToString(), dtRow("SKU").ToString(), ReplPolicyMethod, dtRow("USERID").ToString(), oLogger, 0, dtRow("PICKEDQTY").ToString(), True)
            End If
            'RWMS-2606 END

        End If
    End Sub
    ' Ended new method for PWMS-860 for short Pick

    ' PWMS-756 - Event ID 227 for Wave Planned this method will trigger from Replenishment Service
    Public Sub Replenishment(ByVal pWaveId As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal processCancellation As Boolean = False)
        Dim dtPickloks As New DataTable
        Dim sql As String = String.Format("select fromlocation as location, FROMWAREHOUSEAREA as Warehousearea, pd.consignee, pd.sku from pickdetail pd inner join pickheader ph on pd.picklist = ph.picklist inner join pickloc pl on pd.fromlocation = pl.location where ph.wave = '{0}' and fromload = '' and pd.status in('RELEASED','PLANNED') and (pl.BATCHPICKLOCATION is null or pl.BATCHPICKLOCATION = '0')", pWaveId)
        If Not oLogger Is Nothing Then
            oLogger.Write(sql)
        End If
        DataInterface.FillDataset(sql, dtPickloks)

        If dtPickloks.Rows.Count <= 0 And Not oLogger Is Nothing Then
            oLogger.Write("No Locations Returned for the requested Replenishment")
        End If

        Dim manager As New ReplenishmentManager
        _dueDate = manager.GetWaveDepartureDate(pWaveId)
        ReplenishLocations(dtPickloks, oLogger)

        If processCancellation And dtPickloks.Rows.Count > 0 Then
            Dim drPickLoc As DataRow = dtPickloks.Rows(0)
            manager.CancelReplenishments(drPickLoc("sku"), drPickLoc("consignee"), drPickLoc("location"))
        End If
    End Sub
    'Added for RWMS-1840
    Public Shared Function GetPolicyID() As String
        Dim dt As New DataTable
        Dim policyId As String
        Dim sql As String = String.Format("SELECT TOP 1 PolicyId FROM REPLPOLICY WHERE REPLTYPE = 'NEGTREPL'")
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            'ERRROR NO CONTAINER
            Return ""
        End If
        Dim dr As DataRow = dt.Rows(0)
        policyId = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PolicyId"))

        Return policyId
    End Function
    'Added for RWMS-1840

    'Ended for RWMS-1840

    Private Sub ReplenishLocations(ByVal dtLocsToReplenish As DataTable, ByVal oLogger As LogHandler)
        Dim cons, sku, sql As String
        'Try to replenish the picklocs
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to replenish selected number of picklocks: {0}", If((dtLocsToReplenish Is Nothing), 0, dtLocsToReplenish.Rows.Count))
        End If
        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        For Each drLocsToReplenish As DataRow In dtLocsToReplenish.Rows
            LocReplenish(Convert.ToString(drLocsToReplenish("Location")), Convert.ToString(drLocsToReplenish("Warehousearea")), Convert.ToString(drLocsToReplenish("consignee")), Convert.ToString(drLocsToReplenish("sku")), ReplPolicyMethod, WMS.Lib.USERS.SYSTEMUSER, oLogger)

        Next
        'Try to replenish Zones
        Dim zrepl As New ZoneReplenish
        For Each drWaveException As DataRow In dtLocsToReplenish.Rows
            Dim dt As New DataTable, dr As DataRow
            Dim pr As String
            cons = drWaveException("consignee")
            sku = drWaveException("sku")
            sql = String.Format("SELECT DISTINCT PUTREGION FROM vZONEREPLINVENTORYLEVEL WHERE SKU = '{0}' and consignee = '{1}'", sku, cons)
            DataInterface.FillDataset(sql, dt)
            For Each dr In dt.Rows
                pr = dr("PUTREGION")
                zrepl.ZoneReplenishment(pr, sku, ReplPolicyMethod, WMS.Lib.USERS.SYSTEMUSER, oLogger)
            Next
        Next
    End Sub

    Protected Sub CreateReplTasks(ByVal PolicyDetails As DataTable, ByVal zb As PickLoc, Optional ByVal oLogger As LogHandler = Nothing)
        Dim dr As DataRow
        Dim currentqty As Double
        Dim ReplPolicyMethod As String = GetPolicyID()
        For Each dr In PolicyDetails.Rows
            'Loop through the replenishment policy regions
            Dim pickLocRefreshed As New PickLoc(zb.Location, zb.Warehousearea, zb.Consignee, zb.SKU)
            zb = pickLocRefreshed
            'Calculate the calculated Quantity to fullfill the order demand
            currentqty = zb.CurrentQty + zb.PendingQty - zb.AllocatedQty

            If Not oLogger Is Nothing Then
                oLogger.Write("Get the updated quantities from vPickLoc view")
            End If

            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("PickLoc={0}, SKU={1},ReplQty={2},MaxReplQty={3},PickLoc:CurrentQty={4},PendingQty={5},AllocatedQty={6},OverAllocatedQty={7}", zb.Location, zb.SKU, zb.ReplQty, zb.MaximumReplQty, zb.CurrentQty, zb.PendingQty, zb.AllocatedQty, zb.OverAllocatedQty))
                oLogger.Write("Calculated  Quantity(CurrentQty + PendingQty - AllocatedQty):" & currentqty)
            End If

            'Check the required qty below the replin point
            If currentqty < zb.ReplQty Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Replenishment process begin for pick region:" & dr("PICKREGION"))
                End If

                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Calculated Quantity is BELOW Replen point:(CurrentQty+PendingQty-AllocatedQty({0}) < ReplQty({1}))", currentqty, zb.ReplQty))
                End If
                CreateReplTasksInRegion(dr, zb, currentqty, ReplPolicyMethod, oLogger)
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Calculated Quantity is ABOVE Replen point ,Replenishment will not be created:(CurrentQty+PendingQty-AllocatedQty({0}) > ReplQty({1}))", currentqty, zb.ReplQty))
                End If
                Exit For
            End If
        Next

    End Sub
    Protected Sub CreateReplPickShortTasks(ByVal PolicyDetails As DataTable, ByVal zb As PickLoc, Optional ByVal oLogger As LogHandler = Nothing)

        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = GetPolicyID()
        'Ended for RWMS-1840

        Dim ld As Load
        If Not oLogger Is Nothing Then
            oLogger.Write(" For pick loc Repl Qty " & zb.ReplQty & " Current Qty " & zb.CurrentQty & " Pending Qty " & zb.PendingQty & " Allocated Qty " & zb.AllocatedQty & " Over Allocated Qty " & zb.OverAllocatedQty & " Maximum Qty " & zb.MaximumReplQty)
        End If

        Dim currentqty As Double = zb.CurrentQty + zb.PendingQty - zb.AllocatedQty
        Dim dr As DataRow

        Dim pReplPolicy As String
        pReplPolicy = zb.ReplPolicy

        For Each dr In PolicyDetails.Rows
            If Not oLogger Is Nothing Then
                oLogger.Write(" Computed Available Qty " & currentqty)
            End If

            ' Added code for PWMS-860 for short Pick
            ' CreateReplTasksInRegion(dr, zb, currentqty, ReplenishmentMethods.Replenishments, oLogger)
            ld = getUserPickShort(dr, ReplPolicyMethod, dr("PICKREGION"), dr("UOM"), pReplPolicy, zb, currentqty, 0, oLogger)
            ' Ended code for PWMS-860 for short Pick
        Next


    End Sub

#End Region

#End Region

#Region "Reports"

    Public Function PrintWorksheet(ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "Replanishment"
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "PackingList", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "Replanishment")
        oQsender.Add("DATASETID", "repReplanishment")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
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
        'oQsender.Add("WHERE", String.Format("CONTAINERID = '{0}'", _container))
        oQsender.Send("Report", repType)
    End Function

#End Region

#End Region

End Class

#Region "Replenishment Policy Scoring"

<CLSCompliant(False)> Public Class ReplenishmentPolicyScoring

#Region "Variables"

    Protected _policyid As String
    Protected _attributesscoring As Made4Net.DataAccess.Collections.GenericCollection

#End Region

#Region "Properties"

    Public ReadOnly Property PolicyId() As String
        Get
            Return _policyid
        End Get
    End Property

    Public ReadOnly Property Key(ByVal idx As Int32) As String
        Get
            Return _attributesscoring.Keys(idx)
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal idx As Int32) As Decimal
        Get
            Return _attributesscoring(idx)
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal sKey As String) As Decimal
        Get
            Return _attributesscoring(sKey)
        End Get
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal sPolicyId As String)
        _policyid = sPolicyId
        Load()
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim sql As String = String.Format("SELECT * FROM REPLPOLICYSCORING WHERE POLICYID = {0}", Made4Net.Shared.Util.FormatField(_policyid))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            _attributesscoring = New Made4Net.DataAccess.Collections.GenericCollection
            For Each oCol As DataColumn In dt.Columns
                If Not oCol.ColumnName.ToLower = "policyid" Then
                    _attributesscoring.Add(oCol.ColumnName, Convert.ToDecimal(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr(oCol.ColumnName), "0")))
                End If
            Next
        End If
        dt.Dispose()
    End Sub

    Public Sub Score(ByRef cLoadsCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write("Start Scoring the loads collection...")
        End If
        If cLoadsCollection.Length = 0 Then Return
        Dim oScoreSetter As ScoreSetter =
            New ScoreSetter.ScoreSetterBuilder() _
            .Logger(oLogger) _
            .Build()

        'Added for RWMS-1829
        If Not _attributesscoring Is Nothing Then
            oScoreSetter.Score(cLoadsCollection, _attributesscoring)
        End If
        'Ended for RWMS-1829

        Try
            Dim first As Boolean = True
            For Each LoadRow As DataRow In cLoadsCollection
                Dim DataRowStr, CaptionRowStr As String
                If first Then
                    If oLogger IsNot Nothing Then
                        oLogger.Write("Load".PadRight(23) & "Location".PadRight(10) & "Status".PadRight(11) & "QTY".PadRight(10) & "Allocated".PadRight(11) & "Available".PadRight(11) & "Score".PadRight(10))
                        oLogger.writeSeperator("-", 80)
                        oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    End If
                    first = False
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    End If
                End If
            Next
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator(" ", 20)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub DeleteAll()
        Dim sql As String = String.Format("Delete FROM REPLPOLICYSCORING WHERE POLICYID = {0}", Made4Net.Shared.FormatField(_policyid))
        DataInterface.RunSQL(sql)
        _attributesscoring.Clear()
    End Sub

    '#Region "Old Scoring"

    '    Public Sub Score(ByRef cLoadsCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
    '        If cLoadsCollection.Length = 0 Then Return
    '        ClearScore(cLoadsCollection)
    '        For idx As Int32 = 0 To _attributesscoring.Count - 1
    '            If _attributesscoring(idx) Is DBNull.Value Or _attributesscoring(idx) Is Nothing Or _attributesscoring(idx) = 0 Then
    '                'Do Nothing
    '            Else
    '                If _attributesscoring(idx) < 0 Then
    '                    Sort(cLoadsCollection, _attributesscoring.Keys(idx), SortOrder.Descending)
    '                Else
    '                    Sort(cLoadsCollection, _attributesscoring.Keys(idx), SortOrder.Ascending)
    '                End If
    '                setScore(cLoadsCollection, _attributesscoring.Keys(idx), Math.Abs(_attributesscoring(idx)))
    '            End If
    '        Next
    '        Sort(cLoadsCollection, "Score", SortOrder.Descending)

    '        Try
    '            Dim first As Boolean = True
    '            For Each LoadRow As DataRow In cLoadsCollection
    '                Dim DataRowStr, CaptionRowStr As String
    '                If first Then
    '                    If Not oLogger Is Nothing Then
    '                        oLogger.Write("Load".PadRight(23) & "Location".PadRight(10) & "Status".PadRight(11) & "QTY".PadRight(10) & "Allocated".PadRight(11) & "Available".PadRight(11) & "Score".PadRight(10))
    '                        oLogger.writeSeperator("-", 80)
    '                        oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
    '                    End If
    '                    first = False
    '                Else
    '                    If Not oLogger Is Nothing Then
    '                        oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(20) & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & Convert.ToString(LoadRow("STATUS")).PadRight(10) & Convert.ToString(LoadRow("UNITS")).PadRight(10) & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & Convert.ToString(LoadRow("SCORE")).PadRight(10))
    '                    End If
    '                End If
    '            Next

    '            If Not oLogger Is Nothing Then
    '                oLogger.writeSeperator(" ", 20)
    '            End If

    '        Catch ex As Exception
    '            If Not oLogger Is Nothing Then
    '                oLogger.Write("Error in scoring procedure")
    '                oLogger.Write(ex.Message)
    '            End If
    '        End Try

    '    End Sub

    '    Protected Sub setScore(ByRef cLoadsCollection As DataRow(), ByVal sFieldName As String, ByVal dAttributeScore As Decimal)
    '        Dim iNumValues As Int32
    '        Dim iValueIdx As Int32 = 0
    '        Dim oVal As Object = cLoadsCollection(0)(sFieldName)
    '        iNumValues = getNumValues(cLoadsCollection, sFieldName)
    '        For idx As Int32 = 0 To cLoadsCollection.Length - 1
    '            If (oVal Is Nothing Or oVal Is DBNull.Value) And (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '                ' Do Nothing - Same Values
    '            ElseIf (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '                oVal = cLoadsCollection(idx)(sFieldName)
    '                iValueIdx = iValueIdx + 1
    '            ElseIf oVal <> cLoadsCollection(idx)(sFieldName) Then
    '                oVal = cLoadsCollection(idx)(sFieldName)
    '                iValueIdx = iValueIdx + 1
    '            End If
    '            cLoadsCollection(idx)("score") = cLoadsCollection(idx)("score") + (dAttributeScore - (iValueIdx * dAttributeScore / iNumValues))
    '        Next
    '    End Sub

    '    Protected Function getNumValues(ByRef cLoadsCollection As DataRow(), ByVal sFieldName As String)
    '        Dim iNumValues As Int32 = 1
    '        Dim val As Object = cLoadsCollection(0)(sFieldName)
    '        For idx As Int32 = 0 To cLoadsCollection.Length - 1
    '            If (val Is Nothing Or val Is DBNull.Value) And (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '                ' Do Nothing - Same Values
    '            ElseIf (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '                val = cLoadsCollection(idx)(sFieldName)
    '                iNumValues = iNumValues + 1
    '            ElseIf cLoadsCollection(idx)(sFieldName) <> val Then
    '                val = cLoadsCollection(idx)(sFieldName)
    '                iNumValues = iNumValues + 1
    '            End If
    '        Next
    '        Return iNumValues
    '    End Function

    '    Protected Sub ClearScore(ByRef cLoadsCollection As DataRow())
    '        For Each oLoadRecord As DataRow In cLoadsCollection
    '            oLoadRecord("Score") = 0
    '        Next
    '    End Sub

    '    Protected Sub Sort(ByRef cLoadsCollection As DataRow(), ByVal sFieldName As String, ByVal eSortOrder As SortOrder)
    '        Dim bSorted As Boolean = False
    '        For idx As Int32 = 1 To cLoadsCollection.Length - 1
    '            bSorted = True
    '            For jdx As Int32 = cLoadsCollection.Length - 1 To idx Step -1
    '                Select Case eSortOrder
    '                    Case SortOrder.Ascending
    '                        If (Not cLoadsCollection(jdx).IsNull(sFieldName)) Then
    '                            If cLoadsCollection(jdx - 1).IsNull(sFieldName) Then
    '                                Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                                bSorted = False
    '                            ElseIf (cLoadsCollection(jdx)(sFieldName) < cLoadsCollection(jdx - 1)(sFieldName)) Then
    '                                Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                                bSorted = False
    '                            End If
    '                        End If
    '                    Case SortOrder.Descending
    '                        If (Not cLoadsCollection(jdx).IsNull(sFieldName)) Then
    '                            If cLoadsCollection(jdx - 1).IsNull(sFieldName) Then
    '                                Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                                bSorted = False
    '                            ElseIf (cLoadsCollection(jdx)(sFieldName) > cLoadsCollection(jdx - 1)(sFieldName)) Then
    '                                Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                                bSorted = False
    '                            End If
    '                        End If
    '                End Select
    '            Next
    '            If bSorted Then Exit For
    '        Next
    '    End Sub

    '    Protected Enum SortOrder
    '        Ascending
    '        Descending
    '    End Enum

    '    Protected Sub Swap(ByRef oDataRow1 As DataRow, ByRef oDataRow2 As DataRow)
    '        Dim tempDataRow As DataRow = oDataRow1
    '        oDataRow1 = oDataRow2
    '        oDataRow2 = tempDataRow
    '    End Sub
    '#End Region

#End Region

End Class

#End Region

#End Region


#Region "Replenishment Policy"

<CLSCompliant(False)> Public Class ReplenishmentPolicy

#Region "Variables"

#Region "PrimaryKeys"
    Dim _policyid As String
#End Region

#Region "Other Fields"
    Dim _policyname As String
    Dim _adddate As DateTime
    Dim _adduser As String
    Dim _editdate As DateTime
    Dim _edituser As String
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("PolicyID={0}", Made4Net.Shared.FormatField(_policyid))
        End Get
    End Property

    Public Property PolicyID() As String
        Get
            Return _policyid
        End Get
        Set(ByVal value As String)
            _policyid = value
        End Set
    End Property

    Public Property PolicyName() As String
        Get
            Return _policyname
        End Get
        Set(ByVal value As String)
            _policyname = value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal value As DateTime)
            _adddate = value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal value As String)
            _adduser = value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal value As DateTime)
            _editdate = value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal value As String)
            _edituser = value
        End Set
    End Property
#End Region

#Region "Constructors"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pPolicyId As String)
        _policyid = pPolicyId
    End Sub
#End Region

#Region "Methods"

    Public Sub Delete()
        Dim sql As String = String.Format("Delete FROM REPLPOLICY where {0}", WhereClause)
        DataInterface.RunSQL(sql)

        sql = String.Format("Delete FROM REPLPOLICYSCORING WHERE {0}", WhereClause)
        DataInterface.RunSQL(sql)

        sql = String.Format("Delete from REPLPOLICYDETAIL where {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class

#End Region