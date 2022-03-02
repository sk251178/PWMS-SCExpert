Imports System.Text
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Evaluation
Imports Made4Net.Algorithms
Imports Made4Net.Algorithms.ShortestPath
Imports Made4Net.Algorithms.Interfaces
Imports DataManager.ServiceModel
Imports WMS.Lib
Imports Made4Net.Shared.Conversion.Convert
Imports System.Collections.Generic
Imports System.Linq
Imports Made4Net.Shared

<CLSCompliant(False)> _
Public Class TaskStdCalculation

#Region "Constant"

    Protected Const MaxCnt As Integer = 100 ' max iteration
    Protected Separators As Char() = New [Char]() {"["c, "]"c}  ' equation separators
    '' RWMS-1938 -  Retrofit of RWMS-1919
    ' RWMS-1991 -  Retrofit of RWMS-1985 the value of undefined distance cannot be 0, since 0 is a genuine value of a distance when the [to] and [from] are same, as such changed back to -1.
    ' While printing the logs and writing into the laborperformanceaudit table, the undefined distance will be changed to 0.
    Protected Const UndefinedDistance As Integer = -1


#End Region

#Region "Variables"
    Protected _taskid As String
    Protected _tasktype As String
    Protected _tasksubtype As String
    Protected _prevTaskType As String

    ' Labor calc changes RWMS-952 -> PWMS-903
    Protected _taskStartLocation As String
    ' Labor calc changes RWMS-952 -> PWMS-903
    'RWMS-2742 START
    Protected _taskWHActivityLocation As String
    'RWMS-2742 END

    Protected _calcid As String
    Protected _sourceequation As String
    Protected _targetequation As String

    Protected _fromlocation As String = String.Empty
    Protected _tolocation As String = String.Empty
    Protected _nextpicklistdetaillocation As String
    Protected _nextpicklistdetailWHarea As String

    Protected _lastpicklistdetaillocation As String

    Protected _fromwarehousearea As String = String.Empty
    Protected _towarehousearea As String = String.Empty
    Protected _startwarehousearea As String = String.Empty
    Protected _assigmentlocation As String = String.Empty

    Protected _taskreplenishment As String = String.Empty

    Protected _fromload As String = String.Empty
    Protected _toload As String = String.Empty

    Protected _fromcontainer As String = String.Empty
    Protected _tocontainer As String = String.Empty
    Protected _container As Container

    Protected _sku As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _mheid As String = String.Empty
    Protected _taskuser As String = String.Empty
    Protected _taskterminaltype As String = String.Empty
    Protected _shift As String = String.Empty


    Protected _uom As String = String.Empty
    Protected _uomquaqntity As Decimal = 0
    Protected _totaluomquaqntity As Decimal = 0
    Protected _qty As Decimal = 0

    Protected _taskcube As Double
    Protected _taskweight As Double
    Protected _tasktravelweight As Double
    Protected _tasktravelcube As Double
    Protected _taskhusequencelocation As Integer

    Protected _tasktotalsku As Integer
    Protected _taskcurrenthu As String = String.Empty
    Protected _tasktotalhus As Integer

    Protected _taskactualtime As Double
    Protected _taskstartdate As DateTime
    Protected _taskenddate As DateTime

    Protected _picklistid As String = String.Empty
    Protected _picklist As Picklist
    Protected _picklistdetail As PicklistDetail


    Protected _cntload As Load

    Protected _parallelpicklist As String = String.Empty
    Protected _drparpicklistdetail As DataRow

    Protected _mhetype As String = String.Empty

    Protected _qp As WHQueryPath
    Protected _sp As IShortestPathProvider

    Protected _generictime As Double
    Protected _newassigment As Integer

    Protected _stdTime As Double

    Protected _traveldistance As Double = 0
    Protected _traveldistanceWithAisleWalk As Double = 0
    Protected _assigndistance As Double = 0
    Protected Shared _logger As ILogHandler
    Protected Shared _LogArray As List(Of String) = New List(Of String)
    Protected _executionLocation As String = String.Empty

    Dim _validpicklists As List(Of PicklistDetail)

#End Region

#Region "Properties"
    Public ReadOnly Property TASKID() As String
        Get
            Return _taskid
        End Get
    End Property
    Public ReadOnly Property CALCID() As String
        Get
            Return _calcid
        End Get
    End Property
    Public ReadOnly Property TASKTYPE() As String
        Get
            Return _tasktype
        End Get
    End Property
    Public ReadOnly Property TASKSUBTYPE() As String
        Get
            Return _tasksubtype
        End Get
    End Property

    Public ReadOnly Property SourceEquation() As String
        Get
            Return _sourceequation
        End Get
    End Property
    Public ReadOnly Property TargetEquation() As String
        Get
            Return _targetequation
        End Get
    End Property

    Public ReadOnly Property STDTIME() As Double
        Get
            Return _stdTime
        End Get
    End Property

    Public ReadOnly Property SHIFT() As String
        Get
            Return _shift
        End Get
    End Property
    '' RWMS-1876 - Retrofit of RWMS-1843: Case insenitive searhc is required for system automatically translating the Calculation IDs to lower case.
    '' RWMS-1938 -  Retrofit of RWMS-1919
    '' RWMS-1991 -  Retrofit of RWMS-1985
    Public LaborCalcMethods As Hashtable = System.Collections.Specialized.CollectionsUtil.CreateCaseInsensitiveHashtable()
    '' RWMS-1876 - Retrofit of RWMS-1843
    Public LaborCalcParameters As Hashtable = New Hashtable()
#End Region

#Region "Ctor"

    Public Sub New(ByVal pTaskID As String, _
                   ByVal pTaskUser As String, _
                   Optional ByVal oLogger As ILogHandler = Nothing)

        _logger = oLogger
        _taskid = pTaskID

        addLaborCalcParameters("TaskID", _taskid)

        _taskuser = pTaskUser

        Dim sql As String = String.Format("select top 1 TASKTYPE, TASKSUBTYPE, FROMLOCATION,TOLOCATION,FROMWAREHOUSEAREA,TOWAREHOUSEAREA, STARTLOCATION, FROMCONTAINER, TOCONTAINER, EXECUTIONLOCATION from TASKS where TASK='{0}'", _taskid)

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        Dim dr As DataRow = dt.Rows(0)
        _tasktype = ReplaceDBNull(dr.Item("TASKTYPE"))
        _tasksubtype = ReplaceDBNull(dr.Item("TASKSUBTYPE"))
        _fromlocation = ReplaceDBNull(dr.Item("FROMLOCATION"))
        _tolocation = ReplaceDBNull(dr.Item("TOLOCATION"))
        _fromwarehousearea = ReplaceDBNull(dr.Item("FROMWAREHOUSEAREA"))
        _towarehousearea = ReplaceDBNull(dr.Item("TOWAREHOUSEAREA"))
        _fromcontainer = ReplaceDBNull(dr.Item("FROMCONTAINER"))
        _tocontainer = ReplaceDBNull(dr.Item("TOCONTAINER"))
        _executionLocation = ReplaceDBNull(dr.Item("EXECUTIONLOCATION"))
        _taskStartLocation = ReplaceDBNull(dr.Item("STARTLOCATION"))
        _shift = Common.GetUserShift(_taskuser, oLogger)
        addLaborCalcParameters("TaskUser", _taskuser)
        addLaborCalcParameters("TaskType", _tasktype)

        getTaskCalculationID()

        If _calcid <> String.Empty Then
            addLaborCalcParameters("TaskTypeGenericTime", _generictime)
            LoadTableMethods()
            LoadTableParameters()
            LoadWHactivityParams()
        End If

        _sp = ShortestPath.GetInstance()
    End Sub



    Public Sub New(ByVal pCalcID As String)
        _taskid = String.Empty
        _tasktype = String.Empty
        _calcid = pCalcID
        If _calcid <> String.Empty Then
            LoadTableMethods()
            LoadTableParameters()
        End If
    End Sub




#End Region

