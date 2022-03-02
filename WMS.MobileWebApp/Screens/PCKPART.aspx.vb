Imports WMS.Lib
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports System.Collections.Generic

<CLSCompliant(False)> Public Class PCKPART
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents lblUnits As Made4Net.WebControls.Label
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
    'Begin for RWMS-1294 and RWMS-1222
    Dim logger As WMS.Logic.LogHandler
    'Begin for RWMS-1294 and RWMS-1222

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oWHActivity As New WMS.Logic.WHActivity
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
            If oPicklist.ShouldPrintCaseLabel Then                       'RWMS-3822
                If Session("DefaultPrinter") = "" Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CASELABELPRINT.aspx"))
                End If
                ' Session("DefaultPrinter") = ""
                ' clearSession()
            End If

                If Session("MobileSourceScreen") Is Nothing Then
                If Not Request.QueryString("sourcescreen") Is Nothing Then
                    Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                Else
                    If Not Session("PCKBagOutPicking") Is Nothing Then
                        Session("MobileSourceScreen") = "PCKBagOut"
                    Else
                        Session("MobileSourceScreen") = "PCK"
                    End If

                End If
                MyBase.WriteToRDTLog("Source screen identified as : " & Session("MobileSourceScreen"))

            End If
            'Added for RWMS-2263 RWMS-2243
            Dim pcks1 As Picklist = Session("PCKPicklist")
            If (Session("PCKPicklist") Is Nothing) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
            End If
            Dim pcks As New Picklist(pcks1.PicklistID)
            If Not pcks Is Nothing Then
                Session("PCKPicklist") = pcks
            End If
            'Added for RWMS-2262 RWMS-2222
            If Not WMS.Logic.TaskManager.isUserAssignedPickListTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING, pcks.PicklistID) Then
                If WMS.Logic.TaskManager.isUserAssignedForDeliverTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, pcks.PicklistID) Then
                    MyBase.WriteToRDTLog("Delivery task is assigned to user so redirecting to DELLBLPRNT")

                    Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

                    If Not Session("PCKPicklist") Is Nothing Then
                        'Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                        If oPicklist.isCompleted Then
                            If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                                Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                                MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                                If Not Session("PCKPicklist") Is Nothing Then
                                    If oPicklist.ShouldPrintShipLabel Then
                                        oPicklist.PrintShipLabels(prntr.PrinterQName)
                                    End If
                                    PickTask.UpdateCompletionTime(oPicklist)
                                ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                                    pck2.PrintShipLabels(prntr.PrinterQName)
                                End If
                                If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                                    Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                                End If
                                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                            Else
                                Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                            End If
                        End If

                    End If

                Else
                    If Session("MobileSourceScreen") = "PCKPART" Then
                        If Not Session("PCKBagOutPicking") Is Nothing Then
                            Session("MobileSourceScreen") = "PCKBagOut"
                        Else
                            Session("MobileSourceScreen") = "PCK"
                        End If
                    End If
                End If
            End If

            Dim pck As PickJob
            Try
                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pcks.PicklistID)
                pck = PickTask.getNextPick(pcks)

            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.ToString())
                MyBase.WriteToRDTLog("Eror occured in PCKPART Load logic : " & ex.ToString)
            End Try
            setPick(pck)
            DO1.Value("UOMUNITS") = Session("UomUnits")
            DO1.Value("CONTAINER") = Session("PCKPicklistActiveContainerID")

            If Not Session("WeightNeededConfirm1") Is Nothing Then
                DO1.Value("CONFIRM") = Session("WeightNeededConfirm1")
                Session("WeightNeededConfirm1") = ""
                If Session("CONFTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                    If Not Session("WeightNeededConfirm2") Is Nothing Then
                        DO1.Value("CONFIRMSKU") = Session("WeightNeededConfirm2")
                        Session("WeightNeededConfirm2") = ""
                    End If
                End If
            End If


        End If
    End Sub

    Private Sub setPick(ByVal pck As PickJob)

        If pck Is Nothing Then
            MyBase.WriteToRDTLog(" No pickjob found ")
            If Session("PCKPicklist") IsNot Nothing Then
                Dim oPicklist As Picklist = Session("PCKPicklist")
                Dim pcklist As New Picklist(oPicklist.PicklistID)
                If Not pcklist Is Nothing Then
                    Session("PCKPicklist") = pcklist
                End If
                If Not Session("PCKBagOutPicking") Is Nothing Then
                    Dim tm As New WMS.Logic.TaskManager
                    If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, Nothing) Then
                        Dim TMTask As WMS.Logic.Task = tm.RequestTask(GetCurrentUser, WMS.Lib.TASKTYPE.PICKING)
                        Session.Remove("ShowedTaskManager")
                    End If
                    If pcklist.GetTotalPickedQty = 0 Then
                        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    End If

                    If pcklist.ShouldPrintBagOutReportOnComplete And pcklist.isCompleted Then
                        Response.Redirect(MapVirtualPath("Screens/PCKBAGOUTPRINT.aspx"))
                    Else
                        Response.Redirect(MapVirtualPath("screens/BagOutCloseContainer.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    End If
                ElseIf pcklist.GetTotalPickedQty > 0 Then

                    Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

                    If Not Session("PCKPicklist") Is Nothing Then
                        'Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                        If oPicklist.isCompleted Then
                            If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                                Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                                MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                                If Not Session("PCKPicklist") Is Nothing Then
                                    If oPicklist.ShouldPrintShipLabel Then
                                        oPicklist.PrintShipLabels(prntr.PrinterQName)
                                    End If
                                    PickTask.UpdateCompletionTime(oPicklist)
                                ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                                    pck2.PrintShipLabels(prntr.PrinterQName)
                                End If
                                If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                                    Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                                End If
                                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                            Else
                                Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                            End If
                        End If

                    End If

                Else
                    If Not Session("PCKBagOutPicking") Is Nothing Then
                        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    Else
                        Response.Redirect(MapVirtualPath("screens/taskmanager.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    End If

                End If
            End If
        Else
            MyBase.WriteToRDTLog(" Pickjob found ")
            'Ended for RWMS-1545 and RWMS-1442
            Session("PCKPicklistPickJob") = pck
            Dim pcks As Picklist = Session("PCKPicklist")
            DO1.Value("Picklist") = pck.picklist
            DO1.Value("LOADID") = pck.fromload
            DO1.Value("LOCATION") = pck.fromlocation
            DO1.Value("WAREHOUSEAREA") = pck.fromwarehousearea
            DO1.Value("SKU") = pck.sku
            DO1.Value("SKUDESC") = pck.skudesc
            'DO1.Value("UOM") = pck.uom
            Dim sqluom As String = " SELECT DESCRIPTION FROM CODELISTDETAIL " &
                          " WHERE CODELISTCODE = 'UOM' AND CODE = '" & pck.uom & "'"
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Session("UomUnits") = pck.uomunits
            DO1.Value("UOMUNITS") = Session("UomUnits")
            'Check For LowLimitCount
            If (Not String.IsNullOrEmpty(pck.fromload)) AndAlso WMS.Logic.Load.Exists(pck.fromload) Then
                Dim oLoad As New WMS.Logic.Load(pck.fromload)
                Dim oSku As New WMS.Logic.SKU(oLoad.CONSIGNEE, oLoad.SKU)
                If oLoad.UNITS <= 0 Then
                    Dim pckdet As PicklistDetail
                    For Each pckdet In pcks.Lines
                        If pckdet.FromLoad = pck.fromload And pckdet.UOM = pck.originaluom And pckdet.FromLocation = pck.fromlocation And pckdet.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And pckdet.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                            'Dim pcklst As New WMS.Logic.Picklist(pck.picklist)
                            pckdet.CompleteDetailPicking(pckdet.PickListLine, WMS.Logic.GetCurrentUser)

                        End If
                    Next
                    Dim pcklst As New WMS.Logic.Picklist(pck.picklist)
                    pck = PickTask.getNextPick(pcklst)
                    If Not pck Is Nothing Then
                        Session("PCKPicklist") = pcklst
                        clearSession()
                        setPick(pck)
                    Else
                        If Not Session("PCKBagOutPicking") Is Nothing Then
                            Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                        End If

                    End If
                End If
                DO1.Value("MANUFACTURESKU") = oSku.MANUFACTURERSKU
                If oLoad.UNITS <= oSku.LOWLIMITCOUNT And oLoad.LASTCOUNTDATE.Date < DateTime.Now.Date Then
                    'Redirect to load count and get back
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Load Units is less than low limit count - Please count load"))
                    Session("LoadCNTLoadId") = oLoad.LOADID
                    Session("LoadCountingSourceScreen") = "PCKPART"
                    Response.Redirect(MapVirtualPath("Screens/CNT2.aspx"))
                End If
            End If
        End If
    End Sub

    Private Sub doCloseContainer()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
            Try
                Dim pcklist As Picklist = Session("PCKPicklist")
                Dim relStrat As ReleaseStrategyDetail
                relStrat = pcklist.getReleaseStrategy()
                Dim pck As PickJob = Session("PCKPicklistPickJob")
                If Not relStrat Is Nothing Then
                    If relStrat.DeliverContainerOnClosing Then
                        'Should deliver the container now
                        pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), True, WMS.Logic.GetCurrentUser)
                        Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    Else
                        'Should close the container - go back to PCK to open a new one
                        pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), False, WMS.Logic.GetCurrentUser)
                        Session.Remove("PCKPicklistActiveContainerID")
                        If Not Session("PCKBagOutPicking") Is Nothing Then
                            Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                        End If
                    End If
                End If
            Catch ex As Threading.ThreadAbortException
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
                Return
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot Close Cotnainer - Container is blank"))
        End If
    End Sub

    Private Sub doNext()
        Dim pck As PickJob
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim pcklst1 As Picklist = Session("PCKPicklist")
        Dim pcklst As New Picklist(pcklst1.PicklistID)
        If Not pcklst Is Nothing Then
            Session("PCKPicklist") = pcklst
        End If

        pcklst.DefaultPrinter = Session("DefaultPrinter")
        If pcklst.isCompleted = False Then
            Session("PCKListToResume") = pcklst.PicklistID
        Else
            Session.Remove("PCKListToResume")
        End If
        Session("PCKPicklist") = pcklst
        pck = Session("PCKPicklistPickJob")
        pck.LabelPrinterName = Session("DefaultPrinter")

        Session("PCKPicklistPickJob") = pck
        Try
            If pck Is Nothing Then
                MyBase.WriteToRDTLog(" No pickjob found ")
                If Session("PCKPicklist") IsNot Nothing Then
                    Dim pcklist As Picklist = Session("PCKPicklist")
                    pcklist.Load()
                    If pcklist.isCompleted Then
                        Session.Remove("PCKListToResume")
                    Else
                        Session("PCKListToResume") = pcklist.PicklistID
                    End If
                End If
                If Not Session("PCKBagOutPicking") Is Nothing Then

                    If pcklst.GetTotalPickedQty = 0 Then
                        Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    End If
                    If pcklst.ShouldPrintBagOutReportOnComplete And pcklst.isCompleted Then
                        If pcklst.DefaultPrinter = "" Or pcklst.DefaultPrinter Is Nothing Then
                            Response.Redirect(MapVirtualPath("Screens/PCKBAGOUTPRINT.aspx"))
                        Else
                            Dim prntr As ReportPrinter = New ReportPrinter(Session("DefaultPrinter"))
                            MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                            pcklst.PrintBagOutReport(prntr.PrinterQName, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
                            Response.Redirect(MapVirtualPath("Screens/BagOutCloseContainer.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                        End If
                    Else
                        Response.Redirect(MapVirtualPath("screens/BagOutCloseContainer.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    End If
                Else
                    Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
                    If Not Session("PCKPicklist") Is Nothing Then
                        Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                        If oPicklist.isCompleted Then
                            If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                                Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                                MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                                If Not Session("PCKPicklist") Is Nothing Then
                                    If oPicklist.ShouldPrintShipLabel Then
                                        oPicklist.PrintShipLabels(prntr.PrinterQName)
                                    End If
                                    PickTask.UpdateCompletionTime(oPicklist)
                                ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                                    pck2.PrintShipLabels(prntr.PrinterQName)
                                End If
                                If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                                    Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                                End If
                                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                            Else
                                Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                            End If
                        End If

                    End If

                End If
            End If
            Dim sku As New SKU(pck.consingee, pck.sku)
            Dim UOMUNITS As Decimal = 0
            Try
                UOMUNITS = Math.Round(Decimal.Parse(DO1.Value("UOMUNITS")), 2)
                If UOMUNITS < 0 Then Throw New Exception()
            Catch ex As Exception
                DO1.Value("UOMUNITS") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Invalid UOMUNITS"))
                MyBase.WriteToRDTLog("Error occured as UOM Units invalid : " & ex.ToString)
                MyBase.WriteToRDTLog("Eror occured in PCKPART doCloseContainer logic : " & ex.ToString)
                Return
            End Try
            Try
                Session("PCKOldUomUnits") = pck.uomunits
                pck.pickedqty = sku.ConvertToUnits(pck.uom) * Convert.ToDecimal(DO1.Value("UOMUNITS"))
                pck.uomunits = DO1.Value("UOMUNITS")
                'sku = Nothing
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Error cannot get units"))
                MyBase.WriteToRDTLog(ex.ToString())
                MyBase.WriteToRDTLog("Error occured calculating SKU Units : " & ex.ToString)
                Return
            End Try

            Try
                If Not pcklst.Confirmed(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pck, DO1.Value("CONFIRMSKU")) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Confirmation"))
                    Return
                End If
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Confirmation"))
                MyBase.WriteToRDTLog(ex.ToString())
                Return
            End Try


            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pcklst.PicklistID)
            MyBase.WriteToRDTLog("Picklist : " & pcklst.PicklistID)
            Try
                ExtractAttributes()
                'added for RWMS-593/594 Start
                Dim dMFGDATEOrEXPDATE As DateTime
                Dim attributName As String
                attributName = Convert.ToString(Made4Net.Shared.ContextSwitch.Current.Session("AttributeName"))
                dMFGDATEOrEXPDATE = Convert.ToDateTime(Made4Net.Shared.ContextSwitch.Current.Session("MfgOrExpDate"))

                'Expiry date validation –
                'The system will compare the payload’s expiry date with the minimum ship days parameter in SKUATTRIBUTES table.
                'If the expiry date entered is sooner than <today + minimum ship days >, the validation will fail, and an error message will return


                If attributName = "EXPIRYDATE" Then
                    Dim dEXPIRYDATE As Date
                    dEXPIRYDATE = dMFGDATEOrEXPDATE
                    Dim iShipDay As Int16 = getShipDay(pck.consingee, pck.sku)

                    If DateTime.Compare(dEXPIRYDATE, DateTime.Now.AddDays(iShipDay)) < 0 Then

                        Dim msg As String = "Payload expiry date {0} is sooner than allowed to receive for this product ({1}).Valid Rule: EXPIRYDATE <(today + mindaystoship)"
                        msg = String.Format(msg, dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")), DateTime.Now.AddDays(iShipDay).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
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

                    Dim iShipDay As Int16 = getShipDay(pck.consingee, pck.sku)
                    Dim ishelflife As Int16 = getShelfLife(pck.consingee, pck.sku)
                    Dim isDaytoReceive As Int16 = getDayToReceive(pck.consingee, pck.sku)
                    If iShipDay > 0 Then
                        If dMFGDATE.AddDays(ishelflife).Subtract(DateTime.Now.Date).TotalDays < iShipDay Then
                            Dim msg As String = "Payload manufacture date {0} is older than allowed to receive for this product ({1}. Valid Rule: (mfgdate + shelflife) - today >= mindaystoship)"
                            msg = String.Format(msg, dMFGDATE.ToString("MM/dd/yyyy"), DateTime.Now.Date.AddDays(iShipDay).AddDays(-ishelflife).ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat")))
                            Throw New ApplicationException(t.Translate(msg))
                            Return
                        End If
                    Else
                        Throw New ApplicationException(t.Translate("Mindaystoship must be greater than zero"))
                        Return
                    End If
                End If


                Made4Net.Shared.ContextSwitch.Current.Session("MfgOrExpDate") = Nothing
                Made4Net.Shared.ContextSwitch.Current.Session("AttributeName") = Nothing

                'added for RWMS-593/594 End

                Session.Add("TaskID", tm.getAssignedTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()).TASK)

                pck.container = Session("PCKPicklistActiveContainerID") 'DO1.Value("CONTAINER")

                MyBase.WriteToRDTLog(" Passing the containerid session value to the pick object container varible " & pck.container)
                MyBase.WriteToRDTLog(" This is the first time the container will be assigned to the picklist if the picklist does not have any containerid. Picklist id: " & pcklst.PicklistID & " SKU : " & pck.sku)

                If pck.units > pck.pickedqty Then
                    MyBase.WriteToRDTLog("pck.units > pck.pickedqty")
                    Session("WeightNeededPickJob") = pck
                    Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                    Session("WeightNeededConfirm2") = DO1.Value("CONFIRMSKU")

                    Session("PCKPicklist") = pcklst
                    Response.Redirect(MapVirtualPath("screens/PCKPickShort.aspx?sourcescreen=PCKPART"))
                End If

                If Session("CONFTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                    If SKU.weightNeeded(sku) Then
                        If pck.oSku Is Nothing Then
                            pck.oSku = New WMS.Logic.SKU(pck.consingee, pck.sku)
                        End If
                        If pck.uomunits > 0 AndAlso pck.oSku.OVERPICKPCT > 0 AndAlso pck.oSku.OVERPICKPCT * pck.units >= pck.pickedqty Then
                            Session("WeightNeededPickJob") = pck
                            Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                            Session("WeightNeededConfirm2") = DO1.Value("CONFIRMSKU")
                            Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=PCKPART"))
                        Else
                            logger.SafeWrite("Picklist.Pick : SKU OVERPICKPCT : " & pck.oSku.OVERPICKPCT)
                            Throw New M4NException(New Exception(), "Cannot pick line, Invalid quantities in pick line or order line", "Cannot pick line, Invalid quantities in pick line or order line")
                        End If
                    End If
                    pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM"), DO1.Value("CONFIRMSKU"), pck.fromwarehousearea)

                    'Added for RWMS-1545 and RWMS-1442

                    MyBase.WriteToRDTLog(" Started Picking.. ")

                    'Ended for RWMS-1545 and RWMS-1442

                    Dim key As String = pck.oAttributeCollection("WEIGHT")
                    If sku IsNot Nothing And pck IsNot Nothing Then
                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                            Dim netWeight As Decimal = SKU.GetNetWeight(pck.sku)
                            If Not key Is Nothing Then
                                pck.oAttributeCollection("WEIGHT") = netWeight * pck.uomunits
                            Else
                                pck.oAttributeCollection.Add("WEIGHT", netWeight * pck.uomunits)
                            End If
                        End If

                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                            Dim grossWeight As Decimal = SKU.GetGrossWeight(pck.sku)
                            If Not key Is Nothing Then
                                pck.oAttributeCollection("WEIGHT") = grossWeight * pck.uomunits
                            Else
                                pck.oAttributeCollection.Add("WEIGHT", grossWeight * pck.uomunits)
                            End If
                        End If
                    End If

                    pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                    '' RWMS-2487 : Reload the Task in Session("TMTask"), since if the above statement completes the task(Case of shortpick), the Session wont have updated state.
                    Dim task As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select TASK from tasks where picklist = '{0}' and TASKTYPE = 'PARPICK'", pcklst.PicklistID))
                    Session("TMTask") = New Task(task)

                    If sku IsNot Nothing And Session("PCKPicklistPickJob") IsNot Nothing Then
                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                            Dim netWeight As Decimal = SKU.GetNetWeight(sku.SKU)
                            Dim newPck As PickJob = Session("PCKPicklistPickJob")
                            InsertCasesWeight(newPck, netWeight)
                            Dim totalWeight As Decimal = CalculatTotalWeight(newPck, netWeight)
                        End If

                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                            Dim grossWeight As Decimal = SKU.GetGrossWeight(sku.SKU)
                            Dim newPck As PickJob = Session("PCKPicklistPickJob")
                            InsertCasesWeight(newPck, grossWeight)
                            Dim totalWeight As Decimal = CalculatTotalWeight(newPck, grossWeight)
                        End If
                    End If
                    MyBase.WriteToRDTLog(" Picking Completed. ")
                    clearSession()
                Else
                    If SKU.weightNeeded(sku) Then
                        If pck.oSku Is Nothing Then
                            pck.oSku = New WMS.Logic.SKU(pck.consingee, pck.sku)
                        End If
                        If pck.uomunits > 0 AndAlso pck.oSku.OVERPICKPCT > 0 AndAlso pck.oSku.OVERPICKPCT * pck.units >= pck.pickedqty Then
                            Session("WeightNeededPickJob") = pck
                            Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                            Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=PCKPART"))
                        Else
                            logger.SafeWrite("Picklist.Pick : SKU OVERPICKPCT : " & pck.oSku.OVERPICKPCT)
                            Throw New M4NException(New Exception(), "Cannot pick line, Invalid quantities in pick line or order line", "Cannot pick line, Invalid quantities in pick line or order line")
                        End If
                    End If
                    pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM").Trim(), "", DO1.Value("WAREHOUSEAREA").Trim())
                    MyBase.WriteToRDTLog(" Started Picking.. ")
                    If pck.oAttributeCollection Is Nothing Then
                        pck.oAttributeCollection = New WMS.Logic.AttributesCollection
                    End If
                    Dim key As String = pck.oAttributeCollection("WEIGHT")

                    If sku IsNot Nothing And pck IsNot Nothing Then
                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                            Dim netWeight As Decimal = SKU.GetNetWeight(pck.sku)
                            If Not key Is Nothing Then
                                pck.oAttributeCollection("WEIGHT") = netWeight * pck.uomunits
                            Else
                                pck.oAttributeCollection.Add("WEIGHT", netWeight * pck.uomunits)
                            End If
                        End If

                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                            Dim grossWeight As Decimal = SKU.GetGrossWeight(pck.sku)
                            If Not key Is Nothing Then
                                pck.oAttributeCollection("WEIGHT") = grossWeight * pck.uomunits
                            Else
                                pck.oAttributeCollection.Add("WEIGHT", grossWeight * pck.uomunits)
                            End If
                        End If
                    End If

                    'Ended for RWMS-1545 and RWMS-1442
                    pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                    '' RWMS-2487 : Reload the Task in Session("TMTask"), since if the above statement completes the task(Case of shortpick), the Session wont have updated state.
                    Dim task As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select TASK from tasks where picklist = '{0}' and TASKTYPE = 'PARPICK'", pcklst.PicklistID))
                    Session("TMTask") = New Task(task)

                    If sku IsNot Nothing And Session("PCKPicklistPickJob") IsNot Nothing Then
                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                            Dim netWeight As Decimal = SKU.GetNetWeight(sku.SKU)
                            Dim newPck As PickJob = Session("PCKPicklistPickJob")
                            InsertCasesWeight(newPck, netWeight)
                            Dim totalWeight As Decimal = CalculatTotalWeight(newPck, netWeight)

                        End If

                        If sku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                            Dim grossWeight As Decimal = SKU.GetGrossWeight(sku.SKU)
                            Dim newPck As PickJob = Session("PCKPicklistPickJob")
                            InsertCasesWeight(newPck, grossWeight)
                            Dim totalWeight As Decimal = CalculatTotalWeight(newPck, grossWeight)

                        End If
                    End If
                    MyBase.WriteToRDTLog(" Picking Completed. ")
                    clearSession()
                End If
                Session("PCKPicklist") = pcklst
                Session("DefaultPrinter") = pcklst.DefaultPrinter
                If Not Session("PCKBagOutPicking") Is Nothing Then
                    Dim printer As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("Select isnull(printer,'') as printer from WHACTIVITY where userid = '{0}'", WMS.Logic.Common.GetCurrentUser()))
                    pcklst.DefaultPrinter = printer
                    ' Session("DefaultPrinter") = printer
                End If
            Catch ex As Threading.ThreadAbortException
                MyBase.WriteToRDTLog(ex.ToString())
            Catch ex As Made4Net.Shared.M4NException
                clearSession()
                Session("UomUnits") = pck.uomunits
                DO1.Value("UOMUNITS") = Session("UomUnits")
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                MyBase.WriteToRDTLog("pick units : " & pck.uomunits & " uom units : " & DO1.Value("UOMUNITS"))
                Return
            Catch ex As Exception
                clearSession()
                Session("UomUnits") = pck.uomunits
                DO1.Value("UOMUNITS") = Session("UomUnits")
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
                MyBase.WriteToRDTLog("Error occured in PCKPART doNext Logic : " & ex.ToString)
                Return
            End Try

            ClearAttributes()
            DO1.Value("CONFIRM") = ""
            DO1.Value("CONFIRMSKU") = ""
            If pcklst.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=PCKPART"))
            End If
            MyBase.WriteToRDTLog("Assigning Container Delivery Task to the user : " & WMS.Logic.GetCurrentUser)
            MyBase.WriteToRDTLog("Assigned Delivery Task.")
            setPick(pck)
        Catch ex As System.Threading.ThreadAbortException
            MyBase.WriteToRDTLog(ex.ToString())
        End Try

    End Sub
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

    Private Sub clearSession()
        DO1.Value("CONFIRM") = ""
        DO1.Value("CONFIRMSKU") = ""
        Session("WeightNeededPickJob") = ""
        Session("WeightNeededConfirm1") = ""
        Session("WeightNeededConfirm2") = ""

    End Sub

    Private Sub doBack()
        Session.Remove("CONFTYPE")
        Session.Remove("UomUnits")
        Session.Remove("PCKListToResume")
        Session.Remove("PickListID")
        Dim pcklst As Picklist = Session("PCKPicklist")
        
        Try
            If Session("MobileSourceScreen") Is Nothing Then
                If Not Session("PCKBagOutPicking") Is Nothing Then
                    Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx"))
                Else
                    Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                End If
            Else
                Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
            End If
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Session.Remove("CONFTYPE")
        Session.Remove("UomUnits")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(pck.consingee, pck.sku).SKUClass

                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    'Added for RWMS-2372 - empty check for objSkuClass
                    If Not objSkuClass Is Nothing Then
                        Dim oAtt As WMS.Logic.SkuClassLoadAttribute = objSkuClass.LoadAttributes(pck.oAttributeCollection.Keys(idx))
                        'Added for RWMS-2372 - empty check for oAtt
                        If Not oAtt Is Nothing Then
                            If oAtt.Name.ToUpper <> "WEIGHT" And oAtt.Name.ToUpper <> "WGT" Then
                                If oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                                    DO1.AddTextboxLine(pck.oAttributeCollection.Keys(idx), True, "Next")
                                Else
                                    DO1.AddTextboxLine(pck.oAttributeCollection.Keys(idx))
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        End If
        DO1.AddTextboxLine("CONFIRM", True, "next")
        DO1.AddTextboxLine("CONFIRMSKU")
        DO1.AddTextboxLine("UOMUNITS", True, "next", "UOMUNITS", "")
        Dim pcklist As Picklist = Session("PCKPicklist") 'New Picklist(pck.picklist)
        Dim relStrat As ReleaseStrategyDetail
        relStrat = pcklist.getReleaseStrategy()
        If Not relStrat Is Nothing Then
            Session("CONFTYPE") = relStrat.ConfirmationType
            If relStrat.ConfirmationType = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                DO1.setVisibility("CONFIRMSKU", True)
            Else
                DO1.setVisibility("CONFIRMSKU", False)
            End If
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Try
            Select Case e.CommandText.ToLower
                Case "next"
                    doNext()
                Case "back"
                    doBack()
                Case "close container"
                    Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                    If Not Session("PCKBagOutPicking") Is Nothing Then
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/BagOutCloseContainer.aspx?sourcescreen=PCKPART"))
                    ElseIf Not Session("PCKPicklistActiveContainerID") Is Nothing Then
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKCLOSECONTAINER.aspx?sourcescreen=PCKPART"))
                    Else
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot Close Cotnainer - Container is blank"))
                    End If
            End Select
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        Dim Val As Object
        Dim oSku As String = pck.sku
        Dim oConsignee As String = pck.consingee
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If objSkuClass Is Nothing Then Return Nothing
        Dim oAttCol As New WMS.Logic.AttributesCollection
        For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
            Dim req As Boolean = False
            If oAtt.Name.ToUpper <> "WEIGHT" And oAtt.Name.ToUpper <> "WGT" Then
                If oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                    Try
                        Select Case oAtt.Type
                            Case Logic.AttributeType.Boolean
                                Val = CType(DO1.Value(oAtt.Name), Boolean)
                            Case Logic.AttributeType.DateTime
                                'val = CType(DO1.Value(oAtt.Name), DateTime)
                                Val = DateTime.ParseExact(DO1.Value(oAtt.Name), Made4Net.Shared.AppConfig.DateFormat, Nothing)
                                'added for RWMS-593/594 Start
                                Made4Net.Shared.ContextSwitch.Current.Session("AttributeName") = oAtt.Name
                                Made4Net.Shared.ContextSwitch.Current.Session("MfgOrExpDate") = Val
                                'added for RWMS-593/594 End
                            Case Logic.AttributeType.Decimal
                                Val = CType(DO1.Value(oAtt.Name), Decimal)
                            Case Logic.AttributeType.Integer
                                Val = CType(DO1.Value(oAtt.Name), Int32)
                            Case Else
                                Val = DO1.Value(oAtt.Name)
                        End Select
                    Catch ex As Exception
                        If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Attribute Validation failed for " & oAtt.Name, "Attribute Validation failed for " & oAtt.Name)
                        End If
                    End Try
                End If
            End If
        Next
        If pck IsNot Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    If Val = "" Then Val = Nothing
                    pck.oAttributeCollection(idx) = Val
                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function

    Private Sub ClearAttributes()
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    DO1.Value(pck.oAttributeCollection.Keys(idx)) = ""
                Next
            End If
        End If
    End Sub

    
    Private Sub updateFromLoadWeight(pckJob As WMS.Logic.PickJob, totalWeight As Decimal)
        Dim newWeight As Decimal
        Dim LD As New Load(pckJob.fromload)
        Try
            If Not IsNumeric(LD.GetAttribute("WEIGHT")) Then
                newWeight = 0
            ElseIf Decimal.Parse(LD.GetAttribute("WEIGHT") - totalWeight) < 0 Then
                newWeight = 0
            Else
                newWeight = Decimal.Parse(LD.GetAttribute("WEIGHT") - totalWeight)
            End If

            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", newWeight, LD.LOADID)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        Catch ex As Exception
            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "can't update flomload weight")
        End Try
    End Sub

    Private Function CalculatTotalWeight(pckJob As WMS.Logic.PickJob, weight As Decimal) As Decimal
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        Dim sqlUomNum As String = "select ISNULL(max(UOMNUM),0) from LOADDETWEIGHT where LOADID='{0}'"
        Dim sqlToLoad As String
        Dim dt As New DataTable
        Dim toload As String
        Dim units As Decimal
        Dim SumWeight As Decimal
        Dim AllLines As String = ""

        Try
            For iLine As Integer = 0 To pckJob.PickDetLines.Count - 1
                If AllLines = "" Then
                    AllLines = "'" & pckJob.PickDetLines.Item(iLine) & "'"
                Else
                    AllLines = AllLines & ",'" & pckJob.PickDetLines.Item(iLine) & "'"
                End If
            Next
            sqlToLoad = "select toload, units from vPartialPickGetToLoads where picklist='{0}' and sku='{1}' and picklistline in({2})"
            sqlToLoad = String.Format(sqlToLoad, pckJob.picklist, pckJob.sku, AllLines)

            Made4Net.DataAccess.DataInterface.FillDataset(sqlToLoad, dt)

            Dim ld As WMS.Logic.Load

            For Each dr As DataRow In dt.Rows

                toload = dr("toload")
                ld = New Load(toload)
                units = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format(sqlUomNum, toload)))
                SumWeight = SumWeight + weight * units
            Next
            Return SumWeight
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Sub InsertCasesWeight(pckJob As WMS.Logic.PickJob, weight As Decimal)

        'Dim logger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        Dim user As String = WMS.Logic.GetCurrentUser

        Dim sql As String = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER, UCC128, AICODE, UCC128Date) values"

        Dim sqlUomNum As String = "select ISNULL(max(UOMNUM),0) from LOADDETWEIGHT where LOADID='{0}'"
        Dim sqlToLoad As String
        Dim dt As New DataTable
        Dim toload As String
        Dim units As Decimal
        Dim casesListCounter As Integer = 0
        Dim SumWeight As Decimal
        Dim AllLines As String = ""

        Try

            For iLine As Integer = 0 To pckJob.PickDetLines.Count - 1
                If AllLines = "" Then
                    AllLines = "'" & pckJob.PickDetLines.Item(iLine) & "'"
                Else
                    AllLines = AllLines & ",'" & pckJob.PickDetLines.Item(iLine) & "'"
                End If
            Next
            sqlToLoad = "select toload, units from vPartialPickGetToLoads where picklist='{0}' and sku='{1}' and picklistline in({2})"
            sqlToLoad = String.Format(sqlToLoad, pckJob.picklist, pckJob.sku, AllLines)

            If Not logger Is Nothing Then
                logger.Write("sqlToLoad :" + sqlToLoad)
            End If


            Made4Net.DataAccess.DataInterface.FillDataset(sqlToLoad, dt)

            Dim ld As WMS.Logic.Load
            Dim loadArrayList As New List(Of String)() ''RWMS-1315 Attribute table update wrong for partial pick weight
            If Not logger Is Nothing Then
                logger.Write("Start Outer for loop...")
            End If

            For Each dr As DataRow In dt.Rows

                If Not logger Is Nothing Then
                    logger.Write("Iterating the LOADS...")
                End If

                SumWeight = 0

                toload = dr("toload")
                If Not loadArrayList.Contains(toload) Then
                    loadArrayList.Add(toload)
                End If

                ld = New Load(toload)

                units = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format(sqlUomNum, toload)))

                If Not logger Is Nothing Then
                    logger.Write("Start Inner for loop...")
                End If

                For i As Integer = units To units + ld.UOMUnits - 1

                    If Not logger Is Nothing Then
                        logger.Write("Iterating the LOAD UOM units...")
                    End If

                    If casesListCounter = 0 Then
                        sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}', '{5}', '{6}', NULL) "
                    Else
                        sql += ", ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', '{5}', '{6}', NULL) "
                    End If
                    sql = String.Format(sql, toload, pckJob.uom, i + 1, weight, user, "", "", "")


                    If Not logger Is Nothing Then
                        logger.Write("sql :" + sql)
                    End If

                    casesListCounter = casesListCounter + 1

                    If Not logger Is Nothing Then
                        logger.Write("casesListCounter :" + casesListCounter.ToString())
                    End If

                Next

                If Not logger Is Nothing Then
                    logger.Write("Finished Iterating the LOAD UOM units.")
                    logger.Write("EXIT Inner for loop.")
                End If

            Next

            If Not logger Is Nothing Then
                logger.Write("Finished Iterating the LOADS.")
                logger.Write("EXIT outer for loop.")
            End If
            If Not logger Is Nothing Then
                logger.Write("Started updating LOADDETWEIGHT...")
                logger.Write("Final sql : " + sql)
            End If

            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

            If Not logger Is Nothing Then
                logger.Write("Finished updating LOADDETWEIGHT.")
            End If

            'Start RWMS-1315 - Attribute table update wrong for partial pick weight

            If Not logger Is Nothing Then
                logger.Write("Started updating attribute load weight...")
            End If

            Dim strToLoadid As String
            For Each strToLoadid In loadArrayList
                updateAttToLoadWeight(strToLoadid)
            Next
            'END RWMS-1315 - Attribute table update wrong for partial pick weight

            If Not logger Is Nothing Then
                logger.Write("Finished updating attribute load weight.")
            End If

        Catch ex As Exception
            If Not logger Is Nothing Then
                logger.Write("error :" + ex.ToString())
            End If
        End Try
    End Sub
    Private Sub updateAttToLoadWeight(ByVal strToLoadid As String)
        'Dim logger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        Try

            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT=(SELECT SUM(UOMWEIGHT) FROM LOADDETWEIGHT WHERE LOADID='{0}') WHERE PKEY1='{0}' AND PKEYTYPE='LOAD'", strToLoadid)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

            If Not logger Is Nothing Then
                logger.Write("SQL :" + SQL)
            End If

        Catch ex As Exception

            If Not logger Is Nothing Then
                logger.Write("error :" + ex.ToString())

            End If

        End Try
    End Sub



End Class