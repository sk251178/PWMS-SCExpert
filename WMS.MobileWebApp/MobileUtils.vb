Imports System
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Web
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web

<CLSCompliant(False)> Public Class MobileUtils

    'added for PWMS-303 Start
    Public Shared Function canOverrideLoad(ByVal fromload As String, ByVal toload As String, ByVal pcklst As WMS.Logic.Picklist, ByRef errmsg As String) As Boolean
        Dim ret As Boolean = True
        Dim sql As String
        Dim sql1 As String
        Dim sql2 As String
        Dim sql3 As String

        If Not WMS.Logic.Load.ValidLoadExists(toload) Then
            errmsg = ("Substitute load does not exist")
            Return False
        End If
        'END of  PWMS-792 - RWMS-850
        If Not WMS.Logic.Load.IsSubstituteLoadStatusMatch(fromload, toload) Then
            errmsg = ("Loads must have same status")
            Return False
        End If

        If fromload = toload Then
            errmsg = ("Cannot substitute a similar load")
            Return False
        End If

        Dim activityStatus As String = String.Empty
        activityStatus = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select isnull(ACTIVITYSTATUS,'') as ACTIVITYSTATUS from LOADS where LOADID='{0}'", toload))

        If Not activityStatus = String.Empty And Not activityStatus.ToUpper() = "REPLPEND" Then
            errmsg = ("Cannot substitute a load unless the activity status is Null or Empty or replpend")
            Return False
        End If


        Dim loadAttribute As New Dictionary(Of String, Dictionary(Of String, String))

        Dim excludelist As String = "'PKEYTYPE', 'PKEY2', 'PKEY3'"


        sql = String.Format("select Name from sys.columns where OBJECT_NAME(object_id)='attribute' and name not in ({0})", excludelist)

        Dim dt As New DataTable

        DataInterface.FillDataset(sql, dt)

        Dim columnList As String = String.Empty

        columnList = String.Join(",", (From row In dt.AsEnumerable Select row("Name")).ToArray)

        sql1 = String.Format("SELECT {0} FROM ATTRIBUTE WHERE PKEYTYPE='LOAD' and PKEY1 in ('{1}','{2}')", columnList, fromload, toload)

        Dim dt1 As New DataTable

        DataInterface.FillDataset(sql1, dt1)
        'Below condition will execute when both Original Load and Substitute Load does not have an entry in attribute table.
        If dt1.Rows.Count = 0 Then
            Return True
            'Below condition will execute when Substitute Load has an entry in attribute table but Original Load does not have entry.
        ElseIf dt1.Rows.Count = 1 And dt1.Rows(0).Item("PKEY1") = toload Then
            Return True
            'Below condition will execute when either both Original Load and Substitute Load have an entry in attribute table or only Original Load has an entry in attribute table.
        ElseIf dt1.Rows.Count = 2 Or dt1.Rows(0).Item("pkey1") = fromload Then

            Dim key As String
            Dim valueToRemove As String = "PKEY1"

            For Each dr As DataRow In dt1.Rows
                key = dr("PKEY1")
                Dim attribute As Dictionary(Of String, String) = New Dictionary(Of String, String)()
                For Each dc As DataColumn In dt1.Columns
                    If (dr(dc.ColumnName)) Is DBNull.Value Then
                        attribute(dc.ColumnName) = String.Empty
                    Else
                        attribute(dc.ColumnName) = (dr(dc.ColumnName))
                    End If

                Next

                loadAttribute(key) = attribute

            Next


            Dim attributesToCompare As New List(Of String)

            Dim columnList_New As String = String.Empty
            columnList_New = String.Join(", ", From v In columnList.Split(",")
                                               Where v.Trim() <> valueToRemove
                                               Select v)


            sql2 = String.Format("SELECT {0} FROM (SELECT sc.* FROM PLANSTRATEGYSCORING sc join PICKHEADER ph on sc.STRATEGYID = ph.STRATEGYID WHERE ph.PICKLIST = '{1}')t", columnList_New, pcklst.PicklistID)

            Dim dt2 As New DataTable

            DataInterface.FillDataset(sql2, dt2)
            For Each dr As DataRow In dt2.Rows
                For Each dc As DataColumn In dt2.Columns
                    If Not IsDBNull(dr(dc.ColumnName)) Then
                        'If String.Compare(attribute(dc.ColumnName), Convert.ToString(dr(dc.ColumnName))) = 0 Then
                        attributesToCompare.Add(dc.ColumnName)
                        'End If
                        Continue For
                    End If
                Next
            Next

            sql3 = String.Format("select {0} from (select oa.* from OUTBOUNDORDETAILATTRIBUTE oa join PICKDETAIL pd on oa.ORDERID = pd.ORDERID and oa.ORDERLINE = pd.ORDERLINE where PICKLIST = '{1}')t", columnList_New, pcklst.PicklistID)
            Dim dt3 As New DataTable

            DataInterface.FillDataset(sql3, dt3)

            For Each dr As DataRow In dt3.Rows
                For Each dc As DataColumn In dt3.Columns
                    If Not IsDBNull(dr(dc.ColumnName)) Then
                        attributesToCompare.Add(dc.ColumnName)
                        Continue For
                    End If
                Next
            Next

            'Below condition will execute when Only Original Load has an entry in attribute table and policy does not have weightage for any attribute.
            If dt1.Rows.Count = 1 And dt1.Rows(0).Item("pkey1") = fromload Then

                If attributesToCompare.Count = 0 Then
                    Return True

                End If

            End If

            Dim fValue, tValue As String
            For Each attr As String In attributesToCompare
                loadAttribute(fromload).TryGetValue(attr, fValue)
                loadAttribute(toload).TryGetValue(attr, tValue)
                If (String.Compare(fValue, tValue) <> 0) Then
                    errmsg = "Loads must have similar attributes"
                    Return False
                End If
            Next

            'Added RWMS-1373,RWMS-1544 and RWMS-1263,RWMS-1498 Start
            If dt1.Rows.Count > 0 And dt1.Rows(0).Item("PKEY1") = toload Then
                Dim customerShelflife? As Integer = Nothing
                Dim dtShelflife As New DataTable
                Dim ShelflifeSQL As String
                ShelflifeSQL = String.Format("SELECT CE.EXPIRATIONDAYS,ISNULL(OO.REQUESTEDDATE,GETDATE()) REQUESTEDDATE FROM PICKDETAIL PD INNER JOIN OUTBOUNDORHEADER OO ON PD.CONSIGNEE=OO.CONSIGNEE AND " &
                "PD.ORDERID=OO.ORDERID LEFT JOIN CUSTOMEREXPIRATIONDAYS CE ON OO.COMPANYTYPE=CE.COMPANYTYPE AND OO.TARGETCOMPANY=CE.COMPANY AND PD.SKU=CE.SKU AND PD.CONSIGNEE=CE.CONSIGNEE WHERE PD.PICKLIST='{0}'", pcklst.PicklistID)
                DataInterface.FillDataset(ShelflifeSQL, dtShelflife)
                If Not dt1.Rows(0).Item("EXPIRYDATE") Is Nothing And Not IsDBNull(dt1.Rows(0).Item("EXPIRYDATE")) Then
                    Dim attExpDate As DateTime = Convert.ToDateTime(dt1.Rows(0).Item("EXPIRYDATE"))
                    If dtShelflife.Rows.Count > 0 Then
                        If Not dtShelflife.Rows(0).Item("EXPIRATIONDAYS") Is Nothing And Not IsDBNull(dtShelflife.Rows(0).Item("EXPIRATIONDAYS")) Then
                            customerShelflife = Convert.ToInt32(dtShelflife.Rows(0).Item("EXPIRATIONDAYS"))
                        End If

                        If Not customerShelflife Is Nothing And customerShelflife <> 0 Then
                            If attExpDate < DateTime.Now.AddDays(customerShelflife).Date Then
                                errmsg = "ExpirtyDate of substitute load should be greater than '" & DateTime.Now.AddDays(customerShelflife).Date & "'"
                                Return False
                            End If
                        End If
                    End If
                End If
                If Not dt1.Rows(0).Item("MFGDATE") Is Nothing And Not IsDBNull(dt1.Rows(0).Item("MFGDATE")) Then
                    Dim skuAttributeShelflife As Integer
                    Dim skuAttributeShelflifeSQL As String
                    Dim attMfgDate As DateTime = Convert.ToDateTime(dt1.Rows(0).Item("MFGDATE"))
                    skuAttributeShelflifeSQL = String.Format("SELECT TOP 1 ISNULL(SHELFLIFE,0) SHELFLIFE from SKUATTRIBUTE SA INNER JOIN LOADS LD ON SA.SKU=LD.SKU AND SA.CONSIGNEE =LD.CONSIGNEE WHERE LD.LOADID='{0}'", toload)
                    skuAttributeShelflife = Made4Net.DataAccess.DataInterface.ExecuteScalar(skuAttributeShelflifeSQL)
                    If dtShelflife.Rows.Count > 0 Then
                        If Not dtShelflife.Rows(0).Item("EXPIRATIONDAYS") Is Nothing And Not IsDBNull(dtShelflife.Rows(0).Item("EXPIRATIONDAYS")) Then
                            customerShelflife = Convert.ToInt32(dtShelflife.Rows(0).Item("EXPIRATIONDAYS"))
                        End If

                        If Not customerShelflife Is Nothing And customerShelflife <> 0 Then
                            customerShelflife = customerShelflife * (-1)
                            If attMfgDate < DateTime.Now.AddDays(customerShelflife).Date Then
                                errmsg = "MfgDate of substitute load should be greater than '" & DateTime.Now.AddDays(customerShelflife).Date & "'"
                                Return False
                            End If
                        End If
                    End If
                End If
            End If
            'Ended  RWMS-1373,RWMS-1544 and RWMS-1263,RWMS-1498 End

            Return True

        End If


    End Function


    Public Shared Function RequestTask(ByVal logger As ILogHandler) As WMS.Logic.Task
        Dim ts As New WMS.Logic.TaskManager
        Return ts.GetTaskFromTMService(WMS.Logic.Common.GetCurrentUser, False, logger)
    End Function

    'Added for RWMS-323
    Public Shared Function PLANSTRATEGYDESCRIPTION(ByVal StrategyId As String) As String '// pcklist.StrategyId

        Dim sql As String

        sql = String.Format("select top 1 DESCRIPTION from PLANSTRATEGYHEADER where STRATEGYID='{0}'", StrategyId)
        Try
            sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
            sql = ""
        End Try
        If IsNothing(sql) Then sql = ""

        Return sql

    End Function
    'End Added for RWMS-323

    Public Shared Function ExtractConfirmationValues(ByVal pConfirmType As String, ByVal Confirm As String, Optional ByVal Confirm2 As String = "", Optional ByVal WarehouseArea As String = "") As System.Collections.Specialized.NameValueCollection
        Dim nvc As New System.Collections.Specialized.NameValueCollection
        If String.IsNullOrEmpty(WarehouseArea) Then WarehouseArea = WMS.Logic.Warehouse.getUserWarehouseArea()
        Select Case pConfirmType
            Case WMS.Lib.CONFIRMATIONTYPE.LOAD
                nvc.Add("LOAD", Confirm)
            Case WMS.Lib.CONFIRMATIONTYPE.LOCATION
                nvc.Add("LOCATION", Confirm)
                nvc.Add("WAREHOUSEAREA", WarehouseArea)
            Case WMS.Lib.CONFIRMATIONTYPE.NONE
            Case WMS.Lib.CONFIRMATIONTYPE.SKU
                nvc.Add("SKU", Confirm)
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATION
                nvc.Add("LOCATION", Confirm)
                nvc.Add("SKU", Confirm2)
                nvc.Add("WAREHOUSEAREA", WarehouseArea)
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATIONUOM
                nvc.Add("LOCATION", Confirm)
                nvc.Add("SKU", Confirm2)
                nvc.Add("WAREHOUSEAREA", WarehouseArea)
                nvc.Add("UOM", Confirm)
            Case WMS.Lib.CONFIRMATIONTYPE.SKUUOM
                nvc.Add("SKU", Confirm)
                nvc.Add("UOM", Confirm2)
            Case WMS.Lib.CONFIRMATIONTYPE.UPC
                nvc.Add("UPC", Confirm)
        End Select
        Return nvc
    End Function

    Public Shared Function GetUrlWithParams(Optional ByVal ErrMultiLogIn As Boolean = False) As String
        Dim ret, urlparams As String
        Dim skin, wh, mheid, mhtype, terminaltype, wharea, locationid As String
        skin = HttpContext.Current.Session("Skin")
        wh = HttpContext.Current.Session("WH")
        mheid = HttpContext.Current.Session("MHEID")
        mhtype = HttpContext.Current.Session("MHTYPE")
        terminaltype = HttpContext.Current.Session("TERMINALTYPE")
        wharea = HttpContext.Current.Session("LoginWHArea")
        locationid = HttpContext.Current.Session("LoginLocation")
        urlparams = "?"
        If skin <> String.Empty Then
            urlparams = urlparams & String.Format("Skin={0}&", skin)
        End If
        If wh <> String.Empty Then
            urlparams = urlparams & String.Format("WH={0}&", wh)
        End If
        If mheid <> String.Empty Then
            urlparams = urlparams & String.Format("MHEID={0}&", mheid)
        End If
        If mhtype <> String.Empty Then
            urlparams = urlparams & String.Format("MHTYPE={0}&", mhtype)
        End If
        If terminaltype <> String.Empty Then
            urlparams = urlparams & String.Format("TERMINALTYPE={0}&", terminaltype)
        End If
        If wharea <> String.Empty Then
            urlparams = urlparams & String.Format("WAREHOUSEAREA={0}&", wharea)
        End If
        If locationid <> String.Empty Then
            urlparams = urlparams & String.Format("LOCATION={0}&", locationid)
        End If
        If ErrMultiLogIn Then
            'urlparams = urlparams & "ErrMultiLogIn=1&"
            HttpContext.Current.Session("ErrMultiLogIn") = 1
        End If
        ret = Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx") & urlparams.TrimEnd("&".ToCharArray())
        Return ret
    End Function

    Public Shared Function MultiLogIn(ByVal appId As String) As Boolean
        Dim ret As Boolean = True

        ':2013-04-28
        'if MultiDeviceLogin = allow or block
        'If it is set to “Allow”, login process will continue normally.
        'if whSQL = "0" then MultiDeviceLogin =  block
        'If it is set to “Block”, the system will check if the user already exists in the warehouse activity table (WHACTIVITY).
        'If true, the system will display and error message saying “user already logged in on another device” and return to the login screen.
        Dim whSQL As String = "SELECT COUNT(1) FROM WAREHOUSEPARAMS where PARAMNAME = 'MultiDeviceLogin' and PARAMVALUE='Block'"
        whSQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL)
        If whSQL = "1" Then
            whSQL = String.Format("SELECT COUNT(1) FROM WHACTIVITY where USERID='{0}'", WMS.Logic.GetCurrentUser)
            whSQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(whSQL)
            If whSQL > "0" Then
                Session_End(appId, False)

                Return False
            End If
        End If
        Return ret
    End Function

