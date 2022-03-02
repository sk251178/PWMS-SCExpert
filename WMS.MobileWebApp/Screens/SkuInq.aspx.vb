Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class SkuInq
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
        Session.Remove("SKUInqConsingee")
        Session.Remove("SKUInqSKU")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        If DO1.Value("SKU").Trim <> "" Then
            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SKU) FROM vSKUCODE WHERE (SKUCODE = '" & DO1.Value("SKU") & "' or SKU = '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "SkuInq"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SKU) FROM vSKUCODE WHERE SKUCODE = '" & DO1.Value("SKU") & "'") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT SKU FROM vSKUCODE WHERE SKUCODE = '" & DO1.Value("SKU") & "'")
            End If
        End If

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(String.Format("Select top 1 * from sku where sku = '{0}' and consignee like '{1}%'", DO1.Value("Sku"), DO1.Value("Consignee", Session("consigneeSession"))), dt)
            If dt.Rows.Count = 0 Then
                Throw New ApplicationException("Sku not found")
            End If
            Session("SKUInqConsingee") = dt.Rows(0)("CONSIGNEE")
            Session("SKUInqSKU") = dt.Rows(0)("SKU")
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try
        Response.Redirect(MapVirtualPath("Screens/SKUInq2.aspx"))
        'End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("SKU")
        DO1.AddTextboxLine("Consignee", Nothing, "", False, Session("3PL"))
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
