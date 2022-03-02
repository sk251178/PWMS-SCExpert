Imports System.Drawing
Imports System.Web.UI

Imports Made4Net.DataAccess
Imports Made4Net.Schema
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared.Strings
Imports Made4Net.DataAccess.Collections
Imports made4net.WebControls

Imports Telerik.WebControls
Imports Telerik.Charting
Imports Telerik.RadChartUtils

<CLSCompliant(False)> _
Public Class RoutesGantt
    Inherits Telerik.WebControls.RadChart

    Private _sql As String
    Private _GanttDataSource As New DataTable
    Private t As New Translation.Translator()

#Region "Public properties"
    Protected _Colors As New ArrayList()

    Public h1 As Integer = 40
    Public h2 As Integer = 300
    Public p1 As Integer = 70
    Public chartWidth As Integer = 800

    Public Property SQL() As String
        Get
            Return _sql
        End Get
        Set(ByVal Value As String)
            _sql = Value
        End Set
    End Property

#End Region

    Sub New()
        SkinManager.LoadColorsPalette(_Colors, "GanttColors")

    End Sub

    Sub New(ByVal pSQL As String)
        _sql = pSQL
        LoadChartGroup()
    End Sub


#Region " Methods"
    Public Sub ReloadChart(ByVal pSQL As String)
        _sql = pSQL
        LoadChartGroup()
    End Sub
    Sub ReloadChart(ByVal pSQL As String, _
              ByVal ph1 As Integer, ByVal ph2 As Integer, ByVal pp1 As Integer)
        _sql = pSQL
        h1 = ph1
        h2 = ph2
        p1 = pp1

        LoadChartGroup()
    End Sub

    Private Sub LoadChart()
        DataInterface.FillDataset(_sql, _GanttDataSource)
        If _GanttDataSource.Rows.Count = 0 Then Return

        ID = "GanttID"

        ChartTitle.TextBlock.Text = t.Translate("Route Gantt")
        ChartTitle.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.TopLeft


        PlotArea.EmptySeriesMessage.TextBlock.Text = "no routes"
        DefaultType = ChartSeriesType.Gantt
        Skin = "WebBlue"
        SeriesOrientation = ChartSeriesOrientation.Horizontal

        ''PlotArea.Appearance.Dimensions.Margins = Telerik.Charting.Styles.ChartMargins.Parse("150px")
        PlotArea.Appearance.Dimensions.Margins.Top = Telerik.Charting.Styles.Unit.Pixel(120)
        PlotArea.Appearance.Dimensions.Margins.Bottom = Telerik.Charting.Styles.Unit.Pixel(50)
        PlotArea.Appearance.Dimensions.Margins.Left = Telerik.Charting.Styles.Unit.Pixel(160)
        PlotArea.Appearance.Dimensions.Margins.Right = Telerik.Charting.Styles.Unit.Pixel(150)
        AutoLayout = False


        PlotArea.XAxis.AxisLabel.TextBlock.Text = "routes"
        PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)



        PlotArea.XAxis.Appearance.MajorGridLines.Visible = False


        PlotArea.YAxis2.Appearance.MajorGridLines.Visible = False
        PlotArea.YAxis.Appearance.MajorGridLines.Visible = True


        PlotArea.YAxis2.Visible = Telerik.Charting.Styles.ChartAxisVisibility.True
        PlotArea.YAxis.Visible = Telerik.Charting.Styles.ChartAxisVisibility.True

        Width = Unit.Pixel(chartWidth)

        PlotArea.XAxis.AxisLabel.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Left
        PlotArea.YAxis2.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Bottom
        PlotArea.YAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Top

        Legend.Visible = True
        Legend.TextBlock.Visible = True
        Legend.TextBlock.Text = "Routes Stops"
        Legend.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)
        Legend.Appearance.Border.Visible = False
        Legend.Appearance.FillStyle.MainColor = Color.Transparent

        Legend.TextBlock.Text = "  Routes"


        RemoveAllSeries()

        ''
        If _GanttDataSource.Rows.Count > 0 Then
            Dim DRC As DataRowCollection = _GanttDataSource.Rows
            Dim currRouteID As String = ""
            Dim csRouteStops As ChartSeries = Nothing

            Dim snum As Integer = 0
            Dim minDate As Integer = System.Convert.ToInt32(DRC(0)("startdates"))
            Dim maxDate As Integer = System.Convert.ToInt32(DRC(DRC.Count - 1)("enddates")) + 60

            Dim recroute As Integer = System.Convert.ToInt32(DRC(0).ItemArray(0).ToString().Replace("R", ""))
            Dim minroute As Integer = recroute
            Dim maxroute As Integer = recroute
            Dim DataHash As New Hashtable()
            Dim counter As Integer = -1
            PlotArea.XAxis.AxisLabel.Visible = True
            PlotArea.XAxis.AutoScale = False
            PlotArea.XAxis.IsZeroBased = False
            PlotArea.XAxis.MinValue = 0
            PlotArea.XAxis.[Step] = 1

            PlotArea.YAxis2.AxisMode = ChartYAxisMode.Extended
            PlotArea.YAxis.AxisMode = ChartYAxisMode.Extended

            For Each dbRow As DataRow In DRC
                If minDate > CInt(dbRow("startdates")) Then
                    minDate = CInt(dbRow("startdates"))
                End If
                If maxDate < CInt(dbRow("actualenddates")) Then
                    maxDate = CInt(dbRow("actualenddates")) + 60
                End If

                If currRouteID <> dbRow("routeid") Then

                    currRouteID = dbRow("routeid").ToString()

                    counter += 3
                    Dim oXCharAxisItemPlanned As New ChartAxisItem(currRouteID & "  planned")
                    oXCharAxisItemPlanned.Value = counter
                    PlotArea.XAxis.Items.Add(oXCharAxisItemPlanned)

                    Dim oXCharAxisItemActual As New ChartAxisItem("actual")
                    oXCharAxisItemActual.Value = counter + 1
                    PlotArea.XAxis.Items.Add(oXCharAxisItemActual)

                    Dim oXCharAxisItemSpacer2 As New ChartAxisItem("spacer", Color.AliceBlue, False)

                    oXCharAxisItemSpacer2.Value = counter + 2
                    PlotArea.XAxis.Items.Add(oXCharAxisItemSpacer2)

                    snum = 0
                    If csRouteStops IsNot Nothing Then
                        Series.Add(csRouteStops)
                    End If
                    csRouteStops = New ChartSeries()
                    csRouteStops.Type = ChartSeriesType.Gantt

                    csRouteStops.Name = dbRow("routeid").ToString()

                    csRouteStops.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.BottomLeft
                    csRouteStops.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                    csRouteStops.Appearance.LegendDisplayMode = Telerik.Charting.ChartSeriesLegendDisplayMode.SeriesName

                    recroute = System.Convert.ToInt32(currRouteID.Replace("R", ""))
                    If recroute > maxroute Then
                        maxroute = recroute
                    End If

                    If recroute < minroute Then
                        minroute = recroute
                    End If

                End If



                snum += 1
                currRouteID = DirectCast(dbRow("routeid"), String)

                Dim ci As New ChartSeriesItem()
                ci.YValue = CInt(dbRow("startdates"))
                If ci.YValue < minDate Then
                    minDate = CInt(dbRow("startdates"))
                End If
                ci.YValue2 = CInt(dbRow("enddates"))
                If ci.YValue2 > maxDate Then
                    maxDate = CInt(dbRow("enddates"))
                End If

                ci.Label.Appearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                ci.Appearance.Border.Color = System.Drawing.Color.Black

                If dbRow("LABEL").ToString() <> "" Then
                    ci.ActiveRegion.Tooltip = currRouteID & " - " & snum & " # " & dbRow("LABEL").ToString()
                End If
                ci.XValue = counter

                csRouteStops.Items.Add(ci)

                ''actual
                Dim ciactual As New ChartSeriesItem()
                ciactual.YValue = CInt(dbRow("actualstartdates"))
                If ciactual.YValue < minDate Then
                    minDate = CInt(dbRow("actualstartdates"))
                End If
                ciactual.YValue2 = CInt(dbRow("actualenddates"))
                If ci.YValue2 > maxDate Then
                    maxDate = CInt(dbRow("actualenddates"))
                End If
                ciactual.Appearance.Border.Color = System.Drawing.Color.Red
                ciactual.Appearance.FillStyle.MainColor = System.Drawing.Color.Green


                ci.Label.Appearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                ci.Label.TextBlock.Text = snum
                ci.Label.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 7)

                ciactual.Label.Appearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                ciactual.Label.TextBlock.Text = snum
                ciactual.Label.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 7)


                ci.Label.Visible = True
                ciactual.Label.Visible = True

                If dbRow("ACTUALLABEL").ToString() <> "" Then
                    ciactual.ActiveRegion.Tooltip = currRouteID & " - " & snum.ToString("D2") & " # " & dbRow("ACTUALLABEL").ToString()
                End If


                ciactual.XValue = counter + 1
                csRouteStops.Items.Add(ciactual)
            Next


            If csRouteStops IsNot Nothing Then
                Series.Add(csRouteStops)
                csRouteStops = New ChartSeries()
                csRouteStops.Type = ChartSeriesType.Gantt
                csRouteStops.Appearance.LegendDisplayMode = Telerik.Charting.ChartSeriesLegendDisplayMode.SeriesName


                ''http://localhost/CRTTrunk49/Screens/RouteStopGantt.aspx?h2=300&h1=70&p1=70

                Appearance.BarWidthPercent = p1

                Appearance.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(h2 + counter * h1)

                minDate = (System.Convert.ToInt32(minDate \ 60) - 1) * 60
                maxDate = (System.Convert.ToInt32(maxDate \ 60) + 1) * 60

                PlotArea.YAxis.[Step] = 60
                PlotArea.YAxis.MinValue = minDate
                PlotArea.YAxis.MaxValue = maxDate

                PlotArea.YAxis.AutoScale = False
                PlotArea.YAxis.AddRange(minDate, maxDate, 60)
                For j As Integer = minDate To maxDate Step 60
                    PlotArea.YAxis((j - minDate) / 60).TextBlock.Text = (PlotArea.YAxis((j - minDate) / 60).Value / 60).ToString() & ":00"
                Next

                PlotArea.YAxis2.[Step] = 60
                PlotArea.YAxis2.MinValue = minDate
                PlotArea.YAxis2.MaxValue = maxDate

                PlotArea.YAxis2.AutoScale = False
                PlotArea.YAxis2.AddRange(minDate, maxDate, 60)
                For j As Integer = minDate To maxDate Step 60
                    PlotArea.YAxis2((j - minDate) / 60).TextBlock.Text = (PlotArea.YAxis2((j - minDate) / 60).Value / 60).ToString() & ":00"
                Next


            End If


        End If
    End Sub

    Private Sub LoadChartGroup()
        DataInterface.FillDataset(_sql, _GanttDataSource)
        If _GanttDataSource.Rows.Count = 0 Then Return

        ID = "GanttID"

        ChartTitle.TextBlock.Text = t.Translate("Route Gantt")
        ChartTitle.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.TopLeft


        PlotArea.EmptySeriesMessage.TextBlock.Text = "no routes"
        DefaultType = ChartSeriesType.Gantt
        Skin = "WebBlue"
        SeriesOrientation = ChartSeriesOrientation.Horizontal

        ''PlotArea.Appearance.Dimensions.Margins = Telerik.Charting.Styles.ChartMargins.Parse("150px")
        PlotArea.Appearance.Dimensions.Margins.Top = Telerik.Charting.Styles.Unit.Pixel(120)
        PlotArea.Appearance.Dimensions.Margins.Bottom = Telerik.Charting.Styles.Unit.Pixel(50)
        PlotArea.Appearance.Dimensions.Margins.Left = Telerik.Charting.Styles.Unit.Pixel(160)
        PlotArea.Appearance.Dimensions.Margins.Right = Telerik.Charting.Styles.Unit.Pixel(50)
        AutoLayout = False


        PlotArea.XAxis.AxisLabel.TextBlock.Text = "routes"
        PlotArea.XAxis.AxisLabel.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)



        PlotArea.XAxis.Appearance.MajorGridLines.Visible = False


        PlotArea.YAxis2.Appearance.MajorGridLines.Visible = False
        PlotArea.YAxis.Appearance.MajorGridLines.Visible = True


        PlotArea.YAxis2.Visible = Telerik.Charting.Styles.ChartAxisVisibility.True
        PlotArea.YAxis.Visible = Telerik.Charting.Styles.ChartAxisVisibility.True

        Width = Unit.Pixel(800)

        PlotArea.XAxis.AxisLabel.Appearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Left
        PlotArea.YAxis2.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Bottom
        PlotArea.YAxis.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.Top

        Legend.Visible = False
        Legend.TextBlock.Visible = True
        Legend.TextBlock.Text = "Routes Stops"
        Legend.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)
        Legend.Appearance.Border.Visible = False
        Legend.Appearance.FillStyle.MainColor = Color.Transparent

        Legend.TextBlock.Text = "  Routes"


        RemoveAllSeries()
        If _GanttDataSource.Rows.Count > 0 Then
            Dim DRC As DataRowCollection = _GanttDataSource.Rows
            Dim currRouteID As String = String.Empty
            Dim currGroup As String = String.Empty
            Dim csRouteStops As ChartSeries = Nothing

            Dim snum As Integer = 0
            Dim minDate As Integer = System.Convert.ToInt32(DRC(0)("startdates"))
            Dim maxDate As Integer = System.Convert.ToInt32(DRC(DRC.Count - 1)("enddates")) + 60

            Dim recroute As Integer = System.Convert.ToInt32(DRC(0).ItemArray(0).ToString().Replace("R", ""))
            Dim minroute As Integer = recroute
            Dim maxroute As Integer = recroute
            Dim DataHash As New Hashtable()
            Dim counter As Integer = 0
            Dim colorcounter As Integer = 0
            PlotArea.XAxis.AxisLabel.Visible = True
            PlotArea.XAxis.AutoScale = False
            PlotArea.XAxis.IsZeroBased = False
            PlotArea.XAxis.MinValue = 0
            PlotArea.XAxis.[Step] = 1

            PlotArea.YAxis2.AxisMode = ChartYAxisMode.Extended
            PlotArea.YAxis.AxisMode = ChartYAxisMode.Extended

            If DRC.Count > 0 Then
                currRouteID = DRC(0)("routeid").ToString()
                currGroup = DRC(0)("seriesgroup").ToString()
            End If


            Dim MainColor As System.Drawing.Color = genexttcolor(colorcounter)

            For Each dbRow As DataRow In DRC

                If minDate > CInt(dbRow("startdates")) Then
                    minDate = CInt(dbRow("startdates"))
                End If
                If maxDate < CInt(dbRow("enddates")) Then
                    maxDate = CInt(dbRow("enddates")) + 60
                End If


                If counter = 0 OrElse _
                    (currRouteID <> dbRow("routeid") OrElse currGroup <> dbRow("seriesgroup")) Then
                    counter += 1

                    If currRouteID <> String.Empty AndAlso currRouteID <> dbRow("routeid") Then
                        counter += 1
                        Dim oXCharAxisItemSpacer As New ChartAxisItem("spacer", Color.AliceBlue, False)
                        oXCharAxisItemSpacer.Value = counter
                        PlotArea.XAxis.Items.Add(oXCharAxisItemSpacer)

                        colorcounter += 1
                        MainColor = genexttcolor(colorcounter)
                    End If

                    Dim oXCharAxisItem As New ChartAxisItem(dbRow("seriesname").ToString)
                    oXCharAxisItem.Value = counter
                    PlotArea.XAxis.Items.Add(oXCharAxisItem)

                    snum = 0
                    If csRouteStops IsNot Nothing Then
                        Series.Add(csRouteStops)
                    End If
                    csRouteStops = New ChartSeries()
                    csRouteStops.Type = ChartSeriesType.Gantt

                    csRouteStops.Name = dbRow("routeid").ToString()

                    csRouteStops.Appearance.LabelAppearance.Position.AlignedPosition = Telerik.Charting.Styles.AlignedPositions.BottomLeft
                    csRouteStops.Appearance.LabelAppearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                    csRouteStops.Appearance.LegendDisplayMode = Telerik.Charting.ChartSeriesLegendDisplayMode.SeriesName

                    recroute = System.Convert.ToInt32(currRouteID.Replace("R", ""))
                    If recroute > maxroute Then
                        maxroute = recroute
                    End If

                    If recroute < minroute Then
                        minroute = recroute
                    End If

                End If
                currRouteID = dbRow("routeid").ToString()
                currGroup = dbRow("seriesgroup").ToString()

                snum += 1

                Dim ci As New ChartSeriesItem()
                ci.YValue = CInt(dbRow("startdates"))
                If ci.YValue < minDate Then
                    minDate = CInt(dbRow("startdates"))
                End If
                ci.YValue2 = CInt(dbRow("enddates"))
                If ci.YValue2 > maxDate Then
                    maxDate = CInt(dbRow("enddates"))
                End If

                ci.Label.Appearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                ci.Appearance.Border.Color = System.Drawing.Color.Black

                If dbRow("LABEL").ToString() <> "" Then
                    ci.ActiveRegion.Tooltip = dbRow("LABEL").ToString()
                End If
                ci.XValue = counter

                csRouteStops.Items.Add(ci)


                ci.Label.Appearance.LabelLocation = Telerik.Charting.Styles.StyleSeriesItemLabel.ItemLabelLocation.Outside
                ci.Label.TextBlock.Text = snum
                ci.Label.TextBlock.Appearance.TextProperties.Font = _
                    New System.Drawing.Font(FontFamily.GenericSansSerif, 7)
                ci.Label.Visible = True

                ci.Appearance.FillStyle.MainColor = MainColor
            Next


            If csRouteStops IsNot Nothing Then
                Series.Add(csRouteStops)
                csRouteStops = New ChartSeries()
                csRouteStops.Type = ChartSeriesType.Gantt
                csRouteStops.Appearance.LegendDisplayMode = Telerik.Charting.ChartSeriesLegendDisplayMode.SeriesName


                ''http://localhost/CRTTrunk49/Screens/RouteStopGantt.aspx?h2=300&h1=70&p1=30

                Appearance.BarWidthPercent = p1

                Appearance.Dimensions.Height = Telerik.Charting.Styles.Unit.Pixel(h2 + counter * h1)

                minDate = (System.Convert.ToInt32(minDate \ 60) - 1) * 60
                maxDate = (System.Convert.ToInt32(maxDate \ 60) + 1) * 60

                PlotArea.YAxis.[Step] = 60
                PlotArea.YAxis.MinValue = minDate
                PlotArea.YAxis.MaxValue = maxDate

                PlotArea.YAxis.AutoScale = False
                PlotArea.YAxis.AddRange(minDate, maxDate, 60)
                For j As Integer = minDate To maxDate Step 60
                    PlotArea.YAxis((j - minDate) / 60).TextBlock.Text = (PlotArea.YAxis((j - minDate) / 60).Value / 60).ToString() & ":00"
                Next

                PlotArea.YAxis2.[Step] = 60
                PlotArea.YAxis2.MinValue = minDate
                PlotArea.YAxis2.MaxValue = maxDate

                PlotArea.YAxis2.AutoScale = False
                PlotArea.YAxis2.AddRange(minDate, maxDate, 60)
                For j As Integer = minDate To maxDate Step 60
                    PlotArea.YAxis2((j - minDate) / 60).TextBlock.Text = (PlotArea.YAxis2((j - minDate) / 60).Value / 60).ToString() & ":00"
                Next


            End If


        End If
    End Sub

    Protected Function genexttcolor(ByVal i As Integer) As System.Drawing.Color
        Return _Colors(i Mod _Colors.Count)
    End Function


#End Region



End Class
