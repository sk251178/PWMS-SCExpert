Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports Made4Net.DataAccess
Imports System.Xml
Imports Made4Net.Shared

#Region "SKU"

' <summary>
' This object represents the properties and methods of a SKU.
' </summary>

<CLSCompliant(False)> Public Class SKU

#Region "Inner Classes"

#Region "Sku Attributes"

    Public Class SkuAttributes

#Region "Variables"

        Private _isnew As Boolean
        Private _ischanged As Boolean
        Private _consignee As String
        Private _sku As String
        Private _attributes As NameValueCollection

#End Region

#Region "Properties"

        Private ReadOnly Property WhereClause() As String
            Get
                Return " CONSIGNEE = '" & _consignee & "' AND SKU = '" & _sku & "'"
            End Get
        End Property

        Public ReadOnly Property Attributes() As NameValueCollection
            Get
                Return _attributes
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            _attributes = New NameValueCollection()
        End Sub

        Public Sub New(ByVal Consignee As String, ByVal Sku As String)
            _consignee = Consignee
            _sku = Sku
            _attributes = New NameValueCollection()
            LoadFromPersistantMedia()
        End Sub

#End Region

#Region "Methods"

        Public Shared Function Exist(ByVal Consignee As String, ByVal Sku As String) As Boolean
            If Convert.ToInt32(DataInterface.ExecuteScalar("SELECT COUNT(1) FROM SKUATTRIBUTE WHERE CONSIGNEE='" & Consignee & "' AND SKU='" & Sku & "'")) > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub PopulateFromDataTable(ByVal SkuAttributesTable As DataTable)
            _attributes.Clear()
            For Each cl As DataColumn In SkuAttributesTable.Columns
                ' In this case we have attributes in the table
                If cl.ColumnName.ToUpper <> "CONSIGNEE" And cl.ColumnName.ToUpper <> "SKU" Then
                    _attributes.Add(cl.ColumnName, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(SkuAttributesTable.Rows(0)(cl.ColumnName)))
                Else
                    If cl.ColumnName.ToUpper = "CONSIGNEE" Then
                        _consignee = SkuAttributesTable.Rows(0)(cl.ColumnName)
                    End If
                    If cl.ColumnName.ToUpper = "SKU" Then
                        _sku = SkuAttributesTable.Rows(0)(cl.ColumnName)
                    End If
                End If
            Next
        End Sub

        Public Sub Save()
            ' Lets check the attributes
            Dim oSkuClass As SkuClass
            Dim oSku As SKU = New SKU(_consignee, _sku)
            oSkuClass = oSku.SKUClass
            If Not oSkuClass Is Nothing Then
                If Not oSkuClass.SkuAttValidationExpression Is Nothing Then
                    'New Validation with expression evaluation
                    Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                    ' Add all sku attributes to the list
                    Dim Fields As String = ""
                    Dim Values As String = ""
                    For i As Integer = 0 To _attributes.Keys.Count - 1
                        Fields = Fields & ";" & _attributes.Keys(i)
                        Values = Values & ";" & _attributes.Item(i)
                    Next
                    vals.Add("ATTS", Fields)
                    vals.Add("VALS", Values)
                    vals.Add("CONSIGNEE", _consignee)
                    vals.Add("SKU", _sku)
                    Dim ret As String = Evaluate(oSkuClass.SkuAttValidationExpression, vals)
                    If ret = "-1" Then
                        Throw New ApplicationException("Attribute validation failed")
                    End If
                End If
            End If
            If Not Exist(_consignee, _sku) Then
                Insert()
            Else
                Update()
            End If
        End Sub

        Public Function Evaluate(ByVal expr As String, ByVal vals As Made4Net.DataAccess.Collections.GenericCollection) As String
            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Return exprEval.Evaluate(expr)
        End Function

        'Public Sub Save(ByVal LogFile As Made4Net.Shared.Logging.LogFile)
        '    If Not Exist(_consignee, _sku) Then
        '        Insert(LogFile)
        '    Else
        '        Update(LogFile)
        '    End If
        'End Sub

        'Private Sub Insert(ByVal LogFile As Made4Net.Shared.Logging.LogFile)
        '    If _attributes.Keys.Count > 0 Then
        '        Dim Fields As String = "CONSIGNEE, SKU"
        '        Dim Values As String = "'" & _consignee & "','" & _sku & "'"
        '        For i As Integer = 0 To _attributes.Keys.Count - 1
        '            Fields = Fields & "," & _attributes.Keys(i)
        '            Values = Values & ",'" & _attributes.Item(i) & "'"
        '        Next
        '        Dim SQL As String = "INSERT INTO SKUATTRIBUTE(" & Fields & ") VALUES(" & Values & ")"
        '        LogFile.WriteLine(SQL, True)
        '        DataInterface.RunSQL(SQL)
        '    End If
        'End Sub

        Public Sub Insert()
            If _attributes.Keys.Count > 0 Then
                Dim Fields As String = "CONSIGNEE, SKU"
                Dim Values As String = "'" & _consignee & "','" & _sku & "'"
                For i As Integer = 0 To _attributes.Keys.Count - 1
                    Fields = Fields & "," & _attributes.Keys(i)
                    Values = Values & ",'" & _attributes.Item(i) & "'"
                Next
                Dim SQL As String = "INSERT INTO SKUATTRIBUTE(" & Fields & ") VALUES(" & Values & ")"
                DataInterface.RunSQL(SQL)
            End If
        End Sub

        Public Sub Update(ByVal LogFile As Made4Net.Shared.Logging.LogFile)
            If _attributes.Keys.Count > 0 Then
                Dim Values As String
                For i As Integer = 0 To _attributes.Keys.Count - 1
                    LogFile.WriteLine(" _attributes.Item(i) " & _attributes.Item(i), True)
                    Values = Values & _attributes.Keys(i) & " = '" & _attributes.Item(i) & "',"
                Next
                Dim SQL As String = "UPDATE SKUATTRIBUTE SET " & Values.TrimEnd(",") & " WHERE " & WhereClause
                LogFile.WriteLine(SQL, True)
                DataInterface.RunSQL(SQL)
            End If
        End Sub

        Public Sub Update()
            If _attributes.Keys.Count > 0 Then
                Dim Values As String
                For i As Integer = 0 To _attributes.Keys.Count - 1
                    Values = Values & _attributes.Keys(i) & " = '" & _attributes.Item(i) & "',"
                Next
                Dim SQL As String = "UPDATE SKUATTRIBUTE SET " & Values.TrimEnd(",") & " WHERE " & WhereClause
                DataInterface.RunSQL(SQL)
            End If
        End Sub

        Private Sub LoadFromPersistantMedia()
            Dim SQL As String = "SELECT * FROM SKUATTRIBUTE WHERE " & WhereClause
            Dim dt As DataTable = New DataTable
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 1 Then
                For Each cl As DataColumn In dt.Columns
                    ' In this case we have attributes in the table
                    If cl.ColumnName.ToUpper <> "CONSIGNEE" And cl.ColumnName <> "SKU" Then

                        If Not Convert.IsDBNull(dt.Rows(0)(cl.ColumnName)) Then
                            Select Case cl.DataType.ToString
                                Case "System.Boolean"
                                    _attributes.Add(cl.ColumnName, Convert.ToBoolean(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.DateTime"
                                    _attributes.Add(cl.ColumnName, Convert.ToDateTime(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.Decimal"
                                    _attributes.Add(cl.ColumnName, Convert.ToDecimal(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.Double"
                                    _attributes.Add(cl.ColumnName, Convert.ToDouble(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.Int32"
                                    _attributes.Add(cl.ColumnName, Convert.ToInt32(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case Else
                                    _attributes.Add(cl.ColumnName, Convert.ToString(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                            End Select
                        End If

                    End If
                Next
            End If
        End Sub

        Private Sub LoadFromPersistantMedia(ByVal LogFile As Made4Net.Shared.Logging.LogFile)
            Dim SQL As String = "SELECT * FROM SKUATTRIBUTE WHERE " & WhereClause
            Dim dt As DataTable = New DataTable
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 1 Then
                For Each cl As DataColumn In dt.Columns
                    ' In this case we have attributes in the table
                    If cl.ColumnName.ToUpper <> "CONSIGNEE" And cl.ColumnName <> "SKU" Then
                        LogFile.WriteLine("Loading Column " & cl.ColumnName & " with data " & Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)(cl.ColumnName)), True)

                        If Not Convert.IsDBNull(dt.Rows(0)(cl.ColumnName)) Then
                            Select Case cl.DataType.ToString
                                Case "System.Boolean"
                                    _attributes.Add(cl.ColumnName, Convert.ToBoolean(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.DateTime"
                                    _attributes.Add(cl.ColumnName, Convert.ToDateTime(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.Decimal"
                                    _attributes.Add(cl.ColumnName, Convert.ToDecimal(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.Double"
                                    _attributes.Add(cl.ColumnName, Convert.ToDouble(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case "System.Int32"
                                    _attributes.Add(cl.ColumnName, Convert.ToInt32(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                                Case Else
                                    _attributes.Add(cl.ColumnName, Convert.ToString(dt.Rows(0)(cl.ColumnName)))
                                    Exit Select
                            End Select

                        End If
                    End If
                Next
            End If
        End Sub

        Public Sub DeleteAllAttributes()
            Me.Attributes.Clear()
            Dim sql As String = String.Format("Delete From SKUATTRIBUTE where {0}", WhereClause)
            DataInterface.RunSQL(sql)
        End Sub

#End Region

    End Class

#End Region

#Region "SKUUOM"

    ' <summary>
    ' This object represents the properties and methods of a SKUUOM.
    ' </summary>

    <CLSCompliant(False)> Public Class SKUUOM

#Region "Variables"

#Region "Primary Keys"

        Protected _consignee As String = String.Empty
        Protected _sku As String = String.Empty
        Protected _uom As String = String.Empty

#End Region

#Region "Other Fields"

        Protected _eanupc As String = String.Empty
        Protected _grossweight As Decimal
        Protected _netweight As Decimal
        Protected _length As Decimal
        Protected _width As Decimal
        Protected _height As Decimal
        Protected _volume As Decimal
        Protected _loweruom As String = String.Empty
        Protected _unitspermeasure As Decimal
        Protected _unitsperlowestuom As Decimal
        Protected _shippable As Boolean
        Protected _nestedquantity As Decimal
        Protected _nestedwidthdiff As Decimal
        Protected _nestedheightdiff As Decimal
        Protected _nesteddepthdiff As Decimal
        Protected _nestedvolumediff As Decimal
        Protected _laborpackagetype As String
        Protected _laborgrabtype As String
        Protected _laborpreparationtype As String
        Protected _laborhandlingtype As String
        Protected _adddate As DateTime
        Protected _adduser As String = String.Empty
        Protected _editdate As DateTime
        Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return " CONSIGNEE = '" & _consignee & "' And SKU = '" & _sku & "' And UOM = '" & _uom & "'"
            End Get
        End Property

        Public ReadOnly Property CONSIGNEE() As String
            Get
                Return _consignee
            End Get
        End Property

        Public ReadOnly Property SKU() As String
            Get
                Return _sku
            End Get
        End Property

        Public Property UOM() As String
            Get
                Return _uom
            End Get
            Set(ByVal Value As String)
                _uom = Value
            End Set
        End Property

        Public Property EANUPC() As String
            Get
                Return _eanupc
            End Get
            Set(ByVal Value As String)
                _eanupc = Value
            End Set
        End Property

        Public Property GROSSWEIGHT() As Decimal
            Get
                Return _grossweight
            End Get
            Set(ByVal Value As Decimal)
                _grossweight = Value
            End Set
        End Property

        Public Property NETWEIGHT() As Decimal
            Get
                Return _netweight
            End Get
            Set(ByVal Value As Decimal)
                _netweight = Value
            End Set
        End Property

        Public Property LENGTH() As Decimal
            Get
                Return _length
            End Get
            Set(ByVal Value As Decimal)
                _length = Value
            End Set
        End Property

        Public Property WIDTH() As Decimal
            Get
                Return _width
            End Get
            Set(ByVal Value As Decimal)
                _width = Value
            End Set
        End Property

        Public Property HEIGHT() As Decimal
            Get
                Return _height
            End Get
            Set(ByVal Value As Decimal)
                _height = Value
            End Set
        End Property

        Public Property VOLUME() As Decimal
            Get
                Return _volume
            End Get
            Set(ByVal Value As Decimal)
                _volume = Value
            End Set
        End Property

        Public Property LOWERUOM() As String
            Get
                Return _loweruom
            End Get
            Set(ByVal Value As String)
                _loweruom = Value
            End Set
        End Property

        Public Property UNITSPERMEASURE() As Decimal
            Get
                Return _unitspermeasure
            End Get
            Set(ByVal Value As Decimal)
                _unitspermeasure = Value
            End Set
        End Property

        Public Property UNITSPERLOWESTUOM() As Decimal
            Get
                Return _unitsperlowestuom
            End Get
            Set(ByVal Value As Decimal)
                _unitsperlowestuom = Value
            End Set
        End Property

        Public Property SHIPPABLE() As Boolean
            Get
                Return _shippable
            End Get
            Set(ByVal Value As Boolean)
                _shippable = Value
            End Set
        End Property

        Public Property NESTEDQUANTITY() As Decimal
            Get
                Return _nestedquantity
            End Get
            Set(ByVal Value As Decimal)
                _nestedquantity = Value
            End Set
        End Property

        Public Property NESTEDVOLUMEDIFF() As Decimal
            Get
                Return _nestedvolumediff
            End Get
            Set(ByVal Value As Decimal)
                _nestedvolumediff = Value
            End Set
        End Property

        Public Property NESTEDDEPTHDIFF() As Decimal
            Get
                Return _nesteddepthdiff
            End Get
            Set(ByVal Value As Decimal)
                _nesteddepthdiff = Value
            End Set
        End Property

        Public Property NESTEDHEIGHTDIFF() As Decimal
            Get
                Return _nestedheightdiff
            End Get
            Set(ByVal Value As Decimal)
                _nestedheightdiff = Value
            End Set
        End Property

        Public Property NESTEDWIDTHDIFF() As Decimal
            Get
                Return _nestedwidthdiff
            End Get
            Set(ByVal Value As Decimal)
                _nestedwidthdiff = Value
            End Set
        End Property

        Public Property LABORGRABTYPE() As String
            Get
                Return _laborgrabtype
            End Get
            Set(ByVal value As String)
                _laborgrabtype = value
            End Set
        End Property

        Public Property LABORHANDLINGTYPE() As String
            Get
                Return _laborhandlingtype
            End Get
            Set(ByVal value As String)
                _laborhandlingtype = value
            End Set
        End Property

        Public Property LABORPACKAGETYPE() As String
            Get
                Return _laborpackagetype
            End Get
            Set(ByVal value As String)
                _laborpackagetype = value
            End Set
        End Property

        Public Property LABORPREPARATIONTYPE() As String
            Get
                Return _laborpreparationtype
            End Get
            Set(ByVal value As String)
                _laborpreparationtype = value
            End Set
        End Property

        Public Property ADDDATE() As DateTime
            Get
                Return _adddate
            End Get
            Set(ByVal Value As DateTime)
                _adddate = Value
            End Set
        End Property

        Public Property ADDUSER() As String
            Get
                Return _adduser
            End Get
            Set(ByVal Value As String)
                _adduser = Value
            End Set
        End Property

        Public Property EDITDATE() As DateTime
            Get
                Return _editdate
            End Get
            Set(ByVal Value As DateTime)
                _editdate = Value
            End Set
        End Property

        Public Property EDITUSER() As String
            Get
                Return _edituser
            End Get
            Set(ByVal Value As String)
                _edituser = Value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pUOM As String, Optional ByVal LoadObj As Boolean = True)
            _consignee = pCONSIGNEE
            _sku = pSKU
            _uom = pUOM
            If LoadObj Then
                Load()
            End If
        End Sub

        Public Sub New(ByVal dr As DataRow)
            Load(dr)
        End Sub

#End Region

#Region "Methods"

        Public Shared Function Exists(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pUOM As String) As Boolean
            Dim sql As String = String.Format("SELECT COUNT(1) FROM SKUUOM WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UOM = '{2}'", pCONSIGNEE, pSKU, pUOM)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Public Shared Function GetSKUUOM(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pUOM As String) As SKUUOM
            Return New SKUUOM(pCONSIGNEE, pSKU, pUOM)
        End Function

        Protected Sub Load()
            Dim SQL As String = "SELECT * FROM SKUUOM Where " & WhereClause
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
            End If
            dr = dt.Rows(0)
            Load(dr)
        End Sub

        Protected Sub Load(ByVal dr As DataRow)
            If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
            If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
            If Not dr.IsNull("UOM") Then _uom = dr.Item("UOM")
            If Not dr.IsNull("EANUPC") Then _eanupc = dr.Item("EANUPC")
            If Not dr.IsNull("GROSSWEIGHT") Then _grossweight = dr.Item("GROSSWEIGHT")
            If Not dr.IsNull("NETWEIGHT") Then _netweight = dr.Item("NETWEIGHT")
            If Not dr.IsNull("LENGTH") Then _length = dr.Item("LENGTH")
            If Not dr.IsNull("WIDTH") Then _width = dr.Item("WIDTH")
            If Not dr.IsNull("HEIGHT") Then _height = dr.Item("HEIGHT")
            If Not dr.IsNull("VOLUME") Then _volume = dr.Item("VOLUME")
            If Not dr.IsNull("LOWERUOM") Then _loweruom = dr.Item("LOWERUOM")
            If Not dr.IsNull("UNITSPERMEASURE") Then _unitspermeasure = dr.Item("UNITSPERMEASURE")
            If Not dr.IsNull("UNITSPERLOWESTUOM") Then _unitsperlowestuom = dr.Item("UNITSPERLOWESTUOM")
            If Not dr.IsNull("SHIPPABLE") Then _shippable = dr.Item("SHIPPABLE")
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        End Sub

        Public Sub Delete()
            Dim sql As String
            Dim cntr As Int32
            sql = String.Format("SELECT COUNT(1) FROM SKUUOM WHERE LOWERUOM = '{0}' AND SKU = '{1}' AND CONSIGNEE = '{2}'", _uom, _sku, _consignee)
            cntr = DataInterface.ExecuteScalar(sql)
            If cntr > 0 Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot delete UOM while it relate to another UOM", "Can't delete UOM while it relate to another UOM")
                Throw m4nEx
            End If

            sql = "DELETE FROM SKUUOM WHERE " & WhereClause
            DataInterface.RunSQL(sql)
        End Sub

        Protected Function isFirstUOM(ByVal pCONSIGNEE As String, ByVal pSKU As String) As Int32
            Dim sql As String = String.Format("SELECT isnull(COUNT(1),0) FROM SKUUOM WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", pCONSIGNEE, pSKU)
            Return Convert.ToInt32(DataInterface.ExecuteScalar(sql))
        End Function

        Protected Function isValidLowerUOM(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pLowerUOM As String) As Boolean
            Dim sql As String = String.Format("SELECT COUNT(1) FROM SKUUOM WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UOM ='{2}'", pCONSIGNEE, pSKU, pLowerUOM)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Public Sub SetUOM(ByVal pConsignee As String, ByVal pSku As String, ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
                ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String)

            Dim ProceedSetingProccess As Boolean = False
            Dim sql As String
            _consignee = pConsignee
            _sku = pSku
            _uom = pUOM
            _eanupc = pEANUPC
            _grossweight = pGrossWeight
            _netweight = pNetWeight
            _length = pLength
            _width = pWidth
            _height = pHeight
            _volume = pVolume
            If _volume = 0 Then
                _volume = _length * _width * _height
            End If
            _loweruom = pLowerUOM
            _unitspermeasure = pUnits
            _shippable = pShippable
            _editdate = DateTime.Now
            _edituser = pUser
            ' Check if the UOM provided is the first UOM
            If isFirstUOM(_consignee, _sku) = 0 Then
                If pLowerUOM <> "" Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot insert new UOM, lower UOM is not empty", "Cannot insert new UOM, lower UOM is not empty")
                    Throw m4nEx
                End If
                ProceedSetingProccess = True
            Else
                ' If not the first then check if the UOM provided is with Lowest UOM that belongs to the SKU
                Dim tmpSku As SKU = New SKU(_consignee, _sku)
                If tmpSku.getLowestUom() = _uom Then
                    If pLowerUOM <> "" Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Lower UOM entered is not valid for this SKU", "Lower UOM entered is not valid for this SKU")
                        Throw m4nEx
                    End If
                    ProceedSetingProccess = True
                Else
                    If Not isValidLowerUOM(pConsignee, pSku, pLowerUOM) Then
                        'if not valid try to check if 
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Lower UOM entered is not valid for this SKU", "Lower UOM entered is not valid for this SKU")
                        Throw m4nEx
                    End If
                    ProceedSetingProccess = True
                End If
            End If

            If ProceedSetingProccess Then
                'Calc the conversion to the lowest uom
                If _loweruom = String.Empty Then
                    _unitsperlowestuom = _unitspermeasure
                Else
                    Dim oLowerUom As New SKUUOM(_consignee, _sku, _loweruom)
                    _unitsperlowestuom = oLowerUom.UNITSPERLOWESTUOM * _unitspermeasure
                End If
                If SKUUOM.Exists(pConsignee, pSku, pUOM) Then
                    validate(True)
                    sql = "update SKUUOM set "
                    sql += "EANUPC = " & Made4Net.Shared.Util.FormatField(_eanupc)
                    sql += ",GROSSWEIGHT = " & Made4Net.Shared.Util.FormatField(_grossweight)
                    sql += ",NETWEIGHT = " & Made4Net.Shared.Util.FormatField(_netweight)
                    sql += ",LENGTH = " & Made4Net.Shared.Util.FormatField(_length)
                    sql += ",WIDTH = " & Made4Net.Shared.Util.FormatField(_width)
                    sql += ",HEIGHT = " & Made4Net.Shared.Util.FormatField(_height)
                    sql += ",VOLUME = " & Made4Net.Shared.Util.FormatField(_volume)
                    sql += ",LOWERUOM = " & Made4Net.Shared.Util.FormatField(_loweruom)
                    sql += ",UNITSPERMEASURE = " & Made4Net.Shared.Util.FormatField(_unitspermeasure)
                    sql += ",SHIPPABLE = " & Made4Net.Shared.Util.FormatField(_shippable)
                    sql += ",UNITSPERLOWESTUOM = " & Made4Net.Shared.Util.FormatField(_unitsperlowestuom)
                    sql += ",EDITUSER = " & Made4Net.Shared.Util.FormatField(_edituser)
                    sql += ",EDITDATE = " & Made4Net.Shared.Util.FormatField(_editdate)
                    sql += " Where " & WhereClause
                Else
                    validate(False)
                    _adddate = DateTime.Now
                    _adduser = pUser
                    sql = "Insert into SKUUOM(CONSIGNEE,SKU,UOM,EANUPC,GROSSWEIGHT,NETWEIGHT,LENGTH,WIDTH," & _
                        "HEIGHT,VOLUME,LOWERUOM,UNITSPERMEASURE,SHIPPABLE,UNITSPERLOWESTUOM,ADDDATE,ADDUSER,EDITDATE,EDITUSER) values ("
                    sql += Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_sku) & "," & Made4Net.Shared.Util.FormatField(_uom) & "," & _
                            Made4Net.Shared.Util.FormatField(_eanupc) & "," & Made4Net.Shared.Util.FormatField(_grossweight) & "," & _
                            Made4Net.Shared.Util.FormatField(_netweight) & "," & Made4Net.Shared.Util.FormatField(_length) & "," & Made4Net.Shared.Util.FormatField(_width) & "," & _
                            Made4Net.Shared.Util.FormatField(_height) & "," & Made4Net.Shared.Util.FormatField(_volume) & "," & Made4Net.Shared.Util.FormatField(_loweruom) & "," & _
                            Made4Net.Shared.Util.FormatField(_unitspermeasure) & "," & Made4Net.Shared.Util.FormatField(_shippable) & "," & Made4Net.Shared.Util.FormatField(_unitsperlowestuom) & "," & _
                            Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"
                End If
                DataInterface.RunSQL(sql)
                Dim em As New EventManagerQ
                em.Add("ACTION", WMS.Lib.Actions.Audit.SETUOM)
                em.Add("CONSIGNEE", _consignee)
                em.Add("SKU", _sku)
                em.Add("UOM", _uom)
                em.Add("EANUPC", _eanupc)
                em.Add("GROSSWEIGHT", _grossweight)
                em.Add("NETWEIGHT", _netweight)
                em.Add("LENGTH", _length)
                em.Add("WIDTH", _width)
                em.Add("HEIGHT", _height)
                em.Add("VOLUME", _volume)
                em.Add("LOWERUOM", _loweruom)
                em.Add("UNITSPERMEASURE", _unitspermeasure)
                em.Add("SHIPPABLE", _shippable)
                em.Add("USERID", pUser)
                em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SETUOM)
                em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                em.Send(WMS.Lib.Actions.Audit.SETUOM)
            End If
        End Sub

        Private Sub validate(ByVal pEdit As Boolean)
            Dim tmpSKU As SKU = New SKU(_consignee, _sku)
            If pEdit Then
                'If we edit the lowest UOM
                If Not tmpSKU.getLowestUom() Is Nothing AndAlso tmpSKU.getLowestUom().ToLower() = _uom.ToLower() Then
                    If _loweruom <> String.Empty Then
                        Throw New M4NException(New Exception, "Can not update UOM. Lower UOM must be empty.", "Can not update UOM. Lower uom must be empty.")
                    End If
                Else 'If we edit any other UOM
                    If tmpSKU.isChildOf(_loweruom, _uom) Then
                        Throw New M4NException(New Exception, "Can not update UOM. Lower UOM is not valid.", "Can not update UOM. Lower UOM is not valid.")
                    End If
                End If
            Else 'If we are inserting a new uom
                If Exists(_consignee, _sku, _uom) Then
                    Throw New M4NException(New Exception, "Can not insert UOM. UOM already exists.", "Can not insert UOM. UOM already exists.")
                End If
                If isFirstUOM(_consignee, _sku) = 0 Then 'If it's the first UOM created for that SKU
                    If _loweruom <> String.Empty Then
                        Throw New M4NException(New Exception, "Can not insert UOM. First Uom must have an empty lower UOM.", "Can not insert UOM. First Uom must have an empty lower UOM.")
                    End If
                End If
            End If

            If Not isFirstUOM(_consignee, _sku) = 0 Then
                If _netweight > Me._grossweight Then
                    Throw New M4NException(New Exception, "Net weight must be less than gross weight", "Net weight must be less than gross weight")
                End If
                If tmpSKU.getLowestUom().ToLower() = _uom.ToLower() Then
                    If _unitspermeasure <= 0 Then
                        Throw New M4NException(New Exception, "Can not insert UOM. Units per UOM must be greater than zero.", "Can not insert uom. Units per UOM must be greater than zero.")
                    End If
                Else
                    'General validations that needs to be on both edit and insert modes as long as it's not the lowest UOM
                    If Not Exists(_consignee, _sku, _loweruom) Then
                        Throw New M4NException(New Exception, "Can not update UOM. Lower UOM is not defined.", "Can not update UOM. Lower UOM is not defined.")
                    End If
                    If _loweruom.ToLower() = _uom.ToLower() Then
                        Throw New M4NException(New Exception, "Can not update UOM. Lower uom equals the uom being edited.", "Can not update UOM. Lower uom equals the uom being edited.")
                    End If
                    If _unitspermeasure < 1 Then
                        Throw New M4NException(New Exception, "Can not insert uom. Units per UOM must be greater than 1.", "Can not insert uom. Units per UOM must be greater than 1.")
                    End If
                End If
            End If
        End Sub

        Public Sub Create(ByVal pConsignee As String, ByVal pSku As String, ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
                ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, _
                ByVal pLaborPreparationType As String, ByVal pUser As String)
            Dim sql As String
            _consignee = pConsignee
            _sku = pSku
            _uom = pUOM
            _eanupc = pEANUPC
            _grossweight = pGrossWeight
            _netweight = pNetWeight
            _length = pLength
            _width = pWidth
            _height = pHeight
           _volume = Decimal.Round((_length * _width * _height) / 1728, 3, MidpointRounding.AwayFromZero)
            _loweruom = pLowerUOM
            _unitspermeasure = pUnits
            _shippable = pShippable

            _nesteddepthdiff = pNestedDepthDiff
            _nestedwidthdiff = pNestedWidthDiff
            _nestedheightdiff = pNestedHeightDiff
            _nestedvolumediff = pNestedVolumeDiff
            _nestedquantity = pNestedQuantity

            _laborgrabtype = pLaborGrabType
            _laborhandlingtype = pLaborHandlingType
            _laborpackagetype = pLaborPackageType
            _laborpreparationtype = pLaborPreparationType

            _editdate = DateTime.Now
            _adddate = DateTime.Now
            _edituser = pUser
            _adduser = pUser

            validate(False)

            If _loweruom = String.Empty Then
                _unitsperlowestuom = _unitspermeasure
            Else
                Dim oLowerUom As New SKUUOM(_consignee, _sku, _loweruom)
                _unitsperlowestuom = oLowerUom.UNITSPERLOWESTUOM * _unitspermeasure
            End If


            sql = String.Format("INSERT INTO SKUUOM (CONSIGNEE,SKU,UOM,EANUPC,GROSSWEIGHT,NETWEIGHT,LENGTH,WIDTH,HEIGHT,VOLUME,LOWERUOM," & _
            "UNITSPERMEASURE,SHIPPABLE,UNITSPERLOWESTUOM,ADDDATE,ADDUSER,EDITDATE,EDITUSER,NestedQuantity,NestedWidthDiff,NestedHeightDiff,NestedDepthDiff,NestedVolumeDiff," & _
            "LABORGRABTYPE,LABORHANDLINGTYPE,LABORPACKAGETYPE,LABORPREPARATIONTYPE) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}," & _
            "{12},{13},{14},{15},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24})", _
            Made4Net.Shared.Util.FormatField(_consignee), _
            Made4Net.Shared.Util.FormatField(_sku), _
            Made4Net.Shared.Util.FormatField(_uom), _
            Made4Net.Shared.Util.FormatField(_eanupc), _
            Made4Net.Shared.Util.FormatField(_grossweight), _
            Made4Net.Shared.Util.FormatField(_netweight), _
            Made4Net.Shared.Util.FormatField(_length), _
            Made4Net.Shared.Util.FormatField(_width), _
            Made4Net.Shared.Util.FormatField(_height), _
            Made4Net.Shared.Util.FormatField(_volume), _
            Made4Net.Shared.Util.FormatField(_loweruom), _
            Made4Net.Shared.Util.FormatField(_unitspermeasure), _
            Made4Net.Shared.Util.FormatField(_shippable), _
            Made4Net.Shared.Util.FormatField(_unitsperlowestuom), _
            Made4Net.Shared.Util.FormatField(_adddate), _
            Made4Net.Shared.Util.FormatField(_adduser), _
 _
            Made4Net.Shared.Util.FormatField(_nestedquantity), _
            Made4Net.Shared.Util.FormatField(_nestedwidthdiff), _
            Made4Net.Shared.Util.FormatField(_nestedheightdiff), _
            Made4Net.Shared.Util.FormatField(_nesteddepthdiff), _
            Made4Net.Shared.Util.FormatField(_nestedvolumediff), _
            Made4Net.Shared.Util.FormatField(_laborgrabtype), _
            Made4Net.Shared.Util.FormatField(_laborhandlingtype), _
            Made4Net.Shared.Util.FormatField(_laborpackagetype), _
            Made4Net.Shared.Util.FormatField(_laborpreparationtype))

            DataInterface.RunSQL(sql)

            sendToEM(pUser)
        End Sub

        Public Sub Update(ByVal pConsignee As String, ByVal pSku As String, ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
                ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, _
                ByVal pLaborPreparationType As String)
            Dim sql As String
            _consignee = pConsignee
            _sku = pSku
            _uom = pUOM
            _eanupc = pEANUPC
            _grossweight = pGrossWeight
            _netweight = pNetWeight
            _length = pLength
            _width = pWidth
            _height = pHeight
            _volume = Decimal.Round((_length * _width * _height) / 1728, 3, MidpointRounding.AwayFromZero)
            _loweruom = pLowerUOM
            _unitspermeasure = pUnits
            _shippable = pShippable

            _edituser = pUser

            _nesteddepthdiff = pNestedDepthDiff
            _nestedwidthdiff = pNestedWidthDiff
            _nestedheightdiff = pNestedHeightDiff
            _nestedvolumediff = pNestedVolumeDiff
            _nestedquantity = pNestedQuantity

            _laborgrabtype = pLaborGrabType
            _laborhandlingtype = pLaborHandlingType
            _laborpackagetype = pLaborPackageType
            _laborpreparationtype = pLaborPreparationType


            validate(True)
            _editdate = DateTime.Now

            If _loweruom = String.Empty Then
                _unitsperlowestuom = _unitspermeasure
            Else
                Dim oLowerUom As New SKUUOM(_consignee, _sku, _loweruom)
                _unitsperlowestuom = oLowerUom.UNITSPERLOWESTUOM * _unitspermeasure
            End If

            sql = String.Format("UPDATE SKUUOM SET EANUPC={0},GROSSWEIGHT={1},NETWEIGHT={2},LENGTH={3},WIDTH={4},HEIGHT={5},VOLUME={6},LOWERUOM={7}" & _
            ",UNITSPERMEASURE={8},SHIPPABLE={9},UNITSPERLOWESTUOM={10}, EDITUSER={11},EDITDATE={12},NestedDepthDiff={13},NestedWidthDiff={14},NestedVolumeDiff={15},NestedHeightDiff={16},NestedQuantity={17}," & _
            "LABORGRABTYPE={18},LABORHANDLINGTYPE={19},LABORPACKAGETYPE={20},LABORPREPARATIONTYPE={21} WHERE {22}", _
            Made4Net.Shared.Util.FormatField(_eanupc), Made4Net.Shared.Util.FormatField(_grossweight), _
            Made4Net.Shared.Util.FormatField(_netweight), Made4Net.Shared.Util.FormatField(_length), _
            Made4Net.Shared.Util.FormatField(_width), Made4Net.Shared.Util.FormatField(_height), _
            Made4Net.Shared.Util.FormatField(_volume), Made4Net.Shared.Util.FormatField(_loweruom), _
            Made4Net.Shared.Util.FormatField(_unitspermeasure), Made4Net.Shared.Util.FormatField(_shippable), _
            Made4Net.Shared.Util.FormatField(_unitsperlowestuom), Made4Net.Shared.Util.FormatField(_edituser), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_nesteddepthdiff), _
            Made4Net.Shared.Util.FormatField(_nestedwidthdiff), Made4Net.Shared.Util.FormatField(_nestedvolumediff), _
            Made4Net.Shared.Util.FormatField(_nestedheightdiff), Made4Net.Shared.Util.FormatField(_nestedquantity), _
            Made4Net.Shared.Util.FormatField(_laborgrabtype), Made4Net.Shared.Util.FormatField(_laborhandlingtype), _
            Made4Net.Shared.Util.FormatField(_laborpackagetype), Made4Net.Shared.Util.FormatField(_laborpreparationtype), WhereClause)

            DataInterface.RunSQL(sql)

            sendToEM(pUser)
        End Sub

        Private Sub sendToEM(ByVal pUser As String)
            Dim em As New EventManagerQ
            em.Add("ACTION", WMS.Lib.Actions.Audit.SETUOM)
            em.Add("CONSIGNEE", _consignee)
            em.Add("SKU", _sku)
            em.Add("UOM", _uom)
            em.Add("EANUPC", _eanupc)
            em.Add("GROSSWEIGHT", _grossweight)
            em.Add("NETWEIGHT", _netweight)
            em.Add("LENGTH", _length)
            em.Add("WIDTH", _width)
            em.Add("HEIGHT", _height)
            em.Add("VOLUME", _volume)
            em.Add("LOWERUOM", _loweruom)
            em.Add("UNITSPERMEASURE", _unitspermeasure)
            em.Add("SHIPPABLE", _shippable)
            em.Add("USERID", pUser)
            em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SETUOM)
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMS.Lib.Actions.Audit.SETUOM)
        End Sub

#End Region

    End Class

#End Region

#Region "UOM COLLECTION"

    <CLSCompliant(False)> Public Class SKUUOMCollection
        Inherits ArrayList

#Region "Variables"

        Protected _consignee As String
        Protected _sku As String

#End Region

#Region "Properties"

        Default Public Shadows ReadOnly Property item(ByVal index As Int32) As SKUUOM
            Get
                Return CType(MyBase.Item(index), SKUUOM)
            End Get
        End Property

        Public ReadOnly Property UOM(ByVal pUOM As String) As SKUUOM
            Get
                Dim i As Int32
                'Start PWMS-921 and RWMS-977 If DEFAULTRECLOADUOM is null or blank then take highest UOM of SKU  
               For i = 0 To Me.Count - 1   
                    If item(i).UOM.ToLower() = pUOM.ToLower() Then
                        Return (CType(MyBase.Item(i), SKUUOM))
                    End If
                Next
                'End PWMS-921 and RWMS-977  
                Return Nothing
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByVal pConsignee As String, ByVal pSku As String, Optional ByVal LoadAll As Boolean = True)
            _consignee = pConsignee
            _sku = pSku
            Dim SQL As String
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select * from SKUUOM where consignee = '" & _consignee & "' and sku = '" & _sku & "'"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                Me.add(New SKUUOM(dr))
            Next
        End Sub

#End Region

#Region "Methods"

        Public Shadows Function add(ByVal pObject As SKUUOM) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As SKUUOM)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As SKUUOM)
            MyBase.Remove(pObject)
        End Sub

        Public Sub SetUOM(ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
        ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, _
                ByVal pLaborPreparationType As String)

            Dim oUom As SKUUOM
            oUom = Me.UOM(pUOM)
            If Not SKUUOM.Exists(_consignee, _sku, pUOM) Then
                oUom = New SKUUOM
                oUom.Create(_consignee, _sku, pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, _
                    pLowerUOM, pUnits, pShippable, pNestedQuantity, pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, _
                    pLaborGrabType, pLaborHandlingType, pLaborPackageType, pLaborPreparationType, pUser)
                'Me.add(oUom)
            Else
                oUom.Update(_consignee, _sku, pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, _
                    pLowerUOM, pUnits, pShippable, pUser, pNestedQuantity, pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, _
                    pLaborGrabType, pLaborHandlingType, pLaborPackageType, pLaborPreparationType)
            End If
        End Sub

        Public Sub DeleteUOM(ByVal pUOM As String, Optional ByVal pDeleteAll As Boolean = False)
            Dim oUOM As SKUUOM
            oUOM = UOM(pUOM)
            If oUOM Is Nothing Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Unit of measure does not Exists", "Unit of measure does not Exists")
                Throw m4nEx
            End If
            oUOM.Delete()
            Remove(oUOM)
        End Sub

        Public Sub DeleteAllUOM()
            While (Me.Count > 0)
                DeleteUOM(Me.item(0).UOM, True)
            End While
        End Sub

        Public Sub CreateUOM(ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
        ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, ByVal pLaborPreparationType As String)

            Dim oUom As SKUUOM = New SKUUOM()
            oUom.Create(_consignee, _sku, pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, _
                pLowerUOM, pUnits, pShippable, pNestedQuantity, pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, _
                pLaborGrabType, pLaborHandlingType, pLaborPackageType, pLaborPreparationType, pUser)
            Me.add(oUom)
        End Sub

        Public Sub UpdateUOM(ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
        ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, ByVal pLaborPreparationType As String)
            Dim oUom As SKUUOM
            oUom = Me.UOM(pUOM)
            If oUom Is Nothing Then
                Throw New M4NException(New Exception, "Uom does not exist", "Uom Does not exist")
            End If
            oUom.Update(_consignee, _sku, pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, _
            pLowerUOM, pUnits, pShippable, pUser, pNestedQuantity, pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, _
            pLaborGrabType, pLaborHandlingType, pLaborPackageType, pLaborPreparationType)
        End Sub

        Public Sub Save(ByVal pDefaultUom As String, ByVal pUser As String)
            If Not Me.hasLowestUom() Then
                Dim Uom As New SKUUOM()
                Uom.LOWERUOM = ""
                Uom.UOM = pDefaultUom
                Uom.UNITSPERMEASURE = 1
                Me.add(Uom)
            End If
            sortByUOM()
            For Each uom As SKUUOM In Me
                SetUOM(uom.UOM, uom.EANUPC, uom.GROSSWEIGHT, uom.NETWEIGHT, uom.LENGTH, uom.WIDTH, uom.HEIGHT, uom.VOLUME, uom.LOWERUOM, uom.UNITSPERMEASURE, uom.SHIPPABLE, pUser, uom.NESTEDQUANTITY, uom.NESTEDWIDTHDIFF, uom.NESTEDHEIGHTDIFF, uom.NESTEDDEPTHDIFF, uom.NESTEDVOLUMEDIFF, uom.LABORGRABTYPE, uom.LABORHANDLINGTYPE, uom.LABORPACKAGETYPE, uom.LABORPREPARATIONTYPE)
            Next
        End Sub

        Public Sub sortByUOM()
            If Not hasLowestUom() Then
                Throw New M4NException(New Exception, "Can not sort without a lowest UOM", "Can not sort without a lowest UOM")
            End If
            Dim tmpList() As SKUUOM = New SKUUOM(Me.Count - 1) {}
            Me.CopyTo(tmpList)
            Me.Clear()
            Dim nextLowerUoms As New System.Collections.Generic.List(Of String)
            Dim possibleLowerUoms As New System.Collections.Generic.List(Of String)
            For Each uom As WMS.Logic.SKU.SKUUOM In tmpList
                possibleLowerUoms.Add(uom.UOM)
            Next
            nextLowerUoms.Add("")
            Dim uomsLeft As Integer = tmpList.Length
            While uomsLeft > 0
                For i As Integer = 0 To tmpList.Length - 1
                    If Not tmpList(i) Is Nothing Then
                        If nextLowerUoms.Contains(tmpList(i).LOWERUOM) Then
                            Me.add(tmpList(i))
                            nextLowerUoms.Add(tmpList(i).UOM)
                            tmpList(i) = Nothing
                            uomsLeft -= 1
                        ElseIf Not possibleLowerUoms.Contains(tmpList(i).LOWERUOM) Then
                            Throw New M4NException(New Exception(), "There is a uom with a lower uom that does not exist.", "There is a uom with a lower uom that does not exist." + tmpList(i).LOWERUOM)
                        End If
                    End If
                Next
            End While
        End Sub

        Private Function hasLowestUom() As Boolean
            For Each uom As SKUUOM In Me
                If uom.LOWERUOM = "" Then
                    Return True
                End If
            Next
            Return False
        End Function

#End Region

    End Class

#End Region

#Region "SkuBOM"

    ' <summary>
    ' This object represents the properties and methods of a SKUUOM.
    ' </summary>

    <CLSCompliant(False)> Public Class SKUBOM

#Region "Variables"

#Region "Primary Keys"

        Protected _consignee As String = String.Empty
        Protected _bomsku As String = String.Empty
        Protected _partsku As String = String.Empty

#End Region

#Region "Other Fields"

        Protected _partqty As Decimal
        Protected _bomorder As String
        Protected _adddate As DateTime
        Protected _adduser As String = String.Empty
        Protected _editdate As DateTime
        Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return " CONSIGNEE = '" & _consignee & "' And BOMSKU = '" & _bomsku & "' And PARTSKU = '" & _partsku & "'"
            End Get
        End Property

        Public ReadOnly Property CONSIGNEE() As String
            Get
                Return _consignee
            End Get
        End Property

        Public ReadOnly Property BOMSKU() As String
            Get
                Return _bomsku
            End Get
        End Property

        Public ReadOnly Property PARTSKU() As String
            Get
                Return _partsku
            End Get
        End Property

        Public Property PARTQTY() As Decimal
            Get
                Return _partqty
            End Get
            Set(ByVal Value As Decimal)
                _partqty = Value
            End Set
        End Property

        Public Property BOMORDER() As String
            Get
                Return _bomorder
            End Get
            Set(ByVal Value As String)
                _bomorder = Value
            End Set
        End Property

        Public Property ADDDATE() As DateTime
            Get
                Return _adddate
            End Get
            Set(ByVal Value As DateTime)
                _adddate = Value
            End Set
        End Property

        Public Property ADDUSER() As String
            Get
                Return _adduser
            End Get
            Set(ByVal Value As String)
                _adduser = Value
            End Set
        End Property

        Public Property EDITDATE() As DateTime
            Get
                Return _editdate
            End Get
            Set(ByVal Value As DateTime)
                _editdate = Value
            End Set
        End Property

        Public Property EDITUSER() As String
            Get
                Return _edituser
            End Get
            Set(ByVal Value As String)
                _edituser = Value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal pCONSIGNEE As String, ByVal pBomSKU As String, ByVal pPartSKU As String, Optional ByVal LoadObj As Boolean = True)
            _consignee = pCONSIGNEE
            _bomsku = pBomSKU
            _partsku = pPartSKU
            If LoadObj Then
                Load()
            End If
        End Sub

        Public Sub New(ByVal dr As DataRow)
            Load(dr)
        End Sub

#End Region

#Region "Methods"

        Public Shared Function Exists(ByVal pCONSIGNEE As String, ByVal pBomSKU As String, ByVal pPartSKU As String) As Boolean
            Dim sql As String = String.Format("Select count(1) from BOM where consignee = '{0}' and BOMSKU = '{1}' and PARTSKU = '{2}'", pCONSIGNEE, pBomSKU, pPartSKU)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Public Shared Function GetSKUBOM(ByVal pCONSIGNEE As String, ByVal pBOMSKU As String, ByVal pPartSku As String) As SKUBOM
            Return New SKUBOM(pCONSIGNEE, pBOMSKU, pPartSku)
        End Function

        Protected Sub Load()
            Dim SQL As String = "SELECT * FROM BOM Where " & WhereClause
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Return
            End If
            dr = dt.Rows(0)
            Load(dr)
        End Sub

        Protected Sub Load(ByVal dr As DataRow)
            If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
            If Not dr.IsNull("BOMSKU") Then _bomsku = dr.Item("BOMSKU")
            If Not dr.IsNull("PARTSKU") Then _partsku = dr.Item("PARTSKU")
            If Not dr.IsNull("PARTQTY") Then _partqty = dr.Item("PARTQTY")
            If Not dr.IsNull("BOMORDER") Then _bomorder = dr.Item("BOMORDER")
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        End Sub

        Public Sub Delete()
            Dim sql As String

            sql = "delete from BOM where " & WhereClause
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub Create(ByVal pConsignee As String, ByVal pSku As String, ByVal pPartSku As String, ByVal pPartQty As Decimal, ByVal pBomOrder As String, ByVal pUser As String)
            Dim m4nEx As M4NException = validate(pConsignee, pSku, pPartSku, pPartQty, False)
            If Not m4nEx Is Nothing Then
                Throw m4nEx
            End If
            Dim SQL As String
            _consignee = pConsignee
            _bomsku = pSku
            _partsku = pPartSku
            _partqty = pPartQty
            _bomorder = pBomOrder
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser

            SQL = String.Format("INSERT INTO BOM (CONSIGNEE, BOMSKU, PARTSKU, PARTQTY, BOMORDER, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8})", _
                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_bomsku), Made4Net.Shared.Util.FormatField(_partsku), Made4Net.Shared.Util.FormatField(_partqty), _
                Made4Net.Shared.Util.FormatField(_bomorder), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
            DataInterface.RunSQL(SQL)
        End Sub

        Public Sub Update(ByVal pPartQty As Decimal, ByVal pBomOrder As String, ByVal pUser As String)
            Dim SQL As String
            Dim m4nEx As M4NException = validate(_consignee, _bomsku, _partsku, pPartQty, True)
            If Not m4nEx Is Nothing Then
                Throw m4nEx
            End If
            _partqty = pPartQty
            _bomorder = pBomOrder
            _editdate = DateTime.Now
            _edituser = pUser
            ' WhereClause added by Lev 11.09.2006
            SQL = String.Format("UPDATE BOM SET PARTQTY ={0}, BOMORDER ={1}, EDITDATE ={2}, EDITUSER ={3} WHERE {4}", _
                Made4Net.Shared.Util.FormatField(_partqty), Made4Net.Shared.Util.FormatField(_bomorder), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(SQL)
        End Sub

        Private Function validate(ByVal pConsignee As String, ByVal pSKU As String, ByVal pPartSKU As String, ByVal pPartQty As Decimal, Optional ByVal pEdit As Boolean = True) As M4NException
            Dim m4nEx As M4NException
            If Not WMS.Logic.SKU.Exists(pConsignee, pPartSKU) Then
                m4nEx = New Made4Net.Shared.M4NException(New Exception, "No such SKU", "No such SKU")
                Return m4nEx
            End If

            If pSKU = pPartSKU Then
                m4nEx = New Made4Net.Shared.M4NException(New Exception, "Part SKU is equal to bom SKU", "Part SKU is equal to bom SKU")
                Return m4nEx
            End If

            If pPartQty <= 0 Then
                m4nEx = New Made4Net.Shared.M4NException(New Exception, "Not a positive quantity", "Not a positive quantity")
                Return m4nEx
            End If

            If Not pEdit Then
                If WMS.Logic.SKU.SKUBOM.Exists(pConsignee, pSKU, pPartSKU) Then
                    m4nEx = New Made4Net.Shared.M4NException(New Exception, "The selected part SKU already exists in the bom table", "The selected part SKU already exists in the bom table")
                    Return m4nEx
                End If
            End If

            Return Nothing
        End Function

#End Region

    End Class

#End Region

#Region "BOM COLLECTION"

    <CLSCompliant(False)> Public Class SKUBOMCollection
        Inherits ArrayList

#Region "Variables"

        Protected _consignee As String
        Protected _sku As String

#End Region

#Region "Properties"

        Default Public Shadows ReadOnly Property item(ByVal index As Int32) As SKUBOM
            Get
                Return CType(MyBase.Item(index), SKUBOM)
            End Get
        End Property

        Public ReadOnly Property BOM(ByVal pPartSku As String) As SKUBOM
            Get
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If item(i).PARTSKU = pPartSku Then
                        Return (CType(MyBase.Item(i), SKUBOM))
                    End If
                Next
                Return Nothing
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByVal pConsignee As String, ByVal pSku As String, Optional ByVal LoadAll As Boolean = True)
            _consignee = pConsignee
            _sku = pSku
            Dim SQL As String
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select * from BOM where consignee = '" & _consignee & "' and BOMSKU = '" & _sku & "'"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                Me.add(New SKUBOM(dr))
            Next
        End Sub

#End Region

#Region "Methods"

        Public Shadows Function add(ByVal pObject As SKUBOM) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As SKUBOM)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As SKUBOM)
            MyBase.Remove(pObject)
        End Sub

        Public Sub DeleteBOM(ByVal pPartSKU As String)
            Dim oBom As SKUBOM
            oBom = BOM(pPartSKU)
            If oBom Is Nothing Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "sku does not Exists", "sku does not Exists")
                Throw m4nEx
            End If
            oBom.Delete()
            Remove(oBom)
        End Sub

        Public Sub DeleteAllBOM()
            While Me.Count > 0
                DeleteBOM(Me.item(0).PARTSKU)
            End While
        End Sub

#End Region

    End Class

#End Region

#Region "SKU Substitutes"

    Public Class SkuSubstitutes

#Region "Variables"

#Region "Primary Keys"

        Protected _consignee As String = String.Empty
        Protected _sku As String = String.Empty
        Protected _substitutesku As String = String.Empty

#End Region

#Region "Other Fields"

        Protected _priority As Int32
        Protected _todate As DateTime
        Protected _fromdate As DateTime
        Protected _skuqty As Decimal
        Protected _substituteskuqty As Decimal
        Protected _substitutiontype As String
        Protected _multilevel As Boolean
        Protected _company As String
        Protected _companytype As String
        Protected _adddate As DateTime
        Protected _adduser As String = String.Empty
        Protected _editdate As DateTime
        Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

        Public ReadOnly Property WhereClause() As String
            Get
                Return " CONSIGNEE = '" & _consignee & "' And SKU = '" & _sku & "' And substitutesku = '" & _substitutesku & "'"
            End Get
        End Property

        Public Property CONSIGNEE() As String
            Set(ByVal Value As String)
                _consignee = Value
            End Set
            Get
                Return _consignee
            End Get
        End Property

        Public Property SKU() As String
            Set(ByVal Value As String)
                _sku = Value
            End Set
            Get
                Return _sku
            End Get
        End Property

        Public Property SUBSTITUTESKU() As String
            Set(ByVal Value As String)
                _substitutesku = Value
            End Set
            Get
                Return _substitutesku
            End Get
        End Property

        Public Property PRIORITY() As Int32
            Get
                Return _priority
            End Get
            Set(ByVal Value As Int32)
                _priority = Value
            End Set
        End Property

        Public Property FROMDATE() As DateTime
            Get
                Return _fromdate
            End Get
            Set(ByVal Value As DateTime)
                _fromdate = Value
            End Set
        End Property

        Public Property TODATE() As DateTime
            Get
                Return _todate
            End Get
            Set(ByVal Value As DateTime)
                _todate = Value
            End Set
        End Property

        Public Property SUBSTITUTIONTYPE() As String
            Get
                Return _substitutiontype
            End Get
            Set(ByVal Value As String)
                _substitutiontype = Value
            End Set
        End Property

        Public Property SUBSTITUTESKUQTY() As Decimal
            Get
                Return _substituteskuqty
            End Get
            Set(ByVal Value As Decimal)
                _substituteskuqty = Value
            End Set
        End Property

        Public Property MULTILEVEL() As Boolean
            Get
                Return _multilevel
            End Get
            Set(ByVal Value As Boolean)
                _multilevel = Value
            End Set
        End Property

        Public Property SKUQTY() As Decimal
            Get
                Return _skuqty
            End Get
            Set(ByVal Value As Decimal)
                _skuqty = Value
            End Set
        End Property

        Public Property COMPANY() As String
            Get
                Return _company
            End Get
            Set(ByVal Value As String)
                _company = Value
            End Set
        End Property

        Public Property COMPANYTYPE() As String
            Get
                Return _companytype
            End Get
            Set(ByVal Value As String)
                _companytype = Value
            End Set
        End Property

        Public Property ADDDATE() As DateTime
            Get
                Return _adddate
            End Get
            Set(ByVal Value As DateTime)
                _adddate = Value
            End Set
        End Property

        Public Property ADDUSER() As String
            Get
                Return _adduser
            End Get
            Set(ByVal Value As String)
                _adduser = Value
            End Set
        End Property

        Public Property EDITDATE() As DateTime
            Get
                Return _editdate
            End Get
            Set(ByVal Value As DateTime)
                _editdate = Value
            End Set
        End Property

        Public Property EDITUSER() As String
            Get
                Return _edituser
            End Get
            Set(ByVal Value As String)
                _edituser = Value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Shared Function Exists(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pSUBSTITUTESKU As String) As Boolean
            Dim sql As String = String.Format("Select count(1) from SKUSUBSTITUTE where consignee = '{0}' and SKU = '{1}' and SUBSTITUTESKU = '{2}'", pCONSIGNEE, pSKU, pSUBSTITUTESKU)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Function

        Public Sub New(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pSUBSTITUTESKU As String, Optional ByVal LoadObj As Boolean = True)
            _consignee = pCONSIGNEE
            _sku = pSKU
            _substitutesku = pSUBSTITUTESKU
            If LoadObj Then
                Load()
            End If
        End Sub

        Public Sub New(ByVal dr As DataRow)
            Load(dr)
        End Sub

#End Region

#Region "Methods"

        Public Shared Function GetSkuSubstitutes(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByVal pSubstituteSku As String) As SkuSubstitutes
            Return New SkuSubstitutes(pCONSIGNEE, pSKU, pSubstituteSku)
        End Function

        Protected Sub Load()
            Dim SQL As String = "SELECT * FROM skusubstitute Where " & WhereClause
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Return
            End If
            dr = dt.Rows(0)
            Load(dr)
        End Sub

        Protected Sub Load(ByVal dr As DataRow)
            If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
            If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
            If Not dr.IsNull("SKUQTY") Then _skuqty = dr.Item("SKUQTY")
            If Not dr.IsNull("SUBSTITUTESKU") Then _substitutesku = dr.Item("SUBSTITUTESKU")
            If Not dr.IsNull("PRIORITY") Then _priority = dr.Item("PRIORITY")
            If Not dr.IsNull("FROMDATE") Then _fromdate = dr.Item("FROMDATE") Else _fromdate = DateTime.MinValue
            If Not dr.IsNull("TODATE") Then _todate = dr.Item("TODATE") Else _todate = DateTime.MaxValue
            If Not dr.IsNull("SKUQTY") Then _skuqty = dr.Item("SKUQTY")
            If Not dr.IsNull("SUBSTITUTESKUQTY") Then _substituteskuqty = dr.Item("SUBSTITUTESKUQTY")
            If Not dr.IsNull("SUBSTITUTIONTYPE") Then _substitutiontype = dr.Item("SUBSTITUTIONTYPE")
            If Not dr.IsNull("MULTYLEVEL") Then _multilevel = dr.Item("MULTYLEVEL")
            If Not dr.IsNull("COMPANY") Then _company = dr.Item("COMPANY")
            If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        End Sub

        Public Sub Create(ByVal pConsignee As String, ByVal pSku As String, ByVal pSkuqty As Decimal, ByVal pSubSku As String, _
        ByVal pPriority As Int32, ByVal pSubstituteSkuQty As Decimal, ByVal pSubstitutionType As String, ByVal pFromdate As DateTime, _
        ByVal pTodate As DateTime, ByVal pMultiLevel As Boolean, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pUser As String)

            If Not WMS.Logic.Consignee.Exists(pConsignee) Then
                Throw New M4NException(New Exception, "Consignee does not exists", "Consignee does not exists")
            End If
            If Not WMS.Logic.SKU.Exists(pConsignee, pSku) Then
                Throw New M4NException(New Exception, "Sku does not exists", "Sku does not exists")
            End If
            If Not WMS.Logic.SKU.Exists(pConsignee, pSubSku) Then
                Throw New M4NException(New Exception, "Substitute Sku does not exists", "Substitute Sku does not exists")
            End If

            If pTodate.Subtract(pFromdate).Days < 0 Then
                Throw New M4NException(New Exception, "To Date cannot precede From Date", "To Date cannot precede From Date")
            End If

            If pCompany <> String.Empty AndAlso Not WMS.Logic.Company.Exists(pConsignee, pCompany, pCompanyType) Then
                Throw New M4NException(New Exception, "Company does not exist.", "Company does not exist.")
            End If

            _consignee = pConsignee
            _sku = pSku

            _substitutesku = pSubSku
            _priority = pPriority
            _fromdate = pFromdate
            _todate = pTodate
            _skuqty = pSkuqty
            _substituteskuqty = pSubstituteSkuQty
            _substitutiontype = pSubstitutionType
            _multilevel = pMultiLevel

            _company = pCompany
            _companytype = pCompanyType
            _editdate = DateTime.Now
            _edituser = pUser
            _adddate = DateTime.Now
            _adduser = pUser

            Dim SQL As String = String.Format("INSERT INTO SKUSUBSTITUTE (CONSIGNEE, SKU, SUBSTITUTESKU, PRIORITY, FROMDATE, TODATE, ADDDATE, ADDUSER, EDITDATE, EDITUSER,SKUQTY,SUBSTITUTESKUQTY,SUBSTITUTIONTYPE,MULTYLEVEL,COMPANY,COMPANYTYPE)" & _
                "VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})", Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_substitutesku), _
                Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_fromdate), Made4Net.Shared.Util.FormatField(_todate), Made4Net.Shared.Util.FormatField(_adddate), _
                Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_skuqty), Made4Net.Shared.Util.FormatField(_substituteskuqty), _
                Made4Net.Shared.Util.FormatField(_substitutiontype), Made4Net.Shared.Util.FormatField(_multilevel), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype))
            DataInterface.RunSQL(SQL)
        End Sub

        Public Sub Delete()
            Dim sql As String
            sql = "delete from SKUSUBSTITUTE where " & WhereClause
            DataInterface.RunSQL(sql)
        End Sub

        Public Sub Update(ByVal pPriority As Int32, ByVal pSkuQty As Decimal, ByVal pSubstituteSkuQty As Decimal, ByVal pSubstitutionType As String, ByVal pMultiLevel As Boolean, _
        ByVal pCompany As String, ByVal pCompanyType As String, ByVal pFromdate As DateTime, ByVal pTodate As DateTime, ByVal pUser As String)
            If pTodate.Subtract(pFromdate).Days < 0 Then
                Throw New M4NException(New Exception, "To date is before from date", "To date is before from date")
            End If

            If pCompany <> String.Empty AndAlso Not WMS.Logic.Company.Exists(_consignee, pCompany, pCompanyType) Then
                Throw New M4NException(New Exception, "Company does not exist.", "Company does not exist")
            End If

            _priority = pPriority
            _fromdate = pFromdate
            _todate = pTodate
            _editdate = DateTime.Now
            _edituser = pUser

            _multilevel = pMultiLevel
            _skuqty = pSkuQty
            _substituteskuqty = pSubstituteSkuQty
            _substitutiontype = pSubstitutionType

            _company = pCompany
            _companytype = pCompanyType

            Dim sqlString As String = String.Format("UPDATE SKUSUBSTITUTE SET PRIORITY={0},FROMDATE={1},TODATE={2},EDITDATE={3},EDITUSER={4},MULTYLEVEL={5},SKUQTY={6},SUBSTITUTESKUQTY={7},SUBSTITUTIONTYPE={8},COMPANY={9},COMPANYTYPE={10} where {11} ", _
                Made4Net.Shared.Util.FormatField(_priority), Made4Net.Shared.Util.FormatField(_fromdate), Made4Net.Shared.Util.FormatField(_todate), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                Made4Net.Shared.Util.FormatField(_multilevel), Made4Net.Shared.Util.FormatField(_skuqty), Made4Net.Shared.Util.FormatField(_substituteskuqty), Made4Net.Shared.Util.FormatField(_substitutiontype), _
                Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), WhereClause)

            DataInterface.RunSQL(sqlString)
        End Sub

#End Region

    End Class

#End Region

#Region "Sku Substitutes Collection"

    Public Class SkuSubstitutesCollection
        Inherits ArrayList

#Region "Variables"

        Protected _consignee As String
        Protected _sku As String

#End Region

#Region "Properties"

        Default Public Shadows ReadOnly Property item(ByVal index As Int32) As SkuSubstitutes
            Get
                Return CType(MyBase.Item(index), SkuSubstitutes)
            End Get
        End Property

        Public ReadOnly Property SkuSubstitutes(ByVal pSubstituteSku As String) As SkuSubstitutes
            Get
                Dim i As Int32
                For i = 0 To Me.Count - 1
                    If item(i).SUBSTITUTESKU = pSubstituteSku Then
                        Return (CType(MyBase.Item(i), SkuSubstitutes))
                    End If
                Next
                Return Nothing
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New(ByVal pConsignee As String, ByVal pSku As String, Optional ByVal LoadAll As Boolean = True)
            _consignee = pConsignee
            _sku = pSku
            Dim SQL As String
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select * from SKUSUBSTITUTE where consignee = '" & _consignee & "' and SKU = '" & _sku & "' order by priority"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                Me.add(New SkuSubstitutes(dr))
            Next
        End Sub

#End Region

#Region "Methods"

        Public Shadows Function add(ByVal pObject As SkuSubstitutes) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As SkuSubstitutes)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As SkuSubstitutes)
            MyBase.Remove(pObject)
        End Sub

        Public Sub DeleteSKUSubstitute(ByRef pSKUSubstitute As SkuSubstitutes)
            pSKUSubstitute.Delete()
            Me.Remove(pSKUSubstitute)
        End Sub

        Public Sub DeleteAllSKUsubstitute()
            While Me.Count > 0
                DeleteSKUSubstitute(Me.item(0))
            End While
            Me.Clear()
        End Sub

