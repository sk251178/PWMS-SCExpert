Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports System.Data

Public Class Tasks
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TETASKS As Made4Net.WebControls.TableEditor

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
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "CancelTask"
                For Each dr In ds.Tables(0).Rows
                    Dim oTask As WMS.Logic.Task = TaskManager.GetTask(dr("TASK"))
                    If oTask.STATUS <> WMS.Lib.Statuses.Task.COMPLETE Then
                        oTask.Cancel()
                    End If
                Next
            Case "UnAssign"
                For Each dr In ds.Tables(0).Rows
                    Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(dr("TASK"))
                    oTask.ExitTask()
                Next
            Case "AssignTask"
                For Each dr In ds.Tables(0).Rows
                    Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(dr("TASK"))
                    If oTask.STATUS = WMS.Lib.Statuses.Task.COMPLETE Or oTask.STATUS = WMS.Lib.Statuses.Task.CANCELED Then
                        Throw New M4NException(New Exception, "Task Status Incorrect", "Task Status Incorrect")
                    End If
                    Dim username As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("USERID"), "")
                    If username = "" Then
                        'Throw New M4NException(New Exception, "Assigned User Cannot be Blank", "Assigned User Cannot be Blank")

                        Dim priority As Int32 = -1
                        If Not IsDBNull(dr("PRIORITY")) Then
                            priority = dr("PRIORITY")
                            oTask.SetPriority(priority)
                        End If
                    Else
                        Dim status As Boolean? = DataInterface.ExecuteScalar(String.Format("Select [Status] from UserProfile where UserId='{0}'", dr("USERID")), Made4Net.Schema.CONNECTION_NAME)
                        If status.HasValue And status = True Then
                            Dim priority As Int32 = -1
                            If Not IsDBNull(dr("PRIORITY")) Then
                                priority = dr("PRIORITY")
                            End If
                            oTask.AssignUser(username, WMS.Lib.TASKASSIGNTYPE.MANUAL, "", priority)
                        End If
                    End If
                Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TETASKS_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TETASKS.CreatedChildControls
        With TETASKS
            With .ActionBar
                .AddSpacer()
                .AddExecButton("UnAssign", "UnAssign", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                With .Button("UnAssign")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Tasks"
                    .CommandName = "UnAssign"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to UnAssign the user from the selected Tasks?"
                End With
                .AddSpacer()
                .AddExecButton("CancelTask", "Cancel Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelTask")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Tasks"
                    .CommandName = "CancelTask"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected Tasks?"
                End With

                With .Button("Save")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Tasks"
                    .CommandName = "AssignTask"
                    '.Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    '.ConfirmRequired = True
                    '.ConfirmMessage = "Are you sure you want to Assign the selected Tasks?"
                End With

            End With
        End With
    End Sub

End Class