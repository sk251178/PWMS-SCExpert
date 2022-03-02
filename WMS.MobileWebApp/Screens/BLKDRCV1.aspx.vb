Imports Made4Net.Shared.Collections
Imports Made4Net.DataAccess
Imports System.Collections
Imports Made4Net.Mobile
Imports Made4Net.Shared.Web
Imports WMS.Logic
<CLSCompliant(False)> Public Class BLKDRCV1
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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            Session.Remove("SELECTEDSKU")
        End If


        If Not IsPostBack Then
            DO1.Value("CONTAINERID") = Session("CreateLoadContainerID")
            DO1.Value("RECEIPTID") = Session("CreateLoadReciptId")
            DO1.Value("CONSIGNEE") = Session("CreateLoadConsignee")
            DO1.Value("LOCATION") = Session("CreateLoadReceiptLocation")
            DO1.Value("WAREHOUSEAREA") = Session("CreateLoadReceiptWarehousearea")
            Session("SCANED") = 0
            DO1.Value("SCANED") = Session("SCANED")
            'DO1.Value("RECEIPTLINE") = Session("CreateLoadReciptLine")
            Dim skucoll As New Made4Net.DataAccess.Collections.GenericCollection
            Session("SKUCOLL") = skucoll
        End If
    End Sub


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddLabelLine("RECEIPTID")
        'DO1.AddLabelLine("RECEIPTLINE")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddLabelLine("SCANED")
        DO1.AddTextboxLine("SKU")
        DO1.AddSpacer()
    End Sub

    Private Sub doNextSku()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
            If DO1.Value("SKU").Trim <> "" Then
                ' Check for sku
                If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SKU) FROM vSKUCODE WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "%' OR SKU LIKE '" & DO1.Value("SKU") & "%')") > 1 Then
                    ' Go to Sku select screen
                    Session("FROMSCREEN") = "BLKDRCV1"
                    Session("SKUCODE") = DO1.Value("SKU").Trim
                    ' Add all controls to session for restoring them when we back from that sreen
                    Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) 'Changed
                ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT SKU) FROM vSKUCODE WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "%'") = 1 Then
                    DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT SKU FROM vSKUCODE WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "%'")
                End If
            End If

            Dim oLocation As New WMS.Logic.Location(Session("CreateLoadReceiptLocation"), Session("CreateLoadReceiptWarehousearea"))
            Dim oSku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), DO1.Value("SKU"))
            If Not IsValidSku(Session("CreateLoadReciptId"), oSku.CONSIGNEE, oSku.SKU) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Sku scanned is not valid"))
                Return
            End If
            Dim skuColl As Made4Net.DataAccess.Collections.GenericCollection = CType(Session("SKUCOLL"), Made4Net.DataAccess.Collections.GenericCollection)
            If Not skuColl.Item(oSku.CONSIGNEE & "@" & oSku.SKU) Is Nothing Then
                skuColl.Item(oSku.CONSIGNEE & "@" & oSku.SKU) += 1
            Else
                skuColl.Add(oSku.CONSIGNEE & "@" & oSku.SKU, 1)
            End If
            Session("SCANED") += 1
            DO1.Value("SCANED") = Session("SCANED")
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(m4nEx.Message))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        DO1.Value("SKU") = ""
    End Sub

    Private Sub doMenu()
        MobileUtils.ClearCreateLoadProcessSession(True)
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doCloseContainer()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try
            Dim consignee, sku As String
            Dim oSku As WMS.Logic.SKU
            Dim qty As Int32
            Dim cntr As New WMS.Logic.Container(Session("CreateLoadContainerID"), True)
            Dim rc As New WMS.Logic.ReceiptHeader(Session("CreateLoadReciptId"), True)
            Dim ld As New WMS.Logic.Load
            Dim loc As WMS.Logic.Location

            If Not WMS.Logic.Location.Exists(Session("CreateLoadReceiptLocation"), Session("CreateLoadReceiptWarehousearea")) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location does not exist", "Location does not exist")
                Return
            Else
                loc = New WMS.Logic.Location(Session("CreateLoadReceiptLocation"), Session("CreateLoadReceiptWarehousearea"))
            End If
            Dim skuColl As Made4Net.DataAccess.Collections.GenericCollection = Session("SKUCOLL")
            For i As Int32 = 0 To skuColl.Count - 1
                consignee = skuColl.Keys.Item(i).Split("@")(0)
                sku = skuColl.Keys.Item(i).Split("@")(1)
                oSku = New WMS.Logic.SKU(consignee, sku)
                qty = skuColl.Item(skuColl.Keys.Item(i))
                Dim rd As ReceiptDetail = New ReceiptDetail(Session("CreateLoadReciptId"), ReceiptDetail.LineExist(Session("CreateLoadReciptId"), consignee, sku))

                If qty > rd.QTYEXPECTED Then
                    Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Scaned Sku number is greated the expected!"))
                    Return
                End If
                'generate the load
                ld = rc.CreateLoad(rd.RECEIPTLINE, WMS.Logic.Load.GenerateLoadId(), oSku.getLowestUom, Session("CreateLoadReceiptLocation"), Session("CreateLoadReceiptWarehousearea"), qty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, "", Nothing, WMS.Logic.Common.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
                Try
                    RWMS.Logic.AppUtil.SetReceivedWeight(ld.CONSIGNEE, ld.SKU, ld.RECEIPT, ld.RECEIPTLINE)
                Catch ex As Exception
                End Try
                'place it on the container
                cntr.Place(ld, WMS.Logic.Common.GetCurrentUser)
            Next
            'Print Container Label for created container
            cntr.PrintContainerLabel()
            'Print Packing List for created Container
            cntr.PrintContentList("", Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)

            'Print Load labels if selected
            'Dim oCons As WMS.Logic.Consignee
            Dim strCntrConsignee As String
            Try
                strCntrConsignee = DataInterface.ExecuteScalar("SELECT TOP 1 CONSIGNEE FROM vContainerLabel WHERE CONTAINER = '" & cntr.ContainerId & "'")
            Catch ex As Exception
                strCntrConsignee = consignee
            End Try
            If WMS.Logic.Consignee.AutoPrintLoadIdOnReceiving(strCntrConsignee) Then
                Dim loads As DataTable = New DataTable
                DataInterface.FillDataset("SELECT * FROM CONTAINERLOADS WHERE CONTAINERID = '" & cntr.ContainerId & "'", loads)
                For Each drld As DataRow In loads.Rows
                    Dim prnld As WMS.Logic.Load = New WMS.Logic.Load(drld("LOADID").ToString())
                    prnld.PrintLabel()
                Next
            End If
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(m4nEx.Message))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Container Created"))

        Session.Remove("CreateLoadContainerID")
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/BLKDRCV.aspx"))
    End Sub

    Private Function IsValidSku(ByVal ReceiptId As String, ByVal vConsignee As String, ByVal vSku As String) As Boolean
        'Dim loads As DataTable = New DataTable
        Dim Res As Int32 = DataInterface.ExecuteScalar("SELECT COUNT(1) as RES FROM RECEIPTDETAIL WHERE RECEIPT = '" & ReceiptId & "' AND CONSIGNEE='" & vConsignee & "' AND SKU='" & vSku & "'")
        If Res = 0 Then
            Return False
        End If
        If Res > 1 Then
            Return False
        End If
        If Res = 1 Then
            Return True
        End If
    End Function

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "nextitem"
                doNextSku()
            Case "closecontainer"
                doCloseContainer()
            Case "menu"
                doMenu()
        End Select
    End Sub

End Class
