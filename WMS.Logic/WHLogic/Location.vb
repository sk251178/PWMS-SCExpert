Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion
Imports System.Collections.Generic
Imports System.Linq
Imports Made4Net.Algorithms.DTO
Imports Made4Net.Algorithms.Scoring
Imports System.Reflection

#Region "Location"

' <summary>
' This object represents the properties and methods of a Location.
' </summary>

<CLSCompliant(False)> Public Class Location

#Region "Variables"

#Region "Primary Keys"

    Protected _Location As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _status As Boolean
    Protected _putregion As String = String.Empty
    Protected _pickregion As String = String.Empty
    Protected _warehousearea As String = String.Empty
    Protected _locsortorder As String = String.Empty
    Protected _loctpickype As String = String.Empty
    Protected _locstoragetype As String = String.Empty
    Protected _locusagetype As String = String.Empty
    Protected _locmhtype As String = String.Empty
    Protected _inventory As Boolean
    Protected _lastcountdate As DateTime
    Protected _accesstype As String = String.Empty
    Protected _accessibleloads As Int32
    Protected _length As Double
    Protected _width As Double
    Protected _height As Double
    Protected _weight As Double
    Protected _cubic As Double
    Protected _loadscapacity As Int32
    Protected _pendingweight As Double
    Protected _pendingcubic As Double
    Protected _pendingloads As Double
    Protected _checkdigits As String = String.Empty
    Protected _looseid As Boolean
    Protected _aisle As String = String.Empty
    Protected _bay As String = String.Empty
    Protected _loclevel As String = String.Empty
    Protected _heightfromfloor As Double
    Protected _lastmovein As DateTime
    Protected _lastmoveout As DateTime
    Protected _picksfromlastcount As Int32
    Protected _xcoordinate As Int32
    Protected _ycoordinate As Int32
    Protected _zcoordinate As Int32
    Protected _inhandoff As String = String.Empty
    Protected _outhandoff As String = String.Empty
    Protected _locaccessibility As String = String.Empty
    Protected _laborhufacing As String = String.Empty
    Protected _laborinserttype As String = String.Empty
    Protected _laborretrievetype As String = String.Empty
    Protected _laborpicktype As String = String.Empty
    Protected _laborreachtype As String = String.Empty
    Protected _congestionregion As String = String.Empty
    Protected _problemflag As Boolean
    'Protected _zpicking As String = String.Empty
    Protected _problemflagrc As String
    Protected _zpickinglocation As String = String.Empty
    Protected _zpickingwarehousearea As String = String.Empty
    Protected _hustoragetemplate As String = String.Empty
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
    Protected Shared _containerHeight As Double = -1 ' RWMS-1200

    'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
    Protected _xfillcordinate As Int32
    Protected _yfillcordinate As Int32
    Protected _zfillcordinate As Int32
    Protected _pickedge As Int32
    Protected _filledge As Int32
    'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where Location = '" & _Location & "' and Warehousearea = '" & _warehousearea & "'"
        End Get
    End Property

    Public Property Location() As String
        Get
            Return _Location
        End Get
        Set(ByVal Value As String)
            _Location = Value
        End Set
    End Property

    Public Property STATUS() As Boolean
        Get
            Return _status
        End Get
        Set(ByVal Value As Boolean)
            _status = Value
        End Set
    End Property

    Public Property PROBLEMFLAG() As Boolean
        Get
            Return _problemflag
        End Get
        Set(ByVal Value As Boolean)
            _problemflag = Value
        End Set
    End Property

    Public Property PUTREGION() As String
        Get
            Return _putregion
        End Get
        Set(ByVal Value As String)
            _putregion = Value
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

    Public Property LOCSORTORDER() As String
        Get
            Return _locsortorder
        End Get
        Set(ByVal Value As String)
            _locsortorder = Value
        End Set
    End Property

    Public Property LOCTPICKYPE() As String
        Get
            Return _loctpickype
        End Get
        Set(ByVal Value As String)
            _loctpickype = Value
        End Set
    End Property

    Public Property LOCSTORAGETYPE() As String
        Get
            Return _locstoragetype
        End Get
        Set(ByVal Value As String)
            _locstoragetype = Value
        End Set
    End Property

    Public Property LOCUSAGETYPE() As String
        Get
            Return _locusagetype
        End Get
        Set(ByVal Value As String)
            _locusagetype = Value
        End Set
    End Property

    Public Property LOCMHTYPE() As String
        Get
            Return _locmhtype
        End Get
        Set(ByVal Value As String)
            _locmhtype = Value
        End Set
    End Property

    Public Property INVENTORY() As Boolean
        Get
            Return _inventory
        End Get
        Set(ByVal Value As Boolean)
            _inventory = Value
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

    Public Property ACCESSTYPE() As String
        Get
            Return _accesstype
        End Get
        Set(ByVal Value As String)
            _accesstype = Value
        End Set
    End Property

    Public Property ACCESSIBLELOADS() As Int32
        Get
            Return _accessibleloads
        End Get
        Set(ByVal Value As Int32)
            _accessibleloads = Value
        End Set
    End Property

    Public Property LENGTH() As Double
        Get
            Return _length
        End Get
        Set(ByVal Value As Double)
            _length = Value
        End Set
    End Property

    Public Property WIDTH() As Double
        Get
            Return _width
        End Get
        Set(ByVal Value As Double)
            _width = Value
        End Set
    End Property

    Public Property HEIGHT() As Double
        Get
            Return _height
        End Get
        Set(ByVal Value As Double)
            _height = Value
        End Set
    End Property

    Public Property WEIGHT() As Double
        Get
            Return _weight
        End Get
        Set(ByVal Value As Double)
            _weight = Value
        End Set
    End Property

    Public Property CUBIC() As Double
        Get
            Return _cubic
        End Get
        Set(ByVal Value As Double)
            _cubic = Value
        End Set
    End Property

    Public Property LOADSCAPACITY() As Int32
        Get
            Return _loadscapacity
        End Get
        Set(ByVal Value As Int32)
            _loadscapacity = Value
        End Set
    End Property

    Public Property PENDINGWEIGHT() As Double
        Get
            Return _pendingweight
        End Get
        Set(ByVal Value As Double)
            _pendingweight = Value
        End Set
    End Property

    Public Property PENDINGCUBIC() As Double
        Get
            Return _pendingcubic
        End Get
        Set(ByVal Value As Double)
            _pendingcubic = Value
        End Set
    End Property

    Public Property PENDINGLOADS() As Double
        Get
            Return _pendingloads
        End Get
        Set(ByVal Value As Double)
            _pendingloads = Value
        End Set
    End Property

    Public Property CHECKDIGITS() As String
        Get
            Return _checkdigits
        End Get
        Set(ByVal Value As String)
            _checkdigits = Value
        End Set
    End Property

    Public Property LOOSEID() As Boolean
        Get
            Return _looseid
        End Get
        Set(ByVal Value As Boolean)
            _looseid = Value
        End Set
    End Property

    Public Property AISLE() As String
        Get
            Return _aisle
        End Get
        Set(ByVal Value As String)
            _aisle = Value
        End Set
    End Property

    Public Property BAY() As String
        Get
            Return _bay
        End Get
        Set(ByVal Value As String)
            _bay = Value
        End Set
    End Property

    Public Property LOCLEVEL() As String
        Get
            Return _loclevel
        End Get
        Set(ByVal Value As String)
            _loclevel = Value
        End Set
    End Property

    Public Property HEIGHTFROMFLOOR() As Double
        Get
            Return _heightfromfloor
        End Get
        Set(ByVal Value As Double)
            _heightfromfloor = Value
        End Set
    End Property

    Public Property LASTMOVEIN() As DateTime
        Get
            Return _lastmovein
        End Get
        Set(ByVal Value As DateTime)
            _lastmovein = Value
        End Set
    End Property

    Public Property LASTMOVEOUT() As DateTime
        Get
            Return _lastmoveout
        End Get
        Set(ByVal Value As DateTime)
            _lastmoveout = Value
        End Set
    End Property

    Public Property PICKSFROMLASTCOUNT() As Int32
        Get
            Return _picksfromlastcount
        End Get
        Set(ByVal Value As Int32)
            _picksfromlastcount = Value
        End Set
    End Property

    Public Property XCOORDINATE() As Int32
        Get
            Return _xcoordinate
        End Get
        Set(ByVal Value As Int32)
            _xcoordinate = Value
        End Set
    End Property

    Public Property YCOORDINATE() As Int32
        Get
            Return _ycoordinate
        End Get
        Set(ByVal Value As Int32)
            _ycoordinate = Value
        End Set
    End Property

    Public Property ZCOORDINATE() As Int32
        Get
            Return _zcoordinate
        End Get
        Set(ByVal Value As Int32)
            _zcoordinate = Value
        End Set
    End Property

    Public Property INHANDOFF() As String
        Get
            Return _inhandoff
        End Get
        Set(ByVal Value As String)
            _inhandoff = Value
        End Set
    End Property

    Public Property OUTHANDOFF() As String
        Get
            Return _outhandoff
        End Get
        Set(ByVal Value As String)
            _outhandoff = Value
        End Set
    End Property

    Public Property LOCACCESSIBILITY() As String
        Get
            Return _locaccessibility
        End Get
        Set(ByVal Value As String)
            _locaccessibility = Value
        End Set
    End Property

    Public Property LABORHUFACING() As Boolean
        Get
            Return _laborhufacing
        End Get
        Set(ByVal Value As Boolean)
            _laborhufacing = Value
        End Set
    End Property

    Public Property LABORINSERTTYPE() As Boolean
        Get
            Return _laborinserttype
        End Get
        Set(ByVal Value As Boolean)
            _laborinserttype = Value
        End Set
    End Property

    Public Property LABORRETRIEVETYPE() As String
        Get
            Return _laborretrievetype
        End Get
        Set(ByVal Value As String)
            _laborretrievetype = Value
        End Set
    End Property

    Public Property LABORPICKTYPE() As String
        Get
            Return _laborpicktype
        End Get
        Set(ByVal Value As String)
            _laborpicktype = Value
        End Set
    End Property

    Public Property LABORREACHTYPE() As String
        Get
            Return _laborreachtype
        End Get
        Set(ByVal Value As String)
            _laborreachtype = Value
        End Set
    End Property

    Public Property CONGESTIONREGION() As String
        Get
            Return _congestionregion
        End Get
        Set(ByVal Value As String)
            _congestionregion = Value
        End Set
    End Property

    Public Property ZPICKINGLOCATION() As String
        Get
            Return _zpickinglocation
        End Get
        Set(ByVal Value As String)
            _zpickinglocation = Value
        End Set
    End Property

    Public Property ZPICKINGWAREHOUSEAREA()
        Get
            Return _zpickingwarehousearea
        End Get
        Set(ByVal value)
            _zpickingwarehousearea = value
        End Set
    End Property

    Public Property HUSTORAGETEMPLATE() As String
        Get
            Return _hustoragetemplate
        End Get
        Set(ByVal Value As String)
            _hustoragetemplate = Value
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

    Public Property PROBLEMFLAGRC() As String
        Get
            Return _problemflagrc
        End Get
        Set(ByVal Value As String)
            _problemflagrc = Value
        End Set
    End Property
    'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
    Public Property XFILLCORDINATE() As Int32
        Get
            Return _xfillcordinate
        End Get
        Set(ByVal Value As Int32)
            _xfillcordinate = Value
        End Set
    End Property

    Public Property YFILLCORDINATE() As Int32
        Get
            Return _yfillcordinate
        End Get
        Set(ByVal Value As Int32)
            _yfillcordinate = Value
        End Set
    End Property

    Public Property ZFILLCORDINATE() As Int32
        Get
            Return _zfillcordinate
        End Get
        Set(ByVal Value As Int32)
            _zfillcordinate = Value
        End Set
    End Property
    Public Property PICKEDGE() As Int32
        Get
            Return _pickedge
        End Get
        Set(ByVal Value As Int32)
            _pickedge = Value
        End Set
    End Property
    Public Property FILLEDGE() As Int32
        Get
            Return _filledge
        End Get
        Set(ByVal Value As Int32)
            _filledge = Value
        End Set
    End Property
    'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203

    Public ReadOnly Property IsPickLocation() As Boolean
        Get
            Dim sql As String = String.Format("Select count(1) from pickloc where location='{0}' and warehousearea='{1}'", _Location, _warehousearea)
            Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
        End Get
    End Property
    '' Start RWMS-1200

    Public Shared ReadOnly Property ContainerHeight() As Double
        Get
            If _containerHeight < 0 Then
                _containerHeight = DataInterface.ExecuteScalar("SELECT SUM(ISNULL(HEIGHT,0)) FROM HANDELINGUNITTYPE WHERE CONTAINER=(SELECT PARAMVALUE FROM WAREHOUSEPARAMS WHERE PARAMNAME='HANDLINGUNITTYPE')")
            End If
            Return _containerHeight
        End Get
    End Property
    '' End RWMS-1200

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim t As New Translation.Translator()
        Dim Location As String
        Dim dt As DataTable
        ' Dim vals As List(Of String) = New List(Of String)
        ' vals.Add(Loc)
        'Dim LocationList As String = String.Join(",", vals)
        dt = ds.Tables(0)
        'Dim dr As DataRow = ds.Tables(0).Rows(0)
        Select Case CommandName.ToLower
            Case "printlabels"
                'Added for RWMS-2047 and RWMS-1637
                If Not Made4Net.Label.LabelHandler.Factory.GetNewLableHandler().ValidateLabel("LOCATION") Then
                    Throw New M4NException(New Exception(), "LOCATION Label Not Configured.", "LOCATION Label Not Configured.")
                Else
                    For Each dr As DataRow In dt.Rows
                        Dim qSender As New QMsgSender
                        Location = dr("LOCATION")
                        qSender.Add("LABELNAME", "LOCATION")
                        qSender.Add("PRINTER", "")
                        qSender.Add("LOCATION", Location)
                        qSender.Add("WAREHOUSEAREA", ds.Tables(0).Rows(0)("WAREHOUSEAREA"))
                        'Start RWMS-2452 RWMS-1323
                        qSender.Add("LabelType", "LOCATION")
                        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
                        ht.Hash.Add("LOCATION", ds.Tables(0).Rows(0)("LOCATION"))
                        qSender.Add("LabelDataParameters", Common.GetHashTableXMLString(ht))
                        'End RWMS-2452 RWMS-1323
                        qSender.Send("Label", "Location Label")
                    Next
                End If
                'Ended for RWMS-2047 and RWMS-1637

                'For Each dr As DataRow In dt.Rows
                '    Dim qSender As New QMsgSender
                '    Location = dr("LOCATION")
                '    qSender.Add("LABELNAME", "LOCATION")
                '    qSender.Add("PRINTER", "")
                '    qSender.Add("LOCATION", Location)
                '    qSender.Add("WAREHOUSEAREA", ds.Tables(0).Rows(0)("WAREHOUSEAREA"))
                '    qSender.Send("Label", "Location Label")
                'Next

            Case "delete"
                Dim oLoc As New Location(ds.Tables(0).Rows(0)("LOCATION"), ds.Tables(0).Rows(0)("WAREHOUSEAREA"))

                'Commented for RWMS-2510 Start
                'oLoc.DeleteLocation()
                'Commented for RWMS-2510 End


                'Added for RWMS-2510 Start
                If Not CheckCanDelete(ds.Tables(0).Rows(0)("LOCATION"), Message) Then
                    Throw New ApplicationException(t.Translate(Message))
                Else
                    oLoc.DeleteLocation()
                    DeletePickLocation(ds.Tables(0).Rows(0)("LOCATION"), ds.Tables(0).Rows(0)("WAREHOUSEAREA"))

                    Dim SQL As String = String.Format("delete from tasks where tasktype in ('LOCCNT','LDCOUNT') and status not in ('COMPLETE','CANCELED') and FROMLOCATION='{0}'", ds.Tables(0).Rows(0)("LOCATION"))
                    DataInterface.RunSQL(SQL)
                    Dim aq As EventManagerQ = New EventManagerQ
                    Dim activitystatus As String
                    Dim UserId As String = WMS.Logic.Common.GetCurrentUser()

                    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LocationDeleted)
                    activitystatus = WMS.Lib.Actions.Audit.LOCATIONDEL
                    aq.Add("ACTIVITYTYPE", activitystatus)
                    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                    aq.Add("ACTIVITYTIME", "0")
                    aq.Add("CONSIGNEE", "")
                    aq.Add("DOCUMENT", "")
                    aq.Add("DOCUMENTLINE", 0)
                    aq.Add("FROMLOAD", "")

                    aq.Add("FROMLOC", ds.Tables(0).Rows(0)("LOCATION"))
                    aq.Add("FROMWAREHOUSEAREA", ds.Tables(0).Rows(0)("WAREHOUSEAREA"))

                    aq.Add("FROMQTY", 0)
                    aq.Add("FROMSTATUS", "")
                    aq.Add("NOTES", "")
                    aq.Add("SKU", "")
                    aq.Add("TOLOAD", "")

                    aq.Add("TOLOC", ds.Tables(0).Rows(0)("LOCATION"))
                    aq.Add("TOWAREHOUSEAREA", ds.Tables(0).Rows(0)("WAREHOUSEAREA"))

                    aq.Add("TOQTY", "")
                    aq.Add("TOSTATUS", "")
                    aq.Add("USERID", UserId)
                    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                    aq.Add("ADDUSER", UserId)
                    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                    aq.Add("EDITUSER", UserId)
                    aq.Send(activitystatus)

                End If
                'Added for RWMS-2510 End


            Case "createlocation"
                'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
                Dim oLoc As New Location()

                If Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xfillcordinate")) Is Nothing Then
                    ds.Tables(0).Rows(0)("xfillcordinate") = ds.Tables(0).Rows(0)("xcoordinate")
                End If

                If Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("yfillcordinate")) Is Nothing Then
                    ds.Tables(0).Rows(0)("yfillcordinate") = ds.Tables(0).Rows(0)("ycoordinate")
                End If

                If Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zfillcordinate")) Is Nothing Then
                    ds.Tables(0).Rows(0)("zfillcordinate") = ds.Tables(0).Rows(0)("zcoordinate")
                End If

                oLoc.Create(Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("LOCATION")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("STATUS")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("putregion")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("pickregion")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("warehousearea")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locsortorder")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("loctpickype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locstoragetype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locusagetype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locmhtype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("inventory")), Nothing,
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("accesstype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("accessibleloads")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("length")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("width")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("height")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("weight")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("cubic")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("loadscapacity")),
                Nothing, Nothing,
                Nothing, Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("checkdigits")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("looseid")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("aisle")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Bay")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("loclevel")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("heightfromfloor")), Nothing,
                Nothing, Nothing,
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xcoordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("ycoordinate")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zcoordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("inhandoff")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("outhandoff")), Convert.ReplaceDBNull(DateTime.Now), Common.GetCurrentUser(),
                Convert.ReplaceDBNull(DateTime.Now), Common.GetCurrentUser(), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locaccessibility")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborhufacing")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborinserttype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborretrievetype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborpicktype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborreachtype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("congestionregion")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("problemflag")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("hustoragetemplate")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zpickinglocation")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zpickingwarehousearea")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xfillcordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("yfillcordinate")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zfillcordinate")), getEdge(System.Convert.ToInt32(Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xcoordinate"))), System.Convert.ToInt32(Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("ycoordinate")))),
                getEdge(System.Convert.ToInt32(Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xfillcordinate"))), System.Convert.ToInt32(Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("yfillcordinate")))))

                'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203

            Case "updatelocation"
                'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
                Dim oLoc As New Location(ds.Tables(0).Rows(0)("LOCATION"), ds.Tables(0).Rows(0)("WAREHOUSEAREA"))
                If Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xfillcordinate")) Is Nothing Then
                    ds.Tables(0).Rows(0)("xfillcordinate") = ds.Tables(0).Rows(0)("xcoordinate")
                End If

                If Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("yfillcordinate")) Is Nothing Then
                    ds.Tables(0).Rows(0)("yfillcordinate") = ds.Tables(0).Rows(0)("ycoordinate")
                End If

                If Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zfillcordinate")) Is Nothing Then
                    ds.Tables(0).Rows(0)("zfillcordinate") = ds.Tables(0).Rows(0)("zcoordinate")
                End If
                oLoc.Update(Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("STATUS")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("putregion")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("pickregion")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locsortorder")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("loctpickype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locstoragetype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locusagetype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locmhtype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("inventory")), Convert.ReplaceDBNull(oLoc._lastcountdate),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("accesstype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("accessibleloads")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("length")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("width")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("height")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("weight")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("cubic")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("loadscapacity")),
                Convert.ReplaceDBNull(oLoc._pendingweight), Convert.ReplaceDBNull(oLoc._pendingcubic),
                Convert.ReplaceDBNull(oLoc._pendingloads), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("checkdigits")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("looseid")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("aisle")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("Bay")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("loclevel")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("heightfromfloor")), Convert.ReplaceDBNull(oLoc._lastmovein),
                Convert.ReplaceDBNull(oLoc._lastmoveout), Convert.ReplaceDBNull(oLoc._picksfromlastcount),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xcoordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("ycoordinate")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zcoordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("inhandoff")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("outhandoff")), Convert.ReplaceDBNull(oLoc._adddate),
                Convert.ReplaceDBNull(oLoc._adduser), Convert.ReplaceDBNull(DateTime.Now), Common.GetCurrentUser(),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("locaccessibility")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborhufacing")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborinserttype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborretrievetype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborpicktype")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("laborreachtype")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("congestionregion")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("problemflag")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("hustoragetemplate")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zpickinglocation")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zpickingwarehousearea")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xfillcordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("yfillcordinate")),
                Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zfillcordinate")),
                getEdge(System.Convert.ToInt32(ds.Tables(0).Rows(0)("xcoordinate")), System.Convert.ToInt32(ds.Tables(0).Rows(0)("ycoordinate"))),
                getEdge(System.Convert.ToInt32(ds.Tables(0).Rows(0)("xfillcordinate")), System.Convert.ToInt32(ds.Tables(0).Rows(0)("yfillcordinate"))))
                'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203

            Case "multieditlocation"
                'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oLoc As New Location(dr("LOCATION"), dr("WAREHOUSEAREA"))
                    oLoc.Update(Convert.ReplaceDBNull(dr("STATUS")), Convert.ReplaceDBNull(dr("putregion")),
                    Convert.ReplaceDBNull(dr("pickregion")), Convert.ReplaceDBNull(dr("locsortorder")),
                    Convert.ReplaceDBNull(dr("loctpickype")), Convert.ReplaceDBNull(dr("locstoragetype")),
                    Convert.ReplaceDBNull(dr("locusagetype")), Convert.ReplaceDBNull(dr("locmhtype")),
                    Convert.ReplaceDBNull(dr("inventory")), Convert.ReplaceDBNull(oLoc._lastcountdate),
                    Convert.ReplaceDBNull(dr("accesstype")), Convert.ReplaceDBNull(dr("accessibleloads")),
                    Convert.ReplaceDBNull(dr("length")), Convert.ReplaceDBNull(dr("width")),
                    Convert.ReplaceDBNull(dr("height")), Convert.ReplaceDBNull(dr("weight")),
                    Convert.ReplaceDBNull(dr("cubic")), Convert.ReplaceDBNull(dr("loadscapacity")),
                    Convert.ReplaceDBNull(oLoc._pendingweight), Convert.ReplaceDBNull(oLoc._pendingcubic),
                    Convert.ReplaceDBNull(oLoc._pendingloads), Convert.ReplaceDBNull(dr("checkdigits")),
                    Convert.ReplaceDBNull(dr("looseid")), Convert.ReplaceDBNull(dr("aisle")),
                    Convert.ReplaceDBNull(dr("Bay")), Convert.ReplaceDBNull(dr("loclevel")),
                    Convert.ReplaceDBNull(dr("heightfromfloor")), Convert.ReplaceDBNull(oLoc._lastmovein),
                    Convert.ReplaceDBNull(oLoc._lastmoveout), Convert.ReplaceDBNull(oLoc._picksfromlastcount),
                    Convert.ReplaceDBNull(dr("xcoordinate")), Convert.ReplaceDBNull(dr("ycoordinate")),
                    Convert.ReplaceDBNull(dr("zcoordinate")), Convert.ReplaceDBNull(dr("inhandoff")),
                    Convert.ReplaceDBNull(dr("outhandoff")), Convert.ReplaceDBNull(oLoc._adddate),
                    Convert.ReplaceDBNull(oLoc._adduser), Convert.ReplaceDBNull(DateTime.Now), Common.GetCurrentUser(),
                    Convert.ReplaceDBNull(dr("locaccessibility")),
                    Convert.ReplaceDBNull(dr("laborhufacing")), Convert.ReplaceDBNull(dr("laborinserttype")),
                    Convert.ReplaceDBNull(dr("laborretrievetype")), Convert.ReplaceDBNull(dr("laborpicktype")),
                    Convert.ReplaceDBNull(dr("laborreachtype")), Convert.ReplaceDBNull(dr("congestionregion")),
                    Convert.ReplaceDBNull(dr("problemflag")), Convert.ReplaceDBNull(dr("hustoragetemplate")),
                    Convert.ReplaceDBNull(dr("zpickinglocation")), Convert.ReplaceDBNull(dr("zpickingwarehousearea")),
                    Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("xfillcordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("yfillcordinate")),
                    Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("zfillcordinate")), Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("pickedge")),
                    Convert.ReplaceDBNull(ds.Tables(0).Rows(0)("filledge")))
                    'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
                Next
            Case "calculateedges"
                Dim locationsToCalculateEdges As DataRow() = ds.Tables(0).Select()
                Dim locationEdgeCalc = New Made4Net.Algorithms.EdgeCalculation.LocationsEdgeCalculation()
                ' GetInstancePath is used in case of Base code. When Retrofitting to other branch use other machanism to create path like GetApplicationLogDirectory
                Dim instancePath As String = Made4Net.DataAccess.GetInstancePath()
                ' GetInstancePath is used in case of Base code. When Retrofitting to other branch use other machanism to create path like GetApplicationLogDirectory
                Dim fileName As String = "EdgeCalculation_" & New Random().Next
                If Not String.IsNullOrEmpty(instancePath) Then
                    instancePath = instancePath & "\Logs\EdgeCalculation\"
                Else
                    Try
                        'RWMS-2456
                        instancePath = WMS.Logic.GetSysParam("ApplicationLogDirectory")
                        instancePath = Made4Net.DataAccess.Util.BuildAndGetFilePath(instancePath)
                        instancePath = instancePath & "\EdgeCalculation\"
                    Catch ex As SysParamNotFoundException
                        instancePath = ""
                    End Try
                End If
                ' If no of locations to be updated is greater than 500, do it asynchronously
                If locationsToCalculateEdges.Length > 500 Then
                    Message = t.Translate("EdgeCalculationStarted")
                    ' Asynchronous
                    UpdateEdgesOnLocationsAsync(instancePath, fileName, locationsToCalculateEdges, locationEdgeCalc)
                Else
                    ' Synchronous
                    Dim res As String = UpdateEdgesOnLocations(instancePath, fileName, locationsToCalculateEdges, locationEdgeCalc)
                    If Not String.IsNullOrEmpty(res) Then
                        Message = res
                        Throw New M4NException(New Exception(res), t.Translate("EdgeCalculationError"), res)
                    Else
                        Message = t.Translate("EdgeCalculationDone")
                    End If
                End If
        End Select
    End Sub

    Protected Function getEdge(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim logger As Logging.LogFile
        Dim edge As Integer = Integer.MinValue
        Try
            logger = getEdgeLogFile()
            Dim EdgeCalc = New Made4Net.Algorithms.EdgeCalculation.LocationsEdgeCalculation()
            Dim calcMethod As MethodInfo = EdgeCalc.GetType().GetMethod("FindNearestEdgeIdFromCoordinates", BindingFlags.NonPublic Or BindingFlags.Instance)
            edge = calcMethod.Invoke(EdgeCalc, New Object() {x, y, logger})
        Catch ex As Exception
            If Not logger Is Nothing Then
                logger.WriteLine("Error while calculating Edges " + ex.ToString())
            End If
        Finally
            If Not logger Is Nothing Then
                logger.Dispose()
            End If
        End Try
        Return edge
    End Function

    Protected Function getEdgeLogFile() As Logging.LogFile
        Try
            Dim instancepath = GetInstancePath()
            Dim logPath = System.IO.Path.Combine(instancepath, "Logs\EdgeCalculation")
            Return New Logging.LogFile(logPath, "EdgeCalculation_" & New Random().Next)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function UpdateEdgesOnLocations(ByVal instancePath As String, ByVal fileName As String, ByVal locationsToCalculateEdges As DataRow(), ByVal locationEdgeCalc As Made4Net.Algorithms.EdgeCalculation.LocationsEdgeCalculation) As String
        Dim Logger As Made4Net.Shared.Logging.LogFile
        Dim strMessage As String = ""
        Try
            If Not String.IsNullOrEmpty(instancePath) Then
                Logger = New Made4Net.Shared.Logging.LogFile(instancePath, fileName)
                locationEdgeCalc.UpdateEdgeIdForLocations(locationsToCalculateEdges, Logger)
            Else
                locationEdgeCalc.UpdateEdgeIdForLocations(locationsToCalculateEdges, Nothing)
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Error while calculating Edges " + ex.ToString())
            End If
            strMessage = "Error while calculating Edges, please see the logs for details."
        Finally
            If Not Logger Is Nothing Then
                Logger.Dispose()
            End If
        End Try
        Return strMessage
    End Function

    ' Asynchronous call
    Public Sub UpdateEdgesOnLocationsAsync(ByVal instancePath As String, ByVal fileName As String, ByVal locationsToCalculateEdges As DataRow(), ByVal locationEdgeCalc As Made4Net.Algorithms.EdgeCalculation.LocationsEdgeCalculation)
        Dim evaluator As New System.Threading.Thread(Sub() Me.UpdateEdgesOnLocations(instancePath, fileName, locationsToCalculateEdges, locationEdgeCalc))
        With evaluator
            .IsBackground = True ' not necessary...
            .Start()
        End With
    End Sub


    Public Sub New(ByVal pLocation As String, ByVal pWarehousearea As String, Optional ByVal LoadObj As Boolean = True)
        _Location = pLocation
        _warehousearea = pWarehousearea
        If LoadObj Then
            Load()
        End If
    End Sub

    'RWMS-2646 RWMS-2645 START
    Public Sub New(ByVal pLocation As String)
        _Location = pLocation
        Load(_Location)
    End Sub
    'RWMS-2646 RWMS-2645 END

#End Region

#Region "Methods"

    Public Function Save(ByVal pUser As String)
        Dim SQL As String
        Dim aq As EventManagerQ = New EventManagerQ
        Dim activitystatus As String

        If WMS.Logic.Location.Exists(_Location, _warehousearea) Then
            ' Update new location
            SQL = String.Format("UPDATE LOCATION " &
                                "SET STATUS ={0}, PUTREGION ={1}, PICKREGION ={2}, WAREHOUSEAREA ={3}, LOCSORTORDER ={4}, LOCTPICKYPE ={5}, LOCSTORAGETYPE ={6}, LOCUSAGETYPE ={7}, " &
                                "LOCMHTYPE ={8}, INVENTORY ={9}, LASTCOUNTDATE ={10}, ACCESSTYPE ={11}, ACCESSIBLELOADS ={12}, LENGTH ={13}, WIDTH ={14}, HEIGHT ={15}, WEIGHT ={16}, CUBIC ={17}, " &
                                "LOADSCAPACITY ={18}, PENDINGWEIGHT ={19}, PENDINGCUBIC ={20}, PENDINGLOADS ={21}, CHECKDIGITS ={22}, LOOSEID ={23}, AISLE ={24}, BAY ={25}, LOCLEVEL ={26}, " &
                                "HEIGHTFROMFLOOR ={27}, LASTMOVEIN ={28}, LASTMOVEOUT ={29}, PICKSFROMLASTCOUNT ={30}, XCOORDINATE ={31}, YCOORDINATE ={32}, ZCOORDINATE ={33}, " &
                                "INHANDOFF ={34}, OUTHANDOFF ={35}, LOCACCESSIBILITY = {36},ZPICKINGLOCATION = {37}, EDITDATE ={38}, EDITUSER ={39} WHERE LOCATION={40}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_putregion), Made4Net.Shared.Util.FormatField(_pickregion), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_locsortorder), Made4Net.Shared.Util.FormatField(_loctpickype), Made4Net.Shared.Util.FormatField(_locstoragetype), Made4Net.Shared.Util.FormatField(_locusagetype),
                                Made4Net.Shared.Util.FormatField(_locmhtype), Made4Net.Shared.Util.FormatField(_inventory), Made4Net.Shared.Util.FormatField(_lastcountdate), Made4Net.Shared.Util.FormatField(_accesstype), Made4Net.Shared.Util.FormatField(_accessibleloads), Made4Net.Shared.Util.FormatField(_length), Made4Net.Shared.Util.FormatField(_width), Made4Net.Shared.Util.FormatField(_height), Made4Net.Shared.Util.FormatField(_weight), Made4Net.Shared.Util.FormatField(_cubic),
                                Made4Net.Shared.Util.FormatField(_loadscapacity), Made4Net.Shared.Util.FormatField(_pendingweight), Made4Net.Shared.Util.FormatField(_pendingcubic), Made4Net.Shared.Util.FormatField(_pendingloads), Made4Net.Shared.Util.FormatField(_checkdigits), Made4Net.Shared.Util.FormatField(_looseid), Made4Net.Shared.Util.FormatField(_aisle), Made4Net.Shared.Util.FormatField(_bay), Made4Net.Shared.Util.FormatField(_loclevel),
                                Made4Net.Shared.Util.FormatField(_heightfromfloor), Made4Net.Shared.Util.FormatField(_lastmovein), Made4Net.Shared.Util.FormatField(_lastmoveout), Made4Net.Shared.Util.FormatField(_picksfromlastcount), Made4Net.Shared.Util.FormatField(_xcoordinate), Made4Net.Shared.Util.FormatField(_ycoordinate), Made4Net.Shared.Util.FormatField(_zcoordinate),
                                Made4Net.Shared.Util.FormatField(_inhandoff), Made4Net.Shared.Util.FormatField(_outhandoff), Made4Net.Shared.Util.FormatField(_locaccessibility), Made4Net.Shared.Util.FormatField(_zpickinglocation), Made4Net.Shared.Util.FormatField(DateTime.Now()), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(_Location))

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LocationUpdated)
            activitystatus = WMS.Lib.Actions.Audit.LOCATIONUPD
        Else
            ' Insert new location
            SQL = String.Format("INSERT INTO LOCATION " &
                                "(LOCATION, STATUS, PUTREGION, PICKREGION, WAREHOUSEAREA, LOCSORTORDER, LOCTPICKYPE, LOCSTORAGETYPE, LOCUSAGETYPE, " &
                                "LOCMHTYPE, INVENTORY, LASTCOUNTDATE, ACCESSTYPE, ACCESSIBLELOADS, LENGTH, WIDTH, HEIGHT, WEIGHT, CUBIC, LOADSCAPACITY, " &
                                "PENDINGWEIGHT, PENDINGCUBIC, PENDINGLOADS, CHECKDIGITS, LOOSEID, AISLE, BAY, LOCLEVEL, HEIGHTFROMFLOOR, LASTMOVEIN, " &
                                "LASTMOVEOUT, PICKSFROMLASTCOUNT, XCOORDINATE, YCOORDINATE, ZCOORDINATE, INHANDOFF, OUTHANDOFF, LOCACCESSIBILITY, ADDDATE, ADDUSER, " &
                                "EDITDATE, EDITUSER) VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41})", Made4Net.Shared.Util.FormatField(_Location), Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_putregion), Made4Net.Shared.Util.FormatField(_pickregion), Made4Net.Shared.Util.FormatField(_warehousearea), Made4Net.Shared.Util.FormatField(_locsortorder), Made4Net.Shared.Util.FormatField(_loctpickype), Made4Net.Shared.Util.FormatField(_locstoragetype), Made4Net.Shared.Util.FormatField(_locusagetype),
                                Made4Net.Shared.Util.FormatField(_locmhtype), Made4Net.Shared.Util.FormatField(_inventory), Made4Net.Shared.Util.FormatField(_lastcountdate), Made4Net.Shared.Util.FormatField(_accesstype), Made4Net.Shared.Util.FormatField(_accessibleloads), Made4Net.Shared.Util.FormatField(_length), Made4Net.Shared.Util.FormatField(_width), Made4Net.Shared.Util.FormatField(_height), Made4Net.Shared.Util.FormatField(_weight), Made4Net.Shared.Util.FormatField(_cubic),
                                Made4Net.Shared.Util.FormatField(_loadscapacity), Made4Net.Shared.Util.FormatField(_pendingweight), Made4Net.Shared.Util.FormatField(_pendingcubic), Made4Net.Shared.Util.FormatField(_pendingloads), Made4Net.Shared.Util.FormatField(_checkdigits), Made4Net.Shared.Util.FormatField(_looseid), Made4Net.Shared.Util.FormatField(_aisle), Made4Net.Shared.Util.FormatField(_bay), Made4Net.Shared.Util.FormatField(_loclevel),
                                Made4Net.Shared.Util.FormatField(_heightfromfloor), Made4Net.Shared.Util.FormatField(_lastmovein), Made4Net.Shared.Util.FormatField(_lastmoveout), Made4Net.Shared.Util.FormatField(_picksfromlastcount), Made4Net.Shared.Util.FormatField(_xcoordinate), Made4Net.Shared.Util.FormatField(_ycoordinate), Made4Net.Shared.Util.FormatField(_zcoordinate),
                                Made4Net.Shared.Util.FormatField(_inhandoff), Made4Net.Shared.Util.FormatField(_outhandoff), Made4Net.Shared.Util.FormatField(_locaccessibility), Made4Net.Shared.Util.FormatField(DateTime.Now()), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(DateTime.Now()), Made4Net.Shared.Util.FormatField(pUser))

            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LocationCreated)
            activitystatus = WMS.Lib.Actions.Audit.LOCATIONINS
        End If
        DataInterface.RunSQL(SQL)

        aq.Add("ACTIVITYTYPE", activitystatus)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")

        aq.Add("FROMLOC", _Location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)

        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", _status)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")

        aq.Add("TOLOC", _Location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)

        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", _status)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(activitystatus)
    End Function
    'Added for RWMS-1540 and RWMS-1488
    Public Shared Function Exists(ByVal pLocation As String, ByVal pWarehousearea As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim sql As String = String.Format("Select count(1) from Location where LOCATION = '{0}' and WAREHOUSEAREA= '{1}' ", pLocation, pWarehousearea)
        If Not oLogger Is Nothing Then
            oLogger.Write("Validating Location ....")
            oLogger.Write(String.Format("SQL: {0}", sql))
        End If
        'Ended for RWMS-1540 and RWMS-1488
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Function GetLocation(ByVal pLocation As String, ByVal pWarehousearea As String) As Location
        Return New Location(pLocation, pWarehousearea)
    End Function

    Public Shared Function CheckForCheckDigitMatching(ByVal pLocation As String, ByVal pCheckDigit As String, ByVal pWarehousearea As String) As String
        ' if no matching than empty string will be returned
        Dim str As String = String.Empty
        If String.IsNullOrEmpty(pCheckDigit) Then
            Return str
        End If
        If DataInterface.ExecuteScalar("SELECT COUNT(1) FROM LOCATION WHERE LOCATION='" & pLocation & "' AND  CHECKDIGITS='" & pCheckDigit & "' AND  WAREHOUSEAREA ='" & pWarehousearea & "'") > 0 Then
            str = pLocation
        End If
        Return str
    End Function

    Public Shared Function CheckLocationConfirmation(ByVal pLocation As String, ByVal pCheckDigit As String, ByVal pWarehouseArea As String, Optional ByVal logger As LogHandler = Nothing) As String
        Dim str As String = String.Empty
        If Not logger Is Nothing Then
            logger.Write("Proceeding to check location confirmation for location " + pLocation + " for warehousearea " + pWarehouseArea + " for check digit " + pCheckDigit)
        End If
        ' First lets see if the toLocation = CONFIRMATION was checkdigit of location
        If Exists(pCheckDigit, pWarehouseArea) Then
            str = pCheckDigit
        Else
            Dim tmpLocStr As String = CheckForCheckDigitMatching(pLocation, pCheckDigit, pWarehouseArea)
            If tmpLocStr <> String.Empty Then
                str = tmpLocStr
            Else
                If Exists(pCheckDigit, pWarehouseArea, logger) Then
                    str = pCheckDigit
                Else
                    Throw New Made4Net.Shared.M4NException("Location confirmation incorrect")
                End If
            End If
        End If
        Return str
    End Function

    Public Function getNumberOfLoads() As Int32
        Dim sql As String = String.Format("Select count(1) from invload where Location = '{0}' and WAREHOUSEAREA= '{1}'", _Location, _warehousearea)
        Return System.Convert.ToInt32(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function getNumberOfPendingLoads(ByVal ld As Load) As Int32
        Dim sql As String = String.Format("Select count(1) from invload where DestinationLocation = '{0}' and DESTINATIONWAREHOUSEAREA = '{2}' and sku <> '{1}'", _Location, ld.SKU, _warehousearea)
        Return System.Convert.ToInt32(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function getLoadsVolume() As Double
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim Vol As Double = 0
        DataInterface.FillDataset(String.Format("Select LOADID from invload where Location = '{0}'", _Location), dt)
        For Each dr In dt.Rows
            Vol += New Load(System.Convert.ToString(dr("LOADID"))).Volume
        Next
        Return Vol
    End Function

    Public Function getLoadsWeight() As Double
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim Vol As Double = 0
        DataInterface.FillDataset(String.Format("Select LOADID from invload where Location = '{0}'", _Location), dt)
        For Each dr In dt.Rows
            Vol += New Load(System.Convert.ToString(dr("LOADID"))).CalculateWeight
        Next
        Return Vol
    End Function

    Protected Sub Load()

        'RWMS-2646 RWMS-2645
        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()
        'RWMS-2646 RWMS-2645 END

        Dim SQL As String = "SELECT * FROM Location " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow

        'RWMS-2646 RWMS-2645
        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" sql:{0}", SQL))
        End If
        'RWMS-2646 RWMS-2645 END

        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

            'RWMS-2646 RWMS-2645
            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Location does not exists"))
            End If
            'RWMS-2646 RWMS-2645 END

            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Location does not exists", "Location does not exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("PUTREGION") Then _putregion = dr.Item("PUTREGION")
        If Not dr.IsNull("PICKREGION") Then _pickregion = dr.Item("PICKREGION")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("LOCSORTORDER") Then _locsortorder = dr.Item("LOCSORTORDER")
        If Not dr.IsNull("LOCTPICKYPE") Then _loctpickype = dr.Item("LOCTPICKYPE")
        If Not dr.IsNull("LOCSTORAGETYPE") Then _locstoragetype = dr.Item("LOCSTORAGETYPE")
        If Not dr.IsNull("LOCUSAGETYPE") Then _locusagetype = dr.Item("LOCUSAGETYPE")
        If Not dr.IsNull("LOCMHTYPE") Then _locmhtype = dr.Item("LOCMHTYPE")
        If Not dr.IsNull("INVENTORY") Then _inventory = dr.Item("INVENTORY")
        If Not dr.IsNull("LASTCOUNTDATE") Then _lastcountdate = dr.Item("LASTCOUNTDATE")
        If Not dr.IsNull("ACCESSTYPE") Then _accesstype = dr.Item("ACCESSTYPE")
        If Not dr.IsNull("ACCESSIBLELOADS") Then _accessibleloads = dr.Item("ACCESSIBLELOADS")
        If Not dr.IsNull("LENGTH") Then _length = dr.Item("LENGTH")
        If Not dr.IsNull("WIDTH") Then _width = dr.Item("WIDTH")
        If Not dr.IsNull("HEIGHT") Then _height = dr.Item("HEIGHT")
        If Not dr.IsNull("WEIGHT") Then _weight = dr.Item("WEIGHT")
        If Not dr.IsNull("CUBIC") Then _cubic = dr.Item("CUBIC")
        If Not dr.IsNull("LOADSCAPACITY") Then _loadscapacity = dr.Item("LOADSCAPACITY")
        If Not dr.IsNull("PENDINGWEIGHT") Then _pendingweight = dr.Item("PENDINGWEIGHT")
        If Not dr.IsNull("PENDINGCUBIC") Then _pendingcubic = dr.Item("PENDINGCUBIC")
        If Not dr.IsNull("PENDINGLOADS") Then _pendingloads = dr.Item("PENDINGLOADS")
        If Not dr.IsNull("CHECKDIGITS") Then _checkdigits = dr.Item("CHECKDIGITS")
        If Not dr.IsNull("LOOSEID") Then _looseid = dr.Item("LOOSEID")
        If Not dr.IsNull("AISLE") Then _aisle = dr.Item("AISLE")
        If Not dr.IsNull("BAY") Then _bay = dr.Item("BAY")
        If Not dr.IsNull("LOCLEVEL") Then _loclevel = dr.Item("LOCLEVEL")
        If Not dr.IsNull("HEIGHTFROMFLOOR") Then _heightfromfloor = dr.Item("HEIGHTFROMFLOOR")
        If Not dr.IsNull("LASTMOVEIN") Then _lastmovein = dr.Item("LASTMOVEIN")
        If Not dr.IsNull("LASTMOVEOUT") Then _lastmoveout = dr.Item("LASTMOVEOUT")
        If Not dr.IsNull("PICKSFROMLASTCOUNT") Then _picksfromlastcount = dr.Item("PICKSFROMLASTCOUNT")
        If Not dr.IsNull("XCOORDINATE") Then _xcoordinate = dr.Item("XCOORDINATE")
        If Not dr.IsNull("YCOORDINATE") Then _ycoordinate = dr.Item("YCOORDINATE")
        If Not dr.IsNull("ZCOORDINATE") Then _zcoordinate = dr.Item("ZCOORDINATE")
        If Not dr.IsNull("INHANDOFF") Then _inhandoff = dr.Item("INHANDOFF")
        If Not dr.IsNull("OUTHANDOFF") Then _outhandoff = dr.Item("OUTHANDOFF")
        If Not dr.IsNull("LOCACCESSIBILITY") Then _locaccessibility = dr.Item("LOCACCESSIBILITY")
        If Not dr.IsNull("PROBLEMFLAG") Then _problemflag = dr.Item("PROBLEMFLAG")
        If Not dr.IsNull("PROBLEMFLAGRC") Then _problemflagrc = dr.Item("PROBLEMFLAGRC")
        If Not dr.IsNull("ZPICKINGLOCATION") Then _zpickinglocation = dr.Item("ZPICKINGLOCATION")
        If Not dr.IsNull("ZPICKINGWAREHOUSEAREA") Then _zpickingwarehousearea = dr.Item("ZPICKINGWAREHOUSEAREA")
        If Not dr.IsNull("HUSTORAGETEMPLATE") Then _hustoragetemplate = dr.Item("HUSTORAGETEMPLATE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

    End Sub

    'RWMS-2646 RWMS-2645
    Protected Sub Load(ByVal pLocation As String)

        Dim wmsrdtLogger As WMS.Logic.LogHandler = WMS.Logic.LogHandler.GetWMSRDTLogger()

        Dim SQL As String = String.Format("SELECT * FROM Location where Location = '{0}' ", pLocation)
        Dim dt As New DataTable
        Dim dr As DataRow

        If Not wmsrdtLogger Is Nothing Then
            wmsrdtLogger.Write(String.Format(" sql:{0}", SQL))
        End If

        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

            If Not wmsrdtLogger Is Nothing Then
                wmsrdtLogger.Write(String.Format(" Location does not exists"))
            End If

            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Location does not exists", "Location does not exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("STATUS") Then _status = dr.Item("STATUS")
        If Not dr.IsNull("PUTREGION") Then _putregion = dr.Item("PUTREGION")
        If Not dr.IsNull("PICKREGION") Then _pickregion = dr.Item("PICKREGION")
        If Not dr.IsNull("WAREHOUSEAREA") Then _warehousearea = dr.Item("WAREHOUSEAREA")
        If Not dr.IsNull("LOCSORTORDER") Then _locsortorder = dr.Item("LOCSORTORDER")
        If Not dr.IsNull("LOCTPICKYPE") Then _loctpickype = dr.Item("LOCTPICKYPE")
        If Not dr.IsNull("LOCSTORAGETYPE") Then _locstoragetype = dr.Item("LOCSTORAGETYPE")
        If Not dr.IsNull("LOCUSAGETYPE") Then _locusagetype = dr.Item("LOCUSAGETYPE")
        If Not dr.IsNull("LOCMHTYPE") Then _locmhtype = dr.Item("LOCMHTYPE")
        If Not dr.IsNull("INVENTORY") Then _inventory = dr.Item("INVENTORY")
        If Not dr.IsNull("LASTCOUNTDATE") Then _lastcountdate = dr.Item("LASTCOUNTDATE")
        If Not dr.IsNull("ACCESSTYPE") Then _accesstype = dr.Item("ACCESSTYPE")
        If Not dr.IsNull("ACCESSIBLELOADS") Then _accessibleloads = dr.Item("ACCESSIBLELOADS")
        If Not dr.IsNull("LENGTH") Then _length = dr.Item("LENGTH")
        If Not dr.IsNull("WIDTH") Then _width = dr.Item("WIDTH")
        If Not dr.IsNull("HEIGHT") Then _height = dr.Item("HEIGHT")
        If Not dr.IsNull("WEIGHT") Then _weight = dr.Item("WEIGHT")
        If Not dr.IsNull("CUBIC") Then _cubic = dr.Item("CUBIC")
        If Not dr.IsNull("LOADSCAPACITY") Then _loadscapacity = dr.Item("LOADSCAPACITY")
        If Not dr.IsNull("PENDINGWEIGHT") Then _pendingweight = dr.Item("PENDINGWEIGHT")
        If Not dr.IsNull("PENDINGCUBIC") Then _pendingcubic = dr.Item("PENDINGCUBIC")
        If Not dr.IsNull("PENDINGLOADS") Then _pendingloads = dr.Item("PENDINGLOADS")
        If Not dr.IsNull("CHECKDIGITS") Then _checkdigits = dr.Item("CHECKDIGITS")
        If Not dr.IsNull("LOOSEID") Then _looseid = dr.Item("LOOSEID")
        If Not dr.IsNull("AISLE") Then _aisle = dr.Item("AISLE")
        If Not dr.IsNull("BAY") Then _bay = dr.Item("BAY")
        If Not dr.IsNull("LOCLEVEL") Then _loclevel = dr.Item("LOCLEVEL")
        If Not dr.IsNull("HEIGHTFROMFLOOR") Then _heightfromfloor = dr.Item("HEIGHTFROMFLOOR")
        If Not dr.IsNull("LASTMOVEIN") Then _lastmovein = dr.Item("LASTMOVEIN")
        If Not dr.IsNull("LASTMOVEOUT") Then _lastmoveout = dr.Item("LASTMOVEOUT")
        If Not dr.IsNull("PICKSFROMLASTCOUNT") Then _picksfromlastcount = dr.Item("PICKSFROMLASTCOUNT")
        If Not dr.IsNull("XCOORDINATE") Then _xcoordinate = dr.Item("XCOORDINATE")
        If Not dr.IsNull("YCOORDINATE") Then _ycoordinate = dr.Item("YCOORDINATE")
        If Not dr.IsNull("ZCOORDINATE") Then _zcoordinate = dr.Item("ZCOORDINATE")
        If Not dr.IsNull("INHANDOFF") Then _inhandoff = dr.Item("INHANDOFF")
        If Not dr.IsNull("OUTHANDOFF") Then _outhandoff = dr.Item("OUTHANDOFF")
        If Not dr.IsNull("LOCACCESSIBILITY") Then _locaccessibility = dr.Item("LOCACCESSIBILITY")
        If Not dr.IsNull("PROBLEMFLAG") Then _problemflag = dr.Item("PROBLEMFLAG")
        If Not dr.IsNull("PROBLEMFLAGRC") Then _problemflagrc = dr.Item("PROBLEMFLAGRC")
        If Not dr.IsNull("ZPICKINGLOCATION") Then _zpickinglocation = dr.Item("ZPICKINGLOCATION")
        If Not dr.IsNull("ZPICKINGWAREHOUSEAREA") Then _zpickingwarehousearea = dr.Item("ZPICKINGWAREHOUSEAREA")
        If Not dr.IsNull("HUSTORAGETEMPLATE") Then _hustoragetemplate = dr.Item("HUSTORAGETEMPLATE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")

    End Sub
    'RWMS-2645 END

    Public Sub SetDestLoads(ByVal oLoad As Load, ByVal pUser As String)
        _pendingcubic += oLoad.Volume
        _pendingweight += oLoad.Weight
        _pendingloads += 1
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update location set PENDINGWEIGHT = {0} , PENDINGCUBIC = {1}, PENDINGLOADS = {2}, " &
            " EDITDATE = {3} , EDITUSER = {4} {5}", Made4Net.Shared.Util.FormatField(_pendingweight), Made4Net.Shared.Util.FormatField(_pendingcubic),
            Made4Net.Shared.Util.FormatField(_pendingloads), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.ExecuteScalar(SQL)
    End Sub

    Public Sub CancelPut(ByVal oLoad As Load, ByVal puser As String)
        '_pendingcubic -= oLoad.Volume()
        '_pendingweight -= oLoad.Weight
        '_pendingloads -= 1
        '_editdate = DateTime.Now
        '_edituser = puser
        'Dim sql As String = String.Format("Update location set pendingcubic={0},pendingweight={1},pendingloads={2},editdate={3},edituser={4} {5}", _
        '    Made4Net.Shared.Util.FormatField(_pendingcubic), Made4Net.Shared.Util.FormatField(_pendingweight), Made4Net.Shared.Util.FormatField(_pendingloads), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        'DataInterface.RunSQL(sql)
    End Sub

    Public Function Put(ByVal oLoad As Load, ByVal puser As String)
        If _status = False Then
            Throw New Made4Net.Shared.M4NException(New Exception, "LocationNotActive", "LocationNotActive")
        End If
        If IsPickLocation Then
            Dim oPickLoc As PickLoc = PickLoc.GetPickLoc(_Location, _warehousearea, oLoad.CONSIGNEE, oLoad.SKU)
            oPickLoc.Put(oLoad, puser)
        End If

        'Commented out - this properties are calculated in the views at run time.

        'If oLoad.DESTINATIONLOCATION = _Location And oLoad.DESTINATIONWAREHOUSEAREA = _warehousearea Then
        '    _pendingcubic -= oLoad.Volume()
        '    _pendingweight -= oLoad.Weight
        '    _pendingloads -= 1
        'Else
        '    If Not oLoad.DESTINATIONLOCATION Is Nothing And oLoad.DESTINATIONLOCATION <> "" Then
        '        Dim oLoc As New Location(oLoad.DESTINATIONLOCATION, oLoad.DESTINATIONWAREHOUSEAREA)
        '        oLoc.CancelPut(oLoad, puser)
        '    End If
        'End If
        '_editdate = DateTime.Now
        '_edituser = puser

        'Dim SQL As String = String.Format("update location set PENDINGWEIGHT = {0} , PENDINGCUBIC = {1}, PENDINGLOADS = {2}, " & _
        '    " EDITDATE = {3} , EDITUSER = {4} {5}", Made4Net.Shared.Util.FormatField(_pendingweight), Made4Net.Shared.Util.FormatField(_pendingcubic), _
        '    Made4Net.Shared.Util.FormatField(_pendingloads), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        'DataInterface.RunSQL(SQL)
    End Function
    'Commented for RWMS-467
    'Public Sub Count(ByVal pFromQty As Decimal, ByVal pToQty As Decimal, ByVal pUser As String)
    '    _lastcountdate = DateTime.Now
    '    _picksfromlastcount = 0
    '    _editdate = DateTime.Now
    '    _edituser = pUser

    '    Dim SQL As String = String.Format("update location set lastcountdate={0} ,  picksfromlastcount={1}, " & _
    '        " EDITDATE = {2} , EDITUSER = {3} {4}", Made4Net.Shared.Util.FormatField(_lastcountdate), Made4Net.Shared.Util.FormatField(_picksfromlastcount), _
    '        Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
    '    DataInterface.ExecuteScalar(SQL)


    '    Dim aq As EventManagerQ = New EventManagerQ
    '    aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LocationCount)
    '    aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOCATIONCOUNT)
    '    aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ACTIVITYTIME", "0")
    '    aq.Add("CONSIGNEE", "")
    '    aq.Add("DOCUMENT", "")
    '    aq.Add("DOCUMENTLINE", 0)
    '    aq.Add("FROMLOAD", "")
    '    aq.Add("FROMLOC", _Location)
    '    aq.Add("FROMWAREHOUSEAREA", _warehousearea)
    '    aq.Add("FROMQTY", pFromQty)
    '    aq.Add("FROMSTATUS", _status)
    '    aq.Add("NOTES", "")
    '    aq.Add("SKU", "")
    '    aq.Add("TOLOAD", "")
    '    aq.Add("TOLOC", _Location)
    '    aq.Add("TOWAREHOUSEAREA", _warehousearea)
    '    aq.Add("TOQTY", pToQty)
    '    aq.Add("TOSTATUS", _status)
    '    aq.Add("USERID", pUser)
    '    aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("ADDUSER", pUser)
    '    aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
    '    aq.Add("EDITUSER", pUser)
    '    aq.Send(WMS.Lib.Actions.Audit.LOCATIONCOUNT)
    'End Sub
    'End Commented for RWMS-467
    'Added for RWMS-467
    Public Sub Count(ByVal pFromQty As Decimal, ByVal pToQty As Decimal, ByVal pUser As String, ByVal pDOCUMENT As String, ByVal dt As DataTable)
        _lastcountdate = DateTime.Now
        _picksfromlastcount = 0
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update location set lastcountdate={0} ,  picksfromlastcount={1}, " &
            " EDITDATE = {2} , EDITUSER = {3} {4}", Made4Net.Shared.Util.FormatField(_lastcountdate), Made4Net.Shared.Util.FormatField(_picksfromlastcount),
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.ExecuteScalar(SQL)

        If Not dt Is Nothing Then
            For Each dr As DataRow In dt.Rows
                If dr("counted") = 1 Then
                    'get the sku and consignee from loadid
                    Dim ds As New DataSet
                    Dim dr1 As DataRow

                    SQL = String.Format("select * from Invload where loadid = '{0}'", dr("loadid"))
                    DataInterface.FillDataset(SQL, ds)

                    For Each dr1 In ds.Tables(0).Rows

                        Dim aq As EventManagerQ = New EventManagerQ
                        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LocationCount)
                        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOCATIONCOUNT)
                        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                        aq.Add("ACTIVITYTIME", "0")
                        aq.Add("CONSIGNEE", dr1("consignee"))
                        aq.Add("DOCUMENT", pDOCUMENT)
                        aq.Add("DOCUMENTLINE", 0)
                        aq.Add("FROMLOAD", dr("loadid"))
                        aq.Add("FROMLOC", _Location)
                        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
                        'aq.Add("FROMQTY", pFromQty)
                        aq.Add("FROMQTY", dr("fromqty"))
                        aq.Add("FROMSTATUS", _status)
                        aq.Add("NOTES", "")
                        aq.Add("SKU", dr1("sku"))
                        aq.Add("TOLOAD", "")
                        aq.Add("TOLOC", _Location)
                        aq.Add("TOWAREHOUSEAREA", _warehousearea)
                        'aq.Add("TOQTY", pToQty)
                        aq.Add("TOQTY", dr("toqty"))
                        aq.Add("TOSTATUS", _status)
                        aq.Add("USERID", pUser)
                        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                        aq.Add("ADDUSER", pUser)
                        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                        aq.Add("EDITUSER", pUser)
                        aq.Send(WMS.Lib.Actions.Audit.LOCATIONCOUNT)

                    Next
                End If
            Next
        End If
    End Sub
    'End Added for RWMS-467

    Public Sub SetProblemFlag(ByVal pProblem As Boolean, ByVal pReasonCode As String, ByVal pUser As String)
        Dim oldProblemStatus As Boolean = _problemflag
        If _problemflag = pProblem Then
            If _problemflag Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location Already Marked as problematic", "Location Already Marked as problematic")
            End If
        End If
        _problemflag = pProblem
        _problemflagrc = pReasonCode
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update location set problemflag={0}, problemflagrc={1} , " &
            " EDITDATE = {2} , EDITUSER = {3} {4}", Made4Net.Shared.Util.FormatField(_problemflag), Made4Net.Shared.Util.FormatField(_problemflagrc),
            Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.ExecuteScalar(SQL)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.LocationProblem)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.LOCATIONPRBLM)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", _Location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", 0)
        'Commented for RWMS-1554 and RWMS-1506 Start
        'aq.Add("FROMSTATUS", _status)
        'Commented for RWMS-1554 and RWMS-1506 End

        'Added for RWMS-1554 and RWMS-1506 Start
        aq.Add("FROMSTATUS", oldProblemStatus)
        'Added for RWMS-1554 and RWMS-1506 End

        aq.Add("NOTES", pReasonCode)
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", _Location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", 0)
        'Commented for RWMS-1554 and RWMS-1506 Start
        'aq.Add("TOSTATUS", _status)
        'Commented for RWMS-1554 and RWMS-1506 End

        'Added for RWMS-1554 and RWMS-1506 Start
        aq.Add("TOSTATUS", _problemflag)
        'Added for RWMS-1554 and RWMS-1506 End

        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.LOCATIONPRBLM)
    End Sub
    'Added for RWMS-1554 and RWMS-1506 Start

    Public Sub CancelProblemFlag(ByVal pProblem As Boolean, ByVal pReasonCode As String, ByVal pUser As String)
        Dim oldProblemStatus As Boolean = _problemflag
        If _problemflag = pProblem Then
            If _problemflag Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Location is not in problematic state", "Location is not in problematic state")
            End If
        End If
        _problemflag = pProblem
        _problemflagrc = ""
        _editdate = DateTime.Now
        _edituser = pUser

        Dim SQL As String = String.Format("update location set problemflag={0}, problemflagrc={1} , " &
          " EDITDATE = {2} , EDITUSER = {3} {4}", Made4Net.Shared.Util.FormatField(_problemflag), Made4Net.Shared.Util.FormatField(_problemflagrc),
          Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), WhereClause)
        DataInterface.ExecuteScalar(SQL)

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.CancelLocationProblem)
        aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.CANCELLOCATIONPRBLM)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMLOC", _Location)
        aq.Add("FROMWAREHOUSEAREA", _warehousearea)
        aq.Add("FROMQTY", 0)
        aq.Add("FROMSTATUS", oldProblemStatus)
        aq.Add("NOTES", "")
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOLOC", _Location)
        aq.Add("TOWAREHOUSEAREA", _warehousearea)
        aq.Add("TOQTY", 0)
        aq.Add("TOSTATUS", _problemflag)
        aq.Add("USERID", pUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", pUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", pUser)
        aq.Send(WMS.Lib.Actions.Audit.CANCELLOCATIONPRBLM)
    End Sub
    'Added for RWMS-1554 and RWMS-1506 End

    Public Sub PrintLabels()
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "LOCATION")
        qSender.Add("PRINTER", "")
        qSender.Add("LOCATION", _Location)
        qSender.Send("Label", "Location Label")
    End Sub

    Public Sub DeleteLocation()
        Dim sql As String = String.Format("Delete from handoff where HANDOFFLOCATION ={0} and HANDOFFWAREHOUSEAREA={1} ",
        Made4Net.Shared.Util.FormatField(_Location), Made4Net.Shared.Util.FormatField(_warehousearea))
        DataInterface.RunSQL(sql)

        sql = String.Format("Delete from location {0}", WhereClause)
        DataInterface.RunSQL(sql)

    End Sub

    Public Sub DeletePickLocation(ByVal strPickLoc As String, ByVal strWarehousearea As String)
        Dim sql As String = String.Format("Delete from PICKLOC where LOCATION ={0} and WAREHOUSEAREA={1} ",
        Made4Net.Shared.Util.FormatField(strPickLoc), Made4Net.Shared.Util.FormatField(strWarehousearea))
        DataInterface.RunSQL(sql)
    End Sub
    'Added code to insert XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
    Public Sub Create(ByVal pLocation As String, ByVal pStatus As Boolean, ByVal pPutRegion As String, ByVal pPickRegion As String,
    ByVal pWarehouseArea As String, ByVal pLocSortOrder As String, ByVal pLocPickType As String, ByVal pLocStorageType As String,
    ByVal pLocUsageType As String, ByVal pLocMHType As String, ByVal pInventory As Boolean, ByVal pLastCountDate As DateTime,
    ByVal pAccessType As String, ByVal pAccessibleLoads As Integer, ByVal pLength As Decimal, ByVal pWidth As Decimal,
    ByVal pHeight As Decimal, ByVal pWeight As Decimal, ByVal pCubic As Decimal, ByVal pLoadsCapacity As Integer, ByVal pPendingWeight As Decimal,
    ByVal pPendingCubic As Decimal, ByVal pPendingLoads As Decimal, ByVal pCheckDigits As String, ByVal pLooseID As Boolean, ByVal pAisle As String,
    ByVal pBay As String, ByVal pLocLevel As String, ByVal pHeightFromFloor As Decimal, ByVal pLastMoveIn As DateTime, ByVal pLastMoveOut As DateTime,
    ByVal pPicksFromLastCount As Integer, ByVal pXCoordinate As Integer, ByVal pYCoordinate As Integer, ByVal pZCoordinate As Integer,
    ByVal pInHandOff As String, ByVal pOutHandOff As String, ByVal pAddDate As DateTime, ByVal pAddUser As String, ByVal pEditDate As DateTime,
    ByVal pEditUser As String, ByVal pLocAccessibility As String, ByVal pLaborHUFacing As String, ByVal pLaborInsertType As String,
    ByVal pLaborRetrieveType As String, ByVal pLaborPickType As String, ByVal pLaborReachType As String, ByVal pCongestionRegion As String,
    ByVal pProblemFlag As Boolean, ByVal pHUStorageTemplate As String, ByVal pZPickingLocation As String, ByVal pZPickingWarehouseArea As String,
    ByVal pXfillCordinate As Integer, ByVal pYfillCordinate As Integer, ByVal pZfillCordinate As Integer,
    ByVal pPickEdge As Integer, ByVal pFillEdge As Integer)
        'Ended code to insert XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        _Location = pLocation
        _status = pStatus
        _putregion = pPutRegion
        _pickregion = pPickRegion

        _warehousearea = pWarehouseArea
        _locsortorder = pLocSortOrder
        _loctpickype = pLocPickType
        _locstoragetype = pLocStorageType

        _locusagetype = pLocUsageType
        _locmhtype = pLocMHType
        _inventory = pInventory
        _lastcountdate = pLastCountDate

        _accesstype = pAccessType
        _accessibleloads = pAccessibleLoads
        _length = pLength
        _width = pWidth

        _height = pHeight
        _weight = pWeight
        _cubic = Decimal.Round((pWidth * pLength * pHeight) / 1728, 3, MidpointRounding.AwayFromZero)
        _loadscapacity = pLoadsCapacity
        _pendingweight = pPendingWeight

        _pendingcubic = pPendingCubic
        _pendingloads = pPendingLoads
        _checkdigits = pCheckDigits
        _looseid = pLooseID
        _aisle = pAisle

        _bay = pBay
        _loclevel = pLocLevel
        _heightfromfloor = pHeightFromFloor
        _lastmovein = pLastMoveIn
        _lastmoveout = pLastMoveOut

        _picksfromlastcount = pPicksFromLastCount
        _xcoordinate = pXCoordinate
        _ycoordinate = pYCoordinate
        _zcoordinate = pZCoordinate

        _inhandoff = pInHandOff
        _outhandoff = pOutHandOff
        _adddate = pAddDate
        _adduser = pAddUser
        _editdate = pEditDate

        _edituser = pEditUser
        _locaccessibility = pLocAccessibility
        _laborhufacing = pLaborHUFacing
        _laborinserttype = pLaborInsertType

        _laborretrievetype = pLaborRetrieveType
        _laborpicktype = pLaborPickType
        _laborreachtype = pLaborReachType
        _congestionregion = pCongestionRegion

        _problemflag = pProblemFlag
        _hustoragetemplate = pHUStorageTemplate
        _zpickinglocation = pZPickingLocation
        _zpickingwarehousearea = pZPickingWarehouseArea

        'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        _xfillcordinate = pXfillCordinate
        _yfillcordinate = pYfillCordinate
        _zfillcordinate = pZfillCordinate
        _pickedge = pPickEdge
        _filledge = pFillEdge
        'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203

        validate(False)
        'Added code to insert XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        Dim sql As String = String.Format("Insert into location (location,status,putregion,pickregion,warehousearea,locsortorder," &
        "loctpickype,locstoragetype,locusagetype,locmhtype,inventory,lastcountdate,accesstype,accessibleloads,length,width,height," &
        "weight,cubic,loadscapacity,pendingweight,pendingcubic,pendingloads,checkdigits,looseid,aisle,bay,loclevel,heightfromfloor," &
        "lastmovein,lastmoveout,picksfromlastcount,xcoordinate,ycoordinate,zcoordinate,inhandoff,outhandoff,adddate,adduser," &
        "editdate,edituser,locaccessibility,laborhufacing,laborinserttype,laborretrievetype,laborpicktype,laborreachtype,congestionregion," &
        "problemflag,hustoragetemplate,zpickinglocation,zpickingwarehousearea,xfillcordinate,yfillcordinate,zfillcordinate,pickedge,filledge) Values({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16}," &
        "{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41}," &
        "{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56})",
        Made4Net.Shared.FormatField(_Location), Made4Net.Shared.FormatField(_status), Made4Net.Shared.FormatField(_putregion),
        Made4Net.Shared.FormatField(_pickregion), Made4Net.Shared.FormatField(_warehousearea), Made4Net.Shared.FormatField(_locsortorder),
        Made4Net.Shared.FormatField(_loctpickype), Made4Net.Shared.FormatField(_locstoragetype), Made4Net.Shared.FormatField(_locusagetype),
        Made4Net.Shared.FormatField(_locmhtype), Made4Net.Shared.FormatField(_inventory), Made4Net.Shared.FormatField(_lastcountdate),
        Made4Net.Shared.FormatField(_accesstype), Made4Net.Shared.FormatField(_accessibleloads), Made4Net.Shared.FormatField(_length),
        Made4Net.Shared.FormatField(_width), Made4Net.Shared.FormatField(_height), Made4Net.Shared.FormatField(_weight),
        Made4Net.Shared.FormatField(_cubic), Made4Net.Shared.FormatField(_loadscapacity), Made4Net.Shared.FormatField(_pendingweight),
        Made4Net.Shared.FormatField(_pendingcubic), Made4Net.Shared.FormatField(_pendingloads), Made4Net.Shared.FormatField(_checkdigits),
        Made4Net.Shared.FormatField(_looseid), Made4Net.Shared.FormatField(_aisle), Made4Net.Shared.FormatField(_bay),
        Made4Net.Shared.FormatField(_loclevel), Made4Net.Shared.FormatField(_heightfromfloor), Made4Net.Shared.FormatField(_lastmovein),
        Made4Net.Shared.FormatField(_lastmoveout), Made4Net.Shared.FormatField(_picksfromlastcount), Made4Net.Shared.FormatField(_xcoordinate),
        Made4Net.Shared.FormatField(_ycoordinate), Made4Net.Shared.FormatField(_zcoordinate), Made4Net.Shared.FormatField(_inhandoff),
        Made4Net.Shared.FormatField(_outhandoff), Made4Net.Shared.FormatField(_adddate), Made4Net.Shared.FormatField(_adduser),
        Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_edituser), Made4Net.Shared.FormatField(_locaccessibility),
        Made4Net.Shared.FormatField(_laborhufacing), Made4Net.Shared.FormatField(_laborinserttype), Made4Net.Shared.FormatField(_laborretrievetype),
        Made4Net.Shared.FormatField(_laborpicktype), Made4Net.Shared.FormatField(_laborreachtype), Made4Net.Shared.FormatField(_congestionregion),
        Made4Net.Shared.FormatField(_problemflag), Made4Net.Shared.FormatField(_hustoragetemplate), Made4Net.Shared.FormatField(_zpickinglocation),
        Made4Net.Shared.FormatField(_zpickingwarehousearea),
        Made4Net.Shared.FormatField(_xfillcordinate), Made4Net.Shared.FormatField(_yfillcordinate), Made4Net.Shared.FormatField(_zfillcordinate),
        Made4Net.Shared.FormatField(_pickedge), Made4Net.Shared.FormatField(_filledge))
        'Added code to insert XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        DataInterface.RunSQL(sql)

    End Sub
    'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
    Public Sub Update(ByVal pStatus As Boolean, ByVal pPutRegion As String, ByVal pPickRegion As String,
    ByVal pLocSortOrder As String, ByVal pLocPickType As String, ByVal pLocStorageType As String,
    ByVal pLocUsageType As String, ByVal pLocMHType As String, ByVal pInventory As Boolean, ByVal pLastCountDate As DateTime,
    ByVal pAccessType As String, ByVal pAccessibleLoads As Integer, ByVal pLength As Decimal, ByVal pWidth As Decimal,
    ByVal pHeight As Decimal, ByVal pWeight As Decimal, ByVal pCubic As Decimal, ByVal pLoadsCapacity As Integer, ByVal pPendingWeight As Decimal,
    ByVal pPendingCubic As Decimal, ByVal pPendingLoads As Decimal, ByVal pCheckDigits As String, ByVal pLooseID As Boolean, ByVal pAisle As String,
    ByVal pBay As String, ByVal pLocLevel As String, ByVal pHeightFromFloor As Decimal, ByVal pLastMoveIn As DateTime, ByVal pLastMoveOut As DateTime,
    ByVal pPicksFromLastCount As Integer, ByVal pXCoordinate As Integer, ByVal pYCoordinate As Integer, ByVal pZCoordinate As Integer,
    ByVal pInHandOff As String, ByVal pOutHandOff As String, ByVal pAddDate As DateTime, ByVal pAddUser As String, ByVal pEditDate As DateTime,
    ByVal pEditUser As String, ByVal pLocAccessibility As String, ByVal pLaborHUFacing As String, ByVal pLaborInsertType As String,
    ByVal pLaborRetrieveType As String, ByVal pLaborPickType As String, ByVal pLaborReachType As String, ByVal pCongestionRegion As String,
    ByVal pProblemFlag As Boolean, ByVal pHUStorageTemplate As String, ByVal pZPickingLocation As String, ByVal pZPickingWarehouseArea As String,
    ByVal pXfillCordinate As Integer, ByVal pYfillCordinate As Integer, ByVal pZfillCordinate As Integer,
    ByVal pPickEdge As Integer, ByVal pFillEdge As Integer)
        'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        _status = pStatus
        _putregion = pPutRegion
        _pickregion = pPickRegion

        _locsortorder = pLocSortOrder
        _loctpickype = pLocPickType
        _locstoragetype = pLocStorageType

        _locusagetype = pLocUsageType
        _locmhtype = pLocMHType
        _inventory = pInventory
        _lastcountdate = pLastCountDate

        _accesstype = pAccessType
        _accessibleloads = pAccessibleLoads
        _length = pLength
        _width = pWidth

        _height = pHeight
        _weight = pWeight
        _cubic = Decimal.Round((pWidth * pLength * pHeight) / 1728, 3, MidpointRounding.AwayFromZero)
        _loadscapacity = pLoadsCapacity
        _pendingweight = pPendingWeight

        _pendingcubic = pPendingCubic
        _pendingloads = pPendingLoads
        _checkdigits = pCheckDigits
        _looseid = pLooseID
        _aisle = pAisle

        _bay = pBay
        _loclevel = pLocLevel
        _heightfromfloor = pHeightFromFloor
        _lastmovein = pLastMoveIn
        _lastmoveout = pLastMoveOut

        _picksfromlastcount = pPicksFromLastCount
        _xcoordinate = pXCoordinate
        _ycoordinate = pYCoordinate
        _zcoordinate = pZCoordinate

        _inhandoff = pInHandOff
        _outhandoff = pOutHandOff
        _adddate = pAddDate
        _adduser = pAddUser
        _editdate = pEditDate

        _edituser = pEditUser
        _locaccessibility = pLocAccessibility
        _laborhufacing = pLaborHUFacing
        _laborinserttype = pLaborInsertType

        _laborretrievetype = pLaborRetrieveType
        _laborpicktype = pLaborPickType
        _laborreachtype = pLaborReachType
        _congestionregion = pCongestionRegion

        _problemflag = pProblemFlag
        _hustoragetemplate = pHUStorageTemplate
        _zpickinglocation = pZPickingLocation
        _zpickingwarehousearea = pZPickingWarehouseArea
        'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        _xfillcordinate = pXfillCordinate
        _yfillcordinate = pYfillCordinate
        _zfillcordinate = pZfillCordinate
        _pickedge = pPickEdge
        _filledge = pFillEdge
        'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203

        validate(True)
        'Added code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        Dim sql As String = String.Format("Update Location Set Status={0},Putregion={1},pickregion={2},locsortorder={3},LOCTPICKYPE={4}," &
        "locstoragetype={5},locusagetype={6},locmhtype={7},inventory={8},lastcountdate={9},accesstype={10},accessibleloads={11}," &
        "length={12},width={13},height={14},weight={15},cubic={16},loadscapacity={17},pendingweight={18},pendingcubic={19},pendingloads={20}," &
        "checkdigits={21},looseid={22},aisle={23},bay={24},loclevel={25},heightfromfloor={26},lastmovein={27},lastmoveout={28}," &
        "picksfromlastcount={29},xcoordinate={30},ycoordinate={31},zcoordinate={32},inhandoff={33},outhandoff={34},adddate={35},adduser={36}," &
        "editdate={37},edituser={38},locaccessibility={39},laborhufacing={40},laborinserttype={41},laborretrievetype={42},laborpicktype={43}," &
        "laborreachtype={44},congestionregion={45},problemflag={46},hustoragetemplate={47},zpickinglocation={48},zpickingwarehousearea={49}, " &
        "xfillcordinate={50},yfillcordinate={51},zfillcordinate={52},pickedge={53},filledge={54} {55}",
        Made4Net.Shared.FormatField(_status), Made4Net.Shared.FormatField(_putregion), Made4Net.Shared.FormatField(_pickregion),
        Made4Net.Shared.FormatField(_locsortorder), Made4Net.Shared.FormatField(_loctpickype), Made4Net.Shared.FormatField(_locstoragetype),
        Made4Net.Shared.FormatField(_locusagetype), Made4Net.Shared.FormatField(_locmhtype), Made4Net.Shared.FormatField(_inventory),
        Made4Net.Shared.FormatField(_lastcountdate), Made4Net.Shared.FormatField(_accesstype), Made4Net.Shared.FormatField(_accessibleloads),
        Made4Net.Shared.FormatField(_length), Made4Net.Shared.FormatField(_width), Made4Net.Shared.FormatField(_height),
        Made4Net.Shared.FormatField(_weight), Made4Net.Shared.FormatField(_cubic), Made4Net.Shared.FormatField(_loadscapacity),
        Made4Net.Shared.FormatField(_pendingweight), Made4Net.Shared.FormatField(_pendingcubic), Made4Net.Shared.FormatField(_pendingloads),
        Made4Net.Shared.FormatField(_checkdigits), Made4Net.Shared.FormatField(_looseid), Made4Net.Shared.FormatField(_aisle),
        Made4Net.Shared.FormatField(_bay), Made4Net.Shared.FormatField(_loclevel), Made4Net.Shared.FormatField(_heightfromfloor),
        Made4Net.Shared.FormatField(_lastmovein), Made4Net.Shared.FormatField(_lastmoveout), Made4Net.Shared.FormatField(_picksfromlastcount),
        Made4Net.Shared.FormatField(_xcoordinate), Made4Net.Shared.FormatField(_ycoordinate), Made4Net.Shared.FormatField(_zcoordinate),
        Made4Net.Shared.FormatField(_inhandoff), Made4Net.Shared.FormatField(_outhandoff), Made4Net.Shared.FormatField(_adddate),
        Made4Net.Shared.FormatField(_adduser), Made4Net.Shared.FormatField(_editdate), Made4Net.Shared.FormatField(_edituser),
        Made4Net.Shared.FormatField(_locaccessibility), Made4Net.Shared.FormatField(_laborhufacing), Made4Net.Shared.FormatField(_laborinserttype),
        Made4Net.Shared.FormatField(_laborretrievetype), Made4Net.Shared.FormatField(_laborpicktype), Made4Net.Shared.FormatField(_laborreachtype),
        Made4Net.Shared.FormatField(_congestionregion), Made4Net.Shared.FormatField(_problemflag), Made4Net.Shared.FormatField(_hustoragetemplate),
        Made4Net.Shared.FormatField(_zpickinglocation), Made4Net.Shared.FormatField(_zpickingwarehousearea),
        Made4Net.Shared.FormatField(_xfillcordinate), Made4Net.Shared.FormatField(_yfillcordinate), Made4Net.Shared.FormatField(_zfillcordinate),
        Made4Net.Shared.FormatField(_pickedge), Made4Net.Shared.FormatField(_filledge), WhereClause)
        'Ended code to update XFILLCORDINATE, YFILLCORDINATE,ZFILLCORDINATE,PickEdge,FilledEdge - RWMS-1203
        DataInterface.RunSQL(sql)

    End Sub

    Public Sub CancelReplenishByLocation(ByVal pLocation As String, ByVal pUser As String, ByVal pSKU As String)
        Dim _unitsallocated As Decimal = 0
        _editdate = DateTime.Now
        _edituser = pUser
        Dim _activitystatus As String = Nothing
        Dim _destinationlocation As String = ""
        Dim _destinationwarehousearea As String = ""

        Dim sql As String = String.Format("update loads set DESTINATIONLOCATION = {0},DESTINATIONWAREHOUSEAREA = {5},ACTIVITYSTATUS = {1}, UNITSALLOCATED = {2}, EDITDATE = {3},EDITUSER = {4} where DESTINATIONLOCATION = '{6}' AND SKU = {7}",
                Made4Net.Shared.Util.FormatField(_destinationlocation), Made4Net.Shared.Util.FormatField(_activitystatus), Made4Net.Shared.Util.FormatField(_unitsallocated), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_destinationwarehousearea), pLocation, pSKU)
        DataInterface.RunSQL(sql)
    End Sub

    Private Sub validate(ByVal pEdit As Boolean)

        If Not Warehouse.ValidateWareHouseArea(_warehousearea) Then
            Throw New M4NException(New Exception, "Warehouse area does not exist", "Warehouse area does not exist")
        End If

        If pEdit Then
            If Not Exists(_Location, _warehousearea) Then
                Throw New M4NException(New Exception, "Location does not exist.", "Location does not exist.")
            End If
        Else
            If Exists(_Location, _warehousearea) Then
                Throw New M4NException(New Exception, "Can not add location - it already exists.", "Can not add location - it already exists.")
            End If
        End If

        'If _inhandoff <> String.Empty And Not handOffExists(_inhandoff) Then
        '    Throw New M4NException(New Exception, "In handoff does not exist", "In handoff does not exist")
        'End If
        'If _outhandoff <> String.Empty And Not handOffExists(_outhandoff) Then
        '    Throw New M4NException(New Exception, "Out handoff does not exist", "Out handoff does not exist")
        'End If
        'If _zpickingwarehousearea <> String.Empty And _zpickinglocation = String.Empty Then
        '    Throw New M4NException(New Exception, "Z picking location is not valid", "Z picking location is not valid")
        'End If
        'If _zpickinglocation <> String.Empty And Not Exists(_zpickinglocation, _zpickingwarehousearea) Then
        '    Throw New M4NException(New Exception, "Z picking does not exist", "Z picking does not exist")
        'End If

        If _hustoragetemplate <> String.Empty And Not handelingUnitStorageTemplateExists(_hustoragetemplate) Then
            Throw New M4NException(New Exception, "Handeling unit storage template does not exist.", "Handeling unit storage template does not exist.")
        End If

    End Sub

    Private Function handOffExists(ByVal pHandOffLocation As String) As Boolean
        Dim sql As String = String.Format("select count(1) from handoff where handofflocation ={0}", Made4Net.Shared.FormatField(pHandOffLocation))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Private Function handelingUnitStorageTemplateExists(ByVal pHUStorageTemplate As String)
        Dim sql As String = String.Format("select count(1) from handlingunitstoragetemplate where HUSTORAGETEMPLATEID={0}", Made4Net.Shared.FormatField(pHUStorageTemplate))
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function
#End Region

