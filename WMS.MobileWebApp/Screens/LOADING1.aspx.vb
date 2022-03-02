Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class LOADING1
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        If Not IsPostBack Then
            Dim oLoadingJob As WMS.Logic.LoadingJob = Session("LoadingJob")
            If oLoadingJob Is Nothing Then
                Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Loading Job Corrupted"))
                doBack()
            Else
                setScreen(oLoadingJob)
            End If
        End If
    End Sub

    Private Sub doNext()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Dim oLoading As New WMS.Logic.Loading
            Dim oLoadingJob As WMS.Logic.LoadingJob = Session("LoadingJob")
            If Not oLoadingJob Is Nothing Then
                If oLoadingJob.ToWarehousearea = "" Then oLoadingJob.ToWarehousearea = Warehouse.getUserWarehouseArea()
                'RWMS-2449 task assinged to current user verification
                Dim sql, taskAssignedUserID As String
                Dim dt As New DataTable
                sql = String.Format("Select top 1 userid from TASKS where  assigned = 1 and status = '{0}' and (tasktype = '{1}' or tasktype = '{2}' or tasktype = '{3}')", WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.CONTLOADING, WMS.Lib.TASKTYPE.LOADLOADING, WMS.Lib.TASKTYPE.CASELOADING)
                If oLoadingJob.IsCase Then
                    sql = sql + String.Format(" And caseid = '{0}'", oLoadingJob.LoadId)
                ElseIf oLoadingJob.IsContainer Then
                    sql = sql + String.Format(" And fromcontainer = '{0}'", oLoadingJob.LoadId)
                Else
                    sql = sql + String.Format(" And fromload = '{0}'", oLoadingJob.LoadId)
                End If
                DataInterface.FillDataset(sql, dt)
                If dt.Rows.Count > 0 Then
                    taskAssignedUserID = dt.Rows(0)("userid").ToString()
                    If WMS.Logic.Common.GetCurrentUser.ToUpper() <> taskAssignedUserID.ToUpper() Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Task is already assigned to other user: " & taskAssignedUserID))
                        Return
                    End If
                End If
                'RWMS-2446 multi-user loading validation, get the load activitiy status from database instead of from session value
                If oLoadingJob.IsCase Then
                    Dim Casesql As String = "SELECT STATUS FROM CASEDETAIL WHERE CASEID='" & oLoadingJob.LoadId & "'"
                    Dim CaseDt As New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(Casesql, CaseDt)
                    If CaseDt.Rows.Count > 0 Then
                        If (CaseDt.Rows(0)("STATUS").ToString() = "LOADED") Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Case already loaded"))
                            Return
                        End If
                    End If
                ElseIf oLoadingJob.IsContainer Then
                    Dim ContainerSql As String = "SELECT STATUS FROM CONTAINER WHERE CONTAINER='" & oLoadingJob.LoadId & "'"
                    Dim ContainerDt As DataTable = New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(ContainerSql, ContainerDt)
                    If ContainerDt.Rows.Count > 0 Then

                        If (ContainerDt.Rows(0)("STATUS").ToString() = "LOADED") Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container already loaded"))
                            Return
                        End If
                    End If
                Else

                    Dim l As New WMS.Logic.Load(oLoadingJob.LoadId)
                    If l.ACTIVITYSTATUS = "LOADED" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload already loaded"))
                        Return
                    End If
                End If

                If CType(WarehouseParams.GetWarehouseParam("ShowPositionLoading"), Boolean) Then
                    oLoading.LoadPallet(oLoadingJob, DO1.Value("POSITION"), DO1.Value("CONFIRM"), oLoadingJob.ToWarehousearea, WMS.Logic.Common.GetCurrentUser)
                Else
                    oLoading.LoadPallet(oLoadingJob, Nothing, DO1.Value("CONFIRM"), oLoadingJob.ToWarehousearea, WMS.Logic.Common.GetCurrentUser)
                End If
            End If
            doBack()
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.Message)
            Return
        End Try
    End Sub

    Private Sub doMenu()
        If Not Session("LoadingJob") Is Nothing Then
            Dim oLoadindTask As New LoadingTask
            oLoadindTask.ReleaseTask(Session("LoadingJob"), WMS.Logic.Common.GetCurrentUser)
        End If
        Session.Remove("LoadingJob")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        If Not Session("LoadingJob") Is Nothing Then
            Dim oLoadindTask As New LoadingTask
            oLoadindTask.ReleaseTask(Session("LoadingJob"), WMS.Logic.Common.GetCurrentUser)
        End If
        Session.Remove("LoadingJob")
        Response.Redirect(MapVirtualPath("Screens/LOADING.aspx"))
    End Sub

    Private Sub setScreen(ByVal pldJob As LoadingJob)
        DO1.Value("Shipment") = pldJob.Shipment
        'DO1.Value("OrderId") = pldJob.OrderId
        DO1.Value("CompanyName") = pldJob.ComapnyName
        DO1.Value("PalletId") = pldJob.LoadId
        'DO1.Value("RequestedDate") = pldJob.RequestedDate
        DO1.Value("CarrierName") = pldJob.CarrierName
        DO1.Value("Trailer") = pldJob.Trailer
        DO1.Value("Vehicle") = pldJob.Vehicle
        DO1.Value("Door") = pldJob.Door
        If pldJob.IsContainer Then
            DO1.setVisibility("Sku", False)
            DO1.setVisibility("SkuDescription", False)
        Else
            DO1.setVisibility("Sku", True)
            DO1.setVisibility("SkuDescription", True)
            DO1.Value("Sku") = pldJob.Sku
            DO1.Value("SkuDescription") = pldJob.SkuDesc
        End If

        'Start RWMS-1256: On RDT Loading have the Door Number prepopulated       
        If WMS.Logic.WarehouseParams.GetWarehouseParam("RDTAutoPopulateDoor") = "1" Then
            DO1.Value("CONFIRM") = pldJob.Door
            DO1.Value("POSITION") = ""
            DO1.FocusField = "POSITION"
        Else
            DO1.Value("CONFIRM") = ""
            DO1.Value("POSITION") = ""
        End If
        'End RWMS-1256: On RDT Loading have the Door Number prepopulated

        If Not CType(WarehouseParams.GetWarehouseParam("ShowPositionLoading"), Boolean) Then
            DO1.setVisibility("Position", False)
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Shipment")
        'DO1.AddLabelLine("OrderId")
        DO1.AddLabelLine("CompanyName")
        DO1.AddLabelLine("PalletId")
        DO1.AddLabelLine("Sku")
        DO1.AddLabelLine("SkuDescription")
        'DO1.AddLabelLine("RequestedDate")
        DO1.AddLabelLine("CarrierName")
        DO1.AddLabelLine("Trailer")
        DO1.AddLabelLine("Vehicle")
        DO1.AddLabelLine("Door")
        DO1.AddSpacer()
        DO1.AddTextboxLine("CONFIRM")
        DO1.AddTextboxLine("POSITION", "VEHICLELOCATION")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class


