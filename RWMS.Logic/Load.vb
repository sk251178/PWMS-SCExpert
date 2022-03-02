Imports System
Imports System.Web
Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports WMS.Logic

#Region "Load"

' <summary>
' This object represents the properties and methods of a Load.
' </summary>

<CLSCompliant(False)> Public Class Load

#Region "Variables"

#Region "Primary Keys"

    Protected _loadid As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _consignee As String = String.Empty
    Protected _sku As String = String.Empty
    Protected _loaduom As String = String.Empty
    Protected _location As String = String.Empty
    Protected _destinationlocation As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _destinationwarehousearea As String = String.Empty
    Protected _status As String = String.Empty
    Protected _activitystatus As String = String.Empty
    Protected _units As Decimal
    Protected _unitsallocated As Decimal
    Protected _unitspicked As Decimal
    Protected _receipt As String = String.Empty
    Protected _receiptline As Int32
    Protected _receivedate As DateTime
    Protected _laststatusdate As DateTime
    Protected _lastmovedate As DateTime
    Protected _lastcountdate As DateTime
    Protected _holdrc As String = String.Empty
    Protected _prelimbostatus As String = String.Empty
    Protected _prelimboloc As String = String.Empty
    Protected _prelimbowarehousearea As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty

    '''<remarks>vladimir 12/11/2008</remarks>
    Protected _lastmoveuser As String = String.Empty
    Protected _laststatususer As String = String.Empty
    Protected _lastcounteruser As String = String.Empty

    Protected _laststausdate As DateTime

    Protected _laststatusrc As String = String.Empty
    '''<remarks>vladimir 12/11/2008</remarks>

    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _containerid As String
    'Protected _hutype As String
    Protected _loadattributes As InventoryAttributeBase
    Protected _sublocation As String
    Protected _loadsku As SKU = Nothing

    Protected _weight As Decimal = 0
    Protected _volume As Decimal = 0
    'RWMS-344:Begin
    Protected _sHostReferenceID As String = String.Empty
    Protected _sNotes As String = String.Empty
    'RWMS-344:End

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " LOADID = '" & _loadid & "'"
        End Get
    End Property

    Public ReadOnly Property LOADID() As String
        Get
            Return _loadid
        End Get
    End Property

    Public ReadOnly Property ContainerId() As String
        Get
            Return _containerid
        End Get
    End Property

    'Public ReadOnly Property HUTYPE() As String
    '    Get
    '        Return _hutype
    '    End Get
    'End Property

    Public ReadOnly Property IsLimbo() As Boolean
        Get
            If _status.ToUpper = WMS.Lib.Statuses.LoadStatus.LIMBO Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property LoadAttributes() As InventoryAttributeBase
        Get
            Return _loadattributes
        End Get
    End Property

    Public Property CONSIGNEE() As String
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

    Public Property LOADUOM() As String
        Get
            Return _loaduom
        End Get
        Set(ByVal Value As String)
            _loaduom = Value
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

    Public Property WAREHOUSEAREA() As String
        Get
            Return _warehousearea
        End Get
        Set(ByVal Value As String)
            _warehousearea = Value
        End Set
    End Property

    Public Property DESTINATIONLOCATION() As String
        Get
            Return _destinationlocation
        End Get
        Set(ByVal Value As String)
            _destinationlocation = Value
        End Set
    End Property

    Public Property DESTINATIONWAREHOUSEAREA() As String
        Get
            Return _destinationwarehousearea
        End Get
        Set(ByVal Value As String)
            _destinationwarehousearea = Value
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

    Public Property ACTIVITYSTATUS() As String
        Get
            Return _activitystatus
        End Get
        Set(ByVal Value As String)
            _activitystatus = Value
        End Set
    End Property

    Public Property UNITS() As Decimal
        Get
            Return _units
        End Get
        Set(ByVal Value As Decimal)
            _units = Value
        End Set
    End Property

    Public Property UNITSALLOCATED() As Decimal
        Get
            Return _unitsallocated
        End Get
        Set(ByVal Value As Decimal)
            _unitsallocated = Value
        End Set
    End Property

    Public Property UNITSPICKED() As Decimal
        Get
            Return _unitspicked
        End Get
        Set(ByVal Value As Decimal)
            _unitspicked = Value
        End Set
    End Property

    Public Property RECEIPT() As String
        Get
            Return _receipt
        End Get
        Set(ByVal Value As String)
            _receipt = Value
        End Set
    End Property

    Public Property RECEIPTLINE() As Int32
        Get
            Return _receiptline
        End Get
        Set(ByVal Value As Int32)
            _receiptline = Value
        End Set
    End Property

    Public Property RECEIVEDATE() As DateTime
        Get
            Return _receivedate
        End Get
        Set(ByVal Value As DateTime)
            _receivedate = Value
        End Set
    End Property

    Public Property LASTSTATUSDATE() As DateTime
        Get
            Return _laststatusdate
        End Get
        Set(ByVal Value As DateTime)
            _laststatusdate = Value
        End Set
    End Property

    Public Property LASTMOVEDATE() As DateTime
        Get
            Return _lastmovedate
        End Get
        Set(ByVal Value As DateTime)
            _lastmovedate = Value
        End Set
    End Property

    Public Property LASTCOUNTDATE() As DateTime
        Get
            Return _lastcountdate
        End Get
        Set(ByVal Value As DateTime)
            _lastcountdate = Value
        End Set
    End Property

    Public Property HOLDRC() As String
        Get
            Return _holdrc
        End Get
        Set(ByVal Value As String)
            _holdrc = Value
        End Set
    End Property

    Public Property PRELIMBOSTATUS() As String
        Get
            Return _prelimbostatus
        End Get
        Set(ByVal Value As String)
            _prelimbostatus = Value
        End Set
    End Property

    Public Property PRELIMBOLOC() As String
        Get
            Return _prelimboloc
        End Get
        Set(ByVal Value As String)
            _prelimboloc = Value
        End Set
    End Property

    Public Property PRELIMBOWAREHOUSEAREA() As String
        Get
            Return _prelimbowarehousearea
        End Get
        Set(ByVal Value As String)
            _prelimbowarehousearea = Value
        End Set
    End Property

    Public Property SUBLOCATION() As String
        Get
            Return _sublocation
        End Get
        Set(ByVal value As String)
            _sublocation = value
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

    Public Property LASTSTATUSRC() As String
        Get
            Return _laststatusrc
        End Get
        Set(ByVal Value As String)
            _laststatusrc = Value
        End Set
    End Property

    Public Property LASTSTAUSDATE() As DateTime
        Get
            Return _laststausdate
        End Get
        Set(ByVal Value As DateTime)
            _laststausdate = Value
        End Set
    End Property


    Public Property LASTMOVEUSER() As String
        Get
            Return _lastmoveuser
        End Get
        Set(ByVal Value As String)
            _lastmoveuser = Value
        End Set
    End Property

    Public Property LASTCOUNTERUSER() As String
        Get
            Return _lastcounteruser
        End Get
        Set(ByVal Value As String)
            _lastcounteruser = Value
        End Set
    End Property

    Public Property LASTSTATUSUSER() As String
        Get
            Return _laststatususer
        End Get
        Set(ByVal Value As String)
            _laststatususer = Value
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

    Public ReadOnly Property AvailableUnits() As Decimal
        Get
            Return _units - _unitsallocated
        End Get
    End Property

    Public ReadOnly Property Volume() As Decimal
        Get
            Return _volume
        End Get
    End Property

    Public ReadOnly Property Weight() As Decimal
        Get
            Return _weight
        End Get
    End Property

    Public ReadOnly Property UOMUnits() As Decimal
        Get
            Try
                Dim unitsinuom As Decimal
                Dim osku As New SKU(_consignee, _sku)
                unitsinuom = osku.ConvertUnitsToUom(_loaduom, _units)
                osku = Nothing
                Return unitsinuom
            Catch ex As Exception
                Return _units
            End Try
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow
        If CommandName.ToLower = "printlabels" Then
            For Each dr In ds.Tables(0).Rows
                WMS.Logic.Load.PrintLabel(dr("LOADID"), "")
            Next
            Message = "Labels sent to printing service"
        ElseIf CommandName.ToLower = "createload" Then
            Dim ld As Load = New Load
            dr = ds.Tables(0).Rows(0)

            Dim oAttribute As AttributesCollection
            Dim oSku As New SKU(dr("CONSIGNEE"), dr("SKU"))
            oAttribute = SkuClass.ExtractLoadAttributes(oSku.SKUClass, dr)
            ld.CreateLoad(dr("LOADID"), dr("CONSIGNEE"), dr("SKU"), dr("LOADUOM"), dr("LOCATION"), dr("WAREHOUSEAREA"), dr("STATUS"), "", dr("UNITS"), "", 0, "", oAttribute, "")
        ElseIf CommandName.ToLower = "invadjcreateload" Then
            Dim userid As String = Common.GetCurrentUser

            For Each dr In ds.Tables(0).Rows
                InvAdjCreateLoad(dr, userid)
            Next
        ElseIf CommandName.ToLower = "submitchangesku" Then
            For Each dr In ds.Tables(0).Rows

                'Begin RWMS- 474: Author: Ravi
                If WMS.Logic.Load.LBNVADJParamIsSet AndAlso String.IsNullOrEmpty(dr("HostReferenceID")) AndAlso (dr("REASONCODE") = "CR" OrElse dr("REASONCODE") = "VR") Then
                    Throw New M4NException(New Exception, "Please enter HostReferenceID.", "Please enter HostReferenceID.")
                End If
                'End RWMS- 474
                Dim ld As Load = New Load(Convert.ToString(dr("LOADID")))
                'RWMS-344: Begin
                'ld.ChangeSku(dr("TOSKU"), dr("REASONCODE"), Common.GetCurrentUser)
                ld.ChangeSku(dr("TOSKU"), dr("REASONCODE"), Common.GetCurrentUser, dr("HostReferenceID"), dr("Notes"))
                'RWMS-344: End
            Next
        ElseIf CommandName.ToLower = "changeuom" Then
            For Each dr In ds.Tables(0).Rows
                _loadid = dr("LOADID")
                Load()

                ChangeUom(dr("LOADUOM"), dr("REASONCODE"), Common.GetCurrentUser)
            Next
        ElseIf CommandName.ToLower = "setattributes" Then
            Dim oAttribute As AttributesCollection
            For Each dr In ds.Tables(0).Rows
                Try
                    _loadid = dr("LOADID")
                    Load()
                    Dim oSku As New SKU(_consignee, _sku)
                    If Not oSku.SKUClass Is Nothing Then
                        oAttribute = SkuClass.ExtractLoadAttributes(oSku.SKUClass, dr)
                        setAttributes(oAttribute, Common.GetCurrentUser)
                    End If
                Catch ex As Exception

                End Try
            Next
            'ElseIf CommandName = "SubmitAdjustment" Then
            '    Dim errStr As String = String.Empty
            '    For Each dr In ds.Tables(0).Rows
            '        _loadid = dr("LOADID")
            '        Load()
            '        Try
            '            Dim units As Decimal = Decimal.Parse(dr("TOQTY"))

            '            If Not (Math.Round(units) = units) Then
            '                errStr = String.Format("only full units can be confirmed.")
            '            End If

            '        Catch ex As Exception
            '            Throw New M4NException(New Exception, "Please enter quantity to be adjusted.", "Please enter quantity to be adjusted.")
            '        End Try

            '        If Not String.IsNullOrEmpty(errStr) Then
            '            Throw New M4NException(New Exception, errStr, errStr)
            '        End If


            '        Adjust(dr("ADJUSTMENTTYPE"), dr("TOQTY"), dr("REASONCODE"), Common.GetCurrentUser)
            '    Next
            'ElseIf CommandName = "SubmitMove" Then
            '    For Each dr In ds.Tables(0).Rows
            '        _loadid = dr("LOADID")
            '        Load()
            '        Move(dr("TOLOCATION"), WMS.Logic.Warehouse.getUserWarehouseArea(), "", Common.GetCurrentUser)
            '    Next
        ElseIf CommandName = "ChangeStatus" Then
            For Each dr In ds.Tables(0).Rows
                _loadid = dr("LOADID")
                Load()
                setStatus(dr("STATUS"), dr("HOLDRC"), Common.GetCurrentUser)
            Next
            'ElseIf CommandName = "SubmitSplit" Then
            '    For Each dr In ds.Tables(0).Rows
            '        _loadid = dr("LOADID")
            '        Dim toQty As Decimal = 0
            '        Try
            '            toQty = dr("TOQTY")
            '        Catch ex As Exception
            '            Throw New M4NException(New Exception(), "Invalid quantity", "Invalid quantity")
            '        End Try
            '        Load()
            '        Split(dr("TOLOCATION"), dr("TOWAREHOUSEAREA"), dr("TOQTY"), dr("TOLOADID"), Common.GetCurrentUser)
            '    Next
            'ElseIf CommandName = "InvCount" Then
            '    Dim errStr As String = String.Empty
            '    For Each dr In ds.Tables(0).Rows
            '        _loadid = dr("LOADID")
            '        Load()
            '        'If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("toqty")) Is Nothing Then
            '        '    Throw New M4NException(New Exception(), "Counted quantity can not be empty.", "Counted quantity can not be empty.")
            '        'End If
            '        Try
            '            Dim units As Decimal = Decimal.Parse(dr("TOQTY"))

            '            If Not (Math.Round(units) = units) Then
            '                errStr = String.Format("only full units can be confirmed.")
            '            End If

            '        Catch ex As Exception
            '            Throw New M4NException(New Exception, "Counted quantity can not be empty", "Counted quantity can not be empty")
            '        End Try
            '        If Not String.IsNullOrEmpty(errStr) Then
            '            Throw New M4NException(New Exception, errStr, errStr)
            '        End If
            '        'Dim oSku As New SKU(_consignee, _sku)
            '        'Count(dr("TOQTY"), dr("TOLOCATION"), dr("WAREHOUSEAREA"), SkuClass.ExtractLoadAttributes(oSku.SKUClass, dr), Common.GetCurrentUser)
            '        Count(dr("TOQTY"), dr("TOLOCATION"), dr("WAREHOUSEAREA"), Nothing, Common.GetCurrentUser)
            '    Next
        ElseIf CommandName = "findlocation" Then
            For Each dr In ds.Tables(0).Rows
                _loadid = dr("LOADID")
                Load()
                'Dim pw As New Putaway
                '_destinationlocation = pw.RequestDestinationForLoad(_loadid, 0, False)
                'Dim tm As New TaskManager
                'tm.CreatePutAwayTask(Me, WMS.Logic.GetCurrentUser, False)
                PutAway(_destinationlocation, _destinationwarehousearea, Common.GetCurrentUser)
            Next
        ElseIf CommandName = "findlocationsimulate" Then
            Dim msg As String = "Locations found [ "
            For Each dr In ds.Tables(0).Rows
                _loadid = dr("LOADID")
                Load()
                Dim pw As New Putaway
                Dim locstr As String
                Dim whareastr As String
                'Start RWMS-1277
                locstr = ""
                whareastr = ""
                pw.RequestDestinationForLoad(_loadid, locstr, whareastr, 1, "")
                ''End 'RWMS-1277
                msg = msg & _loadid & " - " & locstr & ","
            Next
            Message = msg.TrimEnd(",") & "]"

        ElseIf CommandName = "approvelocation" Then

            For Each dr In ds.Tables(0).Rows
                _loadid = dr("LOADID")
                Load()

                If Not ((Me.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.PUTAWAYPEND) Or (Me.DESTINATIONLOCATION <> String.Empty)) Then
                    'throw exception
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception(), "Activity Status  of Load [#param0#] is incorrect.", "Activity Status  of Load [#param0#] is incorrect.")
                    m4nEx.Params.Add("load", dr("LOADID"))
                    Throw m4nEx
                End If

                If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Then
                    Throw New M4NException(New Exception(), "Cannot move load in status limbo", "Cannot move load in status limbo")
                End If

                If _unitsallocated > 0 Then
                    Throw New M4NException(New Exception(), "Cannot move load - units allocated", "Cannot move load - units allocated")
                End If

                ' Release the task
                'Dim TaskId As String = DataInterface.ExecuteScalar("SELECT TASK FROM TASKS WHERE TASKTYPE='LOADPW' AND (STATUS='ASSIGNED' OR STATUS='AVAILABLE') AND FROMLOAD='" & _loadid & "'")
                Dim sql As String = String.Format("select task from tasks where tasktype ={0} and (status={1} or status={2}) and fromload={3}", _
                Made4Net.Shared.FormatField(WMS.Lib.TASKTYPE.LOADPUTAWAY), _
                Made4Net.Shared.FormatField(WMS.Lib.Statuses.Task.AVAILABLE), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Task.ASSIGNED), _
                Made4Net.Shared.FormatField(Me.LOADID))
                Dim taskID As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                'Put(dr("NewLocation"), "", Common.GetCurrentUser)
                Dim oPW As New Putaway()
                'oPW.Put(_loadid, _sublocation, dr("NewLocation"), "", dr("DestinationWarehouseArea"), Common.GetCurrentUser)
                oPW.Put(_loadid, _sublocation, dr("NewLocation"), dr("DestinationWarehouseArea"), Common.GetCurrentUser)
                If Not String.IsNullOrEmpty(taskID) Then
                    Dim taskObj As New WMS.Logic.Task(taskID)
                    taskObj.Complete(Nothing)
                End If
            Next
        ElseIf CommandName = "printloadslocations" Then
            Dim arrLoads As New ArrayList
            For Each dr In ds.Tables(0).Rows
                arrLoads.Add(dr("LOADID"))
            Next
            If arrLoads.Count > 0 Then
                printLoadsLocations(arrLoads, Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
            End If
        End If
    End Sub

    Public Sub New(ByVal pLOADID As String, Optional ByVal LoadObj As Boolean = True)
        _loadid = pLOADID
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub

    Public Sub New(ByVal dr As DataRow, ByVal pAttributesDr As DataRow)
        Load(dr, pAttributesDr)
    End Sub

#End Region

#Region "Methods"

#Region "Accessors"

    Public Shared Function GetLoad(ByVal pLOADID As String) As Load
        Return New Load(pLOADID)
    End Function

    Public Function hasActivity() As Boolean
        If _activitystatus Is Nothing Then
            Return False
        ElseIf _activitystatus = "" Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Function isInventory() As Boolean
        If _status Is Nothing Then
            Return False
        ElseIf _status = "" Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function Exists(ByVal pLoadId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from LOADS where LOADID = '{0}'", pLoadId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()
        Dim SQL As String = String.Format("SELECT LOADS.*, skuuom.netweight,skuuom.volume FROM LOADS left outer join skuuom on loads.consignee = skuuom.consignee and loads.sku = skuuom.sku and (skuuom.loweruom = '' or skuuom.loweruom is null) WHERE LOADS.LOADID = {0}", Made4Net.Shared.Util.FormatField(_loadid))
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)

        'Load Attributes
        Dim dtAtt As New DataTable
        Dim drAtt As DataRow = Nothing
        SQL = String.Format("SELECT * from attribute where pkeytype ='LOAD' and pkey1 ='{0}'", _loadid)
        DataInterface.FillDataset(SQL, dtAtt)
        If dtAtt.Rows.Count > 0 Then
            drAtt = dtAtt.Rows(0)
        End If
        Load(dr, drAtt)
    End Sub

    Private Sub Load(ByVal dr As DataRow, Optional ByVal pAttributesDr As DataRow = Nothing)
        If Not dr.IsNull("LOADID") Then _loadid = dr.Item("LOADID")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("SKU") Then _sku = dr.Item("SKU")
        If Not dr.IsNull("LOADUOM") Then _loaduom = dr.Item("LOADUOM")
        If Not dr.IsNull("LOCATION") Then _location = dr.Item("LOCATION")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("DESTINATIONLOCATION") Then _destinationlocation = dr.Item("DESTINATIONLOCATION")
        If Not dr.IsNull("DESTINATIONWAREHOUSEAREA") Then _destinationwarehousearea = dr.Item("DESTINATIONWAREHOUSEAREA")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("ACTIVITYSTATUS") Then _activitystatus = dr.Item("ACTIVITYSTATUS")
        If Not dr.IsNull("UNITS") Then _units = dr.Item("UNITS")
        If Not dr.IsNull("UNITSALLOCATED") Then _unitsallocated = dr.Item("UNITSALLOCATED")
        If Not dr.IsNull("UNITSPICKED") Then _unitspicked = dr.Item("UNITSPICKED")
        If Not dr.IsNull("RECEIPT") Then _receipt = dr.Item("RECEIPT")
        If Not dr.IsNull("RECEIPTLINE") Then _receiptline = dr.Item("RECEIPTLINE")
        If Not dr.IsNull("RECEIVEDATE") Then _receivedate = dr.Item("RECEIVEDATE")
        If Not dr.IsNull("LASTSTATUSDATE") Then _laststatusdate = dr.Item("LASTSTATUSDATE")
        If Not dr.IsNull("LASTMOVEDATE") Then _lastmovedate = dr.Item("LASTMOVEDATE")
        If Not dr.IsNull("LASTCOUNTDATE") Then _lastcountdate = dr.Item("LASTCOUNTDATE")
        If Not dr.IsNull("HOLDRC") Then _holdrc = dr.Item("HOLDRC")
        If Not dr.IsNull("PRELIMBOSTATUS") Then _prelimbostatus = dr.Item("PRELIMBOSTATUS")
        If Not dr.IsNull("PRELIMBOLOC") Then _prelimboloc = dr.Item("PRELIMBOLOC")
        If Not dr.IsNull("LASTMOVEUSER") Then _lastmoveuser = dr.Item("LASTMOVEUSER")
        If Not dr.IsNull("LASTSTATUSUSER") Then _laststatususer = dr.Item("LASTSTATUSUSER")
        If Not dr.IsNull("LASTCOUNTUSER") Then _lastcounteruser = dr.Item("LASTCOUNTUSER")
        If Not dr.IsNull("LASTSTATUSRC") Then _laststatusrc = dr.Item("LASTSTATUSRC")
        If Not dr.IsNull("HANDLINGUNIT") Then _containerid = dr.Item("HANDLINGUNIT")
        'If Not dr.IsNull("HUTYPE") Then _hutype = dr.Item("HUTYPE")
        If Not dr.IsNull("SUBLOCATION") Then _sublocation = dr.Item("SUBLOCATION")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")

        If Not pAttributesDr Is Nothing Then
            _loadattributes = New InventoryAttributeBase(pAttributesDr)
        Else
            _loadattributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Load)
        End If
        '_loadsku = New SKU(_consignee, _sku)
        Try
            _volume = _units * Convert.ToDecimal(dr("volume")) 'CalculateVolume(_loadsku)
        Catch ex As Exception
        End Try
        Try
            _weight = _units * Convert.ToDecimal(dr("netweight")) 'CalculateWeight(_loadsku)
        Catch ex As Exception
        End Try
    End Sub

    'Public Sub EvacuateEmptyContainer(ByVal pUser As String)
    '    Dim dt As DataTable = New DataTable
    '    Try
    '        DataInterface.FillDataset("select HANDLINGUNIT from invload WHERE LOADID = '" & _loadid & "'", dt)
    '        ' Remove load from container
    '        RemoveFromContainer()
    '        ' Check if number of rows returned equal to 1, if yes then it is the same load
    '        If dt.Rows.Count = 1 Then
    '            ' it was the last load , need to check if container removal is nessesary
    '            Dim cnt As Container = New Container(dt.Rows(0)("HANDLINGUNIT"), True)
    '            Dim ts As EmptyHUPickupTask = New EmptyHUPickupTask()
    '            ts.CreateEmptyHUPickupTask(cnt)
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Public Function CalculateVolume(Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Decimal
        Return CalculateVolume(_units, oSku)
    End Function

    Public Function CalculateVolume(ByVal calcUnits As Decimal, Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Decimal
        Return WMS.Logic.Inventory.CalculateVolume(_consignee, _sku, calcUnits, _loaduom, oSku)
    End Function

    Public Function CalculateWeight(Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Decimal
        Return CalculateWeight(_units, oSku)
    End Function

    Public Function CalculateWeight(ByVal calcUnits As Decimal, Optional ByVal oSku As WMS.Logic.SKU = Nothing) As Decimal
        Return WMS.Logic.Inventory.CalculateWeight(_consignee, _sku, calcUnits, _loaduom, oSku)
    End Function

    Public Sub ReCalculateVolumeAndWeight(ByVal calcUnits As Decimal, Optional ByVal oSku As WMS.Logic.SKU = Nothing)
        _volume = CalculateVolume(calcUnits, oSku)
        _weight = CalculateWeight(calcUnits, oSku)
    End Sub

    Private Function SKUUnitPrice(ByVal pSku As String, ByVal pConsignee As String) As Decimal
        Try
            Dim sk As WMS.Logic.SKU
            sk = New WMS.Logic.SKU(pConsignee, pSku, True)
            Return sk.UNITPRICE
        Catch ex As Exception
            ex.ToString()
            Return 0
        End Try
    End Function

    Public Sub SetDestinationLocation(ByVal pTargetLoc As String, ByVal pTargetWarehousearea As String, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser
        _destinationlocation = pTargetLoc
        _destinationwarehousearea = pTargetWarehousearea
        If (_destinationlocation <> "" AndAlso _destinationwarehousearea <> "") AndAlso (_activitystatus Is Nothing Or _activitystatus = "" Or _activitystatus = WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND) Then
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.PUTAWAYPEND
        End If
        Dim sql As String = String.Format("update loads set destinationlocation = {0}, " & _
                " destinationwarehousearea = {5},  activitystatus = {1}, editdate = {2},edituser = {3} where loadid = '{4}'", _
                Made4Net.Shared.Util.FormatField(_destinationlocation), _
                Made4Net.Shared.Util.FormatField(_activitystatus), _
                Made4Net.Shared.Util.FormatField(_editdate), _
                Made4Net.Shared.Util.FormatField(_edituser), _
                Me.LOADID, _
                Made4Net.Shared.Util.FormatField(_destinationwarehousearea))
        DataInterface.ExecuteScalar(sql)

        'set the activity status
        'update the target location pendings as well
        'Try
        '    Dim oLocation As New Location(pTargetLoc, pTargetWarehousearea)
        '    oLocation.SetDestLoads(Me, pUser)
        'Catch ex As Exception
        'End Try
    End Sub

    Public Sub SetReplenishmentJob(ByVal Repl As Replenishment, ByVal pUser As String)
        _editdate = DateTime.Now
        _edituser = pUser
        _destinationlocation = Repl.ToLocation
        _destinationwarehousearea = Repl.ToWarehousearea
        _activitystatus = WMS.Lib.Statuses.ActivityStatus.REPLPENDING
        If Repl.ReplType = Replenishment.ReplenishmentTypes.PartialReplenishment Then
            _unitsallocated += Repl.Units
        Else
            _unitsallocated = _units
        End If

        Dim sql As String = String.Format("update loads set destinationlocation = {0}," & _
                " destinationwarehousearea = {6}, activitystatus = {1}, unitsallocated = {2}, editdate = {3},edituser = {4} where loadid = '{5}'", _
                Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Me.LOADID, _
                Made4Net.Shared.Util.FormatField(_destinationwarehousearea))
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub setAttributes(ByVal oAttributesCollection As AttributesCollection, ByVal sUserId As String)
        Dim porgWeight, porgCube As Decimal
        Dim unitPrice As Decimal
        porgWeight = CalculateWeight()
        porgCube = CalculateVolume()
        unitPrice = SKUUnitPrice(_sku, _consignee)

        Dim aq As New EventManagerQ()
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadAttributeChanged)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.INVENTORY.LDATTOUT)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOQTY", "0")
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", sUserId)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("LASTMOVEUSER", sUserId)
        aq.Add("LASTSTATUSUSER", sUserId)
        aq.Add("LASTCOUNTUSER", sUserId)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("ADDUSER", sUserId)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", sUserId)
        aq.Send(WMS.Lib.INVENTORY.LDATTOUT)

        _loadattributes.Attributes.Clear()

        _loadattributes.Add(oAttributesCollection)
        _loadattributes.PrimaryKey1 = _loadid
        _loadattributes.Save(sUserId)

        aq = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadAttributeChanged)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.INVENTORY.LDATTIN)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", sUserId)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("LASTMOVEUSER", sUserId)
        aq.Add("LASTSTATUSUSER", sUserId)
        aq.Add("LASTCOUNTUSER", sUserId)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("ADDUSER", sUserId)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", sUserId)
        aq.Send(WMS.Lib.INVENTORY.LDATTOUT)

        ' Get Counter for inventory attribute changed
        Dim strcnt As String = Made4Net.Shared.Util.getNextCounter("SETATTDOC")
        Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.LDATTOUT)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", strcnt)
        it.Add("LINE", 0)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        it.Add("QTY", _units)
        it.Add("CUBE", porgCube)
        it.Add("WEIGHT", porgWeight)
        it.Add("AMOUNT", "")
        it.Add("SKU", _sku)
        it.Add("STATUS", _status)
        it.Add("REASONCODE", "")
        it.Add("UNITPRICE", unitPrice)
        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        ' Create row for each attribute
        InventoryTransaction.CreateAttributesRecords(it, _loadattributes)

        it.Add("LASTMOVEUSER", sUserId)
        it.Add("LASTSTATUSUSER", sUserId)
        it.Add("LASTCOUNTUSER", sUserId)
        it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", sUserId)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", sUserId)
        it.Send(WMS.Lib.INVENTORY.LDATTOUT)

        it = New InventoryTransactionQ()
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.LDATTIN)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", strcnt)
        it.Add("LINE", 0)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        it.Add("QTY", _units)
        it.Add("CUBE", porgCube)
        it.Add("WEIGHT", porgWeight)
        it.Add("AMOUNT", "")
        it.Add("SKU", _sku)
        it.Add("STATUS", _status)
        it.Add("REASONCODE", "")
        it.Add("UNITPRICE", unitPrice)
        ' Create row for each attribute
        InventoryTransaction.CreateAttributesRecords(it, _loadattributes)

        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("LASTMOVEUSER", sUserId)
        it.Add("LASTSTATUSUSER", sUserId)
        it.Add("LASTCOUNTUSER", sUserId)
        it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", sUserId)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", sUserId)
        it.Send(WMS.Lib.INVENTORY.LDATTIN)
    End Sub

    Public Function GetAttribute(ByVal sAttributeName As String)
        Try
            If Not LoadAttributes Is Nothing Then
                Return LoadAttributes.Attribute(sAttributeName)
            End If
        Catch ex As Exception
            Return Nothing
        End Try
        Return Nothing
    End Function

    Public Function GetOutboundOrder() As WMS.Logic.OutboundOrderHeader
        Dim SQL As String = String.Format("select consignee, orderid from orderloads where loadid = '{0}'", _loadid)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            Return New WMS.Logic.OutboundOrderHeader(dt.Rows(0)("consignee"), dt.Rows(0)("orderid"))
        Else
            Return Nothing
        End If
    End Function

    Public Function GetFlowThroughOrder() As WMS.Logic.Flowthrough
        Dim SQL As String = String.Format("select consignee, orderid from orderloads where loadid = '{0}'", _loadid)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            Return New Flowthrough(dt.Rows(0)("consignee"), dt.Rows(0)("orderid"))
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Label Printing"

    Public Sub PrintLabel(Optional ByVal lblPrinter As String = "")
        WMS.Logic.Load.PrintLabel(_loadid, lblPrinter)
    End Sub

    Public Shared Sub PrintLabel(ByVal Loadid As String, ByVal lblPrinter As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "LOAD")
        qSender.Add("LabelType", "LOAD")
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("LOADID", Loadid)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("LOADID", Loadid)
        qSender.Send("Label", "Load Label")
    End Sub

    Public Sub PrintFlowthroughLabel(Optional ByVal lblPrinter As String = "")
        WMS.Logic.Load.PrintFlowthroughLabel(_loadid, lblPrinter)
    End Sub

    Public Shared Sub PrintFlowthroughLabel(ByVal Loadid As String, ByVal lblPrinter As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "FLOWTHROUGHLOAD")
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("LOADID", Loadid)
        qSender.Add("LabelType", "FLOWTHROUGHLOAD")
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("LOADID", Loadid)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Send("Label", "Flowthrough Load Label")
    End Sub

    Public Sub PrintShippingLabel(Optional ByVal lblPrinter As String = "", Optional ByVal pLabelFormat As String = "")
        WMS.Logic.Load.PrintShippingLabel(_loadid, lblPrinter, pLabelFormat)
    End Sub

    Public Shared Sub PrintShippingLabel(ByVal Loadid As String, ByVal lblPrinter As String, ByVal pLabelFormat As String)
        If lblPrinter Is Nothing Then
            lblPrinter = ""
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "LOADSHIPLBL")
        qSender.Add("LabelType", pLabelFormat)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("LOADID", Loadid)
        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("FORMATFILE", pLabelFormat)
        qSender.Add("LOADID", Loadid)
        qSender.Send("Label", "Load Ship Label")
    End Sub

