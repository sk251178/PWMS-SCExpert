Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class LOCCONTTASK1
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
        'If Not IsPostBack Then
        DO1.Value("LOCATION") = Session("TaskLocationCNTLocationId")
        DO1.Value("WAREHOUSEAREA") = Session("TaskWarehouseareaCNTWarehouseareaId")
        'End If
        If Session("SELECTEDSKU") <> "" Then
            DO1.Value("SKU") = Session("SELECTEDSKU")
            ' Add all controls to session for restoring them when we back from that sreen
            DO1.Value("LOADID") = Session("SKUSEL_LOADID")
            DO1.Value("CONSIGNEE") = Session("SKUSEL_CONSIGNEE")

            Session.Remove("SKUSEL_LOADID")
            Session.Remove("SKUSEL_CONSIGNEE")
            Session.Remove("SELECTEDSKU")
        End If
    End Sub

    Private Sub doBack()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.LOCATIONCOUNTING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSrcScreen") & ".aspx"))
    End Sub

    Private Sub doEndCount()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If CanCompleteCount() Then
                Dim toqty, fromQty As Decimal
                CalcQty(fromQty, toqty)
                Dim oCntTask As WMS.Logic.CountTask
                If Not Session("LocationBulkCountTask") Is Nothing Then
                    oCntTask = Session("LocationBulkCountTask")
                Else
                    oCntTask = Session("LocationCountTask")
                End If
                'If oCntTask Is Nothing Then  'The context is user initiated --> should not reach this condition -->
                '    Dim oCounting As New WMS.Logic.Counting()
                '    oCounting.COUNTTYPE = WMS.Lib.TASKTYPE.LOCATIONCOUNTING
                '    'oCounting.Count(fromQty, toqty, "", Session("TaskLocationCNTLocationId"), Session("TaskLocationCNTLoadsDT"), WMS.Logic.GetCurrentUser)
                'Else    'The working context is a task
                Dim oCounting As New WMS.Logic.Counting(oCntTask.COUNTID)
                oCounting.LOCATION = Session("TaskLocationCNTLocationId")
                oCounting.WAREHOUSEAREA = Session("TaskWarehouseareaCNTWarehouseareaId")

                'Build and fill count job object
                Dim oCountJob As WMS.Logic.CountingJob = oCntTask.getCountJob(oCounting)
                oCountJob.ExpectedQty = fromQty
                oCountJob.CountedQty = toqty
                oCountJob.CountedLoads = Session("TaskLocationCNTLoadsDT")
                'check if the location is in problem, and redirect to the clear problem screen
                If ShouldGoToClearLocation() Then
                    Session("LocationCntJob") = oCountJob
                    'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location counted has problem. Please confirm problem clearing."))
                    Response.Redirect(MapVirtualPath("Screens/LocationCountClearProblem.aspx"))
                Else
                    oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser)
                End If
                'End If
                Session.Remove("LocationCNTLoadId")
                Session.Remove("TaskLocationCNTLocationId")
                Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
                Session.Remove("TSKTaskId")
                Session.Remove("LocationBulkCountTask")
                Session.Remove("LocationCountTask")
                Session("TaskID") = oCntTask.TASK
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location Count Completed"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Not All Loads Counted for the current Location. Please Confirm"))
                Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK3.aspx"))
            End If
        Catch ex As Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
        End Try
        '--- added by oded 130313---------------------------------------------------------------
        If Not Session("TaskLocationCNTLoadsDT") Is Nothing Then
            Session.Remove("TaskLocationCNTLoadsDT")
        End If
        '---------------------------------------------------------------------------------------

        'If String.IsNullOrEmpty(Session("UserInitCnt")) Then
        '    Session.Remove("UserInitCnt")
        '    Response.Redirect(MapVirtualPath("Screens/loccnt.aspx"))
        'Else
        '    Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSrcScreen") & ".aspx"))
        'End If

        Dim destScreen As String
        If Not String.IsNullOrEmpty(Session("UserInitCnt")) Then
            Session.Remove("UserInitCnt")
            destScreen = "loccnt"
        Else
            destScreen = Session("CountingSrcScreen")
        End If

        If MobileUtils.ShouldRedirectToTaskSummary() Then
            MobileUtils.RedirectToTaskSummary(destScreen)
        Else
            Session.Remove("TaskID")
            Dim oTask As Task = MobileUtils.RequestTask(LogHandler.GetRDTLogger())
            Response.Redirect(MapVirtualPath("Screens/" & destScreen & ".aspx"))
        End If





    End Sub

    Private Function ShouldGoToClearLocation() As Boolean
        Dim oLoc As New Location(Session("TaskLocationCNTLocationId"), Session("TaskWarehouseareaCNTWarehouseareaId"))
        If oLoc.PROBLEMFLAG Then
            Dim sql As String = String.Format("select isnull(CLEARONLOCATIONCOUNT,0) from LOCATIONPROBLEMRC where problemrc ='{0}'", oLoc.PROBLEMFLAGRC)
            Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        Else
            Return False
        End If
    End Function


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONSIGNEE", Nothing, "", False, Session("3PL"))
        DO1.AddTextboxLine("SKU")
        DO1.AddSpacer()
    End Sub

    Private Sub doNext()
        ' If Page.IsValid Then
        ' First Check if Consignee and Sku is full , if yes get check from vSKUCODE
        If DO1.Value("SKU").Trim <> "" Then
            ' Check for sku
            If DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & Session("TaskLocationCNTLocationId") & "' and RD.WAREHOUSEAREA='" & Session("TaskWarehouseareaCNTWarehouseareaId") & "' WHERE (SKUCODE LIKE '" & DO1.Value("SKU") & "' OR vSC.SKU LIKE '" & DO1.Value("SKU") & "')") > 1 Then
                ' Go to Sku select screen
                Session("FROMSCREEN") = "LOCCONTTASK1"
                Session("SKUCODE") = DO1.Value("SKU").Trim
                ' Add all controls to session for restoring them when we back from that sreen
                Session("SKUSEL_LOADID") = DO1.Value("LOADID").Trim
                Session("SKUSEL_CONSIGNEE") = DO1.Value("CONSIGNEE", Session("consigneeSession")).Trim
                Response.Redirect(MapVirtualPath("Screens/MultiSelectForm.aspx")) ' changed
            ElseIf DataInterface.ExecuteScalar("SELECT COUNT(DISTINCT vSC.SKU) FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & Session("TaskLocationCNTLocationId") & "' AND RD.WAREHOUSEAREA='" & Session("TaskWarehouseareaCNTWarehouseareaId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'") = 1 Then
                DO1.Value("SKU") = DataInterface.ExecuteScalar("SELECT vSC.SKU FROM vSKUCODE vSC INNER JOIN INVLOAD RD ON vSC.CONSIGNEE=RD.CONSIGNEE AND vSC.SKU=RD.SKU AND RD.LOCATION='" & Session("TaskLocationCNTLocationId") & "' AND RD.WAREHOUSEAREA='" & Session("TaskWarehouseareaCNTWarehouseareaId") & "' WHERE SKUCODE LIKE '" & DO1.Value("SKU") & "'")
            End If
        End If

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim dt As New DataTable
        Dim sql As String
        Dim LoadId, Loc, Warehousearea, Cons, Sku As String
        LoadId = DO1.Value("LOADID")
        Loc = Session("TaskLocationCNTLocationId")
        Warehousearea = Session("TaskWarehouseareaCNTWarehouseareaId")
        Cons = DO1.Value("CONSIGNEE", Session("consigneeSession"))
        Sku = DO1.Value("SKU")

        Select Case LoadFindType(LoadId, Loc, Warehousearea, Cons, Sku)
            Case LoadSearchType.ByLoad
                sql = String.Format("SELECT LOADID FROM invload WHERE LOADID LIKE '{0}' and location = '{1}' and warehousearea = '{2}'", LoadId.Trim(), Loc.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocationAndSku
                sql = String.Format("SELECT LOADID FROM invload INNER JOIN SKU ON invload.CONSIGNEE = SKU.CONSIGNEE AND invload.SKU = SKU.SKU WHERE invload.LOCATION = '{0}' and invload.WAREHOUSEAREA LIKE '{2}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", Loc.Trim(), Sku.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocationAndConsigneeAndSku
                sql = String.Format("SELECT LOADID FROM invload INNER JOIN SKU ON invload.CONSIGNEE = SKU.CONSIGNEE AND invload.SKU = SKU.SKU WHERE invload.LOCATION = '{0}' and invload.WAREHOUSEAREA LIKE '{3}%' AND SKU.CONSIGNEE LIKE '{1}%' AND (SKU.SKU like '{2}%' or SKU.MANUFACTURERSKU like '{2}%' or SKU.VENDORSKU ='{2}' or SKU.OTHERSKU ='{2}')", Loc.Trim(), Cons.Trim(), Sku.Trim(), Warehousearea.Trim())
            Case LoadSearchType.ByLocation
                sql = String.Format("SELECT LOADID FROM invload WHERE LOCATION = '{0}' and WAREHOUSEAREA LIKE '{1}%' ", Loc.Trim(), Warehousearea.Trim())
            Case LoadSearchType.BySku
                sql = String.Format("SELECT LOADID FROM invload INNER JOIN SKU ON invload.CONSIGNEE = SKU.CONSIGNEE AND invload.SKU = SKU.SKU WHERE SKU.CONSIGNEE LIKE '{0}%' and location = '{2}' and warehousearea = '{3}' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", Cons.Trim(), Sku.Trim(), Loc.Trim, Warehousearea.Trim())
        End Select

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("More than 1 load in location, enter loadid"))
            Return
        ElseIf dt.Rows.Count = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("No Load Found"))
            Return
        End If
        Dim ld As String
        Try
            ld = dt.Rows(0)(0)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage())
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        Session("LocationCNTLoadId") = ld
        Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK2.aspx"))
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

    Private Function LoadFindType(ByVal LoadId As String, ByVal Loc As String, ByVal Warehousearea As String, ByVal cons As String, ByVal sk As String) As Int32
        If LoadId <> "" Then Return LoadSearchType.ByLoad
        If Loc <> "" And Warehousearea <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndSku
        If Loc <> "" And Warehousearea <> "" And cons <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndConsigneeAndSku
        If cons <> "" And sk <> "" Then Return LoadSearchType.BySku
        If sk <> "" Then Return LoadSearchType.BySku
        If Loc <> "" And Warehousearea <> "" Then Return LoadSearchType.ByLocation
    End Function

    Public Enum LoadSearchType
        ByLoad = 1
        ByLocationAndSku = 2
        ByLocationAndConsigneeAndSku = 3
        BySku = 4
        ByLocation = 6
    End Enum

    Private Function CanCompleteCount() As Boolean
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            If dr("counted") = 0 Then Return False
        Next
        Return True
    End Function

    Private Function CreateLimboLoadCollection() As WMS.Logic.LoadsCollection
        Dim ldColl As New WMS.Logic.LoadsCollection
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            If dr("counted") = 0 Then
                ldColl.Add(New WMS.Logic.Load(dr("loadid").ToString()))
            End If
        Next
        Return ldColl
    End Function

    Private Sub CalcQty(ByRef pFromqty As Decimal, ByRef pToQty As Decimal)
        Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
        For Each dr As DataRow In dt.Rows
            pFromqty = pFromqty + Convert.ToDecimal(dr("fromqty"))
            pToQty = pToQty + Convert.ToDecimal(dr("toqty"))
        Next
        Session("TaskLocationCNTLoadsDT") = dt
    End Sub

End Class