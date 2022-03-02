Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared.Evaluation

#Region "ReceiptHeader"

<CLSCompliant(False)> Public Class ReceiptHeader

#Region "Variables"

#Region "Primary Keys"

    Protected _receipt As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _status As String = String.Empty
    Protected _scheduleddate As DateTime
    Protected _bol As String = String.Empty
    Protected _notes As String = String.Empty
    Protected _carriercompany As String = String.Empty
    Protected _startreceiptdate As DateTime
    Protected _vehicle As String = String.Empty
    Protected _trailer As String = String.Empty
    Protected _seal1 As String = String.Empty
    Protected _seal2 As String = String.Empty
    Protected _driver1 As String = String.Empty
    Protected _driver2 As String = String.Empty
    Protected _closereceiptdate As DateTime

    Protected _transportreference As String = String.Empty
    Protected _transporttype As String = String.Empty
    Protected _door As String = String.Empty
    Protected _confirmed As Boolean
    Protected _yardentryid As String = String.Empty

    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _StdTimeCalcParameters As Hashtable = New Hashtable()
    Protected _sourceStdTimeEquation As String = String.Empty
    Protected _labelprinted As Boolean

#End Region

#Region "Collections"

    Protected _lines As ReceiptDetailCollection
    Protected _handlingunits As HandlingUnitTransactionCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " RECEIPT = '" & _receipt & "'"
        End Get
    End Property

    Public Property RECEIPT() As String
        Set(ByVal Value As String)
            _receipt = Value
        End Set
        Get
            Return _receipt
        End Get
    End Property

    Public ReadOnly Property LINES() As ReceiptDetailCollection
        Get
            Return _lines
        End Get
    End Property

    Public ReadOnly Property HandlingUnits() As HandlingUnitTransactionCollection
        Get
            Return _handlingunits
        End Get
    End Property

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property SCHEDULEDDATE() As DateTime
        Get
            Return _scheduleddate
        End Get
        Set(ByVal Value As DateTime)
            _scheduleddate = Value
        End Set
    End Property

    Public Property BOL() As String
        Get
            Return _bol
        End Get
        Set(ByVal Value As String)
            _bol = Value
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

    Public Property CARRIERCOMPANY() As String
        Get
            Return _carriercompany
        End Get
        Set(ByVal Value As String)
            _carriercompany = Value
        End Set
    End Property

    Public Property STARTRECEIPTDATE() As DateTime
        Get
            Return _startreceiptdate
        End Get
        Set(ByVal Value As DateTime)
            _startreceiptdate = Value
        End Set
    End Property

    Public Property CLOSERECEIPTDATE() As DateTime
        Get
            Return _closereceiptdate
        End Get
        Set(ByVal Value As DateTime)
            _closereceiptdate = Value
        End Set
    End Property

    Public Property VEHICLE() As String
        Get
            Return _vehicle
        End Get
        Set(ByVal Value As String)
            _vehicle = Value
        End Set
    End Property

    Public Property TRAILER() As String
        Get
            Return _trailer
        End Get
        Set(ByVal Value As String)
            _trailer = Value
        End Set
    End Property

    Public Property SEAL1() As String
        Get
            Return _seal1
        End Get
        Set(ByVal Value As String)
            _seal1 = Value
        End Set
    End Property

    Public Property SEAL2() As String
        Get
            Return _seal2
        End Get
        Set(ByVal Value As String)
            _seal2 = Value
        End Set
    End Property

    Public Property DRIVER1() As String
        Get
            Return _driver1
        End Get
        Set(ByVal Value As String)
            _driver1 = Value
        End Set
    End Property

    Public Property DRIVER2() As String
        Get
            Return _driver2
        End Get
        Set(ByVal Value As String)
            _driver2 = Value
        End Set
    End Property

    Public Property TRANSPORTREFERENCE() As String
        Get
            Return _transportreference
        End Get
        Set(ByVal Value As String)
            _transportreference = Value
        End Set
    End Property

    Public Property TRANSPORTTYPE() As String
        Get
            Return _transporttype
        End Get
        Set(ByVal Value As String)
            _transporttype = Value
        End Set
    End Property

    Public Property DOOR() As String
        Get
            Return _door
        End Get
        Set(ByVal Value As String)
            _door = Value
        End Set
    End Property

    Public Property YARDENTRYID() As String
        Get
            Return _yardentryid
        End Get
        Set(ByVal Value As String)
            _yardentryid = Value
        End Set
    End Property

    Public Property LABELPRINTED() As Boolean
        Get
            Return _labelprinted
        End Get
        Set(ByVal Value As Boolean)
            _labelprinted = Value
        End Set
    End Property

    Public Property CONFIRMED() As Boolean
        Get
            Return _confirmed
        End Get
        Set(ByVal Value As Boolean)
            _confirmed = Value
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

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "PrintWS" Then
            For Each dr In ds.Tables(0).Rows
                ReceiptHeader.PrintReceivingDoc(dr("Receipt"), Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser, False)
            Next
        ElseIf CommandName = "PrintMN" Then
            For Each dr In ds.Tables(0).Rows
                ReceiptHeader.PrintReceivingDoc(dr("Receipt"), Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser, True)
            Next
        ElseIf CommandName = "Save" Then
            dr = ds.Tables(0).Rows(0)
            If dr("RECEIPT") = "" Then
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                Dim rcnnum As String = CreateNew("", Convert.ReplaceDBNull(dr("SCHEDULEDDATE")), Convert.ReplaceDBNull(dr("BOL")), Convert.ReplaceDBNull(dr("NOTES")), Convert.ReplaceDBNull(dr("CARRIERCOMPANY")), Convert.ReplaceDBNull(dr("VEHICLE")), Convert.ReplaceDBNull(dr("TRAILER")), Convert.ReplaceDBNull(dr("DRIVER1")), Convert.ReplaceDBNull(dr("DRIVER2")), Convert.ReplaceDBNull(dr("SEAL1")), Convert.ReplaceDBNull(dr("SEAL2")), Convert.ReplaceDBNull(dr("TRANSPORTREFERENCE")), Convert.ReplaceDBNull(dr("TRANSPORTTYPE")), Convert.ReplaceDBNull(dr("DOOR")), Convert.ReplaceDBNull(dr("LABELPRINTED")), Common.GetCurrentUser)
                Dim col As New Made4Net.DataAccess.Collections.GenericCollection

                col.Add("RECEIPT", rcnnum.TrimEnd(","))
                Message = t.Translate("Receipts Created [#RECEIPT#]", col)
            Else
                _receipt = ds.Tables(0).Rows(0)("RECEIPT")
                Load()
                Update(Convert.ReplaceDBNull(dr("SCHEDULEDDATE")), Convert.ReplaceDBNull(dr("BOL")), Convert.ReplaceDBNull(dr("NOTES")), Convert.ReplaceDBNull(dr("CARRIERCOMPANY")), Convert.ReplaceDBNull(dr("VEHICLE")), Convert.ReplaceDBNull(dr("TRAILER")), Convert.ReplaceDBNull(dr("DRIVER1")), Convert.ReplaceDBNull(dr("DRIVER2")), Convert.ReplaceDBNull(dr("SEAL1")), Convert.ReplaceDBNull(dr("SEAL2")), Convert.ReplaceDBNull(dr("TRANSPORTREFERENCE")), Convert.ReplaceDBNull(dr("TRANSPORTTYPE")), Convert.ReplaceDBNull(dr("DOOR")), Convert.ReplaceDBNull(dr("LABELPRINTED")), Common.GetCurrentUser)
            End If
        ElseIf CommandName = "saveLine" Then
            'System.Web.HttpContext.Current.Response.Write(XMLData)
            'System.Web.HttpContext.Current.Response.Write(XMLSchema)
            dr = ds.Tables(0).Rows(0)
            _receipt = dr("RECEIPT")
            Load()
            Dim units As Decimal
            If IsDBNull(dr("skuuom")) Then
                units = dr("QTYEXPECTED")
            Else
                units = CalcUnits(dr("CONSIGNEE"), dr("sku"), dr("QTYEXPECTED"), dr("skuuom")) * dr("QTYEXPECTED")
            End If

            Dim oAttCol As AttributesCollection
            If IsDBNull(dr("SKU")) Or dr("SKU") = "" Then
                oAttCol = SkuClass.ExtractLoadAttributes(dr, True)
            Else
                oAttCol = SkuClass.ExtractLoadAttributes(dr)
            End If

            If dr("RECEIPTLINE") Is DBNull.Value Or dr("RECEIPTLINE") = 0 Then
                addLine(Convert.ReplaceDBNull(dr("CONSIGNEE")), Convert.ReplaceDBNull(dr("SKU")), units, Convert.ReplaceDBNull(dr("REFORD")), Convert.ReplaceDBNull(dr("REFORDLINE")), Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Convert.ReplaceDBNull(dr("COMPANY")), Convert.ReplaceDBNull(dr("COMPANYTYPE")), Convert.ReplaceDBNull(dr("AVGWEIGHT")), Convert.ReplaceDBNull(dr("AVGWEIGHTUOM")), Convert.ReplaceDBNull(dr("RECEIVEDWEIGHT")), Common.GetCurrentUser, oAttCol, Convert.ReplaceDBNull(dr("INPUTQTY")), Convert.ReplaceDBNull(dr("INPUTUOM")), Convert.ReplaceDBNull(dr("INPUTSKU")))
            Else
                UpdateLine(Convert.ReplaceDBNull(dr("RECEIPTLINE")), Convert.ReplaceDBNull(dr("CONSIGNEE")), Convert.ReplaceDBNull(dr("SKU")), Convert.ReplaceDBNull(dr("QTYEXPECTED")), Convert.ReplaceDBNull(dr("REFORD")), Convert.ReplaceDBNull(dr("REFORDLINE")), Convert.ReplaceDBNull(dr("INVENTORYSTATUS")), Convert.ReplaceDBNull(dr("COMPANY")), Convert.ReplaceDBNull(dr("COMPANYTYPE")), Convert.ReplaceDBNull(dr("AVGWEIGHT")), Convert.ReplaceDBNull(dr("AVGWEIGHTUOM")), Convert.ReplaceDBNull(dr("RECEIVEDWEIGHT")), Common.GetCurrentUser, oAttCol)
            End If
        ElseIf CommandName = "Close" Then
            For Each dr In ds.Tables(0).Rows
                _receipt = dr("RECEIPT")
                Load()
                close(Common.GetCurrentUser())
            Next
        ElseIf CommandName = "ReceiveFull" Then
            Dim errorLines As Boolean = False
            For Each dr In ds.Tables(0).Rows
                _receipt = dr("RECEIPT")
                Load()
                If ReceiveFullReceipt(Common.GetCurrentUser(), False, WMS.Lib.Statuses.LoadStatus.AVAILABLE) <> "" Then
                    errorLines = True
                End If
            Next
            If errorLines Then
                Throw New Made4Net.Shared.M4NException(New Exception(), String.Format("One or more lines were not received."), String.Format("One or more lines were not received."))
            Else
                Message = "Receipts were successfully receieved."
            End If
        End If
    End Sub

    Public Sub New(ByVal pRECEIPT As String, Optional ByVal LoadObj As Boolean = True)
        _receipt = pRECEIPT
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetReceipt(ByVal pRECEIPT As String) As ReceiptHeader
        Return New ReceiptHeader(pRECEIPT)
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM RECEIPTHEADER WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Receipt Does Not Exists", "Receipt Does Not Exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("SCHEDULEDDATE") Then _scheduleddate = dr.Item("SCHEDULEDDATE")
        If Not dr.IsNull("BOL") Then _bol = dr.Item("BOL")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("CARRIERCOMPANY") Then _carriercompany = dr.Item("CARRIERCOMPANY")
        If Not dr.IsNull("STARTRECEIPTDATE") Then _startreceiptdate = dr.Item("STARTRECEIPTDATE")
        If Not dr.IsNull("CLOSERECEIPTDATE") Then _closereceiptdate = dr.Item("CLOSERECEIPTDATE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("VEHICLE") Then _vehicle = dr.Item("VEHICLE")
        If Not dr.IsNull("TRAILER") Then _trailer = dr.Item("TRAILER")
        If Not dr.IsNull("DRIVER1") Then _driver1 = dr.Item("DRIVER1")
        If Not dr.IsNull("DRIVER2") Then _driver2 = dr.Item("DRIVER2")
        If Not dr.IsNull("SEAL1") Then _seal1 = dr.Item("SEAL1")
        If Not dr.IsNull("SEAL2") Then _seal2 = dr.Item("SEAL2")
        If Not IsDBNull(dr("TRANSPORTREFERENCE")) Then _transportreference = dr("TRANSPORTREFERENCE")
        If Not IsDBNull(dr("TRANSPORTTYPE")) Then _transporttype = dr("TRANSPORTTYPE")
        If Not IsDBNull(dr("DOOR")) Then _door = dr("DOOR")
        If Not IsDBNull(dr("YARDENTRYID")) Then _yardentryid = dr("YARDENTRYID")
        If Not IsDBNull(dr("LABELPRINTED")) Then _labelprinted = dr("LABELPRINTED")
        If Not IsDBNull(dr("CONFIRMED")) Then _confirmed = dr("CONFIRMED")

        _lines = New ReceiptDetailCollection(_receipt)
        _handlingunits = New HandlingUnitTransactionCollection(_receipt, HandlingUnitTransactionTypes.Receipt)
    End Sub

    Public Shared Function Exists(ByVal pRcnId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from ReceiptHeader where Receipt = '{0}'", pRcnId)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function LineExists(ByVal pRcnId As String, ByVal pLine As Int32)
        Dim sql As String = String.Format("Select count(1) from ReceiptDetail where Receipt = '{0}' and ReceiptLine = {1}", pRcnId, pLine)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    'Public Shared Function OrderLineExists(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pLine As Int32)
    '    Dim sql As String = String.Format("Select count(1) from ReceiptDetail where Receipt = '{0}' and ReceiptLine = {1}", pRcnId, pLine)
    '    Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    'End Function

    Private Sub setTimeStamp(ByVal pUser As String)
        _edituser = pUser
        _editdate = DateTime.Now
        Dim sql As String = String.Format("Update receiptHeader set EditDate = {0},EditUser = {1} Where {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

    Private Function SKUUnitPrice(ByVal pSku As String, ByVal pConsignee As String) As Decimal
        Try
            Dim sk As WMS.Logic.SKU
            sk = New WMS.Logic.SKU(pConsignee, pSku, True)
            Return sk.UNITPRICE
        Catch ex As Exception
            ex.ToString()
            Return 0
        End Try
    End Function

    Private Function CalcUnits(ByVal pConsignee As String, ByVal pSku As String, ByVal pUnits As Decimal, ByVal pUom As String) As Decimal
        Dim oSku As New WMS.Logic.SKU(pConsignee, pSku)
        Return oSku.ConvertToUnits(pUom)
    End Function

#End Region

#Region "Create New"

    Public Function CreateNew(ByVal pReceiptId As String, ByVal pSchedDate As DateTime, ByVal pBOL As String, ByVal pNotes As String, ByVal pCarrier As String, ByVal pVEHICLE As String, ByVal pTrailer As String, _
            ByVal pDriver1 As String, ByVal pDriver2 As String, ByVal pSeal1 As String, ByVal pSeal2 As String, ByVal pTransportReference As String, ByVal pTransportType As String, ByVal pDoor As String, ByVal pLabelPrinted As Boolean, ByVal pUser As String) As String

        If WMS.Logic.ReceiptHeader.Exists(pReceiptId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Receipt,Receipt Already Exists", "Cannot Create Receipt,Receipt Already Exists")
        End If

        Dim SQL As String = ""

        _status = WMS.Lib.Statuses.Receipt.STATUSNEW
        _scheduleddate = pSchedDate
        _bol = pBOL
        _vehicle = pVEHICLE
        _trailer = pTrailer
        _driver1 = pDriver1
        _driver2 = pDriver2
        _notes = pNotes
        _seal1 = pSeal1
        _seal2 = pSeal2
        _carriercompany = pCarrier
        _transportreference = pTransportReference
        _transporttype = pTransportType
        _door = pDoor

        _labelprinted = pLabelPrinted
        _adduser = pUser
        _edituser = pUser
        _adddate = DateTime.Now
        _editdate = DateTime.Now
        _startreceiptdate = Nothing

        If pReceiptId <> "" Then
            _receipt = pReceiptId
        Else
            _receipt = Made4Net.Shared.Util.getNextCounter("RCN")
        End If


        SQL = "INSERT INTO RECEIPTHEADER(RECEIPT,STATUS,TRAILER, VEHICLE, DRIVER1, DRIVER2, SEAL1, SEAL2," & _
            "SCHEDULEDDATE,BOL,NOTES,CARRIERCOMPANY,STARTRECEIPTDATE,TRANSPORTREFERENCE,TRANSPORTTYPE,DOOR,ADDDATE,ADDUSER,EDITDATE,EDITUSER,LABELPRINTED) Values ("
        SQL += Made4Net.Shared.Util.FormatField(_receipt) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & _
            Made4Net.Shared.Util.FormatField(_trailer) & "," & Made4Net.Shared.Util.FormatField(_vehicle) & "," & _
            Made4Net.Shared.Util.FormatField(_driver1) & "," & Made4Net.Shared.Util.FormatField(_driver2) & "," & _
            Made4Net.Shared.Util.FormatField(_seal1) & "," & Made4Net.Shared.Util.FormatField(_seal2) & "," & _
            Made4Net.Shared.Util.FormatField(_scheduleddate) & "," & Made4Net.Shared.Util.FormatField(_bol) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & _
            Made4Net.Shared.Util.FormatField(_carriercompany) & "," & Made4Net.Shared.Util.FormatField(_startreceiptdate) & "," & _
            Made4Net.Shared.Util.FormatField(_transportreference) & "," & Made4Net.Shared.Util.FormatField(_transporttype) & "," & Made4Net.Shared.Util.FormatField(_door) & "," & _
            Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & _
            Made4Net.Shared.Util.FormatField(_edituser) & "," & Made4Net.Shared.Util.FormatField(_labelprinted) & ")"
        DataInterface.RunSQL(SQL)
        _lines = New ReceiptDetailCollection(_receipt)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptCreated
        em.Add("EVENT", EventType)
        em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTHINS)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ACTIVITYTIME", "0")
        em.Add("CONSIGNEE", "")
        em.Add("DOCUMENT", _receipt)
        em.Add("DOCUMENTLINE", 0)
        em.Add("FROMLOAD", "")
        em.Add("FROMLOC", "")
        em.Add("FROMQTY", 0)
        em.Add("FROMSTATUS", _status)
        em.Add("NOTES", _notes)
        em.Add("SKU", "")
        em.Add("TOLOAD", "")
        em.Add("TOLOC", "")
        em.Add("TOQTY", 0)
        em.Add("TOSTATUS", _status)
        em.Add("USERID", pUser)
        em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ADDUSER", pUser)
        em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("EDITUSER", pUser)
        em.Send(WMSEvents.EventDescription(EventType))

        Return _receipt
    End Function

    Public Function Update(ByVal pSchedDate As DateTime, ByVal pBOL As String, ByVal pNotes As String, ByVal pCarrier As String, ByVal pVEHICLE As String, ByVal pTrailer As String, _
            ByVal pDriver1 As String, ByVal pDriver2 As String, ByVal pSeal1 As String, ByVal pSeal2 As String, ByVal pTransportReference As String, ByVal pTransportType As String, ByVal pDoor As String, ByVal pLabelPrinted As Boolean, ByVal pUser As String) As String
        Dim SQL As String = ""

        _scheduleddate = pSchedDate
        _bol = pBOL
        _vehicle = pVEHICLE
        _trailer = pTrailer
        _driver1 = pDriver1
        _driver2 = pDriver2
        _notes = pNotes
        _seal1 = pSeal1
        _seal2 = pSeal2
        _transportreference = pTransportReference
        _transporttype = pTransportType
        _door = pDoor
        _labelprinted = pLabelPrinted
        _carriercompany = pCarrier
        _edituser = pUser
        _editdate = DateTime.Now

        SQL = String.Format("UPDATE RECEIPTHEADER SET SCHEDULEDDATE = {0}, BOL = {1}, NOTES = {2}, CARRIERCOMPANY = {3}, TRANSPORTREFERENCE={4},TRANSPORTTYPE={5},DOOR={6}, EDITDATE = {7}, EDITUSER = {8},TRAILER ={9}, VEHICLE ={10}, DRIVER1 ={11}, DRIVER2 ={12}, SEAL1 ={13}, SEAL2 ={14},LabelPrinted={15} Where {16}", _
            Made4Net.Shared.Util.FormatField(_scheduleddate), Made4Net.Shared.Util.FormatField(_bol), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_carriercompany), _
            Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_door), _
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
            Made4Net.Shared.Util.FormatField(_trailer), Made4Net.Shared.Util.FormatField(_vehicle), _
            Made4Net.Shared.Util.FormatField(_driver1), Made4Net.Shared.Util.FormatField(_driver2), _
            Made4Net.Shared.Util.FormatField(_seal1), Made4Net.Shared.Util.FormatField(_seal2), Made4Net.Shared.Util.FormatField(_labelprinted), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptUpdated
        em.Add("EVENT", EventType)
        em.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTHUPD)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ACTIVITYTIME", "0")
        em.Add("CONSIGNEE", "")
        em.Add("DOCUMENT", _receipt)
        em.Add("DOCUMENTLINE", 0)
        em.Add("FROMLOAD", "")
        em.Add("FROMLOC", "")
        em.Add("FROMQTY", 0)
        em.Add("FROMSTATUS", _status)
        em.Add("NOTES", _notes)
        em.Add("SKU", "")
        em.Add("TOLOAD", "")
        em.Add("TOLOC", "")
        em.Add("TOQTY", 0)
        em.Add("TOSTATUS", _status)
        em.Add("USERID", pUser)
        em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ADDUSER", pUser)
        em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("EDITUSER", pUser)
        em.Send(WMSEvents.EventDescription(EventType))

        Return _receipt
    End Function

    Public Sub Save(ByVal pUser As String)
        Dim SQL As String
        Dim aq As EventManagerQ = New EventManagerQ
        Dim acticitytype As String

        If Not Exists(_receipt) Then
            _status = WMS.Lib.Statuses.Receipt.STATUSNEW
            _adduser = pUser
            _edituser = pUser
            _adddate = DateTime.Now
            _editdate = DateTime.Now
            _startreceiptdate = Nothing

            If _receipt = "" Then
                _receipt = Made4Net.Shared.Util.getNextCounter("RCN")
            End If


            SQL = "INSERT INTO RECEIPTHEADER(RECEIPT,STATUS,TRAILER, VEHICLE, DRIVER1, DRIVER2, SEAL1, SEAL2," & _
                "SCHEDULEDDATE,BOL,NOTES,CARRIERCOMPANY,STARTRECEIPTDATE,TRANSPORTREFERENCE,TRANSPORTTYPE,DOOR,ADDDATE,ADDUSER,EDITDATE,EDITUSER,LABELPRINTED) Values ("
            SQL += Made4Net.Shared.Util.FormatField(_receipt) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & _
                Made4Net.Shared.Util.FormatField(_trailer) & "," & Made4Net.Shared.Util.FormatField(_vehicle) & "," & _
                Made4Net.Shared.Util.FormatField(_driver1) & "," & Made4Net.Shared.Util.FormatField(_driver2) & "," & _
                Made4Net.Shared.Util.FormatField(_seal1) & "," & Made4Net.Shared.Util.FormatField(_seal2) & "," & _
                Made4Net.Shared.Util.FormatField(_scheduleddate) & "," & Made4Net.Shared.Util.FormatField(_bol) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & _
                Made4Net.Shared.Util.FormatField(_carriercompany) & "," & Made4Net.Shared.Util.FormatField(_startreceiptdate) & "," & _
                Made4Net.Shared.Util.FormatField(_transportreference) & "," & Made4Net.Shared.Util.FormatField(_transporttype) & "," & Made4Net.Shared.Util.FormatField(_door) & "," & _
                Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(_editdate) & "," & _
                Made4Net.Shared.Util.FormatField(_edituser) & "," & Made4Net.Shared.Util.FormatField(_labelprinted) & ")"
            DataInterface.RunSQL(SQL)
            _lines = New ReceiptDetailCollection(_receipt)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptCreated)
            acticitytype = WMS.Lib.Actions.Audit.RECEIPTHINS
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptCreated
            'em.Add("EVENT", EventType)
            'em.Add("RECEIPT", _receipt)
            'em.Add("USERID", pUser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
        Else
            _edituser = pUser
            _editdate = DateTime.Now

            SQL = String.Format("UPDATE RECEIPTHEADER SET SCHEDULEDDATE = {0}, BOL = {1}, NOTES = {2}, CARRIERCOMPANY = {3}, TRANSPORTREFERENCE={4},TRANSPORTTYPE={5},DOOR={6}, EDITDATE = {7}, EDITUSER = {8},TRAILER ={9}, VEHICLE ={10}, DRIVER1 ={11}, DRIVER2 ={12}, SEAL1 ={13}, SEAL2 ={14},LABELPRINTED={15},STATUS={16} Where {17}", _
                Made4Net.Shared.Util.FormatField(_scheduleddate), Made4Net.Shared.Util.FormatField(_bol), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_carriercompany), _
                Made4Net.Shared.Util.FormatField(_transportreference), Made4Net.Shared.Util.FormatField(_transporttype), Made4Net.Shared.Util.FormatField(_door), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                Made4Net.Shared.Util.FormatField(_trailer), Made4Net.Shared.Util.FormatField(_vehicle), _
                Made4Net.Shared.Util.FormatField(_driver1), Made4Net.Shared.Util.FormatField(_driver2), _
                Made4Net.Shared.Util.FormatField(_seal1), Made4Net.Shared.Util.FormatField(_seal2), Made4Net.Shared.Util.FormatField(_labelprinted), Made4Net.Shared.FormatField(_status), WhereClause)
            DataInterface.RunSQL(SQL)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptUpdated)
            acticitytype = WMS.Lib.Actions.Audit.RECEIPTHUPD
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptUpdated
            'em.Add("EVENT", EventType)
            'em.Add("RECEIPT", _receipt)
            'em.Add("USERID", pUser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))

        End If
        aq.Add("ACTIVITYTYPE", acticitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _receipt)
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
        aq.Send(acticitytype)

        Load()

    End Sub

