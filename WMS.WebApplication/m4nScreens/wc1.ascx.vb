Imports System.Collections.Specialized

Public Class wc1
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEReport As Made4Net.WebControls.TableEditor
    Protected WithEvents TEReportData As Made4Net.WebControls.TableEditor
    Protected WithEvents TEFormat As Made4Net.WebControls.TableEditor
    Protected WithEvents DCFormat As Made4Net.WebControls.DataConnector
    Protected WithEvents DCFilters As Made4Net.WebControls.DataConnector
    Protected WithEvents TEFilters As Made4Net.WebControls.TableEditor
    Protected WithEvents chkShowResultSet As System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEReport_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReport.CreatedChildControls
        With TEReport
            With .ActionBar
                'override existing button
                With .Button("Save")
                    .ObjectDLL = "Made4Net.Schema.dll"
                    .ObjectName = "Made4Net.Schema.Reporting.DynamicReportExec"
                    If TEReport.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .CommandName = "CreateNew"
                    Else
                        .CommandName = "UpdateReport"
                    End If
                End With

                .AddExecButton("RefreshSchema", "Refresh Schema") 'Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnNew"))
                With .Button("RefreshSchema")
                    .ObjectDLL = "Made4Net.Schema.dll"
                    .ObjectName = "Made4Net.Schema.Reporting.DynamicReportExec"
                    .CommandName = "RefreshSchema"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit
                End With

                .AddExecButton("RebuildSchema", "Rebuild Schema") 'Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnNew"))
                With .Button("RebuildSchema")
                    .ObjectDLL = "Made4Net.Schema.dll"
                    .ObjectName = "Made4Net.Schema.Reporting.DynamicReportExec"
                    .CommandName = "RebuildSchema"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit
                End With
            End With
        End With
    End Sub

    Private Sub SetReportDataParams()
        Dim dt As DataTable = TEReport.CreateDataTableForSelectedRecord(False, Made4Net.WebControls.TableEditorMode.Edit)
        If dt Is Nothing Then Exit Sub
        Dim dr As DataRow = dt.Rows(0)

        Dim DTName As String = Made4Net.Schema.Reporting.DynamicReport.GetDataTemplateName(dr("id"))
        Me.PreviewDT = DTName
        TEReportData.DefaultDT = DTName
        TEReportData.SQL = dr("sql_query")
    End Sub

    Private Sub TEFormat_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEFormat.CreatedChildControls
        With TEFormat
            With .ActionBar
                .AddExecButton("SaveFields", "Save Fields")
                With .Button("SaveFields")
                    .ObjectDLL = "Made4Net.Schema.dll"
                    .ObjectName = "Made4Net.Schema.Reporting.DynamicReportExec"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With

            End With
        End With
    End Sub

    Private Sub chkShowResultSet_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowResultSet.CheckedChanged
        If chkShowResultSet.Checked Then
            SetReportDataParams()
            TEReportData.Visible = True
            TEReportData.Restart()
        Else
            TEReportData.Visible = False
        End If
    End Sub

    Private Property PreviewDT() As String
        Get
            Return ViewState("PreviewDT")
        End Get
        Set(ByVal Value As String)
            ViewState("PreviewDT") = Value
        End Set
    End Property

    Private Sub TEReport_BeforeItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEReport.BeforeItemCommand
        'always refresh detail data
        TEReportData.RefreshData()
        TEFormat.RefreshData()
        TEFilters.RefreshData()
    End Sub

    Private Sub TEReportData_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReportData.Load
        If Me.PreviewDT Is Nothing OrElse Me.PreviewDT = String.Empty Then
        Else
            TEReportData.DefaultDT = Me.PreviewDT
        End If
    End Sub

    Private Sub TEFilters_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEFilters.CreatedChildControls
        With TEFilters
            With .ActionBar
                .AddExecButton("SaveFilters", "Save Filters")
                With .Button("SaveFilters")
                    .ObjectDLL = "Made4Net.Schema.dll"
                    .ObjectName = "Made4Net.Schema.Reporting.DynamicReportExec"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With

            End With
        End With

    End Sub
End Class
