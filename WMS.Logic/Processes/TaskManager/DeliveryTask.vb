Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class DeliveryTask
    Inherits TASK

#Region "Constructors"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal TaskId As String)
        MyBase.New(TaskId)
    End Sub

#End Region

#Region "Properties"

    <Obsolete("Property not in use any more, should use the picklist / parallel picking object methods and properties")> _
    Public ReadOnly Property ShouldPrintShipLabelOnPicking() As Boolean
        Get
            Try
                If _task_type = WMS.Lib.TASKTYPE.CONTCONTDELIVERY Then
                    Dim parpck As New ParallelPicking(_parallelpicklist)
                    Return parpck.ShouldPrintShipLabel
                Else
                    Dim pck As New Picklist(_picklist)
                    Return pck.ShouldPrintShipLabel
                End If
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property

    <Obsolete("Property not in use any more, should use the picklist / parallel picking object methods and properties")> _
    Public ReadOnly Property ShouldPrintContentList() As Boolean
        Get
            Try
                If _task_type = WMS.Lib.TASKTYPE.CONTCONTDELIVERY Then
                    Dim parpck As New ParallelPicking(_parallelpicklist)
                    Return parpck.ShouldPrintContentList
                Else
                    Dim pck As New Picklist(_picklist)
                    Return pck.ShouldPrintContentList
                End If
            Catch ex As Exception
                Return False
            End Try
        End Get
    End Property

#End Region

#Region "Methods"

