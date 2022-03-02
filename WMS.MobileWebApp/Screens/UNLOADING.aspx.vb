Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert

<CLSCompliant(False)> Partial Public Class UNLOADING
    Inherits PWMSRDTBase

    Protected _task As String = String.Empty
    Protected _door As String
    Protected _warehousearea As String
    Protected _otask As UnloadingTask

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DO1.AddSpacer()
        DO1.AddLabelLine("TASK")
        DO1.AddLabelLine("DOOR")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddTextboxLine("DOORCONFIRMATION", True, "next")
        DO1.AddLabelLine("No task available")
        DO1.setVisibility("No task available", False)

        If Not IsPostBack Then
            Dim SQL As String
            SQL = String.Format("select TASK,FROMLOCATION as Door, FromWarehouseArea as WarehouseArea from TASKS " & _
            " where TASKTYPE='{0}' and USERID='{1}' and STATUS='{2}'", _
            WMS.Lib.TASKTYPE.UNLOADING, WMS.Logic.Common.GetCurrentUser, WMS.Lib.Statuses.Task.ASSIGNED)

            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(SQL, dt)

            If dt.Rows.Count > 0 Then
                dr = dt.Rows(0)
                _task = ReplaceDBNull(dr.Item("TASK"))
                _door = ReplaceDBNull(dr.Item("Door"))
                _warehousearea = ReplaceDBNull(dr.Item("WarehouseArea"))
                showTask()
            Else
                assignNextTask()
            End If
        End If
    End Sub

    Private Sub assignNextTask()
        Dim Sql As String = String.Format("select top 1 TASK,FROMLOCATION as Door, FromWarehouseArea as WarehouseArea from TASKS " & _
                " where TASKTYPE='{0}'  and STATUS='{1}'", _
                 WMS.Lib.TASKTYPE.UNLOADING, WMS.Lib.Statuses.Task.AVAILABLE)
        Dim dt1 As New DataTable
        Dim dr1 As DataRow
        DataInterface.FillDataset(Sql, dt1)
        If dt1.Rows.Count > 0 Then
            dr1 = dt1.Rows(0)
            _task = ReplaceDBNull(dr1.Item("TASK"))
            _door = ReplaceDBNull(dr1.Item("Door"))
            _warehousearea = ReplaceDBNull(dr1.Item("WarehouseArea"))
            _otask = New UnloadingTask(_task)
            _otask.AssignTask(WMS.Logic.Common.GetCurrentUser)
            showTask()
        Else
            showTask()
            NoTaskAvailable()
        End If
    End Sub

    Private Sub NoTaskAvailable()
        DO1.setVisibility("No task available", True)
        DO1.setVisibility("TASK", False)
        DO1.setVisibility("DOOR", False)
        DO1.setVisibility("WAREHOUSEAREA", False)
        DO1.setVisibility("DOORCONFIRMATION", False)
    End Sub

    Private Sub showTask()
        DO1.Value("TASK") = _task
        DO1.Value("DOOR") = _door
        DO1.Value("WAREHOUSEAREA") = _warehousearea
        DO1.Value("DOORCONFIRMATION") = ""
    End Sub

    Private Sub doNext()
        Try
            
            If DO1.Value("TASK") = "" Then
                assignNextTask()
                Return
            End If

            If DO1.Value("DOOR") <> DO1.Value("DOORCONFIRMATION") Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Door Confirmation Incorrect.")
                Return
            End If
            _otask = New UnloadingTask(DO1.Value("TASK"))
            _otask.Complete(WMS.Logic.LogHandler.GetRDTLogger)
            _otask = Nothing
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  String.Format("Task {0} Completed Successfully.", DO1.Value("TASK")))

            DO1.Value("TASK") = ""
            DO1.Value("DOOR") = ""
            DO1.Value("WAREHOUSEAREA") = ""
            DO1.Value("DOORCONFIRMATION") = ""

            assignNextTask()
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

    Private Sub doMenu()
        If DO1.Value("TASK") <> String.Empty Then
            Dim oUnloadingTask As UnloadingTask = New UnloadingTask(DO1.Value("TASK"))
            oUnloadingTask.DeAssignUser()
        End If
        Made4Net.Mobile.Common.GoToMenu()
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