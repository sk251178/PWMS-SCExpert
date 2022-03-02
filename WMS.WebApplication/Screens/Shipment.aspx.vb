Imports WMS.Logic
Imports System.Data
Imports System.Xml
Imports Made4Net.DataAccess

Public Class Shipment
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterShipment As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEShipmentOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEAssignOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents TEHUTrans As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEYardEntry As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector2 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEShipmentLoads As Made4Net.WebControls.TableEditor
    'Added for RWMS-2343 RWMS-2314
    Protected WithEvents TEShipmentContainers As Made4Net.WebControls.TableEditor
    'End Added for RWMS-2343 RWMS-2314
    Protected WithEvents TEShipmentDetails As Made4Net.WebControls.TableEditor
    Protected WithEvents TEShipmentAssignDetails As Made4Net.WebControls.TableEditor

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        'RWMS-517 - Added the String.Empty
        Dim shipId As String = String.Empty
        Select Case CommandName.ToLower
            Case "addhu"
                dr = ds.Tables(0).Rows(0)
                shipId = Session("HUTRANSSHIPMENTID")
                Dim oShip As New WMS.Logic.Shipment(shipId)
                oShip.AddHandlingUnit(dr("HUTYPE"), dr("HUQTY"), UserId)
            Case "edithu"
                dr = ds.Tables(0).Rows(0)
                shipId = Session("HUTRANSSHIPMENTID")
                Dim oShip As New WMS.Logic.Shipment(shipId)
                oShip.UpdateHandlingUnit(dr("TRANSACTIONID"), dr("HUQTY"), UserId)

            Case "atdock"
                For Each dr In ds.Tables(0).Rows
                    'RWMS-517 - Added the below If condition
                    If shipId <> dr("SHIPMENT") Then
                        shipId = dr("SHIPMENT")
                        'Added for RWMS-2138(RWMS-2017) Start
                        Dim status As String = dr("STATUS")
                        If Not IsBelongFlowthroughToShipment(shipId) Then
                            SetAtDock(UserId, shipId, status)
                        Else
                            SetAtDock(UserId, shipId, status)
                            'Commented for RWMS-848 Start
                            'FlowthroughLoadDeliver(shipId)
                            'Commented for RWMS-848 End

                            'Added for RWMS-848 Start
                            Dim shouldCreateFlowThroughDeliveryTask As String = WMS.Logic.WarehouseParams.GetWarehouseParam("CrtFlowThroughTasks")
                            If shouldCreateFlowThroughDeliveryTask <> String.Empty AndAlso shouldCreateFlowThroughDeliveryTask = "1" Then
                                FlowthroughLoadDeliver(shipId)
                            End If
                            'Added for RWMS-848 End
                        End If
                        'Added for RWMS-2138(RWMS-2017) End

                        'Commented for RWMS-2340(RWMS-2339) Start
                        'Dim oShip As New WMS.Logic.Shipment(shipId)
                        'oShip.SetAtDock(UserId)
                        'Commented for RWMS-2340(RWMS-2339) Start

                        'Commented for RWMS-2138(RWMS-2017) Start
                        ''Added for RWMS-2340(RWMS-2339) Start
                        'Dim status As String = dr("STATUS")
                        'SetAtDock(UserId, shipId, status)
                        ''Added for RWMS-2340(RWMS-2339) End
                        'Commented for RWMS-2138(RWMS-2017) End

                    End If
                    'End
                Next
                'Added for PWMS-816
            Case "addyardentry"
                dr = ds.Tables(0).Rows(0)
                'shipId = Session("HUTRANSSHIPMENTID")
                'Dim oSh As New WMS.Logic.Shipment(shipId)
                Dim oSh As New WMS.Logic.Shipment(dr("SHIPMENT"))
                oSh.DOOR = dr("DOOR")
                oSh.Save(UserId)
                Dim yardentryId As String = Made4Net.Shared.getNextCounter("YARDENTRY")
                Dim ye As WMS.Logic.YardEntry = New WMS.Logic.YardEntry
                ye.Create(yardentryId, dr("CARRIER"), dr("VEHICLE"), dr("TRAILER"), dr("YARDLOCATION"), dr("SCHEDULEDATE"), UserId)
                ye.Schedule(UserId)
                oSh.AssignToYardEntry(yardentryId, UserId)
            Case "edityardentry"
                dr = ds.Tables(0).Rows(0)
                Dim oSh As New WMS.Logic.Shipment(dr("SHIPMENT"))
                oSh.DOOR = dr("DOOR")
                oSh.Save(UserId)
                Dim ye As WMS.Logic.YardEntry = New WMS.Logic.YardEntry(dr("YARDENTRYID"))
                ye.Update(dr("CARRIER"), dr("VEHICLE"), dr("TRAILER"), dr("YARDLOCATION"), ye.CHECKINDATE, ye.CHECKOUTDATE, dr("SCHEDULEDATE"), UserId)
                ye.Schedule(UserId)
                'Ended for PWMS-816
            Case "movetoshipment"
                dr = ds.Tables(0).Rows(0)
                shipId = dr("SHIPMENT")
                Dim oShip As New WMS.Logic.Shipment(shipId)
                For Each dr In ds.Tables(0).Rows
                    oShip.MoveDetailToShipment(dr("CONSIGNEE"), dr("DOCUMENTTYPE"), dr("ORDERID"), dr("ORDERLINE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("toshipment")), UserId)
                Next
            Case "movetoshipmentbycube"
                dr = ds.Tables(0).Rows(0)
                shipId = dr("SHIPMENT")
                Dim oShip As New WMS.Logic.Shipment(shipId)
                For Each dr In ds.Tables(0).Rows
                    MoveLinesToShipmentByCubeAndWeight(dr("CONSIGNEE"), dr("ORDERID"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("volume"), -1), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Weight"), -1), oShip, dr("toshipment"), WMS.Logic.GetCurrentUser)
                Next
            Case "assigntoshipmentbycube"

                XMLData = addXmlNode(XMLData, "ReversedLoadingSeq", "", "1")
                ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

                dr = ds.Tables(0).Rows(0)
                shipId = Session("HUTRANSSHIPMENTID")
                Dim oShip As New WMS.Logic.Shipment(shipId)
                For Each dr In ds.Tables(0).Rows
                    Dim newLoadingSeq As Int32
                    newLoadingSeq = GetLoadingSequenceByCompany(shipId, dr("company"))
                    If newLoadingSeq <= 0 Then
                        If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("loadingseq"), -1) > 0 Then
                            newLoadingSeq = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("loadingseq"), -1)
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ReversedLoadingSeq"), -1) > 0 Then
                            newLoadingSeq = GetLoadingSequence(oShip.SHIPMENT, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ReversedLoadingSeq"), -1))
                        End If
                    End If
                    If newLoadingSeq < 0 Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Loading sequence must be provided", "Loading sequence must be provided")
                    End If

                    'Commented for retrofit task PWMS-483,RWMS-645 Start
                    'If dr("TYPE") = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER Then
                    'Commented for retrofit task PWMS-483,RWMS-645 End
                    'Added for retrofit task PWMS-483,RWMS-645 Start
                    If dr("DOCUMENTTYPE") = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER Then
                        'Added for retrofit task PWMS-483,RWMS-645 End

                        AssignLinesToShipmentByCubeAndWeightBYPICKREGION(dr("CONSIGNEE"), dr("ORDERID"), dr("PICKREGION"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRANSPORTATIONCLASS")), newLoadingSeq, oShip, WMS.Logic.GetCurrentUser)
                    Else
                        'Commented for PWMS-586
                        'oShip.AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), 1, 1, newLoadingSeq, WMS.Logic.GetCurrentUser, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH)
                        'Dim SQL As String
                        'SQL = "INSERT INTO  SHIPMENTDETAIL (SHIPMENT, CONSIGNEE, ORDERID, ORDERLINE, UNITS, ADDDATE, ADDUSER, EDITDATE, EDITUSER, LOADINGSEQ) " _
                        '        & " SELECT '{0}',CONSIGNEE,FLOWTHROUGH,FLOWTHROUGHLINE,QTYMODIFIED,GETDATE(),'{1}',GETDATE(),'{1}',{2} FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '{3}' AND FLOWTHROUGH= '{4}' "
                        'SQL = String.Format(SQL, oShip.SHIPMENT, WMS.Logic.GetCurrentUser, 1, dr("CONSIGNEE"), dr("ORDERID"))
                        'Made4Net.DataAccess.DataInterface.RunSQL(SQL)
                        'End Commented for PWMS-586
                        'Added for PWMS-586
                        AssignFlowThroughLineToShipmentByCube(oShip, newLoadingSeq, dr("CONSIGNEE"), dr("ORDERID"))
                        'End Added for PWMS-586
                    End If

                Next
            Case "unassignfromshipmentbycube"
                dr = ds.Tables(0).Rows(0)
                shipId = Session("HUTRANSSHIPMENTID")
                Dim oShip As New WMS.Logic.Shipment(shipId)

                For Each dr In ds.Tables(0).Rows

                    'Added for RWMS-2342 RWMS-2334 - check whether the LOAD is in ShipmentLoads
                    Dim dtvShipmentLoads As New DataTable
                    Dim shipmentId As String = oShip.SHIPMENT
                    Dim shipmentOrder As String = dr("ORDERID")
                    'Dim shipmentOrderline As String = dr("ORDERLINE")

                    'Commented for RWMS-2824
                    'Dim SQLvShipLoad As String = String.Format("SELECT * FROM vshipmentloads WHERE SHIPMENT='{0}' and ORDERID='{1}'", shipmentId, shipmentOrder)
                    'Commented for RWMS-2824 END
                    'RWMS-2824
                    Dim SQLvShipLoad As String = String.Format("SELECT * FROM vshipmentloads WHERE SHIPMENT='{0}' and ORDERID='{1}' and LOADED=1", shipmentId, shipmentOrder)
                    'RWMS-2824 END

                    DataInterface.FillDataset(SQLvShipLoad, dtvShipmentLoads)

                    'RWMS-2824
                    Dim ldmsg As String = "Cannot unassign order. Shipment payloads needs to be unloaded - "
                    If dtvShipmentLoads.Rows.Count > 0 Then
                        Dim shiploadid As String
                        For i As Integer = 0 To 4
                            shiploadid += dtvShipmentLoads.Rows(i)("LOADID") + ","
                            If i = dtvShipmentLoads.Rows.Count - 1 Then
                                Exit For
                            End If
                        Next
                        'For Each paramLoad As DataRow In dtvShipmentLoads.Rows
                        '    shiploadid += paramLoad("LOADID") + ","
                        'Next
                        ldmsg = ldmsg + shiploadid.TrimEnd(",")
                    End If
                    'RWMS-2824 END

                    'Added for RWMS-2370 RWMS-2365 - check for 'UnAssignLoadedOrder'.
                    Dim dtUnAssignLoadedOrder As New DataTable
                    Dim SQLUnAssignLoadedOrder As String = "SELECT * FROM warehouseparams WHERE PARAMNAME='UnAssignLoadedOrder'"
                    DataInterface.FillDataset(SQLUnAssignLoadedOrder, dtUnAssignLoadedOrder)
                    If dtUnAssignLoadedOrder.Rows.Count > 0 Then
                        Dim unAssignLoadedOrder As String = dtUnAssignLoadedOrder.Rows(0)("PARAMVALUE")
                        If Not String.IsNullOrEmpty(unAssignLoadedOrder) Then
                            If Not unAssignLoadedOrder = "1" Then
                                If dtvShipmentLoads.Rows.Count > 0 Then
                                    'Commented for RWMS-2824
                                    'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                                    'Commented for RWMS-2824 END
                                    'RWMS-2824
                                    Throw New Made4Net.Shared.M4NException(New Exception, ldmsg, ldmsg)
                                    'RWMS-2824 END
                                End If
                            End If
                        Else
                            If dtvShipmentLoads.Rows.Count > 0 Then
                                'Commented for RWMS-2824
                                'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                                'Commented for RWMS-2824 END
                                'RWMS -2824
                                Throw New Made4Net.Shared.M4NException(New Exception, ldmsg, ldmsg)
                                'RWMS -2824 END
                            End If
                        End If
                    Else
                        If dtvShipmentLoads.Rows.Count > 0 Then
                            'Commented for RWMS-2824
                            'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                            'Commented for RWMS-2824 END
                            'RWMS -2824
                            Throw New Made4Net.Shared.M4NException(New Exception, ldmsg, ldmsg)
                            'RWMS -2824 END
                        End If
                    End If
                    'End Added for RWMS-2370 RWMS-2365

                    'Commented for RWMS-2370 RWMS-2365
                    'If dtvShipmentLoads.Rows.Count > 0 Then
                    '    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign order. Shipment payloads needs to be unloaded  ", "Cannot unassign order. Shipment payloads needs to be unloaded")
                    'End If
                    'End Commented for RWMS-2370 RWMS-2365

                    'End Added for RWMS-2342 RWMS-2334

                    If dr("DOCUMENTTYPE") = WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER Then
                        UnAssignLinesToShipmentByCubeAndWeight(dr("CONSIGNEE"), dr("ORDERID"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRANSPORTATIONCLASS")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Volume"), -1), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Weight"), -1), oShip, WMS.Logic.GetCurrentUser)
                    Else
                        oShip.DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), 1, WMS.Logic.GetCurrentUser, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH)
                        '
                        Dim SQL As String
                        SQL = "delete SHIPMENTDETAIL where SHIPMENT='{0}' and  CONSIGNEE='{1}' and ORDERID ='{2}'"
                        SQL = String.Format(SQL, oShip.SHIPMENT, dr("CONSIGNEE"), dr("ORDERID"))
                        Made4Net.DataAccess.DataInterface.RunSQL(SQL)
                    End If

                Next
            Case "completeloading"
                'Commented for RWMS-2014 (RWMS-705,RWMS-1710)

                'Commented for RWMS-1710 Start
                'For Each dr In ds.Tables(0).Rows

                ' Dim dt As New DataTable
                ' Dim SQL As String = "SELECT SUM(ISNULL(QTYPICKED,0)) AS QTYPICKED,SUM(ISNULL(QTYVERIFIED,0)) AS QTYVERIFIED,SUM(ISNULL(QTYPACKED,0)) AS QTYPACKED, " & _
                ' " SUM(ISNULL(QTYALLOCATED,0)) AS QTYALLOCATED,SUM(ISNULL(OD.QTYMODIFIED,0)) AS QTYMODIFIED, " & _
                ' " SUM(ISNULL(QTYALLOCATED,0)+ISNULL(QTYPICKED,0)+ISNULL(QTYSTAGED,0)+ISNULL(QTYVERIFIED,0)+ISNULL(QTYPACKED,0)+ISNULL(QTYLOADED,0)) AS SUMQTY," & _
                ' " SUM(ISNULL(ExceptionQty,0)) AS EXCEPTIONQTY FROM OUTBOUNDORDETAIL OD LEFT JOIN WAVEEXCEPTION WE ON OD.ORDERID=WE.ORDERID AND OD.CONSIGNEE=WE.CONSIGNEE " & _
                ' " INNER JOIN SHIPMENTDETAIL SH ON SH.ORDERID=OD.ORDERID AND SH.ORDERLINE=OD.ORDERLINE " & _
                ' " WHERE SH.SHIPMENT='" & dr("SHIPMENT") & "'"

                ' DataInterface.FillDataset(SQL, dt)
                ' If dt.Rows.Count > 0 Then
                ' If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("EXCEPTIONQTY")) <> 0 Then
                ' Throw New Made4Net.Shared.M4NException(New Exception, "Cancel exceptions first", "Cancel exceptions first")
                ' ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) <> 0 Then
                ' Throw New Made4Net.Shared.M4NException(New Exception, "There are still open picks", "There are still open picks")
                ' ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYMODIFIED")) <> Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("SUMQTY")) Then
                ' Throw New Made4Net.Shared.M4NException(New Exception, "Order not complete, Check picking and staging", "Order not complete, Check picking and staging")
                ' ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) <> 0 Then
                ' Throw New Made4Net.Shared.M4NException(New Exception, "Picking not complete", "Picking not complete")
                ' ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYVERIFIED")) <> 0 Then
                ' Throw New Made4Net.Shared.M4NException(New Exception, "Verification not complete", "Verification not complete")
                ' ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPACKED")) <> 0 Then
                ' Throw New Made4Net.Shared.M4NException(New Exception, "Packing not complete", "Packing not complete")
                ' End If
                ' End If

                ' LoadPallet(WMS.Logic.GetCurrentUser, dr("SHIPMENT"))

                'Next
                'Commented for RWMS-1710 End
                'Commented for RWMS-2014 End

                For Each dr In ds.Tables(0).Rows

                    'Added for RWMS-2447 - if all the orders are cancelled, display message and do nothing
                    Dim shipment As String = dr("SHIPMENT")
                    If isAllOrdersCancelled(shipment) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "All orders cancelled, nothing to load", "All orders cancelled, nothing to load")
                    End If
                    'End Added for RWMS-2447

                    Dim dt As New DataTable
                    'RWMS-2364 RWMS-2361 - added the following condition in one of the joins - AND WE.ORDERLINE=OD.ORDERLINE
                    Dim SQL As String = " SELECT SUM(ISNULL(QTYALLOCATED, 0)) AS QTYALLOCATED,SUM(ISNULL(QTYPICKED, 0)) AS QTYPICKED, " & _
                                        " SUM(ISNULL(QTYPACKED, 0)) AS QTYPACKED,SUM(ISNULL(QTYVERIFIED, 0)) AS QTYVERIFIED, " & _
                                        " SUM(ISNULL(OD.QTYMODIFIED, 0)) AS QTYMODIFIED,SUM(ISNULL(OD.QTYSTAGED, 0)) AS QTYSTAGED," & _
                                        " SUM(ISNULL(OD.QTYLOADED, 0)) AS QTYLOADED,SUM(ISNULL(OD.QTYSHIPPED, 0)) AS QTYSHIPPED, " & _
                                        " SUM(ISNULL(QTYSTAGED, 0) + ISNULL(QTYLOADED, 0) + ISNULL(OD.QTYSHIPPED, 0) + ISNULL(OD.QTYALLOCATED, 0) + ISNULL(OD.QTYPICKED, 0) + ISNULL(OD.QTYPACKED, 0) + ISNULL(OD.QTYVERIFIED, 0)) AS SUMQTY, " & _
                                        " SUM(ISNULL(ExceptionQty,0)) AS EXCEPTIONQTY " & _
                                        " FROM OUTBOUNDORDETAIL OD LEFT JOIN WAVEEXCEPTION WE ON OD.ORDERID=WE.ORDERID AND OD.CONSIGNEE=WE.CONSIGNEE AND WE.ORDERLINE=OD.ORDERLINE " & _
                                        " INNER JOIN SHIPMENTDETAIL SH ON SH.ORDERID = OD.ORDERID AND SH.ORDERLINE = OD.ORDERLINE AND SH.CONSIGNEE=OD.CONSIGNEE " & _
                                        " WHERE OD.QTYMODIFIED > 0 AND SH.SHIPMENT='" & dr("SHIPMENT") & "'"

                    DataInterface.FillDataset(SQL, dt)

                    If dt.Rows.Count > 0 Then
                        If (Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYMODIFIED")) > 0) And (Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("SUMQTY")) = 0) Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Some order lines have not been allocated", "Some order lines have not been allocated")
                            'Commented for RWMS-2340(RWMS-2339) Start
                            'ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPACKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYVERIFIED")) > 0 Then
                            'Commented for (RWMS-2340)RWMS-2339 End

                            'Added for (RWMS-2340)RWMS-2339 Start
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) > 0 Then
                            'Added for (RWMS-2340)RWMS-2339 End

                            Throw New Made4Net.Shared.M4NException(New Exception, "There are still open picks or inventory that has not been staged", "There are still open picks or inventory that has not been staged")
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("EXCEPTIONQTY")) <> 0 Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cancel exceptions first", "Cancel exceptions first")
                        End If
                    End If

                    LoadPallet(WMS.Logic.GetCurrentUser, dr("SHIPMENT"))

                Next
                'Added for RWMS-2014 End
            Case "ship"


                For Each dr In ds.Tables(0).Rows
                    shipId = dr("SHIPMENT")
                    'RWMS-2003 (RWMS-1710)
                    'Added for RWMS-1710 Start
                    ''Validation Rules for SHIPMENT
                    '1. If QtyModified = 0 then ignore those outboundordetails
                    '2. If Qtyallocated or qtypicked or qtypacked or qtyverified or qtystaged > 0 – If true, then display there are still open picks or inventory that has not been loaded
                    '3. If qtymodified > 0 AND (qtystaged + qtyloaded + qtyshipped + Qtyallocated + qtypicked + qtypacked + qtyverified = 0) display message "Some order lines have not been allocated"
                    Dim dt As New DataTable
                    'RWMS-2369 RWMS-2368 - Added the condition in the sql - AND WE.ORDERLINE=OD.ORDERLINE
                    Dim SQL As String = String.Format(" SELECT SUM(ISNULL(QTYALLOCATED, 0)) AS QTYALLOCATED,SUM(ISNULL(QTYPICKED, 0)) AS QTYPICKED, " & _
                                                      " SUM(ISNULL(QTYPACKED, 0)) AS QTYPACKED,SUM(ISNULL(QTYVERIFIED, 0)) AS QTYVERIFIED, " & _
                                                      " SUM(ISNULL(OD.QTYMODIFIED, 0)) AS QTYMODIFIED,SUM(ISNULL(OD.QTYSTAGED, 0)) AS QTYSTAGED, " & _
                                                      " SUM(ISNULL(OD.QTYLOADED, 0)) AS QTYLOADED,SUM(ISNULL(OD.QTYSHIPPED, 0)) AS QTYSHIPPED, " & _
                                                      " SUM(ISNULL(QTYSTAGED, 0) + ISNULL(QTYLOADED, 0) + ISNULL(OD.QTYSHIPPED, 0) + ISNULL(OD.QTYALLOCATED, 0) + ISNULL(OD.QTYPICKED, 0) + ISNULL(OD.QTYPACKED, 0) + ISNULL(OD.QTYVERIFIED, 0)) AS SUMQTY, " & _
                                                      " SUM(ISNULL(ExceptionQty,0)) AS EXCEPTIONQTY " & _
                                                      " FROM OUTBOUNDORDETAIL OD LEFT JOIN WAVEEXCEPTION WE ON OD.ORDERID=WE.ORDERID AND OD.CONSIGNEE=WE.CONSIGNEE AND WE.ORDERLINE=OD.ORDERLINE " & _
                                                      " INNER JOIN SHIPMENTDETAIL SH ON SH.ORDERID = OD.ORDERID AND SH.ORDERLINE = OD.ORDERLINE AND SH.CONSIGNEE = OD.CONSIGNEE " & _
                                                      " WHERE OD.QTYMODIFIED > 0 AND SH.SHIPMENT='{0}'", shipId)

                    DataInterface.FillDataset(SQL, dt)

                    If dt.Rows.Count > 0 Then
                        If (Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYMODIFIED")) > 0) And (Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("SUMQTY")) = 0) Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Some order lines have not been allocated", "Some order lines have not been allocated")

                            'Commented for (RWMS-2340)RWMS-2339 Start
                            'ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPACKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYVERIFIED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYSTAGED")) > 0 Then
                            'Commented for (RWMS-2340)RWMS-2339 End

                            'Added for (RWMS-2340)RWMS-2339 Start
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYSTAGED")) > 0 Then
                            'Added for (RWMS-2340)RWMS-2339 End
                            Throw New Made4Net.Shared.M4NException(New Exception, "There are still open picks or inventory that has not been loaded", "There are still open picks or inventory that has not been loaded")
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("EXCEPTIONQTY")) <> 0 Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cancel exceptions first", "Cancel exceptions first")
                        End If
                    End If

                    'Added for RWMS-1710 End
                    Dim oShip As New WMS.Logic.Shipment(shipId)

                    'RWMS-2376 - checking status for Shipment.SHIPPING and Shipment.SHIPPED
                    If oShip.STATUS <> WMS.Lib.Statuses.Shipment.LOADED And oShip.STATUS <> WMS.Lib.Statuses.Shipment.SHIPPING And oShip.STATUS <> WMS.Lib.Statuses.Shipment.SHIPPED Then
                        Throw New ApplicationException(ts.Translate("Shipment status is incorrect") & " " & shipId)
                        Exit Sub
                    Else
                        'Added for RWMS-2376
                        If oShip.STATUS <> WMS.Lib.Statuses.Shipment.SHIPPING And oShip.STATUS <> WMS.Lib.Statuses.Shipment.SHIPPED Then
                            oShip.Ship(WMS.Logic.GetCurrentUser)
                        End If
                        'End Added for RWMS-2376
                        'Commented for RWMS-2376
                        'oShip.Ship(WMS.Logic.GetCurrentUser)
                        'End Commented for RWMS-2376
                    End If
                Next

            Case "assignorderlinefromshipment"

                Dim oShipment As New WMS.Logic.Shipment(ds.Tables(0).Rows(0)("SHIPID"))
                If oShipment.STATUS = WMS.Lib.Statuses.Shipment.SHIPPED Then
                    Throw New ApplicationException(ts.Translate("Shipment Status Incorrect"))
                    Exit Sub
                End If
                For Each dr In ds.Tables(0).Rows
                    shipId = dr("SHIPMENT")
                    Dim tmpShip As New WMS.Logic.Shipment(shipId)
                    tmpShip.MoveDetailToShipment(dr("CONSIGNEE"), dr("DOCUMENTTYPE"), dr("ORDERID"), dr("ORDERLINE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SHIPID")), UserId)
                    'If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UNITS")) > 0 Then
                    '    oShipment.AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UNITS")), 0, UserId)
                    'End If
                Next
            Case "assignorders"
                Dim oShipment As New WMS.Logic.Shipment(ds.Tables(0).Rows(0)("SHIPID"))
                If oShipment.STATUS = WMS.Lib.Statuses.Shipment.SHIPPED Then
                    Throw New ApplicationException(ts.Translate("Shipment Status Incorrect"))
                    Exit Sub
                End If

                ' XMLData = addXmlNode(XMLData, "Units", "QTYOPEN")

                Dim sp As New WMS.Logic.Shipment(Sender, CommandName, XMLSchema, XMLData, Message)

            Case "updateloadingsequence"
                shipId = Session("HUTRANSSHIPMENTID")
                Dim oShip As New WMS.Logic.Shipment(shipId)
                For Each dr In ds.Tables(0).Rows
                    Dim newLoadingSeq As Int32
                    If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("loadingseq"), -1) > 0 Then
                        newLoadingSeq = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("loadingseq"), -1)
                    ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ReversedLoadingSeq"), -1) > 0 Then
                        newLoadingSeq = GetLoadingSequence(oShip.SHIPMENT, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ReversedLoadingSeq"), -1))
                    End If
                    UpdateLoadingSequence(dr("Company"), oShip, newLoadingSeq, WMS.Logic.GetCurrentUser, dr("DOCUMENTTYPE"))
                Next
            Case "save", "update"
                Dim sp As New WMS.Logic.Shipment(Sender, CommandName, XMLSchema, XMLData, Message)
                Dim STAGINGLANE As String = "", STAGINGWAREHOUSEAREA As String = ""
                If Not IsDBNull(ds.Tables(0).Rows(0)("STAGINGLANE")) Then STAGINGLANE = ds.Tables(0).Rows(0)("STAGINGLANE")
                If Not IsDBNull(ds.Tables(0).Rows(0)("STAGINGWAREHOUSEAREA")) Then STAGINGWAREHOUSEAREA = ds.Tables(0).Rows(0)("STAGINGWAREHOUSEAREA")
                Dim sql As String = String.Format("update SHIPMENT set STAGINGLANE='{0}', STAGINGWAREHOUSEAREA = '{1}' where SHIPMENT='{2}'", STAGINGLANE, STAGINGWAREHOUSEAREA, sp.SHIPMENT)
                Made4Net.DataAccess.DataInterface.RunSQL(sql)

            Case "printshipmentpackinglist"
                Dim shipment As String
                For Each dr In ds.Tables(0).Rows
                    shipment = dr("SHIPMENT")

                    PrintShipmentPackingList(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser, shipment)

                Next
                'Case "unload"
                '    For Each dr In ds.Tables(0).Rows
                '        Dim shipment As String = dr("SHIPMENT")
                '        Dim sp As New WMS.Logic.Shipment(shipment)
                '        Dim load As New WMS.Logic.Load(dr("LOADID"), True)
                '        sp.UnLoad(dr("LOADID"), load.LOCATION, WMS.Logic.GetCurrentUser)

                '    Next
            Case "unload"
                For Each dr In ds.Tables(0).Rows
                    Dim shipment As String = dr("SHIPMENT")
                    Dim sp As New WMS.Logic.Shipment(shipment)
                    Dim load As New WMS.Logic.Load(dr("LOADID"), True)


                    'while unloaded last payload in activitystatus loaded for container, set container status to staged
                    Dim container As String = load.ContainerId

                    'Added for RWMS-2343 RWMS-2314 - If Load has container, then show a message Payload XXXX is part of a container. Use Shipment Containers tab to unload
                    If container <> "" Then
                        Dim ldid As String = dr("LOADID")
                        Throw New Made4Net.Shared.M4NException(New Exception(String.Format("Payload - '{0}' is part of a container. Use Shipment Containers tab to unload", ldid)), "1069", String.Format("Payload - '{0}' is part of a container. Use Shipment Containers tab to unload", ldid))
                    End If
                    'End Added for RWMS-2343 RWMS-2314

                    Dim contChangStatus As Boolean = False
                    Dim sql As String

                    ' If load.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.LOADED Then
                    sql = "select COUNT(1) from INVLOAD where HANDLINGUNIT='{0}' and activitystatus ='{1}'"
                    sql = String.Format(sql, container, WMS.Lib.Statuses.ActivityStatus.LOADED)

                    sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

                    If sql = 1 Then
                        contChangStatus = True
                    End If

                    ' End If

                    'tab: Shipment Payloads, button: Unload, while unload payloadid change shipment status to loading, only if payload related to order and related to shipment
                    If WMS.Logic.OutboundOrderHeader.Exists(load.CONSIGNEE, dr("ORDERID")) Then
                        sp.UnLoad(dr("LOADID"), load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                    Else
                        load.UnLoad(load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                        sp.ShipmentLoads.ShipmentLoad(load.LOADID).Delete()
                    End If

                    'Added for RWMS-2343 RWMS-2314 - Delete the compartment
                    Dim oCompartments As New Logic.Shipment.ShipmentCompartment
                    If Not String.IsNullOrEmpty(load.ContainerId) Then
                        'check if this is the last load on the container - if it is-> remove container from SHIPMENTCOMPARTMENT
                        Dim sSql As String = String.Format("select COUNT(1) from invload where HANDLINGUNIT={0}", Made4Net.Shared.FormatField(load.ContainerId))
                        If DataInterface.ExecuteScalar(sSql) > 0 Then
                            'get vehicleposition
                            Dim pVehicleLocation As String
                            Dim SqlVLocation As String = String.Format("select VEHICLELOCATION from SHIPMENTCOMPARTMENT where SHIPMENT={0} and HANDLINGUNIT={1}", Made4Net.Shared.FormatField(shipment), Made4Net.Shared.FormatField(load.ContainerId))
                            pVehicleLocation = DataInterface.ExecuteScalar(SqlVLocation)
                            oCompartments.Delete(sp.SHIPMENT, pVehicleLocation, load.ContainerId)
                        End If

                    Else
                        'get vehicleposition
                        Dim pVehicleLocation As String
                        Dim SqlVLocation As String = String.Format("select VEHICLELOCATION from SHIPMENTCOMPARTMENT where SHIPMENT={0} and HANDLINGUNIT={1}", Made4Net.Shared.FormatField(shipment), Made4Net.Shared.FormatField(load.LOADID))
                        pVehicleLocation = DataInterface.ExecuteScalar(SqlVLocation)
                        oCompartments.Delete(sp.SHIPMENT, pVehicleLocation, load.LOADID)
                    End If
                    'End Added for RWMS-2343 RWMS-2314

                    'while unloaded last payload in activitystatus loaded for container, set container status to staged
                    If contChangStatus And WMS.Logic.Container.Exists(container) Then
                        Dim cnt As New WMS.Logic.Container(container, False)
                        ' cnt.SetActivityStatus(WMS.Lib.Statuses.Container.STAGED, WMS.Logic.GetCurrentUser)
                        cnt.Status = WMS.Lib.Statuses.Container.STAGED
                        cnt.Save(WMS.Logic.GetCurrentUser)
                    End If
                Next

                'Added for RWMS-2343 RWMS-2314
            Case "unloadcontainers"
                For Each dr In ds.Tables(0).Rows
                    Dim shipment As String = dr("SHIPMENT")
                    Dim sp As New WMS.Logic.Shipment(shipment)
                    'Dim load As New WMS.Logic.Load(dr("LOADID"), True)

                    ''while unloaded last payload in activitystatus loaded for container, set container status to staged
                    Dim container As String = dr("CONTAINERID")
                    Dim contChangStatus As Boolean = False
                    Dim sql As String

                    'Get the containerloads
                    Dim SqlLoad As String = String.Format("select * from VShipmentLoads1 WHERE HANDLINGUNIT='{0}'", container)
                    Dim dtLoad As New DataTable
                    DataInterface.FillDataset(SqlLoad, dtLoad)

                    For Each drLoad As DataRow In dtLoad.Rows
                        Dim load As New WMS.Logic.Load(drLoad("LOADID"), True)

                        sql = "select COUNT(1) from INVLOAD where HANDLINGUNIT='{0}' and activitystatus ='{1}'"
                        sql = String.Format(sql, container, WMS.Lib.Statuses.ActivityStatus.LOADED)
                        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

                        If sql = 1 Then
                            contChangStatus = True
                        End If

                        'tab: Shipment Containers, button: Unload, while unload payloadid change shipment status to loading, only if payload related to order and related to shipment
                        If WMS.Logic.OutboundOrderHeader.Exists(load.CONSIGNEE, drLoad("ORDERID")) Then
                            sp.UnLoad(load.LOADID, load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                        Else
                            load.UnLoad(load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                            sp.ShipmentLoads.ShipmentLoad(load.LOADID).Delete()
                        End If

                        ''tab: Shipment Containers, button: Unload, while unload payloadid change shipment status to loading, only if payload related to order and related to shipment
                        ''get the load order and validate
                        'Dim sqlOrder As String = "select ORDERID from ORDERLOADS where LOADID='{0}'"
                        'sqlOrder = String.Format(sqlOrder, load.LOADID)
                        'Dim Orderid As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqlOrder)

                        'If Not String.IsNullOrEmpty(Orderid) Then
                        '    If Not String.IsNullOrEmpty(Orderid) And WMS.Logic.OutboundOrderHeader.Exists(load.CONSIGNEE, Orderid) Then
                        '        sp.UnLoad(load.LOADID, load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                        '    Else
                        '        load.UnLoad(load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                        '        sp.ShipmentLoads.ShipmentLoad(load.LOADID).Delete()
                        '    End If
                        'Else
                        '    load.UnLoad(load.LOCATION, load.WAREHOUSEAREA, WMS.Logic.GetCurrentUser)
                        '    sp.ShipmentLoads.ShipmentLoad(load.LOADID).Delete()
                        'End If

                    Next

                    'Added for RWMS-2343 RWMS-2314 - Delete the compartment
                    Dim oCompartments As New Logic.Shipment.ShipmentCompartment
                    Dim sSql As String = String.Format("select COUNT(1) from invload where HANDLINGUNIT={0}", Made4Net.Shared.FormatField(container))
                    If DataInterface.ExecuteScalar(sSql) > 0 Then
                        'get vehicleposition
                        Dim pVehicleLocation As String
                        Dim SqlVLocation As String = String.Format("select VEHICLELOCATION from SHIPMENTCOMPARTMENT where SHIPMENT={0} and HANDLINGUNIT={1}", Made4Net.Shared.FormatField(shipment), Made4Net.Shared.FormatField(container))
                        pVehicleLocation = DataInterface.ExecuteScalar(SqlVLocation)
                        oCompartments.Delete(sp.SHIPMENT, pVehicleLocation, container)
                    End If
                    'End Added for RWMS-2343 RWMS-2314

                    'while unloaded last payload in activitystatus loaded for container, set container status to staged
                    If contChangStatus And WMS.Logic.Container.Exists(container) Then
                        Dim cnt As New WMS.Logic.Container(container, False)
                        ' cnt.SetActivityStatus(WMS.Lib.Statuses.Container.STAGED, WMS.Logic.GetCurrentUser)
                        cnt.Status = WMS.Lib.Statuses.Container.STAGED
                        cnt.Save(WMS.Logic.GetCurrentUser)
                    End If
                Next
                'End Added for RWMS-2343 RWMS-2314

            Case "autoship"
                Dim shipment As String
                Dim errMsg As String
                For Each dr In ds.Tables(0).Rows
                    shipment = dr("SHIPMENT")
                    Dim s As New WMS.Logic.Shipment(shipment)


                    If s.STATUS = "SHIPPED" Then
                        errMsg = "Shipment already shiped"
                        Throw New Made4Net.Shared.M4NException(New Exception, ts.Translate(errMsg), ts.Translate(errMsg))
                    End If

                    If Not autoshipValidate(shipment, errMsg) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, ts.Translate(errMsg), ts.Translate(errMsg))
                    End If

                    '"cancel shipment exception"
                    WebAppUtil.processShipmentExceptionLines(shipment)


                    s.PrintShippingManifest(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)


                    WebAppUtil.processShipmentLoadLoads(shipment, s.DOOR)


                    s.Ship(UserId)
                    Message = "Shipment shiped"
                Next
        End Select
    End Sub

    'Added for RWMS-2447 - validate for all the orders are cancelled
    Private Function isAllOrdersCancelled(ByVal shipment As String) As Boolean
        Dim ret As Boolean = False
        Dim sql As String

        sql = "select count(*) from SHIPMENT SH inner join SHIPMENTDETAIL SD on SH.SHIPMENT=sd.SHIPMENT inner join OUTBOUNDORDETAIL OD on SD.ORDERID=OD.ORDERID inner join OUTBOUNDORHEADER OH on OH.ORDERID=OD.ORDERID where SH.SHIPMENT='{0}' and OH.STATUS<>'CANCELED'"
        sql = String.Format(sql, shipment)

        If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) = 0 Then
            Return True
        End If

        Return ret
    End Function
    'End Added for RWMS-2447

    Private Function autoshipValidate(ByVal shipment As String, ByRef errorMsg As String) As Boolean
        Dim ret As Boolean = True
        Dim sql As String

        sql = "select count(*) from vAutoShipIncompletePick where shipment='{0}'"
        sql = String.Format(sql, shipment)

        If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) > 0 Then
            errorMsg = "Not all pick list are complete" ', Press OK to return to shipment"
            Return False
        End If

        'sql = "select count(*) from vAutoShipPayloads where shipment='{0}'"
        'sql = String.Format(sql, shipment)

        'If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) > 0 Then
        '    errorMsg = "All Loads not complete" ', Press OK to return to shipment"
        '    Return False
        'End If

        'sql = "select count(*) from vAutoShipContainers where shipment='{0}'"
        'sql = String.Format(sql, shipment)

        'If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) > 0 Then
        '    errorMsg = "All Containers not complete" ', Press OK to return to shipment"
        '    Return False
        'End If

        Return ret
    End Function


    Public Sub LoadPallet(ByVal pUser As String, ByVal SHIPMENT As String)
        Dim oShip As New WMS.Logic.Shipment(Shipment)

        If oShip.STATUS = WMS.Lib.Statuses.Shipment.ASSIGNED Or oShip.STATUS = WMS.Lib.Statuses.Shipment.ATDOCK Then
            If IsLoadingCompleted(SHIPMENT) Then
                'Logging.logMessage(LogDirPath, filename, "Status is set to loaded  when the loading is complete " + pUser, "INFO")
                oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)

                'Added for RWMS-2274 - validate all the ShipmentOutboundorders and update the status as LOADED
                'Commented for RWMS-2447
                'Dim SqlShipdetail As String = String.Format(" SELECT DISTINCT OH.ORDERID,OH.CONSIGNEE FROM SHIPMENTDETAIL SD INNER JOIN OUTBOUNDORHEADER OH ON OH.ORDERID = SD.ORDERID AND OH.CONSIGNEE = SD.CONSIGNEE WHERE SD.SHIPMENT='{0}'", SHIPMENT)
                'Commented for RWMS-2447
                'Added for RWMS-2447
                Dim SqlShipdetail As String = String.Format(" SELECT DISTINCT OH.ORDERID,OH.CONSIGNEE FROM SHIPMENTDETAIL SD INNER JOIN OUTBOUNDORHEADER OH ON OH.ORDERID = SD.ORDERID AND OH.CONSIGNEE = SD.CONSIGNEE WHERE SD.SHIPMENT='{0}' and OH.STATUS<>'CANCELED'", SHIPMENT)
                'Added for RWMS-2447

                Dim dtOrders As New DataTable
                DataInterface.FillDataset(SqlShipdetail, dtOrders)
                For Each drOrder As DataRow In dtOrders.Rows
                    Try
                        Dim oOrdHeader As OutboundOrderHeader
                        oOrdHeader = New OutboundOrderHeader(drOrder("CONSIGNEE"), drOrder("ORDERID"))

                        'RWMS-2447 - If the outbound order is cancelled, then do not update the order status.
                        If oOrdHeader.STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
                            'Dim bFullyLoaded As Boolean = True
                            If oOrdHeader.FULLYLOADED Then
                                If oOrdHeader.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
                                    'donothing
                                Else
                                    'update order to status LOADED
                                    oOrdHeader.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.LOADED, pUser)
                                End If
                            End If
                        End If

                    Catch ex As Exception

                    End Try
                Next
                'End Added for RWMS-2274

            Else
                'oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)
                'Logging.logMessage(LogDirPath, filename, "Updating the start loading date.. " + pUser, "INFO")
                UpdateStartLoadingDate(pUser, SHIPMENT)
                'Logging.logMessage(LogDirPath, filename, "Setting status to  loading .. " + pUser, "INFO")
                oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
                'RWMS-2003 (RWMS-705)

                'Logging.logMessage(LogDirPath, filename, "Loading process starts .. " + pUser, "INFO")
                LoadingComplete(SHIPMENT)
                'Logging.logMessage(LogDirPath, filename, "Completed the loading .. " + pUser, "INFO")
            End If
        ElseIf oShip.STATUS = WMS.Lib.Statuses.Shipment.LOADING Then
            If IsLoadingCompleted(SHIPMENT) Then
                'Logging.logMessage(LogDirPath, filename, "Status is set to loaded  when the status is in LOADING status " + pUser, "INFO")
                oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)
                'Added for RWMS-1710 Start

                'Added for RWMS-2274 - validate all the ShipmentOutboundorders and update the status as LOADED
                Dim SqlShipdetail As String = String.Format(" SELECT DISTINCT OH.ORDERID,OH.CONSIGNEE FROM SHIPMENTDETAIL SD INNER JOIN OUTBOUNDORHEADER OH ON OH.ORDERID = SD.ORDERID AND OH.CONSIGNEE = SD.CONSIGNEE WHERE SD.SHIPMENT='{0}'", SHIPMENT)

                Dim dtOrders As New DataTable
                DataInterface.FillDataset(SqlShipdetail, dtOrders)
                For Each drOrder As DataRow In dtOrders.Rows
                    Try
                        Dim oOrdHeader As OutboundOrderHeader
                        oOrdHeader = New OutboundOrderHeader(drOrder("CONSIGNEE"), drOrder("ORDERID"))

                        'RWMS-2447 - If the outbound order is cancelled, then do not update the order status.
                        If oOrdHeader.STATUS <> WMS.Lib.Statuses.OutboundOrderHeader.CANCELED Then
                            'Dim bFullyLoaded As Boolean = True
                            If oOrdHeader.FULLYLOADED Then
                                If oOrdHeader.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED Then
                                    'donothing
                                Else
                                    'update order to status LOADED
                                    oOrdHeader.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.LOADED, pUser)
                                End If
                            End If
                        End If

                    Catch ex As Exception

                    End Try
                Next
                'End Added for RWMS-2274

            Else
                'UpdateStartLoadingDate(pUser, SHIPMENT)
                'oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
                LoadingComplete(SHIPMENT)

                'Added for RWMS-1710 End
            End If
        End If
    End Sub
    'Added for RWMS-2014 (RWMS-705)
    Private Sub LoadingComplete(ByVal SHIPMENT As String)
        Dim oShip As New WMS.Logic.Shipment(Shipment)
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'Dim SqlLoads As String = String.Format("SELECT PH.PICKTYPE,OL.LOADID,OH.CONSIGNEE,S.SKU,SKUDESC,L.UNITS,L.HANDLINGUNIT" & _
        '                                       " FROM  ORDERLOADS OL INNER  JOIN OUTBOUNDORHEADER OH ON OH.ORDERID=OL.ORDERID " & _
        '                                       " INNER JOIN INVLOAD L ON L.LOADID=OL.LOADID " & _
        '                                       " INNER JOIN COMPANY C ON C.COMPANY=OH.TARGETCOMPANY " & _
        '                                       " INNER JOIN SKU S ON S.SKU=L.SKU " & _
        '                                       " INNER JOIN PICKDETAIL PD ON PD.ORDERID=OL.ORDERID AND PD.SKU=S.SKU " & _
        '                                       " INNER JOIN PICKHEADER PH ON PH.PICKLIST=PD.PICKLIST " & _
        '                                       " INNER  JOIN SHIPMENTDETAIL SH ON SH.ORDERID=OL.ORDERID AND SH.ORDERLINE=OL.ORDERLINE " & _
        '                                       " WHERE L.ACTIVITYSTATUS NOT IN ('LOADED') and SH.SHIPMENT='{0}'", SHIPMENT)


        'Commented for RWMS-1710 Start

        'Dim SqlLoads As String = String.Format("SELECT DISTINCT PH.PICKTYPE,( CASE WHEN PH.PICKTYPE = 'FULLPICK' THEN OL.LOADID ELSE NULL END ) AS LOADID," & _
        '                                       " OH.CONSIGNEE,( CASE WHEN PH.PICKTYPE = 'FULLPICK' THEN S.SKU ELSE NULL END) AS SKU ," & _
        '                                       " ( CASE WHEN PH.PICKTYPE = 'FULLPICK' THEN SKUDESC ELSE NULL END) AS SKUDESC,  " & _
        '                                       " ( CASE WHEN PH.PICKTYPE = 'FULLPICK' THEN L.UNITS ELSE NULL END) AS UNITS, L.HANDLINGUNIT" & _
        '                                       " FROM  ORDERLOADS OL INNER  JOIN OUTBOUNDORHEADER OH ON OH.ORDERID=OL.ORDERID " & _
        '                                       " INNER JOIN INVLOAD L ON L.LOADID=OL.LOADID " & _
        '                                       " INNER JOIN COMPANY C ON C.COMPANY=OH.TARGETCOMPANY " & _
        '                                       " INNER JOIN SKU S ON S.SKU=L.SKU " & _
        '                                       " INNER JOIN PICKDETAIL PD ON PD.ORDERID=OL.ORDERID AND PD.SKU=S.SKU " & _
        '                                       " INNER JOIN PICKHEADER PH ON PH.PICKLIST=PD.PICKLIST " & _
        '                                       " INNER  JOIN SHIPMENTDETAIL SH ON SH.ORDERID=OL.ORDERID AND SH.ORDERLINE=OL.ORDERLINE " & _
        '                                       " WHERE L.ACTIVITYSTATUS NOT IN ('LOADED') and SH.SHIPMENT='{0}'", SHIPMENT)


        'Commented for RWMS-1710 End


        'Added for RWMS-1710 Start
        Dim SqlLoads As String = String.Format(" SELECT DISTINCT PH.PICKTYPE,L.LOADID,OH.CONSIGNEE,S.SKU,S.SKUDESC,L.UNITS,L.HANDLINGUNIT FROM ORDERLOADS OL" & _
                                               " INNER JOIN OUTBOUNDORHEADER OH ON OH.ORDERID = OL.ORDERID AND OH.CONSIGNEE = OL.CONSIGNEE" & _
                                               " INNER JOIN INVLOAD L ON L.LOADID = OL.LOADID AND L.CONSIGNEE = OH.CONSIGNEE" & _
                                               " INNER JOIN SKU S ON S.SKU = L.SKU AND S.CONSIGNEE = L.CONSIGNEE" & _
                                               " INNER JOIN PICKDETAIL PD ON PD.PICKLIST = OL.PICKLIST AND PD.PICKLISTLINE = OL.PICKLISTLINE" & _
                                               " AND PD.SKU = S.SKU AND PD.CONSIGNEE = L.CONSIGNEE INNER JOIN PICKHEADER PH" & _
                                               " ON PH.PICKLIST = PD.PICKLIST INNER JOIN SHIPMENTDETAIL SH ON SH.ORDERID = OL.ORDERID" & _
                                               " AND SH.ORDERLINE = OL.ORDERLINE AND SH.CONSIGNEE = S.CONSIGNEE WHERE L.ACTIVITYSTATUS NOT IN ('LOADED')" & _
                                               " AND SH.SHIPMENT='{0}'", SHIPMENT)

        'Added for RWMS-1710 End

        Dim dtLoads As New DataTable
        DataInterface.FillDataset(SqlLoads, dtLoads)


        For Each drLoads As DataRow In dtLoads.Rows

            Try

                Dim oLoading As New WMS.Logic.Loading
                Dim oLoadingJob As WMS.Logic.LoadingJob
                Dim IsContainerExists As Boolean = False
                Dim IsLoadExists As Boolean = False


                If drLoads("PICKTYPE") = "PARTIAL" Then
                    IsContainerExists = WMS.Logic.Container.Exists(drLoads("HANDLINGUNIT"))
                Else
                    IsLoadExists = WMS.Logic.Load.LoadExists(drLoads("LOADID"))
                End If

                Dim err1 As String

                If IsContainerExists And IsLoadExists Then
                    If LoadBelong2Shipment(drLoads("LOADID")) Then
                        Dim l As New WMS.Logic.Load(drLoads("LOADID").ToString())
                        If l.ACTIVITYSTATUS = "LOADED" Then
                            'Made4Net.Mobile.MessageQue.Enqueue(trans.Translate("Payload already loaded"))
                            'Return

                            If ContainerBelong2Shipment(drLoads("HANDLINGUNIT"), err1) Then
                                oLoadingJob = oLoading.PickupContainer(drLoads("HANDLINGUNIT"), True, WMS.Logic.Common.GetCurrentUser)
                            Else
                                Throw New ApplicationException(trans.Translate(err1))
                                drLoads("HANDLINGUNIT") = ""
                                Return
                            End If
                        Else

                            Dim loadCont As String
                            loadCont = LoadBelong2Container(drLoads("LOADID"))
                            If loadCont = "" Then
                                oLoadingJob = oLoading.PickupLoad(drLoads("LOADID"), True, WMS.Logic.Common.GetCurrentUser)
                            Else
                                Dim msg As String
                                msg = String.Format("Load belongs to container {0}. Please enter container id", loadCont)
                                Throw New ApplicationException((trans.Translate(msg)))
                                drLoads("LOADID") = ""
                                Return
                            End If

                        End If
                    ElseIf ContainerBelong2Shipment(drLoads("HANDLINGUNIT"), err1) Then
                        oLoadingJob = oLoading.PickupContainer(drLoads("HANDLINGUNIT"), True, WMS.Logic.Common.GetCurrentUser)
                    Else
                        Throw New ApplicationException(trans.Translate("Payload does not belong to shipment"))
                        drLoads("HANDLINGUNIT") = ""
                        Return
                    End If

                    'Added for RWMS-1710 Start
                    CheckLoadPallet(oLoadingJob, drLoads, oLoading, oShip)
                    'Added for RWMS-1710 End
                ElseIf IsLoadExists And Not IsContainerExists Then
                    If LoadBelong2Shipment(drLoads("LOADID")) Then
                        Dim l As New WMS.Logic.Load(drLoads("LOADID").ToString())
                        If l.ACTIVITYSTATUS = "LOADED" Then
                            Throw New ApplicationException(trans.Translate("Payload already loaded"))
                            Return
                        Else
                            'Added for RWMS-670 - check whether the load belongs to container. If not proceed, else display message to enter the container
                            Dim loadCont As String
                            loadCont = LoadBelong2Container(drLoads("LOADID"))
                            If loadCont = "" Then
                                oLoadingJob = oLoading.PickupLoad(drLoads("LOADID"), True, WMS.Logic.Common.GetCurrentUser)
                            Else
                                Dim msg As String
                                msg = String.Format("Load belongs to container {0}. Please enter container id", loadCont)
                                Throw New ApplicationException(trans.Translate(msg))
                                drLoads("LOADID") = ""
                                Return
                            End If

                        End If
                    Else
                        Throw New ApplicationException(trans.Translate("Payload does not belong to shipment"))
                        drLoads("LOADID") = ""
                        Return
                    End If


                    'Added for RWMS-1710 Start
                    CheckLoadPallet(oLoadingJob, drLoads, oLoading, oShip)
                    'Added for RWMS-1710 End
                ElseIf IsContainerExists Then
                    'Added for RWMS-1710 Start
                    Dim CurrentContainer As String = drLoads("HANDLINGUNIT")
                    Dim ContainerStatus As String = DataInterface.ExecuteScalar((String.Format("SELECT STATUS FROM CONTAINER WHERE CONTAINER = '{0}' ", CurrentContainer)))
                    If ContainerStatus <> "LOADED" Then
                        'Added for RWMS-1710 End
                        Dim err As String
                        If ContainerBelong2Shipment(drLoads("HANDLINGUNIT"), err) Then
                            oLoadingJob = oLoading.PickupContainer(drLoads("HANDLINGUNIT"), True, WMS.Logic.Common.GetCurrentUser)
                        Else
                            Throw New ApplicationException(err)
                            drLoads("HANDLINGUNIT") = ""
                            Return
                        End If

                        'Added for RWMS-1710 Start
                        CheckLoadPallet(oLoadingJob, drLoads, oLoading, oShip)
                    End If
                    'Added for RWMS-1710 End

                End If

            Catch ex As Threading.ThreadAbortException
                'Do Nothing
            Catch ex As Made4Net.Shared.M4NException
                Throw New ApplicationException(trans.Translate(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID)))
                Return
            Catch ex As Exception
                Throw New ApplicationException(trans.Translate(ex.Message))
                Return
            End Try
        Next

    End Sub

    'RWMS-2003 (RWMS-1710)
    'Added for RWMS-1710 Start
    Private Sub CheckLoadPallet(oLoadingJob As WMS.Logic.LoadingJob, drLoads As DataRow, oLoading As WMS.Logic.Loading, oShip As WMS.Logic.Shipment)
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try

            If oLoadingJob Is Nothing Then
                Throw New ApplicationException(trans.Translate("Pallet was not found"))
                drLoads("HANDLINGUNIT") = ""
                drLoads("LOADID") = ""
                Return
            Else
                Session("LoadingJob") = oLoadingJob
            End If

            Dim sql, vehicleType As String
            sql = String.Format("SELECT VEHICLETYPENAME FROM VEHICLE where VEHICLEID = {0}", Made4Net.Shared.FormatField(oShip.VEHICLE))
            vehicleType = DataInterface.ExecuteScalar(sql)
            sql = String.Format("Select Location from vehiclelocations where vehicletype={0}", Made4Net.Shared.FormatField(vehicleType))

            Dim dt As New DataTable
            Dim dr As DataRow
            Dim pPosition As String
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
            End If

            If Not dr.IsNull("Location") Then pPosition = dr.Item("Location")


            If Not oLoadingJob Is Nothing Then
                If oLoadingJob.ToWarehousearea = "" Then oLoadingJob.ToWarehousearea = Warehouse.getUserWarehouseArea()

                If CType(WarehouseParams.GetWarehouseParam("ShowPositionLoading"), Boolean) Then
                    oLoading.LoadPallet(oLoadingJob, pPosition, oShip.DOOR.ToLower, oLoadingJob.ToWarehousearea, WMS.Logic.Common.GetCurrentUser)
                Else
                    oLoading.LoadPallet(oLoadingJob, Nothing, oShip.DOOR.ToLower, oLoadingJob.ToWarehousearea, WMS.Logic.Common.GetCurrentUser)
                End If
            End If
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            Throw New ApplicationException(trans.Translate(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID)))
            Return
        Catch ex As Exception
            Throw New ApplicationException(trans.Translate(ex.Message))
            Return
        End Try
    End Sub
    'Added for RWMS-1710 End

    'RWMS-2003 (RWMS-705)

    Private Function LoadBelong2Container(ByVal load As String) As String
        Dim ret As String = ""
        Dim SQL As String
        SQL = String.Format("select HANDLINGUNIT from LOADS where LOADID = '{0}'", load)
        'Commented for RWMS-737
        'ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        'End RWMS-737
        'Added for RWMS-737
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        Catch ex As Exception
            ret = ""
        End Try
        'End Added for RWMS-737
        If String.IsNullOrEmpty(ret) Then
            ret = ""
        End If
        Return ret
    End Function
    Private Function ContainerBelong2Shipment(ByVal container As String, ByRef err As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If IsNothing(Session("LoadingJobsShipmentId")) OrElse Session("LoadingJobsShipmentId").ToString = "" Then
            SQL = String.Format("SELECT  STATUS FROM vLoadContainerShipment WHERE CONTAINERID = '{0}' ", container)
        Else
            SQL = String.Format("SELECT  STATUS FROM vLoadContainerShipment WHERE CONTAINERID = '{0}' and SHIPMENT='{1}' ", container, Session("LoadingJobsShipmentId"))
        End If

        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            err = trans.Translate("Container does not belong to shipment")
            ret = False
        Else
            Try
                If dt.Rows(0)(0) = WMS.Lib.Statuses.Container.LOADED Then
                    err = trans.Translate("Container already loaded")
                    ret = False
                End If
            Catch ex As Exception
                err = trans.Translate("Container already loaded")
                ret = False
            End Try
        End If
        Return ret
    End Function
    Private Function LoadBelong2Shipment(ByVal load As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        If IsNothing(Session("LoadingJobsShipmentId")) OrElse Session("LoadingJobsShipmentId").ToString = "" Then
            SQL = String.Format("SELECT count(1) FROM dbo.ORDERLOADS AS ol INNER JOIN dbo.SHIPMENTDETAIL AS sp ON ol.CONSIGNEE = sp.CONSIGNEE AND ol.ORDERID = sp.ORDERID AND ol.ORDERLINE = sp.ORDERLINE where  ol.LOADID='{0}' ", load)
        Else
            SQL = String.Format("SELECT count(1) FROM dbo.ORDERLOADS AS ol INNER JOIN dbo.SHIPMENTDETAIL AS sp ON ol.CONSIGNEE = sp.CONSIGNEE AND ol.ORDERID = sp.ORDERID AND ol.ORDERLINE = sp.ORDERLINE where  ol.LOADID='{0}' and sp.SHIPMENT='{1}'", load, Session("LoadingJobsShipmentId"))
        End If

        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function
    'Added for RWMS-2014 End
    Private Sub UpdateStartLoadingDate(ByVal pUser As String, ByVal SHIPMENT As String)
        Dim sql As String = String.Format("Update SHIPMENT SET STARTLOADINGDATE = {0},EditUser = {1},EditDate = {2} Where SHIPMENT='{3}'", _
            Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(DateTime.Now), SHIPMENT)
        DataInterface.RunSQL(sql)
    End Sub


    Public Function IsLoadingCompleted(ByVal SHIPMENT As String) As Boolean

        'Commented for RWMS-1710 Start
        'Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits  from (select sd.SHIPMENT,SUM(sd.units) as units from SHIPMENTDETAIL sd group by SHIPMENT) sd left outer join (SELECT     sp1.SHIPMENT, SUM(iv.UNITS) AS unitsLoaded FROM         dbo.SHIPMENTDETAIL AS sp1 INNER JOIN dbo.ORDERLOADS AS ol ON sp1.CONSIGNEE =ol.CONSIGNEE AND sp1.ORDERID = ol.ORDERID INNER JOIN dbo.INVLOAD AS iv ON ol.LOADID = iv.LOADID INNER JOIN dbo.SHIPMENTLOADS sl1 ON ol.LOADID = sl1.LOADID GROUP BY sp1.SHIPMENT) sl on sl.SHIPMENT = sd.SHIPMENT where sd.SHIPMENT = '{0}'", SHIPMENT)
        'Commented for RWMS-1710 End

        'Commented for RWMS-2447
        ''Added for RWMS-1710 Start
        ''Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits  from (select sd.SHIPMENT,SUM(sd.units) as units from SHIPMENTDETAIL sd group by SHIPMENT) sd left outer join (SELECT     sp1.SHIPMENT, SUM(iv.UNITS) AS unitsLoaded FROM         dbo.SHIPMENTDETAIL AS sp1 INNER JOIN dbo.ORDERLOADS AS ol ON sp1.CONSIGNEE =ol.CONSIGNEE AND sp1.ORDERID = ol.ORDERID AND sp1.ORDERLINE = ol.ORDERLINE INNER JOIN dbo.INVLOAD AS iv ON ol.LOADID = iv.LOADID INNER JOIN dbo.SHIPMENTLOADS sl1 ON ol.LOADID = sl1.LOADID GROUP BY sp1.SHIPMENT) sl on sl.SHIPMENT = sd.SHIPMENT where sd.SHIPMENT = '{0}'", SHIPMENT)
        ''Added for RWMS-1710 End
        'Dim remainingUnits As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'If remainingUnits > 0 Then
        '    Return False
        'Else
        '    Return True
        'End If
        'Commented for RWMS-2447

        'Added for RWMS-2447
        Dim sqlUnits As String = String.Format("select sd.SHIPMENT ,sum(od.QTYLOADED) as shipmentunits,sum(od.QTYMODIFIED)-sum(od.QTYLOADED) as RemainingUnits from SHIPMENTDETAIL sd left join OUTBOUNDORDETAIL od on od.ORDERID=sd.ORDERID and sd.ORDERLINE=od.ORDERLINE left join ORDERLOADS ol on ol.CONSIGNEE=od.CONSIGNEE and od.ORDERID=ol.ORDERID and od.ORDERLINE=ol.ORDERLINE where sd.SHIPMENT ='{0}' group by sd.SHIPMENT", SHIPMENT)
        Dim dtUnits As New DataTable
        DataInterface.FillDataset(sqlUnits, dtUnits)
        If dtUnits.Rows.Count > 0 Then
            Dim remainingUnits As Decimal = dtUnits.Rows(0)("RemainingUnits")
            Dim Units As Decimal = dtUnits.Rows(0)("shipmentunits")
            If remainingUnits > 0 Or Units = 0 Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
        'End for RWMS-2447

    End Function
    Public Sub PrintShipmentPackingList(ByVal Language As Int32, ByVal pUser As String, ByVal shipment As String)
        Try
            Dim DocumentsInShipment As DataTable = New DataTable
            DataInterface.FillDataset("Select * FROM vShipingManifestHeader WHERE SHIPMENT='" & shipment & "'", DocumentsInShipment)
        For Each doc As DataRow In DocumentsInShipment.Rows
                Select Case Convert.ToString(doc("DOCTYPE"))
                    Case "OUTBOUND"
                        Dim oDoc As New WMS.Logic.OutboundOrderHeader(doc("CONSIGNEE"), doc("ORDERID"))
                        'oDoc.PrintShippingManifest(Language, pUser)
                        OutboundOrderPrintShippingManifest(Language, pUser, oDoc, shipment)
                        'Start RWMS-1303 Printing shipping documents does not include Flowthrough orders
                        'Case "FLOWTROUGH"
                    Case "FLOWTHROUGH"
                        'End RWMS-1303 Printing shipping documents does not include Flowthrough orders
                        Dim oDoc As New WMS.Logic.Flowthrough(doc("CONSIGNEE"), doc("ORDERID"))
                        ' oDoc.PrintShippingManifest(Language, pUser)
                        FlowthroughPrintShippingManifest(Language, pUser, oDoc, shipment)

                    Case "TRANSSHIPEMNT"
                        Dim oDoc As New WMS.Logic.TransShipment(doc("CONSIGNEE"), doc("ORDERID"))
                        ' oDoc.PrintShippingManifest(Language, pUser)
                        TransShipmentPrintShippingManifest(Language, pUser, oDoc, shipment)

                End Select
            Next
        Catch ex As Exception

        End Try
    End Sub

    <CLSCompliant(False)>
    Public Sub TransShipmentPrintShippingManifest(ByVal Language As Int32, ByVal pUser As String, ByVal oDoc As WMS.Logic.TransShipment, ByVal shipment As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "TranshipmentDelNote"
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "TranshipmentDelNote", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "TranshipmentDelNote")
        oQsender.Add("DATASETID", "repTranshipmentDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONSIGNEE = '{0}' and TRANSSHIPMENT = '{1}' and SHIPMENT='{2}'", oDoc.CONSIGNEE, oDoc.TRANSSHIPMENT, shipment))
        oQsender.Send("Report", repType)
    End Sub


    <CLSCompliant(False)>
    Public Sub FlowthroughPrintShippingManifest(ByVal Language As Int32, ByVal pUser As String, ByVal oDoc As WMS.Logic.Flowthrough, ByVal shipment As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = "FlowthrowghDelNote"
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "FlowthrowghDelNote", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "FlowthrowghDelNote")
        oQsender.Add("DATASETID", "repFlowthrowghDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "") 'dt.Rows(0)("DEFAULTPRINTER"))
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONSIGNEE='{0}' and FLOWTHROUGH='{1}' and SHIPMENT='{2}'", oDoc.CONSIGNEE, oDoc.FLOWTHROUGH, shipment))
        oQsender.Send("Report", repType)
    End Sub


    <CLSCompliant(False)>
    Public Sub OutboundOrderPrintShippingManifest(ByVal Language As Int32, ByVal pUser As String, ByVal oDoc As WMS.Logic.OutboundOrderHeader, ByVal shipment As String)
        Dim repType As String
        ' Deside wich Delivery Note we need to print
        ' If TargetCompany is not empty then print Company delivery note
        Dim oComp As WMS.Logic.Company = New WMS.Logic.Company(oDoc.CONSIGNEE, oDoc.TARGETCOMPANY, oDoc.COMPANYTYPE)
        Dim oCons As WMS.Logic.Consignee = New WMS.Logic.Consignee(oDoc.CONSIGNEE)
        Dim strWhere As String ' RWMS-1196
        If oComp.DELIVERYNOTELAYOUT.Length > 0 Then
            repType = oComp.DELIVERYNOTELAYOUT
        ElseIf oCons.SHIPPINGMANIFEST.Length > 0 Then
            repType = oCons.SHIPPINGMANIFEST
        Else
            'Commented For RWMS-186
            repType = "OutboundDelNote" '' Uncommented for RWMS-1196
            strWhere = String.Format("CONSIGNEE = '{0}' and ORDERID = '{1}'", oDoc.CONSIGNEE, oDoc.ORDERID) '' Added for  RWMS-1196
            'End Commented For RWMS-186
            'Added for RWMS-186
            'repType = "OutboundDelNoteLabodega" '' Commented for RWMS-1196
            'End Added for RWMS-186
        End If

        Dim oQsender As New Made4Net.Shared.QMsgSender

        Dim dt As New DataTable

        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False)
        'Dim rep As Made4Net.Reporting.Common.Report
        'rep = Made4Net.Reporting.Common.Report.getReportInstance("ShipMan")
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        oQsender.Add("DATASETID", DataInterface.ExecuteScalar(String.Format("SELECT ParamValue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = 'DataSetName'", repType))) '"repOutboundDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        '' Start RWMS-1196
        If repType = "OutboundDelNote" Then
            oQsender.Add("WHERE", strWhere)
        Else
            oQsender.Add("WHERE", String.Format("CONSIGNEE = '{0}' and ORDERID = '{1}' and SHIPMENT='{2}'", oDoc.CONSIGNEE, oDoc.ORDERID, shipment))
        End If
        '' End RWMS-1196
        oQsender.Send("Report", repType)
    End Sub


    Private Function addXmlNode(ByVal SXML As String, ByVal misNodeName As String, Optional ByVal toFillNodeName As String = "", Optional ByVal toFillValue As String = "") As String

        Dim myXmlDocument As New XmlDocument()
        Try
            myXmlDocument.LoadXml(SXML)

            Dim node As XmlNode
            node = myXmlDocument.DocumentElement

            For Each node In node.ChildNodes

                If IsNothing(node.SelectSingleNode(misNodeName)) Then
                    Dim elem As XmlElement = myXmlDocument.CreateElement(misNodeName)
                    If toFillNodeName <> "" Then
                        elem.InnerText = node.SelectSingleNode(toFillNodeName).InnerText
                    Else
                        elem.InnerText = toFillValue
                    End If
                    node.AppendChild(elem)
                End If
            Next
        Catch ex As Exception

        End Try
        Return myXmlDocument.InnerXml
    End Function


#End Region

#Region "Methods"

    Private Sub MoveLinesToShipmentByCubeAndWeight(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pCube As Decimal, ByVal pWeight As Decimal, ByVal oShip As WMS.Logic.Shipment, ByVal pTargetShipment As String, ByVal pUser As String)
        Dim AcumWeight, AcumCube As Decimal
        Dim currLineWeight, currLineCube As Decimal
        Dim sql As String = String.Format("select * from vshipmentdetails where shipment ='{0}' and consignee='{1}' and orderid='{2}'", oShip.SHIPMENT, pConsignee, pOrderid)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            currLineWeight = dr("weight")
            currLineCube = dr("volume")
            If (AcumCube + currLineCube <= pCube Or pCube <= 0) And (AcumWeight + currLineWeight <= pWeight Or pWeight <= 0) Then
                oShip.MoveDetailToShipment(dr("CONSIGNEE"), dr("DOCUMENTTYPE"), dr("ORDERID"), dr("ORDERLINE"), pTargetShipment, pUser)
                AcumCube += currLineCube
                AcumWeight += currLineWeight
            End If
        Next
    End Sub

    Private Sub AssignLinesToShipmentByCubeAndWeightBYPICKREGION(ByVal pConsignee As String, ByVal pOrderid As String, ByVal PICKREGION As String, ByVal pTransClass As String, ByVal pLoadingSequence As Int32, ByVal oShip As WMS.Logic.Shipment, ByVal pUser As String)
        Dim sql As String = String.Format("select * from vShipmentAssignDetailFlowthroughDetails where consignee='{0}' and orderid='{1}' and PICKREGION='{2}'", pConsignee, pOrderid, PICKREGION)
        '
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            AssignLinesToShipmentByCubeAndWeightByORDERLINE(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TRANSPORTATIONCLASS")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Cube"), -1), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Weight"), -1), pLoadingSequence, oShip, WMS.Logic.GetCurrentUser)
        Next
    End Sub

    'Added for PWMS-586
    Private Sub AssignFlowThroughLineToShipmentByCube(ByVal shipment As WMS.Logic.Shipment, ByVal newLoadingSeq As Int32, ByVal consignee As String, ByVal orderId As String)
        Dim SQL As String
        Dim dataTable As DataTable = New DataTable()

        Dim orderLine As Int32
        Dim units As Decimal

        'SQL = "INSERT INTO  SHIPMENTDETAIL (SHIPMENT, CONSIGNEE, ORDERID, ORDERLINE, UNITS, ADDDATE, ADDUSER, EDITDATE, EDITUSER, LOADINGSEQ) " _
        '        & " SELECT '{0}',CONSIGNEE,FLOWTHROUGH,FLOWTHROUGHLINE,QTYMODIFIED,GETDATE(),'{1}',GETDATE(),'{1}',{2} FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '{3}' AND FLOWTHROUGH= '{4}' "
        'SQL = String.Format(SQL, oShip.SHIPMENT, WMS.Logic.GetCurrentUser, 1, dr("CONSIGNEE"), dr("ORDERID"))

        SQL = String.Format("SELECT FLOWTHROUGHLINE,QTYMODIFIED FROM FLOWTHROUGHDETAIL WHERE CONSIGNEE = '{0}' AND FLOWTHROUGH= '{1}' ", consignee, orderId)
        DataInterface.FillDataset(SQL, dataTable)

        For Each dataRow As DataRow In dataTable.Rows

            orderLine = dataRow("FLOWTHROUGHLINE")
            units = dataRow("QTYMODIFIED")
            shipment.AssignOrder(consignee, orderId, orderLine, units, newLoadingSeq, WMS.Logic.GetCurrentUser, WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH)
        Next

    End Sub
    'End Added for PWMS-586

    Private Sub AssignLinesToShipmentByCubeAndWeightByORDERLINE(ByVal pConsignee As String, ByVal pOrderid As String, ByVal OrderLine As String, ByVal pTransClass As String, ByVal pCube As Decimal, ByVal pWeight As Decimal, ByVal pLoadingSequence As Int32, ByVal oShip As WMS.Logic.Shipment, ByVal pUser As String)
        Dim AcumWeight, AcumCube As Decimal
        Dim currLineWeight, currLineCube As Decimal
        Dim sql As String = String.Format("select * from vshipmentassignment where consignee='{0}' and orderid='{1}' and isnull(transportationclass,'')='{2}' and isnull(shipment,'')<>'{3}' and ORDERLINE='{4}'", pConsignee, pOrderid, pTransClass, oShip.SHIPMENT, OrderLine)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            currLineWeight = dr("LineWeight")
            currLineCube = dr("LineCube")
            If (AcumCube + currLineCube <= pCube Or pCube <= 0) And (AcumWeight + currLineWeight <= pWeight Or pWeight <= 0) Then
                oShip.AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), dr("QTYOPEN"), pLoadingSequence, pUser)
                AcumCube += currLineCube
                AcumWeight += currLineWeight
            End If
        Next
    End Sub

    Private Sub AssignLinesToShipmentByCubeAndWeight(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pTransClass As String, ByVal pCube As Decimal, ByVal pWeight As Decimal, ByVal pLoadingSequence As Int32, ByVal oShip As WMS.Logic.Shipment, ByVal pUser As String)
        Dim AcumWeight, AcumCube As Decimal
        Dim currLineWeight, currLineCube As Decimal
        Dim sql As String = String.Format("select * from vshipmentassignment where consignee='{0}' and orderid='{1}' and isnull(transportationclass,'')='{2}' and isnull(shipment,'')<>'{3}'", pConsignee, pOrderid, pTransClass, oShip.SHIPMENT)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            currLineWeight = dr("LineWeight")
            currLineCube = dr("LineCube")
            If (AcumCube + currLineCube <= pCube Or pCube <= 0) And (AcumWeight + currLineWeight <= pWeight Or pWeight <= 0) Then
                oShip.AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), dr("QTYOPEN"), pLoadingSequence, pUser)
                AcumCube += currLineCube
                AcumWeight += currLineWeight
            End If
        Next
    End Sub

    Private Sub UnAssignLinesToShipmentByCubeAndWeight(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pTransClass As String, ByVal pCube As Decimal, ByVal pWeight As Decimal, ByVal oShip As WMS.Logic.Shipment, ByVal pUser As String)
        Dim AcumWeight, AcumCube As Decimal
        Dim currLineWeight, currLineCube As Decimal
        Dim sql As String = String.Format("select * from vshipmentdetails where consignee='{0}' and orderid='{1}' and isnull(transportationclass,'')='{2}' and shipment='{3}'", pConsignee, pOrderid, pTransClass, oShip.SHIPMENT)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            currLineWeight = dr("weight")
            currLineCube = dr("volume")
            If (AcumCube + currLineCube <= pCube Or pCube <= 0) And (AcumWeight + currLineWeight <= pWeight Or pWeight <= 0) Then
                oShip.DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), pUser)
                AcumCube += currLineCube
                AcumWeight += currLineWeight
            End If
        Next
    End Sub

    Private Sub UpdateLoadingSequence(ByVal pCompany As String, ByVal oShip As WMS.Logic.Shipment, ByVal pNewLoadingSequence As Int32, ByVal pUser As String, documentType As String)
        Dim sql As String = String.Format("select * from vshipmentdetails where company='{0}' and shipment='{1}'", pCompany, oShip.SHIPMENT)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        Dim oShipDet As WMS.Logic.Shipment.ShipmentDetail
        For Each dr As DataRow In dt.Rows
            oShipDet = oShip.ShipmentDetails.getShipmentDetail(dr("consignee"), dr("orderid"), dr("orderline"), documentType)
            oShipDet.SetLoadingSequence(pNewLoadingSequence, pUser)
        Next
    End Sub

    Private Function GetLoadingSequence(ByVal pShipment As String, ByVal pReversedLoadingSeq As Int32) As Int32
        Dim loadingSeq As Int32 = 0
        Dim sql As String = String.Format("select * from vShipmentDetailLoadingSeq where shipment = '{0}'", pShipment)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return 1
        Else
            Dim maxSeq As Int32 = dt.Rows(0)("highestloadingseq")
            Dim minSeq As Int32 = dt.Rows(0)("lowestloadingseq")
            If maxSeq = 0 And minSeq = 0 Then
                Return pReversedLoadingSeq
            End If
            If pReversedLoadingSeq <= maxSeq And pReversedLoadingSeq >= minSeq Then
                Return maxSeq + minSeq - pReversedLoadingSeq
            ElseIf pReversedLoadingSeq > maxSeq Then
                Return minSeq
            ElseIf pReversedLoadingSeq < minSeq Then
                Return maxSeq
            End If
        End If
    End Function

    Private Function GetLoadingSequenceByCompany(ByVal pShipment As String, ByVal pCompany As String) As Int32
        Dim loadingSeq As Int32 = 0
        Dim sql As String = String.Format("select * from vshipmentdetails where shipment = '{0}' and company='{1}'", pShipment, pCompany)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Return -1
        Else
            Return dt.Rows(0)("loadingseq")
        End If
    End Function

    'Added for RWMS-2138(RWMS-2017) Start
