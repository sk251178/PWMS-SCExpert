Imports System.Collections
Imports Made4Net.DataAccess


'Commented for RWMS-1185 Start
'Imports NCR.GEMS.Core.DataCaching
'Imports NCR.GEMS.WMS.Core.WmsQueryCaching
'Commented for RWMS-1185 End

#Region "LoadAttribute"

<CLSCompliant(False)> Public Class LoadAttribute
    Inherits AttributeBase

#Region "Constructors"

    Public Sub New(ByVal sAttributeName As String)
        MyBase.New(sAttributeName, AttributeBase.AttributeLevel.LoadAttribute)
    End Sub

    Public Sub New(ByVal drAttribute As DataRow)
        MyBase.New(drAttribute)
    End Sub

#End Region

#Region "Methods"

#End Region

End Class

#End Region

#Region "SkuClassLoadAttribute"

<CLSCompliant(False)> Public Class SkuClassLoadAttribute
    Inherits LoadAttribute


#Region "Enums"

    Public Enum CaptureType
        NoCapture
        Capture
        Required
    End Enum

    Public Enum EvaluationType
        Receiving
        Picking
        Verifying
        Counting
    End Enum

#End Region

#Region "Variables"

    Protected _skuclass As SkuClass

    Protected _capatreceiving As CaptureType
    Protected _recvalidator As String               'Protected _recvalidator As IReceivingAttributeValidator

    Protected _capatpicking As CaptureType
    Protected _pckvalidator As String               'Protected _pckvalidator As IPickingAttributeValidator

    Protected _capatverification As CaptureType
    Protected _vervalidator As String               'Protected _vervalidator As IVerificationAttributeValidator

    Protected _capatcounting As CaptureType
    Protected _cntvalidator As String               'Protected _cntvalidator As ICountingAttributeValidator

#End Region

#Region "Properties"

    Public Property CaptureAtReceiving() As CaptureType
        Get
            Return _capatreceiving
        End Get
        Set(ByVal Value As CaptureType)
            _capatreceiving = Value
        End Set
    End Property

    Public Property CaptureAtPicking() As CaptureType
        Get
            Return _capatpicking
        End Get
        Set(ByVal Value As CaptureType)
            _capatpicking = Value
        End Set
    End Property

    Public Property CaptureAtVerification() As CaptureType
        Get
            Return _capatverification
        End Get
        Set(ByVal Value As CaptureType)
            _capatverification = Value
        End Set
    End Property

    Public Property CaptureAtCounting() As CaptureType
        Get
            Return _capatcounting
        End Get
        Set(ByVal Value As CaptureType)
            _capatcounting = Value
        End Set
    End Property

    Public Property VerificationValidator() As String
        Get
            Return _vervalidator
        End Get
        Set(ByVal Value As String)
            _vervalidator = Value
        End Set
    End Property

    Public Property CountingValidator() As String
        Get
            Return _cntvalidator
        End Get
        Set(ByVal Value As String)
            _cntvalidator = Value
        End Set
    End Property

    Public Property ReceivingValidator() As String
        Get
            Return _recvalidator
        End Get
        Set(ByVal Value As String)
            _recvalidator = Value
        End Set
    End Property

    Public Property PickingValidator() As String
        Get
            Return _pckvalidator
        End Get
        Set(ByVal Value As String)
            _pckvalidator = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal oSkuClass As SkuClass, ByVal sAttributeName As String)
        MyBase.New(sAttributeName)
        _skuclass = oSkuClass
        'Dim sql As String = String.Format("SELECT RECEIVINGCAPTURE, RECEIVINGVALIDATOR, PICKINGCAPTURE, PICKINGVALIDATOR FROM SKUCLSLOADATT WHERE CLASSNAME={0} AND ATTRIBUTENAME={1}", Made4Net.Shared.Util.FormatField(_skuclass.ClassName), Made4Net.Shared.Util.FormatField(sAttributeName))
        Dim sql As String = String.Format("SELECT * FROM SKUCLSLOADATT WHERE CLASSNAME={0} AND ATTRIBUTENAME={1}", Made4Net.Shared.Util.FormatField(_skuclass.ClassName), Made4Net.Shared.Util.FormatField(sAttributeName))
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            dt.Dispose()
            Throw New ApplicationException("Load Attribute does not exists")
        End If
        clsLoad(dt.Rows(0))
        dt.Dispose()
    End Sub

    Public Sub New(ByVal oSkuClass As SkuClass, ByVal attDataRow As DataRow)
        MyBase.New(attDataRow)
        clsLoad(attDataRow)
    End Sub
#End Region

