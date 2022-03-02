Imports Made4Net.DataAccess
Imports WMS.Lib

<CLSCompliant(False)> Public Class PlanRequirments
    Inherits Requirments

#Region "Constructor"

    Public Sub New(ByVal Wave As Wave, Optional ByVal oLogger As LogHandler = Nothing)
        MyBase.New()
        Load(Wave.WAVE, oLogger)
        'setStrategy(oLogger)
    End Sub

    Public Sub New(ByVal oOrder As OutboundOrderHeader, Optional ByVal oLogger As LogHandler = Nothing)
        MyBase.New()
        Load(oOrder.CONSIGNEE, oOrder.ORDERID, oLogger)
        'setStrategy(oLogger)
    End Sub

#End Region

#Region "Methods"

#Region "Load Wave"

    Protected Sub Load(ByVal WaveId As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim SQL As String = String.Format("SELECT ORDERREQUIREMENTS.* FROM ORDERREQUIREMENTS inner join outboundorheader oh on oh.consignee = ORDERREQUIREMENTS.consignee " & _
           " and oh.orderid = ORDERREQUIREMENTS.orderid and oh.status in ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')" & _
           " WHERE ORDERREQUIREMENTS.WAVE = '{0}' AND ORDERREQUIREMENTS.QTYLEFTTOFULLFILL > 0 ORDER BY ORDERREQUIREMENTS.ORDERPRIORITY ASC", WaveId, _
                Statuses.OutboundOrderHeader.STATUSNEW, Statuses.OutboundOrderHeader.PICKING, Statuses.OutboundOrderHeader.PLANNED, _
                Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED, Statuses.OutboundOrderHeader.SHIPMENTASSIGNED, Statuses.OutboundOrderHeader.WAVEASSIGNED, _
                Statuses.OutboundOrderHeader.LOADING, Statuses.OutboundOrderHeader.RELEASED)

        'Start RWMS-768
        If Not oLogger Is Nothing Then
            oLogger.Write("Commencing plan requirements ...")
            oLogger.WriteTimeStamp = True
            oLogger.Write(SQL)
            oLogger.writeSeperator()
        End If
        'End  RWMS-768

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)

        Dim oAttributesDataTable As New DataTable
        SQL = String.Format("SELECT NAME,TYPE FROM INVENTORYATTRIBUTELIST")
        DataInterface.FillDataset(SQL, oAttributesDataTable)

        SetRequirements(dt, oAttributesDataTable)

        'Begin -RWMS-790
        'Check if QTYLEFTTOFULLFILL of orderrequirements does not match with pickdetails log all the details

        Dim dr As DataRow
        For Each dr In dt.Rows
            Dim dtPickDetail As New DataTable
            Dim drPickDetail As DataRow
            Dim TotalPickedQty As Integer
            Dim TotalAdujstedQty As Integer
            Dim QtyLeftFromPickDetil As Integer
            Dim strLogMsg As String

            TotalPickedQty = 0
            QtyLeftFromPickDetil = 0
            TotalAdujstedQty = 0

            Dim sqlPickDetail As String = String.Format("SELECT PICKLIST,PICKLISTLINE,ORDERID,ORDERLINE,STATUS,SKU,QTY,ADJQTY,PICKEDQTY,FROMLOCATION,FROMLOAD,ADDDATE,EDITDATE FROM dbo.PICKDETAIL " & _
            " WHERE ORDERID = '{0}' AND ORDERLINE = {1} AND SKU = '{2}'  AND CONSIGNEE = '{3}' AND  STATUS <> 'CANCELED' ", dr("ORDERID"), _
                dr("ORDERLINE"), dr("SKU"), dr("CONSIGNEE"))
            DataInterface.FillDataset(sqlPickDetail, dtPickDetail)
            For Each drPickDetail In dtPickDetail.Rows

                If drPickDetail("PICKEDQTY") = 0 Then
                    TotalAdujstedQty = TotalAdujstedQty + drPickDetail("ADJQTY")
                Else
                    TotalPickedQty = TotalPickedQty + drPickDetail("PICKEDQTY")
                    TotalAdujstedQty = TotalAdujstedQty + drPickDetail("PICKEDQTY")
                End If


            Next
            QtyLeftFromPickDetil = dr("QTYMODIFIED") - TotalAdujstedQty
            If (TotalPickedQty > 0) Then
                strLogMsg = String.Format(" ORDERDETAIL Quantities: ORDERID = {0},ORDERLINE = {1},SKU = {2}," & _
                                          "QTYORIGINAL={3}, QTYMODIFIED={4}, QTYALLOCATED={5}, QTYPICKED={6}, QTYSTAGED={7}, QTYPACKED={8}, QTYLOADED={9}, QTYSHIPPED={10}, QTYVERIFIED={11}", dr("ORDERID"), _
                                          dr("ORDERLINE"), dr("SKU"), dr("QTYORIGINAL"), dr("QTYMODIFIED"), dr("QTYALLOCATED"), dr("QTYPICKED"), dr("QTYSTAGED"), dr("QTYPACKED"), dr("QTYLOADED"), _
                                          dr("QTYSHIPPED"), dr("QTYVERIFIED"))



                oLogger.Write(strLogMsg)
                oLogger.writeSeperator()
                oLogger.Write("OrderRequirements QtyLeftToFulfill OrderRequirements vs  PickDetails ...")
                strLogMsg = String.Format(" QtyLeftToFulfill (order vs pickdetail): QTYLEFTTOFULLFILL From ORDREQ = {0},QtyLeftToFulfil from PickDetail = {1},TotalPicked={2},TotalAdjusted={3}", dr("QTYLEFTTOFULLFILL"), _
                                          QtyLeftFromPickDetil, TotalPickedQty, TotalAdujstedQty)
                oLogger.Write(strLogMsg)
                oLogger.writeSeperator()
                If (dr("QTYLEFTTOFULLFILL") <> QtyLeftFromPickDetil) Then


                    For Each drPickDetail In dtPickDetail.Rows
                        strLogMsg = String.Format(" PICKDETAILS: PICKLIST = {0}, ORDERID = {1},ORDERLINE = {2},SKU = {3}, STATUS ={4},QTY={5} " & _
                         ",ADJQTY={6},PICKEDQTY={7},FROMLOCATION={8},FROMLOAD={9},ADDDATE={10},EDITDATE={11} ", drPickDetail("PICKLIST"), _
                      drPickDetail("ORDERID"), drPickDetail("ORDERLINE"), drPickDetail("SKU"), drPickDetail("STATUS"), drPickDetail("QTY"), drPickDetail("ADJQTY"), _
                     drPickDetail("PICKEDQTY"), drPickDetail("FROMLOCATION"), drPickDetail("FROMLOAD"), drPickDetail("ADDDATE"), drPickDetail("EDITDATE"))

                        oLogger.Write(strLogMsg)
                    Next
                    oLogger.writeSeperator()
                    'Log the order line 
                    strLogMsg = String.Format(" ORDERDETAIL Quantities: ORDERID = {0},ORDERLINE = {1},SKU = {2}," & _
                                              "QTYORIGINAL={3}, QTYMODIFIED={4}, QTYALLOCATED={5}, QTYPICKED={6}, QTYSTAGED={7}, QTYPACKED={8}, QTYLOADED={9}, QTYSHIPPED={10}, QTYVERIFIED={11}", dr("ORDERID"), _
                                              dr("ORDERLINE"), dr("SKU"), dr("QTYORIGINAL"), dr("QTYMODIFIED"), dr("QTYALLOCATED"), dr("QTYPICKED"), dr("QTYSTAGED"), dr("QTYPACKED"), dr("QTYLOADED"), _
                                              dr("QTYSHIPPED"), dr("QTYVERIFIED"))



                    oLogger.Write(strLogMsg)

                End If
            End If
            dtPickDetail.Dispose()
        Next
        'End -RWMS-790

        dt.Dispose()
        oAttributesDataTable.Dispose()
    End Sub

