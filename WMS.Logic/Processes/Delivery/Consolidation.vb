Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class Consolidation

#Region "Consolidation Tasks Collection"

    Protected Class ConsolidationTaskCollection
        Implements ICollection

        Protected _constasks As ArrayList
        Protected _consolidateid As String

#Region "Properties"
        Default Public Property Item(ByVal index As Int32) As ConsolidationTaskDetail
            Get
                Return _constasks(index)
            End Get
            Set(ByVal Value As ConsolidationTaskDetail)
                _constasks(index) = Value
            End Set
        End Property
#End Region

#Region "Constructors"
        Public Sub New(ByVal sConsolidateTaskId As String)
            _consolidateid = sConsolidateTaskId
            _constasks = New ArrayList
            Dim sql As String = String.Format("select * from consolidationdetail where consolidateid = '{0}' order by CONSOLIDATELINE", _consolidateid)
            Dim dt As New DataTable
            Dim dr As DataRow
            DataInterface.FillDataset(sql, dt)
            For Each dr In dt.Rows
                Add(New ConsolidationTaskDetail(dr))
            Next
        End Sub
#End Region

#Region "Overrides"

        Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
            _constasks.CopyTo(array, index)
        End Sub

        Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
            Get
                Return _constasks.Count
            End Get
        End Property

        Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
            Get
                Return _constasks.IsSynchronized()
            End Get
        End Property

        Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
            Get
                Return _constasks.SyncRoot
            End Get
        End Property

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Return _constasks.GetEnumerator
        End Function

#End Region

#Region "Methods"

        Public Function Add(ByVal ctd As ConsolidationTaskDetail) As Int32
            Return _constasks.Add(ctd)
        End Function

#End Region

    End Class

#End Region

#Region "Variables"

    Protected _consolidateid As String
    Protected _status As String
    Protected _tocontainer As String
    Protected _handlingunittype As String
    Protected _usagetype As String
    Protected _serial As String
    Protected _destinationlocation As String
    Protected _destinationwarehousearea As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _constaskcol As ConsolidationTaskCollection

#End Region

#Region "Properties"

    Public Property ConsolidateId() As String
        Get
            Return _consolidateid
        End Get
        Set(ByVal Value As String)
            _consolidateid = Value
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

    Public Property ToContainer() As String
        Get
            Return _tocontainer
        End Get
        Set(ByVal Value As String)
            _tocontainer = Value
        End Set
    End Property

    Public Property HandlingUnitType() As String
        Get
            Return _handlingunittype
        End Get
        Set(ByVal Value As String)
            _handlingunittype = Value
        End Set
    End Property

    Public Property UsageType() As String
        Get
            Return _usagetype
        End Get
        Set(ByVal Value As String)
            _usagetype = Value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal Value As String)
            _serial = Value
        End Set
    End Property

    Public Property DestinationLocation() As String
        Get
            Return _destinationlocation
        End Get
        Set(ByVal Value As String)
            _destinationlocation = Value
        End Set
    End Property

    Public Property DestinationWarehousearea() As String
        Get
            Return _destinationwarehousearea
        End Get
        Set(ByVal Value As String)
            _destinationwarehousearea = Value
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

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" ConsolidateId = {0} ", Made4Net.Shared.Util.FormatField(_consolidateid))
        End Get
    End Property

    Default Public Property Item(ByVal index As Int32) As ConsolidationTaskDetail
        Get
            Return _constaskcol(index)
        End Get
        Set(ByVal Value As ConsolidationTaskDetail)
            _constaskcol(index) = Value
        End Set
    End Property

    Protected ReadOnly Property isCompleted() As Boolean
        Get
            Dim condet As ConsolidationTaskDetail
            Dim hasComp As Boolean = False
            For Each condet In Me
                If condet.Status <> WMS.Lib.Statuses.Consolidation.COMPLETE And condet.Status <> WMS.Lib.Statuses.Consolidation.CANCELED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property
    Protected ReadOnly Property isCanceled() As Boolean
        Get
            Dim condet As ConsolidationTaskDetail
            For Each condet In Me
                If condet.Status <> WMS.Lib.Statuses.Consolidation.CANCELED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property
    Public ReadOnly Property Started() As Boolean
        Get
            Dim condet As ConsolidationTaskDetail
            For Each condet In Me
                If condet.Status = WMS.Lib.Statuses.Consolidation.COMPLETE Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property ConsolidatedLoad(ByVal LoadId As String) As ConsolidationTaskDetail
        Get
            Dim cd As ConsolidationTaskDetail
            For Each cd In Me
                If cd.FromLoad = LoadId Then
                    Return cd
                End If
            Next
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property ConsolidationLine(ByVal LineNumber As Int32) As ConsolidationTaskDetail
        Get
            Dim cd As ConsolidationTaskDetail
            For Each cd In Me
                If cd.ConsolidateLine = LineNumber Then
                    Return cd
                End If
            Next
            Return Nothing
        End Get
    End Property
    Public ReadOnly Property Count() As Int32
        Get
            Return _constaskcol.Count
        End Get
    End Property
    Public ReadOnly Property Units() As Double
        Get
            Dim _units As Double = 0
            Dim cd As ConsolidationTaskDetail
            For Each cd In Me
                Try
                    Dim ld As New Load(cd.FromLoad)
                    _units = _units + ld.UNITS
                Catch ex As Exception

                End Try
            Next
            Return _units
        End Get
    End Property

