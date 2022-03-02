Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports System.Text.RegularExpressions
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class PCKBagOutManager
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
    'Added for RWMS-323
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    Private trans As Object

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
            Session.Remove("PickListID")
            Session("MobileSourceScreen") = "PCKBagOutManager"
            Dim EnableBagOutPicking As String = Made4Net.Shared.Util.GetSystemParameterValue("BAGOUTPICKING").ToString()
            If EnableBagOutPicking = "1" Then
                Session("PCKBagOutPicking") = 1
                WMS.Logic.Picking.BagOutProcess = True
            Else
                WMS.Logic.Picking.BagOutProcess = False
                Session.Remove("PCKBagOutPicking")
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Bag Out Picking is not enabled"))
                Made4Net.Mobile.Common.GoToMenu()
            End If
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

    Private Sub NextClicked()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim picklistID As String
        picklistID = DO1.Value("PickListId")
        Session("PickListID") = picklistID
        Try
            If (String.IsNullOrEmpty(picklistID)) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("PicklistID is empty"))

                Return
            End If
            If Not WMS.Logic.Picklist.Exists(picklistID) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Invalid PicklistID"))
                DO1.Value("PickListId") = ""
                Return
            End If
            Dim pck As New Picklist(picklistID)
            If pck.Status <> "RELEASED" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("PickList is not valid"))
                DO1.Value("PickListId") = ""
                Return
            End If
            If pck.PickType = "FULLPICK" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please enter Partial PicklistID"))
                DO1.Value("PickListId") = ""
                Return
            End If
            Session("PickListID") = DO1.Value("PickListId")

            Dim pcklst As Picklist
            pcklst = New Picklist(DO1.Value("PickListId"))
            If pcklst.IsPickTaskAssigendToOtherUser(DO1.Value("PickListId"), UserId) = True Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pick task assigned to other user"))
                Return
            End If
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
        End Try
        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
    End Sub
    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("PickListId", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                NextClicked()
            Case "gettask"
                GetNextTask()
            Case "menu"
                MenuClick()
        End Select
    End Sub
    Sub GetNextTask()
        Session.Remove("PicklistID")
        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
    End Sub
End Class