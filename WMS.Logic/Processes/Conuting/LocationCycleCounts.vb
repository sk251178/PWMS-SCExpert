Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class LocationCycleCounts
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

#Region "Execute"

    Public Overrides Sub Execute()
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.LocationCycleCounts
        em.Add("EVENT", EventType)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Generate Counting Tasks"

    Public Function CreateCycleCountTasks(ByVal oLogger As LogHandler, ByVal pUser As String)
        If pUser = Nothing Or pUser = "" Then pUser = "SYSTEM"
        If Not oLogger Is Nothing Then
            oLogger.Write("Started Creating Location Cycle Counts tasks.")
            oLogger.writeSeperator("*", 100)
        End If
        'Init Cached Data
        Dim dtWHArea As DataTable = InitWHArea()
        Dim dtLocation As DataTable = InitLocation()
        If Not oLogger Is Nothing Then
            oLogger.Write("Got " & dtLocation.Rows.Count & " Locations. Trying to create tasks")
        End If
        'Create Counting tasks
        Dim interval As Int32
        Dim arrLoads() As DataRow
        For Each dr As DataRow In dtWHArea.Rows
            If Not oLogger Is Nothing Then
                oLogger.Write("Working on WH Area = " & dr("WAREHOUSEAREA"))
            End If
            interval = dr("CYCLESDAYINT")
            arrLoads = dtLocation.Select("WAREHOUSEAREA = '" & dr("WAREHOUSEAREA") & "'")
            CreateTasks(arrLoads, interval, oLogger, pUser)
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator("-", 50)
            End If
        Next
        If Not oLogger Is Nothing Then
            oLogger.writeSeperator(" ", 100)
            oLogger.writeSeperator("*", 100)
            oLogger.Write("Finished Creating Location Cycle Counts tasks.")
        End If
    End Function

    Private Sub CreateTasks(ByVal arrLocation() As DataRow, ByVal pInterval As Int32, ByVal oLogger As LogHandler, ByVal pUser As String)
        Try
            Dim numtasks As Int32 = arrLocation.Length / pInterval
            If numtasks = 0 Then numtasks = 1
            Dim locCnt As New Counting
            For i As Int32 = 0 To numtasks - 1
                If Not oLogger Is Nothing Then
                    oLogger.Write("Trying to Create Tasks for location: " & arrLocation(i)("location"))
                End If
                locCnt.CreateLocationCountJobs(arrLocation(i)("warehousearea"), arrLocation(i)("PICKREGION"), arrLocation(i)("location"), WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", "", pUser)
            Next
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occurred...")
                oLogger.Write(ex.Description)
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occurred...")
                oLogger.Write(ex.ToString)
            End If
        End Try
    End Sub

#Region "Accessors"

    Private Function InitWHArea() As DataTable
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select distinct CYCLESDAYINT, WAREHOUSEAREA from vLocationCycleCount")
        DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

    Private Function InitLocation() As DataTable
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select * from vLocationCycleCount")
        DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

#End Region

#End Region

End Class
