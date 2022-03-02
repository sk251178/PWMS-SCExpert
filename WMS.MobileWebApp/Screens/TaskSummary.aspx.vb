Imports Made4Net.Shared.Web
Imports WMS.Logic
Partial Public Class TaskSummary
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If String.IsNullOrEmpty(Session()("TaskID")) Then
            Response.Redirect(MapVirtualPath("/screens/main.aspx"))
        End If
        If Made4Net.Shared.Util.GetSystemParameterValue("ShowSummaryOnTaskComplete") = "0" Then
            DoNext()
        End If
        If Not IsPostBack Then
            Session.Remove("PCKDELTask")
            Dim oTsk As New WMS.Logic.Task(Session()("TaskID"))
            If oTsk Is Nothing Then
                Response.Redirect(MapVirtualPath("/screens/main.aspx"))
            End If
            ' RWMS-1874 -> RWMS-1853: Labor code needs to check TaskType in "Labor task calculation" table if that is not defined there then no need to call Labor parameters in Execution.
            If Not taskCalculationIDExists(oTsk.TASKTYPE, oTsk.TASKSUBTYPE) Then
                DoNext()
            End If
            ' RWMS-1874 -> RWMS-1853 : Labor code needs to check TaskType in "Labor task calculation" table if that is not defined there then no need to call Labor parameters in Execution.

            Dim sql As String = String.Format("Select count(1) from laborperformanceaudit where taskid={0}", Made4Net.Shared.FormatField(oTsk.TASK))
            Dim laborServiceCompleted As Boolean
            Dim tryNumber As Integer = 0
            While True
                laborServiceCompleted = System.Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
                If laborServiceCompleted Then Exit While
                If tryNumber = 5 Then
                    Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("An error occured while trying to calculate the performance."))
                    Exit While
                End If
                System.Threading.Thread.Sleep(500)
                tryNumber += 1
            End While
            DO1.Value("TaskID") = oTsk.TASK
            DO1.Value("TaskType") = oTsk.TASKTYPE
            DO1.Value("TaskSubType") = oTsk.TASKSUBTYPE
            DO1.Value("MHType") = Session()("MHType")
            'DO1.Value("STDTime") = oTsk.STDTIME

            Dim stdTimeStr As String
            Dim secondsStr As String = Math.Round(oTsk.STDTIME Mod 60).ToString()
            If secondsStr.Length = 1 Then
                secondsStr = secondsStr.PadLeft(2, "0")
            End If
            stdTimeStr = (Math.Floor(oTsk.STDTIME / 60)).ToString() & ":" & secondsStr
            DO1.Value("STDTIME") = stdTimeStr

            Dim actualTimeStr As String
            secondsStr = (oTsk.EXECUTIONTIME Mod 60).ToString()
            If secondsStr.Length = 1 Then
                secondsStr = secondsStr.PadLeft(2, "0")
            End If
            actualTimeStr = (Math.Floor(oTsk.EXECUTIONTIME / 60)).ToString() & ":" & secondsStr
            DO1.Value("ActualTime") = actualTimeStr
            If Made4Net.Shared.Util.GetSystemParameterValue("ShowPerformanceOnTaskComplete") = "1" Then
                'RWMS-2835- Converting negative performance to 0
                DO1.Value("TaskPerformance") = If(oTsk.GetTaskPerformance() < 0, 0, oTsk.GetTaskPerformance()) & "%"
                'RWMS-2835
                ' RWMS-2057
                DO1.Value("UserShiftPerformance") = Math.Round(ShiftInstance.GetUserPerformanceOnShift(WMS.Logic.GetCurrentUser(), ShiftInstance.getShihtIDbyUserID(WMS.Logic.GetCurrentUser())), 2, MidpointRounding.AwayFromZero) & "%"
                ' RWMS-2057
                DO1.setVisibility("TaskPerformance", 1)
            Else
                DO1.setVisibility("TaskPerformance", 0)
                DO1.setVisibility("UserShiftPerformance", 0)
            End If
        End If
        DO1.DefaultButton = DO1.LeftButtonText
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TaskID")
        DO1.AddLabelLine("TaskType")
        DO1.AddLabelLine("TaskSubType")
        DO1.AddLabelLine("MHType")
        DO1.AddLabelLine("STDTime")
        DO1.AddLabelLine("ActualTime")
        DO1.AddLabelLine("TaskPerformance")
        DO1.AddLabelLine("UserShiftPerformance")
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        If e.CommandText.ToLower = "next" Then
            DoNext()
        End If
    End Sub

    Private Sub DoNext()

        Dim nextScreen As String = ""
        If Not IsNothing(Session("ScreenAfterTaskSummary")) Then
            nextScreen = Session("ScreenAfterTaskSummary").ToString()
            Session.Remove("ScreenAfterTaskSummary")
        End If

        WriteToRDTLog(" TaskSummary.DoNext nextScreen='{0}'", nextScreen)

        Dim oTsk As New WMS.Logic.Task(Session()("TaskID"))
        Session()("TaskID") = ""

        If oTsk.ASSIGNMENTTYPE = WMS.Lib.TASKASSIGNTYPE.AUTOMATIC Then
            WriteToRDTLog(" TaskSummary.DoNext - going to TaskManager - assignment type is auto")
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        End If

        'add 16/07 go to taskmanager
        If nextScreen = "PCK" Then
            WriteToRDTLog(" TaskSummary.DoNext - going to TaskManager - next screen is 'PCK'")
            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
        End If

        If nextScreen <> "" Then
            Session.Remove("ScreenAfterTaskSummary")
            Response.Redirect(MapVirtualPath("screens/" & nextScreen & ".aspx"))
        End If

        Select Case oTsk.TASKTYPE.ToUpper
            Case WMS.Lib.TASKTYPE.CONSOLIDATION
                Response.Redirect(MapVirtualPath("Screens/CONS.aspx"))
            Case WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY
                Response.Redirect(MapVirtualPath("Screens/CONS.aspx"))
            Case WMS.Lib.TASKTYPE.CONTCONTDELIVERY
                Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            Case WMS.Lib.TASKTYPE.CONTDELIVERY
                Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY
                Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            Case WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
                Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
            Case WMS.Lib.TASKTYPE.CONTPUTAWAY
                Response.Redirect(MapVirtualPath("Screens/CNTPWCNF.aspx"))
            Case WMS.Lib.TASKTYPE.LOADCOUNTING
                Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
            Case WMS.Lib.TASKTYPE.DELIVERY
                Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            Case WMS.Lib.TASKTYPE.FULLPICKING
                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            Case WMS.Lib.TASKTYPE.PARTREPL
                Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
            Case WMS.Lib.TASKTYPE.LOADDELIVERY
                Response.Redirect(MapVirtualPath("Screens/Del.aspx"))
            Case WMS.Lib.TASKTYPE.LOADPUTAWAY
                If MultiPayloadPutawayHelper.IsMultiPayLoadPutAwayTask(oTsk.TASK) Then
                    Response.Redirect(MapVirtualPath("Screens/MPPW.aspx"))
                Else
                    Response.Redirect(MapVirtualPath("Screens/LDPW.aspx"))
                End If
            Case WMS.Lib.TASKTYPE.NEGTREPL
                Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
            Case WMS.Lib.TASKTYPE.FULLREPL
                Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
            Case WMS.Lib.TASKTYPE.PARALLELPICKING
                Response.Redirect(MapVirtualPath("Screens/PARPCK1.aspx"))
            Case WMS.Lib.TASKTYPE.PARTIALPICKING
                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            Case WMS.Lib.TASKTYPE.NEGPALLETPICK
                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            Case WMS.Lib.TASKTYPE.PICKING
                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            Case WMS.Lib.TASKTYPE.PUTAWAY
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
            Case WMS.Lib.TASKTYPE.REPLENISHMENT
                Response.Redirect(MapVirtualPath("Screens/Repl.aspx"))
        End Select
    End Sub

    ' RWMS-1874 -> RWMS-1853 : Labor code needs to check TaskType in "Labor task calculation" table if that is not defined there then no need to call Labor parameters in Execution.
    Public Function taskCalculationIDExists(_tasktype As String, _tasksubtype As String
     ) As Boolean

        Dim sql As String = String.Format("SELECT top 1 LABORCALCID, isnull(GENERICTIME,0) as GENERICTIME, " & _
         " isnull(NEWASSIGMENT,0) as NEWASSIGMENT, isnull(DEFAULTMHETYPE,'') as DEFAULTMHETYPE " & _
         " FROM LABORTASKCALCULATION where TASKTYPE ='{0}' and isnull(TASKSUBTYPE,'')='{1}' ", _tasktype, _tasksubtype)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

End Class