Imports WMS.Logic
Imports Made4Net.Shared
Imports System.Data
Imports Made4Net.DataAccess
Imports Made4Net.Mobile

Partial Public Class Packing1
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
        DO1.AddLabelLine("PACKINGLISTID", "Packing List")
        DO1.AddTextboxLine("LOADID")
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer")
        DO1.AddSpacer()
        DO1.AddSpacer()
    End Sub

    Private Sub doPackLoad(ByVal pUnpackLoad As Boolean)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oPackList As WMS.Logic.PackingListHeader = GetPackingList()
            If pUnpackLoad Then
                oPackList.UnPackLoad(DO1.Value("LOADID"), WMS.Logic.Common.GetCurrentUser)
                DO1.Value("LOADID") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Unpacked"))
            Else
                oPackList.PackLoad(DO1.Value("LOADID"), WMS.Logic.Common.GetCurrentUser)
                DO1.Value("LOADID") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Packed"))
            End If
            Session("PACKINGLIST") = oPackList
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.ToString())
        End Try
    End Sub

    Private Function GetPackingList() As WMS.Logic.PackingListHeader
        Dim oPackList As WMS.Logic.PackingListHeader = Session("PACKINGLIST")
        If oPackList.Lines.Count = 0 Then
            Dim sql As String = String.Format("select oh.consignee,oh.orderid,oh.targetcompany,oh.companytype from orderloads ol inner join outboundorheader oh on oh.orderid = ol.orderid and oh.consignee = ol.consignee where loadid = '{0}'", DO1.Value("LOADID"))
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                Dim oComp As New WMS.Logic.Company(dt.Rows(0)("consignee"), dt.Rows(0)("targetcompany"), dt.Rows(0)("companytype"))
                If WMS.Logic.PackingListHeader.Exists(oPackList.PACKINGLISTID) Then
                    oPackList.Update(oComp.CONSIGNEE, oComp.COMPANY, oComp.COMPANYTYPE, oComp.DEFAULTCONTACT, 0, WMS.Logic.Common.GetCurrentUser)
                Else
                    oPackList.Create(oPackList.PACKINGLISTID, oComp.CONSIGNEE, oComp.COMPANY, oComp.COMPANYTYPE, oComp.DEFAULTCONTACT, 0, WMS.Logic.Common.GetCurrentUser)
                End If
            End If
        End If
        Return oPackList
    End Function

    Private Sub doPrintPackingList()
        Try
            Dim oPackList As WMS.Logic.PackingListHeader = Session("PACKINGLIST")
            oPackList.PrintPackingList(DO1.Value("Printer"), Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.ToString())
            Return
        End Try
    End Sub

    Private Sub doBack()
        Session.Remove("PACKINGLIST")
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/Packing.aspx"))
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText
            Case "PackLoad"
                doPackLoad(False)
            Case "UnPackLoad"
                doPackLoad(True)
            Case "Back"
                doBack()
            Case "PrintPackingList"
                doPrintPackingList()
        End Select
    End Sub

End Class