#End Region

    End Class

#End Region

#End Region

#Region "BaseItemTypes"

    Public Class BaseItemTypes
        Public Const ALWAYS As String = "ALWAYS"
        Public Const NEVER As String = "NEVER"
    End Class

#End Region

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _sku As String = String.Empty


#End Region

#Region "Other Fields"

    Protected _skuattributescollection As SkuAttributes
    Protected _skuunitsofmeasure As SKUUOMCollection
    Protected _skubom As SKUBOMCollection
    Protected _skuSubstitutes As SkuSubstitutesCollection
    Protected _skudesc As String = String.Empty
    Protected _skuclass As SKUClass
    Protected _skuclassname As String
    Protected _skushortdesc As String = String.Empty
    Protected _manufacturersku As String = String.Empty
    Protected _vendorsku As String = String.Empty
    Protected _othersku As String = String.Empty
    Protected _skugroup As String = String.Empty
    Protected _status As Boolean
    Protected _inventory As Boolean
    Protected _newsku As Boolean
    Protected _initialstatus As String = ""
    Protected _velocity As String = String.Empty
    Protected _fifoindifference As Decimal
    Protected _onsitemin As Decimal
    Protected _onsitemax As Decimal
    Protected _lastcyclecount As DateTime
    Protected _cyclecountint As Int32
    Protected _lowlimitcount As Int32
    Protected _counttolerance As Int32
    Protected _dailydemand As Decimal
    Protected _dailypicks As Decimal
    Protected _preflocation As String = String.Empty
    Protected _prefwarehousearea As String = String.Empty
    Protected _prefputregion As String = String.Empty
    Protected _picture As String = String.Empty
    Protected _unitprice As Decimal
    Protected _picksortorder As String
    Protected _defaultuom As String
    Protected _defaultrecuom As String
    Protected _lowestuom As String
    Protected _hazclass As String
    Protected _transportationclass As String
    Protected _hutype As String
    Protected _overpickpct As Decimal = 1
    Protected _overreceivepct As Decimal = 1
    Protected _storageclass As String
    Protected _notes As String
    'PWMS-862 - Added  to set Opportunity Replenishment flag as true by default 
    Protected _oportunityrelpflag As Boolean = True
    'PWMS-862 - Ended  to set Opportunity Replenishment flag as true by default 
    Protected _newskuupdate As Boolean
    Protected _baseitem As String
    Protected _baseitemqty As Decimal
    Protected _autoadjustcountqty As Decimal = 0
    Protected _defaultrecloaduom As String = String.Empty
    Protected _overallocmode As String = String.Empty
    Protected _receivingweightcapturemethod As String = String.Empty
    Protected _shippingweightcapturemethod As String = String.Empty
    Protected _receivingweighttolerance As Decimal
    Protected _shippingweighttolerance As Decimal

    Protected _fullpickallocation As Integer

    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _parentSKU As String = String.Empty

