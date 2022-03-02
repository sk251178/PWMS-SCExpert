Imports System.Data
Imports System.Security.Cryptography
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Strings
Imports WMS.Lib

#Region "UserClass"

' <summary>
' This object represents the properties and methods of a USER.
' </summary>

<CLSCompliant(False)> Public Class User

#Region "Variables"

#Region "Primary Keys"

    Protected _userid As String = String.Empty

#End Region

#Region "Other Fields"

    'Protected _password As String = String.Empty
    Protected _firstname As String = String.Empty
    Protected _lastname As String = String.Empty
    Protected _fullname As String = String.Empty
    Protected _status As Boolean
    'Protected _expirationdate As DateTime
    Protected _language As String = String.Empty
    Protected _skin As String = String.Empty
    Protected _lastlogindate As DateTime
    Protected _defaultwarehouse As String = String.Empty
    Protected _defaultconsignee As String = String.Empty
    Protected _defaultscreenid As String = String.Empty
    'Protected _skilllevel As String = String.Empty
    'Protected _role As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _isAuthenticated As Boolean = False
    Protected _employmentstartdate As DateTime
    Protected _employmentstatus As String = String.Empty
    Protected _socialsecuritynumber As String = String.Empty
    Protected _phonenumber As String = String.Empty
    Protected _address As String = String.Empty
    Protected _shift As String = String.Empty
    Protected _shiftInstance As String = String.Empty
    'RWMS-2723   
    Protected _defaultprinter As String = String.Empty
    'RWMS-2723 END   

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " USERID = '" & _userid & "'"
        End Get
    End Property

    Public ReadOnly Property USERID() As String
        Get
            Return _userid
        End Get
    End Property

    Public ReadOnly Property IsAuthenticated() As Boolean
        Get
            Return _isAuthenticated
        End Get
    End Property

    Public ReadOnly Property EMPLOYMENTSTARTDATE() As DateTime
        Get
            Return _employmentstartdate
        End Get
    End Property
    Public ReadOnly Property EMPLOYMENTSTATUS() As String
        Get
            Return _employmentstatus
        End Get
    End Property
    Public ReadOnly Property SOCIALSECURITYNUMBER() As String
        Get
            Return _socialsecuritynumber
        End Get
    End Property

    Public ReadOnly Property PHONENUMBER() As String
        Get
            Return _phonenumber
        End Get
    End Property

    Public ReadOnly Property ADDRESS() As String
        Get
            Return _address
        End Get
    End Property

    Public Property FIRSTNAME() As String
        Get
            Return _firstname
        End Get
        Set(ByVal Value As String)
            _firstname = Value
        End Set
    End Property

    Public Property LASTNAME() As String
        Get
            Return _lastname
        End Get
        Set(ByVal Value As String)
            _lastname = Value
        End Set
    End Property

    Public Property FULLNAME() As String
        Get
            Return _fullname
        End Get
        Set(ByVal Value As String)
            _fullname = Value
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

    Public Property LANGUAGE() As String
        Get
            Return _language
        End Get
        Set(ByVal Value As String)
            _language = Value
        End Set
    End Property

    Public Property SKIN() As String
        Get
            Return _skin
        End Get
        Set(ByVal Value As String)
            _skin = Value
        End Set
    End Property

    Public Property LASTLOGINDATE() As DateTime
        Get
            Return _lastlogindate
        End Get
        Set(ByVal Value As DateTime)
            _lastlogindate = Value
        End Set
    End Property

    Public Property DEFAULTWAREHOUSE() As String
        Get
            Return _defaultwarehouse
        End Get
        Set(ByVal Value As String)
            _defaultwarehouse = Value
        End Set
    End Property

    Public Property DEFAULTCONSIGNEE() As String
        Get
            Return _defaultconsignee
        End Get
        Set(ByVal Value As String)
            _defaultconsignee = Value
        End Set
    End Property

    Public Property DEFAULTSCREENID() As String
        Get
            Return _defaultscreenid
        End Get
        Set(ByVal Value As String)
            _defaultscreenid = Value
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

    Public ReadOnly Property SHIFT() As String
        Get
            Return _shift
        End Get
    End Property

    Public ReadOnly Property CurrentShiftId() As String
        Get
            'Return _shiftInstance ''Since system is not using shift instance. 'RWMS-1662
            Return _shift
        End Get
    End Property

    'RWMS-2723   
    Public Property DEFAULTPRINTER() As String
        Get
            Return _defaultprinter
        End Get
        Set(ByVal Value As String)
            _defaultprinter = Value
        End Set
    End Property
    'RWMS-2723 END   


