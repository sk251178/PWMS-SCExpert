Imports Made4Net.DataAccess

'Commented for RWMS-1185 Start
'Imports NCR.GEMS.Core.DataCaching
'Imports NCR.GEMS.WMS.Core.WmsQueryCaching
'Commented for RWMS-1185 End

#Region "InventoryAttributeBase"

'Public MustInherit Class InventoryAttributeBase
<CLSCompliant(False)> Public Class InventoryAttributeBase

#Region "Enums"
    Public Enum AttributeKeyType
        Load
        OutboundOrder
        InboundOrder
        InventoryTransaction
        ASN
        Receipt
        FLOWTHROUGH
        WorkOrder
        WorkOrderBOM
        Counting
    End Enum
#End Region

#Region "Variables"
    Protected _pkeytype As AttributeKeyType
    Protected _pkey1 As String
    Protected _pkey2 As String
    Protected _pkey3 As String
    Protected _attributescollection As AttributesCollection
#End Region

#Region "Properties"
    Public Property PrimaryKeyType() As AttributeKeyType
        Get
            Return _pkeytype
        End Get
        Set(ByVal Value As AttributeKeyType)
            _pkeytype = Value
        End Set
    End Property
    Public Property PrimaryKey1() As String
        Get
            Return _pkey1
        End Get
        Set(ByVal Value As String)
            _pkey1 = Value
        End Set
    End Property
    Public Property PrimaryKey2() As String
        Get
            Return _pkey2
        End Get
        Set(ByVal Value As String)
            _pkey2 = Value
        End Set
    End Property
    Public Property PrimaryKey3() As String
        Get
            Return _pkey3
        End Get
        Set(ByVal Value As String)
            _pkey3 = Value
        End Set
    End Property
    Default Public Property Attribute(ByVal index As Int32) As Object
        Get
            Return _attributescollection(index)
        End Get
        Set(ByVal Value As Object)
            _attributescollection(index) = Value
        End Set
    End Property
    Default Public Property Attribute(ByVal sAttributeName As String) As Object
        Get
            Return _attributescollection(sAttributeName)
        End Get
        Set(ByVal Value As Object)
            _attributescollection(sAttributeName) = Value
        End Set
    End Property
    Protected ReadOnly Property Exists() As Boolean
        Get
            Dim sql As String = String.Format("select count(1) from attribute where {0}", WhereClause)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property
    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" pkeytype={0} and pkey1={1} and pkey2={2} and pkey3={3} ", Made4Net.Shared.Util.FormatField(PrimaryKeyTypeToString()), Made4Net.Shared.Util.FormatField(_pkey1, "''"), Made4Net.Shared.Util.FormatField(_pkey2, "''"), Made4Net.Shared.Util.FormatField(_pkey3, "''"))
        End Get
    End Property
    Public ReadOnly Property Attributes() As AttributesCollection
        Get
            Return _attributescollection
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New(ByVal ePrimaryKeyType As AttributeKeyType, ByVal sPrimaryKey1 As String)
        Me.New(ePrimaryKeyType, sPrimaryKey1, String.Empty, String.Empty)
    End Sub

    Public Sub New(ByVal ePrimaryKeyType As AttributeKeyType)
        _pkeytype = ePrimaryKeyType
        _attributescollection = New AttributesCollection
    End Sub

    Public Sub New(ByVal ePrimaryKeyType As AttributeKeyType, ByVal sPrimaryKey1 As String, ByVal sPrimaryKey2 As String)
        Me.New(ePrimaryKeyType, sPrimaryKey1, sPrimaryKey2, String.Empty)
    End Sub

    Public Sub New(ByVal ePrimaryKeyType As AttributeKeyType, ByVal sPrimaryKey1 As String, ByVal sPrimaryKey2 As String, ByVal sPrimaryKey3 As String)
        _pkeytype = ePrimaryKeyType
        _pkey1 = sPrimaryKey1
        _pkey2 = sPrimaryKey2
        _pkey3 = sPrimaryKey3
        Dim sql As String = String.Format("Select top 1 * from Attribute where PKEYTYPE={0} and PKEY1={1} and PKEY2={2} and PKEY3={3}", Made4Net.Shared.Util.FormatField(PrimaryKeyTypeToString()), Made4Net.Shared.Util.FormatField(_pkey1, ""), Made4Net.Shared.Util.FormatField(_pkey2, ""), Made4Net.Shared.Util.FormatField(_pkey3, ""))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Load(dt.Rows(0))
        Else
            _attributescollection = New AttributesCollection
        End If
        dt.Dispose()
    End Sub

    '<Obsolete("This function is not in use, Please use other constructors.", True)> _
    Public Sub New(ByVal dr As DataRow)
        If Not dr("pkeytype") Is DBNull.Value Then
            _pkeytype = PrimaryKeyTypeFromString(dr("pkeytype"))
            _pkey1 = dr("pkey1")
            _pkey2 = dr("pkey2")
            _pkey3 = dr("pkey3")
            Load(dr)
        End If
    End Sub

    Protected Function PrimaryKeyTypeToString() As String
        Select Case _pkeytype
            Case AttributeKeyType.InboundOrder
                Return "INBOUND"
            Case AttributeKeyType.InventoryTransaction
                Return "INVTRANS"
            Case AttributeKeyType.Load
                Return "LOAD"
            Case AttributeKeyType.OutboundOrder
                Return "OUTBOUND"
            Case AttributeKeyType.ASN
                Return "ASN"
            Case AttributeKeyType.Receipt
                Return "RECEIPT"
            Case AttributeKeyType.FLOWTHROUGH
                Return "FLWTH"
            Case AttributeKeyType.WorkOrder
                Return "WORKORDER"
            Case AttributeKeyType.WorkOrderBOM
                Return "WORKORDERBOM"
            Case AttributeKeyType.Counting
                Return "COUNTING"
        End Select
    End Function

    Protected Function PrimaryKeyTypeFromString(ByVal sAttributeType As String) As AttributeKeyType
        Select Case sAttributeType.ToUpper
            Case "INBOUND"
                Return AttributeKeyType.InboundOrder
            Case "INVTRANS"
                Return AttributeKeyType.InventoryTransaction
            Case "LOAD"
                Return AttributeKeyType.Load
            Case "OUTBOUND"
                Return AttributeKeyType.OutboundOrder
            Case "ASN"
                Return AttributeKeyType.ASN
            Case "RECEIPT"
                Return AttributeKeyType.Receipt
            Case "FLWTH"
                Return AttributeKeyType.FLOWTHROUGH
            Case "WORKORDER"
                Return AttributeKeyType.WorkOrder
            Case "WORKORDERBOM"
                Return AttributeKeyType.WorkOrderBOM
            Case "COUNTING"
                Return AttributeKeyType.Counting
        End Select
    End Function
