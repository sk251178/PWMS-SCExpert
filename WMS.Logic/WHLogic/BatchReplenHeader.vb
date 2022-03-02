Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared



<CLSCompliant(False)> Public Class BatchReplenHeader

#Region "Variables"
#End Region
#Region "Primary Keys"
    Protected _batchreplid As String = String.Empty
#End Region

#Region "Other Fields"
    Protected _replenpolicy As String = String.Empty
    Protected _status As String = String.Empty
    Protected _pickregion As String = String.Empty
    Protected _replContainer As String = String.Empty
    Protected _targetLocation As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _consignee As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected _lastruntime As DateTime
    Protected _replpolicyid As String
    Protected _batchlabelformat As String
    Protected _breplpolicy As String
    Protected _repltype As String
    Protected _fromlocation As String = String.Empty


#End Region


#Region "Properties"
    Public Property ReplPolicyId() As String
        Get
            Return _replpolicyid
        End Get
        Set(ByVal Value As String)
            _replpolicyid = Value
        End Set
    End Property

    Public Property BATCHREPLTYPE() As String
        Get
            Return _repltype
        End Get
        Set(ByVal Value As String)
            _repltype = Value
        End Set
    End Property

    Public Property BATCHLABELFORMAT() As String
        Get
            Return _batchlabelformat
        End Get
        Set(ByVal Value As String)
            _batchlabelformat = Value
        End Set
    End Property

    Public Property BATCHREPLID() As String
        Get
            Return _batchreplid
        End Get
        Set(ByVal Value As String)
            _batchreplid = Value
        End Set
    End Property

    Public Property REPLCONTAINER() As String
        Get
            Return _replContainer
        End Get
        Set(ByVal Value As String)
            _replContainer = Value
        End Set
    End Property

    Public Property TARGETLOCATION() As String
        Get
            Return _targetLocation
        End Get
        Set(ByVal Value As String)
            _targetLocation = Value
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

    Public Property REPLENPOLICY() As String
        Get
            Return _replenpolicy
        End Get
        Set(ByVal Value As String)
            _replenpolicy = Value
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

    Public Property PICKREGION() As String
        Get
            Return _pickregion
        End Get
        Set(ByVal Value As String)
            _pickregion = Value
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

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
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

    Public Property LASTRUNTIME() As DateTime
        Get
            Return _lastruntime
        End Get
        Set(ByVal Value As DateTime)
            _lastruntime = Value
        End Set
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
    End Sub

    Public Function ShouldPrintBatchLabel() As Boolean
        Dim sql As String = String.Format("Select count(1) FROM REPLPOLICY WHERE LEN(BATCHLABELFORMAT) > 0 AND POLICYID = '{0}'", _replenpolicy)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub New(ByVal pBatchReplid As String, Optional ByVal LoadObj As Boolean = True)
        _batchreplid = pBatchReplid
        If LoadObj Then
            LoadByReplId()
            LoadReplPol()
        End If
    End Sub

    Public Sub New(ByVal LoadObj As Boolean, ByVal pReplContainer As String)
        _replContainer = pReplContainer
        If LoadObj Then
            LoadByReplContainer()
        End If
    End Sub

#End Region

#Region "Methods"
#End Region
#Region "Accessors"
    Protected Sub LoadByReplId()
        Dim SQL As String = String.Format("SELECT * FROM BATCHREPLENHEADER WHERE BATCHREPLID = '{0}'", _batchreplid)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Replenishment does not exists", "Replenishment does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Private Sub LoadReplPol()
        Dim batchreplpolicy = New BatchReplenishmentPolicy
        _batchlabelformat = batchreplpolicy.GetBatchLabelFormat(REPLENPOLICY)
    End Sub

    Protected Sub LoadByReplContainer()
        Dim SQL As String = String.Format("SELECT * FROM BATCHREPLENHEADER WHERE REPLCONTAINER = '{0}'", _replContainer)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Container does not exists", "Container does not exists")
            Throw m4nEx
        End If
        dr = dt.Rows(0)
        Load(dr)
    End Sub

    Private Sub Load(ByVal dr As DataRow)
        If Not dr.IsNull("BATCHREPLID") Then _batchreplid = dr.Item("BATCHREPLID")
        If Not dr.IsNull("REPLENPOLICY") Then _replenpolicy = dr.Item("REPLENPOLICY")
        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("PICKREGION") Then _pickregion = dr.Item("PICKREGION")
        If Not dr.IsNull("REPLCONTAINER") Then _replContainer = dr.Item("REPLCONTAINER")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = dr.Item("CONSIGNEE")
        If Not dr.IsNull("TARGETLOCATION") Then _targetLocation = dr.Item("TARGETLOCATION")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("LASTRUNTIME") Then _lastruntime = dr.Item("LASTRUNTIME")
    End Sub

    Public Shared Function GetTargetLocation(pickRegion As String) As String
        If (String.IsNullOrEmpty(pickRegion)) Then Return ""
        Dim SQL As String = String.Format("select top 1 LOCATION from LOCATION where LOCUSAGETYPE='HANDOFF' and pickregion='{0}' and STATUS=1 and PROBLEMFLAG=0", pickRegion)
        Dim targetLocation As String = DataInterface.ExecuteScalar(SQL)
        If (String.IsNullOrEmpty(targetLocation)) Then Return ""
        Return targetLocation
    End Function
