Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ZoneReplenish
    Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

    Protected _all As String
    Protected _consignee As String
    Protected _putregion As String
    Protected _sku As String

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal sPutRegion As String, ByVal sSku As String, ByVal pConsignee As String)
        MyBase.New()
        Try
            If Not sPutRegion Is Nothing Then _putregion = sPutRegion Else _putregion = ""
        Catch ex As Exception
            _putregion = ""
        End Try
        Try
            If Not sSku Is Nothing Then _sku = sSku Else _sku = ""
        Catch ex As Exception
            _sku = ""
        End Try
        Try
            _all = "1"
        Catch ex As Exception
        End Try

        Try
            If Not pConsignee Is Nothing Then _consignee = pConsignee Else _consignee = ""
        Catch ex As Exception
            _consignee = ""
        End Try

    End Sub

#End Region

#Region "Methods"

#Region "Job Scheduler"
    'Added for RWMS-1840   
    Dim ReplPolicyMethod As String = WMS.Logic.Replenishment.GetPolicyID()
    'Ended for RWMS-1840  

    Public Overrides Sub Execute()
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ZoneReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", ReplPolicyMethod)
        em.Add("LOCATION", "")
        em.Add("WAREHOUSEAREA", "")
        em.Add("PICKREGION", _putregion)
        em.Add("ALL", "1")
        em.Add("SKU", _sku)
        em.Add("CONSIGNEE", _consignee)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#End Region

#Region "Replenish"

    Public Function ZoneReplenishment(ByVal pPutRegion As String, ByVal pSku As String, ByVal pReplMethod As String, Optional ByVal pUser As String = "SYSTEM", Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoad As Load = Nothing, Optional ByVal pOnContainer As Boolean = False) As Decimal
        Dim SQL As String = String.Empty
        Dim dt As New DataTable, dr As DataRow
        Dim consignee, sku, putregion, pwpolicy As String
        Dim currQTY As Double = 0, accumQty As Decimal = 0
        Dim ldId As String = ""
        If Not pLoad Is Nothing Then
            ldId = pLoad.LOADID
        End If

        SQL = String.Format("SELECT DISTINCT CONSIGNEE, SKU ,PUTREGION, PUTAWAYPOLICY FROM vZONEREPLINVENTORYLEVEL WHERE SKU LIKE '{0}%' AND PUTREGION LIKE '{1}%'", pSku, pPutRegion)
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            consignee = dr("CONSIGNEE")
            sku = dr("SKU")
            putregion = dr("PUTREGION")
            pwpolicy = dr("PUTAWAYPOLICY")
            currQTY = CalcCurrQTY(consignee, sku, putregion)
            accumQty += UpdateQTY(consignee, sku, putregion, pwpolicy, pReplMethod, currQTY, pUser, oLogger, ldId, pOnContainer) '+= currQTY
        Next
        Return accumQty
    End Function

    Private Function CalcCurrQTY(ByVal pConsignee As String, ByVal pSku As String, ByVal pPutRegion As String) As Double
        Dim SQL As String = String.Empty
        Dim qty As Double = 0
        'SQL = String.Format("select sum(units) as QTY from invload il join location loc on il.location = loc.location where consignee = '{0}' and sku = '{1}' and loc.putregion = '{2}'", _
        '    pConsignee, pSku, pPutRegion)
        'Try
        '    qty = DataInterface.ExecuteScalar(SQL)
        'Catch ex As Exception
        'End Try
        'SQL = String.Format("select sum(units) as QTY from invload il join location loc on il.destinationlocation = loc.location where consignee = '{0}' and sku = '{1}' and loc.putregion = '{2}'", _
        '    pConsignee, pSku, pPutRegion)
        'Try
        '    qty += DataInterface.ExecuteScalar(SQL)
        'Catch ex As Exception
        'End Try
        SQL = String.Format("select isnull(sum(units),0) from vZoneReplCurrentInventoryLevel where consignee = '{0}' and sku = '{1}' and putregion = '{2}' ", _
            pConsignee, pSku, pPutRegion)

        qty = DataInterface.ExecuteScalar(SQL)
        Return qty
    End Function

    Public Function UpdateQTY(ByVal pConsignee As String, ByVal pSku As String, ByVal pPutRegion As String, ByVal pPutAwayPolicy As String, ByVal pReplMethod As String, ByRef pCurrentQTY As Double, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pOnContainer As Boolean = False) As Decimal
        Dim SQL As String = String.Empty
        Dim dtInv As New DataTable, drInv As DataRow
        Dim consignee, sku, putRegion, startegyId, targetLoc, targetWarehousearea As String
        Dim minLevel, maxLevel As Decimal
        Dim accumQTY, currLoadQty As Decimal
        Dim ts As New TaskManager
        Dim ReplPolicyMethod As String = WMS.Logic.Replenishment.GetPolicyID()
        accumQTY = 0
        SQL = String.Format("SELECT * FROM vZONEREPLINVENTORYLEVEL where consignee = '{0}' and sku = '{1}' and putregion = '{2}'", _
            pConsignee, pSku, pPutRegion)
        DataInterface.FillDataset(SQL, dtInv)
        For Each drInv In dtInv.Rows
            consignee = drInv("consignee")
            sku = drInv("sku")
            putRegion = drInv("PUTREGION")
            If pReplMethod = ReplPolicyMethod Then
                minLevel = drInv("hotminlevel")
                startegyId = drInv("REPLPOLICY")
            Else
                minLevel = drInv("minlevel")
                startegyId = drInv("REPLPOLICY")
            End If
            maxLevel = drInv("maxlevel")
            Dim shouldIgnoreMinLevel As Boolean
            If pReplMethod = ReplPolicyMethod Or pReplMethod = Replenishment.ReplenishmentMethods.ManualReplenishment Then
                shouldIgnoreMinLevel = True
            End If
            If minLevel < pCurrentQTY And Not shouldIgnoreMinLevel Then
                Exit Function 'dont need to replenish
            Else
                ' Load the replenishemnt policy 
                Dim dtPolicy As DataTable = CreatePolicyDataTable(startegyId)
                ' Create Replenishment tasks according to policy detail lines
                For Each dr As DataRow In dtPolicy.Rows
                    While accumQTY < maxLevel
                        currLoadQty = CreateZoneReplTasksInRegion(putRegion, pPutAwayPolicy, consignee, sku, maxLevel - pCurrentQTY, startegyId, dr, pCurrentQTY, pReplMethod, pUser, oLogger, pLoadId, pOnContainer)
                        If currLoadQty = 0 Then
                            Exit While
                        Else
                            accumQTY += currLoadQty
                        End If
                    End While
                Next
            End If
        Next
        pCurrentQTY += accumQTY
        Return accumQTY
    End Function

