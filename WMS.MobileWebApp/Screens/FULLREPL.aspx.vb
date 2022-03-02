Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class FULLREPL
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
        If Not IsPostBack Then
            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
            If Not Request.QueryString.Item("sourcescreen") Is Nothing Then
                Session("REPLSRCSCREEN") = Request.QueryString.Item("sourcescreen")
            Else
                Session("REPLSRCSCREEN") = "FULLREPL"
            End If
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=Repl"))
            End If
            If CheckAssigned() Then
                If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
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
        ReplTask = tm.RequestTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.FULLREPL)
        If Not ReplTask Is Nothing Then
            Session.Add("REPLTSKTaskId", ReplTask)
            ReplTaskDetail = New WMS.Logic.Replenishment(ReplTask.Replenishment)
            Session.Add("REPLTSKTDetail", ReplTaskDetail)
            DO1.Value("ASSIGNED") = "ASSIGNED"
            DO1.Value("TASKID") = ReplTask.TASK 'Session("REPLTSKTaskId")
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
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        If Session("REPLTSKTaskId") Is Nothing Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Try
                Dim ts As New WMS.Logic.TaskManager
                ts.RequestTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.FULLREPL)
                CheckAssigned()
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            End Try
        End If
        If Not Session("REPLTSKTaskId") Is Nothing Then
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
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