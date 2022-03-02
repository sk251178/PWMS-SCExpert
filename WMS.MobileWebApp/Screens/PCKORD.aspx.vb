Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKORD
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
            Session.Remove("PCKPicklist")
            Session.Remove("PCKDeliveryJob")
            Session.Remove("PCKPicklistPickJob")
            Session.Remove("PCKPicklistActiveContainerID")
            ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:ActionBar:_ctl1:InnerButton"))
            If Not CheckAssigned() Then
                ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:ConsigneeInpval:tb"))
                NextClicked()
            End If
        Else
            ClientScript.RegisterStartupScript(Page.GetType, "", DO1.SetFocusElement("DO1:ActionBar:_ctl1:InnerButton"))
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

    Private Sub NextClicked(Optional ByVal onload As Boolean = False)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim rdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                tm.RequestPickTaskByOrderId(UserId, WMS.Lib.TASKTYPE.PICKING, DO1.Value("ConsigneeInp"), DO1.Value("OrderIdInp"), rdtLogger)
                If Not CheckAssigned() Then
                    If onload Then
                        DO1.Value("ConsigneeInp") = ""
                        DO1.Value("OrderIdInp") = ""
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Invalid PickList"))
                    End If
                End If
            Catch ex As Made4Net.Shared.M4NException
                DO1.Value("ConsigneeInp") = ""
                DO1.Value("OrderIdInp") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                DO1.Value("ConsigneeInp") = ""
                DO1.Value("OrderIdInp") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            End Try
        Else
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            Dim pcklst As New Picklist(tm.Task.Picklist)
            Session("PCKPicklist") = pcklst
            'SetContainerID()
            Dim errMsg As String
            If Not MobileUtils.CheckContainerID(pcklst.PicklistID, DO1.Value("ContainerId"), errMsg) Then
                DO1.Value("ContainerId") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errMsg)
                Return
            End If
            Response.Redirect(MapVirtualPath("Screens/PCKORD1.aspx"))
        End If
    End Sub

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        If Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim pcklst As Picklist
            Try
                pcklst = New Picklist(tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()).Picklist)
            Catch ex As Exception
            End Try
            If Not pcklst Is Nothing Then
                Session("PCKPicklist") = pcklst
                SetContainerID()
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
        DO1.setVisibility("ConsigneeInp", True)
        DO1.setVisibility("OrderIdInp", True)

        DO1.setVisibility("Picklist", False)
        DO1.setVisibility("Consignee", False)
        DO1.setVisibility("Orderid", False)
        DO1.setVisibility("TargetCompany", False)
        DO1.setVisibility("RequestedDate", False)
        DO1.setVisibility("ContainerID", False)
        DO1.setVisibility("ContainerTypeDesc", False)
    End Sub

    Private Sub SetContainerID()
        If Session("PCKPicklistActiveContainerID") Is Nothing Then
            Dim pcklist As Picklist = Session("PCKPicklist")
            If Not pcklist Is Nothing Then
                If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    Dim contid As String = DO1.Value("ContainerId")
                    If contid.Trim = "" Then
                        contid = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                    End If
                    Session("PCKPicklistActiveContainerID") = contid
                    DO1.Value("ContainerId") = contid
                End If
            End If
        End If
    End Sub

    Protected Sub setAssigned(ByVal pcklst As Picklist)
        Dim oOrder As WMS.Logic.OutboundOrderHeader
        Dim oComp As WMS.Logic.Company
        'Dim sOrder As String
        Dim contid As String

        DO1.Value("Assigned") = "Assigned"
        DO1.setVisibility("Picklist", True)
        DO1.setVisibility("Consignee", True)
        DO1.setVisibility("Orderid", True)
        DO1.setVisibility("TargetCompany", True)
        DO1.setVisibility("RequestedDate", True)
        DO1.setVisibility("OrderIdInp", False)
        DO1.setVisibility("ConsigneeInp", False)

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
            Dim sqltype As String = " select containerdesc from handelingunittype " & _
                          " WHERE container = '" & pcklst.HandelingUnitType & "'"
            DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)
        End If

        oOrder = New OutboundOrderHeader(pcklst.Lines(0).Consignee, pcklst.Lines(0).OrderId)
        oComp = New Company(oOrder.CONSIGNEE, oOrder.TARGETCOMPANY, oOrder.COMPANYTYPE)

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
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Assigned")
        DO1.AddSpacer()
        DO1.AddLabelLine("Consignee")
        DO1.AddLabelLine("Orderid")
        DO1.AddLabelLine("TargetCompany")
        DO1.AddLabelLine("RequestedDate")
        DO1.AddLabelLine("ContainerTypeDesc")
        DO1.AddTextboxLine("ContainerID")
        DO1.AddTextboxLine("ConsigneeInp")
        DO1.AddTextboxLine("OrderIdInp")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                NextClicked(True)
            Case "requestpick"
                NextClicked(True)
            Case "menu"
                MenuClick()
        End Select
    End Sub

End Class