Imports Made4Net.Shared.Collections
Imports Made4Net.DataAccess
Imports System.Collections
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling

Partial Public Class CNTCreateLoad1
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region
    'Commented for RWMS-1834 Start
    ''Added for RWMS-320
    'Public formatDateTime As String = "MMddyy"
    ''End Added for RWMS-320
    'Commented for RWMS-1834 End
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        setAttributes()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "create"
                'RWMS-1247/RWMS-1333 Start
                'doCreate(ExtractAttributeValues())
                Dim errMsg As String
                Dim oattribute As WMS.Logic.AttributesCollection
                oattribute = ExtractAttributeValues(errMsg)
                If Not oattribute Is Nothing Then
                    doCreate(oattribute)
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
                End If
                'RWMS-1247/RWMS-1333 End
            Case "back"
                doBack()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls

    End Sub

    Private Sub doBack()
        Session("CountMissingSKUBackClicked") = 1
        Response.Redirect(MapVirtualPath("Screens/CNTCreateLoad.aspx"))
    End Sub

    Private Sub setAttributes()
        'Added for RWMS-1834 Start
        Dim formatDateTime As String = Session("RDTDateFormat")
        'Added for RWMS-1834 End
        Dim oSku As String = Session("CountMisSKUSku")
        Dim oConsignee As String = Session("CountMisSKUConsignee")
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If Not objSkuClass Is Nothing Then
            If objSkuClass.CaptureAtReceivingLoadAttributesCount > 0 Then
                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    Dim req As Boolean = False
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                        req = True
                    End If
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                        'Added for RWMS-1834 Start
                        If oAtt.Name = "MFGDATE" Or oAtt.Name = "EXPIRYDATE" Then
                            DO1.AddTextboxLine(oAtt.Name, True, "Create", oAtt.Name + " " + "(" + formatDateTime + ")")
                        Else
                            DO1.AddTextboxLine(oAtt.Name, True, "Create", oAtt.Name)
                        End If
                        'Added for RWMS-1834 End

                        'Commented for RWMS-1834 Start
                        'DO1.AddTextboxLine(oAtt.Name, oAtt.Name)
                        'Commented for RWMS-1834 End
                    End If
                Next
            Else
                doCreate(Nothing)
            End If
        Else
            doCreate(Nothing)
        End If
    End Sub

    Private Function ExtractAttributeValues(ByRef errMsg As String) As WMS.Logic.AttributesCollection 'RWMS-1247/RWMS-1333
        'Added for RWMS-1834 Start
        Dim formatDateTime As String = Session("RDTDateFormat")
        'Added for RWMS-1834 End

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim oSku As String = Session("CountMisSKUSku")
        Dim oConsignee As String = Session("CountMisSKUConsignee")
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If objSkuClass Is Nothing Then Return Nothing
        Dim oAttCol As New WMS.Logic.AttributesCollection
        For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
            Dim req As Boolean = False
            If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                Dim val As Object
                Try
                    Select Case oAtt.Type
                        Case Logic.AttributeType.Boolean
                            val = CType(DO1.Value(oAtt.Name), Boolean)
                        Case Logic.AttributeType.DateTime
                            'Commented For RWMS-604
                            'Try
                            '    val = DateTime.ParseExact(DO1.Value(oAtt.Name), Made4Net.Shared.AppConfig.DateFormat, Nothing)
                            'Catch ex As Exception
                            '    val = Nothing
                            'End Try
                            'If val Is Nothing Then
                            '    val = DateTime.ParseExact(DO1.Value(oAtt.Name), "ddMMyyyy", Nothing)
                            'End If
                            'End For RWMS-604
                            'Added For RWMS-604

                            'Commented for RWMS-1834 Start
                            'val = DateTime.ParseExact(DO1.Value(oAtt.Name), Made4Net.Shared.AppConfig.DateFormat, Nothing)
                            'Commented for RWMS-1834 End

                            val = DateTime.ParseExact(DO1.Value(oAtt.Name), formatDateTime, Nothing)
                            Made4Net.Shared.ContextSwitch.Current.Session("AttributeName") = oAtt.Name
                            Made4Net.Shared.ContextSwitch.Current.Session("MfgOrExpDate") = val
                            'End RWMS-604
                        Case Logic.AttributeType.Decimal
                            val = CType(DO1.Value(oAtt.Name), Decimal)
                        Case Logic.AttributeType.Integer
                            val = CType(DO1.Value(oAtt.Name), Int32)
                        Case Else
                            val = DO1.Value(oAtt.Name)
                    End Select
                    oAttCol.Add(oAtt.Name, val)
                Catch ex As Exception
                    'Added For RWMS-604
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                        'RWMS-1247/RWMS-1333 Start
                        ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Attribute Validation failed for " & oAtt.Name))
                        errMsg = t.Translate("Attribute Validation failed for " & oAtt.Name)
                        'RWMS-1247/RWMS-1333 End
                        Return Nothing

                    End If
                    'End RWMS-604
                End Try
            End If
        Next
        Return oAttCol
    End Function

    'added for RWMS-604 Start
    Private Function getShipDay(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(MINDAYSTOSHIP, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    Private Function getShelfLife(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(SHELFLIFE, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    Private Function getDayToReceive(ByVal Consignee As String, ByVal SKU As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(DAYSTORECEIVE, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", Consignee, SKU)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function

    'added for RWMS-604 End

    Private Sub doCreate(ByVal oAttributes As WMS.Logic.AttributesCollection)
        'Added for RWMS-1834 Start
        Dim formatDateTime As String = Session("RDTDateFormat")
        'Added for RWMS-1834 End

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try
            Dim ldId As String = Session("CountMisSKULoadId")
            Dim sConsignee As String = Session("CountMisSKUConsignee")
            Dim sSku As String = Session("CountMisSKUSku")
            'Added for RWMS-1993 
            If ldId = "" Then
                ldId = Made4Net.Shared.Util.getNextCounter("LOAD")
            End If
            'added for RWMS-604 Start   
            Dim dMFGDATEOrEXPDATE As DateTime
            Dim attributName As String
            attributName = Convert.ToString(Made4Net.Shared.ContextSwitch.Current.Session("AttributeName"))
            dMFGDATEOrEXPDATE = Convert.ToDateTime(Made4Net.Shared.ContextSwitch.Current.Session("MfgOrExpDate"))
            'Ended for RWMS-1993 

            'Expiry date validation    
            'The system will compare the payloads expiry date with the minimum ship days parameter in SKUATTRIBUTES table.   
            'If the expiry date entered is sooner than <today + minimum ship days >, the validation will fail, and an error message will return   


            'Added for RWMS-1993 
            If attributName = "EXPIRYDATE" Then
                Dim dEXPIRYDATE As Date
                dEXPIRYDATE = dMFGDATEOrEXPDATE
                If DateTime.Compare(dEXPIRYDATE, DateTime.Now) < 0 Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("EXPIRYDATE Cannot be a past date"))
                    DO1.Value("EXPIRYDATE") = ""
                    DO1.FocusField = "EXPIRYDATE"
                    Return
                End If
                'Ended for RWMS-1993 
                Dim iShipDay As Int16 = getShipDay(sConsignee, sSku)

                If DateTime.Compare(dEXPIRYDATE, DateTime.Now.AddDays(iShipDay)) < 0 Then

                    Dim msg As String = "Payload expiry date {0} is sooner than allowed to receive for this product ({1}.Valid Rule : Expirydate < ( today + mindaystoship ))"
                    'Added for RWMS-1993 
                    msg = String.Format(msg, dEXPIRYDATE.ToString(formatDateTime), DateTime.Now.AddDays(iShipDay).ToString(formatDateTime))
                    'Ended for RWMS-1993 
                    Throw New ApplicationException(t.Translate(msg))
                    Return
                End If

                '1. Mfg date <= today
                '2.if skuattribute.Mindaystoship > 0 and (mfgdate + skuattribute.shelflife) - today >= skuattribute.mindaystoship
                '3.if skuattribute.Mindaystoship > 0 or null then do not validate the mfg date other than rule 1

            ElseIf attributName = "MFGDATE" Then
                Dim dMFGDATE As Date
                dMFGDATE = dMFGDATEOrEXPDATE
                If DateTime.Compare(dMFGDATE, DateTime.Now) > 0 Then
                    Throw New ApplicationException(t.Translate("MFGDATE Cannot be a future date"))
                    Return
                End If

                Dim iShipDay As Int16 = getShipDay(sConsignee, sSku)
                Dim ishelflife As Int16 = getShelfLife(sConsignee, sSku)
                Dim isDaytoReceive As Int16 = getDayToReceive(sConsignee, sSku)
                If iShipDay > 0 Then
                    If dMFGDATE.AddDays(ishelflife).Subtract(DateTime.Now.Date).TotalDays < iShipDay Then
                        Dim msg As String = "Payload manufacture date {0} is older than allowed to receive for this product ({1}. Valid Rule: (mfgdate + shelflife) - today >= mindaystoship)"
                        'Added for RWMS-1993 
                        msg = String.Format(msg, dMFGDATE.ToString(formatDateTime), DateTime.Now.Date.AddDays(iShipDay).AddDays(-ishelflife).ToString(formatDateTime))
                        'Ended for RWMS-1993 
                        Throw New ApplicationException(t.Translate(msg))
                        Return
                    End If
                Else
                    Throw New ApplicationException(t.Translate("Mindaystoship must be greater than zero"))
                    Return
                End If
            End If
            'Added for RWMS-1993 
            'End If   
            'added for RWMS-604 End   
            Dim oCounting As New WMS.Logic.Counting()
            oCounting.CreateLoad(ldId, sConsignee, sSku, Session("CountMisSKUQty"), Session("CountMisSKUStatus"), Session("CountMisSKULocation"), Session("CountMisSKUWarehouseArea"), _
            Session("CountMisSKUUOM"), oAttributes, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger)
            Try
                Dim ld As New WMS.Logic.Load(ldId)
                RWMS.Logic.AppUtil.SetReceivedWeight(ld.CONSIGNEE, ld.SKU, ld.RECEIPT, ld.RECEIPTLINE)
            Catch ex As Exception
            End Try

            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Loads were successfully created."))
            Session.Remove("CountMisSKUConsignee")
            Session.Remove("CountMisSKULocation")
            Session.Remove("CountMisSKULoadId")
            Session.Remove("CountMisSKUQty")
            Session.Remove("CountMisSKUSku")
            Session.Remove("CountMisSKUStatus")
            Session.Remove("CountMisSKUUOM")
            'Ended for RWMS-1993 
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage())
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return
        End Try
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CNT.aspx"))
    End Sub

End Class