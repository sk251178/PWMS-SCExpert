Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

'Packing List Print Screen
Public Class DELPLPRNT
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)>
    Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    <CLSCompliant(False)>
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
            If Session("PCKPicklist") Is Nothing And Session("PARPCKPicklist") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
            End If
            If Not Session("PCKPicklist") Is Nothing Then
                Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                'Commented for NEWRWMS-452
                'If Not oPicklist.ShouldPrintContentList Then
                'End Commented for NEWRWMS-452
                'Added for NEWRWMS-452
                If Not oPicklist.GetPrintContentList() And oPicklist.GetContentListReoprtName() Is Nothing Then
                    'End Added for NEWRWMS-452

                    If MobileUtils.ShouldRedirectToTaskSummary() Then
                        Dim tm As WMS.Logic.TaskManager = WMS.Logic.TaskManager.NewTaskManagerForPicklist(oPicklist.PicklistID)
                        MobileUtils.RedirectToTaskSummary("Del", tm.Task.TASK)
                    Else
                        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    End If
                End If
            ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                Dim pck As ParallelPicking = Session("PARPCKPicklist")
                If Not pck.ShouldPrintContentList Then
                    If MobileUtils.ShouldRedirectToTaskSummary() Then
                        MobileUtils.RedirectToTaskSummary("Del")
                    Else
                        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub doNext()
        Dim prntr As String
        Try
            prntr = DO1.Value("ContentListPrinter")
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
            Return
        End Try
        If Not Session("PCKPicklist") Is Nothing Then
            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
            If Not oPicklist Is Nothing Then
                Dim strpicklistId As String = String.Empty
                strpicklistId = oPicklist.PicklistID
                oPicklist.PrintContentListReport(prntr, oPicklist.GetContentListReoprtName(), LogHandler.GetRDTLogger())
            End If
        ElseIf Not Session("PARPCKPicklist") Is Nothing Then
            Dim pck As ParallelPicking = Session("PARPCKPicklist")
            pck.PrintContentList(prntr, WMS.Logic.GetCurrentUser, Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        End If
        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
    End Sub
    'Added for NEWRWMS-452
    'Added for RWMS-1287 and RWMS-822
    Public Sub PrintContentListReport(ByVal repType As String, ByVal strpicklistId As String)
        'Ended for RWMS-1287 and RWMS-822
        Dim Language As Int32 = Made4Net.Shared.Translation.Translator.CurrentLanguageID
        Dim pUser As String = WMS.Logic.GetCurrentUser
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim dt As New DataTable
        'Added for RWMS-498 - partialpick
        Dim strcont As String = String.Empty



        'If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
        '    strcont = Session("PCKPicklistActiveContainerID")
        'End If
        'End RWMS-498 - partialpick
        'Added for RWMS-498 - fullpick
        Dim strload As String = String.Empty

        If Not Session("PCKPicklistPickJob") Is Nothing Then
            Dim pck As PickJob
            pck = Session("PCKPicklistPickJob")
            strload = pck.fromload
            Made4Net.DataAccess.DataInterface.FillDataset($"select tocontainer from pickdetail where picklist = '{pck.picklist}' and picklistline = {pck.PickDetLines(0)}", dt, False)
            If dt.Rows.Count > 0 Then
                strcont = dt.Rows(0).Field(Of String)("TOCONTAINER")
            End If
        End If
        'End RWMS-498 - fullpick

        'Commented for RWMS-2437
        'Made4Net.DataAccess.DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False, "Made4NetSchema")
        'End Commented for RWMS-2437
        'Added for RWMS-2437
        dt = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False)
        'End Added for RWMS-2437

        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)

        'Commented for RWMS-2437
        'oQsender.Add("DATASETID", Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("SELECT ParamValue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = 'DataSetName'", repType), "Made4NetSchema")) '"repOutboundDelNote")
        'End Commented for RWMS-2437
        'Added for RWMS-2437
        oQsender.Add("DATASETID", Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("SELECT ParamValue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = 'DataSetName'", repType))) '"repOutboundDelNote")
        'End Added for RWMS-2437

        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            'For RWMS-239
            'oQsender.Add("PRINTER", '')
            'End For RWMS-239
            oQsender.Add("PRINTER", DO1.Value("ContentListPrinter"))
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            'oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        'Begin Added for RWMS-498
        If strcont.Length > 0 Then
            'Added for RWMS-1287 and RWMS-822
            oQsender.Add("WHERE", String.Format("CONTAINERID = '{0}' AND PICKLIST = '{1}'", strcont, strpicklistId))
            'Ended for RWMS-1287 and RWMS-822
            oQsender.Send("Report", repType)
        ElseIf strload.Length > 0 Then
            'Added for RWMS-1287 and RWMS-822
            oQsender.Add("WHERE", String.Format("CONTAINERID = '{0}' AND PICKLIST = '{1}'", strload, strpicklistId))
            'Ended for RWMS-1287 and RWMS-822
            oQsender.Send("Report", repType)
        Else
            'oQsender.Add("WHERE", "")
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Error in printing, please reprint from workstation.")
        End If
        'END RWMS-498
        'oQsender.Send("Report", repType)

    End Sub
    'End Added for NEWRWMS-452

    Private Sub doSkip()
        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("ContentListPrinter", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "back"
                doBack()
            Case "next"
                doNext()
            Case "skip"
                doSkip()
        End Select
    End Sub

End Class