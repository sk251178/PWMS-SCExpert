Imports System
Imports System.Data
Imports Made4Net.LayoutViewer
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class WHActivityViewerDataLoader
    Implements IDataLoader

    Public Sub FillDynamicObjects(ByVal dtDynamicObjects As System.Data.DataTable, ByVal dtDynamicObjectsInfo As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillDynamicObjects
        Dim SQL As String = String.Format("select * from vWHActivityViewer where datediff(""dd"",activitytime,getdate()) < {0} and warehousearea='{1}'", Made4Net.Shared.Util.GetSystemParameterValue("WHActivityViewerPeriod"), Warehouse.CurrentWarehouseArea)
        DataInterface.FillDataset(SQL, dtDynamicObjects)
    End Sub

    Public Sub FillStaticObjects(ByVal dtStaticObjects As System.Data.DataTable, ByVal dtStaticObjectsInfo As System.Data.DataTable, ByVal dtStaticObjectsGroups As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillStaticObjects
        'Dim SQL As String = String.Format("select * from vWHLocationsViewer")
        'DataInterface.FillDataset(SQL, dtStaticObjects)
    End Sub

    Public Sub FillStaticObjectsLinks(ByVal dtStaticObjectsLinks As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillStaticObjectsLinks
        'Do Nothing
    End Sub

End Class
