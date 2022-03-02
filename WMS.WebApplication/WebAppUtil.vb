Imports Made4Net.DataAccess

Public Class WebAppUtil
    Public Shared Sub RedirectToTaskGoalTimeDisplay(ByVal pNextScreen As String, ByVal pTaskId As String)
        'If Not ShowGoalTimeOnTaskAssignment() Then
        '    HttpContext.Current.Response.Redirect(MapVirtualPath("screens/" & pNextScreen & ".aspx"))
        '    Return
        'End If
        HttpContext.Current.Session("TaskID") = pTaskId
        HttpContext.Current.Session("ScreenAfterTaskSummary") = pNextScreen
        HttpContext.Current.Response.Redirect(("screens/TaskSummary.aspx?IsSummary=0"))
    End Sub

    Public Shared Function ClockUserCheckOut(ByVal pUserID As String, ByVal InOut As Integer, ByVal pLocation As String, _
                                       Optional ByVal pShiftID As String = "") As Integer
        Dim SQL As String


        If InOut = WMS.Lib.Shift.ClockStatus.OUT Then
            SQL = String.Format("select isnull(SHIFT,'') SHIFT from WHACTIVITY where USERID='{0}' ", pUserID)
            If DataInterface.ExecuteScalar(SQL) = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-out User from Shift. User already Clock-out.", "Cannot Clock-out User from Shift. User already Clock-out.")
                Return -40
            End If

        End If

        ''check if shift started
        If pShiftID <> String.Empty Then
            If WMS.Logic.GetSysParam("AllowClockinBeforeShiftStart") = "0" Then
                Dim CurrentShiftStatus As String = CType(DataInterface.ExecuteScalar(String.Format("select STATUS from SHIFT where SHIFTID='{0}'", pShiftID)), String)
                If CurrentShiftStatus <> WMS.Lib.Shift.ShiftInstanceStatus.STARTED Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-in/out Shift not started.", "Cannot Clock-in Shift not started")
                    Return -50
                End If

            End If

        End If

        'clock in
        ''convert(nvarchar,datepart(hour,getdate()))+ convert(nvarchar,datepart(mi,getdate()))
        Try
            SQL = String.Format("insert dbo.SHIFTUSERCLOCKS " & _
                "([SHIFTID],[USERID],[CLOCKINOUT],[CLOCKTIME],[ADDDATE])" & _
                " values ('{0}','{1}','{2}',getdate(),getdate())", pShiftID, pUserID, InOut)
            DataInterface.RunSQL(SQL)
            WMS.Logic.WHActivity.SetShift(pUserID, pShiftID, InOut, pLocation)

            '' init LABORUSERCOUNTERS for current user
            SQL = String.Format("update LABORUSERCOUNTERS set LABORCOUNTERVALUE=0 where USERID='{0}'", pUserID)
            DataInterface.RunSQL(SQL)

        Catch ex As Exception
            Throw New Made4Net.Shared.M4NException(New Exception, "Error Clock-in User to Shift:" & ex.ToString(), "Error Clock-in User to Shift:" & ex.Message)
            Return -99

        End Try
    End Function


    Public Shared Function CalcUnits(ByVal pConsignee As String, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pUom As String) As Decimal
        Dim oSku As New WMS.Logic.SKU(pConsignee, pSku)
        If WMS.Logic.SKU.SKUUOM.Exists(oSku.CONSIGNEE, oSku.SKU, pUom) Then
            Return oSku.ConvertToUnits(pUom)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Input UOM does not exist", "Input UOM does not exist")
        End If
        'Return oSku.ConvertToUnits(pUom)
    End Function

    Public Shared Function getLastVal(ByVal val As String) As String
        Dim v() As String = val.ToString.Split(",")
        Return v(v.Length - 1)
    End Function

    Public Shared Sub UpdateShipmentDetail(ByVal consignee As String, ByVal orderid As String, ByVal ordline As String, ByVal QTYMODIFIED As Decimal)
        'RWMS-2581 RWMS-2554
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2581 RWMS-2554 END

        Dim dt As New DataTable
        Dim Sql As String = "SELECT UNITS FROM SHIPMENTDETAIL where CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE='{2}'"
        Sql = String.Format(Sql, consignee, orderid, ordline)
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)

        'RWMS-2581 RWMS-2554
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" ShipmentDetail SQL query:{0}", Sql))
        End If
        'RWMS-2581 RWMS-2554 END

        If dt.Rows.Count = 1 Then
            Sql = "update SHIPMENTDETAIL set UNITS='{3}' where CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE='{2}'"
            Sql = String.Format(Sql, consignee, orderid, ordline, QTYMODIFIED)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

            'RWMS-2581 RWMS-2554
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Update ShipmentDetail SQL query:{0}", Sql))
            End If
            'RWMS-2581 RWMS-2554 END
        End If
    End Sub

    'Start RWMS-2247 RWMS-2014
    Public Shared Sub UpdateShipmentToLoaded(ByVal consignee As String, ByVal orderid As String, ByVal ordline As String, ByVal userid As String)
        'RWMS-2581 RWMS-2554
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2581 RWMS-2554 END

        'RWMS-2581 RWMS-2554
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" Started updatating Shipment status to Loaded"))
        End If
        'RWMS-2581 RWMS-2554 END

        Dim dt As New DataTable
        Dim Sql As String = "SELECT SHIPMENT FROM SHIPMENTDETAIL where CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE='{2}'"
        Sql = String.Format(Sql, consignee, orderid, ordline)
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)

        'RWMS-2581 RWMS-2554
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" SHIPMENT to be updated SQL query:{0}", Sql))
        End If
        'RWMS-2581 RWMS-2554 END

        If dt.Rows.Count = 1 Then
            Dim shpmentid As String = dt.Rows(0)("SHIPMENT")
            Dim shpment As WMS.Logic.Shipment = New WMS.Logic.Shipment(shpmentid)
            If shpment.IsLoadingCompleted Then
                shpment.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, userid)
                'RWMS-2581 RWMS-2554
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" SHIPMENT:{0} status updated to LOADED", shpment))
                End If
                'RWMS-2581 RWMS-2554 END
            End If
        Else
            'RWMS-2581 RWMS-2554
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" More than 1 SHIPMENT found for ORDERID:{0}, ORDERLINE:{1} ", orderid, ordline))
            End If
            'RWMS-2581 RWMS-2554 END
        End If
    End Sub
    'End RWMS-2247 RWMS-2014

    Public Shared Sub UpdateToShippedStatus(ByVal shipment As String)

        Dim Sql As String
        Sql = "update SHIPMENT set STATUS='SHIPPED' where SHIPMENT='{0}'"
        Sql = String.Format(Sql, shipment)
        Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

        Sql = "update SHIPMENT set STATUS='SHIPPED' where SHIPMENT='{0}'"
        Sql = String.Format(Sql, shipment)
        Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

        Sql = "update SHIPMENT set STATUS='SHIPPED' where SHIPMENT='{0}'"
        Sql = String.Format(Sql, shipment)
        Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)


    End Sub


    Public Shared Sub UpdateShipmentDetail(ByVal dt As DataTable)

        Dim outordet As WMS.Logic.OutboundOrderHeader.OutboundOrderDetail
        For Each dr As DataRow In dt.Rows
            outordet = New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
            outordet.CancelExceptions(WMS.Logic.Common.GetCurrentUser)
            WebAppUtil.UpdateShipmentDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), outordet.QTYMODIFIED)
            'WebAppUtil.CancelOrder(dr("CONSIGNEE"), dr("ORDERID"))
        Next
    End Sub

    Public Shared Sub processOrderExceptionLines(ByVal consignee As String, ByVal orderid As String) 'As DataTable
        Dim dt As New DataTable

        Dim Sql As String = "SELECT  CONSIGNEE, ORDERID, ORDERLINE FROM WAVEEXCEPTION where CONSIGNEE='{0}' and ORDERID='{1}' "
        Sql = String.Format(Sql, consignee, orderid)
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        UpdateShipmentDetail(dt)
        'dim out
        '--Return dt

    End Sub
    Public Shared Sub processOrderExceptionLines(ByVal wave As String) 'As DataTable
        Dim dt As New DataTable

        Dim Sql As String = "SELECT  CONSIGNEE, ORDERID, ORDERLINE FROM WAVEEXCEPTION where wave='{0}' "
        Sql = String.Format(Sql, wave)
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        UpdateShipmentDetail(dt)


    End Sub
    Public Shared Sub processShipmentExceptionLines(ByVal Shipment As String) 'As DataTable
        Dim dt As New DataTable

        Dim Sql As String = "SELECT  CONSIGNEE, ORDERID, ORDERLINE FROM SHIPMENTEXCEPTION where Shipment='{0}' "
        Sql = String.Format(Sql, Shipment)
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        UpdateShipmentDetail(dt)


    End Sub

    Public Shared Sub processShipmentLoadLoads(ByVal Shipment As String, ByVal DOOR As String)
        Dim dt As New DataTable

        Dim Sql As String = "SELECT  CONSIGNEE, ORDERID, ORDERLINE, LOADID FROM vAutoShipLoaded where Shipment='{0}' "
        Sql = String.Format(Sql, Shipment)
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        Dim outordet As WMS.Logic.OutboundOrderHeader.OutboundOrderDetail
        Dim l As WMS.Logic.Load
        Dim WarehouseArea As String = WMS.Logic.Warehouse.getUserWarehouseArea()
        Dim USER As String = WMS.Logic.GetCurrentUser
        Dim sl As New WMS.Logic.Shipment.ShipmentLoad

        For Each dr As DataRow In dt.Rows
            outordet = New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
            l = New WMS.Logic.Load(dr("LOADID").ToString())
            Try
                l.Load(DOOR, WarehouseArea, USER, outordet)

                sl.Create(Shipment, dr("LOADID").ToString(), USER)


            Catch ex As Exception
            End Try
        Next


    End Sub
    'vAutoShipLoaded()
    'Dim l As New WMS.Logic.Load("")
    '                l.Load(s.DOOR, Warehouse.getUserWarehouseArea(),

    Public Shared Sub processCompleteShipmentOrder(ByVal SHIPMENT As String) 'As DataTable
        Dim dt As DataTable

        Dim Sql As String

        dt = New DataTable
        Sql = "SELECT distinct CONSIGNEE, ORDERID FROM SHIPMENTDETAIL where SHIPMENT='{0}' "
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        For Each dr As DataRow In dt.Rows
            WebAppUtil.CompleteOrder(dr("CONSIGNEE"), dr("ORDERID"))
        Next

    End Sub

    Public Shared Sub processCompleteOrder(ByVal wave As String) 'As DataTable
        Dim dt As DataTable

        Dim Sql As String

        dt = New DataTable
        Sql = "SELECT distinct CONSIGNEE, ORDERID FROM WAVEDETAIL where wave='{0}' "
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        For Each dr As DataRow In dt.Rows
            WebAppUtil.CompleteOrder(dr("CONSIGNEE"), dr("ORDERID"))
        Next

    End Sub
    Public Shared Sub CancelOrder(ByVal consignee As String, ByVal orderid As String) 'As DataTable
        Dim dt As New DataTable

        Dim Sql As String = "select SUM(QTYMODIFIED) from OUTBOUNDORDETAIL where CONSIGNEE='{0}' and ORDERID='{1}' "
        Sql = String.Format(Sql, consignee, orderid)
        Sql = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql))
        If Sql = "0" Then
            Dim out As New WMS.Logic.OutboundOrderHeader(consignee, orderid)
            out.CancelOutbound(WMS.Logic.GetCurrentUser)
            'out.Complete(WMS.Logic.GetCurrentUser)
        End If

    End Sub
    Public Shared Sub CompleteOrder(ByVal consignee As String, ByVal orderid As String) 'As DataTable
        'RWMS-2581 RWMS-2554
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2581 RWMS-2554 END

        Dim dt As New DataTable

        Dim Sql As String = "select QTYPROCESS from vOUTBOUNDQTYPROCESS where CONSIGNEE='{0}' and ORDERID='{1}' "
        Sql = String.Format(Sql, consignee, orderid)
        Sql = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql))

        'RWMS-2581 RWMS-2554
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" CompleteOrder.QTYPROCESS SQL query:{0}", Sql))
        End If
        'RWMS-2581 RWMS-2554 END

        If Sql = "0" Then
            'Dim out As New WMS.Logic.OutboundOrderHeader(consignee, orderid)
            ''out.CancelOutbound(WMS.Logic.GetCurrentUser)
            'out.Complete(WMS.Logic.GetCurrentUser)
            Dim outorheader As New RWMS.Logic.OutboundOrderHeader(consignee, orderid)
            outorheader.Complete(WMS.Logic.GetCurrentUser)
        End If

    End Sub
End Class