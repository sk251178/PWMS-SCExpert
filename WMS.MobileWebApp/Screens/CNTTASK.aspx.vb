Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports WMS.Logic

<CLSCompliant(False)> Public Class CNTTASK
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
            Session.Remove("TSKTaskId")

            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.LOADCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Try
                    Dim tm As New WMS.Logic.TaskManager
                    tm.RequestTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.LOADCOUNTING)
                    If CheckAssigned() Then
                        If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                            If Session("ShowedTaskManager") Is Nothing Then
                                Session("ShowedTaskManager") = True
                                Session("TargetScreen") = "screens/cnttask.aspx"
                                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                            End If
                        End If
                    End If
                Catch ex As Exception
                End Try
            Else
                If CheckAssigned() Then
                    If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                        If Session("ShowedTaskManager") Is Nothing Then
                            Session("ShowedTaskManager") = True
                            Session("TargetScreen") = "screens/cnttask.aspx"
                            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                        Else
                            Response.Redirect(MapVirtualPath("screens/CNTTASK1.aspx"))
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub doMenu()
        Try
            'Dim cnt As New WMS.Logic.Counting
            'cnt.ExitCount(WMS.Logic.GetCurrentUser)
            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            If WLTaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOADCOUNTING, LogHandler.GetRDTLogger()) Then
                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.LOADCOUNTING, LogHandler.GetRDTLogger())
                tm.ExitTask()
            End If
            Made4Net.Mobile.Common.GoToMenu()
        Catch ex As Exception
        End Try
        Session.Remove("TSKTaskId")
        Session.Remove("TaskLoadCNTLoadId")
        Session.Remove("ShowedTaskManager")
        Session.Remove("TMTask")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Function CheckAssigned() As Boolean
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID) ', Made4Net.Schema.CONNECTION_NAME)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOADCOUNTING, rdtLogger) Then
            Dim oCntTask As WMS.Logic.CountTask = tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.LOADCOUNTING, rdtLogger)
            Session.Add("LoadCountTask", oCntTask)
            Session.Add("TSKTaskId", oCntTask.TASK)
            Session.Add("TaskLoadCNTLoadId", oCntTask.FROMLOAD)
            DO1.Value("ASSIGNED") = "Assigned"
            DO1.Value("TASKID") = Session("TSKTaskId")
            Session("TMTask") = oCntTask
            Return True
        Else
            DO1.Value("ASSIGNED") = "Not Assigned"
            DO1.Value("TASKID") = ""
            Return False
        End If
    End Function

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Session.Remove("ShowedTaskManager")
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOADCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                tm.RequestTask(UserId, WMS.Lib.TASKTYPE.LOADCOUNTING)
                CheckAssigned()
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            End Try
        Else
            CheckAssigned()
        End If
        If Session("TSKTaskId") = "" Or Session("TSKTaskId") = Nothing Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No jobs assigned"))
        Else
            Response.Redirect(MapVirtualPath("Screens/CNTTASK1.aspx"))
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