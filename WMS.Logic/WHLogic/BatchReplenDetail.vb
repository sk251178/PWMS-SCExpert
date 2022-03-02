Imports Made4Net.DataAccess

#Region "BatchReplenDetail"

<CLSCompliant(False)> Public Class BatchReplenDetail

#Region "Variables"

#Region "Primary Keys"
    Protected _batchreplid As String = String.Empty
#End Region

#Region "Other Fields"
    Protected _consignee As String = String.Empty
    Protected _replcontainer As String = String.Empty
    Protected _replline As Int32
    Protected _fromsku As String = String.Empty
    Protected _tosku As String = String.Empty
    Protected _fromqty As Decimal
    Protected _toqty As Decimal
    Protected _letdownqty As Decimal
    Protected _unloadqty As Decimal
    Protected _fromskuuom As String = String.Empty
    Protected _toskuuom As String = String.Empty
    Protected _status As String = String.Empty
    Protected _fromlocation As String = String.Empty
    Protected _tolocation As String = String.Empty
    Protected _fromload As String = String.Empty
    Protected _toload As String = String.Empty
    Protected _fromwarehousearea As String = String.Empty
    Protected _towarehousearea As String = String.Empty
    Protected _fromlocationsortorder As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _FROMSKUBASEUOMQTY As Decimal
    Protected _FROMSKUBASEUOM As String = String.Empty
#End Region

#End Region

#Region "Properties"

    Public Property BATCHREPLID() As String
        Get
            Return _batchreplid
        End Get
        Set(ByVal Value As String)
            _batchreplid = Value
        End Set
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property REPLCONTAINER() As String
        Get
            Return _replcontainer
        End Get
        Set(ByVal Value As String)
            _replcontainer = Value
        End Set
    End Property

    Public Property REPLLINE() As Int32
        Get
            Return _replline
        End Get
        Set(ByVal Value As Int32)
            _replline = Value
        End Set
    End Property

    Public Property FROMSKU() As String
        Get
            Return _fromsku
        End Get
        Set(ByVal Value As String)
            _fromsku = Value
        End Set
    End Property

    Public Property TOSKU() As String
        Get
            Return _tosku
        End Get
        Set(ByVal Value As String)
            _tosku = Value
        End Set
    End Property

    Public Property FROMQTY() As Decimal
        Get
            Return _fromqty
        End Get
        Set(ByVal Value As Decimal)
            _fromqty = Value
        End Set
    End Property
    Public Property TOQTY() As Decimal
        Get
            Return _toqty
        End Get
        Set(ByVal Value As Decimal)
            _toqty = Value
        End Set
    End Property

    Public Property LETDOWNQTY() As Decimal
        Get
            Return _letdownqty
        End Get
        Set(ByVal Value As Decimal)
            _letdownqty = Value
        End Set
    End Property

    Public Property UNLOADQTY() As Decimal
        Get
            Return _unloadqty
        End Get
        Set(ByVal Value As Decimal)
            _unloadqty = Value
        End Set
    End Property

    Public Property FROMSKUUOM() As String
        Get
            Return _fromskuuom
        End Get
        Set(ByVal Value As String)
            _fromskuuom = Value
        End Set
    End Property

    Public Property TOSKUUOM() As String
        Get
            Return _toskuuom
        End Get
        Set(ByVal Value As String)
            _toskuuom = Value
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

    Public Property FROMLOCATION() As String
        Get
            Return _fromlocation
        End Get
        Set(ByVal Value As String)
            _fromlocation = Value
        End Set
    End Property

    Public Property TOLOCATION() As String
        Get
            Return _tolocation
        End Get
        Set(ByVal Value As String)
            _tolocation = Value
        End Set
    End Property

    Public Property FROMLOAD() As String
        Get
            Return _fromload
        End Get
        Set(ByVal Value As String)
            _fromload = Value
        End Set
    End Property

    Public Property TOLOAD() As String
        Get
            Return _toload
        End Get
        Set(ByVal Value As String)
            _toload = Value
        End Set
    End Property

    Public Property FROMWAREHOUSEAREA() As String
        Get
            Return _fromwarehousearea
        End Get
        Set(ByVal Value As String)
            _fromwarehousearea = Value
        End Set
    End Property

    Public Property TOWAREHOUSEAREA() As String
        Get
            Return _towarehousearea
        End Get
        Set(ByVal Value As String)
            _towarehousearea = Value
        End Set
    End Property

    Public Property FROMLOCATIONSORTORDER() As String
        Get
            Return _fromlocationsortorder
        End Get
        Set(ByVal Value As String)
            _fromlocationsortorder = Value
        End Set
    End Property

    Public ReadOnly Property SORTORDER() As String
        Get
            Dim so As String = _fromlocationsortorder & _fromsku
            Return so
        End Get
    End Property

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
    Public Property FROMSKUBASEUOM() As String
        Get
            Return _FROMSKUBASEUOM
        End Get
        Set(ByVal Value As String)
            _FROMSKUBASEUOM = Value
        End Set
    End Property
    Public Property FROMSKUBASEUOMQTY() As Decimal
        Get
            Return _FROMSKUBASEUOMQTY
        End Get
        Set(ByVal Value As Decimal)
            _FROMSKUBASEUOMQTY = Value
        End Set
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
    End Sub

    Public Sub New(ByVal batchReplId As String, ByVal replLine As Int32)
        LoadReplenDetail(batchReplId, replLine)
    End Sub

