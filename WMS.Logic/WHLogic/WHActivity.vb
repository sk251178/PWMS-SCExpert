Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports System.Collections.Generic

''' <summary>
''' Labor calc changes RWMS-952 -> PWMS-903
''' </summary>
''' <remarks></remarks>
<CLSCompliant(False)> Public Class WHActivity

#Region "Variables"

    Protected _activityid As String
    Protected _userid As String
    Protected _hetype As String
    Protected _mheid As String
    Protected _activity As String
    Protected _location As String
    Protected _activitytime As DateTime
    Protected _printer As String
    Protected _terminaltype As String
    Protected _warehousearea As String
    Protected _shift As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    ' Labor calc changes RWMS-952 -> PWMS-903
    Protected _previousActivity As String
    Protected _previousTaskEndLoc As String
    ' Labor calc changes RWMS-952 -> PWMS-903

    'Adding the new property for RWMS-1497
    Protected _previousActivityTime As DateTime?

#End Region

#Region "Properties"

    Public Property ACTIVITYID() As String
        Get
            Return _activityid
        End Get
        Set(ByVal Value As String)
            _activityid = Value
        End Set
    End Property

    Public Property USERID() As String
        Get
            Return _userid
        End Get
        Set(ByVal Value As String)
            _userid = Value
        End Set
    End Property

    Public Property HETYPE() As String
        Get
            Return _hetype
        End Get
        Set(ByVal Value As String)
            _hetype = Value
        End Set
    End Property

    Public Property ACTIVITY() As String
        Get
            Return _activity
        End Get
        Set(ByVal Value As String)
            _activity = Value
        End Set
    End Property

    Public Property LOCATION() As String
        Get
            Return _location
        End Get
        Set(ByVal Value As String)
            _location = Value
        End Set
    End Property
    Public Property PRINTER() As String
        Get
            Return _printer
        End Get
        Set(ByVal Value As String)
            _printer = Value
        End Set
    End Property
    Public Property WAREHOUSEAREA() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
        End Set
    End Property

    Public Property HANDLINGEQUIPMENTID() As String
        Get
            Return _mheid
        End Get
        Set(ByVal Value As String)
            _mheid = Value
        End Set
    End Property

    Public Property TERMINALTYPE() As String
        Get
            Return _terminaltype
        End Get
        Set(ByVal Value As String)
            _terminaltype = Value
        End Set
    End Property

    Public Property ACTIVITYTIME() As DateTime
        Get
            Return _activitytime
        End Get
        Set(ByVal Value As DateTime)
            _activitytime = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property
    Public Property SHIFT() As String
        Get
            Return _shift
        End Get
        Set(ByVal Value As String)
            _shift = Value
        End Set
    End Property
    'Labor calc changes RWMS-952 -> PWMS-903
    Public Property PreviousActivity() As String
        Get
            Return _previousActivity
        End Get
        Set(ByVal value As String)
            _previousActivity = value
        End Set
    End Property

    Public Property PreviousTaskEndLocation() As String
        Get
            Return _previousTaskEndLoc
        End Get
        Set(ByVal value As String)
            _previousTaskEndLoc = value
        End Set
    End Property
    ' Labor calc changes RWMS-952 -> PWMS-903

    'Adding the new property for RWMS-1497
    Public Property PREVIOUSACTIVITYTIME() As DateTime?
        Get
            Return _previousActivityTime
        End Get
        Set(ByVal Value As DateTime?)
            _previousActivityTime = Value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New()
        _hetype = ""
    End Sub

    Public Sub New(ByVal pUserId As String)
        _userid = pUserId
        Load()
    End Sub

#End Region


