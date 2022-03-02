Imports System.Collections.Generic
Partial Public Class VERIFYCANCELALL

    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("PayLoadId") Is Nothing Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/MPPW.aspx"))
        End If
        ' Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'DO1.Value("NOTES") = t.Translate("Cancel All Payloads, Are you sure?")
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        ' DO1.AddLabelLine("NOTES")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        DO1.AddLabelLine("WARNING", "WARNING", t.Translate("Cancel All Payloads, Are you sure?"))
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "yes"
                doCancelAll()
            Case "no"
                doBack()
        End Select
    End Sub

    Private Sub doCancelAll()
        Dim list As New List(Of String)
        Session.Remove("PayLoadId")
        Session("PayLoadId") = list
        Session.Remove("SKUMULTISEL_LOADID")
        DO1.Value("LOADID") = Nothing
        DO1.Value("Numberofpayloads") = Nothing
        DO1.Value("LastPayload") = Nothing
        list.Clear()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/MPPW.aspx"))
    End Sub
    Private Sub doBack()
        Dim list As List(Of String) = Session("PayLoadId")
        Dim SequenceList As List(Of String) = Session("Sequence")
        'Session.Remove("PayLoadId")
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/MPPW.aspx"))
    End Sub
End Class