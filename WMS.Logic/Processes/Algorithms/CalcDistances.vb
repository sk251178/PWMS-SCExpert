Imports Made4Net.Algorithms
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data
Imports System.IO

Public Class CalcDistances
    ' Start RWMS-1212
#Region "Variables"
    Protected _edgeid As Integer
    Protected _fromnodeid As String
    Protected _tonodeid As String
    Protected _distance As Double
    Protected _edgetype As String
    Protected _clearance As Double
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty
#End Region

#Region "Properties"
    '
    Public Property EDGEID() As Integer
        Set(ByVal Value As Integer)
            _edgeid = Value
        End Set
        Get
            Return _edgeid
        End Get
    End Property
    Public Property FROMNODEID() As String
        Set(ByVal Value As String)
            _fromnodeid = Value
        End Set
        Get
            Return _fromnodeid
        End Get
    End Property
    Public Property TONODEID() As String
        Set(ByVal Value As String)
            _tonodeid = Value
        End Set
        Get
            Return _tonodeid
        End Get
    End Property
    Public Property DISTANCE() As Double
        Set(ByVal Value As Double)
            _distance = Value
        End Set
        Get
            Return _distance
        End Get
    End Property
    Public Property EDGETYPE() As String
        Set(ByVal Value As String)
            _edgetype = Value
        End Set
        Get
            Return _edgetype
        End Get
    End Property
    Public Property CLEARANCE() As String
        Set(ByVal Value As String)
            _clearance = Value
        End Set
        Get
            Return _clearance
        End Get
    End Property
    Public Property ADDDATE() As Date
        Set(ByVal Value As Date)
            _adddate = Value
        End Set
        Get
            Return _adddate
        End Get
    End Property
    Public Property ADDUSER() As String
        Set(ByVal Value As String)
            _adduser = Value
        End Set
        Get
            Return _adduser
        End Get
    End Property
    Public Property EDITDATE() As Date
        Set(ByVal Value As Date)
            _editdate = Value
        End Set
        Get
            Return _editdate
        End Get
    End Property
    Public Property EDITUSER() As String
        Set(ByVal Value As String)
            _edituser = Value
        End Set
        Get
            Return _edituser
        End Get
    End Property

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("EDGE_ID = '{0}'", _edgeid)
        End Get
    End Property
#End Region
    ' End RWMS-1212
#Region "Constructors"
    ' Start RWMS-1212
    Public Sub New(ByVal pEdgeId As Int32)
        _edgeid = pEdgeId
        Load()
    End Sub
    ' End RWMS-1212

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByRef Message As String)

        Dim distanceCalc As New DistanceCalculation()
        Dim t As New Translation.Translator()
        Dim oLogger As ILogHandler

        If CommandName.ToLower = "shortestpathcalc" Then
            Try
                Dim dirPath As String
                Dim logPath As String = AppConfig.GetSystemParameter(ConfigurationSettingsConsts.ShortestPathCalcLogDirectory)
                dirPath = Made4Net.DataAccess.Util.BuildAndGetFilePath(logPath)
                oLogger = New LogHandler(dirPath, "ShortestpathcalcLog_" & DateTime.Now.ToString("ddMMyyyy_hhmmss") & ".txt")
                distanceCalc.CalculateShortestPathBetweenSourceAndTarget(oLogger)
                Message = t.Translate("Shortest Distance Calculation Done Successfully.")
            Catch ex As M4NException
                Message = ex.Message.ToString()
            Catch ex As Exception
                Message = ex.Message.ToString()
            End Try

        ElseIf CommandName.ToLower = "distcalc" Then
            Try
                calcEdgeDistances()
                Message = t.Translate("Edge Distance Calculation Done Successfully.")
            Catch ex As M4NException
                Message = ex.Message.ToString()
            Catch ex As Exception
                Message = ex.Message.ToString()
            End Try
        End If

    End Sub