#Region "Private Methods"
    Private Sub Load()
        Dim sql As String = String.Format("SELECT TOP 1 * FROM WHACTIVITY WHERE USERID='{0}' ORDER BY ACTIVITYTIME  DESC", _userid)
        Dim data As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, data)

        If data.Rows.Count = 0 Then Return
        dr = data.Rows(0)
        _activityid = ReplaceDBNull(dr.Item("ACTIVITYID"))
        _hetype = ReplaceDBNull(dr.Item("HETYPE"))
        _mheid = ReplaceDBNull(dr.Item("MHEID"))
        _activity = ReplaceDBNull(dr.Item("ACTIVITY"))
        _location = ReplaceDBNull(dr.Item("LOCATION"))
        _printer = ReplaceDBNull(dr.Item("PRINTER"))
        _warehousearea = ReplaceDBNull(dr.Item("WAREHOUSEAREA"))
        _activitytime = ReplaceDBNull(dr.Item("ACTIVITYTIME"))
        _terminaltype = ReplaceDBNull(dr.Item("TERMINALTYPE"))
        _shift = ReplaceDBNull(dr.Item("SHIFT"))
        _previousActivity = ReplaceDBNull(dr.Item("PREVIOUSACTIVITY"))
        _previousTaskEndLoc = ReplaceDBNull(dr.Item("PREVIOUSTASKENDLOCATION"))
        _previousActivityTime = ReplaceDBNull(dr.Item("PREVIOUSACTIVITYTIME"))
        data.Dispose()
    End Sub
#End Region


