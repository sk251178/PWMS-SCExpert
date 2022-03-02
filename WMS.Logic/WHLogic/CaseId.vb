Imports Made4Net.DataAccess
Imports Made4Net.Shared

Public Class CaseDetail

#Region "Variables"
    Private _caseID As String
    Private _consignee As String
    Private _picklist As String
    Private _PickListLine As Integer
    Private _OrderId As String
    Private _OrderLine As Integer
    Private _FromLoad As String
    Private _ToLoad As String
    Private _ToContainer As String
    Private _status As String
    Private _addDate As DateTime
    Private _addUser As String
    Private _editDate As DateTime
    Private _editUser As String
#End Region

#Region "Properties"
    Public Property CaseID() As String
        Get
            Return _caseID
        End Get
        Set(ByVal value As String)
            _caseID = value
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
    Public Property PickList() As String
        Get
            Return _picklist
        End Get
        Set(ByVal value As String)
            _picklist = value
        End Set
    End Property
    Public Property PickListLine() As Integer
        Get
            Return _PickListLine
        End Get
        Set(ByVal value As Integer)
            _PickListLine = value
        End Set
    End Property
    Public Property OrderId() As String
        Get
            Return _OrderId
        End Get
        Set(ByVal value As String)
            _OrderId = value
        End Set
    End Property
    Public Property OrderLine() As Integer
        Get
            Return _OrderLine
        End Get
        Set(ByVal value As Integer)
            _OrderLine = value
        End Set
    End Property
    Public Property FromLoad() As String
        Get
            Return _FromLoad
        End Get
        Set(ByVal value As String)
            _FromLoad = value
        End Set
    End Property
    Public Property ToLoad() As String
        Get
            Return _ToLoad
        End Get
        Set(ByVal value As String)
            _ToLoad = value
        End Set
    End Property
    Public Property ToContainer() As String
        Get
            Return _ToContainer
        End Get
        Set(ByVal value As String)
            _ToContainer = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property
    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" CASEID = '{0}' ", _caseID)
        End Get
    End Property
    Public Property AddDate() As DateTime
        Get
            Return _addDate
        End Get
        Set(ByVal value As DateTime)
            _addDate = value
        End Set
    End Property
    Public Property AddUser() As String
        Get
            Return _addUser
        End Get
        Set(ByVal value As String)
            _addUser = value
        End Set
    End Property
    Public Property EditDate() As DateTime
        Get
            Return _editDate
        End Get
        Set(ByVal value As DateTime)
            _editDate = value
        End Set
    End Property
    Public Property EditUser() As String
        Get
            Return _editUser
        End Get
        Set(ByVal value As String)
            _editUser = value
        End Set
    End Property
#End Region

#Region "Constructors"
    Public Sub New(ByVal CaseId As String)
        _caseID = CaseId
        Load()
    End Sub
    Public Sub New()

    End Sub
#End Region