#End Region

#Region "Reports"

    Public Sub printLoadsLocations(ByVal LoadIdArr As ArrayList, ByVal Language As Int32, ByVal pUser As String)
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String = String.Empty
        Dim loadIds As String = String.Empty
        Dim i As Int32
        Dim dt As New DataTable
        repType = WMS.Lib.REPORTS.LOADLOCATIONS
        '-----------------------------------------------------------------------
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", "PutAwayWorkSheet", "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", "PutAwayWorkSheet")
        oQsender.Add("DATASETID", "repPutAwayWorkSheet")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", "")
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try

        For i = 0 To LoadIdArr.Count - 2
            loadIds = loadIds & "'" & LoadIdArr(i) & "',"
        Next
        loadIds = loadIds & "'" & LoadIdArr(LoadIdArr.Count - 1) & "'"
        oQsender.Add("WHERE", String.Format("loadid in ({0})", loadIds))

        oQsender.Send("Report", repType)

    End Sub

#End Region

#Region "Create"

    Public Sub CreateLoad(ByVal pLoadID As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pLoadUom As String, ByVal pLocation As String, ByVal pWarehousearea As String, _
        ByVal pStatus As String, ByVal pActivityStatus As String, ByVal pUnits As Decimal, ByVal pReceipt As String, ByVal pReceiptLine As Int32, _
        ByVal pHold As String, ByVal oAttributeCollection As AttributesCollection, ByVal pUser As String, Optional ByVal pDocumentType As String = "", _
        Optional ByVal pReceiveDate As DateTime = #1/1/1900#, Optional ByVal pPreLimboLocation As String = Nothing, _
        Optional ByVal pPreLimboStatus As String = Nothing, Optional ByVal oSku As WMS.Logic.SKU = Nothing, Optional ByVal sHostReferenceID As String = "", Optional ByVal sNotes As String = "")

        Dim ts As DateTime = DateTime.Now

        _loadid = pLoadID
        _consignee = pConsignee
        _sku = pSku
        '_reasonCode = pReasonCode
        _loaduom = pLoadUom
        _location = pLocation
        _warehousearea = pWarehousearea
        _destinationlocation = Nothing
        _destinationwarehousearea = Nothing
        _status = pStatus
        _activitystatus = pActivityStatus
        _units = pUnits
        _receipt = pReceipt
        _receiptline = pReceiptLine
        _adddate = ts
        _editdate = ts

        If pReceiveDate = #1/1/1900# Then
            _receivedate = DateTime.Now
        Else
            _receivedate = pReceiveDate
        End If

        _lastmoveuser = pUser
        _laststatususer = pUser
        _lastcounteruser = pUser
        _laststausdate = DateTime.Now
        _laststatusrc = pHold


        _adduser = pUser
        _edituser = pUser
        _holdrc = pHold
        _unitsallocated = 0
        _unitspicked = 0
        '_receivedate = ts
        _laststatusdate = ts
        _lastmovedate = DateTime.Now
        _lastcountdate = Nothing
        _prelimboloc = pPreLimboLocation
        _prelimbostatus = pPreLimboStatus
        'RWMS-344:Begin
        _sHostReferenceID = sHostReferenceID
        _sNotes = sNotes
        'RWMS-344:End

        Create(oSku)
        If Not oAttributeCollection Is Nothing Then
            Dim skuObj As New SKU(pConsignee, pSku)
            For idx As Int32 = 0 To oAttributeCollection.Count - 1
                If oAttributeCollection.Keys(idx).ToUpper() = "WEIGHT" Then
                    If skuObj.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUNET" Then
                        Dim netWeight As Decimal = WMS.Logic.SKU.GetNetWeight(skuObj.SKU)
                        Dim skuNetWght As Decimal = netWeight * pUnits
                        oAttributeCollection(idx) = skuNetWght
                    ElseIf skuObj.RECEIVINGWEIGHTCAPTUREMETHOD = "SKUGROSS" Then
                        Dim grossWeight As Decimal = WMS.Logic.SKU.GetGrossWeight(skuObj.SKU)
                        Dim skuGrossWght As Decimal = grossWeight * pUnits
                        oAttributeCollection(idx) = skuGrossWght
                    End If
                End If
                _loadattributes.Attribute(oAttributeCollection.Keys(idx)) = oAttributeCollection(idx)
            Next
            _loadattributes.Save(pUser)
        End If
        Load()
    End Sub

    Public Shared Function GenerateLoadId() As String
        Dim newldid As String
        newldid = Made4Net.Shared.Util.getNextCounter("LOAD")
        While Exists(newldid)
            newldid = Made4Net.Shared.Util.getNextCounter("LOAD")
        End While
        Return newldid
    End Function

    Private Sub Create(Optional ByVal pSku As WMS.Logic.SKU = Nothing)
        Dim SQL As String

        If _loadid Is Nothing Or _loadid = "" Then
            If WMS.Logic.Consignee.AutoGenerateLoadID(_consignee) Then
                _loadid = GenerateLoadId()
            Else
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Create Load, No Loadid", "Cannot Create Load, No Loadid")
                Throw m4nEx
            End If
        End If

        If WMS.Logic.Load.Exists(_loadid) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Create Load, Loadid Already Exists", "Cannot Create Load, Loadid Already Exists")
            Throw m4nEx
        End If

        If Not WMS.Logic.SKU.Exists(_consignee, _sku) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Consignee/Sku does not exists", "Consignee/Sku does not exists")
            Throw m4nEx
        End If

        Dim oSku As WMS.Logic.SKU
        If pSku Is Nothing Then
            oSku = New WMS.Logic.SKU(_consignee, _sku)
        Else
            oSku = pSku
        End If
        ' Commented out - moved out from all processes just to receiving process. should not be validated on picking according to Tods request.
        'If oSku.NEWSKU Then
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "New SKU", "New SKU")
        '    Throw m4nEx
        'End If
        'If Not oSku.STATUS Then
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Inactive SKU", "Inactive SKU")
        '    Throw m4nEx
        'End If

        If _loaduom Is Nothing Or _loaduom = "" Or oSku.UNITSOFMEASURE.UOM(LOADUOM) Is Nothing Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid Unit Of Measure", "Invalid Unit Of Measure")
            Throw m4nEx
        End If

        If Not IsLimbo And Not (_location = "" Or _warehousearea = "") Then
            If Not (WMS.Logic.Location.Exists(_location, _warehousearea)) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Location or Warehousearea does not exists", "Location or Warehousearea does not exists")
                Throw m4nEx
            End If
        End If

        SQL = "Insert into loads(LOADID,CONSIGNEE,SKU,LOADUOM,LOCATION,DESTINATIONLOCATION,WAREHOUSEAREA,DESTINATIONWAREHOUSEAREA,STATUS,ACTIVITYSTATUS," & _
            "UNITS,UNITSALLOCATED,UNITSPICKED,RECEIPT,RECEIPTLINE,RECEIVEDATE,LASTSTATUSDATE," & _
            "LASTMOVEDATE,LASTCOUNTDATE,HOLDRC,PRELIMBOSTATUS,PRELIMBOLOC,SUBLOCATION,PRELIMBOWAREHOUSEAREA, ADDDATE,ADDUSER," & _
            "LASTMOVEUSER,LASTSTATUSUSER,LASTCOUNTUSER,LASTSTATUSRC," & _
            "EDITDATE,EDITUSER) Values ("

        SQL = SQL & Made4Net.Shared.Util.FormatField(_loadid) & "," & Made4Net.Shared.Util.FormatField(_consignee) & "," & Made4Net.Shared.Util.FormatField(_sku) & "," & _
            Made4Net.Shared.Util.FormatField(_loaduom) & "," & Made4Net.Shared.Util.FormatField(_location, "") & "," & Made4Net.Shared.Util.FormatField(_destinationlocation) & "," & _
            Made4Net.Shared.Util.FormatField(_warehousearea, "''") & "," & Made4Net.Shared.Util.FormatField(_destinationwarehousearea) & "," & _
            Made4Net.Shared.Util.FormatField(_status) & "," & Made4Net.Shared.Util.FormatField(_activitystatus) & "," & _
            Made4Net.Shared.Util.FormatField(_units) & "," & Made4Net.Shared.Util.FormatField(_unitsallocated) & "," & Made4Net.Shared.Util.FormatField(_unitspicked) & "," & _
            Made4Net.Shared.Util.FormatField(_receipt) & "," & Made4Net.Shared.Util.FormatField(_receiptline, , True) & "," & Made4Net.Shared.Util.FormatField(_receivedate) & "," & _
            Made4Net.Shared.Util.FormatField(_laststatusdate) & "," & Made4Net.Shared.Util.FormatField(_lastmovedate) & "," & Made4Net.Shared.Util.FormatField(_lastcountdate) & "," & _
            Made4Net.Shared.Util.FormatField(_holdrc) & "," & Made4Net.Shared.Util.FormatField(_prelimbostatus) & "," & _
            Made4Net.Shared.Util.FormatField(_prelimboloc) & "," & Made4Net.Shared.Util.FormatField(_sublocation) & "," & _
            Made4Net.Shared.Util.FormatField(_prelimbowarehousearea) & "," & _
            Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
            Made4Net.Shared.Util.FormatField(_lastmoveuser) & "," & _
            Made4Net.Shared.Util.FormatField(_laststatususer) & "," & _
            Made4Net.Shared.Util.FormatField(_lastcounteruser) & "," & _
            Made4Net.Shared.Util.FormatField(_laststatusrc) & "," & _
            Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

        DataInterface.RunSQL(SQL)
        _loadattributes = New InventoryAttributeBase(InventoryAttributeBase.AttributeKeyType.Load, _loadid)
    End Sub

    Private Sub InvAdjCreateLoad(ByVal dr As DataRow, ByVal pUser As String)
        Dim oAttribute As AttributesCollection
        Dim oSku As New SKU(dr("CONSIGNEE"), dr("SKU"))
        oAttribute = SkuClass.ExtractLoadAttributes(oSku.SKUClass, dr)

        Dim numOfLoads As Integer = 1
        Try
            Integer.TryParse(dr("NUMLOADS"), numOfLoads)
        Catch ex As Exception

        End Try
        If numOfLoads = 1 Then
            If (dr("LOADID") Is Nothing OrElse dr("LOADID") = "") Then
                'oSkuClass = New SKU(rl.CONSIGNEE, rl.SKU).SKUClass
                If Not WMS.Logic.Consignee.AutoGenerateLoadID(dr("CONSIGNEE")) Then
                    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid Load ID", String.Format("LoadId is not available, CONSIGNEE.GENERATELOAD is false, CONSIGNEE='{0}'", dr("CONSIGNEE")))
                    Throw m4nEx
                End If
                dr("Loadid") = GenerateLoadId()
            End If

            ' RWMS-2171
            If IsDBNull(dr("HostReferenceID")) Then
                dr("HostReferenceID") = String.Empty
                dr.AcceptChanges()
            End If
            ' RWMS-2171

            CreateLoad(dr("LOADID"), dr("CONSIGNEE"), dr("SKU"), dr("LOADUOM"), dr("LOCATION"), WMS.Logic.Warehouse.CurrentWarehouseArea, dr("STATUS"), "", dr("UNITS"), "", 0, "", oAttribute, "", "", New Date(1900, 1, 1), "", "", Nothing, dr("HostReferenceID"), "")
            loadCreatedSendMsg(pUser)
            'Added for RWMS-2454 Start
            WMS.Logic.Load.PrintLabel(dr("LOADID"), dr("PRINTER"))
            'Added for RWMS-2454 End

        Else
            If Not WMS.Logic.Consignee.AutoGenerateLoadID(dr("CONSIGNEE")) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid Load ID", String.Format("LoadId is not available, CONSIGNEE.GENERATELOAD is false, CONSIGNEE='{0}'", dr("CONSIGNEE")))
                Throw m4nEx
            End If
            For i As Integer = 0 To numOfLoads - 1
                dr("Loadid") = GenerateLoadId()
                ' RWMS-2454
                If IsDBNull(dr("HostReferenceID")) Then
                    dr("HostReferenceID") = String.Empty
                    dr.AcceptChanges()
                End If
                ' RWMS-2454
                CreateLoad(dr("LOADID"), dr("CONSIGNEE"), dr("SKU"), dr("LOADUOM"), dr("LOCATION"), WMS.Logic.Warehouse.CurrentWarehouseArea, dr("STATUS"), "", dr("UNITS"), "", 0, "", oAttribute, "", "", New Date(1900, 1, 1), "", "", Nothing, dr("HostReferenceID"), "")
                loadCreatedSendMsg(pUser)
                'Added for RWMS-1546 and RWMS-1398
                WMS.Logic.Load.PrintLabel(dr("LOADID"), dr("PRINTER"))
                'Ended for  RWMS-1546 and RWMS-1398

            Next
        End If

        'Dim aq As EventManagerQ = New EventManagerQ
        'aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CreateLoad)
        'aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CREATELOAD)
        'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("ACTIVITYTIME", "0")
        'aq.Add("CONSIGNEE", _consignee)
        'aq.Add("DOCUMENT", _receipt)
        'aq.Add("DOCUMENTLINE", _receiptline)
        'aq.Add("FROMLOAD", _loadid)
        'aq.Add("FROMLOC", _location)
        'aq.Add("FROMQTY", 0)
        'aq.Add("FROMSTATUS", _status)
        'aq.Add("NOTES", "")
        'aq.Add("SKU", _sku)
        'aq.Add("TOLOAD", _loadid)
        'aq.Add("TOLOC", _location)
        'aq.Add("TOQTY", _units)
        'aq.Add("TOSTATUS", _status)
        'aq.Add("USERID", pUser)
        'aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("ADDUSER", pUser)
        'aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("EDITUSER", pUser)
        'aq.Send(WMS.Lib.Actions.Audit.CREATELOAD)

        'Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        'it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.CREATELOAD)
        'it.Add("CONSIGNEE", _consignee)
        'it.Add("DOCUMENT", _receipt)
        'it.Add("LINE", _receiptline)
        'it.Add("LOADID", _loadid)
        'it.Add("UOM", _loaduom)
        'it.Add("QTY", _units)
        'it.Add("CUBE", 0)
        'it.Add("WEIGHT", 0)
        'it.Add("AMOUNT", 0)
        'it.Add("SKU", _sku)
        'it.Add("STATUS", _status)
        'it.Add("REASONCODE", "")
        'it.Add("UNITPRICE", 0)
        'InventoryTransaction.CreateAttributesRecords(it, _loadattributes)
        'it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("ADDUSER", pUser)
        'it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("EDITUSER", pUser)
        'it.Send(WMS.Lib.INVENTORY.CREATELOAD)
    End Sub

    Private Sub loadCreatedSendMsg(ByVal pUser As String)
        'Dim aq As EventManagerQ = New EventManagerQ
        'aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUnitsChanged)
        'aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.ADDQTY)
        'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("ACTIVITYTIME", "0")
        'aq.Add("CONSIGNEE", _consignee)
        'aq.Add("DOCUMENT", _receipt)
        'aq.Add("DOCUMENTLINE", _receiptline)
        'aq.Add("FROMLOAD", _loadid)
        'aq.Add("FROMLOC", _location)
        'aq.Add("FROMQTY", 0)
        'aq.Add("FROMSTATUS", _status)
        'aq.Add("NOTES", "")
        'aq.Add("SKU", _sku)
        'aq.Add("TOLOAD", _loadid)
        'aq.Add("TOLOC", _location)
        'aq.Add("TOQTY", _units)
        'aq.Add("TOSTATUS", _status)
        'aq.Add("USERID", pUser)
        'aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("ADDUSER", pUser)
        'aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("EDITUSER", pUser)
        'aq.Send(WMS.Lib.Actions.Audit.ADDQTY)

        'Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        'it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.ADDQTY)
        'it.Add("CONSIGNEE", _consignee)
        'it.Add("DOCUMENT", _receipt)
        'it.Add("LINE", _receiptline)
        'it.Add("LOADID", _loadid)
        'it.Add("UOM", _loaduom)
        'it.Add("QTY", _units)
        'it.Add("CUBE", 0)
        'it.Add("WEIGHT", 0)
        'it.Add("AMOUNT", 0)
        'it.Add("SKU", _sku)
        'it.Add("STATUS", _status)
        ''Commented For RWMS-366 Start
        ''it.Add("REASONCODE", "")
        ''Commented For RWMS-366 End
        ''Added For RWMS -366 Start
        'it.Add("REASONCODE", Made4Net.Shared.ContextSwitch.Current.Session("ReasonCode"))
        ''Added For RWMS -366 End
        'it.Add("UNITPRICE", 0)
        'InventoryTransaction.CreateAttributesRecords(it, _loadattributes)
        'it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("ADDUSER", pUser)
        'it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        'it.Add("EDITUSER", pUser)
        'it.Send(WMS.Lib.INVENTORY.ADDQTY)
        'Made4Net.Shared.ContextSwitch.Current.Session("ReasonCode") = Nothing
        'Made4Net.Shared.ContextSwitch.Current.Session.Abandon()

        'Begin: RWMS-344
        'Code changed to post to Inventory transaction queue first, then retrieve that ID and post it to SCEXpert connect to include that as part of
        'Inventory Transaction XML file that gets created under "RWMS\Interfaces\Export" folder
        Dim dtIT As New DataTable
        Dim drIT As DataRow
        Dim sInvTransId As String = String.Empty
        Dim sInvTransIdNew As String = String.Empty
        Dim SQL As String = String.Format("SELECT INVTRANS,LOADID from INVENTORYTRANS where LOADID ='{0}' order by INVTRANS desc", _loadid)

        DataInterface.FillDataset(SQL, dtIT)
        If dtIT.Rows.Count > 0 Then
            drIT = dtIT.Rows(0)
            sInvTransId = drIT("INVTRANS").ToString()
        End If
        'End: RWMS-344

        Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        'Begin: RWMS-344
        it.Add("INVTRANSID", sInvTransId)
        it.Add("HOSTREFERENCEID", _sHostReferenceID)
        it.Add("NOTES", _sNotes)
        'End: RWMS-344
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.ADDQTY)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", _receipt)
        it.Add("LINE", _receiptline)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        it.Add("QTY", _units)
        it.Add("CUBE", 0)
        it.Add("WEIGHT", 0)
        it.Add("AMOUNT", 0)
        it.Add("SKU", _sku)
        it.Add("STATUS", _status)
        'it.Add("REASONCODE", "")
        it.Add("REASONCODE", Made4Net.Shared.ContextSwitch.Current.Session("REASONCODE"))
        'System.Web.HttpContext.Current.Session("ReasonCode")
        it.Add("UNITPRICE", 0)
        InventoryTransaction.CreateAttributesRecords(it, _loadattributes)
        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", pUser)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", pUser)
        it.Send(WMS.Lib.INVENTORY.ADDQTY)
        'System.Web.HttpContext.Current.Session.Remove("ReasonCode")

        'Begin: RWMS-344
        'Retrieve the above
        'Compare the above inventory transaction id that was retried before new INSERT to INVENTORYTRANS table with the below new inventory transaction id in a loop by sleeping for one second each iteration

        SQL = String.Format("SELECT INVTRANS from INVENTORYTRANS where LOADID ='{0}' order by INVTRANS desc", _loadid)
        sInvTransIdNew = sInvTransId
        Do
            'sleep for 1 seconds
            Threading.Thread.Sleep(1000)
            dtIT = New DataTable
            DataInterface.FillDataset(SQL, dtIT)
            If dtIT.Rows.Count > 0 Then
                drIT = dtIT.Rows(0)
                sInvTransIdNew = drIT("INVTRANS").ToString()
            End If
            dtIT = Nothing
        Loop Until (Not sInvTransId.Equals(sInvTransIdNew))
        'End: RWMS-344

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUnitsChanged)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.ADDQTY)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("INVTRANSID", sInvTransIdNew) 'New id should be sent
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        'aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        'Begin: RWMS-344
        aq.Add("NOTES", _sNotes.Trim())
        aq.Add("HOSTREFERENCEID", _sHostReferenceID)
        aq.Add("REASONCODE", Made4Net.Shared.ContextSwitch.Current.Session("REASONCODE"))
        'End: RWMS-344
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.ADDQTY)
    End Sub