#Region "Methods"

    Public Function Post() As String

        Dim SQL As String = ""
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        If _userid = String.Empty Then
            Return String.Empty
        End If

        'Check For User Printer
        If _printer = "" Then
            _printer = DataInterface.ExecuteScalar(String.Format("Select isnull(printer,'') as printer from WHACTIVITY where userid = '{0}'", _userid))
        End If

        'Check For User MHType
        If _hetype = "" Then
            _hetype = DataInterface.ExecuteScalar(String.Format("Select isnull(hetype,'') from WHACTIVITY where userid = '{0}'", _userid))
        End If

        'Check For User MHE ID
        If _mheid = "" Then
            _mheid = DataInterface.ExecuteScalar(String.Format("Select isnull(mheid,'') as mheid from WHACTIVITY where userid = '{0}'", _userid))
        End If

        'Check For User shift
        If _shift = "" Then
            _shift = Common.GetUserShift(_userid, wmsrdtLogger)
        End If

        'Check For terminal type
        If _terminaltype = "" Then
            _terminaltype = DataInterface.ExecuteScalar(String.Format("select isnull(terminaltype,'') as terminaltype from mhe where mheid = '{0}'", _mheid))
        End If


        'Added new column PREVIOUSACTIVITYTIME in both Insert and Update statement.
        If Not Exists(_userid) Then
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format("UserID={0} does not exists in WHACTIVITY Table", Made4Net.Shared.Util.FormatField(_userid)))
            End If
            If _previousActivityTime.HasValue Then
                _activityid = Made4Net.Shared.Util.getNextCounter("WHACTIVITY")

                _activityid = Made4Net.Shared.Util.getNextCounter("WHACTIVITY")
                'Dim oUser As New User(_userid, True)
                ' RWMS-1243
                '_shift = "1" ' Common.GetCurrentUserShiftId 'oUser.SHIFT

                _shift = Common.GetCurrentUserShiftId ''RMWS-1667
                _adddate = DateTime.Now
                _adduser = _userid
                _editdate = DateTime.Now
                _edituser = _userid

                SQL = "INSERT INTO WHACTIVITY (ACTIVITYID , USERID, HETYPE, ACTIVITY, LOCATION, PRINTER,WAREHOUSEAREA, ACTIVITYTIME, MHEID, TERMINALTYPE, ADDDATE, ADDUSER, EDITDATE, EDITUSER,SHIFT, PREVIOUSACTIVITYTIME) VALUES ("
                SQL += Made4Net.Shared.Util.FormatField(_activityid) & "," &
                        Made4Net.Shared.Util.FormatField(_userid) & "," &
                        Made4Net.Shared.Util.FormatField(_hetype) & "," &
                        Made4Net.Shared.Util.FormatField(_activity) & "," &
                        Made4Net.Shared.Util.FormatField(_location) & "," &
                        Made4Net.Shared.Util.FormatField(_printer) & "," &
                        Made4Net.Shared.Util.FormatField(_warehousearea) & "," &
                        Made4Net.Shared.Util.FormatField(_activitytime) & "," &
                        Made4Net.Shared.Util.FormatField(_mheid) & "," &
                        Made4Net.Shared.Util.FormatField(_terminaltype) & "," &
                        Made4Net.Shared.Util.FormatField(_adddate) & "," &
                        Made4Net.Shared.Util.FormatField(_adduser) & "," &
                        Made4Net.Shared.Util.FormatField(_editdate) & "," &
                        Made4Net.Shared.Util.FormatField(_edituser) & "," &
                        Made4Net.Shared.Util.FormatField(_shift) & "," &
                        Made4Net.Shared.Util.FormatField(_previousActivityTime) & ")"
            Else
                _activityid = Made4Net.Shared.Util.getNextCounter("WHACTIVITY")

                _activityid = Made4Net.Shared.Util.getNextCounter("WHACTIVITY")
                'Dim oUser As New User(_userid, True)
                ' RWMS-1243
                ' _shift = "1" ' Common.GetCurrentUserShiftId 'oUser.SHIFT ''RMWS-1667
                _shift = Common.GetCurrentUserShiftId 'oUser.SHIFT  ''RMWS-1667
                _adddate = DateTime.Now
                _adduser = _userid
                _editdate = DateTime.Now
                _edituser = _userid

                SQL = "INSERT INTO WHACTIVITY (ACTIVITYID , USERID, HETYPE, ACTIVITY, LOCATION, PRINTER,WAREHOUSEAREA, ACTIVITYTIME, MHEID, TERMINALTYPE, ADDDATE, ADDUSER, EDITDATE, EDITUSER,SHIFT) VALUES ("
                SQL += Made4Net.Shared.Util.FormatField(_activityid) & "," &
                        Made4Net.Shared.Util.FormatField(_userid) & "," &
                        Made4Net.Shared.Util.FormatField(_hetype) & "," &
                        Made4Net.Shared.Util.FormatField(_activity) & "," &
                        Made4Net.Shared.Util.FormatField(_location) & "," &
                        Made4Net.Shared.Util.FormatField(_printer) & "," &
                        Made4Net.Shared.Util.FormatField(_warehousearea) & "," &
                        Made4Net.Shared.Util.FormatField(_activitytime) & "," &
                        Made4Net.Shared.Util.FormatField(_mheid) & "," &
                        Made4Net.Shared.Util.FormatField(_terminaltype) & "," &
                        Made4Net.Shared.Util.FormatField(_adddate) & "," &
                        Made4Net.Shared.Util.FormatField(_adduser) & "," &
                        Made4Net.Shared.Util.FormatField(_editdate) & "," &
                        Made4Net.Shared.Util.FormatField(_edituser) & "," &
                        Made4Net.Shared.Util.FormatField(_shift) & ")"
            End If

        Else
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format("UserID={0} exists in WHACTIVITY Table", Made4Net.Shared.Util.FormatField(_userid)))
            End If
            If _previousActivityTime.HasValue Then
                SQL = String.Format("UPDATE WHACTIVITY SET HETYPE={0},ACTIVITY={1},LOCATION={2},WAREHOUSEAREA={11},ACTIVITYTIME={3},MHEID={4}, TERMINALTYPE={5}, ADDDATE={6}, ADDUSER={7},EDITDATE={8},EDITUSER={9}, PREVIOUSACTIVITYTIME={12}, PRINTER={13} WHERE USERID = {10}",
                                Made4Net.Shared.Util.FormatField(_hetype), Made4Net.Shared.Util.FormatField(_activity), Made4Net.Shared.Util.FormatField(_location),
                                Made4Net.Shared.Util.FormatField(_activitytime), Made4Net.Shared.Util.FormatField(_mheid), Made4Net.Shared.Util.FormatField(_terminaltype), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser),
                                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_previousActivityTime), Made4Net.Shared.Util.FormatField(_printer))
                Dim SQL2 As String = String.Format("Update userprofile set DEFAULTPRINTER='{0}' where userid='{1}'", Made4Net.Shared.Util.FormatField(_printer), WMS.Logic.Common.GetCurrentUser())
            Else
                SQL = String.Format("UPDATE WHACTIVITY SET HETYPE={0},ACTIVITY={1},LOCATION={2},WAREHOUSEAREA={11},ACTIVITYTIME={3},MHEID={4}, TERMINALTYPE={5}, ADDDATE={6}, ADDUSER={7},EDITDATE={8},EDITUSER={9} WHERE USERID = {10}",
                                Made4Net.Shared.Util.FormatField(_hetype), Made4Net.Shared.Util.FormatField(_activity), Made4Net.Shared.Util.FormatField(_location),
                                Made4Net.Shared.Util.FormatField(_activitytime), Made4Net.Shared.Util.FormatField(_mheid), Made4Net.Shared.Util.FormatField(_terminaltype), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser),
                                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(_warehousearea))

            End If

        End If

        DataInterface.RunSQL(SQL)
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("WHACTIVITY post(): Activity={0}, User={1}. WHACTIVITY Executing Query: {2}", Made4Net.Shared.Util.FormatField(_activity), Made4Net.Shared.Util.FormatField(_userid), SQL))
        End If
        Return _activityid
    End Function

    ' Labor calc changes RWMS-952 -> PWMS-903
    ' User already exists in the WHActivity, read the previous value and save it.
    Public Function SaveLastLocation() As String
        Dim SQL As String = ""
        SQL = String.Format("SELECT Top(1) * FROM WHACTIVITY WHERE USERID='{0}' Order by ACTIVITYTIME desc", _userid)
        Dim data As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, data)
        If data.Rows.Count > 0 Then
            dr = data.Rows(0)

            SQL = String.Format("UPDATE WHACTIVITY SET PREVIOUSACTIVITY={0}, PREVIOUSTASKENDLOCATION={1} WHERE USERID = {2}", _
                            Made4Net.Shared.Util.FormatField(ReplaceDBNull(dr.Item("ACTIVITY"))), Made4Net.Shared.Util.FormatField(ReplaceDBNull(dr.Item("LOCATION"))), Made4Net.Shared.Util.FormatField(_userid))
            data.Dispose()
            DataInterface.RunSQL(SQL)
        End If
    End Function

    Public Function RestorePrevActivityDetails() As String
        Dim SQL As String = ""
        SQL = String.Format("SELECT Top(1) * FROM WHACTIVITY WHERE USERID='{0}' Order by ACTIVITYTIME desc", _userid)
        Dim data As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, data)
        If data.Rows.Count > 0 Then
            dr = data.Rows(0)

            SQL = String.Format("UPDATE WHACTIVITY SET ACTIVITY={0}, LOCATION={1}, ACTIVITYTIME={3} WHERE USERID = {2}", _
                            Made4Net.Shared.Util.FormatField(ReplaceDBNull(dr.Item("PREVIOUSACTIVITY"))), Made4Net.Shared.Util.FormatField(ReplaceDBNull(dr.Item("PREVIOUSTASKENDLOCATION"))), _
                            Made4Net.Shared.Util.FormatField(_userid), Made4Net.Shared.Util.FormatField(ReplaceDBNull(dr.Item("PREVIOUSACTIVITYTIME"))))
            data.Dispose()
            DataInterface.RunSQL(SQL)
        End If
    End Function

    Public Sub UpdateLocation(ByVal lastLocation As String)
        Dim SQL As String = String.Format("UPDATE WHACTIVITY SET LOCATION={0} WHERE USERID = {1}", _
                           Made4Net.Shared.Util.FormatField(lastLocation), Made4Net.Shared.Util.FormatField(_userid))
        DataInterface.RunSQL(SQL)
    End Sub

    Public Sub UpdateMHEType(ByVal MHEType As String)
        Dim SQL As String = String.Format("UPDATE WHACTIVITY SET HETYPE={0} WHERE USERID = {1}",
                           Made4Net.Shared.Util.FormatField(MHEType), Made4Net.Shared.Util.FormatField(_userid))
        DataInterface.RunSQL(SQL)
    End Sub
    Public Sub UpdateMHEID(ByVal MHEId As String)
        Dim SQL As String = String.Format("UPDATE WHACTIVITY SET MHEID={0} WHERE USERID = {1}",
                           Made4Net.Shared.Util.FormatField(MHEId), Made4Net.Shared.Util.FormatField(_userid))
        DataInterface.RunSQL(SQL)
    End Sub

    Public Shared Function Exists(ByVal pUserID As String) As Boolean
        Dim SQL As String = String.Format("SELECT COUNT(*) FROM WHACTIVITY WHERE USERID = '{0}'", pUserID)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
    End Function

    Public Shared Function Delete(ByVal pUserID As String) As Boolean
        Dim SQL As String = String.Format("Delete from WHACTIVITY WHERE USERID = '{0}'", pUserID)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(SQL))
    End Function


    Public Shared Sub SetShift(ByVal pUserID As String, ByVal pShiftID As String, ByVal ClockType As Integer, Optional ByVal pLocation As String = "")
        pShiftID = Common.GetCurrentUserShiftId
        If ClockType = WMS.Lib.Shift.ClockStatus.IN Then
            If pLocation <> String.Empty Then
                DataInterface.RunSQL(String.Format("update WHACTIVITY set  SHIFT='{1}',Location='{2}'  WHERE USERID = '{0}'", pUserID, pShiftID, pLocation))
            Else
                DataInterface.RunSQL(String.Format("update WHACTIVITY set  SHIFT='{1}'  WHERE USERID = '{0}'", pUserID, pShiftID))
            End If
        Else
            DataInterface.RunSQL(String.Format("update WHACTIVITY set  SHIFT='',Location=''  WHERE USERID = '{0}'", pUserID))
        End If
    End Sub

    Public Shared Function getShift(ByVal pUserID As String) As String
        Dim SQL As String = String.Format("SELECT isnull(SHIFT,'') SHIFT FROM WHACTIVITY WHERE USERID = '{0}'", pUserID)
        Return ReplaceDBNull(DataInterface.ExecuteScalar(SQL), "")
    End Function

    Public Shared Function CloseShift(ByVal pShiftID As String) As String
        pShiftID = Common.GetCurrentUserShiftId
        Dim SQL As String = String.Format("update WHACTIVITY set SHIFT='' WHERE SHIFT = '{0}'", pShiftID)
        Return ReplaceDBNull(DataInterface.ExecuteScalar(SQL), "")
    End Function

    Public Shared Function GetPreviousActivityTime(ByVal whPreviousActivityTime As DateTime?, ByVal USERID As String) As DateTime?
        Dim whActivity As New DataTable
        Dim query As String
        query = String.Format("Select * from WHACTIVITY where userid = '{0}'", USERID)
        DataInterface.FillDataset(query, whActivity)
        If whActivity.Rows.Count >= 1 AndAlso Not whActivity.Rows(0).IsNull("PREVIOUSACTIVITYTIME") Then
            whPreviousActivityTime = whActivity.Rows(0)("PREVIOUSACTIVITYTIME")
        End If
        Return whPreviousActivityTime
    End Function

    Public Shared Function GetPreviousActivityLocation(ByVal userid As String) As String
        Dim whActivity As New DataTable
        Dim previousActivityLocation As String = String.Empty
        Dim query As String
        query = String.Format("Select PREVIOUSTASKENDLOCATION from WHACTIVITY where userid = '{0}'", userid)
        DataInterface.FillDataset(query, whActivity)
        If whActivity.Rows.Count >= 1 AndAlso Not whActivity.Rows(0).IsNull("PREVIOUSTASKENDLOCATION") Then
            previousActivityLocation = whActivity.Rows(0)("PREVIOUSTASKENDLOCATION")
        End If
        Return previousActivityLocation
    End Function

    Public Shared Function GetUserCurrentLocation(ByVal userid As String) As String
        Dim whActivity As New DataTable
        Dim currentLocation As String = String.Empty
        Dim query As String
        query = String.Format("Select LOCATION from WHACTIVITY where userid = '{0}'", userid)
        DataInterface.FillDataset(query, whActivity)
        If whActivity.Rows.Count >= 1 AndAlso Not whActivity.Rows(0).IsNull("LOCATION") Then
            currentLocation = whActivity.Rows(0)("LOCATION")
        End If
        Return currentLocation
    End Function
#End Region

End Class