#End Region

#Region "Replenishment Policy & Tasks"

    Private Function CreatePolicyDataTable(ByVal pReplPolicy As String) As DataTable
        Dim oRepl As New Replenishment
        Dim prTbl As DataTable = New DataTable
        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY ORDER BY PRIORITY", prTbl)
        'For each priority found build policy detail table group by priority 
        Dim dt As New DataTable
        dt = oRepl.CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            dt.Rows.Add(oRepl.CreatePolicyDetailRow(dt, pReplPolicy, pr("PRIORITY")))
        Next
        Return dt
    End Function

    Private Function CreateZoneReplTasksInRegion(ByVal pPutRegion As String, ByVal pPutAwayPolicy As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pNeededQty As Decimal, ByVal pPolicyId As String, ByVal PolicyDetail As DataRow, ByRef CurrentQty As Double, ByVal pReplMethod As String, ByVal pUser As String, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadid As String = "", Optional ByVal pOnContainer As Boolean = False) As Decimal
        Dim dt As New DataTable
        Dim sql As String
        Dim repltype As String
        Dim ld As Load
        If pOnContainer Then
            repltype = Replenishment.ReplenishmentTypes.FullReplenishment
        Else
            repltype = PolicyDetail("REPLTYPE")
        End If
        Dim AllocatedQty As Double = 0

        ''If we are at opportunity replenishment lets verify that the original location of the load consist with the policy from pick region
        'If pLoadid <> "" Then
        '    sql = String.Format("select count(1) from invload inner join location on invload.location = location.location and location.pickregion = '{0}' and invload.loadid = '{1}'", PolicyDetail("PICKREGION"), pLoadid)
        '    If Not Convert.ToBoolean(DataInterface.ExecuteScalar(sql)) Then
        '        Return False
        '    End If
        'End If

        Select Case repltype
            Case Replenishment.ReplenishmentTypes.FullReplenishment
                ld = getFullReplLoadInRegion(pConsignee, pSku, pNeededQty, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pPolicyId, CurrentQty, oLogger, pLoadid)
                If Not ld Is Nothing Then
                    'call locAssign with fromput(region) and drloc("loadid")
                    Dim targetLoc, targetWarehousearea As String
                    SendToLocAssign(targetLoc, targetWarehousearea, pPutAwayPolicy, ld.LOADID, ld.UNITS, pPutRegion, oLogger) 'SendRegToLocAssign(pPutAwayPolicy, ld.LOADID, ld.UNITS, pPutRegion, PolicyDetail("STORAGETYPE"), PolicyDetail("INVENTORY"), PolicyDetail("CONTENT"))
                    'and insert the new task
                    If targetLoc <> "" And Not targetLoc Is Nothing Then
                        ld.SetDestinationLocation(targetLoc, targetWarehousearea, pUser)
                        CreateReplTask(ld, targetLoc, targetWarehousearea, PolicyDetail, ld.UNITS, pReplMethod, Nothing, oLogger, pOnContainer)
                        CurrentQty = CurrentQty + ld.UNITS
                        Return ld.UNITS
                    End If
                End If
            Case Replenishment.ReplenishmentTypes.PartialReplenishment, Replenishment.ReplenishmentTypes.NegativeReplenishment
                ld = getPartialReplLoadInRegion(pConsignee, pSku, pNeededQty, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pPolicyId, AllocatedQty, oLogger, pLoadid)
                If Not ld Is Nothing Then
                    'call locAssign with fromput(region) and drloc("loadid")
                    'But first - Recalculate loads
                    ld.ReCalculateVolumeAndWeight(AllocatedQty)
                    Dim targetLoc, targetWarehousearea As String
                    SendToLocAssign(targetLoc, targetWarehousearea, pPutAwayPolicy, ld.LOADID, ld.UNITS, pPutRegion, oLogger) 'SendRegToLocAssign(pPutAwayPolicy, ld.LOADID, ld.UNITS, pPutRegion, "%", "%", "%")
                    'and insert the new task
                    If targetLoc <> "" And Not targetLoc Is Nothing Then
                        ld.SetDestinationLocation(targetLoc, targetWarehousearea, pUser)
                        CreateReplTask(ld, targetLoc, targetWarehousearea, PolicyDetail, ld.UNITS, pReplMethod, Nothing, oLogger, pOnContainer)
                        CurrentQty = CurrentQty + AllocatedQty
                        Return AllocatedQty
                    End If
                End If
            Case Else
                ld = getFullReplLoadInRegion(pConsignee, pSku, pNeededQty, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pPolicyId, CurrentQty, oLogger, pLoadid)
                If Not ld Is Nothing Then
                    'call locAssign with fromput(region) and drloc("loadid")
                    Dim targetLoc, targetWarehousearea As String
                    SendToLocAssign(targetLoc, targetWarehousearea, pPutAwayPolicy, ld.LOADID, ld.UNITS, pPutRegion, oLogger) 'SendRegToLocAssign(pPutAwayPolicy, ld.LOADID, ld.UNITS, pPutRegion, "%", "%", "%")
                    'and insert the new task
                    If targetLoc <> "" And Not targetLoc Is Nothing Then
                        ld.SetDestinationLocation(targetLoc, targetWarehousearea, pUser)
                        CreateReplTask(ld, targetLoc, targetWarehousearea, PolicyDetail, ld.UNITS, pReplMethod, Nothing, oLogger, pOnContainer)
                        CurrentQty = CurrentQty + ld.UNITS
                        Return ld.UNITS
                    End If
                Else
                    ld = getPartialReplLoadInRegion(pConsignee, pSku, pNeededQty, PolicyDetail("PICKREGION"), PolicyDetail("UOM"), pPolicyId, AllocatedQty, oLogger, pLoadid)
                    If Not ld Is Nothing Then
                        'call locAssign with fromput(region) and drloc("loadid")
                        'But first - Recalculate loads
                        ld.ReCalculateVolumeAndWeight(AllocatedQty)
                        Dim targetLoc, targetWarehousearea As String
                        SendToLocAssign(targetLoc, targetWarehousearea, pPutAwayPolicy, ld.LOADID, AllocatedQty, pPutRegion, oLogger) 'SendRegToLocAssign(pPutAwayPolicy, ld.LOADID, AllocatedQty, pPutRegion, "%", "%", "%")
                        'and insert the new task
                        If targetLoc <> "" And Not targetLoc Is Nothing _
                                And targetWarehousearea <> "" And Not targetWarehousearea Is Nothing Then
                            ld.SetDestinationLocation(targetLoc, targetWarehousearea, pUser)
                            CreateReplTask(ld, targetLoc, targetWarehousearea, PolicyDetail, AllocatedQty, pReplMethod, PolicyDetail("UOM"), oLogger, pOnContainer)
                            CurrentQty = CurrentQty + AllocatedQty
                            Return AllocatedQty
                        End If
                    End If
                End If
        End Select
        Return 0
    End Function