#End Region

#Region "Add Lines"

    Public Function addLineFromOrder(ByVal pConsignee As String, ByVal pOrderID As String, ByVal pOrderLine As Int32, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pDocumentType As String = "")
        _lines.CreateNewLine(pConsignee, pOrderID, pOrderLine, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pUser, True, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pDocumentType, pAvgWeight, pAvgWeightUOM)
        CalculateStdtime()
        setTimeStamp(WMS.Logic.GetCurrentUser)
    End Function

    ' If receipt line param < 0 then numbers will be populated automatically...
    Public Function addLine(ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pReceiptLine As Int32 = -1, Optional ByVal pDocumentType As String = "", Optional ByVal pOrderID As String = "", Optional ByVal pOrderLine As Integer = 0) As Int32
        If pReceiptLine > 0 And ReceiptDetail.LineExist(_receipt, pReceiptLine) Then
            Return UpdateLine(pReceiptLine, pConsignee, pSku, pQtyExpected, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pReceivedWeight, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pDocumentType, pOrderID, pOrderLine)
        Else
            Dim line As Int32 = _lines.CreateNewLine(pReceiptLine, pConsignee, pSku, pQtyExpected, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pDocumentType, pOrderID, pOrderLine, pAvgWeight, pAvgWeightUOM, pReceivedWeight)
            CalculateStdtime()
            setTimeStamp(WMS.Logic.GetCurrentUser)
            Return line
        End If
    End Function

    Public Function UpdateLine(ByVal LineNumber As Int32, ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pDocumentType As String = "", Optional ByVal pOrderID As String = "", Optional ByVal pOrderLine As Integer = 0)
        _lines.UpdateLine(LineNumber, pConsignee, pSku, pQtyExpected, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pReceivedWeight, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pDocumentType, pOrderID, pOrderLine)
        CalculateStdtime()
        setTimeStamp(WMS.Logic.GetCurrentUser)
    End Function

    Public Function SaveLine(ByVal oLine As ReceiptDetail, ByVal pUser As String) As Int32
        If ReceiptDetail.LineExist(Me.RECEIPT, oLine.RECEIPTLINE) Then
            UpdateLine(oLine.RECEIPTLINE, oLine.CONSIGNEE, oLine.SKU, oLine.QTYEXPECTED, oLine.REFERENCEORDER, oLine.REFERENCEORDERLINE, oLine.INVENTORYSTATUS, oLine.COMPANY, oLine.COMPANYTYPE, oLine.AVGWEIGHT, oLine.AVGWEIGHTUOM, oLine.RECEIVEDWEIGHT, pUser, oLine.Attributes.Attributes, oLine.INPUTQTY, oLine.INPUTUOM, oLine.DOCUMENTTYPE)
            ' Check if we can update order line
            Select Case oLine.DOCUMENTTYPE
                Case ""
                    If InboundOrderHeader.LineExists(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE) Then
                        Dim rd As ReceiptDetail = New ReceiptDetail(Me.RECEIPT, oLine.RECEIPTLINE)
                        rd.ORDERID = oLine.ORDERID
                        rd.ORDERLINE = oLine.ORDERLINE
                        rd.SetInboundDocument()
                    End If
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    If Flowthrough.LineExists(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE) Then
                        Dim rd As ReceiptDetail = New ReceiptDetail(Me.RECEIPT, oLine.RECEIPTLINE)
                        rd.ORDERID = oLine.ORDERID
                        rd.ORDERLINE = oLine.ORDERLINE
                        rd.SetInboundDocument()
                    End If
            End Select
            Return oLine.RECEIPTLINE
        Else
            ' Check if there is orderid and orderline
            Dim line As Int32
            Select Case oLine.DOCUMENTTYPE
                Case ""
                    If Not InboundOrderHeader.LineExists(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE) Then
                        line = _lines.CreateNewLine(oLine.RECEIPTLINE, oLine.CONSIGNEE, oLine.SKU, oLine.QTYEXPECTED, oLine.REFERENCEORDER, oLine.REFERENCEORDERLINE, oLine.INVENTORYSTATUS, oLine.COMPANY, oLine.COMPANYTYPE, pUser, oLine.Attributes.Attributes, oLine.INPUTQTY, oLine.INPUTUOM, oLine.INPUTSKU, oLine.DOCUMENTTYPE)
                    Else
                        line = _lines.CreateNewLine(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE, oLine.REFERENCEORDER, oLine.REFERENCEORDERLINE, oLine.INVENTORYSTATUS, oLine.COMPANY, oLine.COMPANYTYPE, pUser, True, oLine.Attributes.Attributes, oLine.INPUTQTY, oLine.INPUTUOM, oLine.INPUTSKU, oLine.DOCUMENTTYPE)
                    End If
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    If Not Flowthrough.LineExists(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE) Then
                        line = _lines.CreateNewLine(oLine.RECEIPTLINE, oLine.CONSIGNEE, oLine.SKU, oLine.QTYEXPECTED, oLine.REFERENCEORDER, oLine.REFERENCEORDERLINE, oLine.INVENTORYSTATUS, oLine.COMPANY, oLine.COMPANYTYPE, pUser, oLine.Attributes.Attributes, oLine.INPUTQTY, oLine.INPUTUOM, oLine.INPUTSKU, oLine.DOCUMENTTYPE)
                    Else
                        line = _lines.CreateNewLine(oLine.CONSIGNEE, oLine.ORDERID, oLine.ORDERLINE, oLine.REFERENCEORDER, oLine.REFERENCEORDERLINE, oLine.INVENTORYSTATUS, oLine.COMPANY, oLine.COMPANYTYPE, pUser, True, oLine.Attributes.Attributes, oLine.INPUTQTY, oLine.INPUTUOM, oLine.INPUTSKU, oLine.DOCUMENTTYPE)
                    End If
            End Select
            Calculatestdtime()
            setTimeStamp(WMS.Logic.GetCurrentUser)
            Return line
        End If
    End Function

    Private Sub getEquation()
        _sourceStdTimeEquation = DataInterface.ExecuteScalar(String.Format("select isnull(SERVICETIMEEQUATION,'') SERVICETIMEEQUATION from DOCUMENTTIMECALCULATIONMETHODS where DOCTYPE='{0}'", _
        WMS.Lib.DOCUMENTTYPES.RECEIPT))
    End Sub

    Private Function updateStdTime(ByVal pStdTime As Double)
        DataInterface.RunSQL(String.Format("update RECEIPTHEADER set ESTUNLOADINGTIME='{0}' where RECEIPT='{1}'", Math.Round(pStdTime, 2).ToString(), _receipt))
    End Function

    Private Function CalculateStdtime() As Double
        LoadCalcParameters()
        getEquation()
        Dim StdTime As Double = CalcStdTimeEquation(_sourceStdTimeEquation)
        updateStdTime(StdTime)

    End Function

    Private Function CalcStdTimeEquation(ByVal sourceEquation As String) As Double
        Dim targetEquation As String
        Dim res As Double
        Try
            Dim oEvalEquation As EvalEquation = New EvalEquation(Nothing, _StdTimeCalcParameters)
            res = oEvalEquation.EvalEquation(sourceEquation, targetEquation)
            Return res
        Catch ex As Exception
            Return 0D
        End Try
    End Function

    Private Function LoadCalcParameters() As Integer
        Dim sql As String = String.Format("SELECT * FROM vCalculateReceipStdtime where receipt='{0}'", _receipt)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        Dim dr As DataRow
        If dt.Rows.Count = 0 Then Return 0
        dr = dt.Rows(0)
        For Each column As DataColumn In dt.Columns
            addStdTimeCalcParameters(column.ColumnName, dr(column.ColumnName).ToString())
        Next
    End Function


    Private Sub addStdTimeCalcParameters(ByVal key As String, ByVal value As String)
        If Not _StdTimeCalcParameters.Contains(key) Then
            _StdTimeCalcParameters.Add(key, value)
        Else
            _StdTimeCalcParameters(key) = value
        End If
    End Sub

