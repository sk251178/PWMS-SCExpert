Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class ReceiveById2
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
            setScreen()
        End If
    End Sub

    Private Sub doRecieve(ByVal doPutaway As Boolean)
        Dim type As String = Session("ReceiveByIdType")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If type = "LOAD" Then
                Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
                Dim oLoad As Load = oASN.ReceiveByLoadId(DO1.Value("Location"), ViewState("RecivingWarehouseArea"), WMS.Logic.GetCurrentUser(), "FLOWTHROUGH")
                If doPutaway Then
                    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, "") 'RWMS-1277
                    Response.Redirect(MapVirtualPath("Screens/RPK2.aspx"))
                End If
            Else
                AsnDetail.ReceiveByContId(Session("ReceiveByIdContID"), DO1.Value("Location"), ViewState("RecivingWarehouseArea"), WMS.Logic.GetCurrentUser(), "FLOWTHROUGH")
                If doPutaway Then
                    Response.Redirect(MapVirtualPath("Screens/RPKC.aspx?CntrId=" & Session("ReceiveByIdContID")))
                End If
            End If
            Response.Redirect(MapVirtualPath("Screens/ReceiveById.aspx"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.ToString))
            Return
        End Try
    End Sub

    Private Sub doBack()
        Session.Remove("ReceiveByIdType")
        Session.Remove("ReceiveByIdASNID")
        Session.Remove("ReceiveByIdContID")
        Session.Remove("ReceiveByIdreceiptId")
        Response.Redirect(MapVirtualPath("Screens/ReceiveById.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'in case we want to display the container details
        DO1.AddLabelLine("ContainerId")
        'in case we want to display the Load details
        DO1.AddLabelLine("Loadid")
        DO1.AddLabelLine("Consignee")
        DO1.AddLabelLine("Sku")
        DO1.AddLabelLine("SkuDescription")

        DO1.AddLabelLine("Receipt")
        DO1.AddLabelLine("BOL")
        DO1.AddLabelLine("ReceiptLine")
        DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("Units")

        DO1.AddSpacer()
        DO1.AddTextboxLine("Location")
        'DO1.AddTextbox Line("Warehousearea")
        DO1.AddSpacer()
    End Sub

    Private Sub setScreen()
        Dim type As String = Session("ReceiveByIdType")
        Dim oCons As Consignee
        If type = "LOAD" Then
            DO1.setVisibility("Loadid", True)
            DO1.setVisibility("Consignee", True)
            DO1.setVisibility("Sku", True)
            DO1.setVisibility("SkuDesciption", True)
            DO1.setVisibility("ReceiptLine", True)
            DO1.setVisibility("UOM", True)
            DO1.setVisibility("Units", True)

            Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
            Dim oRecDet As New ReceiptDetail(oASN.Receipt, oASN.ReceiptLine)
            DO1.setVisibility("Loadid", True)
            DO1.Value("Loadid") = oASN.LoadId
            DO1.Value("Consignee") = oRecDet.CONSIGNEE
            DO1.Value("Sku") = oRecDet.SKU
            DO1.Value("SkuDesciption") = Made4Net.DataAccess.DataInterface.ExecuteScalar("select skudesc from sku where sku = '" & oRecDet.SKU & "'")

            DO1.Value("Receipt") = oRecDet.RECEIPT
            DO1.Value("ReceiptLine") = oRecDet.RECEIPTLINE
            DO1.Value("UOM") = oASN.UOM
            DO1.Value("Units") = oRecDet.QTYEXPECTED
            oCons = New Consignee(oRecDet.CONSIGNEE)
            DO1.Value("Location") = oCons.DEFAULTRECEIVINGLOCATION
            'DO1.Value("Warehousearea") = oCons.DEFAULTRECEIVINGWAREHOUSEAREA
            ViewState("RecivingWarehouseArea") = oCons.DEFAULTRECEIVINGWAREHOUSEAREA

            DO1.setVisibility("ContainerId", False)
            DO1.setVisibility("BOL", False)
        Else
            Dim oRec As New ReceiptHeader(Session("ReceiveByIdreceiptId"))
            DO1.setVisibility("ContainerId", True)
            DO1.Value("ContainerId") = Session("ReceiveByIdContID")
            DO1.Value("Receipt") = oRec.RECEIPT
            DO1.Value("BOL") = oRec.BOL
            DO1.Value("Location") = ""
            'DO1.Value("WAREHOUSEAREA") = ""

            DO1.setVisibility("Loadid", False)
            DO1.setVisibility("Consignee", False)
            DO1.setVisibility("Sku", False)
            DO1.setVisibility("SkuDesciption", False)
            DO1.setVisibility("ReceiptLine", False)
            DO1.setVisibility("UOM", False)
            DO1.setVisibility("Units", False)
        End If

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "receive & putaway"
                doRecieve(True)
            Case "receive"
                doRecieve(False)
            Case "back"
                doBack()
        End Select
    End Sub
End Class
