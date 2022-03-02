Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports System.Collections.Generic
Imports Made4Net.Shared

Partial Public Class PCKOVERRIDEWEIGHT
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session("PICKINGSRCSCREEN") = Request("sourcescreen")
            Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
            setScreen(pckJob)

        End If
    End Sub

    Private Sub setScreen(ByVal pPickJob As WMS.Logic.PickJob)
        DO1.Value("ERROR") = Session("ERROROVERRIDE")
        DO1.Value("SKU") = pPickJob.sku
        DO1.Value("SKUDESC") = pPickJob.skudesc
        DO1.Value("CapturedWeight") = Session("WeightOverridConfirm")

        Dim WGT As Decimal = 0, TOLPCT As Decimal = 0
        Dim sql As String = String.Format("select isnull(WGT,0) WGT, isnull(TOLPCT,0) TOLPCT from SKUATTRIBUTE where CONSIGNEE = '{0}' and SKU = '{1}'", pPickJob.consingee, pPickJob.sku)

        Dim dt As New DataTable()
        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            WGT = Math.Truncate(Decimal.Parse(dt.Rows(0)("WGT").ToString()))
            TOLPCT = Math.Truncate(Decimal.Parse(dt.Rows(0)("TOLPCT").ToString()))
        Catch
            WGT = 0
            TOLPCT = 0
        End Try
        Dim oSku As New WMS.Logic.SKU(pPickJob.consingee, pPickJob.sku)

        Try
            DO1.Value("AverageCapturedWeight") = GetWeightPerCase(oSku, pPickJob.units)
        Catch ex As Exception
        End Try

        DO1.Value("WGT") = WGT
        DO1.Value("TOLPCT") = TOLPCT
        
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("ERROR")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        Dim scrn As String
        scrn = Session("PICKINGSRCSCREEN")
        If scrn = "PCKFULL" Then
            'AVERAGEWEIGHT
            DO1.AddLabelLine("AverageCapturedWeight")
        End If


        DO1.AddLabelLine("CapturedWeight")
       
        DO1.AddLabelLine("WGT")
        DO1.AddLabelLine("TOLPCT")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "override"
                doOverride()
            Case "back"
                doBack()
           
        End Select
    End Sub

 
    Private Sub doBack()

        Dim srcScreen As String = Session("PICKINGSRCSCREEN")
        ''Session.Remove("PICKINGSRCSCREEN")
        'Session.Remove("WeightNeededConfirm1")
        'Session.Remove("WeightNeededConfirm2")
        Session.Remove("WeightOverridConfirm")
        'Begin for RWMS-1288 and RWMS-1043   
        Session.Remove("overrideBarcode")
        Session.Remove("overrideAICode")
        Session.Remove("overrideUCC128Date")
        'End for RWMS-1288 and RWMS-1043  

        Try
            If srcScreen.ToString().ToLower() = "pckfull" Then
                If isWeightCaptureNeeded Then
                    Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=" & srcScreen))
                Else
                    Response.Redirect(MapVirtualPath("screens/" & srcScreen & ".aspx"))
                End If
            Else
                Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=" & srcScreen))
            End If
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub doOverride()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim srcScreen As String = Session("PICKINGSRCSCREEN")


        'If srcScreen.ToString().ToLower() = "pckpart" Then
        '    finishPartial()
        'ElseIf srcScreen.ToString().ToLower() = "parpck2" Then
        '    finishParallel()
        'Else ' fullpick
        '    finishFull()
        'End If


        'RWMS-634
        'If srcScreen.ToString().ToLower() = "pckfull" Then
        '    finishFull()
        'Else
        'Dim casesList As System.Collections.Generic.List(Of Decimal) = Session("CasesList")
        Dim WeightOverridConfirm As Decimal = Session("WeightOverridConfirm")
        Dim WeightNeededTOTAL As Decimal = Session("WeightNeededTOTAL")

        'casesList.Add(Math.Round(WeightOverridConfirm, 2))
        'Begin for RWMS-1288 and RWMS-1043   
        'AddtoCaseList(Math.Round(WeightOverridConfirm, 2))
        If Session("CasesList") Is Nothing Then
            Dim casesList As New Dictionary(Of Integer, Decimal)
            Session("CasesList") = casesList
        End If
        If srcScreen.ToString().ToLower() = "pckpart" Then
            AddtoCaseList(Session("overrideBarcode"), Math.Round(WeightOverridConfirm, 2), Session("overrideUCC128Date"), Session("overrideAICode"))
            Session("LastWeight") = Math.Round(WeightOverridConfirm, 2)
            Session("WeightNeededTOTAL") = Math.Round(WeightOverridConfirm + WeightNeededTOTAL, 2)
        ElseIf srcScreen.ToString().ToLower().Contains("pckfull") Then
            If isWeightCaptureNeeded Then
                AddtoCaseList(Session("overrideBarcode"), Math.Round(WeightOverridConfirm, 2), Session("overrideUCC128Date"), Session("overrideAICode"))
                Session("LastWeight") = Math.Round(WeightOverridConfirm, 2)
                Session("WeightNeededTOTAL") = Math.Round(WeightOverridConfirm + WeightNeededTOTAL, 2)
            Else
                Session("MobileSourceScreen") = "PCK"
                finishFull()

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
        End If
        'End If



        sendMessageQ()

        doBack()
    End Sub
    'Begin For RWMS-1288 and RWMS-1043 - Below method is obselete  
    Private Function AddtoCaseList(ByVal dCaseWeight As Decimal) As Integer
        Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")
        Dim intCase As Integer = casesList.Count + 1
        If casesList.ContainsKey(intCase) Then
            casesList.Item(intCase) = dCaseWeight
        Else
            casesList.Add(intCase, dCaseWeight)
        End If
        Session("CasesList") = casesList
        Return casesList.Count
    End Function
    'end For RWMS-1288 and RWMS-1043 - Below method is obselete  
    'Begin For RWMS-1288 and RWMS-1043 - Added the new parameters sbarcode, sUCC128Date, sAICOde   
    Private Function AddtoCaseList(ByVal sbarcode As String, ByVal dCaseWeight As Decimal, ByVal sUCC128Date As String, ByVal sAICode As String) As Integer
        'Weight   
        Dim casesList As Dictionary(Of Integer, Decimal) = Session("CasesList")
        Dim intCase As Integer = casesList.Count + 1
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
:
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
        Return casesList.Count
    End Function
    'End for  RWMS-1288 and RWMS-1043   

    Private Sub finishFull()
        Dim pckJob As WMS.Logic.PickJob = Session("WeightNeededPickJob")
        Dim pcklst As Picklist = Session("PCKPicklist")
        Dim SQL As String
        Try
            pckJob.oAttributeCollection = ExtractAttributes()
            Try
                SQL = "insert into LOADDETWEIGHT( LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) values"
                SQL += " ('{0}', '{1}', '{2}', '{3}',getdate(), '{4}') "
                SQL = String.Format(SQL, pckJob.fromload, pckJob.uom, 1, Session("WeightOverridConfirm"), WMS.Logic.GetCurrentUser)
                Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
                SQL = String.Format("UPDATE ATTRIBUTE SET WEIGHT='{0}' WHERE PKEYTYPE = 'LOAD' AND PKEY1 = '{1}'", Session("WeightOverridConfirm"), pckJob.fromload)
                Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
            Catch ex As Exception
            End Try

            pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), HttpContext.Current.Session("WeightNeededConfirm2"), pckJob.fromwarehousearea)
            pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

            'pckJob = PickTask.Pick(Session("WeightNeededConfirm1"), Session("WeightNeededConfirm2"), pcklst, pckJob, WMS.Logic.Common.GetCurrentUser)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        Finally
            Session.Remove("WeightNeededConfirm1")
            Session.Remove("WeightNeededConfirm2")
        End Try
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
                        pck.oAttributeCollection(idx) = Session("WeightOverridConfirm")
                    End If

                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function

    Private Sub sendMessageQ()
        Dim pck As WMS.Logic.PickJob = Session("WeightNeededPickJob")

        ' Dim MSG As String = "WGTOVRRD"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WGTOVRRD)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WGTOVRRD)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", pck.consingee)
        Dim strLines As String = "1"
        Try
            strLines = pck.PickDetLines(0)
        Catch ex As Exception
        End Try

        Dim srcScreen As String = Session("PICKINGSRCSCREEN")

        aq.Add("DOCUMENT", pck.picklist)
        aq.Add("DOCUMENTLINE", strLines)
        aq.Add("FROMLOAD", pck.fromload)
        aq.Add("FROMLOC", pck.fromlocation)
        'aq.Add("FROMQTY", Session("CreateLoadUnits"))
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", Session("ERROROVERRIDE"))
        aq.Add("SKU", pck.sku)
        If srcScreen.ToString().ToLower() = "pckfull" Then
            aq.Add("TOLOAD", pck.fromload)
            aq.Add("TOQTY", pck.units)
        Else
            aq.Add("TOCONTAINER", pck.container)
            aq.Add("TOQTY", "1")
        End If


        aq.Add("TOLOC", "")
        'aq.Add("TOSTATUS", Session("CreateLoadStatus"))
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(WMS.Lib.Actions.Audit.WGTOVRRD)


    End Sub



    Private Function GetWeightPerCase(ByVal oSku As WMS.Logic.SKU, ByVal units As Integer) As Decimal
        Dim d As Decimal = 0

        'RWMS-635
        'Dim dWeight As Decimal = Session("WeightOverridConfirm")
        Dim dWeight As Decimal = Session("WeightNeededTOTAL")

        Try
            d = Math.Round(dWeight / oSku.ConvertUnitsToUom("CASE", units), 2)
        Catch ex As Exception
            d = 0
        End Try
        Return d
    End Function

    Public ReadOnly Property isWeightCaptureNeeded() As Boolean
        Get
            Try
                Return Not WMS.Logic.GetSysParam("ForceWeightCaptureInFullPick") = 0
            Catch ex As SysParamNotFoundException
                Return False
            End Try
        End Get
    End Property

End Class