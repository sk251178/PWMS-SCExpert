Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic

Imports Barcode128GS.GS128

Partial Public Class BARCODE128
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
            Case "scan"
                Me.doNext()

            Case "menu"
                Me.doMenu()

        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("Barcode")
        DO1.AddTextboxLine("Weight")
        DO1.AddLabelLine("Ret")
        DO1.AddLabelLine("Error")
        DO1.AddSpacer()
    End Sub

    Private Sub doMenu()

        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Translation.Translator.CurrentLanguageID)
        Dim sql As String
        sql = String.Format("insert into barcode128(barcode) values('{0}') ", DO1.Value("barcode"))
        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'DO1.Value("Barcode") = ""
        'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Barcode added", Nothing))
        ' Dim gs As Barcode128GS.GS128 = New Barcode128GS.GS128()
        Dim barcode As String
        DO1.Value("Error") = ""
        DO1.Value("Weight") = ""
        DO1.Value("Ret") = ""

        barcode = DO1.Value("barcode")
        'ret = gs.getWeight(barcode, err)
        'If ret = 1 Then
        DO1.Value("Weight") = barcode
        'Else
        '    DO1.Value("Error") = err
        '    DO1.Value("Ret") = ret
        'End If


    End Sub


End Class