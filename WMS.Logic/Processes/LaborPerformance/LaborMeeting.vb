Option Explicit On

Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports M4NUtil = Made4Net.Shared.Util
Imports M4NConvert = Made4Net.Shared.Conversion.Convert
Imports System.Diagnostics
Imports System.Collections.Generic
Imports System.Text
Imports Made4Net.General.Helpers

Public Class LaborMeeting

    Private Const INVALID_ID = "INVALIDMEETINGID"

    Public Class LaborMeetingInterfaces
        Private logger_ As ILogHandler
        Public ReadOnly Property Logger() As ILogHandler
            Get
                Return logger_
            End Get
        End Property
        Private util_ As IUtil
        Public ReadOnly Property UtilImpl() As IUtil
            Get
                Return util_
            End Get
        End Property

        Public Shared Function DefaultLaborMeetignInterfaces() As LaborMeetingInterfaces
            Dim interfaces As LaborMeetingInterfaces = New LaborMeetingInterfaces
            interfaces.logger_ = Nothing
            interfaces.util_ = New DefaultUtilImplementation()
        End Function
        Public Sub New()

        End Sub
        Public Sub New(logger As ILogHandler, util As IUtil)
            logger_ = logger
            util_ = If(util Is Nothing, New DefaultUtilImplementation(), util)
        End Sub
    End Class
    Public Class ConflictedMeetingCollection
        Private meeting_ As LaborMeeting
        Protected collection_ As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))
        Public ReadOnly Property ConflictedUsers() As IEnumerable(Of String)
            Get
                Dim resultSet As HashSet(Of String) = New HashSet(Of String)(StringComparer.InvariantCultureIgnoreCase)
                For Each kvp As KeyValuePair(Of String, List(Of String)) In collection_
                    resultSet.UnionWith(kvp.Value)
                Next
                Return resultSet
            End Get
        End Property
        Public ReadOnly Property MeetingsInConflict() As IEnumerable(Of String)
            Get
                Return collection_.Keys
            End Get
        End Property
        Private nonConflictedUsers_ As HashSet(Of String) = New HashSet(Of String)(StringComparer.InvariantCultureIgnoreCase)
        Public ReadOnly Property NonConflictedUsers() As IEnumerable(Of String)
            Get
                Return nonConflictedUsers_
            End Get
        End Property
        Private Sub AddConflictingMeeting(meetingId As String, users As IEnumerable(Of String))
            If Not collection_.ContainsKey(meetingId) Then
                collection_.Add(meetingId, Nothing)
            End If
            collection_(meetingId) = users.ToList()
        End Sub

        Public Sub ToLog()
            If meeting_.Logger IsNot Nothing Then
                Dim log As ILogHandler = meeting_.Logger
                log.Write("Conflicted Meeting Collection")
                log.Write("{")
                log.Write("    NonConflictedUsers: {0}", String.Join(", ", NonConflictedUsers))
                For Each kvp As KeyValuePair(Of String, List(Of String)) In collection_
                    log.Write("    Conflicts in {0}: {1}", kvp.Key, String.Join(", ", kvp.Value))
                Next
                log.Write("}")
            End If
        End Sub
        ' Example Timelines of Meeetings
        ' ------------------------------
        '       [-----M1----]
        ' [----M2---]
        '              [---M3----]
        '   [--------M4--------]
        '            [--M5-]
        ' Each meeting may have up to 4 types of conflicts.
        ' M1 conflicts with all meetings.
        ' M2 conflicts with M1, M4, M5
        ' M3 conflicts with M1, M4, M5
        ' M4 conflicts with all meetings
        ' M5 conflicts with M1, M3, M4
        Public Shared Function GetConflictingMeetings(meeting As LaborMeeting) As IEnumerable(Of String)
            Dim meetings As List(Of String) = New List(Of String)
            If meeting.startTime_.HasValue Then
                Dim endDateTime As DateTime = meeting.startTime_.Value.Add(meeting.Duration)

                Dim query As String = String.Format(
                    "select MEETINGID from vLaborMeetings " &
                    "where MEETINGID <> {0} " &
                    "and ENDDATE > {1} and STARTDATE < {2}",
                    FormatField(meeting.MeetingID),
                    FormatField(meeting.StartTime),
                    FormatField(endDateTime))

                Dim dt As New DataTable
                DataInterface.FillDataset(query, dt)
                For Each row As datarow In dt.rows
                    meetings.Add(CType(row.ItemOrDefault("MEETINGID"), String))
                Next
            End If
            Return meetings
        End Function
        Public Shared Function GetConflictingMeetingsForUsers(meeting As LaborMeeting,
                                                      usersWeWantToAdd As IEnumerable(Of String)) As ConflictedMeetingCollection
            Dim conflictingMeetings As New ConflictedMeetingCollection
            Dim logger As ILogHandler = meeting.Logger

            conflictingMeetings.meeting_ = meeting
            conflictingMeetings.nonConflictedUsers_ = New HashSet(Of String)(usersWeWantToAdd, StringComparer.InvariantCultureIgnoreCase)

            For Each meetingId As String In GetConflictingMeetings(meeting)
                Dim usersInMeeting As HashSet(Of String) = meeting.GetUsersForMeeting(meetingId)

                Dim conflictedUsers As String() = usersInMeeting.Intersect(usersWeWantToAdd).ToArray()
                If conflictedUsers.Any Then
                    conflictingMeetings.AddConflictingMeeting(meetingId, conflictedUsers)
                    conflictingMeetings.nonConflictedUsers_.ExceptWith(conflictedUsers)
                End If
            Next
            Return conflictingMeetings
        End Function
    End Class
    Public Class AddUserResult
        Public Enum ResultType
            Successful
            SomeUsersRejected
            Unsuccessful
            NoActionTaken
        End Enum
