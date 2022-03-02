<CLSCompliant(False)> Public MustInherit Class AttributeBase

#Region "Enum"
    Protected Enum AttributeLevel
        SkuAttribute
        LoadAttribute
    End Enum
#End Region

#Region "Variables"
    Protected _attname As String
    Protected _attdesc As String
    Protected _atttype As AttributeType
#End Region

#Region "Properties"
    Public Property Name() As String
        Get
            Return _attname
        End Get
        Set(ByVal Value As String)
            _attname = Value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return _attdesc
        End Get
        Set(ByVal Value As String)
            _attdesc = Value
        End Set
    End Property
    Public Property [Type]() As AttributeType
        Get
            Return _atttype
        End Get
        Set(ByVal Value As AttributeType)
            _atttype = Value
        End Set
    End Property
#End Region

#Region "Constructors"
    Protected Sub New(ByVal sAttributeName As String, ByVal eAttributeLevel As AttributeLevel)
        _attname = sAttributeName
        Dim tblname As String
        Select Case eAttributeLevel
            Case AttributeLevel.LoadAttribute
                tblname = "inventoryattributelist"
            Case AttributeLevel.SkuAttribute
                tblname = "skuattributelist"
        End Select
        Dim sql As String = String.Format("Select * from {0} where name = {1}", tblname, Made4Net.Shared.Util.FormatField(_attname))
        Dim dt As New DataTable
        Dim dr As DataRow
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Attribute does not exists")
        End If
        dr = dt.Rows(0)
        Load(dr)
        dt.Dispose()
    End Sub
    Protected Sub New(ByVal drAttribute As DataRow)
        Load(drAttribute)
    End Sub
#End Region

#Region "Methods"

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("name") Then _attname = dr("name")
        If Not dr.IsNull("description") Then _attdesc = dr("description")
        If Not dr.IsNull("type") Then _atttype = AttributeTypeFromString(dr("type"))
    End Sub

    Public Function AttributeTypeToString(ByVal AttType As AttributeType) As String
        Select Case AttType
            Case AttributeType.Boolean
                Return "Boolean"
            Case AttributeType.Decimal
                Return "Decimal"
            Case AttributeType.Integer
                Return "Integer"
            Case AttributeType.String
                Return "String"
            Case AttributeType.DateTime
                Return "DateTime"
        End Select
    End Function

    Public Function AttributeTypeFromString(ByVal AttType As String) As AttributeType
        Select Case AttType.ToLower
            Case "boolean"
                Return AttributeType.Boolean
            Case "decimal"
                Return AttributeType.Decimal
            Case "integer"
                Return AttributeType.Integer
            Case "string"
                Return AttributeType.String
            Case "datetime"
                Return AttributeType.DateTime
        End Select
    End Function

    Protected Function getValidator(ByVal sValidatorName As String) As Object
        Dim sql As String = String.Format("Select * from attributevalidator where name = {0}", Made4Net.Shared.Util.FormatField(sValidatorName))
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim validator As Object
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            validator = Nothing
        Else
            dr = dt.Rows(0)
            Try
                validator = Made4Net.Shared.Reflection.CreateInstance(dr("ASSEMBLYDLL"), dr("CLASSNAME"), Nothing, dr("ASSEMBLYDLLPATH"))
            Catch ex As Exception
                dt.Dispose()
                Return Nothing
                'Throw New ApplicationException(String.Format("Error Creating Attribute Validator - {0}", sValidatorName))
            End Try
        End If
        dt.Dispose()
        Return validator
    End Function

    '    Public Function getAttributeValue(ByVal sValue As String) As Object
    '        If sValue Is Nothing Or sValue = "" Then Return Nothing
    '        Select Case _atttype
    '            Case AttributeType.Boolean
    '                Return Convert.ToBoolean(sValue)
    '            Case AttributeType.DateTime
    '                Return Convert.ToDateTime(sValue)
    '            Case AttributeType.Decimal
    '                Return Convert.ToDecimal(sValue)
    '            Case AttributeType.Integer
    '                Return Convert.ToInt32(sValue)
    '            Case Else
    '                Return sValue
    '        End Select
    '    End Function
#End Region

End Class

#Region "Enums"
Public Enum AttributeType
    [String]
    [Integer]
    [Decimal]
    [Boolean]
    [DateTime]
End Enum
#End Region