#Region "AdDock"

    Private Function IsBelongFlowthroughToShipment(ByVal Shipment As String) As Boolean
        Dim sql As String = String.Format("select count(1) from vShipmentFlowthrough where Shipment='{0}' ", Shipment)

        If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) = 0 Then
            Return False
        Else
            Return True
        End If

    End Function

    Private Function ValidateFlowthroughShipment(ByVal Shipment As String) As Boolean
        Dim sql As String = String.Format("select count(1) from vShipmentFlowthrough where Shipment='{0}' and (door='' or STAGINGLANE='') ", Shipment)

        If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) = 0 Then
            Return True
        Else
            Return False
        End If

    End Function


    Private Function FlowthroughLoadDeliver(ByVal Shipment As String) As Boolean

        Dim sql As String = String.Format("SELECT STAGINGLANE, LOADID, STAGINGWAREHOUSEAREA from vShipmentFlowthrough where Shipment='{0}' and (door<>'' and STAGINGLANE<>'') ", Shipment)
        Dim dt As New DataTable
        'Dim fl As WMS.Logic.Load
        Dim oStaging As New WMS.Logic.Staging
        Dim pUser As String = WMS.Logic.Common.GetCurrentUser
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        Dim oLoad As WMS.Logic.Load
        Dim oDelTask As WMS.Logic.DeliveryTask

        For Each dr As DataRow In dt.Rows
            oLoad = New WMS.Logic.Load(dr("LOADID").ToString())
            'If oLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.STAGED Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Load is already staged.", "Load is already staged.")
            'End If
            'If oLoad.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.LOADED Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Load is already loaded.", "Load is already loaded.")
            'End If

            oLoad.SetDestinationLocation(dr("STAGINGLANE"), dr("STAGINGWAREHOUSEAREA"), pUser)
            oDelTask = New WMS.Logic.DeliveryTask
            CreateLoadDeliveryTask(oDelTask, oLoad.LOADID, dr("STAGINGLANE"), dr("STAGINGWAREHOUSEAREA"), pUser)

            '            oDelTask.CreateLoadDeliveryTask(oLoad.LOADID, dr("STAGINGLANE"), dr("STAGINGWAREHOUSEAREA"), pUser)

            ' oStaging.StageLoad(dr("loadid"), WMS.Logic.GetCurrentUser)
        Next


    End Function

    <CLSCompliant(False)>
    Public Sub CreateLoadDeliveryTask(ByVal oDelTask As WMS.Logic.DeliveryTask, ByVal pLoadId As String, ByVal pDestinationLocation As String, ByVal pDestinationWarehousearea As String, ByVal pUser As String, Optional ByVal pPicklistId As String = "")
        If Not WMS.Logic.Load.Exists(pLoadId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Deliver load, Load does not exists", "Cannot Deliver load, Load does not exists")
        End If
        'Check if a task already exist
        Dim oTask As Task = DeliveryExists(WMS.Lib.TASKTYPE.LOADDELIVERY, pLoadId, pUser)
        If Not oTask Is Nothing Then
            Return
        End If
        Dim oLoad As New WMS.Logic.Load(pLoadId)
        If oLoad.DESTINATIONLOCATION = String.Empty Or oLoad.DESTINATIONWAREHOUSEAREA = String.Empty Then
            oLoad.SetDestinationLocation(pDestinationLocation, pDestinationWarehousearea, pUser)
        End If

        oDelTask.Picklist = pPicklistId
        CreateLoadDeliveryTask(oDelTask, oLoad, pUser)
    End Sub
    Private Function DeliveryExists(ByVal pDeliveryType As String, ByVal pPalletId As String, ByVal pUserId As String) As Task
        Dim oTaskManager As New WMS.Logic.TaskManager()
        Dim oTask As Task = oTaskManager.RequestDeliveryTask(pUserId, pDeliveryType, pPalletId)
        If Not oTask Is Nothing Then
            oTask.Post(pUserId)
        End If
        Return oTask
    End Function
    Private Sub CreateLoadDeliveryTask(ByVal oDelTask As WMS.Logic.DeliveryTask, ByVal ld As WMS.Logic.Load, ByVal puser As String)
        oDelTask.CONSIGNEE = ld.CONSIGNEE
        oDelTask.FROMLOAD = ld.LOADID
        oDelTask.FROMLOCATION = ld.LOCATION
        oDelTask.FROMWAREHOUSEAREA = ld.WAREHOUSEAREA
        oDelTask.PRIORITY = 200
        oDelTask.SKU = ld.SKU
        oDelTask.TASKTYPE = WMS.Lib.TASKTYPE.LOADDELIVERY
        oDelTask.TOLOAD = ld.LOADID
        oDelTask.TOLOCATION = ld.DESTINATIONLOCATION
        oDelTask.TOWAREHOUSEAREA = ld.DESTINATIONWAREHOUSEAREA

        oDelTask.Post()
    End Sub


#End Region

    'Added for RWMS-2138(RWMS-2017) End

    'Added for RWMS-2340(RWMS-2339) Start
    Public Sub SetAtDock(ByVal pUserId As String, ByVal shipId As String, ByVal status As String)
        If status = WMS.Lib.Statuses.Shipment.STATUSNEW OrElse status = WMS.Lib.Statuses.Shipment.ASSIGNED OrElse status = WMS.Lib.Statuses.Shipment.SHEDULED OrElse _
        status = WMS.Lib.Statuses.Shipment.LOADED OrElse status = WMS.Lib.Statuses.Shipment.LOADING Then
            Dim oldStatus, SQL As String
            oldStatus = status
            status = WMS.Lib.Statuses.Shipment.ATDOCK
            SQL = String.Format("UPDATE SHIPMENT SET STATUS ='{0}', EDITDATE ={1}, EDITUSER ={2} WHERE shipment={3}", status, Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(pUserId), Made4Net.Shared.Util.FormatField(shipId))
            DataInterface.RunSQL(SQL)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipmentAtDock)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPATDOCK)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", shipId)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", oldStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", status)
            aq.Add("USERID", pUserId)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUserId)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUserId)
            aq.Send(WMS.Lib.Actions.Audit.SHIPATDOCK)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Shipment Status Incorrect", "Shipment Status Incorrect")
        End If
    End Sub
    'Added for RWMS-2340(RWMS-2339) End
