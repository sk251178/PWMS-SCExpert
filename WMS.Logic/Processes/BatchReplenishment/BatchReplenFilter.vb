Imports Made4Net.DataAccess
Imports WMS.Logic
Imports Made4Net.General
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports WMS.Lib
Imports Made4Net.Algorithms.Scoring
Imports Made4Net.Algorithms.SortingAlgorithms
Imports Made4Net.Shared

#Region "BatchReplenFilter"

<CLSCompliant(False)> Public Class BatchReplenFilter

#Region "Variables"
    Protected _batchreplenheader As BatchReplenHeader
#End Region

#Region "Properties"




#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal batchreplenheader As BatchReplenHeader)
        _batchreplenheader = batchreplenheader
    End Sub

    Sub Load()

    End Sub

#End Region

#Region "Methods"


#Region "Post"

#End Region

#Region "Replenish"

#Region "Accessors"

    Public Function CreatePolicyDetailTable() As DataTable
        Dim dt As New DataTable
        Dim dc As DataColumn = New DataColumn
        Try
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "PICKREGION"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "UOM"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "REPLTYPE"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.Int32")
            dc.ColumnName = "CREATETASK"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.Int32")
            dc.ColumnName = "TASKPRIORITY"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "ALLOCUOMQTY"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "TASKSUBTYPE"
            dt.Columns.Add(dc)
            dc = New DataColumn
            dc.DataType = System.Type.GetType("System.String")
            dc.ColumnName = "POLICYID"
            dt.Columns.Add(dc)
        Catch ex As Exception

        End Try
        Return dt
    End Function

    Public Function CreatePolicyDetailRow(ByVal oPolicyDetail As DataTable, ByVal sPolicyID As String, ByVal sPriority As String) As DataRow
        Dim rpLn As DataTable = New DataTable
        Dim masterDao As MasterDao = New MasterDao()
        Dim priorityNormalValueStr As String = masterDao.GetPriorityCodeForTask(WMS.Lib.TASKPRIORITY.PRIORITY_NORMAL)
        DataInterface.FillDataset("SELECT ISNULL(POLICYID,'')as POLICYID,REPLACE(ISNULL(FROMPICKREGION,'%'),'*','%') AS FROMPICKREGION, REPLACE(ISNULL(UOM ,'%'),'*','%') AS UOM, ISNULL(REPLTYPE,'') AS REPLTYPE,ISNULL(CREATETASK,0) AS CREATETASK,ISNULL(TASKPRIORITY," & priorityNormalValueStr & ") AS TASKPRIORITY, ISNULL(ALLOCUOMQTY,'') as ALLOCUOMQTY,ISNULL(TASKSUBTYPE,'') as TASKSUBTYPE FROM REPLPOLICYDETAIL WHERE POLICYID = '" & sPolicyID & "' AND PRIORITY = '" & sPriority & "'", rpLn)
        Dim lnPickRegion As String
        Dim lnUom As String
        Dim lnReplType As String
        Dim lnCreateTask As Integer
        Dim lnTaskPriorety As String
        Dim lnAllocUOMQty As String
        Dim lnTaskSubType As String
        Dim lnPolicyID As String
        For Each ln As DataRow In rpLn.Rows
            If ln("FROMPICKREGION") = "%" Then
                lnPickRegion = ln("FROMPICKREGION")
            End If
            If lnPickRegion <> "%" Then
                lnPickRegion = lnPickRegion & ln("FROMPICKREGION") & ","
            End If
            If ln("UOM") = "%" Then
                lnUom = ln("UOM")
            End If
            If lnUom <> "%" Then
                lnUom = lnUom & ln("UOM") & ","
            End If
            lnReplType = ln("REPLTYPE")
            If ln("CREATETASK") = "1" Then
                lnCreateTask = 1
            End If
            If lnCreateTask <> 1 Then
                lnCreateTask = ln("CREATETASK")
            End If
            lnTaskPriorety = ln("TASKPRIORITY")
            lnAllocUOMQty = ln("ALLOCUOMQTY")
            lnTaskSubType = ln("TASKSUBTYPE")
            lnPolicyID = ln("POLICYID")
        Next
        Dim tmpLn As DataRow = oPolicyDetail.NewRow

        tmpLn("PICKREGION") = lnPickRegion.TrimEnd(",")
        tmpLn("UOM") = lnUom.TrimEnd(",")
        tmpLn("REPLTYPE") = lnReplType
        tmpLn("CREATETASK") = lnCreateTask
        tmpLn("TASKPRIORITY") = lnTaskPriorety
        tmpLn("ALLOCUOMQTY") = lnAllocUOMQty
        tmpLn("TASKSUBTYPE") = lnTaskSubType
        tmpLn("POLICYID") = lnPolicyID
        Return tmpLn
    End Function

    Public Function getStockFilterByPriority(ByVal sPickRegions As String, ByVal sUoms As String) As String
        Dim aPickRegions As String() = sPickRegions.Split(",")
        Dim aUoms As String() = sUoms.Split(",")

        If aPickRegions.Length = 1 Then
            Return " (PICKREGION LIKE '" & sPickRegions & "' AND LOADUOM LIKE '" & sUoms & "') "
        End If
        If aUoms.Length >= 1 Then
            sUoms = aUoms(0)
        End If
        Dim SQLFilter As String = "LOADUOM LIKE '" & sUoms & "' AND ("
        For i As Int32 = 0 To aPickRegions.Length - 1
            SQLFilter = SQLFilter & " (PICKREGION LIKE '" & aPickRegions(i) & "') OR"
        Next
        SQLFilter = " (" & SQLFilter.TrimEnd("OR".ToCharArray) & ")) "
        Return SQLFilter
    End Function



