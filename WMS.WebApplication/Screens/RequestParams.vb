Imports System
Imports System.Web
Imports System.Xml
Imports System.Xml.XPath
Imports System.IO
Imports System.Text
Imports WMS.Logic

<CLSCompliant(False)> Public Class RequestParams
    Private m_DataFrame As String = Nothing
    Private m_RequestType As String = "startup"
    Private m_x As Double = 0
    Private m_y As Double = 0
    Private m_minx As Double = -180
    Private m_miny As Double = -90
    Private m_maxx As Double = 180
    Private m_maxy As Double = 90
    Private m_select_minx As Double = 0
    Private m_select_miny As Double = 0
    Private m_select_maxx As Double = 0.01
    Private m_select_maxy As Double = 0.01
    Private m_shape As String = "point"
    Private m_click_tolerance As Integer = 3
    Private m_width As Integer = 500
    Private m_height As Integer = 500
    Private m_visible_layers As String = ""
    Private m_start As String = "fullextent"
    Private m_pandirection As String = "north"
    Private m_panpercent As Double = 75
    Private m_zoomfactor As Double = 2
    Private m_showLayerInfo As Boolean = False

    Private m_mapType As String
    Private m_mapName As String
    Private m_realZoom As Boolean

    Private m_sendOV As Boolean = False
    Private m_OVHost As String = "localhost"
    Private m_OVServerObject As String = "world"
    Private m_OVDataFrame As String = [String].Empty
    Private m_ovwidth As Integer = 200
    Private m_ovheight As Integer = 100

    Private m_configURL As String = ""
    Private m_errorString As String = ""

    Private m_imageFormat As String = "PNG"

    Private m_actionParams As String = ""
    Private m_mapMode As Integer = 0

    Private m_points As Made4Net.GeoData.GeoPointHashtable = New Made4Net.GeoData.GeoPointHashtable
    Private m_temppoints As Made4Net.GeoData.GeoPointHashtable = New Made4Net.GeoData.GeoPointHashtable
    Private m_netitems As Made4Net.GeoData.GeoNetworkHashtable = New Made4Net.GeoData.GeoNetworkHashtable
    Private m_routes_xml As String = String.Empty
    Private m_routes As RouteHashtable = New RouteHashtable
    Private m_routecolorid As Integer = -1

    '/ <summary>
    '/ Constructor without parameters
    '/ </summary>
    Public Sub New()
    End Sub 'New

    '/ <summary>
    '/ Constructor with parameter
    '/ </summary>
    '/ <param name="rp">Request parameter collection</param>
    Public Sub New(ByVal rp As System.Collections.Specialized.NameValueCollection)
        GetParams(rp)
    End Sub 'New

    '/ <summary>
    '/ Reads in request parameters and sets property values
    '/ </summary>
    '/ <param name="rp">Request parameter collection</param>
    Public Sub GetParams(ByVal rp As System.Collections.Specialized.NameValueCollection)
        'm_Host = MakeString(rp.Get("host"), m_Host)
        'm_ServerObject = MakeString(rp.Get("serverobject"), m_ServerObject)
        m_DataFrame = MakeString(rp.Get("dataframe"), m_DataFrame)
        m_RequestType = MakeString(rp.Get("requesttype"), m_RequestType).ToLower()
        m_mapType = MakeString(rp.Get("type"), m_RequestType).ToLower()
        m_minx = MakeDouble(rp.Get("minx"), m_minx)
        m_miny = MakeDouble(rp.Get("miny"), m_miny)
        m_maxx = MakeDouble(rp.Get("maxx"), m_maxx)
        m_maxy = MakeDouble(rp.Get("maxy"), m_maxy)
        m_width = MakeInteger(rp.Get("width"), m_width)
        m_height = MakeInteger(rp.Get("height"), m_height)
        m_visible_layers = MakeString(rp.Get("visible"), m_visible_layers)
        m_x = MakeDouble(rp.Get("x"), m_x)
        m_y = MakeDouble(rp.Get("y"), m_y)
        m_select_minx = MakeDouble(rp.Get("selectminx"), m_select_minx)
        m_select_miny = MakeDouble(rp.Get("selectminy"), m_select_miny)
        m_select_maxx = MakeDouble(rp.Get("selectmaxx"), m_select_maxx)
        m_select_maxy = MakeDouble(rp.Get("selectmaxy"), m_select_maxy)
        m_select_minx = MakeDouble(rp.Get("idminx"), m_select_minx)
        m_select_miny = MakeDouble(rp.Get("idminy"), m_select_miny)
        m_select_maxx = MakeDouble(rp.Get("idmaxx"), m_select_maxx)
        m_select_maxy = MakeDouble(rp.Get("idmaxy"), m_select_maxy)
        m_shape = MakeString(rp.Get("shape"), m_shape)
        m_click_tolerance = MakeInteger(rp.Get("tolerance"), m_click_tolerance)
        m_mapMode = MakeInteger(rp.Get("mapMode"), 0)
        m_OVHost = MakeString(rp.Get("ovhost"), m_OVHost)
        m_OVServerObject = MakeString(rp.Get("ovserverobject"), m_OVServerObject)
        m_OVDataFrame = MakeString(rp.Get("ovdataframe"), m_OVDataFrame)
        m_ovwidth = MakeInteger(rp.Get("ovwidth"), m_ovwidth)
        m_ovheight = MakeInteger(rp.Get("ovheight"), m_ovheight)
        m_sendOV = MakeBool(rp.Get("overview"), m_sendOV)
        m_start = MakeString(rp.Get("startextent"), m_start)
        m_pandirection = MakeString(rp.Get("direction"), m_pandirection)
        m_panpercent = MakeDouble(rp.Get("panpercent"), m_panpercent)
        m_zoomfactor = MakeDouble(rp.Get("zoomfactor"), m_zoomfactor)
        m_showLayerInfo = MakeBool(rp.Get("layerinfo"), m_showLayerInfo)

        m_mapName = MakeString(rp.Get("mapid"), m_mapName)

        ' each update action has different params
        m_actionParams = MakeString(rp.Get("params"), m_actionParams)

        Dim s As String = "false"
        s = MakeString(rp.Get("realZoom"), s)
        m_realZoom = s.Equals("true")
    End Sub 'GetParams 

    Public Property Points() As Made4Net.GeoData.GeoPointHashtable
        Get
            Return m_points
        End Get
        Set(ByVal Value As Made4Net.GeoData.GeoPointHashtable)
            m_points = Value
        End Set
    End Property

    Public Property TempPoints() As Made4Net.GeoData.GeoPointHashtable
        Get
            Return m_temppoints
        End Get
        Set(ByVal Value As Made4Net.GeoData.GeoPointHashtable)
            m_temppoints = Value
        End Set
    End Property

    Public Property NetItems() As Made4Net.GeoData.GeoNetworkHashtable
        Get
            Return m_netitems
        End Get
        Set(ByVal Value As Made4Net.GeoData.GeoNetworkHashtable)
            m_netitems = Value
        End Set
    End Property

    Public Property RoutesXML() As String
        Get
            Return m_routes_xml
        End Get
        Set(ByVal Value As String)
            m_routes_xml = Value
        End Set
    End Property

    Public Property Routes() As RouteHashtable
        Get
            Return m_routes
        End Get
        Set(ByVal Value As RouteHashtable)
            m_routes = Value
        End Set
    End Property

    Public Property MapName() As String
        Get
            Return m_mapName
        End Get
        Set(ByVal Value As String)
            m_mapName = Value
        End Set
    End Property
    Public Property MapMode() As Integer
        Get
            Return m_mapMode
        End Get
        Set(ByVal Value As Integer)
            m_mapMode = Value
        End Set
    End Property


    Public Property IsRealZoom() As Boolean
        Get
            Return m_realZoom
        End Get
        Set(ByVal Value As Boolean)
            m_realZoom = Value
        End Set
    End Property

    Public Property ActionParams() As String
        Get
            Return m_actionParams
        End Get
        Set(ByVal Value As String)
            m_actionParams = Value
        End Set
    End Property


    Public Property DataFrame() As String
        Get
            Return m_DataFrame
        End Get
        Set(ByVal Value As String)
            m_DataFrame = Value
        End Set
    End Property

    Public Property MapType() As String
        Get
            Return m_mapType
        End Get
        Set(ByVal Value As String)
            m_mapType = Value
        End Set
    End Property


    Public Property RequestType() As String
        Get
            Return m_RequestType
        End Get
        Set(ByVal Value As String)
            m_RequestType = Value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return m_width
        End Get
        Set(ByVal Value As Integer)
            m_width = Value
        End Set
    End Property

    Public Property Height() As Integer
        Get
            Return m_height
        End Get
        Set(ByVal Value As Integer)
            m_height = Value
        End Set
    End Property

    Public Property MinX() As Double
        Get
            Return m_minx
        End Get
        Set(ByVal Value As Double)
            m_minx = Value
        End Set
    End Property

    Public Property MinY() As Double
        Get
            Return m_miny
        End Get
        Set(ByVal Value As Double)
            m_miny = Value
        End Set
    End Property

    Public Property MaxX() As Double
        Get
            Return m_maxx
        End Get
        Set(ByVal Value As Double)
            m_maxx = Value
        End Set
    End Property

    Public Property MaxY() As Double
        Get
            Return m_maxy
        End Get
        Set(ByVal Value As Double)
            m_maxy = Value
        End Set
    End Property

    Public Property X() As Double
        Get
            Return m_x
        End Get
        Set(ByVal Value As Double)
            m_x = Value
        End Set
    End Property

    Public Property Y() As Double
        Get
            Return m_y
        End Get
        Set(ByVal Value As Double)
            m_y = Value
        End Set
    End Property

    Public Property SelectMinX() As Double
        Get
            Return m_select_minx
        End Get
        Set(ByVal Value As Double)
            m_select_minx = Value
        End Set
    End Property

    Public Property SelectMinY() As Double
        Get
            Return m_select_miny
        End Get
        Set(ByVal Value As Double)
            m_select_miny = Value
        End Set
    End Property

    Public Property SelectMaxX() As Double
        Get
            Return m_select_maxx
        End Get
        Set(ByVal Value As Double)
            m_select_maxx = Value
        End Set
    End Property

    Public Property SelectMaxY() As Double
        Get
            Return m_select_maxy
        End Get
        Set(ByVal Value As Double)
            m_select_maxy = Value
        End Set
    End Property

    Public Property Tolerance() As Integer
        Get
            Return m_click_tolerance
        End Get
        Set(ByVal Value As Integer)
            m_click_tolerance = Value
        End Set
    End Property

    Public Property SelectShape() As String
        Get
            Return m_shape
        End Get
        Set(ByVal Value As String)
            m_shape = Value
        End Set
    End Property

    Public Property ImageFormat() As String
        Get
            Return m_imageFormat
        End Get
        Set(ByVal Value As String)
            m_imageFormat = Value
        End Set
    End Property

    Public Property VisibleLayerList() As String
        Get
            Return m_visible_layers
        End Get
        Set(ByVal Value As String)
            m_visible_layers = Value
        End Set
    End Property

    Public Property StartExtent() As String
        Get
            Return m_start
        End Get
        Set(ByVal Value As String)
            m_start = Value
        End Set
    End Property


    Public Property OVHost() As String
        Get
            Return m_OVHost
        End Get
        Set(ByVal Value As String)
            m_OVHost = Value
        End Set
    End Property


    Public Property OVServerObject() As String
        Get
            Return m_OVServerObject
        End Get
        Set(ByVal Value As String)
            m_OVServerObject = Value
        End Set
    End Property

    Public Property OVDataFrame() As String
        Get
            Return m_OVDataFrame
        End Get
        Set(ByVal Value As String)
            m_OVDataFrame = Value
        End Set
    End Property

    Public Property OVWidth() As Integer
        Get
            Return m_ovwidth
        End Get
        Set(ByVal Value As Integer)
            m_ovwidth = Value
        End Set
    End Property

    Public Property OVHeight() As Integer
        Get
            Return m_ovheight
        End Get
        Set(ByVal Value As Integer)
            m_ovheight = Value
        End Set
    End Property

    Public Property SendOV() As Boolean
        Get
            Return m_sendOV
        End Get
        Set(ByVal Value As Boolean)
            m_sendOV = Value
        End Set
    End Property

    Public Property ConfigUrl() As String
        Get
            Return m_configURL
        End Get
        Set(ByVal Value As String)
            m_configURL = Value
        End Set
    End Property

    Public Property LastError() As String
        Get
            Return m_errorString
        End Get
        Set(ByVal Value As String)
            m_errorString = Value
        End Set
    End Property

    Public Property PanDirection() As String
        Get
            Return m_pandirection
        End Get
        Set(ByVal Value As String)
            m_pandirection = Value
        End Set
    End Property

    Public Property PanPercent() As Double
        Get
            Return m_panpercent
        End Get
        Set(ByVal Value As Double)
            m_panpercent = Value
        End Set
    End Property

    Public Property ZoomFactor() As Double
        Get
            Return m_zoomfactor
        End Get
        Set(ByVal Value As Double)
            m_zoomfactor = Value
        End Set
    End Property

    Public Property ShowLayerInfo() As Boolean
        Get
            Return m_showLayerInfo
        End Get
        Set(ByVal Value As Boolean)
            m_showLayerInfo = Value
        End Set
    End Property
    Public ReadOnly Property RouteColorID() As Integer
        Get
            Return m_routecolorid
        End Get

    End Property

    '/ <summary>
    '/ Sets an integer value from a string. Used to make sure a member has a valid integer value when reading in request parameters.
    '/ </summary>
    '/ <param name="valueString">String to be converted.</param>
    '/ <param name="defaultValue">Default value to be used.</param>
    '/ <returns>Integer value</returns>
    Private Function MakeInteger(ByVal valueString As String, ByVal defaultValue As Integer) As Integer
        Dim returnval As Integer = defaultValue
        If valueString Is Nothing Then
            valueString = [String].Empty
        End If
        If Not valueString.Equals([String].Empty) Then
            returnval = Integer.Parse(valueString)
        End If
        Return returnval
    End Function 'MakeInteger


    '/ <summary>
    '/ Sets a double value from a string. Used to make sure a member has a valid double value when reading in request parameters.
    '/ </summary>
    '/ <param name="valueString">String to be converted.</param>
    '/ <param name="defaultValue">Default value to be used.</param>
    '/ <returns>Double value</returns>
    Private Function MakeDouble(ByVal valueString As String, ByVal defaultValue As Double) As Double
        Dim returnval As Double = defaultValue
        If valueString Is Nothing Then
            valueString = [String].Empty
        End If
        If Not valueString.Equals([String].Empty) Then
            valueString.Replace(",", ".") ' in case someday Macromedia decides to handle properly the locales :)
            returnval = Convert.ToDouble(valueString, System.Globalization.CultureInfo.InvariantCulture)
        End If
        Return returnval
    End Function 'MakeDouble


    '/ <summary>
    '/ Sets a string value. Used to make sure a member has a valid string value when reading in request parameters.
    '/ </summary>
    '/ <param name="valueString">String to be converted.</param>
    '/ <param name="defaultValue">Default value to be used.</param>
    '/ <returns>String value</returns>
    Private Function MakeString(ByVal valueString As String, ByVal defaultValue As String) As String
        Dim returnval As String = defaultValue
        If valueString Is Nothing Then
            valueString = [String].Empty
        End If
        If Not valueString.Equals([String].Empty) Then
            returnval = valueString
        End If
        Return returnval
    End Function 'MakeString


    '/ <summary>
    '/ Sets a boolean value. Used to make sure a member has a valid boolean value when reading in request parameters.
    '/ </summary>
    '/ <param name="valueString">String to be converted.</param>
    '/ <param name="defaultValue">Default value to be used.</param>
    '/ <returns>Boolean value</returns>
    Private Function MakeBool(ByVal valueString As String, ByVal defaultValue As Boolean) As Boolean
        Dim returnval As Boolean = defaultValue
        If valueString Is Nothing Then
            valueString = [String].Empty
        End If
        If Not valueString.Equals([String].Empty) Then
            returnval = Boolean.Parse(valueString)
        End If
        Return returnval
    End Function 'MakeBool

    Public Sub ApplyNextRouteColor()
        If Me.m_routecolorid >= MapObjectColor.GetColors.Count Then
            Me.m_routecolorid = 0
        Else
            Me.m_routecolorid = Me.m_routecolorid + 1
        End If
    End Sub

End Class