#End Region

#Region "Method"

#Region "Accessors"
    Public Sub Load(ByVal dr As DataRow)
        _batchreplid = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("BATCHREPLID"))
        _consignee = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONSIGNEE"))
        _replcontainer = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REPLCONTAINER"))
        _replline = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("REPLLINE"))
        _fromsku = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMSKU"))
        _tosku = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOSKU"))
        _fromqty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMQTY"), 0)
        _toqty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOQTY"), 0)
        _letdownqty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("LETDOWNQTY"), 0)
        _unloadqty = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("UNLOADQTY"), 0)
        _fromskuuom = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMSKUUOM"))
        _toskuuom = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOSKUUOM"))
        _status = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        _fromlocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMLOCATION"))
        _tolocation = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOLOCATION"))
        _fromload = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMLOAD"))
        _toload = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOLOAD"))
        _fromwarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("FROMWAREHOUSEAREA"))
        _towarehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TOWAREHOUSEAREA"))
        _fromlocationsortorder = String.Empty
        _adddate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDDATE"))
        _adduser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ADDUSER"))
        _editdate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITDATE"))
        _edituser = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("EDITUSER"))
    End Sub

    Private Sub LoadReplenDetail(ByVal batchReplId As String, ByVal replLine As Int32)
        Dim Sql = String.Format("SELECT * FROM BATCHREPLENDETAIL Where BATCHREPLID={0} and REPLLINE={1}",
        Made4Net.Shared.Util.FormatField(batchReplId),
        Made4Net.Shared.Util.FormatField(replLine))
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count = 1 Then
            Load(dt.Rows(0))
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Batch replenishment detail does not exists", "Batch replenishment detail does not exists")
        End If
    End Sub

#End Region

