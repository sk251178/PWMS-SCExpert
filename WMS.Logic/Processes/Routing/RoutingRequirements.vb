Imports Made4Net.DataAccess
Imports Made4Net.GeoData


#Region "RoutingRequirements"

<CLSCompliant(False)> Public Class RoutingRequirements
    Inherits ArrayList
    Protected _points As Collection
    '<Obsolete()> Protected _CompaniesCash As Hashtable
    Protected _ContactsCash As Hashtable
    Protected _vRoutingRequirementsName As String = String.Empty

#Region "Constructor"

    Public Sub New(ByVal pRoutingSetId As String, Optional ByVal oLogger As LogHandler = Nothing)
        MyBase.New()

        _vRoutingRequirementsName = "ROUTINGREQUIREMENTS"
        If _ContactsCash Is Nothing Then
            LoadContacts(pRoutingSetId, 2, oLogger)
        End If
        Load(pRoutingSetId, oLogger)
        ''setStrategy(oLogger)
    End Sub


    Public Sub New(ByVal pRoutingSetId As String, _
            ByVal pRoutingRequirementsName As String, _
            Optional ByVal oLogger As LogHandler = Nothing)
        MyBase.New()

        _vRoutingRequirementsName = pRoutingRequirementsName
        If _ContactsCash Is Nothing Then
            LoadContacts(pRoutingSetId, 2, oLogger)
        End If
        Load(pRoutingSetId, oLogger)
        ''setStrategy(oLogger)
    End Sub


    Public Sub New(ByVal pRoutingSetId As String, _
            ByVal pRunID As String, _
            ByVal pReplanType As Integer, _
            Optional ByVal oLogger As LogHandler = Nothing)
        MyBase.New()

        Dim SQL As String

        _vRoutingRequirementsName = "ROUTINGREQUIREMENTS"
        If _ContactsCash Is Nothing Then
            LoadContacts(pRoutingSetId, pReplanType, oLogger)
        End If


        If pReplanType = 0 Then
            SQL = String.Format("select distinct * from vSourceROUTINGREQUIREMENTS where routingset={0} and " & _
                                " orderid not in " & _
                                " (select documentid from ROUTESTOPTASK where routeid in " & _
                                " (select routeid from vreplanroutes where runid in ({1})))", _
                                   Made4Net.Shared.Util.FormatField(pRoutingSetId), _
                                   pRunID)
        ElseIf pReplanType = 1 Then
            SQL = String.Format("select distinct * from vSourceBackHaulROUTINGREQUIREMENTS where routingset={0} and " & _
                                " orderid not in " & _
                                " (select documentid from ROUTESTOPTASK where routeid in " & _
                                " (select routeid from vReplanBackHaulRoutes where runid in ({1})))", _
                                   Made4Net.Shared.Util.FormatField(pRoutingSetId), _
                                   pRunID)
        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("Start Loading Routing Requirements for " & pRoutingSetId)
        End If

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        SetRequirements(dt, PlanType.ReplanOnly, oLogger)
        dt.Dispose()

        If Not oLogger Is Nothing Then
            oLogger.Write("Replan type: " & pReplanType & " Total New Routing Requirements Loaded: " & Me.Count)
        End If


    End Sub


#End Region

