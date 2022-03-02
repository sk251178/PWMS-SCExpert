Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports WMS.Lib

Partial Public Class ShiftInstances
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()


    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region "Ctors"
    Public Sub New()

    End Sub
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Try
            Dim ds As New DataSet
            ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
            For Each dr As DataRow In ds.Tables(0).Rows

                Select Case CommandName.ToLower
                    Case "setstatusstarted"
                        Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim CurrentStatus As String = ReplaceDBNull(dr("STATUS"))
                        Dim StartDate As String = ReplaceDBNull(dr("STARTDATE"))
                        Dim EndDate As String = ReplaceDBNull(dr("ENDDATE"))

                        Dim SCHEDULEDSTARTTIME As Integer = ReplaceDBNull(dr("SCHEDULEDSTARTTIME"))
                        Dim SCHEDULEDENDTIME As Integer = ReplaceDBNull(dr("SCHEDULEDENDTIME"))

                        Dim StartTime As String = InttoTime(SCHEDULEDSTARTTIME)
                        Dim EndTime As String = InttoTime(SCHEDULEDENDTIME)

                        If CurrentStatus <> WMS.Lib.Shift.ShiftInstanceStatus.[NEW] Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Only Shift with status " & WMS.Lib.Shift.ShiftInstanceStatus.[NEW] & " can started.", "Only Shift with status " & WMS.Lib.Shift.ShiftInstanceStatus.[NEW] & " can started.")
                            Exit For
                        End If

                        If StartDate = String.Empty Or EndDate = String.Empty Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot start Shift without Start and End Date.", "Cannot start Shift without Start and End Date.")
                            Exit For
                        End If

                        ' ShiftInstance.ChangeShiftStatus(WMS.Lib.Shift.ShiftInstanceStatus.STARTED, ShiftID, StartDate & " " & StartTime, EndDate & " " & EndTime) 'RWMS-1667
                        ' SendEvent(ShiftID, WMS.Lib.Shift.ShiftInstanceStatus.STARTED, WMS.Logic.WMSEvents.EventType.ShiftInstanceStarted) 'RWMS-1667
                        '  Message = "Shift started successfully." 'RWMS-1667

                    Case "setstatusclosed"
                        Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim CurrentStatus As String = ReplaceDBNull(dr("STATUS"))
                        If CurrentStatus <> WMS.Lib.Shift.ShiftInstanceStatus.STARTED Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Only Shift with status " & WMS.Lib.Shift.ShiftInstanceStatus.STARTED & " can close.", "Only Shift with status " & WMS.Lib.Shift.ShiftInstanceStatus.STARTED & " can close.")
                            Exit For
                        End If
                        ShiftInstance.ChangeShiftStatus(WMS.Lib.Shift.ShiftInstanceStatus.CLOSED, ShiftID)

                        If WMS.Logic.GetSysParam("AutoClockOutonShiftClose") = "1" Then
                            ShiftInstance.CkeckOutUsersonShiftClose(ShiftID)
                        End If

                        SendEvent(ShiftID, WMS.Lib.Shift.ShiftInstanceStatus.CLOSED, WMS.Logic.WMSEvents.EventType.ShiftInstanceClosed)
                        Message = "Shift closed successfully."

                    Case "setstatuscanceled"
                        Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim CurrentStatus As String = ReplaceDBNull(dr("STATUS"))
                        If CurrentStatus = WMS.Lib.Shift.ShiftInstanceStatus.CLOSED Then
                            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not cancel a closed shift", "Can not cancel a closed shift")
                        End If
                        If CurrentStatus <> WMS.Lib.Shift.ShiftInstanceStatus.CANCELED Then
                            ShiftInstance.ChangeShiftStatus(WMS.Lib.Shift.ShiftInstanceStatus.CANCELED, ShiftID)
                            SendEvent(ShiftID, WMS.Lib.Shift.ShiftInstanceStatus.CANCELED, WMS.Logic.WMSEvents.EventType.ShiftInstanceCanceled)
                        End If
                        Message = "Shift canceled successfully."

                    Case "userclockin"
                        ''Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim MasterShiftID As String = dr("MASTERSHIFTID")
                        Dim ShiftID As String = dr("SHIFTID")
                        Dim User As String = ReplaceDBNull(dr("USERID"))
                        If ShiftID = String.Empty Then
                            ShiftInstance.ClockUser(User, WMS.Lib.Shift.ClockStatus.IN, "", MasterShiftID)
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-in User to Shift. User already Clock-in.", "Cannot Clock-in User to Shift. User already Clock-in.")
                        End If
                        Message = "Users clock-in successfully."

                    Case "userclockout"
                        Dim ShiftID As String = ReplaceDBNull(dr("SHIFTID"))
                        Dim MasterShiftID As String = dr("MASTERSHIFTID")
                        Dim User As String = ReplaceDBNull(dr("USERID"))
                        If ShiftID <> String.Empty Then
                            ShiftInstance.ClockUser(User, WMS.Lib.Shift.ClockStatus.OUT, "", MasterShiftID)
                            'WebAppUtil.ClockUserCheckOut(User, WMS.Lib.Shift.ClockStatus.OUT, ShiftID)
                        Else
                            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-out User from Shift. User not Clock-in.", "Cannot Clock-out User from Shift. User not Clock-in.")
                        End If
                        Message = "Users clock-out successfully."


                End Select

            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function InttoTime(ByVal TM As Integer) As String
        Try
            Dim md As Integer = TM Mod 100
            Return (Math.Round(TM / 100, 0).ToString & ":" & IIf(md < 10, "0" & md.ToString(), md.ToString()))
        Catch ex As Exception
            Return "00:00"
        End Try

    End Function


    Private Sub SendEvent(ByVal pShiftid As String, ByVal pStatus As String, _
                            ByVal pEventType As WMS.Logic.WMSEvents.EventType)
        Dim em As New EventManagerQ
        em.Add("EVENT", pEventType)
        em.Add("SHIFTID", pShiftid)
        em.Add("STATUS", pStatus)
        em.Add("USERID", WMS.Lib.Common.GetCurrentUser)
        em.Add("CREATEDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(pEventType))

    End Sub

    Private Sub TEShift_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEShift.CreatedChildControls
        With TEShift.ActionBar


            .AddSpacer()
            .AddExecButton("StartShiftInstance", "Start Shift", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarExecuteScheduler"))
            With .Button("StartShiftInstance")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftInstances"
                .CommandName = "SetStatusStarted"
                .ConfirmMessage = "Do you want start the Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("CloseShiftInstance", "Close Shift", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("CloseShiftInstance")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftInstances"
                .CommandName = "SetStatusClosed"
                .ConfirmMessage = "Do you want close the Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("CancelShiftInstance", "Cancel Shift", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCancel"))
            With .Button("CancelShiftInstance")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftInstances"
                .CommandName = "SetStatusCanceled"
                .ConfirmMessage = "Do you want cancel the Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

        End With

    End Sub

    Private Sub TEUsersClockIn_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUsersClockIn.CreatedChildControls
        With TEUsersClockIn.ActionBar
            .AddSpacer()
            .AddExecButton("UserClockIn", "User Clock-In", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("UserClockIn")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftInstances"
                .CommandName = "UserClockIn"
                .ConfirmMessage = "Do you want clock-in User to Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("UserClockOut", "User Clock-Out", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("UserClockOut")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ShiftInstances"
                .CommandName = "UserClockOut"
                .ConfirmMessage = "Do you want clock-out User from Shift?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

        End With
    End Sub



    Private Sub TEShift_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEShift.RecordSelected
        Dim tds As DataTable = TEShift.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Clear()
        vals.Add("MASTERSHIFTID", tds.Rows(0)("SHIFTID"))
        TEUsersClockIn.PreDefinedValues = vals
    End Sub

#End Region
End Class