#Region "Update"


    Public Sub UpdateUnloadQuantityAndStatus(ByVal unloadqty As Decimal, ByVal status As String, ByVal user As String)
        Dim Sql = String.Format("UPDATE BATCHREPLENDETAIL SET UNLOADQTY={0}, STATUS={1}, EDITDATE={2}, EDITUSER={3} Where BATCHREPLID={4} and REPLLINE={5} ",
        Made4Net.Shared.Util.FormatField(unloadqty),
        Made4Net.Shared.Util.FormatField(status),
        Made4Net.Shared.Util.FormatField(DateTime.Now),
        Made4Net.Shared.Util.FormatField(user),
        Made4Net.Shared.Util.FormatField(_batchreplid),
        Made4Net.Shared.Util.FormatField(_replline))
        DataInterface.RunSQL(Sql)
    End Sub

    Public Sub UpdateLetdownDetails(ByVal user As String)
        Dim Sql = String.Format("UPDATE BATCHREPLENDETAIL SET LETDOWNQTY={0},TOLOAD={1}, STATUS={2}, EDITDATE={3}, EDITUSER={4} Where BATCHREPLID={5} and REPLLINE={6} ",
        Made4Net.Shared.Util.FormatField(LETDOWNQTY),
        Made4Net.Shared.Util.FormatField(TOLOAD),
        Made4Net.Shared.Util.FormatField(STATUS),
        Made4Net.Shared.Util.FormatField(DateTime.Now),
        Made4Net.Shared.Util.FormatField(user),
        Made4Net.Shared.Util.FormatField(_batchreplid),
        Made4Net.Shared.Util.FormatField(_replline))
        DataInterface.RunSQL(Sql)
    End Sub

    Public Function Clone() As BatchReplenDetail
        Dim retreplen As New BatchReplenDetail
        retreplen.CONSIGNEE = _consignee
        retreplen.REPLCONTAINER = _replcontainer
        retreplen.REPLLINE = _replline
        retreplen.TOSKU = _tosku
        retreplen.TOSKUUOM = _toskuuom
        retreplen.TOQTY = _toqty
        retreplen.FROMSKU = _fromsku
        retreplen.FROMSKUUOM = _fromskuuom
        retreplen.FROMQTY = _fromqty
        retreplen.FROMSKUBASEUOM = _FROMSKUBASEUOM
        retreplen.FROMSKUBASEUOMQTY = _FROMSKUBASEUOMQTY
        retreplen.LETDOWNQTY = _letdownqty
        retreplen.UNLOADQTY = _unloadqty
        retreplen.STATUS = _status
        retreplen.FROMLOCATION = _fromlocation
        retreplen.TOLOCATION = _tolocation
        retreplen.FROMLOAD = _fromload
        retreplen.TOLOAD = _toload
        retreplen.FROMWAREHOUSEAREA = _fromwarehousearea
        retreplen.TOWAREHOUSEAREA = _towarehousearea
        retreplen.FROMLOCATIONSORTORDER = _fromlocationsortorder
        retreplen.ADDDATE = _adddate
        retreplen.ADDUSER = _adduser
        retreplen.EDITDATE = _editdate
        retreplen.EDITUSER = _edituser
        Return retreplen
    End Function
    Public Sub New(ByVal dr As DataRow)
        Load(dr)
    End Sub
    Public Sub Post(ByVal idx As Int32)
        _editdate = DateTime.Now
        _edituser = WMS.Lib.USERS.SYSTEMUSER
        _adddate = DateTime.Now
        _adduser = WMS.Lib.USERS.SYSTEMUSER
        _replline = idx
        _status = WMS.Lib.Statuses.BatchReplensishment.PLANNED

        Dim sql As String = String.Format("INSERT INTO BATCHREPLENDETAIL(BATCHREPLID,CONSIGNEE,REPLCONTAINER,REPLLINE,FROMSKU,TOSKU,FROMQTY,TOQTY," &
            "LETDOWNQTY,UNLOADQTY,FROMSKUUOM,TOSKUUOM,[STATUS],FROMLOCATION,TOLOCATION,FROMLOAD,TOLOAD,FROMWAREHOUSEAREA,TOWAREHOUSEAREA,ADDDATE,ADDUSER,EDITDATE,EDITUSER) values (" &
            "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22})", Made4Net.Shared.Util.FormatField(_batchreplid),
            Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_replcontainer), Made4Net.Shared.Util.FormatField(_replline),
            Made4Net.Shared.Util.FormatField(_fromsku), Made4Net.Shared.Util.FormatField(_tosku), Made4Net.Shared.Util.FormatField(_fromqty), Made4Net.Shared.Util.FormatField(_toqty), Made4Net.Shared.Util.FormatField(_letdownqty), Made4Net.Shared.Util.FormatField(_unloadqty),
            Made4Net.Shared.Util.FormatField(_fromskuuom), Made4Net.Shared.Util.FormatField(_toskuuom), Made4Net.Shared.Util.FormatField(_status),
            Made4Net.Shared.Util.FormatField(_fromlocation), Made4Net.Shared.Util.FormatField(_tolocation),
            Made4Net.Shared.Util.FormatField(_fromload), Made4Net.Shared.Util.FormatField(_toload),
            Made4Net.Shared.Util.FormatField(_fromwarehousearea), Made4Net.Shared.Util.FormatField(_towarehousearea),
            Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser))
        DataInterface.RunSQL(sql)
    End Sub