#End Region

#Region "Constructors"
    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        If CommandName.ToLower = "consolidate" Then
            If ds.Tables(0).Rows.Count = 0 Then
                Throw New ApplicationException("Nothing selected")
            End If
            Dim aq As New EventManagerQ
            Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ConsolidationRequested
            aq.Add("EVENT", EventType)
            aq.Values.Add("ACTION", WMS.Lib.Actions.Audit.CONSOLIDATE)
            For Each dr In ds.Tables(0).Rows
                aq.Values.Add("CONSIGNEE", dr("CONSIGNEE"))
                aq.Values.Add("ORDERID", dr("ORDERID"))
            Next
            aq.Values.Add("USER", Common.GetCurrentUser)
            aq.Send(WMS.Logic.WMSEvents.EventDescription(WMSEvents.EventType.ConsolidationRequested))
            Message = "Creating Consolidation Tasks"
        ElseIf CommandName.ToLower = "approveconsdetail" Then
            For Each dr In ds.Tables(0).Rows
                Try
                    _consolidateid = dr("ConsolidateId")
                    Load()
                    Dim consdet As ConsolidationTaskDetail = Me.ConsolidationLine(dr("ConsolidateLine"))
                    Me.ConsolidateLine(consdet, Common.GetCurrentUser)
                Catch ex As Exception

                End Try
            Next
        ElseIf CommandName.ToLower = "cancelconsdetail" Then
            For Each dr In ds.Tables(0).Rows
                Try
                    _consolidateid = dr("ConsolidateId")
                    Load()
                    Dim consdet As ConsolidationTaskDetail = Me.ConsolidationLine(dr("ConsolidateLine"))
                    Me.CancelLine(consdet, Common.GetCurrentUser)
                Catch ex As Exception

                End Try
            Next
        ElseIf CommandName.ToLower = "approvecons" Then
            For Each dr In ds.Tables(0).Rows
                Try
                    _consolidateid = dr("ConsolidateId")
                    Load()
                    Me.Consolidate(Common.GetCurrentUser)
                Catch ex As Exception

                End Try
            Next
        ElseIf CommandName.ToLower = "cancelcons" Then
            For Each dr In ds.Tables(0).Rows
                Try
                    _consolidateid = dr("ConsolidateId")
                    Load()
                    Me.Cancel(Common.GetCurrentUser)
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Public Sub New(ByVal sConsolidationTaskId As String)
        _consolidateid = sConsolidationTaskId
        Load()
    End Sub
