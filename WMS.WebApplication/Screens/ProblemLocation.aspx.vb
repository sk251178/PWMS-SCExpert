Imports WMS.Logic

Partial Public Class ProblemLocation
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
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "reportproblem"
                Dim oLoc As WMS.Logic.Location
                For Each dr In ds.Tables(0).Rows
                    oLoc = New WMS.Logic.Location(dr("location"), dr("warehousearea"))
                    oLoc.SetProblemFlag(True, dr("REASONCODE"), Common.GetCurrentUser)
                Next
            Case "cancelproblem"
                Dim oLoc As WMS.Logic.Location
                For Each dr In ds.Tables(0).Rows
                    oLoc = New WMS.Logic.Location(dr("location"), dr("warehousearea"))
                    'Commented for RWMS-1506 Start   
                    'oLoc.SetProblemFlag(False, dr("REASONCODE"), Common.GetCurrentUser)   
                    'Commented for RWMS-1506 End   

                    'Added for RWMS-1506 Start   
                    oLoc.CancelProblemFlag(False, dr("REASONCODE"), Common.GetCurrentUser)
                    'Added for RWMS-1506 End  

                Next
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddReasonCode.DataBind()
            ddReasonCode_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub ddReasonCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReasonCode.SelectedIndexChanged
        Dim vals As New Specialized.NameValueCollection
        vals.Add("REASONCODE", ddReasonCode.SelectedValue)
        TELocationProblems.PreDefinedValues = vals
    End Sub

    Private Sub TELocationProblems_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELocationProblems.CreatedChildControls
        With TELocationProblems.ActionBar
            .AddSpacer()
            .AddExecButton("ReportProblem", "Report Problem", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
            With .Button("ReportProblem")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ProblemLocation"
                .CommandName = "ReportProblem"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
            .AddSpacer()
            .AddExecButton("CancelProblem", "Cancel Problem", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
            With .Button("CancelProblem")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ProblemLocation"
                .CommandName = "CancelProblem"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
        End With
        If TELocationProblems.CurrentMode <> Made4Net.WebControls.TableEditorMode.Search Then
            pnlAdj.Visible = True
        Else
            pnlAdj.Visible = False
        End If
    End Sub
End Class