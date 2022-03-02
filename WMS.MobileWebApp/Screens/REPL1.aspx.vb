Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports Made4Net.DataAccess
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

<CLSCompliant(False)> Public Class REPL1
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

    Protected WithEvents ddUOM As Made4Net.WebControls.MobileDropDown
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            SetScreen()
        End If
        Try
            If Session("SELECTEDSKU") <> "" Then
                DO1.Value("SKU") = Session("SELECTEDSKU")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SetScreen()
        If (Session("REPLTSKTDetail") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        Dim replJob As ReplenishmentJob = ReplTask.getReplenishmentJob(ReplTaskDetail)
        Session("REPLJobDetail") = replJob

        DO1.Value("TASKTYPE") = replJob.TaskType
        DO1.Value("LOCATION") = replJob.fromLocation
        DO1.Value("FROMLOADID") = replJob.fromLoad
        DO1.Value("UNITS") = replJob.Units
        DO1.Value("CONSIGNEE") = replJob.Consignee
        DO1.Value("SKU") = replJob.Sku
        DO1.Value("SKUDESC") = replJob.skuDesc
        DO1.Value("UOMUNITS") = replJob.UOMUnits
        If ReplTaskDetail.ReplType = WMS.Logic.Replenishment.ReplenishmentTypes.FullReplenishment Then
            DO1.setVisibility("UOMUNITS", False)
        Else
            DO1.setVisibility("UOMUNITS", True)
        End If
        'Fill the problem code drop down if operation allowed
        Dim dd1 As Made4Net.WebControls.MobileDropDown
        dd1 = DO1.Ctrl("TaskProblemCode")
        'dd1.AllOption = False
        dd1.AllOption = True
        dd1.AllOptionText = ""
        dd1.TableName = "vTaskTypesProblemCodes"
        dd1.ValueField = "PROBLEMCODEID"
        dd1.TextField = "PROBLEMCODEDESC"

        'Commented for RWMS-2598 Start
        'dd1.Where = "TASKTYPE = '" & ReplTask.TASKTYPE & "'"
        'Commented for RWMS-2598 End

        'Added for RWMS-2598 Start
        dd1.Where = "TASKTYPE = '" & ReplTask.TASKTYPE & "' AND LOCATIONFLAG = 0 "
        'Added for RWMS-2598 End
        dd1.DataBind()

        If Session("SELECTEDSKU") <> "" Then
            Try
                DO1.Value("ITEM") = Session("SELECTEDSKU")
            Catch ex As Exception

            End Try
            ' Add all controls to session for restoring them when we back from that sreen
            Session.Remove("SELECTEDSKU")

        End If

        Try
            If dd1.GetValues.Count > 0 Then
                DO1.setVisibility("TaskProblemCode", True)
            Else
                DO1.setVisibility("TaskProblemCode", False)
            End If
        Catch ex As Exception
            DO1.setVisibility("TaskProblemCode", False)
        End Try
    End Sub

    Private Sub doNext()
        If CheckLoadId() Then

            Session.Remove("CONFIRMATIONTYPE")
            Response.Redirect(MapVirtualPath("Screens/REPL2.aspx"))
        End If
    End Sub

    Private Sub doMenu()
        Try
            'Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            If Not Session("REPLTSKTaskId") Is Nothing Then
                Dim ReplTask As WMS.Logic.ReplenishmentTask = New WMS.Logic.ReplenishmentTask(CType(Session("REPLTSKTaskId"), WMS.Logic.ReplenishmentTask).TASK)
                ReplTask.ExitTask()
                Session.Remove("REPLTSKTaskId")

                'Jira-332 cancel delete tasks on back button press
                Dim oTask As Task = Session("TMTask")
                Dim oRepl As New Replenishment(oTask.Replenishment)

                If Session("Manrepl") Or ((oTask.USERID = WMS.Logic.GetCurrentUser) And (oTask.ASSIGNED = True) And (oRepl.ReplMethod = WMS.Logic.Replenishment.ReplenishmentMethods.ManualReplenishment)) Then

                    If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE Then

                        If oRepl.Status = WMS.Lib.Statuses.Replenishment.PLANNED Then
                            oRepl.Cancel(WMS.Logic.GetCurrentUser)
                        End If

                        oTask.Cancel()
                    End If
                    Session.Remove("Manrepl")
                Else
                    ReplTask.ExitTask()
                    Session.Remove("REPLTSKTaskId")
                End If
                'ReplTask.ExitTask()
                'Session.Remove("REPLTSKTaskId")
            End If

            ManageMutliUOMUnits.Clear(True)
            Session.Remove("CONFIRMATIONTYPE")
            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
        Catch ex As Exception
        End Try
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    'Commented For PWMS-303 Start
    'Private Sub doOverrideLoad()
    '    Try
    '        Dim sql As String
    '        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
    '        If DO1.Value("FROMLOADID") = DO1.Value("SUBSTITUTELOAD") Then
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Cannot substitute a similar load")
    '            Return
    '        End If
    '        If Not WMS.Logic.Load.Exists(DO1.Value("SUBSTITUTELOAD")) Then
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Substitute load does not exist")
    '            Return
    '        End If
    '        Dim ld As New WMS.Logic.Load(DO1.Value("FROMLOADID"))
    '        Dim ldsub As New WMS.Logic.Load(DO1.Value("SUBSTITUTELOAD"))
    '        If ld.SKU <> ldsub.SKU Then
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "SKU does not match")
    '            Return
    '        End If
    '        If ld.LOADUOM <> ldsub.LOADUOM Then
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Load UOM does not match")
    '            Return
    '        End If

    '        Dim activityStatus As String = String.Empty
    '        activityStatus = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select isnull(ACTIVITYSTATUS,'') as ACTIVITYSTATUS from LOADS where LOADID='{0}'", DO1.Value("SUBSTITUTELOAD")))

    '        If Not activityStatus = String.Empty And Not activityStatus.ToUpper() = "REPLPEND" Then
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Cannot substitute a load unless the activity status is Null or Empty or replpend")
    '            Return
    '        End If

    '        Dim i, j As Integer
    '        sql = "select count(1) from ATTRIBUTE where pkeytype='load' and pkey1='{0}'"
    '        sql = String.Format(sql, DO1.Value("FROMLOADID"))
    '        i = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    '        sql = "select count(1) from ATTRIBUTE where pkeytype='load' and pkey1='{0}'"
    '        sql = String.Format(sql, DO1.Value("SUBSTITUTELOAD"))
    '        j = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

    '        'one of loads does not have attribute
    '        If i <> j Then
    '            If i = 1 And j = 0 Then
    '                sql = String.Format("SELECT * FROM ATTRIBUTE WHERE PKEYTYPE='LOAD' AND PKEY1='{0}' ", DO1.Value("FROMLOADID"))
    '            ElseIf i = 0 And j = 1 Then
    '                sql = String.Format("SELECT * FROM ATTRIBUTE WHERE PKEYTYPE='LOAD' AND PKEY1='{0}' ", DO1.Value("SUBSTITUTELOAD"))
    '            End If
    '            Dim dt As New DataTable
    '            Dim dr As DataRow
    '            DataInterface.FillDataset(sql, dt)
    '            If dt.Rows.Count = 0 Then
    '                'Exception
    '            End If
    '            dr = dt.Rows(0)
    '            If IsDBNull(dr("MFGDATE")) And IsDBNull(dr("EXPIRYDATE")) Then

    '            Else
    '                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Loads must have similar attributes")
    '                Return
    '            End If
    '        ElseIf i > 0 And j > 0 Then
    '            'if loads have attribut
    '            'lets compare all loads attributes
    '            sql = "select count(1) from vSimilarAttributes where fromload='{0}' and toload='{1}'"
    '            sql = String.Format(sql, DO1.Value("FROMLOADID"), DO1.Value("SUBSTITUTELOAD"))
    '            sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    '            If sql = "0" Then
    '                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Loads must have similar attributes")
    '                Return
    '            End If
    '            'else
    '            'botht loads does not have attribute - true
    '        End If

    '        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
    '        Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
    '        Dim replJob As ReplenishmentJob = Session("REPLJobDetail")

    '        replJob.TaskType = WMS.Lib.TASKTYPE.FULLREPL

    '        ReplTask.SubtitueLoad(DO1.Value("SUBSTITUTELOAD"), ReplTaskDetail, replJob, WMS.Logic.Common.GetCurrentUser)
    '        Session("REPLJobDetail") = replJob
    '        Response.Redirect(MapVirtualPath("Screens/REPL2.aspx"))
    '    Catch ex As Made4Net.Shared.M4NException
    '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
    '        Return
    '    Catch ex As Exception
    '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
    '        Return
    '    End Try
    'End Sub

    'End Commented For PWMS-303
    'Added for PWMS -303 Start

    Private Sub doOverrideLoad()
        Try
            Dim sql As String
            Dim sql1 As String
            Dim sql2 As String

            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            If DO1.Value("FROMLOADID") = DO1.Value("SUBSTITUTELOAD") Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Cannot substitute a similar load")
                Return
            End If
            If Not WMS.Logic.Load.Exists(DO1.Value("SUBSTITUTELOAD")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Substitute load does not exist")
                Return
            End If
            Dim ld As New WMS.Logic.Load(DO1.Value("FROMLOADID"))
            Dim ldsub As New WMS.Logic.Load(DO1.Value("SUBSTITUTELOAD"))
            If ld.SKU <> ldsub.SKU Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "SKU does not match")
                Return
            End If
            If ld.LOADUOM <> ldsub.LOADUOM Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Load UOM does not match")
                Return
            End If

            Dim activityStatus As String = String.Empty
            activityStatus = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select isnull(ACTIVITYSTATUS,'') as ACTIVITYSTATUS from LOADS where LOADID='{0}'", DO1.Value("SUBSTITUTELOAD")))

            If Not activityStatus = String.Empty And Not activityStatus.ToUpper() = "REPLPEND" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Cannot substitute a load unless the activity status is Null or Empty or replpend")
                Return
            End If

            Dim i, j As Integer
            sql = "select count(1) from ATTRIBUTE where pkeytype='load' and pkey1='{0}'"
            sql = String.Format(sql, DO1.Value("FROMLOADID"))
            i = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            sql = "select count(1) from ATTRIBUTE where pkeytype='load' and pkey1='{0}'"
            sql = String.Format(sql, DO1.Value("SUBSTITUTELOAD"))
            j = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

            'one of loads does not have attribute
            If i <> j Then
                If i = 1 And j = 0 Then
                    sql = String.Format("SELECT * FROM ATTRIBUTE WHERE PKEYTYPE='LOAD' AND PKEY1='{0}' ", DO1.Value("FROMLOADID"))
                ElseIf i = 0 And j = 1 Then
                    sql = String.Format("SELECT * FROM ATTRIBUTE WHERE PKEYTYPE='LOAD' AND PKEY1='{0}' ", DO1.Value("SUBSTITUTELOAD"))
                End If
                Dim dtt As New DataTable
                Dim dr As DataRow
                DataInterface.FillDataset(sql, dtt)
                If dtt.Rows.Count = 0 Then
                    'Exception
                End If
                dr = dtt.Rows(0)
                If IsDBNull(dr("MFGDATE")) And IsDBNull(dr("EXPIRYDATE")) Then

                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Loads must have similar attributes")
                    Return
                End If
            ElseIf i > 0 And j > 0 Then
                'if loads have attribut
                'lets compare all loads attributes
                sql = "select count(1) from vSimilarAttributes where fromload='{0}' and toload='{1}'"
                sql = String.Format(sql, DO1.Value("FROMLOADID"), DO1.Value("SUBSTITUTELOAD"))
                sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                If sql = "0" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Loads must have similar attributes")
                    Return
                End If
                'else
                'botht loads does not have attribute - true
            End If


            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            Dim replJob As ReplenishmentJob = Session("REPLJobDetail")

            replJob.TaskType = WMS.Lib.TASKTYPE.FULLREPL

            Dim loadAttribute As New Dictionary(Of String, Dictionary(Of String, String))

            Dim excludelist As String = "'PKEYTYPE', 'PKEY2', 'PKEY3','CAPTURED','FED','GRAZED','RAISED'"


            sql = String.Format("select Name from sys.columns where OBJECT_NAME(object_id)='attribute' and name not in ({0})", excludelist)

            Dim dt As New DataTable

            DataInterface.FillDataset(sql, dt)

            Dim columnList As String = String.Empty

            columnList = String.Join(",", (From row In dt.AsEnumerable Select row("Name")).ToArray)

            sql1 = String.Format("SELECT {0} FROM ATTRIBUTE WHERE PKEYTYPE='LOAD' and PKEY1 in ('{1}','{2}')", columnList, DO1.Value("FROMLOADID"), DO1.Value("SUBSTITUTELOAD"))

            Dim dt1 As New DataTable

            DataInterface.FillDataset(sql1, dt1)

            'Below condition will execute when both Original Load and Substitute Load does not have an entry in attribute table.
            If dt1.Rows.Count = 0 Then
                'Below condition will execute when Substitute Load has an entry in attribute table but Original Load does not have entry.
            ElseIf dt1.Rows.Count = 1 And dt1.Rows(0).Item("PKEY1") = DO1.Value("SUBSTITUTELOAD") Then
                'Below condition will execute when either both Original Load and Substitute Load have an entry in attribute table or only Original Load has an entry in attribute table.
            ElseIf dt1.Rows.Count = 2 Or dt1.Rows(0).Item("pkey1") = DO1.Value("FROMLOADID") Then
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


                sql2 = String.Format("SELECT {0} FROM (SELECT rs.* FROM REPLPOLICYSCORING rs join REPLENISHMENT rp on rs.POLICYID = rp.STRATEGYID WHERE rp.REPLID = '{1}')t", columnList_New, replJob.Replenishment)

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

                If dt1.Rows.Count = 2 And dt1.Rows(0).Item("pkey1") = DO1.Value("FROMLOADID") Then

                    Dim fValue, tValue As String
                    For Each attr As String In attributesToCompare
                        loadAttribute(DO1.Value("FROMLOADID")).TryGetValue(attr, fValue)
                        loadAttribute(DO1.Value("SUBSTITUTELOAD")).TryGetValue(attr, tValue)
                        If (String.Compare(fValue, tValue) <> 0) Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Loads must have similar attributes")
                            Return
                        End If
                    Next
                    'Below condition will execute when Only Original Load has an entry in attribute table and policy does not have weightage for any attribute.
                ElseIf dt1.Rows.Count = 1 And dt1.Rows(0).Item("pkey1") = DO1.Value("FROMLOADID") Then

                    If attributesToCompare.Count = 0 Then
                    End If

                End If

            End If

            ReplTask.SubtitueLoad(DO1.Value("SUBSTITUTELOAD"), ReplTaskDetail, replJob, WMS.Logic.Common.GetCurrentUser)
            Session("REPLJobDetail") = replJob
            Response.Redirect(MapVirtualPath("Screens/REPL2.aspx"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            'Added for RWMS-1989 and RWMS-1946 Start
            DO1.Value("SUBSTITUTELOAD") = ""
            'Added for RWMS-1989 and RWMS-1946 End
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
            Return
        End Try
    End Sub

    'End Added for PWMS -303
    Private Sub doBack()
        Try
            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            ReplTask.ExitTask()
            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
            Session.Remove("CONFIRMATIONTYPE")
        Catch ex As Exception
        End Try
        'Commented for RWMS-2505
        'Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
        'End Commented for RWMS-2505
        'Added for RWMS-2505
        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        'End Added for RWMS-2505
    End Sub

    'RWMS-1467 and RWMS-829 Added Start
    Private Sub doBackNew()
        'Commented for RWMS-2505 START
        'Try
        '    Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        '    'ReplTask.ExitTask()
        '    Session.Remove("REPLTSKTaskId")
        '    Session.Remove("REPLTSKTDetail")
        '    Session.Remove("CONFIRMATIONTYPE")
        'Catch ex As Exception
        'End Try
        'Commented for RWMS-2505 END

        'RWMS-2505 START - since newload is found, stay in the same screen REPL1.aspx. and refresh the existing repleishment task.
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        Dim refreshReplTask As WMS.Logic.ReplenishmentTask = New ReplenishmentTask(ReplTask.TASK)
        Dim refreshReplTaskDetail As WMS.Logic.Replenishment = New WMS.Logic.Replenishment(refreshReplTask.Replenishment)
        Dim refreshreplJob As ReplenishmentJob = ReplTask.getReplenishmentJob(refreshReplTaskDetail)

        Session.Remove("REPLTSKTaskId")
        Session.Remove("REPLTSKTDetail")
        Session.Remove("CONFIRMATIONTYPE")

        Session("REPLTSKTaskId") = refreshReplTask
        Session("REPLTSKTDetail") = refreshReplTaskDetail
        Session("REPLJobDetail") = refreshreplJob

        Response.Redirect(MapVirtualPath("Screens/REPL1.aspx"))
        'RWMS-2505 END

        'Commented for RWMS-2505 START
        'Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
        'Commented for RWMS-2505 END
    End Sub
    'RWMS-1467 and RWMS-829 Added End

    Private Function CheckLoadId() As Boolean

        Dim inpLoadId As String = DO1.Value("LOADID")
        Dim inpConsignee As String = DO1.Value("CONSIGNEE")
        Dim inpSku As String = DO1.Value("ITEM")
        Dim inpLocation As String = DO1.Value("LOC")
        Dim SQL As String
        'Dim SQL As String = String.Format("Select loadid from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku  where location = '{0}' and sku.consignee like '{1}%' and (sku.sku like '{2}%' or sku.othersku like '%{2}%') and loadid='{3}'", inpLocation, inpConsignee, inpSku, CType(Session("REPLTSKTDetail"), WMS.Logic.Replenishment).FromLoad.Trim)

        If (Session("REPLJobDetail") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        Dim replJob As ReplenishmentJob = Session("REPLJobDetail")
        Try
            inpConsignee = replJob.Consignee
            ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
            If Not String.IsNullOrEmpty(inpSku) Then
                ' Check for sku
                If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & inpConsignee & "' and (SKUCODE LIKE '" & inpSku & "' OR vSC.SKU LIKE '" & inpSku & "')") > 1 Then
                    ' Go to Sku select screen
                    Session("FROMSCREEN") = "REPL1"
                    Session("SKUCODE") = inpSku ' DO1.Value("SKU").Trim
                    ' Add all controls to session for restoring them when we back from that sreen
                    Session("SKUSEL_LOADID") = replJob.fromLoad ' DO1.Value("LOADID").Trim
                    Session("SKUSEL_QTY") = replJob.Units ' DO1.Value("QTY").Trim
                    Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed

                    'ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & Session("CreateLoadReciptId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & inpConsignee & "' and (SKUCODE LIKE '" & inpSku & "' OR vSC.SKU LIKE '" & inpSku & "')") = 1 Then
                    'DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN RECEIPTDETAIL RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.RECEIPT='" & Session("CreateLoadReciptId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
                    inpSku = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & inpConsignee & "' and (SKUCODE LIKE '" & inpSku & "' OR vSC.SKU LIKE '" & inpSku & "')")
                End If
                If Not WMS.Logic.SKU.Exists(inpConsignee, inpSku) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Illegal item")
                    Return False
                End If
                If replJob.Sku <> inpSku Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Wrong item confirmation")
                    Return False
                End If
            End If

            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            'CHECK location digits
            If Not String.IsNullOrEmpty(inpLocation) Then
                Dim FLAG As Boolean
                If Not CheckLocation(FLAG) Then
                    If FLAG Then HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Locations Does Not Match"))
                    Return False
                Else
                    inpLocation = replJob.fromLocation
                End If
            End If


            Dim CONFIRMATIONTYPE As String = Session("CONFIRMATIONTYPE")

            Select Case CONFIRMATIONTYPE ' LoadFindType(inpLoadId, inpLocation, inpConsignee, inpSku)
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOAD 'LoadSearchType.ByLoad
                    If String.IsNullOrEmpty(inpLoadId.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal load", "Illegal load")
                    SQL = String.Format("SELECT count(1) FROM LOADS WHERE LOADID = '{0}'", inpLoadId.Trim())
                    SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
                    If SQL = "0" Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Loads Does Not Match", "Loads Does Not Match")
                    End If

                    SQL = String.Format("SELECT LOADID FROM LOADS WHERE LOADID = '{0}'", inpLoadId.Trim())

                    inpLoadId = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

                    If inpLoadId.Trim.ToLower <> replJob.fromLoad.ToLower Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Loads Does Not Match"))
                        Return False
                    Else
                        Return True
                    End If

                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION 'LoadSearchType.ByLocationAndSku
                    If String.IsNullOrEmpty(inpLocation.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal location", "Illegal location")
                    If String.IsNullOrEmpty(inpSku.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal item", "Illegal item")
                    'SQL = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE LOADS.LOCATION LIKE '{0}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", inpLocation.Trim(), inpSku.Trim())
                    'Case LoadSearchType.ByLocationAndConsigneeAndSku
                    '    If String.IsNullOrEmpty(inpLocation.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal location", "Illegal location")
                    '    If String.IsNullOrEmpty(inpSku.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal SKU", "Illegal SKU")
                    '    If String.IsNullOrEmpty(inpConsignee.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal consignee", "Illegal consignee")
                    '    SQL = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE LOADS.LOCATION LIKE '{0}%' AND SKU.CONSIGNEE LIKE '{1}%' AND (SKU.SKU like '{2}%' or SKU.MANUFACTURERSKU like '{2}%' or SKU.VENDORSKU ='{2}' or SKU.OTHERSKU ='{2}')", inpLocation.Trim(), inpConsignee.Trim(), inpSku.Trim())
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOCATION ' LoadSearchType.ByLocation
                    If String.IsNullOrEmpty(inpLocation.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal location", "Illegal location")
                    'SQL = String.Format("SELECT LOADID FROM LOADS WHERE LOCATION LIKE '{0}%'", inpLocation.Trim())
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU ' LoadSearchType.BySku
                    If String.IsNullOrEmpty(inpSku.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal item", "Illegal item")
                    'SQL = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE SKU.CONSIGNEE LIKE '{0}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", inpConsignee.Trim(), inpSku.Trim())
            End Select

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return False
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return False
        End Try
        Return True

    End Function

    Private Function CheckLocation(ByRef flag As Boolean) As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try

            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
            Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(repljob.fromLocation, DO1.Value("LOC"), ReplTaskDetail.FromWarehousearea)

            'Dim inpLocation As String = DO1.Value("TOLOCATION")
            Dim inpWarehousearea As String = ReplTaskDetail.FromWarehousearea


            If strConfirmationLocation.Trim.ToLower <> repljob.fromLocation.Trim.ToLower OrElse _
                inpWarehousearea.Trim.ToLower <> repljob.toWarehousearea.Trim.ToLower Then
                Try
                    Dim locDesc As New WMS.Logic.Location(strConfirmationLocation.Trim.ToLower, inpWarehousearea.Trim.ToLower)
                    Dim locTo As New WMS.Logic.Location(repljob.fromLocation.Trim.ToLower, repljob.toWarehousearea.Trim.ToLower)

                    If locTo.CHECKDIGITS <> locDesc.CHECKDIGITS Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong location confirmation"))
                        flag = False
                        Return False
                    Else
                        Return True
                    End If
                Catch ex As Exception
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong location confirmation"))
                    flag = False
                    Return False
                End Try
            Else
                Return True
            End If
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong location confirmation"))
            flag = False
            Return False
        End Try
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TASKTYPE")
        DO1.AddLabelLine("FROMLOADID", "LOADID")
        'RWMS-2173 Start
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        'RWMS-2173 End
        DO1.AddLabelLine("SKU")
        DO1.setVisibility("SKU", False)
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("UOMUNITS")
        'DO1.AddLabelLine("UNITS")
        DO1.AddSpacer()
        'DO1.AddTextboxLine("LOADID")
        'DO1.AddTextboxLine("CONS", "CONSIGNEE")
        'DO1.AddTextboxLine("ITEM", "SKU")
        'DO1.AddTextboxLine("LOC", "LOCATION")

        'Dim osku As New WMS.Logic.SKU(replJob.Consignee, replJob.Sku)

        'DrowAttributes(osku)
        DrowConfirmFields()

        DO1.AddDropDown("TaskProblemCode")
        DO1.AddTextboxLine("SUBSTITUTELOAD")
    End Sub

    Public Function getCONFIRMATIONTYPE() As String
        Dim sql As String
        Dim sCONFTYPE As String
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        'RWMS-1379 Commented Start
        'Dim replJob As ReplenishmentJob = ReplTask.getReplenishmentJob(ReplTaskDetail)
        'RWMS-1379 Commented End

        'RWMS-1379 Start
        Dim replJob As ReplenishmentJob
        Try
            replJob = ReplTask.getReplenishmentJob(ReplTaskDetail)
            If replJob.IsHandOff = True Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Destination location is not Accessible by this equipment", "Destination location is not Accessible by this equipment")
            End If
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Return Nothing
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return Nothing
        End Try
        'RWMS-1379 End
        Dim POLICYID As String
        If ReplTaskDetail.ReplMethod = "NORMALREPL" Then
            POLICYID = "pl.NORMALREPLPOLICY"
        Else
            POLICYID = "pl.REPLPOLICY"
        End If

        sql = "SELECT rp.CONFTYPE FROM REPLPOLICYDETAIL AS rp INNER JOIN PICKLOC AS pl ON rp.POLICYID = {4} WHERE pl.CONSIGNEE='{0}' and pl.SKU='{1}' and pl.LOCATION='{2}' and pl.WAREHOUSEAREA='{3}'"
        sql = String.Format(sql, ReplTask.CONSIGNEE, ReplTask.SKU, ReplTask.TOLOCATION, ReplTask.TOWAREHOUSEAREA, POLICYID)
        sCONFTYPE = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Return sCONFTYPE
    End Function

    Public Sub DrowConfirmFields()
        Dim sCONFTYPE As String = getCONFIRMATIONTYPE()
        Session("CONFIRMATIONTYPE") = sCONFTYPE
        'Dim pl As New WMS.Logic.PickLoc(ReplTask.TOLOCATION, ReplTask.TOWAREHOUSEAREA, ReplTask.CONSIGNEE, ReplTask.SKU)

        'Dim relStrat As New WMS.Logic.ReleaseStrategyDetail(pl.NormalReplPolicy)

        If Not sCONFTYPE Is Nothing Then
            Select Case sCONFTYPE
                Case WMS.Lib.Release.CONFIRMATIONTYPE.NONE
                    Session("CONFIRMATIONTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
                    DO1.AddTextboxLine("LOADID", False, "", "LOADID")
                    Return
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOCATION
                    DO1.AddTextboxLine("LOC", False, "", "LOCATION")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
                    DO1.AddTextboxLine("LOADID", False, "", "LOADID")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU
                    DO1.AddTextboxLine("ITEM", False, "", "SKU")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION
                    DO1.AddTextboxLine("ITEM", False, "", "SKU")
                    DO1.AddTextboxLine("LOC", False, "", "LOCATION")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.UPC
                    '  DO1.AddTextboxLine("ITEM", "SKU")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKUUOM
                    '  DO1.AddTextboxLine("ITEM", "SKU")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATIONUOM
                    'DO1.AddTextboxLine("ITEM", "SKU")
                    'DO1.AddTextboxLine("LOC", "LOCATION")
            End Select
        Else
            Session("CONFIRMATIONTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
            DO1.AddTextboxLine("LOADID", False, "", "LOADID")
            Return
        End If

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "back"
                doMenu()
            Case "next"
                doNext()
            Case "subtituteload"
                doOverrideLoad()
            Case "reportproblem"
                ReportProblem()
        End Select
    End Sub

    Private Sub ReportProblem()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'RWMS-1467 and RWMS-829 Adedd Start
        Dim loadFound As Boolean = False
        'RWMS-1467 and RWMS-829 Adedd End

        Try
            'If Not CheckLocationOverrirde() Then
            '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Does Not Match"))
            '    Return
            'End If
            If String.IsNullOrEmpty(DO1.Value("TaskProblemCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No problem code selected"))
                Exit Sub
            End If
            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")

            Dim inpLocation As String = repljob.fromLocation

            Dim tran As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            ''CHECK location digits
            ''If Not String.IsNullOrEmpty(inpLocation) Then
            ''    Dim FLAG As Boolean
            ''    If Not CheckLocation(FLAG) Then
            ''        If FLAG Then HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  tran.Translate("Locations Does Not Match"))
            ''        Exit Sub
            ''    End If
            ''Else
            ''    If String.IsNullOrEmpty(inpLocation.Trim()) Then Throw New Made4Net.Shared.M4NException(New Exception, "Illegal location", "Illegal location")
            ''End If


            'Dim loc As New WMS.Logic.Location(inpLocation, repljob.toWarehousearea)
            'If loc.PROBLEMFLAG Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Location already marked as problematic", "Location already marked as problematic")
            'End If

            Dim sql As String
            Dim dt As New DataTable
            Dim ld As New WMS.Logic.Load(repljob.fromLoad)
            Dim prevLoc, prevWarehousearea As String
            sql = String.Format("SELECT top 1 * FROM vWHACTIVITY where  userid ='{0}' order by ACTIVITYTIME desc", UserId)
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                prevLoc = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("LOCATION"), "")
                prevWarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("WAREHOUSEAREA"), "")
            End If

            'ReplTask.ReportProblem(repl, DO1.Value("TaskProblemCode"), repljob.toLocation, repljob.toWarehousearea, WMS.Logic.Common.GetCurrentUser)
            sendProblemToAudit(repljob)

            Dim tsk As New WMS.Logic.Task(repljob.TaskId)


            tsk.ReportProblem(DO1.Value("TaskProblemCode"), inpLocation, repljob.toWarehousearea, WMS.Logic.Common.GetCurrentUser)

            'cansel task with execution location
            'Execution location, execution warehousearea, to load, to container,
            ' problem flag, and problem reason code are not updated in the tasks table
            tsk.ExecutionWarehousearea = repljob.toWarehousearea
            tsk.ExecutionLocation = inpLocation
            tsk.Save()
            'ReplTask.ExecutionWarehousearea = repljob.toWarehousearea
            'ReplTask.ExecutionLocation = repljob.toLocation
            'ReplTask.Save()
            'RWMS-1467 and RWMS-829 Commeted Start
            'ReplTask.Cancel()
            'RWMS-1467 and RWMS-829 Commeted End
            ' ReplTask.Save()

            'sql = String.Format("Update loads set location='{1}', activitystatus='',destinationlocation='',unitsallocated=0 where loadid='{0}'", repljob.fromLoad, repljob.toLocation)
            'Made4Net.DataAccess.DataInterface.RunSQL(sql)

            'RWMS-497 Finding alternate load for problem load
            'RWMS-1467 and RWMS-829 added Start
            Dim replMethod As String = repl.ReplMethod
            Dim replReplType As String = repl.ReplType
            Dim errMsg As String
            Dim pUser As String = WMS.Logic.GetCurrentUser

            Dim newReplLoad As Load = IsLoadExistsManual(tsk.TOLOCATION, tsk.TOWAREHOUSEAREA, tsk.CONSIGNEE, tsk.SKU, replMethod)


            If newReplLoad Is Nothing Then
                ReplTask.Cancel()
                errMsg = "Location problem reported. No alternate load found"
            Else
                'Commented for RWMS-2480 RWMS-2478
                'UpdateNewLoad(repl, tsk, newReplLoad, pUser)
                '' Passing param as per method signature. These params are not used inside as it resets value of loads
                'ld.CancelReplenish(repljob.fromLocation, repljob.fromWarehousearea, repljob.Units, pUser)
                'errMsg = "Location problem reported. Alternate Load ID is:" & newReplLoad.LOADID
                'loadFound = True
                'End Commented for RWMS-2480 RWMS-2478
                'Added for RWMS-2480 RWMS-2478 - checking the newReplLoad.LOADID for empty
                If newReplLoad.LOADID <> "" Then
                    UpdateNewLoad(repl, tsk, newReplLoad, pUser)
                    ' Passing param as per method signature. These params are not used inside as it resets value of loads
                    ld.CancelReplenish(repljob.fromLocation, repljob.fromWarehousearea, repljob.Units, pUser)
                    errMsg = "Location problem reported. Alternate Load ID is:" & newReplLoad.LOADID
                    loadFound = True
                Else
                    ReplTask.Cancel()
                    errMsg = "Location problem reported. No alternate load found"
                End If
                'End Added for RWMS-2480 RWMS-2478

            End If
            'RWMS-1467 and RWMS-829 added End

            Dim replen As New WMS.Logic.ReplenishmentTask
            Dim ldTemp As New WMS.Logic.Load(repljob.fromLoad)
            ldTemp.setStatus("PROBLEM", DO1.Value("TaskProblemCode"), WMS.Logic.GetCurrentUser)

            'RWMS-1467 and RWMS-829 Commeted Start
            'tsk.Cancel()
            'Dim ld As New WMS.Logic.Load(repljob.fromLoad)
            'ld.setStatus("PROBLEM", DO1.Value("TaskProblemCode"), WMS.Logic.GetCurrentUser)
            'RWMS-1467 and RWMS-829 Commeted End




            sql = String.Format("update LOCATION set PROBLEMFLAGRC=(SELECT LOCATIONPROBLEMRC FROM TASKPROBLEMCODE where PROBLEMCODEID = '{0}' and LOCATIONPROBLEM = 1) where LOCATION = '{1}' and WAREHOUSEAREA='{2}'", DO1.Value("TaskProblemCode"), inpLocation, repljob.toWarehousearea)
            Try
                Made4Net.DataAccess.DataInterface.RunSQL(sql)
            Catch ex As Exception
            End Try
            'create a new task of type MISC and subtype TRAVEL,
            'with from location as the user location from activity history, and to location as the problematic location.
            'Assign it to the user and complete it.
            Dim t As WMS.Logic.Task
            t = New WMS.Logic.Task()
            t.TASKTYPE = "MISC"
            t.TASKSUBTYPE = "TRAVEL"


            If String.IsNullOrEmpty(prevLoc) Then
                prevLoc = inpLocation
                prevWarehousearea = repljob.fromWarehousearea
            End If
            t.FROMLOCATION = prevLoc
            t.FROMWAREHOUSEAREA = prevWarehousearea
            t.TOLOCATION = inpLocation
            t.TOWAREHOUSEAREA = repljob.fromWarehousearea
            t.Create()
            t.AssignUser(UserId)
            t.Complete(WMS.Logic.LogHandler.GetRDTLogger)


            'RWMS-1467 and RWMS-829 Commeted Start
            'Session("REPLLDPCK") = ld.LOADID
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load problem reported"))
            'RWMS-1467 and RWMS-829 Commeted End

            'RWMS-1467 and RWMS-829 added Start
            Session("REPLLDPCK") = ldTemp.LOADID
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(errMsg))
            'RWMS-1467 and RWMS-829 added End


            ManageMutliUOMUnits.Clear(True)

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            Return
        End Try
        'RWMS-829 Commeted Start
        'doBack()
        'RWMS-829 Commeted End
        'RWMS-1467 and RWMS-829 added Start
        If Not loadFound Then
            doBack()
        Else
            doBackNew()
        End If
        'RWMS-1467 and RWMS-829 added End

    End Sub

    Private Sub sendProblemToAudit(ByVal pwjob As ReplenishmentJob)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim ld As New WMS.Logic.Load(pwjob.fromLoad)

        Dim MSG As String = "REPORTPROBLEM"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.REPORTPROBLEM)
        aq.Add("ACTIVITYTYPE", MSG)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", ld.CONSIGNEE)
        aq.Add("DOCUMENT", ld.RECEIPT)
        aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)
        aq.Add("FROMLOAD", ld.LOADID)
        aq.Add("FROMLOC", ld.LOCATION)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)

        Dim SQL As String
        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")

        SQL = "SELECT PROBLEMCODEDESC FROM vTaskTypesProblemCodes WHERE TASKTYPE = '" & ReplTask.TASKTYPE & "' AND PROBLEMCODEID = '" & DO1.Value("TaskProblemCode") & "'"
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        aq.Add("NOTES", SQL)
        aq.Add("SKU", ld.SKU)
        aq.Add("TOLOAD", ld.LOADID)


        aq.Add("TOLOC", ld.LOCATION)
        aq.Add("TOQTY", pwjob.Units)
        aq.Add("TOSTATUS", "PROBLEM")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(MSG)

    End Sub

    Private Function LoadFindType(ByVal LoadId As String, ByVal Loc As String, ByVal cons As String, ByVal sk As String) As Int32
        If LoadId <> "" Then Return LoadSearchType.ByLoad
        If Loc <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndSku
        If Loc <> "" And cons <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndConsigneeAndSku
        If cons <> "" And sk <> "" Then Return LoadSearchType.BySku
        If sk <> "" Then Return LoadSearchType.BySku
        If Loc <> "" Then Return LoadSearchType.ByLocation
    End Function

    Public Enum LoadSearchType
        ByLoad = 1
        ByLocationAndSku = 2
        ByLocationAndConsigneeAndSku = 3
        BySku = 4
        ByLocation = 6
    End Enum

    'Jira-RWMS 497 Check if load exists at regions as per policy

    Public Function IsLoadExistsManual(ByVal location As String, ByVal warehouse As String, ByVal consignee As String, ByVal sku As String, ByVal replMethod As String) As Load
        Dim strLOCATION As String = String.Empty
        Dim strWAREHOUSEAREA As String = String.Empty
        Dim strCONSIGNEE As String = String.Empty
        Dim strSKU As String = String.Empty
        Dim pReplPolicy As String = String.Empty
        Dim strPICKREGION As String = String.Empty
        Dim strUOM As String = String.Empty
        Dim currentqty As Double = 0
        Dim tmpcurrentqty As Double = 0
        Dim repl As New Replenishment()
        Dim isLoadFnd As Boolean = False


        'Commented for RWMS-2480 RWMS-2478
        'Dim zb As New PickLoc(location, warehouse, consignee, sku)
        'End Commented for RWMS-2480 RWMS-2478
        'Added for RWMS-2480 RWMS-2478 - If the location is not a picklocation (i.e. if its a flowrack location), then get the FrontTracklocation.
        Dim zb As PickLoc
        Dim sql As String = String.Format("Select isnull(FRONTRACKLOCATION,'') from LOCATION where LOCATION='{0}'", location)
        Dim frontTrackLoc As String = DataInterface.ExecuteScalar(sql)
        If String.IsNullOrEmpty(frontTrackLoc) Or frontTrackLoc Is Nothing Or frontTrackLoc = "" Or frontTrackLoc = " " Then
            zb = New PickLoc(location, warehouse, consignee, sku)
        Else
            zb = New PickLoc(frontTrackLoc, warehouse, consignee, sku)
        End If
        'End Added for RWMS-2480 RWMS-2478

        'Added for RWMS-1840
        Dim ReplPolicyMethod As String = WMS.Logic.Replenishment.GetPolicyID()
        'Ended for RWMS-1840

        If replMethod = ReplPolicyMethod Then
            'Commented for RWMS-1956 Start
            'pReplPolicy = zb.NormalReplPolicy
            'Commented for RWMS-1956 End

            'Added for RWMS-1956 Start
            pReplPolicy = zb.ReplPolicy
            'Added for RWMS-1956 End
        Else
            pReplPolicy = zb.ReplPolicy
        End If


        Dim prTbl As DataTable = New DataTable

        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY", prTbl)

        Dim dt1 As New DataTable
        Dim dr1 As DataRow

        'Commented for RWMS-2480 RWMS-2478
        'Dim load As New Load
        'End Commented for RWMS-2480 RWMS-2478
        'Added for RWMS-2480 RWMS-2478
        Dim load As WMS.Logic.Load
        'End Added for RWMS-2480 RWMS-2478

        dt1 = repl.CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            dt1.Rows.Add(repl.CreatePolicyDetailRow(dt1, pReplPolicy, pr("PRIORITY")))
        Next

        If dt1.Rows.Count > 0 Then
            For i As Int32 = 0 To dt1.Rows.Count - 1
                dr1 = dt1.Rows(i)
                strPICKREGION = dr1("PICKREGION")
                strUOM = dr1("UOM")
                'RWMS-2480 RWMS-2478 - creating instance of load
                load = New WMS.Logic.Load()
                load = getManualFullReplLoadInRegion(strPICKREGION, strUOM, pReplPolicy, zb, currentqty)

                If Not load Is Nothing Then
                    isLoadFnd = True
                    Exit For
                End If
            Next

        End If

        Return load


    End Function

    'Jira-497 New function for Manual replenishment-Not used
    Public Function getManualFullReplLoadInRegion(ByVal PickRegion As String, ByVal pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByVal CurrentQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "") As Load
        Dim sql As String
        Dim dt As New DataTable
        'Get Loads for scoring procedure

        If Not oLogger Is Nothing Then
            oLogger.Write("Inside->getManualFullReplLoadInRegion()")
        End If

        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{5}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {4} ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{4}' ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If

        Dim FullUomSize As Decimal
        If pAllocUOMQty <> "" Then
            Dim oSku As New SKU(zb.Consignee, zb.SKU)
            FullUomSize = oSku.ConvertToUnits(pAllocUOMQty)
            sql = sql & " and UNITSAVAILABLE = " & FullUomSize
        End If

        sql = sql & " ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC"

        DataInterface.FillDataset(sql, dt)

        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to get loads according to query:")
            oLogger.Write(sql.ToString)
        End If

        If dt.Rows.Count > 0 Then
            ' Loads found for scoring
            Dim rps As ReplenishmentPolicyScoring = New ReplenishmentPolicyScoring(pPolicyID)
            Dim arrLoadsToCheck As DataRow()
            arrLoadsToCheck = dt.Select()
            'Run scoring procedure
            rps.Score(arrLoadsToCheck, oLogger)
            'if there is more than one load then return the best load from scoring run
            Dim ld As New Load(Convert.ToString(arrLoadsToCheck(0)("LOADID")))
            Return ld
        Else
            ' No loads found for scoring
            If Not oLogger Is Nothing Then
                oLogger.Write("NO MATCHING LOADS FOUND.")
                oLogger.writeSeperator(" ", 20)
            End If
            Return Nothing
        End If
    End Function
    Public Function getStockFilterByPriority(ByVal sPickRegions As String, ByVal sUoms As String) As String
        Dim aPickRegions As String() = sPickRegions.Split(",")
        Dim aUoms As String() = sUoms.Split(",")

        If aPickRegions.Length = 1 Then
            Return " (PICKREGION LIKE '" & sPickRegions & "' AND LOADUOM LIKE '" & sUoms & "') "
        End If
        If aUoms.Length >= 1 Then
            sUoms = aUoms(0)
        End If
        Dim SQLFilter As String = "LOADUOM LIKE '" & sUoms & "' AND ("
        For i As Int32 = 0 To aPickRegions.Length - 1
            SQLFilter = SQLFilter & " (PICKREGION LIKE '" & aPickRegions(i) & "') OR"
        Next
        SQLFilter = " (" & SQLFilter.TrimEnd("OR".ToCharArray) & ")) "
        Return SQLFilter
    End Function

    Public Sub UpdateNewLoad(ByVal replTask As WMS.Logic.Replenishment, ByVal Task As WMS.Logic.Task, ByVal newLoad As Load, ByVal user As String)

        Dim sql As String
        Dim loadid As String = Made4Net.Shared.Util.FormatField(newLoad.LOADID)
        Dim loadid1 As String = newLoad.LOADID

        Try


            sql = String.Format("UPDATE LOADS SET DESTINATIONLOCATION ={0}, ACTIVITYSTATUS ={1}, UNITSALLOCATED={2}, edituser = {4}, editdate = {5} WHERE LOADID ={3} ", Made4Net.Shared.Util.FormatField(Task.TOLOCATION), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.ActivityStatus.REPLPENDING), Made4Net.Shared.Util.FormatField(newLoad.UNITS), Made4Net.Shared.Util.FormatField(loadid1), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(DateTime.Now))
            'sql = String.Format("UPDATE LOADS SET DESTINATIONLOCATION ={0), ACTIVITYSTATUS ={1} WHERE LOADID ={2} ", Made4Net.Shared.Util.FormatField(Task.TOLOCATION), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.ActivityStatus.REPLPENDING), Made4Net.Shared.Util.FormatField(newLoad.LOADID))
            DataInterface.RunSQL(sql)

            ' For negative replen type the ToLoad in Replenishment and Task table should not be updated as the load may split

            If (replTask.ReplType = WMS.Logic.Replenishment.ReplenishmentTypes.FullReplenishment) Then

                'sql = String.Format("UPDATE TASKS SET FROMLOAD={1}, TOLOAD={5}, FROMLOCATION={2}, ASSIGNED=0 , USERID={3}, STATUS={4} WHERE TASK = {0} ", Made4Net.Shared.Util.FormatField(Task.TASK), Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(newLoad.LOCATION), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Task.AVAILABLE), Made4Net.Shared.Util.FormatField(newLoad.LOADID))
                sql = String.Format("UPDATE TASKS SET FROMLOAD={1}, TOLOAD={4}, FROMLOCATION={2}, USERID={3}, ASSIGNED=1 , PROBLEMFLAG=0, PROBLEMRC={5}, STATUS={6} WHERE TASK = {0} ", Made4Net.Shared.Util.FormatField(Task.TASK), Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(newLoad.LOCATION), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(String.Empty), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Task.ASSIGNED))
                DataInterface.RunSQL(sql)

                sql = String.Format("UPDATE REPLENISHMENT SET FROMLOAD={0}, TOLOAD={5}, FROMLOCATION={2}, edituser = {3}, UNITS={6} , editdate = {4} WHERE REPLID = {1} ", Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(replTask.ReplId), Made4Net.Shared.Util.FormatField(newLoad.LOCATION), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(newLoad.UNITS))
                DataInterface.RunSQL(sql)
            Else
                'sql = String.Format("UPDATE TASKS SET FROMLOAD={1}, FROMLOCATION={2}, ASSIGNED=0 , USERID={3}, STATUS={4} WHERE TASK = {0} ", Made4Net.Shared.Util.FormatField(Task.TASK), Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(newLoad.LOCATION), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Task.AVAILABLE))
                sql = String.Format("UPDATE TASKS SET FROMLOAD={1}, FROMLOCATION={2}, USERID={3}, ASSIGNED=1, PROBLEMFLAG=0, PROBLEMRC={4}, STATUS={5} WHERE TASK = {0} ", Made4Net.Shared.Util.FormatField(Task.TASK), Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(newLoad.LOCATION), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(String.Empty), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Task.ASSIGNED))
                DataInterface.RunSQL(sql)

                sql = String.Format("UPDATE REPLENISHMENT SET FROMLOAD={0}, FROMLOCATION={2}, edituser = {3}, UNITS={4}, editdate = {5} WHERE REPLID = {1} ", Made4Net.Shared.Util.FormatField(newLoad.LOADID), Made4Net.Shared.Util.FormatField(replTask.ReplId), Made4Net.Shared.Util.FormatField(newLoad.LOCATION), Made4Net.Shared.Util.FormatField(user), Made4Net.Shared.Util.FormatField(newLoad.UNITS), Made4Net.Shared.Util.FormatField(DateTime.Now))
                DataInterface.RunSQL(sql)

            End If


        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))

        End Try


    End Sub



End Class