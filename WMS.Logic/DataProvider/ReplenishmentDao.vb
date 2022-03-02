Imports Made4Net.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.Logging
'Data access layer to fetch replenishment data
<CLSCompliant(False)> Public Class ReplenishmentDao
    ''' <summary>
    ''' Sets priority for the task based on replenishment location
    ''' </summary>
    ''' <param name="PriorityType"></param>
    ''' <param name="PickLocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetPriority(PriorityType As Integer, ByVal PickLocation As PickLoc)
        Dim sqlpriority As String = String.Format(SqlQueries.UpdatePriorityForReplenishment, PriorityType, PickLocation.Consignee, PickLocation.SKU)
        Logger.Write(String.Concat("The SQL is :", sqlpriority), "General")
        DataInterface.RunSQL(sqlpriority)
    End Function

    'RWMS-2714
    Public Function ReSetPriority(PriorityType As Integer, ByVal PickLocation As PickLoc, PriorityReset As Integer, Optional logHandler As LogHandler = Nothing)
        Dim sqlpriority As String = String.Format(SqlQueries.ReSetPriorityForReplenishment, PriorityType, PickLocation.Consignee, PickLocation.SKU, PriorityReset)
        Logger.Write(String.Concat("The SQL is :", sqlpriority), "General")
        If Not logHandler Is Nothing Then
            logHandler.Write(String.Concat("The SQL is :", sqlpriority))
        End If
        DataInterface.RunSQL(sqlpriority)
    End Function
    'RWMS-2714 END

    ''' <summary>
    ''' Sets priority for the task based on replenishment location
    ''' </summary>
    ''' <param name="dueDate"></param>
    ''' <param name="PickLocation"></param>
    ''' <remarks></remarks>
    '''
    Sub UpdateReplenishmentDate(dueDate As Date, PickLocation As PickLoc)
        Dim sqlReplenish As String = String.Format(SqlQueries.UpdateDueDateForReplenishment, dueDate, PickLocation.Consignee, PickLocation.SKU)
        Logger.Write(String.Concat("The SQL is :", sqlReplenish), "General")
        DataInterface.RunSQL(sqlReplenish)
    End Sub

    ''' <summary>
    ''' Get the Cancel Replenishments for the task based on replenishment location
    ''' </summary>
    ''' <param name="psku"></param>
    ''' <param name="pconsignee"></param>
    ''' <param name="plocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCancelReplenishments(psku As String, pconsignee As String, plocation As String, Optional ByVal oLogger As LogHandler = Nothing) As DataTable

        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetCancelReplenishmentsValue, psku, pconsignee, plocation), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetCancelReplenishmentsValue, psku, pconsignee, plocation)), "General")

        If Not oLogger Is Nothing Then
            oLogger.Write(String.Concat(" Calling GetLoadIDForPayload() The SQL is : ", String.Format(SqlQueries.GetCancelReplenishmentsValue, psku, pconsignee, plocation)))
        End If

        Return dt

    End Function

    ''' <summary>
    ''' Get the Get OutBound Departure Date for the Order ID
    ''' </summary>
    ''' <param name="pOrderId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetOutBoundDepartureDateByOrder(ByVal pOrderId As String) As DataTable
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetOutBoundDepartureDateValue, pOrderId), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetOutBoundDepartureDateValue, pOrderId)), "General")
        Return dt

    End Function

    ''' <summary>
    ''' Get the  Wave Departure Date for the Wave ID
    ''' </summary>
    ''' <param name="pWaveId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWaveDepartureDateByWave(ByVal pWaveId As String) As DataTable
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetWaveDepartureDateValue, pWaveId), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetWaveDepartureDateValue, pWaveId)), "General")
        Return dt
    End Function
    ''' <summary>
    ''' Get the  Get Standard Time For User
    ''' </summary>
    ''' <param name="pUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStandardTimeForUser(ByVal pUser As String) As DataTable
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetStandardTimeValue, pUser), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetStandardTimeValue, pUser)), "General")
        Return dt

    End Function

    ''' <summary>
    ''' Get the Calculate Pick Time For User
    ''' </summary>
    ''' <param name="pUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CalculatePickTimeForUser(ByVal pUser As String) As DataTable
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.CalculatePickTimeValue, pUser), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.CalculatePickTimeValue, pUser)), "General")
        Return dt
    End Function

    ''' <summary>
    ''' Get the Calculate Replenishment Time For User
    ''' </summary>
    ''' <param name="PickLocation"></param>
    ''' <param name="pUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''
    Public Function CalculateReplenishmentTimeForUser(ByVal PickLocation As String, ByVal pUser As String) As DataTable

        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.CalculateReplenishmentTimeValue, pUser), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.CalculateReplenishmentTimeValue, pUser)), "General")
        Return dt

    End Function

    ''' <summary>
    ''' Update Replenishment Status
    ''' </summary>
    ''' <param name="_status"></param>
    ''' <param name="_editdate"></param>
    ''' <param name="_edituser"></param>
    ''' <param name="_replId"></param>
    ''' <remarks></remarks>
    '''
    Sub UpdateReplenishmentStatus(_status As String, _editdate As Date, _edituser As String, _replId As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim sqlReplenish As String = String.Format(SqlQueries.UpdateStatusForReplenishmentValue, _status, _editdate, "SYSTEM", _replId)
        Logger.Write(String.Concat("The SQL is :", sqlReplenish), "General")
        DataInterface.RunSQL(sqlReplenish)

        If Not oLogger Is Nothing Then
            oLogger.Write(sqlReplenish)
        End If

    End Sub

    ''' <summary>
    ''' Get the LOADID for PayLoad
    ''' </summary>
    ''' <param name="pLoadId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''
    Public Function GetLoadIDForPayload(ByVal pLoadId As String, Optional ByVal oLogger As LogHandler = Nothing) As DataTable

        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetLoadIDForPayloadValue, pLoadId), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetLoadIDForPayloadValue, pLoadId)), "General")

        If Not oLogger Is Nothing Then
            oLogger.Write(String.Concat(" Calling GetLoadIDForPayload() The SQL is : ", String.Format(SqlQueries.GetLoadIDForPayloadValue, pLoadId)))
        End If

        Return dt

    End Function
    ''' <summary>
    ''' Update Payload  Status and Units
    ''' </summary>
    ''' <param name="_unitsallocated"></param>
    ''' <param name="_editdate"></param>
    ''' <param name="_edituser"></param>
    ''' <param name="_loadId"></param>
    ''' <remarks></remarks>
    '''
    Sub UpdatePayloadStatus(_unitsallocated As Double, _editdate As Date, _edituser As String, _loadId As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim sqlPayload As String = String.Format(SqlQueries.UpdateStatusForPayloadValue, _unitsallocated, _editdate, "SYSTEM", _loadId)
        Logger.Write(String.Concat("The SQL is :", sqlPayload), "General")
        DataInterface.RunSQL(sqlPayload)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Concat(" Calling UpdatePayloadStatus() The SQL is : ", sqlPayload))
        End If

    End Sub

    Public ReadOnly Property hasTask(_replid As String, Optional ByVal oLogger As LogHandler = Nothing) As String
        Get
            Dim sql As String = String.Format("Select count(1) from TASKS where tasktype in ('{0}','{1}','{2}') and REPLENISHMENT='{3}'", WMS.Lib.TASKTYPE.PARTREPL, WMS.Lib.TASKTYPE.FULLREPL, WMS.Lib.TASKTYPE.NEGTREPL, _replid)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Concat(" Calling hasTask() The SQL is : ", sql))
            End If

        End Get
    End Property

    Public Function getReplTask(ByVal pIgnoreStatus As Boolean, _replid As String, Optional ByVal oLogger As LogHandler = Nothing) As ReplenishmentTask

        Dim sql As String
        If pIgnoreStatus Then
            sql = String.Format("SELECT DISTINCT TASK FROM TASKS WHERE REPLENISHMENT='{0}'", _replid)
        Else
            sql = String.Format("SELECT DISTINCT TASK FROM TASKS WHERE STATUS<>'COMPLETE' AND STATUS<>'CANCELED' AND REPLENISHMENT='{0}'", _replid)
        End If
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)

        If Not oLogger Is Nothing Then
            oLogger.Write(String.Concat(" Calling getReplTask() The SQL is : ", sql))
        End If

        If dt.Rows.Count = 0 Then
            Return Nothing
        Else
            Return New ReplenishmentTask(dt.Rows(0)("TASK"))
        End If
    End Function

End Class