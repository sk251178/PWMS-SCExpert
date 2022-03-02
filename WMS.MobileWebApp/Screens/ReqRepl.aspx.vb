Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
Partial Public Class ReqRepl
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
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("RelpTaskID")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim sql As String = ""

            Try
                sql = getSQLQuery()
            Catch m4nEx As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As ApplicationException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                Return
            End Try

            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No pick location found"))
            Else
                For Each dr As DataRow In dt.Rows
                    If (WMS.Logic.PickLoc.IsBatchPickLocation(dr("CONSIGNEE"), dr("LOCATION"))) Then
                        Dim message As String = String.Format("SKU {0} in Location {1} is a Batch Pick Location", dr("SKU"), dr("LOCATION"))
                        Throw New Made4Net.Shared.M4NException(New Exception(), message, message)
                    End If
                Next
                Dim oRepl As New NormalReplenish()
                For Each dr As DataRow In dt.Rows
                    oRepl.NormalReplenish(dr("LOCATION"), dr("WAREHOUSEAREA"), dr("CONSIGNEE"), dr("SKU"), "")
                Next
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Replenishment Request For Picking Locations Sent"))
            End If

            'If WMS.Logic.PickLoc.Exists(DO1.Value("CONSIGNEE"), DO1.Value("SKU"), DO1.Value("PICKLOC"), DO1.Value("PICKWAREHOUSEAREA")) Then
            'Dim oRepl As New NormalReplenish()
            '  oRepl.NormalReplenish(DO1.Value("PICKLOC"), DO1.Value("PICKWAREHOUSEAREA"), DO1.Value("CONSIGNEE"), DO1.Value("SKU"), "")
            '  HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Replenishment Request For Picking Location Sent"))
            '  Else
            '  HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Picking Location does not exist"))
            '  End If
            DO1.Value("PICKLOC") = ""
            DO1.Value("PICKWAREHOUSEAREA") = WMS.Logic.Warehouse.CurrentWarehouseArea
            DO1.Value("CONSIGNEE") = ""
            DO1.Value("SKU") = ""
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
    End Sub

    Private Function getSQLQuery() As String
        Dim sql As String = ""

        If String.IsNullOrEmpty(DO1.Value("PICKLOC")) Then
            If String.IsNullOrEmpty(DO1.Value("SKU")) Then
                Throw New Made4Net.Shared.M4NException(New Exception(), "Can not leave both location and SKU fields empty", "Can not leave both location and SKU fields empty")
            End If
            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE", Session("consigneeSession")) & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "ReqRepl"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE", Session("consigneeSession")) & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE", Session("consigneeSession")) & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')")
            Else
                Throw New Made4Net.Shared.M4NException(New Exception(), "SKU does not exist", "SKU does not exist")
            End If
        End If
        sql = String.Format("select location, warehousearea, consignee, sku from vpickloc where location like '{0}%' and " & _
        "warehousearea like '{1}%' and consignee like '{2}%' and sku like '{3}%'", DO1.Value("PICKLOC"), DO1.Value("PICKWAREHOUSEAREA"), _
         DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("SKU"))

        Return sql
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("PICKLOC")
        DO1.AddTextboxLine("PICKWAREHOUSEAREA")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU")

        DO1.setVisibility("PICKWAREHOUSEAREA", False)
        DO1.Value("PICKWAREHOUSEAREA") = WMS.Logic.Warehouse.CurrentWarehouseArea
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

End Class
