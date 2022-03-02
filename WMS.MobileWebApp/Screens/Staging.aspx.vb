Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

Partial Public Class Staging
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONTAINERID")
        DO1.AddTextboxLine("LOADID")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oStaging As New WMS.Logic.Staging
            Dim Sql As String = String.Format("select ol.consignee, ol.orderid, ol.loadid, isnull(inv.handlingunit,'') as handlingunit from invload inv inner join orderloads ol on ol.loadid = inv.loadid where ol.loadid like '%{0}' and isnull(inv.handlingunit,'') like '%{1}'", DO1.Value("LOADID"), DO1.Value("CONTAINERID"))
            Dim dt As New DataTable
            DataInterface.FillDataset(Sql, dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("handlingunit") = "" Then 'Create Load delivery
                    oStaging.StageLoad(dt.Rows(0)("loadid"), WMS.Logic.GetCurrentUser)
                Else
                    oStaging.StageContainer(dt.Rows(0)("handlingunit"), WMS.Logic.GetCurrentUser)
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No Loads / Containers found"))
                DO1.Value("CONTAINERID") = ""
                DO1.Value("LOADID") = ""
                Return
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
        Session("printed") = 1
        Response.Redirect(MapVirtualPath("Screens/Del.aspx?sourcescreen=Staging"))        
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

End Class
