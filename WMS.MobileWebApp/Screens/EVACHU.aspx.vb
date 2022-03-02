Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

Partial Public Class EVACHU
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
    End Sub

    Private Sub doNext()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            ' Check if there is tasks for this location
            Dim sqlCheckForTasks As String = "SELECT TASK FROM TASKS WHERE TASKTYPE='EMPHUDEL' AND FROMLOCATION='" & DO1.Value("FROMLOCATION") & "' AND FROMWAREHOUSEAREA='" & DO1.Value("FROMWAREHOUSEAREA") & "' "
            Dim dt As DataTable = New DataTable()
            DataInterface.FillDataset(sqlCheckForTasks, dt)

            If dt.Rows.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No empty HU pickup tasks for this location"))
                Return
            Else
                Dim HUPickUpTask As EmptyHUPickupTask = New EmptyHUPickupTask(dt.Rows(0)("TASK"))
                Dim HUPickUpJob As EmptyHUPickupJob = HUPickUpTask.getDeliveryJob()
                Session("EmptyHUPickUpJob") = HUPickUpJob
                Session("FromLocation") = DO1.Value("FROMLOCATION")
                Session("FromWarehousearea") = DO1.Value("FROMWAREHOUSEAREA")

                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/EVACHU1.aspx"))

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
        Session.Remove("FromLocation")
        Session.Remove("FromWarehousearea")
        Session.Remove("EmptyHUPickUpJob")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("FROMLOCATION")
        DO1.AddTextboxLine("FROMWAREHOUSEAREA")
        DO1.AddSpacer()
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