Imports made4net.Shared.Conversion
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteStopTask

#Region "Variables"

    Protected _routeid As String
    Protected _stopnumber As Int32
    Protected _schedualeddate As DateTime
    Protected _stoptaskid As Int32
    Protected _stoptasktype As StopTaskType
    Protected _stoptaskname As String
    Protected _consignee As String
    Protected _documentid As String
    Protected _documenttype As String
    Protected _company As String
    Protected _companytype As String
    Protected _packtype As String
    Protected _numpacks As Int32
    Protected _transportationclass As String
    Protected _stopdetvolume As Decimal
    Protected _stopdetweight As Decimal
    Protected _stopdetvalue As Decimal
    Protected _status As StopTaskStatus
    Protected _contactid As String

    Protected _chkpnt As String '' contactid

    Protected _contact As WMS.Logic.Contact
    Protected _confirmationtype As StopTaskConfirmationType
    Protected _comments As String
    Protected _reasoncode As String
    Protected _confdocid As String
    Protected _confirmationnumber As String
    Protected _confirmationlevel As String
    Protected _allowpartialloading As Boolean
    Protected _allowpartialunloading As Boolean
    Protected _allowpartialpickup As Boolean
    Protected _confirmpackageatunloading As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    Protected _packagescollection As RouteStopTaskPackagesCollection
    Protected _itemscollection As RouteStopTaskItemsCollection
    'Task Object - Loaded only if this task is type of general task
    Protected _routegentask As RouteGeneralTask



#End Region

