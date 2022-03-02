Imports Made4Net.DataAccess

#Region "INVENTORYTRANS"


' <summary>
' This object represents the properties and methods of a INVENTORYTRANS.
' </summary>

<CLSCompliant(False)> Public Class InventoryTransaction

#Region "Variables"

#Region "Primary Keys"

    Protected _invtrans As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _invtrntype As String
    Protected _consignee As String
    Protected _document As String
    Protected _line As Int32
    Protected _sku As String
    Protected _loadid As String
    Protected _uom As String
    Protected _qty As Decimal
    Protected _cube As Decimal
    Protected _weight As Decimal
    Protected _amount As Decimal
    Protected _trandate As DateTime
    '--- Added by Lev 16.08.2006
    ' will hold the status of each transaction
    Protected _status As String
    ' if the status of the load is not AVAILABLE then it should hold the reason why
    Protected _reasoncode As String
    '-----------------------------
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _invattributes As InventoryAttributeBase
    'RWMS-344: Begin
    Protected _hostreferenceid As String
    Protected _notes As String
    'RWMS-344: End

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " INVTRANS = '" & _invtrans & "'"
        End Get
    End Property

    Public Property INVTRANS() As String
        Get
            Return _invtrans
        End Get
        Set(ByVal Value As String)
            _invtrans = Value
        End Set
    End Property

    Public Property INVTRNTYPE() As String
        Get
            Return _invtrntype
        End Get
        Set(ByVal Value As String)
            _invtrntype = Value
        End Set
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property DOCUMENT() As String
        Get
            Return _document
        End Get
        Set(ByVal Value As String)
            _document = Value
        End Set
    End Property

    Public Property LINE() As Int32
        Get
            Return _line
        End Get
        Set(ByVal Value As Int32)
            _line = Value
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

    Public Property LOADID() As String
        Get
            Return _loadid
        End Get
        Set(ByVal Value As String)
            _loadid = Value
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

    Public Property QTY() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal Value As Decimal)
            _qty = Value
        End Set
    End Property

    Public Property CUBE() As Decimal
        Get
            Return _cube
        End Get
        Set(ByVal Value As Decimal)
            _cube = Value
        End Set
    End Property

    Public Property WEIGHT() As Decimal
        Get
            Return _weight
        End Get
        Set(ByVal Value As Decimal)
            _weight = Value
        End Set
    End Property

    Public Property AMOUNT() As Decimal
        Get
            Return _amount
        End Get
        Set(ByVal Value As Decimal)
            _amount = Value
        End Set
    End Property

    Public Property TRANDATE() As DateTime
        Get
            Return _trandate
        End Get
        Set(ByVal Value As DateTime)
            _trandate = Value
        End Set
    End Property

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property REASONCODE() As String
        Get
            Return _reasoncode
        End Get
        Set(ByVal Value As String)
            _reasoncode = Value
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
    'RWMS-344: Begin
    Public Property HOSTREFERENCEID() As String
        Get
            Return _hostreferenceid
        End Get
        Set(ByVal Value As String)
            _hostreferenceid = Value
        End Set

    End Property

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal Value As String)
            _notes = Value
        End Set
    End Property
    'RWMS-344: End
#End Region

#Region "Constructors"

    Public Sub New()
        _invattributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.InventoryTransaction)
    End Sub

#End Region

