Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PARPCK1
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
            Session.Remove("PARPCKPicklist")
            Session.Remove("PARPCKPicklistPickJob")
            Session("MobileSourceScreen") = "PARPCK1"
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then

                Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
                If Not Session("PCKPicklist") Is Nothing Then
                    Dim oPicklst As WMS.Logic.Picklist = Session("PCKPicklist")
                    Dim oPicklist As New Picklist(oPicklst.PicklistID)
                    If Not oPicklist Is Nothing Then
                        Session("PCKPicklist") = oPicklist
                    End If
                    If oPicklist.isCompleted Then
                        If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                            Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                            MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                            If Not Session("PCKPicklist") Is Nothing Then
                                If oPicklist.ShouldPrintShipLabel Then
                                    oPicklist.PrintShipLabels(prntr.PrinterQName)
                                End If
                                PickTask.UpdateCompletionTime(oPicklist)
                            ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                                pck2.PrintShipLabels(prntr.PrinterQName)
                            End If
                            If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                                Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                            End If
                            Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                        Else
                            Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                        End If
                    End If

                End If

            End If

            If Not CheckAssigned() Then
                NextClicked()
            End If
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                If Session("ShowedTaskManager") Is Nothing Then
                    Session("ShowedTaskManager") = True
                    Dim task As WMS.Logic.Task = New WMS.Logic.Task(WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()))
                    Session("TMTask") = task

                    If MobileUtils.ShouldRedirectToTaskSummary() Then
                        Session("TaskID") = task.TASK
                    End If

                    Session("TargetScreen") = "screens/parpck1.aspx"
                    Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                End If
            End If
        End If
    End Sub

    Private Sub MenuClick()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                Dim TMTask As WMS.Logic.Task = tm.RequestTask(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING)
                CheckAssigned()
                If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                    Session("ShowedTaskManager") = True
                    Session("TMTask") = TMTask
                    Session("TargetScreen") = "screens/parpck1.aspx"
                    Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                End If
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session("TaskID") = TMTask.TASK
                End If
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage())
            Catch ex As Threading.ThreadAbortException
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            End Try
        Else
            Session.Remove("ShowedTaskManager")
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, LogHandler.GetRDTLogger())
            Dim pcklsts As New ParallelPicking(tm.Task.ParallelPicklist)
            Session("PARPCKPicklist") = pcklsts
            If pcklsts.ShouldPrintPickLabelOnPicking Then
                Response.Redirect(MapVirtualPath("Screens/PARPCKLBLPRNT.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/PARPCK2.aspx"))
            End If
        End If
    End Sub

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        If Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim pcklsts As ParallelPicking
            Try
                pcklsts = New ParallelPicking(tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()).ParallelPicklist)
            Catch ex As Exception

            End Try
            If Not pcklsts Is Nothing Then
                Session("PARPCKPicklist") = pcklsts
                setAssigned(pcklsts)
                Return True
            Else
                setNotAssigned()
                Return False
            End If
        Else
            setNotAssigned()
            Return False
        End If
    End Function

    Protected Sub setNotAssigned()
        DO1.Value("Assigned") = "Not Assigned"
        DO1.LeftButtonText = "next"

        DO1.setVisibility("Picklist", False)
        'DO1.setVisibility("PickMethod", False)
        DO1.setVisibility("NumPicklists", False)
        DO1.setVisibility("NumPicks", False)
        DO1.setVisibility("InnerHandlingUnitType", False)
        DO1.setVisibility("ContainerId", False)
        DO1.setVisibility("ContainerType", False)
    End Sub

    Protected Sub setAssigned(ByVal pcklsts As ParallelPicking)
        DO1.Value("Assigned") = "Assigned"

        DO1.setVisibility("Picklist", True)
        'DO1.setVisibility("PickMethod", True)
        DO1.setVisibility("NumPicklists", True)
        DO1.setVisibility("NumPicks", True)
        DO1.setVisibility("InnerHandlingUnitType", True)

        If Not pcklsts.ToContainer Is Nothing And Not pcklsts.ToContainer = String.Empty Then
            DO1.setVisibility("ContainerId", True)
            DO1.setVisibility("ContainerType", True)
            DO1.Value("ContainerType") = pcklsts.HandlingUnitType
            DO1.Value("ContainerId") = pcklsts.ToContainer
        Else
            DO1.setVisibility("ContainerId", False)
            DO1.setVisibility("ContainerType", False)
        End If

        DO1.Value("Assigned") = "Assigned"
        DO1.Value("Picklist") = pcklsts.ParallelPickId
        'DO1.Value("PickMethod") = WMS.Lib.PickMethods.PickMethod.PARALELORDERPICKING
        DO1.Value("NumPicklists") = pcklsts.PickLists.Count
        DO1.Value("NumPicks") = pcklsts.NumPicks
        DO1.Value("InnerHandlingUnitType") = pcklsts.PickListsHandlingUnitType
        DO1.LeftButtonText = "Next"
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Assigned")
        DO1.AddLabelLine("Picklist")
        DO1.AddSpacer()
        'DO1.AddLabelLine("PickMethod")
        DO1.AddLabelLine("NumPicklists")
        DO1.AddLabelLine("NumPicks")
        DO1.AddLabelLine("InnerHandlingUnitType")
        DO1.AddLabelLine("ContainerType")
        DO1.AddLabelLine("ContainerID")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                NextClicked()
            Case "requestpick"
                NextClicked()
            Case "menu"
                MenuClick()
        End Select
    End Sub

End Class