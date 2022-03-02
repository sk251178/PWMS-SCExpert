<CLSCompliant(False)> Public Class CountTask
    Inherits Task

#Region "Constructors"

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

    'Complete the count task , return false if no counting commited (which means needs to verify the count job)
    Public Function Count(ByVal oCnt As WMS.Logic.Counting, ByVal oCntJob As CountingJob, ByVal pUser As String) As Boolean
        If (oCntJob.TaskType = WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING Or oCntJob.TaskType = WMS.Lib.TASKTYPE.LOCATIONCOUNTING) And oCntJob.LocationCountingLoadCounted Then
            Return CountLoad(oCntJob.LoadId, oCntJob.CountedQty, oCntJob.UOM, oCntJob.Location, oCntJob.Warehousearea, oCntJob.LoadCountVerified, oCntJob.CountingAttributes, pUser)
        Else
            Dim success As Boolean = oCnt.Count(oCntJob, pUser)
            If Not success Then
                Return False
            End If
            _executionlocation = oCntJob.Location
            _executionwarehousearea = oCntJob.Warehousearea
            _edituser = pUser
            Me.Complete(Nothing)
        End If
        Return True
    End Function

    Private Function CountLoad(ByVal pLoadid As String, ByVal pCountedQty As Decimal, ByVal pCountedUom As String, ByVal pConutedLocation As String, ByVal pConutedWarehousearea As String, ByVal pCountingVerified As Boolean, ByVal pLoadAttributes As WMS.Logic.AttributesCollection, ByVal pUser As String) As Boolean
        Dim ld As New WMS.Logic.Load(pLoadid, True)
        'Check the delta of conuting and match it to sku tolerance
        If (ld.ShouldVerifyCounting(pCountedQty, pCountedUom) Or Not WMS.Logic.Load.ShouldVerifyCountingAttributes(pLoadid, pLoadAttributes)) And Not pCountingVerified Then
            Return False
        Else
            ld.Count(pCountedQty, pCountedUom, pConutedLocation, pConutedWarehousearea, pLoadAttributes, pUser)
            Return True
        End If
    End Function

    Public Function getCountJob(ByVal oCnt As WMS.Logic.Counting) As CountingJob
        Dim countJob As New CountingJob
        countJob.TaskId = _task
        countJob.TaskType = _task_type
        countJob.CountId = _countid
        countJob.Location = _fromlocation
        countJob.Warehousearea = _fromwarehousearea

        Select Case _task_type
            Case WMS.Lib.TASKTYPE.LOADCOUNTING
                Dim oLoad As New Load(_fromload)
                countJob.Consignee = _consignee
                countJob.Sku = _sku
                countJob.LoadId = _fromload
                countJob.ExpectedQty = oLoad.UNITS
                countJob.UOM = oLoad.LOADUOM
            Case WMS.Lib.TASKTYPE.LOCATIONCOUNTING
                'qty will be filled by screen....
            Case WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                'qty will be filled by screen....
        End Select
        ' And set the task start time
        MyBase.SetStartTime()
        Return countJob
    End Function

    Public Function ValidateBulkCount(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pQty As Decimal, ByVal pUser As String) As Boolean
        Dim SQL As String = String.Format("select sum(units) as units from invload where consignee = {0} and sku = {1} and location = {2} and warehousearea = {3} group by consignee, sku", _
            Made4Net.Shared.Util.FormatField(pConsignee), Made4Net.Shared.Util.FormatField(pSku), Made4Net.Shared.Util.FormatField(pLocation), Made4Net.Shared.Util.FormatField(pWarehousearea))
        Dim units As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If units = pQty Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Overrides Sub Cancel()
        Dim oCounting As New WMS.Logic.Counting(_countid)
        oCounting.Cancel(_countid, WMS.Logic.GetCurrentUser)
    End Sub

    Public Overrides Sub Complete(ByVal logger As LogHandler, Optional ByVal pProblemRC As String = "")
        Dim oCounting As New WMS.Logic.Counting(_countid)
        If oCounting.STATUS.Equals(WMS.Lib.Statuses.Counting.COMPLETE, StringComparison.OrdinalIgnoreCase) Then
            Return
        End If
        ' RWMS-1701, RWMS-1702
        Dim oWHActivity As New WHActivity
        oWHActivity.USERID = WMS.Logic.GetCurrentUser
        oWHActivity.SaveLastLocation()
        ' RWMS-1701, RWMS-1702
        oCounting.Complete(WMS.Logic.GetCurrentUser)
    End Sub

End Class

#Region "Counting Job"

Public Class CountingJob
    Public TaskId As String
    Public TaskType As String
    Public CountId As String
    Public Consignee As String
    Public Sku As String
    Public Location As String
    Public Warehousearea As String
    Public LoadId As String
    Public ExpectedQty As Double
    Public CountedQty As Double
    Public UOM As String
    Public CountedLoads As DataTable

    'This flag will hold whether the bulk count succeeded or moved to location count
    Public BulkCountLoadsCounted As Boolean

    'These Flags are for counting verifications
    Public LocationCountingLoadCounted As Boolean
    Public LoadCountVerified As Boolean
    <CLSCompliant(False)>
    Public CountingAttributes As WMS.Logic.AttributesCollection
End Class

#End Region