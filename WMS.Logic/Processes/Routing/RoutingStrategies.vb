Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.GeoData
Imports Made4Net.Algorithms
Imports Made4Net.Algorithms.GeneticAlgorithm
Imports Made4Net.Algorithms.Interfaces
Imports System.Threading
Imports System.Collections.Generic
Imports Made4Net.Shared.Conversion.Convert
Imports System.Text

#Region "PlanRoutesObject"
Public Class PlanRoutesObject
    Public oGeneticSolver As Object
    Public oGAParams As Object
    Public bestChromosome As Object
    Public keygenesArray As ArrayList
    Public runnum As Integer
    Public clusterNumber As Integer
    Public clusterCount As Integer

    Public territoryKey As String
    Public signal As ManualResetEvent

    Public Sub New(ByVal pGeneticSolver As Object, ByVal pGAParams As Object, ByVal pKeygenesArray As ArrayList, _
            ByVal prunnum As Integer, ByVal pclusterNumber As Integer, _
            ByVal pclusterCount As Integer, ByVal pterritoryKey As String)
        oGeneticSolver = DirectCast(pGeneticSolver, GeneticSolver)
        oGAParams = DirectCast(pGAParams, GAParams)
        keygenesArray = pKeygenesArray
        runnum = prunnum
        clusterNumber = pclusterNumber
        clusterCount = pclusterCount
        territoryKey = pterritoryKey
    End Sub
End Class
#End Region


#Region "Resequence Routing"
<CLSCompliant(False)> Public Class ResequenceRouting
    Implements IGeneticDataModel

    Protected _pointsarray As ArrayList
    Protected _resequencepointsarray As ArrayList = New ArrayList()

    <ThreadStatic()> _
    Protected iCivCount As Integer
    Protected _connectionname As String

    <ThreadStatic()> _
    Protected bestChromosomeHash As Hashtable = New Hashtable()

    <ThreadStatic()> _
    Protected oGeneticSolver As GeneticSolver

    Protected pGAParams As GAParams

    <ThreadStatic()> _
    Protected _tempDist As Double = 0D
    <ThreadStatic()> _
    Protected _tmpDrivingTime As Double = 0D
    Protected PointsHashtableCache As Hashtable

    <ThreadStatic()> _
        Public DistCost As Double
    <ThreadStatic()> _
        Public TimeCost As Double

    <ThreadStatic()> _
    Public BestKnownChrom As Chromosome

    Protected Shared _logger As LogHandler
    Protected _isDistanceAdd As Integer = 0
    Protected _distcosttype As Integer = 0
    Protected _isdistcost As Integer = 0

    Protected _civmultithreading As Integer = "2"


    Protected _StartDepot As String = String.Empty
    Protected _ReturnDepot As String = String.Empty

    <ThreadStatic()> _
