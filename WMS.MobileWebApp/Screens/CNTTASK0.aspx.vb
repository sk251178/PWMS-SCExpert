Public Partial Class CNTTASK0
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim ld As New WMS.Logic.Load(Convert.ToString(Session("TaskLoadCNTLoadId")))
            DO1.FilterExpression = String.Format("(LOADID = '{0}' )", _
                               ld.LOADID)

            'DO1.Value("LOADID") = ld.LOADID
            'DO1.Value("LOCATION") = ld.LOCATION
            Try
                If Session("UserInitiatedCounting") = True Then
                    Session.Remove("UserInitiatedCounting")
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CNTTASK1.aspx"))
                End If
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'DO1.AddLabelLine("LOADID")
        'DO1.AddLabelLine("LOCATION")
        'DO1.AddSpacer()
        'DO1.AddTextboxLine("CONFIRM", "Confirm Load")
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("LOADID").ToLower() = DO1.Value("CONFIRM").ToLower() Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CNTTASK1.aspx"))
        Else
            DO1.Value("CONFIRM") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Loadid confirmation incorrect"))
        End If
    End Sub

    Private Sub doBack()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CNTTASK.aspx"))
    End Sub
End Class