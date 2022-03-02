Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class REPL
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

        WriteToRDTLog(" REPL.Page_Load")
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
            If Not Request.QueryString.Item("sourcescreen") Is Nothing Then
                Session("REPLSRCSCREEN") = Request.QueryString.Item("sourcescreen")
            Else
                Session("REPLSRCSCREEN") = "REPL"
            End If

            Session("MobileSourceScreen") = "REPL"
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=Repl"))
            End If
            If CheckAssigned() Then
                If MobileUtils.ShowGoalTimeOnTaskAssignment() AndAlso Not Session("ShowedTaskManager") Then
                    Session("TargetScreen") = "screens/REPL1.aspx"
                    Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                End If
            End If
            doNext()
        End If
    End Sub

    Private Function CheckAssigned() As Boolean
        Dim ReplTask As WMS.Logic.ReplenishmentTask
        Dim ReplTaskDetail As WMS.Logic.Replenishment
        Dim tm As New WMS.Logic.TaskManager
        WriteToRDTLog(" REPL.CheckAssigned")

        Dim sessionTask As WMS.Logic.Task = Session("TMTask")
        WriteToRDTLog("Session('TMTask').TASK={0}", If(sessionTask IsNot Nothing, sessionTask.TASK, "Nothing"))
        WriteToRDTLog("Session('TMTask').STATUS={0}", If(sessionTask IsNot Nothing, sessionTask.STATUS, "Nothing"))

        ' If there's an assigned task stored in the session, use that session task first
        If sessionTask IsNot Nothing Then
            ReplTask = WMS.Logic.TaskManager.GetTask(sessionTask.TASK)
            If ReplTask IsNot Nothing Then
                WriteToRDTLog("Loaded task {0}. STATUS={1}", ReplTask.TASK, ReplTask.STATUS)
                If ReplTask.STATUS <> WMS.Lib.Statuses.Task.ASSIGNED Then
                    ReplTask = Nothing
                End If
            End If
        Else
            WriteToRDTLog("GetTask returned nothing")
        End If

        If ReplTask Is Nothing Then
            WriteToRDTLog(" REPL.CheckAssigned RequestTask")
            ReplTask = tm.RequestTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.REPLENISHMENT, WMS.Logic.LogHandler.GetRDTLogger())
        End If

        If Not ReplTask Is Nothing Then
            WriteToRDTLog("Requested task {0}. STATUS={1}", ReplTask.TASK, ReplTask.STATUS)
            Session.Add("REPLTSKTaskId", ReplTask)
            ReplTaskDetail = New WMS.Logic.Replenishment(ReplTask.Replenishment)
            Session.Add("REPLTSKTDetail", ReplTaskDetail)
            DO1.Value("ASSIGNED") = "ASSIGNED"
            DO1.Value("TASKID") = ReplTask.TASK 'Session("REPLTSKTaskId")
            'TMTask is used for task manager.
            Session("TMTask") = ReplTask
            Return True
        Else
            DO1.Value("ASSIGNED") = "Not Assigned"
            Return False
        End If
    End Function

    Private Sub doMenu()
        If Not Session("REPLTSKTaskId") Is Nothing Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForTask(CType(Session("REPLTSKTaskId"), ReplenishmentTask).TASK)
            Try
                tm.ExitTask()
            Catch ex As Exception
            End Try
        End If
        Session.Remove("REPLTSKTaskId")
        Session.Remove("REPLTSKTDetail")
        ManageMutliUOMUnits.Clear(True)

        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        If Session("REPLTSKTaskId") Is Nothing Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Try
                Dim ts As New WMS.Logic.TaskManager
                ts.RequestTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.REPLENISHMENT)
                'Commented for RWMS-547
                'CheckAssigned()
                'End Commented for RWMS-547
                'Added for RWMS-547
                If Not CheckAssigned() Then
                    'Added for RWMS-2077 and RWMS-1717 commented start
                    'RWMS-1467 and RWMS-829 Added Start
                    'Dim div As New System.Web.UI.HtmlControls.HtmlGenericControl("DIV")
                    'div.ID = "divErrorMessage"
                    'div.InnerHtml = "<script language=JavaScript>window.location='Main.aspx'</script>"
                    'Form.Controls.Add(div)
                    'RWMS-1467 and RWMS-829 Added End
                    'Added for RWMS-2077 and RWMS-1717 commented end
                    'RWMS-829 Commented Start
                    'Response.Redirect(MapVirtualPath("screens/Main.aspx"))
                    'RWMS-829 Commented End

                End If
                'End Added for RWMS-547
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                'Added for RWMS-547
            Catch e As System.Threading.ThreadAbortException
                'End Added for RWMS-547
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
            End Try
        End If
        If Not Session("REPLTSKTaskId") Is Nothing Then
            If MobileUtils.ShowGoalTimeOnTaskAssignment() AndAlso Not Session("ShowedTaskManager") Then
                Session("TargetScreen") = "screens/REPL1.aspx"
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/REPL1.aspx"))
            End If
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("ASSIGNED")
        DO1.AddLabelLine("TASKID")
        DO1.AddSpacer()
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
End Class