#Region "Methods"

    Protected Sub clsLoad(ByVal dr As DataRow)
        If Not dr.IsNull("RECEIVINGCAPTURE") Then _capatreceiving = CaptureTypeFromString(dr("RECEIVINGCAPTURE"))
        If Not dr.IsNull("RECEIVINGVALIDATOR") Then _recvalidator = dr("RECEIVINGVALIDATOR")
        If Not dr.IsNull("PICKINGCAPTURE") Then _capatpicking = CaptureTypeFromString(dr("PICKINGCAPTURE"))
        If Not dr.IsNull("PICKINGVALIDATOR") Then _pckvalidator = dr("PICKINGVALIDATOR")
        If Not dr.IsNull("VERIFICATIONCAPTURE") Then _capatverification = CaptureTypeFromString(dr("VERIFICATIONCAPTURE"))
        If Not dr.IsNull("VERIFICATIONVALIDATOR") Then _vervalidator = dr("VERIFICATIONVALIDATOR")
        If Not dr.IsNull("COUNTINGCAPTURE") Then _capatcounting = CaptureTypeFromString(dr("COUNTINGCAPTURE"))
        If Not dr.IsNull("COUNTINGVALIDATOR") Then _cntvalidator = dr("COUNTINGVALIDATOR")
    End Sub

    Protected Function CaptureTypeToString(ByVal eCapType As CaptureType) As String
        Select Case eCapType
            Case CaptureType.NoCapture
                Return "NoCapture"
            Case CaptureType.Capture
                Return "Capture"
            Case CaptureType.Required
                Return "Required"
        End Select
    End Function

    Protected Function CaptureTypeFromString(ByVal sCapType As String) As CaptureType
        Select Case sCapType.ToLower
            Case "nocapture"
                Return CaptureType.NoCapture
            Case "capture"
                Return CaptureType.Capture
            Case "required"
                Return CaptureType.Required
        End Select
    End Function

    Public Function Evaluate(ByVal eEvalType As EvaluationType, ByVal vals As Made4Net.DataAccess.Collections.GenericCollection) As String
        Dim expr As String
        Select Case eEvalType
            Case EvaluationType.Receiving
                expr = _recvalidator
            Case EvaluationType.Picking
                expr = _pckvalidator
            Case EvaluationType.Counting
                expr = _cntvalidator
            Case EvaluationType.Verifying
                expr = _vervalidator
        End Select
        If expr = String.Empty Then
            Return ""
        End If
        Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
        exprEval.FieldValues = vals
        Return exprEval.Evaluate(expr)
    End Function

#End Region

End Class

#End Region

#Region "SkuClassLoadAttributeCollection"

<CLSCompliant(False)> Public Class SkuClassLoadAttributeCollection
    Implements ICollection

#Region "Variables"
    Protected _clsskuloadattributes As System.Collections.ArrayList
    Protected _skuclass As SkuClass
#End Region

#Region "Properties"
    Default Public ReadOnly Property Item(ByVal index As Int32) As SkuClassLoadAttribute
        Get
            Return CType(_clsskuloadattributes.Item(index), SkuClassLoadAttribute)
        End Get
    End Property
    Public Property IsWeightCaptureRequiredAtPicking() As Boolean
    Default Public ReadOnly Property Item(ByVal sAttributeName As String) As SkuClassLoadAttribute
        Get
            For idx As Int32 = 0 To _clsskuloadattributes.Count - 1
                If CType(_clsskuloadattributes(idx), SkuClassLoadAttribute).Name.ToLower = sAttributeName.ToLower Then
                    Return CType(_clsskuloadattributes.Item(idx), SkuClassLoadAttribute)
                End If
            Next
            Return Nothing
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New(ByVal oSkuClass As SkuClass)
        _skuclass = oSkuClass
        _clsskuloadattributes = New ArrayList
        Load()
    End Sub
#End Region

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _clsskuloadattributes.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _clsskuloadattributes.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _clsskuloadattributes.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _clsskuloadattributes.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _clsskuloadattributes.GetEnumerator
    End Function

#End Region

