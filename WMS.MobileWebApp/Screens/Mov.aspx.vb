Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class Mov
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOADID") = Session("SKUSEL_LOADID")
            DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
            DO1.Value("WAREHOUSEAREA") = Session("SKUSEL_WAREHOUSEAREA")

            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_LOADID")
            Session.Remove("SKUSEL_LOCATION")
            Session.Remove("SKUSEL_WAREHOUSEAREA")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadMoveLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU")
        DO1.AddTextboxLine("LOCATION")
        DO1.AddTextboxLine("WAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If DO1.Value("LOADID").Trim = "" Then
                ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
                If DO1.Value("SKU").Trim <> "" Then
                    ' Check for sku
                    If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                        ' Go to Sku select screen
                        Session("FROMSCREEN") = "Mov"
                        Session("SKUCODE") = DO1.Value("SKU").Trim
                        ' Add all controls to session for restoring them when we back from that sreen
                        Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                        Session("SKUSEL_WAREHOUSEAREA") = DO1.Value("WAREHOUSEAREA").Trim

                        Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                        Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                        Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
                    ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                        DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "%'")
                    End If
                End If
            End If

            Dim ld As WMS.Logic.Load
            Dim oSku As WMS.Logic.SKU
            If DO1.Value("SKU") <> "" And DO1.Value("CONSIGNEE", Session("consigneeSession")) <> "" Then
                oSku = New WMS.Logic.SKU(DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("SKU"))
            Else
                oSku = New WMS.Logic.SKU
            End If
            Dim SQL As String
            If DO1.Value("LOADID") <> "" Then
                'RWMS-2892
                'SQL = String.Format("select * from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku where loadid = '{0}' and location like '{1}%' and warehousearea like '{4}%' and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and loads.consignee like '{3}%'", _
                'DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("WAREHOUSEAREA"))

                SQL = String.Format("select loadid from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku where loadid = '{0}' and location <> '' and warehousearea like '{3}%' and (sku.sku like '%{1}%' or sku.othersku like '%{1}%') and loads.consignee like '{2}%'", _
                    DO1.Value("LOADID"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("WAREHOUSEAREA"))

                'RWMS-2892
            ElseIf DO1.Value("LOCATION") <> "" Then
                'RWMS-2892
                'SQL = String.Format("select * from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku where loadid like '{0}%' and location = '{1}' and warehousearea like '{4}%'  and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and loads.consignee like '{3}%'", _
                'DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("WAREHOUSEAREA"))

                SQL = String.Format("select loadid from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku where loadid like '{0}%' and location = '{1}' and warehousearea like '{4}%'  and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and loads.consignee like '{3}%'", _
                    DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("WAREHOUSEAREA"))

                'RWMS-2892
            Else
                'RWMS-2892
                'SQL = String.Format("select * from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku where loadid like '{0}%' and location like '{1}%' and warehousearea like '{4}%'  and (sku.sku like '%{2}%' or sku.othersku like '%{2}%') and loads.consignee = '{3}'", _
                'DO1.Value("LOADID"), DO1.Value("LOCATION"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("WAREHOUSEAREA"))

                SQL = String.Format("select loadid from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku where loadid like '{0}%' and location <> '' and warehousearea like '{3}%'  and (sku.sku like '%{1}%' or sku.othersku like '%{1}%') and loads.consignee = '{2}'", _
                    DO1.Value("LOADID"), DO1.Value("SKU"), DO1.Value("CONSIGNEE", Session("consigneeSession")), DO1.Value("WAREHOUSEAREA"))

                'RWMS-2892
            End If
            Dim dt As New DataTable
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Throw New M4NException(New Exception, "No Loads were found", "No Loads were found")
            ElseIf dt.Rows.Count > 1 Then
                Throw New M4NException(New Exception, "More than 1 load was found", "More than 1 load was found")
            End If
            ld = New WMS.Logic.Load(dt.Rows(0)("LOADID").ToString())
            If Not ld.isInventory Then
                Throw New ApplicationException(trans.Translate("Load does not exist"))
            End If
            If ld.hasActivity Then
                Throw New ApplicationException(trans.Translate("Load is assigned to another activity"))
            End If
            Session("LoadMoveLoadId") = ld.LOADID
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        Response.Redirect(MapVirtualPath("Screens/Mov2.aspx"))
        'End If
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