#End Region

#Region "Methods"
    Protected Sub Load(ByVal dr As DataRow)
        _attributescollection = New AttributesCollection(dr)
    End Sub

    Public Sub Save(ByVal pUser As String)
        Dim sql As String
        If _attributescollection.Count > 0 Then

            'RWMS-2632 RWMS-2630 - get the WMSRDT logger
            Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()

            If Not Exists() Then
                sql = BuildInsertStatement()

                'RWMS-2632 RWMS-2630 START
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" Insert ATTRIBUTE SQL query:{0}", sql))
                End If
                'RWMS-2632 RWMS-2630 END

            Else
                sql = BuildUpdateStatement()

                'RWMS-2632 RWMS-2630 START
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" Update ATTRIBUTE SQL query:{0}", sql))
                End If
                'RWMS-2632 RWMS-2630 END

            End If
            DataInterface.RunSQL(sql)
        End If
    End Sub

    Public Sub Delete()
        Dim sql As String = String.Format("delete from attribute where {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Sub


    Public Shared Function LineExists(ByVal ePrimaryKeyType As AttributeKeyType, ByVal sPrimaryKey1 As String, ByVal sPrimaryKey2 As String, ByVal sPrimaryKey3 As String) As Boolean
        Dim sql As String = String.Format("Select top 1 * from Attribute where PKEYTYPE={0} and PKEY1={1} and PKEY2={2} and PKEY3={3}", _
        Made4Net.Shared.Util.FormatField(PrimaryKeyTypeToString(ePrimaryKeyType)), Made4Net.Shared.Util.FormatField(sPrimaryKey1, ""), _
        Made4Net.Shared.Util.FormatField(sPrimaryKey2, ""), Made4Net.Shared.Util.FormatField(sPrimaryKey3, ""))
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Function PrimaryKeyTypeToString(ByVal ePrimaryKeyType As AttributeKeyType) As String
        Select Case ePrimaryKeyType
            Case AttributeKeyType.InboundOrder
                Return "INBOUND"
            Case AttributeKeyType.InventoryTransaction
                Return "INVTRANS"
            Case AttributeKeyType.Load
                Return "LOAD"
            Case AttributeKeyType.OutboundOrder
                Return "OUTBOUND"
            Case AttributeKeyType.ASN
                Return "ASN"
            Case AttributeKeyType.Receipt
                Return "RECEIPT"
            Case AttributeKeyType.FLOWTHROUGH
                Return "FLWTH"
            Case AttributeKeyType.WorkOrder
                Return "WORKORDER"
            Case AttributeKeyType.WorkOrderBOM
                Return "WORKORDERBOM"
            Case AttributeKeyType.Counting
                Return "COUNTING"
        End Select
    End Function


    Protected Function BuildInsertStatement() As String
        Dim sql As String
        sql = "INSERT INTO ATTRIBUTE(PKEYTYPE,PKEY1,PKEY2,PKEY3,"
        For Each sAttributeName As String In _attributescollection.Keys
            sql = sql & sAttributeName & ","c
        Next
        sql = sql.TrimEnd(","c) & ") VALUES ("
        sql = sql & String.Format("{0},{1},{2},{3},", Made4Net.Shared.Util.FormatField(PrimaryKeyTypeToString()), Made4Net.Shared.Util.FormatField(_pkey1, "''"), Made4Net.Shared.Util.FormatField(_pkey2, "''"), Made4Net.Shared.Util.FormatField(_pkey3, "''"))
        ' Fill all custom attributes to the list
        Dim dt As DataTable = GetAttributeTableSchema()
        For Each oAttributeKey As String In _attributescollection.Keys
            If Not String.IsNullOrEmpty(oAttributeKey) Then 'RWMS-2937 Start
                Select Case dt.Select(" COLUMN_NAME = '" & oAttributeKey & "'")(0)("DATA_TYPE")
                    Case "datetime"
                        Dim atDate As DateTime
                        If Not DateTime.TryParse(_attributescollection(oAttributeKey), atDate) Then
                            sql = sql & "null" & ","c
                        Else
                            sql = sql & Made4Net.Shared.Util.FormatField(_attributescollection(oAttributeKey)) & ","c
                        End If
                    Case "decimal", "float", "real", "numeric"
                        Dim atDec As Decimal
                        If Not Decimal.TryParse(_attributescollection(oAttributeKey), atDec) Then
                            sql = sql & "null" & ","c
                        Else
                            sql = sql & Made4Net.Shared.Util.FormatField(_attributescollection(oAttributeKey)) & ","c
                        End If
                    Case "bit"
                        Dim atBool As Boolean
                        If Not Boolean.TryParse(_attributescollection(oAttributeKey), atBool) Then
                            sql = sql & "null" & ","c
                        Else
                            sql = sql & Made4Net.Shared.Util.FormatField(_attributescollection(oAttributeKey)) & ","c
                        End If
                    Case "int", "bigint", "tinyint", "smallint"
                        Dim atInt As Integer
                        If Not Integer.TryParse(_attributescollection(oAttributeKey), atInt) Then
                            sql = sql & "null" & ","c
                        Else
                            sql = sql & Made4Net.Shared.Util.FormatField(_attributescollection(oAttributeKey)) & ","c
                        End If
                    Case "nvarchar", "nchar", "varchar"
                        If _attributescollection(oAttributeKey) = String.Empty Or _attributescollection(oAttributeKey) = "" Then
                            sql = sql & "null" & ","c
                        Else
                            sql = sql & Made4Net.Shared.Util.FormatField(_attributescollection(oAttributeKey)) & ","c
                        End If
                    Case Else
                        sql = sql & Made4Net.Shared.Util.FormatField(_attributescollection(oAttributeKey)) & ","c
                End Select
            End If 'RWMS-2937 End
        Next
        sql = sql.TrimEnd(","c) & ")"c
        Return sql
    End Function

    Protected Function BuildUpdateStatement() As String
        Dim sql As String
        sql = "UPDATE ATTRIBUTE SET PKEYTYPE=" & Made4Net.Shared.Util.FormatField(PrimaryKeyTypeToString()) & ","
        Dim dt As DataTable = GetAttributeTableSchema()
        For Each sAttributeName As String In _attributescollection.Keys
            If Not String.IsNullOrEmpty(sAttributeName) Then 'RWMS-2937 Start
                Select Case dt.Select(" COLUMN_NAME = '" & sAttributeName & "'")(0)("DATA_TYPE")
                    Case "datetime"
                        Dim atDate As DateTime
                        If DateTime.TryParse(_attributescollection(sAttributeName), atDate) Then
                            sql = sql & sAttributeName & "="c & Made4Net.Shared.Util.FormatField(_attributescollection(sAttributeName)) & ","
                        End If
                    Case "decimal", "float", "real", "numeric"
                        Dim atDec As Decimal
                        If Decimal.TryParse(_attributescollection(sAttributeName), atDec) Then
                            sql = sql & sAttributeName & "="c & Made4Net.Shared.Util.FormatField(_attributescollection(sAttributeName)) & ","
                        End If
                    Case "bit"
                        Dim atBool As Boolean
                        If Boolean.TryParse(_attributescollection(sAttributeName), atBool) Then
                            sql = sql & sAttributeName & "="c & Made4Net.Shared.Util.FormatField(_attributescollection(sAttributeName)) & ","
                        End If
                    Case "int", "bigint", "tinyint", "smallint"
                        Dim atInt As Integer
                        If Integer.TryParse(_attributescollection(sAttributeName), atInt) Then
                            sql = sql & sAttributeName & "="c & Made4Net.Shared.Util.FormatField(_attributescollection(sAttributeName)) & ","
                        End If
                    Case "nvarchar", "nchar", "varchar"
                        If _attributescollection(sAttributeName) = String.Empty Or _attributescollection(sAttributeName) = "" Then
                            sql = sql & sAttributeName & "="c & "NULL,"
                        Else
                            sql = sql & sAttributeName & "="c & Made4Net.Shared.Util.FormatField(_attributescollection(sAttributeName)) & ","
                        End If
                    Case Else
                        sql = sql & sAttributeName & "="c & Made4Net.Shared.Util.FormatField(_attributescollection(sAttributeName)) & ","
                End Select
            End If 'RWMS-2937 End
        Next
        sql = sql.TrimEnd(","c) & " Where " & WhereClause
        Return sql
    End Function

    Public Shared Function GetAttributeTableSchema() As DataTable
        Dim TableStructureSQL As String = "SELECT COLUMN_NAME, DATA_TYPE " & _
                                            "FROM INFORMATION_SCHEMA.COLUMNS " & _
                                            "WHERE TABLE_NAME = 'ATTRIBUTE' AND (COLUMN_NAME<>'PKEYTYPE' AND COLUMN_NAME<>'PKEY1' AND COLUMN_NAME<>'PKEY2' AND COLUMN_NAME<>'PKEY3') " & _
                                            "ORDER BY ORDINAL_POSITION"
        'Added for RWMS-1185 Start
        Dim dataTable As DataTable
        dataTable = New DataTable()
        DataInterface.FillDataset(TableStructureSQL, dataTable)
        Return dataTable

        'Added for RWMS-1185 End

        'Commented for RWMS-1185 Start
        'Dim dataTable As DataTable
        'Dim objectId As String = "ATTRIBUTE"

        'If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AttributeTableSchema, objectId) Then
        '    dataTable = New DataTable()
        '    DataInterface.FillDataset(TableStructureSQL, dataTable)
        'Else
        '    dataTable = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AttributeTableSchema, objectId)
        '    If IsNothing(dataTable) Then
        '        dataTable = New DataTable()
        '        DataInterface.FillDataset(TableStructureSQL, dataTable)
        '        GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AttributeTableSchema, objectId, dataTable, TableStructureSQL)
        '    End If
        'End If
        'Return dataTable
        'Commented for RWMS-1185 End
    End Function

    Public Sub Add(ByVal oAttributeCollection As AttributesCollection)
        _attributescollection.Clear()
        _attributescollection.Add(oAttributeCollection)
    End Sub

    Public Sub SetAttributes(ByVal pAttributes As AttributesCollection, ByVal pPkey1 As String)

        If pAttributes Is Nothing OrElse pAttributes.Count = 0 Then
            Return
        End If
        If _attributescollection.Count > 0 Then
            For i As Integer = 0 To pAttributes.Count - 1
                Me._attributescollection(i) = pAttributes(i)
            Next
            DataInterface.RunSQL(Me.BuildUpdateStatement())
        Else
            Me._attributescollection = pAttributes
            Me._pkey1 = pPkey1
            DataInterface.RunSQL(Me.BuildInsertStatement())
        End If

    End Sub

#End Region

End Class

#End Region

#Region "AttributesCollection"

<CLSCompliant(False)> Public Class AttributesCollection
    Inherits System.Collections.Specialized.NameObjectCollectionBase

#Region "Properties"
    Default Public Property Item(ByVal index As Int32) As Object
        Get
            Return Me.BaseGet(index)
        End Get
        Set(ByVal Value As Object)
            Me.BaseSet(index, Value)
        End Set
    End Property

    Default Public Property Item(ByVal key As String) As Object
        Get
            If BaseGet(key) Is Nothing Then
                Return ""
            ElseIf IsDBNull(BaseGet(key)) Then
                Return ""
            Else
                Return Me.BaseGet(key)
            End If
        End Get
        Set(ByVal Value As Object)
            Me.BaseSet(key, Value)
        End Set
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal dr As DataRow)
        MyBase.New()
        Load(dr)
    End Sub
