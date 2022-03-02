Public Partial Class HUStacking
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            DO1.Value("HUID") = ""
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        If e.CommandText.ToLower() = "menu" Then
            doMenu()
        ElseIf e.CommandText.ToLower() = "next" Then
            doNext()
        End If
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If WMS.Logic.Container.Exists(DO1.Value("HUID")) Then
            Session()("TopHU") = New WMS.Logic.Container(DO1.Value("HUID"), True)
            Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/HUSTACKING2.aspx"))
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Handling Unit ID does not exist."))
            Return
        End If

    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("HUID")
        DO1.AddSpacer()
    End Sub
End Class