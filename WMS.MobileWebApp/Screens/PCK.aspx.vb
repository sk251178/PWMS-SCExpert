Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports System.Text.RegularExpressions

<CLSCompliant(False)> Public Class PCK
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
            MyBase.WriteToRDTLog(" Page Load Event of PCK.aspx.vb -- Removing all the sessions -- ")
            Session.Remove("PCKPicklist")
            Session.Remove("PCKDeliveryJob")
            Session.Remove("PCKPicklistPickJob")
            Session.Remove("PCKPicklistActiveContainerID")
            Session.Remove("PCKPicklistActiveContainerIDSecond")
            Session.Remove("PCKOldUomUnits")
            Session.Remove("UOMUnits_2")
            Session.Remove("WeightNeededPickJob")
            Session.Remove("WeightNeededConfirm1")
            Session.Remove("WeightNeededConfirm2")
            Session.Remove("PICKINGCOMMENTS")
            Session.Remove("TMTask")
            Session("MobileSourceScreen") = "PCK"

            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) AndAlso Session("PCKListToResume") Is Nothing Then

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
            If Session("ReversePickon") = 1 Then
                Session.Remove("ReversePickon")
                NextClicked(True)
            End If
            If Session("COMMENTSSHOWN") = 1 Then
                Session.Remove("COMMENTSSHOWN")
                NextClicked()
            End If
            Dim PicklistID As String
            If Not Session("PickListID") Is Nothing Then
                PicklistID = Session("PickListID")
            End If
            If Not Session("PCKListToResume") Is Nothing Then
                PicklistID = Session("PCKListToResume")
            End If
            If Not PicklistID Is Nothing Then
                Dim tm As New WMS.Logic.TaskManager
                Dim pcklst As Picklist
                pcklst = New Picklist(PicklistID)
                If pcklst.IsPickTaskAssigendToOtherUser(PicklistID, WMS.Logic.Common.GetCurrentUser) = False Then
                    tm.RequestPartialPickTaskByPicklistId(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, PicklistID, Nothing)
                    Session("TMTask") = tm.Task
                    Session("PCKPicklist") = pcklst
                    If pcklst.IsFirstPick = False Then
                        NextClicked()
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
                        Dim userAssignedTask As String = WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger())
                        If Not userAssignedTask Is Nothing Then
                            Dim task As WMS.Logic.Task = New WMS.Logic.Task(userAssignedTask)
                            Session("TMTask") = task
                            If MobileUtils.ShouldRedirectToTaskSummary() AndAlso Not Session("ShowedTaskManager") Then
                                Session("TaskID") = task.TASK
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                    Session("TargetScreen") = "screens/pck.aspx"
                    Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                End If
            End If
        End If
    End Sub

    Private Sub MenuClick()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked(Optional ByVal bUserSelected As Boolean = False)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                Dim TMTask As WMS.Logic.Task = tm.RequestTask(UserId, WMS.Lib.TASKTYPE.PICKING)
                Session("TMTask") = TMTask
                CheckAssigned()
                If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                    Session("ShowedTaskManager") = True
                    Session("TMTask") = TMTask
                    Session("TargetScreen") = "screens/pck.aspx"
                    Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                    'Else
                    '    Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                End If
                If MobileUtils.ShouldRedirectToTaskSummary() And Not IsNothing(TMTask) Then
                    Session("TaskID") = TMTask.TASK
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
            If Not Session("PCKListToResume") Is Nothing Then
                tm = WLTaskManager.NewTaskManagerForPicklist(Session("PCKListToResume"))
                Session("PCKListToResume") = Nothing
            Else
                tm = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            End If
            Dim pcklst As Picklist
            If tm.Task IsNot Nothing Then
                pcklst = New Picklist(tm.Task.Picklist)
                Session("PCKPicklist") = pcklst
            Else
                MyBase.WriteToRDTLog("No pick task available for the user {0}", UserId)
            End If
            Dim contid As String
            If DO1.Value("ContainerId") <> "" Then
                Dim rgx As Regex = New Regex("^[a-zA-Z0-9]+$")
                Dim strContainer As String = DO1.Value("ContainerId")
                If Not rgx.IsMatch(strContainer) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container ID can not have blank space and cannot be special character. It can be empty or can be alphanumeric."))
                    Return
                End If
                If strContainer.Trim.Length > 20 Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container must be up to 20 characters"))
                    Return
                End If
            End If
            contid = Session("PCKPicklistActiveContainerID")
            If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
                If DO1.Value("ContainerId") = "" Or DO1.Value("ContainerId") <> contid Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKCLOSECONTAINER.aspx?sourcescreen=PCK"))
                Else
                    If Not SetContainerID() Then Return
                End If
            Else
                If Not SetContainerID() Then Return
            End If

            Dim strPickingComments As String
            If bUserSelected = True Then
                strPickingComments = MobileUtils.getPCKComments(pcklst.PicklistID)
                If strPickingComments.Length > 0 And pcklst.IsFirstPick = True Then
                    Session("PICKINGCOMMENTS") = strPickingComments
                    Response.Redirect(MapVirtualPath("Screens/PCKCOMMENTS.aspx"))
                End If
            End If
            If pcklst.ShouldPrintPickLabelOnPicking Then
                If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                    Dim prntr As LabelPrinter
                    prntr = New LabelPrinter(Session("DefaultPrinter"))
                    MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                    pcklst.PrintPickLabels(prntr.PrinterQName)
                    If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                        Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
                    Else
                        Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
                    End If
                Else
                    Response.Redirect(MapVirtualPath("Screens/PCKLBLPRNT.aspx"))
                End If
            Else
                If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
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
                Dim oTask As WMS.Logic.Task = GetAssignerTask(UserId)
                Session("TMTask") = oTask
                pcklst = New Picklist(oTask.Picklist)
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
    ''' </summary>
    ''' <param name="pUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsTaskAssigned(ByVal pUser As String) As Boolean
        Dim oTask As WMS.Logic.Task = Session("TMTask")
        If Not oTask Is Nothing Then
            If oTask.ASSIGNED = True And oTask.USERID = pUser.Trim() And (oTask.TASKTYPE = WMS.Lib.TASKTYPE.FULLPICKING Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING) Then
                Return True
            Else
                Return Logic.TaskManager.isAssigned(pUser, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger())
            End If
        Else
            Return Logic.TaskManager.isAssigned(pUser, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger())
        End If
    End Function

    ''' <summary>
    ''' RWMS-2362
    ''' </summary>
    ''' <param name="UserId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAssignerTask(ByVal UserId As String) As Task
        Dim tm As New WMS.Logic.TaskManager
        Dim oTask As WMS.Logic.Task = Session("TMTask")
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not oTask Is Nothing Then
            If oTask.ASSIGNED = True And oTask.USERID = UserId.Trim() And (oTask.TASKTYPE = WMS.Lib.TASKTYPE.FULLPICKING Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING) Then
                Return oTask
            Else
                Return tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PICKING, rdtLogger)
            End If
        Else
            Return tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PICKING, rdtLogger)
        End If
    End Function

    Protected Sub setNotAssigned()
        DO1.Value("Assigned") = "Not Assigned"
        DO1.LeftButtonText = "next"
        DO1.CenterLeftButtonText = "reverse"

        DO1.setVisibility("Picklist", False)
        DO1.setVisibility("PickType", False)
        DO1.setVisibility("PickMethod", False)
        DO1.setVisibility("ContainerId", False)
        DO1.setVisibility("ContainerType", False)
        DO1.setVisibility("ContainerTypeDesc", False)
        DO1.setVisibility("Pick Cube", False)
    End Sub

    Protected Sub setAssigned(ByVal pcklst As Picklist)
        Dim contid As String
        DO1.Value("Assigned") = "Assigned"
        DO1.setVisibility("Picklist", True)
        DO1.setVisibility("PickType", True)
        DO1.setVisibility("PickMethod", True)
        DO1.setVisibility("Pick Cube", False)
        If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            DO1.setVisibility("ContainerId", False)
            DO1.setVisibility("ContainerType", False)
            DO1.setVisibility("ContainerTypeDesc", False)
        Else
            DO1.setVisibility("ContainerId", True)
            DO1.setVisibility("ContainerType", False)
            DO1.setVisibility("ContainerTypeDesc", True)
            DO1.Value("ContainerType") = pcklst.HandelingUnitType
            contid = pcklst.ActiveContainer()
            MyBase.WriteToRDTLog(" Active Container Method in PCK.aspx. Active Container Id: " & contid)
            If contid = "" Then
                DO1.Value("ContainerId") = ""
            Else
                DO1.Value("ContainerId") = contid
                Session("PCKPicklistActiveContainerID") = contid
                MyBase.WriteToRDTLog(" Active Container Method in PCK.aspx. If Container is already present . Active Container Id: " & contid)
            End If
            Dim sqltype As String = " select containerdesc from handelingunittype " &
                          " WHERE container = '" & pcklst.HandelingUnitType & "'"
            DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)
        End If

        DO1.Value("Pick Policy description") = MobileUtils.PLANSTRATEGYDESCRIPTION(pcklst.StrategyId)

        Dim Sql As String = " select count(PICKLISTLINE) from PICKDETAIL " &
                         " WHERE PICKLIST = '" & pcklst.PicklistID & "'"
        DO1.Value("Total Lines") = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

        If pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
            DO1.setVisibility("Pick Cube", True)
            DO1.Value("Pick Cube") = pcklst.GetPickCube()
        End If

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
        DO1.CenterLeftButtonText = "Reverse"

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
        DO1.AddLabelLine("Pick Cube")
        DO1.AddLabelLine("ContainerType")
        DO1.AddLabelLine("ContainerTypeDesc")
        DO1.AddTextboxLine("ContainerID")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Try
            Select Case e.CommandText.ToLower
                Case "next"
                    NextClicked(True)
                Case "reverse"
                    ReverseClicked()
                Case "requestpick"
                    NextClicked()
                Case "menu"
                    MenuClick()
            End Select
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub


End Class