#Region "Methods"

    Public Shared Sub CreateAttributesRecords(ByRef it As IInventoryTransactionQ, ByVal InvAttCollection As InventoryAttributeBase)
        Dim dt As DataTable = InventoryAttributeBase.GetAttributeTableSchema()

        For Each oAttributeKey As String In InvAttCollection.Attributes.Keys
            Try
                Select Case dt.Select(" COLUMN_NAME = '" & oAttributeKey & "'")(0)("DATA_TYPE")
                    Case "datetime"
                        Dim atDate As DateTime
                        If DateTime.TryParse(InvAttCollection.Attributes(oAttributeKey), atDate) Then
                            it.Add(oAttributeKey, Made4Net.Shared.Util.DateTimeToDbString(InvAttCollection.Attributes(oAttributeKey)))
                        End If
                    Case Else
                        it.Add(oAttributeKey, InvAttCollection.Attributes(oAttributeKey).ToString())
                End Select
            Catch ex As Exception
            End Try
        Next
    End Sub

    Public Sub ExtractAttributesFromQSender(ByVal qSender As Made4Net.Shared.QMsgSender, Optional ByVal Logger As Made4Net.Shared.Logging.LogFile = Nothing)
        Dim sku As New SKU(_consignee, _sku)
        If Not Logger Is Nothing Then
            Logger.WriteLine("Starting to extract Sku Class")
        End If
        If Not sku.SKUClass Is Nothing Then
            For Each loadatt As SkuClassLoadAttribute In sku.SKUClass.LoadAttributes
                Try
                    Select Case loadatt.Type
                        Case AttributeType.Boolean
                            _invattributes.Attributes.Add(loadatt.Name, Convert.ToBoolean(qSender.Values(loadatt.Name)))
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Attribute name : " & loadatt.Name & " Attribute Value : " & qSender.Values(loadatt.Name))
                            End If
                        Case AttributeType.DateTime
                            _invattributes.Attributes.Add(loadatt.Name, Made4Net.Shared.Util.DateTimeToDbString(qSender.Values(loadatt.Name)))
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Attribute name : " & loadatt.Name & " Attribute Value : " & qSender.Values(loadatt.Name))
                            End If
                        Case AttributeType.Decimal
                            _invattributes.Attributes.Add(loadatt.Name, Convert.ToDecimal(qSender.Values(loadatt.Name)))
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Attribute name : " & loadatt.Name & " Attribute Value : " & qSender.Values(loadatt.Name))
                            End If
                        Case AttributeType.Integer
                            _invattributes.Attributes.Add(loadatt.Name, Convert.ToInt32(qSender.Values(loadatt.Name)))
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Attribute name : " & loadatt.Name & " Attribute Value : " & qSender.Values(loadatt.Name))
                            End If
                        Case Else
                            _invattributes.Attributes.Add(loadatt.Name, qSender.Values(loadatt.Name))
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Attribute name : " & loadatt.Name & " Attribute Value : " & qSender.Values(loadatt.Name))
                            End If
                    End Select
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Public Sub Post()
        'RWMS-344 : Begin
        'Notes,HostReferenceID are also included in the INSERT statement
        _invtrans = Made4Net.Shared.Util.getNextCounter("INVENTORYTRANS")
        'Added  for RWMS-1197 and RWMS-1058 Start   
        If _invtrntype = WMS.Lib.INVENTORY.UNRECEIVELOAD Then
            _status = "UNRECEIVED"
        End If
        'Added  for RWMS-1197 and RWMS-1058 End   
        Dim sql As String = String.Format("INSERT INTO INVENTORYTRANS(INVTRANS,INVTRNTYPE,CONSIGNEE,DOCUMENT,LINE," & _
            "SKU,LOADID,UOM,QTY,CUBE,WEIGHT,AMOUNT,TRANDATE,STATUS,REASONCODE,ADDDATE,ADDUSER,EDITDATE,EDITUSER,Notes,HostReferenceID) VALUES (" & _
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20})", Made4Net.Shared.Util.FormatField(_invtrans), _
            Made4Net.Shared.Util.FormatField(_invtrntype), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_document), Made4Net.Shared.Util.FormatField(_line, , True), _
            Made4Net.Shared.Util.FormatField(_sku), Made4Net.Shared.Util.FormatField(_loadid), Made4Net.Shared.Util.FormatField(_uom), Made4Net.Shared.Util.FormatField(_qty), Made4Net.Shared.Util.FormatField(_cube), _
            Made4Net.Shared.Util.FormatField(_weight), Made4Net.Shared.Util.FormatField(_amount), Made4Net.Shared.Util.FormatField(_trandate), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_reasoncode), Made4Net.Shared.Util.FormatField(_adddate), _
            Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_hostreferenceid))
        'RWMS-344 : End
        DataInterface.RunSQL(sql)
        _invattributes.PrimaryKey1 = _invtrans
        _invattributes.Save(_adduser)
        'Added  for RWMS-1197 and RWMS-1058 Start   
        If _invtrntype = WMS.Lib.INVENTORY.UNRECEIVELOAD Then
            Dim sql1 As String = String.Format("UPDATE INVENTORYTRANS SET STATUS = '' WHERE INVTRNTYPE='CREATELOAD' AND STATUS='AVAILABLE' AND LOADID={0}", Made4Net.Shared.Util.FormatField(_loadid))
            DataInterface.RunSQL(sql1)
        End If
        'Added  for RWMS-1197 and RWMS-1058 End   
    End Sub

    Public Sub ExtractAttributes(ByVal nvc As System.Collections.Specialized.NameValueCollection)
        Dim sku As New SKU(_consignee, _sku)
        If Not sku.SKUClass Is Nothing Then
            For Each loadatt As SkuClassLoadAttribute In sku.SKUClass.LoadAttributes
                Try
                    Select Case loadatt.Type
                        Case AttributeType.Boolean
                            _invattributes.Attributes.Add(loadatt.Name, Convert.ToBoolean(nvc(loadatt.Name)))
                        Case AttributeType.DateTime
                            _invattributes.Attributes.Add(loadatt.Name, Made4Net.Shared.Util.WMSStringToDate(nvc(loadatt.Name)))
                        Case AttributeType.Decimal
                            _invattributes.Attributes.Add(loadatt.Name, Convert.ToDecimal(nvc(loadatt.Name)))
                        Case AttributeType.Integer
                            _invattributes.Attributes.Add(loadatt.Name, Convert.ToInt32(nvc(loadatt.Name)))
                        Case Else
                            _invattributes.Attributes.Add(loadatt.Name, nvc(loadatt.Name))
                    End Select
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

#End Region

End Class

#End Region

