Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class CONTASK1
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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONSOLIDATION, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
            End If

            Dim cons As ConsolidationTask = Session("ConsolidationTask")
            Dim cnjob As ConsolidationJob
            Try
                cnjob = cons.getConsolidationJob()
            Catch ex As Exception

            End Try

            setConsolidation(cnjob)
        End If
    End Sub

    Private Sub setConsolidation(ByVal cnjob As ConsolidationJob)
        If cnjob Is Nothing Then

            Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
        Else
            Session("ConsolidationJob") = cnjob
            DO1.Value("CONTAINERID") = cnjob.FromContainer
            DO1.Value("LOADID") = cnjob.FromLoad
            DO1.Value("CONSIGNEE") = cnjob.consignee
            DO1.Value("SKU") = cnjob.Sku
            DO1.Value("SKUDESC") = cnjob.skuDesc
            DO1.Value("FROMLOCATION") = cnjob.FromLocation
            DO1.Value("FROMWAREHOUSEAREA") = cnjob.FromWarehousearea
            DO1.Value("UNITS") = cnjob.Units
            DO1.Value("UOM") = cnjob.UOM
            DO1.Value("UOMUNITS") = cnjob.UOMUnits
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("FROMLOCATION")
        DO1.AddLabelLine("FROMWAREHOUSEAREA")
        DO1.AddLabelLine("UNITS")
        DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("UOMUNITS")
        DO1.AddSpacer()
        DO1.AddTextboxLine("CONFIRMLOAD")
        DO1.AddTextboxLine("CONFIRMCONTAINER")
    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.CONSOLIDATION, LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.CONSOLIDATION, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim conj As ConsolidationJob = Session("ConsolidationJob")
        Dim contask As ConsolidationTask = Session("ConsolidationTask")

        If conj Is Nothing Then
            Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForTask(contask.TASK)
            If DO1.Value("CONFIRMLOAD").Trim = String.Empty Then
                conj = CType(tm.Task, WMS.Logic.ConsolidationTask).ConsolidateContainer(conj, DO1.Value("CONFIRMCONTAINER"), WMS.Logic.Common.GetCurrentUser)
            Else
                conj = CType(tm.Task, WMS.Logic.ConsolidationTask).ConsolidateLoad(conj, DO1.Value("CONFIRMLOAD"), WMS.Logic.Common.GetCurrentUser)
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try

        DO1.Value("CONFIRMLOAD") = ""
        DO1.Value("CONFIRMCONTAINER") = ""
        setConsolidation(conj)
    End Sub

    Private Sub doCloseContainer()
        Dim contask As ConsolidationTask = Session("ConsolidationTask")

        If contask Is Nothing Then
            Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
        End If

        contask.ConsolidationObject.Close(WMS.Logic.Common.GetCurrentUser())
        Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "close"
                doCloseContainer()
        End Select
    End Sub
End Class