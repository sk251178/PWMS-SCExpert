Imports Made4Net.DataAccess
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging


<CLSCompliant(False)> Public Class LoadsWithSequence
    Public Sub New()
    End Sub

    Private ld As Load
    Public Property Load() As Load
        Get
            Return ld
        End Get
        Set(value As Load)
            ld = value
        End Set
    End Property
    Private sequence_No As Integer
    Public Property SequenceNo() As Integer
        Get
            Return sequence_No
        End Get
        Set(value As Integer)
            sequence_No = value
        End Set
    End Property
    Private _location As String
    Public Property Location() As String
        Get
            Return _location
        End Get
        Set(value As String)
            _location = value
        End Set
    End Property
End Class

<CLSCompliant(False)> Public Class LoadsWithGroup
    Public Sub New()
    End Sub
    Private ld As String
    Public Property Load() As String
        Get
            Return ld
        End Get
        Set(value As String)
            ld = value
        End Set
    End Property
    Private _skugroup As String
    Public Property SKUGROUP() As String
        Get
            Return _skugroup
        End Get
        Set(value As String)
            _skugroup = value
        End Set
    End Property
    Private _classname As String
    Public Property CLASSNAME() As String
        Get
            Return _classname
        End Get
        Set(value As String)
            _classname = value
        End Set
    End Property
    Private _hazclass As String
    Public Property HAZCLASS() As String
        Get
            Return _hazclass
        End Get
        Set(value As String)
            _hazclass = value
        End Set
    End Property
    Private sequence_No As Integer
    Public Property SequenceNo() As Integer
        Get
            Return sequence_No
        End Get
        Set(value As Integer)
            sequence_No = value
        End Set
    End Property
    Private _groupNumber As Integer
    Public Property GroupNumber() As Integer
        Get
            Return _groupNumber
        End Get
        Set(value As Integer)
            _groupNumber = value
        End Set
    End Property
End Class

<CLSCompliant(False)> Public Class MultiPayloadPutawayHelper

    Public Sub New()
    End Sub
    ' Start RWMS-1200
    Shared Sub New()
        MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads = -1
        MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads = -1
        MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads = -1
    End Sub
    ' End RWMS-1200
