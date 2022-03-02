Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class SPECPCK
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
 		Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not IsPostBack Then
            Session.Remove("PCKPicklist")
            Session.Remove("PCKDeliveryJob")
            Session.Remove("PCKPicklistPickJob")
            Session.Remove("PCKPicklistActiveContainerID")
            Session.Remove("PCKBagOutPicking")
            Session("MobileSourceScreen") = "SPECPCK"
            WMS.Logic.Picking.BagOutProcess = False
            Session.Remove("PCKBagOutPicking")
            ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:ActionBar:_ctl1:InnerButton"))

            If Session("ReversePickon") = 1 Then
                Session.Remove("ReversePickon")
                NextClicked(False, True)
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
            If Not Session("PCKListToResume") Is Nothing Then
                Dim pcklst As Picklist = New Picklist(Session("PCKListToResume"))
                If Not pcklst.Status.Equals(Statuses.Picklist.COMPLETE, StringComparison.InvariantCultureIgnoreCase) Then
                    PicklistID = Session("PCKListToResume")
                End If
            End If
            If DO1.Value("PicklistId") <> "" Then
                PicklistID = DO1.Value("PicklistId")
            End If
            If Not PicklistID Is Nothing Then
                Dim tm As New WMS.Logic.TaskManager
                Dim pcklst As Picklist
                pcklst = New Picklist(PicklistID)

                If pcklst.IsPickTaskAssigendToOtherUser(PicklistID, UserId) = False Then
                    tm.RequestPartialPickTaskByPicklistId(UserId, WMS.Lib.TASKTYPE.PARTIALPICKING, PicklistID, Nothing)
                    If Not tm.Task Is Nothing Then
                        Session("TMTask") = tm.Task
                        pcklst = New Picklist(tm.Task.Picklist)
                        Session("PCKPicklist") = pcklst
                        If pcklst.IsFirstPick = False Then
                            NextClicked()
                        End If
                    End If
                End If
            End If

            If Not CheckAssigned() Then
                ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:PickListIdval:tb"))
                NextClicked()
                DO1.Button("Reverse").Visible = False 'RWMS-3736
            End If
        Else
            ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:ActionBar:_ctl1:InnerButton"))
        End If
    End Sub

    Private Sub MenuClick()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, TASKTYPE.PICKING, LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked(Optional ByVal onload As Boolean = False, Optional ByVal bUserSelection As Boolean = False)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If onload = True Then
            If DO1.Value("PicklistId") <> "" Then
                Dim pcklst As Picklist
                pcklst = New Picklist(DO1.Value("PicklistId"))
                If pcklst.IsPickTaskAssigendToOtherUser(DO1.Value("PicklistId"), UserId) = True Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pick task assigned to other user"))
                    Return
                End If
            End If
        End If
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                tm.RequestPickTaskByPicklistId(UserId, WMS.Lib.TASKTYPE.PICKING, DO1.Value("PickListId"), Nothing)
                Session("TMTask") = tm
                If Not CheckAssigned() Then
                    If onload Then
                        DO1.Value("PicklistId") = ""
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Invalid PickList"))
                    End If
                Else
                    If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                        If Session("ShowedTaskManager") Is Nothing Then
                            Session("ShowedTaskManager") = True
                            Session("TargetScreen") = "screens/specpck.aspx"
                            Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                        End If
                        'RWMS -2968
                    Else
                        Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                    End If
                End If
            Catch ex As Made4Net.Shared.M4NException
                DO1.Value("Picklist") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Threading.ThreadAbortException
            Catch ex As Exception
                DO1.Value("Picklist") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            End Try
        Else
            Session.Remove("ShowedTaskManager")
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            Dim pcklst As Picklist
            If tm.Task IsNot Nothing Then
                pcklst = New Picklist(tm.Task.Picklist)
                Session("PCKPicklist") = pcklst
            Else
                MyBase.WriteToRDTLog("No pick task available for the user {0}", UserId)
            End If

            Dim contid As String
            contid = Session("PCKPicklistActiveContainerID")
            If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
                If DO1.Value("ContainerId") = "" Or DO1.Value("ContainerId") <> contid Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKCLOSECONTAINER.aspx?sourcescreen=specpck"))
                Else
                    If Not SetContainerID() Then Return
                End If
            Else
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

            If pcklst IsNot Nothing AndAlso pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
            End If
        End If
    End Sub
    Private Sub sendREVREPLEN()

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.REVIEW_REPLEN)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.REVIEW_REPLEN)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", Session("Consignee"))
        aq.Add("PCKPicklist", Session("PCKPicklist"))
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Send(WMS.Lib.Actions.Audit.REVIEW_REPLEN)

    End Sub
    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        Dim task As WMS.Logic.Task
        If Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim pcklst As Picklist
            Try
                If Not Session("PCKListToResume") Is Nothing Then
                    Dim tsk As String = Made4Net.DataAccess.DataInterface.ExecuteScalar($"select top 1 task from tasks where picklist='{Session("PCKListToResume")}' and tasktype  like '%pick%'")
                    task = New PickTask(tsk)
                    Session("PCKListToResume") = Nothing
                Else
                    task = tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger())
                End If
                pcklst = New Picklist(task.Picklist)
            Catch ex As Exception

            End Try
            If Not pcklst Is Nothing Then
                Session("PCKPicklist") = pcklst
                If MobileUtils.ShowGoalTimeOnTaskAssignment Then
                    Session("TMTask") = task
                End If
                'Commented for RWMS-1643 and RWMS-745
                'SetContainerID()
                'End Commented for RWMS-1643 and RWMS-745
                setAssigned(pcklst)
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
        DO1.LeftButtonText = "requestpick"
        DO1.setVisibility("PickListId", True)

        DO1.setVisibility("Picklist", False)
        DO1.setVisibility("Consignee", False)
        DO1.setVisibility("Orderid", False)
        DO1.setVisibility("TargetCompany", False)
        DO1.setVisibility("RequestedDate", False)
        DO1.setVisibility("ContainerID", False)
        DO1.setVisibility("ContainerTypeDesc", False)
        DO1.setVisibility("Pick Cube", False)
    End Sub

	'RWMS-3734
    Private Sub ReverseClicked()
        Response.Redirect(MapVirtualPath("Screens/VRFIReverse.aspx"))
    End Sub
    'RWMS-3734
    'Added for RWMS-1643 and RWMS-745 replaced sub with function and made as boolean return
    Private Function SetContainerID() As Boolean
        'Commented for RWMS-745
        'If Session("PCKPicklistActiveContainerID") Is Nothing Then
        ' Dim pcklist As Picklist = Session("PCKPicklist")
        ' If Not pcklist Is Nothing Then
        ' If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
        ' Dim contid As String = DO1.Value("ContainerId")
        ' If contid.Trim = "" Then
        ' contid = Made4Net.Shared.Util.getNextCounter("CONTAINER")
        ' End If
        ' Session("PCKPicklistActiveContainerID") = contid
        ' DO1.Value("ContainerId") = contid
        ' End If
        ' End If
        'End If
        'End Commented for RWMS-745
        'Added for  RWMS-1643 and RWMS-745
        Dim pcklist As Picklist = Session("PCKPicklist")
        If Not pcklist Is Nothing Then
            If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                Dim contid As String = DO1.Value("ContainerId")
                Dim errMsg As String
                If Not MobileUtils.CheckContainerID(pcklist.PicklistID, contid, errMsg) Then
                    DO1.Value("ContainerId") = ""
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
                    Return False
                End If
                Session("PCKPicklistActiveContainerID") = contid

            End If
        End If
        Return True
        'Ended  for RWMS-1643 and  RWMS-745
    End Function


    Protected Sub setAssigned(ByVal pcklst As Picklist)
        Dim oOrder As WMS.Logic.OutboundOrderHeader
        Dim oComp As WMS.Logic.Company
        Dim contid As String

        DO1.Value("Assigned") = "Assigned"
        DO1.setVisibility("Picklist", True)
        DO1.setVisibility("Consignee", True)
        DO1.setVisibility("Orderid", True)
        DO1.setVisibility("TargetCompany", True)
        DO1.setVisibility("RequestedDate", True)
        DO1.setVisibility("PickListId", False)
        DO1.setVisibility("Pick Cube", False)

        If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            DO1.setVisibility("ContainerId", False)
            DO1.setVisibility("ContainerTypeDesc", False)
        Else
            DO1.setVisibility("ContainerId", True)
            DO1.setVisibility("ContainerTypeDesc", True)
            contid = pcklst.ActiveContainer()
            If contid = "" Then
                DO1.Value("ContainerId") = ""
            Else
                DO1.Value("ContainerId") = contid
                Session("PCKPicklistActiveContainerID") = contid
            End If
            Dim sqltype As String = " select containerdesc from handelingunittype " &
                          " WHERE container = '" & pcklst.HandelingUnitType & "'"
            DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)
        End If

        oOrder = New OutboundOrderHeader(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId)
        oComp = New Company(oOrder.CONSIGNEE, oOrder.TARGETCOMPANY, oOrder.COMPANYTYPE)

        If pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK Then
            DO1.setVisibility("Pick Cube", True)
            DO1.Value("Pick Cube") = pcklst.GetPickCube()
        End If

        DO1.Value("Picklist") = pcklst.PicklistID
        DO1.Value("Consignee") = oOrder.CONSIGNEE
        DO1.Value("Orderid") = oOrder.ORDERID
        DO1.Value("TargetCompany") = oComp.COMPANYNAME
        DO1.Value("RequestedDate") = oOrder.REQUESTEDDATE
        DO1.LeftButtonText = "Next"
        '---------- Pass the fields to other screens
        Session("Consignee") = oOrder.CONSIGNEE
        Session("Orderid") = oOrder.ORDERID
        Session("TargetCompany") = oComp.COMPANYNAME
        '----------------
        If Len(pcklst.PICKORDERSTATUS) > 0 Then
            If (pcklst.PICKORDERSTATUS = "REVERSE") Then
                DO1.Button("Reverse").Visible = False
            ElseIf (pcklst.PICKORDERSTATUS <> "REVERSE") And pcklst.IsFirstPick = True Then
                DO1.Button("Reverse").Visible = True
            Else
                DO1.Button("Reverse").Visible = False
            End If
        Else
            If pcklst.IsFirstPick = False Then
                DO1.Button("Reverse").Visible = False
            Else
                DO1.Button("Reverse").Visible = True
            End If
        End If
    End Sub
    '--------RWMS 3726----

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Assigned")
        DO1.AddLabelLine("Picklist")
        DO1.AddSpacer()
        DO1.AddLabelLine("Consignee")
        DO1.AddLabelLine("Orderid")
        DO1.AddLabelLine("TargetCompany")
        DO1.AddLabelLine("RequestedDate")
        DO1.AddLabelLine("Pick Cube")
        DO1.AddLabelLine("ContainerTypeDesc")
        DO1.AddTextboxLine("ContainerID")
        DO1.AddTextboxLine("PickListId", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                NextClicked(True, True)
            Case "reverse"
                ReverseClicked()
            Case "requestpick"
                NextClicked(True)
            Case "menu"
                MenuClick()
        End Select
    End Sub
End Class