Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class ReceiveByID
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session.Remove("ReceiveByIdType")
            Session.Remove("ReceiveByIdASNID")
            Session.Remove("ReceiveByIdContID")
            Session.Remove("ReceiveByIdreceiptId")
        End If
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim sql As String
        Dim dt As New DataTable
        Dim type As String

        If DO1.Value("LoadId").Trim <> "" Then
            sql = String.Format("Select distinct asnid from asndetail where loadid = '{0}' and container like '{1}%' and status = 'EXPECTED'", DO1.Value("LoadId"), DO1.Value("ContainerId"))
            type = "LOAD"
        Else
            sql = String.Format("Select distinct receipt from asndetail where container = '{0}' and loadid like '{1}%' and status = 'EXPECTED'", DO1.Value("ContainerId"), DO1.Value("LoadId"))
            type = "CONT"
        End If
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Found more than 1 record. Please enter receipt line"))
            Return
        End If
        If dt.Rows.Count = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("No Data Found"))
        ElseIf dt.Rows.Count = 1 Then
            Session("ReceiveByIdType") = type
            If type = "LOAD" Then
                Session("ReceiveByIdASNID") = dt.Rows(0)("asnid")
                'RWMS-1336 Start
                Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
                Dim oRecDet As New ReceiptDetail(oASN.Receipt, oASN.ReceiptLine)
                Session("CreateLoadRCN") = oRecDet.RECEIPT
                Session("CreateLoadRCNLine") = oRecDet.RECEIPTLINE
                Session("CreateLoadRCNMultipleLines") = False
                Session("CreateLoadConsignee") = oRecDet.CONSIGNEE
                Session("CreateLoadSKU") = oRecDet.SKU
                Session("CreateLoadASNLoadID") = oASN.LoadId
                Session("CreateLoadUOM") = oASN.UOM
                ' Session("CreateLoadContainerID") = oASN.Container
                Session("CreateLoadASNRemainingUnits") = oASN.Units
                Session("CreateLoadASNAttributes") = oASN.ASNAttributes
                Response.Redirect(MapVirtualPath("Screens/RcvByID.aspx"))
                'Response.Redirect(MobileUtils.GetURLByScreenCode("RDTCLD1"))
                'RWMS-1336 End
            Else
                Session("ReceiveByIdContID") = DO1.Value("ContainerId")
                Session("ReceiveByIdreceiptId") = dt.Rows(0)("receipt")
            End If

            If Session("FLOWTHROUGH") = "1" Then
                Response.Redirect(MapVirtualPath("Screens/ReceiveById2.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/ReceiveById1.aspx"))
            End If

        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("ReceiveByIdType")
        Session.Remove("ReceiveByIdASNID")
        Session.Remove("ReceiveByIdContID")
        Session.Remove("ReceiveByIdreceiptId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LoadId")
        DO1.AddTextboxLine("ContainerId")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class
