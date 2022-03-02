Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion.Convert

<CLSCompliant(False)> Public Class RoutingSet

#Region "Inner Classes"

#Region "Outbound Order Collection"

    <CLSCompliant(False)> Public Class OutboundOrderCollection
        Inherits ArrayList

#Region "Variables"

        Protected _setid As String

#End Region

#Region "Properties"

        Public Property SetId() As String
            Get
                Return _setid
            End Get
            Set(ByVal Value As String)
                _setid = Value
            End Set
        End Property

        Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As OutboundOrderHeader
            Get
                Return CType(MyBase.Item(index), OutboundOrderHeader)
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()

        End Sub

        Public Sub New(ByVal pSetId As String, Optional ByVal LoadAll As Boolean = True)
            _setid = pSetId
            Dim SQL As String
            Dim dt As New DataTable
            Dim dr As DataRow
            SQL = "Select CONSIGNEE,ORDERID from OUTBOUNDORHEADER where routingset = '" & _setid & "'"
            DataInterface.FillDataset(SQL, dt)
            For Each dr In dt.Rows
                Me.add(New OutboundOrderHeader(dr.Item("CONSIGNEE"), dr.Item("ORDERID"), LoadAll))
            Next
        End Sub

#End Region

#Region "Methods"

        Public Function getOrder(ByVal Consignee As String, ByVal OrderId As String) As OutboundOrderHeader
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).CONSIGNEE = Consignee.ToUpper And Item(i).ORDERID = OrderId.ToUpper Then
                    Return (CType(MyBase.Item(i), OutboundOrderHeader))
                End If
            Next
            Return Nothing
        End Function

        Public Shadows Function add(ByVal pObject As OutboundOrderHeader) As Integer
            Return MyBase.Add(pObject)
        End Function

        Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As OutboundOrderHeader)
            MyBase.Insert(index, Value)
        End Sub

        Public Shadows Sub Remove(ByVal pObject As OutboundOrderHeader)
            MyBase.Remove(pObject)
        End Sub

        Public Shadows Function Clone() As Object
            Dim col As New OutboundOrderCollection
            col.SetId = _setid
            Dim i As Int32
            For i = 0 To Me.Count - 1
                col.add((CType(MyBase.Item(i), OutboundOrderHeader)))
            Next
            Return col
        End Function

#End Region

    End Class

#End Region

#End Region

#Region "Variables"