#Region "Accessors"

    Private Function DeliveryExists(ByVal pDeliveryType As String, ByVal pPalletId As String, ByVal pUserId As String) As Task
        Dim oTaskManager As New WMS.Logic.TaskManager()
        Dim oTask As Task = oTaskManager.RequestDeliveryTask(pUserId, pDeliveryType, pPalletId)
        If Not oTask Is Nothing Then
            oTask.Post(pUserId)
        End If
        Return oTask
    End Function

    <Obsolete("Property not in use any more, should use the picklist / parallel picking object methods and properties")> _
    Public Sub PrintShipLabel(ByVal prntr As String)
        Select Case _task_type
            Case WMS.Lib.TASKTYPE.CONTDELIVERY
                Dim cntr As New Container(_fromcontainer, False)
                cntr.PrintShipLabel(prntr)
            Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY
                Dim cntr As New Container(_fromcontainer, False)
                cntr.PrintShipLabel(prntr)
            Case WMS.Lib.TASKTYPE.LOADDELIVERY
                Dim ld As New Load(_fromload)
                ld.PrintShippingLabel(prntr)
            Case WMS.Lib.TASKTYPE.CONTCONTDELIVERY
                Dim parpck As New ParallelPicking(_parallelpicklist)
                Dim parpcklist As ParallelPickList
                For Each parpcklist In parpck
                    If parpcklist.ShouldPrintShipLabel Then
                        Try
                            Dim cntr As New Container(parpcklist.Lines(0).ToContainer, False)
                            cntr.PrintShipLabel(prntr)
                        Catch ex As Exception

                        End Try
                    End If
                Next
            Case Else

        End Select
    End Sub

    <Obsolete("Property not in use any more, should use the picklist / parallel picking object methods and properties")> _
    Public Sub PrintContentList(ByVal prntr As String, ByVal pUser As String, Optional ByVal pLang As Int32 = 0, Optional ByVal pReprtName As String = "ContentList")
        Select Case _task_type
            Case WMS.Lib.TASKTYPE.CONTDELIVERY
                Dim cntr As New Container(_fromcontainer, False)
                cntr.PrintContentList(prntr, pLang, pUser, pReprtName)
            Case WMS.Lib.TASKTYPE.CONTLOADDELIVERY
                Dim cntr As New Container(_fromcontainer, False)
                cntr.PrintContentList(prntr, pLang, pUser, pReprtName)
            Case WMS.Lib.TASKTYPE.LOADDELIVERY
                'Dim ld As New Load(_fromload)
                'ld.PrintShippingLabel(prntr)
            Case WMS.Lib.TASKTYPE.CONTCONTDELIVERY
                Dim parpck As New ParallelPicking(_parallelpicklist)
                Dim parpcklist As ParallelPickList
                For Each parpcklist In parpck
                    If parpcklist.ShouldPrintContentList Then
                        Try
                            Dim cntr As New Container(parpcklist.Lines(0).ToContainer, False)
                            cntr.PrintContentList(prntr, pLang, pUser, pReprtName)
                        Catch ex As Exception
                        End Try
                    End If
                Next
            Case Else
        End Select
    End Sub

    Public Function getDeliveryJob()

        'RWMS-2646 RWMS-2645
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2646 RWMS-2645 END

        Dim djob As New DeliveryJob
        If _task_type = WMS.Lib.TASKTYPE.CONTDELIVERY Then

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Task Type:{0}", _task_type))
            End If
            'RWMS-2646 RWMS-2645 END

            djob.isContainer = True
            djob.LoadId = _fromcontainer
            djob.Picklist = _picklist
            djob.toLocation = _tolocation
            djob.toWarehousearea = _towarehousearea
            Dim oCont As New WMS.Logic.Container(_fromcontainer, True)
            djob.HandlingUnitType = oCont.HandlingUnitType
            If _fromlocation = String.Empty Then
                If oCont.Location = String.Empty Then
                    Dim Sql As String = String.Format("SELECT TOP 1 fromlocation FROM  PICKDETAIL INNER JOIN INVLOAD ON PICKDETAIL.TOCONTAINER = INVLOAD.HANDLINGUNIT and pickdetail.toload = INVLOAD.loadid INNER JOIN LOCATION ON PICKDETAIL.TOLOCATION = LOCATION.LOCATION " & _
                                    " where picklist = '{0}' ORDER BY LOCATION.LOCSORTORDER ", _picklist)

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" sql:{0}", Sql))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    Dim loc As String = DataInterface.ExecuteScalar(Sql)
                    _fromlocation = loc

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" location:{0}", loc))
                    End If
                    'RWMS-2646 RWMS-2645 END

                Else
                    _fromlocation = oCont.Location
                    _fromwarehousearea = oCont.Warehousearea
                End If
            End If
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTCONTDELIVERY Then

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Task Type:{0}", _task_type))
            End If
            'RWMS-2646 RWMS-2645 END

            Dim sql As String = String.Format("Select top 1 container, hutype, isnull(destinationlocation, tolocation) as destinationlocation, isnull(DESTINATIONWAREHOUSEAREA, TOWAREHOUSEAREA) as DESTINATIONWAREHOUSEAREA, picklistseq, fromlocation, fromwarehousearea from container left outer join location on container.destinationlocation = location.location " & _
                "join pickdetail on container.container = pickdetail.tocontainer join parallelpickdetail on pickdetail.picklist = PARALLELPICKDETAIL.picklist " & _
                "where oncontainer = '{0}' order by locsortorder,container", _fromcontainer)
            Dim dt As New DataTable

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" sql:{0}", sql))
            End If
            'RWMS-2646 RWMS-2645 END

            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count = 0 Then
                dt.Dispose()
                Return Nothing
            End If
            Dim dr As DataRow = dt.Rows(0)
            djob.isContainer = True
            djob.LoadId = dr("container")
            djob.Picklist = _picklist
            djob.TaskId = _task
            djob.toLocation = dr("destinationlocation")
            djob.toWarehousearea = dr("destinationwarehousearea")
            djob.OrderSeq = dr("picklistseq")
            djob.HandlingUnitType = dr("hutype")
            If _fromlocation = String.Empty Then
                _fromlocation = dr("fromlocation")
                _fromwarehousearea = dr("fromwarehousearea")
            End If
            dt.Dispose()
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTLOADDELIVERY Then

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Task Type:{0}", _task_type))
            End If
            'RWMS-2646 RWMS-2645 END

            Dim SQl As String = String.Format("SELECT TOP 1 * FROM  PICKDETAIL INNER JOIN INVLOAD ON PICKDETAIL.TOCONTAINER = INVLOAD.HANDLINGUNIT and pickdetail.toload = INVLOAD.loadid INNER JOIN  LOCATION ON PICKDETAIL.TOLOCATION = LOCATION.LOCATION " & _
                " where picklist = '{0}' ORDER BY LOCATION.LOCSORTORDER ", _picklist)
            Dim dt As New DataTable

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" SQL:{0}", SQl))
            End If
            'RWMS-2646 RWMS-2645 END

            DataInterface.FillDataset(SQl, dt)
            If dt.Rows.Count = 0 Then
                dt.Dispose()
                Return Nothing
            End If
            Dim dr As DataRow = dt.Rows(0)
            Dim ld As New Load(Convert.ToString(dr("loadid")))
            djob.isContainer = False
            djob.Consignee = ld.CONSIGNEE
            djob.Sku = ld.SKU
            djob.LoadId = ld.LOADID
            djob.Units = ld.UNITS
            djob.UOM = ld.LOADUOM
            If _fromlocation = String.Empty Then
                _fromlocation = dr("fromlocation")
                _fromwarehousearea = dr("fromwarehousearea")
            End If
            Try
                Dim sku As New SKU(ld.CONSIGNEE, ld.SKU)
                djob.skuDesc = sku.SKUDESC
                djob.UOMUnits = sku.ConvertUnitsToUom(ld.LOADUOM, ld.UNITS)
            Catch ex As Exception
            End Try
            djob.Picklist = _picklist
            djob.toLocation = dr("tolocation")
            djob.toWarehousearea = dr("towarehousearea")

            SQl = String.Format("select isnull(hutype,'') as handlingunittype  from invload left outer join container on container.CONTAINER = isnull(INVLOAD.HANDLINGUNIT,'') where loadid = '{0}'", ld.LOADID) 'String.Format("select handlingunittype from invload where loadid = '{0}'", ld.LOADID)

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" sql:{0}", SQl))
            End If
            'RWMS-2646 RWMS-2645 END

            djob.HandlingUnitType = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQl)

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" HandlingUnitType:{0}", djob.HandlingUnitType))
            End If
            'RWMS-2646 RWMS-2645 END

            dt.Dispose()
        Else
            Dim ld As New Load(_toload)
            djob.isContainer = False
            djob.Consignee = ld.CONSIGNEE
            djob.Sku = ld.SKU
            djob.LoadId = ld.LOADID
            djob.Units = ld.UNITS
            djob.UOM = ld.LOADUOM
            Try
                Dim sku As New SKU(ld.CONSIGNEE, ld.SKU)
                djob.skuDesc = sku.SKUDESC
                djob.UOMUnits = sku.ConvertUnitsToUom(ld.LOADUOM, ld.UNITS)
            Catch ex As Exception
            End Try
            djob.Picklist = _picklist
            djob.toLocation = _tolocation
            djob.toWarehousearea = _towarehousearea
            Dim SQL As String

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" From Location:{0}", _fromlocation))
            End If
            'RWMS-2646 RWMS-2645 END

            If _fromlocation = String.Empty Then
                If ld.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.PICKED OrElse ld.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.STAGED Then
                    SQL = String.Format("SELECT TOP 1 FROMLOCATION, FROMWAREHOUSEAREA FROM PICKDETAIL WHERE TOLOAD='{0}'", ld.LOADID)
                    Dim dt As New DataTable
                    Dim dr As DataRow

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" sql:{0}", SQL))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    DataInterface.FillDataset(SQL, dt)
                    If dt.Rows.Count = 0 Then
                        _fromlocation = String.Empty
                        _fromwarehousearea = String.Empty
                    Else
                        _fromlocation = dt.Rows(0)("FROMLOCATION")
                        _fromwarehousearea = dt.Rows(0)("FROMWAREHOUSEAREA")
                    End If
                Else
                    _fromlocation = ld.LOCATION
                    _fromwarehousearea = ld.WAREHOUSEAREA
                End If
            End If
            SQL = String.Format("select isnull(hutype,'') as handlingunittype  from invload left outer join container on container.CONTAINER = isnull(INVLOAD.HANDLINGUNIT,'') where loadid = '{0}'", ld.LOADID) ' String.Format("select handlingunittype from invload where loadid = '{0}'", ld.LOADID)

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" sql:{0}", SQL))
            End If
            'RWMS-2646 RWMS-2645 END

            djob.HandlingUnitType = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" HandlingUnitType:{0}", djob.HandlingUnitType))
            End If
            'RWMS-2646 RWMS-2645 END

        End If
        djob.TaskId = _task
        'check if we have to do an handoff-if the user can access the destination location with his current hetype
        djob.IsHandOff = False
        Dim query, userMHType As String
        Dim dtHE As New DataTable
        query = String.Format("Select * from WHACTIVITY where userid = '{0}' ", USERID)

        'RWMS-2646 RWMS-2645
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" sql:{0}", query))
        End If
        'RWMS-2646 RWMS-2645 END

        DataInterface.FillDataset(query, dtHE)
        If dtHE.Rows.Count >= 1 Then
            userMHType = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dtHE.Rows(0)("hetype"), "%")

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" userMHType:{0}", userMHType))
            End If
            'RWMS-2646 RWMS-2645 END

        Else
            userMHType = ""

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" userMHType:{0}", userMHType))
            End If
            'RWMS-2646 RWMS-2645 END

        End If
        If userMHType <> "" Then
            'check if the delivery can be done directly to the destination location

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" check if the delivery can be done directly to the destination location.. "))
            End If
            'RWMS-2646 RWMS-2645 END

            If Location.Exists(djob.toLocation, djob.toWarehousearea) Then

                'RWMS-2646 RWMS-2645
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" Location {0} exist ", djob.toLocation))
                End If
                'RWMS-2646 RWMS-2645 END

                Dim oLoc As New Location(djob.toLocation, djob.toWarehousearea)
                If Not TaskManager.CanAccessByMHType(userMHType, oLoc.Location, oLoc.WAREHOUSEAREA) Then

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" CanAccessByMHType: NO"))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    Dim originalDest As String = djob.toLocation
                    Dim originalDestWarehousearea As String = djob.toWarehousearea

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" originalDestLocation {0}, originalDestWarehousearea {1}", originalDest, originalDestWarehousearea))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    djob.IsHandOff = True

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" IsHandOff: TRUE"))
                        wmsrdtLogger.Write(String.Format(" Trying to get the HandOffLocation for USERID: {0}, FromLocation: {1}, OriginalDestLocation: {2}, FromWarehouseArea: {3}, OriginalDestWarehouseArea: {4}, HandlingUnitType: {5}", USERID, _fromlocation, originalDest, _fromwarehousearea, originalDestWarehousearea, djob.HandlingUnitType))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    TaskManager.GetFinalDestinationLocation(USERID, _fromlocation, originalDest, _fromwarehousearea, _
                         originalDestWarehousearea, djob.HandlingUnitType)
                    djob.toLocation = originalDest
                    djob.toWarehousearea = originalDestWarehousearea

                    'RWMS-2646 RWMS-2645
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" Location Found: {0}, Warehousearea: {1}", originalDest, originalDestWarehousearea))
                    End If
                    'RWMS-2646 RWMS-2645 END

                    'and create a delivery task to the original destination location
                    'CreateDeliveryTask(djob, originalDest, originalDestWarehousearea)

                    'RWMS-2646 RWMS-2645
                Else
                    If Not wmsrdtLogger Is Nothing Then
                        wmsrdtLogger.Write(String.Format(" CanAccessByMHType: YES"))
                    End If
                    'RWMS-2646 RWMS-2645 END

                End If

                'RWMS-2646 RWMS-2645
            Else
                If Not wmsrdtLogger Is Nothing Then
                    wmsrdtLogger.Write(String.Format(" Location {0} not exist ", djob.toLocation))
                End If
                'RWMS-2646 RWMS-2645 END

            End If
        End If
        ' And set the task start time
        MyBase.SetStartTime()
        Return djob
    End Function

