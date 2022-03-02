Imports System.Threading
Imports Made4Net.Shared.Web
'Added for Replen error
Imports Made4Net.DataAccess
'End Added for Replen error
Partial Public Class StatusWait
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Added for Replen error
        Dim SkuClause As String = String.Empty
        If Request.Params("SKU") IsNot Nothing Then
            SkuClause = " and sku='" & Request.QueryString("SKU").ToString() & "'"
        End If
        Dim index As Integer = 0
        While index < 1000
            index += 1
            'call the query here and update index
            Dim sql As String
            Dim dt As New DataTable
            Dim count As Integer = 0
            sql = String.Format("Select top 1 Task from TASKS where userid = '{0}' and assigned = 1 and tasktype in ('FULLREPL','NEGTREPL','PARTREPL') and status = 'ASSIGNED'" & SkuClause, WMS.Logic.Common.GetCurrentUser)
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                count = 0
                Thread.Sleep(200)
            Else
                count = 1
                Session("MANREPLENISHMENT") = New WMS.Logic.Task(dt.Rows(0)("Task"))
            End If
            If count = 0 Then
                Continue While
            Else
                Exit While
            End If

        End While

        If index = 0 Then
            Response.AddHeader("Refresh", "1")
        Else
            lblStatus.Text = "Replenishment task created.Redirecting..."
            ImageStatus.Visible = False

            'Thread.Sleep(2000)
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        End If

        'End Added for Replen error
    End Sub

End Class