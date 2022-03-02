Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic


Partial Public Class BagOutCloseContainer
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Public Shared activCount As Integer = 0
    Public Shared activCont As String
    Public lastPick As Boolean = False
    Public picklistComplete As Boolean = False
    Public sourceScreen As String = "PCKBAGOUT"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack() Then
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            activCount = 0
            activCont = ""
            CheckPicklistResume()
            If picklistComplete Then
                lastPick = Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, Nothing)
            Else
                lastPick = False
            End If
            If picklistComplete Then
                If lastPick Then
                    DO1.Value("Info") = trans.Translate("No pick task available. Please close the tote.")
                Else
                    DO1.Value("Info") = trans.Translate("Pickist complete. Would you like to close the tote?")
                End If
            Else
                DO1.Value("Warning") = trans.Translate("Are you sure you want to close tote?")
            End If
            DO1.setVisibility("CONTAINER", True)
            DO1.Value("CONTAINER") = Session("PCKPicklistActiveContainerID")
            'DO1.Button(1).Visible = Not lastPick
            WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, Nothing)
            MyBase.WriteToRDTLog(" Pck closing container page. ")
        End If
    End Sub

    Private Function IsActive(ByVal cont As String) As Boolean
        Dim pcklist As Picklist = Session("PCKPicklist")
        Dim sql As String = String.Format("select COUNT(1) from PICKDETAIL where PICKLIST='{0}' and TOCONTAINER='{1}'", pcklist.PicklistID, cont)
        If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "close and deliver"
                close()
            Case "back"
                doBack()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As EventArgs) Handles DO1.CreatedChildControls

        If picklistComplete Then
            If lastPick Then
                DO1.AddLabelLine("Info")
                DO1.Button(3).Text = ""
            Else
                DO1.AddLabelLine("Info")
                DO1.Button(3).Text = "Next"
            End If
        Else
            DO1.AddLabelLine("Warning")
            DO1.Button(3).Text = "Back"
        End If
        DO1.AddSpacer()
        DO1.AddLabelLine("CONTAINER")
    End Sub

    Private Sub doBack()
        Try
            If Not Request.QueryString("sourcescreen") Is Nothing Then
                Dim src As String = Request.QueryString("sourcescreen")
                Response.Redirect(MapVirtualPath("screens/" & src & ".aspx"))
            Else
                Response.Redirect(MapVirtualPath("screens/PCKPART.aspx"))
            End If

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub
    Private Sub doDeliver()
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        Dim pcklist As WMS.Logic.Picklist = New Picklist(pck.picklist)

        Try
            Dim pContainerId As String = DO1.Value("CONTAINER")
            pcklist.CloseContainer(DO1.Value("CONTAINER"), True, WMS.Logic.GetCurrentUser)
            If lastPick Then
                sourceScreen = "PCKBagOut"
            End If


            If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                If Not Session("PCKPicklist") Is Nothing Then
                    Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                    If oPicklist.ShouldPrintShipLabel Then
                        oPicklist.PrintShipLabels(prntr.PrinterQName)
                    End If
                    If oPicklist.isCompleted Then
                        PickTask.UpdateCompletionTime(oPicklist)
                    End If
                ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                    Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
                    pck2.PrintShipLabels(prntr.PrinterQName)
                End If
                If Not Session("PCKPicklist") Is Nothing Then
                    Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                    If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                        Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    End If
                End If
                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
            Else
                Response.Redirect(MapVirtualPath($"screens/DELLBLPRNT.aspx?sourcescreen={sourceScreen}"))
            End If

        Catch ex As System.Threading.ThreadAbortException
        End Try

    End Sub
    Private Sub close()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        MyBase.WriteToRDTLog(" Close method call")
        Session("BAGOUTCLOSECONTAINER") = 1
        'CheckPicklistResume()

        If DO1.Value("CONTAINER").Trim = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Cannot Close Container- Container is blank"))
            Exit Sub
        End If

        If Not IsActive(DO1.Value("CONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("can not deliver container, nothing was picked"))
            Exit Sub
        End If

        If Not WMS.Logic.Container.Exists(DO1.Value("CONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("container does not exist"))
            Exit Sub
        End If
        doDeliver()
    End Sub

    Private Sub CheckPicklistResume()
        Dim pcklist As Picklist
        If Not Session("PCKPicklist") Is Nothing Then
            pcklist = Session("PCKPicklist")
            picklistComplete = pcklist.isCompleted
            If pcklist.isCompleted = False Then
                Session("PCKListToResume") = pcklist.PicklistID
            Else
                Session.Remove("PCKListToResume")
            End If
        End If
    End Sub
End Class