<CLSCompliant(False)> Public Class AllocationCollection
    Implements ICollection

    Protected _allocs As ArrayList
    Protected _allocatedUnits As Double = 0

#Region "Constructor"
    Public Sub New()
        _allocs = New ArrayList
    End Sub
#End Region

#Region "Properies"
    Public ReadOnly Property AllocatedUnits() As Double
        Get
            Return _allocatedUnits
        End Get
    End Property
#End Region

#Region "Overrides"

    Public Sub AddRange(ByVal newAlloc As AllocationCollection)
        _allocatedUnits = _allocatedUnits + newAlloc.AllocatedUnits
        _allocs.AddRange(newAlloc)
    End Sub

    Public Function Add(ByVal value As Allocate) As Double
        _allocatedUnits = _allocatedUnits + value.Units
        Return _allocs.Add(value)
    End Function

    Public Sub Clear()
        _allocs.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As Allocate)
        _allocs.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As Allocate)
        _allocs.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _allocs.RemoveAt(index)
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _allocs.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _allocs.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _allocs.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _allocs.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _allocs.GetEnumerator()
    End Function

#End Region

End Class

<CLSCompliant(False)> Public Class Allocate

#Region "Variables"

    Protected _loadid As String
    Protected _location As String
    Protected _pickregion As String
    Protected _uom As String
    Protected _units As Decimal
    Protected _uomunits As Decimal
    Protected _unitspermeasure As Decimal
    Protected _sortorder As String
    Protected _picktype As String
    Protected _warehousearea As String
    Protected _ispicklocalloc As Boolean
    Protected _isoveralloc As Boolean
    Protected _allocationquanta As Decimal
    '' Used in case of substitute SKU
    Protected _subSKUconversionUnits As Decimal = 1

#End Region

#Region "Properties"

    Public Property LoadId() As String
        Get
            Return _loadid
        End Get
        Set(ByVal Value As String)
            _loadid = Value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return _location
        End Get
        Set(ByVal Value As String)
            _location = Value
        End Set
    End Property

    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property

    Public Property PickRegion() As String
        Get
            Return _pickregion
        End Get
        Set(ByVal Value As String)
            _pickregion = Value
        End Set
    End Property

    Public Property SortOrder() As String
        Get
            Return _sortorder
        End Get
        Set(ByVal Value As String)
            _sortorder = Value
        End Set
    End Property

    Public Property Units() As Decimal
        Get
            Return _units
        End Get
        Set(ByVal Value As Decimal)
            _units = Value
        End Set
    End Property

    Public Property UOMUnits() As Decimal
        Get
            Return _uomunits
        End Get
        Set(ByVal Value As Decimal)
            _uomunits = Value
        End Set
    End Property

    Public Property UnitsPerMeasure() As Decimal
        Get
            Return _unitspermeasure
        End Get
        Set(ByVal Value As Decimal)
            _unitspermeasure = Value
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

    Public Property WarehouseArea() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
        End Set
    End Property

    Public Property IsPickLocAllocation() As Boolean
        Get
            Return _ispicklocalloc
        End Get
        Set(ByVal Value As Boolean)
            _ispicklocalloc = Value
        End Set
    End Property

    Public Property IsOverAllocation() As Boolean
        Get
            Return _isoveralloc
        End Get
        Set(ByVal Value As Boolean)
            _isoveralloc = Value
        End Set
    End Property

    Public Property AllocationQuanta() As Decimal
        Get
            Return _allocationquanta
        End Get
        Set(ByVal Value As Decimal)
            _allocationquanta = Value
        End Set
    End Property

    Public Property SubSKUConversionUnits() As Decimal
        Get
            Return _subSKUconversionUnits
        End Get
        Set(ByVal value As Decimal)
            _subSKUconversionUnits = value
        End Set
    End Property

#End Region

End Class
