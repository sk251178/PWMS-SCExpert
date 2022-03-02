Imports WMS.Logic
Imports System.Data

Public Class AppUtil

    Public Shared Sub UpdateReceiptLineAverageWeight(ByVal receipt As String, ByVal ReceiptLine As String)

        Dim Sql As String
        Sql = " UPDATE RECEIPTDETAIL "
        Sql += " SET  AVGWEIGHT= RECEIVEDWEIGHT / (QTYRECEIVED / dbo.SKUUOM.UNITSPERLOWESTUOM) "
        Sql += " FROM dbo.RECEIPTDETAIL LEFT OUTER JOIN "
        Sql += " dbo.SKUUOM ON dbo.RECEIPTDETAIL.SKU = dbo.SKUUOM.SKU AND dbo.RECEIPTDETAIL.CONSIGNEE = dbo.SKUUOM.CONSIGNEE "
        Sql += " WHERE (dbo.RECEIPTDETAIL.RECEIPT = '{0}') and dbo.RECEIPTDETAIL.RECEIPTline = '{1}' AND (dbo.SKUUOM.UOM = N'CASE') "
        Sql += " AND QTYRECEIVED>0 AND dbo.SKUUOM.UNITSPERLOWESTUOM>0 "
        Try
            Sql = String.Format(Sql, receipt, ReceiptLine)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

        Catch ex As Exception
        End Try
    End Sub

    Public Shared Function isBackLocMoveFront(ByVal toLOADID As String, ByVal toLock As String, ByVal toWarehouse As String, ByRef frontLoc As String, ByRef ErrMessage As String) As Boolean
        Dim sql As String
        Dim ret As Boolean = False
        'sql = String.Format("select FRONTRACKLOCATION from vLocationFlowRack where (FRONTRACKLOCATION <> '' AND NOT FRONTRACKLOCATION IS NULL) and LOCATION='{0}' and WAREHOUSEAREA='{1}'", toLock, toWarehouse)
        sql = String.Format("select FRONTRACKLOCATION from vLocationFlowRack  WHERE flowracktype='BACKRACK' and LOCATION='{0}' and WAREHOUSEAREA='{1}'", toLock, toWarehouse)
        '
        Try
            sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If String.IsNullOrEmpty(sql) Then
                ret = True
            ElseIf WMS.Logic.Location.Exists(sql, toWarehouse) And sql <> toLOADID Then
                frontLoc = sql
                If WMS.Logic.Load.Exists(toLOADID) Then
                    Dim ld As New WMS.Logic.Load(toLOADID)
                    If ld.STATUS <> "" Then
                        'ld.Move(frontLoc, toWarehouse, "", WMS.Logic.GetCurrentUser)
                        Move(toLOADID, frontLoc, toWarehouse, "", WMS.Logic.GetCurrentUser)
                    End If
                End If
                ret = True
            Else
                ErrMessage = " Error FRONTRACKLOCATION " & sql & ";"
            End If
        Catch ex As Exception
            ErrMessage = ex.Message
        End Try

        Return ret
    End Function

    Public Shared Function Move(ByVal LOADID As String, ByVal pNewLoc As String, ByVal pNewWarehousearea As String, ByVal pSubLocation As String, ByVal pUser As String) As Boolean

        If Not WMS.Logic.Location.Exists(pNewLoc, pNewWarehousearea) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot relocate load", "Cannot relocate load")
            Throw m4nEx
        End If
        Dim ld As New WMS.Logic.Load(LOADID)
        If Not ld.isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exist", "Load does not exist")
            Throw m4nEx
        End If
        If ld.hasActivity() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load location", "Cannot Change load location")
            Throw m4nEx
        End If
        If ld.UNITSALLOCATED > 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load location qty allocated", "Cannot Change load location qty allocated")
            Throw m4nEx
        End If
        Dim strOldLoc As String = ld.LOCATION
        Dim strOldWarehousearea As String = ld.WAREHOUSEAREA

        ld.Put(pNewLoc, pNewWarehousearea, pSubLocation, pUser)

        'Dim oWHActivity As New WHActivity()
        'oWHActivity.LOCATION = pNewLoc
        'oWHActivity.USERID = pUser
        'oWHActivity.ACTIVITY = WMS.Lib.Actions.Audit.MOVELOAD
        'oWHActivity.ACTIVITYTIME = DateTime.Now
        'oWHActivity.EDITDATE = DateTime.Now
        'oWHActivity.EDITUSER = pUser
        'oWHActivity.Post()

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadLocationChanged)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.MOVELOAD)
        aq.Add("CONSIGNEE", ld.CONSIGNEE)
        aq.Add("DOCUMENT", ld.RECEIPT)
        aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)
        aq.Add("FROMLOAD", ld.LOADID)
        aq.Add("FROMLOC", strOldLoc)
        aq.Add("FROMWAREHOUSEAREA", strOldWarehousearea)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)
        aq.Add("NOTES", "")
        aq.Add("SKU", ld.SKU)
        aq.Add("TOLOAD", ld.LOADID)
        aq.Add("TOLOC", ld.LOCATION)
        aq.Add("TOWAREHOUSEAREA", ld.WAREHOUSEAREA)
        aq.Add("TOQTY", ld.UNITS)
        aq.Add("TOSTATUS", ld.STATUS)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))


        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.MOVELOAD)


    End Function
    '
    Public Shared Function deleteInboundLine(ByVal consignee As String, ByVal orderid As String, ByVal orderline As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("select COUNT(*) from INBOUNDORDDETAIL where CONSIGNEE = '{0}' AND ORDERID = '{1}' and orderline='{2}' and QTYRECEIVED>0", consignee, orderid, orderline)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot delete line with received quantity") '& " " & orderid
            Return False
        End If


        strsql = String.Format("SELECT  COUNT(*) FROM  dbo.INBOUNDORDHEADER AS ih INNER JOIN dbo.RECEIPTDETAIL AS rd ON ih.CONSIGNEE = rd.CONSIGNEE AND ih.ORDERID = rd.ORDERID INNER JOIN dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT WHERE (NOT (rh.STATUS IN ('{2}', '{3}'))) AND (ih.CONSIGNEE = '{0}') AND (ih.ORDERID = '{1}') and rd.orderline='{4}'", consignee, orderid, WMS.Lib.Statuses.Receipt.CANCELLED, WMS.Lib.Statuses.Receipt.CLOSED, orderline)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot delete line with open receipt lines") '& " " & orderid
            Return False
            'Throw New ApplicationException(Message)
        End If
        Return ret
    End Function



    Public Shared Function deleteInbound(ByVal consignee As String, ByVal orderid As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("select COUNT(*) from INBOUNDORDDETAIL where CONSIGNEE = '{0}' AND ORDERID = '{1}' and QTYRECEIVED>0", consignee, orderid)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot delete order with received quantity") '& " " & orderid
            Return False
        End If


        strsql = String.Format("SELECT  COUNT(*) FROM  dbo.INBOUNDORDHEADER AS ih INNER JOIN dbo.RECEIPTDETAIL AS rd ON ih.CONSIGNEE = rd.CONSIGNEE AND ih.ORDERID = rd.ORDERID INNER JOIN dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT WHERE (NOT (rh.STATUS IN ('{2}', '{3}'))) AND (ih.CONSIGNEE = '{0}') AND (ih.ORDERID = '{1}')", consignee, orderid, WMS.Lib.Statuses.Receipt.CANCELLED, WMS.Lib.Statuses.Receipt.CLOSED)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot delete order with open receipt lines") '& " " & orderid
            Return False
            'Throw New ApplicationException(Message)
        End If
        Return ret
    End Function

    Public Shared Function cancelInbound(ByVal consignee As String, ByVal orderid As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim oOrd As New WMS.Logic.InboundOrderHeader(consignee, orderid)
        If oOrd.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
            Message = t.Translate("Cannot cancel a closed order [order ###]")
            Message = Message.Replace("###", orderid)
            Return False
        End If
        If oOrd.STATUS = WMS.Lib.Statuses.InboundOrderHeader.RECEIVING Then
            Message = t.Translate("Cannot cancel an order with open receipt lines [order ###]")
            Message = Message.Replace("###", orderid)
            Return False
        End If
        strsql = String.Format("SELECT  COUNT(*) FROM  dbo.INBOUNDORDHEADER AS ih INNER JOIN dbo.RECEIPTDETAIL AS rd ON ih.CONSIGNEE = rd.CONSIGNEE AND ih.ORDERID = rd.ORDERID INNER JOIN dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT WHERE (NOT (rh.STATUS IN ('{2}', '{3}'))) AND (ih.CONSIGNEE = '{0}') AND (ih.ORDERID = '{1}')", consignee, orderid, WMS.Lib.Statuses.Receipt.CANCELLED, WMS.Lib.Statuses.Receipt.CLOSED)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot cancel an order with open receipt lines [order ###]")
            Message = Message.Replace("###", orderid)
            Return False
        End If
        Return ret
    End Function


    Public Shared Function cancelFlowthrough(ByVal consignee As String, ByVal orderid As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        strsql = String.Format("select COUNT(*) from FLOWTHROUGHDETAIL where CONSIGNEE = '{0}' AND FLOWTHROUGH = '{1}' and QTYRECEIVED>0", consignee, orderid)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot cancel an order with received quantity [order ###]")
            Message = Message.Replace("###", orderid)
            Return False
        End If

        strsql = String.Format("SELECT  COUNT(*) FROM  dbo.FLOWTHROUGHHEADER AS ih INNER JOIN dbo.RECEIPTDETAIL AS rd ON ih.CONSIGNEE = rd.CONSIGNEE AND ih.FLOWTHROUGH = rd.ORDERID INNER JOIN dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT WHERE (NOT (rh.STATUS IN ('{2}', '{3}'))) AND (ih.CONSIGNEE = '{0}') AND (ih.FLOWTHROUGH = '{1}')", consignee, orderid, WMS.Lib.Statuses.Receipt.CANCELLED, WMS.Lib.Statuses.Receipt.CLOSED)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot cancel an order with open receipt lines [order ###]")
            Message = Message.Replace("###", orderid)
            Return False
        End If

        Return ret
    End Function



    Public Shared Function closeInbound(ByVal consignee As String, ByVal orderid As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("SELECT  COUNT(*) FROM  dbo.INBOUNDORDHEADER AS ih INNER JOIN dbo.RECEIPTDETAIL AS rd ON ih.CONSIGNEE = rd.CONSIGNEE AND ih.ORDERID = rd.ORDERID INNER JOIN dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT WHERE (NOT (rh.STATUS IN ('{2}', '{3}'))) AND (ih.CONSIGNEE = '{0}') AND (ih.ORDERID = '{1}')", consignee, orderid, WMS.Lib.Statuses.Receipt.CANCELLED, WMS.Lib.Statuses.Receipt.CLOSED)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Cannot close order with open receipt lines [order ###]")
            Message = Message.Replace("###", orderid)
            ret = False
            'Throw New ApplicationException(Message)
        Else
            Dim inh As New WMS.Logic.InboundOrderHeader(consignee, orderid)
            Dim fromStatus As String = inh.STATUS

            strsql = String.Format("update INBOUNDORDHEADER set STATUS ='{3}',editdate=getdate(),edituser='{2}' where CONSIGNEE = '{0}' AND ORDERID = '{1}'", consignee, orderid, WMS.Logic.GetCurrentUser, WMS.Lib.Statuses.InboundOrderHeader.CLOSED)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)

            strsql = String.Format("update INBOUNDORDDETAIL set qtyadjusted = qtyreceived,editdate=getdate(),edituser='{0}' where CONSIGNEE = '{0}' AND ORDERID = '{1}'", consignee, orderid, WMS.Logic.GetCurrentUser)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundUpdated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDHUPD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", consignee)
            aq.Add("DOCUMENT", orderid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", fromStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", WMS.Lib.Statuses.InboundOrderHeader.CLOSED)
            aq.Add("USERID", WMS.Logic.GetCurrentUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
            aq.Send(WMS.Lib.Actions.Audit.INBOUNDHUPD)
        End If

        Return ret
    End Function

    Public Shared Function IsCreateLoad(ByVal receipt As String, ByVal receiptLine As String, ByRef Message As String, Optional ByVal RecCheck As Boolean = False)
        Dim ret As Boolean = True

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim rd As New WMS.Logic.ReceiptDetail(receipt, receiptLine)
        If String.IsNullOrEmpty(rd.ORDERID) Then
            Return ret
        End If

        '   Dim ih As New WMS.Logic.InboundOrderHeader(rd.CONSIGNEE, rd.ORDERID)

        If RecCheck Then
            Dim rh As New WMS.Logic.ReceiptHeader(receipt)
            If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                Message = t.Translate("Receipt is closed, cannot create payload")
                Return False
            End If
            If rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
                Message = t.Translate("Receipt is cancelled, cannot create payload")
                Return False
            End If
        End If

        If rd.DOCUMENTTYPE = WMS.Lib.DOCUMENTTYPES.INBOUNDORDER Then
            If WMS.Logic.InboundOrderHeader.Exists(rd.CONSIGNEE, rd.ORDERID) Then
                Dim inh As New WMS.Logic.InboundOrderHeader(rd.CONSIGNEE, rd.ORDERID)
                If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
                    Message = t.Translate("inbound order closed, cannot create payload")
                    Return False
                End If
                If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Then
                    Message = t.Translate("inbound order canceled, cannot create payload")
                    Return False
                End If
            End If
        ElseIf rd.DOCUMENTTYPE = WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH Then
            If WMS.Logic.Flowthrough.Exists(rd.CONSIGNEE, rd.ORDERID) Then
                Dim inh As New WMS.Logic.Flowthrough(rd.CONSIGNEE, rd.ORDERID)
                If inh.STATUS = WMS.Lib.Statuses.Flowthrough.CANCELED Then
                    Message = t.Translate("Flowthrough order canceled, cannot create payload")
                    Return False
                End If
            End If
        End If

        Return ret
    End Function
    Public Shared Function IsReceiptAverage(ByVal receipt As String, ByVal receiptLine As String, ByRef Message As String, ByRef AVGWEIGHT As Decimal)
        Dim ret As Boolean = True

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim strsql As String
        Dim dt As New DataTable

        strsql = String.Format("SELECT RECEIPT, RECEIPTLINE, CONSIGNEE, SKU, RECEIVINGWEIGHTCAPTUREMETHOD, AVGWEIGHT, ATTRIBUTENAME FROM vReceiptAverage where RECEIPT='{0}' and RECEIPTLINE='{1}'", receipt, receiptLine)
        Made4Net.DataAccess.DataInterface.FillDataset(strsql, dt)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("AVGWEIGHT") = 0 Then
                Message = t.Translate("Receipt Average Weight Not Entered")
                ret = False
            Else
                AVGWEIGHT = dt.Rows(0)("AVGWEIGHT")
            End If
        Else

        End If
        Return ret
    End Function
    'Added for RWMS-1484 and RWMS-1402      
    Public Shared Function CloseInboundByReceiptCancel(ByVal receipt As String) As Boolean
        Dim ret As Boolean = True
        'Start RWMS-978   
        Dim sql As String = "SELECT CONSIGNEE, ORDERID FROM vInboundQtyReceived WHERE QTYADJUSTED <= (QTYRECEIVED + QTYRECEXCEPTION) AND RECEIPT = '{0}'"
        'End RWMS-978   
        sql = String.Format(sql, receipt)
        Dim DT As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)
        If DT.Rows.Count > 0 Then
            For Each DR1 As DataRow In DT.Rows
                ret = AppUtil.closeInboundOnReceiptcancel(DR1("CONSIGNEE"), DR1("ORDERID"))
            Next
        End If
        Return ret
    End Function

    Public Shared Function closeInboundOnReceiptcancel(ByVal consignee As String, ByVal orderid As String) As Boolean
        Dim ret As Boolean = True
        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("SELECT COUNT(*) FROM dbo.INBOUNDORDHEADER AS ih INNER JOIN dbo.RECEIPTDETAIL AS rd ON ih.CONSIGNEE = rd.CONSIGNEE AND ih.ORDERID = rd.ORDERID INNER JOIN dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT WHERE (NOT (rh.STATUS IN ('{2}', '{3}'))) AND (ih.CONSIGNEE = '{0}') AND (ih.ORDERID = '{1}')", consignee, orderid, WMS.Lib.Statuses.Receipt.CANCELLED, WMS.Lib.Statuses.Receipt.CLOSED)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            ret = False
        Else
            Dim inh As New WMS.Logic.InboundOrderHeader(consignee, orderid)
            Dim fromStatus As String = inh.STATUS

            strsql = String.Format("update INBOUNDORDHEADER set STATUS ='{3}',editdate=getdate(),edituser='{2}' where CONSIGNEE = '{0}' AND ORDERID = '{1}'", consignee, orderid, WMS.Logic.GetCurrentUser, WMS.Lib.Statuses.InboundOrderHeader.CLOSED)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)

            strsql = String.Format("update INBOUNDORDDETAIL set qtyadjusted = qtyreceived,editdate=getdate(),edituser='{0}' where CONSIGNEE = '{0}' AND ORDERID = '{1}'", consignee, orderid, WMS.Logic.GetCurrentUser)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundUpdated)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDHUPD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", consignee)
            aq.Add("DOCUMENT", orderid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", fromStatus)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", WMS.Lib.Statuses.InboundOrderHeader.CLOSED)
            aq.Add("USERID", WMS.Logic.GetCurrentUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
            aq.Send(WMS.Lib.Actions.Audit.INBOUNDHUPD)
        End If

        Return ret
    End Function
   
    'Ended for RWMS-1484 and RWMS-1402     

    Public Shared Function CloseInboundByReceipt(ByVal receipt As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True
        'Commented for PWMS-919(RWMS-978) Start
        'Dim sql As String = "SELECT CONSIGNEE, ORDERID FROM vInboundQtyReceived WHERE  QTYADJUSTED <= QTYRECEIVED AND RECEIPT = '{0}'"
        'Commented for PWMS-919(RWMS-978) End

        'Added for PWMS-919(RWMS-978) Start
        Dim sql As String = "SELECT CONSIGNEE, ORDERID FROM vInboundQtyReceived WHERE  QTYADJUSTED <= (QTYRECEIVED + QTYRECEXCEPTION) AND RECEIPT = '{0}'"
        'Added for PWMS-919(RWMS-978) Start
        sql = String.Format(sql, receipt)
        Dim DT As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)

        For Each DR1 As DataRow In DT.Rows
            ret = AppUtil.closeInbound(DR1("CONSIGNEE"), DR1("ORDERID"), Message)
        Next
        Return ret

    End Function

    Public Shared Function WeightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        If Not pSKU.SKUClass Is Nothing Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If (oAtt.Name.ToUpper = "WEIGHT" Or oAtt.Name.ToUpper = "WGT") AndAlso _
                (oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function


    Public Shared Function SetReceivedWeight(ByVal consignee As String, ByVal sku As String, ByVal receipt As String, ByVal receiptLine As Integer) As Decimal

        Dim sql As String = String.Format("SELECT TotalWEIGHT from  ReceiptLineTotalWeight where RECEIPT='{0}' and RECEIPTLINE='{1}'", receipt, receiptLine)
        Try
            Dim oSku As New WMS.Logic.SKU(consignee, sku)
            If WeightNeeded(oSku) Then

                sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

                'Below update statement commented as the running average weight is not calculating accurately due to total weight insteasd of receipt line weight
                'sql = String.Format("update RECEIPTDETAIL set RECEIVEDWEIGHT = '{2}' where RECEIPT='{0}' and RECEIPTLINE='{1}'", receipt, receiptLine, sql)
                'Made4Net.DataAccess.DataInterface.RunSQL(sql)
            End If

        Catch ex As Exception
        End Try
    End Function

    Public Shared Sub UpdateReceiptAverageWeight(ByVal receipt As String)

        Dim Sql As String
        Sql = " UPDATE RECEIPTDETAIL "
        Sql += " SET  AVGWEIGHT= RECEIVEDWEIGHT / (QTYRECEIVED / dbo.SKUUOM.UNITSPERLOWESTUOM) "
        Sql += " FROM dbo.RECEIPTDETAIL LEFT OUTER JOIN "
        Sql += " dbo.SKUUOM ON dbo.RECEIPTDETAIL.SKU = dbo.SKUUOM.SKU AND dbo.RECEIPTDETAIL.CONSIGNEE = dbo.SKUUOM.CONSIGNEE "
        Sql += " WHERE (dbo.RECEIPTDETAIL.RECEIPT = '{0}') AND (dbo.SKUUOM.UOM = N'CASE') "
        Sql += " AND QTYRECEIVED>0 AND dbo.SKUUOM.UNITSPERLOWESTUOM>0 "
        Try
            Sql = String.Format(Sql, receipt)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

        Catch ex As Exception
        End Try
    End Sub


    Public Shared Function ChangeLocationValidationRegularLocation(ByVal Location As String, ByVal Warehouse As String, ByRef Consignee As String, ByVal Sku As String, ByRef ErrMessage As String) As Boolean
        Dim sql As String
        Dim ret As Boolean = False
        sql = String.Format("select COUNT(1) from PICKLOC where LOCATION = '{0}' and WAREHOUSEAREA='{1}'", Location, Warehouse)
        '
        Try
            If Not WMS.Logic.Location.Exists(Location, Warehouse) Then
                ErrMessage = "Location does not exist"
                Return False
            End If
            sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            'if loc does not exist in pickloc - location is pass validation
            If sql = "0" Then
                GoTo CheckLocStatus
            Else
                sql = String.Format("select COUNT(1) from PICKLOC where LOCATION = '{0}' and WAREHOUSEAREA='{1}' and CONSIGNEE='{2}' and SKU='{3}'", Location, Warehouse, Consignee, Sku)
                sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                'if loc not exist in pickloc but not for curent sku- location is NOT pass validation
                If sql = "0" Then
                    ErrMessage = "Pick location does not belong to this SKU"
                    Return False
                Else
CheckLocStatus:

                    sql = "select STATUS,PROBLEMFLAG from LOCATION where  LOCATION = '{0}' and WAREHOUSEAREA='{1}' "
                    sql = String.Format(sql, Location, Warehouse)
                    Dim DT As New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)
                    For Each DR1 As DataRow In DT.Rows
                        'Start PWMS-875 and RWMS-927   
                        If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DR1("STATUS")) = False Then
                            'End PWMS-875 and RWMS-927  
                            ErrMessage = "Inactive location"
                            Return False
                            'Start PWMS-875 and RWMS-927   
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DR1("PROBLEMFLAG")) = True Then
                            'End PWMS-875 and RWMS-927   
                            ErrMessage = "The location has a problem"
                            Return False
                        Else
                            Return True
                        End If
                    Next
                End If

            End If
        Catch ex As Exception
            ErrMessage = ex.Message
        End Try
        Return ret
    End Function

    Public Shared Function ChangeLocationValidation(ByVal Location As String, ByVal Warehouse As String, ByRef Consignee As String, ByVal Sku As String, ByRef ErrMessage As String) As Boolean
        Dim sql As String
        Dim ret As Boolean = False
        Dim frontLoc As String
        sql = String.Format("select FRONTRACKLOCATION from vLocationFlowRack  WHERE flowracktype='BACKRACK' and LOCATION='{0}' and WAREHOUSEAREA='{1}'", Location, Warehouse)
        '
        frontLoc = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If String.IsNullOrEmpty(frontLoc) Then
            'The system will check if the location approved is a pick location for a different item. If yes the system need to block the user doing it
            Return ChangeLocationValidationRegularLocation(Location, Warehouse, Consignee, Sku, ErrMessage)
        Else
            Location = frontLoc
        End If

        'The system will also check if the location approved is a back location and if the front location is a pick location for a different item. If yes the system need to block the user doing it
        sql = String.Format("select COUNT(1) from PICKLOC where LOCATION = '{0}' and WAREHOUSEAREA='{1}'", Location, Warehouse)
        '
        Try
            If Not WMS.Logic.Location.Exists(Location, Warehouse) Then
                ErrMessage = "Front rack location does not exist"
                Return False
            End If
            sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            'if loc does not exist in pickloc - location is pass validation
            If sql = "0" Then
                GoTo CheckLocStatus
            Else
                sql = String.Format("select COUNT(1) from PICKLOC where LOCATION = '{0}' and WAREHOUSEAREA='{1}' and CONSIGNEE='{2}' and SKU='{3}'", Location, Warehouse, Consignee, Sku)
                sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                'if loc not exist in pickloc but not for curent sku- location is NOT pass validation
                If sql = "0" Then
                    ErrMessage = "Front rack pick location does not belong to this SKU"
                    Return False
                Else
CheckLocStatus:

                    sql = "select STATUS,PROBLEMFLAG from LOCATION where  LOCATION = '{0}' and WAREHOUSEAREA='{1}' "
                    sql = String.Format(sql, Location, Warehouse)
                    Dim DT As New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, DT)
                    For Each DR1 As DataRow In DT.Rows

                        'Start PWMS-875 and  RWMS-927   
                        If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DR1("STATUS")) = False Then
                            'End PWMS-875 and  RWMS-927  
                            ErrMessage = "Inactive front rack location"
                            Return False
                            'Start PWMS-875 and RWMS-927   
                        ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DR1("PROBLEMFLAG")) = True Then
                            'End PWMS-875 and RWMS-927  
                            ErrMessage = "The front rack location has a problem"
                            Return False
                        Else
                            Return True
                        End If
                    Next
                End If

            End If
        Catch ex As Exception
            ErrMessage = ex.Message
        End Try
        Return ret
    End Function
End Class
