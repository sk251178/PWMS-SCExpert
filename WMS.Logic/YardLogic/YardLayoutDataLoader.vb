Imports System
Imports System.Data
Imports Made4Net.LayoutViewer
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class YardLayoutDataLoader
    Implements IDataLoader

    Public Sub FillDynamicObjects(ByVal dtDynamicObjects As System.Data.DataTable, ByVal dtDynamicObjectsInfo As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillDynamicObjects
        Dim SQL As String = String.Format("select * from vYardVehiclesPosition")
        DataInterface.FillDataset(SQL, dtDynamicObjects, False, Nothing)
    End Sub

    Public Sub FillStaticObjects(ByVal dtStaticObjects As System.Data.DataTable, ByVal dtStaticObjectsInfo As System.Data.DataTable, ByVal dtStaticObjectsGroups As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillStaticObjects
        LoadYardLocations(dtStaticObjects)
    End Sub

    Private Sub LoadYardLocations(ByRef dtLocations As DataTable)
        Dim SQL As String = String.Format("select * from vYardLocations")
        DataInterface.FillDataset(SQL, dtLocations, False, Nothing)
    End Sub

    Public Sub FillStaticObjectsLinks(ByVal dtStaticObjectsLinks As System.Data.DataTable) Implements Made4Net.LayoutViewer.IDataLoader.FillStaticObjectsLinks
        'Do Nothing
    End Sub
End Class
