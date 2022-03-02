Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports made4net.Shared.Conversion.Convert
Imports Made4Net.Shared
Imports WMS.Logic
Imports WMS.Lib

Public Class AssemblyWorkOrder
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEWorkOrder As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEWorkOrderSelecttLoads As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable

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
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "executeorder"
                ExecuteOrder(ds, UserId)
                Message = "Work Order Executed"
        End Select
    End Sub

#End Region

#Region "Ctor Functions"

    Private Sub ExecuteOrder(ByVal ds As DataSet, ByVal UserId As String)
        Dim wo As New WorkOrderHeader(ReplaceDBNull(ds.Tables(0).Rows(0)("consignee")), ReplaceDBNull(ds.Tables(0).Rows(0)("orderid")))
        Dim loadUom, loadStatus As String
        Dim qtyToProduce As Int32
        loadUom = ds.Tables(0).Rows(0)("LOADUOM")
        loadStatus = ds.Tables(0).Rows(0)("STATUS")
        qtyToProduce = ds.Tables(0).Rows(0)("UNITS")
        Dim oLoadColl As New WMS.Logic.LoadsCollection
        For Each dr As DataRow In ds.Tables(0).Rows
            oLoadColl.Add(New WMS.Logic.Load(dr("loadid"), True))
        Next
        Dim drWOSku As DataRow = ds.Tables(0).Rows(0)
        drWOSku("SKU") = wo.Sku
        drWOSku("CONSIGNEE") = wo.Consignee
        Dim oAttCol As AttributesCollection = WMS.Logic.SkuClass.ExtractReceivingAttributes(drWOSku)
        wo.Assemble(qtyToProduce, oLoadColl, oAttCol, loadUom, loadStatus, UserId)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEWorkOrderSelecttLoads_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWorkOrderSelecttLoads.CreatedChildControls
        With TEWorkOrderSelecttLoads.ActionBar
            '.AddExecButton("ExecuteOrder", "Execute Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
            'With .Button("ExecuteOrder")
            '    .ToolTip = "Execute Work Order"
            '    .Visible = True
            '    .ObjectDLL = "WMS.WebApp.dll"
            '    .ObjectName = "WMS.WebApp.WorkCenter"
            '    .CommandName = "ExecuteOrder"
            '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            '    .ConfirmRequired = True
            '    .ConfirmMessage = "Are you sure you want to Execute the Work Order?"
            '    .EnabledInMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            'End With
            'If Not .Button("Save") Is Nothing Then .Button("Save").Visible = False
            'If Not .Button("Find") Is Nothing Then .Button("Find").Visible = False
            'If Not .Button("Search") Is Nothing Then .Button("Search").Visible = False
            'If Not .Button("Delete") Is Nothing Then .Button("Delete").Visible = False
            'If Not .Button("Cancel") Is Nothing Then .Button("Cancel").Visible = False
            With .Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.AssemblyWorkOrder"
                .CommandName = "ExecuteOrder"
                '.Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
        Dim nv As New System.Collections.Specialized.NameValueCollection
        nv.Add("SKU", TEWorkOrder.SelectedRecordPrimaryKeyValues().Get("SKU"))
        TEWorkOrderSelecttLoads.PreDefinedValues = nv
    End Sub

End Class
