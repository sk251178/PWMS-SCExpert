Imports Made4Net.DataAccess
Imports Microsoft.Practices.EnterpriseLibrary.Logging
'Data access layer to fetch Multipayloads data
Public Class MultipayloadsDao

    ''' <summary>
    ''' Get the  Multipayloads for new RDT Screen
    ''' </summary>
    ''' <param name="pLoadId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMultipayloads(pLoadId As String) As DataTable

        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetMultipaylodValue, pLoadId), dt)
        Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetMultipaylodValue, pLoadId)), "General")
        Return dt

    End Function
    ''' <summary>
    ''' Get the  Group of Loads 
    ''' </summary>
    ''' <param name="ploads"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetGroupOfloads(ploads As String) As Integer

        Return DataInterface.ExecuteScalar(String.Format(SqlQueries.GetGroupOfLoadsValue, ploads))

    End Function

    ''' <summary>
    ''' Get the  Group of Loads Height
    ''' </summary>
    ''' <param name="ploadId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLoadsHeight(ploadId As String) As Integer

        Return DataInterface.ExecuteScalar(String.Format(SqlQueries.GetfLoadsHeightValue, ploadId))

    End Function
    ''' <summary>
    ''' Get the  Group of Loads plocation
    ''' </summary>
    ''' <param name="plocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLoadsInLocation(plocation As String) As DataTable

        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetLoadsinLocationValue, plocation), dt)
        'Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetLoadsinLocationValue, plocation)), "General")
        Return dt

    End Function
    ''' <summary>
    ''' Get the  Group of Loads plocation
    ''' </summary>
    ''' <param name="plocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLoadsInDestinationLocation(plocation As String) As DataTable

        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format(SqlQueries.GetLoadsinDestinationLocationValue, plocation), dt)
        ' Logger.Write(String.Concat("The SQL is :", String.Format(SqlQueries.GetLoadsinDestinationLocationValue, plocation)), "General")
        Return dt

    End Function
End Class