#End Region

#Region "Load Order"

    Protected Sub Load(ByVal pConsignee As String, ByVal pOrderid As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim SQL As String = String.Format("SELECT * FROM ORDERREQUIREMENTS WHERE CONSIGNEE = '{0}' AND ORDERID = '{1}' " & _
            " AND QTYLEFTTOFULLFILL > 0 ORDER BY ORDERPRIORITY ASC ", pConsignee, pOrderid)

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)

        Dim oAttributesDataTable As New DataTable
        SQL = String.Format("SELECT NAME,TYPE FROM INVENTORYATTRIBUTELIST")
        DataInterface.FillDataset(SQL, oAttributesDataTable)

        SetRequirements(dt, oAttributesDataTable)

        dt.Dispose()
        oAttributesDataTable.Dispose()
    End Sub

#End Region

    Private Sub SetRequirements(ByVal pReqs As DataTable, ByVal pAtributes As DataTable)
        Dim req As Requirment
        Dim dr As DataRow
        Dim oPolicyMatcher As New PolicyMatcher("consigneeplanpolicy")
        For Each dr In pReqs.Rows
            req = New Requirment(dr, pAtributes)
            Add(req)
            'assign the requirements according to policies
            req.SetPlanStrategy(oPolicyMatcher.FindMatchingPolicies(dr))
        Next
    End Sub

    Public Sub setStrategy(ByVal pDtReqs As DataTable)
        'Old Code 
        'Dim req As Requirment
        'Dim dt As DataTable = setConsigneePolicies(oLogger)
        'For Each req In Me
        '    req.setStrategy(dt, oLogger)
        'Next
        'dt.Dispose()
    End Sub

    Protected Function setConsigneePolicies(Optional ByVal oLogger As LogHandler = Nothing) As DataTable
        Dim sql As String = "select consignee,priority,replace(isnull(ordertype,'%'),'*','%') as ordertype,replace(isnull(classname,'%'),'*','%') as classname,replace(isnull(sku,'%'),'*','%') as sku, " & _
            "replace(isnull(velocity,'%'),'*','%') as velocity,replace(isnull(skugroup,'%'),'*','%') as skugroup,replace(isnull(companytype,'%'),'*','%') as companytype, replace(isnull(targetcompany,'%'),'*','%') as targetcompany,isnull(orderlines,'0') as orderlines,isnull(linesunits,'0') as linesunits,isnull(orderpriority,'0') as orderpriority," & _
            "replace(isnull(route,'%'),'*','%') as route,isnull(ordervalue,'0') as ordervalue,isnull(ordervolume,'0') as ordervolume,replace(isnull(companygroup,'%'),'*','%') as companygroup,replace(isnull(invstatus,'%'),'*','%') as invstatus, isnull(linevolume,'0') as linevolume,isnull(linevalue,'0') as linevalue,isnull(orderunits,'0') as orderunits,strategyid ,isnull(carrier,'%') as carrier, isnull(unloadingtype,'%') as unloadingtype, " & _
            "isnull(vehicletype,'%') as vehicletype, isnull(transporttype,'%') as transporttype, isnull(hazclass,'%') as hazclass from consigneeplanpolicy order by Priority"

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        Dim idx As Int32
        For Each dr In dt.Rows()
            For idx = 0 To dt.Columns.Count - 1
                If dt.Columns(idx).DataType Is System.Type.GetType("System.String") Then
                    If dr(idx) = "" Then
                        dr(idx) = "%"
                    End If
                End If
            Next
        Next

        Return dt
    End Function

    'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
    'Public Overrides Function CanPlace(ByVal req As Requirment) As Boolean
    'Commented for Retrofit Item PWMS-748 (RWMS-439) End
    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Overrides Function CanPlace(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        Return True
    End Function
#End Region

End Class
