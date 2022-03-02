Imports WMS.Logic
Imports Made4Net.DataAccess
Imports RWMS.Logic

Public Class Receipt
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterReceipt As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEReceiptDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TECL As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEReceiptLOADS As Made4Net.WebControls.TableEditor
    Protected WithEvents TEReceiptASN As Made4Net.WebControls.TableEditor
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEHUTrans As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector
    Protected WithEvents lblAdjReasonCode As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddReasonCode As Made4Net.WebControls.DropDownList
    Protected WithEvents pnlAdj As System.Web.UI.WebControls.Panel
    Protected WithEvents TEAvailableInboundLines As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector2 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEYardEntry As Made4Net.WebControls.TableEditor
    Protected WithEvents TEReceivingException As Made4Net.WebControls.TableEditor
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


#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'RWMS-2684
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2684 END

        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim recId As String
        Select Case CommandName.ToLower
            Case "addhu"
                dr = ds.Tables(0).Rows(0)
                recId = Session("HUTRANSRECEIPTID")
                Dim oRec As New WMS.Logic.ReceiptHeader(recId)
                oRec.AddHandlingUnit(dr("HUTYPE"), dr("HUQTY"), UserId)
            Case "edithu"
                dr = ds.Tables(0).Rows(0)
                recId = Session("HUTRANSRECEIPTID")
                Dim oRec As New WMS.Logic.ReceiptHeader(recId)
                oRec.UpdateHandlingUnit(dr("TRANSACTIONID"), dr("HUQTY"), UserId)
            Case "cancelreceive"
                Dim dt As DataTable

                For Each dr In ds.Tables(0).Rows
                    Dim load As WMS.Logic.Load = New Logic.Load(Convert.ToString(dr("loadid")))
                    recId = load.RECEIPT
                    Dim oRec As New WMS.Logic.ReceiptHeader(recId)
                    dt = New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(String.Format("select top 1 pkey1, ISNULL(weight,0) weight from ATTRIBUTE WHERE PKEYTYPE='INVTRANS' AND PKEY1 IN (SELECT INVTRANS FROM INVENTORYTRANS WHERE LOADID = '{0}') AND PKEY2='' AND PKEY3='' order by PKEY1 desc", dr("loadid")), dt)

                    'RWMS-2684
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" trying to unreceive receipt..."))
                        wmsrdtLogger.Write(String.Format(" receipt:{0}, receiptline:{1}, load:{2}", recId, dr("receiptline"), dr("loadid")))
                    End If
                    'RWMS-2684 END

                    oRec.CancelReceive(dr("receiptline"), dr("loadid"), dr("REASONCODE"), UserId)

                    'RWMS-2684
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" unreceived receipt."))
                    End If
                    'RWMS-2684 END

                    RWMS.Logic.AppUtil.UpdateReceiptLineAverageWeight(recId, dr("receiptline"))


                    If dt.Rows.Count > 0 Then
                        If Not IsDBNull(dt.Rows(0)("weight")) AndAlso Not String.IsNullOrEmpty(dt.Rows(0)("weight")) Then
                            If dt.Rows(0)("weight") > 0 Then

                                DataInterface.RunSQL(String.Format("UPDATE RECEIPTDETAIL SET RECEIVEDWEIGHT = ISNULL(RECEIVEDWEIGHT,0) -{2} Where  RECEIPT = '{0}' and RECEIPTLINE='{1}' and ISNULL(RECEIVEDWEIGHT,0)>0", recId, dr("receiptline"), dt.Rows(0)("weight")))

                                DataInterface.RunSQL(String.Format("UPDATE ATTRIBUTE SET WEIGHT = 0 WHERE PKEYTYPE='INVTRANS' AND PKEY1 = '{0}' AND PKEY2='' AND PKEY3=''", dt.Rows(0)("pkey1")))

                            End If
                        End If
                    End If
                Next
                dt = Nothing

            Case "cancelreceipt"
                For Each dr In ds.Tables(0).Rows
                    Dim oRec As New WMS.Logic.ReceiptHeader(dr("receipt"))
                    oRec.Cancel(UserId)
                Next
                'Added for RWMS-1484 and RWMS-1402
                For Each dr1 As DataRow In ds.Tables(0).Rows
                    RWMS.Logic.AppUtil.CloseInboundByReceiptCancel(dr1("RECEIPT"))
                Next
                'Ended for RWMS-1484 and RWMS-1402

            Case "importinboundlines"
                For Each dr In ds.Tables(0).Rows
                    Dim oAttCol As AttributesCollection
                    Dim oReceipt As New ReceiptHeader(dr("RECEIPT"))
                    If IsDBNull(dr("SKU")) Then
                        oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(dr, True)
                    Else
                        oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(dr)
                    End If
                    Dim oOrder As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                    Dim oInboundLine As WMS.Logic.InboundOrderDetail = oOrder.Lines.Line(dr("ORDERLINE"))
                    oReceipt.addLineFromOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Nothing, Nothing, oInboundLine.INVENTORYSTATUS, oOrder.SOURCECOMPANY, oOrder.COMPANYTYPE, -1, Nothing, UserId, oAttCol, Nothing, Nothing, Nothing, dr("DOCUMENTTYPE"))
                Next
            Case "autodockassignment"
                For Each dr In ds.Tables(0).Rows
                    Dim oRec As New WMS.Logic.ReceiptHeader(dr("RECEIPT"))
                    oRec.SetBestDoorForReceiving(UserId)
                Next
            Case "atdock"
                For Each dr In ds.Tables(0).Rows
                    Dim oRec As New WMS.Logic.ReceiptHeader(dr("RECEIPT"))
                    'Added for RWMS-2151  start
                    'oRec.STATUS = WMS.Lib.Statuses.Receipt.ATDOCK
                    'oRec.Save(UserId)
                    'ended for RWMS-2151  end
                    oRec.AtDock(WMS.Logic.Common.GetCurrentUser)
                    ' Create unloading task's
                    Dim ult As UnloadingTask = New UnloadingTask()
                    ult.Create(oRec.DOOR, UserId)
                Next
            Case "addyardentry"
                'dr = ds.Tables(0).Rows(0)
                'Dim oRec As New WMS.Logic.ReceiptHeader(Session("YARDENTRYRECEIPTID"))
                'Dim yardentryId As String = Made4Net.Shared.getNextCounter("YARDENTRY")
                'Dim ye As WMS.Logic.YardEntry = New WMS.Logic.YardEntry
                'ye.Create(yardentryId, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CARRIER")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRAILER")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("YARDLOCATION")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE")), UserId)
                'ye.Schedule(UserId)
                'oRec.DOOR = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DOOR"))
                'oRec.SCHEDULEDDATE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE"))
                'oRec.CARRIERCOMPANY = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CARRIER"))
                'oRec.VEHICLE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLE"))
                'oRec.TRAILER = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRAILER"))
                'oRec.Save(UserId)
                'oRec.AssignToYardEntry(yardentryId, UserId)
                'Session("RECEIPTYARDENTRYID") = yardentryId
            Case "edityardentry"
                'dr = ds.Tables(0).Rows(0)
                'Dim ye As WMS.Logic.YardEntry = New WMS.Logic.YardEntry(dr("YARDENTRYID"))
                'ye.Update(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CARRIER")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRAILER")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("YARDLOCATION")), ye.CHECKINDATE, ye.CHECKOUTDATE, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE")), UserId)
                'ye.Schedule(UserId)
                'Dim oRec As New WMS.Logic.ReceiptHeader(Session("YARDENTRYRECEIPTID"))
                'oRec.DOOR = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DOOR"))
                'oRec.SCHEDULEDDATE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE"))
                'oRec.CARRIERCOMPANY = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CARRIER"))
                'oRec.VEHICLE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLE"))
                'oRec.TRAILER = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRAILER"))
                'oRec.Save(UserId)
            Case "save"
                dr = ds.Tables(0).Rows(0)
                If ReceiptHeader.Exists(dr("RECEIPT")) Then
                    Dim rh As ReceiptHeader = New ReceiptHeader(dr("RECEIPT"))
                    If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                        Message = t.Translate("Cannot update a closed receipt")
                        Throw New ApplicationException(Message)
                    End If
                    If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                        Message = t.Translate("Cannot update a cancelled receipt")
                        Throw New ApplicationException(Message)
                    End If

                End If

                Dim rh1 As New WMS.Logic.ReceiptHeader(Sender, CommandName, XMLSchema, XMLData, Message)

            Case "saveline"
                dr = ds.Tables(0).Rows(0)

                Dim rh As ReceiptHeader = New ReceiptHeader(dr("RECEIPT"))
                If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                    Message = t.Translate("Cannot update a closed receipt")
                    Throw New ApplicationException(Message)
                End If
                If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                    Message = t.Translate("Cannot update a cancelled receipt")
                    Throw New ApplicationException(Message)
                End If



                Dim units As Decimal
                If IsDBNull(dr("skuuom")) Then
                    units = dr("QTYEXPECTED")
                Else
                    units = CalcUnits(dr("CONSIGNEE"), dr("sku"), dr("QTYEXPECTED"), dr("SKUUOM")) * dr("QTYEXPECTED")
                End If

                Dim oAttCol As AttributesCollection
                If IsDBNull(dr("SKU")) Or dr("SKU") = "" Then
                    oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(dr, True)
                Else
                    oAttCol = WMS.Logic.SkuClass.ExtractLoadAttributes(dr)
                End If

                If dr("RECEIPTLINE") Is DBNull.Value Or dr("RECEIPTLINE") = 0 Then
                    rh.addLine(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKU")), units, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFORD")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFORDLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("COMPANY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("COMPANYTYPE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AVGWEIGHT")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AVGWEIGHTUOM")), 0, Common.GetCurrentUser, oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTQTY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTSKU")))
                Else
                    rh.UpdateLine(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RECEIPTLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYEXPECTED")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFORD")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFORDLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("COMPANY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("COMPANYTYPE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AVGWEIGHT")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AVGWEIGHTUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RECEIVEDWEIGHT")), Common.GetCurrentUser, oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTQTY")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INPUTUOM")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DOCUMENTTYPE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("orderid")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("orderline")))
                End If
            Case "deleteline"
                dr = ds.Tables(0).Rows(0)
                Dim rd As New WMS.Logic.ReceiptDetail(dr("RECEIPT"), dr("RECEIPTLINE"))
                Dim rh As New WMS.Logic.ReceiptHeader(dr("RECEIPT"))

                If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                    Throw New ApplicationException(t.Translate("Cannot delete line. Receipt is closed"))
                End If
                If rd.QTYRECEIVED > 0 Then
                    Throw New ApplicationException(t.Translate("Cannot delete line with received quantity"))
                End If
                rd.Delete()
            Case "exportheaderstoasn"
                If ds.Tables(0).Rows.Count = 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "No rows selected.", "No rows selected.")
                End If
                'Added for RWMS-1470  and RWMS-1340 Start
                Dim isDoor As Boolean = False
                Dim rcpt As String = String.Empty
                Dim cntTrue, cntFalse As Integer
                Dim msg As String = "No Door number on receipt : "

                For Each rDr As DataRow In ds.Tables(0).Rows
                    If rDr("Door").ToString() = "" Then
                        isDoor = False
                        rcpt = rcpt & rDr("Receipt").ToString() & ","
                        cntFalse = cntFalse + 1
                    Else
                        isDoor = True
                        cntTrue = cntTrue + 1
                    End If
                    If isDoor Then
                        Dim receiptHeaderObj As New WMS.Logic.ReceiptHeader(rDr("Receipt"))
                        receiptHeaderObj.CreateASNDetailsForReceipt(Common.GetCurrentUser())
                    End If
                Next
                If cntTrue > 0 And cntFalse > 0 Then
                    Message = "ASN created. " & msg & rcpt.Trim(",")
                ElseIf cntTrue = 0 And cntFalse > 0 Then
                    Throw New ApplicationException(t.Translate(msg & rcpt.Trim(",")))
                Else
                    Message = "ASN created. "
                End If
                'Ended  for RWMS-1470  and RWMS-1340 End

            Case "exportdetailstoasn"
                If ds.Tables(0).Rows.Count = 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "No rows selected.", "No rows selected.")
                End If
                Dim receiptHeaderObj As New WMS.Logic.ReceiptHeader(ds.Tables(0).Rows(0)("Receipt"))
                For Each dDr As DataRow In ds.Tables(0).Rows
                    receiptHeaderObj.CreateASNDetailsForLine(dDr("ReceiptLine"), Common.GetCurrentUser())
                Next
            Case "setconfirmed"
                For Each dDr As DataRow In ds.Tables(0).Rows
                    Dim oReceipt As New ReceiptHeader(dDr("RECEIPT"))
                    oReceipt.Confirm(UserId)
                Next
            Case "close"

                Dim RH As New WMS.Logic.ReceiptHeader(Sender, CommandName, XMLSchema, XMLData, Message)

                For Each dDr As DataRow In ds.Tables(0).Rows
                    RWMS.Logic.AppUtil.UpdateReceiptAverageWeight(dDr("RECEIPT"))
                Next

                For Each dDr As DataRow In ds.Tables(0).Rows
                    If Not RWMS.Logic.AppUtil.CloseInboundByReceipt(dDr("RECEIPT"), Message) Then
                        Throw New ApplicationException(Message)
                        Exit Sub
                    End If
                Next
            Case "update" 'ReceivingException
                dr = ds.Tables(0).Rows(0)
                Dim qty As String
                Dim rh As New WMS.Logic.ReceiptHeader(dr("RECEIPT"))

                If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                    Throw New ApplicationException(t.Translate("Cannot delete exception. Receipt is closed"))
                End If
                If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                    Throw New ApplicationException(t.Translate("Cannot delete exception. Receipt is cancelled"))
                End If

                Dim rd As New WMS.Logic.ReceiptDetail(dr("RECEIPT"), dr("RECEIPTLINE"))
                Dim sk As New WMS.Logic.SKU(rd.CONSIGNEE, rd.SKU)
                If Not IsNumeric(dr("QTY").ToString) Or dr("QTY").ToString <= 0 Then
                    Message = t.Translate("Error qty")
                    Throw New ApplicationException(Message)
                    Exit Sub
                End If
                qty = sk.ConvertToUnits(dr("UOM")) * dr("QTY")
                If qty > rd.QTYEXPECTED - rd.QTYRECEIVED Then
                    Throw New ApplicationException(t.Translate("Exception quantity cannot be greater than receipt line open quantity"))
                End If

                Dim re As New WMS.Logic.ReceivingException(Sender, CommandName, XMLSchema, XMLData, Message)
            Case "insert" 'ReceivingException
                Dim RECEIPTLINE, qty As String
                dr = ds.Tables(0).Rows(0)

                For Each dr1 As DataRow In ds.Tables(0).Rows
                    If String.IsNullOrEmpty(dr1("RECEIPTLINE").ToString) Then
                        If Not getRecLine(dr1, RECEIPTLINE, Message) Then
                            Throw New ApplicationException(Message)
                            Exit Sub
                        End If
                    Else
                        RECEIPTLINE = dr1("RECEIPTLINE")
                    End If
                    If WMS.Logic.ReceivingException.Exists(dr1("RECEIPT"), RECEIPTLINE) Then
                        Throw New ApplicationException(t.Translate("Exception already exists"))
                    End If
                    Dim rh As New WMS.Logic.ReceiptHeader(dr("RECEIPT"))

                    If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                        Throw New ApplicationException(t.Translate("Cannot delete exception. Receipt is closed"))
                    End If
                    If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                        Throw New ApplicationException(t.Translate("Cannot delete exception. Receipt is cancelled"))
                    End If

                    Dim rd As New WMS.Logic.ReceiptDetail(dr1("RECEIPT"), RECEIPTLINE)
                    Dim sk As New WMS.Logic.SKU(rd.CONSIGNEE, rd.SKU)
                    If Not IsNumeric(dr1("QTY").ToString) Or dr1("QTY").ToString <= 0 Then
                        Message = t.Translate("Error qty")
                        Throw New ApplicationException(Message)
                        Exit Sub
                    End If
                    qty = sk.ConvertToUnits(dr1("UOM")) * dr1("QTY")
                    If qty > rd.QTYEXPECTED - rd.QTYRECEIVED Then
                        Throw New ApplicationException(t.Translate("Exception quantity cannot be greater than receipt line open quantity"))
                    End If

                    Dim re As New WMS.Logic.ReceivingException()
                    re.RECEIPT = dr1("RECEIPT")
                    re.RECEIPTLINE = RECEIPTLINE
                    re.REASONCODE = dr1("REASONCODE").ToString
                    re.QTY = qty
                    re.Save(WMS.Logic.GetCurrentUser)
                Next
            Case "deleteexception"
                Dim sql As String
                sql = "delete RECEIVINGEXCEPTION where RECEIPT='{0}' and RECEIPTLINE='{1}'"
                dr = ds.Tables(0).Rows(0)


                Dim rh As New WMS.Logic.ReceiptHeader(dr("RECEIPT"))

                If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                    Throw New ApplicationException(t.Translate("Cannot delete exception. Receipt is closed"))
                End If
                If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                    Throw New ApplicationException(t.Translate("Cannot delete exception. Receipt is cancelled"))
                End If
                sql = String.Format(sql, dr("RECEIPT"), dr("RECEIPTLINE"))

                Made4Net.DataAccess.DataInterface.RunSQL(sql)

        End Select
    End Sub

    Private Function getRecLine(ByVal dr As DataRow, ByRef RecLine As String, ByRef message As String) As Boolean
        Dim ret As Boolean = True
        Dim sql As String
        Dim dt As New DataTable
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        sql = "select ReceiptLine from RECEIPTDETAIL where Consignee='{0}' and Receipt = '{1}' and sku='{2}'"
        sql = String.Format(sql, dr("Consignee"), dr("Receipt"), dr("SKU"))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count = 0 Then
            message = t.Translate("No Data Found")
            Return False
        ElseIf dt.Rows.Count > 1 Then
            message = t.Translate("Found more than 1 record. Please enter receipt line")
            Return False
        Else
            RecLine = dt.Rows(0)("ReceiptLine")
        End If

        Return ret
    End Function

