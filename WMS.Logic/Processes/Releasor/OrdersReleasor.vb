Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class OrdersReleasor
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Public Overrides Sub Execute()
        Dim dt As New DataTable
        Dim oPckList As Picklist
        Dim dr As DataRow
        Dim sql As String = String.Format("select distinct picklist from pickheader where status = '{0}'", _
            WMS.Lib.Statuses.Picklist.PLANNED)

        DataInterface.FillDataset(sql, dt)
        For i As Int32 = 0 To dt.Rows.Count - 1
            Try
                dr = dt.Rows(i)
                oPckList = New Picklist(dr("picklist"))
                'If oPckList.FULLYPLANNED Then
                oPckList.setStatus(WMS.Lib.Statuses.Picklist.RELEASING, WMS.Lib.USERS.SYSTEMUSER)
                Dim qh As New Made4Net.Shared.QMsgSender
                qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.RELEASEPICKLIST)
                qh.Values.Add("PICKLISTID", oPckList.PicklistID)
                qh.Values.Add("USER", "SYSTEM")
                qh.Send("Releasor", oPckList.PicklistID)
                'End If
            Catch ex As Exception
            End Try
        Next
    End Sub

End Class
