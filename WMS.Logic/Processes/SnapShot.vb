
<CLSCompliant(False)> Public Class SnapShot
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Public Sub New()

    End Sub

    Public Overrides Sub Execute()
        'Dim aq As New EventManagerQ
        'Dim action As String = WMS.Lib.Actions.Audit.SNAPSHOT
        'aq.Add("ACTION", action)
        'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'aq.Send(action)
        Try
            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.SnapShot
            em.Add("EVENT", EventType)
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))
        Catch ex As Exception
        End Try
    End Sub
End Class
