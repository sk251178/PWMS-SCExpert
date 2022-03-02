Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Imports WMS.Logic
Partial Public Class LocationCountClearProblem
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
            DO1.Value("LOCATION") = Session("TaskLocationCNTLocationId")
            DO1.Value("WAREHOUSEAREA") = Session("TaskWarehouseareaCNTWarehouseareaId")
            Dim oLoc As New Location(Session("TaskLocationCNTLocationId"), Session("TaskWarehouseareaCNTWarehouseareaId"))
            Dim sql As String = String.Format("select problemrcdesc from LOCATIONPROBLEMRC where problemrc ='{0}'", oLoc.PROBLEMFLAGRC)
            DO1.Value("LocPRC") = DataInterface.ExecuteScalar(sql)
        End If

    End Sub

    Private Sub doEndCount(ByVal pClearLocationProblem As Boolean)
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oCntTask As WMS.Logic.CountTask
            If Not Session("LocationBulkCountTask") Is Nothing Then
                oCntTask = Session("LocationBulkCountTask")
            Else
                oCntTask = Session("LocationCountTask")
            End If
            Dim oCounting As New WMS.Logic.Counting(oCntTask.COUNTID)
            Dim oCountJob As WMS.Logic.CountingJob = Session("LocationCntJob")
            If pClearLocationProblem Then
                Dim oLoc As New WMS.Logic.Location(Session("TaskLocationCNTLocationId"), Session("TaskWarehouseareaCNTWarehouseareaId"))
                oLoc.SetProblemFlag(False, "", WMS.Logic.GetCurrentUser)
            End If
            oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser)
            Session.Remove("LocationCntJob")
            Session.Remove("LocationCNTLoadId")
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session.Remove("TSKTaskId")
            Session.Remove("LocationBulkCountTask")
            Session.Remove("LocationCountTask")
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location Count Completed"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
        End Try
        Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSrcScreen") & ".aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddLabelLine("LocPRC", "Location Problem Description")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "clearproblem&endcount"
                doEndCount(True)
            Case "endcount"
                doEndCount(False)
        End Select
    End Sub

End Class