#End Region

#End Region

#Region "Properties"
    Public ReadOnly Property IsWeightCaptureRequiredAtPicking As Boolean
        Get
            Return If(_skuclass Is Nothing, False, _skuclass.IsWeightCaptureRequiredAtPicking)
        End Get
    End Property
    Public ReadOnly Property HasShippingWeightCaptureMethod() As Boolean
        Get
            If _shippingweightcapturemethod = "SKUNET" OrElse _shippingweightcapturemethod = "SKUGROSS" Then
                Return True
            End If
        End Get
    End Property
    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where CONSIGNEE = '" & _consignee & "' And SKU = '" & _sku & "'"
        End Get
    End Property

    Public ReadOnly Property Attributes() As SkuAttributes
        Get
            Return _skuattributescollection
        End Get
    End Property

    Public Property PARENTSKU() As String
        Get
            Return _parentSKU
        End Get
        Set(ByVal value As String)
            _parentSKU = value
        End Set
    End Property

    Public Property UNITSOFMEASURE() As SKUUOMCollection
        Get
            Return _skuunitsofmeasure
        End Get
        Set(ByVal value As SKUUOMCollection)
            _skuunitsofmeasure = value
        End Set
    End Property

    Public ReadOnly Property SUBSTITUTESSKU() As SkuSubstitutesCollection
        Get
            Return _skuSubstitutes
        End Get
    End Property

    Public ReadOnly Property BOM() As SKUBOMCollection
        Get
            Return _skubom
        End Get
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property DEFAULTUOM() As String
        Get
            Return _defaultuom
        End Get
        Set(ByVal Value As String)
            _defaultuom = Value
        End Set
    End Property

    Public Property DEFAULTRECUOM() As String
        Get
            'added for RWMS-2525 Start
            If String.IsNullOrEmpty(_defaultrecuom) Then
                Return getLowestUom()
            Else
                If Not SKUUOM.Exists(_consignee, _sku, _defaultrecuom) Then
                    Return getLowestUom()
                End If
            End If
            'added for RWMS-2525 End
            Return _defaultrecuom
        End Get
        Set(ByVal Value As String)
            _defaultrecuom = Value
        End Set
    End Property

    Public Property SKU() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property TRANSPORTATIONCLASS() As String
        Get
            Return _transportationclass
        End Get
        Set(ByVal Value As String)
            _transportationclass = Value
        End Set
    End Property

    Public Property SKUClass() As SkuClass
        Get
            If _skuclassname <> String.Empty And (Not _skuclassname Is Nothing) And _skuclass Is Nothing Then
                _skuclass = New SkuClass(_skuclassname)
            End If
            Return _skuclass
        End Get
        Set(ByVal value As SkuClass)
            _skuclass = value
        End Set
    End Property

    Public Property SKUClassName() As String
        Get
            Return _skuclassname
        End Get
        Set(ByVal value As String)
            _skuclassname = value
            _skuclass = Nothing
        End Set
    End Property

    Public Property SKUDESC() As String
        Get
            Return _skudesc
        End Get
        Set(ByVal Value As String)
            _skudesc = Value
        End Set
    End Property

    Public Property PICKSORTORDER() As String
        Get
            Return _picksortorder
        End Get
        Set(ByVal Value As String)
            _picksortorder = Value
        End Set
    End Property

    Public Property SKUSHORTDESC() As String
        Get
            Return _skushortdesc
        End Get
        Set(ByVal Value As String)
            _skushortdesc = Value
        End Set
    End Property

    Public Property MANUFACTURERSKU() As String
        Get
            Return _manufacturersku
        End Get
        Set(ByVal Value As String)
            _manufacturersku = Value
        End Set
    End Property

    Public Property VENDORSKU() As String
        Get
            Return _vendorsku
        End Get
        Set(ByVal Value As String)
            _vendorsku = Value
        End Set
    End Property

    Public Property OTHERSKU() As String
        Get
            Return _othersku
        End Get
        Set(ByVal Value As String)
            _othersku = Value
        End Set
    End Property

    Public Property SKUGROUP() As String
        Get
            Return _skugroup
        End Get
        Set(ByVal Value As String)
            _skugroup = Value
        End Set
    End Property

    Public Property STATUS() As Boolean
        Get
            Return _status
        End Get
        Set(ByVal Value As Boolean)
            _status = Value
        End Set
    End Property

    Public Property INVENTORY() As Boolean
        Get
            Return _inventory
        End Get
        Set(ByVal Value As Boolean)
            _inventory = Value
        End Set
    End Property

    Public Property NEWSKU() As Boolean
        Get
            Return _newsku
        End Get
        Set(ByVal Value As Boolean)
            _newskuupdate = True
            _newsku = Value
        End Set
    End Property

    Public Property INITIALSTATUS() As String
        Get
            Return _initialstatus
        End Get
        Set(ByVal Value As String)
            _initialstatus = Value
        End Set
    End Property

    Public Property VELOCITY() As String
        Get
            Return _velocity
        End Get
        Set(ByVal Value As String)
            _velocity = Value
        End Set
    End Property

    Public Property FIFOINDIFFERENCE() As Decimal
        Get
            Return _fifoindifference
        End Get
        Set(ByVal Value As Decimal)
            _fifoindifference = Value
        End Set
    End Property

    Public Property ONSITEMIN() As Decimal
        Get
            Return _onsitemin
        End Get
        Set(ByVal Value As Decimal)
            _onsitemin = Value
        End Set
    End Property

    Public Property ONSITEMAX() As Decimal
        Get
            Return _onsitemax
        End Get
        Set(ByVal Value As Decimal)
            _onsitemax = Value
        End Set
    End Property

    Public Property LASTCYCLECOUNT() As DateTime
        Get
            Return _lastcyclecount
        End Get
        Set(ByVal Value As DateTime)
            _lastcyclecount = Value
        End Set
    End Property

    Public Property CYCLECOUNTINT() As Int32
        Get
            Return _cyclecountint
        End Get
        Set(ByVal Value As Int32)
            _cyclecountint = Value
        End Set
    End Property

    Public Property COUNTTOLERANCE() As Int32
        Get
            Return _counttolerance
        End Get
        Set(ByVal Value As Int32)
            _counttolerance = Value
        End Set
    End Property

    Public Property LOWLIMITCOUNT() As Int32
        Get
            Return _lowlimitcount
        End Get
        Set(ByVal Value As Int32)
            _lowlimitcount = Value
        End Set
    End Property

    Public Property PREFLOCATION() As String
        Get
            Return _preflocation
        End Get
        Set(ByVal Value As String)
            _preflocation = Value
        End Set
    End Property

    Public Property PREFWAREHOUSEAREA() As String
        Get
            Return _prefwarehousearea
        End Get
        Set(ByVal Value As String)
            _prefwarehousearea = Value
        End Set
    End Property

    Public Property LOWESTUOM() As String
        Get
            'Return getLowestUom()
            For Each currUOM As WMS.Logic.SKU.SKUUOM In Me.UNITSOFMEASURE
                If currUOM.LOWERUOM = String.Empty Then
                    Return currUOM.UOM
                End If
            Next
            Return getLowestUom()
        End Get
        Set(ByVal Value As String)
            _lowestuom = Value
        End Set
    End Property

    Public Property PICTURE() As String
        Get
            Return _picture
        End Get
        Set(ByVal Value As String)
            _picture = Value
        End Set
    End Property

    Public Property UNITPRICE() As Decimal
        Get
            Return _unitprice
        End Get
        Set(ByVal Value As Decimal)
            _unitprice = Value
        End Set
    End Property

    Public Property PREFPUTREGION() As String
        Get
            Return _prefputregion
        End Get
        Set(ByVal Value As String)
            _prefputregion = Value
        End Set
    End Property

    Public Property HAZCLASS() As String
        Get
            Return _hazclass
        End Get
        Set(ByVal Value As String)
            _hazclass = Value
        End Set
    End Property

    Public Property HUTYPE() As String
        Get
            Return _hutype
        End Get
        Set(ByVal Value As String)
            _hutype = Value
        End Set
    End Property

    Public Property OVERPICKPCT() As Decimal
        Get
            Return _overpickpct
        End Get
        Set(ByVal Value As Decimal)
            _overpickpct = Value
        End Set
    End Property

    Public Property OVERRECEIVEPCT() As Decimal
        Get
            Return _overreceivepct
        End Get
        Set(ByVal Value As Decimal)
            _overreceivepct = Value
        End Set
    End Property

    Public Property STORAGECLASS() As String
        Get
            Return _storageclass
        End Get
        Set(ByVal value As String)
            _storageclass = value
        End Set
    End Property

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal value As String)
            _notes = value
        End Set
    End Property

    Public Property OPORTUNITYRELPFLAG() As Boolean
        Get
            Return _oportunityrelpflag
        End Get
        Set(ByVal value As Boolean)
            _oportunityrelpflag = value
        End Set
    End Property

    Public Property BASEITEM() As String
        Get
            Return _baseitem
        End Get
        Set(ByVal value As String)
            _baseitem = value
        End Set
    End Property

    Public Property FULLPICKALLOCATION() As Integer
        Get
            Return _fullpickallocation
        End Get
        Set(ByVal value As Integer)
            _fullpickallocation = value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public Property DailyDemand() As Decimal
        Get
            Return _dailydemand
        End Get
        Set(ByVal value As Decimal)
            _dailydemand = value
        End Set
    End Property

    Public Property DailyPicks()
        Get
            Return _dailypicks
        End Get
        Set(ByVal value)
            _dailypicks = value
        End Set
    End Property

    Public Property AUTOADJUSTCOUNTQTY() As Decimal
        Get
            Return _autoadjustcountqty
        End Get
        Set(ByVal Value As Decimal)
            _autoadjustcountqty = Value
        End Set
    End Property

    Public Property BASEITEMQTY() As Decimal
        Get
            Return _baseitemqty
        End Get
        Set(ByVal Value As Decimal)
            _baseitemqty = Value
        End Set
    End Property

    Public Property DEFAULTRECLOADUOM() As String
        Get
            'Start PWMS-921 and RWMS-977 If DEFAULTRECLOADUOM is null or blank then take highest UOM of SKU   
            If String.IsNullOrEmpty(_defaultrecloaduom) Then
                Return getHighestUom()
                'added for RWMS-2525 Start
            Else
                If Not SKUUOM.Exists(_consignee, _sku, _defaultrecloaduom) Then
                    Return getHighestUom()
                End If
                'added for RWMS-2525 End
            End If
            'End PWMS-921 and RWMS-977  
            Return _defaultrecloaduom
        End Get
        Set(ByVal value As String)
            _defaultrecloaduom = value
        End Set
    End Property

    Public Property OVERALLOCMODE() As String
        Get
            Return _overallocmode
        End Get
        Set(ByVal value As String)
            _overallocmode = value
        End Set
    End Property

    Public Property RECEIVINGWEIGHTCAPTUREMETHOD() As String
        Get
            Return _receivingweightcapturemethod
        End Get
        Set(ByVal value As String)
            _receivingweightcapturemethod = value
        End Set
    End Property

    Public Property SHIPPINGWEIGHTCAPTUREMETHOD() As String
        Get
            Return _shippingweightcapturemethod
        End Get
        Set(ByVal value As String)
            _shippingweightcapturemethod = value
        End Set
    End Property

    Public Property RECEIVINGWEIGHTTOLERANCE() As Decimal
        Get
            Return _receivingweighttolerance
        End Get
        Set(ByVal value As Decimal)
            _receivingweighttolerance = value
        End Set
    End Property

    Public Property SHIPPINGWEIGHTTOLERANCE() As Decimal
        Get
            Return _shippingweighttolerance
        End Get
        Set(ByVal value As Decimal)
            _shippingweighttolerance = value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim pUser As String = Common.GetCurrentUser
        Dim pWhs As String = Warehouse.CurrentWarehouse
        Select Case CommandName.ToLower
            Case "printlabels"
                WMS.Logic.SKU.PrintLabel(ds, pUser, pWhs)
                Message = "Label Printed"
            Case "newskuattributes"
                Dim sk As SKU = New SKU(ds.Tables(0).Rows(0)("CONSIGNEE"), ds.Tables(0).Rows(0)("SKU"))
                sk.Attributes.PopulateFromDataTable(ds.Tables(0))
                sk.Attributes.Save()

            Case "updateskuattributes"
                Dim sk As SKU = New SKU(ds.Tables(0).Rows(0)("CONSIGNEE"), ds.Tables(0).Rows(0)("SKU"))
                sk.Attributes.PopulateFromDataTable(ds.Tables(0))
                sk.Attributes.Save()

            Case "setuom"
                Try
                    _consignee = ds.Tables(0).Rows(0)("CONSIGNEE")
                    _sku = ds.Tables(0).Rows(0)("SKU")
                Catch ex As Exception
                    Throw New ApplicationException("No data found")
                End Try
                Load()
                Dim puom As String = ds.Tables(0).Rows(0)("UOM")
                Dim peanupc As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("EANUPC"))
                Dim pgross As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("GROSSWEIGHT"))
                Dim pnet As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NetWeight"))
                Dim plen As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Length"))
                Dim pwidth As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("WIDTH"))
                Dim pheight As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("HEIGHT"))
                Dim pvolume As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("VOLUME"))
                Dim ploweruom As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LOWERUOM"))
                Dim punit As Decimal = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("UNITSPERMEASURE"))
                Dim pship As Boolean = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SHIPPABLE"))

                Dim pnestedQuantity As Boolean = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NestedQuantity"))
                Dim pnestedDepthDiff As Boolean = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NestedDepthDiff"))
                Dim pnestedHeightDiff As Boolean = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NestedHeightDiff"))
                Dim pnestedWidthDiff As Boolean = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NestedWidthDiff"))
                Dim pnestedVolumeDiff As Boolean = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("NestedVolumeDiff"))

                Dim plaborGrabType As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborGrabType"))
                Dim plaborHandlingType As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborHandlingType"))
                Dim plaborPackageType As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborPackageType"))
                Dim plaborPreparationType As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LaborPreparationType"))

                Me.SetUOM(puom, peanupc, pgross, pnet, plen, pwidth, pheight, pvolume, ploweruom, punit, pship, Common.GetCurrentUser, pnestedQuantity,
                            pnestedWidthDiff, pnestedHeightDiff, pnestedDepthDiff, pnestedVolumeDiff, plaborGrabType, plaborHandlingType, plaborPackageType,
                            plaborPreparationType)
            Case "deluom"
                Try
                    _consignee = ds.Tables(0).Rows(0)("CONSIGNEE")
                    _sku = ds.Tables(0).Rows(0)("SKU")
                Catch ex As Exception
                    Throw New ApplicationException("No data found")
                End Try
                Load()
                Dim pUOM As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("UOM"))
                UNITSOFMEASURE.DeleteUOM(pUOM)
            Case "newsku"
                If ds.Tables(0).Rows(0)("LOWESTUOM") Is Nothing Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot insert new SKU, lower UOM is empty", "Cannot insert new SKU, lower UOM is empty")
                    Throw m4nEx
                End If
                Dim oSKU As SKU = New SKU
                oSKU.Create(Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("CONSIGNEE")), Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SKU")),
                            Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("SKUDESC")), Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing, Nothing,
                            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                            Conversion.Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LOWESTUOM")), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 0, Common.GetCurrentUser(), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, 0)
                oSKU.Load()
                oSKU.SetUOM(ds.Tables(0).Rows(0)("LOWESTUOM"), Nothing, 0, 0, 0, 0, 0, 0, String.Empty, 1, True, pUser, 0, 0, 0, 0, 0, "", "", "", "")
                'oSku.CONSIGNEE = ds.Tables(0).Rows(0)("CONSIGNEE")
                'oSku.SKU = ds.Tables(0).Rows(0)("SKU")
                'oSku.SKUDESC = ds.Tables(0).Rows(0)("SKUDESC")
                'oSKU.Save()
                'oSKU.Load()
                'oSKU.SetUOM(ds.Tables(0).Rows(0)("LOWESTUOM"), Nothing, 0, 0, 0, 0, 0, 0, String.Empty, 1, True, pUser)
            Case "updatesku"
                Dim oSKU As SKU = New SKU(ds.Tables(0).Rows(0)("CONSIGNEE"), ds.Tables(0).Rows(0)("SKU"))
                oSKU.CONSIGNEE = ds.Tables(0).Rows(0)("CONSIGNEE")
                oSKU.SKU = ds.Tables(0).Rows(0)("SKU")
                oSKU.SKUDESC = ds.Tables(0).Rows(0)("SKUDESC")
                Try
                    oSKU.DEFAULTRECUOM = ds.Tables(0).Rows(0)("DEFAULTRECUOM")
                Catch ex As Exception
                End Try
                Try
                    oSKU.DEFAULTUOM = ds.Tables(0).Rows(0)("DEFAULTUOM")
                Catch ex As Exception
                End Try
                Try
                    oSKU.UNITPRICE = ds.Tables(0).Rows(0)("UNITPRICE")
                Catch ex As Exception
                End Try
                oSKU.Save()
                'oSKU.SetUOM(ds.Tables(0).Rows(0)("LOWESTUOM"), Nothing, 0, 0, 0, 0, 0, 0, String.Empty, 1, True, pUser)
        End Select
    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pSKU As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _sku = pSKU
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetSKU(ByVal pCONSIGNEE As String, ByVal pSKU As String) As SKU
        Return New SKU(pCONSIGNEE, pSKU)
    End Function
    Public Shared Function weightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean

        If pSKU Is Nothing Then
            Return False
        End If
        If pSKU.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Or pSKU.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
            Return False
        End If
        If Not pSKU.SKUClass Is Nothing Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If (oAtt.Name.ToUpper = "WEIGHT" Or oAtt.Name.ToUpper = "WGT") AndAlso
                (oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Required) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function
    Public Shared Sub PrintLabel(ByVal ds As DataSet, ByVal pUser As String, ByVal pWarehouse As String)

        'Added for RWMS-2047 and RWMS-1637   
        If Not Made4Net.Label.LabelHandler.Factory.GetNewLableHandler().ValidateLabel("SKU") Then
            Throw New M4NException(New Exception(), "SKU Label Not Configured.", "SKU Label Not Configured.")
        Else
            Dim qSender As New QMsgSender
            Dim dr As DataRow
            For Each dr In ds.Tables(0).Rows
                qSender.Clear()
                qSender.Add("LABELNAME", "SKU")
                qSender.Add("LabelType", "SKU")
                Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
                ht.Hash.Add("CONSIGNEE", dr("CONSIGNEE"))
                ht.Hash.Add("SKU", dr("SKU"))
                qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
                qSender.Add("PRINTER", "")
                qSender.Add("CONSIGNEE", dr("CONSIGNEE"))
                qSender.Add("SKU", dr("SKU"))
                qSender.Send("Label", "SKU Label")
            Next
        End If
        'Ended for RWMS-2047 and RWMS-1637   

        'Dim qSender As New QMsgSender
        'Dim dr As DataRow
        'For Each dr In ds.Tables(0).Rows
        '    qSender.Clear()
        '    'qSender.Add("LABELNAME", "SKU")
        '    'qSender.Add("WAREHOUSE", pWarehouse)
        '    ''qSender.Add("PRINTER", "\\SRV1\WCPE16")
        '    'qSender.Add("CONSIGNEE", dr("CONSIGNEE"))
        '    'qSender.Add("SKU", dr("SKU"))
        '    'qSender.Send("Label", "Sku Label")
        '    qSender.Add("LABELNAME", "SKU")
        '    qSender.Add("LabelType", "SKU")
        '    Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        '    ht.Hash.Add("CONSIGNEE", dr("CONSIGNEE"))
        '    ht.Hash.Add("SKU", dr("SKU"))
        '    qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        '    qSender.Add("PRINTER", "")
        '    qSender.Add("CONSIGNEE", dr("CONSIGNEE"))
        '    qSender.Add("SKU", dr("SKU"))
        '    qSender.Send("Label", "SKU Label")
        'Next

    End Sub

    Public Sub PrintLabel()
        'Dim qSender As New QMsgSender
        'qSender.Add("LABELNAME", "SKU")
        'qSender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        'qSender.Add("CONSIGNEE", _consignee)
        'qSender.Add("SKU", _sku)
        'qSender.Send("Label", "Sku Label")
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "SKU")
        qSender.Add("LabelType", "SKU")
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Add("PRINTER", "")
        qSender.Add("CONSIGNEE", _consignee)
        qSender.Add("SKU", _sku)
        qSender.Send("Label", "SKU Label")
    End Sub

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM SKU " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
        End If
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        Try
            If Not dr.IsNull("CLASSNAME") Then
                _skuclass = New SkuClass(dr.Item("CLASSNAME"))
                _skuclassname = dr.Item("CLASSNAME")
            End If
        Catch ex As Exception
        End Try
        If Not dr.IsNull("SKUDESC") Then _skudesc = dr.Item("SKUDESC")
        If Not dr.IsNull("SKUSHORTDESC") Then _skushortdesc = dr.Item("SKUSHORTDESC")
        If Not dr.IsNull("MANUFACTURERSKU") Then _manufacturersku = dr.Item("MANUFACTURERSKU")
        If Not dr.IsNull("VENDORSKU") Then _vendorsku = dr.Item("VENDORSKU")
        If Not dr.IsNull("OTHERSKU") Then _othersku = dr.Item("OTHERSKU")
        If Not dr.IsNull("SKUGROUP") Then _skugroup = dr.Item("SKUGROUP")
        If Not dr.IsNull("PARENTSKU") Then _parentSKU = dr.Item("PARENTSKU")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("INVENTORY") Then _inventory = dr.Item("INVENTORY")
        If Not dr.IsNull("NEWSKU") Then _newsku = dr.Item("NEWSKU")
        If Not dr.IsNull("INITIALSTATUS") Then _initialstatus = dr.Item("INITIALSTATUS")
        If Not dr.IsNull("VELOCITY") Then _velocity = dr.Item("VELOCITY")
        If Not dr.IsNull("FIFOINDIFFERENCE") Then _fifoindifference = dr.Item("FIFOINDIFFERENCE")
        If Not dr.IsNull("ONSITEMIN") Then _onsitemin = dr.Item("ONSITEMIN")
        If Not dr.IsNull("ONSITEMAX") Then _onsitemax = dr.Item("ONSITEMAX")
        If Not dr.IsNull("LASTCYCLECOUNT") Then _lastcyclecount = dr.Item("LASTCYCLECOUNT")
        If Not dr.IsNull("CYCLECOUNTINT") Then _cyclecountint = dr.Item("CYCLECOUNTINT")
        If Not dr.IsNull("LOWLIMITCOUNT") Then _lowlimitcount = dr.Item("LOWLIMITCOUNT")
        If Not dr.IsNull("COUNTTOLERANCE") Then _counttolerance = dr.Item("COUNTTOLERANCE") Else _counttolerance = 0
        If Not dr.IsNull("PREFLOCATION") Then _preflocation = dr.Item("PREFLOCATION")
        If Not dr.IsNull("PREFWAREHOUSEAREA") Then _prefwarehousearea = dr.Item("PREFWAREHOUSEAREA")
        If Not dr.IsNull("PREFPUTREGION") Then _prefputregion = dr.Item("PREFPUTREGION")
        If Not dr.IsNull("PICTURE") Then _picture = dr.Item("PICTURE")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("UNITPRICE") Then _unitprice = dr.Item("UNITPRICE")
        If Not dr.IsNull("DEFAULTUOM") Then _defaultuom = dr.Item("DEFAULTUOM")
        If Not dr.IsNull("DEFAULTRECUOM") Then _defaultrecuom = dr.Item("DEFAULTRECUOM")
        If Not dr.IsNull("HAZCLASS") Then _hazclass = dr.Item("HAZCLASS")
        If Not dr.IsNull("OVERPICKPCT") Then _overpickpct = dr.Item("OVERPICKPCT")
        If Not dr.IsNull("OVERRECEIVEPCT") Then _overreceivepct = dr.Item("OVERRECEIVEPCT")
        If Not dr.IsNull("transportationclass") Then _transportationclass = dr.Item("transportationclass")
        If Not dr.IsNull("STORAGECLASS") Then _storageclass = dr.Item("STORAGECLASS")
        If Not dr.IsNull("OPORTUNITYRELPFLAG") Then _oportunityrelpflag = dr.Item("OPORTUNITYRELPFLAG")
        If Not dr.IsNull("HUTYPE") Then _hutype = dr.Item("HUTYPE")
        If Not dr.IsNull("BASEITEM") Then _baseitem = dr.Item("BASEITEM")
        If Not dr.IsNull("BASEITEMQTY") Then _baseitemqty = dr.Item("BASEITEMQTY")
        If Not dr.IsNull("AUTOADJUSTCOUNTQTY") Then _autoadjustcountqty = dr.Item("AUTOADJUSTCOUNTQTY")
        If Not dr.IsNull("DEFAULTRECLOADUOM") Then _defaultrecloaduom = dr.Item("DEFAULTRECLOADUOM")
        If Not dr.IsNull("OVERALLOCMODE") Then _overallocmode = dr.Item("OVERALLOCMODE")
        If Not dr.IsNull("RECEIVINGWEIGHTCAPTUREMETHOD") Then _receivingweightcapturemethod = dr.Item("RECEIVINGWEIGHTCAPTUREMETHOD")
        If Not dr.IsNull("SHIPPINGWEIGHTCAPTUREMETHOD") Then _shippingweightcapturemethod = dr.Item("SHIPPINGWEIGHTCAPTUREMETHOD")
        If Not dr.IsNull("RECEIVINGWEIGHTTOLERANCE") Then _receivingweighttolerance = dr.Item("RECEIVINGWEIGHTTOLERANCE")
        If Not dr.IsNull("SHIPPINGWEIGHTTOLERANCE") Then _shippingweighttolerance = dr.Item("SHIPPINGWEIGHTTOLERANCE")

        If Not dr.IsNull("FULLPICKALLOCATION") Then _fullpickallocation = dr.Item("FULLPICKALLOCATION")

        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _skuattributescollection = New SkuAttributes(_consignee, _sku)

        _skuunitsofmeasure = New SKUUOMCollection(_consignee, _sku)
        Try
            _skubom = New SKUBOMCollection(_consignee, _sku)
        Catch ex As Exception
        End Try

        Try
            _skuSubstitutes = New SkuSubstitutesCollection(_consignee, _sku)
        Catch ex As Exception
        End Try
    End Sub

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pSku As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from SKU where consignee = '{0}' and SKU = '{1}'", pConsignee, pSku)
        Dim Exist As Boolean
        Exist = Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        If Not Exist Then
            Dim count As Int32
            ' Lets Change sku exits to use new view for checking if sku is in database
            sql = String.Format("SELECT COUNT(1) FROM vSKUCODE WHERE CONSIGNEE = '{0}' AND SKUCODE = '{1}'", pConsignee, pSku)
            'sql = String.Format("SELECT count(1) FROM SKU where (MANUFACTURERSKU ='{0}' or VENDORSKU ='{0}' or OTHERSKU ='{0}') and consignee = '{1}'", pSku, pConsignee)
            count = DataInterface.ExecuteScalar(sql)
            If count = 1 Then
                Exist = True
            Else
                Exist = False
            End If
        End If
        Return Exist
    End Function

    'Added for RWMS-536
    Public Shared Function SKUExists(ByVal pConsignee As String, ByVal pSku As String) As Boolean
        Dim sql As String = String.Format("Select count(*) from SKU where consignee = '{0}' and SKU = '{1}'", pConsignee, pSku)
        Dim Exist As Boolean
        Exist = Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        Return Exist
    End Function
    'End Added for RWMS-536

    Public Shared Function isNew(ByVal pConsignee As String, ByVal pSku As String) As Boolean
        Dim sql As String = String.Format("Select newsku from SKU where consignee = '{0}' and SKU = '{1}'", pConsignee, pSku)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function isNew() As Boolean
        Return WMS.Logic.SKU.isNew(_consignee, _sku)
    End Function

    Public Function UOM(ByVal pUom As String) As SKUUOM
        Return _skuunitsofmeasure.UOM(pUom)
    End Function

