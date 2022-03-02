Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class REPLT
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
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
        'Put user code to initialize the page here
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadID")
        Session.Remove("RelpTaskID")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Session("LoadID") = DO1.Value("LOADID")
        Session("RelpTaskID") = DO1.Value("RELPTASKID")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim strTask As String = AssignedToTask(DO1.Value("LOADID"), DO1.Value("RELPTASKID"))
            If strTask <> "" Then
                Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                Response.Redirect(MapVirtualPath("Screens/REPL.aspx"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No Available Task Found"))
            End If
        Catch ex As Threading.ThreadAbortException
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("RELPTASKID")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
    ' Returns TaskID that this load is assigned to
    Private Function AssignedToTask(ByVal LoadId As String, ByVal ReplTaskId As String) As String
        Dim strSql As String
        If LoadId <> "" Then
            Dim oLoad As New WMS.Logic.Load(LoadId)
            strSql = "SELECT TASK FROM TASKS WHERE (STATUS = 'AVAILABLE') AND FROMLOAD='" & LoadId & "'"
        Else
            strSql = "SELECT TASK FROM TASKS WHERE (STATUS = 'AVAILABLE') AND REPLENISHMENT ='" & ReplTaskId & "'"
        End If
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

End Class
