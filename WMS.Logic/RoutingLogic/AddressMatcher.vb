''Imports System.Data.SqlClient
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class AddressMatcher

    Protected _ratio As Decimal
    Protected _lowerhouse, _upperhouse As Int32
    Protected _distance As Decimal
    Protected _roadid As Int32
    Protected _lowerbound As MapPoint
    Protected _upperbound As MapPoint
    Protected _roadsegments As RoadSegments
    Protected _leftiseven, _isasc As Boolean

    Public Sub New()

    End Sub

    Public Function MatchAddress(ByVal sCityName As String, ByVal sStreetName As String, ByVal iStreetNum As Int32) As MapPoint
        If sCityName = "" Then
            Throw New AddressNotFoundException
        End If
        If sStreetName Is Nothing Then sStreetName = ""
        CorrectAddress(sCityName, sStreetName)
        Dim mp As MapPoint
        Try
            mp = getAddress(sCityName, sStreetName, iStreetNum)
        Catch ex As Exception
        End Try

        If mp Is Nothing Then
            Try
                mp = getLocation(sCityName, sStreetName, iStreetNum)
            Catch ex As Exception
            End Try
        End If

        'If we didnt find the exact street for the current city - get the center of the city
        If mp Is Nothing Then
            Try
                mp = getAddress(sCityName, "", 0)
            Catch ex As Exception
            End Try
        End If

        If mp Is Nothing Then Throw New AddressNotFoundException
        Return mp
    End Function

    Protected Function getLocation(ByVal sCityName As String, ByVal sStreetName As String, ByVal iStreetNum As Int32) As MapPoint
        Dim sql As String = String.Format("select * from is_location where replace(replace(replace(replace(replace(replace(replace(cityname,'יי','י'),'וו','ו'),'''',''),'""',''),'-',''),'(',''),')','') like '{0}' and " & _
                    "replace(replace(replace(replace(replace(replace(replace(locationname,'יי','י'),'וו','ו'),'''',''),'""',''),'-',''),'(',''),')','') like '{1}'", sCityName, sStreetName)

        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New LocationNotFoundException
        End If

        Return New MapPoint(dt.Rows(0)("XCENTER"), dt.Rows(0)("YCENTER"))

    End Function

    Protected Function getAddress(ByVal sCityName As String, ByVal sStreetName As String, ByVal iStreetNum As Int32) As MapPoint
        Dim setMAOffset As Boolean = False
        If isSingleStreet(sCityName) Then
            Return getSingleStreetCenter(sCityName)
        End If
        setMatchAddress(sCityName, sStreetName, iStreetNum)
        setRoadBounds(_roadid)
        Return GetGeoPoint(iStreetNum)
    End Function

    Protected Function isSingleStreet(ByVal sCityName As String) As Boolean
        Dim exists As Boolean
        Dim sql As String = String.Format("select count(1) from vSingleStreetCity where cityname like '{0}'", sCityName)
        exists = DataInterface.ExecuteScalar(sql)
        Return exists
    End Function

    Protected Function getSingleStreetCenter(ByVal sCityName As String) As MapPoint
        Dim mp As New MapPoint
        Dim dt As New DataTable
        Dim sql As String = String.Format("select * from vSingleStreetCity where cityname like '{0}'", sCityName)
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then Return Nothing

        mp.x = dt.Rows(0)("Latitude")
        mp.y = dt.Rows(0)("Longitude")
        Return mp
    End Function

    Protected Sub CorrectAddress(ByRef sCityName As String, ByRef sStreetName As String)
        Dim sql As String
        Dim dt As DataTable
        Dim starr As String()

        sql = String.Format("select * from SYNONYMS where orgword = {0}", Made4Net.Shared.Util.FormatField(sCityName))
        dt = New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            sCityName = dt.Rows(0)("trans")
        Else
            starr = sCityName.Split(" "c)
            For idx As Int32 = 0 To starr.Length - 1
                Dim str As String = starr(idx)
                sql = String.Format("select * from SYNONYMS where orgword = '{0}'", str.Replace("'"c, "''"))
                dt = New DataTable
                DataInterface.FillDataset(sql, dt)
                If dt.Rows.Count > 0 Then
                    starr(idx) = dt.Rows(0)("trans")
                End If
            Next
            sCityName = ""
            For Each str As String In starr
                sCityName = sCityName & str & " "c
            Next
        End If

        sCityName = sCityName.Trim
        sCityName = sCityName.Replace("-", " ")
        sCityName = sCityName.Replace("'", "")
        sCityName = sCityName.Replace("""", "")
        sCityName = sCityName.Replace("(", "")
        sCityName = sCityName.Replace(")", "")
        sCityName = sCityName.Replace(".", "")
        sCityName = sCityName.Replace("/", "")
        sCityName = sCityName.Replace("\", "")
        sCityName = sCityName.Replace("יי", "י").Replace("וו", "ו")
        sCityName = sCityName.Replace("  ", " ")
        sCityName = sCityName.Trim()

        sStreetName = sStreetName.Replace("רח '", "").Replace("רח'", "")
        starr = sStreetName.Split(" "c)
        For idx As Int32 = 0 To starr.Length - 1
            Dim str As String = starr(idx)
            sql = String.Format("select * from SYNONYMS where orgword = '{0}'", str.Replace("'"c, "''"))
            dt = New DataTable
            DataInterface.FillDataset(sql, dt)
            If dt.Rows.Count > 0 Then
                starr(idx) = dt.Rows(0)("trans")
            End If
        Next
        sStreetName = ""
        For Each str As String In starr
            sStreetName = sStreetName & str & " "c
        Next
        sStreetName = sStreetName.Trim
        If sStreetName.IndexOf("רח ") = 0 Then
            sStreetName = sStreetName.Replace("רח ", "")
        End If
        If sStreetName.IndexOf("רחוב ") = 0 Then
            sStreetName = sStreetName.Replace("רחוב ", "")
        End If
        If sStreetName.IndexOf("שד ") = 0 Then
            sStreetName = sStreetName.Replace("שד ", "שדרות ")
        End If
        sStreetName = sStreetName.Replace("יי", "י").Replace("וו", "ו")
        sStreetName = sStreetName.Replace("-", "")
        sStreetName = sStreetName.Replace("'", "")
        sStreetName = sStreetName.Replace("""", "")
        sStreetName = sStreetName.Replace("(", "")
        sStreetName = sStreetName.Replace(")", "")
        sStreetName = sStreetName.Replace(".", "")
        sStreetName = sStreetName.Replace("/", "")
        sStreetName = sStreetName.Replace("\", "")
        sStreetName = sStreetName.Replace("  ", " ")

        sql = String.Format("select * from ALTERNATESTREETS where cityname = '{0}' and street = '{1}'", sCityName, sStreetName.Replace("'"c, "''"))
        dt = New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            sStreetName = dt.Rows(0)("FULLSTREETNAME")
        End If
    End Sub

#Region "GetPoint"

    Protected Function GetGeoPoint(ByVal iStreetNum As Int32) As MapPoint

        Dim ratio As Decimal

        If (_lowerhouse = _upperhouse) Then Return _roadsegments(0)
        ratio = Math.Abs(((iStreetNum - _lowerhouse) / Math.Abs(_lowerhouse - _upperhouse)) * (_roadsegments.Count - 1))
        If ratio >= _roadsegments.Count Then
            ratio = 0
        End If
        If ratio = 0 Or ratio = 1 Then
            Return _roadsegments(ratio)
        Else
            Dim newmp As New MapPoint
            Dim pnt1, pnt2 As MapPoint
            Dim ratx, raty As Decimal
            If _isasc Then
                If ratio.ToString.IndexOf("."c) > -1 Then
                    pnt1 = _roadsegments(Decimal.Floor(ratio))
                    pnt2 = _roadsegments(Decimal.Floor(ratio + 1))
                Else
                    pnt1 = _roadsegments(Decimal.Floor(ratio - 0.1))
                    pnt2 = _roadsegments(Decimal.Floor(ratio))
                End If
                ratx = Math.Abs((pnt2.x - pnt1.x) * (ratio - Decimal.Floor(ratio)))
                raty = Math.Abs((pnt2.y - pnt1.y) * (ratio - Decimal.Floor(ratio)))
            Else
                If ratio.ToString.IndexOf("."c) > -1 Then
                    pnt1 = _roadsegments(Decimal.Floor(ratio + 1))
                    pnt2 = _roadsegments(Decimal.Floor(ratio))
                Else
                    pnt1 = _roadsegments(Decimal.Floor(ratio - 0.1))
                    pnt2 = _roadsegments(Decimal.Floor(ratio))
                End If
                ratx = Math.Abs((pnt1.x - pnt2.x) * (ratio - Decimal.Floor(ratio)))
                raty = Math.Abs((pnt1.y - pnt2.y) * (ratio - Decimal.Floor(ratio)))
            End If

            If pnt1.x > pnt2.x Then
                newmp.x = pnt1.x - ratx
            Else
                newmp.x = pnt1.x + ratx
            End If
            If pnt1.y > pnt2.y Then
                newmp.y = pnt1.y - raty
            Else
                newmp.y = pnt1.y + raty
            End If

            Return newmp
        End If
    End Function

#End Region

#Region "Match Address"

    Protected Sub setMatchAddress(ByVal sCityName As String, ByVal sStreetName As String, ByVal iStreetNum As Int32)
        If sStreetName = "" Then sStreetName = " "
        Dim sql As String = String.Format("select * from vTrimStreets where cityname like '%{0}%' and " & _
            "street like '%{1}%'", sCityName, sStreetName)
        _leftiseven = True
        _isasc = True
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Road not found")
        End If

        Dim dr As DataRow() = dt.Select("fromleft <> '0' and toleft <> '0' and fromright <> '0' and toright <> '0'")
        If dr.Length = 0 Then
            _roadid = dt.Rows(Math.Ceiling(dt.Rows.Count / 2) - 1)("ROADID")
            _distance = dt.Rows(Math.Ceiling(dt.Rows.Count / 2) - 1)("LENGTH")
            _ratio = 0
            dt.Dispose()
            Return
        Else
            If isEven(dr(0)("fromleft")) Then
                _leftiseven = True
            Else
                _leftiseven = False
            End If
            For tasc As Int32 = 0 To dr.Length
                If Not dr(0)("fromleft") = dr(0)("toleft") Then
                    If dr(0)("fromleft") < dr(0)("toleft") Then
                        _isasc = True
                    Else
                        _isasc = False
                    End If
                    Exit For
                End If
            Next
            If isEven(iStreetNum) Then
                If _leftiseven Then
                    If _isasc Then
                        sql = String.Format("fromleft <= {0} and toleft >= {0}", iStreetNum)
                    Else
                        sql = String.Format("fromleft >= {0} and toleft <= {0}", iStreetNum)
                    End If
                Else
                    If _isasc Then
                        sql = String.Format("fromright <= {0} and toright >= {0}", iStreetNum)
                    Else
                        sql = String.Format("fromright >= {0} and toright <= {0}", iStreetNum)
                    End If
                End If
            Else
                If _leftiseven Then
                    If _isasc Then
                        sql = String.Format("fromright <= {0} and toright >= {0}", iStreetNum)
                    Else
                        sql = String.Format("fromright >= {0} and toright <= {0}", iStreetNum)
                    End If
                Else
                    If _isasc Then
                        sql = String.Format("fromleft <= {0} and toleft >= {0}", iStreetNum)
                    Else
                        sql = String.Format("fromleft >= {0} and toleft <= {0}", iStreetNum)
                    End If
                End If
            End If
        End If

        dr = dt.Select(sql)

        If dr.Length = 0 Then
            _roadid = dt.Rows(Math.Ceiling(dt.Rows.Count / 2) - 1)("ROADID")
            _distance = dt.Rows(Math.Ceiling(dt.Rows.Count / 2) - 1)("LENGTH")
            _ratio = 0
            dt.Dispose()
            Return
        End If

        _roadid = dr(0)("ROADID")
        _distance = dr(0)("LENGTH")
        If isEven(iStreetNum) Then
            If _leftiseven Then
                If _isasc Then
                    _lowerhouse = dr(0)("FROMLEFT")
                    _upperhouse = dr(0)("TOLEFT")
                Else
                    _lowerhouse = dr(0)("TOLEFT")
                    _upperhouse = dr(0)("FROMLEFT")
                End If
            Else
                If _isasc Then
                    _lowerhouse = dr(0)("FROMRIGHT")
                    _upperhouse = dr(0)("TORIGHT")
                Else
                    _lowerhouse = dr(0)("TORIGHT")
                    _upperhouse = dr(0)("FROMRIGHT")
                End If
            End If
        Else
            If _leftiseven Then
                If _isasc Then
                    _lowerhouse = dr(0)("FROMRIGHT")
                    _upperhouse = dr(0)("TORIGHT")
                Else
                    _lowerhouse = dr(0)("TORIGHT")
                    _upperhouse = dr(0)("FROMRIGHT")
                End If
            Else
                If _isasc Then
                    _lowerhouse = dr(0)("FROMLEFT")
                    _upperhouse = dr(0)("TOLEFT")
                Else
                    _lowerhouse = dr(0)("TOLEFT")
                    _upperhouse = dr(0)("FROMLEFT")
                End If
            End If
        End If
        dt.Dispose()
    End Sub

    Protected Sub setRoadBounds(ByVal sRoadId As String)
        Dim StrDetailTableName As String = Made4Net.Shared.AppConfig.Get("StrDetailTableName", "is_str_detail")

        Dim sql As String = String.Format("select * from {1} where roadid = '{0}' order by segseq", _roadid, StrDetailTableName)
        Dim dt As New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("No Geocode for road")
        End If

        _roadsegments = New RoadSegments(dt)

        _lowerbound = New MapPoint(dt.Rows(0)("xpos"), dt.Rows(0)("ypos"))
        _upperbound = New MapPoint(dt.Rows(dt.Rows.Count - 1)("xpos"), dt.Rows(dt.Rows.Count - 1)("ypos"))
        dt.Dispose()
    End Sub

#End Region

#Region "Accessors"

    Protected Function isEven(ByVal num As Int32) As Boolean
        Return num Mod 2 = 0
    End Function

    Public Shared Function Int2Deg(ByVal x As Integer) As Double
        x = Math.Floor(x / 100)
        Dim secs, minutes As Integer
        Dim deg As Double = Math.DivRem(x, 3600, minutes)
        minutes = Math.DivRem(minutes, 60, secs)
        Return deg + (minutes + (secs / 60.0)) / 60.0
    End Function

    Public Shared Function Deg2Int(ByVal d As Double) As Double
        Dim deg As Double = Math.Floor(d)
        Dim minutes As Double = Math.Floor((d - deg) * 60)
        Dim secs As Double = Math.Floor(((d - deg) * 60 - minutes) * 60)
        Return Math.Floor((deg * 3600 + minutes * 60 + secs) * 100)
    End Function

#End Region



End Class

#Region "Exception"

Public Class RoadNotFoundException
    Inherits ApplicationException
    Public Sub New()
        MyBase.New("Road not found")
    End Sub
End Class

Public Class LocationNotFoundException
    Inherits ApplicationException
    Public Sub New()
        MyBase.New("Location not found")
    End Sub
End Class

Public Class AddressNotFoundException
    Inherits ApplicationException
    Public Sub New()
        MyBase.New("Address not found")
    End Sub
End Class

#End Region

#Region "MapPoint"

Public Class MapPoint
    Public x As Decimal
    Public y As Decimal

    Public Sub New()

    End Sub

    Public Sub New(ByVal dx As Decimal, ByVal dy As Decimal)
        x = dx
        y = dy
    End Sub

    Public Function toInt() As String
        Return AddressMatcher.Deg2Int(x) & "," & AddressMatcher.Deg2Int(y)
    End Function

    Public ReadOnly Property LatitudeDec() As Double
        Get
            Return AddressMatcher.Deg2Int(y)
        End Get
    End Property

    Public ReadOnly Property LongitudeDec() As Double
        Get
            Return AddressMatcher.Deg2Int(x)
        End Get
    End Property

End Class

#End Region