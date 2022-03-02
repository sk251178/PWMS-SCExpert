Imports Made4Net.DataAccess
Imports System.Data
Imports System.Web

#Region "Picking Location"

<CLSCompliant(False)> Public Class PickLoc

#Region "Variables"

#Region "Primary Keys"

    Protected _location As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _sku As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _currentqty As Double
    Protected _pendingqty As Double
    Protected _allocatedqty As Double
    Protected _normalreplqty As Double
    'Started for PWMS-756
    Protected _replqty As Double
    Protected _replpolicy As String
    Protected _maxreplqty As Decimal
    'Ended for PWMS-756
    Protected _maximumqty As Double
    Protected _overallocatedqty As Double
    Protected _normalreplpolicy As String
    Protected _batchpicklocation As Boolean
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _parentsku As String

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            'Return " LOCATION = '" & _location & "' and WAREHOUSEAREA= '" & _warehousearea & "'"
            Return String.Format("Location = '{0}' and warehousearea='{1}' and consignee='{2}' and sku='{3}'", _location, _warehousearea, _consignee, _sku)
        End Get
    End Property

    Public Property Location() As String
        Get
            Return _location
        End Get
        Set(ByVal Value As String)
            _location = Value
        End Set
    End Property

    Public Property Warehousearea() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property SKU() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public ReadOnly Property PendingQty() As Double
        Get
            Return _pendingqty
        End Get
    End Property

    Public ReadOnly Property CurrentQty() As Double
        Get
            Return _currentqty
        End Get
    End Property

    Public Property AllocatedQty() As Double
        Get
            Return _allocatedqty
        End Get
        Set(ByVal Value As Double)
            _allocatedqty = Value
        End Set
    End Property

    Public Property NormalReplQty() As Double
        Get
            Return _normalreplqty
        End Get
        Set(ByVal Value As Double)
            _normalreplqty = Value
        End Set
    End Property
    'Started for PWMS-756
    Public Property ReplQty() As Double
        Get
            Return _replqty
        End Get
        Set(ByVal Value As Double)
            _replqty = Value
        End Set
    End Property
    'Ended for PWMS-756

    Public Property MaximumQty() As Double
        Get
            Return _maximumqty
        End Get
        Set(ByVal Value As Double)
            _maximumqty = Value
        End Set
    End Property
    'Started for PWMS-756
    Public Property MaximumReplQty() As Decimal
        Get
            Return _maxreplqty
        End Get
        Set(ByVal Value As Decimal)
            _maxreplqty = Value
        End Set
    End Property
    'Ended for PWMS-756
    Public Property OverAllocatedQty() As Double
        Get
            Return _overallocatedqty
        End Get
        Set(ByVal Value As Double)
            _overallocatedqty = Value
        End Set
    End Property

    Public Property NormalReplPolicy() As String
        Get
            Return _normalreplpolicy
        End Get
        Set(ByVal Value As String)
            _normalreplpolicy = Value
        End Set
    End Property

    Public Property BatchPickLocation() As Boolean
        Get
            Return _batchpicklocation
        End Get
        Set(ByVal Value As Boolean)
            _batchpicklocation = Value
        End Set
    End Property

    'Started for PWMS-756
    Public Property ReplPolicy() As String
        Get
            Return _replpolicy
        End Get
        Set(ByVal Value As String)
            _replpolicy = Value
        End Set
    End Property
    'Ended for PWMS-756
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

    Public Property PARENTSKU() As String
        Get
            Return _parentsku
        End Get
        Set(ByVal Value As String)
            _parentsku = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSKU As String, Optional ByVal LoadObj As Boolean = True)
        _location = pLocation
        _warehousearea = pWarehousearea
        _consignee = pConsignee
        _sku = pSKU
        If LoadObj Then
            Load()
        End If
    End Sub
    Public Sub New(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSKU As String, ByVal pUnallocation As String, Optional ByVal LoadObj As Boolean = True)
        _location = pLocation
        _warehousearea = pWarehousearea
        _consignee = pConsignee
        _sku = pSKU
        If LoadObj Then
            LoadPickLocTable()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        If Not dr Is Nothing Then
            Load(dr)
        End If
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = Common.GetCurrentUser()
        Select Case CommandName.ToLower
            Case "newpickloc"
                dr = ds.Tables(0).Rows(0)
                Dim pk As WMS.Logic.PickLoc = New WMS.Logic.PickLoc
                If Not dr.IsNull("LOCATION") Then pk.Location = dr.Item("LOCATION")
                If Not dr.IsNull("CONSIGNEE") Then pk.Consignee = dr.Item("CONSIGNEE")
                If Not dr.IsNull("SKU") Then pk.SKU = dr.Item("SKU")
                If Not dr.IsNull("NORMALREPLQTY") Then
                    pk.NormalReplQty = dr.Item("NORMALREPLQTY")
                Else
                    pk.NormalReplQty = 0
                End If
                'Started for PWMS-756
                If Not dr.IsNull("REPLQTY") Then
                    pk.ReplQty = dr.Item("REPLQTY")
                Else
                    pk.ReplQty = 0
                End If
                'Ended for PWMS-756
                If Not dr.IsNull("MAXIMUMQTY") Then
                    pk.MaximumQty = dr.Item("MAXIMUMQTY")
                Else
                    pk.MaximumQty = pk.SetPickLocMaxBySku()
                End If
                'Started for PWMS-756
                If Not dr.IsNull("MAXREPLQTY") Then
                    pk.MaximumReplQty = dr.Item("MAXREPLQTY")
                Else
                    pk.MaximumReplQty = pk.SetPickLocMaxBySku()
                End If
                'Ended for PWMS-756
                If Not dr.IsNull("NORMALREPLPOLICY") Then pk.NormalReplPolicy = dr.Item("NORMALREPLPOLICY")
                If Not dr.IsNull("BATCHPICKLOCATION") Then
                    pk.BatchPickLocation = dr.Item("BATCHPICKLOCATION")
                Else
                    pk.BatchPickLocation = Boolean.FalseString
                End If
                If Not dr.IsNull("REPLPOLICY") Then pk.ReplPolicy = dr.Item("REPLPOLICY")
                pk.Save(WMS.Logic.GetCurrentUser())

            Case "editpickloc"
                dr = ds.Tables(0).Rows(0)
                Dim pk As WMS.Logic.PickLoc = New WMS.Logic.PickLoc
                If Not dr.IsNull("LOCATION") Then pk.Location = dr.Item("LOCATION")
                If Not dr.IsNull("CONSIGNEE") Then pk.Consignee = dr.Item("CONSIGNEE")
                If Not dr.IsNull("SKU") Then pk.SKU = dr.Item("SKU")
                If Not dr.IsNull("NORMALREPLQTY") Then
                    pk.NormalReplQty = dr.Item("NORMALREPLQTY")
                Else
                    pk.NormalReplQty = 0
                End If
                'Started for PWMS-756
                If Not dr.IsNull("REPLQTY") Then
                    pk.ReplQty = dr.Item("REPLQTY")
                Else
                    pk.ReplQty = 0
                End If
                'Ended for PWMS-756
                If Not dr.IsNull("MAXIMUMQTY") Then
                    pk.MaximumQty = dr.Item("MAXIMUMQTY")
                Else
                    pk.MaximumQty = pk.SetPickLocMaxBySku()
                End If
                'Started for PWMS-756
                If Not dr.IsNull("MAXREPLQTY") Then
                    pk.MaximumReplQty = dr.Item("MAXREPLQTY")
                Else
                    pk.MaximumReplQty = pk.SetPickLocMaxBySku()
                End If
                'Ended for PWMS-756
                If Not dr.IsNull("NORMALREPLPOLICY") Then pk.NormalReplPolicy = dr.Item("NORMALREPLPOLICY")
                If Not dr.IsNull("BATCHPICKLOCATION") Then
                    pk.BatchPickLocation = dr.Item("BATCHPICKLOCATION")
                Else
                    pk.BatchPickLocation = Boolean.FalseString
                End If
                'Started for PWMS-756
                If Not dr.IsNull("REPLPOLICY") Then pk.ReplPolicy = dr.Item("REPLPOLICY")
                'Ended for PWMS-756
                pk.Save(WMS.Logic.GetCurrentUser())

        End Select


    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function IsBatchPickLocation(consignee As String, location As String, Optional oLogger As LogHandler = Nothing) As Boolean
        Dim dt As New DataTable
        DataInterface.FillDataset(String.Format("select * from pickloc where CONSIGNEE='{0}' and LOCATION='{1}' and BATCHPICKLOCATION='1'", consignee, location), dt)
        If (dt.Rows.Count > 0) Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Location '" + location + "' for  CONSIGNEE='" + consignee + "' is a BATCHPICKLOCATION")
            End If
            Return True
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Location '" + location + "' for  CONSIGNEE='" + consignee + "' is not a BATCHPICKLOCATION")
        End If
        Return False
    End Function

    Public Function HasBatchReplenScheduled(ByVal pBatchPickRegion As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM BATCHREPLENDETAIL BRDETAIL LEFT OUTER JOIN BATCHREPLENHEADER BRHEAD ON BRHEAD.BATCHREPLID = BRDETAIL.BATCHREPLID      WHERE BRDETAIL.CONSIGNEE='{0}' AND BRDETAIL.TOSKU ='{1}' AND BRDETAIL.TOWAREHOUSEAREA='{2}' AND BRDETAIL.TOLOCATION='{3}' AND BRDETAIL.STATUS IN ('PLANNED','RELEASED','LETDOWN') AND BRHEAD.PICKREGION = '{4}'", Me.Consignee, Me.SKU, Me.Warehousearea, Me.Location, pBatchPickRegion)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetPickLoc(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String) As PickLoc
        Return New PickLoc(pLocation, pWarehousearea, pConsignee, pSku)
    End Function

    Public Shared Function Exists(ByVal pConsignee As String, ByVal pSKU As String, ByVal pLocation As String, ByVal pWarehousearea As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM PICKLOC WHERE CONSIGNEE='{0}' and SKU='{1}' and LOCATION = '{2}' and  WAREHOUSEAREA = '{3}' ", pConsignee, pSKU, pLocation, pWarehousearea)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    'Added for RWMS-368
    Public Shared Function SKUExists(ByVal pConsignee As String, ByVal pSKU As String, ByVal pLocation As String, ByVal pWarehousearea As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM PICKLOC WHERE CONSIGNEE='{0}' and SKU<>'{1}' and LOCATION = '{2}' and  WAREHOUSEAREA = '{3}' ", pConsignee, pSKU, pLocation, pWarehousearea)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    'End Added for RWMS-368

    'Added for RWMS-563
    Public Shared Function SKUExistsInPickLoc(ByVal pConsignee As String, ByVal pSKU As String, ByVal pLocation As String, ByVal pWarehousearea As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM PICKLOC WHERE CONSIGNEE='{0}' and SKU='{1}' and LOCATION <> '{2}' and  WAREHOUSEAREA = '{3}' ", pConsignee, pSKU, pLocation, pWarehousearea)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
    'End Added for RWMS-563

    'Added for RWMS-2510 Start
    Public Function CheckCanDelete(ByVal strPickLoc As String, ByRef checkMessage As String) As Boolean
        Dim sql As String
        Dim intval As Integer
        sql = String.Format("select COUNT(1) from PICKDETAIL where status not in ('COMPLETE','CANCELED') and FROMLOCATION='{0}'", strPickLoc)
        intval = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If intval > 0 Then
            checkMessage = "Cannot delete there are open picks"
            Return False
        End If

        sql = String.Format("select COUNT(1) from REPLENISHMENT where status not in ('COMPLETE','CANCELED') and TOLOCATION='{0}'", strPickLoc)
        intval = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If intval > 0 Then
            checkMessage = "Cannot delete there are Replens headed to this pick loc"
            Return False
        End If
        Return True

    End Function
    'Added for RWMS-2510 End

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM vPickLoc Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("CURRENTQTY") Then _currentqty = dr.Item("CURRENTQTY")
        If Not dr.IsNull("PENDINGQTY") Then _pendingqty = dr.Item("PENDINGQTY")
        If Not dr.IsNull("ALLOCATEDQTY") Then _allocatedqty = dr.Item("ALLOCATEDQTY")
        If Not dr.IsNull("NORMALREPLQTY") Then _normalreplqty = dr.Item("NORMALREPLQTY")
        'Started for PWMS-756
        If Not dr.IsNull("REPLQTY") Then _replqty = dr.Item("REPLQTY")
        'Ended for PWMS-756
        If Not dr.IsNull("MAXIMUMQTY") Then _maximumqty = dr.Item("MAXIMUMQTY")
        If Not dr.IsNull("OVERALLOCATEDQTY") Then _overallocatedqty = dr.Item("OVERALLOCATEDQTY")
        If Not dr.IsNull("NORMALREPLPOLICY") Then _normalreplpolicy = dr.Item("NORMALREPLPOLICY")
        'Started for PWMS-756
        If Not dr.IsNull("REPLPOLICY") Then _replpolicy = dr.Item("REPLPOLICY")
        If Not dr.IsNull("MAXREPLQTY") Then _maxreplqty = dr.Item("MAXREPLQTY")
        If Not dr.IsNull("BATCHPICKLOCATION") Then _batchpicklocation = dr.Item("BATCHPICKLOCATION")
        'Ended for PWMS-756
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("PARENTSKU") Then _parentsku = dr.Item("PARENTSKU")
    End Sub

    Protected Sub LoadPickLocTable()
        'Get all the PickLoc table instead of vPickLoc , this is only used for PickList unallocation
        Dim SQL As String = "SELECT * FROM PickLoc Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("CURRENTQTY") Then _currentqty = dr.Item("CURRENTQTY")
        If Not dr.IsNull("PENDINGQTY") Then _pendingqty = dr.Item("PENDINGQTY")
        If Not dr.IsNull("ALLOCATEDQTY") Then _allocatedqty = dr.Item("ALLOCATEDQTY")
        If Not dr.IsNull("NORMALREPLQTY") Then _normalreplqty = dr.Item("NORMALREPLQTY")
        'Started for PWMS-756
        If Not dr.IsNull("REPLQTY") Then _replqty = dr.Item("REPLQTY")
        'Ended for PWMS-756
        If Not dr.IsNull("MAXIMUMQTY") Then _maximumqty = dr.Item("MAXIMUMQTY")
        If Not dr.IsNull("OVERALLOCATEDQTY") Then _overallocatedqty = dr.Item("OVERALLOCATEDQTY")
        If Not dr.IsNull("NORMALREPLPOLICY") Then _normalreplpolicy = dr.Item("NORMALREPLPOLICY")
        'Started for PWMS-756
        If Not dr.IsNull("REPLPOLICY") Then _replpolicy = dr.Item("REPLPOLICY")
        If Not dr.IsNull("MAXREPLQTY") Then _maxreplqty = dr.Item("MAXREPLQTY")
        If Not dr.IsNull("BATCHPICKLOCATION") Then _batchpicklocation = dr.Item("BATCHPICKLOCATION")
        'Ended for PWMS-756
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Protected Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("LOCATION") Then _location = dr.Item("LOCATION")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("CURRENTQTY") Then _currentqty = dr.Item("CURRENTQTY")
        If Not dr.IsNull("PENDINGQTY") Then _pendingqty = dr.Item("PENDINGQTY")
        If Not dr.IsNull("ALLOCATEDQTY") Then _allocatedqty = dr.Item("ALLOCATEDQTY")
        If Not dr.IsNull("NORMALREPLQTY") Then _normalreplqty = dr.Item("NORMALREPLQTY")
        'Started for PWMS-756
        If Not dr.IsNull("REPLQTY") Then _replqty = dr.Item("REPLQTY")
        'Ended for PWMS-756
        If Not dr.IsNull("MAXIMUMQTY") Then _maximumqty = dr.Item("MAXIMUMQTY")
        If Not dr.IsNull("OVERALLOCATEDQTY") Then _overallocatedqty = dr.Item("OVERALLOCATEDQTY")
        If Not dr.IsNull("NORMALREPLPOLICY") Then _normalreplpolicy = dr.Item("NORMALREPLPOLICY")
        'Started for PWMS-756
        If Not dr.IsNull("REPLPOLICY") Then _replpolicy = dr.Item("REPLPOLICY")
        If Not dr.IsNull("MAXREPLQTY") Then _maxreplqty = dr.Item("MAXREPLQTY")
        If Not dr.IsNull("BATCHPICKLOCATION") Then _batchpicklocation = dr.Item("BATCHPICKLOCATION")
        'Ended for PWMS-756
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("PARENTSKU") Then _parentsku = dr.Item("PARENTSKU")
    End Sub

    Public Sub Save(ByVal pUser As String)
        Dim sql As String
        Dim EventType As Int32
        'Added for RWMS-2510 Start
        Dim activitystatus As String
        'Added for RWMS-2510 End
        If Not Exists(_consignee, _sku, _location, _warehousearea) Then
            'Started for PWMS-756
            sql = "INSERT INTO [PICKLOC] " &
                                "([CONSIGNEE],[SKU],[LOCATION],[WAREHOUSEAREA],[NORMALREPLQTY],[REPLQTY],[MAXIMUMQTY],[OVERALLOCATEDQTY],[NORMALREPLPOLICY],[REPLPOLICY],[ADDDATE],[ADDUSER],[EDITDATE],[EDITUSER],[MAXREPLQTY],[BATCHPICKLOCATION]) " &
                                "VALUES " &
                                "(" & Made4Net.Shared.Util.FormatField(_consignee) &
                                "," & Made4Net.Shared.Util.FormatField(_sku) &
                                "," & Made4Net.Shared.Util.FormatField(_location) &
                                "," & Made4Net.Shared.Util.FormatField(_warehousearea) &
                                "," & Made4Net.Shared.Util.FormatField(_normalreplqty) &
                                "," & Made4Net.Shared.Util.FormatField(_replqty) &
                                "," & Made4Net.Shared.Util.FormatField(_maximumqty) &
                                "," & Made4Net.Shared.Util.FormatField(_overallocatedqty) &
                                "," & Made4Net.Shared.Util.FormatField(_normalreplpolicy) &
                                "," & Made4Net.Shared.Util.FormatField(_replpolicy) &
                                ",GETDATE()" &
                                "," & Made4Net.Shared.Util.FormatField(pUser) &
                                ",GETDATE() " &
                                "," & Made4Net.Shared.Util.FormatField(pUser) &
                                "," & Made4Net.Shared.Util.FormatField(_maxreplqty) &
                                "," & Made4Net.Shared.Util.FormatField(_batchpicklocation) & ")"
            EventType = WMS.Logic.WMSEvents.EventType.PickLocationCreated
            'Added for RWMS-2510 Start
            activitystatus = WMS.Lib.Actions.Audit.PICKLOCINS
            'Added for RWMS-2510 End
        Else
            sql = "UPDATE [PICKLOC] " &
                   "SET [CONSIGNEE] = " & Made4Net.Shared.Util.FormatField(_consignee) &
                      ",[SKU] = " & Made4Net.Shared.Util.FormatField(_sku) &
                      ",[NORMALREPLQTY] = " & Made4Net.Shared.Util.FormatField(_normalreplqty) &
                      ",[REPLQTY] = " & Made4Net.Shared.Util.FormatField(_replqty) &
                      ",[MAXIMUMQTY] = " & Made4Net.Shared.Util.FormatField(_maximumqty) &
                      ",[OVERALLOCATEDQTY] = " & Made4Net.Shared.Util.FormatField(_overallocatedqty) &
                      ",[NORMALREPLPOLICY] = " & Made4Net.Shared.Util.FormatField(_normalreplpolicy) &
                      ",[REPLPOLICY] = " & Made4Net.Shared.Util.FormatField(_replpolicy) &
                      ",[EDITDATE] = GETDATE() " &
                      ",[EDITUSER] = " & Made4Net.Shared.Util.FormatField(pUser) &
                      ",[MAXREPLQTY] = " & Made4Net.Shared.Util.FormatField(_maxreplqty) &
                      ",[BATCHPICKLOCATION] = " & Made4Net.Shared.Util.FormatField(_batchpicklocation) &
                 " WHERE " & WhereClause
            EventType = WMS.Logic.WMSEvents.EventType.PickLocationUpdated
            'Added for RWMS-2510 Start
            activitystatus = WMS.Lib.Actions.Audit.PICKLOCUDP
            'Added for RWMS-2510 End
        End If
        DataInterface.RunSQL(sql)
        'Ended for PWMS-756

        'Commented for RWMS-2510 Start
        'Dim em As New EventManagerQ
        'em.Add("EVENT", EventType)
        'em.Add("CONSIGNEE", _consignee)
        'em.Add("WAREHOUSEAREA", _warehousearea)
        'em.Add("SKU", _sku)
        'em.Add("LOCATION", _location)
        'em.Add("USERID", _adduser)
        'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'em.Send(WMSEvents.EventDescription(EventType))
        'Commented for RWMS-2510 End

        'Added for RWMS-2510 Start
        Dim em As New EventManagerQ
        em.Add("EVENT", EventType)
        em.Add("ACTIVITYTYPE", activitystatus)
        em.Add("CONSIGNEE", _consignee)
        em.Add("WAREHOUSEAREA", _warehousearea)
        em.Add("SKU", _sku)
        em.Add("FROMLOC", _location)
        em.Add("TOLOC", _location)
        em.Add("USERID", _adduser)
        em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        em.Send(WMSEvents.EventDescription(EventType))
        'Added for RWMS-2510 End
    End Sub

    Public Sub Create(ByVal pUser As String)
        If Not WMS.Logic.SKU.Exists(_consignee, _sku) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create picking location. SKU does not exist.", "Can not create picking location. SKU does not exist.")
        End If

        If Not WMS.Logic.Location.Exists(_location, _warehousearea) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create picking location. Location does not exist.", "Can not create picking location. Location does not exist.")
        End If

        'Added for RWMS-368
        If SKUExists(_consignee, _sku, _location, _warehousearea) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Location already assigned to another SKU.", "Location already assigned to another SKU.")
        End If
        'End Added for RWMS-368

        'Added for RWMS-563
        If SKUExistsInPickLoc(_consignee, _sku, _location, _warehousearea) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "SKU already exists in another Pick Location.", "SKU already exists in another Pick Location.")
        End If
        'End Added for RWMS-563
        If Exists(_consignee, _sku, _location, _warehousearea) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Location already assigned to another SKU.", "Location already assigned to another SKU.")
        End If

        Save(pUser)
    End Sub

    Public Sub Update(ByVal pUser As String)
        If Not WMS.Logic.SKU.Exists(_consignee, _sku) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not update picking location. SKU does not exist.", "Can not update picking location. SKU does not exist.")
        End If

        If Not WMS.Logic.Location.Exists(_location, _warehousearea) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not update picking location. Location does not exist.", "Can not update picking location. Location does not exist.")
        End If

        If Not Exists(_consignee, _sku, _location, _warehousearea) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not edit picking location. It does not exist.", "Can not edit picking location.It does not exist.")
        End If
        Save(pUser)
    End Sub

    Public Sub Delete()
        Dim sql As String
        sql = String.Format("Delete from pickloc where {0}", WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Over Allocation"

    Public Sub AdjustAllocationQuantity(ByVal pAdj As String, ByVal pQty As Decimal, ByVal pUser As String)
        Dim sql As String

        'RWMS-2387 RWMS-2335 - Do SQL validation and write to the log If pAdj = WMS.Lib.INVENTORY.SUBQTY Then
        If pAdj = WMS.Lib.INVENTORY.SUBQTY Then
            sql = String.Format("select Location,sku,isnull(AllocatedQty,0) as AllocatedQty,isnull(PDQty,0) as PDQty from vPickLocAllocMisMatch WHERE Location  = '{0}' AND SKU ='{1}'", _location, _sku)
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                'Additional logging for QtyAllocated Tracking
                Dim logger As WMS.Logic.LogHandler
                Dim dirpath As String = "Logs\"
                dirpath = Made4Net.DataAccess.Util.BuildAndGetFilePath(dirpath)
                dirpath = dirpath & "\PickLocAllocMisMatch\"
                logger = New LogHandler(dirpath, DateTime.Now.ToString("_ddMMyyyy_hhmmss_") & "LOC_" & _location.ToString() & "_SKU_" & _sku.ToString() & "_" & New Random().Next() & ".txt")
                Dim callingMethod As New System.Diagnostics.StackTrace(1, True)
                Dim callingmethodname As String
                callingmethodname = callingMethod.GetFrame(0).GetMethod().ToString()
                If Not logger Is Nothing Then
                    logger.Write("Calling MethodName =" + callingmethodname + ",Location =" + _location + ",SKU=" + _sku + ",PickLoc AllocatedQty=" + _allocatedqty.ToString())
                End If
                If Not logger Is Nothing Then
                    logger.Write("AllocatedQty of PickLoc table =" + _allocatedqty.ToString() + ",Allocated Qty ready to subtract=" + pQty.ToString())
                End If
                If Not logger Is Nothing Then
                    logger.Write("Quantities of PickLoc Allocation and Pickdetail open pick qty are not matching: Location=" + _location + ",SKU=" + _sku + ",PickLoc AllocatedQty=" + dt.Rows(0)("AllocatedQty").ToString() + ",PickDetail Open Picks Qty=" + dt.Rows(0)("PDQty").ToString())
                End If
                Dim sqlPickDetail As String
                sqlPickDetail = String.Format("SELECT PICKLIST,PICKLISTLINE,QTY FROM pickdetail   where  STATUS in ('PLANNED','RELEASED')  and len(FROMLOAD) <=0 and  FromLocation  = '{0}' AND SKU ='{1}'", _location, _sku)
                Dim dtPickDetail As New DataTable
                DataInterface.FillDataset(sqlPickDetail, dtPickDetail)
                If dtPickDetail.Rows.Count > 0 Then
                    Dim dr As DataRow
                    For Each dr In dtPickDetail.Rows
                        If Not logger Is Nothing Then
                            logger.Write("Open Pick: picklist =" + dr("PICKLIST").ToString() + ",PICKLISTLINE=" + dr("PICKLISTLINE").ToString() + ",Qty=" + dr("QTY").ToString())
                        End If
                    Next
                End If
            End If
        End If

        If pAdj = WMS.Lib.INVENTORY.ADDQTY Then
            _allocatedqty += pQty
            sql = String.Format("update pickloc set allocatedqty=allocatedqty+{0},", Made4Net.Shared.Util.FormatField(pQty))
        ElseIf pAdj = WMS.Lib.INVENTORY.SUBQTY Then
            Dim auditAllocatedQty As Double
            auditAllocatedQty = _allocatedqty
            _allocatedqty -= pQty
            If _allocatedqty < 0 Then
                _allocatedqty = 0
                sql = String.Format("update pickloc set allocatedqty=0,")
                ''Dumping data when pickloc allocated qty is set to 0
                Dim logger As WMS.Logic.LogHandler
                Dim dirpath As String = "Logs\"
                dirpath = Made4Net.DataAccess.Util.BuildAndGetFilePath(dirpath)
                dirpath = dirpath & "\PickLocAllocMisMatch\"
                logger = New LogHandler(dirpath, DateTime.Now.ToString("_ddMMyyyy_hhmmss_") & "AllocZero_" & _location.ToString() & "_SKU_" & _sku.ToString() & "_" & New Random().Next() & ".txt")
                Dim callingMethod As New System.Diagnostics.StackTrace(1, True)
                Dim callingmethodname As String
                callingmethodname = callingMethod.GetFrame(0).GetMethod().ToString()
                If Not logger Is Nothing Then
                    logger.Write("Calling MethodName =" + callingmethodname + ",Location =" + _location)
                    logger.Write("PickLoc AllocatedQty before update=" + auditAllocatedQty.ToString() + ",Current pick Qty Ready to Subtract=" + pQty.ToString())
                End If

            Else
                sql = String.Format("update pickloc set allocatedqty=allocatedqty-{0},", Made4Net.Shared.Util.FormatField(pQty))
            End If
        End If
        _editdate = DateTime.Now
        _edituser = pUser
        'Dim Sql As String = String.Format("update pickloc set allocatedqty={0}, editdate={1}, edituser={2} where {3}", _
        '    Made4Net.Shared.Util.FormatField(_allocatedqty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        sql = String.Format("{0} editdate={1}, edituser={2} where {3}",
        sql, Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Put"

    Public Function Put(ByVal oLoad As Load, ByVal puser As String)
        'If oLoad.CONSIGNEE <> _consignee Or oLoad.SKU <> _sku Then
        If oLoad.CONSIGNEE.ToLower() <> _consignee.ToLower() OrElse oLoad.SKU.ToLower() <> _sku.ToLower() Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot put Load.Invalid Sku", "Cannot put Load.Invalid Sku")
        End If
    End Function

    Public Function SetPickLocMaxBySku() As Integer
        Dim QTY As Integer = -1

        Dim oSku As New SKU(_consignee, _sku)
        Dim VSku, VLocation As Double

        With oSku.UOM(oSku.LOWESTUOM)
            VSku = .HEIGHT * .LENGTH * .HEIGHT
        End With

        With New Location(_location, _warehousearea)
            VLocation = .CUBIC
            'If VLocation = 0 Then
            '    VLocation = .HEIGHT * .LENGTH * .HEIGHT
            'End If
            If VLocation <> 0 And VSku <> 0 Then
                QTY = CInt(VLocation / VSku)
            End If
        End With
        Return QTY

    End Function

#End Region





#End Region

End Class

#End Region