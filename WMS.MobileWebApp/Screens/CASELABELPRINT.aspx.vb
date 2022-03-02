Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

Public Class CASELABELPRINT
    Inherits PWMSRDTBase 'System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
    Private Sub doNext()
        Dim oWHActivity As New WMS.Logic.WHActivity
        Dim oLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetRDTLogger()
        Dim SQL As String
        If Not oLogger Is Nothing Then
            oLogger.Write("Flow Navigated to Case Label Printer... ")
        End If

        Session("DefaultPrinter") = DO1.Value("CaseLabelPrinter")
        MyBase.WriteToRDTLog("Case Labe Printer Value: " & DO1.Value("CaseLabelPrinter"))

        SQL = String.Format("update WHACTIVITY set PRINTER = '{0}' where USERID = '{1}'", Session("DefaultPrinter"), WMS.Logic.GetCurrentUser)

        Dim dt As New DataTable
        If Not oLogger Is Nothing Then
            oLogger.Write("Case Label Printer value is updated at WHACTIVITY : " & SQL)
        End If

        MyBase.WriteToRDTLog("Updated the WHACTIVITY with Default Printer Value : SQL : " & SQL)
        Made4Net.DataAccess.DataInterface.RunSQL(SQL)
        MobileUtils.UpdateDPrinterInUserProfile(Session("DefaultPrinter"), LogHandler.GetRDTLogger())

        Response.Redirect(MapVirtualPath("SCREENS/PCKPART.aspx"))
    End Sub
    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("CaseLabelPrinter", True, "next")
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