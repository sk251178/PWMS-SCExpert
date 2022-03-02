Imports System.Xml
Imports System.Xml.XPath
Imports System.IO
Imports System.Text
Imports system.drawing.imaging
Imports system.drawing.drawing2d
Imports WMS.Logic
Imports Made4Net.DataAccess

<CLSCompliant(False)> Partial Class Map
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private m_visible() As String
    Private m_params As RequestParams

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        m_params = CType(Session("paramObject"), RequestParams)
        If m_params Is Nothing Then
            ' get default value for serverobject from localhost
            Dim gotFirstMapServer As Boolean = False

            m_params = New RequestParams
        End If
        m_params.GetParams(Page.Request.Params)

        If m_params.Width = 0 Then m_params.Width = 500
        If m_params.Height = 0 Then m_params.Height = 479

        Session.Add("paramObject", m_params)
        ProcessThisRequest()


    End Sub

    '/ <summary>
    '/ Processes incoming request and writes out xml response
    '/ </summary>
    Private Sub ProcessThisRequest()
        Dim action As String = m_params.RequestType
        Dim res As String = ""

        If (m_params.MapType.Equals("overview")) Then
            action = "overview"
        End If

        If action.Equals("defaultextent") Then
            res = WrapAGSResponseNodes(DefaultExtent())
        ElseIf action.Equals("fullextent") Then
            res = WrapAGSResponseNodes(FullExtent())
        ElseIf action.Equals("zoomtoextent") Then
            res = WrapAGSResponseNodes(ZoomToExtent())
        ElseIf action.Equals("overview") Then
            res = WrapAGSResponseNodes(Overview())
        ElseIf action.Equals("initialize") Then
            res = Initialize()
        ElseIf action.Equals("pointmoved") Then
            res = WrapAGSResponseNodes(ActionPointMoved())
        ElseIf action.Equals("pointcreate") Then
            res = WrapAGSResponseNodes(ActionPointCreate())
        ElseIf action.Equals("netconnect") Then
            res = WrapAGSResponseNodes(ActionNetConnect())
        ElseIf action.Equals("netitemdeleted") Then
            res = WrapAGSResponseNodes(ActionNetitemDelete())
        ElseIf action.Equals("pointdeleted") Then
            res = WrapAGSResponseNodes(ActionPointDelete())
            'ElseIf action.Equals("mapmodedata") Then
            'res = WrapAGSResponseNodes(LoadMapModeData())
        ElseIf action.Equals("locate_by_coord") Then
            res = WrapAGSResponseNodes(locateByCoord())
        ElseIf action.Equals("locate_by_pointid") Then
            res = WrapAGSResponseNodes(locateByPointId())
        ElseIf action.Equals("add_points") Then
            res = WrapAGSResponseNodes(addPoints())
        ElseIf action.Equals("add_net_items") Then
            res = WrapAGSResponseNodes(addNetItems())
        ElseIf action.Equals("add_routes") Then
            res = addRoutes()
        ElseIf action.Equals("add_route_set") Then
            res = addRouteSet()
        ElseIf action.Equals("load_territory") Then
            res = WrapAGSResponseNodes(loadTerritory())
        ElseIf action.Equals("save_territory") Then
            res = WrapAGSResponseNodes(saveTerritory())
        ElseIf action.Equals("save_itempoint") Then
            res = WrapAGSResponseNodes(saveItemPoint())
        ElseIf action.Equals("map_clear") Then
            res = WrapAGSResponseNodes(ClearMapObjects())
        End If

        If res.Length > 0 Then
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.Write(res)
        End If
    End Sub 'ProcessRequest

    Public Function ClearMapObjects() As String
        m_params.RoutesXML = ""
        m_params.Routes.Clear()
        m_params.Points.Clear()
        m_params.NetItems.Clear()
        Dim sb As New StringBuilder
        Return WrapAGSResponseNodes(AppendMapData(sb).ToString())
    End Function

    Public Function loadTerritory() As String
        Dim sb As New StringBuilder
        Return AppendTerritoryData(sb).ToString
    End Function

    Public Function saveTerritory() As String
        Dim rid As String() = m_params.ActionParams.Split(",")
        Dim ter As Made4Net.GeoData.GeoTerritory = Made4Net.GeoData.GeoTerritory.GetTerritories(Integer.Parse(rid(0)))
        Dim mpc As Made4Net.GeoData.GeoPointCollection = ter.BOUNDARY
        Dim i As Integer
        mpc.Clear()
        For i = 1 To rid.Length
            mpc.Add(Made4Net.GeoData.GeoPoint.GetPoint(Integer.Parse(rid(i))))
        Next
        ter.Update()
        Return ""
    End Function

    Public Function addRoutes() As String
        Dim sb As StringBuilder = New StringBuilder
        Dim rid As String() = m_params.ActionParams.Split("_")
        Dim VehicleRouteXml, AllObjXml As String
        If rid.Length <= 0 Then
            Return ""
        End If
        If rid(0) = "VehicleRoute" Then
            Dim vehicleid As String = rid(1).Split("@")(0)
            Dim fromDate As DateTime = rid(1).Split("@")(1)
            Dim toDate As DateTime = rid(1).Split("@")(2)
            VehicleRouteXml = BuildVehicleRouteXML(vehicleid, fromDate, toDate)
            AllObjXml = WrapAGSResponseNodes(AppendMapData(sb).ToString())
            Dim oDoc As New XmlDocument
            oDoc.LoadXml(AllObjXml)
            VehicleRouteXml &= oDoc.SelectSingleNode("AGS_CONNECTION/RESPONSE/routes").InnerXml
            m_params.RoutesXML &= VehicleRouteXml
            oDoc.SelectSingleNode("AGS_CONNECTION/RESPONSE/routes").RemoveAll()
            oDoc.SelectSingleNode("AGS_CONNECTION/RESPONSE/routes").InnerXml = VehicleRouteXml
            Return oDoc.InnerXml
        ElseIf rid(0) = "RouteSet" Then
            Return addRouteSet()
        ElseIf rid(0) = "Vehicle" Then
            Dim lat As String = m_params.ActionParams.Split("_")(1).Split("@")(0)
            Dim lon As String = m_params.ActionParams.Split("_")(1).Split("@")(1)
            Dim vehicleId As String = m_params.ActionParams.Split("_")(1).Split("@")(2)
            VehicleRouteXml = BuildVehiclePositionXML(vehicleId, lat, lon)
            Dim zoommap As String = locateByCoord_internal(lon.ToString() + "," + lat.ToString())
            AllObjXml = WrapAGSResponseNodes(AppendMapData(sb).ToString())
            Dim oDoc As New XmlDocument
            Dim oMap As New XmlDocument
            oDoc.LoadXml(AllObjXml)
            oMap.LoadXml(zoommap)
            VehicleRouteXml &= oMap.SelectSingleNode("MAP/routes").InnerXml
            m_params.RoutesXML &= VehicleRouteXml
            oMap.SelectSingleNode("MAP/routes").RemoveAll()
            oMap.SelectSingleNode("MAP/routes").InnerXml = VehicleRouteXml
            oDoc.SelectSingleNode("AGS_CONNECTION/RESPONSE").RemoveAll()
            oDoc.SelectSingleNode("AGS_CONNECTION/RESPONSE").InnerXml = oMap.InnerXml
            Return oDoc.InnerXml
        Else
            Dim tmpRoute As String = rid(1)
            If Not m_params.Routes.Contains(tmpRoute) Then
                Try
                    'Dim vr As VehicleRoute = New VehicleRoute(Integer.Parse(s))
                    Dim vr As WMS.Logic.Route = New WMS.Logic.Route(tmpRoute)
                    If Not vr Is Nothing Then
                        m_params.ApplyNextRouteColor()
                        vr.Color = MapObjectColor.GetColor(CStr(m_params.RouteColorID)).INT_VALUE
                        m_params.Routes.Add(vr)
                    End If
                Catch ex As Exception
                End Try
            End If
            Return WrapAGSResponseNodes(AppendMapData(sb).ToString())
        End If
    End Function

    Public Function addRouteSet() As String
        Dim sb As StringBuilder = New StringBuilder
        Dim rid As String() = m_params.ActionParams.Split("_")
        If rid.Length <= 0 Then
            Return ""
        End If
        Dim tmpRouteSet As String = rid(1)
        Dim tmpRoute As String
        Dim dt As New DataTable
        Dim sql As String = String.Format("Select distinct routeid from route where routeset = '{0}' and status <> 'Canceled'", tmpRouteSet)
        DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            tmpRoute = dr("routeid")
            If Not m_params.Routes.Contains(tmpRoute) Then
                Try
                    Dim vr As WMS.Logic.Route = New WMS.Logic.Route(tmpRoute)
                    If Not vr Is Nothing Then
                        m_params.ApplyNextRouteColor()
                        vr.Color = MapObjectColor.GetColor(CStr(m_params.RouteColorID)).INT_VALUE
                        m_params.Routes.Add(vr)
                    End If
                Catch ex As Exception
                End Try
            End If
        Next
        Return WrapAGSResponseNodes(AppendMapData(sb).ToString())
    End Function

    Public Function addNetItems() As String
        Dim tni As String() = m_params.ActionParams.Split(",")
        Dim s As String

        If tni.Length <= 0 Then
            Return ""
        End If

        If tni(0) = "-1" Then
            m_params.NetItems = New Made4Net.GeoData.GeoNetworkHashtable
        End If

        If tni(0) = "0" Then
            m_params.NetItems = Made4Net.GeoData.GeoNetworkItem.GetNetwork()
        Else

            For Each s In tni
                If Not m_params.NetItems.Contains(s) Then
                    Try
                        Dim ids As String() = s.Split("@")
                        If ids.Length = 2 Then
                            Dim mp As Made4Net.GeoData.GeoNetworkItem = Made4Net.GeoData.GeoNetworkItem.GetNetworkItem(Integer.Parse(ids(0)), Integer.Parse(ids(1)))
                            If Not mp Is Nothing Then
                                m_params.NetItems.Add(mp)
                            End If
                        End If
                    Catch ex As LogicException
                    End Try
                End If
            Next
        End If
        Dim sb As StringBuilder = New StringBuilder
        Return AppendMapData(sb).ToString()
    End Function

    Public Function addPoints() As String
        Dim tps As String() = m_params.ActionParams.Split(",")
        Dim s As String

        If tps.Length <= 0 Then
            Return ""
        End If

        If tps(0) = "-1" Then
            m_params.Points = New Made4Net.GeoData.GeoPointHashtable
        Else

            If tps(0) = "0" Then
                ' load all the stuff currently on the screen !!!
                m_params.Points = Made4Net.GeoData.GeoPoint.GetPoints()
            Else
                For Each s In tps
                    If Not m_params.Points.Contains(s) Then
                        Try
                            'Dim mp As MapPoint = MapPoint.GetPoint(s)
                            Dim mp As Made4Net.GeoData.GeoPoint = New Made4Net.GeoData.GeoPoint(s)
                            If Not mp Is Nothing Then
                                m_params.Points.Add(mp)
                            End If
                        Catch ex As LogicException
                        End Try
                    End If
                Next
            End If
        End If

        Dim sb As StringBuilder = New StringBuilder
        Return AppendMapData(sb).ToString()
    End Function

    Public Function locateByCoord() As String
        Return locateByCoord_internal(m_params.ActionParams)
    End Function

    Public Function locateByCoord_internal(ByRef s As String) As String
        Dim par As String = s
        Dim fcom As Integer = par.IndexOf(",")
        Dim longitude As String = par.Substring(0, fcom)
        Dim latitude As String = par.Substring(fcom + 1)

        m_params.MinX = Double.Parse(longitude) - 1
        m_params.MaxX = m_params.MinX + 2
        m_params.MinY = Double.Parse(latitude) + 1
        m_params.MaxY = m_params.MinY - 2
        m_params.IsRealZoom = True '' in order to search for the best map
        Return ZoomToExtent()
    End Function

    Public Function locateByPointId() As String
        Dim pType As String = m_params.ActionParams.Split("_")(0)
        If pType.ToLower = "vehicle" Then
            Try
                Dim lat As String = m_params.ActionParams.Split("_")(1).Split("@")(0)
                Dim lon As String = m_params.ActionParams.Split("_")(1).Split("@")(1)
                Dim vehicleId As String = m_params.ActionParams.Split("_")(1).Split("@")(2)
                Return LocateVehicle(vehicleId, lon, lat)
            Catch ex As Exception
            End Try
        Else
            Try
                Dim pid As String = m_params.ActionParams.Split("_")(1)
                Dim mp As Made4Net.GeoData.GeoPoint = Made4Net.GeoData.GeoPointNode.GetPoints(Convert.ToInt32(pid))
                If Not mp Is Nothing Then
                    If (Not m_params.Points.Contains(mp.POINTID)) Then
                        m_params.Points.Add(mp)
                    End If
                    Return locateByCoord_internal(mp.LONGITUDE.ToString() + "," + mp.LATITUDE.ToString())
                End If
            Catch ex As Exception
            End Try
        End If
        Return ""
    End Function

    Private Function LocateVehicle(ByVal VehicleId As String, ByVal lon As String, ByVal lat As String) As String
        'Draw the point on the map
        Dim mp As New Made4Net.GeoData.GeoPoint
        mp.POINTID = New Random().Next()
        mp.POINTTYPEID = "Company"
        mp.LONGITUDE = lon
        mp.LATITUDE = lat
        m_params.TempPoints = m_params.Points
        m_params.Points.Clear()
        m_params.Points.Add(mp)
        Dim sb As StringBuilder = New StringBuilder
        AppendMapData(sb).ToString()
        Return locateByCoord_internal(lon.ToString() + "," + lat.ToString())
    End Function

    Public Function ActionPointMoved() As String
        ' update the mappoint
        ' do not return anythng
        Dim par As String = m_params.ActionParams
        Dim fcom As Integer = par.IndexOf(",")
        Dim fsec As Integer = par.IndexOf(",", fcom + 1)

        Dim pid As String = par.Substring(0, fcom)
        Dim longitude As String = par.Substring(fcom + 1, fsec - fcom - 1)
        Dim latitude As String = par.Substring(fsec + 1)

        Try
            Dim mp As Made4Net.GeoData.GeoPoint = Made4Net.GeoData.GeoPoint.GetPoint(pid)
            ' the flash always puts DOT here !!!
            If Not mp Is Nothing Then
                SyncLock mp
                    mp.LONGITUDE = Integer.Parse(longitude.Substring(0, longitude.IndexOf(".")))
                    mp.LATITUDE = Integer.Parse(latitude.Substring(0, latitude.IndexOf(".")))
                    mp.Update(Common.GetCurrentUser)
                End SyncLock
            End If
        Catch ex As LogicException
            ' somebody has deleted the point most probably!!!
        End Try
        Return ""
    End Function

    Public Function ActionPointCreate() As String
        Dim sb As New System.Text.StringBuilder
        Dim mp As New Made4Net.GeoData.GeoPoint

        Dim par As String = m_params.ActionParams
        Dim fcom As Integer = par.IndexOf(",")
        Dim fsec As Integer = par.IndexOf(",", fcom + 1)
        Dim fthird As Integer = par.IndexOf(",", fsec + 1)
        Dim pid As String = par.Substring(0, fcom)
        Dim longitude As String = par.Substring(fcom + 1, fsec - fcom - 1)
        Dim latitude As String = par.Substring(fsec + 1, fthird - fsec - 1)
        Dim type As String = par.Substring(fthird + 1)

        Dim nn As Integer = longitude.IndexOf(".")
        If (nn > 0) Then
            mp.LONGITUDE = Integer.Parse(longitude.Substring(0, nn))
        Else
            mp.LONGITUDE = Integer.Parse(longitude)
        End If
        nn = latitude.IndexOf(".")
        If (nn > 0) Then
            mp.LATITUDE = Integer.Parse(latitude.Substring(0, latitude.IndexOf(".")))
        Else
            mp.LATITUDE = Integer.Parse(latitude)
        End If
        mp.POINTTYPEID = type

        Dim id As String = mp.Create(Common.GetCurrentUser)
        sb.Append("<pointcreated pid=""" + pid + """ dbpid=""" + mp.POINTID.ToString() + """ />" + ControlChars.Lf)

        If Not m_params.Points.Contains(mp.POINTID) Then
            Try
                m_params.Points.Add(mp)
            Catch ex As LogicException
            End Try
        End If
        Return sb.ToString()
    End Function

    Public Function saveItemPoint() As String
        Try
            Dim sb As New System.Text.StringBuilder
            Dim par As String() = m_params.ActionParams.Split(",")
            Dim pid As String = par(0)
            'Dim pidtype As String = par(1)
            'Dim objectdata As String = par(2)
            Dim longitude As Double = par(2)
            Dim latitude As Double = par(1)
            Dim isNew As Boolean = False
            Dim pidtype As String = pid.Split("_")(0)
            Dim objectdata As String = pid.Split("_")(1)

            Dim mp As New Made4Net.GeoData.GeoPointNode
            If Made4Net.GeoData.GeoPoint.GeoPointExist(longitude, latitude) Then
                Dim tmpId As String = Made4Net.GeoData.GeoPointNode.GetGeoPointId(longitude, latitude)
                mp = Made4Net.GeoData.GeoPointNode.GetPoints(Convert.ToInt32(tmpId))
            Else
                isNew = True
                Made4Net.GeoData.GeoPointNode.GetPoints()
                mp = New Made4Net.GeoData.GeoPointNode
                mp.LONGITUDE = longitude
                mp.LATITUDE = latitude
                mp.POINTTYPEID = pidtype
                Try
                    mp.Create(Common.GetCurrentUser)
                Catch ex As Exception
                End Try
            End If

            If Not m_params.Points.Contains(mp.POINTID) Then
                Try
                    m_params.Points.Add(mp)
                Catch ex As Exception
                End Try
            Else
                Try
                    m_params.Points.Remove(mp.POINTID)
                    m_params.Points.Add(mp)
                Catch ex As Exception
                End Try
            End If

            If isNew Then
                If pidtype.ToLower = "company" Then
                    Dim objectdataarray As String() = objectdata.Split("@")
                    Dim comType As String = objectdataarray(2)
                    Dim comID As String = objectdataarray(1)
                    Dim comCons As String = objectdataarray(0)
                    Dim cmp As New WMS.Logic.Company(comCons, comID, comType, True)
                    cmp.Pin(cmp.DEFAULTCONTACT, mp.POINTID, WMS.Logic.Common.GetCurrentUser)
                    ''cmp.Save()

                    cmp = Nothing
                ElseIf pidtype.ToLower = "contact" Then
                    Dim objectdataarray As String() = objectdata.Split("@")
                    Dim contactID As String = objectdataarray(0)
                    Dim oContact As New WMS.Logic.Contact(contactID)
                    oContact.setPointId(mp.POINTID, WMS.Logic.Common.GetCurrentUser)
                    oContact = Nothing

                ElseIf pidtype = "DPT" Then
                    Dim dpt As New WMS.Logic.Depots(objectdata, True)
                    dpt.SetPoint(pid, WMS.Logic.Common.GetCurrentUser)
                    dpt = Nothing
                ElseIf pidtype = "DRV" Then
                    Dim drv As New WMS.Logic.Driver(objectdata, True)
                    drv.POINTID = pid
                    drv.SetStartingPoint(pid, WMS.Logic.Common.GetCurrentUser)
                    drv = Nothing
                End If
            End If

            Return AppendPointsData(sb).ToString()

        Catch ex As Exception
            Return ex.Message()
        End Try
    End Function

    Public Function ActionNetConnect() As String
        ' insert a mapnetwork and do not return anything

        Dim par As String = m_params.ActionParams
        Dim fcom As Integer = par.IndexOf(",")
        Dim pid As String = par.Substring(0, fcom)
        Dim nextpid As String = par.Substring(fcom + 1)

        Dim mni As Made4Net.GeoData.GeoNetworkItem
        Try
            mni = New Made4Net.GeoData.GeoNetworkItem
        Catch ex As Exception
            mni = New Made4Net.GeoData.GeoNetworkItem
        End Try

        mni.POINTID = pid
        mni.NEXTPOINTID = nextpid

        mni.AVERAGESPEED = 0
        mni.DRIVINGDIST = Int32.MaxValue
        mni.DRIVINGTIME = Int32.MaxValue

        mni.Create(Common.GetCurrentUser)

        Dim s As String = mni.POINTID.ToString() + "@" + mni.NEXTPOINTID.ToString()
        If Not m_params.NetItems.Contains(s) Then
            Try
                m_params.NetItems.Add(mni)
            Catch ex As LogicException
            End Try
        End If

        Return ""
    End Function

    Public Function ActionNetitemDelete() As String
        Dim par As String = m_params.ActionParams
        Dim fcom As Integer = par.IndexOf(",")
        Dim pid As String = par.Substring(0, fcom)
        Dim nextpid As String = par.Substring(fcom + 1)

        Dim mni As Made4Net.GeoData.GeoNetworkItem = Made4Net.GeoData.GeoNetworkItem.GetNetworkItem(pid, nextpid)

        Dim s As String = mni.POINTID.ToString() + "@" + mni.NEXTPOINTID.ToString()

        mni.Delete()

        ' remvoe it from the currently selected list
        If m_params.NetItems.Contains(s) Then
            m_params.NetItems.Remove(s)
        End If

        Return ""
    End Function

    Public Function ActionPointDelete() As String
        Dim par As String = m_params.ActionParams
        Dim pid As String = par

        Dim mp As Made4Net.GeoData.GeoPoint = Made4Net.GeoData.GeoPoint.GetPoint(Integer.Parse(pid))
        If Not mp Is Nothing Then
            mp.Delete()
        End If


        Return ""
    End Function

    '/ <summary>
    '/ Zooms to default full extent
    '/ </summary>
    '/ <returns>XML string containing image url and new extent</returns>
    Public Function FullExtent() As String
        Dim sb As New System.Text.StringBuilder

        Dim mp As Made4Net.Maps.IMap = Made4Net.Maps.MapManager.GetHomeMap()

        If Not mp Is Nothing Then
            m_params.MinX = mp.TopLeftX
            m_params.MaxX = mp.BottomRightX
            m_params.MinY = mp.TopLeftY
            m_params.MaxY = mp.BottomRightY
            Dim bmp As Bitmap = mp.getBitmap(m_params.MinX, m_params.MinY, m_params.MaxX, m_params.MaxY, m_params.Width, m_params.Height)
            'Dim bmp As Bitmap = mp.getBitmap(m_params.Width, m_params.Height)
            If Not bmp Is Nothing Then
                sb.Append(WriteImageNodes2("home", mp, bmp))
            End If
        End If
        Return sb.ToString()
    End Function 'FullExtent


    '/ <summary>
    '/ Zoom to default extent
    '/ </summary>
    '/ <returns>XML string containing image url and new extent</returns>
    Public Function DefaultExtent() As String
        Return FullExtent()
    End Function 'DefaultExtent

    Private Function Initialize() As String
        Dim sb As New System.Text.StringBuilder
        sb.Append("<?xml version=""1.0"" encoding=""utf-8"" ?> " + ControlChars.Lf)

        sb.Append("<PARAMETERS>" + ControlChars.Lf)

        sb.Append("<TITLE value=""Routing Map Viewer"" />" + ControlChars.Lf)

        ' this is just for debugging purposes
        Dim port As String = ""
        If Request.Url.Host.Equals("inray.spnet.net") Then
            port = ":9091"
        End If

        sb.Append("<REQUEST_URL path=""http://" + Request.Url.Host + port + Request.Url.AbsolutePath.Replace("/map.aspx", "") + "/map.aspx"" />" + ControlChars.Lf)

        sb.Append("<MAP instance_name=""Map""" + ControlChars.Lf)
        sb.Append("host = """ + Request.Url.Host + """" + ControlChars.Lf)
        sb.Append("serverobject = ""world""" + ControlChars.Lf)
        sb.Append("dataframe = ""MainMap""" + ControlChars.Lf)
        sb.Append("map_index = ""0""" + ControlChars.Lf)
        sb.Append("map_type = ""map""" + ControlChars.Lf)
        sb.Append("image_width = ""705""" + ControlChars.Lf)
        sb.Append("image_height = ""450""" + ControlChars.Lf)
        sb.Append("three_d_look = ""false""" + ControlChars.Lf)
        sb.Append("unique_url = """"" + ControlChars.Lf)
        sb.Append("map_to_overview_connection = ""Overview""" + ControlChars.Lf)
        sb.Append("map_to_map_connection = """"" + ControlChars.Lf)
        sb.Append("data_display = ""DataDisplay""" + ControlChars.Lf)
        sb.Append("/>" + ControlChars.Lf)

        sb.Append("<IMAGE format=""JPG"" />" + ControlChars.Lf)
        sb.Append("<TOLERANCE value=""3"" />" + ControlChars.Lf)

        sb.Append("<MAP instance_name=""Overview""" + ControlChars.Lf)
        sb.Append("host = """ + Request.Url.Host + """" + ControlChars.Lf)
        sb.Append("serverobject = ""world""" + ControlChars.Lf)
        sb.Append("dataframe = ""LocationMap""" + ControlChars.Lf)
        sb.Append("map_index = ""1""" + ControlChars.Lf)
        sb.Append("map_type = ""overview""" + ControlChars.Lf)
        sb.Append("image_width = ""300""" + ControlChars.Lf)
        sb.Append("image_height = ""300""" + ControlChars.Lf)
        sb.Append("three_d_look = ""false""" + ControlChars.Lf)
        sb.Append("unique_url = """"" + ControlChars.Lf)
        sb.Append("map_to_overview_connection = """"" + ControlChars.Lf)
        sb.Append("overview_to_map_connection = ""Map""" + ControlChars.Lf)
        sb.Append("map_to_map_connection = """"" + ControlChars.Lf)
        sb.Append("data_display = """"" + ControlChars.Lf)
        sb.Append("/>" + ControlChars.Lf)

        'Dim dc As Made4Net.GeoData.GeoPointTypeCollection = New Made4Net.GeoData.GeoPointTypeCollection(True)
        'Dim d As Made4Net.GeoData.GeoPointType

        'sb.Append("<pointtypes count=""" + dc.Count.ToString() + """>" + ControlChars.Lf)
        'For Each d In dc
        '    sb.Append("<pointtype id=""" + d.MAPPOINTTYPEID + """ name=""" + d.MAPPOINTTYPEID + """ color=""" + d.COLOR_INT.ToString + """ icon=""" + d.ICON + """/>" + ControlChars.Lf)
        'Next
        'sb.Append("</pointtypes>" + ControlChars.Lf)

        sb.Append("</PARAMETERS>" + ControlChars.Lf)
        Return sb.ToString()
    End Function

    '/ <summary>
    '/ Zooms to requested extent
    '/ </summary>
    '/ <param name="xmin">Minimum x-coordinate of requested extent</param>
    '/ <param name="ymin">Minimum y-coordinate of requested extent</param>
    '/ <param name="xmax">Maximum x-coordinate of requested extent</param>
    '/ <param name="ymax">Maximum y-coordinate of requested extent</param>
    '/ <returns>XML string containing image url and new extent</returns>
    Public Function ZoomToExtent() As String
        Dim sb As New System.Text.StringBuilder

        Dim mp As Made4Net.Maps.IMap
        If (m_params.IsRealZoom) Then
            ' this is real zoom - lets see what map we can find 
            mp = Made4Net.Maps.MapManager.GetBestMap(m_params.MinX, m_params.MinY, m_params.MaxX, m_params.MaxY)
        Else
            ' this is panning or something like this - lets keep the current map (if any)
            mp = Made4Net.Maps.MapManager.GetMapByName(m_params.MapName)
            If mp Is Nothing Then
                ' no success with the id - lets get the best map
                mp = Made4Net.Maps.MapManager.GetBestMap(m_params.MinX, m_params.MinY, m_params.MaxX, m_params.MaxY)
            End If
        End If

        If Not mp Is Nothing Then
            Dim bmp As Bitmap = mp.getBitmap(m_params.MinX, m_params.MinY, m_params.MaxX, m_params.MaxY, m_params.Width, m_params.Height)
            If Not bmp Is Nothing Then
                sb.Append(WriteImageNodes2("map", mp, bmp))
            End If
        End If
        Return sb.ToString()
    End Function 'ZoomToExtent

    '/ <summary>
    '/ Create image with Overview parameters
    '/ </summary>
    '/ <returns>XML string containing image url and extent</returns>
    Public Function Overview() As String
        Dim sb As New System.Text.StringBuilder

        Dim mp As Made4Net.Maps.IMap = Made4Net.Maps.MapManager.GetMapByName(m_params.MapName)

        If Not mp Is Nothing Then
            ' set them in the beginning
            m_params.MinX = mp.TopLeftX
            m_params.MinY = mp.TopLeftY
            m_params.MaxX = mp.BottomRightX
            m_params.MaxY = mp.BottomRightY

            ' if required the call will change  the extent
            Dim bmp As Bitmap = mp.getOverviewBitmap(m_params.MinX, m_params.MinY, m_params.MaxX, m_params.MaxY, m_params.Width, m_params.Height)
            If Not bmp Is Nothing Then
                sb.Append(WriteImageNodes2("overview", mp, bmp))
            End If
        End If
        Return sb.ToString()
    End Function 'Overview

    Private Function WriteImageNodes2(ByVal type As String, ByRef mp As Made4Net.Maps.IMap, ByRef bmp As Bitmap) As String
        Dim sb As New System.Text.StringBuilder
        Dim nodename As String = "MAP"
        If type.ToLower().Equals("overview") Then
            nodename = "OVERVIEW"
        End If

        Dim gimg As System.Guid = Guid.NewGuid()

        Session.Add(gimg.ToString(), bmp)
        sb.Append(("<" + nodename + ">" + ControlChars.Lf))

        ' this is just for debugging purposes
        Dim port As String = ""
        If Request.Url.Host.Equals("inray.spnet.net") Then
            port = ":9091"
        End If

        sb.Append(("<IMAGE url=""http://" + Request.Url.Host + port + Request.Url.AbsolutePath.Replace("/map.aspx", "") + "/getmapimg.aspx?guid=" + gimg.ToString() + """ imagescale=""1"" />" + ControlChars.Lf))
        ' we always return the requested size
        'sb.Append("<DEVIATION x=""" + ((mp.TopLeftX - mp.BottomLeftX) + (mp.TopRightX - mp.BottomRightX)).ToString() + """ y=""" + ((mp.TopLeftY - mp.TopRightY) + (mp.BottomLeftY - mp.BottomRightY)).ToString() + """/>" + ControlChars.Lf)
        sb.Append("<DEVIATION x=""0"" y=""0"" />" + ControlChars.Lf)
        sb.Append("<ENVELOPE minx=""" + m_params.MinX.ToString() + """ miny=""" + m_params.MinY.ToString() + """ maxx=""" + m_params.MaxX.ToString() + """ maxy=""" + m_params.MaxY.ToString() + """ />" + ControlChars.Lf)
        sb.Append(("<MAPNAME id = """ + mp.ID + """ />" + ControlChars.Lf))

        Dim col As Made4Net.GeoData.GeoTerritoryCollection = New Made4Net.GeoData.GeoTerritoryCollection
        'Dim tr As MapTerritory = col.GetFromPoint(m_params.MinX + (m_params.MaxX - m_params.MinX) / 2, m_params.MinY + (m_params.MaxY - m_params.MinY) / 2)

        If Not col Is Nothing Then
            'col.ToXml(sb)
        End If

        If Not type.ToLower().Equals("overview") Then
            AppendMapData(sb)
        End If

        sb.Append(("</" + nodename + ">" + ControlChars.Lf))
        Return sb.ToString()
    End Function

    Function AppendPointsData(ByRef sb As System.Text.StringBuilder) As StringBuilder
        Dim ps As Made4Net.GeoData.GeoPointHashtable = m_params.Points
        Dim mnx As Double = m_params.MinX
        Dim mny As Double = m_params.MinY
        Dim mxx As Double = m_params.MaxX
        Dim mxy As Double = m_params.MaxY
        SyncLock ps.SyncRoot
            'If ps.Count > 0 Then
            sb.Append("<points>")
            sb.Append(ControlChars.Lf)

            Dim mp As Made4Net.GeoData.GeoPoint
            For Each mp In ps.Values
                If mp.IsVisible(mnx, mny, mxx, mxy) Then
                    GeoPointToXml(mp, sb)
                End If
            Next

            sb.Append("</points>")
            sb.Append(ControlChars.Lf)
            'End If
        End SyncLock
        Return sb
    End Function

    Function AppendTerritoryData(ByRef sb As System.Text.StringBuilder) As StringBuilder
        Dim ps As Made4Net.GeoData.GeoTerritoryHashtable = Made4Net.GeoData.GeoTerritory.GetTerritories
        Dim mnx As Double = m_params.MinX
        Dim mny As Double = m_params.MinY
        Dim mxx As Double = m_params.MaxX
        Dim mxy As Double = m_params.MaxY
        SyncLock ps.SyncRoot
            'If ps.Count > 0 Then
            sb.Append("<territories>")
            sb.Append(ControlChars.Lf)
            Dim mp As Made4Net.GeoData.GeoTerritory
            For Each mp In ps.Values
                GeoTerritoryToXml(mp, sb)
            Next
            sb.Append("</territories>")
            sb.Append(ControlChars.Lf)
            'End If
        End SyncLock
        Return sb
    End Function

    Function AppendMapData(ByRef sb As System.Text.StringBuilder) As StringBuilder
        Dim mnx As Double = m_params.MinX
        Dim mny As Double = m_params.MinY
        Dim mxx As Double = m_params.MaxX
        Dim mxy As Double = m_params.MaxY

        Dim ps As Made4Net.GeoData.GeoPointHashtable = m_params.Points
        'Dim vh As Made4Net.GeoData.GeoPointHashtable = m_params.VehiclesPoints
        Dim ni As Made4Net.GeoData.GeoNetworkHashtable = m_params.NetItems
        'Dim vr As VehicleRouteHashtable = m_params.Routes
        Dim vr As RouteHashtable = m_params.Routes

        SyncLock ps.SyncRoot
            sb.Append("<points>")
            sb.Append(ControlChars.Lf)
            Dim mp As Made4Net.GeoData.GeoPoint
            For Each mp In ps.Values
                If mp.IsVisible(mnx, mny, mxx, mxy) Then
                    GeoPointToXml(mp, sb)
                End If
            Next
            sb.Append("</points>")
            sb.Append(ControlChars.Lf)
        End SyncLock

        ' now net items
        SyncLock ni.SyncRoot
            sb.Append("<network count=""" + ni.Count.ToString() + """>")
            sb.Append(ControlChars.Lf)
            Dim mn As Made4Net.GeoData.GeoNetworkItem
            For Each mn In ni.Values
                Dim mpf, mpn As Made4Net.GeoData.GeoPoint
                mpf = Made4Net.GeoData.GeoPoint.GetPoint(mn.POINTID)
                mpn = Made4Net.GeoData.GeoPoint.GetPoint(mn.NEXTPOINTID)
                If mpf.IsVisible(mnx, mny, mxx, mxy) Or mpn.IsVisible(mnx, mny, mxx, mxy) Then
                    mn.LON = mpf.LONGITUDE
                    mn.LON_NEXT = mpn.LONGITUDE
                    mn.LAT = mpf.LATITUDE
                    mn.LAT_NEXT = mpn.LATITUDE
                    GeoNetworkToXml(mn, sb)
                End If
            Next
            sb.Append("</network>")
            sb.Append(ControlChars.Lf)
        End SyncLock

        ' ok lets see whether we have routes for sending
        sb.Append("<routes count=""" + vr.Count.ToString() + """>")
        sb.Append(m_params.RoutesXML)
        sb.Append(ControlChars.Lf)
        Dim v As WMS.Logic.Route
        For Each v In vr.Values
            RouteToXml(v, sb)
        Next
        sb.Append("</routes>")
        sb.Append(ControlChars.Lf)
        Try
            Dim trs As Made4Net.GeoData.GeoTerritoryHashtable = Made4Net.GeoData.GeoTerritory.GetTerritories()
            Dim tr As Made4Net.GeoData.GeoTerritory

            SyncLock trs.SyncRoot
                sb.Append("<territories count=""" + trs.Count.ToString() + """>")
                sb.Append(ControlChars.Lf)
                For Each tr In trs.Values
                    GeoTerritoryToXml(tr, sb)
                Next
                sb.Append("</territories>")
                sb.Append(ControlChars.Lf)
            End SyncLock

        Catch ex As Exception

        End Try
        Return sb
    End Function

    'Private Function LoadMapModeData() As String
    '    Dim sb As StringBuilder = New StringBuilder

    '    AddMapModeData(sb)

    '    Return sb.ToString()
    'End Function

    'Private Function AddMapModeData(ByRef sb As StringBuilder)
    '    If m_params.MapMode <> 0 Then

    '        Dim mnx = m_params.MinX
    '        Dim mny = m_params.MinY
    '        Dim mxx = m_params.MaxX
    '        Dim mxy = m_params.MaxY

    '        AppendMapPoints(mnx, mny, mxx, mxy, sb)

    '        If m_params.MapMode = 2 Then
    '            AppendMapNetworkItems(mnx, mny, mxx, mxy, sb)
    '        End If
    '        If m_params.MapMode = 3 Then
    '            '' for the routes - it is not clear what exaclt to display
    '            '' we need an ID of the route here
    '        End If
    '        If m_params.MapMode = 4 Then
    '            AppendMapNetworkItems(mnx, mny, mxx, mxy, sb)
    '        End If
    '    End If
    'End Function

    Private Sub AppendMapNetworkItems(ByVal mnx As Integer, ByVal mny As Integer, ByVal mxx As Integer, ByVal mxy As Integer, Optional ByRef mph As Made4Net.GeoData.GeoPointHashtable = Nothing)
        Dim mpic As Made4Net.GeoData.GeoNetworkHashtable = Made4Net.GeoData.GeoNetworkHashtable.LoadFromCoordinates(mnx, mny, mxx, mxy, mph)

        Dim neti As Made4Net.GeoData.GeoNetworkItem
        Dim ni As Hashtable = m_params.NetItems
        For Each neti In mpic
            ni.Add(neti.POINTID.ToString() + "#" + neti.NEXTPOINTID.ToString(), neti)
        Next
    End Sub

    Private Sub AppendMapPoints(ByRef mnx As Integer, ByRef mny As Integer, ByRef mxx As Integer, ByRef mxy As Integer)
        Dim mpc As Made4Net.GeoData.GeoPointHashtable = New Made4Net.GeoData.GeoPointHashtable
        Made4Net.GeoData.GeoPointHashtable.FillFromCoordinates(mnx, mny, mxx, mxy, 400) ' no more than 200 points
        Dim mp As Made4Net.GeoData.GeoPoint
        Dim ps As Hashtable = m_params.Points
        For Each mp In mpc
            ps.Add(mp.POINTID.ToString(), mp)
        Next
    End Sub

    '/ <summary>
    '/ Inserts responseString into standard response xml stream
    '/ </summary>
    '/ <param name="responseString">XML string containing current response</param>
    '/ <returns>Complete XML response to be sent back to client</returns>
    Private Function WrapAGSResponseNodes(ByVal responseString As String) As String
        Dim sb As New System.Text.StringBuilder
        sb.Append("<?xml version=""1.0"" encoding=""utf-8"" ?> " + ControlChars.Lf)
        sb.Append("<AGS_CONNECTION version=""9.0"">" + ControlChars.Lf)
        sb.Append("<RESPONSE>" + ControlChars.Lf)
        sb.Append(responseString)
        sb.Append(m_params.LastError)
        sb.Append("</RESPONSE>" + ControlChars.Lf)
        sb.Append("</AGS_CONNECTION>" + ControlChars.Lf)
        m_params.LastError = ""
        Return sb.ToString()
    End Function 'WrapAGSResponseNodes

    '/ <summary>
    '/ Writes out error xml node
    '/ To be finished... not currently used
    '/ </summary>
    '/ <param name="errorstring"></param>
    '/ <returns></returns>
    Private Function ErrorMessage(ByVal errorstring As String, ByVal errortype As String, ByVal exception As Exception) As String
        Dim sb As New System.Text.StringBuilder
        sb.Append(("<ERROR type=""" + errortype + """>" + errorstring + "</ERROR>" + ControlChars.Lf))
        If Not exception Is Nothing Then sb.Append("<STACKTRACE>" + exception.StackTrace + "</STACKTRACE>" + ControlChars.Lf)
        Return sb.ToString()
    End Function 'ErrorMessage

#Region "BuildObjectsXML"

    Private Function BuildVehiclePositionXML(ByVal pVehicleId As String, ByVal pLat As String, ByVal pLon As String) As String
        Dim color As String
        Dim sb As StringBuilder = New StringBuilder
        m_params.ApplyNextRouteColor()
        color = MapObjectColor.GetColor(CStr(m_params.RouteColorID)).INT_VALUE
        sb.Append("<route routeid=""" & pVehicleId & """ vehicleid=""" & pVehicleId & """ color=""" & color & """")
        sb.Append(" start=""" & "START" & """ end=""" & "END" & """")
        sb.Append(" comment=""" & "COMMENTS" & """>")
        sb.Append("<routepoints>")
        'add the points and coordinates
        sb.Append("<point id=""" & New Random().Next(1000) & """ latitude=""" & pLat & """ longitude=""" & pLon & """/>")
        sb.Append(ControlChars.Lf)
        sb.Append("</routepoints>")
        sb.Append("</route>")
        sb.Append(ControlChars.Lf)
        Return sb.ToString
    End Function

    Private Function BuildVehicleRouteXML(ByVal pVehicleId As String, ByVal pFromdate As DateTime, ByVal pToDate As DateTime) As String
        Dim sql As String
        Dim color As String
        Dim sb As StringBuilder = New StringBuilder
        Dim dt As New DataTable
        sql = String.Format("select * from vehicleposition where vehicleid = '{0}' and [timestamp] >= CONVERT(DATETIME,{1},102) and " & _
            " [timestamp] <= CONVERT(DATETIME,{2},102) order by [timestamp]", pVehicleId, Made4Net.Shared.Util.FormatField(pFromdate), Made4Net.Shared.Util.FormatField(pToDate))
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            m_params.ApplyNextRouteColor()
            color = MapObjectColor.GetColor(CStr(m_params.RouteColorID)).INT_VALUE
            sb.Append("<route routeid=""" & pVehicleId & """ vehicleid=""" & pVehicleId & """ color=""" & color & """")
            sb.Append(" start=""" & "START" & """ end=""" & "END" & """")
            sb.Append(" comment=""" & "COMMENTS" & """>")
            sb.Append("<routepoints>")
            'add the points and coordinates
            Dim i As Int32 = 0
            For Each dr As DataRow In dt.Rows
                sb.Append("<point id=""" & i & """ latitude=""" & dr("latitude") & """ longitude=""" & dr("longitude") & """/>")
                sb.Append(ControlChars.Lf)
                i += 1
            Next
            sb.Append("</routepoints>")
            sb.Append("</route>")
            sb.Append(ControlChars.Lf)
        End If
        dt = Nothing
        Return sb.ToString
    End Function

    Public Function RouteToXml(ByVal oRoute As WMS.Logic.Route, ByRef sb As System.Text.StringBuilder) As System.Text.StringBuilder
        sb.Append("<route routeid=""" & oRoute.RouteId.TrimStart("R").TrimStart("0").Insert(0, "R") & """ vehicleid=""" & "abcd" & """ color=""" & oRoute.Color & """")
        sb.Append(" start=""" & "START" & """ end=""" & "END" & """")
        sb.Append(" comment=""" & "COMMENTS" & """>")
        sb.Append("<routepoints>")
        ' lets add the points and coordinates
        For Each _routestop As WMS.Logic.RouteStop In oRoute.Stops
            Dim mp As Made4Net.GeoData.GeoPoint = Made4Net.GeoData.GeoPointNode.GetPoints(Convert.ToInt32(_routestop.PointId))
            If Not mp Is Nothing Then
                sb.Append("<point id=""" & mp.POINTID & """ latitude=""" & mp.LATITUDE.ToString() & """ longitude=""" & mp.LONGITUDE.ToString() & """/>")
                sb.Append(ControlChars.Lf)
            End If
        Next
        sb.Append("</routepoints>")
        sb.Append("</route>")
        sb.Append(ControlChars.Lf)
        Return sb
    End Function

    Public Function GeoPointToXml(ByVal oPoint As Made4Net.GeoData.GeoPoint, ByRef sb As StringBuilder) As StringBuilder
        sb.Append("<point id=""" + oPoint.POINTID.ToString() + """")
        sb.Append(" type=""" + oPoint.POINTTYPEID.ToString() + """")
        sb.Append(" comment=""" + oPoint.COMMENTS.ToString() + """")
        sb.Append(" countrycode=""" + oPoint.COUNTRY_CODE.ToString() + """")
        sb.Append(" latitude=""" + oPoint.LATITUDE.ToString() + """ longitude =""" + oPoint.LONGITUDE.ToString() + """")
        sb.Append(" />")
        sb.Append(ControlChars.Lf)
        Return sb
    End Function

    Public Function GeoNetworkToXml(ByVal oGeoNetworkItem As Made4Net.GeoData.GeoNetworkItem, ByRef sb As StringBuilder) As StringBuilder
        sb.Append("<networkitem pointid=""" + oGeoNetworkItem.POINTID.ToString() + """ nextpointid=""" + oGeoNetworkItem.NEXTPOINTID.ToString() + """")
        sb.Append(" drivingtime=""" + oGeoNetworkItem.DRIVINGTIME.ToString() + """ drivingdistance=""" + oGeoNetworkItem.DRIVINGDIST.ToString() + """")
        sb.Append(" roadname=""" + oGeoNetworkItem.ROADNAME.ToString() + """ averagespeed=""" + oGeoNetworkItem.AVERAGESPEED.ToString() + """")
        sb.Append(" rushamfrom=""" + oGeoNetworkItem.RUSHAMFROM.ToString() + """ rushamto=""" + oGeoNetworkItem.RUSHAMTO.ToString() + """")
        sb.Append(" rushpmfrom=""" + oGeoNetworkItem.RUSHPMFROM.ToString() + """ rushpmto=""" + oGeoNetworkItem.RUSHPMTO.ToString() + """")
        sb.Append(" rushfactor=""" + oGeoNetworkItem.RUSH_FACTOR.ToString() + """ toll=""" + oGeoNetworkItem.TOLL.ToString() + """")
        sb.Append(" lon=""" + oGeoNetworkItem.LON.ToString() + """ lat=""" + oGeoNetworkItem.LAT.ToString() + """")
        sb.Append(" lon_next=""" + oGeoNetworkItem.LON_NEXT.ToString() + """ lat_next=""" + oGeoNetworkItem.LAT_NEXT.ToString() + """")
        sb.Append("/>")
        sb.Append(ControlChars.Lf)
        Return sb
    End Function

    Public Function GeoTerritoryToXml(ByVal oGeoTerritory As Made4Net.GeoData.GeoTerritory, ByRef sb As StringBuilder) As StringBuilder
        sb.Append(("<territory id=""" + oGeoTerritory.TERRITORYID.ToString() + """ name=""" + oGeoTerritory.TERRITORYNAME + """>"))
        sb.Append(ControlChars.Lf)
        Dim mp As Made4Net.GeoData.GeoPoint
        For Each mp In oGeoTerritory.BOUNDARY
            GeoPointToXml(mp, sb)
        Next
        sb.Append("</territory>")
        sb.Append(ControlChars.Lf)
        Return sb
    End Function

#End Region

End Class