#Region "Methods"

    Protected Sub Load(ByVal RoutingSetId As String, Optional ByVal oLogger As LogHandler = Nothing)
        Dim strWhereClause As String = ""

        If Not _points Is Nothing Then
            For Each ps As String In _points
                strWhereClause += ps + ","
            Next
            If strWhereClause <> "" Then
                strWhereClause = " and POINTID in (" & strWhereClause.Substring(0, strWhereClause.Length - 2) & ")"
            End If

        End If

        Dim SQL As String = String.Format("SELECT distinct * FROM {1} WHERE routingset = '{0}' and pointid <> '' and pointid is not null " & _
            " order by pointid,contactid,orderid ", RoutingSetId, _vRoutingRequirementsName)

        If Not oLogger Is Nothing Then
            oLogger.Write("Start Loading Routing Requirements for " & RoutingSetId)
            ''            oLogger.Write("Start Loading Routing Requirements, Sql Query: " & SQL)
        End If

        If _ContactsCash Is Nothing Then
            LoadContacts(RoutingSetId, 2, oLogger)
        End If

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)



        SetRequirements(dt, PlanType.Allways, oLogger)
        dt.Dispose()

        If Not oLogger Is Nothing Then
            oLogger.Write("Total Routing Requirements Loaded: " & Me.Count)
        End If
    End Sub

    Private Sub LoadContacts(ByVal RoutingSetId As String, _
            ByVal pReplanType As Integer, _
           Optional ByVal oLogger As LogHandler = Nothing)
        _ContactsCash = New Hashtable

        Dim SQL As String
        If pReplanType = 0 Then
            SQL = String.Format("select distinct contactid from vSourceROUTINGREQUIREMENTS where ROUTINGSET='{0}' and isnull(POINTID,'')<>'' ", _
             RoutingSetId)
        ElseIf pReplanType = 0 Then
            SQL = String.Format("select distinct contactid from vSourceBackHaulROUTINGREQUIREMENTS where ROUTINGSET='{0}' and isnull(POINTID,'')<>'' ", _
             RoutingSetId)
        Else
            SQL = String.Format("select distinct contactid from {1} where ROUTINGSET='{0}' and isnull(POINTID,'')<>'' ", _
             RoutingSetId, _vRoutingRequirementsName)
        End If

        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        For Each dr As DataRow In dt.Rows()
            Dim oContact As Contact = New Contact(dr.Item("contactid"), True)
            If Contact.Exists(dr.Item("contactid").ToString) Then
                _ContactsCash.Add(dr.Item("contactid").ToString(), oContact)
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Requirement Contact: " & dr.Item("contactid").ToString() & " does not exist")
                End If

            End If
        Next

        If Not oLogger Is Nothing Then
            oLogger.Write("Total contacts loaded: " & _ContactsCash.Count)
        End If

    End Sub

    Private Sub SetRequirements(ByVal pReqs As DataTable, ByVal pPlanType As String, _
                Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.Write("Start setting strategies to all requirements...")
        End If

        Dim req As RoutingRequirement
        Dim dr As DataRow
        Dim oPolicyMatcher As New PolicyMatcher("vROUTINGPLANSTRATEGY")

        For Each dr In pReqs.Rows

            '' place to spit req for....
            req = New RoutingRequirement(dr, _ContactsCash, oLogger)
            If Not (req.PlanType Is PlanType.Allways OrElse req.PlanType Is pPlanType) Then Continue For

            If req.PointID = String.Empty Then Continue For
            Add(req)
            'assign the requirements according to policies
            Dim dt As DataTable = oPolicyMatcher.FindMatchingPolicies(dr)
            Dim ret As Integer = req.SetRoutingStrategy(oPolicyMatcher.FindMatchingPolicies(dr))
            If (ret < 0) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** Orderid: " & req.OrderId & " Not found strategy for contact:" & req.ContactId.ToString() & _
                    " point:" & req.PointID)
                End If
            End If

        Next

        If Not oLogger Is Nothing Then
            oLogger.Write("Strategies to all requirements are set...")
        End If

    End Sub

    ''obsolute
    'Public Sub setStrategy(Optional ByVal oLogger As LogHandler = Nothing)
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Start setting strategies to all requirements...")
    '    End If
    '    Dim req As RoutingRequirement
    '    Dim dt As DataTable = GetRoutingStrategies(oLogger)
    '    For Each req In Me
    '        req.setStrategy(dt, oLogger)
    '    Next
    '    If Not oLogger Is Nothing Then
    '        oLogger.Write("Strategies to all requirements are set...")
    '    End If
    '    dt.Dispose()
    'End Sub

    ''obsolute
    Protected Function GetRoutingStrategies(Optional ByVal oLogger As LogHandler = Nothing) As DataTable
        ''        Dim sql = "select * from vROUTINGPLANSTRATEGY"

        Dim sql As String = "select replace(isnull(CONSIGNEE,'%'),'*','%') as consignee," & _
                                " PRIORITY," & _
                                " replace(isnull(ORDERTYPE,'%'),'*','%') as ordertype," & _
                                " replace(isnull(COMPANYTYPE,'%'),'*','%') as companytype," & _
                                " replace(isnull(TARGETCOMPANY,'%'),'*','%') as targetcompany," & _
                                " isnull(ORDERPRIORITY,'0') as orderpriority," & _
                                " replace(isnull(ROUTE,'%'),'*','%') as route," & _
                                " isnull(ORDERVALUE,'0') as ordervalue," & _
                                " isnull(ORDERVOLUME,'0') as ordervolume," & _
                                " isnull(ORDERWEIGHT,'0') as orderweight," & _
                                " replace(isnull(COMPANYGROUP,'%'),'*','%') as companygroup," & _
                                " strategyid ,isnull(CARRIER,'%') as carrier," & _
                                " isnull(UNLOADINGTYPE,'%') as unloadingtype," & _
                                " isnull(VEHICLETYPE,'%') as vehicletype " & _
                            " from ROUTINGPLANSTRATEGY"


        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        Dim idx As Int32
        For Each dr In dt.Rows()
            For idx = 0 To dt.Columns.Count - 1
                If dt.Columns(idx).DataType Is System.Type.GetType("System.String") Then
                    If dr(idx) = "" Then
                        dr(idx) = "%"
                    End If
                End If
            Next
        Next
        Return dt
    End Function