#Region "Primary Keys"

    Protected _setid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _createdate As DateTime
    Protected _plandate As DateTime
    Protected _distributiondate As DateTime
    Protected _status As String = String.Empty
    Protected _trip As String = String.Empty
    'Protected _routingpolicy As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _orders As OutboundOrderCollection

    Protected _activerunid As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " SETID = '" & _setid & "'"
        End Get
    End Property

    Public Property ActiveRunID() As String
        Get
            Return _activerunid
        End Get
        Set(ByVal value As String)
            _activerunid = value
        End Set
    End Property

    Public ReadOnly Property SetID() As String
        Get
            Return _setid
        End Get
    End Property

    Public Property CreateDate() As DateTime
        Get
            Return _createdate
        End Get
        Set(ByVal Value As DateTime)
            _createdate = Value
        End Set
    End Property

    Public Property PlanDate() As DateTime
        Get
            Return _plandate
        End Get
        Set(ByVal Value As DateTime)
            _plandate = Value
        End Set
    End Property

    Public Property DistributionDate() As DateTime
        Get
            Return _distributiondate
        End Get
        Set(ByVal Value As DateTime)
            _distributiondate = Value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal Value As String)
            _status = Value
        End Set
    End Property

    Public Property Trip() As String
        Get
            Return _trip
        End Get
        Set(ByVal Value As String)
            _trip = Value
        End Set
    End Property

    'Public Property RoutingPolicy() As String
    '    Get
    '        Return _routingpolicy
    '    End Get
    '    Set(ByVal Value As String)
    '        _routingpolicy = Value
    '    End Set
    'End Property

    'Public ReadOnly Property RoutingPolicyObj() As RoutingStrategy
    '    Get
    '        Return New RoutingStrategy(_routingpolicy)
    '    End Get
    'End Property

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

    Public ReadOnly Property Orders() As OutboundOrderCollection
        Get
            Return _orders
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pSetId As String, Optional ByVal LoadObj As Boolean = True)
        _setid = pSetId
        Load(LoadObj)
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        If CommandName.ToLower = "save" Then
            dr = ds.Tables(0).Rows(0)
            Me.Create(Conversion.Convert.ReplaceDBNull(dr("SetId"), ""), Conversion.Convert.ReplaceDBNull(dr("DistributionDate"), Nothing), Conversion.Convert.ReplaceDBNull(dr("trip"), ""), Common.GetCurrentUser)
        ElseIf CommandName = "DeAssignOrders" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            _setid = ds.Tables(0).Rows(0)("SETID")
            Load()
            For Each dr In ds.Tables(0).Rows
                DeAssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser())
            Next
        ElseIf CommandName = "AssignOrders" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            _setid = ds.Tables(0).Rows(0)("SETID")
            Load()
            For Each dr In ds.Tables(0).Rows
                AssignOrder(dr("CONSIGNEE"), dr("ORDERID"), Common.GetCurrentUser())
            Next
        ElseIf CommandName = "plan" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            For Each dr In ds.Tables(0).Rows
                _setid = dr("SETID")
                Load(False)
                PlanRoutes(Common.GetCurrentUser())
            Next
            Message = "Planning Routes"

        ElseIf CommandName = "planwithrunid" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            For Each dr In ds.Tables(0).Rows
                _activerunid = Replacedbnull(dr("ACTIVERUNID"))
                _setid = dr("SETID")
                Load(False)
                If Me.Status <> "PLANNED" Then
                    Message &= " Routing Set " & _setid & " not planned. "
                    Continue For
                End If
                If _activerunid = String.Empty Then
                    Message &= " Undefined Active RunID for Routing Set " & _setid & ". "
                    Continue For
                End If
                Me.PlanWithRunID(Common.GetCurrentUser(), _activerunid)
                Message &= "Routing Set " & _setid & " planned with RUNID. " & _activerunid
            Next
            ''

        ElseIf CommandName = "replan" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            For Each dr In ds.Tables(0).Rows
                _setid = dr("SETID")
                _activerunid = Replacedbnull(dr("ACTIVERUNID"))

                Load(False)
                If Me.Status <> "PLANNED" Then
                    Message &= " Routing Set " & _setid & " not planned. "
                    Continue For
                End If
                If _activerunid = String.Empty Then
                    Message &= " Undefined Active RunID for Routing Set " & _setid & ". "
                    Continue For
                End If
                RePlanRoutes(Common.GetCurrentUser(), _activerunid)

                ''                Dim sql As String = String.Format("select runid from vReplanRunID where routeset='{0}'", _setid)
                ''              Dim runid As String = DataInterface.ExecuteScalar(sql)
                Message &= "Routing Set " & _setid & " RePlanned with RUNID " & _activerunid
            Next


        ElseIf CommandName = "replanbackhaul" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("No Row Selected")
                Return
            End If
            For Each dr In ds.Tables(0).Rows
                _setid = dr("SETID")
                _activerunid = ReplaceDBNull(dr("ACTIVERUNID"))

                Load(False)
                If Me.Status <> "PLANNED" Then
                    Message &= " Routing Set " & _setid & " not planned. "
                    Continue For
                End If
                If _activerunid = String.Empty Then
                    Message &= " Undefined Active RunID for Routing Set " & _setid & ". "
                    Continue For
                End If
                Me.RePlanBackHaulRoutes(Common.GetCurrentUser(), _activerunid)
                Message &= "Routing Set " & _setid & " planned with RUNID. " & _activerunid

            Next



        End If

    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetRoutingSet(ByVal pSetId As String) As RoutingSet
        Return New RoutingSet(pSetId)
    End Function

    Protected Sub Load(Optional ByVal LoadObj As Boolean = True)

        Dim SQL As String = "SELECT * FROM ROUTINGSET WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Throw New M4NException(New Exception, "Routing set Not Found", "Routing set Not Found")
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("CREATEDATE") Then _createdate = dr.Item("CREATEDATE")
        If Not dr.IsNull("PLANDATE") Then _plandate = dr.Item("PLANDATE")
        If Not dr.IsNull("DISTRIBUTIONDATE") Then _distributiondate = dr.Item("DISTRIBUTIONDATE")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("TRIP") Then _trip = dr.Item("TRIP")
        'If Not dr.IsNull("ROUTINGPOLICY") Then _routingpolicy = dr.Item("ROUTINGPOLICY")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

        If Not dr.IsNull("ACTIVERUNID") Then _activerunid = dr.Item("ACTIVERUNID")

        If LoadObj Then
            _orders = New OutboundOrderCollection(_setid)
        End If
    End Sub

    Public Shared Function Exists(ByVal pSetId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from RoutingSet where setid = '{0}'", pSetId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub SetStatus(ByVal pStatus As String, ByVal pUser As String)
        Dim oldStat As String = _status
        _status = pStatus
        _edituser = pUser
        _editdate = DateTime.Now

        Dim sql As String = String.Format("Update routingset SET STATUS = {0},EditUser = {1},EditDate = {2} Where {3}", _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause)
        DataInterface.RunSQL(sql)

        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.RoutingSetStatusChanged
        em.Add("EVENT", EventType)
        em.Add("SETID", _setid)
        em.Add("FROMSTATUS", oldStat)
        em.Add("TOSTATUS", _status)
        em.Add("USERID", pUser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))

    End Sub


    Public Sub SetPlanDate()
        Dim sql As String = String.Format("Update routingset SET PLANDATE = getdate() Where {0}", _
            WhereClause)
        DataInterface.RunSQL(sql)

    End Sub

    Public Sub SetActiveRrunID(ByVal pRunID As String)
        Dim sql As String = String.Format("Update routingset SET ACTIVERUNID = '{1}' Where SETID='{0}' ", _
            _setid, pRunID)
        DataInterface.RunSQL(sql)

    End Sub