#End Region

#Region "Methods"
    Public Sub Add(ByVal sAttributeName As String, ByVal oAttributeValue As Object)
        BaseAdd(sAttributeName, oAttributeValue)
    End Sub

    Public Sub Add(ByVal oAttributeCollection As AttributesCollection)
        If oAttributeCollection Is Nothing Then Return
        For idx As Int32 = 0 To oAttributeCollection.Count - 1
            Add(oAttributeCollection.Keys(idx), oAttributeCollection(idx))
        Next
    End Sub

    Public Sub Clear()
        Me.BaseClear()
    End Sub

    Public Function Remove(ByVal key As String) As Object
        Dim obj As Object = Me.Item(key)
        Me.BaseRemove(key)
        Return obj
    End Function

    Public Function RemoveAt(ByVal index As Int32) As Object
        Dim obj As Object = Me.Item(index)
        Me.BaseRemoveAt(index)
        Return obj
    End Function

    Protected Sub Load(ByVal dr As DataRow)
        Dim cColumns As DataColumnCollection = dr.Table.Columns
        For index As Int32 = 4 To cColumns.Count - 1
            Dim oColumn As DataColumn = cColumns(index)
            Me.Add(oColumn.ColumnName, dr(oColumn.ColumnName))
        Next
    End Sub

    Public Function ToNameValueCollection() As System.Collections.Specialized.NameValueCollection
        Dim nvc As New System.Collections.Specialized.NameValueCollection
        Dim Value As Object
        For Each sKey As String In Me.Keys
            Value = Me(sKey)
            If Not Value Is Nothing And Not Value Is System.DBNull.Value Then
                If TypeOf Value Is System.DateTime Then
                    nvc.Add(sKey, Made4Net.Shared.Util.DateTimeToWMSString(Value))
                Else
                    nvc.Add(sKey, Convert.ToString(Value))
                End If
            End If
        Next
        Return nvc
    End Function

    Public Shared Function Equal(ByVal oAttributeCollection1 As AttributesCollection, ByVal oAttributeCollection2 As AttributesCollection) As Boolean
        If oAttributeCollection1.Count = 0 And oAttributeCollection2.Count = 0 Then Return True
        If oAttributeCollection1.Count <> oAttributeCollection2.Count Then Return False

        For idx As Int32 = 0 To oAttributeCollection1.Count - 1
            Dim sKey As String = oAttributeCollection1.Keys(idx)
            If IsDBNull(oAttributeCollection1(sKey)) Then
                oAttributeCollection1(sKey) = ""
            End If
            If IsDBNull(oAttributeCollection2(sKey)) Then
                oAttributeCollection2(sKey) = ""
            End If
            Dim Value As Object
            Value = oAttributeCollection1(sKey)
            If Not Value Is Nothing And Not Value Is System.DBNull.Value Then
                Try
                    If TypeOf Value Is System.DateTime Then
                        If Convert.ToDateTime(oAttributeCollection1(sKey)).Date <> Convert.ToDateTime(oAttributeCollection2(sKey)).Date Then
                            Return False
                        End If
                    Else
                        If oAttributeCollection1(sKey) <> oAttributeCollection2(sKey) Then
                            Return False
                        End If
                    End If
                Catch ex As Exception
                    Return False
                End Try
            End If
        Next
        Return True
    End Function

#End Region

End Class

#End Region