Imports Made4Net.Schema
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Web

'Commented for RWMS-1185 Start
'Imports NCR.GEMS.Core.DataCaching
'Imports NCR.GEMS.WMS.Core.WmsQueryCaching
'Commented for RWMS-1185 End

<CLSCompliant(False)> Public Class Warehouse
    Inherits Made4Net.Shared.Warehouse
#Region "Variables"

    'Protected Shared _whid As String
    'Protected Shared _whname As String
    'Protected Shared _whcon As String
    <ThreadStatic()> Protected Shared _UserSelectedWHArea As String
    <ThreadStatic()> Protected Shared _UserSelectedWHAreaDesc As String
    <ThreadStatic()> Protected Shared _WHLayoutBackgroundImage As String
    Protected Shared _YardLayoutBackgroundImage As String
    'Protected Shared _tz As Made4NetTimeZone

    'Warehouse area parameters
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutBackgroundImage As String
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutXRatio As Decimal
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutYRatio As Decimal
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutXOffset As Decimal
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutYOffset As Decimal
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutDataMAxX As Decimal
    <ThreadStatic()> Protected Shared _CurrentWHAreaLayoutDataMAxY As Decimal

#End Region

#Region "Properties"

    'Public Shared ReadOnly Property CurrentWarehouse() As String
    '    Get
    '        If IsHttpContext() Then
    '            Return HttpContext.Current.Session("Warehouse_CurrentWarehouseId")
    '        Else
    '            Return _whid
    '        End If
    '    End Get
    'End Property

    'Public Shared ReadOnly Property WarehouseName() As String
    '    Get
    '        If IsHttpContext() Then
    '            Return HttpContext.Current.Session("Warehouse_CurrentWarehouseName")
    '        Else
    '            Return _whname
    '        End If
    '    End Get
    'End Property

    'Public Shared ReadOnly Property WarehouseConnection() As String
    '    Get
    '        If IsHttpContext() Then
    '            Return HttpContext.Current.Session("Warehouse_CurrentWarehouseConnectionName")
    '        Else
    '            Return _whcon
    '        End If
    '    End Get
    'End Property

    Public Shared ReadOnly Property WarehouseLayoutBackgroundImage() As String
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionLayoutBackImage")
            End If
        End Get
    End Property

    'Public Shared ReadOnly Property YardLayoutBackgroundImage() As String
    '    Get
    '        If IsHttpContext() Then
    '            Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionYardLayoutBackImage")
    '        End If
    '    End Get
    'End Property

    Public Shared ReadOnly Property CurrentWarehouseArea() As String
        Get
            Return getUserWarehouseArea()
        End Get
    End Property

    Public Shared ReadOnly Property CurrentWarehouseAreaLayoutBackgroundImage() As String
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseAreaConnectionLayoutBackImage")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CurrentWHAreaLayoutXRatio() As Decimal
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutXRatio")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CurrentWHAreaLayoutYRatio() As Decimal
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutYRatio")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CurrentWHAreaLayoutXOffset() As Decimal
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutXOffset")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property CurrentWHAreaLayoutYOffset() As Decimal
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutYOffset")
            End If
        End Get
    End Property

    Public Shared Property CurrentWHAreaLayoutDataMAxX() As Decimal
        Get
            Return _CurrentWHAreaLayoutDataMAxX
        End Get
        Set(ByVal value As Decimal)
            _CurrentWHAreaLayoutDataMAxX = value
        End Set
    End Property

    Public Shared Property CurrentWHAreaLayoutDataMAxY() As Decimal
        Get
            Return _CurrentWHAreaLayoutDataMAxY
        End Get
        Set(ByVal value As Decimal)
            _CurrentWHAreaLayoutDataMAxY = value
        End Set
    End Property

    Public Shared ReadOnly Property YardLayoutBackgroundImage() As String
        Get
            If IsHttpContext() Then
                Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionYardLayoutBackImage")
            End If
        End Get
    End Property

    'Public Shared ReadOnly Property WarehouseTimeZone() As Made4NetTimeZone
    '    Get
    '        'If IsHttpContext() Then
    '        '    Return Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseTimeZone")
    '        'Else
    '        '    Return _tz
    '        'End If
    '        Return Made4Net.Shared.Warehouse.WarehouseTimeZone
    '    End Get
    'End Property