#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pUserId As String, Optional ByVal LoadObj As Boolean = True)
        _userid = pUserId

        If Not Exists(_userid) Then
            Throw New M4NException(New Exception, "User does not exist.", "User does not exist.")
        End If

        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName.ToLower = "setskillrole" Then
            _userid = dr("userid")
            UpdateSkillRole(dr("skill"), dr("role"), dr("wharea"))
        ElseIf CommandName.ToLower = "insertskillrole" Then
            _userid = dr("userid")
            InsertSkillRole(dr("skill"), dr("role"), dr("wharea"))
            'RWMS-2723   
        ElseIf CommandName.ToLower = "saveuserprofile" Then
            Dim oUser As New WMS.Logic.User(dr("userid"))
            _userid = dr("userid")
            _firstname = ds.Tables(0).Rows(0)("FIRSTNAME")
            _lastname = ds.Tables(0).Rows(0)("LASTNAME")
            _fullname = ds.Tables(0).Rows(0)("FULLNAME")
            If Not IsDBNull(ds.Tables(0).Rows(0)("STATUS")) Then _status = ds.Tables(0).Rows(0)("STATUS")

            If Not IsDBNull(ds.Tables(0).Rows(0)("EMPLOYMENTSTARTDATE")) Then _employmentstartdate = ds.Tables(0).Rows(0)("EMPLOYMENTSTARTDATE")
            If Not IsDBNull(ds.Tables(0).Rows(0)("EMPLOYMENTSTATUS")) Then _employmentstatus = ds.Tables(0).Rows(0)("EMPLOYMENTSTATUS")
            If Not IsDBNull(ds.Tables(0).Rows(0)("SOCIALSECURITYNUMBER")) Then _socialsecuritynumber = ds.Tables(0).Rows(0)("SOCIALSECURITYNUMBER")
            If Not IsDBNull(ds.Tables(0).Rows(0)("ADDRESS")) Then _address = ds.Tables(0).Rows(0)("ADDRESS")
            If Not IsDBNull(ds.Tables(0).Rows(0)("PHONENUMBER")) Then _phonenumber = ds.Tables(0).Rows(0)("PHONENUMBER")

            If Not IsDBNull(ds.Tables(0).Rows(0)("DEFAULTPRINTER")) Then _defaultprinter = ds.Tables(0).Rows(0)("DEFAULTPRINTER")

            If Not IsDBNull(ds.Tables(0).Rows(0)("DEFAULTWAREHOUSE")) Then _defaultwarehouse = ds.Tables(0).Rows(0)("DEFAULTWAREHOUSE")
            If Not IsDBNull(ds.Tables(0).Rows(0)("DEFAULTSCREEN")) Then _defaultscreenid = ds.Tables(0).Rows(0)("DEFAULTSCREEN")
            If Not IsDBNull(ds.Tables(0).Rows(0)("LANGUAGE")) Then _language = ds.Tables(0).Rows(0)("LANGUAGE")
            If Not IsDBNull(ds.Tables(0).Rows(0)("SKIN")) Then _skin = ds.Tables(0).Rows(0)("SKIN")
            If Not IsDBNull(ds.Tables(0).Rows(0)("LASTLOGINDATE")) Then _lastlogindate = ds.Tables(0).Rows(0)("LASTLOGINDATE")
            If Not IsDBNull(ds.Tables(0).Rows(0)("ADDUSER")) Then _adduser = ds.Tables(0).Rows(0)("ADDUSER")
            _edituser = WMS.Logic.Common.GetCurrentUser()
            UpdateProfile()

            'RWMS-2723 END   

        End If
    End Sub


#End Region

#Region "Methods"

    Public Shared Function GetUser(ByVal pUserId As String) As User
        Return New User(pUserId)
    End Function

    Protected Sub Load()
