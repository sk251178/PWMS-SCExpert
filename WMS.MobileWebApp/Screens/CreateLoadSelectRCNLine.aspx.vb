Imports Made4Net.Shared.Web
Imports Made4Net.DataAccess
Imports WMS.Logic
'Added for RWMS-569
Imports System.Collections.Generic
'End Added for RWMS-569

'Added for RWMS-2476 Start
Imports Made4Net.WebControls
Imports Made4Net.DataAccess.Collections
'Added for RWMS-2476 End

<CLSCompliant(False)> Public Class CreateLoadSelectRCNLine
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
            DO1.Value("RCN") = Session("CreateLoadRCN")
            If Session("LineChanged") <> 1 Then
                MobileUtils.ClearCreateLoadProcessSession()
            End If
            Session.Remove("LineChanged")
            'If String.IsNullOrEmpty(Session("SKUCODE")) Or _
            '    String.IsNullOrEmpty(Session("SKUSEL_RECEIPTLINE")) Then
            '    Try
            ' If (DO1.Value("RCN") <> "") Then DO1.FocusField = "SKU"
            If (DO1.Value("RCN") <> "") Then DO1.FocusField = "RCNLINE"
            'DO1.Value("SKU") = Session("SKUCODE")
            'DO1.Value("RCNLINE") = Session("SKUSEL_RECEIPTLINE")
            'DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")
            '        Catch ex As Exception
            '    End Try
            'End If
        End If
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("RCN") = Session("SKUSEL_RECEIPT")
            DO1.Value("RCNLINE") = Session("SKUSEL_RECEIPTLINE")
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_RECEIPT")
            Session.Remove("SKUSEL_RECEIPTLINE")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        ' Check form
        If DO1.Value("RCN").Trim = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt must not be empty"))
            Return
        End If
        If WMS.Logic.ReceiptHeader.Exists(DO1.Value("RCN").Trim) Then
            Dim rh As New WMS.Logic.ReceiptHeader(DO1.Value("RCN").Trim)

            If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt is cancelled, cannot create payload"))
                Return
            End If
        End If
        'Dim cntr As Int32
        Dim sql As String
        Dim dt As New DataTable
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("SKU").Trim <> "" Then

            InitSKUBarCode128(DO1.Value("SKU"))

            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & DO1.Value("RCN").Trim & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "CreateLoadSelectRCNLine"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_RECEIPT") = DO1.Value("RCN").Trim
                Session("SKUSEL_RECEIPTLINE") = DO1.Value("RCNLINE").Trim
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & DO1.Value("RCN").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & DO1.Value("RCN").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
            End If

            If DataInterface.ExecuteScalar("SELECT NEWSKU FROM SKU WHERE SKU='" & DO1.Value("SKU") & "'") = 1 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("New sku, can not reaceive"))
                Return
            End If
        End If
        'Try
        '    Session("SKUCODE") = DO1.Value("SKU").Trim
        '    Session("SKUSEL_RECEIPTLINE") = DO1.Value("RCNLINE").Trim
        '    Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
        'Catch ex As Exception

        'End Try
        If DO1.Value("RCNLINE").Trim <> "" Then
            Session("CreateLoadRCNMultipleLines") = False
            sql = String.Format("SELECT RECEIPT,RECEIPTLINE,SKU.CONSIGNEE,SKU.SKU FROM RECEIPTDETAIL INNER JOIN SKU ON RECEIPTDETAIL.CONSIGNEE = SKU.CONSIGNEE AND RECEIPTDETAIL.SKU = SKU.SKU WHERE RECEIPT LIKE '{0}%' AND " & _
                "RECEIPTLINE = {1} AND SKU.CONSIGNEE LIKE '{2}%' AND SKU.SKU LIKE '{3}%'", DO1.Value("RCN"), DO1.Value("RCNLINE"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("SKU"))
        Else
            If DO1.Value("SKU").Trim = "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt line or SKU field must be filled"))
                Return
            End If
            Session("CreateLoadRCNMultipleLines") = True
            sql = String.Format("SELECT RECEIPT,RECEIPTLINE,SKU.CONSIGNEE,SKU.SKU FROM RECEIPTDETAIL INNER JOIN SKU ON RECEIPTDETAIL.CONSIGNEE = SKU.CONSIGNEE AND RECEIPTDETAIL.SKU = SKU.SKU  WHERE RECEIPT LIKE '{0}%' AND " & _
                "SKU.CONSIGNEE LIKE '{1}%' AND SKU.SKU like '{2}%'", DO1.Value("RCN"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("SKU"))
        End If
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            'Try to search for a substitue sku
            dt = New DataTable
            sql = String.Format("SELECT RECEIPT, RECEIPTLINE, SKU.CONSIGNEE, SKUSUBSTITUTE.SUBSTITUTESKU as SKU FROM SKU INNER JOIN SKUSUBSTITUTE ON SKU.CONSIGNEE = SKUSUBSTITUTE.CONSIGNEE AND SKU.SKU = SKUSUBSTITUTE.SKU INNER JOIN " & _
               " RECEIPTDETAIL ON SKU.CONSIGNEE = RECEIPTDETAIL.CONSIGNEE AND SKU.SKU = RECEIPTDETAIL.SKU WHERE RECEIPT LIKE '{0}%' AND SKUSUBSTITUTE.CONSIGNEE LIKE '{1}%' AND SKUSUBSTITUTE.SUBSTITUTESKU like '{2}'", DO1.Value("RCN"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("SKU"))
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("No Data Found"))
            ElseIf dt.Rows.Count = 1 Then
                GoToCLDInfo(dt.Rows(0))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Found more than 1 record. Please enter receipt line"))
            End If
        ElseIf dt.Rows.Count = 1 Then
            GoToCLDInfo(dt.Rows(0))
        ElseIf dt.Rows.Count > 1 And Session("CreateLoadRCNMultipleLines") Then
            GoToCLDInfo(dt.Rows(0), True)
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Found more than 1 record. Please enter receipt line"))
        End If
    End Sub

    Private Sub GoToCLDInfo(ByVal dr As DataRow, Optional ByVal pReceiveMultipleLines As Boolean = False)
        Session("CreateLoadRCN") = dr("Receipt")
        If Session("CreateLoadRCNMultipleLines") = True And pReceiveMultipleLines Then
            Dim sql As String
            Dim dt As New DataTable
            sql = "select ReceiptLine from RECEIPTDETAIL where Consignee='{0}' and Receipt = '{1}' and sku='{2}'"
            sql = String.Format(sql, dr("Consignee"), dr("Receipt"), dr("SKU"))
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            Dim message As String

            For Each dRow As DataRow In dt.Rows
                If Not RWMS.Logic.AppUtil.IsCreateLoad(dr("Receipt"), dRow("ReceiptLine"), message, True) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, message)
                    Exit Sub
                End If
            Next

            Session("CreateLoadRCNLine") = -1
        Else
            Dim message As String
            If Not RWMS.Logic.AppUtil.IsCreateLoad(dr("Receipt"), dr("ReceiptLine"), message, True) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, message)
                Exit Sub
            End If

            Session("CreateLoadRCNLine") = dr("ReceiptLine")
            Session("CreateLoadRCNMultipleLines") = False
        End If
        Session("CreateLoadConsignee") = dr("Consignee")
        Session("CreateLoadSKU") = dr("SKU")

        If DO1.Value("AdditionalAttributes") <> "" Then
            InitAdditionalAttributes(DO1.Value("AdditionalAttributes"))
        End If
        'Response.Redirect(MapVirtualPath("Screens/CLDInfo.aspx"))
        Response.Redirect(MobileUtils.GetURLByScreenCode("RDTCLD1"))
    End Sub

    Private Sub doMenu()
        'Session.Remove("CreateLoadRCN")
        'Session.Remove("CreateLoadRCNLine")
        'Session.Remove("CreateLoadConsignee")
        'Session.Remove("CreateLoadSKU")
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    'Added for RWMS-569
    Public Function InitAdditionalAttributes(ByRef BarCode As String) As Integer
        Dim ret As Integer = 1
        Dim ParceGS1 As Dictionary(Of String, String) = New Dictionary(Of String, String)
        Dim gs As Barcode128GS.GS128 = New Barcode128GS.GS128()

        ret = gs.ParceGS1Code128(BarCode, ParceGS1, "")
        If ret = 1 Then
            Session("CreateLoadAdditionalAttributesCode128") = ParceGS1
        End If

        Return ret
    End Function
    'End Added for RWMS-569

    'Added for RWMS-569
    Public Function InitSKUBarCode128(ByRef BarCode As String) As Integer
        Dim ret As Integer = 1
        Dim ParceGS1 As Dictionary(Of String, String) = New Dictionary(Of String, String)
        Dim gs As Barcode128GS.GS128 = New Barcode128GS.GS128()

        ret = gs.ParceGS1Code128(BarCode, ParceGS1, "")
        If ParceGS1.Count > 0 Then ' ret = 1 Then
            Session("CreateLoadDictSKUCode128") = ParceGS1
            If ParceGS1.ContainsKey("SKU") Then
                BarCode = ParceGS1("SKU").ToString()
            End If
        End If

        Return ret
    End Function
    'End Added for RWMS-569

    Private Sub doClose()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("RCN") <> "" Then
            Try
                'Dim rc As RWMS.Logic.GelsonsReceiptHeader = New RWMS.Logic.GelsonsReceiptHeader(DO1.Value("RCN"), True)
                'rc.closeReceiptHeader(WMS.Logic.GetCurrentUser)
                Dim rc As ReceiptHeader = New ReceiptHeader(DO1.Value("RCN"), True)
                rc.close(WMS.Logic.GetCurrentUser)

                RWMS.Logic.AppUtil.UpdateReceiptAverageWeight(DO1.Value("RCN"))
                Dim message As String
                If Not RWMS.Logic.AppUtil.CloseInboundByReceipt(DO1.Value("RCN"), message) Then
                    ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, message)
                    'Exit Sub
                End If
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt Closed"))
                'Code to calculate the running average
                WMS.Logic.Inventory.CalculateAndUpdateRunningAvgWgt(DO1.Value("RCN"))

                'Dim sSqlFlag As String = "SELECT PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME = 'CalcRuningAvgWt'"
                'Dim paramValue As String
                'Dim avgWeight As Decimal = 0
                'Dim totalInventory As Integer = 0
                'Dim currentInventory As Integer = 0
                'Dim AvgRunningVal As Decimal = 0.0


                'paramValue = Made4Net.DataAccess.DataInterface.ExecuteScalar(sSqlFlag)
                'If (paramValue = "1") Then

                '    Dim strClassUnits As String = "select rcpt.SKU,rcpt.RECEIVEDWEIGHT,rcpt.QTYRECEIVED,sum(units) as UNITS,attb.WGT,sku.CLASSNAME from loads ld " & _
                '            " inner join SKUATTRIBUTE attb on attb.SKU = ld.SKU " & _
                '            " inner join SKU sku on sku.sku = ld.sku " & _
                '            " inner join (select sum(receivedweight) as RECEIVEDWEIGHT,sum(QTYRECEIVED) as QTYRECEIVED, SKU from receiptdetail " & _
                '            " where RECEIPT = '" & DO1.Value("RCN") & "' group by sku) as rcpt " & _
                '            " on rcpt.SKU = ld.SKU where ld.STATUS != '' and ld.LOCATION !='' group by ld.sku, rcpt.RECEIVEDWEIGHT,rcpt.SKU, attb.WGT, sku.CLASSNAME, rcpt.QTYRECEIVED"
                '    Dim dtSkuDetails As New DataTable
                '    Dim drSKU As DataRow
                '    Made4Net.DataAccess.DataInterface.FillDataset(strClassUnits, dtSkuDetails)
                '    For Each drSKU In dtSkuDetails.Rows
                '        If Not IsDBNull(drSKU("CLASSNAME")) And drSKU("CLASSNAME") = "WGT" Or drSKU("CLASSNAME") = "EXPW" Or drSKU("CLASSNAME") = "MANW" Then
                '            avgWeight = drSKU("WGT")
                '            totalInventory = drSKU("UNITS")
                '            currentInventory = totalInventory - drSKU("QTYRECEIVED")
                '            'AvgRunningVal = ((currentInventory * avgWeight) + Decimal.Parse(drSKU("RECEIVEDWEIGHT"))) / totalInventory
                '            AvgRunningVal = WMS.Logic.Inventory.CalculateRunningAverageWeight(currentInventory, avgWeight, totalInventory, Decimal.Parse(drSKU("RECEIVEDWEIGHT")))
                '            Dim sqlUpdate As String = String.Format("Update SKUATTRIBUTE set WGT = " & Decimal.Parse(AvgRunningVal) & " where SKU='" & drSKU("SKU") & "'")
                '            DataInterface.RunSQL(sqlUpdate)
                '        End If
                '    Next
                'End If
                'nd of running average calculation
                DO1.Value("RCN") = ""
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage())
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt not found"))
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("RCN", True, "next")
        DO1.AddTextboxLine("RCNLINE")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU", "SKU / EAN")
        'End Added for RWMS-129
        'Added for RWMS-569
        DO1.AddTextboxLine("AdditionalAttributes")
        'End Added for RWMS-569

        'Added for RWMS-2476 Start
        Dim RDTReceivingEnableCloseReceiptButton As String = String.Empty
        Try
            RDTReceivingEnableCloseReceiptButton = Made4Net.Shared.Util.GetSystemParameterValue("RDTReceivingEnableCloseReceiptButton")
        Catch
        End Try
        If String.IsNullOrEmpty(RDTReceivingEnableCloseReceiptButton) Or RDTReceivingEnableCloseReceiptButton <> "1" Then
            HideCloseReceipt()
        End If
        'Added for RWMS-2476 End

        DO1.AddSpacer()
    End Sub
    'Added for RWMS-2476 Start
    Private Sub HideCloseReceipt()
        Dim R As New RecursiveControlFinder
        Dim btns As GenericCollection = R.SearchByType(GetType(ExecutionButton), Me.Controls)
        Dim btn As ExecutionButton, n As Int32
        For n = 0 To btns.Count - 1
            btn = CType(btns(n), ExecutionButton)
            If btn.Text = "Close Receipt" Then
                btn.Visible = False
            End If
        Next
    End Sub
    'Added for RWMS-2476 End


    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "close receipt"
                doClose()
        End Select
    End Sub
End Class