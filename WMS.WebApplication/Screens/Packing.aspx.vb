Imports WMS.Logic

Public Class Packing
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterOutboundOrders As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEPackOrder As Made4Net.WebControls.TableEditor
    Protected WithEvents TEUnpackOrder As Made4Net.WebControls.TableEditor
    Protected WithEvents txtShippingContainer As Made4Net.WebControls.TextBox
    Protected WithEvents pnlAdj As System.Web.UI.WebControls.Panel
    Protected WithEvents lblContID As Made4Net.WebControls.FieldLabel
    Protected WithEvents pnlcnt As System.Web.UI.WebControls.Panel

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

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "packorder"
                Dim outorheader As OutboundOrderHeader
                For Each dr In ds.Tables(0).Rows
                    outorheader = New OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                    If Not outorheader.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
                        outorheader.Pack(Common.GetCurrentUser)
                    End If
                Next
            Case "printpl"
                'For Each dr In ds.Tables(0).Rows
                '    PrintOutboundPackingList(dr("CONSIGNEE"), dr("ORDERID"), Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                'Next
            Case "packloads"
                Dim cont As String = Convert.ToString(ds.Tables(0).Rows(0)("ShipContId"))
                If cont.Trim = "" Then cont = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                For Each dr In ds.Tables(0).Rows
                    Dim cons, ordid, loadid As String
                    cons = Convert.ToString(dr("consignee"))
                    ordid = Convert.ToString(dr("orderid"))
                    loadid = Convert.ToString(dr("loadid"))
                    Dim oOrdLoad As New WMS.Logic.OrderLoads(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, cons, ordid, loadid)
                    'oOrdLoad.Pack(cont, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOQTY")), UserId)
                Next
            Case "unpackload"
                For Each dr In ds.Tables(0).Rows
                    Dim cons, ordid, loadid, cont As String
                    cons = Convert.ToString(dr("consignee"))
                    ordid = Convert.ToString(dr("orderid"))
                    loadid = Convert.ToString(dr("loadid"))
                    cont = Convert.ToString(dr("PICKCONTAINER"))
                    Dim oOrdLoad As New WMS.Logic.OrderLoads(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, cons, ordid, loadid)
                    'oOrdLoad.UnPack(cont, UserId)
                Next
            Case "printshiplabels"
                For Each dr In ds.Tables(0).Rows
                    Dim cons, ordid As String
                    cons = Convert.ToString(dr("consignee"))
                    ordid = Convert.ToString(dr("orderid"))
                    Dim oOrder As New WMS.Logic.OutboundOrderHeader(cons, ordid)
                    oOrder.PrintShipLabels("")
                Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterOutboundOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterOutboundOrders.CreatedChildControls
        With TEMasterOutboundOrders
            With .ActionBar
                .AddSpacer()
                .AddExecButton("PackOrder", "Pack Orders", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("PackOrder")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Packing"
                    .CommandName = "PackOrder"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to pack the selected orders?"
                End With
            End With
        End With
    End Sub

    Private Sub TEUnpackOrder_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUnpackOrder.CreatedChildControls
        With TEUnpackOrder
            With .ActionBar
                .AddSpacer()
                .AddExecButton("UnPackLoad", "UnPack Loads", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
                With .Button("UnPackLoad")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Packing"
                    .CommandName = "UnPackLoad"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unpack the selected loads?"
                End With
            End With
        End With
    End Sub

    Private Sub TEPackOrder_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEPackOrder.AfterItemCommand
        TEPackOrder.RefreshData()
    End Sub

    Private Sub TEPackOrder_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPackOrder.CreatedChildControls
        With TEPackOrder
            With .ActionBar
                .AddSpacer()
                .AddExecButton("PackLoad", "Pack Loads", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("PackLoad")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Packing"
                    .CommandName = "PackLoads"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to pack the selected loads?"
                End With
            End With
        End With
        Dim nc As New System.Collections.Specialized.NameValueCollection
        nc.Add("ShipContId", Me.txtShippingContainer.Text)
        TEPackOrder.PreDefinedValues = nc
        Me.txtShippingContainer.Text = ""
    End Sub

    Private Sub TEMasterOutboundOrders_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterOutboundOrders.RecordSelected
        DTC.Visible = True
    End Sub

    Private Sub TEMasterOutboundOrders_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterOutboundOrders.AfterItemCommand
        If e.CommandName = "PackOrder" Then
            DTC.Visible = False
            TEMasterOutboundOrders.Restart()
        End If
    End Sub

    Private Sub TEMasterOutboundOrders_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterOutboundOrders.CreatedGrid
        TEMasterOutboundOrders.Grid.AddExecButton("printshiplabels", "Print Ship Labels", "WMS.WebApp.dll", "WMS.WebApp.Packing", 2, Made4Net.WebControls.SkinManager.GetImageURL("BtnShipLabel"))
    End Sub

End Class