#End Region

#Region "Activity Statuses"
    Public Function SetActivityStatus(ByVal pStatus As String, ByVal pUser As String) As Boolean

        If pStatus = WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND Then
            If (Not (_activitystatus Is Nothing Or _activitystatus = "")) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load Already Assigned", "Load Already Assigned")
                Throw m4nEx
            End If
        End If
        _activitystatus = pStatus
        _editdate = DateTime.Now
        _edituser = pUser
        Dim sql As String = String.Format("Update Loads set ACTIVITYSTATUS = {0}, EditDate = {1}, EditUser = {2} Where {3}", Made4Net.Shared.Util.FormatField(_activitystatus), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)
        Return True
    End Function
#End Region

#Region "Allocation / Picking / Staging / Packing / Loading / Shipping"

    Public Sub Allocate(ByVal AllocUnits As Decimal, ByVal pUser As String)
        _unitsallocated += AllocUnits
        _editdate = DateTime.Now
        _edituser = pUser
        DataInterface.RunSQL(String.Format("Update Loads set UNITSALLOCATED = UNITSALLOCATED + {0},EDITUSER = {1},EDITDATE = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(AllocUnits), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
    End Sub

    Public Sub Pick(ByVal PickUnits As Decimal, ByVal pUser As String, Optional ByVal AllocatedUnits As Decimal = 0, Optional ByVal pPickedWeight As Decimal = -1)
        Dim originalUnits As Decimal = _units
        _units -= PickUnits
        If AllocatedUnits <> 0 Then
            _unitsallocated -= AllocatedUnits
        Else
            _unitsallocated -= PickUnits
        End If
        _unitspicked += PickUnits
        _editdate = DateTime.Now
        _edituser = pUser
        If _units < 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity - qty cannot become negative", "Invalid quantity - cannot become negative")
        End If
        If _unitsallocated < 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity - qty cannot become negative", "Invalid quantity - cannot become negative")
        End If
        If _units = 0 Then
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.NONE
            _status = WMS.Lib.Statuses.LoadStatus.NONE
            _location = ""
            If AllocatedUnits <> 0 Then
                DataInterface.RunSQL(String.Format("Update Loads set UNITS = UNITS - {0},UNITSALLOCATED = UNITSALLOCATED - {0},UNITSPICKED = UNITSPICKED + {7},STATUS = {1},ACTIVITYSTATUS = {2},LOCATION = {3},EDITUSER = {4},EDITDATE = {5} WHERE {6}", Made4Net.Shared.Util.FormatField(AllocatedUnits), _
                    Made4Net.Shared.Util.FormatField(_status, ""), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(PickUnits)))
            Else
                DataInterface.RunSQL(String.Format("Update Loads set UNITS = UNITS - {0},UNITSALLOCATED = UNITSALLOCATED - {0},UNITSPICKED = UNITSPICKED + {0},STATUS = {1},ACTIVITYSTATUS = {2},LOCATION = {3},EDITUSER = {4},EDITDATE = {5} WHERE {6}", Made4Net.Shared.Util.FormatField(PickUnits), _
                    Made4Net.Shared.Util.FormatField(_status, ""), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
            End If
            ' Here we need to check if there is container and update this loads --> Moved to picktask
            'EvacuateEmptyContainer(pUser)
        Else
            If AllocatedUnits <> 0 Then
                DataInterface.RunSQL(String.Format("Update Loads set UNITS = UNITS - {4},UNITSALLOCATED = UNITSALLOCATED - {0},UNITSPICKED = UNITSPICKED + {4},EDITUSER = {1},EDITDATE = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(AllocatedUnits), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(PickUnits)))
            Else
                DataInterface.RunSQL(String.Format("Update Loads set UNITS = UNITS - {0},UNITSALLOCATED = UNITSALLOCATED - {0},UNITSPICKED = UNITSPICKED + {0},EDITUSER = {1},EDITDATE = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(PickUnits), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
            End If

            If pPickedWeight < 0 Then
                Me.AdjustLoadWeightAttribute(originalUnits, originalUnits - PickUnits, Me.WeightBeforeAdjusting, pUser)
            Else
                Me.LoadAttributes.Attribute("WEIGHT") = Math.Max(0, Me.WeightBeforeAdjusting - pPickedWeight)
                Me.LoadAttributes.Save(pUser)
            End If

        End If
    End Sub

    Public Sub PickFull(ByVal pDestinationLocation As String, ByVal pDestinationWarehousearea As String, ByVal pUser As String)
        _unitspicked += _units
        _unitsallocated = 0
        _destinationlocation = pDestinationLocation
        _destinationwarehousearea = pDestinationWarehousearea
        _location = WMS.Lib.Statuses.LoadStatus.NONE
        _warehousearea = WMS.Lib.Statuses.LoadStatus.NONE
        _activitystatus = WMS.Lib.Statuses.ActivityStatus.PICKED
        _editdate = DateTime.Now
        _edituser = pUser
        DataInterface.RunSQL(String.Format("Update Loads set UNITSALLOCATED = 0,UNITSPICKED = {0},STATUS = {1},ACTIVITYSTATUS = {2},DESTINATIONLOCATION = {3}, LOCATION= {4},DESTINATIONWAREHOUSEAREA = {8}, WAREHOUSEAREA= {9},EDITUSER = {5},EDITDATE = {6} WHERE {7}", Made4Net.Shared.Util.FormatField(_unitspicked), _
            Made4Net.Shared.Util.FormatField(_status, ""), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_destinationlocation, ""), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(_destinationwarehousearea, ""), Made4Net.Shared.Util.FormatField(_warehousearea, "")))
    End Sub

    Public Sub UpdatePickQty(ByVal PickUnits As Decimal, ByVal pUser As String)
        _units += PickUnits
        _editdate = DateTime.Now
        _edituser = pUser
        DataInterface.RunSQL(String.Format("Update Loads set UNITS = UNITS + {0},EDITUSER = {1},EDITDATE = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(PickUnits), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
    End Sub

    Public Function ValidatePick(ByVal PickUnits As Decimal, ByVal pLocation As String, ByVal pWarehousearea As String) As Boolean
        If _location.ToLower <> pLocation.ToLower Or _warehousearea.ToLower <> pWarehousearea.ToLower Then
            Return False
        End If
        If (_units - PickUnits < 0) Or (_unitsallocated - PickUnits < 0) Then
            Return False
        End If
        Return True
    End Function

    Public Sub unAllocate(ByVal unAllocUnits As Decimal, ByVal pUser As String)
        _unitsallocated -= unAllocUnits
        If _unitsallocated < 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity - qty cannot become negative", "Invalid quantity - cannot become negative")
        End If
        _editdate = DateTime.Now
        _edituser = pUser
        DataInterface.RunSQL(String.Format("Update Loads set UNITSALLOCATED = UNITSALLOCATED - {0},EDITUSER = {1},EDITDATE = {2} WHERE {3}", Made4Net.Shared.Util.FormatField(unAllocUnits), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), WhereClause))
    End Sub

    Public Sub Ship(ByVal puser As String, Optional ByVal pOrderid As String = "", Optional ByVal pOrderLine As Int32 = -1, Optional ByVal od As OutboundOrderHeader.OutboundOrderDetail = Nothing, Optional ByVal Header As OutboundOrderHeader = Nothing, Optional ByVal fd As FlowthroughDetail = Nothing, Optional ByVal fh As Flowthrough = Nothing, Optional ByVal pDocType As String = "")
        Dim orderid As String = Nothing
        Dim orderline As Int32
        Dim sOldStatus As String = _status
        If pOrderid = "" And pOrderLine = -1 Then
            If od Is Nothing Then
                Dim SQL As String = String.Format("select orderid, orderline from orderloads where loadid = '{0}'", _loadid)
                Dim dt As New DataTable
                DataInterface.FillDataset(SQL, dt)
                If dt.Rows.Count > 0 Then
                    orderid = dt.Rows(0)("orderid")
                    orderline = dt.Rows(0)("orderline")
                End If
            Else
                orderid = od.ORDERID
                orderline = od.ORDERLINE
            End If
        Else
            orderid = pOrderid
            orderline = pOrderLine
        End If
        Dim OldActivityType As String = GetPreviousActivityType()
        Delete(puser, "", False)

        ' Dim sk As SKU = New SKU(_consignee, _sku)
        Dim unitPrice As Decimal = 0
        'Try
        '    unitPrice = _loadsku.UNITPRICE  'SKUUnitPrice(_loadsku)
        'Catch ex As Exception
        'End Try
        'Dim porgWeight, porgCube As Decimal
        'porgWeight = CalculateWeight(_loadsku)
        'porgCube = CalculateVolume(_loadsku)

        Dim aq As New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ShipLoad)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SHIPLOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", orderid)
        aq.Add("DOCUMENTLINE", orderline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", sOldStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", puser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", puser)
        aq.Add("LASTSTATUSUSER", puser)
        aq.Add("LASTCOUNTUSER", puser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", puser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", puser)
        aq.Send(WMS.Lib.Actions.Audit.SHIPLOAD)

        Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.Actions.Audit.SHIPLOAD)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", orderid)
        it.Add("LINE", orderline)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        it.Add("QTY", _units)
        it.Add("CUBE", _volume)
        it.Add("WEIGHT", _weight)
        it.Add("AMOUNT", "")
        it.Add("SKU", _sku)
        it.Add("STATUS", sOldStatus)
        it.Add("REASONCODE", _holdrc)
        it.Add("UNITPRICE", unitPrice)
        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("LASTSTATUSRC", _holdrc)
        it.Add("LASTMOVEUSER", puser)
        it.Add("LASTSTATUSUSER", puser)
        it.Add("LASTCOUNTUSER", puser)
        it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", puser)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", puser)

        'Dim oLoad As WMS.Logic.Load = New WMS.Logic.Load(_loadid)
        InventoryTransaction.CreateAttributesRecords(it, _loadattributes)
        it.Send(WMS.Lib.Actions.Audit.SHIPLOAD)

        UpdateOrder(WMS.Lib.LoadActivityTypes.SHIPPING, OldActivityType, puser, od, Header, fd, fh, pDocType)
    End Sub

    Public Sub Pack(ByVal pUser As String)
        If _activitystatus = WMS.Lib.Statuses.ActivityStatus.PACKED Then
            Return
        End If
        If Not isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
            Throw m4nEx
        End If
        Dim OldActivityType As String = GetPreviousActivityType()
        _activitystatus = WMS.Lib.Statuses.ActivityStatus.PACKED
        _editdate = DateTime.Now
        _edituser = pUser
        _laststatusdate = DateTime.Now
        DataInterface.RunSQL(String.Format("Update loads set activitystatus = {0},LastStatusDate = {1},editdate = {2},edituser = {3} where {4}", _
            Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))

        Dim aq As New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadPacked)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.PACKLOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.PACKLOAD)

        UpdateOrder(WMS.Lib.LoadActivityTypes.PACKING, OldActivityType, pUser)
    End Sub

    Public Sub UnPack(ByVal pUser As String)
        If _activitystatus <> WMS.Lib.Statuses.ActivityStatus.PACKED Then
            Return
        End If
        If Not isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
            Throw m4nEx
        End If
        Dim OldActivityType As String
        Dim oOrd As OutboundOrderHeader
        Dim SQL As String = String.Format("select * from orderloads where loadid = '{0}'", _loadid)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        oOrd = New OutboundOrderHeader(dt.Rows(0)("consignee"), dt.Rows(0)("orderid"))
        If oOrd.STAGINGLANE = _location And oOrd.STAGINGWAREHOUSEAREA = _warehousearea Then
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.STAGED
            OldActivityType = WMS.Lib.LoadActivityTypes.STAGING
        Else
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.PICKED
            OldActivityType = WMS.Lib.LoadActivityTypes.PICKING
        End If
        _editdate = DateTime.Now
        _edituser = pUser
        _laststatusdate = DateTime.Now
        DataInterface.RunSQL(String.Format("Update loads set activitystatus = {0},LastStatusDate = {1},editdate = {2},edituser = {3} where {4}", _
            Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))

        UpdateOrder(WMS.Lib.LoadActivityTypes.UNPACKING, OldActivityType, pUser)

        Dim aq As New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUnPacked)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNPACKLOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.UNPACKLOAD)
    End Sub

    Public Sub Load(ByVal pDestinationLocation As String, ByVal pDestinationWarehousearea As String, ByVal pUser As String, Optional ByVal od As OutboundOrderHeader.OutboundOrderDetail = Nothing, Optional ByVal Header As OutboundOrderHeader = Nothing, Optional ByVal fd As FlowthroughDetail = Nothing, Optional ByVal fh As Flowthrough = Nothing, Optional ByVal CheckLoadOnContainer As Boolean = False)
        If Not isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
            Throw m4nEx
        End If
        Dim oldStat As String = _status
        Dim OldLoc As String = _location
        Dim OldWarehousearea As String = _warehousearea
        Dim OldActivityType As String = GetPreviousActivityType()
        If OldActivityType.Equals(WMS.Lib.Statuses.ActivityStatus.LOADED, StringComparison.OrdinalIgnoreCase) Then
            Return
        End If
        _activitystatus = WMS.Lib.Statuses.ActivityStatus.LOADED
        _editdate = DateTime.Now
        _edituser = pUser
        _location = pDestinationLocation
        _warehousearea = pDestinationWarehousearea
        _laststatusdate = DateTime.Now
        Dim sql As String = String.Format("Update loads set status = {0},activitystatus = {1},LastStatusDate = {2},location = {3},warehousearea = {4}, editdate = {5},edituser = {6} where {7}", _
            Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_activitystatus), _
            Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location), _
            Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.RunSQL(sql)

        If CheckLoadOnContainer Then
            If Not String.IsNullOrEmpty(Me.ContainerId) Then
                Dim contObj As New WMS.Logic.Container(Me.ContainerId, True)
                If contObj.Loads.Count > 0 Then
                    Me.RemoveFromContainer()
                Else
                    contObj.Location = _location
                    contObj.Warehousearea = _warehousearea
                    contObj.Status = WMS.Lib.Statuses.ActivityStatus.LOADED
                    contObj.Save(pUser)
                End If
            End If
        End If


        Dim aq As New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadLoaded)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOADLOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.LOADLOAD)

        UpdateOrder(WMS.Lib.LoadActivityTypes.LOADING, OldActivityType, pUser, od, Header, fd, fh)
    End Sub

    'Public Sub UnLoad(ByVal pLocation As String, ByVal pWarehoueseArea As String, ByVal pUser As String)
    '    Dim fromLocation As String = _location
    '    Dim fromWarehouseArea As String = _warehousearea

    '    Dim sql As String = String.Format("select consignee,orderid,orderline from orderloads where loadid='{0}'", _loadid)
    '    Dim dt As New DataTable
    '    DataInterface.FillDataset(sql, dt)

    '    Dim oLine As New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dt.Rows(0)("Consignee"), dt.Rows(0)("OrderID"), dt.Rows(0)("OrderLine"))

    '    Dim oHeader As OutboundOrderHeader = Me.GetOutboundOrder()
    '    If oHeader.STAGINGLANE.Equals(pLocation, StringComparison.OrdinalIgnoreCase) AndAlso oHeader.STAGINGWAREHOUSEAREA.Equals(pWarehoueseArea, StringComparison.OrdinalIgnoreCase) Then
    '        _activitystatus = WMS.Lib.Statuses.ActivityStatus.STAGED
    '        oLine.UnLoad(_units, True, pUser)
    '    Else
    '        _activitystatus = WMS.Lib.Statuses.ActivityStatus.PICKED
    '        oLine.UnLoad(_units, False, pUser)
    '    End If
    '    _laststatusdate = DateTime.Now
    '    _location = pLocation
    '    _warehousearea = pWarehoueseArea
    '    _editdate = DateTime.Now
    '    _edituser = pUser
    '    DataInterface.RunSQL(String.Format("Update loads set activitystatus = {0},LastStatusDate = {1},location = {2}, warehousearea={3}, editdate = {4}, edituser = {5} where {6}", _
    '        Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_laststatusdate), _
    '        Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_editdate), _
    '        Made4Net.Shared.Util.FormatField(_edituser), WhereClause))


    '    sendUnloadedEventMessage(fromLocation, fromWarehouseArea, pUser)

    '    'oHeader.Loads.OrderLoad(_loadid).DeleteOrderLoad(WMS.Lib.DOCUMENTTYPES.OUTBOUNDORDER, _consignee, oHeader.ORDERID, _loadid, pUser)

    'End Sub

    Private Sub sendUnloadedEventMessage(ByVal pFromLocation As String, ByVal pFromWarehouseArea As String, ByVal pUser As String)
        Dim aq As New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUnloaded)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.UNLOADLOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", pFromLocation)
        aq.Add("FROMWAREHOUSEAREA", pFromWarehouseArea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.UNLOADLOAD)
    End Sub

    Private Function GetPreviousActivityType() As String
        Select Case _activitystatus
            Case WMS.Lib.Statuses.ActivityStatus.PICKED
                Return WMS.Lib.LoadActivityTypes.PICKING
            Case WMS.Lib.Statuses.ActivityStatus.STAGED
                Return WMS.Lib.LoadActivityTypes.STAGING
            Case WMS.Lib.Statuses.ActivityStatus.PACKED
                Return WMS.Lib.LoadActivityTypes.PACKING
            Case WMS.Lib.Statuses.ActivityStatus.LOADED
                Return WMS.Lib.LoadActivityTypes.LOADING
            Case WMS.Lib.Statuses.ActivityStatus.VERIFIED
                Return WMS.Lib.LoadActivityTypes.VERIFYING

        End Select
        Return String.Empty
    End Function

    Private Sub UpdateOrder(ByVal pActivityType As String, ByVal pPreviousActivty As String, ByVal pUser As String, Optional ByVal od As OutboundOrderHeader.OutboundOrderDetail = Nothing, Optional ByVal Header As OutboundOrderHeader = Nothing, Optional ByVal fd As FlowthroughDetail = Nothing, Optional ByVal fh As Flowthrough = Nothing, Optional ByVal pDocType As String = "")
        Dim sDocType As String = String.Empty
        Dim sConsignee As String = String.Empty
        Dim sOrderId As String = String.Empty
        Dim iOrderline As Int32
        If pDocType = "" Then
            GetDocDetails(sConsignee, sOrderId, iOrderline, sDocType)
        Else
            sDocType = pDocType
        End If
        Select Case sDocType.ToUpper
            Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                Dim oOrdLine As FlowthroughDetail
                If fd Is Nothing Then
                    oOrdLine = New FlowthroughDetail(sConsignee, sOrderId, iOrderline)
                Else
                    oOrdLine = fd
                End If
                If oOrdLine Is Nothing Then Return
                Select Case pActivityType
                    Case WMS.Lib.LoadActivityTypes.LOADING
                        oOrdLine.Load(_units, pPreviousActivty, pUser)
                    Case WMS.Lib.LoadActivityTypes.STAGING
                        oOrdLine.Stage(_units, pPreviousActivty, pUser)
                    Case WMS.Lib.LoadActivityTypes.VERIFYING
                        oOrdLine.Verify(_units, pPreviousActivty, pUser)
                    Case WMS.Lib.LoadActivityTypes.SHIPPING
                        oOrdLine.Ship(_units, pPreviousActivty, pUser, fh)
                End Select
            Case Else
                Dim oOrdLine As OutboundOrderHeader.OutboundOrderDetail
                If od Is Nothing Then
                    oOrdLine = New OutboundOrderHeader.OutboundOrderDetail(sConsignee, sOrderId, iOrderline)
                Else
                    oOrdLine = od
                End If
                If oOrdLine Is Nothing Then Return
                Select Case pActivityType
                    Case WMS.Lib.LoadActivityTypes.PACKING
                        oOrdLine.Pack(_units, pPreviousActivty, pUser)
                    Case WMS.Lib.LoadActivityTypes.UNPACKING
                        oOrdLine.UnPack(_units, pPreviousActivty, pUser)
                    Case WMS.Lib.LoadActivityTypes.LOADING
                        oOrdLine.Load(_units, pPreviousActivty, pUser, Header)
                    Case WMS.Lib.LoadActivityTypes.STAGING
                        oOrdLine.Stage(_units, pPreviousActivty, pUser, Header)
                    Case WMS.Lib.LoadActivityTypes.SHIPPING
                        oOrdLine.Ship(_units, pPreviousActivty, pUser, Header)
                    Case WMS.Lib.LoadActivityTypes.VERIFYING
                        oOrdLine.Verify(_units, pPreviousActivty, pUser)
                End Select
        End Select
    End Sub

    Private Sub GetDocDetails(ByRef pConsignee As String, ByRef pOrderId As String, ByRef pOrderline As Int32, ByRef pDocType As String)
        Dim SQL As String = String.Format("select * from orderloads where loadid = '{0}'", _loadid)
        Dim dt As New DataTable
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            pDocType = dt.Rows(0)("DOCUMENTTYPE")
            pConsignee = dt.Rows(0)("consignee")
            pOrderId = dt.Rows(0)("orderid")
            pOrderline = dt.Rows(0)("orderline")
        End If
    End Sub

