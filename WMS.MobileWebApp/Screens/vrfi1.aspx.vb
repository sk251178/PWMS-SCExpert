Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess

Imports Wms.Logic

Partial Public Class vrfi1
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack() Then
            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("CONSIGNEE")
            dd.AllOption = False
            dd.TableName = "CONSIGNEE"
            dd.ValueField = "CONSIGNEE"
            dd.TextField = "CONSIGNEENAME"
            dd.DataBind()

            DO1.SetFocusElement("SKU")
            If Not String.IsNullOrEmpty(Session("VRFISKU")) Then
                DO1.Value("SKU") = Session("VRFISKU").ToString
                'Session.Remove("VRFISKU")
            End If
            If Not String.IsNullOrEmpty(Session("VRFICONSIGNEE")) Then
                dd.SelectedValue = Session("VRFICONSIGNEE").ToString
                ' Session.Remove("VRFICONSIGNEE")
            End If
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "check"
                'addToTotalQtyPerNextClick()
                'ManageMutliUOMUnits.AddUOMUnits("", 0, "")
                'ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                'doNext()
                doCheck()
            Case "back"
                ' doMenu()
                Session.Remove("VRFISKU")
                Session.Remove("VRFICONSIGNEE")
                Session.Remove("objMultiUOMUnits")
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))
        End Select
    End Sub

    Private Sub doCheck()
        'If (MobileUtils.IsFinishVerification()) Then
        '    MobileUtils.doDoneVerification()
        'Else
        loadContainerDictionary()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI3.aspx?sourcescreen=VRFI1"))


        'End If
    End Sub

    Private Sub addToTotalQtyPerNextClick()
        Dim d, t As Decimal
        Try
            d = Convert.ToDecimal(0)
            If d <= 0 Then Exit Sub
            d = 0 ' MobileUtils.ConvertToUnits(Session("VRFICONSIGNEE").ToString, Session("VRFISKU").ToString, DO1.Value("UOM"), d)
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


    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'DO1.AddTextboxLine("LoadID")
        'DO1.AddTextboxLine("ContainerID")
        'DO1.AddTextboxLine("LABELID")
        DO1.AddDropDown("CONSIGNEE", Session("3PL"))
        DO1.AddTextboxLine("SKU")
        DO1.DefaultButton = "Next"
        DO1.FocusField = "SKU"
    End Sub

    Private Sub doNext()

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not WMS.Logic.SKU.Exists(DO1.Value("CONSIGNEE"), DO1.Value("SKU").Trim()) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("SKU does not exist"))
            Return
        End If

        Dim sql As String = String.Format("SELECT COUNT(DISTINCT vSC.SKU + vSC.Consignee) FROM vSKUCODE vSC INNER JOIN loads ld ON vSC.CONSIGNEE=ld.CONSIGNEE " & _
        "AND vSC.SKU=ld.SKU WHERE (SKUCODE LIKE '{0}' OR vSC.SKU LIKE '{0}') and vSC.Consignee like '%{1}'", DO1.Value("SKU").Trim(), DO1.Value("CONSIGNEE"))
        'If Not Session("VRFICONT") Is Nothing Then
        '    sql += String.Format(" AND ld.loadid in (select loadid from INVLOAD where HANDLINGUNIT='{0}')", CType(Session("VRFICONT"), WMS.Logic.Container).ContainerId)
        'End If
        'If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN loads ld ON vSC.CONSIGNEE=ld.CONSIGNEE AND vSC.SKU=ld.SKU WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "') and vSC.Consignee like '" & DO1.Value("CONSIGNEE") & "%' and ld.Loadid in (select loadid from containerloads where containerid=") > 1 Then
        If DataInterface.ExecuteScalar(sql) > 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("More than one sku exists for selected sku. Please choose a consignee."))

            Return
        ElseIf DataInterface.ExecuteScalar(sql) = 1 Then
            Dim dt As New DataTable()

            sql = String.Format("SELECT vSC.SKU,vSC.Consignee FROM vSKUCODE vSC INNER JOIN loads ld ON vSC.CONSIGNEE=ld.CONSIGNEE " & _
            "AND vSC.SKU=ld.SKU WHERE (SKUCODE LIKE '{0}' OR vSC.SKU LIKE '{0}') and vSC.Consignee like '%{1}'", DO1.Value("SKU").Trim(), DO1.Value("CONSIGNEE"))
            'If Not Session("VRFICONT") Is Nothing Then
            '    sql += String.Format(" AND ld.loadid in (select loadid from INVLOAD where HANDLINGUNIT='{0}')", CType(Session("VRFICONT"), WMS.Logic.Container).ContainerId)
            'End If

            'DataInterface.FillDataset("SELECT vSC.SKU,vSC.Consignee FROM FROM vSKUCODE vSC INNER JOIN loads ld ON vSC.CONSIGNEE=ld.CONSIGNEE AND vSC.SKU=ld.SKU WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')", dt)
            DataInterface.FillDataset(sql, dt)
            Session("VRFISKU") = dt.Rows(0)("SKU")
            Session("VRFICONSIGNEE") = dt.Rows(0)("CONSIGNEE")


            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI2.aspx"))

        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No SKU was found."))
            DO1.Value("SKU") = ""
            Return
        End If
    End Sub


    Private Sub loadContainerDictionary()

        If IsNothing(Session("VRFIDICT")) Then
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
                'End Added for RWMS-558
            Else
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))
            End If
            Session("VRFIDICT") = dict
        End If

    End Sub



    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Function ValidateContainerOrderStatus(ByVal pContId As String) As Boolean
        Dim Sql As String = String.Format("select distinct oh.STATUS from INVLOAD il inner join ORDERLOADS ol on ol.LOADID = il.LOADID inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = ol.CONSIGNEE and oh.ORDERID = ol.ORDERID where il.HANDLINGUNIT = '{0}'", pContId)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count > 1 Or dt.Rows.Count = 0 Then
            'More than one order found / No orders were found -> not valid
            Return False
        Else
            Dim status As String = dt.Rows(0)("STATUS")
            If Not String.Equals(status, WMS.Lib.Statuses.OutboundOrderHeader.STAGED, StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Function ValidateLoadOrderStatus(ByVal pLoadid As String) As Boolean
        Dim Sql As String = String.Format("select distinct oh.STATUS from ORDERLOADS ol inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = ol.CONSIGNEE and oh.ORDERID = ol.ORDERID where ol.LOADID = '{0}'", pLoadid)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count > 1 Or dt.Rows.Count = 0 Then
            'More than one order found / No orders were found -> not valid
            Return False
        Else
            Dim status As String = dt.Rows(0)("STATUS")
            If Not String.Equals(status, WMS.Lib.Statuses.OutboundOrderHeader.STAGED, StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
        End If
        Return True
    End Function

End Class