#End Region

#Region "Create"

    Public Sub Create(ByVal pSetId As String, ByVal pDistributionDate As DateTime, ByVal pTrip As String, ByVal pUser As String)

        If pSetId Is Nothing Or pSetId.Trim() = "" Then
            _setid = Made4Net.Shared.Util.getNextCounter("ROUTINGSET")
        Else
            If Exists(pSetId) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Routing Set Already Exists", "Routing Set Already Exists")
                Throw m4nEx
            End If
            _setid = pSetId
        End If

        _adddate = DateTime.Now
        _adduser = pUser
        _editdate = DateTime.Now
        _edituser = pUser
        _createdate = DateTime.Now
        _status = WMS.Lib.Statuses.RoutingSet.STATUSNEW
        _distributiondate = pDistributionDate
        _trip = pTrip
        '_routingpolicy = pRoutingPolicy

        Dim sql As String = String.Format("INSERT INTO ROUTINGSET (SETID, CREATEDATE, STATUS, DISTRIBUTIONDATE, TRIP, ADDDATE, ADDUSER, EDITDATE, EDITUSER) VALUES (" & _
            "{0},{1},{2},{3},{4},{5},{6},{7},{8})", Made4Net.Shared.Util.FormatField(_setid), Made4Net.Shared.Util.FormatField(_createdate), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_distributiondate), _
            Made4Net.Shared.Util.FormatField(_trip), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

        DataInterface.RunSQL(sql)

        'Load all Orders Assigned
        _orders = New OutboundOrderCollection(_setid)
    End Sub

#End Region