#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from Consolidation where {0}", WhereClause)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException("Consolidation does not exists")
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("STATUS") Then _status = dr("STATUS")
        If Not dr.IsNull("TOCONTAINER") Then _tocontainer = dr("TOCONTAINER")
        If Not dr.IsNull("HANDLINGUNITTYPE") Then _handlingunittype = dr("HANDLINGUNITTYPE")
        If Not dr.IsNull("USAGETYPE") Then _usagetype = dr("USAGETYPE")
        If Not dr.IsNull("SERIAL") Then _serial = dr("SERIAL")
        If Not dr.IsNull("DESTINATIONLOCATION") Then _destinationlocation = dr("DESTINATIONLOCATION")
        If Not dr.IsNull("DESTINATIONWAREHOUSEAREA") Then _destinationlocation = dr("DESTINATIONWAREHOUSEAREA")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr("EDITUSER")

        _constaskcol = New ConsolidationTaskCollection(_consolidateid)
    End Sub

    Public Shared Function Exists(ByVal sConsolidationId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from CONSOLIDATION where CONSOLIDATEID = '{0}'", sConsolidationId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function getEnumerator() As IEnumerator
        Return _constaskcol.GetEnumerator()
    End Function

    Public Sub Save(ByVal pUser As String)
        If _consolidateid Is Nothing Or _consolidateid Is String.Empty Then
            _consolidateid = Made4Net.Shared.Util.getNextCounter("CONSOLIDATION")
        End If

        Dim sql As String

        If Not Exists(_consolidateid) Then
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser
            _status = WMS.Lib.Statuses.Consolidation.AVAILABLE

            sql = String.Format("INSERT INTO CONSOLIDATION(CONSOLIDATEID, STATUS, TOCONTAINER, HANDLINGUNITTYPE, USAGETYPE, SERIAL, DESTINATIONLOCATION, DESTINATIONWAREHOUSEAREA, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                "VALUES({0},{1},{2},{3},{4},{5},{6},{11},{7},{8},{9},{10})", _
                Made4Net.Shared.Util.FormatField(_consolidateid), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_tocontainer), Made4Net.Shared.Util.FormatField(_handlingunittype), Made4Net.Shared.Util.FormatField(_usagetype), Made4Net.Shared.Util.FormatField(_serial), Made4Net.Shared.Util.FormatField(_destinationlocation), _
                Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                Made4Net.Shared.Util.FormatField(_destinationwarehousearea))

            DataInterface.RunSQL(sql)
            _constaskcol = New ConsolidationTaskCollection(_consolidateid)
        Else
            _editdate = DateTime.Now
            _edituser = pUser

            sql = String.Format("UPDATE CONSOLIDATION SET STATUS={0}, TOCONTAINER={1}, HANDLINGUNITTYPE={2}, USAGETYPE={3}, SERIAL={4}, DESTINATIONLOCATION={5}, , DESTINATIONWAREHOUSEAREA={9}, EDITDATE={6}, EDITUSER={7} Where {8} ", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_tocontainer), Made4Net.Shared.Util.FormatField(_handlingunittype), Made4Net.Shared.Util.FormatField(_usagetype), Made4Net.Shared.Util.FormatField(_serial), Made4Net.Shared.Util.FormatField(_destinationlocation), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, _
                Made4Net.Shared.Util.FormatField(_destinationwarehousearea))

            DataInterface.RunSQL(sql)
        End If

    End Sub

    Public Sub AddLine(ByVal oLoad As Load, ByVal pUser As String)
        Dim oConsLine As New ConsolidationTaskDetail
        oConsLine.ConsolidateId = _consolidateid
        oConsLine.FromLoad = oLoad.LOADID
        oConsLine.FromLocation = oLoad.LOCATION
        oConsLine.FromWarehousearea = oLoad.WAREHOUSEAREA
        oConsLine.Save(pUser)

        _constaskcol.Add(oConsLine)
    End Sub

#End Region

#Region "Tasks Management"

    Public Sub AssignUser(ByVal pUser As String)
        _status = WMS.Lib.Statuses.Consolidation.ASSIGNED
        Save(pUser)
    End Sub

    Public Sub UnAssignUser(ByVal pUser As String)
        If Not Started Then
            _status = WMS.Lib.Statuses.Consolidation.AVAILABLE
            Save(pUser)
        End If
    End Sub

#End Region

