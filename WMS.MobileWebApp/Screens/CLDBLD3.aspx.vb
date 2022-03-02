Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
<CLSCompliant(False)> Public Class CLDBLD3
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected dt As DataTable
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
                        If req Then
                            DO1.AddTextboxLine(oAtt.Name, True, "create", oAtt.Name)
                        Else
                            DO1.AddTextboxLine(oAtt.Name, oAtt.Name)
                        End If
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
            Dim oSku As WMS.Logic.SKU
            Dim rc As New WMS.Logic.ReceiptHeader(Session("CreateLoadReciptId"), True)
            Dim ld As New WMS.Logic.Load
            Dim loc As WMS.Logic.Location
            If Not WMS.Logic.Location.Exists(Session("CreateLoadLocation"), Session("UserSelectedWHArea")) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location does not exist", "Location does not exist")
            Else
                loc = New WMS.Logic.Location(Session("CreateLoadLocation"), Session("UserSelectedWHArea"))
            End If

            If Not WMS.Logic.SKU.Exists(Session("CreateLoadConsignee"), Session("CreateLoadSku")) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "SKU does not exist", "SKU does not exist")
            Else
                oSku = New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSku"))
            End If

            ''create the receipt line
            'Dim linenumber As Int32 = rc.addLine(oSku.CONSIGNEE, oSku.SKU, Session("CreateLoadQty"), Nothing, Nothing, Nothing, Nothing, Nothing, -1, "", 0, WMS.Logic.Common.GetCurrentUser, Nothing)
            ''generate the load
            Dim ldId As String = Session("CreateLoadLoadId")
            If ldId = "" Then
                ldId = WMS.Logic.Load.GenerateLoadId()
            End If
            'ld = rc.CreateLoad(linenumber, ldId, Session("CreateLoadUOM"), loc.Location, loc.Warehousearea, Session("CreateLoadQty"), WMS.Lib.Statuses.LoadStatus.AVAILABLE, "", oAttributes, WMS.Logic.Common.GetCurrentUser)
            Dim oRec As New WMS.Logic.Receiving()
            ld = oRec.CreateBlindReceivingLoads(rc.RECEIPT, oSku.CONSIGNEE, oSku.SKU, "", loc.Location, loc.Warehousearea, 1, ldId, WMS.Lib.Statuses.LoadStatus.AVAILABLE, Session("CreateLoadQty"), Session("CreateLoadUOM"), oAttributes, WMS.Logic.GetCurrentUser)(0)
            If Session("CreateLoadContainerID") <> "" Then
                Dim cntr As New WMS.Logic.Container
                cntr.ContainerId = Session("CreateLoadContainerID")
                cntr.Location = Session("CreateLoadLocation")
                cntr.Warehousearea = Session("CreateLoadWarehousearea")
                cntr.Post(WMS.Logic.Common.GetCurrentUser)
                'place it on the container
                cntr.Place(ld, WMS.Logic.Common.GetCurrentUser)
                'Print Container Label for created container
                cntr.PrintContainerLabel()
                'Print Packing List for created Container
                cntr.PrintContentList("", Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
            End If

            'Print Load labels if selected
            'Dim oCons As WMS.Logic.Consignee
            If Consignee.AutoPrintLoadIdOnReceiving(Session("CreateLoadConsignee")) Then
                ld.PrintLabel()
            End If

        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(m4nEx.Message))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Created"))
        Response.Redirect(MapVirtualPath("Screens/CLDBLD1.aspx"))
    End Sub

    Private Sub doChangeLine()
        MobileUtils.ClearBlindReceivingSession()
        Response.Redirect(MapVirtualPath("Screens/CLDBLD1.aspx"))
    End Sub

    Private Sub doMenu()
        'Session.Remove("CreateLoadRCN")
        'Session.Remove("CreateLoadRCNLine")
        'Session.Remove("CreateLoadConsignee")
        'Session.Remove("CreateLoadSKU")
        MobileUtils.ClearBlindReceivingSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                doChangeLine()
            Case "create"
                SubmitCreate(ExtractAttributeValues(), Session("CreateLoadDoPutAway"))
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
