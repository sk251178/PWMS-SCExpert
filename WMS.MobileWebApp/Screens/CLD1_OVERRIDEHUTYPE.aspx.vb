Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports Wms.Logic


<CLSCompliant(False)> Public Class CLD1_OVERRIDEHUTYPE
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
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            DO1.Value("WARNING") = t.Translate("The payload created was not placed on a handling unit. Are you sure?")

           
        End If
    End Sub

    Private Sub Override()
        Try
            doNext()
            Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD1'", "Made4NetSchema")
            Response.Redirect(MapVirtualPath(url))
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
    End Sub

    Private Sub doBack()
        'Session("CreateLoadOverrideUnits") = Session("CreateLoadUnits")

        Dim screenid As String = ""
        If Not IsNothing(Request.QueryString("sourcescreen")) Then
            screenid = Request.QueryString("sourcescreen")
        End If


        Dim url As String = DataInterface.ExecuteScalar(String.Format("select url from sys_screen where  screen_id = '{0}'", screenid), "Made4NetSchema")
        Try
            Session("LoadPreviewsVals") = "RDTCLD1OVERRIDEHUTYPE"
            Response.Redirect(MapVirtualPath(url)) '& "?sourcescreen=RDTCLD1OVERRIDEHUTYPE")

        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception

        End Try

    End Sub


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("WARNING")
      
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "no"
                doBack()
            Case "yes"
                Override()
        End Select
    End Sub

  
    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

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
                    ld(i).RequestPickUp(Logic.GetCurrentUser, "") ''RWMS-1277
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
        Try

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
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception

        End Try
    End Sub


End Class
