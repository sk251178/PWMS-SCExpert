Imports WMS.Logic
Imports Made4Net.Shared
Imports System.Data
Imports Made4Net.DataAccess
Imports Made4Net.Mobile

Partial Public Class Packing
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("PACKINGLISTID", True, "PackLoads", "Packing List")
        DO1.AddSpacer()
    End Sub

    Private Sub doCreatePackingList()
        If Create() Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Packing1.aspx"))
        End If
    End Sub

    Private Sub doPackLoads()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not WMS.Logic.PackingListHeader.Exists(DO1.Value("PACKINGLISTID")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Packing List does not exist"))
            Return
        End If
        Session("PACKINGLIST") = New WMS.Logic.PackingListHeader(DO1.Value("PACKINGLISTID"))
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Packing1.aspx"))
    End Sub

    Private Function Create() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim packlist As New WMS.Logic.PackingListHeader
        Try
            If WMS.Logic.PackingListHeader.Exists(DO1.Value("PACKINGLISTID")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Packing List Already exists"))
                DO1.Value("PACKINGLISTID") = ""
                Return False
            Else
                packlist.PACKINGLISTID = DO1.Value("PACKINGLISTID")
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            DO1.Value("PACKINGLISTID") = ""
            Return False
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.ToString())
            DO1.Value("PACKINGLISTID") = ""
            Return False
        End Try
        Session("PACKINGLIST") = packlist
        Return True
    End Function

    Private Sub doMenu()
        Session.Remove("PACKINGLIST")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText
            Case "CreatePackingList"
                doCreatePackingList()
            Case "PackLoads"
                doPackLoads()
            Case "Menu"
                doMenu()
        End Select
    End Sub

End Class