#Region "Multi PayLoad Putaway"
    'Start RWMS-1200
    Private Shared _preCalculatedVolumeOfAllLoads As Double
    Public Shared Property PreCalculatedVolumeOfAllLoads() As Double
        Set(ByVal value As Double)
            _preCalculatedVolumeOfAllLoads = value
        End Set
        Get
            Return _preCalculatedVolumeOfAllLoads
        End Get
    End Property

    Private Shared _preCalculatedWeightOfAllLoads As Double
    Public Shared Property PreCalculatedWeightOfAllLoads() As Double
        Set(ByVal value As Double)
            _preCalculatedWeightOfAllLoads = value
        End Set
        Get
            Return _preCalculatedWeightOfAllLoads
        End Get
    End Property

    Private Shared _preCalculatedHeightOfAllLoads As Double
    Public Shared Property PreCalculatedHeightOfAllLoads() As Double
        Set(ByVal value As Double)
            _preCalculatedHeightOfAllLoads = value
        End Set
        Get
            Return _preCalculatedHeightOfAllLoads
        End Get
    End Property
    'End RWMS-1200

    Public Shared Function GroupLoads(ByVal loads As List(Of LoadsWithSequence), Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim strLoggerBuild As New StringBuilder
        Dim groupedLoads As New List(Of LoadsWithGroup)()
        Dim listofListLoad As New List(Of List(Of LoadsWithGroup))()
        Dim ldseq As New LoadsWithSequence
        Dim strloads As String
        Dim numOfGroup As Integer
        strLoggerBuild.Append("Start Grouping the loads with Sequence...")

        strloads = "'" & String.Join("','", loads.[Select](Function(x) x.Load.LOADID)) & "'"

        strLoggerBuild.Append("List of Loads to Group :" & strloads.ToString())

        Try
            'sql = String.Format("SELECT LD.LOADID,SK.SKUGROUP,SK.CLASSNAME,SK.HAZCLASS,GROUPNUMBER = DENSE_RANK() OVER (ORDER BY SK.SKUGROUP,SK.CLASSNAME,SK.HAZCLASS) FROM LOADS LD INNER JOIN SKU SK ON LD.SKU=SK.SKU WHERE LD.LOADID IN (" & strloads & ")")
            'If Not oLogger Is Nothing Then
            '    oLogger.Write("Query to Group of Loads :" & sql)
            'End If
            'DataInterface.FillDataset(sql, dt)
            Dim multipayloadsdao = New MultipayloadsDao
            numOfGroup = multipayloadsdao.GetGroupOfloads(strloads)
            If numOfGroup > 1 Then
                Return False
            Else
                Return True
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write(strLoggerBuild.ToString())
            End If
            'dt.Columns.Add("SEQUENCE", GetType(Integer))
            'For Each dr As DataRow In dt.Rows
            '    dr("SEQUENCE") = (From seq In loads Where seq.Load.LOADID = dr("LOADID").ToString() Select seq.SequenceNo).FirstOrDefault()
            'Next
            'Dim view As New DataView(dt)
            'view.Sort = "SEQUENCE"
            'dt1 = view.ToTable(True, "GROUPNUMBER")
            'Dim innerGroupedLoad As List(Of LoadsWithGroup)
            'For Each dr As DataRow In dt1.Rows
            '    innerGroupedLoad = New List(Of LoadsWithGroup)()
            '    For Each dr1 As DataRow In dt.Rows
            '        If dr("GROUPNUMBER") = dr1("GROUPNUMBER") Then
            '            innerGroupedLoad.Add(New LoadsWithGroup() With {.Load = dr1("LOADID").ToString(), .SKUGROUP = dr1("SKUGROUP").ToString(), .CLASSNAME = dr1("CLASSNAME").ToString(), .HAZCLASS = dr1("HAZCLASS").ToString(), .SequenceNo = dr1("SEQUENCE").ToString(), .GroupNumber = dr1("GROUPNUMBER").ToString()})
            '        End If
            '    Next
            '    innerGroupedLoad = (From lstLoads In innerGroupedLoad Order By lstLoads.SequenceNo Select lstLoads).ToList()
            '    listofListLoad.Add(innerGroupedLoad)
            'Next
            'Dim _seq, i, j As Integer
            'For Each lst As List(Of LoadsWithGroup) In listofListLoad.ToList()
            '    i = 0
            '    For Each lds As LoadsWithGroup In lst.ToList()
            '        If (i > 0) Then
            '            If Not lds.SequenceNo - 1 = lst(i - 1).SequenceNo Then

            '                Dim tempGroupedLoads As New List(Of LoadsWithGroup)()
            '                i = i - 1
            '                lst.Remove(lds)
            '                tempGroupedLoads.Add(lds)
            '                listofListLoad.Add(tempGroupedLoads)
            '            End If
            '        End If
            '        i = i + 1
            '    Next
            'Next
            'For Each lstlog As List(Of LoadsWithGroup) In listofListLoad
            '    j = 0
            '    For Each ldslog As LoadsWithGroup In lstlog.ToList
            '        If Not oLogger Is Nothing Then
            '            oLogger.Write("Group with Consecutive Sequence : " & j.ToString() & "Group Number :" & ldslog.GroupNumber & "Sequence Number :" & ldslog.SequenceNo)
            '        End If
            '    Next
            '    j = j + 1
            'Next
            'Return listofListLoad
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function CanPlaceLoads(ByVal dr As DataRow, ByVal ldLst As List(Of LoadsWithSequence), ByVal sLoadHandlingUnitTypeList As List(Of String), ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByPalletType As Boolean, ByVal pContentType As String, ByVal pDtContent As DataTable, ByVal dtHUTemplates As DataTable, ByVal dtHULocContent As DataTable, ByVal assignedLoads As List(Of LoadsWithSequence), ByVal contHeight As Double, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim looseid As Boolean = False
        Dim LocHUTemplate, SQL, SQLLocationHeight, LoadsinLocation, LoadsinDestinationLocation, strloads As String
        Dim volume, weight, NumLoads, pendingloads, pendingWeight, pendingVol, height, pendingHeight As Double
        Dim locationCubicLimit As Double ' RWMS-1200
        Dim locationWeightLimit, locationheightLimit As Double ' RWMS-1200
        Dim NumberOfLoads As Integer
        Dim ldsVolume, ldsWeight As Double
        'Dim strloads As New StringBuilder()
        Dim dtLoadinLocation, dtLoadIdDestination As DataRow() ' RWMS-1200
        Dim ld, ldDestination As WMS.Logic.Load
        Dim ldHeight, loadsHeight, locationsLodHeight, locationsPendingHeight As Double
        Dim strLoggerBuild As New StringBuilder
        Dim multipayloadsdao = New MultipayloadsDao

        Dim loadCappacity As Int32 = dr("MULTILOADCAPACITY")
        Dim Loc As String = dr("LOCATION")
        Dim Warehousearea As String = dr("WAREHOUSEAREA")
        NumLoads = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NumLoads"), 0)
        pendingloads = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NEWPENDINGLOADS"), 0)
        pendingVol = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NEWPENDINGVOLUME"), 0)
        pendingWeight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NEWPENDINGWEIGHT"), 0)
        looseid = dr("looseid")
        locationCubicLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CUBIC"), 0)
        locationWeightLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WEIGHT"), 0)
        locationheightLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Height"), 0)
        LocHUTemplate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("hustoragetemplate"))

        'Check for Handling Unit Type Allowed combinations
        If LocHUTemplate <> "" AndAlso sLoadHandlingUnitTypeList.Count > 0 Then
            ' Loop through all the handling unit type from the list and validate each one. All should pass.
            For Each hutype As String In sLoadHandlingUnitTypeList
                If Not Location.ValidateLocationHU(LocHUTemplate, Loc, Warehousearea, hutype, dtHUTemplates, dtHULocContent) Then
                    strLoggerBuild.Append("Failed to match Location Handling Unit Type template.... Cannot proceed.")
                    Return False
                End If
            Next
        End If

        'check if we can put some more loads in the location
        If loadCappacity < ldLst.Count + System.Convert.ToInt32(pendingloads) Then 'RWMS-1200
            If Not looseid Then
                'then we can merge for now with current loads; in the future - check for attributes
                strLoggerBuild.Append("Load Capacity exceeded: Load Capacity = " & loadCappacity & ", Number of Loads = " & ldLst.Count & ", Number of Pending Loads = " & pendingloads) '' RWMS-1200
                Return False
            Else
                'check if attribute will allow to merge the loads when moving to the location
                'if on load from the same sku will allow to merge - first load from same sku and check
                Dim mergeFound As Boolean = False
                For Each lds As LoadsWithSequence In ldLst
                    SQL = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & lds.Load.SKU & "' and consignee = '" & lds.Load.CONSIGNEE & "')"
                    For Each currLoad As DataRow In pDtContent.Select(SQL)
                        Dim tmpLoad As New WMS.Logic.Load(currLoad("loadid").ToString())
                        tmpLoad.ACTIVITYSTATUS = ""
                        If WMS.Logic.Merge.CanMerge(tmpLoad, lds.Load) Then
                            mergeFound = True
                        Else
                            mergeFound = False
                        End If
                    Next
                    If mergeFound = False Then
                        Exit For
                    End If

                Next

                If Not mergeFound Then
                    strLoggerBuild.Append("Load Capacity exceeded (no possible merges found): Load Capacity = " & loadCappacity & ", Number of Loads = " & NumLoads & ", Number of Pending Loads = " & pendingloads)
                    Return False
                End If
                'and then check loc content according to policy.....

                For Each lds1 As LoadsWithSequence In ldLst
                    If Not Location.CheckLocContent(dr, lds1.Load, pContentType, pDtContent) Then
                        Return False
                    End If
                Next
            End If
        End If

        'Start RWMS-1200

        '' Only selective calculation
        Dim flags As New List(Of Boolean)()
        flags.Add(pFitByHeight)
        flags.Add(pFitByVolume)
        flags.Add(pFitByWeight)

        ' Tests if nay flag is on
        If flags.Any(Function(x) x) Then
            ''Number of Loads and Loads volume,weight,height 
            If _preCalculatedHeightOfAllLoads < 0 Or _preCalculatedVolumeOfAllLoads < 0 Or _preCalculatedWeightOfAllLoads < 0 Then
                For Each ldseq As LoadsWithSequence In ldLst
                    ldsVolume = ldseq.Load.Volume + ldsVolume
                    ldsWeight = ldseq.Load.GrossWeight + ldsWeight
                    ldHeight = If(pFitByWeight, multipayloadsdao.GetLoadsHeight(ldseq.Load.LOADID), 0)
                    loadsHeight = loadsHeight + ldHeight + contHeight
                Next
            Else
                ldsVolume = _preCalculatedVolumeOfAllLoads
                ldsWeight = _preCalculatedWeightOfAllLoads
                ldHeight = _preCalculatedHeightOfAllLoads
            End If


            strloads = "'" & String.Join("','", ldLst.[Select](Function(x) x.Load.LOADID)) & "'"
            strLoggerBuild.Append("List of Loads :" & strloads.ToString() & ", Loads Height : " & loadsHeight & ", Volume :" & ldsVolume & ", Weight : " & ldsWeight)

            'Getting Existing loads in Location.
            'dtLoadinLocation = multipayloadsdao.GetLoadsInLocation(Loc)

            SQL = "(Location = '" & Loc & "' )"
            dtLoadinLocation = pDtContent.Select(SQL)
            If Not dtLoadinLocation.Length = 0 Then
                For Each drLoad As DataRow In dtLoadinLocation
                    ld = New Load(drLoad("LOADID"), True)
                    volume = ld.Volume + volume
                    weight = ld.GrossWeight + weight
                    locationsLodHeight = If(pFitByWeight, multipayloadsdao.GetLoadsHeight(ld.LOADID), 0)
                    height = locationsLodHeight + height + contHeight
                Next
                strLoggerBuild.Append("Location existing loads Volume :" & volume & ", Weight :" & weight & ", Height :" & height)
            End If

            'Getting Pending loads height for the location
            'dtLoadIdDestination = multipayloadsdao.GetLoadsInDestinationLocation(Loc)

            SQL = "(destinationlocation = '" & Loc & "')"
            dtLoadIdDestination = pDtContent.Select(SQL)

            If Not dtLoadIdDestination.Length = 0 Then
                For Each drLoadID As DataRow In dtLoadIdDestination
                    ldDestination = New Load(drLoadID("LOADID"), True)
                    locationsPendingHeight = If(pFitByWeight, multipayloadsdao.GetLoadsHeight(ldDestination.LOADID), 0)
                    pendingHeight = locationsPendingHeight + pendingHeight + contHeight
                Next
                strLoggerBuild.Append("Location's Pending Height :" & pendingHeight)
            End If

            ' Add assigned loads in Single putaway
            Dim loadsInThisLocation As List(Of LoadsWithSequence) = assignedLoads.Where(Function(s As LoadsWithSequence) s.Location = Loc).ToList()

            loadCappacity = loadCappacity - loadsInThisLocation.Count - pendingloads

            'Getting assigned loads height,weight and volume in Single putaway
            For Each ldsInLocation As LoadsWithSequence In loadsInThisLocation
                volume = ldsInLocation.Load.Volume + volume
                weight = ldsInLocation.Load.GrossWeight + weight
                locationsLodHeight = If(pFitByWeight, multipayloadsdao.GetLoadsHeight(ldsInLocation.Load.LOADID), 0)
                height = locationsLodHeight + height + contHeight
            Next
        End If

        '' End RWMS-1200

        'check for the cubic of the location
        If pFitByVolume Then
            If locationCubicLimit < (volume + pendingVol + ldsVolume) Then
                strLoggerBuild.Append("Load Volume exceeded: Cube limit = " & locationCubicLimit & ", Total Volume = " & volume & ", Pending Volume = " & pendingVol & ", Current Load Volume = " & ldsVolume & ",Total Loads cube for the Location in putaway =" & (volume + pendingVol + ldsVolume)) ' RWMS-1200
                Return False
            End If
        End If
        'check for the weight of the location
        If pFitByWeight Then
            If locationWeightLimit < (weight + pendingWeight + ldsWeight) Then
                strLoggerBuild.Append("Load Volume exceeded: Weight limit = " & locationWeightLimit & ", Total Weight = " & weight & ", Pending Weight = " & pendingWeight & ", Current Load Weight = " & ldsWeight & ",Total Loads weight for the Location in putaway =" & (weight + pendingWeight + ldsWeight))
                Return False
            End If
        End If
        'Check For the load height
        If pFitByHeight Then
            If locationheightLimit < (height + pendingHeight + loadsHeight) Then
                strLoggerBuild.Append("Load Height exceeded: Height limit = " & locationheightLimit & ", Current Loads Height = " & loadsHeight & ", Pending Height = " & pendingHeight & ",Total Loads height for the Location in putaway =" & (height + pendingHeight + loadsHeight))
                Return False
            End If
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write(strLoggerBuild.ToString())
        End If
        'we passed all checks
        Return True
    End Function


    Public Shared Function IsMultiPayLoadPutAwayTask(ByVal task As String) As Boolean
        Dim Sql As String
        Dim dt As New DataTable

        Sql = String.Format("select * from PutAwayDetail Where TaskID = {0}", Made4Net.Shared.Util.FormatField(task))

        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function MultiPayloadPutAwayLoads(ByVal task As String) As DataTable
        Dim Sql As String
        Dim dt As New DataTable
        Sql = String.Format("SELECT FROMLOAD FROM PUTAWAYDETAIL where TASKID={0}", Made4Net.Shared.Util.FormatField(task))
        DataInterface.FillDataset(Sql, dt)
        Return dt
    End Function
#End Region
End Class

<CLSCompliant(False)> Public Class Multiputaway

    Public Sub New()
    End Sub
#Region "Multiple PayLoad Putaway"

    ''' <summary>
    ''' When Simulation flag is set then we will only find location and not assign load to any activity
    ''' </summary>
    Public Sub RequestDestinationForMultiLoad(ByVal pLoadIds() As String, ByRef destLocation As String, ByRef destWarehousearea As String, ByVal Simulation As Integer, ByRef prePopulateLocation As String, Optional ByVal CreateTask As Boolean = True, Optional ByVal onContainer As Boolean = False) 'RWMS-1277

        For Each loadid As String In pLoadIds
            If Not Load.Exists(loadid) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot assign Location to load", "Cannot assign Location to load - Load does not exist")
                m4nEx.Params.Add("Loadid", loadid)
                Throw m4nEx
            End If

        Next

        RequestDestinationForMultiLoad(pLoadIds, destLocation, destWarehousearea, prePopulateLocation, CreateTask, onContainer) 'RWMS-1277
        'End If

    End Sub

    Private Sub RequestDestinationForMultiLoad(ByVal pLoadIds As String(), ByRef destLocation As String, ByRef destWarehousearea As String, ByRef prePopulateLocation As String, Optional ByVal CreateTask As Boolean = True, Optional ByVal onContainer As Boolean = False) 'RWMS-1277
        Dim Loc As New List(Of String)
        Dim tm As New TaskManager
        Dim taskId As String = Nothing
        Dim s As String
        Dim IsSingleLocation As Boolean = False
        Dim position As Integer = 0
        For Each loadid As String In pLoadIds

            Dim oLoad As New Load(loadid)
            If oLoad.UNITSALLOCATED > 0 Then
                If Not String.Equals(oLoad.ACTIVITYSTATUS, WMS.Lib.Statuses.ActivityStatus.REPLPENDING, StringComparison.CurrentCultureIgnoreCase) Then

                    Throw New Made4Net.Shared.M4NException(New Exception(), "Can not putaway load - units allocated", "Can not putaway load - units allocated")
                End If
            End If
        Next
        RequestDestination(pLoadIds, destLocation, destWarehousearea, Common.GetCurrentUser, prePopulateLocation) 'RWMS-1277
        Dim destLoads() As String = destLocation.Split(",")

        For Each loadid As String In destLoads
            Dim destLoads1() As String = loadid.Split("=")
            Dim destlocs As String = destLoads1(1)

            Loc.Add(destlocs)
            Dim oLoad As Load = New Load(destLoads1(0))
            oLoad.SetDestinationLocation(destlocs, destWarehousearea, Common.GetCurrentUser)
        Next

        If Loc.Distinct().Count() > 1 Then
            IsSingleLocation = False
        Else
            IsSingleLocation = True
        End If


        If IsSingleLocation Then
            ''MultiPayload
            Dim destLoads1() As String = destLoads(0).Split("=")
            Dim oLoad As Load = New Load(destLoads1(0))
            tm.CreatePutAwayTask(oLoad, Common.GetCurrentUser, True, 200, "", "", taskId, prePopulateLocation) 'RWMS-1277
            Dim seq As Integer = 1
            For Each loadid As String In destLoads
                Dim ld() As String = loadid.Split("=")
                UpdatePutwayDetail(taskId, seq, ld(0), Common.GetCurrentUser)
                seq = seq + 1
            Next
        Else
            ''Single Payload
            Dim oLoad As Load = New Load(destLoads(0).Split("=")(0))
            Dim lastLocation As String = oLoad.LOCATION
            For Each loadid As String In destLoads
                Dim load() As String = loadid.Split("=")
                oLoad = New Load(load(0))
                oLoad.LOCATION = lastLocation
                tm.CreatePutAwayTask(oLoad, Common.GetCurrentUser, True, 200, "", "", Nothing, prePopulateLocation)
                lastLocation = load(1)
            Next
        End If

    End Sub
    Private Sub UpdatePutwayDetail(ByVal taskId As String, ByVal sequenceNumber As Integer, ByVal fromLoad As String, ByVal user As String)
        Dim Sql As String
        Dim _adddate As DateTime
        _adddate = DateTime.Now
        Sql = String.Format("Insert into PUTAWAYDETAIL(TASKID,SEQUENCENUMBER,FROMLOAD,ADDDATE,ADDUSER) Values({0},{1},{2},{3},{4})", Made4Net.Shared.Util.FormatField(taskId), Made4Net.Shared.Util.FormatField(sequenceNumber), Made4Net.Shared.Util.FormatField(fromLoad), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(user))

        DataInterface.RunSQL(Sql)
    End Sub

    Public Sub RequestDestination(ByVal pLoadIds() As String, _
            ByRef destLocation As String, ByRef destWarehousearea As String, ByVal pUser As String, ByRef prePopulateLocation As String) 'RWMS-1277
        For Each loadid As String In pLoadIds
            Dim oLoad As Load = New Load(loadid)
            oLoad.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, pUser)
        Next
        SendToLocassign(destLocation, destWarehousearea, pLoadIds, pUser, prePopulateLocation) 'RWMS-1277
    End Sub

    Protected Sub SendToLocassign(ByRef destLocation As String, ByRef destWarehousearea As String, _
        ByVal pLoadIds() As String, ByVal pUser As String, ByRef prePopulateLocation As String) 'RWMS-1277


        RequestDest(destLocation, destWarehousearea, pLoadIds, prePopulateLocation) 'RWMS-1277

        'For Each loadid As String In pLoadIds
        '    ld.SetDestinationLocation(destLocation, destWarehousearea, pUser)
        'Next

    End Sub

    'this function gets load id and return a destination for load
    Protected Sub RequestDest(ByRef destLocation As String, ByRef destWarehousearea As String, _
        ByVal pLoadId() As String, ByRef prePopulateLocation As String) 'RWMS-1277
        Dim ld As Load
        Try

            'If destLocation = String.Empty Then
            'else - we need to check the putaway policies -> send the load to loc assign service
            SendLoadToLocAssign(destLocation, destWarehousearea, pLoadId, prePopulateLocation) 'RWMS-1277
            'End If

            'else we didnt find any location, return load's current location
        Catch ex As Exception
        End Try
    End Sub

    Public Sub SendLoadToLocAssign(ByRef destLocations As String, ByRef destWarehousearea As String, _
        ByVal pLoadId() As String, ByRef prePopulateLocation As String) 'RWMS-1277
        Dim LoadList As String = String.Join(",", pLoadId)

        ' Added for RWMS-1440 to Fix for RWMS-1427 : Send single message per load id, insetad of sending all the loads as CSV to Audit.   
        For Each loadid As String In pLoadId
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SendLocToLocAssign)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Services.REQUESTLOCATIONFORMULTILOAD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", "")
            aq.Add("DOCUMENTLINE", "")
            aq.Add("FROMLOAD", loadid)
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", "")
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", loadid)
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", "")
            aq.Add("USERID", Common.GetCurrentUser())
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", Common.GetCurrentUser())
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", Common.GetCurrentUser())
            aq.Send(WMS.Lib.Actions.Services.REQUESTLOCATIONFORMULTILOAD)
        Next
        ' Ended for RWMS-1440 to Fix for RWMS-1427 : Send single message per load id, insetad of sending all the loads as CSV to Audit.

        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        oQ.Add("LOADID", LoadList)
        oQ.Add("ACTION", WMS.Lib.Actions.Services.REQUESTLOCATIONFORMULTILOAD)
        oQ.Add("USERID", Common.GetCurrentUser())
        oQ.Add("WAREHOUSE", Warehouse.CurrentWarehouse())
        oMsg = oQ.Send("LocAssign")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        If Not qm.Values("ERROR") Is Nothing Then
            Throw New ApplicationException(qm.Values("ERROR"))
        End If

        destLocations = qm.Values("LOCATIONS")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
        prePopulateLocation = qm.Values("PREPOPULATELOCATION") 'RWMS-1277
    End Sub

    Private Sub setDestination(ByVal pLocationName As String, ByVal pWarehouseareaName As String, ByVal ld As Load)
        Dim PendingWeight, PendingCube As Double
        Dim sql As String
        Try
            PendingWeight = ld.CalculateWeight()
        Catch ex As Exception
            PendingWeight = 0
        End Try

        Try
            PendingCube = ld.Volume
        Catch ex As Exception
            PendingCube = 0
        End Try
        sql = String.Format("Update Loads set DestinationLocation = '{0}',DestinationWarehousearea = '{4}', EditDate = '{1}',EditUser = '{2}' Where LoadId = '{3}'", pLocationName, Made4Net.Shared.Util.DateTimeToDbString(DateTime.Now), Common.GetCurrentUser(), ld.LOADID, pWarehouseareaName)
        DataInterface.RunSQL(sql)
    End Sub

    'Public Sub Put(ByVal pLoadId As String, ByVal pSubLocation As String, ByVal pLocation As String, _
    '                ByVal pSubWarehousearea As String, ByVal pWarehousearea As String, ByVal pUser As String, Optional ByVal pIsHandOff As Boolean = False)
    Public Sub Put(ByVal pLoadIds As String, ByVal pSubLocation As String, ByVal pLocation As String, _
                    ByVal pWarehousearea As String, ByVal pUser As String, Optional ByVal pIsHandOff As Boolean = False)
        Dim oLoad As New Load(pLoadIds)
        Dim FromLocation As String = oLoad.LOCATION
        Dim FromWarehousearea As String = oLoad.WAREHOUSEAREA
        ' oLoad.Put(pLocation, pWarehousearea, pSubLocation, pUser)
        oLoad.Put(pLocation, pWarehousearea, pSubLocation, pUser, pIsHandOff)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadPutaway)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOADPUTAWAY)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", oLoad.CONSIGNEE)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", pLoadIds)

        aq.Add("FROMLOC", FromLocation)
        aq.Add("FROMWAREHOUSEAREA ", FromWarehousearea)

        aq.Add("FROMQTY", oLoad.UNITS)
        aq.Add("FROMSTATUS", oLoad.STATUS)
        aq.Add("NOTES", "PROC")
        aq.Add("SKU", oLoad.SKU)
        aq.Add("TOLOAD", pLoadIds)

        aq.Add("TOLOC", pLocation)
        aq.Add("TOWAREHOUSEAREA", pWarehousearea)

        aq.Add("TOQTY", oLoad.UNITS)
        aq.Add("TOSTATUS", oLoad.STATUS)
        aq.Add("USERID", Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser())
        aq.Send(WMS.Lib.Actions.Audit.LOADPUTAWAY)
    End Sub

#End Region

End Class






