Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RPK
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
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
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPKC.aspx"))
            End If
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                If Session("LoadPUSrcScreen") <> String.Empty Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=" & Session("LoadPUSrcScreen")))
                    Session.Remove("LoadPUSrcScreen")
                Else
                    If Not IsNothing(Session("MobileSourceScreen")) AndAlso Session("MobileSourceScreen") <> "" Then
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    Else
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=Main"))
                    End If

                End If

            End If

            If Not String.IsNullOrEmpty(Session("CREATELOADPCKUP")) Then
                DO1.Value("LOADID") = Session("CREATELOADPCKUP")
                Session.Remove("CREATELOADPCKUP")
                doNext()
            End If

            If Not String.IsNullOrEmpty(Session("REPLLDPCK")) Then
                DO1.Value("LOADID") = Session("REPLLDPCK")
                Session.Remove("REPLLDPCK")
                doNext()
            End If
        End If
        If Session("NSPICKUPLOAD") <> "" Then
            DO1.Value("LOADID") = Session("NSPICKUPLOAD")
            Session.Remove("NSPICKUPLOAD")
            doNext()
        End If
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOADID") = Session("SKUSEL_LOADID")
            DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_LOADID")
            Session.Remove("SKUSEL_LOCATION")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("RPKLoadID")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If DO1.Value("LOADID").Trim = "" Then
                ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
                If DO1.Value("SKU").Trim <> "" Then
                    ' Check for sku
                    If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                        ' Go to Sku select screen
                        Session("FROMSCREEN") = "RPK"
                        Session("SKUCODE") = DO1.Value("SKU").Trim
                        ' Add all controls to session for restoring them when we back from that sreen
                        Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                        Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                        Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                        Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx"))
                    ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                        DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
                    End If
                End If
            End If

            Dim strLoadid As String = getLoadId()
            Dim prePopulateLocation As String ' RWMS-1277
            prePopulateLocation = "" ' RWMS-1277
            If strLoadid = "" Then
                'err message print from getLoadId() function

                'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No Loads/More than one load were found"))
                Return
            End If
            Session("RPKLoadID") = DO1.Value("LOADID")
            Dim strTask As String
            Dim oLoad As New WMS.Logic.Load(strLoadid)
            'oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser)
            'Added for RWMS-478
            'write a sql statement to check whether theere are any other older loads exists other than this load.
            'if it finds an other load which is older than this load then call the new overloaded RequestPickUp.
            'else call the existing RequestPickUp
            'Dim sql As String
            'sql = String.Format("SELECT COUNT(*) FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND ( SELECT TOP 1 EXPIRYDATE FROM vReplenishmentInventory WHERE SKU = '{1}' AND LOADID = '{2}' ) > EXPIRYDATE ", oLoad.CONSIGNEE, oLoad.SKU, strLoadid)
            'If DataInterface.ExecuteScalar(sql) Then
            '    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, strLoadid, prePopulateLocation) 'RWMS-1277
            'Else
            '    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, prePopulateLocation) 'RWMS-1277
            'End If
            'End Added for RWMS-478
            'Commented for RWMS-478
            'oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser)
            'End Commented for RWMS-478

            'Added for RWMS-2079 and RWMS-1698(RWMS-1700) Start
            Dim sql As String
            If Not String.IsNullOrEmpty(oLoad.LoadAttributes.Attribute("EXPIRYDATE")) Then
                sql = String.Format("SELECT COUNT(*) FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND ( SELECT TOP 1 EXPIRYDATE FROM vReplenishmentInventory WHERE SKU = '{1}' AND LOADID = '{2}' ) > EXPIRYDATE ", oLoad.CONSIGNEE, oLoad.SKU, strLoadid)
                If DataInterface.ExecuteScalar(sql) Then
                    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, strLoadid, prePopulateLocation)
                Else
                    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, prePopulateLocation)
                End If
            ElseIf Not String.IsNullOrEmpty(oLoad.LoadAttributes.Attribute("MFGDATE")) Then

                sql = String.Format("SELECT COUNT(*) FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND ( SELECT TOP 1 MFGDATE FROM vReplenishmentInventory WHERE SKU = '{1}' AND LOADID = '{2}' ) > MFGDATE ", oLoad.CONSIGNEE, oLoad.SKU, strLoadid)
                If DataInterface.ExecuteScalar(sql) Then
                    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, strLoadid, prePopulateLocation)
                Else
                    oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, prePopulateLocation)
                End If
            Else
                oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, prePopulateLocation)
            End If
            'Added for RWMS-2079 and  RWMS-1698(RWMS-1700) End

            'Commented for RWMS-2079 and RWMS-1698(RWMS-1700) Start
            'oLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser)
            'Commented for RWMS-2079 and  RWMS-1698(RWMS-1700) End

            strTask = isAssignedToTask(oLoad.LOADID)
            If strTask <> "" Then
                Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                taskFoundRedirector(oTask)
            Else
                Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=RPK"))
            End If
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub taskFoundRedirector(ByVal pTaskObj As WMS.Logic.Task)
        pTaskObj.AssignUser(WMS.Logic.Common.GetCurrentUser)
        If isReplTask(pTaskObj) Then
            Dim ReplTask As New WMS.Logic.ReplenishmentTask(pTaskObj.TASK)
            Session("REPLTSKTaskId") = ReplTask
            Dim ReplTaskDetail As New WMS.Logic.Replenishment(ReplTask.Replenishment)
            Session("REPLTSKTDetail") = ReplTaskDetail
            Session("IsLoadPickUp") = "1"
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Session("TMTask") = ReplTask
                Session("TargetScreen") = "Screens/REPL2.aspx?sourcescreen=RPK"
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/REPL2.aspx?sourcescreen=RPK"))
            End If
        ElseIf pTaskObj.TASKTYPE = WMS.Lib.TASKTYPE.LOADPUTAWAY Then
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Session("TMTask") = pTaskObj
                Session("TargetScreen") = "Screens/RPK2.aspx?sourcescreen=RPK"
                Session("SkipLoadScanScreen") = 2
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=RPK"))
            End If
        Else
            Session("TASKMANAGERSOURCESCREENID") = "rdtrpk"
            Session("TASKMANAGERSHOULDREDIRECTTOSRC") = "0"
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        End If

    End Sub

    Private Function isReplTask(ByVal pTaskObj As WMS.Logic.Task) As Boolean
        If pTaskObj.TASKTYPE = WMS.Lib.TASKTYPE.FULLREPL OrElse pTaskObj.TASKTYPE = WMS.Lib.TASKTYPE.PARTREPL OrElse _
            pTaskObj.TASKTYPE = WMS.Lib.TASKTYPE.NEGTREPL Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU")
        DO1.AddTextboxLine("LOCATION")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    ' Returns TaskID that this load is assigned to
    Private Function isAssignedToTask(ByVal LoadId As String) As String
        Dim strSql As String = "SELECT TASK FROM TASKS WHERE (STATUS <> 'COMPLETE' AND STATUS <> 'CANCELED') AND FROMLOAD='" & LoadId & "' AND TASKTYPE <> 'COUNTING'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

    Private Function getLoadId() As String
        Try
            Dim oSku As WMS.Logic.SKU
            If DO1.Value("SKU") <> "" And DO1.Value("CONSIGNEE", Session("consigneeSession")) <> "" Then
                oSku = New WMS.Logic.SKU(DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("SKU"))
            Else
                oSku = New WMS.Logic.SKU
            End If
            Dim SQL As String
            If DO1.Value("LOADID") <> "" Then
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid = '{0}' and location like '{1}%' and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and invload.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")))
            ElseIf DO1.Value("LOCATION") <> "" Then
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid like '{0}%' and location = '{1}' and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and invload.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")))
            Else
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid like '{0}%' and location like '{1}%' and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and invload.consignee = '{3}'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")))
            End If
            Dim dt As New DataTable
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Throw New M4NException(New Exception, "No Loads were found", "No Loads were found")
            ElseIf dt.Rows.Count > 1 Then
                Throw New M4NException(New Exception, "More than 1 load was found", "More than 1 load was found")
            End If
            Return dt.Rows(0)("LOADID")
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
        Return String.Empty
    End Function

End Class