#Region "Location Assignment"

#Region "Load Putaway"

    'this method gets a load and checks if its possible to place the load in the location(returns True)
    Private Shared Function CanPlace(ByVal dr As DataRow, ByVal ld As Load, ByVal sLoadHandlingUnitType As String, ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByPalletType As Boolean, ByVal pContentType As String, ByVal pDtContent As DataTable, ByVal dtHUTemplates As DataTable, ByVal dtHULocContent As DataTable, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim looseid As Boolean = False
        Dim LocHUTemplate, SQL As String
        Dim volume, weight, NumLoads, pendingloads, pendingWeight, pendingVol, height As Double
        Dim locationCubicLimit, loadCubic As Double
        Dim locationWeightLimit, loadWeight As Double
        Dim loadCappacity As Int32 = dr("LOADSCAPACITY")
        Dim Loc As String = dr("LOCATION")
        Dim Warehousearea As String = dr("WAREHOUSEAREA")
        volume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TotalVolume"), 0)
        weight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TotalWeight"), 0)
        height = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Height"), 0)
        NumLoads = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("NumLoads"), 0)
        pendingloads = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PendingLoads"), 0)
        pendingVol = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PendingVolume"), 0)
        pendingWeight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PendingWeight"), 0)
        looseid = dr("looseid")
        locationCubicLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CUBIC"), 0)
        locationWeightLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WEIGHT"), 0)
        LocHUTemplate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("hustoragetemplate"))

        'Check for Handling Unit Type Allowed combinations
        If LocHUTemplate <> "" AndAlso sLoadHandlingUnitType <> "" Then
            If Not ValidateLocationHU(LocHUTemplate, Loc, Warehousearea, sLoadHandlingUnitType, dtHUTemplates, dtHULocContent) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Failed to match Location Handling Unit Type template....")
                End If
                Return False
            End If
        End If
        'check if we can put some more loads in the location
        If loadCappacity <= NumLoads + System.Convert.ToInt32(pendingloads) Then
            If Not looseid Then
                'then we can merge for now with current loads; in the future - check for attributes
                If Not oLogger Is Nothing Then
                    oLogger.Write("Load Capacity exceeded: Load Capacity = " & loadCappacity & ", Number of Loads = " & NumLoads & ", Number of Pending Loads = " & pendingloads)
                End If
                Return False
            Else
                'check if attribute will allow to merge the loads when moving to the location
                'if on load from the same sku will allow to merge - first load from same sku and check
                Dim mergeFound As Boolean = False
                SQL = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & ld.SKU & "' and consignee = '" & ld.CONSIGNEE & "')"
                For Each currLoad As DataRow In pDtContent.Select(SQL)
                    Dim tmpLoad As New WMS.Logic.Load(currLoad("loadid").ToString())
                    tmpLoad.ACTIVITYSTATUS = ""
                    If WMS.Logic.Merge.CanMerge(tmpLoad, ld) Then
                        mergeFound = True
                        Exit For
                    End If
                Next
                If Not mergeFound Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Load Capacity exceeded (no possible merges found): Load Capacity = " & loadCappacity & ", Number of Loads = " & NumLoads & ", Number of Pending Loads = " & pendingloads)
                    End If
                    Return False
                End If
                'and then check loc content according to policy.....
                If Not CheckLocContent(dr, ld, pContentType, pDtContent) Then
                    Return False
                End If
            End If
        End If
        'check for the cubic of the location
        If pFitByVolume Then
            loadCubic = ld.Volume
            If locationCubicLimit < (volume + pendingVol + loadCubic) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Load Volume exceeded: Cube limit = " & locationCubicLimit & ", Total Volume = " & volume & ", Pending Volume = " & pendingVol & ", Current Load Volume = " & loadCubic)
                End If
                Return False
            End If
        End If
        'check for the weight of the location
        If pFitByWeight Then
            loadWeight = ld.Weight
            If locationWeightLimit < (weight + pendingWeight + loadWeight) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Load Volume exceeded: Weight limit = " & locationWeightLimit & ", Total Weight = " & weight & ", Pending Weight = " & pendingWeight & ", Current Load Weight = " & loadWeight)
                End If
                Return False
            End If
        End If
        If pFitByHeight Then
            'Check For the load height
        End If
        'we passed all checks
        Return True
    End Function

    'this function gets a load id and a put region and returnd the name of the
    'location which the load fit into.
    Public Shared Function LocToPlace(ByRef Location As String, ByRef Warehousearea As String, ByVal pPutawayPolicyId As String, ByVal ld As Load, ByVal pPutRegion As String,
            ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByPalletType As Boolean,
            ByVal pNumberOfPalletsPerPutregion As Integer,
            ByVal operatorsEqpHeight As Double,
            Optional ByVal pStorageType As String = "",
            Optional ByVal pContent As String = "", Optional ByVal pQtyToPlace As Decimal = -1,
            Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing, Optional ByVal pLocation As String = "",
            Optional ByVal pWarehouseArea As String = "") As String

        Dim S As WMS.Logic.SKU = New SKU()


        Dim PutAwayScoring As New WMS.Logic.Putaway.PutAwayScoring(pPutawayPolicyId)
        Dim loc As String
        Dim dtLocations As New DataTable
        Dim dtLocContent As New DataTable
        Dim dtHUTemplates As New DataTable
        Dim dtHULocContent As DataTable
        Dim dtLoad As New DataTable
        Dim dr As DataRow
        Dim IsFit As Boolean = False
        Dim sameContent As Boolean = False
        If pQtyToPlace > 0 Then
            ld.ReCalculateVolumeAndWeight(pQtyToPlace)
        End If

        ' Check the limit of pallets per put region
        'If Not ValidateNumberOfPallets(pPutRegion, ld.CONSIGNEE, ld.SKU, pNumberOfPalletsPerPutregion) Then
        '    Location = String.Empty
        '    Warehousearea = String.Empty
        '    Return String.Empty
        'End If
        'dtLocations = GetRegioLocationTable(ld, pPutRegion, pStorageType, pFitByVolume, pFitByWeight, pContent, oLogger, pLocation)
        'Added for RWMS-1683 and RWMS-1656 - Passing Max Number of Pallets
        dtLocations = GetRegioLocationTable(ld, pPutRegion, pStorageType, pFitByVolume, pFitByWeight, pContent, pNumberOfPalletsPerPutregion, oLogger, pLocation)
        'Ended for RWMS-1683 and RWMS-1656 - Passing Max Number of Pallets
        If dtLocations.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No available Locations found in region....")
            End If
            Location = String.Empty
            Warehousearea = String.Empty
        End If

        Dim enableLogging As Integer = IIf(String.IsNullOrEmpty(WMS.Logic.GetSysParam("LOGLOCHEIGHTVALIDATION")), 0, System.Convert.ToInt32(WMS.Logic.GetSysParam("LOGLOCHEIGHTVALIDATION")))
        If pFitByHeight AndAlso Not enableLogging = 0 Then
            oLogger.Write("Validating location for fit by height.")
        End If

        Dim dtScoredLocation As DataRow()
        If (pFitByHeight) Then
            dtScoredLocation = dtLocations.AsEnumerable().Where(Function(row) ValidateHeight(pFitByHeight, row, enableLogging, oLogger)).ToArray()
        Else
            dtScoredLocation = dtLocations.Select()
        End If
        dtLocContent = GetRegioLoadsTable(pPutRegion, oLogger) 'GetLocationContentTable(pPutRegion, oLogger)
        dtHUTemplates = GetHUStorageTemplates(oLogger)

        ' Distance Applied
        ' Check if the policy uses the distance
        If PutAwayScoring.DoesAttributeExistsOrHasValue("DISTANCE") Then
            Dim pickLoc As String

            Dim pickLocFrontRackLoc As String
            pickLocFrontRackLoc = ""
            'pickLoc = DistanceApplied.GetPickLocationForSKU(ld, oLogger)
            pickLoc = DistanceApplied.GetPickLocAndFrontRackLocForSKU(ld, pickLocFrontRackLoc, oLogger)
            If Len(pickLocFrontRackLoc) > 0 Then
                pickLoc = pickLocFrontRackLoc
            End If
            If pickLoc IsNot Nothing And pickLoc <> String.Empty Then
                DistanceApplied.UpdateWithActualDistance(pickLoc, dtScoredLocation, operatorsEqpHeight, oLogger)
            End If
        End If
        ' Distance Applied
        'RWMS-2727
        'Duplicate code , commented out, since we have implemented putway scoring methods
        'Try
        '    If Not oLogger Is Nothing Then
        '        oLogger.Write("Started Scoring the locations WITH Applied Distance.[Original Scoring Algorithm Implementation]")
        '    End If
        '    Dim watch As System.Diagnostics.Stopwatch = Stopwatch.StartNew()
        '    PutAwayScoring.Score(dtScoredLocation, oLogger)
        '    watch.Stop()
        '    If Not oLogger Is Nothing Then
        '        oLogger.Write(String.Format("Finished Scoring the locations.[Original Scoring Algorithm Implementation] Time taken : {0} Milliseconds", watch.Elapsed.TotalMilliseconds))
        '    End If
        'Catch ex As Exception
        '    If Not oLogger Is Nothing Then
        '        oLogger.Write("Exception in Scoring Location" & ex.Message)
        '    End If
        'End Try
        'RWMS-2727

        If Not oLogger Is Nothing Then
            oLogger.Write("Total Location Found: " & dtLocations.Rows.Count)
        End If

        ' New Scoring Algorithm Implementation
        Dim locationList As List(Of LocationDTO) = PutAwayScoreSetter.ConvertLocationDataRowToDTOs(dtScoredLocation)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("{0} After ConvertLocationDataRowToDTOs(): Convert all the location DataRow to DTO ", DateTime.Now.ToString()))
        End If


        PutAwayScoring.Score(locationList, oLogger)

        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("{0} After PutAwayScoring.Score():  Scoring the locations.[New Scoring Algorithm Implementation]", DateTime.Now.ToString()))
        End If

        Dim convertedDt As DataTable = PutAwayScoreSetter.ConvertDTOToDataTable(locationList)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("{0} ConvertDTOToDataTable(): Convert all the location DTO back to DataTable", DateTime.Now.ToString()))
        End If

        Dim dtScoredLocationNew As DataRow() = convertedDt.Select()
        ' New Scoring Algorithm Implementation

        Dim tmpLocation, tmpWarehousearea As String
        'dtLoad = GetRegioLoadsTable(pPutRegion, oLogger)
        'now that we have all locations in region,find the one to place the load in
        Dim sLoadHandlingUnitType As String
        If ld.ContainerId <> String.Empty Then
            sLoadHandlingUnitType = DataInterface.ExecuteScalar(String.Format("select isnull(HUTYPE,'') as HUTYPE from CONTAINER where CONTAINER = '{0}'", ld.ContainerId))
        End If
        For i As Int32 = 0 To dtScoredLocationNew.Length - 1
            dr = dtScoredLocationNew(i)
            tmpLocation = dr("location")
            tmpWarehousearea = dr("warehousearea")
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator("-", 50)
                oLogger.Write("Inspecting location " & tmpLocation)
            End If
            Dim LocHUTemplate As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("hustoragetemplate"))
            If dtHULocContent Is Nothing And LocHUTemplate <> "" Then
                dtHULocContent = GetHULocationContent(pPutRegion, oLogger)
            End If

            Dim isThreading As Boolean = False
            Try
                isThreading = System.Convert.ToBoolean(System.Convert.ToInt32(Made4Net.Shared.AppConfig.Get("isThreading", "0")))

                If isThreading Then
                    DataInterface.BeginTransaction()
                    Dim sql As String = String.Format("update LOCATION with (HOLDLOCK, ROWLOCK) set PROBLEMFLAG=PROBLEMFLAG  where LOCATION='{0}' an WAREHOUSEAREA='{1}' ", dr.Item("LOCATION"), dr.Item("WAREHOUSEAREA"))
                    DataInterface.RunSQL(sql)
                End If

                IsFit = CanPlace(dr, ld, sLoadHandlingUnitType, pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType, pContent, dtLocContent, dtHUTemplates, dtHULocContent, oLogger)
                If IsFit Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Location is suitable for receiving the load, inspecting content....")
                    End If
                    'need to check the location content
                    sameContent = CheckLocContent(dr, ld, pContent, dtLocContent)
                    If sameContent Then
                        Location = tmpLocation
                        Warehousearea = tmpWarehousearea
                        If Not oLogger Is Nothing Then
                            oLogger.writeSeperator("*", 50)
                            oLogger.Write("Location Found: " & Location)
                            oLogger.writeSeperator("*", 50)
                        End If
                        Return ""
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Location is not suitable for receiving the load, Content type mismatch...")
                        End If
                    End If
                End If
            Catch ex As Exception
            Finally
                If isThreading Then
                    DataInterface.CommitTransaction()
                End If
            End Try
        Next
        Return String.Empty
    End Function

    'this function gets a list of load id and a put region and returnd the name of the
    'location in which all the loads fit into.
    Public Shared Function LocToPlaceMulti(ByRef Location As String, ByRef Warehousearea As String, ByVal pPutawayPolicyId As String, ByVal loadsWithSequence As List(Of LoadsWithSequence), ByVal pPutRegion As String,
            ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByPalletType As Boolean,
            ByVal pNumberOfPalletsPerPutregion As Integer, ByVal assignedLoads As List(Of LoadsWithSequence),
            ByVal operatorsEqpHeight As Double,
            Optional ByVal pStorageType As String = "",
            Optional ByVal pContent As String = "", Optional ByVal pQtyToPlace As Decimal = -1,
            Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing, Optional ByVal pLocation As String = "",
            Optional ByVal pWarehouseArea As String = "") As String

        Dim S As WMS.Logic.SKU = New SKU()


        Dim PutAwayScoring As New WMS.Logic.Putaway.PutAwayScoring(pPutawayPolicyId)
        Dim loc As String
        Dim dtLocations As New DataTable
        Dim dtLocContent As New DataTable
        Dim dtHUTemplates As New DataTable
        Dim dtHULocContent As DataTable
        Dim dtLoad As New DataTable
        Dim dr As DataRow
        Dim IsFit As Boolean = False
        Dim sameContent As Boolean = False
        Dim containerHeight As Double
        ' Note pQtyToPlace always gets passed as -1, which is never greater than 0. Need to see whether recalculating volume is mandatory in case of multi-payload putaway.
        If pQtyToPlace > 0 Then
            For Each ld As LoadsWithSequence In loadsWithSequence
                ld.Load.ReCalculateVolumeAndWeight(pQtyToPlace)
            Next
        End If
        'Getting Container height
        containerHeight = WMS.Logic.Location.ContainerHeight ' RWMS-1200

        ' Check the limit of pallets per put region for all the loads.
        If Not ValidateNumberOfPalletsForMultipleLoads(pPutRegion, loadsWithSequence, pNumberOfPalletsPerPutregion) Then
            Location = String.Empty
            Warehousearea = String.Empty
            Return String.Empty
        End If

        ' Call to VPutAway
        ' This method is modified to get common elligible locations(Intersection) for all the loads
        dtLocations = GetRegioLocationTableForMultipleLoads(loadsWithSequence, pPutRegion, pStorageType, pFitByVolume, pFitByWeight, pContent, oLogger, pLocation)

        If dtLocations.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No available Locations found in region....")
            End If
            Location = String.Empty
            Warehousearea = String.Empty
        End If

        ' Gets the existing content in the putregion
        dtLocContent = GetRegioLoadsTable(pPutRegion, oLogger) 'GetLocationContentTable(pPutRegion, oLogger)
        ' ??
        dtHUTemplates = GetHUStorageTemplates(oLogger)

        Dim dtScoredLocation As DataRow() = dtLocations.Select()

        ' Distance Applied
        If PutAwayScoring.DoesAttributeExistsOrHasValue("DISTANCE") Then
            Dim pickLoc As String
            Dim pickLocFrontRackLoc As String
            pickLocFrontRackLoc = ""
            ' Consider the pick location for first load as scanned in sequence fro distance calculation.
            'pickLoc = DistanceApplied.GetPickLocationForSKU(loadsWithSequence.First().Load, oLogger)
            pickLoc = DistanceApplied.GetPickLocAndFrontRackLocForSKU(loadsWithSequence.First().Load, pickLocFrontRackLoc, oLogger)
            If Len(pickLocFrontRackLoc) > 0 Then
                pickLoc = pickLocFrontRackLoc
            End If
            If pickLoc IsNot Nothing And pickLoc <> String.Empty Then
                DistanceApplied.UpdateWithActualDistance(pickLoc, dtScoredLocation, operatorsEqpHeight, oLogger)
            End If
        End If
        ' Distance Applied

        Try
            If Not oLogger Is Nothing Then
                oLogger.Write("Started Scoring the locations WITH Applied Distance.")
            End If
            Dim watch As System.Diagnostics.Stopwatch = Stopwatch.StartNew()
            PutAwayScoring.Score(dtScoredLocation, oLogger)
            watch.Stop()

            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Finished Scoring the locations. Time taken : {0} Milliseconds", watch.Elapsed.TotalMilliseconds))
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Exception in Scoring Location" & ex.Message)
            End If
        End Try

        If Not oLogger Is Nothing Then
            oLogger.Write("Total Location Found: " & dtLocations.Rows.Count)
        End If


        Dim tmpLocation, tmpWarehousearea As String

        ' Since this is a case of Multiple Loads, get all the valid Handling Unit Type and store in the list to be used in the validation for placing.
        Dim sLoadHandlingUnitTypeList As List(Of String)
        For Each ld As LoadsWithSequence In loadsWithSequence
            If ld.Load.ContainerId <> String.Empty Then
                sLoadHandlingUnitTypeList.Add(DataInterface.ExecuteScalar(String.Format("select isnull(HUTYPE,'') as HUTYPE from CONTAINER where CONTAINER = '{0}'", ld.Load.ContainerId)))
            End If
        Next
        '' Start RWMS-1200
        Dim multipayloadsdao = New MultipayloadsDao
        If MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads < 0 Or MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads < 0 Or MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads < 0 Then
            MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads = 0
            MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads = 0
            MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads = 0
            For Each ldseq As LoadsWithSequence In loadsWithSequence
                MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads = ldseq.Load.Volume + MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads
                MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads = ldseq.Load.GrossWeight + MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads
                Dim ldHeight As Integer = If(pFitByWeight, multipayloadsdao.GetLoadsHeight(ldseq.Load.LOADID), 0)
                MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads = MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads + ldHeight + containerHeight
            Next
        End If
        '' End RWMS-1200
        'now that we have all common locations for all loads in region, find the one to place the loads into
        ' Put the locations processed in a list, so that it is not processe more than once time.
        Dim ProcessedLinesByPriority As New System.Collections.ArrayList()

        For i As Int32 = 0 To dtScoredLocation.Length - 1

            dr = dtScoredLocation(i)

            tmpLocation = dr("location")
            tmpWarehousearea = dr("warehousearea")

            If Not ProcessedLinesByPriority.Contains(tmpLocation) Then

                ' Avoid duplicate processing
                ProcessedLinesByPriority.Add(tmpLocation)

                If Not oLogger Is Nothing Then
                    oLogger.writeSeperator("-", 50)
                    oLogger.Write("Inspecting location " & tmpLocation)
                End If

                Dim LocHUTemplate As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("hustoragetemplate"))

                '?? Ignored since LocHUTemplate is always an empty string
                If dtHULocContent Is Nothing And LocHUTemplate <> "" Then
                    dtHULocContent = GetHULocationContent(pPutRegion, oLogger)
                End If
                '?? Ignored

                Dim isThreading As Boolean = False
                Try
                    isThreading = System.Convert.ToBoolean(System.Convert.ToInt32(Made4Net.Shared.AppConfig.Get("isThreading", "0")))

                    If isThreading Then
                        DataInterface.BeginTransaction()
                        Dim sql As String = String.Format("update LOCATION with (HOLDLOCK, ROWLOCK) set PROBLEMFLAG=PROBLEMFLAG  where LOCATION='{0}' an WAREHOUSEAREA='{1}' ", dr.Item("LOCATION"), dr.Item("WAREHOUSEAREA"))
                        DataInterface.RunSQL(sql)
                    End If

                    'IsFit = CanPlace(dr, ld, sLoadHandlingUnitType, pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType, pContent, dtLocContent, dtHUTemplates, dtHULocContent, oLogger)
                    IsFit = MultiPayloadPutawayHelper.CanPlaceLoads(dr,
                                                                    loadsWithSequence,
                                                                    sLoadHandlingUnitTypeList,
                                                                    pFitByVolume,
                                                                    pFitByWeight,
                                                                    pFitByHeight,
                                                                    pFitByPalletType,
                                                                    pContent,
                                                                    dtLocContent,
                                                                    dtHUTemplates,
                                                                    dtHULocContent,
                                                                    assignedLoads,
                                                                    containerHeight,
                                                                    oLogger)
                    If IsFit Then
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Location is suitable for receiving all the loads, now inspecting exting contents....")
                        End If
                        'need to check the location content
                        ' Check for all loads
                        sameContent = CheckLocContentWithMultipleLoads(dr, loadsWithSequence, pContent, dtLocContent)
                        If sameContent Then
                            Location = tmpLocation
                            Warehousearea = tmpWarehousearea
                            If Not oLogger Is Nothing Then
                                oLogger.writeSeperator("*", 50)
                                oLogger.Write("Location Found: " & Location)
                                oLogger.writeSeperator("*", 50)
                            End If
                            MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads = -1
                            MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads = -1
                            MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads = -1
                            Return ""
                        Else
                            If Not oLogger Is Nothing Then
                                oLogger.Write("Location is not suitable for receiving the load, Content type mismatch...")
                            End If
                        End If
                    End If
                Catch ex As Exception
                Finally
                    If isThreading Then
                        DataInterface.CommitTransaction()
                    End If
                End Try
            End If
        Next
        MultiPayloadPutawayHelper.PreCalculatedHeightOfAllLoads = -1
        MultiPayloadPutawayHelper.PreCalculatedVolumeOfAllLoads = -1
        MultiPayloadPutawayHelper.PreCalculatedWeightOfAllLoads = -1
        Return String.Empty

    End Function

    Private Shared Function ValidateNumberOfPallets(ByVal pPutregion As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pMaxPallets As Int32, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing)
        If pMaxPallets < 0 Then Return True

        Dim sSql As String = String.Format("select numpallets from vPutRegionPallets where putregion in ({0}) and CONSIGNEE='{1}' and SKU='{2}'",
                        pPutregion, pConsignee, pSku)
        Dim iNumPallets As Int32 = DataInterface.ExecuteScalar(sSql)
        If iNumPallets < pMaxPallets Then
            Return True
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Maximum number of pallets exceeded for the current putregion / sku ....")
                oLogger.Write(String.Format("Put region: {0}", pPutregion))
                oLogger.Write(String.Format("Consignee / SKU: {0}, {1}", pConsignee, pSku))
                oLogger.Write(String.Format("Total pallets in region / Policy pallet limit: {0} / {1}", iNumPallets, pMaxPallets))
            End If
            Return False
        End If
    End Function

    Private Shared Function ValidateNumberOfPalletsForMultipleLoads(ByVal pPutregion As String, ByVal loadsWithSequence As IList(Of LoadsWithSequence), ByVal pMaxPallets As Int32, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing)
        If pMaxPallets < 0 Then Return True
        Dim consineeList As String
        Dim skuList As String

        'Make  the lists
        consineeList = String.Join(",", loadsWithSequence.Select(Function(x) x.Load.CONSIGNEE))
        consineeList = "'" + consineeList.Replace(",", "','") + "'"

        skuList = String.Join(",", loadsWithSequence.Select(Function(x) x.Load.SKU))
        skuList = "'" + skuList.Replace(",", "','") + "'"

        Dim sSql As String = String.Format("select sum(numpallets) from vPutRegionPallets where putregion in ('{0}') and CONSIGNEE in ({1}) and SKU in ({2})",
                        pPutregion, consineeList, skuList)
        Dim iNumPallets As Int32 = DataInterface.ExecuteScalar(sSql)
        If iNumPallets < pMaxPallets Then
            Return True
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Maximum number of pallets exceeded for the current putregion / sku ....")
                oLogger.Write(String.Format("Put region: {0}", pPutregion))
                oLogger.Write(String.Format("Consignee / SKU: {0}, {1}", consineeList, skuList))
                oLogger.Write(String.Format("Total pallets in region / Policy pallet limit: {0} / {1}", iNumPallets, pMaxPallets))
            End If
            Return False
        End If
    End Function
    'Added for RWMS-1683 and RWMS-1656 - Passing Max Number of Pallets
    Private Shared Function GetRegioLocationTable(ByVal ld As Load, ByVal pPutRegion As String, ByVal pStorageType As String, ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pContent As String, ByVal pMaxPallets As Int32, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing, Optional ByVal pLocation As String = "", Optional ByVal pWarehousearea As String = "") As DataTable
        'Ended for RWMS-1683 and RWMS-1656 - Passing Max Number of Pallets
        Dim dt As New DataTable
        oLogger.Write("Putregion : " & pPutRegion)
        Dim sqlQuery As String = String.Format("select * from vPutAway ")
        Dim lstConditions As New List(Of String)
        If (Not String.IsNullOrEmpty(pPutRegion)) Then
            lstConditions.Add("PUTREGION = " + Made4Net.Shared.FormatField(pPutRegion))
        End If
        If (Not String.IsNullOrEmpty(pLocation)) And (Not String.IsNullOrEmpty(pWarehousearea)) Then
            lstConditions.Add("LOCATION = " + Made4Net.Shared.FormatField(pLocation))
            lstConditions.Add("WAREHOUSEAREA = " + Made4Net.Shared.FormatField(pWarehousearea))
        End If
        If (Not String.IsNullOrEmpty(pStorageType)) Then
            lstConditions.Add("LOCSTORAGETYPE = " + Made4Net.Shared.FormatField(pStorageType))
        End If
        If pFitByVolume Then
            lstConditions.Add(String.Format("CUBIC >=  REPLACE('{0}',',','.')", ld.Volume))
        End If
        If pFitByWeight Then
            lstConditions.Add(String.Format("WEIGHT >= REPLACE('{0}',',','.')", ld.Weight))

        End If
        Dim LocContentFilter As String = BuildLocContentFilter(pContent, pPutRegion, ld.SKU)
        If LocContentFilter <> "" Then
            lstConditions.Add(String.Format("LOCATION  IN ({0})", LocContentFilter))

        End If
        'Added for RWMS-1683 and RWMS-1656 - Passing Max Number of Pallets
        If pMaxPallets > 0 Then
            lstConditions.Add(String.Format("AISLEPALLETS < ({0})", pMaxPallets))
        End If
        'Added for RWMS-1683 and RWMS-1656 - Passing Max Number of Pallets
        lstConditions.Add("LOADID = " + Made4Net.Shared.FormatField(ld.LOADID))
        If lstConditions.Any() Then
            sqlQuery += " WHERE " + String.Join(" AND ", lstConditions.ToArray())
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Get All Locations for putregion(s): " & pPutRegion)
            oLogger.Write("SQL : " & sqlQuery)
        End If
        DataInterface.FillDataset(sqlQuery, dt)
        Return dt
    End Function

    Private Shared Function GetRegioLocationTableForMultipleLoads(ByVal loadsWithSequence As List(Of LoadsWithSequence), ByVal pPutRegion As String, ByVal pStorageType As String, ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pContent As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing, Optional ByVal pLocation As String = "", Optional ByVal pWarehousearea As String = "") As DataTable
        Dim dt As New DataTable
        Dim ldsVolume, ldsWeight As Double
        Dim SQL As String = String.Empty
        Dim count As Integer = 1

        oLogger.Write("Putregion : " & pPutRegion)

        For Each ldseq As LoadsWithSequence In loadsWithSequence
            ldsVolume = ldseq.Load.Volume + ldsVolume
            ldsWeight = ldseq.Load.GrossWeight + ldsWeight
        Next
        ' Gets the intersection of elligible location for individual loads. The below forms a single query to do this.
        For Each ldseq As LoadsWithSequence In loadsWithSequence
            SQL = SQL + FormRegioLocationQuery(ldseq.Load,
                                         loadsWithSequence,
                                         ldsVolume,
                                         ldsWeight,
                                         pPutRegion,
                                         pStorageType,
                                         pFitByVolume,
                                         pFitByWeight,
                                         pContent,
                                         oLogger,
                                         pLocation,
                                         pWarehousearea)

            If count < loadsWithSequence.Count Then
                SQL = SQL + " intersect "
            End If
            count += 1
        Next

        SQL = String.Format("Select * from vMultiPutAway Where Location in ({0})", SQL)
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Get All Locations for putregion(s): " & pPutRegion)
            oLogger.Write("SQL(Multiple) : " & SQL)
        End If
        ' Will have duplicate location, remember to filter
        DataInterface.FillDataset(SQL, dt)

        Return dt
    End Function

    ' Forms query for getting locations for a single load
    Private Shared Function FormRegioLocationQuery(ByVal ld As Load, ByVal loadsWithSequence As List(Of LoadsWithSequence), ByVal ldsVolume As Double, ByVal ldsWeight As Double, ByVal pPutRegion As String, ByVal pStorageType As String, ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pContent As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing, Optional ByVal pLocation As String = "", Optional ByVal pWarehousearea As String = "") As String
        Dim SQL As String = String.Format("select LOCATION from vMultiPutAway ")
        If (Not String.IsNullOrEmpty(pPutRegion)) Then
            SQL += String.Format("WHERE PUTREGION = '{0}' ", pPutRegion)
        End If
        If (Not String.IsNullOrEmpty(pLocation)) And (Not String.IsNullOrEmpty(pWarehousearea)) Then
            SQL += String.Format("AND Location = '{0}' AND WAREHOUSEAREA = '{1}' ", pLocation, pWarehousearea)
        End If
        If (Not String.IsNullOrEmpty(pStorageType)) Then
            SQL += String.Format("AND LOCSTORAGETYPE = '{0}' ", pStorageType)
        End If
        If pFitByVolume Then
            SQL += String.Format(" AND CUBIC >=  REPLACE('{0}',',','.')", ldsVolume)
        End If
        If pFitByWeight Then
            SQL += String.Format(" AND WEIGHT >= REPLACE('{0}',',','.')", ldsWeight)
        End If
        Dim LocContentFilter As String = BuildLocContentFilterForMultipleSKUs(pContent, pPutRegion, loadsWithSequence)
        If LocContentFilter <> "" Then
            SQL += String.Format(" And location in ({0})", LocContentFilter)
        End If
        SQL += String.Format(" And loadid = '{0}'", ld.LOADID)
        Return SQL
    End Function