#Region "Consolidate"

    Public Sub Consolidate(ByVal ConJob As ConsolidationJob, ByVal pConfirmLoad As Load, ByVal pUser As String) 'As ConsolidationJob
        If pConfirmLoad.LOADID <> ConJob.FromLoad Then
            Throw New Made4Net.Shared.M4NException("Wrong Loadid from consolidation")
        End If
        Dim condet As ConsolidationTaskDetail = Me.ConsolidatedLoad(pConfirmLoad.LOADID)

        ConsolidateLine(condet, pUser)
    End Sub

    Public Sub Consolidate(ByVal pUser As String)
        Dim consdet As ConsolidationTaskDetail
        For Each consdet In Me
            Try
                ConsolidateLine(consdet, pUser)
            Catch ex As Exception

            End Try
        Next
    End Sub

    Public Sub ConsolidateLine(ByVal consDet As ConsolidationTaskDetail, ByVal pUser As String)
        Dim cont As Container
        If Not Container.Exists(_tocontainer) Then
            cont = New Container
            cont.HandlingUnitType = _handlingunittype
            cont.UsageType = _usagetype
            cont.Serial = _serial
            cont.ContainerId = _tocontainer
            cont.Post(pUser)
        Else
            cont = New Container(_tocontainer, True)
        End If

        consDet.Consolidate(cont, pUser)
        setStatus(pUser)
    End Sub

    Public Sub CancelLine(ByVal consDet As ConsolidationTaskDetail, ByVal pUser As String)
        consDet.Cancel(pUser)
        setStatus(pUser)
    End Sub

    Public Sub Consolidate(ByVal ConJob As ConsolidationJob, ByVal pContainer As Container, ByVal pUser As String) 'As ConsolidationJob
        If pContainer.Loads(ConJob.FromLoad) Is Nothing Then
            Throw New Made4Net.Shared.M4NException("Load is not on this container")
        End If

        Dim sql As String = String.Format("select count(1) from CONSOLIDATIONDETAIL where fromload in (select loadid from invload where HANDLINGUNIT = '{0}') and (status = '{1}' or status = '{2}')", pContainer.ContainerId, WMS.Lib.Statuses.Consolidation.ASSIGNED, WMS.Lib.Statuses.Consolidation.AVAILABLE)
        Dim numLoads As Int32 = DataInterface.ExecuteScalar(sql)

        If numLoads = pContainer.Loads.Count Then
            Dim ld As Load
            Dim cont As Container
            If Not Container.Exists(_tocontainer) Then
                cont = New Container
                cont.HandlingUnitType = _handlingunittype
                cont.UsageType = _usagetype
                cont.Serial = _serial
                cont.ContainerId = _tocontainer
                cont.Post(pUser)
            Else
                cont = New Container(_tocontainer, True)
            End If

            Dim condet As ConsolidationTaskDetail
            For Each ld In pContainer.Loads
                condet = Me.ConsolidatedLoad(ld.LOADID)
                condet.Consolidate(cont, pUser)
            Next
        Else
            Throw New Made4Net.Shared.M4NException("Not all container loads are schedule for consolidation")
        End If
        setStatus(pUser)
    End Sub

    Public Sub Consolidate(ByVal oLoad As Load, ByVal ToContainer As Container, ByVal pUser As String)
        'If ToContainer.CanConsolidate And oLoad.CanConsolidate Then
        ToContainer.Place(oLoad, pUser)
        'Else
        '    Throw New Made4Net.Shared.M4NException(New Exception, "Cant consolidate load", "Cant consolidate load")
        'End If
    End Sub

    Public Sub Consolidate(ByVal FromContainer As Container, ByVal ToContainer As Container, ByVal pUser As String)
        'If FromContainer.CanConsolidate And ToContainer.CanConsolidate Then
        Dim ld As Load
        For Each ld In FromContainer.Loads
            ToContainer.Place(ld, pUser)
        Next
        'ToContainer.Place(oLoad, pUser)
        'Else
        'Throw New Made4Net.Shared.M4NException(New Exception, "Cant consolidate containers", "Cant consolidate containers")
        'End If
    End Sub

    Public Sub Cancel(ByVal pUser As String)
        Dim oConLine As ConsolidationTaskDetail
        For Each oConLine In Me
            If oConLine.Status = WMS.Lib.Statuses.Consolidation.AVAILABLE Or oConLine.Status = WMS.Lib.Statuses.Consolidation.ASSIGNED Then
                oConLine.Cancel(pUser)
            End If
        Next
        setStatus(pUser)
    End Sub

    Protected Sub setStatus(ByVal pUser As String)
        If isCompleted Then
            If isCanceled Then
                _status = WMS.Lib.Statuses.Consolidation.CANCELED
            Else
                _status = WMS.Lib.Statuses.Consolidation.COMPLETE
            End If
            Save(pUser)

            If TaskManager.ExistConsolidationTask(_consolidateid) Then
                Dim tm As New TaskManager
                tm.getConsolidationTask(_consolidateid)
                'Begin for RWMS-1294 and RWMS-1222
                'tm.Complete()
                tm.Complete(Nothing)
                'End for RWMS-1294 and RWMS-1222
            End If
        End If
    End Sub

