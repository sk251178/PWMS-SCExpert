Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class HandelingUnit

#Region "Variables"
    Protected _container As String
    Protected _containerdesc As String
    Protected _containercube As Double
    Protected _containerweight As Double
    Protected _cubecapacity As Double
    Protected _weightcapacity As Double
    Protected _width As Double
    Protected _height As Double
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String
#End Region

#Region "Properties"
    Public ReadOnly Property WhereClause() As String
        Get
            Return " CONTAINER = '" & _container & "'"
        End Get
    End Property
    Public Property Container() As String
        Get
            Return _container
        End Get
        Set(ByVal Value As String)
            _container = Value
        End Set
    End Property
    Public Property ContainerDesc() As String
        Get
            Return _containerdesc
        End Get
        Set(ByVal Value As String)
            _containerdesc = Value
        End Set
    End Property
    Public Property ContainerCube() As Double
        Get
            Return _containercube
        End Get
        Set(ByVal Value As Double)
            _containercube = Value
        End Set
    End Property
    Public Property ContainerWeight() As Double
        Get
            Return _containerweight
        End Get
        Set(ByVal Value As Double)
            _containerweight = Value
        End Set
    End Property
    Public Property CubeCapacity() As Double
        Get
            Return _cubecapacity
        End Get
        Set(ByVal Value As Double)
            _cubecapacity = Value
        End Set
    End Property
    Public Property WeightCapacity() As Double
        Get
            Return _weightcapacity
        End Get
        Set(ByVal Value As Double)
            _weightcapacity = Value
        End Set
    End Property
    Public Property Height() As Double
        Get
            Return _height
        End Get
        Set(ByVal Value As Double)
            _height = Value
        End Set
    End Property
    Public Property Width() As Double
        Get
            Return _width
        End Get
        Set(ByVal Value As Double)
            _width = Value
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
#End Region

#Region "Constructor"
    Public Sub New(ByVal pContainer As String, Optional ByVal LoadObj As Boolean = True)
        _container = pContainer
        If LoadObj Then
            Load()
        End If
    End Sub
#End Region

#Region "Methods"
    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM HANDELINGUNITTYPE Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then

        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("CONTAINER") Then _container = dr.Item("CONTAINER")
        If Not dr.IsNull("CONTAINERDESC") Then _containerdesc = dr.Item("CONTAINERDESC")
        If Not dr.IsNull("CONTAINERCUBE") Then _containercube = dr.Item("CONTAINERCUBE")
        If Not dr.IsNull("CONTAINERWEIGHT") Then _containerweight = dr.Item("CONTAINERWEIGHT")
        If Not dr.IsNull("CUBECAPACITY") Then _cubecapacity = dr.Item("CUBECAPACITY")
        If Not dr.IsNull("WEIGHTCAPACITY") Then _weightcapacity = dr.Item("WEIGHTCAPACITY")
        'If Not dr.IsNull("WIDTH") Then _width = dr.Item("WIDTH")
        If Not dr.IsNull("HEIGHT") Then _height = dr.Item("HEIGHT")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

#Region "HandlingUnitStorageTemplate"

    'added for PWMS-418 Start
    Public Shared Function Delete(ByVal pHustoragetemplateid As String)
        Dim oSql As String = String.Format("delete from HANDLINGUNITSTORAGETEMPLATE where HUSTORAGETEMPLATEID='{0}'", pHustoragetemplateid)
        DataInterface.RunSQL(oSql)
    End Function

    'added for PWMS-418 End
#End Region

#End Region

End Class
