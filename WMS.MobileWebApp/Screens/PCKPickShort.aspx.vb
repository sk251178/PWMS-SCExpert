Imports WMS.Lib
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports System.Collections.Generic

Partial Public Class PCKPickShort
    Inherits PWMSRDTBase

    Dim logger As WMS.Logic.LogHandler

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then

            Session("PICKINGSRCSCREEN") = Request("sourcescreen")

            Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

            setScreen(pckJob)


        End If
    End Sub

    Private Sub setScreen(ByVal pPickJob As WMS.Logic.PickJob)

        DO1.Value("Picklist") = pPickJob.picklist
        DO1.Value("SKU") = pPickJob.sku
        DO1.Value("PicklistLine") = pPickJob.PickDetLines(0)
        DO1.Value("Location") = pPickJob.fromlocation
        DO1.Value("QtyToPick") = Session("PCKOldUomUnits")

        'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) And Not IsNothing(Session("UOMUnits_2")) Then
        '    DO1.Value("QTYConfirmed") = pPickJob.uomunits + MultiContManage.GetTotalPickedContainerUOMUnits(pPickJob) 'Session("UOMUnits_2")
        'Else
        DO1.Value("QTYConfirmed") = pPickJob.uomunits
        'End If



    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("PicklistLine")
        DO1.AddLabelLine("Location")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("QtyToPick")
        DO1.AddLabelLine("QTYConfirmed")
        DO1.AddSpacer()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        DO1.AddLabelLine("WARNING", "WARNING", t.Translate("Short product, are you sure?"))

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Select Case e.CommandText.ToLower
            Case "no"
                doBack()
            Case "yes"
                ' doFinish()
                doYes()
        End Select

    End Sub

    Private Sub ClearSession()
        Try

            Session.Remove("PICKINGSRCSCREEN")
            Session.Remove("WeightNeededPickJob")
            Session.Remove("WeightNeededConfirm1")
            Session.Remove("WeightNeededConfirm2")
            Session.Remove("PCKOldUomUnits")
            ' Session.Remove("PCKPicklist")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub doBack()
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        Try

            'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
            '    If Not IsNothing(Session("UOMUnits_2")) And Session("UOMUnits_2") <> "0" Then

            '        Response.Redirect(MapVirtualPath("screens/PCKPickMulti.aspx?sourcescreen=PCKPART"))
            '    Else

            '        Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
            '    End If
            'Else
            ClearSession()
            Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
            'End If


        Catch ex As System.Threading.ThreadAbortException

        End Try


    End Sub

    Private Sub goNextPick(ByRef pckJob As WMS.Logic.PickJob)
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        Try

            ClearSession()

        Catch ex As Exception

        End Try
        Try
            If pckJob Is Nothing Then
                If Session("PCKPicklist") IsNot Nothing Then
                    Dim pcklist As Picklist = Session("PCKPicklist")
                    pcklist.Load()
                    If pcklist.isCompleted Then
                        Session.Remove("PCKListToResume")
                    Else
                        Session("PCKListToResume") = pcklist.PicklistID
                    End If
                    If pcklist.GetTotalPickedQty > 0 Then
                        If Session("PCKBagOutPicking") Is Nothing AndAlso Session("PCKBagOutPicking") = 0 Then

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

                            If Not Session("PCKPicklist") Is Nothing Then
                                Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                                If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                                    Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                                End If
                            End If
                        Else
                            Dim tm As New WMS.Logic.TaskManager
                            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, Nothing) Then
                                Dim TMTask As WMS.Logic.Task = tm.RequestTask(GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING)
                            End If
                            If pcklist.ShouldPrintBagOutReportOnComplete And pcklist.isCompleted Then
                                Response.Redirect(MapVirtualPath("Screens/PCKBAGOUTPRINT.aspx"))
                            Else
                                Response.Redirect(MapVirtualPath("screens/BagOutCloseContainer.aspx?sourcescreen=PCKBagOut"))
                            End If
                        End If
                    End If
                    End If
            End If
            Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub doFinish(ByRef pckJob As WMS.Logic.PickJob)
        ' Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim srcScreen As String = Session("PICKINGSRCSCREEN")

        If srcScreen.ToString().ToLower() = "pckpart" Then
            'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
            '    MultiContManage.finishPartial()
            'Else
            finishPartial(pckJob)
            'End If
        Else
            finishParallel(pckJob)
        End If
        'Session.Remove("WeightNeededConfirm1")
        'Session.Remove("WeightNeededConfirm2")

        ' Session("PICKINGSRCSCREEN") = Request("sourcescreen")

        'Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

        'doBack()
    End Sub

    Private Sub doYes()
        'Session("PICKINGSRCSCREEN") = Request("sourcescreen")
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")

        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")


        If Not pckJob Is Nothing AndAlso Not pckJob.container Is Nothing AndAlso WMS.Logic.Container.Exists(pckJob.container) Then
            Dim container As WMS.Logic.Container = New WMS.Logic.Container(pckJob.container, True)
            container.UpdateLastPickLocation(pckJob.fromlocation, pckJob.fromwarehousearea, WMS.Logic.GetCurrentUser)
        End If

        Dim C As New WMS.Logic.Counting(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", False)

        C.CreateLocationCountJobs(pckJob.fromwarehousearea, "", pckJob.fromlocation, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", "", WMS.Logic.GetCurrentUser)

        Try
            Dim sql As String = String.Format("update tasks set PRIORITY=400 where TASKTYPE='LOCCNT' and COUNTID = '{0}' and FROMLOCATION='{1}' and FROMWAREHOUSEAREA='{2}'", C.COUNTID, C.LOCATION, C.WAREHOUSEAREA)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        sendMessageQ()

        Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)
        insertDataInExceptionHistory(pckJob)
        Try

            If weightNeeded(osku) Then
                If pckJob.uomunits > 0 Then
                    'Session("WeightNeededPickJob") = PCK
                    'Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                    Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=" & srcScreen))
                Else
                    doFinish(pckJob)
                    goNextPick(pckJob)
                End If
            Else
                doFinish(pckJob)
                goNextPick(pckJob)
            End If

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    'Added for RWMS-2599 Start
    Private Sub insertDataInExceptionHistory(ByRef pckJob As WMS.Logic.PickJob)
        Dim pcklstdetail As New WMS.Logic.PicklistDetail(pckJob.picklist, pckJob.PickDetLines(0), True)
        Dim ExceptionId As String = Made4Net.Shared.Util.getNextCounter("ExceptionId")
        Dim addDate As Date = DateAndTime.Now
        Dim code As String = String.Empty
        Dim strSQL As String = DataInterface.ExecuteScalar(String.Format("Select count(1) from TASKS where TASKTYPE in ('{0}','{1}','{2}') and sku='{3}' and STATUS not in ('CANCELED','COMPLETE')", WMS.Lib.TASKTYPE.NEGTREPL, WMS.Lib.TASKTYPE.FULLREPL, WMS.Lib.TASKTYPE.PARTREPL, pckJob.sku))
        If strSQL = 0 Then
            code = "U"
        Else
            code = "R"
        End If

        Dim exceptionqty As Decimal = pckJob.adjustedunits - pckJob.pickedqty
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()

        Dim oLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        If Not oLogger Is Nothing Then
            oLogger.Write(" Proceeding to write for pickshort exception history for orderID : " & pcklstdetail.OrderId & " orderline : " & pcklstdetail.OrderLine.ToString() & " Exception qty = " & exceptionqty.ToString())
        End If


        Dim sql As String = String.Format("INSERT INTO ExceptionHistory(ExceptionId,ActivityDate,Consignee,Orderid,OrderLine,Code,ExceptionQty,AddUser) values ({0},{1},{2},{3},{4},{5},{6},{7})", _
                                        Made4Net.Shared.Util.FormatField(ExceptionId), Made4Net.Shared.Util.FormatField(addDate), Made4Net.Shared.Util.FormatField(pckJob.consingee),
                                        Made4Net.Shared.Util.FormatField(pcklstdetail.OrderId), Made4Net.Shared.Util.FormatField(pcklstdetail.OrderLine),
                                        Made4Net.Shared.Util.FormatField(code), Made4Net.Shared.Util.FormatField(exceptionqty), Made4Net.Shared.Util.FormatField(UserId))

        DataInterface.RunSQL(sql)

    End Sub

    'Added for RWMS-2599 End

    Private Function weightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        'RWMS-2829
        If pSKU.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Or pSKU.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
            Return False
        End If
        'RWMS-2829
        If Not pSKU.SKUClass Is Nothing Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If (oAtt.Name.ToUpper = "WEIGHT" Or oAtt.Name.ToUpper = "WGT") AndAlso _
                (oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Private Sub sendMessageQ()
        Dim pck As WMS.Logic.PickJob = Session("WeightNeededPickJob")
        'Commented for PWMS-756 and made it generic
        'Dim MSG As String = "SHORTPICK"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SHORTPICK)
        aq.Add("ACTIVITYTYPE", WMS.Lib.TASKTYPE.SHORTPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")

        aq.Add("USERID", WMS.Logic.GetCurrentUser)

        aq.Add("DOCUMENT", pck.picklist)
        Dim strLines As String = "1"
        Try
            strLines = pck.PickDetLines(0)
        Catch ex As Exception
        End Try
        aq.Add("DOCUMENTLINE", strLines)

        aq.Add("CONSIGNEE", pck.consingee)

        aq.Add("SKU", pck.sku)

        aq.Add("FROMLOAD", pck.fromload)
        'aq.Add("TOLOAD", "") '??

        'Commented for RWMS-1871(RWMS-1878)
        'If PickTask.doesLoadExists(pck) Then
        '    Dim L As New WMS.Logic.Load(pck.fromload)
        '    aq.Add("FROMSTATUS", L.STATUS) '??
        'Else
        '    aq.Add("FROMSTATUS", "")
        'End If
        'End Commented for RWMS-1871(RWMS-1878)
        'Added for RWMS-1871(RWMS-1878) - Removed the condition check doesLoadExists
        Dim L As New WMS.Logic.Load(pck.fromload)
        aq.Add("FROMSTATUS", L.STATUS) '??
        'End Added for RWMS-1871(RWMS-1878)

        aq.Add("FROMLOC", pck.fromlocation)
        aq.Add("FROMQTY", pck.units)
        'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) And Not IsNothing(Session("UOMUnits_2")) Then
        '    'Dim sku As New WMS.Logic.SKU(pck.consingee, pck.sku)
        '    aq.Add("TOQTY", pck.pickedqty + MultiContManage.GetTotalPickedContainerUnits) '(sku.ConvertToUnits(pck.uom) * Session("UOMUnits_2")))
        'Else
        aq.Add("TOQTY", pck.pickedqty)
        'End If



        aq.Add("MHEID", Session("MHEID"))
        aq.Add("MHType", Session("MHType"))

        aq.Add("FROMCONTAINER", pck.container)

        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        'aq.Add("TOLOC", "")
        'aq.Add("TOSTATUS", Session("CreateLoadStatus"))
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)

        'PWMS-756 and made it generic  for shortpick
        aq.Send(WMS.Lib.TASKTYPE.SHORTPICK)


    End Sub

    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("WeightNeededPickJob")
        ' Dim Val As Object
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    'Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    'If Val = "" Then Val = Nothing
                    If (pck.oAttributeCollection.Keys(idx).ToUpper = "WEIGHT" Or pck.oAttributeCollection.Keys(idx).ToUpper = "WGT") Then
                        pck.oAttributeCollection(idx) = "0" 'DO1.Value("TOTAL") 'Val
                    End If

                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function


    Private Sub finishPartial(ByRef pckJob As WMS.Logic.PickJob)
        'Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

        Dim pcklst As New WMS.Logic.Picklist(pckJob.picklist)



        Try
            Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)
            If weightNeeded(osku) Then
                pckJob.oAttributeCollection = ExtractAttributes()
            End If


            'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
            '    Session("PCKPickLineSecond") = PCKPickMulti.splitPick(pckJob, osku.ConvertToUnits(pckJob.uom) * Session("UOMUnits_2"))

            '    If Not IsNothing(Session("PCKPickLineSecond")) Then
            '        'Dim pckLineSecond As WMS.Logic.PicklistDetail = Session("PCKPickLineSecond")
            '        'Dim sku As New SKU(pckJob.consingee, pckJob.sku)
            '        ''                pckLineSecond.Pick(sku.ConvertToUnits(pckJob.uom) * Session("UOMUnits_2"), "CASE", WMS.Logic.GetCurrentUser, pckJob.oAttributeCollection)
            '        'pckLineSecond.Pick(Session("UOMUnits_2"), pckLineSecond.UOM, WMS.Logic.GetCurrentUser, pckJob.oAttributeCollection)
            '        MobileUtils.PickRemaiderUnits(pckJob)
            '    End If
            'End If
            'RWMS-2829
            If pckJob.oAttributeCollection Is Nothing Then
                pckJob.oAttributeCollection = New WMS.Logic.AttributesCollection
            End If
            Dim key As String = pckJob.oAttributeCollection("WEIGHT")

            If osku IsNot Nothing And pckJob IsNot Nothing Then
                If osku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                    Dim netWeight As Decimal = WMS.Logic.SKU.GetNetWeight(pckJob.sku)
                    If Not key Is Nothing Then
                        pckJob.oAttributeCollection("WEIGHT") = netWeight * pckJob.uomunits
                    Else
                        pckJob.oAttributeCollection.Add("WEIGHT", netWeight * pckJob.uomunits)
                    End If
                End If

                If osku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                    Dim grossWeight As Decimal = WMS.Logic.SKU.GetGrossWeight(pckJob.sku)
                    If Not key Is Nothing Then
                        pckJob.oAttributeCollection("WEIGHT") = grossWeight * pckJob.uomunits
                    Else
                        pckJob.oAttributeCollection.Add("WEIGHT", grossWeight * pckJob.uomunits)
                    End If
                End If
            End If
            'RWMS-2829



            If Not String.IsNullOrEmpty(Session("WeightNeededConfirm2")) Then
                pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), HttpContext.Current.Session("WeightNeededConfirm2"), pckJob.fromwarehousearea)
                pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                ' pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser, True, Session("WeightNeededConfirm2"))
            Else
                pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), "", pckJob.fromwarehousearea)
                pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                '                pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser)
            End If

            Dim task As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select TASK from tasks where picklist = '{0}' and TASKTYPE = 'PARPICK'", pcklst.PicklistID))
            Session("TMTask") = New Task(task)

            If osku IsNot Nothing And Session("PCKPicklistPickJob") IsNot Nothing Then
                If osku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                    Dim netWeight As Decimal = SKU.GetNetWeight(osku.SKU)
                    Dim newPck As PickJob = Session("PCKPicklistPickJob")
                    InsertCasesWeight(newPck, netWeight)
                    Dim totalWeight As Decimal = CalculatTotalWeight(newPck, netWeight)
                    updateFromLoadWeight(newPck, totalWeight)
                End If

                If osku.SHIPPINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                    Dim grossWeight As Decimal = SKU.GetGrossWeight(osku.SKU)
                    Dim newPck As PickJob = Session("PCKPicklistPickJob")
                    InsertCasesWeight(newPck, grossWeight)
                    Dim totalWeight As Decimal = CalculatTotalWeight(newPck, grossWeight)
                    updateFromLoadWeight(newPck, totalWeight)
                End If
            End If
            'Commented for RWMS-2363
            'If IsNothing(pckJob) And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '    Session.Remove("PCKPicklist")
            '    Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=PCK"))
            'End If
            'End Commented for RWMS-2363

        Catch ex As System.Threading.ThreadAbortException


        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return
        End Try
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
            ' MessageQue.Enqueue("can't update flomload weight")
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
    'Private Sub setAttributes(ByRef pck As PickJob)
    '    If Not pck Is Nothing Then
    '        If Not pck.oAttributeCollection Is Nothing Then
    '            For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
    '                If pck.oAttributeCollection.Keys(idx) = "WEIGHT" Then
    '                    pck.oAttributeCollection.Keys(idx) = "" 'DO1.Value("TOTAL")
    '                End If
    '            Next
    '        End If
    '    End If
    'End Sub

    'Private Function ExtractAttributes() As AttributesCollection
    '    Dim pck As PickJob = Session("WeightNeededPickJob")
    '    ' Dim Val As Object
    '    If Not pck Is Nothing Then
    '        If Not pck.oAttributeCollection Is Nothing Then
    '            For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
    '                'Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
    '                'If Val = "" Then Val = Nothing
    '                If pck.oAttributeCollection.Keys(idx) = "WEIGHT" Then
    '                    pck.oAttributeCollection(idx) = DO1.Value("TOTAL") 'Val
    '                End If

    '            Next
    '            Return pck.oAttributeCollection
    '        End If
    '    End If
    '    Return Nothing
    'End Function


    Private Sub finishParallel(ByRef pckJob As WMS.Logic.PickJob)
        'Try
        '    Dim pck As PickJob
        '    pck = Session("PARPCKPicklistPickJob")

        '    Dim pcks As ParallelPicking = New ParallelPicking(pck.parallelpicklistid)
        '    Session("PARPCKPicklist") = pcks

        '    pck.oAttributeCollection = ExtractAttributes()
        '    pcks.Pick(Session("WeightNeededConfirm1"), pck.fromwarehousearea, pck, WMS.Logic.Common.GetCurrentUser)
        'Catch ex As Exception

        'End Try
        '        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
        Dim pcks As ParallelPicking = New ParallelPicking(pckJob.parallelpicklistid)
        Try
            Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)
            If weightNeeded(osku) Then
                pckJob.oAttributeCollection = ExtractAttributes()
            End If
            pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), "", pckJob.fromwarehousearea)
            pckJob = pcks.Pick(pckJob, WMS.Logic.GetCurrentUser, Nothing)

            'pckJob = pcks.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pckJob, WMS.Logic.Common.GetCurrentUser)
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then

                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(GetCurrentUser, TASKTYPE.PARALLELPICKING, LogHandler.GetRDTLogger())
                Dim pcklsts As New ParallelPicking(tm.Task.ParallelPicklist)
                Session("PARPCKPicklist") = pcklsts
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return
        End Try
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



End Class