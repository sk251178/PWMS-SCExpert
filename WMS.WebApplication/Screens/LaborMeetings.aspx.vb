Imports Made4Net.Shared
Imports M4NUtil = Made4Net.Shared.Util
Imports Made4Net.DataAccess
Imports Made4Net.WebControls
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Linq
Imports Made4Net.DataAccess.Collections
Imports WMS.Logic
Imports Made4Net.General.Helpers
Imports System.Collections.Generic

Module ToLogExtensions
    <Extension()>
    Public Function ToLogString(ByVal o As Object) As String
        If o Is Nothing Then
            Return "NULL"
        End If
        Return o.ToString()
    End Function

    <Extension()>
    Public Function ToLogString(ByVal o As RecordUpdateType) As String
        Return o.ToString()
    End Function

    <Extension()>
    Public Function ToLogString(ByVal o As TableEditorMultiSelectActionEventArgs) As String
        If o IsNot Nothing AndAlso o.DataTable IsNot Nothing Then
            Return o.DataTable.ToLogString()
        End If
        Return String.Empty
    End Function

    <Extension()>
    Public Function ToLogString(ByVal o As GenericCollection) As String
        If o Is Nothing Then
            Return "NULL"
        End If
        Dim sb As New StringBuilder()
        For Each k As Object In o.Keys
            If sb.Length > 0 Then
                sb.Append(", ")
            End If
            sb.Append(k.ToString())
            sb.Append("=")
            sb.Append(o.Item(k).ToString())
        Next
        Return sb.ToString()
    End Function

    <Extension()>
    Public Function ToLogString(ByVal ea As DataGridCommandEventArgs2) As String
        If ea Is Nothing Then
            Return "NULL"
        End If
        Dim sb As New StringBuilder()
        sb.Append("CommandArgument=")
        sb.Append(ea.CommandArgument.ToLogString())
        sb.Append(", CommandName=")
        sb.Append(ea.CommandName)
        sb.Append(", CommandSource=")
        sb.Append(ea.CommandSource)
        Return sb.ToString()
    End Function

    <Extension()>
    Public Function ToLogString(ByRef ea As TableEditorEventArgs) As String
        If ea Is Nothing Then
            Return "NULL"
        End If
        Dim sb As New StringBuilder()
        sb.Append("UpdateType=")
        sb.Append(ea.UpdateType.ToLogString())
        sb.Append(", Fields.Keys={")
        sb.Append(ea.Fields.ToLogString())
        sb.Append("}")
        Return sb.ToString()
    End Function

    <Extension()>
    Public Function ToLogString(ByRef ea As StandardSuccessEventArgs) As String
        If ea Is Nothing Then
            Return "NULL"
        End If
        Dim sb As New StringBuilder()
        sb.Append("Argument=")
        sb.Append(ea.Argument)
        sb.Append(", CommandName=")
        sb.Append(ea.CommandName)
        sb.Append(", Message=")
        sb.Append(ea.Message)
        Return sb.ToString()
    End Function
End Module

<CLSCompliant(False)>
Public Class LaborMeetings
    Inherits System.Web.UI.Page

    Public Class Commands
        Public Const InsertMeeting As String = "insertmeeting"
        Public Const UpdateMeeting As String = "updatemeeting"
        Public Const DeleteMeeting As String = "deletemeeting"
        Public Const AddUserToMeeting As String = "addusertomeeting"
        Public Const RemoveUserFromMeeting As String = "removeuserfrommeeting"
    End Class
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents wmsScreen As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TE_MEETINGS As TableEditor

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
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, wmsScreen.ScreenID)
    End Sub

