Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class ReceiveById1
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
            setScreen()
        End If
    End Sub

    Private Sub doRecieve(ByVal doPutaway As Boolean, ByVal ScanAnotherLoad As Boolean, ByVal ContainerPW As Boolean)
        Dim type As String = Session("ReceiveByIdType")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        ViewState()("RecivingWarehouseArea") = WMS.Logic.Warehouse.getUserWarehouseArea()
       
        Try
            'Added for PWMS-746 Start
            If DO1.Value("Location").Trim = "" Or DO1.Value("Location").Trim = String.Empty Then
                Throw New M4NException(New Exception, "Please enter Location", "Please enter Location")
            End If
            'Added for PWMS-746 End
            If type = "LOAD" Then
                Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
                Dim oLoad As Load = oASN.ReceiveByLoadId(DO1.Value("Location"), ViewState()("RecivingWarehouseArea"), WMS.Logic.GetCurrentUser())
                Dim oCont As WMS.Logic.Container
                If DO1.Value("CONTAINER").ToLower <> String.Empty Then
                    'Create the container and place the load on it
                    If Not WMS.Logic.Container.Exists(DO1.Value("CONTAINER").ToLower) Then
                        oCont = New WMS.Logic.Container()
                        oCont.ContainerId = DO1.Value("CONTAINER").ToLower
                        oCont.UsageType = DO1.Value("USAGETYPE")
                        oCont.Location = DO1.Value("Location")
                        oCont.Warehousearea = ViewState()("RecivingWarehouseArea")

                        oCont.HandlingUnitType = DO1.Value("HUTYPE")
                        oCont.Post(WMS.Logic.GetCurrentUser())
                    Else
                        oCont = New WMS.Logic.Container(DO1.Value("CONTAINER").ToLower, True)
                    End If
                    oCont.Place(oLoad, WMS.Logic.GetCurrentUser())
                End If
                Session("LoadsPW") = Session("LoadsPW") + "," + oLoad.LOADID
                If ScanAnotherLoad Then
                    Response.Redirect(MapVirtualPath("Screens/ReceiveById.aspx"))
                End If
                If doPutaway And Not ContainerPW Then
                    'Iterate the loads and request pick up
                    Dim loadsArr() As String = Convert.ToString(Session("LoadsPW")).TrimStart(",").Split(",")
                    Dim tmpLoad As Load
                    Session.Remove("CREATELOADPCKUP")
                    For i As Int32 = 0 To loadsArr.Length - 1
                        tmpLoad = New Load(loadsArr(i))
                        tmpLoad.RequestPickUp(WMS.Logic.Common.GetCurrentUser, "") 'RWMS-1277
                        Session("CREATELOADPCKUP") = tmpLoad.LOADID
                    Next
                    Session.Remove("LoadsPW")
                    If Not String.IsNullOrEmpty(Session("CREATELOADPCKUP")) Then
                        Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=receivebyid1"))
                    End If
                    'Response.Redirect(MapVirtualPath("Screens/RPK2.aspx"))
                ElseIf ContainerPW Then
                    oCont.PutAway(WMS.Logic.GetCurrentUser())
                End If
            Else
                AsnDetail.ReceiveByContId(Session("ReceiveByIdContID"), DO1.Value("Location"), ViewState("RecivingWarehouseArea"), WMS.Logic.GetCurrentUser())
                If doPutaway Then
                    Response.Redirect(MapVirtualPath("Screens/RPKC.aspx?CntrId=" & Session("ReceiveByIdContID")))
                End If
            End If
            Response.Redirect(MapVirtualPath("Screens/ReceiveById.aspx"))
        Catch ex As Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return
        End Try
    End Sub

    Private Sub doBack()
        Session.Remove("ReceiveByIdType")
        Session.Remove("ReceiveByIdASNID")
        Session.Remove("ReceiveByIdContID")
        Session.Remove("ReceiveByIdreceiptId")
        Response.Redirect(MapVirtualPath("Screens/ReceiveById.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'in case we want to display the container details
        DO1.AddLabelLine("ContainerId")
        'in case we want to display the Load details
        DO1.AddLabelLine("Loadid")
        DO1.AddLabelLine("Consignee", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("Sku")
        DO1.AddLabelLine("SkuDesc")
        DO1.AddLabelLine("Receipt")
        DO1.AddLabelLine("BOL")
        DO1.AddLabelLine("ReceiptLine")
        DO1.AddLabelLine("UOM")
        'DO1.AddLabelLine("UOMDesc")
        DO1.AddLabelLine("Units")
        DO1.AddSpacer()
        DO1.AddTextboxLine("CONTAINER")
        DO1.AddDropDown("HUTYPE")
        DO1.AddDropDown("USAGETYPE")
        DO1.AddTextboxLine("Location", True, "Receive")
        'DO1.AddTextbox Line("Warehousearea")
        DO1.AddSpacer()
    End Sub

    Private Sub setScreen()
        Dim type As String = Session("ReceiveByIdType")
        Dim oCons As Consignee
        Dim dd As Made4Net.WebControls.MobileDropDown
        If type = "LOAD" Then
            DO1.setVisibility("Loadid", True)
            DO1.setVisibility("Consignee", True, Session("3PL"))
            DO1.setVisibility("Sku", False)
            DO1.setVisibility("SkuDesc", True)
            DO1.setVisibility("ReceiptLine", True)
            DO1.setVisibility("UOM", True)
            'DO1.setVisibility("UOMDesc", True)
            DO1.setVisibility("Units", True)

            Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
            Dim oRecDet As New ReceiptDetail(oASN.Receipt, oASN.ReceiptLine)
            DO1.setVisibility("Loadid", True)
            DO1.Value("Loadid") = oASN.LoadId
            DO1.Value("Consignee") = oRecDet.CONSIGNEE
            DO1.Value("Sku") = oRecDet.SKU
            Dim osku As New SKU(oRecDet.CONSIGNEE, oRecDet.SKU)
            DO1.Value("SkuDesc") = osku.SKUDESC

            DO1.Value("SkuDesc") = osku.SKUDESC

            DO1.Value("Receipt") = oRecDet.RECEIPT
            DO1.Value("ReceiptLine") = oRecDet.RECEIPTLINE

            Dim sqluom As String = " SELECT DESCRIPTION FROM CODELISTDETAIL " & _
                          " WHERE CODELISTCODE = 'UOM' AND CODE = '" & oASN.UOM & "'"

            'DO1.Value("UOMDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)

            DO1.Value("UOM") = oASN.UOM
            updateUnitsDropDown(osku.CONSIGNEE, osku.SKU, oASN.UOM, oASN.Units) 'oASN.Units
            oCons = New Consignee(oRecDet.CONSIGNEE)
            DO1.Value("Location") = oCons.DEFAULTRECEIVINGLOCATION
            ViewState()("RecivingWarehouseArea") = oCons.DEFAULTRECEIVINGWAREHOUSEAREA
            'DO1.Value("Warehousearea") = oCons.DEFAULTRECEIVINGWAREHOUSEAREA


            'dd = DO1.Ctrl("HUTYPE")
            'dd.AllOption = False
            'dd.TableName = "HANDELINGUNITTYPE"
            'dd.ValueField = "CONTAINER"
            'dd.TextField = "CONTAINERDESC"
            'dd.DataBind()

            dd = DO1.Ctrl("USAGETYPE")
            dd.AllOption = False
            dd.TableName = "CODELISTDETAIL"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.Where = "CODELISTCODE = 'CONTUSAGE'"
            dd.DataBind()


            DO1.Value("CONTAINER") = oASN.Container
            If WMS.Logic.Container.Exists(oASN.Container) Then
                Dim cont As New WMS.Logic.Container(oASN.Container, False)
                Try
                    dd.Value = cont.UsageType
                Catch
                End Try
            End If

            DO1.setVisibility("ContainerId", False)
            DO1.setVisibility("BOL", False)
        Else
            Dim oRec As New ReceiptHeader(Session("ReceiveByIdreceiptId"))
            DO1.setVisibility("ContainerId", True)
            DO1.Value("ContainerId") = Session("ReceiveByIdContID")

            DO1.Value("Receipt") = oRec.RECEIPT
            DO1.Value("BOL") = oRec.BOL

            oCons = New Consignee(oRec.LINES(0).CONSIGNEE)
            DO1.Value("Location") = oCons.DEFAULTRECEIVINGLOCATION

            'DO1.Value("Location") = ""
            'DO1.Value("Warehousearea") = ""

            DO1.setVisibility("Loadid", False)
            DO1.setVisibility("Consignee", False, Session("3PL"))
            DO1.setVisibility("Sku", False)
            DO1.setVisibility("SkuDesc", False)
            DO1.setVisibility("ReceiptLine", False)
            DO1.setVisibility("UOM", False)
            'DO1.setVisibility("UOMDesc", False)
            DO1.setVisibility("Units", False)
            DO1.setVisibility("CONTAINER", False)
            DO1.setVisibility("HUTYPE", True)
            DO1.setVisibility("USAGETYPE", False)
        End If
        dd = DO1.Ctrl("HUTYPE")
        dd.AllOption = False
        dd.TableName = "HANDELINGUNITTYPE"
        dd.ValueField = "CONTAINER"
        dd.TextField = "CONTAINERDESC"
        dd.DataBind()

    End Sub

    Private Sub updateUnitsDropDown(ByVal pConsignee As String, ByVal pSKU As String, ByVal pUom As String, ByVal pUnits As Decimal)
        Dim sql As String = String.Format("Select isnull(unitsperlowestuom,1) from skuuom where sku='{0}' and consignee='{1}' and uom='{2}'", pSKU, pConsignee, pUom)
        Dim unitsPerLowestUom As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If unitsPerLowestUom = 0 Then unitsPerLowestUom = 1
        DO1.Value("Units") = Math.Floor(pUnits / unitsPerLowestUom)
    End Sub


    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "receive & putaway"
                doRecieve(True, False, False)
            Case "receive"
                doRecieve(False, False, False)
            Case "receive & putaway container"
                doRecieve(False, True, True)
            Case "adjust"
                doAdjust()
            Case "back"
                doBack()
        End Select
    End Sub

    Private Sub doAdjust()
        Dim type As String = Session("ReceiveByIdType")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If type <> "LOAD" Then
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Adjust is only possible for loads, not containers"))
            'Return
            'Dim oCont As WMS.Logic.Container
            'oCont = New WMS.Logic.Container
            'oCont.ContainerId = DO1.Value("CONTAINERID")
            'oCont.HandlingUnitType = DO1.Value("HUTYPE")
            'oCont.Location = DO1.Value("LOCATION")
            'oCont.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea()
            'oCont.Post(WMS.Logic.Common.GetCurrentUser)
            Dim sql As String = String.Format("select asnid,loadid from asndetail where container={0} and status={1}", _
            Made4Net.Shared.FormatField(DO1.Value("CONTAINERID")), Made4Net.Shared.FormatField(WMS.Lib.Statuses.ASN.EXPECTED))

            Dim dt As New DataTable()
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            For Each dr As DataRow In dt.Rows
                If String.IsNullOrEmpty(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LOADID"), "")) Then
                    sql = String.Format("update asndetail set loadid={0} where asnid={1}", _
                    Made4Net.Shared.FormatField(WMS.Logic.Load.GenerateLoadId()), Made4Net.Shared.FormatField(dr("ASNID")))
                    Made4Net.DataAccess.DataInterface.RunSQL(sql)
                End If
                WMS.Logic.AsnDetail.PrintLabel(dr("ASNID").ToString(), "")
            Next
            doBack()
        Else
            GoToCLDInfo()
        End If

    End Sub

    Private Sub GoToCLDInfo()
        Dim oASN As New AsnDetail(Convert.ToString(Session("ReceiveByIdASNID")))
        Dim oRecDet As New ReceiptDetail(oASN.Receipt, oASN.ReceiptLine)
        Session("CreateLoadRCN") = oRecDet.RECEIPT

        Session("CreateLoadRCNLine") = oRecDet.RECEIPTLINE
        Session("CreateLoadRCNMultipleLines") = False
        Session("CreateLoadConsignee") = oRecDet.CONSIGNEE
        Session("CreateLoadSKU") = oRecDet.SKU
        Session("CreateLoadASNLoadID") = oASN.LoadId
        Session("CreateLoadUOM") = oASN.UOM
        Session("CreateLoadContainerID") = oASN.Container
        Session("CreateLoadASNRemainingUnits") = oASN.Units
        Session("CreateLoadASNAttributes") = oASN.ASNAttributes
        Response.Redirect(MobileUtils.GetURLByScreenCode("RDTCLD1"))
    End Sub

End Class