#End Region
    'Start RWMS-1212
    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM WAREHOUSEMAPEDGES WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Edge does not exists", "Edge does not exists")
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("DISTANCE") Then DISTANCE = dr.Item("DISTANCE")
        If Not dr.IsNull("EDGETYPE") Then EDGETYPE = dr.Item("EDGETYPE")
        If Not dr.IsNull("CLEARANCE") Then CLEARANCE = dr.Item("CLEARANCE")
    End Sub
    'End RWMS-1212
    Public Function calcEdgeDistances() As Integer

        Dim dt1 As New DataTable
        Dim dt As New DataTable
        Dim dr As DataRow
        dt = New DataTable()
        dt1 = New DataTable()
        Dim SQL1 As String
        Dim SQL As String

        'SQL = String.Format(" WITH CTE AS ( SELECT FROMNODEID, TONODEID, B.XCOORDINATE, B.YCOORDINATE FROM WAREHOUSEMAPEDGES A" & _
        '                   " INNER JOIN WAREHOUSEMAPNODES B ON ( A.FROMNODEID = B.NODEID ) OR ( A.TONODEID = B.NODEID ))" & _
        '                     "SELECT DISTINCT B.FROMNODEID,B.TONODEID," & _
        '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 1, 5) AS X1," & _
        '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 5, 5) AS Y1," & _
        '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 9, 5) AS X2," & _
        '                                      "SUBSTRING( STUFF(( SELECT ';' + CAST(XCOORDINATE AS CHAR(5)) + ';' + CAST(YCOORDINATE AS CHAR(5) ) FROM CTE A1 WHERE A1.FROMNODEID = B.FROMNODEID AND A1.TONODEID = B.TONODEID FOR XML PATH('')), 1,1, '') , 13, 5) AS Y2" + " " + " FROM CTE B")

        SQL = String.Format("SELECT * FROM ( SELECT  B.FROMNODEID, B.XCOORDINATE AS X1, B.YCOORDINATE AS Y1, C.TONODEID, C.XCOORDINATE AS X2, C.YCOORDINATE AS Y2 FROM WAREHOUSEMAPEDGES A" &
                             " LEFT JOIN ( SELECT DISTINCT FROMNODEID, XCOORDINATE, YCOORDINATE FROM  WAREHOUSEMAPEDGES A" &
                             " JOIN WAREHOUSEMAPNODES B ON A.FROMNODEID = B.NODEID ) AS B ON B.FROMNODEID = A.FROMNODEID" &
                             " LEFT JOIN ( SELECT DISTINCT TONODEID, XCOORDINATE, YCOORDINATE FROM  WAREHOUSEMAPEDGES A JOIN  WAREHOUSEMAPNODES B ON    A.TONODEID = B.NODEID ) AS C ON C.TONODEID = A.TONODEID ) MapEdgePoints where MapEdgePoints.FROMNODEID IS NOT NULL AND MapEdgePoints.TONODEID IS NOT NULL")

        DataInterface.FillDataset(SQL, dt1)

        If (dt1.Rows.Count = 0) Then
            Return -1
        Else

            For Each dr In dt1.Rows

                Dim dist As Double
                dist = WHQueryPath.CalcDistance(Convert.ToInt32(dr("X1")),
                                           Convert.ToInt32(dr("Y1")),
                                           Convert.ToInt32(dr("X2")),
                                           Convert.ToInt32(dr("Y2")))

                SQL1 = String.Format("UPDATE WAREHOUSEMAPEDGES SET DISTANCE={0} WHERE FROMNODEID='{1}' and TONODEID='{2}'", Math.Round(dist, 2, MidpointRounding.ToEven), dr("FROMNODEID"), dr("TONODEID"))
                DataInterface.RunSQL(SQL1)
                Made4Net.DataAccess.DataInterface.RunSQL(SQL1)

            Next

        End If


        Return 1



    End Function

    'Start RWMS-1212
    Private Function CheckEdgeExist(ByVal pFromNodeId As String, ByVal pToNodeID As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(*) FROM WAREHOUSEMAPEDGES WHERE FROMNODEID={0} AND TONODEID={1}", FormatField(pFromNodeId), FormatField(pToNodeID))
        Return DataInterface.ExecuteScalar(sql)
    End Function
    Private Function CheckReverseEdgeExist(ByVal pFromNodeId As String, ByVal pToNodeID As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(*) FROM WAREHOUSEMAPEDGES WHERE FROMNODEID={0} AND TONODEID={1}", FormatField(pToNodeID), FormatField(pFromNodeId))
        Return DataInterface.ExecuteScalar(sql)
    End Function
    ' End RWMS-1212
    'Start RWMS-1212
#Region "Create / Update"
    Public Sub CreateEdge(ByVal pFromNodeId As String, ByVal pToNodeId As String, ByVal pDistance As Double, ByVal pEdgeType As String, ByVal pUser As String, ByVal pClearance As Double)
        If CheckEdgeExist(pFromNodeId, pToNodeId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Edge  - Already Exists", "Cannot Create Edge - Already Exists")
        End If
        If CheckReverseEdgeExist(pFromNodeId, pToNodeId) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Cannot Create Edge - Reverse Edge Exists", "Cannot Create Edge - Reverse Edge Exists")
        End If
        If pFromNodeId.ToLower() = pToNodeId.ToLower() Then
            Throw New Made4Net.Shared.M4NException(New Exception, "From Node and To Node Can Not Be Same", "From Node and To Node Can Not Be Same")
        End If
        FROMNODEID = pFromNodeId
        TONODEID = pToNodeId
        DISTANCE = pDistance
        EDGETYPE = pEdgeType
        CLEARANCE = pClearance
        ADDDATE = DateTime.Now
        ADDUSER = pUser
        Dim sql As String = String.Format("INSERT INTO WAREHOUSEMAPEDGES (FROMNODEID,TONODEID,DISTANCE,EDGETYPE,ADDDATE,ADDUSER,CLEARANCE) VALUES ({0},{1},{2},{3},{4},{5},{6})",
                   FormatField(FROMNODEID), FormatField(TONODEID), FormatField(DISTANCE), FormatField(EDGETYPE), FormatField(ADDDATE), FormatField(ADDUSER), FormatField(CLEARANCE))
        DataInterface.RunSQL(sql)

    End Sub
    Public Sub UpdateEdge(ByVal pDistance As Double, ByVal pEdgeType As String, ByVal pUser As String, ByVal pClearance As Double)

        DISTANCE = pDistance
        EDGETYPE = pEdgeType
        CLEARANCE = pClearance
        EDITDATE = DateTime.Now
        EDITUSER = pUser
        Dim sql As String = String.Format("UPDATE WAREHOUSEMAPEDGES SET DISTANCE={0},EDGETYPE={1}, CLEARANCE={2},EDITDATE={3},EDITUSER={4} WHERE {5}",
                   FormatField(DISTANCE), FormatField(EDGETYPE), FormatField(CLEARANCE), FormatField(EDITDATE), FormatField(EDITUSER), WhereClause)
        DataInterface.RunSQL(sql)

    End Sub
#End Region
    'End RWMS-1212
End Class