#End Region

End Class

#End Region

#Region "RoutingRequirement"

<CLSCompliant(False)> Public Class RoutingRequirement

#Region "Variables"

    'Shipment/RoutingSet
    Protected _routingset As String
    Protected _shipment As String

    'Order Header Information
    Protected _consignee As String
    Protected _orderid As String
    Protected _ordertype As String
    Protected _targetcompany As String
    Protected _companytype As String
    Protected _companygroup As String
    Protected _staginglane As String
    Protected _route As String
    Protected _orderpriority As Int32
    Protected _ordervalue As Decimal
    Protected _ordervolume As Decimal
    Protected _orderweight As Decimal
    'Protected _deliverycontactid As String
    'Protected _pickupcontactid As String

    Protected _contactid As String
    Protected _chkpnt As String


    Protected _pdtype As String

    'always
    'replan only
    Protected _plantype As String = String.Empty


    'Routing / Loading Inforamtion params
    Protected _carrier As String
    Protected _unloadingtype As String
    Protected _vehicletype As String

    'Strategy Selection
    Protected _strategy As String

    'Target GeoPoint, Company and Order Objects
    Protected _order As OutboundOrderHeader
    'Protected _company As Company
    Protected _contact As Contact
    Protected _TargetGeoPointNode As GeoPointNode
    Protected _TargetGeoPointID As String

    Protected _servicetime As Integer = 0

    Protected _ContactsCash As Hashtable
    Protected _territorykey As String = String.Empty
    Protected _transportationclass As String = String.Empty

    Public planned As Integer = 0
    Public checked As Integer = 0
    ''28/02/2011
    Public EstArrivalHour As Integer = 0
    Public EstDepartureHour As Integer = 0
    Public addDepDays As Integer = 0

#End Region