#End Region
#End Region

#Region " BatchReplenFilter"

    Protected Sub FindQualifiedBatchPickLocations(ByRef qualifiedbatchpicklocs As ArrayList, Optional ByVal oLogger As LogHandler = Nothing)
        Dim batchpicklocdt As VPickLocDataTable = New VPickLocDataTable
        Dim sql As String = String.Format("select * from vpickloc where  BATCHPICKLOCATION ='1' and pickregion='" & _batchreplenheader.PICKREGION & "'")
        DataInterface.FillDataset(sql, batchpicklocdt)
        If batchpicklocdt.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then oLogger.Write("None of the batch pick locationds qualified for batch replenishment for the selected pickregion:." & _batchreplenheader.PICKREGION.ToString())
        End If
        For Each dr As DataRow In batchpicklocdt.Rows
            Dim pickloc As PickLoc = New PickLoc(dr)
            Dim locobj As Location = New Location(pickloc.Location, pickloc.Warehousearea)
            pickloc.AllocatedQty = pickloc.AllocatedQty + dr.Item("LOADALLOCATEDQTY")
            If locobj.STATUS = True AndAlso locobj.PROBLEMFLAG = False AndAlso pickloc.CurrentQty + pickloc.PendingQty - pickloc.AllocatedQty <= pickloc.ReplQty AndAlso pickloc.HasBatchReplenScheduled(_batchreplenheader.PICKREGION) = False Then
                qualifiedbatchpicklocs.Add(pickloc)
                If Not oLogger Is Nothing Then oLogger.Write(pickloc.Location.ToString() & " (Picklocation) is qualified to add to batch replenishment")
            Else
                If Not oLogger Is Nothing Then oLogger.Write(pickloc.Location.ToString() & " (Picklocation) not qualified to add to batch replenishment")
            End If
        Next
        If Not oLogger Is Nothing Then oLogger.Write("Filtered Batch Pick Locations for Replenishment.")
    End Sub

    Protected Sub GenerateBatchReplenObjsForPickLoc(ByVal policyDetails As DataTable, ByVal pl As PickLoc, ByRef batchrepobjs As BatchReplenDetailCollection, Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        Dim oFromSku As SKU
        Dim oToSku As SKU = New SKU(pl.Consignee, pl.SKU)
        Dim fromSkuUom As String
        Dim toSkuUom As String = oToSku.LOWESTUOM
        Dim reqFromQty As Decimal
        Dim netQtyAtPickloc As Decimal = pl.CurrentQty + pl.PendingQty - pl.AllocatedQty
        If oToSku.PARENTSKU = String.Empty Then
            oFromSku = oToSku
            fromSkuUom = toSkuUom
            reqFromQty = pl.MaximumReplQty - netQtyAtPickloc
        Else
            oFromSku = New SKU(pl.Consignee, oToSku.PARENTSKU)
            fromSkuUom = oFromSku.LOWESTUOM
            reqFromQty = oToSku.ConvertMyLowestUomUnitsToParentLowestUomUnits(pl.MaximumReplQty - netQtyAtPickloc)
        End If


        If reqFromQty <= 0 Then Return
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Batch PickLoc={0}, SKU={1}, ReplQty={2}, MaxReplQty={3}, CurrentQty={4},PendingQty={5},TotalAllocatedQty=AllocatedQty+LoadAllocatedQty={6},OverAllocatedQty={7}", pl.Location, pl.SKU, pl.ReplQty, pl.MaximumReplQty, pl.CurrentQty, pl.PendingQty, pl.AllocatedQty, pl.OverAllocatedQty))
            oLogger.Write(String.Format("NetQuantity = CurrentQty + PendingQty - TotalAllocatedQty = {0}", netQtyAtPickloc))
            oLogger.Write(String.Format("Quantity Needed = MaxReplQty - NetQuantity = {0}", pl.MaximumReplQty - netQtyAtPickloc))
            oLogger.Write(String.Format("From Sku:{0}, From SkuUom:{1}, Required From Qty:{2}", oFromSku.SKU, fromSkuUom, reqFromQty))
        End If

        Dim pd As DataRow
        'Loop over each policy detail based on priority
        For Each pd In policyDetails.Rows
            'Searching for Loads
            Dim sql As String = String.Format("SELECT * FROM vBatchReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {2} ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC",
                                                pl.Consignee, oFromSku.SKU, getStockFilterByPriority(pd("PICKREGION"), pd("UOM")))
            Dim loaddt As New DataTable
            If Not oLogger Is Nothing Then oLogger.Write("Selecting loads" & vbNewLine & sql)
            DataInterface.FillDataset(sql, loaddt)

            'Run scoring procedure - arrange loads best to worst
            Dim rps As BatchReplenishmentPolicyScoring = New BatchReplenishmentPolicyScoring(_batchreplenheader.REPLENPOLICY)
            Dim arrLoadsToCheck As DataRow()
            arrLoadsToCheck = loaddt.Select()
            If Not oLogger Is Nothing Then
                oLogger.Write("Filling loads table for scoring procedure, found " & arrLoadsToCheck.Length)
            End If
            rps.Score(arrLoadsToCheck, oLogger)

            If Not oLogger Is Nothing Then oLogger.Write("Running on loads array after scoring.")
            Dim isAnyFound As Boolean = False
            Dim loaddr As DataRow
            'loop over all filtered loads
            For Each loaddr In arrLoadsToCheck
                If reqFromQty = 0 Then Exit For
                Dim ld As New Load(Convert.ToString(loaddr("LOADID")))
                If ld.UNITS - ld.UNITSALLOCATED <= 0 Then Continue For
                isAnyFound = True
                'determine the available load qty 
                Dim ldavaliableqty = ld.UNITS - ld.UNITSALLOCATED
                Dim allocatedQty As Decimal
                Dim toQty As Decimal
                If (ldavaliableqty <= reqFromQty) Then
                    allocatedQty = ldavaliableqty
                    reqFromQty = reqFromQty - ldavaliableqty
                    'Allocate load 
                    ld.Allocate(allocatedQty, UserId)
                    'toQty = oToSku.ConvertToUnits(fromSkuUom) * allocatedQty
                Else
                    allocatedQty = reqFromQty
                    reqFromQty = 0
                    'Allocate load 
                    ld.Allocate(allocatedQty, UserId)
                    'toQty = oToSku.ConvertToUnits(fromSkuUom) * allocatedQty
                End If
                Dim brdetail As BatchReplenDetail = New BatchReplenDetail
                brdetail.FROMSKUBASEUOM = oFromSku.DEFAULTRECUOM
                brdetail.CONSIGNEE = oToSku.CONSIGNEE
                brdetail.TOSKU = oToSku.SKU
                brdetail.TOSKUUOM = oToSku.LOWESTUOM
                brdetail.FROMSKU = oFromSku.SKU
                brdetail.FROMSKUUOM = oFromSku.LOWESTUOM
                brdetail.TOLOCATION = pl.Location
                brdetail.TOWAREHOUSEAREA = pl.Warehousearea
                'Create and Add Batch Replen Detail to the collection
                brdetail.FROMWAREHOUSEAREA = ld.WAREHOUSEAREA
                brdetail.FROMLOCATIONSORTORDER = (New Location(ld.LOCATION)).LOCSORTORDER
                brdetail.FROMLOCATION = ld.LOCATION
                brdetail.FROMLOAD = ld.LOADID
                brdetail.FROMQTY = allocatedQty
                If brdetail.FROMSKUUOM = brdetail.FROMSKUBASEUOM Then
                    brdetail.FROMSKUBASEUOMQTY = brdetail.FROMQTY
                Else
                    brdetail.FROMSKUBASEUOMQTY = oFromSku.ConvertUnitsToUom(brdetail.FROMSKUBASEUOM, brdetail.FROMQTY)
                End If

                If brdetail.FROMSKU <> brdetail.TOSKU Then
                    brdetail.TOQTY = oToSku.ConvertParentLowestUomUnitsToMyLowestUomUnits(brdetail.FROMQTY)
                Else
                    brdetail.TOQTY = oToSku.ConvertToUnits(brdetail.FROMSKUUOM) * brdetail.FROMQTY
                End If

                batchrepobjs.Add(brdetail)
                If Not oLogger Is Nothing Then oLogger.Write(String.Format("Batch replen line item with updated values.FromSKU={0}, FROMSKUUOM={1},FromQty={2},FROMSKUBASEUOM={3},FROMSKUBASEUOMQTY={4},FromLoadId={5}", brdetail.FROMSKU, brdetail.FROMSKUUOM, brdetail.FROMQTY, brdetail.FROMSKUBASEUOM, brdetail.FROMSKUBASEUOMQTY, ld.LOADID))
                If Not oLogger Is Nothing Then oLogger.Write(String.Format("Load Allocated. LoadID={0},Allocated Qty={1}", ld.LOADID, allocatedQty))
            Next
            If isAnyFound = False AndAlso Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND IN REGION:" & pd("PICKREGION"))
            End If
            If reqFromQty = 0 Then
                If Not oLogger Is Nothing Then oLogger.Write("No more Inventory required. Requirement is fullfilled for Batch Pick Location:" & pl.Location)
                Exit For
            End If
        Next

    End Sub

    Public Function GenerateBatchReplenObjs(Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing) As BatchReplenDetailCollection
        Dim qualifiedbatchpicklocs As New ArrayList
        ' Filter Batch Pick Locations
        FindQualifiedBatchPickLocations(qualifiedbatchpicklocs, oLogger)
        Dim batchrepobjs As New BatchReplenDetailCollection

        'Build policyDetails 
        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Using {0} as replenishment policy.", _batchreplenheader.REPLENPOLICY))
        Dim prTbl As DataTable = New DataTable
        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & _batchreplenheader.REPLENPOLICY & "' GROUP BY PRIORITY order by PRIORITY", prTbl)
        'For each priority found build policy detail table group by priority
        Dim policyDetails As DataTable = CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            policyDetails.Rows.Add(CreatePolicyDetailRow(policyDetails, _batchreplenheader.REPLENPOLICY, pr("PRIORITY")))
        Next
        Dim pl As PickLoc
        For Each pl In qualifiedbatchpicklocs
            'Find inventory in RESERVE for each PickLoc
            GenerateBatchReplenObjsForPickLoc(policyDetails, pl, batchrepobjs, UserId, oLogger)
        Next

        If Not oLogger Is Nothing Then oLogger.Write(String.Format("QuickSort: Based on FROMLOCATIONSORTORDER & FROMSKU"))
        batchrepobjs.Sort("SORTORDER", BreplenDetailSortDirection.Ascending)
        'Assign line no's to Batch Replen Objects
        For index As Integer = 0 To batchrepobjs.Count() - 1
            batchrepobjs.Item(index).REPLLINE = index + 1
        Next
        Return batchrepobjs
    End Function

