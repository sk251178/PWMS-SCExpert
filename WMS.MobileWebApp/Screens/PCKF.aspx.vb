Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKF
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        End If
    End Sub

    Private Sub MenuClick()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.FULLPICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub NextClicked()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Try
                Dim tm As New WMS.Logic.TaskManager
                tm.RequestTask(UserId, WMS.Lib.TASKTYPE.FULLPICKING)
                CheckAssigned()
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            End Try
        Else
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.FULLPICKING, LogHandler.GetRDTLogger())
            Dim pcklst As New Picklist(tm.Task.Picklist)
            Session("PCKPicklist") = pcklst
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
                    Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
            End If
        End If
    End Sub

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        If Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim pcklst As Picklist
            Try
                pcklst = New Picklist(tm.getAssignedTask(UserId, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Logic.LogHandler.GetRDTLogger()).Picklist)
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
    End Sub

    Protected Sub setAssigned(ByVal pcklst As Picklist)
        DO1.Value("Assigned") = "Assigned"

        DO1.setVisibility("Picklist", True)
        DO1.setVisibility("PickType", True)
        DO1.setVisibility("PickMethod", True)

        DO1.setVisibility("ContainerId", False)
        DO1.setVisibility("ContainerType", False)
        DO1.setVisibility("ContainerTypeDesc", False)

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