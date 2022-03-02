Imports Made4Net.DataAccess
Imports Made4Net.Shared.Web
Imports System.Collections.Generic
Partial Public Class VRFI2
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            'INIT SESSION FOR MULTI COUNT
            Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString, "CASE")
            clearToTotalQtyPerNextClick()

            If Session("Recount") Is Nothing Then
                'Dim dict As New System.Collections.Generic.Dictionary(Of WMS.Logic.Load, MobileUtils.VerificationData)
                Dim dict As New System.Collections.Generic.Dictionary(Of String, MobileUtils.VerificationData)
                If Not Session("VRFICONT") Is Nothing Then
                    Dim cont As WMS.Logic.Container = CType(Session("VRFICONT"), WMS.Logic.Container)

                    For i As Integer = 0 To cont.Loads.Count - 1
                        Dim skuStr As String = cont.Loads(i).CONSIGNEE & "_" & cont.Loads(i).SKU  'New WMS.Logic.SKU(cont.Loads(i).CONSIGNEE, cont.Loads(i).SKU)
                        Dim skuExists As Boolean = False
                        For Each pair As System.Collections.Generic.KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                            If pair.Key = skuStr Then 'AndAlso pair.Key.SKU = skuObj.SKU Then
                                dict(skuStr).TotalQty = dict(skuStr).TotalQty + MobileUtils.getLoadUnits(cont.Loads(i)) 'cont.Loads(i).UNITS
                                skuExists = True
                                Continue For
                            End If
                        Next
                        If Not skuExists Then
                            dict.Add(skuStr, New MobileUtils.VerificationData(True, MobileUtils.getLoadUnits(cont.Loads(i)), 0))
                        End If
                        'dict.Add(cont.Loads(i), New MobileUtils.VerificationData(True, 0))
                    Next

                    Dim QTYUNITS As Decimal = 0

                    Dim sql As String = String.Format("select * from verifiedHU where handlingunit={0}", _
                    Made4Net.Shared.FormatField(cont.ContainerId))
                    Dim dt As New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
                    For Each dr As DataRow In dt.Rows
                        Dim skuExists As Boolean = False
                        For Each pair As System.Collections.Generic.KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                            If pair.Key = dr("CONSIGNEE").ToString() & "_" & dr("SKU").ToString() Then '.CONSIGNEE = dr("CONSIGNEE") AndAlso pair.Key.SKU = dr("SKU") Then
                                pair.Value.VerifiedQty = dr("QTY")
                                skuExists = True
                                Continue For
                                'Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                                'HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Skus were previously scanned."))
                            End If
                        Next
                        If Not skuExists Then
                            dict.Add(dr("CONSIGNEE").ToString() & "_" & dr("SKU").ToString(), New MobileUtils.VerificationData(True, -1, dr("QTY")))
                        End If
                    Next
                    sql = String.Format("select top 1 qty from verifiedHU where handlingunit={0} and consignee='{1}' and sku='{2}'", _
                              Made4Net.Shared.FormatField(cont.ContainerId), Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString)
                    dt = New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
                    For Each dr As DataRow In dt.Rows
                        ManageMutliUOMUnits.AddUOMUnits("EACH", dr("QTY"), "")
                    Next
                    ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                    Session("VRFIHUID") = cont.ContainerId
                    'Commented for RWMS-558
                    'ElseIf Session("VRFILD") Is Nothing Then
                    '    Dim ld As WMS.Logic.Load = CType(Session("VRFILD"), WMS.Logic.Load)
                    '    'dict.Add(ld, New MobileUtils.VerificationData(True, 0))
                    '    Dim skuObj As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
                    '    dict.Add(ld.CONSIGNEE & "_" & ld.SKU, New MobileUtils.VerificationData(True, MobileUtils.getLoadUnits(ld), 0))
                    '    Session("VRFIHUID") = ld.LOADID
                    'End Commented for RWMS-558
                    'Added for RWMS-558
                ElseIf Not Session("VRFILD") Is Nothing Then
                    Dim ld As WMS.Logic.Load = CType(Session("VRFILD"), WMS.Logic.Load)
                    'Dim skuObj As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
                    Dim skuStr As String = ld.CONSIGNEE & "_" & ld.SKU
                    Dim skuExists As Boolean = False
                    For Each pair As System.Collections.Generic.KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                        If pair.Key = skuStr Then
                            dict(skuStr).TotalQty = dict(skuStr).TotalQty + MobileUtils.getLoadUnits(ld)
                            skuExists = True
                            Continue For
                        End If
                    Next
                    If Not skuExists Then
                        dict.Add(skuStr, New MobileUtils.VerificationData(True, MobileUtils.getLoadUnits(ld), 0))
                    End If
                    Session("VRFIHUID") = ld.LOADID
                Else
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))
                End If
                Session("VRFIDICT") = dict

            Else
                Session.Remove("RECOUNT")
            End If

            'Dim dd As Made4Net.WebControls.MobileDropDown
            'dd = DO1.Ctrl("CONSIGNEE")
            'dd.AllOption = False
            'dd.TableName = "CONSIGNEE"
            'dd.ValueField = "CONSIGNEE"
            'dd.TextField = "CONSIGNEENAME"
            'dd.DataBind()

            'DO1.SetFocusElement("SKU")



            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
            dd.AllOption = False
            dd.TableName = "SKUUOMDESC"
            dd.ValueField = "UOM"
            dd.TextField = "DESCRIPTION"
            dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString)
            dd.DataBind()
            Try
                dd.SelectedValue = "CASE"
            Catch ex As Exception
            End Try

            If Not String.IsNullOrEmpty(Session("VRFISKU")) Then
                DO1.Value("SKU") = Session("VRFISKU").ToString
            End If
            If Not String.IsNullOrEmpty(Session("VRFICONSIGNEE")) Then
                DO1.Value("CONSIGNEE") = Session("VRFICONSIGNEE").ToString
            End If


        End If
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("SKU")
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString, "CASE")
        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)

        DO1.AddTextboxLine("QTYCASE", "QTY")
        DO1.AddDropDown("UOM")

    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                If IsNothing(Session("TotalQtyPerNextClick")) And DO1.Value("QTYCASE") <> "" Then
                    addToTotalQtyPerNextClick()
                    ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("QTYCASE"), "")
                    ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                End If

                DO1.Value("QTYCASE") = ""
                doNext()
                doback()

            Case "check"
                addToTotalQtyPerNextClick()
                ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("QTYCASE"), "")
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("QTYCASE") = ""
                doNext()
                doCheck()
            Case "back"
                doback()
            Case "addunits"
                addToTotalQtyPerNextClick()
                ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("QTYCASE"), "")
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("QTYCASE") = ""
                'Case "clearunits"
                '    ManageMutliUOMUnits.Clear()
                '    ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        End Select
    End Sub

    Private Sub doback()
        ManageMutliUOMUnits.Clear(True)
        Session.Remove("VRFISKU")
        Session.Remove("VRFICONSIGNEE")
        Session.Remove("objMultiUOMUnits")
        ' doMenu()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/vrfi1.aspx"))

    End Sub

    Private Sub clearToTotalQtyPerNextClick()
        Session.Remove("TotalQtyPerNextClick")
    End Sub

    Private Sub addToTotalQtyPerNextClick()
        Dim d, t As Decimal
        Try
            d = Convert.ToDecimal(DO1.Value("QTYCASE"))
            If d <= 0 Then Exit Sub
            d = MobileUtils.ConvertToUnits(Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString, DO1.Value("UOM"), d)
            If IsNothing(Session("TotalQtyPerNextClick")) Then
                Session("TotalQtyPerNextClick") = d
            Else
                If IsNumeric(Session("TotalQtyPerNextClick")) Then
                    t = Session("TotalQtyPerNextClick")
                    Session("TotalQtyPerNextClick") = t + d
                Else
                    Session("TotalQtyPerNextClick") = d
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim qty As Decimal

        If IsNothing(Session("TotalQtyPerNextClick")) Then
            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "QTY field is mandatory")
            Return
        End If
        If Not IsNumeric(Session("TotalQtyPerNextClick")) Then
            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, "Illigal QTY")
            Return
        End If

        qty = Session("TotalQtyPerNextClick") ' MobileUtils.ConvertToUnits(Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString, DO1.Value("UOM"), Session("TotalQtyPerNextClick")) 'ManageMutliUOMUnits.GetTotalEachUnits()
        DO1.Value("QTYCASE") = qty

        Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))

        For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
            If pair.Key = DO1.Value("CONSIGNEE") & "_" & DO1.Value("SKU") Then ' pair.Key.SKU = DO1.Value("SKU") AndAlso pair.Key.CONSIGNEE = DO1.Value("CONSIGNEE") Then
                If Not pair.Value.IsRecountStarted Then
                    pair.Value.VerifiedQty = 0
                    dict(pair.Key).VerifiedQty = 0
                    pair.Value.IsRecountStarted = True
                End If
                If pair.Value.VerifiedQty + qty > pair.Value.TotalQty Then
                    Session("VRFICONFITEMSKU") = pair.Key
                    Session("VRFICONFITEMQTYISSUE") = 1
                    Session("VRFICONFITEMQTY") = qty
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFICONFITEM.aspx"))
                End If
                pair.Value.VerifiedQty += qty
                MobileUtils.VerificationData.AddToDB(Session("VRFIHUID"), pair.Key, pair.Value.VerifiedQty)
                Session("VRFIDICT") = dict
                ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Quantity added."))
                DO1.Value("QTYCASE") = ""
                'DO1.Value("SKU") = ""
                'DO1.Value("CONSIGNEE") = ""
                clearToTotalQtyPerNextClick()
                Return
            End If
        Next
        clearToTotalQtyPerNextClick()
        Session("VRFICONFITEMSKU") = DO1.Value("CONSIGNEE") & "_" & DO1.Value("SKU") 'New WMS.Logic.SKU(DO1.Value("CONSIGNEE"), DO1.Value("SKU"))
        Session("VRFICONFITEMQTY") = qty
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFICONFITEM.aspx"))
        ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No load with entered sku exists."))
        'DO1.Value("QTYCASE") = ""
        'DO1.Value("SKU") = ""
        'DO1.Value("CONSIGNEE") = ""
    End Sub

    Private Sub editQty(ByVal pHandlingUnit As String, ByVal pSKU As WMS.Logic.SKU, ByVal pQty As String)
        Dim sql As String = String.Format("update verifiedHU set qty={0} where handlingunit={1} and consignee={2} and sku={3}", _
        Made4Net.Shared.FormatField(pQty), Made4Net.Shared.FormatField(pHandlingUnit), _
        Made4Net.Shared.FormatField(pSKU.CONSIGNEE), Made4Net.Shared.FormatField(pSKU.SKU))

        Made4Net.DataAccess.DataInterface.RunSQL(sql)
    End Sub

    Private Sub doCheck()
        If (MobileUtils.IsFinishVerification()) Then
            MobileUtils.doDoneVerification()
        Else
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI3.aspx"))
        End If
    End Sub



    Private Sub doMenu()
        Session.Remove("VRFIDICT")
        Session.Remove("VRFICONT")
        Session.Remove("VRFILD")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.PreRender

    End Sub
End Class