#Region "Properties"

    Public Property RoutingSet() As String
        Get
            Return _routingset
        End Get
        Set(ByVal value As String)
            _routingset = value
        End Set
    End Property
    Public Property TransportationClass() As String
        Get
            Return _transportationclass
        End Get
        Set(ByVal value As String)
            _transportationclass = value
        End Set
    End Property


    Public Property TerritoryKey() As String
        Get
            Return _territorykey
        End Get
        Set(ByVal value As String)
            _territorykey = value
        End Set
    End Property
    Public Property PDType() As String
        Get
            Return _pdtype
        End Get
        Set(ByVal value As String)
            _pdtype = value
        End Set
    End Property
    Public Property PlanType() As String
        Get
            Return _plantype
        End Get
        Set(ByVal value As String)
            _plantype = value
        End Set
    End Property


    Public Property Shipment() As String
        Get
            Return _shipment
        End Get
        Set(ByVal value As String)
            _shipment = value
        End Set
    End Property

    Public Property OrderId() As String
        Get
            Return _orderid
        End Get
        Set(ByVal value As String)
            _orderid = value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal value As String)
            _consignee = value
        End Set
    End Property

    Public Property OrderType() As String
        Get
            Return _ordertype
        End Get
        Set(ByVal value As String)
            _ordertype = value
        End Set
    End Property

    Public Property TargetCompany() As String
        Get
            Return _targetcompany
        End Get
        Set(ByVal value As String)
            _targetcompany = value
        End Set
    End Property

    Public Property CompanyType() As String
        Get
            Return _companytype
        End Get
        Set(ByVal value As String)
            _companytype = value
        End Set
    End Property

    Public Property CompanyGroup() As String
        Get
            Return _companygroup
        End Get
        Set(ByVal value As String)
            _companygroup = value
        End Set
    End Property

    Public Property StagingLane() As String
        Get
            Return _staginglane
        End Get
        Set(ByVal value As String)
            _staginglane = value
        End Set
    End Property

    Public Property Route() As String
        Get
            Return _route
        End Get
        Set(ByVal value As String)
            _route = value
        End Set
    End Property

    Public Property OrderValue() As Decimal
        Get
            Return _ordervalue
        End Get
        Set(ByVal value As Decimal)
            _ordervalue = value
        End Set
    End Property

    Public Property OrderVolume() As Decimal
        Get
            Return _ordervolume
        End Get
        Set(ByVal value As Decimal)
            _ordervolume = value
        End Set
    End Property

    Public Property OrderWeight() As Decimal
        Get
            Return _orderweight
        End Get
        Set(ByVal value As Decimal)
            _orderweight = value
        End Set
    End Property

    Public Property OrderPriority() As Int32
        Get
            Return _orderpriority
        End Get
        Set(ByVal value As Int32)
            _orderpriority = value
        End Set
    End Property

    Public Property Strategy() As String
        Get
            Return _strategy
        End Get
        Set(ByVal Value As String)
            _strategy = Value
        End Set
    End Property

    Public Property Carrier() As String
        Get
            Return _carrier
        End Get
        Set(ByVal Value As String)
            _carrier = Value
        End Set
    End Property

    Public Property UnloadingType() As String
        Get
            Return _unloadingtype
        End Get
        Set(ByVal Value As String)
            _unloadingtype = Value
        End Set
    End Property

    Public Property VehicleType() As String
        Get
            Return _vehicletype
        End Get
        Set(ByVal Value As String)
            _vehicletype = Value
        End Set
    End Property


    Public ReadOnly Property ContactObj() As Contact
        Get
            Return _contact
        End Get

    End Property


    Public ReadOnly Property TargetGeoPoint() As GeoPointNode
        Get
            Return _TargetGeoPointNode
        End Get
    End Property

    Public Property PointID() As String
        Get
            Return _TargetGeoPointID
        End Get
        Set(ByVal Value As String)
            _TargetGeoPointID = Value
        End Set
    End Property


    Public Property ContactId() As String
        Get
            Return _contactid
        End Get
        Set(ByVal Value As String)
            _contactid = Value
        End Set
    End Property

    Public Property ChkPnt() As String
        Get
            Return _chkpnt
        End Get
        Set(ByVal Value As String)
            _chkpnt = Value
        End Set
    End Property


#End Region

