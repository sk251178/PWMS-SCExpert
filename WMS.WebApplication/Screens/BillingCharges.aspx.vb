Imports WMS.Logic

Public Class BillingCharges
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEBillingChargesMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEBillingChargesDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable

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
        'Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "printpdp"
                For Each docdr As DataRow In ds.Tables(0).Rows
                    Dim oQsender As New Made4Net.Shared.QMsgSender
                    'Dim repType As String
                    Dim dt As New DataTable
                    oQsender.Add("REPORTID", "ProformaDetailed")
                    oQsender.Add("DATASETID", "repProformaDetailed")
                    oQsender.Add("REPORTNAME", "ProformaDetailed")
                    oQsender.Add("FORMAT", "PDF")
                    oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
                    oQsender.Add("USERID", UserId)
                    oQsender.Add("LANGUAGE", Made4Net.Shared.Util.GetSystemParameterValue("DEFAULTLANGUAGE"))
                    Try
                        oQsender.Add("PRINTER", Made4Net.Shared.Util.GetSystemParameterValue("DEFAULTPRINTER"))
                        oQsender.Add("COPIES", 1)
                        If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                            oQsender.Add("ACTION", Made4Net.Reporting.Common.ReportActionType.SAVE)
                        Else
                            oQsender.Add("ACTION", Made4Net.Reporting.Common.ReportActionType.PRINT)
                        End If
                    Catch ex As Exception
                    End Try
                    oQsender.Add("WHERE", "CHARGEID = '" & docdr("CHARGEID") & "'")
                    oQsender.Send("Report", "ProformaDetailed")
                Next
        End Select
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEBillingChargesMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBillingChargesMaster.CreatedChildControls
        With TEBillingChargesMaster
            With .ActionBar
                .AddSpacer()

                .AddExecButton("PrintPDP", "Print Detailed Proforma", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintPDP")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.BillingCharges"
                    .CommandName = "PrintPDP"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
            End With
        End With
    End Sub

End Class
