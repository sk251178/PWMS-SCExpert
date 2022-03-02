Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class LOCBLKCNTTASK
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    'Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        If WMS.Logic.GetCurrentUser Is Nothing Or WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove("LocationBulkCountTask")
            Session.Remove("TSKTaskId")
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session.Remove("CountingSrcScreen")
            If CheckAssigned() Then
                If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                    If Session("ShowedTaskManager") Is Nothing Then
                        Session("ShowedTaskManager") = 1
                        Session("TargetScreen") = "screens/locblkcnttask.aspx"
                        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                    End If
                End If
            End If
            'doNext()
        End If
    End Sub

    Private Sub doMenu()
        Try
            'Dim cnt As New WMS.Logic.Counting
            'cnt.ExitCount(WMS.Logic.GetCurrentUser)
            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.LOCATIONBULKCOUNTING, LogHandler.GetRDTLogger())
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
        Dim tm As New WMS.Logic.TaskManager
        Dim oCntTask As WMS.Logic.CountTask
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then

            oCntTask = tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING, WMS.Logic.LogHandler.GetRDTLogger())
            Session.Add("LocationBulkCountTask", oCntTask)
            Session.Add("TSKTaskId", oCntTask.TASK)
            Session.Add("TaskLocationCNTLocationId", oCntTask.FROMLOCATION)
            Session.Add("TaskWarehouseareaCNTWarehouseareaId", oCntTask.FROMWAREHOUSEAREA)
            Session("TMTask") = oCntTask
            DO1.Value("ASSIGNED") = "Assigned"
            DO1.Value("TASKID") = Session("TSKTaskId")
            Return True
        Else
            oCntTask = tm.RequestTask(UserId, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING)
            If Not oCntTask Is Nothing Then
                Session.Add("LocationCountTask", oCntTask)
                Session.Add("TSKTaskId", oCntTask.TASK)
                Session.Add("TaskLocationCNTLocationId", oCntTask.FROMLOCATION)
                Session.Add("TaskWarehouseareaCNTWarehouseareaId", oCntTask.FROMWAREHOUSEAREA)
                DO1.Value("ASSIGNED") = "Assigned"
                DO1.Value("TASKID") = Session("TSKTaskId")
                Session("TMTask") = oCntTask
                Return True
            Else
                DO1.Value("ASSIGNED") = "Not Assigned"
                DO1.Value("TASKID") = ""
                Return False
            End If
        End If
    End Function

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Session.Remove("ShowedTaskManager")
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                tm.RequestTask(UserId, WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING)
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
            Session("CountingSrcScreen") = "LOCBLKCNTTASK"
            Session("TaskLocationCNTLoadsDT") = CreateLoadsDatatable()
            Response.Redirect(MapVirtualPath("Screens/LOCBLKCNTTASK1.aspx"))
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

    Private Function CreateLoadsDatatable() As DataTable
        Dim dt As New DataTable
        Dim SQL As String = String.Format("select loadid, consignee, sku, units as fromqty, 0 as toqty, 0 as counted from invload where location = '{0}' and warehousearea = '{1}'", Session("TaskLocationCNTLocationId"), Session("TaskWarehouseareaCNTWarehouseareaId"))
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

End Class