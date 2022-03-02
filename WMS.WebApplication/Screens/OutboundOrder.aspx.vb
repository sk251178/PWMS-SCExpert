Imports WMS.Logic
Imports RWMS.Logic

Public Class OutboundOrder
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterOutboundOrders As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEOutboundOrderLines As Made4Net.WebControls.TableEditor
    Protected WithEvents TEOrderPicks As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEOrderException As Made4Net.WebControls.TableEditor
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEContactDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEHUTrans As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEOrderLoads As Made4Net.WebControls.TableEditor
    Protected WithEvents DC5 As Made4Net.WebControls.DataConnector
    Protected WithEvents TERouteStop As Made4Net.WebControls.TableEditor
    Protected WithEvents DC6 As Made4Net.WebControls.DataConnector

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
        Dim msg As String
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "editroutestop"

                dr = ds.Tables(0).Rows(0)
                Dim Sql As String
                Sql = " select COUNT(1) from outboundorheader where CONSIGNEE='{0}' and ORDERID='{1}' and STATUS<>'NEW'"
                Sql = String.Format(Sql, dr("CONSIGNEE"), dr("ORDERID"))

                Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                If Sql <> "0" Then
                    msg = "Route and Stop cannot be modified, order status is not new."
                    Throw New Made4Net.Shared.M4NException(New Exception, msg, msg)
                Else
                    Dim OutboundOrdRouteStop As New RWMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), "Route", True)

                    OutboundOrdRouteStop.EditRouteStop(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERID")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STOPNUMBER")), OutboundOrdRouteStop.EDITUSER)

                End If

            Case "multieditroutestop"

                Dim Sql As String
                Sql = " select COUNT(1) from outboundorheader where CONSIGNEE='{0}' and ORDERID='{1}' and STATUS<>'NEW'"
                Sql = String.Format(Sql, ds.Tables(0).Rows(0)("CONSIGNEE"), ds.Tables(0).Rows(0)("ORDERID"))

                Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                If Sql <> "0" Then
                    msg = "Route and Stop cannot be modified, order status is not new."
                    Throw New Made4Net.Shared.M4NException(New Exception, msg, msg)
                Else
                    For Each dr1 As DataRow In ds.Tables(0).Rows
                        Dim OutboundOrdRouteStop As New RWMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr1("CONSIGNEE"), dr1("ORDERID"), "Route", True)

                        OutboundOrdRouteStop.EditRouteStop(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr1("CONSIGNEE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr1("ORDERID")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr1("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr1("STOPNUMBER")), OutboundOrdRouteStop.EDITUSER)

                    Next
                End If

            Case "editorderdetail"
                dr = ds.Tables(0).Rows(0)
                Dim Sql As String
                'Commented for RWMS-1246/RWMS-1334
                'Sql = " select COUNT(1) from PICKDETAIL where CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE= '{2}'"
                'Sql = String.Format(Sql, dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                'End Commented for RWMS-1246/RWMS-1334
                'RWMS-1246/RWMS-1334
                Sql = String.Format("select COUNT(1) from PICKDETAIL where CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE= '{2}' and STATUS in ('PLANNED','RELEASED','COMPLETE')", dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
                'End RWMS-1246/RWMS-1334
                If Sql <> "0" Then
                    msg = "Cannot edit line - there is an open picklist for the line"
                    Throw New Made4Net.Shared.M4NException(New Exception, msg, msg)
                End If
                '
                Sql = "select COUNT(1) from vEditOrderDetail where CONSIGNEE='{0}' and ORDERID='{1}' "
                Sql = String.Format(Sql, dr("CONSIGNEE"), dr("ORDERID"))

                Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                If Sql <> "0" Then
                    msg = "Cannot edit line - order status is incorrect"
                    Throw New Made4Net.Shared.M4NException(New Exception, msg, msg)
                End If

                Dim OutboundOrderHeader As New WMS.Logic.OutboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)

            Case "createnewcontact"
                dr = ds.Tables(0).Rows(0)
                Dim oContact As WMS.Logic.Contact = New WMS.Logic.Contact
                Dim oOutOrd As WMS.Logic.OutboundOrderHeader = New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID")

                oContact.CONTACT1EMAIL = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1EMAIL"))
                oContact.CITY = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1EMAIL"))
                oContact.CONTACT1FAX = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1FAX"))
                oContact.CONTACT1NAME = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1NAME"))
                oContact.CONTACT1PHONE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1PHONE"))
                oContact.CONTACT2EMAIL = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2EMAIL"))
                oContact.CONTACT2FAX = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2FAX"))
                oContact.CONTACT2NAME = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2NAME"))
                oContact.CONTACT2PHONE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2PHONE"))
                oContact.STATE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATE"))
                oContact.STREET1 = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STREET1"))
                oContact.STREET2 = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STREET2"))
                oContact.ZIP = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ZIP"))
                oContact.Save(UserId)

                oOutOrd.SetShipTo(oContact.CONTACTID)
            Case "cancellineexception"
                'RWMS-2581 RWMS-2554
                Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
                If Not wmsrdtLogger Is Nothing Then
                    'wmsrdtLogger.Write(String.Format(" OutboundOrderDetail.Complete - outboundorder detail SQL query:{0}", Sql))
                    wmsrdtLogger.Write(" Cancel Order line exception - START")
                End If
                'RWMS-2581 RWMS-2554 END

                Dim outordet As WMS.Logic.OutboundOrderHeader.OutboundOrderDetail
                Dim ORDERID, Consignee As String
                For Each dr In ds.Tables(0).Rows

                    'RWMS-2581 RWMS-2554
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" OrderId:{0}, Orderline:{1}", dr("ORDERID"), dr("ORDERLINE")))
                    End If
                    'RWMS-2581 RWMS-2554 END

                    outordet = New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                    outordet.CancelExceptions(Common.GetCurrentUser)

                    WebAppUtil.UpdateShipmentDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), outordet.QTYMODIFIED)
                    ' WebAppUtil.CancelOrder(dr("CONSIGNEE"), dr("ORDERID"))
                    ORDERID = dr("ORDERID")
                    Consignee = dr("CONSIGNEE")
                Next
                WebAppUtil.CompleteOrder(Consignee, ORDERID)

                'Strart RWMS-2247 RWMS-2014
                For Each dr In ds.Tables(0).Rows
                    outordet = New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                    WebAppUtil.UpdateShipmentToLoaded(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), UserId)
                Next
                'End RWMS-2247 RWMS-2014

                'RWMS-2581 RWMS-2554
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(" Cancel Order line exception - END")
                End If
                'RWMS-2581 RWMS-2554 END
            Case "cancelexception"
                'RWMS-2581 RWMS-2554
                Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(" Cancel Order exception - START")
                End If
                'RWMS-2581 RWMS-2554 END

                'Dim outorheader As OutboundOrderHeader
                For Each dr In ds.Tables(0).Rows

                    'RWMS-2581 RWMS-2554
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" OrderId:{0}", dr("ORDERID")))
                    End If
                    'RWMS-2581 RWMS-2554 END

                    WebAppUtil.processOrderExceptionLines(dr("CONSIGNEE"), dr("ORDERID"))
                    WebAppUtil.CompleteOrder(dr("CONSIGNEE"), dr("ORDERID"))
                    'outorheader = New OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                    'outorheader.CancelExceptions(Common.GetCurrentUser)
                Next

                'RWMS-2581 RWMS-2554
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(" Cancel Order exception - END")
                End If
                'RWMS-2581 RWMS-2554 END
            Case "addhu"
                dr = ds.Tables(0).Rows(0)
                Dim cons, ordid As String
                cons = Convert.ToString(dr("consignee")).Split(",")(0)
                ordid = Convert.ToString(dr("orderid")).Split(",")(0)
                Dim oOrd As New WMS.Logic.OutboundOrderHeader(cons, ordid)
                oOrd.AddHandlingUnit(dr("HUTYPE"), dr("HUQTY"), UserId)
            Case "edithu"
                dr = ds.Tables(0).Rows(0)
                Dim cons, ordid As String
                cons = Convert.ToString(dr("consignee")).Split(",")(0)
                ordid = Convert.ToString(dr("orderid")).Split(",")(0)
                Dim oOrd As New WMS.Logic.OutboundOrderHeader(cons, ordid)
                oOrd.UpdateHandlingUnit(dr("TRANSACTIONID"), dr("HUQTY"), UserId)
            Case "assignsl"
                If ds.Tables(0).Rows.Count = 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "No Rows Selected", "No Rows Selected")
                End If
                Dim SLSetter As New StagingLaneAssignmentSetter
                Dim ret As Boolean = True
                Dim outOrders As String = ""
                For Each dr In ds.Tables(0).Rows
                    If Not SLSetter.SetDocumentStagingLane(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, dr("CONSIGNEE"), dr("ORDERID")) Then
                        ret = False
                        outOrders += dr("ORDERID").ToString() & ","
                    End If
                Next
                If ret Then
                    Message = ts.Translate("Orders assigned to staging lanes")
                Else
                    Message = ts.Translate("Not all order were assigned to staging lane")
                    'Dim msg As String = ts.Translate("Not all orders were assigned to staging lane. There was a problem with the following orders: ") & outOrders.TrimEnd(",")
                    'Message = ts.Translate("Not all orders were assigned to staging lane. There was a problem with the following orders: ") & outOrders.TrimEnd(",")
                    'Throw New Made4Net.Shared.M4NException(New Exception, msg, msg)
                End If
            Case "unpick"
                Dim outorheader As WMS.Logic.OutboundOrderHeader
                For Each dr In ds.Tables(0).Rows
                    outorheader = New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                    outorheader.UnPick(dr("orderline"), dr("loadid"), dr("units"), Common.GetCurrentUser)
                Next
                Message = ts.Translate("Orders loads unpicked")
            Case "setlocation"
                Dim oLoad As WMS.Logic.Load
                For Each dr In ds.Tables(0).Rows
                    oLoad = New WMS.Logic.Load(dr("loadid"), True)
                    oLoad.Put(dr("location"), oLoad.WAREHOUSEAREA, "", UserId)
                Next
                Message = ts.Translate("Location set for selected loads.")
            Case "approvepicks"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))

                    Dim pd As New WMS.Logic.PicklistDetail(dr("PICKLIST"), dr("PICKLISTLINE"), True)

                    If pd.Status = "COMPLETE" Then Continue For
                    Dim oAttributeCollection As WMS.Logic.AttributesCollection
                    Dim UNITS As Decimal = 0
                    Try
                        UNITS = Math.Round(Decimal.Parse(dr("UNITS")), 2)
                        If UNITS < 0 Then Throw New Exception()
                    Catch ex As Exception
                        Throw New ApplicationException(("Invalid UNITS"))
                        Exit Sub
                    End Try

                    Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))
                    If UNITS <> 0 Then
                        OutboundOrder.MainValidation(dr, oSku)
                    End If

                    If pd.Quantity - pd.PickedQuantity > UNITS Then
                        OutboundOrder.PickShort(pd, pd.Quantity - pd.PickedQuantity, UNITS)
                    End If
                    Try
                        oAttributeCollection = WMS.Logic.SkuClass.ExtractPickingAttributes(oSku.SKUClass, dr)
                        Dim oPicking As WMS.Logic.Picking = New WMS.Logic.Picking
                        oPicking.PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection)
                    Catch ex As Made4Net.Shared.M4NException
                        Throw ex
                    Catch ex As Exception
                    End Try


                Next
                '   Dim PICK As New WMS.Logic.Picking(Sender, CommandName, XMLSchema, XMLData, Message)
            Case "planorder"

                For Each dr In ds.Tables(0).Rows

                    If String.IsNullOrEmpty(WMS.Logic.Load.DoesOrderOrShipmentHaveStagingLane(dr("ORDERID"))) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "19090", String.Format("Order {0} cannot be planned - no staging lane", dr("ORDERID")))
                    End If

                    Dim Sql As String



                    Sql = " select COUNT(1) from OUTBOUNDORHEADER where CONSIGNEE='{0}' and ORDERID='{1}' and STATUS= 'LOADING'"
                    Sql = String.Format(Sql, dr("CONSIGNEE"), dr("ORDERID"))

                    Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                    If Sql <> "0" Then

                        Sql = " update OUTBOUNDORHEADER "
                        Sql += " set STATUS ='PLANNED', EDITDATE = GETDATE(), EDITUSER = '{0}' "
                        Sql += " FROM         dbo.OUTBOUNDORHEADER AS oh "
                        Sql += " WHERE  oh.CONSIGNEE='{1}' AND oh.ORDERID='{2}'  and (oh.STATUS = 'LOADING')  "
                        Sql = String.Format(Sql, WMS.Logic.GetCurrentUser, dr("CONSIGNEE"), dr("ORDERID"))

                        Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                    End If

                Next
                Dim OutboundOrderHeader As New WMS.Logic.OutboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)
        End Select
    End Sub

    ' Added for RWMS-2309
    Public Shared Function DeriveStagingLane(ByVal dr As DataRow) As String
        Dim stagingLane As String = String.Empty
        ' Check if shipment is there
        Dim sql As String = "SELECT A.SHIPMENT, B.ORDERID, B.ORDERLINE, A.STAGINGLANE FROM SHIPMENT A (NOLOCK) INNER JOIN SHIPMENTDETAIL B (NOLOCK) ON A.SHIPMENT = B.SHIPMENT  AND B.ORDERID = '{0}'"
        Dim dt As DataTable = New DataTable()
        Made4Net.DataAccess.DataInterface.FillDataset(String.Format(sql, dr("ORDERID")), dt)
        If dt.Rows.Count > 0 Then
            Dim staginigLaneOnShipment As String = dt.Rows(0)("STAGINGLANE")
            If Not String.IsNullOrEmpty(staginigLaneOnShipment) Then
                stagingLane = staginigLaneOnShipment
            Else
                If Not String.IsNullOrEmpty(dr("STAGINGLANE")) Then
                    stagingLane = dr("STAGINGLANE")
                End If
            End If
        Else
            If Not String.IsNullOrEmpty(dr("STAGINGLANE")) Then
                stagingLane = dr("STAGINGLANE")
            End If
        End If
        Return stagingLane
    End Function
    ' RWMS-2309

    'Added for RWMS-323
    'approvepicks
    Public Shared Sub ApprovePicks(ByVal drPickDetail As DataRow)
        Dim oSku As WMS.Logic.SKU
        Dim weightNeeded As Integer = 0
        Dim attFromLoad As Integer = 0
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        Dim t As New Made4Net.Shared.Translation.Translator()
        Dim WEIGHT As Decimal = 0

        weightNeeded = 0
        Dim pd As New WMS.Logic.PicklistDetail(drPickDetail("PICKLIST"), drPickDetail("PICKLISTLINE"), True)

        If pd.Status = "COMPLETE" Then Exit Sub

        oSku = New WMS.Logic.SKU(drPickDetail("CONSIGNEE"), drPickDetail("SKU"))

        Dim ld As New WMS.Logic.Load(drPickDetail("fromload").ToString)
        Dim UNITS As Decimal = 0


        Try
            If IsDBNull(drPickDetail("UNITS")) Or Not IsNumeric(drPickDetail("UNITS")) Then
                Throw New ApplicationException(t.Translate("Invalid UNITS"))
            End If
            UNITS = Math.Round(Decimal.Parse(drPickDetail("UNITS")), 2)
            If UNITS < 0 Then Throw New Exception()

            UNITS = UNITS * oSku.ConvertToUnits(ld.LOADUOM)

        Catch ex As Exception
            ''comes from 'pick' weight or picklist screen
            attFromLoad = 1
            UNITS = drPickDetail("qty") - drPickDetail("pickedqty")
        End Try


        If UNITS <> "0" Then
            weightNeeded = PickDetailWeightValidation(drPickDetail, oSku, UNITS / oSku.ConvertToUnits(ld.LOADUOM), WEIGHT)
        End If

        'If pd.Quantity - pd.PickedQuantity > UNITS Then
        '    PickShort(pd, pd.Quantity - pd.PickedQuantity, UNITS)
        'End If

        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(drPickDetail("PICKLIST"))
        Dim oAttributeCollection As AttributesCollection
        If attFromLoad Then
            oAttributeCollection = ld.LoadAttributes.Attributes
        Else
            oAttributeCollection = WMS.Logic.SkuClass.ExtractPickingAttributes(oSku.SKUClass, drPickDetail)
        End If


        If weightNeeded = 1 Then
            oAttributeCollection.Item("WEIGHT") = WEIGHT.ToString
        End If

        Dim oPicking As Picking = New Picking
        oPicking.PickLine(oPickList, drPickDetail("PICKLISTLINE"), UNITS, UserId, oAttributeCollection)

        '   oSku = New WMS.Logic.SKU(drPickDetail("CONSIGNEE"), drPickDetail("SKU"))
        If weightNeeded = 1 Then
            'Commented for RWMS-323
            'OutboundOrder.InsertCasesWeight(drPickDetail)
            'End Commented for RWMS-323
            'Added for RWMS-323
            Throw New ApplicationException(t.Translate("Weight must be Entered"))
            'End Added for RWMS-323
        End If

    End Sub
    'End Added for RWMS-323

    <CLSCompliant(False)>
    Public Shared Sub PickShort(ByVal pd As WMS.Logic.PicklistDetail, ByVal fromQty As Decimal, ByVal toQTY As Decimal)
        Dim C As New WMS.Logic.Counting(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", False)

        C.CreateLocationCountJobs(pd.FromWarehousearea, "", pd.FromLocation, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", "", WMS.Logic.GetCurrentUser)

        Try
            Dim sql As String = String.Format("update tasks set PRIORITY=400 where TASKTYPE='LOCCNT' and COUNTID = '{0}' and FROMLOCATION='{1}' and FROMWAREHOUSEAREA='{2}'", C.COUNTID, C.LOCATION, C.WAREHOUSEAREA)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try

        sendPickShortMessageQ(pd, fromQty, toQTY)

    End Sub

    <CLSCompliant(False)>
    Public Shared Sub sendPickShortMessageQ(ByVal pd As WMS.Logic.PicklistDetail, ByVal fromQty As Decimal, ByVal toQTY As Decimal)

        'Commented for PWMS-756 and made it generic
        'Dim MSG As String = "SHORTPICK"
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SHORTPICK)
        aq.Add("ACTIVITYTYPE", WMS.Lib.TASKTYPE.SHORTPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")

        aq.Add("USERID", WMS.Logic.GetCurrentUser)

        aq.Add("DOCUMENT", pd.PickList)
        Dim strLines As String = "1"
        Try
            strLines = pd.PickListLine
        Catch ex As Exception
        End Try
        aq.Add("DOCUMENTLINE", strLines)

        aq.Add("CONSIGNEE", pd.Consignee)

        aq.Add("SKU", pd.SKU)

        aq.Add("FROMLOAD", pd.FromLoad)
        'aq.Add("TOLOAD", "") '??
        Dim L As New WMS.Logic.Load(pd.FromLoad)
        aq.Add("FROMSTATUS", L.STATUS) '??
        aq.Add("FROMLOC", pd.FromLocation)
        aq.Add("FROMQTY", fromQty)
        aq.Add("TOQTY", toQTY)

        'aq.Add("MHEID", Session("MHEID"))
        'aq.Add("MHType", Session("MHType"))

        aq.Add("FROMCONTAINER", pd.ToContainer)

        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        'aq.Add("TOLOC", "")
        'aq.Add("TOSTATUS", Session("CreateLoadStatus"))
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)

        'PWMS-756 and made it generic  for shortpick
        aq.Send(WMS.Lib.TASKTYPE.SHORTPICK)


    End Sub

    'Added for RWMS-323
    <CLSCompliant(False)>
    Public Shared Function PickDetailWeightValidation(ByVal dr As DataRow, ByVal oSku As WMS.Logic.SKU, ByVal units As Decimal, ByRef WEIGHT As Decimal) As Integer
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim countWEIGHT As Decimal


        If (weightNeeded(oSku)) Then
            WEIGHT = Math.Round(getWeight(dr("consignee"), dr("orderid"), dr("orderline"), countWEIGHT), 2)

            If WEIGHT > 0 Then

                Dim UNITSPERCASE As Decimal = units 'Integer.Parse(dr("UNITS")) '/ oSku.ConvertToUnits("CASE")
                If UNITSPERCASE <> countWEIGHT Then
                    Throw New ApplicationException(ts.Translate("need to enter case level weight for the pick"))
                    Exit Function
                Else

                    Dim weightPerCase As Decimal = Math.Round(WEIGHT / UNITSPERCASE, 2)

                    ' if validator is fall will throw exaption
                    VALIDATEWEIGHT(dr("CONSIGNEE"), dr("SKU"), weightPerCase)

                    updateFromLoadWeight(dr("fromload"), WEIGHT)
                End If
            Else
                Throw New ApplicationException(ts.Translate("please fill Units and Weight"))
                Exit Function
            End If
            Return 1
        End If
        Return 0
    End Function

    'dr("CONSIGNEE"), dr("SKU"), dr("orderid"), dr("orderline"), dr("PICKLIST"), dr("PICKLISTLINE")
    Public Shared Sub InsertCasesWeight(ByVal dr As DataRow)

        Dim sql As String
        Dim user As String = WMS.Logic.GetCurrentUser

        sql = "INSERT INTO LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) select TOLOAD,LoadUOM,ROW_NUMBER() OVER( ORDER BY TOLOAD )  + (select ISNULL(MAX(UOMNUM),0) from LOADDETWEIGHT where LOADID = TOLOAD),UOMWEIGHT, GETDATE(),'{2}' from vPickCompleteCaseWeight WHERE PICKLIST='{0}' AND PICKLISTLINE = '{1}'"

        sql = String.Format(sql, dr("PICKLIST"), dr("PICKLISTLINE"), user)

        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

        ' sql = "DELETE FROM ORDERLINECASEWEIGHT WHERE CONSIGNEE = '{0}' AND ORDERID='{1}' AND ORDERLINE = '{2}'"
        sql = "delete ORDERLINECASEWEIGHT FROM dbo.ORDERLINECASEWEIGHT AS w INNER JOIN dbo.PICKDETAIL AS p ON w.CONSIGNEE = p.CONSIGNEE AND w.ORDERID = p.ORDERID AND w.ORDERLINE = p.ORDERLINE WHERE (p.PICKLIST = '{0}') AND (p.PICKLISTLINE = '{1}')"
        sql = String.Format(sql, dr("PICKLIST"), dr("PICKLISTLINE"))

        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

    End Sub

    Public Shared Function getWeight(ByVal consignee As String, ByVal orderid As String, ByVal orderline As String, ByRef weightCount As Integer) As Decimal

        Dim SQL As String = String.Format("select count(1) weightCount, sum(UOMWEIGHT) sumUOMWEIGHT from ORDERLINECASEWEIGHT WHERE CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE='{2}'", consignee, orderid, orderline)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            weightCount = dt.Rows(0)("weightCount")
            Return dt.Rows(0)("sumUOMWEIGHT")
        Else
            weightCount = 0
            Return 0
        End If

    End Function
    'End Added for RWMS-323

    <CLSCompliant(False)>
    Public Shared Sub MainValidation(ByVal dr As DataRow, ByVal oSku As WMS.Logic.SKU)
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim UNITS As Decimal = 0
        Try
            UNITS = Math.Round(Decimal.Parse(dr("UNITS")), 2)
            If UNITS < 0 Then Throw New Exception()
        Catch ex As Exception
            Throw New ApplicationException(ts.Translate("Invalid UNITS"))
            Exit Sub
        End Try


        If (weightNeeded(oSku)) Then
            If Not IsDBNull(dr("WEIGHT")) Then

                Dim UNITSPERCASE As Decimal = Integer.Parse(dr("UNITS")) / oSku.ConvertToUnits("CASE")

                Dim weight As Decimal = Math.Round(Decimal.Parse(dr("WEIGHT")), 2)

                Dim weightPerCase As Decimal = Math.Round(weight / UNITSPERCASE, 2)

                ' if validator is fall will throw exaption
                VALIDATEWEIGHT(dr("CONSIGNEE"), dr("SKU"), weightPerCase)

                updateFromLoadWeight(dr("fromload"), weight)
            Else
                Throw New ApplicationException(ts.Translate("please fill Units and Weight"))
                Exit Sub
            End If
        End If
    End Sub

    Public Shared Sub updateFromLoadWeight(ByVal fromload As String, ByVal totalWeight As Decimal)
        Dim newWeight As Decimal
        Dim LD As New WMS.Logic.Load(fromload)
        Try

            If Decimal.Parse(LD.Weight - totalWeight) < 0 Then
                newWeight = 0
            Else
                newWeight = Decimal.Parse(LD.Weight - totalWeight)
            End If

            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", newWeight, LD.LOADID)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        Catch ex As Exception
            ' updateFromLoadWeight = ("can't update flomload weight")
        End Try
    End Sub

    <CLSCompliant(False)>
    Public Shared Function weightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        If Not pSKU.SKUClass Is Nothing Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = "WEIGHT" AndAlso _
                (oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Public Shared Function VALIDATEWEIGHT(ByVal consingee As String, ByVal sku As String, ByVal WEIGHT As Decimal) As Boolean
        Dim RETVAL As Boolean = True
        'added validation code for weight
        Dim oSku As New WMS.Logic.SKU(consingee, sku)
        Dim PICKOVERRIDEVALIDATOR As String
        PICKOVERRIDEVALIDATOR = getPICKOVERRIDEVALIDATOR(consingee, sku)

        'If weightNeeded(oSku) And Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then
        If Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then

            'New Validation with expression evaluation
            Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

            vals.Add("CONSIGNEE", consingee)
            vals.Add("SKU", sku)
            vals.Add("CASEWEIGHT", WEIGHT)

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            ' Dim statement As String = "[0];func:tmValidWeightPerCase(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                Throw New ApplicationException(ex.Message)
            End Try
            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                RETVAL = False
                Throw New ApplicationException(returnedResponse(1))
            End If
        End If
        Return RETVAL
    End Function


    Public Shared Function getPICKOVERRIDEVALIDATOR(ByVal oConsignee As String, ByVal oSku As String) As String
        Dim ret As String = String.Empty
        Dim objSku As WMS.Logic.SKU = New WMS.Logic.SKU(oConsignee, oSku)

        If Not objSku.SKUClass Is Nothing Then

            Dim objSkuClass As WMS.Logic.SkuClass = objSku.SKUClass

            Dim sql As String = String.Format("SELECT ISNULL(PICKOVERRIDEVALIDATOR, '') FROM SKUCLSLOADATT WHERE CLASSNAME = '{0}' AND ATTRIBUTENAME = 'WEIGHT'", objSkuClass.ClassName)
            Try
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
            Return ret
        End If
        Return ret
    End Function


#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterOutboundOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterOutboundOrders.CreatedChildControls
        With TEMasterOutboundOrders
            With .ActionBar
                .AddSpacer()
                .AddExecButton("AssignSL", "Assign Orders to SL", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("AssignSL")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    .CommandName = "AssignSL"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to assign the orders to Staging Lane?"
                End With
                .AddSpacer()
                .AddExecButton("Ship", "Ship Orders", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
                With .Button("Ship")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.OutboundOrderHeader"
                    .CommandName = "Ship"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to ship the selected orders?"
                End With
                .AddExecButton("PrintSH", "Print Outbound Delivery Note", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint")) ' RWMS-1198
                With .Button("PrintSH")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.OutboundOrderHeader"
                    .CommandName = "PrintSH"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
                .AddExecButton("planorder", "Plan Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("planorder")
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.OutboundOrderHeader"
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                End With
                .AddExecButton("softpaln", "Soft Plan", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
                With .Button("softpaln")
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.OutboundOrderHeader"
                End With
                .AddSpacer()
                .AddExecButton("CancelException", "Cancel Order Exception", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelException")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    .CommandName = "CancelException"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the order's exceptions?"
                End With

                .AddSpacer()
                'Added by priel
                .AddExecButton("canceloutbound", "Cancel Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
                With .Button("canceloutbound")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.OutboundOrderHeader"
                    .CommandName = "CancelOutbound"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the order?"
                End With


            End With
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.OutboundOrderHeader"
                If TEMasterOutboundOrders.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "NewOrder"
                ElseIf TEMasterOutboundOrders.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "EditOrder"
                End If
            End With

            With .ActionBar.Button("Delete")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.OutboundOrderHeader"
                .CommandName = "DeleteHeader"
            End With
        End With

    End Sub

    Private Sub TEMasterOutboundOrders_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterOutboundOrders.AfterItemCommand
        'If e.CommandName = "Ship" Then
        TEMasterOutboundOrders.RefreshData()
        'Added for RWMS-1289 and RWMS-980
        TEOrderPicks.RefreshData()
        'End for RWMS-1289 and RWMS-980

        'If TEMasterOutboundOrders.Data Is Nothing Then
        '    If TEMasterOutboundOrders.CurrentMode = Made4Net.WebControls.TableEditorMode.Grid Then
        '        TEMasterOutboundOrders.Restart()
        '    End If
        'ElseIf TEMasterOutboundOrders.Data.Rows.Count = 0 Then
        '    TEMasterOutboundOrders.Restart()
        'End If
        'End If
    End Sub

    Private Sub TEOutboundOrderLines_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEOutboundOrderLines.CreatedChildControls
        With TEOutboundOrderLines
            With .ActionBar.Button("Save")
                If TEOutboundOrderLines.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.OutboundOrderHeader"
                    .CommandName = "NewOrderDetail"
                ElseIf TEOutboundOrderLines.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.OutboundOrderHeader"
                    .CommandName = "EditOrderDetail"
				ElseIf TEOutboundOrderLines.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                    .ObjectDLL = "RWMS.Logic.dll"
                    .ObjectName = "RWMS.Logic.OutboundOrderHeader"
                    .CommandName = "EditOrderDetail"
                End If
            End With

            With .ActionBar.Button("Delete")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.OutboundOrderHeader"
                .CommandName = "DeleteLine"
            End With
        End With
    End Sub

    Private Sub TEMasterOutboundOrders_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterOutboundOrders.CreatedGrid
        TEMasterOutboundOrders.Grid.AddExecButton("printlabels", "Print Label", "WMS.Logic.dll", "WMS.Logic.OutboundOrderHeader", 4, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
    End Sub

    Private Sub TEOrderPicks_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEOrderPicks.CreatedChildControls
        'With TEOrderPicks
        '    With .ActionBar
        '        .AddSpacer()
        '        .AddExecButton("ApprovePicks", "Approve Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
        '        With .Button("ApprovePicks")
        '            .ObjectName = "WMS.WebApp.OutboundOrder"
        '            .ObjectDLL = "WMS.WebApp.dll"
        '            '.ObjectDLL = "WMS.Logic.dll"
        '            '.ObjectName = "WMS.Logic.Picking"
        '            .CommandName = "ApprovePicks"
        '            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '            .ConfirmRequired = True
        '            .ConfirmMessage = "Are you sure you want to pick the selected picks?"
        '        End With

        '        .AddExecButton("CancelPicks", "Cancel Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
        '        With .Button("CancelPicks")
        '            .ObjectDLL = "WMS.Logic.dll"
        '            .ObjectName = "WMS.Logic.Picklist"
        '            .CommandName = "CancelPicks"
        '            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '            .ConfirmRequired = True
        '            .ConfirmMessage = "Are you sure you want to cancel the selected picks?"
        '        End With

        '        .AddExecButton("unallocatePicks", "Unallocate Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
        '        With .Button("unallocatePicks")
        '            .ObjectDLL = "WMS.Logic.dll"
        '            .ObjectName = "WMS.Logic.Picklist"
        '            .CommandName = "unallocatePicks"
        '            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '            .ConfirmRequired = True
        '            .ConfirmMessage = "Are you sure you want to unallocated the selected picks?"
        '        End With

        '        '.AddExecButton("unPick", "Undo Pick", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
        '        'With .Button("unPick")
        '        '    .ObjectDLL = "WMS.Logic.dll"
        '        '    .ObjectName = "WMS.Logic.Picklist"
        '        '    .CommandName = "unpick"
        '        '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '        '    .ConfirmRequired = True
        '        '    .ConfirmMessage = "Are you sure you want to undo the selected picks?"
        '        'End With

        '    End With
        'End With
    End Sub

    Private Sub TEContactDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEContactDetail.CreatedChildControls
        With TEContactDetail
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.OutboundOrder"
                If TEContactDetail.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "createnewcontact"
                End If
            End With
        End With
    End Sub

    Private Sub TEMasterOutboundOrders_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterOutboundOrders.RecordSelected
        Dim tds As DataTable = TEMasterOutboundOrders.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Clear()
        vals.Add("ORDERID", tds.Rows(0)("ORDERID"))
        vals.Add("CONSIGNEE", tds.Rows(0)("CONSIGNEE"))
        TEContactDetail.PreDefinedValues = vals
        'Added for RWMS-1289 and RWMS-980
        TEOrderPicks.RefreshData()
        TEOrderPicks.RefreshGridInlineValues()
        'End   for RWMS-1289 and RWMS-980

        'TEHUTrans.PreDefinedValues = vals
    End Sub

    Private Sub TEContactDetail_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEContactDetail.AfterItemCommand
        If e.CommandName = "createnewcontact" Then
            TEMasterOutboundOrders.RefreshData()
            TEContactDetail.RefreshData()
        End If
    End Sub

    Private Sub TEOrderException_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEOrderException.CreatedChildControls
        With TEOrderException
            With .ActionBar
                .AddSpacer()
                .AddExecButton("CancelException", "Cancel Order Exception", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelException")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    .CommandName = "CancelLineException"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the order's exceptions?"
                End With
            End With
        End With
    End Sub

    Private Sub TEHUTrans_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEHUTrans.CreatedChildControls
        With TEHUTrans
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.OutboundOrder"
                If TEHUTrans.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addhu"
                Else
                    .CommandName = "edithu"
                End If
            End With
        End With
    End Sub

    Private Sub TEOrderLoads_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEOrderLoads.CreatedChildControls
        With TEOrderLoads.ActionBar
            '.AddExecButton("unPick", "Undo Pick", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
            'With .Button("unPick")
            '    .ObjectDLL = "WMS.WebApp.dll"
            '    .ObjectName = "WMS.WebApp.OutboundOrder"
            '    .CommandName = "unpick"
            '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            '    .ConfirmRequired = True
            '    .ConfirmMessage = "Are you sure you want to unpick the selected loads?"
            'End With
            With .Button("Save")
                If TEOrderLoads.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    .CommandName = "SetLocation"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End If
            End With
        End With
    End Sub

    Protected Sub TERouteStop_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERouteStop.CreatedChildControls
        With TERouteStop
            With .ActionBar.Button("Save")
                If TERouteStop.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    .CommandName = "EditRouteStop"
                ElseIf TERouteStop.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OutboundOrder"
                    .CommandName = "MultiEditRouteStop"
                End If
            End With
        End With
    End Sub
End Class