#End Region

#Region "Methods"

    Private Function CalcUnits(ByVal pConsignee As String, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pUom As String) As Decimal
        Dim oSku As New WMS.Logic.SKU(pConsignee, pSku)
        If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pUom) Then
            Return oSku.ConvertToUnits(pUom)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
        End If
        Return oSku.ConvertToUnits(pUom)
    End Function

#End Region

#Region "Handlers"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Try
                ddReasonCode.DataBind()
                ddReasonCode_SelectedIndexChanged(Nothing, Nothing)
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub TEMasterReceipt_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterReceipt.CreatedGrid
        TEMasterReceipt.ActionBar.AddSpacer()

        TEMasterReceipt.ActionBar.AddExecButton("PrintWS", "Print Receiving Worksheet", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
        With TEMasterReceipt.ActionBar.Button("PrintWS")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.ReceiptHeader"
        End With

        TEMasterReceipt.ActionBar.AddExecButton("PrintMN", "Print Receiving Manifest", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
        With TEMasterReceipt.ActionBar.Button("PrintMN")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.ReceiptHeader"
        End With

        TEMasterReceipt.ActionBar.AddSpacer()
    End Sub

    Private Sub TEMasterReceipt_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterReceipt.CreatedChildControls
        With TEMasterReceipt.ActionBar
            With .Button("Save")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.ReceiptHeader"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
            End With

            .AddExecButton("Receive", "Receive Receipt", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
            With .Button("Receive")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.ReceiptHeader"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "ReceiveFull"
            End With

            .AddSpacer()
            .AddExecButton("CancelReceipt", "Cancel Receipt", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            With .Button("CancelReceipt")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the receipt?"
                .CommandName = "CancelReceipt"
            End With

            .AddSpacer()
            .AddExecButton("Close", "Close Receipt", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCloseReceipt"))
            With .Button("Close")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.ReceiptHeader"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to close the receipt?"
                .CommandName = "Close"
            End With

            .AddSpacer()
            .AddExecButton("AutoDockAssignment", "Auto Dock Assignment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AutoDockAssignment")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "AutoDockAssignment"
            End With
            .AddExecButton("AtDock", "At Dock", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AtDock")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "AtDock"
            End With
            .AddExecButton("ExportToASN", "Create ASN Loads and Labels", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("ExportToASN")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "exportheaderstoasn"
            End With

            .AddSpacer()
            .AddExecButton("SetConfirmed", "Set Confirmed", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("SetConfirmed")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "SetConfirmed"
            End With
        End With

    End Sub

    Private Sub TEReceiptDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReceiptDetail.CreatedChildControls
        With TEReceiptDetail.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Receipt"
            .CommandName = "saveLine"
        End With

        With TEReceiptDetail.ActionBar.Button("Delete")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Receipt"
            .CommandName = "DeleteLine"
        End With

        'TEReceiptDetail.ActionBar().AddExecButton("ExportToASN", "Export To ASN", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        'With TEReceiptDetail.ActionBar().Button("ExportToASN")
        '    .ObjectDLL = "WMS.WebApp.dll"
        '    .ObjectName = "WMS.WebApp.Receipt"
        '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
        '    .CommandName = "exportdetailstoasn"
        'End With

    End Sub

    Private Sub TEMasterReceipt_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterReceipt.AfterItemCommand
        If e.CommandName = "Close" Then
            TEMasterReceipt.RefreshData()
        End If
    End Sub

    Private Sub TEReceiptASN_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReceiptASN.CreatedChildControls
        With TEReceiptASN.ActionBar
            'RWMS-1345 - added the print label icon
            .AddExecButton("printlabels", "Print Label", Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
            With .Button("printlabels")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.AsnDetail"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
            .AddSpacer()
            'End RWMS-1345
            .AddExecButton("Receive", "Receive By ID", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
            With .Button("Receive")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.AsnDetail"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "ReceiveASNLoad"
            End With
        End With
    End Sub

    Private Sub TEReceiptASN_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEReceiptASN.AfterItemCommand
        TEReceiptASN.RefreshData()
        TEMasterReceipt.RefreshData()
    End Sub

    Private Sub TEMasterReceipt_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterReceipt.RecordSelected
        Dim tds As DataTable = TEMasterReceipt.CreateDataTableForSelectedRecord(False)
        Session("HUTRANSRECEIPTID") = tds.Rows(0)("RECEIPT")
        Session("YARDENTRYRECEIPTID") = tds.Rows(0)("RECEIPT")
        Session("RECEIPTYARDENTRYID") = tds.Rows(0)("YARDENTRYID")
        Dim vals As New Specialized.NameValueCollection
        vals.Add("RECEIPT", tds.Rows(0)("RECEIPT"))
        TEAvailableInboundLines.PreDefinedValues = vals
        'TEYardEntry.PreDefinedValues = vals
    End Sub

    Private Sub TEHUTrans_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEHUTrans.CreatedChildControls
        With TEHUTrans
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                If TEHUTrans.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addhu"
                Else
                    .CommandName = "edithu"
                End If
            End With
        End With
    End Sub

    Private Sub ddReasonCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReasonCode.SelectedIndexChanged
        Dim vals As New Specialized.NameValueCollection
        vals.Add("REASONCODE", ddReasonCode.SelectedValue)
        TEReceiptLOADS.PreDefinedValues = vals
    End Sub

    Private Sub TEReceiptLOADS_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReceiptLOADS.CreatedChildControls
        With TEReceiptLOADS.ActionBar
            .AddExecButton("CancelReceive", "Cancel Receive", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
            With .Button("CancelReceive")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "CancelReceive"
            End With
        End With
    End Sub

    Private Sub TEAvailableInboundLines_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAvailableInboundLines.CreatedChildControls
        With TEAvailableInboundLines.ActionBar
            .AddExecButton("ImportInboundLines", "Import Inbound Lines", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("ImportInboundLines")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .CommandName = "ImportInboundLines"
            End With
        End With
    End Sub

    Private Sub TEYardEntry_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEYardEntry.AfterItemCommand
        If e.CommandName.Equals("addyardentry", StringComparison.OrdinalIgnoreCase) OrElse e.CommandName.Equals("edityardentry", StringComparison.OrdinalIgnoreCase) Then
            TEMasterReceipt.RefreshData()

            TEYardEntry.RefreshData()
            Dataconnector2.Shake()
        End If
    End Sub

    'Private Sub TEYardEntry_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEYardEntry.CreatedChildControls
    '    With TEYardEntry
    '        With .ActionBar.Button("Save")
    '            .ObjectDLL = "WMS.WebApp.dll"
    '            .ObjectName = "WMS.WebApp.Receipt"
    '            If TEYardEntry.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
    '                .CommandName = "addyardentry"
    '            Else
    '                .CommandName = "edityardentry"
    '            End If
    '        End With
    '        If Not Session("RECEIPTYARDENTRYID") Is Nothing AndAlso Not IsDBNull(Session("RECEIPTYARDENTRYID")) AndAlso Session("RECEIPTYARDENTRYID") <> "" Then
    '            .DefaultMode = Made4Net.WebControls.TableEditorMode.View
    '        Else
    '            .DefaultMode = Made4Net.WebControls.TableEditorMode.Grid
    '        End If
    '    End With
    'End Sub

    Private Sub TEReceivingException_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReceivingException.CreatedChildControls
        With TEReceivingException
            With .ActionBar.Button("Save")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.ReceivingException"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                If TEReceivingException.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "insert"
                Else
                    .CommandName = "update"
                End If
            End With
            With .ActionBar.Button("Delete")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.ReceivingException"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Receipt"
                '  If TEReceivingException.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "DeleteException"

                'End If
            End With
        End With
    End Sub

#End Region


End Class