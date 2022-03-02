Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Made4Net.DataAccess
Imports System.Data

Partial Public Class WeightCaptureNeededCLD1
    Inherits PWMSRDTBase

    <CLSCompliant(False)>
    Public Shared gs As Barcode128GS.GS128

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            gs = New Barcode128GS.GS128()
            setScreen()

            If Session("CasesList") Is Nothing Then
                Dim casesList As New System.Collections.Generic.List(Of Decimal)
                Session("CasesList") = casesList
            End If
        End If
    End Sub

    Private Function twoCont() As Boolean
        If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) And Not IsNothing(Session("UOMUnits_2")) _
          And Session("UOMUnits_2") > 0 Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Sub setScreen()

        Dim osku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
        DO1.Value("SKU") = osku.SKU
        DO1.Value("Description") = osku.SKUDESC

        DO1.Value("RECEIPT") = Session("CreateLoadRCN")

        DO1.Value("Confirmed") = Session("CreateLoadUnits")

        If Session("LoadPreviewsVals") = "cld1overrideWGT" Then
            Session.Remove("LoadPreviewsVals")
        ElseIf Session("LoadPreviewsVals") = "cld1overrideWGTBACK" Then
            Session.Remove("LoadPreviewsVals")
            doDelLast()
        Else
            DO1.Value("Captured") = 0
            DO1.Value("Total") = 0
            DO1.Value("Last") = 0
        End If

        If Not Session("CasesList") Is Nothing Then

            Try
                Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")

                'Session("CasesList") = casesList
                If casesList.Count = 0 Then
                    DO1.Value("Last") = 0
                    DO1.Value("TOTAL") = 0
                    DO1.Value("CAPTURED") = 0
                Else
                    DO1.Value("Last") = Session("LastWeight") 'casesList.Item(casesList.Count - 1) '
                    DO1.Value("TOTAL") = Session("WeightNeededTOTAL")
                    DO1.Value("CAPTURED") = casesList.Count
                End If
            Catch ex As Exception

            End Try

        End If


    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("RECEIPT")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("Description")
        DO1.AddLabelLine("CONFIRMED", "Number of cases approved")
        DO1.AddLabelLine("CAPTURED", "Number of cases captured")
        DO1.AddLabelLine("TOTAL", "Total weight")
        DO1.AddLabelLine("LAST", "Last weight")
        DO1.AddSpacer()
        DO1.AddTextboxLine("WEIGHT", "capture weight of each case")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "ready"
                doFinish()
            Case "back"
                doBack()
            Case "dellast"
                doDelLast()
        End Select
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim numOfCaptured As Integer = DO1.Value("CAPTURED")
        Dim numOfConfirmed As Integer = DO1.Value("CONFIRMED")
        If numOfCaptured = numOfConfirmed Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Too many cases were captured"))
            DO1.Value("WEIGHT") = ""
            Return
        End If

        Dim currentWeight As Decimal = 0

        Dim err, barcode, ret As String
        'RWMS-1253/RWMS-1336 Start
        If String.IsNullOrEmpty(DO1.Value("WEIGHT")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Please Enter Valid Weight"))
            Return
        Else
            barcode = DO1.Value("WEIGHT")
        End If
        'barcode = DO1.Value("WEIGHT")
        'RWMS-1253/RWMS-1336 End
        ret = gs.getWeight(barcode, err)
        If ret = 1 Then
            DO1.Value("WEIGHT") = barcode
            currentWeight = barcode
            'Added for RWMS-406
            'Added for RWMS-1529 and RWMS-1059 Start
        ElseIf ret = 2 Then
            If DO1.Value("WEIGHT").Trim = "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Weight cannot be empty"))
                Return
            ElseIf Not IsNumeric(DO1.Value("WEIGHT")) Then
                DO1.Value("WEIGHT") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid Weight"))
                Return
            End If

        Else
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret & " " & err)
            Return
        End If

        If Not IsNumeric(DO1.Value("WEIGHT")) Then
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid Weight"))
            Return

            'Added for RWMS-1059 Start
        ElseIf (DO1.Value("WEIGHT") = 0) Then
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid weight"))
            Return
            'Added for  RWMS-1529 and RWMS-1059 End
            'RWMS-2587 START
        ElseIf (DO1.Value("WEIGHT") < 0) Then
            DO1.Value("WEIGHT") = ""
            MessageQue.Enqueue(t.Translate("Please enter valid weight. Weight cannot be Negative."))
            Return
            'RWMS-2587 END
        Else
            'RWMS-2633 RWMS-2631 START
            If (DO1.Value("WEIGHT") > 999999999) Then
                DO1.Value("WEIGHT") = ""
                'MessageQue.Enqueue(t.Translate("Weight must be less than 1000000000"))
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Weight must be less than 1000000000"))
                Return
            End If
            'RWMS-2633 RWMS-2631 END

            currentWeight = DO1.Value("WEIGHT")
        End If
        'Ended for RWMS-1529 and RWMS-1059 End
        If Not IsNumeric(DO1.Value("WEIGHT")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Illegal weight")
            Exit Sub
        End If
        Dim oAttributes As WMS.Logic.AttributesCollection = Session("CreateLoadAttributes")

        Dim gotoOverride As Boolean = False
        Dim gotoOverrideMessage As String = ""
        Dim errMsg As String = ""
        Dim WEIGHT As String = DO1.Value("WEIGHT")

        Dim caseUNITS As Decimal = 1
        Dim UNITS As Decimal
        Dim wgtVal As New RWMS.Logic.WeightValidator

        Dim osku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))

        UNITS = osku.ConvertToUnits(Session("CreateLoadUOM")) * caseUNITS

        If Not wgtVal.ValidateWeightSku(osku, WEIGHT, UNITS, gotoOverride, gotoOverrideMessage, errMsg, False) Then ''RWMS-1336
            If gotoOverride Then
                ' oAttributes.Item("WEIGHT") = WEIGHT
                Session("CreateLoadUnitsEach") = UNITS
                Session("WeightOverridConfirm") = WEIGHT

                ' Session("CreateLoadAttributes") = oAttributes

                Session("ERROROVERRIDE") = errMsg

                ' gotoOverrideWeight()

            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
            End If

        End If

        ''Dim gs As Barcode128GS.GS128 = New Barcode128GS.GS128()
        'Dim err, barcode, ret As String

        'barcode = DO1.Value("WEIGHT")
        'ret = gs.getWeight(barcode, err)
        'If ret = 1 Then
        '    currentWeight = barcode
        'Else
        '    DO1.Value("WEIGHT") = ""
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret & " " & err)
        '    Return
        'End If

        'Try
        '    currentWeight = Math.Round(Decimal.Parse(DO1.Value("WEIGHT")), 2)
        '    If currentWeight <= 0 Then Throw New Exception()
        'Catch ex As Exception
        '    DO1.Value("WEIGHT") = ""
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Invalid Weight"))
        '    Return
        'End Try



        'If Session("WeightNeededTOTAL") Is Nothing Then
        '    Session("WeightNeededTOTAL") = currentWeight
        'Else

        'End If


        ''added validation code for weight
        'If Not VALIDATEWEIGHT(pckJob, currentWeight) Then
        '    'VALIDATION NOT PASSED; REDIRECTED TO OVERIDE SCREEN
        '    Exit Sub
        'End If

        Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
        casesList.Add(currentWeight)

        Session("LastWeight") = currentWeight
        Session("CasesList") = casesList
        DO1.Value("Last") = currentWeight
        DO1.Value("TOTAL") = Math.Round(Decimal.Parse(DO1.Value("TOTAL")) + currentWeight, 2)

        Session("WeightNeededTOTAL") = Math.Round(Decimal.Parse(DO1.Value("TOTAL")), 2) '+ currentWeight

        oAttributes.Item("WEIGHT") = Session("WeightNeededTOTAL")
        Session("CreateLoadAttributes") = oAttributes

        DO1.Value("CAPTURED") = casesList.Count
        DO1.Value("WEIGHT") = ""

        If gotoOverride Then
            gotoOverrideWeight()
        End If

    End Sub


    Public Sub gotoOverrideWeight()

        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'cld1overrideWGT'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url) & "?sourcescreen=WCLD1")

    End Sub

    Private Sub ClearSession()
        Try
            Session.Remove("ContCount")
            Session.Remove("CasesList")
            Session.Remove("LastWeight")
            Session.Remove("AttContainerID")
            Session.Remove("WeightNeededTOTAL")
            Session.Remove("UOMUnits_2")
            'Session.Remove("PICKINGSRCSCREEN")
            '    Session.Remove("PCKPicklist")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ClearSessionWEIGHTNEEDEDCount()
        Try
            Session.Remove("ContCount")
            Session.Remove("CasesList")
            Session.Remove("LastWeight")
            Session.Remove("AttContainerID")
            Session.Remove("WeightNeededTOTAL")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub doBack()

        Try
            ClearSessionWEIGHTNEEDEDCount()
            Response.Redirect(MapVirtualPath("screens/WeightCaptureNeeded.aspx"))

            'ClearSession()

        Catch ex As System.Threading.ThreadAbortException

        End Try


    End Sub

    Private Sub goNextPick(ByVal pckJob As WMS.Logic.PickJob)

        Dim srcScreen As String
        If Not IsNothing(Session("PICKINGSRCSCREEN")) Then
            srcScreen = Session("PICKINGSRCSCREEN")
        Else
            srcScreen = "PCKPART"
        End If

        Try

            ClearSession()

        Catch ex As Exception

        End Try
        Try
            If pckJob Is Nothing Then
                Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & srcScreen))
            Else
                Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
            End If

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub
    Private Sub doFinish()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim numOfCaptured As Integer = DO1.Value("CAPTURED")
        Dim numOfConfirmed As Integer = DO1.Value("CONFIRMED")
        If numOfCaptured < numOfConfirmed Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Need to capture weight for more cases"))
            Return
        End If

        ClearSession()
        Session("LoadPreviewsVals") = "cld1overrideWGT"
        Response.Redirect(MapVirtualPath("Screens/CLD1.aspx"))

    End Sub


    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("WeightNeededPickJob")
        ' Dim Val As Object
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    'Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    'If Val = "" Then Val = Nothing
                    If (pck.oAttributeCollection.Keys(idx).ToUpper = "WEIGHT" Or pck.oAttributeCollection.Keys(idx).ToUpper = "WGT") Then
                        pck.oAttributeCollection(idx) = DO1.Value("TOTAL") 'Val
                    End If

                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function


    Private Sub doDelLast()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Session("LastWeight") <> 0 Then ' DO1.Value("Last") <> 0 Then

            Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")

            If casesList.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No weight to remove"))
                Return
            End If

            Dim lastWeight As Decimal = casesList(casesList.Count - 1)

            Try
                DO1.Value("TOTAL") = Decimal.Parse(Session("WeightNeededTOTAL")) - lastWeight

            Catch ex As Exception
                DO1.Value("TOTAL") = 0
            End Try
            Session("WeightNeededTOTAL") = DO1.Value("TOTAL")
            casesList.RemoveAt(casesList.Count - 1)
            Session("CasesList") = casesList
            DO1.Value("CAPTURED") = casesList.Count
            DO1.Value("Last") = 0 ' casesList.Item(casesList.Count - 1)
            Session("LastWeight") = 0

            DO1.Value("WEIGHT") = ""
        End If

    End Sub


    Private Function VALIDATEWEIGHT(ByRef pck As PickJob, ByVal currentWeight As Decimal) As Boolean
        Dim RETVAL As Boolean = True
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'added validation code for weight
        Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
        Dim PICKOVERRIDEVALIDATOR As String
        PICKOVERRIDEVALIDATOR = getPICKOVERRIDEVALIDATOR(pck.consingee, pck.sku)

        '  If weightNeeded(oSku) And Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then
        If Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then
            'New Validation with expression evaluation
            Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

            vals.Add("CONSIGNEE", pck.consingee)
            vals.Add("SKU", oSku.SKU)
            vals.Add("CASEWEIGHT", currentWeight) 'DO1.Value("WEIGHT"))

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal validation function") & statement & ex.Message)
                Return False
            End Try

            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                RETVAL = False
                gotoOverride(returnedResponse(1))

            ElseIf returnedResponse(0) = "-1" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret)
                Return False
            End If
        End If
        Return RETVAL
    End Function

    Private Function getPICKOVERRIDEVALIDATOR(ByVal oConsignee As String, ByVal oSku As String) As String
        Dim ret As String = String.Empty
        Dim objSku As WMS.Logic.SKU = New WMS.Logic.SKU(oConsignee, oSku)

        If Not objSku.SKUClass Is Nothing Then

            Dim objSkuClass As WMS.Logic.SkuClass = objSku.SKUClass

            Dim sql As String = String.Format("SELECT ISNULL(PICKOVERRIDEVALIDATOR, '') FROM SKUCLSLOADATT WHERE CLASSNAME = '{0}' AND ATTRIBUTENAME = 'WEIGHT'", objSkuClass.ClassName)
            Try
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
            Return ret
        End If
        Return ret
    End Function


    Private Sub gotoOverride(ByVal errorOverride As String)
        Session("ERROROVERRIDE") = errorOverride

        'Session("WeightNeededPickJob") = Session("PCKPicklistPickJob")

        Session("WeightOverridConfirm") = DO1.Value("WEIGHT")

        Try
            Response.Redirect(MapVirtualPath("screens/PCKOVERRIDEWEIGHT.aspx?sourcescreen=" & Session("PICKINGSRCSCREEN")))

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub


    'Private Sub InsertCasesWeight()

    '    Dim srcScreen As String = Session("PICKINGSRCSCREEN")
    '    Dim ret As Boolean = True

    '    Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
    '    Dim sql As String = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) values"
    '    Dim user As String = WMS.Logic.GetCurrentUser

    '    Dim sqlToLoad As String
    '    Dim dt As New DataTable
    '    Dim toload As String
    '    Dim units As Decimal
    '    Dim casesListCounter As Integer = 0
    '    Dim weight As Decimal

    '    If srcScreen.ToString().ToLower() = "pckpart" Then
    '        sqlToLoad = "select toload, units from vPartialPickComplete where fromload='{0}' and picklist='{1}' and sku='{2}'"
    '        sqlToLoad = String.Format(sqlToLoad, pckJob.fromload, pckJob.picklist, pckJob.sku)
    '    Else 'parallel
    '        sqlToLoad = "select toload, units from vParallelPickComplete where fromload='{0}' and PARALLELPICKID='{1}' and sku='{2}'"
    '        sqlToLoad = String.Format(sqlToLoad, pckJob.fromload, pckJob.parallelpicklistid, pckJob.sku)
    '    End If

    '    Made4Net.DataAccess.DataInterface.FillDataset(sqlToLoad, dt)

    '    Dim ld As WMS.Logic.Load

    '    For Each dr As DataRow In dt.Rows

    '        toload = dr("toload")
    '        ld = New Load(toload)

    '        units = ld.UOMUnits

    '        For i As Integer = 0 To units - 1
    '            If casesListCounter < casesList.Count Then
    '                weight = casesList(casesListCounter)

    '                If casesListCounter = 0 Then
    '                    sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}') "
    '                Else
    '                    sql += ", ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}') "
    '                End If
    '                sql = String.Format(sql, toload, pckJob.uom, 1, weight, user)

    '                casesListCounter = casesListCounter + 1
    '            Else
    '                Exit For
    '            End If
    '        Next

    '    Next

    '    Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

    'End Sub

End Class