Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Globalization

Partial Public Class AddQty2
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
            'INIT SESSION FOR MULTI COUNT
            Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM)

            DO1.Value("UNITS") = ld.UOMUnits

            DO1.Value("LOCATION") = ld.LOCATION
            DO1.Value("LOADID") = ld.LOADID
            Try
                DO1.Value("SKUDESC") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where sku = '{0}' and consignee = '{1}'", ld.SKU, ld.CONSIGNEE))
                ' DO1.Value("TOQTY") = oSku.ConvertUnitsToUom(ld.UOMUnits, ld.UNITS) ' ld.UNITS
            Catch ex As Exception
            End Try
            Try
                DO1.Value("INVENTORYSTATUS") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select DESCRIPTION from INVSTATUSES where CODE = '{0}'", ld.STATUS))
            Catch ex As Exception
                DO1.Value("INVENTORYSTATUS") = ld.STATUS
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


            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
            dd.AllOption = False
            dd.TableName = "SKUUOMDESC"
            dd.ValueField = "UOM"
            dd.TextField = "DESCRIPTION"
            dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", ld.CONSIGNEE, ld.SKU)
            dd.DataBind()
            Try
                dd.SelectedValue = ld.LOADUOM
            Catch ex As Exception
            End Try



            Dim dd1 As Made4Net.WebControls.MobileDropDown
            dd1 = DO1.Ctrl("ReasonCode")
            dd1.AllOption = True
            dd1.TableName = "CODELISTDETAIL"
            dd1.ValueField = "CODE"
            dd1.TextField = "DESCRIPTION"
            dd1.Where = "CODELISTCODE = 'INVADJRC'"
            dd1.DataBind()

        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadStatusUpdateLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("LoadStatusUpdateLoadId")
        Response.Redirect(MapVirtualPath("Screens/AddQty.aspx"))
    End Sub

    Private Sub doNext()

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim ld As New WMS.Logic.Load(Convert.ToString(Session("LoadStatusUpdateLoadId")))
        

           
            If String.IsNullOrEmpty(DO1.Value("ReasonCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "ReasonCode field is mandatory.")
                Return
            End If
            ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("ADJUSTQTY"), "")
            ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
            If Not IsNumeric(ManageMutliUOMUnits.GetTotalEachUnits()) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "ADJUSTQTY field is mandatory.")
                Return
            End If
            'Dim oSku As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
            Dim units As Decimal
            units = ManageMutliUOMUnits.GetTotalEachUnits ' oSku.ConvertToUnits(DO1.Value("UOM")) * Convert.ToDecimal(DO1.Value("ADJUSTQTY"))

            ld.Adjust(WMS.Lib.Actions.Audit.ADDQTY, units, DO1.Value("ReasonCode"), WMS.Logic.GetCurrentUser)

        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        DO1.Value("ADJUSTQTY") = ""
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load Adjust QTY update completed."))
        Session.Remove("LoadStatusUpdateLoadId")
        ManageMutliUOMUnits.Clear(True)
        Response.Redirect(MapVirtualPath("Screens/AddQTY.aspx"))

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls

        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("UNITS")

        DO1.AddLabelLine("INVENTORYSTATUS")
        DO1.AddLabelLine("SKUCLASS")

        DO1.AddLabelLine("EXPIRYDATE")
        DO1.AddLabelLine("WEIGHT")
        ' DO1.AddSpacer()
        Dim ld As New WMS.Logic.Load(Session("LoadStatusUpdateLoadId").ToString())
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM)
        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)

        DO1.AddTextboxLine("ADJUSTQTY", False, "add qty")

        DO1.AddDropDown("UOM")
        DO1.AddDropDown("ReasonCode")
    End Sub

    Private Function IsAttributeNeeded(ByVal ld As WMS.Logic.Load, ByVal pSKU As WMS.Logic.SKU, ByVal attName As String, ByRef attVal As String) As Boolean
        If Not IsNothing(pSKU.SKUClass) Then

            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = attName Then ' "EXPIRYDATE" or weight
                    If String.IsNullOrEmpty(ld.LoadAttributes.Attribute(oAtt.Name.ToUpper)) Then ' And oAtt.CaptureAtReceiving = WMS.Logic.SkuClassLoadAttribute.CaptureType.Capture Then
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
            Case "next"
                
                doNext()

            Case "back"
                ManageMutliUOMUnits.Clear(True)
                doBack()
            Case "menu"
                ManageMutliUOMUnits.Clear(True)
                doMenu()
            Case "addunits"
                ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("ADJUSTQTY"), "")
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("ADJUSTQTY") = ""
            Case "clearunits"
                ManageMutliUOMUnits.Clear()
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        End Select
    End Sub
End Class