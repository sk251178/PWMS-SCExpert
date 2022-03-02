Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports WMS.Logic
<CLSCompliant(False)> Public Class LDPW
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
        If Not IsPostBack Then
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=LDPW"))
            End If
            Session.Remove("TASKMANAGERSOURCESCREENID")
            Session.Remove("TASKMANAGERSHOULDREDIRECTTOSRC")
        End If
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOADID") = Session("SKUSEL_LOADID")
            DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
            DO1.Value("WAREHOUSEAREA") = Session("SKUSEL_WAREHOUSEAREA")

            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_LOADID")
            Session.Remove("SKUSEL_RECEIPTLINE")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If

    End Sub

    Private Sub doMenu()
        Session.Remove("LDPWLoadID")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU")
        DO1.AddTextboxLine("LOCATION")
        'DO1.AddTextbox Line("WAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("LOADID").Trim = "" Then
            If DO1.Value("SKU").Trim <> "" Then
                ' Check for sku
                If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & WMS.Logic.Warehouse.getUserWarehouseArea() & "'  WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                    ' Go to Sku select screen
                    Session("FROMSCREEN") = "LDPW"
                    Session("SKUCODE") = DO1.Value("SKU").Trim
                    ' Add all controls to session for restoring them when we back from that sreen
                    Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                    Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                    Session("SKUSEL_WAREHOUSEAREA") = WMS.Logic.Warehouse.getUserWarehouseArea()

                    Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                    Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
                ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & Session("SKUSEL_WAREHOUSEAREA") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                    DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & Session("SKUSEL_WAREHOUSEAREA") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
                End If
            End If
        End If

        Dim strLoadid As String = getLoadId()
        If strLoadid = "" Then
            Return
        End If
        Session("LDPWLoadID") = DO1.Value("LOADID")
        Try
            Dim strTask As String = isAssignedToTask(strLoadid)
            If strTask <> "" Then
                Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                If oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARTREPL Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.FULLREPL Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.NEGTREPL Then
                    Dim ReplTask As New WMS.Logic.ReplenishmentTask(strTask)
                    Session("REPLTSKTaskId") = ReplTask
                    Dim ReplTaskDetail As New WMS.Logic.Replenishment(ReplTask.Replenishment)
                    Session("REPLTSKTDetail") = ReplTaskDetail
                    If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                        Session("TMTask") = ReplTask
                        Session("TargetScreen") = "Screens/REPL2.aspx?sourcescreen=LDPW"
                        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                    Else
                        Response.Redirect(MapVirtualPath("Screens/REPL2.aspx?sourcescreen=LDPW"))
                    End If
                Else
                    'Session("TASKMANAGERSOURCESCREENID") = "LDPW"
                    'Session("TASKMANAGERSHOULDREDIRECTTOSRC") = "0"
                    Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
                End If
            End If
            Dim pwProc As New Putaway
            Dim destLoc, destWHArea, prepopulateLocation As String 'RWMS-1277
            prepopulateLocation = "" 'RWMS-1277
            pwProc.RequestDestinationForLoad(strLoadid, destLoc, destWHArea, 0, prepopulateLocation) 'RWMS-1277
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Dim pwtask As PutawayTask
                Dim tm As New WMS.Logic.TaskManager
                pwtask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
                Session("TMTask") = pwtask
                Session("TargetScreen") = "Screens/RPK2.aspx?sourcescreen=LDPW"
                Session("LoadPUSrcScreen") = "LDPW"
                Session("MobileSourceScreen") = "LDPW"
                Session("SkipLoadScanScreen") = 1
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Session("MobileSourceScreen") = "LDPW"
                Session("LoadPUSrcScreen") = "LDPW"
                Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=LDPW"))
            End If

        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
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
        Dim strSql As String = "SELECT TASK FROM TASKS WHERE (STATUS <> 'COMPLETE' AND STATUS <> 'CANCELED') AND FROMLOAD='" & LoadId & "'"   ' AND TASKTYPE <> 'COUNTING' AND TASKTYPE <> 'LOADPW' AND TASKTYPE NOT LIKE '%REPL'"
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
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid = '{0}' and location like '{1}%' and warehousearea like '{4}%' and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and invload.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), Session("SKUSEL_WAREHOUSEAREA"))
            ElseIf DO1.Value("LOCATION") <> "" Then
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid like '{0}%' and location = '{1}' and warehousearea like '{4}%'  and (sku.sku like '{2}%' or sku.othersku like '{2}%') and invload.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), Session("SKUSEL_WAREHOUSEAREA"))
            Else
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid like '{0}%' and location like '{1}%' and warehousearea like '{4}%'  and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and invload.consignee = '{3}'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), Session("SKUSEL_WAREHOUSEAREA"))
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