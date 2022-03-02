Imports System.Collections.Generic
Imports Made4Net.DataAccess
Imports Made4Net.General
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
'Base class for prioritizing tasks, TODO: Extend the implementation to use chain of responsibility when task interleaving need to be handled.
Public MustInherit Class Prioritize

    Private _logHandler As LogHandler
    Public Property LogHandler As LogHandler
        Get
            Return _logHandler
        End Get
        Set(value As LogHandler)
            _logHandler = value
        End Set
    End Property


    'Mehod to be implemented by concrete class based on business logic. Dictionary having Key as parameter name and value as parameter value. Put parameter as constants
    Public MustOverride Function GetTaskPriority(parameters As Dictionary(Of String, Object)) As String
    Public MustOverride Function GetTaskPriorityImmediate() As String
    ''' <summary>
    ''' Dictionalry that holds priority values
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared priority As Dictionary(Of String, String)
    ''' <summary>
    ''' Gets priority code for the defined priority name
    ''' </summary>
    ''' <param name="priorityName">priority Name</param>
    ''' <returns>priority code for tasks</returns>
    ''' <remarks></remarks>
    Public Function GetPriorityValue(priorityName As String) As String
        If priority Is Nothing Then
            priority = New Dictionary(Of String, String)
            Dim dao = New MasterDao
            Dim dt As DataTable
            dt = dao.GetAllTaskPriorities()
            Dim row As DataRow
            For Each row In dt.Rows
                priority.Add(row("Parameter"), row("DataValue1"))
            Next
        End If
        Dim priorityCode As String
        priority.TryGetValue(priorityName, priorityCode)
        Return priorityCode
    End Function


    ''' <summary>
    ''' Sets priority for the task based on replenishment location
    ''' </summary>
    ''' <param name="PriorityType"></param>
    ''' <param name="PickLocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <CLSCompliant(False)>
    Public Function SetPriority(PriorityType As Integer, ByVal PickLocation As PickLoc)
        Try
            Dim dao = New ReplenishmentDao
            dao.SetPriority(PriorityType, PickLocation)
        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function

    'RWMS-2714
    <CLSCompliant(False)>
    Public Function ReSetPriority(PriorityType As Integer, ByVal PickLocation As PickLoc, PriorityReset As Integer, Optional logHandler As LogHandler = Nothing)
        Try
            Dim dao = New ReplenishmentDao
            dao.ReSetPriority(PriorityType, PickLocation, PriorityReset, logHandler)
        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function
    'RWMS-2714 END

    MustOverride Sub PickListAssignmentPrioritization(ByVal pTaskID As String, Optional ByVal oLogger As LogHandler = Nothing)

End Class

Public Class REPLENISHMENTS

    Public Const REPLENISHMENTTYPE As String = "REPLENISHMENTTYPE"
    Public Const PICKLOC As String = "PICKLOC"
    Public Const Replenishment As String = "Replenishment"
    'Public Const PICKQTY As String = "PICKDQUANTITY"
    Public Const StatusOfDetailsToSearch As String = "'RELEASED'"
End Class