#End Region

#Region "Message Queuing"
    Public Sub Unload(ByVal unLoadQty)
        If _status = WMS.Lib.Statuses.BatchReplenDetail.LETDOWN Then
            Dim user As String = Common.GetCurrentUser

            ' Unload Detail Tasl
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenLineUnload)
            qh.Values.Add("BATCHREPLID", _batchreplid)
            qh.Values.Add("REPLLINE", _replline)
            qh.Values.Add("UNLOADQTY", unLoadQty)
            qh.Values.Add("USER", user)
            qh.Send("BatchReplenishment", _batchreplid & "_" & _replcontainer & "_" & _replline)

            ' Unload Detail Audit Task
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenLineUnload)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLUNLOAD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _batchreplid)
            aq.Add("DOCUMENTLINE", _replline)
            aq.Add("FROMLOAD", _fromload)
            aq.Add("FROMLOC", _fromlocation)
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", _tosku)
            aq.Add("TOLOAD", _toload)
            aq.Add("TOLOC", _tolocation)
            aq.Add("TOQTY", unLoadQty)
            aq.Add("TOSTATUS", WMS.Lib.Statuses.BatchReplenDetail.UNLOAD)
            aq.Add("USERID", user)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", user)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", user)
            aq.Add("FROMCONTAINER", _replcontainer)
            aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
            aq.Add("TOWAREHOUSEAREA", _towarehousearea)
            aq.Send(WMS.Lib.Actions.Audit.BATCHREPLUNLOAD)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cant Unload Replenishment, incorrect status", "Cant Unload Replenishment, incorrect status")
        End If
    End Sub

    Public Sub LetDown(ByVal letdownQty)
        If _status = WMS.Lib.Statuses.BatchReplenDetail.RELEASED Then
            Dim user As String = Common.GetCurrentUser

            ' Unload Detail Tasl
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenLineLetdown)
            qh.Values.Add("BATCHREPLID", _batchreplid)
            qh.Values.Add("REPLLINE", _replline)
            qh.Values.Add("LETDOWNQTY", letdownQty)
            qh.Values.Add("USER", user)
            qh.Send("BatchReplenishment", _batchreplid & "_" & _replcontainer & "_" & _replline)

            ' Unload Detail Audit Task
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenLineLetdown)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLLETDOWN)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _batchreplid)
            aq.Add("DOCUMENTLINE", _replline)
            aq.Add("FROMLOAD", _fromload)
            aq.Add("FROMLOC", _fromlocation)
            aq.Add("FROMQTY", _fromqty)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", _tosku)
            aq.Add("TOLOAD", _toload)
            aq.Add("TOLOC", _tolocation)
            aq.Add("TOQTY", letdownQty)
            aq.Add("TOSTATUS", WMS.Lib.Statuses.BatchReplenDetail.LETDOWN)
            aq.Add("USERID", user)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", user)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", user)
            aq.Add("FROMCONTAINER", _replcontainer)
            aq.Add("FROMWAREHOUSEAREA", _fromwarehousearea)
            aq.Add("TOWAREHOUSEAREA", _towarehousearea)
            aq.Send(WMS.Lib.Actions.Audit.BATCHREPLLETDOWN)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cant Letdoww Replenishment, incorrect status", "Cant Letdown Replenishment, incorrect status")
        End If
    End Sub
