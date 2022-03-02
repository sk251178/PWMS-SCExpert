Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class DELLBLPRNT
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
        If Not IsPostBack Then
            'start for 1420 and RWMS-1412


            MyBase.WriteToRDTLog(" Flow Navigated to page DELLBPRINT ")

            'end for 1420 and RWMS-1412

            If Session("PCKPicklist") Is Nothing And Session("PARPCKPicklist") Is Nothing Then
                'start for 1420 and RWMS-1412

                MyBase.WriteToRDTLog(" Page load redirect to DEL ")

                'end for 1420 and RWMS-1412
                ' Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
            End If
            If Not Session("PCKPicklist") Is Nothing Then
                ''Uncommented for RWMS-323 17-10-14
                'Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                'If Not oPicklist.ShouldPrintShipLabel Then
                '    Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                'End If
                ''End Uncommented for RWMS-323 17-10-14

                'Added for NEWRWMS-452
                ' Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                'If Not oPicklist.ShouldPrintShipLabel Then
                '    Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                'End If
                'End Added for NEWRWMS-452

                'ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                '    Dim pck As ParallelPicking = Session("PARPCKPicklist")
                '    If Not pck.ShouldPrintShipLabel Then
                '        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                '    End If
                'End If
                If Not Request.QueryString("sourcescreen") Is Nothing Then
                    Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                End If
            End If
        End If
    End Sub

Private Sub doNext()

        'start for 1420 and RWMS-1412


            MyBase.WriteToRDTLog(" Performing Next operation in DELLBPRINT ")

        'end for 1420 and RWMS-1412
        Dim prntr As LabelPrinter
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            prntr = New LabelPrinter(DO1.Value("Printer"))
            MobileUtils.getDefaultPrinter(prntr.PrinterQName, LogHandler.GetRDTLogger())
            MobileUtils.UpdateDPrinterInUserProfile(prntr.PrinterQName, LogHandler.GetRDTLogger())
            Session("DefaultPrinter") = prntr.PrinterQName
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.ToString()))
            Return
        End Try
        If Not Session("PCKPicklist") Is Nothing Then
            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
            'oPicklist.PrintShipLabels(prntr.PrinterQName)
            If oPicklist.ShouldPrintShipLabel Then
                oPicklist.PrintShipLabels(prntr.PrinterQName)
            End If
            Try
                ' Update End Time : RWMS-1497
                If oPicklist.isCompleted Then
                    PickTask.UpdateCompletionTime(oPicklist)
                End If
                ' Update End Time :  RWMS-1497
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            End Try
        ElseIf Not Session("PARPCKPicklist") Is Nothing Then
            Dim pck As ParallelPicking = Session("PARPCKPicklist")
            pck.PrintShipLabels(prntr.PrinterQName)
        End If
        If Not Session("PCKPicklist") Is Nothing Then
            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
            If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
            Else
                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
            End If
        End If

    End Sub

    Private Sub doSkip()
        'Commented for PWMS-913(RWMS-1055) Start
        'Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
        'Commented for PWMS-913(RWMS-1055) End

        'Added for PWMS-913(RWMS-1055) Start
        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen")))
        'Added for PWMS-913(RWMS-1055) End

    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                doMenu()
            Case "next"
                doNext()
            Case "skip"
                doSkip()
        End Select
    End Sub

End Class