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

Public Class YardViewer
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    <CLSCompliant(False)> Protected WithEvents DV As Made4Net.LayoutViewer.UI.DiagramViewer
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim TemplatesArrayList As New ArrayList
        LoadTemplates(TemplatesArrayList)
        Dim TemplateArr(TemplatesArrayList.Count - 1) As ObjectTemplateParams
        For i As Int32 = 0 To TemplatesArrayList.Count - 1
            TemplateArr(i) = TemplatesArrayList.Item(i)
        Next
        DV.DiagramConfigurator = New Made4Net.LayoutViewer.DiagramConfigurator(TemplateArr, Warehouse.YardLayoutBackgroundImage, DV.GridXRatio, DV.GridYRatio, DV.GridXOffset, DV.GridYOffset, -1, -1, -1, -1, DV.ShowLegend)
        DV.DataLoader = New WMS.Logic.YardLayoutDataLoader
    End Sub

    Private Sub LoadTemplates(ByRef TemplatesArray As ArrayList)
        LoadVehiclesTemplates(TemplatesArray)
        LoadLocationsTemplates(TemplatesArray)
    End Sub

    Private Sub LoadVehiclesTemplates(ByRef TemplatesArray As ArrayList)
        Dim SQL As String = String.Format("select distinct TemplateName,TemplateWidth,TemplateHeight from vYardVehiclesPosition")
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            If Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TemplateName"), "")) <> "" Then
                TemplatesArray.Add(New ObjectTemplateParams(ObjectType.DynamicObject, dr("TemplateName"), dr("TemplateWidth"), dr("TemplateHeight"), HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL(dr("TemplateName")))))
            End If
        Next
        'Load a default template
        'TemplatesArray.Add(New ObjectTemplateParams(ObjectType.DynamicObject, "DefaultDynamic", 25.0F, 75.0F, HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL("Vehicle_Default"))))
    End Sub

    Private Sub LoadLocationsTemplates(ByRef TemplatesArray As ArrayList)
        Dim SQL As String = String.Format("select distinct TemplateName,TemplateWidth,TemplateHeight from vYardLocations")
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim LocationType As String = "YardLocation_"
            If Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("templatename"), "")) <> "" Then
                LocationType &= dr("templatename")
                TemplatesArray.Add(New ObjectTemplateParams(ObjectType.StaticObject, dr("templatename"), dr("TemplateWidth"), dr("TemplateHeight"), HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL(dr("TemplateName")))))
            End If
        Next
        'Load a default template
        'TemplatesArray.Add(New ObjectTemplateParams(ObjectType.StaticObject, "DefaultStatic", 25.0F, 25.0F, HttpContext.Current.Request.MapPath(Made4Net.WebControls.SkinManager.GetImageURL("YardLocation_Default"))))
    End Sub

End Class
