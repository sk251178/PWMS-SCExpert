Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WMS.Logic
Imports System.Threading

Partial Public Class CreateRepl

    Inherits PWMSRDTBase
    Protected repId As Guid

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("RelpTaskID")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim sql As String = ""

            Try
                If (String.IsNullOrEmpty(DO1.Value("PICKLOC")) And String.IsNullOrEmpty(DO1.Value("SKU"))) Then
                    Throw New Made4Net.Shared.M4NException(New Exception(), "Can not leave fields empty", "Can not leave fields empty")
                End If

                sql = getSQLQuery()
            Catch m4nEx As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As ApplicationException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
                Return
            End Try
            Dim sku As String = String.Empty
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

            If dt.Rows.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No pick location found"))
            Else
                Dim repl As New Replenishment
                If Not repl.ReplenishmentExists(dt.Rows(0)("SKU"), dt.Rows(0)("LOCATION"), dt.Rows(0)("CONSIGNEE"), dt.Rows(0)("WAREHOUSEAREA")) Then
                    If Not (IsLoadExistsManual(dt)) Then
                        DO1.Value("PICKLOC") = ""
                        Throw New Made4Net.Shared.M4NException(New Exception, "No Payloads found for replenishment.", "No Payloads found for replenishment.")
                    End If
                End If
                For Each dr As DataRow In dt.Rows
                    If (WMS.Logic.PickLoc.IsBatchPickLocation(dr("CONSIGNEE"), dr("LOCATION"))) Then
                        Dim message As String = String.Format("SKU {0} in Location {1} is a Batch Pick Location", dr("SKU"), dr("LOCATION"))
                        Throw New Made4Net.Shared.M4NException(New Exception(), message, message)
                    End If
                Next
                sku = dt.Rows(0)("SKU").ToString()
                Dim manRepl As New ManualReplenishment()
                For Each dr As DataRow In dt.Rows
                    manRepl.ManualLocationReplenish(dr("LOCATION"), dr("WAREHOUSEAREA"), dr("CONSIGNEE"), dr("SKU"), "", WMS.Logic.Common.GetCurrentUser)
                Next
                '----Start sleep wait
                ' assign an unique ID

                repId = Guid.NewGuid()

                    ' start a new thread
                    Dim ts As New ThreadStart(AddressOf LongRunningProcess)
                    Dim th As New Thread(ts)

                    th.Start()
                ' redirect to waiting page
                Response.Redirect("StatusWait.aspx?SKU=" & sku)
                '----End sleep wait
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Man Replenishment Request For Picking Locations Sent"))
            End If


                DO1.Value("PICKLOC") = ""
            DO1.Value("PICKWAREHOUSEAREA") = WMS.Logic.Warehouse.CurrentWarehouseArea
            DO1.Value("CONSIGNEE") = ""
            DO1.Value("SKU") = ""
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
        End Try
    End Sub

    Private Function getSQLQuery() As String
        Dim sql As String = ""

        If String.IsNullOrEmpty(DO1.Value("PICKLOC")) Then

            If (String.IsNullOrEmpty(DO1.Value("SKU"))) Then
                Throw New Made4Net.Shared.M4NException(New Exception(), "Can not leave sku  field empty", "Can not leave sku field empty")
            End If



            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE") & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "ReqRepl"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' Changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE") & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC WHERE CONSIGNEE like '" & DO1.Value("CONSIGNEE") & "%' and (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')")
            Else
                Throw New Made4Net.Shared.M4NException(New Exception(), "SKU does not exist", "SKU does not exist")
            End If
        End If
        sql = String.Format("select location, warehousearea, consignee, sku from vpickloc where location like '{0}%' and " & _
        "warehousearea like '{1}%' and consignee like '{2}%' and sku like '{3}%'", DO1.Value("PICKLOC"), DO1.Value("PICKWAREHOUSEAREA"), _
         DO1.Value("CONSIGNEE"), DO1.Value("SKU"))

        Return sql
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("PICKLOC")
        DO1.AddTextboxLine("PICKWAREHOUSEAREA")
        DO1.AddTextboxLine("CONSIGNEE")
        DO1.setVisibility("CONSIGNEE", False)
        DO1.AddTextboxLine("SKU")
        DO1.setVisibility("PICKWAREHOUSEAREA", False)
        DO1.Value("PICKWAREHOUSEAREA") = WMS.Logic.Warehouse.CurrentWarehouseArea
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    'Jira-332 for dummy delay for create manual replen
    Protected Sub LongRunningProcess()

        ' do nothing actually, dummy delay till we figure message Q process time

        Thread.Sleep(1000)

        ' add result to the controller
        ProcessController.Add(repId, "Some result.")


    End Sub

    'Jira-332 Check if load exists at regions as per policy
    Public Function IsLoadExists(ByVal dt As DataTable) As Boolean
        Dim strLOCATION As String = String.Empty
        Dim strWAREHOUSEAREA As String = String.Empty
        Dim strCONSIGNEE As String = String.Empty
        Dim strSKU As String = String.Empty
        Dim pReplPolicy As String = String.Empty
        Dim strPICKREGION As String = String.Empty
        Dim strUOM As String = String.Empty
        Dim currentqty As Double = 0
        Dim tmpcurrentqty As Double = 0
        Dim repl As New Replenishment()
        Dim isLoadFnd As Boolean = False


        'Jira-332 Start Payload availability validation
        For Each dr As DataRow In dt.Rows
            strLOCATION = dr("LOCATION")
            strWAREHOUSEAREA = dr("WAREHOUSEAREA")
            strCONSIGNEE = dr("CONSIGNEE")
            strSKU = dr("SKU")
        Next
        Dim zb As New PickLoc(strLOCATION, strWAREHOUSEAREA, strCONSIGNEE, strSKU)
        pReplPolicy = zb.ReplPolicy


        Dim maxReplQty As Double = zb.MaximumReplQty
        Dim prTbl As DataTable = New DataTable

        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY", prTbl)

        Dim dt1 As New DataTable
        Dim dr1 As DataRow

        dt1 = repl.CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            dt1.Rows.Add(repl.CreatePolicyDetailRow(dt1, pReplPolicy, pr("PRIORITY")))
        Next

        If dt1.Rows.Count > 0 Then
            For i As Int32 = 0 To dt1.Rows.Count - 1
                dr1 = dt1.Rows(i)
                strPICKREGION = dr1("PICKREGION")
                strUOM = dr1("UOM")
                currentqty = getFullReplLoadInRegion(strPICKREGION, strUOM, pReplPolicy, zb, currentqty)
                tmpcurrentqty = tmpcurrentqty + currentqty
                If tmpcurrentqty >= maxReplQty Then
                    isLoadFnd = True
                    Exit For
                End If
            Next

        End If

        If Not (isLoadFnd) Then
            Return False
        Else
            Return True
        End If


        'Jira-332 End Payload availability validation


    End Function
    'Jira-332 Check if load exists at regions as per policy without hotmaxqty validation
    Public Function IsLoadExistsManual(ByVal dt As DataTable) As Boolean
        Dim strLOCATION As String = String.Empty
        Dim strWAREHOUSEAREA As String = String.Empty
        Dim strCONSIGNEE As String = String.Empty
        Dim strSKU As String = String.Empty
        Dim pReplPolicy As String = String.Empty
        Dim strPICKREGION As String = String.Empty
        Dim strUOM As String = String.Empty
        Dim currentqty As Double = 0
        Dim tmpcurrentqty As Double = 0
        Dim repl As New Replenishment()
        Dim isLoadFnd As Boolean = False


        'Jira-332 Start Payload availability validation
        For Each dr As DataRow In dt.Rows
            strLOCATION = dr("LOCATION")
            strWAREHOUSEAREA = dr("WAREHOUSEAREA")
            strCONSIGNEE = dr("CONSIGNEE")
            strSKU = dr("SKU")
        Next
        Dim zb As New PickLoc(strLOCATION, strWAREHOUSEAREA, strCONSIGNEE, strSKU)
        pReplPolicy = zb.ReplPolicy


        Dim maxReplQty As Double = zb.MaximumReplQty
        Dim prTbl As DataTable = New DataTable

        DataInterface.FillDataset("SELECT PRIORITY FROM REPLPOLICYDETAIL WHERE POLICYID = '" & pReplPolicy & "' GROUP BY PRIORITY", prTbl)

        Dim dt1 As New DataTable
        Dim dr1 As DataRow

        dt1 = repl.CreatePolicyDetailTable()
        For Each pr As DataRow In prTbl.Rows
            'Add Policy Detail to Details Table
            dt1.Rows.Add(repl.CreatePolicyDetailRow(dt1, pReplPolicy, pr("PRIORITY")))
        Next

        If dt1.Rows.Count > 0 Then
            For i As Int32 = 0 To dt1.Rows.Count - 1
                dr1 = dt1.Rows(i)
                strPICKREGION = dr1("PICKREGION")
                strUOM = dr1("UOM")


                If (getManualFullReplLoadInRegion(strPICKREGION, strUOM, pReplPolicy, zb, currentqty)) Then
                    isLoadFnd = True
                    Exit For
                End If
            Next

        End If

        If Not (isLoadFnd) Then
            Return False
        Else
            Return True
        End If


        'Jira-332 End Payload availability validation


    End Function

    <CLSCompliant(False)>
    Public Function getFullReplLoadInRegion(ByVal PickRegion As String, ByVal pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByRef CurrentQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "") As Decimal
        Dim sql As String
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim LOADID As String = String.Empty
        Dim ld As Load
        'Get Loads for scoring procedure
        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE <= {2} AND LOADID like '%{6}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {5} ", zb.Consignee, zb.SKU, zb.MaximumQty - CurrentQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND UNITSAVAILABLE <= {2} AND LOADID like '%{5}' ", zb.Consignee, zb.SKU, zb.MaximumQty - CurrentQty, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If
        Dim FullUomSize As Decimal
        If pAllocUOMQty <> "" Then
            Dim oSku As New SKU(zb.Consignee, zb.SKU)
            FullUomSize = oSku.ConvertToUnits(pAllocUOMQty)
            sql = sql & " and UNITSAVAILABLE = " & FullUomSize
        End If
        sql = sql & " ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC"
        DataInterface.FillDataset(sql, dt)

        ' ''If Not oLogger Is Nothing Then
        ' ''    oLogger.Write("Trying to get loads according to query:")
        ' ''    oLogger.Write(sql.ToString)
        ' ''End If

        If dt.Rows.Count > 0 Then
            For i As Int32 = 0 To dt.Rows.Count - 1
                dr = dt.Rows(i)
                LOADID = dr("LOADID")
                ld = New Load(LOADID)
                CurrentQty = CurrentQty + ld.UNITS
            Next

            Return CurrentQty

        Else

            Return Nothing
        End If
    End Function

    'Jira -332 check payload exists in inventory
    <CLSCompliant(False)>
    Public Function getManualFullReplLoadInRegion(ByVal PickRegion As String, ByVal pUom As String, ByVal pPolicyID As String, ByVal zb As PickLoc, ByRef CurrentQty As Double, Optional ByVal oLogger As LogHandler = Nothing, Optional ByVal pLoadId As String = "", Optional ByVal pAllocUOMQty As String = "") As Boolean
        Dim sql As String
        Dim dt As New DataTable
        Dim LOADID As String = String.Empty
        'Get Loads for scoring procedure
        If pLoadId = "" Then
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{5}' AND LOCATION NOT IN (SELECT LOCATION FROM PICKLOC WHERE CONSIGNEE = '{0}' AND SKU = '{1}') AND {4} ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, getStockFilterByPriority(PickRegion, pUom), pLoadId)
        Else
            sql = String.Format("SELECT * FROM vReplenishmentInventory WHERE CONSIGNEE = '{0}' AND SKU = '{1}' AND LOADID like '%{4}' ", zb.Consignee, zb.SKU, WMS.Lib.Statuses.LoadStatus.AVAILABLE, WMS.Lib.Statuses.ActivityStatus.NONE, pLoadId)
        End If
        Dim FullUomSize As Decimal
        If pAllocUOMQty <> "" Then
            Dim oSku As New SKU(zb.Consignee, zb.SKU)
            FullUomSize = oSku.ConvertToUnits(pAllocUOMQty)
            sql = sql & " and UNITSAVAILABLE = " & FullUomSize
        End If
        sql = sql & " ORDER BY RECEIVEDATE,UNITSAVAILABLE DESC"
        DataInterface.FillDataset(sql, dt)

        ' ''If Not oLogger Is Nothing Then
        ' ''    oLogger.Write("Trying to get loads according to query:")
        ' ''    oLogger.Write(sql.ToString)
        ' ''End If

        If dt.Rows.Count > 0 Then

            Return True

        Else

            Return False
        End If
    End Function

    Public Function getStockFilterByPriority(ByVal sPickRegions As String, ByVal sUoms As String) As String
        Dim aPickRegions As String() = sPickRegions.Split(",")
        Dim aUoms As String() = sUoms.Split(",")

        If aPickRegions.Length = 1 Then
            Return " (PICKREGION LIKE '" & sPickRegions & "' AND LOADUOM LIKE '" & sUoms & "') "
        End If
        If aUoms.Length >= 1 Then
            sUoms = aUoms(0)
        End If
        Dim SQLFilter As String = "LOADUOM LIKE '" & sUoms & "' AND ("
        For i As Int32 = 0 To aPickRegions.Length - 1
            SQLFilter = SQLFilter & " (PICKREGION LIKE '" & aPickRegions(i) & "') OR"
        Next
        SQLFilter = " (" & SQLFilter.TrimEnd("OR".ToCharArray) & ")) "
        Return SQLFilter
    End Function

End Class