#End Region

#Region "Adjustments"

    Public Sub ChangeSku(ByVal pSku As String, ByVal AdjustmentReasonCode As String, ByVal pUser As String, Optional ByVal sHostReferenceID As String = "", Optional ByVal sNotes As String = "")
        If _unitsallocated > 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Can not change sku for loads with allocated quantity", "Can not change sku for loads with allocated quantity")
            Throw m4nEx
        End If
        If pSku = "" Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Sku can not be blank", "Sku can not be blank")
            Throw m4nEx
        End If
        If Not WMS.Logic.SKU.Exists(_consignee, pSku) Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Sku does not exist", "Sku does not exist")
            Throw m4nEx
        End If
        'If Not WMS.Logic.SKU.SKUUOM.Exists(_consignee, pSku, pUOM) Then
        '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Uom does not exist for sku", "Uom does not exist for sku")
        '    Throw m4nEx
        'End If
        If Not isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exist", "Load does not exist")
            Throw m4nEx
        End If
        If hasActivity() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust load", "Cannot adjust load")
            Throw m4nEx
        End If

        _edituser = pUser
        _editdate = DateTime.Now

        Dim porgWeight, porgCube As Decimal
        porgWeight = CalculateWeight()
        porgCube = CalculateVolume()

        Dim sCorrelationId As String = Made4Net.Shared.Util.getNextCounter("CHNGSKUCRL")
        'Send Events for sub qty from the "old sku" load
        'RWMS-344: Begin
        'SendLoadUnitsChangedEvent(WMS.Lib.INVENTORY.SUBQTY, _units, _units, _status, _status, porgWeight, porgCube, AdjustmentReasonCode, pUser, True, sCorrelationId)
        SendLoadUnitsChangedEvent(WMS.Lib.INVENTORY.SUBQTY, _units, _units, _status, _status, porgWeight, porgCube, AdjustmentReasonCode, pUser, True, sCorrelationId, sHostReferenceID, sNotes)
        'RWMS-344: End

        DataInterface.RunSQL(String.Format("UPDATE LOADS SET SKU = {0}, EDITDATE = {1}, EDITUSER = {2} where {3}", Made4Net.Shared.Util.FormatField(pSku), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
        _sku = pSku

        'Send Events for sub qty from the "old sku" load
        'RWMS-344: Begin
        SendLoadUnitsChangedEvent(WMS.Lib.INVENTORY.ADDQTY, _units, _units, _status, _status, porgWeight, porgCube, AdjustmentReasonCode, pUser, True, sCorrelationId, sHostReferenceID, sNotes)
        'SendLoadUnitsChangedEvent(WMS.Lib.INVENTORY.ADDQTY, _units, _units, _status, _status, porgWeight, porgCube, AdjustmentReasonCode, pUser, True, sCorrelationId)
        'RWMS-344: End
    End Sub

    'Public Sub AdjustLimbo(ByVal AdjustmentReasonCode As String, ByVal pUser As String)
    '    If Not IsLimbo Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Incorrect Load Status", "Incorrect Load Status")
    '    End If
    '    Dim delta As Double = Math.Abs(_units)
    '    Dim adjType As String
    '    If _units > 0 Then
    '        adjType = WMS.Lib.INVENTORY.SUBQTY
    '    Else
    '        adjType = WMS.Lib.INVENTORY.ADDQTY
    '    End If
    '    Adjust(adjType, delta, AdjustmentReasonCode, pUser)
    'End Sub

    'Public Sub Adjust(ByVal pAdj As String, ByVal pUnits As Decimal, ByVal AdjustmentReasonCode As String, ByVal pUser As String)
    '    If pUnits < 0 Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Incorrect Units", "Incorrect Units")
    '        Throw m4nEx
    '    End If
    '    If Not isInventory() Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
    '        Throw m4nEx
    '    End If
    '    If hasActivity() Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot adjust load", "Cannot adjust load")
    '        Throw m4nEx
    '    End If
    '    'Validate that load is not allocated to a full pick
    '    Dim sSql As String = String.Format("select COUNT(1) from PICKDETAIL pd inner join PICKHEADER ph on ph.PICKLIST = pd.PICKLIST and ph.PICKTYPE = '{0}' and ph.STATUS in ('{1}','{2}') and pd.FROMLOAD = '{3}'", _
    '            WMS.Lib.PICKTYPE.FULLPICK, WMS.Lib.Statuses.Picklist.PLANNED, WMS.Lib.Statuses.Picklist.RELEASED, _loadid)
    '    If Convert.ToInt32(DataInterface.ExecuteScalar(sSql)) > 0 Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot adjust load - there are open full picks", "Cannot adjust load - there are open full picks")
    '    End If

    '    Dim orgUnits As Decimal = _units
    '    Dim porgWeight, porgCube As Decimal
    '    porgWeight = CalculateWeight()
    '    porgCube = CalculateVolume()

    '    ' Save the old status of the load, if limbo then pre limbo status should be considered
    '    Dim sOldStatus As String
    '    If String.Equals(_status, WMS.Lib.Statuses.LoadStatus.LIMBO, StringComparison.OrdinalIgnoreCase) Then
    '        sOldStatus = _prelimbostatus
    '    Else
    '        sOldStatus = _status
    '    End If

    '    If pAdj = WMS.Lib.INVENTORY.ADDQTY Then

    '        Me.AdjustLoadWeightAttribute(_units, _units + pUnits, Me.WeightBeforeAdjusting, pUser)

    '        _units = _units + pUnits
    '        _edituser = pUser
    '        _editdate = DateTime.Now
    '        DataInterface.RunSQL(String.Format("Update loads set units = units + {0},editdate = {1},edituser = {2} where {3}", Made4Net.Shared.Util.FormatField(pUnits), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
    '    ElseIf pAdj = WMS.Lib.INVENTORY.SUBQTY Then
    '        If pUnits > _units Then
    '            Throw New ApplicationException("Cannot adjust load, adjusted units is greater than load units")
    '        ElseIf _unitsallocated > _units - pUnits Then
    '            Throw New ApplicationException("Cannot adjust load, allocated units is greater than adjusted units")
    '        End If
    '        _edituser = pUser
    '        _editdate = DateTime.Now
    '        _units = _units - pUnits
    '        If _units < 0 Then
    '            Throw New Made4Net.Shared.M4NException(New Exception, "Invalid quantity - qty cannot become negative", "Invalid quantity - cannot become negative")
    '        End If

    '        Me.AdjustLoadWeightAttribute(_units + pUnits, _units, Me.WeightBeforeAdjusting, pUser)

    '        DataInterface.RunSQL(String.Format("Update loads set units = units - {0},editdate = {1},edituser = {2} where {3}", Made4Net.Shared.Util.FormatField(pUnits), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
    '    End If

    '    ' This should update the load in both cases , ADDQTY and SUBQTY
    '    If _units = 0 Then
    '        _activitystatus = Nothing
    '        _status = ""
    '        _laststatusdate = DateTime.Now
    '        _location = ""
    '        _warehousearea = ""
    '        'DataInterface.RunSQL(String.Format("Update loads set status = {0},LastStatusDate = {1},location = {2},warehousearea = {8},activitystatus = {3},units = units - {4},editdate = {5},edituser = {6} where {7}", _
    '        '    Made4Net.Shared.Util.FormatField(_status, ""), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(pUnits), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_warehousearea, "")))
    '        DataInterface.RunSQL(String.Format("Update loads set status = {0},LastStatusDate = {1},location = {2},warehousearea = {8},activitystatus = {3},units = {4},editdate = {5},edituser = {6} where {7}", _
    '        Made4Net.Shared.Util.FormatField(_status, ""), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_warehousearea, "")))

    '        ' Here we need to check if there is container and update this loads
    '        EvacuateEmptyContainer(pUser)
    '    End If

    '    porgWeight = Math.Abs(porgWeight - CalculateWeight())
    '    porgCube = Math.Abs(porgCube - CalculateVolume())
    '    SendLoadUnitsChangedEvent(pAdj, orgUnits, _units, sOldStatus, _status, porgWeight, porgCube, AdjustmentReasonCode, pUser)
    'End Sub

    'Private Sub SendLoadUnitsChangedEvent(ByVal pAdj As String, ByVal pFromUnits As Decimal, ByVal pToUnits As Decimal, ByVal pFromStatus As String, ByVal pToStatus As String, ByVal pCube As Decimal, ByVal pWeight As Decimal, ByVal AdjustmentReasonCode As String, ByVal pUser As String, Optional ByVal pChangeSku As Boolean = False, Optional ByVal pCorrelId As String = "")
    '    Dim unitPrice As Decimal
    '    unitPrice = SKUUnitPrice(_sku, _consignee)

    '    Dim aq As EventManagerQ = New EventManagerQ
    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUnitsChanged)
    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ACTIVITYTIME", "0")
    '    aq.Add("ACTIVITYTYPE", pAdj)
    '    aq.Add("CONSIGNEE", _consignee)
    '    aq.Add("DOCUMENT", _receipt)
    '    aq.Add("DOCUMENTLINE", _receiptline)
    '    aq.Add("FROMLOAD", _loadid)
    '    aq.Add("FROMLOC", _location)
    '    aq.Add("FROMWAREHOUSEAREA", _warehousearea)
    '    aq.Add("FROMQTY", pFromUnits)
    '    aq.Add("FROMSTATUS", pFromStatus)
    '    aq.Add("NOTES", "")
    '    aq.Add("SKU", _sku)
    '    aq.Add("TOLOAD", _loadid)
    '    aq.Add("TOLOC", _location)
    '    aq.Add("TOWAREHOUSEAREA", _warehousearea)
    '    aq.Add("TOQTY", pToUnits)
    '    aq.Add("TOSTATUS", pToStatus)
    '    aq.Add("USERID", pUser)
    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("LASTMOVEUSER", pUser)
    '    aq.Add("LASTSTATUSUSER", pUser)
    '    aq.Add("LASTCOUNTUSER", pUser)
    '    aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("REASONCODE", AdjustmentReasonCode)
    '    aq.Add("CORRELATIONID", pCorrelId)
    '    aq.Add("ADDUSER", pUser)
    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("EDITUSER", pUser)
    '    aq.Send(pAdj)

    '    Dim it As InventoryTransactionQ = New InventoryTransactionQ()
    '    it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    it.Add("INVTRNTYPE", pAdj)
    '    it.Add("CONSIGNEE", _consignee)
    '    it.Add("DOCUMENT", _receipt)
    '    it.Add("LINE", _receiptline)
    '    it.Add("LOADID", _loadid)
    '    it.Add("UOM", _loaduom)
    '    If pChangeSku Then
    '        it.Add("QTY", pToUnits)
    '    Else
    '        it.Add("QTY", Math.Abs(pToUnits - pFromUnits))
    '    End If
    '    it.Add("CUBE", pCube)
    '    it.Add("WEIGHT", pWeight)
    '    it.Add("AMOUNT", "")
    '    it.Add("SKU", _sku)
    '    it.Add("STATUS", pFromStatus)
    '    it.Add("REASONCODE", AdjustmentReasonCode)
    '    it.Add("UNITPRICE", unitPrice)
    '    it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    InventoryTransaction.CreateAttributesRecords(it, _loadattributes)
    '    it.Add("LASTMOVEUSER", pUser)
    '    it.Add("LASTSTATUSUSER", pUser)
    '    it.Add("LASTCOUNTUSER", pUser)
    '    it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    it.Add("ADDUSER", pUser)
    '    it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    it.Add("EDITUSER", pUser)
    '    it.Send(pAdj)
    'End Sub

    Private Sub SendLoadUnitsChangedEvent(ByVal pAdj As String, ByVal pFromUnits As Decimal, ByVal pToUnits As Decimal, ByVal pFromStatus As String, ByVal pToStatus As String, ByVal pCube As Decimal, ByVal pWeight As Decimal, ByVal AdjustmentReasonCode As String, ByVal pUser As String, Optional ByVal pChangeSku As Boolean = False, Optional ByVal pCorrelId As String = "", Optional ByVal sHostReferenceID As String = "", Optional ByVal sNotes As String = "")
        Dim unitPrice As Decimal
        unitPrice = SKUUnitPrice(_sku, _consignee)

        'Begin: RWMS-344
        'Code changed to post to Inventory transaction queue first, then retrieve that ID and post it to SCEXpert connect to include that as part of
        'Inventory Transaction XML file that gets created under "RWMS\Interfaces\Export" folder
        Dim dtIT As New DataTable
        Dim drIT As DataRow
        Dim sInvTransId As String = String.Empty
        Dim sInvTransIdNew As String = String.Empty
        Dim SQL As String = String.Format("SELECT INVTRANS,LOADID from INVENTORYTRANS where LOADID ='{0}' order by INVTRANS desc", _loadid)

        DataInterface.FillDataset(SQL, dtIT)
        If dtIT.Rows.Count > 0 Then
            drIT = dtIT.Rows(0)
            sInvTransId = drIT("INVTRANS").ToString()
        End If
        'End: RWMS-344

        Dim it As InventoryTransactionQ = New InventoryTransactionQ()

        'Post message to Inventory transaction service queue to insert the adjustment
        'Begin: RWMS-344
        it.Add("INVTRANSID", sInvTransId)
        it.Add("HOSTREFERENCEID", sHostReferenceID)
        it.Add("NOTES", sNotes)
        'End: RWMS-344

        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", pAdj)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", _receipt)
        it.Add("LINE", _receiptline)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        If pChangeSku Then
            it.Add("QTY", pToUnits)
        Else
            it.Add("QTY", Math.Abs(pToUnits - pFromUnits))
        End If
        it.Add("CUBE", pCube)
        it.Add("WEIGHT", pWeight)
        it.Add("AMOUNT", "")
        it.Add("SKU", _sku)
        it.Add("STATUS", pFromStatus)
        it.Add("REASONCODE", AdjustmentReasonCode)
        it.Add("UNITPRICE", unitPrice)
        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        InventoryTransaction.CreateAttributesRecords(it, _loadattributes)
        it.Add("LASTMOVEUSER", pUser)
        it.Add("LASTSTATUSUSER", pUser)
        it.Add("LASTCOUNTUSER", pUser)
        it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", pUser)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", pUser)
        it.Send(pAdj)

        'Begin: RWMS-344
        'Retrieve the above
        'Compare the above inventory transaction id that was retried before new INSERT to INVENTORYTRANS table with the below new inventory transaction id in a loop by sleeping for one second each iteration

        SQL = String.Format("SELECT INVTRANS from INVENTORYTRANS where LOADID ='{0}' order by INVTRANS desc", _loadid)
        sInvTransIdNew = sInvTransId
        Do
            'sleep for 1 seconds
            Threading.Thread.Sleep(1000)
            dtIT = New DataTable
            DataInterface.FillDataset(SQL, dtIT)
            If dtIT.Rows.Count > 0 Then
                drIT = dtIT.Rows(0)
                sInvTransIdNew = drIT("INVTRANS").ToString()
            End If
            dtIT = Nothing
        Loop Until (Not sInvTransId.Equals(sInvTransIdNew))
        'End: RWMS-344

        'Post message to EVENT SERVICE that will be processed by the SCExpert
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUnitsChanged)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("INVTRANSID", sInvTransIdNew) 'New id should be sent
        aq.Add("ACTIVITYTYPE", pAdj)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", pFromUnits)
        aq.Add("FROMSTATUS", pFromStatus)
        'aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", pToUnits)
        aq.Add("TOSTATUS", pToStatus)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("REASONCODE", AdjustmentReasonCode)
        aq.Add("CORRELATIONID", pCorrelId)
        aq.Add("ADDUSER", pUser)
        'Begin: RWMS-344
        aq.Add("NOTES", sNotes.Trim())
        aq.Add("HOSTREFERENCEID", sHostReferenceID)
        'End: RWMS-344

        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)

        aq.Send(pAdj)
    End Sub

    Public Sub ChangeUom(ByVal pUom As String, ByVal pReason As String, ByVal pUser As String)
        If hasActivity() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load uom activity", "Cannot Change load uom activity")
            Throw m4nEx
        End If
        If Not isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
            Throw m4nEx
        End If
        If _unitsallocated > 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load uom qty allocated", "Cannot Change load uom qty allocated")
            Throw m4nEx
        End If
        Dim oSku As New WMS.Logic.SKU(_consignee, _sku)
        If oSku.UNITSOFMEASURE.UOM(pUom) Is Nothing Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load uom invalid uom", "Cannot Change load uom invalid uom")
            Throw m4nEx
        Else
            _loaduom = pUom
            _editdate = DateTime.Now
            _edituser = pUser

            DataInterface.RunSQL(String.Format("Update loads set loaduom={0},editdate={1},edituser={2} where {3}", _
            Made4Net.Shared.Util.FormatField(_loaduom), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))

            'Dim aq As New EventManagerQ
            'Dim action As String = WMS.Lib.Actions.Audit.CHANGEUOM
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.LoadUomChanged
            'aq.Add("EVENT", EventType)
            'aq.Add("ACTION", action)
            'aq.Add("USERID", pUser)
            'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'aq.Add("LOADID", _loadid)
            'aq.Add("CONSIGNEE", _consignee)
            'aq.Add("SKU", _sku)
            'aq.Send(action)

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadUomChanged)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CHANGEUOM)
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _receipt)
            aq.Add("DOCUMENTLINE", _receiptline)
            aq.Add("FROMLOAD", _loadid)
            aq.Add("FROMLOC", _location)
            aq.Add("FROMWAREHOUSEAREA", _warehousearea)
            aq.Add("FROMQTY", _units)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", _sku)
            aq.Add("TOLOAD", _loadid)
            aq.Add("TOWAREHOUSEAREA", _warehousearea)
            aq.Add("TOLOC", _location)
            aq.Add("TOQTY", _units)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("REASONCODE", pReason)

            aq.Add("LASTMOVEUSER", pUser)
            aq.Add("LASTSTATUSUSER", pUser)
            aq.Add("LASTCOUNTUSER", pUser)
            aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

            aq.Add("ADDUSER", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.CHANGEUOM)

        End If
    End Sub

    Public Sub setStatus(ByVal pNewStat As String, ByVal pReason As String, ByVal pUser As String)
        If _status.ToLower <> pNewStat.ToLower Then
            If hasActivity() Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change status, load is assigned to another activity", "Cannot Change status, load is assigned to another activity")
                Throw m4nEx
            End If
            If Not isInventory() Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
                Throw m4nEx
            End If
            If _unitsallocated > 0 Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load status qty allocated", "Cannot Change load status qty allocated")
                Throw m4nEx
            End If
            Dim strOldStat As String = _status
            _editdate = DateTime.Now
            _edituser = pUser
            _status = pNewStat
            _holdrc = pReason
            _laststatusdate = DateTime.Now
            DataInterface.RunSQL(String.Format("Update loads set status={0},Holdrc={1},LastStatusDate={2},editdate={3},edituser={4} where {5}", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_holdrc), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))

            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadStatusChanged)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SETSTATUS)
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _receipt)
            aq.Add("DOCUMENTLINE", _receiptline)
            aq.Add("FROMLOAD", _loadid)
            aq.Add("FROMLOC", _location)
            aq.Add("FROMWAREHOUSEAREA", _warehousearea)
            aq.Add("FROMQTY", _units)
            aq.Add("FROMSTATUS", strOldStat)
            aq.Add("REASONCODE", pReason)
            aq.Add("NOTES", "")
            aq.Add("SKU", _sku)
            aq.Add("TOLOAD", _loadid)
            aq.Add("TOLOC", _location)
            aq.Add("TOWAREHOUSEAREA", _warehousearea)
            aq.Add("TOQTY", _units)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("LASTMOVEUSER", pUser)
            aq.Add("LASTSTATUSUSER", pUser)
            aq.Add("LASTCOUNTUSER", pUser)
            aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.SETSTATUS)
        End If
    End Sub

    'Public Sub Move(ByVal pNewLoc As String, ByVal pNewWarehousearea As String, ByVal pSubLocation As String, ByVal pUser As String)
    '    If Not WMS.Logic.Location.Exists(pNewLoc, pNewWarehousearea) Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot relocate load", "Cannot relocate load")
    '        Throw m4nEx
    '    End If
    '    If Not isInventory() Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
    '        Throw m4nEx
    '    End If
    '    If hasActivity() Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load location", "Cannot Change load location")
    '        Throw m4nEx
    '    End If
    '    If _unitsallocated > 0 Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load location qty allocated", "Cannot Change load location qty allocated")
    '        Throw m4nEx
    '    End If
    '    Dim strOldLoc As String = _location
    '    Dim strOldWarehousearea As String = _warehousearea

    '    Me.Put(pNewLoc, pNewWarehousearea, pSubLocation, pUser)

    '    Dim oWHActivity As New WHActivity()
    '    oWHActivity.LOCATION = pNewLoc
    '    oWHActivity.USERID = pUser
    '    oWHActivity.ACTIVITY = WMS.Lib.Actions.Audit.MOVELOAD
    '    oWHActivity.ACTIVITYTIME = DateTime.Now
    '    oWHActivity.EDITDATE = DateTime.Now
    '    oWHActivity.EDITUSER = pUser
    '    oWHActivity.Post()

    '    Dim aq As EventManagerQ = New EventManagerQ
    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadLocationChanged)
    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ACTIVITYTIME", "0")
    '    aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.MOVELOAD)
    '    aq.Add("CONSIGNEE", _consignee)
    '    aq.Add("DOCUMENT", _receipt)
    '    aq.Add("DOCUMENTLINE", _receiptline)
    '    aq.Add("FROMLOAD", _loadid)
    '    aq.Add("FROMLOC", strOldLoc)
    '    aq.Add("FROMWAREHOUSEAREA", strOldWarehousearea)
    '    aq.Add("FROMQTY", _units)
    '    aq.Add("FROMSTATUS", _status)
    '    aq.Add("NOTES", "")
    '    aq.Add("SKU", _sku)
    '    aq.Add("TOLOAD", _loadid)
    '    aq.Add("TOLOC", _location)
    '    aq.Add("TOWAREHOUSEAREA", _warehousearea)
    '    aq.Add("TOQTY", _units)
    '    aq.Add("TOSTATUS", _status)
    '    aq.Add("USERID", pUser)
    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

    '    aq.Add("LASTMOVEUSER", pUser)
    '    aq.Add("LASTSTATUSUSER", pUser)
    '    aq.Add("LASTCOUNTUSER", pUser)
    '    aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))


    '    aq.Add("ADDUSER", pUser)
    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("EDITUSER", pUser)
    '    aq.Send(WMS.Lib.Actions.Audit.MOVELOAD)

    '    'Dim aq As New EventManagerQ
    '    'Dim action As String = WMS.Lib.Actions.Audit.MOVELOAD
    '    'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.LoadLocationChanged
    '    'aq.Add("EVENT", EventType)
    '    'aq.Add("ACTION", action)
    '    'aq.Add("USERID", pUser)
    '    'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    'aq.Add("LOADID", _loadid)
    '    'aq.Add("QTY", _units)
    '    'aq.Add("CONSIGNEE", _consignee)
    '    'aq.Add("SKU", _sku)
    '    'aq.Add("FROMLOCATION", strOldLoc)
    '    'aq.Add("TOLOCATION", _location)
    '    'aq.Add("STATUS", _status)
    '    'aq.Send(action)
    'End Sub

    'Public Sub Split(ByVal pNewLoc As String, ByVal pNewWarehousearea As String, ByVal pSplitQty As Decimal, ByVal pNewLoadId As String, ByVal pUser As String)
    '    If pSplitQty <= 0 Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cant Split Load", "Cant Split load")
    '        Throw m4nEx
    '    End If
    '    If Not isInventory() Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
    '        Throw m4nEx
    '    End If
    '    If Exists(pNewLoadId) Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load Id already exists", "Load Id already exists")
    '        Throw m4nEx
    '    End If
    '    If hasActivity() Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Change load location", "Cannot Change load location")
    '        Throw m4nEx
    '    End If
    '    If _units - _unitsallocated < pSplitQty Then
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot Split load", "Cannot Split load")
    '        Throw m4nEx
    '    End If
    '    If String.IsNullOrEmpty(pNewLoc) AndAlso String.IsNullOrEmpty(pNewWarehousearea) Then
    '        pNewLoc = _location
    '        pNewWarehousearea = _warehousearea
    '    End If
    '    If Not WMS.Logic.Location.Exists(pNewLoc, pNewWarehousearea) Then
    '        'pNewLoc = _location
    '        'pNewWarehousearea = _warehousearea
    '        Throw New M4NException(New Exception(), "Location does not exist", "Location does not exist")
    '    End If
    '    If pNewLoadId.Trim = "" Then
    '        pNewLoadId = GenerateLoadId()
    '    End If

    '    _editdate = DateTime.Now
    '    _edituser = pUser
    '    _units = _units - pSplitQty

    '    Dim newLoadStatus As String = _status
    '    If _units = 0 Then
    '        Delete(pUser)
    '    Else
    '        DataInterface.RunSQL(String.Format("Update loads set units = units - {0},editdate = {1},edituser = {2} where {3}", Made4Net.Shared.Util.FormatField(pSplitQty), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))
    '    End If
    '    Dim oldLoc As String = _location
    '    Dim oldWarehousearea As String = _warehousearea
    '    Dim oldLoadId As String = _loadid
    '    Dim oldQty As String = _units
    '    Dim oldReceiveDate As String = _receivedate


    '    Dim originalWeight As Decimal = Me.WeightBeforeAdjusting
    '    Dim originalQty As Decimal = _units + pSplitQty
    '    Me.AdjustLoadWeightAttribute(originalQty, _units, originalWeight, pUser)


    '    CreateLoad(pNewLoadId, _consignee, _sku, _loaduom, _location, pNewWarehousearea, newLoadStatus, _activitystatus, pSplitQty, _receipt, _receiptline, Nothing, _loadattributes.Attributes, pUser, "", oldReceiveDate, _prelimboloc, _prelimbostatus)

    '    Me.AdjustLoadWeightAttribute(originalQty, pSplitQty, originalWeight, pUser)

    '    Move(pNewLoc, pNewWarehousearea, "", pUser)





    '    Dim aq As EventManagerQ = New EventManagerQ
    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadSplit)
    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ACTIVITYTIME", "0")
    '    aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SPLITLOAD)
    '    aq.Add("CONSIGNEE", _consignee)
    '    aq.Add("DOCUMENT", _receipt)
    '    aq.Add("DOCUMENTLINE", _receiptline)
    '    aq.Add("FROMLOAD", oldLoadId)
    '    aq.Add("FROMLOC", oldLoc)
    '    aq.Add("FROMWAREHOUSEAREA", oldWarehousearea)
    '    aq.Add("FROMQTY", oldQty + pSplitQty)
    '    aq.Add("FROMSTATUS", _status)
    '    aq.Add("NOTES", "")
    '    aq.Add("SKU", _sku)
    '    aq.Add("TOLOAD", pNewLoadId)
    '    aq.Add("TOLOC", pNewLoc)
    '    aq.Add("TOWAREHOUSEAREA", pNewWarehousearea)
    '    aq.Add("TOQTY", pSplitQty)
    '    aq.Add("TOSTATUS", _status)
    '    aq.Add("USERID", pUser)
    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

    '    aq.Add("LASTMOVEUSER", pUser)
    '    aq.Add("LASTSTATUSUSER", pUser)
    '    aq.Add("LASTCOUNTUSER", pUser)
    '    aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

    '    aq.Add("ADDUSER", pUser)
    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("EDITUSER", pUser)
    '    aq.Send(WMS.Lib.Actions.Audit.SPLITLOAD)

    '    'Dim aq As New EventManagerQ
    '    'Dim action As String = WMS.Lib.Actions.Audit.SPLITLOAD
    '    ''Added by lev , Load split event sending
    '    'aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadSplit)
    '    'aq.Add("ACTION", action)
    '    'aq.Add("USERID", pUser)
    '    'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    'aq.Add("FROMLOADID", oldLoadId)
    '    'aq.Add("TOLOADID", pNewLoadId)
    '    'aq.Add("FROMQTY", oldQty + pSplitQty)
    '    'aq.Add("TOQTY", pSplitQty)
    '    'aq.Add("CONSIGNEE", _consignee)
    '    'aq.Add("SKU", _sku)
    '    'aq.Add("FROMLOCATION", oldLoc)
    '    'aq.Add("TOLOCATION", pNewLoc)
    '    'aq.Add("STATUS", _status)
    '    'aq.Send(action)

    'End Sub

    Public Sub SplitReplenish(ByVal pSplitQty As Decimal, ByVal pUom As String, ByVal pNewLoadId As String, ByVal pUser As String, Optional ByVal isHandOff As Boolean = False)

        Dim oldActStatus As String = _activitystatus
        Dim oldDestLocation As String = _destinationlocation
        Dim oldDestinationWarehousearea As String = _destinationwarehousearea
        Dim originalUnits As Decimal = _units
        Dim OrigLoadId As String = _loadid
        Dim ShouldDeleteOrigLoad As Boolean = False
        _editdate = DateTime.Now
        _edituser = pUser
        If _units - pSplitQty < 0 Then
            Throw New M4NException(New Exception, "Quantity cannot become negative", "Quantity cannot become negative")
        ElseIf _units - pSplitQty = 0 Then
            ShouldDeleteOrigLoad = True
        End If
        _units = _units - pSplitQty
        _activitystatus = Nothing
        _destinationlocation = Nothing
        _destinationwarehousearea = Nothing
        _unitsallocated = 0
        Dim originalStatus As String = _status
        DataInterface.RunSQL(String.Format("Update loads set units = {0},activitystatus={1},unitsallocated={2}, destinationlocation={3}, destinationwarehousearea={7}, editdate = {4},edituser = {5} where {6}", Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_destinationwarehousearea)))
        If ShouldDeleteOrigLoad Then
            Delete(pUser)
            'Else
            '    Dim orgLoad As New Load(OrigLoadId)
            '    orgLoad.UNITSALLOCATED = 0
            '    orgLoad.Delete(pUser)
        End If

        Dim originalWeight As Decimal = Me.WeightBeforeAdjusting
        'AdjustLoadWeightAttribute(OrigLoadId, originalUnits, _units, originalWeight, pUser)
        AdjustLoadWeightAttribute(originalUnits, _units, originalWeight, pUser)

        CreateLoad(pNewLoadId, _consignee, _sku, pUom, _location, _warehousearea, originalStatus, _activitystatus, pSplitQty, _receipt, _receiptline, Nothing, _loadattributes.Attributes, pUser, "", _receivedate)

        'adjustLoadWeight(pNewLoadId, originalUnits, _units, originalWeight, pUser)
        AdjustLoadWeightAttribute(originalUnits, _units, originalWeight, pUser)


        If isHandOff Then
            SetActivityStatus(oldActStatus, pUser)
            SetDestinationLocation(oldDestLocation, oldDestinationWarehousearea, pUser)
        End If


        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.PartialReplenishment)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.SPLITREPL)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", pNewLoadId)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units - pSplitQty)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.SPLITREPL)

        ' Added by Lev : Send Partial replenishment message to event manager
        'Dim aq As New EventManagerQ
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.PartialReplenishment
        'aq.Add("EVENT", EventType)
        'aq.Add("USERID", _editdate)
        'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("FROMLOAD", _loadid)
        'aq.Add("TOLOAD", pNewLoadId)
        'aq.Add("QTY", _units)
        'aq.Add("CONSIGNEE", _consignee)
        'aq.Add("SKU", _sku)
        'aq.Add("LOCATION", _location)
        'aq.Add("STATUS", _status)
        'aq.Add("REASONCODE", _holdrc)
        'aq.Send(WMS.Logic.WMSEvents.EventType.PartialReplenishment)
    End Sub

    Public Sub DeleteLoad(ByVal pReasonCode As String, ByVal pUser As String)
        If hasActivity() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot delete load while load assign to activity", "Cannot delete load while load assign to activity")
            Throw m4nEx
        End If
        Delete(pUser)

        Dim porgWeight, porgCube As Decimal
        porgWeight = CalculateWeight()
        porgCube = CalculateVolume()

        Dim unitPrice As Decimal
        unitPrice = SKUUnitPrice(_sku, _consignee)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadDelete)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.DELETELOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("REASONCODE", pReasonCode)

        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.DELETELOAD)

        'Dim aq As New EventManagerQ
        'Dim action As String = WMS.Lib.Actions.Audit.DELETELOAD
        'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.LoadDelete
        'aq.Add("EVENT", EventType)
        'aq.Add("ACTION", action)
        'aq.Add("USERID", pUser)
        'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        'aq.Add("LOADID", _loadid)
        'aq.Add("QTY", _units)
        'aq.Add("CONSIGNEE", _consignee)
        'aq.Add("SKU", _sku)
        'aq.Add("LOCATION", _location)
        'aq.Add("STATUS", _status)
        '' ----- Added by Lev , to inform InvTrans about the reason code
        'aq.Add("REASONCODE", _holdrc)
        ''---------------------
        'aq.Add("WEIGHT", porgWeight)
        'aq.Add("UNITPRICE", unitPrice)
        'aq.Add("CUBE", porgCube)
        'aq.Send(action)

    End Sub

    Public Sub CancelReceive(ByVal pReasonCode As String, ByVal puser As String)
        If hasActivity() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot delete load while load assign to activity", "Cannot delete load while load assign to activity")
            Throw m4nEx
        End If
        Delete(puser, pReasonCode)
    End Sub

    Private Sub Delete(ByVal pUser As String, Optional ByVal pReasonCode As String = "", Optional ByVal pRemoveFromContainer As Boolean = True)
        If Not isInventory() Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
            Throw m4nEx
        End If
        If _unitsallocated > 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot delete load allocated quantity", "Cannot delete load allocated quantity")
            Throw m4nEx
        End If
        Dim oldStat As String = _status
        Dim OldLoc As String = _location
        Dim OldWarehousearea As String = _warehousearea

        _status = WMS.Lib.Statuses.LoadStatus.NONE
        _activitystatus = WMS.Lib.Statuses.ActivityStatus.NONE
        _editdate = DateTime.Now
        _edituser = pUser
        _location = ""
        _warehousearea = ""
        _laststatusdate = DateTime.Now

        'Here we need to check if there is container and update this loads
        'EvacuateEmptyContainer(pUser)
        If pRemoveFromContainer Then
            RemoveFromContainer()
        End If

        DataInterface.RunSQL(String.Format("Update loads set status = {0},activitystatus = {1},LastStatusDate = {2},location = {3},editdate = {4},edituser = {5} where {6}", Made4Net.Shared.Util.FormatField(_status, ""), _
            Made4Net.Shared.Util.FormatField(_activitystatus, ""), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadDelete)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.DELETELOAD)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("REASONCODE", pReasonCode)

        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.DELETELOAD)
    End Sub

    Public Sub ReturnShippedLoadsToInventory(ByVal pStatus As String, ByVal pLocation As String, ByVal pWarehouseArea As String, ByVal pLastStatusRc As String, ByVal pUser As String)
        _status = pStatus
        _location = pLocation
        _warehousearea = pWarehouseArea

        If Not WMS.Logic.Location.Exists(_location, _warehousearea) Then
            Throw New M4NException(New Exception, "Location does not exist.", "Location does not exist.")
        End If
        _edituser = pUser
        _laststatusdate = DateTime.Now
        _laststatususer = pUser
        _laststatusrc = pLastStatusRc
        _editdate = DateTime.Now

        Dim sql As String = String.Format("Update Loads set Status={0},Location={1},WarehouseArea={2},EditUser={3},EditDate={4},LastStatusDate={5}," & _
                    "LastStatusUser={6},laststatusrc={7} Where {8}", _
                    Made4Net.Shared.FormatField(_status), Made4Net.Shared.FormatField(_location), Made4Net.Shared.FormatField(_warehousearea), _
                    Made4Net.Shared.FormatField(_edituser), Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_laststatusdate), _
                    Made4Net.Shared.FormatField(_laststatususer), Made4Net.Shared.FormatField(_laststatusrc), WhereClause)
        DataInterface.RunSQL(sql)
    End Sub



    'Public Sub AdjustLoadWeightAttribute(ByVal pLoadID As String, ByVal pFromUnits As Decimal, ByVal pToUnits As String, ByVal pFromWeight As Decimal, ByVal pUser As String)
    Public Sub AdjustLoadWeightAttribute(ByVal pFromUnits As Decimal, ByVal pToUnits As String, ByVal pFromWeight As Decimal, ByVal pUser As String)
        If pFromWeight <= 0 Then
            Return
        End If

        'Dim loadObj As New Load(pLoadID)
        If pFromUnits <= 0 Then
            Return
        End If
        Dim weightDensity As Decimal = pFromWeight / pFromUnits
        Me.LoadAttributes.Attribute("WEIGHT") = weightDensity * pToUnits
        Me.LoadAttributes.Save(pUser)
        'loadObj.LoadAttributes.Attribute("WEIGHT") = weightDensity * pToUnits
        'loadObj.LoadAttributes.Save(pUser)
    End Sub

    Public ReadOnly Property WeightBeforeAdjusting() As Decimal
        Get
            Try
                If Not Me.LoadAttributes Is Nothing Then
                    If Not Me.LoadAttributes.Attribute("WEIGHT") Is Nothing Then
                        Dim originalWeight As Decimal = Me.LoadAttributes.Attribute("WEIGHT")
                        Return originalWeight
                    End If
                End If
                Return 0
            Catch ex As Exception
                Return 0
            End Try
        End Get
    End Property