#Region "Methods for handling the session"

    Public Shared Sub Session_End(ByVal appId As String, ByVal fDelWH As Boolean)
        '''''''''''''''''''''''''''''''
        ' Handles the license manager '
        '''''''''''''''''''''''''''''''

        'remove  user WHACTIVITY
        If fDelWH Then
            [Global].userSessionManager.RemoveSession(HttpContext.Current.Session().Item("Made4NetLoggedInUserName"), HttpContext.Current.Session.SessionID, HttpContext.Current.Request.UserHostAddress, Logic.LogHandler.GetRDTLogger())
        End If


        'remove the current user + session from license server
        'userid = Made4Net.Shared.Web.User.GetCurrentUser.UserName
        'sessionid = HttpContext.Current.Session.SessionID
        'ipAddress = HttpContext.Current.Request.UserHostAddress
        Dim userid, sessionid, ipAddress, conn As String
        ' appId = System.Web.UI.Page.Application("Made4NetLicensing_ApplicationId")
        userid = HttpContext.Current.Session().Item("Made4NetLoggedInUserName")
        sessionid = HttpContext.Current.Session().SessionID
        ipAddress = HttpContext.Current.Session().Item("Made4NetLoggedInUserAddress")
        conn = Made4Net.DataAccess.DataInterface.ConnectionName

        Dim Session_Id As String = ipAddress & "_" & sessionid
        Dim key As String = "DisConnect" & "@" & userid & "@" & Session_Id & "@" & appId & "@" & conn
        Dim SQL As String = "select param_value from sys_param where param_code = 'LicenseServer'"
        Dim server As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        SQL = "select param_value from sys_param where param_code = 'LicenseServerPort'"
        Dim port As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL, "Made4NetSchema")
        Dim tcpClient As New Made4Net.Net.TCPIP.Client(server, port)
        Dim ret As Boolean = Convert.ToBoolean(tcpClient.SendRequest(key))

        'close all connections to the DB and release memory
        ' If fDelWH Then Made4Net.DataAccess.DataInterface.CloseAllConnections()

        'call loag off session + remove wh
        'If fDelWH Then Made4Net.Shared.Web.User.Logout()
        Try
            Made4Net.DataAccess.DataInterface.CloseAllConnections(HttpContext.Current.Session())
        Catch ex As Exception
        End Try
        'Made4Net.Mobile.Common.GoToMenu()
        'UnCommented for PWMS-593
        Made4Net.Shared.Authentication.User.Logout()
        'End UnCommented for PWMS-593
        'WMS.Logic.Common.GotoLogin()
        'HttpContext.Current.Response.Redirect("m4nScreens/Login.aspx")
    End Sub

    Public Shared Sub ClearCreateLoadProcessSession(Optional ByVal isDescreateReceiving As Boolean = False)
        If Not isDescreateReceiving Then
            HttpContext.Current.Session.Remove("CreateLoadRCN")
            HttpContext.Current.Session.Remove("CreateLoadRCNLine")
            HttpContext.Current.Session.Remove("CreateLoadConsignee")
            HttpContext.Current.Session.Remove("CreateLoadSKU")
            HttpContext.Current.Session.Remove("CreateLoadContainerID")
            HttpContext.Current.Session.Remove("CreateLoadLocation")
            HttpContext.Current.Session.Remove("CreateLoadLoadId")
            HttpContext.Current.Session.Remove("CreateLoadStatus")
            HttpContext.Current.Session.Remove("CreateLoadHoldRC")
            HttpContext.Current.Session.Remove("CreateLoadLabelPrinter")
            HttpContext.Current.Session.Remove("CreateLoadUnits")
            HttpContext.Current.Session.Remove("CreateLoadUOM")
            HttpContext.Current.Session.Remove("CreateLoadNumLoads")
            HttpContext.Current.Session.Remove("attributes")
            HttpContext.Current.Session.Remove("CreateLoadAdditionalAttributesCode128")
            HttpContext.Current.Session.Remove("CreateLoadDictSKUCode128")
        Else
            HttpContext.Current.Session.Remove("CreateLoadReciptId")
            HttpContext.Current.Session.Remove("CreateLoadReciptLine")
            HttpContext.Current.Session.Remove("CreateLoadConsignee")
            HttpContext.Current.Session.Remove("CreateLoadSKU")
            HttpContext.Current.Session.Remove("CreateLoadAdditionalAttributesCode128")
            HttpContext.Current.Session.Remove("CreateLoadDictSKUCode128")
            HttpContext.Current.Session.Remove("CreateLoadReceiptLocation")
            HttpContext.Current.Session.Remove("CreateLoadContainerID")
            HttpContext.Current.Session.Remove("SCANED")
            HttpContext.Current.Session.Remove("SKUCOLL")
        End If
    End Sub

    Public Shared Sub ClearCreateLoadChangeLineSession(Optional ByVal isDescreateReceiving As Boolean = False)
        If Not isDescreateReceiving Then
            HttpContext.Current.Session.Remove("CreateLoadRCNLine")
            HttpContext.Current.Session.Remove("CreateLoadConsignee")
            HttpContext.Current.Session.Remove("CreateLoadSKU")
            HttpContext.Current.Session.Remove("CreateLoadAdditionalAttributesCode128")
            HttpContext.Current.Session.Remove("CreateLoadDictSKUCode128")
            HttpContext.Current.Session.Remove("CreateLoadStatus")
            HttpContext.Current.Session.Remove("CreateLoadHoldRC")
            HttpContext.Current.Session.Remove("CreateLoadLabelPrinter")
            HttpContext.Current.Session.Remove("CreateLoadUnits")
            HttpContext.Current.Session.Remove("CreateLoadUOM")
            HttpContext.Current.Session.Remove("CreateLoadNumLoads")
            HttpContext.Current.Session.Remove("CreateLoadQty")
            HttpContext.Current.Session.Remove("CreateLoadSku")
            HttpContext.Current.Session.Remove("attributes")
            HttpContext.Current.Session("LineChanged") = 1
        End If
    End Sub

    Public Shared Sub ClearBlindReceivingSession()
        HttpContext.Current.Session.Remove("CreateLoadQty")
        HttpContext.Current.Session.Remove("CreateLoadSku")
        HttpContext.Current.Session.Remove("CreateLoadAdditionalAttributesCode128")
        HttpContext.Current.Session.Remove("CreateLoadDictSKUCode128")
        HttpContext.Current.Session.Remove("CreateLoadReciptId")
        HttpContext.Current.Session.Remove("CreateLoadConsignee")
        HttpContext.Current.Session.Remove("CreateLoadSKU")
        HttpContext.Current.Session.Remove("CreateLoadContainerID")
        HttpContext.Current.Session.Remove("CreateLoadLocation")
        HttpContext.Current.Session.Remove("CreateLoadLoadId")
    End Sub
    'Start RWMS-1487
    Public Shared Sub ClearBasicSessionOnLogOff()
        HttpContext.Current.Session.Remove("LoginWHArea")
        HttpContext.Current.Session.Remove("LoginLocation")
        HttpContext.Current.Session.Remove("Skin")
        HttpContext.Current.Session.Remove("WH")
        HttpContext.Current.Session.Remove("MHEID")
        HttpContext.Current.Session.Remove("MHTYPE")
        HttpContext.Current.Session.Remove("TERMINALTYPE")
        HttpContext.Current.Session.Remove("LoginWHArea")
        HttpContext.Current.Session.Remove("LoginLocation")

        HttpContext.Current.Session.Remove("Warehouse_CurrentWarehouseId")
        HttpContext.Current.Session.Remove("Warehouse_CurrentWarehouseName")
        HttpContext.Current.Session.Remove("Warehouse_CurrentWarehouseConnectionName")
        HttpContext.Current.Session.Remove("Warehouse_CurrentWarehouseTimeZone")
        'RWMS-1834 Start
        HttpContext.Current.Session.Remove("RDTDateFormat")

        'RWMS-1834 End
        HttpContext.Current.Session.Remove("IsGetTaskRequestFromUuser")
    End Sub
    'End RWMS-1487
