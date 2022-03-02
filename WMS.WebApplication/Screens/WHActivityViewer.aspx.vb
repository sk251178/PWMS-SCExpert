Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Made4Net.LayoutViewer.GoWeb
Imports Made4Net.LayoutViewer
Imports Made4Net.DataAccess
Imports WMS.Logic

Public Class WHActivityViewer
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
    <CLSCompliant(False)> Protected WithEvents DV As Made4Net.LayoutViewer.UI.DiagramViewer

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim TemplatesArrayList As New ArrayList
        LoadTemplates(TemplatesArrayList)
        Dim TemplateArr(TemplatesArrayList.Count - 1) As ObjectTemplateParams
        For i As Int32 = 0 To TemplatesArrayList.Count - 1
            TemplateArr(i) = TemplatesArrayList.Item(i)
        Next
        Dim BackgroundImage As String
        Dim GridXRatio As Single
        Dim GridYRatio As Single
        Dim GridXOffset As Single
        Dim GridYOffset As Single
        Dim LocDt As New DataTable
        Dim dataMaxXval, dataMinXVal, dataMaxYval, dataMinYVal As Decimal
        If Warehouse.CurrentWarehouseAreaLayoutBackgroundImage <> String.Empty Then
            BackgroundImage = Warehouse.CurrentWarehouseAreaLayoutBackgroundImage
        Else
            BackgroundImage = Warehouse.WarehouseLayoutBackgroundImage
        End If
        GridXRatio = Warehouse.CurrentWHAreaLayoutXRatio
        GridYRatio = Warehouse.CurrentWHAreaLayoutYRatio
        GridXOffset = Warehouse.CurrentWHAreaLayoutXOffset
        GridYOffset = Warehouse.CurrentWHAreaLayoutYOffset
        Dim SQL As String = String.Format("select warehousearea,Min(xcoordinate) as MinX, Max(xcoordinate) as MaxX, Min(ycoordinate) as MinY, Max(ycoordinate) as MaxY from location where warehousearea='{0}' group by warehousearea", Warehouse.CurrentWarehouseArea)
        DataInterface.FillDataset(SQL, LocDt)
        If LocDt.Rows.Count >= 1 Then
            dataMaxXval = LocDt.Rows(0)("MaxX")
            dataMinXVal = LocDt.Rows(0)("MinX")
            dataMaxYval = LocDt.Rows(0)("MaxY")
            dataMinYVal = LocDt.Rows(0)("MinY")
        Else
            dataMaxXval = -1
            dataMinXVal = -1
            dataMaxYval = -1
            dataMinYVal = -1
        End If
        DV.DiagramConfigurator = New Made4Net.LayoutViewer.DiagramConfigurator(TemplateArr, BackgroundImage, GridXRatio, GridYRatio, GridXOffset, GridYOffset, dataMinXVal, dataMaxXval, dataMinYVal, dataMaxYval, DV.ShowLegend)
        DV.DataLoader = New WMS.Logic.WHActivityViewerDataLoader
    End Sub

    Private Sub LoadTemplates(ByRef TemplatesArray As ArrayList)
        LoadActivityTypeTemplates(TemplatesArray)
        LoadLocationsTemplates(TemplatesArray)
    End Sub

    Private Sub LoadActivityTypeTemplates(ByRef TemplatesArray As ArrayList)
        Dim SQL As String = String.Format("select distinct TemplateName,TemplateWidth,TemplateHeight from vWHActivityViewer")
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            If Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TemplateName"), "")) <> "" Then
                TemplatesArray.Add(New ObjectTemplateParams(ObjectType.DynamicObject, dr("TemplateName"), dr("TemplateWidth"), dr("TemplateHeight"), HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL(dr("TemplateName")))))
            End If
        Next
        'Load a default template
        'TemplatesArray.Add(New ObjectTemplateParams(ObjectType.DynamicObject, "DefaultDynamic", 0.0F, 0.0F, HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL("WHUser_Default"))))
    End Sub

    Private Sub LoadLocationsTemplates(ByRef TemplatesArray As ArrayList)
        Dim SQL As String = String.Format("select distinct TemplateName,TemplateWidth,TemplateHeight from vWHLocationsViewer")
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            If Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TemplateName"), "")) <> "" Then
                TemplatesArray.Add(New ObjectTemplateParams(ObjectType.StaticObject, dr("TemplateName"), dr("TemplateWidth"), dr("TemplateHeight"), HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL(dr("TemplateName")))))
            End If
        Next
        'Load a default template
        'TemplatesArray.Add(New ObjectTemplateParams(ObjectType.StaticObject, "DefaultStatic", 0.0F, 0.0F, HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL("Location_Default"))))
    End Sub

End Class
