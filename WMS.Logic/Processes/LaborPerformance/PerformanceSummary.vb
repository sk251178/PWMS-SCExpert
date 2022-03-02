Imports Made4Net.DataAccess
Imports Made4Net.Shared
<CLSCompliant(False)> Public Class PerformanceSummary
    Public Function GetTaskPerformance(ByVal _task As String) As Decimal
        Dim sql As String = String.Format("Select shiftperformance from vShiftTaskPerformance where taskid={0}", Made4Net.Shared.FormatField(_task))
        Try
            Return CType(DataInterface.ExecuteScalar(sql), Decimal)
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Shared Function GetUserPerformanceOnShift(ByVal pUserID As String) As Decimal
        Dim pShiftID As String = getShiftIDbyUserID(pUserID)
        Dim sql As String = String.Format("Select shiftuserperformance from vcurrentusersperformance where userid='{0}' and shiftid='{1}'", pUserID, pShiftID)
        Return CType(DataInterface.ExecuteScalar(sql), Decimal)
    End Function
    Public Shared Function getShiftIDbyUserID(ByVal pUserID As String)
        Dim SQL As String = String.Format("select isnull(DEFAULTSHIFT,'') DEFAULTSHIFT from USERWAREHOUSE where WAREHOUSE='{0}' and USERID='{1}' ", Warehouse.CurrentWarehouse, pUserID)
        Dim DefaultShift As String = DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME)
        Return DefaultShift
    End Function
End Class
