Imports System.Data
Partial Public Class DeliveryProgress
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Me.Request.QueryString("w")) And Not IsNothing(Me.Request.QueryString("h")) Then
            If Me.Request.QueryString("cid").Equals("1") Then
                'BuildChart(Request.Params.Get("w"), Request.Params.Get("h")).Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
                BuildMade4NetChartBitmap(Request.Params.Get("w"), Request.Params.Get("h")).Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            Else
                BuildMade4NetPercentChartBitmap(Request.Params.Get("w"), Request.Params.Get("h")).Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg)
            End If
            Response.End()
        Else
            'Me.Controls.Add(BuildMade4NetChart(300, 300))
        End If

    End Sub
    'Private Function BuildPercentMade4NetChart(ByVal width As Int32, ByVal height As Int32) As System.Drawing.Bitmap
    '    Dim PercentChartInfo As New Made4Net.WebControls.Charting.ChartInfo
    '    Dim PercentChart As New Made4Net.WebControls.Charting.ChartViewer()

    'End Function
    Private Function BuildPercentChart(ByVal width As Int32, ByVal height As Int32) As System.Drawing.Bitmap
        ' upgrade telerik controls from 2.0 to 4.5
        'Dim PercentChart As New Telerik.WebControls.RadChart
        Dim PercentChart As New Telerik.Web.UI.RadChart()
        PercentChart.Width = width
        PercentChart.Height = height
        PercentChart.Skin = "DeepBlue"
        PercentChart.ChartTitle.TextBlock.Text = Made4Net.WebControls.TranslationManager.Translate("Delivery Percent")
        PercentChart.ChartTitle.Visible = True
        PercentChart.PlotArea.YAxis.AutoScale = False
        PercentChart.PlotArea.YAxis.MaxValue = 100
        PercentChart.PlotArea.YAxis.Step = 10
        PercentChart.PlotArea.XAxis.Visible = Telerik.Charting.Styles.ChartAxisVisibility.False
        Dim TasksPercent As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT * from vDeliveryStopProgressPercent")
        Dim TaskPercentSeries As New Telerik.Charting.ChartSeries()
        Dim TaskPercentValue As New Telerik.Charting.ChartSeriesItem
        Dim PackPercent As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT * from vDeliveryPackageProgressPercent")
        Dim PackPercentSeries As New Telerik.Charting.ChartSeries()
        Dim PackPercentValue As New Telerik.Charting.ChartSeriesItem
        TaskPercentValue.YValue = TasksPercent
        TaskPercentSeries.Name = Made4Net.WebControls.TranslationManager.Translate("Stops")
        TaskPercentSeries.AddItem(TaskPercentValue)
        PercentChart.AddChartSeries(TaskPercentSeries)
        PackPercentValue.YValue = PackPercent
        PackPercentSeries.Name = Made4Net.WebControls.TranslationManager.Translate("Packages")
        PackPercentSeries.AddItem(PackPercentValue)
        PercentChart.AddChartSeries(PackPercentSeries)
        Return PercentChart.GetBitmap
    End Function
    Private Function BuildMade4NetChart(ByVal width As Int32, ByVal height As Int32) As Made4Net.WebControls.Charting.ChartViewer
        Dim _ChartInfo As New Made4Net.WebControls.Charting.ChartInfo
        _ChartInfo.Title = "Delivery Progress Chart"
        _ChartInfo.ChartInfoDataTemplate = Made4Net.Schema.DataTemplate.Load(0, "DTDeliveryProgressChart")
        _ChartInfo.Func = "COUNT"
        _ChartInfo.XField = "STATUS"
        _ChartInfo.YField = "ROUTE"
        '_ChartInfo.YValueField = ""
        _ChartInfo.XTable = _ChartInfo.ChartInfoDataTemplate.SQL
        _ChartInfo.YTable = _ChartInfo.ChartInfoDataTemplate.SQL
        _ChartInfo.StaticPixelHeight = height
        _ChartInfo.StaticPixelWidth = width
        Dim _Chart As New Made4Net.WebControls.Charting.ChartViewer(_ChartInfo)
        _Chart.Regenerate()
        Return _Chart
    End Function
    Private Function BuildMade4NetPercentChartBitmap(ByVal width As Int32, ByVal height As Int32) As System.Drawing.Bitmap
        Dim _ChartInfo As New Made4Net.WebControls.Charting.ChartInfo
        _ChartInfo.Title = ""
        _ChartInfo.ChartType = Made4Net.WebControls.Charting.ChartType.Stack
        _ChartInfo.MultipleSeries = True
        _ChartInfo.ChartInfoDataTemplate = Made4Net.Schema.DataTemplate.Load(0, "DTDeliveryProgressPercentChart")
        _ChartInfo.Func = "SUM"
        _ChartInfo.XField = "PTYPE"
        '_ChartInfo.YField = "STATUS"
        _ChartInfo.YValueField = "PPROGRESS"
        _ChartInfo.XTable = _ChartInfo.ChartInfoDataTemplate.SQL
        _ChartInfo.YTable = _ChartInfo.ChartInfoDataTemplate.SQL
        _ChartInfo.StaticPixelHeight = height
        _ChartInfo.StaticPixelWidth = width
        Dim _Chart As New Made4Net.WebControls.Charting.ChartViewer(_ChartInfo)
        _Chart.Regenerate()
        Return _Chart.GetChartBitmap
    End Function
    Private Function BuildMade4NetChartBitmap(ByVal width As Int32, ByVal height As Int32) As System.Drawing.Bitmap
        Dim _ChartInfo As New Made4Net.WebControls.Charting.ChartInfo
        _ChartInfo.Title = "Delivery Progress Chart"
        _ChartInfo.ChartType = Made4Net.WebControls.Charting.ChartType.Stack
        _ChartInfo.MultipleSeries = True
        _ChartInfo.ChartInfoDataTemplate = Made4Net.Schema.DataTemplate.Load(0, "DTDeliveryProgressChart")
        _ChartInfo.Func = "COUNT"
        _ChartInfo.XField = "ROUTENAME"
        _ChartInfo.YField = "STATUS"
        '_ChartInfo.YValueField = ""
        _ChartInfo.XTable = _ChartInfo.ChartInfoDataTemplate.SQL
        _ChartInfo.YTable = _ChartInfo.ChartInfoDataTemplate.SQL
        _ChartInfo.StaticPixelHeight = height
        _ChartInfo.StaticPixelWidth = width
        Dim _Chart As New Made4Net.WebControls.Charting.ChartViewer(_ChartInfo)
        _Chart.Regenerate()
        Return _Chart.GetChartBitmap
    End Function
    Private Function BuildChart(ByVal width As Int32, ByVal height As Int32) As System.Drawing.Bitmap
        'upgrade 2.0 to 4.5.1
        'Dim RadChart1 As New Telerik.WebContro'ls.RadChart
        Dim RadChart1 As New Telerik.Web.UI.RadChart
        RadChart1.Width = width
        RadChart1.Height = height
        RadChart1.SeriesOrientation = Telerik.Charting.ChartSeriesOrientation.Horizontal
        RadChart1.ChartTitle.TextBlock.Text = Made4Net.WebControls.TranslationManager.Translate("Delivery Progress")
        RadChart1.ChartTitle.Visible = True

        'RadChart1.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Solid
        'RadChart1.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Center
        'RadChart1.Appearance.FillStyle.MainColor = Color.White
        'RadChart1.Appearance.FillStyle.SecondColor = Color.Gray
        'RadChart1.Chart.Appearance.FillStyle.MainColor = Color.White
        'RadChart1.Chart.Appearance.FillStyle.SecondColor = Color.Gray
        'RadChart1.Chart.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient
        RadChart1.Skin = "DeepBlue"
        RadChart1.SkinsOverrideStyles = False
        RadChart1.Appearance.Corners.BottomLeft = Telerik.Charting.Styles.CornerType.Round
        RadChart1.Appearance.Corners.BottomRight = Telerik.Charting.Styles.CornerType.Round
        RadChart1.Appearance.Corners.TopLeft = Telerik.Charting.Styles.CornerType.Round
        RadChart1.Appearance.Corners.TopRight = Telerik.Charting.Styles.CornerType.Round
        Dim StatusesTbl As DataTable = GetStatuses()
        Dim RoutesTbl As DataTable = GetTodayRoutes()
        Dim CountersTbl As DataTable
        Dim StatusRow As DataRow
        Dim CounterRow As DataRow
        Dim RouteRow As DataRow

        RadChart1.PlotArea.XAxis.LayoutMode = Telerik.Charting.Styles.ChartAxisLayoutMode.Between
        RadChart1.PlotArea.XAxis.AutoScale = False
        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Text = Made4Net.WebControls.TranslationManager.Translate("Routes")
        RadChart1.PlotArea.XAxis.AxisLabel.Visible = True
        RadChart1.PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Text = Made4Net.WebControls.TranslationManager.Translate("Number Of Deliveries")
        RadChart1.PlotArea.YAxis.AxisLabel.Visible = True
        RadChart1.PlotArea.YAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
        RadChart1.PlotArea.XAxis.Appearance.TextAppearance.TextProperties.Font = New System.Drawing.Font("Arial", 10, FontStyle.Bold)
        'RadChart1.PlotArea.XAxis.Appearance.
        For Each RouteRow In RoutesTbl.Rows
            RadChart1.PlotArea.XAxis.AddItem(Convert.ToString(RouteRow("ROUTENAME")))
        Next
        For Each StatusRow In StatusesTbl.Rows
            Dim StatusSeries As New Telerik.Charting.ChartSeries()
            Dim StatusCode As String = Convert.ToString(StatusRow("CODE"))
            Dim StatusDescr As String = Convert.ToString(StatusRow("DESCRIPTION"))
            StatusSeries.Type = Telerik.Charting.ChartSeriesType.StackedBar
            StatusSeries.Name = Made4Net.WebControls.TranslationManager.Translate(StatusDescr)

            StatusSeries.Appearance.FillStyle.FillSettings.GradientAngle = 90
            StatusSeries.Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient
            'StatusSeries.Appearance.FillStyle.MainColor = GetStatusSecondColor(StatusCode) 'GetStatusMainColor(StatusCode)
            'StatusSeries.Appearance.FillStyle.SecondColor = GetStatusMainColor(StatusCode) 'GetStatusSecondColor(StatusCode)
            'StatusSeries.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside
            StatusSeries.Appearance.ShowLabels = True
            CountersTbl = GetCountersForStatus(StatusCode)
            Dim CountersColl As Collection = GetRoutesChartItemsCollection(RoutesTbl)
            For Each CounterRow In CountersTbl.Rows
                Dim RouteID As String = Convert.ToString(CounterRow("ROUTEID"))
                Dim RouteName As String = Convert.ToString(CounterRow("ROUTENAME"))
                Dim StatusCounter As Int32 = Convert.ToInt32(CounterRow("TASKCOUNT"))
                CType(CountersColl.Item(RouteID), Telerik.Charting.ChartSeriesItem).Visible = True
                CType(CountersColl.Item(RouteID), Telerik.Charting.ChartSeriesItem).Label.Visible = True
                CType(CountersColl.Item(RouteID), Telerik.Charting.ChartSeriesItem).Label.TextBlock.Text = StatusCounter
                CType(CountersColl.Item(RouteID), Telerik.Charting.ChartSeriesItem).YValue = StatusCounter
                'CType(CountersColl.Item(RouteID), Telerik.Charting.ChartSeriesItem).Appearance.FillStyle.FillSettings.GradientAngle = 90
                'CType(CountersColl.Item(RouteID), Telerik.Charting.ChartSeriesItem).Appearance.FillStyle.FillType = Telerik.Charting.Styles.FillType.Gradient
            Next
            For i As Int32 = 1 To CountersColl.Count
                StatusSeries.AddItem(CType(CountersColl.Item(i), Telerik.Charting.ChartSeriesItem))
            Next
            RadChart1.Series.Add(StatusSeries)

        Next
        RadChart1.PlotArea.YAxis.AutoScale = False
        Dim MaxTasks As Int32 = GetMaxTasksPerRoute()
        RadChart1.PlotArea.YAxis.MaxValue = Math.Ceiling(GetMaxTasksPerRoute() + (GetMaxTasksPerRoute() * 0.1))
        If MaxTasks < 10 Then
            RadChart1.PlotArea.YAxis.Step = 1
        ElseIf MaxTasks < 100 Then
            RadChart1.PlotArea.YAxis.Step = 10
        Else
            RadChart1.PlotArea.YAxis.Step = 100
        End If
        Return RadChart1.GetBitmap
    End Function

    Private Function GetStatuses() As DataTable
        Dim statusTable As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset("select code,description from codelistdetail where codelistcode='RSTSTAT' order by code", statusTable)
        Return statusTable
    End Function

    Private Function GetCountersForStatus(ByVal Status As String) As DataTable
        Dim countersTable As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset("SELECT ROUTENAME, ROUTEID, TASKCOUNT FROM vDeliveryProgress WHERE ROUTEDATE = CONVERT(varchar, GETDATE(), 101) AND STATUS='" & Status & "' order by ROUTENAME", countersTable)
        Return countersTable
    End Function
    Private Function GetMaxTasksPerRoute() As Int32
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT max(TASKCOUNT) from (SELECT sum(TASKCOUNT) as TASKCOUNT FROM vDeliveryProgress WHERE ROUTEDATE = CONVERT(varchar, GETDATE(), 101) group by ROUTEID) as tbl")
    End Function

    Private Function GetTodayRoutes() As DataTable
        Dim routesTable As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset("SELECT ROUTENAME, ROUTEID FROM ROUTE WHERE CONVERT(varchar, ROUTEDATE,101) = CONVERT(varchar, GETDATE(), 101) order by ROUTENAME", routesTable)
        Return routesTable
    End Function

    Private Function GetRoutesChartItemsCollection(ByVal RoutesTable As DataTable) As Collection
        Dim RoutesColl As New Collection
        Dim route As DataRow
        For Each route In RoutesTable.Rows
            Dim CounterSeriesItem As New Telerik.Charting.ChartSeriesItem
            'CounterSeriesItem.t
            CounterSeriesItem.Visible = False
            CounterSeriesItem.Name = Convert.ToString(route("ROUTEID"))
            CounterSeriesItem.YValue = 0
            CounterSeriesItem.Label.TextBlock.Text = "0"
            CounterSeriesItem.Label.Visible = False
            CounterSeriesItem.Label.Appearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Inside
            CounterSeriesItem.Label.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Right
            'CounterSeriesItem.Label.TextBlock.Appearance.TextProperties.Font = New System.Drawing.Font("Arial", 12, FontStyle.Bold)
            'CounterSeriesItem.Label.TextBlock.Appearance.TextProperties.Color = Color.Black
            'CounterSeriesItem.Appearance.Border.PenStyle = Drawing2D.DashStyle.Solid
            'CounterSeriesItem.Appearance.Border.Color = Color.Gray
            RoutesColl.Add(CounterSeriesItem, Convert.ToString(route("ROUTEID")))
        Next
        Return RoutesColl
    End Function

    Private Function GetStatusMainColor(ByVal Status As String) As System.Drawing.Color
        Select Case Status
            Case "1" 'New
                Return Color.FromArgb(149, 179, 215)
                'Return Color.LightBlue
            Case "2" 'Suspended
                Return Color.FromArgb(255, 255, 0)
                'Return Color.Yellow
            Case "3" 'Completed
                Return Color.FromArgb(146, 208, 80)
                'Return Color.Green
            Case "4" 'Canceled
                Return Color.FromArgb(217, 149, 148)
                'Return Color.Red
            Case "5" 'Incomplete
                Return Color.FromArgb(255, 192, 0)
                'Return Color.Orange
            Case Else
                Return System.Drawing.Color.White
        End Select
    End Function

    Private Function GetStatusSecondColor(ByVal Status As String) As System.Drawing.Color
        Select Case Status
            Case "1" 'New
                Return Color.FromArgb(174, 197, 224)
                'Return Color.LightBlue
            Case "2" 'Suspended
                Return Color.FromArgb(255, 255, 83)
                'Return Color.Yellow
            Case "3" 'Completed
                Return Color.FromArgb(176, 221, 127)
                'Return Color.Green
            Case "4" 'Canceled
                Return Color.FromArgb(232, 191, 190)
                'Return Color.Red
            Case "5" 'Incomplete
                Return Color.FromArgb(255, 209, 63)
                'Return Color.Orange
            Case Else
                Return System.Drawing.Color.White
        End Select
    End Function

End Class