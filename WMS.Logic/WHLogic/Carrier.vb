Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

#Region "CARRIER"

' <summary>
' This object represents the properties and methods of a CARRIER.
' </summary>

<CLSCompliant(False)> Public Class Carrier

#Region "Variables"

#Region "Primary Keys"

    Protected _carrier As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _carriername As String = String.Empty
    Protected _notes As String = String.Empty
    Protected _contactid As String
    Protected _contact As Contact
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " CARRIER = '" & _carrier & "'"
        End Get
    End Property

    Public Property CARRIER() As String
        Get
            Return _carrier
        End Get
        Set(ByVal Value As String)
            _carrier = Value
        End Set
    End Property

    Public Property CARRIERNAME() As String
        Get
            Return _carriername
        End Get
        Set(ByVal Value As String)
            _carriername = Value
        End Set
    End Property

    Public ReadOnly Property CarrierContact() As Contact
        Get
            Return _contact
        End Get
    End Property

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal Value As String)
            _notes = Value
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

    Public Sub New(ByVal pCARRIER As String, Optional ByVal LoadObj As Boolean = True)
        _carrier = pCARRIER
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName.ToLower = "setcontactupdate" Then
            _contactid = ds.Tables(0).Rows(0)("contactid")
            LoadByContactId(_contactid)
            _contact.SetContact(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("contactid")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("street1")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("street2")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("city")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("state")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("zip")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1NAME")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2NAME")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1PHONE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2PHONE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1FAX")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2FAX")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1EMAIL")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2EMAIL")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STAGINGLANE")), Nothing, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "setcontactinsert" Then
            _contactid = Made4Net.Shared.Util.getNextCounter("CONTACTID")
            _contact = New WMS.Logic.Contact
            _contact.SetContact(_contactid, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("street1")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("street2")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("city")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("state")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("zip")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1NAME")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2NAME")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1PHONE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2PHONE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1FAX")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2FAX")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1EMAIL")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2EMAIL")), _
                Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ROUTE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STAGINGLANE")), Nothing, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKUPCONFIRMATIONTYPE")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DELIVERYCONFIRMATIONTYPE")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "createnew" Then
            CreateNew(dr("carrier"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("carriername")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("notes")), Common.GetCurrentUser)
        ElseIf CommandName.ToLower = "update" Then
            Dim oCarrier As New Carrier(dr("carrier"))
            oCarrier.Update(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("carriername")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("notes")), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("contactid")), Common.GetCurrentUser())
        End If
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetCARRIER(ByVal pCARRIER As String) As Carrier
        Return New Carrier(pCARRIER)
    End Function

    Public Shared Function Exists(ByVal pCarrier As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from Carrier where Carrier = '{0}'", pCarrier)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub LoadByContactId(ByVal pContactId As String)
        _contactid = pContactId
        Try
            _carrier = DataInterface.ExecuteScalar("Select carrier from carrier where contactid = '" & pContactId & "'")
            Load()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM CARRIER Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("CARRIERNAME") Then _carriername = dr.Item("CARRIERNAME")
        If Not dr.IsNull("NOTES") Then _notes = dr.Item("NOTES")
        If Not dr.IsNull("CONTACTID") Then _contactid = dr.Item("CONTACTID")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        _contact = New Contact(_contactid, ContactReference.CarrierContact)

    End Sub

    Public Sub Save()
        Dim SQL As String
        If _contactid = String.Empty Then _contactid = Made4Net.Shared.Util.getNextCounter("CONTACTID")

        If WMS.Logic.Carrier.Exists(_carrier) Then
            SQL = String.Format("Update carrier set carrier = {0}, carriername = {1}, notes={2}, editdate = {3}, edituser = {4},  contactid = {5} where " & WhereClause, _
                 Made4Net.Shared.Util.FormatField(_carrier), Made4Net.Shared.Util.FormatField(_carriername), Made4Net.Shared.Util.FormatField(_notes), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_contactid))

            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.CarrierUpdated
            em.Add("EVENT", EventType)
            em.Add("CARRIER", _carrier)
            em.Add("USERID", _adduser)
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))
        Else
            SQL = "Insert into carrier(carrier,carriername,contactid, ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ("
            SQL += Made4Net.Shared.Util.FormatField(_carrier) & "," & _
               Made4Net.Shared.Util.FormatField(_carriername) & "," & _
               Made4Net.Shared.Util.FormatField(_contactid) & "," & _
               Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
               Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

            Dim em As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.CarrierCreated
            em.Add("EVENT", EventType)
            em.Add("CARRIER", _carrier)
            em.Add("USERID", _adduser)
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Send(WMSEvents.EventDescription(EventType))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub CreateNew(ByVal pCarrier As String, ByVal pCarrierName As String, ByVal pNotes As String, ByVal pUser As String)
        Dim SQL As String

        'If pContactID <> String.Empty And Not pContactID Is Nothing Then
        '    _contactid = pContactID
        'Else
        _contactid = Made4Net.Shared.Util.getNextCounter("CONTACTID")
        'End If
        _carrier = pCarrier
        _carriername = pCarrierName
        _notes = pNotes
        _adddate = DateTime.Now
        _editdate = DateTime.Now
        _adduser = pUser
        _edituser = pUser

        If WMS.Logic.Carrier.Exists(_carrier) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Carrier Already Exist", "Carrier Already Exist")
        Else
            SQL = "Insert into carrier(carrier,carriername,contactid,Notes,ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ("
            SQL += Made4Net.Shared.Util.FormatField(_carrier) & "," & _
               Made4Net.Shared.Util.FormatField(_carriername) & "," & _
               Made4Net.Shared.Util.FormatField(_contactid) & "," & _
               Made4Net.Shared.Util.FormatField(_notes) & "," & _
               Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
               Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"
        End If
        DataInterface.RunSQL(SQL)

        If Not Contact.Exists(_contactid) Then
            Dim contact As New WMS.Logic.Contact()
            contact.SetContact(_contactid, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, _
            Nothing, Nothing, Nothing, Nothing, Nothing, Common.GetCurrentUser())
        End If

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.CarrierCreated
        em.Add("EVENT", EventType)
        em.Add("CARRIER", _carrier)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub

    Public Sub Update(ByVal pCarrierName As String, ByVal pNotes As String, ByVal pContactID As String, ByVal pUser As String)
        If Not Exists(_carrier) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Carrier does not exist", "Carrier does not exist")
        End If

        If Not Contact.Exists(pContactID) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Contact does not exist. Can only modify with existing contact.", "Contact does not exist. Can only modify with existing contact.")
        End If

        _carriername = pCarrierName
        _notes = pNotes
        _contactid = pContactID
        _editdate = DateTime.Now
        _edituser = pUser

        Dim sql As String = String.Format("Update carrier set carriername={0},notes={1},editdate={2},edituser={3},contactid={4} where {5}", _
                Made4Net.Shared.FormatField(_carriername), Made4Net.Shared.FormatField(_notes), Made4Net.Shared.FormatField(_editdate), _
                Made4Net.Shared.FormatField(_edituser), Made4Net.Shared.FormatField(_contactid), WhereClause)

        DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class

#End Region

