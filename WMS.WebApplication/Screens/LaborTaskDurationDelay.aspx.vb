Imports WMS.Logic
Imports Made4Net.DataAccess

Public Class LaborTaskDurationDelay
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TELABORTASKDURATION As Made4Net.WebControls.TableEditor

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here


    End Sub
    Public Sub New()

    End Sub
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim strActualTime As String, strAssignmentID As String
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        strAssignmentID = IIf(String.IsNullOrEmpty(Session("ASSIGNMENTID")), "0", Session("ASSIGNMENTID"))
        strActualTime = Session("ACTUALTIME").ToString()
        Select Case CommandName.ToLower
            Case "update"
                dr = ds.Tables(0).Rows(0)
                'If dr("DELAY").ToString() = "00:00" Then Throw New ApplicationException(t.Translate("Enter valid delay (24 Hr format - HH:MM)."))
                If ValidateDelay(dr("DELAY").ToString(), strActualTime) Then
                    Dim sql As String = String.Format("Update LABORPERFORMANCEAUDIT set DELAY ='{1}', DELAYTYPE ='{2}' where ASSIGNMENTID='{0}'", strAssignmentID, dr("DELAY").ToString(), dr("DELAYTYPE").ToString())
                    Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                Else
                    Throw New ApplicationException(t.Translate("Delay : " + dr("DELAY").ToString()) + " should not be greater than actual time : " + strActualTime.ToString())
                End If
        End Select
    End Sub

    Protected Sub TELABORTASKDURATION_CreatedChildControls(sender As Object, e As EventArgs) Handles TELABORTASKDURATION.CreatedChildControls
        With TELABORTASKDURATION
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LaborTaskDurationDelay"
                If TELABORTASKDURATION.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "Update"
                End If
            End With
        End With
    End Sub

    Protected Function ValidateDelay(ByVal Delaytime As String, ByVal strActualTime As String) As Boolean
        Try
            Dim iDelayTimeInSeconds As Integer, iActualTimeinSeconds As Integer
            iActualTimeinSeconds = ConvertTimeToSeconds(strActualTime)
            iDelayTimeInSeconds = ConvertTimeToSeconds(Delaytime)
            If iDelayTimeInSeconds > iActualTimeinSeconds Or iActualTimeinSeconds < 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
        End Try
    End Function

    Protected Sub TELABORTASKDURATION_RecordSelected(sender As Made4Net.WebControls.TableEditor, e As Made4Net.WebControls.TableEditorEventArgs) Handles TELABORTASKDURATION.RecordSelected
        Dim tds As DataTable = TELABORTASKDURATION.CreateDataTableForSelectedRecord()
        Session("ASSIGNMENTID") = tds.Rows(0)("ASSIGNMENTID")
        Session("ACTUALTIME") = tds.Rows(0)("ACTUALTIME")

    End Sub

    Private Function ConvertTimeToSeconds(strTime As String) As Integer
        Dim strDelaytime() As String, iHoursToMinutes As Integer, iMinutesInDelaytime As Integer, iTimeInSeconds As Integer

        strDelaytime = strTime.Split(":") '12:30

        iHoursToMinutes = Convert.ToInt32(IIf(String.IsNullOrEmpty(strDelaytime(0)), "0", strDelaytime(0))) '12
        iHoursToMinutes = iHoursToMinutes * 60 '(12*60)
        iMinutesInDelaytime = Convert.ToInt32(IIf(String.IsNullOrEmpty(strDelaytime(1)), "0", strDelaytime(1))) '30
        iTimeInSeconds = (iHoursToMinutes + iMinutesInDelaytime) * 60
        If strDelaytime.Length = 3 Then
            If strDelaytime(2) IsNot Nothing Then
                iTimeInSeconds += Convert.ToInt32(IIf(String.IsNullOrEmpty(strDelaytime(2)), "0", strDelaytime(2)))
            End If
        End If

        Return iTimeInSeconds
    End Function
End Class