#End Region

#Region "Close"

    Public Sub Close(ByVal pUser As String)
        If Started Then
            Dim oCon As New Consolidation
            oCon.DestinationLocation = _destinationlocation
            oCon.DestinationWarehousearea = _destinationwarehousearea
            oCon.HandlingUnitType = _handlingunittype
            oCon.Status = WMS.Lib.Statuses.Consolidation.AVAILABLE
            oCon.ToContainer = Made4Net.Shared.Util.getNextCounter("CONTAINER")
            oCon.UsageType = _usagetype
            oCon.Save(pUser)

            Dim oCondet As ConsolidationTaskDetail
            For Each oCondet In Me
                If oCondet.Status = WMS.Lib.Statuses.Consolidation.AVAILABLE Or oCondet.Status = WMS.Lib.Statuses.Consolidation.ASSIGNED Then
                    oCon.AddLine(New Load(oCondet.FromLoad), pUser)
                    oCondet.Cancel(pUser)
                End If
            Next

            Dim tm As New TaskManager
            tm.CreateConsolidationTask(oCon)

            setStatus(pUser)
        End If
    End Sub

#End Region

#End Region

End Class

<CLSCompliant(False)> Public Class ConsolidationTaskDetail

#Region "Variables"

    Protected _consolidateid As String
    Protected _consolidateline As Int32
    Protected _fromload As String
    Protected _fromlocation As String
    Protected _fromwarehousearea As String
    Protected _fromcontainer As String
    Protected _status As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#Region "Properties"
    Public Property ConsolidateId() As String
        Get
            Return _consolidateid
        End Get
        Set(ByVal Value As String)
            _consolidateid = Value
        End Set
    End Property
    Public Property ConsolidateLine() As Int32
        Get
            Return _consolidateline
        End Get
        Set(ByVal Value As Int32)
            _consolidateline = Value
        End Set
    End Property
    Public Property FromLoad() As String
        Get
            Return _fromload
        End Get
        Set(ByVal Value As String)
            _fromload = Value
        End Set
    End Property
    Public Property FromLocation() As String
        Get
            Return _fromlocation
        End Get
        Set(ByVal Value As String)
            _fromlocation = Value
        End Set
    End Property

    Public Property FromWarehousearea() As String
        Get
            Return _fromwarehousearea
        End Get
        Set(ByVal Value As String)
            _fromwarehousearea = Value
        End Set
    End Property

    Public Property FromContainer() As String
        Get
            Return _fromcontainer
        End Get
        Set(ByVal Value As String)
            _fromcontainer = Value
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
    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" ConsolidateId = {0} and ConsolidateLine = {1} ", Made4Net.Shared.Util.FormatField(_consolidateid), Made4Net.Shared.Util.FormatField(_consolidateline))
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New()

    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

    Public Sub New(ByVal sConId As String, ByVal iConsLine As Int32)
        _consolidateid = sConId
        _consolidateline = iConsLine

        Dim dt As New DataTable
        Dim sql As String = String.Format("Select * from CONSOLIDATIONDETAIL where {0}", WhereClause)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException("Consolidation Task Detail not found")
        End If
        Load(dt.Rows(0))
    End Sub