#End Region

#End Region


#End Region

End Class

#Region "Batch Replenishment Policy Scoring"

<CLSCompliant(False)> Public Class BatchReplenishmentPolicyScoring

#Region "Variables"

    Protected _policyid As String
    Protected _attributesscoring As Made4Net.DataAccess.Collections.GenericCollection

#End Region

#Region "Properties"

    Public ReadOnly Property PolicyId() As String
        Get
            Return _policyid
        End Get
    End Property

    Public ReadOnly Property Key(ByVal idx As Int32) As String
        Get
            Return _attributesscoring.Keys(idx)
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal idx As Int32) As Decimal
        Get
            Return _attributesscoring(idx)
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal sKey As String) As Decimal
        Get
            Return _attributesscoring(sKey)
        End Get
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal sPolicyId As String)
        _policyid = sPolicyId
        Load()
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim sql As String = String.Format("SELECT * FROM REPLPOLICYSCORING WHERE POLICYID = {0}", Made4Net.Shared.Util.FormatField(_policyid))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            _attributesscoring = New Made4Net.DataAccess.Collections.GenericCollection
            For Each oCol As DataColumn In dt.Columns
                If Not oCol.ColumnName.ToLower = "policyid" Then
                    _attributesscoring.Add(oCol.ColumnName, Convert.ToDecimal(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr(oCol.ColumnName), "0")))
                End If
            Next
        End If
        dt.Dispose()
    End Sub

    Public Sub Score(ByRef cLoadsCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write("Start Scoring the loads collection...")
        End If
        If cLoadsCollection.Length = 0 Then Return
        Dim oScoreSetter As ScoreSetter =
            New ScoreSetter.ScoreSetterBuilder() _
            .Logger(oLogger) _
            .Build()

        'Added for RWMS-1829
        If Not _attributesscoring Is Nothing Then
            oScoreSetter.Score(cLoadsCollection, _attributesscoring)
        End If
        'Ended for RWMS-1829

        Try
            Dim first As Boolean = True
            For Each LoadRow As DataRow In cLoadsCollection
                Dim DataRowStr, CaptionRowStr As String
                If first Then
                    If oLogger IsNot Nothing Then
                        oLogger.Write("Load".PadRight(23) & "Location".PadRight(10) & "Status".PadRight(11) & "QTY".PadRight(10) & "Allocated".PadRight(11) & "Available".PadRight(11) & "Score".PadRight(10))
                        oLogger.writeSeperator("-", 80)
                        oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    End If
                    first = False
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    End If
                End If
            Next
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator(" ", 20)
            End If
        Catch ex As Exception
        End Try
    End Sub

#End Region

End Class

#End Region



#Region "Batch Replenishment Policy"

<CLSCompliant(False)> Public Class BatchReplenishmentPolicy

#Region "Variables"

#Region "PrimaryKeys"
    Dim _policyid As String
#End Region

#Region "Other Fields"
    Dim _policyname As String
    Dim _batchlabelformat As String
    Dim _adddate As DateTime
    Dim _adduser As String
    Dim _editdate As DateTime
    Dim _edituser As String
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("PolicyID={0}", Made4Net.Shared.FormatField(_policyid))
        End Get
    End Property

    Public Property PolicyID() As String
        Get
            Return _policyid
        End Get
        Set(ByVal value As String)
            _policyid = value
        End Set
    End Property

    Public Property PolicyName() As String
        Get
            Return _policyname
        End Get
        Set(ByVal value As String)
            _policyname = value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal value As DateTime)
            _adddate = value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal value As String)
            _adduser = value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal value As DateTime)
            _editdate = value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal value As String)
            _edituser = value
        End Set
    End Property
