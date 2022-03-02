Imports ZedGraph
Imports System.Drawing
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.WebControls
Imports Made4Net.Shared.Web
Imports System.Web

'Upgraded from 2.0 to 4.5.1 fr telerik
'Imports Telerik.WebControls
'Imports Telerik.Charting
Imports Telerik.Web.UI
Imports Telerik.Charting

<CLSCompliant(False)> Public Class SLPlans
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents pnlSL As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlAddSubQty As System.Web.UI.WebControls.Panel
    Protected WithEvents TEOutboundOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents ZedGraphWeb1 As ZedGraph.Web.ZedGraphWeb
    Protected WithEvents _GraphTable As Table
    Protected WithEvents pnlAddOrders As System.Web.UI.WebControls.Panel
    Protected WithEvents TEWaveOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents rbSL As System.Web.UI.WebControls.RadioButtonList
    Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
    Protected WithEvents rbShowAll As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbOnlyOccupied As System.Web.UI.WebControls.RadioButton
    Protected WithEvents rbOnlyEmpty As System.Web.UI.WebControls.RadioButton
    Protected WithEvents TEOutboundOrdersStat As Made4Net.WebControls.TableEditor

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region


    Private Const C_SERIES1 As String = "NewOrdersMeasure"
    Private Const C_SERIES2 As String = "WaveAssignedOrdersMeasure"
    Private Const C_SERIES3 As String = "PlannedOrdersMeasure"
    Private Const C_SERIES4 As String = "PickingOrdersMeasure"
    Private Const C_SERIES5 As String = "PickedOrdersMeasure"
    Private Const C_SERIES6 As String = "StagedOrdersMeasure"
    Private Const C_SERIES7 As String = "ShipmentAssignedOrdersMeasure"
    Private Const C_SERIES8 As String = "Free"
    Private _radChart As RadChart

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "assignsl"
                Dim SLSetter As New WMS.Logic.StagingLaneAssignmentSetter
                For Each dr In ds.Tables(0).Rows
                    SLSetter.SetDocumentStagingLane(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, dr("CONSIGNEE"), dr("ORDERID"))
                Next
                Message = ts.Translate("Orders assigned to staging lanes")
            Case "savesl"
                For Each dr In ds.Tables(0).Rows
                    Dim ord As New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                    ord.SetStagingLane(dr("STAGINGLANE"), dr("STAGINGWAREHOUSEAREA"), Nothing, WMS.Logic.GetCurrentUser)
                Next
                Message = ts.Translate("Orders assigned to staging lanes")
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GenerateSLGraphs()
    End Sub

    Private Sub GenerateSLGraphs()
        _GraphTable = New Table
        _GraphTable.ID = "GRPHTBL" & New Random().Next().ToString()
        _GraphTable.AddRow()
        '_GraphTable.AddedRow.Cells.Add(CreateGrapgh())
        _GraphTable.AddedRow.Cells.Add(GenerateRadChart())
        Dim ctrlNum As Integer = -1
        For i As Integer = 0 To pnlSL.Controls.Count - 1
            If Me.Controls(i).ID.StartsWith("TBLCEL") Then
                ctrlNum = i
                Exit For
            End If
        Next
        If ctrlNum <> -1 Then
            Me.Controls.RemoveAt(ctrlNum)
        End If
        pnlSL.Controls.Add(_GraphTable)
    End Sub

    Private Sub rbSL_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSL.SelectedIndexChanged
        GenerateSLGraphs()
    End Sub

    Private Function BuildWhereCondition() As String
        Dim SQL As String
        If rbShowAll.Checked Then
            'Do Nothing
        ElseIf rbOnlyEmpty.Checked Then
            SQL = String.Format(" Where NewOrdersMeasure = 0 and PlannedOrdersMeasure = 0 and PickingOrdersMeasure = 0 and PickedOrdersMeasure = 0" & _
                " and WaveAssignedOrdersMeasure = 0 and ShipmentAssignedOrdersMeasure = 0 and StagedOrdersMeasure = 0 ")
        Else
            SQL = String.Format(" Where NewOrdersMeasure > 0 or PlannedOrdersMeasure > 0 or PickingOrdersMeasure > 0 or PickedOrdersMeasure > 0" & _
                            " or WaveAssignedOrdersMeasure > 0 or ShipmentAssignedOrdersMeasure > 0 or StagedOrdersMeasure > 0 ")
        End If
        Return SQL
    End Function

