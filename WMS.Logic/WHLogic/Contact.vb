Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "CONTACT"

' <summary>
' This object represents the properties and methods of a CONTACT.
' </summary>

<CLSCompliant(False)> Public Class Contact

#Region "Variables"

#Region "Primary Keys"

    Protected _contactid As String = String.Empty
    'Protected _contacttype As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _street1 As String = String.Empty
    Protected _street2 As String = String.Empty
    Protected _city As String = String.Empty
    Protected _state As String = String.Empty
    Protected _zip As String = String.Empty
    Protected _contact1name As String = String.Empty
    Protected _contact2name As String = String.Empty
    Protected _contact1phone As String = String.Empty
    Protected _contact2phone As String = String.Empty
    Protected _contact1fax As String = String.Empty
    Protected _contact2fax As String = String.Empty
    Protected _contact1email As String = String.Empty
    Protected _contact2email As String = String.Empty
    Protected _pointid As String = String.Empty
    Protected _staginglane As String = String.Empty
    Protected _stagingwarehousearea As String = String.Empty
    Protected _route As String = String.Empty
    Protected _pickupconfirmationtype As String = String.Empty
    Protected _deliveryconfirmationtype As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

    Protected _fixedservicetime As Integer = 0

#End Region

    Protected _servicetimecollection As CompanyServiceTimeCollection = Nothing

#End Region

