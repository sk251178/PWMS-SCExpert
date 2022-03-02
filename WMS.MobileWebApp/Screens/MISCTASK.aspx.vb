Imports Made4Net.Shared.Web
Imports WMS.Logic

Public Class MISCTASK
    Inherits PWMSRDTBase

    Public ReadOnly Property SessionTaskExists As Boolean
        Get
            Return Session("TMTask") IsNot Nothing
        End Get
    End Property

    <CLSCompliant(False)>
    Public Property SessionTask As Task
        Get
            Dim val As Task = Session("TMTask")
            Return val
        End Get
        Set(value As Task)
            If value Is Nothing Then
                Session.Remove("TMTask")
            Else
                Session("TMTask") = value
            End If
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            If SessionTaskExists Then
                SessionTask.STARTTIME = DateTime.Now
            End If
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Message", "Actual task time will be calculated when you select the 'Complete Task' button")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        If e.CommandText.ToLower.Equals("complete task") Then
            donext()
        End If

    End Sub

    Private Sub donext()

        If SessionTaskExists Then
            With SessionTask
                .ENDTIME = DateTime.Now
                .Save()
                .Complete(LogHandler.GetRDTLogger())
                Dim duration As TimeSpan = .ENDTIME - .STARTTIME
                WriteToRDTLog(" Updated task {0}, Duration={1}", .TASK, duration.TotalSeconds)
            End With
        End If

        Session("SetScreen") = False
        Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
    End Sub

    Private Function validateTime(time As String) As Boolean
        Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$")
        Return regex.IsMatch(time)
    End Function

End Class