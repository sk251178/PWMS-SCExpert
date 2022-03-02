Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class MapObjectColor

    Protected _id As String
    Protected _int As String
    Protected _rgb As String
    Protected Shared _globalColorCache As ColorsHashtable = LoadColorCache()

    Public Shared ReadOnly Property GetColor(ByVal pid As String) As MapObjectColor
        Get
            Return _globalColorCache.Item(pid) ' the point retured should be synced on modifications
        End Get
    End Property

    Public Shared ReadOnly Property GetColors() As ColorsHashtable
        Get
            Return _globalColorCache ' the point retured should be synced on modifications
        End Get
    End Property

    Public ReadOnly Property ID() As String
        Get
            Return _id
        End Get
    End Property

    Public ReadOnly Property count() As Integer
        Get
            Return _globalColorCache.Count
        End Get
    End Property

    Public ReadOnly Property INT_VALUE() As String
        Get
            Return _int
        End Get
    End Property

    Public ReadOnly Property RGB_VALUE() As String
        Get
            Return _rgb
        End Get
    End Property

    Protected Sub New(ByRef dr As DataRow)
        SetObj(dr)
    End Sub

    Protected Sub SetObj(ByRef dr As DataRow)
        If Not dr.IsNull("CODE") Then _id = dr.Item("CODE")
        If Not dr.IsNull("CODE") Then _rgb = dr.Item("CODE")
        _int = Integer.Parse(_rgb, Globalization.NumberStyles.HexNumber)
    End Sub

    Protected Shared Function LoadColorCache() As ColorsHashtable
        Dim ret As ColorsHashtable = New ColorsHashtable
        Dim sql As String = "SELECT * FROM CODELISTDETAIL WHERE CODELISTCODE='MAPCLRPLT'"
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        For Each dr In dt.Rows
            ret.Add(New MapObjectColor(dr))
        Next
        Return ret
    End Function

End Class

#Region "COLORSCACHE"

<CLSCompliant(False)> Public Class ColorsHashtable
    Inherits Hashtable

#Region "Fields"



#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property Item(ByVal key As Object) As MapObjectColor
        Get
            Return CType(MyBase.Item(key), MapObjectColor)
        End Get
    End Property

#End Region

    Public Overloads Sub Add(ByVal obj As MapObjectColor)
        MyBase.Add(CStr(MyBase.Count), obj)
    End Sub









End Class
#End Region