#End Region
    Public Shared Function GetURLByScreenCode(ByVal pScreenCode As String) As String
        Dim url As String = DataInterface.ExecuteScalar(String.Format("select url from sys_screen where  screen_id = '{0}'", pScreenCode), "Made4NetSchema")
        Return (MapVirtualPath(url))
    End Function

    Public Shared Function GetMHEDefaultPrinter(Optional ByVal pMHEId As String = "") As String
        Dim lblPrinter As String = ""
        If Not HttpContext.Current.Session("MHEIDLabelPrinter") Is Nothing Then
            lblPrinter = HttpContext.Current.Session("MHEIDLabelPrinter")
        ElseIf Not HttpContext.Current.Session("MHEID") Is Nothing Then
            lblPrinter = DataInterface.ExecuteScalar(String.Format("SELECT isnull(LABELPRINTER,'') as LABELPRINTER FROM MHE WHERE MHEID = '{0}'", HttpContext.Current.Session("MHEID")))
        Else
            lblPrinter = DataInterface.ExecuteScalar(String.Format("SELECT isnull(LABELPRINTER,'') as LABELPRINTER FROM MHE WHERE MHEID = '{0}'", pMHEId))
        End If
        Return lblPrinter
    End Function

    Public Shared Function LoggedInMHEID() As String
        If Not HttpContext.Current.Session("MHEID") Is Nothing Then
            Return HttpContext.Current.Session("MHEID")
        Else
            Return ""
        End If
    End Function

    Public Shared Function CheckContainerID(ByVal picklist As String, ByRef containerID As String, ByRef errMessage As String) As Boolean
        Dim ret As Boolean = True
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Added for RWMS-1643 and RWMS-745
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        'Ended for RWMS-1643 and RWMS-745
        If containerID.Trim = "" Then
            'errMessage = trans.Translate("Illegal Container")
            'Return False

            containerID = Made4Net.Shared.Util.getNextCounter("CONTAINER")
            'Added for RWMS-1643 and RWMS-745  logs
            If Not rdtLogger Is Nothing Then
                rdtLogger.Write("If containerid is empty then it will create new container by using GetNextCounter method. New container ID: " & containerID)
                rdtLogger.writeSeperator("-", 60)
            End If
            'Ended for RWMS-1643 and RWMS-745

            'RWMS-691 Add the code to
            'every time the counter is incremented we check
            'if the conatiner exists, then increment again
            'i.e call the getNextCounter again and again..... untill it doesn't find the container
            While (True)
                If WMS.Logic.Container.Exists(containerID) Then
                    containerID = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                    'Added for RWMS-1643 and RWMS-745  logs
                    If Not rdtLogger Is Nothing Then
                        rdtLogger.Write(" If container already exists, then get the new containerid " & containerID)
                        rdtLogger.writeSeperator("-", 60)
                    End If
                    'Ended for RWMS-1643 and RWMS-745

                Else
                    Exit While
                End If
            End While
            'End RWMS-691
            HttpContext.Current.Session("PCKPicklistActiveContainerID") = containerID
            'Added for RWMS-1643 and RWMS-745  logs
            If Not rdtLogger Is Nothing Then
                rdtLogger.Write("Assign newly created containerid to the session(PCKPicklistActiveContainerID) " & containerID)
                rdtLogger.writeSeperator("-", 60)
            End If
            'Ended for RWMS-1643 and RWMS-745

            Return True

        End If

        If WMS.Logic.Container.Exists(containerID) Then
            Dim sql As String = "SELECT PICKLIST, TOCONTAINER FROM  dbo.PICKDETAIL where picklist = '{0}' and tocontainer = '{1}'"
            sql = String.Format(sql, picklist, containerID)
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                errMessage = trans.Translate("Container belongs to another picklist")
                'Throw New Made4Net.Shared.M4NException(New Exception, "Container belong to another picklist", "Container belong to another picklist")
                Return False
            Else
                Dim oCont As New WMS.Logic.Container(containerID, True)
                If oCont.Status <> WMS.Lib.Statuses.Container.STATUSNEW Then
                    'Throw New Made4Net.Shared.M4NException(New Exception, "Container status incorrect", "Container status incorrect")
                    errMessage = trans.Translate("Container status incorrect")
                    Return False
                End If
            End If

        End If

        'If containerID.Trim = "" Then
        '    containerID = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        'End If

        Return ret
    End Function

    Public Shared Function IsSafetyCheckRequiredForMHE(ByVal pMHType As String) As Boolean
        Dim sql As String = String.Format("Select SafetyCheckRequired from HANDLINGEQUIPMENT Where HANDLINGEQUIPMENT={0}", Made4Net.Shared.FormatField(pMHType))
        If System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql)) Then
            'sql = String.Format("Select LASTHANDLINGEUQIPSAFETYCHECK,LASTSHIFTSAFETYCHECK from userprofile where userid={0}", Made4Net.Shared.FormatField(WMS.Logic.GetCurrentUser()))
            'Added for RWMS-2046 and RWMS-1674 Start
            sql = String.Format("Select LASTHANDLINGEUQIPSAFETYCHECK,LASTSHIFTSAFETYCHECK,LASTSAFETYCHECKDATE from userprofile where userid={0}", Made4Net.Shared.FormatField(WMS.Logic.GetCurrentUser()))
            'Ended for RWMS-2046 and RWMS-1674 End
            Dim dt As DataTable = New DataTable()
            DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.CONNECTION_NAME)
            Dim lastShiftSafetyCheck As String = dt.Rows(0).Item("LASTSHIFTSAFETYCHECK").ToString()
            Dim lastHandlingEquipment As String = dt.Rows(0).Item("LASTHANDLINGEUQIPSAFETYCHECK").ToString()
            'Added for RWMS-2046 and RWMS-1674 Start
            Dim lastSafetyCheckDate As DateTime
            If dt.Rows(0).Item("LASTSAFETYCHECKDATE").ToString() = "" Then
                lastSafetyCheckDate = DateTime.MinValue
            Else
                lastSafetyCheckDate = Convert.ToDateTime(dt.Rows(0).Item("LASTSAFETYCHECKDATE"))
            End If
            'Ended for RWMS-2046 and RWMS-1674  End

            Dim curShiftID As String = GetCurrentShift()
            If curShiftID <> lastShiftSafetyCheck Then
                Return True
            End If

            If pMHType <> lastHandlingEquipment Then
                Return True
            End If
            'Added for RWMS-2046 and RWMS-1674 Start
            Dim whpSQL As String
            whpSQL = "select PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME='SafetyCheckOnLogin'"
            Dim paramval As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(whpSQL)
            If paramval = "1" Then
                If DateTime.Now.Date <> lastSafetyCheckDate.Date Then
                    Return True
                End If
            End If
            'Ended for RWMS-2046 and RWMS-1674  End

        End If

        Return False
    End Function

    Public Shared Sub UpdateUserProfilesShiftIDMHE(ByVal pHEType As String)
        Dim shiftID As String = GetCurrentShift()
        ' Dim SQL As String = String.Format("Update userprofile set LASTSHIFTSAFETYCHECK='{0}',LASTHANDLINGEUQIPSAFETYCHECK='{1}' where userid='{2}'", shiftID, pHEType, WMS.Logic.Common.GetCurrentUser())
        'Added for RWMS-2046 and RWMS-1674 Start
        Dim SQL As String = String.Format("Update userprofile set LASTSHIFTSAFETYCHECK='{0}',LASTHANDLINGEUQIPSAFETYCHECK='{1}',LASTSAFETYCHECKDATE='{2}' where userid='{3}'", shiftID, pHEType, DateTime.Now, WMS.Logic.Common.GetCurrentUser())
        'Ended for RWMS-2046 and RWMS-1674  End
        DataInterface.RunSQL(SQL, Made4Net.Schema.CONNECTION_NAME)

    End Sub
    Public Shared Sub UpdateDPrinterInUserProfile(ByVal pDefaultPrinter As String, ByVal ologger As ILogHandler)
        Dim SQL As String = String.Format("Update userprofile set DEFAULTPRINTER='{0}' where userid='{1}'", pDefaultPrinter, WMS.Logic.Common.GetCurrentUser())

        If Not ologger Is Nothing Then
            ologger.Write("Default Printer value is updated at USERPROFILE : " & SQL)
        End If

        DataInterface.RunSQL(SQL, Made4Net.Schema.CONNECTION_NAME)

    End Sub

    Public Shared Sub getDefaultPrinter(ByVal pDefaultPrinter As String, ByVal ologger As ILogHandler)
        Dim dt As New DataTable
        Dim SQL As String = String.Format("update WHACTIVITY set PRINTER = '{0}' where USERID = '{1}'", pDefaultPrinter, WMS.Logic.GetCurrentUser)

        If Not ologger Is Nothing Then
            ologger.Write("Default Printer value is updated at WHACTIVITY : " & SQL)
        End If
        Made4Net.DataAccess.DataInterface.RunSQL(SQL)

    End Sub

    Public Shared Function GetCurrentShift() As String
        Dim curShiftID As String
        '' This is commented as the shift is not implemented and without this user is not able to login. PWMS -920
        'Dim user As WMS.Logic.User = New WMS.Logic.User(WMS.Logic.GetCurrentUser())
        'Dim SQL As String = String.Format("Select shiftid from shift where shiftcode='{0}' and status='{1}'", user.SHIFT, "STARTED")

        'Try
        '    ''curShiftID = DataInterface.ExecuteScalar(SQL).ToString()
        '    curShiftID = "1"
        'Catch ex As Exception
        '    Throw New Exception("No shift is set for the entered user. Can not login.")
        'End Try
        curShiftID = "1"
        Return curShiftID
    End Function

    Public Shared Function ShowGoalTimeOnTaskAssignment() As Boolean
        If WMS.Logic.GetSysParam("ShowGoalTimeOnTaskAssignment") = 1 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function ShouldRedirectToTaskSummary() As Boolean
        If WMS.Logic.GetSysParam("ShowPerformanceOnTaskComplete") = 1 Then
            Return True
        End If
        Return False
    End Function

    Public Shared Sub RedirectToTaskSummary(ByVal pNextScreen As String, Optional ByVal pTaskId As String = "")
        If pTaskId <> "" Then
            HttpContext.Current.Session("TaskID") = pTaskId
        End If
        HttpContext.Current.Session("ScreenAfterTaskSummary") = pNextScreen
        '?sourcescreen=" & Session("MobileSourceScreen")
        HttpContext.Current.Response.Redirect(MapVirtualPath("screens/TaskSummary.aspx"))
    End Sub

    Public Shared Sub doDoneVerification()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Commented for RWMS-558
        'Dim cont As WMS.Logic.Container = CType(HttpContext.Current.Session("VRFICONT"), WMS.Logic.Container)
        'End Commented for RWMS-558
        'Added for RWMS-558
        If Not IsNothing(HttpContext.Current.Session("VRFILD")) Then
            Dim cont As WMS.Logic.Load = CType(HttpContext.Current.Session("VRFILD"), WMS.Logic.Load)
            Dim ld As New WMS.Logic.Load(cont.LOADID)
            Try
                ld.Verify(WMS.Logic.GetCurrentUser)
            Catch ex As Exception
            End Try
            Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(HttpContext.Current.Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))
            For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                Dim oSku As New WMS.Logic.SKU(pair.Key.Split("_")(0), pair.Key.Split("_")(1))
                sendMsgToAudit(oSku, pair.Value.TotalQty, pair.Value.VerifiedQty, cont.ContainerId)
            Next
        End If
        If Not IsNothing(HttpContext.Current.Session("VRFICONT")) Then
            Dim cont As WMS.Logic.Container = CType(HttpContext.Current.Session("VRFICONT"), WMS.Logic.Container)
            For i As Integer = 0 To cont.Loads.Count - 1
                Dim ld As New WMS.Logic.Load(cont.Loads(i).LOADID)
                Try
                    ld.Verify(WMS.Logic.GetCurrentUser)
                Catch ex As Exception
                End Try
            Next
            Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(HttpContext.Current.Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))
            For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                Dim oSku As New WMS.Logic.SKU(pair.Key.Split("_")(0), pair.Key.Split("_")(1))
                sendMsgToAudit(oSku, pair.Value.TotalQty, pair.Value.VerifiedQty, cont.ContainerId)
            Next
        End If
        'End Added for RWMS-558
        'Commented for RWMS-558
        'For i As Integer = 0 To cont.Loads.Count - 1
        '    'Dim skuStr As String = cont.Loads(i).CONSIGNEE & "_" & cont.Loads(i).SKU
        '    Dim ld As New WMS.Logic.Load(cont.Loads(i).LOADID)
        '    Try
        '        ld.Verify(WMS.Logic.GetCurrentUser)
        '    Catch ex As Exception
        '    End Try
        '    'sendMsgToAudit(pair.Key, previousStatus, pair.Value.VerifiedQty)
        'Next
        'Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(HttpContext.Current.Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))
        'For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
        '    ' If pair.Value.TotalQty > 0 AndAlso pair.Value.TotalQty <> pair.Value.VerifiedQty Then
        '    Dim oSku As New WMS.Logic.SKU(pair.Key.Split("_")(0), pair.Key.Split("_")(1))
        '    sendMsgToAudit(oSku, pair.Value.TotalQty, pair.Value.VerifiedQty, cont.ContainerId)
        '    ' End If
        'Next
        'End Commented for RWMS-558
        ' printReport(MobileUtils.GetMHEDefaultPrinter(), Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, Session("VRFIHUID"))
        HttpContext.Current.Session.Remove("VRFIDICT")
        'If Not Session("VRFICONT") Is Nothing Then
        '    sendMsgToAudit(Session("VRFICONT"))
        'End If
        HttpContext.Current.Session.Remove("VRFICONT")
        HttpContext.Current.Session.Remove("VRFILD")
        HttpContext.Current.Session.Remove("VRFIHUID")
        'Added for RWMS-559
        HttpContext.Current.Session.Remove("VRFISKU")
        'HttpContext.Current.Session.Remove("VRFICONSIGNEE")
        HttpContext.Current.Session.Remove("objMultiUOMUnits")
        'End Added for RWMS-559
        HandheldPopupNAlertMessageHandler.DisplayMessage(New Object(), t.Translate("Verification complete."))
        Try
            HttpContext.Current.Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub



    Public Shared Sub sendMsgToAudit(ByVal pLoad As WMS.Logic.SKU, ByVal TotalQty As Decimal, ByVal pVerifiedQty As Decimal, ByVal ContainerId As String)
        Dim aq As WMS.Logic.EventManagerQ = New WMS.Logic.EventManagerQ
        'Dim eventType As Integer = 2006

        Dim ACTIVITYTYPE As String
        If TotalQty = pVerifiedQty Then
            ACTIVITYTYPE = "SKUVERIFICATION"
        Else
            ACTIVITYTYPE = "SKUVERFAIL"
        End If
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SKUVERIFICATION)
        aq.Add("ACTIVITYTYPE", ACTIVITYTYPE)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", pLoad.CONSIGNEE)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMCONTAINER", ContainerId)
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", TotalQty.ToString())
        aq.Add("FROMSTATUS", "")
        'Dim sql As String = String.Format("select userid from audit where activitytype='PICKLOAD' and toload={0}", _
        'Made4Net.Shared.FormatField(pLoad.LOADID))
        'Dim pickedUser As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'aq.Add("NOTES", "") 'pickedUser)
        aq.Add("SKU", pLoad.SKU)
        aq.Add("TOLOAD", "")
        aq.Add("TOCONTAINER", ContainerId)
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", pVerifiedQty)
        aq.Add("TOSTATUS", "")
        aq.Add("NOTES", pLoad.EDITUSER)
        '
        If TotalQty <> pVerifiedQty Then
            aq.Add("REASONCODE", "Discrepancy")
        End If

        aq.Add("FROMWAREHOUSEAREA", HttpContext.Current.Session("LoginWHArea"))

        aq.Add("TOWAREHOUSEAREA", HttpContext.Current.Session("LoginWHArea"))


        aq.Add("USERID", WMS.Logic.Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Send(ACTIVITYTYPE)
    End Sub


    Public Shared Function IsFinishVerification() As Boolean

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Commented for RWMS-558
        'If Not HttpContext.Current.Session("VRFICONT") Is Nothing Then

        '    Dim cont As WMS.Logic.Container = CType(HttpContext.Current.Session("VRFICONT"), WMS.Logic.Container)
        '    Dim sqlstr As String = String.Format("select CONSIGNEE, SKU, UNITS from vVRFICONFITEM where HANDLINGUNIT = '{0}'", cont.ContainerId)
        '    Dim dt As New DataTable
        '    Made4Net.DataAccess.DataInterface.FillDataset(sqlstr, dt)

        '    Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(HttpContext.Current.Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))

        '    For Each dr As DataRow In dt.Rows
        '        For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
        '            If pair.Key = dr("CONSIGNEE") & "_" & dr("SKU") Then ' pair.Key.SKU = DOVerify.Value("SKU") AndAlso pair.Key.CONSIGNEE = DOVerify.Value("CONSIGNEE") Then
        '                If pair.Value.VerifiedQty < pair.Value.TotalQty Then
        '                    Return False
        '                End If
        '            End If
        '        Next
        '    Next
        '    Return True
        'Else
        '    Return False
        'End If
        'End Commented for RWMS-558
        'Added for RWMS-558
        If Not HttpContext.Current.Session("VRFICONT") Is Nothing Then

            Dim cont As WMS.Logic.Container = CType(HttpContext.Current.Session("VRFICONT"), WMS.Logic.Container)
            Dim sqlstr As String = String.Format("select CONSIGNEE, SKU, UNITS from vVRFICONFITEM where HANDLINGUNIT = '{0}'", cont.ContainerId)
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sqlstr, dt)

            Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(HttpContext.Current.Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))

            For Each dr As DataRow In dt.Rows
                For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                    If pair.Key = dr("CONSIGNEE") & "_" & dr("SKU") Then ' pair.Key.SKU = DOVerify.Value("SKU") AndAlso pair.Key.CONSIGNEE = DOVerify.Value("CONSIGNEE") Then
                        If pair.Value.VerifiedQty < pair.Value.TotalQty Then
                            Return False
                        End If
                    End If
                Next
            Next
            Return True
        ElseIf Not HttpContext.Current.Session("VRFILD") Is Nothing Then
            'Commented for RWMS-616
            'Dim cont As WMS.Logic.Load = CType(HttpContext.Current.Session("VRFICONT"), WMS.Logic.Load)
            'Commented for RWMS-616
            'Added for RWMS-616
            Dim cont As WMS.Logic.Load = CType(HttpContext.Current.Session("VRFILD"), WMS.Logic.Load)
            'End Added for RWMS-616
            Dim sqlstr As String = String.Format("select CONSIGNEE, SKU, UNITS from vVRFICONFITEM where HANDLINGUNIT = '{0}'", cont.LOADID)
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sqlstr, dt)

            Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(HttpContext.Current.Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))

            For Each dr As DataRow In dt.Rows
                For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                    If pair.Key = dr("CONSIGNEE") & "_" & dr("SKU") Then ' pair.Key.SKU = DOVerify.Value("SKU") AndAlso pair.Key.CONSIGNEE = DOVerify.Value("CONSIGNEE") Then
                        If pair.Value.VerifiedQty < pair.Value.TotalQty Then
                            Return False
                        End If
                    End If
                Next
            Next
            Return True
        End If
        'End Added for RWMS-558
    End Function

    Public Shared Sub PickRemaiderUnits(ByVal pckJob As WMS.Logic.PickJob)

        Dim picking As New WMS.Logic.Picking()
        Dim pckLineSecond As WMS.Logic.PicklistDetail = HttpContext.Current.Session("PCKPickLineSecond")
        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(pckLineSecond.PickList)
        Dim sku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)

        picking.PickLine(oPickList, pckLineSecond.PickListLine, sku.ConvertToUnits(pckJob.uom) * HttpContext.Current.Session("UOMUnits_2"), _
        WMS.Logic.GetCurrentUser, pckJob.oAttributeCollection, WMS.Logic.UserPickShort.PickPartialCreateException, _
        HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond"))

        HttpContext.Current.Session.Remove("PCKPickLineSecond")

    End Sub

    Public Shared Function getLoadUnits(ByVal ld As WMS.Logic.Load) As Decimal
        Return ld.UNITS
    End Function

    Public Shared Function QTYToCase(ByVal sk As WMS.Logic.SKU, ByVal qty As Decimal) As Decimal
        If qty = 0 Then
            Return qty
        Else
            Return Math.Round(qty / sk.ConvertToUnits("CASE"), 2)
        End If

    End Function

    Public Shared Function ValidateWeightSku(ByVal oSku As WMS.Logic.SKU, ByRef Weight As String, ByVal Units As Integer, ByRef gotoOverride As Boolean, ByRef gotoOverrideMessage As String, ByRef errMsg As String) As Boolean

    End Function

    ' <System.Web.Services.WebMethod()> _
    Public Shared Function GetCurrentTime(ByVal name As String) As String
        Return "Hello " & name & Environment.NewLine & "The Current Time is: " & _
                    DateTime.Now.ToString()
    End Function

    'convert from UOMUnits to each units
    Public Shared Function ConvertToUnits(ByVal consignee As String, ByVal sku As String, ByVal uom As String, ByVal UOMunits As Decimal) As Decimal
        Dim sk As New WMS.Logic.SKU(consignee, sku)

        If UOMunits = 0 Or Not WMS.Logic.SKU.SKUUOM.Exists(consignee, sku, uom) Then
            Return UOMunits
        Else
            Return Math.Round(UOMunits * sk.ConvertToUnits(uom), 2)
        End If
    End Function

    'convert from each units to different UOMunits:: like case, pallet...
    Public Shared Function ConvertUnitsToUom(ByVal consignee As String, ByVal sku As String, ByVal uom As String, ByVal units As Decimal) As Decimal
        Dim sk As New WMS.Logic.SKU(consignee, sku)

        If units = 0 Or Not WMS.Logic.SKU.SKUUOM.Exists(consignee, sku, uom) Then
            Return units
        Else
            Return Math.Round(sk.ConvertUnitsToUom(uom, units), 2)
        End If
    End Function

    Public Shared Function getSKU(ByVal consignee As String, ByVal sku As String, ByVal FROMSCREEN As String) As String

        If sku <> "" Then
            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SKU) FROM vSKUCODE WHERE (SKUCODE LIKE '" & sku & "%' OR SKU LIKE '" & consignee & "%')") > 1 Then
                ' Go to Sku select screen
                HttpContext.Current.Session("FROMSCREEN") = FROMSCREEN
                HttpContext.Current.Session("SKUCODE") = sku
                ' Add all controls to session for restoring them when we back from that sreen
                HttpContext.Current.Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) 'Changed

            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SKU) FROM vSKUCODE WHERE SKUCODE LIKE '" & sku & "%'") = 1 Then
                sku = DataInterface.ExecuteScalar("SELECT SKU FROM vSKUCODE WHERE SKUCODE LIKE '" & sku & "%'")
            End If
        End If

        Return sku
    End Function

    'RWMS-3726

    Public Shared Function getPCKComments(ByVal picklist As String) As String
        Dim sqlPickingComments As String = String.Format("SELECT top 1 ISNULL(PICKINGCOMMENTS,'') FROM vPicklistPickingComments WHERE PICKLIST = '{0}' ", picklist)
        Return DataInterface.ExecuteScalar(sqlPickingComments)
    End Function

    'RWMS-3726

    'Public Shared Sub AddTaskToTaskSummaryList(ByVal pTaskID As String)
    '    If Not HttpContext.Current.Session("TaskID") Is Nothing Then
    '        Dim tasksList As System.Collections.Generic.List(Of String) = CType(HttpContext.Current.Session("TaskID"), System.Collections.Generic.List(Of String))
    '        tasksList.Add(pTaskID)
    '        HttpContext.Current.Session("TaskID") = tasksList
    '    Else
    '        Dim tasksList As New System.Collections.Generic.List(Of String)
    '        tasksList.Add(pTaskID)
    '        HttpContext.Current.Session("TaskID") = tasksList
    '    End If
    'End Sub

