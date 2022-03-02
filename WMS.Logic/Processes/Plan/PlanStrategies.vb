Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Algorithms.Scoring
Imports Made4Net.Algorithms.SortingAlgorithms

#Region "Plan Strategies"

<CLSCompliant(False)> Public Class PlanStrategies
    Implements ICollection

#Region "Variables"

    Protected _stratcol As ArrayList

    Protected Shared DetailsDataTable As DataTable = Nothing

#End Region

#Region "Constructor"

    Public Sub New(Optional ByVal oLogger As LogHandler = Nothing)
        _stratcol = New ArrayList
        Load(oLogger)
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As PlanStrategy
        Get
            Return CType(_stratcol(index), PlanStrategy)
        End Get
        Set(ByVal Value As PlanStrategy)
            _stratcol(index) = Value
        End Set
    End Property

    Default Public ReadOnly Property Item(ByVal StrategyName As String) As PlanStrategy
        Get
            Dim strat As PlanStrategy
            For Each strat In Me
                If strat.StrategyId.ToLower = StrategyName.ToLower Then
                    Return CType(strat, PlanStrategy)
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public Shared ReadOnly Property Details() As PlanStrategyDetailDataTable
        Get
            DetailsDataTable = New PlanStrategyDetailDataTable
            Dim sSql As String = "SELECT STRATEGYID, PRIORITY, PICKTYPE, ISNULL(PICKREGION,'%') AS PICKREGION, ISNULL(UOM,'%') AS UOM, ISNULL(LOCPICKTYPE,'%') AS LOCPICKTYPE FROM PLANSTRATEGYDETAIL"
            DataInterface.FillDataset(sSql, DetailsDataTable)
            Return DetailsDataTable
        End Get
    End Property

#End Region

#Region "Methods"


    'RWMS-RWMS-1279
    Public Sub ExportToLog(ByVal oLogger As LogHandler)
        If Not oLogger Is Nothing Then
            Try
                oLogger.Write("Exporting PlanStrategies collection")
                Dim lgstratobj As PlanStrategy
                For Each lgstratobj In Me
                    lgstratobj.ExportToLog(oLogger)
                Next
            Catch ex As Exception
                oLogger.Write(ex.ToString())
            End Try
        End If

    End Sub

    Protected Sub Load(Optional ByVal oLogger As LogHandler = Nothing)

        Dim Sql As String = String.Format("SELECT * FROM PLANSTRATEGYHEADER")

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim st As PlanStrategy
        For Each dr In dt.Rows
            st = New PlanStrategy(dr)
            Add(st)
        Next
    End Sub

    Public Sub Plan(ByVal Stock As VPlannerInventoryDataTable, ByVal OnHandStock As VAllocationOnHandDataTable, Optional ByVal oLogger As LogHandler = Nothing)
        Dim strat As PlanStrategy
        For Each strat In Me
            strat.Plan(Stock, OnHandStock, oLogger)
        Next
    End Sub

    Public Sub CreatePickList(Optional ByVal oLogger As LogHandler = Nothing)
        Dim strat As PlanStrategy
        For Each strat In Me
            If strat.ShouldCreateLoadingPlan Then
                strat.CreateLoadingPlan(oLogger)
            Else
                strat.CreatePickList(oLogger)
            End If
        Next
    End Sub

    Public Sub ReleasePickLists(Optional ByVal oLogger As LogHandler = Nothing)
        Dim strat As PlanStrategy
        For Each strat In Me
            strat.ReleasePickList(oLogger)
        Next
    End Sub
    ''Start RWMS-1279
    Public Function ShouldRelease(Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim strat As PlanStrategy
        Try
            For Each strat In Me
                oLogger.Write("Strategy retrieved before calling ShouldReleasePickList() : " & strat.StrategyId.ToString())
                If strat.ShouldReleasePickList(oLogger) Then
                    Return True
                End If
            Next
        Catch ex As Exception
            oLogger.Write("Exception caught in ShouldRelease() : " & ex.Message.ToString())
        End Try

        Return False
    End Function
    ''End RWMS-1279
#End Region

#Region "Overrides"

    Public Function Add(ByVal value As PlanStrategy) As Integer
        Return _stratcol.Add(value)
    End Function

    Public Sub Clear()
        _stratcol.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As PlanStrategy)
        _stratcol.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As PlanStrategy)
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

#Region "Plan Strategy"

<CLSCompliant(False)> Public Class PlanStrategy

#Region "Variables"

    Protected _strategyid As String
    Protected _description As String
    Protected _pickmethod As String
    Protected _fullalocpicklist As Boolean
    Protected _createloadingplan As Boolean
    Protected _pickpartialuom As Boolean
    Protected _picklistbasecube As Decimal
    Protected _picklistbaseweight As Decimal
    Protected _substituteskumode As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    Protected _stratdetail As PlanStrategyDetailCollection
    Protected _reqbaskets As RequirmentsBaskets
    Protected _stratbreaker As PicklistBreakerCollection
    Protected _stratrelease As ReleaseStrategyCollection
    Protected _parallelstratrelease As ParallelReleaseStrategyCollection
    Protected _scoring As PlanStrategyScoring
    Protected _picklistcollection As ArrayList
    'RWMS-2604
    Protected _caseperhour As Int32

#End Region

