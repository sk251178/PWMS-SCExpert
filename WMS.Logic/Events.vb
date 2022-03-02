<CLSCompliant(False)> Public Class TMSEvents

    Public Enum [Event]
        RouteStatusChanged
    End Enum

    Public Shared Function EventToString(ByVal e As [Event]) As String
        Select Case e
            Case [Event].RouteStatusChanged
                Return "RouteStatusChanged"
        End Select
    End Function

    Public Shared Function EventFromString(ByVal eName As String) As [Event]
        Select Case eName.ToLower
            Case "routestatuschanged"
                Return [Event].RouteStatusChanged
        End Select
    End Function

    Public Shared Function getRegisteredProcesses(ByVal e As [Event]) As System.Collections.Specialized.StringCollection
        Dim regs As New System.Collections.Specialized.StringCollection
        Dim dt As New DataTable
        Dim sql As String = String.Format("select queuename from eventsregistration where eventname = {0}", Made4Net.Shared.Util.FormatField(EventToString(e)))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            regs.Add(Convert.ToString(dr("queuename")))
        Next
        Return regs
    End Function
End Class
