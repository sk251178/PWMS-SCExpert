Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class LOCBLKCNTTASK1
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    'Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        If Not Page.IsPostBack Then
            If Session("CountingSrcScreen") Is Nothing Then
                Session("CountingSrcScreen") = "LOCBLKCNTTASK1"
            End If
        End If
        DO1.Value("LOCATION") = Session("TaskLocationCNTLocationId")
        DO1.Value("WAREHOUSEAREA") = Session("TaskWarehouseareaCNTWarehouseareaId")

        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("UNITS") = Session("SKUSEL_UNITS")
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_UNITS")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If


    End Sub

    Private Sub doBack()
        Session.Remove("TaskLocationCNTLocationId")
        Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
        Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSrcScreen") & ".aspx"))
    End Sub

    Private Sub doEndCount()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If CanCompleteCount() Then
                Dim toqty, fromQty As Decimal
                CalcQty(fromQty, toqty)
                Dim oCntTask As WMS.Logic.CountTask
                Dim oCounting As New WMS.Logic.Counting
                Dim oCountJob As WMS.Logic.CountingJob
                If Not Session("LocationBulkCountTask") Is Nothing Then
                    oCntTask = Session("LocationBulkCountTask")
                    oCounting = New WMS.Logic.Counting(oCntTask.COUNTID)
                    'Build and fill count job object
                    If Session("LocationBulkCountJob") Is Nothing Then
                        oCountJob = oCntTask.getCountJob(oCounting)
                    Else
                        oCountJob = Session("LocationBulkCountJob")
                    End If
                    oCountJob.ExpectedQty = fromQty
                    oCountJob.CountedQty = toqty
                    oCountJob.CountedLoads = Session("TaskLocationCNTLoadsDT")
                    oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser)
                Else
                    'oCounting = New WMS.Logic.Counting()
                    'oCounting.COUNTTYPE = WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING
                    'oCounting.Count(fromQty, toqty, "", Session("TaskLocationCNTLocationId"), Session("TaskLocationCNTLoadsDT"), WMS.Logic.GetCurrentUser)
                End If
                Session.Remove("LocationCNTLoadId")
                Session.Remove("TaskLocationCNTLocationId")
                Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
                Session.Remove("TSKTaskId")
                Session("TaskID") = oCntTask.TASK
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location Count Completed"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Not All Loads Counted for the current Location. Please Confirm"))
                Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK3.aspx"))
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
        End Try
        Dim nextScreen As String = ""
        If Not Session("CountingSrcScreen") Is Nothing Then
            nextScreen = Session("CountingSrcScreen")
            'Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSrcScreen") & ".aspx"))
        Else
            nextScreen = "LOCBLKCNTTASK"
            'Response.Redirect(MapVirtualPath("Screens/LOCBLKCNTTASK.aspx"))
        End If

        If MobileUtils.ShouldRedirectToTaskSummary() Then
            MobileUtils.RedirectToTaskSummary(nextScreen)
        Else
            Response.Redirect(MapVirtualPath("Screens/" & nextScreen & ".aspx"))
        End If

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddTextboxLine("CONSIGNEE")
        DO1.AddTextboxLine("SKU")
        DO1.AddTextboxLine("UNITS")
        DO1.AddSpacer()
    End Sub


    Private Sub doNext()
        'If Page.IsValid Then
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("SKU").Trim <> "" Then
            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & Session("TaskLocationCNTLocationId") & "' AND RD.WAREHOUSEAREA='" & Session("TaskWarehouseareaCNTWarehouseareaId") & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "%' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "%')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "LOCBLKCNTTASK1"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_UNITS") = DO1.Value("UNITS").Trim
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE").Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) 'Changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & Session("TaskLocationCNTLocationId") & "' AND RD.WAREHOUSEAREA='" & Session("TaskWarehouseareaCNTWarehouseareaId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & Session("TaskLocationCNTLocationId") & "' AND RD.WAREHOUSEAREA='" & Session("TaskWarehouseareaCNTWarehouseareaId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
            End If
        End If

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim dt As New DataTable
        Dim Loc, Warehousearea, Cons, Sku As String
        Dim Units As Decimal
        Try
            Units = DO1.Value("UNITS")
        Catch ex As Exception
            Units = 0
        End Try
        Loc = Session("TaskLocationCNTLocationId")
        Warehousearea = Session("TaskWarehouseareaCNTWarehouseareaId")
        Cons = DO1.Value("CONSIGNEE")
        Sku = DO1.Value("SKU")
        Dim success As Boolean = False
        Try
            Dim oCntTask As WMS.Logic.CountTask = Session("LocationBulkCountTask")
            Dim oSku As WMS.Logic.SKU = getSku(Cons, Sku)
            success = oCntTask.ValidateBulkCount(Loc, Warehousearea, oSku.CONSIGNEE, oSku.SKU, Units, WMS.Logic.Common.GetCurrentUser)
            If Not success Then
                Dim oCounting As New WMS.Logic.Counting(oCntTask.COUNTID)
                'Build and fill count job object
                Dim oCountJob As WMS.Logic.CountingJob = oCntTask.getCountJob(oCounting)
                oCountJob.BulkCountLoadsCounted = True
                Session("LocationBulkCountJob") = oCountJob
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Counted Units does not match.Please Count Location"))
                Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK1.aspx"))
            End If
            UpdateLoadCount(oSku.CONSIGNEE, oSku.SKU)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("SKU counted successfully"))
            DO1.Value("CONSIGNEE") = ""
            DO1.Value("SKU") = ""
            DO1.Value("UNITS") = ""
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
        End Try
        'End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack()
            Case "endcount"
                doEndCount()
        End Select
    End Sub

    Private Function getSku(ByVal cons As String, ByVal sku As String) As WMS.Logic.SKU
        Dim sql As String = String.Format("select * from sku where consignee like '{0}%' AND (SKU like '{1}' or MANUFACTURERSKU like '{1}' or VENDORSKU ='{1}' or OTHERSKU ='{1}')", _
            cons, sku)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "No SKU was found", "No SKU was found")
        ElseIf dt.Rows.Count > 1 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "More than one SKU was found", "More than one SKU was found")
        Else
            Dim oSku As New WMS.Logic.SKU(dt.Rows(0)("consignee"), dt.Rows(0)("sku"))
            Return oSku
        End If
        Return Nothing
    End Function

    Private Function CanCompleteCount() As Boolean
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            If dr("counted") = 0 Then Return False
        Next
        Return True
    End Function

    Private Sub UpdateLoadCount(ByVal pConsignee As String, ByVal pSku As String)
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            If dr("consignee") = pConsignee And dr("sku") = pSku Then
                dr("counted") = 1
                dr("toqty") = dr("fromqty")
            End If
        Next
        Session("TaskLocationCNTLoadsDT") = dt
    End Sub

    Private Sub CalcQty(ByRef pFromqty As Decimal, ByRef pToQty As Decimal)
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            pFromqty = pFromqty + Convert.ToDecimal(dr("fromqty"))
            pToQty = pToQty + Convert.ToDecimal(dr("toqty"))
        Next
        Session("TaskLocationCNTLoadsDT") = dt
    End Sub

    'Private Function CreateLimboLoadCollection() As WMS.Logic.LoadsCollection
    '    Dim ldColl As New WMS.Logic.LoadsCollection
    '    Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
    '    For Each dr As DataRow In dt.Rows
    '        If dr("counted") = 0 Then
    '            ldColl.Add(New WMS.Logic.Load(dr("loadid")))
    '        End If
    '    Next
    '    Return ldColl
    'End Function

End Class

