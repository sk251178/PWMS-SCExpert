Imports System.Collections.Generic
Partial Public Class VRFICONFITEM
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If Session("VRFICONFITEMSKU") Is Nothing Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("screens/vrfi.aspx"))
            End If
            'Dim skuObj As WMS.Logic.SKU = CType(Session("VRFICONFITEMSKU"), WMS.Logic.SKU)
            Dim skuConsigneeStr As String = Session("VRFICONFITEMSKU")
            DO1.Value("CONSIGNEE") = skuConsigneeStr.Split("_")(0) 'skuObj.CONSIGNEE
            DO1.Value("SKU") = skuConsigneeStr.Split("_")(1) 'skuObj.SKU

            Dim oSku As New WMS.Logic.SKU(skuConsigneeStr.Split("_")(0), skuConsigneeStr.Split("_")(1))

            If Session("VRFICONFITEMQTYISSUE") Is Nothing Then
                DO1.Value("NOTES") = "Sku does not exist on handling unit. Please remove it"
                DO1.Value("QTY") = MobileUtils.QTYToCase(oSku, Session("VRFICONFITEMQTY"))
            Else
                Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))
                DO1.Value("QTY") = MobileUtils.QTYToCase(oSku, dict(skuConsigneeStr).VerifiedQty + Session("VRFICONFITEMQTY") - dict(skuConsigneeStr).TotalQty)
                DO1.Value("HANDLINGUNITQTY") = MobileUtils.QTYToCase(oSku, dict(skuConsigneeStr).TotalQty)
                DO1.Value("NOTES") = "Quantity entered is greater than quantity exists for sku on handling unit. Please remove it"
            End If
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doYes()
            Case "no"
                doBack()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("NOTES")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("HANDLINGUNITQTY")
        DO1.AddLabelLine("QTY")
    End Sub

    Private Sub doYes()
        Dim dict As System.Collections.Generic.Dictionary(Of String, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), System.Collections.Generic.Dictionary(Of String, MobileUtils.VerificationData))
        If Session("VRFICONFITEMQTYISSUE") Is Nothing Then
            dict.Add(Session("VRFICONFITEMSKU").ToString(), New MobileUtils.VerificationData(True, -1, DO1.Value("QTY")))
        Else
            dict(Session("VRFICONFITEMSKU").ToString()).VerifiedQty = dict(Session("VRFICONFITEMSKU").ToString()).VerifiedQty + Session("VRFICONFITEMQTY")
        End If
        'MobileUtils.VerificationData.AddToDB(Session("VRFIHUID"), (CType(Session("VRFICONFITEMSKU"), WMS.Logic.SKU)), DO1.Value("QTY"))
        MobileUtils.VerificationData.AddToDB(Session("VRFIHUID"), Session("VRFICONFITEMSKU").ToString(), dict(Session("VRFICONFITEMSKU").ToString()).VerifiedQty)
        Session("VRFIDICT") = dict

        'Session.Remove("TotalQtyPerNextClick")
        doBack()
        'ManageMutliUOMUnits.Clear(True)
        'Session.Remove("VRFISKU")
        'Session.Remove("VRFICONSIGNEE")
        'Session.Remove("objMultiUOMUnits")
        'Session.Remove("VRFICONFITEMSKU")
        'Session.Remove("VRFICONFITEMQTYISSUE")
        'Session.Remove("VRFICONFITEMQTY")
        '' Session("Recount") = 1
        '' doMenu()
        'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/vrfi2.aspx"))

    End Sub

    Private Sub doBack()
        Session.Remove("VRFICONFITEMSKU")
        Session.Remove("VRFICONFITEMQTYISSUE")
        Session.Remove("VRFICONFITEMQTY")
        Session("Recount") = 1

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI2.aspx"))
    End Sub
End Class