#Region "Orders Assignment"

    Public Sub AssignOrder(ByVal Consignee As String, ByVal OrdId As String, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.RoutingSet.ASSIGNED Or _status = WMS.Lib.Statuses.RoutingSet.STATUSNEW Or _status = WMS.Lib.Statuses.RoutingSet.PLANNED Then
            Dim Ord As New OutboundOrderHeader(Consignee, OrdId)
            Ord.AssignToRoutingSet(_setid, pUser)
            Orders.add(Ord)
            _editdate = DateTime.Now
            _edituser = pUser
            If Me.Status = WMS.Lib.Statuses.RoutingSet.STATUSNEW Then
                _status = WMS.Lib.Statuses.RoutingSet.ASSIGNED
                DataInterface.RunSQL(String.Format("Update ROUTINGSET set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            Else
                DataInterface.RunSQL(String.Format("Update ROUTINGSET set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            End If
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect RoutingSet Status", "Incorrect RoutingSet Status")
            Throw m4nEx
        End If
    End Sub

    Public Sub DeAssignOrder(ByVal Consignee As String, ByVal OrdId As String, ByVal pUser As String)
        If _status = WMS.Lib.Statuses.Wave.ASSIGNED Or _status = WMS.Lib.Statuses.Wave.PLANNED Then
            Dim ord As OutboundOrderHeader
            ord = Me.Orders.getOrder(Consignee, OrdId)
            ord.DeAssignRoutingSet(pUser)
            Me.Orders.Remove(ord)
            _editdate = DateTime.Now
            _edituser = pUser
            If Orders.Count = 0 Then
                _status = WMS.Lib.Statuses.RoutingSet.STATUSNEW
                DataInterface.RunSQL(String.Format("Update ROUTINGSET set STATUS = {0}, EDITDATE = {1},EDITUSER = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            Else
                DataInterface.RunSQL(String.Format("Update ROUTINGSET set EDITDATE = {0},EDITUSER = {1} WHERE {2}", Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
            End If
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect RoutingSet Status deassign", "Incorrect RoutingSet Status deassign")
            Throw m4nEx
        End If
    End Sub

#End Region

#Region "Plan"

    Public Sub PlanRoutes(ByVal pUser As String)
        If _status = WMS.Lib.Statuses.RoutingSet.STATUSNEW Or _status = WMS.Lib.Statuses.RoutingSet.ASSIGNED Or _status = WMS.Lib.Statuses.RoutingSet.PLANNED Then
            SetStatus(WMS.Lib.Statuses.RoutingSet.PLANNING, pUser)
            Dim currentWarehouse As String = Made4Net.Shared.Warehouse.CurrentWarehouse()

            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.PLANROUTINGSET)
            qh.Values.Add("ROUTINGSET", _setid)
            qh.Values.Add("USER", pUser)
            qh.Values.Add("ONCOMPLETE", 0) ''
            qh.Values.Add("WAREHOUSE", currentWarehouse) ''
            qh.Send("RoutingPlanner", _setid)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Routing set Status, Cannot Plan Routes", "Incorrect Routing set Status, Cannot Plan Routes")
            Throw m4nEx
        End If
    End Sub


    Public Sub RePlanRoutes(ByVal pUser As String, ByVal RunID As String)
        If _status = WMS.Lib.Statuses.RoutingSet.PLANNED   Then
            SetStatus(WMS.Lib.Statuses.RoutingSet.PLANNING, pUser)
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.REPLANROUTINGSET)
            qh.Values.Add("ROUTINGSET", _setid)
            qh.Values.Add("USER", pUser)
            qh.Values.Add("RUNID", RunID)
            qh.Send("RoutingPlanner", _setid)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Routing set Status, Cannot Plan Routes", "Incorrect Routing set Status, Cannot Plan Routes")
            Throw m4nEx
        End If
    End Sub


    Public Sub PlanWithRunID(ByVal pUser As String, ByVal RunID As String)
        If _status = WMS.Lib.Statuses.RoutingSet.STATUSNEW Or _status = WMS.Lib.Statuses.RoutingSet.ASSIGNED Or _status = WMS.Lib.Statuses.RoutingSet.PLANNED Then
            SetStatus(WMS.Lib.Statuses.RoutingSet.PLANNING, pUser)
            Dim currentWarehouse As String = Made4Net.Shared.Warehouse.CurrentWarehouse()

            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.PLANWITHRUNIDROUTINGSET)
            qh.Values.Add("ROUTINGSET", _setid)
            qh.Values.Add("USER", pUser)
            qh.Values.Add("RUNID", RunID)
            qh.Values.Add("ONCOMPLETE", 0) ''
            qh.Values.Add("WAREHOUSE", currentWarehouse) ''
            qh.Send("RoutingPlanner", _setid)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Routing set Status, Cannot Plan Routes", "Incorrect Routing set Status, Cannot Plan Routes")
            Throw m4nEx
        End If
    End Sub

    Public Sub RePlanBackHaulRoutes(ByVal pUser As String, ByVal RunID As String)
        If _status = WMS.Lib.Statuses.RoutingSet.PLANNED Then
            SetStatus(WMS.Lib.Statuses.RoutingSet.PLANNING, pUser)
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("ACTION", WMS.Lib.Actions.Audit.BACKHAULROUTINGSET)
            qh.Values.Add("ROUTINGSET", _setid)
            qh.Values.Add("USER", pUser)
            qh.Values.Add("RUNID", RunID)
            qh.Send("RoutingPlanner", _setid)
        Else
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Routing set Status, Cannot Plan Routes", "Incorrect Routing set Status, Cannot Plan Routes")
            Throw m4nEx
        End If
    End Sub


    Public Sub PlanComplete(ByVal pUser As String)
        _status = WMS.Lib.Statuses.RoutingSet.PLANNED
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _editdate = DateTime.Now
        SetStatus(_status, pUser)
        SetPlanDate()


    End Sub

#End Region

#End Region

End Class
