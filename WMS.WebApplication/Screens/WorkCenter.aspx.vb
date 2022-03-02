Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports made4net.Shared.Conversion.Convert
Imports Made4Net.Shared
Imports WMS.Logic
Imports WMS.Lib

Public Class WorkCenter
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEWaveException As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEWavePicks As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEWorkOrder As Made4Net.WebControls.TableEditor
    Protected WithEvents TEWorkOrderBOM As Made4Net.WebControls.TableEditor
    Protected WithEvents TEWorkOrderLoads As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEWorkOrderSelecttLoads As Made4Net.WebControls.TableEditor

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
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "createnew"
                Dim wo As New WorkOrderHeader
                For Each dr In ds.Tables(0).Rows
                    wo.Create(ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("orderid")), ReplaceDBNull(dr("ordertype")), _
                         WorkOrderHeader.WorkOrderTypeFromString(ReplaceDBNull(dr("documenttype"))), ReplaceDBNull(dr("referenceord")), ReplaceDBNull(dr("referenceordline")), _
                         ReplaceDBNull(dr("outboundorder")), ReplaceDBNull(dr("outboundorderline")), ReplaceDBNull(dr("sku")), ReplaceDBNull(dr("statustopick")), _
                         ReplaceDBNull(dr("originalqty")), ReplaceDBNull(dr("duedate")), ReplaceDBNull(dr("notes")), ReplaceDBNull(dr("location")), ReplaceDBNull(dr("warehousearea")), Nothing, UserId)
                Next
            Case "editorder"
                Dim wo As WorkOrderHeader
                For Each dr In ds.Tables(0).Rows
                    wo = New WorkOrderHeader(ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("orderid")))
                    wo.Edit(ReplaceDBNull(dr("ordertype")), WorkOrderHeader.WorkOrderTypeFromString(ReplaceDBNull(dr("documenttype"))), ReplaceDBNull(dr("referenceord")), ReplaceDBNull(dr("referenceordline")), _
                         ReplaceDBNull(dr("location")), ReplaceDBNull(dr("warehousearea")), ReplaceDBNull(dr("statustopick")), ReplaceDBNull(dr("outboundorder")), ReplaceDBNull(dr("outboundorderline")), ReplaceDBNull(dr("sku")), _
                         ReplaceDBNull(dr("modifiedqty")), ReplaceDBNull(dr("duedate")), ReplaceDBNull(dr("notes")), Nothing, UserId)
                Next
            Case "cancelorder"
                Dim wo As WorkOrderHeader
                For Each dr In ds.Tables(0).Rows
                    wo = New WorkOrderHeader(ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("orderid")))
                    wo.Cancel(UserId)
                Next
            Case "generatebom"
                Dim wo As WorkOrderHeader
                For Each dr In ds.Tables(0).Rows
                    wo = New WorkOrderHeader(ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("orderid")))
                    wo.GenerateBomBySku(UserId)
                Next
            Case "addbom"
                Dim wo As New WorkOrderHeader(ReplaceDBNull(ds.Tables(0).Rows(0)("consignee")), ReplaceDBNull(ds.Tables(0).Rows(0)("orderid")))
                For Each dr In ds.Tables(0).Rows
                    Dim oAttCol As AttributesCollection '= SkuClass.ExtractLoadAttributes(dr)
                    Try
                        Dim skuObj As New WMS.Logic.SKU(dr("consignee"), dr("Partsku"))
                        oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(skuObj.SKUClass, dr)
                    Catch
                        oAttCol = Nothing
                    End Try

                    Try
                        Dim partQty As Decimal = ReplaceDBNull(dr("partqty"))
                        If partQty <= 0 Then
                            Throw New Exception()
                        End If
                    Catch ex As Exception
                        Throw New M4NException(New Exception(), "Part quantity must be a positive number.", "Part quantity must be a positive number.")
                    End Try

                    wo.AddPartSku(ReplaceDBNull(dr("partsku")), ReplaceDBNull(dr("partqty")), ReplaceDBNull(dr("inventorystatus")), oAttCol, UserId)
                Next
            Case "editbom"
                Dim wo As New WorkOrderHeader(ReplaceDBNull(ds.Tables(0).Rows(0)("consignee")), ReplaceDBNull(ds.Tables(0).Rows(0)("orderid")))
                For Each dr In ds.Tables(0).Rows
                    Dim oAttCol As AttributesCollection '= SkuClass.ExtractLoadAttributes(dr)
                    Try
                        Dim skuObj As New WMS.Logic.SKU(dr("consignee"), dr("Partsku"))
                        oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(skuObj.SKUClass, dr)
                    Catch
                        oAttCol = Nothing
                    End Try

                    Try
                        Dim partQty As Decimal = ReplaceDBNull(dr("partqty"))
                        If partQty <= 0 Then
                            Throw New Exception()
                        End If
                    Catch ex As Exception
                        Throw New M4NException(New Exception(), "Part quantity must be a positive number.", "Part quantity must be a positive number.")
                    End Try

                    wo.EditPartSku(ReplaceDBNull(dr("partsku")), ReplaceDBNull(dr("partqty")), ReplaceDBNull(dr("inventorystatus")), oAttCol, UserId)
                Next
            Case "deletebom"
                Dim wo As New WorkOrderHeader(ReplaceDBNull(ds.Tables(0).Rows(0)("consignee")), ReplaceDBNull(ds.Tables(0).Rows(0)("orderid")))
                wo.DeletePartSku(ReplaceDBNull(ds.Tables(0).Rows(0)("partsku")), UserId)
        End Select
    End Sub

#End Region

#Region "Ctor Functions"

#End Region

#Region "Page & Table Editor Events"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEWorkOrder_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWorkOrder.CreatedChildControls
        With TEWorkOrder.ActionBar
            .AddSpacer()
            .AddExecButton("PopulateBom", "Populate BOM", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
            .AddExecButton("complete", "Complete Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
            .AddSpacer()
            .AddExecButton("PrintManifest", "Print Manifest", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            With .Button("Save")
                If TEWorkOrder.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "CreateNew"
                Else
                    .CommandName = "EditOrder"
                End If
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
            End With
            With .Button("PopulateBom")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
                .CommandName = "GenerateBom"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
            With .Button("complete")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
                .CommandName = "complete"
                .ConfirmMessage = "Are you sure you want to complete the Work Order?"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
            With .Button("PrintManifest")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
                .CommandName = "printmanifest"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
            .AddSpacer()
            .AddSpacer()
            .AddExecButton("CancelOrder", "Cancel Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            With .Button("CancelOrder")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
                .CommandName = "CancelOrder"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the Work Order?"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
        End With
    End Sub

    Private Sub TEWorkOrderBOM_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWorkOrderBOM.CreatedChildControls
        With TEWorkOrderBOM.ActionBar
            With .Button("Save")
                If TEWorkOrderBOM.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "AddBom"
                Else
                    .CommandName = "EditBom"
                End If
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
            End With
            With .Button("Delete")
                .CommandName = "DeleteBom"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WorkCenter"
            End With
        End With
    End Sub

#End Region

End Class