#Region "Properties"

    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" routeid = {0} and stopnumber = {1} and stoptaskid = {2}", Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_stoptaskid))
        End Get
    End Property

    Public Property RouteID() As String
        Set(ByVal Value As String)
            _routeid = Value
        End Set
        Get
            Return _routeid
        End Get
    End Property

    Public Property StopNumber() As Int32
        Get
            Return _stopnumber
        End Get
        Set(ByVal Value As Int32)
            _stopnumber = Value
        End Set
    End Property

    Public Property StopTaskType() As StopTaskType
        Get
            Return _stoptasktype
        End Get
        Set(ByVal Value As StopTaskType)
            _stoptasktype = Value
        End Set
    End Property

    Public Property SchedualedDate() As DateTime
        Get
            Return _schedualeddate
        End Get
        Set(ByVal Value As DateTime)
            _schedualeddate = Value
        End Set
    End Property

    Public ReadOnly Property CompanyObj() As WMS.Logic.Company
        Get
            Try
                Return New Company(_consignee, _company, _companytype)
            Catch ex As Exception
                Return Nothing
            End Try
        End Get
    End Property

    Public Property Company() As String
        Get
            Return _company
        End Get
        Set(ByVal Value As String)
            _company = Value
        End Set
    End Property

    Public Property CompanyType() As String
        Get
            Return _companytype
        End Get
        Set(ByVal Value As String)
            _companytype = Value
        End Set
    End Property

    Public Property StopTaskId() As Int32
        Get
            Return _stoptaskid
        End Get
        Set(ByVal Value As Int32)
            _stoptaskid = Value
        End Set
    End Property

    Public Property StopTaskName() As String
        Get
            Return _stoptaskname
        End Get
        Set(ByVal Value As String)
            _stoptaskname = Value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property DocumentId() As String
        Get
            Return _documentid
        End Get
        Set(ByVal Value As String)
            _documentid = Value
        End Set
    End Property

    Public Property DocumentType() As String
        Get
            Return _documenttype
        End Get
        Set(ByVal Value As String)
            _documenttype = Value
        End Set
    End Property

    Public Property PackType() As String
        Get
            Return _packtype
        End Get
        Set(ByVal Value As String)
            _packtype = Value
        End Set
    End Property

    Public Property NumPacks() As Int32
        Get
            Return _numpacks
        End Get
        Set(ByVal Value As Int32)
            _numpacks = Value
        End Set
    End Property

    Public Property TransportationClass() As String
        Get
            Return _transportationclass
        End Get
        Set(ByVal Value As String)
            _transportationclass = Value
        End Set
    End Property

    Public Property StopDetailVolume() As Decimal
        Get
            Return _stopdetvolume
        End Get
        Set(ByVal Value As Decimal)
            _stopdetvolume = Value
        End Set
    End Property

    Public Property StopDetailWeight() As Decimal
        Get
            Return _stopdetweight
        End Get
        Set(ByVal Value As Decimal)
            _stopdetweight = Value
        End Set
    End Property

    Public Property StopDetailValue() As Decimal
        Get
            Return _stopdetvalue
        End Get
        Set(ByVal Value As Decimal)
            _stopdetvalue = Value
        End Set
    End Property

    Public Property Status() As StopTaskStatus
        Get
            Return _status
        End Get
        Set(ByVal Value As StopTaskStatus)
            _status = Value
        End Set
    End Property

    Public Property Comments() As String
        Get
            Return _comments
        End Get
        Set(ByVal Value As String)
            _comments = Value
        End Set
    End Property

    Public Property ContactId() As String
        Get
            Return _contactid
        End Get
        Set(ByVal Value As String)
            _contactid = Value
        End Set
    End Property

    Public Property ReasonCode() As String
        Get
            Return _reasoncode
        End Get
        Set(ByVal Value As String)
            _reasoncode = Value
        End Set
    End Property

    Public Property ConfirmationDocId() As String
        Get
            Return _confdocid
        End Get
        Set(ByVal Value As String)
            _confdocid = Value
        End Set
    End Property

    Public Property ConfirmationNumber() As String
        Get
            Return _confirmationnumber
        End Get
        Set(ByVal Value As String)
            _confirmationnumber = Value
        End Set
    End Property

    Public Property ConfirmationLevel() As String
        Get
            Return _confirmationlevel
        End Get
        Set(ByVal Value As String)
            _confirmationlevel = Value
        End Set
    End Property

    Public Property AllowPartailLoading() As Boolean
        Get
            Return _allowpartialloading
        End Get
        Set(ByVal Value As Boolean)
            _allowpartialloading = Value
        End Set
    End Property

    Public Property AllowPartailUnLoading() As Boolean
        Get
            Return _allowpartialunloading
        End Get
        Set(ByVal Value As Boolean)
            _allowpartialunloading = Value
        End Set
    End Property

    Public Property AllowPartialPickup() As Boolean
        Get
            Return _allowpartialpickup
        End Get
        Set(ByVal Value As Boolean)
            _allowpartialpickup = Value
        End Set
    End Property

    Public Property ConfirmPackageAtUnLoading() As Boolean
        Get
            Return _confirmpackageatunloading
        End Get
        Set(ByVal Value As Boolean)
            _confirmpackageatunloading = Value
        End Set
    End Property

    Public Property Contact() As WMS.Logic.Contact
        Get
            If _contact Is Nothing AndAlso WMS.Logic.Contact.Exists(_contactid) Then
                _contact = New WMS.Logic.Contact(_contactid)
            End If
            Return _contact
        End Get
        Set(ByVal Value As Contact)
            _contact = Value
        End Set
    End Property

    Public Property ConfirmationType() As StopTaskConfirmationType
        Get
            Return _confirmationtype
        End Get
        Set(ByVal Value As StopTaskConfirmationType)
            _confirmationtype = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public Property ChkPnt() As String
        Get
            Return _chkpnt
        End Get
        Set(ByVal Value As String)
            _chkpnt = Value
        End Set
    End Property


    Public ReadOnly Property StopPackages() As RouteStopTaskPackagesCollection
        Get
            Return _packagescollection
        End Get
    End Property
    Public ReadOnly Property StopItems() As RouteStopTaskItemsCollection
        Get
            Return _itemscollection
        End Get
    End Property

    Public ReadOnly Property GeneralTask() As RouteGeneralTask
        Get
            If _routegentask Is Nothing AndAlso _stoptasktype = Logic.StopTaskType.General Then
                If RouteGeneralTask.Exists(_documentid) Then
                    _routegentask = New RouteGeneralTask(_documentid)
                End If
            End If
            Return _routegentask
        End Get
    End Property

    Public ReadOnly Property Completed() As Boolean
        Get
            For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                If oStopPackage.Package.Status <> WMS.Lib.Statuses.RoutePackages.DELIVERED And oStopPackage.Package.Status <> WMS.Lib.Statuses.RoutePackages.OFFLOADED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property

    Public ReadOnly Property Loaded() As Boolean
        Get
            If _stoptasktype = Logic.StopTaskType.Delivery Then
                For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                    If oStopPackage.Package.Status <> WMS.Lib.Statuses.RoutePackages.LOADED Then
                        Return False
                    End If
                Next
            End If
            Return True
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal sRouteId As String, ByVal iStopNumber As Int32, ByVal iStopDetId As Int32)
        _stopnumber = iStopNumber
        _stoptaskid = iStopDetId
        _routeid = sRouteId
        Load()
    End Sub

    Public Sub New(ByVal sRouteId As String, ByVal iStopNumber As Int32, ByVal dr As DataRow)
        _routeid = sRouteId
        _stopnumber = iStopNumber
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetRouteStopTask(ByVal pRouteId As String, ByVal pConsignee As String, ByVal pOrderId As String) As RouteStopTask
        Dim sql As String = String.Format("Select * from routestoptask where consignee = {0} and documentid = {1} and routeid = {2} ", _
            Made4Net.Shared.Util.FormatField(pConsignee), Made4Net.Shared.Util.FormatField(pOrderId), Made4Net.Shared.Util.FormatField(pRouteId))
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Detail Not Found")
        End If
        dr = dt.Rows(0)
        Dim oRouteStopTask As New RouteStopTask(dr("ROUTEID"), System.Convert.ToInt32(dr("STOPNUMBER")), System.Convert.ToInt32(dr("STOPTASKID")))
        Return oRouteStopTask
    End Function

    Private Function GetNextStopNumber() As Int32
        Dim SQL As String
        SQL = String.Format("select isnull(max(stoptaskid),0) + 1 from routestoptask where routeid = '{0}' and stopnumber = {1}", _routeid, _stopnumber)
        Return DataInterface.ExecuteScalar(SQL)
    End Function

    Public Shared Function Exists(ByVal sRouteId As String, ByVal iStopNumber As Int32, ByVal iStopTaskId As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from routestoptask where routeid = {0} and stopnumber = {1} and stoptaskid = {2}", Made4Net.Shared.Util.FormatField(sRouteId), Made4Net.Shared.Util.FormatField(iStopNumber), Made4Net.Shared.Util.FormatField(iStopTaskId))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load(ByVal dr As DataRow)
        _schedualeddate = Convert.ReplaceDBNull(dr("schedualeddate"))
        _stoptaskid = dr("stoptaskid")
        _stoptasktype = StopTaskTypeFromString(Convert.ReplaceDBNull(dr("stoptasktype")))
        _stoptaskname = Convert.ReplaceDBNull(dr("stoptaskname"))
        _consignee = Convert.ReplaceDBNull(dr("consignee"))
        _company = Convert.ReplaceDBNull(dr("company"))
        _companytype = Convert.ReplaceDBNull(dr("companytype"))
        _documentid = Convert.ReplaceDBNull(dr("documentid"))
        _documenttype = Convert.ReplaceDBNull(dr("documenttype"))
        _packtype = Convert.ReplaceDBNull(dr("packtype"))
        _numpacks = Convert.ReplaceDBNull(dr("numpacks"))
        _transportationclass = Convert.ReplaceDBNull(dr("transportationclass"))
        _stopdetvolume = Convert.ReplaceDBNull(dr("stopdetvolume"))
        _stopdetweight = Convert.ReplaceDBNull(dr("stopdetweight"))
        _stopdetvalue = Convert.ReplaceDBNull(dr("stopdetvalue"))
        _status = StopTaskStatusFromString(Convert.ReplaceDBNull(dr("status")))
        _comments = Convert.ReplaceDBNull(dr("comments"))
        _contactid = Convert.ReplaceDBNull(dr("contactid"))
        _chkpnt = Convert.ReplaceDBNull(dr("CHKPNT"))

        _confirmationtype = StopTaskConfirmationTypeFromString(Convert.ReplaceDBNull(dr("confirmationtype")))
        _confdocid = Convert.ReplaceDBNull(dr("confdocid"))
        _confirmationnumber = Convert.ReplaceDBNull(dr("confirmationnumber"))
        _confirmationlevel = Convert.ReplaceDBNull(dr("confirmationlevel"))
        _allowpartialloading = Convert.ReplaceDBNull(dr("allowpartialloading"))
        _allowpartialunloading = Convert.ReplaceDBNull(dr("allowpartialunloading"))
        _allowpartialpickup = Convert.ReplaceDBNull(dr("allowpartialpickup"))
        _confirmpackageatunloading = Convert.ReplaceDBNull(dr("confirmpackageatunloading"))
        _reasoncode = Convert.ReplaceDBNull(dr("REASONCODE"))
        _adddate = Convert.ReplaceDBNull(dr("adddate"))
        _adduser = Convert.ReplaceDBNull(dr("adduser"))
        _editdate = Convert.ReplaceDBNull(dr("editdate"))
        _edituser = Convert.ReplaceDBNull(dr("edituser"))


        _packagescollection = New RouteStopTaskPackagesCollection(Me)
        _itemscollection = New RouteStopTaskItemsCollection(Me)
    End Sub

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from routestoptask where " & WhereClause)
        Dim dt As DataTable = New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Detail Not Found")
        End If
        dr = dt.Rows(0)
        If Not IsDBNull(dr("SCHEDUALEDDATE")) Then _schedualeddate = dr("SCHEDUALEDDATE")
        If Not IsDBNull(dr("stoptaskid")) Then _stoptaskid = dr("stoptaskid")
        If Not IsDBNull(dr("stoptasktype")) Then _stoptasktype = StopTaskTypeFromString(dr("stoptasktype"))
        If Not IsDBNull(dr("stoptaskname")) Then _stoptaskname = dr("stoptaskname")
        If Not IsDBNull(dr("consignee")) Then _consignee = dr("consignee")
        If Not IsDBNull(dr("company")) Then _company = dr("company")
        If Not IsDBNull(dr("companytype")) Then _companytype = dr("companytype")
        If Not IsDBNull(dr("documentid")) Then _documentid = dr("documentid")
        If Not IsDBNull(dr("documenttype")) Then _documenttype = dr("documenttype")
        If Not IsDBNull(dr("packtype")) Then _packtype = dr("packtype")
        If Not IsDBNull(dr("numpacks")) Then _numpacks = dr("numpacks")
        If Not IsDBNull(dr("transportationclass")) Then _transportationclass = dr("transportationclass")
        If Not IsDBNull(dr("stopdetvolume")) Then _stopdetvolume = dr("stopdetvolume")
        If Not IsDBNull(dr("stopdetweight")) Then _stopdetweight = dr("stopdetweight")
        If Not IsDBNull(dr("stopdetvalue")) Then _stopdetvalue = dr("stopdetvalue")
        If Not IsDBNull(dr("status")) Then _status = StopTaskStatusFromString(dr("status"))
        If Not IsDBNull(dr("confirmationtype")) Then _confirmationtype = StopTaskConfirmationTypeFromString(dr("confirmationtype"))
        If Not IsDBNull(dr("comments")) Then _comments = dr("comments")
        _contactid = Convert.ReplaceDBNull(dr("contactid"))
        _chkpnt = Convert.ReplaceDBNull(dr("CHKPNT"))


        _confdocid = Convert.ReplaceDBNull(dr("confdocid"))
        _confirmationnumber = Convert.ReplaceDBNull(dr("confirmationnumber"))
        _confirmationlevel = Convert.ReplaceDBNull(dr("confirmationlevel"))
        _allowpartialloading = Convert.ReplaceDBNull(dr("allowpartialloading"))
        _allowpartialunloading = Convert.ReplaceDBNull(dr("allowpartialunloading"))
        _allowpartialpickup = Convert.ReplaceDBNull(dr("allowpartialpickup"))
        _confirmpackageatunloading = Convert.ReplaceDBNull(dr("confirmpackageatunloading"))
        _reasoncode = Convert.ReplaceDBNull(dr("REASONCODE"))
        If Not IsDBNull(dr("adddate")) Then _adddate = dr("adddate")
        If Not IsDBNull(dr("adduser")) Then _adduser = dr("adduser")
        If Not IsDBNull(dr("editdate")) Then _editdate = dr("editdate")
        If Not IsDBNull(dr("edituser")) Then _edituser = dr("edituser")

        _packagescollection = New RouteStopTaskPackagesCollection(Me)
        _itemscollection = New RouteStopTaskItemsCollection(Me)
    End Sub

    Public Sub SetStatus(ByVal pNewStatus As StopTaskStatus, ByVal pReasonCode As String, ByVal pUser As String)
        Dim oldStatus As String = _status
        _status = pNewStatus
        _reasoncode = pReasonCode
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update routestoptask set STATUS={0},REASONCODE={1},EDITDATE={2}, EDITUSER={3} Where {4}", _
            Made4Net.Shared.Util.FormatField(StopTaskStatusToString(_status)), Made4Net.Shared.Util.FormatField(_reasoncode), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)

        Dim em As EventManagerQ = New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.RouteStopTaskStatusChanged
        Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.RouteStopTaskStatusChanged)
        em.Add("EVENT", EventType.ToString())
        em.Add("ACTION", "RSTSETSTAT")
        em.Add("ACTIVITYTYPE", "RSTSETSTAT")
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ACTIVITYTIME", "0")
        em.Add("ROUTE", _routeid)
        em.Add("ROUTESTOP", _stopnumber)
        em.Add("ROUTESTOPTASK", _stoptaskid)
        em.Add("FROMSTATUS", oldStatus)
        em.Add("NOTES", "")
        em.Add("TOSTATUS", _status)
        em.Add("USERID", pUser)
        em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("ADDUSER", pUser)
        em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        em.Add("EDITUSER", pUser)
        em.Send("RSTSETSTAT")
    End Sub

    Public Sub SetConfirmationDocId(ByVal pDocId As String, ByVal pUser As String)
        _confdocid = pDocId
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update routestoptask set confdocid={0},EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_confdocid), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub SetConfirmationNumber(ByVal pConfirmNumber As String, ByVal pUser As String)
        _confirmationnumber = pConfirmNumber
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update routestoptask set confirmationnumber={0},EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_confirmationnumber), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub UpdateStopNumber(ByVal pNewStopNumber As Int32, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser
        Dim SQL As String = String.Format("update routestoptask set stopnumber={0},EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(pNewStopNumber), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
        _stopnumber = pNewStopNumber
        'Update All Documents the StopNumber
        Dim oOrd As OutboundOrderHeader
        If _consignee <> "" And _documentid <> "" Then
            If OutboundOrderHeader.Exists(_consignee, _documentid) Then
                oOrd = New OutboundOrderHeader(_consignee, _documentid)
                'oOrd.SetRoute(_routeid, pNewStopNumber, pUser)
            End If
        End If
    End Sub

    'Private Function GetContactID() As String
    '    Dim contactid As String
    '    If _stoptasktype = Logic.StopTaskType.General Then
    '        If RouteGeneralTask.Exists(_documentid) Then
    '            Dim oTask As New RouteGeneralTask(_documentid)
    '            Return oTask.ContactId
    '        End If
    '    Else
    '        Select Case _documenttype
    '            Case WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER
    '                If OutboundOrderHeader.Exists(_consignee, _documentid) Then
    '                    Dim oOrd As New OutboundOrderHeader(_consignee, _documentid)
    '                    Return oOrd.SHIPTOID
    '                End If
    '            Case WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
    '                If InboundOrderHeader.Exists(_consignee, _documentid) Then
    '                    Dim oOrd As New InboundOrderHeader(_consignee, _documentid)
    '                    Return oOrd.RECEIVEDFROMID
    '                End If
    '        End Select
    '    End If
    '    Return String.Empty
    'End Function

#End Region

#Region "Create/Update"

    Public Sub Create(ByVal pRouteId As String, ByVal pStopNumber As Int32, ByVal pStopTaskType As StopTaskType, ByVal pStopName As String, _
                ByVal pContactId As String, _
                ByVal pChkPnt As String, _
                ByVal pScheduleDate As DateTime, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pDocType As String, ByVal pCompany As String, ByVal pCompType As String, _
                ByVal pPackType As String, ByVal pNumPacks As Int32, ByVal pTransClass As String, ByVal pVolume As Double, ByVal pWeight As Double, ByVal pValue As Double, _
                ByVal pComments As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pUserId As String, Optional ByVal pStopTaskId As Int32 = -1)

        'Validate
        If pRouteId Is Nothing Or pRouteId = String.Empty Then
            Throw New M4NException(New Exception, "Cannot Add Stop.Invalid Route", "Cannot Add Stop.Invalid Route")
        End If

        Dim sql As String
        _routeid = pRouteId
        _stopnumber = pStopNumber
        _stoptaskid = pStopTaskId
        _stoptasktype = pStopTaskType
        _stoptaskname = pStopName
        _schedualeddate = pScheduleDate
        _consignee = pConsignee
        _documentid = pOrderid
        _documenttype = pDocType
        _company = pCompany
        _companytype = pCompType
        _packtype = pPackType
        _numpacks = pNumPacks
        _transportationclass = pTransClass
        _stopdetvolume = pVolume
        _stopdetvalue = pValue
        _stopdetweight = pWeight
        _status = StopTaskStatus.[New]
        _comments = pComments
        _contactid = pContactId
        _chkpnt = pChkPnt
        _confirmationtype = pConfirmationType
        _adddate = DateTime.Now
        _adduser = pUserId
        _editdate = DateTime.Now
        _edituser = pUserId

        If _stoptaskid = -1 Or DataInterface.ExecuteScalar("SELECT COUNT(1) FROM ROUTESTOPTASK WHERE ROUTEID=" & Made4Net.Shared.Util.FormatField(_routeid) & " AND STOPNUMBER=" & Made4Net.Shared.Util.FormatField(_stopnumber)) > 0 Then
            _stoptaskid = GetNextStopNumber()
        End If
        sql = String.Format("Insert into routestoptask(ROUTEID, STOPNUMBER, SCHEDUALEDDATE, COMPANY, COMPANYTYPE, STOPTASKID, STOPTASKTYPE, STOPTASKNAME, CONSIGNEE, DOCUMENTID, DOCUMENTTYPE, PACKTYPE, NUMPACKS, " & _
            "TRANSPORTATIONCLASS, STOPDETVOLUME, STOPDETWEIGHT, STOPDETVALUE, STATUS, COMMENTS, CONTACTID, CONFIRMATIONTYPE, ADDDATE, ADDUSER, EDITDATE, EDITUSER,chkpnt) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25})", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_schedualeddate), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(StopTaskTypeToString(_stoptasktype)), _
            Made4Net.Shared.Util.FormatField(_stoptaskname), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_documentid), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_packtype), Made4Net.Shared.Util.FormatField(_numpacks), Made4Net.Shared.Util.FormatField(_transportationclass), Made4Net.Shared.Util.FormatField(_stopdetvolume), _
            Made4Net.Shared.Util.FormatField(_stopdetweight), Made4Net.Shared.Util.FormatField(_stopdetvalue), Made4Net.Shared.Util.FormatField(StopTaskStatusToString(_status)), Made4Net.Shared.Util.FormatField(_comments), Made4Net.Shared.Util.FormatField(_contactid), _
            Made4Net.Shared.Util.FormatField(StopTaskConfirmationTypeToString(_confirmationtype)), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                        Made4Net.Shared.Util.FormatField(_chkpnt))
        DataInterface.RunSQL(sql)

        _packagescollection = New RouteStopTaskPackagesCollection(Me)
        _itemscollection = New RouteStopTaskItemsCollection(Me)

        'If OutboundOrderHeader.Exists(pConsignee, pOrderid) Then
        '    Dim rt As New Route(_routeid)
        '    oOrd.AssignToRoutingSet(rt.RouteSet, pUserId)
        '    oOrd.SetRoute(_routeid, _stopnumber, pUserId)
        '    oOrd = Nothing
        '    rt = Nothing
        'End If
    End Sub

    Public Sub Update(ByVal pStopTaskType As StopTaskType, ByVal pStopName As String, ByVal pScheduleDate As DateTime, ByVal pConsignee As String, _
                ByVal pOrderid As String, ByVal pDocType As String, ByVal pCompany As String, ByVal pCompType As String, ByVal pContactId As String, _
                 ByVal pChkPnt As String, _
                ByVal pPackType As String, ByVal pNumPacks As Int32, ByVal pTransClass As String, ByVal pVolume As Double, ByVal pWeight As Double, ByVal pValue As Double, _
                ByVal pComments As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pConfirmationLevel As String, _
                ByVal pAllowPartialLoading As Boolean, ByVal pAllowPartialUnLoading As Boolean, ByVal pAllowPartialPickups As Boolean, ByVal pConfirmPackageAtUnLoading As Boolean, ByVal pUserId As String)

        Dim sql As String
        _stoptasktype = pStopTaskType
        _stoptaskname = pStopName
        _schedualeddate = pScheduleDate
        _consignee = pConsignee
        _documentid = pOrderid
        _documenttype = pDocType
        _company = pCompany
        _companytype = pCompType
        _packtype = pPackType
        _numpacks = pNumPacks
        _transportationclass = pTransClass
        _stopdetvolume = pVolume
        _stopdetvalue = pValue
        _stopdetweight = pWeight
        _comments = pComments
        _contactid = pContactId
        _chkpnt = pChkPnt
        _confirmationtype = pConfirmationType
        _confirmationlevel = pConfirmationLevel
        _allowpartialloading = pAllowPartialLoading
        _allowpartialunloading = pAllowPartialUnLoading
        _allowpartialpickup = pAllowPartialPickups
        _confirmpackageatunloading = pConfirmPackageAtUnLoading
        _editdate = DateTime.Now
        _edituser = pUserId

        sql = String.Format("update routestoptask set ROUTEID={0}, STOPNUMBER={1}, COMPANY={2}, STOPTASKID={3}, STOPTASKTYPE={4}, STOPTASKNAME={5}, DOCUMENTID={6}, PACKTYPE={7}, NUMPACKS={8}, " & _
            "TRANSPORTATIONCLASS={9}, STOPDETVOLUME={10}, STOPDETWEIGHT={11}, STOPDETVALUE={12}, STATUS={13}, ADDDATE={14}, ADDUSER={15}, EDITDATE={16}, EDITUSER={17}, SCHEDUALEDDATE={18}, CONSIGNEE={19}, " & _
            "DOCUMENTTYPE={20}, COMPANYTYPE={21}, COMMENTS={22}, CONTACTID={23}, CONFIRMATIONTYPE={24},CONFIRMATIONLEVEL={25}, ALLOWPARTIALLOADING={26}, ALLOWPARTIALUNLOADING={27}, CONFIRMPACKAGEATUNLOADING={28}, allowpartialpickup={29},CHKPNT={31} Where {30}", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(StopTaskTypeToString(_stoptasktype)), _
            Made4Net.Shared.Util.FormatField(_stoptaskname), Made4Net.Shared.Util.FormatField(_documentid), Made4Net.Shared.Util.FormatField(_packtype), Made4Net.Shared.Util.FormatField(_numpacks), Made4Net.Shared.Util.FormatField(_transportationclass), Made4Net.Shared.Util.FormatField(_stopdetvolume), _
            Made4Net.Shared.Util.FormatField(_stopdetweight), Made4Net.Shared.Util.FormatField(_stopdetvalue), Made4Net.Shared.Util.FormatField(StopTaskStatusToString(_status)), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
            Made4Net.Shared.Util.FormatField(_schedualeddate), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_comments), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(StopTaskConfirmationTypeToString(_confirmationtype)), _
            Made4Net.Shared.Util.FormatField(_confirmationlevel), Made4Net.Shared.Util.FormatField(_allowpartialloading), Made4Net.Shared.Util.FormatField(_allowpartialunloading), Made4Net.Shared.Util.FormatField(_confirmpackageatunloading), Made4Net.Shared.Util.FormatField(_allowpartialpickup), WhereClause, Made4Net.Shared.Util.FormatField(_chkpnt))
        DataInterface.RunSQL(sql)

        _packagescollection = New RouteStopTaskPackagesCollection(Me)
        _itemscollection = New RouteStopTaskItemsCollection(Me)
    End Sub

    Public Sub Save(ByVal sUserId As String)
        Dim sql As String
        If _routeid Is Nothing Or _routeid = String.Empty Then
            Throw New M4NException(New Exception, "Cannot Add Stop.Invalid Route", "Cannot Add Stop.Invalid Route")
        End If
        If _status = StopTaskStatus.Completed Or _status = StopTaskStatus.Canceled Then
            Throw New M4NException(New Exception, "Cannot save task, incorrect status", "Cannot save task, incorrect status")
        End If
        If _stoptaskid = -1 Then
            _stoptaskid = GetNextStopNumber()
        End If
        'If _contactid = String.Empty Or _contactid Is Nothing Then
        '    _contactid = GetContactID()
        'End If
        If Not Exists(_routeid, _stopnumber, _stoptaskid) Then
            _adddate = DateTime.Now
            _adduser = sUserId
            _editdate = DateTime.Now
            _edituser = sUserId
            _status = StopTaskStatus.New
            sql = String.Format("Insert into routestoptask(ROUTEID, STOPNUMBER, SCHEDUALEDDATE, COMPANY, COMPANYTYPE, STOPTASKID, STOPTASKTYPE, STOPTASKNAME, CONSIGNEE, DOCUMENTID, DOCUMENTTYPE, PACKTYPE, NUMPACKS, " & _
            "TRANSPORTATIONCLASS, STOPDETVOLUME, STOPDETWEIGHT, STOPDETVALUE, STATUS, COMMENTS, CONTACTID, CONFIRMATIONTYPE, CONFIRMATIONLEVEL,ALLOWPARTIALLOADING,ALLOWPARTIALUNLOADING,ALLOWPARTIALPICKUP,CONFIRMPACKAGEATUNLOADING, ADDDATE, ADDUSER, EDITDATE, EDITUSER,CHKPNT) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30})", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_schedualeddate), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(StopTaskTypeToString(_stoptasktype)), _
            Made4Net.Shared.Util.FormatField(_stoptaskname), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_documentid), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_packtype), Made4Net.Shared.Util.FormatField(_numpacks), Made4Net.Shared.Util.FormatField(_transportationclass), Made4Net.Shared.Util.FormatField(_stopdetvolume), _
            Made4Net.Shared.Util.FormatField(_stopdetweight), Made4Net.Shared.Util.FormatField(_stopdetvalue), Made4Net.Shared.Util.FormatField(StopTaskStatusToString(_status)), Made4Net.Shared.Util.FormatField(_comments), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(StopTaskConfirmationTypeToString(_confirmationtype)), Made4Net.Shared.Util.FormatField(_confirmationlevel), _
            Made4Net.Shared.Util.FormatField(Me._allowpartialloading), Made4Net.Shared.Util.FormatField(Me._allowpartialunloading), Made4Net.Shared.Util.FormatField(_allowpartialpickup), Made4Net.Shared.Util.FormatField(Me._confirmpackageatunloading), _
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_chkpnt))
        Else
            _editdate = DateTime.Now
            _edituser = sUserId
            sql = String.Format("update routestoptask set ROUTEID={0}, STOPNUMBER={1}, COMPANY={2}, STOPTASKID={3}, STOPTASKTYPE={4}, STOPTASKNAME={5}, DOCUMENTID={6}, PACKTYPE={7}, NUMPACKS={8}, " & _
            "TRANSPORTATIONCLASS={9}, STOPDETVOLUME={10}, STOPDETWEIGHT={11}, STOPDETVALUE={12}, STATUS={13}, ADDDATE={14}, ADDUSER={15}, EDITDATE={16}, EDITUSER={17}, SCHEDUALEDDATE={18}, CONSIGNEE={19}, DOCUMENTTYPE={20}, COMPANYTYPE={21}, COMMENTS={22}, CONTACTID={23}, CONFIRMATIONTYPE={24} , CONFIRMATIONLEVEL={25} ,ALLOWPARTIALLOADING={26},ALLOWPARTIALUNLOADING={27},CONFIRMPACKAGEATUNLOADING={28},ALLOWPARTIALPICKUP={29},CHKPNT={31} Where {30}", _
            Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber), Made4Net.Shared.Util.FormatField(_company), Made4Net.Shared.Util.FormatField(_stoptaskid), Made4Net.Shared.Util.FormatField(StopTaskTypeToString(_stoptasktype)), _
            Made4Net.Shared.Util.FormatField(_stoptaskname), Made4Net.Shared.Util.FormatField(_documentid), Made4Net.Shared.Util.FormatField(_packtype), Made4Net.Shared.Util.FormatField(_numpacks), Made4Net.Shared.Util.FormatField(_transportationclass), Made4Net.Shared.Util.FormatField(_stopdetvolume), _
            Made4Net.Shared.Util.FormatField(_stopdetweight), Made4Net.Shared.Util.FormatField(_stopdetvalue), Made4Net.Shared.Util.FormatField(StopTaskStatusToString(_status)), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
            Made4Net.Shared.Util.FormatField(_schedualeddate), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_documenttype), Made4Net.Shared.Util.FormatField(_companytype), Made4Net.Shared.Util.FormatField(_comments), Made4Net.Shared.Util.FormatField(_contactid), Made4Net.Shared.Util.FormatField(StopTaskConfirmationTypeToString(_confirmationtype)), Made4Net.Shared.Util.FormatField(_confirmationlevel), Made4Net.Shared.Util.FormatField(_allowpartialloading), Made4Net.Shared.Util.FormatField(_allowpartialunloading), Made4Net.Shared.Util.FormatField(_confirmpackageatunloading), Made4Net.Shared.Util.FormatField(_allowpartialpickup), WhereClause, _
             Made4Net.Shared.Util.FormatField(_chkpnt))
        End If
        DataInterface.RunSQL(sql)
        _packagescollection = New RouteStopTaskPackagesCollection(Me)
        _itemscollection = New RouteStopTaskItemsCollection(Me)
    End Sub

    Public Sub Delete(ByVal pUserId As String)
        For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
            oStopPackage.Delete(pUserId)
        Next
        For Each oStopItem As RouteStopTaskItems In Me.StopItems
            oStopItem.Delete(pUserId)
        Next
        Dim SQL As String = String.Format("delete from routestoptask where {0}", WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Cancel / Confirm"

    Public Function Cancel(ByVal pUser As String)
        Dim oldStatus As String = _status
        If Me.isRouteDeparted Then
            If Not Me.Status = StopTaskStatus.Completed Then
                Select Case _stoptasktype
                    Case Logic.StopTaskType.Delivery, Logic.StopTaskType.PickUp

                        SetStatus(StopTaskStatus.Canceled, "", pUser)
                    Case Logic.StopTaskType.General
                        Dim oTask As New RouteGeneralTask(_documentid)
                        oTask.Cancel("", pUser)
                        SetStatus(StopTaskStatus.Canceled, "", pUser)
                End Select

                Dim em As EventManagerQ = New EventManagerQ
                Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.RouteStopTaskCancelled
                Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.RouteStopTaskStatusChanged)
                em.Add("EVENT", EventType.ToString())
                em.Add("ACTION", "RSTSETSTAT")
                em.Add("ACTIVITYTYPE", "RSTSETSTAT")
                em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                em.Add("ACTIVITYTIME", "0")
                em.Add("ROUTE", _routeid)
                em.Add("ROUTESTOP", _stopnumber)
                em.Add("ROUTESTOPTASK", _stoptaskid)
                em.Add("FROMSTATUS", oldStatus)
                em.Add("NOTES", "")
                em.Add("TOSTATUS", _status)
                em.Add("USERID", pUser)
                em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                em.Add("ADDUSER", pUser)
                em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                em.Add("EDITUSER", pUser)
                em.Send("RSTSETSTAT")
            Else
                Throw New M4NException(New Exception, "Task Status Incorrect ,Task Already Complete", "Task Status Incorrect ,Task Already Complete")
            End If
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function

    Public Function Confirm(ByVal pUser As String)
        SetStatus(StopTaskStatus.Completed, "", pUser)
    End Function

    Public Function Complete(ByVal pUser As String, ByVal DeliverAllPackages As Boolean)
        Dim oldStatus As String = _status
        If Me.isRouteDeparted Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery, Logic.StopTaskType.PickUp
                    If DeliverAllPackages Then
                        For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                            oStopPackage.Package.Deliver(pUser)
                        Next
                    End If
                    SetStatus(StopTaskStatus.Completed, "", pUser)
                Case Logic.StopTaskType.General
                    Dim oTask As New RouteGeneralTask(_documentid)
                    oTask.Complete(pUser)
                    SetStatus(StopTaskStatus.Completed, "", pUser)
            End Select

            Dim em As EventManagerQ = New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.RouteStopTaskCompleted
            Dim actType As String = WMS.Logic.WMSEvents.GetEventTransactionType(WMS.Logic.WMSEvents.EventType.RouteStopTaskStatusChanged)
            em.Add("EVENT", EventType.ToString())
            em.Add("ACTION", "RSTSETSTAT")
            em.Add("ACTIVITYTYPE", "RSTSETSTAT")
            em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            em.Add("ACTIVITYTIME", "0")
            em.Add("ROUTE", _routeid)
            em.Add("ROUTESTOP", _stopnumber)
            em.Add("ROUTESTOPTASK", _stoptaskid)
            em.Add("FROMSTATUS", oldStatus)
            em.Add("NOTES", "")
            em.Add("TOSTATUS", _status)
            em.Add("USERID", pUser)
            em.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            em.Add("ADDUSER", pUser)
            em.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            em.Add("EDITUSER", pUser)
            em.Send("RSTSETSTAT")
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function

    Public Function InComplete(ByVal pReasonCode As String, ByVal pUser As String)
        If Me.isRouteDeparted Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery, Logic.StopTaskType.PickUp
                    'For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                    '    oStopPackage.Package.OffLoad(pUser)
                    'Next
                    SetStatus(StopTaskStatus.Incomplete, pReasonCode, pUser)
                Case Logic.StopTaskType.General
                    Dim oTask As New RouteGeneralTask(_documentid)
                    oTask.InComplete(pReasonCode, pUser)
                    SetStatus(StopTaskStatus.Incomplete, pReasonCode, pUser)
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function
    Public Function Suspend(ByVal pReasonCode As String, ByVal pUser As String)
        If Me.isRouteDeparted Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery, Logic.StopTaskType.PickUp

                    SetStatus(StopTaskStatus.Suspended, pReasonCode, pUser)
                Case Logic.StopTaskType.General
                    Dim oTask As New RouteGeneralTask(_documentid)
                    oTask.InComplete(pReasonCode, pUser)
                    SetStatus(StopTaskStatus.Suspended, pReasonCode, pUser)
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function
#End Region

