Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports System.Collections.Generic

Partial Public Class CLDGTIN1
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            SetScreen(-1, DateTime.MinValue, "")
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "createload"
                doCreateLoadPerScan()
                'CreateLoadForCachedData()
            Case "back"
                ClearSession()
                doBack()
            Case "scancase"
                doCreateLoadPerScan()
        End Select
    End Sub

    Private Sub ClearSession()
        Session("TotalScannedUnits") = 0
        Session("TotalScannedWeight") = 0
        Session("TotalScannedCases") = 0
        Session("ScannedCasesConsignee") = ""
        Session("ScannedCasesSKU") = ""
    End Sub

    Private Sub doBack()
        ClearSession()
        Response.Redirect(MapVirtualPath("Screens/CLDGTIN.aspx"))
    End Sub

    Private Sub ClearForm()
        DO1.Value("GTIN") = ""
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("RCN")
        DO1.AddLabelLine("PALLETID")
        DO1.AddLabelLine("ScannedCases", "Total scanned cases")
        DO1.AddLabelLine("ScannedWeight", "Total scanned weight")
        DO1.AddSpacer()
        DO1.AddTextboxLine("GTIN", True, "ScanCase", "128 Barcode")
        DO1.AddSpacer()
        DO1.AddLabelLine("Last case data")
        DO1.AddLabelLine("CasesManDate", "Manufacture Date")
        DO1.AddLabelLine("CaseWeight", "Weight")
        DO1.AddLabelLine("CaseSerial", "Serial number")
        DO1.AddSpacer()
        DO1.DefaultButton = "ScanCase"
    End Sub

    Private Sub SetScreen(ByVal pScannedWeight As Decimal, ByVal pScannedManDate As DateTime, ByVal pScannedSerial As String)
        DO1.Value("RCN") = Session("GTINReceiptNumber")
        DO1.Value("PALLETID") = Session("GTINContainer")

        'Accumulated data
        If pScannedWeight > -1 Then
            Session("TotalScannedCases") += 1
            Session("TotalScannedWeight") += pScannedWeight
            DO1.Value("ScannedCases") = Session("TotalScannedCases")
            DO1.Value("ScannedWeight") = Session("TotalScannedWeight")

            'Last scanned case data
            If (Not pScannedManDate.Equals(DateTime.MinValue)) Then
                DO1.Value("CasesManDate") = pScannedManDate.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat"))
            Else
                DO1.Value("CasesManDate") = ""
            End If
            If pScannedWeight > 0 Then
                DO1.Value("CaseWeight") = pScannedWeight
            Else
                DO1.Value("CaseWeight") = 0
            End If
            If (pScannedSerial.ToString().Length > 0) Then
                DO1.Value("CaseSerial") = pScannedSerial
            Else
                DO1.Value("CaseSerial") = ""
            End If
        Else
            DO1.Value("ScannedCases") = 0
            DO1.Value("ScannedWeight") = 0

            'Last scanned case data
            DO1.Value("CasesManDate") = ""
            DO1.Value("CaseWeight") = ""
            DO1.Value("CaseSerial") = ""
        End If
        
    End Sub

    'Private Function ExtractAttributeValues(ByVal oSku As WMS.Logic.SKU, ) As WMS.Logic.AttributesCollection
    Private Function ExtractAttributeValues(ByVal oSku As WMS.Logic.SKU, ByVal ParceGS1 As Dictionary(Of String, String)) As WMS.Logic.AttributesCollection

        Dim objSkuClass As WMS.Logic.SkuClass = oSku.SKUClass
        If objSkuClass Is Nothing Then Return Nothing
        Dim oAttCol As New WMS.Logic.AttributesCollection
        For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
            Dim req As Boolean = False
            If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                Dim val As Object
                Try
                    'RWMS -569 - using dictionary of CODE128GS parser instead of GTINBarcodeParser
                    If ParceGS1.ContainsKey(oAtt.Name) = False OrElse _
                                ParceGS1("SKU") = String.Empty Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Attribute Validation failed for " & oAtt.Name, "Attribute Validation failed for " & oAtt.Name)
                    Else
                        Select Case oAtt.Type
                            Case Logic.AttributeType.Boolean
                                val = CType(ParceGS1("SKU"), Boolean)
                            Case Logic.AttributeType.DateTime
                                val = DateTime.ParseExact(ParceGS1(oAtt.Name), "yyMMdd", Nothing)
                            Case Logic.AttributeType.Decimal
                                val = CType(ParceGS1(oAtt.Name), Decimal)
                            Case Logic.AttributeType.Integer
                                val = CType(ParceGS1(oAtt.Name), Int32)
                            Case Else
                                val = ParceGS1(oAtt.Name)
                        End Select
                        oAttCol.Add(oAtt.Name, val)
                    End If

                  
                Catch ex As Exception
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Attribute Validation failed for " & oAtt.Name, "Attribute Validation failed for " & oAtt.Name)
                    End If
                End Try
            End If
        Next
        Return oAttCol
    End Function

    Private Function GetContainer() As WMS.Logic.Container
        Dim oContainer As WMS.Logic.Container
        If WMS.Logic.Container.Exists(DO1.Value("PALLETID")) Then
            oContainer = New WMS.Logic.Container(DO1.Value("PALLETID"), True)
        Else
            oContainer = New WMS.Logic.Container()
            oContainer.ContainerId = DO1.Value("PALLETID")
            oContainer.Save(WMS.Logic.GetCurrentUser)
        End If
        Return oContainer
    End Function