#Region "Properties"

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

    Public ReadOnly Property PickMethod() As String
        Get
            Return _pickmethod
        End Get
    End Property

    Public ReadOnly Property CreatePickListForFullyAllocated() As Boolean
        Get
            Return _fullalocpicklist
        End Get
    End Property

    Public ReadOnly Property PickPartialUom() As Boolean
        Get
            Return _pickpartialuom
        End Get
    End Property

    Public ReadOnly Property ShouldCreateLoadingPlan() As Boolean
        Get
            Return _createloadingplan
        End Get
    End Property

    Public ReadOnly Property PickListBaseCube() As Decimal
        Get
            Return _picklistbasecube
        End Get
    End Property

    Public ReadOnly Property PickListBaseWeight() As Decimal
        Get
            Return _picklistbaseweight
        End Get
    End Property

    Public ReadOnly Property SubstituteSkuMode() As String
        Get
            Return _substituteskumode
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

    Public ReadOnly Property PlanDetails() As PlanStrategyDetailCollection
        Get
            Return _stratdetail
        End Get
    End Property

    Public ReadOnly Property ReleaseStrategyDetails() As ReleaseStrategyCollection
        Get
            Return _stratrelease
        End Get
    End Property

    Public ReadOnly Property ParallelReleaseStrategyDetails() As ParallelReleaseStrategyCollection
        Get
            Return _parallelstratrelease
        End Get
    End Property

    Public ReadOnly Property ShouldPrintPickLabelOnPicking(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKSTART Then 'Or releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKDETAILCOMPLETE Then
                        If releasestrat.LabelFormat <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                            Return True
                        End If
                    End If
                    Return False
                    Exit For
                End If
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property ShouldPrintBagOutReportOnStart(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.BagOutPrintOption = WMS.Lib.Release.BAGOUTPRINTOPTION.ONPCKSTART Then
                        If releasestrat.BagOutReportName <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                            Return True
                        End If
                    End If
                    Return False
                    Exit For
                End If
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property ShouldPrintBagOutReportOnComplete(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.BagOutPrintOption = WMS.Lib.Release.BAGOUTPRINTOPTION.ONPCKCOMPLETE Then
                        If releasestrat.BagOutReportName <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                            Return True
                        End If
                    End If
                    Return False
                    Exit For
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property ShouldPrintPickLabelOnPickLineCompleted(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKDETAILCOMPLETE Then
                        If releasestrat.LabelFormat <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                            Return True
                        End If
                    End If
                    Return False
                    Exit For
                End If
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property ShouldPrintCaseLabel(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then

                    If releasestrat.LabelFormat <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                        Return releasestrat.CaseLabelPrintOption
                    End If
                    Return False
                    Exit For
                End If
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property ShouldPrintShipLabel(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType.ToLower = pcklst.PickType.ToLower Or releasestrat.PickType = "") Then
                    If releasestrat.ShipLabelFormat <> "" Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property ShipLabelFormat(ByVal pcklst As Picklist) As String
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType.ToLower = pcklst.PickType.ToLower Or releasestrat.PickType = "") Then
                    Return releasestrat.ShipLabelFormat
                End If
            Next
            Return ""
        End Get
    End Property

    Public ReadOnly Property ShouldPrintContentList(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType.ToLower = pcklst.PickType.ToLower Or releasestrat.PickType = "") Then
                    If releasestrat.ContentListDocName <> "" Then
                        Return releasestrat.PrintContentList
                    End If
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property ContentListDocumentName(ByVal pcklst As Picklist) As String
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType.ToLower = pcklst.PickType.ToLower Or releasestrat.PickType = "") Then
                    Return releasestrat.ContentListDocName
                End If
            Next
            Return ""
        End Get
    End Property
    Public ReadOnly Property GetBagOutDocumentName(ByVal pcklst As Picklist) As String
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType.ToLower = pcklst.PickType.ToLower Or releasestrat.PickType = "") And (releasestrat.PrintBagOutReport) Then
                    Return releasestrat.BagOutReportName
                End If
            Next
            Return ""
        End Get
    End Property
    Public ReadOnly Property GetCaseLabelName(ByVal pcklst As Picklist) As String
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType.ToLower = pcklst.PickType.ToLower Or releasestrat.PickType = "") And (releasestrat.CaseLabelPrintOption) Then
                    Return releasestrat.CaseLabelFormat
                End If
            Next
            Return ""
        End Get
    End Property

    <Obsolete("This function is not in use. Please use the PickLabelFormat function or ShouldPrintPickLabel instead.", True)> _
    Public ReadOnly Property PickLabelType(ByVal pcklst As Picklist) As String
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKSTART Or releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONRELEASE Or releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKDETAILCOMPLETE Then
                        If releasestrat.LabelFormat <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                            Return releasestrat.LabelFormat
                        End If
                    End If
                    Return Nothing
                    Exit For
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property PickLabelFormat(ByVal pcklst As Picklist) As String
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.LabelFormat <> WMS.Lib.Release.PICKLABELTYPE.NONE Then
                        Return releasestrat.LabelFormat
                    End If
                    Return Nothing
                    Exit For
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property ShouldPrintPickLabel(ByVal pcklst As Picklist) As Boolean
        Get
            Dim releasestrat As ReleaseStrategyDetail
            For Each releasestrat In Me.ReleaseStrategyDetails
                If (releasestrat.PickType = pcklst.PickType Or releasestrat.PickType = "") Then
                    If releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKSTART Or releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONRELEASE Or releasestrat.AutoPrintPickLabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONPCKDETAILCOMPLETE Then
                        If releasestrat.LabelFormat <> "" Then
                            Return True
                        End If

                    End If
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property Scoring() As PlanStrategyScoring
        Get
            Return _scoring
        End Get
    End Property

    ''' <summary>
    ''' RWMS-2604
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property CASEPERHOUR() As Int32
        Get
            Return _caseperhour
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal planStrategyId As String)
        _strategyid = planStrategyId
        Dim sql As String = String.Format("SELECT * FROM PLANSTRATEGYHEADER WHERE STRATEGYID = '{0}'", _strategyid)
        Dim dt As New Data(sql)
        Dim dr As DataRow = dt.CreateDataRow()
        Load(dr)
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    'RWMS-439
    Public Sub ExportToLog(ByVal oLogger As LogHandler)
        If Not oLogger Is Nothing Then
            oLogger.Write("Strategy : " + _strategyid + " PickBasketCount " + _reqbaskets.Count.ToString + " PickMethod " + _reqbaskets.PickMethod + " FullAllocPickList " + _fullalocpicklist.ToString())
        End If
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _description = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DESCRIPTION"))
        _pickmethod = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKMETHOD"))
        _fullalocpicklist = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PCKSFORFULLALOC"))
        _createloadingplan = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CREATELOADINGPLAN"))
        If IsDBNull(dr("PICKPARTIALUOM")) Then _pickpartialuom = False Else _pickpartialuom = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKPARTIALUOM"))
        If IsDBNull(dr("PICKLISTBASECUBE")) Then _picklistbasecube = 0 Else _picklistbasecube = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKLISTBASECUBE"))
        If IsDBNull(dr("PICKLISTBASEWEIGHT")) Then _picklistbaseweight = 0 Else _picklistbaseweight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKLISTBASEWEIGHT"))
        If IsDBNull(dr("SUBSTITUTESKUMODE")) Then _substituteskumode = WMS.Lib.Plan.SUBSTITUTESKUMODE.NEVER Else _substituteskumode = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SUBSTITUTESKUMODE"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
        'RWMS-2604
        _caseperhour = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CASESPERHOUR"))

        _stratdetail = New PlanStrategyDetailCollection(_strategyid)
        _stratbreaker = New PicklistBreakerCollection(_strategyid)
        _stratrelease = New ReleaseStrategyCollection(_strategyid)
        _scoring = New PlanStrategyScoring(_strategyid)
        'If _pickmethod = WMS.Lib.PickMethods.PickMethod.PARALELORDERPICKING Then
        _parallelstratrelease = New ParallelReleaseStrategyCollection(_strategyid)
        'End If
        _reqbaskets = New RequirmentsBaskets(_pickmethod)
        _picklistcollection = New ArrayList
    End Sub

    Public Sub addRequirment(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing)
        _reqbaskets.Place(req, oLogger)
    End Sub

    Public Sub Plan(ByVal Stock As VPlannerInventoryDataTable, ByVal OnHandStock As VAllocationOnHandDataTable, Optional ByVal oLogger As LogHandler = Nothing)
        Dim reqsbaskets As PickBaskets
        Dim stratline As PlanStrategyDetail
        Dim pStrategyAllocationSequence As Int32 = 0
        For Each reqsbaskets In _reqbaskets
            reqsbaskets.Allocate(Stock, OnHandStock, Me, pStrategyAllocationSequence, oLogger)
        Next
    End Sub

    Public Sub CreatePickList(Optional ByVal oLogger As LogHandler = Nothing)
        Dim bskt As PickBaskets
        Dim pck As PicksObject
        Dim pcks As PicksObjectCollection
        Dim pcksbr As PicksObjectCollection
        Dim pcksbreaked As ArrayList
        Dim pickListId As String

        '-------------------------------------------------------
        For Each bskt In _reqbaskets
            pcks = bskt.CreatePickLists(_strategyid, oLogger)

            'RWMS-439
            If (Not pcks Is Nothing) Then
                pcks.ExportToLog(oLogger)
            End If


            If Not oLogger Is Nothing Then

                Try
                    oLogger.writeSeperator(" ", 20)
                    oLogger.Write(" PlanStrategy.CreatePickList - Picklist Breakdowns") ''RWMS-1485
                    oLogger.writeSeperator("-", 20)
                    oLogger.writeSeperator(" ", 20)
                    oLogger.Write("Load".PadRight(22) & "|" & "Location".PadRight(10) & "|" & "SKU".PadRight(20) & "|" & "Pick qty".PadRight(10))
                    oLogger.writeSeperator("-", 60)
                Catch ex As Exception
                End Try
            End If

            Try
                If Not pcks Is Nothing Then
                    For i As Integer = 0 To pcks.Count - 1
                        If Not oLogger Is Nothing Then
                            oLogger.Write(pcks.Item(i).LoadId.PadRight(22) & "|" & pcks.Item(i).FromLocation.PadRight(10) & "|" & pcks.Item(i).SKU.PadRight(20) & "|" & pcks.Item(i).Units.ToString.PadRight(10))
                        End If
                        ' SOFT Allocation is turned on , dont create picklists only update order details
                        If Planner.SOFTPLAN Then
                            Dim ordetail As New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(pcks.Item(i).Consignee, pcks.Item(i).OrderId, pcks.Item(i).OrderLine)
                            ordetail.SoftAllocate(pcks.Item(i).Units, WMS.Lib.USERS.SYSTEMUSER)
                        End If
                    Next
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write(" PlanStrategy.CreatePickList - NO PICKLISTS CREATED FOR THIS REQUIREMENT.") ''RWMS-1485
                    End If
                End If
            Catch ex As Exception
            End Try

            If Not oLogger Is Nothing Then
                oLogger.writeSeperator(" ", 20)
            End If

            'Check if the SoftPlaning flag is on , if it is not turned on then create picklists
            If Not Planner.SOFTPLAN Then

                If Not oLogger Is Nothing Then
                    oLogger.Write(" PlanStrategy.CreatePickList - This is not a SOFTPLAN") 'RWMS-1485
                    oLogger.writeSeperator("-", 23)
                End If

                'Added by udi - checking if the picklist should be created
                If _fullalocpicklist Then

                    If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - This is full allocation picklist") ''RWMS-1485

                    If bskt.IsFullyAllocated Then
                        If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - The basket is fully allocated") ''RWMS-1485
                        If Not pcks Is Nothing Then
                            'Start RWMS-768
                            If Not oLogger Is Nothing Then
                                Try
                                    oLogger.writeSeperator(" ", 20)
                                    oLogger.Write(" PlanStrategy.CreatePickList - Fullalocpicklist - Picklist BaseCube:" & _picklistbasecube.ToString() & " PickListBaseWeight:" & _picklistbaseweight.ToString()) ''RWMS-1485
                                Catch ex As Exception
                                End Try

                            End If
                            'End  RWMS-768
                            pcks.Sort(_picklistbasecube, _picklistbaseweight)
                            pcksbreaked = _stratbreaker.BreakFull(pcks, oLogger)
                            If Not pcksbreaked Is Nothing Then
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Posting all picks for BreakFull strategy.... : " + pcksbreaked.Count.ToString()) ''RWMS-1485
                                For Each pcksbr In pcksbreaked
                                    pickListId = pcksbr.Post()
                                    'Start RWMS-768
                                    If Not oLogger Is Nothing Then
                                        oLogger.Write(" PlanStrategy.CreatePickList - Fullalocpicklist - BreakFull pickListId created:" & pickListId) ''RWMS-1485
                                    End If
                                    'End  RWMS-768
                                    _picklistcollection.Add(pickListId)
                                Next
                            End If
                            pcksbreaked = _stratbreaker.BreakNPP(pcks, oLogger)
                            If Not pcksbreaked Is Nothing Then
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Posting all picks for BreakNPP strategy.... : " + pcksbreaked.Count.ToString()) ''RWMS-1485
                                For Each pcksbr In pcksbreaked
                                    pickListId = pcksbr.Post()
                                    'Start RWMS-768
                                    If Not oLogger Is Nothing Then
                                        oLogger.Write(" PlanStrategy.CreatePickList - BreakNPP pickListId created:" & pickListId) ''RWMS-1485
                                    End If
                                    'End  RWMS-768
                                    _picklistcollection.Add(pickListId)
                                Next
                            End If
                            pcksbreaked = _stratbreaker.BreakParallel(pcks, _pickpartialuom, _picklistbasecube, _picklistbaseweight, False, oLogger)
                            If Not pcksbreaked Is Nothing Then
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Posting all picks for BreakParallel strategy.... : " + pcksbreaked.Count.ToString()) ''RWMS-1485
                                For Each pcksbr In pcksbreaked
                                    pickListId = pcksbr.Post()
                                    _picklistcollection.Add(pickListId)
                                Next
                            End If
                            pcksbreaked = _stratbreaker.BreakPartial(pcks, _pickpartialuom, _picklistbasecube, _picklistbaseweight, False, oLogger)
                            If Not pcksbreaked Is Nothing Then
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Posting all picks for BreakPartial strategy.... : " + pcksbreaked.Count.ToString()) ''RWMS-1485
                                For Each pcksbr In pcksbreaked
                                    'RWMS-439
                                    Try
                                        pcksbr.ExportToLog(oLogger)
                                        pickListId = pcksbr.Post()
                                    Catch ex As Exception
                                        If Not oLogger Is Nothing Then oLogger.Write("Exception raised for statement  pcksbr.Post(): " + ex.Message.ToString()) ''RWMS-1485
                                    End Try

                                    If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Picklist created: " + pickListId.ToString()) ''RWMS-1485
                                    _picklistcollection.Add(pickListId)
                                Next
                            End If
                            If pcks.Count > 0 Then
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Posting all remaining picks .... : " + pcks.Count.ToString()) ''RWMS-1485
                                Try
                                    pcks.ExportToLog(oLogger)
                                    pickListId = pcks.Post()
                                Catch ex As Exception
                                    If Not oLogger Is Nothing Then oLogger.Write("Exception raised for statement  pcks.Post(): " + ex.Message.ToString()) ''RWMS-1485
                                End Try
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - Picklist created: " + pickListId.ToString()) ''RWMS-1485
                                _picklistcollection.Add(pickListId)
                            End If
                        End If
                    End If
                Else
                    If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - This is not a full allocation picklist") 'RWMS-1485
                    'this is the original code section - create picklist anyway
                    If Not pcks Is Nothing Then
                        'Start RWMS-768
                        If Not oLogger Is Nothing Then
                            Try
                                oLogger.writeSeperator(" ", 20)
                                oLogger.Write(" PlanStrategy.CreatePickList - Picklist BaseCube: " & _picklistbasecube.ToString() & " PickListBaseWeight: " & _picklistbaseweight.ToString()) 'RWMS-1485
                            Catch ex As Exception
                            End Try

                        End If
                        'End  RWMS-768
                        pcks.Sort(_picklistbasecube, _picklistbaseweight)
                        pcksbreaked = _stratbreaker.BreakFull(pcks, oLogger)
                        If Not pcksbreaked Is Nothing Then
                            If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Posting all picks for BreakFull strategy.... : " + pcksbreaked.Count.ToString()) 'RWMS-1485
                            For Each pcksbr In pcksbreaked
                                pickListId = pcksbr.Post()
                                'Start RWMS-768
                                If Not oLogger Is Nothing Then
                                    oLogger.Write(" PlanStrategy.CreatePickList - >BreakFull pickListId created: " & pickListId) 'RWMS-1485
                                End If
                                'End  RWMS-768
                                _picklistcollection.Add(pickListId)
                            Next
                        End If
                        pcksbreaked = _stratbreaker.BreakNPP(pcks, oLogger)
                        If Not pcksbreaked Is Nothing Then
                            If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Posting all picks for BreakNPP strategy.... : " + pcksbreaked.Count.ToString()) 'RWMS-1485
                            For Each pcksbr In pcksbreaked
                                pickListId = pcksbr.Post()
                                'Start RWMS-768
                                If Not oLogger Is Nothing Then
                                    oLogger.Write(" PlanStrategy.CreatePickList - >BreakNPP pickListId created:" & pickListId) 'RWMS-1485
                                End If
                                'End  RWMS-768
                                _picklistcollection.Add(pickListId)
                            Next
                        End If
                        pcksbreaked = _stratbreaker.BreakParallel(pcks, _pickpartialuom, _picklistbasecube, _picklistbaseweight, False, oLogger)
                        If Not pcksbreaked Is Nothing Then
                            If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Posting all picks for BreakParallel strategy.... : " + pcksbreaked.Count.ToString()) 'RWMS-1485
                            For Each pcksbr In pcksbreaked
                                pickListId = pcksbr.Post()
                                _picklistcollection.Add(pickListId)
                            Next
                        End If
                        pcksbreaked = _stratbreaker.BreakPartial(pcks, _pickpartialuom, _picklistbasecube, _picklistbaseweight, False, oLogger)
                        If Not pcksbreaked Is Nothing Then
                            If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Posting all picks for BreakPartial strategy.... : " + pcksbreaked.Count.ToString()) ''RWMS-1485
                            For Each pcksbr In pcksbreaked

                                If Not oLogger Is Nothing Then oLogger.Write("Before Calling the ExportToLog() .... : " + pcksbr.Count.ToString())
                                Try
                                    pcksbr.ExportToLog(oLogger)
                                    pickListId = pcksbr.Post()
                                Catch ex As Exception
                                    If Not oLogger Is Nothing Then oLogger.Write("Exception raised for statement  pcksbr.Post(): " + ex.Message.ToString()) ''RWMS-1485
                                End Try
                                If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Picklist created: " + pickListId.ToString()) 'RWMS-1485
                                _picklistcollection.Add(pickListId)
                            Next
                        End If
                        If pcks.Count > 0 Then
                            If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Posting all remaining picks .... : " + pcks.Count.ToString()) 'RWMS-1485
                            Try
                                pcks.ExportToLog(oLogger)
                                pickListId = pcks.Post()
                            Catch ex As Exception
                                If Not oLogger Is Nothing Then oLogger.Write("Exception raised for statement  pcks.Post(): " + ex.Message.ToString()) ''RWMS-1485
                            End Try
                            If Not oLogger Is Nothing Then oLogger.Write(" PlanStrategy.CreatePickList - >Picklist created: " + pickListId.ToString()) 'RWMS-1485
                            _picklistcollection.Add(pickListId)
                        End If
                    End If
                End If
            End If
        Next
        If _reqbaskets.Count <> 0 Then
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator(" ", 100)
            End If
        End If
    End Sub

    Public Sub CreateLoadingPlan(Optional ByVal oLogger As LogHandler = Nothing)
        Dim bskt As PickBaskets
        Dim pck As PicksObject
        Dim pcks As PicksObjectCollection
        Dim pcksbr As PicksObjectCollection
        Dim pcksbreaked As ArrayList
        Dim LoadingPlanId As String
        Dim pickListId As String

        For Each bskt In _reqbaskets
            pcks = bskt.CreatePickLists(_strategyid)
            If Not pcks Is Nothing Then
                pcks.Sort(_picklistbasecube, _picklistbaseweight)
                pcksbreaked = _stratbreaker.BreakPartial(pcks, _pickpartialuom, _picklistbasecube, _picklistbaseweight, True, oLogger)
                If Not pcksbreaked Is Nothing Then
                    For Each pcksbr In pcksbreaked
                        'and create the picklist for the picks obj
                        pickListId = pcksbr.Post()
                        _picklistcollection.Add(pickListId)
                        pcksbr.CreateLoadingPlan(pickListId)
                    Next
                End If
                If pcks.Count > 0 Then
                    'and create the picklist for the picks obj
                    pickListId = pcks.Post()
                    _picklistcollection.Add(pickListId)
                    pcks.CreateLoadingPlan(pickListId)
                End If
            End If
        Next
    End Sub

    Public Sub ReleasePickList(Optional ByVal oLogger As LogHandler = Nothing)
        Dim pcklist As Picklist
        Dim pickid As String
        For Each pickid In _picklistcollection
            pcklist = New Picklist(pickid)
            Dim relStrat As ReleaseStrategyDetail = pcklist.getReleaseStrategy()
            oLogger.Write("Checking if release flag : " & relStrat.AutoRelease)
            If relStrat.AutoRelease = WMS.Lib.Release.AUTORELEASE.PLANANDRELEASE Then
                pcklist.ReleasePicklist("SYSTEM")
                oLogger.Write("Picklist : " & pcklist.PicklistID & " released .")
            End If
        Next
    End Sub
    'Start RWMS-1279 Planner service not planning end of pick
    Public Function ShouldReleasePickList(Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Dim pcklist As Picklist
        Dim pickid As String
        Try
            For Each pickid In _picklistcollection
                pcklist = New Picklist(pickid)
                oLogger.Write("Get picklist object for picklistid: " & pickid.ToString())
                Dim relStrat As ReleaseStrategyDetail = pcklist.getReleaseStrategy()
                oLogger.Write("relStart strategy ID: " & relStrat.StrategyId.ToString())
                If relStrat.AutoRelease = WMS.Lib.Release.AUTORELEASE.PLANANDRELEASE Then
                    Return True
                Else
                    Return False
                End If
            Next
        Catch ex As Exception
            oLogger.Write("Exception caught in ShouldReleasePickList() : " & ex.Message.ToString())
        End Try

    End Function
    'End RWMS-1279 Planner service not planning end of pick

    Public Sub Delete()
        Dim sql As String = String.Format("Delete from PLANSTRATEGYHEADER where STRATEGYID={0}", Made4Net.Shared.FormatField(_strategyid))
        DataInterface.RunSQL(sql)

        _stratdetail.DeleteAll()
        _stratbreaker.DeleteAll()
        _stratrelease.DeleteAll()
        _scoring.DeleteAll()
        _parallelstratrelease.DeleteAll()
    End Sub

    'Gets ship label format for partial pick release strategy for the current plan strategy
    Public Function GetPartialPickLabelFormat() As String
        Dim releasestrat As ReleaseStrategyDetail
        For Each releasestrat In Me.ReleaseStrategyDetails
            If (releasestrat.PickType.ToLower = "partial") Then
                Return releasestrat.ShipLabelFormat
            End If
        Next
        Return ""
    End Function

#End Region

End Class

#End Region

#Region "Plan Strategy Scoring"

<CLSCompliant(False)> Public Class PlanStrategyScoring

#Region "Variables"

    Protected _strategyid As String
    Protected _attributesscoring As Made4Net.DataAccess.Collections.GenericCollection
    Protected Shared _logger As ILogHandler

#End Region

#Region "Properties"
    Public ReadOnly Property StrategyId() As String
        Get
            Return _strategyid
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
    Public Sub New(ByVal sStrategyId As String)
        _strategyid = sStrategyId
        Load()
    End Sub
#End Region

#Region "Methods"

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from PLANSTRATEGYSCORING where strategyid = {0}", Made4Net.Shared.Util.FormatField(_strategyid))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            _attributesscoring = New Made4Net.DataAccess.Collections.GenericCollection
            For Each oCol As DataColumn In dt.Columns
                If Not oCol.ColumnName.ToLower = "strategyid" Then
                    _attributesscoring.Add(oCol.ColumnName, Convert.ToDecimal(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr(oCol.ColumnName), "0")))
                End If
            Next
        End If
        dt.Dispose()
    End Sub

    Public Sub Score(ByRef cLoadsCollection As DataRow(), Optional ByVal _logger As ILogHandler = Nothing)
        _logger.SafeWrite("Start Scoring the loads collection...")
        If cLoadsCollection.Length <= 1 Then Return

        Dim oScoreSetter As ScoreSetter =
            New ScoreSetter.ScoreSetterBuilder() _
                .Logger(_logger) _
                .Build()

        oScoreSetter.Score(cLoadsCollection, _attributesscoring)
        Try
            Dim first As Boolean = True
            For Each LoadRow As DataRow In cLoadsCollection
                Dim DataRowStr, CaptionRowStr As String
                If first Then
                    _logger.SafeWrite("Load".PadRight(23) & "Location".PadRight(10) & "Status".PadRight(11) & "QTY".PadRight(10) & "Allocated".PadRight(11) & "Available".PadRight(11) & "Score".PadRight(10))
                    _logger.SafeWriteSeperator("-", 80)
                    _logger.SafeWrite(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                    first = False
                Else
                    _logger.SafeWrite(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
                End If
            Next
            _logger.SafeWriteSeperator(" ", 20)
        Catch ex As Exception
        End Try
    End Sub

    Public Sub DeleteAll()
        Dim sql As String = String.Format("delete from PLANSTRATEGYSCORING where strategyid={0}", Made4Net.Shared.FormatField(_strategyid))
        DataInterface.RunSQL(sql)
        If Not _attributesscoring Is Nothing Then _attributesscoring.Clear()
    End Sub

#Region "Old Scoring"

    'Public Sub Score(ByRef cLoadsCollection As DataRow(), Optional ByVal oLogger As LogHandler = Nothing)
    '    If cLoadsCollection.Length = 0 Then Return
    '    ClearScore(cLoadsCollection)
    '    For idx As Int32 = 0 To _attributesscoring.Count - 1
    '        If _attributesscoring(idx) Is DBNull.Value Or _attributesscoring(idx) Is Nothing Or _attributesscoring(idx) = 0 Then
    '            'Do Nothing
    '        Else
    '            If _attributesscoring(idx) < 0 Then
    '                Sort(cLoadsCollection, _attributesscoring.Keys(idx), SortOrder.Descending)
    '            Else
    '                Sort(cLoadsCollection, _attributesscoring.Keys(idx), SortOrder.Ascending)
    '            End If
    '            setScore(cLoadsCollection, _attributesscoring.Keys(idx), Math.Abs(_attributesscoring(idx)))
    '        End If
    '    Next
    '    Sort(cLoadsCollection, "Score", SortOrder.Descending)

    '    Try
    '        Dim first As Boolean = True
    '        For Each LoadRow As DataRow In cLoadsCollection
    '            Dim DataRowStr, CaptionRowStr As String
    '            If first Then
    '                'For i As Integer = 0 To LoadRow.ItemArray.Length - 1
    '                '    CaptionRowStr = CaptionRowStr & "|" & LoadRow.Table.Columns(i).ColumnName()
    '                '    DataRowStr = DataRowStr & "|" & LoadRow.Item(i) & vbTab
    '                'Next
    '                oLogger.Write("Load".PadRight(23) & "Location".PadRight(10) & "Status".PadRight(11) & "QTY".PadRight(10) & "Allocated".PadRight(11) & "Available".PadRight(11) & "Score".PadRight(10))
    '                oLogger.writeSeperator("-", 80)
    '                oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
    '                first = False
    '                'If Not oLogger Is Nothing Then
    '                '    oLogger.Write(CaptionRowStr.TrimStart("|"))
    '                '    oLogger.Write(DataRowStr.TrimStart("|"))
    '                'End If
    '            Else
    '                'For i As Integer = 0 To LoadRow.ItemArray.Length - 1
    '                '    DataRowStr = DataRowStr & "|" & LoadRow.Item(i) & vbTab
    '                'Next
    '                If Not oLogger Is Nothing Then
    '                    oLogger.Write(Convert.ToString(LoadRow("LOADID")).PadRight(22) & "|" & Convert.ToString(LoadRow("LOCATION")).PadRight(10) & "|" & Convert.ToString(LoadRow("STATUS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITS")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSALLOCATED")).PadRight(10) & "|" & Convert.ToString(LoadRow("UNITSAVAILABLE")).PadRight(10) & "|" & Convert.ToString(LoadRow("SCORE")).PadRight(10))
    '                End If
    '            End If
    '        Next

    '        If Not oLogger Is Nothing Then
    '            oLogger.writeSeperator(" ", 20)
    '        End If

    '    Catch ex As Exception
    '    End Try

    'End Sub

    'Protected Sub setScore(ByRef cLoadsCollection As DataRow(), ByVal sFieldName As String, ByVal dAttributeScore As Decimal)
    '    Dim iNumValues As Int32
    '    Dim iValueIdx As Int32 = 0
    '    Dim oVal As Object = cLoadsCollection(0)(sFieldName)
    '    iNumValues = getNumValues(cLoadsCollection, sFieldName)
    '    For idx As Int32 = 0 To cLoadsCollection.Length - 1
    '        If (oVal Is Nothing Or oVal Is DBNull.Value) And (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            ' Do Nothing - Same Values
    '        ElseIf (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            oVal = cLoadsCollection(idx)(sFieldName)
    '            iValueIdx = iValueIdx + 1
    '        ElseIf oVal <> cLoadsCollection(idx)(sFieldName) Then
    '            oVal = cLoadsCollection(idx)(sFieldName)
    '            iValueIdx = iValueIdx + 1
    '        End If
    '        cLoadsCollection(idx)("score") = cLoadsCollection(idx)("score") + (dAttributeScore - (iValueIdx * dAttributeScore / iNumValues))
    '    Next
    'End Sub

    'Protected Function getNumValues(ByRef cLoadsCollection As DataRow(), ByVal sFieldName As String) As Integer
    '    Dim iNumValues As Int32 = 1
    '    Dim val As Object = cLoadsCollection(0)(sFieldName)
    '    For idx As Int32 = 0 To cLoadsCollection.Length - 1
    '        If (val Is Nothing Or val Is DBNull.Value) And (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            ' Do Nothing - Same Values
    '        ElseIf (cLoadsCollection(idx)(sFieldName) Is Nothing Or cLoadsCollection(idx)(sFieldName) Is DBNull.Value) Then
    '            val = cLoadsCollection(idx)(sFieldName)
    '            iNumValues = iNumValues + 1
    '        ElseIf cLoadsCollection(idx)(sFieldName) <> val Then
    '            val = cLoadsCollection(idx)(sFieldName)
    '            iNumValues = iNumValues + 1
    '        End If
    '    Next
    '    Return iNumValues
    'End Function

    'Protected Sub ClearScore(ByRef cLoadsCollection As DataRow())
    '    For Each oLoadRecord As DataRow In cLoadsCollection
    '        oLoadRecord("Score") = 0
    '    Next
    'End Sub

    'Protected Sub Sort(ByRef cLoadsCollection As DataRow(), ByVal sFieldName As String, ByVal eSortOrder As SortOrder)
    '    Dim bSorted As Boolean = False
    '    For idx As Int32 = 1 To cLoadsCollection.Length - 1
    '        bSorted = True
    '        For jdx As Int32 = cLoadsCollection.Length - 1 To idx Step -1
    '            Select Case eSortOrder
    '                Case SortOrder.Ascending
    '                    If (Not cLoadsCollection(jdx).IsNull(sFieldName)) Then
    '                        If cLoadsCollection(jdx - 1).IsNull(sFieldName) Then
    '                            Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                            bSorted = False
    '                        ElseIf (cLoadsCollection(jdx)(sFieldName) < cLoadsCollection(jdx - 1)(sFieldName)) Then
    '                            Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                            bSorted = False
    '                        End If
    '                    End If
    '                Case SortOrder.Descending
    '                    If (Not cLoadsCollection(jdx).IsNull(sFieldName)) Then
    '                        If cLoadsCollection(jdx - 1).IsNull(sFieldName) Then
    '                            Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                            bSorted = False
    '                        ElseIf (cLoadsCollection(jdx)(sFieldName) > cLoadsCollection(jdx - 1)(sFieldName)) Then
    '                            Swap(cLoadsCollection(jdx), cLoadsCollection(jdx - 1))
    '                            bSorted = False
    '                        End If
    '                    End If
    '            End Select
    '        Next
    '        If bSorted Then Exit For
    '    Next
    'End Sub

    'Protected Enum SortOrder
    '    Ascending
    '    Descending
    'End Enum

    'Protected Sub Swap(ByRef oDataRow1 As DataRow, ByRef oDataRow2 As DataRow)
    '    Dim tempDataRow As DataRow = oDataRow1
    '    oDataRow1 = oDataRow2
    '    oDataRow2 = tempDataRow
    'End Sub

#End Region

#End Region

End Class

#End Region

#Region "Plan Strategy Detail"

<CLSCompliant(False)> Public Class PlanStrategyDetail

#Region "Variables"

    Protected _strategyid As String
    Protected _priority As Int32
    Protected _pickregion As String
    Protected _uom As String
    Protected _picktype As String
    Protected _allocfullrequirement As Boolean
    Protected _allocstratlineUOM As Boolean
    Protected _locpicktype As String
    Protected _npppwvol As Decimal
    Protected _allocbyhighestuom As Boolean
    Protected _allocuomqty As String
    Protected _zpicking As Boolean
    Protected _overallocpicklocs As Boolean

    Protected _fullpickallocation As String

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

    Public ReadOnly Property PickRegion() As String
        Get
            Return _pickregion
        End Get
    End Property

    Public ReadOnly Property UOM() As String
        Get
            Return _uom
        End Get
    End Property

    Public ReadOnly Property PickType() As String
        Get
            Return _picktype
        End Get
    End Property

    Public ReadOnly Property LocPickType() As String
        Get
            Return _locpicktype
        End Get
    End Property

    Public ReadOnly Property AllocFullRequirment() As Boolean
        Get
            Return _allocfullrequirement
        End Get
    End Property

    Public ReadOnly Property AllocByHighestUOM() As Boolean
        Get
            Return _allocbyhighestuom
        End Get
    End Property

    Public ReadOnly Property AllocationUOMQTY() As String
        Get
            Return _allocuomqty
        End Get
    End Property

    Public ReadOnly Property AllocStratLineUOM() As Boolean
        Get
            Return _allocstratlineUOM
        End Get
    End Property

    Public ReadOnly Property NegativePalletPickVol() As Decimal
        Get
            Return _npppwvol
        End Get
    End Property

    Public ReadOnly Property ZPicking() As Boolean
        Get
            Return _zpicking
        End Get
    End Property

    Public ReadOnly Property OverAllocatePickLocations() As Boolean
        Get
            Return _overallocpicklocs
        End Get
    End Property

    Public ReadOnly Property FullPickAllocation() As String
        Get
            Return _fullpickallocation
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

    Public Sub New(ByVal dr As DataRow)
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        _pickregion = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKREGION"), "")).Replace("*", "%")
        If _pickregion = "" Then _pickregion = "%"
        _uom = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UOM"), "")).Replace("*", "").Replace("%", "")
        _picktype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKTYPE"), "")).Replace("*", "").Replace("%", "")
        _locpicktype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LOCPICKTYPE"), "")).Replace("*", "").Replace("%", "")
        _allocfullrequirement = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ALLOCFULLREQUIREMENT"))
        _allocstratlineUOM = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ALLOCBYLINEUOM"))
        _npppwvol = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NPPPWVOL"), Decimal.MaxValue)
        _allocbyhighestuom = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ALLOCBYHIGHESTUOM"))
        _allocuomqty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ALLOCUOMQTY"))
        _zpicking = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ZPICKING"))
        _overallocpicklocs = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("OVERALLOCPICKLOCS"))

        _fullpickallocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FULLPICKALLOCATION"))

        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

#End Region

    ' Added by Lev:
    ' For Strategy line get all lines with the same priorery and set the filter string with the values

    Public Function getDetailFilterByPriority(Optional ByVal pPickLocAllocation As Boolean = False) As String

        Dim drArr() As DataRow = PlanStrategies.Details.GetRowsForStrategyIdPriorityPickType(_strategyid, _priority, _picktype)
        Dim filter As String
        If drArr.Length = 0 Then
            Return ""
        End If
        If drArr.Length = 1 Then
            filter = " AND (PICKREGION LIKE '" & drArr(0)("PICKREGION") & "' AND ISNULL(LOCPICKTYPE,'') LIKE '" & drArr(0)("LOCPICKTYPE") & "'"
            If Not pPickLocAllocation Then
                filter = filter & " AND LOADUOM LIKE '" & drArr(0)("UOM") & "'"
            End If
            filter = filter & ")"
            Return filter
        End If
        Dim SQLFilter As String
        For Each dr As DataRow In drArr
            SQLFilter = SQLFilter & " (PICKREGION LIKE '" & dr("PICKREGION") & "' AND LOCPICKTYPE LIKE '" & dr("LOCPICKTYPE") & "'"
            If Not pPickLocAllocation Then
                SQLFilter = SQLFilter & " AND LOADUOM LIKE '" & dr("UOM") & "'"
            End If
            SQLFilter = SQLFilter & ") OR"
        Next
        SQLFilter = " AND (" & SQLFilter.TrimEnd("OR".ToCharArray) & ") "
        Return SQLFilter
    End Function

End Class

#End Region

#Region "Plan Strategy Detail Collection"

<CLSCompliant(False)> Public Class PlanStrategyDetailCollection
    Implements ICollection

#Region "Variables"

    Protected _stratlines As ArrayList
    Protected _planstrategyid As String

#End Region

#Region "Constructor"

    Public Sub New(ByVal PlanStrategyId As String)
        _stratlines = New ArrayList
        _planstrategyid = PlanStrategyId
        Load()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As PlanStrategyDetail
        Get
            Return CType(_stratlines(index), PlanStrategyDetail)
        End Get
        Set(ByVal Value As PlanStrategyDetail)
            _stratlines(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from planstrategydetail Where strategyid='{0}' order by priority asc", _planstrategyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim stDetail As PlanStrategyDetail
        For Each dr In dt.Rows
            stDetail = New PlanStrategyDetail(dr)
            Add(stDetail)
        Next

    End Sub

    Public Sub DeleteAll()
        Dim sql As String = String.Format("delete from planstrategydetail where strategyid={0}", Made4Net.Shared.FormatField(_planstrategyid))
        DataInterface.RunSQL(sql)
        Me.Clear()
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As PlanStrategyDetail) As Integer
        Return _stratlines.Add(value)
    End Function

    Public Sub Clear()
        _stratlines.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As PlanStrategyDetail)
        _stratlines.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As PlanStrategyDetail)
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

