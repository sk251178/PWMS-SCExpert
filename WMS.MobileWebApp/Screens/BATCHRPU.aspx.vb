Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class BATCHRPU
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK)
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENUNLOADHEADER)
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION)
            Session.Remove(WMS.Lib.SESSION.SHOWTASKMANAGER)
            SetScreen()
        End If
    End Sub

    Private Sub SetScreen()
        DO1.Value("CONTAINER") = String.Empty
    End Sub

    Private Sub doNext()
        If IsValidated() Then
            If MobileUtils.ShowGoalTimeOnTaskAssignment() AndAlso Not Session(WMS.Lib.SESSION.SHOWTASKMANAGER) Then
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENUNLOAD1))
            End If
        End If
    End Sub

    Private Function IsValidated() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            ' Get and validate the Batch Replenishment Header
            Dim brh As BatchReplenHeader = New BatchReplenHeader(True, DO1.Value("CONTAINER").ToString())
            If brh.STATUS <> WMS.Lib.Statuses.BatchReplenHeader.LETDOWN Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid batch status"))
                Return False
            End If
            ' Verify the Task, if not valid it will throw Made4Net.Shared.M4NException
            Dim tm As New WMS.Logic.TaskManager
            Dim task As Task = tm.RequestTask(WMS.Logic.Common.GetCurrentUser(), WMS.Lib.TASKTYPE.BRUNLOAD, WMS.Logic.LogHandler.GetRDTLogger(), brh.BATCHREPLID)
            If task Is Nothing Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No Available task found..."))
                Return False
            End If
            WriteToRDTLog("Requested TASK {0}, TASKTYPE {1}, STATUS={2}", task.TASK, task.TASKTYPE, task.STATUS)
            ' Get any detail's that are in "LETDOWN" status
            Dim crdC As BatchReplenDetailCollection = New BatchReplenDetailCollection(brh.BATCHREPLID, WMS.Lib.Statuses.BatchReplenDetail.LETDOWN)
            If crdC.Count > 0 Then
                crdC.Sort("REPLLINE", BreplenDetailSortDirection.Descending)
                Session.Add(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK, task)
                Session.Add(WMS.Lib.SESSION.BATCHREPLENUNLOADHEADER, brh)
                Session.Add(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION, crdC)
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No Details for Container selected"))
                Return False
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return False
        End Try
        Return True
    End Function

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONTAINER", False, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class