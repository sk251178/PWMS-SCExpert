Imports WMS.Logic
Imports Made4Net.AppSessionManagement

Public Class AppPageUserLogin
    Inherits AppPageProcessor

    Private Enum ResponseCode
        LoginOK
        UnknownUser
        WrongPassword
        NoLicense
        WrongWarehouse
        UnknownError
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        _messageDate = Now.ToString("yyyy-MM-dd HH:mm:ss")  '''Validate that this line is in the base class.
        Dim WH As String = _msg(0)("Warehouse").FieldValue
        Dim Location As String = _msg(0)("Location").FieldValue
        Dim MHTYPE As String = _msg(0)("MHTYPE").FieldValue
        Dim MHID As String = _msg(0)("MHID").FieldValue
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Login...")
            oLogger.Write("Warehouse received in message: " & WH)
            oLogger.Write("Location received in message: " & Location)
            oLogger.Write("MHTYPE received in message: " & MHTYPE)
            oLogger.Write("MHID received in message: " & MHID)
        End If
        Try
            Session.Timeout = Convert.ToInt32(WMS.Logic.GetSysParam("TalkMan_Logoff"))
        Catch ex As Exception
        End Try
        Try
            If Not WMS.Logic.User.Exists(_msg(0)("User").FieldValue) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Unkown User, returning error code.")
                End If
                _responseCode = ResponseCode.UnknownUser
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            If _msg(0)("Password").FieldValue.Equals("") Then
                If WMS.Logic.GetSysParam("TalkMan_RequirePWD").Equals("1") Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Password is blank, returning error code.")
                    End If
                    _responseCode = ResponseCode.WrongPassword
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End If
            Else
                Try
                    If Not oLogger Is Nothing Then
                        oLogger.Write("User received in message: " & _msg(0)("User").FieldValue)
                    End If
                    If Made4Net.Shared.Authentication.User.IsLoggedIn Then
                        oLogger.Write("User already logedin for this session: " & Made4Net.Shared.Authentication.User.GetCurrentUser.UserName & ",logging off")
                        'Made4Net.Shared.Authentication.User.Logout()
                        Made4Net.Shared.UserLicense.DisconnectLicense()
                        Made4Net.DataAccess.DataInterface.CloseAllAppSessionConnections()
                        Made4Net.Shared.ContextSwitch.Current.Session("Made4NetLoggedInUserName") = Nothing
                        Made4Net.Shared.ContextSwitch.Current.Session("Made4NetLoggedInUserInfo") = Nothing
                        Made4Net.Shared.ContextSwitch.Current.Session("Made4NetLoggedInUserAddress") = Nothing
                    End If
                    Made4Net.Shared.Authentication.User.Login(_msg(0)("User").FieldValue, _msg(0)("Password").FieldValue)
                Catch ex As Exception
                    _responseCode = ResponseCode.WrongPassword
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End Try
            End If
            If Not Made4Net.DataAccess.DataInterface.IsLicensedUser(Made4Net.DataAccess.LicenseUtils.ConnectionContext.M4NAppSession) Then
                _responseCode = ResponseCode.NoLicense
                If Not oLogger Is Nothing Then
                    oLogger.Write("No Available License Found...")
                End If
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            Dim _user As New WMS.Logic.User(_msg(0)("User").FieldValue)
            Made4Net.Shared.Translation.Translator.CurrentLanguageID = Int32.Parse(_user.LANGUAGE)
            If Location.Equals("") Then
                Location = WMS.Logic.GetSysParam("TalkMan_LoginLocation")
            End If
            Session("LoginLocation") = Location
            If Not WH.Equals("") Then
                Dim whSQL As String = "SELECT count(*) FROM USERWAREHOUSE WHERE USERID='" & _user.USERID & "' AND WAREHOUSE='" & WH & "'"
                If Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL, "Made4NetSchema") = 0 Then
                    _responseCode = ResponseCode.WrongWarehouse
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End If
            Else
                Dim whSQL As String = "SELECT * FROM USERWAREHOUSE WHERE USERID='" & _user.USERID & "'"
                Dim dt As New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset(whSQL, dt, False, "Made4NetSchema")
                If dt.Rows.Count = 1 Then
                    WH = dt.Rows(0)("warehouse")
                Else
                    _responseCode = ResponseCode.WrongWarehouse
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End If
            End If
            Session("WH") = WH
            Warehouse.setCurrentWarehouse(Session("WH"))
            Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Warehouse {0} was set...", WH))
            End If
            If Not MHTYPE.Equals("") Then
                Session("MHType") = MHTYPE
                Session("MHEID") = MHID
            ElseIf Not MHID.Equals("") Then
                setMHType(MHID)
            Else
                Session("MHType") = WMS.Logic.GetSysParam("TalkMan_MHType")
                Session("MHEID") = MHID
            End If
            Session("TERMINALTYPE") = ""
            If Not oLogger Is Nothing Then
                oLogger.Write("Session Was set for all login params, trying to add user to activity table...")
            End If
            Dim oWHActivity As New WMS.Logic.WHActivity
            oWHActivity.ACTIVITY = "Login"
            oWHActivity.LOCATION = Session("LoginLocation")
            oWHActivity.USERID = _msg(0)("User").FieldValue
            oWHActivity.HETYPE = Session("MHType")
            oWHActivity.ACTIVITYTIME = DateTime.Now
            oWHActivity.ADDDATE = DateTime.Now
            oWHActivity.EDITDATE = DateTime.Now
            oWHActivity.ADDUSER = _msg(0)("User").FieldValue
            oWHActivity.EDITUSER = _msg(0)("User").FieldValue
            oWHActivity.HANDLINGEQUIPMENTID = Session("MHEID")
            oWHActivity.TERMINALTYPE = Session("TERMINALTYPE")
            Session("ActivityID") = oWHActivity.Post()
            _responseCode = ResponseCode.LoginOK
            FillRecordsFromView(String.Format("username='{0}'", _user.USERID), oLogger)
            If Not oLogger Is Nothing Then
                oLogger.Write("Login Proccessed successfully.")
            End If
        Catch ex As Made4Net.Shared.M4NException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.GetErrMessage
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.GetErrMessage(0))
            End If
            Me.FillSingleRecord(oLogger)
        Catch ex As ApplicationException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
        Catch ex As Exception
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
        End Try
        Return _resp
    End Function

    Private Sub setMHType(ByVal MHEID As String)
        Dim mhSQL As String = "SELECT isnull(LABELPRINTER,'') as LABELPRINTER, isnull(MHETYPE,'') as MHETYPE  FROM MHE WHERE MHEID='" & MHEID & "'"
        Dim dtMH As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(mhSQL, dtMH)
        If dtMH.Rows.Count = 1 Then
            Session("MHEID") = MHEID
            Session("MHType") = dtMH.Rows(0)("MHETYPE")
            Session("MHEIDLabelPrinter") = dtMH.Rows(0)("LABELPRINTER")
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "MHE ID not found", "MHE ID not found")
        End If
    End Sub
End Class