#End Region

#Region "Create Loads"

    Public Function CreateLoad(ByVal pLine As Int32, ByVal pLoadId As String, ByVal pLoadUom As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUnits As Decimal, ByVal pStatus As String, ByVal pResCode As String, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String, ByVal oLogger As LogHandler, Optional ByVal pDocumentType As String = "", Optional ByVal pIsUOMQty As Boolean = False) As Load
        If _status = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CANT CREATE LOAD RECEIPTCLOSED", "CANT CREATE LOAD RECEIPTCLOSED")
        End If
        Dim rLine As ReceiptDetail = _lines.Line(pLine)
        Dim oLoad As Load
        If IsNothing(_lines.Line(pLine)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Receipt Line Does Not Exists", "Receipt Line Does Not Exists")
            Throw m4nEx
        End If
        oLoad = rLine.CreateLoad(pLoadId, pLoadUom, pLocation, pWarehousearea, pUnits, pStatus, pResCode, oAttributeCollection, pUser, oLogger, pDocumentType, pIsUOMQty)
        If _startreceiptdate = DateTime.MinValue Then
            _startreceiptdate = DateTime.Now
        End If
        _editdate = DateTime.Now
        DataInterface.RunSQL(String.Format("update RECEIPTHEADER set Status = '{0}', StartReceiptDate = {1}, EditUser = {2}, EditDate = {3} Where {4}", WMS.Lib.Statuses.Receipt.RECEIVING, Made4Net.Shared.Util.FormatField(_startreceiptdate), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
        'Dim oQMsg As New EventManagerQ
        Dim unitPrice As Decimal
        unitPrice = SKUUnitPrice(rLine.SKU, rLine.CONSIGNEE)
        'oQMsg.Add("EVENT", WMS.Logic.WMSEvents.EventType.CreateLoad)
        'oQMsg.Add("LOADID", pLoadId)
        'oQMsg.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        'oQMsg.Add("USERID", pUser)
        'oQMsg.Add("TOLOC", pLocation)
        'oQMsg.Add("TOQTY", pUnits.ToString())
        'oQMsg.Add("UOM", pLoadUom)
        'oQMsg.Add("RECEIPT", _receipt)
        'oQMsg.Add("DOCUMENT", _receipt)
        'oQMsg.Add("RECEIPTLINE", pLine)
        'oQMsg.Add("CONSIGNEE", rLine.CONSIGNEE)
        'oQMsg.Add("UNITPRICE", unitPrice)
        'oQMsg.Add("SKU", rLine.SKU)
        'oQMsg.Add("STATUS", pStatus)
        'oQMsg.Add("REASONCODE", pResCode)
        'oQMsg.Add("RECEIVEDDATE", Made4Net.Shared.Util.DateTimeToWMSString(oLoad.RECEIVEDATE))
        'oQMsg.Add(oLoad.LoadAttributes.Attributes.ToNameValueCollection)
        'oQMsg.Send(WMS.Logic.WMSEvents.EventType.CreateLoad)


        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CreateLoad)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CREATELOAD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", rLine.CONSIGNEE)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", rLine.RECEIPTLINE)
        aq.Add("FROMLOAD", oLoad.LOADID)
        aq.Add("FROMLOC", oLoad.LOCATION)
        aq.Add("FROMWAREHOUSEAREA", oLoad.WAREHOUSEAREA)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oLoad.STATUS)
        aq.Add("NOTES", "")
        aq.Add("SKU", rLine.SKU)
        aq.Add("TOLOAD", oLoad.LOADID)
        aq.Add("TOLOC", oLoad.LOCATION)
        aq.Add("TOWAREHOUSEAREA", oLoad.WAREHOUSEAREA)
        aq.Add("TOQTY", oLoad.UNITS)
        aq.Add("TOSTATUS", oLoad.STATUS)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.CREATELOAD)


        Dim it As IInventoryTransactionQ = InventoryTransactionQ.Factory.NewInventoryTransactionQ()
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.CREATELOAD)
        it.Add("CONSIGNEE", oLoad.CONSIGNEE)
        it.Add("DOCUMENT", _receipt)
        it.Add("LINE", rLine.RECEIPTLINE)
        it.Add("LOADID", oLoad.LOADID)
        it.Add("UOM", oLoad.LOADUOM)
        it.Add("QTY", oLoad.UNITS)
        it.Add("CUBE", oLoad.Volume)
        it.Add("LOADWEIGHT", oLoad.Weight)
        it.Add("AMOUNT", 0)
        it.Add("SKU", rLine.SKU)
        it.Add("STATUS", oLoad.STATUS)
        it.Add("REASONCODE", "")
        it.Add("UNITPRICE", unitPrice)
        InventoryTransaction.CreateAttributesRecords(it, oLoad.LoadAttributes)

        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", pUser)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", pUser)
        it.Send(WMS.Lib.INVENTORY.CREATELOAD)
        Return oLoad
    End Function

    Public Sub CancelReceive(ByVal pLine As Int32, ByVal pLoadid As String, ByVal pReasonCode As String, ByVal pUser As String)

        'RWMS-2684
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2684 END

        If _status = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "CANT CREATE LOAD RECEIPTCLOSED", "CANT CREATE LOAD RECEIPTCLOSED")
        End If
        Dim rLine As ReceiptDetail = _lines.Line(pLine)
        If IsNothing(_lines.Line(pLine)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Receipt Line Does Not Exists", "Receipt Line Does Not Exists")
            Throw m4nEx
        End If
        'RWMS-2684
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" Started to Cancel receipt:{0}, receiptline:{1}", _receipt, pLine))
        End If
        'RWMS-2684 END

        rLine.CancelReceive(pLoadid, pReasonCode, pUser)
    End Sub