#Region "Stop Task Packages"

    Public Function AddStopTaskPackage(ByVal pPackageId As String, ByVal pUserId As String) As RouteStopTaskPackages
        Dim oRouteStopPackage As RouteStopTaskPackages
        If Me.StopPackages.RouteStopPackageExists(pPackageId) Then
            oRouteStopPackage = Me.StopPackages.GetRouteStopPackage(pPackageId)
        Else
            oRouteStopPackage = New RouteStopTaskPackages
            oRouteStopPackage.Create(_routeid, _stopnumber, _stoptaskid, pPackageId, pUserId)
            If Not _stoptasktype = Logic.StopTaskType.PickUp Then
                UpdateNumberOfPackages("AddPackage", pUserId)
            End If
            Me.StopPackages.Add(oRouteStopPackage)
            ' And if the package is offloaded - change its status to new so the delivery process will be available for it.
            Dim oPackage As New WMS.Logic.RoutePackage(pPackageId)
            If oPackage.Status.Equals(WMS.Lib.Statuses.RoutePackages.OFFLOADED, StringComparison.OrdinalIgnoreCase) Then
                oPackage.SetStatus(WMS.Lib.Statuses.RoutePackages.[NEW], pUserId)
            End If
        End If
        Return oRouteStopPackage
    End Function

    Public Sub RemoveStopTaskPackage(ByVal pPackageId As String, ByVal pUserId As String)
        Dim oRouteStopPackage As RouteStopTaskPackages
        If Me.StopPackages.RouteStopPackageExists(pPackageId) Then
            Me.StopPackages.Remove(oRouteStopPackage)
            UpdateNumberOfPackages("RemovePackage", pUserId)
            oRouteStopPackage = Me.StopPackages.GetRouteStopPackage(pPackageId)
            oRouteStopPackage.Delete(pUserId)
            If Me.StopPackages.Count = 0 Then
                Delete(pUserId)
            End If
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Remove Package - Package Does not exists", "Cannot Remove Package - Package Does not exists")
        End If
    End Sub

    Private Sub UpdateNumberOfPackages(ByVal pActionType As String, ByVal pUserId As String)
        _edituser = pUserId
        _editdate = DateTime.Now
        If pActionType = "AddPackage" Then
            _numpacks += 1
        ElseIf pActionType = "RemovePackage" Then
            _numpacks -= 1
        End If
        If _numpacks < 0 Then _numpacks = 0
        Dim SQL As String = String.Format("update routestoptask set numpacks={0},EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_numpacks), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Function DeliverPackage(ByVal pPackageId As String, ByVal pUser As String)
        Dim delivered As Boolean
        If Me.isRouteDeparted Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery
                    For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                        If oStopPackage.PackageId = pPackageId Then
                            oStopPackage.Package.Deliver(pUser)
                            delivered = True
                        End If
                    Next
                    If Not delivered Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                    End If
                    If Completed Then
                        SetStatus(StopTaskStatus.Completed, "", pUser)
                    End If
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function
    Public Function DeliverPackagePartial(ByVal pPackageId As String, ByVal pUser As String)
        Dim delivered As Boolean
        If Me.isRouteDeparted Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery
                    For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                        If oStopPackage.PackageId = pPackageId Then
                            oStopPackage.Package.DeliverPartial(pUser)
                            delivered = True
                        End If
                    Next
                    If Not delivered Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                    End If
                    
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function
    Public Function LoadPackage(ByVal pPackageId As String, ByVal pUser As String)
        Dim loaded As Boolean
        Select Case _stoptasktype
            Case Logic.StopTaskType.Delivery
                For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                    If oStopPackage.PackageId = pPackageId Then
                        oStopPackage.Package.Load(pUser)
                        loaded = True
                    End If
                Next
                If Not loaded Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                End If
        End Select
    End Function

    Public Function ReturnPackage(ByVal pPackageId As String, ByVal pReasonCode As String, ByVal pUser As String)
        Dim returned As Boolean
        If isRouteReturned() Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery
                    For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                        If oStopPackage.PackageId = pPackageId Then
                            oStopPackage.Package.OffLoad(pUser)
                            returned = True
                        End If
                    Next
                    If Not returned Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                    End If
                    'If Completed Then
                    '    SetStatus(StopTaskStatus.Completed, pReasonCode, pUser)
                    'End If
                Case Logic.StopTaskType.PickUp
                    For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                        If oStopPackage.PackageId = pPackageId Then
                            oStopPackage.Package.OffLoad(pUser)
                            returned = True
                        End If
                    Next
                    If Not returned Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                    End If
                    'If Me.NumPacks = Me.StopPackages.Count Then
                    'If Completed Then
                    'SetStatus(StopTaskStatus.Completed, pReasonCode, pUser)
                    'End If
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function

    Public Function UnloadPackageBeforeDeparting(ByVal pPackageId As String, ByVal pReasonCode As String, ByVal pUser As String)
        Dim bUnloaded As Boolean
        If UnloadingPackageBeforeDepartingAllowed() Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery
                    For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                        If oStopPackage.PackageId = pPackageId Then
                            oStopPackage.Package.OffLoad(pUser)
                            bUnloaded = True
                        End If
                    Next
                    If Not bUnloaded Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                    End If
                Case Logic.StopTaskType.PickUp
                    For Each oStopPackage As RouteStopTaskPackages In Me.StopPackages
                        If oStopPackage.PackageId = pPackageId Then
                            oStopPackage.Package.OffLoad(pUser)
                            bUnloaded = True
                        End If
                    Next
                    If Not bUnloaded Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Package does not belong to route task", "Package does not belong to route task")
                    End If
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function

    Public Function PickupPackage(ByVal pPackageId As String, ByVal pReasonCode As String, ByVal pUser As String)
        If isRouteDeparted() Then
            Select Case _stoptasktype
                Case Logic.StopTaskType.Delivery
                Case Logic.StopTaskType.PickUp
                    If Not Me.StopPackages.RouteStopPackageExists(pPackageId) Then
                        Dim _newPackage As New RoutePackage()
                        _newPackage.Create(pPackageId, "", Me.DocumentType, Me.DocumentId, Me.Consignee, pUser)
                        _newPackage.PickUp(pUser)
                        _newPackage = Nothing
                        Me.AddStopTaskPackage(pPackageId, pUser)
                    Else
                        Dim oNewPackage As New RoutePackage(pPackageId)
                        oNewPackage.PickUp(pUser)
                        oNewPackage = Nothing
                    End If
            End Select
        Else
            Throw New M4NException(New Exception, "Route Status Incorrect", "Route Status Incorrect")
        End If
    End Function

#End Region

#Region "Enum Conversion"

    Public Shared Function StopTaskTypeToString(ByVal sdt As StopTaskType) As String
        Select Case sdt
            Case StopTaskType.Delivery
                Return "Delivery"
            Case StopTaskType.PickUp
                Return "PickUp"
            Case StopTaskType.General
                Return "General"
            Case StopTaskType.ChkPnt
                Return "ChkPnt"
        End Select
    End Function

    Public Shared Function StopTaskTypeFromString(ByVal sStopTaskType As String) As StopTaskType
        Select Case sStopTaskType.ToLower
            Case "delivery"
                Return StopTaskType.Delivery
            Case "pickup"
                Return StopTaskType.PickUp
            Case "general"
                Return StopTaskType.General
        End Select
    End Function

    Public Function StopTaskStatusToString(ByVal sds As StopTaskStatus) As String
        Select Case sds
            Case StopTaskStatus.[New]
                Return "New"
            Case StopTaskStatus.Suspended
                Return "Suspended"
            Case StopTaskStatus.Canceled
                Return "Canceled"
            Case StopTaskStatus.Completed
                Return "Completed"
            Case StopTaskStatus.Incomplete
                Return "Incomplete"
        End Select
    End Function

    Public Function StopTaskStatusFromString(ByVal sStopTaskStatus As String) As StopTaskStatus
        Select Case sStopTaskStatus.ToLower
            Case "new"
                Return StopTaskStatus.[New]
            Case "suspended"
                Return StopTaskStatus.Suspended
            Case "canceled"
                Return StopTaskStatus.Canceled
            Case "completed"
                Return StopTaskStatus.Completed
            Case "incomplete"
                Return StopTaskStatus.Incomplete
        End Select
    End Function

    Public Shared Function StopTaskConfirmationTypeToString(ByVal sct As StopTaskConfirmationType) As String
        Select Case sct
            Case StopTaskConfirmationType.None
                Return ""
            Case StopTaskConfirmationType.ConfirmationNumbers
                Return "ConfirmationNumbers"
            Case StopTaskConfirmationType.ScanDocument
                Return "ScanDocument"
            Case StopTaskConfirmationType.Signature
                Return "Signature"
            Case Else
                Return ""
        End Select
    End Function

    Public Shared Function StopTaskConfirmationTypeFromString(ByVal sStopTaskConfirmationType As String) As StopTaskConfirmationType
        Select Case sStopTaskConfirmationType.ToLower
            Case "confirmationnumbers"
                Return StopTaskConfirmationType.ConfirmationNumbers
            Case "scandocument"
                Return StopTaskConfirmationType.ScanDocument
            Case "signature"
                Return StopTaskConfirmationType.Signature
            Case ""
                Return StopTaskConfirmationType.None
            Case Else
                Return StopTaskConfirmationType.None
        End Select
    End Function

#End Region

    Private Function isRouteDeparted() As Boolean
        If Route.GetRouteStatus(Me.RouteID) = RouteStatus.Departed Then Return True
        Return False
    End Function

    Private Function isRouteReturned() As Boolean
        If Route.GetRouteStatus(Me.RouteID) = RouteStatus.Returned Then Return True
        Return False
    End Function

    Private Function UnloadingPackageBeforeDepartingAllowed() As Boolean
        If Route.GetRouteStatus(Me.RouteID) >= RouteStatus.Returned Then Return False
        Return True
    End Function

#End Region

End Class

#Region "ENUMS"

Public Enum StopTaskType
    Delivery = 1
    PickUp = 2
    General = 3
    ChkPnt = 4
End Enum

Public Enum StopTaskStatus
    [New] = 1
    Suspended = 2
    Completed = 3
    Canceled = 4
    Incomplete = 5
End Enum

Public Enum StopTaskConfirmationType
    None = 0
    Signature = 1
    ConfirmationNumbers = 2
    ScanDocument = 3
End Enum

#End Region


