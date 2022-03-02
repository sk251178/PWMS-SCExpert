Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib



<CLSCompliant(False)> Public Class BATCHREPLPRINT
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            MyBase.WriteToRDTLog(" Flow Navigated to page BATCH LABEL PRINT... ")
        End If
    End Sub
    Private Sub doNext()
        Dim oWHActivity As New WMS.Logic.WHActivity

        MyBase.WriteToRDTLog(" Performing Next operation in BATCH LABEL PRINT...")

        Dim prntr As LabelPrinter
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        MyBase.WriteToRDTLog("Batch Label Printer Value: " & DO1.Value("BatchLabelPrinter"))

        Try
            prntr = New LabelPrinter(DO1.Value("BatchReplenPrinter"))
            MobileUtils.getDefaultPrinter(prntr.PrinterQName, LogHandler.GetRDTLogger())
            MobileUtils.UpdateDPrinterInUserProfile(prntr.PrinterQName, LogHandler.GetRDTLogger())
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.ToString()))
            Return
        End Try

        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENLETDOWNHEADER)
        brh.PrintBatchLabels(prntr.PrinterQName)
        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENLETDOWN1))

    End Sub
    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        'If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
        '    Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
        '    tm.ExitTask()
        'End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("BatchReplenPrinter", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                doMenu()
            Case "next"
                doNext()
        End Select
    End Sub

End Class