#Region "Properties"

    Public ReadOnly Property FixedServiceTime() As Integer
        Get
            Return _fixedservicetime
        End Get
    End Property

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONTACTID = '" & _contactid & "'"
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

    Public Property STREET1() As String
        Get
            Return _street1
        End Get
        Set(ByVal Value As String)
            _street1 = Value
        End Set
    End Property

    Public Property STREET2() As String
        Get
            Return _street2
        End Get
        Set(ByVal Value As String)
            _street2 = Value
        End Set
    End Property

    Public Property CITY() As String
        Get
            Return _city
        End Get
        Set(ByVal Value As String)
            _city = Value
        End Set
    End Property

    Public Property STATE() As String
        Get
            Return _state
        End Get
        Set(ByVal Value As String)
            _state = Value
        End Set
    End Property

    Public Property ZIP() As String
        Get
            Return _zip
        End Get
        Set(ByVal Value As String)
            _zip = Value
        End Set
    End Property

    Public Property CONTACT1NAME() As String
        Get
            Return _contact1name
        End Get
        Set(ByVal Value As String)
            _contact1name = Value
        End Set
    End Property

    Public Property CONTACT2NAME() As String
        Get
            Return _contact2name
        End Get
        Set(ByVal Value As String)
            _contact2name = Value
        End Set
    End Property

    Public Property CONTACT1PHONE() As String
        Get
            Return _contact1phone
        End Get
        Set(ByVal Value As String)
            _contact1phone = Value
        End Set
    End Property

    Public Property CONTACT2PHONE() As String
        Get
            Return _contact2phone
        End Get
        Set(ByVal Value As String)
            _contact2phone = Value
        End Set
    End Property

    Public Property CONTACT1FAX() As String
        Get
            Return _contact1fax
        End Get
        Set(ByVal Value As String)
            _contact1fax = Value
        End Set
    End Property

    Public Property CONTACT2FAX() As String
        Get
            Return _contact2fax
        End Get
        Set(ByVal Value As String)
            _contact2fax = Value
        End Set
    End Property

    Public Property CONTACT1EMAIL() As String
        Get
            Return _contact1email
        End Get
        Set(ByVal Value As String)
            _contact1email = Value
        End Set
    End Property

    Public Property CONTACT2EMAIL() As String
        Get
            Return _contact2email
        End Get
        Set(ByVal Value As String)
            _contact2email = Value
        End Set
    End Property

    Public Property POINTID() As String
        Get
            Return _pointid
        End Get
        Set(ByVal Value As String)
            _pointid = Value
        End Set
    End Property

    Public Property ROUTE() As String
        Get
            Return _route
        End Get
        Set(ByVal Value As String)
            _route = Value
        End Set
    End Property

    Public Property STAGINGLANE() As String
        Get
            Return _staginglane
        End Get
        Set(ByVal Value As String)
            _staginglane = Value
        End Set
    End Property

    Public Property STAGINGWAREHOUSEAREA() As String
        Get
            Return _stagingwarehousearea
        End Get
        Set(ByVal Value As String)
            _stagingwarehousearea = Value
        End Set
    End Property

    Public Property PICKUPCONFIRMATIONTYPE() As String
        Get
            Return _pickupconfirmationtype
        End Get
        Set(ByVal Value As String)
            _pickupconfirmationtype = Value
        End Set
    End Property

    Public Property DELIVERYCONFIRMATIONTYPE() As String
        Get
            Return _deliveryconfirmationtype
        End Get
        Set(ByVal Value As String)
            _deliveryconfirmationtype = Value
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

    Public ReadOnly Property ServiceTimeCollection() As WMS.Logic.CompanyServiceTimeCollection
        Get
            Return _servicetimecollection
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
        _servicetimecollection = New CompanyServiceTimeCollection
    End Sub

    Public Sub New(ByVal pCONTACTID As String, Optional ByVal LoadObj As Boolean = True)
        _contactid = pCONTACTID
        If Contact.Exists(_contactid) And LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetCONTACT(ByVal pCONTACTID As String, ByVal pContactType As ContactReference) As Contact
        Return New Contact(pCONTACTID, pContactType)
    End Function

    Public Shared Function Exists(ByVal pContactId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from CONTACT where ContactId = '{0}'", pContactId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function ContactTypeToString(ByVal pContactType As ContactReference) As String
        Select Case pContactType
            Case ContactReference.CarrierContact
                Return "CARRIER"
            Case ContactReference.CompanyContact
                Return "COMPANY"
            Case ContactReference.ConsigneeContact
                Return "CONSIGNEE"
        End Select
        Return String.Empty
    End Function

    Public Shared Function ContactTypeFromString(ByVal pContactType As String) As ContactReference
        Select Case pContactType.ToLower
            Case "carrier"
                Return ContactReference.CarrierContact
            Case "company"
                Return ContactReference.CompanyContact
            Case "consignee"
                Return ContactReference.ConsigneeContact
        End Select
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM CONTACT Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Contact does not exist", "Contact does not exist")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        setObj(dr)

        Try
            _servicetimecollection = New CompanyServiceTimeCollection(_contactid)
        Catch ex As Exception
            _servicetimecollection = New CompanyServiceTimeCollection
        End Try
    End Sub

    Private Sub setObj(ByVal dr As DataRow)
        Try
            If Not dr.IsNull("FIXEDSERVICETIME") Then _fixedservicetime = _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr.Item("FIXEDSERVICETIME"))
        Catch ex As Exception
        End Try

        Try
            If Not dr.IsNull("STREET1") Then _street1 = dr.Item("STREET1")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("STREET2") Then _street2 = dr.Item("STREET2")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CITY") Then _city = dr.Item("CITY")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("STATE") Then _state = dr.Item("STATE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("ZIP") Then _zip = dr.Item("ZIP")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT1NAME") Then _contact1name = dr.Item("CONTACT1NAME")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT2NAME") Then _contact2name = dr.Item("CONTACT2NAME")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT1PHONE") Then _contact1phone = dr.Item("CONTACT1PHONE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT2PHONE") Then _contact2phone = dr.Item("CONTACT2PHONE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT1FAX") Then _contact1fax = dr.Item("CONTACT1FAX")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT2FAX") Then _contact2fax = dr.Item("CONTACT2FAX")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT1EMAIL") Then _contact1email = dr.Item("CONTACT1EMAIL")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("CONTACT2EMAIL") Then _contact2email = dr.Item("CONTACT2EMAIL")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("POINTID") Then _pointid = dr.Item("POINTID")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        Catch ex As Exception
        End Try
        _route = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE"))
        _staginglane = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STAGINGLANE"))
        _stagingwarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STAGINGWAREHOUSEAREA"))
        _pickupconfirmationtype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("pickupconfirmationtype"))
        _deliveryconfirmationtype = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("deliveryconfirmationtype"))
    End Sub

#End Region