#End Region

    Public Sub Save()

        Dim sql As String
        'Dim classname As String
        If Me.SKUClass Is Nothing Then
            _skuclassname = ""
        Else
            _skuclassname = Me.SKUClass.ClassName
        End If
        Dim EventType As Int32
        'Commented for RWMS-536
        'If WMS.Logic.SKU.Exists(_consignee, _sku) Then
        'End Commented for RWMS-536
        'Added for RWMS-536
        If WMS.Logic.SKU.SKUExists(_consignee, _sku) Then
            'End Added for RWMS-536
            validate(True)
            _editdate = DateTime.Now
            sql = String.Format("UPDATE SKU SET SKUDESC={0},SKUSHORTDESC={1},MANUFACTURERSKU={2}, VENDORSKU={3}, OTHERSKU={4}, SKUGROUP={5},NEWSKU={6}," &
                          "OPORTUNITYRELPFLAG={7},STATUS = {8},CLASSNAME={9},INVENTORY={10},INITIALSTATUS={11},VELOCITY={12},FIFOINDIFFERENCE={13},ONSITEMIN={14}" &
                          ",ONSITEMAX={15},LASTCYCLECOUNT={16},CYCLECOUNTINT={17},LOWLIMITCOUNT={18},PREFLOCATION={19},PREFWAREHOUSEAREA={20},PREFPUTREGION={21}," &
                          "PICKSORTORDER={22},DEFAULTUOM={23},OVERPICKPCT={24},UNITPRICE={25},OVERRECEIVEPCT={26},DAILYDEMAND={27},DAILYPICKS={28},TRANSPORTATIONCLASS={29}," &
                          "HAZCLASS={30},HUTYPE={31},DEFAULTRECUOM={32},STORAGECLASS={33},NOTES={34},BASEITEM={35},COUNTTOLERANCE={36},AUTOADJUSTCOUNTQTY={37},EDITUSER={38},EDITDATE={39},DEFAULTRECLOADUOM={40},OVERALLOCMODE={41},BASEITEMQTY={42}, RECEIVINGWEIGHTCAPTUREMETHOD={43}, SHIPPINGWEIGHTCAPTUREMETHOD={44}, RECEIVINGWEIGHTTOLERANCE={45}, SHIPPINGWEIGHTTOLERANCE={46},ParentSKU= {47} {48}",
                         Made4Net.Shared.Util.FormatField(_skudesc), Made4Net.Shared.Util.FormatField(_skushortdesc), Made4Net.Shared.Util.FormatField(_manufacturersku),
                          Made4Net.Shared.Util.FormatField(_vendorsku), Made4Net.Shared.Util.FormatField(_othersku), Made4Net.Shared.Util.FormatField(_skugroup),
                          Made4Net.Shared.Util.FormatField(_newsku), Made4Net.Shared.Util.FormatField(_oportunityrelpflag), Made4Net.Shared.Util.FormatField(_status),
                          Made4Net.Shared.Util.FormatField(_skuclassname), Made4Net.Shared.Util.FormatField(_inventory), Made4Net.Shared.Util.FormatField(_initialstatus),
                          Made4Net.Shared.Util.FormatField(_velocity), Made4Net.Shared.Util.FormatField(_fifoindifference), Made4Net.Shared.Util.FormatField(_onsitemin),
                          Made4Net.Shared.Util.FormatField(_onsitemax), Made4Net.Shared.Util.FormatField(_lastcyclecount), Made4Net.Shared.Util.FormatField(_cyclecountint),
                          Made4Net.Shared.Util.FormatField(_lowlimitcount), Made4Net.Shared.Util.FormatField(_preflocation), Made4Net.Shared.Util.FormatField(_prefwarehousearea),
                          Made4Net.Shared.Util.FormatField(_prefputregion), Made4Net.Shared.Util.FormatField(_picksortorder), Made4Net.Shared.Util.FormatField(_defaultuom),
                          Made4Net.Shared.Util.FormatField(_overpickpct), Made4Net.Shared.Util.FormatField(_unitprice), Made4Net.Shared.Util.FormatField(_overreceivepct),
                          Made4Net.Shared.Util.FormatField(_dailydemand), Made4Net.Shared.Util.FormatField(_dailypicks), Made4Net.Shared.Util.FormatField(_transportationclass),
                          Made4Net.Shared.Util.FormatField(_hazclass), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_defaultrecuom),
                          Made4Net.Shared.Util.FormatField(_storageclass), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_baseitem),
                          Made4Net.Shared.Util.FormatField(_counttolerance), Made4Net.Shared.Util.FormatField(_autoadjustcountqty), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate),
                          Made4Net.Shared.Util.FormatField(_defaultrecloaduom), Made4Net.Shared.Util.FormatField(_overallocmode), Made4Net.Shared.Util.FormatField(_baseitemqty),
                          Made4Net.Shared.Util.FormatField(_receivingweightcapturemethod), Made4Net.Shared.Util.FormatField(_shippingweightcapturemethod),
                          Made4Net.Shared.Util.FormatField(_receivingweighttolerance), Made4Net.Shared.Util.FormatField(_shippingweighttolerance), Made4Net.Shared.Util.FormatField(_parentSKU), WhereClause)


            EventType = WMS.Logic.WMSEvents.EventType.SkuUpdated
        Else
            If Not _newskuupdate Then
                _newsku = True
            End If
            validate(False)
            _adddate = DateTime.Now

            sql = String.Format("INSERT INTO SKU (CONSIGNEE,SKU,SKUDESC,SKUSHORTDESC,MANUFACTURERSKU,VENDORSKU,OTHERSKU,SKUGROUP,NEWSKU,PARENTSKU," &
                          "OPORTUNITYRELPFLAG,STATUS,CLASSNAME,INVENTORY,INITIALSTATUS,VELOCITY,FIFOINDIFFERENCE,ONSITEMIN,ONSITEMAX,LASTCYCLECOUNT," &
                          "CYCLECOUNTINT,LOWLIMITCOUNT,PREFLOCATION,PREFWAREHOUSEAREA,PREFPUTREGION,PICKSORTORDER,DEFAULTUOM,OVERPICKPCT,UNITPRICE," &
                          "OVERRECEIVEPCT,DAILYDEMAND,DAILYPICKS,TRANSPORTATIONCLASS,HAZCLASS,HUTYPE,DEFAULTRECUOM,STORAGECLASS,NOTES,BASEITEM, BASEITEMQTY," &
                          "COUNTTOLERANCE, AUTOADJUSTCOUNTQTY ,ADDUSER,ADDDATE,EDITUSER,EDITDATE,DEFAULTRECLOADUOM,OVERALLOCMODE, RECEIVINGWEIGHTCAPTUREMETHOD, SHIPPINGWEIGHTCAPTUREMETHOD, RECEIVINGWEIGHTTOLERANCE, SHIPPINGWEIGHTTOLERANCE) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}," &
                          "{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51})",
                           Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_skudesc),
                          Made4Net.Shared.Util.FormatField(_skushortdesc), Made4Net.Shared.Util.FormatField(_manufacturersku), Made4Net.Shared.Util.FormatField(_vendorsku),
                          Made4Net.Shared.Util.FormatField(_othersku), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_newsku), Made4Net.Shared.Util.FormatField(_parentSKU),
                          Made4Net.Shared.Util.FormatField(_oportunityrelpflag), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_skuclassname),
                          Made4Net.Shared.Util.FormatField(_inventory), Made4Net.Shared.Util.FormatField(_initialstatus), Made4Net.Shared.Util.FormatField(_velocity),
                          Made4Net.Shared.Util.FormatField(_fifoindifference), Made4Net.Shared.Util.FormatField(_onsitemin), Made4Net.Shared.Util.FormatField(_onsitemax),
                          Made4Net.Shared.Util.FormatField(_lastcyclecount), Made4Net.Shared.Util.FormatField(_cyclecountint), Made4Net.Shared.Util.FormatField(_lowlimitcount),
                          Made4Net.Shared.Util.FormatField(_preflocation), Made4Net.Shared.Util.FormatField(_prefwarehousearea), Made4Net.Shared.Util.FormatField(_prefputregion),
                          Made4Net.Shared.Util.FormatField(_picksortorder), Made4Net.Shared.Util.FormatField(_defaultuom), Made4Net.Shared.Util.FormatField(_overpickpct),
                          Made4Net.Shared.Util.FormatField(_unitprice), Made4Net.Shared.Util.FormatField(_overreceivepct), Made4Net.Shared.Util.FormatField(_dailydemand),
                          Made4Net.Shared.Util.FormatField(_dailypicks), Made4Net.Shared.Util.FormatField(_transportationclass), Made4Net.Shared.Util.FormatField(_hazclass),
                          Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_defaultrecuom), Made4Net.Shared.Util.FormatField(_storageclass),
                          Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_baseitem), Made4Net.Shared.Util.FormatField(_baseitemqty), Made4Net.Shared.Util.FormatField(_counttolerance), Made4Net.Shared.Util.FormatField(_autoadjustcountqty),
                          Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_defaultrecloaduom), Made4Net.Shared.Util.FormatField(_overallocmode),
                          Made4Net.Shared.Util.FormatField(_receivingweightcapturemethod), Made4Net.Shared.Util.FormatField(_shippingweightcapturemethod),
                          Made4Net.Shared.Util.FormatField(_receivingweighttolerance), Made4Net.Shared.Util.FormatField(_shippingweighttolerance))

            EventType = WMS.Logic.WMSEvents.EventType.SkuCreated
        End If
        DataInterface.RunSQL(sql)

        Dim em As IEventManagerQ = EventManagerQ.Factory.NewEventManagerQ()
        em.Add("EVENT", EventType)
        em.Add("CONSIGNEE", _consignee)
        em.Add("SKU", _sku)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))

        Load()
    End Sub

    Public Sub Create(ByVal pConsignee As String, ByVal pSku As String, ByVal pSkuDesc As String, ByVal pShortSkuDesc As String, ByVal pManufactureSku As String, ByVal pVendorSku As String,
        ByVal pOtherSku As String, ByVal pSkuGroup As String, ByVal pNewSku As Boolean, ByVal pOportunityRelp As Boolean, ByVal pStatus As Boolean, ByVal pClassName As String,
        ByVal pInventory As Boolean, ByVal pInitialStatus As String, ByVal pVelocity As String, ByVal pFifoWindow As Decimal, ByVal pOnSiteMin As Decimal,
        ByVal pOnSiteMax As Decimal, ByVal pLastCycleCount As DateTime, ByVal pCycleCount As Int32, ByVal pLowLimitCount As Int32,
        ByVal pPrefLocation As String, ByVal pPrefWarehouseArea As String, ByVal pPrefPutRegion As String, ByVal pPickSortOrder As String,
        ByVal pDefaultUom As String, ByVal pOverPickpct As Decimal, ByVal pUnitPrice As Decimal, ByVal pOverReceivePct As Decimal,
        ByVal pDailyDemand As Decimal, ByVal pDailyPicks As Decimal, ByVal pTransportationClass As String, ByVal pHazardClass As String,
        ByVal pHUType As String, ByVal pDefaultRecUom As String, ByVal pStorageClass As String, ByVal pNotes As String, ByVal pBaseItem As String, ByVal pBaseItemQty As Decimal,
        ByVal pUser As String, ByVal pCountTolerance As Decimal, ByVal pAutoAdjustCountQty As Decimal, ByVal pPicture As String, ByVal pDefaultRecLoadUom As String, ByVal pOverAllocMode As String,
        ByVal pReceivingWeightCaptureMethod As String, ByVal pShippingWeightCaptureMethod As String,
        ByVal pReceivingWeightTolerance As Int32, ByVal pShippingWeightTolerance As Int32, ByVal pFullPickAllocation As Integer)

        _consignee = pConsignee
        _sku = pSku
        _skudesc = pSkuDesc
        _skushortdesc = pShortSkuDesc
        _manufacturersku = pManufactureSku
        _vendorsku = pVendorSku

        _othersku = pOtherSku
        _skugroup = pSkuGroup
        _newsku = pNewSku
        'PWMS-862 - Added  to set Opportunity Replenishment flag as true by default 
        _oportunityrelpflag = True
        'PWMS-862 - Ended  to set Opportunity Replenishment flag as true by default 
        _status = pStatus
        _skuclassname = pClassName

        _inventory = pInventory
        If Not pInitialStatus Is Nothing Then _initialstatus = pInitialStatus
        If Not pVelocity Is Nothing Then _velocity = pVelocity
        _fifoindifference = pFifoWindow
        _onsitemin = pOnSiteMin

        _onsitemax = pOnSiteMax
        _lastcyclecount = pLastCycleCount
        _cyclecountint = pCycleCount
        _lowlimitcount = pLowLimitCount

        _preflocation = pPrefLocation
        _prefwarehousearea = pPrefWarehouseArea
        _prefputregion = pPrefPutRegion
        _picksortorder = pPickSortOrder

        _defaultuom = pDefaultUom
        _overpickpct = pOverPickpct
        _unitprice = pUnitPrice
        _overreceivepct = pOverReceivePct

        _dailydemand = pDailyDemand
        _dailypicks = pDailyPicks
        _transportationclass = pTransportationClass
        _hazclass = pHazardClass

        _hutype = pHUType
        _defaultrecuom = pDefaultRecUom
        _storageclass = pStorageClass
        _notes = pNotes
        _baseitem = pBaseItem
        _baseitemqty = pBaseItemQty
        _autoadjustcountqty = pAutoAdjustCountQty

        _edituser = pUser
        _editdate = DateTime.Now

        _counttolerance = pCountTolerance

        _adduser = pUser

        _defaultrecloaduom = pDefaultRecLoadUom
        _overallocmode = pOverAllocMode
        _receivingweightcapturemethod = pReceivingWeightCaptureMethod
        _shippingweightcapturemethod = pShippingWeightCaptureMethod
        _receivingweighttolerance = pReceivingWeightTolerance
        _shippingweighttolerance = pShippingWeightTolerance

        _fullpickallocation = pFullPickAllocation

        validate(False)
        'Save()
        _adddate = Date.Now
        _defaultrecuom = _defaultuom
        _overpickpct = 1
        _overreceivepct = 1
        Dim sql As String = String.Format("INSERT INTO SKU " &
                                  "(CONSIGNEE,SKU,SKUDESC," &
                                  "SKUSHORTDESC,MANUFACTURERSKU,VENDORSKU," &
                                  "OTHERSKU,SKUGROUP,NEWSKU,ParentSKU," &
                                  "OPORTUNITYRELPFLAG,STATUS,CLASSNAME," &
                                  "INVENTORY,INITIALSTATUS,VELOCITY," &
                                  "FIFOINDIFFERENCE,ONSITEMIN,ONSITEMAX," &
                                  "LASTCYCLECOUNT,CYCLECOUNTINT,LOWLIMITCOUNT," &
                                  "PREFLOCATION,PREFWAREHOUSEAREA,PREFPUTREGION," &
                                  "PICKSORTORDER,DEFAULTUOM,OVERPICKPCT," &
                                  "UNITPRICE,OVERRECEIVEPCT,DAILYDEMAND," &
                                  "DAILYPICKS,TRANSPORTATIONCLASS,HAZCLASS," &
                                  "HUTYPE,DEFAULTRECUOM,STORAGECLASS," &
                                  " NOTES,BASEITEM,BASEITEMQTY," &
                                  "COUNTTOLERANCE,AUTOADJUSTCOUNTQTY,ADDUSER," &
                                  "ADDDATE,EDITUSER,EDITDATE," &
                                  "DEFAULTRECLOADUOM,OVERALLOCMODE,RECEIVINGWEIGHTCAPTUREMETHOD," &
                                  "SHIPPINGWEIGHTCAPTUREMETHOD, RECEIVINGWEIGHTTOLERANCE, SHIPPINGWEIGHTTOLERANCE) VALUES({0},{1},{2},{3},{4},{5},{6},{7},{8},{51},{9},{10},{11},{12},{13},{14}," &
                                  "{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50})",
                                  Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_skudesc),
                                  Made4Net.Shared.Util.FormatField(_skushortdesc), Made4Net.Shared.Util.FormatField(_manufacturersku), Made4Net.Shared.Util.FormatField(_vendorsku),
                                  Made4Net.Shared.Util.FormatField(_othersku), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(_newsku),
                                  Made4Net.Shared.Util.FormatField(_oportunityrelpflag), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_skuclassname),
                                  Made4Net.Shared.Util.FormatField(_inventory), Made4Net.Shared.Util.FormatField(_initialstatus), Made4Net.Shared.Util.FormatField(_velocity),
                                  Made4Net.Shared.Util.FormatField(_fifoindifference), Made4Net.Shared.Util.FormatField(_onsitemin), Made4Net.Shared.Util.FormatField(_onsitemax),
                                  Made4Net.Shared.Util.FormatField(_lastcyclecount), Made4Net.Shared.Util.FormatField(_cyclecountint), Made4Net.Shared.Util.FormatField(_lowlimitcount),
                                  Made4Net.Shared.Util.FormatField(_preflocation), Made4Net.Shared.Util.FormatField(_prefwarehousearea), Made4Net.Shared.Util.FormatField(_prefputregion),
                                  Made4Net.Shared.Util.FormatField(_picksortorder), Made4Net.Shared.Util.FormatField(_defaultuom), Made4Net.Shared.Util.FormatField(_overpickpct),
                                  Made4Net.Shared.Util.FormatField(_unitprice), Made4Net.Shared.Util.FormatField(_overreceivepct), Made4Net.Shared.Util.FormatField(_dailydemand),
                                  Made4Net.Shared.Util.FormatField(_dailypicks), Made4Net.Shared.Util.FormatField(_transportationclass), Made4Net.Shared.Util.FormatField(_hazclass),
                                  Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_defaultrecuom), Made4Net.Shared.Util.FormatField(_storageclass),
                                  Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_baseitem), Made4Net.Shared.Util.FormatField(_baseitemqty),
                                  Made4Net.Shared.Util.FormatField(_counttolerance), Made4Net.Shared.Util.FormatField(_autoadjustcountqty), Made4Net.Shared.Util.FormatField(_adduser),
                                  Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate),
                                  Made4Net.Shared.Util.FormatField(_defaultrecloaduom), Made4Net.Shared.Util.FormatField(_overallocmode), Made4Net.Shared.Util.FormatField(_receivingweightcapturemethod),
                    Made4Net.Shared.Util.FormatField(_shippingweightcapturemethod), Made4Net.Shared.Util.FormatField(_receivingweighttolerance), Made4Net.Shared.Util.FormatField(_shippingweighttolerance), Made4Net.Shared.Util.FormatField(_parentSKU))

        DataInterface.RunSQL(sql)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.SkuCreated
        em.Add("EVENT", EventType)
        em.Add("CONSIGNEE", _consignee)
        em.Add("SKU", _sku)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub Update(ByVal pSkuDesc As String, ByVal pShortSkuDesc As String, ByVal pManufactureSku As String, ByVal pVendorSku As String,
        ByVal pOtherSku As String, ByVal pSkuGroup As String, ByVal pNewSku As Boolean, ByVal pOportunityRelp As Boolean, ByVal pStatus As Boolean, ByVal pClassName As String,
        ByVal pInventory As Boolean, ByVal pInitialStatus As String, ByVal pVelocity As String, ByVal pFifoWindow As Decimal, ByVal pOnSiteMin As Decimal,
        ByVal pOnSiteMax As Decimal, ByVal pLastCycleCount As DateTime, ByVal pCycleCount As Int32, ByVal pLowLimitCount As Int32,
        ByVal pPrefLocation As String, ByVal pPrefWarehouseArea As String, ByVal pPrefPutRegion As String, ByVal pPickSortOrder As String,
        ByVal pDefaultUom As String, ByVal pOverPickpct As Decimal, ByVal pUnitPrice As Decimal, ByVal pOverReceivePct As Decimal,
        ByVal pDailyDemand As Decimal, ByVal pDailyPicks As Decimal, ByVal pTransportationClass As String, ByVal pHazardClass As String,
        ByVal pHUType As String, ByVal pDefaultRecUom As String, ByVal pStorageClass As String, ByVal pNotes As String, ByVal pBaseItem As String, ByVal pBaseItemQty As Decimal,
        ByVal pUser As String, ByVal pCountTolerance As String, ByVal pAutoAdjustCountQty As Decimal, ByVal pPicture As String, ByVal pDefaultRecLoadUom As String, ByVal pOverAllocMode As String,
        ByVal pReceivingWeightCaptureMethod As String, ByVal pShippingWeightCaptureMethod As String,
        ByVal pReceivingWeightTolerance As Int32, ByVal pShippingWeightTolerance As Int32, ByVal pFullPickAllocation As Integer, ByVal parentSKU As String)
        _skudesc = pSkuDesc
        _skushortdesc = pShortSkuDesc
        _manufacturersku = pManufactureSku
        _vendorsku = pVendorSku

        _othersku = pOtherSku
        _skugroup = pSkuGroup
        _newsku = pNewSku
        'PWMS-862 - Added  to set Opportunity Replenishment flag as true by default 
        _oportunityrelpflag = True
        'PWMS-862 - Ended  to set Opportunity Replenishment flag as true by default 
        _status = pStatus
        _skuclassname = pClassName

        _inventory = pInventory
        If Not pInitialStatus Is Nothing Then _initialstatus = pInitialStatus
        If Not pVelocity Is Nothing Then _velocity = pVelocity
        _fifoindifference = pFifoWindow
        _onsitemin = pOnSiteMin

        _onsitemax = pOnSiteMax
        _lastcyclecount = pLastCycleCount
        _cyclecountint = pCycleCount
        _lowlimitcount = pLowLimitCount

        _preflocation = pPrefLocation
        _prefwarehousearea = pPrefWarehouseArea
        _prefputregion = pPrefPutRegion
        _picksortorder = pPickSortOrder

        _defaultuom = pDefaultUom
        _overpickpct = pOverPickpct
        _unitprice = pUnitPrice
        _overreceivepct = pOverReceivePct

        _dailydemand = pDailyDemand
        _dailypicks = pDailyPicks
        _transportationclass = pTransportationClass
        _hazclass = pHazardClass

        _hutype = pHUType
        _defaultrecuom = pDefaultRecUom
        _storageclass = pStorageClass
        _notes = pNotes
        _baseitem = pBaseItem
        _baseitemqty = pBaseItemQty
        _autoadjustcountqty = pAutoAdjustCountQty

        _edituser = pUser

        _counttolerance = pCountTolerance

        _defaultrecloaduom = pDefaultRecLoadUom
        _overallocmode = pOverAllocMode

        _receivingweightcapturemethod = pReceivingWeightCaptureMethod
        _shippingweightcapturemethod = pShippingWeightCaptureMethod
        _receivingweighttolerance = pReceivingWeightTolerance
        _shippingweighttolerance = pShippingWeightTolerance

        _fullpickallocation = pFullPickAllocation
        _parentSKU = parentSKU

        validate(True)
        _editdate = DateTime.Now
        Dim sql As String = String.Format("UPDATE SKU SET SKUDESC={0},SKUSHORTDESC={1},MANUFACTURERSKU={2}, VENDORSKU={3}, OTHERSKU={4}, SKUGROUP={5},NEWSKU={6}," &
                          "OPORTUNITYRELPFLAG={7},STATUS = {8},CLASSNAME={9},INVENTORY={10},INITIALSTATUS={11},VELOCITY={12},FIFOINDIFFERENCE={13},ONSITEMIN={14}" &
                          ",ONSITEMAX={15},LASTCYCLECOUNT={16},CYCLECOUNTINT={17},LOWLIMITCOUNT={18},PREFLOCATION={19},PREFWAREHOUSEAREA={20},PREFPUTREGION={21}," &
                          "PICKSORTORDER={22},DEFAULTUOM={23},OVERPICKPCT={24},UNITPRICE={25},OVERRECEIVEPCT={26},DAILYDEMAND={27},DAILYPICKS={28},TRANSPORTATIONCLASS={29}," &
                          "HAZCLASS={30},HUTYPE={31},DEFAULTRECUOM={32},STORAGECLASS={33},NOTES={34},BASEITEM={35},COUNTTOLERANCE={36},AUTOADJUSTCOUNTQTY={37}, EDITUSER={38},EDITDATE={39},DEFAULTRECLOADUOM={40},OVERALLOCMODE={41}, BASEITEMQTY={42}, RECEIVINGWEIGHTCAPTUREMETHOD={43}, SHIPPINGWEIGHTCAPTUREMETHOD={44}, RECEIVINGWEIGHTTOLERANCE={45}, SHIPPINGWEIGHTTOLERANCE={46}, FULLPICKALLOCATION={47},ParentSKU= {48} {49}",
                          Made4Net.Shared.Util.FormatField(_skudesc), Made4Net.Shared.Util.FormatField(_skushortdesc), Made4Net.Shared.Util.FormatField(_manufacturersku),
                          Made4Net.Shared.Util.FormatField(_vendorsku), Made4Net.Shared.Util.FormatField(_othersku), Made4Net.Shared.Util.FormatField(_skugroup),
                          Made4Net.Shared.Util.FormatField(_newsku), Made4Net.Shared.Util.FormatField(_oportunityrelpflag), Made4Net.Shared.Util.FormatField(_status),
                          Made4Net.Shared.Util.FormatField(_skuclassname), Made4Net.Shared.Util.FormatField(_inventory), Made4Net.Shared.Util.FormatField(_initialstatus),
                          Made4Net.Shared.Util.FormatField(_velocity), Made4Net.Shared.Util.FormatField(_fifoindifference), Made4Net.Shared.Util.FormatField(_onsitemin),
                          Made4Net.Shared.Util.FormatField(_onsitemax), Made4Net.Shared.Util.FormatField(_lastcyclecount), Made4Net.Shared.Util.FormatField(_cyclecountint),
                          Made4Net.Shared.Util.FormatField(_lowlimitcount), Made4Net.Shared.Util.FormatField(_preflocation), Made4Net.Shared.Util.FormatField(_prefwarehousearea),
                          Made4Net.Shared.Util.FormatField(_prefputregion), Made4Net.Shared.Util.FormatField(_picksortorder), Made4Net.Shared.Util.FormatField(_defaultuom),
                          Made4Net.Shared.Util.FormatField(_overpickpct), Made4Net.Shared.Util.FormatField(_unitprice), Made4Net.Shared.Util.FormatField(_overreceivepct),
                          Made4Net.Shared.Util.FormatField(_dailydemand), Made4Net.Shared.Util.FormatField(_dailypicks), Made4Net.Shared.Util.FormatField(_transportationclass),
                          Made4Net.Shared.Util.FormatField(_hazclass), Made4Net.Shared.Util.FormatField(_hutype), Made4Net.Shared.Util.FormatField(_defaultrecuom),
                          Made4Net.Shared.Util.FormatField(_storageclass), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_baseitem),
                          Made4Net.Shared.Util.FormatField(_counttolerance), Made4Net.Shared.Util.FormatField(_autoadjustcountqty), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate),
                          Made4Net.Shared.Util.FormatField(_defaultrecloaduom), Made4Net.Shared.Util.FormatField(_overallocmode), Made4Net.Shared.Util.FormatField(_baseitemqty),
                          Made4Net.Shared.Util.FormatField(_receivingweightcapturemethod), Made4Net.Shared.Util.FormatField(_shippingweightcapturemethod),
                          Made4Net.Shared.Util.FormatField(_receivingweighttolerance), Made4Net.Shared.Util.FormatField(_shippingweighttolerance), Made4Net.Shared.FormatField(_fullpickallocation), Made4Net.Shared.FormatField(_parentSKU), WhereClause)

        DataInterface.RunSQL(sql)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.SkuUpdated
        em.Add("EVENT", EventType)
        em.Add("CONSIGNEE", _consignee)
        em.Add("SKU", _sku)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub Delete()
        Me.UNITSOFMEASURE.DeleteAllUOM()
        Me.BOM.DeleteAllBOM()
        Me.SUBSTITUTESSKU.DeleteAllSKUsubstitute()
        Me.Attributes.DeleteAllAttributes()

        Dim sql As String = String.Format("delete from sku {0}", WhereClause)
        DataInterface.RunSQL(sql)

    End Sub

    Private Sub validate(ByVal edit As Boolean)
        If edit Then
            If Not WMS.Logic.SKU.Exists(_consignee, _sku) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "SKU Does not Exists", "SKU Does not Exists")
                Throw m4nEx
            End If
            If Not WMS.Logic.SKU.SKUUOM.Exists(_consignee, _sku, _defaultuom) Then
                Throw New M4NException(New Exception, "Default Uom does not exist", "Default Uom does not exist")
            End If
            If _defaultrecuom <> String.Empty And (Not WMS.Logic.SKU.SKUUOM.Exists(_consignee, _sku, _defaultrecuom)) Then
                Throw New M4NException(New Exception, "Default receiving UOM does not exist", "Default receiving UOM does not exist")
            End If

            If _prefwarehousearea <> String.Empty And Not Warehouse.ValidateWareHouseArea(_prefwarehousearea) Then
                Throw New M4NException(New Exception, "Can not insert sku. Warehouse Area does not exist", "Can not insert sku. Warehouse Area does not exist")
            End If
            'parentsku cannot be the same item
            If _parentSKU <> String.Empty And (_sku.CompareTo(_parentSKU) = 0) Then
                Throw New M4NException(New Exception, "SKU and ParentSKU cannot be same", "SKU and ParentSKU cannot be same")
            End If

            If _parentSKU <> String.Empty And IsSKUParent(_sku) Then
                Throw New M4NException(New Exception, "SKU is already parent to other SKU", "SKU is already parent to other SKU")
            End If
            'sku cannot have a parent if sku is parent to some other SKU
            If _parentSKU <> String.Empty And IsParentSKUChildOfAnotherSKU(_parentSKU) Then
                Throw New M4NException(New Exception, "ParentSKU cannot be child of another SKU", "ParentSKU cannot be child of another SKU")
            End If
            'parentsku cannot be child to other parent item
            If _parentSKU <> String.Empty And IsParentSKUChildOfAnotherSKU(_parentSKU) Then
                Throw New M4NException(New Exception, "ParentSKU cannot be child of another SKU", "ParentSKU cannot be child of another SKU")
            End If

        Else
            'If WMS.Logic.SKU.Exists(_consignee, _sku) Then
            Dim sql As String = String.Format("Select count(1) from SKU where consignee = '{0}' and SKU = '{1}'", _consignee, _sku)
            Dim Exist As Boolean = Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
            If Exist Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "SKU already Exists", "SKU already Exists")
                Throw m4nEx
            End If
            If _defaultuom = String.Empty Or _defaultuom = Nothing Then
                Throw New M4NException(New Exception, "Default UOM must be set when setting up a new sku", "Default Uom must be set when setting up a new sku")
            End If

            If Not WMS.Logic.Consignee.Exists(_consignee) Then
                Throw New M4NException(New Exception, "Client does not exist", "Client does not exist")
            End If
        End If

        If _onsitemin > _onsitemax Then
            Throw New M4NException(New Exception, "On site minimum is bigger than the maximum", "On site minimum is bigger than the maximum")
        End If

        If Me.isChildOf(_defaultrecuom, _defaultrecloaduom) Then
            Throw New M4NException(New Exception, "Default Receiving load uom must be greater than default receiving uom.", "Default Receiving load uom must be greater than default receiving uom.")
        End If

        If Me._fullpickallocation < 0 Then
            Throw New M4NException(New Exception(), "Full pick allocation can not be negative", "Full pick allocation can not be negative")
        End If
    End Sub

    Public Sub Edit(ByVal pSkuDesc As String, ByVal pShortSkuDesc As String, ByVal pManufactureSku As String, ByVal pVendorSku As String, _
        ByVal pOtherSku As String, ByVal pSkuGroup As String, ByVal pClassName As String, ByVal pUser As String, Optional ByVal pDefaultUom As String = "", Optional ByVal pDefaultRecUom As String = "")

        If Not WMS.Logic.SKU.Exists(_consignee, _sku) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "SKU Does not Exists", "SKU Does not Exists")
            Throw m4nEx
        End If

        If Not pClassName Is Nothing And Not pClassName = "" Then
            _skuclass = New SkuClass(pClassName)
        End If

        _skudesc = pSkuDesc
        _skushortdesc = pShortSkuDesc
        _manufacturersku = pManufactureSku
        _vendorsku = pVendorSku
        _othersku = pOtherSku
        _skugroup = pSkuGroup
        _status = True
        _defaultuom = pDefaultUom
        _defaultrecuom = pDefaultRecUom
        _adduser = pUser
        _adddate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String = String.Format("Update SKU Set CONSIGNEE={0}, SKU={1}, SKUDESC={2}, SKUSHORTDESC={3}, MANUFACTURERSKU={4}, VENDORSKU={5}, OTHERSKU={6}, SKUGROUP={7}, CLASSNAME={8}, STATUS={9},PICKSORTORDER={10}, EDITUSER={11}, EDITDATE={12}, DEFAULTUOM={13}, DEFAULTRECUOM={14}  {15}", _
                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_skudesc), Made4Net.Shared.Util.FormatField(_skushortdesc), Made4Net.Shared.Util.FormatField(_manufacturersku), Made4Net.Shared.Util.FormatField(_vendorsku), _
                Made4Net.Shared.Util.FormatField(_othersku), Made4Net.Shared.Util.FormatField(_skugroup), Made4Net.Shared.Util.FormatField(pClassName), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_picksortorder), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_defaultuom), Made4Net.Shared.Util.FormatField(_defaultrecuom), WhereClause)

        DataInterface.RunSQL(sql)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.SkuUpdated
        em.Add("EVENT", EventType)
        em.Add("CONSIGNEE", _consignee)
        em.Add("SKU", _sku)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

