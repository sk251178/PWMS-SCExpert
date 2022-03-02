Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKLBLPRNT
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
            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Session.Remove("PCKPicklist")
                If Not Session("PCKBagOutPicking") Is Nothing Then
                    Response.Redirect(MapVirtualPath("Screens/PCKBagOut.aspx"))
                Else
                    Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
                End If

            End If

            Dim pck As Picklist = Session("PCKPicklist")
            DO1.Value("Picklist") = pck.PicklistID
            DO1.Value("PickMethod") = pck.PickMethod
            DO1.Value("PickType") = pck.PickType
        End If
    End Sub

    Private Sub doSkipLabel()
        Dim pck As Picklist = Session("PCKPicklist")
        If pck.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
        Else
            Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
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
        Dim pck As Picklist = Session("PCKPicklist")

        If Not Session("PCKBagOutPicking") Is Nothing Then
            Dim rprntr As ReportPrinter
            Try
                rprntr = New ReportPrinter(DO1.Value("Printer"))
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception

            End Try
            pck.PrintBagOutReport(rprntr.PrinterQName, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
        End If
        Dim prntr As LabelPrinter
        Try
            prntr = New LabelPrinter(DO1.Value("Printer"))
            MobileUtils.getDefaultPrinter(prntr.PrinterQName, LogHandler.GetRDTLogger())
            MobileUtils.UpdateDPrinterInUserProfile(prntr.PrinterQName, LogHandler.GetRDTLogger())
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception

        End Try
        pck.PrintPickLabels(prntr.PrinterQName)



        If pck.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
            Response.Redirect(MapVirtualPath("Screens/PCKFULL.aspx"))
        Else
            Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("PickMethod")
        DO1.AddLabelLine("PickType")
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "skip"
                doSkipLabel()
        End Select
    End Sub
End Class