#End Region

#Region "Methods"

#Region "Accessors"

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("CONSOLIDATEID") Then _consolidateid = dr("CONSOLIDATEID")
        If Not dr.IsNull("CONSOLIDATELINE") Then _consolidateline = dr("CONSOLIDATELINE")
        If Not dr.IsNull("FROMLOAD") Then _fromload = dr("FROMLOAD")
        If Not dr.IsNull("FROMLOCATION") Then _fromlocation = dr("FROMLOCATION")
        If Not dr.IsNull("FROMWAREHOUSEAREA") Then _fromlocation = dr("FROMWAREHOUSEAREA")
        If Not dr.IsNull("FROMCONTAINER") Then _fromcontainer = dr("FROMCONTAINER")
        If Not dr.IsNull("STATUS") Then _status = dr("STATUS")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr("EDITUSER")
    End Sub

    Protected Function Exists(ByVal sConsolidationId As String, ByVal iConsolidationLine As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from CONSOLIDATIONDETAIL where CONSOLIDATEID = '{0}' and CONSOLIDATELINE = {1}", sConsolidationId, iConsolidationLine)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Save(ByVal pUser As String)
        Dim sql As String
        If Not Exists(_consolidateid, _consolidateline) Then
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser

            _status = WMS.Lib.Statuses.Consolidation.AVAILABLE

            _consolidateline = DataInterface.ExecuteScalar(String.Format("select count(1)+1 from CONSOLIDATIONDETAIL where CONSOLIDATEID='{0}'", _consolidateid))

            sql = String.Format("INSERT INTO CONSOLIDATIONDETAIL(CONSOLIDATEID, CONSOLIDATELINE, FROMLOAD, FROMLOCATION, FROMWAREHOUSEAREA, FROMCONTAINER, STATUS, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                "VALUES({0},{1},{2},{3},{10},{4},{5},{6},{7},{8},{9})", _
                Made4Net.Shared.Util.FormatField(_consolidateid), Made4Net.Shared.Util.FormatField(_consolidateline), Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromlocation), _
                Made4Net.Shared.Util.FormatField(_fromcontainer), Made4Net.Shared.Util.FormatField(_status), _
                Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), _
                Made4Net.Shared.Util.FormatField(_fromwarehousearea))
        Else
            _editdate = DateTime.Now
            _edituser = pUser

            sql = String.Format("UPDATE CONSOLIDATIONDETAIL SET FROMLOAD={0},FROMLOCATION={1},FROMWAREHOUSEAREA={7},FROMCONTAINER={2},STATUS={3},EDITDATE={4}, EDITUSER={5} where {6}", _
                Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_fromcontainer), Made4Net.Shared.Util.FormatField(_status), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_fromwarehousearea))
        End If

        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Consolidate(ByVal cont As Container, ByVal pUser As String)
        If Me.Status <> WMS.Lib.Statuses.Consolidation.CANCELED And Me.Status <> WMS.Lib.Statuses.Consolidation.COMPLETE Then
            Dim pConfirmLoad As Load = New Load(_fromload)
            Dim con As New Consolidation
            con.Consolidate(pConfirmLoad, cont, pUser)
            _status = WMS.Lib.Statuses.Consolidation.COMPLETE
            Save(pUser)
        Else
            Throw New Made4Net.Shared.M4NException("Invalid Consolidation Status")
        End If
    End Sub

#End Region

    Public Sub Cancel(ByVal pUser As String)
        _status = WMS.Lib.Statuses.Consolidation.CANCELED
        Save(pUser)
    End Sub

#End Region

End Class

#Region "Consolidation Job"

<CLSCompliant(False)> Public Class ConsolidationJob
    Public TaskId As String
    Public ConsolidationId As String
    Public ConsolidationLine As Int32
    Public FromLoad As String
    Public FromLocation As String
    Public FromWarehousearea As String
    Public FromContainer As String
    Public isContainer As Boolean
    Public consignee As String
    Public Sku As String
    Public skuDesc As String
    Public tocontainer As String
    Public Units As Double
    Public UOM As String
    Public UOMUnits As Double
End Class

#End Region