#Region "Properties"
        Private result_ As ResultType = ResultType.NoActionTaken
        Public Property Result() As ResultType
            Get
                Return result_
            End Get
            Set(ByVal value As ResultType)
                result_ = value
            End Set
        End Property

        Private usersAdded_ As List(Of String) = New List(Of String)
        Public Property UsersAdded() As List(Of String)
            Get
                Return usersAdded_
            End Get
            Set(ByVal value As List(Of String))
                usersAdded_ = value
            End Set
        End Property

        Private conflictedMeetings_ As ConflictedMeetingCollection
        Public Property ConflictedMeetings() As ConflictedMeetingCollection
            Get
                Return conflictedMeetings_
            End Get
            Set(value As ConflictedMeetingCollection)
                conflictedMeetings_ = value
            End Set
        End Property
#End Region
    End Class

#Region "Fields"
    Private ReadOnly interfaces_ As LaborMeetingInterfaces
    Private ReadOnly validationAction_ As Action(Of String)
#End Region

#Region "Properties"
    Public ReadOnly Property Logger() As ILogHandler
        Get
            Return interfaces_.Logger
        End Get
    End Property
    Public ReadOnly Property UtilImpl() As IUtil
        Get
            Return interfaces_.UtilImpl
        End Get
    End Property
    Private meetingID_ As String
    Public Property MeetingID() As String
        Get
            Return meetingID_
        End Get
        Set(ByVal value As String)
            meetingID_ = value
        End Set
    End Property
    Private description_ As String
    Public Property Description() As String
        Get
            Return description_
        End Get
        Set(ByVal value As String)
            description_ = value
        End Set
    End Property
    Private startTime_ As DateTime?
    Public Property StartTime() As DateTime?
        Get
            Return startTime_
        End Get
        Set(ByVal value As DateTime?)
            startTime_ = value
        End Set
    End Property
    Private duration_ As TimeSpan?
    Public Property Duration() As TimeSpan?
        Get
            Return duration_
        End Get
        Set(ByVal value As TimeSpan?)
            duration_ = value
        End Set
    End Property
    Private userIDs_ As HashSet(Of String) = New HashSet(Of String)(StringComparer.InvariantCultureIgnoreCase)
    Public ReadOnly Property UserIDs() As IEnumerable(Of String)
        Get
            Return userIDs_
        End Get
    End Property

