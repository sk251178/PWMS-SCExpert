Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

Imports WMS.Logic
<CLSCompliant(False)> Public Class LOCCONTTASK3
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
        DO1.Value("LOCATION") = Session("TaskLocationCNTLocationId")
        DO1.Value("WAREHOUSEAREA") = WMS.Logic.Warehouse.getUserWarehouseArea()
    End Sub

    Private Sub doBack()
        Session.Remove("LocationCNTLoadId")
        If Not Session("CountingSrcScreen") Is Nothing And Session("CountingSrcScreen") <> "" Then
            Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSrcScreen") & ".aspx"))
        End If
        Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK1.aspx"))
    End Sub

    Private Sub doEndCount()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim toqty, fromQty As Decimal
            CalcQty(fromQty, toqty)
            Dim oCntTask As WMS.Logic.CountTask
            If Not Session("LocationBulkCountTask") Is Nothing Then
                oCntTask = Session("LocationBulkCountTask")
            Else
                oCntTask = Session("LocationCountTask")
            End If
            Dim oCounting As New WMS.Logic.Counting(oCntTask.COUNTID)
            'Build and fill count job object
            Dim oCountJob As WMS.Logic.CountingJob = oCntTask.getCountJob(oCounting)
            oCountJob.ExpectedQty = fromQty
            oCountJob.CountedQty = toqty
            oCountJob.CountedLoads = Session("TaskLocationCNTLoadsDT")
            'check if the location is in problem, and redirect to the clear problem screen
            If ShouldGoToClearLocation() Then
                Session("LocationCntJob") = oCountJob
                Response.Redirect(MapVirtualPath("Screens/LocationCountClearProblem.aspx"))
            Else
                oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser)
            End If
            Session.Remove("LocationCNTLoadId")
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session("TaskID") = oCntTask.TASK
            Session.Remove("TSKTaskId")
            Session.Remove("LocationBulkCountTask")
            Session.Remove("LocationCountTask")
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location Count Completed"))
            '--- added by oded 130313---------------------------------------------------------------
            If Not Session("TaskLocationCNTLoadsDT") Is Nothing Then
                Session.Remove("TaskLocationCNTLoadsDT")
            End If
            '---------------------------------------------------------------------------------------

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
        End Try

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
            Dim oTask As Task = MobileUtils.RequestTask(LogHandler.GetRDTLogger())
            Response.Redirect(MapVirtualPath("Screens/" & destScreen & ".aspx"))
        End If

    End Sub

    Private Function ShouldGoToClearLocation() As Boolean
        Dim oLoc As New Location(Session("TaskLocationCNTLocationId"), Session("TaskWarehouseareaCNTWarehouseareaId"))
        If oLoc.PROBLEMFLAG Then
            Dim sql As String = String.Format("select isnull(CLEARONLOCATIONCOUNT,0) from LOCATIONPROBLEMRC where problemrc ='{0}'", oLoc.PROBLEMFLAGRC)
            Return Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
        Else
            Return False
        End If
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "back"
                doBack()
            Case "endcount"
                doEndCount()
        End Select
    End Sub

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
