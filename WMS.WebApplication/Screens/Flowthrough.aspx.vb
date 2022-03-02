Imports WMS.Logic
Imports Made4Net.Shared.Conversion
Imports Made4Net.DataAccess
Imports System.Data
Imports RWMS.Logic

Public Class Flowtrough
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterFlowtrough As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEFlowtroughDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TEFlowtroughLoad As Made4Net.WebControls.TableEditor
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TECL As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector

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
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "assignsl"
                Dim SLSetter As New StagingLaneAssignmentSetter
                For Each dr In ds.Tables(0).Rows
                    SLSetter.SetDocumentStagingLane(WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH, dr("CONSIGNEE"), dr("FLOWTHROUGH"))
                Next
                Message = ts.Translate("Orders Assigned To Staging Lanes!")
            Case "update"
                dr = ds.Tables(0).Rows(0)
                Dim _consignee As String = dr("CONSIGNEE")
                Dim _flowthrough As String = dr("FLOWTHROUGH")
                'Modified for RWMS-2138(RWMS-2017) Start(Change the parameter from Nothing to Convert.ReplaceDBNull(dr("ORDERTYPE")))
                Edit(dr("CONSIGNEE"), dr("FLOWTHROUGH"), Convert.ReplaceDBNull(dr("ORDERTYPE")), Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                          dr("SOURCECOMPANY"), dr("SOURCECOMPANYTYPE"), dr("TARGETCOMPANY"), dr("TARGETCOMPANYTYPE"), Convert.ReplaceDBNull(dr("STATUS")), Convert.ReplaceDBNull(dr("STATUSDATE")), _
                          Convert.ReplaceDBNull(dr("NOTES")), DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                          Convert.ReplaceDBNull(dr("requesteddeliverydate")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SHIPPEDDATE")), _
                          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("SHIPMENT")), Nothing, Convert.ReplaceDBNull(dr("LOADINGSEQ")), Nothing, _
                          Nothing, Nothing, Nothing, Nothing, Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                          DateTime.Now, Common.GetCurrentUser)
                'Modified for RWMS-2138(RWMS-2017) End

                'Added for RWMS-2138(RWMS-2017) Start
                UpdateFields(dr("CONSIGNEE"), dr("FLOWTHROUGH"), Convert.ReplaceDBNull(dr("SOURCEREF")), Convert.ReplaceDBNull(dr("TARGETREF")))
                'Added for RWMS-2138(RWMS-2017) End
            Case "create"
                dr = ds.Tables(0).Rows(0)
                'Added for RWMS-2138(RWMS-2017) Start
                Dim FLID As String = Convert.ReplaceDBNull(dr("FLOWTHROUGH"))
                If String.IsNullOrEmpty(FLID) Then
                    FLID = Made4Net.Shared.Util.getNextCounter("FLOWTHROUGH")
                End If
                'Added for RWMS-2138(RWMS-2017) End

                'Modified for RWMS-2138(RWMS-2017) Start(changed the parameter dr("FLOWTHROUGH") to FLID)
                CreateNew(dr("CONSIGNEE"), FLID, Convert.ReplaceDBNull(dr("ORDERTYPE")), Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                          dr("SOURCECOMPANY"), dr("SOURCECOMPANYTYPE"), dr("TARGETCOMPANY"), dr("TARGETCOMPANYTYPE"), Nothing, _
                          Convert.ReplaceDBNull(dr("NOTES")), WMS.Logic.Warehouse.CurrentWarehouseArea, DateTime.Now(), Convert.ReplaceDBNull(dr("SCHEDULEDARRIVALDATE")), _
                          Convert.ReplaceDBNull(dr("REQUESTEDDELIVERYDATE")), Convert.ReplaceDBNull(dr("SCHEDULEDDELIVERYDATE")), Nothing, _
                          Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("SHIPMENT")), Nothing, Convert.ReplaceDBNull(dr("LOADINGSEQ")), Nothing, _
                          Nothing, Nothing, Nothing, Nothing, Convert.ReplaceDBNull(dr("SHIPTO")), Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                          DateTime.Now, Common.GetCurrentUser, DateTime.Now, Common.GetCurrentUser)
                'Modified for RWMS-2138(RWMS-2017) End

                'Added for RWMS-2138(RWMS-2017) Start
                UpdateFields(dr("CONSIGNEE"), FLID, Convert.ReplaceDBNull(dr("SOURCEREF")), Convert.ReplaceDBNull(dr("TARGETREF")))
                Message = Made4Net.WebControls.TranslationManager.Translate("FLOWTHROUGH created ") & FLID
                'Added for RWMS-2138(RWMS-2017) End
            Case "createreceipt"
                Dim inb As New WMS.Logic.Flowthrough(Sender, CommandName, XMLSchema, XMLData, Message)

                Dim sreceipt As String = Message.Substring(Message.LastIndexOf(" ")).Trim()

                Dim orec As New WMS.Logic.ReceiptHeader(sreceipt)
                WMS.Logic.ReceiptHeader.PrintReceivingDoc(sreceipt, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, False)

                For Each dr In ds.Tables(0).Rows
                    Dim FLOWTHROUGH As String = dr("FLOWTHROUGH")
                    Dim CONSIGNEE As String = dr("CONSIGNEE")

                    Dim strsql As String
                    strsql = "update ReceiptDetail  set REFORD = ih.REFERENCEORD  " _
                     & " FROM dbo.FLOWTHROUGHHEADER AS ih INNER JOIN " _
                     & " dbo.FLOWTHROUGHDETAIL AS id ON ih.CONSIGNEE = id.CONSIGNEE AND ih.FLOWTHROUGH = id.FLOWTHROUGH INNER JOIN " _
                     & " dbo.RECEIPTDETAIL AS rd INNER JOIN " _
                     & " dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT ON id.FLOWTHROUGH = rd.ORDERID AND id.FLOWTHROUGHLINE = rd.ORDERLINE AND  " _
                     & " id.CONSIGNEE = rd.CONSIGNEE " _
                     & " WHERE rd.CONSIGNEE='{0}' and rd.ORDERID = '{1}' " ' AND rd.RECEIPT = '{2}' "
                    strsql = String.Format(strsql, CONSIGNEE, FLOWTHROUGH)
                    'strsql = String.Format(strsql, inb.CONSIGNEE, inb.FLOWTHROUGH, receipt)

                    Made4Net.DataAccess.DataInterface.RunSQL(strsql)

                Next

            Case "cancel"



                For Each dr In ds.Tables(0).Rows
                    If Not AppUtil.cancelFlowthrough(dr("CONSIGNEE"), dr("FLOWTHROUGH"), Message) Then
                        Throw New ApplicationException(Message)
                    End If
                Next

                Dim inb As New WMS.Logic.Flowthrough(Sender, CommandName, XMLSchema, XMLData, Message)

        End Select
    End Sub

    Public Sub CreateNew(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pOrdertype As String, ByVal pReferenceord As String, _
                          ByVal pSourcecompany As String, ByVal pSourcecompanytype As String, ByVal pTargetcompany As String, ByVal pTargetcompanytype As String, _
                          ByVal pStatusdate As DateTime, ByVal pNotes As String, ByVal pStagingWarehousearea As String, _
                          ByVal pCreatedate As DateTime, ByVal pScheduledarrivaldate As DateTime, ByVal pRequesteddeliverydate As DateTime, _
                          ByVal pScheduleddeliverydate As DateTime, ByVal pShippeddate As DateTime, ByVal pStaginglane As String, _
                          ByVal pShipment As String, ByVal pStopnumber As String, ByVal pLoadingseq As String, ByVal pRoutingset As String, _
                          ByVal pRoute As String, ByVal pDeliverystatus As String, ByVal pPod As String, ByVal pOrderpriority As String, _
                          ByVal pShipTo As String, ByVal pReceivedFrom As String, ByVal pAdddate As DateTime, ByVal pAdduser As String, ByVal pEditdate As DateTime, ByVal pEdituser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        Dim fl As New WMS.Logic.Flowthrough(pConsignee, pFlowthrough)

        If pStaginglane Is Nothing Then
            pStaginglane = ""
        End If

        exist = WMS.Logic.Consignee.Exists(pConsignee)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Consignee", "Can't create order.Invalid Consignee")
            Throw m4nEx
        Else
            fl.CONSIGNEE = pConsignee
        End If

        exist = WMS.Logic.Flowthrough.Exists(pConsignee, pFlowthrough)
        If exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.OrderId already Exist", "Can't create order.OrderId already Exist")
            Throw m4nEx
        Else
            fl.FLOWTHROUGH = pFlowthrough
        End If

        exist = WMS.Logic.Company.Exists(fl.CONSIGNEE, pSourcecompany, pSourcecompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
            Throw m4nEx
        Else
            If pReceivedFrom Is Nothing Then pReceivedFrom = ""
            If pReceivedFrom.Length = 0 Then
                fl.SOURCECOMPANYTYPE = pSourcecompanytype
                fl.SOURCECOMPANY = pSourcecompany
                Dim oComp As New WMS.Logic.Company(pConsignee, pSourcecompany, pSourcecompanytype)
                fl.ReceivedFrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pReceivedFrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                fl.ReceivedFrom = pReceivedFrom
                fl.SOURCECOMPANYTYPE = pSourcecompanytype
                fl.SOURCECOMPANY = pSourcecompany
            End If
        End If

        exist = WMS.Logic.Company.Exists(fl.CONSIGNEE, pTargetcompany, pTargetcompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If pShipTo Is Nothing Then pShipTo = ""
            If pShipTo.Length = 0 Then
                fl.TARGETCOMPANYTYPE = pTargetcompanytype
                fl.TARGETCOMPANY = pTargetcompany
                Dim oComp As New WMS.Logic.Company(pConsignee, pTargetcompany, pTargetcompanytype)
                fl.ShipTo = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pShipTo) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                fl.ShipTo = pShipTo
                fl.TARGETCOMPANYTYPE = pTargetcompanytype
                fl.TARGETCOMPANY = pTargetcompany
            End If
        End If

        If pStaginglane = "" Or pStagingWarehousearea = "" Then
            fl.STAGINGLANE = pStaginglane
            fl.STAGINGWAREHOUSEAREA = pStagingWarehousearea
        Else
            exist = WMS.Logic.Location.Exists(pStaginglane, pStagingWarehousearea)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            Else
                fl.STAGINGLANE = pStaginglane
                fl.STAGINGWAREHOUSEAREA = pStagingWarehousearea
            End If
        End If
        'Added for RWMS-2138(RWMS-2017) Start
        fl.ORDERTYPE = pOrdertype
        'Added for RWMS-2138(RWMS-2017) End
        fl.REFERENCEORD = pReferenceord
        fl.STATUS = WMS.Lib.Statuses.Flowthrough.STATUSNEW
        fl.STATUSDATE = pStatusdate
        fl.NOTES = pNotes
        fl.CREATEDATE = DateTime.Now()
        fl.SCHEDULEDARRIVALDATE = pScheduledarrivaldate
        fl.REQUESTEDDELIVERYDATE = pRequesteddeliverydate
        fl.SCHEDULEDDELIVERYDATE = pScheduleddeliverydate
        fl.SHIPPEDDATE = pShippeddate
        fl.SHIPMENT = pShipment
        fl.STOPNUMBER = pStopnumber
        fl.LOADINGSEQ = pLoadingseq
        fl.ROUTINGSET = pRoutingset
        fl.ROUTE = pRoute
        fl.DELIVERYSTATUS = pDeliverystatus
        fl.POD = pPod
        fl.ORDERPRIORITY = pOrderpriority

        fl.ADDDATE = DateTime.Now
        fl.ADDUSER = pAdduser
        fl.EDITDATE = DateTime.Now
        fl.EDITUSER = pEdituser

        SQL = "INSERT INTO FLOWTHROUGHHEADER(CONSIGNEE, FLOWTHROUGH, ORDERTYPE, REFERENCEORD, SOURCECOMPANY, SOURCECOMPANYTYPE, TARGETCOMPANY, " & _
                                            "TARGETCOMPANYTYPE, STATUS, STATUSDATE, NOTES, CREATEDATE, SCHEDULEDARRIVALDATE, REQUESTEDDELIVERYDATE, " & _
                                            "SCHEDULEDDELIVERYDATE, SHIPPEDDATE, STAGINGLANE, STAGINGWAREHOUSEAREA, SHIPMENT, STOPNUMBER, LOADINGSEQ, ROUTINGSET, ROUTE, DELIVERYSTATUS, " & _
                                            "POD, ORDERPRIORITY, RECEIVEDFROM, SHIPTO, ADDDATE, ADDUSER, EDITDATE, EDITUSER) "
        SQL += "VALUES(" & Made4Net.Shared.Util.FormatField(fl.CONSIGNEE) & "," & Made4Net.Shared.Util.FormatField(fl.FLOWTHROUGH) & "," & Made4Net.Shared.Util.FormatField(fl.ORDERTYPE) & "," & Made4Net.Shared.Util.FormatField(fl.REFERENCEORD) & "," & Made4Net.Shared.Util.FormatField(fl.SOURCECOMPANY) & "," & Made4Net.Shared.Util.FormatField(fl.SOURCECOMPANYTYPE) & "," & Made4Net.Shared.Util.FormatField(fl.TARGETCOMPANY) & "," & _
                           Made4Net.Shared.Util.FormatField(fl.TARGETCOMPANYTYPE) & "," & Made4Net.Shared.Util.FormatField(fl.STATUS) & "," & Made4Net.Shared.Util.FormatField(fl.STATUSDATE) & "," & Made4Net.Shared.Util.FormatField(fl.NOTES) & "," & Made4Net.Shared.Util.FormatField(fl.CREATEDATE) & "," & Made4Net.Shared.Util.FormatField(fl.SCHEDULEDARRIVALDATE) & "," & Made4Net.Shared.Util.FormatField(fl.REQUESTEDDELIVERYDATE) & "," & _
                           Made4Net.Shared.Util.FormatField(fl.SCHEDULEDDELIVERYDATE) & "," & Made4Net.Shared.Util.FormatField(fl.SHIPPEDDATE) & "," & Made4Net.Shared.Util.FormatField(fl.STAGINGLANE) & "," & Made4Net.Shared.Util.FormatField(fl.STAGINGWAREHOUSEAREA) & "," & Made4Net.Shared.Util.FormatField(fl.SHIPMENT) & "," & Made4Net.Shared.Util.FormatField(fl.STOPNUMBER) & "," & Made4Net.Shared.Util.FormatField(fl.LOADINGSEQ) & "," & Made4Net.Shared.Util.FormatField(fl.ROUTINGSET) & "," & Made4Net.Shared.Util.FormatField(fl.ROUTE) & "," & Made4Net.Shared.Util.FormatField(fl.DELIVERYSTATUS) & "," & _
                           Made4Net.Shared.Util.FormatField(fl.POD) & "," & Made4Net.Shared.Util.FormatField(fl.ORDERPRIORITY) & "," & Made4Net.Shared.Util.FormatField(fl.ReceivedFrom) & "," & Made4Net.Shared.Util.FormatField(fl.ShipTo) & "," & Made4Net.Shared.Util.FormatField(fl.ADDDATE) & "," & Made4Net.Shared.Util.FormatField(fl.ADDUSER) & "," & Made4Net.Shared.Util.FormatField(fl.EDITDATE) & "," & Made4Net.Shared.Util.FormatField(fl.EDITUSER) & ")"

        DataInterface.RunSQL(SQL)

    End Sub



    Public Sub Edit(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal pOrdertype As String, ByVal pReferenceord As String, _
                         ByVal pSourcecompany As String, ByVal pSourcecompanytype As String, ByVal pTargetcompany As String, ByVal pTargetcompanytype As String, _
                         ByVal pStatus As String, ByVal pStatusdate As DateTime, ByVal pNotes As String, _
                         ByVal pCreatedate As DateTime, ByVal pScheduledarrivaldate As DateTime, ByVal pRequesteddeliverydate As DateTime, _
                         ByVal pScheduleddeliverydate As DateTime, ByVal pShippeddate As DateTime, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, _
                         ByVal pShipment As String, ByVal pStopnumber As String, ByVal pLoadingseq As String, ByVal pRoutingset As String, _
                         ByVal pRoute As String, ByVal pDeliverystatus As String, ByVal pPod As String, ByVal pOrderpriority As String, _
                         ByVal pShipTo As String, ByVal pReceivedFrom As String, ByVal pEditdate As DateTime, ByVal pEdituser As String)

        Dim exist As Boolean = True
        Dim SQL As String = String.Empty

        If pStaginglane Is Nothing Then
            pStaginglane = ""
        End If

        Dim fl As New WMS.Logic.Flowthrough(pConsignee, pFlowthrough)

        fl.CONSIGNEE = pConsignee
        fl.FLOWTHROUGH = pFlowthrough

        'Commented for RWMS-2138(RWMS-2017) Start
        'fl.ORDERTYPE = "CUST"
        'Commented for RWMS-2138(RWMS-2017) End

        exist = WMS.Logic.Company.Exists(fl.CONSIGNEE, pSourcecompany, pSourcecompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid source company", "Can't create order.Invalid source company")
            Throw m4nEx
        Else
            If pReceivedFrom.Length = 0 Then
                fl.SOURCECOMPANYTYPE = pSourcecompanytype
                fl.SOURCECOMPANY = pSourcecompany
                Dim oComp As New WMS.Logic.Company(pConsignee, pSourcecompany, pSourcecompanytype)
                fl.ReceivedFrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pReceivedFrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                fl.ReceivedFrom = pReceivedFrom
                fl.SOURCECOMPANYTYPE = pSourcecompanytype
                fl.SOURCECOMPANY = pSourcecompany
            End If
        End If

        exist = WMS.Logic.Company.Exists(fl.CONSIGNEE, pTargetcompany, pTargetcompanytype)
        If Not exist Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid target company", "Can't create order.Invalid target company")
            Throw m4nEx
        Else
            If pShipTo Is Nothing Then pShipTo = ""
            If pShipTo.Length = 0 Then
                fl.TARGETCOMPANYTYPE = pTargetcompanytype
                fl.TARGETCOMPANY = pTargetcompany
                Dim oComp As New WMS.Logic.Company(pConsignee, pTargetcompany, pTargetcompanytype)
                fl.ShipTo = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pShipTo) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                fl.ShipTo = pShipTo
                fl.TARGETCOMPANYTYPE = pTargetcompanytype
                fl.TARGETCOMPANY = pTargetcompany
            End If
        End If


        exist = WMS.Logic.Location.Exists(pStaginglane, pStagingwarehousearea)
        If pStaginglane = "" Or pStagingwarehousearea = "" Then
            fl.STAGINGLANE = pStaginglane
            fl.STAGINGWAREHOUSEAREA = pStagingwarehousearea
        Else
            exist = WMS.Logic.Location.Exists(pStaginglane, pStagingwarehousearea)
            If Not exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid Staging Lane", "Can't create order.Invalid Staging Lane")
                Throw m4nEx
            Else
                fl.STAGINGLANE = pStaginglane
                fl.STAGINGWAREHOUSEAREA = pStagingwarehousearea
            End If
        End If

        'Added for RWMS-2138(RWMS-2017) Start
        fl.ORDERTYPE = pOrdertype
        'Added for RWMS-2138(RWMS-2017) End

        fl.REFERENCEORD = pReferenceord
        fl.STATUS = pStatus
        fl.STATUSDATE = pStatusdate
        fl.NOTES = pNotes
        fl.CREATEDATE = DateTime.Now()
        fl.SCHEDULEDARRIVALDATE = pScheduledarrivaldate
        fl.REQUESTEDDELIVERYDATE = pRequesteddeliverydate
        fl.SCHEDULEDDELIVERYDATE = pScheduleddeliverydate
        fl.SHIPPEDDATE = pShippeddate
        fl.SHIPMENT = pShipment
        fl.STOPNUMBER = pStopnumber
        fl.LOADINGSEQ = pLoadingseq
        fl.ROUTINGSET = pRoutingset
        fl.ROUTE = pRoute
        fl.DELIVERYSTATUS = pDeliverystatus
        fl.POD = pPod
        fl.ORDERPRIORITY = pOrderpriority

        fl.EDITDATE = DateTime.Now
        fl.EDITUSER = Common.GetCurrentUser

        SQL = String.Format("UPDATE FLOWTHROUGHHEADER " & _
                            "SET ORDERTYPE ={0}, REFERENCEORD ={1}, SOURCECOMPANY ={2}, SOURCECOMPANYTYPE ={3}, TARGETCOMPANY ={4}, " & _
                                " TARGETCOMPANYTYPE ={5}, STATUS ={6}, STATUSDATE ={7}, NOTES ={8}, CREATEDATE ={9}, SCHEDULEDARRIVALDATE ={10}, REQUESTEDDELIVERYDATE ={11}, " & _
                                " SCHEDULEDDELIVERYDATE ={12}, SHIPPEDDATE ={13}, STAGINGLANE ={14},STAGINGWAREHOUSEAREA={29}, SHIPMENT ={15}, STOPNUMBER ={16}, LOADINGSEQ ={17}, ROUTINGSET ={18}, ROUTE ={19}, " & _
                                " DELIVERYSTATUS ={20}, POD ={21}, ORDERPRIORITY ={22}, RECEIVEDFROM={27}, SHIPTO={28}, EDITDATE ={23}, EDITUSER = {24}" & _
                            " WHERE CONSIGNEE ={25} AND FLOWTHROUGH ={26}", _
                            Made4Net.Shared.Util.FormatField(fl.ORDERTYPE), Made4Net.Shared.Util.FormatField(fl.REFERENCEORD), Made4Net.Shared.Util.FormatField(fl.SOURCECOMPANY), Made4Net.Shared.Util.FormatField(fl.SOURCECOMPANYTYPE), Made4Net.Shared.Util.FormatField(fl.TARGETCOMPANY), Made4Net.Shared.Util.FormatField(fl.TARGETCOMPANYTYPE), Made4Net.Shared.Util.FormatField(fl.STATUS), Made4Net.Shared.Util.FormatField(fl.STATUSDATE), _
                            Made4Net.Shared.Util.FormatField(fl.NOTES), Made4Net.Shared.Util.FormatField(fl.CREATEDATE), Made4Net.Shared.Util.FormatField(fl.SCHEDULEDARRIVALDATE), Made4Net.Shared.Util.FormatField(fl.REQUESTEDDELIVERYDATE), Made4Net.Shared.Util.FormatField(fl.SCHEDULEDDELIVERYDATE), Made4Net.Shared.Util.FormatField(fl.SHIPPEDDATE), Made4Net.Shared.Util.FormatField(fl.STAGINGLANE), Made4Net.Shared.Util.FormatField(fl.SHIPMENT), _
                            Made4Net.Shared.Util.FormatField(fl.STOPNUMBER), Made4Net.Shared.Util.FormatField(fl.LOADINGSEQ), Made4Net.Shared.Util.FormatField(fl.ROUTINGSET), Made4Net.Shared.Util.FormatField(fl.ROUTE), Made4Net.Shared.Util.FormatField(fl.DELIVERYSTATUS), Made4Net.Shared.Util.FormatField(fl.POD), Made4Net.Shared.Util.FormatField(fl.ORDERPRIORITY), Made4Net.Shared.Util.FormatField(fl.EDITDATE), Made4Net.Shared.Util.FormatField(fl.EDITUSER), _
                            Made4Net.Shared.Util.FormatField(fl.CONSIGNEE), Made4Net.Shared.Util.FormatField(fl.FLOWTHROUGH), Made4Net.Shared.Util.FormatField(fl.ReceivedFrom), Made4Net.Shared.Util.FormatField(fl.ShipTo), Made4Net.Shared.Util.FormatField(fl.STAGINGWAREHOUSEAREA))

        DataInterface.RunSQL(SQL)

    End Sub

    'Added for RWMS-2138(RWMS-2017) Start
    Public Sub UpdateFields(ByVal pConsignee As String, ByVal pFlowthrough As String, ByVal SOURCEREF As String, ByVal TARGETREF As String)


        Dim Sql As String = String.Format("UPDATE FLOWTHROUGHHEADER " & _
                            "SET SOURCEREF ={0}, TARGETREF ={1}, EDITDATE =getdate(), EDITUSER = {2}" & _
                            " WHERE CONSIGNEE ={3} AND FLOWTHROUGH ={4}", _
                            Made4Net.Shared.Util.FormatField(SOURCEREF), Made4Net.Shared.Util.FormatField(TARGETREF), Made4Net.Shared.Util.FormatField(WMS.Logic.GetCurrentUser), _
                            Made4Net.Shared.Util.FormatField(pConsignee), Made4Net.Shared.Util.FormatField(pFlowthrough))

        DataInterface.RunSQL(Sql)

    End Sub
    'Added for RWMS-2138(RWMS-2017) End
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterFlowtrough_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterFlowtrough.CreatedChildControls
        With TEMasterFlowtrough.ActionBar
            With .Button("Save")
                If TEMasterFlowtrough.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Flowtrough"
                    .CommandName = "UPDATE"
                Else
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Flowtrough"
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.Flowthrough"
                    .CommandName = "CREATE"
                End If
            End With
            .AddSpacer()
            .AddExecButton("AssignSL", "Assign Orders to SL", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AssignSL")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Flowtrough"
                .CommandName = "AssignSL"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to assign the orders to Staging Lane?"
            End With
            .AddSpacer()
            .AddExecButton("canceloutbound", "Cancel Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            With .Button("canceloutbound")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Flowthrough"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Flowtrough"
                .CommandName = "CANCEL"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure you want to cancel the Flowthrough?", Nothing)
            End With

            .AddExecButton("CreateRCN", "Export To Receipt", Made4Net.WebControls.SkinManager.GetImageURL("Receipt"))
            With .Button("CreateRCN")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Flowthrough"

                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Flowtrough"

                .CommandName = "CREATERECEIPT"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            End With

            .AddExecButton("Ship", "Ship Flowthrough", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
            With .Button("Ship")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Flowthrough"
                .CommandName = "SHIP"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure you want to ship the selected Flowthrough?", Nothing)
            End With

            .AddExecButton("PrintSH", "Print Freight Manifest", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            With .Button("PrintSH")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Flowthrough"
                .CommandName = "PrintSH"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

        End With
    End Sub

    Private Sub TEFlowtroughDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEFlowtroughDetail.CreatedChildControls
        With TEFlowtroughDetail
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Flowthrough"
                If TEFlowtroughDetail.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "INSERTLINE"
                ElseIf TEFlowtroughDetail.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "EDITLINE"
                End If
            End With
        End With
    End Sub

End Class