#End Region

    Protected Sub New(interfaces As LaborMeetingInterfaces,
                      meetingDataRow As DataRow, validationAction As Action(Of String))
        interfaces_ = If(interfaces Is Nothing, LaborMeetingInterfaces.DefaultLaborMeetignInterfaces(), interfaces)
        validationAction_ = validationAction

        Debug.Assert(meetingDataRow IsNot Nothing, "dr parameter must be non null")

        meetingID_ = meetingDataRow.ItemOrDefault("MEETINGID", INVALID_ID)
        description_ = meetingDataRow.ItemOrDefault("DESCRIPTION")
        startTime_ = meetingDataRow.ItemOrDefault("STARTDATE")
        duration_ = M4NConvert.HHMMSSToTimeSpan(meetingDataRow.ItemOrDefault("DURATION"))

        ' Load the users
        If Exists() Then
            userIDs_ = GetUsersForMeeting(meetingID_)
        End If

    End Sub

    Public Shared Function FromDataRow(interfaces As LaborMeetingInterfaces, dr As DataRow,
                                       validationAction As Action(Of String)) As LaborMeeting
        Return New LaborMeeting(interfaces, dr, validationAction)
    End Function

    Public Function IsValid() As Boolean

        ' Duration must not be null
        If Not Duration.HasValue Then
            InvokeValidationError("Duration must not be empty.")
            Return False
        End If

        ' Duration must be less than 8 hours
        If Duration.Value.TotalSeconds >= 28800 Then
            InvokeValidationError("Duration must be less than 8 hours.")
            Return False
        End If

        ' Duration must not be 0
        If Duration.Value.TotalSeconds = 0 Then
            InvokeValidationError("Duration must be greater than 0.")
            Return False
        End If

        ' Start time must not be null
        If Not StartTime.HasValue Then
            InvokeValidationError("Meeting Date and Time is in an invalid format.")
            Return False
        End If

        ' Start time must be prior to now
        If StartTime.Value > DateTime.Now Then
            InvokeValidationError("Meeting Date and Time must be in the past.")
            Return False
        End If

        ' Description must not be empty
        If String.IsNullOrWhiteSpace(Description) Then
            InvokeValidationError("Description must not be empty.")
            Return False
        End If

        Return True
    End Function

    Protected Sub InvokeValidationError(msg As String)
        If validationAction_ IsNot Nothing Then
            validationAction_.Invoke(msg)
        End If
    End Sub

    Protected Sub InvokeValidationError(fmt As String, ParamArray args As Object())
        InvokeValidationError(String.Format(fmt, args))
    End Sub

    Protected Function GetUsersForMeeting(meetID As String) As HashSet(Of String)
        Dim userIDs As New HashSet(Of String)(StringComparer.InvariantCultureIgnoreCase)
        Dim usersDataTable As DataTable = New DataTable
        Dim sql As String = String.Format("SELECT USERID FROM LABORUSERMEETINGS " &
                            "WHERE MEETINGID = {0}",
                            FormatField(meetID))

        DataInterface.FillDataset(sql, usersDataTable)

        For Each usersDataRow As DataRow In usersDataTable.Rows
            userIDs.Add(CType(usersDataRow.ItemOrDefault("USERID"), String))
        Next

        Return userIDs
    End Function

    Public Function GetMeetingsInConflict() As ConflictedMeetingCollection
        Return ConflictedMeetingCollection.GetConflictingMeetingsForUsers(Me, Me.UserIDs)
    End Function

    Public Function AddUsers(addUserList As IEnumerable(Of String)) As AddUserResult
        Dim result As AddUserResult = New AddUserResult()
        Logger.SafeWrite("LaborMeeting.AddUsers: ({0})", String.Join(", ", addUserList))

        If addUserList.Any() Then
            Logger.SafeWrite("Attempting to add users to meeting {0}: {1}",
                              MeetingID, String.Join(", ", addUserList))

            Dim usersThatWeWantToAdd As HashSet(Of String) = New HashSet(Of String)(addUserList, StringComparer.InvariantCultureIgnoreCase)

            ' Now, we can exclude any current users from the list of users we want to add
            ' (Note: This shouldn't happen - it's just a sanity check.)
            usersThatWeWantToAdd.ExceptWith(userIDs_)

            Dim conflictedMeetings As ConflictedMeetingCollection =
                ConflictedMeetingCollection.GetConflictingMeetingsForUsers(Me, usersThatWeWantToAdd)

            result.ConflictedMeetings = conflictedMeetings

            ' If there are any users we want to add in conflict, then our return value must be partial.
            If (conflictedMeetings.ConflictedUsers.Any()) Then
                result.Result = AddUserResult.ResultType.SomeUsersRejected
                Logger.SafeWrite("AddUsers: Some users were rejected")
                conflictedMeetings.ToLog()
            End If

            If conflictedMeetings.NonConflictedUsers.Any() Then
                DataInterface.BeginTransaction()
                Try
                    For Each usr As String In conflictedMeetings.NonConflictedUsers
                        Dim sql As String = String.Format("insert into LABORUSERMEETINGS (MEETINGID, USERID) VALUES ({0}, {1})",
                                              FormatField(MeetingID),
                                              FormatField(usr))

                        DataInterface.RunSQL(sql)
                    Next
                    DataInterface.CommitTransaction()
                    userIDs_.UnionWith(conflictedMeetings.NonConflictedUsers)
                    If result.Result <> AddUserResult.ResultType.SomeUsersRejected Then
                        result.Result = AddUserResult.ResultType.Successful
                    End If
                Catch ex As Exception
                    result.Result = AddUserResult.ResultType.Unsuccessful
                    Logger.SafeWrite("Exception during AddUsers {0}", ex.ToString())
                    DataInterface.RollbackTransaction()
                    Throw ex
                End Try
            End If
        End If
        Return result
    End Function

    Public Function RemoveUsers(delUsers As IEnumerable(Of String)) As Boolean
        Dim success As Boolean = False
        Logger.SafeWrite("LaborMeeting.RemoveUsers: ({0})", String.Join(", ", delUsers))
        If delUsers.Any() Then
            Dim delUserSet As HashSet(Of String) = New HashSet(Of String)(delUsers, StringComparer.InvariantCultureIgnoreCase)
            Dim nextUserIDs As HashSet(Of String) = New HashSet(Of String)(userIDs_, StringComparer.InvariantCultureIgnoreCase)
            delUserSet.IntersectWith(userIDs_)

            If delUserSet.Any() Then
                DataInterface.BeginTransaction()
                Try
                    For Each usr As String In delUserSet
                        nextUserIDs.Remove(usr)
                        Dim sql As String = String.Format("delete from LABORUSERMEETINGS WHERE MEETINGID={0} AND USERID={1}",
                                              FormatField(MeetingID),
                                              FormatField(usr))

                        Logger.SafeWrite("RemoveUsers: {0}", sql)
                        DataInterface.RunSQL(sql)
                    Next
                    DataInterface.CommitTransaction()
                    userIDs_ = nextUserIDs
                    success = True
                Catch ex As Exception
                    Logger.SafeWrite("Exception during AddUsers {0}", ex.ToString())
                    DataInterface.RollbackTransaction()
                    Throw ex
                End Try
            End If
        End If
        Return success
    End Function

    Public Sub Delete()
        If Exists() Then
            Try
                DataInterface.BeginTransaction()
                DataInterface.RunSQL("delete from LABORUSERMEETINGS where MEETINGID = {0}",
                                      FormatField(MeetingID))
                DataInterface.RunSQL("delete from LABORMEETINGS where MEETINGID = {0}",
                                      FormatField(MeetingID))
                DataInterface.CommitTransaction()
            Catch ex As Exception
                Logger.SafeWrite("Exception during AddUsers {0}", ex.ToString())
                DataInterface.RollbackTransaction()
                Throw ex
            End Try
        End If
    End Sub
    Public Sub Update()
        If AssertExists() AndAlso IsValid() Then

            ' Check to see if any conflicts are created from this update...
            Dim conflictedMeetings As ConflictedMeetingCollection =
                ConflictedMeetingCollection.GetConflictingMeetingsForUsers(Me, Me.UserIDs)

            If conflictedMeetings.ConflictedUsers.Any() Then
                Logger.SafeWrite("Update unsuccessful. The change in meeting information would result in users in conflicting meetings.")
                conflictedMeetings.ToLog()
                Throw New Exception(String.Format(
                                    "Update rejected because changes to the meeting would assign users to conflicting meetings: {0}",
                                    String.Join(", ", conflictedMeetings.MeetingsInConflict)))
            End If
            Dim query As String() = {
                "UPDATE LABORMEETINGS SET",
                "DESCRIPTION = ", FormatField(Description),
                ", STARTDATE = ", FormatField(StartTime.Value),
                ", DURATION = ", FormatField(Duration.Value.ToHHMMSS()),
                ", EDITUSER = ", FormatField(WMS.Logic.GetCurrentUser),
                ", EDITDATE = ", FormatField(DateTime.Now),
                "WHERE MEETINGID = ", FormatField(MeetingID)
            }
            Dim sql = String.Join(" ", query)

            Logger.SafeWrite("LaborMeeting.Save - sql: {0}", sql)
            DataInterface.RunSQL(sql)
        End If
    End Sub

    Public Sub Insert()
        If IsValid() Then
            MeetingID = UtilImpl.getNextCounter("MEETINGID")

            Dim sql = String.Format("INSERT INTO LABORMEETINGS " &
                                    "(MEETINGID, DESCRIPTION, STARTDATE, DURATION, EDITUSER, EDITDATE, ADDUSER, ADDDATE) " &
                                    "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                                    FormatField(MeetingID),
                                    FormatField(Description),
                                    FormatField(StartTime.Value),
                                    FormatField(Duration.Value.ToHHMMSS()),
                                    FormatField(WMS.Logic.GetCurrentUser),
                                    FormatField(Now),
                                    FormatField(WMS.Logic.GetCurrentUser),
                                    FormatField(Now))

            Logger.SafeWrite("LaborMeeting.Save - sql: {0}", sql)
            DataInterface.RunSQL(sql)
        End If
    End Sub

    Protected Function Exists() As Boolean
        If MeetingID = INVALID_ID Then
            Return False
        End If

        Return Convert.ToBoolean(DataInterface.ExecuteScalar($"Select count(1) from LABORMEETINGS where MEETINGID = '{MeetingID}'"))
    End Function

    Protected Function AssertExists() As Boolean
        Dim recordExists = Exists()
        If Not recordExists Then
            InvokeValidationError("A record in the LABORMEETINGS table with a MEETINGID of {0} was exected to exist, but did not.", MeetingID)
        End If
        Return recordExists
    End Function
End Class