#Region "Private Methods"

    Private Sub LoadTaskTypeParameters()
        LoadObjectsParameters("vLaborTask", String.Format("TASK='{0}'", _taskid))

        ''location
        If _fromlocation <> String.Empty Then
            LoadObjectsParameters("vLaborFromLocation", String.Format("FromLocation='{0}' and FromLocationWarehousearea ='{1}'  ", _
               _fromlocation, _fromwarehousearea))
        End If
        If _tolocation <> String.Empty Then
            LoadObjectsParameters("vLaborToLocation", String.Format("ToLocation='{0}' and ToLocationWarehousearea ='{1}'  ", _
               _tolocation, _towarehousearea))
        End If

        ''sku
        If _sku <> String.Empty And _consignee <> String.Empty Then
            LoadObjectsParameters("vLaborSKU", String.Format("SKU='{0}' and CONSIGNEE ='{1}'  ", _
               _sku, _consignee))


            If _uom <> String.Empty Then
                LoadObjectsParameters("vLaborSKUUOM", String.Format("SKU='{0}' and CONSIGNEE ='{1}'  and UOM ='{2}' ", _
                   _sku, _consignee, _uom))

            End If
        End If

        ''MHE
        If _mheid <> String.Empty Then
            LoadObjectsParameters("vLaborMHE", String.Format("MHEID='{0}' ", _
               _mheid))
        End If
        If (Not _picklistdetail Is Nothing) Then
            If _picklistdetail.PickList <> String.Empty Then
                LoadObjectsParameters("vLaborpickheaderview", String.Format("CONSIGNEE='{0}' and PICKLIST ='{1}'  and PICKLISTLINE ={2} ", _
                    _consignee, _picklistdetail.PickList, _picklistdetail.PickListLine))

            End If
        End If

    End Sub

    Private Sub LoadTaskTypeStartParameters(ByVal startLocation As String, ByVal pickingStartLoc As String, ByVal startWarehouseArea As String, ByVal pickingWareHouseArea As String)
        Dim pDataSourceName As String = "vLaborTask"
        Dim pWCL As String = String.Format("TASK='{0}'", _taskid)

        Dim sql As String = "SELECT * FROM " & pDataSourceName
        If pWCL <> String.Empty Then
            sql += " where " & pWCL
        End If

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        Dim dr As DataRow
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)

            For Each column As DataColumn In dt.Columns
                Select Case column.ColumnName.ToLower()
                    Case "taskfromlocation", "fromlocation"
                        addLaborCalcParameters(column.ColumnName, dr(column.ColumnName).ToString(), True, True)
                    Case Else
                        addLaborCalcParameters(column.ColumnName, dr(column.ColumnName).ToString())
                End Select

            Next
        End If

        ''location
        If startLocation <> String.Empty Then
            LoadObjectsParameters("vLaborFromLocation", String.Format("FromLocation='{0}' and FromLocationWarehousearea ='{1}'  ", _
               startLocation, startWarehouseArea))
        End If
        If pickingStartLoc <> String.Empty Then
            LoadObjectsParameters("vLaborToLocation", String.Format("ToLocation='{0}' and ToLocationWarehousearea ='{1}'  ", _
               pickingStartLoc, pickingWareHouseArea))
        End If
    End Sub

    Private Sub LoadWHactivityParams()
        Dim WHActivity As New WHActivity(_taskuser)

        _mheid = WHActivity.HANDLINGEQUIPMENTID
        _mhetype = WHActivity.HETYPE
        _taskterminaltype = WHActivity.TERMINALTYPE
        _prevTaskType = WHActivity.PreviousActivity
        _taskWHActivityLocation = WHActivity.LOCATION

        If String.IsNullOrEmpty(_taskStartLocation) Then
            _taskStartLocation = _taskWHActivityLocation
        End If

        addLaborCalcParameters("TaskMHE", _mheid)
        addLaborCalcParameters("TaskMHEType", _mhetype)
        addLaborCalcParameters("TaskTerminalType", _taskterminaltype)
        addLaborCalcParameters("UserShift", _shift)
        addLaborCalcParameters("PrevTaskType", _prevTaskType)
        addLaborCalcParameters("TaskStartLocation", _taskStartLocation)
    End Sub

    Public Sub LoadWHactivityParamsOnAssign()
        Dim WHActivity As New WHActivity(_taskuser)

        ' Labor calc changes RWMS-952
        _prevTaskType = WHActivity.ACTIVITY
        _taskStartLocation = WHActivity.LOCATION
        'RWMS-2742 START
        _taskWHActivityLocation = WHActivity.LOCATION
        'RWMS-2742 END
        ' Labor calc changes RWMS-952

        addLaborCalcParameters("PrevTaskType", _prevTaskType)
        addLaborCalcParameters("TaskStartLocation", _taskStartLocation)
    End Sub


    Public Sub UpdateWHactivityParamsOnComplete()
        Dim WHActivity As New WHActivity(_taskuser)

        'Update WHActivity Location while picking but it should not be system short pick
        If _validpicklists IsNot Nothing AndAlso _validpicklists.Any() Then
            Dim lastLocation As String = _validpicklists.Last().FromLocation
            WHActivity.UpdateLocation(lastLocation)
        End If

        'Save last location for all tasks except while system short pick
        If _validpicklists Is Nothing OrElse _validpicklists.Any() Then
            WHActivity.SaveLastLocation()
        Else
            'In case of system short pick, keep previous activity details
            WHActivity.RestorePrevActivityDetails()
        End If
    End Sub



    Public Sub UpdateAudit(ByVal taskEndDate As String, ByVal actualTime As String)
        Dim sql As String = String.Format("Update LABORPERFORMANCEAUDIT set ENDDATE = '{0}', ACTUALTIME = '{1}' where taskid = '{2}'", taskEndDate, actualTime, _taskid)
        _logger.SafeWrite(String.Format("Updating LABORPERFORMANCEAUDIT for Task {0} with ENDDATE {1}, SQL Query : {2}", _taskid, taskEndDate, sql))
        DataInterface.RunSQL(sql)
    End Sub

    Private Function getParameterValuebyID(ByVal pParameterID As String) As String
        Dim dr As DataRow
        dr = LaborCalcParameters(pParameterID)
        Return dr("PARAMETERVALUE")
    End Function

    Private Function getMethodEquationbyID(ByVal pCalcID As String) As String
        If LaborCalcMethods.Contains(_calcid) Then
            Return LaborCalcMethods(_calcid)
        Else
            Return String.Empty
        End If
    End Function

    Private Function LoadTableMethods() As Integer
        Dim sql As String = "SELECT * FROM LABORCALCULATIONMETHODS"
        Dim dt As New DataTable
        Dim dr As DataRow

        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            If Not LaborCalcMethods.Contains(dr("CALCULATIONID").ToString()) Then
                LaborCalcMethods.Add(dr("CALCULATIONID").ToString(), dr("CALCULATIONEQUATION").ToString())
            End If
        Next
        If LaborCalcMethods.Contains(_calcid.ToString()) Then
            _sourceequation = LaborCalcMethods(_calcid.ToString())
        Else
            _sourceequation = String.Empty
        End If
        Return LaborCalcMethods.Count()

    End Function

    Private Function LoadTableParameters() As Integer
        Dim sql As String = "SELECT * FROM LABORCALCULATIONPARAMETERS "
        Dim dt As New DataTable
        Dim dr As DataRow

        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            addLaborCalcParameters(dr("PARAMETERID").ToString(), dr("PARAMETERVALUE").ToString())
        Next
        Return LaborCalcParameters.Count()
    End Function

    Private Sub addLaborCalcParameters(ByVal key As String, ByVal value As String, _
            Optional ByVal islog As Boolean = True, _
            Optional ByVal isalwayslog As Boolean = False)
        Dim isnewValue As Boolean = True
        If String.Equals(_nextpicklistdetaillocation, _fromlocation, StringComparison.CurrentCultureIgnoreCase) Then
            Select Case key.ToLower()
                Case "taskfromlocwalkdistance", "taskfromlocaisledistance", _
                    "tasktolocwalkdistance", "tasktolocaisledistance", "tasktotaldistance"
                    value = 0
            End Select

        End If

        'RWMS-1991 -  Retrofit of RWMS-1985
        ' RWMS-1985/RWMS-1919, the value of undefined distance cannot be 0, since 0 is a genuine value of a distance when the [to] and [from] are same, as such changed back to -1.
        ' While printing the logs and writing into the laborperformanceaudit table, the undefined distance will be changed to 0.
        If key.Contains("Distance") Then
            Dim distanceValue As Double = 0
            If Double.TryParse(value, distanceValue) Then
                If distanceValue = "-1" Then
                    value = "0"
                End If
            End If
        End If

        If Not LaborCalcParameters.Contains(key) Then
            LaborCalcParameters.Add(key, value)
        Else
            If LaborCalcParameters(key) = value Then
                isnewValue = False
            Else
                LaborCalcParameters(key) = value
            End If
        End If

        If (islog AndAlso Not _logger Is Nothing And value <> String.Empty) Then
            If isalwayslog Or isnewValue Then
                Dim doesExists As Boolean = _LogArray.Any(Function(c) c.StartsWith(key & ":"))
                If doesExists Then
                    ' Overwrite Existing Entry
                    _LogArray.RemoveAll(Function(c) c.Contains(key))
                    _LogArray.Add(AddSpaces(35, key & ":", "b") & AddSpaces(10, value.Substring(0, Math.Min(100, value.Length)), "a"))
                    If value.Length > 100 Then
                        _LogArray.Add(AddSpaces(35, key & ":", "b") & AddSpaces(10, value.Substring(100, value.Length - 100), "a"))
                    End If
                Else
                    _LogArray.Add(AddSpaces(35, key & ":", "b") & AddSpaces(10, value.Substring(0, Math.Min(100, value.Length)), "a"))
                    If value.Length > 100 Then
                        _LogArray.Add(AddSpaces(35, key & ":", "b") & AddSpaces(10, value.Substring(100, value.Length - 100), "a"))
                    End If
                End If
            End If
        End If

    End Sub
    Private Function AddSpaces(ByVal LN As Integer, ByVal value As String, ByVal p As String) As String
        If LN > value.Length Then
            If p = "a" Then
                value = Space(LN - value.Length) & value.Trim
            Else
                value = value.Trim & Space(LN - value.Length)
            End If
        End If
        Return value
    End Function

    Private Function LoadObjectsParameters(ByVal pDataSourceName As String, _
                                    ByVal pWCL As String) As Integer
        Dim sql As String = "SELECT * FROM " & pDataSourceName
        If pWCL <> String.Empty Then
            sql += " where " & pWCL
        End If

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        Dim dr As DataRow
        If dt.Rows.Count = 0 Then Return 0




        dr = dt.Rows(0)

        For Each column As DataColumn In dt.Columns
            Select Case column.ColumnName.ToLower()
                Case "taskfromlocation", "fromlocation"
                    addLaborCalcParameters(column.ColumnName, dr(column.ColumnName).ToString(), True, True)
                Case Else
                    addLaborCalcParameters(column.ColumnName, dr(column.ColumnName).ToString())
            End Select

        Next

        If pDataSourceName = "vLaborTask" Then
            If Not dr.IsNull("TaskAssigmentLocation") Then _assigmentlocation = dr.Item("TaskAssigmentLocation")

            _taskactualtime = ReplaceDBNull(dr.Item("TaskActualTime"), 0)
            _taskstartdate = ReplaceDBNull(dr.Item("TaskStartDate"))
            _taskenddate = ReplaceDBNull(dr.Item("TaskEndDate"))

            Select Case _tasktype
                Case WMS.Lib.TASKTYPE.NEGTREPL, WMS.Lib.TASKTYPE.REPLENISHMENT, WMS.Lib.TASKTYPE.PARTREPL, WMS.Lib.TASKTYPE.FULLREPL

                    If Not dr.IsNull("TaskReplenishment") Then _taskreplenishment = dr.Item("TaskReplenishment")

                    If Not dr.IsNull("TaskSKU") Then _sku = dr.Item("TaskSKU")
                    If Not dr.IsNull("TaskConsignee") Then _consignee = dr.Item("TaskConsignee")

                    Dim oReplenishment As New Replenishment(_taskreplenishment)
                    If Not oReplenishment Is Nothing Then
                        _fromwarehousearea = oReplenishment.FromWarehousearea
                        _towarehousearea = oReplenishment.ToWarehousearea

                        _fromlocation = oReplenishment.FromLocation
                        _tolocation = oReplenishment.ToLocation

                        _fromload = oReplenishment.FromLoad
                        _toload = oReplenishment.ToLoad

                        Dim oLoad As Load = New Load(_fromload)
                        Dim oSKU As SKU = New SKU(_consignee, _sku)


                        _uom = oReplenishment.UOM
                        _uomquaqntity = oSKU.ConvertUnitsToUom(_uom, oReplenishment.Units)
                        _totaluomquaqntity += _uomquaqntity

                        _taskweight = oLoad.CalculateWeight().ToString()
                        _taskcube = oLoad.CalculateVolume().ToString()
                        _tasktravelweight = 0D
                        _tasktravelcube = 0D
                        _taskhusequencelocation = 1

                        _tasktotalsku = 1
                        _taskcurrenthu = "1"
                        _tasktotalhus = 1


                    End If

                Case WMS.Lib.TASKTYPE.LOADPUTAWAY, WMS.Lib.TASKTYPE.LOADDELIVERY
                    If Not dr.IsNull("TaskFromWhArea") Then _fromwarehousearea = dr.Item("TaskFromWhArea")
                    If Not dr.IsNull("TaskTowhArea") Then _towarehousearea = dr.Item("TaskTowhArea")

                    If Not dr.IsNull("TaskFromLocation") Then _fromlocation = dr.Item("TaskFromLocation")
                    If Not dr.IsNull("TaskToLocation") Then _tolocation = dr.Item("TaskToLocation")
                    If Not dr.IsNull("TaskSKU") Then _sku = dr.Item("TaskSKU")
                    If Not dr.IsNull("TaskConsignee") Then _consignee = dr.Item("TaskConsignee")
                    If Not dr.IsNull("TaskFromLoad") Then _fromload = dr.Item("TaskFromLoad")
                    If Not dr.IsNull("TaskToLoad") Then _toload = dr.Item("TaskToLoad")

                    Dim oLoad As Load = New Load(_fromload)
                    Dim oSKU As SKU = New SKU(_consignee, _sku)

                    _uom = oLoad.LOADUOM
                    _uomquaqntity = oSKU.ConvertUnitsToUom(oLoad.LOADUOM, oLoad.UNITS)
                    _totaluomquaqntity += _uomquaqntity


                    _taskweight = oLoad.CalculateWeight().ToString()
                    _taskcube = oLoad.CalculateVolume().ToString()
                    _tasktravelweight = 0D
                    _tasktravelcube = 0D
                    _taskhusequencelocation = 1

                    _tasktotalsku = 1
                    _taskcurrenthu = "1"
                    _tasktotalhus = 1

                Case WMS.Lib.TASKTYPE.CONTPUTAWAY,
                     WMS.Lib.TASKTYPE.CONTDELIVERY,
                     WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
                    'RWMS-2919'removed ContLoadDelivery

                    If _tasktype <> WMS.Lib.TASKTYPE.CONTDELIVERY Then
                        _fromwarehousearea = _cntload.WAREHOUSEAREA
                        _towarehousearea = _cntload.DESTINATIONWAREHOUSEAREA

                        _fromlocation = _cntload.LOCATION
                        _tolocation = _cntload.DESTINATIONLOCATION
                    Else
                        sql = String.Format("select top 1 TASKTYPE, TASKSUBTYPE, FROMLOCATION,TOLOCATION,FROMWAREHOUSEAREA,TOWAREHOUSEAREA, STARTLOCATION from TASKS where TASK='{0}'", _taskid)

                        Dim dtContDel As New DataTable
                        DataInterface.FillDataset(sql, dtContDel)
                        Dim drContDel As DataRow = dtContDel.Rows(0)

                        _fromwarehousearea = ReplaceDBNull(drContDel("FROMWAREHOUSEAREA"))
                        _towarehousearea = ReplaceDBNull(drContDel("TOWAREHOUSEAREA"))

                        _fromlocation = ReplaceDBNull(drContDel("FROMLOCATION"))
                        _tolocation = ReplaceDBNull(drContDel("TOLOCATION"))
                    End If

                    _sku = _cntload.SKU
                    _consignee = _cntload.CONSIGNEE

                    _fromload = _cntload.LOADID


                    _taskweight += _cntload.Weight()
                    _taskcube += _cntload.Volume()

                    _tasktravelweight = _cntload.Weight
                    _tasktravelcube = _cntload.Volume()
                    _taskhusequencelocation = 1

                    _tasktotalsku = 1
                    _taskcurrenthu = "1"
                    _tasktotalhus = 1
                    'RWMS-2919
                Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY

                    _fromlocation = _lastpicklistdetaillocation
                    _tolocation = _picklistdetail.ToLocation

                    _fromwarehousearea = _picklistdetail.FromWarehousearea
                    _towarehousearea = _picklistdetail.FromWarehousearea


                    _sku = _picklistdetail.SKU
                    _consignee = _picklistdetail.Consignee
                    _fromload = _picklistdetail.FromLoad
                    _toload = _picklistdetail.ToLoad


                    _taskhusequencelocation = DataInterface.ExecuteScalar(String.Format("select isnull(ACCESSIBILITY,0) ACCESSIBILITY from dbo.LOADSACCESSIBILITY where loadid='{0}'",
                        _fromload))
                    _qty = _picklistdetail.AdjustedQuantity

                    _uom = _picklistdetail.UOM
                    Dim oSKU As SKU = New SKU(_consignee, _sku)

                    _uomquaqntity = oSKU.ConvertUnitsToUom(_uom, _qty)
                    _totaluomquaqntity += _uomquaqntity

                    _tasktravelweight = Inventory.CalculateWeight(_consignee, _sku, _qty, _uom)
                    _tasktravelcube = Inventory.CalculateVolume(_consignee, _sku, _qty, _uom)

                    _taskcube += _tasktravelcube
                    _taskweight += _tasktravelweight


                    _taskcurrenthu = "1"
                    _tasktotalsku = 1
                    _tasktotalhus = 1

                Case WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.NEGPALLETPICK
                    _fromlocation = _lastpicklistdetaillocation
                    ''_tolocation = _picklistdetail.ToLocation
                    _tolocation = _picklistdetail.FromLocation
                    _towarehousearea = _picklistdetail.FromWarehousearea

                    _sku = _picklistdetail.SKU
                    _consignee = _picklistdetail.Consignee
                    _fromload = _picklistdetail.FromLoad
                    _toload = _picklistdetail.ToLoad
                    _fromwarehousearea = _picklistdetail.FromWarehousearea
                    ' _towarehousearea = _picklistdetail.ToWarehousearea
                    _taskhusequencelocation = DataInterface.ExecuteScalar(String.Format("select isnull(ACCESSIBILITY,0) ACCESSIBILITY from dbo.LOADSACCESSIBILITY where loadid='{0}'", _
                        _fromload))
                    _qty = _picklistdetail.AdjustedQuantity

                    _uom = _picklistdetail.UOM
                    Dim oSKU As SKU = New SKU(_consignee, _sku)

                    _uomquaqntity = oSKU.ConvertUnitsToUom(_uom, _qty)
                    _totaluomquaqntity += _uomquaqntity

                    _tasktravelweight = Inventory.CalculateWeight(_consignee, _sku, _qty, _uom)
                    _tasktravelcube = Inventory.CalculateVolume(_consignee, _sku, _qty, _uom)

                    _taskcube += _tasktravelcube
                    _taskweight += _tasktravelweight


                    _taskcurrenthu = "1"
                    _tasktotalsku = 1
                    _tasktotalhus = 1
                Case WMS.Lib.TASKTYPE.PARALLELPICKING

                    _fromlocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("FROMLOCATION"))
                    '_tolocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("TOLOCATION"))
                    _sku = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("SKU"))
                    _consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("CONSIGNEE"))
                    _fromload = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("FROMLOAD"))
                    ''_toload = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("TOLOAD"))
                    _tolocation = _nextpicklistdetaillocation
                    _towarehousearea = _nextpicklistdetailWHarea
                    _fromwarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("FROMWAREHOUSEAREA"))
                    ' _towarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("TOWAREHOUSEAREA"))
                    _uom = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("UOM"))
                    _qty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_drparpicklistdetail("ADJQTY"))

                    _taskhusequencelocation = DataInterface.ExecuteScalar(String.Format("select isnull(ACCESSIBILITY,0) ACCESSIBILITY from dbo.LOADSACCESSIBILITY where loadid='{0}'", _
                        _fromload))

                    Dim oSKU As SKU = New SKU(_consignee, _sku)
                    _uomquaqntity = oSKU.ConvertUnitsToUom(_uom, _qty)
                    _totaluomquaqntity += _uomquaqntity

                    _tasktravelweight = Inventory.CalculateWeight(_consignee, _sku, _qty, _uom)
                    _tasktravelcube = Inventory.CalculateVolume(_consignee, _sku, _qty, _uom)

                    _taskcube += _tasktravelcube
                    _taskweight += _tasktravelweight


                    _taskcurrenthu = "1"
                    _tasktotalsku = 1
                    _tasktotalhus = 1
                Case WMS.Lib.TASKTYPE.MISC
                    _taskstartdate = ReplaceDBNull(dr.Item("TaskStartDate"))
                    _taskenddate = ReplaceDBNull(dr.Item("TaskEndDate"))

            End Select

            addLaborCalcParameters("TaskTotalSKU", _tasktotalsku.ToString())
            addLaborCalcParameters("TaskCurrentHU", _taskcurrenthu)
            addLaborCalcParameters("TaskTotalHUs", _tasktotalhus.ToString())


            addLaborCalcParameters("TaskUOM", _uom)
            addLaborCalcParameters("TaskUOMQuantity", _uomquaqntity)
            addLaborCalcParameters("TotalTaskUOMQuantity", _totaluomquaqntity)
            addLaborCalcParameters("TaskCube", _taskcube.ToString())
            addLaborCalcParameters("TaskWeight", _taskweight.ToString())
            addLaborCalcParameters("TaskTravelWeight", _tasktravelweight.ToString())
            addLaborCalcParameters("TaskTravelCube", _tasktravelcube.ToString())
            addLaborCalcParameters("TaskHUSequenceLocation", _taskhusequencelocation.ToString())

        End If



        Return LaborCalcParameters.Count()
    End Function

    ' Labor calc changes RWMS-952 -> PWMS-903
    ' This needs to be changed or be totally removed, temporarly marked with Obsolete
    <Obsolete("Either remove or refactor")>
    Private Sub InitDistanceParameters(ByVal DistanceType As String)
        Dim DistanceKeyName As String
        For Each key As String In LaborPerFormance.LaborPerfDistancesNames.DistanceArray
            DistanceKeyName = DistanceType & key
            addLaborCalcParameters(DistanceKeyName, "0")
        Next
    End Sub


    ' Labor calc changes RWMS-952 -> PWMS-903
    ' This needs to be changed or be totally removed for new method CalculateTravelDistance, temporarly marked with Obsolete
    <Obsolete("Either remove or refactor or replace")>
    Private Sub DivTravelDistance(ByVal DistanceType As String)

        Dim currentkey As String = WHQueryPath.ToLoc
        Dim prevkey As String
        Dim current As WHQueryEdge = DirectCast(_qp.routeFW(currentkey), WHQueryEdge)
        Dim prev As WHQueryEdge

        Dim cnt As Integer = 0
        Dim currDist As Double = 0
        Dim segmentDistance As Double = 0

        Dim dist As Double = Convert.ToDouble(LaborCalcParameters(DistanceType & "FromLocationToLocationDistance"))

        If LaborCalcParameters.ContainsKey("FromLocationAisle") And LaborCalcParameters.ContainsKey("ToLocationAisle") Then
            If String.Equals(LaborCalcParameters("FromLocationAisle"), LaborCalcParameters("ToLocationAisle"), StringComparison.CurrentCultureIgnoreCase) Then
                'If Not LaborCalcParameters.ContainsKey("FromLocationToLocationWalkDistanceSameAisle") Then
                addLaborCalcParameters("FromLocationToLocationWalkDistanceSameAisle", _traveldistance, True, True)
                Dim distanceWithinAisle As Double
                If Not String.Equals(_nextpicklistdetaillocation, _fromlocation, StringComparison.CurrentCultureIgnoreCase) Then
                    distanceWithinAisle = Math.Abs(Double.Parse(LaborCalcParameters("TaskToLocWalkDistance")) - Double.Parse(LaborCalcParameters("TaskFromLocWalkDistance")))
                    segmentDistance = distanceWithinAisle + Double.Parse(LaborCalcParameters("TaskFromLocAisleDistance")) + Double.Parse(LaborCalcParameters("TaskToLocAisleDistance"))
                    addLaborCalcParameters("FromLocationToLocationWalkDistanceSameAisle", _traveldistance, True, True)
                    dist = segmentDistance
                Else
                    distanceWithinAisle = 0
                    segmentDistance = 0
                    addLaborCalcParameters("FromLocationToLocationWalkDistanceSameAisle", _traveldistance, True, True)
                    dist = segmentDistance
                End If


            End If
        End If

        Select Case _tasktype
            Case WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Lib.TASKTYPE.PARTIALPICKING, _
                    WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.NEGPALLETPICK
                addLaborCalcParameters("TaskToLocation", _nextpicklistdetaillocation, True, True)
                addLaborCalcParameters("TaskToLocationDistance", _assigndistance, True, True)
                addLaborCalcParameters("TaskToWhArea", _nextpicklistdetailWHarea, True, True)
        End Select
        If Not LaborCalcParameters.ContainsKey("FromLocationToLocationWalkDistanceSameAisle") Then
            addLaborCalcParameters("FromLocationToLocationWalkDistanceSameAisle", "0", True, True)
        End If

        addLaborCalcParameters("FromLocationToLocationDistance", _traveldistance, True, True)


    End Sub

    ' Labor calc changes RWMS-952 -> PWMS-903
    ' New method used for getting  travel distance
    ' Travel Distance = StartLocation->TaskFromLocation->TaskToLocation
    Private Sub CalculateTravelDistance()

        If Not String.IsNullOrEmpty(_taskStartLocation) And _tasktype <> WMS.Lib.TASKTYPE.LOADLOADING Then
            ' Start location exists

            'Initalize location data
            Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"

            Dim locationsStart As New DataTable()
            DataInterface.FillDataset([String].Format(sql, _taskStartLocation), locationsStart)
            Dim start As DataRow = locationsStart.Rows(0)


            Dim locationsfrom As New DataTable()
            DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsfrom)
            Dim from As DataRow = locationsfrom.Rows(0)

            Dim locationsTaskTo As New DataTable()


            ' RWMS-1497 : Excution Location to To Location
            If _tasktype <> WMS.Lib.TASKTYPE.PARTIALPICKING And _tasktype <> WMS.Lib.TASKTYPE.FULLPICKING Then
                If Not String.IsNullOrEmpty(_executionLocation) Then
                    _tolocation = _executionLocation
                End If
            End If

            ' RWMS-1497 : Excution Location to To Location

            DataInterface.FillDataset([String].Format(sql, _tolocation), locationsTaskTo)
            Dim [to] As DataRow = locationsTaskTo.Rows(0)

            Dim edgeDetails As New DataTable()
            Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

            DataInterface.FillDataset(SQLEdgeDetails, edgeDetails)

            ' Calculate AISLE WALK Distance - Start

            ' Start AISLE Walk Distance

            Dim startAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, start, _prevTaskType)

            addLaborCalcParameters("StartAisleWalkDistance", startAisleWalkDistance, True, True)

            ' Task From AISLE Walk Distance

            Dim taskFromAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, from, _prevTaskType)

            addLaborCalcParameters("TaskFromAisleWalkDistance", taskFromAisleWalkDistance, True, True)

            ' Task To AISLE Walk Distance

            Dim taskToAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, [to], _tasktype)

            addLaborCalcParameters("TaskToAisleWalkDistance", taskToAisleWalkDistance, True, True)

            ' Calculate AISLE WALK Distance - End

            ' Calculate Travel Distance - Start

            Dim operatorsEqpHeight As Double = 0

            'fetch operators equipment height
            If Not String.IsNullOrEmpty(_taskuser) Then
                operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
            End If
            'fetch operators equipment height


            Dim path As Path
            Dim distStartToFrom As Double = 0
            Try

                Dim rules As List(Of Rules) = New List(Of Rules)

                Dim rule As New Rules()

                rule.Parameter = Made4Net.Algorithms.Constants.Height
                rule.Data = operatorsEqpHeight
                rule.Operator = ">"

                rules.Add(rule)

                ' Task Type : _prevTaskType

                path = _sp.GetShortestPathWithContsraints(start, from, _prevTaskType, _tasktype, False, rules, GetShortestPathLogger(_logger))

                ' Check required for checking whether incoming locations are having same coordinates and edges
                If path.Distance.SourceToTargetLocation > 0 Then
                    distStartToFrom = path.Distance.SourceToTargetLocation
                Else
                    If locationsHavingSameCoordinatesAndEdges(start, from, _prevTaskType, _tasktype) Then
                        distStartToFrom = path.Distance.SourceToTargetLocation
                    Else
                        ''RWMS-1938 -  Retrofit of RWMS-1919
                        _logger.SafeWrite("Error in distance calculation from {0} to {1}, setting it to ZERO value.", start, from)
                        distStartToFrom = UndefinedDistance
                    End If
                End If
                _logger.SafeWrite("Calculated travel distance between Start Location {0} and From Location {1} : {2}", start, from, distStartToFrom)
            Catch ex As Exception
                ''RWMS-1938 -  Retrofit of RWMS-1919
                _logger.SafeWrite("Error in distance calculation from {0} to {1}, setting it to ZERO value.", start, from)
                distStartToFrom = UndefinedDistance
            End Try

            Dim distFromToOfTask As Double = 0
            Try

                Dim rules As List(Of Rules) = New List(Of Rules)

                Dim rule As New Rules()

                rule.Parameter = Made4Net.Algorithms.Constants.Height
                rule.Data = operatorsEqpHeight
                rule.Operator = ">"

                rules.Add(rule)

                ' Task Type : _prevTaskType

                path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, False, rules, GetShortestPathLogger(_logger))
                If path.Distance.SourceToTargetLocation > 0 Then
                    distFromToOfTask = path.Distance.SourceToTargetLocation
                Else
                    If locationsHavingSameCoordinatesAndEdges(from, [to], _prevTaskType, _tasktype) Then
                        distFromToOfTask = path.Distance.SourceToTargetLocation
                    Else
                        '' RWMS-1991 -  Retrofit of RWMS-1985
                        _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {1}, setting it to Zero.", from, [to]))
                        distFromToOfTask = UndefinedDistance
                    End If
                End If
                _logger.SafeWrite("Calculated travel distance between From Location {0} and To Location {1} : {2}", from, [to], distFromToOfTask)
            Catch ex As Exception
                '' RWMS-1991 -  Retrofit of RWMS-1985
                _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {1}, setting it to Zero.", from, [to]))
                distFromToOfTask = UndefinedDistance
            End Try

            ' PWMS-907
            _traveldistance = If((distStartToFrom = UndefinedDistance Or distFromToOfTask = UndefinedDistance), UndefinedDistance, distStartToFrom + distFromToOfTask)
            _traveldistanceWithAisleWalk = If((_traveldistance = UndefinedDistance Or startAisleWalkDistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskToAisleWalkDistance = UndefinedDistance), UndefinedDistance, _traveldistance + startAisleWalkDistance + taskFromAisleWalkDistance + taskFromAisleWalkDistance + taskToAisleWalkDistance)
            ' PWMS-907


            'addLaborCalcParameters("TravelDistanceFromStartToTask", distStartToFrom, True, True)
            addLaborCalcParameters("TravelDistanceTaskFromTo", distFromToOfTask, True, True)
            addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)
            addLaborCalcParameters("TotalTravelDistanceWithAisleWalk", _traveldistanceWithAisleWalk, True, True)
            ' Calculate Travel Distance - End
        Else
            _taskStartLocation = ""

            'RWMS-2742 START
            If Not (_tasktype = WMS.Lib.TASKTYPE.LOADDELIVERY Or _tasktype = WMS.Lib.TASKTYPE.CONTDELIVERY) Then
                If String.IsNullOrEmpty(_taskWHActivityLocation) Then
                    _logger.SafeWrite("No Start location using Task Source Location." + _fromlocation)
                Else
                    _logger.SafeWrite("No Start location using WHActivity Location." + _taskWHActivityLocation)
                End If

            End If
            'RWMS-2742 END

            'RWMS-2062
            If _tasktype = WMS.Lib.TASKTYPE.CONTDELIVERY And _tasktype = WMS.Lib.TASKTYPE.LOADLOADING Then
                _taskStartLocation = _fromlocation
                addLaborCalcParameters("TaskStartLocation", _taskStartLocation, True, True)
            End If
            'RWMS-2062

            ' Start location don't exists

            'Initalize location data
            Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"

            Dim from As DataRow
            If _tasktype <> WMS.Lib.TASKTYPE.LOADDELIVERY And _tasktype <> WMS.Lib.TASKTYPE.CONTLOADING And _tasktype <> WMS.Lib.TASKTYPE.LOADLOADING Then
                Dim locationsfrom As New DataTable()
                DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsfrom)
                from = locationsfrom.Rows(0)
            Else
                If _tasktype <> WMS.Lib.TASKTYPE.CONTLOADING And _tasktype <> WMS.Lib.TASKTYPE.LOADLOADING Then
                    sql = "select FromLocation from Tasks where PICKLIST = (select PICKLIST from tasks where task = '{0}') and TASKTYPE <> 'LOADDEL'"
                    Dim fromLocationForLoadDel As String = DataInterface.ExecuteScalar([String].Format(sql, _taskid))
                    _fromlocation = fromLocationForLoadDel
                    _taskStartLocation = _fromlocation
                    Dim locationsfrom As New DataTable()
                    sql = "Select * from LOCATION where LOCATION = '{0}'"
                    DataInterface.FillDataset([String].Format(sql, fromLocationForLoadDel), locationsfrom)
                    from = locationsfrom.Rows(0)
                    _fromwarehousearea = from("WAREHOUSEAREA")
                Else
                    If _tasktype = WMS.Lib.TASKTYPE.CONTLOADING Then
                        _taskStartLocation = _fromlocation
                        Dim locationsfrom As New DataTable()
                        sql = "Select * from LOCATION where LOCATION = '{0}'"
                        DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsfrom)
                        from = locationsfrom.Rows(0)
                        _fromwarehousearea = from("WAREHOUSEAREA")
                    ElseIf _tasktype = WMS.Lib.TASKTYPE.LOADLOADING Then
                        Dim locationsfrom As New DataTable()
                        DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsfrom)
                        _taskStartLocation = _fromlocation
                        from = locationsfrom.Rows(0)
                    End If
                End If

            End If


            Dim locationsTaskTo As New DataTable()


            ' RWMS-1497 : Excution Location to To Location
            If _tasktype <> WMS.Lib.TASKTYPE.PARTIALPICKING And _tasktype <> WMS.Lib.TASKTYPE.FULLPICKING Then
                If Not String.IsNullOrEmpty(_executionLocation) Then
                    _tolocation = _executionLocation
                End If
            End If

            ' RWMS-1497 : Excution Location to To Location

            DataInterface.FillDataset([String].Format(sql, _tolocation), locationsTaskTo)
            Dim [to] As DataRow = locationsTaskTo.Rows(0)

            Dim edgeDetails As New DataTable()
            Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

            DataInterface.FillDataset(SQLEdgeDetails, edgeDetails, False, Nothing)

            ' Calculate AISLE WALK Distance - Start

            Dim edgeCoordinates As DataRow()


            ' Task From AISLE Walk Distance

            Dim taskFromAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, from, _prevTaskType)

            addLaborCalcParameters("TaskFromAisleWalkDistance", taskFromAisleWalkDistance, True, True)

            ' Task To AISLE Walk Distance

            Dim taskToAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, [to], _tasktype)

            addLaborCalcParameters("TaskToAisleWalkDistance", taskToAisleWalkDistance, True, True)

            ' Calculate AISLE WALK Distance - End

            ' Calculate Travel Distance - Start

            Dim operatorsEqpHeight As Double = 0

            'fetch operators equipment height
            If Not String.IsNullOrEmpty(_taskuser) Then
                operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
            End If
            'fetch operators equipment height


            Dim path As Path

            Dim distFromToOfTask As Double = 0
            Try

                Dim rules As List(Of Rules) = New List(Of Rules)

                Dim rule As New Rules()

                rule.Parameter = Made4Net.Algorithms.Constants.Height
                rule.Data = operatorsEqpHeight
                rule.Operator = ">"

                rules.Add(rule)

                ' Task Type : _prevTaskType

                path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, False, rules, GetShortestPathLogger(_logger))
                If path.Distance.SourceToTargetLocation > 0 Then
                    distFromToOfTask = path.Distance.SourceToTargetLocation
                Else
                    If locationsHavingSameCoordinatesAndEdges(from, [to], _prevTaskType, _tasktype) Then
                        distFromToOfTask = path.Distance.SourceToTargetLocation
                    Else
                        _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", from, [to]))
                        distFromToOfTask = UndefinedDistance
                    End If
                End If
                _logger.SafeWrite("Calculated travel distance between From Location {0} and To Location {1} : {2}", from, [to], distFromToOfTask)
            Catch ex As Exception
                _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", from, [to]))
                distFromToOfTask = UndefinedDistance
            End Try

            _traveldistance = distFromToOfTask
            ' PWMS-907
            _traveldistanceWithAisleWalk = If((_traveldistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskToAisleWalkDistance = UndefinedDistance), UndefinedDistance, _traveldistance + taskFromAisleWalkDistance + taskFromAisleWalkDistance + taskToAisleWalkDistance)
            ' PWMS-907

            'addLaborCalcParameters("TravelDistanceFromStartToTask", 0, True, True)
            addLaborCalcParameters("TravelDistanceTaskFromTo", distFromToOfTask, True, True)
            addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)
            addLaborCalcParameters("TotalTravelDistanceWithAisleWalk", _traveldistanceWithAisleWalk, True, True)
            ' Calculate Travel Distance - End
        End If
    End Sub

    Private Function CalculateAisleWalkDistance(edgeDetails As DataTable, location As DataRow, prevTaskType As String) As Double
        Dim edgeCoordinates As DataRow()
        Try
            If prevTaskType = Types.TaskType.NEGTREPL.ToString() Or prevTaskType = Types.TaskType.LDCOUNT.ToString() Or prevTaskType = Types.TaskType.LOCCNT.ToString() Then
                edgeCoordinates = edgeDetails.Select(String.Format("{0} = {1}", "EDGE_ID", Convert.ToString(location("FILLEDGE"))))
            Else
                edgeCoordinates = edgeDetails.Select(String.Format("{0} = {1}", "EDGE_ID", Convert.ToString(location("PICKEDGE"))))
            End If

            ' Now we have all the details, calculate the perpendicular intersection distance
            ' Ref : https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line

            Dim X1, Y1, X2, Y2, X0, Y0 As Integer

            X1 = Convert.ToInt32(edgeCoordinates(0)("X1"))
            Y1 = Convert.ToInt32(edgeCoordinates(0)("Y1"))

            X2 = Convert.ToInt32(edgeCoordinates(0)("X2"))
            Y2 = Convert.ToInt32(edgeCoordinates(0)("Y2"))

            X0 = Convert.ToInt32(location("XCOORDINATE"))
            Y0 = Convert.ToInt32(location("YCOORDINATE"))

            Return Math.Abs(((Y2 - Y1) * X0) - ((X2 - X1) * Y0) + ((X2 * Y1) - (Y2 * X1))) / (Math.Sqrt(Math.Pow((Y2 - Y1), 2) + Math.Pow((X2 - X1), 2)))
        Catch ex As Exception
            _logger.SafeWrite("Exception while calculating Aisle Distance for Location  " + location("LOCATION"))
            Return 0
        End Try
    End Function


    ' Labor calc changes RWMS-952 -> PWMS-903
    ' New method used for getting  travel distance
    Private Sub CalculateTravelDistanceForPickingTask(ByVal startLocation As String, ByVal endLocation As String, ByVal prevousTT As String, ByVal nextTT As String, ByRef travelDistanceFromStartToEnd As Double, ByRef startAisleWalkDistance As Double, ByRef endAisleWalkDistance As Double)
        _logger.SafeWrite("Calculating Travel Distance For Picking Task ")
        If _tasktype = WMS.Lib.TASKTYPE.PARTIALPICKING Then
            Dim distStartToFrom As Double = 0
            If Not String.IsNullOrEmpty(startLocation) Then
                'Initalize location data
                Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"

                Dim locationsStart As New DataTable()
                DataInterface.FillDataset([String].Format(sql, startLocation), locationsStart)
                Dim start As DataRow = locationsStart.Rows(0)

                Dim locationsfrom As New DataTable()
                DataInterface.FillDataset([String].Format(sql, endLocation), locationsfrom)
                Dim from As DataRow = locationsfrom.Rows(0)

                Dim edgeDetails As New DataTable()
                Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

                DataInterface.FillDataset(SQLEdgeDetails, edgeDetails)

                ' Calculate AISLE WALK Distance - Start

                ' Start AISLE Walk Distance
                _logger.SafeWrite("Calculating Start AISLE Walk Distance")
                startAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, start, prevousTT)

                ' Task End AISLE Walk Distance
                _logger.SafeWrite("Calculating End AISLE Walk Distance")
                endAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, from, prevousTT)

                ' Task End AISLE Walk Distance

                ' Calculate AISLE WALK Distance - End


                ' Calculate Travel Distance - Start

                Dim operatorsEqpHeight As Double = 0
                _logger.SafeWrite("Calculating operators equipment height")
                'fetch operators equipment height
                If Not String.IsNullOrEmpty(_taskuser) Then
                    operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
                End If
                'fetch operators equipment height


                Dim path As Path

                Try

                    Dim rules As List(Of Rules) = New List(Of Rules)

                    Dim rule As New Rules()

                    rule.Parameter = Made4Net.Algorithms.Constants.Height
                    rule.Data = operatorsEqpHeight
                    rule.Operator = ">"

                    rules.Add(rule)

                    Dim rule1 As New Rules()

                    rule1.Parameter = Made4Net.Algorithms.Constants.Equipment
                    rule1.Operator = Made4Net.Algorithms.Constants.UniDirection

                    rules.Add(rule1)
                    ' Task Type : _prevTaskType
                    path = _sp.GetShortestPathWithContsraints(start, from, prevousTT, nextTT, False, rules, GetShortestPathLogger(_logger))
                    If Not IsNothing(path.Distance) Then
                        If path.Distance.SourceToTargetLocation > 0 Then
                            distStartToFrom = path.Distance.SourceToTargetLocation
                        Else
                            If locationsHavingSameCoordinatesAndEdges(start, from, _prevTaskType, _tasktype) Then
                                _logger.SafeWrite(String.Format("locationsHavingSameCoordinatesAndEdges() from {0} to {1}, Travel Distance is Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                                distStartToFrom = path.Distance.SourceToTargetLocation
                            Else
                                '' RWMS-1991 -  Retrofit of RWMS-1985
                                _logger.SafeWrite(String.Format("CalculateTravelDistanceForPickingTask():Error in distance calculation from {0} to {1}, Travel Distance is Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                                distStartToFrom = UndefinedDistance
                            End If
                        End If
                    Else
                        _logger.SafeWrite(String.Format("CalculateTravelDistanceForPickingTask():Error in distance calculation from {0} to {1}, Travel Distance is Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                        distStartToFrom = UndefinedDistance
                    End If
                Catch ex As Exception
                    distStartToFrom = UndefinedDistance
                    _logger.SafeWrite("Exception raised in CalculateTravelDistanceForPickingTask(): {0} .", ex.ToString())
                End Try
            End If

            travelDistanceFromStartToEnd = distStartToFrom

            ' Calculate Travel Distance - End
        ElseIf TASKTYPE = WMS.Lib.TASKTYPE.FULLPICKING Then
            Dim distStartToFrom As Double = 0
            If Not String.IsNullOrEmpty(startLocation) Then
                Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"

                Dim locationsStart As New DataTable()
                DataInterface.FillDataset([String].Format(sql, startLocation), locationsStart)
                Dim start As DataRow = locationsStart.Rows(0)

                Dim locationsfrom As New DataTable()
                DataInterface.FillDataset([String].Format(sql, endLocation), locationsfrom)
                Dim from As DataRow = locationsfrom.Rows(0)

                Dim edgeDetails As New DataTable()
                Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

                DataInterface.FillDataset(SQLEdgeDetails, edgeDetails)

                ' Calculate AISLE WALK Distance - Start

                ' Start AISLE Walk Distance
                _logger.SafeWrite("Calculating Start AISLE Walk Distance")
                startAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, start, prevousTT)

                ' Task End AISLE Walk Distance
                _logger.SafeWrite("Calculating End AISLE Walk Distance")
                endAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, from, prevousTT)

                ' Task End AISLE Walk Distance

                ' Calculate AISLE WALK Distance - End


                ' Calculate Travel Distance - Start

                Dim operatorsEqpHeight As Double = 0
                _logger.SafeWrite("Calculating operators equipment height")
                'fetch operators equipment height
                If Not String.IsNullOrEmpty(_taskuser) Then
                    operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
                End If
                'fetch operators equipment height


                Dim path As Path
                Try

                    Dim rules As List(Of Rules) = New List(Of Rules)

                    Dim rule As New Rules()

                    rule.Parameter = Made4Net.Algorithms.Constants.Height
                    rule.Data = operatorsEqpHeight
                    rule.Operator = ">"

                    rules.Add(rule)
                    ' Task Type : _prevTaskType
                    path = _sp.GetShortestPathWithContsraints(start, from, prevousTT, nextTT, False, rules, GetShortestPathLogger(_logger))
                    If Not IsNothing(path.Distance) Then
                        If path.Distance.SourceToTargetLocation > 0 Then
                            distStartToFrom = path.Distance.SourceToTargetLocation
                        Else
                            If locationsHavingSameCoordinatesAndEdges(start, from, _prevTaskType, _tasktype) Then
                                _logger.SafeWrite(String.Format("locationsHavingSameCoordinatesAndEdges from {0} to {1}, setting it to Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                                distStartToFrom = path.Distance.SourceToTargetLocation
                            Else
                                '' RWMS-1991 -  Retrofit of RWMS-1985
                                _logger.SafeWrite(String.Format("CalculateTravelDistanceForPickingTask(incl.prevTask):Error in distance calculation from {0} to {0}, setting it to Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                                distStartToFrom = UndefinedDistance
                            End If
                        End If
                    Else
                        _logger.SafeWrite(String.Format("CalculateTravelDistanceForPickingTask(incl.prevTask):Error in distance calculation from {0} to {0}, setting it to Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                        distStartToFrom = UndefinedDistance
                    End If
                Catch ex As Exception
                    distStartToFrom = UndefinedDistance
                    _logger.SafeWrite("Exception raised in CalculateTravelDistanceForPickingTask(incl.prevTask): {0} .", ex.ToString())
                End Try
            End If
            travelDistanceFromStartToEnd = distStartToFrom
        End If
    End Sub
    'RWMS-2919
    Private Sub CalculateTravelDistanceForContLoadDelivery(ByVal startLocation As String, ByVal endLocation As String, ByVal prevousTT As String, ByVal nextTT As String, ByRef travelDistanceFromStartToEnd As Double)

        Dim distStartToFrom As Double = 0
        If Not String.IsNullOrEmpty(startLocation) Then
            'Initalize location data
            Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"

            Dim locationsStart As New DataTable()
            DataInterface.FillDataset([String].Format(sql, startLocation), locationsStart)
            Dim start As DataRow = locationsStart.Rows(0)

            Dim locationsfrom As New DataTable()
            DataInterface.FillDataset([String].Format(sql, endLocation), locationsfrom)
            Dim from As DataRow = locationsfrom.Rows(0)

            'Dim edgeDetails As New DataTable()
            ' Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

            'DataInterface.FillDataset(SQLEdgeDetails, edgeDetails)

            ' Calculate AISLE WALK Distance - Start

            ' Start AISLE Walk Distance

            'startAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, start, prevousTT)

            ' Task End AISLE Walk Distance

            'endAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, from, prevousTT)

            ' Task End AISLE Walk Distance

            ' Calculate AISLE WALK Distance - End


            ' Calculate Travel Distance - Start

            Dim operatorsEqpHeight As Double = 0

            'fetch operators equipment height
            If Not String.IsNullOrEmpty(_taskuser) Then
                operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
            End If
            'fetch operators equipment height


            Dim path As Path

            Try

                Dim rules As List(Of Rules) = New List(Of Rules)

                Dim rule As New Rules()

                rule.Parameter = Made4Net.Algorithms.Constants.Height
                rule.Data = operatorsEqpHeight
                rule.Operator = ">"

                rules.Add(rule)

                Dim rule1 As New Rules()

                rule1.Parameter = Made4Net.Algorithms.Constants.Equipment
                rule1.Operator = Made4Net.Algorithms.Constants.UniDirection

                rules.Add(rule1)

                path = _sp.GetShortestPathWithContsraints(start, from, prevousTT, nextTT, False, rules, GetShortestPathLogger(_logger))
                If path.Distance.SourceToTargetLocation > 0 Then
                    distStartToFrom = path.Distance.SourceToTargetLocation
                Else
                    If locationsHavingSameCoordinatesAndEdges(start, from, _prevTaskType, _tasktype) Then
                        _logger.SafeWrite(String.Format("locationsHavingSameCoordinatesAndEdges() from {0} to {1}, Travel Distance is Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                        distStartToFrom = path.Distance.SourceToTargetLocation
                    Else
                        _logger.SafeWrite(String.Format("CalculateTravelDistanceForPickingTask():Error in distance calculation from {0} to {1}, Travel Distance is Zero.", start("LOCATION").ToString(), from("LOCATION").ToString()))
                        distStartToFrom = UndefinedDistance
                    End If
                End If
                _logger.SafeWrite("Calculated travel distance between Start Location {0} and From Location {1} : {2}", start, from, distStartToFrom)
            Catch ex As Exception
                distStartToFrom = UndefinedDistance
                _logger.SafeWrite("Exception raised in CalculateTravelDistanceForContLoadDelivery(): {0} .", ex.Message.ToString())
            End Try
        End If

        travelDistanceFromStartToEnd = distStartToFrom

        ' Calculate Travel Distance - End

    End Sub


    ' Labor calc changes RWMS-952 -> PWMS-903
    ' This needs to be changed or be totally removed, temporarly marked with mandatory Obsolete since we do not use old M4N Distance calcualtion API, compiler will trwo error if this is used.
    <Obsolete("Either remove or refactor", True)>
    Private Sub LoadGraph()
        Dim filterarr As ArrayList = New ArrayList()
        If _mhetype <> String.Empty Then
            Dim sql As String = String.Format("SELECT EDGETYPE FROM MHEEDGETYPE where MHETYPE='{0}'", _mhetype)
            Dim dt As New DataTable
            DataInterface.FillDataset(sql, dt)
            Dim dr As DataRow
            For Each dr In dt.Rows
                filterarr.Add(dr.Item("EDGETYPE"))
            Next
        End If
        _qp = New WHQueryPath(filterarr)

    End Sub

    ' Labor calc changes RWMS-952 -> PWMS-903
    ' This needs to be changed or be totally removed for a new method, temporarly marked with Obsolete
    <Obsolete("Either remove or refactor or replace")>
    Private Function LoadTravelDistanceAssignParameter() As Double
        Try
            Dim dist As Double = 0D
            If _fromlocation <> String.Empty And _assigmentlocation <> String.Empty Then
                ' Fetch Location
                Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
                Dim locationsFrom As New DataTable()
                DataInterface.FillDataset([String].Format(sql, _assigmentlocation), locationsFrom)
                Dim from As DataRow = locationsFrom.Rows(0)
                Dim locationsTo As New DataTable()
                DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsTo)
                Dim [to] As DataRow = locationsTo.Rows(0)

                Dim operatorsEqpHeight As Double = 0

                'fetch operators equipment height
                If Not String.IsNullOrEmpty(_taskuser) Then
                    operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
                End If
                'fetch operators equipment height

                Dim path As Path
                Dim sp As Stopwatch = Stopwatch.StartNew()
                Try
                    ' Unidirectional for Partial Picking
                    If Me.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING Then
                        Dim rules As List(Of Rules) = New List(Of Rules)

                        Dim rule As New Rules()

                        rule.Parameter = Made4Net.Algorithms.Constants.Height
                        rule.Data = operatorsEqpHeight
                        rule.Operator = ">"

                        rules.Add(rule)

                        Dim rule1 As New Rules()
                        rule1.Parameter = Made4Net.Algorithms.Constants.Equipment
                        rule1.Operator = Made4Net.Algorithms.Constants.UniDirection

                        rules.Add(rule1)

                        path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, True, rules, GetShortestPathLogger(_logger))
                        If path.Distance.SourceToTargetLocation > 0 Then
                            dist = path.Distance.SourceToTargetLocation
                        Else
                            dist = UndefinedDistance
                        End If
                    Else
                        Dim rules As List(Of Rules) = New List(Of Rules)

                        Dim rule As New Rules()

                        rule.Parameter = Made4Net.Algorithms.Constants.Height
                        rule.Data = operatorsEqpHeight
                        rule.Operator = ">"

                        rules.Add(rule)

                        path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, True, rules, GetShortestPathLogger(_logger))
                        If path.Distance.SourceToTargetLocation > 0 Then
                            dist = path.Distance.SourceToTargetLocation
                        Else
                            dist = UndefinedDistance
                        End If
                    End If
                Catch ex As Exception
                    dist = UndefinedDistance
                End Try
                sp.Stop()
                _logger.SafeWrite("Time taken for Distance Calculation: {0} Milliseconds", sp.Elapsed.TotalMilliseconds)
                ''111114
                _logger.SafeWrite(String.Format("LoadTravelDistanceAssignParameter:_fromlocation: {0}: _assigmentlocation: {1}", _fromlocation, _assigmentlocation))

                _assigndistance = dist

                If dist > 0 Then
                    DivTravelDistance(LaborPerFormance.DistanceType.Assign)
                End If
            End If

            ''111114
            _logger.SafeWrite("_assigndistance = dist:" & _assigndistance.ToString())

            Return dist
        Catch ex As Exception
            Return 0D
        End Try
    End Function

    ' Labor calc changes RWMS-952 -> PWMS-903
    ' This needs to be changed or be totally removed for a new method, temporarly marked with Obsolete
    <Obsolete("Either remove or refactor or replace")>
    Private Function LoadTravelDistanceParameter() As Double
        Try
            Dim dist As Double = 0D

            If _fromlocation <> String.Empty And _tolocation <> String.Empty Then
                ' Fetch Location
                Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
                Dim locationsFrom As New DataTable()
                DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsFrom)
                Dim from As DataRow = locationsFrom.Rows(0)
                Dim locationsTo As New DataTable()
                DataInterface.FillDataset([String].Format(sql, _tolocation), locationsTo)
                Dim [to] As DataRow = locationsTo.Rows(0)

                Dim path As Path

                Dim operatorsEqpHeight As Double = 0

                'fetch operators equipment height
                If Not String.IsNullOrEmpty(_taskuser) Then
                    operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
                End If
                'fetch operators equipment height

                Dim sp As Stopwatch = Stopwatch.StartNew()
                Try
                    ' Unidirectional for Partial Picking
                    If Me.TASKTYPE = WMS.Lib.TASKTYPE.PARTIALPICKING Then
                        Dim rules As List(Of Rules) = New List(Of Rules)

                        Dim rule As New Rules()

                        rule.Parameter = Made4Net.Algorithms.Constants.Height
                        rule.Data = operatorsEqpHeight
                        rule.Operator = ">"

                        rules.Add(rule)

                        Dim rule1 As New Rules()
                        rule1.Parameter = Made4Net.Algorithms.Constants.Equipment
                        rule1.Operator = Made4Net.Algorithms.Constants.UniDirection

                        rules.Add(rule1)

                        path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, True, rules, GetShortestPathLogger(_logger))
                        If path.Distance.SourceToTargetLocation > 0 Then
                            dist = path.Distance.SourceToTargetLocation
                        Else
                            dist = UndefinedDistance
                        End If
                    Else
                        Dim rules As List(Of Rules) = New List(Of Rules)

                        Dim rule As New Rules()

                        rule.Parameter = Made4Net.Algorithms.Constants.Height
                        rule.Data = operatorsEqpHeight
                        rule.Operator = ">"

                        rules.Add(rule)

                        path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, True, rules, GetShortestPathLogger(_logger))
                        If path.Distance.SourceToTargetLocation > 0 Then
                            dist = path.Distance.SourceToTargetLocation
                        Else
                            dist = UndefinedDistance
                        End If
                    End If
                Catch ex As Exception
                    dist = UndefinedDistance
                End Try
                sp.Stop()
                _logger.SafeWrite("Time taken for Distance Calculation: {0} Milliseconds", sp.Elapsed.TotalMilliseconds)

                _traveldistance = dist

                If dist > 0 Then
                    DivTravelDistance(LaborPerFormance.DistanceType.Tasks)
                End If
            End If

            Return _traveldistance
        Catch ex As Exception
            Return 0D
        End Try
    End Function


    Private Function runEval() As Double
        Dim LogPath As String = String.Empty
        If Not _logger Is Nothing Then LogPath = _logger.LogFilePath
        Dim oEvalEquation As EvalEquation = New EvalEquation(LaborCalcMethods, LaborCalcParameters, LogPath)
        _targetequation = oEvalEquation.Bind(_sourceequation, 0)
        _logger.SafeWriteSeperator("*", 50)
        _logger.SafeWrite("source: " & _sourceequation)
        _logger.SafeWrite("target: " & _targetequation)

        Dim res As Double = oEvalEquation.EvalEquation(_sourceequation, _targetequation)
        _logger.SafeWrite("res: " & res.ToString())
        _logger.SafeWriteSeperator("-", 50)
        Return res

    End Function

    Private Function GetOperatorsEquipmentHeight(user As [String]) As Double
        Dim sql As [String] = [String].Format("select  COALESCE(HANDLINGEQUIPMENT.HEIGHT, 0) as HEIGHT from HANDLINGEQUIPMENT inner join WHACTIVITY on HANDLINGEQUIPMENT.HANDLINGEQUIPMENT = WHACTIVITY.HETYPE where WHACTIVITY.USERID = '{0}'", user)
        Dim dt As New DataTable()
        DataInterface.FillDataset(sql, dt, False, "")
        If dt.Rows.Count > 0 Then
            Return Convert.ToDouble(dt.Rows(0)("HEIGHT"))
        End If
        Return 0
    End Function

#End Region

#Region "Public Methods"
    ' Labor calc changes RWMS-952 -> PWMS-903
    ' Not clear the purpose, as it appears currently this is not relavant, no data exists in any of the related tables. Marked Obsolete temporarily
    <Obsolete("Not clear the purpose")>
    Public Sub CalculateCounters(ByVal pLaborCounterScope As String)
        Try
            If _taskuser = String.Empty Then Return

            Dim sql As String = String.Format("SELECT * FROM LABORUSERCOUNTERCALCULATIONS where isnull(COUNTERCALCULATION,'') <>'' and LABORCOUNTERSCOPE='{0}'", pLaborCounterScope)
            Dim dt As New DataTable

            DataInterface.FillDataset(sql, dt)
            Dim currentCounter As String
            For Each dr As DataRow In dt.Rows
                currentCounter = dr("LABORCOUNTERID").ToString()
                _logger.SafeWriteSeperator("*", 60)
                _logger.SafeWrite("Write counter: " & currentCounter)

                _sourceequation = dr("COUNTERCALCULATION").ToString()
                Dim res As Double = runEval()
                If res <> 0 Then
                    sql = String.Format("select top 1 isnull(LABORCOUNTERVALUE,0) as LABORCOUNTERVALUE from LABORUSERCOUNTERS " & _
                                    " where USERID='{0}' and LABORCOUNTERID='{1}'", _taskuser, currentCounter)

                    Dim dt1 As New DataTable
                    DataInterface.FillDataset(sql, dt1)
                    If dt1.Rows.Count = 0 Then
                        sql = String.Format("insert into LABORUSERCOUNTERS " & _
                                        " (USERID,LABORCOUNTERID,LABORCOUNTERVALUE,LABORCOUNTERLASTRESETDATE)" & _
                                        " values " & _
                                        " ('{0}', '{1}','{2}', getdate()) ", _
                                        _taskuser, currentCounter, res.ToString())

                    Else
                        If _newassigment = 0 Then
                            res += Convert.ToDouble(dt1.Rows(0)("LABORCOUNTERVALUE"))
                        End If
                        sql = String.Format("update LABORUSERCOUNTERS " & _
                                            " SET LABORCOUNTERVALUE = '{2}'" & _
                                            " ,LABORCOUNTERLASTRESETDATE = getdate() " & _
                                            " where USERID='{0}' and LABORCOUNTERID='{1} '", _taskuser, currentCounter, res.ToString())
                    End If
                    DataInterface.RunSQL(sql)
                End If

            Next

        Catch ex As Exception
            _logger.SafeWrite("Error CalculateCounters: " & ex.ToString())

        End Try
    End Sub

    ' Labor calc changes RWMS-952 -> PWMS-903
    ' Needs altering due to schema changes
    Public Sub WriteAudit()
        Try
            If _calcid = String.Empty AndAlso Not TASKTYPE.Equals("MISC", StringComparison.CurrentCultureIgnoreCase) Then
                _logger.SafeWrite("Labor Calculation ID is empty. Task details not added to Labor Performance Audit.")
                Return
            End If
            Dim sql As String
            Dim isInsert As Boolean = True
            Dim AssigmentID As String
            AssigmentID = Made4Net.Shared.Util.getNextCounter("LABORASSIGMENT")

            ' Labor calc changes RWMS-952 -> PWMS-903 - ENDTIME is not required

            sql = String.Format("SELECT top 1 * FROM TASKS where TASK='{0}' ", _taskid)

            Dim dt As New DataTable

            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

            'Dim sEndTime As String = DataInterface.ExecuteScalar(sql)
            ' Dim sEndTime As String = dt.Rows(0)("endtime1")

            ' Labor calc changes RWMS-952 -> PWMS-903 - ENDTIME is not required

            ' RWMS-1957 -> RWMS-1951 : Dont override with task form locaion, since the actual form location depends on calculation by labor for picking tasks
            If Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("TASKTYPE")) <> "FULLPICK" And Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("TASKTYPE")) <> "PARPICK" Then
                _fromlocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("fromlocation"))
                _fromwarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("fromwarehousearea"))
            End If
            ' RWMS-1957 -> RWMS-1951

            '_tolocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("tolocation"))
            _towarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("towarehousearea"))

            _taskactualtime = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0)("EXECUTIONTIME"))

            ' RWMS-1497 : Excution Location to To Location
            If _tasktype <> WMS.Lib.TASKTYPE.PARTIALPICKING And _tasktype <> WMS.Lib.TASKTYPE.FULLPICKING Then
                If Not String.IsNullOrEmpty(_executionLocation) Then
                    _tolocation = _executionLocation
                End If
            End If
            'RWMS-2828
            _taskStartLocation = dt.Rows(0)("STARTLOCATION")
            'RWMS-2828 End
            'RWMS-2141
            Dim break As BreakDetails = CalculateBreaksInSeconds(_taskstartdate, _taskenddate)
            'RWMS-2141
            ' RWMS-1497 : Excution Location to To Location

            'Begin RWMS-469

            ' RWMS-1688 : Added from location again.
            '' RWMS-1991 -  Retrofit of RWMS-1985
            ' RWMS-1985/RWMS-1919, the value of undefined distance cannot be 0, since 0 is a genuine value of a distance when the [to] and [from] are same, as such changed back to -1.
            ' While printing the logs and writing into the laborperformanceaudit table, the undefined distance will be changed to 0.
            If _validpicklists Is Nothing OrElse _validpicklists.Count > 0 Then
                If Not IsTaskIdFound(_taskid) Then

                    'RWMS-2795 - round the _stdTime
                    Dim roundedstdTime As Double = Math.Round(_stdTime)
                    'in the below sql replaced _stdTime with roundedstdTime
                    'RWMS-2795 END

                    'RWMS-2141 Modification of query to insert three extra columns
                    ' PWMS-907
                    sql = String.Format("INSERT INTO [LABORPERFORMANCEAUDIT] " & _
                    " ([ASSIGNMENTID]" & _
                    " ,[TASKID]" & _
                    " ,[TASKTYPE]" & _
                    " ,[USERID]" & _
                    " ,[TERMINALTYPE]" & _
                    " ,[MHEID]" & _
                    " ,[SHIFTID]" & _
                    " ,[STARTDATE]" & _
                    " ,[ENDDATE]" & _
                    " ,[STARTLOCATION]" & _
                    " ,[FROMLOCATION]" & _
                    " ,[TOLOCATION]" & _
                    " ,[STARTWAREHOUSEAREA]" & _
                    " ,[FROMWAREHOUSEAREA]" & _
                    " ,[TOWAREHOUSEAREA]" & _
                    " ,[TRAVELDISTANCE]" & _
                    " ,[ACTUALTIME]" & _
                    " ,[STANDARTTIME]" & _
                    " ,[TIMEBLOCKTYPE]" & _
                    " ,[BREAK]" & _
                    " ,[IDLETIME]" & _
                    " )VALUES(" & _
                    " '{0}'" & _
                    " ,'{1}'" & _
                    " ,'{2}'" & _
                    " ,'{3}'" & _
                    " ,'{4}'" & _
                    " ,'{5}'" & _
                    " ,'{6}'" & _
                    " ,{7}" & _
                    " ,{8}" & _
                    " ,'{9}'" & _
                    " ,'{10}'" & _
                    " ,'{11}'" & _
                    " ,'{12}'" & _
                    " ,'{13}'" & _
                    " ,'{14}'" & _
                    " ,'{15}'" & _
                    " ,'{16}'" & _
                    " ,'{17}'" & _
                    " ,'{18}'" & _
                    " ,'{19}'" & _
                    " ,'{20}'" & _
                    " )", _
                       AssigmentID, _taskid, _tasktype, _taskuser, _taskterminaltype, _mheid, _shift, _
                       Made4Net.Shared.Util.FormatField(_taskstartdate), _
                       Made4Net.Shared.Util.FormatField(_taskenddate), _
                    If(String.IsNullOrEmpty(_taskStartLocation), _fromlocation, _taskStartLocation), _fromlocation, _tolocation, _startwarehousearea, _fromwarehousearea, _towarehousearea, _
                    Convert.ToInt32(If((_traveldistance = UndefinedDistance), 0, _traveldistance)).ToString(), _taskactualtime, roundedstdTime, break.breakTimeBlockType, break.breakTimeInSeconds, break.idleTimeInSec) ' PWMS-907

                    ''SHIFTID ????
                    DataInterface.RunSQL(sql)

                    _logger.SafeWrite("Write Audit AssigmentID#" & AssigmentID)
                    _logger.SafeWrite("SQL statement for insertion in Audit : " & sql)

                Else
                    'Update the Laboerperformanceaudit table
                    sql = String.Format("UPDATE [LABORPERFORMANCEAUDIT] SET " & _
                    "[ENDDATE]={0}" & _
                    " ,[TRAVELDISTANCE]={1}" & _
                    " ,[ACTUALTIME]={2}" & _
                    " ,[TIMEBLOCKTYPE]={3}" & _
                    " ,[BREAK]={4}" & _
                    " ,[IDLETIME]={5}" & _
                    " WHERE TASKID={6}", _
                    Made4Net.Shared.Util.FormatField(_taskenddate), _
                    Made4Net.Shared.Util.FormatField(Convert.ToInt32(_traveldistance).ToString()), Made4Net.Shared.Util.FormatField(_taskactualtime), _
                    Made4Net.Shared.Util.FormatField(break.breakTimeBlockType), Made4Net.Shared.Util.FormatField(break.breakTimeInSeconds), break.idleTimeInSec, Made4Net.Shared.Util.FormatField(_taskid))
                    ''SHIFTID ????
                    DataInterface.RunSQL(sql)

                    _logger.SafeWrite("Updated the Table LaborPerformanceAudit for the TASKID#" & _taskid)
                    _logger.SafeWrite("SQL statement for updating the Audit entry : " & sql)

                End If
                'END RWMS-469
            End If
        Catch ex As Exception
            _logger.SafeWrite("Write Audit Error:" & ex.ToString())

        End Try

    End Sub
    'Begin RWMS-469
    Function IsTaskIdFound(ByVal taskId As String) As Boolean

        Dim SQL As String = String.Format("SELECT TASKID FROM LABORPERFORMANCEAUDIT WHERE TASKID='{0}'", taskId.Trim())
        Dim dt As New DataTable

        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    'End RWMS-469
    Public Sub getTaskCalculationID()
        Dim sql As String = String.Format("SELECT top 1 LABORCALCID, isnull(GENERICTIME,0) as GENERICTIME, " & _
        " isnull(NEWASSIGMENT,0) as NEWASSIGMENT, isnull(DEFAULTMHETYPE,'') as DEFAULTMHETYPE  " & _
        " FROM LABORTASKCALCULATION where TASKTYPE ='{0}'  and isnull(TASKSUBTYPE,'')='{1}' ", _tasktype, _tasksubtype)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)

        If dt.Rows.Count > 0 Then
            _calcid = dt.Rows(0)("LABORCALCID").ToString()
            _generictime = Convert.ToDouble(dt.Rows(0)("GENERICTIME"))
            _newassigment = Convert.ToInt32(dt.Rows(0)("NEWASSIGMENT"))
            _mhetype = dt.Rows(0)("DEFAULTMHETYPE").ToString()
            addLaborCalcParameters("TaskMHEType", _mhetype)

        Else
            _calcid = String.Empty
            _logger.SafeWrite("No related Labor Calculation ID found for Task Type: {0} and Task Sub Type: {1}. Labor Calculation ID set to empty.", _tasktype, _tasksubtype)
        End If
    End Sub
    Private Sub PushtoLog(Optional ByVal cnt As String = "")
        _LogArray.OrderBy(Function(c) c)

        RearrangeLogArray(_LogArray)

        cnt = Space(10) & cnt & Space(10)
        Dim lengthOfAppendedString As Int32 = 175 - cnt.ToString().Length
        If cnt <> "" Then
            If lengthOfAppendedString Mod 2 Then
                _logger.SafeWrite(StrDup(Convert.ToInt32(lengthOfAppendedString / 2), "*") & cnt & StrDup(Convert.ToInt32(lengthOfAppendedString / 2), "*"))
            Else
                _logger.SafeWrite(StrDup(Convert.ToInt32(Math.Floor(lengthOfAppendedString / 2)), "*") & cnt & StrDup(Convert.ToInt32(Math.Ceiling(lengthOfAppendedString / 2)), "*"))
            End If
        End If

        Dim sprev As String = String.Empty
        For Each s As String In _LogArray
            If sprev <> s Then _logger.SafeWrite(s)
            sprev = s
        Next
        _LogArray.Clear()
    End Sub
    Function getNextPicklistDetailLocation(ByVal ln As Integer, ByRef nextloc As String, ByRef nextWHarea As String)
        nextWHarea = _picklist.Lines(ln).ToWarehousearea
        For i As Integer = ln + 1 To _picklist.Lines.Count - 1
            Dim oPicklistDetail As PicklistDetail = _picklist.Lines(i)
            If oPicklistDetail.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                nextloc = oPicklistDetail.FromLocation
                nextWHarea = oPicklistDetail.FromWarehousearea
                Exit For
            End If
        Next
        'Return nextloc
    End Function

    Function getNextValidPickLine(ByVal ln As Integer) As PicklistDetail
        For i As Integer = ln To _picklist.Lines.Count - 1
            Dim oPicklistDetail As PicklistDetail = _picklist.Lines(i)
            If oPicklistDetail.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                Return oPicklistDetail
            End If
        Next
    End Function

    Function getLastValidPickLine() As Integer
        For i As Integer = _picklist.Lines.Count - 1 To 0 Step -1
            Dim oPicklistDetail As PicklistDetail = _picklist.Lines(i)
            If oPicklistDetail.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                Return i
            End If
        Next
    End Function


    Function getNextParPicklistDetailLocation(ByVal ln As Integer, ByVal Rows As DataRowCollection, ByRef nextloc As String, ByRef nextWHarea As String)
        Dim dtnext As DataRow = Rows(ln)
        nextloc = dtnext("TOLOCATION").ToString()
        nextWHarea = dtnext("TOWAREHOUSEAREA").ToString()
        For i As Integer = ln + 1 To Rows.Count - 1
            nextloc = Rows(i)("FROMLOCATION").ToString()
            nextWHarea = dtnext("FROMWAREHOUSEAREA").ToString()
            Exit For
        Next
        ' Return nextloc
    End Function

    'bind and eval
    Public Function EvalEquation() As Double
        Try
            If _calcid = String.Empty AndAlso Not TASKTYPE.Equals("misc", StringComparison.InvariantCultureIgnoreCase) Then
                _logger.SafeWrite("Labor Calculation ID is empty. Travel Distance Calculation aborted.")
                Return 0D
            End If
            Dim cnt As Integer = 0
            _stdTime = 0D
            Dim oEvalEquation As EvalEquation

            Select Case _tasktype
                Case WMS.Lib.TASKTYPE.NEGTREPL, WMS.Lib.TASKTYPE.REPLENISHMENT, WMS.Lib.TASKTYPE.PARTREPL, WMS.Lib.TASKTYPE.FULLREPL
                    cnt = 0
                    addLaborCalcParameters("counter", cnt.ToString())
                    LoadTaskTypeParameters()

                    ' Labor calc changes RWMS-952 -> PWMS-903
                    ' Commmented out - Distance calculation is done by new method
                    'InitDistanceParameters(LaborPerFormance.DistanceType.Assign)
                    'LoadTravelDistanceAssignParameter()

                    'InitDistanceParameters(LaborPerFormance.DistanceType.Tasks)
                    'LoadTravelDistanceParameter()

                    CalculateTravelDistance()
                    addLaborCalcParameters("FromLocation", _fromlocation, True, True)
                    ' Labor calc changes RWMS-952 -> PWMS-903
                    PushtoLog(cnt.ToString())

                    _stdTime = runEval()

                Case WMS.Lib.TASKTYPE.LOADPUTAWAY, WMS.Lib.TASKTYPE.LOADDELIVERY, WMS.Lib.TASKTYPE.CONTLOADING, WMS.Lib.TASKTYPE.LOADLOADING
                    cnt = 0
                    addLaborCalcParameters("counter", cnt.ToString())
                    LoadTaskTypeParameters()

                    ' Labor calc changes RWMS-952 -> PWMS-903
                    ' Commmented out - Distance calculation is done by new method
                    'InitDistanceParameters(LaborPerFormance.DistanceType.Assign)
                    'LoadTravelDistanceAssignParameter()

                    'InitDistanceParameters(LaborPerFormance.DistanceType.Tasks)
                    'LoadTravelDistanceParameter()

                    CalculateTravelDistance()
                    addLaborCalcParameters("FromLocation", _fromlocation, True, True)

                    ' Labor calc changes RWMS-952 -> PWMS-903
                    PushtoLog(cnt.ToString())
                    _stdTime = runEval()


                Case WMS.Lib.TASKTYPE.PARTIALPICKING, WMS.Lib.TASKTYPE.FULLPICKING, WMS.Lib.TASKTYPE.NEGPALLETPICK
                    cnt = 0
                    _picklistid = DataInterface.ExecuteScalar(String.Format("select isnull(PICKLIST,'') as PICKLIST from TASKS where TASK='{0}'", _taskid))
                    Dim oPicklist As New Picklist(_picklistid)
                    _picklist = oPicklist
                    If oPicklist.Lines Is Nothing Then Return 0D
                    Dim ruEvalResult As Double
                    Dim sql As String
                    Dim travelDistanceFromStartToEnd As Double = 0
                    Dim startAisleWalkDistance As Double = 0
                    Dim endAisleWalkDistance As Double = 0
                    Dim countSysShortLines As Integer = 0

                    ' Labor calc changes RWMS-952 -> PWMS-903

                    Dim ln As Integer = 0

                    Dim lastPickListDetail As PicklistDetail

                    addLaborCalcParameters("TaskStartLocation", _taskStartLocation, True, True)

                    _validpicklists = (From picklists In oPicklist.Lines
                                       Where picklists.Status <> WMS.Lib.Statuses.Picklist.CANCELED _
                                                AndAlso Not (picklists.Status = WMS.Lib.Statuses.Picklist.COMPLETE _
                                                AndAlso picklists.PickedQuantity = Decimal.Zero _
                                                AndAlso picklists.EditUser.ToUpper = "SYSTEM")
                                       Select CType(picklists, PicklistDetail)).ToList()

                    If _validpicklists.Count > 0 Then
                        Dim firstPickLineFromLocation As String = _validpicklists(0).FromLocation
                        If String.IsNullOrEmpty(_taskStartLocation) Then
                            _logger.SafeWrite("No Start location using Task Source Location." + firstPickLineFromLocation.ToString())
                            _taskStartLocation = firstPickLineFromLocation
                        End If

                        For Each _picklistdetail In _validpicklists

                            If lastPickListDetail Is Nothing Then
                                _lastpicklistdetaillocation = _taskStartLocation
                            Else
                                _lastpicklistdetaillocation = lastPickListDetail.FromLocation
                            End If

                            If Not String.IsNullOrEmpty(_lastpicklistdetaillocation) Then
                                LoadTaskTypeParameters()
                                addLaborCalcParameters("counter", cnt.ToString())
                                addLaborCalcParameters("FromLocation", _lastpicklistdetaillocation, True, True)
                                travelDistanceFromStartToEnd = 0
                                startAisleWalkDistance = 0
                                endAisleWalkDistance = 0
                                If ln = 0 Then
                                    CalculateTravelDistanceForPickingTask(_lastpicklistdetaillocation, _picklistdetail.FromLocation, _prevTaskType, _tasktype, travelDistanceFromStartToEnd, startAisleWalkDistance, endAisleWalkDistance)
                                Else
                                    CalculateTravelDistanceForPickingTask(_lastpicklistdetaillocation, _picklistdetail.FromLocation, _tasktype, _tasktype, travelDistanceFromStartToEnd, startAisleWalkDistance, endAisleWalkDistance)
                                End If
                                _tolocation = _picklistdetail.FromLocation
                                '' RWMS-1991 -  Retrofit of RWMS-1985
                                If (travelDistanceFromStartToEnd > 0) Then
                                    _traveldistance = If((_traveldistance = UndefinedDistance Or travelDistanceFromStartToEnd = UndefinedDistance), UndefinedDistance, _traveldistance + travelDistanceFromStartToEnd)
                                End If


                                If ln = 0 Then
                                    addLaborCalcParameters("FromPickLineAisleWalkDistance", startAisleWalkDistance, True, True)
                                    addLaborCalcParameters("ToPickLineAisleWalkDistance", endAisleWalkDistance, True, True)
                                    addLaborCalcParameters("AssignTravelDistance", travelDistanceFromStartToEnd, True, True)
                                    addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)

                                Else
                                    addLaborCalcParameters("FromPickLineAisleWalkDistance", startAisleWalkDistance, True, True)
                                    addLaborCalcParameters("ToPickLineAisleWalkDistance", endAisleWalkDistance, True, True)
                                    addLaborCalcParameters("TravelDistanceWithinPickLine", travelDistanceFromStartToEnd, True, True)
                                    addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)

                                End If
                                PushtoLog(cnt.ToString)


                            End If
                            'End If
                            'End If
                            lastPickListDetail = _picklistdetail
                        Next
                    End If

                    '' RWMS-1875 - Retrofit of RWMS-1842, check to see that if there is only one pickline, or all picklines are cancelled, still the std time is calculated. Moved the Run eval outside the loop.
                    _stdTime = runEval()
                    PushtoLog(cnt.ToString)
                    '' RWMS-1875 - Retrofit of RWMS-1842, check to see that if there is only one pickline, or all picklines are cancelled, still the std time is calculated. Moved the Run eval outside the loop.


                Case WMS.Lib.TASKTYPE.PARALLELPICKING
                    Dim sql As String = String.Format("select  pickdetail.*, parallelpickdetail.picklistseq from pickdetail " & _
                        "join sku  on pickdetail.consignee = sku.consignee and pickdetail.sku = sku.sku " & _
                        "left join location  on pickdetail.fromlocation = location.location and pickdetail.fromwarehousearea = location.warehousearea " & _
                        "join parallelpickdetail on pickdetail.picklist = parallelpickdetail.picklist " & _
                        " where pickdetail.picklist in (select picklist from parallelpickdetail where parallelpickid in " & _
                        " (select PARALLELPICKLIST as PARALLELPICKLIST  from TASKS where TASK='{0}')) " & _
                        " order by picksortorder, locsortorder", _taskid)
                    Dim dt As New DataTable
                    DataInterface.FillDataset(sql, dt)
                    cnt = 0

                    addLaborCalcParameters("Counter", cnt.ToString())

                    InitDistanceParameters(LaborPerFormance.DistanceType.Assign)
                    LoadTravelDistanceAssignParameter()
                    PushtoLog(cnt.ToString)

                    InitDistanceParameters(LaborPerFormance.DistanceType.Tasks)
                    Dim ln As Integer = 0
                    For Each _drparpicklistdetail In dt.Rows
                        getNextParPicklistDetailLocation(ln, dt.Rows, _nextpicklistdetaillocation, _nextpicklistdetailWHarea)

                        LoadTaskTypeParameters()
                        LoadTravelDistanceParameter()
                        cnt += 1
                        addLaborCalcParameters("Counter", cnt.ToString())
                        PushtoLog(cnt.ToString)
                        _stdTime += runEval()


                        ln += 1
                    Next

                Case WMS.Lib.TASKTYPE.CONTPUTAWAY, _
                     WMS.Lib.TASKTYPE.CONTDELIVERY, _
                     WMS.Lib.TASKTYPE.CONTLOADPUTAWAY
                    'WMS.Lib.TASKTYPE.CONTLOADDELIVERY
                    cnt = 0
                    addLaborCalcParameters("counter", cnt.ToString())
                    ' PWMS-909
                    ' Even if there are multiple Delivery Task associated to the same PickList, but on completion of each deleviry task a seperate labor calculation request will be sent.
                    ' So for each delivery task the laborperformanceaudit will have a seperate row.

                    _fromcontainer = DataInterface.ExecuteScalar(String.Format("select FROMCONTAINER from TASKS where TASK='{0}'", _taskid))

                    If Not _fromcontainer Is Nothing Then

                        SetContainerLoad()
                        LoadTaskTypeParameters()
                        CalculateTravelDistance()
                        addLaborCalcParameters("FromLocation", _fromlocation, True, True)
                        PushtoLog(cnt.ToString())
                        _stdTime += runEval()

                    End If
                    'RWMS-2919
                Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY


                    cnt = 0
                    _picklistid = DataInterface.ExecuteScalar(String.Format("select isnull(PICKLIST,'') as PICKLIST from TASKS where TASK='{0}'", _taskid))
                    Dim oPicklist As New Picklist(_picklistid)
                    Dim locationTable As New DataTable()

                    Dim sql As String = String.Format("SELECT PICKLISTLINE, TOLOCATION FROM  PICKDETAIL   INNER JOIN  LOCATION ON PICKDETAIL.TOLOCATION = LOCATION.LOCATION  where picklist = '{0}' AND PICKDETAIL.STATUS <> 'CANCELED' ORDER BY LOCATION.LOCSORTORDER", _picklistid)
                    DataInterface.FillDataset(sql, locationTable)
                    Dim travelDistanceFromStartToEnd As Double = 0
                    Dim startAisleWalkDistance As Double = 0
                    Dim endAisleWalkDistance As Double = 0
                    Dim totalStartAisleWalkDistance As Double = 0
                    Dim totalEndAisleWalkDistance As Double = 0
                    Dim pickLine As Integer = 0

                    addLaborCalcParameters("TaskStartLocation", _taskStartLocation, True, True)

                    _lastpicklistdetaillocation = DataInterface.ExecuteScalar(String.Format("SELECT TOP 1 FROMLOCATION FROM PICKDETAIL WHERE PICKLIST='{0}'", _picklistid))

                    _taskStartLocation = _lastpicklistdetaillocation

                    addLaborCalcParameters("TaskStartLocation", _taskStartLocation, True, True)

                    For Each dr As DataRow In locationTable.Rows
                        pickLine = Convert.ToInt32(dr("PICKLISTLINE"))
                        _picklistdetail = oPicklist.Item(pickLine)
                        Dim ToLocation As String = Convert.ToString(dr("TOLOCATION"))
                        LoadTaskTypeParameters()
                        addLaborCalcParameters("counter", cnt.ToString())
                        addLaborCalcParameters("FromLocation", _lastpicklistdetaillocation, True, True)
                        travelDistanceFromStartToEnd = 0

                        CalculateTravelDistanceForContLoadDelivery(_lastpicklistdetaillocation, ToLocation, _prevTaskType, _tasktype, travelDistanceFromStartToEnd)

                        If (travelDistanceFromStartToEnd > 0) Then
                            _traveldistance = If((_traveldistance = UndefinedDistance Or travelDistanceFromStartToEnd = UndefinedDistance), UndefinedDistance, _traveldistance + travelDistanceFromStartToEnd)
                        End If

                        addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)


                        PushtoLog(cnt.ToString)
                        cnt += 1
                        _lastpicklistdetaillocation = ToLocation

                    Next

                    _stdTime = runEval()
                    PushtoLog(cnt.ToString)

                Case WMS.Lib.TASKTYPE.LOADCOUNTING, WMS.Lib.TASKTYPE.LOCATIONCOUNTING
                    ' RWMS-1497 : Counting task has no to location, only fromlocation, also if execution location exists and is different from from location then the from->to distance will be calculated or else
                    ' only start -> from will be calculated.
                    cnt = 0
                    addLaborCalcParameters("counter", cnt.ToString())
                    LoadTaskTypeParameters()

                    CalculateTravelDistanceForCounting()
                    addLaborCalcParameters("FromLocation", _fromlocation, True, True)

                    PushtoLog(cnt.ToString())

                    _stdTime = runEval()

                Case WMS.Lib.TASKTYPE.MISC
                    Dim misc As Task = New Task(TASKID)
                    LoadTaskTypeParameters()
                    _stdTime = misc.STDTIME
            End Select


            _logger.SafeWriteSeperator("+", 60)
            _logger.SafeWrite("Complete.")

            Return _stdTime

        Catch ex As Exception
            _logger.SafeWriteSeperator("+", 60)
            _logger.SafeWrite("Error EvalEquation: " & ex.ToString())
            Return 0D
        End Try
    End Function



