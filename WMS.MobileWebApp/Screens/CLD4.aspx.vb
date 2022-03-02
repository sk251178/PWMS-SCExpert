Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

Imports WMS.Logic
<CLSCompliant(False)> Public Class CLD4
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected dt As DataTable
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dt = Session("attributes")
    End Sub

    Private Sub setAttributes()
        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If Not objSkuClass Is Nothing Then
            If objSkuClass.CaptureAtReceivingLoadAttributesCount > 0 Then
                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    Dim req As Boolean = False
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                        req = True
                    End If
                    If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                        DO1.AddTextboxLine(oAtt.Name, oAtt.Name)
                    End If
                Next
            Else
                SubmitCreate(Nothing, Session("CreateLoadDoPutAway"))
            End If
        Else
            SubmitCreate(Nothing, Session("CreateLoadDoPutAway"))
        End If
    End Sub

    Private Sub SubmitCreate(ByVal oAttributes As WMS.Logic.AttributesCollection, Optional ByVal DoPutaway As Boolean = False)
        Dim ResponseMessage As String = ""
        Dim attributesarray As New ArrayList
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        
        Try
            Dim ld() As WMS.Logic.Load
            Dim oRec As New Logic.Receiving
            If Session("CreateLoadNumLoads") Is Nothing Or Session("CreateLoadNumLoads") = "" Then Session("CreateLoadNumLoads") = 1
            Dim oReceiptLine As ReceiptDetail
            If Session("CreateLoadRCNMultipleLines") Then
                ld = oRec.CreateLoadFromMultipleLines(Session("CreateLoadRCN"), Session("CreateLoadSKU"), _
                    Session("CreateLoadUOM"), Session("CreateLoadLocation"), Session("CreateLoadWarehousearea"), Session("CreateLoadUnits"), Session("CreateLoadStatus"), _
                    Session("CreateLoadHoldRC"), Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, Session("CreateLoadLabelPrinter"), "", "")
                Try
                    RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
                Catch ex As Exception
                End Try
            Else
                oReceiptLine = New ReceiptDetail(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
                ld = oRec.CreateLoad(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"), Session("CreateLoadSKU"), Session("CreateLoadLoadId"), _
                    Session("CreateLoadUOM"), Session("CreateLoadLocation"), Session("CreateLoadWarehousearea"), Session("CreateLoadUnits"), Session("CreateLoadStatus"), _
                    Session("CreateLoadHoldRC"), Session("CreateLoadNumLoads"), Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, Session("CreateLoadLabelPrinter"), oReceiptLine.DOCUMENTTYPE, "", "")
                Try
                    RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
                Catch ex As Exception
                End Try
            End If


            ' After Load Creation we will put the loads on his container
            If Session("CreateLoadContainerID") <> "" Then
                Dim cntr As New WMS.Logic.Container(Session("CreateLoadContainerID"), True)
                Dim LoadsCreatedCount As Int32
                For LoadsCreatedCount = 0 To ld.Length() - 1
                    cntr.Place(ld(LoadsCreatedCount), WMS.Logic.Common.GetCurrentUser)
                Next
            End If

            ' If Create and Putaway then request pickup for each load
            If DoPutaway Then
                Dim pw As New Putaway
                Dim i As Int32
                For i = 0 To ld.Length() - 1
                    'ld(i).RequestPickUp(Logic.GetCurrentUser)
                    pw.RequestDestinationForLoad(ld(i).LOADID, ld(i).DESTINATIONLOCATION, ld(i).DESTINATIONWAREHOUSEAREA, 0, "") ''RWMS-1277
                Next
                Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=cld1"))
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
            'Response.Redirect(MapVirtualPath("Screens/CLD3.aspx"))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
            'Response.Redirect(MapVirtualPath("Screens/CLD3.aspx"))
        End Try
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Created"))
        Response.Redirect(MapVirtualPath("Screens/CLD1.aspx"))
    End Sub

    Private Sub doChangeLine()
        MobileUtils.ClearCreateLoadChangeLineSession()
        Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLine.aspx"))
    End Sub

    Private Sub doMenu()
        'Session.Remove("CreateLoadRCN")
        'Session.Remove("CreateLoadRCNLine")
        'Session.Remove("CreateLoadConsignee")
        'Session.Remove("CreateLoadSKU")
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "changeline"
                doChangeLine()
            Case "create"
                SubmitCreate(ExtractAttributeValues(), Session("CreateLoadDoPutAway"))
                'Case "create & putaway"
                'SubmitCreate(ExtractAttributeValues(), True)
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        setAttributes()
    End Sub

    Private Function ExtractAttributeValues() As WMS.Logic.AttributesCollection
        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
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
                            val = CType(DO1.Value(oAtt.Name), DateTime)
                        Case Logic.AttributeType.Decimal
                            val = CType(DO1.Value(oAtt.Name), Decimal)
                        Case Logic.AttributeType.Integer
                            val = CType(DO1.Value(oAtt.Name), Int32)
                        Case Else
                            val = DO1.Value(oAtt.Name)
                    End Select
                    oAttCol.Add(oAtt.Name, val)
                Catch ex As Exception

                End Try
            End If
        Next
        Return oAttCol
    End Function
End Class