#Region "Zed Graph"

    Private Function CreateGrapgh() As TableCell 'As ZedGraph.Web.ZedGraphWeb
        Dim tblCell As New TableCell
        Dim oTempGrapgh As New ZedGraph.Web.ZedGraphWeb
        AddHandler oTempGrapgh.RenderGraph, AddressOf RenderGraph
        Dim SQL As String = String.Format("Select count(1) from vSLPlanner {0}", BuildWhereCondition)
        Dim cnt As Int32 = DataInterface.ExecuteScalar(SQL)
        oTempGrapgh.Height = cnt * 43
        oTempGrapgh.Width = 250
        tblCell.Controls.Add(oTempGrapgh)
        Return tblCell
    End Function

    Protected Sub RenderGraph(ByVal z As ZedGraph.Web.ZedGraphWeb, ByVal g As System.Drawing.Graphics, ByVal masterPane As ZedGraph.MasterPane)

        ' Get the GraphPane so we can work with it
        Dim myPane As GraphPane = masterPane(0)
        ' Set the title and axis labels
        myPane.Title.Text = "Staging Lane Usage"
        myPane.YAxis.Title.Text = "Staging Lanes"
        myPane.XAxis.Title.Text = "Usage (%)"

        ' Make up some data points
        Dim SQL As String = String.Format("Select * from vSLPlanner {0} order by location desc", BuildWhereCondition)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        Dim cnt As Int32 = dt.Rows.Count
        Dim strSL As String
        Dim idx As Int32 = 0
        Dim dNew(cnt), dPicking(cnt), dPicked(cnt), dWassigned(cnt), dSAssigned(cnt), dStaged(cnt), dPlanned(cnt), dEmpty(cnt) As Double
        For Each dr As DataRow In dt.Rows
            strSL = strSL & "," & dr("location")
            dNew(idx) = dr("NewOrdersMeasure") * dr("cubic") / 100
            dPlanned(idx) = dr("PlannedOrdersMeasure") * dr("cubic") / 100
            dPicking(idx) = dr("PickingOrdersMeasure") * dr("cubic") / 100
            dPicked(idx) = dr("PickedOrdersMeasure") * dr("cubic") / 100
            dWassigned(idx) = dr("WaveAssignedOrdersMeasure") * dr("cubic") / 100
            dSAssigned(idx) = dr("ShipmentAssignedOrdersMeasure") * dr("cubic") / 100
            dStaged(idx) = dr("StagedOrdersMeasure") * dr("cubic") / 100
            dEmpty(idx) = dr("cubic") / 100 * (100 - dr("NewOrdersMeasure") - dr("PlannedOrdersMeasure") - dr("PickingOrdersMeasure") - dr("PickedOrdersMeasure") - dr("WaveAssignedOrdersMeasure") - dr("ShipmentAssignedOrdersMeasure") - dr("StagedOrdersMeasure"))
            If dEmpty(idx) < 0 Then dEmpty(idx) = 0
            idx += 1
        Next
        Dim SL() As String = strSL.TrimStart(",".ToCharArray).Split(",")

        Dim myCurve As BarItem = myPane.AddBar("NEW", dNew, Nothing, Color.Salmon)
        myCurve.Bar.Fill = New Fill(Color.Salmon, Color.White, Color.Salmon, 90.0F)
        myCurve = myPane.AddBar("Shipment Assigned", dSAssigned, Nothing, Color.Yellow)
        myCurve.Bar.Fill = New Fill(Color.Yellow, Color.White, Color.Yellow, 90.0F)
        myCurve = myPane.AddBar("Wave Assigned", dWassigned, Nothing, Color.Orange)
        myCurve.Bar.Fill = New Fill(Color.Orange, Color.White, Color.Orange, 90.0F)
        myCurve = myPane.AddBar("Planned", dPlanned, Nothing, Color.SkyBlue)
        myCurve.Bar.Fill = New Fill(Color.SkyBlue, Color.White, Color.SkyBlue, 90.0F)
        myCurve = myPane.AddBar("Picking", dPicking, Nothing, Color.MediumSlateBlue)
        myCurve.Bar.Fill = New Fill(Color.MediumSlateBlue, Color.White, Color.MediumSlateBlue, 90.0F)
        myCurve = myPane.AddBar("Picked", dPicked, Nothing, Color.Blue)
        myCurve.Bar.Fill = New Fill(Color.Blue, Color.White, Color.Blue, 90.0F)
        myCurve = myPane.AddBar("Staged", dStaged, Nothing, Color.Indigo)
        myCurve.Bar.Fill = New Fill(Color.Indigo, Color.White, Color.Indigo, 90.0F)
        myCurve = myPane.AddBar("Available", dEmpty, Nothing, Color.Green)
        myCurve.Bar.Fill = New Fill(Color.Green, Color.White, Color.Green, 90.0F)

        ' Draw the Y tics between the labels instead of at the labels
        myPane.YAxis.MajorTic.IsBetweenLabels = False
        ' Set the YAxis labels
        myPane.YAxis.Scale.TextLabels = SL
        ' Set the YAxis to Text type
        myPane.YAxis.Type = AxisType.Text
        ' Set the bar type to stack, which stacks the bars by automatically accumulating the values
        myPane.BarSettings.Type = BarType.Stack
        ' Make the bars horizontal by setting the BarBase to "Y"
        myPane.BarSettings.Base = BarBase.Y
        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, _
            Color.FromArgb(247, 247, 255), 45.0F)
        masterPane.AxisChange(g)
        'BarItem.CreateBarLabels(myPane, True, "f0")
    End Sub