#End Region

#Region "Container Putaway"
    Private Shared Function ValidateHeight(fitByHeight As Boolean, dr As DataRow, enableLogging As Integer, Optional logger As ILogHandler = Nothing) As Boolean

        Dim locationHeightLimit, pendingLoadsHeight, loadHeight, height As Double
        locationHeightLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("Height"), 0)
        pendingLoadsHeight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PENDINGLOADSHEIGHT"), 0)
        loadHeight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PWPENDINGLOADSHEIGHT"), 0)
        height = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TotalHeight"), 0)
        Dim retVal As Boolean = locationHeightLimit >= height + pendingLoadsHeight + loadHeight

        If enableLogging = 1 Then
            logger.SafeWrite($" Height validation for location { dr("Location")} { IIf(retVal, " passed", " failed") }.Condition: Location Height ({locationHeightLimit}) >= HEIGHT ({height}) + PENDINGLOADSHEIGHT ({pendingLoadsHeight}) + PWPENDINGLOADSHEIGHT ({loadHeight})  ,{ IIf(retVal, " passed", " failed") }")
        End If
        Return retVal
    End Function
    Private Shared Function CanPlace(ByVal dr As DataRow, ByVal oCont As Container, ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByPalletType As Boolean, ByVal dtHUTemplates As DataTable, ByVal dtHULocContent As DataTable, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        Dim looseid As Boolean = False
        Dim LocHUTemplate As String
        Dim volume, weight, pendingWeight, pendingVol As Double
        Dim locationCubicLimit, ContainerCubic As Double
        Dim locationWeightLimit, ContainerWeight As Double

        Dim Loc As String = dr("LOCATION")
        Dim Warehousearea As String = dr("warehousearea")
        volume = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TotalVolume"), 0)
        weight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TotalWeight"), 0)
        pendingVol = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PendingVolume"), 0)
        pendingWeight = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("PendingWeight"), 0)
        locationCubicLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CUBIC"), 0)
        locationWeightLimit = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("WEIGHT"), 0)
        LocHUTemplate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("hustoragetemplate"))

        'Check for Handling Unit Type Allowed combinations
        If LocHUTemplate <> "" And oCont.HandlingUnitType <> "" Then
            If Not ValidateLocationHU(LocHUTemplate, Loc, Warehousearea, oCont.HandlingUnitType, dtHUTemplates, dtHULocContent, oLogger) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Failed to match Location Handling Unit Type template....")
                End If
                Return False
            End If
        End If
        'now check for the cubic of the location
        If pFitByVolume Then
            ContainerCubic = oCont.Volume
            If locationCubicLimit < (volume + pendingVol + ContainerCubic) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Container Volume exceeded: Cube limit = " & locationCubicLimit & ", Total Volume = " & volume & ", Pending Volume = " & pendingVol & ", Current Load Volume = " & ContainerCubic)
                End If
                Return False
            End If
        End If
        'now check for the weight of the location
        If pFitByWeight Then
            ContainerWeight = oCont.Weight
            If locationWeightLimit < (weight + pendingWeight + ContainerWeight) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Container Weight exceeded: Weight limit = " & locationWeightLimit & ", Total Weight = " & weight & ", Pending Weight = " & pendingWeight & ", Current Load Weight = " & ContainerWeight)
                End If
                Return False
            End If
        End If
        'we passed all checks
        Return True
    End Function

    'this function gets a load id and a put region and returnd the name of thelocation which the container fit into.
    Public Shared Function LocToPlace(ByRef loc As String, ByRef warehousearea As String, ByVal pPutawayPolicyId As String, ByVal oCont As Container, ByVal pPutRegion As String,
            ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pFitByHeight As Boolean, ByVal pFitByPalletType As Boolean,
            ByVal pStorageType As String, ByVal pContent As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As String

        Dim PutAwayScoring As New WMS.Logic.Putaway.PutAwayScoring(pPutawayPolicyId)
        'Dim loc As String
        Dim dtLocations As New DataTable
        Dim dtLocContent As New DataTable
        Dim dtHUTemplates As New DataTable
        Dim dtHULocContent As DataTable
        Dim dtLoad As New DataTable
        Dim dr As DataRow
        Dim IsFit As Boolean = False
        Dim sameContent As Boolean = False

        dtLocations = GetRegioLocationTable(oCont, pPutRegion, pStorageType, pFitByVolume, pFitByWeight, pContent, oLogger)
        If dtLocations.Rows.Count = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No available Locations found in region....")
            End If
            loc = String.Empty
            warehousearea = String.Empty
        End If
        dtLocContent = GetRegioLoadsTable(pPutRegion, oLogger)
        dtHUTemplates = GetHUStorageTemplates(oLogger)
        Dim dtScoredLocation As DataRow() = dtLocations.Select()
        Try
            If Not oLogger Is Nothing Then
                oLogger.Write("Started Scoring the locations.")
            End If
            PutAwayScoring.Score(dtScoredLocation, oLogger)
            If Not oLogger Is Nothing Then
                oLogger.Write("Finished Scoring the locations.")
            End If
        Catch ex As Exception
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Total Location Found: " & dtLocations.Rows.Count)
        End If
        'dtLoad = GetRegioLoadsTable(pPutRegion, oLogger)
        Dim tmpLocation, tmpWarehousearea As String
        'now that we have all locations in region,find the one to place the load in
        For i As Int32 = 0 To dtScoredLocation.Length - 1
            dr = dtScoredLocation(i)
            tmpLocation = dr("location")
            tmpWarehousearea = dr("warehousearea")
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator("-", 50)
                oLogger.Write("Inspecting location " & tmpLocation)
            End If
            Dim LocHUTemplate As String = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("hustoragetemplate"))
            If dtHULocContent Is Nothing And LocHUTemplate <> "" Then
                dtHULocContent = GetHULocationContent(pPutRegion, oLogger)
            End If

            Dim isThreading As Boolean = False
            Try
                isThreading = System.Convert.ToBoolean(System.Convert.ToInt32(Made4Net.Shared.AppConfig.Get("isThreading", "0")))
                If isThreading Then
                    DataInterface.BeginTransaction()
                    Dim sql As String = String.Format("update LOCATION with (HOLDLOCK, ROWLOCK) set PROBLEMFLAG=PROBLEMFLAG  where LOCATION='{0}' and WAREHOUSEAREA='{1}' ", dr.Item("LOCATION"), dr.Item("WAREHOUSEAREA"))
                    DataInterface.RunSQL(sql)
                End If
                IsFit = CanPlace(dr, oCont, pFitByVolume, pFitByWeight, pFitByHeight, pFitByPalletType, dtHUTemplates, dtHULocContent, oLogger)
                If IsFit Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Location is suitable for receiving the container, inpecting content....")
                    End If
                    'need to check the location content
                    sameContent = CheckLocContent(dr, oCont, pContent, dtLocContent)


                    If sameContent Then

                        If Not oLogger Is Nothing Then
                            oLogger.writeSeperator("*", 50)
                            oLogger.Write("Location Found: " & loc)
                            oLogger.writeSeperator("*", 50)
                        End If
                        loc = tmpLocation
                        warehousearea = tmpWarehousearea
                        Return ""
                    Else
                        If Not oLogger Is Nothing Then
                            oLogger.Write("Location is not suitable for receiving the container, Content type mismatch...")
                        End If
                    End If
                Else
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Location is not suitable for receiving the Container...")
                    End If
                End If
            Catch ex As Exception
            Finally
                If isThreading Then
                    DataInterface.CommitTransaction()
                End If
            End Try
        Next
        Return String.Empty
    End Function

    Private Shared Function GetRegioLocationTable(ByVal oCont As Container, ByVal pPutRegion As String, ByVal pStorageType As String, ByVal pFitByVolume As Boolean, ByVal pFitByWeight As Boolean, ByVal pContent As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As DataTable
        Dim dt As New DataTable
        oLogger.Write("Putregion : " & pPutRegion)
        Dim sqlQuery As String = String.Format("select * from vPutAway ")

        Dim lstConditions As New List(Of String)
        If (Not String.IsNullOrEmpty(pPutRegion)) Then
            lstConditions.Add("PUTREGION = " + Made4Net.Shared.FormatField(pPutRegion))
        End If

        If (Not String.IsNullOrEmpty(pStorageType)) Then
            lstConditions.Add("LOCSTORAGETYPE = " + Made4Net.Shared.FormatField(pStorageType))
        End If
        If pFitByVolume Then
            lstConditions.Add(String.Format("CUBIC >=  REPLACE('{0}',',','.')", oCont.Volume))
        End If
        If pFitByWeight Then
            lstConditions.Add(String.Format("WEIGHT >= REPLACE('{0}',',','.')", oCont.Weight))

        End If
        Dim LocContentFilter As String = BuildLocContentFilter(pContent, pPutRegion, oCont)
        If LocContentFilter <> "" Then
            lstConditions.Add(String.Format("LOCATION  IN ({0})", LocContentFilter))

        End If
        lstConditions.Add("LOADID = " + Made4Net.Shared.FormatField(oCont.Loads(0).LOADID))
        If lstConditions.Any() Then
            sqlQuery += " WHERE " + String.Join(" AND ", lstConditions.ToArray())
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Get All Locations for putregion(s): " & pPutRegion)
            oLogger.Write("SQL : " & sqlQuery)
        End If
        DataInterface.FillDataset(sqlQuery, dt)
        Return dt
    End Function

#End Region

#Region "HUStorageTemplates"

    Private Shared Function GetHUStorageTemplates(Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As DataTable
        Dim dt As New DataTable
        Dim SQL As String = String.Format("SELECT HT.HUSTORAGETEMPLATEID, HT.HUSTORAGETEMPLATENAME, HTD.HUSTORAGETEMPLATELINE, HTD.HANDLINGUNIT, HTD.HANDLINGUNITQTY " &
                "FROM HANDLINGUNITSTORAGETEMPLATE HT INNER JOIN HANDLINGUNITSTORAGETEMPLATEDETAIL HTD ON HT.HUSTORAGETEMPLATEID = HTD.HUSTORAGETEMPLATEID order by HTD.HUSTORAGETEMPLATELINE")
        If Not oLogger Is Nothing Then
            oLogger.Write("Loading Handling Unit Storage Templates...")
        End If
        DataInterface.FillDataset(SQL, dt)
        If Not oLogger Is Nothing Then
            oLogger.Write("Handling Unit Storage Templates Loaded...")
        End If
        Return dt
    End Function

    ' vLocationHUTypeContent View doesnot exits in the current db
    Private Shared Function GetHULocationContent(ByVal pPutRegion As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing, Optional ByVal pLocation As String = "", Optional ByVal pWarehousearea As String = "") As DataTable
        Dim dt As New DataTable
        Dim SQL As String
        If pLocation <> String.Empty Then
            SQL = String.Format("select * from vLocationHUTypeContent WHERE location = '{0}' and warehousearea = '{1}' ", pLocation, pWarehousearea)
        Else
            SQL = String.Format("select * from vLocationHUTypeContent WHERE putregion in ({0})", pPutRegion)
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Loading Locations Handling Unit Content...")
            oLogger.Write("SQL Query: " & SQL)
        End If
        DataInterface.FillDataset(SQL, dt)
        If Not oLogger Is Nothing Then
            oLogger.Write("Locations Handling Unit Content Loaded...")
        End If
        Return dt
    End Function

    Public Shared Function ValidateLocationHU(ByVal pTemplateID As String, ByVal pLoc As String, ByVal pWarehousearea As String, ByVal pLoadHUType As String, ByVal pHUTemplateDT As DataTable, ByVal dtHULocContent As DataTable, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As Boolean
        'First - Load datatables if neccessary
        If pHUTemplateDT Is Nothing Then
            pHUTemplateDT = GetHUStorageTemplates()
        End If
        If dtHULocContent Is Nothing Then
            dtHULocContent = GetHULocationContent("", oLogger, pLoc)
        End If
        Dim SQL As String
        'if Load Handling Unit not in template return false
        Dim LoadHUArr() As DataRow = pHUTemplateDT.Select(String.Format("HUSTORAGETEMPLATEID = '{0}' and HANDLINGUNIT = '{1}' ", pTemplateID, pLoadHUType))
        If Not oLogger Is Nothing Then
            If LoadHUArr.Length = 0 Then
                oLogger.Write(String.Format("Handling Unit {0} not found in template {1}", pLoadHUType, pTemplateID))
            End If
        End If
        If LoadHUArr.Length = 0 Then Return False
        Dim LocContentHUArr() As DataRow = dtHULocContent.Select(String.Format("location = '{0}' and warehousearea = '{1}' ", pLoc, pWarehousearea))
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Location Handling Unit Content found and contains {0} rows", LocContentHUArr.Length))
        End If
        If LocContentHUArr.Length = 0 Then Return True
        Dim lineFound As Boolean = True
        Dim LocationHUTypeContent, LocationpendingHUTypeContent As String
        getContentHUString(LocContentHUArr, LocationHUTypeContent, LocationpendingHUTypeContent)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Location Current HU Types: {0}, Pending HU Types: {1}", LocationHUTypeContent, LocationpendingHUTypeContent))
        End If
        Dim currHUType, pendingHUType As String
        Dim currHUTypeQty, pendingHUTypeQty As Int32
        Dim iLoadHuTypeQty As Int32 = 0
        For currLocContRow As Int32 = 0 To LocContentHUArr.Length - 1
            currHUType = LocContentHUArr(currLocContRow)("currenthutype")
            pendingHUType = LocContentHUArr(currLocContRow)("pendinghutype")
            currHUTypeQty = LocContentHUArr(currLocContRow)("currentqty")
            pendingHUTypeQty = LocContentHUArr(currLocContRow)("pendingqty")
            If pLoadHUType = currHUType Then iLoadHuTypeQty += currHUTypeQty
            If pLoadHUType = pendingHUType Then iLoadHuTypeQty += pendingHUTypeQty
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Current HU Type: {0} of Qty: {1}, Pending HU Type: {2} of Qty: {3}", currHUType, currHUTypeQty, pendingHUType, pendingHUTypeQty))
            End If
            If currHUTypeQty = 0 And pendingHUTypeQty = 0 Then Return True
        Next
        Dim TemplateLinesArr() As DataRow = pHUTemplateDT.Select(String.Format("HUSTORAGETEMPLATEID = '{0}' and (HANDLINGUNIT = '{1}' or (HANDLINGUNIT in ({2}) or HANDLINGUNIT in ({3})))", pTemplateID, pLoadHUType, LocationHUTypeContent, LocationpendingHUTypeContent))
        If TemplateLinesArr.Length = 0 Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("No HU Template found involving the pending pallet type and the cureent content in location"))
            End If
            Return False
        End If
        For i As Int32 = 0 To TemplateLinesArr.Length - 1
            Dim line As Int32 = TemplateLinesArr(i)("HUSTORAGETEMPLATELINE")
            Dim TemplateHUQTYArr() As DataRow = pHUTemplateDT.Select(String.Format("HUSTORAGETEMPLATEID = '{0}' and HUSTORAGETEMPLATELINE = {1}", pTemplateID, line))
            If LineValid(TemplateHUQTYArr, LocContentHUArr, pLoadHUType) Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Trying to match HU types according to line {0}", line))
                End If
                For j As Int32 = 0 To TemplateHUQTYArr.Length - 1
                    Dim currTemplateHu As String = TemplateHUQTYArr(j)("HANDLINGUNIT")
                    Dim currTemplateHuQty As Int32 = TemplateHUQTYArr(j)("HANDLINGUNITQTY")
                    Dim shouldSkipLine As Boolean = False
                    For currLocContRow1 As Int32 = 0 To LocContentHUArr.Length - 1
                        currHUType = LocContentHUArr(currLocContRow1)("currenthutype")
                        pendingHUType = LocContentHUArr(currLocContRow1)("pendinghutype")
                        currHUTypeQty = LocContentHUArr(currLocContRow1)("currentqty")
                        pendingHUTypeQty = LocContentHUArr(currLocContRow1)("pendingqty")
                        If currHUType = currTemplateHu Then
                            If currHUTypeQty > currTemplateHuQty Then shouldSkipLine = True
                        End If
                        If pendingHUType = currTemplateHu Then
                            If pendingHUTypeQty > currTemplateHuQty Then shouldSkipLine = True
                        End If
                    Next
                    If shouldSkipLine Then
                        Exit For
                    End If
                    If pLoadHUType = currTemplateHu Then
                        If iLoadHuTypeQty < currTemplateHuQty Then Return True
                    End If
                Next
            End If
        Next
        'No Matching found - Return False
        Return False
    End Function

    Private Shared Function LineValid(ByVal TemplateHUQTYArr() As DataRow, ByVal LocContentHUArr() As DataRow, ByVal pIncomingPalletHUType As String) As Boolean
        For currLocContRow1 As Int32 = 0 To LocContentHUArr.Length - 1
            Dim currHUType1 As String = LocContentHUArr(currLocContRow1)("currenthutype")
            Dim pendingHUType1 = LocContentHUArr(currLocContRow1)("pendinghutype")
            Dim currHUTypeQty1 As Int32 = LocContentHUArr(currLocContRow1)("currentqty")
            Dim pendingHUTypeQty1 As Int32 = LocContentHUArr(currLocContRow1)("pendingqty")
            Dim found As Boolean = False
            Dim qtyExceeded As Boolean = False
            Dim incomingHUTypeFound As Boolean = False
            Dim currExistsInLine, pendExistsInLine As Boolean
            For x As Int32 = 0 To TemplateHUQTYArr.Length - 1
                If TemplateHUQTYArr(x)("HANDLINGUNIT") = currHUType1 Or currHUType1 = "" Then
                    currExistsInLine = True
                End If
            Next
            If Not currExistsInLine Then Return False
            For x As Int32 = 0 To TemplateHUQTYArr.Length - 1
                If TemplateHUQTYArr(x)("HANDLINGUNIT") = pendingHUType1 Or pendingHUType1 = "" Then
                    pendExistsInLine = True
                End If
            Next
            If Not pendExistsInLine Then Return False
            For x As Int32 = 0 To TemplateHUQTYArr.Length - 1
                Dim currTemplateHu As String = TemplateHUQTYArr(x)("HANDLINGUNIT")
                Dim currTemplateHuQty As Int32 = TemplateHUQTYArr(x)("HANDLINGUNITQTY")
                If currHUType1 = currTemplateHu AndAlso currHUType1 <> "" Then
                    found = True
                    If currHUTypeQty1 > currTemplateHuQty Then
                        Return False
                    ElseIf currHUType1 = pIncomingPalletHUType And currHUTypeQty1 + 1 > currTemplateHuQty Then
                        Return False
                    End If
                End If
                If pendingHUType1 = currTemplateHu AndAlso pendingHUType1 <> "" Then
                    found = True
                    If pendingHUTypeQty1 > currTemplateHuQty Then
                        Return False
                    ElseIf pendingHUType1 = pIncomingPalletHUType And pendingHUTypeQty1 + 1 > currTemplateHuQty Then
                        Return False
                    End If
                End If
                If currHUType1 = currTemplateHu And pendingHUType1 = currTemplateHu Then
                    If pendingHUTypeQty1 + currHUTypeQty1 > currTemplateHuQty Then
                        Return False
                    End If
                End If
                If pIncomingPalletHUType = currTemplateHu AndAlso pIncomingPalletHUType <> "" Then
                    incomingHUTypeFound = True
                End If
            Next
            If Not found Then Return False
            If Not incomingHUTypeFound Then Return False
        Next
        Return True
    End Function

    Private Shared Sub getContentHUString(ByVal LocContentHUArr() As DataRow, ByRef strCurrentHU As String, ByRef strPending As String)
        Try
            For currLocContRow As Int32 = 0 To LocContentHUArr.Length - 1
                strCurrentHU = strCurrentHU & String.Format("'{0}',", LocContentHUArr(currLocContRow)("currenthutype"))
                strPending = strPending & String.Format("'{0}',", LocContentHUArr(currLocContRow)("pendinghutype"))
            Next
            strCurrentHU = strCurrentHU.TrimEnd(",".ToCharArray)
            strPending = strPending.TrimEnd(",".ToCharArray)
        Catch ex As Exception
        End Try
    End Sub

#End Region

#Region "Location Content"

#Region "Location Content Type Constants"

    Protected Const Empty As String = "EMPTY"
    Protected Const AlsoSameSku As String = "ALSSKU"
    Protected Const AlsoSameSkuAndAttributes As String = "ALSSKUNATT"
    Protected Const OnlySameSku As String = "ONLSKU"
    Protected Const OnlySameSkuAndAttributes As String = "ONLSKUNATT"
    Protected Const NotSameSku As String = "NOTSKU"

#End Region

    Public Shared Function CheckLocContentWithMultipleLoads(ByVal dr As DataRow, ByVal ldLst As List(Of LoadsWithSequence), ByVal pContent As String, ByVal pDtLocContent As DataTable) As Boolean
        Dim numloads As Int32 = dr("NumLoads")
        Dim pendLoads As Int32 = dr("PendingLoads")
        Dim Warehousearea As String = dr("warehousearea")
        Dim Loc As String = dr("location")
        Dim tmpDrArr As DataRow()

        Dim skuList As String
        skuList = String.Join(",", ldLst.Select(Function(x) x.Load.SKU))
        skuList = "'" + skuList.Replace(",", "','") + "'"

        Dim consineeList As String
        consineeList = String.Join(",", ldLst.Select(Function(x) x.Load.CONSIGNEE))
        consineeList = "'" + consineeList.Replace(",", "','") + "'"

        Select Case pContent
            Case Empty
                If numloads + pendLoads = 0 Then
                    Return True
                Else
                    Return False
                End If
            Case OnlySameSku
                'need to count different sku's
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku not in (" & skuList & ") or consignee not in (" & consineeList & "))"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length > 0 Then
                    Return False
                End If

                'And dont forget to look for the same sku in the location....
                SQL = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku in (" & skuList & ") and consignee in (" & consineeList & "))"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
            Case OnlySameSkuAndAttributes
                'need to count different sku's and their load's attributes
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku not in (" & skuList & ") or consignee not in (" & consineeList & "))"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length > 0 Then
                    'we have more than one sku
                    Return False
                End If
                'And dont forget to look for the same sku in the location....
                SQL = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku in (" & skuList & ") and consignee in (" & consineeList & "))"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
                'we have one sku - we have to match the loads attributes
                ' Check only the first load, instead of checking with all the loads which makes the matching creteria more complicated
                SQL = "Select distinct loadid From invload Where (Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and loadid <> '" & ldLst.First().Load.LOADID & "'"
                Dim dt As New DataTable
                DataInterface.FillDataset(SQL, dt)
                Dim tmpLoad As Load
                For Each drAtt As DataRow In dt.Rows
                    tmpLoad = New Load(System.Convert.ToString(drAtt("loadid")))
                    If Not AttributesCollection.Equal(ldLst.First().Load.LoadAttributes().Attributes, tmpLoad.LoadAttributes.Attributes) Then
                        Return False
                    End If
                Next
                ' Check only the first load, instead of checking with all the loads which makes the matching creteria more complicated
            Case AlsoSameSku
                'look for the same sku in the location
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku in (" & skuList & ") and consignee in (" & consineeList & "))"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
            Case AlsoSameSkuAndAttributes
                'look for the same sku in the location
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku in (" & skuList & ") and consignee in (" & consineeList & "))"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
                'we have one sku - we have to match the loads attributes
                ' Check only the first load, instead of checking with all the loads which makes the matching creteria more complicated
                SQL = "Select distinct loadid From invload Where (Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and loadid <> '" & ldLst.First().Load.LOADID & "'"
                Dim dt As New DataTable
                DataInterface.FillDataset(SQL, dt)
                Dim tmpLoad As Load
                For Each drAtt As DataRow In dt.Rows
                    tmpLoad = New Load(System.Convert.ToString(drAtt("loadid")))
                    If AttributesCollection.Equal(ldLst.First().Load.LoadAttributes().Attributes, tmpLoad.LoadAttributes.Attributes) Then
                        Return True
                    End If
                Next
                ' Check only the first load, instead of checking with all the loads which makes the matching creteria more complicated
                Return False
            Case NotSameSku
                'look for the same sku in the location
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku in (" & skuList & ") and consignee in (" & consineeList & ")')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length > 0 Then
                    Return False
                End If
        End Select
        Return True
    End Function

    Public Shared Function CheckLocContent(ByVal dr As DataRow, ByVal pLoad As Load, ByVal pContent As String, ByVal pDtLocContent As DataTable) As Boolean
        Dim numloads As Int32 = dr("NumLoads")
        Dim pendLoads As Int32 = dr("PendingLoads")
        Dim Warehousearea As String = dr("warehousearea")
        Dim Loc As String = dr("location")
        Dim tmpDrArr As DataRow()
        Select Case pContent
            Case Empty
                If numloads + pendLoads = 0 Then
                    Return True
                Else
                    Return False
                End If
            Case OnlySameSku
                'need to count different sku's
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku <> '" & pLoad.SKU & "' or consignee <> '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length > 0 Then
                    Return False
                End If

                'And dont forget to look for the same sku in the location....
                SQL = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & pLoad.SKU & "' and consignee = '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
            Case OnlySameSkuAndAttributes
                'need to count different sku's and their load's attributes
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku <> '" & pLoad.SKU & "' or consignee <> '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length > 0 Then
                    'we have more than one sku
                    Return False
                End If
                'And dont forget to look for the same sku in the location....
                SQL = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & pLoad.SKU & "' and consignee = '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
                'we have one sku - we have to match the loads attributes
                SQL = "Select distinct loadid From invload Where (Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and loadid <> '" & pLoad.LOADID & "'"
                Dim dt As New DataTable
                DataInterface.FillDataset(SQL, dt)
                Dim tmpLoad As Load
                For Each drAtt As DataRow In dt.Rows
                    tmpLoad = New Load(System.Convert.ToString(drAtt("loadid")))
                    If Not AttributesCollection.Equal(pLoad.LoadAttributes().Attributes, tmpLoad.LoadAttributes.Attributes) Then
                        Return False
                    End If
                Next
            Case AlsoSameSku
                'look for the same sku in the location
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & pLoad.SKU & "' and consignee = '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
            Case AlsoSameSkuAndAttributes
                'look for the same sku in the location
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & pLoad.SKU & "' and consignee = '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length = 0 Then
                    Return False
                End If
                'we have one sku - we have to match the loads attributes
                SQL = "Select distinct loadid From invload Where (Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and loadid <> '" & pLoad.LOADID & "'"
                Dim dt As New DataTable
                DataInterface.FillDataset(SQL, dt)
                Dim tmpLoad As Load
                For Each drAtt As DataRow In dt.Rows
                    tmpLoad = New Load(System.Convert.ToString(drAtt("loadid")))
                    If AttributesCollection.Equal(pLoad.LoadAttributes().Attributes, tmpLoad.LoadAttributes.Attributes) Then
                        Return True
                    End If
                Next
                Return False
            Case NotSameSku
                'look for the same sku in the location
                Dim SQL As String = "(Location = '" & Loc & "' or destinationlocation = '" & Loc & "') and (sku = '" & pLoad.SKU & "' and consignee = '" & pLoad.CONSIGNEE & "')"
                tmpDrArr = pDtLocContent.Select(SQL)
                If tmpDrArr.Length > 0 Then
                    Return False
                End If
        End Select
        Return True
    End Function

    Private Shared Function CheckLocContent(ByVal dr As DataRow, ByVal pCont As Container, ByVal pContent As String, ByVal pDtLocContent As DataTable) As Boolean
        Dim numloads As Int32 = dr("NumLoads")
        Dim pendLoads As Int32 = dr("PendingLoads")
        Dim Loc As String = dr("location")
        Dim tmpDrArr As DataRow()
        Select Case pContent
            Case Empty
                If numloads + pendLoads = 0 Then
                    Return True
                Else
                    Return False
                End If
            Case OnlySameSku
                'need to count different sku's
                For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                    Dim existsOnCont As Boolean = False
                    For Each ld As WMS.Logic.Load In pCont.Loads
                        If ld.CONSIGNEE = contentDr("CONSIGNEE").ToString() AndAlso
                        ld.SKU = contentDr("SKU").ToString() Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If Not existsOnCont Then
                        Return False
                    End If
                Next
                For Each ld As Load In pCont.Loads
                    Dim existsOnCont As Boolean = False
                    For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                        If ld.CONSIGNEE = contentDr("CONSIGNEE") AndAlso
                        ld.SKU = contentDr("SKU") Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If Not existsOnCont Then
                        Return False
                    End If
                Next
            Case OnlySameSkuAndAttributes
                For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                    Dim existsOnCont As Boolean = False
                    For Each ld As WMS.Logic.Load In pCont.Loads
                        If ld.CONSIGNEE = contentDr("CONSIGNEE").ToString() AndAlso
                        ld.SKU = contentDr("SKU").ToString() Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If Not existsOnCont Then
                        Return False
                    End If
                Next

                For Each ld As Load In pCont.Loads
                    Dim existsOnCont As Boolean = False
                    For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                        If ld.CONSIGNEE = contentDr("CONSIGNEE") AndAlso
                        ld.SKU = contentDr("SKU") Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If Not existsOnCont Then
                        Return False
                    End If
                Next
                'we have one sku - we have to match the loads attributes
                Dim sql As String = "Select distinct loadid From invload Where (Location = '" & Loc & "' or destinationlocation = '" & Loc & "') " 'and loadid <> '" & pLoad.LOADID & "'"
                Dim dt As New DataTable
                DataInterface.FillDataset(sql, dt)
                Dim tmpLoad As Load
                For Each drAtt As DataRow In dt.Rows
                    tmpLoad = New Load(System.Convert.ToString(drAtt("loadid")))
                    For Each ld As Load In pCont.Loads
                        If ld.LOADID <> tmpLoad.LOADID AndAlso ld.CONSIGNEE = tmpLoad.CONSIGNEE AndAlso ld.SKU = tmpLoad.SKU Then
                            If Not AttributesCollection.Equal(ld.LoadAttributes().Attributes, tmpLoad.LoadAttributes.Attributes) Then
                                Return False
                            End If
                        End If
                    Next
                Next
            Case AlsoSameSku
                For Each ld As Load In pCont.Loads
                    Dim existsOnCont As Boolean = False
                    For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                        If ld.CONSIGNEE = contentDr("CONSIGNEE") AndAlso
                        ld.SKU = contentDr("SKU") Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If Not existsOnCont Then
                        Return False
                    End If
                Next
            Case AlsoSameSkuAndAttributes
                For Each ld As Load In pCont.Loads
                    Dim existsOnCont As Boolean = False
                    For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                        If ld.CONSIGNEE = contentDr("CONSIGNEE") AndAlso
                        ld.SKU = contentDr("SKU") Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If Not existsOnCont Then
                        Return False
                    End If
                Next
                Dim sql As String = "Select distinct loadid From invload Where (Location = '" & Loc & "' or destinationlocation = '" & Loc & "') " 'and loadid <> '" & pLoad.LOADID & "'"
                Dim dt As New DataTable
                DataInterface.FillDataset(sql, dt)
                Dim tmpLoad As Load
                For Each drAtt As DataRow In dt.Rows
                    tmpLoad = New Load(System.Convert.ToString(drAtt("loadid")))
                    For Each ld As Load In pCont.Loads
                        If ld.LOADID <> tmpLoad.LOADID AndAlso ld.CONSIGNEE = tmpLoad.CONSIGNEE AndAlso ld.SKU = tmpLoad.SKU Then
                            If Not AttributesCollection.Equal(ld.LoadAttributes().Attributes, tmpLoad.LoadAttributes.Attributes) Then
                                Return False
                            End If
                        End If
                    Next
                Next
            Case NotSameSku
                For Each ld As Load In pCont.Loads
                    Dim existsOnCont As Boolean = False
                    For Each contentDr As DataRow In pDtLocContent.Select("(Location = '" & Loc & "' or destinationlocation = '" & Loc & "')")
                        If ld.CONSIGNEE = contentDr("CONSIGNEE") AndAlso ld.SKU = contentDr("SKU") Then
                            existsOnCont = True
                            Continue For
                        End If
                    Next
                    If existsOnCont Then
                        Return False
                    End If
                Next
        End Select
        Return True
    End Function

    Private Shared Function BuildLocContentFilter(ByVal pContent As String, ByVal pPutRegion As String, ByVal pSku As String) As String
        Dim strFinalQuery As String
        Select Case pContent
            Case Empty
                If (Not String.IsNullOrEmpty(pPutRegion)) Then
                    strFinalQuery = String.Format("select location from location where putregion = '{0}' and location not in( select distinct l.location from invload il inner join location l on l.location = il.location where putregion = '{0}')", pPutRegion)
                Else
                    strFinalQuery = String.Format("select location from location where location not in( select distinct l.location from invload il inner join location l on l.location = il.location)")
                End If

            Case OnlySameSku, OnlySameSkuAndAttributes
                If (Not String.IsNullOrEmpty(pPutRegion)) Then
                    strFinalQuery = String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion = '{0}' and il.sku = '{1}' and l.location not in (select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion ='{0}' and il.sku <> '{1}')", pPutRegion, pSku)
                Else
                    strFinalQuery = String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where il.sku = '{0}' and l.location not in (select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where il.sku <> '{0}')", pSku)
                End If
            Case AlsoSameSku, AlsoSameSkuAndAttributes
                If (Not String.IsNullOrEmpty(pPutRegion)) Then
                    strFinalQuery = String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion = '{0}' and il.sku = '{1}'", pPutRegion, pSku)
                Else
                    strFinalQuery = String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where  il.sku = '{0}'", pSku)
                End If
            Case NotSameSku
                If (Not String.IsNullOrEmpty(pPutRegion)) Then
                    strFinalQuery = String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion ='{0}' and il.sku <> '{1}'", pPutRegion, pSku)
                Else
                    strFinalQuery = String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where  il.sku <> '{0}'", pSku)
                End If
            Case Else
                strFinalQuery = ""
        End Select
        Return strFinalQuery
    End Function

    ' Builds Content filter as according to the Policy considring all the SKUs from all the loads for Multi-payload putaway.
    Private Shared Function BuildLocContentFilterForMultipleSKUs(ByVal pContent As String, ByVal pPutRegion As String, ByVal loadsWithSequence As IList(Of LoadsWithSequence)) As String
        Dim skuList As String
        skuList = String.Join(",", loadsWithSequence.Select(Function(x) x.Load.SKU))
        skuList = "'" + skuList.Replace(",", "','") + "'"
        Select Case pContent
            Case Empty
                Return String.Format("select location from location where putregion ='{0}' and location not in( select distinct l.location from invload il inner join location l on l.location = il.location where putregion = '{0}')", pPutRegion)
            Case OnlySameSku, OnlySameSkuAndAttributes
                Return String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion = '{0}' and il.sku in ({1}) and l.location not in (select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion ='{0}' and il.sku not in ({1}) )", pPutRegion, skuList)
            Case AlsoSameSku, AlsoSameSkuAndAttributes
                Return String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion ='{0}' and il.sku in ({1})", pPutRegion, skuList)
            Case NotSameSku
                Return String.Format("select distinct l.location from invload il inner join location l on l.location = il.location or l.location = il.destinationlocation where putregion = '{0}' and il.sku not in ({1})", pPutRegion, skuList)
            Case Else
                Return ""
        End Select
    End Function

    Private Shared Function BuildLocContentFilter(ByVal pContent As String, ByVal pPutRegion As String, ByVal pCont As Container) As String
        Select Case pContent
            Case Empty
                Return String.Format("select location from location where putregion = '{0}' and location not in( select distinct l.location from invload il inner join location l on l.location = il.location where putregion = '{0}')", pPutRegion)
            Case OnlySameSku, OnlySameSkuAndAttributes
                Dim sql As String = String.Format("select distinct invload.LOCATION,CONSIGNEE,SKU from INVLOAD inner join location on invload.location = location.location where putregion = '{0}'", pPutRegion)
                Dim invDt As New DataTable()
                Made4Net.DataAccess.DataInterface.FillDataset(sql, invDt)

                Dim locList As New System.Collections.Generic.List(Of String)
                Dim checkedSKUsList As New System.Collections.Generic.List(Of String)
                For Each ld As WMS.Logic.Load In pCont.Loads
                    If checkedSKUsList.Contains(ld.CONSIGNEE & "_" & ld.SKU) Then
                        Continue For
                    End If

                    'Means this sku is not stored any where.
                    If invDt.Select(String.Format("consignee='{0}' and sku='{1}'", ld.CONSIGNEE, ld.SKU)).Length = 0 Then
                        Return "''"
                    End If

                    Dim foundLocations As New System.Collections.Generic.List(Of String)
                    For Each dr As DataRow In invDt.Select(String.Format("consignee='{0}' and sku='{1}'", ld.CONSIGNEE, ld.SKU))
                        If checkedSKUsList.Count = 0 Then
                            locList.Add(dr("LOCATION"))
                        Else
                            If locList.Contains(dr("LOCATION")) Then
                                foundLocations.Add(dr("LOCATION"))
                            End If
                        End If
                    Next

                    'Means no location was found.
                    If Not checkedSKUsList.Count = 0 AndAlso foundLocations.Count = 0 Then
                        Return "''"
                    End If

                    If Not checkedSKUsList.Count = 0 Then
                        Dim locationsToKeep As New System.Collections.Generic.List(Of String)
                        For Each loc As String In locList
                            If foundLocations.Contains(loc) Then
                                locationsToKeep.Add(loc)
                            End If
                        Next

                        locList = locationsToKeep
                    End If

                    checkedSKUsList.Add(ld.CONSIGNEE & "_" & ld.SKU)
                Next
                Dim locationsStr As String = ""
                For Each location As String In locList
                    If invDt.Select(String.Format("LOCATION='{0}'", location)).Length = checkedSKUsList.Count Then
                        locationsStr = locationsStr & ",'" & location & "'"
                    End If
                Next
                If locationsStr = "" Then
                    Return "''"
                End If
                Return locationsStr.TrimStart(",".ToCharArray())
            Case AlsoSameSku, AlsoSameSkuAndAttributes
                Dim sql As String = String.Format("select distinct invload.LOCATION,CONSIGNEE,SKU from INVLOAD inner join location on invload.location = location.location where putregion in ({0})", pPutRegion)
                Dim invDt As New DataTable()
                Made4Net.DataAccess.DataInterface.FillDataset(sql, invDt)

                Dim locList As New System.Collections.Generic.List(Of String)
                Dim checkedSKUsList As New System.Collections.Generic.List(Of String)
                For Each ld As WMS.Logic.Load In pCont.Loads
                    If checkedSKUsList.Contains(ld.CONSIGNEE & "_" & ld.SKU) Then
                        Continue For
                    End If

                    'Means this sku is not stored any where.
                    If invDt.Select(String.Format("consignee='{0}' and sku='{1}'", ld.CONSIGNEE, ld.SKU)).Length = 0 Then
                        Return "''"
                    End If

                    Dim foundLocations As New System.Collections.Generic.List(Of String)
                    For Each dr As DataRow In invDt.Select(String.Format("consignee='{0}' and sku='{1}'", ld.CONSIGNEE, ld.SKU))
                        If checkedSKUsList.Count = 0 Then
                            locList.Add(dr("LOCATION"))
                        Else
                            If locList.Contains(dr("LOCATION")) Then
                                foundLocations.Add(dr("LOCATION"))
                            End If
                        End If
                    Next

                    'Means no location was found.
                    If Not checkedSKUsList.Count = 0 AndAlso foundLocations.Count = 0 Then
                        Return "''"
                    End If

                    If Not checkedSKUsList.Count = 0 Then
                        Dim locationsToKeep As New System.Collections.Generic.List(Of String)
                        For Each loc As String In locList
                            If foundLocations.Contains(loc) Then
                                locationsToKeep.Add(loc)
                            End If
                        Next

                        locList = locationsToKeep
                    End If

                    checkedSKUsList.Add(ld.CONSIGNEE & "_" & ld.SKU)
                Next
                Dim locationsStr As String = ""
                For Each location As String In locList
                    If invDt.Select(String.Format("LOCATION='{0}'", location)).Length >= checkedSKUsList.Count Then
                        locationsStr = locationsStr & ",'" & location & "'"
                    End If
                Next
                If locationsStr = "" Then
                    Return "''"
                End If
                Return locationsStr.TrimStart(",".ToCharArray())
            Case Else
                Return ""
        End Select
    End Function