#Region "Release Strategy"

<CLSCompliant(False)> Public Class ReleaseStrategyDetail

#Region "Variables"

    Protected _releasepolicyid As Int32
    Protected _strategyid As String
    Protected _priority As Int32
    Protected _picktype As String
    Protected _printpicklist As Boolean
    Protected _printbagoutreport As Boolean
    Protected _bagoutprintoption As String
    Protected _bagoutreportname As String
    Protected _caseLabelPrintOption As Boolean
    Protected _printcontentlist As Boolean
    Protected _contentlistdocname As String
    Protected _autoprintpicklabels As String
    Protected _createjob As Boolean
    Protected _jobpriority As Int32
    Protected _confirmationtype As String
    Protected _grouppickdetail As Boolean
    Protected _systempickshort As String
    Protected _userpickshort As String
    Protected _autorelease As String
    Protected _delcontonclose As Boolean
    Protected _labelformat As String
    Protected _shiplabelformat As String
    Protected _tasksubtype As String
    Protected _caseLabelFromat As String
    Protected _dynamicdeliloc As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String


#End Region

#Region "Properties"
    Public Property CaseLabelFormat() As String
        Get
            Return _caseLabelFromat
        End Get
        Set(ByVal value As String)
            _caseLabelFromat = value
        End Set
    End Property


    Public Property CaseLabelPrintOption() As Boolean
        Get
            Return _caseLabelPrintOption
        End Get
        Set(ByVal value As Boolean)
            _caseLabelPrintOption = value
        End Set
    End Property
    Public ReadOnly Property WhereClause() As String
        Get
            Return "ReleasePolicyid = " & _releasepolicyid
        End Get
    End Property

    Public ReadOnly Property ReleasePolicyId() As Int32
        Get
            Return _releasepolicyid
        End Get
    End Property

    Public ReadOnly Property StrategyId() As String
        Get
            Return _strategyid
        End Get
    End Property

    Public Property Priority() As Int32
        Get
            Return _priority
        End Get
        Set(ByVal Value As Int32)
            _priority = Value
        End Set
    End Property

    Public Property PickType() As String
        Get
            Return _picktype
        End Get
        Set(ByVal Value As String)
            _picktype = Value
        End Set
    End Property

    Public Property PrintPickList() As Boolean
        Get
            Return _printpicklist
        End Get
        Set(ByVal Value As Boolean)
            _printpicklist = Value
        End Set
    End Property

    Public Property PrintContentList() As Boolean
        Get
            Return _printcontentlist
        End Get
        Set(ByVal Value As Boolean)
            _printcontentlist = Value
        End Set
    End Property

    Public Property ContentListDocName() As String
        Get
            Return _contentlistdocname
        End Get
        Set(ByVal Value As String)
            _contentlistdocname = Value
        End Set
    End Property
    Public Property PrintBagOutReport() As Boolean
        Get
            Return _printbagoutreport
        End Get
        Set(ByVal Value As Boolean)
            _printbagoutreport = Value
        End Set
    End Property

    Public Property BagOutReportName() As String
        Get
            Return _bagoutreportname
        End Get
        Set(ByVal Value As String)
            _bagoutreportname = Value
        End Set
    End Property
    Public Property BagOutPrintOption() As String
        Get
            Return _bagoutprintoption
        End Get
        Set(ByVal Value As String)
            _bagoutprintoption = Value
        End Set
    End Property

    Public Property AutoPrintPickLabels() As String
        Get
            Return _autoprintpicklabels
        End Get
        Set(ByVal Value As String)
            _autoprintpicklabels = Value
        End Set
    End Property

    Public Property CreateJob() As Boolean
        Get
            Return _createjob
        End Get
        Set(ByVal Value As Boolean)
            _createjob = Value
        End Set
    End Property

    Public Property JobPriority() As Int32
        Get
            Return _jobpriority
        End Get
        Set(ByVal Value As Int32)
            _jobpriority = Value
        End Set
    End Property

    Public Property ConfirmationType() As String
        Get
            Return _confirmationtype
        End Get
        Set(ByVal Value As String)
            _confirmationtype = Value
        End Set
    End Property

    Public Property GroupPickDetails() As Boolean
        Get
            Return _grouppickdetail
        End Get
        Set(ByVal Value As Boolean)
            _grouppickdetail = Value
        End Set
    End Property

    Public Property AutoRelease() As String
        Get
            Return _autorelease
        End Get
        Set(ByVal Value As String)
            _autorelease = Value
        End Set
    End Property

    Public Property SystemPickShort() As String
        Get
            Return _systempickshort
        End Get
        Set(ByVal Value As String)
            _systempickshort = Value
        End Set
    End Property

    Public Property UserPickShort() As String
        Get
            Return _userpickshort
        End Get
        Set(ByVal Value As String)
            _userpickshort = Value
        End Set
    End Property

    Public Property DeliverContainerOnClosing() As Boolean
        Get
            Return _delcontonclose
        End Get
        Set(ByVal Value As Boolean)
            _delcontonclose = Value
        End Set
    End Property

    Public Property LabelFormat() As String
        Get
            Return _labelformat
        End Get
        Set(ByVal Value As String)
            _labelformat = Value
        End Set
    End Property

    Public Property ShipLabelFormat() As String
        Get
            Return _shiplabelformat
        End Get
        Set(ByVal Value As String)
            _shiplabelformat = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public Property TaskSubType() As String
        Get
            Return _tasksubtype
        End Get
        Set(ByVal value As String)
            _tasksubtype = value
        End Set
    End Property

    Public Property DynamicDeliLoc() As String
        Get
            Return _dynamicdeliloc
        End Get
        Set(ByVal value As String)
            _dynamicdeliloc = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal PolicyId As Int32)
        Dim dt As New DataTable
        Dim sql As String = String.Format("Select * from PLANSTRATEGYRELEASE where ReleasePolicyid={0}", PolicyId)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            'Exception
        Else
            Load(dt.Rows(0))
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load(ByVal dr As DataRow)
        _releasepolicyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RELEASEPOLICYID"))
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        _picktype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKTYPE"), ""))
        _printpicklist = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRINTPICKLIST"), False))
        _contentlistdocname = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTENTLISTDOCNAME"), ""))
        _printcontentlist = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRINTCONTENTLIST"), ""))
        _printbagoutreport = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRINTBAGOUTREPORT"), False))
        _bagoutprintoption = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("BAGOUTPRINTOPTION"), ""))
        _bagoutreportname = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("BAGOUTREPORTNAME"), ""))
        _autoprintpicklabels = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AUTOPRINTPICKLABELS"), ""))
        _createjob = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CREATEJOB"), True))
        _jobpriority = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("JOBPRIORITY"), 200))
        _confirmationtype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONFIRMATIONTYPE"), 200))
        _grouppickdetail = Convert.ToBoolean(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("GROUPPICKDETAILS"), 1))
        _autorelease = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("AUTORELEASE"), ""))
        _systempickshort = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SYSTEMPICKSHORT"), ""))
        _userpickshort = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("USERPICKSHORT"), ""))
        _delcontonclose = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DELCONTONCLOSE"), True))
        _shiplabelformat = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SHIPLABELFORMAT"), ""))
        _labelformat = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LABELFORMAT"), ""))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
        _tasksubtype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TASKSUBTYPE"), ""))
        _dynamicdeliloc = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DYNAMICDELILOC"), ""))
        _caseLabelPrintOption = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRINTCASELABELS"), ""))
        _caseLabelFromat = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CASELABELFORMAT"), ""))
    End Sub

    Public Sub Delete()
        Dim sql As String = String.Format("from PLANSTRATEGYRELEASE where {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Create()
        If Exists(_releasepolicyid) Then
            Throw New M4NException(New Exception, "Release strategy detail already exists.", "Release strategy detail already exists.")
        End If
        Save()
    End Sub

    Public Sub Update()
        If Not Exists(_releasepolicyid) Then
            Throw New M4NException(New Exception, "Release strategy detail does not exists.", "Release strategy detail does not exists.")
        End If
        Save()
    End Sub

    Public Sub Save()
        Dim sql As String
        _adduser = WMS.Logic.GetCurrentUser()
        _edituser = WMS.Logic.GetCurrentUser()

        If Exists(_releasepolicyid) Then
            sql = String.Format("Update PLANSTRATEGYRELEASE set STRATEGYID={0},PRIORITY={1},PICKTYPE={2},PRINTPICKLIST={3},AUTOPRINTPICKLABELS={4},CREATEJOB={5}," &
                        "JOBPRIORITY={6},CONFIRMATIONTYPE={7},GROUPPICKDETAILS={8},AUTORELEASE={9},ADDDATE={10},ADDUSER={11},EDITDATE={12},EDITUSER={13},LABELFORMAT={14},SYSTEMPICKSHORT={15}," &
                        "USERPICKSHORT={16},DELCONTONCLOSE={17},SHIPLABELFORMAT={18},PRINTCONTENTLIST={19},CONTENTLISTDOCNAME={20},TASKSUBTYPE={21},DYNAMICDELILOC={22},PRINTCASELABELS={23},CASELABELFORMAT={24} where {25}",
                        Made4Net.Shared.FormatField(_strategyid), Made4Net.Shared.FormatField(_priority), Made4Net.Shared.FormatField(_picktype),
                        Made4Net.Shared.FormatField(_printpicklist), Made4Net.Shared.FormatField(_autoprintpicklabels),
                        Made4Net.Shared.FormatField(_createjob), Made4Net.Shared.FormatField(_jobpriority), Made4Net.Shared.FormatField(_confirmationtype),
                        Made4Net.Shared.FormatField(_grouppickdetail), Made4Net.Shared.FormatField(_autorelease), Made4Net.Shared.FormatField(_adddate),
                        Made4Net.Shared.FormatField(_adduser), Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_edituser),
                        Made4Net.Shared.FormatField(_labelformat), Made4Net.Shared.FormatField(_systempickshort), Made4Net.Shared.FormatField(_userpickshort),
                        Made4Net.Shared.FormatField(_delcontonclose), Made4Net.Shared.FormatField(_shiplabelformat), Made4Net.Shared.FormatField(_printcontentlist),
                        Made4Net.Shared.FormatField(_contentlistdocname), Made4Net.Shared.FormatField(_tasksubtype), Made4Net.Shared.FormatField(_dynamicdeliloc),
                        Made4Net.Shared.FormatField(_caseLabelPrintOption), Made4Net.Shared.FormatField(_caseLabelFromat),
                        WhereClause)
        Else
            sql = String.Format("Insert into PLANSTRATEGYRELEASE (RELEASEPOLICYID, STRATEGYID, PRIORITY,  PICKTYPE, PRINTPICKLIST, " &
            "AUTOPRINTPICKLABELS,CREATEJOB,JOBPRIORITY,CONFIRMATIONTYPE,GROUPPICKDETAILS,AUTORELEASE,ADDDATE,ADDUSER,EDITDATE,EDITUSER,LABELFORMAT," &
            "SYSTEMPICKSHORT,USERPICKSHORT,DELCONTONCLOSE,SHIPLABELFORMAT,PRINTCONTENTLIST,CONTENTLISTDOCNAME,TASKSUBTYPE,DYNAMICDELILOC,PRINTCASELABELS,CASELABELFORMAT) " &
            "values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15,{16},{17},{18},{19},{20},{21},{22},{23},{24},{25})",
            Made4Net.Shared.FormatField(_releasepolicyid), Made4Net.Shared.FormatField(_strategyid), Made4Net.Shared.FormatField(_priority),
            Made4Net.Shared.FormatField(_picktype), Made4Net.Shared.FormatField(_printpicklist), Made4Net.Shared.FormatField(_autoprintpicklabels),
            Made4Net.Shared.FormatField(_createjob), Made4Net.Shared.FormatField(_jobpriority), Made4Net.Shared.FormatField(_confirmationtype),
            Made4Net.Shared.FormatField(_grouppickdetail), Made4Net.Shared.FormatField(_autorelease), Made4Net.Shared.FormatField(_adddate),
            Made4Net.Shared.FormatField(_adduser), Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_edituser),
            Made4Net.Shared.FormatField(_labelformat), Made4Net.Shared.FormatField(_systempickshort), Made4Net.Shared.FormatField(_userpickshort),
            Made4Net.Shared.FormatField(_delcontonclose), Made4Net.Shared.FormatField(_shiplabelformat), Made4Net.Shared.FormatField(_printcontentlist),
            Made4Net.Shared.FormatField(_contentlistdocname), Made4Net.Shared.FormatField(_tasksubtype), Made4Net.Shared.FormatField(_dynamicdeliloc),
            Made4Net.Shared.FormatField(_caseLabelPrintOption), Made4Net.Shared.FormatField(_caseLabelFromat))
        End If
        DataInterface.RunSQL(sql)
    End Sub

    Public Shared Function Exists(ByVal pReleasePolicyID As Integer) As Boolean
        Dim sql As String = String.Format("Select count(1) from PLANSTRATEGYRELEASE where PARALLELSTRATID={0}", pReleasePolicyID)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Release(ByVal Pcklist As Picklist, Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.writeSeperator("-", 100)
            oLogger.Write("Releasing PickList " & Pcklist.PicklistID)
        End If
        If _printpicklist Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Printing Picklist...")
            End If
            Pcklist.Print()
        End If
        If _autoprintpicklabels = WMS.Lib.Release.AUTOPRINTPICKLABEL.ONRELEASE Then
            If Not _labelformat = WMS.Lib.Release.PICKLABELTYPE.NONE Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Printing Pick Labels...")
                End If
                Pcklist.PrintPickLabels(_labelformat, Nothing)
            End If
        End If
        If (_createjob And Not Pcklist.PickType = WMS.Lib.PICKTYPE.PARALLELPICK) Then ' Or (_createjob And Not Pcklist.PickMethod = WMS.Lib.PickMethods.PickMethod.PARALELORDERPICKING)
            If Not TaskExists(Pcklist.PicklistID) Then
                Dim ts As New TaskManager
                ts.CreatePickTask(Pcklist, _jobpriority, _tasksubtype)
                If Not oLogger Is Nothing Then
                    oLogger.Write("Creating Pick Tasks. Task " & ts.Task.TASK & " Was created...")
                End If
                Pcklist.Release()
                Pcklist.ReleaseComplete()
                CreateZPickingPWTasks(Pcklist.PicklistID, oLogger)
            End If
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("PickList " & Pcklist.PicklistID & " Released.")
            oLogger.writeSeperator("-", 100)
        End If
    End Sub

    Private Function TaskExists(ByVal pPickListId As String) As Boolean
        Dim sql As String = String.Format("select count(1) from TASKS where status = '{0}' and (TASKTYPE = '{1}' or TASKTYPE = '{2}' or TASKTYPE = '{4}') and picklist = '{3}'", WMS.Lib.Statuses.Task.AVAILABLE, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.FULLPICKING, pPickListId, WMS.Lib.TASKTYPE.NEGPALLETPICK)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Sub CreateZPickingPWTasks(ByVal pPickListID As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim dtZPickLines As DataTable = GetZPickingLines(pPickListID)
        If dtZPickLines.Rows.Count > 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Creating PW Tasks for lines allocated from ZPicking Locations of picklist: " & pPickListID)
            End If
        End If
        Dim ts As New TaskManager
        For Each dr As DataRow In dtZPickLines.Rows
            Try
                'check for the putaway task type
                If dr("handlingunit") <> String.Empty Then
                    If ShouldCreateContPW(dr("handlingunit")) Then
                        Dim oCont As New WMS.Logic.Container(dr("handlingunit"), True)
                        ts.CreateContainerPutAwayTask(oCont, WMS.Lib.USERS.SYSTEMUSER, _jobpriority, dr("fromlocation"), False)
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Creating Container putaway Tasks. Task " & ts.Task.TASK & " Was created for container " & oCont.ContainerId & ".")
                        End If
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Container putaway Tasks already exists. will not create new task for z-pick")
                        End If
                    End If
                Else
                    'If ShouldCreateLoadPW(dr("fromload")) Then
                    If ShouldCreateLoadPW(dr("loadid")) Then
                        Dim oLoad As New WMS.Logic.Load(Convert.ToString(dr("loadid")))
                        oLoad.SetDestinationLocation(dr("fromlocation"), dr("fromwarehousearea"), WMS.Lib.USERS.SYSTEMUSER)
                        ts.CreatePutAwayTask(oLoad, WMS.Lib.USERS.SYSTEMUSER, False, _jobpriority, dr("fromlocation"))
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Creating Load putaway Tasks. Task " & ts.Task.TASK & " Was created for Load " & oLoad.LOADID & ".")
                        End If
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Container putaway Tasks already exists. will not create new task for z-pick")
                        End If
                    End If
                End If
            Catch ex As Made4Net.Shared.M4NException
                If Not oLogger Is Nothing Then
                    oLogger.Write("Error occured while creating putaway task: " & ex.Description)
                End If
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write("Error occured while creating putaway task: " & ex.ToString)
                End If
            End Try
        Next
    End Sub

    Private Function ShouldCreateContPW(ByVal pContId As String) As Boolean
        Dim SQL As String = String.Format("select count(1) from tasks where tasktype = '{1}' and status not in ('COMPLETE','CANCELED') and fromcontainer = '{0}'", pContId, WMS.Lib.TASKTYPE.CONTPUTAWAY)
        Return Not Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
    End Function

    Private Function ShouldCreateLoadPW(ByVal pLoadid As String) As Boolean
        Dim SQL As String = String.Format("select count(1) from tasks where tasktype = '{1}' and status not in ('COMPLETE','CANCELED') and fromload = '{0}'", pLoadid, WMS.Lib.TASKTYPE.LOADPUTAWAY)
        Return Not Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
    End Function

    Private Function GetZPickingLines(ByVal pPickList As String) As DataTable
        Dim SQL As String = String.Format("select distinct handlingunit, '' as loadid,fromlocation from pickdetail pd inner join invload on pd.fromlocation <> invload.location and pd.fromload = invload.loadid where pd.picklist = '{0}' and (handlingunit <> '' and handlingunit is not null) " & _
            "union select distinct '' as handlingunit, fromload as loadid, fromlocation from pickdetail pd inner join invload on pd.fromlocation <> invload.location and pd.fromload = invload.loadid where pd.picklist = '{0}' and (handlingunit = '' or handlingunit is null) ", pPickList)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

#End Region

End Class

#Region "Pick Short Types"

<CLSCompliant(False)> Public Class SystemPickShort
    Public Const PickPartialCreateException As String = "PCKPRTCREX"
    Public Const PickPartialLeaveOpen As String = "PCKPRTOPEN"
    Public Const PickPartialCancelException As String = "PCKPRTCNEX"
    Public Const PickZeroCreateException As String = "PCKZERCREX"
    Public Const PickZeroLeaveOpen As String = "PCKZEROPEN"
    Public Const PickZeroCancelException As String = "PCKZERCNEX"
End Class

<CLSCompliant(False)> Public Class UserPickShort
    Public Const PickPartialCreateException As String = "PCKPRTCREX"
    Public Const PickPartialLeaveOpen As String = "PCKPRTOPEN"
    Public Const PickPartialCancelException As String = "PCKPRTCNEX"
End Class

#End Region

#End Region

#Region "Release Strategy Collection"

<CLSCompliant(False)> Public Class ReleaseStrategyCollection
    Implements ICollection

#Region "Variables"

    Protected _stratreleaselines As ArrayList
    Protected _planstrategyid As String

#End Region

#Region "Constructor"

    Public Sub New(ByVal PlanStrategyId As String)
        _stratreleaselines = New ArrayList
        _planstrategyid = PlanStrategyId
        Load()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As ReleaseStrategyDetail
        Get
            Return CType(_stratreleaselines(index), ReleaseStrategyDetail)
        End Get
        Set(ByVal Value As ReleaseStrategyDetail)
            _stratreleaselines(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from PLANSTRATEGYRELEASE Where strategyid='{0}' order by priority asc", _planstrategyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim stDetail As ReleaseStrategyDetail
        For Each dr In dt.Rows
            stDetail = New ReleaseStrategyDetail(dr)
            Add(stDetail)
        Next

    End Sub

    'Public Function AutoReleaseAfterPlanning(ByVal pPickType As String) As Boolean
    '    Dim stDetail As ReleaseStrategyDetail
    '    For Each stDetail In Me._stratreleaselines
    '        If stDetail.PickType = pPickType Then
    '            If stDetail.AutoRelease = WMS.Lib.Release.AUTORELEASE.PLANANDRELEASE Then
    '                Return True
    '            End If
    '        End If
    '    Next
    '    Return False
    'End Function

    Public Sub DeleteAll()
        Dim sql As String = String.Format("delete from PLANSTRATEGYRELEASE where strategyid ={0}", Made4Net.Shared.FormatField(_planstrategyid))
        DataInterface.RunSQL(sql)
        Me.Clear()
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As ReleaseStrategyDetail) As Integer
        Return _stratreleaselines.Add(value)
    End Function

    Public Sub Clear()
        _stratreleaselines.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As ReleaseStrategyDetail)
        _stratreleaselines.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As ReleaseStrategyDetail)
        _stratreleaselines.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _stratreleaselines.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _stratreleaselines.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _stratreleaselines.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _stratreleaselines.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _stratreleaselines.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _stratreleaselines.GetEnumerator()
    End Function