#End Region

#Region "Rad Charts OLD"

    'Public Function GenerateRadChart() As TableCell
    '    Dim tblCell As New TableCell
    '    Dim Chart As New RadChart
    '    SetChartOptions(Chart)
    '    Dim SQL As String = String.Format("Select * from vSLPlanner {0} order by location desc", BuildWhereCondition)
    '    Dim dt As New DataTable
    '    DataInterface.FillDataset(SQL, dt)
    '    BindToData(Chart, dt)
    '    tblCell.Controls.Add(Chart)
    '    Return tblCell
    'End Function

    'Protected Sub SetChartOptions(ByVal C As RadChart)
    '    With C
    '        .SeriesOrientation = ChartSeriesOrientation.Horizontal
    '        .UseSession = True
    '        .TempImagesFolder = Made4Net.Shared.Web.MapVirtualPath("ChartTemp")
    '        .EnableViewState = False
    '        With .ChartTitle
    '            .Text = "Staging Lane Usage"
    '            .Visible = True
    '            .TextFont = New Font("Arial", 12)
    '            .TextColor = System.Drawing.Color.Navy
    '        End With
    '        With .XAxis
    '            With .Label
    '                .Text = "Usage (%)"
    '                .Visible = True
    '                With .Background
    '                    .MainColor = Color.Transparent
    '                    .BorderColor = Color.Transparent
    '                End With
    '            End With
    '            .ShowLabels = True
    '            .Visible = True
    '            .DefaultItemFont = New Font("Arial", 8)
    '            .AutoScale = False
    '            .Clear()
    '        End With
    '        With .YAxis
    '            .DefaultItemFont = New Font("Arial", 8)
    '            With .Label
    '                .Text = "Staging Lanes"
    '                .TextFont = New Font("Arial", 8)
    '                .Visible = True
    '            End With
    '            .AutoScale = True
    '        End With
    '        With .Gridlines.HorizontalGridlines
    '            .Visible = True
    '            .Color = System.Drawing.Color.SeaShell
    '        End With
    '        With .Background
    '            .MainColor = Color.FromArgb(198, 219, 255)
    '            .SecondColor = Color.FromArgb(198, 219, 255)
    '            .GradientFocus = 1
    '        End With
    '        With .PlotArea
    '            .MainColor = Color.FromArgb(198, 219, 255)
    '            .SecondColor = Color.FromArgb(198, 219, 255)
    '            .BorderColor = Color.FromArgb(198, 219, 255)
    '            .GradientFillStyle = GradientFillStyle.Horizontal
    '            .FillStyle = FillStyle.Solid
    '        End With
    '        With .Legend
    '            .ItemMark = ChartLegendItemMark.Circle
    '            .ItemFont = New Font("Arial", 8)
    '            .VSpacing = 20
    '            With .Background
    '                .MainColor = Color.Ivory
    '                .BorderColor = Color.Gray
    '                .GradientFocus = 1
    '            End With
    '            .Location = ChartLocation.OutsidePlotArea
    '            .Visible = False
    '        End With
    '        .BarWidthPercent = 30
    '        .DefaultType = ChartSeriesType.StackedBar
    '    End With
    'End Sub

    'Protected Sub BindToData(ByVal C As RadChart, ByVal Data As System.Data.DataTable)
    '    Dim DR As DataRow
    '    Dim CurrX As String
    '    Dim arrSeries As New ArrayList
    '    Dim SeriesName As String
    '    Dim SeriesColor As System.Drawing.Color
    '    Dim arrXVals As New ArrayList
    '    Dim Rand As New Random
    '    SeriesColor = System.Drawing.Color.Green

    '    For Each DR In Data.Rows
    '        Dim Total As Double = 0
    '        CurrX = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DR("Location"), "N/A")
    '        SeriesName = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DR("Location"), "N/A")
    '        Dim s As ChartSeries
    '        s = CreateSeries(C, SeriesName, SeriesColor)
    '        For Each DC As DataColumn In Data.Columns
    '            If DC.ColumnName.ToLower.IndexOf("ordersmeasure") <> -1 Then
    '                Dim i As New ChartSeriesItem
    '                With i
    '                    .YValue = DR(DC.ColumnName) * DR("cubic") / 100
    '                    Total += .YValue
    '                    .Name = CurrX
    '                    With .Appearance
    '                        .FillStyle = FillStyle.Gradient
    '                        .GradientFillStyle = GradientFillStyle.Horizontal
    '                        .MainColor = GetStatusColor(DC.ColumnName)
    '                    End With
    '                End With
    '                s.Items.Add(i)
    '            End If
    '        Next
    '        'Dim empty As New ChartSeriesItem
    '        'With empty
    '        '    .YValue = (100 - Total) * DR("cubic") / 100
    '        '    .Name = CurrX
    '        '    With .Appearance
    '        '        .FillStyle = FillStyle.Gradient
    '        '        .GradientFillStyle = GradientFillStyle.Horizontal
    '        '        .MainColor = System.Drawing.Color.Green
    '        '    End With
    '        'End With
    '        's.Items.Add(empty)
    '        C.XAxis.AddItem(CurrX)
    '    Next
    '    C.Width = System.Web.UI.WebControls.Unit.Pixel(400)
    '    C.Height = System.Web.UI.WebControls.Unit.Pixel(Data.Rows.Count * 25)
    'End Sub

    'Private Function CreateSeries(ByRef Chart As RadChart, ByVal SeriesName As String, ByVal Color As System.Drawing.Color) As ChartSeries
    '    Dim s As ChartSeries
    '    s = Chart.CreateSeries(SeriesName, Color, ChartSeriesType.StackedBar)
    '    With s
    '        .LegendDisplayMode = ChartSeriesLegendDisplayMode.Nothing
    '        .LineWidth = 100
    '        .ShowLabels = False
    '        With .Appearance
    '            .SecondColor = Color.White
    '            .BorderColor = Color.Black
    '        End With
    '        With .LabelAppearance.Background
    '            .MainColor = Color.Transparent
    '            .BorderColor = Color.Transparent
    '        End With
    '    End With
    '    Return s
    'End Function

    'Private Function CreateLegend(ByVal Data As System.Data.DataTable) As ChartLegend
    '    Dim l As New ChartLegend
    '    For Each DC As DataColumn In Data.Columns
    '        If DC.ColumnName.ToLower.IndexOf("ordersmeasure") <> -1 Then
    '            Dim tmpLegItem As New ChartLegendItem
    '            tmpLegItem.Name = DC.ColumnName
    '            tmpLegItem.ItemMark = ChartLegendItemMark.Circle
    '            l.Items.Add(tmpLegItem)
    '        End If
    '    Next
    '    Return l
    'End Function

    'Private Function GetStatusColor(ByVal pStatus As String) As System.Drawing.Color
    '    Select Case pStatus.ToLower
    '        Case "newordersmeasure"
    '            Return Color.Salmon
    '        Case "plannedordersmeasure"
    '            Return Color.SkyBlue
    '        Case "pickingordersmeasure"
    '            Return Color.Tomato
    '        Case "pickedordersmeasure"
    '            Return Color.Blue
    '        Case "stagedordersmeasure"
    '            Return Color.Indigo
    '        Case "waveassignedordersmeasure"
    '            Return Color.Orange
    '        Case "shipmentassignedordersmeasure"
    '            Return Color.Yellow
    '        Case Else
    '            Return Color.Green
    '    End Select
    'End Function

