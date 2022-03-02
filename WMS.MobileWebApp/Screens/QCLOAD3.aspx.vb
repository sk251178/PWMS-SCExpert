Imports Made4Net.Mobile.WebCtrls
Imports Made4Net.Shared.Web
Imports Made4Net.Shared.Translation
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic

Partial Public Class QCLOAD3
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack Then
            DO1.Value("LOADID") = Session.Item("LoadId")
            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("STATUS")
            dd.AllOption = False
            dd.TableName = "INVSTATUSES"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.DataBind()
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "finish"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOADID")
        DO1.AddDropDown("STATUS")
        DO1.AddSpacer()
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadId")
        Session.Remove("QCCode")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        'If Me.Page.IsValid Then
        Dim trans As New Translator(Translator.CurrentLanguageID)
        Try
            Dim oLoad As New WMS.Logic.Load(Session("LoadId"), True)
            oLoad.setStatus(DO1.Value("STATUS"), "", WMS.Logic.Common.GetCurrentUser)
        Catch m4nEx As M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
        Me.Response.Redirect(MapVirtualPath("Screens/QCLOAD1.aspx", False))
        'End If
    End Sub

End Class