#End Region

#Region "Constructors"
    Public Sub New()

    End Sub

    Public Sub New(ByVal pPolicyId As String)
        _policyid = pPolicyId
    End Sub

    Public Function GetCubeLimit(ByVal pPolicyId As String) As Decimal
        Dim sql As String = String.Format("Select TOP 1 IsNull(CUBELIMIT,0) as CubeLimit FROM REPLPOLICY WHERE POLICYID = '{0}'", pPolicyId)
        Try
            Return CType(DataInterface.ExecuteScalar(sql), Decimal)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetStagingLocation(ByVal pPolicyId As String) As String
        Dim sql As String = String.Format("Select TOP 1 IsNull(STAGINGLOCATION,'') as CubeLimit FROM REPLPOLICY WHERE POLICYID = '{0}'", pPolicyId)
        Try
            Return CType(DataInterface.ExecuteScalar(sql), String)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetBatchLabelFormat(ByVal pPolicyId As String) As String
        Dim sql As String = String.Format("Select TOP 1 IsNull(BATCHLABELFORMAT,'') as BatchLabelFormat FROM REPLPOLICY WHERE POLICYID = '{0}'", pPolicyId)
        Try
            Return Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("Select TOP 1 BATCHLABELFORMAT as BatchLabelFormat FROM REPLPOLICY WHERE POLICYID = '{0}'", pPolicyId))
        Catch ex As Exception
            Return 0
        End Try
    End Function
#End Region


End Class

#End Region