#Region "Create load for cached data"

    Private Sub CreateLoadForCachedData()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            
            Dim sConsignee, sSku, sLoadid, sReceipt, sReceiptLine As String
            Dim units As Decimal
            units = Session("TotalScannedUnits")
            sConsignee = Session("ScannedCasesConsignee")
            sSku = Session("ScannedCasesSKU")
            sReceipt = Session("ScannedCasesReceipt")
            sReceiptLine = Session("ScannedCasesReceiptLine")

            Dim oReceiving As New Logic.Receiving
            Dim oCons As New WMS.Logic.Consignee(sConsignee)
            Dim oSku As New WMS.Logic.SKU(sConsignee, sSku)
            Dim oRecDetail As New WMS.Logic.ReceiptDetail(sReceipt, sReceiptLine)

            Dim oAttributes As New WMS.Logic.AttributesCollection
            oAttributes.Add("weight", Session("TotalScannedWeight"))

            Dim ld() As WMS.Logic.Load = oReceiving.CreateLoad(sReceipt, sReceiptLine, sSku, sLoadid, oSku.DEFAULTRECUOM, oCons.DEFAULTRECEIVINGLOCATION, oCons.DEFAULTRECEIVINGWAREHOUSEAREA, _
                            units, oSku.INITIALSTATUS, "", 1, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, "", oRecDetail.DOCUMENTTYPE, "", "")
            Try
                RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
            Catch ex As Exception
            End Try

            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Load created"))
            ClearSession()
            ClearForm()
            DO1.Value("ScannedCases") = Session("TotalScannedCases")
            DO1.FocusField = "GTIN"
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
            Return
        End Try
    End Sub

    Private Sub ScanCase()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("RCN").Trim = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt must not be empty"))
            Return
        End If
        If DO1.Value("GTIN").Trim = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("No barcode was scanned"))
            Return
        End If
        Try
            'parse the barcode scanned and extract all data to match the receipt data...
            'RWMS -569 - Calling CODE128GS parser instead of GTINBarcodeParser
            Dim ret As Integer = 1
            Dim strErrorMessage As String
            Dim ParceGS1 As Dictionary(Of String, String) = New Dictionary(Of String, String)
            Dim gsBarcodeParser As Barcode128GS.GS128 = New Barcode128GS.GS128()
            Dim sSku As String = ""
            ret = gsBarcodeParser.ParceGS1Code128(DO1.Value("GTIN"), ParceGS1, strErrorMessage)
            If ParceGS1.Count > 0 Then

                If ParceGS1.ContainsKey("SKU") Then
                    sSku = ParceGS1("SKU").ToString()
                End If
            End If

            Dim sql As String
            Dim dt As New DataTable
            sql = String.Format("SELECT distinct RECEIPT,RECEIPTLINE,vSKUCODE.CONSIGNEE,vSKUCODE.SKU FROM RECEIPTDETAIL INNER JOIN vSKUCODE ON RECEIPTDETAIL.CONSIGNEE = vSKUCODE.CONSIGNEE AND RECEIPTDETAIL.SKU = vSKUCODE.SKU  WHERE RECEIPT LIKE '{0}%' AND vSKUCODE.SKUCODE like '{1}%' and QTYEXPECTED - QTYRECEIVED > 0", DO1.Value("RCN"), sSku)
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count >= 1 Then
                If Not Session("ScannedCasesSKU") Is Nothing AndAlso Session("ScannedCasesSKU") <> "" Then
                    If dt.Rows(0)("sku") <> Session("ScannedCasesSKU") Then
                        DO1.Value("GTIN") = ""
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Scanned item does not match to current cached cases"))
                        Return
                    End If
                ElseIf Session("ScannedCasesSKU") Is Nothing Or Session("ScannedCasesSKU") = "" Then
                    Session("ScannedCasesSKU") = dt.Rows(0)("sku")
                End If
                'Set the receipt and the receipt line in the session
                If Session("ScannedCasesReceipt") Is Nothing OrElse Session("ScannedCasesReceipt") = "" Then
                    Session("ScannedCasesReceipt") = dt.Rows(0)("receipt")
                    Session("ScannedCasesReceiptLine") = dt.Rows(0)("receiptline")
                    Session("ScannedCasesConsignee") = dt.Rows(0)("Consignee")
                    Session("ScannedCasesSKU") = dt.Rows(0)("sku")
                End If
                'add the current case to the cached data
                'RWMS -569 - using dictionary of CODE128GS parser instead of GTINBarcodeParser
                AddCaseDataToCache(dt.Rows(0), ParceGS1)
                DO1.Value("GTIN") = ""
                DO1.Value("ScannedCases") = Session("TotalScannedCases")
                DO1.FocusField = "GTIN"
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Item not found in selected receipt"))
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

    Private Sub AddCaseDataToCache(ByVal dr As DataRow, ByRef ParceGS1 As Dictionary(Of String, String))
        Dim sConsignee, sSku As String
        Dim units As Decimal
        sConsignee = dr("consignee")
        sSku = dr("sku")
        'RWMS -569 - using dictionary of CODE128GS parser instead of GTINBarcodeParser
        If ParceGS1.Count > 0 Then

            If ParceGS1.ContainsKey("UNITS") Then
                units = ParceGS1("UNITS").ToString()
            End If
        End If
        Dim oSku As New WMS.Logic.SKU(sConsignee, sSku)
        Dim oAttributes As WMS.Logic.AttributesCollection
        oAttributes = ExtractAttributeValues(oSku, ParceGS1)

        Session("TotalScannedUnits") += units
        Session("TotalScannedWeight") += oAttributes.Item("weight")
        Session("TotalScannedCases") += 1
    End Sub