#End Region

#Region "Update"
    Public Sub UpdateStatus(ByVal status As String, ByVal user As String)
        Dim Sql = String.Format("UPDATE BATCHREPLENHEADER SET STATUS={0}, EDITDATE={1}, EDITUSER={2} Where BATCHREPLID={3} ",
        Made4Net.Shared.Util.FormatField(status),
        Made4Net.Shared.Util.FormatField(DateTime.Now),
        Made4Net.Shared.Util.FormatField(user),
        Made4Net.Shared.Util.FormatField(_batchreplid))
        DataInterface.RunSQL(Sql)
    End Sub

    Public Function Post(ByVal brepleCollection As BatchReplenDetailCollection) As BatchReplenHeader
        Dim returnBatchHeader As BatchReplenHeader
        Try
            If brepleCollection.Count > 0 Then
                'Add batch replen hedader 
                _adddate = DateTime.Now
                _editdate = DateTime.Now
                _adduser = WMS.Lib.USERS.SYSTEMUSER
                _edituser = WMS.Lib.USERS.SYSTEMUSER
                _status = WMS.Lib.Statuses.BatchReplensishment.PLANNED
                Dim sql As String = String.Format("INSERT INTO BATCHREPLENHEADER(BATCHREPLID,REPLENPOLICY,[STATUS],PICKREGION,REPLCONTAINER,WAREHOUSEAREA,CONSIGNEE,TARGETLOCATION,ADDDATE,ADDUSER,EDITDATE,EDITUSER,LASTRUNTIME)  values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12})",
                                                Made4Net.Shared.Util.FormatField(_batchreplid), Made4Net.Shared.Util.FormatField(_replenpolicy), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_pickregion), Made4Net.Shared.Util.FormatField(_replContainer),
                                                Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_consignee), Made4Net.Shared.Util.FormatField(_targetLocation), Made4Net.Shared.Util.FormatField(_adddate), Made4Net.Shared.Util.FormatField(_adduser),
                                                Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_lastruntime))

                If Not Container.Exists(_replContainer) Then
                    Dim container As Container = New Container()
                    container.ContainerId = _replContainer
                    container.Post(WMS.Lib.USERS.SYSTEMUSER)
                End If
                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenPlanned)
                aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLPLANNED)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")
                aq.Add("CONSIGNEE", _consignee)
                aq.Add("DOCUMENT", _batchreplid)
                aq.Add("DOCUMENTLINE", 0)
                aq.Add("FROMLOAD", "")
                aq.Add("FROMLOC", "")
                aq.Add("FROMWAREHOUSEAREA", _warehousearea)
                aq.Add("FROMQTY", 0)
                aq.Add("FROMSTATUS", _status)
                aq.Add("NOTES", "")
                aq.Add("SKU", "")
                aq.Add("TOLOAD", "")
                aq.Add("TOLOC", "")
                aq.Add("TOWAREHOUSEAREA", _warehousearea)
                aq.Add("TOQTY", 0)
                aq.Add("TOSTATUS", _status)
                aq.Add("USERID", _edituser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", _edituser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", _edituser)
                aq.Send(WMS.Lib.Actions.Audit.BATCHREPLPLANNED)

                DataInterface.RunSQL(sql)
                returnBatchHeader = New BatchReplenHeader(_batchreplid, True)
                Dim replendetails As New BatchReplenDetailCollection(_batchreplid)
                replendetails.post(brepleCollection)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return returnBatchHeader
    End Function
    Public Sub ReleaseBatchReplen()
        Try
            UpdateStatus(WMS.Lib.Statuses.BatchReplensishment.RELEASED, WMS.Lib.USERS.SYSTEMUSER)
            Dim breplendetails As New BatchReplenDetailCollection(_batchreplid)
            breplendetails.UpdateStatus(WMS.Lib.Statuses.BatchReplensishment.RELEASED, WMS.Lib.USERS.SYSTEMUSER)
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenReleased)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLRELEASED)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _batchreplid)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", _warehousearea)
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", 0)
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", _edituser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", _edituser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", _edituser)
            aq.Send(WMS.Lib.Actions.Audit.BATCHREPLRELEASED)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region


