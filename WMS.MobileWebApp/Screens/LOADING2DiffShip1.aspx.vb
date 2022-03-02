Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports System.Collections.Generic

Partial Public Class LOADING2DiffShip1
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If IsNothing(Session("LoadingPalletDict")) Then
                Response.Redirect(MapVirtualPath("Screens/LOADING2DiffShip.aspx"))
            End If
            Dim dict As Dictionary(Of String, String) = Session("LoadingPalletDict")


            DO1.Value("PalletID") = dict.Item("PALLETID").ToString
            If dict.Item("COMPANYNAME").ToString <> "" Then
                DO1.setVisibility("CompanyName", True)
                DO1.Value("CompanyName") = dict.Item("COMPANYNAME").ToString
            Else
                DO1.setVisibility("CompanyName", False)
            End If
            DO1.Value("AssignedShipment") = dict.Item("SHIPMENT").ToString
            DO1.Value("AssignedDoor") = dict.Item("DOOR").ToString
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        If e.CommandText.ToLower = "back" Then
            Session.Remove("LoadingPalletDict")
            Response.Redirect(MapVirtualPath("Screens/LOADING2DiffShip.aspx"))
        ElseIf e.CommandText.ToLower = "next" Then
            doNext()
        End If
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        '        •	Pallet ID (as scanned)
        '•	Company name – will be displayed only if all payloads on the container belong to orders to the same target company. If the container is mixed it will be empty
        '•	Assigned shipment
        '•	Assigned door
        DO1.AddLabelLine("PalletID")
        DO1.AddLabelLine("CompanyName")
        DO1.AddLabelLine("AssignedShipment", "Assigned Shipment")
        DO1.AddLabelLine("AssignedDoor", "Assigned Door")


        DO1.AddTextboxLine("Door")
        DO1.AddTextboxLine("Shipment")
    End Sub



    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'SHIPMENT<>'' and
        If DO1.Value("Door") = "" And DO1.Value("Shipment") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please fill at least one of the fields"))
            Return
        End If

        If Not String.IsNullOrEmpty(DO1.Value("Shipment")) Then
            If DO1.Value("Shipment") = DO1.Value("AssignedShipment") Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please fill different Shipment"))
                Return
            End If
            If Not WMS.Logic.Shipment.Exists(DO1.Value("Shipment")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Shipment does not exist"))
                Return
            Else
                Dim sm As New WMS.Logic.Shipment(DO1.Value("Shipment"))
                If Not (sm.STATUS = WMS.Lib.Statuses.Shipment.ATDOCK Or sm.STATUS = WMS.Lib.Statuses.Shipment.LOADING) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Shipment is not ready for loading"))
                    Return
                End If
                'add new ship to dict session down -->
            End If
        ElseIf Not String.IsNullOrEmpty(DO1.Value("Door")) Then
            Dim sql As String = String.Format("select shipment from shipment where door={0} and status IN ({1},{2})", _
            Made4Net.Shared.FormatField(DO1.Value("Door")), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Shipment.ATDOCK), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Shipment.LOADING))
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
        'Session("LoadingJobsShipmentId") = DO1.Value("Shipment")
        'Session("LoadingJobsDoor") = DO1.Value("Door")
        If DO1.Value("Shipment") = DO1.Value("AssignedShipment") Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please fill different Shipment"))
            Return
        End If

        Dim dict As Dictionary(Of String, String) = Session("LoadingPalletDict")

        If dict.ContainsKey("NEWSHIPMENT") Then
            dict.Item("NEWSHIPMENT") = DO1.Value("Shipment")
        Else
            dict.Add("NEWSHIPMENT", DO1.Value("Shipment"))
        End If
        Session("LoadingPalletDict") = dict

        Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/LOADING2DiffShip2.aspx"))
    End Sub

End Class