#End Region

#Region "Create load per case scan"

    Private Sub doCreateLoadPerScan()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("RCN").Trim = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt must not be empty"))
            Return
        End If
        Try
            'parse the barcode scanned and extract all data to match the receipt data...
            'RWMS -569 - using CODE128GS parser instead of GTINBarcodeParser
            Dim ParceGS1 As Dictionary(Of String, String) = New Dictionary(Of String, String)
            Dim gsBarcodeParser As Barcode128GS.GS128 = New Barcode128GS.GS128()
            Dim sSku As String = ""
            Dim strErrorMessage As String
            Dim ret As Integer = 1

            Try
                ret = gsBarcodeParser.ParceGS1Code128(DO1.Value("GTIN"), ParceGS1, strErrorMessage)
                If ParceGS1.Count > 0 Then

                    If ParceGS1.ContainsKey("SKU") Then
                        sSku = ParceGS1("SKU").ToString()
                    End If
                End If
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Barcode does not match to the 128 barcode format"))
                ClearForm()
                Return
            End Try


            Dim sql As String
            Dim dt As New DataTable
            sql = String.Format("SELECT distinct RECEIPT,RECEIPTLINE,vSKUCODE.CONSIGNEE,vSKUCODE.SKU FROM RECEIPTDETAIL INNER JOIN vSKUCODE ON RECEIPTDETAIL.CONSIGNEE = vSKUCODE.CONSIGNEE AND RECEIPTDETAIL.SKU = vSKUCODE.SKU  WHERE RECEIPT LIKE '{0}%' AND vSKUCODE.SKUCODE like '{1}%' and QTYEXPECTED - QTYRECEIVED > 0", DO1.Value("RCN"), sSku)
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count >= 1 Then
                'Check if handling unit exists, if not create one so all cases can be placed on.
                Dim oLoad As WMS.Logic.Load
                'RWMS -569 - using dictionary of CODE128GS parser instead of GTINBarcodeParser
                oLoad = CreateLoad(dt.Rows(0), ParceGS1)
                'RWMS -569 - Scanned barcode may not contain all 3 weight, mfgdate and serial (added validation fro all 3)
                Dim weight As Decimal = 0
                Dim mfgdate As DateTime = Nothing
                Dim serial As String = ""
                If (oLoad.LoadAttributes("weight") Is Nothing) Then
                    weight = 0
                Else
                    weight = oLoad.LoadAttributes("weight")
                End If

                If (oLoad.LoadAttributes("mfgdate").Equals(DateTime.MinValue) Or oLoad.LoadAttributes("mfgdate").ToString() = String.Empty) Then
                    mfgdate = Nothing
                Else
                    mfgdate = oLoad.LoadAttributes("mfgdate")
                End If
                If (oLoad.LoadAttributes("serial") Is Nothing) Then
                    serial = ""
                Else
                    serial = oLoad.LoadAttributes("serial")
                End If
                'SetScreen(oLoad.LoadAttributes("weight"), oLoad.LoadAttributes("mfgdate"), oLoad.LoadAttributes("serial"))
                SetScreen(weight, mfgdate, serial)
                DO1.Value("GTIN") = ""
                DO1.DefaultButton = "ScanCase"
                DO1.FocusField = "GTIN"
                ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Load ctrated"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Item not found in selected receipt"))
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

    'Private Function CreateLoad(ByVal dr As DataRow, ByVal oParser As GTINBarcodeParser) As WMS.Logic.Load
    Private Function CreateLoad(ByVal dr As DataRow, ByVal ParceGS1 As Dictionary(Of String, String)) As WMS.Logic.Load

        Dim ld As WMS.Logic.Load
        Dim sConsignee, sSku, sLoadid, sReceipt, sReceiptLine As String
        Dim units As Decimal

        sConsignee = dr("consignee")
        sSku = dr("sku")
        sReceipt = dr("receipt")
        sReceiptLine = dr("receiptline")
        sLoadid = ""

        Dim oReceiving As New Logic.Receiving
        Dim oCons As New WMS.Logic.Consignee(sConsignee)
        Dim oSku As New WMS.Logic.SKU(sConsignee, sSku)
        Dim oRecDetail As New WMS.Logic.ReceiptDetail(sReceipt, sReceiptLine)

        Dim oAttributes As WMS.Logic.AttributesCollection
        'RWMS -569 - using dictionary of CODE128GS parser instead of GTINBarcodeParser
        oAttributes = ExtractAttributeValues(oSku, ParceGS1)

        units = 1 'oSku.ConvertToUnits(oSku.DEFAULTRECUOM)
        Dim oContainer As WMS.Logic.Container = GetContainer()
        'oContainer.Place(oLoad, WMS.Logic.GetCurrentUser)
        ld = oReceiving.CreateLoad(sReceipt, sReceiptLine, sSku, sLoadid, oSku.DEFAULTRECUOM, oCons.DEFAULTRECEIVINGLOCATION, oCons.DEFAULTRECEIVINGWAREHOUSEAREA, _
                        units, oSku.INITIALSTATUS, "", 1, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, "", oRecDetail.DOCUMENTTYPE, oContainer.ContainerId, oContainer.HandlingUnitType)(0)
        Try
            RWMS.Logic.AppUtil.SetReceivedWeight(ld.CONSIGNEE, ld.SKU, ld.RECEIPT, ld.RECEIPTLINE)
        Catch ex As Exception
        End Try

        Return ld
    End Function

#End Region

End Class