Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

#Region "CONSIGNEE"

' <summary>
' This object represents the properties and methods of a CONSIGNEE.
' </summary>

<CLSCompliant(False)> Public Class Consignee

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _consigneename As String = String.Empty
    Protected _notes As String = String.Empty
    Protected _credit As Double
    Protected _contactid As String = String.Empty
    Protected _loadlabel As String = String.Empty
    Protected _flowthroughloadlabel As String = String.Empty
    Protected _skulabel As String = String.Empty
    Protected _loaddetaillbl As String = String.Empty
    Protected _shpcartonlbl As String = String.Empty
    Protected _shpcontainerlbl As String = String.Empty
    Protected _rcvmanifest As String = String.Empty
    Protected _packinglist As String = String.Empty
    Protected _shippingmanifest As String = String.Empty
    Protected _billingperformadoc As String = String.Empty
    Protected _mixshipping As Boolean
    Protected _cyclecounting As Int32
    Protected _replanplanshort As Boolean
    Protected _generateloadid As Boolean
    Protected _autoprintloadlabelrcv As Boolean
    Protected _creditlimit As Double
    Protected _cubelimit As Double
    Protected _shippartiaload As Boolean
    Protected _receivingloc As String = String.Empty
    Protected _receivingwarehousearea As String = String.Empty
    Protected _billingaccount As String = String.Empty
    Protected _packmultipleorders As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _contact As Contact
    Protected _promptforrcvqty As Boolean

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "'"
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

    Public ReadOnly Property Contact() As Logic.Contact
        Get
            Return _contact
        End Get
    End Property

    Public Property CONSIGNEENAME() As String
        Get
            Return _consigneename
        End Get
        Set(ByVal Value As String)
            _consigneename = Value
        End Set
    End Property

    Public Property DEFAULTRECEIVINGLOCATION() As String
        Get
            Return _receivingloc
        End Get
        Set(ByVal Value As String)
            _receivingloc = Value
        End Set
    End Property

    Public Property DEFAULTRECEIVINGWAREHOUSEAREA() As String
        Get
            Return _receivingWarehousearea
        End Get
        Set(ByVal Value As String)
            _receivingWarehousearea = Value
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

    Public Property CREDIT() As Double
        Get
            Return _credit
        End Get
        Set(ByVal Value As Double)
            _credit = Value
        End Set
    End Property

    Public Property CONTACTID() As String
        Get
            Return _contactid
        End Get
        Set(ByVal Value As String)
            _contactid = Value
        End Set
    End Property

    Public Property LOADLABEL() As String
        Get
            Return _loadlabel
        End Get
        Set(ByVal Value As String)
            _loadlabel = Value
        End Set
    End Property

    Public Property FLOWTHROUGHLOADLABEL() As String
        Get
            Return _flowthroughloadlabel
        End Get
        Set(ByVal Value As String)
            _flowthroughloadlabel = Value
        End Set
    End Property

    Public Property SKULABEL() As String
        Get
            Return _skulabel
        End Get
        Set(ByVal Value As String)
            _skulabel = Value
        End Set
    End Property

    Public Property LOADDETAILLBL() As String
        Get
            Return _loaddetaillbl
        End Get
        Set(ByVal Value As String)
            _loaddetaillbl = Value
        End Set
    End Property

    Public Property SHPCARTONLBL() As String
        Get
            Return _shpcartonlbl
        End Get
        Set(ByVal Value As String)
            _shpcartonlbl = Value
        End Set
    End Property

    Public Property SHPCONTAINERLBL() As String
        Get
            Return _shpcontainerlbl
        End Get
        Set(ByVal Value As String)
            _shpcontainerlbl = Value
        End Set
    End Property

    Public Property RCVMANIFEST() As String
        Get
            Return _rcvmanifest
        End Get
        Set(ByVal Value As String)
            _rcvmanifest = Value
        End Set
    End Property

    Public Property PACKINGLIST() As String
        Get
            Return _packinglist
        End Get
        Set(ByVal Value As String)
            _packinglist = Value
        End Set
    End Property

    Public Property SHIPPINGMANIFEST() As String
        Get
            Return _shippingmanifest
        End Get
        Set(ByVal Value As String)
            _shippingmanifest = Value
        End Set
    End Property

    Public Property BILLINGPERFORMADOC() As String
        Get
            Return _billingperformadoc
        End Get
        Set(ByVal Value As String)
            _billingperformadoc = Value
        End Set
    End Property

    Public Property BILLINGACCOUNT() As String
        Get
            Return _billingaccount
        End Get
        Set(ByVal Value As String)
            _billingaccount = Value
        End Set
    End Property

    Public Property MIXSHIPPING() As Boolean
        Get
            Return _mixshipping
        End Get
        Set(ByVal Value As Boolean)
            _mixshipping = Value
        End Set
    End Property

    Public Property CYCLECOUNTING() As Int32
        Get
            Return _cyclecounting
        End Get
        Set(ByVal Value As Int32)
            _cyclecounting = Value
        End Set
    End Property

    Public Property REPLANPLANSHORT() As Boolean
        Get
            Return _replanplanshort
        End Get
        Set(ByVal Value As Boolean)
            _replanplanshort = Value
        End Set
    End Property

    Public Property GENERATELOADID() As Boolean
        Get
            Return _generateloadid
        End Get
        Set(ByVal Value As Boolean)
            _generateloadid = Value
        End Set
    End Property

    Public Property AUTOGENERATELOADLABELRCV() As Boolean
        Get
            Return _autoprintloadlabelrcv
        End Get
        Set(ByVal Value As Boolean)
            _autoprintloadlabelrcv = Value
        End Set
    End Property

    Public Property CREDITLIMIT() As Double
        Get
            Return _creditlimit
        End Get
        Set(ByVal Value As Double)
            _creditlimit = Value
        End Set
    End Property

    Public Property CUBELIMIT() As Double
        Get
            Return _cubelimit
        End Get
        Set(ByVal Value As Double)
            _cubelimit = Value
        End Set
    End Property

    Public Property SHIPPARTIALOAD() As Boolean
        Get
            Return _shippartiaload
        End Get
        Set(ByVal Value As Boolean)
            _shippartiaload = Value
        End Set
    End Property

    Public Property PACKMULTIPLEORDERS() As Boolean
        Get
            Return _packmultipleorders
        End Get
        Set(ByVal Value As Boolean)
            _packmultipleorders = Value
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

    Public Property PROMPTFORRCVQTY() As Boolean
        Get
            Return _promptforrcvqty
        End Get
        Set(ByVal Value As Boolean)
            _promptforrcvqty = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName.ToLower = "setcontact" Then
            _contactid = ds.Tables(0).Rows(0)("contactid")
            LoadByContactId(_contactid)
            _contact.SetContact(Convert.ReplaceDBNull(dr("contactid")), Convert.ReplaceDBNull(dr("street1")), Convert.ReplaceDBNull(dr("street2")), Convert.ReplaceDBNull(dr("city")), Convert.ReplaceDBNull(dr("state")), Convert.ReplaceDBNull(dr("zip")), Convert.ReplaceDBNull(dr("CONTACT1NAME")), _
               Convert.ReplaceDBNull(dr("CONTACT2NAME")), Convert.ReplaceDBNull(dr("CONTACT1PHONE")), Convert.ReplaceDBNull(dr("CONTACT2PHONE")), Convert.ReplaceDBNull(dr("CONTACT1FAX")), Convert.ReplaceDBNull(dr("CONTACT2FAX")), Convert.ReplaceDBNull(dr("CONTACT1EMAIL")), Convert.ReplaceDBNull(dr("CONTACT2EMAIL")), _
               Convert.ReplaceDBNull(dr("ROUTE")), Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), Convert.ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "createnew" Then
            CreateNew(dr("consignee"), Convert.ReplaceDBNull(dr("consigneename")), Convert.ReplaceDBNull(dr("Notes")), Convert.ReplaceDBNull(dr("ReceivingLoc")), Convert.ReplaceDBNull(dr("receivingwarehousearea")), Convert.ReplaceDBNull(dr("flowthroughloadlabel")), Convert.ReplaceDBNull(dr("billingaccount")), dr("packmultipleorders"), Convert.ReplaceDBNull(dr("contactid")), Convert.ReplaceDBNull(dr("promptforrcvqty")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "update" Then
            Dim oConsignee As New WMS.Logic.Consignee(dr("consignee"))
            oConsignee.Update(Convert.ReplaceDBNull(dr("consigneename")), Convert.ReplaceDBNull(dr("Notes")), Convert.ReplaceDBNull(dr("ReceivingLoc")), Convert.ReplaceDBNull(dr("receivingwarehousearea")), Convert.ReplaceDBNull(dr("flowthroughloadlabel")), Convert.ReplaceDBNull(dr("billingaccount")), dr("packmultipleorders"), Convert.ReplaceDBNull(dr("contactid")), Convert.ReplaceDBNull(dr("promptforrcvqty")), Common.GetCurrentUser)
        End If
    End Sub

#End Region

#Region "Methods"

    Public Sub Save(ByVal pUser As String)
        Dim SQL As String
        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String
        If (WMS.Logic.Consignee.Exists(_consignee)) Then
            SQL = String.Format("UPDATE CONSIGNEE " & _
                                "SET CONSIGNEENAME ={0}, NOTES ={1}, CREDIT ={2}, CONTACTID ={3}, LOADLABEL ={4}, SKULABEL ={5}, LOADDETAILLBL ={6}, SHPCARTONLBL ={7}, SHPCONTAINERLBL ={8}, " & _
                                "RCVMANIFEST ={9}, PACKINGLIST ={10}, SHIPPINGMANIFEST ={11}, BILLINGPERFORMADOC ={12}, MIXSHIPPING ={13}, CYCLECOUNTING ={14}, REPLANPLANSHORT ={15}, " & _
                                "GENERATELOADID ={16}, AUTOPRINTLOADLABELRCV ={17}, CREDITLIMIT ={18}, CUBELIMIT ={19}, SHIPPARTIALOAD ={20}, RECEIVINGLOC ={21}, RECEIVINGWAREHOUSEAREA  ={27}, EDITDATE ={22}, EDITUSER ={23}, " & _
                                "FLOWTHROUGHLOADLABEL ={24}, BILLINGACCOUNT ={25},PROMPTFORRCVQTY={28} WHERE CONSIGNEE={26}", _
                                Made4Net.Shared.Util.FormatField(_consigneename), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_credit), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_loadlabel), Made4Net.Shared.Util.FormatField(_skulabel), Made4Net.Shared.Util.FormatField(_loaddetaillbl), Made4Net.Shared.Util.FormatField(_shpcartonlbl), Made4Net.Shared.Util.FormatField(_shpcontainerlbl), _
                                Made4Net.Shared.Util.FormatField(_rcvmanifest), Made4Net.Shared.Util.FormatField(_packinglist), Made4Net.Shared.Util.FormatField(_shippingmanifest), Made4Net.Shared.Util.FormatField(_billingperformadoc), Made4Net.Shared.Util.FormatField(_mixshipping), Made4Net.Shared.Util.FormatField(_cyclecounting), Made4Net.Shared.Util.FormatField(_replanplanshort), _
                                Made4Net.Shared.Util.FormatField(_generateloadid), Made4Net.Shared.Util.FormatField(_autoprintloadlabelrcv), Made4Net.Shared.Util.FormatField(_creditlimit), Made4Net.Shared.Util.FormatField(_cubelimit), Made4Net.Shared.Util.FormatField(_shippartiaload), Made4Net.Shared.Util.FormatField(_receivingloc), Made4Net.Shared.Util.FormatField(DateTime.Now()), Made4Net.Shared.Util.FormatField("SYSTEM"), _
                                Made4Net.Shared.Util.FormatField(_flowthroughloadlabel), Made4Net.Shared.Util.FormatField(_billingaccount), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_receivingwarehousearea), Made4Net.Shared.Util.FormatField(_promptforrcvqty))

            DataInterface.RunSQL(SQL)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ConsigneeUpdated)
            activitytype = WMS.Lib.Actions.Audit.CONSIGNEEUPD
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ConsigneeUpdated
            'em.Add("EVENT", EventType)
            'em.Add("CONSIGNEE", _consignee)
            'em.Add("USERID", pUser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))

        Else
            SQL = String.Format("INSERT INTO CONSIGNEE " & _
                      "(CONSIGNEE, CONSIGNEENAME, NOTES, CREDIT, CONTACTID, LOADLABEL, SKULABEL, LOADDETAILLBL, SHPCARTONLBL, SHPCONTAINERLBL, " & _
                      "RCVMANIFEST, PACKINGLIST, SHIPPINGMANIFEST, BILLINGPERFORMADOC, MIXSHIPPING, CYCLECOUNTING, REPLANPLANSHORT, " & _
                      "GENERATELOADID, AUTOPRINTLOADLABELRCV, CREDITLIMIT, CUBELIMIT, SHIPPARTIALOAD, RECEIVINGLOC, RECEIVINGWAREHOUSEAREA, ADDDATE, ADDUSER, EDITDATE, " & _
                      "EDITUSER, FLOWTHROUGHLOADLABEL, BILLINGACCOUNT,PROMPTFORRCVQTY) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28})", _
                      Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_consigneename), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_credit), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_loadlabel), Made4Net.Shared.Util.FormatField(_skulabel), Made4Net.Shared.Util.FormatField(_loaddetaillbl), Made4Net.Shared.Util.FormatField(_shpcartonlbl), Made4Net.Shared.Util.FormatField(_shpcontainerlbl), _
                      Made4Net.Shared.Util.FormatField(_rcvmanifest), Made4Net.Shared.Util.FormatField(_packinglist), Made4Net.Shared.Util.FormatField(_shippingmanifest), Made4Net.Shared.Util.FormatField(_billingperformadoc), Made4Net.Shared.Util.FormatField(_mixshipping), Made4Net.Shared.Util.FormatField(_cyclecounting), Made4Net.Shared.Util.FormatField(_replanplanshort), _
                      Made4Net.Shared.Util.FormatField(_generateloadid), Made4Net.Shared.Util.FormatField(_autoprintloadlabelrcv), Made4Net.Shared.Util.FormatField(_creditlimit), Made4Net.Shared.Util.FormatField(_cubelimit), Made4Net.Shared.Util.FormatField(_shippartiaload), Made4Net.Shared.Util.FormatField(_receivingloc), Made4Net.Shared.Util.FormatField(_receivingwarehousearea), Made4Net.Shared.Util.FormatField(DateTime.Now()), Made4Net.Shared.Util.FormatField("SYSTEM"), Made4Net.Shared.Util.FormatField(DateTime.Now()), Made4Net.Shared.Util.FormatField("SYSTEM"), _
                      Made4Net.Shared.Util.FormatField(_flowthroughloadlabel), Made4Net.Shared.Util.FormatField(_billingaccount), Made4Net.Shared.Util.FormatField(_promptforrcvqty))

            DataInterface.RunSQL(SQL)
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ConigneeCreated)
            activitytype = WMS.Lib.Actions.Audit.CONSIGNEEINS
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ConigneeCreated
            'em.Add("EVENT", EventType)
            'em.Add("CONSIGNEE", _consignee)
            'em.Add("USERID", pUser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
        End If

        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")

        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")

        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(activitytype)

    End Sub

    Public Sub CreateNew(ByVal pConsignee As String, ByVal pConsigneeName As String, ByVal pNotes As String, ByVal pReceivingLocation As String, _
    ByVal pReceivingWarehouseArea As String, ByVal pFlowThroughLoadLabel As String, ByVal pBillingAccount As String, _
    ByVal pPackMultipleOrders As Boolean, ByVal pContactID As String, ByVal pPromptForRcvQTY As Boolean, ByVal pUser As String)
        Dim SQL As String = ""

        'If (WMS.Logic.Consignee.Exists(pConsignee)) Then 
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception("Consignee Already Exists"), "Consignee Already Exists", "Consignee Already Exists")
        '    Throw m4nEx
        'End If


        _adduser = pUser
        _edituser = pUser
        _consignee = pConsignee
        _consigneename = pConsigneeName
        If pContactID <> String.Empty And Not pContactID Is Nothing Then
            _contactid = pContactID
        Else
            _contactid = Made4Net.Shared.Util.getNextCounter("CONTACTID")
        End If
        _notes = pNotes
        _receivingloc = pReceivingLocation
        _receivingwarehousearea = pReceivingWarehouseArea
        _flowthroughloadlabel = pFlowThroughLoadLabel
        _billingaccount = pBillingAccount
        _packmultipleorders = pPackMultipleOrders
        _promptforrcvqty = pPromptForRcvQTY
        validate(False)

        _adddate = DateTime.Now
        _editdate = DateTime.Now

        SQL = String.Format("Insert INTO CONSIGNEE(CONSIGNEE, CONSIGNEENAME, Notes, ContactID, ReceivingLoc, ReceivingWarehousearea, flowthroughloadlabel, billingaccount, packmultipleorders, ADDDATE, ADDUSER, EDITDATE, EDITUSER,PROMPTFORRCVQTY) values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13})", _
        Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_consigneename), Made4Net.Shared.Util.FormatField(_notes), _
        Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_receivingloc), Made4Net.Shared.Util.FormatField(_receivingwarehousearea), _
        Made4Net.Shared.Util.FormatField(_flowthroughloadlabel), Made4Net.Shared.Util.FormatField(_billingaccount), Made4Net.Shared.Util.FormatField(_packmultipleorders), _
        Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), _
        Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_promptforrcvqty))

        DataInterface.RunSQL(SQL)

        If Not Contact.Exists(_contactid) Then
            Dim contact As New WMS.Logic.Contact()
            contact.SetContact(_contactid, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, _
            Nothing, Nothing, Nothing, Nothing, Nothing, Common.GetCurrentUser())
        End If



        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String = WMS.Lib.Actions.Audit.CONSIGNEEINS
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ConigneeCreated)

        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")

        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")

        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")

        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(activitytype)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ConigneeCreated
        'em.Add("EVENT", EventType)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("USERID", pUser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))

    End Sub

    Public Sub Update(ByVal pConsigneeName As String, ByVal pNotes As String, ByVal pReceivingLocation As String, _
    ByVal pReceivingWarehouseArea As String, ByVal pFlowThroughLoadLabel As String, ByVal pBillingAccount As String, _
    ByVal pPackMultipleOrders As Boolean, ByVal pContactID As String, ByVal pPromptForRcvQty As Boolean, ByVal pUser As String)

        Dim sql As String
        _consigneename = pConsigneeName
        _notes = pNotes
        _receivingloc = pReceivingLocation
        _receivingwarehousearea = pReceivingWarehouseArea
        _flowthroughloadlabel = pFlowThroughLoadLabel
        _billingaccount = pBillingAccount
        _packmultipleorders = pPackMultipleOrders
        _edituser = pUser
        _contactid = pContactID
        _promptforrcvqty = pPromptForRcvQty
        validate(True)
        _editdate = DateTime.Now

        sql = String.Format("UPDATE CONSIGNEE SET CONSIGNEENAME={0},NOTES={1},RECEIVINGLOC={2},RECEIVINGWAREHOUSEAREA={3}, FLOWTHROUGHLOADLABEL={4}," & _
        "BILLINGACCOUNT={5},PACKMULTIPLEORDERS={6},EDITUSER={7},EDITDATE={8},ContactID={9},PromptForRcvQTY={10} WHERE {11}", Made4Net.Shared.Util.FormatField(_consigneename), _
        Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_receivingloc), Made4Net.Shared.Util.FormatField(_receivingwarehousearea), _
        Made4Net.Shared.Util.FormatField(_flowthroughloadlabel), Made4Net.Shared.Util.FormatField(_billingaccount), _
        Made4Net.Shared.Util.FormatField(_packmultipleorders), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), _
        Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_promptforrcvqty), WhereClause)

        DataInterface.RunSQL(sql)

    End Sub

    Public Shared Function GetCONSIGNEE(ByVal pCONSIGNEE As String) As Consignee
        Return New Consignee(pCONSIGNEE)
    End Function

    Public Sub LoadByContactId(ByVal pContactId As String)
        _contactid = pContactId
        _consignee = DataInterface.ExecuteScalar("Select consignee from consignee where contactid = '" & pContactId & "'")
        Try
            Load()
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Function Exists(ByVal pConsignee As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from CONSIGNEE where consignee = '{0}'", pConsignee)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM CONSIGNEE WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not load consignee, it does not exist", "Can not load consignee, it does not exist")
        End If
        dr = dt.Rows(0)

        If Not dr.IsNull("CONSIGNEENAME") Then _consigneename = dr.Item("CONSIGNEENAME")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("CREDIT") Then _credit = dr.Item("CREDIT")
        If Not dr.IsNull("CONTACTID") Then _contactid = dr.Item("CONTACTID")
        If Not dr.IsNull("LOADLABEL") Then _loadlabel = dr.Item("LOADLABEL")
        If Not dr.IsNull("FLOWTHROUGHLOADLABEL") Then _flowthroughloadlabel = dr.Item("FLOWTHROUGHLOADLABEL")
        If Not dr.IsNull("SKULABEL") Then _skulabel = dr.Item("SKULABEL")
        If Not dr.IsNull("LOADDETAILLBL") Then _loaddetaillbl = dr.Item("LOADDETAILLBL")
        If Not dr.IsNull("SHPCARTONLBL") Then _shpcartonlbl = dr.Item("SHPCARTONLBL")
        If Not dr.IsNull("SHPCONTAINERLBL") Then _shpcontainerlbl = dr.Item("SHPCONTAINERLBL")
        If Not dr.IsNull("RCVMANIFEST") Then _rcvmanifest = dr.Item("RCVMANIFEST")
        If Not dr.IsNull("PACKINGLIST") Then _packinglist = dr.Item("PACKINGLIST")
        If Not dr.IsNull("SHIPPINGMANIFEST") Then _shippingmanifest = dr.Item("SHIPPINGMANIFEST")
        If Not dr.IsNull("BILLINGPERFORMADOC") Then _billingperformadoc = dr.Item("BILLINGPERFORMADOC")
        If Not dr.IsNull("MIXSHIPPING") Then _mixshipping = dr.Item("MIXSHIPPING")
        If Not dr.IsNull("CYCLECOUNTING") Then _cyclecounting = dr.Item("CYCLECOUNTING")
        If Not dr.IsNull("REPLANPLANSHORT") Then _replanplanshort = dr.Item("REPLANPLANSHORT")
        If Not dr.IsNull("GENERATELOADID") Then _generateloadid = dr.Item("GENERATELOADID")
        If Not dr.IsNull("AUTOPRINTLOADLABELRCV") Then _autoprintloadlabelrcv = dr.Item("AUTOPRINTLOADLABELRCV")
        If Not dr.IsNull("CREDITLIMIT") Then _creditlimit = dr.Item("CREDITLIMIT")
        If Not dr.IsNull("CUBELIMIT") Then _cubelimit = dr.Item("CUBELIMIT")
        If Not dr.IsNull("SHIPPARTIALOAD") Then _shippartiaload = dr.Item("SHIPPARTIALOAD")
        If Not dr.IsNull("RECEIVINGLOC") Then _receivingloc = dr.Item("RECEIVINGLOC")
        If Not dr.IsNull("RECEIVINGWAREHOUSEAREA") Then _receivingwarehousearea = dr.Item("RECEIVINGWAREHOUSEAREA")
        If Not dr.IsNull("BILLINGACCOUNT") Then _billingaccount = dr.Item("BILLINGACCOUNT")
        If Not dr.IsNull("PACKMULTIPLEORDERS") Then _packmultipleorders = dr.Item("PACKMULTIPLEORDERS")
        If Not dr.IsNull("PROMPTFORRCVQTY") Then _promptforrcvqty = dr.Item("PROMPTFORRCVQTY")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _contact = New Contact(_contactid, ContactReference.ConsigneeContact)

    End Sub

    Public Shared Function AutoPrintLoadIdOnReceiving(ByVal pConsignee As String) As Boolean
        Dim sql As String = String.Format("Select AUTOPRINTLOADLABELRCV from CONSIGNEE where consignee = '{0}'", pConsignee)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function AutoGenerateLoadID(ByVal pConsignee As String) As Boolean
        Dim sql As String = String.Format("Select GENERATELOADID from CONSIGNEE where consignee = '{0}'", pConsignee)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Sub validate(ByVal pEdit As Boolean)
        If pEdit Then
            If Not Exists(_consignee) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Can not edit client. It does not exist", "Can not edit client. It does not exist")
            End If
            If Not WMS.Logic.Contact.Exists(_contactid) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Contact does not exist. Can only modify with existing contact.", "Contact does not exist. Can only modify with existing contact.")
            End If
        Else
            If Exists(_consignee) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Can not add client. Client already exists.", "Can not add client. Client already exists.")
            End If
        End If
        If ((_receivingloc <> String.Empty And Not (_receivingloc Is Nothing)) And (_receivingwarehousearea = String.Empty Or _receivingwarehousearea Is Nothing)) Or _
        ((_receivingwarehousearea <> String.Empty Or Not (_receivingwarehousearea Is Nothing)) And (_receivingloc = String.Empty And (_receivingloc Is Nothing))) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Receiving location\warehousearea is not valid.", "Receiving location\warehousearea is not valid.")
        End If

        If _receivingloc <> String.Empty And Not (_receivingloc Is Nothing) Then
            If _receivingwarehousearea = String.Empty Or _receivingwarehousearea Is Nothing Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Receiving location\warehousearea is not valid.", "Receiving location\warehousearea is not valid.")
            Else
                If Not Location.Exists(_receivingloc, _receivingwarehousearea) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Receiving location does not exist in selected warehousearea.", "Receiving location does not exist in selected warehousearea.")
                End If
            End If
        End If

        If _receivingwarehousearea <> String.Empty And Not (_receivingwarehousearea Is Nothing) Then
            If _receivingloc = String.Empty Or _receivingloc Is Nothing Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Receiving location\warehousearea is not valid.", "Receiving location\warehousearea is not valid.")
            Else
                If Not Location.Exists(_receivingloc, _receivingwarehousearea) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Receiving location does not exist in selected warehousearea.", "Receiving location does not exist in selected warehousearea.")
                End If
            End If
        End If
    End Sub

#End Region

End Class

#End Region