#End Region

#Region "Handlers"

    Private Sub TEYardEntry_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEYardEntry.CreatedChildControls
        With TEYardEntry
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                If TEYardEntry.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addyardentry"
                Else
                    .CommandName = "edityardentry"
                End If
            End With
        End With
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub TEAssignOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignOrders.CreatedChildControls
        TEAssignOrders.ActionBar.AddSpacer()
        TEAssignOrders.ActionBar.AddExecButton("AssignOrders", "Assign Order Line to Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignOrders.ActionBar.Button("AssignOrders")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            '.ObjectDLL = "WMS.Logic.dll"
            '.ObjectName = "WMS.Logic.Shipment"
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Shipment"
        End With
        'TEAssignOrders.ActionBar.AddSpacer()
        'TEAssignOrders.ActionBar.AddExecButton("AssignOrderLineFromShipment", "Move from shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        'With TEAssignOrders.ActionBar.Button("AssignOrderLineFromShipment")
        '    .ObjectDLL = "WMS.WebApp.dll"
        '    .ObjectName = "WMS.WebApp.Shipment"
        '    .CommandName = "AssignOrderLineFromShipment"
        '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '    .ConfirmRequired = True
        '    .ConfirmMessage = "Are you sure you want to move the selected lines to shipment?"
        'End With

    End Sub

    Private Sub TEShipmentOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShipmentOrders.CreatedChildControls

        With TEShipmentOrders.ActionBar
            .AddSpacer()
            .AddExecButton("DeAssignOrders", "UnAssign Orders From Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("DeAssignOrders")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Shipment"
            End With
            .AddSpacer()
            .AddExecButton("MoveToShipment", "Move To Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("MoveToShipment")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "MoveToShipment"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to move the selected lines from shipment?"
            End With
        End With
    End Sub

    Private Sub TEShipmentDetails_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShipmentDetails.CreatedChildControls
        With TEShipmentDetails.ActionBar
            .AddSpacer()
            .AddExecButton("UpdateLoadingSequence", "Update Loading Sequence", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("UpdateLoadingSequence")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "UpdateLoadingSequence"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to set new loading sequence?"
            End With
            .AddSpacer()
            .AddExecButton("MoveToShipmentByCube", "Move To Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("MoveToShipmentByCube")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "MoveToShipmentByCube"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to move the selected lines from shipment?"
            End With
            .AddSpacer()
            .AddExecButton("UnAssignFromShipmentByCube", "UnAssign From Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
            With .Button("UnAssignFromShipmentByCube")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "UnAssignFromShipmentByCube"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to unassign the selected lines from shipment?"
            End With
        End With
    End Sub

    Private Sub TEMasterShipment_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterShipment.AfterItemCommand
        If TEMasterShipment.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            TEMasterShipment.Restart()
        End If
    End Sub
    Private Sub TEMasterShipment_BeforeDataChanged(ByVal sender As Object, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterShipment.BeforeDataChanged
        Dim shipId As Object = Session("HUTRANSSHIPMENTID")
        Dim shipmntDetail As New Logic.Shipment.ShipmentDetail
        Dim Message As String
        If e.UpdateType = Made4Net.WebControls.RecordUpdateType.Delete Then
            e.CancelDBUpdate = shipmntDetail.CanDeleteShipment(shipId, Message)
            If e.CancelDBUpdate Then
                Screen1.Warn(Message)
            End If
        End If
    End Sub
    Private Sub TEMasterShipment_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterShipment.RecordSelected
        Dim tds As DataTable = TEMasterShipment.CreateDataTableForSelectedRecord(False)
        Dim vals As New Specialized.NameValueCollection
        vals.Add("SHIPID", tds.Rows(0)("SHIPMENT"))
        TEAssignOrders.PreDefinedValues = vals
        'TEYardEntry.PreDefinedValues = vals
        Session("HUTRANSSHIPMENTID") = tds.Rows(0)("SHIPMENT")
    End Sub

    Private Sub TEShipmentOrders_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEShipmentOrders.AfterItemCommand
        If e.CommandName = "DeAssignOrders" Then
            TEShipmentOrders.RefreshData()
            TEMasterShipment.RefreshData()
        End If
    End Sub

    Private Sub TEAssignOrders_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAssignOrders.AfterItemCommand
        If e.CommandName = "AssignOrders" Then
            TEAssignOrders.RefreshData()
            TEMasterShipment.RefreshData()
        End If
    End Sub

    Private Sub TEMasterShipment_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterShipment.CreatedChildControls
        Dim whSQL As String = "SELECT COUNT(1) FROM WAREHOUSEPARAMS where PARAMNAME = 'AutoShip' and PARAMVALUE='1'"
        whSQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL)
        With TEMasterShipment
            With .ActionBar

                .AddSpacer()
                .AddExecButton("AtDock", "At Dock", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("AtDock")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Shipment"
                    .CommandName = "AtDock"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to update the shipment's status?"
                End With



                If whSQL = "1" Then   'AutoShip enabled in Warehouse Params , we will display the button on screen
                    .AddSpacer()
                    .AddExecButton("AutoShip", "Auto Ship", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
                    With .Button("AutoShip")
                        .ObjectDLL = "WMS.WebApp.dll"
                        .ObjectName = "WMS.WebApp.Shipment"
                        .CommandName = "AutoShip"
                        .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                        .ConfirmRequired = True
                        .ConfirmMessage = "Are you sure you want to auto ship the shipment?"
                    End With
                    'Else
                    '    With .Button("AutoShip")
                    '        .Enabled = False
                    '    End With
                End If


                .AddSpacer()
                .AddExecButton("Ship", "Ship Shipments", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
                With .Button("Ship")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Shipment"
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.Shipment"
                    .CommandName = "Ship"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to ship the selected Shipments?"
                End With

                With .Button("Save")
                    If TEMasterShipment.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .CommandName = "Update"
                    End If
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.Shipment"
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Shipment"
                End With
                .AddExecButton("CancelShipment", "Cancel Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
                With .Button("CancelShipment")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Shipment"
                    .CommandName = "CancelShip"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected shipment/s?"
                End With
                .AddExecButton("CompleteLoading", "Complete Loading", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("CompleteLoading")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Shipment"
                    .CommandName = "CompleteLoading"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to complete loading shipment/s?"
                End With

                .AddExecButton("PrintShipment", "Print Shipping Manifest", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintShipment")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Shipment"
                    .CommandName = "PrintShipment"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
                .AddExecButton("PrintShipmentPackingList", "Print Outbound Delivery Notes", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintShipmentPackingList")
                    '.ObjectDLL = "WMS.Logic.dll"
                    '.ObjectName = "WMS.Logic.Shipment"
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Shipment"
                    .CommandName = "PrintShipmentPackingList"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
                .AddExecButton("PrintLoadingWorksheet", "Print Loading Worksheet", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintLoadingWorksheet")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Shipment"
                    .CommandName = "PrintLoadingWorksheet"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
                .AddExecButton("PrintLoadingPlan", "Print Loading Plan", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintLoadingPlan")
                    .ObjectDLL = "RWMS.Logic.dll"
                    .ObjectName = "RWMS.Logic.Shipment"
                    .CommandName = "PrintLoadingPlan"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
            End With
        End With
    End Sub

    Private Sub TEHUTrans_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEHUTrans.CreatedChildControls
        With TEHUTrans
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                If TEHUTrans.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addhu"
                Else
                    .CommandName = "edithu"
                End If
            End With
        End With
    End Sub

    Private Sub TEShipmentLoads_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShipmentLoads.CreatedChildControls
        With TEShipmentLoads.ActionBar
            .AddSpacer()
            .AddExecButton("Unload", "Unload", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("Unload")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "Unload"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to unload the selected lines?"
            End With
        End With
    End Sub


    'Added for RWMS-2343 RWMS-2314
    Private Sub TEShipmentContainers_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShipmentContainers.CreatedChildControls
        With TEShipmentContainers.ActionBar
            .AddSpacer()
            .AddExecButton("Unload", "Unload", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("Unload")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "UnloadContainers"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to unload the selected lines?"
            End With
        End With
    End Sub
    'End Added for RWMS-2343 RWMS-2314

    Private Sub TEShipmentAssignDetails_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShipmentAssignDetails.CreatedChildControls
        With TEShipmentAssignDetails.ActionBar
            .AddSpacer()
            .AddExecButton("AssignToShipmentByCube", "Assign To Shipment", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AssignToShipmentByCube")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Shipment"
                .CommandName = "AssignToShipmentByCube"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to assign the selected lines to shipment?"
            End With
        End With
    End Sub

#End Region

End Class