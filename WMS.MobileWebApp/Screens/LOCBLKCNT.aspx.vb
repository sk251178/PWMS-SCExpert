Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

<CLSCompliant(False)> Public Class LOCBLKCNT
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
        If WMS.Logic.GetCurrentUser Is Nothing Or WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session.Remove("CountingSrcScreen")
            Session.Remove("TaskLocationCNTLoadsDT")
        End If
        DO1.Value("WAREHOUSEAREA") = Warehouse.getUserWarehouseArea()
    End Sub

    Private Sub doMenu()
        Dim cnt As New WMS.Logic.Counting
        Try
            Session.Remove("CountingSrcScreen")
            Session.Remove("TaskLocationCNTLocationId")
            Session.Remove("TaskWarehouseareaCNTWarehouseareaId")
            Session.Remove("TaskLocationCNTLoadsDT")
            Made4Net.Mobile.Common.GoToMenu()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Session("TaskLocationCNTLocationId") = DO1.Value("LOCATION").Trim
        Session("TaskWarehouseareaCNTWarehouseareaId") = DO1.Value("WAREHOUSEAREA")

        Session("CountingSrcScreen") = "LOCBLKCNT"
        Session("TaskLocationCNTLoadsDT") = CreateLoadsDatatable()
        ' Create Location counting tasks
        Dim oCount As New WMS.Logic.Counting
        'Commented for Retrofit Item PWMS-869(RWMS-928) Start
        'If oCount.CreateLocationCountJobs("", DO1.Value("LOCATION"), Session("TaskWarehouseareaCNTWarehouseareaId"), WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING, "", "", WMS.Logic.Common.GetCurrentUser) > 0 Then
        'Commented for Retrofit Item PWMS-869(RWMS-928) End

        'Added for Retrofit Item PWMS-869(RWMS-928) Start
        If oCount.CreateLocationCountJobs(Session("TaskWarehouseareaCNTWarehouseareaId"), "", DO1.Value("LOCATION"), WMS.Lib.TASKTYPE.LOCATIONBULKCOUNTING, "", "", WMS.Logic.Common.GetCurrentUser) > 0 Then
            'Added for Retrofit Item PWMS-869(RWMS-928) End
            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                Dim sql As String = String.Format("Select task from tasks where countid={0}", Made4Net.Shared.FormatField(oCount.COUNTID))
                Dim tsk As New WMS.Logic.Task(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
                Session("TMTask") = tsk
                Session("TargetScreen") = "Screens/LOCBLKCNTTASK.aspx"
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
            Else
                Response.Redirect(MapVirtualPath("Screens/LOCBLKCNTTASK.aspx"))
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Can not create tasks for current location"))
            Return
        End If

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOCATION", True, "next")
        'DO1.AddTextbox Line("WAREHOUSEAREA") 
        DO1.AddLabelLine("WAREHOUSEAREA")
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
