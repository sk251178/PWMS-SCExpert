Imports System
Imports Made4Net.DataAccess
Imports Made4net.Shared
Imports Made4Net.Shared.Web
Imports System.Collections.Generic
Imports WMS.Logic

Public Class WeightValidator

#Region "variables"

#End Region


    Public Function WeightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        If pSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUNET" Or pSKU.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
            Return False
        End If
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

    'Added for RWMS-488(Retrofit PWMS-364) Start

    Public Function ValidateWeightNoSkuWtNeededCheck(ByVal strConsignee As String, ByVal strSku As String, ByVal Weight As Decimal, ByRef errMsg As String) As Boolean
        Dim RETVAL As Boolean = True
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Not IsNumeric(Weight) Then
            errMsg = t.Translate("Illegal weight")
            Return False
        ElseIf (Weight = 0) Then
            errMsg = t.Translate("Illegal weight")
            Return False
        End If

        'added validation code for weight
        Dim PICKOVERRIDEVALIDATOR As String = getPICKOVERRIDEVALIDATOR(strConsignee, strSku)

        If Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then

            'New Validation with expression evaluation
            Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

            vals.Add("CONSIGNEE", strConsignee)
            vals.Add("SKU", strSku)
            vals.Add("CASEWEIGHT", Weight)

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                errMsg = (t.Translate("Illegal validation function") & statement & ex.Message)
                Return False
            End Try

            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                RETVAL = False
                errMsg = returnedResponse(1)
            ElseIf returnedResponse(0) = "-1" Then
                errMsg = (ret)
                RETVAL = False
            End If
        End If
        Return RETVAL

    End Function

    'End Added for RWMS-488(Retrofit PWMS-364)

   

    Public Function ValidateWeightSku(ByVal oSku As WMS.Logic.SKU, ByRef Weight As String, ByVal Units As Integer, ByRef gotoOverride As Boolean, ByRef gotoOverrideMessage As String, ByRef errMsg As String, Optional ByVal IsFromRDT As Boolean = True) As Boolean
        Dim ret As Boolean = True

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'Dim currentWeight As Decimal = 0

        If WeightNeeded(oSku) Then
           
            If IsFromRDT Then
                Dim gs As Barcode128GS.GS128

                gs = New Barcode128GS.GS128()

                Dim err = "", barcode, retBarCode As String

                barcode = Weight

                retBarCode = gs.getWeight(barcode, err)
                If retBarCode = 1 Then
                    Weight = barcode
                Else
                    ' DO1.Value("WEIGHT") = ""
                    errMsg = retBarCode & " " & err
                    Return False
                End If

            ElseIf Not IsNumeric(Weight) Then
                errMsg = t.Translate("Illegal weight")
                Return False
                Exit Function
            ElseIf (Weight = 0) Then
                errMsg = t.Translate("Illegal weight")
                Return False
                Exit Function
            End If


            'added validation code for weight
            If Not ValidateWeight(oSku, Weight, Units, gotoOverride, gotoOverrideMessage, errMsg) Then
                'VALIDATION NOT PASSED; REDIRECTED TO OVERIDE SCREEN                 
                Return False
            End If

        End If

        Return ret

    End Function

    Public Function ValidateWeight(ByVal oSku As WMS.Logic.SKU, ByVal currentWeight As Decimal, ByVal Units As Integer, ByRef gotoOverride As Boolean, ByRef gotoOverrideMessage As String, ByRef errMsg As String) As Boolean
        Dim RETVAL As Boolean = True
        gotoOverride = False
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'added validation code for weight
        Dim PICKOVERRIDEVALIDATOR As String
        PICKOVERRIDEVALIDATOR = getPICKOVERRIDEVALIDATOR(oSku.CONSIGNEE, oSku.SKU)

        If WeightNeeded(oSku) And Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then

            'New Validation with expression evaluation
            Dim vals As New Made4Net.DataAccess.Collections.GenericCollection


            Dim dAvgWeight As Decimal = GetWeightPerCase(oSku, Units, currentWeight)

            vals.Add("CONSIGNEE", oSku.CONSIGNEE)
            vals.Add("SKU", oSku.SKU)
            vals.Add("CASEWEIGHT", dAvgWeight)

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                errMsg = (t.Translate("Illegal validation function") & statement & ex.Message)
                Return False
            End Try

            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                RETVAL = False
                ' gotoOverride(returnedResponse(1), currentWeight)
                Dim WGT As Decimal = 0, TOLPCT As Decimal = 0

                getWeightBorders(oSku, WGT, TOLPCT)

                gotoOverrideMessage = " AverageWeight = " & dAvgWeight & " WGT = " & WGT & " TOLPCT = " & TOLPCT

                errMsg = returnedResponse(1)

                gotoOverride = True

                Return False

            ElseIf returnedResponse(0) = "-1" Then
                errMsg = (ret)
                Return False
            End If
        End If
        Return RETVAL
    End Function

    
    Private Sub getWeightBorders(ByVal oSku As WMS.Logic.SKU, ByRef WGT As Decimal, ByRef TOLPCT As Decimal)


        Dim sql As String = String.Format("select isnull(WGT,0) WGT, isnull(TOLPCT,0) TOLPCT from SKUATTRIBUTE where CONSIGNEE = '{0}' and SKU = '{1}'", oSku.CONSIGNEE, oSku.SKU)

        Dim dt As New DataTable()
        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            WGT = Math.Truncate(Decimal.Parse(dt.Rows(0)("WGT").ToString()))
            TOLPCT = Math.Truncate(Decimal.Parse(dt.Rows(0)("TOLPCT").ToString()))
        Catch
            WGT = 0
            TOLPCT = 0
        End Try

    End Sub

    Public Shared Function getPICKOVERRIDEVALIDATOR(ByVal oConsignee As String, ByVal oSku As String) As String
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

    Private Function GetWeightPerCase(ByVal oSku As WMS.Logic.SKU, ByVal units As Integer, ByVal currentWeight As Decimal) As Decimal
        Dim d As Decimal = 0
        Try
            d = Math.Round(currentWeight / oSku.ConvertUnitsToUom("CASE", units), 2)
        Catch ex As Exception
            d = 0
        End Try
        Return d
    End Function

    
End Class
