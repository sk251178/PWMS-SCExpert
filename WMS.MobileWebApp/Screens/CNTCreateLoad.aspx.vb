Imports Made4Net.Shared.Collections
Imports Made4Net.DataAccess
Imports System.Collections
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web

Partial Public Class CNTCreateLoad
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            DO1.Value("SKU") = Session("CNTCreateLoadSku")
            DO1.Value("LOCATION") = Session("CNTCreateLoadLocation")
            If Not IsNothing(Session("CountMisSKUWarehouseArea")) Then
                DO1.Value("WAREHOUSEAREA") = Session("CountMisSKUWarehouseArea")
            Else
                DO1.Value("WAREHOUSEAREA") = WMS.Logic.Warehouse.getUserWarehouseArea()
                'CNTCreateLoad.aspx
            End If
            setStatusDropDown()
            setConsigneeDropDown()
            SetUOM()
            
            If Session("SELECTEDSKU") <> "" Then
                DO1.Value("SKU") = Session("SELECTEDSKU")
                ' Add all controls to session for restoring them when we back from that sreen
                DO1.Value("LOADID") = Session("SKUSEL_LOADID")
                DO1.Value("LOCATION") = Session("SKUSEL_LOCATION")
                DO1.Value("STATUS") = Session("SKUSEL_STATUS")
                DO1.Value("CONSIGNEE") = Session("SELECTEDCONSIGNEE")
                DO1.Value("QTY") = Session("SKUSEL_QTY")

                Session.Remove("SKUSEL_LOADID")
                Session.Remove("SKUSEL_QTY")
                Session.Remove("SELECTEDSKU")
                Session.Remove("SELECTEDCONSIGNEE")
                Session.Remove("SKUSEL_LOCATION")
                Session.Remove("SKUSEL_STATUS")
            End If
            DO1.DefaultButton = DO1.LeftButtonText
        End If
    End Sub

    Private Sub setStatusDropDown()
        Dim dd As Made4Net.WebControls.MobileDropDown
        dd = DO1.Ctrl("STATUS")
        dd.AllOption = False
        dd.TableName = "CODELISTDETAIL"
        dd.ValueField = "CODE"
        dd.TextField = "DESCRIPTION"
        dd.Where = "CODELISTCODE = 'INVHOLDSTT'"
        dd.DataBind()
        Try
            dd.SelectedValue = WMS.Lib.Statuses.LoadStatus.AVAILABLE
        Catch ex As Exception
        End Try
    End Sub

    Private Sub setConsigneeDropDown()
        Dim dd As Made4Net.WebControls.MobileDropDown
        dd = DO1.Ctrl("CONSIGNEE")
        dd.AllOption = False
        dd.TableName = "CONSIGNEE"
        dd.ValueField = "CONSIGNEE"
        dd.TextField = "CONSIGNEENAME"
        dd.DataBind()
        Try
            dd.SelectedValue = Session("CNTCreateLoadConsignee")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetUOM()
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
        dd.AllOption = False
        dd.TableName = "SKUUOMDESC"
        dd.ValueField = "UOM"
        dd.TextField = "DESCRIPTION"
        dd.Where = String.Format("CONSIGNEE like '{0}%' and SKU like '{1}'", DO1.Value("CONSIGNEE"), DO1.Value("SKU"))
        dd.DataBind()
        Try
            Dim dfuom As String = Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT ISNULL(DEFAULTRECUOM,'') FROM SKU WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE") & "%' and SKU like '" & DO1.Value("SKU") & "'")
            If dfuom <> "" Then
                dd.SelectedValue = dfuom
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "create"
                doCreate()
            Case "back"
                doBack()
        End Select
    End Sub

    Private Sub doCreate()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("LOADID").Trim() <> "" AndAlso WMS.Logic.Load.Exists(DO1.Value("LOADID").Trim()) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Loadid already exists"))
            DO1.Value("LOADID") = ""
            DO1.FocusField = "LOADID"
            Return
        End If
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("SKU").Trim <> "" Then
            ' Check for sku
            If Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU + vSC.Consignee) FROM vSKUCODE vSC WHERE vSC.CONSIGNEE like '" & DO1.Value("CONSIGNEE") & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "CNTMISSKU"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
                Session("SKUSEL_QTY") = DO1.Value("QTY").Trim
                Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                Session("SKUSEL_STATUS") = DO1.Value("STATUS").Trim
                Session("SKUSEL_LOCATION") = DO1.Value("LOCATION").Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU + vSC.Consignee) FROM vSKUCODE vSC WHERE vSC.CONSIGNEE like'" & DO1.Value("CONSIGNEE") & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") = 1 Then
                Dim dt As New DataTable()
                DataInterface.FillDataset("SELECT vSC.SKU,vSC.CONSIGNEE FROM vSKUCODE vSC WHERE vSC.CONSIGNEE like'" & DO1.Value("CONSIGNEE") & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')", dt)
                DO1.Value("SKU") = dt.Rows(0)("SKU") ' DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC WHERE vSC.CONSIGNEE='" & Session("CreateLoadConsignee") & "' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')")
                DO1.Value("CONSIGNEE") = dt.Rows(0)("CONSIGNEE")
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No sku entered"))
            DO1.FocusField = "SKU"
            Return
        End If

        If Not WMS.Logic.Consignee.Exists(DO1.Value("Consignee").Trim()) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Consignee does not exist"))
            DO1.FocusField = "CONSIGNEE"
            Return
        End If

        If Not WMS.Logic.SKU.Exists(DO1.Value("CONSIGNEE"), DO1.Value("SKU")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("SKU does not exist"))
            DO1.Value("SKU") = ""
            DO1.FocusField = "SKU"
            Return
        End If

        Dim oCon As New WMS.Logic.Consignee(DO1.Value("CONSIGNEE"))
        If Not oCon.GENERATELOADID AndAlso DO1.Value("LOADID") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load ID for selected consignee can not be blank"))
            DO1.Value("LOADID") = ""
            DO1.FocusField = "LOADID"
            Return
        End If


        If Not WMS.Logic.Location.Exists(DO1.Value("LOCATION"), DO1.Value("WAREHOUSEAREA")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Location does not exist"))
            DO1.Value("LOCATION") = ""
            DO1.FocusField = "LOCATION"
            Return
        Else
            Dim locObj As New WMS.Logic.Location(DO1.Value("LOCATION"), DO1.Value("WAREHOUSEAREA"))
            If locObj.STATUS = False Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Location must be active"))
                DO1.Value("LOCATION") = ""
                DO1.FocusField = "LOCATION"
                Return
            End If
        End If

        Try
            Dim num As Decimal
            Decimal.TryParse(DO1.Value("QTY"), num)
            If num <= 0 Then Throw New Exception
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Quantity must be a positive number."))
            DO1.Value("QTY") = ""
            DO1.FocusField = "QTY"
            Return
        End Try

        Session("CountMisSKUConsignee") = DO1.Value("Consignee")
        Session("CountMisSKULocation") = DO1.Value("LOCATION")
        Session("CountMisSKULoadId") = DO1.Value("LOADID")
        Session("CountMisSKUQty") = DO1.Value("QTY")
        Session("CountMisSKUSku") = DO1.Value("SKU")
        Session("CountMisSKUStatus") = DO1.Value("STATUS")
        Session("CountMisSKUUOM") = DO1.Value("UOM")

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CNTCreateLoad1.aspx"))
    End Sub

    Private Sub doBack()
        Session.Remove("CNTCreateLoadConsignee")
        Session.Remove("CNTCreateLoadSku")
        Session.Remove("CNTCreateLoadLocation")

        Session.Remove("CountMisSKUConsignee")
        Session.Remove("CountMisSKULocation")
        Session.Remove("CountMisSKULoadId")
        Session.Remove("CountMisSKUQty")
        Session.Remove("CountMisSKUSku")
        Session.Remove("CountMisSKUStatus")
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CNT.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddDropDown("CONSIGNEE")
        DO1.AddTextboxLine("SKU")
        DO1.AddDropDown("STATUS")
        DO1.AddTextboxLine("LOCATION")
        DO1.AddTextboxLine("WAREHOUSEAREA")
        DO1.AddTextboxLine("QTY")
        DO1.AddDropDown("UOM")
        DO1.AddSpacer()
    End Sub

End Class