Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert



<CLSCompliant(False)> Public Class VehicleType



#Region "Primary Keys"

    Protected _vehicletypeid As String

#End Region

#Region "Other Fields"

    Protected _name As String = String.Empty
    Protected _vehiclescount As Int32
    Protected _vehiclepriorityinpool As Int32
    Protected _costperdistanceuinit As Double
    Protected _costperhour As Double
    Protected _costperday As Double
    Protected _totalvolume As Double
    Protected _totalweight As Double
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edittuser As String = String.Empty

    ' the Sku param collection for the vehicle type
    Protected _skuparams As VehicleTypeSkuParamsCollection
    Protected _vehicletypetransportationclasscollection As TransportationClassCollection

    Protected _maxroutesperday As Integer = 0

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where VEHICLETYPENAME = " & Made4Net.Shared.Util.FormatField(_vehicletypeid) & ""
        End Get
    End Property

    Public ReadOnly Property VEHICLETYPEID() As String
        Get
            Return _vehicletypeid
        End Get
    End Property

    Public Property NAME() As String
        Get
            Return _name
        End Get
        Set(ByVal Value As String)
            _name = Value
        End Set
    End Property

    Public Property VehiclePriorityinPool() As Int32
        Get
            Return _vehiclepriorityinpool
        End Get
        Set(ByVal Value As Int32)
            _vehiclepriorityinpool = Value
        End Set
    End Property

    Public Property VEHICLESCOUNT() As Int32
        Get
            Return _vehiclescount
        End Get
        Set(ByVal Value As Int32)
            _vehiclescount = Value
        End Set
    End Property

    Public Property COSTPERDISTANCEUINIT() As Double
        Get
            Return _costperdistanceuinit
        End Get
        Set(ByVal Value As Double)
            _costperdistanceuinit = Value
        End Set
    End Property

    Public Property COSTPERHOUR() As Double
        Get
            Return _costperhour
        End Get
        Set(ByVal Value As Double)
            _costperhour = Value
        End Set
    End Property

    Public Property MAXROUTESPERDAY() As Integer
        Get
            Return _maxroutesperday
        End Get
        Set(ByVal Value As Integer)
            _maxroutesperday = Value
        End Set
    End Property

    Public Property COSTPERDAY() As Double
        Get
            Return _costperday
        End Get
        Set(ByVal Value As Double)
            _costperday = Value
        End Set
    End Property

    Public Property TOTALVOLUME() As Double
        Get
            Return _totalvolume
        End Get
        Set(ByVal Value As Double)
            _totalvolume = Value
        End Set
    End Property

    Public Property TOTALWEIGHT() As Double
        Get
            Return _totalweight
        End Get
        Set(ByVal Value As Double)
            _totalweight = Value
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

    Public Property EDITTUSER() As String
        Get
            Return _edittuser
        End Get
        Set(ByVal Value As String)
            _edittuser = Value
        End Set
    End Property

    Public ReadOnly Property SKUPARAMS() As VehicleTypeSkuParamsCollection
        Get
            Return _skuparams
        End Get
    End Property


    Public ReadOnly Property VehicleTypeTransportationClass() As TransportationClassCollection
        Get
            Return _vehicletypetransportationclasscollection
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pVEHICLETYPEID As String, Optional ByVal LoadObj As Boolean = True)
        _vehicletypeid = pVEHICLETYPEID
        If LoadObj Then
            Load()
            _vehicletypetransportationclasscollection = New TransportationClassCollection(_vehicletypeid, True)
        End If
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetVEHICLETYPE(ByVal pVEHICLETYPEID As String) As VehicleType
        Return New VehicleType(pVEHICLETYPEID)
    End Function

    Public Shared Function Exists(ByVal pVEHICLETYPEID As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from VEHICLETYPE Where VEHICLETYPENAME  = {0}", Made4Net.Shared.FormatField(pVEHICLETYPEID))
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function



    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM VEHICLETYPE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("VehicleType Record Does Not Exists")
        End If

        dr = dt.Rows(0)

        _vehicletypeid = dr.Item("VEHICLETYPENAME")
        If Not dr.IsNull("DESCRIPTION") Then _name = dr.Item("DESCRIPTION")
        'If Not dr.IsNull("VEHICLESCOUNT") Then _vehiclescount = dr.Item("VEHICLESCOUNT")
        If Not dr.IsNull("COSTPERDISTANCEUNIT") Then _costperdistanceuinit = dr.Item("COSTPERDISTANCEUNIT")
        If Not dr.IsNull("COSTPERHOUR") Then _costperhour = dr.Item("COSTPERHOUR")
        If Not dr.IsNull("COSTPERDAY") Then _costperday = dr.Item("COSTPERDAY")
        If Not dr.IsNull("TOTALVOLUME") Then _totalvolume = dr.Item("TOTALVOLUME")
        If Not dr.IsNull("TOTALWEIGHT") Then _totalweight = dr.Item("TOTALWEIGHT")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITTUSER") Then _edittuser = dr.Item("EDITTUSER")

        If Not dr.IsNull("MAXROUTESPERDAY") Then _maxroutesperday = dr.Item("MAXROUTESPERDAY")

        _skuparams = New VehicleTypeSkuParamsCollection
        'SIMON DEMO
        '_skuparams.LoadFromVehicleType(_vehicletypeid)

    End Sub




#End Region

End Class

<CLSCompliant(False)> _
Public Class TransportationClassCollection


#Region "Primary Keys"
    Protected _vehicletypeid As String
#End Region

#Region "Other Fields"

    Protected _transportationclasscollection As New Hashtable()

#End Region

#Region "Properties"
    Public ReadOnly Property VEHICLETYPEID() As String
        Get
            Return _vehicletypeid
        End Get
    End Property

    Public Property TransportationClassCollection() As Hashtable
        Get
            Return _transportationclasscollection
        End Get
        Set(ByVal value As Hashtable)
            _transportationclasscollection = value
        End Set
    End Property


#End Region

#Region "Constructors"
    Public Sub New()
    End Sub

    Public Sub New(ByVal pVEHICLETYPEID As String, Optional ByVal LoadObj As Boolean = True)
        _vehicletypeid = pVEHICLETYPEID
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

    Protected Sub Load()

        Dim SQL As String = String.Format("SELECT * FROM VEHICLETYPETRANSPORTATIONCLASS where VEHICLETYPENAME={0} ", _
                    Made4Net.Shared.Util.FormatField(_vehicletypeid))

        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        For Each dr In dt.Rows()
            Dim TRANSPORTATIONCLASS As String = ReplaceDBNull(dr.Item("TRANSPORTATIONCLASS"))
            If Not _transportationclasscollection.Contains(TRANSPORTATIONCLASS) Then
                Dim oTRANSPORTATIONCLASS As New TransportationClass(TRANSPORTATIONCLASS, _
                ReplaceDBNull(dr.Item("MAXWEIGHT")), _
                ReplaceDBNull(dr.Item("MINWEIGHT")), _
                ReplaceDBNull(dr.Item("MAXCUBE")), _
                ReplaceDBNull(dr.Item("MINCUBE")))
                _transportationclasscollection.Add(TRANSPORTATIONCLASS, oTRANSPORTATIONCLASS)
            End If
        Next
    End Sub

End Class

<CLSCompliant(False)> _
Public Class TransportationClass

#Region "Primary Keys"
    Protected _transportationclass As String

#End Region

#Region "Other Fields"
    Protected _maxweight As Double = 0
    Protected _minweight As Double = 0
    Protected _maxvolume As Double = 0
    Protected _minvolume As Double = 0
#End Region

#Region "Properties"
    Public Property TransportationClass() As String
        Get
            Return _transportationclass
        End Get
        Set(ByVal value As String)
            _transportationclass = value
        End Set
    End Property


    Public Property MaxTotalWeight() As Double
        Get
            Return _maxweight
        End Get
        Set(ByVal value As Double)
            _maxweight = value
        End Set
    End Property

    Public Property MinTotalWeight() As Double
        Get
            Return _minweight
        End Get
        Set(ByVal value As Double)
            _minweight = value
        End Set
    End Property

    Public Property MinTotalVolume() As Double
        Get
            Return _minvolume
        End Get
        Set(ByVal value As Double)
            _minvolume = value
        End Set
    End Property

    Public Property MaxTotalVolume() As Double
        Get
            Return _maxvolume
        End Get
        Set(ByVal value As Double)
            _maxvolume = value
        End Set
    End Property
#End Region

#Region "Constructors"
    Sub New(ByVal pTransportationclass As String)
        _transportationclass = pTransportationclass
    End Sub

    Sub New(ByVal pTransportationclass As String, _
                    ByVal pmaxweight As Double, _
                    ByVal pminweight As Double, _
                    ByVal pmaxvolume As Double, _
                    ByVal pminvolume As Double)

        _transportationclass = pTransportationclass
        _maxweight = pmaxweight
        _minweight = pminweight
        _maxvolume = pmaxvolume
        _minvolume = pminvolume

    End Sub

#End Region

End Class



