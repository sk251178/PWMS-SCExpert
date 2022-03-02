Imports Made4Net.DataAccess

#Region "ParallelPicking"

<CLSCompliant(False)> Public Class ParallelPicking

#Region "Variables"
    Protected _parallelpickid As String
    Protected _status As String
    Protected _tocontainer As String
    Protected _handlingunittype As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
    Protected _picklists As ParallelPickListCollection
#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format(" PARALLELPICKID = '{0}' ", _parallelpickid)
        End Get
    End Property

    Public Property ParallelPickId() As String
        Get
            Return _parallelpickid
        End Get
        Set(ByVal Value As String)
            _parallelpickid = Value
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

    Public ReadOnly Property PickLists() As ParallelPickListCollection
        Get
            Return _picklists
        End Get
    End Property

    Default ReadOnly Property Item(ByVal index As Int32) As ParallelPickList
        Get
            Return _picklists(index)
        End Get
    End Property

    Public ReadOnly Property PickListsHandlingUnitType() As String
        Get
            Return _picklists(0).HandelingUnitType
        End Get
    End Property

    Public ReadOnly Property NumPicks() As Int32
        Get
            Dim npick As Int32 = 0
            Dim parpck As ParallelPickList
            For Each parpck In Me
                npick = npick + parpck.Lines.Count
            Next
            Return npick
        End Get
    End Property

    Public ReadOnly Property ShouldPrintPickLabelOnPicking() As Boolean
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If parpck.ShouldPrintPickLabelOnPicking Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property ShouldPrintShipLabel() As Boolean
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If parpck.ShouldPrintShipLabel Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property ShouldPrintContentList() As Boolean
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If parpck.ShouldPrintContentList Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property Started() As Boolean
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If parpck.Started Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property
    Public ReadOnly Property Completed() As Boolean
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If parpck.Status = WMS.Lib.Statuses.Picklist.RELEASED Or parpck.Status = WMS.Lib.Statuses.Picklist.PLANNED Or parpck.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property
    Public ReadOnly Property Canceled() As Boolean
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If Not parpck.Status = WMS.Lib.Statuses.Picklist.CANCELED Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property
    Protected ReadOnly Property ParallelPickList(ByVal PickListId As String) As WMS.Logic.ParallelPickList
        Get
            Dim parpck As ParallelPickList
            For Each parpck In Me
                If parpck.PicklistID = PickListId Then
                    Return parpck
                End If
            Next
            Return Nothing
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New()
        _picklists = New ParallelPickListCollection
    End Sub

    Public Sub New(ByVal sParallelPickId As String)
        _parallelpickid = sParallelPickId
        Load()
    End Sub

    Public Sub New(ByVal pcklst As Picklist)
        Dim sql As String = String.Format("Select top 1 parallelpickid from PARALLELPICKDETAIL where picklist = '{0}'", pcklst.PicklistID)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException
        End If
        _parallelpickid = dt.Rows(0)("parallelpickid")
        Load()
    End Sub
#End Region

