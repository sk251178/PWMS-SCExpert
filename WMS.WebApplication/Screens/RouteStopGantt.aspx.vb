Imports System.Drawing
Imports System.Web.UI

Imports Made4Net.DataAccess
Imports Made4Net.Schema
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared.Strings
Imports Made4Net.DataAccess.Collections


Imports Telerik.WebControls
Imports Telerik.Charting
Imports Telerik.RadChartUtils


Partial Public Class RouteStopGantt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sql As String = "select * from vGanttRouteStopGroup  " & _
            " order by 1,2,3 "
        Dim oGantt As New RoutesGantt()

        Dim h1, h2, p1 As Integer
        If Not Page.Request("h2") Is Nothing Then
            h2 = Page.Request("h2")
        End If
        If Not Page.Request("h1") Is Nothing Then
            h1 = Page.Request("h1")
        End If
        If Not Page.Request("p1") Is Nothing Then
            p1 = Page.Request("p1")
        End If

        If Not Page.Request("h2") Is Nothing Then
            oGantt.ReloadChart(sql, h1, h2, p1)
        Else
            oGantt.ReloadChart(sql)
        End If

        Controls.Add(oGantt)

    End Sub



   
End Class

