Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic

Partial Public Class CLDGTIN
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
            If Not Session("GTINReceiptNumber") Is Nothing Then
                DO1.Value("RCN") = Session("GTINReceiptNumber")
                DO1.FocusField = "PALLETID"
            Else
                DO1.FocusField = "RCN"
            End If
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                ClearSession()
                doMenu()
            Case "next"
                doNext()
            Case "closereceipt"
                ClearSession()
                doClose()
        End Select
    End Sub

    Private Sub ClearSession()
        Session("GTINReceiptNumber") = Nothing
        Session("GTINContainer") = Nothing
    End Sub

    Private Sub doMenu()
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doClose()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("RCN") <> "" Then
            Try
                Dim rc As ReceiptHeader = New ReceiptHeader(DO1.Value("RCN"), True)
                rc.close(WMS.Logic.GetCurrentUser)
                RWMS.Logic.AppUtil.UpdateReceiptAverageWeight(DO1.Value("RCN"))
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt Closed"))
                ClearForm()
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage())
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt not found"))
        End If
    End Sub

    Private Sub ClearForm()
        DO1.Value("RCN") = ""
        DO1.Value("PALLETID") = ""
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("RCN", True, "Next")
        DO1.AddTextboxLine("PALLETID", True, "Next")
        DO1.AddSpacer()
    End Sub

    Private Sub doNext()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            If DO1.Value("RCN").Trim = "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt must not be empty"))
                Return
            End If
            If Not WMS.Logic.ReceiptHeader.Exists(DO1.Value("RCN")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Receipt not found"))
                Return
            End If
            If DO1.Value("PALLETID").Trim = "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Handling unit must not be empty"))
                Return
            ElseIf DO1.Value("PALLETID").Length >= 20 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Handling unit id must be up to 20 characters"))
                Return
            End If
            Session("GTINReceiptNumber") = DO1.Value("RCN")
            Session("GTINContainer") = DO1.Value("PALLETID")
            Response.Redirect(MapVirtualPath("Screens/CLDGTIN1.aspx"))
        Catch ex As Threading.ThreadAbortException

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

End Class