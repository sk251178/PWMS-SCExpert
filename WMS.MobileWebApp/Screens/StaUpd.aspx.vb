Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Partial Public Class StaUpd
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then


            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("WAREHOUSEAREA")
            dd.AllOption = False
            dd.TableName = "WAREHOUSEAREA"
            dd.ValueField = "WAREHOUSEAREACODE"
            dd.TextField = "WAREHOUSEAREADESCRIPTION"
            dd.DataBind()
            Try
                dd.SelectedValue = Session("LoginWHArea")
            Catch ex As Exception
            End Try

            Dim dd1 As Made4Net.WebControls.MobileDropDown
            dd1 = DO1.Ctrl("CONSIGNEE")
            dd1.AllOption = False
            dd1.TableName = "CONSIGNEE"
            dd1.ValueField = "CONSIGNEE"
            dd1.TextField = "CONSIGNEENAME"
            dd1.DataBind()
            Try
                dd1.SelectedValue = "FSA"
            Catch ex As Exception
            End Try

            If Session("SELECTEDSKU") <> "" Then
                DO1.Value("SKU") = Session("SELECTEDSKU")
                DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
                DO1.Value("LOADID") = Session("SKUSEL_LOADID")
                DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")
                DO1.Value("WAREHOUSEAREA") = Session("SKUSEL_WAREHOUSEAREA")
                ' Add all controls to session for restoring them when we back from that sreen
                Session.Remove("SELECTEDSKU")
                Session.Remove("SKUSEL_LOCATION")
                Session.Remove("SKUSEL_LOADID")
                Session.Remove("SKUSEL_CONSIGNEE")
                Session.Remove("SKUSEL_WAREHOUSEAREA")
            End If
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadStatusUpdateLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("LOCATION")
        DO1.AddTextboxLine("SKU")

        DO1.AddDropDown("CONSIGNEE")
        DO1.AddDropDown("WAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Sub CLEAR()
        DO1.Value("LOADID") = ""
        DO1.Value("LOCATION") = ""
        DO1.Value("SKU") = ""
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If DO1.Value("LOADID").Trim = "" Then
                ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
                If DO1.Value("SKU").Trim <> "" Then
                    If DO1.Value("LOCATION").Trim = "" Or Not WMS.Logic.Location.Exists(DO1.Value("LOCATION").Trim, DO1.Value("WAREHOUSEAREA").Trim) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("LOCATION does not exist"))
                        CLEAR()
                        Exit Sub
                    End If

                    ' Check for sku
                    If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                        ' Go to Sku select screen
                        Session("FROMSCREEN") = "StaUpd"
                        Session("SKUCODE") = DO1.Value("SKU").Trim
                        ' Add all controls to session for restoring them when we back from that sreen
                        Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                        Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                        Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
                        Session("SKUSEL_WAREHOUSEAREA") = DO1.Value("WAREHOUSEAREA").Trim
                        Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
                    ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                        DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "%'")
                    ElseIf Not WMS.Logic.SKU.Exists(DO1.Value("CONSIGNEE"), DO1.Value("SKU")) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("SKU does not exist"))
                        CLEAR()
                        Exit Sub
                    End If
                End If
            End If

            Dim ld As WMS.Logic.Load
            Dim oSku As WMS.Logic.SKU
           

            If DO1.Value("SKU") <> "" And DO1.Value("CONSIGNEE") <> "" Then
                If Not WMS.Logic.SKU.Exists(DO1.Value("CONSIGNEE"), DO1.Value("SKU")) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("SKU does not exist"))
                    CLEAR()
                    Exit Sub
                End If
                oSku = New WMS.Logic.SKU(DO1.Value("CONSIGNEE"), DO1.Value("SKU"))
            Else
                oSku = New WMS.Logic.SKU
            End If
            Dim SQL As String
            If DO1.Value("LOADID") <> "" Then
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid = '{0}' and location like '{1}%' and (sku.sku like '%{2}%') and invload.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE"))
            ElseIf DO1.Value("LOCATION") <> "" Then
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid like '{0}%' and location = '{1}' and (sku.sku like '%{2}%' ) and invload.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE"))
            Else
                SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid like '{0}%' and location like '{1}%' and (sku.sku like '%{2}%' ) and invload.consignee = '{3}'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE"))
            End If
            Dim dt As New DataTable
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                CLEAR()
                Throw New M4NException(New Exception, "payload not found", "payload not found")
            ElseIf dt.Rows.Count > 1 Then
                Throw New M4NException(New Exception, "more than one payload found", "more than one payload found")
            End If
            ld = New WMS.Logic.Load(Convert.ToString(dt.Rows(0)("LOADID")))
            If Not ld.isInventory Then
                CLEAR()
                Throw New ApplicationException(trans.Translate("payload does not exist"))
            End If
            If ld.hasActivity Then
                CLEAR()
                Throw New ApplicationException(trans.Translate("payload is assigned to another activity"))
            End If
            Session("LoadStatusUpdateLoadId") = ld.LOADID
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            'Return
        End Try
        Response.Redirect(MapVirtualPath("Screens/StaUpd2.aspx"))

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