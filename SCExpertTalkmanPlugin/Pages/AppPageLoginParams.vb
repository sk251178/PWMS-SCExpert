Imports WMS.Logic
Imports Made4Net.AppSessionManagement
Public Class AppPageLoginParams
    Inherits AppPageProcessor
    Private Enum ResponseCode
        LoginOK
        UserNotLoggedIn
        ShiftNotDefined
        DefaultWHAreaNotSet
        WrongHandlingEquipement
        WrongWarehouse
        UnknownError
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        PrintMessageContent(oLogger)
        Dim WH As String = "" '= _msg(0)("Warehouse").FieldValue
        Dim WHArea As String = _msg(0)("WarehouseArea").FieldValue.Trim
        Dim Location As String = _msg(0)("Location").FieldValue.Trim
        Dim MHTYPE As String = _msg(0)("MHTYPE").FieldValue.Trim
        Dim MHID As String = _msg(0)("MHID").FieldValue.Trim
        If Not oLogger Is Nothing Then
            oLogger.Write("Processing Login...")
            oLogger.Write("Warehouse area received in message: " & WHArea)
            oLogger.Write("Location received in message: " & Location)
            oLogger.Write("MHTYPE received in message: " & MHTYPE)
            oLogger.Write("MHID received in message: " & MHID)
        End If
        Try
            Session.Timeout = Convert.ToInt32(WMS.Logic.GetSysParam("TalkMan_Logoff"))
        Catch ex As Exception
            Session.Timeout = 20
        End Try
        Try
            If WMS.Logic.Common.GetCurrentUser Is Nothing OrElse WMS.Logic.Common.GetCurrentUser = "" Then
                oLogger.Write("No user is logged in for this session. Terminating request.")
                _responseCode = ResponseCode.UserNotLoggedIn
                _responseText = String.Format("No user is logged in for this session")
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If

            If String.IsNullOrEmpty(GetCurrentShift(GetCurrentUser)) Then
                _responseCode = ResponseCode.ShiftNotDefined
                _responseText = "User is not assigned any shift ID"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If

            If String.IsNullOrEmpty(Common.GetDefaultWareHouseArea(GetCurrentUser, oLogger)) Then
                _responseCode = ResponseCode.DefaultWHAreaNotSet
                _responseText = "Default Warehouse area is not set for the User"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If

            Dim _user As New WMS.Logic.User(WMS.Logic.Common.GetCurrentUser)
            Made4Net.Shared.Translation.Translator.CurrentLanguageID = Int32.Parse(_user.LANGUAGE)
            If Location.Equals("") Then
                Location = WMS.Logic.GetSysParam("TalkMan_LoginLocation")
            End If
            Session("LoginLocation") = Location
            If Not WH.Equals("") Then
                Dim whSQL As String = "SELECT count(*) FROM USERWAREHOUSE WHERE USERID='" & _user.USERID & "' AND WAREHOUSE='" & WH & "'"
                If Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL, "Made4NetSchema") = 0 Then
                    _responseCode = ResponseCode.WrongWarehouse
                    _responseText = String.Format("User Is Not Assigned To Warehouse")
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
                    If dt.Rows.Count = 0 Then
                        _responseText = String.Format("User Is Not Assigned To any Warehouse")
                    Else
                        _responseText = String.Format("User Is Assigned To More Than One Warehouse")
                    End If
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End If
            End If
            Session("WH") = WH
            Made4Net.Shared.Warehouse.setCurrentWarehouse(Session("WH"))
            Made4Net.DataAccess.DataInterface.ConnectionName = Made4Net.Shared.Warehouse.WarehouseConnection
            Warehouse.setUserWarehouseArea(WHArea)
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Warehouse {0} was set (warehouse area {1})...", WH, WHArea))
            End If
            If Not MHTYPE.Equals("") Then
                Session("MHType") = MHTYPE
                Session("MHEID") = MHID
            ElseIf Not MHID.Equals("") Then
                If Not setMHType(MHID) Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write(String.Format("Handling equipement {0} not found...", MHID))
                    End If
                    _responseCode = ResponseCode.WrongHandlingEquipement
                    _responseText = String.Format("Wrong Handling Equipement")
                    Me.FillSingleRecord(oLogger)
                    Return _resp
                End If
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
            oWHActivity.USERID = WMS.Logic.Common.GetCurrentUser
            oWHActivity.HETYPE = Session("MHType")
            oWHActivity.ACTIVITYTIME = DateTime.Now
            oWHActivity.WAREHOUSEAREA = WHArea
            oWHActivity.ADDDATE = DateTime.Now
            oWHActivity.EDITDATE = DateTime.Now
            oWHActivity.ADDUSER = WMS.Logic.Common.GetCurrentUser
            oWHActivity.EDITUSER = WMS.Logic.Common.GetCurrentUser
            oWHActivity.HANDLINGEQUIPMENTID = Session("MHEID")
            oWHActivity.TERMINALTYPE = Session("TERMINALTYPE")
            Session("ActivityID") = oWHActivity.Post()
            'MHE ID Safety check
            If IsSafetyCheckRequiredForMHE(Session("MHType")) Then
                _responseCode = ResponseCode.LoginOK
                FillSingleRecord(oLogger)
                _resp(0)("EquipmentSafetyCheck").FieldValue = 1
            Else
                _responseCode = ResponseCode.LoginOK
                FillSingleRecord(oLogger)
                _resp(0)("EquipmentSafetyCheck").FieldValue = 0
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Login Proccessed successfully.")
            End If
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

    Private Function setMHType(ByVal MHEID As String) As Boolean
        Dim mhSQL As String = "SELECT isnull(LABELPRINTER,'') as LABELPRINTER, isnull(MHETYPE,'') as MHETYPE  FROM MHE WHERE MHEID='" & MHEID & "'"
        Dim dtMH As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(mhSQL, dtMH)
        If dtMH.Rows.Count = 1 Then
            Session("MHEID") = MHEID
            Session("MHType") = dtMH.Rows(0)("MHETYPE")
            Session("MHEIDLabelPrinter") = dtMH.Rows(0)("LABELPRINTER")
            Return True
        Else
            'Throw New Made4Net.Shared.M4NException(New Exception, "MHE ID not found", "MHE ID not found")
            Return False
        End If
    End Function

    Public Function IsSafetyCheckRequiredForMHE(ByVal pMHType As String) As Boolean
        Dim sql As String = String.Format("Select SafetyCheckRequired from HANDLINGEQUIPMENT Where HANDLINGEQUIPMENT={0}", Made4Net.Shared.FormatField(pMHType))
        Dim user As String = GetCurrentUser()
        If System.Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)) Then
            sql = String.Format("Select LASTHANDLINGEUQIPSAFETYCHECK,LASTSHIFTSAFETYCHECK from userprofile where userid={0}", Made4Net.Shared.FormatField(user))
            Dim dt As DataTable = New DataTable()
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.CONNECTION_NAME)
            Dim lastShiftSafetyCheck As String = dt.Rows(0).Item("LASTSHIFTSAFETYCHECK").ToString()
            Dim lastHandlingEquipment As String = dt.Rows(0).Item("LASTHANDLINGEUQIPSAFETYCHECK").ToString()
            Dim curShiftID As String = GetCurrentShift(user)
            If curShiftID <> lastShiftSafetyCheck Then
                Return True
            End If
            If pMHType <> lastHandlingEquipment Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function GetCurrentShift(ByVal strUser As String) As String
        Dim curShiftID As String = Common.GetUserShift(strUser, Nothing)
        If String.IsNullOrEmpty(curShiftID) Then
            Throw New Exception("No shift is set for the entered user. Can not login.")
        End If
        Return curShiftID
    End Function



End Class