#Region "Full & Partial replenishment load selecting procedures"

    Public Function getFullReplLoadInRegion(ByVal pConsignee As String, ByVal pSku As String, ByVal pNeededQty As Decimal, ByVal PickRegion As String, ByVal pUom As String, ByVal pPolicyID As String, ByVal CurrentQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadid As String = "") As Load

        Dim sk As SKU = New SKU(pConsignee, pSku)
        If (Not String.IsNullOrEmpty(sk.PARENTSKU)) Then
            If oLogger IsNot Nothing Then oLogger.Write("getFullReplLoadInRegion() is using the parent sku for vReplenishmentInventory SKU {0}, PARENTSKU {1}", sk.SKU, sk.PARENTSKU)
            pSku = sk.PARENTSKU
        End If

        Dim sql As String
        Dim oRepl As New Replenishment
        Dim dt As New DataTable
        If pLoadid = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE <= {2} AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {5} AND LOADID like '%{6}' ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC", pConsignee, pSku, pNeededQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, oRepl.getStockFilterByPriority(PickRegion, pUom), pLoadid)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE <= {2} AND LOADID like '%{5}' ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC", pConsignee, pSku, pNeededQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadid)
        End If
        DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count > 0 Then
            ' Loads found for scoring
            Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
            Dim arrLoadsToCheck As DataRow()
            arrLoadsToCheck = dt.Select()
            'Run scoring procedure
            rps.Score(arrLoadsToCheck, oLogger)
            'if there is more than one load then return the best load from scoring run
            Dim ld As New Load(Convert.ToString(arrLoadsToCheck(0)("LOADID")))
            Return ld
        Else
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND.")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        End If
    End Function

    Public Function getPartialReplLoadInRegion(ByVal pConsignee As String, ByVal pSku As String, ByVal pNeededQty As Decimal, ByVal PickRegion As String, ByRef pUom As String, ByVal pPolicyID As String, ByRef AllocatedQty As Decimal, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadid As String = "") As Load

        Dim sk As SKU = New SKU(pConsignee, pSku)
        If (Not String.IsNullOrEmpty(sk.PARENTSKU)) Then
            If oLogger IsNot Nothing Then oLogger.Write("getPartialReplLoadInRegion() is using the parent sku for vReplenishmentInventory SKU {0}, PARENTSKU {1}", sk.SKU, sk.PARENTSKU)
            pSku = sk.PARENTSKU
        End If

        Dim sql As String
        Dim oRepl As New Replenishment
        Dim dt As New DataTable
        Dim dr As DataRow
        If pLoadid = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {4} AND LOADID like '%{5}' ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC", pConsignee, pSku, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, oRepl.getStockFilterByPriority(PickRegion, pUom), pLoadid)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{4}' ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC", pConsignee, pSku, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadid)
        End If
        DataInterface.FillDataset(sql, dt)
        If Not oLogger Is Nothing Then
            oLogger.Write("Selecting Loads to replenishment by sql: ")
            oLogger.Write(sql)
        End If
        ' Loads found for scoring
        Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
        Dim arrLoadsToCheck As DataRow()
        arrLoadsToCheck = dt.Select()
        'Run scoring procedure
        If Not oLogger Is Nothing Then
            oLogger.Write("Filling loads table for scoring procedure, found " & arrLoadsToCheck.Length)
        End If
        rps.Score(arrLoadsToCheck, oLogger)
        'if there is more than one load then return the best load from scoring run

        If dt.Rows.Count > 0 Then
            Dim oSku As New SKU(pConsignee, pSku)
            ' for each load in after scoring procedure select from the best available
            If Not oLogger Is Nothing Then
                oLogger.Write("Running on loads array after scoring.")
            End If
            For Each dr In arrLoadsToCheck 'dt.Rows
                Dim ld As New Load(Convert.ToString(dr("LOADID")))
                Dim CheckedUom As String
                If pUom Is Nothing Or pUom = "" Or pUom = "%" Then
                    CheckedUom = ld.LOADUOM
                    While True
                        Dim unitsperuom As Decimal = oSku.ConvertToUnits(CheckedUom)
                        If unitsperuom <= pNeededQty Then
                            Dim usedQty As Double = Math.Min(ld.UNITS, pNeededQty)
                            'Dim uomunits As Double = oSku.ConvertUnitsToUom(pUom, usedQty)
                            Dim uomunits As Double = oSku.ConvertUnitsToUom(CheckedUom, usedQty)
                            AllocatedQty = uomunits * unitsperuom
                            pUom = CheckedUom
                            Return ld
                        Else
                            CheckedUom = oSku.getNextUom(CheckedUom)
                            If CheckedUom = "" Or CheckedUom Is Nothing Then
                                Exit While
                            End If
                        End If
                    End While
                Else
                    If ld.LOADUOM = pUom Or oSku.isChildOf(ld.LOADUOM, pUom) Then
                        Dim unitsperuom As Double = oSku.ConvertToUnits(pUom)
                        If unitsperuom <= pNeededQty Then
                            Dim usedQty As Double = Math.Min(ld.UNITS, pNeededQty)
                            Dim uomunits As Double = oSku.ConvertUnitsToUom(pUom, usedQty)
                            AllocatedQty = uomunits * unitsperuom
                            Return ld
                        End If
                    End If
                End If
            Next
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND.")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        Else
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND[BEFORE SCORING PROCEDURE, SQL RESULT 0 ROWS].")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        End If
    End Function