#Region "Ctor"
    Public Sub New(ByVal requirment As DataRow)
        getReq(requirment)
    End Sub

    Public Sub New(ByVal requirment As DataRow, _
                ByVal ContactsCash As Hashtable, _
                Optional ByVal oLogger As LogHandler = Nothing)
        getReq(requirment)

        _ContactsCash = ContactsCash

        Dim sContactid As String = _contactid

        SetContact(sContactid)

        If _TargetGeoPointID Is Nothing OrElse _TargetGeoPointID = String.Empty Then
            If Not oLogger Is Nothing Then
                oLogger.Write("*** RoutingRequirement constructor: Undefined Requirement PointID contact: " & _contactid & _
                    " order: " & _orderid)
            End If
            _TargetGeoPointID = String.Empty
        Else
            If GeoPointNode.AddPointtoGlobalCache(Convert.ToInt32(_TargetGeoPointID)) Then
                _TargetGeoPointNode = GeoPointNode.GetPoints(Convert.ToInt32(_TargetGeoPointID))
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("*** RoutingRequirement constructor: Point not belongs to Network: " & _
                        _TargetGeoPointID & " contact: " & _contactid & _
                    " order: " & _orderid)
                End If
            End If

        End If

    End Sub


    Public Sub getReq(ByVal requirment As DataRow)

        For Each dc As DataColumn In requirment.Table.Columns
            Try
                CallByName(Me, dc.ColumnName, _
                            CallType.Set, _
                            Made4Net.Shared.Conversion.Convert.ReplaceDBNull(requirment(dc.ColumnName)))
            Catch ex As Exception
            End Try
        Next
    End Sub


#End Region

#Region "Methods"




#Region "Accessors"

    Private Sub SetContact(ByVal contactID As String)
        If _ContactsCash.Contains(contactID) Then
            _contact = _ContactsCash(contactID)
        Else
            _contact = New Contact(contactID, True)
        End If
    End Sub



    'Private Sub SetOrder(ByVal dr As DataRow)
    '    Dim oOrd As New OutboundOrderHeader
    '    If Not dr.IsNull("CONSIGNEE") Then oOrd.CONSIGNEE = dr.Item("CONSIGNEE")
    '    If Not dr.IsNull("ORDERID") Then oOrd.ORDERID = dr.Item("ORDERID")
    '    If Not dr.IsNull("ORDERTYPE") Then oOrd.ORDERTYPE = dr.Item("ORDERTYPE")
    '    If Not dr.IsNull("TARGETCOMPANY") Then oOrd.TARGETCOMPANY = dr.Item("TARGETCOMPANY")
    '    If Not dr.IsNull("COMPANYTYPE") Then oOrd.COMPANYTYPE = dr.Item("COMPANYTYPE")
    '    If Not dr.IsNull("STATUS") Then oOrd.STATUS = dr.Item("STATUS")
    '    If Not dr.IsNull("CREATEDATE") Then oOrd.CREATEDATE = dr.Item("CREATEDATE")
    '    If Not dr.IsNull("NOTES") Then oOrd.NOTES = dr.Item("NOTES")
    '    If Not dr.IsNull("STAGINGLANE") Then oOrd.STAGINGLANE = dr.Item("STAGINGLANE")
    '    If Not dr.IsNull("REQUESTEDDATE") Then oOrd.REQUESTEDDATE = dr.Item("REQUESTEDDATE")
    '    If Not dr.IsNull("SCHEDULEDDATE") Then oOrd.SCHEDULEDDATE = dr.Item("SCHEDULEDDATE")
    '    If Not dr.IsNull("SHIPMENT") Then oOrd.SHIPMENT = dr.Item("SHIPMENT")
    '    If Not dr.IsNull("ROUTINGSET") Then oOrd.ROUTINGSET = dr.Item("ROUTINGSET")
    '    If Not dr.IsNull("HOSTORDERID") Then oOrd.HOSTORDERID = dr.Item("HOSTORDERID")
    '    If Not dr.IsNull("ORDERPRIORITY") Then oOrd.ORDERPRIORITY = dr.Item("ORDERPRIORITY")
    '    'If Not dr.IsNull("SHIPTO") Then oOrd.SHIPTO = dr.Item("SHIPTO")
    '    _order = oOrd
    'End Sub

#End Region

    Public Function SetRoutingStrategy(ByVal pDtPolicy As DataTable) As Integer
        If pDtPolicy.Rows.Count > 0 Then
            _strategy = pDtPolicy.Rows(0)("policyid")
            Return 0
        Else
            Return -1
        End If
    End Function



#End Region

End Class

Public Class PlanType
    Public Shared Allways As String = String.Empty
    Public Shared ReplanOnly As String = "R"
End Class

#End Region


