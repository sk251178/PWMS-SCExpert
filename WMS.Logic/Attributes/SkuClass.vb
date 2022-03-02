
'Commented for RWMS-1185 Start
'Imports NCR.GEMS.Core.DataCaching
'Imports NCR.GEMS.WMS.Core.WmsQueryCaching
'Commented for RWMS-1185 End

<CLSCompliant(False)> Public Class SkuClass

#Region "Variables"

    Protected _classname As String
    Protected _classdesc As String
    Protected _mergevalidationexpression As String
    Protected _skuattvalidationexpression As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    'Collections
    Protected _skuattributes As SkuClassAttributeCollection
    Protected _loadattributes As SkuClassLoadAttributeCollection

#End Region

#Region "Properties"

    Public Property ClassName() As String
        Get
            Return _classname
        End Get
        Set(ByVal Value As String)
            _classname = Value
        End Set
    End Property
    Public ReadOnly Property IsWeightCaptureRequiredAtPicking As Boolean
        Get
            Return If(_loadattributes Is Nothing, False, _loadattributes.IsWeightCaptureRequiredAtPicking)
        End Get
    End Property
    Public Property ClassDescription() As String
        Get
            Return _classdesc
        End Get
        Set(ByVal Value As String)
            _classdesc = Value
        End Set
    End Property

    Public Property SkuAttValidationExpression() As String
        Get
            Return _skuattvalidationexpression
        End Get
        Set(ByVal value As String)
            _skuattvalidationexpression = value
        End Set
    End Property

    Public Property MergeValidationExpression() As String
        Get
            Return _mergevalidationexpression
        End Get
        Set(ByVal Value As String)
            _mergevalidationexpression = Value
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
    Public ReadOnly Property SkuClassAttributes() As SkuClassAttributeCollection
        Get
            Return _skuattributes
        End Get
    End Property
    Public ReadOnly Property LoadAttributes() As SkuClassLoadAttributeCollection
        Get
            Return _loadattributes
        End Get
    End Property
    Public ReadOnly Property CaptureAtReceivingLoadAttributesCount() As Int32
        Get
            Dim cntr As Int32 = 0
            For Each oLoadAttribute As SkuClassLoadAttribute In _loadattributes
                If oLoadAttribute.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAttribute.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                    cntr = cntr + 1
                End If
            Next
            Return cntr
        End Get
    End Property

    Public ReadOnly Property CaptureAtPickingLoadAttributesCount() As Int32
        Get
            Dim cntr As Int32 = 0
            For Each oLoadAttribute As SkuClassLoadAttribute In _loadattributes
                If oLoadAttribute.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAttribute.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                    cntr = cntr + 1
                End If
            Next
            Return cntr
        End Get
    End Property

    Public ReadOnly Property CaptureAtCountingLoadAttributesCount() As Int32
        Get
            Dim cntr As Int32 = 0
            For Each oLoadAttribute As SkuClassLoadAttribute In _loadattributes
                If oLoadAttribute.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAttribute.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Required Then
                    cntr = cntr + 1
                End If
            Next
            Return cntr
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New(ByVal sClassName As String)
        _classname = sClassName
        Load()
    End Sub
#End Region

