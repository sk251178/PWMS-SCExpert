Imports Made4Net.WebControls
Imports Made4Net.Shared
Imports System.Globalization
Imports Made4Net.DataAccess

Public Class SchedulingApplications
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TESchedulerApplications As Made4Net.WebControls.TableEditor
    Protected WithEvents TESchedulerApplicationSchedule As Made4Net.WebControls.TableEditor
    Protected WithEvents SheduleTrigger As Made4Net.WebControls.ShedulerForm
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TESchedulerApplicationArguments As Made4Net.WebControls.TableEditor
    Protected WithEvents TESchedulerTriggers As Made4Net.WebControls.TableEditor
    Protected WithEvents TESchedulerApplicationsToTriggers As Made4Net.WebControls.TableEditor
    Protected WithEvents TESysLogging As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector1 As Made4Net.WebControls.DataConnector
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

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "Run" Then
            For Each dr In ds.Tables(0).Rows
                Dim msgSender As New Made4Net.Shared.QMsgSender()
                Dim appId As String = dr("ApplicationId")
                Dim schId As String = dr("ScheduleId")
                msgSender.Add("ApplicationId", appId)
                msgSender.Add("ScheduleId", schId)
                msgSender.Send("Scheduler", "Execute " & appId)
            Next
        End If
        'Start RWMS-1754
        If CommandName = "newSchedule" Then
            dr = ds.Tables(0).Rows(0)
            Dim objSchdularApplication As New WMS.Logic.SchedExecuter
            objSchdularApplication.CreateSchedule(dr("ScheduleId"), dr("ApplicationId"), dr("DBConnectionName"))
        End If
        If CommandName = "editSchedule" Then
            dr = ds.Tables(0).Rows(0)
            Dim objSchdularApplication As New WMS.Logic.SchedExecuter(dr("ScheduleId"))
            objSchdularApplication.UpdateSchedule(dr("ApplicationId"), dr("DBConnectionName"))
        End If
        'End RWMS-1754
    End Sub

#End Region

    Public Sub TESchedulerApplicationSchedule_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESchedulerApplicationSchedule.CreatedChildControls
        With TESchedulerApplicationSchedule
            With .ActionBar
                .ID = ActionBarID
                .AddSpacer()
                .AddExecButton("Run", "Run", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarExecuteScheduler"))
                With .Button("Run")
                    .ObjectDLL = ExecDLL
                    .ObjectName = ExecClassName
                    .CommandName = "Run"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "The selected applications will be executed.Would you like to continue ?"
                End With
                'Start RWMS-1754
                With .Button("Save")
                    .ObjectDLL = ExecDLL
                    .ObjectName = ExecClassName
                    If TESchedulerApplicationSchedule.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .CommandName = "newSchedule"
                    ElseIf TESchedulerApplicationSchedule.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .CommandName = "editSchedule"
                    End If
                End With
                'End Start RWMS-1754
            End With
        End With
    End Sub

    'Encrption of password coming from user so it will be saved to database in 
    'its encrypted form 
    'Private Sub TESchedulerApplications_BeforeItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TESchedulerApplications.BeforeItemCommand

    '    If String.Compare(e.CommandName, SaveCommandName, True) = 0 Then
    '        Dim teActionBar As Made4Net.WebControls.TableEditorActionBar = CType(sender, Made4Net.WebControls.TableEditorActionBar)
    '        Dim tableEditor As Made4Net.WebControls.TableEditor = CType(teActionBar.BindingContainer, Made4Net.WebControls.TableEditor)

    '        'Encrypt the password
    '        If Not tableEditor.RecordEditor.FieldValues(Password) Is Nothing AndAlso _
    '           Not tableEditor.RecordEditor.FieldValues(Password) Is DBNull.Value Then
    '            Dim currentPwd As String = CStr(tableEditor.RecordEditor.FieldValues(Password))
    '            Dim encryptedPassword As String = Made4Net.Shared.Encryption.Cryptor.EncryptBase64(currentPwd)
    '            tableEditor.RecordEditor.FieldValuesChanged(Password) = encryptedPassword
    '        End If
    '    End If
    'End Sub

    Private Sub TESysLogging_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESysLogging.CreatedGrid
        AddHandler TESysLogging.Grid.DataCreated, AddressOf TESysLoggingDataCreated
    End Sub

    'Filtering the logs for the job so it will show the log of the last run 
    Sub TESysLoggingDataCreated(ByVal source As Object, ByVal e As Made4Net.WebControls.DataGridDataCreatedEventArgs)

        If e.Data.Rows.Count > 0 Then
            e.Data.DefaultView.Sort = TimeColumnName + " DESC"
            Dim sortedTable As DataTable = e.Data.DefaultView.Table.Clone()
            If e.Data.DefaultView.Count > 0 Then
                Dim drv As DataRowView = e.Data.DefaultView(0)
                sortedTable.ImportRow(drv.Row)
                e.Data = sortedTable
            End If
        End If

    End Sub
    Private Sub FillControl(ByVal ApplicationID As String, ByVal ScheduleID As String, ByVal ConnectionName As String)
        SheduleTrigger.ApplicationConnection = ConnectionName
        SheduleTrigger.ApplicationToRun = ApplicationID
        SheduleTrigger.ScheduleID = ScheduleID
        'Dim sap As SchedulerApplicationParam = New SchedulerApplicationParam
        'sap.Argument = Session("TEMPLATE")
        'sap.ArgumentOrdinal = 1
        'sap.ArgumentSystemType = "System.String"
        'Dim arr As New ArrayList
        'arr.Add(sap)

        'SheduleTrigger.SetApplicationParams(arr)
        SheduleTrigger.ReLoad()
        'SheduleTrigger.refresh()

    End Sub

    Private Sub TESchedulerApplicationSchedule_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TESchedulerApplicationSchedule.RecordSelected
        Dim tds As DataTable = TESchedulerApplicationSchedule.CreateDataTableForSelectedRecord()
        'Session("TEMPLATE") = tds.Rows(0)("TEMPLATENAME")
        Dim AppID As String = tds.Rows(0)("APPLICATIONID")
        Dim SchID As String = tds.Rows(0)("SCHEDULEID")
        Dim Conn As String = tds.Rows(0)("DBCONNECTIONNAME")
        FillControl(AppID, SchID, Conn)
    End Sub
   
End Class