#End Region

#Region "Counting"

    Public Shared Function ShouldVerifyCountingAttributes(ByVal pLoadId As String, ByVal oAttributesCollection As WMS.Logic.AttributesCollection) As Boolean
        Dim ld As New WMS.Logic.Load(pLoadId, True)
        Dim oSku As String = ld.SKU
        Dim oConsignee As String = ld.CONSIGNEE
        Dim oSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        ' No attributes , return validated
        If oSkuClass Is Nothing Then Return True
        Try
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                Dim typeValidationResult As Int32

                If oLoadAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Capture Then
                    If oAttributesCollection Is Nothing Then Continue For
                    If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value OrElse (oLoadAtt.Type = AttributeType.String AndAlso oAttributesCollection(oLoadAtt.Name) = "") Then Continue For
                ElseIf oLoadAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oAttributesCollection Is Nothing Then Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                    If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                        Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                    End If
                End If
                If oLoadAtt.CaptureAtCounting <> SkuClassLoadAttribute.CaptureType.NoCapture Then
                    ' Validate that the attributes supplied are valid
                    typeValidationResult = ValidateAttributeByType(oLoadAtt, oAttributesCollection(oLoadAtt.Name))
                    If typeValidationResult = -1 Then
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Attribute #param0# is not Valid", "Attribute #param0# is not Valid")
                        m4nEx.Params.Add("AttName", oLoadAtt.Name)
                        Throw m4nEx
                    ElseIf typeValidationResult = 0 Then
                        'Continue For
                    End If
                    ' Validator
                    If Not oLoadAtt.CountingValidator Is Nothing Then
                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                        vals.Add(oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name))
                        vals.Add("LOADID", pLoadId)
                        Dim ret As String = oLoadAtt.Evaluate(SkuClassLoadAttribute.EvaluationType.Counting, vals)
                        Dim returnedResponse() As String = ret.Split(";")
                        'If ret = "-1" Then
                        If returnedResponse(0) = "-1" Then
                            If returnedResponse.Length > 1 Then
                                Throw New M4NException(New Exception, "Invalid Attribute Value " & oLoadAtt.Name & ". " & returnedResponse(1), "Invalid Attribute Value " & oLoadAtt.Name & "." & returnedResponse(1))
                            Else
                                Throw New M4NException(New Exception, "Invalid Attribute Value " & oLoadAtt.Name, "Invalid Attribute Value " & oLoadAtt.Name)
                            End If
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                    End If

                    'Now need to match the values of the attribues

                    Dim Value As Object
                    Value = oAttributesCollection(oLoadAtt.Name)
                    If Not Value Is Nothing And Not Value Is System.DBNull.Value Then
                        Try
                            If TypeOf Value Is System.DateTime Then
                                If Convert.ToDateTime(oAttributesCollection(oLoadAtt.Name)).Date <> Convert.ToDateTime(ld.LoadAttributes.Attributes(oLoadAtt.Name)).Date Then
                                    Return False
                                End If
                            Else
                                If oAttributesCollection(oLoadAtt.Name) <> ld.LoadAttributes.Attributes(oLoadAtt.Name) Then
                                    Return False
                                End If
                            End If
                        Catch ex As Exception
                            If oLoadAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Required Then
                                Return False
                            End If
                        End Try
                    End If
                End If
            Next
        Catch ex As Made4Net.Shared.M4NException
            Throw ex
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Shared Function ValidateAttributeByType(ByVal oAtt As SkuClassLoadAttribute, ByVal oAttVal As Object) As Int32
        Select Case oAtt.Type
            Case WMS.Logic.AttributeType.DateTime
                Dim Val As DateTime
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, DateTime)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case WMS.Logic.AttributeType.Decimal
                Dim Val As Decimal
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Decimal)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case WMS.Logic.AttributeType.Integer
                Dim Val As Int32
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Int32)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case WMS.Logic.AttributeType.String
                Try
                    If oAttVal Is Nothing Or Convert.ToString(oAttVal) = "" Then
                        If oAtt.CaptureAtCounting = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
        End Select
    End Function

    'Public Sub Count(ByVal toQty As Decimal, ByVal toUom As String, ByVal toLoc As String, ByVal toWarehousearea As String, ByVal toAtt As WMS.Logic.AttributesCollection, ByVal pUser As String)
    '    Dim oSku As New SKU(_consignee, _sku)
    '    Try
    '        toQty = oSku.ConvertToUnits(toUom) * toQty
    '    Catch ex As Exception
    '        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid load UOM", "Invalid load UOM")
    '        Throw m4nEx
    '    End Try
    '    Count(toQty, toLoc, toWarehousearea, toAtt, pUser)
    'End Sub

    'Public Sub Count(ByVal toQty As Decimal, ByVal toLoc As String, ByVal toWarehousearea As String, ByVal toAtt As WMS.Logic.AttributesCollection, ByVal pUser As String)
    '    If Not isInventory() Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Load does not exists", "Load does not exists")
    '    End If
    '    If hasActivity() Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Load assigned to another activity", "Load assigned to another activity")
    '    End If
    '    If toQty < 0 Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Counted qty can't be less than 0", "Counted qty can't be less than 0")
    '    End If
    '    If _status = WMS.Lib.Statuses.LoadStatus.LIMBO AndAlso toQty = 0 Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Counted qty can not be 0 for limbo load", "Counted qty can not be 0 for limbo load")
    '    End If
    '    If Not WMS.Logic.Location.Exists(toLoc, toWarehousearea) Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Location does not exists", "Location does not exists")
    '    End If
    '    If _units < 0 Then
    '        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot count this load", "Cannot count this load")
    '    End If
    '    'Check if inventory adjustment is needed instead of count
    '    Dim diff As Decimal = _units - toQty
    '    Dim oSku As WMS.Logic.SKU
    '    If diff <> 0 Then
    '        Dim adjType As String
    '        If oSku Is Nothing Then oSku = New SKU(_consignee, _sku)
    '        If oSku.AUTOADJUSTCOUNTQTY >= Math.Abs(diff) Then
    '            If diff > 0 Then
    '                adjType = WMS.Lib.INVENTORY.SUBQTY
    '            Else
    '                adjType = WMS.Lib.INVENTORY.ADDQTY
    '            End If
    '            Adjust(adjType, Math.Abs(diff), "", pUser)
    '            Return
    '        End If
    '    End If
    '    Dim OrgUnits As Decimal = _units
    '    Dim OrgLoc As String = _location
    '    Dim OrgWarehousearea As String = _warehousearea
    '    Dim FromStat As String = _status

    '    Dim locChanged As Boolean = False
    '    Dim ldid As String
    '    If toQty <> _units Then
    '        If toQty = 0 Then
    '            sendToLimbo(pUser)
    '        Else
    '            ldid = GenerateLoadId()
    '            Dim newld As New Load

    '            'Dim originalWeight As Decimal
    '            'If Not toAtt Is Nothing AndAlso Not toAtt("WEIGHT") Is Nothing Then
    '            'originalWeight = Me.WeightBeforeAdjusting - toAtt("WEIGHT")
    '            ' OrgUnits = _units - toQty
    '            'Else
    '            'originalWeight = Me.WeightBeforeAdjusting
    '            'Me.AdjustLoadWeightAttribute(OrgUnits, toQty, originalWeight, pUser)
    '            'Try
    '            'toAtt("WEIGHT") = Me.LoadAttributes.Attribute("WEIGHT")
    '            'Catch
    '            'End Try
    '            'End If
    '            Dim originalWeight As Decimal = Me.WeightBeforeAdjusting
    '            Me.AdjustLoadWeightAttribute(OrgUnits, toQty, originalWeight, pUser)

    '            newld.CreateLoad(ldid, _consignee, _sku, _loaduom, _location, _warehousearea, _status, Nothing, _units - toQty, _receipt, _receiptline, Nothing, _loadattributes.Attributes, pUser, "", New DateTime(1900, 1, 1), _prelimboloc, _prelimbostatus)
    '            If newld.STATUS <> WMS.Lib.Statuses.LoadStatus.LIMBO Then
    '                newld.sendToLimbo(pUser)
    '                newld.AdjustLoadWeightAttribute(OrgUnits, newld.UNITS, originalWeight, pUser)
    '            End If
    '            _units = toQty
    '            _location = toLoc
    '            _warehousearea = toWarehousearea
    '            If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Then
    '                _status = _prelimbostatus
    '                _prelimbostatus = ""
    '                _prelimboloc = ""
    '                _prelimbowarehousearea = ""
    '            End If
    '            ' Check if attributes changed and send attribute event
    '            If Not toAtt Is Nothing Then
    '                'If Not WMS.Logic.Load.ShouldVerifyCountingAttributes(_loadid, toAtt) Then
    '                ' Validate Attributes

    '                ' Set Attributes
    '                Try
    '                    toAtt("WEIGHT") = Me.LoadAttributes.Attribute("WEIGHT")
    '                Catch
    '                End Try

    '                setAttributes(toAtt, pUser)
    '                'End If
    '            End If
    '        End If
    '    Else
    '        ' Check if attributes changed and send attribute event
    '        If Not toAtt Is Nothing Then
    '            If Not WMS.Logic.Load.ShouldVerifyCountingAttributes(_loadid, toAtt) Then
    '                'Validate(Attributes)

    '                'Set Attributes
    '                Try
    '                    toAtt("WEIGHT") = Me.LoadAttributes.Attribute("WEIGHT")
    '                Catch
    '                End Try
    '                setAttributes(toAtt, pUser)
    '            End If
    '        End If
    '        _location = toLoc
    '        _warehousearea = toWarehousearea
    '        If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Then
    '            _status = _prelimbostatus
    '            _prelimbostatus = ""
    '            _prelimboloc = ""
    '            _prelimbowarehousearea = ""
    '        End If
    '    End If

    '    If _units = 0 Then
    '        ' Here we need to check if there is container and update this loads
    '        EvacuateEmptyContainer(pUser)
    '    End If

    '    _lastcountdate = DateTime.Now
    '    _lastcounteruser = pUser
    '    _editdate = DateTime.Now
    '    _edituser = pUser
    '    Dim sql As String = String.Format("Update loads set LOCATION={0}, WAREHOUSEAREA={1}, UNITS={2}, STATUS={3}, PRELIMBOSTATUS={4}, PRELIMBOLOC={5}, LASTCOUNTDATE={6}, LASTCOUNTUSER={7}, EDITDATE={8}, EDITUSER={9} WHERE {10}", Made4Net.Shared.Util.FormatField(_location), _
    '        Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_prelimbostatus), Made4Net.Shared.Util.FormatField(_prelimboloc), Made4Net.Shared.Util.FormatField(_lastcountdate), _
    '        Made4Net.Shared.Util.FormatField(_lastcounteruser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
    '    DataInterface.RunSQL(sql)

    '    'Me.setStatus(_status, "", _edituser)

    '    If (toLoc.ToUpper <> OrgLoc.ToUpper) And (toWarehousearea.ToUpper <> OrgWarehousearea.ToUpper) Then locChanged = True

    '    Dim aq As EventManagerQ = New EventManagerQ
    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadCount)
    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ACTIVITYTIME", "0")
    '    aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.COUNTLOAD)
    '    aq.Add("CONSIGNEE", _consignee)
    '    aq.Add("DOCUMENT", _receipt)
    '    aq.Add("DOCUMENTLINE", _receiptline)
    '    aq.Add("FROMLOAD", _loadid)
    '    aq.Add("FROMLOC", OrgLoc)
    '    aq.Add("FROMWAREHOUSEAREA", OrgWarehousearea)
    '    aq.Add("FROMQTY", OrgUnits)
    '    aq.Add("FROMSTATUS", FromStat)
    '    aq.Add("NOTES", "")
    '    aq.Add("SKU", _sku)
    '    aq.Add("TOLOAD", ldid)
    '    aq.Add("TOLOC", _location)
    '    aq.Add("TOWAREHOUSEAREA", _warehousearea)
    '    aq.Add("TOQTY", toQty)
    '    aq.Add("TOSTATUS", _status)
    '    aq.Add("USERID", pUser)
    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("LASTMOVEUSER", pUser)
    '    aq.Add("LASTSTATUSUSER", pUser)
    '    aq.Add("LASTCOUNTUSER", pUser)
    '    aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ADDUSER", pUser)
    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("EDITUSER", pUser)
    '    aq.Send(WMS.Lib.Actions.Audit.COUNTLOAD)

    '    If _unitsallocated > 0 Then
    '        recalcPicks(locChanged)
    '    End If

    '    'update the warehouse activity table
    '    Dim oWHActivity As New WHActivity()
    '    oWHActivity.LOCATION = _location
    '    oWHActivity.USERID = pUser
    '    oWHActivity.ACTIVITY = WMS.Lib.Actions.Audit.COUNTLOAD
    '    oWHActivity.ACTIVITYTIME = DateTime.Now
    '    oWHActivity.EDITDATE = DateTime.Now
    '    oWHActivity.EDITUSER = pUser
    '    oWHActivity.Post()
    'End Sub

    Private Sub recalcPicks(ByVal LocChanged As Boolean)
        Dim dt As New DataTable
        Dim dtFullPicks As New DataTable
        Dim dr As DataRow
        Dim pcklist As Picklist
        Dim pckdet As PicklistDetail
        Dim sql As String

        'First - unallocate all full picks, no matter what
        sql = String.Format("select ph.picklist from PICKDETAIL pd inner join PICKHEADER ph on ph.PICKLIST = pd.PICKLIST and ph.PICKTYPE = '{0}' and ph.STATUS in ('{1}','{2}') and pd.FROMLOAD = '{3}'", _
                WMS.Lib.PICKTYPE.FULLPICK, WMS.Lib.Statuses.Picklist.PLANNED, WMS.Lib.Statuses.Picklist.RELEASED, _loadid)
        DataInterface.FillDataset(sql, dtFullPicks)
        For Each dr In dtFullPicks.Rows
            pcklist = New Picklist(dr("picklist"))
            pcklist.unAllocate(WMS.Lib.USERS.SYSTEMUSER)
        Next
        'Then unallocate all the rest
        sql = String.Format("Select picklist,picklistline from pickdetail where (status = '{0}' or status = '{1}' or status = '{2}') and fromload = '{3}' order by adddate", WMS.Lib.Statuses.Picklist.PLANNED, WMS.Lib.Statuses.Picklist.RELEASED, WMS.Lib.Statuses.Picklist.PARTPICKED, _loadid)
        DataInterface.FillDataset(sql, dt)
        If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Or LocChanged Then
            For Each dr In dt.Rows
                pcklist = New Picklist(dr("picklist"))
                pcklist.unAllocateLine(dr("picklistline"), WMS.Lib.USERS.SYSTEMUSER)
            Next
        ElseIf _units < _unitsallocated Then
            Dim RemainingAllocatedUnits As Decimal = _units
            For Each dr In dt.Rows
                pcklist = New Picklist(dr("picklist"))
                pckdet = pcklist(dr("picklistline"))
                If pckdet.AdjustedQuantity - pckdet.PickedQuantity > RemainingAllocatedUnits Then
                    pckdet.unAllocate(pckdet.AdjustedQuantity - pckdet.PickedQuantity - RemainingAllocatedUnits, WMS.Lib.USERS.SYSTEMUSER)
                End If
                RemainingAllocatedUnits -= pckdet.AdjustedQuantity - pckdet.PickedQuantity
            Next
        End If
    End Sub

    Private Sub recalcReplenishments(ByVal LocChanged As Boolean)
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim pcklist As Picklist
        Dim pckdet As PicklistDetail
        Dim sql As String = String.Format("Select picklist,picklistline from pickdetail where (status = '{0}' or status = '{1}') and fromload = '{2}' order by adddate", WMS.Lib.Statuses.Picklist.PLANNED, WMS.Lib.Statuses.Picklist.RELEASED, _loadid)
        DataInterface.FillDataset(sql, dt)
        If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Or LocChanged Then
            For Each dr In dt.Rows
                pcklist = New Picklist(dr("picklist"))
                If pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    If pcklist.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And pcklist.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                        pcklist.unAllocate(WMS.Lib.USERS.SYSTEMUSER)
                    End If
                Else
                    pcklist.unAllocateLine(dr("picklistline"), WMS.Lib.USERS.SYSTEMUSER)
                End If
            Next
        ElseIf _units < _unitsallocated Then
            For Each dr In dt.Rows
                pcklist = New Picklist(dr("picklist"))
                pckdet = pcklist(dr("picklistline"))
                If _unitsallocated - (pckdet.AdjustedQuantity - pckdet.PickedQuantity) > _units Then
                    _unitsallocated = _unitsallocated - (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                    If _unitsallocated < 0 Then
                        _unitsallocated = 0
                    End If
                    pckdet.unAllocate(WMS.Lib.USERS.SYSTEMUSER)
                Else
                    pckdet.unAllocate(_unitsallocated - _units, WMS.Lib.USERS.SYSTEMUSER)
                    _unitsallocated = _units
                    If _unitsallocated < 0 Then
                        _unitsallocated = 0
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub sendToLimbo(ByVal puser As String)
        If hasActivity() Then
            Return
        End If
        _prelimboloc = _location
        _prelimbowarehousearea = _warehousearea
        _prelimbostatus = _status
        _laststatusdate = DateTime.Now
        _lastcountdate = DateTime.Now
        _location = ""
        _warehousearea = ""
        _status = WMS.Lib.Statuses.LoadStatus.LIMBO
        _editdate = DateTime.Now
        _edituser = puser
        Dim sql As String = String.Format("Update LOADS set PRELIMBOLOC={0},PRELIMBOWAREHOUSEAREA={9},PRELIMBOSTATUS={1},LASTSTATUSDATE={2},LASTCOUNTDATE={3},LOCATION={4},WAREHOUSEAREA={10}, STATUS={5}, EDITDATE={6},EDITUSER={7} WHERE {8}", Made4Net.Shared.Util.FormatField(_prelimboloc, ""), _
            Made4Net.Shared.Util.FormatField(_prelimbostatus, ""), Made4Net.Shared.Util.FormatField(_laststatusdate), Made4Net.Shared.Util.FormatField(_lastcountdate), Made4Net.Shared.Util.FormatField(_location, ""), Made4Net.Shared.Util.FormatField(_status, ""), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_prelimbowarehousearea, ""), Made4Net.Shared.Util.FormatField(_warehousearea, ""))
        DataInterface.RunSQL(sql)

        'check the container - if there are other loads remove from container
        Try
            If _containerid <> "" Then
                Dim oCont As New WMS.Logic.Container(_containerid, True)
                If oCont.Loads.Count > 1 Then
                    RemoveFromContainer()
                End If
            End If
        Catch ex As Exception
        End Try

    End Sub

    Public Function ShouldVerifyCounting(ByVal pCountedUnits As Decimal, ByVal pCountedUom As String)
        Dim oSku As New SKU(_consignee, _sku)
        Try
            pCountedUnits = oSku.ConvertToUnits(pCountedUom) * pCountedUnits
        Catch ex As Exception
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid load UOM", "Invalid load UOM")
            Throw m4nEx
        End Try
        If Math.Abs(_units - pCountedUnits) > oSku.COUNTTOLERANCE Then
            Return True
        End If
        Return False
    End Function