#End Region

#Region "Accessors"

    Private Shared Function GetRegioLoadsTable(ByVal pPutRegion As String, Optional ByVal oLogger As WMS.Logic.LogHandler = Nothing) As DataTable
        Dim dt As New DataTable
        Dim SQL As String
        If (Not String.IsNullOrEmpty(pPutRegion)) Then
            SQL = String.Format("select invload.* from location " &
               " inner join invload on invload.location = location.location  " &
               " where location.putregion = '{0}' union" &
               " select invload.* from location " &
               " inner join invload on invload.destinationlocation = location.location  " &
               " where location.putregion = '{0}'", pPutRegion)
        Else
            SQL = String.Format("select invload.* from location " &
               " inner join invload on invload.location = location.location  " &
               " union " &
               " select invload.* from location " &
               " inner join invload on invload.destinationlocation = location.location")

        End If

        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to Get All Loads for putregion(s): " & pPutRegion)
            oLogger.Write("SQL : " & SQL)
        End If
        DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

#End Region

#End Region

    'Added for RWMS-2510 Start
    Public Function CheckCanDelete(ByVal strLoc As String, ByRef checkMessage As String) As Boolean
        Dim sql As String
        Dim intval As Integer
        Dim ret As Boolean = True
        Try
            'Check if location is a pickloc
            sql = String.Format("select COUNT(1) from pickloc where PICKLOC.LOCATION='{0}'", strLoc)
            intval = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If intval > 0 Then

                Dim oPickLoc As PickLoc = New PickLoc
                ret = oPickLoc.CheckCanDelete(strLoc, checkMessage)
                If Not ret Then
                    Return False
                End If
            End If

            sql = String.Format("select COUNT(1) from INVLOAD where LOCATION='{0}'", strLoc)
            intval = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If intval > 0 Then
                checkMessage = "Cannot delete as there is inventory in the location"
                Return False
            End If

            sql = String.Format("select COUNT(1) from TASKS WHERE STATUS NOT IN ('COMPLETE','CANCELED') AND TOLOCATION='{0}'", strLoc)
            intval = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If intval > 0 Then
                checkMessage = "Cannot delete as there are putaway tasks destined for that location"
                Return False
            End If

        Catch ex As Exception
            ret = False
            checkMessage = ex.ToString()
        End Try
        Return ret

    End Function
    'Added for RWMS-2510 End

End Class

#End Region