#End Region

    ' RWMS-1243 : Push Task ID and PIckList ID at the begnining of the Log file.
    Private Sub RearrangeLogArray(LogArray As List(Of String))
        Dim picklist As String = ""
        Dim taskPicklist As String = ""
        Dim taskId As String = ""
        Dim pickListIndex As Int32 = LogArray.FindIndex(Function(c) c.StartsWith("PICKLIST:"))
        If (pickListIndex > 0) Then
            picklist = LogArray(pickListIndex)
            LogArray.RemoveAt(pickListIndex)
        End If
        Dim taskPickListIndex As Int32 = LogArray.FindIndex(Function(c) c.StartsWith("TaskPickList:"))
        If (taskPickListIndex > 0) Then
            taskPicklist = LogArray(taskPickListIndex)
            LogArray.RemoveAt(taskPickListIndex)
        End If
        Dim taskIdIndex As Int32 = LogArray.FindIndex(Function(c) c.StartsWith("TaskID:"))
        If (taskIdIndex > 0) Then
            taskId = LogArray(taskIdIndex)
            LogArray.RemoveAt(taskIdIndex)
        End If

        If (pickListIndex > 0) Then
            LogArray.Insert(0, picklist)
        End If
        If (taskPickListIndex > 0) Then
            LogArray.Insert(0, taskPicklist)
        End If
        If (taskIdIndex > 0) Then
            LogArray.Insert(0, taskId)
        End If
    End Sub

    Private Sub SetContainerLoad()
        Try
            _container = New Container(_fromcontainer, True)
            If _container.Loads.Count > 0 Then
                _cntload = _container.Loads(0)
            Else
                _cntload = Load.CreateLoadFromLastMovedDateForHandlingUnit(_fromcontainer)
            End If
        Catch ex As Exception
            _logger.SafeWrite("Error while retrieving containerloads: " & ex.ToString())
        End Try

    End Sub

    ''' <summary>
    ''' RWMS-1497 : Counting task has no to location, only fromlocation, also if execution location exists and is different from from location then the from->to distance will be calculated or else
    ''' only start -> from will be calculated.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CalculateTravelDistanceForCounting()
        If Not String.IsNullOrEmpty(_taskStartLocation) Then
            ' Start location exists

            'Initalize location data
            Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"

            Dim locationsStart As New DataTable()
            DataInterface.FillDataset([String].Format(sql, _taskStartLocation), locationsStart)
            Dim start As DataRow = locationsStart.Rows(0)


            Dim locationsfrom As New DataTable()
            DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsfrom)
            Dim from As DataRow = locationsfrom.Rows(0)

            Dim locationsTaskTo As New DataTable()


            ' RWMS-1497 : Excution Location to To Location
            If Not String.IsNullOrEmpty(_executionLocation) Then
                _tolocation = _executionLocation
            End If
            Dim [to] As DataRow
            ' RWMS-1497 : Excution Location to To Location
            If Not String.IsNullOrEmpty(_tolocation) Then
                DataInterface.FillDataset([String].Format(sql, _tolocation), locationsTaskTo)
                [to] = locationsTaskTo.Rows(0)
            End If


            Dim edgeDetails As New DataTable()
            Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

            DataInterface.FillDataset(SQLEdgeDetails, edgeDetails)

            ' Calculate AISLE WALK Distance - Start

            ' Start AISLE Walk Distance

            Dim startAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, start, _prevTaskType)

            addLaborCalcParameters("StartAisleWalkDistance", startAisleWalkDistance, True, True)

            ' Task From AISLE Walk Distance

            Dim taskFromAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, from, _prevTaskType)

            addLaborCalcParameters("TaskFromAisleWalkDistance", taskFromAisleWalkDistance, True, True)

            Dim taskToAisleWalkDistance As Double = 0
            ' Task To AISLE Walk Distance
            If Not String.IsNullOrEmpty(_tolocation) Then
                taskToAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, [to], _tasktype)

                addLaborCalcParameters("TaskToAisleWalkDistance", taskToAisleWalkDistance, True, True)
            End If


            ' Calculate AISLE WALK Distance - End

            ' Calculate Travel Distance - Start

            Dim operatorsEqpHeight As Double = 0

            'fetch operators equipment height
            If Not String.IsNullOrEmpty(_taskuser) Then
                operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
            End If
            'fetch operators equipment height


            Dim path As Path
            Dim distStartToFrom As Double = 0
            Try

                Dim rules As List(Of Rules) = New List(Of Rules)

                Dim rule As New Rules()

                rule.Parameter = Made4Net.Algorithms.Constants.Height
                rule.Data = operatorsEqpHeight
                rule.Operator = ">"

                rules.Add(rule)

                ' Task Type : _prevTaskType

                path = _sp.GetShortestPathWithContsraints(start, from, _prevTaskType, _tasktype, False, rules, GetShortestPathLogger(_logger))
                If path.Distance.SourceToTargetLocation > 0 Then
                    distStartToFrom = path.Distance.SourceToTargetLocation
                Else
                    If locationsHavingSameCoordinatesAndEdges(start, from, _prevTaskType, _tasktype) Then
                        distStartToFrom = path.Distance.SourceToTargetLocation
                    Else
                        _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", start, from))
                        distStartToFrom = UndefinedDistance
                    End If
                End If
                _logger.SafeWrite("Calculated travel distance between Start Location {0} and From Location {1} : {2}", start, from, distStartToFrom)
            Catch ex As Exception
                _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", start, from))
                distStartToFrom = UndefinedDistance
            End Try

            Dim distFromToOfTask As Double = 0
            If Not String.IsNullOrEmpty(_tolocation) And _fromlocation <> _tolocation Then
                Try

                    Dim rules As List(Of Rules) = New List(Of Rules)

                    Dim rule As New Rules()

                    rule.Parameter = Made4Net.Algorithms.Constants.Height
                    rule.Data = operatorsEqpHeight
                    rule.Operator = ">"

                    rules.Add(rule)

                    ' Task Type : _prevTaskType

                    path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, False, rules, GetShortestPathLogger(_logger))
                    If path.Distance.SourceToTargetLocation > 0 Then
                        distFromToOfTask = path.Distance.SourceToTargetLocation
                    Else
                        If locationsHavingSameCoordinatesAndEdges(from, [to], _prevTaskType, _tasktype) Then
                            distFromToOfTask = path.Distance.SourceToTargetLocation
                        Else
                            _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", from, [to]))
                            distFromToOfTask = UndefinedDistance
                        End If
                    End If
                    _logger.SafeWrite("Calculated travel distance between From Location {0} and To Location {1} : {2}", from, [to], distFromToOfTask)
                Catch ex As Exception
                    _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", from, [to]))
                    distFromToOfTask = UndefinedDistance
                End Try
            End If


            ' PWMS-907
            _traveldistance = If((distStartToFrom = UndefinedDistance Or distFromToOfTask = UndefinedDistance), UndefinedDistance, distStartToFrom + distFromToOfTask)
            _traveldistanceWithAisleWalk = If((_traveldistance = UndefinedDistance Or startAisleWalkDistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskToAisleWalkDistance = UndefinedDistance), UndefinedDistance, _traveldistance + startAisleWalkDistance + taskFromAisleWalkDistance + taskFromAisleWalkDistance + taskToAisleWalkDistance)
            ' PWMS-907


            'addLaborCalcParameters("TravelDistanceFromStartToTask", distStartToFrom, True, True)
            addLaborCalcParameters("TravelDistanceTaskFromTo", distFromToOfTask, True, True)
            addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)
            addLaborCalcParameters("TotalTravelDistanceWithAisleWalk", _traveldistanceWithAisleWalk, True, True)
            ' Calculate Travel Distance - End
        Else
            Dim sql As [String] = "Select * from LOCATION where LOCATION = '{0}'"
            Dim locationsfrom As New DataTable()
            DataInterface.FillDataset([String].Format(sql, _fromlocation), locationsfrom)
            Dim from As DataRow = locationsfrom.Rows(0)

            Dim locationsTaskTo As New DataTable()


            ' RWMS-1497 : Excution Location to To Location
            If Not String.IsNullOrEmpty(_executionLocation) Then
                _tolocation = _executionLocation
            End If
            Dim [to] As DataRow
            ' RWMS-1497 : Excution Location to To Location
            If Not String.IsNullOrEmpty(_tolocation) Then
                DataInterface.FillDataset([String].Format(sql, _tolocation), locationsTaskTo)
                [to] = locationsTaskTo.Rows(0)
            End If


            Dim edgeDetails As New DataTable()
            Dim SQLEdgeDetails As [String] = "select WE.EDGE_ID,Temp.FROMNODEID,Temp.X1,Temp.Y1,Temp.TONODEID ,Temp.X2,Temp.Y2 " + " from (SELECT   B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 " + " FROM  WAREHOUSEMAPEDGES A LEFT JOIN  ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.FROMNODEID = B.NODEID ) AS B " + " ON     B.FROMNODEID = A.FROMNODEID LEFT JOIN  ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE " + " FROM   WAREHOUSEMAPEDGES A JOIN   WAREHOUSEMAPNODES B ON     A.TONODEID = B.NODEID ) AS C " + " ON     C.TONODEID = A.TONODEID) Temp Join WAREHOUSEMAPEDGES WE on we.FROMNODEID =Temp.FROMNODEID and we.TONODEID=temp.TONODEID " + " where Temp.FROMNODEID is not null and Temp.TONODEID is not null  order by EDGE_ID   "

            DataInterface.FillDataset(SQLEdgeDetails, edgeDetails)

            ' Calculate AISLE WALK Distance - Start

            ' Task From AISLE Walk Distance

            Dim taskFromAisleWalkDistance As Double = CalculateAisleWalkDistance(edgeDetails, from, _prevTaskType)

            addLaborCalcParameters("TaskFromAisleWalkDistance", taskFromAisleWalkDistance, True, True)

            Dim taskToAisleWalkDistance As Double = 0
            ' Task To AISLE Walk Distance
            If Not String.IsNullOrEmpty(_tolocation) Then
                taskToAisleWalkDistance = CalculateAisleWalkDistance(edgeDetails, [to], _tasktype)

                addLaborCalcParameters("TaskToAisleWalkDistance", taskToAisleWalkDistance, True, True)
            End If


            ' Calculate AISLE WALK Distance - End

            ' Calculate Travel Distance - Start

            Dim operatorsEqpHeight As Double = 0

            'fetch operators equipment height
            If Not String.IsNullOrEmpty(_taskuser) Then
                operatorsEqpHeight = GetOperatorsEquipmentHeight(_taskuser)
            End If
            'fetch operators equipment height


            Dim path As Path
            Dim distFromToOfTask As Double = 0
            If Not String.IsNullOrEmpty(_tolocation) And _fromlocation <> _tolocation Then
                Try

                    Dim rules As List(Of Rules) = New List(Of Rules)

                    Dim rule As New Rules()

                    rule.Parameter = Made4Net.Algorithms.Constants.Height
                    rule.Data = operatorsEqpHeight
                    rule.Operator = ">"

                    rules.Add(rule)

                    ' Task Type : _prevTaskType

                    path = _sp.GetShortestPathWithContsraints(from, [to], _prevTaskType, _tasktype, False, rules, GetShortestPathLogger(_logger))
                    If path.Distance.SourceToTargetLocation > 0 Then
                        distFromToOfTask = path.Distance.SourceToTargetLocation
                    Else
                        If locationsHavingSameCoordinatesAndEdges(from, [to], _prevTaskType, _tasktype) Then
                            distFromToOfTask = path.Distance.SourceToTargetLocation
                        Else
                            _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", from, [to]))
                            distFromToOfTask = UndefinedDistance
                        End If
                    End If
                    _logger.SafeWrite("Calculated travel distance between From Location {0} and To Location {1} : {2}", from, [to], distFromToOfTask)
                Catch ex As Exception
                    _logger.SafeWrite(String.Format("Error in distance calculation from {0} to {0}, setting it to max value.", from, [to]))
                    distFromToOfTask = UndefinedDistance
                End Try
            End If


            ' PWMS-907
            _traveldistance = If((distFromToOfTask = UndefinedDistance), UndefinedDistance, distFromToOfTask)
            _traveldistanceWithAisleWalk = If((_traveldistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskFromAisleWalkDistance = UndefinedDistance Or taskToAisleWalkDistance = UndefinedDistance), UndefinedDistance, _traveldistance + taskFromAisleWalkDistance + taskFromAisleWalkDistance + taskToAisleWalkDistance)
            ' PWMS-907

            addLaborCalcParameters("TravelDistanceTaskFromTo", distFromToOfTask, True, True)
            addLaborCalcParameters("TotalTravelDistance", _traveldistance, True, True)
            addLaborCalcParameters("TotalTravelDistanceWithAisleWalk", _traveldistanceWithAisleWalk, True, True)
            ' Calculate Travel Distance - End
        End If
    End Sub

    ' Check required for checking whether incoming locations are having same coordinates and edges
    Private Function locationsHavingSameCoordinatesAndEdges(FromLoc As DataRow, ToLoc As DataRow, prevTaskType As String, nextTaskType As String) As Boolean
        'In new system we have only negative replen
        Dim FromEdge, ToEdge As String
        Dim FromX, FromY, ToX, ToY As Int16

        If prevTaskType = Types.TaskType.NEGTREPL.ToString() OrElse prevTaskType = Types.TaskType.LDCOUNT.ToString() OrElse prevTaskType = Types.TaskType.LOCCNT.ToString() Then
            FromEdge = Convert.ToString(FromLoc("FILLEDGE"))
            FromX = Convert.ToInt16(FromLoc("XFILLCORDINATE"))
            FromY = Convert.ToInt16(FromLoc("YFILLCORDINATE"))
        Else
            FromEdge = Convert.ToString(FromLoc("PICKEDGE"))
            FromX = Convert.ToInt16(FromLoc("XCOORDINATE"))

            FromY = Convert.ToInt16(FromLoc("YCOORDINATE"))
        End If


        If nextTaskType = Types.TaskType.NEGTREPL.ToString() OrElse nextTaskType = Types.TaskType.LDCOUNT.ToString() OrElse nextTaskType = Types.TaskType.LOCCNT.ToString() Then

            ToEdge = Convert.ToString(ToLoc("FILLEDGE"))
            'test for private member
            ToX = Convert.ToInt16(ToLoc("XFILLCORDINATE"))
            ToY = Convert.ToInt16(ToLoc("YFILLCORDINATE"))
        Else
            ToEdge = Convert.ToString(ToLoc("PICKEDGE"))
            ToX = Convert.ToInt16(ToLoc("XCOORDINATE"))

            ToY = Convert.ToInt16(ToLoc("YCOORDINATE"))
        End If
        If (String.Equals(FromEdge, ToEdge)) Then
            If FromX = ToX And FromY = ToY Then
                Return True
            End If
        End If

    End Function

#Region "RWMS-2141"
    ' RWMS-2141
    Private Function CalculateBreaksInSeconds(ByVal taskStartDate As Date, ByVal taskEndDate As Date) As BreakDetails
        'RWMS-2678
        Dim sql As String = String.Format("select top(1) * from LABORPERFORMANCEAUDIT where userid = '{0}' AND ENDDATE IS NOT NULL order by ENDDATE desc", _taskuser)
        _logger.SafeWrite("Proceeding to fetch the most recent completed task for userid " & _taskuser & " - " & sql)
        Dim lastTaskAudit As New DataTable
        DataInterface.FillDataset(sql, lastTaskAudit)
        Dim lastTaskEndTime As Date?
        Dim idleTimeInSpan As TimeSpan = New TimeSpan(0)
        Dim break As BreakDetails

        If lastTaskAudit.Rows.Count > 0 Then
            Dim row As DataRow = lastTaskAudit.Rows(0)
            lastTaskEndTime = row("ENDDATE")
            _logger.SafeWrite("Last Task End Time: {0}", lastTaskEndTime)
            If lastTaskEndTime.HasValue Then
                idleTimeInSpan = CalculateIdleTimeSpan(lastTaskEndTime, taskStartDate)
            End If
            _logger.SafeWrite("Idle Time: {0}", idleTimeInSpan)
            ' RWMS-2232 - Reset the Idle time to 0 if the Idle time is greater than 8 hours(Different Shift Day)
            If idleTimeInSpan.TotalHours > 8 Then
                _logger.SafeWrite("Idle time found greater than 8 hours(Different shift Day), resetting it to 0(Accounting it as first task of the Day) for the TASKID#" & _taskid)
                idleTimeInSpan = New TimeSpan(0)
            ElseIf idleTimeInSpan.TotalSeconds < 0 Then
                _logger.SafeWrite("Idle time found negative, resetting it to 0(Accounting it as first task of the Day) for the TASKID#" & _taskid)
                idleTimeInSpan = New TimeSpan(0)
            End If
            ' RWMS-2232 - Reset the Idle time to 0 if the Idle time is greater than 8 hours(Different Shift Day)
            break = FindBreaksInSecondsAndType(taskStartDate, taskEndDate, idleTimeInSpan)

        Else
            _logger.SafeWrite("No recent completed tasks found for userid " & _taskuser)
            'Since last completed task does not exist return break as nothing
            break = New BreakDetails()
            break.idleTimeInSec = Nothing
            break.breakTimeInSeconds = Nothing
            break.breakTimeBlockType = Nothing
        End If
        Return break
    End Function

    Private Function CalculateIdleTimeSpan(ByVal lastTaskEndTime As Date, ByVal taskStartDate As Date) As TimeSpan
        Dim idleTime As TimeSpan = New TimeSpan(0)
        Dim shift As ShiftDetail = New ShiftDetail(_shift)
        Dim shiftStartTime As DateTime = shift.GetShiftStartTime(taskStartDate)
        If shiftStartTime <> DateTime.MinValue Then
            'Calculate idle time by subtracting max of last task end time and shift start time.
            idleTime = taskStartDate - If(lastTaskEndTime < shiftStartTime, shiftStartTime, lastTaskEndTime)
        End If
        Return idleTime
    End Function

    Private Class BreakDetails
        Public breakTimeInSeconds As Integer?
        Public breakTimeBlockType As String
        Public idleTimeInSec As Integer
    End Class

    Private Function FindBreaksInSecondsAndType(ByVal taskStartDate As Date, ByVal taskEndDate As Date, ByVal idleTime As TimeSpan) As BreakDetails
        ' Note the DayOfWeek code in CODELISTDETAIL starts the week from 1 rather than 0 as in .net datetime object.
        Dim dayOfWeek As Integer = taskEndDate.DayOfWeek + 1
        Dim sql As String = String.Format("select * from SHIFTMASTERTIMEBLOCKS where SHIFTCODE = '{0}' and SHIFTDAY = '{1}'", _shift, dayOfWeek)
        Dim timeBlocks As New DataTable
        DataInterface.FillDataset(sql, timeBlocks)
        Dim breakDetails As BreakDetails = New BreakDetails()
        breakDetails.breakTimeInSeconds = Nothing
        breakDetails.breakTimeBlockType = Nothing
        _logger.SafeWrite("Calculating Break Details: ")
        For Each dr As DataRow In timeBlocks.Rows
            Dim fromTime As String = dr("FROMTIME")
            Dim splittedFromTime As String() = fromTime.Split(":")
            Dim hour As Integer = 0
            Dim min As Integer = 0
            If splittedFromTime.Count > 1 Then
                hour = splittedFromTime(0)
                min = splittedFromTime(1)
            ElseIf splittedFromTime.Count = 1 Then
                If splittedFromTime(0).Length = 4 Then
                    hour = splittedFromTime(0).Substring(0, 2)
                    min = splittedFromTime(0).Substring(2, 2)
                ElseIf splittedFromTime(0).Length = 3 Then
                    hour = splittedFromTime(0).Substring(0, 1)
                    min = splittedFromTime(0).Substring(1, 2)
                End If
            End If
            _logger.SafeWrite(String.Format("start hour:{0}   min:{1}", hour, min))
            Dim timeBlockType As String = dr("TIMEBLOCKTYPE")
            Dim breakStartDate As Date = New Date(taskEndDate.Year, taskEndDate.Month, taskEndDate.Day, hour, min, 0)
            _logger.SafeWrite("Break Start Time: {0}", breakStartDate)
            ' Reset -ive Ideal time to 0
            If idleTime.Seconds < 0 Then
                idleTime = New TimeSpan(0)
            End If
            ' Reset -ive Ideal time to 0
            If (taskStartDate - idleTime) <= breakStartDate And taskEndDate > breakStartDate Then
                ' We have a break, calculate span
                Dim toTime As String = dr("TOTIME")
                Dim splittedToTime As String() = toTime.Split(":")
                Dim hourToTime As Integer = 0
                Dim minToTime As Integer = 0
                If splittedToTime.Count > 1 Then
                    hourToTime = splittedToTime(0)
                    minToTime = splittedToTime(1)
                ElseIf splittedFromTime.Count = 1 Then
                    If splittedFromTime(0).Length = 4 Then
                        hourToTime = splittedToTime(0).Substring(0, 2)
                        minToTime = splittedToTime(0).Substring(2, 2)
                    ElseIf splittedFromTime(0).Length = 3 Then
                        hourToTime = splittedToTime(0).Substring(0, 1)
                        minToTime = splittedToTime(0).Substring(1, 2)
                    End If
                End If
                _logger.SafeWrite(String.Format("end hour:{0}   min:{1}", hourToTime, minToTime))
                Dim breakEndtDate As Date = New Date(breakStartDate.Year, breakStartDate.Month, breakStartDate.Day, hourToTime, minToTime, 0)
                _logger.SafeWrite("Break End Date: {0}", breakEndtDate)
                breakDetails.breakTimeInSeconds = (breakEndtDate - breakStartDate).TotalSeconds
                breakDetails.breakTimeBlockType = timeBlockType
                _logger.SafeWrite(String.Format("Break Details - Break Time: {0}    Time Block Type: {1}", breakDetails.breakTimeInSeconds, breakDetails.breakTimeBlockType))
            End If
        Next
        _logger.SafeWrite("Break Details - Idle Time: {0}", idleTime.TotalSeconds)
        breakDetails.idleTimeInSec = idleTime.TotalSeconds
        Return breakDetails
    End Function
    ' RWMS-2141
#End Region


End Class