#End Region

#Region "Verification"

    Public Sub Verify(ByVal pUser As String)
        If _activitystatus = WMS.Lib.Statuses.ActivityStatus.VERIFIED Then
            Throw New Made4Net.Shared.M4NException("Load is already verified")
        End If

        If Not isInventory() Then
            Throw New Made4Net.Shared.M4NException("Load does not exist.")
        End If

        If _activitystatus = WMS.Lib.Statuses.ActivityStatus.LOADED Then
            Throw New Made4Net.Shared.M4NException("Can not verify a loaded load.")
        End If

        Dim oldstat As String = Me.STATUS
        Dim newStat As String = WMS.Lib.Statuses.ActivityStatus.VERIFIED
        UpdateOrder(WMS.Lib.LoadActivityTypes.VERIFYING, GetPreviousActivityType(), pUser)
        SetActivityStatus(newStat, pUser)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadVerified)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOADVER)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", oldstat)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", newStat)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.LOADVER)
    End Sub

#End Region

#Region "Pickup, Putaway, Merge"

    Public Sub RequestPickUp(ByVal pUser As String, Optional ByVal CreateTask As Boolean = True, Optional ByVal OnContainer As Boolean = False)
        'Flow of the request pickup proc: First check if there is a task concerning that load - if yes ->
        'return the task with the load. If not -> try to use the load for opportunity replenishment jobs.
        'If the load is not used for repl as well, go to putaway for location assignment..

        If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Then
            Throw New M4NException(New Exception(), "Can not pickup load in status limbo", "Can not pickup load in status limbo")
        End If

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.RequestPickup)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.REQUESTPICKUP)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOLOC", _location)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.REQUESTPICKUP)

        Dim destLoc As String = String.Empty
        Dim destWarehousearea As String = String.Empty
        Dim strTask As String = String.Empty
        strTask = IsAssignedToTask()
        If strTask <> "" Then
            Return
        End If

        If _unitsallocated > 0 Then
            Throw New M4NException(New Exception(), "Can not pickup load - units allocated", "Can not pickup load - units allocated")
        End If

        'check if the current load can be used for Replenishemnt jobs
        SendLoadToReplenishment(destLoc, destWarehousearea, OnContainer, pUser)
        If destLoc <> "" Then
            Return
        End If

        'Else - send to putaway for location assignment
        Dim pw As New Putaway
        pw.RequestDestinationForLoad(_loadid, destLoc, destWarehousearea, 0, "", CreateTask, OnContainer) 'RWMS-1277
    End Sub

    Public Sub PutAway(ByRef destLocation As String, ByRef destWarehousearea As String, ByVal pUser As String, Optional ByVal CreateTask As Boolean = True)
        Dim destLoc As String = Nothing
        Dim strTask As String = IsAssignedToTask()
        If strTask <> "" Then
            Return
        End If

        If _status = WMS.Lib.Statuses.LoadStatus.LIMBO Then
            Throw New M4NException(New Exception(), "Cannot do putaway for load in status limbo", "Cannot do putaway for load in status limbo")
        End If

        If _unitsallocated > 0 Then
            Throw New M4NException(New Exception(), "Cannot pickup load - units allocated", "Cannot pickup load - units allocated")
        End If

        Dim pw As New Putaway
        pw.RequestDestinationForLoad(_loadid, destLocation, destWarehousearea, 0, "", CreateTask) ''RWMS-1277
        If Not destLocation = String.Empty Then
            _destinationlocation = destLoc
            _destinationwarehousearea = destWarehousearea
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.PUTAWAYPEND
        End If
    End Sub

    Public Function IsAssignedToTask() As String
        Dim strSql As String = "SELECT TASK,STATUS,isnull(userid,'') as USERID FROM TASKS WHERE STATUS IN ('ASSIGNED','AVAILABLE') AND FROMLOAD='" & _loadid & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("status") = WMS.Lib.Statuses.Task.ASSIGNED And Convert.ToString(dt.Rows(0)("userid")).ToLower <> GetCurrentUser.ToLower Then
                'Throw New Made4Net.Shared.M4NException(New Exception, "Load already assigned to a task for another user", "Load already assigned to a task for another user")
                Dim oTask As New Task(dt.Rows(0)("TASK"))
                oTask.AssignUser(GetCurrentUser)
                Return Convert.ToString(dt.Rows(0)("TASK"))
            ElseIf dt.Rows(0)("status") = WMS.Lib.Statuses.Task.AVAILABLE Then
                Dim oTask As New Task(dt.Rows(0)("TASK"))
                oTask.AssignUser(GetCurrentUser)
                Return Convert.ToString(dt.Rows(0)("TASK"))
            End If
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function

    Private Sub SendLoadToReplenishment(ByRef destLocation As String, ByRef destWarehousearea As String, ByVal OnContainer As Boolean, ByVal pUser As String)
        Dim oQ As New Made4Net.Shared.SyncQMsgSender
        Dim oMsg As System.Messaging.Message
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.OpportunityReplenishment
        oQ.Add("LOADID", _loadid)
        oQ.Add("EVENT", EventType)
        oQ.Add("ONCONTAINER", OnContainer)
        oQ.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        oQ.Add("USERID", pUser)
        oQ.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse())
        oMsg = oQ.Send("Replenishment")
        Dim qm As Made4Net.Shared.QMsgSender = Made4Net.Shared.QMsgSender.Deserialize(oMsg.BodyStream)
        'Return qm.Values("LOCATION")
        destLocation = qm.Values("LOCATION")
        destWarehousearea = qm.Values("WAREHOUSEAREA")
    End Sub

    'Public Sub Put(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pSubLocation As String, ByVal pUser As String, Optional ByVal isHandoff As Boolean = False, Optional ByVal pContDelivery As Boolean = False, Optional ByVal pIsStaging As Boolean = False)
    '    'update the target location pendings as well
    '    Dim FromLocation As String = _location
    '    Dim FromWarehousearea As String = _warehousearea
    '    Dim oLocation As New Location(pLocation, pWarehousearea)
    '    oLocation.Put(Me, pUser)

    '    'now update the load
    '    _editdate = DateTime.Now
    '    _edituser = pUser
    '    _location = pLocation
    '    _warehousearea = pWarehousearea
    '    _sublocation = pSubLocation
    '    _lastmovedate = DateTime.Now
    '    _lastmoveuser = pUser

    '    ' Check if the put of the load is from HandOff job (not final destination)
    '    ' and if it is than do not update DESTINATIONLOCATION and ACTIVITYSTATUS
    '    If Not isHandoff Then
    '        _destinationlocation = ""
    '        _destinationwarehousearea = ""
    '        If _activitystatus = WMS.Lib.Statuses.ActivityStatus.LOCASSIGNPEND Or _activitystatus = WMS.Lib.Statuses.ActivityStatus.PUTAWAYPEND Or _activitystatus = WMS.Lib.Statuses.ActivityStatus.REPLPENDING Then
    '            _activitystatus = Nothing
    '        End If
    '    End If

    '    _lastmovedate = DateTime.Now
    '    Dim sql As String = String.Format("update loads set LOCATION = {0}, WAREHOUSEAREA = {8}, SUBLOCATION = {1}, DESTINATIONLOCATION = {2}, DESTINATIONWAREHOUSEAREA = {9},ACTIVITYSTATUS = {3} ,LASTMOVEDATE = {4}, EDITDATE = {5}, EDITUSER = {6} where {7}", _
    '            Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_sublocation), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_lastmovedate), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_destinationwarehousearea))
    '    DataInterface.RunSQL(sql)

    '    'If Not pContDelivery OrElse Not pIsStaging Then
    '    '    Dim aq As EventManagerQ = New EventManagerQ
    '    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadPutaway)
    '    '    aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOADPUTAWAY)
    '    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    '    aq.Add("ACTIVITYTIME", "0")
    '    '    aq.Add("CONSIGNEE", _consignee)
    '    '    aq.Add("DOCUMENT", "")
    '    '    aq.Add("DOCUMENTLINE", 0)
    '    '    aq.Add("FROMLOAD", _loadid)
    '    '    aq.Add("FROMLOC", FromLocation)
    '    '    aq.Add("FROMQTY", _units)
    '    '    aq.Add("FROMSTATUS", _status)
    '    '    aq.Add("NOTES", "")
    '    '    aq.Add("SKU", _sku)
    '    '    aq.Add("TOLOAD", _loadid)
    '    '    aq.Add("TOLOC", pLocation)
    '    '    aq.Add("TOQTY", _units)
    '    '    aq.Add("TOSTATUS", _status)
    '    '    aq.Add("USERID", Common.GetCurrentUser())
    '    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    '    aq.Add("ADDUSER", Common.GetCurrentUser())
    '    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    '    aq.Add("EDITUSER", Common.GetCurrentUser())
    '    '    aq.Send(WMS.Lib.Actions.Audit.LOADPUTAWAY)
    '    'End If

    '    WMS.Logic.Merge.Put(Me, pLocation, pWarehousearea)
    'End Sub

    'Public Sub Replenish(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pUser As String)
    '    _unitsallocated = 0
    '    _editdate = DateTime.Now
    '    _edituser = pUser
    '    _destinationlocation = pLocation
    '    _location = pLocation
    '    _warehousearea = pWarehousearea
    '    _activitystatus = Nothing
    '    _lastmovedate = DateTime.Now

    '    If WMS.Logic.PickLoc.Exists(_consignee, _sku, pLocation, pWarehousearea) Then
    '        Dim oPickLoc As New PickLoc(pLocation, pWarehousearea, _consignee, _sku)
    '        oPickLoc.Put(Me, pUser)
    '    End If

    '    _destinationlocation = ""
    '    _destinationwarehousearea = ""

    '    'Dim sql As String = String.Format("update loads set LOCATION = {0}, DESTINATIONLOCATION = {1},ACTIVITYSTATUS = {2} ,LASTMOVEDATE = {3}, UNITSALLOCATED = {4}, EDITDATE = {5},EDITUSER = {6} where {7}", _
    '    '       Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_lastmovedate), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
    '    'Dim sql As String = String.Format("update loads set LOCATION = {0}, DESTINATIONLOCATION = {1}, DESTINATIONWAREHOUSEAREA = {2}, ACTIVITYSTATUS = {3} ,LASTMOVEDATE = {4}, UNITSALLOCATED = {5}, EDITDATE = {6},EDITUSER = {7} where {8}", _
    '    'Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_lastmovedate), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
    '    Dim sql As String = String.Format("update loads set LOCATION = {0}, DESTINATIONLOCATION = {1}, DESTINATIONWAREHOUSEAREA = {2},WAREHOUSEAREA = {3}, ACTIVITYSTATUS = {4} ,LASTMOVEDATE = {5}, UNITSALLOCATED = {6}, EDITDATE = {7},EDITUSER = {8} where {9}", _
    '           Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_lastmovedate), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

    '    DataInterface.RunSQL(sql)

    '    WMS.Logic.Merge.Put(Me, pLocation, pWarehousearea)

    '    'Dim aq As EventManagerQ = New EventManagerQ
    '    'aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadReplenish)
    '    'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    'aq.Add("ACTIVITYTIME", "0")
    '    'aq.Add("ACTIVITYTYPE", "LOADREPL")
    '    'aq.Add("CONSIGNEE", _consignee)
    '    'aq.Add("DOCUMENT", _receipt)
    '    'aq.Add("DOCUMENTLINE", _receiptline)
    '    'aq.Add("FROMLOAD", _loadid)
    '    'aq.Add("FROMLOC", _location)
    '    'aq.Add("FROMQTY", _units)
    '    'aq.Add("FROMSTATUS", _status) ' _units)
    '    'aq.Add("NOTES", "")
    '    'aq.Add("SKU", _sku)
    '    'aq.Add("TOLOAD", _loadid)
    '    'aq.Add("TOLOC", _location)
    '    'aq.Add("TOQTY", _units)
    '    'aq.Add("TOSTATUS", _status)
    '    'aq.Add("USERID", pUser)
    '    'aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    'aq.Add("LASTMOVEUSER", pUser)
    '    'aq.Add("LASTSTATUSUSER", pUser)
    '    'aq.Add("LASTCOUNTUSER", pUser)
    '    'aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    'aq.Add("REASONCODE", "")
    '    'aq.Add("ADDUSER", pUser)
    '    'aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    'aq.Add("EDITUSER", pUser)
    '    'aq.Send("LOADREPL")

    '    WMS.Logic.Merge.Put(Me, pLocation, pWarehousearea)
    'End Sub

    Public Sub CancelReplenish(ByVal pLocation As String, ByVal pWAREHOUSEAREA As String, ByVal CanceledUnits As Decimal, ByVal pUser As String)
        _unitsallocated = 0
        _editdate = DateTime.Now
        _edituser = pUser
        _activitystatus = Nothing
        _destinationlocation = ""
        _destinationwarehousearea = ""

        Dim sql As String = String.Format("update loads set DESTINATIONLOCATION = {0},DESTINATIONWAREHOUSEAREA = {6},ACTIVITYSTATUS = {1}, UNITSALLOCATED = {2}, EDITDATE = {3},EDITUSER = {4} where {5}", _
                Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause, Made4Net.Shared.Util.FormatField(_destinationwarehousearea))
        DataInterface.RunSQL(sql)
    End Sub

    Public Sub Merge(ByVal DestLoad As Load)
        _loaduom = DestLoad.LOADUOM
        If _loaduom <> DestLoad.LOADUOM Then
            Dim oSku As New SKU(_consignee, _sku)
            If oSku.isChildOf(DestLoad.LOADUOM, _loaduom) Then
                _loaduom = DestLoad.LOADUOM
            End If
        End If

        Try
            DestLoad.LoadAttributes("WEIGHT") = Me.WeightBeforeAdjusting + DestLoad.WeightBeforeAdjusting
        Catch ex As Exception
            DestLoad.LoadAttributes("WEIGHT") = Me.WeightBeforeAdjusting
        End Try



        Me.LoadAttributes.SetAttributes(DestLoad.LoadAttributes.Attributes, Me.LOADID)

        _receivedate = DestLoad.RECEIVEDATE
        _receipt = DestLoad.RECEIPT
        _units += DestLoad.UNITS
        _unitsallocated += DestLoad.UNITSALLOCATED
        _unitspicked += DestLoad.UNITSPICKED
        'In case of linbo adjust, if negative and positive gets even
        If _units = 0 Then
            _status = WMS.Lib.Statuses.LoadStatus.NONE
            _activitystatus = WMS.Lib.Statuses.ActivityStatus.NONE
            _location = ""
        ElseIf _units > 0 AndAlso _location = String.Empty AndAlso _status = String.Empty Then
            _status = DestLoad.STATUS
            _location = DestLoad.LOCATION
        End If
        Dim sql As String = String.Format("Update Loads set LOADUOM={0},UNITS={1},UNITSALLOCATED={2},UNITSPICKED={3},STATUS={4},ACTIVITYSTATUS={5},LOCATION={6},RECEIPT={7},RECEIVEDATE={8} WHERE {9}", Made4Net.Shared.Util.FormatField(_loaduom), _
             Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_unitspicked), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_activitystatus), _
             Made4Net.Shared.Util.FormatField(_location), Made4Net.Shared.Util.FormatField(_receipt), Made4Net.Shared.Util.FormatField(_receivedate), WhereClause)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadMerge)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOADMERGE)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", _receipt)
        aq.Add("DOCUMENTLINE", _receiptline)
        aq.Add("FROMLOAD", DestLoad.LOADID)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", DestLoad.UNITS)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", Common.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", Common.GetCurrentUser)
        aq.Add("LASTSTATUSUSER", Common.GetCurrentUser)
        aq.Add("LASTCOUNTUSER", Common.GetCurrentUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", Common.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", Common.GetCurrentUser)
        aq.Send(WMS.Lib.Actions.Audit.LOADMERGE)

        DataInterface.RunSQL(sql)
        DestLoad.UNITSALLOCATED = 0
        DestLoad.Delete("SYSTEM")
    End Sub

    Public Sub RemoveFromContainer()
        Dim sql As String
        'sql = String.Format("delete from containerloads where loadid = {0}", Made4Net.Shared.Util.FormatField(_loadid))
        sql = String.Format("update loads set HANDLINGUNIT = '' where LOADID = {0}", Made4Net.Shared.Util.FormatField(_loadid))
        DataInterface.RunSQL(sql)
        _containerid = Nothing
    End Sub

    'Public Sub Deliver(ByVal toLocation As String, ByVal toWarehousearea As String, ByVal RemoveLoadFromContainer As Boolean, ByVal UserId As String, Optional ByVal pShouldStage As Boolean = False, Optional ByVal isHandOffDelivery As Boolean = False, Optional ByVal pContDelivery As Boolean = False, Optional ByVal od As OutboundOrderHeader.OutboundOrderDetail = Nothing, Optional ByVal Header As OutboundOrderHeader = Nothing, Optional ByVal fd As FlowthroughDetail = Nothing, Optional ByVal fh As Flowthrough = Nothing)
    '    'If Me.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.STAGED OrElse _
    '    'Me.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.LOADED Then
    '    '    Throw New M4NException(New Exception(), "Can not stage load. Incorrect status.", "Can not stage load. Incorrect status.")
    '    'End If
    '    Me.Put(toLocation, toWarehousearea, _sublocation, UserId, isHandOffDelivery, pContDelivery, True)

    '    Dim aq As New EventManagerQ
    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LoadStaged)
    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ACTIVITYTIME", "0")
    '    aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.STAGELOAD)
    '    aq.Add("CONSIGNEE", _consignee)
    '    aq.Add("DOCUMENT", "")
    '    aq.Add("DOCUMENTLINE", 0)
    '    aq.Add("FROMLOAD", _loadid)
    '    aq.Add("FROMLOC", _location)
    '    aq.Add("FROMQTY", 0)
    '    aq.Add("FROMSTATUS", _status)
    '    aq.Add("FROMCONTAINER", _containerid)
    '    aq.Add("TOCONTAINER", _containerid)
    '    aq.Add("NOTES", "")
    '    aq.Add("SKU", _sku)
    '    aq.Add("TOLOAD", _loadid)
    '    aq.Add("TOLOC", _location)
    '    aq.Add("TOQTY", _units)
    '    aq.Add("TOSTATUS", _status)
    '    aq.Add("USERID", UserId)
    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("LASTMOVEUSER", UserId)
    '    aq.Add("LASTSTATUSUSER", UserId)
    '    aq.Add("LASTCOUNTUSER", UserId)
    '    aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ADDUSER", UserId)
    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("EDITUSER", UserId)
    '    aq.Send(WMS.Lib.Actions.Audit.STAGELOAD)

    '    If RemoveLoadFromContainer Then
    '        RemoveFromContainer()
    '    End If
    '    'Dim sOrderDestination As String = GetLoadFinalDestination()
    '    Dim sOrderDestination As String
    '    Dim sOrderDestinationWarehouseArea As String
    '    GetLoadFinalDestination(sOrderDestination, sOrderDestinationWarehouseArea)
    '    If toLocation.Equals(sOrderDestination, StringComparison.OrdinalIgnoreCase) Then
    '        'after delivering the load we have to update the order detail staged qty and order header status
    '        'if not is hand off delivery
    '        Dim OldActivityType As String = GetPreviousActivityType()
    '        If _activitystatus.Equals(WMS.Lib.Statuses.ActivityStatus.STAGED, StringComparison.OrdinalIgnoreCase) OrElse _
    '            _activitystatus.Equals(WMS.Lib.Statuses.ActivityStatus.LOADED, StringComparison.OrdinalIgnoreCase) Then
    '            Return
    '        End If
    '        Me.SetActivityStatus(WMS.Lib.Statuses.ActivityStatus.STAGED, UserId)
    '        UpdateOrder(WMS.Lib.LoadActivityTypes.STAGING, OldActivityType, UserId, od, Header, fd, fh)
    '    End If
    'End Sub

    Public Sub GetLoadFinalDestination(ByRef pLocation As String, ByRef pWarehouseArea As String)
        'Dim sql As String = String.Format("select (case DOCUMENTTYPE when 'FLWTH' then isnull(ft.staginglane,'') else isnull(oh.staginglane,'') end) Location, (case DOCUMENTTYPE when 'FLWTH' then isnull(ft.STAGINGWAREHOUSEAREA,'') else isnull(oh.STAGINGWAREHOUSEAREA,'') end) WareHouseArea from orderloads ol left outer join outboundorheader oh on oh.consignee = ol.consignee and oh.orderid = ol.orderid left outer join FLOWTHROUGHHEADER ft on ft.CONSIGNEE = ol.consignee and ft.FLOWTHROUGH = ol.orderid where loadid = '{0}'", _loadid)
        Dim sql As String = String.Format("select isnull(sh.staginglane,'') as ShipmentStagingLane, isnull(sh.stagingwarehousearea,'') as ShipmentStagingWarehouseArea, (case ol.DOCUMENTTYPE when 'FLWTH' then isnull(ft.staginglane,'') else isnull(oh.staginglane,'') end) Location, (case ol.DOCUMENTTYPE when 'FLWTH' then isnull(ft.STAGINGWAREHOUSEAREA,'') else isnull(oh.STAGINGWAREHOUSEAREA,'') end) WareHouseArea from orderloads ol left outer join outboundorheader oh on oh.consignee = ol.consignee and oh.orderid = ol.orderid left outer join FLOWTHROUGHHEADER ft on ft.CONSIGNEE = ol.consignee and ft.FLOWTHROUGH = ol.orderid left outer join shipmentdetail sd on sd.consignee= ol.consignee and sd.orderid= ol.orderid and sd.orderline = ol.orderline left outer join SHIPMENT sh on sd.SHIPMENT = sh.SHIPMENT where loadid = '{0}'", _loadid)

        Dim dt As New DataTable()
        DataInterface.FillDataset(sql, dt)

        If Not String.IsNullOrEmpty(dt.Rows(0)("ShipmentStagingLane")) Then
            pLocation = dt.Rows(0)("ShipmentStagingLane")
            pWarehouseArea = dt.Rows(0)("ShipmentStagingWarehouseArea")
        Else
            pLocation = dt.Rows(0)("Location")
            pWarehouseArea = dt.Rows(0)("WareHouseArea")
        End If

        'pLocation = dt.Rows(0)("Location")
        'pWarehouseArea = dt.Rows(0)("WareHouseArea")
        'Return DataInterface.ExecuteScalar(sql)
    End Sub

