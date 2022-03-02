Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports System.Text.RegularExpressions
Imports WMS.Lib
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class PCKBagOut
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
        Dim trans As New Made4Net.Shared.Translation.Translator(Translation.Translator.CurrentLanguageID)
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            MyBase.WriteToRDTLog(" Page Load Event of PCKBagOut.aspx.vb -- Removing all the sessions -- ")
            Session.Remove("PCKDeliveryJob")
            Session.Remove("PCKPicklistPickJob")
            Session.Remove("PCKPicklistActiveContainerIDSecond")
            Session.Remove("PCKOldUomUnits")
            Session.Remove("UOMUnits_2")
            Session.Remove("WeightNeededPickJob")
            Session.Remove("WeightNeededConfirm1")
            Session.Remove("WeightNeededConfirm2")
            Session.Remove("TMTask")
            Session("MobileSourceScreen") = "PCKBagOut"
            Dim EnableBagOutPicking As String = Made4Net.Shared.Util.GetSystemParameterValue("BAGOUTPICKING").ToString()
            If EnableBagOutPicking = "1" Then
                Session("PCKBagOutPicking") = 1
                WMS.Logic.Picking.BagOutProcess = True
            Else
                Session("PCKBagOutPicking") = 0
                WMS.Logic.Picking.BagOutProcess = False
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Bag Out Picking is not enabled"))
                Return
            End If
            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
            Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                If Not Session("PCKPicklist") Is Nothing Then
                    'Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                    If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                        Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    End If
                End If
            End If
            Dim PicklistID As String
            If Not Session("PickListID") Is Nothing Then
                PicklistID = Session("PickListID")
            End If
            If Session("ReversePickon") = 1 Then
                Session.Remove("ReversePickon")
                NextClicked()
            End If
            If Session("COMMENTSSHOWN") = 1 Then
                Session.Remove("COMMENTSSHOWN")
                NextClicked()
            End If
            If Not Session("PCKListToResume") Is Nothing Then
                PicklistID = Session("PCKListToResume")
            End If


            If Not PicklistID Is Nothing Then
                Dim tm As New WMS.Logic.TaskManager
                Dim pcklst As Picklist
                pcklst = New Picklist(PicklistID)
                If Picklist.IsPickTaskAssigendToOtherUser(PicklistID, WMS.Logic.Common.GetCurrentUser) = False AndAlso Not pcklst.isCompleted Then
                    tm.RequestPartialPickTaskByPicklistId(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, PicklistID, Nothing)
                    Session("TMTask") = tm.Task
                    Session("PCKPicklist") = pcklst
                    If pcklst.IsFirstPick = False Then
                        NextClicked()
                    End If
                    If Session("TMTask") Is Nothing Then
                        DO1.Button("Reverse").Visible = False
                        DO1.Button("Next").Visible = False
                    End If
                End If

            End If
            If Not CheckAssigned() Then
                    NextClicked()
                End If
                If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                    If Session("ShowedTaskManager") Is Nothing Then
                        Session("ShowedTaskManager") = True
                        Try
                            Dim userAssignedTask As String = WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Logic.LogHandler.GetRDTLogger())
                            If Not userAssignedTask Is Nothing Then
                                Dim task As WMS.Logic.Task = New WMS.Logic.Task(userAssignedTask)
                                Session("TMTask") = task
                                If MobileUtils.ShouldRedirectToTaskSummary() AndAlso Not Session("ShowedTaskManager") Then
                                    Session("TaskID") = task.TASK
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                        Session("TargetScreen") = "screens/PCKBagOut.aspx"
                        Response.Redirect(MapVirtualPath("screens/TaskManager.aspx"))
                    End If

                End If
            End If
    End Sub

    Private Sub MenuClick()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked(Optional ByVal onload As Boolean = False, Optional ByVal bUserSelection As Boolean = False)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If (Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Logic.LogHandler.GetRDTLogger())) And (onload = False) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                If Session("TMTask") Is Nothing Then
                    Dim TMTask As WMS.Logic.Task
                    If (String.IsNullOrEmpty(Session("PickListID"))) Then
                        TMTask = tm.RequestTask(UserId, WMS.Lib.TASKTYPE.PICKING)
                        Session("TMTask") = tm.Task
                    Else
                        TMTask = tm.RequestPartialPickTaskByPicklistId(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, Session("PickListID"), Nothing)
                        Session("TMTask") = tm.Task
                    End If
                Else
                    Dim oTask As WMS.Logic.Task
                    oTask = Session("TMTask")
                    Dim oPickList As Picklist = New Picklist(oTask.Picklist)
                    Session("PCKPicklist") = oPickList
                End If
                If CheckAssigned() Then
                    If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                        If Session("ShowedTaskManager") Is Nothing Then
                            Session("ShowedTaskManager") = True
                            Session("TargetScreen") = "screens/PCKBagOut.aspx"
                            Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                        End If
                        'RWMS -2968
                        'Else
                        '    Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                    End If
                End If
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Threading.ThreadAbortException
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            End Try
        Else
            Session.Remove("ShowedTaskManager")
            Dim tm As WLTaskManager
            Dim pcklst As Picklist
            If Session("TMTask") Is Nothing Then
                tm = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, LogHandler.GetRDTLogger())
                Session("TMTask") = tm.Task
                If tm.Task IsNot Nothing Then
                    pcklst = New Picklist(tm.Task.Picklist)
                    Session("PCKPicklist") = pcklst
                Else
                    MyBase.WriteToRDTLog("No pick task available for the user {0}", UserId)
                End If
            Else
                pcklst = Session("PCKPicklist")
            End If
            Dim contid As String
            contid = Session("PCKPicklistActiveContainerID")
            If Session("PCKPicklistActiveContainerID") Is Nothing Then
                If Not SetContainerID() Then Return
            End If
            Dim strPickingComments As String
            If bUserSelection = True Then
                strPickingComments = MobileUtils.getPCKComments(pcklst.PicklistID)
                If strPickingComments.Length > 0 And pcklst.IsFirstPick = True And Session("COMMENTSSHOWN") = 0 Then
                    Session("PICKINGCOMMENTS") = strPickingComments
                    Response.Redirect(MapVirtualPath("Screens/PCKCOMMENTS.aspx"))
                End If
            End If

            If Not pcklst Is Nothing Then
                If (pcklst.ShouldPrintPickLabelOnPicking And pcklst.GetTotalPickedQty = 0) Then
                    If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                        Dim prntr As LabelPrinter
                        prntr = New LabelPrinter(Session("DefaultPrinter"))
                        MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        MobileUtils.UpdateDPrinterInUserProfile(prntr.PrinterQName, LogHandler.GetRDTLogger())
                        pcklst.PrintPickLabels(prntr.PrinterQName)
                        If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                            Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
                        End If
                    Else
                        Response.Redirect(MapVirtualPath("Screens/PCKLBLPRNT.aspx"))
                    End If
                End If
                If (pcklst.ShouldPrintBagOutReportOnStart And pcklst.GetTotalPickedQty = 0) Then
                    Response.Redirect(MapVirtualPath("Screens/PCKBAGOUTPRINT.aspx"))
                Else
                    If pcklst.getReleaseStrategy().ConfirmationType = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATIONUOM Or pcklst.getReleaseStrategy().ConfirmationType = WMS.Lib.Release.CONFIRMATIONTYPE.SKUUOM Then
                        Response.Redirect(MapVirtualPath("Screens/PCKPARTUOM.aspx"))
                    Else
                        Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
                    End If
                End If
            End If
        End If

    End Sub
	'RWMS-3734
    Private Sub ReverseClicked()
        Response.Redirect(MapVirtualPath("Screens/VRFIReverse.aspx"))
    End Sub
    'RWMS-3734

    Private Function SetContainerID() As Boolean
        Dim pcklist As Picklist = Session("PCKPicklist")
        If Not pcklist Is Nothing Then
            If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                Dim contid As String = DO1.Value("ContainerId")
                Dim errMsg As String
                MyBase.WriteToRDTLog("Before setting the new Container Id for the picklist. ")
                If Not MobileUtils.CheckContainerID(pcklist.PicklistID, contid, errMsg) Then
                    DO1.Value("ContainerId") = ""
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errMsg)
                    Return False
                Else
                    DO1.Value("ContainerId") = contid
                End If
                Session("PCKPicklistActiveContainerID") = contid
                MyBase.WriteToRDTLog("Session(PCKPicklistActiveContainerID) has set for the new containerid " & contid)
            End If
        End If
        Return True
    End Function

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If IsTaskAssigned(UserId) Then
            Dim pcklst As Picklist
            Try
                MyBase.WriteToRDTLog(" --If Task has been assigned and create the picklist object for the assinged Task --- CheckAssigned() --- ")
                Dim oTask As WMS.Logic.Task
                Dim PicklistID As String
                If Not Session("PickListID") Is Nothing Then
                    PicklistID = Session("PickListID")
                    Session("PickListID") = Nothing
                End If
                If Not Session("PCKListToResume") Is Nothing Then
                    PicklistID = Session("PCKListToResume")
                    Session("PCKListToResume") = Nothing
                End If
                If Not PicklistID Is Nothing Then
                    Dim tm As New WMS.Logic.TaskManager
                    tm.RequestPartialPickTaskByPicklistId(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, PicklistID, Nothing)
                    oTask = tm.Task
                ElseIf Not Session("TMTask") Is Nothing Then
                    oTask = Session("TMTask")
                Else
                    oTask = GetAssignerTask(UserId)
                End If
                Session("TMTask") = oTask
                If oTask IsNot Nothing Then
                    pcklst = New Picklist(oTask.Picklist)
                    Session("PCKPicklist") = pcklst
                End If
                MyBase.WriteToRDTLog("After getting the Picklist object and created the Container id and stored in containercollection class: " & pcklst.PicklistID & "Container Id: " & pcklst.ActiveContainer())
                MyBase.WriteToRDTLog("Once the picklist object created and if any available containers are there for the picklist then it will fetch in the container collections. ")
            Catch ex As Exception
                MyBase.WriteToRDTLog("Catch Exception " & ex.Message.ToString())
            End Try
            If Not pcklst Is Nothing Then
                Session("PCKPicklist") = pcklst
                setAssigned(pcklst)
                MyBase.WriteToRDTLog("If Picklist is Not nothing Then call SetAssinged Method and check for the activecontainer and assing to session")
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

    ''' <summary>
    ''' RWMS-2362
    ''' </summary>
    ''' <param name="pUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsTaskAssigned(ByVal pUser As String) As Boolean
        Dim oTask As WMS.Logic.Task = Session("TMTask")
        If Not oTask Is Nothing Then
            If oTask.ASSIGNED = True And oTask.USERID = pUser.Trim() And (oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING) Then
                Return True
            Else
                Return Logic.TaskManager.isAssigned(pUser, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Logic.LogHandler.GetRDTLogger())
            End If
        Else
            Return Logic.TaskManager.isAssigned(pUser, WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Logic.LogHandler.GetRDTLogger())
        End If
    End Function

    ''' <summary>
    ''' RWMS-2362
    ''' </summary>
    ''' <param name="UserId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAssignerTask(UserId As String) As Task
        Dim tm As New WMS.Logic.TaskManager

        Dim oTask As WMS.Logic.Task = Session("TMTask")
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not oTask Is Nothing Then
            If oTask.ASSIGNED = True And oTask.USERID = UserId.Trim() And (oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING) Then
                Return oTask
            Else
                Return tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, rdtLogger)
            End If
        Else
            Return tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, rdtLogger)
        End If
    End Function
    Protected Sub setNotAssigned()
        DO1.Value("Assigned") = "Not Assigned"
        DO1.LeftButtonText = "next"

        DO1.setVisibility("Picklist", False)
        DO1.setVisibility("PickType", False)
        DO1.setVisibility("PickMethod", False)
        DO1.setVisibility("ContainerId", False)
        DO1.setVisibility("ContainerType", False)
        DO1.setVisibility("ContainerTypeDesc", False)
        DO1.setVisibility("Reverse", False)
        DO1.setVisibility("Next", False)
        DO1.Button("Reverse").Visible = False
        DO1.Button("Next").Visible = False
    End Sub
    Protected Sub setAssigned(ByVal pcklst As Picklist)
        Dim contid As String
        DO1.Value("Assigned") = "Assigned"
        DO1.setVisibility("Picklist", True)
        DO1.setVisibility("PickType", True)
        DO1.setVisibility("PickMethod", True)

        DO1.setVisibility("ContainerId", True)
        DO1.setVisibility("ContainerType", False)
        DO1.setVisibility("ContainerTypeDesc", True)
        DO1.Value("ContainerType") = pcklst.HandelingUnitType
        contid = pcklst.ActiveContainer()
        MyBase.WriteToRDTLog(" Active Container Method in PCKBagOut.aspx. Active Container Id: " & contid)
        If contid = "" Then
            DO1.Value("ContainerId") = ""
        Else
            DO1.Value("ContainerId") = contid
            Session("PCKPicklistActiveContainerID") = contid
            MyBase.WriteToRDTLog(" Active Container Method in PCKBagOut.aspx. If Container is already present . Active Container Id: " & contid)
        End If
        Dim sqltype As String = " select containerdesc from handelingunittype " &
                        " WHERE container = '" & pcklst.HandelingUnitType & "'"
        DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)

        DO1.Value("Pick Policy description") = MobileUtils.PLANSTRATEGYDESCRIPTION(pcklst.StrategyId)

        Dim Sql As String = " select count(PICKLISTLINE) from PICKDETAIL " &
                         " WHERE PICKLIST = '" & pcklst.PicklistID & "'"
        DO1.Value("Total Lines") = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

        Dim OpenCases As String

        Sql = String.Format(" select sum(OpenCases) from vPicklistOpenCases WHERE PICKLIST  = '{0}' ", pcklst.PicklistID)

        Try
            OpenCases = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql).ToString.Replace(".0000", "")
        Catch ex As Exception
            OpenCases = "0"
        End Try

        DO1.Value("Total Units") = OpenCases

        DO1.Value("Assigned") = "Assigned"
        DO1.Value("Picklist") = pcklst.PicklistID
        DO1.Value("PickMethod") = pcklst.PickMethod
        DO1.Value("PickType") = pcklst.PickType
        DO1.LeftButtonText = "Next"
        DO1.Button("Next").Visible = True
        If Len(pcklst.PICKORDERSTATUS) > 0 Then
            If (pcklst.PICKORDERSTATUS = "REVERSE") Then
                DO1.Button("Reverse").Visible = False
            ElseIf (pcklst.PICKORDERSTATUS <> "REVERSE") And pcklst.IsFirstPick = True And (DO1.Value("Assigned") = "Assigned") Then
                DO1.Button("Reverse").Visible = True
            Else
                DO1.Button("Reverse").Visible = False
            End If
        Else
            If pcklst.IsFirstPick = False Then
                DO1.Button("Reverse").Visible = False
            Else
                If (DO1.Value("Assigned") = "Assigned") Then
                    DO1.Button("Reverse").Visible = True
                    DO1.Button("Next").Visible = True
                Else
                    DO1.Button("Reverse").Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Assigned")
        DO1.AddLabelLine("Picklist")
        DO1.AddSpacer()
        DO1.AddLabelLine("PickType")
        DO1.AddLabelLine("PickMethod")
        DO1.AddLabelLine("Pick Policy description")
        DO1.AddLabelLine("Total Units")
        DO1.AddLabelLine("Total Lines")
        DO1.AddLabelLine("ContainerType")
        DO1.AddLabelLine("ContainerTypeDesc")
        DO1.AddTextboxLine("ContainerID")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Try
            Select Case e.CommandText.ToLower
                Case "next"
                    NextClicked(True, True)
                Case "requestpick"
                    NextClicked(True)
                Case "reverse"
                    ReverseClicked()
                Case "menu"
                    MenuClick()
            End Select
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

End Class