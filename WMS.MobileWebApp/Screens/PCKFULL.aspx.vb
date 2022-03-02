Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections.Generic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKFULL
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject
    'Added for RWMS-323
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
    'End Added for RWMS-323

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
    'UnCommented for RWMS-202
    Public Shared gs As Barcode128GS.GS128
    'End UnCommented for RWMS-202

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then

            'start for 1420 and RWMS-1412


            MyBase.WriteToRDTLog(" Flow Navigated to page PCKFULL ")

            'end for 1420 and RWMS-1412

            'UnCommented for RWMS-323
            gs = New Barcode128GS.GS128()
            'End UnCommented for RWMS-323

            If String.IsNullOrEmpty(Session("MobileSourceScreen")) Then
                If Not Request.QueryString("sourcescreen") Is Nothing Then
                    Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                Else
                    Session("MobileSourceScreen") = "PCK"
                End If
            End If

            'Commented for RWMS-510
            'If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '    Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            'ElseIf Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING) Then
            '    'Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            '    Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
            'End If
            'End Commented for RWMS-510
            'Added for RWMS-510

            'Added for RWMS-2263 RWMS-2243
            Dim pcks1 As Picklist = Session("PCKPicklist")

            Dim pcks As New Picklist(pcks1.PicklistID)
            If Not pcks Is Nothing Then
                Session("PCKPicklist") = pcks
            End If

            'End Added RWMS-2263 RWMS-2243

            'Commented for RWMS-2262 RWMS-2222
            'If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING) Then
            '    If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '        'Add code to check for the release strategy configuration
            '        Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            '    Else
            '        If Session("MobileSourceScreen") = "PCKFULL" Then
            '            Session("MobileSourceScreen") = "PCK"
            '        End If
            '        Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
            '    End If
            'End If
            'End Added for RWMS-510
            'End Commented for RWMS-2262 RWMS-2222

            'Added for RWMS-2262 RWMS-2222
            If Not WMS.Logic.TaskManager.isUserAssignedPickListTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING, pcks.PicklistID) Then
                If WMS.Logic.TaskManager.isUserAssignedForDeliverTask(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, pcks.PicklistID) Then
                    'Add code to check for the release strategy configuration

                    Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

                    If Not Session("PCKPicklist") Is Nothing Then
                        Dim oPicklst As WMS.Logic.Picklist = Session("PCKPicklist")
                        Dim oPicklist As New Picklist(oPicklst.PicklistID)
                        If Not oPicklist Is Nothing Then
                            Session("PCKPicklist") = oPicklist
                        End If
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
                    If Session("MobileSourceScreen") = "PCKFULL" Then
                        Session("MobileSourceScreen") = "PCK"
                    End If
                    Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
                End If
            End If
            'End Added for RWMS-2262 RWMS-2222

            'Commented for RWMS-2263 RWMS-2243
            'Dim pcks As Picklist = Session("PCKPicklist")
            'End Commented for RWMS-2263 RWMS-2243

            Dim pck As PickJob
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pcks.PicklistID)
            Try
                pck = PickTask.getNextPick(pcks)
            Catch ex As Exception
            End Try
            setPick(pck)

            If Not Session("WeightNeededConfirm1") Is Nothing Then
                DO1.Value("CONFIRMPAYLOADID") = Session("WeightNeededConfirm1")
                Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
                If oSku.IsWeightCaptureRequiredAtPicking AndAlso Not oSku.HasShippingWeightCaptureMethod Then
                    Try
                        DO1.FocusField = "WEIGHT"
                    Catch ex As Exception
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WLTaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        'Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
        Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
    End Sub

    Private Sub setPick(ByVal pck As PickJob)
        'start for 1420 and RWMS-1412


        MyBase.WriteToRDTLog(" Doing setPick operation in page PCKFULL ")

        'end for 1420 and RWMS-1412

        If pck Is Nothing Then
            'start for 1420 and RWMS-1412

            MyBase.WriteToRDTLog(" No pickjob found ")

            'end for 1420 and RWMS-1412

            'Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))

            Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

            If Not Session("PCKPicklist") Is Nothing Then
                Dim oPcklst As WMS.Logic.Picklist = Session("PCKPicklist")
                Dim oPicklist As New Picklist(oPcklst.PicklistID)
                If Not oPicklist Is Nothing Then
                    Session("PCKPicklist") = oPicklist
                End If
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
            Session("PCKPicklistPickJob") = pck
            Dim pcks As Picklist = Session("PCKPicklist")
            DO1.Value("LOADID") = pck.fromload
            DO1.Value("LOCATION") = pck.fromlocation
            'RWMS-383: Begin
            'DO1.Value("PICKMETHOD") = pcks.PickMethod
            'RWMS-383: End
            DO1.Value("WAREHOUSEAREA") = pck.fromwarehousearea
            DO1.Value("PICKTYPE") = pcks.PickType
            DO1.Value("SKU") = pck.sku
            DO1.Value("SKUDESC") = pck.skudesc

            'Added for RWMS-755
            Dim sqluom As String = " SELECT top 1 COMPANYNAME FROM vPicklistCompanyName " &
                                      " WHERE PICKLIST = '" & pck.picklist & "'"
            DO1.Value("COMPANYNAME") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
            'End Added for RWMS-755

            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            'DO1.Value("UOMUNITS") = "(" & pck.units & ")" & " " & trans.Translate(pck.uom) & " " & pck.uomunits
            DO1.Value("QTY") = "(" & pck.units & ")" & " " & trans.Translate(pck.uom) & " " & pck.uomunits
            DO1.setVisibility("WAREHOUSEAREA", False)
            If pck.SystemPickShort Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("System Pick Short- Quantity available is less than quantity required / Wrong Location!"))
            End If
            'Label Printing
            'Begin RWMS-573 - Commented
            'If pcks.ShouldPrintShipLabelOnPickLineCompleted Then
            '    If MobileUtils.LoggedInMHEID <> String.Empty And MobileUtils.GetMHEDefaultPrinter <> String.Empty Then
            '        DO1.setVisibility("PRINTER", False)
            '    Else
            '        DO1.setVisibility("PRINTER", True)
            '    End If
            'Else
            '    DO1.setVisibility("PRINTER", False)
            'End If
            'END RWMS-573 - Commented
            'Fill the problem code drop down if operation allowed
            Dim dd1 As Made4Net.WebControls.MobileDropDown
            dd1 = DO1.Ctrl("TaskProblemCode")
            'dd1.AllOption = False
            dd1.AllOption = True
            dd1.AllOptionText = ""
            dd1.TableName = "vTaskTypesProblemCodes"
            dd1.ValueField = "PROBLEMCODEID"
            dd1.TextField = "PROBLEMCODEDESC"
            dd1.Where = "TASKTYPE = '" & WMS.Lib.TASKTYPE.FULLPICKING & "'"
            dd1.DataBind()
            Try
                If dd1.GetValues.Count > 0 Then
                    DO1.setVisibility("TaskProblemCode", True)
                Else
                    DO1.setVisibility("TaskProblemCode", False)
                End If
            Catch ex As Exception
                DO1.setVisibility("TaskProblemCode", False)
            End Try

            Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
            If oSku.IsWeightCaptureRequiredAtPicking AndAlso Not oSku.HasShippingWeightCaptureMethod Then
                Dim ld As New Load(pck.fromload)
                Try

                    If Math.Round(ld.GetAttribute("WEIGHT"), 2) > 0 Then DO1.Value("WEIGHT") = Math.Round(ld.GetAttribute("WEIGHT"), 2)
                Catch ex As Exception
                    DO1.Value("WEIGHT") = 0
                End Try
            Else
                DO1.setVisibility("WEIGHT", False)
            End If
        End If
    End Sub

    Private Function GetWeightPerCase(ByVal oSku As WMS.Logic.SKU, ByVal units As Integer, ByVal currentWeight As Decimal) As Decimal
        Dim d As Decimal = 0

        Try
            '            d = Math.Round(ld.GetAttribute("WEIGHT") / oSku.ConvertUnitsToUom("CASE", units), 2)
            '            d = Math.Round(DO1.Value("WEIGHT") / oSku.ConvertUnitsToUom("CASE", units), 2)
            d = Math.Round(currentWeight / oSku.ConvertUnitsToUom("CASE", units), 2)
        Catch ex As Exception
            d = 0
        End Try
        Return d
    End Function


    'Commented for RWMS-202
    'Private Sub DoNext()
    'End Commented for RWMS-202
    'Added for RWMS-202
    Private Sub DoNext(Optional ByVal bWeightCapture As Boolean = False)
        'End Added for RWMS-202
        Dim pck As PickJob
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'Commented for RWMS-2263 RWMS-2243
        'Dim pcklst As Picklist = Session("PCKPicklist")
        'pck = Session("PCKPicklistPickJob")
        'Commented for RWMS-2263 RWMS-2243

        'Added for RWMS-1643 and RWMS-745
        Dim pcklst1 As Picklist = Session("PCKPicklist")
        Dim pcklst As New Picklist(pcklst1.PicklistID)
        pck = Session("PCKPicklistPickJob")
        'End Added for RWMS-1643 and RWMS-745



        Try

            'RWMS-2752 Commented
            ''Added for PWMS-367
            ''Insert Total gross weight to database
            'If Not isWeightCaptureNeeded Then
            '    'RWMS-2736 Commented
            '    'InsertGrossWeight()
            '    'RWMS-2736 Commented END
            '    'RWMS-2736
            '    If Not pck Is Nothing Then
            '        Dim objSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
            '        If weightNeeded(objSku) Then
            '            InsertGrossWeight()
            '        End If
            '    End If
            '    'RWMS-2736 END
            'End If
            ''Added for PWMS-367
            'RWMS-2752 Commented END

            'RWMS-2752
            Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
            Dim currentWeight As Decimal = 0
            If Not ForceWeightCaptureInFullPick Then
                If Not pck Is Nothing Then
                    If oSku.IsWeightCaptureRequiredAtPicking AndAlso Not oSku.HasShippingWeightCaptureMethod Then
                        Dim err, barcode, ret As String

                        barcode = DO1.Value("WEIGHT")
                        ret = gs.getWeight(barcode, err)
                        If ret = 1 Then
                            DO1.Value("WEIGHT") = barcode
                            currentWeight = barcode
                        Else
                            DO1.Value("WEIGHT") = ""
                            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ret & " " & err)
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please enter valid weight"))
                            Return
                        End If

                        If Not IsNumeric(DO1.Value("WEIGHT")) Then
                            DO1.Value("WEIGHT") = ""
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please enter valid weight"))
                            Return
                        ElseIf (DO1.Value("WEIGHT") = 0) Then
                            DO1.Value("WEIGHT") = ""
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please enter valid weight"))
                            Return
                        ElseIf (DO1.Value("WEIGHT") < 0) Then
                            DO1.Value("WEIGHT") = ""
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please enter valid weight. Weight cannot be Negative."))
                            Return
                        End If
                    End If
                End If
            End If
            'RWMS-2752 END

            If pck Is Nothing Then
                'Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))

                Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

                If Not Session("PCKPicklist") Is Nothing Then
                    Dim oPcklst As WMS.Logic.Picklist = Session("PCKPicklist")
                    Dim oPicklist As New Picklist(oPcklst.PicklistID)
                    If Not oPicklist Is Nothing Then
                        Session("PCKPicklist") = oPicklist
                    End If
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
            pck.pickedqty = pck.units
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pck.picklist)

            If MobileUtils.LoggedInMHEID.Trim <> String.Empty Then
                pck.LabelPrinterName = MobileUtils.GetMHEDefaultPrinter
            End If



            Try
                'Commented for RWMS-202
                'If Not pcklst.Confirmed(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pck) Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Wrong Confirmation"))
                '    Return
                'End If
                'End Commented for RWMS-202
                'Added for RWMS-202
                If pck.TaskConfirmation.ConfirmationType = WMS.Lib.CONFIRMATIONTYPE.SKULOCATION Then
                    If Not pcklst.Confirmed(DO1.Value("CONFIRMPAYLOADID"), DO1.Value("WAREHOUSEAREA"), pck, DO1.Value("CONFIRM(SKU)").Trim()) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Wrong Confirmation"))
                        Return
                    End If
                Else
                    If Not pcklst.Confirmed(DO1.Value("CONFIRMPAYLOADID"), DO1.Value("WAREHOUSEAREA"), pck) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Wrong Confirmation"))
                        Return
                    End If
                End If
                'End Added for RWMS-202
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                'Start RWMS-1326

                MyBase.WriteToRDTLog(ex.ToString())

                'End RWMS-1326

                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
                'Start RWMS-1326

                MyBase.WriteToRDTLog(ex.ToString())

                'End RWMS-1326

                Return
            End Try

            'RWMS-2752 Commented
            'Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
            'Dim currentWeight As Decimal = 0
            'RWMS-2752 Commented END

            'Added for RWMS-202
            'End Added for RWMS-202
            If ForceWeightCaptureInFullPick AndAlso oSku.IsWeightCaptureRequiredAtPicking AndAlso Not oSku.HasShippingWeightCaptureMethod Then
                pck.oAttributeCollection = ExtractAttributes(currentWeight)
                Session("PCKPicklistPickJob") = pck
                Session("WeightNeededPickJob") = pck
                Session("WeightNeededConfirm1") = DO1.Value("CONFIRMPAYLOADID")
                Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=PCKFULL&frompckweightneeded=1"))
            End If
            'End Added for RWMS-202
            'Commented for RWMS-202
            'If weightNeeded(oSku) Then
            '    Dim err, barcode, ret As String

            '    barcode = DO1.Value("WEIGHT")
            '    'ret = gs.getWeight(barcode, err)
            '    'If ret = 1 Then
            '    currentWeight = barcode
            '    'Else
            '    '    DO1.Value("WEIGHT") = ""
            '    '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret & " " & err)
            '    '    Return
            '    'End If

            '    'Try
            '    '    currentWeight = DO1.Value("WEIGHT")
            '    '    If currentWeight <= 0 Then Throw New Exception()
            '    'Catch ex As Exception
            '    '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Invalid Weight"))
            '    '    Return
            '    'End Try


            '    'added validation code for weight
            '    If Not VALIDATEWEIGHT(pck, currentWeight) Then
            '        'VALIDATION NOT PASSED; REDIRECTED TO OVERIDE SCREEN
            '        Exit Sub
            '    End If

            'End If
            'End Commented for RWMS-202
            'Added for RWMS-202
            'RWMS-2752 Commented
            'If weightNeeded(oSku) Then
            'RWMS-2752 Commented END
            If oSku.IsWeightCaptureRequiredAtPicking AndAlso Not oSku.HasShippingWeightCaptureMethod Then
                Dim err, barcode, ret As String

                barcode = DO1.Value("WEIGHT")
                ret = gs.getWeight(barcode, err)
                If ret = 1 Then
                    DO1.Value("WEIGHT") = barcode
                    currentWeight = barcode
                Else
                    DO1.Value("WEIGHT") = ""
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ret & " " & err)
                    Return
                End If

                'Try
                '    currentWeight = DO1.Value("WEIGHT")
                '    If currentWeight <= 0 Then Throw New Exception()
                'Catch ex As Exception
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Invalid Weight"))
                '    Return
                'End Try


                'added validation code for weight
                If Not VALIDATEWEIGHT(pck, currentWeight) Then
                    'VALIDATION NOT PASSED; REDIRECTED TO OVERIDE SCREEN
                    Exit Sub
                End If

            End If
            'End Added for RWMS-202
            If oSku IsNot Nothing And pck IsNot Nothing Then
                If oSku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                    currentWeight = GetNetWeight(pck.sku)
                End If

                If oSku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                    currentWeight = GetGrossWeight(pck.sku)
                End If
            End If
            pck.oAttributeCollection = ExtractAttributes(currentWeight)
            If currentWeight > 0 Then
                Try
                    Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", currentWeight, pck.fromload)
                    Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
                    MyBase.WriteToRDTLog("SQL : " & SQL)
                Catch ex As Exception
                    MyBase.WriteToRDTLog(ex.ToString())
                End Try
            End If
            Try
                'Commented for RWMS-202
                'pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM").Trim(), "", DO1.Value("WAREHOUSEAREA").Trim())
                'End Commented for RWMS-202
                'Added for RWMS-202
                If pck.TaskConfirmation.ConfirmationType = WMS.Lib.CONFIRMATIONTYPE.SKULOCATION Then
                    pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRMPAYLOADID").Trim(), DO1.Value("CONFIRM(SKU)").Trim(), DO1.Value("WAREHOUSEAREA").Trim())

                Else
                    pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRMPAYLOADID").Trim(), "", DO1.Value("WAREHOUSEAREA").Trim())
                End If
                'End Added for RWMS-202
                pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
                If ForceWeightCaptureInFullPick AndAlso oSku.IsWeightCaptureRequiredAtPicking AndAlso oSku.HasShippingWeightCaptureMethod Then
                    Dim newPck As PickJob = Session("PCKPicklistPickJob")
                    InsertCasesWeight(newPck)
                ElseIf oSku.IsWeightCaptureRequiredAtPicking Then
                    InsertGrossWeight(oSku)
                End If
                'pck = PickTask.Pick(DO1.Value("CONFIRM").Trim(), DO1.Value("WAREHOUSEAREA").Trim(), pcklst, pck, WMS.Logic.Common.GetCurrentUser)
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                ClearAttributes()
                'Start RWMS-1326

                MyBase.WriteToRDTLog(ex.ToString())

                'End RWMS-1326

                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
                ClearAttributes()
                'Start RWMS-1326

                MyBase.WriteToRDTLog(ex.ToString())

                'End RWMS-1326

                Return
                'Catch ex As System.Threading.ThreadStartException
                '    Return
            End Try
            ClearAttributes()

            ' Assign Load delivery Task - PWMS-520
            tm.AssignDeleveryTask(WMS.Logic.GetCurrentUser, pcklst, WMS.Lib.TASKTYPE.LOADDELIVERY)
            ' Assign Load delivery Task - PWMS-520

            setPick(pck)
        Catch ex As System.Threading.ThreadAbortException
            'Start RWMS-1326

            MyBase.WriteToRDTLog(ex.ToString())

            'End RWMS-1326

        End Try

    End Sub

    Private Function VALIDATEWEIGHT(ByRef pck As PickJob, ByVal currentWeight As Decimal) As Boolean
        Dim RETVAL As Boolean = True
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'added validation code for weight
        Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
        Dim PICKOVERRIDEVALIDATOR As String
        PICKOVERRIDEVALIDATOR = getPICKOVERRIDEVALIDATOR(pck.consingee, pck.sku)

        If oSku.IsWeightCaptureRequiredAtPicking AndAlso Not oSku.HasShippingWeightCaptureMethod AndAlso Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then

            'New Validation with expression evaluation
            Dim vals As New Made4Net.DataAccess.Collections.GenericCollection


            Dim dAvgWeight As Decimal = GetWeightPerCase(oSku, pck.units, currentWeight)

            vals.Add("CONSIGNEE", pck.consingee)
            vals.Add("SKU", oSku.SKU)
            vals.Add("CASEWEIGHT", dAvgWeight)

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Illegal validation function") & statement & ex.Message)
                Return False
            End Try

            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                RETVAL = False
                gotoOverride(returnedResponse(1), currentWeight)

            ElseIf returnedResponse(0) = "-1" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ret)
                Return False
            End If
        End If
        Return RETVAL
    End Function

    Private Sub gotoOverride(ByVal errorOverride As String, ByVal currentWeight As Decimal)

        Session("ERROROVERRIDE") = errorOverride

        Session("WeightNeededPickJob") = Session("PCKPicklistPickJob")

        Session("WeightOverridConfirm") = currentWeight ' DO1.Value("WEIGHT")

        Session("WeightNeededConfirm1") = DO1.Value("CONFIRMPAYLOADID").Trim
        Session("WeightNeededConfirm2") = DO1.Value("WAREHOUSEAREA").Trim
        ClearAttributes()
        Try
            Response.Redirect(MapVirtualPath("screens/PCKOVERRIDEWEIGHT.aspx?sourcescreen=PCKFULL"), False)

        Catch ex As System.Threading.ThreadAbortException
            Context.ApplicationInstance.CompleteRequest()
            Return
        End Try
    End Sub
    Private Function ProcessSubstituteLoad() As Boolean
        Dim pck As PickJob
        Dim pcklst As Picklist = Session("PCKPicklist")
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        pck = Session("PCKPicklistPickJob")

        If pck Is Nothing Then

            Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
            If Not Session("PCKPicklist") Is Nothing Then
                Dim oPicklst As WMS.Logic.Picklist = Session("PCKPicklist")
                Dim oPicklist As New Picklist(oPicklst.PicklistID)
                If Not oPicklist Is Nothing Then
                    Session("PCKPicklist") = oPicklist
                End If
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

        Dim errmsg As String
        If Not MobileUtils.canOverrideLoad(pck.fromload, DO1.Value("SubstituteLoad"), pcklst, errmsg) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errmsg)
            Return False
        End If
        Try
            pck.pickedqty = pck.units
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pck.picklist)
            If MobileUtils.LoggedInMHEID.Trim <> String.Empty Then
                pck.LabelPrinterName = MobileUtils.GetMHEDefaultPrinter
            End If

            Dim dirPath As String = WMS.Logic.GetSysParam(Made4Net.Shared.ConfigurationSettingsConsts.RDTSubstituteLoadLogPath)
            Dim logdir As String = Made4Net.DataAccess.Util.BuildAndGetFilePath(dirPath)
            If Not PickTask.UpdateSubstituteLoad(DO1.Value("SubstituteLoad"), pcklst, pck, WMS.Logic.GetCurrentUser, logdir, errmsg) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errmsg)
                Return False
            End If
            pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("SubstituteLoad"), "", DO1.Value("WAREHOUSEAREA").Trim())
            Session("PCKPicklistPickJob") = pck
            DO1.Value("CONFIRMPAYLOADID") = DO1.Value("SubstituteLoad")
            setPick(pck)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return False
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            Return False
        End Try
        Return True
    End Function
    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'RWMS-383: Begin
        'DO1.AddLabelLine("PickMethod")
        'RWMS-383: End
        DO1.AddLabelLine("PickType")
        'Added for RWMS-755
        DO1.AddLabelLine("COMPANYNAME")
        'End Added for RWMS-755
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LoadId")
        DO1.AddLabelLine("Location")
        DO1.AddLabelLine("WAREHOUSEAREA")
        'DO1.AddLabelLine("UomUnits")
        DO1.AddLabelLine("QTY")
        'DO1.AddSpacer()

        Dim pck As PickJob = Session("PCKPicklistPickJob")
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    DO1.AddTextboxLine(pck.oAttributeCollection.Keys(idx))
                Next
            End If
        End If
        'DO1.AddSpacer()
        DO1.AddDropDown("TaskProblemCode")
        DO1.AddTextboxLine("CONFIRMPAYLOADID", False, "next")
        'Added for RWMS-202
        addConfirmationFields(pck.TaskConfirmation.ConfirmationType)
        'Added for RWMS-202
        'DO1.AddTextboxLine("SubtituteLoad")
        DO1.AddTextboxLine("SubstituteLoad", "Substitute Load")
        'RWMS-573 - Remove the Printer text box
        'DO1.AddTextboxLine("PRINTER")
    End Sub

    Private Sub InsertCasesWeight(pckJob As WMS.Logic.PickJob)


        Dim user As String = WMS.Logic.GetCurrentUser

        Dim sql As String = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER, UCC128, AICODE, UCC128Date) values"

        Dim sqlUomNum As String = "select ISNULL(max(UOMNUM),0) from LOADDETWEIGHT where LOADID='{0}'"
        Dim sqlToLoad As String
        Dim dt As New DataTable
        Dim toload As String
        Dim units As Decimal
        Dim casesListCounter As Integer = 0
        Dim weight As Decimal
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


            MyBase.WriteToRDTLog("sqlToLoad :" + sqlToLoad)



            Made4Net.DataAccess.DataInterface.FillDataset(sqlToLoad, dt)

            Dim ld As WMS.Logic.Load
            Dim loadArrayList As New List(Of String)() ''RWMS-1315 Attribute table update wrong for partial pick weight

            MyBase.WriteToRDTLog("Start Outer for loop...")


            For Each dr As DataRow In dt.Rows


                MyBase.WriteToRDTLog("Iterating the LOADS...")


                SumWeight = 0

                toload = dr("toload")
                If Not loadArrayList.Contains(toload) Then
                    loadArrayList.Add(toload)
                End If

                ld = New Load(toload)

                units = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format(sqlUomNum, toload)))
                Dim ldSku As WMS.Logic.SKU = New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
                If ldSku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                    weight = GetNetWeight(ld.SKU)
                End If

                If ldSku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                    weight = GetGrossWeight(ld.SKU)
                End If


                MyBase.WriteToRDTLog("Start Inner for loop...")


                For i As Integer = units To units + ld.UOMUnits - 1


                    MyBase.WriteToRDTLog("Iterating the LOAD UOM units...")


                    If casesListCounter = 0 Then
                        sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}', '{5}', '{6}', NULL) "
                    Else
                        sql += ", ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', '{5}', '{6}', NULL) "
                    End If
                    sql = String.Format(sql, toload, pckJob.uom, i + 1, weight, user, "", "", "")



                    MyBase.WriteToRDTLog("sql :" + sql)


                    casesListCounter = casesListCounter + 1


                    MyBase.WriteToRDTLog("casesListCounter :" + casesListCounter.ToString())


                Next


                MyBase.WriteToRDTLog("Finished Iterating the LOAD UOM units.")
                MyBase.WriteToRDTLog("EXIT Inner for loop.")


            Next


            MyBase.WriteToRDTLog("Finished Iterating the LOADS.")
            MyBase.WriteToRDTLog("EXIT outer for loop.")


            MyBase.WriteToRDTLog("Started updating LOADDETWEIGHT...")
            MyBase.WriteToRDTLog("Final sql : " + sql)


            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)


            MyBase.WriteToRDTLog("Finished updating LOADDETWEIGHT.")


            'Start RWMS-1315 - Attribute table update wrong for partial pick weight


            MyBase.WriteToRDTLog("Started updating attribute load weight...")


            Dim strToLoadid As String
            For Each strToLoadid In loadArrayList
                updateAttToLoadWeight(strToLoadid)
            Next
            'END RWMS-1315 - Attribute table update wrong for partial pick weight


            MyBase.WriteToRDTLog("Finished updating attribute load weight.")


        Catch ex As Exception

            MyBase.WriteToRDTLog("error :" + ex.ToString())

        End Try
    End Sub

    Private Function GetNetWeight(ByVal sku As String) As Decimal
        Dim sqluom As String = String.Format("select NETWEIGHT from SKUUOM where isnull(loweruom,'')=''  and SKU = '{0}'", sku)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
    End Function

    Private Function GetGrossWeight(ByVal sku As String) As Decimal
        Dim sqluom As String = String.Format("select GROSSWEIGHT from SKUUOM where isnull(loweruom,'')=''  and SKU = '{0}'", sku)
        Return Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
    End Function

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Try
            Select Case e.CommandText.ToLower
                Case "next", "Case Weight"
                    DoNext()
                Case "pickanotherload"
                    If ProcessSubstituteLoad() Then
                        DoNext()
                    End If
                Case "back"
                    doBack()
                Case "reportproblem"
                    ReportProblem()
            End Select
        Catch ex As System.Threading.ThreadAbortException

        End Try


    End Sub

    'Added for RWMS-202
    Private Sub addConfirmationFields(ByVal pConfirmType As String)
        Select Case pConfirmType
            'Case WMS.Lib.CONFIRMATIONTYPE.LOAD
            '    DO1.AddTextboxLine("CONFIRM(LOAD)", True, "next")
            'Case WMS.Lib.CONFIRMATIONTYPE.LOCATION
            '    DO1.AddTextboxLine("CONFIRM(LOCATION)", True, "next")
            'Case WMS.Lib.CONFIRMATIONTYPE.NONE
            'Case WMS.Lib.CONFIRMATIONTYPE.SKU
            '    DO1.AddTextboxLine("CONFIRM(SKU)", True, "next")
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATION
                '    DO1.AddTextboxLine("CONFIRM(SKU)", True, "next")
                DO1.AddTextboxLine("CONFIRM(SKU)", True, "next")
                'Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATIONUOM
                '    DO1.AddTextboxLine("CONFIRM(SKU)", True, "next")
                '    DO1.AddTextboxLine("CONFIRM(LOCATION)", True, "next")
                '    DO1.AddTextboxLine("CONFIRM(UOM)", True, "next")
                'Case WMS.Lib.CONFIRMATIONTYPE.SKUUOM
                '    DO1.AddTextboxLine("CONFIRM(SKU)", True, "next")
                '    DO1.AddTextboxLine("CONFIRM(UOM)", True, "next")
                'Case WMS.Lib.CONFIRMATIONTYPE.UPC
                '    DO1.AddTextboxLine("CONFIRM(UPC)", True, "next")
        End Select
    End Sub
    'End Added for RWMS-202

    'Added for RWMS-202
    Private Sub clearConfirmationFields(ByVal pConfirmType As String)
        Select Case pConfirmType
            Case WMS.Lib.CONFIRMATIONTYPE.LOAD
                DO1.Value("CONFIRM(LOAD)") = ""
            Case WMS.Lib.CONFIRMATIONTYPE.LOCATION
                DO1.Value("CONFIRM(LOCATION)") = ""
            Case WMS.Lib.CONFIRMATIONTYPE.NONE
            Case WMS.Lib.CONFIRMATIONTYPE.SKU
                DO1.Value("CONFIRM(SKU)") = ""
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATION
                DO1.Value("CONFIRM(SKU)") = ""
                DO1.Value("CONFIRM(LOCATION)") = ""
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATIONUOM
                DO1.Value("CONFIRM(SKU)") = ""
                DO1.Value("CONFIRM(LOCATION)") = ""
                DO1.Value("CONFIRM(UOM)") = ""
            Case WMS.Lib.CONFIRMATIONTYPE.SKUUOM
                DO1.Value("CONFIRM(SKU)") = ""
                DO1.Value("CONFIRM(UOM)") = ""
            Case WMS.Lib.CONFIRMATIONTYPE.UPC
                DO1.Value("CONFIRM(UPC)") = ""
        End Select
    End Sub
    'End Added for RWMS-202

    Private Sub ReportProblem()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try

            If String.IsNullOrEmpty(DO1.Value("TaskProblemCode")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("No problem code selected"))
                Exit Sub
            End If


            Dim UserId As String = WMS.Logic.Common.GetCurrentUser
            Dim pcklst As Picklist = Session("PCKPicklist")
            Dim pck As PickJob = Session("PCKPicklistPickJob")
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pck.picklist)
            Dim oPickTask As New WMS.Logic.PickTask(tm.Task.TASK)

            Dim confirmLocation As String = pck.fromlocation


            If Not WMS.Logic.Location.Exists(confirmLocation, pck.fromwarehousearea) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location does not exist", "Location does not exist")
            End If

            Dim loc As New WMS.Logic.Location(confirmLocation, pck.fromwarehousearea)
            If loc.PROBLEMFLAG Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location already marked as problematic", "Location already marked as problematic")
            End If


            Dim sql As String
            Dim dt As New DataTable
            Dim prevLoc, prevWarehousearea As String
            sql = String.Format("SELECT top 1 * FROM vWHACTIVITY where userid ='{0}' order by ACTIVITYTIME desc", UserId)
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                prevLoc = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("LOCATION"), "")
                prevWarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("WAREHOUSEAREA"), "")
            End If

            sendProblemToAudit(pck)


            sql = String.Format("update TASKS set FROMLOCATION='{0}' where TASK='{1}'", tm.Task.TOLOCATION, tm.Task.TASK)
            Made4Net.DataAccess.DataInterface.RunSQL(sql)

            pcklst.unAllocate(WMS.Logic.GetCurrentUser)


            sql = String.Format("Update loads set location='{1}', activitystatus='',destinationlocation='',unitsallocated=0 where loadid='{0}'", pck.fromload, confirmLocation)
            Made4Net.DataAccess.DataInterface.RunSQL(sql)


            Dim ld As New WMS.Logic.Load(pck.fromload)

            ld.setStatus("PROBLEM", DO1.Value("TaskProblemCode"), WMS.Logic.GetCurrentUser)

            oPickTask.FROMLOCATION = confirmLocation


            sql = String.Format("update LOCATION set PROBLEMFLAGRC=(SELECT LOCATIONPROBLEMRC FROM TASKPROBLEMCODE where PROBLEMCODEID = '{0}') where LOCATION = '{1}' and WAREHOUSEAREA='{2}'", DO1.Value("TaskProblemCode"), confirmLocation, DO1.Value("WAREHOUSEAREA"))
            Made4Net.DataAccess.DataInterface.RunSQL(sql)


            oPickTask.ReportProblem(pcklst, DO1.Value("TaskProblemCode"), confirmLocation, pck.fromwarehousearea, UserId)

            Dim tsk As New WMS.Logic.Task(tm.Task.TASK)

            tsk.ExecutionWarehousearea = oPickTask.TOWAREHOUSEAREA
            tsk.ExecutionLocation = confirmLocation
            tsk.PROBLEMFLAG = True

            sql = "SELECT PROBLEMCODEDESC FROM vTaskTypesProblemCodes WHERE TASKTYPE = '" & WMS.Lib.TASKTYPE.FULLPICKING & "' AND PROBLEMCODEID = '" & DO1.Value("TaskProblemCode") & "'"
            sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

            tsk.PROBLEMRC = sql
            tsk.Save()
            tsk.Cancel()
            'create a new task of type MISC and subtype TRAVEL,
            'with from location as the user location from activity history, and to location as the problematic location.
            'Assign it to the user and complete it.
            Dim t As WMS.Logic.Task
            t = New WMS.Logic.Task()
            t.TASKTYPE = "MISC"
            t.TASKSUBTYPE = "TRAVEL"


            If String.IsNullOrEmpty(prevLoc) Then
                prevLoc = confirmLocation
                prevWarehousearea = oPickTask.FROMWAREHOUSEAREA
            End If
            t.FROMLOCATION = prevLoc
            t.FROMWAREHOUSEAREA = prevWarehousearea
            t.TOLOCATION = confirmLocation
            t.TOWAREHOUSEAREA = oPickTask.FROMWAREHOUSEAREA
            t.Create()
            t.AssignUser(UserId)
            t.Complete(WMS.Logic.LogHandler.GetRDTLogger)

            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Location problem reported"))


        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.ToString())
            Return
        End Try
        If Not Session("TMTaskId") Is Nothing Then
            Dim oTask As WMS.Logic.Task = Session("TMTaskId")
            Dim taskid As String = oTask.TASK
            ' Reload
            oTask = New WMS.Logic.Task(taskid, True)
            Session("TMTaskId") = oTask
        End If

        Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

        If Not Session("PCKPicklist") Is Nothing Then
            Dim oPicklst As WMS.Logic.Picklist = Session("PCKPicklist")
            Dim oPicklist As New Picklist(oPicklst.PicklistID)
            If Not oPicklist Is Nothing Then
                Session("PCKPicklist") = oPicklist
            End If
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

    End Sub

    Private Sub sendProblemToAudit(ByVal pwjob As PickJob)
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim ld As New WMS.Logic.Load(pwjob.fromload)

        Dim MSG As String = "REPORTPROBLEM"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.REPORTPROBLEM)
        aq.Add("ACTIVITYTYPE", MSG)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", ld.CONSIGNEE)
        aq.Add("DOCUMENT", ld.RECEIPT)
        aq.Add("DOCUMENTLINE", ld.RECEIPTLINE)
        aq.Add("FROMLOAD", ld.LOADID)
        aq.Add("FROMLOC", ld.LOCATION)
        aq.Add("FROMQTY", ld.UNITS)
        aq.Add("FROMSTATUS", ld.STATUS)


        Dim SQL As String

        SQL = "SELECT PROBLEMCODEDESC FROM vTaskTypesProblemCodes WHERE TASKTYPE = '" & WMS.Lib.TASKTYPE.FULLPICKING & "' AND PROBLEMCODEID = '" & DO1.Value("TaskProblemCode") & "'"
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        aq.Add("NOTES", SQL)

        aq.Add("SKU", ld.SKU)
        aq.Add("TOLOAD", ld.LOADID)
        aq.Add("TOLOC", ld.LOCATION)
        aq.Add("TOQTY", pwjob.units)
        aq.Add("TOSTATUS", "PROBLEM")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(MSG)

    End Sub

    Private Function ExtractAttributes(ByVal currentWeight As Decimal) As AttributesCollection
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        Dim Val As Object
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    If Val = "" Then Val = Nothing
                    If pck.oAttributeCollection.Keys(idx).ToUpper = "WEIGHT" Then
                        pck.oAttributeCollection(idx) = currentWeight.ToString()
                    Else
                        pck.oAttributeCollection(idx) = Val
                    End If

                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function

    Private Sub ClearAttributes()
        Dim pck As PickJob
        pck = Session("PCKPicklistPickJob")
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    DO1.Value(pck.oAttributeCollection.Keys(idx)) = ""
                Next
            End If
        End If
    End Sub

    Private Function getPICKOVERRIDEVALIDATOR(ByVal oConsignee As String, ByVal oSku As String) As String
        Dim ret As String = String.Empty
        Dim objSku As WMS.Logic.SKU = New WMS.Logic.SKU(oConsignee, oSku)

        If Not objSku.SKUClass Is Nothing Then

            Dim objSkuClass As WMS.Logic.SkuClass = objSku.SKUClass

            Dim sql As String = String.Format("SELECT ISNULL(PICKOVERRIDEVALIDATOR, '') FROM SKUCLSLOADATT WHERE CLASSNAME = '{0}' AND ATTRIBUTENAME = 'WEIGHT'", objSkuClass.ClassName)
            Try
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
            Return ret
        End If
        Return ret
    End Function

    'Added for RWMS-323
    Protected Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        DoNext()
    End Sub
    'End Added for RWMS-323
    'Added for RMWS-584
    Private Sub InsertGrossWeight(ByVal oSku As WMS.Logic.SKU)

        Dim sql As String = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) values"
        Dim pckJob As WMS.Logic.PickJob = Session("PCKPicklistPickJob")
        Dim user As String = WMS.Logic.GetCurrentUser
        Dim grossweight As Decimal



        If ((DO1.Value("WEIGHT") Is Nothing) Or (DO1.Value("WEIGHT").Length = 0)) Then

            Try
                Dim ld As New Load(pckJob.fromload)
                If Math.Round(ld.GetAttribute("WEIGHT"), 2) > 0 Then DO1.Value("WEIGHT") = Math.Round(ld.GetAttribute("WEIGHT"), 2)
                'Start RWMS-1326

                MyBase.WriteToRDTLog(" From Load : " & pckJob.fromload & " WEIGHT : " & DO1.Value("WEIGHT"))

                'End RWMS-1326

            Catch ex As Exception
                DO1.Value("WEIGHT") = 0
                'Start RWMS-1326

                MyBase.WriteToRDTLog(ex.ToString())

                'End RWMS-1326

            End Try


        End If
        If oSku.HasShippingWeightCaptureMethod Then
            grossweight = (pckJob.pickedqty * DO1.Value("WEIGHT"))
        Else
            grossweight = DO1.Value("WEIGHT")
        End If
        sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}') "
        sql = String.Format(sql, pckJob.fromload, pckJob.uom, 1, grossweight, user)

        ' Update weight
        updateToLoadWeight(pckJob.fromload, grossweight)

        Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'Start RWMS-1326

        MyBase.WriteToRDTLog(" SQL : " & sql)

        'End RWMS-1326




    End Sub

    Private Sub updateToLoadWeight(ByVal Loadid As String, ByVal Weight As Decimal)



        Try


            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", Weight, Loadid)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
            'Start RWMS-1326

            MyBase.WriteToRDTLog(" SQL : " & SQL)

            'End RWMS-1326


        Catch ex As Exception
            'Start RWMS-1326

            MyBase.WriteToRDTLog(ex.ToString())

            'End RWMS-1326

            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "can't update flomload weight")
        End Try
    End Sub

    Private Sub updateAttToLoadWeight(ByVal strToLoadid As String)

        Try

            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT=(SELECT SUM(UOMWEIGHT) FROM LOADDETWEIGHT WHERE LOADID='{0}') WHERE PKEY1='{0}' AND PKEYTYPE='LOAD'", strToLoadid)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)


            MyBase.WriteToRDTLog("SQL :" + SQL)


        Catch ex As Exception


            MyBase.WriteToRDTLog("error :" + ex.ToString())



        End Try
    End Sub
    'End Addded for RWMS-584

    'Added for PWMS-367
    Public ReadOnly Property ForceWeightCaptureInFullPick() As Boolean
        Get
            Try
                If WMS.Logic.GetSysParam("ForceWeightCaptureInFullPick") = 0 Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As SysParamNotFoundException
                Return False
            End Try
        End Get
    End Property
    'Added for PWMS-367
End Class