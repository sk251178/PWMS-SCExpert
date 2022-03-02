Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class PicklistList
    Implements ICollection

#Region "Variables"

    Protected _Wave As Wave
    Protected _picklists As ArrayList
    Protected _PickList As PickList

#End Region

#Region "Properties"

    Public ReadOnly Property Wave() As String
        Get
            Return _Wave.WAVE
        End Get
    End Property

    Public ReadOnly Property PickList() As PickList
        Get
            Return _PickList
        End Get
    End Property

    Default Public Property Item(ByVal index As Int32) As PickList
        Get
            Return _picklists(index)
        End Get
        Set(ByVal Value As PickList)
            _picklists(index) = Value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        _picklists = New ArrayList
    End Sub

    Public Sub New(ByVal Wave As String, Optional ByVal oLogger As LogHandler = Nothing)
        _Wave = New WMS.Logic.Wave(Wave)
        _picklists = New ArrayList
        LoadWavePicklists(oLogger)
    End Sub

    Public Sub New(ByVal Wave As Wave, Optional ByVal oLogger As LogHandler = Nothing)
        _Wave = Wave
        _picklists = New ArrayList
        LoadWavePicklists(oLogger)
    End Sub

    Public Sub New(ByVal PickList As PickList, Optional ByVal oLogger As LogHandler = Nothing)
        _PickList = PickList
        _picklists = New ArrayList
        AddPickList(_PickList)
        If Not oLogger Is Nothing Then
            oLogger.Write("Adding Picklist " & PickList.PicklistID & " To Collection.")
        End If
    End Sub

#End Region

#Region "Methods"

    Public Sub LoadWavePicklists(Optional ByVal oLogger As LogHandler = Nothing)
        Dim pck As PickList
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("select picklist from pickheader where wave='{0}' and status='{1}'", _Wave.WAVE, WMS.Lib.Statuses.Picklist.PLANNED)
        DataInterface.FillDataset(sql, dt)
        If Not oLogger Is Nothing Then
            oLogger.Write("Getting all Wave Pick Lists (Total of " & dt.Rows.Count & "). PickList ID's:")
        End If
        For Each dr In dt.Rows
            pck = New PickList(dr("picklist"))
            'If Not oLogger Is Nothing Then
            '    oLogger.Write(dr("picklist"))
            'End If
            _picklists.Add(pck)
        Next
        dt.Dispose()
    End Sub

    Public Sub AddPickList(ByVal pck As PickList)
        _picklists.Add(pck)
    End Sub

    Public Overridable Function Add(ByVal pcklst As PickList) As Int32
        Return _picklists.Add(pcklst)
    End Function

    'Public Sub LoadOrderPicklists()
    '    Dim pck As Picklist
    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    Dim sql As String = String.Format("select distinct picklist from pickdetail where consignee='{0}' and orderid='{1}'", _Order.CONSIGNEE, _Order.ORDERID)
    '    DataInterface.FillDataset(sql, dt)
    '    For Each dr In dt.Rows
    '        pck = New Picklist(dr("picklist"))
    '        If pck.Status = WMS.Lib.Statuses.Picklist.PLANNED Then
    '            _picklists.Add(pck)
    '        End If
    '    Next
    '    dt.Dispose()
    'End Sub

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _picklists.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _picklists.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _picklists.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _picklists.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _picklists.GetEnumerator()
    End Function

#End Region

#End Region

End Class