#Region "Methods"
    Private Sub Load()
        Dim dt As New DataTable
        Dim sql As String = String.Format("Select * from casedetail where {0}", WhereClause)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException("Casedetail does not exists")
        End If
        Dim dr As DataRow = dt.Rows(0)
        _caseID = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CASEID"))
        _consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE"))
        _picklist = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKLIST"))
        _PickListLine = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PICKLINE"))
        _OrderId = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERID"))
        _OrderLine = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ORDERLINE"))
        _FromLoad = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMLOAD"))
        _ToLoad = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOLOAD"))
        _ToContainer = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOCONTAINER"))
        _status = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        _addDate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _addUser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editDate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _editUser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub
    Private Sub Save(caseID As String, user As String)
        Dim Sql As String
        If WMS.Logic.CaseDetail.Exists(caseID) Then
            Sql = $"Update casedetail set consignee='{Consignee}', picklist='{PickList}',pickline={PickListLine},orderid='{OrderId}',orderline={OrderLine},fromload='{FromLoad}',toload='{ToLoad}',tocontainer='{ToContainer}',status='{Status}',editdate='{DateTime.Now}',edituser='{user}' WHERE CaseID ='{caseID}'"
        Else
            Sql = $" INSERT INTO casedetail ([CASEID],[CONSIGNEE],[PICKLIST],[PICKLINE],[ORDERID],[ORDERLINE],[FROMLOAD],[TOLOAD],[TOCONTAINER],[STATUS], ADDDATE,ADDUSER,EDITDATE,EDITUSER) values ('{caseID}','{Consignee}','{PickList}',{PickListLine},'{OrderId}',{OrderLine},'{FromLoad}','{ToLoad}','{ToContainer}','{Status}',{Made4Net.Shared.Util.FormatField(_addDate)},'{user}',{Made4Net.Shared.Util.FormatField(_editDate)},'{user}')"
        End If
        DataInterface.RunSQL(Sql)
    End Sub
    Public Sub UpdateStatus(caseId As String, status As String, user As String)
        _status = status
        Save(_caseID, user)
    End Sub
    Public Shared Function Create(Consignee As String, PickLIst As String, PickListLine As Integer, OrderId As String, OrderLine As Integer, FromLoad As String, ToLoad As String, ToContainer As String, Status As String, User As String) As CaseDetail
        Dim counter As String = getNextCounter("CASEID")
        Dim caseId As CaseDetail = New CaseDetail()
        caseId.CaseID = counter
        caseId.Consignee = Consignee
        caseId.PickList = PickLIst
        caseId.PickListLine = PickListLine
        caseId.OrderId = OrderId
        caseId.OrderLine = OrderLine
        caseId.FromLoad = FromLoad
        caseId.ToLoad = ToLoad
        caseId.ToContainer = ToContainer
        caseId.Status = Status
        caseId.AddDate = Date.Now
        caseId.EditDate = DateTime.Now
        caseId.AddUser = User
        caseId.EditUser = User
        caseId.Save(counter, User)
        Return New CaseDetail(counter)
    End Function
    Public Shared Function Exists(ByVal CaseID As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from casedetail where CASEID = '{0}'", CaseID)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    Public Shared Sub StageCases(Picklist As String, User As String)
        Dim sql As String = $"select caseid from casedetail where picklist='{Picklist}'"
        Dim dt As DataTable = New DataTable()
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Dim caseid As CaseDetail = New CaseDetail(Convert.ToString(dr("caseid")))
            caseid._status = "STAGED"
            caseid.Save(caseid.CaseID, User)
        Next
    End Sub

    Public Shared Sub SetStatusByShipment(shipment As String, status As String, user As String)
        Dim sql As String = String.Format("update CD set CD.STATUS = {0}, CD.EDITDATE={1}, CD.EDITUSER={2} from SHIPMENTDETAIL SD inner join PICKDETAIL PD on SD.ORDERID = PD.ORDERID AND SD.ORDERLINE = PD.ORDERLINE inner join CASEDETAIL CD on PD.TOLOAD = CD.TOLOAD where SD.SHIPMENT={3}",
                                           Made4Net.Shared.Util.FormatField(status),
                                           Made4Net.Shared.Util.FormatField(DateTime.Now),
                                           Made4Net.Shared.Util.FormatField(user),
                                           Made4Net.Shared.Util.FormatField(shipment)
                                          )
        DataInterface.RunSQL(sql)
    End Sub

#Region "Loading"
    Public Function IsLastOneToBeLoadedInThePayload() As Boolean
        Dim sql As String = String.Format("select count(1) from casedetail where toload='{0}' and status<>'LOADED' and caseid<>'{1}'", ToLoad, CaseID)
        Return Not Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    Private Sub UpdateOrderAfterCaseLoading(ByVal pUser As String)
        If OutboundOrderHeader.OutboundOrderDetail.Exists(Me.Consignee, Me.OrderId, Me.OrderLine) Then
            Dim oOrdLine As OutboundOrderHeader.OutboundOrderDetail = New OutboundOrderHeader.OutboundOrderDetail(Me.Consignee, Me.OrderId, Me.OrderLine)
            If oOrdLine Is Nothing Then Return
            oOrdLine.Load(1, WMS.Lib.LoadActivityTypes.STAGING, pUser)
        ElseIf FlowthroughDetail.Exists(Me.Consignee, Me.OrderId, Me.OrderLine) Then
            Dim oOrdLine As FlowthroughDetail = New FlowthroughDetail(Me.Consignee, Me.OrderId, Me.OrderLine)
            If oOrdLine Is Nothing Then Return
            oOrdLine.Load(1, WMS.Lib.LoadActivityTypes.STAGING, pUser)
        End If
    End Sub
    Public Sub Load(ByVal pConfirmation As String, ByVal pConfirmationWarehousearea As String, ByVal pUser As String, Optional ByVal pShipment As WMS.Logic.Shipment = Nothing)
        _status = WMS.Lib.Statuses.Container.LOADED
        _editUser = pUser
        _editDate = DateTime.Now
        Dim sql As String = String.Format("UPDATE CASEDETAIL SET status = {0},EDITUSER={1},EDITDATE={2} WHERE {3}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_editUser), Made4Net.Shared.Util.FormatField(_editDate), WhereClause)
        DataInterface.RunSQL(sql)
        UpdateOrderAfterCaseLoading(pUser)

        'Load the Payload if all its cases are loaded
        Dim oLoad As New WMS.Logic.Load(Me.ToLoad)
        If oLoad.AllCasesAreLoaded() Then
            oLoad.Load(pConfirmation, pConfirmationWarehousearea, pUser, Nothing, Nothing, Nothing, Nothing, False)

            'Load the container if all its loads are loaded
            Dim oContainer As New WMS.Logic.Container(Me.ToContainer, True)
            If oContainer.AllLoadsAreLoaded() Then
                oContainer.Load(pConfirmation, pConfirmationWarehousearea, pUser, pShipment)
            End If
        End If
    End Sub
#End Region
#End Region
End Class