Private _ChromosomePool As Hashtable = New Hashtable

    Private _currentContextType As Made4Net.DataAccess.LicenseUtils.ConnectionContext
    Private _currentContext As Object


    Public Sub New(ByVal pPointsArray As ArrayList, Optional ByVal oLogger As LogHandler = Nothing)
        _pointsarray = pPointsArray
        _connectionname = DataInterface.ConnectionName
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 1)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)

        _civmultithreading = Made4Net.Shared.AppConfig.Get("CivMultiThreading", "2")

        If PointsHashtableCache Is Nothing Then
            Load()
        End If

        _logger = oLogger
    End Sub

    Public Sub New(ByVal pPointsArray As ArrayList, _
                    ByVal pStatrtDepot As String, _
                    ByVal pReturnDepot As String, _
                    Optional ByVal oLogger As LogHandler = Nothing)

        _pointsarray = pPointsArray
        _connectionname = DataInterface.ConnectionName
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 1)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)

        _civmultithreading = Made4Net.Shared.AppConfig.Get("CivMultiThreading", "2")

        _StartDepot = pStatrtDepot
        _ReturnDepot = pReturnDepot

        If PointsHashtableCache Is Nothing Then
            Load()
        End If


        _logger = oLogger
    End Sub


    Private Sub Load()
        Dim WhereClause As String = ""
        For Each s As String In _pointsarray
            WhereClause &= ",'" & s.ToString() & "'"
        Next

        If _StartDepot <> "" Then
            WhereClause &= ",'" & _StartDepot.ToString() & "'"
        End If
        If _ReturnDepot <> "" Then
            WhereClause &= ",'" & _ReturnDepot.ToString() & "'"
        End If

        If WhereClause = "" Then Return
        WhereClause = " where POINTID in(" & WhereClause.Substring(1, WhereClause.Length - 1) & ")"

        Dim SQL As String = "SELECT POINTID,LATITUDE,LONGITUDE FROM MAPPOINTS " & WhereClause
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)

        PointsHashtableCache = New Hashtable()
        For Each dr As DataRow In dt.Rows

            ''   Dim gp As GeoPointCoord = New GeoPointCoord(dr.Item("POINTID"), dr.Item("LATITUDE"), dr.Item("LONGITUDE"))
            Dim gp As GeoPointNode = New GeoPointNode(dr.Item("POINTID").ToString)

            PointsHashtableCache.Add(dr.Item("POINTID").ToString(), gp)
        Next

    End Sub

    Public Function runResequenceRoutes() As ArrayList
        Try
            If _pointsarray.Count > 0 Then

                Dim NumberOfGenes As Integer = _pointsarray.Count

                Dim ChromArray(NumberOfGenes - 1) As Integer
                For g As Integer = 0 To ChromArray.Length - 1
                    ChromArray(g) = g + 1
                Next

                BestKnownChrom = New Chromosome(ChromArray)
                Dim BestKnownfitness As Double = getChromosomeCost(BestKnownChrom)
                BestKnownChrom.SetFitness(BestKnownfitness)

                bestChromosomeHash = New Hashtable()
                If _pointsarray.Count < 3 Then
                    ''2 points route
                    If _pointsarray.Count = 2 Then
                        If _StartDepot <> String.Empty Or _ReturnDepot <> String.Empty Then
                            ChromArray(0) = 2
                            ChromArray(1) = 1
                            Dim OppositeChromosome As Chromosome = New Chromosome(ChromArray)
                            Dim BNF As Double = getChromosomeCost(OppositeChromosome)
                            If BestKnownfitness > BNF Then
                                BestKnownChrom = OppositeChromosome
                                BestKnownfitness = BNF
                            End If
                            decodeChromosome(BestKnownChrom)
                            If Not _logger Is Nothing Then
                                _logger.Write(" Cost after Optimization: " & BestKnownfitness.ToString())
                            End If
                            Return _resequencepointsarray

                        End If
                    Else
                        Return _pointsarray
                    End If

                End If

                pGAParams = New GAParams(NumberOfGenes)
                pGAParams.TAG = 1
                Dim CivilizationsCount As Integer = pGAParams.CIVILIZATIONS

                If Not _logger Is Nothing Then
                    _logger.writeSeperator("*", 100)
                    _logger.Write("Optimization Number of genes (Stops) : " & NumberOfGenes.ToString())
                    _logger.Write("Optimization  Population: " & pGAParams.POPULATIONSIZE)
                    _logger.Write("Optimization  Civilizations: " & pGAParams.CIVILIZATIONS)
                    _logger.Write("Optimization  Generations: " & pGAParams.GENERATIONS)
                    _logger.Write("Optimization  Genetinc Solver Started Running...")
                End If


                Dim currChrom As Chromosome = BestKnownChrom
                Dim currfitness As Double = BestKnownfitness

                Dim dirpath2 As String = "c:\\tmp\\" ''"
                Dim LoggingEnabled2 As String = Made4Net.Shared.AppConfig.Get("UseShortLogs", "")
                If LoggingEnabled2 = "1" Then
                    dirpath2 = Made4Net.Shared.AppConfig.Get("ServiceShortLogDirectory", "")
                Else
                    dirpath2 = ""
                End If

                Dim eventsCivilizations As List(Of ManualResetEvent) = New List(Of ManualResetEvent)


                iCivCount = 0
                For i As Integer = 0 To CivilizationsCount - 1
                    'If Not _logger Is Nothing Then
                    '    _logger.writeSeperator("*", 100)
                    '    _logger.Write("Optimization Civilization:" & i.ToString & " of " & CivilizationsCount)
                    '    _logger.writeSeperator("*", 100)
                    'End If

                    pGAParams.TAG1 = i
                    oGeneticSolver = New GeneticSolver(Me, pGAParams, Nothing, dirpath2)
                    Dim PRO As New PlanRoutesObject(oGeneticSolver, pGAParams, Nothing, 1, 0, 0, "")

                    PRO.signal = New ManualResetEvent(False)
                    eventsCivilizations.Add(PRO.signal)

                    '''return
                    Me._currentContextType = GetCurrentContext()
                    If Me._currentContextType = LicenseUtils.ConnectionContext.HttpSession Then
                        Me._currentContext = System.Web.HttpContext.Current
                    End If


                    ''multithreading
                    Select Case _civmultithreading
                        Case "1"
                            Dim job As ParameterizedThreadStart = New ParameterizedThreadStart(AddressOf runResequencRoutes)
                            Dim thread As Thread = New Thread(job)
                            thread.Start(PRO)
                        Case "2" ''default
                            runResequencRoutes(PRO)
                        Case Else ''0
                            ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf runResequencRoutes), PRO)
                            Thread.Sleep(0)
                    End Select

                Next
                WaitHandle.WaitAll(eventsCivilizations.ToArray())


                BestKnownfitness = Double.MaxValue
                Dim oEn As IDictionaryEnumerator = bestChromosomeHash.GetEnumerator()
                oEn.Reset()
                Dim k As Integer = 0
                While (oEn.MoveNext())
                    currChrom = DirectCast(oEn.Value, Chromosome)
                    currfitness = currChrom.GetFitness()
                    If BestKnownfitness > currfitness And currfitness > 1 Then
                        BestKnownfitness = currfitness
                        BestKnownChrom = currChrom
                    End If
                    k += 1
                End While


                decodeChromosome(BestKnownChrom)
                Dim Cost As Double = getChromosomeCost(BestKnownChrom)

                If Not _logger Is Nothing Then
                    _logger.Write(" Cost after Optimization: " & Cost.ToString())
                End If

                Return _resequencepointsarray
            End If


            If Not _logger Is Nothing Then
                _logger.Write("Genetinc Solver Finished Resequence Its run Successfully for " & _pointsarray.Count & " orders.")
                _logger.writeSeperator("*", 100)
            End If

        Catch ex As Exception
            _logger.writeSeperator("*", 100)
            _logger.Write("Resequence Routes Error: " & ex.Message)
            _logger.writeSeperator("*", 100)
            Return Nothing

        End Try



    End Function
    Public Function GetCurrentContext() As LicenseUtils.ConnectionContext

        Dim connContext As LicenseUtils.ConnectionContext = LicenseUtils.ConnectionContext.HttpSession
        If System.Web.HttpContext.Current Is Nothing Then
            connContext = LicenseUtils.ConnectionContext.NonHttp
        Else
            If System.Web.HttpContext.Current.Session Is Nothing Then
                connContext = LicenseUtils.ConnectionContext.HttpApplication
            End If
        End If

        Return connContext
    End Function
    Public Function getChromosomeCost(ByVal chrom As Chromosome) As Double
        DistCost = 0D
        TimeCost = 0D

        For i As Integer = 0 To chrom.GetSize() - 2
            GetCost(chrom.GetGene(i), chrom.Gene(i + 1), "")
            DistCost += _tempDist
            TimeCost += _tmpDrivingTime
        Next

        addDepotGetCost(chrom)
        DistCost += _tempDist
        TimeCost += _tmpDrivingTime


        Return DistCost



    End Function

    Public Function getRouteCost() As Double

        Dim ChromArray(_pointsarray.Count - 1) As Integer
        For g As Integer = 0 To ChromArray.Length - 1
            ChromArray(g) = g + 1
        Next

        Dim chrom As New Chromosome(ChromArray)
        DistCost = 0D
        TimeCost = 0D

        For i As Integer = 0 To _pointsarray.Count - 2
            GetCost(i + 1, i + 2, "")
            DistCost += _tempDist
            TimeCost += _tmpDrivingTime
        Next
        addDepotGetCost(chrom)
        DistCost += _tempDist
        TimeCost += _tmpDrivingTime

        Return DistCost

    End Function


    Private Function addDepotGetCost(ByVal chrom As Chromosome) As Double
        _tempDist = 0
        _tmpDrivingTime = 0
        Try

            Dim distcost As Double = 0
            Dim timecost As Double = 0

            If _StartDepot <> String.Empty Then

                Dim StartDepotNode As GeoPointNode = PointsHashtableCache(_StartDepot)
                Dim Target As GeoPointNode = PointsHashtableCache(_pointsarray(chrom.GetGene(0) - 1).ToString())

                GeoNetworkItem.GetDistance(StartDepotNode, Target, distcost, timecost, "RP", _isDistanceAdd, _
                _isdistcost, _distcosttype)
                _tempDist += distcost
                _tmpDrivingTime += timecost
            End If

            distcost = 0
            timecost = 0
            If _ReturnDepot <> String.Empty Then



                Dim Source As GeoPointNode = PointsHashtableCache(_pointsarray(chrom.GetGene(_pointsarray.Count - 1) - 1).ToString())
                Dim ReturnDepotNode As GeoPointNode = PointsHashtableCache(_ReturnDepot)


                GeoNetworkItem.GetDistance(Source, ReturnDepotNode, distcost, timecost, "RP", _isDistanceAdd, _
                _isdistcost, _distcosttype)
                _tempDist += distcost
                _tmpDrivingTime += timecost
            End If
            Return _tempDist

        Catch ex As Exception
            Return 0
        End Try

    End Function

    Private Sub runResequencRoutes(ByVal p As Object)
        If Me._currentContextType = LicenseUtils.ConnectionContext.HttpSession And IsNothing(System.Web.HttpContext.Current) Then
            System.Web.HttpContext.Current = Me._currentContext
        End If
        Dim PRO As PlanRoutesObject = DirectCast(p, PlanRoutesObject)
        Try
            Dim oGAParams As GAParams = DirectCast(PRO.oGAParams, GAParams)

            Dim oGeneticSolver As GeneticSolver = DirectCast(PRO.oGeneticSolver, GeneticSolver)
            Dim bestChromosome As New Chromosome(oGAParams.NUMBEROFGENES)

            DataInterface.ConnectionName = _connectionname

            ''        _keygenesArray = PRO.keygenesArray
            bestChromosome = oGeneticSolver.Run()

            SyncLock bestChromosomeHash
                Dim r As New Random(DateTime.Now.Millisecond)
                Dim hashkey As String
                Do
                    Thread.Sleep(1)
                    hashkey = iCivCount.ToString() & "_" & _
                        (((Thread.CurrentThread.GetHashCode().ToString() & "_") & _
                        DateTime.Now.Millisecond.ToString() & "_") & _
                        DateTime.Now.Ticks.ToString() & "_") & _
                        r.[Next](10000).ToString()
                Loop While bestChromosomeHash.ContainsKey(hashkey)

                bestChromosomeHash.Add(hashkey, DirectCast(bestChromosome, Chromosome))
            End SyncLock

        Catch ex As Exception
        Finally
            PRO.signal.Set()
            Interlocked.Increment(iCivCount)

        End Try
    End Sub

    Protected Sub decodeChromosome(ByVal chrom As Chromosome)

        For iGene As Integer = 0 To chrom.GetSize() - 1
            _resequencepointsarray.Add(_pointsarray(chrom.GetGene(iGene) - 1))
        Next

    End Sub
    Public Function EvaluateChromosomeFitness(ByVal chrom As Made4Net.Algorithms.GeneticAlgorithm.Chromosome, _
                                       ByVal keygenesArray As ArrayList) As Double _
                                      Implements Made4Net.Algorithms.Interfaces.IGeneticDataModel.EvaluateChromosomeFitness


        Dim chromfitness As Double = Integer.MaxValue
        Try
            If chrom Is Nothing Then Return chromfitness
            Dim sk As String = chrom.getGenKey()

            If _ChromosomePool.ContainsKey(sk) Then
                chromfitness = _ChromosomePool.Item(sk)
            Else
                chromfitness = getChromosomeCost(chrom)


                ''Integer.MaxValue-cache size
                If (_ChromosomePool.Count = Integer.MaxValue) Then
                    _ChromosomePool = New Hashtable
                End If
                Try
                    SyncLock _ChromosomePool
                        If Not _ChromosomePool.ContainsKey(sk) Then
                            _ChromosomePool.Add(sk, chromfitness)
                        End If
                    End SyncLock
                Catch ex As Exception
                    If Not _logger Is Nothing Then
                        _logger.Write("Evaluate Fitness add to pool Error: " & ex.ToString)
                    End If

                End Try
            End If

            If Not chrom Is Nothing Then chrom.SetFitness(chromfitness)
            Return chromfitness

        Catch ex As Exception
            If Not _logger Is Nothing Then
                _logger.Write("Evaluate Fitness Error: " & ex.ToString)
            End If
            Return chromfitness
        End Try



        'Dim chromfitness As Double = Integer.MaxValue
        'Try
        '    If chrom Is Nothing Then Return chromfitness
        '    chromfitness = getChromosomeCost(chrom)
        '    chrom.SetFitness(chromfitness)
        '    Return chromfitness
        'Catch ex As Exception
        '    If Not _logger Is Nothing Then
        '        _logger.Write("Evaluate Fitness Error: " & ex.ToString)
        '    End If
        '    Return chromfitness
        'End Try
    End Function



    ' fast calculate  distance
    Private Function GetCost0(ByVal FromPoint As Int32, ByVal ToPoint As Int32, ByVal pCostParam As String) As Double ''Implements Made4Net.Algorithms.Interfaces.IGeneticDataModel.GetCost
        Try
            If PointsHashtableCache Is Nothing Then
                Load()
            End If

            Dim Source As GeoPointCoord = PointsHashtableCache(_pointsarray(FromPoint - 1).ToString())
            Dim Target As GeoPointCoord = PointsHashtableCache(_pointsarray(ToPoint - 1).ToString())


            If Source.POINTID = Target.POINTID Then Return 0
            Dim tempDist As Double = Double.MaxValue
            Dim tmpDrivingTime As Double = Double.MaxValue

            tempDist = GeoPoint.CalculateDistance(Source.LATITUDE, Source.LONGITUDE, Target.LATITUDE, Target.LONGITUDE)
            tmpDrivingTime = tempDist / (50000 / 3600)

            If pCostParam = "DRIVINGDIST" Then
                Return tempDist
            ElseIf pCostParam = "DRIVINGTIME" Then
                Return tmpDrivingTime
            ElseIf pCostParam = "" Then
                Return tempDist
            End If

        Catch ex As Exception
            If Not _logger Is Nothing Then
                _logger.writeSeperator("Get Cost Simple  Error:", ex.Message)
            End If
            Return 0

        End Try
    End Function


    Private Function GetCost(ByVal FromPoint As Int32, ByVal ToPoint As Int32, ByVal pCostParam As String) As Double Implements Made4Net.Algorithms.Interfaces.IGeneticDataModel.GetCost
        Try

            If FromPoint = ToPoint Then Return 0
            _tempDist = 0
            _tmpDrivingTime = 0

            'Dim Source As GeoPointNode = New GeoPointNode(_pointsarray(FromPoint - 1).ToString())
            'Dim Target As GeoPointNode = New GeoPointNode(_pointsarray(ToPoint - 1).ToString())



            Dim Source As GeoPointNode = PointsHashtableCache(_pointsarray(FromPoint - 1).ToString())
            Dim Target As GeoPointNode = PointsHashtableCache(_pointsarray(ToPoint - 1).ToString())

            ''GeoNetworkItem.GetDistance(Source, Target, _tempDist, _tmpDrivingTime, "", _isDistanceAdd)

            GeoNetworkItem.GetDistance(Source, Target, _tempDist, _tmpDrivingTime, "RP", _isDistanceAdd, _
            _isdistcost, _distcosttype)
            If pCostParam = "DRIVINGDIST" Then
                Return _tempDist
            ElseIf pCostParam = "DRIVINGTIME" Then
                Return _tmpDrivingTime
            ElseIf pCostParam = "" Then
                Return _tempDist
            End If


        Catch ex As Exception
            _tempDist = 0
            _tmpDrivingTime = 0

            If Not _logger Is Nothing And oGeneticSolver.Generation = pGAParams.GENERATIONS Then
                _logger.writeSeperator("Get Cost Resequence Errror:", ex.Message)
            End If
            Return 0

        End Try
    End Function


End Class

#End Region

#Region "Routing Strategies"

<CLSCompliant(False)> Public Class RoutingStrategies
    Implements ICollection

#Region "Variables"

    Protected _stratcol As ArrayList
    Protected Shared _connectionname As String
    Protected _logger As LogHandler
    Public Shared maxexit As Integer = 3 ''change get from strategy

#End Region

#Region "Constructor"

    Public Sub New(ByVal oLogger As LogHandler)
        _connectionname = DataInterface.ConnectionName
        _stratcol = New ArrayList
        _logger = oLogger
        Load()

    End Sub

#End Region

#Region "Properties"
    <ThreadStatic()> _
    Public iClusterCount As Integer = 0

    Default Public Property Item(ByVal index As Int32) As RoutingStrategy
        Get
            Return CType(_stratcol(index), RoutingStrategy)
        End Get
        Set(ByVal Value As RoutingStrategy)
            _stratcol(index) = Value
        End Set
    End Property

    Default Public ReadOnly Property Item(ByVal StrategyName As String) As RoutingStrategy
        Get
            Dim strat As RoutingStrategy
            For Each strat In Me
                If strat.StrategyId.ToLower = StrategyName.ToLower Then
                    Return CType(strat, RoutingStrategy)
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from routingstrategyheader")
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)
        Dim st As RoutingStrategy
        For Each dr In dt.Rows
            st = New RoutingStrategy(dr)
            If st.StrategyDetails.Count = 0 Then
                If Not _logger Is Nothing Then
                    _logger.Write("***Not defined details for strategy: " & st.StrategyId)
                End If
                Continue For
            End If

            Dim oVehicle As VehicleType = st.StrategyVehicleAllocation.GetNextAvailableVehicle(False)

            If oVehicle Is Nothing And st.StrategyDetails(0).DefaultVehicle Is Nothing Then
                If Not _logger Is Nothing Then
                    _logger.Write("***Not defined vehicles for strategy: " & st.StrategyId)
                End If
            Else
                Add(st)
            End If

        Next
    End Sub


    Public Function PlanRoutes(ByVal pTerritoryKey As String, ByVal pRoutingSet As RoutingSet, ByVal RunId As String, _
                         Optional ByVal oLogger As LogHandler = Nothing)
        Try

            Dim strat As RoutingStrategy
            Dim oRoutesetNodeArrayList As New GeoPointArrayList
            Dim oRoutesetReqArrayList As New ArrayList()
            Dim numrun As Integer

            For Each strat In Me
                Dim tag As Integer = 0

                ''27/02/2011
                oRoutesetNodeArrayList.Clear()
                oRoutesetReqArrayList.Clear()
                For Each tmpReq As RoutingRequirement In strat
                    If tmpReq.TerritoryKey <> pTerritoryKey Then Continue For

                    If tmpReq.TargetGeoPoint Is Nothing Then
                        If Not oLogger Is Nothing Then
                            oLogger.Write("PlanRoutes: Point not belongs to Network: " & tmpReq.PointID)
                        End If
                        Exit For
                    End If
                    tmpReq.TargetGeoPoint.TAG = tag

                    ''23/02/2011
                    If Not oRoutesetNodeArrayList.Contains(tmpReq.TargetGeoPoint) Then
                        oRoutesetNodeArrayList.Add(tmpReq.TargetGeoPoint)
                        tag += 1
                    End If
                    oRoutesetReqArrayList.Add(tmpReq)

                Next
                If oRoutesetNodeArrayList.Count = 0 Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Not found target points for strategy: " & strat.StrategyId.ToString())
                    End If
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.writeSeperator("-", 60)
                        oLogger.Write("Found " & oRoutesetNodeArrayList.Count.ToString() & " target points for strategy: " & strat.StrategyId.ToString())
                    End If

                    Dim numPointsInCluster As Integer = strat.StrategyDetails(0).Clustersize

                    If numPointsInCluster = 0 Then
                        numPointsInCluster = oRoutesetNodeArrayList.Count
                    End If

                    ''
                    Dim oStratDet As RoutingStrategyDetail = DirectCast(strat.StrategyDetails(0), RoutingStrategyDetail)
                    Dim PointClustersHash As New Hashtable()

                    Dim lowlimitclustercount As Integer = 0
                    Dim realclustersCount As Integer = 0

                    numrun = 0
                    Dim numexit As Integer = 0
                    Dim countUnplanned As Integer = oRoutesetNodeArrayList.Count
                    While numexit <= maxexit * 4 ''2
                        If countUnplanned = 0 Or oRoutesetNodeArrayList.Count = 0 Then
                            Exit While
                        End If

                        numrun += 100

                        'If numexit > 4 Then
                        '    numPointsInCluster = 25
                        'End If

                        Dim clusterCount As Integer = Math.Floor(oRoutesetNodeArrayList.Count / numPointsInCluster)
                        Dim clusterCountMod As Integer = oRoutesetNodeArrayList.Count Mod numPointsInCluster
                        If clusterCountMod > 0 And clusterCount <> 0 And _
                            (clusterCountMod > numPointsInCluster * strat.StrategyDetails(0).MinClusterSize Or _
                            oRoutesetNodeArrayList.Count / clusterCount > numPointsInCluster * strat.StrategyDetails(0).MaxClusterSize) Then
                            clusterCount += 1
                        End If
                        If clusterCount = 0 Then clusterCount = 1

                        PointClustersHash.Clear()
                        Dim MergedClusterArray As New ArrayList()




                        Dim clusters As ClusterCollection = DivideIntoClusters(Math.Max(clusterCount, 1), oRoutesetNodeArrayList, oStratDet, PointClustersHash, MergedClusterArray, oLogger)
                        realclustersCount = clusters.Count

                        For Each oReq As RoutingRequirement In strat
                            oReq.TargetGeoPoint.CLUSTERNUMBER = 0
                            oReq.TargetGeoPoint.CLUSTERITEMNUMBER = 0
                        Next


                        Dim currDistance As Double = 0D
                        Dim plannedClusterNumber As Integer = 1

                        Dim minClusterSize As Integer = Convert.ToInt32(Math.Round(oStratDet.Clustersize * oStratDet.MinClusterSize, 0))

                        Dim TryplannedClusterNumber As New ArrayList()

                        Dim findplannedcluster As Boolean = False
                        While Not findplannedcluster

                            If TryplannedClusterNumber.Count = realclustersCount Then
                                Exit While
                            End If
                            numrun += 1

                            ''add ROUTEFARCLUSTERSFIRST  logic
                            If oStratDet.RouteFarClustersFirst = 0 Then
                                Dim minDistance As Double = Integer.MaxValue
                                For cn As Integer = 0 To clusters.Count - 1
                                    If TryplannedClusterNumber.Contains(cn + 1) Then Continue For

                                    If MergedClusterArray.Contains(cn + 1) Then
                                        TryplannedClusterNumber.Add(cn + 1)
                                        Continue For
                                    End If

                                    Dim oCluster As Cluster = clusters(cn)
                                    currDistance = GeoPoint.CalculateDistance(oStratDet.DepoPointNode.LATITUDE, oStratDet.DepoPointNode.LONGITUDE, _
                                        oCluster.ClusterMean(1), oCluster.ClusterMean(2))
                                    If (currDistance < minDistance And oCluster.Count > minClusterSize) _
                                        OrElse (clusters(plannedClusterNumber - 1).Count < minClusterSize) Then
                                        minDistance = currDistance
                                        plannedClusterNumber = cn + 1
                                    End If
                                Next
                            Else
                                Dim maxDistance As Double = 0
                                For cn As Integer = 0 To clusters.Count - 1
                                    If TryplannedClusterNumber.Contains(cn + 1) Then Continue For

                                    If MergedClusterArray.Contains(cn + 1) Then
                                        TryplannedClusterNumber.Add(cn + 1)
                                        Continue For
                                    End If

                                    Dim oCluster As Cluster = clusters(cn)
                                    currDistance = GeoPoint.CalculateDistance(oStratDet.DepoPointNode.LATITUDE, oStratDet.DepoPointNode.LONGITUDE, _
                                        oCluster.ClusterMean(1), oCluster.ClusterMean(2))
                                    If (currDistance > maxDistance And oCluster.Count > minClusterSize) _
                                        OrElse (clusters(plannedClusterNumber - 1).Count < minClusterSize) Then
                                        maxDistance = currDistance
                                        plannedClusterNumber = cn + 1
                                    End If
                                Next
                            End If
                            TryplannedClusterNumber.Add(plannedClusterNumber)

                            ''''
                            Dim clnum As Integer = 0
                            For Each tmpReq As RoutingRequirement In strat
                                If plannedClusterNumber = PointClustersHash(tmpReq.PointID) And tmpReq.planned = 0 Then
                                    tmpReq.TargetGeoPoint.CLUSTERNUMBER = PointClustersHash(tmpReq.PointID)
                                    tmpReq.TargetGeoPoint.CLUSTERITEMNUMBER = clnum
                                    clnum += 1
                                End If
                            Next


                            If Not oLogger Is Nothing Then
                                Dim excludecounter As Integer = 0
                                Dim excludePoints As New ArrayList()
                                For Each oReq As RoutingRequirement In oRoutesetReqArrayList
                                    If oReq.planned = 2 Then
                                        excludecounter += 1
                                        If Not excludePoints.Contains(oReq.PointID) Then
                                            excludePoints.Add(oReq.PointID)
                                        End If
                                    End If
                                Next
                                oLogger.Write("Unplanned strategy Points: " & oRoutesetNodeArrayList.Count.ToString() & _
                                    " with Total Req(s):" & oRoutesetReqArrayList.Count & _
                                    " Excluded req(s):" & excludecounter & _
                                    " Excluded point(s):" & excludePoints.Count & _
                                    " Current cluster: #" & plannedClusterNumber & _
                                    " Real Cluster Count: " & realclustersCount)

                            End If

                            Dim alwaysAddLastRoute As Boolean = (clusters.Count = 1 And clusterCount < 2) _
                                Or (oRoutesetNodeArrayList.Count < oStratDet.Clustersize)

                            Dim prevcount As Integer = oRoutesetReqArrayList.Count

                            strat.PlanRoutes(pTerritoryKey, pRoutingSet, RunId, plannedClusterNumber, _
                            oRoutesetNodeArrayList, oRoutesetReqArrayList, _
                                numrun, alwaysAddLastRoute, numexit, oLogger)

                            oRoutesetNodeArrayList.Clear()
                            oRoutesetNodeArrayList.Clear()
                            tag = 0
                            For Each oReq As RoutingRequirement In oRoutesetReqArrayList
                                If Not oRoutesetNodeArrayList.Contains(oReq.TargetGeoPoint) Then
                                    oRoutesetNodeArrayList.Add(oReq.TargetGeoPoint)
                                    tag += 1
                                End If
                            Next

                            If oRoutesetReqArrayList.Count = prevcount Then
                                If Not oLogger Is Nothing Then
                                    oLogger.Write("Unplanned cluster# " & plannedClusterNumber.ToString)
                                End If

                                If TryplannedClusterNumber.Count = realclustersCount Then
                                    numexit += 1

                                    ''add if numexit>3 try allowsplit ???

                                    If Not oLogger Is Nothing Then
                                        oLogger.Write(" *** numexit: " & numexit & " Strategy: " & strat.StrategyId & " - could not place the " & oRoutesetReqArrayList.Count.ToString() & " last req(s).")
                                    End If
                                End If

                                countUnplanned = 0
                                For Each oReq As RoutingRequirement In oRoutesetReqArrayList
                                    If oReq.planned = 0 Then
                                        countUnplanned += 1
                                    End If
                                Next
                                If countUnplanned = 0 Then
                                    Exit While
                                End If

                            Else
                                findplannedcluster = True
                                countUnplanned = oRoutesetReqArrayList.Count
                                numexit = 0
                                For Each oReq As RoutingRequirement In oRoutesetReqArrayList
                                    If oReq.planned = 2 Then
                                        oReq.planned = 0
                                    End If
                                Next

                            End If
                        End While ''findplannedcluster  And TryplannedClusterNumber.Count <= realclustersCount

                    End While ''numexit < maxexit

                End If
            Next



        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Plan routes Error: " & ex.ToString)
            End If

        End Try
    End Function

    Protected Function DivideIntoClusters(ByVal clusterCount As Integer, _
                ByVal oRoutesetNodeArrayList As GeoPointArrayList, _
                ByVal oStratDet As RoutingStrategyDetail, _
                 ByRef PointClustersHash As Hashtable, _
                 ByRef MergedClusterArray As ArrayList, _
                ByVal oLogger As LogHandler) As ClusterCollection
        If Not oLogger Is Nothing Then
            oLogger.Write("Divide into: " & clusterCount.ToString() & " clusters ")
        End If

        Dim clusters As ClusterCollection
        Dim data As Double(,) = New Double(oRoutesetNodeArrayList.Count - 1, 2) {}
        Dim pi As Integer = 0

        'Dim checkHash As New Hashtable()
        For Each GP As GeoPointNode In oRoutesetNodeArrayList
            data(pi, 0) = Convert.ToDouble(GP.POINTID)
            data(pi, 1) = GP.LATITUDE
            data(pi, 2) = GP.LONGITUDE
            pi += 1
        Next

        clusters = KMeans.ClusterDataSet(clusterCount, data)
        Dim MaxClustersize As Integer = Convert.ToInt32(Math.Round(oStratDet.Clustersize * oStratDet.MaxClusterSize, 0))

        ''recursive check max count and split large clusters
        Dim j As Integer = 0
        While j < clusters.Count
            If clusters(j).Count > MaxClustersize Then
                Dim data2 As Double(,) = New Double(clusters(j).Count - 1, 2) {}
                For k As Integer = 0 To clusters(j).Count - 1
                    data2(k, 0) = Convert.ToInt32(clusters(j)(k)(0)).ToString()
                    data2(k, 1) = clusters(j)(k)(1)
                    data2(k, 2) = clusters(j)(k)(2)
                Next
                Dim clusters2 As ClusterCollection
                clusters2 = KMeans.ClusterDataSet(2, data2)
                If clusters2.Count < 2 Then
                    Exit While
                End If
                If Not oLogger Is Nothing Then
                    oLogger.Write("Split Cluster  #" & (j + 1).ToString & " " & clusters(j).Count & " points into 2 cluster with " & clusters2(0).Count & " and " & clusters2(1).Count & " points.")
                End If

                clusters.RemoveAt(j)
                clusters.Add(clusters2(0))
                clusters.Add(clusters2(1))
            Else
                j += 1
            End If
        End While


        clusterCount = clusters.Count
        If Not oLogger Is Nothing Then
            oLogger.Write("Real Divided into: " & clusterCount.ToString() & " clusters ")
        End If

        For hj As Integer = 0 To clusters.Count - 1
            For k As Integer = 0 To clusters(hj).Count - 1
                Dim key As String = Convert.ToInt32(clusters(hj)(k)(0)).ToString()
                If Not PointClustersHash.ContainsKey(key) Then
                    PointClustersHash.Add(key, hj + 1)
                End If
            Next
        Next


        ''merge small clusters
        Dim MINCLUSTERSIZEFORMERGE As Integer = oStratDet.MinClusterSizeForMerge
        Dim currnum As Integer = 0

        Dim clusterSizeHash As New Hashtable()
        For Each oCluster As Cluster In clusters
            clusterSizeHash.Add(oCluster, oCluster.Count)
        Next

        For Each oCluster As Cluster In clusters
            oCluster.Tag = String.Empty
            If oCluster.Count < MINCLUSTERSIZEFORMERGE Then
                Dim minClusterMean As Double = Integer.MaxValue
                Dim currClusterMean As Double

                ''find nearest cluster
                j = 0
                Dim minCluster As Cluster
                For Each currcluster As Cluster In clusters
                    If currnum = j Or currcluster.Count < MINCLUSTERSIZEFORMERGE Or _
                        (clusterSizeHash(currcluster) + oCluster.Count) > MaxClustersize Then
                        j += 1
                        Continue For
                    End If

                    currClusterMean = KMeans.EuclideanDistance(oCluster.ClusterMean, currcluster.ClusterMean)
                    If currClusterMean < minClusterMean Then
                        minClusterMean = currClusterMean
                        minCluster = currcluster
                        oCluster.Tag = j.ToString
                    End If
                    j += 1

                Next



                ''overwrite points cluster nuumber
                If oCluster.Tag <> String.Empty Then
                    clusterSizeHash(minCluster) = clusterSizeHash(minCluster) + oCluster.Count
                    MergedClusterArray.Add(currnum + 1)
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Cluster  #" & (currnum + 1).ToString & " " & oCluster.Count & " points merged with #" & (Convert.ToInt32(oCluster.Tag) + 1).ToString() & " cluster.")
                    End If

                    For k As Integer = 0 To oCluster.Count - 1
                        Dim key As String = Convert.ToInt32(oCluster(k)(0)).ToString()
                        If Not PointClustersHash.ContainsKey(key) Then
                            PointClustersHash.Add(key, Convert.ToInt32(oCluster.Tag) + 1)
                        Else
                            PointClustersHash(key) = Convert.ToInt32(oCluster.Tag) + 1
                        End If
                    Next
                End If
            End If

            currnum += 1
        Next


        Return clusters
    End Function



