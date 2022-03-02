Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKLBLREPRNT
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


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

    End Sub



    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim picklistID, printQName As String
        picklistID = DO1.Value("PicklistID")
        printQName = DO1.Value("Printer")
        Try
            If (String.IsNullOrEmpty(picklistID) Or String.IsNullOrEmpty(printQName)) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Picklist or Printer is empty"))

                Return
            End If
            If Not WMS.Logic.Picklist.Exists(picklistID) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Unavailable Picklist Number"))
                DO1.Value("PicklistID") = ""
                Return
            End If
            Dim pck As New Picklist(picklistID)
            If Not pck.isCompleted Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Picklist is not COMPLETE"))
                DO1.Value("PicklistID") = ""
                Return
            End If
            Dim IsPickLabel, IsShipLabel As Boolean
            IsPickLabel = pck.ShouldPrintPickLabel
            IsShipLabel = pck.ShouldPrintShipLabel
            If IsPickLabel Or IsShipLabel Then
                If Not IsValidLablePrinter(printQName) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Incorrect Printer ID"))
                    DO1.Value("Printer") = ""
                    Return
                End If
                If IsPickLabel Then
                    pck.PrintPickLabels(DO1.Value("Printer"))
                End If
                If IsShipLabel Then
                    pck.PrintShipLabels(DO1.Value("Printer"), True)
                End If
            End If
            If pck.ShouldPrintContentList Then
                Dim stContentReportName As String = pck.GetContentListReoprtName()
                If String.IsNullOrEmpty(stContentReportName) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("ContentList Report not configured"))
                End If
                If Not ReportPrinter.Exists(printQName) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Incorrect Printer ID"))
                    DO1.Value("Printer") = ""
                    Return
                End If
                pck.PrintContentListReport(printQName, stContentReportName, LogHandler.GetRDTLogger())
            End If
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Label(s) sent to Printer"))
            DO1.Value("PicklistID") = ""
            DO1.Value("Printer") = ""

        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
        End Try

    End Sub
    Private Function IsValidLablePrinter(ByVal printQName As String) As Boolean
        If Not LabelPrinter.Exists(printQName) Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("PicklistID")
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()

        End Select
    End Sub
End Class