#End Region

#Region "Rad Charts"

    Public Function GenerateRadChart() As TableCell
        Dim tblCell As New TableCell
        tblCell.ID = "TBLCEL" & New Random().Next().ToString()
        _radChart = New RadChart()
        _radChart.ID = "RadChart" + New Random().Next().ToString()
        SetChartOptions(_radChart)
        Dim SQL As String = String.Format("Select * from vSLPlanner {0} order by location desc", BuildWhereCondition)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        BindToData(_radChart, dt)

        'Dim t As New Made4Net.Shared.Translation.Translator()
        'For Each i As ChartLabel In Chart.Legend.Items
        '    i.TextBlock.Text = t.Translate(i.TextBlock.Text)
        'Next
        tblCell.Controls.Add(_radChart)
        '_radChart.Dispose()
        Return tblCell
    End Function

    Protected Sub SetChartOptions(ByVal C As RadChart)
        With C
            .SeriesOrientation = ChartSeriesOrientation.Horizontal
            .UseSession = True
            .TempImagesFolder = Made4Net.Shared.Web.MapVirtualPath("ChartTemp")
            .EnableViewState = False
            With .ChartTitle.TextBlock
                .Text = "Staging Lane Usage"
                .Visible = True
                .Appearance.TextProperties.Font = New Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel, Nothing)
                .Appearance.TextProperties.Color = System.Drawing.Color.Navy
                .Appearance.Position.AlignedPosition = Styles.AlignedPositions.None
                .Appearance.Position.Y = 0
                .Appearance.Position.X = 0
                .Appearance.Position.Auto = False
            End With
            .ChartTitle.Appearance.Position.X = 30
            .ChartTitle.Appearance.Position.Y = 20
            .ChartTitle.Appearance.Position.AlignedPosition = Styles.AlignedPositions.None
            .ChartTitle.Appearance.Position.Auto = False
            With .PlotArea.XAxis
                With .AxisLabel
                    .TextBlock.Text = "Staging Lanes"
                    .Visible = True
                    .TextBlock.Appearance.FillStyle.MainColor = Color.Transparent
                    .TextBlock.Appearance.Border.Color = Color.Transparent
                End With
                .Appearance.LabelAppearance.Visible = True
                .Visible = True
                .Appearance.TextAppearance.TextProperties.Font = New Font("Arial", 10)
                .AutoScale = False
                .Clear()
            End With
            With .PlotArea.YAxis
                With .AxisLabel
                    .TextBlock.Text = "Usage (%)"
                    .TextBlock.Appearance.TextProperties.Font = New Font("Arial", 10)
                    .Visible = True
                End With
                .AutoScale = True
                .Appearance.TextAppearance.TextProperties.Font = New Font("Arial", 10)
                With .Appearance.MajorGridLines
                    .Visible = True
                    .Color = System.Drawing.Color.SeaShell
                End With
            End With
            With .Appearance.FillStyle
                .MainColor = Color.FromArgb(198, 219, 255)
                .SecondColor = Color.FromArgb(198, 219, 255)
                '.GradientFocus = 1
            End With
            With .PlotArea.Appearance.FillStyle
                .MainColor = Color.FromArgb(198, 219, 255)
                .SecondColor = Color.FromArgb(198, 219, 255)
                .FillSettings.GradientMode = Styles.GradientFillStyle.Horizontal ' = GradientFillStyle.Horizontal
                .FillType = Styles.FillType.Solid
            End With
            With .PlotArea.Appearance.Border
                .Color = Color.FromArgb(198, 219, 255)
            End With
            With .Legend
                Dim t As New Made4Net.Shared.Translation.Translator()
                Dim tmpLegItem As New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES1)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES1)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES2)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES2)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES3)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES3)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES4)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES4)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES5)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES5)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES6)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES6)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES7)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES7)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)

                tmpLegItem = New LabelItem
                tmpLegItem.TextBlock.Text = t.Translate(C_SERIES8)
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES8)
                tmpLegItem.Marker.Appearance.FillStyle.FillType = Styles.FillType.Solid
                .Items.Add(tmpLegItem)
                '''''''''''''''''.Appearance.ItemMarkerAppearance.Figure =
                '.ItemMark = ChartLegendItemMark.Circle
                '.ItemFont = New Font("Arial", 8)
                '.VSpacing = 20
                With .Appearance.FillStyle ' .Background
                    .MainColor = Color.Ivory
                    ''''''''''''''''''.FillSettings.compl
                End With
                With .Appearance.Border
                    '.BorderColor = Color.Gray
                    .Color = Color.Gray
                    '.GradientFocus = 1
                End With
                '.Location = ChartLocation.OutsidePlotArea
                .Visible = True
                .Appearance.Location = Styles.LabelLocation.OutsidePlotArea
                .Appearance.Position.AlignedPosition = Styles.AlignedPositions.None
                .Appearance.Position.Auto = False
                .Appearance.Position.Y = 20
                .Appearance.Position.X = 525
            End With
            .Appearance.BarWidthPercent = 30
            .DefaultType = ChartSeriesType.StackedBar
            With .PlotArea
                .Appearance.Dimensions.Margins.Bottom.Type = Styles.UnitType.Pixel
                .Appearance.Dimensions.Margins.Bottom.Value = 55

                .Appearance.Dimensions.Margins.Left.Type = Styles.UnitType.Pixel
                .Appearance.Dimensions.Margins.Left.Value = 85

                .Appearance.Dimensions.Margins.Right.Type = Styles.UnitType.Pixel
                .Appearance.Dimensions.Margins.Right.Value = 230

                .Appearance.Dimensions.Margins.Top.Type = Styles.UnitType.Pixel
                .Appearance.Dimensions.Margins.Top.Value = 50
            End With


            '.Appearance.Dimensions.Margins.Left = 10
            '.Appearance.Dimensions.Margins.Top = 20
        End With
    End Sub

    Protected Sub BindToData(ByVal C As RadChart, ByVal Data As System.Data.DataTable)
        Dim DR As DataRow
        Dim arrSeries As New ArrayList
        Dim SeriesColor As System.Drawing.Color
        Dim arrXVals As New ArrayList
        Dim Rand As New Random
        SeriesColor = System.Drawing.Color.Green


        'C.PlotArea.XAxis.AutoScale = False
        'C.PlotArea.XAxis.Items.Add(New ChartAxisItem("a"))
        'C.PlotArea.XAxis.Items.Add(New ChartAxisItem("b"))
        'C.PlotArea.XAxis.Items.Add(New ChartAxisItem("c"))
        'Dim series1 As New ChartSeries()
        'series1.Type = ChartSeriesType.StackedBar100
        'series1.Appearance.FillStyle.MainColor = Color.Blue
        'Dim item As ChartSeriesItem
        'item = New ChartSeriesItem()
        'item.Name = "a"
        'item.YValue = 20
        'item.Label.TextBlock.Text = ""
        'series1.Items.Add(item)
        'item = New ChartSeriesItem()
        'item.Name = "b"
        'item.YValue = 30
        'item.Label.TextBlock.Text = ""
        'item = New ChartSeriesItem()
        'item.Name = "c"
        'item.YValue = 50
        'item.Label.TextBlock.Text = ""
        'series1.Items.Add(item)

        'Dim series2 As New ChartSeries()
        'series2.Type = ChartSeriesType.StackedBar100
        'series2.Appearance.FillStyle.MainColor = Color.Yellow
        'item = New ChartSeriesItem()
        'item.Name = "a"
        'item.YValue = 10
        'item.Label.TextBlock.Text = ""
        'series2.Items.Add(item)
        'item = New ChartSeriesItem()
        'item.Name = "b"
        'item.YValue = 15
        'item.Label.TextBlock.Text = ""
        'item = New ChartSeriesItem()
        'item.Name = "c"
        'item.YValue = 25
        'item.Label.TextBlock.Text = ""
        'series2.Items.Add(item)

        'C.Series.Add(series1)
        'C.Series.Add(series2)

        'Return

        Dim seriesByNameDict As New System.Collections.Generic.Dictionary(Of String, ChartSeries)

        Dim s1 As ChartSeries = CreateSeries(C_SERIES1, GetStatusColor(C_SERIES1))
        seriesByNameDict.Add(C_SERIES1.ToLower(), s1)

        Dim s2 As ChartSeries = CreateSeries(C_SERIES2, GetStatusColor(C_SERIES2))
        seriesByNameDict.Add(C_SERIES2.ToLower(), s2)

        Dim s3 As ChartSeries = CreateSeries(C_SERIES3, GetStatusColor(C_SERIES3))
        seriesByNameDict.Add(C_SERIES3.ToLower(), s3)

        Dim s4 As ChartSeries = CreateSeries(C_SERIES4, GetStatusColor(C_SERIES4))
        seriesByNameDict.Add(C_SERIES4.ToLower(), s4)

        Dim s5 As ChartSeries = CreateSeries(C_SERIES5, GetStatusColor(C_SERIES5))
        seriesByNameDict.Add(C_SERIES5.ToLower(), s5)

        Dim s6 As ChartSeries = CreateSeries(C_SERIES6, GetStatusColor(C_SERIES6))
        seriesByNameDict.Add(C_SERIES6.ToLower(), s6)

        Dim s7 As ChartSeries = CreateSeries(C_SERIES7, GetStatusColor(C_SERIES7))
        seriesByNameDict.Add(C_SERIES7.ToLower(), s7)

        Dim s8 As ChartSeries = CreateSeries(C_SERIES8, GetStatusColor(C_SERIES8))
        seriesByNameDict.Add(C_SERIES8.ToLower(), s8)

        'CurrX = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Data.Rows(0)("Location"), "N/A")
        'SeriesName = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(Data.Rows(0)("Location"), "N/A")
        Dim total As Double
        Dim csi As ChartSeriesItem
        C.PlotArea.XAxis.AutoScale = False
        Dim cai As ChartAxisItem

        For Each DR In Data.Rows
            cai = New ChartAxisItem(DR("LOCATION").ToString())
            cai.Appearance.Dimensions.Width.Type = Styles.UnitType.Pixel
            cai.Appearance.Dimensions.Width.Value = 100
            C.PlotArea.XAxis.Items.Add(cai)
            total = 0
            For Each DC As DataColumn In Data.Columns
                If DC.ColumnName.ToLower.IndexOf("ordersmeasure") <> -1 Then
                    'If DR(DC.ColumnName) > 0 Then
                    csi = New ChartSeriesItem()
                    csi.Name = DC.ColumnName
                    csi.YValue = DR(DC.ColumnName)
                    csi.Label.TextBlock.Text = ""
                    seriesByNameDict(DC.ColumnName.ToLower()).Items.Add(csi)
                    total = total + DR(DC.ColumnName)
                    'End If
                End If
            Next
            If total < 100 Then
                csi = New ChartSeriesItem()
                csi.Name = "FREE"
                csi.YValue = 100 - total
                csi.Label.TextBlock.Text = ""
                seriesByNameDict("free").Items.Add(csi)
            End If
        Next
        For Each cs As ChartSeries In seriesByNameDict.Values
            cs.Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.Nothing
            C.Series.Add(cs)
        Next
        C.Width = System.Web.UI.WebControls.Unit.Pixel(750) 'System.Web.UI.WebControls.Unit.Pixel(400)
        C.Height = System.Web.UI.WebControls.Unit.Pixel(Math.Max(100 * Data.Rows.Count, 400))
    End Sub

    Private Function CreateSeries(ByRef Chart As RadChart, ByVal SeriesName As String, ByVal Color As System.Drawing.Color) As ChartSeries
        Dim s As ChartSeries
        s = Chart.CreateSeries(SeriesName, Color, Nothing, ChartSeriesType.StackedBar)
        With s
            .Appearance.LegendDisplayMode = ChartSeriesLegendDisplayMode.ItemLabels
            .Appearance.LineSeriesAppearance.Width = 100
            .Appearance.LabelAppearance.Visible = False
            With .Appearance
                .Border.Color = Color.Black
                .FillStyle.SecondColor = Color.White
            End With
            With .Appearance.LabelAppearance.FillStyle
                .MainColor = Color.Transparent
            End With
            .Appearance.LabelAppearance.Border.Color = Color.Transparent
        End With
        Return s
    End Function

    Private Function CreateSeries(ByVal pSeriesName As String, ByVal pColor As Color) As ChartSeries
        Dim s As New ChartSeries()
        s.Name = pSeriesName
        s.Type = ChartSeriesType.StackedBar100
        s.Appearance.BarWidthPercent = 50
        s.Appearance.LabelAppearance.Position.X = -1
        s.Appearance.LabelAppearance.Position.Y = -1
        s.DefaultLabelValue = ""
        s.Appearance.FillStyle.MainColor = pColor
        s.Appearance.FillStyle.FillType = Styles.FillType.Solid
        s.Appearance.TextAppearance.Position.AlignedPosition = Styles.AlignedPositions.Center
        Return s
    End Function

    Private Function CreateLegend(ByVal Data As System.Data.DataTable) As ChartLegend
        Dim l As New ChartLegend
        For Each DC As DataColumn In Data.Columns
            If DC.ColumnName.ToLower.IndexOf("ordersmeasure") <> -1 Then
                Dim tmpLegItem As New ChartLabel
                tmpLegItem.TextBlock.Text = DC.ColumnName
                tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(DC.ColumnName)
                'tmpLegItem.ItemMark = ChartLegendItemMark.Circle
                l.Items.Add(tmpLegItem)
            End If
        Next
        Return l
    End Function
    Private Function CreateLegend() As ChartLegend
        Dim l As New ChartLegend

        Dim tmpLegItem As New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES1
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES1)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES2
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES2)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES3
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES3)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES4
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES4)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES5
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES5)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES6
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES6)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES7
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES7)
        l.Items.Add(tmpLegItem)

        tmpLegItem = New ChartLabel
        tmpLegItem.TextBlock.Text = C_SERIES8
        tmpLegItem.Marker.Appearance.FillStyle.MainColor = GetStatusColor(C_SERIES8)
        l.Items.Add(tmpLegItem)

        Return l
    End Function

    Private Function GetStatusColor(ByVal pStatus As String) As System.Drawing.Color
        Select Case pStatus.ToLower
            Case "newordersmeasure"
                Return Color.Salmon
            Case "plannedordersmeasure"
                Return Color.SkyBlue
            Case "pickingordersmeasure"
                Return Color.Tomato
            Case "pickedordersmeasure"
                Return Color.Blue
            Case "stagedordersmeasure"
                Return Color.Indigo
            Case "waveassignedordersmeasure"
                Return Color.Orange
            Case "shipmentassignedordersmeasure"
                Return Color.Yellow
            Case Else
                Return Color.Green
        End Select
    End Function



#End Region

    Private Sub TEOutboundOrdersStat_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEOutboundOrdersStat.CreatedChildControls
        With TEOutboundOrdersStat
            With .ActionBar
                .AddSpacer()
                .AddExecButton("AssignSL", "Assign Orders to SL", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("AssignSL")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.SLPlans"
                    .CommandName = "AssignSL"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                End With
                With .Button("Save")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.SLPlans"
                    .CommandName = "savesl"
                End With
            End With
        End With
    End Sub

End Class