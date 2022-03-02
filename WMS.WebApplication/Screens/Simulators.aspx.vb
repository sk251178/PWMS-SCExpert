Imports System.IO
Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Algorithms
Imports DataManager.ServiceModel
Imports DataManager.DataModel
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports Made4Net.Shared

Partial Public Class Simulators
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


    Protected WithEvents LabelLoad As Label
    Protected WithEvents LabelContainer As Label
    Protected WithEvents rdbLoad As RadioButton
    Protected WithEvents rdbContainer As RadioButton

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        taWareahousearea.Value = Session()("UserSelectedWHArea")
    End Sub

#Region "Planner"
    Protected Sub rdbPlanner_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rdbWave.Checked Then
            txtWave.Visible = True
            txtOrder.Visible = False
            txtCons.Visible = False

            lblWave.Visible = True
            lblOrder.Visible = False
            lblCons.Visible = False
        Else
            txtWave.Visible = False
            txtOrder.Visible = True
            txtCons.Visible = True

            lblWave.Visible = False
            lblOrder.Visible = True
            lblCons.Visible = True
        End If

    End Sub

    Protected Sub rdbPlanner_Simulate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rdbWave.Checked And txtWave.Value = String.Empty Then Return
        If rdbOrders.Checked And (txtCons.Value = String.Empty Or txtOrder.Value = String.Empty) Then Return

        Dim filename As String = "SimulatePlaner_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & "_" & New Random().Next & ".txt"
        Dim LogPath As String = WMS.Logic.GetSysParam("ApplicationLogDirectory")
        'RWMS-2456
        'RWMS-2786 Commented
        'LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath, True)
        'RWMS-2786 Commented END
        'RWMS-2786
        LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath)
        'RWMS-2786 END
        Dim oLogger As LogHandler = New LogHandler(LogPath, filename)
        oLogger.writeSeperator("-", 50)
        oLogger.Write("Start Planner simulation...")

        Dim oPlanner As Planner = New Planner(True)
        If rdbWave.Checked Then
            oPlanner.Plan(txtWave.Value, False, Common.GetCurrentUser, oLogger)
        Else
            oPlanner.PlanOrder(txtCons.Value, txtOrder.Value, False, Common.GetCurrentUser, oLogger)
        End If

        'oLogger.Write("End Planner simulation.")
        oLogger.EndWrite()
        oLogger = Nothing

        Dim LogStr As String = LoadFile(LogPath & filename)
        logFiletxt.Text = LogStr
        If chkisDeleteLog.Checked Then
            Dim TargetFile As New System.IO.FileInfo(LogPath & filename)
            TargetFile.Delete()
            TargetFile = Nothing
        End If
        rdbPlanner_CheckedChanged(Nothing, Nothing)
    End Sub

#End Region

#Region "Task Manager"
    Protected Sub TaskManager_Simulate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If taMHEquip.Value = String.Empty Or taUserID.Value = String.Empty Or taInitLoc.Value = String.Empty Then Return

        Dim filename As String = "SimulateTaskManager_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & "_" & New Random().Next & ".txt"
        Dim LogPath As String = WMS.Logic.GetSysParam("ApplicationLogDirectory")
        'RWMS-2456
        'RWMS-2785 Commented
        'LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath, True)
        'RWMS-2785 Commented END
        'RWMS-2785
        LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath)
        'RWMS-2785 END
        Dim oLogger As LogHandler = New LogHandler(LogPath, filename)
        oLogger.writeSeperator("-", 50)
        oLogger.Write("Start Task Manager simulation...")

        Dim sql As String = String.Format("Select Top(1) * from WHACTIVITY where USERID='{0}'", taUserID.Value)
        Dim ds As DataTable = New DataTable()

        DataInterface.FillDataset(sql, ds)
        Dim isInserted As Boolean = False
        If ds.Rows.Count > 0 Then
            Dim activityId As String = ds.Rows(0)("ACTIVITYID")
            DataInterface.RunSQL(String.Format("update WHACTIVITY set MHEID='{0}', HETYPE=(select MHETYPE from MHE where MHEID = {0}), LOCATION='{1}' where USERID='{2}' AND ACTIVITYID='{3}'", taMHEquip.Value, taInitLoc.Value, taUserID.Value, activityId))
        Else
            sql = String.Format("INSERT [dbo].[WHACTIVITY] ([ACTIVITYID], [USERID], [HETYPE], [ACTIVITY], [LOCATION], [ACTIVITYTIME], [MHEID], [TERMINALTYPE], [ADDDATE], [ADDUSER], [EDITDATE], [EDITUSER], [WAREHOUSEAREA], [SHIFT]) VALUES ((select CounterVal+1 from COUNTER where COUNTER = 'WHACTIVITY'), N'{0}',(select MHETYPE from MHE where MHEID = {2}), N'Login', '{3}', '{1}', N'{2}', N'HANDHELD', '{1}', N'{0}', '{1}', N'{0}', N'NCR', N'0000000002')", taUserID.Value,
                                DateTime.Now.ToString(), taMHEquip.Value, taInitLoc.Value)
            DataInterface.RunSQL(sql)
            isInserted = True

        End If

        Dim oTM As TaskManager = New TaskManager()
        oTM.RequestTask(taUserID.Value, 1, oLogger)

        oLogger.Write("End Task Manager simulation.")
        oLogger.EndWrite()
        oLogger = Nothing

        Dim LogStr As String = LoadFile(LogPath & filename)
        logFiletxt.Text = LogStr
        If chkisDeleteLog.Checked Then
            Dim TargetFile As New System.IO.FileInfo(LogPath & filename)
            TargetFile.Delete()
            TargetFile = Nothing
        End If

        'Cleanup
        If isInserted Then
            sql = String.Format("delete from WHACTIVITY where USERID = '{0}'", taUserID.Value)
            DataInterface.RunSQL(sql)
        End If

    End Sub

