Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class LOCCONTTASK
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

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
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session.Remove("CountingSrcScreen")
            If CheckAssigned() Then
                If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                    If Session("ShowedTaskManager") Is Nothing Then
                        Session("ShowedTaskManager") = True
                        Session("TargetScreen") = "screens/LOCCONTTASK.aspx"
                        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                    Else
                        doNext()
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
            If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.LOCATIONCOUNTING, LogHandler.GetRDTLogger())
                tm.ExitTask()
            End If
            Made4Net.Mobile.Common.GoToMenu()
        Catch ex As Exception
        End Try
        Session.Remove("TSKTaskId")
        Session.Remove("TaskLocationCNTLocationId")
        Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
        Session.Remove("ShowedTaskManager")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub


    Private Function CheckAssigned() As Boolean
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID) ', Made4Net.Schema.CONNECTION_NAME)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        Dim tm As New WMS.Logic.TaskManager
        Dim oCntTask As WMS.Logic.CountTask



        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, rdtLogger) Then
            oCntTask = tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, rdtLogger)
            Session.Add("LocationCountTask", oCntTask)
            Session.Add("TSKTaskId", oCntTask.TASK)
            Session.Add("TaskLocationCNTLocationId", oCntTask.FROMLOCATION)
            Session.Add("TaskWarehouseareaCNTWarehouseareaId", oCntTask.FROMWAREHOUSEAREA)
            Session("TMTask") = oCntTask
            DO1.Value("ASSIGNED") = "Assigned"
            DO1.Value("TASKID") = Session("TSKTaskId")
            Return True
        Else
            If Not String.IsNullOrEmpty(Session("COUNTID")) Then
                'Dim strTask As String = isAssignedToTask(oCount.COUNTID)
                'If strTask <> "" Then
                '    Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                '    oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                oCntTask = tm.GetCountingTask(Session("COUNTID").ToString(), UserId)
                Session("COUNTID") = ""
                'End If
            Else
                oCntTask = tm.RequestTask(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING)
            End If


            If Not oCntTask Is Nothing Then
                Session.Add("LocationCountTask", oCntTask)
                Session.Add("TSKTaskId", oCntTask.TASK)
                Session.Add("TaskLocationCNTLocationId", oCntTask.FROMLOCATION)
                Session.Add("TaskWarehouseareaCNTWarehouseareaId", oCntTask.FROMWAREHOUSEAREA)
                Session("TMTask") = oCntTask
                DO1.Value("ASSIGNED") = "Assigned"
                DO1.Value("TASKID") = Session("TSKTaskId")
                Return True
            Else
                DO1.Value("ASSIGNED") = "Not Assigned"
                DO1.Value("TASKID") = ""
                Return False
            End If
        End If
    End Function



    ' Returns TaskID that this load is assigned to
    Private Function isAssignedToTask(ByVal COUNTID As String) As String
        Dim strSql As String = "SELECT top 1 TASK FROM TASKS WHERE COUNTID = '" & COUNTID & "' AND TASKTYPE = 'LOCCNT' order by TASK desc "
        Dim dt As DataTable = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

    Private Sub doNext()
        Try
            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Session.Remove("ShowedTaskManager")
            If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Try
                    Dim tm As New WMS.Logic.TaskManager
                    tm.RequestTask(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING)
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
                Session("CountingSrcScreen") = "LOCCONTTASK"
                '--- added by oded 130313---------------------------------------------------------------
                Dim isDTnull As Boolean
                Dim dt1 As DataTable
                Try
                    Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
                    dt1 = dt.Clone()
                    dt1 = dt.Copy()

                Catch
                    isDTnull = True
                Finally
                    If isDTnull Then
                        Session("TaskLocationCNTLoadsDT") = CreateLoadsDatatable()
                    ElseIf dt1.Rows.Count < 1 Then
                        Session("TaskLocationCNTLoadsDT") = CreateLoadsDatatable()

                    End If
                End Try
                '---------------------------------------------------------------------------------------
                'Session("TaskLocationCNTLoadsDT") = CreateLoadsDatatable()
                Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK1.aspx"))
            End If
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
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

    Private Function CreateLoadsDatatable() As DataTable
        Dim oCntTask As WMS.Logic.CountTask = Session("LocationCountTask")
        Dim dt As New DataTable
        Dim SQL As String = String.Format("select loadid,consignee,sku, units as fromqty, 0 as toqty, 0 as counted from invload where location = '{0}' and Warehousearea = '{1}' ", oCntTask.FROMLOCATION, oCntTask.FROMWAREHOUSEAREA)
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

End Class