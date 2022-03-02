Imports System.Collections
Imports Made4Net.DataAccess

#Region "SkuClassAttribute"

<CLSCompliant(False)> Public Class SkuClassAttribute
    Inherits AttributeBase

#Region "Variables"
    Protected _skuclass As SkuClass
#End Region

#Region "Constructors"
    Public Sub New(ByVal oSkuClass As SkuClass, ByVal sAttributeName As String)
        MyBase.New(sAttributeName, AttributeBase.AttributeLevel.SkuAttribute)
        _skuclass = oSkuClass
    End Sub
    Public Sub New(ByVal oSkuClass As SkuClass, ByVal drAttribute As DataRow)
        MyBase.New(drAttribute)
        _skuclass = oSkuClass
    End Sub
#End Region

#Region "Methods"

#End Region

End Class

#End Region

#Region "SkuClassAttributeCollection"

<CLSCompliant(False)> Public Class SkuClassAttributeCollection
    Implements ICollection

#Region "Variables"
    Protected _clsskuattributes As System.Collections.ArrayList
    Protected _skucls As SkuClass
#End Region

#Region "Properties"
    Default Public ReadOnly Property Item(ByVal index As Int32) As SkuClassAttribute
        Get
            Return CType(_clsskuattributes.Item(index), SkuClassAttribute)
        End Get
    End Property
    Default Public ReadOnly Property Item(ByVal sAttributeName As String) As SkuClassAttribute
        Get
            For idx As Int32 = 0 To _clsskuattributes.Count - 1
                If CType(_clsskuattributes(idx), SkuClassAttribute).Name.ToLower = sAttributeName.ToLower Then
                    Return CType(_clsskuattributes.Item(idx), SkuClassAttribute)
                End If
            Next
            Return Nothing
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New(ByVal oSkuClass As SkuClass)
        _skucls = oSkuClass
        _clsskuattributes = New ArrayList
        Load()
    End Sub
#End Region

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _clsskuattributes.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _clsskuattributes.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _clsskuattributes.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _clsskuattributes.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _clsskuattributes.GetEnumerator
    End Function

#End Region

#Region "Methods"
    Protected Sub Load()
        Dim sql As String = String.Format("select * from SKUATTRIBUTELIST where name in (select attributename from skuclsatt where classname = {0})", Made4Net.Shared.Util.FormatField(_skucls.ClassName))
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Me.Add(New SkuClassAttribute(_skucls, dr))
        Next
    End Sub

    Public Function Add(ByVal oSkuAttribute As SkuClassAttribute) As Int32
        Return _clsskuattributes.Add(oSkuAttribute)
    End Function
#End Region

End Class

#End Region

'#Region "SkuAttributes"

'<CLSCompliant(False)>    Public Class SkuAttributes
'    Inherits SkuAttribute

'#Region "Variables"
'    Protected _sku As SKU
'    Protected _value As String
'#End Region

'#Region "Properties"
'    Public Property AttributeValue() As Object
'        Get
'            Return MyBase.getAttributeValue(_value)
'        End Get
'        Set(ByVal Value As Object)
'            _value = Value
'        End Set
'    End Property
'#End Region

'#Region "Constructors"
'    Public Sub New(ByVal sConsignee As String, ByVal sSku As String, ByVal sAttributeName As String)
'        MyBase.New(sAttributeName)
'        _sku = New SKU(sConsignee, sSku)
'        Load()
'    End Sub

'    Public Sub New(ByVal oSku As SKU, ByVal sAttributeName As String)
'        MyBase.New(sAttributeName)
'        _sku = oSku
'        Load()
'    End Sub
'#End Region

'#Region "Methods"
'    Protected Sub Load()
'        Dim sql As String = String.Format("Select attributevalue from skuattributes where consignee = {0} and sku = {1} and attributename = {2}", Made4Net.Shared.Util.FormatField(_sku.CONSIGNEE), Made4Net.Shared.Util.FormatField(_sku.SKU), Made4Net.Shared.Util.FormatField(_attname))
'        Dim dt As New DataTable
'        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
'        If dt.Rows.Count = 0 Then
'            _value = Nothing
'        Else
'            If Not dt.Rows(0).IsNull("attributevalue") Then
'                _value = dt.Rows(0)("attributevalue")
'            Else
'                _value = Nothing
'            End If
'        End If
'        dt.Dispose()
'    End Sub
'#End Region

'End Class

'#End Region

'#Region "SkuAttributesCollection"

'<CLSCompliant(False)>    Public Class SkuAttributesCollection
'    Implements ICollection

'#Region "Variables"
'    Protected _skuattributes As System.Collections.ArrayList
'    Protected _sku As SKU
'#End Region

'#Region "Properties"
'    Default Public Property Item(ByVal index As Int32) As SkuAttributes
'        Get
'            Return _skuattributes(index)
'        End Get
'        Set(ByVal Value As SkuAttributes)
'            _skuattributes(index) = Value
'        End Set
'    End Property
'    Default Public ReadOnly Property Item(ByVal sAttribuetName As String) As SkuAttributes
'        Get
'            For Each oAtt As SkuAttributes In Me
'                If oAtt.Name.ToLower = sAttribuetName Then
'                    Return oAtt
'                End If
'            Next
'            Return Nothing
'        End Get
'    End Property
'#End Region

'#Region "Constructor"
'    Public Sub New(ByVal oSku As SKU)
'        _sku = oSku
'        _skuattributes = New System.Collections.ArrayList
'        Load()
'    End Sub
'    Public Sub New(ByVal sConsignee As String, ByVal sSku As String)
'        _sku = New SKU(sConsignee, sSku)
'        _skuattributes = New System.Collections.ArrayList
'        Load()
'    End Sub
'#End Region

'#Region "Overrides"

'    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
'        _skuattributes.CopyTo(array, index)
'    End Sub

'    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
'        Get
'            Return _skuattributes.Count
'        End Get
'    End Property

'    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
'        Get
'            Return _skuattributes.IsSynchronized()
'        End Get
'    End Property

'    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
'        Get
'            Return _skuattributes.SyncRoot
'        End Get
'    End Property

'    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
'        Return _skuattributes.GetEnumerator
'    End Function

'#End Region

'#Region "Methods"
'    Protected Sub Load()
'        Dim sql As String = String.Format("Select attributename from skuclsatt where classname={0}", Made4Net.Shared.Util.FormatField(_sku.CLASSNAME))
'        Dim dt As New DataTable
'        DataInterface.FillDataset(sql, dt)
'        For Each dr As DataRow In dt.Rows
'            Add(New SkuAttributes(_sku, dr("attributename")))
'        Next
'        dt.Dispose()
'    End Sub
'    Public Function Add(ByVal oSkuAttribute As SkuAttributes) As Int32
'        Return _skuattributes.Add(oSkuAttribute)
'    End Function
'#End Region

'End Class

'#End Region