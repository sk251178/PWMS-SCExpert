Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data

Public Class StagingLaneAssignmentSetter

    Public Function SetDocumentStagingLane(ByVal pDocType As String, ByVal pConsignee As String, ByVal pDocId As String, Optional ByVal oLogger As Made4Net.Shared.Logging.LogFile = Nothing) As Boolean
        'Set the appropriate Assign Policy Table
        Dim docRow As DataRow = GetDocParamsRow(pDocType, pConsignee, pDocId, oLogger)
        If docRow Is Nothing Then Return False
        Dim dtSLPolicies As DataTable = GetSLAssignmentPolicies(docRow("consignee"), docRow("doctype"), docRow("ordertype"), docRow("company"), docRow("companytype"), docRow("carrier"), docRow("transporttype"), docRow("orderpriority"), docRow("route"), docRow("door"), oLogger)
        If dtSLPolicies Is Nothing Then Return False
        'Find Fitable SL for the current order
        'The try catch block is for SL which are not numeric..... should implement it as well
        Dim sl As String = ""
        Dim stagingWarehousArea As String = ""
        'sl = FindSL(docRow, dtSLPolicies, oLogger)
        FindSL(sl, stagingWarehousArea, docRow, dtSLPolicies, oLogger)
        If String.IsNullOrEmpty(stagingWarehousArea) Then
            Return False
        End If
        If sl <> "" Then
            If Not oLogger Is Nothing Then
                oLogger.WriteLine(String.Format("Setting staging lane: {0}", sl))
            End If
            Try
                'SetSL(sl, "", pDocType, pConsignee, pDocId, oLogger)
                SetSL(sl, stagingWarehousArea, pDocType, pConsignee, pDocId, oLogger)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End If
        Return False
    End Function

    Private Function GetSLAssignmentPolicies(ByVal pCONSIGNEE As String, ByVal pDOCUMENTTYPE As String, ByVal pORDERTYPE As String, ByVal pCOMPANY As String, ByVal pCOMPANYTYPE As String, ByVal pCARRIER As String, ByVal pTRANSMETHOD As String, ByVal pPRIORITY As Int32, ByVal pROUTE As String, ByVal pDOOR As String, ByVal oLogger As Made4Net.Shared.Logging.LogFile) As DataTable
        Dim dtSLPolicies As New DataTable
        Dim SQL As String
        SQL = String.Format("SELECT CONSIGNEE, PRIORITY, FROMSTAGINGLANE, TOSTAGINGLANE, STAGINGLANEINTERVAL, ISNULL(LASTUSEDSL, FROMSTAGINGLANE) AS LASTUSEDSL, STAGINGWAREHOUSEAREA FROM STAGINGLANEASSIGNMENT WHERE '{0}' like isnull(replace(CONSIGNEE,'','%'),'%') and '{1}' like isnull(replace(DOCUMENTTYPE,'','%'),'%') and '{2}' like isnull(replace(ordertype,'','%'),'%') " & _
                " and '{3}' like isnull(replace(company,'','%'),'%') and '{4}' like isnull(replace(companytype,'','%'),'%') and '{5}' like isnull(replace(CARRIER,'','%'),'%') and '{6}' like isnull(replace(TRANSMETHOD,'','%'),'%')" & _
                " and ({7} >= isnull(FROMPRIORITY,0) and {7} <= isnull(TOPRIORITY,0)) and ('{8}' >= isnull(FROMROUTE,'') and '{8}' <= isnull(TOROUTE,'ZZZZZZZZZZZZZZ')) and ('{9}' >= isnull(FROMDOOR,'') and '{9}' <= isnull(TODOOR,'ZZZZZZZZZZZZZZ'))" & _
                " order by priority ", pCONSIGNEE, pDOCUMENTTYPE, pORDERTYPE, pCOMPANY, pCOMPANYTYPE, pCARRIER, pTRANSMETHOD, pPRIORITY, pROUTE, pDOOR)

        If Not oLogger Is Nothing Then
            oLogger.WriteLine("Trying to get SL assignment policies:")
            oLogger.WriteLine(String.Format("SQL: {0}", SQL))
        End If

        DataInterface.FillDataset(SQL, dtSLPolicies)
        If Not oLogger Is Nothing Then
            oLogger.WriteLine(String.Format("Found {0} lines.", dtSLPolicies.Rows.Count.ToString()))
        End If
        If dtSLPolicies.Rows.Count > 0 Then
            Return dtSLPolicies
        Else
            Return Nothing
        End If
    End Function

    Private Function GetDocParamsRow(ByVal pDocType As String, ByVal pConsignee As String, ByVal pDocId As String, ByVal oLogger As Made4Net.Shared.Logging.LogFile) As DataRow
        Dim SQL As String = String.Format("select * from DOCUMENTSSLASSIGN where consignee = '{0}' and orderid = '{1}' and doctype = '{2}'", _
                pConsignee, pDocId, pDocType)
        If Not oLogger Is Nothing Then
            oLogger.WriteLine("Trying to get doc params:")
            oLogger.WriteLine(String.Format("SQL: {0}", SQL))
        End If
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If Not oLogger Is Nothing Then
            oLogger.WriteLine(String.Format("Found {0} lines.", dt.Rows.Count.ToString()))
        End If
        If dt.Rows.Count = 1 Then
            Return dt.Rows(0)
        Else
            Return Nothing
        End If
    End Function

    'Private Function FindSL(ByVal drOrderParam As DataRow, ByVal dtPolicy As DataTable, ByVal oLogger As Made4Net.Shared.Logging.LogFile) As String
    Private Sub FindSL(ByRef pStagingLocation As String, ByRef pStagingWarehouseArea As String, ByVal drOrderParam As DataRow, ByVal dtPolicy As DataTable, ByVal oLogger As Made4Net.Shared.Logging.LogFile)
        'Load all SL Capacity for non shipped orders
        'Dim dtSLCurrCapacity As New DataTable
        'Dim SQL As String = String.Format("select staginglane, sum(ordervolume) as TotalVolume, sum(orderweight) as TotalWeight from DOCUMENTSSLASSIGN group by staginglane")
        'DataInterface.FillDataset(SQL, dtSLCurrCapacity)

        'Loop the policies and find available SL
        Dim sl As String = String.Empty
        If Not oLogger Is Nothing Then
            oLogger.WriteLine("Starting to loop on found policies.")
        End If
        Dim policyCounter As Integer = 1
        Dim lastUsedSL As String

        For Each drPolicy As DataRow In dtPolicy.Rows

            Dim lastUsed As String = drPolicy("LASTUSEDSL")
            ' If lastUsed = drPolicy("TOSTAGINGLANE") Then
            If String.IsNullOrEmpty(lastUsed) Then
                lastUsed = drPolicy("FROMSTAGINGLANE")
            End If
            lastUsedSL = lastUsed
            If Not oLogger Is Nothing Then
                oLogger.WriteLine(String.Format("Checking policy {0}/{1}", policyCounter.ToString(), dtPolicy.Rows.Count.ToString()))
                oLogger.WriteLine("From staging lane: " + drPolicy("FROMSTAGINGLANE"))
                oLogger.WriteLine("To staging lane: " + drPolicy("TOSTAGINGLANE"))
                oLogger.WriteLine("Last used staging lane: " + lastUsed)
                oLogger.WriteLine(String.Format("Staging lane interval: {0}", drPolicy("STAGINGLANEINTERVAL").ToString()))
                oLogger.WriteLine(String.Format("Order weight: {0}", drOrderParam("orderweight").ToString()))
                oLogger.WriteLine(String.Format("Order volume: {0}", drOrderParam("ordervolume").ToString()))
            End If
            'sl = FindSL(lastUsed, drPolicy("TOSTAGINGLANE"), drPolicy("STAGINGLANEINTERVAL"), drOrderParam("ordervolume"), drOrderParam("orderweight"), dtSLCurrCapacity)
            sl = locateSL(drPolicy("FROMSTAGINGLANE"), drPolicy("TOSTAGINGLANE"), drPolicy("STAGINGLANEINTERVAL"), lastUsed, drOrderParam("orderweight"), drOrderParam("ordervolume"), oLogger)
            If sl <> "" Then

                'Update the last used SL
                'Dim lastUsedSL As String
                'If sl.ToLower = Convert.ToString(drPolicy("TOSTAGINGLANE")).ToLower Then
                '    lastUsedSL = drPolicy("FROMSTAGINGLANE")
                'Else
                lastUsedSL = sl
                'End If
                Dim sql As String
                sql = String.Format("UPDATE STAGINGLANEASSIGNMENT SET LASTUSEDSL = {0} where CONSIGNEE= {1} AND PRIORITY = {2}", _
                                        Made4Net.Shared.FormatField(lastUsedSL), _
                                        Made4Net.Shared.FormatField(drPolicy("CONSIGNEE")), _
                                        Made4Net.Shared.FormatField(drPolicy("priority")) _
                                    )

                If Not oLogger Is Nothing Then
                    oLogger.WriteLine("Updating last used staging lane")
                    'Dim sql As String = String.Format("UPDATE STAGINGLANEASSIGNMENT SET LASTUSEDSL = (select locsortorder from location where location = '{0}') where PRIORITY = {1}", lastUsedSL, drPolicy("priority"))
                    oLogger.WriteLine("SQL: " + sql)
                End If
                DataInterface.RunSQL(sql)

                If Not oLogger Is Nothing Then
                    oLogger.WriteLine("Staging lane found: " + sl)
                    oLogger.WriteLine(String.Format("Finished checking policy {0}/{1}", policyCounter.ToString(), dtPolicy.Rows.Count.ToString()))
                    policyCounter += 1
                End If
                pStagingLocation = sl
                pStagingWarehouseArea = drPolicy("STAGINGWAREHOUSEAREA").ToString()
                Exit For
            End If
            If Not oLogger Is Nothing Then
                oLogger.WriteLine("No staging lane found.")
                oLogger.WriteLine(String.Format("Finished checking policy {0}/{1}", policyCounter.ToString(), dtPolicy.Rows.Count.ToString()))
                policyCounter += 1
            End If
        Next
        'Return sl
        ''ADDED BY ODED
        If String.IsNullOrEmpty(pStagingWarehouseArea) Then
            Try
                pStagingWarehouseArea = DataInterface.ExecuteScalar(String.Format("select WAREHOUSEAREA from vSL WHERE LOCATION = {0}", Made4Net.Shared.FormatField(lastUsedSL))).ToString()

            Catch ex As Exception
                pStagingWarehouseArea = Nothing
            End Try
            End If
    End Sub

    Private Function FindSL(ByVal pFromSL As String, ByVal pToSL As String, ByVal SLInterval As Int32, ByVal pOrderWeight As Decimal, ByVal pOrderVolume As Decimal, ByVal pDtSLContent As DataTable) As String
        Dim tmpSL As Location
        Dim CurrentSL As String
        Dim CurrVol As Decimal = 0, CurrWeight As Decimal = 0
        Dim dtSLS As New DataTable
        Dim SQL As String = String.Format("select * from vSL where locsortorder > '{0}' and locsortorder <= '{1}' order by locsortorder", pFromSL, pToSL)
        DataInterface.FillDataset(SQL, dtSLS)
        Dim found As Boolean = False
        Dim TotalLocations As Int32 = dtSLS.Rows.Count
        If TotalLocations = 0 Then
            Throw New M4NException(New Exception, "No Available Staging lane locations found!", "No Available Staging lane locations found!")
        End If
        Dim i As Int32 = 0
        While (Not found)
            CurrentSL = dtSLS.Rows(i)("location")
            Dim SLRow() As DataRow = pDtSLContent.Select(String.Format("staginglane = '{0}'", CurrentSL))
            If SLRow.Length = 1 Then
                CurrVol = SLRow(0)("TotalVolume")
                CurrWeight = SLRow(0)("TotalWeight")
            End If
            tmpSL = New Location(CurrentSL, dtSLS.Rows(i)("warehousearea"))
            'If (tmpSL.CUBIC >= CurrVol + pOrderVolume) And (tmpSL.WEIGHT >= CurrWeight + pOrderWeight) Then
            '    found = True
            '    Return CurrentSL
            'End If
            If (tmpSL.CUBIC >= pOrderVolume) And (tmpSL.WEIGHT >= pOrderWeight) Then
                found = True
                Return CurrentSL
            End If
            If (i + SLInterval < TotalLocations) Then
                i += SLInterval
            Else
                i = (i + SLInterval) Mod TotalLocations
            End If
        End While
        Return CurrentSL
    End Function

    Private Function locateSL(ByVal pFromSL As String, ByVal pToSL As String, ByVal pSLInterval As Int32, ByVal pLastUsedSL As String, ByVal pOrderWeight As Decimal, ByVal pOrderVolume As Decimal, ByVal oLogger As Made4Net.Shared.Logging.LogFile) As String
        Dim dtSLS As New DataTable
        Dim SQL As String = String.Format("select * from vSL where location >= '{0}' and location <= '{1}' order by locsortorder", pFromSL, pToSL)
        If Not oLogger Is Nothing Then
            oLogger.WriteLine("Selecting all possible staging lanes from DB")
            oLogger.WriteLine(String.Format("SQL: {0}", SQL))
        End If

        DataInterface.FillDataset(SQL, dtSLS)
        If Not oLogger Is Nothing Then
            oLogger.WriteLine(String.Format("Found {0} possible staging lanes.", dtSLS.Rows.Count.ToString()))
        End If

        If dtSLS.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.WriteLine("No Available Staging lane locations found!")
            End If
            'The exception here causes the sl process not to check the other strategies. Why is it here?
            'Throw New M4NException(New Exception, "No Available Staging lane locations found!", "No Available Staging lane locations found!")
            Return ""
        End If

        Dim i As Int32
        Dim finishedLoop As Boolean = False
        Try
            Dim indexOfLastUsed = dtSLS.Rows.IndexOf(dtSLS.Select(String.Format(String.Format("location='{0}'", pLastUsedSL)))(0))
            'Dim indexOfLastUsed = dtSLS.Rows.IndexOf(dtSLS.Select(String.Format(String.Format("locsortorder='{0}'", pLastUsedSL)))(0))
            'Dim indexOfLastUsed As Int32
            'indexOfLastUsed = dtSLS.Rows.IndexOf(dtSLS.Select(String.Format(String.Format("location={0}", Made4Net.Shared.FormatField(pLastUsedSL))))(0))
            If dtSLS.Rows.Count > indexOfLastUsed + pSLInterval Then
                i = indexOfLastUsed + pSLInterval
            Else
                i = (i + pSLInterval) Mod dtSLS.Rows.Count - 1
                finishedLoop = True
            End If
        Catch ex As Exception
            i = 0
        End Try

        Dim firstTriedIndex As Integer = i

        While (True)
            Dim currentSL As Location = New Location(dtSLS.Rows(i)("location"), dtSLS.Rows(i)("warehousearea"))
            If (currentSL.CUBIC >= pOrderVolume) And (currentSL.WEIGHT >= pOrderWeight) Then
                Return currentSL.Location
            End If

            If (i + pSLInterval < dtSLS.Rows.Count) Then
                i += pSLInterval
            Else
                If Not finishedLoop Then
                    finishedLoop = True
                Else
                    Return ""
                End If
                i = (i + pSLInterval) Mod dtSLS.Rows.Count
            End If
        End While
    End Function

    Private Sub SetSL(ByVal pSL As String, ByVal pStagingWHArea As String, ByVal pDocType As String, ByVal pConsignee As String, ByVal pDocId As String, ByVal oLogger As Made4Net.Shared.Logging.LogFile)
        Try
            If Not oLogger Is Nothing Then
                oLogger.WriteLine("Assigning staging lane to order")
                oLogger.WriteLine(String.Format("Found staginglane: {0}", pSL))
                oLogger.WriteLine(String.Format("Order id: {0}", pDocId))
                oLogger.WriteLine(String.Format("Document type: {0}", pDocType))
                oLogger.WriteLine(String.Format("Consignee: {0}", pConsignee))
            End If
            Select Case pDocType
                Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
                    Dim oOrd As New OutboundOrderHeader(pConsignee, pDocId)
                    oOrd.SetStagingLane(pSL, pStagingWHArea, oOrd.REQUESTEDDATE, "SLASSIGN")
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim oOrd As New Flowthrough(pConsignee, pDocId)
                    oOrd.SetStagingLane(pSL, pStagingWHArea, "SLASSIGN")
                Case WMS.Lib.DOCUMENTTYPES.TRANSHIPMENT
                    Dim oOrd As New TransShipment(pConsignee, pDocId)
                    oOrd.SetStagingLane(pSL, pStagingWHArea, "SLASSIGN")
            End Select
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.WriteLine("Error occured while trying to assign staging lane to order:")
                oLogger.WriteLine(ex.ToString())
            End If
        End Try
    End Sub

End Class
