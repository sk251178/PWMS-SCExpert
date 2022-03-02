Imports WMS.Logic

Partial Public Class LoadCountSchedule
    Inherits System.Web.UI.Page

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
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "CreateCountJobs"
                Dim oCount As New WMS.Logic.Counting
                For Each dr In ds.Tables(0).Rows
                    oCount.CreateLoadCountJobs("", "", "", "", dr("loadid"), Common.GetCurrentUser)
                Next
                Message = ts.Translate("Count tasks created for selected loads")
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TELoadsSched_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELoadsSched.CreatedChildControls
        With TELoadsSched
            With .ActionBar
                .AddSpacer()
                .AddExecButton("CreateCountJobs", "Create Count Jobs", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("CreateCountJobs")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.LoadCountSchedule"
                    .CommandName = "CreateCountJobs"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                End With
            End With
        End With
    End Sub

End Class