#Region "Insert / Delete"

    Public Sub Save(ByVal pUser As String)
        Dim sql As String
        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitytype As String

        If Contact.Exists(_contactid) Then
            _editdate = DateTime.Now
            _edituser = pUser
            sql = String.Format("UPDATE CONTACT SET STREET1 = {0},STREET2 = {1},CITY = {2},STATE = {3},ZIP = {4},CONTACT1NAME = {5}," & _
                "CONTACT2NAME = {6},CONTACT1PHONE = {7},CONTACT2PHONE = {8},CONTACT1FAX = {9},CONTACT2FAX = {10}," & _
                "CONTACT1EMAIL = {11},CONTACT2EMAIL = {12},ROUTE = {13}, STAGINGLANE = {14},STAGINGWAREHOUSEAREA={19}, PICKUPCONFIRMATIONTYPE={15} ,DELIVERYCONFIRMATIONTYPE={16}, EDITDATE = {17},EDITUSER = {18} WHERE " & WhereClause, Made4Net.Shared.Util.FormatField(_street1, , True), _
                Made4Net.Shared.Util.FormatField(_street2, , True), Made4Net.Shared.Util.FormatField(_city, , True), Made4Net.Shared.Util.FormatField(_state, , True), Made4Net.Shared.Util.FormatField(_zip, , True), _
                Made4Net.Shared.Util.FormatField(_contact1name, , True), Made4Net.Shared.Util.FormatField(_contact2name, , True), Made4Net.Shared.Util.FormatField(_contact1phone, , True), Made4Net.Shared.Util.FormatField(_contact2phone, , True), _
                Made4Net.Shared.Util.FormatField(_contact1fax, , True), Made4Net.Shared.Util.FormatField(_contact2fax, , True), Made4Net.Shared.Util.FormatField(_contact1email, , True), Made4Net.Shared.Util.FormatField(_contact2email, , True), _
                Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_pickupconfirmationtype), Made4Net.Shared.Util.FormatField(_deliveryconfirmationtype), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_stagingwarehousearea))

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ContactUpdated)
            activitytype = WMS.Lib.Actions.Audit.CONTACTUPD
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContactUpdated
            'em.Add("EVENT", EventType)
            'em.Add("CONTACT", _contactid)
            'em.Add("USERID", _adduser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))

        Else
            _editdate = DateTime.Now
            _edituser = pUser
            _adddate = DateTime.Now
            _adduser = pUser
            sql = "Insert into Contact(CONTACTID,STREET1,STREET2,CITY,STATE,ZIP,CONTACT1NAME," & _
                "CONTACT2NAME,CONTACT1PHONE,CONTACT2PHONE,CONTACT1FAX,CONTACT2FAX,CONTACT1EMAIL," & _
                "CONTACT2EMAIL,ROUTE,STAGINGLANE,STAGINGWAREHOUSEAREA,PICKUPCONFIRMATIONTYPE,DELIVERYCONFIRMATIONTYPE,ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ("

            sql += Made4Net.Shared.Util.FormatField(_contactid) & "," & Made4Net.Shared.Util.FormatField(_street1, , True) & "," & _
               Made4Net.Shared.Util.FormatField(_street2, , True) & "," & Made4Net.Shared.Util.FormatField(_city, , True) & "," & Made4Net.Shared.Util.FormatField(_state, , True) & "," & Made4Net.Shared.Util.FormatField(_zip, , True) & _
               "," & Made4Net.Shared.Util.FormatField(_contact1name, , True) & "," & Made4Net.Shared.Util.FormatField(_contact2name, , True) & "," & _
               Made4Net.Shared.Util.FormatField(_contact1phone, , True) & "," & Made4Net.Shared.Util.FormatField(_contact2phone, , True) & "," & _
               Made4Net.Shared.Util.FormatField(_contact1fax, , True) & "," & Made4Net.Shared.Util.FormatField(_contact2fax, , True) & "," & _
               Made4Net.Shared.Util.FormatField(_contact1email, , True) & "," & Made4Net.Shared.Util.FormatField(_contact2email, , True) & "," & _
               Made4Net.Shared.Util.FormatField(_route) & "," & Made4Net.Shared.Util.FormatField(_staginglane) & "," & Made4Net.Shared.Util.FormatField(_stagingwarehousearea) & "," & _
               Made4Net.Shared.Util.FormatField(_pickupconfirmationtype) & "," & Made4Net.Shared.Util.FormatField(_deliveryconfirmationtype) & "," & _
               Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
               Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ContactCreated)
            activitytype = WMS.Lib.Actions.Audit.CONTACTINS
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ContactCreated
            'em.Add("EVENT", EventType)
            'em.Add("CONTACT", _contactid)
            'em.Add("USERID", _adduser)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
        End If
        DataInterface.RunSQL(sql)

        aq.Add("ACTIVITYTYPE", activitytype)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _contactid)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", "")
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(activitytype)
    End Sub

    Public Sub SetContact(ByVal pContactID As String, ByVal pStreet1 As String, ByVal pStreet2 As String, ByVal pCity As String, ByVal pState As String, _
            ByVal pZip As String, ByVal pName1 As String, ByVal pName2 As String, ByVal pPhone1 As String, ByVal pPhone2 As String, ByVal pFax1 As String, _
            ByVal pFax2 As String, ByVal pEMail1 As String, ByVal pEMail2 As String, ByVal pRoute As String, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, ByVal pPickupConfirmationType As String, ByVal pDeliveryConfirmationType As String, ByVal pUser As String)
        Dim dr As DataRow
        Dim sql As String = ""
        If pContactID = String.Empty Then
            _contactid = Made4Net.Shared.Util.getNextCounter("CONTACTID")
        Else
            _contactid = pContactID
        End If
        _street1 = pStreet1
        _street2 = pStreet2
        _city = pCity
        _state = pState
        _zip = pZip
        _contact1name = pName1
        _contact2name = pName2
        _contact1phone = pPhone1
        _contact2phone = pPhone2
        _contact1fax = pFax1
        _contact2fax = pFax2
        _contact1email = pEMail1
        _contact2email = pEMail2
        _route = pRoute

        _staginglane = pStaginglane
        _stagingwarehousearea = pStagingwarehousearea

        If _staginglane <> String.Empty And Not _staginglane Is Nothing Then
            If _stagingwarehousearea = String.Empty Or _stagingwarehousearea Is Nothing Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Staging lane is not valid.", "Staging lane is not valid.")
            Else
                If Not Location.Exists(_staginglane, _stagingwarehousearea) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Staging lane does not exist.", "Staging lane does not exist.")
                End If
            End If
        End If

        If _stagingwarehousearea <> String.Empty And Not _stagingwarehousearea Is Nothing Then
            If _staginglane = String.Empty Or _staginglane Is Nothing Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Staging lane is not valid.", "Staging lane is not valid.")
            End If
        End If

        _pickupconfirmationtype = pPickupConfirmationType
        _deliveryconfirmationtype = pDeliveryConfirmationType
        _editdate = DateTime.Now
        _edituser = pUser
        If Not (Contact.Exists(_contactid)) Then
            _adduser = pUser
            _adddate = DateTime.Now
            sql = "Insert into Contact(CONTACTID,STREET1,STREET2,CITY,STATE,ZIP,CONTACT1NAME," & _
                "CONTACT2NAME,CONTACT1PHONE,CONTACT2PHONE,CONTACT1FAX,CONTACT2FAX,CONTACT1EMAIL," & _
                "CONTACT2EMAIL,ROUTE,STAGINGLANE,STAGINGWAREHOUSEAREA, PICKUPCONFIRMATIONTYPE,DELIVERYCONFIRMATIONTYPE,ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ("

            sql = String.Format("{0}{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23})", _
            sql, Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_street1, , True), Made4Net.Shared.Util.FormatField(_street2, , True), _
            Made4Net.Shared.Util.FormatField(_city, , True), Made4Net.Shared.Util.FormatField(_state, , True), Made4Net.Shared.Util.FormatField(_zip, , True), _
            Made4Net.Shared.Util.FormatField(_contact1name, , True), Made4Net.Shared.Util.FormatField(_contact2name, , True), _
            Made4Net.Shared.Util.FormatField(_contact1phone, , True), Made4Net.Shared.Util.FormatField(_contact2phone, , True), _
            Made4Net.Shared.Util.FormatField(_contact1fax, , True), Made4Net.Shared.Util.FormatField(_contact2fax, , True), _
            Made4Net.Shared.Util.FormatField(_contact1email, , True), Made4Net.Shared.Util.FormatField(_contact2email, , True), _
            Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_stagingwarehousearea), _
            Made4Net.Shared.Util.FormatField(_pickupconfirmationtype), Made4Net.Shared.Util.FormatField(_deliveryconfirmationtype), _
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), _
            Made4Net.Shared.Util.FormatField(_edituser))

        Else
            sql = String.Format("UPDATE CONTACT SET STREET1 = {0},STREET2 = {1},CITY = {2},STATE = {3},ZIP = {4},CONTACT1NAME = {5}," & _
                "CONTACT2NAME = {6},CONTACT1PHONE = {7},CONTACT2PHONE = {8},CONTACT1FAX = {9},CONTACT2FAX = {10}," & _
                "CONTACT1EMAIL = {11},CONTACT2EMAIL = {12},ROUTE = {13}, STAGINGLANE = {14}, STAGINGWAREHOUSEAREA = {19},PICKUPCONFIRMATIONTYPE={15} ,DELIVERYCONFIRMATIONTYPE={16}, EDITDATE = {17},EDITUSER = {18} WHERE " & WhereClause, Made4Net.Shared.Util.FormatField(_street1, , True), _
                Made4Net.Shared.Util.FormatField(_street2, , True), Made4Net.Shared.Util.FormatField(_city, , True), Made4Net.Shared.Util.FormatField(_state, , True), Made4Net.Shared.Util.FormatField(_zip, , True), _
                Made4Net.Shared.Util.FormatField(_contact1name, , True), Made4Net.Shared.Util.FormatField(_contact2name, , True), Made4Net.Shared.Util.FormatField(_contact1phone, , True), Made4Net.Shared.Util.FormatField(_contact2phone, , True), _
                Made4Net.Shared.Util.FormatField(_contact1fax, , True), Made4Net.Shared.Util.FormatField(_contact2fax, , True), Made4Net.Shared.Util.FormatField(_contact1email, , True), Made4Net.Shared.Util.FormatField(_contact2email, , True), _
                Made4Net.Shared.Util.FormatField(_route), Made4Net.Shared.Util.FormatField(_staginglane), Made4Net.Shared.Util.FormatField(_pickupconfirmationtype), Made4Net.Shared.Util.FormatField(_deliveryconfirmationtype), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_stagingwarehousearea))
        End If

        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Delete()
        Dim SQL As String
        SQL = String.Format("delete from contact where {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub CreateContact(ByVal pContactID As String, ByVal pStreet1 As String, ByVal pStreet2 As String, ByVal pCity As String, ByVal pState As String, _
            ByVal pZip As String, ByVal pName1 As String, ByVal pName2 As String, ByVal pPhone1 As String, ByVal pPhone2 As String, ByVal pFax1 As String, _
            ByVal pFax2 As String, ByVal pEMail1 As String, ByVal pEMail2 As String, ByVal pRoute As String, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, ByVal pPickupConfirmationType As String, ByVal pDeliveryConfirmationType As String, ByVal pUser As String)

        If Exists(pContactID) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create contact. Contact with same id already exists in system.", "Cannot create contact. Contact with same id already exists in system.")
        End If

        SetContact(pContactID, pStreet1, pStreet2, pCity, pState, pZip, pName1, pName2, pPhone1, pPhone2, pFax1, pFax2, pEMail1, pEMail2, pRoute, pStaginglane, _
        pStagingwarehousearea, pPickupConfirmationType, pDeliveryConfirmationType, pUser)

    End Sub

    Public Sub UpdateContact(ByVal pContactID As String, ByVal pStreet1 As String, ByVal pStreet2 As String, ByVal pCity As String, ByVal pState As String, _
            ByVal pZip As String, ByVal pName1 As String, ByVal pName2 As String, ByVal pPhone1 As String, ByVal pPhone2 As String, ByVal pFax1 As String, _
            ByVal pFax2 As String, ByVal pEMail1 As String, ByVal pEMail2 As String, ByVal pRoute As String, ByVal pStaginglane As String, ByVal pStagingwarehousearea As String, ByVal pPickupConfirmationType As String, ByVal pDeliveryConfirmationType As String, ByVal pUser As String)

        If Not Exists(pContactID) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not edit contact. It does not exist.", "Can not edit contact. It does not exist.")
        End If
        SetContact(pContactID, pStreet1, pStreet2, pCity, pState, pZip, pName1, pName2, pPhone1, pPhone2, pFax1, pFax2, pEMail1, pEMail2, pRoute, pStaginglane, _
        pStagingwarehousearea, pPickupConfirmationType, pDeliveryConfirmationType, pUser)
    End Sub

