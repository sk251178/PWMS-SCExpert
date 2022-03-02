Imports Made4Net.DataAccess
''' <summary>
''' Data access layer for master Data
''' </summary>
''' <remarks></remarks>
Public Class MasterDao

    ''' <summary>
    ''' Get code for priority name
    ''' </summary>
    ''' <param name="param"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPriorityCodeForTask(param As String) As String
        Return Convert.ToString(DataInterface.ExecuteScalar(String.Format(SqlQueries.GetCodeListValue, "REPLENISHMENTS", param), "Made4NetSchema"))
    End Function

    ''' <summary>
    ''' Get the sum of qty of Replens at PRIORITY_IMMEDIATE 
    ''' </summary>
    ''' <param name="location"></param>
    ''' <param name="priority"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSumOfQtyInReplenWithPriority(location As String, priority As String) As String
        Return Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DataInterface.ExecuteScalar(String.Format(SqlQueries.GetQtyInReplenAtPriority, location, priority)), "0"))
    End Function
    ''' <summary>
    ''' Gets list of task priority codes from codelist
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllTaskPriorities() As DataTable
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetCodeList, "REPLENISHMENTS"), dt, False, "Made4NetSchema")
        Return dt
    End Function
    ''' <summary>
    ''' Set Priority of Task
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetPriorityOfTask(taskid As String, toPriority As Integer) As Integer
        Return DataInterface.RunSQL(String.Format(SqlQueries.UpdatePriorityOfReplenishment, toPriority, taskid))
    End Function

End Class