#End Region

#Region "Overrides"

    Public Function Add(ByVal value As RoutingStrategy) As Integer
        Return _stratcol.Add(value)
    End Function

    Public Sub Clear()
        _stratcol.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As RoutingStrategy)
        _stratcol.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As RoutingStrategy)
        _stratcol.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _stratcol.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _stratcol.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _stratcol.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _stratcol.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _stratcol.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _stratcol.GetEnumerator()
    End Function

#End Region

End Class

#End Region

Public Class RoutsPlanerParams
    <CLSCompliant(False)> _
    Public strat As RoutingStrategy

    <CLSCompliant(False)> _
    Public pRoutingSet As RoutingSet
    Public RunId As String
    Public TerritoryKey As String
    Public clusterNumber As Integer
    Public oLogger As LogHandler
    Public signal As ManualResetEvent
End Class

#Region "Routing Strategy"

<CLSCompliant(False)> _
Public Class RoutingStrategy
    Inherits ArrayList
    Implements IGeneticDataModel


#Region "Variables"

    Protected _strategyid As String
    Protected _description As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    'Details Objects
    Protected _stratdetail As RoutingStrategyDetailCollection
    Protected _vehicleallocdetail As RoutingVehicleAllocationCollection

    'Routing Objects
    Protected _RoutingSet As RoutingSet
    ''Protected _TargetGeoPointsNodes As GeoPointNodeList
    'Protected _TargetPointsOrders As ArrayList              'Cache of the orders to be routed
    'Protected _TargetCompanies As Hashtable                 'Cache of the Target Companies to be routed
    Protected _NumberOfGenes As Int32
    Protected _PopulationSize As Int32
    Protected _GenerationCount As Int32
    Protected _RunId As String
    Protected _OrdersRoutingParamTable As DataTable

    'Route Cost Factors
    Protected _DefCostPerDay, _DefCostPerDistUnit, _DefCostPerHour As Double

    Protected oGeneticSolver As GeneticSolver
    Protected _logger As LogHandler

    <ThreadStatic()> _
    Protected _keygenesArray As ArrayList = New ArrayList
    Protected Shared _connectionname As String

    Protected _isDistanceAdd As Integer = 0
    Protected _distcosttype As Integer = 0

    Protected _isdistcost As String = "0"


    Protected _civmultithreading As String = "0"
    Protected _isOpenHoursValidation As Integer = 0

    <ThreadStatic()> _
    Private _ChromosomePool As Hashtable = New Hashtable

    ''trips 06/09/2011
    <ThreadStatic()> _
    Public VechicleInTripPool As Hashtable = New Hashtable()
    Public VehicleCounterFromPool As Integer = 0
    Public CurrentVehicleKey As String = String.Empty

    Public Shared DefaultVehicleGroup As String = "9999"
#End Region

#Region "Properties"
    <ThreadStatic()> _
    Public bestChromosomeHashHash As Hashtable = New Hashtable()

    <ThreadStatic()> _
    Public iCivCount As Integer = 0

    <ThreadStatic()> _
    Public iCivCount2 As Integer = 0

    ''Protected PointsHashtableCache As Hashtable
    Public ReadOnly Property DefCostPerDay() As Double
        Get
            Return _DefCostPerDay
        End Get
    End Property

    Public ReadOnly Property DefCostPerDistUnit() As Double
        Get
            Return _DefCostPerDistUnit
        End Get
    End Property

    Public ReadOnly Property DefCostPerHour() As Double
        Get
            Return _DefCostPerHour
        End Get
    End Property

    Public ReadOnly Property StrategyId() As String
        Get
            Return _strategyid
        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return _description
        End Get
    End Property

    Public ReadOnly Property StrategyDetails() As RoutingStrategyDetailCollection
        Get
            Return _stratdetail
        End Get
    End Property

    Public ReadOnly Property StrategyVehicleAllocation() As RoutingVehicleAllocationCollection
        Get
            Return _vehicleallocdetail
        End Get
    End Property

    Public ReadOnly Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
    End Property

    Public ReadOnly Property AddUser() As String
        Get
            Return _adduser
        End Get
    End Property

    Public ReadOnly Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
    End Property

    Public ReadOnly Property EditUser() As String
        Get
            Return _edituser
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal RoutingStrategyId As String)
        _connectionname = DataInterface.ConnectionName
        ''_isoptimization = Made4Net.Shared.AppConfig.Get("isFinalOptimization", "0")
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _civmultithreading = Made4Net.Shared.AppConfig.Get("CivMultiThreading", "0")
        _isOpenHoursValidation = Made4Net.Shared.AppConfig.Get("isOpenHoursValidation", "0")


        _strategyid = RoutingStrategyId
        Dim sql As String = String.Format("Select * from RoutingStrategyHeader Where StrategyId = '{0}'", _strategyid)

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            Load(dr)
            InitAvailableVehicle()
        End If

        ''        Dim dt As New Data(sql)
        ''      Dim dr As DataRow = dt.CreateDataRow()
    End Sub

    Public Sub New(ByVal dr As DataRow)
        _connectionname = DataInterface.ConnectionName
        ''_isoptimization = Made4Net.Shared.AppConfig.Get("isFinalOptimization", "0")
        _isDistanceAdd = Made4Net.Shared.AppConfig.Get("isDistanceAdd", 0)
        _isdistcost = Made4Net.Shared.AppConfig.Get("isDistCost", 0)
        _distcosttype = Made4Net.Shared.AppConfig.Get("DistCostType", 0)
        _civmultithreading = Made4Net.Shared.AppConfig.Get("CivMultiThreading", "0")
        _isOpenHoursValidation = Made4Net.Shared.AppConfig.Get("isOpenHoursValidation", "0")

        Load(dr)
        ''        _TargetGeoPointsNodes = New GeoPointNodeList
        '_TargetPointsOrders = New ArrayList
        '_TargetCompanies = New Hashtable
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub Load(ByVal dr As DataRow)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _description = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DESCRIPTION"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))

        _stratdetail = New RoutingStrategyDetailCollection(_strategyid)
        _vehicleallocdetail = New RoutingVehicleAllocationCollection(_strategyid)

        'Route Cost Default Values
        _DefCostPerDay = Made4Net.Shared.SysParam.Get("DefVehicleCostPerDay")
        _DefCostPerDistUnit = Made4Net.Shared.SysParam.Get("DefVehicleCostPerDistUnit")
        _DefCostPerHour = Made4Net.Shared.SysParam.Get("DefVehicleCostPerHour")


    End Sub

