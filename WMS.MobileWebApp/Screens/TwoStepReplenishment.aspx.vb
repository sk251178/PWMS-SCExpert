Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess

Imports Wms.Logic

Partial Public Class TwoStepReplenishment
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    <CLSCompliant(False)>
    Protected Sub DOVerify_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DOVerify.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()

        End Select
    End Sub

    Protected Sub DOVerify_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DOVerify.CreatedChildControls
        DOVerify.AddTextboxLine("Trolley")
        DOVerify.DefaultButton = "Next"
        DOVerify.FocusField = "Trolley"
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If String.IsNullOrEmpty(DOVerify.Value("Trolley")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Trolley must be filled"))
            Return
        End If

        If WMS.Logic.Location.Exists(DOVerify.Value("Trolley"), Session("LoginWHArea")) Then
            Dim ld As New WMS.Logic.Location(DOVerify.Value("Trolley"), Session("LoginWHArea"))
            If ld.LOCUSAGETYPE.ToUpper = "TROLLEY" Then
                Dim clsRepl As RWMS.Logic.TwoStepReplenishment

                If Not IsNothing(Session("CLSREPL")) Then
                    clsRepl = Session("CLSREPL")
                    clsRepl.UnAssignTaskAllRepl()
                    clsRepl.clear()
                    Session.Remove("CLSREPL")
                End If
                'clsRepl = Session("CLSREPL")
                'If clsRepl.TaskIndex > clsRepl.alistRepl.Count-1 Then
                '    clsRepl.LoadAvailable(True)
                'End If
                ' Else
                clsRepl = New RWMS.Logic.TwoStepReplenishment(DOVerify.Value("Trolley"), Session("LoginWHArea"))
                clsRepl.Load()
                'End If

                Session("CLSREPL") = clsRepl

               If clsRepl.alistRepl.Count + clsRepl.TaskDeliveryIndex = 0 Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No replenishment tasks were found for trolley"))
                    Return
                Else
                    Session("CLSREPL") = clsRepl
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment2.aspx"))
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Trolley does not exist"))
                Return
            End If
        End If


    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub


End Class