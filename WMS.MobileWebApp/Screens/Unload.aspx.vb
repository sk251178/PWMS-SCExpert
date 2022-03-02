Imports Made4Net.DataAccess
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web

Partial Public Class Unload
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub doNext()


        Dim ship As String = DO1.Value("SHIPMENT")
        Dim door As String = DO1.Value("DOOR")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim shipObj As WMS.Logic.Shipment
        If Not String.IsNullOrEmpty(ship) Then
            If Not WMS.Logic.Shipment.Exists(ship) Then
                DO1.Value("SHIPMENT") = ""
                MessageQue.Enqueue(t.Translate("Shipment does not exist"))
                Return
            End If
            shipObj = New WMS.Logic.Shipment(ship)
            If Not shipObj.STATUS.Equals(WMS.Lib.Statuses.Shipment.LOADED, StringComparison.OrdinalIgnoreCase) AndAlso _
            Not shipObj.STATUS.Equals(WMS.Lib.Statuses.Shipment.LOADING, StringComparison.OrdinalIgnoreCase) Then
                DO1.Value("SHIPMENT") = ""
                MessageQue.Enqueue(t.Translate("Incorrect shipment status"))
                Return
            End If

        ElseIf Not String.IsNullOrEmpty(door) Then
            Dim sql As String = String.Format("select * from shipment where door={0} and status in ({1},{2})", _
            Made4Net.Shared.FormatField(door), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Shipment.LOADED), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Shipment.LOADING))
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                DO1.Value("DOOR") = ""
                MessageQue.Enqueue(t.Translate("Door does not have any shipment with loaded loads"))
                Return
            ElseIf dt.Rows.Count > 1 Then
                MessageQue.Enqueue(t.Translate("Door has more than one shipment with loaded loads, please scan shipment"))
                Return
            Else
                shipObj = New WMS.Logic.Shipment(dt.Rows(0)("SHIPMENT"))
            End If

        Else
            MessageQue.Enqueue(t.Translate("Can not leave both fields empty"))
            Return
        End If

        If shipObj Is Nothing Then
            Return
        End If

        Session("unldShipment") = shipObj
        Response.Redirect(MapVirtualPath("Screens/Unload1.aspx"))


    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("SHIPMENT")
        DO1.AddTextboxLine("DOOR")
    End Sub
End Class