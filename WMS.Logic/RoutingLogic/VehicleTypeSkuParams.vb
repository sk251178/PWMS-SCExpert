Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "VEHICLETYPESKUPARAMS"

<CLSCompliant(False)> Public Class VehicleTypeSkuParams

#Region "Variables"

#Region "Primary Keys"

    Protected _vehicletypeid As Int32 = -1
    Protected _skuclsname As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _maxcube As Double = -1
    Protected _maxweight As Double = -1
    Protected _mincube As Double = -1
    Protected _minweight As Double = -1
    Protected _adduser As String = String.Empty
    Protected _adddate As DateTime
    Protected _edituser As String = String.Empty
    Protected _editdate As DateTime

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " Where VEHICLETYPEID = '" & _vehicletypeid & "' And SKUCLSNAME = '" & _skuclsname & "'"
        End Get
    End Property

    Public ReadOnly Property VEHICLETYPEID() As Double
        Get
            Return _vehicletypeid
        End Get
    End Property

    Public ReadOnly Property SKUCLSNAME() As Double
        Get
            Return _skuclsname
        End Get
    End Property

    Public Property MAXCUBE() As Double
        Get
            Return _maxcube
        End Get
        Set(ByVal Value As Double)
            _maxcube = Value
        End Set
    End Property

    Public Property MAXWEIGHT() As Double
        Get
            Return _maxweight
        End Get
        Set(ByVal Value As Double)
            _maxweight = Value
        End Set
    End Property

    Public Property MINCUBE() As Double
        Get
            Return _mincube
        End Get
        Set(ByVal Value As Double)
            _mincube = Value
        End Set
    End Property

    Public Property MINWEIGHT() As Double
        Get
            Return _minweight
        End Get
        Set(ByVal Value As Double)
            _minweight = Value
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

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
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

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property



#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pVEHICLETYPEID As Int32, ByVal pSKUCLSNAME As String, Optional ByVal LoadObj As Boolean = True)
        _vehicletypeid = pVEHICLETYPEID
        _skuclsname = pSKUCLSNAME
        If LoadObj Then
            Load()
        End If
    End Sub

    Public Sub New(ByRef dr As DataRow)
        SetObj(dr)
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetVEHICLETYPESKUPARAMS(ByVal pVEHICLETYPEID As String, ByVal pSKUCLSNAME As String) As VehicleTypeSkuParams
        Return New VehicleTypeSkuParams(pVEHICLETYPEID, pSKUCLSNAME)
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM VEHICLETYPESKUPARAMS " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return
        End If

        dr = dt.Rows(0)
        SetObj(dr)

    End Sub

    Protected Sub SetObj(ByRef dr As DataRow)
        _vehicletypeid = dr.Item("VEHICLETYPEID")
        _skuclsname = dr.Item("SKUCLSNAME")
        If Not dr.IsNull("MAXCUBE") Then _maxcube = dr.Item("MAXCUBE")
        If Not dr.IsNull("MAXWEIGHT") Then _maxweight = dr.Item("MAXWEIGHT")
        If Not dr.IsNull("MINCUBE") Then _mincube = dr.Item("MINCUBE")
        If Not dr.IsNull("MINWEIGHT") Then _minweight = dr.Item("MINWEIGHT")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
    End Sub

#End Region

End Class
#End Region

#Region "VEHICLETYPESKUPARAMSCOLLECTION"
<CLSCompliant(False)> Public Class VehicleTypeSkuParamsCollection
    Inherits ArrayList
#Region "Properties"
    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As VehicleTypeSkuParams
        Get
            Return CType(MyBase.Item(index), VehicleTypeSkuParams)
        End Get
    End Property
#End Region
#Region "Constructors"
    Public Sub New()
    End Sub
#End Region

#Region "Methods"
    Public Sub LoadFromVehicleType(ByVal vehicletype As Int32)
        Dim sql As String = "SELECT * FROM VEHICLETYPESKUPARAMS WHERE VEHICLETYPEID = " + vehicletype.ToString()
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            Me.Add(New VehicleTypeSkuParams(dr))
        Next
    End Sub
    Public Function GetBySKU(ByVal sku As String)
        Dim vsp As VehicleTypeSkuParams
        For Each vsp In Me
            If vsp.SKUCLSNAME.Equals(sku) Then
                Return vsp
            End If
        Next
        Return Nothing
    End Function
#End Region
End Class
#End Region