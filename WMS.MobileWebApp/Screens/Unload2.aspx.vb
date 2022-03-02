Imports Made4Net.DataAccess
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web

Partial Public Class Unload2
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
                doBack(True)
            End If
            Dim shipObj As WMS.Logic.Shipment = Session("unldShipment")
            DO1.Value("SHIPMENT") = shipObj.SHIPMENT
            If Not String.IsNullOrEmpty(Session("unldCont")) Then
                DO1.setVisibility("CONTAINERID", True)
                DO1.setVisibility("LOADID", False)
                DO1.Value("CONTAINERID") = Session("unldCont")
                DO1.Value("LOADID") = ""
            ElseIf Not String.IsNullOrEmpty(Session("unldLoad")) Then
                DO1.setVisibility("CONTAINERID", False)
                DO1.setVisibility("LOADID", True)
                DO1.Value("LOADID") = Session("unldLoad")
                DO1.Value("CONTAINERID") = ""
            Else
                doBack(False)
            End If


        End If

    End Sub

    Private Sub doNext()
        Dim loc As String = DO1.Value("LOCATION")
        Dim wharea As String = WMS.Logic.Warehouse.getUserWarehouseArea

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim shipObj As WMS.Logic.Shipment = Session("unldShipment")

        If Not WMS.Logic.Location.Exists(loc, wharea) Then
            DO1.Value("LOCATION") = ""
            MessageQue.Enqueue(t.Translate("Location does note exist"))
            Return
        End If

        If shipObj.DOOR.Equals(loc, StringComparison.OrdinalIgnoreCase) Then
            DO1.Value("LOCATION") = ""
            MessageQue.Enqueue(t.Translate("Can not unload to the current location"))
            Return
        End If

        Try
            If Not String.IsNullOrEmpty(DO1.Value("CONTAINERID")) Then
                shipObj.UnloadContainer(DO1.Value("CONTAINERID"), loc, wharea, WMS.Logic.GetCurrentUser)
            ElseIf Not String.IsNullOrEmpty(DO1.Value("LOADID")) Then
                shipObj.UnLoad(DO1.Value("LOADID"), loc, wharea, WMS.Logic.GetCurrentUser)
            End If

            If shipObj.STATUS.Equals(WMS.Lib.Statuses.Shipment.ATDOCK, StringComparison.OrdinalIgnoreCase) Then
                MessageQue.Enqueue(t.Translate("Shipment fully unloaded"))
                doBack(True)
            Else
                doBack(False)
            End If
        Catch ex As Made4Net.Shared.M4NException
            MessageQue.Enqueue(t.Translate(ex.Message))
            Return
        Catch ex As Exception
            MessageQue.Enqueue(t.Translate(ex.ToString()))
            Return
        End Try


    End Sub

    Private Sub doBack(ByVal pScanShipment As Boolean)
        Session.Remove("unldLoad")
        Session.Remove("unldCont")
        If pScanShipment Then
            Session.Remove("unldShipment")
            Response.Redirect(MapVirtualPath("Screens/Unload.aspx"))
        Else
            Response.Redirect(MapVirtualPath("Screens/Unload1.aspx"))
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack(False)
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("SHIPMENT")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddSpacer()
        DO1.AddTextboxLine("LOCATION")
    End Sub

End Class