#End Region

#Region "Vehicle Allocation"
    <Obsolete()> _
    Public Function GetNextAvailableVehicle() As VehicleType
        Dim oVehicle As VehicleType = Me.StrategyVehicleAllocation.GetNextAvailableVehicle(False)

        If oVehicle Is Nothing Then oVehicle = Me.StrategyDetails(0).DefaultVehicle
        Return oVehicle
    End Function

    ''trips 06/09/2011
    
    Public Function GetNextAvailableVehicleWithTrips(ByVal numExit As Integer, _
            ByVal prevcount As Integer) As VehicleType

        Dim oVehicle As VehicleType

        If StrategyDetails(0).MaximizeTripsPerVehicle Then
            If numExit = 0 Then
                oVehicle = getMinEndDatetimeVehicleFromPool()
            End If

            If oVehicle Is Nothing Then
                VehicleCounterFromPool = Me.StrategyVehicleAllocation.GetNumAllAvailableVehicle()
                oVehicle = Me.StrategyVehicleAllocation.GetNextAvailableVehicle(True)
            End If
        Else
            VehicleCounterFromPool = Me.StrategyVehicleAllocation.GetNumAllAvailableVehicle()
            oVehicle = Me.StrategyVehicleAllocation.GetNextAvailableVehicle(True)

            If oVehicle Is Nothing And numExit = 0 Then
                oVehicle = getMinEndDatetimeVehicleFromPool()
            End If

        End If


        If oVehicle Is Nothing And _
        Not StrategyDetails(0).DefaultVehicle Is Nothing Then
            oVehicle = Me.StrategyDetails(0).DefaultVehicle
            oVehicle.VehiclePriorityinPool = DefaultVehicleGroup '9999
            VehicleCounterFromPool = DefaultVehicleGroup
            If Not _logger Is Nothing Then
                _logger.Write("Not enough allocated vehicles for strategy. Default Vehicle Assigned - Vehicle: " & CurrentVehicleKey)
            End If
        End If

        If oVehicle Is Nothing Then
            If Not _logger Is Nothing Then
                _logger.Write("*** Undefined strategy " & StrategyId & " default vehicle")
            End If
            Return oVehicle
        End If
        ''''''''''''''''

        Dim oVehicleDataArray As New ArrayList()
        Dim key As String = VehicleCounterFromPool.ToString() & "#" & oVehicle.VEHICLETYPEID
        If Not VechicleInTripPool.ContainsKey(key) Then
            ''0 - _vehicleCounterFromPool
            ''1 - oVehicle
            ''2 - TripNumber
            ''3 - EndDateTime
            ''4- TotalTripsDistance
            ''5- TotalTripsTime
            oVehicleDataArray.Add(VehicleCounterFromPool)
            oVehicleDataArray.Add(oVehicle)
            oVehicleDataArray.Add(0)
            oVehicleDataArray.Add(DateTime.MaxValue) '
            oVehicleDataArray.Add(0)
            oVehicleDataArray.Add(0)
            oVehicleDataArray.Add(DateTime.MaxValue) '
            oVehicleDataArray.Add(0) ''flag
            oVehicleDataArray.Add(prevcount) ''prevcount
            oVehicleDataArray.Add(0) ''attempts
            oVehicleDataArray.Add(oVehicle.VehiclePriorityinPool) ''vehicle prior

            VechicleInTripPool.Add(key, oVehicleDataArray)
        End If

        VechicleInTripPool(key)(7) = 1 ''set flag
        CurrentVehicleKey = key

        Return oVehicle

    End Function


    ''trips 06/09/2011
    Private Function getMinEndDatetimeVehicleFromPool() As VehicleType
        Dim oVehicle As VehicleType = Nothing


        Dim minDateTime As DateTime = DateTime.MaxValue
        Dim minKey As String
        For Each objKey As String In VechicleInTripPool.Keys
            If VechicleInTripPool(objKey)(0) = DefaultVehicleGroup Then Continue For

            Dim currentDatetime As DateTime = VechicleInTripPool(objKey)(3)
            Dim oCurrentVehicle As VehicleType = VechicleInTripPool(objKey)(1)
            Dim oCurrentVehiclePriority As Integer = VechicleInTripPool(objKey)(10)

            If currentDatetime < minDateTime And _
                VechicleInTripPool(objKey)(7) = 0 Then
                If VechicleInTripPool(objKey)(2) + 1 <= oCurrentVehicle.MAXROUTESPERDAY Then
                    ''add also
                    ''check if attempts-tripnumber=0 for the same prevcount
                    ''check VEHICLE min priority
                    ''check oRoutesetReqArrayList.Count (prevcount)
                    ''If VechicleInTripPool(objKey)(11) <= oCurrentVehiclePriority Or _
                    ''VechicleInTripPool(objKey)(10) > 0 Then

                    minDateTime = currentDatetime
                    minKey = objKey
                    VehicleCounterFromPool = VechicleInTripPool(objKey)(0)
                    oVehicle = VechicleInTripPool(objKey)(1)
                    CurrentVehicleKey = objKey
                End If
            End If
        Next
        If Not oVehicle Is Nothing Then
            VechicleInTripPool(CurrentVehicleKey)(7) = 1 ''set flag
        End If

        Return oVehicle
    End Function

    Public Sub RestoreTripVehicleData(ByVal pTotalTripsDistance As Double, _
        ByVal pTotalTripsTime As Double, _
        ByVal RestoreVehicleKey As String)
        If RestoreVehicleKey <> String.Empty AndAlso _
            VechicleInTripPool.ContainsKey(RestoreVehicleKey) Then
            Dim oVehicleDataArray As ArrayList = VechicleInTripPool(RestoreVehicleKey)
            oVehicleDataArray(2) -= 1
            oVehicleDataArray(3) = oVehicleDataArray(6)
            oVehicleDataArray(4) -= pTotalTripsDistance
            oVehicleDataArray(5) -= pTotalTripsTime
            oVehicleDataArray(7) = 0 ''release flag
            oVehicleDataArray(9) += 1 ''attempts

            If Not _logger Is Nothing Then
                _logger.Write("Trip DeAssigned -  Vehicle: " & RestoreVehicleKey & _
                    "  Trip Number: " & oVehicleDataArray(2).ToString() & _
                    "  End DateTime: " & oVehicleDataArray(3).ToString() & _
                    "  Total Trips Distance: " & oVehicleDataArray(4).ToString() & _
                    "  Total Trips Time: " & oVehicleDataArray(5).ToString())

            End If

        End If
    End Sub
    Public Sub SetTripVehicleData(ByVal pEndDateTime As DateTime, _
    ByVal pTotalTripsDistance As Double, _
    ByVal pTotalTripsTime As Double, _
    ByVal SaveVehicleKey As String, _
            ByRef TripGroup As String, _
            ByRef TripNum As Integer, _
                ByVal prevcount As Integer)

        If SaveVehicleKey <> String.Empty AndAlso _
            VechicleInTripPool.ContainsKey(SaveVehicleKey) AndAlso VehicleCounterFromPool <> DefaultVehicleGroup Then
            Dim oVehicleDataArray As ArrayList = VechicleInTripPool(SaveVehicleKey)
            oVehicleDataArray(2) += 1
            oVehicleDataArray(6) = oVehicleDataArray(3)
            oVehicleDataArray(3) = pEndDateTime
            oVehicleDataArray(4) += pTotalTripsDistance
            oVehicleDataArray(5) += pTotalTripsTime

            oVehicleDataArray(7) = 0 ''release flag

            TripGroup = oVehicleDataArray(0)
            TripNum = oVehicleDataArray(2)

            If Not _logger Is Nothing Then
                _logger.Write("Trip Assigned - Vehicle: " & SaveVehicleKey & _
                    "  Trip Group: " & TripGroup & _
                    "  Trip Number: " & TripNum.ToString() & _
                    "  End DateTime: " & oVehicleDataArray(3).ToString() & _
                    "  Total Trips Distance: " & oVehicleDataArray(4).ToString() & _
                    "  Total Trips Time: " & oVehicleDataArray(5).ToString())

            End If
        End If
    End Sub


    Public Sub InitAvailableVehicle()
        Me.StrategyVehicleAllocation.InitAvailableVehicle()
    End Sub

#End Region

#Region "Overrides"

    Public Overrides Function Add(ByVal value As Object) As Integer
        Dim tmpReq As RoutingRequirement = CType(value, RoutingRequirement)
        Dim tmpGeoPointNode As GeoPointNode

        If tmpGeoPointNode.isPointExist(tmpReq.PointID) Then
            MyBase.Add(value)
            Return 0
        Else
            Return -1
        End If

        'tmpGeoPointNode = GeoPointNode.GetPoints(Convert.ToInt32(tmpReq.TargetGeoPointID))
        '_
        'If tmpGeoPointNode Is Nothing Then Return -1
        'If Not _TargetGeoPointsNodes.Contains(tmpGeoPointNode) Then
        '    _TargetGeoPointsNodes.Add(tmpGeoPointNode)
        'End If
        ''key = tmpReq.Consignee & "_" & tmpReq.Company & "_" & tmpReq.CompanyType

    End Function

#End Region

#Region "Plan Routes"

    Public Sub PlanRoutes(ByVal pTerritoryKey As String, ByVal pRoutingSet As RoutingSet, _
                      ByVal RunId As String, _
                      ByVal clusterNumber As Integer, _
                      ByRef oRoutesetNodeArrayList As GeoPointArrayList, _
                      ByRef oRoutesetReqArrayList As ArrayList, _
                      ByVal numrun As Integer, _
                      ByVal alwaysAddLastRoute As Boolean, _
                        ByVal numexit As Integer, _
                      Optional ByVal oLogger As LogHandler = Nothing)
        Try

            Dim NumberOfGenes = 0
            Dim NumberOfGenesFull = 0

            Dim keygenesArray As ArrayList = New ArrayList
            Dim keygenesArrayShort As ArrayList = New ArrayList
            Dim keygenesHashPoints As Hashtable = New Hashtable()

            Dim LongbyShortHash As Hashtable = New Hashtable()
            ''Dim oRoutesetNodeArrayList As New GeoPointArrayList

            Dim sb As New StringBuilder
            sb.AppendLine()

            For i As Integer = 0 To Me.Count - 1
                Dim tmpReq As RoutingRequirement = Me.Item(i)
                If tmpReq.planned = 1 Then Continue For

                If tmpReq.TargetGeoPoint.CLUSTERNUMBER = clusterNumber Then

                    If Not keygenesHashPoints.Contains(tmpReq.PointID) Then
                        keygenesHashPoints.Add(tmpReq.PointID, i.ToString())

                        Dim gp As GeoPointNode
                        Try
                            gp = New GeoPointNode(tmpReq.PointID) '''''???check

                        Catch ex As Exception
                            If Not oLogger Is Nothing Then
                                sb.AppendLine("Point Error: " & tmpReq.PointID)
                            End If
                        End Try
                        If Not gp Is Nothing Then
                            keygenesArrayShort.Add(i)

                            If Not LongbyShortHash.Contains(NumberOfGenes) Then
                                LongbyShortHash.Add(NumberOfGenes, i)
                            End If

                            NumberOfGenes += 1
                            keygenesArray.Add(i)
                            NumberOfGenesFull += 1

                        End If
                    Else
                        keygenesHashPoints(tmpReq.PointID) &= "#" & i.ToString()
                        keygenesArray.Add(i)
                        NumberOfGenesFull += 1
                    End If


                End If
            Next

            _RoutingSet = pRoutingSet
            _RunId = RunId

            _logger = oLogger

            ''test log
            Dim oEnCluster As IDictionaryEnumerator = keygenesHashPoints.GetEnumerator()
            oEnCluster.Reset()
            Dim sPoints As String = "#Cluster num #" & clusterNumber.ToString() & " with Points count-" & keygenesHashPoints.Count.ToString() & ": "
            While (oEnCluster.MoveNext())
                sPoints &= oEnCluster.Key.ToString() & ","
            End While
            If Not oLogger Is Nothing Then
                sPoints = sPoints.Substring(0, sPoints.Length - 1)
                sb.AppendLine(sPoints)
            End If
            ''end test log



            If NumberOfGenes > 0 Then
                Dim ChromArray(NumberOfGenes - 1) As Integer
                ''                ChromArray = oRoutesetNodeArrayList.SortbyMiddlePoint()

                Dim g As Integer = 1
                For gi As Integer = 0 To ChromArray.Length - 1
                    ChromArray(gi) = g
                    g = g + 1
                Next


                Dim BestKnownChrom As Chromosome = New Chromosome(ChromArray)
                Dim BestKnownfitness As Double

                BestKnownfitness = DistanceRouteCost(keygenesArrayShort, BestKnownChrom)
                BestKnownChrom.SetFitness(BestKnownfitness)


                Dim BNC As Chromosome
                Dim BNF As Double

                If NumberOfGenes < 3 Then
                    BNC = BestKnownChrom
                    BNF = BestKnownChrom.GetFitness()

                    If NumberOfGenes = 2 Then
                        Dim oRoutingStrategyDetail As RoutingStrategyDetail = DirectCast(StrategyDetails(0), RoutingStrategyDetail)

                        If oRoutingStrategyDetail.CalcRetTimeToFirstStop Or oRoutingStrategyDetail.CalcRetTimeToDepot Then
                            ChromArray(0) = 2
                            ChromArray(1) = 1
                            Dim OppositeChromosome As Chromosome = New Chromosome(ChromArray)
                            If BNF > DistanceRouteCost(keygenesArrayShort, OppositeChromosome) Then
                                BestKnownChrom = OppositeChromosome
                                BNC = OppositeChromosome
                            End If
                        End If
                    End If
                Else

                    Dim pGAParams As GAParams = New GAParams(NumberOfGenes)
                    pGAParams.TAG = clusterNumber
                    Dim CivilizationsCount As Integer = pGAParams.CIVILIZATIONS

                    If Not oLogger Is Nothing Then
                        sb.AppendLine()
                        sb.AppendLine(_strategyid & ": " & _description & " Initiated it's Genetinc Solver...")
                        sb.AppendLine("Cluster Number: " & clusterNumber.ToString())
                        sb.AppendLine("Number of Tasks : " & NumberOfGenesFull.ToString())
                        sb.AppendLine("Number of Genes (Stops) in cluster: " & NumberOfGenes)
                        sb.AppendLine("Population: " & pGAParams.POPULATIONSIZE)
                        sb.AppendLine("Civilizations: " & pGAParams.CIVILIZATIONS)
                        sb.AppendLine("Generations: " & pGAParams.GENERATIONS)
                        sb.AppendLine("Genetinc Solver Started Running...")
                    End If

                    Dim currChrom As Chromosome = BestKnownChrom
                    Dim currfitness As Double = BestKnownfitness


                    Dim dirpath2 As String = "c:\\tmp\\" ''' 
                    Dim LoggingEnabled2 As String = Made4Net.Shared.AppConfig.Get("UseShortLogs", "")
                    If LoggingEnabled2 = "1" Then
                        dirpath2 = Made4Net.Shared.AppConfig.Get("ServiceShortLogDirectory", "")
                    Else
                        dirpath2 = ""
                    End If

                    iCivCount = 0

                    Dim eventsCivilizations As List(Of ManualResetEvent) = New List(Of ManualResetEvent)

                    bestChromosomeHashHash.Add(pTerritoryKey & "##" & clusterNumber & "#" & numrun, New Hashtable())
                    _ChromosomePool.Clear()
                    For i As Integer = 0 To CivilizationsCount - 1
                        pGAParams.TAG1 = i
                        oGeneticSolver = New GeneticSolver(Me, pGAParams, keygenesArrayShort, dirpath2)
                        Dim PRO As New PlanRoutesObject(oGeneticSolver, pGAParams, keygenesArrayShort, 1, _
                        clusterNumber, numrun, pTerritoryKey)

                        PRO.signal = New ManualResetEvent(False)
                        eventsCivilizations.Add(PRO.signal)

                        '' t + !!!''''''''''''return

                        Select Case _civmultithreading
                            Case "1"
                                Dim job As ParameterizedThreadStart = New ParameterizedThreadStart(AddressOf runPlanRoutes)
                                Dim thread As Thread = New Thread(job)
                                thread.Start(PRO)
                            Case "2"
                                runPlanRoutes(PRO)
                            Case Else ''0 default
                                ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf runPlanRoutes), PRO)
                                Thread.Sleep(2)
                        End Select


                    Next
                    WaitHandle.WaitAll(eventsCivilizations.ToArray())
                    'If Not oLogger Is Nothing Then
                    '    sb.AppendLine("_ChromosomePool after civ:" & _ChromosomePool.Count.ToString)
                    'End If


                    Dim oEn As IDictionaryEnumerator = bestChromosomeHashHash(pTerritoryKey & "##" & clusterNumber & "#" & numrun).GetEnumerator()
                    oEn.Reset()
                    Dim k As Integer = 0


                    BNC = New Chromosome(ChromArray)
                    BNF = Double.MaxValue

                    While (oEn.MoveNext())
                        currChrom = DirectCast(oEn.Value, Chromosome)
                        If Not currChrom Is Nothing Then
                            currfitness = currChrom.GetFitness()
                            If BNF > currfitness Then
                                BNF = currfitness
                                BNC = currChrom
                            End If
                        Else
                            If Not oLogger Is Nothing Then
                                sb.AppendLine("currfitness0:" & currfitness.ToString)
                            End If
                        End If
                        k += 1
                    End While

                    If Not oLogger Is Nothing Then
                        sb.AppendLine("Best Known fitness:" & BNF.ToString & " of " & CivilizationsCount)
                    End If
                End If


                Dim BNCChromInt(NumberOfGenesFull - 1) As Integer
                Dim j As Integer = 0

                For si As Integer = 0 To BNC.GetSize() - 1
                    Dim keygenesHashKey As String = Me.Item(LongbyShortHash(BNC.GetGene(si) - 1)).PointID
                    Dim keygenesHashValue As String() = keygenesHashPoints(keygenesHashKey).Split("#")

                    For Each genid As Integer In keygenesHashValue
                        BNCChromInt(j) = genid
                        j += 1
                    Next
                Next
                Dim BNCFull As Chromosome = New Chromosome(BNCChromInt)

                If Not _logger Is Nothing Then
                    _logger.Write(sb.ToString())
                    _logger.writeSeperator("*", 100)
                End If

                ''''''''''''''''
                Dim routeCount As Integer = 0
                Dim resDecodeChromosome As ArrayList = decodeChromosome(BNCFull, routeCount, numexit, oRoutesetReqArrayList.Count)
                sb = New StringBuilder
                UpdateResults(resDecodeChromosome, oRoutesetNodeArrayList, oRoutesetReqArrayList, routeCount, alwaysAddLastRoute, sb, numexit)

                If Not _logger Is Nothing Then
                    _logger.Write(sb.ToString())
                    _logger.writeSeperator("*", 100)
                End If
            End If

        Catch ex As Exception
            _logger.writeSeperator("*", 100)
            _logger.Write("PlanRoutes Error: " & ex.ToString())
            _logger.Write("PlanRoutes clusterNumber: " & clusterNumber.ToString())

            _logger.writeSeperator("*", 100)

        End Try

    End Sub


    Private Sub runPlanRoutes(ByVal p As Object)
        Dim PRO As PlanRoutesObject = DirectCast(p, PlanRoutesObject)
        Dim oGAParams As GAParams = DirectCast(PRO.oGAParams, GAParams)

        Dim oGeneticSolver As GeneticSolver = DirectCast(PRO.oGeneticSolver, GeneticSolver)
        Dim bestChromosome As New Chromosome(oGAParams.NUMBEROFGENES)
        Try

            DataInterface.ConnectionName = _connectionname

            ''        _keygenesArray = PRO.keygenesArray
            bestChromosome = oGeneticSolver.Run()

            SyncLock bestChromosomeHashHash(PRO.territoryKey & "##" & PRO.clusterNumber & "#" & PRO.clusterCount)
                Dim r As New Random(DateTime.Now.Millisecond)
                Dim hashkey As String
                Do
                    Thread.Sleep(1)
                    hashkey = iCivCount.ToString() & "_" & _
                        (((Thread.CurrentThread.GetHashCode().ToString() & "_") & _
                        DateTime.Now.Millisecond.ToString() & "_") & _
                        DateTime.Now.Ticks.ToString() & "_") & _
                        r.[Next](10000).ToString()
                Loop While bestChromosomeHashHash(PRO.territoryKey & "##" & PRO.clusterNumber & "#" & PRO.clusterCount).ContainsKey(hashkey)

                bestChromosomeHashHash(PRO.territoryKey & "##" & PRO.clusterNumber & "#" & PRO.clusterCount).Add(hashkey, DirectCast(bestChromosome, Chromosome))
            End SyncLock

        Catch ex As Exception
            _logger.writeSeperator("*", 100)
            _logger.Write("Genetic Error: " & ex.Message)
            _logger.writeSeperator("*", 100)
        Finally
            PRO.signal.Set()
            Interlocked.Increment(iCivCount)

        End Try
    End Sub


