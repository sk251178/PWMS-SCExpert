Imports Made4Net.Shared.Collections
Imports Made4Net.DataAccess
Imports System.Collections
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web
Imports WMS.Logic
<CLSCompliant(False)> Public Class CLDBLD2
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            DO1.Value("CONTAINERID") = Session("CreateLoadContainerID")
            DO1.Value("RECEIPTID") = Session("CreateLoadReciptId")
            DO1.Value("LOCATION") = Session("CreateLoadLocation")
            DO1.Value("WAREHOUSEAREA") = WMS.Logic.Warehouse.getUserWarehouseArea()

            DO1.Value("CONSIGNEE") = Session("CreateLoadConsignee")
            DO1.Value("SKU") = Session("CreateLoadSKU")
            SetUOM()
        End If
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOADID") = Session("SKUSEL_LOADID")
            DO1.Value("QTY") = Session("SKUSEL_QTY")

            Session.Remove("SKUSEL_LOADID")
            Session.Remove("SKUSEL_QTY")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub SetUOM()
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
        dd.AllOption = False
        dd.TableName = "codelistdetail"
        dd.ValueField = "CODE"
        dd.TextField = "DESCRIPTION"
        dd.Where = String.Format("Codelistcode='UOM'")
        dd.DataBind()
    End Sub


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddLabelLine("RECEIPTID")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        Dim oConsignee As New Consignee(Session("CreateLoadConsignee"))
        If oConsignee.GENERATELOADID Then
            DO1.AddTextboxLine("LOADID")
        Else
            DO1.AddTextboxLine("LOADID", True, "create")
        End If
        DO1.AddTextboxLine("SKU", True, "create")
        DO1.AddTextboxLine("QTY", True, "create", "UOMUNITS")
        DO1.AddDropDown("UOM")
        DO1.AddSpacer()
    End Sub

    Private Sub doMenu()
        MobileUtils.ClearBlindReceivingSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doCreate()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("SKU").Trim <> "" Then
            ' Check for sku
            'If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & Session("CreateLoadReciptId") & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & Session("CreateLoadConsignee") & "' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "CLDBLD2"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                Session("SKUSEL_QTY") = DO1.Value("QTY").Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
                'ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & Session("CreateLoadReciptId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & Session("CreateLoadConsignee") & "' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") = 1 Then
                'DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & Session("CreateLoadReciptId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & Session("CreateLoadConsignee") & "' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')")
            End If
        End If

        Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))
        If Not oCon.GENERATELOADID And DO1.Value("LOADID") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load ID can not be blank"))
            Return
        End If

        If Not WMS.Logic.SKU.Exists(oCon.CONSIGNEE, DO1.Value("SKU")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("SKU does not exist"))
            Return
        End If

        Dim skuObj As New WMS.Logic.SKU(oCon.CONSIGNEE, DO1.Value("SKU"))
        Dim uomObj As SKU.SKUUOM = skuObj.UOM(DO1.Value("UOM"))
        If uomObj Is Nothing Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid uom for sku"))
            Return
        End If
        Dim qty As Decimal
        Try
            qty = Decimal.Parse(DO1.Value("QTY"))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid quantity"))
            Return
        End Try
        If qty <= 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Quantity must be positive"))
            Return
        End If
        Session("CreateLoadLoadId") = DO1.Value("LOADID")
        Session("CreateLoadQty") = qty '* uomObj.UNITSPERLOWESTUOM
        Session("CreateLoadSku") = DO1.Value("SKU")
        Session("CreateLoadUOM") = uomObj.UOM
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CLDBLD3.aspx"))
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "create"
                doCreate()
            Case "menu"
                doMenu()
        End Select
    End Sub

End Class
