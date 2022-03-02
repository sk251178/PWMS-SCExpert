Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports WMS.Lib

Partial Public Class UseClockInAssignTask
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#Region "Ctor"

    Public Sub New() 'Default Ctor
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Try
            Dim ds As New DataSet
            ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
            For Each dr As DataRow In ds.Tables(0).Rows
                Select Case CommandName.ToLower
                    Case "userclockin"
                        ''Dim MasterShiftID As String = DataInterface.ExecuteScalar(String.Format("select "))
                        Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim User As String = ReplaceDBNull(dr("USERID"))
                        If ShiftID = String.Empty Then
                            ShiftInstance.ClockUser(User, WMS.Lib.Shift.ClockStatus.IN, "")
                            Message = String.Format("User {0} clock-in.", User)
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-in User to Shift. User already Clock-in.", "Cannot Clock-in User to Shift. User already Clock-in.")
                        End If
                    Case "userclockout"
                        Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim User As String = ReplaceDBNull(dr("USERID"))
                        If ShiftID <> String.Empty Then
                            ShiftInstance.ClockUser(User, WMS.Lib.Shift.ClockStatus.OUT, "", ShiftID)
                            ' WebAppUtil.ClockUserCheckOut(User, WMS.Lib.Shift.ClockStatus.OUT, ShiftID)
                            Message = String.Format("User {0} clock-out.", User)
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-out User from Shift. User not Clock-in.", "Cannot Clock-out User from Shift. User not Clock-in.")
                        End If
                    Case "assigntask"
                        Dim User As String = WMS.Logic.GetCurrentUser
                        Dim TaskID As String = ReplaceDBNull(dr("TASK"))
                        If TaskID <> String.Empty And User <> String.Empty Then
                            Dim oTask As WMS.Logic.Task = New Task(TaskID, True)
                            If oTask.STATUS = WMS.Lib.Statuses.Task.AVAILABLE Then
                                oTask.AssignUser(User)
                                Message = "Task assigned successfully."
                            Else
                                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot assign task: Task status: " & oTask.STATUS, "Cannot assign task: Task status: " & oTask.STATUS)
                            End If
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot assign task: user or task is empty.", "Cannot assign task: user or task is empty.")

                        End If
                    Case "unassigntask"
                        Dim User As String = WMS.Logic.GetCurrentUser
                        Dim TaskID As String = ReplaceDBNull(dr("TASK"))
                        If TaskID <> String.Empty Then
                            Dim oTask As WMS.Logic.Task = New Task(TaskID, True)
                            If oTask.STATUS = WMS.Lib.Statuses.Task.ASSIGNED Then
                                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot assign task, Incorrect status", "Cannot assign task, Incorrect status")
                                oTask.DeAssignUser()
                                Message = "Task deassigned successfully."
                            Else
                                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot assign task, Incorrect status", "Cannot assign task, Incorrect status")
                            End If
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot unassign task: task is empty.", "Cannot unassign task: task is empty.")
                        End If
                    Case "completetask"
                        Dim User As String = WMS.Logic.GetCurrentUser
                        Dim TaskID As String = ReplaceDBNull(dr("TASK"))
                        If TaskID <> String.Empty Then
                            Dim oTask As WMS.Logic.Task = New Task(TaskID, True)
                            If oTask.STATUS = WMS.Lib.Statuses.Task.ASSIGNED Then
                                oTask.Complete(Nothing)
                                Message = "Task completed successfully."
                            Else
                                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot complete task: Task status: " & oTask.STATUS, "Cannot complete task: Task status: " & oTask.STATUS)
                            End If
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot complete task: task is empty.", "Cannot deassign task: task is empty.")
                        End If
                End Select
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

    Private Sub TEUseClockInAssignTask_BeforeDataLoaded(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUseClockInAssignTask.BeforeDataLoaded
        TEUseClockInAssignTask.FilterExpression = "USERID='" & WMS.Logic.GetCurrentUser & "'"
    End Sub

    Private Sub TEUserAssignTask_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUserAssignTask.CreatedChildControls
        With TEUserAssignTask.ActionBar
            .AddSpacer()
            .AddExecButton("AssignTask", "Assign Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AssignTask")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UseClockInAssignTask"
                .CommandName = "AssignTask"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
                .ConfirmMessage = "Do you want assign task to User?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("UnAssignTask", "UnAssign Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("UnAssignTask")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UseClockInAssignTask"
                .CommandName = "UnAssignTask"
                .ConfirmMessage = "Do you want deassign User from task?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid

            End With

            .AddSpacer()
            .AddExecButton("CompleteTask", "Complete Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
            With .Button("CompleteTask")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UseClockInAssignTask"
                .CommandName = "CompleteTask"
                .ConfirmMessage = "Do you want complete task?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid

            End With
        End With
    End Sub

    Private Sub TEClockInAssignTask_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUseClockInAssignTask.CreatedChildControls
        With TEUseClockInAssignTask.ActionBar
            .AddSpacer()
            .AddExecButton("UserClockIn", "User Clock-In", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("UserClockIn")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UseClockInAssignTask"
                .CommandName = "UserClockIn"
                .ConfirmMessage = "Do you want clock-in User to Shift?"
                '.Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With

            .AddSpacer()
            .AddExecButton("UserClockOut", "User Clock-Out", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("UserClockOut")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.UseClockInAssignTask"
                .CommandName = "UserClockOut"
                .ConfirmMessage = "Do you want clock-out User from Shift?"
                '.Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With

        End With
    End Sub

End Class