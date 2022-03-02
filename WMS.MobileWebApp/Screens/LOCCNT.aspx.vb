Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

<CLSCompliant(False)> Partial Class LOCCNT
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

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
        If WMS.Logic.GetCurrentUser Is Nothing Or WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session.Remove("CountingSrcScreen")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("TaskLocationCNTLocationId")
        Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If DO1.Value("LOCATION") = String.Empty Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location cannot be empty"))
            Return
        End If

        Session("TaskLocationCNTLocationId") = DO1.Value("LOCATION")
        Session("TaskWarehouseareaCNTWarehouseareaId") = Warehouse.getUserWarehouseArea()

        Session("CountingSrcScreen") = "LOCCNT"
        Session("TaskLocationCNTLoadsDT") = CreateLoadsDatatable()
        Dim oCount As New WMS.Logic.Counting

        If oCount.CreateLocationCountJobs(Session("TaskWarehouseareaCNTWarehouseareaId"), "", DO1.Value("LOCATION"), WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", "", WMS.Logic.Common.GetCurrentUser) > 0 Then
            If oCount.COUNTID = "" Then
                oCount = New WMS.Logic.Counting(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, DO1.Value("LOCATION"))
                Session("COUNTID") = oCount.COUNTID
            Else
                Session("COUNTID") = oCount.COUNTID
            End If

            Session("UserInitCnt") = "1"
            Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK.aspx"))
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Can not create tasks for current location"))
            Return
        End If
    End Sub



    ' Returns TaskID that this load is assigned to
    Private Function isAssignedToTask(ByVal COUNTID As String) As String
        Dim strSql As String = "SELECT top 1 TASK FROM TASKS WHERE COUNTID = '" & COUNTID & "' AND TASKTYPE = 'LOCCNT' order by TASK desc "
        Dim dt As DataTable = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOCATION", True, "next")
        'DO1.AddTextbox Line("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Function CreateLoadsDatatable() As DataTable
        Dim dt As New DataTable
        Dim SQL As String = String.Format("select loadid, consignee, sku, units as fromqty, 0 as toqty, 0 as counted from invload where location = '{0}' and warehousearea = '{1}' ", DO1.Value("LOCATION").Trim, Session("TaskWarehouseareaCNTWarehouseareaId"))
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

End Class

