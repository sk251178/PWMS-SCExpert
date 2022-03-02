Imports Made4Net.DataAccess

Public Class ReportSetup
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEReport As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS2 As Made4Net.WebControls.TabStrip
    Protected WithEvents TEReportParam As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents TEReportGenerator As Made4Net.WebControls.TableEditor
    Protected WithEvents SQLBuilder As System.Web.UI.WebControls.Label
    Protected WithEvents SQLText As System.Web.UI.WebControls.TextBox
    Protected WithEvents RunSQL As System.Web.UI.WebControls.Button
    Protected WithEvents SQLResult As System.Web.UI.WebControls.Label
    Protected WithEvents ResDataGrid As System.Web.UI.WebControls.DataGrid
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblTabStrip2 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents tblReportParam As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEDataProvider As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDataProviderInterface As Made4Net.WebControls.TableEditor
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim UserId As String = "SYSTEM"
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)
        Select Case CommandName.ToLower
            Case "createnewreport"
                'CreateReportEntry(dr("ReportID"), dr("ReportName"), dr("ReportType"), dr("ReportFormDT"))
                CopyFromDefaultTemplate(dr("ReportID"), dr("ReportName"), dr("ReportType"), dr("ReportFormDT"))
            Case "createdataprovider"
                CreateDataProvider(dr("DATASETID"), dr("SQLTEXT"), dr("TABLENAME"))
        End Select
    End Sub

#End Region

#Region "Methods"

    Public Sub CreateDataProvider(ByVal DataSetID As String, ByVal SQLText As String, ByVal TableName As String)
        Dim SYS_REPORT_DATAPROVIDER_SQL As String = "INSERT INTO sys_dataprovider(DATASETID, SQLTEXT, TABLENAME) VALUES ('" & DataSetID & "','" & SQLText & "','" & TableName & "')"
        DataInterface.RunSQL(SYS_REPORT_DATAPROVIDER_SQL)
    End Sub

    Public Sub CreateReportEntry(ByVal ReportID As String, ByVal ReportName As String, ByVal ReportType As String, ByVal ReportDT As String)
        Dim SYS_REPORT_SQL As String = "INSERT INTO sys_reports(ReportID, ReportName, ReportType, ReportFormDT) VALUES ('" & ReportID & "','" & ReportName & "','" & ReportType & "','" & ReportDT & "')"
        DataInterface.RunSQL(SYS_REPORT_SQL)

        Dim SYS_REPORT_PARAMS_SQL As String
        Dim SYS_REPORT_DATAPROVIDERINTERFACE_SQL As String

        Select Case ReportType
            Case "SharpShooterPS"
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','Copies','1')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','DataSetName','')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','DefaultLang','0')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','DefaultReportSavedFormat','PDF')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','LTRTemplateName','')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportClass','Made4Net.Reporting.SharpShooterPS.SharpShooterPSReport')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportDLL','Made4Net.Reporting.SharpShooterPS.dll')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportPath','')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportSavePath','')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','RTLTemplateName','')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','TemplatePath','')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
        End Select

        SYS_REPORT_DATAPROVIDERINTERFACE_SQL = "INSERT INTO SYS_REPORTDATAPROVIDERINTERFACE(REPORTID, DATASETID) VALUES('" & ReportID & "','')"
        DataInterface.RunSQL(SYS_REPORT_DATAPROVIDERINTERFACE_SQL)
    End Sub

    Public Sub CopyFromDefaultTemplate(ByVal ReportID As String, ByVal ReportName As String, ByVal ReportType As String, ByVal ReportDT As String)
        Dim SYS_REPORT_SQL As String = "INSERT INTO sys_reports(ReportID, ReportName, ReportType, ReportFormDT) VALUES ('" & ReportID & "','" & ReportName & "','" & ReportType & "','" & ReportDT & "')"
        DataInterface.RunSQL(SYS_REPORT_SQL)

        Dim SYS_REPORT_PARAMS_SQL As String
        Dim SYS_REPORT_DATAPROVIDERINTERFACE_SQL As String

        Dim DefaultReportData As New DataTable
        DataInterface.FillDataset("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = 'DEFAULTREPORT'", DefaultReportData)

        If DefaultReportData.Rows.Count = 0 Then
            Select Case ReportType
                Case "SharpShooterPS"
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','Copies','1')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','DataSetName','')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','DefaultLang','0')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','DefaultReportSavedFormat','PDF')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','LTRTemplateName','')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportClass','Made4Net.Reporting.SharpShooterPS.SharpShooterPSReport')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportDLL','Made4Net.Reporting.SharpShooterPS.dll')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportPath','')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','ReportSavePath','')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','RTLTemplateName','')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
                    SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','TemplatePath','')"
                    DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
            End Select
        Else
            For Each param As DataRow In DefaultReportData.Rows
                SYS_REPORT_PARAMS_SQL = "INSERT INTO SYS_REPORTPARAMS(ReportID, ParamName, ParamValue) VALUES('" & ReportID & "','" & param("ParamName") & "','" & param("ParamValue") & "')"
                DataInterface.RunSQL(SYS_REPORT_PARAMS_SQL)
            Next
        End If

        SYS_REPORT_DATAPROVIDERINTERFACE_SQL = "INSERT INTO SYS_REPORTDATAPROVIDERINTERFACE(REPORTID, DATASETID) VALUES('" & ReportID & "','')"
        DataInterface.RunSQL(SYS_REPORT_DATAPROVIDERINTERFACE_SQL)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEReport_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReport.CreatedChildControls
        With TEReport
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ReportSetup"
                If TEReport.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "createnewreport"
                End If
            End With
        End With
    End Sub

    Private Sub TEDataProvider_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDataProvider.CreatedChildControls
        With TEDataProvider
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ReportSetup"
                If TEDataProvider.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "createdataprovider"
                End If
            End With
        End With
    End Sub

    Private Sub RunSQL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunSQL.Click
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(SQLText.Text, dt)
        ResDataGrid.DataSource = dt
        ResDataGrid.DataBind()
    End Sub
End Class