#Region "Methods"

    Protected Sub Load()
        Dim sql As String = String.Format("Select * from PARALLELPICK where ParallelPickId = '{0}'", _parallelpickid)
        Dim dr As DataRow
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        dr = dt.Rows(0)
        _status = dr("STATUS")
        If Not dr.IsNull("tocontainer") Then _tocontainer = dr("tocontainer")
        If Not dr.IsNull("handlingunittype") Then _handlingunittype = dr("handlingunittype")
        If Not dr.IsNull("adddate") Then _adddate = dr("adddate")
        If Not dr.IsNull("adduser") Then _adduser = dr("adduser")
        If Not dr.IsNull("editdate") Then _editdate = dr("editdate")
        If Not dr.IsNull("edituser") Then _edituser = dr("edituser")

        _picklists = New ParallelPickListCollection(_parallelpickid)
    End Sub

    Public Shared Function Exists(ByVal sParallelPickId As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from PARALLELPICK where PARALLELPICKID = '{0}'", sParallelPickId)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Save(ByVal pUser As String)
        If _parallelpickid Is Nothing Or _parallelpickid Is String.Empty Then
            _parallelpickid = Made4Net.Shared.Util.getNextCounter("PARALLELPICK")
        End If

        Dim sql As String

        If Not Exists(_parallelpickid) Then
            _adddate = DateTime.Now
            _adduser = pUser
            _editdate = DateTime.Now
            _edituser = pUser
            _status = WMS.Lib.Statuses.ParallelPickList.AVAILABLE

            sql = String.Format("INSERT INTO PARALLELPICK(PARALLELPICKID, STATUS, TOCONTAINER, HANDLINGUNITTYPE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                "VALUES({0},{1},{2},{3},{4},{5},{6},{7})", _
                Made4Net.Shared.Util.FormatField(_parallelpickid), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_tocontainer), Made4Net.Shared.Util.FormatField(_handlingunittype), _
                Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))

            DataInterface.RunSQL(sql)

            _picklists = New ParallelPickListCollection(_parallelpickid)
        Else
            _editdate = DateTime.Now
            _edituser = pUser

            sql = String.Format("UPDATE PARALLELPICK SET STATUS={0}, TOCONTAINER={1}, HANDLINGUNITTYPE={2}, EDITDATE={3}, EDITUSER={4} Where {5} ", _
                Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_tocontainer), Made4Net.Shared.Util.FormatField(_handlingunittype), _
                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)

            DataInterface.RunSQL(sql)
        End If
    End Sub

    Public Sub AddLine(ByVal pcklst As Picklist)
        Dim seq As Int32 = _picklists.Count + 1
        DataInterface.RunSQL(String.Format("Insert into PARALLELPICKDETAIL(parallelpickid,picklist,picklistseq) values ({0},{1},{2})", Made4Net.Shared.Util.FormatField(_parallelpickid), Made4Net.Shared.Util.FormatField(pcklst.PicklistID), Made4Net.Shared.Util.FormatField(seq)))
        Dim parPckLst As New ParallelPickList(seq, pcklst)
        _picklists.Add(parPckLst)
    End Sub

    Public Function GetEnumerator() As IEnumerator
        Return _picklists.GetEnumerator
    End Function

    Public Sub PrintPickLabels(ByVal prntr As String)
        Dim parpck As ParallelPickList
        For Each parpck In Me
            If parpck.ShouldPrintPickLabelOnPicking Then
                parpck.PrintPickLabels(prntr)
            End If
        Next
    End Sub

    Public Sub PrintShipLabels(ByVal prntr As String)
        Dim parpck As ParallelPickList
        For Each parpck In Me
            If parpck.ShouldPrintShipLabel Then
                parpck.PrintShipLabels(prntr)
            End If
        Next
    End Sub

    Public Sub PrintContentList(ByVal prntr As String, ByVal pUser As String, Optional ByVal pLang As Int32 = 0)
        Dim parpck As ParallelPickList
        For Each parpck In Me
            If parpck.ShouldPrintContentList Then
                parpck.PrintContentList(prntr, pUser, pLang)
            End If
        Next
    End Sub

    Public Sub UnAssign(ByVal UserId As String)
        Dim parpck As ParallelPickList
        For Each parpck In Me
            parpck.UnAssign(UserId)
        Next

        _status = WMS.Lib.Statuses.ParallelPickList.AVAILABLE
        Save(UserId)
    End Sub

    Public Sub AssignUser(ByVal UserId As String)
        Dim parpck As ParallelPickList
        For Each parpck In Me
            parpck.AssignUser(UserId)
        Next

        _status = WMS.Lib.Statuses.ParallelPickList.ASSIGNED
        Save(UserId)
    End Sub

    Public Function GetNextPick() As PickJob
        Dim pckJob As PickJob
        Dim pickjobfound As Boolean = False
        Dim oRelStrat As ReleaseStrategyDetail
        _picklists = New ParallelPickListCollection(_parallelpickid)
        While Not pickjobfound
            pckJob = getPickJob()

            If pckJob Is Nothing Then
                'Calling the complete on the last picklist will complete this.
                'setComplete(WMS.Logic.GetCurrentUser)
                Return pckJob
            End If

            Dim pStrat As New PlanStrategy(_picklists.PickList(pckJob.picklist).StrategyId)
            For Each TempRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
                If TempRelStrat.PickType = _picklists.PickList(pckJob.picklist).PickType Or TempRelStrat.PickType Is Nothing Then
                    oRelStrat = TempRelStrat
                    Exit For
                End If
            Next
            Dim pckList As New WMS.Logic.Picklist(pckJob.picklist)
            If ValidatePickJob(pckList, pckJob, oRelStrat) Then
                pickjobfound = True
                getTaskConfirmation(pckList.getReleaseStrategy().ConfirmationType, pckJob)
            Else
                If pckList.isCompleted OrElse pckList.isCanceled Then
                    pckList.CompleteList(WMS.Lib.USERS.SYSTEMUSER, Nothing)
                End If
            End If
            _picklists.PickList(pckList.PicklistID) = pckList
        End While

        Return pckJob
    End Function

    Protected Function getPickJob() As PickJob
        Dim jobFound As Boolean = False
        Dim currLineStatus As String = ""
        Dim pckjb As PickJob
        Dim grpPickDetails As Boolean = False
        Dim oRelStrat As ReleaseStrategyDetail
        Dim dt As New DataTable
        Dim pckSku As SKU

        Dim sql As String = String.Format("select * from vParallelPickingDetails where parallelpickid = '{0}' and status in ('{1}','{2}') order by PicksSortOrder", _parallelpickid, WMS.Lib.Statuses.Picklist.RELEASED, WMS.Lib.Statuses.Picklist.PLANNED)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            dt = New DataTable
            sql = String.Format("select * from vParallelPickingDetails where parallelpickid = '{0}' and status in ('{1}') order by PicksSortOrder", _parallelpickid, WMS.Lib.Statuses.Picklist.PARTPICKED)
            DataInterface.FillDataset(sql, dt)
        End If
        If dt.Rows.Count = 0 Then
            Return Nothing
        Else
            ' Need to go over all pickdetails in all picklists and check if group pickdetails set to true
            Dim pcklst As Picklist = New Picklist(dt.Rows(0)("PICKLIST"))
            Dim pStrat As New PlanStrategy(pcklst.StrategyId)
            For Each TempRelStrat As ReleaseStrategyDetail In pStrat.ReleaseStrategyDetails
                If TempRelStrat.PickType = pcklst.PickType Or TempRelStrat.PickType Is Nothing Then
                    grpPickDetails = TempRelStrat.GroupPickDetails
                    oRelStrat = TempRelStrat
                    Exit For
                End If
            Next

            Dim isFirst As Boolean = True
            For Each dr As DataRow In dt.Rows
                Dim pckdet As New PicklistDetail(dr)
                If isFirst Then
                    pckjb = New PickJob
                    If dr("status") = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        Dim newPickDetail As PicklistDetail = pckdet.SplitPartPickedLine()
                        pcklst.Lines.Add(newPickDetail)
                        pckdet = newPickDetail
                        pckjb.BasedOnPartPickedLine = True
                    End If
                    pckjb.consingee = pckdet.Consignee ' dr("Consignee")
                    pckjb.sku = pckdet.SKU ' dr("SKU")
                    pckjb.fromload = pckdet.FromLoad ' dr("FromLoad")
                    pckjb.fromlocation = pckdet.FromLocation ' dr("FromLocation")
                    pckjb.fromwarehousearea = pckdet.FromWarehousearea ' dr("FromLocation")
                    pckjb.originaluom = pckdet.UOM ' dr("UOM")
                    pckjb.uom = pckdet.UOM ' dr("UOM")
                    pckjb.picklist = pckdet.PickList ' dr("Picklist")
                    pckjb.units = pckdet.AdjustedQuantity - pckdet.PickedQuantity '  dr("adjqty") - dr("pickedqty")
                    pckjb.parallelpicklistid = _parallelpickid
                    pckjb.parallelpicklistseq = dr("picklistseq")
                    pckSku = New SKU(pckjb.consingee, pckjb.sku)
                    pckjb.skudesc = pckSku.SKUDESC
                    currLineStatus = pckdet.Status ' dr("status")
                    pckjb.PickDetLines.Add(pckdet.PickListLine)
                    isFirst = False
                    Try
                        pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
                        While pckjb.uomunits = 0
                            pckjb.uom = pckSku.getNextUom(pckjb.uom)
                            pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
                        End While
                    Catch ex As Exception
                    End Try
                Else
                    'If dr("FromLoad") = pckjb.fromload And dr("UOM") = pckjb.uom And dr("Picklist") = pckjb.picklist Then
                    If Not String.IsNullOrEmpty(pckdet.FromLoad) Then
                        If pckdet.FromLoad = pckjb.fromload And pckdet.UOM = pckjb.uom And pckdet.PickList = pckjb.picklist Then
                            If dr("status") = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                                Dim newPickDetail As PicklistDetail = pckdet.SplitPartPickedLine()
                                pcklst.Lines.Add(newPickDetail)
                                pckdet = newPickDetail

                                pckjb.BasedOnPartPickedLine = True
                            End If
                            'pckjb.units = pckjb.units + (dr("adjqty") - dr("pickedqty"))
                            pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                            If pckSku Is Nothing Then
                                pckSku = New SKU(pckjb.consingee, pckjb.sku)
                            End If
                            currLineStatus = pckdet.Status ' dr("status")
                            Try
                                pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
                                While pckjb.uomunits = 0
                                    pckjb.uom = pckSku.getNextUom(pckjb.uom)
                                    pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
                                End While
                            Catch ex As Exception
                            End Try
                            pckjb.PickDetLines.Add(pckdet.PickListLine)
                        End If
                    Else
                        If pckdet.FromLocation = pckjb.fromlocation AndAlso pckdet.UOM = pckjb.uom AndAlso pckdet.PickList = pckjb.picklist Then
                            'pckjb.units = pckjb.units + (dr("adjqty") - dr("pickedqty"))
                            If dr("status") = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                                Dim newPickDetail As PicklistDetail = pckdet.SplitPartPickedLine()
                                pcklst.Lines.Add(newPickDetail)
                                pckdet = newPickDetail
                                pckjb.BasedOnPartPickedLine = True
                            End If
                            pckjb.units = pckjb.units + (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                            If pckSku Is Nothing Then
                                pckSku = New SKU(pckjb.consingee, pckjb.sku)
                            End If
                            currLineStatus = pckdet.Status ' dr("status")
                            Try
                                pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
                                While pckjb.uomunits = 0
                                    pckjb.uom = pckSku.getNextUom(pckjb.uom)
                                    pckjb.uomunits = pckSku.ConvertUnitsToUom(pckjb.uom, pckjb.units)
                                End While
                            Catch ex As Exception
                            End Try
                            pckjb.PickDetLines.Add(pckdet.PickListLine)
                        End If
                    End If
                End If
                If Not grpPickDetails Then Exit For
            Next
        End If

        If pckjb.units > 0 Then
            pckjb.oAttributeCollection = getPickAttributes(pckSku)
            Return pckjb
        Else
            Return Nothing
        End If
    End Function

    Protected Shared Function getPickAttributes(ByVal oSku As SKU) As AttributesCollection
        Dim oAttributeCol As AttributesCollection
        Dim objSkuClass As WMS.Logic.SkuClass = oSku.SKUClass
        If Not objSkuClass Is Nothing Then
            If objSkuClass.CaptureAtPickingLoadAttributesCount > 0 Then
                oAttributeCol = New AttributesCollection
                For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
                    If oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                        oAttributeCol.Add(oAtt.Name, Nothing)
                    End If
                Next
            End If
        End If
        Return oAttributeCol
    End Function

    Private Shared Function ValidatePickJob(ByVal pPickList As Picklist, ByVal pPickJob As PickJob, ByVal pReleaseStrategy As ReleaseStrategyDetail) As Boolean
        If String.IsNullOrEmpty(pPickJob.fromload) Then
            Dim isFullPick As Boolean = False
            If pPickList.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                isFullPick = True
            End If
            If Not pPickList.Lines.AllocateLines(pPickJob, pReleaseStrategy, isFullPick) Then
                For Each i As Integer In pPickJob.PickDetLines
                    Dim oOrder As New OutboundOrderHeader(pPickList.Lines.PicklistLine(i).Consignee, pPickList.Lines.PicklistLine(i).OrderId)
                    If pPickJob.BasedOnPartPickedLine Then
                        pPickList.Lines.PicklistLine(i).Status = WMS.Lib.Statuses.Picklist.PARTPICKED
                        pPickList.Lines.PicklistLine(i).SystemPickShort(pReleaseStrategy.SystemPickShort, pPickList.Lines.PicklistLine(i).AdjustedQuantity, oOrder, Nothing, WMS.Lib.USERS.SYSTEMUSER)
                        pPickList.Lines.PicklistLine(i).Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                        pPickList.Lines.PicklistLine(pPickList.Lines.PicklistLine(i).PickListLine) = pPickList.Lines.PicklistLine(i)
                        'Dim pickLoc As New PickLoc(pPickList.Lines.PicklistLine(i).FromLocation)
                        'pickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pPickJob.uomunits, WMS.Lib.USERS.SYSTEMUSER)
                    Else
                        pPickList.Lines.PicklistLine(i).SystemPickShort(pReleaseStrategy.SystemPickShort, pPickJob.uomunits, oOrder, Nothing, WMS.Lib.USERS.SYSTEMUSER)
                        pPickList.Lines.PicklistLine(i).Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                        If pReleaseStrategy.SystemPickShort.ToLower() <> WMS.Logic.SystemPickShort.PickZeroLeaveOpen.ToLower() AndAlso _
                            pReleaseStrategy.SystemPickShort.ToLower() <> WMS.Logic.SystemPickShort.PickPartialLeaveOpen.ToLower() Then
                            Dim pickLoc As New PickLoc(pPickList.Lines.PicklistLine(i).FromLocation, pPickList.Lines.PicklistLine(i).FromWarehousearea, pPickList.Lines.PicklistLine(i).Consignee, pPickList.Lines.PicklistLine(i).SKU)
                            pickLoc.AdjustAllocationQuantity(WMS.Lib.INVENTORY.SUBQTY, pPickJob.uomunits, WMS.Lib.USERS.SYSTEMUSER)
                        End If
                    End If
                Next
                Return False
            End If
        End If
        Dim oLoad As New Load(pPickJob.fromload)
        If Not oLoad.ValidatePick(pPickJob.units, pPickJob.fromlocation, pPickJob.fromwarehousearea) Then
            Dim pickDet As PicklistDetail
            Dim remainingUnits As Decimal = oLoad.UNITSALLOCATED
            For Each i As Integer In pPickJob.PickDetLines
                'pickDet = pPickList.Lines(i - 1)
                pickDet = pPickList.Lines.PicklistLine(i)
                Dim oOrder As New OutboundOrderHeader(pickDet.Consignee, pickDet.OrderId)
                If pickDet.Quantity >= remainingUnits OrElse oLoad.LOCATION = String.Empty Then
                    If pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCancelException OrElse _
                       pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroCreateException OrElse _
                        pReleaseStrategy.SystemPickShort = SystemPickShort.PickZeroLeaveOpen Then
                        If pPickJob.BasedOnPartPickedLine Then
                            pickDet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED
                        End If
                        pickDet.SystemPickShort(pReleaseStrategy.SystemPickShort, pPickList.Lines.PicklistLine(i).Quantity, oOrder, oLoad, WMS.Lib.USERS.SYSTEMUSER)
                        pickDet.Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                        pPickList.Lines.PicklistLine(pickDet.PickListLine) = pickDet
                        Return False
                    ElseIf oLoad.UNITS = 0 OrElse oLoad.IsLimbo OrElse oLoad.LOCATION = String.Empty _
                                            OrElse pickDet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then
                        If pPickJob.BasedOnPartPickedLine Then
                            pickDet.Status = WMS.Lib.Statuses.Picklist.PARTPICKED
                        End If
                        pickDet.SystemPickShort(pReleaseStrategy.SystemPickShort, pickDet.AdjustedQuantity, oOrder, Nothing, WMS.Lib.USERS.SYSTEMUSER)
                        pickDet.Pick(0, WMS.Lib.USERS.SYSTEMUSER, Nothing, "", True, pReleaseStrategy)
                        pPickList.Lines.PicklistLine(pickDet.PickListLine) = pickDet
                        Return False
                    Else
                        Dim qtyToUnallocate As Decimal
                        If pPickJob.units > oLoad.UNITS Then
                            pPickJob.units = oLoad.UNITS
                        End If
                        If pickDet.AdjustedQuantity - pickDet.PickedQuantity > remainingUnits Then
                            qtyToUnallocate = pickDet.AdjustedQuantity - pickDet.PickedQuantity - remainingUnits
                        End If
                        pPickJob.SystemPickShort = True
                        pickDet.SystemPickShort(pReleaseStrategy.SystemPickShort, qtyToUnallocate, oOrder, oLoad, WMS.Lib.USERS.SYSTEMUSER)
                        If pReleaseStrategy.SystemPickShort <> SystemPickShort.PickPartialLeaveOpen Then
                            If pPickJob.units > oLoad.UNITS Then pickDet.AdjustedQuantity = oLoad.UNITS
                            pPickList.Lines.PicklistLine(pickDet.PickListLine) = pickDet
                        End If
                        Return True
                    End If
                End If
                remainingUnits -= pickDet.AdjustedQuantity
            Next
        End If
        Return True
    End Function

    Private Sub getTaskConfirmation(ByVal pConfType As String, ByVal oPckJob As PickJob)
        Select Case pConfType
            Case WMS.Lib.CONFIRMATIONTYPE.LOAD
                oPckJob.TaskConfirmation = New TaskConfirmationLoad(oPckJob.fromload)
            Case WMS.Lib.CONFIRMATIONTYPE.LOCATION
                oPckJob.TaskConfirmation = New TaskConfirmationLocation(oPckJob.fromlocation, oPckJob.fromwarehousearea)
            Case WMS.Lib.CONFIRMATIONTYPE.NONE
                oPckJob.TaskConfirmation = New TaskConfirmationNone()
            Case WMS.Lib.CONFIRMATIONTYPE.SKU
                oPckJob.TaskConfirmation = New TaskConfirmationSKU(oPckJob.sku)
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATION
                oPckJob.TaskConfirmation = New TaskConfirmationSKULocation(oPckJob.sku, oPckJob.fromlocation, oPckJob.fromwarehousearea)
            Case WMS.Lib.CONFIRMATIONTYPE.SKULOCATIONUOM
                oPckJob.TaskConfirmation = New TaskConfirmationSKULocationUOM(oPckJob.sku, oPckJob.fromlocation, oPckJob.fromwarehousearea, oPckJob.uom)
            Case WMS.Lib.CONFIRMATIONTYPE.SKUUOM
                oPckJob.TaskConfirmation = New TaskConfirmationSKUUOM(oPckJob.sku, oPckJob.uom)
            Case WMS.Lib.CONFIRMATIONTYPE.UPC
                Dim oSKU As New SKU(oPckJob.consingee, oPckJob.sku)
                oPckJob.TaskConfirmation = New TaskConfirmationUPC(oSKU.UOM(oPckJob.uom).EANUPC)
            Case Else 'Using Load Confirmation as default
                oPckJob.TaskConfirmation = New TaskConfirmationLoad(oPckJob.fromload)
        End Select
    End Sub

    Private Sub UpdateTaskFromLocation(ByVal pckJob As PickJob)
        Dim sql As String = String.Format("select task from tasks where parallelpicklist = '{0}'", _parallelpickid)
        Dim strTaskId As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If WMS.Logic.Task.Exists(strTaskId) Then
            Dim oTask As New Task(strTaskId)
            oTask.FROMLOCATION = pckJob.fromlocation
            oTask.FROMWAREHOUSEAREA = pckJob.fromwarehousearea
            If oTask.STARTTIME = DateTime.MinValue Then
                oTask.STARTTIME = DateTime.Now
            End If
            oTask.Save()
        End If
    End Sub

    'Public Function Pick(ByVal Confirmation As String, ByVal WareHoseuAreaConfirmation As String, ByVal pck As PickJob, ByVal pUser As String, Optional ByVal pShoudReturnNextPick As Boolean = True, Optional ByVal SecondConfirmation As String = Nothing, Optional ByVal shouldConfirm As Boolean = True) As PickJob
    Public Function Pick(ByVal pck As PickJob, ByVal pUser As String, ByVal logger As LogHandler, Optional ByVal pShoudReturnNextPick As Boolean = True, Optional ByVal SecondConfirmation As String = Nothing, Optional ByVal shouldConfirm As Boolean = True) As PickJob
        If pck.uomunits < 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Quantity", "Wrong Quantity")
        End If
        'Dim pcklst As ParallelPickList = ParallelPickList(pck.picklist)
        Dim pcklst As Picklist = New Picklist(pck.picklist)
        If pck.uomunits > 0 AndAlso shouldConfirm Then
            'If Not pcklst.Confirmed(Confirmation, WareHoseuAreaConfirmation, pck, SecondConfirmation) Then
            '    Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Confirmation", "Wrong Confirmation")
            'End If
            pck.TaskConfirmation.Confirm()
        End If
        ValidateLineAttributes(pck.consingee, pck.sku, pck.oAttributeCollection)
        setContainer(pUser)
        pcklst.Pick(pck, pUser, logger)
        EvacuateEmptyContainer(pck, pUser)

        Dim oWHActivity As New WHActivity()
        oWHActivity.USERID = pUser
        oWHActivity.WAREHOUSEAREA = pck.fromwarehousearea 'WareHoseuAreaConfirmation
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.LOCATION = pck.fromlocation
        oWHActivity.Post()

        If Not Completed Then
            If pShoudReturnNextPick Then
                Return GetNextPick()
            End If
        Else
            setComplete(pUser)
            Return Nothing
        End If
    End Function

    Protected Sub ValidateLineAttributes(ByVal pConsignee As String, ByVal pSku As String, ByVal oAttributesCollection As AttributesCollection)
        Dim oSku As New SKU(pConsignee, pSku)
        Dim oSkuClass As SkuClass = oSku.SKUClass

        If Not oSkuClass Is Nothing Then
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                Dim typeValidationResult As Int32

                If oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Then
                    If oAttributesCollection Is Nothing Then Continue For
                    Select Case oLoadAtt.Type
                        Case AttributeType.String
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value OrElse oAttributesCollection(oLoadAtt.Name) = "" Then Continue For
                        Case AttributeType.Decimal, AttributeType.Integer
                            Try
                                If Convert.ToString(oAttributesCollection(oLoadAtt.Name)) = "" Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then Continue For
                        Case AttributeType.DateTime
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then Continue For
                            Try
                                If oAttributesCollection(oLoadAtt.Name) = String.Empty Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                            Try
                                If oAttributesCollection(oLoadAtt.Name) = DateTime.MinValue Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                        Case AttributeType.Boolean
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Then Continue For
                    End Select
                ElseIf oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oAttributesCollection Is Nothing Then Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                    If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                        Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                    End If
                End If
                If oLoadAtt.CaptureAtPicking <> SkuClassLoadAttribute.CaptureType.NoCapture Then
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
                    If Not oLoadAtt.PickingValidator Is Nothing Then
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                        vals.Add(oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name))
                        Dim ret As String = oLoadAtt.Evaluate(SkuClassLoadAttribute.EvaluationType.Picking, vals)
                        If ret = "-1" Then
                            Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Private Function ValidateAttributeByType(ByVal oAtt As SkuClassLoadAttribute, ByVal oAttVal As Object) As Int32
        Select Case oAtt.Type
            Case Logic.AttributeType.DateTime
                Dim Val As DateTime
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Then
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
            Case Logic.AttributeType.Decimal
                Dim Val As Decimal
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Then
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
            Case Logic.AttributeType.Integer
                Dim Val As Int32
                Try
                    If oAttVal Is Nothing Then
                        If oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Then
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
            Case Logic.AttributeType.String
                Try
                    If oAttVal Is Nothing Or Convert.ToString(oAttVal) = "" Then
                        If oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Then
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

    Private Sub EvacuateEmptyContainer(ByVal pck As PickJob, ByVal pUser As String)
        Dim dt As DataTable = New DataTable
        Try
            Dim oLoad As New Load(pck.fromload)
            If oLoad.UNITS = 0 Then
                oLoad.EvacuateEmptyContainer(pUser)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function GetConfirmation(ByVal oRelStrat As ReleaseStrategyDetail, ByVal pckJob As PickJob) As String
        Select Case oRelStrat.ConfirmationType
            Case WMS.Lib.Release.CONFIRMATIONTYPE.LOAD
                Return pckJob.fromload
            Case WMS.Lib.Release.CONFIRMATIONTYPE.LOCATION
                Return pckJob.fromlocation
            Case WMS.Lib.Release.CONFIRMATIONTYPE.SKU
                Return pckJob.sku
        End Select
    End Function

    Public Sub setComplete(ByVal pUser As String, Optional ByVal pCompleteTask As Boolean = True)
        If Completed Then
            If pCompleteTask Then
                Dim tm As New TaskManager
                Try
                    tm.getParallelPickTask(_parallelpickid)
                    'Begin for RWMS-1294 and RWMS-1222
                    tm.Complete(Nothing)
                    'End for RWMS-1294 and RWMS-1222
                Catch ex As Exception
                End Try
            End If
            If Canceled Then
                _status = WMS.Lib.Statuses.ParallelPickList.CANCELED
            Else
                _status = WMS.Lib.Statuses.ParallelPickList.COMPLETE
            End If
            Save(pUser)
        End If
    End Sub

    Protected Sub setContainer(ByVal pUser As String)
        If Not _tocontainer Is Nothing And Not _tocontainer = String.Empty Then
            If Not Container.Exists(_tocontainer) Then
                Dim cnt As New Container
                cnt.ContainerId = _tocontainer
                cnt.HandlingUnitType = _handlingunittype
                cnt.UsageType = Container.ContainerUsageType.PickingContainer
                cnt.Post(pUser)
                cnt = Nothing
            End If
        End If
    End Sub

