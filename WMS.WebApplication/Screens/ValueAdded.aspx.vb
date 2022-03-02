Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports made4net.Shared.Conversion.Convert
Imports Made4Net.Shared
Imports WMS.Logic
Imports WMS.Lib

Public Class ValueAdded
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEWorkOrder As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEValueAddedLoads As Made4Net.WebControls.TableEditor
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
        If ds.Tables(0).Rows.Count = 0 Then
            Throw New M4NException(New Exception, "No load was selected.", "No load was selected.")
        End If
        Dim wo As New WMS.Logic.WorkOrderHeader(ReplaceDBNull(ds.Tables(0).Rows(0)("WOCONSIGNEE")), ReplaceDBNull(ds.Tables(0).Rows(0)("WOORDERID")))
        Dim status As String, qty As Decimal
        status = ds.Tables(0).Rows(0)("STATUS")
        qty = ds.Tables(0).Rows(0)("VAQTY")
        Dim oAttribute As AttributesCollection
        Dim oSku As New WMS.Logic.SKU(ds.Tables(0).Rows(0)("WOCONSIGNEE"), ds.Tables(0).Rows(0)("WOSKU"))
        If Not oSku.SKUClass Is Nothing Then
            oAttribute = WMS.Logic.SkuClass.ExtractLoadAttributes(oSku.SKUClass, ds.Tables(0).Rows(0))
        End If
        For Each dr As DataRow In ds.Tables(0).Rows
            Dim oLoad As New WMS.Logic.Load(dr("loadid").ToString())
            wo.ExecuteValueAddedLoads(oLoad.LOADID, status, qty, oAttribute, UserId)
        Next
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEValueAddedLoads_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEValueAddedLoads.CreatedChildControls
        With TEValueAddedLoads.ActionBar
            With .Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ValueAdded"
                .CommandName = "ExecuteOrder"
            End With
        End With
        Dim nv As New System.Collections.Specialized.NameValueCollection
        nv.Add("WOCONSIGNEE", TEWorkOrder.SelectedRecordPrimaryKeyValues().Get("consignee"))
        nv.Add("WOORDERID", TEWorkOrder.SelectedRecordPrimaryKeyValues().Get("orderid"))
        nv.Add("WOSKU", TEWorkOrder.SelectedRecordPrimaryKeyValues().Get("sku"))
        TEValueAddedLoads.PreDefinedValues = nv
    End Sub

End Class
