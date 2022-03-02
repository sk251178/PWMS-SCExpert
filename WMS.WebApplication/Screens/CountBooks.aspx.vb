Public Partial Class CountBooks
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        If CommandName = "CountAll" Then
            Dim oCntBook As New WMS.Logic.CountBook(dr("countbook"))
            oCntBook.CreateCountTasks(WMS.Logic.Common.GetCurrentUser)
            Message = "Creating Count Tasks"
        ElseIf CommandName = "RecountDiscrepancies" Then
            Dim oCntBook As New WMS.Logic.CountBook(dr("countbook"))
            oCntBook.CreateCountTaskDescripanices(WMS.Logic.Common.GetCurrentUser)
            Message = "Recount discrepancies for location count tasks created"
        ElseIf CommandName = "createbook" Then
            Dim oCntBook As New WMS.Logic.CountBook
            oCntBook.CreateNew(dr("countbook"), dr("note"), dr("COUNTTYPE"), WMS.Logic.Common.GetCurrentUser)
        ElseIf CommandName = "complete" Then
            Dim oCntBook As New WMS.Logic.CountBook(dr("countbook"))
            oCntBook.Complete(WMS.Logic.Common.GetCurrentUser)
            Message = "All location count tasks completed"
        End If
    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ddSummaryType.DataBind()
            ddSummaryType_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub ddSummaryType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddSummaryType.SelectedIndexChanged
        If ddSummaryType.SelectedValue.ToLower = "location" Then
            pnlLocationSummary.Visible = True
            pnlLoadDiscrepancies.Visible = False
            TELocSummary.Visible = True
            TELocSummary.Restart()
        Else
            pnlLoadDiscrepancies.Visible = True
            pnlLocationSummary.Visible = False
            TECountDisc.Visible = True
            TECountDisc.Restart()
        End If
    End Sub

    Private Sub TECountBook_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECountBook.CreatedChildControls
        With TECountBook.ActionBar
            .AddSpacer()
            .AddExecButton("CountAll", "Count All", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            .AddSpacer()
            .AddExecButton("RecountDiscrepancies", "Recount Discrepancies", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
            .AddSpacer()
            .AddExecButton("complete", "Complete", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            .AddSpacer()

            With .Button("CountAll")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CountBooks"
                .ConfirmRequired = True
                .ConfirmMessage = "All unfinished tasks will be canceled, are you sure you want to continue?"
            End With
            With .Button("RecountDiscrepancies")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CountBooks"
                .ConfirmRequired = True
                .ConfirmMessage = "All unfinished tasks will be canceled, are you sure you want to continue?"
            End With
            With .Button("complete")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CountBooks"
                .ConfirmRequired = True
                .ConfirmMessage = "All unfinished tasks will be canceled, are you sure you want to continue?"
            End With
            With .Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CountBooks"
                If TECountBook.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "createbook"
                    'ElseIf TECountBook.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    '    .CommandName = "EditOrder"
                End If
            End With

        End With
    End Sub
End Class
