Imports Made4Net.Shared
Imports Made4Net.Scheduler

Partial Public Class OrderAutomationShipmentAssign
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'FillControl()
    End Sub

    Private Sub TESHIPASSIGNPOLICY_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TESHIPASSIGNPOLICY.RecordSelected
        Dim tds As DataTable = TESHIPASSIGNPOLICY.CreateDataTableForSelectedRecord()
        Session("TEMPLATE") = tds.Rows(0)("TEMPLATENAME")

        FillControl()
    End Sub


    Private Sub FillControl()
        SheduleTrigger.ApplicationConnection = Warehouse.WarehouseConnection
        SheduleTrigger.ApplicationToRun = "OrdersAutomationShipmentAssignment"
        SheduleTrigger.ScheduleID = "OrdersAutomationShipmentAssignment_" & Session("TEMPLATE")
        Dim sap As SchedulerApplicationParam = New SchedulerApplicationParam
        sap.Argument = Session("TEMPLATE")
        sap.ArgumentOrdinal = 1
        sap.ArgumentSystemType = "System.String"
        Dim arr As New ArrayList
        arr.Add(sap)

        SheduleTrigger.SetApplicationParams(arr)
        SheduleTrigger.ReLoad()
        'SheduleTrigger.refresh()

    End Sub
End Class