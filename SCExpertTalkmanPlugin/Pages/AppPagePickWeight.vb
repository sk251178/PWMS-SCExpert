Imports WMS.Logic
Imports WMS.Lib
Imports Made4Net.DataAccess

Public Class AppPagePickWeight
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Y
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Try
            PrintMessageContent(oLogger)
            If Not ValidateUserLoggedIn() Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("User is not logged in. Please sign in again."))
                End If
                Me._responseCode = ResponseCode.NoUserLoggedIn
                Me._responseText = "User is not logged in"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to capture the case weight...")
            End If
            CaptureCaseWeight(_msg(0)("Device ID").FieldValue, _msg(0)("Picklist").FieldValue, _msg(0)("Picklist Line").FieldValue, _msg(0)("Picked Weight").FieldValue, oLogger)
            Me._responseCode = ResponseCode.Y
            Me._responseText = "Case weight capture successfully"
            oLogger.Write("Case weight captured successfully. Sending response code " + Me._responseCode)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("General error occured during case weight capture operation. Error details: \r\n {0}", ex.ToString))
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
    End Function

    Private Sub CaptureCaseWeight(ByVal strDeviceID As String, ByVal strPicklist As String, ByVal strPicklistLine As String, ByVal strPickedWeight As String, ByVal oLogger As WMS.Logic.LogHandler)

        Try
            Dim pcklstDetail As PicklistDetail = New PicklistDetail(strPicklist, strPicklistLine)

            Dim currentWeight As Decimal = 0

            If Decimal.TryParse(strPickedWeight, currentWeight) Then
                currentWeight = strPickedWeight
                InsertCaseWeight(strDeviceID, pcklstDetail, currentWeight, oLogger)
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("Weight is not a valid decimal ..... " + strPickedWeight)
                End If
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured in method CaptureCaseWeight ... " + ex.ToString())
            End If
        End Try

    End Sub

    Private Sub InsertCaseWeight(ByVal strDeviceID As String, ByVal pcklstDetail As PicklistDetail, ByVal currentWeight As Decimal, ByVal oLogger As LogHandler)

        Dim sql As String
        Dim user As String = GetCurrentUser()
        Try
            sql = "INSERT INTO PS_VOICEWEIGHTCAPTURE(VOICEDEVICEID,PICKLIST,PICKLISTLINE, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) VALUES('{0}','{1}',{2},'{3}',(select ISNULL(MAX(UOMNUM),0)+1 FROM PS_VOICEWEIGHTCAPTURE WHERE VOICEDEVICEID = '{0}' AND PICKLIST = '{1}' AND PICKLISTLINE = {2} AND UOM = '{3}'),{4},GETDATE(),'{5}')"
            sql = String.Format(sql, strDeviceID, pcklstDetail.PickList, pcklstDetail.PickListLine, pcklstDetail.UOM, currentWeight, user)
            oLogger.Write("SQL to capture case weight....... " + sql)
            DataInterface.ExecuteScalar(sql)
            ValidateCaseWeght(pcklstDetail, currentWeight, oLogger)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Failed to insert the values in PS_VOICEWEIGHTCAPTURE table in AppPagePickWeight page ") & ex.Message)
            End If
        End Try
    End Sub

    Private Sub ValidateCaseWeght(ByRef pck As PicklistDetail, ByVal currentWeight As Decimal, ByRef ologger As LogHandler)

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim oSku As New SKU(pck.Consignee, pck.SKU)
        Dim PICKOVERRIDEVALIDATOR As String
        PICKOVERRIDEVALIDATOR = getPICKOVERRIDEVALIDATOR(pck.Consignee, pck.SKU)
        If Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then
            Dim vals As New Collections.GenericCollection

            vals.Add("CONSIGNEE", pck.Consignee)
            vals.Add("SKU", oSku.SKU)
            vals.Add("CASEWEIGHT", currentWeight)

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String = String.Empty
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                If Not ologger Is Nothing Then
                    ologger.Write(t.Translate("Illegal validation function") & statement & ex.Message)
                End If
            End Try

            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                sendOverrideMessage(pck)
            End If
        End If

    End Sub

    Private Sub sendOverrideMessage(ByRef pck As PicklistDetail)
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WGTOVRRD)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WGTOVRRD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", pck.Consignee)
        aq.Add("DOCUMENT", pck.PickList)
        aq.Add("DOCUMENTLINE", pck.PickListLine)
        aq.Add("FROMLOAD", pck.FromLoad)
        aq.Add("FROMLOC", pck.FromLocation)
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", Session("ERROROVERRIDE"))
        aq.Add("SKU", pck.SKU)
        Dim pcklist As Picklist = New Picklist(pck.PickList)
        If String.Equals(pcklist.PickType, WMS.Lib.PICKTYPE.FULLPICK, StringComparison.InvariantCultureIgnoreCase) Then
            aq.Add("TOLOAD", pck.FromLoad)
            aq.Add("TOQTY", pck.PickedQuantity)
        Else
            aq.Add("TOCONTAINER", pck.ToContainer)
            aq.Add("TOQTY", "1")
        End If
        aq.Add("TOLOC", "")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Send(WMS.Lib.Actions.Audit.WGTOVRRD)
    End Sub

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
End Class