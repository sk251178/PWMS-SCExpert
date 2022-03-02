<CLSCompliant(False)> Public Class ReleasePickListCollection
    Inherits PicklistList

#Region "Constructors"

    Public Sub New(ByVal pcklst As Picklist, ByVal parrelstrat As ParallelReleaseStrategyDetail, ByVal pTaskSubType As String)
        Dim oh As New OutboundOrderHeader(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId)
        Dim od As New OutboundOrderHeader.OutboundOrderDetail(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId, pcklst.Lines(0).OrderLine)
        _company = oh.TARGETCOMPANY
        _companytype = oh.COMPANYTYPE
        _consignee = oh.CONSIGNEE
        _handelingunittype = pcklst.HandelingUnitType
        _pickregion = pcklst.Lines(0).PickRegion
        _picktype = pcklst.PickType
        _tasksubtype = pTaskSubType
        _prs = parrelstrat
        _route = od.ROUTE
        _shipment = od.Shipment
        _staginglane = oh.STAGINGLANE
        _picklists = New ArrayList
        Add(pcklst)
    End Sub

#End Region

#Region "Variables"

    Protected _prs As ParallelReleaseStrategyDetail
    Protected _company As String
    Protected _companytype As String
    Protected _consignee As String
    Protected _handelingunittype As String
    Protected _picktype As String
    Protected _route As String
    Protected _staginglane As String
    Protected _shipment As String
    Protected _pickregion As String

    Protected _tasksubtype As String

#End Region

#Region "Properties"

    Public Property TaskSubType() As String
        Get
            Return _tasksubtype
        End Get
        Set(ByVal value As String)
            _tasksubtype = value
        End Set
    End Property
#End Region

#Region "Methods"

    Public Function CanPlace(ByVal pcklst As Picklist, ByVal parrelstrat As ParallelReleaseStrategyDetail) As Boolean
        If pcklst.StrategyId <> _prs.StrategyId Then
            Return False
        End If

        If Not _prs.HandelingUnitType Is Nothing And Not _prs.HandelingUnitType = String.Empty Then
            If _handelingunittype <> pcklst.HandelingUnitType Then Return False
        End If

        'If Not _prs.PickType Is Nothing And Not _prs.PickType = String.Empty Then
        '    If _picktype <> pcklst.PickType Then Return False
        'End If

        Dim oh As New OutboundOrderHeader(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId)
        Dim od As New OutboundOrderHeader.OutboundOrderDetail(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId, pcklst.Lines(0).OrderLine)

        If Me.Count >= _prs.NumLists Or Me.Count >= parrelstrat.NumLists Then
            Return False
        End If

        If _prs.Customer Or parrelstrat.Customer Then
            If oh.CONSIGNEE <> _consignee Or oh.TARGETCOMPANY <> _company Or oh.COMPANYTYPE <> _companytype Then
                Return False
            End If
        End If

        If _prs.Route Or parrelstrat.Route Then
            If od.ROUTE <> _route Then Return False
        End If

        If _prs.StagingLane Or parrelstrat.StagingLane Then
            If oh.STAGINGLANE <> _staginglane Then Return False
        End If

        If _prs.Shipment Or parrelstrat.Shipment Then
            If od.Shipment <> _shipment Then Return False
        End If

        If _prs.PickRegion Or parrelstrat.PickRegion Then
            If pcklst.Lines(0).PickRegion <> _pickregion Then Return False
        End If

        Return True
    End Function

    Public Sub Release()
        _prs.Release(Me)
    End Sub

#End Region

End Class