#End Region

#Region "Routing"
    Public Function CanDeliver(ByVal DeliveryDate As DateTime, ByVal EstArrivalHour As Int32, ByVal pDefaultOpenHour As Int32, ByVal pDefaultCloseHour As Int32, _
                               ByVal AllowedTimeBeforeOpen As Integer, _
                               ByRef OpenHour As Integer, ByRef CloseHour As Integer) As Boolean
        Dim cst As CompanyServiceTime

        ''EstArrivalHour = DifTime(AllowedTimeBeforeOpen, EstArrivalHour)
        Dim addDays As Integer
        Dim waitingEstArrivalHour As Integer = DifTime(AllowedTimeBeforeOpen, EstArrivalHour, addDays)

        If ServiceTimeCollection Is Nothing OrElse ServiceTimeCollection.Count = 0 Then Return True

        'OpenHour = pDefaultOpenHour
        'CloseHour = pDefaultCloseHour
        'If (OpenHour <= watiingEstArrivalHour Or OpenHour = 0) And _
        '    (CloseHour >= EstArrivalHour Or CloseHour = 0) Then
        '    Return True
        'End If
        'Else
        For Each cst In Me.ServiceTimeCollection
            Select Case DeliveryDate.DayOfWeek
                Case DayOfWeek.Sunday
                    OpenHour = cst.SunOpenHour
                    CloseHour = cst.SunCloseHour
                Case DayOfWeek.Monday
                    OpenHour = cst.MonOpenHour
                    CloseHour = cst.MonCloseHour
                Case DayOfWeek.Tuesday
                    OpenHour = cst.TueOpenHour
                    CloseHour = cst.TueCloseHour
                Case DayOfWeek.Wednesday
                    OpenHour = cst.WedOpenHour
                    CloseHour = cst.WedCloseHour
                Case DayOfWeek.Thursday
                    OpenHour = cst.ThuOpenHour
                    CloseHour = cst.ThuCloseHour
                Case DayOfWeek.Friday
                    OpenHour = cst.FriOpenHour
                    CloseHour = cst.FriCloseHour
                Case DayOfWeek.Saturday
                    OpenHour = cst.SatOpenHour
                    CloseHour = cst.SatCloseHour
            End Select
            If (OpenHour <= waitingEstArrivalHour Or OpenHour = 0) And _
            (CloseHour >= EstArrivalHour Or CloseHour = 0) Then
                Return True
            End If
        Next
        '' End If

        Return False
    End Function

    Public Function GetWaitingTime(ByVal DeliveryDate As DateTime, ByVal EstArrivalHour As Int32, ByVal pDefaultOpenHour As Int32) As Integer
        Dim res As Integer
        If ServiceTimeCollection Is Nothing OrElse Me.ServiceTimeCollection.Count = 0 Then
            res = Math.Max(EstArrivalHour, pDefaultOpenHour)
            Return res
        End If
        Dim cst As CompanyServiceTime
        For Each cst In Me.ServiceTimeCollection
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

    Public Function OpeningHour(ByVal DeliveryDate As DateTime) As Int32
        If ServiceTimeCollection Is Nothing Then Return 0
        Dim OpenHour As Integer = 0

        Dim cst As CompanyServiceTime
        For Each cst In Me.ServiceTimeCollection
            Select Case DeliveryDate.DayOfWeek
                Case DayOfWeek.Sunday
                    OpenHour = cst.SunOpenHour
                Case DayOfWeek.Monday
                    OpenHour = cst.MonOpenHour
                Case DayOfWeek.Tuesday
                    OpenHour = cst.TueOpenHour
                Case DayOfWeek.Wednesday
                    OpenHour = cst.WedOpenHour
                Case DayOfWeek.Thursday
                    OpenHour = cst.ThuOpenHour
                Case DayOfWeek.Friday
                    OpenHour = cst.FriOpenHour
                Case DayOfWeek.Saturday
                    OpenHour = cst.SatOpenHour
            End Select
        Next
        If OpenHour = 0 Then OpenHour = 0
        Return OpenHour

    End Function
    Public Function CloseHour(ByVal DeliveryDate As DateTime) As Int32
        If ServiceTimeCollection Is Nothing Then Return 2400
        Dim ClosingHour As Integer = 2400

        Dim cst As CompanyServiceTime
        For Each cst In Me.ServiceTimeCollection
            Select Case DeliveryDate.DayOfWeek
                Case DayOfWeek.Sunday
                    ClosingHour = cst.SunCloseHour
                Case DayOfWeek.Monday
                    ClosingHour = cst.MonCloseHour
                Case DayOfWeek.Tuesday
                    ClosingHour = cst.TueCloseHour
                Case DayOfWeek.Wednesday
                    ClosingHour = cst.WedCloseHour
                Case DayOfWeek.Thursday
                    ClosingHour = cst.ThuCloseHour
                Case DayOfWeek.Friday
                    ClosingHour = cst.FriCloseHour
                Case DayOfWeek.Saturday
                    ClosingHour = cst.SatCloseHour
            End Select
        Next
        If ClosingHour = 0 Then ClosingHour = 2400
        Return ClosingHour
    End Function

    Public Function DifTime(ByVal itotalTime As Double, ByVal pStartTime As Integer, _
        ByRef AddDays As Integer) As Integer
        Dim min, res As Integer
        min = Math.Floor(itotalTime / 60) + Math.Floor(pStartTime / 100) * 60 + Math.Floor(pStartTime Mod 100)
        res = Math.Floor(min / 60) * 100 + Math.Floor(min Mod 60)
        ''        AddDays = WMS.Logic.Route.get24Hour(res)
        ''      res = get24Hour(res)

        Return res
    End Function

    Public Sub setPointId(ByVal paramPointId As String, ByVal paramUser As String)
        _pointid = paramPointId
        _edituser = paramUser
        _editdate = DateTime.Now
        Dim sql As String
        If Contact.Exists(_contactid) Then
            sql = String.Format("update contact set pointid={0},edituser={1},editdate={2} where {3}", Made4Net.Shared.Util.FormatField(_pointid), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        Else
            _adduser = paramUser
            _adddate = DateTime.Now
            sql = String.Format("insert into contact(contactid,pointid,adduser,adddate,edituser,editdate) values ({0},{1},{2},{3},{4},{5})", Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(_pointid), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate))
        End If
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub SetServiceTime(ByVal pContact As String, ByVal pPriority As Int32, ByVal pSunOpen As Int32, ByVal pSunClose As Int32, _
                ByVal pMonOpen As Int32, ByVal pMonClose As Int32, ByVal pTueOpen As Int32, ByVal pTueClose As Int32, _
                ByVal pWedOpen As Int32, ByVal pWedClose As Int32, ByVal pThuOpen As Int32, ByVal pThuClose As Int32, _
                 ByVal pFriOpen As Int32, ByVal pFriClose As Int32, ByVal pSatOpen As Int32, ByVal pSatClose As Int32, ByVal pUser As String)

        Me.ServiceTimeCollection.setServiceTime(pContact, pPriority, pSunOpen, pSunClose, pMonOpen, pMonClose, pTueOpen, pTueClose, _
               pWedOpen, pWedClose, pThuOpen, pThuClose, pFriOpen, pFriClose, pSatOpen, pSatClose, pUser)
    End Sub

    Public Sub SetServiceTime(ByVal dr As DataRow, ByVal pUser As String)
        Me.ServiceTimeCollection.setServiceTime(dr, pUser)
    End Sub

#End Region

#End Region

End Class

Public Enum ContactReference
    ConsigneeContact
    CompanyContact
    CarrierContact
End Enum

#End Region



