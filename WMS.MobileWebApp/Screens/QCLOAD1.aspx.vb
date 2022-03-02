Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic

Partial Public Class QCLOAD1
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

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                Me.doNext()
                Exit Select
            Case "menu"
                Me.doMenu()
                Exit Select
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID", True, "next")
        DO1.AddSpacer()
    End Sub

    Private Sub doMenu()
        Me.Session.Remove("LoadId")
        Me.Session.Remove("QCCode")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        'If Me.Page.IsValid Then
        Dim trans As New Made4Net.Shared.Translation.Translator(Translation.Translator.CurrentLanguageID)
        Try
            If WMS.Logic.Load.Exists(DO1.Value("LOADID")) Then
                Dim ld As New Load(DO1.Value("LOADID"), True)
                Dim dt As New DataTable
                DataInterface.FillDataset(String.Concat(New String() {"SELECT ISNULL(HAZCLASS,'') AS CODE FROM SKU INNER JOIN QCHEADER ON HAZCLASS=QCCODE WHERE CONSIGNEE='", ld.CONSIGNEE, "' AND SKU='", ld.SKU, "'"}), dt, False, Nothing)
                If (dt.Rows.Count = 1) Then
                    Me.Session.Item("QCCode") = Convert.ToString(dt.Rows.Item(0).Item("CODE"))
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("QC not defined for this SKU"))
                    Return
                End If
                Me.Session.Item("LoadId") = DO1.Value("LOADID")
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load does not exist", Nothing))
                Return
            End If
        Catch exception1 As M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception1.GetErrMessage(Translation.Translator.CurrentLanguageID))
            Return
        Catch exception2 As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception2.Message)
            Return
        End Try
        Me.Response.Redirect(MapVirtualPath("Screens/QCLOAD2.aspx", False))
        'End If
    End Sub


End Class