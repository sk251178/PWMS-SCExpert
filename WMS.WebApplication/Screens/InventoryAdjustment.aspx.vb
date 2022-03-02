Imports RWMS.Logic
Imports System.Xml
Imports Made4Net.Shared.Web
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports System.Globalization

Public Class InventoryAdjustment
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents ddInvAct As Made4Net.WebControls.DropDownList
    Protected WithEvents pnlAdj As System.Web.UI.WebControls.Panel
    Protected WithEvents lblAdjType As Made4Net.WebControls.FieldLabel
    Protected WithEvents lblAdjReasonCode As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddReasonCode As Made4Net.WebControls.DropDownList
    Protected WithEvents pnlAddSubQty As System.Web.UI.WebControls.Panel
    Protected WithEvents TEINVLOADADJ As Made4Net.WebControls.TableEditor
    Protected WithEvents pnlCancelLoad As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlSplitLoad As System.Web.UI.WebControls.Panel
    Protected WithEvents TESPLITLOAD As Made4Net.WebControls.TableEditor
    Protected WithEvents TECancelLoad As Made4Net.WebControls.TableEditor
    Protected WithEvents TECHANGEUOM As Made4Net.WebControls.TableEditor
    Protected WithEvents pnlChangeUOM As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlChangeSku As System.Web.UI.WebControls.Panel
    Protected WithEvents TECHANHESKU As Made4Net.WebControls.TableEditor
    Protected WithEvents pnlCreateLoad As System.Web.UI.WebControls.Panel
    Protected WithEvents TECREATELOAD As Made4Net.WebControls.TableEditor
    Protected WithEvents TECREATELOADNEW As Made4Net.WebControls.TableEditor
    Protected WithEvents pnlCreateLoadNew As System.Web.UI.WebControls.Panel
    Protected WithEvents pnlSubQtyVendorReturns As System.Web.UI.WebControls.Panel
    Protected WithEvents TEINVLOADADJSUBQTYVENDORRETURNS As Made4Net.WebControls.TableEditor
    Protected WithEvents pnlSubQtyCustomereturns As System.Web.UI.WebControls.Panel
    Protected WithEvents TEINVLOADADJSUBQTYCUSTOMERRETURNS As Made4Net.WebControls.TableEditor

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
    Public formatDateTime As String = "MMddyy"

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        Dim _loadid As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        Select Case CommandName.ToLower
            Case "submitsplit"
                For Each dr In ds.Tables(0).Rows
                    Dim err As String
                    _loadid = dr("LOADID")

                    If Logic.Load.LBNVADJParamIsSet AndAlso String.IsNullOrEmpty(dr("HostReferenceID")) And (dr("REASONCODE") = "CR" Or dr("REASONCODE") = "VR") Then
                        Throw New M4NException(New Exception, "Please enter HostReferenceID.", "Please enter HostReferenceID.")
                    End If

                    Dim ld As New WMS.Logic.Load(_loadid)
                    If dr("TOWAREHOUSEAREA") = "" Then
                        dr("TOWAREHOUSEAREA") = dr("WAREHOUSEAREA")
                    End If
                    If Not AppUtil.ChangeLocationValidation(dr("TOLOCATION"), dr("TOWAREHOUSEAREA"), ld.CONSIGNEE, ld.SKU, err) Then
                        If err <> "" Then Message += err
                        Throw New ApplicationException(Message)
                    End If

                    Dim pNewLoadId As String = dr("TOLOADID")
                    If pNewLoadId.Trim = "" Then
                        pNewLoadId = WMS.Logic.Load.GenerateLoadId
                    End If
                    'RWMS-1380 Start
                    If Not IsNumeric(dr("TOQTY")) Then
                        Throw New M4NException(New Exception, "Cannot Split Load.Please enter valid quantity", "Cannot Split Load.Please enter valid quantity")
                    End If
                    'RWMS-1380 End

                    ld.Split(dr("TOLOCATION"), dr("TOWAREHOUSEAREA"), dr("TOQTY"), pNewLoadId, UserId)
                    AppUtil.isBackLocMoveFront(pNewLoadId, dr("TOLOCATION"), dr("TOWAREHOUSEAREA"), "", err)
                    If err <> "" Then Message += err
                    'Added for RWMS-367
                    WMS.Logic.Load.PrintLabel(ld.LOADID, dr("PRINTER"))
                    'End of RWMS-367
                Next

            Case "invadjcreateload"

                For Each dr In ds.Tables(0).Rows

                    'start validate load weight if needed
                    Dim wgtVal As New RWMS.Logic.WeightValidator
                    Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))

                    If Logic.Load.LBNVADJParamIsSet And (Made4Net.Shared.ContextSwitch.Current.Session("REASONCODE") = "CR" Or Made4Net.Shared.ContextSwitch.Current.Session("REASONCODE") Is Nothing) Then
                        ValidateCreatePayLoadCustomerReturns(dr)
                    End If

                    If wgtVal.WeightNeeded(oSku) Then
                        If IsDBNull(dr("WEIGHT")) Then
                            Throw New ApplicationException("Please fill weight")
                        End If
                        'If dr("WEIGHT") <> "" Then
                        Dim gotoOverride As Boolean = False
                        Dim gotoOverrideMessage As String = ""
                        Dim errMsg As String = ""
                        Dim WEIGHT As String = dr("WEIGHT")
                        Dim UNITS As Decimal = Convert.ToDecimal(dr("UNITS"))
                        UNITS = oSku.ConvertToUnits(dr("LOADUOM")) * UNITS

                        If Not wgtVal.ValidateWeightSku(oSku, WEIGHT, UNITS, gotoOverride, gotoOverrideMessage, errMsg, False) Then
                            If gotoOverride Then
                                dr("WEIGHT") = WEIGHT
                                Throw New ApplicationException(errMsg & gotoOverrideMessage)
                            Else
                                Throw New ApplicationException(errMsg)
                            End If

                            Exit Sub
                        Else
                            dr("WEIGHT") = WEIGHT
                        End If
                        ' End If
                    End If


                    Dim dEXPIRYDATE As Date
                    formatDateTime = "MM/dd/yyyy"
                    '2013-04-08
                    'Expiry date validation –
                    'The system will compare the payload’s expiry date with the minimum ship days parameter in SKUATTRIBUTES table.
                    'If the expiry date entered is sooner than <today + minimum ship days >, the validation will fail, and an error message will return
                    If AttributeNeeded(oSku, dr, "EXPIRYDATE") Then
                        If IsDBNull(dr("EXPIRYDATE")) OrElse Not IsDate(dr("EXPIRYDATE")) Then 'Not DateTime.TryParseExact(dr("EXPIRYDATE"), formatDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, dEXPIRYDATE) Then
                            Throw New ApplicationException(t.Translate("Illegal EXPIRYDATE format"))
                            ' Message = t.Translate("Illegal EXPIRYDATE format")
                            Return
                        Else
                            dEXPIRYDATE = dr("EXPIRYDATE")
                            Dim iShipDay As Int16 = getShipDay(dr("CONSIGNEE"), dr("SKU"))
                            'If DateTime.Compare(dEXPIRYDATE, DateTime.Now.AddDays(iShipDay)) < 0 Then
                            'RWMS-608
                            If DateDiff(DateInterval.Day, dEXPIRYDATE, Convert.ToDateTime(DateTime.Now.AddDays(iShipDay).ToShortDateString())) > 0 Then

                                Dim msg As String = "Payload expiry date {0} is sooner than allowed to receive for this product ({1})"
                                msg = String.Format(msg, dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")), DateTime.Now.AddDays(iShipDay).ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")))
                                Throw New ApplicationException(t.Translate(msg))
                                Return
                            End If
                        End If
                    End If

                    'Begin RWMS-608 - adding the validation for MFG DATE
                    Dim dMFGDATE As Date
                    formatDateTime = "MM/dd/yyyy"
                    '2013-04-08
                    'Manufacture date validation
                    If AttributeNeeded(oSku, dr, "MFGDATE") Then
                        If IsDBNull(dr("MFGDATE")) OrElse Not IsDate(dr("MFGDATE")) Then
                            'If Not DateTime.TryParseExact(dr("MFGDATE").ToString(), formatDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None, dMFGDATE) Then
                            Throw New ApplicationException(t.Translate("Illegal MFGDATE format"))
                            Return
                        Else
                            'Begin RWMS-475 Added the new rules Date: 01/09/2015
                            'Modified the below for RWMS-608
                            '1. Mfg date <= today
                            '2.if skuattribute.Mindaystoship > 0 and (mfgdate + skuattribute.shelflife) - today >= skuattribute.mindaystoship
                            '3.if skuattribute.Mindaystoship > 0 or null then do not validate the mfg date other than rule 1
                            '4.Once mfgdate is validated then store it in attribute.mfgdate (this is happening today)
                            dMFGDATE = dr("MFGDATE")
                            If DateTime.Compare(dMFGDATE, DateTime.Now) > 0 Then
                                Throw New ApplicationException(t.Translate("MFGDATE Cannot be a future date"))
                                Return
                            End If
                            Dim iShipDay As Int16 = getShipDay(dr("CONSIGNEE"), dr("SKU"))
                            Dim ishelflife As Int16 = getShelfLife(dr("CONSIGNEE"), dr("SKU"))
                            Dim isDaytoReceive As Int16 = getDayToReceive(dr("CONSIGNEE"), dr("SKU"))
                            If iShipDay > 0 Then
                                If dMFGDATE.AddDays(ishelflife).Subtract(DateTime.Now.Date).TotalDays < iShipDay Then
                                    Dim msg As String = "Payload manufacture date {0} is older than allowed to receive for this product ({1}. Valid Rule: (mfgdate + shelflife) - today >= mindaystoship)"
                                    msg = String.Format(msg, dMFGDATE.ToString("MM/dd/yyyy"), DateTime.Now.Date.AddDays(iShipDay).AddDays(-ishelflife).ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")))
                                    Throw New ApplicationException(t.Translate(msg))
                                    Return
                                End If
                            Else
                                Throw New ApplicationException(t.Translate("Mindaystoship must be greater than zero"))
                                Return
                            End If
                            'END RWMS-475
                        End If
                    End If
                    'END RWMS-608

                    Dim RECOVERRIDEVALIDATOR As String
                    RECOVERRIDEVALIDATOR = getRECOVERRIDEVALIDATOR(dr("CONSIGNEE"), dr("SKU"))
                    'If SkuMenage("EXPIRYDATE") Then

                    If EXPIRYDATENeeded(oSku, dr) And Not String.IsNullOrEmpty(RECOVERRIDEVALIDATOR) Then
                        '      testValidation()

                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

                        vals.Add("EXPIRYDATE", dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")))
                        vals.Add("DAYSTORECEIVE", getDAYSTORECEIVE(dr("CONSIGNEE"), dr("SKU")).ToString())

                        Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
                        exprEval.FieldValues = vals
                        Dim statement As String = "[0];func:" & RECOVERRIDEVALIDATOR & "(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
                        'Dim statement As String = "[0];func:ValidateExpiryDate(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
                        '
                        Dim ret As String
                        Try
                            ret = exprEval.Evaluate(statement)
                        Catch ex As Exception
                            Throw New ApplicationException(t.Translate("Illegal validation function") & statement & ex.Message)
                            Return
                        End Try

                        Dim returnedResponse() As String = ret.Split(";")
                        If returnedResponse(0) = "0" Then

                            'Session("CreateLoadAttributes") = oAttributes
                            'gotoOverride(returnedResponse(1))
                            'Exit Sub
                            Throw New ApplicationException(t.Translate(returnedResponse(1)))
                            Return
                        ElseIf returnedResponse(0) = "-1" Then
                            Throw New ApplicationException(ret)
                            Return
                        End If
                        ' End If
                    End If

                    'END validate load weight
                    If IsDBNull(dr("LOADID")) Then
                        Dim oXMLDoc As New XmlDocument
                        oXMLDoc.LoadXml(XMLData)
                        Dim oXmlNode As XmlNode = oXMLDoc.CreateNode(XmlNodeType.Element, "loadid", "")
                        'oXmlNode.InnerText = Made4Net.Shared.Util.getNextCounter("LOAD")
                        oXMLDoc.SelectSingleNode("NewDataSet/Table1").InsertBefore(oXmlNode, oXMLDoc.SelectSingleNode("NewDataSet/Table1").FirstChild)
                        XMLData = oXMLDoc.InnerXml
                        'Added below line for RWMS-2045 and 1651
                        Dim ld2 As New RWMS.Logic.Load(Sender, CommandName, XMLSchema, XMLData, Message)
                        RWMS.Logic.AppUtil.SetReceivedWeight(ld2.CONSIGNEE, ld2.SKU, ld2.RECEIPT, ld2.RECEIPTLINE)
                        Message = "load created " & ld2.LOADID
                        'Ended below line for RWMS-2045 and 1651
                        Exit Sub
                        ''XMLSchema = Session("XMLSchema")
                        ''Throw New ApplicationException("loadid is db null")
                        '' Return
                    ElseIf dr("LOADID") = "" Then
                        ''Throw New ApplicationException(t.Translate("Load ID can not be blank"))
                        'Dim NEWLOAD As String = Made4Net.Shared.Util.getNextCounter("LOAD")
                        'Dim xDoc As New XmlDocument
                        'xDoc.LoadXml(XMLData)
                        'xDoc.SelectSingleNode("NewDataSet/Table1/loadid").InnerText = NEWLOAD
                        ''If IsNothing(Session("XMLSchema")) Then Session("XMLSchema") = XMLSchema
                        ''Else
                        ''    If IsNothing(Session("XMLSchema")) Then Session("XMLSchema") = XMLSchema
                    End If
                    'Commented For RWMS -366 Start RWMS-774 and RWMS-591
                    'Dim ld As New WMS.Logic.Load(Sender, CommandName, XMLSchema, XMLData, Message)
                    'Commented For RWMS -366 End RWMS-774 and RWMS-591
                    'Added For RWMS -366 Start
                    Dim ld As New RWMS.Logic.Load(Sender, CommandName, XMLSchema, XMLData, Message)
                    'Added For RWMS -366 Start RWMS-774 and RWMS-591
                    ' Try
                    RWMS.Logic.AppUtil.SetReceivedWeight(ld.CONSIGNEE, ld.SKU, ld.RECEIPT, ld.RECEIPTLINE)
                    Message = "load created " & ld.LOADID
                    ' Catch ex As Exception
                    ' End Try

                    'Added for RWMS-367
                    'Added for RWMS-1546 and RWMS-1398
                    ' WMS.Logic.Load.PrintLabel(ld.LOADID, dr("PRINTER"))
                    'Ended for  RWMS-1546 and RWMS-1398
                    'End of RWMS-367

                Next


        End Select
    End Sub
    'Begin RWMS-608

    Private Function getShelfLife(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(SHELFLIFE, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    'RWMS-475 - Adding new method to get DAYTORECEIVE column value by passing SKU and Consignee
    Private Function getDayToReceive(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(DAYTORECEIVE, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function
    'End RWMS-475
    'End RWMS-608



    Private Function getDAYSTORECEIVE(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(DAYREC, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    Private Function getShipDay(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(MINDAYSTOSHIP, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    Private Function AttributeNeeded(ByVal pSKU As WMS.Logic.SKU, ByVal dr As DataRow, Optional ByVal attName As String = "EXPIRYDATE") As Boolean
        Dim attval As String = ""
        If Not IsNothing(pSKU.SKUClass) Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = attName Then

                    If Not IsDBNull(dr(attName)) Then attval = dr(attName)

                    If String.IsNullOrEmpty(attval) And oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
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

    Private Function EXPIRYDATENeeded(ByVal pSKU As WMS.Logic.SKU, ByVal DR As DataRow) As Boolean
        If Not IsNothing(pSKU.SKUClass) Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = "EXPIRYDATE" Then
                    If String.IsNullOrEmpty(DR(oAtt.Name.ToUpper)) And oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
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

    Private Function getRECOVERRIDEVALIDATOR(ByVal Consignee As String, ByVal SKU As String) As String
        Dim ret As String = String.Empty


        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(Consignee, SKU).SKUClass

        If Not objSkuClass Is Nothing Then
            Dim sql As String = String.Format("SELECT ISNULL(RECOVERRIDEVALIDATOR, '') FROM SKUCLSLOADATT WHERE CLASSNAME = '{0}' AND ATTRIBUTENAME = 'EXPIRYDATE'", objSkuClass.ClassName)
            Try
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
        End If
        Return ret
    End Function

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ddInvAct.DataBind()
            ddReasonCode.DataBind()
            ddInvAct.SelectedIndex = 1
            If Logic.Load.LBNVADJParamIsSet Then
                ddReasonCode.SelectedIndex = 1
            End If
            ddInvAct_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub ddInvAct_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddInvAct.SelectedIndexChanged
        ValidateInvAdj()
    End Sub
    'Begin RWMS-474 - Author:Ravi
    Private Sub ValidateInvAdj()
        If Logic.Load.LBNVADJParamIsSet Then
            pnlSubQtyCustomereturns.Visible = False
            TEINVLOADADJSUBQTYCUSTOMERRETURNS.Visible = False
            If (ddInvAct.SelectedValue = WMS.Lib.INVENTORY.ADDQTY And ddReasonCode.SelectedValue <> "CR") Or (ddInvAct.SelectedValue = WMS.Lib.INVENTORY.SUBQTY And ddReasonCode.SelectedValue <> "VR") Then
                pnlAddSubQty.Visible = True
                TEINVLOADADJ.Visible = True
                TEINVLOADADJ.Restart()
                Dim tds As DataTable = TEINVLOADADJ.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TEINVLOADADJ.PreDefinedValues = vals
            Else
                pnlAddSubQty.Visible = False
                pnlSubQtyVendorReturns.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.SPLITLOAD Then
                pnlSplitLoad.Visible = True
                TESPLITLOAD.Visible = True
                TESPLITLOAD.Restart()
                Dim tds As DataTable = TESPLITLOAD.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TESPLITLOAD.PreDefinedValues = vals
            Else
                pnlSplitLoad.Visible = False
                pnlSubQtyVendorReturns.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.CHANGEUOM Then
                pnlChangeUOM.Visible = True
                TECHANGEUOM.Visible = True
                TECHANGEUOM.Restart()
                Dim tds As DataTable = TECHANGEUOM.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TECHANGEUOM.PreDefinedValues = vals
            Else
                pnlChangeUOM.Visible = False
                pnlSubQtyVendorReturns.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.CHANGESKU Then
                pnlChangeSku.Visible = True
                TECHANHESKU.Visible = True
                TECHANHESKU.Restart()
                Dim tds As DataTable = TECHANHESKU.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TECHANHESKU.PreDefinedValues = vals

            Else
                pnlChangeSku.Visible = False
                pnlSubQtyVendorReturns.Visible = False
            End If
            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.CREATELOAD Then
                If IsCRVR() Then
                    pnlCreateLoad.Visible = False
                    pnlCreateLoadNew.Visible = True
                    TECREATELOADNEW.Visible = True
                    TECREATELOADNEW.Restart()
                    Dim tds As DataTable = TECREATELOADNEW.CreateDataTableForSelectedRecord()
                    Dim vals As New Specialized.NameValueCollection
                    vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                    vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                    TECREATELOADNEW.PreDefinedValues = vals
                Else
                    pnlCreateLoadNew.Visible = False
                    pnlCreateLoad.Visible = True
                    TECREATELOAD.Visible = True
                    TECREATELOAD.Restart()
                    Dim tds As DataTable = TECREATELOAD.CreateDataTableForSelectedRecord()
                    Dim vals As New Specialized.NameValueCollection
                    vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                    vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                    TECREATELOAD.PreDefinedValues = vals
                End If
            Else
                pnlCreateLoad.Visible = False
                pnlCreateLoadNew.Visible = False
                pnlSubQtyVendorReturns.Visible = False
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.SUBQTY And ddReasonCode.SelectedValue = "VR" Then
                pnlSubQtyVendorReturns.Visible = True
                TEINVLOADADJSUBQTYVENDORRETURNS.Visible = True
                TEINVLOADADJSUBQTYVENDORRETURNS.Restart()
                Dim tds As DataTable = TEINVLOADADJSUBQTYVENDORRETURNS.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TEINVLOADADJSUBQTYVENDORRETURNS.PreDefinedValues = vals
                pnlAddSubQty.Visible = False
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.Visible = False
            Else
                pnlSubQtyVendorReturns.Visible = False
                pnlSubQtyCustomereturns.Visible = False
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.ADDQTY And ddReasonCode.SelectedValue = "CR" Then
                pnlSubQtyCustomereturns.Visible = True
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.Visible = True
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.Restart()
                Dim tds As DataTable = TEINVLOADADJSUBQTYCUSTOMERRETURNS.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.PreDefinedValues = vals
                pnlAddSubQty.Visible = False
                pnlSubQtyVendorReturns.Visible = False
            Else
                pnlSubQtyCustomereturns.Visible = False
                TEINVLOADADJSUBQTYCUSTOMERRETURNS.Visible = False
            End If
        Else
            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.ADDQTY Or ddInvAct.SelectedValue = WMS.Lib.INVENTORY.SUBQTY Then

                pnlAddSubQty.Visible = True
                TEINVLOADADJ.Visible = True
                TEINVLOADADJ.Restart()
                Dim tds As DataTable = TEINVLOADADJ.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TEINVLOADADJ.PreDefinedValues = vals
            Else
                pnlAddSubQty.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.SPLITLOAD Then
                pnlSplitLoad.Visible = True
                TESPLITLOAD.Visible = True
                TESPLITLOAD.Restart()
                Dim tds As DataTable = TESPLITLOAD.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TESPLITLOAD.PreDefinedValues = vals
            Else
                pnlSplitLoad.Visible = False
            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.CHANGEUOM Then
                pnlChangeUOM.Visible = True
                TECHANGEUOM.Visible = True
                TECHANGEUOM.Restart()
                Dim tds As DataTable = TECHANGEUOM.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TECHANGEUOM.PreDefinedValues = vals
            Else
                pnlChangeUOM.Visible = False

            End If

            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.CHANGESKU Then
                pnlChangeSku.Visible = True
                TECHANHESKU.Visible = True
                TECHANHESKU.Restart()
                Dim tds As DataTable = TECHANHESKU.CreateDataTableForSelectedRecord()
                Dim vals As New Specialized.NameValueCollection
                vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                TECHANHESKU.PreDefinedValues = vals

            Else
                pnlChangeSku.Visible = False
            End If
            If ddInvAct.SelectedValue = WMS.Lib.INVENTORY.CREATELOAD Then
                If IsCRVR() Then
                    pnlCreateLoad.Visible = False
                    pnlCreateLoadNew.Visible = True
                    TECREATELOADNEW.Visible = True
                    TECREATELOADNEW.Restart()
                    Dim tds As DataTable = TECREATELOADNEW.CreateDataTableForSelectedRecord()
                    Dim vals As New Specialized.NameValueCollection
                    vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                    vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                    TECREATELOADNEW.PreDefinedValues = vals
                Else
                    pnlCreateLoadNew.Visible = False
                    pnlCreateLoad.Visible = True
                    TECREATELOAD.Visible = True
                    TECREATELOAD.Restart()
                    Dim tds As DataTable = TECREATELOAD.CreateDataTableForSelectedRecord()
                    Dim vals As New Specialized.NameValueCollection
                    vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
                    vals.Add("REASONCODE", ddReasonCode.SelectedValue)
                    TECREATELOAD.PreDefinedValues = vals
                End If
            Else
                pnlCreateLoad.Visible = False
                pnlCreateLoadNew.Visible = False
            End If
        End If

    End Sub

    Private Function IsCRVR() As Boolean
        If ddReasonCode.SelectedValue = "CR" Or ddReasonCode.SelectedValue = "VR" Then
            Return True
        End If
    End Function

    'End RWMS-474

    Private Sub TECREATELOAD_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECREATELOAD.CreatedChildControls
        With TECREATELOAD.ActionBar
            With .Button("Save")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Load"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InventoryAdjustment"
                .CommandName = "invadjcreateload"
            End With
        End With
    End Sub

    Private Sub TEINVLOADADJ_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEINVLOADADJ.CreatedChildControls
        With TEINVLOADADJ.ActionBar
            .AddSpacer()

            .AddExecButton("SubmitAdjustment", "Adjust", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SubmitAdjustment")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "SubmitAdjustment"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure?", Nothing)
            End With
        End With
    End Sub



    Private Sub TEINVLOADADJSUBQTYCUSTOMERRETURNS_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEINVLOADADJSUBQTYCUSTOMERRETURNS.CreatedChildControls
        With TEINVLOADADJSUBQTYCUSTOMERRETURNS.ActionBar
            .AddSpacer()

            .AddExecButton("SubmitCustomerReturn", "CustomerReturn", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SubmitCustomerReturn")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "SubmitCustomerReturn"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure?", Nothing)
            End With
        End With
    End Sub

    Private Sub TEINVLOADADJSUBQTYVENDORRETURNS_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEINVLOADADJSUBQTYVENDORRETURNS.CreatedChildControls
        With TEINVLOADADJSUBQTYVENDORRETURNS.ActionBar
            .AddSpacer()

            .AddExecButton("SubmitAdjustmentVendorReturns", "Adjust", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SubmitAdjustmentVendorReturns")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "SubmitAdjustmentVendorReturns"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure?", Nothing)
            End With
        End With
    End Sub

    Private Sub TEINVLOADADJ_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEINVLOADADJ.AfterItemCommand
        If e.CommandName = "SubmitAdjustment" Then
            TEINVLOADADJ.RefreshData()
        End If
    End Sub

    Private Sub ddReasonCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReasonCode.SelectedIndexChanged
        If Not IsPostBack Then
            Dim tds As DataTable = TEINVLOADADJ.CreateDataTableForSelectedRecord()
            Dim vals As New Specialized.NameValueCollection
            vals.Add("ADJUSTMENTTYPE", ddInvAct.SelectedValue)
            vals.Add("REASONCODE", ddReasonCode.SelectedValue)
            TEINVLOADADJ.PreDefinedValues = vals
        Else
            ValidateInvAdj()
        End If
        'Added For RWMS -366 Start
        Made4Net.Shared.ContextSwitch.Current.Session("REASONCODE") = ddReasonCode.SelectedValue
        'Added For RWMS -366 End
        'Added For RWMS -605 Start
        Made4Net.Shared.ContextSwitch.Current.Session("REASONCODE") = ddReasonCode.SelectedValue
        'Added For RWMS -605 End

    End Sub

    Private Sub TESPLITLOAD_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESPLITLOAD.CreatedChildControls
        With TESPLITLOAD.ActionBar
            .AddSpacer()

            .AddExecButton("SubmitSplit", "Split", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SubmitSplit")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Load"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InventoryAdjustment"
                .CommandName = "SubmitSplit"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
        End With
    End Sub

    Private Sub TESPLITLOAD_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TESPLITLOAD.AfterItemCommand
        If e.CommandName = "SubmitSplit" Then
            TESPLITLOAD.RefreshData()
        End If
    End Sub

    Private Sub TECHANGEUOM_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECHANGEUOM.CreatedChildControls
        With TECHANGEUOM.ActionBar
            With .Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "ChangeUOM"
            End With
        End With
    End Sub

    Private Sub TECHANGEUOM_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TECHANGEUOM.AfterItemCommand
        If e.CommandName = "Save" Then
            TECHANGEUOM.RefreshData()
        End If
    End Sub

    Private Sub TECHANHESKU_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECHANHESKU.CreatedChildControls
        With TECHANHESKU.ActionBar
            .AddSpacer()

            .AddExecButton("SubmitChangeSku", "Submit Change Sku", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SubmitChangeSku")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "SubmitChangeSku"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure?", Nothing)
            End With
        End With
    End Sub

    'Begin RWMS-474 Author: Ravi
    Protected Sub TECREATELOADNEW_CreatedChildControls(sender As Object, e As EventArgs) Handles TECREATELOADNEW.CreatedChildControls
        With TECREATELOADNEW.ActionBar
            With .Button("Save")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Load"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InventoryAdjustment"
                .CommandName = "invadjcreateload"
            End With
        End With
    End Sub

    Protected Sub TEINVLOADADJSUBQTYVENDORRETURNS_Load(sender As Object, e As EventArgs) Handles TEINVLOADADJSUBQTYVENDORRETURNS.Load
        TEINVLOADADJSUBQTYVENDORRETURNS.ForbidRequiredFieldsInSearchMode = False
    End Sub

    Private Sub TEINVLOADADJSUBQTYVENDORRETURNS_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEINVLOADADJSUBQTYVENDORRETURNS.AfterItemCommand
        If e.CommandName = "Search" Then
            TEINVLOADADJSUBQTYVENDORRETURNS.RefreshData()
        End If
    End Sub

    Private Sub TEINVLOADADJSUBQTYVENDORRETURNS_AfterModeChanged(ByVal sender As Object, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEINVLOADADJSUBQTYVENDORRETURNS.AfterModeChange
        If e.UpdateType Then
            TEINVLOADADJSUBQTYVENDORRETURNS.RefreshData()
        End If
    End Sub

    Protected Sub pnlSubQtyVendorReturns_Load(sender As Object, e As EventArgs) Handles pnlSubQtyVendorReturns.Load

    End Sub

    Private Sub ValidateCreatePayLoadCustomerReturns(dr As DataRow)
        Dim sql As String = "select count(*) As CountOb from OUTBOUNDORHEADER where orderid = '"
        ' Validate Order ID
        'RWMS-2317
        If IsDBNull(dr("HostReferenceID")) Then
            Throw New ApplicationException("Please fill HostReferenceID")
        End If
        'RWMS-2317
        Dim oborder As String = dr("HostReferenceID")
        If String.IsNullOrEmpty(oborder) Then
            Throw New M4NException(New Exception(String.Format("This is not a valid Outbound Order number - '{0}'", oborder)), "1059", String.Format("This is not a valid Outbound Order number - '{0}'", oborder))
        Else
            sql = sql + oborder + "'"
            Dim orderIdDt As DataTable = New DataTable()
            DataInterface.FillDataset(sql, orderIdDt)
            Dim orderIdCount As Integer = 0
            If orderIdDt.Rows.Count > 0 Then
                If Not IsDBNull(orderIdDt.Rows(0)("CountOb")) Then
                    Try
                        orderIdCount = orderIdDt.Rows(0)("CountOb")
                    Catch ex As Exception
                        orderIdCount = 0
                    End Try
                End If
            End If

            If orderIdCount < 1 Then
                Throw New M4NException(New Exception("This is not a valid Outbound Order number - '" & oborder & "'"), "1060", "This is not a valid Outbound Order number - '" & oborder & "'")
            End If
        End If
        ' Validate Item
        If String.IsNullOrEmpty(dr("SKU")) Then
            Throw New M4NException(New Exception("Item - '' not part of this order – '" & oborder & "'"), "1059", "Item - '' not part of this order – '" & oborder & "'")
        Else
            sql = "select count(*) from OUTBOUNDORDETAIL  where orderid = '{0}' and sku = '{1}'"
            sql = String.Format(sql, oborder, dr("SKU"))
            Dim itemCount As Integer = DataInterface.ExecuteScalar(sql)
            If itemCount < 1 Then
                Throw New M4NException(New Exception(String.Format("Item - '{0}' not part of this order – '{1}'", dr("SKU"), oborder)), "1069", String.Format("Item - '{0}' not part of this order – '{1}'", dr("SKU"), oborder))
            End If
        End If
        ' Validate OutboundOrder status
        sql = "select STATUS from OUTBOUNDORHEADER where orderid = '{0}'"
        sql = String.Format(sql, oborder)
        Dim orderstatus As String = DataInterface.ExecuteScalar(sql)
        If orderstatus = "PICKING" Then
            Throw New M4NException(New Exception(String.Format("Cannot create payload - invalid order status for Outbound Order – {0}.", oborder)), "1069", String.Format("Cannot create payload - invalid order status for Outbound Order – {0}.", oborder))
        End If
        ' Validate return qty
        Dim qtyAvailbeForReturn As Integer = 0
        Dim qtyAtObDT As DataTable = New DataTable()
        sql = "select sum(OUTBOUNDORDETAIL.QTYSHIPPED) As SUMQTY from OUTBOUNDORDETAIL where orderid='" & oborder & "' and sku='" & dr("SKU") & "'"
        Dim qtyAtOb As Decimal = 0
        DataInterface.FillDataset(sql, qtyAtObDT)
        If qtyAtObDT.Rows.Count > 0 Then
            If Not IsDBNull(qtyAtObDT.Rows(0)("SUMQTY")) Then
                Try
                    qtyAtOb = qtyAtObDT.Rows(0)("SUMQTY")
                Catch ex As Exception
                    qtyAtOb = 0
                End Try
            End If
        End If

        sql = "select sum(QTY) As SUMQTY from INVENTORYTRANS where HostReferenceID='" & oborder & "' and SKU='" & dr("SKU") & "' and INVTRNTYPE='ADDQTY' and REASONCODE='CR'"
        Dim previousReturns As DataTable = New DataTable()
        Dim previousReturnsInt As Decimal = 0
        DataInterface.FillDataset(sql, previousReturns)
        If previousReturns.Rows.Count > 0 Then
            If Not IsDBNull(previousReturns.Rows(0)("SUMQTY")) Then
                Try
                    previousReturnsInt = previousReturns.Rows(0)("SUMQTY")
                Catch ex As Exception
                    previousReturnsInt = 0
                End Try
            End If
        End If
        qtyAvailbeForReturn = qtyAtOb - previousReturnsInt
        Dim UNITS As Decimal = Convert.ToDecimal(dr("UNITS"))
        If UNITS > qtyAvailbeForReturn Then
            'RWMS-2316
            Throw New M4NException(New Exception("Quantity entered " & UNITS & " is greater than quantity allowed for this Outbound Order  - " & qtyAvailbeForReturn & "."), "1055", "Quantity entered " & UNITS & " is greater than quantity allowed for this Outbound Order  - " & qtyAvailbeForReturn & ".")
            'RWMS-2316
        End If
    End Sub

End Class