#End Region

#End Region

#End Region

End Class

<CLSCompliant(False)> Public Class BatchReplenDetailCollection
    Implements ICollection

#Region "Variables"
    Dim _batchreplidcollection As ArrayList
    Dim _batchreplid As String
    Protected _currvolume As Double
#End Region

#Region "Constructors"
    Public Sub New()
        _batchreplidcollection = New ArrayList
    End Sub

    Public Sub New(ByVal BatchReplId As String, Optional ByVal status As String = Nothing)
        _batchreplidcollection = New ArrayList
        _batchreplid = BatchReplId
        Load(status)
    End Sub

#End Region

#Region "Properties"
    Default Public Property Item(ByVal index As Int32) As BatchReplenDetail
        Get
            Return CType(_batchreplidcollection(index), BatchReplenDetail)
        End Get
        Set(ByVal Value As BatchReplenDetail)
            _batchreplidcollection(index) = Value
        End Set
    End Property
    Public Property CurrVolume() As String
        Set(ByVal Value As String)
            _currvolume = Value
        End Set
        Get
            Return _currvolume
        End Get
    End Property
#End Region

#Region "Methods"
    Protected Sub Load(ByVal status As String)
        Dim Sql As String = String.Format("SELECT * FROM BATCHREPLENDETAIL WHERE BATCHREPLID='{0}'", _batchreplid)
        If Not status Is Nothing Then
            Sql += String.Format(" AND STATUS='{0}'", status)
        End If
        Sql += " ORDER BY REPLLINE"
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(Sql, dt)
        For Each dr In dt.Rows
            Add(New BatchReplenDetail(dr))
        Next
        dt.Dispose()
    End Sub

    Public Sub UpdateStatus(ByVal status As String, ByVal user As String)
        Dim Sql = String.Format("UPDATE BATCHREPLENDETAIL SET STATUS={0}, EDITDATE={1}, EDITUSER={2} Where BATCHREPLID={3} ",
        Made4Net.Shared.Util.FormatField(status),
        Made4Net.Shared.Util.FormatField(DateTime.Now),
        Made4Net.Shared.Util.FormatField(user),
        Made4Net.Shared.Util.FormatField(_batchreplid))
        DataInterface.RunSQL(Sql)
    End Sub

    Public Sub post(ByVal batchreplens As BatchReplenDetailCollection)
        Me.Add(batchreplens)
        Post()
    End Sub
    Public Function IsAnyBatchReplenLETDOWNStatus() As Boolean
        Dim brdetail As BatchReplenDetail
        For Each brdetail In Me
            If brdetail.STATUS = "LETDOWN" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Sub Post()
        Dim brdetail As BatchReplenDetail
        Dim idx As Int32 = 1
        For Each brdetail In Me
            brdetail.Post(idx)
            idx = idx + 1
        Next
    End Sub

    Public Function Post(ByVal breplenheader As BatchReplenHeader, breplencolleciton As BatchReplenDetailCollection) As BatchReplenHeader
        Dim returnBatchHeader As BatchReplenHeader
        Try
            If Me.Count > 0 Then
                Dim batchreplid As String = Made4Net.Shared.Util.getNextCounter("BATCHREPLID")
                Dim replcontainerid As String = Made4Net.Shared.Util.getNextCounter("CONTAINER")

                breplenheader.BATCHREPLID = batchreplid
                breplenheader.REPLCONTAINER = replcontainerid
                breplenheader.LASTRUNTIME = DateTime.Now.ToString("g")

                Dim brupdatedcollection As New BatchReplenDetailCollection
                'update batchreplid, replcontainerid
                For Each brdetail As BatchReplenDetail In breplencolleciton
                    brdetail.BATCHREPLID = breplenheader.BATCHREPLID
                    brdetail.REPLCONTAINER = breplenheader.REPLCONTAINER
                    brupdatedcollection.Add(brdetail)
                Next
                returnBatchHeader = breplenheader.Post(brupdatedcollection)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return returnBatchHeader
    End Function
    Public Sub PlaceInNewBatchReplen(ByVal Pck As BatchReplenDetail, Optional ByVal oLogger As LogHandler = Nothing)
        Dim pckvol, pckweight As Double
        Try
            pckvol = Inventory.CalculateVolumeForGivenUOM(Pck.CONSIGNEE, Pck.FROMSKU, Pck.FROMSKUBASEUOMQTY, Pck.FROMSKUBASEUOM)
            _currvolume = _currvolume + pckvol
            If Not oLogger Is Nothing Then
                oLogger.Write("Replen line added to current BatchReplen. FromLocation=" + Pck.FROMLOCATION.ToString() + ",FromSKU=" + Pck.FROMSKU.ToString() + ",Fromqty=" + Pck.FROMQTY.ToString() + ",FromLoad=" + Pck.FROMLOAD.ToString() + ",FROMSKUBASEUOMQTY=" + Pck.FROMSKUBASEUOMQTY.ToString() + ",pckVol=" + pckvol.ToString() + ",BatchReplen Total CubeVol=" + _currvolume.ToString())
            End If

            Me.Add(Pck)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Exception found in PlaceInNewBatchReplen(). Error Msg=" + ex.Message.ToString())
            End If
        End Try
    End Sub

