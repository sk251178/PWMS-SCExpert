Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class RPKC1
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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If String.IsNullOrEmpty(Session("RPKCSourceScreen")) Then
            If Not Request.QueryString.Item("sourcescreen") Is Nothing Then
                Session("RPKCSourceScreen") = Request.QueryString.Item("sourcescreen")
            Else
                Session("RPKCSourceScreen") = "taskmanager"
            End If
        End If
        doNext()
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("CONTAINERID")
        If String.IsNullOrEmpty(Session("RPKCSourceScreen")) Then
            Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
        Else
            Dim destScreen As String = Session("RPKCSourceScreen")
            Session.Remove("RPKCSourceScreen")
            Response.Redirect(MapVirtualPath("Screens/" & destScreen & ".aspx"))
        End If

        'Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try
            Dim actStat, currLoadId As String
            Dim cntId As String = Session("CONTAINERID")
            'Dim SQL As String = String.Format("select top 1 * from containerloads cl inner join invload il on cl.loadid = il.loadid " & _
            '        "inner join location on location.location = il.destinationlocation and location.warehousearea = il.destinationwarehousearea where containerid = '{0}' " & _
            '        "ORDER BY LOCATION.LOCSORTORDER", cntId)
            Dim SQL As String = String.Format("select top 1 * from CONTLOADS cl inner join invload il on cl.loadid = il.loadid " & _
                    "inner join location on location.location = il.destinationlocation and location.warehousearea = il.destinationwarehousearea where containerid = '{0}' " & _
                    "ORDER BY LOCATION.LOCSORTORDER", cntId)
            Dim dt As New DataTable
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                '                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container Unloaded"))
                If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container Unloaded"))
                    Dim pwtask As Logic.PutawayTask
                    Dim tm As New WMS.Logic.TaskManager
                    pwtask = tm.getAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONTLOADPUTAWAY, WMS.Logic.LogHandler.GetRDTLogger())
                    pwtask.Complete(WMS.Logic.LogHandler.GetRDTLogger)
                    If MobileUtils.ShouldRedirectToTaskSummary() Then
                        Session("TaskID") = pwtask.TASK
                        'MobileUtils.RedirectToTaskSummary(Session("MobileSourceScreen"))
                        If String.IsNullOrEmpty(Session("RPKCSourceScreen")) Then
                            MobileUtils.RedirectToTaskSummary("RPKC")
                        Else
                            Dim destScreen As String = Session("RPKCSourceScreen")
                            Session.Remove("RPKCSourceScreen")
                            MobileUtils.RedirectToTaskSummary(destScreen)
                        End If
                    End If
                End If
                doBack()
            Else
                currLoadId = dt.Rows(0)("loadid")
                actStat = dt.Rows(0)("activitystatus")
                If actStat = WMS.Lib.Statuses.ActivityStatus.REPLPENDING Then
                    Dim oReplTask As New WMS.Logic.ReplenishmentTask
                    oReplTask.ASSIGNMENTTYPE = WMS.Lib.TASKASSIGNTYPE.MANUAL
                    Session("REPLTSKTaskId") = oReplTask
                    Session("REPLTSKTDetail") = WMS.Logic.Replenishment.getReplenishment(currLoadId)
                    Response.Redirect(MapVirtualPath("Screens/REPL2.aspx?sourcescreen=RPKC1"))
                Else
                    Session("LOADIDONCONTAINER") = currLoadId
                    Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=RPKC1"))
                End If
            End If
        Catch ex As Threading.ThreadAbortException
            'do nothing
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("ContainerId")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "menu"
                doBack()
        End Select
    End Sub

End Class