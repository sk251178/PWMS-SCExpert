Imports Made4Net.DataAccess
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web

Partial Public Class Unload1
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
        If Not IsPostBack Then
            If Session("unldShipment") Is Nothing Then
                doBack()
            End If
            Dim shipObj As WMS.Logic.Shipment = Session("unldShipment")
            DO1.Value("SHIPMENT") = shipObj.SHIPMENT

        End If

    End Sub

    Private Sub doNext()
        Dim pallet As String = DO1.Value("PALLET")

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim shipObj As WMS.Logic.Shipment = Session("unldShipment")
        If String.IsNullOrEmpty(pallet) Then
            MessageQue.Enqueue(t.Translate("Can not leave the field empty"))
            Return
        End If

        If Not WMS.Logic.Container.Exists(pallet) AndAlso Not WMS.Logic.Load.Exists(pallet) Then
            DO1.Value("PALLET") = ""
            MessageQue.Enqueue(t.Translate("Pallet does not exist"))
            Return
        End If

        If WMS.Logic.Container.Exists(pallet) Then
            Dim sql As String
            Dim dt As New DataTable
            sql = String.Format("select sl.LOADID from SHIPMENTLOADS sl inner join CONTLOADS cl on sl.LOADID = cl.LOADID where shipment={0} and containerid={1}", _
            Made4Net.Shared.FormatField(shipObj.SHIPMENT), Made4Net.Shared.FormatField(pallet))

            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                Session("unldCont") = pallet
                Response.Redirect(MapVirtualPath("Screens/Unload2.aspx"))
            End If
        End If

        If WMS.Logic.Load.Exists(pallet) Then
            Dim sql As String = String.Format("select loadid from shipmentloads where shipment={0} and loadid={1}", _
            Made4Net.Shared.FormatField(shipObj.SHIPMENT), Made4Net.Shared.FormatField(pallet))
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 1 Then
                Session("unldLoad") = pallet
                Response.Redirect(MapVirtualPath("Screens/Unload2.aspx"))
            End If
        End If

        DO1.Value("PALLET") = ""
        MessageQue.Enqueue(t.Translate("Pallet does not belong to a shipment"))

    End Sub

    Private Sub doBack()
        Session.Remove("unldShipment")
        Response.Redirect(MapVirtualPath("Screens/Unload.aspx"))
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
        DO1.AddLabelLine("SHIPMENT")
        DO1.AddTextboxLine("PALLET")
    End Sub

End Class