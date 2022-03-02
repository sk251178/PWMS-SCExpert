Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports System.Text
Imports WMS.Logic

#Region "WAVE"

' <summary>
' This object represents the properties and methods of a WAVE.
' </summary>

'Changes:
'19-12-05
'priel Cancel method
<CLSCompliant(False)> Public Class Wave

#Region "Inner Classes"

#Region "Wave Details Collection"

    <CLSCompliant(False)> Public Class WaveDetailsCollection
        Inherits ArrayList

#Region "Variables"

        Protected _wave As String

#End Region

#Region "Properties"

        Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As WaveDetail
            Get
                Return CType(MyBase.Item(index), WaveDetail)
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            'Default Empty Ctor
        End Sub

        Public Sub New(ByVal pWaveId As String, Optional ByVal LoadAll As Boolean = True)
            _wave = pWaveId
            Dim SQL As String
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select * from wavedetail where WAVE = '" & _wave & "'"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                Me.add(New WaveDetail(dr))
            Next
        End Sub

#End Region

#Region "Methods"

        Public Function getWaveDetail(ByVal Consignee As String, ByVal OrderId As String, ByVal pOrderLine As Int32) As WaveDetail
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).CONSIGNEE = Consignee.ToUpper And Item(i).ORDERID.ToUpper = OrderId.ToUpper And Item(i).ORDERLINE = pOrderLine Then
                    Return (CType(MyBase.Item(i), WaveDetail))
                End If
            Next
            Return Nothing
        End Function

        Public Shadows Function add(ByVal pObject As WaveDetail) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As WaveDetail)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As WaveDetail)
            MyBase.Remove(pObject)
        End Sub

#End Region

    End Class

#End Region

#Region "Outbound Order Collection"

    '    <CLSCompliant(False)> Public Class OutboundOrderCollection
    '        Inherits ArrayList

    '#Region "Variables"

    '        Protected _wave As String

    '#End Region

    '#Region "Properties"

    '        Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As OutboundOrderHeader
    '            Get
    '                Return CType(MyBase.Item(index), OutboundOrderHeader)
    '            End Get
    '        End Property

    '#End Region

    '#Region "Constructors"

    '        Public Sub New(ByVal pWaveId As String, Optional ByVal LoadAll As Boolean = True)
    '            _wave = pWaveId
    '            Dim SQL As String
    '            Dim dt As New DataTable
    '            Dim dr As DataRow
    '            SQL = "Select * from OUTBOUNDORHEADER where WAVE = '" & _wave & "'"
    '            DataInterface.FillDataset(SQL, dt)
    '            For Each dr In dt.Rows
    '                Me.add(New OutboundOrderHeader(dr))
    '            Next
    '        End Sub

    '#End Region

    '#Region "Methods"

    '        Public Function getOrder(ByVal Consignee As String, ByVal OrderId As String) As OutboundOrderHeader
    '            Dim i As Int32
    '            For i = 0 To Me.Count - 1
    '                If Item(i).CONSIGNEE = Consignee.ToUpper And Item(i).ORDERID = OrderId.ToUpper Then
    '                    Return (CType(MyBase.Item(i), OutboundOrderHeader))
    '                End If
    '            Next
    '            Return Nothing

    '        End Function

    '        Public Shadows Function add(ByVal pObject As OutboundOrderHeader) As Integer
    '            Return MyBase.Add(pObject)
    '        End Function

    '        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As OutboundOrderHeader)
    '            MyBase.Insert(index, Value)
    '        End Sub

    '        Public Shadows Sub Remove(ByVal pObject As OutboundOrderHeader)
    '            MyBase.Remove(pObject)
    '        End Sub

    '#End Region

    '    End Class

#End Region

#End Region

#Region "Variables"