#Region "Build Routes"

    Private Function BuildNewRoute(ByVal pRouteParams As String, ByVal pUser As String) As Route
        Try
            Dim oRoute As New Route

            'We have a new Route -> Extract the distance and the Driving time params
            Dim CurrentRouteDist, CurrentRouteTime, CurrentRouteVol, CurrentRouteWeight As Double
            Dim startTime, endTime, CurrentRouteVehicleType, startPoint, endPoint, depoPoint As String
            Dim TerritorySetID, TerritoryID As String

            Try
                CurrentRouteDist = pRouteParams.Split("#")(0)
            Catch ex As Exception
                CurrentRouteDist = -1
                _logger.Write("Could not convert param CurrentRouteDist to double. Value - " & pRouteParams.Split("#")(0))
            End Try
            Try
                CurrentRouteTime = pRouteParams.Split("#")(1)
            Catch ex As Exception
                CurrentRouteTime = -1
                _logger.Write("Could not convert param CurrentRouteTime to double. Value - " & pRouteParams.Split("#")(1))
            End Try
            Try
                CurrentRouteVol = pRouteParams.Split("#")(2)
            Catch ex As Exception
                CurrentRouteVol = -1
                _logger.Write("Could not convert param CurrentRouteVol to double. Value - " & pRouteParams.Split("#")(2))
            End Try
            Try
                CurrentRouteWeight = pRouteParams.Split("#")(3)
            Catch ex As Exception
                CurrentRouteWeight = -1
                _logger.Write("Could not convert param CurrentRouteWeight to double. Value - " & pRouteParams.Split("#")(3))
            End Try
            CurrentRouteVehicleType = pRouteParams.Split("#")(4)
            startTime = pRouteParams.Split("#")(5)
            endTime = pRouteParams.Split("#")(6)

            startPoint = pRouteParams.Split("#")(7)
            endPoint = pRouteParams.Split("#")(8)
            depoPoint = pRouteParams.Split("#")(9)
            TerritorySetID = pRouteParams.Split("#")(10)
            TerritoryID = pRouteParams.Split("#")(11)

            Dim numStops, numTasks, numDays As Integer
            numStops = pRouteParams.Split("#")(16)
            numTasks = pRouteParams.Split("#")(17)

            numDays = pRouteParams.Split("#")(20)

            Dim routeStartDate As DateTime = pRouteParams.Split("#")(21)
            ''if needs check route minimums and donnot create

            ''Dim oVehicleDataArray As ArrayList = Me.VechicleInTripPool(CurrentVehicleKey)
            ''Dim TripNumber As Integer = oVehicleDataArray(2)
            ''Dim TripGroup As String = oVehicleDataArray(0).ToString()  ''_vehicleCounterFromPool
            Dim TripNumber As Integer = pRouteParams.Split("#")(23)
            Dim TripGroup As String = pRouteParams.Split("#")(22)

            oRoute.Create(Nothing, _RoutingSet.SetID, depoPoint, Me.StrategyDetails(0).RouteNamePrefix, routeStartDate, startPoint, endPoint, "", CurrentRouteVehicleType, "", _
                TerritorySetID, TerritoryID, _RunId, 0, startTime, endTime, numDays, CurrentRouteDist, CurrentRouteTime, CurrentRouteWeight, CurrentRouteVol, _strategyid, pUser, _
                TripNumber, TripGroup)

            ' ''check direction
            'oRoute.ChangeRouteDirection(True, _logger)
            Return oRoute

        Catch ex As Exception
            _logger.Write("BuildNewRoute: " & ex.ToString())
        End Try
    End Function

    Private Sub AddRouteStop(ByVal pRouteParams As String, ByRef oRoute As Route, ByVal oReq As RoutingRequirement, ByVal pUser As String)
        Try
            Dim estArrvHour As Int32
            Dim estDepartureHour As Int32
            Dim addDays As Integer
            Try
                estArrvHour = pRouteParams.Split("#")(3)
                estDepartureHour = pRouteParams.Split("#")(4)

                addDays = pRouteParams.Split("#")(5)

            Catch ex As Exception
                estArrvHour = -1
                _logger.Write("Could not convert param estArrvHour to double. Value - " & pRouteParams.Split("#")(3))
            End Try

            'Add the stop to the route
            Dim oRouteStop As RouteStop = oRoute.AddStop("", oReq.PointID, estArrvHour, estDepartureHour, _
                    pUser, addDays)

            Dim oStopTaskType As StopTaskType
            Select Case oReq.PDType
                Case PDType.Delivery
                    oStopTaskType = StopTaskType.Delivery
                Case PDType.Pickup
                    oStopTaskType = StopTaskType.PickUp
                Case PDType.General
                    oStopTaskType = StopTaskType.General
            End Select


            oRouteStop.AddStopDetail(oStopTaskType, oRoute.RouteDate, oReq.Consignee, oReq.OrderId, _
                oReq.OrderType, oReq.TargetCompany, oReq.CompanyType, oReq.ContactId, oReq.ChkPnt, "", 0, _
                oReq.TransportationClass, oReq.OrderVolume, oReq.OrderWeight, 0, "", _
                StopTaskConfirmationType.None, pUser)

        Catch ex As Exception
            _logger.Write("AddRouteStop error: " & ex.ToString)

        End Try

    End Sub
    <Obsolete()> _
    Private Function CalcEstimatedDepartureHour(ByVal oComp As Company, ByVal pArrvHour As Int32) As Int32
        Dim itotalTime As Int32
        Dim tmpTime As Int32
        If oComp.SERVICETIME <> 0 Then
            itotalTime = Convert.ToInt32(oComp.SERVICETIME)
        Else
            itotalTime = Convert.ToInt32(Me.StrategyDetails(0).MinServiceTime)
        End If



        Dim hr, min, hr1, min1 As Int32
        hr = itotalTime / 60
        min = itotalTime Mod 60
        tmpTime = (pArrvHour + hr * 100 + min)
        If (tmpTime Mod 100) / 60 > 1 Then
            min1 = (tmpTime Mod 100) Mod 60
            hr1 = (tmpTime Mod 100) / 60
            Return Math.Floor(tmpTime / 100) * 100 + hr1 * 100 + min1
        Else
            Return tmpTime
        End If
    End Function

#End Region

#End Region

