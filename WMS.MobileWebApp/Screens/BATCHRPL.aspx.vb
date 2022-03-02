Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class BATCHRPL
    Inherits PWMSRDTBase

    Public Const CURRENTLETDOWNINDEX As String = "currentLetdownIndex"

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

    Private confQty As Decimal = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION)
            Session.Add(CURRENTLETDOWNINDEX, 0)
            If LoadDetails() Then
                Dim brh As BatchReplenDetailCollection = Session(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION)
                If Not IsNothing(brh) Then
                    SetScreen(GetDetailLine())
                End If
            Else
                doMenu()
            End If
        End If
    End Sub

    Private Function LoadDetails() As Boolean
        Dim t As Made4Net.Shared.Translation.Translator = GetTranslator()
        Try
            ' Verify the Task, if not valid it will throw Made4Net.Shared.M4NException
            'Dim task As Task = New Task().GetAvailableTaskByReplenishmentType(WMS.Lib.TASKTYPE.BRLETDOWN)
            Dim tmgr As WLTaskManager = New WLTaskManager()
            Dim task As Task = tmgr.RequestTask(WMS.Logic.GetCurrentUser(), TASKTYPE.BRLETDOWN, Nothing)
            If task Is Nothing Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No task available"))
                Return False
            End If
            If task.STATUS = WMS.Lib.Statuses.Task.COMPLETE Or task.STATUS = WMS.Lib.Statuses.Task.CANCELED Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid task status"))
                Return False
            End If
            Session.Add(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK, task)
            Dim brh As BatchReplenHeader = New BatchReplenHeader(task.Replenishment)
            Session.Add(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER, brh)
            If Not Session(WMS.Lib.SESSION.SHOWTASKMANAGER) Then
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            End If

            If brh.STATUS <> WMS.Lib.Statuses.Picklist.RELEASED Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid batch status"))
                Return False
            End If
            WriteToRDTLog("Requested TASK {0}, TASKTYPE {1}, STATUS={2}", task.TASK, task.TASKTYPE, task.STATUS)

            Dim crdC As BatchReplenDetailCollection = New BatchReplenDetailCollection(brh.BATCHREPLID, WMS.Lib.Statuses.BatchReplenDetail.RELEASED)
            If crdC.Count > 0 Then
                crdC.Sort("REPLLINE", BreplenDetailSortDirection.Ascending)
                Session.Add(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION, crdC)
            Else
                MyBase.WriteToRDTLog("Default Printer Batch Label Printer Value : " & Session("DefaultPrinter"))
                If brh.STATUS <> WMS.Lib.Statuses.BatchReplenHeader.LETDOWN AndAlso brh.ShouldPrintBatchLabel() Then                     'RWMS-3921
                    If Session("DefaultPrinter") = "" Then
                        MyBase.WriteToRDTLog(" Flow Navigating to page BATCH LABEL PRINT... ")
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLPRINT))
                    Else
                        Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                        MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        MobileUtils.UpdateDPrinterInUserProfile(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        brh.PrintBatchLabels(prntr.PrinterQName)
                    End If
                Else
                    If brh.STATUS <> WMS.Lib.Statuses.BatchReplenHeader.LETDOWN Then
                        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLETDOWN1))
                        Return True
                    Else
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No Details for batch replenishment"))
                        Return False
                    End If
                End If

            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return False
        End Try
        Return True
    End Function

    Private Sub SetScreen(ByVal batchReplenDetail As BatchReplenDetail)
        Dim task As Task = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
        Dim ld As New Load(batchReplenDetail.FROMLOAD)
        Dim sku As New SKU(batchReplenDetail.CONSIGNEE, batchReplenDetail.FROMSKU)

        DO1.Value("TASKTYPE") = task.TASKTYPE
        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
        DO1.Value("CONTAINER") = brh.REPLCONTAINER
        DO1.Value("PICKREGION") = brh.PICKREGION
        DO1.Value("LOCATION") = batchReplenDetail.FROMLOCATION
        DO1.Value("FROMLOAD") = batchReplenDetail.FROMLOAD
        DO1.Value("REPLID") = brh.BATCHREPLID
        DO1.Value("REPLLINE") = batchReplenDetail.REPLLINE

        DO1.Value("TOSKU") = batchReplenDetail.FROMSKU
        DO1.Value("SKUUOM") = ld.LOADUOM
        'DO1.Value("FROMQTY") = sku.ConvertUnitsToUom(ld.LOADUOM, batchReplenDetail.FROMQTY)

        DO1.Value("CONFIRMATION") = String.Empty
        'DO1.Value("QTY") = batchReplenDetail.FROMQTY

        DO1.Value("QTY") = sku.ConvertUnitsToUom(ld.LOADUOM, batchReplenDetail.FROMQTY)

    End Sub

    Private Sub doNext()
        Dim batchReplenDetail As BatchReplenDetail = GetDetailLine()
        If IsValidated(batchReplenDetail) Then

            ' Update the detail record
            batchReplenDetail.LetDown(confQty)
            ' If more details to process, then get next detail record
            Session.Add(CURRENTLETDOWNINDEX, Session(CURRENTLETDOWNINDEX) + 1)
            If CType(Session(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION), BatchReplenDetailCollection).Count > Session(CURRENTLETDOWNINDEX) Then
                SetScreen(GetDetailLine())
                UpdateTaskFromLocation(batchReplenDetail)
            Else
                MyBase.WriteToRDTLog("Default Printer Batch Label Printer Value : " & Session("DefaultPrinter"))
                Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
                If brh.STATUS <> WMS.Lib.Statuses.BatchReplenHeader.LETDOWN AndAlso brh.ShouldPrintBatchLabel() Then                     'RWMS-3921
                    If Session("DefaultPrinter") = "" Then
                        MyBase.WriteToRDTLog(" Flow Navigating to page BATCH LABEL PRINT... ")
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLPRINT))
                    Else
                        Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                        MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        MobileUtils.UpdateDPrinterInUserProfile(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        brh.PrintBatchLabels(prntr.PrinterQName)
                    End If
                End If
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLETDOWN1))
            End If
        End If
    End Sub

    Private Sub UpdateTaskFromLocation(batchReplenDetail As BatchReplenDetail)
        Dim task As Task = CType(Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK), Task)
        task.FROMLOCATION = batchReplenDetail.FROMLOCATION
        task.Save()
    End Sub

    Private Function GetTranslator() As Made4Net.Shared.Translation.Translator
        Return New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
    End Function

    Private Function IsValidated(ByVal batchReplenDetail As BatchReplenDetail) As Boolean
        Dim t = GetTranslator()
        Dim sCONFTYPE As String = Session("CONFIRMATIONTYPE")
        If Not sCONFTYPE Is Nothing Then
            Select Case sCONFTYPE
                Case WMS.Lib.Release.CONFIRMATIONTYPE.NONE
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOCATION
                    If DO1.Value("CONFIRMATION").Trim() <> batchReplenDetail.FROMLOCATION.Trim() Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Location Confirmation"))
                        Return False
                    End If
                Case WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
                    If DO1.Value("CONFIRMATION").Trim() <> batchReplenDetail.FROMLOAD.Trim() Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Load Confirmation"))
                        Return False
                    End If
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU
                    If DO1.Value("CONFIRMATION").Trim() <> batchReplenDetail.FROMSKU.Trim() Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong SKU Confirmation"))
                        Return False
                    End If
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION
                    If DO1.Value("CONFIRMATION").Trim() <> batchReplenDetail.FROMSKU.Trim() Or DO1.Value("LOC").Trim() <> batchReplenDetail.FROMLOCATION.Trim() Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong SKU Location Confirmation"))
                        Return False
                    End If
            End Select
        Else
            If DO1.Value("CONFIRMATION").Trim() <> batchReplenDetail.FROMLOCATION.Trim() Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Location Confirmation"))
                Return False
            End If
        End If
        ' Validate the Quantity
        Try
            Dim ld As New Load(batchReplenDetail.FROMLOAD)
            Dim sku As New SKU(batchReplenDetail.CONSIGNEE, batchReplenDetail.FROMSKU)
            confQty = Convert.ToDecimal(DO1.Value("QTY"))
            confQty = confQty * sku.ConvertToUnits(ld.LOADUOM)
            MyBase.WriteToRDTLog(String.Format("BatchReplen FromSKU={0},UserConfirmQty={1},ConfirmQtyinLowestUOM={2},skuconvertunitsofloaduom={3}", batchReplenDetail.FROMSKU, DO1.Value("QTY").ToString(), confQty.ToString(), sku.ConvertToUnits(ld.LOADUOM).ToString()))
            If confQty < 0 Or confQty > batchReplenDetail.FROMQTY Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid Quantity"))
                Return False
            End If
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return False
        End Try

        Return True
    End Function

    Private Function GetDetailLine() As BatchReplenDetail
        Return CType(Session(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION), BatchReplenDetailCollection).Item(Session(CURRENTLETDOWNINDEX))
    End Function

    Private Sub doMenu()
        Dim task As Task = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
        If Not IsNothing(task) Then
            task.ExitTask()
            Session.Remove(WMS.Lib.SESSION.BATCHREPLENLETDOWNTASK)
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TASKTYPE")
        DO1.AddLabelLine("CONTAINER")
        DO1.AddLabelLine("REPLID", "Batch replen ID")
        DO1.AddLabelLine("REPLLINE", "Line")
        DO1.AddLabelLine("PICKREGION")
        DO1.AddLabelLine("FROMLOAD")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("TOSKU", "SKU")
        DO1.AddLabelLine("SKUUOM", "UOM")
        'DO1.AddLabelLine("FROMQTY", "UOM Quantity")
        DO1.AddSpacer()
        DO1.AddTextboxLine("QTY", False, "next")
        SetConfirmation()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
    Public Sub SetConfirmation()
        Dim sCONFTYPE As String = getCONFIRMATIONTYPE()
        Session("CONFIRMATIONTYPE") = sCONFTYPE
        If Not sCONFTYPE Is Nothing Then
            Select Case sCONFTYPE
                Case Release.CONFIRMATIONTYPE.NONE
                Case Release.CONFIRMATIONTYPE.LOCATION
                    Session("CONFIRMATIONTYPE") = Release.CONFIRMATIONTYPE.LOCATION
                    DO1.AddTextboxLine("CONFIRMATION", False, "", "Confirm Location")
                Case Release.CONFIRMATIONTYPE.LOAD
                    Session("CONFIRMATIONTYPE") = Release.CONFIRMATIONTYPE.LOAD
                    DO1.AddTextboxLine("CONFIRMATION", False, "", "Confirm Load")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU
                    Session("CONFIRMATIONTYPE") = Release.CONFIRMATIONTYPE.SKU
                    DO1.AddTextboxLine("CONFIRMATION", False, "", "SKU")
                Case WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION
                    Session("CONFIRMATIONTYPE") = Release.CONFIRMATIONTYPE.SKULOCATION
                    DO1.AddTextboxLine("CONFIRMATION", False, "", "SKU")
                    DO1.AddTextboxLine("LOC", False, "", "Confirm Location")
            End Select
        Else
            Session("CONFIRMATIONTYPE") = Release.CONFIRMATIONTYPE.LOCATION
            DO1.AddTextboxLine("CONFIRMATION", False, "", "Confirm Location")
            Return
        End If

    End Sub
    Public Function getCONFIRMATIONTYPE() As String
        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
        Dim sql As String = $"select top 1 CONFTYPE from REPLPOLICYDETAIL where POLICYID='{brh.REPLENPOLICY}'"
        Dim sCONFTYPE As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Return sCONFTYPE
    End Function



End Class