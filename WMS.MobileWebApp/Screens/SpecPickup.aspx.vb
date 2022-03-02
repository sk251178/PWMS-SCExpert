Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Partial Public Class SpecPickup
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.SPICKUP, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Dim ts As WMS.Logic.Task
                ts = New WMS.Logic.Task(WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.SPICKUP, WMS.Logic.LogHandler.GetRDTLogger()))
                DO1.Value("LOCATION") = ts.FROMLOCATION
                If ts.FROMLOAD <> String.Empty Then
                    DO1.Value("LOADID") = ts.FROMLOAD
                    DO1.setVisibility("HANDLINGUNIT", False)
                Else
                    DO1.Value("HANDLINGUNIT") = ts.FromContainer
                    DO1.setVisibility("LOADID", False)
                End If
                Session("SPECPICKUPTASK") = ts
            End If
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("SPECPICKUPTASK")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("HANDLINGUNIT")
        DO1.AddSpacer()
        DO1.AddTextboxLine("CONFIRM")
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try
            If Not ValidateForm() Then
                Return
            End If
            Dim ts As WMS.Logic.Task = Session("SPECPICKUPTASK")
            If ts.FROMLOAD <> String.Empty Then
                Dim ld As WMS.Logic.Load = New WMS.Logic.Load(DO1.Value("CONFIRM"))
                Session("SPICKUPLOAD") = DO1.Value("LOADID")
                If Not ld.LOCATION.Equals(ts.FROMLOCATION, StringComparison.OrdinalIgnoreCase) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load not in the requested location"))
                    Return
                End If
                'ld.RequestPickUp(WMS.Logic.GetCurrentUser)
            Else
                Dim cnt As WMS.Logic.Container = New WMS.Logic.Container(DO1.Value("CONFIRM"), True)
                Session("SPICKUPCONTAINER") = DO1.Value("CONFIRM")
                If Not cnt.Location.Equals(ts.FROMLOCATION, StringComparison.OrdinalIgnoreCase) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Container not in the requested location"))
                    Return
                End If
                'cnt.RequestPickUp(WMS.Logic.GetCurrentUser)
            End If
            ts.Complete(WMS.Logic.LogHandler.GetRDTLogger)
            If ts.FromContainer <> String.Empty Then
                Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
            End If
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Function ValidateForm() As Boolean
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim ts As WMS.Logic.Task = Session("SPECPICKUPTASK")
        If ts.FROMLOAD <> String.Empty Then
            If Not WMS.Logic.Load.Exists(DO1.Value("CONFIRM")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load does not exist"))
                Return False
            Else
                If Not DO1.Value("CONFIRM").Equals(ts.FROMLOAD, StringComparison.OrdinalIgnoreCase) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load does not match to task"))
                    Return False
                End If
            End If
        Else
            If Not WMS.Logic.Container.Exists(DO1.Value("CONFIRM")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Container does not exist"))
                Return False
            Else
                If Not DO1.Value("CONFIRM").Equals(ts.FromContainer, StringComparison.OrdinalIgnoreCase) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Handling unit does not match to task"))
                    Return False
                End If
            End If
        End If
        Return True
    End Function

End Class