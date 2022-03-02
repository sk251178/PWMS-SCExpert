Imports System
Imports System.Web
Imports WMS.Lib
Imports Made4Net.Shared.Web
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Strings
Imports Made4Net.Shared

Public Module Common

#Region "User Login"

    <CLSCompliant(False)>
    Public Function GetHashTableXMLString(ByVal pDataParameters As Made4Net.General.Serialization.HashTableWrapper) As String
        Dim xs As Made4Net.General.Serialization.XmlSerializer = New Made4Net.General.Serialization.XmlSerializer
        xs.IgnoreSerializableAttribute = True
        Dim dataParameterStr As String = xs.Serialize(pDataParameters)
        Return dataParameterStr
    End Function

    Public Function GetCurrentUser() As String
        Try
            If Made4Net.Shared.Authentication.User.IsLoggedIn Then
                Return Made4Net.Shared.Authentication.User.GetCurrentUser().UserName
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetCurrentUserDisplayName() As String
        Dim u As New WMS.Logic.User(WMS.Logic.GetCurrentUser())
        If u.FULLNAME Is Nothing OrElse u.FULLNAME.Length = 0 Then
            Return u.USERID
        Else
            Return u.FULLNAME
        End If
    End Function

    Public Function GetCurrentUserShiftId() As String
        Dim u As WMS.Logic.User
        Try
            u = New WMS.Logic.User(WMS.Logic.GetCurrentUser())
        Catch ex As Exception
            Return ""
        End Try

        Return u.CurrentShiftId


    End Function

    Public Function GetUserShift(ByVal strUser As String, logger As Made4Net.Shared.ILogHandler) As String
        Dim curShiftID As String = String.Empty
        Try
            Dim SQL As String = String.Format("Select isnull(shift,'') as shift from WHACTIVITY where userid = '{0}'", strUser)
            curShiftID = DataInterface.ExecuteScalar(SQL)
            If String.IsNullOrEmpty(curShiftID) Then
                SQL = String.Format("select isnull(DEFAULTSHIFT,'') DEFAULTSHIFT from USERWAREHOUSE where USERID='{0}'", strUser)
                curShiftID = DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME).ToString()
            End If
            If String.IsNullOrEmpty(curShiftID) Then
                logger.SafeWrite("No shift is set for the entered user")
            End If
        Catch ex As Exception
            logger.SafeWrite("Exception raised while retrieving the shiftID. Exception Details: " + ex.Message)
        End Try
        Return curShiftID
    End Function

    Public Function GetDefaultWareHouseArea(ByVal strUser As String, logger As Made4Net.Shared.ILogHandler) As String
        Dim defWarehouseArea As String = String.Empty
        Try
            Dim SQL = String.Format("select isnull(DEFAULTWHAREA,'') DEFAULTWHAREA from USERWAREHOUSE where USERID='{0}'", strUser)
            defWarehouseArea = DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME).ToString()
            If String.IsNullOrEmpty(defWarehouseArea) Then
                logger.SafeWrite("No default warehouse area set for the entered user")
            End If
        Catch ex As Exception
            logger.SafeWrite("Exception raised while retrieving the warehouse area. Exception Details: " + ex.Message)
        End Try
        Return defWarehouseArea
    End Function

    Public Sub SetCurrentUser(ByVal pUser As String)
        HttpContext.Current.Session("user") = pUser
    End Sub

    Public Sub EnsureUser()
        If GetCurrentUser() Is Nothing Then
            GotoLogin()
        End If
    End Sub

    Public Sub LogOut(Optional ByVal pGotoLogin As Boolean = True)
        'Added for RWMS-2540 Start
        Made4Net.Shared.ContextSwitch.Current.Session("LogoutUser") = Nothing
        Dim strUsername = WMS.Logic.GetCurrentUser
        Dim strUserWarehousearea = WMS.Logic.Warehouse.getUserWarehouseArea()
        'Added for RWMS-2540 End

        Made4Net.Shared.Web.User.Logout()

        'Added for RWMS-2540 Start
        Made4Net.Shared.ContextSwitch.Current.Session("LogoutUser") = 1
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LOGOUT)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOGOUT)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("USERID", strUsername)
        aq.Add("FROMWAREHOUSEAREA", strUserWarehousearea)
        aq.Add("TOWAREHOUSEAREA", strUserWarehousearea)

        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Send(WMS.Lib.Actions.Audit.LOGOUT)
        'Added for RWMS-2540 End

        If pGotoLogin Then
            GotoLogin()
        End If
    End Sub

    Public Sub GotoLogin()
        Dim url As String = MapVirtualPath(Made4Net.Shared.Web.M4N_SCREENS.LOGIN)
        If HttpContext.Current.Request.RawUrl.IndexOf(url) = -1 Then
            HttpContext.Current.Response.Redirect(url)
        End If
    End Sub

    Public Function GetWarehousesForCurrentUser() As SortedList
        Dim where As String = String.Format("USERID='{0}'", PSQ(GetCurrentUser()))
        Dim SQL As String = String.Format("SELECT {0}.WAREHOUSE, {1}.WAREHOUSEDESC FROM {0} INNER JOIN {1} ON {0}.WAREHOUSE = {1}.WAREHOUSE WHERE {2}", WMS.Lib.TABLES.USERWAREHOUSE, WMS.Lib.TABLES.WAREHOUSES, where)
        Dim D As New Made4Net.DataAccess.Data(SQL, Made4Net.Schema.CONNECTION_NAME)

        Dim DT As DataTable
        Try
            DT = D.CreateDataTable()
        Catch ex As NoDataFoundException
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "No warehouses are configured for user", "No warehouses are configured for user")
            m4nEx.Params.Add("user", GetCurrentUser())
            Throw m4nEx
        End Try

        Dim DR As DataRow, List As New SortedList
        For Each DR In DT.Rows
            List.Add(DR(1), DR(0))
        Next

        Return List
    End Function
#End Region

#Region "System Params"
    Public Function GetSysParam(ByVal pParamName As String) As Object
        'If pParamName Is Nothing OrElse pParamName.Length = 0 Then
        '    Throw New ArgumentNullException("pParamName")
        'End If

        'Dim val As Object
        'Dim where As String = String.Format("PARAM='{0}'", PSQ(pParamName))
        'val = DataInterface.DataLookup(TABLES.SYSTEMPARAMETERS, "PARAMVALUE", where, Made4Net.Schema.CONNECTION_NAME)
        'Return val
        Return Made4Net.Shared.Util.GetSystemParameterValue(pParamName)
    End Function

    Public Sub SetSysParam(ByVal pParamName As String, ByVal pValue As String)

    End Sub

    Public Function GetCopyrightText() As String
        Return Made4Net.Shared.AppConfig.Get("Copyright")
    End Function

    Public Function GetAppInfo() As String
        Dim info As String = String.Format("<P CLASS=AppInfo>Version: {0}<br>Licensed to: {1}<br> {2}</p>" _
        , GetSysParam("VERSION"), GetSysParam("LICENSEDTO"), GetCopyrightText())
        Return info
    End Function

    Public Function GetShortestPathLogger(ByVal logger As Made4Net.Shared.ILogHandler) As Made4Net.Shared.ILogHandler
        If (GetSysParam("ShortestPathCalculationLogger") <> "1") Then
            logger = Nothing
        End If
        Return logger
    End Function
#End Region

End Module