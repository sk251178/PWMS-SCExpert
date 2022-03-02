Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

Partial Public Class PCKPickMulti
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then

            Session("PICKINGSRCSCREEN") = Request("sourcescreen")

            Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

            setScreen(pckJob)


        End If
    End Sub

    Private Sub setScreen(ByVal pPickJob As WMS.Logic.PickJob)

        DO1.Value("Picklist") = pPickJob.picklist
        DO1.Value("SKU") = pPickJob.sku
        DO1.Value("PicklistLine") = pPickJob.PickDetLines(0)
        DO1.Value("Location") = pPickJob.fromlocation
        DO1.Value("QtyToPick") = Session("PCKOldUomUnits")
        'DO1.AddLabelLine("PickedUOMUnits")
        'DO1.AddLabelLine("PickContainersCount")
        DO1.Value("PickedUOMUnits") = pPickJob.uomunits + MultiContManage.GetTotalPickedContainerUOMUnits(pPickJob)
        DO1.Value("PickContainersCount") = MultiContManage.GetPickedContainerCount + 1 'Session("PCKPicklistActiveContainerID")

        DO1.Value("UOMUnits") = Session("PCKOldUomUnits") - pPickJob.uomunits - MultiContManage.GetTotalPickedContainerUOMUnits(pPickJob) 'pPickJob.uomunits
        'DO1.Value("Container_2") = Session("PCKPicklistActiveContainerIDSecond")



    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("NOTES", "NOTES", t.Translate("Not all units were picked, place the rest in the second container"))

        DO1.AddLabelLine("PicklistLine")
        DO1.AddLabelLine("Location")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("QtyToPick")
        DO1.AddLabelLine("PickedUOMUnits")
        DO1.AddLabelLine("PickContainersCount")
        DO1.AddSpacer()

        DO1.AddTextboxLine("UOMUnits")
        DO1.AddTextboxLine("Container")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Select Case e.CommandText.ToLower
            Case "back"
                doBack()
            Case "next"
                doNext()
            Case "pickshort"
                Dim srcScreen As String = Session("PICKINGSRCSCREEN")
                Try
                    Response.Redirect(MapVirtualPath("screens/PCKPickShort.aspx?sourcescreen=" & srcScreen))

                Catch ex As System.Threading.ThreadAbortException

                End Try

        End Select

    End Sub

    Private Sub clearSession()
        Try

            Session.Remove("WeightNeededPickJob")
            Session.Remove("WeightNeededConfirm1")
            Session.Remove("WeightNeededConfirm2")
            Session.Remove("PCKOldUomUnits")

            MultiContManage.ClearPickedContainer()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub doBack()
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        'Try
        '    If srcScreen <> "" Then
        '        Session.Remove("PICKINGSRCSCREEN")
        '        Session.Remove("WeightNeededPickJob")
        '        Session.Remove("WeightNeededConfirm1")
        '        Session.Remove("WeightNeededConfirm2")
        '        Session.Remove("PCKOldUomUnits")
        '    End If
        '    ' Session.Remove("PCKPicklist")
        'Catch ex As Exception

        'End Try

        'clear all container that was added
        MultiContManage.ClearPickedContainer()
        Try
            Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub


    Private Sub doNext()
        'Session("PICKINGSRCSCREEN") = Request("sourcescreen")
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim UOMUnits_2 As Decimal = 0
        Try
            Try
                UOMUnits_2 = Math.Round(Decimal.Parse(DO1.Value("UOMUNITS")), 2)
                If UOMUnits_2 <= 0 Then Throw New Exception()
            Catch ex As Exception
                DO1.Value("UOMUNITS") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid UOMUNITS"))
                Return
            End Try
            Session("UOMUnits_2") = UOMUnits_2

            'Try
            '    UOMUnits_2 = DO1.Value("UOMUnits")
            '    Session("UOMUnits_2") = DO1.Value("UOMUnits")
            'Catch ex As Exception
            '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong UOMUnits_2"))
            '    Return
            'End Try

            'UOMUnits_2 <> 0 And
            If Not MultiContManage.ContainerContainKey(DO1.Value("CONTAINER")) Or Session("PCKPicklistActiveContainerID") = DO1.Value("CONTAINER") Then ' DO1.Value("Container_2") <> Session("PCKPicklistActiveContainerIDSecond") Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong container"))
                Return
            End If

            Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
            Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)

            Dim sumPickedQTY As Decimal = pckJob.uomunits + UOMUnits_2 + MultiContManage.GetTotalPickedContainerUOMUnits(pckJob)

            If sumPickedQTY > Session("PCKOldUomUnits") Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong UOMUnits"))
                Return
            ElseIf sumPickedQTY < Session("PCKOldUomUnits") Then 'run in loop, scan containers

                ' Session("PCKPickLineSecond") = splitPick(pckJob, osku.ConvertToUnits(pckJob.uom) * UOMUnits_2)

                'If sumPickedQTY < Session("PCKOldUomUnits") Then
                '    'goto pickshort
                '    Response.Redirect(MapVirtualPath("screens/PCKPickShort.aspx?sourcescreen=" & srcScreen))
                'End If

                'add pick to collection
                MultiContManage.SetPickedContainerUnits(DO1.Value("CONTAINER"), osku.ConvertToUnits(pckJob.uom) * UOMUnits_2)

                Response.Redirect(MapVirtualPath("screens/PCKPickMulti.aspx?sourcescreen=" & srcScreen))

            Else
                MultiContManage.SetPickedContainerUnits(DO1.Value("CONTAINER"), osku.ConvertToUnits(pckJob.uom) * UOMUnits_2)

                'if sumPickedQTY = Session("PCKOldUomUnits")
                'than, we finish to pick
                If weightNeeded(osku) Then
                    If pckJob.uomunits > 0 Then
                        'Session("WeightNeededPickJob") = PCK
                        'Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                        Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=" & srcScreen))
                    Else
                        MultiContManage.finishPartial(WMS.Logic.LogHandler.GetRDTLogger())
                        clearSession()
                        doBack()
                    End If
                Else
                    MultiContManage.finishPartial(WMS.Logic.LogHandler.GetRDTLogger())
                    clearSession()
                    doBack()
                End If
            End If
        Catch ex As System.Threading.ThreadAbortException

        End Try
    End Sub

    <CLSCompliant(False)>
    Shared Function splitPick(ByVal pckJob As WMS.Logic.PickJob, ByVal qty As Decimal) As WMS.Logic.PicklistDetail
        Dim plDet As WMS.Logic.PicklistDetail
        If pckJob.uomunits = HttpContext.Current.Session("PCKOldUomUnits") Then Return Nothing
        Dim plSplitDet As New WMS.Logic.PicklistDetail
        'If Not IsNothing(HttpContext.Current.Session("PCKPickLineSecond")) Then
        '    plSplitDet = HttpContext.Current.Session("PCKPickLineSecond")
        '    If plSplitDet.PickList = pckJob.picklist Then
        '    End If
        Dim sumPickedQTY As Decimal = pckJob.uomunits + HttpContext.Current.Session("UOMUnits_2")
        If IsNothing(HttpContext.Current.Session("PCKPickLineSecond")) Then
            If sumPickedQTY <= HttpContext.Current.Session("PCKOldUomUnits") And qty > 0 Then


                plDet = New WMS.Logic.PicklistDetail(pckJob.picklist, pckJob.PickDetLines(0))
                plSplitDet = plDet.SplitLine(qty) 'HttpContext.Current.Session("UOMUnits_2")) '

                'PCK.pickedqty = SKU.ConvertToUnits(PCK.uom) * Convert.ToDecimal(DO1.Value("UOMUNITS"))
                'plSplitDet. = HttpContext.Current.Session("UOMUnits_2")

                plSplitDet.SetContainer(HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond"), WMS.Logic.GetCurrentUser)

                plSplitDet.Post(plSplitDet.PickListLine)

                Return New WMS.Logic.PicklistDetail(pckJob.picklist, plSplitDet.PickListLine)
            Else
                Return Nothing
            End If
        Else
            Return HttpContext.Current.Session("PCKPickLineSecond")
        End If
    End Function


    Private Function weightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        If Not pSKU.SKUClass Is Nothing Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If (oAtt.Name.ToUpper = "WEIGHT" Or oAtt.Name.ToUpper = "WGT") AndAlso _
                (oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("WeightNeededPickJob")
        ' Dim Val As Object
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    'Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    'If Val = "" Then Val = Nothing
                    If (pck.oAttributeCollection.Keys(idx).ToUpper = "WEIGHT" Or pck.oAttributeCollection.Keys(idx).ToUpper = "WGT") Then
                        pck.oAttributeCollection(idx) = "0" 'DO1.Value("TOTAL") 'Val
                    End If

                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function


    Private Sub finishPartial()

        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

        Dim pcklst As New WMS.Logic.Picklist(pckJob.picklist)
        Try
            Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)

            ''do i need it
            If weightNeeded(osku) Then
                pckJob.oAttributeCollection = ExtractAttributes()
            End If


            Session("PCKPickLineSecond") = splitPick(pckJob, osku.ConvertToUnits(pckJob.uom) * Session("UOMUnits_2"))
            If Not IsNothing(Session("PCKPickLineSecond")) Then
                MobileUtils.PickRemaiderUnits(pckJob)
            End If

            If Not String.IsNullOrEmpty(Session("WeightNeededConfirm2")) Then
                pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), HttpContext.Current.Session("WeightNeededConfirm2"), pckJob.fromwarehousearea)
                pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
                'pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser, True, Session("WeightNeededConfirm2"))
            Else
                pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), "", pckJob.fromwarehousearea)
                pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                ' pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser)
            End If

            If IsNothing(pckJob) And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Session.Remove("PCKPicklist")
                Try
                    Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=PCK"))

                Catch ex As System.Threading.ThreadAbortException

                End Try

            End If

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try

    End Sub




End Class