#Region "Message Queuing"
    Public Sub Unload()
        If _status = WMS.Lib.Statuses.BatchReplenHeader.LETDOWN Then
            Dim user As String = Common.GetCurrentUser

            ' Unload Header Task
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenUnload)
            qh.Values.Add("BATCHREPLID", _batchreplid)
            qh.Values.Add("USER", user)
            qh.Send("BatchReplenishment", _consignee & "_" & _batchreplid)

            ' Unload Audit Task
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenUnload)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLUNLOAD)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _batchreplid)
            aq.Add("FROMSTATUS", _status)
            aq.Add("TOSTATUS", WMS.Lib.Statuses.BatchReplenHeader.UNLOAD)
            aq.Add("USERID", user)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", user)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", user)
            aq.Add("FROMCONTAINER", _replContainer)
            aq.Send(WMS.Lib.Actions.Audit.BATCHREPLUNLOAD)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cant Unload Replenishment, incorrect status", "Cant Unload Replenishment, incorrect status")
        End If
    End Sub

    Public Sub Letdown()
        If _status = WMS.Lib.Statuses.BatchReplenHeader.RELEASED Then
            Dim user As String = Common.GetCurrentUser
            Dim qh As New Made4Net.Shared.QMsgSender
            qh.Values.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenLetdown)
            qh.Values.Add("BATCHREPLID", _batchreplid)
            qh.Values.Add("USER", User)
            qh.Send("BatchReplenishment", _consignee & "_" & _batchreplid)

            ' Letdown Audit Task
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.BReplenLetdown)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.BATCHREPLLETDOWN)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", _consignee)
            aq.Add("DOCUMENT", _batchreplid)
            aq.Add("FROMSTATUS", _status)
            aq.Add("TOSTATUS", WMS.Lib.Statuses.BatchReplenHeader.LETDOWN)
            aq.Add("USERID", User)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", User)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", User)
            aq.Add("FROMCONTAINER", _replContainer)
            aq.Send(WMS.Lib.Actions.Audit.BATCHREPLLETDOWN)
        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Cant Letdown Replenishment, incorrect status", "Cant Letdown Replenishment, incorrect status")
        End If
    End Sub

    Public Sub PrintBatchLabels(ByVal lblPrinter As String)
        Dim lbltype As String
        Try
            lbltype = _batchlabelformat
        Catch ex As Exception

        End Try
        If Not lbltype Is Nothing Then
            PrintBatchLabels(lbltype, lblPrinter)
        End If
    End Sub
    Public Sub UpdateToLoadTargetLocationofBatchReplen()
        Dim sql As String

        Dim breplendetails As New BatchReplenDetailCollection(_batchreplid)

        Dim brd As New WMS.Logic.BatchReplenDetail

        For Each brd In breplendetails
            Try
                Dim toLoad As New WMS.Logic.Load(brd.TOLOAD)
                toLoad.LOCATION = _targetLocation
                toLoad.UpdateLocation()
            Catch ex As Exception

            End Try
        Next
    End Sub
    Public Sub PrintBatchLabels(ByVal LabelType As String, ByVal lblPrinter As String)

        If LabelType Is Nothing OrElse LabelType = "" Then
            Throw New M4NException(New Exception(), "'" + LabelType + "'Batch Label Not Configured.", "'" + LabelType + "'Batch Label Not Configured.")
        Else
            If lblPrinter Is Nothing Then
                lblPrinter = ""
            End If

            Dim qSender As IQMsgSender = QMsgSender.Factory.Create()
            qSender.Add("LABELNAME", LabelType)
            qSender.Add("LabelType", LabelType)
            qSender.Add("PRINTER", lblPrinter)
            qSender.Add("BATCHREPLID", _batchreplid)
            Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
            ht.Hash.Add("BATCHREPLID", _batchreplid)
            qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
            qSender.Send("Label", "Batch Replen Label")
        End If

    End Sub

#End Region





End Class