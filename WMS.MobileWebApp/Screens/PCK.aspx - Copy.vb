Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

<CLSCompliant(False)> Public Class PCK
    Inherits System.Web.UI.Page

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

            Session("MobileSourceScreen") = "PCK"
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
                Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            End If

            If Not CheckAssigned() Then
                NextClicked()
            End If
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                If Session("ShowedTaskManager") Is Nothing Then
                    Session("ShowedTaskManager") = True
                    Dim task As WMS.Logic.Task = New WMS.Logic.Task(WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING))
                    Session("TMTask") = task

                    If MobileUtils.ShouldRedirectToTaskSummary() AndAlso Not Session("ShowedTaskManager") Then
                        Session("TaskID") = task.TASK
                    End If

                    Session("TargetScreen") = "screens/pck.aspx"
                    Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                End If
            End If
        End If
    End Sub

    Private Sub MenuClick()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING) Then
            Dim tm As New WMS.Logic.TaskManager(UserId, WMS.Lib.TASKTYPE.PICKING)
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                Dim TMTask As WMS.Logic.Task = tm.RequestTask(UserId, WMS.Lib.TASKTYPE.PICKING)
                CheckAssigned()
                If MobileUtils.ShowGoalTimeOnTaskAssignment() AndAlso Not Session("ShowedTaskManager") Then
                    Session("ShowedTaskManager") = True
                    Session("TMTask") = TMTask
                    Session("TargetScreen") = "screens/pck.aspx"
                    Response.Redirect(MapVirtualPath("screens/taskmanager.aspx"))
                End If
                If MobileUtils.ShouldRedirectToTaskSummary() Then
                    Session("TaskID") = TMTask.TASK
                End If
            Catch ex As Made4Net.Shared.M4NException
                MessageQue.Enqueue(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Threading.ThreadAbortException
            Catch ex As Exception
                MessageQue.Enqueue(trans.Translate(ex.Message))
            End Try
        Else
            Session.Remove("ShowedTaskManager")
            Dim tm As New WMS.Logic.TaskManager(UserId, WMS.Lib.TASKTYPE.PICKING)
            Dim pcklst As New Picklist(tm.Task.Picklist)
            Session("PCKPicklist") = pcklst

            'If Not (pcklst.HandelingUnitType = WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType") And pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK) Then 'Session("MHType")  = HANDLINGEQUIPMENT
            '    Try
            'If Not ((pcklst.HandelingUnitType = WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType") And pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK)) Then 'Session("MHType")  = HANDLINGEQUIPMENT
            If Not SetContainerID() Then Return
            'End If


            '    Catch ex As Made4Net.Shared.M4NException
            '        MessageQue.Enqueue(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            '        Return
            '    Catch ex As Exception
            '        MessageQue.Enqueue(ex.Message)
            '        Return
            '    End Try
            'End If

            If pcklst.ShouldPrintPickLabelOnPicking Then
                Response.Redirect(MapVirtualPath("Screens/PCKLBLPRNT.aspx"))
            Else
                If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
                Else
                    If pcklst.getReleaseStrategy().ConfirmationType = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATIONUOM Or pcklst.getReleaseStrategy().ConfirmationType = WMS.Lib.Release.CONFIRMATIONTYPE.SKUUOM Then
                        Response.Redirect(MapVirtualPath("Screens/PCKPARTUOM.aspx"))
                    Else
                        'If (pcklst.HandelingUnitType = WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType") And pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK) Then 'Session("MHType")  = HANDLINGEQUIPMENT
                        '    Response.Redirect(MapVirtualPath("Screens/PCKPARTMULTICONT.aspx"))
                        'Else
                        Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
                        'End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function SetContainerID() As Boolean
        '  If Session("PCKPicklistActiveContainerID") Is Nothing Then
        Dim pcklist As Picklist = Session("PCKPicklist")
        If Not pcklist Is Nothing Then
            If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                Dim contid As String = DO1.Value("ContainerId")               
                Dim errMsg As String
                If Not MobileUtils.CheckContainerID(pcklist.PicklistID, contid, errMsg) Then
                    DO1.Value("ContainerId") = ""
                    MessageQue.Enqueue(errMsg)
                    Return False
                End If
                Session("PCKPicklistActiveContainerID") = contid

            End If
        End If
        ' End If
        Return True
    End Function

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        If Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING) Then
            Dim pcklst As Picklist
            Try
                pcklst = New Picklist(tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PICKING).Picklist)
            Catch ex As Exception

            End Try
            If Not pcklst Is Nothing Then
                Session("PCKPicklist") = pcklst
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
        DO1.LeftButtonText = "next"

        DO1.setVisibility("Picklist", False)
        DO1.setVisibility("PickType", False)
        DO1.setVisibility("PickMethod", False)
        DO1.setVisibility("ContainerId", False)
        DO1.setVisibility("ContainerType", False)
        DO1.setVisibility("ContainerTypeDesc", False)
    End Sub

    Protected Sub setAssigned(ByVal pcklst As Picklist)
        Dim contid As String
        DO1.Value("Assigned") = "Assigned"
        DO1.setVisibility("Picklist", True)
        DO1.setVisibility("PickType", True)
        DO1.setVisibility("PickMethod", True)
        If pcklst.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            DO1.setVisibility("ContainerId", False)
            DO1.setVisibility("ContainerType", False)
            DO1.setVisibility("ContainerTypeDesc", False)
        Else
            DO1.setVisibility("ContainerId", True)
            DO1.setVisibility("ContainerType", False)
            DO1.setVisibility("ContainerTypeDesc", True)
            DO1.Value("ContainerType") = pcklst.HandelingUnitType

            'If (pcklst.HandelingUnitType = WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType") And pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK) Then 'Session("MHType")  = HANDLINGEQUIPMENT
            '    'go to Multiple containers screen
            '    DO1.setVisibility("ContainerId", False)
            '    DO1.setVisibility("ContainerType", False)
            '    DO1.setVisibility("ContainerTypeDesc", False)
            'Else
            contid = pcklst.ActiveContainer()
            If contid = "" Then
                DO1.Value("ContainerId") = ""
            Else
                DO1.Value("ContainerId") = contid
                Session("PCKPicklistActiveContainerID") = contid
            End If
            'End If


            Dim sqltype As String = " select containerdesc from handelingunittype " & _
                          " WHERE container = '" & pcklst.HandelingUnitType & "'"
            DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)
        End If

        DO1.Value("Assigned") = "Assigned"
        DO1.Value("Picklist") = pcklst.PicklistID
        DO1.Value("PickMethod") = pcklst.PickMethod
        DO1.Value("PickType") = pcklst.PickType
        DO1.LeftButtonText = "Next"
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Assigned")
        DO1.AddLabelLine("Picklist")
        DO1.AddSpacer()
        DO1.AddLabelLine("PickType")
        DO1.AddLabelLine("PickMethod")
        DO1.AddLabelLine("ContainerType")
        DO1.AddLabelLine("ContainerTypeDesc")
        DO1.AddTextboxLine("ContainerID")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Try
            Select Case e.CommandText.ToLower
                Case "next"
                    NextClicked()
                Case "requestpick"
                    NextClicked()
                Case "menu"
                    MenuClick()
            End Select
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub
End Class
