Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Made4Net.DataAccess
Imports System.Data
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

Partial Public Class PCKWEIGHTNEEDED
    Inherits PWMSRDTBase



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IsPostBack Then

            Session("PICKINGSRCSCREEN") = Request("sourcescreen")
            Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

            setScreen(pckJob)

            If Session("CasesList") Is Nothing Then
                'Dim casesList As New System.Collections.Generic.List(Of Decimal)
                Dim casesList As New Dictionary(Of Integer, Decimal)
                Session("CasesList") = casesList
            End If
            '
            If Session("BarCodeList") Is Nothing Then
                Dim barcodeList As New Dictionary(Of Integer, String)
                Session("BarCodeList") = barcodeList
            End If
            If Session("DateList") Is Nothing Then
                Dim dateList As New Dictionary(Of Integer, String)
                Session("DateList") = dateList
            End If
            If Session("AICodeList") Is Nothing Then
                Dim aicodeList As New Dictionary(Of Integer, String)
                Session("AICodeList") = aicodeList
            End If
            '
        End If
    End Sub

    Private Function twoCont() As Boolean
        If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) And Not IsNothing(Session("UOMUnits_2")) _
          And Session("UOMUnits_2") > 0 Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Sub setScreen(ByVal pPickJob As WMS.Logic.PickJob)
        DO1.Value("Picklist") = pPickJob.picklist
        DO1.Value("SKU") = pPickJob.sku
        DO1.Value("Description") = pPickJob.skudesc
        DO1.Value("Confirmed") = pPickJob.uomunits
        DO1.setVisibility("CONTAINER", False)
        DO1.Value("Captured") = 0
        DO1.Value("Total") = 0
        DO1.Value("Last") = 0


        Try
            MyBase.WriteToRDTLog("In PCKWEIGHTNEEDED method setScreen for picklist - " + pPickJob.picklist + " SKU - " + pPickJob.sku + " UOMUnits - " + pPickJob.uomunits)
        Catch ex As Exception
        End Try



        If Not Session("CasesList") Is Nothing Then

            Try
                'Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
                Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")

                If casesList.Count = 0 Then
                    DO1.Value("Last") = 0
                    DO1.Value("TOTAL") = 0
                    DO1.Value("CAPTURED") = 0
                Else
                    DO1.Value("Last") = Session("LastWeight") 'casesList.Item(casesList.Count - 1) '
                    DO1.Value("TOTAL") = Session("WeightNeededTOTAL")
                    DO1.Value("CAPTURED") = casesList.Count
                End If
            Catch ex As Exception

            End Try

        End If


    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("Description")
        DO1.AddLabelLine("CONFIRMED", "Number of cases approved")
        DO1.AddLabelLine("CAPTURED", "Number of cases captured")
        DO1.AddLabelLine("TOTAL", "Total weight")
        DO1.AddLabelLine("LAST", "Last weight")
        DO1.AddLabelLine("CONTAINER")
        DO1.AddSpacer()
        DO1.AddTextboxLine("WEIGHT", "capture weight")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "finish"
                doFinish()
            Case "back"
                doBack()
            Case "dellast"
                doDelLast()
        End Select
    End Sub

    Private Sub doNext()
        If (Session("WeightNeededPickJob") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim numOfCaptured As Integer = DO1.Value("CAPTURED")
        Dim numOfConfirmed As Integer = DO1.Value("CONFIRMED")
        If numOfCaptured = numOfConfirmed Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Too many cases were captured"))
            DO1.Value("WEIGHT") = ""
            Return
        End If

        Dim currentWeight As Decimal = 0
        Dim currentDate As String = ""
        Dim currentAICode As String = ""

        Dim gs As Barcode128GS.GS128 = New Barcode128GS.GS128()

        Dim err, barcode, ret, weight, ucc128date, aicode As String

        barcode = DO1.Value("WEIGHT")
        weight = DO1.Value("WEIGHT")


        MyBase.WriteToRDTLog("Weight :" + weight)


        MyBase.WriteToRDTLog("Validating the barcode/weight :" + weight)


        ret = gs.getWeightDateAICode(barcode, weight, ucc128date, aicode, err)

        If ret = 1 Then
            'weight
            If Decimal.TryParse(weight, currentWeight) Then
                currentWeight = weight
            Else

                MyBase.WriteToRDTLog("Invalid weight :" + weight)


                DO1.Value("WEIGHT") = "" 'Added for RWMS-1047/RWMS-1381
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Please enter valid weight"))
                Return 'Added for RWMS-1047/RWMS-1381 Start
            End If
            'date
            currentDate = ucc128date
            'aicode
            currentAICode = aicode
            'make the textbox value as weight
            DO1.Value("WEIGHT") = weight
            'Added for RWMS-1047/RWMS-1381 Start


            MyBase.WriteToRDTLog("Validated the weight :" + weight)
            MyBase.WriteToRDTLog("Validated the currentDate :" + currentDate)
            MyBase.WriteToRDTLog("Validated the AICode :" + currentAICode)


        ElseIf ret = 2 Then

            MyBase.WriteToRDTLog("Invalid Weight :" + weight)


            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Please enter valid weight"))
            Return
            'Added for RWMS-1047/RWMS-1381 End
        Else

            MyBase.WriteToRDTLog("Invalid weight : " + weight + ".Returnvalue : " + ret + ".Error : " + err)


            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret & " " & err)
            Return
        End If

        'Added for RWMS-1047/RWMS-1381 Start
        If DO1.Value("WEIGHT").Trim = "" Then
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Weight cannot be empty"))
            Return
        ElseIf Not IsNumeric(DO1.Value("WEIGHT")) Then
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Please enter valid weight"))
            Return

        ElseIf (DO1.Value("WEIGHT") = 0) Then
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Please enter valid weight"))
            Return
            'Added for RWMS-1644 and RWMS-1599
        ElseIf (DO1.Value("WEIGHT") < 0) Then
            DO1.Value("WEIGHT") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Please enter valid weight. Weight cannot be Negative."))
            Return
            'Ended for RWMS-1644 and RWMS-1599
            'RWMS-2633 RWMS-2631 START
        ElseIf (DO1.Value("WEIGHT") > 999999999) Then
            DO1.Value("WEIGHT") = ""
            'MessageQue.Enqueue(t.Translate("Weight must be less than 1000000000"))
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Weight must be less than 1000000000"))
            Return
            'RWMS-2633 RWMS-2631 END
        Else
            currentWeight = DO1.Value("WEIGHT")
        End If

        'Added for RWMS-1047/RWMS-1381 End
        'Begin for RWMS-1288 and RWMS-1043
        Session("overrideBarcode") = barcode
        Session("overrideAICode") = currentAICode
        Session("overrideUCC128Date") = currentDate
        'END  for RWMS-1288 and RWMS-1043

        Session("WeightNeededTOTAL") = Math.Round(Decimal.Parse(DO1.Value("TOTAL")), 2) '+ currentWeight

        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

        'added validation code for weight
        If Not VALIDATEWEIGHT(pckJob, currentWeight) Then
            'VALIDATION NOT PASSED; REDIRECTED TO OVERIDE SCREEN
            Exit Sub
        End If

        'Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
        'casesList.Add(currentWeight)

        Session("LastWeight") = currentWeight
        'Session("CasesList") = casesList
        DO1.Value("Last") = currentWeight
        DO1.Value("TOTAL") = Math.Round(Decimal.Parse(DO1.Value("TOTAL")) + currentWeight, 2)
        'DO1.Value("CAPTURED") = casesList.Count
        'Commented RWMS-784
        'DO1.Value("CAPTURED") = AddtoCaseList(currentWeight)
        'End Commented RWMS-784
        'Added RWMS-784

        MyBase.WriteToRDTLog("Adding the barcode/weight to List.")
        MyBase.WriteToRDTLog("barcode :" + barcode)
        MyBase.WriteToRDTLog("currentWeight :" + currentWeight.ToString())
        MyBase.WriteToRDTLog("currentDate :" + currentDate)
        MyBase.WriteToRDTLog("currentAICode :" + currentAICode)


        DO1.Value("CAPTURED") = AddtoCaseList(barcode, currentWeight, currentDate, currentAICode)
        'End Added RWMS-784
        DO1.Value("WEIGHT") = ""
    End Sub

    Private Function GetCurrentCaseNumber() As Integer
        Return Convert.ToInt32(DO1.Value("CAPTURED"))
    End Function

    Private Function AddtoCaseList(ByVal dCaseWeight As Decimal) As Integer
        Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")
        Dim intCase As Integer = GetCurrentCaseNumber() + 1
        If casesList.ContainsKey(intCase) Then
            casesList.Item(intCase) = dCaseWeight
        Else
            casesList.Add(intCase, dCaseWeight)
        End If
        Session("CasesList") = casesList
        Return casesList.Count
    End Function
    'End Commented for RWMS-784
    'Added for RWMS-784
    Private Function AddtoCaseList(ByVal sbarcode As String, ByVal dCaseWeight As Decimal, ByVal sUCC128Date As String, ByVal sAICode As String) As Integer
        'Weight
        Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")
        Dim intCase As Integer = GetCurrentCaseNumber() + 1
        If casesList.ContainsKey(intCase) Then
            casesList.Item(intCase) = dCaseWeight
        Else
            casesList.Add(intCase, dCaseWeight)
        End If
        Session("CasesList") = casesList
        'Barcode
        Dim barcodeList As Dictionary(Of Integer, String) = Session("BarCodeList")
        'TODO : check for NULL or Nothing

        If barcodeList.ContainsKey(intCase) Then
            barcodeList.Item(intCase) = sbarcode
        Else
            barcodeList.Add(intCase, sbarcode)
        End If

        Session("BarCodeList") = barcodeList
        'date
        Dim dateList As Dictionary(Of Integer, String) = Session("DateList")

        If dateList.ContainsKey(intCase) Then
            dateList.Item(intCase) = sUCC128Date
        Else
            dateList.Add(intCase, sUCC128Date)
        End If

        Session("DateList") = dateList
        'AICode
        Dim aicodeList As Dictionary(Of Integer, String) = Session("AICodeList")

        If aicodeList.ContainsKey(intCase) Then
            aicodeList.Item(intCase) = sAICode
        Else
            aicodeList.Add(intCase, sAICode)
        End If

        Session("AICodeList") = aicodeList
        'return the caselistcount

        MyBase.WriteToRDTLog("caseList count :" + casesList.Count.ToString())


        Return casesList.Count
    End Function
    'End Added for RWMS-784

    Private Sub ClearSession()
        Try
            Session.Remove("CasesList")
            'Added for RWMS-784
            Session.Remove("BarCodeList")
            Session.Remove("DateList")
            Session.Remove("AICodeList")
            'End Added for RWMS-784
            Session.Remove("PICKINGSRCSCREEN")
            Session.Remove("WeightNeededPickJob")
            Session.Remove("WeightNeededTOTAL")
            Session.Remove("UOMUnits_2")
            Session.Remove("LastWeight")
            Session.Remove("ContCount")
            Session.Remove("AttContainerID")
            'Begin for RWMS-1288 and RWMS-1043
            Session.Remove("overrideBarcode")
            Session.Remove("overrideAICode")
            Session.Remove("overrideUCC128Date")
            'End  for RWMS-1288 and RWMS-1043
            '    Session.Remove("PCKPicklist")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ClearSessionWEIGHTNEEDEDCount()
        Try
            Session.Remove("ContCount")
            Session.Remove("CasesList")
            'Added for RWMS-784
            Session.Remove("BarCodeList")
            Session.Remove("DateList")
            Session.Remove("AICodeList")
            'End Added for RWMS-784
            Session.Remove("LastWeight")
            Session.Remove("ContCount")
            Session.Remove("AttContainerID")
            Session.Remove("WeightNeededTOTAL")
        Catch ex As Exception

        End Try
    End Sub
    Private Sub doBack()
        If (Session("PICKINGSRCSCREEN") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")

        Try
            If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
                If Not IsNothing(Session("UOMUnits_2")) And Session("UOMUnits_2") <> "0" Then
                    ClearSessionWEIGHTNEEDEDCount()
                    Response.Redirect(MapVirtualPath("screens/PCKPickMulti.aspx?sourcescreen=PCKPART"))
                Else
                    ClearSessionWEIGHTNEEDEDCount()
                    Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
                End If
            Else
                ClearSession()
                Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
            End If



        Catch ex As System.Threading.ThreadAbortException

        End Try


    End Sub

    Private Sub goNextPick(ByVal pckJob As WMS.Logic.PickJob)

        Dim srcScreen As String
        If Not IsNothing(Session("PICKINGSRCSCREEN")) Then
            srcScreen = Session("PICKINGSRCSCREEN")
        Else
            srcScreen = "PCKPART"
        End If

        Try

            ClearSession()

        Catch ex As Exception

        End Try
        Try
            If pckJob Is Nothing Then
                If Not Session("PCKBagOutPicking") Is Nothing Then
                    Dim tm As New WMS.Logic.TaskManager
                    If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARTIALPICKING, Nothing) Then
                        Dim TMTask As WMS.Logic.Task = tm.GetTaskFromTMService(WMS.Logic.Common.GetCurrentUser, True, LogHandler.GetRDTLogger())
                    End If
                    If Session("PCKPicklist") IsNot Nothing Then
                        Dim pcklst As Picklist = Session("PCKPicklist")
                        pcklst.Load()
                        If pcklst.isCompleted Then
                            Session.Remove("PCKListToResume")
                        Else
                            Session("PCKListToResume") = pcklst.PicklistID
                        End If
                        If pcklst.GetTotalPickedQty = 0 Then
                            Response.Redirect(MapVirtualPath("screens/PCKBagOut.aspx?sourcescreen=" & srcScreen))
                        End If
                        If pcklst.ShouldPrintBagOutReportOnComplete Then
                            Response.Redirect(MapVirtualPath("Screens/PCKBAGOUTPRINT.aspx"))
                        Else
                            Response.Redirect(MapVirtualPath("screens/BagOutCloseContainer.aspx?sourcescreen=PCKBagOut"))
                        End If
                    Else
                        Response.Redirect(MapVirtualPath("screens/BagOutCloseContainer.aspx?sourcescreen=PCKBagOut"))
                    End If
                Else


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
                End If
            Else
                Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
            End If

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub
    Private Sub doFinish()
        If (Session("WeightNeededPickJob") Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("m4nScreens/Login.aspx"))
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim numOfCaptured As Integer = DO1.Value("CAPTURED")
        Dim numOfConfirmed As Integer = DO1.Value("CONFIRMED")
        If numOfCaptured < numOfConfirmed Then

            MyBase.WriteToRDTLog("Finished Capturing. Need to capture weight for more cases")


            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Need to capture weight for more cases"))
            Return
        End If


        MyBase.WriteToRDTLog("Finished Capturing weight.")


        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")

        Dim ret As Boolean = True
        Dim srcScreen As String
        If Not IsNothing(Session("PICKINGSRCSCREEN")) Then
            srcScreen = Session("PICKINGSRCSCREEN")
        Else
            srcScreen = ""

            MyBase.WriteToRDTLog("srcScreen value = " + srcScreen + " ---- ")

        End If

        If srcScreen.ToString().ToLower() = "pckpart" Then
            ret = finishPartial(pckJob)
        ElseIf srcScreen.ToString().ToLower().Contains("pckfull") Then
            ret = finishFullPick(pckJob)
        Else

            ret = finishParallel(pckJob)
        End If
        Try
            Session.Remove("WeightNeededConfirm1")
            Session.Remove("WeightNeededConfirm2")
            MyBase.WriteToRDTLog(".......................................................................")
            MyBase.WriteToRDTLog("..............Started Inserting Case weight..............")
            InsertCasesWeight()
            MyBase.WriteToRDTLog("..............Finished Inserting Case weight..............")
            MyBase.WriteToRDTLog(".......................................................................")
        Catch ex As Exception

        End Try

        goNextPick(pckJob)
        '  End If

    End Sub

    'Added for RWMS-202
    ''' <summary>
    ''' Modified the code for the JIRA item RWMS-671
    ''' </summary>
    ''' <param name="Loadid"></param>
    ''' <param name="Weight"></param>
    ''' <remarks></remarks>
    Private Sub updateToLoadWeight(ByVal Loadid As String, ByVal Weight As Decimal)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim SQL As String
            If Not Loadid Is Nothing Then
                SQL = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", Weight, Loadid)
            Else
                SQL = String.Format("INSERT INTO ATTRIBUTE (PKEYTYPE,PKEY1,PKEY2,PKEY3,WEIGHT) VALUES ('LOAD','{1}',' ',' ',{0})", Weight, Loadid)
            End If

            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Can't update the Attribute table."))
        End Try
    End Sub
    'End Added for RWMS-202

    Private Sub updateFromLoadWeight()
        Dim newWeight As Decimal
        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
        Dim LD As New Load(pckJob.fromload)
        Try
            If Not IsNumeric(LD.GetAttribute("WEIGHT")) Then
                newWeight = 0
            ElseIf Decimal.Parse(LD.GetAttribute("WEIGHT") - DO1.Value("TOTAL")) < 0 Then
                newWeight = 0
            Else
                newWeight = Decimal.Parse(LD.GetAttribute("WEIGHT") - DO1.Value("TOTAL"))
            End If

            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", newWeight, LD.LOADID)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        Catch ex As Exception
            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "can't update flomload weight")
        End Try
    End Sub
    Private Sub InsertCasesWeight()

        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        Dim ret As Boolean = True

        'Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
        Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")
        'Added for RWMS-784
        Dim barcodeList As Dictionary(Of Integer, String) = Session("BarCodeList")
        Dim dateList As Dictionary(Of Integer, String) = Session("DateList")
        Dim aicodeList As Dictionary(Of Integer, String) = Session("AICodeList")
        'End Added for RWMS-784
        'Commented for RWMS-784
        'Dim sql As String = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) values"
        'End Commented for RWMS-784
        'Added for RWMS-784
        Dim sql As String = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER, UCC128, AICODE, UCC128Date) values"
        'End Added for RWMS-784
        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
        Dim user As String = WMS.Logic.GetCurrentUser

        Dim sqlUomNum As String = "select ISNULL(max(UOMNUM),0) from LOADDETWEIGHT where LOADID='{0}'"
        Dim sqlToLoad As String
        Dim dt As New DataTable
        Dim toload As String
        Dim units As Decimal
        Dim casesListCounter As Integer = 1
        Dim weight As Decimal
        'Added for RWMS-784
        Dim barcode As String
        Dim aicode As String
        Dim ucc128date As String
        'End Added for RWMS-784
        Dim SumWeight As Decimal
        Dim AllLines As String = ""
        Try
            If srcScreen.ToString().ToLower() = "pckpart" Or srcScreen.ToString().ToLower().Contains("pckfull") Then
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
            Else 'parallel
                sqlToLoad = "select toload, units from vParallelPickComplete where fromload='{0}' and PARALLELPICKID='{1}' and sku='{2}'"
                sqlToLoad = String.Format(sqlToLoad, pckJob.fromload, pckJob.parallelpicklistid, pckJob.sku)
                MyBase.WriteToRDTLog("sqlToLoad :" + sqlToLoad)
            End If
            Made4Net.DataAccess.DataInterface.FillDataset(sqlToLoad, dt)
            Dim ld As WMS.Logic.Load
            Dim loadArrayList As New List(Of String)() ''RWMS-1315 Attribute table update wrong for partial pick weight
            MyBase.WriteToRDTLog("Start Outer for loop...")
            For Each dr As DataRow In dt.Rows
                MyBase.WriteToRDTLog("Iterating the LOADS...")
                SumWeight = 0
                toload = dr("toload")
                '' Start 'RWMS-1315 Attribute table update wrong for partial pick weight
                If Not loadArrayList.Contains(toload) Then
                    loadArrayList.Add(toload)
                End If
                '' End 'RWMS-1315 Attribute table update wrong for partial pick weight
                ld = New Load(toload)
                units = Convert.ToInt32(Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format(sqlUomNum, toload)))
                ' units = ld.UOMUnits
                For i As Integer = units To units + ld.UOMUnits - 1
                    MyBase.WriteToRDTLog("Iterating the LOAD UOM units...")
                    If casesListCounter <= casesList.Count Then
                        weight = casesList(casesListCounter)
                        'Added for RWMS-784
                        barcode = barcodeList(casesListCounter)
                        aicode = aicodeList(casesListCounter)
                        ucc128date = dateList(casesListCounter)
                        'End Added for RWMS-784
                        SumWeight = SumWeight + weight
                        MyBase.WriteToRDTLog("weight :" + weight.ToString())
                        MyBase.WriteToRDTLog("barcode :" + barcode)
                        MyBase.WriteToRDTLog("aicode :" + aicode)
                        MyBase.WriteToRDTLog("ucc128date :" + ucc128date)
                        MyBase.WriteToRDTLog("SumWeight :" + SumWeight.ToString())


                        'Commented for RWMS-784
                        'If casesListCounter = 1 Then
                        '    sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}') "
                        'Else
                        '    sql += ", ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}') "
                        'End If
                        'sql = String.Format(sql, toload, pckJob.uom, i + 1, weight, user)
                        'End Commented for RWMS-784
                        'Added for RWMS-784
                        If casesListCounter = 1 Then
                            If Not ucc128date Is Nothing Then
                                sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}', '{5}', '{6}', '{7}') "
                            Else
                                sql += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}', '{5}', '{6}', NULL) "
                            End If
                        Else
                            If Not ucc128date Is Nothing Then
                                sql += ", ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', '{5}', '{6}', '{7}') "
                            Else
                                sql += ", ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', '{5}', '{6}', NULL) "
                            End If
                        End If
                        sql = String.Format(sql, toload, pckJob.uom, i + 1, weight, user, barcode, aicode, ucc128date)
                        'End Added for RWMS-784
                        MyBase.WriteToRDTLog("sql :" + sql)
                        casesListCounter = casesListCounter + 1
                        MyBase.WriteToRDTLog("casesListCounter :" + casesListCounter.ToString())
                    Else
                        MyBase.WriteToRDTLog("Exiting Inner for loop.")
                        Exit For
                    End If
                Next

                ' If pckJob.PickDetLines.Count > 0 And srcScreen.ToString().ToLower() = "pckpart" And SumWeight > 0 Then
                '' Start Commented for RWMS-1315 - Attribute table update wrong for partial pick weight
                'If srcScreen.ToString().ToLower() = "pckpart" Or srcScreen.ToString().ToLower().Contains("pckfull") Then
                '    updateToLoadWeight(toload, SumWeight)
                'End If
                '' Start Commented for RWMS-1315 - Attribute table update wrong for partial pick weight
                'End If

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
            Dim isWeightCaptureNeeded As Boolean = SKU.weightNeeded(pckJob.oSku)
            For Each strToLoadid In loadArrayList
                updateAttToLoadWeight(strToLoadid)
                If (isWeightCaptureNeeded) Then
                    updateAttFromLoadWeight(strToLoadid)
                End If

            Next
            'END RWMS-1315 - Attribute table update wrong for partial pick weight
            MyBase.WriteToRDTLog("Finished updating attribute load weight.")
        Catch ex As Exception
            MyBase.WriteToRDTLog("error :" + ex.ToString())
        End Try
    End Sub
    'Start RWMS-1315 - Attribute table update wrong for partial pick weight
    Private Sub updateAttToLoadWeight(ByVal strToLoadid As String)
        Try
            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT=(SELECT SUM(UOMWEIGHT) FROM LOADDETWEIGHT WHERE LOADID='{0}') WHERE PKEY1='{0}' AND PKEYTYPE='LOAD'", strToLoadid)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
            MyBase.WriteToRDTLog("SQL :" + SQL)
        Catch ex As Exception
            MyBase.WriteToRDTLog("error :" + ex.ToString())
        End Try
    End Sub
    Private Sub updateAttFromLoadWeight(ByVal strToLoadid As String)
        Try
            Dim dtFromLoads As New DataTable
            Dim sqlFromLoads As String = String.Format("SELECT DISTINCT FROMLOAD FROM PICKDETAIL WHERE TOLOAD='{0}'", strToLoadid)
            Made4Net.DataAccess.DataInterface.FillDataset(sqlFromLoads, dtFromLoads)
            For Each dr As DataRow In dtFromLoads.Rows
                Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT= WEIGHT - (SELECT SUM(UOMWEIGHT) FROM LOADDETWEIGHT WHERE LOADID='{0}') WHERE PKEY1='{1}' AND PKEYTYPE='LOAD'", strToLoadid, dr("FROMLOAD"))
                Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
                MyBase.WriteToRDTLog("SQL :" + SQL)
            Next
        Catch ex As Exception
            MyBase.WriteToRDTLog("error :" + ex.ToString())
        End Try
    End Sub
    'End RWMS-1315 - Attribute table update wrong for partial pick weight
    Private Sub updateAddFromLoadWeight()
        Dim newWeight As Decimal
        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
        Dim LD As New Load(pckJob.fromload)
        Try

            If Decimal.Parse(LD.GetAttribute("WEIGHT") - DO1.Value("TOTAL")) < 0 Then
                newWeight = 0
            Else
                newWeight = Decimal.Parse(LD.GetAttribute("WEIGHT") - DO1.Value("TOTAL"))
            End If

            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT=WEIGHT + '{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", newWeight, LD.LOADID)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

        Catch ex As Exception
            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "can't update flomload weight")
        End Try
    End Sub
    Private Function finishPartial(ByRef pckJob As WMS.Logic.PickJob) As Boolean
        Dim ret As Boolean = True

        ' pckJob.oAttributeCollection.Item("WEIGHT") = DO1.Value("Total")
        Dim pcklst As New WMS.Logic.Picklist(pckJob.picklist)
        Try
            Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)
            Dim weight As String
            Try
                If Not IsNothing(Session("AttContainerID")) Then
                    weight = Session("AttContainerID").Item("WEIGHT")
                End If
            Catch ex As Exception
            End Try

            pckJob.oAttributeCollection = ExtractAttributes()

            'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
            '    Session("PCKPickLineSecond") = PCKPickMulti.splitPick(pckJob, osku.ConvertToUnits(pckJob.uom) * Session("UOMUnits_2"))
            '    If Not IsNothing(Session("PCKPickLineSecond")) Then
            '        'Dim pckLineSecond As WMS.Logic.PicklistDetail = Session("PCKPickLineSecond")
            '        'Dim sku As New SKU(pckJob.consingee, pckJob.sku)
            '        ''pckLineSecond.Pick(sku.ConvertToUnits(pckJob.uom) * Session("UOMUnits_2"), "EACH", WMS.Logic.GetCurrentUser, pckJob.oAttributeCollection)
            '        'pckLineSecond.Pick(Session("UOMUnits_2"), pckLineSecond.UOM, WMS.Logic.GetCurrentUser, pckJob.oAttributeCollection)


            '        MobileUtils.PickRemaiderUnits(pckJob)

            '        ' pckJob.oAttributeCollection = Session("AttContainerID")

            '        'Dim att As WMS.Logic.AttributesCollection = Session("AttContainerID")
            '        pckJob.oAttributeCollection.Item("WEIGHT") = weight ' att.Item("WEIGHT")
            '    End If
            'End If


            If Not String.IsNullOrEmpty(Session("WeightNeededConfirm2")) Then
                pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), HttpContext.Current.Session("WeightNeededConfirm2"), pckJob.fromwarehousearea)
                pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                'pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser, True, Session("WeightNeededConfirm2"))
            Else
                pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), "", pckJob.fromwarehousearea)
                pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                'pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser)
            End If

            'If IsNothing(pckJob) And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '    Session.Remove("PCKPicklist")
            '    Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=PCK"))
            'End If

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            ret = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            'Return
        Catch ex As Exception
            ret = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            'Return
        End Try
        Return ret
    End Function
    Private Function finishFullPick(ByRef pck As WMS.Logic.PickJob) As Boolean
        Dim ret As Boolean = True

        Dim pcklst As New WMS.Logic.Picklist(pck.picklist)
        Try
            pck.oAttributeCollection.Item("WEIGHT") = DO1.Value("TOTAL") 'pck.oAttributeCollection.Item("WEIGHT")
            Dim SQL As String = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", pck.oAttributeCollection.Item("WEIGHT"), pck.fromload)
            Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        Catch ex As Exception
        End Try

        Try
            'Commented for RWMS-510
            'pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM").Trim(), "", DO1.Value("WAREHOUSEAREA").Trim())
            pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, pck.fromload, "", "")
            pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
            'End Added for RWMS-510
            '   pck = PickTask.Pick(Session("WeightNeededConfirm1"), pck.fromwarehousearea, pcklst, pck, WMS.Logic.Common.GetCurrentUser)

        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            ret = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            'Return
        Catch ex As Exception
            ret = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            'Return
        End Try

        Return ret
    End Function
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

    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("WeightNeededPickJob")
        ' Dim Val As Object
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    'Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    'If Val = "" Then Val = Nothing
                    If (pck.oAttributeCollection.Keys(idx).ToUpper = "WEIGHT" Or pck.oAttributeCollection.Keys(idx).ToUpper = "WGT") Then
                        pck.oAttributeCollection(idx) = DO1.Value("TOTAL") 'Val
                    End If

                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function


    Private Function finishParallel(ByRef pckJob As WMS.Logic.PickJob) As Boolean
        Dim ret As Boolean = True

        Dim pcks As ParallelPicking = New ParallelPicking(pckJob.parallelpicklistid)
        Try
            pckJob.oAttributeCollection = ExtractAttributes()

            pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), "", pckJob.fromwarehousearea)
            pckJob = pcks.Pick(pckJob, WMS.Logic.GetCurrentUser, Nothing)

            ' pckJob = pcks.Pick(Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pckJob, WMS.Logic.Common.GetCurrentUser)
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then

                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(GetCurrentUser, TASKTYPE.PARALLELPICKING, LogHandler.GetRDTLogger())
                Dim pcklsts As New ParallelPicking(tm.Task.ParallelPicklist)
                Session("PARPCKPicklist") = pcklsts
            End If
        Catch ex As Made4Net.Shared.M4NException
            ret = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))

        Catch ex As Exception
            ret = False
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)

        End Try
        Return ret
    End Function

    Private Sub doDelLast()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Session("LastWeight") <> 0 Then ' DO1.Value("Last") <> 0 Then

            'Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
            Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")
            'Added for RWMS-784
            Dim barcodeList As Dictionary(Of Integer, String) = Session("BarCodeList")
            Dim dateList As Dictionary(Of Integer, String) = Session("DateList")
            Dim aicodeList As Dictionary(Of Integer, String) = Session("AICodeList")
            'End Added for RWMS-784
            If casesList.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No weight to remove"))
                Return
            End If
            '
            Dim lastWeight As Decimal = casesList(casesList.Count)
            '
            'Dim lastWeight As Decimal = casesList(casesList.Count - 1)

            DO1.Value("TOTAL") = Decimal.Parse(DO1.Value("TOTAL")) - lastWeight
            'casesList.RemoveAt(casesList.Count - 1)
            'Commented for RWMS-784
            'casesList.Remove(GetCurrentCaseNumber())
            'Session("CasesList") = casesList
            'End Commented for RWMS-784
            'Added for RWMS-784
            Dim intCase As Integer = GetCurrentCaseNumber()
            casesList.Remove(intCase)
            Session("CasesList") = casesList
            barcodeList.Remove(intCase)
            Session("BarCodeList") = barcodeList
            dateList.Remove(intCase)
            Session("DateList") = dateList
            aicodeList.Remove(intCase)
            Session("AICodeList") = aicodeList
            'End Added for RWMS-784
            DO1.Value("CAPTURED") = casesList.Count
            DO1.Value("Last") = 0 ' casesList.Item(casesList.Count - 1)
            Session("LastWeight") = 0

            DO1.Value("WEIGHT") = ""
        End If

    End Sub


    Private Function VALIDATEWEIGHT(ByRef pck As PickJob, ByVal currentWeight As Decimal) As Boolean
        Dim RETVAL As Boolean = True
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'added validation code for weight
        Dim oSku As New WMS.Logic.SKU(pck.consingee, pck.sku)
        Dim PICKOVERRIDEVALIDATOR As String
        PICKOVERRIDEVALIDATOR = getPICKOVERRIDEVALIDATOR(pck.consingee, pck.sku)

        '  If weightNeeded(oSku) And Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then
        If Not String.IsNullOrEmpty(PICKOVERRIDEVALIDATOR) Then
            'New Validation with expression evaluation
            Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

            vals.Add("CONSIGNEE", pck.consingee)
            vals.Add("SKU", oSku.SKU)
            vals.Add("CASEWEIGHT", currentWeight) 'DO1.Value("WEIGHT"))

            Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
            exprEval.FieldValues = vals
            Dim statement As String = "[0];func:" & PICKOVERRIDEVALIDATOR & "(FIELD:CONSIGNEE,FIELD:SKU,FIELD:CASEWEIGHT)"
            Dim ret As String
            Try
                ret = exprEval.Evaluate(statement)
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal validation function") & statement & ex.Message)
                Return False
            End Try

            Dim returnedResponse() As String = ret.Split(";")
            If returnedResponse(0) = "0" Then
                RETVAL = False
                gotoOverride(returnedResponse(1))

            ElseIf returnedResponse(0) = "-1" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ret)
                Return False
            End If
        End If
        Return RETVAL
    End Function

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


    Private Sub gotoOverride(ByVal errorOverride As String)
        Session("ERROROVERRIDE") = errorOverride

        'Session("WeightNeededPickJob") = Session("PCKPicklistPickJob")

        Session("WeightOverridConfirm") = DO1.Value("WEIGHT")

        Try
            Response.Redirect(MapVirtualPath("screens/PCKOVERRIDEWEIGHT.aspx?sourcescreen=" & Session("PICKINGSRCSCREEN")))

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

End Class