#End Region

#Region "Send To LocAssign"

    Private Function SendToLocAssign(ByRef destLocation As String, ByRef destWarehousearea As String, ByVal pPutAwayPolicy As String, ByVal pLoadid As String, ByVal pUnits As Decimal, ByVal pPutRegion As String, ByVal oLogger As LogHandler)
        Dim SQL As String
        'Dim destLoc As String
        SQL = String.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE, ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " & _
                        " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", pPutAwayPolicy)
        Dim dtRegions As New DataTable
        Dim drRegion As DataRow
        DataInterface.FillDataset(SQL, dtRegions)

        Dim loadObj As New WMS.Logic.Load(pLoadid)
        loadObj.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, WMS.Lib.USERS.SYSTEMUSER)

        For Each drRegion In dtRegions.Rows
            Dim locStorageType, content As String
            Dim fitByWeight, fitByVolume, fitByHeight As Boolean

            If IsDBNull(drRegion("LOCSTORAGETYPE")) Then locStorageType = "%" Else locStorageType = drRegion("LOCSTORAGETYPE")
            If IsDBNull(drRegion("CONTENT")) Then content = "%" Else content = drRegion("CONTENT")

            If IsDBNull(drRegion("FitByWeight")) Then fitByWeight = True Else fitByWeight = drRegion("FitByWeight")
            If IsDBNull(drRegion("FitByVolume")) Then fitByVolume = True Else fitByVolume = drRegion("fitByVolume")
            If IsDBNull(drRegion("FitByHeight")) Then fitByHeight = True Else fitByHeight = drRegion("FitByHeight")


            SendRegToLocAssign(destLocation, destWarehousearea, drRegion("putawaypolicy"), pLoadid, pUnits, pPutRegion, locStorageType, "True", content, _
            fitByWeight, fitByVolume, fitByHeight, oLogger)
            'If destLoc <> "" And Not destLoc Is Nothing Then
            '    'setDestination(destLoc, pLoadId)
            '    Return destLoc
            'End If
        Next

        If String.IsNullOrEmpty(loadObj.DESTINATIONLOCATION) And loadObj.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND Then
            loadObj.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Lib.USERS.SYSTEMUSER)
        End If


    End Function

    Private Function SendRegToLocAssign(ByRef destLocation As String, ByRef destWarehousearea As String, ByVal pPutAwayPolicy As String, ByVal pLoadId As String, ByVal pQty As Decimal, ByVal pRegion As String, ByVal pSTORAGETYPE As String, ByVal pINVENTORY As String, ByVal pCONTENT As String, ByVal pFITBYWEIGHT As Boolean, ByVal pFITBYVOLUME As Boolean, ByVal pFITBYHEIGHT As Boolean, ByVal oLogger As LogHandler) As String
        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message


        oQ.Add("LOADID", pLoadId)
        oQ.Add("REGION", String.Format("{0}", pRegion))
        oQ.Add("POLICYID", pPutAwayPolicy)
        oQ.Add("QTYTOPLACE", pQty)
        oQ.Add("STORAGETYPE", pSTORAGETYPE)
        oQ.Add("INV", pINVENTORY)
        oQ.Add("CONTENT", pCONTENT)
        oQ.Add("FITBYVOLUME", pFITBYVOLUME.ToString())
        oQ.Add("FITBYHEIGHT", pFITBYHEIGHT.ToString())
        oQ.Add("FITBYWEIGHT", pFITBYWEIGHT.ToString())
        oQ.Add("FITBYPALLETTYPE", "True")
        oQ.Add("ACTION", WMS.Lib.Actions.Services.REQUESTLOCATIONBYREGION)
        oQ.Add("USERID", Common.GetCurrentUser())
        Dim qm As Made4Net.Shared.QMsgSender
        Try
            oMsg = oQ.Send("LocAssign")
            qm = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured while sending region to loc assign...")
                oLogger.WriteException(ex)
            End If
        End Try
        If Not qm.Values("ERROR") Is Nothing Then
            Throw New ApplicationException(qm.Values("ERROR"))
        End If
        destLocation = qm.Values("LOCATION")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
    End Function