Start:

        Dim SQL As String = String.Format("SELECT * FROM {0} WHERE {1}", WMS.Lib.TABLES.USERPROFILE, WhereClause)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt, False, Made4Net.Schema.CONNECTION_NAME)
        If dt.Rows.Count = 0 Then
            CreateProfile(_userid)
            GoTo Start
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("FIRSTNAME") Then _firstname = dr.Item("FIRSTNAME")
        If Not dr.IsNull("LASTNAME") Then _lastname = dr.Item("LASTNAME")
        If Not dr.IsNull("FULLNAME") Then _fullname = dr.Item("FULLNAME")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("LANGUAGE") Then _language = dr.Item("LANGUAGE")
        If Not dr.IsNull("SKIN") Then _skin = dr.Item("SKIN")
        If Not dr.IsNull("LASTLOGINDATE") Then _lastlogindate = dr.Item("LASTLOGINDATE")
        If Not dr.IsNull("DEFAULTWAREHOUSE") Then _defaultwarehouse = dr.Item("DEFAULTWAREHOUSE")
        'If Not dr.IsNull("DEFAULTCONSIGNEE") Then _defaultconsignee = dr.Item("DEFAULTCONSIGNEE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("EMPLOYMENTSTARTDATE") Then _edituser = dr.Item("EMPLOYMENTSTARTDATE")
        If Not dr.IsNull("EMPLOYMENTSTATUS") Then _edituser = dr.Item("EMPLOYMENTSTATUS")
        If Not dr.IsNull("SOCIALSECURITYNUMBER") Then _edituser = dr.Item("SOCIALSECURITYNUMBER")
        If Not dr.IsNull("PHONENUMBER") Then _edituser = dr.Item("PHONENUMBER")

        _shift = Common.GetUserShift(_userid, Nothing)
        _shiftInstance = _shift 'RWMS-1667
        'RWMS-2723   
        If Not dr.IsNull("DEFAULTPRINTER") Then _defaultprinter = dr.Item("DEFAULTPRINTER")
        'RWMS-2723 END   


        ' _shiftInstance = DataInterface.ExecuteScalar(String.Format("Select shiftid from shift where shiftcode='{0}' and status='STARTED'", _shift))
    End Sub

    'Public Function Login(ByVal uPwd As String) As Boolean
    '	'If Cryptor.Decrypt(PASSWORD) = uPwd Then
    '	If Cryptor.Encrypt(uPwd) = PASSWORD Then
    '		_isAuthenticated = True
    '		Dim sql As String
    '		_lastlogindate = DateTime.Now
    '		sql = "update USERS set LastLoginDate = '" & Made4Net.Shared.Util.DateTimeToDbString(_lastlogindate) & "' Where " & WhereClause
    '		DataInterface.RunSQL(sql, Made4Net.Schema.CONNECTION_NAME)
    '	Else
    '		_isAuthenticated = False
    '	End If
    '	Return _isAuthenticated
    'End Function

    Public Function CreateProfile(ByVal uId As String) As Boolean
        If User.ProfileExists(uId) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "User already exists", "User already exists")
            Throw m4nEx
        End If
        Dim SQL As String, Flds As String, Vals As String

        Flds = "UserID,FIRSTNAME,LASTNAME,FULLNAME,STATUS," _
        & "LANGUAGE,DEFAULTWAREHOUSE,DEFAULTCONSIGNEE,DEFAULTSCREEN," _
        & "ADDDATE,ADDUSER,EDITDATE,EDITUSER"

        Vals = String.Format("'{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','','{9}',''" _
        , PSQ(uId), PSQ(FIRSTNAME), PSQ(LASTNAME), PSQ(FULLNAME), Convert.ToInt32(STATUS).ToString() _
        , PSQ(LANGUAGE), PSQ(DEFAULTWAREHOUSE), PSQ(DEFAULTCONSIGNEE), PSQ(DEFAULTSCREENID), Conversion.Convert.DateTimeToDBFormat(DateTime.Now))

        SQL = String.Format("INSERT INTO {0} ({1}) Values ({2})", WMS.Lib.TABLES.USERPROFILE, Flds, Vals)

        DataInterface.RunSQL(SQL, Made4Net.Schema.CONNECTION_NAME)
        _userid = uId
    End Function

    Private Function Exists() As Boolean
        If (DataInterface.ExecuteScalar("Select count(1) from USERS Where " & WhereClause, Made4Net.Schema.CONNECTION_NAME) = 0) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function Exists(ByVal uId As String) As Boolean
        If (DataInterface.ExecuteScalar(String.Format("Select count(1) from Sys_USERS Where Username='{0}'", uId), Made4Net.Schema.CONNECTION_NAME) = 0) Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function ProfileExists(ByVal uId As String) As Boolean
        If (DataInterface.ExecuteScalar(String.Format("Select count(1) from {0} WHERE UserID = '{1}'", WMS.Lib.TABLES.USERPROFILE, uId), Made4Net.Schema.CONNECTION_NAME) = 0) Then
            Return False
        Else
            Return True
        End If
    End Function

    'Public Shared Function Authenticated(ByVal uId As String, ByVal uPwd As String)
    '	Dim sql As String = "Select password from users where USERID = '" & uId & "'"
    '	Dim tPwd As String
    '	If User.Exists(uId) Then
    '		tPwd = DataInterface.ExecuteScalar(sql, Made4Net.Schema.CONNECTION_NAME)
    '		If Cryptor.Encrypt(uPwd) = tPwd Then
    '			Return True
    '		Else
    '			Return False
    '		End If
    '	Else
    '		Return False
    '	End If
    'End Function

    'Public Shared Sub RenewPassword(ByVal uId As String, ByVal NewPassword As String)
    '	Dim days As Int32
    '	Try
    '		days = Int32.Parse(GetSysParam("PSWDVALDAY"))
    '	Catch ex As Exception
    '	End Try
    '	If days = 0 Then days = 30

    '	Dim ExpDate As DateTime
    '	ExpDate = Now().AddDays(days)

    '	Dim SQL As String = String.Format("" _
    '	 & "UPDATE {0} SET PASSWORD = '{1}', EXPIRATIONDATE = '{2}' WHERE USERID = '{3}'" _
    '	 , TABLES.USERPROFILE, Cryptor.Encrypt(NewPassword), Made4Net.Shared.Util.DateTimeToDbString(ExpDate), uId)

    '	DataInterface.RunSQL(SQL, Made4Net.Schema.CONNECTION_NAME)

    'End Sub

    Public Sub Update(ByVal pFirstName As String, ByVal pLastName As String, ByVal pFullName As String, ByVal pWarehouse As String, _
       ByVal pDefaultScreenId As String, ByVal pLang As Int32, ByVal pSkin As String, ByVal pUser As String)
        _firstname = pFirstName
        _lastname = pLastName
        _fullname = pFullName
        '_defaultconsignee = pConsignee
        _language = pLang
        _skin = pSkin
        _defaultwarehouse = pWarehouse
        _defaultscreenid = pDefaultScreenId
        _editdate = DateTime.Now
        _edituser = pUser

        Dim nvcol As New System.Collections.Specialized.NameValueCollection
        nvcol.Add("FIRSTNAME", Made4Net.Shared.Util.FormatField(_firstname))
        nvcol.Add("LASTNAME", Made4Net.Shared.Util.FormatField(_lastname))
        nvcol.Add("FULLNAME", Made4Net.Shared.Util.FormatField(_fullname))
        nvcol.Add("STATUS", Made4Net.Shared.Util.FormatField(_status))
        nvcol.Add("LANGUAGE", Made4Net.Shared.Util.FormatField(_language))
        nvcol.Add("SKIN", "'" & _skin & "'")
        nvcol.Add("DEFAULTWAREHOUSE", Made4Net.Shared.Util.FormatField(_defaultwarehouse))
        'nvcol.Add("DEFAULTCONSIGNEE", Made4Net.Shared.Util.FormatField(_defaultconsignee))
        nvcol.Add("DEFAULTSCREEN", Made4Net.Shared.Util.FormatField(_defaultscreenid))
        nvcol.Add("EDITDATE", Made4Net.Shared.Util.FormatField(_editdate))
        nvcol.Add("EDITUSER", Made4Net.Shared.Util.FormatField(_edituser))

        PostUpdate(nvcol)
    End Sub

    Private Sub PostUpdate(ByVal col As System.Collections.Specialized.NameValueCollection)
        Dim SQL As String = Made4Net.DataAccess.SQLStatement.CreateUpdateStatement(WMS.Lib.TABLES.USERPROFILE, col, WhereClause)
        DataInterface.RunSQL(SQL, Made4Net.Schema.CONNECTION_NAME)

    End Sub
    'RWMS-2723   
    'Public Sub UpdateProfile(ByVal pFirstName As String, ByVal pLastName As String, ByVal pFullName As String, ByVal pStatus As String, ByVal pLang As Int32, ByVal pSkin As String, ByVal pWarehouse As String, _   
    ' ByVal pDefaultScreenId As String, ByVal pUser As String)   
    Public Sub UpdateProfile()
        _editdate = DateTime.Now

        Dim nvcol As New System.Collections.Specialized.NameValueCollection
        nvcol.Add("FIRSTNAME", Made4Net.Shared.Util.FormatField(_firstname))
        nvcol.Add("LASTNAME", Made4Net.Shared.Util.FormatField(_lastname))
        nvcol.Add("FULLNAME", Made4Net.Shared.Util.FormatField(_fullname))
        nvcol.Add("STATUS", Made4Net.Shared.Util.FormatField(_status))

        nvcol.Add("EMPLOYMENTSTARTDATE", Made4Net.Shared.Util.FormatField(_employmentstartdate))
        nvcol.Add("EMPLOYMENTSTATUS", Made4Net.Shared.Util.FormatField(_employmentstatus))
        nvcol.Add("SOCIALSECURITYNUMBER", Made4Net.Shared.Util.FormatField(_socialsecuritynumber))
        nvcol.Add("ADDRESS", Made4Net.Shared.Util.FormatField(_address))
        nvcol.Add("PHONENUMBER", Made4Net.Shared.Util.FormatField(_phonenumber))

        nvcol.Add("DEFAULTPRINTER", Made4Net.Shared.Util.FormatField(_defaultprinter))

        nvcol.Add("LANGUAGE", Made4Net.Shared.Util.FormatField(_language))
        nvcol.Add("SKIN", "'" & _skin & "'")
        nvcol.Add("DEFAULTWAREHOUSE", Made4Net.Shared.Util.FormatField(_defaultwarehouse))
        'nvcol.Add("DEFAULTCONSIGNEE", Made4Net.Shared.Util.FormatField(_defaultconsignee))   
        nvcol.Add("DEFAULTSCREEN", Made4Net.Shared.Util.FormatField(_defaultscreenid))
        nvcol.Add("LASTLOGINDATE", Made4Net.Shared.Util.FormatField(_lastlogindate))
        nvcol.Add("EDITDATE", Made4Net.Shared.Util.FormatField(_editdate))
        nvcol.Add("ADDUSER", Made4Net.Shared.Util.FormatField(_adduser))
        nvcol.Add("EDITUSER", Made4Net.Shared.Util.FormatField(_edituser))

        PostUpdate(nvcol)
    End Sub
    'RWMS-2723 END   


    Public Function IsWarehouseAssigned() As Boolean
        Dim cnt As Int32
        Dim sql As String = String.Format("SELECT COUNT(1) AS WAREHOUSES FROM USERWAREHOUSE WHERE USERID='{0}'", _userid)
        Try
            cnt = DataInterface.ExecuteScalar(sql, Made4Net.Schema.CONNECTION_NAME)
            If cnt = 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub ApplyDefaultWarehouse()
        If Not IsWarehouseAssigned() Then
            Dim DefaultWHCode As String = Convert.ToString(DataInterface.ExecuteScalar("SELECT WAREHOUSEID FROM WAREHOUSE WHERE WAREHOUSECONNECTION='Default'", Made4Net.Schema.CONNECTION_NAME))
            If Not DefaultWHCode.Equals("") Then
                Dim sql As String = String.Format("INSERT INTO USERWAREHOUSE (USERID,WAREHOUSE) VALUES ('{0}','{1}')", Me.USERID, DefaultWHCode)
                DataInterface.RunSQL(sql, Made4Net.Schema.CONNECTION_NAME)
            End If

        End If
    End Sub

