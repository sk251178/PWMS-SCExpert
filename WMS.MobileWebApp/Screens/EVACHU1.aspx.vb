Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

Partial Public Class EVACHU1
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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            DO1.Value("FROMLOCATION") = Session("FromLocation")
            DO1.Value("FROMWAREHOUSEAREA") = Session("FromWAREHOUSEAREA")
        End If

    End Sub

    Private Sub doNext()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Dim HUPickUpJob As EmptyHUPickupJob = Session("EmptyHUPickUpJob")
            If HUPickUpJob.ToLocation = DO1.Value("TOLOCATION") And _
            HUPickUpJob.ToWarehousearea = DO1.Value("TOWAREHOUSEAREA") Then
                Dim ts As EmptyHUPickupTask = New EmptyHUPickupTask(HUPickUpJob.TaskId)
                ts.Deliver(WMS.Logic.GetCurrentUser)

                Session.Remove("TMTaskId")
                Session.Remove("TOLOCATION")
                Session.Remove("FromLocation")
                If Not String.IsNullOrEmpty(Session("MOBILESOURCESCREEN")) Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/" & Session("MOBILESOURCESCREEN") & ".aspx"))
                Else
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/EVACHU.aspx"))
                End If

            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("To location confirmation incorrect"))
                Return
            End If
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

    Private Sub doMenu()
        Session.Remove("EmptyHUPickUpJob")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddLabelLine("FROMLOCATION")
        DO1.AddTextboxLine("TOLOCATION")
        DO1.AddLabelLine("FROMWAREHOUSEAREA")
        DO1.AddTextboxLine("TOWAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "finish"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

End Class