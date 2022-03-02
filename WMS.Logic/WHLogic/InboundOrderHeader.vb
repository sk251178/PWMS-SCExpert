Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports Made4Net.DataAccess
Imports Made4Net.Shared

#Region "InboundOrderHeader"

' <summary>
' This object represents the properties and methods of a InboundOrderHeader.
' </summary>

<CLSCompliant(False)> Public Class InboundOrderHeader

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _orderid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _ordertype As String = String.Empty
    Protected _referenceord As String = String.Empty
    Protected _sourcecompany As String = String.Empty
    Protected _companytype As String = String.Empty
    Protected _status As String = String.Empty
    Protected _createdate As DateTime
    Protected _expecteddate As DateTime
    Protected _notes As String = String.Empty
    Protected _hostorderid As String = String.Empty
    Protected _receivedfrom As String = String.Empty
    Protected _scheduledate As DateTime
    Protected _appointmentid As String = String.Empty
    Protected _carrierid As String = String.Empty

    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Collections"

    Protected _lines As InboundOrderDetailCollection
    Protected _handlingunits As HandlingUnitTransactionCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where CONSIGNEE = '" & _consignee & "' And ORDERID = '" & _orderid & "'"
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

    Public Property ORDERID() As String
        Get
            Return _orderid
        End Get
        Set(ByVal Value As String)
            _orderid = Value
        End Set
    End Property

    Public ReadOnly Property Lines() As InboundOrderDetailCollection
        Get
            Return _lines
        End Get
    End Property

    Public ReadOnly Property HandlingUnits() As HandlingUnitTransactionCollection
        Get
            Return _handlingunits
        End Get
    End Property

    Public Property ORDERTYPE() As String
        Get
            Return _ordertype
        End Get
        Set(ByVal Value As String)
            _ordertype = Value
        End Set
    End Property

    Public Property REFERENCEORD() As String
        Get
            Return _referenceord
        End Get
        Set(ByVal Value As String)
            _referenceord = Value
        End Set
    End Property

    Public Property SOURCECOMPANY() As String
        Get
            Return _sourcecompany
        End Get
        Set(ByVal Value As String)
            _sourcecompany = Value
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

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property CREATEDATE() As DateTime
        Get
            Return _createdate
        End Get
        Set(ByVal Value As DateTime)
            _createdate = Value
        End Set
    End Property

    Public Property EXPECTEDDATE() As DateTime
        Get
            Return _expecteddate
        End Get
        Set(ByVal Value As DateTime)
            _expecteddate = Value
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

    Public Property HOSTORDERID() As String
        Get
            Return _hostorderid
        End Get
        Set(ByVal Value As String)
            _hostorderid = Value
        End Set
    End Property

    Public ReadOnly Property RECEIVEDFROM() As Contact
        Get
            Try
                Return New Contact(_receivedfrom, ContactReference.CompanyContact)
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public Property RECEIVEDFROMID() As String
        Get
            Return _receivedfrom
        End Get
        Set(ByVal value As String)
            _receivedfrom = value
        End Set
    End Property

    Public Property CARRIERID() As String
        Get
            Return _carrierid
        End Get
        Set(ByVal value As String)
            _carrierid = value
        End Set
    End Property

    Public Property APPOINTMENTID() As String
        Get
            Return _appointmentid
        End Get
        Set(ByVal value As String)
            _appointmentid = value
        End Set
    End Property

    Public Property SCHEDULEDATE() As DateTime
        Get
            Return _scheduledate
        End Get
        Set(ByVal Value As DateTime)
            _scheduledate = Value
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

    'Public ReadOnly Property CanCancel() As String
    '    Get
    '        Return True
    '    End Get

    'End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Try
            dr = ds.Tables(0).Rows(0)
        Catch ex As Exception

        End Try
        Select Case CommandName.ToLower
            Case "creatercn"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                Dim t As New Translation.Translator()
                Dim rct As String = CreateReceipt(ds, Common.GetCurrentUser)
                Dim col As New Made4Net.DataAccess.Collections.GenericCollection
                col.Add("RECEIPT", rct)
                Message = t.Translate("Receipt Created #RECEIPT#", col)
            Case "updateline"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                Dim oAttCol As AttributesCollection = SkuClass.ExtractLoadAttributes(dr)
                Me.setLine(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFERENCEORDLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYADJUSTED"), "-1"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser, oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("inputqty"), "-1"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("inputuom")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("inputsku")))
            Case "createline"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                'Dim units As Decimal
                'If IsDBNull(dr("skuuom")) Or dr("skuuom") = "" Then
                '    units = dr("QTYORDERED")
                'Else
                '    units = CalcUnits(dr("sku"), dr("QTYORDERED"), dr("skuuom")) * dr("QTYORDERED")
                'End If

                Dim oAttCol As AttributesCollection
                If IsDBNull(dr("SKU")) Then
                    oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                Else
                    oAttCol = SkuClass.ExtractLoadAttributes(dr)
                End If

                Me.setLine(dr("ORDERLINE"), Conversion.Convert.ReplaceDBNull(dr("REFERENCEORDLINE")), Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), _
                    Conversion.Convert.ReplaceDBNull(dr("SKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYORDERED"), "-1"), Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser, oAttCol, Conversion.Convert.ReplaceDBNull(dr("inputqty"), "-1"), Conversion.Convert.ReplaceDBNull(dr("inputuom")), Conversion.Convert.ReplaceDBNull(dr("INPUTSKU")))
            Case "cancelinbound"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                CancelInbound(Common.GetCurrentUser)
            Case "closeinbound"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                CancelInbound(Common.GetCurrentUser)
            Case "updateheader"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                Try
                    'Begin RWMS-520 Author: Ravi
                    'Commented the below code
                    'Dim hostorderid As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'Added the below code
                    HOSTORDERID = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'END RWMS-520
                Catch ex As Exception
                    HOSTORDERID = ""
                End Try
                Me.Update(dr("ORDERTYPE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                     dr("SOURCECOMPANY"), dr("COMPANYTYPE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), _
                     Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NOTES")), HOSTORDERID, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                     Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CARRIERID")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("APPOINTMENTID")), Common.GetCurrentUser)
            Case "createheader"
                Try
                    'Begin RWMS-520 Author: Ravi
                    'Commented the below code
                    'Dim hostorderid As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'Added the below code
                    HOSTORDERID = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'END RWMS-520
                Catch ex As Exception
                    HOSTORDERID = ""
                End Try
                'RWMS-2481 replaced "Nothing" with Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE"))
                Me.Create(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERTYPE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                    dr("SOURCECOMPANY"), dr("COMPANYTYPE"), DateTime.Now, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NOTES")), HOSTORDERID, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RECEIVEDFROM"), ""), "", Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE")), "", Common.GetCurrentUser)
        End Select
    End Sub

    'RWMS-2543 START
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String, ByRef pReceipt As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Try
            dr = ds.Tables(0).Rows(0)
        Catch ex As Exception

        End Try
        Select Case CommandName.ToLower
            Case "creatercn"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                Dim t As New Translation.Translator()
                Dim rct As String = CreateReceipt(ds, Common.GetCurrentUser)
                pReceipt=rct
                Dim col As New Made4Net.DataAccess.Collections.GenericCollection
                col.Add("RECEIPT", rct)
                Message = t.Translate("Receipt Created #RECEIPT#", col)
            Case "updateline"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                Dim oAttCol As AttributesCollection = SkuClass.ExtractLoadAttributes(dr)
                Me.setLine(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFERENCEORDLINE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYADJUSTED"), "-1"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser, oAttCol, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("inputqty"), "-1"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("inputuom")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("inputsku")))
            Case "createline"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                'Dim units As Decimal
                'If IsDBNull(dr("skuuom")) Or dr("skuuom") = "" Then
                '    units = dr("QTYORDERED")
                'Else
                '    units = CalcUnits(dr("sku"), dr("QTYORDERED"), dr("skuuom")) * dr("QTYORDERED")
                'End If

                Dim oAttCol As AttributesCollection
                If IsDBNull(dr("SKU")) Then
                    oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                Else
                    oAttCol = SkuClass.ExtractLoadAttributes(dr)
                End If

                Me.setLine(dr("ORDERLINE"), Conversion.Convert.ReplaceDBNull(dr("REFERENCEORDLINE")), Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), _
                    Conversion.Convert.ReplaceDBNull(dr("SKU")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("QTYORDERED"), "-1"), Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Common.GetCurrentUser, oAttCol, Conversion.Convert.ReplaceDBNull(dr("inputqty"), "-1"), Conversion.Convert.ReplaceDBNull(dr("inputuom")), Conversion.Convert.ReplaceDBNull(dr("INPUTSKU")))
            Case "cancelinbound"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                CancelInbound(Common.GetCurrentUser)
            Case "closeinbound"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                CancelInbound(Common.GetCurrentUser)
            Case "updateheader"
                _consignee = dr("CONSIGNEE")
                _orderid = dr("ORDERID")
                Load()
                Try
                    'Begin RWMS-520 Author: Ravi
                    'Commented the below code
                    'Dim hostorderid As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'Added the below code
                    HOSTORDERID = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'END RWMS-520
                Catch ex As Exception
                    HOSTORDERID = ""
                End Try
                Me.Update(dr("ORDERTYPE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                     dr("SOURCECOMPANY"), dr("COMPANYTYPE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), _
                     Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NOTES")), HOSTORDERID, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RECEIVEDFROM")), _
                     Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CARRIERID")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("APPOINTMENTID")), Common.GetCurrentUser)
            Case "createheader"
                Try
                    'Begin RWMS-520 Author: Ravi
                    'Commented the below code
                    'Dim hostorderid As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'Added the below code
                    HOSTORDERID = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("HOSTORDERID"))
                    'END RWMS-520
                Catch ex As Exception
                    HOSTORDERID = ""
                End Try
                'RWMS-2481 replaced "Nothing" with Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE"))
                Me.Create(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERTYPE"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REFERENCEORD")), _
                    dr("SOURCECOMPANY"), dr("COMPANYTYPE"), DateTime.Now, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EXPECTEDDATE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NOTES")), HOSTORDERID, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("RECEIVEDFROM"), ""), "", Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("SCHEDULEDATE")), "", Common.GetCurrentUser)
        End Select
    End Sub
    'RWMS-2543 END

    Public Function CreateReceipt(ByVal ds As DataSet, ByVal pUser As String) As String
        If ds.Tables(0).Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "There Are No Lines Selected", "There Are No Lines Selected")
            Throw m4nEx
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim col As New Made4Net.DataAccess.Collections.GenericCollection
        Dim Message As String
        Dim sFirstFailedLineNumber As String

        Message = ""
        Dim oReceipt As New ReceiptHeader
        Dim dr As DataRow
        Dim listOfDataRows As New List(Of DataRow)

        For Each dr In ds.Tables(0).Rows
            If dr("QTYADJUSTED") - dr("QTYRECEIVED") > 0 Then
                listOfDataRows.Add(dr)

            ElseIf (col.Count = 0) Then ' finding first "bad" line 
                sFirstFailedLineNumber = dr("ORDERLINE").ToString()
                col.Add("LINE", sFirstFailedLineNumber)

            End If


        Next

        If listOfDataRows.Count = 0 Then
            If dr("QTYADJUSTED") - dr("QTYRECEIVED") = 0 And dr("QTYADJUSTED") > 0 Then
                Message = "NoOpenQtyLeft-line[#param0#]"
            Else
                Message = "Cannot export order lines to receipt, invalid quantity"
            End If

            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, Message, Message)
            m4nEx.Params.Add("line", dr("orderline"))

            Throw m4nEx
        End If


        'If (OrederDetailsReceived(ds)) Then
        '    Message = "some order lines already recieved"
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, Message, Message)
        '    Throw m4nEx
        'Else


        oReceipt.CreateNew("", _scheduledate, Nothing, Nothing, _carrierid, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, pUser)
        '  End If
        For Each dr In ds.Tables(0).Rows


            If (dr("ORDERLINE").ToString() <> sFirstFailedLineNumber) Then
                If _status <> WMS.Lib.Statuses.InboundOrderHeader.CANCELED Then
                    Dim oAttCol As AttributesCollection
                    If IsDBNull(dr("SKU")) Then
                        oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
                    Else
                        oAttCol = SkuClass.ExtractLoadAttributes(dr)
                    End If
                    Dim idl As InboundOrderDetail = New InboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                    If idl.QTYADJUSTED - idl.QTYRECEIVED > 0 Then
                        oReceipt.addLineFromOrder(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), _referenceord, idl.REFERENCEORDLINE, Conversion.Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), _sourcecompany, _companytype, -1, "", pUser, oAttCol)
                    End If
                End If
            Else ' stop inserting lind to receipt and notify the user (throw exception)

                Message = "NoOpenQtyLeft-line[#param0#]" 'message will be translated using the vocabulary table

                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, Message, Message)
                m4nEx.Params.Add("line", dr("orderline"))

                Throw m4nEx
            End If


        Next
        Return oReceipt.RECEIPT
    End Function

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pORDERID As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _orderid = pORDERID
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Sub SetReceivedFrom(ByVal pContactID As String)
        Dim SQL As String = "UPDATE INBOUNDORDHEADER SET RECEIVEDFROM = '" & pContactID & "'" & WhereClause
        DataInterface.RunSQL(SQL)
    End Sub

    Public Shared Function GetInboundOrderHeader(ByVal pCONSIGNEE As String, ByVal pORDERID As String) As InboundOrderHeader
        Return New InboundOrderHeader(pCONSIGNEE, pORDERID)
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM InboundOrdHeader " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Inbound Order Does Not Exists", "Inbound Order Does Not Exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("ORDERID") Then _orderid = dr.Item("ORDERID")
        If Not dr.IsNull("ORDERTYPE") Then _ordertype = dr.Item("ORDERTYPE")
        If Not dr.IsNull("REFERENCEORD") Then _referenceord = dr.Item("REFERENCEORD")
        If Not dr.IsNull("SOURCECOMPANY") Then _sourcecompany = dr.Item("SOURCECOMPANY")
        If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("EXPECTEDDATE") Then _expecteddate = dr.Item("EXPECTEDDATE")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("HOSTORDERID") Then _hostorderid = dr.Item("HOSTORDERID")
        If Not dr.IsNull("RECEIVEDFROM") Then _receivedfrom = dr.Item("RECEIVEDFROM")
        If Not dr.IsNull("APPOINTMENTID") Then _appointmentid = dr.Item("APPOINTMENTID")
        If Not dr.IsNull("CARRIERID") Then _carrierid = dr.Item("CARRIERID")
        If Not dr.IsNull("SCHEDULEDATE") Then _scheduledate = dr.Item("SCHEDULEDATE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _lines = New InboundOrderDetailCollection(_consignee, _orderid)
        _handlingunits = New HandlingUnitTransactionCollection(_consignee, _orderid, HandlingUnitTransactionTypes.InboundOrder)
    End Sub

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pOrderId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from InboundOrdHeader where Consignee = '{0}' and ORDERID = '{1}'", pConsignee, pOrderId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function LineExists(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pLine As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from InboundOrdDetail where Consignee = '{0}' and ORDERID = '{1}' and OrderLine = {2}", pConsignee, pOrderId, pLine)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Function CalcUnits(ByVal pSku As String, ByVal pUnits As Decimal, ByVal pUom As String) As Decimal
        Dim oSku As New WMS.Logic.SKU(_consignee, pSku)
        Return oSku.ConvertToUnits(pUom)
    End Function

