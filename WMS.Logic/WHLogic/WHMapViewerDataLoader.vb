Imports System
Imports System.Data
Imports Made4Net.LayoutViewer
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class WHMapViewerDataLoader
    Implements IDataLoader

    Public Sub FillDynamicObjects(ByVal dtDynamicObjects As System.Data.DataTable, ByVal dtDynamicObjectsInfo As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillDynamicObjects
        'Dim SQL As String = String.Format("select * from vWHActivityViewer where datediff(""dd"",activitytime,getdate()) < {0}", Made4Net.Shared.Util.GetSystemParameterValue("WHActivityViewerPeriod"))
        'DataInterface.FillDataset(SQL, dtDynamicObjects)
    End Sub

    Public Sub FillStaticObjects(ByVal dtStaticObjects As System.Data.DataTable, ByVal dtStaticObjectsInfo As System.Data.DataTable, ByVal dtStaticObjectsGroups As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillStaticObjects
        Dim SQL As String = String.Format("select * from vWHMapNodes where warehousearea ='{0}'", Warehouse.CurrentWarehouseArea)
        DataInterface.FillDataset(SQL, dtStaticObjects)
    End Sub

    Public Sub FillStaticObjectsLinks(ByVal dtStaticObjectsLinks As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillStaticObjectsLinks
        Dim SQL As String = String.Format("select * from vWHMapEdges where warehousearea ='{0}'", Warehouse.CurrentWarehouseArea)
        DataInterface.FillDataset(SQL, dtStaticObjectsLinks)
    End Sub

End Class