#End Region

End Class

#End Region

#Region "Parallel Release Strategy"

<CLSCompliant(False)> Public Class ParallelReleaseStrategyDetail

#Region "Variables"

    Protected _parallelstratid As Int32
    Protected _strategyid As String
    Protected _priority As Int32
    'Protected _picktype As String
    Protected _handelingunittype As String
    Protected _route As Boolean
    Protected _staginglane As Boolean
    Protected _customer As Boolean
    Protected _shipment As Boolean
    Protected _onhutype As String
    Protected _numlists As Int32
    Protected _pickregion As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return "PARALLELSTRATID = " & _parallelstratid.ToString()
        End Get
    End Property

    Public ReadOnly Property ParallelStratId() As Int32
        Get
            Return _parallelstratid
        End Get
    End Property

    Public ReadOnly Property StrategyId() As String
        Get
            Return _strategyid
        End Get
    End Property

    Public Property Priority() As Int32
        Get
            Return _priority
        End Get
        Set(ByVal Value As Int32)
            _priority = Value
        End Set
    End Property

    'Public Property PickType() As String
    '    Get
    '        Return _picktype
    '    End Get
    '    Set(ByVal Value As String)
    '        _picktype = Value
    '    End Set
    'End Property

    Public Property HandelingUnitType() As String
        Get
            Return _handelingunittype
        End Get
        Set(ByVal Value As String)
            _handelingunittype = Value
        End Set
    End Property

    Public Property Route() As Boolean
        Get
            Return _route
        End Get
        Set(ByVal Value As Boolean)
            _route = Value
        End Set
    End Property

    Public Property StagingLane() As Boolean
        Get
            Return _staginglane
        End Get
        Set(ByVal Value As Boolean)
            _staginglane = Value
        End Set
    End Property

    Public Property Customer() As Boolean
        Get
            Return _customer
        End Get
        Set(ByVal Value As Boolean)
            _customer = Value
        End Set
    End Property

    Public Property Shipment() As Boolean
        Get
            Return _shipment
        End Get
        Set(ByVal Value As Boolean)
            _shipment = Value
        End Set
    End Property

    Public Property OnHandelingUnitType() As String
        Get
            Return _onhutype
        End Get
        Set(ByVal Value As String)
            _onhutype = Value
        End Set
    End Property

    Public Property PickRegion() As Boolean
        Get
            Return _pickregion
        End Get
        Set(ByVal Value As Boolean)
            _pickregion = Value
        End Set
    End Property

    Public Property NumLists() As Int32
        Get
            Return _numlists
        End Get
        Set(ByVal Value As Int32)
            _numlists = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal PolicyId As Int32)
        Dim dt As New DataTable
        Dim sql As String = String.Format("Select * from PLANPARALLELRELEASESTRATEGY where PARALLELSTRATID={0}", PolicyId)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            'Exception
        Else
            Load(dt.Rows(0))
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load(ByVal dr As DataRow)
        _parallelstratid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PARALLELSTRATID"))
        _strategyid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STRATEGYID"))
        _priority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"))
        '_picktype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKTYPE"), ""))
        _handelingunittype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HANDELINGUNITTYPE"), ""))
        _route = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE"), False))
        _staginglane = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STAGINGLANE"), False))
        _customer = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CUSTOMER"), False))
        _shipment = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SHIPMENT"), False))
        _pickregion = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKREGION"), False))
        _onhutype = Convert.ToString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ONHUTYPE"), ""))
        _numlists = Convert.ToInt32(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NUMLISTS"), 1))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

    Public Sub Release(ByVal parrelstrat As ReleasePickListCollection)
        Dim par As New ParallelPicking
        par.HandlingUnitType = Me.HandelingUnitType
        par.Status = WMS.Lib.Statuses.Picklist.RELEASED
        If Not _onhutype = Nothing And Not _onhutype = String.Empty Then
            par.ToContainer = Made4Net.Shared.Util.getNextCounter("CONTAINER")
            par.HandlingUnitType = _onhutype
        End If
        par.Save(WMS.Lib.USERS.SYSTEMUSER)
        Dim dt As DataTable = getLocSortOrderPicklists(parrelstrat)
        Dim pcklist As Picklist
        For Each dr As DataRow In dt.Rows
            For Each pcklist In parrelstrat
                If pcklist.PicklistID = dr("picklist") Then
                    par.AddLine(pcklist)
                    ' Release the picklist
                    pcklist.Release()
                    pcklist.ReleaseComplete()
                End If
            Next
        Next
        Dim t As New TaskManager
        t.CreateParallelPickTask(par, _priority, parrelstrat.TaskSubType)
    End Sub

    Private Function getLocSortOrderPicklists(ByVal parrelstrat As ReleasePickListCollection) As DataTable
        Dim dt As New DataTable
        Dim Sql As String = String.Empty
        Dim picklistid As String = String.Empty
        For Each pcklist As Picklist In parrelstrat
            picklistid = picklistid & String.Format("'{0}',", pcklist.PicklistID)
        Next
        picklistid = picklistid.TrimEnd(",".ToCharArray)
        Sql = String.Format("select distinct picklist,locsortorder from pickheader ph inner join location loc on (select top 1 fromlocation from pickdetail where picklist = ph.picklist) = loc.location where picklist in ({0}) order by locsortorder", picklistid)
        DataInterface.FillDataset(Sql, dt)
        Return dt
    End Function

    Public Sub Save()
        Dim sql As String
        _adduser = WMS.Logic.GetCurrentUser()
        _edituser = WMS.Logic.GetCurrentUser()

        If Exists(_parallelstratid) Then
            sql = String.Format("Update PLANPARALLELRELEASESTRATEGY set STRATEGYID={0},PRIORITY={1),HANDELINGUNITTYPE={2},ROUTE={3}" & _
            "STAGINGLANE={4},CUSTOMER={5},SHIPMENT={6},ONHUTYPE={7},NUMLISTS={8},PICKREGION={9},EDITDATE={10},EDITUSER={11} where {12}", _
            Made4Net.Shared.FormatField(_strategyid), Made4Net.Shared.FormatField(_priority), Made4Net.Shared.FormatField(_handelingunittype), _
            Made4Net.Shared.FormatField(_route), Made4Net.Shared.FormatField(_staginglane), Made4Net.Shared.FormatField(_customer), _
            Made4Net.Shared.FormatField(_shipment), Made4Net.Shared.FormatField(_onhutype), Made4Net.Shared.FormatField(_numlists), _
            Made4Net.Shared.FormatField(_pickregion), Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_edituser), WhereClause)
        Else
            _adddate = DateTime.Now
            _editdate = DateTime.Now
            sql = String.Format("Insert into PLANPARALLELRELEASESTRATEGY (PARALLELSTRATID,STRATEGYID,PRIORITY,HANDELINGUNITTYPE," & _
            "ROUTE,STAGINGLANE,CUSTOMER,SHIPMENT,ONHUTYPE,NUMLISTS,PICKREGION,ADDDATE,ADDUSER,EDITDATE,EDITUSER) values " & _
            "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", Made4Net.Shared.FormatField(_parallelstratid), _
            Made4Net.Shared.FormatField(_strategyid), Made4Net.Shared.FormatField(_priority), Made4Net.Shared.FormatField(_handelingunittype), _
            Made4Net.Shared.FormatField(_route), Made4Net.Shared.FormatField(_staginglane), Made4Net.Shared.FormatField(_customer), _
            Made4Net.Shared.FormatField(_shipment), Made4Net.Shared.FormatField(_onhutype), Made4Net.Shared.FormatField(_numlists), _
            Made4Net.Shared.FormatField(_pickregion), Made4Net.Shared.FormatField(_adddate), Made4Net.Shared.FormatField(_adduser), _
            Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_edituser))
        End If
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Create()
        If Exists(_parallelstratid) Then
            Throw New M4NException(New Exception, "Parallel release strategy detail already exists.", "Parallel release strategy detail already exists.")
        End If
        Save()
    End Sub

    Public Sub Update()
        If Not Exists(_parallelstratid) Then
            Throw New M4NException(New Exception, "Parallel release strategy detail does not exists.", "Parallel release strategy detail does not exists.")
        End If
        Save()
    End Sub

    Public Shared Function Exists(ByVal pParallelPolicyID As Integer) As Boolean
        Dim sql As String = String.Format("Select count(1) from PLANPARALLELRELEASESTRATEGY where PARALLELSTRATID={0}", pParallelPolicyID)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function Delete()
        Dim sql As String = String.Format("Delete from PLANPARALLELRELEASESTRATEGY where {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Function

#End Region

End Class

#End Region

#Region "Parallel Release Strategy Collection"

<CLSCompliant(False)> Public Class ParallelReleaseStrategyCollection
    Implements ICollection

#Region "Variables"

    Protected _stratreleaselines As ArrayList
    Protected _planstrategyid As String

#End Region

#Region "Constructor"

    Public Sub New(ByVal PlanStrategyId As String)
        _stratreleaselines = New ArrayList
        _planstrategyid = PlanStrategyId
        Load()
    End Sub

#End Region

#Region "Properties"

    Default Public Property Item(ByVal index As Int32) As ParallelReleaseStrategyDetail
        Get
            Return CType(_stratreleaselines(index), ParallelReleaseStrategyDetail)
        End Get
        Set(ByVal Value As ParallelReleaseStrategyDetail)
            _stratreleaselines(index) = Value
        End Set
    End Property

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim Sql As String = String.Format("Select * from PLANPARALLELRELEASESTRATEGY Where strategyid='{0}' order by priority asc", _planstrategyid)

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)

        Dim stDetail As ParallelReleaseStrategyDetail
        For Each dr In dt.Rows
            stDetail = New ParallelReleaseStrategyDetail(dr)
            Add(stDetail)
        Next

    End Sub

    Public Sub DeleteAll()
        Dim sql As String = String.Format("Delete from PLANPARALLELRELEASESTRATEGY where strategyID={0}", Made4Net.Shared.FormatField(_planstrategyid))
        DataInterface.RunSQL(sql)
        Me.Clear()
    End Sub

#End Region

#Region "Overrides"

    Public Function Add(ByVal value As ParallelReleaseStrategyDetail) As Integer
        Return _stratreleaselines.Add(value)
    End Function

    Public Sub Clear()
        _stratreleaselines.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As ReleaseStrategyDetail)
        _stratreleaselines.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As ReleaseStrategyDetail)
        _stratreleaselines.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _stratreleaselines.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _stratreleaselines.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _stratreleaselines.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _stratreleaselines.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _stratreleaselines.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _stratreleaselines.GetEnumerator()
    End Function

#End Region

End Class


#End Region