#Region "IGeneticDataModel & GeoData Implementation"

    ''obsolute
    Public Sub OptimizeRoute(ByVal oRoute As Route)
        Dim sourceArrayList As ArrayList = New ArrayList()
        For Each oRouteStop As RouteStop In oRoute.Stops
            sourceArrayList.Add(oRouteStop.PointId)
        Next
        Dim oResequenceRouting As ResequenceRouting = New ResequenceRouting(sourceArrayList, _logger)
        Dim targetArrayList As ArrayList = oResequenceRouting.runResequenceRoutes()

        For i As Integer = 0 To oResequenceRouting.BestKnownChrom.GetSize() - 1
            Dim newStopNumber = oResequenceRouting.BestKnownChrom.GetGene(i)
            If i + 1 <> newStopNumber Then UpdateStopnumber(oRoute.RouteId, i + 1, newStopNumber)

        Next



    End Sub

    Private Sub UpdateStopnumber(ByVal routeID As String, _
            ByVal oldStopNumber As Integer, _
            ByVal newStopNumber As Integer)
        Try

            Dim whc As String = String.Format(" routeid = {0} and stopnumber = {1} ", _
                Made4Net.Shared.Util.FormatField(routeID), _
                Made4Net.Shared.Util.FormatField(newStopNumber))
            Dim SQL As String = String.Format("select count(*) from routestop where {0} ", whc)
            Dim isExistsNewStop As Integer = DataInterface.ExecuteScalar(SQL)
            Dim tempStopNumber As Int32

            If isExistsNewStop > 0 Then
                tempStopNumber = (New Random).Next(9000000, 9999999)
                Dim oNewRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(routeID, newStopNumber)
                oNewRouteStop.UpdateStopNumber(tempStopNumber, _adduser)
            End If

            Dim oRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(routeID, oldStopNumber)
            oRouteStop.UpdateStopNumber(newStopNumber, _adduser)

            If isExistsNewStop > 0 Then
                Dim otempRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(routeID, tempStopNumber)
                otempRouteStop.UpdateStopNumber(oldStopNumber, _adduser)
            End If

        Catch ex As Exception
            _logger.WriteException(ex)
        End Try

    End Sub



    Public Sub UpdateResults(ByVal result As System.Collections.ArrayList, _
                        ByRef oRoutesetNodeArrayList As GeoPointArrayList, _
                        ByRef oRoutesetReqArrayList As ArrayList, _
                        ByVal routeCount As Integer, _
                        ByVal alwaysAddLastRoute As Boolean, _
                        ByRef sb As StringBuilder, _
                        ByVal numexit As Integer)
        sb.AppendLine()

        Try
            Dim user As String = "SYSTEM"
            Dim i As Int32
            Dim oRoute As Route
            Dim oRouteStop As RouteStop
            Dim oRouteStopDetail As RouteStopTask
            Dim oRouteOptArray As ArrayList = New ArrayList
            Dim currCLUSTERNUMBER As Integer

            Dim cntNew As Integer = 1


            For i = 0 To result.Count - 1
                If Convert.ToString(result(i)) = "@" Then
                    ''                    If (Convert.ToInt32(Convert.ToString(result(i + 1)).Split("#")(6)) > 0) Then
                    Dim stopArr As Array = Convert.ToString(result(i + 1)).Split("#")
                    If (stopArr.Length > 0) Then
                        Dim isFillTargetCapacity As Boolean = Convert.ToBoolean(Convert.ToString(result(i + 1)).Split("#")(18))
                        If cntNew = routeCount And routeCount <> 1 And isFillTargetCapacity = False And Not alwaysAddLastRoute Then
                            sb.AppendLine()
                            sb.AppendLine("### Could not place Route. Basket: " & (cntNew - 1).ToString & " with " & result(i + 1).Split("#")(17).ToString & " task(s) -  Not enough capacity for last route - Volume: " & Convert.ToString(result(i + 1)).Split("#")(2) & " Weight: " & Convert.ToString(result(i + 1)).Split("#")(3))


                            Dim RestoreVehicleKey As String = Convert.ToString(result(i + 1)).Split("#")(24) & "#" & Convert.ToString(result(i + 1)).Split("#")(4)
                            Dim TotalTripsDistance As Double = Convert.ToString(result(i + 1)).Split("#")(0)
                            Dim TotalTripsTime As Double = Convert.ToString(result(i + 1)).Split("#")(1)
                            Dim RestoreVehicleID As String = Convert.ToString(result(i + 1)).Split("#")(4)
                            Dim TripNum As Integer = Convert.ToString(result(i + 1)).Split("#")(23)

                            If RestoreVehicleKey.Split("#")(0) <> DefaultVehicleGroup Then
                                RestoreTripVehicleData(TotalTripsDistance, TotalTripsTime, RestoreVehicleKey)
                            End If

                            If TripNum = 1 Or numexit < 2 Then
                                StrategyVehicleAllocation.RestoreAvailableVehicle(RestoreVehicleID)
                            End If


                            i = i + 1
                            While Convert.ToString(result(i + 1)) <> "@"
                                i = i + 1
                                If i = result.Count - 1 Then Exit While
                            End While
                        Else
                            oRoute = BuildNewRoute(Convert.ToString(result(i + 1)), user)
                            oRouteOptArray.Add(oRoute)

                            'Dim TotalTripsDistance As Double = Convert.ToString(result(i + 1)).Split("#")(0)
                            'Dim TotalTripsTime As Double = Convert.ToString(result(i + 1)).Split("#")(1)
                            'Dim RouteStartTime As Integer = Convert.ToString(result(i + 1)).Split("#")(5)
                            'Dim RouteDepoStartDate As DateTime = Convert.ToString(result(i + 1)).Split("#")(21)
                            'Dim StartDateTime As DateTime = New DateTime(RouteDepoStartDate.Year, _
                            '        RouteDepoStartDate.Month, _
                            '        RouteDepoStartDate.Day, _
                            '        RouteStartTime \ 100, _
                            '        RouteStartTime Mod 100, 0)
                            'Dim RouteEndTime As DateTime = StartDateTime.AddSeconds(TotalTripsTime)
                            'Me.StrategyVehicleAllocation.GetNextAvailableVehicle(True)
                            'Dim SaveVehicleKey As String = Convert.ToString(result(i + 1)).Split("#")(24) & "#" & Convert.ToString(result(i + 1)).Split("#")(4)
                            'Me.SetTripVehicleData(RouteEndTime, TotalTripsDistance, TotalTripsTime, SaveVehicleKey)


                            i = i + 1
                            cntNew += 1
                            sb.AppendLine()
                        End If
                    Else
                        While Convert.ToString(result(i + 1)) <> "@"
                            i = i + 1
                            If i = result.Count - 1 Then Exit While
                        End While
                    End If
                ElseIf Convert.ToString(result(i)) <> "-" Then
                    'we should add stop and stop detail
                    Dim tmpReq As RoutingRequirement = CType(Me.Item((Convert.ToString(result(i)).Split("#")(0))), RoutingRequirement)
                    currCLUSTERNUMBER = tmpReq.TargetGeoPoint.CLUSTERNUMBER

                    If Not _logger Is Nothing Then
                        sb.AppendLine("Cluster: #" & currCLUSTERNUMBER & " Route: " & oRoute.RouteId & " Add Stop: (point - " & tmpReq.PointID & " contact - " & tmpReq.ContactId & " order - " & tmpReq.OrderId & " n#d#t#arr#dep#days: " & result(i).ToString())
                    End If

                    AddRouteStop(Convert.ToString(result(i)), oRoute, tmpReq, user)
                    oRoutesetReqArrayList.Remove(tmpReq)
                    tmpReq.planned = 1
                End If
            Next

            sb.AppendLine()
            For Each oRoute In oRouteOptArray
                Dim RouteCost As Double = oRoute.CalculateRouteCost()
                oRoute.SetRouteCost(RouteCost)

                If Not _logger Is Nothing Then
                    sb.AppendLine("Route: " & oRoute.RouteId)
                    sb.AppendLine("Route StrategyID: " & oRoute.StrategyId)
                    sb.AppendLine("Route TerrytorySetID: " & oRoute.TerritorySETID)
                    sb.AppendLine("Route TerrytoryID: " & oRoute.Territory)
                    sb.AppendLine("Route Cluster: #" & currCLUSTERNUMBER)
                    sb.AppendLine("Route Start Date Time: " & oRoute.StartDate.ToString)
                    sb.AppendLine("Route End Date Time: " & oRoute.EndDate.ToString)
                    sb.AppendLine("Route Cost: " & oRoute.RouteCost)
                    sb.AppendLine("Route Start Point: " & oRoute.StartPoint)
                    sb.AppendLine("Route End Point: " & oRoute.EndPoint)
                    sb.AppendLine("Route Depot Point: " & oRoute.Depo)

                    sb.AppendLine("Route Distance: " & oRoute.TotalDistance)
                    sb.AppendLine("Route Time: " & oRoute.TotalTime)
                    sb.AppendLine("Route Volume: " & oRoute.TotalVolume)
                    sb.AppendLine("Route Weight: " & oRoute.TotalWeight)
                    sb.AppendLine("Route Vehicle Type: " & oRoute.VehicleType)
                    sb.AppendLine("Trip Group: " & oRoute.TripGroup)
                    sb.AppendLine("Trip Num: " & oRoute.TripNum)

                    sb.AppendLine("Total Route Stops " & oRoute.RouteId & ": " & oRoute.getTotalStopNumber())
                    sb.AppendLine("Total Route Tasks " & oRoute.RouteId & ": " & oRoute.getTotalTaskNumber())

                    oRoute.SetStatus(RouteStatus.Planned, DateTime.Now, Common.GetCurrentUser())
                    oRoute.SetStatus(RouteStatus.Assigned, DateTime.Now, Common.GetCurrentUser())

                    sb.AppendLine()

                End If
            Next
            sb.AppendLine()




        Catch ex As Exception
            sb.AppendLine("UpdateResults error: " & ex.ToString())
        End Try
    End Sub

    Public Function decodeChromosome(ByVal chrom As Made4Net.Algorithms.GeneticAlgorithm.Chromosome, _
            ByRef routeCount As Integer, _
            ByVal numexit As Integer, _
    ByVal prevcount As Integer) As System.Collections.ArrayList

        Dim oRoutingBasketsCol As New RouteBasketsCollection(Me, _RoutingSet.DistributionDate, Nothing) '_OrdersRoutingParamTable)
        Dim logLevel As String = System.Configuration.ConfigurationManager.AppSettings.Get("LogLevel")
        Dim al As ArrayList
        Dim sb As New StringBuilder
        If logLevel = "1" Then
            al = oRoutingBasketsCol.decodeChromosome2(chrom, sb, Nothing, numexit, prevcount)
        Else
            al = oRoutingBasketsCol.decodeChromosome2(chrom, sb, _logger, numexit, prevcount)
            _logger.Write(sb.ToString())
        End If
        routeCount = oRoutingBasketsCol.Count
        oRoutingBasketsCol = Nothing
        Return al
    End Function


    Private Function DistanceRouteCost(ByVal keygenesArray As ArrayList, _
                                    ByVal chrom As Chromosome) As Double
        Dim DistCost As Double = 0D
        Dim DrivingTimecost As Double = 0D
        Dim DistRoutcost As Double = 0D

        ''25/04
        Dim DistToDepot As Double = 0D
        Dim TimetoDepot As Double = 0D
        Dim oRoutingStrategyDetail As RoutingStrategyDetail = DirectCast(StrategyDetails(0), RoutingStrategyDetail)
        If oRoutingStrategyDetail.CalcRetTimeToFirstStop Then
            Dim StartPoint As GeoPointNode = Me(keygenesArray(chrom.GetGene(0) - 1)).TargetGeoPoint()
            GeoNetworkItem.GetDistance(oRoutingStrategyDetail.DepoPointNode, StartPoint, DistToDepot, TimetoDepot, "", _isDistanceAdd, _
                    _isdistcost, _distcosttype)
            DistRoutcost += DistToDepot
        End If

        Try
            For i As Integer = 0 To chrom.GetSize() - 2
                Dim sourceReq As RoutingRequirement = Me(keygenesArray(chrom.GetGene(i) - 1))
                Dim targetReq As RoutingRequirement = Me(keygenesArray(chrom.GetGene(i + 1) - 1))


                Dim sourcePoint As GeoPointNode = sourceReq.TargetGeoPoint()
                Dim targetPoint As GeoPointNode = targetReq.TargetGeoPoint()

                GeoNetworkItem.GetDistance(sourcePoint, targetPoint, DistCost, DrivingTimecost, "", _isDistanceAdd, _
                    _isdistcost, _distcosttype)

                ''Add penalty factors
                If StrategyDetails(0).OpenHourMissPenalty > 0 Then
                    Dim AddCostFactor As Integer = StrategyDetails(0).OpenHourMissPenalty

                    Dim AddCost As Double = 0
                    Dim sourceCH As Integer = sourceReq.ContactObj.CloseHour(_RoutingSet.DistributionDate)
                    Dim sourceOH As Integer = sourceReq.ContactObj.OpeningHour(_RoutingSet.DistributionDate)

                    Dim targetOH As Integer = targetReq.ContactObj.OpeningHour(_RoutingSet.DistributionDate)
                    Dim targetCH As Integer = targetReq.ContactObj.CloseHour(_RoutingSet.DistributionDate)

                    Dim temptime As Double = DrivingTimecost + oRoutingStrategyDetail.AllowedTimeBeforeOpen
                    Dim diftime, diftime2 As Double
                    If _isOpenHoursValidation AndAlso DrivingTimecost > 0 Then
                        ''TCH-SOH-DT>=0
                        diftime = RouteBaskets.DifTimeSec(sourceOH, targetCH) - DrivingTimecost

                        ''TOH-SCH-DT-WT<=0
                        diftime2 = RouteBaskets.DifTimeSec(sourceCH, targetOH) - temptime

                        If diftime <= 0 Then AddCost += AddCostFactor
                        If diftime2 >= 0 Then AddCost += AddCostFactor
                    End If

                    If DrivingTimecost > oRoutingStrategyDetail.MaxTimeBetweenStops Then AddCost += AddCostFactor
                    If DistCost > oRoutingStrategyDetail.MaxDistanceBetweenStops Then AddCost += AddCostFactor

                    DistRoutcost += DistCost + AddCost
                    'If diftime <= 0 OrElse diftime2 >= 0 Then
                    '    If Not _logger Is Nothing Then
                    '        _logger.Write("AddCost: " & AddCost.ToString & _
                    '        " source ContactId:" & sourceReq.ContactId & _
                    '        " sourceCH:" & sourceCH.ToString & _
                    '        " sourceOH:" & sourceOH.ToString & _
                    '        " target ContactId:" & targetReq.ContactId & _
                    '        " targetCH:" & targetCH.ToString & _
                    '        " targetOH:" & targetOH.ToString & _
                    '        " diftime:" & diftime.ToString & _
                    '        " diftime2:" & diftime2.ToString & _
                    '        " DrivingTimecost:" & DrivingTimecost.ToString & _
                    '        " DistCost:" & DistCost.ToString)

                    '    End If
                    'End If
                End If

            Next

            ''??
            'If oRoutingStrategyDetail.CalcRetTimeToFirstStop Then
            '    Dim StartPoint As GeoPointNode = Me(keygenesArray(chrom.GetSize() - 1)).TargetGeoPoint()
            '    GeoNetworkItem.GetDistance(StartPoint, oRoutingStrategyDetail.DepoPointNode, DistToDepot, TimetoDepot, "", _isDistanceAdd, _
            '            _isdistcost, _distcosttype)
            '    DistRoutcost += DistToDepot
            'End If

            Return DistRoutcost

        Catch ex As Exception
            If Not _logger Is Nothing Then
                _logger.Write("DistanceRouteCost Error: " & ex.ToString)
            End If
            Return Me.StrategyDetails(0).MaxDistancePerRoute * 10   '' Integer.MaxValue

        End Try

    End Function


    Private Function EvaluateChromosomeFitness(ByVal Chrom As Made4Net.Algorithms.GeneticAlgorithm.Chromosome, _
                                    ByVal keygenesArray As ArrayList) As Double _
        Implements Made4Net.Algorithms.Interfaces.IGeneticDataModel.EvaluateChromosomeFitness

        Dim chromfitness As Double = Integer.MaxValue
        Try
            If Chrom Is Nothing Then Return chromfitness
            Dim sk As String = Me(keygenesArray(Chrom.GetGene(0) - 1)).TargetGeoPoint().Clusternumber().ToString() & _
               "#" & Chrom.getGenKey()

            If _ChromosomePool.ContainsKey(sk) Then
                chromfitness = _ChromosomePool.Item(sk)
            Else
                chromfitness = DistanceRouteCost(keygenesArray, Chrom)


                ''Integer.MaxValue-cache size
                If (_ChromosomePool.Count = Integer.MaxValue) Then
                    _ChromosomePool = New Hashtable
                End If
                Try
                    SyncLock _ChromosomePool
                        If Not _ChromosomePool.ContainsKey(sk) Then
                            _ChromosomePool.Add(sk, chromfitness)
                        End If
                    End SyncLock
                Catch ex As Exception
                    If Not _logger Is Nothing Then
                        _logger.Write("Evaluate Fitness add to pool Error: " & ex.ToString)
                    End If

                End Try
            End If


            If Not Chrom Is Nothing Then Chrom.SetFitness(chromfitness)
            Return chromfitness

        Catch ex As Exception
            If Not _logger Is Nothing Then
                _logger.Write("Evaluate Fitness Error: " & ex.ToString)
            End If
            Return chromfitness
        End Try


    End Function

    Private Function GetCost(ByVal FromPoint As Int32, ByVal ToPoint As Int32, ByVal pCostParam As String) As Double Implements Made4Net.Algorithms.Interfaces.IGeneticDataModel.GetCost
        Try
            If FromPoint = ToPoint Then Return 0
            FromPoint -= 1
            ToPoint -= 1
            Dim tempDist As Double = Double.MaxValue
            Dim tmpDrivingTime As Double = Double.MaxValue
            Dim Source As GeoPointNode = DirectCast(Me.Item(FromPoint), RoutingRequirement).TargetGeoPoint
            Dim Target As GeoPointNode = DirectCast(Me.Item(ToPoint), RoutingRequirement).TargetGeoPoint

            GeoNetworkItem.GetDistance(Source, Target, tempDist, tmpDrivingTime, "", _isDistanceAdd, _isdistcost, _
                    _distcosttype)

            If pCostParam = "DRIVINGDIST" Then
                Return tempDist
            ElseIf pCostParam = "DRIVINGTIME" Then
                Return tmpDrivingTime
            ElseIf pCostParam = "" Then
                Return tempDist
            End If

        Catch ex As Exception
            If Not _logger Is Nothing Then
                _logger.writeSeperator("Get Distance Error (Get Cost):", ex.Message & " From:" & FromPoint.ToString() & " To: " & ToPoint.ToString())
            End If
            Return 0

        End Try
    End Function


    <Obsolete()> _
    Private Function RouteCost(ByVal DecodedChrom As ArrayList) As Double
        Dim i, j As Int32
        Dim TotalCost As Double = 0
        Dim FullTotalCost As Double = 0

        Dim tmpDist, tmpDrvTime As Double
        Dim vehType As VehicleType
        Dim chromStr As String = ""
        For i = 0 To DecodedChrom.Count - 1
            chromStr += Convert.ToString(DecodedChrom(i))
        Next
        Dim chromArr() As String = chromStr.Split("@")


        For i = 0 To chromArr.Length - 1
            Try

                If chromArr(i).Length > 0 Then
                    Dim routeArr() As String = chromArr(i).Replace("@", "").Split("-")
                    tmpDist = Convert.ToDouble(routeArr(0).Split("#")(0))
                    If tmpDist = 0 Then tmpDist = Me.StrategyDetails(0).MaxDistancePerRoute
                    tmpDrvTime += Convert.ToDouble(routeArr(0).Split("#")(1))
                    'Have to validate that we have a vehicle in the current basket
                    vehType = RoutePlanner.GetVehicle(routeArr(0).Split("#")(4))
                    If vehType Is Nothing Then
                        TotalCost = TotalCost + _DefCostPerDay + _DefCostPerDistUnit * tmpDist + tmpDrvTime * _DefCostPerHour
                    Else
                        TotalCost = TotalCost + vehType.COSTPERDAY + vehType.COSTPERDISTANCEUINIT * tmpDist + tmpDrvTime * vehType.COSTPERHOUR
                    End If
                End If

            Catch ex As Exception
                If Not _logger Is Nothing And Not oGeneticSolver Is Nothing Then
                    _logger.Write("route cost error:" & ex.ToString())
                End If

            End Try
        Next

        ''TotalCost = TotalCost * Me.StrategyDetails(0).RouteCostFactor + chromArr.Length * Me.StrategyDetails(0).NumVehiclesFactor

        If Not _logger Is Nothing And Not oGeneticSolver Is Nothing Then
            _logger.Write("Total Cost=" & TotalCost & "*" & Me.StrategyDetails(0).RouteCostFactor & "+" & chromArr.Length & "*" & Me.StrategyDetails(0).NumVehiclesFactor)
            _logger.writeSeperator("-", 60)
        End If

        Return TotalCost
    End Function


