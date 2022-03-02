Imports Made4Net.DataAccess

Public Class OrderDao
    ''' <summary>
    ''' Gets list of task priority codes from codelist
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllOrdersForWave(wave As String) As DataTable
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetOrdersForWave, wave), dt)
        Return dt
    End Function
End Class