#End Region

#Region "Create/Update"

    Public Sub Create(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderType As String, ByVal pRefOrder As String, ByVal pSourceCompany As String, ByVal pCompanyType As String, ByVal pCreateDate As DateTime, _
                        ByVal pExpectedDate As DateTime, ByVal pNotes As String, ByVal pHostOrderId As String, ByVal pReceivedFrom As String, _
                            ByVal pCarrierId As String, ByVal pScheduleDate As DateTime, ByVal pAppointmentId As String, ByVal pUser As String)
        If Exists(pConsignee, pOrderId) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.OrderId already Exist", "Can't create order.OrderId already Exist")
            Throw m4nEx
        End If

        If Not Company.Exists(pConsignee, pSourceCompany, pCompanyType) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order, Source company", "Can't create order, Source company does not exists")
            Throw m4nEx
        Else
            If pReceivedFrom.Length = 0 Then
                _sourcecompany = pSourceCompany
                Dim oComp As New Company(pConsignee, pSourceCompany, pCompanyType)
                _receivedfrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pReceivedFrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _receivedfrom = pReceivedFrom
                _sourcecompany = pSourceCompany
            End If
        End If

        _consignee = pConsignee
        _orderid = pOrderId
        _ordertype = pOrderType
        _referenceord = pRefOrder
        _companytype = pCompanyType
        _createdate = pCreateDate
        _expecteddate = pExpectedDate
        _notes = pNotes
        _hostorderid = pHostOrderId
        _status = WMS.Lib.Statuses.InboundOrderHeader.STATUSNEW
        _carrierid = pCarrierId
        _appointmentid = pAppointmentId
        _scheduledate = pScheduleDate
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("INSERT INTO INBOUNDORDHEADER(CONSIGNEE,ORDERID,ORDERTYPE,REFERENCEORD,SOURCECOMPANY,COMPANYTYPE,STATUS,CREATEDATE,EXPECTEDDATE,NOTES,HOSTORDERID, RECEIVEDFROM, APPOINTMENTID, SCHEDULEDATE, CARRIERID, ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values (" & _
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18})", Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid), Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_sourcecompany), _
            Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_expecteddate), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_hostorderid), _
            Made4Net.Shared.Util.FormatField(_receivedfrom), Made4Net.Shared.Util.FormatField(_appointmentid), Made4Net.Shared.Util.FormatField(_scheduledate), Made4Net.Shared.Util.FormatField(_carrierid), _
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)
        Load()

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDHINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.INBOUNDHINS)
    End Sub

    Public Sub Update(ByVal pOrderType As String, ByVal pRefOrder As String, ByVal pSourceCompany As String, ByVal pCompanyType As String, ByVal pExpectedDate As DateTime, ByVal pNotes As String, ByVal pHostOrderId As String, ByVal pReceivedFrom As String, ByVal pCarrierId As String, ByVal pScheduleDate As DateTime, ByVal pAppointmentId As String, ByVal pUser As String)
        If Not Exists(_consignee, _orderid) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot update order, Order does not exists", "Can't update order, Order does not exists")
            Throw m4nEx
        End If

        If pReceivedFrom Is Nothing Then pReceivedFrom = ""

        If Not Company.Exists(_consignee, pSourceCompany, pCompanyType) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order, Source company", "Can't create order, Source company does not exists")
            Throw m4nEx
        Else
            If pReceivedFrom.Length = 0 Then
                _sourcecompany = pSourceCompany
                Dim oComp As New Company(_consignee, pSourceCompany, pCompanyType)
                _receivedfrom = oComp.DEFAULTCONTACT
            Else
                If Not WMS.Logic.Contact.Exists(pReceivedFrom) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot create order.Invalid contact", "Can't create order.Invalid contact")
                    Throw m4nEx
                End If
                _receivedfrom = pReceivedFrom
                _sourcecompany = pSourceCompany
            End If
        End If

        _ordertype = pOrderType
        _referenceord = pRefOrder
        _companytype = pCompanyType
        _expecteddate = pExpectedDate
        _carrierid = pCarrierId
        _appointmentid = pAppointmentId
        _scheduledate = pScheduleDate
        _notes = pNotes
        _hostorderid = pHostOrderId
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("UPDATE INBOUNDORDHEADER SET ORDERTYPE={0},REFERENCEORD={1},SOURCECOMPANY={2},COMPANYTYPE={3},NOTES={4},EXPECTEDDATE={5},HOSTORDERID={6},RECEIVEDFROM={7}, APPOINTMENTID={8},SCHEDULEDATE={9},CARRIERID={10} ,EDITDATE={11},EDITUSER={12} {13} ", _
            Made4Net.Shared.Util.FormatField(_ordertype), Made4Net.Shared.Util.FormatField(_referenceord), Made4Net.Shared.Util.FormatField(_sourcecompany), _
            Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_expecteddate), _
            Made4Net.Shared.Util.FormatField(_hostorderid), Made4Net.Shared.Util.FormatField(_receivedfrom), _
            Made4Net.Shared.Util.FormatField(_appointmentid), Made4Net.Shared.Util.FormatField(_scheduledate), Made4Net.Shared.Util.FormatField(_carrierid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

        DataInterface.RunSQL(sql)
        Load()
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.InboundUpdated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.INBOUNDHUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.INBOUNDHUPD)
    End Sub

    Public Sub Save()
        If Exists(_consignee, _orderid) Then
            Update(_ordertype, _referenceord, _sourcecompany, _companytype, _expecteddate, _notes, _hostorderid, _receivedfrom, _carrierid, _scheduledate, _appointmentid, _edituser)
        Else
            Create(_consignee, _orderid, _ordertype, _referenceord, _sourcecompany, _companytype, _createdate, _expecteddate, _notes, _hostorderid, _receivedfrom, _carrierid, _scheduledate, _appointmentid, _adduser)
        End If
    End Sub

    Public Sub Delete()
        Dim sql As String
        sql = String.Format("Delete from inboundordheader {0}", WhereClause)
        DataInterface.RunSQL(sql)

        sql = String.Format("Delete from inboundorddetail {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Create Line"

    Public Sub setLine(ByVal pLineNumber As Int32, ByVal pRefOrdLine As String, ByVal pExpDate As DateTime, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing)
        _lines.setLine(pLineNumber, pRefOrdLine, pExpDate, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
    End Sub

    Public Sub setLine(ByVal pRefOrdLine As String, ByVal pExpDate As DateTime, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, ByVal pInputQTY As Decimal, ByVal pInpupUOM As String, Optional ByVal pInputSku As String = Nothing)
        Dim LineNumber As Int32 = getNextLine()
        _lines.setLine(LineNumber, pRefOrdLine, pExpDate, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
    End Sub

    Public Sub setLine(ByVal oLine As InboundOrderDetail)
        Me.setLine(oLine.ORDERLINE, oLine.REFERENCEORDLINE, oLine.EXPECTEDDATE, oLine.SKU, oLine.QTYORDERED, oLine.INVENTORYSTATUS, oLine.ADDUSER, oLine.Attributes.Attributes, oLine.INPUTQTY, oLine.INPUTUOM, oLine.INPUTSKU)
    End Sub

    Private Function getNextLine() As Int32
        Return DataInterface.ExecuteScalar("Select isnull(max(orderline),0) + 1 from INBOUNDORDDETAIL where orderid = '" & _orderid & "'")
    End Function

#End Region

#Region "Receive"

    Public Sub Receive(ByVal pLine As Int32, ByVal pQuantity As Decimal, ByVal pUser As String)
        Dim tLine As InboundOrderDetail = _lines.Line(pLine)
        If IsNothing(tLine) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Inbound Order Line Does Not Exists", "Inbound Order Line Does Not Exists")
            Throw m4nEx
        End If
        If (tLine.QTYADJUSTED = 0) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "InboundOrder status is incorrect for receipt assignment", "Inbound Order Line Does Not Exists")
            Throw m4nEx
        End If

        tLine.Receive(pQuantity, pUser)
        _editdate = DateTime.Now
        _edituser = pUser
        If _status = WMS.Lib.Statuses.InboundOrderHeader.STATUSNEW Then
            _status = WMS.Lib.Statuses.InboundOrderHeader.RECEIVING
        End If
        Dim sql As String = String.Format("update inboundordheader set status = '{0}',editdate = {1},edituser = {2} {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub CancelReceive(ByVal pLine As Int32, ByVal pQuantity As Decimal, ByVal pUser As String)
        Dim tLine As InboundOrderDetail = _lines.Line(pLine)
        If IsNothing(tLine) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Inbound Order Line Does Not Exists", "Inbound Order Line Does Not Exists")
            Throw m4nEx
        End If
        If (tLine.QTYADJUSTED = 0) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "InboundOrder status is incorrect for receipt assignment", "Inbound Order Line Does Not Exists")
            Throw m4nEx
        End If

        tLine.CancelReceive(pQuantity, pUser)
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("update inboundordheader set status = '{0}',editdate = {1},edituser = {2} {3}", _status, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

    'Added by priel
    Public Sub setStatus(ByVal pStatus As String, ByVal puser As String)
        Dim sql As String
        Dim oldstat As String = _status
        _status = pStatus
        _editdate = DateTime.Now
        _edituser = puser
        sql = String.Format("update INBOUNDORDHEADER set STATUS = {0},EDITDATE = {1},EDITUSER = {2}  {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OrderStatusChanged
        'em.Add("EVENT", EventType)
        'em.Add("ORDERID", _orderid)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("FROMSTATUS", oldstat)
        'em.Add("TOSTATUS", _status)
        'em.Add("USERID", puser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.OrderStatusChanged)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.ORDSTUPD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _orderid)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldstat)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.ORDSTUPD)
    End Sub

#Region "Cancel"
    'Added by priel - cancel curren outboundorder
    Public Sub CancelInbound(ByVal puser As String)
        Dim sql As String
        Dim inordetail As InboundOrderDetail

        Select Case _status
            Case WMS.Lib.Statuses.InboundOrderHeader.STATUSNEW

                For Each inordetail In Lines
                    inordetail.QTYADJUSTED = 0  ' ???????????
                    inordetail.UpdateLine(inordetail.REFERENCEORDLINE, inordetail.EXPECTEDDATE, inordetail.SKU, _
                                            inordetail.QTYADJUSTED, inordetail.INVENTORYSTATUS, puser, Nothing)

                Next


                setStatus(WMS.Lib.Statuses.InboundOrderHeader.CANCELED, puser)

            Case Else
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "InboundOrder status is incorrect for canceling", "InboundOrder status is incorrect for canceling")
                Throw m4nEx
        End Select

        'If CanCancel Then
        '    Dim sql As String
        '    Dim inordetail As InboundOrderDetail
        '    For Each inordetail In Lines
        '        '  inordetail.QTYMODIFIED = 0 ' ???????????
        '    Next
        '    _editdate = DateTime.Now
        '    _edituser = puser
        '    '      _status = WMS.Lib.Statuses.InboundOrderHeader.CANCELED ' No Such Status ????


        '    Dim dt As New DataTable
        '    Dim dr As DataRow
        '    Dim shpld As Load

        'sql = String.Format("Update OUTBOUNDORHEADER SET SHIPPEDDATE = {0},STATUS = {1},STATUSDATE = {2},EDITDATE = {3},EDITUSER = {4} WHERE {5}", Made4Net.Shared.Util.FormatField(_shippeddate), _
        '                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_statusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        'DataInterface.RunSQL(sql)

        'send message ?????
        'Try
        '    Dim em As New EventManagerQ
        '    Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OrderShipped
        '    em.Add("EVENT", EventType)
        '    em.Add("ORDERID", _orderid)
        '    em.Add("CONSIGNEE", _consignee)
        '    em.Add("USERID", puser)
        '    em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        '    em.Send(WMSEvents.EventDescription(EventType))
        'Catch ex As Exception
        'End Try
        '------------------------------------------------------------
        'Dim aq As New EventManagerQ
        'Dim action As String = WMS.Lib.Actions.Audit.
        'aq.Add("ACTION", action)
        'aq.Add("USERID", puser)
        'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("CONSIGNEE", _consignee)
        'aq.Add("DOCUMENT", _orderid)
        'aq.Add("FROMSTATUS", _status)
        'aq.Add("TOSTATUS", WMS.Lib.Statuses.OutboundOrderHeader.SHIPPED)
        'aq.Add("FROMLOC", _staginglane)
        'aq.Send(action)

        'Else
        '    Throw New ApplicationException(String.Format("Order {0} status is incorrect, can't cancel order", _orderid))
        'End If
    End Sub

#End Region

#Region "Close"

    Public Sub CloseInbound(ByVal puser As String)
        Dim sql As String
        Dim inordetail As InboundOrderDetail
        For Each inordetail In Lines
            inordetail.QTYADJUSTED = inordetail.QTYRECEIVED
            inordetail.UpdateLine(inordetail.REFERENCEORDLINE, inordetail.EXPECTEDDATE, inordetail.SKU, inordetail.QTYADJUSTED, inordetail.INVENTORYSTATUS, puser, inordetail.Attributes.Attributes)
        Next
        setStatus(WMS.Lib.Statuses.InboundOrderHeader.CLOSED, puser)
    End Sub

#End Region

#Region "Handling Units"

    Public Sub AddHandlingUnit(ByVal pHandlingUnitType As String, ByVal pQty As Decimal, ByVal pUser As String)
        'If _status = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Order Status", "Incorrect Order Status")
        'End If
        validate(pQty)
        Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction
        oHUTrans.Create(Nothing, WMS.Logic.HandlingUnitTransactionTypes.InboundOrder, Nothing, _consignee, _
            _orderid, _ordertype, _sourcecompany, _companytype, pHandlingUnitType, pQty, pUser)
        _handlingunits.Add(oHUTrans)
    End Sub

    Public Sub UpdateHandlingUnit(ByVal pHUTransId As String, ByVal pQty As Decimal, ByVal pUser As String)
        'If _status = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Order Status", "Incorrect Order Status")
        'End If
        validate(pQty)
        Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction(pHUTransId)
        oHUTrans.UpdateHUQty(pQty, pUser)
        _handlingunits.Remove(_handlingunits.HandlingUnitTransaction(pHUTransId))
        _handlingunits.Add(oHUTrans)
    End Sub

    Private Sub validate(ByVal pQty As Decimal)
        If _status = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Or _status = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Order Status", "Incorrect Order Status")
        End If
        If pQty <= 0 Then
            Throw New M4NException(New Exception, "Quantity must be positive.", "Quantity must be positive.")
        End If
    End Sub

#End Region

#End Region

End Class

#End Region

