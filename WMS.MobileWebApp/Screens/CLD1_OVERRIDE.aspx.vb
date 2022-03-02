Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports Wms.Logic


<CLSCompliant(False)> Public Class CLD1_OVERRIDE
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
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
        If Not IsPostBack Then
            DO1.Value("ERROR") = Session("ERROROVERRIDE")
            DO1.Value("SKU") = Session("CreateLoadSKU")
            Dim oSku As New Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
            DO1.Value("SKUDESC") = oSku.SKUDESC
            DO1.Value("UOM") = Session("CreateLoadUOM")

            DO1.Value("UNITS") = Session("CreateLoadUnits")
        End If
    End Sub

    Private Sub Override()
        Try
            sendRECOVRRD()
            doNext()
            Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1'", "Made4NetSchema")
            Response.Redirect(MapVirtualPath(url))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub doBack()
        Session("LoadPreviewsVals") = "RDTCLD1OVERRIDEHUTYPE"
        '  Session("CreateLoadOverrideUnits") = Session("CreateLoadUnits")
        Session.Remove("CreateLoadOverrideUnits")
        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url))
    End Sub

 
    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("ERROR")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("UNITS")
        DO1.AddLabelLine("UOM")
        setAttributes()
        DO1.AddSpacer()
    End Sub

    Private Sub setAttributes()
        Dim oSku As String = Session("CreateLoadSKU")
        Dim oConsignee As String = Session("CreateLoadConsignee")
        Dim oRecLine As WMS.Logic.ReceiptDetail
        Dim invAttrColl As InventoryAttributeBase


        Dim oAttr As WMS.Logic.AttributesCollection = Session("CreateLoadAttributes")


        If Session("CreateLoadRCNLine") > -1 Then
            oRecLine = New WMS.Logic.ReceiptDetail(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"))
            invAttrColl = oRecLine.Attributes
        End If
        If Not Session("CreateLoadASNAttributes") Is Nothing Then
            invAttrColl = Session("CreateLoadASNAttributes")
        End If
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
                              DO1.AddLabelLine(oAtt.Name, oAtt.Name)
                        Else
                            DO1.AddLabelLine(oAtt.Name, oAtt.Name)
                        End If
                        Try
                            'oAttr
                            If Not oAttr Is Nothing Then
                                DO1.Value(oAtt.Name) = oAttr.Item(oAtt.Name)

                            End If
                           
                        Catch ex As Exception
                        End Try
                    End If
                Next
            Else
                'SubmitCreate(Nothing, Session("CreateLoadDoPutAway"))
            End If
        Else
            'SubmitCreate(Nothing, Session("CreateLoadDoPutAway"))
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "back"
                doBack()
            Case "override"
                Override()                
        End Select
    End Sub

   
    Private Sub sendRECOVRRD()
        'Dim MSG As String = "RECOVRRD"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.RECOVRRD)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECOVRRD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))        
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", Session("CreateLoadConsignee"))
        aq.Add("DOCUMENT", Session("CreateLoadRCN"))
        aq.Add("DOCUMENTLINE", Session("CreateLoadRCNLine"))
        aq.Add("FROMLOAD", Session("CreateLoadLoadId"))
        aq.Add("FROMLOC", Session("CreateLoadLocation"))
        'aq.Add("FROMQTY", Session("CreateLoadUnits"))
        aq.Add("FROMSTATUS", Session("CreateLoadStatus"))
        aq.Add("NOTES", Session("ERROROVERRIDE"))
        aq.Add("SKU", Session("CreateLoadSKU"))
        aq.Add("TOLOAD", Session("CreateLoadLoadId"))
        aq.Add("TOLOC", Session("CreateLoadLocation"))
        aq.Add("TOQTY", Session("CreateLoadUnits"))
        aq.Add("TOSTATUS", Session("CreateLoadStatus"))
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(WMS.Lib.Actions.Audit.RECOVRRD)

    End Sub


    Private Sub gotoOverrideHUType()

        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1OVERRIDEHUTYPE'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url) & "?sourcescreen=RDTCLD1")

    End Sub

    Private Sub doNext()
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Session("CreateLoadContainerId") = "" And Session("HUTYPE") = "" Then
            gotoOverrideHUType()
        End If


        If Session("CreateLoadContainerId") <> "" Then
            Dim CheckCntSql As String = "SELECT * FROM CONTAINER WHERE CONTAINER='" & Session("CreateLoadContainerId") & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            'Checking if container already exists, if not creating new one
            If dt.Rows.Count > 0 Then
                Session("CreateLoadContainerID") = Session("CreateLoadContainerId")
            Else
                If Session("HUTYPE") = "" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("HUType is empty"))
                    Return
                End If
                ' Remember it in session if there is one load
                Dim oCont As WMS.Logic.Container
                oCont = New WMS.Logic.Container
                oCont.ContainerId = Session("CreateLoadContainerId")
                oCont.HandlingUnitType = Session("HUTYPE")
                oCont.Location = Session("CreateLoadLocation")
                oCont.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea()
                oCont.Post(WMS.Logic.Common.GetCurrentUser)

                Session("CreateLoadContainerID") = Session("CreateLoadContainerId")

            End If
        Else
            If Session("HUTYPE") <> "" Then
                ' Create new container with counter

                ' Remember it in session if there is one load
                If Session("CreateLoadNumLoads") = 1 Then
                    Dim oCont1 As WMS.Logic.Container
                    oCont1 = New WMS.Logic.Container
                    oCont1.ContainerId = Made4Net.Shared.getNextCounter("CONTAINER")
                    oCont1.HandlingUnitType = Session("HUTYPE")
                    oCont1.Location = Session("CreateLoadLocation")
                    oCont1.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea()
                    oCont1.Post(WMS.Logic.Common.GetCurrentUser)
                    Session("CreateLoadContainerId") = oCont1.ContainerId

                Else
                    Session("HUTYPE") = Session("HUTYPE")
                    Session("CreateContainer") = "1"
                    Session("CreateLoadContainerID") = ""
                End If

            Else
                Session("CreateLoadContainerID") = ""
            End If
        End If

        SubmitCreate(Nothing, False)

    End Sub

    Private Sub SubmitCreate(ByVal oAttributes As WMS.Logic.AttributesCollection, Optional ByVal DoPutaway As Boolean = False)
        Dim ResponseMessage As String = ""
        Dim attributesarray As New ArrayList
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        
        Try
            oAttributes = Session("CreateLoadAttributes")

            Dim ld() As WMS.Logic.Load
            Dim oRec As New Logic.Receiving
            If String.IsNullOrEmpty(Session("CreateLoadNumLoads")) OrElse Session("CreateLoadNumLoads") = 0 Then Session("CreateLoadNumLoads") = 1
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

                ' Check if number of loads is more than one and if we need to create containers
                ld = oRec.CreateLoad(Session("CreateLoadRCN"), Session("CreateLoadRCNLine"), Session("CreateLoadSKU"), Session("CreateLoadLoadId"), _
                    Session("CreateLoadUOM"), Session("CreateLoadLocation"), Session("CreateLoadWarehousearea"), Session("CreateLoadUnits"), Session("CreateLoadStatus"), _
                    Session("CreateLoadHoldRC"), Session("CreateLoadNumLoads"), Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger, oAttributes, Session("CreateLoadLabelPrinter"), oReceiptLine.DOCUMENTTYPE, "", "")
                Try
                    RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
                Catch ex As Exception
                End Try
            End If

            ' After Load Creation we will put the loads on his container
            If Session("CreateContainer") = "1" Then
                Dim LoadsCreatedCount As Int32
                For LoadsCreatedCount = 0 To ld.Length() - 1
                    Dim cntr As New WMS.Logic.Container()
                    cntr.ContainerId = Made4Net.Shared.getNextCounter("CONTAINER")
                    cntr.HandlingUnitType = Session("HUTYPE")
                    cntr.Location = Session("CreateLoadLocation")
                    cntr.Warehousearea = Session("CreateLoadWarehousearea")
                    cntr.Post(WMS.Logic.Common.GetCurrentUser)

                    cntr.Place(ld(LoadsCreatedCount), WMS.Logic.Common.GetCurrentUser)
                Next
            Else
                If Session("CreateLoadContainerID") <> "" Then
                    Dim cntr As New WMS.Logic.Container(Session("CreateLoadContainerID"), True)
                    Dim LoadsCreatedCount As Int32
                    For LoadsCreatedCount = 0 To ld.Length() - 1
                        cntr.Place(ld(LoadsCreatedCount), WMS.Logic.Common.GetCurrentUser)
                    Next
                End If
            End If
            If Not String.IsNullOrEmpty(Session("ReceiveByIdASNID")) Then
                Session("ASNQTYRECEIVED") = 1
                Session("CreateLoadASNRemainingUnits") = Session("CreateLoadASNRemainingUnits") - ld(0).UNITS
            End If

            ' Create Handling unit transaction if needed
            If Session("CreateLoadHUTRANS") = "YES" Then
                Try
                    Dim strHUTransIns As String
                    If Session("CreateLoadContainerID") <> "" Then
                        strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                                      "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','RECEIPT','" & Session("CreateLoadRCN") & "',GETDATE(),'" & Session("CreateLoadConsignee") & "','" & oReceiptLine.ORDERID & "','" & oReceiptLine.DOCUMENTTYPE & "','" & oReceiptLine.COMPANY & "','" & oReceiptLine.COMPANYTYPE & "','" & Session("HUTYPE") & "','1',GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                    Else
                        strHUTransIns = "INSERT INTO [HANDLINGUNITTRANSACTION]([TRANSACTIONID],[TRANSACTIONTYPE],[TRANSACTIONTYPEID],[TRANSACTIONDATE],[CONSIGNEE],[ORDERID],[DOCUMENTTYPE],[COMPANY],[COMPANYTYPE],[HUTYPE],[HUQTY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER])" & _
                                                      "VALUES('" & Made4Net.Shared.getNextCounter("HUTRANSID") & "','RECEIPT','" & Session("CreateLoadRCN") & "',GETDATE(),'" & Session("CreateLoadConsignee") & "','" & oReceiptLine.ORDERID & "','" & oReceiptLine.DOCUMENTTYPE & "','" & oReceiptLine.COMPANY & "','" & oReceiptLine.COMPANYTYPE & "','" & Session("HUTYPE") & "','" & Session("CreateLoadNumLoads") & "',GETDATE(),'" & Logic.GetCurrentUser & "',GETDATE(),'" & Logic.GetCurrentUser & "')"
                    End If
                    DataInterface.RunSQL(strHUTransIns)
                Catch ex As Exception
                End Try
            End If

            ' If Create and Putaway then request pickup for each load
            If DoPutaway Then
                Dim pw As New Putaway
                Dim i As Int32
                Session.Remove("CREATELOADPCKUP")
                For i = 0 To ld.Length() - 1
                    ld(i).RequestPickUp(Logic.GetCurrentUser, "") '' RWMS-1277
                    Session("CREATELOADPCKUP") = ld(i).LOADID
                Next
                If Not String.IsNullOrEmpty(Session("CREATELOADPCKUP")) Then
                    'Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=cld1"))
                    doCloseASN(True)
                    Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=cld1"))
                End If
            End If
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Created"))
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try

    End Sub

    Private Sub doCloseASN(ByVal pCloseForRPK As Boolean)
        If String.IsNullOrEmpty(Session("ReceiveByIdASNID")) Then
            Return
        End If
        Session.Remove("CreateLoadRCN")
        Session.Remove("CreateLoadContainerID")
        Session.Remove("CreateLoadASNLoadID")
        Session.Remove("CreateLoadASNRemainingUnits")
        Session.Remove("CreateLoadASNAttributes")
        If Not pCloseForRPK AndAlso String.IsNullOrEmpty(Session("ASNQTYRECEIVED")) Then
            Response.Redirect(MapVirtualPath("Screens/ReceiveByID1.aspx"))
        End If
        Session.Remove("ASNQTYRECEIVED")
        Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
        oASN.SetStatus(WMS.Lib.Statuses.ASN.RECEIVED, WMS.Logic.GetCurrentUser)
        Session.Remove("ReceiveByIdASNID")
        If Not pCloseForRPK Then
            Response.Redirect(MapVirtualPath("Screens/ReceiveByID.aspx"))
        End If
    End Sub


End Class