#Region "Skill & Role"

    Public Sub UpdateSkillRole(ByVal pSkill As String, ByVal pRole As String, ByVal pWHArea As String)
        Dim SQL As String
        SQL = String.Format("update USERSKILL SET ROLE = '{0}', SKILL = '{1}' where userid = '{2}' and WHAREA = '{3}'", pRole, pSkill, _userid, pWHArea)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub InsertSkillRole(ByVal pSkill As String, ByVal pRole As String, ByVal pWHArea As String)
        Dim SQL As String
        Dim exist As Boolean

        SQL = String.Format("select count(1) from USERSKILL where userid = '{0}' and WHAREA = '{1}'", _userid, pWHArea)
        exist = Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
        If exist Then
            Throw New M4NException(New Exception, "User Already has skill and role", "User Already has skill and role")
        Else
            SQL = String.Format("INSERT INTO USERSKILL (USERID, WHAREA, ROLE, SKILL) VALUES ('{0}','{1}','{2}','{3}') ", _userid, pWHArea, pRole, pSkill)
            DataInterface.RunSQL(SQL)
        End If
    End Sub
    Public Function HasRoleDefined(ByVal pUSERID, ByVal pWHArea) As Boolean
        Dim SQL As String
        SQL = String.Format("select count(1) from USERSKILL where userid = '{0}' and WHAREA = '{1}'", _userid, pWHArea)
        If Convert.ToInt32(DataInterface.ExecuteScalar(SQL)) > 0 Then
            Return True
        End If
        Return False
    End Function

    Public Sub DeleteSkillRole(ByVal pWHArea As String)
        Dim SQL As String
        SQL = String.Format("delete from USERSKILL where userid = {0} and WHAREA = {1}", Made4Net.Shared.FormatField(_userid), Made4Net.Shared.FormatField(pWHArea))
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Warehouse Area"

    Public Function AssignedWarehouseArea(ByVal pWHArea As String) As Boolean
        Dim sql As String
        sql = String.Format("select count(1) from USERWHAREA where userid = '{0}' and WHAREA='{1}'", _userid, pWHArea)
        Return Convert.ToBoolean(Convert.ToInt32(DataInterface.ExecuteScalar(sql)))
    End Function

    Public Sub AssignToWarehouseArea(ByVal pWHArea As String)
        Dim sql As String
        sql = String.Format("select count(1) from USERWHAREA where userid = '{0}' and WHAREA='{1}'", _userid, pWHArea)
        If Convert.ToInt32(DataInterface.ExecuteScalar(sql)) = 0 Then
            sql = String.Format("INSERT INTO USERWHAREA (USERID, WHAREA) VALUES ('{0}','{1}')", _userid, pWHArea)
            DataInterface.RunSQL(sql)
            'if this wh area is the default one, change it
            Dim defWHArea As String = DataInterface.ExecuteScalar(String.Format("select isnull(defaultwharea,'') from userwarehouse where userid='{0}'", _userid), Made4Net.Schema.CONNECTION_NAME)
            If String.IsNullOrEmpty(defWHArea) Then
                SetDefaultWarehouseArea(pWHArea)
            End If
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "User is already assigned to warehouse area", "User is already assigned to warehouse area")
        End If
    End Sub

    Public Sub UnAssignFromWarehouseArea(ByVal pWHArea As String)
        Dim sql As String = String.Format("delete from USERWHAREA where userid = '{0}' and WHAREA='{1}'", _userid, pWHArea)
        DataInterface.RunSQL(sql)
        'if this wh area is the default one, change it
        Dim defWHArea As String = DataInterface.ExecuteScalar(String.Format("select defaultwharea from userwarehouse where userid='{0}'", _userid), Made4Net.Schema.CONNECTION_NAME)
        If Not String.IsNullOrEmpty(defWHArea) AndAlso defWHArea.Equals(pWHArea, StringComparison.OrdinalIgnoreCase) Then
            Dim newDefWHArea As String = DataInterface.ExecuteScalar(String.Format("select top 1 isnull(WHAREA,'') from USERWHAREA where userid = '{0}'", _userid)) ', Made4Net.Schema.CONNECTION_NAME)
            SetDefaultWarehouseArea(newDefWHArea)
        End If
        'Delete user work regions
        sql = String.Format("delete FROM USERWORKREGION where USERID = '{0}' and WAREHOUSEAREA = '{1}'", _userid, pWHArea)
        DataInterface.RunSQL(sql)
        'Delete user role and skill for the current wh area
        DeleteSkillRole(pWHArea)
    End Sub

    Public Sub SetDefaultWarehouseArea(ByVal pWarehouseArea As String)
        If Not pWarehouseArea Is Nothing AndAlso Not pWarehouseArea.Equals("") Then
            Dim sql As String = String.Format("update userwarehouse set defaultwharea='{0}' where userid='{1}'", pWarehouseArea, _userid)
            DataInterface.RunSQL(sql, Made4Net.Schema.CONNECTION_NAME)
        End If
    End Sub

#End Region

#End Region

End Class

#End Region

