Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared

#Region "Companys"

' <summary>
' This object represents the properties and methods of a COMPANY.
' </summary>

<CLSCompliant(False)> Public Class Company

#Region "Variables"

#Region "Primary Keys"

    Protected _consignee As String = String.Empty
    Protected _company As String = String.Empty
    Protected _companytype As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _companyname As String = String.Empty
    Protected _othercompany As String = String.Empty
    Protected _companygroup As String = String.Empty
    Protected _container As String = String.Empty
    Protected _status As Boolean
    Protected _mixpicking As Boolean
    Protected _deliverycomments As String = String.Empty
    Protected _deliverynotelayout As String = String.Empty
    Protected _defaultcontact As String = String.Empty
    Protected _servicetime As Int32
    Protected _allowasnrec As Boolean
    Protected _prefunloadingside As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    'Added for RWMS-1830
    Protected _minimumquantity As Decimal
    'Ended for RWMS-1830
    'Contacts collection
    Protected _defaultcontactObj As WMS.Logic.Contact
    Protected _contactsCollection As CompanyContactsCollection
    Protected _pickingcomments As String = String.Empty           'RWMS-3726

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE = '" & _consignee & "' And COMPANY = '" & _company & "' And companytype = '" & _companytype & "'"
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

    Public ReadOnly Property CONTACTS() As CompanyContactsCollection
        Get
            If _contactsCollection Is Nothing Then
                _contactsCollection = New CompanyContactsCollection(_consignee, _company, _companytype)
            End If
            Return _contactsCollection
        End Get
    End Property

    Public ReadOnly Property CONTACT(ByVal pContactId As String) As WMS.Logic.Contact
        Get
            Return New WMS.Logic.Contact(pContactId)
        End Get
    End Property

    Public ReadOnly Property DEFAULTCONTACTOBJ() As WMS.Logic.Contact
        Get
            If _defaultcontactObj Is Nothing Then
                _defaultcontactObj = New Contact(_defaultcontact)
            End If
            Return _defaultcontactObj
        End Get
    End Property

    Public Property COMPANYNAME() As String
        Get
            Return _companyname
        End Get
        Set(ByVal Value As String)
            _companyname = Value
        End Set
    End Property

    Public Property OTHERCOMPANY() As String
        Get
            Return _othercompany
        End Get
        Set(ByVal Value As String)
            _othercompany = Value
        End Set
    End Property

    Public Property COMPANYGROUP() As String
        Get
            Return _companygroup
        End Get
        Set(ByVal Value As String)
            _companygroup = Value
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

    Public Property MIXPICKING() As Boolean
        Get
            Return _mixpicking
        End Get
        Set(ByVal Value As Boolean)
            _mixpicking = Value
        End Set
    End Property

    Public Property ALLOWASNREC() As Boolean
        Get
            Return _allowasnrec
        End Get
        Set(ByVal Value As Boolean)
            _allowasnrec = Value
        End Set
    End Property

    Public Property CONTAINER() As String
        Get
            Return _container
        End Get
        Set(ByVal Value As String)
            _container = Value
        End Set
    End Property

    Public Property DEFAULTCONTACT() As String
        Get
            Return _defaultcontact
        End Get
        Set(ByVal Value As String)
            _defaultcontact = Value
        End Set
    End Property

    Public Property DELIVERYCOMMENTS() As String
        Get
            Return _deliverycomments
        End Get
        Set(ByVal Value As String)
            _deliverycomments = Value
        End Set
    End Property

    Public Property PICKINGCOMMENTS() As String
        Get
            Return _pickingcomments
        End Get
        Set(ByVal Value As String)
            _pickingcomments = Value
        End Set
    End Property

    Public Property DELIVERYNOTELAYOUT() As String
        Get
            Return _deliverynotelayout
        End Get
        Set(ByVal Value As String)
            _deliverynotelayout = Value
        End Set
    End Property
    'Added for RWMS-1830
    Public Property MINIMUMQUANTITY() As Decimal
        Get
            Return _minimumquantity
        End Get
        Set(ByVal Value As Decimal)
            _minimumquantity = Value
        End Set
    End Property
    'Ended for RWMS-1830
    Public Property SERVICETIME() As Int32
        Get
            Return _servicetime
        End Get
        Set(ByVal Value As Int32)
            _servicetime = Value
        End Set
    End Property

    Public Property PREFUNLOADINGSIDE() As String
        Get
            Return _prefunloadingside
        End Get
        Set(ByVal Value As String)
            _prefunloadingside = Value
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

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName.ToLower = "createcontact" Then
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            CreateContact(Convert.ReplaceDBNull(dr("contactid")), Convert.ReplaceDBNull(dr("street1")), Convert.ReplaceDBNull(dr("street2")), Convert.ReplaceDBNull(dr("city")), Convert.ReplaceDBNull(dr("state")), Convert.ReplaceDBNull(dr("zip")), Convert.ReplaceDBNull(dr("CONTACT1NAME")), _
                Convert.ReplaceDBNull(dr("CONTACT2NAME")), Convert.ReplaceDBNull(dr("CONTACT1PHONE")), Convert.ReplaceDBNull(dr("CONTACT2PHONE")), Convert.ReplaceDBNull(dr("CONTACT1FAX")), Convert.ReplaceDBNull(dr("CONTACT2FAX")), Convert.ReplaceDBNull(dr("CONTACT1EMAIL")), Convert.ReplaceDBNull(dr("CONTACT2EMAIL")), _
                Convert.ReplaceDBNull(dr("ROUTE")), Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), Convert.ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower() = "updatecontact" Then
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            UpdateContact(Convert.ReplaceDBNull(dr("contactid")), Convert.ReplaceDBNull(dr("street1")), Convert.ReplaceDBNull(dr("street2")), Convert.ReplaceDBNull(dr("city")), Convert.ReplaceDBNull(dr("state")), Convert.ReplaceDBNull(dr("zip")), Convert.ReplaceDBNull(dr("CONTACT1NAME")), _
                Convert.ReplaceDBNull(dr("CONTACT2NAME")), Convert.ReplaceDBNull(dr("CONTACT1PHONE")), Convert.ReplaceDBNull(dr("CONTACT2PHONE")), Convert.ReplaceDBNull(dr("CONTACT1FAX")), Convert.ReplaceDBNull(dr("CONTACT2FAX")), Convert.ReplaceDBNull(dr("CONTACT1EMAIL")), Convert.ReplaceDBNull(dr("CONTACT2EMAIL")), _
                Convert.ReplaceDBNull(dr("ROUTE")), Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), Convert.ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "setcontact" Then
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            SetContact(Convert.ReplaceDBNull(dr("contactid")), Convert.ReplaceDBNull(dr("street1")), Convert.ReplaceDBNull(dr("street2")), Convert.ReplaceDBNull(dr("city")), Convert.ReplaceDBNull(dr("state")), Convert.ReplaceDBNull(dr("zip")), Convert.ReplaceDBNull(dr("CONTACT1NAME")), _
                Convert.ReplaceDBNull(dr("CONTACT2NAME")), Convert.ReplaceDBNull(dr("CONTACT1PHONE")), Convert.ReplaceDBNull(dr("CONTACT2PHONE")), Convert.ReplaceDBNull(dr("CONTACT1FAX")), Convert.ReplaceDBNull(dr("CONTACT2FAX")), Convert.ReplaceDBNull(dr("CONTACT1EMAIL")), Convert.ReplaceDBNull(dr("CONTACT2EMAIL")), _
                Convert.ReplaceDBNull(dr("ROUTE")), Convert.ReplaceDBNull(dr("STAGINGLANE")), Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA")), Convert.ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), Convert.ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "removecontact" Then
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            DeleteContact(Convert.ReplaceDBNull(dr("contactid")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "setdefaultcontact" Then
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            SetDefaultContact(Convert.ReplaceDBNull(dr("contactid")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "setdelivery" Then
            If Not WMS.Logic.Contact.Exists(ds.Tables(0).Rows(0)("contact")) Then
                Throw New M4NException(New Exception, "Can not create delivery. Contact does not exist.", "Can not create delievery. Contact does not exist.")
            End If
            Dim _contactobj As Contact = New Contact(ds.Tables(0).Rows(0)("contact"))
            _contactobj.SetServiceTime(ds.Tables(0).Rows(0), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "newcompany" Then
            CreateNew(dr("consignee"), dr("company"), dr("companytype"), Convert.ReplaceDBNull(dr("companyname")), Convert.ReplaceDBNull(dr("othercompany")), Convert.ReplaceDBNull(dr("companygroup")), Convert.ReplaceDBNull(dr("status")), Convert.ReplaceDBNull(dr("mixpicking")), Convert.ReplaceDBNull(dr("deliverycomments")), Convert.ReplaceDBNull(dr("pickingcomments")), Convert.ReplaceDBNull(dr("deliverynotelayout")), Convert.ReplaceDBNull(dr("prefunloadingside")), Convert.ReplaceDBNull(dr("ALLOWASNREC")), Convert.ReplaceDBNull(dr("largequantity")), Convert.ReplaceDBNull(dr("container")), Convert.ReplaceDBNull(dr("servicetime")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "editcompany" Then
            dr = ds.Tables(0).Rows(0)
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            EditComp(Convert.ReplaceDBNull(dr("companyname")), Convert.ReplaceDBNull(dr("othercompany")), Convert.ReplaceDBNull(dr("container")), Convert.ReplaceDBNull(dr("companygroup")), Convert.ReplaceDBNull(dr("status")), Convert.ReplaceDBNull(dr("mixpicking")), Convert.ReplaceDBNull(dr("deliverycomments")), Convert.ReplaceDBNull(dr("pickingcomments")), Convert.ReplaceDBNull(dr("deliverynotelayout")), Convert.ReplaceDBNull(dr("prefunloadingside")), Convert.ReplaceDBNull(dr("ALLOWASNREC")), Convert.ReplaceDBNull(dr("largequantity")), Convert.ReplaceDBNull(dr("servicetime")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "pin" Then
            dr = ds.Tables(0).Rows(0)
            _consignee = ds.Tables(0).Rows(0)("consignee")
            _company = ds.Tables(0).Rows(0)("company")
            _companytype = ds.Tables(0).Rows(0)("companytype")
            Load()
            'Pin(Convert.ReplaceDBNull(dr("NEWPOINTID")), Common.GetCurrentUser())
        End If
    End Sub

    Public Sub New()

    End Sub

    Public Sub New(ByVal pCONSIGNEE As String, ByVal pCOMPANY As String, ByVal pCompanyType As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pCONSIGNEE
        _company = pCOMPANY
        _companytype = pCompanyType
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Pin"

    Public Function Pin(ByVal pContactId As String, ByVal PointID As String, ByVal pUser As String)
        Me.CONTACT(pContactId).setPointId(PointID, pUser)
    End Function

    Public Function Pin(ByVal pContactId As String, ByVal pLongitude As Double, ByVal pLatitude As Double, ByVal pUser As String)
        Dim oPoint As New Made4Net.GeoData.GeoPoint
        oPoint.LONGITUDE = pLongitude
        oPoint.LATITUDE = pLatitude
        oPoint.Create(pUser)
        Me.CONTACT(pContactId).setPointId(oPoint.POINTID, pUser)
    End Function

#End Region

#Region "Accessors"

    Public Shared Function GetCOMPANY(ByVal pCONSIGNEE As String, ByVal pCOMPANY As String, ByVal pCompanyType As String) As Company
        Return New Company(pCONSIGNEE, pCOMPANY, pCompanyType)
    End Function

    Public Shared Function Exists(ByVal pCONSIGNEE As String, ByVal pCOMPANY As String, ByVal pCompanyType As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from COMPANY where Consignee = '{0}' and Company = '{1}' and CompanyType = '{2}'", pCONSIGNEE, pCOMPANY, pCompanyType)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub LoadByContactId(ByVal pContactId As String)
        '_contactid = pContactId
        Dim dt As New DataTable
        DataInterface.FillDataset("Select company,consignee,companytype from company where contactid = '" & pContactId & "'", dt)
        Try
            _consignee = dt.Rows(0)("CONSIGNEE")
            _company = dt.Rows(0)("COMPANY")
            _companytype = dt.Rows(0)("COMPANYTYPE")
            Load()
        Catch ex As Exception
        End Try
    End Sub

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM COMPANY Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Company does not exists", "Company does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        setObj(dr)
    End Sub

    Private Sub setObj(ByVal dr As DataRow)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("COMPANY") Then _company = dr.Item("COMPANY")
        If Not dr.IsNull("COMPANYTYPE") Then _companytype = dr.Item("COMPANYTYPE")
        If Not dr.IsNull("COMPANYNAME") Then _companyname = dr.Item("COMPANYNAME")
        If Not dr.IsNull("OTHERCOMPANY") Then _othercompany = dr.Item("OTHERCOMPANY")
        If Not dr.IsNull("COMPANYGROUP") Then _companygroup = dr.Item("COMPANYGROUP")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("MIXPICKING") Then _mixpicking = dr.Item("MIXPICKING")
        If Not dr.IsNull("CONTAINER") Then _container = dr.Item("CONTAINER")
        If Not dr.IsNull("DELIVERYCOMMENTS") Then _deliverycomments = dr.Item("DELIVERYCOMMENTS")
        If Not dr.IsNull("DELIVERYNOTELAYOUT") Then _deliverynotelayout = dr.Item("DELIVERYNOTELAYOUT")
        If Not dr.IsNull("SERVICETIME") Then _servicetime = dr.Item("SERVICETIME")
        If Not dr.IsNull("DEFAULTCONTACT") Then _defaultcontact = dr.Item("DEFAULTCONTACT")
        If Not dr.IsNull("PREFUNLOADINGSIDE") Then _prefunloadingside = dr.Item("PREFUNLOADINGSIDE")
        If Not dr.IsNull("ALLOWASNREC") Then _allowasnrec = dr.Item("ALLOWASNREC")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("PICKINGCOMMENTS") Then _pickingcomments = dr.Item("PICKINGCOMMENTS")   'RWMS-3726

        If _contactsCollection Is Nothing Then
            _contactsCollection = New CompanyContactsCollection(_consignee, _company, _companytype)
        End If
    End Sub
#End Region

#Region "Insert/Update"

    Public Sub Save()
        Dim SQL As String

        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String

        If (WMS.Logic.Company.Exists(_consignee, _company, _companytype)) Then
            SQL = String.Format("update company set COMPANYNAME = {0} ,OTHERCOMPANY = {1} , COMPANYGROUP = {2} , STATUS = {3}  ,MIXPICKING  = {4}, DELIVERYCOMMENTS = {5} " &
              " ,DELIVERYNOTELAYOUT={10}, PREFUNLOADINGSIDE={11}, CONTAINER={12} ,SERVICETIME={13}, DEFAULTCONTACT = {14}, ALLOWASNREC={15} ,LARGEQUANTITY={16},EDITDATE = {6}  ,EDITUSER = {7}, PICKINGCOMMENTS = {8} where {9}",
            Made4Net.Shared.Util.FormatField(_companyname), Made4Net.Shared.Util.FormatField(_othercompany),
            Made4Net.Shared.Util.FormatField(_companygroup), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_mixpicking), Made4Net.Shared.Util.FormatField(_deliverycomments),
            Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_pickingcomments), WhereClause, Made4Net.Shared.Util.FormatField(_deliverynotelayout), Made4Net.Shared.Util.FormatField(_prefunloadingside), Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_servicetime), Made4Net.Shared.Util.FormatField(_defaultcontact), Made4Net.Shared.Util.FormatField(_allowasnrec), Made4Net.Shared.Util.FormatField(_minimumquantity))

            DataInterface.RunSQL(SQL)
            activitytype = WMS.Lib.Actions.Audit.COMPANYUDP
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CompanyUpdated)
        Else
            '_defaultcontact = Made4Net.Shared.Util.getNextCounter("CONTACTID")
            SQL = "insert into company (CONSIGNEE, COMPANYTYPE ,COMPANY, COMPANYNAME ,OTHERCOMPANY, COMPANYGROUP, STATUS ,MIXPICKING " &
              " , ADDDATE, ADDUSER ,EDITDATE ,EDITUSER, DELIVERYCOMMENTS, PICKINGCOMMENTS, DELIVERYNOTELAYOUT, DEFAULTCONTACT, PREFUNLOADINGSIDE, CONTAINER, SERVICETIME, ALLOWASNREC,LARGEQUANTITY  ) values( "
            SQL += Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_companytype) & "," &
                Made4Net.Shared.Util.FormatField(_company) & "," & Made4Net.Shared.Util.FormatField(_companyname) & "," & Made4Net.Shared.Util.FormatField(_othercompany) & "," &
                Made4Net.Shared.Util.FormatField(_companygroup) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_mixpicking) & "," &
                Made4Net.Shared.Util.FormatField(DateTime.Now) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & Made4Net.Shared.Util.FormatField(DateTime.Now) & "," &
                Made4Net.Shared.Util.FormatField(_edituser) & "," & Made4Net.Shared.Util.FormatField(_deliverycomments) & "," & Made4Net.Shared.Util.FormatField(_pickingcomments) & "," & Made4Net.Shared.Util.FormatField(_deliverynotelayout) & "," & Made4Net.Shared.Util.FormatField(_defaultcontact) & "," & Made4Net.Shared.Util.FormatField(_prefunloadingside) & "," & Made4Net.Shared.Util.FormatField(_container) & "," & Made4Net.Shared.Util.FormatField(_servicetime) & "," & Made4Net.Shared.Util.FormatField(_allowasnrec) & "," & Made4Net.Shared.Util.FormatField(_minimumquantity) & ")"

            DataInterface.RunSQL(SQL)

            activitytype = WMS.Lib.Actions.Audit.COMPANYINS
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CompanyCreated)
        End If

        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _company)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", _companytype)
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Add("PICKINGCOMMENTS", _pickingcomments)   'RWMS-3726
        aq.Send(activitytype)

        Load()
    End Sub

    Public Sub CreateNew(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanytype As String, ByVal pCompanyname As String, ByVal pOthercompany As String, ByVal pCompanygroup As String, ByVal pStatus As String, ByVal pMixpicking As Boolean, ByVal pDeliveryComments As String, ByVal _pPickingComments As String, ByVal pdeliverynotelayout As String, ByVal pPrefUnloadingSide As String, ByVal pAllowAsnReceiving As Boolean, ByVal pminimumquantity As Decimal, ByVal pcontainer As String, ByVal pservicetime As Integer, ByVal pUser As String)
        Dim SQL As String = ""

        If (WMS.Logic.Company.Exists(pConsignee, pCompany, pCompanytype)) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception("Company Already Exists"), "Company Already Exists", "Company Already Exists")
            Throw m4nEx
        End If

        _consignee = pConsignee
        _company = pCompany
        _companytype = pCompanytype
        _othercompany = pOthercompany
        _companyname = pCompanyname
        _companygroup = pCompanygroup
        _status = pStatus
        _mixpicking = pMixpicking
        _prefunloadingside = pPrefUnloadingSide
        _deliverycomments = pDeliveryComments
        _pickingcomments = _pPickingComments          'RWMS-3726
        _deliverynotelayout = pdeliverynotelayout
        _defaultcontact = Made4Net.Shared.Util.getNextCounter("CONTACTID")
        _allowasnrec = pAllowAsnReceiving
        'Added for RWMS-1830
        _minimumquantity = pminimumquantity
        _container = pcontainer
        _servicetime = pservicetime
        'Ended for RWMS-1830
        SQL = "insert into company (CONSIGNEE, COMPANYTYPE ,COMPANY, COMPANYNAME ,OTHERCOMPANY, COMPANYGROUP, STATUS ,MIXPICKING " &
              " , PREFUNLOADINGSIDE, ADDDATE, ADDUSER ,EDITDATE ,EDITUSER,DELIVERYCOMMENTS, PICKINGCOMMENTS, ALLOWASNREC,DELIVERYNOTELAYOUT,LARGEQUANTITY,CONTAINER,SERVICETIME) values( "
        SQL += Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_companytype) & "," &
            Made4Net.Shared.Util.FormatField(_company) & "," & Made4Net.Shared.Util.FormatField(_companyname) & "," & Made4Net.Shared.Util.FormatField(_othercompany) & "," &
            Made4Net.Shared.Util.FormatField(_companygroup) & "," & Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_mixpicking) & "," &
            Made4Net.Shared.Util.FormatField(_prefunloadingside) & "," & Made4Net.Shared.Util.FormatField(DateTime.Now) & "," & Made4Net.Shared.Util.FormatField(pUser) & "," & Made4Net.Shared.Util.FormatField(DateTime.Now) & "," &
            Made4Net.Shared.Util.FormatField(pUser) & "," & Made4Net.Shared.Util.FormatField(_deliverycomments) & "," & Made4Net.Shared.Util.FormatField(_pickingcomments) & "," & Made4Net.Shared.Util.FormatField(_allowasnrec) & "," & Made4Net.Shared.Util.FormatField(_deliverynotelayout) & "," & Made4Net.Shared.Util.FormatField(_minimumquantity) & "," & Made4Net.Shared.Util.FormatField(_container) & "," & Made4Net.Shared.Util.FormatField(_servicetime) & ")"

        DataInterface.RunSQL(SQL)

        'And create new default blank contact 
        SetContact(_defaultcontact, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", _adduser)

        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String
        activitytype = WMS.Lib.Actions.Audit.COMPANYINS
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CompanyCreated)
        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _company)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", _companytype)
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Add("PICKINGCOMMENTS", _pickingcomments)   'RWMS-3726
        aq.Send(activitytype)

        Load()
    End Sub

    Public Sub EditComp(ByVal pCompanyname As String, ByVal pOtherCompany As String, ByVal pContainer As String,
        ByVal pCompanygroup As String, ByVal pStatus As Boolean, ByVal pMixpicking As Boolean,
        ByVal pDeliveryComments As String, ByVal pPickingComments As String, ByVal pdeliverynotelayout As String, ByVal pPrefUnloadingSide As String, ByVal pAllowAsnReceiving As Boolean, ByVal pminimumquantity As Decimal, ByVal pservicetime As Integer, ByVal pUser As String)

        Dim SQL As String = ""
        _companyname = pCompanyname
        _othercompany = pOtherCompany
        _companygroup = pCompanygroup
        _status = pStatus
        _container = pContainer
        _mixpicking = pMixpicking
        _deliverycomments = pDeliveryComments
        _pickingcomments = pPickingComments
        _deliverynotelayout = pdeliverynotelayout
        _prefunloadingside = pPrefUnloadingSide
        _allowasnrec = pAllowAsnReceiving
        'Added for RWMS-1830
        _minimumquantity = pminimumquantity
        _container = pContainer
        _servicetime = pservicetime
        'Ended for RWMS-1830
        SQL = String.Format("update company set COMPANYNAME = {0} ,OTHERCOMPANY = {1} , COMPANYGROUP = {2} , STATUS = {3}  ,MIXPICKING  = {4} " &
              " ,DELIVERYNOTELAYOUT={10}, CONTAINER = {11}, DELIVERYCOMMENTS = {5}, EDITDATE = {6}  ,EDITUSER = {7}, PREFUNLOADINGSIDE={12}, ALLOWASNREC={13},LARGEQUANTITY={14},SERVICETIME={15},PICKINGCOMMENTS={8} where {9}",
            Made4Net.Shared.Util.FormatField(_companyname), Made4Net.Shared.Util.FormatField(_othercompany),
            Made4Net.Shared.Util.FormatField(_companygroup), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_mixpicking),
            Made4Net.Shared.Util.FormatField(_deliverycomments), Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(_pickingcomments), WhereClause, Made4Net.Shared.Util.FormatField(_deliverynotelayout), Made4Net.Shared.Util.FormatField(_container), Made4Net.Shared.Util.FormatField(_prefunloadingside), Made4Net.Shared.Util.FormatField(_allowasnrec), Made4Net.Shared.Util.FormatField(_minimumquantity), Made4Net.Shared.Util.FormatField(_servicetime))

        DataInterface.RunSQL(SQL)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.CompanyUpdated
        'em.Add("EVENT", EventType)
        'em.Add("COMPANY", _company)
        'em.Add("COMPANYTYPE", _companytype)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("USERID", pUser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))

        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String
        activitytype = WMS.Lib.Actions.Audit.COMPANYUDP
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CompanyUpdated)

        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _company)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", _companytype)
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", _edituser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", _edituser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", _edituser)
        aq.Add("PICKINGCOMMENTS", _pickingcomments)        'RWMS-3726
        aq.Send(activitytype)

    End Sub

    Private Sub validate(ByVal pEdit As Boolean)
        If pEdit Then
            If Not Exists(_consignee, _company, _companytype) Then
                Throw New M4NException(New Exception, "Could not edit company - It does not exist", "Could not edit company - It does not exist")
            End If

            If Not _contactsCollection.Contains(_defaultcontact) Then
                Throw New M4NException(New Exception, "Could not edit company - Default contact does not exist", "Could not edit company - Default contact does not exist")
            End If
        Else
            If Not WMS.Logic.Consignee.Exists(_consignee) Then
                Throw New M4NException(New Exception, "Could not add company. Client does not exist", "Could not add company. Client does not exist")
            End If
            If Exists(_consignee, _company, _companytype) Then
                Throw New M4NException(New Exception, "Could not insert company - It already exists", "Could not insert company - It already exists")
            End If
        End If

        If _container <> String.Empty And Not WMS.Logic.Container.Exists(_container) Then
            Throw New M4NException(New Exception, "Could not edit company - Container does not exist", "Could not edit company - Container does not exist")
        End If
    End Sub

