Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports System.Globalization
Imports System.Collections.Generic
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Text.RegularExpressions


<CLSCompliant(False)> Public Class CLD1
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
    'Added for RWMS-441
    'Public Shared gs As Barcode128GS.GS128
    'End for RWMS-441
    'Commented for RWMS-1834 Start
    'Public formatDateTime As String = "MMddyy"
    'Commented for RWMS-1834 End

    Public palletQty As Decimal = 0
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            'Added for RWMS-441
            'gs = New Barcode128GS.GS128()
            'End for RWMS-441

            If Session("CreateLoadRCN") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRcnLine.aspx"))
            End If
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            If getRemainingUnits() <= 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Line already received"))
                Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLine.aspx"))

                Return
            End If

            Dim TiHi As String = String.Empty

            'CLDInfo
            'RWMS-518
            'Commented code for RWMS-518
            'DO1.Value("CONSIGNEE") = Session("CreateLoadConsignee")
            'DO1.Value("SKU") = Session("CreateLoadSKU")
            'Added the below code

            Dim strReceipt As String = Session("CreateLoadRCN").ToString() + "  Line : "
            Dim strREceiptLine As String = Session("CreateLoadRCNLine").ToString()
            Dim strResult As String
            strResult = String.Concat(strReceipt, strREceiptLine)
            DO1.Value("RECEIPT") = strResult
            'DO1.Value("RECEIPT") = Session("CreateLoadRCNNew") '
            'DO1.Value("RECEIPTLINE") = Session("CreateLoadRCNLine")

            Dim oSku As New Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
            Dim strSKU As String = Session("CreateLoadSKU").ToString() + "   "
            Dim strSkuDesc As String = "    " + oSku.SKUDESC
            Dim strResultSku As String
            strResultSku = String.Concat(strSKU, strSkuDesc)
            DO1.Value("SKU") = strResultSku
            'DO1.Value("SKUDESC") = oSku.SKUDESC
            'End RWMS-518

            DO1.Value("LOADID") = ""

            DO1.Value("CONTAINERID") = ""


            'Try
            '    If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, "CASE") Then
            '        DO1.Value("CPQ") = oSku.ConvertToUnits("CASE")
            '    Else
            '        DO1.setVisibility("CPQ", False)
            '    End If
            'Catch ex As Exception
            'End Try


            If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, "LAYER") Then
                Dim oUom As New WMS.Logic.SKU.SKUUOM(oSku.CONSIGNEE, oSku.SKU, "LAYER")
                TiHi = Math.Truncate(oUom.UNITSPERMEASURE) & "/"
            Else
                TiHi = "/"
            End If



            If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, "PALLET") Then
                Dim oUom As New WMS.Logic.SKU.SKUUOM(oSku.CONSIGNEE, oSku.SKU, "PALLET")
                'RWMS-436 - To Get the UNITSPERLOWESTUOM of the pallet
                palletQty = oUom.UNITSPERLOWESTUOM
                TiHi &= Math.Truncate(oUom.UNITSPERMEASURE)
            End If


            DO1.Value("Ti/Hi") = TiHi

            'DO1.Value("SKUNOTES") = oSku.NOTES
            DO1.Value("CONTAINERID") = "" ' Session("CreateLoadContainerID")
            If Not String.IsNullOrEmpty(Session("CreateLoadASNLoadID")) Then
                DO1.Value("LOADID") = Session("CreateLoadASNLoadID")
            End If
            'CLD1
            If Session("CreateLoadLocation") = "" Or WMS.Logic.Warehouse.getUserWarehouseArea() = "" Then
                Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))
                DO1.Value("LOCATION") = oCon.DEFAULTRECEIVINGLOCATION
                'DO1.Value("WAREHOUSEAREA") = oCon.DEFAULTRECEIVINGWAREHOUSEAREA
                Session("CreateLoadWarehousearea") = oCon.DEFAULTRECEIVINGWAREHOUSEAREA
            Else
                DO1.Value("LOCATION") = Session("CreateLoadLocation")
                'DO1.Value("WAREHOUSEAREA") = Session("CreateLoadWarehousearea")
            End If
            'CLD2
            SetStatusAndPrinter()
            'CLD3
            SetUOM()
            'SetHUTYpe()
            'SetHUTrans()

            'Added for RWMS-151
            If Session("CreateLoadAVGWEIGHT") > 0 Then
                DO1.setVisibility("WEIGHT", False)
            End If
            'End Added for RWMS-151

            If Not Session("CreateLoadOverrideUnits") Is Nothing Then
                Try
                    ClearAttributes()
                    DO1.Value("EXPIRYDATE") = ""
                    DO1.FocusField = "EXPIRYDATE"
                Catch ex As Exception
                    If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                        Throw
                    End If
                End Try
            Else
                'return all previews values to screen when come back from OVERRIDE HUTYPE
                If Not LoadPreviewsVals() Then
                    ClearAttributes()
                    'Added for RWMS-151 - LATEST
                    setAttributesFromBarCode("CreateLoadDictSKUCode128")
                    setAttributesFromBarCode("CreateLoadAdditionalAttributesCode128")
                    'End Added for RWMS-151 - LATEST
                End If
            End If
        Else
            Dim dd As Made4Net.WebControls.MobileDropDown = CType(DO1.Ctrl("UOM"), Made4Net.WebControls.MobileDropDown)
            AddHandler dd.OnNextButtonClick, AddressOf uomDropDownNextButtonClick
            AddHandler dd.OnPrevButtonClick, AddressOf uomDropDownPreviousButtonClick

        End If

        'If DO1.Value("UOM") <> String.Empty Then
        '    updateUnitsDropDown(DO1.Value("UOM"))
        'End If
    End Sub

    Private Function LoadPreviewsVals() As Boolean
        Dim RET As Boolean = False
        Dim screenid As String = ""
        If Not IsNothing(Session("LoadPreviewsVals")) Then
            screenid = Session("LoadPreviewsVals")
        End If

        'when return from RDTCLD1OVERRIDEHUTYPE screen, need to return all previes values
        If screenid = "RDTCLD1OVERRIDEHUTYPE" Or screenid.Contains("cld1overrideWGT") Then
            RET = True
            DO1.Value("LOADID") = Session("CreateLoadLoadID")
            DO1.Value("UNITS") = Session("CreateLoadUnits")
            DO1.Value("NUMLOADS") = Session("CreateLoadNumLoads")
            setAttributesFromObject()
            'cld1overrideWGTBACK
            DO1.Value("LOCATION") = Session("CreateLoadLocation")
            DO1.Value("HUTYPE") = Session("HUTYPE")
            If screenid.Contains("cld1overrideWGT") Then
                'MEAN OVERRIDE AND CREATE LOAD
                If screenid = "cld1overrideWGT" Then doNext(False)
            End If
            Session.Remove("LoadPreviewsVals")
        End If

        Return RET
    End Function

    Private Sub doMenu()
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub updateUnitsDropDown(ByVal pUom As String)
        Dim sql As String = String.Format("Select isnull(unitsperlowestuom,1) from skuuom where sku='{0}' and consignee='{1}' and uom='{2}'", Session("CreateLoadSKU"), Session("CreateLoadConsignee"), pUom)
        Dim unitsPerLowestUom As Decimal = DataInterface.ExecuteScalar(sql)
        If unitsPerLowestUom = 0 Then unitsPerLowestUom = 1

        Dim oSku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))

        'Dim UNITS As Decimal = Session("CreateLoadUnits")
        'Session("CreateLoadUnits") = oSku.ConvertToUnits(DO1.Value("UOM")) * UNITS

        If Not String.IsNullOrEmpty(Session("CreateLoadASNRemainingUnits")) Then
            DO1.Value("Units") = Math.Max(0, Math.Floor(CType(Session("CreateLoadASNRemainingUnits"), Decimal) / unitsPerLowestUom))
            Return
        End If
        If Not String.IsNullOrEmpty(ViewState()("UnitsEntered")) Then
            If Math.Floor(CType(ViewState()("UnitsEntered"), Decimal)) < Math.Floor(Session("CreateLoadUnits") / unitsPerLowestUom) Then
                DO1.Value("Units") = Math.Floor(CType(ViewState()("UnitsEntered"), Decimal))
                Return
            End If
        End If

        If String.IsNullOrEmpty(ViewState()("DefaultRecLoadUomUnits")) Then
            'If Not String.IsNullOrEmpty(ViewState()("UnitsEntered")) Then
            '    If Math.Floor(CType(ViewState()("UnitsEntered"), Decimal)) < Math.Floor(Session("CreateLoadUnits") / unitsPerLowestUom) Then
            '        DO1.Value("Units") = Math.Floor(CType(ViewState()("UnitsEntered"), Decimal))
            '        Return
            '    End If
            'End If
            DO1.Value("Units") = Math.Floor(Session("CreateLoadUnits") / unitsPerLowestUom)
        Else
            Dim value As Decimal
            If Math.Floor(Session("CreateLoadUnits") / unitsPerLowestUom) > Math.Floor(Decimal.Parse(ViewState()("DefaultRecLoadUomUnits")) / unitsPerLowestUom) Then
                value = Math.Floor(Decimal.Parse(ViewState()("DefaultRecLoadUomUnits")) / unitsPerLowestUom)
            Else
                value = Math.Floor(Session("CreateLoadUnits") / unitsPerLowestUom)
            End If
            DO1.Value("Units") = value
            'If Not String.IsNullOrEmpty(ViewState()("UnitsEntered")) Then
            '    If ViewState()("UnitsEntered") < value Then
            '        DO1.Value("Units") = Math.Floor(CType(ViewState()("UnitsEntered"), Decimal))
            '    End If
            'End If
        End If



    End Sub

    Private Sub doNext(ByVal doPutAway As Boolean)
        'If Page.IsValid Then
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'Added for RWMS-1834 Start
        Dim formatDateTime As String = Session("RDTDateFormat")
        'Added for RWMS-1834 End

        'Added for RWMS-2726
        formatDateTime = formatDateTime.Replace("m", "M").Replace("D", "d").Replace("Y", "y")
        'Added for RWMS-1834 End

        If WMS.Logic.ReceiptHeader.Exists(Session("CreateLoadRCN")) Then
            Dim rh As New WMS.Logic.ReceiptHeader(Session("CreateLoadRCN"))
            If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Receipt is cancelled, cannot create payload"))
                Return
            End If
        End If
        'CLD1
        Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))

        'Added for RWMS-2337
        If DO1.Value("LOADID") <> "" Then
            Dim rgx As Regex = New Regex("^[a-zA-Z0-9]+$")
            If Not rgx.IsMatch(DO1.Value("LOADID")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Load ID can not have blank space and cannot be special character."))
                Return
            End If
        End If
        'End Added for RWMS-2337

        'RWMS-2137 Start
        If Not oCon.GENERATELOADID And DO1.Value("LOADID").Trim = "" Then
            'RWMS-2137 End
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Load ID can not be blank"))
            Return
        End If

        'Added for RWMS-2137(RWMS-2130) Start
        If DO1.Value("LOADID").Trim.Length > 20 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("LOADID must be up to 20 characters"))
            Return
        End If
        'Added for RWMS-2137(RWMS-2130) End

        If DO1.Value("LOCATION") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Location can not be blank"))
            Return
        End If
        'If DO1.Value("WAREHOUSEAREA") = "" Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("WAREHOUSEAREA can not be blank"))
        '    Return
        'End If

        'Commented for RWMS-2137(RWMS-2130) Start
        'Session("CreateLoadLoadId") = DO1.Value("LOADID")
        'Commented for RWMS-2137(RWMS-2130) End

        'Added for RWMS-2137(RWMS-2130) Start
        Session("CreateLoadLoadId") = DO1.Value("LOADID").Trim
        'Added for RWMS-2137(RWMS-2130) End

        Session("CreateLoadLocation") = DO1.Value("LOCATION")
        Session("CreateLoadWarehousearea") = Session("LoginWHArea") ' WMS.Logic.Warehouse.getUserWarehouseArea()
        Session("CreateLoadHUTRANS") = DO1.Value("HUTRANS")

        Session("CreateContainer") = "0"
        ' Handling Container

        'tanya add 11-11-12
        'Response.Redirect(MapVirtualPath("Screens/CLD2.aspx"))
        ' CLD2
        Session("CreateLoadStatus") = DO1.Value("STATUS")
        Session("CreateLoadHoldRC") = DO1.Value("REASONCODE")
        If DO1.Ctrl("PRINTER").Visible = True Then

            'RWMS-2623 START - replaced mobiledropdown with dropdownlist
            Dim printerDropDown As Made4Net.WebControls.DropDownList = CType(DO1.Ctrl("PRINTER"), Made4Net.WebControls.DropDownList)
            DO1.Value("PRINTER") = printerDropDown.SelectedValue
            Session("CreateLoadLabelPrinter") = printerDropDown.SelectedValue
            'RWMS-2623 END

        End If
        ' CLD3
        Session("CreateLoadUOM") = DO1.Value("UOM")
        Session("CreateLoadUnits") = DO1.Value("UNITS")
        ViewState()("UnitsEntered") = DO1.Value("UNITS")
        Session("CreateLoadNumLoads") = DO1.Value("NUMLOADS")
        Session("HUTYPE") = DO1.Value("HUTYPE")

        Session("CreateLoadContainerId") = DO1.Value("CONTAINERID")
        Dim oSku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))

        Dim dEXPIRYDATE As Date
        '2013-04-08
        'Expiry date validation –
        'The system will compare the payload’s expiry date with the minimum ship days parameter in SKUATTRIBUTES table.
        'If the expiry date entered is sooner than <today + minimum ship days >, the validation will fail, and an error message will return
        If AttributeNeeded(oSku) Then
            If Not DateTime.TryParseExact(DO1.Value("EXPIRYDATE"), formatDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, dEXPIRYDATE) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Illegal EXPIRYDATE format"))
                DO1.Value("EXPIRYDATE") = ""
                DO1.FocusField = "EXPIRYDATE"
                Return
                'RWMS-169 'System not validating EXP or MFG date during validation'
            ElseIf DateTime.Compare(dEXPIRYDATE, DateTime.Now) < 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("EXPIRYDATE Cannot be a past date"))
                DO1.Value("EXPIRYDATE") = ""
                DO1.FocusField = "EXPIRYDATE"
                Return
                'End RWMS-169 'System not validating EXP or MFG date during validation'
            Else
                'Begin RWMS-475 Added the new rules. Date: 01/09/2015

                '1. Expiry date captured must be > today
                '2. if skuattribute.DAYTORECEIVE > 0 and expdate -Today >= skuattribute.DAYTORECEIVE
                '3. if skuattribute.DAYTORECEIVE = 0 or null then do not do validate the expdate other than rule 1
                '4. Once expdate is validated then store it in attribute.expdate (this is happening today)

                Dim iShipDay As Int16 = getShipDay()
                Dim ishelflife As Int16 = getShelfLife()
                Dim isDaytoReceive As Int16 = getDayToReceive()
                If isDaytoReceive > 0 Then
                    'If dEXPIRYDATE.AddDays(ishelflife).Subtract(DateTime.Now).TotalDays < isDaytoReceive Then
                    If dEXPIRYDATE.Subtract(DateTime.Now.Date).TotalDays < isDaytoReceive Then
                        'Commented for RWMS-531
                        'Dim msg As String = "Payload expiry date {0} is sooner than allowed to receive for this product ({1}. Valid Rule: (Expirydate + shelflife) - today > DAYTORECEIVE )"
                        'End Commented for RWMS-531
                        Dim msg As String = "Payload expiry date {0} is sooner than allowed to receive for this product ({1}. Valid Rule: (Expirydate) - today >= DAYTORECEIVE )"

                        'Commented for RWMS-1834 Start
                        'msg = String.Format(msg, dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")), DateTime.Now.Date.AddDays(isDaytoReceive).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
                        'Commented for RWMS-1834 End

                        'Commented for RWMS-2690
                        ''Added for RWMS-1834 Start
                        'msg = String.Format(msg, dEXPIRYDATE.ToString(formatDateTime, DateTime.Now.Date.AddDays(isDaytoReceive).ToString(formatDateTime)))
                        ''Added for RWMS-1834 End
                        'Commented for RWMS-2690 END

                        'RWMS-2690
                        Dim expdate As DateTime
                        expdate = dEXPIRYDATE
                        Dim daytoreceive As DateTime = DateTime.Now.Date.AddDays(isDaytoReceive)
                        Dim displaydaytoreceive As String = daytoreceive.ToString(formatDateTime)
                        Dim displayexpdate As String = expdate.ToString(formatDateTime)
                        msg = String.Format(msg, displayexpdate, displaydaytoreceive)
                        'RWMS-2690 END


                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, msg)
                        DO1.Value("EXPIRYDATE") = ""
                        DO1.FocusField = "EXPIRYDATE"
                        'Added for RWMS-535,RWMS-539
                        Return
                        'End Added for RWMS-535,RWMS-539
                    End If
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("DaytoReceive must be greater than zero"))
                    DO1.Value("EXPIRYDATE") = ""
                    DO1.FocusField = "EXPIRYDATE"
                    'Added for RWMS-535,RWMS-539
                    Return
                    'End Added for RWMS-535,RWMS-539
                End If
                'Commented for RWMS-475
                'If DateTime.Compare(dEXPIRYDATE, DateTime.Now.AddDays(iShipDay)) < 0 Then

                '    Dim msg As String = "Payload expiry date {0} is sooner than allowed to receive for this product ({1})"
                '    msg = String.Format(msg, dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")), DateTime.Now.AddDays(iShipDay).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(msg))
                '    DO1.Value("EXPIRYDATE") = ""
                '    DO1.FocusField = "EXPIRYDATE"
                '    Return
                'End If
            End If
        End If


        Dim dMFGDATE As Date
        '2013-04-08
        'Manufacture date validation –
        'The system will compare the payload’s manufacture date with, Minimum Ship Day, shelf life parameter in SKUATTRIBUTES table.
        'If the result of the function:[ (manufacture date +shelf life)- today < minimum ship days]  then the system will stop the user with a pop up error
        If AttributeNeeded(oSku, "MFGDATE") Then
            If Not DateTime.TryParseExact(DO1.Value("MFGDATE"), formatDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, dMFGDATE) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Illegal MFGDATE format"))
                DO1.Value("MFGDATE") = ""
                DO1.FocusField = "MFGDATE"
                Return
                'RWMS-169 'System not validating EXP or MFG date during validation'
            ElseIf DateTime.Compare(dMFGDATE, DateTime.Now) > 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("MFGDATE Cannot be a future date"))
                DO1.Value("MFGDATE") = ""
                DO1.FocusField = "MFGDATE"
                Return
                'End RWMS-169 'System not validating EXP or MFG date during validation'
            Else

                'Begin RWMS-475 Added the new rules Date: 01/09/2015
                '1. Mfg date <= today
                '2.if skuattribute.DAYTORECEIVE > 0 and (mfgdate + skuattribute.shelflife) - today >= skuattribute.DAYTORECEIVE
                '3.if skuattribute.DAYTORECEIVE > 0 or null then do not validate the mfg date other than rule 1
                '4.Once mfgdate is validated then store it in attribute.mfgdate (this is happening today)
                Dim iShipDay As Int16 = getShipDay()
                Dim ishelflife As Int16 = getShelfLife()
                Dim isDaytoReceive As Int16 = getDayToReceive()
                If isDaytoReceive > 0 Then
                    If dMFGDATE.AddDays(ishelflife).Subtract(DateTime.Now.Date).TotalDays < isDaytoReceive Then
                        Dim msg As String = "Payload manufacture date {0} is older than allowed to receive for this product ({1}. Valid Rule: (mfgdate + shelflife) - today >= DAYTORECEIVE)"
                        'msg = String.Format(msg, dMFGDATE.ToString("MM/dd/yyyy"), DateTime.Now.AddDays(ishelflife).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))

                        'Commented for RWMS-1834 Start
                        'msg = String.Format(msg, dMFGDATE.ToString("MM/dd/yyyy"), DateTime.Now.Date.AddDays(isDaytoReceive).AddDays(-ishelflife).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
                        'Commented for RWMS-1834 End

                        'Added for RWMS-1834 Start
                        msg = String.Format(msg, dMFGDATE.ToString(formatDateTime), DateTime.Now.Date.AddDays(isDaytoReceive).AddDays(-ishelflife).ToString(formatDateTime))
                        'Added for RWMS-1834 End

                        'msg = String.Format(msg, dMFGDATE.ToString("MM/dd/yyyy"), dMFGDATE.AddDays(ishelflife).Subtract(DateTime.Now).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(msg))
                        DO1.Value("MFGDATE") = ""
                        DO1.FocusField = "MFGDATE"
                        'Added for RWMS-535,RWMS-539
                        Return
                        'End Added for RWMS-535,RWMS-539
                    End If
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("DaytoReceive must be greater than zero"))
                    DO1.Value("MFGDATE") = ""
                    DO1.FocusField = "MFGDATE"
                    'Added for RWMS-535,RWMS-539
                    Return
                    'End Added for RWMS-535,RWMS-539
                End If
                'END RWMS-475
                'Commented for RWMS-475
                'If dMFGDATE.AddDays(ishelflife).Subtract(DateTime.Now).TotalDays < iShipDay Then
                '    Dim msg As String = "Payload manufacture date {0} is sooner than allowed to receive for this product ({1})"
                '    msg = String.Format(msg, dMFGDATE.ToString("MM/dd/yyyy"), DateTime.Now.AddDays(iShipDay).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(msg))
                '    DO1.Value("MFGDATE") = ""
                '    DO1.FocusField = "MFGDATE"
                '    Return
                'End If
            End If
        End If

        'Added for RWMS-151
        Dim WEIGHT As String
        'End Added for RWMS-151

        Dim oAttributes As WMS.Logic.AttributesCollection
        oAttributes = ExtractAttributeValues()
        Dim wgtVal As New RWMS.Logic.WeightValidator
        'Dim screenid As String = ""
        'If Not IsNothing(Request.QueryString("sourcescreen")) Then
        '    screenid = Request.QueryString("sourcescreen")
        'End If
        Dim screenid As String = ""
        If Not IsNothing(Session("LoadPreviewsVals")) Then
            screenid = Session("LoadPreviewsVals")
            Session.Remove("LoadPreviewsVals")
        End If

        If Not ValidateLoad(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"), Session("CreateLoadSKU"), Session("CreateLoadLoadId"), _
                   Session("CreateLoadUOM"), Session("CreateLoadLocation"), Session("CreateLoadWarehousearea"), Session("CreateLoadUnits"), Session("CreateLoadStatus"), _
                   Session("CreateLoadHoldRC"), Session("CreateLoadNumLoads"), Logic.GetCurrentUser, oAttributes, Session("CreateLoadLabelPrinter"), "", "", "") Then
            Exit Sub
        End If

        If wgtVal.WeightNeeded(oSku) And screenid <> "cld1overrideWGT" Then
            If DO1.Value("WEIGHT") <> "" Then

                If Not IsNumeric(DO1.Value("WEIGHT")) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Illegal weight")
                    Exit Sub
                    'RWMS-2587 START
                ElseIf (DO1.Value("WEIGHT") < 0) Then
                    DO1.Value("WEIGHT") = ""
                    MessageQue.Enqueue(t.Translate("Please enter valid weight. Weight cannot be Negative."))
                    Return
                    'RWMS-2587 END
                    'RWMS-2633 START
                ElseIf (DO1.Value("WEIGHT") > 999999999) Then
                    DO1.Value("WEIGHT") = ""
                    MessageQue.Enqueue(t.Translate("Weight must be less than 1000000000."))
                    Return
                    'RWMS-2633 END
                End If

                Dim gotoOverride As Boolean = False
                Dim gotoOverrideMessage As String = ""
                Dim errMsg As String = ""
                'Commented for RWMS-151
                'Dim WEIGHT As String = DO1.Value("WEIGHT")
                'End Commented for RWMS-151

                'Added for RWMS-151
                Dim caseUNITS As Decimal = Session("CreateLoadUnits")
                'End Added for RWMS-151

                Dim UNITS As Decimal = Session("CreateLoadUnits")
                UNITS = oSku.ConvertToUnits(DO1.Value("UOM")) * UNITS

                'Added for RWMS-151
                If Session("CreateLoadAVGWEIGHT") > 0 Then
                    WEIGHT = DO1.Value("AVGWEIGHT") * oSku.ConvertUnitsToUom("CASE", UNITS) ' Session("CreateLoadAVGWEIGHT") / Session("CreateLoadQTYEXPECTED")
                Else
                    WEIGHT = DO1.Value("WEIGHT")
                End If
                'End Added for RWMS-151

                If Not wgtVal.ValidateWeightSku(oSku, WEIGHT, UNITS, gotoOverride, gotoOverrideMessage, errMsg, False) Then
                    If gotoOverride Then
                        oAttributes.Item("WEIGHT") = WEIGHT
                        Session("CreateLoadUnitsEach") = UNITS
                        Session("WeightOverridConfirm") = WEIGHT

                        Session("CreateLoadAttributes") = oAttributes

                        Session("ERROROVERRIDE") = errMsg

                        'Commeted for RWMS-151
                        'gotoOverrideWeight()
                        'End Commeted for RWMS-151
                        'Added for RWMS-151
                        If caseUNITS > 1 And Session("CreateLoadAVGWEIGHT") = 0 Then
                            Response.Redirect(MapVirtualPath("Screens/WeightCaptureNeeded.aspx?sourcescreen=cld1overrideWGT"))
                        Else
                            gotoOverrideWeight()
                        End If
                        'Added for RWMS-151
                    Else
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errMsg)
                    End If

                    Exit Sub
                Else
                    oAttributes.Item("WEIGHT") = WEIGHT
                    Session("CreateLoadAttributes") = oAttributes
                    'Added for RWMS-151
                    If caseUNITS > 1 Then
                        Response.Redirect(MapVirtualPath("Screens/WeightCaptureNeeded.aspx"))
                    End If
                    'End Added for RWMS-151
                End If
            End If
            'RWMS-2587 START
        ElseIf wgtVal.WeightNeeded(oSku) And screenid = "cld1overrideWGT" Then
            If DO1.Value("WEIGHT") <> "" Then

                'Commented for RWMS-2633
                'If (DO1.Value("WEIGHT") < 0) Then
                '    DO1.Value("WEIGHT") = ""
                '    MessageQue.Enqueue(t.Translate("Please enter valid weight. Weight cannot be Negative."))
                '    Return
                'End If
                'Commented for RWMS-2633 END
                'RWMS-2633 START
                If Not IsNumeric(DO1.Value("WEIGHT")) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Illegal weight")
                    Exit Sub
                ElseIf (DO1.Value("WEIGHT") < 0) Then
                    DO1.Value("WEIGHT") = ""
                    MessageQue.Enqueue(t.Translate("Please enter valid weight. Weight cannot be Negative."))
                    Return
                    'RWMS-2633 START
                ElseIf (DO1.Value("WEIGHT") > 999999999) Then
                    DO1.Value("WEIGHT") = ""
                    MessageQue.Enqueue(t.Translate("Weight must be less than 1000000000."))
                    Return
                    'RWMS-2633 END
                End If
                'RWMS-2633 START

            End If
            'RWMS-2587 END
        End If



        If Not validateHUType() Then
            Exit Sub
        End If

        'Dim RECOVERRIDEVALIDATOR As String
        'RECOVERRIDEVALIDATOR = getRECOVERRIDEVALIDATOR()
        ''If SkuMenage("EXPIRYDATE") Then

        'If EXPIRYDATENeeded(oSku) And Not String.IsNullOrEmpty(RECOVERRIDEVALIDATOR) Then
        '    '      testValidation()

        '    'New Validation with expression evaluation
        '    Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

        '    vals.Add("EXPIRYDATE", dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
        '    vals.Add("DAYSTORECEIVE", getDAYSTORECEIVE().ToString())

        '    Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
        '    exprEval.FieldValues = vals
        '    Dim statement As String = "[0];func:" & RECOVERRIDEVALIDATOR & "(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
        '    'Dim statement As String = "[0];func:ValidateExpiryDate(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
        '    '
        '    Dim ret As String
        '    Try
        '        ret = exprEval.Evaluate(statement)
        '    Catch ex As Exception
        '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal validation function") & statement & ex.Message)
        '        Return
        '    End Try

        '    Dim returnedResponse() As String = ret.Split(";")
        '    If returnedResponse(0) = "0" Then

        '        Session("CreateLoadAttributes") = oAttributes
        '        gotoOverride(returnedResponse(1))
        '        Exit Sub

        '    ElseIf returnedResponse(0) = "-1" Then
        '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret)
        '        Return
        '    End If
        '    ' End If
        'End If

        ':2013-04-21
        'If DO1.Value("CONTAINERID") = "" And DO1.Value("HUTYPE") = "" Then
        '    Session("CreateLoadAttributes") = oAttributes

        '    gotoOverrideHUType()
        'End If

        If DO1.Value("CONTAINERID") <> "" Then
            Dim CheckCntSql As String = "SELECT * FROM CONTAINER WHERE CONTAINER='" & DO1.Value("CONTAINERID") & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            'Checking if container already exists, if not creating new one
            If dt.Rows.Count > 0 Then
                Session("CreateLoadContainerID") = DO1.Value("CONTAINERID")
            Else
                If DO1.Value("HUTYPE") = "" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("HUType is empty"))
                    Return
                End If
                ' Remember it in session if there is one load
                Dim oCont As WMS.Logic.Container
                oCont = New WMS.Logic.Container
                oCont.ContainerId = DO1.Value("CONTAINERID")
                oCont.HandlingUnitType = DO1.Value("HUTYPE")
                oCont.Location = DO1.Value("LOCATION")
                oCont.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea()
                oCont.Post(WMS.Logic.Common.GetCurrentUser)

                Session("CreateLoadContainerID") = DO1.Value("CONTAINERID")

            End If
        Else
            If DO1.Value("HUTYPE") <> "" Then
                ' Create new container with counter

                ' Remember it in session if there is one load
                If DO1.Value("NUMLOADS") = 1 Then
                    Dim oCont1 As WMS.Logic.Container
                    oCont1 = New WMS.Logic.Container
                    oCont1.ContainerId = Made4Net.Shared.getNextCounter("CONTAINER")
                    oCont1.HandlingUnitType = DO1.Value("HUTYPE")
                    oCont1.Location = DO1.Value("LOCATION")
                    oCont1.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea()
                    oCont1.Post(WMS.Logic.Common.GetCurrentUser)
                    DO1.Value("CONTAINERID") = oCont1.ContainerId
                    Session("CreateLoadContainerID") = DO1.Value("CONTAINERID")
                Else
                    Session("HUTYPE") = DO1.Value("HUTYPE")
                    Session("CreateContainer") = "1"
                    Session("CreateLoadContainerID") = ""
                End If

            Else
                Session("CreateLoadContainerID") = ""
            End If
        End If

        'Session("CreateLoadDoPutAway") = doPutAway
        '            Try
        SubmitCreate(Nothing, doPutAway)
        'Catch ex As System.Threading.ThreadAbortException
        'Catch ex As Made4Net.Shared.M4NException
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        '    Return
        'Catch ex As Exception
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        '    Return
        'End Try
        Session("CreateLoadUnits") = getRemainingUnits()
        updateUnitsDropDown(DO1.Value("UOM"))
        'If Not String.IsNullOrEmpty(Session("ReceiveByIdASNID")) AndAlso Not String.IsNullOrEmpty(Session("CreateLoadContainerID")) Then
        '    DO1.Value("CONTAINERID") = Session("CreateLoadContainerID")
        'End If
        If Session("CreateLoadUnits").ToString <= 0 Then
            Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLine.aspx"))
            Return
        End If

        DO1.Value("CONTAINERID") = ""
        If screenid = "RDTCLD1OVERRIDEHUTYPE" Or screenid.Contains("cld1overrideWGT") Then
            Try
                Session.Remove("LoadPreviewsVals")
                Response.Redirect(MapVirtualPath("Screens/CLD1.aspx"))
            Catch ex As System.Threading.ThreadAbortException

            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                    Throw
                End If
            End Try
        End If

        'End If
    End Sub



    'false - exit from donext
    'true - continue to donext()
    Public Function validateHUType() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim ret As Boolean = True
        If DO1.Value("HUTYPE") = "" And DO1.Value("CONTAINERID") <> "" Then
            If Not WMS.Logic.Container.Exists(DO1.Value("CONTAINERID")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container does not exist"))
                Return False
            End If
            'Return True
            'ElseIf DO1.Value("HUTYPE") = "" And DO1.Value("CONTAINERID") <> "" Then
            '    gotoOverrideHUType()
            '    Return False
        End If
        Return ret
    End Function

    Private Function AttributeNeeded(ByVal pSKU As WMS.Logic.SKU, Optional ByVal attName As String = "EXPIRYDATE") As Boolean
        'Added for RWMS-531
        If Not IsNothing(pSKU.SKUClass) Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = attName Then
                    If String.IsNullOrEmpty(DO1.Value(oAtt.Name.ToUpper)) And oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                        Return False
                    Else
                        Return True
                    End If
                    'If oAtt.Name.ToUpper = "EXPIRYDATE" AndAlso _
                    '(oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                End If
            Next
        End If

        Return False
    End Function
    'End Added for RWMS-531

    Private Function getShipDay() As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(MINDAYSTOSHIP, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    Private Function getShelfLife() As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(SHELFLIFE, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    'RWMS-475 - Adding new method to get DAYTORECEIVE column value by passing SKU and Consignee
    Private Function getDayToReceive() As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(DAYTORECEIVE, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        Logger.Write(String.Concat("The SQL is :", sql), "General")
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                Throw
            End If
        End Try
        Return ret
    End Function
    'End RWMS-475



    Public Sub gotoOverrideHUType()

        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1OVERRIDEHUTYPE'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url) & "?sourcescreen=RDTCLD1")

    End Sub

    Public Sub gotoOverrideWeight()

        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'cld1overrideWGT'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url) & "?sourcescreen=RDTCLD1")

    End Sub



    Public Function ValidateLoad(ByVal pReceipt As String, ByVal pLine As Int32, ByVal pSku As String, ByVal pLoadId As String, ByVal pUOM As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pQty As Double, ByVal pStat As String, ByVal pHoldRc As String, ByVal pNumLoads As Int32, ByVal pUser As String, ByVal oAttributesCollection As AttributesCollection, ByVal lblPrinter As String, ByVal pDocumentType As String, ByVal pHandlingUnitId As String, ByVal pHandlingUnitType As String) As Boolean
        Dim retVal As Boolean = True
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        If pQty = 0 Then
            retVal = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Cannot create load with zero quantity"))
        End If

        If pQty < 0 Then
            retVal = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Cannot create load with negative quantity"))
        End If
        Dim rh As ReceiptHeader = ReceiptHeader.GetReceipt(pReceipt)
        Dim oSkuClass As SkuClass
        Dim oSku As SKU
        Dim isSubstituteSku As Boolean = False
        Dim AutoPrintLoadLabels As Boolean = False
        If rh.STATUS = "CLOSE" Then
            retVal = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Receipt is CLOSED"))
        End If
        If Session("CreateLoadRCNMultipleLines") Then
            pLine = 1
        End If
        Dim rl As ReceiptDetail = rh.LINES.Line(pLine)

        If Not WMS.Logic.Location.Exists(DO1.Value("LOCATION"), WMS.Logic.Warehouse.getUserWarehouseArea()) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Illegal Location"))
            retVal = False
        End If

        'Validate substitute sku
        If rl.SKU.ToLower <> pSku.ToLower Then
            Dim tmpSku As New SKU(rl.CONSIGNEE, rl.SKU)
            If Not tmpSku.ContainsSubstituteSku(pSku) Then
                retVal = False
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("SKU is not Valid sku for this receipt"))
            End If
            oSku = New SKU(rl.CONSIGNEE, pSku)
            oSkuClass = oSku.SKUClass
            isSubstituteSku = True
            rl.UpdateOriginalSku(pSku, pUser)
        Else
            oSku = New SKU(rl.CONSIGNEE, rl.SKU)
            oSkuClass = oSku.SKUClass
        End If

        If (pNumLoads = 1 And (pLoadId Is Nothing Or pLoadId = "")) Then
            'oSkuClass = New SKU(rl.CONSIGNEE, rl.SKU).SKUClass
            If Not Consignee.AutoGenerateLoadID(rl.CONSIGNEE) Then
                retVal = False
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid Load ID"))
                'Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid Load ID", "Invalid Load ID")
                'Throw m4nEx
            End If
        End If

        'Attribute Validation
        If Not oSkuClass Is Nothing Then
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                Dim typeValidationResult As Int32

                If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                    If oAttributesCollection Is Nothing Then Continue For
                    Select Case oLoadAtt.Type
                        Case AttributeType.String
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value OrElse oAttributesCollection(oLoadAtt.Name) = "" Then Continue For
                        Case AttributeType.Decimal, AttributeType.Integer
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then Continue For
                        Case AttributeType.DateTime
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then Continue For
                            Try
                                If oAttributesCollection(oLoadAtt.Name) = String.Empty Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                            Try
                                If oAttributesCollection(oLoadAtt.Name) = DateTime.MinValue Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                        Case AttributeType.Boolean
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Then Continue For
                    End Select
                ElseIf oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oAttributesCollection Is Nothing Then
                        retVal = False
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name)))
                    End If

                    If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                        retVal = False
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name)))
                    End If
                End If
                If oLoadAtt.CaptureAtReceiving <> SkuClassLoadAttribute.CaptureType.NoCapture Then
                    ' Validate that the attributes supplied are valid
                    typeValidationResult = ValidateAttributeByType(oLoadAtt, oAttributesCollection(oLoadAtt.Name))
                    If typeValidationResult = -1 Then
                        retVal = False
                        ' Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Attribute #param0# is not Valid", "Attribute #param0# is not Valid")
                        'm4nEx.Params.Add("AttName", oLoadAtt.Name)
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(String.Format("Attribute {0} is not Valid", oLoadAtt.Name)))
                        ' Throw m4nEx
                    ElseIf typeValidationResult = 0 Then
                        'Continue For
                    End If
                    ' Validator
                    If Not oLoadAtt.ReceivingValidator Is Nothing Then
                        'Old Validation Code
                        'If Not oLoadAtt.ReceivingValidator.Validate(rh, pLine, pUOM, pLocation, pQty, pStat, pHoldRc, pUser, oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name), oAttributesCollection) Then
                        '    Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        'End If

                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                        vals.Add(oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name))
                        vals.Add("RECEIPT", pReceipt)
                        vals.Add("LINE", CStr(pLine))
                        vals.Add("LOADSTATUS", pStat)
                        Dim ret As String = oLoadAtt.Evaluate(SkuClassLoadAttribute.EvaluationType.Receiving, vals)
                        Dim returnedResponse() As String = ret.Split(";")
                        'If ret = "-1" Then
                        If returnedResponse(0) = "-1" Then
                            If returnedResponse.Length > 1 Then
                                retVal = False
                                '  Throw New M4NException(New Exception, "Invalid Attribute Value " & oLoadAtt.Name & ". " & returnedResponse(1), "Invalid Attribute Value " & oLoadAtt.Name & "." & returnedResponse(1))
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid Attribute Value "))

                            Else
                                retVal = False
                                '                                Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid Attribute Value "))

                            End If
                            'Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                    End If
                End If
            Next
        End If
        'End Of Attribute Validation

        Return retVal
    End Function

    Private Function ValidateAttributeByType(ByVal oAtt As SkuClassLoadAttribute, ByVal oAttVal As Object) As Int32
        Select Case oAtt.Type
            Case Logic.AttributeType.DateTime
                Dim Val As DateTime
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, DateTime)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.Decimal
                Dim Val As Decimal
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Decimal)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.Integer
                Dim Val As Int32
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Int32)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.String
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
        End Select
    End Function


    Private Function testValidation() As Boolean
        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

        vals.Add("CONSIGNEE", "TEST")
        vals.Add("SKU", "1234")

        Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
        exprEval.FieldValues = vals
        Dim statement As String = "[0];func:ValidateTest(FIELD:CONSIGNEE,FIELD:SKU)"
        'Dim statement As String = "[0];func:ValidateExpiryDate(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
        '
        Dim ret As String
        Try
            ret = exprEval.Evaluate(statement)
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Illegal TEST validation function")
        End Try

        Dim returnedResponse() As String = ret.Split(";")
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ret)

    End Function


    Private Function EXPIRYDATENeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        If Not IsNothing(pSKU.SKUClass) Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = "EXPIRYDATE" Then
                    If String.IsNullOrEmpty(DO1.Value(oAtt.Name.ToUpper)) And oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                        Return False
                    Else
                        Return True
                    End If
                    'If oAtt.Name.ToUpper = "EXPIRYDATE" AndAlso _
                    '(oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                End If
            Next
        End If

        Return False
    End Function

    Private Sub doChangeLine()
        Try
            MobileUtils.ClearCreateLoadChangeLineSession()
            Session.Remove("CreateContainer")
            doCloseASN(False)
            Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLine.aspx"))
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gotoOverride(ByVal errorOverride As String)
        Session("CreateLoadStatus") = DO1.Value("STATUS")
        Session("CreateLoadHoldRC") = DO1.Value("REASONCODE")
        If DO1.Ctrl("PRINTER").Visible = True Then

            'RWMS-2623 Commented
            'Session("CreateLoadLabelPrinter") = DO1.Value("PRINTER")
            'RWMS-2623 Commented END

            'RWMS-2623 START - replaced mobiledropdown with dropdownlist
            Dim printerDropDown As Made4Net.WebControls.DropDownList = CType(DO1.Ctrl("PRINTER"), Made4Net.WebControls.DropDownList)
            DO1.Value("PRINTER") = printerDropDown.SelectedValue
            Session("CreateLoadLabelPrinter") = printerDropDown.SelectedValue
            'RWMS-2623 END

        End If
        ' CLD3
        Session("CreateLoadUOM") = DO1.Value("UOM")
        Session("CreateLoadUnits") = DO1.Value("UNITS")
        ViewState()("UnitsEntered") = DO1.Value("UNITS")
        Session("CreateLoadNumLoads") = DO1.Value("NUMLOADS")

        Session("ERROROVERRIDE") = errorOverride
        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1OVERRIDE'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("RECEIPT")
        'RWMS-518
        'Commented for the JIRA RWMS-518
        'DO1.AddLabelLine("RECEIPTLINE")
        'DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("SKU")
        'DO1.AddLabelLine("SKUDESC")
        'tanya add 11-11-12
        DO1.AddLabelLine("Ti/Hi")

        'DO1.AddLabelLine("CPQ")
        'Commented for RWMS-454, RWMS-330
        'If Not String.IsNullOrEmpty(getDangerous()) Then
        '    DO1.AddLabelLine("Dangerous Goods", "Dangerous Goods", "Yes")
        'End If
        'End Commented for RWMS-454, RWMS-330
        'Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))
        'If oCon.GENERATELOADID Then
        '    DO1.AddTextboxLine("LOADID")
        'Else
        DO1.AddTextboxLine("LOADID", False, "Create")
        'End If
        DO1.AddTextboxLine("LOCATION", True, "Create")
        'DO1.AddTextbox Line("WAREHOUSEAREA")
        DO1.AddTextboxLine("UNITS", True, "Create")
        setAttributes()
        'Added for RWMS-151
        If Session("CreateLoadAVGWEIGHT") > 0 And Not IsNothing(Session("CreateLoadQTYEXPECTED")) Then
            Dim w As Decimal
            Try
                w = Session("CreateLoadAVGWEIGHT") / Session("CreateLoadQTYEXPECTED")
                DO1.AddLabelLine("AVGWEIGHT", "AVGWEIGHT", w)
                DO1.Value("WEIGHT") = w

            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                    Throw
                End If
            End Try
        End If
        'End Added for RWMS-151
        DO1.AddTextboxLine("CONTAINERID")
        DO1.AddTextboxLine("NUMLOADS")
        '   DO1.setVisibility("NUMLOADS", False)

        'Commented for RWMS-2298
        'DO1.AddTextboxLine("Printer")
        'End Commented for RWMS-2298
        'Added for RWMS-2298

        'RWMS-2623 START - Replaced mobiledropdown with dropdownlist
        'DO1.AddDropDown("Printer")
        DO1.AddDropDownList("Printer")
        'RWMS-2623 END

        'End Added for RWMS-2298
        DO1.AddDropDown("UOM")
        DO1.AddDropDown("STATUS")
        ' DO1.AddDropDown("HUTYPE")
        'DO1.AddDropDown("HUTRANS")
        DO1.AddDropDown("ReasonCode")

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

        'DO1.AddSpacer()

    End Sub

    'Added for RWMS-2476 Start
    Private Sub HideCloseReceipt()
        Dim R As New Made4Net.WebControls.RecursiveControlFinder
        Dim btns As Made4Net.DataAccess.Collections.GenericCollection = R.SearchByType(GetType(Made4Net.WebControls.ExecutionButton), Me.Controls)
        Dim btn As Made4Net.WebControls.ExecutionButton, n As Int32
        For n = 0 To btns.Count - 1
            btn = CType(btns(n), Made4Net.WebControls.ExecutionButton)
            If btn.Text = "CloseReceipt" Then
                btn.Visible = False
            End If
        Next
    End Sub
    'Added for RWMS-2476 End

    'Added for RWMS-151 - LATEST
    Private Sub setAttributesFromBarCode(ByVal sessionDictName As String)
        Try
            If Not IsNothing(Session(sessionDictName)) Then
                Dim ParceGS1 As Dictionary(Of String, String) = Session(sessionDictName)

                For Each pair As KeyValuePair(Of String, String) In ParceGS1
                    If pair.Key <> "SKU" Then
                        Try
                            DO1.Value(pair.Key) = pair.Value
                        Catch ex As Exception
                            If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                                Throw
                            End If
                        End Try
                    End If
                Next
            End If

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Made4Net.General.Constants.UI_Policy) Then
                Throw
            End If
        End Try
    End Sub
    'End Added for RWMS-151 - LATEST

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "changeline"
                doChangeLine()
            Case "create&pickup"
                doNext(True)
                ' clearLoadSelectRCNLineSession()
            Case "create"
                doNext(False)
                ' clearLoadSelectRCNLineSession()
            Case "closereceipt"
                doCloseReceipt()
                ' clearLoadSelectRCNLineSession()
                DO1.Value("EXPIRYDATE") = ""
                DO1.FocusField = "EXPIRYDATE"
        End Select
        ' ClearAttributes()
    End Sub

    Sub clearLoadSelectRCNLineSession()
        Session.Remove("SKUCODE")
        Session.Remove("SKUSEL_RECEIPTLINE")
        Session.Remove("SKUSEL_CONSIGNEE")

    End Sub

    'CLD2
    Private Sub SetStatusAndPrinter()
        'RWMS-2623 Commented START
        'If Session("CreateLoadLabelPrinter") Is Nothing Or Session("CreateLoadLabelPrinter") = "" Then
        '    Try
        '        Session("CreateLoadLabelPrinter") = Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT DEFAULTPRINTER FROM LABELS WHERE LABELNAME = 'LOAD'"))
        '    Catch ex As Exception
        '    End Try
        'End If
        'RWMS-2623 Commented END

        Dim dd As Made4Net.WebControls.MobileDropDown
        dd = DO1.Ctrl("STATUS")
        dd.AllOption = False
        dd.TableName = "INVSTATUSES"
        dd.ValueField = "CODE"
        dd.TextField = "DESCRIPTION"
        dd.Where = " CODE <> 'LIMBO'"
        dd.DataBind()
        If Not Session("CreateLoadStatus") Is Nothing Then
            dd.SelectedValue = Session("CreateLoadStatus")
        End If
        Try
            Dim oSku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
            dd.SelectedValue = oSku.INITIALSTATUS
        Catch ex As Exception
        End Try
        dd = DO1.Ctrl("REASONCODE")
        dd.AllOption = True
        dd.TableName = "CODELISTDETAIL"
        dd.ValueField = "CODE"
        dd.TextField = "DESCRIPTION"
        dd.Where = "CODELISTCODE = 'INVHOLDRC'"
        dd.DataBind()
        If Not Session("CreateLoadHoldRC") Is Nothing Then
            dd.SelectedValue = Session("CreateLoadHoldRC")
        End If
        If WMS.Logic.Consignee.AutoPrintLoadIdOnReceiving(Session("CreateLoadConsignee")) Then
            DO1.setVisibility("PRINTER", True)

            'Commented for RWMS-2298
            'Dim prnt As String = MobileUtils.GetMHEDefaultPrinter()
            'If prnt <> "" Then
            '    DO1.Value("PRINTER") = prnt
            'Else
            '    DO1.Value("PRINTER") = Session("CreateLoadLabelPrinter")
            'End If
            'End Commented for RWMS-2298

            'Added for RWMS-2298

            'RWMS-2623 START - replaced mobile dropdown with dropdownlist
            'Dim printerDropDown As Made4Net.WebControls.DropDownList
            Dim printerDropDown As Made4Net.WebControls.DropDownList = CType(DO1.Ctrl("PRINTER"), Made4Net.WebControls.DropDownList)
            'printerDropDown = DO1.Ctrl("PRINTER")
            'RWMS-2623 START

            printerDropDown.AllOption = False
            printerDropDown.TableName = "LABELPRINTERS"
            printerDropDown.ValueField = "PRINTQNAME"
            printerDropDown.TextField = "PRINTQNAME"
            'dd.Where = " CODE <> 'LIMBO'"
            printerDropDown.DataBind()
            'End Added for RWMS-2298

            If Not Session("CreateLoadLabelPrinter") Is Nothing Then
                printerDropDown.SelectedValue = Session("CreateLoadLabelPrinter")
            End If
            'RWMS-2623 END

        Else
            DO1.setVisibility("PRINTER", False)
        End If
    End Sub
    'CLD3
    Private Sub SetUOM()
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
        dd.AllOption = False
        dd.TableName = "SKUUOMDESC"
        dd.ValueField = "UOM"
        dd.TextField = "DESCRIPTION"
        dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        AddHandler dd.OnPrevButtonClick, AddressOf uomDropDownPreviousButtonClick
        AddHandler dd.OnNextButtonClick, AddressOf uomDropDownNextButtonClick
        dd.DataBind()
        'Commented for RWMS-151
        'If Not Session("CreateLoadUOM") Is Nothing Then
        '    dd.SelectedValue = Session("CreateLoadUOM")

        '    'Dim screenid As String = ""
        '    'If Not IsNothing(Request.QueryString("sourcescreen")) Then
        '    '    screenid = Request.QueryString("sourcescreen")
        '    'End If
        '    Dim screenid As String = ""
        '    If Not IsNothing(Session("LoadPreviewsVals")) Then
        '        screenid = Session("LoadPreviewsVals")
        '    End If
        '    If Not (screenid = "RDTCLD1OVERRIDEHUTYPE" Or screenid.Contains("cld1overrideWGT")) Then

        '        Session("CreateLoadUnits") = getRemainingUnits()
        '    End If

        '    updateUnitsDropDown(DO1.Value("UOM"))
        '    DO1.Value("NUMLOADS") = 1
        'Else
        'End Commented for RWMS-151
        'Dim sql As String = String.Format("Select QTYEXPECTED-QTYRECEIVED from receiptdetail where receipt='{0}' and receiptline='{1}'", Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
        'Dim qtyExpected As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'Session("CreateLoadUnits") = qtyExpected


        'Commented for RWMS-151
        'Session("CreateLoadUnits") = getRemainingUnits()
        'End Commented for RWMS-151
        'Added for RWMS-151
        Dim screenid As String = ""
        If Not IsNothing(Session("LoadPreviewsVals")) Then
            screenid = Session("LoadPreviewsVals")
        End If

        'when return from RDTCLD1OVERRIDEHUTYPE screen, need to return all previes values
        'RWMS-436 - Checking the condition for Remainingunits < Resulted pallet value i.e. resTiHi value
        If screenid <> "RDTCLD1OVERRIDEHUTYPE" And Not screenid.Contains("cld1overrideWGT") Then
            If getRemainingUnits() < palletQty Then
                Session("CreateLoadUnits") = getRemainingUnits()
            Else
                Session("CreateLoadUnits") = palletQty
            End If

        End If
        'End Added for RWMS-151


        Dim sql As String = String.Format("Select PromptForRcvQty from consignee where consignee='{0}'", Session("CreateLoadConsignee"))
        Dim promptForRcvQty As Boolean = System.Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))


        If promptForRcvQty Then
            sql = "SELECT ISNULL(DEFAULTRECLoadUOM,'') FROM SKU WHERE CONSIGNEE = '" & Session("CreateLoadConsignee") & "' and SKU = '" & Session("CreateLoadSKU") & "'"
            Dim defaultRecLoadUom As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If defaultRecLoadUom <> "" Then
                'sql = String.Format("Select unitsperlowestuom from skuuom where consignee='{0}' and sku='{1}' and uom='{2}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"), defaultRecLoadUom)
                ' = DataInterface.ExecuteScalar(sql)
                sql = String.Format("Select UnitsPerLowestUom from skuuom where consignee='{0}' and sku='{1}' and uom='{2}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"), defaultRecLoadUom)
                'Dim unitsPerLowestUom As Decimal =
                ViewState()("DefaultRecLoadUomUnits") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                'unitsPerLowestUom()
                'sql = String.Format("Select QTYEXPECTED-QTYRECEIVED from receiptdetail where receipt='{0}' and receiptline='{1}'", Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
                'Dim qtyExpected As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                'Session("CreateLoadUnits") = qtyExpected
            End If
            sql = "SELECT ISNULL(DEFAULTRECUOM,'') FROM SKU WHERE CONSIGNEE = '" & Session("CreateLoadConsignee") & "' and SKU = '" & Session("CreateLoadSKU") & "'"
            Dim usedUOM As String = DataInterface.ExecuteScalar(sql)
            If usedUOM = "" Then
                'Commented for RWMS-25252 Start
                'sql = "Select top(1)uom from skuuom WHERE CONSIGNEE = '" & Session("CreateLoadConsignee") & "' and SKU = '" & Session("CreateLoadSKU") & "'"
                'Commented for RWMS-25252 End

                'Added for RWMS-2525 Start
                sql = String.Format("SELECT TOP 1 isnull(UOM,'') FROM SKUUOM WHERE (LOWERUOM = '' OR LOWERUOM IS NULL) AND (SKU = '{0}') AND (CONSIGNEE  = '{1}') ", Session("CreateLoadSKU"), Session("CreateLoadConsignee"))
                'Added for RWMS-2525 End

                usedUOM = DataInterface.ExecuteScalar(sql)
            End If
            Try
                dd.SelectedValue = usedUOM
            Catch ex As Exception
            End Try

            sql = String.Format("Select UnitsPerLowestUom from skuuom where consignee='{0}' and sku='{1}' and uom='{2}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"), usedUOM)
            Dim unitsPerLowestUom As Decimal = CType(DataInterface.ExecuteScalar(sql), Decimal)
            updateUnitsDropDown(DO1.Value("UOM"))
            'DO1.Value("Units") = Session("CreateLoadUnits") / unitsPerLowestUom
            DO1.Value("NUMLOADS") = 1
            Return
        End If
        Dim dfuom As String = Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT ISNULL(DEFAULTRECUOM,'') FROM SKU WHERE CONSIGNEE = '" & Session("CreateLoadConsignee") & "' and SKU = '" & Session("CreateLoadSKU") & "'")
        Try
            If dfuom <> "" Then
                dd.SelectedValue = dfuom
            End If
        Catch ex As Exception
        End Try
        'updateUnitsDropDown(dfuom)
        DO1.Value("NUMLOADS") = 1
        'Commented for RWMS-151
        'End If
        'End Commented for RWMS-151
    End Sub

    Private Sub uomDropDownPreviousButtonClick(ByVal Source As Object, ByVal e As EventArgs)
        Dim dd As Made4Net.WebControls.MobileDropDown = CType(DO1.Ctrl("UOM"), Made4Net.WebControls.MobileDropDown)
        Dim uom As String
        If dd.SelectedIndex = 0 Then
            uom = dd.TableData.Rows(dd.TableData.Rows.Count - 1)("Value").ToString()
        Else
            uom = dd.TableData.Rows(dd.SelectedIndex - 1)("Value").ToString()
        End If
        ViewState()("UnitsEntered") = ""
        updateUnitsDropDown(uom)
    End Sub

    Private Sub uomDropDownNextButtonClick(ByVal Source As Object, ByVal e As EventArgs)
        Dim dd As Made4Net.WebControls.MobileDropDown = CType(DO1.Ctrl("UOM"), Made4Net.WebControls.MobileDropDown)
        Dim uom As String
        If dd.SelectedIndex = dd.TableData.Rows.Count - 1 Then
            uom = dd.TableData.Rows(0)("Value").ToString()
        Else
            uom = dd.TableData.Rows(dd.SelectedIndex + 1)("Value").ToString()
        End If
        ViewState()("UnitsEntered") = ""
        updateUnitsDropDown(uom)
    End Sub

    Private Sub SetHUTrans()
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("HUTRANS")
        dd.AllOption = False
        dd.TableName = "CODELISTDETAIL"
        dd.ValueField = "CODE"
        dd.TextField = "DESCRIPTION"
        dd.Where = String.Format("CODELISTCODE = 'ISHUTRANS'")
        dd.DataBind()
        Dim dfuom As String = Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT ISNULL(HUTYPE,'') FROM SKU WHERE CONSIGNEE = '" & Session("CreateLoadConsignee") & "' and SKU = '" & Session("CreateLoadSKU") & "'")
        Try
            If dfuom <> "" Then
                dd.SelectedValue = dfuom
            Else
                dd.SelectedValue = "NO"
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetHUTYpe()
        'Commented for RWMS-129
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("HUTYPE")
        dd.AllOption = True
        dd.AllOptionText = ""
        dd.TableName = "HANDELINGUNITTYPE"
        dd.ValueField = "CONTAINER"
        dd.TextField = "CONTAINERDESC"
        dd.DataBind()
        'End Commented for RWMS-129
        'Added for RWMS-129
        'Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("HUTYPE")
        'dd.AllOption = False
        ''dd.AllOptionText = ""
        'dd.TableName = "HANDELINGUNITTYPE"
        'dd.ValueField = "CONTAINER"
        'dd.TextField = "CONTAINERDESC"
        'dd.DataBind()
        'Dim dfuom As String = Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT CONTAINER FROM HANDELINGUNITTYPE")
        'Try
        '    If dfuom <> "" Then
        '        dd.SelectedValue = dfuom
        '    End If
        'Catch ex As Exception
        'End Try
        'End Added for RWMS-129
    End Sub

    Private Function SkuMenage(ByVal attName As String) As Boolean
        Dim ret As Boolean = False
        Try
            Dim oSku As String = Session("CreateLoadSKU")
            Dim oConsignee As String = Session("CreateLoadConsignee")

            Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
            If Not objSkuClass Is Nothing Then
                If objSkuClass.CaptureAtReceivingLoadAttributesCount > 0 Then
                    'EXPIRYDATE
                    If Not objSkuClass.LoadAttributes.Item(attName) Is Nothing Then
                        ret = True
                    End If
                End If
            End If
        Catch ex As Made4Net.Shared.M4NException
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As ApplicationException
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  (ex.Message))
        End Try

        Return ret
    End Function

    Private Sub ClearAttributes()

        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")

        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If Not objSkuClass Is Nothing Then

            If objSkuClass.CaptureAtReceivingLoadAttributesCount > 0 Then

                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    Try
                        DO1.Value(oAtt.Name) = ""
                    Catch ex As Exception

                    End Try
                Next
            End If
        End If

    End Sub

    Private Sub setAttributesFromObject()
        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")

        If IsNothing(Session("CreateLoadAttributes")) Then Exit Sub

        Dim oAttColl As WMS.Logic.AttributesCollection = Session("CreateLoadAttributes")
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If Not objSkuClass Is Nothing Then

            If objSkuClass.CaptureAtReceivingLoadAttributesCount > 0 Then

                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    Try
                        Select Case oAtt.Type
                            Case Logic.AttributeType.DateTime
                                DO1.Value(oAtt.Name) = Session(oAtt.Name & "SOURCE") 'oAttColl(oAtt.Name & "SOURCE").ToString()
                            Case Else
                                DO1.Value(oAtt.Name) = oAttColl(oAtt.Name).ToString()
                        End Select


                    Catch ex As Exception

                    End Try
                Next
            End If
        End If
    End Sub

    'CLD4
    Private Sub setAttributes()

        'Added for RWMS-1834 Start
        Dim formatDateTime As String = Session("RDTDateFormat")
        'Added for RWMS-1834 End

        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")
        Dim oRecLine As WMS.Logic.ReceiptDetail
        Dim invAttrColl As InventoryAttributeBase
        If Session("CreateLoadRCNLine") > -1 Then
            oRecLine = New WMS.Logic.ReceiptDetail(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
            invAttrColl = oRecLine.Attributes
        End If
        If Not Session("CreateLoadASNAttributes") Is Nothing Then
            invAttrColl = Session("CreateLoadASNAttributes")
        End If
        Dim objSKU As WMS.Logic.SKU = New WMS.Logic.SKU(oConsignee, oSku)
        Dim objSkuClass As WMS.Logic.SkuClass = objSKU.SKUClass

        If Not objSkuClass Is Nothing Then
            If objSkuClass.CaptureAtReceivingLoadAttributesCount > 0 Then
                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    Dim skipWeightCapture As Boolean = False

                    If oAtt.Name = "WEIGHT" Then
                        If objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUNET" Or objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                            skipWeightCapture = True
                        End If
                    End If
                    Dim req As Boolean = False
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                        req = True
                    End If
                    If (oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Capture) And Not skipWeightCapture Then
                        If req Then
                            'Added for RWMS-1834 Start
                            If oAtt.Name = "MFGDATE" Or oAtt.Name = "EXPIRYDATE" Then
                                DO1.AddTextboxLine(oAtt.Name, True, "Create", oAtt.Name + " " + "(" + formatDateTime + ")")
                            Else
                                DO1.AddTextboxLine(oAtt.Name, True, "Create", oAtt.Name)
                            End If
                            'Added for RWMS-1834 End

                            'Commented for RWMS-1834 Start
                            'DO1.AddTextboxLine(oAtt.Name, True, "Create", oAtt.Name)
                            'Commented for RWMS-1834 End
                        Else
                            DO1.AddTextboxLine(oAtt.Name, oAtt.Name)
                        End If
                        Try
                            If Not invAttrColl Is Nothing Then
                                If oAtt.Type = AttributeType.DateTime Then
                                    If CType(invAttrColl.Attribute(oAtt.Name), DateTime).Year <> 1 Then
                                        DO1.Value(oAtt.Name) = CType(invAttrColl.Attribute(oAtt.Name), DateTime).ToString(FormatDateTime)
                                    End If
                                Else
                                    DO1.Value(oAtt.Name) = invAttrColl.Attribute(oAtt.Name)
                                End If
                            End If
                        Catch ex As Exception
                        End Try
                    End If
                Next
            Else
                'SubmitCreate(Nothing, Session("CreateLoadDoPutAway"))
            End If
        Else
            'SubmitCreate(Nothing, Session("CreateLoadDoPutAway"))
        End If
    End Sub

    Private Function getRemainingUnits() As Decimal
        If Not Session("CreateLoadASNRemainingUnits") Is Nothing Then
            Return Session("CreateLoadASNRemainingUnits")
        End If
        If Not Session("CreateLoadOverrideUnits") Is Nothing Then
            Dim d As Decimal = Session("CreateLoadOverrideUnits")
            Session.Remove("CreateLoadOverrideUnits")

            Return Math.Truncate(d)
        End If

        Dim sql As String = String.Format("Select QTYEXPECTED-QTYRECEIVED from receiptdetail where receipt='{0}' and receiptline='{1}'", Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
        Dim units As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Return Math.Max(units, 0)
    End Function

    Private Function checkReceipQtyBySku(ByVal receipt As String, ByVal sku As String, ByRef errMsg As String) As Boolean
        Dim sql As String
        Dim ret As Boolean = True
        Dim dt As New DataTable
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim numloads As Integer

        'sql = String.Format("select count(1) from RECEIPTDETAIL where (qtyexpected-qtyreceived)>0 and receipt='{0}' and SKU='{1}' and consignee='{2}'", receipt, sku, Session("CreateLoadConsignee"))
        'numloads = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

        'If numloads < Session("CreateLoadNumLoads") Then
        '    errMsg = (t.Translate("Number of loads exceed number of lines"))
        '    Return False
        'End If

        'sql = String.Format("select min(qtyexpected-qtyreceived) as QtyLeftToReceive,RECEIPTLINE from RECEIPTDETAIL where receipt='{0}' and SKU='{1}' group by RECEIPTLINE", receipt, sku)
        Dim oSku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        Dim pUnits As Decimal = oSku.ConvertToUnits(Session("CreateLoadUOM")) * Session("CreateLoadUnits") '* CDec(Session("CreateLoadNumLoads"))
        '        sql = String.Format("select COUNT(1) from RECEIPTDETAIL where ((qtyexpected-qtyreceived)*{3}) >= {2} and receipt='{0}' and SKU='{1}' and consignee='{4}' group by receipt,SKU,consignee", receipt, sku, pUnits, oSku.OVERRECEIVEPCT, Session("CreateLoadConsignee"))
        sql = String.Format("select COUNT(1) from RECEIPTDETAIL where (qtyexpected * {3} - qtyreceived) >= {2} and receipt='{0}' and SKU='{1}' and consignee='{4}' group by receipt,SKU,consignee", receipt, sku, pUnits, oSku.OVERRECEIVEPCT, Session("CreateLoadConsignee"))
        numloads = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If numloads <= 0 Then
            errMsg = (t.Translate("Receipt line open quantity is less then loads quantity"))
            Return False
        End If
        '
        'Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        'For Each dr As DataRow In dt.Rows
        '    ret = checkReceipLineQty(receipt, dr("RECEIPTLINE"), errMsg)
        'Next
        Return ret
    End Function

    Private Sub SubmitCreate(ByVal oAttributes As WMS.Logic.AttributesCollection, Optional ByVal DoPutaway As Boolean = False)
        Dim ResponseMessage As String = ""
        Dim attributesarray As New ArrayList
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try
            oAttributes = ExtractAttributeValues()
            Dim ld() As WMS.Logic.Load
            Dim oRec As New Logic.Receiving
            If String.IsNullOrEmpty(Session("CreateLoadNumLoads")) OrElse Session("CreateLoadNumLoads") = 0 Then Session("CreateLoadNumLoads") = 1
            Dim oReceiptLine As ReceiptDetail
            If Session("CreateLoadRCNMultipleLines") Then
                'Dim errmsg As String

                'If Not checkReceipQtyBySku(Session("CreateLoadRCN"), Session("CreateLoadSKU"), errmsg) Then
                '     HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errmsg)
                '    Return
                'End If

                'CreateLoadFromMultipleLines(Session("CreateLoadRCN"), Session("CreateLoadSKU"), oAttributes, DoPutaway, errmsg)
                'Exit Sub
                ld = oRec.CreateLoadFromMultipleLines(Session("CreateLoadRCN"), Session("CreateLoadSKU"), _
                    Session("CreateLoadUOM"), Session("CreateLoadLocation"), Session("CreateLoadWarehousearea"), Session("CreateLoadUnits"), Session("CreateLoadStatus"), _
                    Session("CreateLoadHoldRC"), Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, Session("CreateLoadLabelPrinter"), "", "")
                Try
                    RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
                Catch ex As Exception
                End Try
            Else
                Dim errmsg As String
                If Not checkReceipQtyBySku(Session("CreateLoadRCN"), Session("CreateLoadSKU"), errmsg) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errmsg)
                    Return
                End If

                oReceiptLine = New ReceiptDetail(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
                Dim count As Decimal = CDec(Session("CreateLoadNumLoads"))
                For j As Integer = 0 To count - 1

                    ' Check if number of loads is more than one and if we need to create containers
                    ld = oRec.CreateLoad(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"), Session("CreateLoadSKU"), Session("CreateLoadLoadId"), _
                        Session("CreateLoadUOM"), Session("CreateLoadLocation"), Session("CreateLoadWarehousearea"), Session("CreateLoadUnits"), Session("CreateLoadStatus"), _
                        Session("CreateLoadHoldRC"), 1, Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, Session("CreateLoadLabelPrinter"), oReceiptLine.DOCUMENTTYPE, "", "")
                    Try
                        RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
                    Catch ex As Exception
                    End Try
                    'Code to calculate the runnig average weight and update back the same
                    'Dim sSqlFlag As String = "SELECT PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME = 'CalcRuningAvgWt'"
                    'Dim paramValue As String
                    'Dim skuClass As String
                    'Dim avgWeight As Decimal = 0
                    'Dim totalInventory As Decimal = 0
                    'Dim AvgRunningVal As Decimal = 0
                    'Dim currentInventory As Decimal = 0



                    'paramValue = Made4Net.DataAccess.DataInterface.ExecuteScalar(sSqlFlag)

                    'If (paramValue = "1") Then
                    '    Dim strSKUClass As String = "SELECT sku.CLASSNAME,skuattr.WGT  FROM SKU sku inner join SKUATTRIBUTE skuattr on sku.SKU = skuattr.SKU where sku.SKU = '" & ld(0).SKU & "'"
                    '    Dim dt As New DataTable
                    '    Made4Net.DataAccess.DataInterface.FillDataset(strSKUClass, dt)
                    '    If dt.Rows.Count > 0 Then
                    '        If Not IsDBNull(dt.Rows(0)("CLASSNAME")) And dt.Rows(0)("CLASSNAME") = "WGT" Then
                    '            If Not IsDBNull(dt.Rows(0)("WGT")) Then
                    '                avgWeight = dt.Rows(0)("WGT")
                    '            End If
                    '            Dim strUnits As String = "SELECT UNITS FROM LOADS WHERE LOCATION = '" & Session("CreateLoadLocation") & "' AND RECEIPT = '" & ld(0).RECEIPT & "'"
                    '            totalInventory = Made4Net.DataAccess.DataInterface.ExecuteScalar(strUnits)
                    '            currentInventory = totalInventory - ld(0).UNITS
                    '            AvgRunningVal = ((currentInventory * avgWeight) + Decimal.Parse(oAttributes.Item("WEIGHT"))) / totalInventory
                    '            'Dim sqlUpdate As String = String.Format("Update SKUATTRIBUTE set WGT = " & Decimal.Parse(AvgRunningVal) & " where SKU='" & ld(0).SKU & "'")
                    '            'DataInterface.RunSQL(sqlUpdate)
                    '        End If

                    '    End If
                    'End If


                    'End of running average weight
                    ' After Load Creation we will put the loads on his container
                    If Session("CreateContainer") = "1" Then
                        Dim LoadsCreatedCount As Int32
                        For LoadsCreatedCount = 0 To ld.Length() - 1
                            Dim cntr As New WMS.Logic.Container()
                            cntr.ContainerId = Made4Net.Shared.getNextCounter("CONTAINER")
                            cntr.HandlingUnitType = Session("HUTYPE")
                            cntr.Location = Session("CreateLoadLocation")
                            cntr.Warehousearea = Session("CreateLoadWarehousearea")
                            cntr.Post(WMS.Logic.Common.GetCurrentUser)

                            cntr.Place(ld(LoadsCreatedCount), WMS.Logic.Common.GetCurrentUser)
                        Next
                    Else
                        If Session("CreateLoadContainerID") <> "" Then
                            Dim cntr As New WMS.Logic.Container(Session("CreateLoadContainerID"), True)
                            Dim LoadsCreatedCount As Int32
                            For LoadsCreatedCount = 0 To ld.Length() - 1
                                cntr.Place(ld(LoadsCreatedCount), WMS.Logic.Common.GetCurrentUser)
                            Next
                        End If
                    End If

                    'CreateNonSpecificPickUpTasks(Session("CreateLoadLocation"), ld)

                    If Not String.IsNullOrEmpty(Session("ReceiveByIdASNID")) Then
                        Session("ASNQTYRECEIVED") = 1
                        Session("CreateLoadASNRemainingUnits") = Session("CreateLoadASNRemainingUnits") - ld(0).UNITS
                    End If

                    ' Create Handling unit transaction if needed
                    If DO1.Value("HUTRANS") = "YES" Then
                        Try
                            Dim strHUTransIns As String
                            If Session("CreateLoadContainerID") <> "" Then
                                strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                                              "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','RECEIPT','" & Session("CreateLoadRCN") & "',GETDATE(),'" & Session("CreateLoadConsignee") & "','" & oReceiptLine.ORDERID & "','" & oReceiptLine.DOCUMENTTYPE & "','" & oReceiptLine.COMPANY & "','" & oReceiptLine.COMPANYTYPE & "','" & DO1.Value("HUTYPE") & "','1',GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                            Else
                                strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                                              "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','RECEIPT','" & Session("CreateLoadRCN") & "',GETDATE(),'" & Session("CreateLoadConsignee") & "','" & oReceiptLine.ORDERID & "','" & oReceiptLine.DOCUMENTTYPE & "','" & oReceiptLine.COMPANY & "','" & oReceiptLine.COMPANYTYPE & "','" & DO1.Value("HUTYPE") & "','" & Session("CreateLoadNumLoads") & "',GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                            End If
                            DataInterface.RunSQL(strHUTransIns)
                        Catch ex As Exception
                        End Try
                    End If

                    ' If Create and Putaway then request pickup for each load
                    If DoPutaway Then
                        Dim pw As New Putaway
                        Dim i As Int32
                        Session.Remove("CREATELOADPCKUP")
                        For i = 0 To ld.Length() - 1
                            ld(i).RequestPickUp(Logic.GetCurrentUser, "") 'RWMS-1277
                            Session("CREATELOADPCKUP") = ld(i).LOADID
                        Next
                        If Not String.IsNullOrEmpty(Session("CREATELOADPCKUP")) Then
                            'Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=cld1"))
                            doCloseASN(True)
                            Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=cld1"))
                        End If
                    End If
                Next

            End If

            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Load Created"))

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
            Return
        End Try
        DO1.Value("LOADID") = ""
        DO1.Value("UNITS") = ""
        DO1.Value("CONTAINERID") = ""

    End Sub

    Private Sub CreateNonSpecificPickUpTasks(ByVal pLocation As String, ByVal ld() As WMS.Logic.Load)
        For i As Int32 = 0 To ld.Length - 1
            Dim sSql As String = String.Format("select LOADID,PRIORITY from vNSPickupLoads where LOADID = '{0}'", ld(i).LOADID)
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sSql, dt)

            If dt.Rows.Count > 0 Then
                Dim ts As New WMS.Logic.Task
                ts.FROMLOCATION = pLocation
                ts.PRIORITY = dt.Rows(0)("PRIORITY")
                ts.TASKTYPE = WMS.Lib.TASKTYPE.NSPICKUP
                ts.DOCUMENT = Session("CreateLoadRCN") ' DO1.Value("RECEIPT")
                ts.DOCUMENTLINE = Session("CreateLoadRCNLine") ' DO1.Value("RECEIPTLINE")
                ts.FROMWAREHOUSEAREA = ld(i).WAREHOUSEAREA
                ts.TOWAREHOUSEAREA = ld(i).WAREHOUSEAREA
                ts.ADDUSER = WMS.Logic.GetCurrentUser
                ts.EDITUSER = WMS.Logic.GetCurrentUser
                ts.EDITDATE = DateTime.Now
                ts.ADDDATE = DateTime.Now
                ts.Post()
            End If
        Next
    End Sub

    Function CreateLoads(ByVal dr As DataRow, ByVal pUser As String) As Load()

        Dim rcode As String, ssku As String, rline As Int32, holdrc As String, stat As String, qty As Double, loc As String, warehousearea As String, ldid As String, loaduom As String, numloads As Int32, printer As String, pDocumentType As String
        rcode = Conversion.Convert.ReplaceDBNull(dr("RECEIPT"))
        rline = Conversion.Convert.ReplaceDBNull(dr("RECEIPTLINE"))
        holdrc = Conversion.Convert.ReplaceDBNull(dr("HOLDRC"))
        loc = Conversion.Convert.ReplaceDBNull(dr("LOCATION"))
        warehousearea = Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))
        numloads = Conversion.Convert.ReplaceDBNull(dr("NUMLOADS"))
        printer = Conversion.Convert.ReplaceDBNull(dr("PRINTER"))
        If numloads = 1 Then
            ldid = Conversion.Convert.ReplaceDBNull(dr("LOADID"))
        End If
        stat = Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        qty = Conversion.Convert.ReplaceDBNull(dr("UNITS"))
        loaduom = Conversion.Convert.ReplaceDBNull(dr("LOADUOM"))
        Dim oAttCol As AttributesCollection = SkuClass.ExtractReceivingAttributes(dr)
        Try
            Dim rh As ReceiptHeader = ReceiptHeader.GetReceipt(rcode)
            Dim rl As ReceiptDetail = rh.LINES.Line(rline)
            ssku = rl.SKU
            pDocumentType = rl.DOCUMENTTYPE
        Catch ex As Exception
            pDocumentType = ""
        End Try
        Dim rec As New WMS.Logic.Receiving()

        Dim ld() As WMS.Logic.Load = rec.CreateLoad(rcode, rline, ssku, ldid, loaduom, loc, warehousearea, qty, stat, holdrc, numloads, pUser, WMS.Logic.LogHandler.GetRDTLogger, oAttCol, printer, pDocumentType, Nothing, Nothing)
        Try
            RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
        Catch ex As Exception
        End Try

        Return ld
    End Function

    Private Function getDAYSTORECEIVE() As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(DAYREC, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    'Commented for RWMS-454, RWMS-330
    'Private Function getDangerous() As String
    '    Dim ret As String = ""
    '    Dim sql As String = String.Format("SELECT ISNULL(DANG, '') FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
    '    Try
    '        ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    '    Catch ex As Exception
    '    End Try
    '    Return ret
    'End Function
    'End Commented for RWMS-454, RWMS-330

    Private Function getRECOVERRIDEVALIDATOR() As String
        Dim ret As String = String.Empty

        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")


        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass

        If Not objSkuClass Is Nothing Then
            Dim sql As String = String.Format("SELECT ISNULL(RECOVERRIDEVALIDATOR, '') FROM SKUCLSLOADATT WHERE CLASSNAME = '{0}' AND ATTRIBUTENAME = 'EXPIRYDATE'", objSkuClass.ClassName)
            Try
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
        End If
        Return ret
    End Function

    Private Function ExtractAttributeValues() As WMS.Logic.AttributesCollection

        'Added for RWMS-1834 Start
        Dim formatDateTime As String = Session("RDTDateFormat")
        'Added for RWMS-2726
        formatDateTime = formatDateTime.Replace("m", "M").Replace("D", "d").Replace("Y", "y")
        'Added for RWMS-1834 End
        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")
        Dim objSKU As SKU = New WMS.Logic.SKU(oConsignee, oSku)
        Dim objSkuClass As WMS.Logic.SkuClass = objSKU.SKUClass
        Dim isWeightCaptured As Boolean = False
        Dim oAttCol As New WMS.Logic.AttributesCollection
        If objSkuClass Is Nothing Then
            ' RWMS-2172
            ' Even if the SKU does not have a SKU Class that expects to capture attributes
            If objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                isWeightCaptured = True
                Dim netWeight As Decimal = GetNetWeight(objSKU.SKU)
                Dim skuNetWght As Decimal = netWeight * Session("CreateLoadUnits")
                oAttCol.Add("WEIGHT", skuNetWght)
                Return oAttCol
            ElseIf objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                isWeightCaptured = True
                Dim grossWeight As Decimal = GetGrossWeight(objSKU.SKU)
                Dim skuGrossWght As Decimal = grossWeight * Session("CreateLoadUnits")
                oAttCol.Add("WEIGHT", skuGrossWght)
            Else
                Return Nothing
            End If
            ' RWMS-2172
        End If

        'Added for RWMS-2420 Start
        If Not objSkuClass Is Nothing Then
            'Added for RWMS-2420 End

            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                ' RWMS-2172
                Dim isUseSKUNetWght As Boolean = False
                Dim isUseSKUGrossWght As Boolean = False
                If oAtt.Name = "WEIGHT" Then
                    If objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                        isUseSKUNetWght = True
                    End If
                    If objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                        isUseSKUGrossWght = True
                    End If
                End If
                ' RWMS-2172
                Dim req As Boolean = False
                If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                    Dim val As Object
                    Try
                        Select Case oAtt.Type
                            Case Logic.AttributeType.Boolean
                                val = CType(DO1.Value(oAtt.Name), Boolean)
                            Case Logic.AttributeType.DateTime
                                Dim d As Date
                                If DateTime.TryParseExact(DO1.Value(oAtt.Name), formatDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, d) Then
                                    'val = d.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat"))


                                    'Commented for RWMS-1834 Start
                                    'val = d.ToString("yyyy-MM-dd")
                                    'Commented for RWMS-1834 End

                                    'RWMS-2726 Commented
                                    ''Added for RWMS-1834 Start
                                    'val = d.ToString(formatDateTime)
                                    ''Added for RWMS-1834 End
                                    'RWMS-2726 Commented END

                                    'RWMS-2726
                                    'Convert the date to a MMM type format string so that it gets updated in the database correctly
                                    val = d.ToString("dd-MMM-yyyy")
                                    'RWMS-2726 END

                                    'oAttCol.Add(oAtt.Name & "SOURCE", DO1.Value(oAtt.Name))
                                    Session(oAtt.Name & "SOURCE") = DO1.Value(oAtt.Name)
                                End If

                                'val = CType(DO1.Value(oAtt.Name), DateTime)
                                'val = DateTime.ParseExact(DO1.Value(oAtt.Name), Made4Net.Shared.AppConfig.DateFormat, Nothing)
                                ' val = CType(DO1.Value(oAtt.Name), DateTime).ToString(Made4Net.Shared.AppConfig.DateFormat)
                            Case Logic.AttributeType.Decimal
                                'Commetnted for RWMS-151
                                'val = CType(DO1.Value(oAtt.Name), Decimal)
                                'End Commented for RWMS-151
                                'Added for RWMS-151
                                If oAtt.Name.ToUpper = "WEIGHT" Then
                                    ' RWMS-2172
                                    If isUseSKUNetWght Then
                                        isWeightCaptured = True
                                        ' Use the Net Weight (or Gross Weight) multiplied by the quantity
                                        Dim netWeight As Decimal = GetNetWeight(objSKU.SKU)
                                        val = netWeight * Session("CreateLoadUnits")
                                        ' RWMS-2172
                                    ElseIf isUseSKUGrossWght Then
                                        isWeightCaptured = True
                                        ' Use the Net Weight (or Gross Weight) multiplied by the quantity
                                        Dim grossWeight As Decimal = GetGrossWeight(objSKU.SKU)
                                        val = grossWeight * Session("CreateLoadUnits")
                                        ' RWMS-2172
                                    Else
                                        isWeightCaptured = True
                                        val = DO1.Value(oAtt.Name)

                                        'Added for RWMS-441 : 10/21
                                        Dim err, barcode, ret As String
                                        barcode = val.ToString() 'DO1.Value("WEIGHT")
                                        Dim gs As Barcode128GS.GS128 = New Barcode128GS.GS128()
                                        ret = gs.getWeight(barcode, err)
                                        If ret = 1 Then
                                            val = barcode
                                            DO1.Value("WEIGHT") = val
                                        Else
                                            val = ""
                                            DO1.Value("WEIGHT") = val
                                            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret & " " & err)
                                            'Exit
                                        End If
                                        'End Added for RWMS-441 : 10/21
                                    End If

                                Else
                                    val = CType(DO1.Value(oAtt.Name), Decimal)
                                End If
                                'End Added for RWMS-151
                            Case Logic.AttributeType.Integer
                                val = CType(DO1.Value(oAtt.Name), Int32)
                            Case Else
                                val = DO1.Value(oAtt.Name)
                        End Select
                        oAttCol.Add(oAtt.Name, val)
                    Catch ex As Exception
                        If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Attribute Validation failed for " & oAtt.Name, "Attribute Validation failed for " & oAtt.Name)
                        End If
                    End Try
                End If
            Next
            ' RWMS-2172
            'Added for RWMS-2420 Start
        End If
        'Added for RWMS-2420 End
        ' Even if the SKU does not have a SKU Class that expects to capture attributes
        If Not isWeightCaptured Then
            If objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                isWeightCaptured = True
                Dim netWeight As Decimal = GetNetWeight(objSKU.SKU)
                Dim skuNetWght As Decimal = netWeight * Session("CreateLoadUnits")
                oAttCol.Add("WEIGHT", skuNetWght)
                Return oAttCol
            ElseIf objSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                isWeightCaptured = True
                Dim grossWeight As Decimal = GetGrossWeight(objSKU.SKU)
                Dim skuGrossWght As Decimal = grossWeight * Session("CreateLoadUnits")
                oAttCol.Add("WEIGHT", skuGrossWght)
            End If
        End If
        ' RWMS-2172
        Return oAttCol
    End Function

    Private Function GetNetWeight(ByVal sku As String) As Decimal
        Dim sqluom As String = String.Format("select NETWEIGHT from SKUUOM where isnull(loweruom,'')=''  and SKU = '{0}'", sku)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
    End Function

    Private Function GetGrossWeight(ByVal sku As String) As Decimal
        Dim sqluom As String = String.Format("select GROSSWEIGHT from SKUUOM where isnull(loweruom,'')=''  and SKU = '{0}'", sku)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
    End Function

    Private Sub doCloseReceipt()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oReceipt As New WMS.Logic.ReceiptHeader(Session("CreateLoadRCN"))
            oReceipt.close(WMS.Logic.Common.GetCurrentUser)
            RWMS.Logic.AppUtil.UpdateReceiptAverageWeight(Session("CreateLoadRCN"))
            Dim message As String
            If Not RWMS.Logic.AppUtil.CloseInboundByReceipt(Session("CreateLoadRCN"), message) Then
                ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, message)
                'Exit Sub
            End If
            DO1.Value("RCN") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Receipt Closed"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return
        End Try
        Session.Remove("CreateLoadRCN")
        MobileUtils.ClearCreateLoadProcessSession()

        doCloseASN(False)

        Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=cld1"))
    End Sub

    Private Sub doCloseASN(ByVal pCloseForRPK As Boolean)
        If String.IsNullOrEmpty(Session("ReceiveByIdASNID")) Then
            Return
        End If
        Session.Remove("CreateLoadRCN")
        Session.Remove("CreateLoadContainerID")
        Session.Remove("CreateLoadASNLoadID")
        Session.Remove("CreateLoadASNRemainingUnits")
        Session.Remove("CreateLoadASNAttributes")
        If Not pCloseForRPK AndAlso String.IsNullOrEmpty(Session("ASNQTYRECEIVED")) Then
            Response.Redirect(MapVirtualPath("Screens/ReceiveByID1.aspx"))
        End If
        Session.Remove("ASNQTYRECEIVED")
        Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
        oASN.SetStatus(WMS.Lib.Statuses.ASN.RECEIVED, WMS.Logic.GetCurrentUser)
        Session.Remove("ReceiveByIdASNID")
        If Not pCloseForRPK Then
            Response.Redirect(MapVirtualPath("Screens/ReceiveByID.aspx"))
        End If
    End Sub

End Class