#End Region

#Region "Container"

    Public Sub PutOnContainer(ByVal sContainerId As String, ByVal pLocation As String, ByVal pWarehouseArea As String, ByVal pUser As String)
        'DataInterface.RunSQL(String.Format("delete from containerloads where loadid = '{0}'", _loadid))
        _lastmovedate = DateTime.Now
        _containerid = sContainerId
        _editdate = DateTime.Now
        _edituser = pUser
        If pLocation <> "" And Not pLocation Is Nothing Then
            _location = pLocation
        End If
        If pWarehouseArea <> "" And Not pWarehouseArea Is Nothing Then
            _warehousearea = pWarehouseArea
        End If
        Dim sql As String = String.Format("update loads set lastmovedate={0}, Location={1}, Warehousearea={2}, handlingunit={6}, EditUser={3}, EditDate={4} Where {5}", _
           Made4Net.Shared.Util.FormatField(_lastmovedate), Made4Net.Shared.Util.FormatField(_location), _
           Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_edituser), _
           Made4Net.Shared.Util.FormatField(_editdate), WhereClause, Made4Net.Shared.Util.FormatField(_containerid))

        DataInterface.RunSQL(sql)
    End Sub

#End Region

#Region "Work Orders"

    Public Sub ConsumeForWorkOrder(ByVal pWorkOrderId As String, ByVal pWOQty As Decimal, ByVal pUser As String)
        'Substruct thq needed qty from the original load
        _editdate = DateTime.Now
        _edituser = pUser
        If _units - pWOQty < 0 Then
            Throw New M4NException(New Exception, "Quantity cannot become negative", "Quantity cannot become negative")
        End If

        _units = _units - pWOQty
        _activitystatus = Nothing
        _destinationlocation = Nothing
        _destinationwarehousearea = Nothing
        _unitsallocated = 0
        Dim sStatus As String = _status
        If _units = 0 Then
            _location = ""
            _warehousearea = ""
            _status = WMS.Lib.Statuses.LoadStatus.NONE
        End If
        DataInterface.RunSQL(String.Format("Update loads set units = {0},activitystatus={1},unitsallocated={2}, destinationlocation={3}," & _
                "destinationwarehousearea={4},location={5},warehousearea={6},status={7}, editdate = {8},edituser = {9} where {10}", _
                Made4Net.Shared.Util.FormatField(_units), Made4Net.Shared.Util.FormatField(_activitystatus), _
                Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_destinationlocation), _
                Made4Net.Shared.Util.FormatField(_destinationwarehousearea), Made4Net.Shared.Util.FormatField(_location), _
                Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_status), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause))

        Me.AdjustLoadWeightAttribute(_units + pWOQty, _units, Me.WeightBeforeAdjusting, pUser)


        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WorkOrderLoadConsumed)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.INVENTORY.WOLDCNSM)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", pWorkOrderId)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", _units + pWOQty)
        aq.Add("FROMSTATUS", sStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", _loadid)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", _units)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Add("UNITPRICE", 0)
        aq.Add("UOM", _loaduom)
        aq.Add(_loadattributes.Attributes.ToNameValueCollection())
        aq.Send(WMS.Lib.INVENTORY.WOLDCNSM)

        Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.WOLDCNSM)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", pWorkOrderId)
        it.Add("LINE", _receiptline)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        it.Add("QTY", pWOQty)
        it.Add("CUBE", 0)
        it.Add("WEIGHT", 0)
        it.Add("AMOUNT", "")
        it.Add("SKU", _sku)
        it.Add("STATUS", sStatus)
        it.Add("REASONCODE", "")
        it.Add("UNITPRICE", 0)
        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("LASTMOVEUSER", pUser)
        it.Add("LASTSTATUSUSER", pUser)
        it.Add("LASTCOUNTUSER", pUser)
        it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("ADDUSER", pUser)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", pUser)
        it.Add(_loadattributes.Attributes.ToNameValueCollection())
        it.Send(WMS.Lib.INVENTORY.WOLDCNSM)
    End Sub

    Public Sub ProduceForWorkOrder(ByVal pConsignee As String, ByVal pWorkOrderId As String, ByVal pSku As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pQty As Decimal, ByVal pLoadId As String, ByVal pUOM As String, ByVal pStatus As String, ByVal pLoadAttributes As AttributesCollection, ByVal pUser As String)
        If pLoadId = "" Then
            pLoadId = GenerateLoadId()
        End If
        CreateLoad(pLoadId, pConsignee, pSku, pUOM, pLocation, pWarehousearea, pStatus, "", pQty, "", 0, "", pLoadAttributes, pUser)
        'Raise event to audit and invtrans
        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.WorkOrderLoadProduced)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("ACTIVITYTYPE", WMS.Lib.INVENTORY.WOLDPRDC)
        aq.Add("CONSIGNEE", _consignee)
        aq.Add("DOCUMENT", pWorkOrderId)
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", _loadid)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMLOC", _location)
        aq.Add("FROMQTY", _units)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", _sku)
        aq.Add("TOLOAD", pLoadId)
        aq.Add("TOLOC", _location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", pQty)
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("LASTMOVEUSER", pUser)
        aq.Add("LASTSTATUSUSER", pUser)
        aq.Add("LASTCOUNTUSER", pUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Add("UNITPRICE", 0)
        aq.Add("UOM", _loaduom)
        aq.Add(_loadattributes.Attributes.ToNameValueCollection())
        aq.Send(WMS.Lib.INVENTORY.WOLDPRDC)

        Dim it As InventoryTransactionQ = New InventoryTransactionQ()
        it.Add("TRANDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        it.Add("INVTRNTYPE", WMS.Lib.INVENTORY.WOLDPRDC)
        it.Add("CONSIGNEE", _consignee)
        it.Add("DOCUMENT", pWorkOrderId)
        it.Add("LINE", 0)
        it.Add("LOADID", _loadid)
        it.Add("UOM", _loaduom)
        it.Add("QTY", pQty)
        it.Add("CUBE", 0)
        it.Add("WEIGHT", 0)
        it.Add("AMOUNT", "")
        it.Add("SKU", _sku)
        it.Add("STATUS", _status)
        it.Add("REASONCODE", "")
        it.Add("UNITPRICE", 0)
        it.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        it.Add("LASTMOVEUSER", pUser)
        it.Add("LASTSTATUSUSER", pUser)
        it.Add("LASTCOUNTUSER", pUser)
        it.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))

        it.Add("ADDUSER", pUser)
        it.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        it.Add("EDITUSER", pUser)
        it.Add(_loadattributes.Attributes.ToNameValueCollection())
        it.Send(WMS.Lib.INVENTORY.WOLDPRDC)
    End Sub

#End Region

#End Region

End Class

#End Region

#Region "Load Collection"

<CLSCompliant(False)> Public Class LoadsCollection
    Inherits ArrayList

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As WMS.Logic.Load
        Get
            Return CType(MyBase.Item(index), WMS.Logic.Load)
        End Get
    End Property

    Public ReadOnly Property Load(ByVal pConsignee As String, ByVal pSku As String) As WMS.Logic.Load
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If Item(i).SKU = pSku And Item(i).CONSIGNEE = pConsignee Then
                    Return (CType(MyBase.Item(i), WMS.Logic.Load))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Methods"

    Public Shadows Function Add(ByVal pObject As WMS.Logic.Load) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Remove(ByVal pObject As WMS.Logic.Load)
        MyBase.Remove(pObject)
    End Sub

#End Region

End Class

#End Region