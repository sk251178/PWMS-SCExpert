Imports WMS.Logic

Partial Public Class Loading0
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session("LoadingJobsShipmentId") = DO1.Value("Shipment")
            Session("LoadingJobsDoor") = DO1.Value("Door")
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        If e.CommandText.ToLower = "menu" Then
            doMenu()
        ElseIf e.CommandText.ToLower = "next" Then
            doNext()
        End If
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("Door")
        DO1.AddTextboxLine("Shipment")
    End Sub

    Private Sub doMenu()
        If Not Session("LoadingJob") Is Nothing Then
            Dim oLoadindTask As New LoadingTask
            oLoadindTask.ReleaseTask(Session("LoadingJob"), WMS.Logic.Common.GetCurrentUser)
        End If
        Session.Remove("LoadingJob")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not String.IsNullOrEmpty(DO1.Value("Shipment")) Then
            If Not WMS.Logic.Shipment.Exists(DO1.Value("Shipment")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Shipment does not exist"))
                Return
            End If
        ElseIf Not String.IsNullOrEmpty(DO1.Value("Door")) Then
            Dim sql As String = String.Format("select shipment from shipment where door={0} and status={1}", _
            Made4Net.Shared.FormatField(DO1.Value("Door")), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Shipment.ATDOCK))
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Door has no shipments assigned"))
                Return
            ElseIf dt.Rows.Count > 1 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Door has more than one shipment assigned"))
                Return
            Else
                DO1.Value("Shipment") = dt.Rows(0)("Shipment").ToString()
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please fill at least one of the fields"))
            Return
        End If
        Session("LoadingJobsShipmentId") = DO1.Value("Shipment")
        Session("LoadingJobsDoor") = DO1.Value("Door")
        Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/LOADING.aspx"))
    End Sub

End Class