#End Region

#Region "Create Task"

    Protected Sub CreateReplTask(ByVal ld As Load, ByVal pDestLocation As String, ByVal pDestWarehousearea As String, ByVal PolicyDetail As DataRow, ByVal UnitsAllocated As Double, ByVal pReplMethod As String, Optional ByVal ReplUom As String = Nothing, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pOnContainer As Boolean = False)
        Dim Repl As New Replenishment
        Dim pUser As String = WMS.Lib.USERS.SYSTEMUSER
        If Not oLogger Is Nothing Then
            oLogger.Write("Creating replenishment task")
            oLogger.writeSeperator(" ", 20)
        End If
        Repl.FromLoad = ld.LOADID
        Repl.FromLocation = ld.LOCATION
        Repl.FromWarehousearea = ld.WAREHOUSEAREA
        Repl.ReplMethod = pReplMethod
        If ld.UNITS = UnitsAllocated AndAlso PolicyDetail("REPLTYPE") = Replenishment.ReplenishmentTypes.FullReplenishment Then
            Repl.ReplType = Replenishment.ReplenishmentTypes.FullReplenishment
            Repl.ToLoad = ld.LOADID
            Repl.UOM = ld.LOADUOM
        Else
            Repl.ReplType = PolicyDetail("REPLTYPE")
            Repl.ToLoad = ""

            Dim skuObj As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
            Dim uomUnits As Decimal = skuObj.ConvertUnitsToUom(ld.LOADUOM, UnitsAllocated)
            Dim uomUnitsInt As Integer = uomUnits
            If uomUnits <> uomUnitsInt Then
                Repl.UOM = skuObj.LOWESTUOM
            Else
                Repl.UOM = ld.LOADUOM
            End If
            'Repl.UOM = ReplUom
        End If
        Repl.ToLocation = pDestLocation
        Repl.ToWarehousearea = pDestWarehousearea
        Repl.Units = UnitsAllocated
        If Not oLogger Is Nothing Then
            oLogger.Write("From LoadId " & Repl.FromLoad & " with qty " & UnitsAllocated & " in Location " & Repl.FromLocation & " with replenishment methods :" & Repl.ReplMethod)
            oLogger.Write("To LoadId " & Repl.ToLoad & " with uom " & Repl.UOM)
        End If
        Repl.Post(pUser)
        ld.SetReplenishmentJob(Repl, pUser)
        If PolicyDetail("CREATETASK") And Not pOnContainer Then
            Dim tm As New TaskManager
            tm.CreateReplenishmentTask(Repl, ld, PolicyDetail("TASKPRIORITY"))
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Replenishment task created")
            oLogger.writeSeperator()
        End If
    End Sub

#End Region

#End Region

#End Region

End Class