#End Region

#Region "Methods"

    'Commented for RWMS-568 Start
    'Public Shared Sub setCurrentWarehouse(ByVal WarehouseId As String)

    '    Dim sql As String = String.Format("Select * from warehouse where WAREHOUSEID='{0}'", WarehouseId)
    '    Dim dataRow As DataRow
    '    Dim dataTable As DataTable

    '    If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.Warehouse, WarehouseId) Then
    '        dataTable = New DataTable()
    '        DataInterface.FillDataset(sql, dataTable, False, Made4Net.Schema.CONNECTION_NAME)
    '    Else
    '        dataTable = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.Warehouse, WarehouseId)
    '        If IsNothing(dataTable) Then
    '            dataTable = New DataTable()
    '            DataInterface.FillDataset(sql, dataTable, False, Made4Net.Schema.CONNECTION_NAME)
    '            GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.Warehouse, WarehouseId, dataTable, sql)
    '        End If
    '    End If

    '    If dataTable.Rows.Count > 0 Then
    '        dataRow = dataTable.Rows(0)
    '        Made4Net.Shared.Warehouse.setCurrentWarehouse(dataRow)
    '        If IsHttpContext() Then
    '            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionLayoutBackImage") = dataRow("WAREHOUSELAYOUTBACKGROUNDIMAGE")
    '            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionYardLayoutBackImage") = dataRow("YARDLAYOUTBACKGROUNDIMAGE")
    '        End If
    '    End If
    'End Sub
    'Commented for RWMS-568 End
    Public Shared Shadows Sub setCurrentWarehouse(ByVal WarehouseId As String)
        Dim sql As String = String.Format("Select * from warehouse where WAREHOUSEID='{0}'", WarehouseId)
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.CONNECTION_NAME)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            Made4Net.Shared.Warehouse.setCurrentWarehouse(dr)
            If IsHttpContext() Then
                'HttpContext.Current.Session("Warehouse_CurrentWarehouseId") = dr("WAREHOUSEID")
                'HttpContext.Current.Session("Warehouse_CurrentWarehouseName") = dr("WAREHOUSENAME")
                'HttpContext.Current.Session("Warehouse_CurrentWarehouseConnectionName") = dr("WAREHOUSECONNECTION")

                'Commented for PWMS-931 Start
                'Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionLayoutBackImage") = dr("WAREHOUSELAYOUTBACKGROUNDIMAGE")
                'Commented for PWMS-931 End

                'Added for PWMS-931 Start
                Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionLayoutBackImage") = GetInstancePath() + "\Workstation\Images\" + dr("WAREHOUSELAYOUTBACKGROUNDIMAGE")
                'Added for PWMS-931 End

                Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseConnectionYardLayoutBackImage") = dr("YARDLAYOUTBACKGROUNDIMAGE")
            Else
                '_whname = dr("WAREHOUSENAME")
                '_whid = dr("WAREHOUSEID")
                '_whcon = dr("WAREHOUSECONNECTION")
            End If
        End If
    End Sub

    Public Shared Sub setUserWarehouseArea(ByVal pWHArea As String)
        'Set the warehouse area background image and map ratios
        Dim sql As String = String.Format("SELECT ISNULL(WAREHOUSEAREADESCRIPTION,WAREHOUSEAREACODE) as WAREHOUSEAREADESCRIPTION,ISNULL(WAREHOUSEAREALAYOUTBACKGROUNDIMAGE, '') AS WAREHOUSEAREALAYOUTBACKGROUNDIMAGE, isnull(WAREHOUSEAREALAYOUTXOFFSET, 0) as WAREHOUSEAREALAYOUTXOFFSET, isnull(WAREHOUSEAREALAYOUTYOFFSET, 0) as WAREHOUSEAREALAYOUTYOFFSET, " & _
            " ISNULL(WAREHOUSEAREALAYOUTXRATIO, 0) AS WAREHOUSEAREALAYOUTXRATIO, ISNULL(WAREHOUSEAREALAYOUTYRATIO, 0) AS WAREHOUSEAREALAYOUTYRATIO" & _
            " FROM WAREHOUSEAREA where WAREHOUSEAREACODE = '{0}'", pWHArea)

        Dim dataTable As New DataTable

        'Commented for RWMS-1185 Start

        'If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.WarehouseArea, pWHArea) Then
        '    dataTable = New DataTable()
        '    DataInterface.FillDataset(sql, dataTable)
        'Else
        '    dataTable = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.WarehouseArea, pWHArea)
        '    If IsNothing(dataTable) Then
        '        dataTable = New DataTable()
        '        DataInterface.FillDataset(sql, dataTable)
        '        GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.WarehouseArea, pWHArea, dataTable, sql)
        '    End If
        'End If

        'Commented for RWMS-1185 Start
        'Added for RWMS-1185 Start
        DataInterface.FillDataset(sql, dataTable)
        'Added for RWMS-1185 Start

        If dataTable.Rows.Count = 1 Then
            Dim dataRow As DataRow = dataTable.Rows(0)
            'Commented for PWMS-931 Start
            ' _CurrentWHAreaLayoutBackgroundImage = dataRow("WAREHOUSEAREALAYOUTBACKGROUNDIMAGE")
            'Commented for PWMS-931 End

            'Added for PWMS-931 Start
            If Not dataRow("WAREHOUSEAREALAYOUTBACKGROUNDIMAGE") = "" Then
                _CurrentWHAreaLayoutBackgroundImage = GetInstancePath() + "\Workstation\Images\" + dataRow("WAREHOUSEAREALAYOUTBACKGROUNDIMAGE")
            End If
            'Added for PWMS-931 End
            _CurrentWHAreaLayoutXRatio = dataRow("WAREHOUSEAREALAYOUTXRATIO")
            _CurrentWHAreaLayoutYRatio = dataRow("WAREHOUSEAREALAYOUTYRATIO")
            _CurrentWHAreaLayoutXOffset = dataRow("WAREHOUSEAREALAYOUTXOFFSET")
            _CurrentWHAreaLayoutYOffset = dataRow("WAREHOUSEAREALAYOUTYOFFSET")
            _UserSelectedWHAreaDesc = dataRow("WAREHOUSEAREADESCRIPTION")
        End If

        'Set session with params
        If IsHttpContext() Then
            Made4Net.Shared.ContextSwitch.Current.Session("UserSelectedWHArea") = pWHArea
            Made4Net.Shared.ContextSwitch.Current.Session("UserSelectedWHAreaDescription") = _UserSelectedWHAreaDesc
            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWarehouseAreaConnectionLayoutBackImage") = _CurrentWHAreaLayoutBackgroundImage
            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutXRatio") = _CurrentWHAreaLayoutXRatio
            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutYRatio") = _CurrentWHAreaLayoutYRatio
            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutXOffset") = _CurrentWHAreaLayoutXOffset
            Made4Net.Shared.ContextSwitch.Current.Session("Warehouse_CurrentWHAreaLayoutYOffset") = _CurrentWHAreaLayoutYOffset
        Else
            _UserSelectedWHArea = pWHArea
        End If
    End Sub

    Public Shared Function getUserWarehouseArea() As String
        If IsHttpContext() Then
            Return Made4Net.Shared.ContextSwitch.Current.Session("UserSelectedWHArea")
        Else
            Return _UserSelectedWHArea
        End If
    End Function

    Public Shared Function getUserWarehouseAreaDescription() As String
        If IsHttpContext() Then
            Return Made4Net.Shared.ContextSwitch.Current.Session("UserSelectedWHAreaDescription")
        Else
            Return _UserSelectedWHAreaDesc
        End If
    End Function

    'Protected Shared Function IsHttpContext() As Boolean
    '    If HttpContext.Current Is Nothing Then
    '        Return False
    '    Else
    '        If HttpContext.Current.Session Is Nothing Then
    '            Return False
    '        End If
    '    End If
    '    Return True
    'End Function

    Public Shared Function ValidateWareHouseArea(ByVal pWarehouseArea As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(*) FROM WAREHOUSEAREA WHERE WAREHOUSEAREACODE='{0}'", pWarehouseArea)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

#End Region

End Class