Imports System.Runtime.CompilerServices

Public Module ExtensionMethods
#Region "Methods"
    'Returns week start date time by deducting current day of week from today's date time
    <Extension()> _
    Public Function GetWeekStart(ByVal timeOfDay As DateTime) As DateTime
        Dim result As DateTime = DateTime.MinValue
        If timeOfDay <> Nothing Then
            result = timeOfDay.Date.AddDays(-timeOfDay.DayOfWeek)
        End If
        Return result
    End Function

    'Returns last week's start date time by deducting 7 + current day of week from today's date time
    <Extension()>
    Public Function GetPriorWeekStart(ByVal timeOfDay As DateTime) As DateTime
        Dim result As DateTime = DateTime.MinValue
        If timeOfDay <> Nothing Then
            result = timeOfDay.Date.AddDays(-timeOfDay.DayOfWeek - 7)
        End If
        Return result
    End Function

    'Checks if the data row contains column names provided as param array
    <Extension()> _
    Public Function ContainsColumns(ByVal dr As DataRow, ParamArray columnNames() As String) As Boolean
        If dr Is Nothing OrElse columnNames Is Nothing Then
            Return False
        End If

        Return columnNames.All(Function(x As String)
                                   Return dr.Table.Columns.Contains(x)
                               End Function)
    End Function
#End Region
End Module
