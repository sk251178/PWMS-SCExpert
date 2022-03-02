Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports WMS.Lib
Imports Made4Net.Shared.Conversion.Convert
Imports System.Collections.Generic

#Region "ShiftClass"

<CLSCompliant(False)> Public Class ShiftTimeBlock
#Region "Other Fields"
    Protected _blocktype As String = String.Empty
    Protected _fromtime As Integer
    Protected _totime As Integer
    Protected _accountabletime As Integer

#Region "Properties"
    Public ReadOnly Property BLOCKTYPE() As String
        Get
            Return _blocktype
        End Get
    End Property
    Public ReadOnly Property FROMTIME() As Integer
        Get
            Return _fromtime
        End Get
    End Property
    Public ReadOnly Property TOTIME() As Integer
        Get
            Return _totime
        End Get
    End Property
    Public ReadOnly Property ACCOUNTABLETIME() As Integer
        Get
            Return _accountabletime
        End Get
    End Property

#End Region

#Region "Constructors"
    Sub New(ByVal pBlocktype As String, ByVal pFromTime As Integer, ByVal pToTime As Integer, ByVal pAaccountabletime As Boolean)
        _blocktype = pBlocktype
        _fromtime = pFromTime
        _totime = pToTime
        _accountabletime = pAaccountabletime
    End Sub
#End Region

#End Region

End Class

<CLSCompliant(False)> Public Class Shift
#Region "Primary Keys"

    Protected _shiftid As String = String.Empty

#Region "Other Fields"
    Protected _shiftcode As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _scheduledstarttime As Integer
    Protected _scheduledendtime As Integer
    Protected _startdate As DateTime
    Protected _enddate As DateTime
    Protected _status As String = String.Empty
    Protected _timeblocks As ArrayList = New ArrayList()

#End Region

#Region "Constructors"
    Sub New()
    End Sub

    Sub New(ByVal pShiftID As String, Optional ByVal LoadObj As Boolean = True)
        _shiftid = pShiftID
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region