#End Region

#End Region

End Class

#End Region

#Region "Routing Strategy Detail"

<CLSCompliant(False)> Public Class RoutingStrategyDetail

#Region "Variables"

    Protected _strategyid As String
    Protected _priority As Int32
    Protected _depo As String
    Protected _routenameprefix As String
    Protected _numtripsperday As Int32
    Protected _territoryset As String
    Protected _depostarttime As Int32
    Protected _depolateststarttime As Int32

    Protected _continueroutewithreq As Int32

    Protected _targetfillweight As Double
    Protected _targetfillvolume As Double

    Protected _minservicetime As Double
    Protected _maxstopsperroute As Int32
    Protected _maxdistanceperroute As Double
    Protected _maxadditionaldistanceperroute As Double

    Protected _maxtimeperroute As Double
    Protected _maxdistancebetweenstops As Double
    Protected _maxtimebetweenstops As Double
    Protected _calcrettimetodepot As Boolean
    Protected _calcrettimetofirststop As Boolean
    Protected _defaultvehicletype As String
    Protected _defaultvehicletypeobj As VehicleType
    Protected _numvehiclefactor As Double
    Protected _routecostfactor As Double
    Protected _openhourmisspenalty As Double
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _clustersize As Integer
    Protected _minclustersize As Double
    Protected _maxclustersize As Double

    Protected _minclustersizeformerge As Integer


    Protected _servicetimeequation As String
    Protected _routecostequation As String

    Protected _allowsplitorder As Integer
    Protected _allowsplitstop As Integer
    Protected _allowsplitcontact As Integer


    Protected _allowedoverweightforlaststop As Double
    Protected _allowedovervolumeforlaststop As Double

    Protected _minrouteweight As Double
    Protected _minroutevolume As Double

    Protected _territorysetobj As GeoTerritorySet
    Protected _depotObj As Depots


    Protected _allowedtimebeforeopen As Integer

    Protected _allowpickupbeforedelivery As Double ''procent
    Protected _maxdistancebetweendeliveryandpickup As Double

    ''trips
    Protected _depotreloadtime As Integer
    Protected _totaltripsallocationtime As Integer
    Protected _totaltripsallocationdistance As Integer
    Protected _routefarclustersfirst As Integer
    Protected _maximizetripspervehicle As Integer
    Protected _distanceforsplitstop As Double
    Protected _chkpntpositiontype As String


    Public DepoPointNode As GeoPointNode

#End Region

#Region "Properties"

    Public ReadOnly Property StrategyId() As String
        Get
            Return _strategyid
        End Get
    End Property

    Public ReadOnly Property DepotId() As String
        Get
            Return _depo
        End Get
    End Property

    Public ReadOnly Property Depot() As Depots
        Get
            Return _depotObj
        End Get
    End Property

    Public ReadOnly Property RouteNamePrefix() As String
        Get
            Return _routenameprefix
        End Get
    End Property

    Public ReadOnly Property Priority() As Int32
        Get
            Return _priority
        End Get
    End Property

    Public ReadOnly Property NumTripsPerDay() As Int32
        Get
            Return _numtripsperday
        End Get
    End Property

    Public ReadOnly Property TerritorySetID() As String
        Get
            Return _territoryset
        End Get
    End Property

    Public ReadOnly Property TerritorySet() As GeoTerritorySet
        Get
            Return _territorysetobj
        End Get
    End Property

    Public ReadOnly Property DepoStartTime() As Int32
        Get
            Return _depostarttime
        End Get
    End Property

    Public ReadOnly Property DepoLatestStartTime() As Int32
        Get
            Return _depolateststarttime
        End Get
    End Property

    Public ReadOnly Property ContinueRouteWithReq() As Int32
        Get
            Return _continueroutewithreq
        End Get
    End Property


    Public ReadOnly Property MinServiceTime() As Int32
        Get
            Return _minservicetime
        End Get
    End Property

    Public ReadOnly Property TargetFillWeight() As Double
        Get
            Return _targetfillweight
        End Get
    End Property

    Public ReadOnly Property TargetFillVolume() As Double
        Get
            Return _targetfillvolume
        End Get
    End Property

    Public ReadOnly Property MaxStopsPerRoute() As Int32
        Get
            Return _maxstopsperroute
        End Get
    End Property

    Public ReadOnly Property MaxDistancePerRoute() As Double
        Get
            Return _maxdistanceperroute
        End Get
    End Property

    Public ReadOnly Property MaxAdditionalDistancePerRoute() As Double
        Get
            Return _maxadditionaldistanceperroute
        End Get
    End Property


    Public ReadOnly Property MaxTimePerRoute() As Double
        Get
            Return _maxtimeperroute
        End Get
    End Property

    Public ReadOnly Property MaxDistanceBetweenStops() As Double
        Get
            Return _maxdistancebetweenstops
        End Get
    End Property

    Public ReadOnly Property MaxTimeBetweenStops() As Double
        Get
            Return _maxtimebetweenstops
        End Get
    End Property
    Public ReadOnly Property ServicetimeEquation() As String
        Get
            Return _servicetimeequation
        End Get
    End Property
    Public ReadOnly Property RouteCostEquation() As String
        Get
            Return _routecostequation
        End Get
    End Property

    Public ReadOnly Property MaxClusterSize() As Double
        Get
            Return _maxclustersize
        End Get
    End Property

    Public ReadOnly Property MinClusterSizeForMerge() As Double
        Get
            Return _minclustersizeformerge
        End Get
    End Property


    Public ReadOnly Property MinClusterSize() As Double
        Get
            Return _minclustersize
        End Get
    End Property


    Public ReadOnly Property Clustersize() As Integer
        Get
            Return _clustersize
        End Get
    End Property
    Public ReadOnly Property CalcRetTimeToDepot() As Boolean
        Get
            Return _calcrettimetodepot
        End Get
    End Property

    Public ReadOnly Property CalcRetTimeToFirstStop() As Boolean
        Get
            Return _calcrettimetofirststop
        End Get
    End Property

    Public ReadOnly Property DefaultVehicleType() As String
        Get
            Return _defaultvehicletype
        End Get
    End Property

    Public ReadOnly Property DefaultVehicle() As VehicleType
        Get
            Return _defaultvehicletypeobj
        End Get
    End Property

    Public ReadOnly Property NumVehiclesFactor() As Double
        Get
            Return _numvehiclefactor
        End Get
    End Property

    Public ReadOnly Property RouteCostFactor() As Double
        Get
            Return _routecostfactor
        End Get
    End Property

    Public ReadOnly Property OpenHourMissPenalty() As Double
        Get
            Return _openhourmisspenalty
        End Get
    End Property

    Public ReadOnly Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
    End Property

    Public ReadOnly Property AddUser() As String
        Get
            Return _adduser
        End Get
    End Property

    Public ReadOnly Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
    End Property

    Public ReadOnly Property EditUser() As String
        Get
            Return _edituser
        End Get
    End Property
    Public ReadOnly Property AllowSplitOrder() As Integer
        Get
            Return _allowsplitorder
        End Get
    End Property
    Public ReadOnly Property AllowSplitStop() As Integer
        Get
            Return _allowsplitstop
        End Get
    End Property
    Public ReadOnly Property AllowSplitContact() As Integer
        Get
            Return _allowsplitcontact
        End Get
    End Property


    Public ReadOnly Property AllowedTimeBeforeOpen() As Integer
        Get
            Return _allowedtimebeforeopen
        End Get
    End Property
    Public ReadOnly Property AllowedOverWeightforLastStop() As Double
        Get
            Return _allowedoverweightforlaststop
        End Get
    End Property
    Public ReadOnly Property AllowedOverVolumeforLastStop() As Double
        Get
            Return _allowedovervolumeforlaststop
        End Get
    End Property
    Public ReadOnly Property MinRouteWeight() As Double
        Get
            Return _minrouteweight
        End Get
    End Property

    Public ReadOnly Property AllowPickupBeforeDelivery() As Double
        Get
            Return _allowpickupbeforedelivery
        End Get
    End Property

    Public ReadOnly Property MaxdistanceBetweenDeliveryAndPickup() As Double
        Get
            Return _maxdistancebetweendeliveryandpickup
        End Get
    End Property

    Public ReadOnly Property MinRouteVolume() As Double
        Get
            Return _minroutevolume
        End Get
    End Property

    ''trips
    Public ReadOnly Property DepotReloadTime() As Integer
        Get
            Return _depotreloadtime
        End Get
    End Property
    Public ReadOnly Property TotalTripsAllocationTime() As Integer
        Get
            Return _totaltripsallocationtime
        End Get
    End Property
    Public ReadOnly Property TotalTripsAllocationDistance() As Integer
        Get
            Return _totaltripsallocationdistance
        End Get
    End Property

    Public ReadOnly Property RouteFarClustersFirst() As Integer
        Get
            Return _routefarclustersfirst
        End Get
    End Property

    Public ReadOnly Property MaximizeTripsPerVehicle() As Integer
        Get
            Return _maximizetripspervehicle
        End Get
    End Property

    Public ReadOnly Property DistanceForSplitStop() As Integer
        Get
            Return _distanceforsplitstop
        End Get
    End Property

    Public ReadOnly Property ChkPntPositionType() As Integer
        Get
            Return _chkpntpositiontype
        End Get
    End Property



