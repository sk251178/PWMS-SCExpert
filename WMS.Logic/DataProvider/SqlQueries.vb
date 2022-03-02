'Class to define SQL queries
Public Class SqlQueries

    ''' <summary>
    ''' Get All Orders for the WAVE
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GetOrdersForWave As String = "select ORDERID,CONSIGNEE from WAVEDETAIL where WAVE = '{0}' and ORDERID is not null"

    ''' <summary>
    ''' string literal for SQL to Get Codelist for task priority
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GetCodeList As String = "Select [Parameter], [DataValue1] from [SystemConfig] where [Category] = '{0}' "
    ''' <summary>
    ''' string literal for SQL to Get Code for task priority description
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GetCodeListValue As String = "Select [DataValue1] from [SystemConfig] where [Category] = '{0}' and Parameter='{1}'"

    ' <summary>
    '  string literal for SQL to Update Priority For Replenishment
    ' </summary>
    ' <remarks></remarks>
    ' RWMS-2583
    ' ''RWMS-2714 Commented
    'Public Const UpdatePriorityForReplenishment As String = "UPDATE TASKS SET PRIORITY={0} WHERE TASK = (SELECT TOP 1  TASK FROM TASKS " +
    '                                                        " WHERE CONSIGNEE = '{1}' AND SKU = '{2}' AND STATUS='AVAILABLE' AND " +
    '                                                        " REPLENISHMENT IS NOT NULL AND REPLENISHMENT<>'' AND PRIORITY<{0} order by ADDDATE desc)"
    'RWMS-2714 Commented END
    'RWMS-2714
    Public Const UpdatePriorityForReplenishment As String = "UPDATE TASKS SET PRIORITY={0} WHERE TASK = (SELECT TOP 1  TASK FROM TASKS " +
                                                            " WHERE CONSIGNEE = '{1}' AND SKU = '{2}' AND STATUS='AVAILABLE' AND " +
                                                            " REPLENISHMENT IS NOT NULL AND REPLENISHMENT<>'' AND PRIORITY<{0} order by REPLENISHMENT desc)"

    Public Const ReSetPriorityForReplenishment As String = "UPDATE TASKS SET PRIORITY={0} WHERE TASK = (SELECT TOP 1  TASK FROM TASKS " +
                                                            " WHERE CONSIGNEE = '{1}' AND SKU = '{2}' AND STATUS='AVAILABLE' AND " +
                                                            " REPLENISHMENT IS NOT NULL AND REPLENISHMENT<>'' AND PRIORITY<={3} order by REPLENISHMENT desc)"
    'RWMS-2714 END

    ''' <summary>
    ''' string literal for SQL to Update DueDate For Replenishment
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UpdateDueDateForReplenishment As String = "UPDATE REPLENISHMENT SET REPLDATETIME='{0}' WHERE " +
                                                          " REPLID = (SELECT TOP 1 REPLENISHMENT FROM TASKS WHERE " +
                                                          " CONSIGNEE = '{1}' AND SKU = '{2}' AND STATUS='AVAILABLE' " +
                                                          " AND REPLENISHMENT IS NOT NULL AND REPLENISHMENT<>'')"

    Public Const GetCancelReplenishmentsValue As String = "SELECT REPLENISHMENT.REPLID,REPLENISHMENT.UNITS,REPLENISHMENT.FROMLOAD,vPickLoc.* FROM REPLENISHMENT INNER JOIN vPickLoc " +
                                                          " ON REPLENISHMENT.TOLOCATION = vPickLoc.LOCATION WHERE (REPLENISHMENT.STATUS<>'CANCELED')  " +
                                                          " AND (REPLENISHMENT.STATUS<>'COMPLETE') AND SKU='{0}' AND CONSIGNEE='{1}' AND LOCATION='{2}'" +
                                                          " ORDER BY REPLENISHMENT.REPLID DESC"

    Public Const GetOutBoundDepartureDateValue As String = "SELECT ISNULL(OUTBOUNDORHEADER.SCHEDULEDDATE,'') AS SCHEDULEDDATE FROM OUTBOUNDORHEADER " +
                                                          " INNER JOIN OUTBOUNDORDETAIL ON OUTBOUNDORHEADER.CONSIGNEE = OUTBOUNDORDETAIL.CONSIGNEE AND " +
                                                          "  OUTBOUNDORHEADER.ORDERID = OUTBOUNDORDETAIL.ORDERID WHERE (OUTBOUNDORHEADER.SCHEDULEDDATE IS NOT NULL) " +
                                                          "  AND OUTBOUNDORDETAIL.ORDERID ='{0}'"

    Public Const GetWaveDepartureDateValue As String = "SELECT ISNULL(OUTBOUNDORHEADER.SCHEDULEDDATE, '') AS SCHEDULEDDATE FROM OUTBOUNDORDETAIL INNER JOIN WAVEDETAIL ON OUTBOUNDORDETAIL.CONSIGNEE = WAVEDETAIL.CONSIGNEE AND OUTBOUNDORDETAIL.ORDERID = WAVEDETAIL.ORDERID AND  OUTBOUNDORDETAIL.ORDERLINE = WAVEDETAIL.ORDERLINE INNER JOIN OUTBOUNDORHEADER ON OUTBOUNDORDETAIL.CONSIGNEE = OUTBOUNDORHEADER.CONSIGNEE AND OUTBOUNDORDETAIL.ORDERID = OUTBOUNDORHEADER.ORDERID   WHERE OUTBOUNDORHEADER.SCHEDULEDDATE IS NOT NULL AND WAVEDETAIL.WAVE = '{0}'"

    Public Const GetStandardTimeValue As String = "SELECT STDTIME FROM  TASKS WHERE  STATUS='ASSIGNED' AND PICKLIST IS NOT NULL AND PICKLIST<>'' AND USERID='{0}'"

    Public Const CalculatePickTimeValue As String = "SELECT PICKDETAIL.PICKLIST, PICKDETAIL.ORDERID, PICKDETAIL.ORDERLINE, PICKDETAIL.SKU, " +
                                                    " PICKDETAIL.UOM, PICKDETAIL.QTY, PICKDETAIL.FROMLOCATION FROM TASKS INNER JOIN PICKDETAIL " +
                                                    " ON TASKS.PICKLIST = PICKDETAIL.PICKLIST WHERE TASKS.STATUS='ASSIGNED'"

    Public Const CalculateReplenishmentTimeValue As String = "SELECT COALESCE(TASKS.STARTTIME, TASKS.ASSIGNEDTIME) AS STARTTIME,DATEDIFF(SECOND, DATEADD(DAY, DATEDIFF(DAY, 0, COALESCE(TASKS.STARTTIME, TASKS.ASSIGNEDTIME)), 0), " +
                                                            " COALESCE(TASKS.STARTTIME, TASKS.ASSIGNEDTIME)) as TotalStandardSeconds,PICKDETAIL.PICKLIST, PICKDETAIL.ORDERID, " +
                                                            " PICKDETAIL.ORDERLINE, PICKDETAIL.SKU, PICKDETAIL.UOM, PICKDETAIL.QTY, " +
                                                            " PICKDETAIL.FROMLOCATION FROM TASKS INNER JOIN PICKDETAIL ON " +
                                                            " TASKS.PICKLIST = PICKDETAIL.PICKLIST WHERE TASKS.STATUS='ASSIGNED' AND  tasks.USERID='{0}'"

    Public Const UpdateStatusForReplenishmentValue As String = "Update REPLENISHMENT SET status='{0}',editdate='{1}',edituser='{2}' where REPLID='{3}'"

    Public Const GetLoadIDForPayloadValue As String = "SELECT  * FROM LOADS WHERE LOADID='{0}'"

    Public Const UpdateStatusForPayloadValue As String = "Update LOADS SET ACTIVITYSTATUS='',DESTINATIONLOCATION = '',DESTINATIONWAREHOUSEAREA = '',UNITSALLOCATED='{0}',editdate='{1}',edituser='{2}' where LOADID='{3}'"

    Public Const GetMultipaylodValue As String = "select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid = '{0}'"

    Public Const GetGroupOfLoadsValue As String = "SELECT COUNT(DISTINCT(GRP.GROUPNUMBER)) NUMBEROFGROUP FROM(SELECT GROUPNUMBER = DENSE_RANK() OVER (ORDER BY ISNULL(SK.SKUGROUP,''),ISNULL(SK.CLASSNAME,''),ISNULL(SK.HAZCLASS,'')) FROM LOADS LD INNER JOIN SKU SK ON LD.SKU=SK.SKU WHERE LD.LOADID IN ({0})) AS GRP"

    Public Const GetfLoadsHeightValue As String = "SELECT SKUUOM.HEIGHT/UNITSPERMEASURE * CEILING(UNITS/(SELECT SKUUOM.UNITSPERMEASURE FROM SKUUOM RIGHT OUTER JOIN LOADS ON LOADS.CONSIGNEE = SKUUOM.CONSIGNEE AND SKUUOM.SKU=LOADS.SKU WHERE SKUUOM.LOWERUOM='CASE' AND LOADID ='{0}') ) AS LOADSHEIGHT FROM LOADS LEFT OUTER JOIN SKUUOM ON LOADS.CONSIGNEE = SKUUOM.CONSIGNEE AND LOADS.SKU = SKUUOM.SKU  WHERE  SKUUOM.LOWERUOM='LAYER' AND  LOADID ='{0}'"

    Public Const GetLoadsinLocationValue As String = "SELECT LOADID FROM LOADS WHERE LOCATION ='{0}'"

    Public Const GetLoadsinDestinationLocationValue As String = "SELECT LOADID FROM LOADS WHERE DESTINATIONLOCATION ='{0}'"

    Public Const GetQtyInReplenAtPriority As String = "select sum(REPLENISHMENT.UNITS) from Tasks inner join REPLENISHMENT on Tasks.REPLENISHMENT = REPLENISHMENT.REPLID where Tasks.STATUS = 'AVAILABLE' and REPLENISHMENT.TOLOCATION = '{0}' and Tasks.PRIORITY='{1}'"

    Public Const UpdatePriorityOfReplenishment As String = "Update Tasks Set Priority = '{0}' where Task = '{1}'"
End Class