#Region "Class VerificationData"


    Public Class VerificationData

        Private _isRecountStarted As Boolean
        Private _verifiedQty As Decimal
        Private _totalqty As Decimal

        Public Sub New(ByVal pIsRecountStarted As Boolean, ByVal pTotalQty As Decimal, ByVal pVerifiedQty As Decimal)
            _isRecountStarted = pIsRecountStarted
            _verifiedQty = pVerifiedQty
            _totalqty = pTotalQty
        End Sub

        Public Property IsRecountStarted() As Boolean
            Get
                Return _isRecountStarted
            End Get
            Set(ByVal value As Boolean)
                _isRecountStarted = value
            End Set
        End Property

        Public Property VerifiedQty() As Decimal
            Get
                Return _verifiedQty
            End Get
            Set(ByVal value As Decimal)
                _verifiedQty = value
            End Set
        End Property

        Public Property TotalQty() As Decimal
            Get
                Return _totalqty
            End Get
            Set(ByVal value As Decimal)
                _totalqty = value
            End Set
        End Property

        Public Shared Sub DeleteFromDB(ByVal pHandlingUnit As String)
            Dim sql As String = String.Format("delete from verifiedHU where handlingunit={0}", _
            Made4Net.Shared.FormatField(pHandlingUnit))

            Made4Net.DataAccess.DataInterface.RunSQL(sql)
        End Sub


        Public Shared Sub DeleteFromDB(ByVal pHandlingUnit As String, ByVal pConsigneeSKU As String)
            Dim sql As String = String.Format("delete from verifiedHU where handlingunit={0} and consignee={1} and sku={2}", _
            Made4Net.Shared.FormatField(pHandlingUnit), Made4Net.Shared.FormatField(pConsigneeSKU.Split("_")(0)), Made4Net.Shared.FormatField(pConsigneeSKU.Split("_")(1)))

            Made4Net.DataAccess.DataInterface.RunSQL(sql)
        End Sub

        Public Shared Sub AddToDB(ByVal pHandlingUnit As String, ByVal pConsigneeSKU As String, ByVal pQty As Decimal)
            Dim sql As String = String.Format("delete from verifiedHU where handlingunit={0} and consignee={1} and sku={2}", _
            Made4Net.Shared.FormatField(pHandlingUnit), Made4Net.Shared.FormatField(pConsigneeSKU.Split("_")(0)), Made4Net.Shared.FormatField(pConsigneeSKU.Split("_")(1)))

            Made4Net.DataAccess.DataInterface.RunSQL(sql)

            sql = String.Format("insert into verifiedHU values ({0},{1},{2},{3})", Made4Net.Shared.FormatField(pHandlingUnit), _
            Made4Net.Shared.FormatField(pConsigneeSKU.Split("_")(0)), Made4Net.Shared.FormatField(pConsigneeSKU.Split("_")(1)), Made4Net.Shared.FormatField(pQty))
            Made4Net.DataAccess.DataInterface.RunSQL(sql)
        End Sub



    End Class

#End Region

End Class