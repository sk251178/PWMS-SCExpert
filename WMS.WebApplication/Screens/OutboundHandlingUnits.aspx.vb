Imports WMS.Logic

Public Class OutboundHandlingUnits
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterOutboundOrders As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEOrderHU As Made4Net.WebControls.TableEditor

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
            Case "printpl"
                For Each dr In ds.Tables(0).Rows
                    PrintOutboundPackingList(dr("CONSIGNEE"), dr("ORDERID"), Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next
            Case "printordershiplabels"
                For Each dr In ds.Tables(0).Rows
                    Dim cons, ordid As String
                    cons = Convert.ToString(dr("consignee"))
                    ordid = Convert.ToString(dr("orderid"))
                    Dim oOrder As New WMS.Logic.OutboundOrderHeader(cons, ordid)
                    oOrder.PrintShipLabels("")
                Next
            Case "printcontshiplabels"
                For Each dr In ds.Tables(0).Rows
                    Dim cons, ordid, loadid, cont As String
                    cons = Convert.ToString(dr("consignee"))
                    ordid = Convert.ToString(dr("orderid"))
                    loadid = Convert.ToString(dr("loadid"))
                    cont = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("pickcontainer"), "")
                    Dim oOrder As New WMS.Logic.OutboundOrderHeader(cons, ordid)
                    If cont = "" Then
                        oOrder.PrintLoadShipLabels(loadid)
                    Else
                        oOrder.PrintContShipLabels(cont)
                    End If
                Next
        End Select
    End Sub

    Public Sub PrintOutboundPackingList(ByVal pConsignee As String, ByVal pOrderId As String, ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "OutboundPackingList"
        Made4Net.DataAccess.DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        oQsender.Add("DATASETID", "repPackingList")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", Made4Net.Shared.Util.GetSystemParameterValue("DEFAULTPRINTER"))
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONSIGNEE = '{0}' and ORDERID = '{1}'", pConsignee, pOrderId))
        oQsender.Send("Report", repType)
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterOutboundOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterOutboundOrders.CreatedChildControls
        With TEMasterOutboundOrders
            With .ActionBar
                .AddSpacer()
                .AddExecButton("PrintPL", "Print Packing List", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintPL")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundHandlingUnits"
                    .CommandName = "PrintPL"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
            End With
        End With
    End Sub

    Private Sub TEMasterOutboundOrders_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterOutboundOrders.CreatedGrid
        TEMasterOutboundOrders.Grid.AddExecButton("printordershiplabels", "Print Ship Labels", "WMS.WebApp.dll", "WMS.WebApp.OutboundHandlingUnits", 2, Made4Net.WebControls.SkinManager.GetImageURL("BtnShipLabel"))
    End Sub

    Private Sub TEOrderHU_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEOrderHU.CreatedGrid
        TEOrderHU.Grid.AddExecButton("printcontshiplabels", "Print Ship Labels", "WMS.WebApp.dll", "WMS.WebApp.OutboundHandlingUnits", 2, Made4Net.WebControls.SkinManager.GetImageURL("BtnShipLabel"))
    End Sub
End Class