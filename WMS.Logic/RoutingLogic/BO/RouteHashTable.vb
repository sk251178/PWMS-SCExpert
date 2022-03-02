
#Region "RoutesCollection"

<CLSCompliant(False)> Public Class RouteCollection
    Inherits ArrayList

#Region "Properties"
    Default Public Shadows ReadOnly Property Item(ByVal index As Int32) As Route
        Get
            Return CType(MyBase.Item(index), Route)
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New()
    End Sub
#End Region

#Region "Methods"
    'Public Sub LoadPredefinedRoutes(ByVal vehicleid As Int32)
    '    Dim sql As String = "SELECT * FROM VEHICLEROUTE WHERE PREDEFINED=1 AND VEHICLEID = " + vehicleid.ToString()
    '    Dim dt As New DataTable
    '    Dim dr As DataRow
    '    DataInterface.FillDataset(sql, dt)
    '    For Each dr In dt.Rows
    '        Me.Add(New VehicleRoute(dr))
    '    Next
    'End Sub
    'Public Function GetPredefinedForDay(ByVal forday As DateTime) As VehicleRoute
    '    Dim vsp As VehicleRoute
    '    Dim weekday As DayOfWeek = forday.DayOfWeek
    '    For Each vsp In Me
    '        If vsp.PREDEFINED Then
    '            Dim found = False

    '            Select Case weekday
    '                Case DayOfWeek.Sunday
    '                    found = vsp.SUNDAY
    '                Case DayOfWeek.Monday
    '                    found = vsp.MONDAY
    '                Case DayOfWeek.Tuesday
    '                    found = vsp.TUESDAY
    '                Case DayOfWeek.Wednesday
    '                    found = vsp.WEDNESDAY
    '                Case DayOfWeek.Thursday
    '                    found = vsp.THURSDAY
    '                Case DayOfWeek.Friday
    '                    found = vsp.FRIDAY
    '                Case DayOfWeek.Saturday
    '                    found = vsp.SATURDAY
    '            End Select

    '            If found Then
    '                Return vsp
    '            End If
    '        End If
    '    Next
    '    Return Nothing
    'End Function
#End Region

End Class
#End Region

#Region "RouteHashtable"
<CLSCompliant(False)> Public Class RouteHashtable
    Inherits Hashtable

#Region "Properties"
    Default Public Shadows ReadOnly Property Item(ByVal key As Object) As Route
        Get
            Return CType(MyBase.Item(key), Route)
        End Get
    End Property
#End Region

#Region "Methods"
    Public Overloads Sub Add(ByVal obj As Route)
        MyBase.Add(obj.RouteId, obj)
    End Sub
#End Region

End Class

#End Region
