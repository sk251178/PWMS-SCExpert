Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class PRNLD
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session.Remove("LoadCNTLoadId")
        End If

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
        Session.Remove("LoadCNTLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("LOCATION")
        DO1.AddTextboxLine("WAREHOUSEAREA")
        DO1.AddTextboxLine("CONSIGNEE")
        DO1.AddTextboxLine("SKU")
        DO1.AddSpacer()
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then

        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("SKU").Trim <> "" Then
            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "'  AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "%' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "%')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "PRNLD"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                Session("SKUSEL_WAREHOUSEAREA") = DO1.Value("WAREHOUSEAREA").Trim

                Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx"))
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & DO1.Value("LOCATION").Trim & "' AND RD.WAREHOUSEAREA='" & DO1.Value("WAREHOUSEAREA").Trim & "'  WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "%'")
            End If
        End If

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim dt As New DataTable
        Dim sql As String
        Dim LoadId, Loc, Warehousearea, Cons, Sku As String
        LoadId = DO1.Value("LOADID")
        Loc = DO1.Value("LOCATION")
        Warehousearea = DO1.Value("WAREHOUSEAREA")
        Cons = DO1.Value("CONSIGNEE")
        Sku = DO1.Value("SKU")

        'sql = String.Format("SELECT LOADID FROM LOADS WHERE LOADID LIKE '%{0}' and location like '%{1}' and consignee like '{2}%' and sku like '{3}%'", LoadId, Loc, Cons, Sku)
        Select Case LoadFindType(LoadId, Loc, Warehousearea, Cons, Sku)
            Case LoadSearchType.ByLoad
                sql = String.Format("SELECT LOADID FROM LOADS WHERE LOADID LIKE '{0}'", LoadId.Trim())
            Case LoadSearchType.ByLocationAndSku
                sql = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE LOADS.LOCATION LIKE '{0}%' and LOADS.WAREHOUSEAREA LIKE '{2}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", Loc.Trim(), Sku.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocationAndConsigneeAndSku
                sql = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE LOADS.LOCATION LIKE '{0}%' and LOADS.WAREHOUSEAREA LIKE '{3}%'  AND SKU.CONSIGNEE LIKE '{1}%' AND (SKU.SKU like '{2}%' or SKU.MANUFACTURERSKU like '{2}%' or SKU.VENDORSKU ='{2}' or SKU.OTHERSKU ='{2}')", Loc.Trim(), Cons.Trim(), Sku.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocation
                sql = String.Format("SELECT LOADID FROM LOADS WHERE LOCATION LIKE '{0}%' and WAREHOUSEAREA LIKE '{1}%'", Loc.Trim(), Warehousearea.Trim())
            Case LoadSearchType.BySku
                sql = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE SKU.CONSIGNEE LIKE '{0}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", Cons.Trim(), Sku.Trim())
        End Select

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("More than 1 load in location, enter loadid"))
            Return
        ElseIf dt.Rows.Count = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No Load Found"))
            Return
        End If
        Dim ld As WMS.Logic.Load
        Try
            ld = New WMS.Logic.Load(dt.Rows(0)(0).ToString())
            If Not ld.isInventory Then
                Throw New ApplicationException("Load does not exist")
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage())
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        Session("LoadCNTLoadId") = ld.LOADID
        Response.Redirect(MapVirtualPath("Screens/PRNLD1.aspx"))
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

    Private Function LoadFindType(ByVal LoadId As String, ByVal Loc As String, ByVal Warehousearea As String, ByVal cons As String, ByVal sk As String) As Int32
        If LoadId <> "" Then Return LoadSearchType.ByLoad
        If Loc <> "" And Warehousearea <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndSku
        If Loc <> "" And Warehousearea <> "" And cons <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndConsigneeAndSku
        If cons <> "" And sk <> "" Then Return LoadSearchType.BySku
        If Loc <> "" And Warehousearea <> "" Then Return LoadSearchType.ByLocation
    End Function

    Public Enum LoadSearchType
        ByLoad = 1
        ByLocationAndSku = 2
        ByLocationAndConsigneeAndSku = 3
        BySku = 4
        ByLocation = 6
    End Enum

End Class
