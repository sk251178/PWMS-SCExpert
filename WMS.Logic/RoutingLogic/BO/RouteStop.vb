Imports Made4Net.DataAccess
Imports Made4Net.Shared.Util
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class RouteStop

#Region "Variables"

    Protected _routeid As String
    Protected _stopnumber As Int32
    Protected _stopname As String
    Protected _status As StopStatus
    Protected _pointid As String

    Protected _arrivaltime As Int32
    Protected _departuretime As Int32

    Protected _arrivaldate As DateTime
    Protected _departuredate As DateTime

    Protected _adddays As Int32

    Protected _actualarrivaldate As DateTime
    Protected _actualdeparturedate As DateTime



    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

    Protected _routestopdetailcollection As New RouteStopTaskCollection
    'Protected _taskscollection As RouteTaskCollection

#End Region

#Region "Properties"
    Public Property ArrivalDate() As DateTime
        Get
            Return _arrivaldate
        End Get
        Set(ByVal Value As DateTime)
            _arrivaldate = Value
        End Set
    End Property

    Public Property DepartureDate() As DateTime
        Get
            Return _departuredate
        End Get
        Set(ByVal Value As DateTime)
            _departuredate = Value
        End Set
    End Property

    Protected ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" routeid = {0} and stopnumber = {1} ", Made4Net.Shared.Util.FormatField(_routeid), Made4Net.Shared.Util.FormatField(_stopnumber))
        End Get
    End Property

    Public Property RouteID() As String
        Set(ByVal Value As String)
            _routeid = Value
        End Set
        Get
            Return _routeid
        End Get
    End Property

    Public Property StopNumber() As Int32
        Get
            Return _stopnumber
        End Get
        Set(ByVal Value As Int32)
            _stopnumber = Value
        End Set
    End Property

    Public Property StopName() As String
        Get
            Return _stopname
        End Get
        Set(ByVal Value As String)
            _stopname = Value
        End Set
    End Property

    Public Property Status() As StopStatus
        Get
            Return _status
        End Get
        Set(ByVal Value As StopStatus)
            _status = Value
        End Set
    End Property

    Public Property PointId() As String
        Get
            Return _pointid
        End Get
        Set(ByVal Value As String)
            _pointid = Value
        End Set
    End Property

    Public Property ArrivalTime() As Int32
        Get
            Return _arrivaltime
        End Get
        Set(ByVal Value As Int32)
            _arrivaltime = Value
        End Set
    End Property

    Public Property DepartureTime() As Int32
        Get
            Return _departuretime
        End Get
        Set(ByVal Value As Int32)
            _departuretime = Value
        End Set
    End Property
    Public Property AddDays() As Int32
        Get
            Return _adddays
        End Get
        Set(ByVal Value As Int32)
            _adddays = Value
        End Set
    End Property

    Public Property ActualArrivalDate() As DateTime
        Get
            Return _actualarrivaldate
        End Get
        Set(ByVal Value As DateTime)
            _actualarrivaldate = Value
        End Set
    End Property

    Public Property ActualDepartureDate() As DateTime
        Get
            Return _actualdeparturedate
        End Get
        Set(ByVal Value As DateTime)
            _actualdeparturedate = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public Property RouteStopTask() As RouteStopTaskCollection
        Get
            Return _routestopdetailcollection
        End Get
        Set(ByVal Value As RouteStopTaskCollection)
            _routestopdetailcollection = Value
        End Set
    End Property

    'Public ReadOnly Property GeneralTasks() As RouteTaskCollection
    '    Get
    '        Return _taskscollection
    '    End Get
    'End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal oRoute As Route, ByVal iStopNumber As Int32)
        _routeid = oRoute.RouteId
        _stopnumber = iStopNumber
        Dim sql As String = String.Format("Select * from ROUTESTOP where {0}", WhereClause)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Not Found")
        End If
        Load(dt.Rows(0))
    End Sub

    Public Sub New(ByVal oRoute As Route, ByVal dr As DataRow)
        _routeid = oRoute.RouteId
        Load(dr)
    End Sub

    Public Sub New(ByVal sRouteID As String, ByVal iStopNumber As Int32)
        _routeid = sRouteID
        _stopnumber = iStopNumber
        Dim sql As String = String.Format("Select * from ROUTESTOP where {0}", WhereClause)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Not Found")
        End If
        Load(dt.Rows(0))
    End Sub

    Public Sub New(ByVal sRouteID As String, ByVal dr As DataRow)
        _routeid = sRouteID
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub Load(ByVal dr As DataRow)
        _stopnumber = dr("stopnumber")
        _stopname = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("stopname"))
        _pointid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("pointid"))
        _status = StopStatusFromString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("status")))

        _arrivaldate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ARRIVALDATE"), Nothing)
        _departuredate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DEPARTUREDATE"), Nothing)

        _actualarrivaldate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("actualarrivaldate"), Nothing)
        _actualdeparturedate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("actualdeparturedate"), Nothing)
        _adddate = dr("adddate")
        _adduser = dr("adduser")
        _editdate = dr("editdate")
        _edituser = dr("edituser")

        _routestopdetailcollection = New RouteStopTaskCollection(Me)
        '_taskscollection = New RouteTaskCollection(Me)
    End Sub

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from ROUTESTOP where {0}", WhereClause)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Route Stop Not Found")
        End If
        Dim dr As DataRow = dt.Rows(0)
        _stopnumber = dr("stopnumber")
        _stopname = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("stopname"))
        _status = StopStatusFromString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("status")))
        _pointid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("pointid"))
        _arrivaldate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ARRIVALDATE"), Nothing)
        _departuredate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DEPARTUREDATE"), Nothing)
        _actualarrivaldate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("actualarrivaldate"), Nothing)
        _actualdeparturedate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("actualdeparturedate"), Nothing)
        _adddate = dr("adddate")
        _adduser = dr("adduser")
        _editdate = dr("editdate")
        _edituser = dr("edituser")

        _routestopdetailcollection = New RouteStopTaskCollection(Me)
        '_taskscollection = New RouteTaskCollection(Me)
    End Sub

    Private Function GetNextStopNumber() As Int32
        Dim SQL As String
        SQL = String.Format("select isnull(max(stopnumber),0) + 1 from routestop where routeid = '{0}'", _routeid)
        Return DataInterface.ExecuteScalar(SQL)
    End Function

    Public Shared Function Exists(ByVal sRouteId As String, ByVal pStopNumber As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from ROUTESTOP where routeid = {0} and stopnumber = {1}", Made4Net.Shared.Util.FormatField(sRouteId), pStopNumber)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function ExistInRoute(ByVal sRouteId As String, ByVal pPointId As String, ByRef pStopNumber As Int32) As Boolean
        Dim SQL As String = String.Format("select count(1) from ROUTESTOP where routeid = '{0}' and pointid = '{1}'", sRouteId, pPointId)
        If System.Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)) Then
            SQL = String.Format("select stopnumber from ROUTESTOP where routeid = '{0}' and pointid = '{1}'", sRouteId, pPointId)
            pStopNumber = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub UpdateStopNumber(ByVal pNewStopNumber As Int32, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser
        Dim SQL As String = String.Format("update routestop set stopnumber={0},EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(pNewStopNumber), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
        _stopnumber = pNewStopNumber
        'And Update all StopDetails Stopnumbers
        For Each oStopDetail As RouteStopTask In Me.RouteStopTask
            oStopDetail.UpdateStopNumber(pNewStopNumber, pUser)
        Next
    End Sub

#End Region

#Region "Create/Update"
    Public Shared Function getDateTime(ByVal DT As Date, ByVal TM As Integer, _
            ByRef adddays As Integer) As String
        Dim res As String
        If TM = 2400 Then
            TM = 0
            DT = DT.AddHours(24)
            adddays += 1
        End If
        res = DT.ToString("yyyy/MM/dd") & " " & _
            Format(TM, "0000").ToString.Substring(0, 2) & _
            ":" & _
            Format(TM, "0000").ToString.Substring(2, 2)
        Return res
    End Function
    Public Sub UpdateDepartureTime(ByVal pDepartureTime As Int32, _
        ByVal pUser As String)

        If DepartureTime <> pDepartureTime Then
            Dim servTime As Integer = RouteBaskets.DifTimeSecwithDays(Me.DepartureTime, pDepartureTime)
            Dim SQL As String
            SQL = String.Format("update ROUTESTOP set DEPARTUREDATE=dateadd(SS,{2},DEPARTUREDATE)," & _
                        " EDITUSER='{3}'" & _
                " where ROUTEID='{0}' and STOPNUMBER='{1}' ", Me.RouteID, Me.StopNumber, servTime, pUser)
            DataInterface.RunSQL(SQL)

            Me.DepartureTime = pDepartureTime

        End If
    End Sub



    Public Sub Create(ByVal pRouteId As String, ByVal pStopName As String, ByVal pPointId As String, ByVal pArrivalTime As Int32, ByVal pDepartureTime As Int32, ByVal pUser As String, _
    ByVal adddays As Int32, ByVal pRouteDate As Date, _
    Optional ByVal pStopNumber As Int32 = -1)

        Dim SQL As String
        _routeid = pRouteId
        _stopnumber = pStopNumber
        _stopname = pStopName
        _pointid = pPointId
        _status = StopStatus.Assigned
        If IsNumeric(pArrivalTime) Then _arrivaltime = pArrivalTime
        If IsNumeric(pDepartureTime) Then _departuretime = pDepartureTime
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser

        'Validate the Parameters for the new stop
        If ExistInRoute(_routeid, _pointid, _stopnumber) Then
            Load()
            Return
        End If
        If _routeid Is Nothing Or _routeid = String.Empty Then
            Throw New M4NException(New Exception, "Cannot Add Stop.Invalid Route", "Cannot Add Stop.Invalid Route")
        End If
        If _stopnumber = -1 Or _stopnumber = 0 Then
            _stopnumber = GetNextStopNumber()
        End If

        Dim ArrDate, DepDate As String

        Dim adddaysArrival As Integer = AddDays
        If pDepartureTime < pArrivalTime Then
            adddaysArrival -= 1
        End If

        ArrDate = getDateTime(DateAdd(DateInterval.Day, adddaysArrival, pRouteDate), pArrivalTime, adddaysArrival)
        DepDate = getDateTime(DateAdd(DateInterval.Day, AddDays, pRouteDate), pDepartureTime, AddDays)

        'Insert the new stop into the DB
        SQL = String.Format("INSERT INTO ROUTESTOP (ROUTEID, STOPNUMBER, STOPNAME, STATUS, POINTID,  ADDDATE, ADDUSER, EDITDATE, EDITUSER, ARRIVALDATE,DEPARTUREDATE) " & _
            " VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},convert(datetime,'{9}',120) ,convert(datetime,'{10}',120) )", FormatField(_routeid), FormatField(_stopnumber), FormatField(_stopname), Made4Net.Shared.Util.FormatField(StopStatusToString(_status)), FormatField(_pointid), _
             FormatField(_adddate), FormatField(_adduser), FormatField(_editdate), FormatField(_edituser), ArrDate, DepDate)
        DataInterface.RunSQL(SQL)

        'Create New Detail Collection
        If _routestopdetailcollection Is Nothing Then
            _routestopdetailcollection = New RouteStopTaskCollection
        End If
    End Sub

    Public Sub Update(ByVal pStopName As String, ByVal pArrivalTime As String, ByVal pDepartureTime As String, ByVal pUser As String, _
        ByVal pAddDays As Integer, ByVal pRouteDate As Date)

        Dim SQL As String
        _stopname = pStopName
        _arrivaltime = pArrivalTime
        _departuretime = pDepartureTime
        _editdate = DateTime.Now
        _edituser = pUser

        Dim ArrDate, DepDate As String

        Dim adddaysArrival As Integer = AddDays
        If pDepartureTime < pArrivalTime Then
            adddaysArrival -= 1
        End If


        ArrDate = getDateTime(DateAdd(DateInterval.Day, AddDays, pRouteDate), pArrivalTime, adddaysArrival)
        DepDate = getDateTime(DateAdd(DateInterval.Day, AddDays, pRouteDate), pDepartureTime, pAddDays)

        If _routeid Is Nothing Or _routeid = String.Empty Then
            Throw New M4NException(New Exception, "Cannot Update Stop.Invalid Route", "Cannot Update Stop.Invalid Route")
        End If
        SQL = String.Format("UPDATE ROUTESTOP SET STOPNAME ={0}, POINTID ={1},  EDITDATE ={2}, EDITUSER ={3},  ARRIVALDATE='{4}',DEPARTUREDATE='{5}' where {6}", _
            FormatField(_stopname), FormatField(_pointid), _
              FormatField(_editdate), FormatField(_edituser), _
                ArrDate, DepDate, WhereClause)
        DataInterface.RunSQL(SQL)

        If _routestopdetailcollection Is Nothing Then
            _routestopdetailcollection = New RouteStopTaskCollection(Me)
        End If
    End Sub

    Public Function Delete()
        Dim SQL As String
        SQL = String.Format("delete from routestop where {0}", WhereClause)
        DataInterface.ExecuteScalar(SQL)
    End Function

#End Region

#Region "Add/Remove Stop Details"

    Public Function AddStopDetail(ByVal pStopDetailType As StopTaskType, ByVal pScheduleDate As DateTime, _
                ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderType As String, ByVal pCompany As String, ByVal pCompType As String, ByVal pContactid As String, _
                ByVal pChkPnt As String, _
                ByVal pPackType As String, ByVal pNumPacks As Int32, ByVal pTransClass As String, ByVal pVolume As Double, ByVal pWeight As Double, ByVal pValue As Double, _
                ByVal pComments As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pUserId As String, Optional ByVal pStopTaskId As Int32 = -1) As RouteStopTask

        Dim oRouteStopDet As RouteStopTask
        If Me.RouteStopTask.StopDetailExists(pConsignee, pOrderid) Then
            oRouteStopDet = Me.RouteStopTask.GetStopDetail(pConsignee, pOrderid)
        Else
            oRouteStopDet = New RouteStopTask
            'Dim oContact As New WMS.Logic.Contact(pContactid)
            oRouteStopDet.Create(_routeid, _stopnumber, pStopDetailType, "", pContactid, pChkPnt, pScheduleDate, _
              pConsignee, pOrderid, pOrderType, pCompany, pCompType, pPackType, pNumPacks, pTransClass, pVolume, pWeight, pValue, pComments, pConfirmationType, pUserId, pStopTaskId)
            Me.RouteStopTask.Add(oRouteStopDet)
        End If
        Return oRouteStopDet
    End Function

    Public Sub AddStopDetail(ByVal oRouteStopDet As RouteStopTask)
        Me.RouteStopTask.Add(oRouteStopDet)
    End Sub

    Public Function RemoveStopTask(ByVal oStopDetail As RouteStopTask, ByVal pUser As String)
        'first update the status of the route stop detail to canceled
        oStopDetail.Delete(pUser)
        Me.RouteStopTask = New RouteStopTaskCollection(Me)
        'now check if we shoud cancel the route stop as well
        If Me.RouteStopTask.Count = 0 Then
            Delete()
        End If
    End Function

    Public Function CancelStopDetail(ByVal oStopDetail As RouteStopTask, ByVal pUser As String)
        'first update the status of the route stop detail to canceled
        oStopDetail.Cancel(pUser)
        Me.RouteStopTask = New RouteStopTaskCollection(Me)
        'now check if we shoud cancel the route stop as well
        For Each tmpRouteStopDet As RouteStopTask In Me.RouteStopTask
            If tmpRouteStopDet.Status <> StopTaskStatus.Canceled Then
                Exit Function
            End If
        Next
        SetStatus(StopStatus.Canceled, pUser)
    End Function

#End Region

#Region "Add/Remove Stop General Task"

    Public Function AddGeneralTask(ByVal pTaskId As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pUserId As String) As RouteStopTask
        Dim oRouteTask As RouteGeneralTask
        Dim oRouteStopTask As RouteStopTask
        If Me.RouteStopTask.RouteStopGeneralTaskExists(pTaskId) Then
            oRouteStopTask = Me.RouteStopTask.GetGeneralTask(pTaskId)
        Else
            'Create the route stop tasks that relate to the task
            oRouteTask = New RouteGeneralTask(pTaskId)
            oRouteStopTask = AddStopDetail(StopTaskType.General, oRouteTask.ScheduleDate, oRouteTask.Consignee, oRouteTask.TaskId, oRouteTask.TaskType, oRouteTask.Company, oRouteTask.CompanyType, oRouteTask.ContactId, "", "", 0, "", 0, 0, 0, oRouteTask.Notes, pConfirmationType, pUserId)
            Me.RouteStopTask.Add(oRouteStopTask)
        End If
        Return oRouteStopTask
    End Function

    Public Function AddChkPntTask(ByVal pTaskId As String, ByVal pConfirmationType As StopTaskConfirmationType, ByVal pUserId As String) As RouteStopTask
        Dim oRouteTask As RouteGeneralTask
        Dim oRouteStopTask As RouteStopTask
        If Me.RouteStopTask.RouteStopGeneralTaskExists(pTaskId) Then
            oRouteStopTask = Me.RouteStopTask.GetGeneralTask(pTaskId)
        Else
            'Create the route stop tasks that relate to the task
            oRouteTask = New RouteGeneralTask(pTaskId)
            oRouteStopTask = AddStopDetail(StopTaskType.ChkPnt, oRouteTask.ScheduleDate, oRouteTask.Consignee, oRouteTask.TaskId, oRouteTask.TaskType, oRouteTask.Company, oRouteTask.CompanyType, oRouteTask.ContactId, "", "", 0, "", 0, 0, 0, oRouteTask.Notes, pConfirmationType, pUserId)
            Me.RouteStopTask.Add(oRouteStopTask)
        End If
        Return oRouteStopTask
    End Function


    Public Sub RemoveGeneralTask(ByVal pTaskId As String, ByVal pUserId As String)
        Dim oRouteTask As RouteGeneralTask
        Dim oRouteStopTask As RouteStopTask
        If Me.RouteStopTask.RouteStopGeneralTaskExists(pTaskId) Then
            oRouteStopTask = Me.RouteStopTask.GetGeneralTask(pTaskId)
            oRouteStopTask.GeneralTask.Cancel("", pUserId)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Remove Task - Task Does not exists", "Cannot Remove Task - Task Does not exists")
        End If
    End Sub

#End Region

#Region "Statuses"

    Protected Sub SetStatus(ByVal pNewStatus As StopStatus, ByVal pUser As String)
        _status = pNewStatus
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update routestop set STATUS={0},EDITDATE={1}, EDITUSER={2} Where {3}", _
            Made4Net.Shared.Util.FormatField(StopStatusToString(_status)), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

    Public Function Cancel(ByVal pUser As String)
        'Update all stopdetails
        For Each oStopDetail As RouteStopTask In Me.RouteStopTask
            If oStopDetail.Status <> StopTaskStatus.Completed Then
                oStopDetail.Cancel(pUser)
            End If
        Next
        Me.RouteStopTask = New RouteStopTaskCollection(Me)
        'now check if we shoud cancel the route stop as well
        For Each tmpRouteStopDet As RouteStopTask In Me.RouteStopTask
            If tmpRouteStopDet.Status <> StopTaskStatus.Canceled Then
                Exit Function
            End If
        Next
        SetStatus(StopStatus.Canceled, pUser)
    End Function

    Public Function Confirm(ByVal pUser As String)
        'Update all stopdetails
        For Each oStopDetail As RouteStopTask In Me.RouteStopTask
            If oStopDetail.Status <> StopTaskStatus.Canceled Then
                oStopDetail.Confirm(pUser)
            End If
        Next
        SetStatus(StopStatus.Completed, pUser)
    End Function

#End Region

#Region "Actual Arrival / Departure Time"

    Public Sub SetActualArrivalDate(ByVal pArrivalDate As DateTime, ByVal pUserId As String)
        _actualarrivaldate = pArrivalDate
        _editdate = DateTime.Now
        _edituser = pUserId

        Dim sSql As String = String.Format("update ROUTESTOP set ACTUALARRIVALDATE={0},EDITDATE={1},EDITUSER={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_actualarrivaldate), Made4Net.Shared.Util.FormatField(_editdate), _
                Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sSql)
    End Sub

    Public Sub SetActualDepartureDate(ByVal pDepartureDate As DateTime, ByVal pUserId As String)
        _actualdeparturedate = pDepartureDate
        _editdate = DateTime.Now
        _edituser = pUserId

        Dim sSql As String = String.Format("update ROUTESTOP set ACTUALDEPARTUREDATE={0},EDITDATE={1},EDITUSER={2} where {3}", _
                Made4Net.Shared.Util.FormatField(_actualdeparturedate), Made4Net.Shared.Util.FormatField(_editdate), _
                Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sSql)
    End Sub

#End Region

#Region "Enum Conversion"

    Protected Function StopStatusToString(ByVal sds As StopStatus) As String
        Select Case sds
            Case StopStatus.Assigned
                Return "ASSIGNED"
            Case StopStatus.Canceled
                Return "CANCELED"
            Case StopStatus.Completed
                Return "COMPLETED"
        End Select
    End Function

    Protected Function StopStatusFromString(ByVal sStopStatus As String) As StopStatus
        Select Case sStopStatus.ToLower
            Case "assigned"
                Return StopStatus.Assigned
            Case "canceled"
                Return StopStatus.Canceled
            Case "completed"
                Return StopStatus.Completed
        End Select
    End Function

#End Region

#End Region

End Class

#Region "ENUMS"

Public Enum StopStatus
    Assigned = 1
    Completed = 2
    Canceled = 3
End Enum

#End Region
