Imports WMS.Logic
Imports Made4Net.AppSessionManagement
Imports System.Text

Public Class AppPageUserLogin
    Inherits AppPageProcessor

    Private Enum ResponseCode
        LoginOK
        UnknownUser
        WrongPassword
        NoLicense
        UnknownError
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Try
            If Not oLogger Is Nothing Then
                oLogger.Write("Processing Login...")
                'PrintMessageContent(oLogger)
            End If
            Try
                Session.Timeout = Convert.ToInt32(WMS.Logic.GetSysParam("TalkMan_Logoff"))
            Catch ex As Exception
                Session.Timeout = 20
            End Try

            If Not WMS.Logic.User.Exists(_msg(0)("User").FieldValue) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Unkown User ({0}), returning error code.", _msg(0)("User").FieldValue))
                End If
                _responseCode = ResponseCode.UnknownUser
                _responseText = "Unkown User"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
           
            If _msg(0)("Password").FieldValue.Equals("") Then
                If WMS.Logic.GetSysParam("TalkMan_RequirePWD").Equals("1") Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Password is blank, returning error code.")
                    End If
                    _responseCode = ResponseCode.WrongPassword
                    _responseText = "Wrong Password"
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End If
            Else
                Try
                    If Not oLogger Is Nothing Then
                        Dim sb As New StringBuilder()

                        For index As Integer = 0 To _msg.ReceivedMessage.Length - 1
                            If Not Char.IsWhiteSpace(_msg.ReceivedMessage(index)) Then
                                sb.Append(_msg.ReceivedMessage(index))
                            End If
                        Next

                        oLogger.Write("received message's data : " & (sb.ToString()))
                        oLogger.writeSeperator()
                        oLogger.Write("User received in message: " & _msg(0)("User").FieldValue.Trim)
                    End If
                    Made4Net.Shared.Authentication.User.Login(_msg(0)("User").FieldValue.Trim, _msg(0)("Password").FieldValue.Trim)
                    '--
                    'if MultiDeviceLogin = allow or block
                    'If it is set to “Allow”, login process will continue normally.
                    'if whSQL = "0" then MultiDeviceLogin =  block
                    'If it is set to “Block”, the system will check if the user already exists in the warehouse activity table (WHACTIVITY).
                    'If true, the system will display and error message saying “user already logged in on another device” and return to the login screen.
                    Dim whSQL As String = "SELECT COUNT(1) FROM WAREHOUSEPARAMS where PARAMNAME = 'MultiDeviceLogin' and PARAMVALUE='Allow'"
                    whSQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL)
                    If whSQL = "0" Then
                        whSQL = String.Format("SELECT COUNT(1) FROM WHACTIVITY where USERID='{0}'", _msg(0)("User").FieldValue)
                        whSQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL)
                        If whSQL > "0" Then
                            '_responseCode = ResponseCode.UnknownError
                            '_responseText = "User already logged in on another device"
                            'Me.FillSingleRecord(oLogger, False)
                            '_resp(0)("Interleave").FieldValue = 0
                            'Return _resp
                            'Made4Net.Shared.Authentication.User.Logout()
                            Throw New Made4Net.Shared.M4NException("User already logged in on another device")
                        End If
                    End If

                    '--
                Catch ex As Made4Net.DataAccess.InvalidLicenseException
                    _responseCode = ResponseCode.NoLicense
                    _responseText = "No available license"
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Error Occured: " & ex.ToString())
                    End If
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                Catch ex As Made4Net.Shared.M4NException
                    _responseCode = ResponseCode.WrongPassword
                    _responseText = ex.Description
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Error Occured: " & ex.GetErrMessage(0))
                    End If
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                Catch ex As Exception
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Error Occured: " & ex.ToString())
                    End If
                    _responseCode = ResponseCode.WrongPassword
                    ' _responseText = "Wrong Password"
                    _responseText = ex.Message.ToString()
                    Me.FillSingleRecord(oLogger)
                    Return _resp

                End Try

            End If
            If Not Made4Net.DataAccess.DataInterface.IsLicensedUser(Made4Net.DataAccess.LicenseUtils.ConnectionContext.M4NAppSession) Then
                _responseCode = ResponseCode.NoLicense
                _responseText = "No Available License"
                If Not oLogger Is Nothing Then
                    oLogger.Write("No Available License Found...")
                End If

                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            Dim _user As New WMS.Logic.User(_msg(0)("User").FieldValue)
            Made4Net.Shared.Translation.Translator.CurrentLanguageID = Int32.Parse(_user.LANGUAGE)
            Dim oWHActivity As New WMS.Logic.WHActivity
            oWHActivity.ACTIVITY = "Login"
            oWHActivity.LOCATION = ""
            oWHActivity.USERID = _msg(0)("User").FieldValue
            oWHActivity.HETYPE = ""
            oWHActivity.ACTIVITYTIME = DateTime.Now
            oWHActivity.WAREHOUSEAREA = ""
            oWHActivity.ADDDATE = DateTime.Now
            oWHActivity.EDITDATE = DateTime.Now
            oWHActivity.ADDUSER = _msg(0)("User").FieldValue
            oWHActivity.EDITUSER = _msg(0)("User").FieldValue
            oWHActivity.HANDLINGEQUIPMENTID = ""
            oWHActivity.TERMINALTYPE = ""
            Session("ActivityID") = oWHActivity.Post()
            _responseCode = ResponseCode.LoginOK
            FillRecordsFromView(String.Format("username='{0}'", _user.USERID), oLogger)
        Catch ex As Made4Net.Shared.M4NException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Description
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

    Public Function IsSafetyCheckRequiredForMHE(ByVal pMHType As String) As Boolean
        Dim sql As String = String.Format("Select SafetyCheckRequired from HANDLINGEQUIPMENT Where HANDLINGEQUIPMENT={0}", Made4Net.Shared.FormatField(pMHType))
        If System.Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)) Then
            sql = String.Format("Select ISNULL(LASTHANDLINGEUQIPSAFETYCHECK,''),ISNULL(LASTSHIFTSAFETYCHECK,'') from userprofile  where userid={0}", Made4Net.Shared.FormatField(WMS.Logic.GetCurrentUser()))
            Dim dt As DataTable = New DataTable()
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.CONNECTION_NAME)
            Dim lastShiftSafetyCheck As String = dt.Rows(0).Item("LASTSHIFTSAFETYCHECK").ToString()
            Dim lastHandlingEquipment As String = dt.Rows(0).Item("LASTHANDLINGEUQIPSAFETYCHECK").ToString()
            Dim curShiftID As String = GetCurrentShift()
            If curShiftID <> lastShiftSafetyCheck Then
                Return True
            End If
            If pMHType <> lastHandlingEquipment Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Shared Function GetCurrentShift() As String
        Dim user As WMS.Logic.User = New WMS.Logic.User(WMS.Logic.GetCurrentUser())
        Dim SQL As String = String.Format("Select shiftid from shift where shiftcode='{0}' and status='{1}'", user.SHIFT, "STARTED")
        Dim curShiftID As String
        Try
            curShiftID = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL).ToString()
        Catch ex As Exception
            Throw New Exception("No shift is set for the entered user. Can not login.")
        End Try
        Return curShiftID
    End Function

End Class