#Region "UOM"

    Public Sub SetUOM(ByVal oUOM As SKUUOM)
        SetUOM(oUOM.UOM, oUOM.EANUPC, oUOM.GROSSWEIGHT, oUOM.NETWEIGHT, oUOM.LENGTH, oUOM.WIDTH, oUOM.HEIGHT, oUOM.VOLUME, oUOM.LOWERUOM, oUOM.UNITSPERMEASURE, oUOM.SHIPPABLE, oUOM.EDITUSER, oUOM.NESTEDQUANTITY, oUOM.NESTEDWIDTHDIFF, oUOM.NESTEDHEIGHTDIFF, oUOM.NESTEDDEPTHDIFF, oUOM.NESTEDVOLUMEDIFF, _
        oUOM.LABORGRABTYPE, oUOM.LABORHANDLINGTYPE, oUOM.LABORPACKAGETYPE, oUOM.LABORPREPARATIONTYPE)
    End Sub

    Public Sub SetUOM(ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
        ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, ByVal pLaborPreparationType As String)
        _skuunitsofmeasure.SetUOM(pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, pLowerUOM, pUnits, pShippable, pUser, pNestedQuantity, _
                                    pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, pLaborGrabType, _
                                    pLaborHandlingType, pLaborPackageType, pLaborPreparationType)
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.SkuUOMSettled
        Dim em As New EventManagerQ
        em.Add("EVENT", EventType)
        em.Add("CONSIGNEE", _consignee)
        em.Add("SKU", _sku)
        em.Add("UOM", pUOM)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub CreateUOM(ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
        ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, ByVal pLaborPreparationType As String)
        _skuunitsofmeasure.CreateUOM(pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, pLowerUOM, pUnits, pShippable, pUser, pNestedQuantity, pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, pLaborGrabType, _
        pLaborHandlingType, pLaborPackageType, pLaborPreparationType)
        sendToEM(pUOM, pUser)
    End Sub

    Public Sub UpdateUOM(ByVal pUOM As String, ByVal pEANUPC As String, ByVal pGrossWeight As Decimal, ByVal pNetWeight As Decimal, ByVal pLength As Decimal, ByVal pWidth As Decimal, _
        ByVal pHeight As Decimal, ByVal pVolume As Decimal, ByVal pLowerUOM As String, ByVal pUnits As Decimal, ByVal pShippable As Boolean, ByVal pUser As String, ByVal pNestedQuantity As Decimal, _
                ByVal pNestedWidthDiff As Decimal, ByVal pNestedHeightDiff As Decimal, ByVal pNestedDepthDiff As Decimal, ByVal pNestedVolumeDiff As Decimal, _
                ByVal pLaborGrabType As String, ByVal pLaborHandlingType As String, ByVal pLaborPackageType As String, ByVal pLaborPreparationType As String)
        _skuunitsofmeasure.UpdateUOM(pUOM, pEANUPC, pGrossWeight, pNetWeight, pLength, pWidth, pHeight, pVolume, pLowerUOM, pUnits, pShippable, pUser, pNestedQuantity, pNestedWidthDiff, pNestedHeightDiff, pNestedDepthDiff, pNestedVolumeDiff, _
        pLaborGrabType, pLaborHandlingType, pLaborPackageType, pLaborPreparationType)
        sendToEM(pUOM, pUser)

        updateUnitsPerLowestUom(_skuunitsofmeasure.UOM(pUOM), _skuunitsofmeasure, pUser)
    End Sub

    Private Sub updateUnitsPerLowestUom(ByVal pLowerUom As SKUUOM, ByVal pUOMColl As SKUUOMCollection, ByVal pUser As String)
        Dim unitsPerLowestUom As Decimal = pLowerUom.UNITSPERLOWESTUOM
        Dim sql As String = "update SKUUOM set UNITSPERLOWESTUOM = {0}, EDITDATE ={1}, EDITUSER={2} where {3}"
        Dim now As DateTime = DateTime.Now
        For Each upperUOM As SKUUOM In pUOMColl
            If upperUOM.LOWERUOM.Equals(pLowerUom.UOM, StringComparison.OrdinalIgnoreCase) Then
                upperUOM.UNITSPERLOWESTUOM = upperUOM.UNITSPERMEASURE * pLowerUom.UNITSPERLOWESTUOM

                DataInterface.RunSQL(String.Format(sql, Made4Net.Shared.FormatField(upperUOM.UNITSPERLOWESTUOM), _
                Made4Net.Shared.FormatField(now), Made4Net.Shared.FormatField(pUser), upperUOM.WhereClause))

                updateUnitsPerLowestUom(upperUOM, pUOMColl, pUser)
            End If
        Next
    End Sub


    Private Sub sendToEM(ByVal pUOM As String, ByVal pUser As String)
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.SkuUOMSettled
        Dim em As New EventManagerQ
        em.Add("EVENT", EventType)
        em.Add("CONSIGNEE", _consignee)
        em.Add("SKU", _sku)
        em.Add("UOM", pUOM)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Function ConvertToUnits(ByVal FromUOM As String) As Decimal
        Dim tempUOM1, tempUOM2 As SKUUOM
        Dim ConvertedUnits As Decimal = 1
        Dim last As Boolean = False
        tempUOM1 = _skuunitsofmeasure.UOM(FromUOM)
        If tempUOM1 Is Nothing Then
            Return ConvertedUnits
        End If
        While Not last
            tempUOM2 = _skuunitsofmeasure.UOM(tempUOM1.LOWERUOM)
            If tempUOM2 Is Nothing Then
                Return ConvertedUnits
            End If
            If tempUOM2.UOM = FromUOM Then
                Throw New ApplicationException(FromUOM & " cannot be parent of itself - circular definition")
            End If
            ConvertedUnits *= tempUOM1.UNITSPERMEASURE
            tempUOM1 = tempUOM2
        End While
    End Function

    Public Function ConvertUnitsToUom(ByVal FromUOM As String, ByVal Qty As Decimal) As Decimal
        Try
            Dim unitsinuom As Decimal
            unitsinuom = ConvertToUnits(FromUOM)
            Return Qty / unitsinuom 'Convert.ToDecimal(Decimal.Floor(Qty / unitsinuom))
        Catch ex As Exception
            Return Qty
        End Try
    End Function

    Public Function ConvertMyLowestUomUnitsToParentLowestUomUnits(ByVal myLowestUomUnits As Decimal) As Decimal
        Try
            If Me.PARENTSKU = String.Empty Then Return myLowestUomUnits
            Dim oParentSku As SKU = New SKU(Me.CONSIGNEE, Me.PARENTSKU)
            Dim unitsInParentLowestUom As Decimal = ConvertToUnits(oParentSku.LOWESTUOM)
            Return myLowestUomUnits \ unitsInParentLowestUom
        Catch ex As Exception
            Return myLowestUomUnits
        End Try
    End Function
    Public Function ConvertParentLowestUomUnitsToMyLowestUomUnits(ByVal parentLowestUomUnits As Decimal) As Decimal
        Try
            If Me.PARENTSKU = String.Empty Then Return parentLowestUomUnits
            Dim oParentSku As SKU = New SKU(Me.CONSIGNEE, Me.PARENTSKU)
            Dim unitsInParentLowestUom As Decimal = ConvertToUnits(oParentSku.LOWESTUOM)
            Return unitsInParentLowestUom * parentLowestUomUnits
        Catch ex As Exception
            Return parentLowestUomUnits
        End Try
    End Function

    Public Function ConvertParentUnitsToChildLowestUomUnits(ByVal parentLowestUomUnits As Decimal) As Decimal
        Try
            Dim unitsInChildLowestUom As Decimal = ConvertToUnits(Me.LOWESTUOM)
            Return unitsInChildLowestUom * parentLowestUomUnits
        Catch ex As Exception
            Return parentLowestUomUnits
        End Try
    End Function
    Public Function isChildOf(ByVal _parentUom As String, ByVal _childUom As String) As Boolean
        Try
            Dim oUomParnet, oUomChild As SKUUOM
            oUomParnet = _skuunitsofmeasure.UOM(_parentUom)
            oUomChild = _skuunitsofmeasure.UOM(_childUom)
            Return isChildOf(oUomParnet, oUomChild)
        Catch ex As Exception
            Return False
        End Try

    End Function


    Public Function IsParentSKUChildOfAnotherSKU(ByVal parentSKU As String) As Boolean
        Dim SQL As String
        SQL = String.Format("SELECT count(1) FROM SKU WHERE (SKU = '{0}') AND LEN(PARENTSKU) > 0 AND CONSIGNEE ='{1}' ",
        parentSKU, _consignee)
        Return DataInterface.ExecuteScalar(SQL)
    End Function

    Public Function IsSKUParent(ByVal sku As String) As Boolean
        Dim SQL As String
        SQL = String.Format("SELECT count(1) FROM SKU WHERE PARENTSKU = '{0}' AND CONSIGNEE ='{1}' ",
        sku, _consignee)
        Return DataInterface.ExecuteScalar(SQL)
    End Function

    Public Function isChildOf(ByVal _parentUom As SKUUOM, ByVal _childUom As SKUUOM) As Boolean
        Dim tempUOM1, tempUOM2 As SKUUOM
        Dim last As Boolean = False
        tempUOM1 = _parentUom
        If tempUOM1 Is Nothing Or _childUom Is Nothing Then
            Return False
        End If
        While True
            Try
                tempUOM1 = _skuunitsofmeasure.UOM(tempUOM1.LOWERUOM)
            Catch ex As Exception
                Return False
            End Try
            If tempUOM1 Is Nothing Then
                Return False
            End If
            If tempUOM1.UOM = _childUom.UOM Then
                Return True
            End If
        End While
    End Function

    Function getNextUom(ByVal CurrentUom As String) As String
        Dim currUom As SKUUOM = _skuunitsofmeasure.UOM(CurrentUom)
        If currUom Is Nothing Then Return Nothing
        currUom = _skuunitsofmeasure.UOM(currUom.LOWERUOM)
        If currUom Is Nothing Then Return Nothing
        Return currUom.UOM
    End Function

    Function getLowestUom() As String
        Dim SQL As String
        Dim uom As String
        SQL = String.Format("SELECT TOP 1 isnull(UOM,'') FROM SKUUOM WHERE (LOWERUOM = '' OR LOWERUOM IS NULL) AND (SKU = '{0}') AND (CONSIGNEE  = '{1}') ", _
                    _sku, _consignee)
        Return DataInterface.ExecuteScalar(SQL)
    End Function
    'Start PWMS-921 and RWMS-977 To get Highest UOM   
    Function getHighestUom() As String
        Dim SQL As String
        Dim uom As String
        SQL = String.Format("SELECT TOP 1 isnull(UOM,'') FROM SKUUOM WHERE (SKU = '{0}') AND (CONSIGNEE = '{1}') ORDER BY UNITSPERLOWESTUOM DESC ", _
        _sku, _consignee)
        Return DataInterface.ExecuteScalar(SQL)
    End Function
    'End PWMS-921 and  RWMS-977  

    'RWMS-2829
    Public Shared Function GetNetWeight(ByVal sku As String) As Decimal
        Dim sqluom As String = String.Format("select NETWEIGHT from SKUUOM where isnull(loweruom,'')=''  and SKU = '{0}'", sku)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
    End Function

    Public Shared Function GetGrossWeight(ByVal sku As String) As Decimal
        Dim sqluom As String = String.Format("select GROSSWEIGHT from SKUUOM where isnull(loweruom,'')=''  and SKU = '{0}'", sku)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
    End Function
    'RWMS-2829

    Public Sub SaveUomCollection(ByVal pUser As String)
        Me.UNITSOFMEASURE.Save(Me.DEFAULTUOM, pUser)
    End Sub

#End Region

#Region "SubstituteSku"

    Public Function ContainsSubstituteSku(ByVal pSubSku As String)
        If SUBSTITUTESSKU Is Nothing Then Return False
        If SUBSTITUTESSKU.Count = 0 Then Return False
        For Each oSkuSub As SkuSubstitutes In SUBSTITUTESSKU
            If oSkuSub.SUBSTITUTESKU.ToLower = pSubSku.ToLower Then
                Return True
            End If
        Next
        Return False
    End Function

#End Region

#End Region

End Class

#End Region

