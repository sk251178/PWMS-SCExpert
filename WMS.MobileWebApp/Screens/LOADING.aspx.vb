Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class LOADING
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
            Session.Remove("LoadingJob")
        End If
    End Sub

    Private Sub doNext()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Dim oLoading As New WMS.Logic.Loading
            Dim oLoadingJob As LoadingJob
            'Start RWMS-1018/RWMS-1292
            Dim IsContainerExists As Boolean = WMS.Logic.Container.Exists(DO1.Value("PALLETID"))
            Dim IsLoadExists As Boolean = WMS.Logic.Load.LoadExists(DO1.Value("PALLETID"))
            Dim IsCaseExists As Boolean = WMS.Logic.CaseDetail.Exists(DO1.Value("PALLETID"))
            If (IsContainerExists = False) AndAlso (IsLoadExists = False) AndAlso (IsCaseExists = False) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pallet/Load/Case was not found"))
                DO1.Value("PALLETID") = ""
                Return
            End If
            Dim err1 As String
            'RWMS-2449 task assinged to current user verification
            Dim sql, taskAssignedUserID As String
            Dim dt As New DataTable
            sql = String.Format("Select top 1 userid from TASKS where  assigned = 1 and status = '{0}' and (tasktype = '{1}' or tasktype = '{2}' or tasktype = '{3}')", WMS.Lib.Statuses.Task.ASSIGNED, WMS.Lib.TASKTYPE.CONTLOADING, WMS.Lib.TASKTYPE.LOADLOADING, WMS.Lib.TASKTYPE.CASELOADING)
            Dim oCase As WMS.Logic.CaseDetail
            If IsCaseExists Then
                sql = sql + String.Format(" And caseid = '{0}'", DO1.Value("PALLETID").ToString())
                oCase = New WMS.Logic.CaseDetail(DO1.Value("PALLETID"))
            ElseIf IsContainerExists Then
                sql = sql + String.Format(" And fromcontainer = '{0}'", DO1.Value("PALLETID").ToString())
            ElseIf IsLoadExists Then
                sql = sql + String.Format(" And fromload = '{0}'", DO1.Value("PALLETID").ToString())
            End If
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                taskAssignedUserID = dt.Rows(0)("userid").ToString()
                If WMS.Logic.Common.GetCurrentUser.ToUpper() <> taskAssignedUserID.ToUpper() Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Task is already assigned to other user: " & taskAssignedUserID))
                    Return
                End If
            End If
            If IsContainerExists And IsLoadExists Then
                If LoadBelong2Shipment(DO1.Value("PALLETID")) Then
                    Dim l As New WMS.Logic.Load(DO1.Value("PALLETID"))
                    If l.ACTIVITYSTATUS = "LOADED" Then
                        ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload already loaded"))
                        'Return
                        Dim oCont As New WMS.Logic.Container(DO1.Value("PALLETID"), False)
                        If oCont.HasCaseDetails Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot load the container! Please load individual Cases for this container"))
                            DO1.Value("PALLETID") = ""
                            Return
                        End If
                        If ContainerBelong2Shipment(DO1.Value("PALLETID"), err1) Then
                            'Added for RWMS-2343 RWMS-2314 - validate for container belongs to single shipment
                            If ContainerBelong2SingleShipment(DO1.Value("PALLETID"), err1) Then
                                oLoadingJob = oLoading.PickupContainer(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                            Else
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err1)
                                DO1.Value("PALLETID") = ""
                                Return
                            End If
                            'End Added for RWMS-2343 RWMS-2314

                            'Commented for RWMS-2343 RWMS-2314
                            'oLoadingJob = oLoading.PickupContainer(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                            'End Commented for RWMS-2343 RWMS-2314
                        Else
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err1)
                            DO1.Value("PALLETID") = ""
                            Return
                        End If
                    Else
                        If l.HasCaseDetails Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot load the payload! Please load individual Cases for this payload"))
                            DO1.Value("PALLETID") = ""
                            Return
                        End If
                        'Added for RWMS-670 - check whether the load belongs to container. If not proceed, else display message to enter the container 
                        Dim loadCont As String
                        loadCont = LoadBelong2Container(DO1.Value("PALLETID"))
                        If loadCont = "" Then
                            oLoadingJob = oLoading.PickupLoad(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                        Else
                            Dim msg As String
                            msg = String.Format("Load belongs to container {0}. Please enter container id", loadCont)
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(msg))
                            DO1.Value("PALLETID") = ""
                            Return
                        End If
                        'End Added for RWMS-670
                        'Commented for RWMS-670
                        'oLoadingJob = oLoading.PickupLoad(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                        'End Commented for RWMS-670
                    End If
                ElseIf ContainerBelong2Shipment(DO1.Value("PALLETID"), err1) Then
                    Dim oCont As New WMS.Logic.Container(DO1.Value("PALLETID"), False)
                    If oCont.HasCaseDetails Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot load the container! Please load individual Cases for this container"))
                        DO1.Value("PALLETID") = ""
                        Return
                    End If
                    'Added for RWMS-2343 RWMS-2314 - validate for container belongs to single shipment
                    If ContainerBelong2SingleShipment(DO1.Value("PALLETID"), err1) Then
                        oLoadingJob = oLoading.PickupContainer(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                    Else
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err1)
                        DO1.Value("PALLETID") = ""
                        Return
                    End If
                    'End Added for RWMS-2343 RWMS-2314

                    'Commented for RWMS-2343 RWMS-2314
                    'oLoadingJob = oLoading.PickupContainer(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                    'End Commented for RWMS-2343 RWMS-2314
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload does not belong to shipment"))
                    DO1.Value("PALLETID") = ""
                    Return
                End If

            ElseIf IsLoadExists And Not IsContainerExists Then
                'Start RWMS-1018/RWMS-1292
                If LoadBelong2Shipment(DO1.Value("PALLETID")) Then
                    Dim l As New WMS.Logic.Load(DO1.Value("PALLETID"))
                    If l.HasCaseDetails Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot load the payload! Please load individual Cases for this payload"))
                        DO1.Value("PALLETID") = ""
                        Return
                    End If
                    If l.ACTIVITYSTATUS = "LOADED" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload already loaded"))
                        Return
                    Else
                        'Added for PWMS-479 Retrofit of RWMS-670 - check whether the load belongs to container. If not proceed, else display message to enter the container    
                        Dim loadCont As String
                        loadCont = LoadBelong2Container(DO1.Value("PALLETID"))
                        If loadCont = "" Then
                            oLoadingJob = oLoading.PickupLoad(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                        Else
                            Dim msg As String
                            msg = String.Format("Load belongs to container {0}. Please enter container id", loadCont)
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(msg))
                            DO1.Value("PALLETID") = ""
                            Return
                        End If
                        'End Added for PWMS-479 Retrofit of RWMS-670 
                        'Commented for PWMS-479 Retrofit of RWMS-670 
                        'oLoadingJob = oLoading.PickupLoad(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)   
                        'End Commented for PWMS-479 Retrofit of RWMS-670 
                    End If
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload does not belong to shipment"))
                    DO1.Value("PALLETID") = ""
                    Return
                End If
            ElseIf IsContainerExists Then 'Start RWMS-1018/RWMS-1292
                Dim err As String
                If ContainerBelong2Shipment(DO1.Value("PALLETID"), err) Then
                    Dim oCont As New WMS.Logic.Container(DO1.Value("PALLETID"), False)
                    If oCont.HasCaseDetails Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Cannot load the container! Please load individual Cases for this container"))
                        DO1.Value("PALLETID") = ""
                        Return
                    End If
                    'RWMS-2446 validation of container loading
                    Dim ContainerSql As String = "SELECT STATUS FROM CONTAINER WHERE CONTAINER='" & DO1.Value("PALLETID") & "'"
                    Dim ContainerDt As DataTable = New DataTable
                    Made4Net.DataAccess.DataInterface.FillDataset(ContainerSql, ContainerDt)
                    If ContainerDt.Rows.Count > 0 Then

                        If (ContainerDt.Rows(0)("STATUS").ToString() = "LOADED") Then
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container already loaded"))
                            Return
                        End If
                    End If
                    'Added for RWMS-2343 RWMS-2314 - validate for container belongs to single shipment
                    If ContainerBelong2SingleShipment(DO1.Value("PALLETID"), err) Then
                        oLoadingJob = oLoading.PickupContainer(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                    Else
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err)
                        DO1.Value("PALLETID") = ""
                        Return
                    End If
                    'End Added for RWMS-2343 RWMS-2314

                    'Commented for RWMS-2343 RWMS-2314
                    'oLoadingJob = oLoading.PickupContainer(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                    'End Commented for RWMS-2343 RWMS-2314
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err)
                    DO1.Value("PALLETID") = ""
                    Return
                End If
            ElseIf IsCaseExists Then
                If oCase.Status = "LOADED" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Case already loaded"))
                    DO1.Value("PALLETID") = ""
                    Return
                ElseIf oCase.Status <> "STAGED" Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Incorrect Case status"))
                    DO1.Value("PALLETID") = ""
                    Return
                End If
                If LoadBelong2Shipment(oCase.ToLoad) Then
                    oLoadingJob = oLoading.PickupCase(DO1.Value("PALLETID"), True, WMS.Logic.Common.GetCurrentUser)
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Case Payload does not belong to shipment"))
                    DO1.Value("PALLETID") = ""
                    Return
                End If
            End If
            'RWMS-1018/RWMS-1292

            'If WMS.Logic.Load.Exists(DO1.Value("PALLETID")) Then

            'ElseIf WMS.Logic.Container.Exists(DO1.Value("PALLETID")) Then

            'End If

            If oLoadingJob Is Nothing Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pallet/Load/Case was not found"))
                DO1.Value("PALLETID") = ""
                Return
            Else
                Session("LoadingJob") = oLoadingJob
                Response.Redirect(MapVirtualPath("Screens/LOADING1.aspx"))
            End If
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

    'Added for RWMS-2343 RWMS-2314
    Private Function ContainerBelong2SingleShipment(ByVal container As String, ByRef err As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        SQL = String.Format("SELECT * FROM vShipmentContainers WHERE CONTAINERID = '{0}' ", container)

        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count > 1 Then
            err = trans.Translate("Container belong to more than one shipment")
            ret = False
        ElseIf dt.Rows.Count = 0 Then
            err = trans.Translate("Container does not belong to shipment")
            ret = False
        End If
        Return ret
    End Function
    'End Added for RWMS-2343 RWMS-2314

    'Added for PWMS-479 Retrofit of RWMS-670 - method to check whether the load belongs to container   
    Private Function LoadBelong2Container(ByVal load As String) As String
        Dim ret As String = ""
        Dim SQL As String
        SQL = String.Format("select HANDLINGUNIT from LOADS where LOADID = '{0}'", load)

        If Not IsDBNull(Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)) Then
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        End If

        Return ret
    End Function
    'End Added for PWMS-479 Retrofit of RWMS-670 


    'Session("LoadingJobsShipmentId") = DO1.Value("Shipment")
    '            Session("LoadingJobsDoor") = DO1.Value("Door")
    'Private Function ContainerExists(ByVal Container As String) As Boolean
    '    Dim ret As Boolean = True
    '    Dim SQL As String

    '    SQL = String.Format("select COUNT(*) from INVLOAD where HANDLINGUNIT = '{0}'", Container)
    '    SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
    '    If SQL = "0" Then
    '        ret = False
    '    End If
    '    Return ret
    'End Function

    Private Function LoadBelong2Shipment(ByVal load As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        If IsNothing(Session("LoadingJobsShipmentId")) OrElse Session("LoadingJobsShipmentId").ToString = "" Then
            SQL = String.Format("SELECT count(1) FROM dbo.ORDERLOADS AS ol INNER JOIN dbo.SHIPMENTDETAIL AS sp ON ol.CONSIGNEE = sp.CONSIGNEE AND ol.ORDERID = sp.ORDERID AND ol.ORDERLINE = sp.ORDERLINE where  ol.LOADID='{0}' ", load)
        Else
            SQL = String.Format("SELECT count(1) FROM dbo.ORDERLOADS AS ol INNER JOIN dbo.SHIPMENTDETAIL AS sp ON ol.CONSIGNEE = sp.CONSIGNEE AND ol.ORDERID = sp.ORDERID AND ol.ORDERLINE = sp.ORDERLINE where  ol.LOADID='{0}' and sp.SHIPMENT='{1}'", load, Session("LoadingJobsShipmentId"))
        End If

        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function

    Private Function ContainerBelong2Shipment(ByVal container As String, ByRef err As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If IsNothing(Session("LoadingJobsShipmentId")) OrElse Session("LoadingJobsShipmentId").ToString = "" Then
            SQL = String.Format("SELECT STATUS FROM vLoadContainerShipment WHERE CONTAINERID = '{0}' ", container)
        Else
            SQL = String.Format("SELECT STATUS FROM vLoadContainerShipment WHERE CONTAINERID = '{0}' and SHIPMENT='{1}' ", container, Session("LoadingJobsShipmentId"))
        End If

        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            err = trans.Translate("Container does not belong to shipment")
            ret = False
        Else
            Try
                If dt.Rows(0)(0) = WMS.Lib.Statuses.Container.LOADED Then
                    err = trans.Translate("Container already loaded")
                    ret = False
                End If
            Catch ex As Exception
                err = trans.Translate("Container already loaded")
                ret = False
            End Try
        End If
        Return ret
    End Function

    Private Sub doMenu()
        If Not Session("LoadingJob") Is Nothing Then
            Dim oLoadindTask As New LoadingTask
            oLoadindTask.ReleaseTask(Session("LoadingJob"), WMS.Logic.Common.GetCurrentUser)
        End If
        Session.Remove("LoadingJob")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("PALLETID", True, "next")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "install bulkhead"
                InstallBulkhead()
        End Select
    End Sub

    Private Sub InstallBulkhead()

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim oTask As New WMS.Logic.Task
        oTask.DOCUMENT = Session("LoadingJobsShipmentId")
        oTask.FROMLOCATION = Session("LoadingJobsDoor")
        oTask.TASKSUBTYPE = WMS.Lib.TASKSUBTYPE.BULKHEADINSTAL
        oTask.TASKTYPE = WMS.Lib.TASKTYPE.MISC
        oTask.TOLOCATION = Session("LoadingJobsDoor")
        oTask.Post(WMS.Logic.Common.GetCurrentUser)
        oTask.Complete(WMS.Logic.LogHandler.GetRDTLogger)

        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Bulkhead installed"))
    End Sub

End Class