#Region "Primary Keys"

    Protected _wave As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _createdate As DateTime
    Protected _releasedate As DateTime
    Protected _status As String = String.Empty
    Protected _wavetype As String = String.Empty
    Protected _adddate As DateTime
    Protected _closedate As DateTime

    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _wavedetails As WaveDetailsCollection
    'Protected _orders As OutboundOrdersCollection

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " WAVE = '" & _wave & "'"
        End Get
    End Property

    Public ReadOnly Property WAVE() As String
        Get
            Return _wave
        End Get
    End Property

    Public Property CREATEDATE() As DateTime
        Get
            Return _createdate
        End Get
        Set(ByVal Value As DateTime)
            _createdate = Value
        End Set
    End Property

    Public Property RELEASEDATE() As DateTime
        Get
            Return _releasedate
        End Get
        Set(ByVal Value As DateTime)
            _releasedate = Value
        End Set
    End Property

    Public Property STATUS() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property WAVETYPE() As String
        Get
            Return _wavetype
        End Get
        Set(ByVal Value As String)
            _wavetype = Value
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
    Public Property CLOSEDATE() As DateTime
        Get
            Return _closedate
        End Get
        Set(ByVal Value As DateTime)
            _closedate = Value
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

    'Public ReadOnly Property Orders() As OutboundOrderCollection
    '    Get
    '        Return _orders
    '    End Get
    'End Property

    Public ReadOnly Property WaveDetails() As WaveDetailsCollection
        Get
            Return _wavedetails
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pWAVE As String, Optional ByVal LoadObj As Boolean = True)
        _wave = pWAVE
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        If CommandName.ToLower = "save" Then
            Me.Create(ds.Tables(0).Rows(0)("WAVE"), ds.Tables(0).Rows(0)("WAVETYPE"), Common.GetCurrentUser)
        ElseIf CommandName = "DeAssignOrders" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            _wave = ds.Tables(0).Rows(0)("WAVE")
            Load()
            For Each dr In ds.Tables(0).Rows
                'DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser())
                UnAssignOrderLine(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Common.GetCurrentUser())
            Next
        ElseIf CommandName = "AssignOrders" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            _wave = ds.Tables(0).Rows(0)("WAVEID")
            Load()
            For Each dr In ds.Tables(0).Rows
                'AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser())
                AssignOrderLine(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), False, Common.GetCurrentUser())
            Next
        ElseIf CommandName.ToLower = "plan" Then
            Dim allPlanned As Boolean = True
            Dim orderWithoutStagingLane As String = String.Empty
            Dim waveWithoutStagingLane As String = String.Empty

            For Each dr In ds.Tables(0).Rows
                _wave = dr("WAVE")
                Load()
                CalibrateSoftAlloc()
                If DoAllWaveDetailsHaveAStagingLane(orderWithoutStagingLane) Then
                    Plan(False, Common.GetCurrentUser())
                Else
                    allPlanned = False
                    If String.IsNullOrEmpty(_wave) Then
                        waveWithoutStagingLane = _wave
                    End If
                End If
            Next
            If Not allPlanned Then
                Message = String.Format("Not all wave could be planned, since staging lane couldnot be dervied for some of the orders belonging to selected waves. ({0} - {1} )", waveWithoutStagingLane, orderWithoutStagingLane)
            Else
                Message = "Planning Wave/s"
            End If
        ElseIf CommandName.ToLower = "softpaln" Then
            For Each dr In ds.Tables(0).Rows
                _wave = dr("WAVE")
                Load()
                CalibrateSoftAlloc()
                Plan(False, Common.GetCurrentUser, True)
            Next
            Message = "Soft Planning Wave/s"
        ElseIf CommandName.ToLower = "cancelwave" Then
            For Each dr In ds.Tables(0).Rows
                _wave = dr("WAVE")
                Load()
                Cancel(Common.GetCurrentUser())
            Next
        ElseIf CommandName.ToLower = "release" Then
            For Each dr In ds.Tables(0).Rows
                _wave = dr("WAVE")
                Load()
                Release(Common.GetCurrentUser())
            Next
            Message = "Releasing Wave/s"
        ElseIf CommandName.ToLower = "complete" Then
            For Each dr In ds.Tables(0).Rows
                _wave = dr("WAVE")
                Load()
                Complete(Common.GetCurrentUser)
            Next
            Message = "Completing Wave/s"
        ElseIf CommandName.ToLower = "printpicklist" Then
            For Each dr In ds.Tables(0).Rows
                _wave = dr("WAVE")
                Load()
                PrintPickLists(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
            Next
        ElseIf CommandName.ToLower = "setpririty" Then
            'RWMS-823 Validate Priority can not be zero or negative Start
            If ds.Tables(0).Rows(0)("PRIORITY") = 0 Or ds.Tables(0).Rows(0)("PRIORITY") <= -1 Then
                Throw New ApplicationException("Priority can not be Zero or Negative value")
                Return
            End If
            'RWMS-823 Validate Priority can not be zero or negative End

            Dim flag As Boolean = False
            For Each dr In ds.Tables(0).Rows
                ChangeTaskPriority(dr("TASK"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("USERID"), ""), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PRIORITY"), -1), Common.GetCurrentUser)
            Next
            Message = "Task was updated successfully"
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetWAVE(ByVal pWAVE As String) As Wave
        Return New Wave(pWAVE)
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM WAVE WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("RELEASEDATE") Then _releasedate = dr.Item("RELEASEDATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("WAVETYPE") Then _wavetype = dr.Item("WAVETYPE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("CLOSEDATE") Then _closedate = dr.Item("CLOSEDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        '_orders = New OutboundOrderCollection(_wave)
        _wavedetails = New WaveDetailsCollection(_wave)

    End Sub

    Public Shared Function Exists(ByVal pWaveId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from WAVE where WAVE = '{0}'", pWaveId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub SetStatus(ByVal paramStatus As String, ByVal pUser As String)
        _status = paramStatus
        _edituser = pUser
        _editdate = DateTime.Now
        Dim SQL As String
        Dim outboundord As OutboundOrderHeader
        If paramStatus = WMS.Lib.Statuses.Wave.PLANNED Then
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select distinct Consignee,Orderid from wavedetail where WAVE = '" & _wave & "'"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                outboundord = New OutboundOrderHeader(dr("consignee"), dr("orderid"))
                outboundord.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.RELEASED, pUser)
            Next
        End If
        SQL = String.Format("Update WAVE SET STATUS = {0},EditUser = {1},EditDate = {2} Where {3}",
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Create"

    Public Sub Create(ByVal pWaveId As String, ByVal pWaveType As String, ByVal pUser As String)

        If pWaveId Is Nothing Or pWaveId.Trim() = "" Then
            _wave = Made4Net.Shared.Util.getNextCounter("WAVE")
        Else
            If Exists(pWaveId) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Wave Already Exists", "Wave Already Exists")
                Throw m4nEx
            End If
            _wave = pWaveId
        End If

        _wavetype = pWaveType
        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser
        _createdate = DateTime.Now
        _status = WMS.Lib.Statuses.Wave.STATUSNEW

        Dim sql As String = String.Format("INSERT INTO WAVE(WAVE,CREATEDATE,RELEASEDATE,STATUS,WAVETYPE,CLOSEDATE,ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES (" &
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", Made4Net.Shared.Util.FormatField(_wave), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_releasedate), Made4Net.Shared.Util.FormatField(_status),
            Made4Net.Shared.Util.FormatField(_wavetype), Made4Net.Shared.Util.FormatField(_closedate), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)

        'Dim em As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.WaveCreated
        'em.Add("EVENT", EventType)
        'em.Add("WAVE", _wave)
        'em.Add("USERID", _adduser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WaveCreated)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WAVEINS)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _wave)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.WAVEINS)

        _wavedetails = New WaveDetailsCollection(_wave)
        '_orders = New OutboundOrderCollection(_wave)
    End Sub

#End Region

#Region "Orders Assignment"

    Public Sub AssignOrderLine(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pIgnoreStatus As Boolean, ByVal pUser As String)
        If Not pIgnoreStatus AndAlso Not (_status = WMS.Lib.Statuses.Wave.ASSIGNED OrElse _status = WMS.Lib.Statuses.Wave.STATUSNEW OrElse
        _status = WMS.Lib.Statuses.Wave.PLANNED OrElse _status = WMS.Lib.Statuses.Wave.RELEASED) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Wave Status", "Incorrect Wave Status")
            Throw m4nEx
        End If
        Dim oWaveDetail As New WaveDetail
        oWaveDetail.Create(_wave, Consignee, OrdId, pOrderLine, pUser)
        _wavedetails.add(oWaveDetail)
        _editdate = DateTime.Now
        _edituser = pUser
        If Me.STATUS = WMS.Lib.Statuses.Wave.STATUSNEW Then
            _status = WMS.Lib.Statuses.Wave.ASSIGNED
            DataInterface.RunSQL(String.Format("Update WAVE set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        Else
            DataInterface.RunSQL(String.Format("Update WAVE set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        End If
        'If _status = WMS.Lib.Statuses.Wave.ASSIGNED Or _status = WMS.Lib.Statuses.Wave.STATUSNEW Or _status = WMS.Lib.Statuses.Wave.PLANNED Or _status = WMS.Lib.Statuses.Wave.RELEASED Then
        '    Dim oWaveDetail As New WaveDetail
        '    oWaveDetail.Create(_wave, Consignee, OrdId, pOrderLine, pUser)
        '    _wavedetails.add(oWaveDetail)
        '    _editdate = DateTime.Now
        '    _edituser = pUser
        '    If Me.STATUS = WMS.Lib.Statuses.Wave.STATUSNEW Then
        '        _status = WMS.Lib.Statuses.Wave.ASSIGNED
        '        DataInterface.RunSQL(String.Format("Update WAVE set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        '    Else
        '        DataInterface.RunSQL(String.Format("Update WAVE set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        '    End If
        'Else
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Wave Status", "Incorrect Wave Status")
        '    Throw m4nEx
        'End If

    End Sub

    Public Sub UnAssignOrderLine(ByVal Consignee As String, ByVal OrdId As String, ByVal pOrderLine As Int32, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Wave.ASSIGNED Or _status = WMS.Lib.Statuses.Wave.PLANNED Or _status = WMS.Lib.Statuses.Wave.RELEASED Then
            Dim wd As WaveDetail
            wd = Me.WaveDetails.getWaveDetail(Consignee, OrdId, pOrderLine)
            wd.Delete()
            Me.WaveDetails.Remove(wd)
            _editdate = DateTime.Now
            _edituser = pUser
            If WaveDetails.Count = 0 Then
                _status = WMS.Lib.Statuses.Wave.STATUSNEW
                DataInterface.RunSQL(String.Format("Update WAVE set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            Else
                DataInterface.RunSQL(String.Format("Update WAVE set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            End If
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Wave Status deassign", "Incorrect Wave Status deassign")
            Throw m4nEx
        End If
    End Sub

#End Region

#Region "Complete"

    Public Sub Complete(ByVal puser As String)
        Dim sql As String
        'Dim rq As New EventManagerQ
        Dim action As String = WMS.Lib.Actions.Audit.COMPLETEWAVE

        If _status = WMS.Lib.Statuses.Wave.PLANNED Or _status = WMS.Lib.Statuses.Wave.RELEASED Or _status = WMS.Lib.Statuses.Wave.COMPLETING Then
            SetStatus(WMS.Lib.Statuses.Wave.COMPLETING, puser)


            Dim WvDetail As WaveDetail
            Dim ordDetail As OutboundOrderHeader.OutboundOrderDetail
            Dim ordheader As OutboundOrderHeader
            Dim dt As DataTable
            Dim dr As DataRow
            Dim pld As PicklistDetail
            Dim pl As Picklist

            For Each WvDetail In Me.WaveDetails
                ordDetail = New OutboundOrderHeader.OutboundOrderDetail(WvDetail.CONSIGNEE, WvDetail.ORDERID, WvDetail.ORDERLINE)
                ordheader = New OutboundOrderHeader(WvDetail.CONSIGNEE, WvDetail.ORDERID)

                ' sql = String.Format("select distinct picklist from PICKHEADER where (status = {0} or status = {1} or status = {2}) and wave = {3} ", Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.PARTPICKED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.PLANNED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.RELEASED), Made4Net.Shared.Util.FormatField(WvDetail.WAVE))
                '----
                sql = String.Format("SELECT DISTINCT dbo.PICKHEADER.PICKLIST, dbo.PICKDETAIL.PICKLISTLINE, dbo.PICKDETAIL.PICKREGION, dbo.PICKDETAIL.CONSIGNEE, dbo.PICKDETAIL.ORDERID," &
                                        " dbo.PICKDETAIL.ORDERLINE, dbo.PICKDETAIL.STATUS, dbo.PICKDETAIL.SKU, dbo.PICKDETAIL.UOM, dbo.PICKDETAIL.QTY, dbo.PICKDETAIL.ADJQTY," &
                                        " dbo.PICKDETAIL.PICKEDQTY, dbo.PICKDETAIL.FROMLOCATION, dbo.PICKDETAIL.FROMLOAD, dbo.PICKDETAIL.TOLOAD, dbo.PICKDETAIL.TOCONTAINER," &
                                        " dbo.PICKDETAIL.TOLOCATION, dbo.PICKDETAIL.ADDUSER, dbo.PICKDETAIL.EDITDATE, dbo.PICKDETAIL.EDITUSER, dbo.PICKDETAIL.FROMWAREHOUSEAREA," &
                                        " dbo.PICKDETAIL.TOWAREHOUSEAREA, dbo.PICKHEADER.WAVE,  dbo.PICKDETAIL.ADDDATE" &
                                    " FROM   dbo.PICKHEADER INNER JOIN dbo.PICKDETAIL " &
                                        "ON dbo.PICKHEADER.PICKLIST = dbo.PICKDETAIL.PICKLIST " &
                                    "WHERE (dbo.PICKHEADER.STATUS in({0},{1},{2}) AND (dbo.PICKHEADER.WAVE ={3} ))", Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.PARTPICKED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.PLANNED), Made4Net.Shared.Util.FormatField(WMS.Lib.Statuses.Picklist.RELEASED), Made4Net.Shared.Util.FormatField(WvDetail.WAVE))


                dt = New DataTable()
                DataInterface.FillDataset(sql, dt)
                For Each dr In dt.Rows
                    pld = New PicklistDetail(dr)
                    pld.Cancel(puser)


                    pl = New Picklist(dr("PICKLIST"))
                    If pl.isCompleted Or pl.isCanceled Then
                        pl.CompleteList(puser, Nothing)
                    End If
                Next
                '--
                ordDetail.Complete(puser)
                Dim NextOrderStatus As String = ""
                NextOrderStatus = ordheader.getOrderStatusByActivity()

                If Not String.IsNullOrEmpty(NextOrderStatus) Then
                    ordheader.setStatus(NextOrderStatus, puser)
                End If
                '--


            Next


            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WaveCompleted)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.COMPLETEWAVE)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _wave)
            aq.Add("DOCUMENTLINE", "")
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", puser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", puser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", puser)
            aq.Send(WMS.Lib.Actions.Audit.COMPLETEWAVE)


            SetStatus(WMS.Lib.Statuses.Wave.COMPLETE, puser)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Wave status is incorrect ", "Wave status is incorrect ")
            Throw m4nEx
        End If
    End Sub

#End Region

#Region "Cancel"

    Public Sub Cancel(ByVal puser As String)
        'Dim rq As New EventManagerQ
        'Dim action As String = WMS.Lib.Actions.Audit.CANWAVE

        Select Case _status
            Case WMS.Lib.Statuses.Wave.CANCELED
                Throw New Made4Net.Shared.M4NException(New Exception(), "Incorrect status for canceling.", "Incorrect status for canceling.")
            Case WMS.Lib.Statuses.Wave.STATUSNEW
                SetStatus(WMS.Lib.Statuses.Wave.CANCELED, puser)

                'rq.Add("ACTION", action)
                'rq.Add("USERID", puser)
                'rq.Add("DATE", DateTime.Now.ToString())
                'rq.Add("WAVE", _wave)
                'rq.Send(WMS.Lib.Actions.Audit.CANWAVE)


                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WaveCanceled)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CANWAVE)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", "")
                aq.Add("DOCUMENT", _wave)
                aq.Add("DOCUMENTLINE", "")
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMWAREHOUSEAREA", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", _status)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOWAREHOUSEAREA", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", puser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", puser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", puser)
                aq.Send(WMS.Lib.Actions.Audit.CANWAVE)

            Case WMS.Lib.Statuses.Wave.ASSIGNED
                Dim dt As DataTable = New DataTable()
                DataInterface.FillDataset("SELECT * FROM wavedetail WHERE WAVE='" & _wave & "'", dt)
                For Each dr As DataRow In dt.Rows
                    Dim wavedet As WaveDetail = New WaveDetail(dr)
                    UnAssignOrderLine(wavedet.CONSIGNEE, wavedet.ORDERID, wavedet.ORDERLINE, puser)
                    wavedet.Delete()
                Next
                SetStatus(WMS.Lib.Statuses.Wave.CANCELED, puser)

                'rq.Add("ACTION", action)
                'rq.Add("USERID", puser)
                'rq.Add("DATE", DateTime.Now.ToString())
                'rq.Add("WAVE", _wave)
                'rq.Send(WMS.Lib.Actions.Audit.CANWAVE)
                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WaveAssigned)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WAVEASGN)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", "")
                aq.Add("DOCUMENT", _wave)
                aq.Add("DOCUMENTLINE", "")
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMWAREHOUSEAREA", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", _status)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOWAREHOUSEAREA", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", puser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", puser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", puser)
                aq.Send(WMS.Lib.Actions.Audit.WAVEASGN)
                'Case Else
                '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Wave status is incorrect for canceling", "Wave status is incorrect for canceling")
                '    Throw m4nEx
        End Select

    End Sub

#End Region

#Region "Print pick lists"

    Public Function PrintPickLists(ByVal LanguageId As Int32, ByVal UserId As String)
        If _status <> WMS.Lib.Statuses.Wave.PLANNED Or _status <> WMS.Lib.Statuses.Wave.RELEASED Then

        End If
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = WMS.Lib.REPORTS.PICKLISTS
        'DataInterface.FillDataset(String.Format("Select * from reports where reportname = '{0}'", repType), dt)

        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("LANGUAGE", LanguageId)
        oQsender.Add("USERID", UserId)
        oQsender.Add("PRINTER", "")
        Try
            'RWMS-2609 - replaced the db connection from sys to app
            Dim numCopies As Integer = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select ParamValue from sys_reportparams where ReportID='{0}' and ParamName='COPIES'", repType))
            oQsender.Add("COPIES", numCopies) ' dt.Rows(0)("COPIES"))
        Catch ex As Exception
            oQsender.Add("COPIES", 1) ' dt.Rows(0)("COPIES"))
        End Try
        oQsender.Add("WHERE", String.Format("ph.Wave = '{0}'", _wave))
        oQsender.Send("Report", repType)
    End Function

#End Region

#Region "Plan & Release"

    Public Sub CalibrateSoftAlloc()
        Dim oOutOrd As OutboundOrderHeader
        'For Each oOutOrd In Me.Orders
        '    oOutOrd.CalibrateSoftAlloc()
        'Next
    End Sub

    Public Sub Plan(ByVal doRelease As Boolean, ByVal pUser As String, Optional ByVal doSoftPlan As Boolean = False)
        If _status = WMS.Lib.Statuses.Wave.ASSIGNED Or _status = WMS.Lib.Statuses.Wave.PLANNED Or _status = WMS.Lib.Statuses.Wave.RELEASED Then
            If Not doSoftPlan Then
                SetStatus(WMS.Lib.Statuses.Wave.PLANNING, pUser)
            End If
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.PLANWAVE)
            qh.Values.Add("DORELEASE", doRelease.ToString())
            qh.Values.Add("WAVE", _wave)
            qh.Values.Add("SOFTPALN", doSoftPlan)
            qh.Values.Add("USER", pUser)
            qh.Send("Planner", _wave)


            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WavePlan)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PLANWAVE)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _wave)
            aq.Add("DOCUMENTLINE", "")
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.PLANWAVE)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Wave Status, Cannot Plan Picks", "Incorrect Wave Status, Cannot Plan Picks")
            Throw m4nEx
        End If
    End Sub

    Public Sub PlanComplete(Optional ByVal oLogger As LogHandler = Nothing)
        _status = WMS.Lib.Statuses.Wave.PLANNED
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now

        'update all orders to planned status
        OrdersPlanComplete(WMS.Lib.USERS.SYSTEMUSER)

        Dim sql As String = String.Format("Update WAVE SET STATUS = {0},EditUser = {1},EditDate = {2} Where {3}",
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)
        Dim OrderDao As New OrderDao
        Dim dt As DataTable = OrderDao.GetAllOrdersForWave(_wave)
        Dim orderList As New StringBuilder
        Dim consignee As String
        For Each dr As DataRow In dt.Rows
            orderList.Append(dr("ORDERID"))
            orderList.Append(",")
            'Assume all orders in Wave belong to one consignee
            consignee = Convert.ToString(dr("CONSIGNEE"))
        Next
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WavePlaned)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", "WAVEPLANED")
        aq.Add("CONSIGNEE", consignee)
        aq.Add("DOCUMENT", _wave)
        'Commented for RWMS-2521
        'aq.Add("DOCUMENTLINE", orderList.ToString().TrimEnd(","))
        'End 'Commented for RWMS-2521
        'Added for RWMS-2521
        aq.Add("DOCUMENTLINE", "")
        'Added for RWMS-2521
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", "")
        aq.Add("FROMSTATUS", "")
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("LASTSTATUSUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("LASTCOUNTUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("REASONCODE", "")
        aq.Add("ADDUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Lib.USERS.SYSTEMUSER)
        aq.Send("WAVEPLANED")
    End Sub

    Public Sub Release(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Wave.ASSIGNED Then
            Plan(True, pUser)
        Else
            If _status = WMS.Lib.Statuses.Wave.PLANNED Or _status = WMS.Lib.Statuses.Wave.RELEASED Then
                SetStatus(WMS.Lib.Statuses.Wave.RELEASING, pUser)
                Dim qh As New Made4Net.Shared.QMsgSender
                qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.RELEASEWAVE)
                qh.Values.Add("WAVE", _wave)
                qh.Values.Add("USER", pUser)
                qh.Send("Releasor", _wave)


                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WaveRelease)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RELEASEWAVE)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", "")
                aq.Add("DOCUMENT", _wave)
                aq.Add("DOCUMENTLINE", "")
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMWAREHOUSEAREA", "")
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", _status)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOWAREHOUSEAREA", "")
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", pUser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", pUser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", pUser)
                aq.Send(WMS.Lib.Actions.Audit.RELEASEWAVE)
            Else
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Wave Status, Cannot Plan Picks", "Incorrect Wave Status, Cannot Release wave")
                Throw m4nEx
            End If
        End If
    End Sub

    Public Sub ReleaseComplete()
        _status = WMS.Lib.Statuses.Wave.RELEASED
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now
        _releasedate = DateTime.Now
        Dim outboundord As OutboundOrderHeader
        'For Each outboundord In Me.Orders
        '    If outboundord.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.PLANNED Then
        '        outboundord.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.RELEASED, WMS.Lib.USERS.SYSTEMUSER)
        '    End If
        'Next
        'Commented for PWMS-782(RWMS-837) Start
        'Dim sql As String = String.Format("Update WAVE SET STATUS = {0},EditUser = {1},EditDate = {2}, RELEASEDATE= {3} Where {4}", _
        '    Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_releasedate), WhereClause)
        'Commented for PWMS-782(RWMS-837) End

        'Added for PWMS-782(RWMS-837) Start
        Dim bUpdateReleaseDate = False
        Dim sqlVerify As String = String.Format("Select count(1) from WAVE where WAVE = {0} AND RELEASEDATE IS NULL ", Made4Net.Shared.Util.FormatField(_wave))
        bUpdateReleaseDate = Convert.ToBoolean(DataInterface.ExecuteScalar(sqlVerify))
        Dim sql As String
        If (bUpdateReleaseDate = True) Then
            sql = String.Format("Update WAVE SET STATUS = {0},EditUser = {1},EditDate = {2}, RELEASEDATE= {3} Where {4}",
           Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_releasedate), WhereClause)
        Else
            sql = String.Format("Update WAVE SET STATUS = {0},EditUser = {1},EditDate = {2} Where {3}",
           Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        End If
        'Added for PWMS-782(RWMS-837) End
        DataInterface.RunSQL(sql)
    End Sub

    Private Sub OrdersPlanComplete(ByVal pUserId As String)
        Dim sql As String
        'sql = String.Format("select distinct consignee,orderid from wavedetail where wave = '{0}'", _wave)
        ' sql = "Select oh.* from wavedetail orq inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = orq.CONSIGNEE and orq.ORDERID = oh.ORDERID where WAVE = '" & _wave & "'"
        'RWMS-2715 Note:- We raise event 19 only one time for each order id, that is why we updated distinct in below SQL
        sql = "Select distinct oh.consignee,oh.orderid from wavedetail orq inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = orq.CONSIGNEE and orq.ORDERID = oh.ORDERID where WAVE = '" & _wave & "'"
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        Dim outOrd As OutboundOrderHeader
        For Each dr As DataRow In dt.Rows
            outOrd = WMS.Logic.Planner.GetOutboundOrder(dr("consignee"), dr("orderid"))
            If outOrd Is Nothing Then
                Continue For
            End If
            'If outOrd Is Nothing Then
            '    outOrd = New OutboundOrderHeader(dr)
            'End If
            'RWMS-2715 note:-Allow the update outboundorder header status even the the order is PLANNED, to allow Wave replanning
            If outOrd.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.STATUSNEW Or outOrd.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.WAVEASSIGNED Or outOrd.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.ROUTINGSETASSIGNED Or outOrd.STATUS = WMS.Lib.Statuses.OutboundOrderHeader.PLANNED Then
                outOrd.setStatus(WMS.Lib.Statuses.OutboundOrderHeader.PLANNED, pUserId)
            End If
        Next
    End Sub

    Public Sub CancelExceptions(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Wave.CANCELED OrElse _status = WMS.Lib.Statuses.Wave.COMPLETE OrElse _status = WMS.Lib.Statuses.Wave.PLANNING Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wave status Incorrect", "Wave status Incorrect")
        End If
        'Dim orderheader As OutboundOrderHeader
        'For Each orderheader In Me.Orders
        '    orderheader.CancelExceptions(pUser)
        'Next

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WaveCancelException)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.WAVECANCEX)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", _wave)
        aq.Add("DOCUMENTLINE", "")
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", "")
        aq.Add("FROMWAREHOUSEAREA", "")
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", "")
        aq.Add("TOWAREHOUSEAREA", "")
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("CLOSEDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.WAVECANCEX)
    End Sub

#End Region

#Region "Change Task Priority"

    Private Function ChangeTaskPriority(ByVal pTaskId As String, ByVal pUserId As String, ByVal pPriority As Int32, ByVal pUser As String)
        Dim SQL As String
        Dim dt As New DataTable
        Dim oTask As New Task(pTaskId)
        If oTask.STATUS = WMS.Lib.Statuses.Task.CANCELED Or oTask.STATUS = WMS.Lib.Statuses.Task.COMPLETE Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Task Status Incorrect", "Task Status Incorrect")
        Else
            If pUserId <> String.Empty Then
                oTask.AssignUser(pUserId)
            End If
            If pPriority > -1 Then
                oTask.SetPriority(pPriority)
            End If
        End If
    End Function

#End Region

#Region "Wave Assignment"

    'added for PWMS-419 Start
    Public Shared Function Delete(ByVal pTemplateName As String)
        Dim oSql As String = String.Format("delete from WAVEASSIGNMENT where TEMPLATENAME='{0}'", pTemplateName)
        DataInterface.RunSQL(oSql)
    End Function

    'added for PWMS-419 End
#End Region

    'Begin RWMS-2599
    Public Sub CheckException(ByVal userId As String, ByVal oLogger As LogHandler)
        Try
            If Not oLogger Is Nothing Then
                oLogger.Write(" Plan Wave complete checking for exceptions ")
            End If
            'RWMS-2682
            Dim ProcessedConsigneeOrder As Hashtable = New Hashtable()
            Dim hkey As Integer = 0
            Dim owavedetail As WaveDetail
            For Each owavedetail In Me.WaveDetails
                'Check if the consignee orderid in the wavedetail has already been processed for exceptions
                If Not ProcessedConsigneeOrder.ContainsValue(owavedetail.CONSIGNEE + "-" + owavedetail.ORDERID) Then
                    Dim ord As OutboundOrderHeader = New OutboundOrderHeader(owavedetail.CONSIGNEE, owavedetail.ORDERID, True)
                    ord.CheckException(userId, oLogger)
                    ProcessedConsigneeOrder.Add(hkey, owavedetail.CONSIGNEE + "-" + owavedetail.ORDERID)
                    hkey = hkey + 1
                End If
            Next
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(" Error occured while checking for order exception " & ex.ToString())
            End If
        End Try

    End Sub
    'End RWMS-2599

#End Region

    Private Function DoAllWaveDetailsHaveAStagingLane(ByRef orderWithoutStagingLane As String) As Boolean
        Dim isAllOdersStagingLaneExists As Boolean = True

        For Each wd As WaveDetail In WaveDetails
            If String.IsNullOrEmpty(WMS.Logic.Load.DoesOrderOrShipmentHaveStagingLane(wd.ORDERID)) Then
                isAllOdersStagingLaneExists = False
                If String.IsNullOrEmpty(orderWithoutStagingLane) Then
                    orderWithoutStagingLane = wd.ORDERID
                End If
            End If
        Next

        Return isAllOdersStagingLaneExists
    End Function

End Class

#End Region

#Region "Wave Details"

<CLSCompliant(False)> Public Class WaveDetail

#Region "Variables"

    Protected _wave As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _orderid As String = String.Empty
    Protected _orderline As Int32
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("wave = '{0}' and consignee = '{1}' and orderid = '{2}' and orderline = {3}", _wave, _consignee, _orderid, _orderline)
        End Get
    End Property

    Public Property WAVE() As String
        Set(ByVal value As String)
            _wave = value
        End Set
        Get
            Return _wave
        End Get
    End Property

    Public Property CONSIGNEE() As String
        Set(ByVal value As String)
            _consignee = value
        End Set
        Get
            Return _consignee
        End Get
    End Property

    Public Property ORDERID() As String
        Set(ByVal value As String)
            _orderid = value
        End Set
        Get
            Return _orderid
        End Get
    End Property

    Public Property ORDERLINE() As Int32
        Set(ByVal value As Int32)
            _orderline = value
        End Set
        Get
            Return _orderline
        End Get
    End Property
#End Region

#Region "Ctors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pWave As String, ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32)
        _wave = pWave
        _consignee = pConsignee
        _orderid = pOrderId
        _orderline = pOrderLine
        Load()
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Private Sub Load()
        Dim oSql As String = String.Format("select * from wavedetail where {0}", WhereClause)
        Dim dt As New DataTable
        DataInterface.FillDataset(oSql, dt)
        If dt.Rows.Count = 1 Then
            Load(dt.Rows(0))
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Wave detail does not exists", "Wave detail does not exists")
        End If
    End Sub

    Private Sub Load(ByVal dr As DataRow)
        _wave = dr("wave")
        _consignee = dr("consignee")
        _orderid = dr("orderid")
        _orderline = dr("orderline")
        _adddate = dr("adddate")
        _adduser = dr("adduser")
        _editdate = dr("editdate")
        _edituser = dr("edituser")
    End Sub

    Public Shared Function Exists(ByVal pWaveId As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from wavedetail where WAVE = '{0}' and consignee = '{1}' and orderid = '{2}' and orderline = {3}", pWaveId, pConsignee, pOrderid, pOrderLine)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Shared Function LineAssignedToAnotherWave(ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from wavedetail where consignee = '{0}' and orderid = '{1}' and orderline = {2}", pConsignee, pOrderid, pOrderLine)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

#Region "Create / Delete"

    Public Sub Create(ByVal pWave As String, ByVal pConsignee As String, ByVal pOrderid As String, ByVal pOrderLine As Int32, ByVal pUser As String)
        If LineAssignedToAnotherWave(pConsignee, pOrderid, pOrderLine) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Order Detail already assigned to another wave", "Order Detail already assigned to another wave")
        End If
        _wave = pWave
        _consignee = pConsignee
        _orderid = pOrderid
        _orderline = pOrderLine
        _adddate = DateTime.Now
        _editdate = DateTime.Now
        _adduser = pUser
        _edituser = pUser

        Dim oSql As String = String.Format("INSERT INTO WAVEDETAIL (WAVE, CONSIGNEE, ORDERID, ORDERLINE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7})",
                Made4Net.Shared.Util.FormatField(_wave), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_orderid),
                Made4Net.Shared.Util.FormatField(_orderline), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser),
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(oSql)
    End Sub

    Public Sub Delete()
        Dim oSql As String = String.Format("delete from wavedetail where {0}", WhereClause)
        DataInterface.RunSQL(oSql)
    End Sub



#End Region

#End Region

End Class

#End Region