#End Region

    Public Property SessionLogger As ILogHandler
        Get
            Dim val As ILogHandler = HttpContext.Current.Session("WMSLogger")
            Return val
        End Get
        Set(value As ILogHandler)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove("WMSLogger")
            Else
                HttpContext.Current.Session("WMSLogger") = value
            End If
        End Set
    End Property
    Public Property SessionMeeting() As LaborMeeting
        Get
            Return HttpContext.Current.Session("CurrentMeeting")
        End Get
        Set(ByVal value As LaborMeeting)
            If value Is Nothing Then
                HttpContext.Current.Session.Remove("CurrentMeeting")
            Else
                HttpContext.Current.Session("CurrentMeeting") = value
            End If
        End Set
    End Property

    Protected Function LaborMeetingFromDataRow(dr As DataRow) As LaborMeeting
        Dim translator As New Translation.Translator(Translation.Translator.CurrentLanguageID)
        Dim validationAction As Action(Of String) = Sub(err As String) Throw New ApplicationException(translator.Translate(err))
        Dim interfaces As LaborMeeting.LaborMeetingInterfaces = New LaborMeeting.LaborMeetingInterfaces(
                                                                SessionLogger,
                                                                New DefaultUtilImplementation())

        Return LaborMeeting.FromDataRow(interfaces, dr, validationAction)
    End Function

    Public Sub New()

    End Sub
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String,
                   ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        SessionLogger.SafeWrite("CommandName = {0}", CommandName)

        Dim ds As DataSet = M4NUtil.XmlToDS(XMLSchema, XMLData)

        If SessionLogger IsNot Nothing Then
            SessionLogger.SafeWrite("Inbound Data:")
            ds.Tables(0).ToLog(AddressOf SessionLogger.Write, AddressOf SessionLogger.writeSeperator)
        End If

        Select Case CommandName
            Case Commands.InsertMeeting
                LaborMeetingFromDataRow(ds.Tables(0).Rows(0)).Insert()
            Case Commands.UpdateMeeting
                LaborMeetingFromDataRow(ds.Tables(0).Rows(0)).Update()
            Case Commands.DeleteMeeting
                LaborMeetingFromDataRow(ds.Tables(0).Rows(0)).Delete()
                SessionMeeting = Nothing
            Case Commands.AddUserToMeeting
                If SessionMeeting IsNot Nothing Then
                    Dim addResult As LaborMeeting.AddUserResult =
                        SessionMeeting.AddUsers(ds.Tables(0).ColumnToStrings("username"))

                    Select Case addResult.Result
                        Case LaborMeeting.AddUserResult.ResultType.SomeUsersRejected
                            Message = String.Format("Added {0} user(s). Rejected {1} user(s) due to meeting conflicts: {2}",
                                                    addResult.ConflictedMeetings.NonConflictedUsers.Count(),
                                                    addResult.ConflictedMeetings.ConflictedUsers.Count(),
                                                    String.Join(",", addResult.ConflictedMeetings.MeetingsInConflict))

                        Case LaborMeeting.AddUserResult.ResultType.Successful
                            Message = String.Format("Added {0} users to the meeting.",
                                                    addResult.ConflictedMeetings.NonConflictedUsers.Count())

                        Case LaborMeeting.AddUserResult.ResultType.Unsuccessful
                            Message = "There was an error adding users to the meeting."
                    End Select
                End If
            Case Commands.RemoveUserFromMeeting
                If SessionMeeting IsNot Nothing Then
                    Message = If(SessionMeeting.RemoveUsers(ds.Tables(0).ColumnToStrings("username")),
                                 "Selected users were removed from the meeting.",
                                 "There was an error removing users.")
                End If
        End Select
    End Sub

    Protected Sub UpdateFilterSelectionForCurrentMeeting()
        If SessionMeeting IsNot Nothing Then
            Dim warehouseFilter As String = String.Format("(WAREHOUSE = {0})", FormatField(WMS.Logic.Warehouse.CurrentWarehouse))
            If SessionMeeting.UserIDs.Any() Then
                Dim userList As String = String.Join(", ",
                                            From user In SessionMeeting.UserIDs
                                            Select FormatField(user))


                TEUsersNotInMeeting.FilterExpression = String.Format("{0} AND (username NOT IN ({1}))", warehouseFilter, userList)
                TEUsersInMeeting.FilterExpression = String.Format("{0} AND (username IN ({1}))", warehouseFilter, userList)
            Else
                TEUsersNotInMeeting.FilterExpression = warehouseFilter
                TEUsersInMeeting.FilterExpression = "(username IS NULL)"
            End If
            SessionLogger.SafeWrite("TEUsersNotInMeeting.FilterExpression={0}", TEUsersNotInMeeting.FilterExpression)
            SessionLogger.SafeWrite("TEUsersInMeeting.FilterExpression={0}", TEUsersInMeeting.FilterExpression)
            TEUsersInMeeting.RefreshData()
            TEUsersNotInMeeting.RefreshData()
        End If
    End Sub

    Protected Sub TE_MEETINGS_RecordSelected(sender As TableEditor, e As TableEditorEventArgs) Handles TE_MEETINGS.RecordSelected
        SessionLogger.SafeWrite("TE_MEETINGS_RecordSelected")
        Dim dt As DataTable = sender.CreateDataTableForSelectedRecord(True, TableEditorMode.Grid)
        SessionMeeting = LaborMeetingFromDataRow(dt.Rows(0))
        SessionLogger.SafeWrite("TE_MEETING_RecordSelected. SessionMeeting.MeetingId={0}", SessionMeeting.MeetingID)
        UpdateFilterSelectionForCurrentMeeting()
    End Sub

    Protected Sub TE_MEETINGS_RecordUnSelected(sender As TableEditor, e As TableEditorEventArgs) Handles TE_MEETINGS.RecordUnSelected
        SessionLogger.SafeWrite("TE_MEETINGS_RecordUnSelected")
        SessionMeeting = Nothing
    End Sub

    Protected Sub TE_MEETINGS_CreatedChildControls(sender As Object, e As EventArgs) Handles TE_MEETINGS.CreatedChildControls
        If TE_MEETINGS.CurrentMode = TableEditorMode.Edit Then
            With TE_MEETINGS.ActionBar.Button("Delete")
                .ObjectDLL = "WMS.WebApp.DLL"
                .ObjectName = "WMS.WebApp.LaborMeetings"
                .CommandName = Commands.DeleteMeeting
                .ConfirmMessage = "Are you sure you want to delete this?"
            End With
        End If
        With TE_MEETINGS.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.DLL"
            .ObjectName = "WMS.WebApp.LaborMeetings"
            If TE_MEETINGS.CurrentMode = TableEditorMode.Insert Then
                .CommandName = Commands.InsertMeeting
            ElseIf TE_MEETINGS.CurrentMode = TableEditorMode.Edit Then
                .CommandName = Commands.UpdateMeeting
            End If
        End With
    End Sub

    Protected Sub TEUsersInMeeting_CreatedChildControls(sender As Object, e As EventArgs) Handles TEUsersInMeeting.CreatedChildControls
        With TEUsersInMeeting.ActionBar
            .AddSpacer()
            .AddExecButton("RemoveUsersFromMeeting", "Remove Users from Meeting",
                           SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("RemoveUsersFromMeeting")
                .ObjectDLL = "WMS.WebApp.DLL"
                .ObjectName = "WMS.WebApp.LaborMeetings"
                .CommandName = Commands.RemoveUserFromMeeting
                .ConfirmMessage = "Do you want to remove the User from the Meeting?"
                .Mode = ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

    Protected Sub TEUsersNotInMeeting_CreatedChildControls(sender As Object, e As EventArgs) Handles TEUsersNotInMeeting.CreatedChildControls
        With TEUsersNotInMeeting.ActionBar
            .AddSpacer()
            .AddExecButton("AddUsersToMeeting", "Add Users to Meeting",
                           SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("AddUsersToMeeting")
                .ObjectDLL = "WMS.WebApp.DLL"
                .ObjectName = "WMS.WebApp.LaborMeetings"
                .CommandName = Commands.AddUserToMeeting
                .Mode = ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

    Protected Sub TEUsersNotInMeeting_AfterItemCommand(sender As Object, e As CommandEventArgs) Handles TEUsersNotInMeeting.AfterItemCommand
        UpdateFilterSelectionForCurrentMeeting()
    End Sub

    Protected Sub TEUsersInMeeting_AfterItemCommand(sender As Object,
                                                    e As CommandEventArgs) Handles TEUsersInMeeting.AfterItemCommand
        UpdateFilterSelectionForCurrentMeeting()
    End Sub
End Class