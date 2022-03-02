Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Partial Class LOCCONT1
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

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
        'Put user code to initialize the page here
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
            DO1.Value("LOCATION") = Session("SKUSEL_WAREHOUSEAREA")

            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_LOCATION")
            Session.Remove("SKUSEL_WAREHOUSEAREA")

            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("SKU")
        Session.Remove("CONSIGNEE")
        Session.Remove("LOCATION")
        Session.Remove("WAREHOUSEAREA")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONSIGNEE")
        DO1.AddTextboxLine("SKU")
        DO1.AddTextboxLine("LOCATION")
        'DO1.AddTextbox Line("WAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
            If DO1.Value("SKU").Trim <> "" Then
                ' Check for sku
                If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' and RD.WAREHOUSEAREA='" & WMS.Logic.Warehouse.getUserWarehouseArea() & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                    ' Go to Sku select screen
                    Session("FROMSCREEN") = "LOCCONT1"
                    Session("SKUCODE") = DO1.Value("SKU").Trim
                    ' Add all controls to session for restoring them when we back from that sreen
                    Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                    Session("SKUSEL_WAREHOUSEAREA") = WMS.Logic.Warehouse.getUserWarehouseArea()

                    Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
                    Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
                ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "'  and RD.WAREHOUSEAREA='" & Session("SKUSEL_WAREHOUSEAREA") & "'  WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                    DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "'  and RD.WAREHOUSEAREA='" & Session("SKUSEL_WAREHOUSEAREA") & "'  WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
                End If
            End If

            Session("CONSIGNEE") = DO1.Value("CONSIGNEE")
            Session("SKU") = DO1.Value("SKU")

            Dim oLoc As WMS.Logic.Location
            oLoc = New WMS.Logic.Location(DO1.Value("LOCATION"), WMS.Logic.Warehouse.getUserWarehouseArea)
            Session("LOCATION") = DO1.Value("LOCATION")
            Session("WAREHOUSEAREA") = WMS.Logic.Warehouse.getUserWarehouseArea
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        Response.Redirect(MapVirtualPath("Screens/LOCCONT2.aspx"))
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
