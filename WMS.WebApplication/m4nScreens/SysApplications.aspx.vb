Imports Made4Net.WebControls
Imports Made4Net.Shared
Imports System.Globalization
Imports Made4Net.DataAccess

Public Class SysApplications
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TESchedulerApplications As Made4Net.WebControls.TableEditor
    
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    
    

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Const ActionBarID As String = "TESchedulerApplications_ActionBar"
    Private Const ActionBarCommandName As String = "Run"
    Private Const ExecDLL As String = "WMS.WebApp.dll"
    Private Const ExecClassName As String = "WMS.WebApp.SchedulingApplications"
    Private Const Password As String = "Password"
    Private Const SaveCommandName As String = "Save"
    Private Const TimeColumnName As String = "LogTimeStamp"
    Private Const SysLoggingTabIndex As Int16 = 2

#Region "Ctors"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Public Sub New() 'Empty default Ctor
    End Sub

    

#End Region

    

    

    'Filtering the logs for the job so it will show the log of the last run 
   

   

End Class