#End Region

#Region "Create"

    'Function for re-create the delivery when original delivery was to handoff locations
    Public Sub CreateDeliveryTask(ByVal deljob As DeliveryJob, ByVal destinationLocation As String, ByVal destinationWarehousearea As String)
        _task = ""
        _userid = ""
        _fromlocation = deljob.toLocation
        _tolocation = destinationLocation
        _fromwarehousearea = deljob.toWarehousearea
        _towarehousearea = destinationWarehousearea
        MyBase.Post()
    End Sub

    Public Sub CreateDeliveryTask(ByVal fromLocation As String, ByVal fromwarehousearea As String, ByVal destinationLocation As String, ByVal destinationWarehousearea As String)
        _task = ""
        _userid = ""
        _fromwarehousearea = fromwarehousearea
        _fromlocation = fromLocation
        _tolocation = destinationLocation
        _towarehousearea = destinationWarehousearea
        MyBase.Post()
    End Sub

    Public Sub CreateLoadDeliveryTask(ByVal pLoadId As String, ByVal pDestinationLocation As String, ByVal pDestinationWarehousearea As String, ByVal pUser As String, Optional ByVal pPicklistId As String = "")
        If Not WMS.Logic.Load.Exists(pLoadId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Deliver load, Load does not exists", "Cannot Deliver load, Load does not exists")
        End If
        'Check if a task already exist
        Dim oTask As Task = DeliveryExists(WMS.Lib.TASKTYPE.LOADDELIVERY, pLoadId, pUser)
        If Not oTask Is Nothing Then
            Return
        End If
        Dim oLoad As New WMS.Logic.Load(pLoadId)
        If oLoad.DESTINATIONLOCATION = String.Empty Or oLoad.DESTINATIONWAREHOUSEAREA = String.Empty Then
            oLoad.SetDestinationLocation(pDestinationLocation, pDestinationWarehousearea, pUser)
        End If
        _picklist = pPicklistId
        CreateLoadDeliveryTask(oLoad, pUser)
    End Sub
    'Begin for RWMS-1294 and RWMS-1222
    'Public Sub CreateContainerDeliveryTask(ByVal pContainerId As String, ByVal pDestinationLocation As String, ByVal pDestinationWarehousearea As String, ByVal pUser As String, Optional ByVal pPicklistId As String = "", Optional ByVal pIsParallel As Boolean = False)
    Public Sub CreateContainerDeliveryTask(ByVal pContainerId As String, ByVal pDestinationLocation As String, ByVal pDestinationWarehousearea As String, ByVal pUser As String, ByVal logger As LogHandler, Optional ByVal pPicklistId As String = "", Optional ByVal pIsParallel As Boolean = False)
        If Not logger Is Nothing Then
            logger.Write("...Checking if the specified container exists for container id " + pContainerId)
        End If

        'End for RWMS-1294 and RWMS-1222

        If Not WMS.Logic.Container.Exists(pContainerId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Deliver Container, Container does not exists", "Cannot Deliver Container, Container does not exists")
        End If
        Dim sameDest As Boolean = True
        Dim oCont As New WMS.Logic.Container(pContainerId, True)
        If oCont.IsExternalContainer Then 'Parallel Picking
            _parallelpicklist = pPicklistId
            Dim Sql As String = String.Format("select count(distinct destinationlocation) FROM container WHERE ONCONTAINER = '{0}'", pContainerId)
            Dim cnt As Int64 = DataInterface.ExecuteScalar(Sql)
            If oCont.DestinationLocation = String.Empty Then
                oCont.SetDestinationLocation(pDestinationLocation, pDestinationWarehousearea, pUser)
            End If
            If cnt > 1 Then
                CreateParallelContainerDeliveryTask(oCont, pUser)
            Else
                CreateContainerDeliveryTask(oCont, pUser)
            End If
        Else
            If oCont.Loads.Count > 0 Then
                Dim destLoc As String = oCont.Loads(0).DESTINATIONLOCATION
                Dim destWarehousearea As String = oCont.Loads(0).DESTINATIONWAREHOUSEAREA
                For Each tmpLoad As WMS.Logic.Load In oCont.Loads
                    If tmpLoad.DESTINATIONLOCATION = String.Empty Or tmpLoad.DESTINATIONWAREHOUSEAREA = String.Empty Then
                        tmpLoad.SetDestinationLocation(pDestinationLocation, pDestinationWarehousearea, pUser)
                    End If
                    If Not tmpLoad.DESTINATIONLOCATION.ToString.Equals(destLoc, StringComparison.OrdinalIgnoreCase) And _
                        tmpLoad.DESTINATIONWAREHOUSEAREA.ToString.Equals(destWarehousearea, StringComparison.OrdinalIgnoreCase) Then
                        sameDest = False
                    End If
                Next
            End If
            If pIsParallel Then
                _parallelpicklist = pPicklistId
            Else
                _picklist = pPicklistId
            End If
            If sameDest Then
                'Check if a task already exist
                'Begin for RWMS-1294 and RWMS-1222
                If Not logger Is Nothing Then
                    logger.Write("...Checking whether Continer delivery task already exist for container id " + pContainerId)
                End If
                'End for RWMS-1294 and RWMS-1222
                Dim oTask As Task = DeliveryExists(WMS.Lib.TASKTYPE.CONTDELIVERY, pContainerId, pUser)
                If Not oTask Is Nothing Then
                    'Begin for RWMS-1294 and RWMS-1222
                    If Not logger Is Nothing Then
                        logger.Write("...Continer delivery task already exists...")
                    End If
                    'Assign the oTask details to Delivery Task
                    _task = oTask.TASK
                    Load()
                    'End for RWMS-1294 and RWMS-1222
                    Return
                End If
                oCont.SetDestinationLocation(pDestinationLocation, pDestinationWarehousearea, pUser)
                
                CreateContainerDeliveryTask(oCont, pUser)
                'Begin for RWMS-1294 and RWMS-1222
                If Not logger Is Nothing Then
                    logger.Write("...Continer delivery task got created...")
                End If
                'End for RWMS-1294 and RWMS-1222
            Else
                'Check if a task already exist
                Dim oTask As Task = DeliveryExists(WMS.Lib.TASKTYPE.CONTLOADDELIVERY, pContainerId, pUser)
                If Not oTask Is Nothing Then
                    'Begin for RWMS-1294 and RWMS-1222
                    If Not logger Is Nothing Then
                        logger.Write("...Continer delivery task already exists...")
                    End If
                    'End for RWMS-1294 and RWMS-1222
                    'Assign the oTask details to Delivery Task
                    _task = oTask.TASK
                    Load()
                    Return
                End If
                
                CreateContainerLoadDeliveryTask(oCont, oCont.Loads(0), pUser)
            End If
        End If
    End Sub

    Private Sub CreateLoadDeliveryTask(ByVal ld As Load, ByVal puser As String)
        _consignee = ld.CONSIGNEE
        _fromload = ld.LOADID
        '_fromlocation = ld.LOCATION
        'Added for RWMS-1462 and RWMS-1194   
        'if ld.location = empty   
        'get the from location from pickdetail table by passing the fromload - because the original location of the load   
        Try
            Dim SQL As String = String.Format("SELECT FROMLOCATION FROM PICKDETAIL WHERE FROMLOAD='{0}'", ld.LOADID)
            Dim dt As New DataTable
            Dim dr As DataRow
            : DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                'Exception   
            End If

            dr = dt.Rows(0)

            If ld.LOCATION = String.Empty Then
                If Not dr.IsNull("FROMLOCATION") Then _fromlocation = dr.Item("FROMLOCATION")
            Else
                _fromlocation = ld.LOCATION
            End If
        Catch ex As Exception

        End Try
        'Ended for  RWMS-1462 and RWMS-1194   

        _fromwarehousearea = ld.WAREHOUSEAREA
        _priority = 200
        _sku = ld.SKU
        _task_type = WMS.Lib.TASKTYPE.LOADDELIVERY
        _toload = ld.LOADID
        _tolocation = ld.DESTINATIONLOCATION
        _towarehousearea = ld.DESTINATIONWAREHOUSEAREA
        MyBase.Post(puser, Nothing) ' RWMS-1227

    End Sub

    Private Sub CreateContainerDeliveryTask(ByVal cn As Container, ByVal puser As String)
        _fromcontainer = cn.ContainerId
        _priority = 200
        _task_type = WMS.Lib.TASKTYPE.CONTDELIVERY
        _tocontainer = cn.ContainerId
        _fromlocation = cn.Location
        _tolocation = cn.DestinationLocation
        _fromwarehousearea = cn.Warehousearea
        _towarehousearea = cn.DestinationWarehousearea

        'RWMS-1807(RWMS-1892)
        'MyBase.Post(puser)
        MyBase.Post(puser, Nothing)
    End Sub

    Private Sub CreateContainerLoadDeliveryTask(ByVal cn As Container, ByVal ld As Load, ByVal puser As String)
        _consignee = ld.CONSIGNEE
        _fromload = ld.LOADID
        _fromlocation = cn.Location
        _fromwarehousearea = cn.Warehousearea
        _fromcontainer = cn.ContainerId
        _priority = 200
        _sku = ld.SKU
        _task_type = WMS.Lib.TASKTYPE.CONTLOADDELIVERY
        _toload = ld.LOADID
        _tolocation = ld.DESTINATIONLOCATION
        _towarehousearea = ld.DESTINATIONWAREHOUSEAREA
        'RWMS-2914
        MyBase.Post(puser, Nothing)
        'RWMS-2914
    End Sub

    Private Sub CreateParallelContainerDeliveryTask(ByVal cn As Container, ByVal puser As String)
        _priority = 200
        _task_type = WMS.Lib.TASKTYPE.CONTCONTDELIVERY
        _fromcontainer = cn.ContainerId

        MyBase.Post(puser)
    End Sub

#End Region

#Region "Deliver"
    'RWMS-2863 
    Public Sub Deliver(ByVal toLocation As String, ByVal toWarehousearea As String, Optional ByVal IsHandOFF As Boolean = False, Optional ByVal oCont As Container = Nothing, Optional ByVal logger As LogHandler = Nothing, Optional ByVal pickList As String = "")
        Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(_tolocation, toLocation, toWarehousearea, logger)

        If _task_type = WMS.Lib.TASKTYPE.LOADDELIVERY Then
            Dim ld As New Load(_fromload)
            If Not oCont Is Nothing Then
                oCont.Place(ld, _userid)
            End If
            If IsHandOFF Then
                ld.Deliver(strConfirmationLocation, toWarehousearea, False, _userid, False, True)
            Else
                ld.Deliver(strConfirmationLocation, toWarehousearea, False, _userid, True, False)
            End If
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTLOADDELIVERY Then
            'RWMS-2863 
            _picklist = pickList
            Dim dJob As DeliveryJob = Me.getDeliveryJob
            Dim ld As New Load(dJob.LoadId)
            If Not oCont Is Nothing Then
                oCont.Place(ld, _userid)
            End If
            If dJob.IsHandOff Then
                ld.Deliver(strConfirmationLocation, toWarehousearea, False, _userid, False, True)
            Else
                ld.Deliver(strConfirmationLocation, toWarehousearea, True, _userid, True, False)
            End If
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTDELIVERY Then
            Dim cntr As New Container(_fromcontainer, True)
            ' Lets see if there is container that on our container and deliver them also
            Dim dt As DataTable = New DataTable
            DataInterface.FillDataset("SELECT CONTAINER FROM CONTAINER WHERE ONCONTAINER='" & _fromcontainer & "'", dt)
            For Each dr As DataRow In dt.Rows
                Dim c As Container = New Container(dr("CONTAINER"), True)
                c.Deliver(strConfirmationLocation, toWarehousearea, USERID, IsHandOFF, False)
            Next
            ' In the end / or if we dont have on container containers lets deliver our container
            cntr.Deliver(strConfirmationLocation, toWarehousearea, USERID, IsHandOFF)
            If Not oCont Is Nothing Then
                For Each oLoad As WMS.Logic.Load In cntr.Loads
                    oCont.Place(oLoad, _userid)
                Next
            End If
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTCONTDELIVERY Then
            If IsHandOFF Then
                Dim cntr As New Container(_fromcontainer, True)
                cntr.Deliver(strConfirmationLocation, toWarehousearea, USERID, IsHandOFF)
            Else
                Dim sql As String = String.Format("Select top 1 container from container left outer join location on container.destinationlocation = location.location and container.destinationwarehousearea = location.warehousearea " & _
                                "where oncontainer = '{0}' order by locsortorder,container", _fromcontainer)
                Dim dt As New DataTable
                DataInterface.FillDataset(sql, dt)
                Dim dr As DataRow = dt.Rows(0)
                Dim cntr As New Container(dr("container"), True)
                cntr.Deliver(strConfirmationLocation, toWarehousearea, USERID, IsHandOFF)
                dt.Dispose()
            End If
        End If
        _executionlocation = strConfirmationLocation
        _executionwarehousearea = toWarehousearea
        If IsHandOFF Then
            Dim tsk As New DeliveryTask()
            tsk.FromContainer = Me.FromContainer
            tsk.FROMLOAD = Me.FROMLOAD
            tsk.TASKTYPE = Me.TASKTYPE
            tsk.ToContainer = Me.ToContainer
            tsk.TOLOAD = Me.TOLOAD
            tsk.Picklist = Me.Picklist
            tsk.ParallelPicklist = Me.ParallelPicklist

            tsk.CreateDeliveryTask(Me.ExecutionLocation, Me.FROMWAREHOUSEAREA, Me.TOLOCATION, Me.TOWAREHOUSEAREA)
            Me.Complete(Nothing)
            Return
        End If

        Dim pCurrentUser As String = _userid

        If _task_type = WMS.Lib.TASKTYPE.CONTLOADDELIVERY Then
            'Me.Complete()
            'Dim pcklst As New Picklist(_picklist)
            'CreateDeliveryTask(pcklst, pCurrentUser)
            If Me.getDeliveryJob Is Nothing Then
                Me.Complete(Nothing)
            End If
        ElseIf _task_type = WMS.Lib.TASKTYPE.CONTCONTDELIVERY Then
            If Me.getDeliveryJob Is Nothing Then
                Me.Complete(Nothing)
            End If
        Else
            Me.Complete(Nothing)
        End If
    End Sub

#End Region

#End Region

End Class

#Region "Delivery Job"

<CLSCompliant(False)> Public Class DeliveryJob
    Public isContainer As Boolean
    Public TaskId As String
    Public LoadId As String
    Public Consignee As String
    Public Sku As String
    Public skuDesc As String
    Public Picklist As String
    Public toLocation As String
    Public toWarehousearea As String
    Public Units As Double
    Public UOM As String
    Public UOMUnits As Double
    Public OrderSeq As Int32
    Public IsHandOff As Boolean
    Public HandlingUnitType As String
End Class

#End Region