Imports Made4Net.DataAccess

Public Class Reports
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Generator As Made4Net.WebControls.ReportScreen
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen

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
        If Not IsPostBack Then
            ' Get Report ID
            Generator.ReportID = DataInterface.ExecuteScalar("SELECT [id] FROM sys_menu WHERE screen_id = '" & Request.QueryString("sc") & "'", "Made4NetSchema")
            ' Get report Type
            Select Case Convert.ToString(DataInterface.ExecuteScalar("SELECT ReportType FROM sys_reports WHERE ReportID = '" & Generator.ReportID & "'"))
                Case "SharpShooterPS"
                    Generator.ReportType = Made4Net.WebControls.ReportTypes.SharpShooterPS
            End Select
            ' Get Report Dataset ID
            Generator.DataSetID = DataInterface.ExecuteScalar("SELECT DATASETID FROM sys_reportdataproviderinterface WHERE ReportID = '" & Generator.ReportID & "'")
            ' Get Report DT
            Generator.DataTemplateName = DataInterface.ExecuteScalar("SELECT ReportFormDT FROM sys_reports WHERE ReportID = '" & Generator.ReportID & "'")
            Try
                'Generator.ReportLanguage = Made4Net.Shared.Translation.Translator.CurrentLanguageID
            Catch ex As Exception
                'Generator.ReportLanguage = 0
            End Try

        End If

    End Sub

End Class