#End Region

#Region "Putaway"

    Protected Sub PutAway_Simulate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If taLoad.Value = String.Empty And taContainer.Value = String.Empty Then Return

        Dim filename As String = "SimulatePutAway_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & "_" & New Random().Next & ".txt"
        Dim LogPath As String = WMS.Logic.GetSysParam("ApplicationLogDirectory")
        'RWMS-2456
        'RWMS-2787 Commented
        'LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath, True)
        'RWMS-2787 Commented END
        'RWMS-2787
        LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath)
        'RWMS-2787 END
        Dim oLogger As LogHandler = New LogHandler(LogPath, filename)
        oLogger.writeSeperator("-", 50)
        oLogger.Write("Start Put Away simulation...")

        'Dim oContainer As WMS.Logic.Container = New WMS.Logic.Container(taContainer.Value, False)
        'Dim oLocation As WMS.Logic.Location = New WMS.Logic.Location()

        ''oLocation.LocToPlace (Location,Warehouse,PoliciID,oContainer,...)
        If taLoad.Value <> String.Empty Then
            putawayLoad(oLogger)
        Else
            putawayContainer(oLogger)
        End If

        oLogger.Write("End Put Away simulation.")
        oLogger.EndWrite()
        oLogger = Nothing

        Dim LogStr As String = LoadFile(LogPath & filename)
        logFiletxt.Text = LogStr
        If chkisDeleteLog.Checked Then
            Dim TargetFile As New System.IO.FileInfo(LogPath & filename)
            TargetFile.Delete()
            TargetFile = Nothing
        End If

    End Sub

    Private Sub putawayLoad(ByVal lg As LogHandler)
        Dim LoadID As String = ""
        Dim Region As String = ""
        Dim LocationID As String = ""
        Dim WarehouseareaID As String = ""

        Dim User As String = ""
        Dim StorageType As String = ""
        Dim Content As String = ""
        Dim Inv As String = ""
        Dim QtyToPlace As Decimal = -1
        Dim ld As WMS.Logic.Load = Nothing
        Dim sk As WMS.Logic.SKU = Nothing
        Dim co As WMS.Logic.Consignee = Nothing
        Dim useLogs As Boolean = True

        Try

            LoadID = taLoad.Value

            If LoadID = Nothing Or LoadID = "" Then
                'Load can not be null
            End If
            ld = New WMS.Logic.Load(LoadID, True)

        Catch m4nex As Made4Net.Shared.M4NException

            lg.Write("Error Occured - could not load the Load Object.")
            lg.Write(m4nex.Description)

            'ResponseOnError(LoadID, 1, qMsg)
            Return
        End Try
        Try
            sk = New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
            Dim dtConsStrategy As DataTable = New DataTable()
            Dim DTLoadParams As DataTable = New DataTable()
            Made4Net.DataAccess.DataInterface.FillDataset(String.Format("select * from vLoadsPutaway where loadid = '{0}'", ld.LOADID), DTLoadParams, False, Nothing)

            If DTLoadParams.Rows.Count = 0 Then

                If useLogs Then
                    lg.Write("Error Occured - could not extract Load params from vLoadsPutaway....")
                End If
            Else

                If useLogs Then
                    lg.Write("Load Params extracted from vLoadsPutaway....")
                End If
            End If

            Dim oPolicyMatcher As PolicyMatcher = New PolicyMatcher("CONSIGNEEPUTAWAY", DTLoadParams)
            dtConsStrategy = oPolicyMatcher.FindMatchingPolicies()
            Dim locationFound As Boolean = False
            ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND, WMS.Logic.GetCurrentUser)
            For Each drConsStrategy As DataRow In dtConsStrategy.Rows
                If useLogs Then

                    lg.writeSeperator("*", 80)
                    lg.Write("Trying to find location according to policy: " & drConsStrategy("POLICYID"))

                End If
                Dim SQL As String = Nothing
                SQL = String.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE, ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " + " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", drConsStrategy("POLICYID"))
                Dim dtRegions As DataTable = New DataTable()
                DataInterface.FillDataset(SQL, dtRegions, False, "")
                For Each drRegion As DataRow In dtRegions.Rows

                    Dim sRegions As String = GetRegionsByPriority(Convert.ToInt32(drRegion("Priority")), dtRegions)
                    Dim locStorageType As String = Nothing
                    Dim loccontent As String = Nothing
                    Dim iMAxNumPallets As Int32 = -1
                    If drRegion("LOCSTORAGETYPE") Is System.DBNull.Value Then
                        locStorageType = "%"
                    Else
                        locStorageType = Convert.ToString(drRegion("LOCSTORAGETYPE"))
                    End If
                    If Content = "" Then

                        If drRegion("CONTENT") Is System.DBNull.Value Then
                            loccontent = "%"
                        Else
                            loccontent = Convert.ToString(drRegion("CONTENT"))
                        End If
                    Else
                        loccontent = Content
                    End If
                    iMAxNumPallets = Convert.ToInt32(drRegion("MAXNUMPALLETS"))

                    If useLogs Then
                        lg.Write("Calling the Location.LocToPlace function.")
                    End If
                    WarehouseareaID = ""
                    LocationID = "" ' Convert.ToString(drRegion("PUTAWAYPOLICY"))

                    WMS.Logic.Location.LocToPlace(LocationID, WarehouseareaID, Convert.ToString(drRegion("PUTAWAYPOLICY")), ld, sRegions, Convert.ToBoolean(drRegion("FITBYVOLUME")), Convert.ToBoolean(drRegion("FITBYWEIGHT")), Convert.ToBoolean(drRegion("FITBYHEIGHT")), Convert.ToBoolean(drRegion("FITBYPALLETTYPE")), iMAxNumPallets, 0, locStorageType, loccontent, QtyToPlace, lg, "", "")
                    If (Not String.IsNullOrEmpty(LocationID)) Then
                        'ResponseOnSuccess(LoadID, LocationID, qMsg)
                        locationFound = True
                        Exit For
                    End If
                Next
                If locationFound Then
                    Dim pickloc As String = DistanceApplied.GetPickLocationForSKU(ld, lg)
                    Dim SHPath As ShortestPath
                    Dim path As DataManager.ServiceModel.Path
                    SHPath = ShortestPath.GetInstance()
                    ' Fetch source Location
                    Dim sqlLoc As [String] = "Select * from LOCATION where LOCATION = '{0}'"
                    Dim locationFrom As New DataTable()
                    DataInterface.FillDataset([String].Format(sqlLoc, pickloc), locationFrom)
                    Dim from As DataRow = locationFrom.Rows(0)

                    Dim locationTo As New DataTable()
                    DataInterface.FillDataset([String].Format(sqlLoc, LocationID), locationTo)
                    Dim toLoc As DataRow = locationTo.Rows(0)

                    Try
                        If TypeAheadBoxUsrId.Value <> String.Empty Then
                            Dim rules As New List(Of Rules)()
                            Dim rule As New Rules()
                            rule.Parameter = Made4Net.Algorithms.Constants.Height
                            rule.Data = GetOperatorsEquipmentHeight(TypeAheadBoxUsrId.Value)
                            rule.[Operator] = ">"
                            rules.Add(rule)

                            path = SHPath.GetShortestPathWithContsraints(from, toLoc, Nothing, "LOADPW", True, rules, GetShortestPathLogger(lg))
                        Else
                            path = SHPath.GetShortestPathWithContsraints(from, toLoc, Nothing, "LOADPW", True, Nothing, GetShortestPathLogger(lg))
                        End If
                        lg.Write("Node to Node Traversed Distance")
                        lg.writeSeperator("-", 80)

                        Dim item As KeyValuePair(Of String, Double)
                        For Each item In path.TraversedNodes
                            lg.Write(String.Format("Node : {0} " & vbTab & "-- Distance : {1}" & vbCrLf, item.Key, item.Value))
                        Next
                        lg.writeSeperator("-", 80)
                    Catch ex As Exception
                        lg.Write("Distance calculation threw exception for the selected location found by putaway. Cannot display the walk nodes.")
                    End Try

                    Exit For
                End If

            Next

            If useLogs Then
                lg.Write("Got Location : " + LocationID + " From the LocToPlace.")
                lg.Write("Got Warehousearea : " + WarehouseareaID + " From the LocToPlace.")

            End If
            'If no location was found -> return empty location
            'ResponseOnSuccess(LoadID, "", qMsg)

        Catch ex As Exception

            If useLogs Then

                lg.Write("Error Occured - " + ex.ToString())
            End If
            'ResponseOnError(Region, 1, qMsg)
            Return
        Finally
            ld.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.NONE, WMS.Logic.GetCurrentUser)
        End Try


    End Sub

    Private Sub putawayContainer(ByVal lg As LogHandler)
        Dim contId As String = ""
        Dim LocationID As String = ""
        Dim WarehouseareaID As String = ""
        Dim User As String = ""
        Dim oCont As WMS.Logic.Container = Nothing
        Dim useLogs As Boolean = True
        'User = qSender.Values("USERID")
        Dim locationFound As Boolean = False
        Try

            'contId = qSender.Values("CONTAINERID")
            If useLogs Then lg.Write("Got the Container Id from the QSender: " + contId)
            If contId Is Nothing Or contId = "" Then
                'ResponseOnError(contId, 2, qMsg)
                'Return
            End If
            oCont = New WMS.Logic.Container(contId, True)

        Catch m4nex As Made4Net.Shared.M4NException
            If useLogs Then

                lg.Write("Error Occured - could not load the Container Object...")
                lg.Write(m4nex.Description)
            End If
            'ResponseOnError(contId, 1, qMsg)
            'return;
        Catch ex As Exception
            If useLogs Then
                lg.Write("Error Occured - could not load the Container Object...")
                lg.Write(ex.ToString())
            End If
            'ResponseOnError(contId, 1, qMsg);
            'return;
        End Try
        Try

            If useLogs Then lg.Write("Trying to match putaway policy according to container content...")
            Dim loads As String = ContainerLoadsString(oCont)
            Dim dtConsStrategy As DataTable = New DataTable()
            Dim DTLoadParams As DataTable = New DataTable()
            Made4Net.DataAccess.DataInterface.FillDataset(String.Format("select * from vLoadsPutaway where loadid in ({0})", loads), DTLoadParams, False, Nothing)
            If DTLoadParams.Rows.Count = 0 Then
                If (useLogs) Then lg.Write("Error Occured - could not extract Container Load params from vLoadsPutaway...")
            Else
                If useLogs Then lg.Write("Container Load Params extracted from vLoadsPutaway...")
            End If
            GetCommonFiledsTable(DTLoadParams)
            Dim oPolicyMatcher As PolicyMatcher = New PolicyMatcher("CONSIGNEEPUTAWAY", DTLoadParams)
            dtConsStrategy = oPolicyMatcher.FindMatchingPolicies()
            If useLogs Then lg.Write(dtConsStrategy.Rows.Count.ToString() + " Policies found after matching policies...")
            For Each drConsStrategy As DataRow In dtConsStrategy.Rows

                If useLogs Then
                    lg.writeSeperator("*", 80)
                    lg.Write("Trying to find location according to policy: " + drConsStrategy("POLICYID"))
                End If
                Dim SQL As String = Nothing
                SQL = String.Format("select PUTAWAYPOLICY.PUTAWAYPOLICY, PUTAWAYPOLICY.POLICYNAME, PUTAWAYPOLICYDETAIL.PRIORITY, PUTAWAYPOLICYDETAIL.PUTREGION, PUTAWAYPOLICYDETAIL.LOCSTORAGETYPE, PUTAWAYPOLICYDETAIL.CONTENT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYVOLUME, 1) AS FITBYVOLUME, ISNULL(PUTAWAYPOLICYDETAIL.FITBYWEIGHT, 1) AS FITBYWEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYHEIGHT, 1) AS FITBYHEIGHT, ISNULL(PUTAWAYPOLICYDETAIL.FITBYPALLETTYPE, 1) AS FITBYPALLETTYPE, ISNULL(PUTAWAYPOLICYDETAIL.MAXNUMPALLETS, -1) AS MAXNUMPALLETS from putawaypolicy inner join putawaypolicydetail on PUTAWAYPOLICY.putawaypolicy = putawaypolicydetail.putawaypolicy " + " where PUTAWAYPOLICY.putawaypolicy = '{0}' order by priority", drConsStrategy("POLICYID"))
                Dim dtRegions As DataTable = New DataTable()
                DataInterface.FillDataset(SQL, dtRegions, False, "")
                For Each drRegion As DataRow In dtRegions.Rows

                    Dim sRegions As String = GetRegionsByPriority(Convert.ToInt32(drRegion("Priority")), dtRegions)
                    Dim locStorageType As String = Nothing
                    Dim content As String = Nothing
                    If drRegion("LOCSTORAGETYPE") Is System.DBNull.Value Then
                        locStorageType = "%"
                    Else
                        locStorageType = Convert.ToString(drRegion("LOCSTORAGETYPE"))
                    End If
                    If drRegion("CONTENT") Is System.DBNull.Value Then
                        content = "%"
                    Else
                        content = Convert.ToString(drRegion("CONTENT"))
                    End If
                    If useLogs Then lg.Write("Calling the Location Search for Container Putaway...")

                    LocationID = Convert.ToString(drRegion("PUTAWAYPOLICY"))
                    WMS.Logic.Location.LocToPlace(LocationID, WarehouseareaID, "", oCont, sRegions, Convert.ToBoolean(drRegion("FITBYVOLUME")), Convert.ToBoolean(drRegion("FITBYWEIGHT")), Convert.ToBoolean(drRegion("FITBYHEIGHT")), Convert.ToBoolean(drRegion("FITBYPALLETTYPE")), locStorageType, content, lg)
                    If (Not String.IsNullOrEmpty(LocationID)) Then

                        If useLogs Then
                            lg.Write("Got Location : " + LocationID + " From the LocToPlace.")
                            locationFound = True
                            Exit For
                        End If

                        'ResponseOnSuccess(oCont.ContainerId, LocationID, qMsg);
                        'return;
                    End If
                Next
                If locationFound Then
                    Exit For
                End If

            Next
            If useLogs Then
                lg.Write("No Location Found for receiving the container...")
                'f no location was found -> return empty location
                'ResponseOnSuccess(oCont.ContainerId, "", qMsg);
            End If
        Catch ex As Exception

            If useLogs Then lg.Write("Error Occured - " + ex.ToString())


            'ResponseOnError(oCont.ContainerId, 1, qMsg);
            'return;

        End Try

    End Sub

    Private Function GetRegionsByPriority(ByVal pPriority As Int32, ByVal dt As DataTable) As String
        Dim regions As String = ""
        For Each dr As DataRow In dt.Rows
            If Convert.ToInt32(dr("Priority")) = pPriority Then

                regions = regions & "," & "'" & dr("putregion") & "'"
            End If
        Next
        regions = regions.TrimStart(",".ToCharArray())
        If regions.IndexOf("'") < 0 Then

            regions = String.Format("'{0}'", regions)
        End If
        Return regions
    End Function

    Private Function ContainerLoadsString(ByVal oCont As WMS.Logic.Container) As String

        Dim loads As String = String.Empty
        For Each oLoad As WMS.Logic.Load In oCont.Loads
            loads = loads + "," + "'" + oLoad.LOADID + "'"
        Next
        loads = loads.TrimStart(",".ToCharArray())
        Return loads
    End Function

    Private Sub GetCommonFiledsTable(ByRef DTLoadParams As DataTable)

        'DataTable commonFields = new DataTable();
        Dim commonRow As DataRow

        If DTLoadParams.Rows.Count <= 1 Then Return

        commonRow = DTLoadParams.NewRow()
        For Each tmpCol As DataColumn In DTLoadParams.Columns

            If DataMatched(DTLoadParams, tmpCol.ColumnName) Then
                commonRow(tmpCol.ColumnName) = DTLoadParams.Rows(0)(tmpCol.ColumnName)
            End If
        Next
        DTLoadParams.Rows.InsertAt(commonRow, 0)
        For i As Integer = 1 To DTLoadParams.Rows.Count - 1
            DTLoadParams.Rows.RemoveAt(i)
        Next

    End Sub

    Private Function DataMatched(ByVal DTLoadParams As DataTable, ByVal pColumnName As String) As Boolean
        For i As Integer = 0 To DTLoadParams.Rows.Count - 1
            Select Case DTLoadParams.Rows(i)(pColumnName).GetType().FullName
                Case "System.String"
                    If Convert.ToString(DTLoadParams.Rows(i)(pColumnName)) <> Convert.ToString(DTLoadParams.Rows(i + 1)(pColumnName)) Then
                        Return False
                    End If
                Case "System.DateTime"
                    If Convert.ToDateTime(DTLoadParams.Rows(i)(pColumnName)) <> Convert.ToDateTime(DTLoadParams.Rows(i + 1)(pColumnName)) Then
                        Return False
                    End If
                Case "System.Decimal"
                    If Convert.ToDecimal(DTLoadParams.Rows(i)(pColumnName)) <> Convert.ToDecimal(DTLoadParams.Rows(i + 1)(pColumnName)) Then
                        Return False
                    End If
                Case "System.Int32"
                    If Convert.ToInt32(DTLoadParams.Rows(i)(pColumnName)) <> Convert.ToInt32(DTLoadParams.Rows(i + 1)(pColumnName)) Then
                        Return False
                    End If
                Case "System.Boolean"
                    If Convert.ToBoolean(DTLoadParams.Rows(i)(pColumnName)) <> Convert.ToBoolean(DTLoadParams.Rows(i + 1)(pColumnName)) Then
                        Return False
                    End If
            End Select
        Next
        Return True
    End Function

    Protected Sub rdbPutaway_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rdbLoad.Checked Then
            LabelLoad.Visible = True
            LabelContainer.Visible = False
            taLoad.Visible = True
            taContainer.Visible = False
            TypeAheadBoxUsrId.Visible = True
            LabeUsrId.Visible = True

        Else
            LabelLoad.Visible = False
            LabelContainer.Visible = True
            taLoad.Visible = False
            taContainer.Visible = True
            TypeAheadBoxUsrId.Visible = False
            LabeUsrId.Visible = False


        End If

    End Sub

    Private Function GetOperatorsEquipmentHeight(user As [String]) As Double
        Dim sql As [String] = [String].Format("select  COALESCE(HANDLINGEQUIPMENT.HEIGHT, 0) as HEIGHT from HANDLINGEQUIPMENT inner join WHACTIVITY on HANDLINGEQUIPMENT.HANDLINGEQUIPMENT = WHACTIVITY.HETYPE where WHACTIVITY.USERID = '{0}'", user)
        Dim dt As New DataTable()
        DataInterface.FillDataset(sql, dt, False, "")
        If dt.Rows.Count > 0 Then
            Return Convert.ToDouble(dt.Rows(0)("HEIGHT"))
        End If
        Return 0
    End Function