#End Region

#Region "Overrides"

    Public Function GetItem(index As Integer) As BatchReplenDetail
        Return CType(_batchreplidcollection(index), BatchReplenDetail)
    End Function

    Public Function Add(ByVal value As BatchReplenDetail) As Integer
        Return _batchreplidcollection.Add(value)
    End Function

    Public Function Add(ByVal value As BatchReplenDetailCollection) As Integer
        Dim brdetail As BatchReplenDetail
        For Each brdetail In value
            Add(brdetail)
        Next
        Return 0
    End Function

    Public Sub Clear()
        _batchreplidcollection.Clear()
    End Sub

    Public Sub Insert(ByVal index As Integer, ByVal value As BatchReplenDetail)
        _batchreplidcollection.Insert(index, value)
    End Sub

    Public Sub Remove(ByVal value As BatchReplenDetail)
        _batchreplidcollection.Remove(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer)
        _batchreplidcollection.RemoveAt(index)
    End Sub

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
            Return _batchreplidcollection.IsSynchronized
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return _batchreplidcollection.SyncRoot
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return _batchreplidcollection.GetEnumerator()
    End Function

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        _batchreplidcollection.CopyTo(array, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return _batchreplidcollection.Count
        End Get
    End Property

    Public Sub Sort(ByVal sortBy As String, ByVal sortDirection As BreplenDetailSortDirection)
        _batchreplidcollection.Sort(New BreplenDetailClassSorter(sortBy, sortDirection))
    End Sub


#End Region

End Class

#Region "Comparer"
<CLSCompliant(False)> Public Class BreplenDetailClassSorter
    Implements IComparer

    Protected _sortBy As String
    Protected _sortDirection As BreplenDetailSortDirection

    Public Sub New(ByVal sortBy As String, ByVal sortDirection As BreplenDetailSortDirection)
        _sortBy = sortBy
        _sortDirection = sortDirection
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object, ByVal Comparer As String) As Integer
        Dim icx, icy As IComparable
        icx = x.GetType().GetProperty(Comparer).GetValue(x, Nothing)
        icy = y.GetType().GetProperty(Comparer).GetValue(y, Nothing)
        If (_sortDirection = BreplenDetailSortDirection.Descending) Then
            Return icy.CompareTo(icx)
        Else
            Return icx.CompareTo(icy)
        End If
    End Function

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Int32 Implements System.Collections.IComparer.Compare
        Return Compare(x, y, _sortBy)
    End Function

End Class
#End Region

Public Enum BreplenDetailSortDirection
    Ascending = 0
    Descending = 1
End Enum