#End Region

#Region "Confirm"

    Public Sub Confirm(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Receipt.STATUSNEW Then
            _confirmed = True
            _edituser = pUser
            _editdate = DateTime.Now
            DataInterface.RunSQL(String.Format("update receiptheader set CONFIRMED = {0},edituser={1},editdate={2} where {3}", Made4Net.Shared.Util.FormatField(_confirmed), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Receipt status incorrect", "Receipt status incorrect")
        End If

    End Sub

#End Region

#Region "Cancel"

    Public Sub Cancel(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Receipt.STATUSNEW Then
            _status = WMS.Lib.Statuses.Receipt.CANCELLED
            _edituser = pUser
            _editdate = DateTime.Now
            DataInterface.RunSQL(String.Format("update receiptheader set status = {0},edituser={1},editdate={2} where {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptCancelled
            'em.Add("EVENT", EventType)
            'em.Add("USERID", pUser)
            'em.Add("DOCUMENT", _receipt)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptCancelled)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTHCNCL)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _receipt)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", "")
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.RECEIPTHCNCL)

        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Receipt Status", "Incorrect Receipt Status")
        End If
    End Sub

#End Region

#Region "CLose"

    Public Sub close(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Receipt.CLOSED OrElse _status = WMS.Lib.Statuses.Receipt.CANCELLED Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not close receipt. Incorrect status.", "Can not close receipt. Incorrect status.")
        End If

        Dim _prVal As New ProcessValidator(WMS.Logic.WMSEvents.EventType.ReceiptClose)
        Dim _prValRes As String
        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
        vals.Add("RECEIPT", Me.RECEIPT)
        _prVal.FieldValues = vals
        _prValRes = _prVal.Validate()
        If _prValRes.Split(";")(0).Equals("-1") Then
            If _prValRes.Split(";").Length > 1 Then
                Dim errMsg As String = _prValRes.Split(";")(1)
                Throw New Made4Net.Shared.M4NException(New Exception, errMsg, errMsg)
            Else
                'Commented for RWMS-533
                'Throw New ApplicationException("Cannot close receipt.Process validation failed.")
                'End Commented for RWMS-533
                'Added for RWMS-533
                Throw New ApplicationException("Cannot close receipt. Open receipt lines.")
                'End Added for RWMS-533
            End If
        End If

        _status = WMS.Lib.Statuses.Receipt.CLOSED
        _closereceiptdate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now
        DataInterface.RunSQL(String.Format("update receiptheader set status = {0},edituser={1},editdate={2},CLOSERECEIPTDATE={3} where {4}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_closereceiptdate), WhereClause))
        For Each rl As ReceiptDetail In _lines
            rl.CalculateAverageWeight()
            Select Case rl.DOCUMENTTYPE
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim oFlowthrough As New Flowthrough(rl.CONSIGNEE, rl.ORDERID)
                    oFlowthrough.SetReceived(pUser)
            End Select
        Next
        'Added for RWMS-1450 and RWMS-1452
        RaiseReceiptCloseEvent(pUser)

    End Sub


    Public Sub RaiseReceiptCloseEvent(ByVal pUser As String)
        'Ended for RWMS-1450 and RWMS-1452
        Try
           
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptClose)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTCLOSE)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("WAREHOUSE", Warehouse.CurrentWarehouse) 'RWMS-1487
            aq.Add("DOCUMENT", _receipt)
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
            aq.Add("TOQTY", "")
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.RECEIPTCLOSE)
        Catch ex As Exception
        End Try
    End Sub

#End Region

#Region "At Dock"

    Public Sub AtDock(ByVal pUser As String)
        If Me.STATUS <> WMS.Lib.Statuses.Receipt.STATUSNEW AndAlso Me.STATUS <> WMS.Lib.Statuses.Receipt.SCHEDULED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Receipt Status Incorrect", "Receipt Status Incorrect")
        End If
        Dim oldStatus, SQL As String
        oldStatus = _status
        _edituser = pUser
        _editdate = DateTime.Now
        _status = WMS.Lib.Statuses.Shipment.ATDOCK
        Me.Save(pUser)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptAtDock)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTHADC)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.RECEIPTHADC)
    End Sub

#End Region

#Region "Batch Receive"

    Public Sub Receive(ByVal sLocation As String, ByVal sWarehousearea As String, ByVal sUom As String, ByVal sLoadStatus As String, ByVal bPrintLabel As Boolean, ByVal sLabelPrinter As String, ByVal sUserId As String)
        Receive(sLocation, sWarehousearea, sUom, sLoadStatus, bPrintLabel, sLabelPrinter, sUserId, True)
    End Sub

    Public Sub Receive(ByVal sLocation As String, ByVal sWarehousearea As String, ByVal sUom As String, ByVal sLoadStatus As String, ByVal bPrintLabel As Boolean, ByVal sLabelPrinter As String, ByVal sUserId As String, ByVal bCloseReceipt As Boolean)
        If Me.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New ApplicationException("Receipt already closed")
        End If

        Dim iQtyLeft As Decimal
        Dim iUnitsPerMeasure As Decimal
        Dim oSku As SKU
        Dim ldid As String
        Dim oLd As Load
        For Each oRecLine As ReceiptDetail In Me.LINES
            iQtyLeft = oRecLine.QTYEXPECTED - oRecLine.QTYRECEIVED
            oSku = New SKU(oRecLine.CONSIGNEE, oRecLine.SKU)
            ThrowExceptionOnNewSku(oSku)
            iUnitsPerMeasure = oSku.ConvertToUnits(sUom)
            While iQtyLeft > 0
                ldid = Made4Net.Shared.Util.getNextCounter("LOAD")
                If iQtyLeft < iUnitsPerMeasure Then
                    oLd = Me.CreateLoad(oRecLine.RECEIPTLINE, ldid, sUom, sLocation, sWarehousearea, iQtyLeft, sLoadStatus, Nothing, Nothing, sUserId, Nothing)
                    iQtyLeft = 0
                Else
                    oLd = Me.CreateLoad(oRecLine.RECEIPTLINE, ldid, sUom, sLocation, sWarehousearea, 1, sLoadStatus, Nothing, Nothing, sUserId, Nothing)
                    iQtyLeft = iQtyLeft = iUnitsPerMeasure
                End If
                If bPrintLabel Then
                    oLd.PrintLabel(sLabelPrinter)
                End If
            End While
        Next
        If bCloseReceipt Then
            Me.close(sUserId)
        End If
    End Sub
    Private Sub ThrowExceptionOnNewSku(ByVal osku As SKU)

        If osku.NEWSKU Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot recieve line, New sku", "Cannot recieve line, New sku")
            Throw m4nEx
        End If
    End Sub

    Public Function ReceiveFullReceipt(ByVal sUserId As String, ByVal bCloseReceipt As Boolean, Optional ByVal sLoadStatus As String = "", Optional ByVal bPrintLabel As Boolean = False, Optional ByVal sLabelPrinter As String = "") As String
        If Me.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New ApplicationException("Receipt already closed")
        End If

        Dim iQtyLeft As Decimal
        Dim oSku As SKU
        Dim ldid As String
        Dim oLd As Load
        Dim oCons As Consignee
        Dim oLoc As Location
        For Each oRecLine As ReceiptDetail In Me.LINES
            iQtyLeft = oRecLine.OPENQTY

            If iQtyLeft > 0 Then

                oSku = New SKU(oRecLine.CONSIGNEE, oRecLine.SKU)
                ThrowExceptionOnNewSku(oSku)
                oCons = New Consignee(oRecLine.CONSIGNEE)

                ldid = Made4Net.Shared.Util.getNextCounter("LOAD")
                iQtyLeft = iQtyLeft / oSku.ConvertToUnits(oSku.DEFAULTUOM)

                If (String.IsNullOrEmpty(oCons.DEFAULTRECEIVINGLOCATION) Or String.IsNullOrEmpty(oCons.DEFAULTRECEIVINGWAREHOUSEAREA)) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Default receiving location or WHarea problem", "Default receiving location or WHarea problem")
                    Throw m4nEx
                Else 'check if location exists
                    If Not WMS.Logic.Location.Exists(oCons.DEFAULTRECEIVINGLOCATION, oCons.DEFAULTRECEIVINGWAREHOUSEAREA) Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Default receiving location does not exist", "Default receiving location does not exist")
                        Throw m4nEx
                    End If
                End If

                oLd = Me.CreateLoad(oRecLine.RECEIPTLINE, ldid, oSku.DEFAULTUOM, oCons.DEFAULTRECEIVINGLOCATION, oCons.DEFAULTRECEIVINGWAREHOUSEAREA, _
                    iQtyLeft, sLoadStatus, Nothing, Nothing, sUserId, Nothing, oRecLine.DOCUMENTTYPE)

                If bPrintLabel Then
                    oLd.PrintLabel(sLabelPrinter)
                End If
            End If
        Next
        If bCloseReceipt Then
            Me.close(sUserId)
        End If
    End Function

#End Region

#Region "Print"

    Public Shared Function PrintReceivingDoc(ByVal ReceiptID As String, ByVal Language As Int32, ByVal UserId As String, Optional ByVal isManifest As Boolean = True)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        If isManifest Then
            repType = WMS.Lib.REPORTS.RECEIVINGMANIFEST
            oQsender.Add("REPORTID", "ReceivingManifest")
            oQsender.Add("DATASETID", "repReceivingManifest")
            'RWMS-2609 - replaced the db connection from sys to app 
            DataInterface.FillDataset(String.Format("Select * from sys_reportparams where reportid = '{0}' and paramname = '{1}'", "ReceivingManifest", "Copies"), dt, False)
        Else
            repType = WMS.Lib.REPORTS.RECEIVINGWORKSHEET
            oQsender.Add("REPORTID", "ReceivingWorksheet")
            oQsender.Add("DATASETID", "repReceivingWorksheet")
            'RWMS-2609 - replaced the db connection from sys to app 
            DataInterface.FillDataset(String.Format("Select * from sys_reportparams where reportid = '{0}' and paramname = '{1}'", "ReceivingWorksheet", "Copies"), dt, False)
        End If
        'DataInterface.FillDataset(String.Format("Select * from reports where reportname = '{0}'", repType), dt)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", UserId)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try


        oQsender.Add("WHERE", "RECEIPT = '" & ReceiptID & "'")
        oQsender.Send("Report", repType)
    End Function

#End Region

#Region "Blind Receive"

    Public Function AddLineFromBlindReceiving(ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pUser As String) As ReceiptDetail
        Dim line As Int32 = ReceiptDetail.LineExist(_receipt, pConsignee, pSku)
        Dim recDetail As ReceiptDetail
        Dim oSku As SKU = New SKU(pConsignee, pSku)
        If line = -1 Then
            'add new line to the receipt
            recDetail = New ReceiptDetail(_receipt, addLine(pConsignee, pSku, pQtyExpected, Nothing, Nothing, oSku.INITIALSTATUS, Nothing, Nothing, -1, "", 0, pUser, Nothing))
        Else
            'update the line
            recDetail = New ReceiptDetail(_receipt, line)
            recDetail.Update(pConsignee, pSku, recDetail.QTYEXPECTED + pQtyExpected, Nothing, Nothing, oSku.INITIALSTATUS, Nothing, Nothing, -1, "", 0, pUser, Nothing)
        End If
        Return recDetail
    End Function

#End Region

#Region "Handling Units"

    Public Sub AddHandlingUnit(ByVal pHandlingUnitType As String, ByVal pQty As Decimal, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Receipt Status", "Incorrect Receipt Status")
        End If
        Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction
        oHUTrans.Create(Nothing, WMS.Logic.HandlingUnitTransactionTypes.Receipt, _receipt, Nothing, _
                    Nothing, Nothing, Nothing, Nothing, pHandlingUnitType, pQty, pUser)
        _handlingunits.Add(oHUTrans)
    End Sub

    Public Sub UpdateHandlingUnit(ByVal pHUTransId As String, ByVal pQty As Decimal, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Receipt Status", "Incorrect Receipt Status")
        End If
        Dim oHUTrans As New WMS.Logic.HandlingUnitTransaction(pHUTransId)
        oHUTrans.UpdateHUQty(pQty, pUser)
        _handlingunits.Remove(_handlingunits.HandlingUnitTransaction(pHUTransId))
        _handlingunits.Add(oHUTrans)
    End Sub

#End Region

#Region "Yard"

    Public Sub AssignToYardEntry(ByVal pYardEntryId As String, ByVal pUserId As String)
        'If _status = WMS.Lib.Statuses.Receipt.CANCELLED Or _status = WMS.Lib.Statuses.Receipt.CLOSED Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Receipt Status Incorrect", "Receipt Status Incorrect")
        'End If
        'If Not YardEntry.Exists(pYardEntryId) Then
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Yard Entry does not exists", "Yard Entry does not exists")
        'End If
        'Dim SQL As String
        '_edituser = pUserId
        '_editdate = DateTime.Now
        '_yardentryid = pYardEntryId
        'SQL = String.Format("UPDATE RECEIPTHEADER SET YARDENTRYID ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_yardentryid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        'DataInterface.RunSQL(SQL)
    End Sub

    Public Sub UnAssignFromYardEntry(ByVal pUserId As String)
        If _status = WMS.Lib.Statuses.Receipt.CANCELLED Or _status = WMS.Lib.Statuses.Receipt.CLOSED Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Receipt Status Incorrect", "Receipt Status Incorrect")
        End If
        Dim SQL As String
        _edituser = pUserId
        _editdate = DateTime.Now
        _yardentryid = ""
        SQL = String.Format("UPDATE RECEIPTHEADER SET YARDENTRYID ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_yardentryid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

    Public Sub Schedule(ByVal pUserId As String)
        If _status = WMS.Lib.Statuses.Receipt.STATUSNEW Then
            Dim SQL As String
            _edituser = pUserId
            _editdate = DateTime.Now
            SQL = String.Format("UPDATE RECEIPTHEADER SET STATUS ={0}, EDITDATE ={1}, EDITUSER ={2} WHERE {3}", WMS.Lib.Statuses.Receipt.SCHEDULED, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(SQL)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Receipt Status Incorrect", "Receipt Status Incorrect")
        End If
    End Sub

    Public Sub SetBestDoorForReceiving(ByVal pUserId As String)
        If _status = WMS.Lib.Statuses.Receipt.CLOSED Or _status = WMS.Lib.Statuses.Receipt.CANCELLED Or _status = WMS.Lib.Statuses.Receipt.RECEIVING Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Receipt Status Incorrect", "Receipt Status Incorrect")
        Else
            Dim SQL As String
            Dim dt As New DataTable
            SQL = String.Format("select * from vReceiptReceivingLocations where receipt = '{0}' order by distancetolocation asc", _receipt)
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Return
            End If
            _door = dt.Rows(0)("Location")
            _edituser = pUserId
            _editdate = DateTime.Now
            SQL = String.Format("UPDATE RECEIPTHEADER SET DOOR={0}, EDITDATE={1}, EDITUSER={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_door), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(SQL)
        End If
    End Sub

#End Region

#Region "Create ASNs"

    Public Sub CreateASNDetailsForLine(ByVal pLine As Int32, ByVal pUser As String)
        Me.LINES(pLine - 1).CreateASNDetails(pUser)
        'Dim lineIndexInCollection = pLine - 1
        'Dim lineSKU As New WMS.Logic.SKU(Me.LINES(lineIndexInCollection).CONSIGNEE, Me.LINES(lineIndexInCollection).SKU)
        'Dim quantity = Me.LINES(lineIndexInCollection).QTYEXPECTED - Me.LINES(lineIndexInCollection).QTYRECEIVED
        'If quantity > 0 Then
        '    Dim remainder As Decimal = quantity Mod lineSKU.UNITSOFMEASURE.UOM(lineSKU.DEFAULTRECLOADUOM).UNITSPERLOWESTUOM
        '    Dim numOfLoads As Int32 = quantity / lineSKU.UNITSOFMEASURE.UOM(lineSKU.DEFAULTRECLOADUOM).UNITSPERLOWESTUOM
        '    Dim asnDet As AsnDetail = New AsnDetail()
        '    For i As Integer = 1 To numOfLoads
        '        asnDet.Create("", Me.RECEIPT, pLine, "", lineSKU.DEFAULTRECLOADUOM, _
        '        lineSKU.UNITSOFMEASURE.UOM(lineSKU.DEFAULTRECLOADUOM).UNITSPERLOWESTUOM, WMS.Logic.Load.GenerateLoadId(), Me.LINES(lineIndexInCollection).Attributes.Attributes, pUser)
        '    Next
        '    asnDet.Create("", Me.RECEIPT, pLine, "", lineSKU.LOWESTUOM, remainder, WMS.Logic.Load.GenerateLoadId(), _
        '    Me.LINES(lineIndexInCollection).Attributes.Attributes, pUser)
        'End If
    End Sub

    Public Sub CreateASNDetailsForReceipt(ByVal pUser As String)
        If Not Me.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED And Not Me.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
            For i As Int32 = 1 To Me.LINES.Count
                CreateASNDetailsForLine(i, pUser)
            Next
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Receipt status is not valid for this operation.", "Receipt status is not valid for this operation.")
        End If

        updateLabelPrinted(True, pUser)
    End Sub

    Private Sub updateLabelPrinted(ByVal pValue As Boolean, ByVal pUser As String)
        _labelprinted = pValue
        Me.Update(_scheduleddate, _bol, _notes, _carriercompany, _vehicle, _trailer, _driver1, _driver2, _seal1, _seal2, _transportreference, _transporttype, _
        _door, _labelprinted, pUser)
    End Sub

#End Region

End Class

#End Region