#Region "Methods"

    Public Shared Function GetShift(ByVal pShiftId As String) As Shift
        Return New Shift(pShiftId)
    End Function

    Public Function CreateInstance(ByVal pShiftCode As String, _
            ByVal pwarehousearea As String, _
            ByVal shiftday As String) As String
        _shiftid = Made4Net.Shared.Util.getNextCounter("SHIFTID")
        _shiftcode = pShiftCode
        _warehousearea = pwarehousearea
        _status = WMS.Lib.Shift.ShiftInstanceStatus.[NEW]

        Dim sql = String.Format("insert into SHIFT " & _
            "(SHIFTID,SHIFTCODE,warehousearea,SCHEDULEDSTARTTIME,SCHEDULEDENDTIME,STATUS) " & _
            "select '{0}','{1}','{2}', SHIFTSTARTTIME,SHIFTENDTIME, '{3}'  from SHIFTDETAIL " & _
            "where SHIFTCODE='{1}' and SHIFTDAY='{4}'", _
            _shiftid, _shiftcode, _warehousearea, _status, shiftday)
        DataInterface.RunSQL(sql)

        sql = String.Format("insert into dbo.SHIFTTIMEBLOCKS " & _
            "(SHIFTID,TIMEBLOCKTYPE,FROMTIME,TOTIME,ACCOUNTABLETIME) " & _
            " select '{0}', TIMEBLOCKTYPE, FROMTIME, TOTIME, ACCOUNTABLETIME from SHIFTMASTERTIMEBLOCKS " & _
            " where SHIFTCODE='{1}'  and SHIFTDAY='{2}'", _
            _shiftid, _shiftcode, shiftday)
        DataInterface.RunSQL(sql)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ShiftInstanceCreated
        em.Add("EVENT", EventType)
        em.Add("SHIFTID", _shiftid)
        em.Add("SHIFTCODE", _shiftcode)
        em.Add("WAREHOUSEAREA", _warehousearea)
        em.Add("USERID", Common.GetCurrentUser)
        em.Add("CREATEDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))


        Return _shiftid


    End Function

    Protected Sub Load()
        ''''?????????
    End Sub
#End Region

#End Region

End Class

#End Region

#Region "Shift Instance"
<CLSCompliant(False)> Public Class ShiftInstance
#Region "Key Fields"
    Protected Shared _shiftid As String = String.Empty
#End Region

#Region "Other Fields"
    Protected Shared _shiftcode As String = String.Empty
    Protected Shared _warehousearea As String = String.Empty
    Protected Shared _starttime As Integer
    Protected Shared _endtime As Integer
    Protected Shared _status As String = String.Empty
    Protected Shared _startdate As DateTime
    Protected Shared _enddate As DateTime
    Protected _timeblocks As ArrayList = New ArrayList()
#End Region

#Region "Properties"
    Public ReadOnly Property SHIFTCODE() As String
        Get
            Return _shiftcode
        End Get
    End Property

    Public ReadOnly Property WAREHOUSEAREA() As String
        Get
            Return _warehousearea
        End Get
    End Property
    Public ReadOnly Property STARTTIME() As Integer
        Get
            Return _starttime
        End Get
    End Property
    Public ReadOnly Property ENDTIME() As Integer
        Get
            Return _endtime
        End Get
    End Property


    Public ReadOnly Property STATUS() As String
        Get
            Return _status
        End Get
    End Property

    Public ReadOnly Property ENDDATE() As DateTime
        Get
            Return _enddate
        End Get
    End Property
    Public ReadOnly Property STARTDATE() As DateTime
        Get
            Return _startdate
        End Get
    End Property

    Public ReadOnly Property TIMEBLOCKS() As ArrayList
        Get
            Return _timeblocks
        End Get
    End Property

#End Region

#Region "Constructors"
    Sub New()
    End Sub

    Sub New(ByVal pShiftID As String, _
        Optional ByVal LoadObj As Boolean = True, _
        Optional ByVal LoadTimeBlocksObj As Boolean = True)
        _shiftid = pShiftID
        If LoadObj Then
            Load()
        End If
        If LoadTimeBlocksObj Then
            LoadTimeBlocks()
        End If
    End Sub
#End Region


#Region "Methods"
    Protected Sub Load()
        Dim SQL As String = String.Format("SELECT * FROM SHIFT WHERE SHIFTID='{0}'", _shiftid)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then Return
        dr = dt.Rows(0)

        If Not dr.IsNull("SHIFTCODE") Then _shiftcode = dr.Item("SHIFTCODE")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("SCHEDULEDSTARTTIME") Then _starttime = dr.Item("SCHEDULEDSTARTTIME")
        If Not dr.IsNull("SCHEDULEDENDTIME") Then _endtime = dr.Item("SCHEDULEDENDTIME")
        If Not dr.IsNull("STARTDATE") Then _startdate = dr.Item("STARTDATE")
        If Not dr.IsNull("ENDDATE") Then _enddate = dr.Item("ENDDATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")


    End Sub

    Private Sub LoadTimeBlocks()
        ''load time blocks
        Dim SQL As String = String.Format("SELECT * FROM SHIFTTIMEBLOCKS WHERE SHIFTID='{0}' order by STARTTIME ", _shiftid)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows
            Dim oShiftTimeBlock As ShiftTimeBlock = New ShiftTimeBlock(ReplaceDBNull(dr.Item("TIMEBLOCKTYPE")), _
                                          ReplaceDBNull(dr.Item("FROMTIME")), _
                                          ReplaceDBNull(dr.Item("TOTIME")), _
                                          ReplaceDBNull(dr.Item("ACCOUNTABLETIME")))

            _timeblocks.Add(oShiftTimeBlock)
        Next
    End Sub
    Public Shared Function getShihtIDbyUserID(ByVal pUserID As String) As String

        Dim SQL As String = String.Format("select isnull(DEFAULTSHIFT,'') DEFAULTSHIFT from USERWAREHOUSE where WAREHOUSE='{0}' and USERID='{1}' ", Warehouse.CurrentWarehouse, pUserID)
        Dim DefaultShift As String = DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME)
        ''Start RWMS-1667' Since system is not validating shift status =Started.
        'SQL = String.Format("select top 1 SHIFTID from SHIFT where SHIFTCODE='{0}' and STATUS='{1}'", DefaultShift, WMS.Lib.Shift.ShiftInstanceStatus.STARTED)
        'Dim ShiftID As String = DataInterface.ExecuteScalar(SQL)
        '  Return ShiftID
        ''End RWMS-1667' Since system is not validating shift status =Started.
        Return DefaultShift
    End Function

    Protected Shared Function checkShiftIDbyUserID(ByVal pUserID As String, ByVal pShiftID As String) As Boolean
        Dim SQL As String = String.Format("select isnull(DEFAULTSHIFT,'') DEFAULTSHIFT from USERWAREHOUSE where WAREHOUSE='{0}' and USERID='{1}' ", Warehouse.CurrentWarehouse, pUserID)
        Dim DefaultShift As String = DataInterface.ExecuteScalar(SQL, Made4Net.Schema.CONNECTION_NAME)
        If WMS.Logic.GetSysParam("AllowClockinBeforeShiftStart") = "0" Then
            SQL = String.Format("select SHIFTCODE from SHIFT where  SHIFTID='{0}' ", pShiftID, WMS.Lib.Shift.ShiftInstanceStatus.STARTED)
        Else
            SQL = String.Format("select SHIFTCODE from SHIFT where  SHIFTID='{0}' and STATUS='{1}'", pShiftID, WMS.Lib.Shift.ShiftInstanceStatus.STARTED)
        End If

        Dim ShiftCode As String = DataInterface.ExecuteScalar(SQL)
        If DefaultShift = "" Or DefaultShift = ShiftCode Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function GetCurrentShiftID(Optional ByVal pUserID As String = "") As String
        If pUserID = "" Then
            pUserID = Common.GetCurrentUser()
        End If

        Dim sql As String = String.Format("Select top(1)shift from whactivity where userid={0}", Made4Net.Shared.FormatField(pUserID))
        Return DataInterface.ExecuteScalar(sql)

    End Function

    Public Shared Function ClockUser(ByVal pUserID As String, ByVal InOut As Integer, ByVal pLocation As String, _
                                    Optional ByVal pShiftID As String = "") As Integer
        Dim SQL As String
        If InOut = WMS.Lib.Shift.ClockStatus.IN Then

            If pShiftID = "" Then
                pShiftID = getShihtIDbyUserID(pUserID)
                If pShiftID = "" Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Shift is not set for current user", "Shift is not set for current user")
                    Return -10
                End If
            Else
                If Not checkShiftIDbyUserID(pUserID, pShiftID) Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-in User to Shift. User asssigned to another Default Shift.", "Cannot Assign User to Shift. User asssigned to another Default Shift.")
                    Return -20
                End If
            End If

            ''check if clock in
            SQL = String.Format("select isnull(SHIFT,'') SHIFT from WHACTIVITY where USERID='{0}' ", pUserID)
            If DataInterface.ExecuteScalar(SQL) <> String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-in User to Shift. User is already clocked-in.", "Cannot Clock-in User to Shift. User is already clocked-in.")
                Return -30
            End If
        End If

        If InOut = WMS.Lib.Shift.ClockStatus.OUT Then
            SQL = String.Format("select isnull(SHIFT,'') SHIFT from WHACTIVITY where USERID='{0}' ", pUserID)
            If DataInterface.ExecuteScalar(SQL) = String.Empty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-out User from Shift. User already Clock-out.", "Cannot Clock-out User from Shift. User already Clock-out.")
                Return -40
            End If

        End If

        ''check if shift started
        If pShiftID <> String.Empty Then
            If WMS.Logic.GetSysParam("AllowClockinBeforeShiftStart") = "0" Then
                Dim CurrentShiftStatus = DataInterface.ExecuteScalar(String.Format("select STATUS from SHIFT where SHIFTID='{0}'", pShiftID))
                If CurrentShiftStatus <> WMS.Lib.Shift.ShiftInstanceStatus.STARTED Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Clock-in/out Shift not started.", "Cannot Clock-in Shift not started")
                    Return -50
                End If

            End If

        End If

        'clock in
        ''convert(nvarchar,datepart(hour,getdate()))+ convert(nvarchar,datepart(mi,getdate()))
        Try
            SQL = String.Format("insert dbo.SHIFTUSERCLOCKS " & _
                "([SHIFTID],[USERID],[CLOCKINOUT],[CLOCKTIME],[ADDDATE])" & _
                " values ('{0}','{1}','{2}',getdate(),getdate())", pShiftID, pUserID, InOut)
            DataInterface.RunSQL(SQL)
            WHActivity.SetShift(pUserID, pShiftID, InOut, pLocation)

            '' init LABORUSERCOUNTERS for current user
            SQL = String.Format("update LABORUSERCOUNTERS set LABORCOUNTERVALUE=0 where USERID='{0}'", pUserID)
            DataInterface.RunSQL(SQL)

        Catch ex As Exception
            Throw New Made4Net.Shared.M4NException(New Exception, "Error Clock-in User to Shift:" & ex.ToString(), "Error Clock-in User to Shift:" & ex.Message)
            Return -99

        End Try
    End Function

    Public Shared Function checkUserClockinShift(ByVal pUserID As String) As Boolean
        Dim ShiftID As String = WHActivity.getShift(pUserID)
        If ShiftID <> "" Then
            Dim SQL As String = String.Format("select CLOCKINOUT from SHIFTUSERCLOCKS where SHIFTID='{0}' and USERID='{1}' ", ShiftID, pUserID)
            Return (ReplaceDBNull(DataInterface.ExecuteScalar(SQL), 0) = 1)
        Else
            Return False
        End If
    End Function

    Public Shared Sub CkeckOutUsersonShiftClose(ByVal pShiftID As String)
        Dim SQL As String = String.Format("update  SHIFTUSERCLOCKS set CLOCKINOUT='{1}' where SHIFTID='{0}' ", pShiftID, WMS.Lib.Shift.ClockStatus.OUT)
        DataInterface.RunSQL(SQL)
    End Sub


    Public Shared Sub ChangeShiftStatus(ByVal pStatus As String, ByVal pShiftID As String, _
                Optional ByVal pStartDate As String = "", Optional ByVal pEndDate As String = "")
        Dim sql As String
        Select Case pStatus
            Case WMS.Lib.Shift.ShiftInstanceStatus.STARTED
                sql = String.Format("update SHIFT set STATUS={0}, STARTDATE={1}, ENDDATE={2} where SHIFTID={3}", _
                        Made4Net.Shared.FormatField(pStatus), Made4Net.Shared.FormatField(pStartDate), Made4Net.Shared.FormatField(pEndDate), Made4Net.Shared.FormatField(pShiftID))
                DataInterface.RunSQL(sql)

            Case WMS.Lib.Shift.ShiftInstanceStatus.CLOSED, WMS.Lib.Shift.ShiftInstanceStatus.CANCELED
                sql = String.Format("update SHIFT set STATUS={0} where SHIFTID={1}", _
                        Made4Net.Shared.FormatField(pStatus), Made4Net.Shared.FormatField(pShiftID))
                DataInterface.RunSQL(sql)
                WHActivity.CloseShift(pShiftID)
        End Select


    End Sub
    Protected Shared Function ConvertToDouble(dt As DataTable, Optional defval As Double = 0, Optional row As Integer = 0, Optional col As Integer = 0) As Double
        If dt IsNot Nothing AndAlso dt.Rows.Count > row AndAlso dt.Columns.Count > col AndAlso Not dt.Rows(row).IsNull(col) Then
            Try
                Return Convert.ToDouble(dt(row)(col))
            Catch ex As Exception
            End Try
        End If
        Return defval
    End Function
    Protected Shared Function ConvertToInt32(dt As DataTable, Optional defval As Int32 = 0, Optional row As Integer = 0, Optional col As Integer = 0) As Int32
        If dt IsNot Nothing AndAlso dt.Rows.Count > row AndAlso dt.Columns.Count > col AndAlso Not dt.Rows(row).IsNull(col) Then
            Try
                Return Convert.ToInt32(dt(row)(col))
            Catch ex As Exception
            End Try
        End If
        Return defval
    End Function

    Public Shared Function GetUserPerformanceOnShift(ByVal pUserID As String, ByVal pShiftID As String) As Decimal
        '' RWMS-2057
        'Dim dayOfWeek As System.DayOfWeek = DateTime.Now.DayOfWeek
        'Dim dayOfWeekInInteger As Integer = dayOfWeek
        'Dim SQL As String = String.Format("select isnull(sum(isnull(actualtime, 0)),0) as SumOfActuals, isnull(sum(isnull(STANDARTTIME,0)),0) as SumOfStandards from LABORPERFORMANCEAUDIT where DATEPART(dw,ENDDATE) = {0} and userid = '{1}'", dayOfWeekInInteger + 1, pUserID)
        'Dim dt As DataTable = New DataTable()
        'Dim dr As DataRow

        'DataInterface.FillDataset(SQL, dt)
        'If dt.Rows.Count = 0 Then Return 0
        'dr = dt.Rows(0)

        'Dim sumOfActuals As Integer = dr("SumOfActuals")
        'Dim sumOfStandards As Decimal = dr("SumOfStandards")
        'Dim userShiftPerformance As Decimal = 0
        'If sumOfActuals > 0 Then
        '    userShiftPerformance = 100 * (sumOfStandards / sumOfActuals)
        'End If
        'Return userShiftPerformance
        '' RWMS-2057
        'RWMS-2835
        Dim dayOfWeek As System.DayOfWeek = DateTime.Now.DayOfWeek
        Dim dayOfWeekInInteger As Integer = dayOfWeek + 1
        Dim SQL As String = String.Empty

        Dim dt As DataTable = New DataTable()
        Dim dr As DataRow
        Dim ActualTime As Decimal = 0
        Dim TotalStandardTime As Decimal = 0
        Dim TotalBreakTime As Integer = 0
        Dim userShiftPerformance As Decimal = 0
        Dim ExtraBreaktime = 0
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        Dim startDateTime As DateTime = DateTime.MinValue
        Dim endDateTime As DateTime = DateTime.MinValue

        SQL = "Select * from ShiftDateTimeView"

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Executing Query: {0}", SQL))
        End If
        Try
            dt = New DataTable()
            DataInterface.FillDataset(SQL, dt)
            Dim shiftDetail As DataRow = dt.AsEnumerable().Where(Function(rw) Convert.ToInt32(rw("SHIFTDAY")) = dayOfWeekInInteger And Convert.ToInt32(rw("SHIFTCODE")) = pShiftID).FirstOrDefault()
            If (dt.Rows.Count > 0 And Not shiftDetail Is Nothing) Then
                startDateTime = Convert.ToDateTime(shiftDetail("SHIFTSTARTDATETIME"))
                endDateTime = Convert.ToDateTime(shiftDetail("SHIFTENDDATETIME"))
            Else
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format("Shift details not found for shift code: {0} and shift day: {1}. \n User Shift Performance = 0 %", pShiftID, dayOfWeekInInteger))
                End If
                Return userShiftPerformance
            End If
        Catch
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format("Invalid shift data for shift code: {0} and shift day: {1}. User Shift Performance = 0 %", pShiftID, dayOfWeekInInteger))
            End If
            Return userShiftPerformance

        End Try

        If (startDateTime.Subtract(DateTime.Now).Hours > 4) Then
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format("User logged in 4 hours prior to the shift start time for shift code: {0} and shift day: {1}. Hence SHift performance is considered as 0 %", pShiftID, dayOfWeekInInteger))
            End If
            Return userShiftPerformance
        ElseIf startDateTime.Subtract(DateTime.Now).Hours > 0 And startDateTime.Subtract(DateTime.Now).Hours <= 4 Then
            startDateTime = DateTime.Now.AddHours(-4)
        End If

        If (DateTime.Now.Subtract(endDateTime).Hours > 4) Then
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format("User logged in after 4 hours of shift end time for shift code: {0} and shift day: {1}. Hence Shift performance is considered as 0 %", pShiftID, dayOfWeekInInteger))
            End If
            Return userShiftPerformance
        End If

        'If shift end day is yesterday and the user is still working the day of week in integer has to be decremented by 1 to get the tasks and breaks
        If (DateTime.Now.Subtract(endDateTime).Days > 0) Then
            dayOfWeekInInteger = If(dayOfWeekInInteger = 1, 7, dayOfWeekInInteger - 1)
        End If


        'Sum of standardtime
        SQL = String.Format("select sum(isnull(standarttime,0)) from LABORPERFORMANCEAUDIT where USERID ='{0}' and STARTDATE between convert(datetime,'{1}') and GETDATE()", pUserID, startDateTime.ToString())
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Query to get Total Standard Time of Tasks performed by user on the shift : {0}", SQL))
        End If

        dt = New DataTable()
        DataInterface.FillDataset(SQL, dt)
        If Not (dt.Rows.Count > 0) Then
            Return userShiftPerformance
        End If

        TotalStandardTime = ConvertToDouble(dt, userShiftPerformance)

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Total Standard time: {0}", TotalStandardTime.ToString()))
        End If

        'Total Break time in shift
        If (startDateTime.Hour > DateTime.Now.Hour) Then
            SQL = String.Format("select sum(DATEDIFF(ss, convert(datetime,fromtime),convert(datetime,totime))) from SHIFTMASTERTIMEBLOCKS where SHIFTDAY={0} and SHIFTCODE={1} and ((convert(time,fromtime) between convert(time,'{2}') and convert(time,('23:59:59:59'))) or (convert(time,fromtime) between convert(time,'00:00') and convert(time,GETDATE())))", dayOfWeekInInteger, pShiftID, startDateTime)
        Else
            SQL = String.Format("select sum(DATEDIFF(ss, convert(datetime,fromtime),convert(datetime,totime))) from SHIFTMASTERTIMEBLOCKS where SHIFTDAY={0} and SHIFTCODE={1} and ((convert(time,fromtime) between convert(time,'{2}') and convert(time,convert(time,GETDATE()))))", dayOfWeekInInteger, pShiftID, startDateTime)
        End If

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Query to get Total Break time of Tasks performed by user on the shift : {0}", SQL))
        End If

        dt = New DataTable()
        DataInterface.FillDataset(SQL, dt)
        TotalBreakTime = ConvertToInt32(dt, userShiftPerformance)

        'Calculation of extra break time if the current time is falling in the interval of any break time in the ShiftMasterTimeBlocks
        SQL = String.Format("select DATEDIFF(ss, convert(time,getdate()),convert(datetime,totime)) from SHIFTMASTERTIMEBLOCKS  where SHIFTDAY={0} and SHIFTCODE={1} and convert(time,getdate()) between convert(time,fromtime) and convert(time,totime)", dayOfWeekInInteger, pShiftID)
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Query to get the extra break time to subtract from the Total Break Time : {0}", SQL))
        End If

        dt = New DataTable()
        DataInterface.FillDataset(SQL, dt)

        ExtraBreaktime = ConvertToInt32(dt, userShiftPerformance)

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Total break time: {0}", (TotalBreakTime - ExtraBreaktime).ToString()))
        End If


        ActualTime = (DateTime.Now - startDateTime).TotalSeconds - TotalBreakTime - ExtraBreaktime

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("Actual time: (Total Shift Time - Total Break Time) :{0}", ActualTime.ToString()))
        End If

        userShiftPerformance = 100 * TotalStandardTime / ActualTime

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format("User Shift Performance (100 * Total Standard Time / ActualTime): {0}", userShiftPerformance.ToString()))
        End If

        Return userShiftPerformance
        'RWMS-2835
    End Function

#End Region




End Class

#End Region

#Region "Shift Detail"

Public Class ShiftDetail


#Region "Primary Keys"
#End Region

#Region "Other fields"
    Private dateRangeCollection As New List(Of ShiftDetailRow)
#End Region

#Region "Constructor"
    Private Sub New()
    End Sub

    Sub New(ByVal pShiftCode As String)
        Load(pShiftCode)
    End Sub

    Sub New(ByVal pShiftDetail As DataTable)
        Load(pShiftDetail)
    End Sub
#End Region

#Region "Methods"
    Private Sub Load(ByVal shiftCode As String)
        If Not String.IsNullOrEmpty(shiftCode) Then
            Dim SQL As String = String.Format("SELECT * FROM SHIFTDETAIL WHERE SHIFTCODE='{0}'", shiftCode)
            Dim shiftDetail As New DataTable
            DataInterface.FillDataset(SQL, shiftDetail)
            Load(shiftDetail)
        End If

    End Sub

    Private Sub Load(ByVal shiftDetail As DataTable)
        If Not shiftDetail Is Nothing Then
            For Each dr As DataRow In shiftDetail.Rows
                dateRangeCollection.Add(New ShiftDetailRow(dr))
            Next
        End If

    End Sub

    Public Function GetShiftStartTime(ByVal taskStartDate As Date) As DateTime
        Dim result As DateTime = DateTime.MinValue
        Dim shiftRow As ShiftDetailRow
        ' Get this week's start date and last week's start date based on task start date
        For Each projectedWeekStart As DateTime In New DateTime() {taskStartDate.GetWeekStart(), taskStartDate.GetPriorWeekStart()}
            ' Get shift start time for the task start date
            shiftRow = dateRangeCollection.Where(Function(x As ShiftDetailRow)
                                                     Return x.IsInRange(taskStartDate, projectedWeekStart)
                                                 End Function).FirstOrDefault()
            If Not shiftRow Is Nothing Then
                result = shiftRow.StartTimeInWeek(projectedWeekStart)
                Exit For
            End If
        Next

        Return result
    End Function
#End Region

End Class
#End Region


#Region "Shift Detail Row"
Public Class ShiftDetailRow

    Private Class Columns
        Public Shared ReadOnly Property StartTime() As String
            Get
                Return "SHIFTSTARTTIME"
            End Get
        End Property

        Public Shared ReadOnly Property StopTime() As String
            Get
                Return "SHIFTENDTIME"
            End Get
        End Property

        Public Shared ReadOnly Property ShiftDay() As String
            Get
                Return "SHIFTDAY"
            End Get
        End Property
    End Class

    Sub New(ByVal shiftDetailRow As DataRow)
        Dim spanStart As TimeSpan = New TimeSpan(0)
        Dim spanEnd As TimeSpan = New TimeSpan(0)
        Dim shiftDay As Integer


        If Not shiftDetailRow Is Nothing _
           AndAlso shiftDetailRow.ContainsColumns(Columns.StartTime, Columns.StopTime, Columns.ShiftDay) _
           AndAlso TimeSpan.TryParse(shiftDetailRow(Columns.StartTime).ToString(), spanStart) _
           AndAlso TimeSpan.TryParse(shiftDetailRow(Columns.StopTime).ToString(), spanEnd) _
           AndAlso Integer.TryParse(shiftDetailRow(Columns.ShiftDay).ToString(), shiftDay) Then
            StartTime = spanStart
            StopTime = spanEnd
            ShiftDayWeek = shiftDay
            If StopTime < StartTime Then
                StopTime = StopTime.Add(New TimeSpan(1, 0, 0, 0))
            End If
        End If

    End Sub

    Private startTimeSpan As TimeSpan
    Public Property StartTime() As TimeSpan
        Get
            Return startTimeSpan
        End Get
        Private Set(ByVal value As TimeSpan)
            startTimeSpan = value
        End Set
    End Property

    Private stopTimeSpan As TimeSpan
    Public Property StopTime() As TimeSpan
        Get
            Return stopTimeSpan
        End Get
        Private Set(ByVal value As TimeSpan)
            stopTimeSpan = value
        End Set
    End Property

    Private shiftDayOfWeek As Integer
    Public Property ShiftDayWeek() As Integer
        Get
            Return shiftDayOfWeek
        End Get
        Private Set(ByVal value As Integer)
            shiftDayOfWeek = value
        End Set
    End Property

    'Returns true if the date falls between start time and end time of the shift
    Public Function IsInRange(ByVal dt As DateTime, ByVal weekStart As DateTime) As Boolean
        If dt < weekStart Then
            Return False
        End If

        Dim relativeTime As TimeSpan = dt - weekStart - TimeSpan.FromDays(ShiftDayWeek - 1)

        Return relativeTime >= startTimeSpan AndAlso relativeTime <= stopTimeSpan
    End Function

    'Returns shift start date time by adding days and hours to the week start date time
    Public Function StartTimeInWeek(ByVal weekStart As DateTime) As DateTime
        Return weekStart.Add(New TimeSpan(ShiftDayWeek - 1, StartTime.Hours, StartTime.Minutes, 0))
    End Function

End Class
#End Region