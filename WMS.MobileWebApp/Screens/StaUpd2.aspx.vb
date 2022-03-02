Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Partial Public Class StaUpd2
    Inherits PWMSRDTBase

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim ld As New WMS.Logic.Load(Convert.ToString(Session("LoadStatusUpdateLoadId")))
            DO1.Value("SKU") = ld.SKU
            Dim oSku As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)

            Try
                DO1.Value("SKUCLASS") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select CLASSDESCRIPTION from SKUCLS where CLASSNAME = '{0}'", oSku.SKUClassName)) 'oSku.SKUClassName
            Catch ex As Exception
                DO1.setVisibility("SKUCLASS", False)
            End Try

            DO1.Value("UNITS") = ld.UOMUnits
            DO1.Value("LOCATION") = ld.LOCATION
            DO1.Value("LOADID") = ld.LOADID
            Try
                DO1.Value("SKUDESC") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where sku = '{0}' and consignee = '{1}'", ld.SKU, ld.CONSIGNEE))
                'DO1.Value("TOQTY") = ld.UNITS
            Catch ex As Exception
            End Try

            Dim attVal As String
            If Not (IsAttributeNeeded(ld, oSku, "EXPIRYDATE", attVal)) Or attVal = "" Then
                DO1.setVisibility("EXPIRYDATE", False)
            Else
                DO1.Value("EXPIRYDATE") = attVal
            End If

            If Not (IsAttributeNeeded(ld, oSku, "WEIGHT", attVal)) Or attVal = "" Then
                DO1.setVisibility("WEIGHT", False)
            Else
                DO1.Value("WEIGHT") = attVal
            End If



            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("Status")
            dd.AllOption = False
            dd.TableName = "invstatuses"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.Where = "Description <> 'LIMBO'"
            dd.DataBind()
            dd.SelectedValue = ld.STATUS



            Dim dd1 As Made4Net.WebControls.MobileDropDown
            dd1 = DO1.Ctrl("ReasonCode")
            dd1.AllOption = True
            dd1.TableName = "CODELISTDETAIL"
            dd1.ValueField = "CODE"
            dd1.TextField = "DESCRIPTION"
            dd1.Where = "CODELISTCODE = 'INVHOLDRC'"
            dd1.DataBind()

        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadStatusUpdateLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("LoadStatusUpdateLoadId")
        Response.Redirect(MapVirtualPath("Screens/StaUpd.aspx"))
    End Sub

    Private Sub doNext()
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If String.IsNullOrEmpty(DO1.Value("ReasonCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "ReasonCode field is mandatory.")
                Return
            End If
            Dim ld As New WMS.Logic.Load(Convert.ToString(Session("LoadStatusUpdateLoadId")))
            If ld.STATUS = DO1.Value("Status") Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Can not change load to its current status.")
                Return
            End If
            ld.setStatus(DO1.Value("Status"), DO1.Value("ReasonCode"), WMS.Logic.GetCurrentUser)
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load status update process complete."))
        Session.Remove("LoadStatusUpdateLoadId")
        Response.Redirect(MapVirtualPath("Screens/staupd.aspx"))

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
       
        '•	Expiry date (if exists)
        '•	Weight (if exists)

        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("UNITS")
        DO1.AddLabelLine("SKUCLASS")
        
        DO1.AddLabelLine("EXPIRYDATE")
        DO1.AddLabelLine("WEIGHT")
        DO1.AddSpacer()
        DO1.AddDropDown("Status")
        DO1.AddDropDown("ReasonCode")
    End Sub

    Private Function IsAttributeNeeded(ByVal ld As WMS.Logic.Load, ByVal pSKU As WMS.Logic.SKU, ByVal attName As String, ByRef attVal As String) As Boolean
        If Not IsNothing(pSKU.SKUClass) Then

            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = attName Then ' "EXPIRYDATE" or weight
                    If String.IsNullOrEmpty(ld.LoadAttributes.Attribute(oAtt.Name.ToUpper)) Then 'And oAtt.CaptureAtReceiving = WMS.Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                        Return False
                    Else
                        ' Dim ldAtt As New WMS.Logic.LoadAttribute(oAtt.Name.ToUpper)
                        Select Case oAtt.Type
                            Case Logic.AttributeType.Boolean
                                attVal = CType(ld.LoadAttributes.Attribute(oAtt.Name.ToUpper), Boolean)
                            Case Logic.AttributeType.DateTime
                                'val = CType(DO1.Value(oAtt.Name), DateTime)
                                attVal = CType(ld.LoadAttributes.Attribute(oAtt.Name.ToUpper), DateTime).ToString(Made4Net.Shared.AppConfig.DateFormat)
                            Case Logic.AttributeType.Decimal
                                attVal = CType(ld.LoadAttributes.Attribute(oAtt.Name.ToUpper), Decimal)
                            Case Logic.AttributeType.Integer
                                attVal = CType(ld.LoadAttributes.Attribute(oAtt.Name.ToUpper), Int32)
                            Case Else
                                attVal = ld.LoadAttributes.Attribute(oAtt.Name.ToUpper)
                        End Select
                        Return True
                    End If
                End If
            Next
        End If

        Return False
    End Function


    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "set status"
                doNext()
            Case ("back")
                doBack()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class