#End Region

#Region "Contacts"

    Public Function SetContact(ByVal pContactID As String, ByVal pStreet1 As String, ByVal pStreet2 As String, ByVal pCity As String, ByVal pState As String, _
            ByVal pZip As String, ByVal pName1 As String, ByVal pName2 As String, ByVal pPhone1 As String, ByVal pPhone2 As String, ByVal pFax1 As String, _
            ByVal pFax2 As String, ByVal pEMail1 As String, ByVal pEMail2 As String, ByVal pRoute As String, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, ByVal pPickupConfirmationType As String, ByVal pDeliveryConfirmationType As String, ByVal pUser As String) As String

        Dim oContact As New WMS.Logic.Contact
        oContact.SetContact(pContactID, pStreet1, pStreet2, pCity, pState, pZip, pName1, pName2, pPhone1, pPhone2, pFax1, pFax2, pEMail1, pEMail2, pRoute, pStaginglane, pStagingwarehousearea, pPickupConfirmationType, pDeliveryConfirmationType, pUser)
        If Me.CONTACTS.Count = 0 Then
            SetDefaultContact(oContact.CONTACTID, pUser)
        End If
        Me.CONTACTS.AddContact(oContact.CONTACTID, pUser)
    End Function

    Public Sub CreateContact(ByVal pContactID As String, ByVal pStreet1 As String, ByVal pStreet2 As String, ByVal pCity As String, ByVal pState As String, _
            ByVal pZip As String, ByVal pName1 As String, ByVal pName2 As String, ByVal pPhone1 As String, ByVal pPhone2 As String, ByVal pFax1 As String, _
            ByVal pFax2 As String, ByVal pEMail1 As String, ByVal pEMail2 As String, ByVal pRoute As String, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, ByVal pPickupConfirmationType As String, ByVal pDeliveryConfirmationType As String, ByVal pUser As String)
        If WMS.Logic.Contact.Exists(pContactID) Then
            Throw New M4NException(New Exception, "Cannot create contact. Contact with same id already exists in system.", "Cannot create contact. Contact with same id already exists in system.")
        End If
        SetContact(pContactID, pStreet1, pStreet2, pCity, pState, pZip, pName1, pName2, pPhone1, pPhone2, pFax1, pFax2, pEMail1, pEMail2, pRoute, pStaginglane, _
        pStagingwarehousearea, pPickupConfirmationType, pDeliveryConfirmationType, pUser)

    End Sub

    Public Sub UpdateContact(ByVal pContactID As String, ByVal pStreet1 As String, ByVal pStreet2 As String, ByVal pCity As String, ByVal pState As String, _
            ByVal pZip As String, ByVal pName1 As String, ByVal pName2 As String, ByVal pPhone1 As String, ByVal pPhone2 As String, ByVal pFax1 As String, _
            ByVal pFax2 As String, ByVal pEMail1 As String, ByVal pEMail2 As String, ByVal pRoute As String, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, ByVal pPickupConfirmationType As String, ByVal pDeliveryConfirmationType As String, ByVal pUser As String)
        If Not WMS.Logic.Contact.Exists(pContactID) Then
            Throw New M4NException(New Exception, "Can not edit contact, contact does not exist.", "Can not edit contact, contact does not exist.")
        End If
        SetContact(pContactID, pStreet1, pStreet2, pCity, pState, pZip, pName1, pName2, pPhone1, pPhone2, pFax1, pFax2, pEMail1, pEMail2, pRoute, pStaginglane, _
pStagingwarehousearea, pPickupConfirmationType, pDeliveryConfirmationType, pUser)
    End Sub

    Public Function AddContact(ByVal pContactID As String, ByVal pUser As String) As String
        Dim oContact As WMS.Logic.Contact
        If WMS.Logic.Contact.Exists(pContactID) Then
            If Me.CONTACTS.Count = 0 Then
                SetDefaultContact(pContactID, pUser)
            End If
            Me.CONTACTS.AddContact(pContactID, pUser)
        End If
    End Function

    Public Sub DeleteContact(ByVal pContactID As String, ByVal pUser As String)
        Dim oContact As WMS.Logic.Contact
        If WMS.Logic.Contact.Exists(pContactID) Then
            oContact = New WMS.Logic.Contact(pContactID)
            oContact.Delete()
            Me.CONTACTS.DeleteContact(pContactID, pUser)
            If _defaultcontact = pContactID Then
                DeleteDefaultContact(pUser)
            End If
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot delete contact - Contact does not exists", "Cannot delete contact - Contact does not exists")
        End If
    End Sub

    Public Sub RemoveContact(ByVal pContactID As String, ByVal pUser As String)
        Dim oContact As WMS.Logic.Contact
        If WMS.Logic.Contact.Exists(pContactID) Then
            Me.CONTACTS.DeleteContact(pContactID, pUser)
            If _defaultcontact = pContactID Then
                DeleteDefaultContact(pUser)
            End If
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot delete contact - Contact does not exists", "Cannot delete contact - Contact does not exists")
        End If
    End Sub

    Public Sub SetDefaultContact(ByVal pContactID As String, ByVal pUser As String)
        If WMS.Logic.Contact.Exists(pContactID) Then
            _editdate = DateTime.Now
            _edituser = pUser
            _defaultcontact = pContactID
            Dim SQL As String = String.Format("update company set defaultcontact = {0},editdate={1}, edituser={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_defaultcontact), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
            DataInterface.RunSQL(SQL)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot set contact as default- Contact does not exists", "Cannot set contact as default- Contact does not exists")
        End If
    End Sub

    Public Sub DeleteDefaultContact(ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser
        _defaultcontact = ""
        Dim SQL As String = String.Format("update company set defaultcontact = {0},editdate={1}, edituser={2} where {3}", _
            Made4Net.Shared.Util.FormatField(_defaultcontact), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Company Delivery Hours"

    Public Sub SetServiceTime(ByVal pContact As String, ByVal pPriority As Int32, ByVal pSunOpen As Int32, ByVal pSunClose As Int32, _
                ByVal pMonOpen As Int32, ByVal pMonClose As Int32, ByVal pTueOpen As Int32, ByVal pTueClose As Int32, _
                ByVal pWedOpen As Int32, ByVal pWedClose As Int32, ByVal pThuOpen As Int32, ByVal pThuClose As Int32, _
                 ByVal pFriOpen As Int32, ByVal pFriClose As Int32, ByVal pSatOpen As Int32, ByVal pSatClose As Int32, ByVal pUser As String)

        Dim oContact As New WMS.Logic.Contact(pContact)
        oContact.SetServiceTime(pContact, pPriority, pSunOpen, pSunClose, pMonOpen, pMonClose, pTueOpen, pTueClose, _
                pWedOpen, pWedClose, pThuOpen, pThuClose, pFriOpen, pFriClose, pSatOpen, pSatClose, pUser)
    End Sub
    Public Function DifTime(ByVal itotalTime As Double, ByVal pStartTime As Integer) As Integer
        Dim min, res As Integer
        min = Math.Floor(itotalTime / 60) + Math.Floor(pStartTime / 100) * 60 + Math.Floor(pStartTime Mod 100)
        res = Math.Floor(min / 60) * 100 + Math.Floor(min Mod 60)
        Return res
    End Function

    <Obsolete()> _
    Public Function CanDeliver(ByVal DeliveryDate As DateTime, ByVal EstArrivalHour As Int32, ByVal pDefaultOpenHour As Int32, ByVal pDefaultCloseHour As Int32, _
                               ByVal AllowedTimeBeforeOpen As Integer, _
                               ByRef OpenHour As Integer, ByRef CloseHour As Integer) As Boolean
        Dim cst As CompanyServiceTime

        EstArrivalHour = DifTime(AllowedTimeBeforeOpen, EstArrivalHour)


        If Me.DEFAULTCONTACTOBJ.ServiceTimeCollection.Count = 0 Then
            OpenHour = pDefaultOpenHour
            CloseHour = pDefaultCloseHour
            If EstArrivalHour >= pDefaultOpenHour And EstArrivalHour <= pDefaultCloseHour Then
                Return True
            End If
        End If
        For Each cst In Me.DEFAULTCONTACTOBJ.ServiceTimeCollection
            Select Case DeliveryDate.DayOfWeek
                Case DayOfWeek.Sunday
                    OpenHour = cst.SunOpenHour
                    CloseHour = cst.SunCloseHour
                    If cst.SunOpenHour <= EstArrivalHour And cst.SunCloseHour >= EstArrivalHour Then
                        Return True
                    End If
                Case DayOfWeek.Monday
                    OpenHour = cst.MonOpenHour
                    CloseHour = cst.MonCloseHour
                    If cst.MonOpenHour <= EstArrivalHour And cst.MonCloseHour >= EstArrivalHour Then
                        Return True
                    End If
                Case DayOfWeek.Tuesday
                    OpenHour = cst.TueOpenHour
                    CloseHour = cst.TueCloseHour
                    If cst.TueOpenHour <= EstArrivalHour And cst.TueCloseHour >= EstArrivalHour Then
                        Return True
                    End If
                Case DayOfWeek.Wednesday
                    OpenHour = cst.WedOpenHour
                    CloseHour = cst.WedCloseHour
                    If cst.WedOpenHour <= EstArrivalHour And cst.WedCloseHour >= EstArrivalHour Then
                        Return True
                    End If
                Case DayOfWeek.Thursday
                    OpenHour = cst.ThuOpenHour
                    CloseHour = cst.ThuCloseHour
                    If cst.ThuOpenHour <= EstArrivalHour And cst.ThuCloseHour >= EstArrivalHour Then
                        Return True
                    End If
                Case DayOfWeek.Friday
                    OpenHour = cst.FriOpenHour
                    CloseHour = cst.FriCloseHour
                    If cst.FriOpenHour <= EstArrivalHour And cst.FriCloseHour >= EstArrivalHour Then
                        Return True
                    End If
                Case DayOfWeek.Saturday
                    OpenHour = cst.SatOpenHour
                    CloseHour = cst.SatCloseHour
                    If cst.SatOpenHour <= EstArrivalHour And cst.SatCloseHour >= EstArrivalHour Then
                        Return True
                    End If
            End Select
        Next
        Return False
    End Function

    <Obsolete()> _
    Public Function GetWaitingTime(ByVal DeliveryDate As DateTime, ByVal EstArrivalHour As Int32, ByVal pDefaultOpenHour As Int32) As Integer
        Dim res As Integer
        If Me.DEFAULTCONTACTOBJ.ServiceTimeCollection.Count = 0 Then
            res = Math.Max(EstArrivalHour, pDefaultOpenHour)
            Return res
        End If
        Dim cst As CompanyServiceTime
        For Each cst In Me.DEFAULTCONTACTOBJ.ServiceTimeCollection
            Select Case DeliveryDate.DayOfWeek
                Case DayOfWeek.Sunday
                    res = Math.Max(EstArrivalHour, cst.SunOpenHour)
                    Return res
                Case DayOfWeek.Monday
                    res = Math.Max(EstArrivalHour, cst.MonOpenHour)
                    Return res
                Case DayOfWeek.Tuesday
                    res = Math.Max(EstArrivalHour, cst.TueOpenHour)
                    Return res
                Case DayOfWeek.Wednesday
                    res = Math.Max(EstArrivalHour, cst.WedOpenHour)
                    Return res
                Case DayOfWeek.Thursday
                    res = Math.Max(EstArrivalHour, cst.ThuOpenHour)
                    Return res
                Case DayOfWeek.Friday
                    res = Math.Max(EstArrivalHour, cst.FriOpenHour)
                    Return res
                Case DayOfWeek.Saturday
                    res = Math.Max(EstArrivalHour, cst.SatOpenHour)
                    Return res
            End Select
        Next
        Return False

    End Function

    

#End Region

#End Region

End Class

#End Region

#Region "Company Contact"

<CLSCompliant(False)> Public Class CompanyContact

#Region "Variables"

    Protected _consignee As String = String.Empty
    Protected _company As String = String.Empty
    Protected _companytype As String = String.Empty
    Protected _contactid As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONSIGNEE='" & _consignee & "' AND COMPANY='" & _company & "' AND COMPANYTYPE ='" & _companytype & "' AND CONTACTID = '" & _contactid & "'"
        End Get
    End Property

    Public Property CONTACTID() As String
        Get
            Return _contactid
        End Get
        Set(ByVal Value As String)
            _contactid = Value
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

    Public Sub New(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pContactId As String, Optional ByVal LoadObj As Boolean = True)
        _consignee = pConsignee
        _company = pCompany
        _companytype = pCompanyType
        _contactid = pContactId
        Load()
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetCompanyContact(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pContactId As String) As CompanyContact
        Return New CompanyContact(pConsignee, pCompany, pCompanyType, pContactId)
    End Function

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pContactId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from COMPANYCONTACTS where Consignee = '{0}' and Company = '{1}' and CompanyType = '{2}' And ContactId = '{3}'", pConsignee, pCompany, pCompanyType, pContactId)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM COMPANYCONTACTS Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Company's Contact does not exists", "Contact does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        setObj(dr)
    End Sub

    Private Sub setObj(ByVal dr As DataRow)
        _consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE"))
        _company = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("COMPANY"))
        _companytype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("COMPANYTYPE"))
        _contactid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ContactId"))
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

#End Region

#Region "Save"

    Public Sub CreateCompanyContact(ByVal pUser As String)
        Dim sql As String
        If Not CompanyContact.Exists(_consignee, _company, _companytype, _contactid) Then
            _editdate = DateTime.Now
            _edituser = pUser
            _adddate = DateTime.Now
            _adduser = pUser
            sql = String.Format("INSERT INTO COMPANYCONTACTS(CONSIGNEE, COMPANY, COMPANYTYPE, CONTACTID, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7}) ", _
                Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_adddate), _
                Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
            DataInterface.RunSQL(sql)
        End If
    End Sub

    Public Sub RemoveCompanyContact(ByVal pUser As String)
        Dim sql As String
        sql = String.Format("DELETE FROM COMPANYCONTACTS where {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region
    
#End Region

End Class

#End Region

#Region "Comapny Contacts Collection"

<CLSCompliant(False)> Public Class CompanyContactsCollection
    Inherits ArrayList

#Region "Variables"

    Protected _consignee As String
    Protected _company As String
    Protected _companytype As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As CompanyContact
        Get
            Return CType(MyBase.Item(index), CompanyContact)
        End Get
    End Property

    Public ReadOnly Property Contact(ByVal pContactId As String) As CompanyContact
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).CONTACTID = pContactId Then
                    Return (CType(MyBase.Item(i), CompanyContact))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pConsignee As String, ByVal pCompany As String, ByVal pCompanyType As String, Optional ByVal LoadAll As Boolean = True)
        _consignee = pConsignee
        _company = pCompany
        _companytype = pCompanyType
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = String.Format("Select contactid from COMPANYCONTACTS where consignee ='{0}' and company='{1}' and companytype = '{2}'", _consignee, _company, _companytype)
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New CompanyContact(_consignee, _company, _companytype, dr.Item("contactid"), LoadAll))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As CompanyContact) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As CompanyContact)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As CompanyContact)
        MyBase.Remove(pObject)
    End Sub

    Public Sub AddContact(ByVal pContactId As String, ByVal pUser As String)
        Dim oContact As CompanyContact = Contact(pContactId)
        If IsNothing(oContact) Then
            oContact = New CompanyContact()
            oContact.COMPANY = _company
            oContact.COMPANYTYPE = _companytype
            oContact.CONSIGNEE = _consignee
            oContact.CONTACTID = pContactId
            oContact.CreateCompanyContact(pUser)
            Me.add(oContact)
        End If
    End Sub

    Public Sub DeleteContact(ByVal pContactId As String, ByVal pUser As String)
        Dim oContact As CompanyContact = Contact(pContactId)
        If Not IsNothing(oContact) Then
            oContact.RemoveCompanyContact(pUser)
            Me.Remove(oContact)
        End If
    End Sub

#End Region

End Class

#End Region