#End Region

#Region "Constructor"

    Public Sub New(ByVal dr As DataRow)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        _depo = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DEPO"), "")
        _routenameprefix = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTENAMEPREFIX"), "")
        If Not IsDBNull(dr("NUMTRIPSPERDAY")) Then _numtripsperday = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NUMTRIPSPERDAY"))

        ''_territoryset = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TERRITORYSET"), "")
        _territoryset = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TERRITORYSET"))

        If Not IsDBNull(dr("STARTTIMEATDEPO")) Then _depostarttime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STARTTIMEATDEPO"))
        If Not IsDBNull(dr("LATESTTIMESTARTDEPOT")) Then _depolateststarttime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LATESTTIMESTARTDEPOT"))

        If Not IsDBNull(dr("CONTINUEROUTEWITHREQ")) Then _continueroutewithreq = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTINUEROUTEWITHREQ"))


        If Not IsDBNull(dr("TARGETFILLWEIGHT")) Then _targetfillweight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TARGETFILLWEIGHT"))
        If Not IsDBNull(dr("TARGETFILLVOLUME")) Then _targetfillvolume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TARGETFILLVOLUME"))


        If Not IsDBNull(dr("MINSERVICETIME")) Then _minservicetime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MINSERVICETIME"))
        If Not IsDBNull(dr("MAXSTOPSPERROUTE")) Then _maxstopsperroute = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXSTOPSPERROUTE"))
        If Not IsDBNull(dr("MAXDISTANCEPERROUTE")) Then _maxdistanceperroute = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXDISTANCEPERROUTE"))
        If Not IsDBNull(dr("MAXADDITIONALDISTANCEPERROUTE")) Then _maxadditionaldistanceperroute = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXADDITIONALDISTANCEPERROUTE"))


        If Not IsDBNull(dr("MAXTIMEPERROUTE")) Then _maxtimeperroute = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXTIMEPERROUTE"))
        If Not IsDBNull(dr("MAXDISTANCEBETWEENSTOPS")) Then _maxdistancebetweenstops = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXDISTANCEBETWEENSTOPS"))
        If Not IsDBNull(dr("MAXTIMEBETWEENSTOPS")) Then _maxtimebetweenstops = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("MAXTIMEBETWEENSTOPS"))
        If Not IsDBNull(dr("CALCRETFROMLASTSTOP")) Then _calcrettimetodepot = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CALCRETFROMLASTSTOP"))
        If Not IsDBNull(dr("CALCRETTOFIRSTSTOP")) Then _calcrettimetofirststop = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CALCRETTOFIRSTSTOP"))
        If Not IsDBNull(dr("OPENHOURSMISSPENALTY")) Then _openhourmisspenalty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("OPENHOURSMISSPENALTY"))
        If Not IsDBNull(dr("ROUTECOSTFACTOR")) Then _routecostfactor = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTECOSTFACTOR"))
        If Not IsDBNull(dr("NUMVEHICLEFACTOR")) Then _numvehiclefactor = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NUMVEHICLEFACTOR"))
        If Not IsDBNull(dr("DEFAULTVEHICLETYPE")) Then _defaultvehicletype = dr("DEFAULTVEHICLETYPE")
        If Not IsDBNull(dr("CLUSTERSIZE")) Then _clustersize = dr("CLUSTERSIZE")

        If Not IsDBNull(dr("MINCLUSTERSIZE")) Then _minclustersize = dr("MINCLUSTERSIZE")
        If Not IsDBNull(dr("MAXCLUSTERSIZE")) Then _maxclustersize = dr("MAXCLUSTERSIZE")

        If Not IsDBNull(dr("MINCLUSTERSIZEFORMERGE")) Then _minclustersizeformerge = dr("MINCLUSTERSIZEFORMERGE")

        If Not IsDBNull(dr("SERVICETIMEEQUATION")) Then _servicetimeequation = dr("SERVICETIMEEQUATION")
        If Not IsDBNull(dr("ROUTECOSTEQUATION")) Then _routecostequation = dr("ROUTECOSTEQUATION")

        _allowsplitorder = ReplaceDBNull(dr("ALLOWSPLITORDER"))
        _allowsplitstop = ReplaceDBNull(dr("ALLOWSPLITSTOP"))
        _allowsplitcontact = ReplaceDBNull(dr("ALLOWSPLITCONTACT"))


        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))

        _allowedtimebeforeopen = ReplaceDBNull(dr("ALLOWEDTIMEBEFOREOPEN"))

        _allowedoverweightforlaststop = ReplaceDBNull(dr("ALLOWEDOVERWEIGHTFORLASTSTOP"))
        _allowedovervolumeforlaststop = ReplaceDBNull(dr("ALLOWEDOVERVOLUMEFORLASTSTOP"))

        _minrouteweight = ReplaceDBNull(dr("MINROUTEWEIGHT"))
        _minroutevolume = ReplaceDBNull(dr("MINROUTEVOLUME"))

        _allowpickupbeforedelivery = ReplaceDBNull(dr("ALLOWPICKUPBEFOREDELIVERY"))
        _maxdistancebetweendeliveryandpickup = ReplaceDBNull(dr("MAXDISTANCEBETWEENDELIVERYANDPICKUP"))

        _depotreloadtime = ReplaceDBNull(dr("DepotReloadTime"))
        _totaltripsallocationtime = ReplaceDBNull(dr("TotalTripsAllocationTime"))
        _totaltripsallocationdistance = ReplaceDBNull(dr("TotalTripsAllocationDistance"))

        _routefarclustersfirst = ReplaceDBNull(dr("ROUTEFARCLUSTERSFIRST"))
        _maximizetripspervehicle = ReplaceDBNull(dr("MAXIMIZETRIPSPERVEHICLE"))

        _distanceforsplitstop = ReplaceDBNull(dr("DistanceForSplitStop"))
        _chkpntpositiontype = ReplaceDBNull(dr("CHKPNTPOSITIONTYPE"))


        'Convert the Max Distance Per Route to meters,Max Driving Time Per Route to seconds
        '_maxdistanceperroute = _maxdistanceperroute * 1000
        '_maxdistancebetweenstops = _maxdistancebetweenstops * 1000
        '_maxtimebetweenstops = _maxtimebetweenstops * 3600
        '_maxtimeperroute = _maxtimeperroute * 3600

        'try to load the territory set object
        '_territorysetobj = New GeoTerritorySet(_territoryset)
        'try to load the Depot object
        If _depo <> "" And Depots.Exists(_depo) Then
            _depotObj = New Depots(_depo)
        End If
        If WMS.Logic.VehicleType.Exists(_defaultvehicletype) Then
            If _defaultvehicletype <> String.Empty Then
                _defaultvehicletypeobj = New VehicleType(_defaultvehicletype)
            Else
                _defaultvehicletypeobj = Nothing
            End If
        Else
            _defaultvehicletypeobj = Nothing
        End If

        DepoPointNode = New GeoPointNode(_depotObj.POINTID)
    End Sub

#End Region

#Region "Enums"

    '    Protected Function RouteCostParamToString(ByVal rc As RouteCostParam) As String
    '        Select Case rc
    '            Case RouteCostParam.Daily
    '                Return "DAILY"
    '            Case RouteCostParam.Distance
    '                Return "DISTANCE"
    '            Case RouteCostParam.Time
    '                Return "HOUR"
    '        End Select
    '    End Function

    '    Protected Function RouteCostParamFromString(ByVal sRouteCostParam As String) As RouteCostParam
    '        Select Case sRouteCostParam.ToUpper
    '            Case "DAILY"
    '                Return RouteCostParam.Daily
    '            Case "DISTANCE"
    '                Return RouteCostParam.Distance
    '            Case "HOUR"
    '                Return RouteCostParam.Time
    '        End Select
    '    End Function

    '    Public Enum RouteCostParam
    '        Daily
    '        Time
    '        Distance
    '    End Enum

#End Region

End Class

#End Region

#Region "Routing Strategy Detail Collection"

<CLSCompliant(False)> Public Class RoutingStrategyDetailCollection
    Implements ICollection

#Region "Variables"

    Protected _stratlines As ArrayList
    Protected _routingstrategyid As String

#End Region

#Region "Constructor"

    Public Sub New(ByVal RoutingStrategyId As String)
        _stratlines = New ArrayList
        _routingstrategyid = RoutingStrategyId
        Load()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As RoutingStrategyDetail
        Get
            Return CType(_stratlines(index), RoutingStrategyDetail)
        End Get
        Set(ByVal Value As RoutingStrategyDetail)
            _stratlines(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from routingstrategydetail Where strategyid='{0}' order by priority asc", _routingstrategyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim stDetail As RoutingStrategyDetail
        For Each dr In dt.Rows
            stDetail = New RoutingStrategyDetail(dr)
            Add(stDetail)
        Next

    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As RoutingStrategyDetail) As Integer
        Return _stratlines.Add(value)
    End Function

    Public Sub Clear()
        _stratlines.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As RoutingStrategyDetail)
        _stratlines.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As RoutingStrategyDetail)
        _stratlines.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _stratlines.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _stratlines.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _stratlines.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _stratlines.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _stratlines.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _stratlines.GetEnumerator()
    End Function

#End Region

End Class

#End Region

#Region "Routing Vehicle Allocation"

<CLSCompliant(False)> Public Class RoutingVehicleAllocation

#Region "Variables"

    Protected _strategyid As String
    Protected _priority As Int32
    Protected _vehicletype As String
    Protected _vehicletypeobj As VehicleType
    Protected _numvehicles As Int32
    Protected _availableVehicles As Int32
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"

    Public ReadOnly Property StrategyId() As String
        Get
            Return _strategyid
        End Get
    End Property

    Public ReadOnly Property Priority() As Int32
        Get
            Return _priority
        End Get
    End Property

    Public ReadOnly Property VehicleType() As String
        Get
            Return _vehicletype
        End Get
    End Property

    Public ReadOnly Property VehicleTypeObj() As VehicleType
        Get
            Return _vehicletypeobj
        End Get
    End Property

    Public ReadOnly Property NumVehicles() As Int32
        Get
            Return _numvehicles
        End Get
    End Property

    Public Property AvailableVehicles() As Int32
        Get
            Return _availableVehicles
        End Get
        Set(ByVal Value As Int32)
            _availableVehicles = Value
        End Set
    End Property

    Public ReadOnly Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
    End Property

    Public ReadOnly Property AddUser() As String
        Get
            Return _adduser
        End Get
    End Property

    Public ReadOnly Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
    End Property

    Public ReadOnly Property EditUser() As String
        Get
            Return _edituser
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal dr As DataRow)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        _vehicletype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLETYPE"), "")
        If Not IsDBNull(dr("NUMVEHICLES")) Then _numvehicles = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NUMVEHICLES"))
        _availableVehicles = NumVehicles
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
        'Load the current vehicle type params
        _vehicletypeobj = New VehicleType(_vehicletype)
    End Sub

#End Region

End Class

#End Region

#Region "Routing Vehicle Allocation Collection"

<CLSCompliant(False)> Public Class RoutingVehicleAllocationCollection
    Implements ICollection

#Region "Variables"

    Protected _stratlines As ArrayList
    Protected _routingstrategyid As String

#End Region

#Region "Constructor"

    Public Sub New(ByVal RoutingStrategyId As String)
        _stratlines = New ArrayList
        _routingstrategyid = RoutingStrategyId
        Load()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As RoutingVehicleAllocation
        Get
            Return CType(_stratlines(index), RoutingVehicleAllocation)
        End Get
        Set(ByVal Value As RoutingVehicleAllocation)
            _stratlines(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from ROUTINGPOLICYVEHICLEALLOCATION Where strategyid='{0}' order by priority asc", _routingstrategyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim stDetail As RoutingVehicleAllocation
        For Each dr In dt.Rows
            stDetail = New RoutingVehicleAllocation(dr)
            Add(stDetail)
        Next
    End Sub

    'This function returns the next available vehicle for use in the routing proccess
    Public Function GetNextAvailableVehicle(ByVal isupdate As Boolean) As VehicleType
        For Each tempVehicleAlloc As RoutingVehicleAllocation In Me
            If tempVehicleAlloc.AvailableVehicles > 0 Then
                If isupdate Then
                    tempVehicleAlloc.AvailableVehicles = tempVehicleAlloc.AvailableVehicles - 1
                End If
                tempVehicleAlloc.VehicleTypeObj.VehiclePriorityinPool = tempVehicleAlloc.Priority
                Return tempVehicleAlloc.VehicleTypeObj
            End If
        Next
        Return Nothing
    End Function

    Public Sub RestoreAvailableVehicle(ByVal pVEHICLETYPEID As String)
        For Each tempVehicleAlloc As RoutingVehicleAllocation In Me
            If tempVehicleAlloc.VehicleTypeObj.VEHICLETYPEID = pVEHICLETYPEID Then
                tempVehicleAlloc.AvailableVehicles += 1
                Exit Sub
            End If
        Next

    End Sub


    Public Function GetNumAllAvailableVehicle() As Integer
        Dim cnt As Integer = 0
        For Each tempVehicleAlloc As RoutingVehicleAllocation In Me
            cnt += tempVehicleAlloc.AvailableVehicles
        Next
        Return cnt
    End Function


    Public Sub InitAvailableVehicle()
        Dim tempVehicleAlloc As RoutingVehicleAllocation
        For Each tempVehicleAlloc In Me
            tempVehicleAlloc.AvailableVehicles = tempVehicleAlloc.NumVehicles
        Next
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As RoutingVehicleAllocation) As Integer
        Return _stratlines.Add(value)
    End Function

    Public Sub Clear()
        _stratlines.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As RoutingVehicleAllocation)
        _stratlines.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As RoutingVehicleAllocation)
        _stratlines.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _stratlines.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _stratlines.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _stratlines.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _stratlines.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _stratlines.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _stratlines.GetEnumerator()
    End Function

#End Region

End Class

#End Region