#Region "Methods"
    Protected Sub Load()
        Dim skuClassName As String = Made4Net.Shared.Util.FormatField(_classname)
        Dim sql As String = String.Format("Select * from skucls where classname = {0}", skuClassName)
        Dim dataTable As New DataTable
        Dim dataRow As DataRow


        'Added for RWMS-1185 Start
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dataTable)
        'Added for RWMS-1185 End

        'Commented for RWMS-1185 Start
        'If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKUCLS, skuClassName) Then
        '    dataTable = New DataTable()
        '    Made4Net.DataAccess.DataInterface.FillDataset(sql, dataTable)
        'Else
        '    dataTable = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKUCLS, skuClassName)
        '    If IsNothing(dataTable) Then
        '        dataTable = New DataTable()
        '        Made4Net.DataAccess.DataInterface.FillDataset(sql, dataTable)
        '        GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKUCLS, skuClassName, dataTable, sql)
        '    End If
        'End If
        'Commented for RWMS-1185 End

        If dataTable.Rows.Count = 0 Then
            dataTable.Dispose()
            'Commented for RWMS-1185 Start
            'GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.SKUCLS, skuClassName, Nothing, sql)
            'Commented for RWMS-1185 End
            Throw New ApplicationException("Sku Class does not exists")
        End If

        dataRow = dataTable.Rows(0)
        If Not dataRow.IsNull("classdescription") Then _classdesc = dataRow("classdescription")
        If Not dataRow.IsNull("mergevalidationexpression") Then _mergevalidationexpression = dataRow("mergevalidationexpression")
        If Not dataRow.IsNull("skuattvalidationexpression") Then _skuattvalidationexpression = dataRow("skuattvalidationexpression")
        If Not dataRow.IsNull("adddate") Then _adddate = dataRow("adddate")
        If Not dataRow.IsNull("adduser") Then _adduser = dataRow("adduser")
        If Not dataRow.IsNull("editdate") Then _editdate = dataRow("editdate")
        If Not dataRow.IsNull("edituser") Then _edituser = dataRow("edituser")

        _loadattributes = New SkuClassLoadAttributeCollection(Me)
        '_skuattributes = New SkuClassAttributeCollection(Me)
    End Sub

    Public Shared Function ExtractLoadAttributes(ByVal dr As DataRow, Optional ByVal isInputSku As Boolean = False) As AttributesCollection
        Dim oSku As SKU
        If isInputSku Then
            oSku = New SKU(dr("Consignee"), dr("INPUTSKU"))
        Else
            oSku = New SKU(dr("Consignee"), dr("SKU"))
        End If

        Return ExtractLoadAttributes(oSku.SKUClass, dr)
    End Function

    Public Shared Function ExtractLoadAttributes(ByVal oSkuClass As SkuClass, ByVal dr As DataRow) As AttributesCollection
        Dim oAttCol As AttributesCollection
        If Not oSkuClass Is Nothing Then
            If oSkuClass.LoadAttributes Is Nothing Then Return Nothing
            If oSkuClass.LoadAttributes.Count = 0 Then Return Nothing
            oAttCol = New AttributesCollection
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                oAttCol.Add(oLoadAtt.Name, dr(oLoadAtt.Name))
            Next
        Else
            Return Nothing
        End If
        Return oAttCol
    End Function

    Public Shared Function ExtractReceivingAttributes(ByVal dr As DataRow) As AttributesCollection
        Dim oSku As New SKU(dr("Consignee"), dr("SKU"))
        Return ExtractReceivingAttributes(oSku.SKUClass, dr)
    End Function

    Public Shared Function ExtractReceivingAttributes(ByVal oSkuClass As SkuClass, ByVal dr As DataRow) As AttributesCollection
        Dim oAttCol As AttributesCollection
        If Not oSkuClass Is Nothing Then
            If oSkuClass.LoadAttributes Is Nothing Then Return Nothing
            If oSkuClass.LoadAttributes.Count = 0 Then Return Nothing
            If oSkuClass.CaptureAtReceivingLoadAttributesCount = 0 Then Return Nothing
            oAttCol = New AttributesCollection
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                    oAttCol.Add(oLoadAtt.Name, dr(oLoadAtt.Name))
                End If
            Next
        Else
            Return Nothing
        End If
        Return oAttCol
    End Function

    Public Shared Function ExtractPickingAttributes(ByVal dr As DataRow) As AttributesCollection
        Dim oSku As New SKU(dr("Consignee"), dr("SKU"))
        Return ExtractPickingAttributes(oSku.SKUClass, dr)
    End Function

    Public Shared Function ExtractPickingAttributes(ByVal oSkuClass As SkuClass, ByVal dr As DataRow) As AttributesCollection
        Dim oAttCol As AttributesCollection
        If Not oSkuClass Is Nothing Then
            If oSkuClass.LoadAttributes Is Nothing Then Return Nothing
            If oSkuClass.LoadAttributes.Count = 0 Then Return Nothing
            If oSkuClass.CaptureAtPickingLoadAttributesCount = 0 Then Return Nothing
            oAttCol = New AttributesCollection
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                If oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                    oAttCol.Add(oLoadAtt.Name, dr(oLoadAtt.Name))
                End If
            Next
        Else
            Return Nothing
        End If
        Return oAttCol
    End Function

#End Region

End Class