#Region "Methods"
    Protected Sub Load()
        Dim skuClassName As String = Made4Net.Shared.Util.FormatField(_skuclass.ClassName)
        Dim sql As String = String.Format("SELECT SKUCLSLOADATT.*, INVENTORYATTRIBUTELIST.NAME, INVENTORYATTRIBUTELIST.DESCRIPTION, INVENTORYATTRIBUTELIST.TYPE " & _
            "FROM SKUCLSLOADATT JOIN INVENTORYATTRIBUTELIST ON SKUCLSLOADATT.ATTRIBUTENAME = INVENTORYATTRIBUTELIST.NAME " & _
            "where skuclsloadatt.classname={0}", skuClassName)

        'Added for RWMS-1185 Start
        Dim dataTable As DataTable
        dataTable = New DataTable()
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dataTable)
        For Each dataRow As DataRow In dataTable.Rows
            Dim oAtt As SkuClassLoadAttribute = New SkuClassLoadAttribute(_skuclass, dataRow)
            If (oAtt.Name.ToUpper = "WEIGHT" OrElse oAtt.Name.ToUpper = "WGT") AndAlso (oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                IsWeightCaptureRequiredAtPicking = True
            End If
            Me.Add(oAtt)
        Next
        dataTable.Dispose()
        'Added for RWMS-1185 End

        'Commented for RWMS-1185 Start
        'If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKULoadAttribute, skuClassName) Then
        '    dataTable = New DataTable()
        '    Made4Net.DataAccess.DataInterface.FillDataset(sql, dataTable)
        '    For Each dataRow As DataRow In dataTable.Rows
        '        Me.Add(New SkuClassLoadAttribute(_skuclass, dataRow))
        '    Next
        '    dataTable.Dispose()
        'Else
        '    dataTable = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKULoadAttribute, skuClassName)
        '    If IsNothing(dataTable) Then
        '        dataTable = New DataTable()
        '        Made4Net.DataAccess.DataInterface.FillDataset(sql, dataTable)
        '        GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKULoadAttribute, skuClassName, dataTable, sql)
        '    End If

        '    For Each dataRow As DataRow In dataTable.Rows
        '        Me.Add(New SkuClassLoadAttribute(_skuclass, dataRow))
        '    Next
        'End If
        'Commented for RWMS-1185 Start



  End Sub

    Public Function Add(ByVal oLoadAttribute As SkuClassLoadAttribute) As Int32
        Return _clsskuloadattributes.Add(oLoadAttribute)
    End Function
#End Region

End Class

#End Region

'#Region "LoadAttributes"

'<CLSCompliant(False)>    Public Class LoadAttributes
'    Inherits LoadAttribute

'#Region "Variables"
'    Protected _load As Load
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
'    Public Sub New(ByVal sLoadid As String, ByVal sAttributeName As String)
'        MyBase.New(sAttributeName)
'        _load = New Load(sLoadid)
'        Load()
'    End Sub

'    Public Sub New(ByVal oLoad As Load, ByVal sAttributeName As String)
'        MyBase.New(sAttributeName)
'        _load = oLoad
'        Load()
'    End Sub
'#End Region

'#Region "Methods"
'    Protected Sub Load()
'        Dim sql As String = String.Format("Select attributevalue from loadatt where loadid = {0} and attributename = {1}", Made4Net.Shared.Util.FormatField(_load.LOADID), Made4Net.Shared.Util.FormatField(_attname))
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

'#Region "LoadAttributeCollection"

'<CLSCompliant(False)>    Public Class LoadAttributeCollection
'    Implements ICollection

'#Region "Variables"
'    Protected _loadattributes As System.Collections.ArrayList
'    Protected _load As Load
'#End Region

'#Region "Properties"
'    Default Public Property Item(ByVal index As Int32) As LoadAttributes
'        Get
'            Return _loadattributes(index)
'        End Get
'        Set(ByVal Value As LoadAttributes)
'            _loadattributes(index) = Value
'        End Set
'    End Property
'    Default Public ReadOnly Property Item(ByVal sAttribuetName As String) As LoadAttributes
'        Get
'            For Each oAtt As LoadAttributes In Me
'                If oAtt.Name.ToLower = sAttribuetName Then
'                    Return oAtt
'                End If
'            Next
'            Return Nothing
'        End Get
'    End Property
'#End Region

'#Region "Constructor"
'    Public Sub New(ByVal oLoad As Load)
'        _load = oLoad
'        _loadattributes = New System.Collections.ArrayList
'        Load()
'    End Sub
'    Public Sub New(ByVal sLoadId As String)
'        _load = New Load(sLoadId)
'        _loadattributes = New System.Collections.ArrayList
'        Load()
'    End Sub
'#End Region

'#Region "Overrides"

'    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
'        _loadattributes.CopyTo(array, index)
'    End Sub

'    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
'        Get
'            Return _loadattributes.Count
'        End Get
'    End Property

'    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
'        Get
'            Return _loadattributes.IsSynchronized()
'        End Get
'    End Property

'    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
'        Get
'            Return _loadattributes.SyncRoot
'        End Get
'    End Property

'    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
'        Return _loadattributes.GetEnumerator
'    End Function

'#End Region

'#Region "Methods"
'    Protected Sub Load()
'        Dim oSku As New SKU(_load.CONSIGNEE, _load.SKU)
'        Dim sql As String = String.Format("Select attributename from skuclsloadatt where classname={0}", Made4Net.Shared.Util.FormatField(oSku.CLASSNAME))
'        oSku = Nothing
'        Dim dt As New DataTable
'        DataInterface.FillDataset(sql, dt)
'        For Each dr As DataRow In dt.Rows
'            Add(New LoadAttributes(_load, dr("attributename")))
'        Next
'        dt.Dispose()
'    End Sub
'    Public Function Add(ByVal oLoadAttributes As LoadAttributes) As Int32
'        Return _loadattributes.Add(oLoadAttributes)
'    End Function
'#End Region


'End Class

'#End Region