#End Region

#Region "Replenisment"
    Protected Sub rdbReplenisment_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rdbReplPicking.Checked Then
            taLocation.Visible = True
            lblLocation.Visible = True
            lblWareahousearea.Visible = True
            taWareahousearea.Visible = True
        Else
            taLocation.Visible = False
            lblLocation.Visible = False
            lblWareahousearea.Visible = False
            taWareahousearea.Visible = False

        End If

    End Sub

    Protected Sub rdbReplenisment_Simulate(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If rdbReplPicking.Checked And (taWareahousearea.Value = String.Empty Or taLocation.Value = String.Empty) Then Return
        'If taSKU.Value = String.Empty Or taPutRegion.Value = String.Empty Then Return

        Dim filename As String = "SimulateReplenisment_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & "_" & New Random().Next & ".txt"
        Dim LogPath As String = WMS.Logic.GetSysParam("ApplicationLogDirectory")
        'RWMS-2456
        LogPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(LogPath)
        Dim oLogger As LogHandler = New LogHandler(LogPath, filename)
        oLogger.writeSeperator("-", 50)
        oLogger.Write("Start Replenisment simulation...")
        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = WMS.Logic.Replenishment.GetPolicyID()
        'Ended for RWMS-1840

        If rdbReplPicking.Checked Then
            Dim oReplenisment As WMS.Logic.Replenishment = New WMS.Logic.Replenishment()
            oReplenisment.NormalReplenish(taLocation.Value, taWareahousearea.Value, taPutRegion.Value, taSKU.Value, oLogger, 1)
        Else
            Dim oZoneReplenisment As WMS.Logic.ZoneReplenish = New WMS.Logic.ZoneReplenish()
            oZoneReplenisment.ZoneReplenishment(taPutRegion.Value, taSKU.Value, _
                ReplPolicyMethod, _
                    Common.GetCurrentUser, oLogger)
        End If

        oLogger.Write("End Replenisment simulation.")
        oLogger.EndWrite()
        oLogger = Nothing

        Dim LogStr As String = LoadFile(LogPath & filename)
        logFiletxt.Text = LogStr
        If chkisDeleteLog.Checked Then
            Dim TargetFile As New System.IO.FileInfo(LogPath & filename)
            TargetFile.Delete()
            TargetFile = Nothing
        End If
        rdbReplenisment_CheckedChanged(Nothing, Nothing)
    End Sub

#End Region

#Region "Show Log"
    Private Function LoadFile(ByVal SourceFile As String) As String

        Try
            Dim fs As New FileStream(SourceFile, FileMode.OpenOrCreate, FileAccess.Read)
            Dim ByteArr(fs.Length) As Byte

            fs.Read(ByteArr, 0, fs.Length)
            fs.Close()

            Dim enc As System.Text.ASCIIEncoding = New System.Text.ASCIIEncoding()
            Dim LogStr As String = enc.GetString(ByteArr)
            Return LogStr
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Protected Sub TS_OnSelectedIndexChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
        logFiletxt.Text = ""
    End Sub

#End Region


#Region "Distance Calculation"

    Protected Sub Clear_Distance(ByVal sender As System.Object, ByVal e As System.EventArgs)
        TypeAheadBoxLocationFrom.Value = ""
        TypeAheadBoxLocationTo.Value = ""
        TextBoxHeight.Value = "0"
        CheckBoxUnidirection.Checked = False
        TypeAheadBoxHandlingEquipment.Value = ""
        logFiletxt.Value = ""
    End Sub

    Protected Sub Distance_Calculate(ByVal sender As System.Object, ByVal e As System.EventArgs)

        logFiletxt.Text = ""

        If TypeAheadBoxLocationFrom.Value = String.Empty Or TypeAheadBoxLocationTo.Value = String.Empty Then Return

        Dim logToPrint As StringBuilder = New StringBuilder()
        Dim fromLoc As String
        Dim toLoc As String
        Dim equipmentHeight As Double = -1
        Dim isUnidirecton As Boolean


        If TextBoxHeight.Value <> String.Empty Then
            If Not Double.TryParse(TextBoxHeight.Value, equipmentHeight) Then
                logToPrint.Append("Equipment Height entered is not a number." & vbCrLf)
            End If
        End If

        If equipmentHeight = 0 Or equipmentHeight = -1 Then
            If TypeAheadBoxHandlingEquipment.Value <> String.Empty Then
                If Not ValidateHandlingEquipmentType() Then
                    logToPrint.Append("Handling Equipment entered is not valid." & vbCrLf)
                Else
                    equipmentHeight = GetHandlingEquipmentHeight()
                End If
            End If
        End If

        If Not ValidateDataExistInWarehouseMapEdges() Then
            logToPrint.Append("No Data in WarehouseMapEdges Table." & vbCrLf)
        End If

        If Not ValidateDataExistInWarehouseMapNodes() Then
            logToPrint.Append("No Data in WarehouseMapNodes Table." & vbCrLf)
        End If

        If Not ValidateDataExistInWarehouseMapShortDistance() Then
            logToPrint.Append("No Data in WarehouseMapNodes Table." & vbCrLf)
        End If

        Dim locFrom As DataRow = ValidateLocation(TypeAheadBoxLocationFrom.Value)
        If Not locFrom Is Nothing Then
            If locFrom("PICKEDGE") = -1 Then
                logToPrint.Append(String.Format("PickEdge for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationFrom.Value))
            ElseIf locFrom("FILLEDGE") = -1 Then
                logToPrint.Append(String.Format("FillEdge for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationFrom.Value))
            ElseIf locFrom("XFILLCORDINATE") = -1 Then
                logToPrint.Append(String.Format("XFillCoordinate for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationFrom.Value))
            ElseIf locFrom("YFILLCORDINATE") = -1 Then
                logToPrint.Append(String.Format("YFillCoordinate for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationFrom.Value))
            End If
        Else
            logToPrint.Append(String.Format("Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationFrom.Value))
        End If

        Dim locTo As DataRow = ValidateLocation(TypeAheadBoxLocationTo.Value)
        If Not locTo Is Nothing Then
            If locTo("PICKEDGE") = -1 Then
                logToPrint.Append(String.Format("PickEdge for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationTo.Value))
            ElseIf locTo("FILLEDGE") = -1 Then
                logToPrint.Append(String.Format("FillEdge for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationTo.Value))
            ElseIf locTo("XFILLCORDINATE") = -1 Then
                logToPrint.Append(String.Format("XFillCoordinate for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationTo.Value))
            ElseIf locTo("YFILLCORDINATE") = -1 Then
                logToPrint.Append(String.Format("YFillCoordinate for Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationTo.Value))
            End If
        Else
            logToPrint.Append(String.Format("Location {0} doesnot exists ." & vbCrLf, TypeAheadBoxLocationTo.Value))
        End If

        If logToPrint.Length > 0 Then
            logFiletxt.Text = logToPrint.ToString()
            Return
        End If

        fromLoc = TypeAheadBoxLocationFrom.Value
        toLoc = TypeAheadBoxLocationTo.Value
        If ValidateDataExistInWarehouseMapEdgesForOneWay() Then
            isUnidirecton = CheckBoxUnidirection.Checked
        Else
            logToPrint.Append("No Oneway Data in WarehouseMapEdges Table." & vbCrLf)
            isUnidirecton = False
        End If

        If isUnidirecton Then
            Try
                CalculateShortestPathInterfaceTest(fromLoc, toLoc, equipmentHeight.ToString(), isUnidirecton, logToPrint)
                logFiletxt.Text = logToPrint.ToString()
            Catch ex As Exception
                logToPrint.Append(String.Format("Call to GetShortestPathWithContsraints Threw Exception. Exception message : {0}" & vbCrLf, ex.Message))
                logFiletxt.Text = logToPrint.ToString()
                Return
            End Try
        ElseIf equipmentHeight > 0 Then
            Try
                CalculateShortestPathInterfaceTest(fromLoc, toLoc, equipmentHeight, logToPrint)
                logFiletxt.Text = logToPrint.ToString()
            Catch ex As Exception
                logToPrint.Append(String.Format("Call to GetShortestPathWithContsraints Threw Exception. Exception message : {0}" & vbCrLf, ex.Message))
                logFiletxt.Text = logToPrint.ToString()
                Return
            End Try
        Else
            Try
                CalculateShortestPathInterfaceTest(fromLoc, toLoc, logToPrint)
                logFiletxt.Text = logToPrint.ToString()
            Catch ex As Exception
                logToPrint.Append(String.Format("Call to GetShortestPath Threw Exception. Exception message : {0}" & vbCrLf, ex.Message))
                logFiletxt.Text = logToPrint.ToString()
                Return
            End Try
        End If
    End Sub

    Private Function ValidateDataExistInWarehouseMapEdges() As Boolean
        Dim SQL As String = Nothing
        SQL = "select COUNT(*) from WAREHOUSEMAPEDGES"
        Dim count As Integer = DataInterface.ExecuteScalar(SQL)
        Return If(count > 0, True, False)
    End Function

    Private Function ValidateDataExistInWarehouseMapEdgesForOneWay() As Boolean
        Dim SQL As String = Nothing
        SQL = "select COUNT(*) from WAREHOUSEMAPEDGES Where EDGETYPE = 'ONEWAY'"
        Dim count As Integer = DataInterface.ExecuteScalar(SQL)
        Return If(count > 0, True, False)
    End Function

    Private Function ValidateDataExistInWarehouseMapShortDistance() As Boolean
        Dim SQL As String = Nothing
        SQL = "select COUNT(*) from WAREHOUSEMAPSHORTDISTANCE"
        Dim count As Integer = DataInterface.ExecuteScalar(SQL)
        Return If(count > 0, True, False)
    End Function

    Private Function ValidateDataExistInWarehouseMapNodes() As Boolean
        Dim SQL As String = Nothing
        SQL = "select COUNT(*) from WAREHOUSEMAPNODES"
        Dim count As Integer = DataInterface.ExecuteScalar(SQL)
        Return If(count > 0, True, False)
    End Function

    Private Function ValidateLocation(location As String) As DataRow
        Dim SQL As String = Nothing
        SQL = String.Format("select CASE WHEN PICKEDGE IS NOT NULL THEN PICKEDGE ELSE -1 END as PICKEDGE, CASE WHEN FILLEDGE IS NOT NULL THEN FILLEDGE ELSE -1 END as FILLEDGE, CASE WHEN XFILLCORDINATE IS NOT NULL THEN XFILLCORDINATE ELSE -1 END as XFILLCORDINATE, CASE WHEN YFILLCORDINATE IS NOT NULL THEN YFILLCORDINATE ELSE -1 END as YFILLCORDINATE from LOCATION where Location= '{0}'", location)
        Dim locationDetails As DataTable = New DataTable()
        DataInterface.FillDataset(SQL, locationDetails)
        If locationDetails.Rows.Count > 0 Then
            Return locationDetails.Rows.Item(0)
        Else
            Return Nothing
        End If
    End Function

    Private Sub CalculateShortestPathInterfaceTest(FromLoc As String, toLoc As String, height As String, isUnidirecton As Boolean, log As StringBuilder)
        Dim rules As New List(Of Rules)()
        Dim rule As New Rules()
        rule.Parameter = Made4Net.Algorithms.Constants.Height
        rule.Data = height
        rule.[Operator] = ">"
        rules.Add(rule)

        If isUnidirecton = True Then
            Dim rule1 As New Rules()
            rule1.Parameter = Made4Net.Algorithms.Constants.Equipment
            rule1.[Operator] = Made4Net.Algorithms.Constants.UniDirection
            rules.Add(rule1)
        End If

        Dim SHPath As ShortestPath
        SHPath = ShortestPath.GetInstance()


        Dim ps As New DataManager.ServiceModel.Path()

        log.Append("-------------------------------------------" & vbCrLf)
        log.Append("" & vbCrLf)

        ' Fetch Location
        Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
        Dim locationsFrom As New DataTable()
        DataInterface.FillDataset([String].Format(sql, FromLoc), locationsFrom)
        Dim from As DataRow = locationsFrom.Rows(0)
        Dim locationsTo As New DataTable()
        DataInterface.FillDataset([String].Format(sql, toLoc), locationsTo)
        Dim [to] As DataRow = locationsTo.Rows(0)

        Dim watch As Stopwatch = Stopwatch.StartNew()
        ps = SHPath.GetShortestPathWithContsraints(from, [to], "LDCNT1", "LOCCNT1", True, rules, Nothing)
        watch.Stop()

        log.Append(String.Format("Call to GetShortestPathWithContsraints took :{0} Milliseconds" & vbCrLf, watch.Elapsed.TotalMilliseconds))
        If (ps.Distance.SourceToTargetLocation > 0) Then
            log.Append(String.Format("The shortest distance is:{0}" & vbCrLf, ps.Distance.SourceToTargetLocation))

            log.Append("" & vbCrLf)
            log.Append("Node to Node Traversed Distance" & vbCrLf)
            log.Append("" & vbCrLf)
            Dim item As KeyValuePair(Of String, Double)
            For Each item In ps.TraversedNodes
                log.Append(String.Format("Node : {0} " & vbTab & "-- Distance : {1}" & vbCrLf, item.Key, item.Value))
            Next

            log.Append("-------------------------------------------" & vbCrLf)
        Else
            log.Append(String.Format("There is no valid path between the locations under given conditions, the distance returned is :{0}" & vbCrLf, ps.Distance.SourceToTargetLocation))
            log.Append("" & vbCrLf)
            log.Append("Node to Node Traversed Distance" & vbCrLf)
            log.Append("" & vbCrLf)
            Dim item As KeyValuePair(Of String, Double)
            For Each item In ps.TraversedNodes
                log.Append(String.Format("Node : {0} " & vbTab & "-- Distance : {1}" & vbCrLf, item.Key, item.Value))
            Next

            log.Append("-------------------------------------------" & vbCrLf)
        End If


    End Sub

    Private Sub CalculateShortestPathInterfaceTest(FromLoc As String, toLoc As String, log As StringBuilder)
        Dim SHPath As ShortestPath
        SHPath = ShortestPath.GetInstance()

        Dim ps As New DataManager.ServiceModel.Path()

        log.Append("-------------------------------------------" & vbCrLf)
        log.Append("" & vbCrLf)

        ' Fetch Location
        Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
        Dim locationsFrom As New DataTable()
        DataInterface.FillDataset([String].Format(sql, FromLoc), locationsFrom)
        Dim from As DataRow = locationsFrom.Rows(0)
        Dim locationsTo As New DataTable()
        DataInterface.FillDataset([String].Format(sql, toLoc), locationsTo)
        Dim [to] As DataRow = locationsTo.Rows(0)

        Dim watch As Stopwatch = Stopwatch.StartNew()
        ps = SHPath.GetShortestPath(from, [to], "LDCNT1", "LOCCNT1", True)
        watch.Stop()

        log.Append(String.Format("Call to GetShortestPath took :{0} Milliseconds" & vbCrLf, watch.Elapsed.TotalMilliseconds))

        If ps.Distance.SourceToTargetLocation > 0 Then
            log.Append(String.Format("The shortest distance is:{0}" & vbCrLf, ps.Distance.SourceToTargetLocation))

            log.Append("" & vbCrLf)

            log.Append("Node to Node Traversed Distance" & vbCrLf)
            log.Append("" & vbCrLf)
            Dim item As KeyValuePair(Of String, Double)
            For Each item In ps.TraversedNodes
                log.Append(String.Format("Node : {0,-30} {1} {2}" & vbCrLf, item.Key, "-- Distance :", item.Value))
            Next

            log.Append("-------------------------------------------" & vbCrLf)
        Else
            log.Append(String.Format("There is no valid path between the locations under given conditions, the distance returned is :{0}" & vbCrLf, ps.Distance.SourceToTargetLocation))
            log.Append("" & vbCrLf)
            log.Append("Node to Node Traversed Distance" & vbCrLf)
            log.Append("" & vbCrLf)
            Dim item As KeyValuePair(Of String, Double)
            For Each item In ps.TraversedNodes
                log.Append(String.Format("Node : {0} " & vbTab & "-- Distance : {1}" & vbCrLf, item.Key, item.Value))
            Next

            log.Append("-------------------------------------------" & vbCrLf)
        End If

    End Sub

    Private Sub CalculateShortestPathInterfaceTest(fromLoc As String, toLoc As String, height As Double, log As StringBuilder)
        Dim rules As New List(Of Rules)()
        Dim rule As New Rules()
        rule.Parameter = Made4Net.Algorithms.Constants.Height
        rule.Data = height
        rule.[Operator] = ">"
        rules.Add(rule)
        Dim SHPath As ShortestPath
        SHPath = ShortestPath.GetInstance()


        Dim ps As New DataManager.ServiceModel.Path()

        log.Append("-------------------------------------------" & vbCrLf)
        log.Append("" & vbCrLf)

        ' Fetch Location
        Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
        Dim locationsFrom As New DataTable()
        DataInterface.FillDataset([String].Format(sql, fromLoc), locationsFrom)
        Dim from As DataRow = locationsFrom.Rows(0)
        Dim locationsTo As New DataTable()
        DataInterface.FillDataset([String].Format(sql, toLoc), locationsTo)
        Dim [to] As DataRow = locationsTo.Rows(0)

        Dim watch As Stopwatch = Stopwatch.StartNew()
        ps = SHPath.GetShortestPathWithContsraints(from, [to], "LDCNT1", "LOCCNT1", True, rules, Nothing)
        watch.Stop()

        log.Append(String.Format("Call to GetShortestPathWithContsraints took :{0} Milliseconds" & vbCrLf, watch.Elapsed.TotalMilliseconds))
        If ps.Distance.SourceToTargetLocation > 0 Then
            log.Append(String.Format("The shortest distance is:{0}" & vbCrLf, ps.Distance.SourceToTargetLocation))

            log.Append("" & vbCrLf)
            log.Append("Node to Node Traversed Distance" & vbCrLf)
            log.Append("" & vbCrLf)
            Dim item As KeyValuePair(Of String, Double)
            For Each item In ps.TraversedNodes
                log.Append(String.Format("Node : {0} " & vbTab & "-- Distance : {1}" & vbCrLf, item.Key, item.Value))
            Next

            log.Append("-------------------------------------------" & vbCrLf)
        Else
            log.Append(String.Format("There is no valid path between the locations under given conditions, the distance returned is :{0}" & vbCrLf, ps.Distance.SourceToTargetLocation))
            log.Append("" & vbCrLf)
            log.Append("Node to Node Traversed Distance" & vbCrLf)
            log.Append("" & vbCrLf)
            Dim item As KeyValuePair(Of String, Double)
            For Each item In ps.TraversedNodes
                log.Append(String.Format("Node : {0} " & vbTab & "-- Distance : {1}" & vbCrLf, item.Key, item.Value))
            Next

            log.Append("-------------------------------------------" & vbCrLf)
        End If
    End Sub

    Private Function ValidateHandlingEquipmentType() As Boolean
        Dim SQL As String = Nothing
        SQL = "select COUNT(*) from HANDLINGEQUIPMENT Where HANDLINGEQUIPMENT = '{0}'"
        Dim count As Integer = DataInterface.ExecuteScalar(String.Format(SQL, TypeAheadBoxHandlingEquipment.Value))
        Return If(count > 0, True, False)
    End Function

    Private Function GetHandlingEquipmentHeight() As Double
        Dim SQL As String = Nothing
        SQL = "select HEIGHT from HANDLINGEQUIPMENT Where HANDLINGEQUIPMENT = '{0}'"
        Dim heightObj As Object = DataInterface.ExecuteScalar(String.Format(SQL, TypeAheadBoxHandlingEquipment.Value))
        If IsDBNull(heightObj) Then
            Return 0
        Else
            Return Double.Parse(heightObj)
        End If
    End Function

#End Region




End Class
Module StringExtensions

    <Extension()> _
    Public Function AsFixedWidth(ByVal value As String, desiredLength As Integer) As String
        Return value.PadRight(desiredLength).Substring(0, desiredLength)
    End Function

End Module