#End Region

End Class

#End Region

#Region "ParallelPickListCollection"

<CLSCompliant(False)> Public Class ParallelPickListCollection
    Implements ICollection

#Region "Variables"
    Protected _pcklst As ArrayList
    Protected _parpicklistid As String
#End Region

#Region "Properties"
    Default Public ReadOnly Property Item(ByVal index As Int32) As ParallelPickList
        Get
            Return _pcklst.Item(index)
        End Get
    End Property

    Public Property PickList(ByVal picklistId As String) As Picklist
        Get
            For Each picklistObj As Picklist In Me
                If picklistObj.PicklistID = picklistId Then
                    Return picklistObj
                End If
            Next
            Return Nothing
        End Get
        Set(ByVal value As Picklist)
            For Each picklistObj As Picklist In Me
                If picklistObj.PicklistID = picklistId Then
                    picklistObj = value
                    Return
                End If
            Next
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New()
        _pcklst = New ArrayList
    End Sub
    Public Sub New(ByVal ParallelPickId As String)
        _pcklst = New ArrayList
        _parpicklistid = ParallelPickId
        Load()
    End Sub
#End Region

#Region "Overrides"

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _pcklst.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _pcklst.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _pcklst.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _pcklst.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _pcklst.GetEnumerator
    End Function

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim sql As String
        Dim dt As New DataTable
        Dim dr As DataRow
        sql = String.Format("Select * from PARALLELPICKDETAIL where ParallelPickId = '{0}' order by PICKLISTSEQ", _parpicklistid)
        dt = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            Dim pcklst As New ParallelPickList(dr)
            Me.Add(pcklst)
        Next
    End Sub

    Public Function Add(ByVal pcklst As ParallelPickList)
        _pcklst.Add(pcklst)
    End Function

#End Region

End Class

#End Region

#Region "ParallelPickList"

<CLSCompliant(False)> Public Class ParallelPickList
    Inherits PickList

#Region "Variables"
    Protected _seq As Int32
#End Region

#Region "Properties"
    Public Property Seq() As Int32
        Get
            Return _seq
        End Get
        Set(ByVal Value As Int32)
            _seq = Value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New(ByVal dr As DataRow)
        MyBase.new(dr("picklist"))
        _seq = dr("picklistseq")
    End Sub

    Public Sub New(ByVal seq As Int32, ByVal pcklst As Picklist)
        MyBase.New(pcklst.PicklistID)
        _seq = seq
    End Sub

#End Region

#Region "Methods"

    Public Shared Function ShouldComplete(ByVal pParallelPickListID As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from pickheader where picklist in (select picklist from parallelpickdetail where parallelpickid={0}) and (status not in ({1},{2}))", _
        Made4Net.Shared.FormatField(pParallelPickListID), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Picklist.COMPLETE), _
        Made4Net.Shared.FormatField(WMS.Lib.Statuses.Picklist.CANCELED))
        Return Not Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

End Class

#End Region