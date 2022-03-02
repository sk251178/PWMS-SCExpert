Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKBAGOUTPRINT
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

        If Not IsPostBack Then
            Dim pck As Picklist = Session("PCKPicklist")
        End If
    End Sub

    Private Sub doSkipReport()
        Dim pck As Picklist = Session("PCKPicklist")
        Response.Redirect(MapVirtualPath("Screens/BagOutCloseContainer.aspx?sourcescreen=" & Session("MobileSourceScreen")))
    End Sub
    Private Sub doNext()
        Dim pck As Picklist = Session("PCKPicklist")

        If Not Session("PCKBagOutPicking") Is Nothing Then
            Dim prntr As ReportPrinter
            Try
                prntr = New ReportPrinter(DO1.Value("Printer"))
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception

            End Try
            pck.PrintBagOutReport(prntr.PrinterQName, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
        End If
        If (pck.ShouldPrintBagOutReportOnStart) Then
            If pck.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
            End If
        ElseIf (pck.ShouldPrintBagOutReportOnComplete) Then
            Response.Redirect(MapVirtualPath("Screens/BagOutCloseContainer.aspx?sourcescreen=" & Session("MobileSourceScreen")))
        End If

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "skip"
                doSkipReport()
        End Select
    End Sub
End Class