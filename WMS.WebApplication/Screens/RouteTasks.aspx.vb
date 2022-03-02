Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert
Imports WMS.Lib

Partial Public Class RouteTasks
    Inherits System.Web.UI.Page

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "AssignPackages"
                Dim routeid As String = Session("CurrentRouteId")
                Dim oRoute As New WMS.Logic.Route(routeid)
                For Each dr In ds.Tables(0).Rows
                    Dim added As Boolean = False
                    Dim docId, cons As String
                    docId = dr("documentid")
                    cons = dr("consignee")
                    For Each oStop As WMS.Logic.RouteStop In oRoute.Stops
                        For Each oStopTask As WMS.Logic.RouteStopTask In oStop.RouteStopTask
                            If oStopTask.Consignee = cons And oStopTask.DocumentId = docId Then
                                oStopTask.AddStopTaskPackage(dr("packageid"), UserId)
                                added = True
                            End If
                        Next
                    Next
                    If Not added Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot add package to route", "Cannot add package to route - stop task mismatch")
                    End If
                Next
            Case "UnAssignPackages"
                For Each dr In ds.Tables(0).Rows
                    Dim oRouteStopTask As New WMS.Logic.RouteStopTask(Session("CurrentRouteId"), Convert.ToInt32(dr("stopnumber")), Convert.ToInt32(dr("stoptaskid")))
                    oRouteStopTask.RemoveStopTaskPackage(dr("packageid"), UserId)
                Next
            Case "DeliverPackage"
                For Each dr In ds.Tables(0).Rows
                    Dim oRouteStopTask As New WMS.Logic.RouteStopTask(Session("CurrentRouteId"), Convert.ToInt32(dr("stopnumber")), Convert.ToInt32(dr("stoptaskid")))
                    oRouteStopTask.DeliverPackage(dr("packageid"), UserId)
                Next
            Case "AssignTasks"
                Dim routeid As String = Session("CurrentRouteId")
                Dim oRoute As New WMS.Logic.Route(routeid)
                For Each dr In ds.Tables(0).Rows
                    oRoute.AddStopDetail(WMS.Logic.RouteStopTask.StopTaskTypeFromString(dr("stoptasktype")), ReplaceDBNull(dr("scheduledate")), ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("documentid")), ReplaceDBNull(dr("documenttype")), _
                        ReplaceDBNull(dr("company")), ReplaceDBNull(dr("companytype")), ReplaceDBNull(dr("contactid")), "", "", 0, "", 0, 0, 0, "", StopTaskConfirmationType.None, UserId)
                Next
            Case "UnAssignTasks"
                Dim routeid As String = Session("CurrentRouteId")
                Dim oRoute As New WMS.Logic.Route(routeid)
                For Each dr In ds.Tables(0).Rows
                    oRoute.RemoveStopTask(dr("stopnumber"), dr("stoptaskid"), UserId)
                Next
            Case "CompleteTask"
                For Each dr In ds.Tables(0).Rows
                    Dim oRouteStopTask As New WMS.Logic.RouteStopTask(Session("CurrentRouteId"), Convert.ToInt32(dr("stopnumber")), Convert.ToInt32(dr("stoptaskid")))
                    oRouteStopTask.Complete(UserId, True)
                Next
            Case "CancelTask"
                For Each dr In ds.Tables(0).Rows
                    Dim oRouteStopTask As New WMS.Logic.RouteStopTask(Session("CurrentRouteId"), Convert.ToInt32(dr("stopnumber")), Convert.ToInt32(dr("stoptaskid")))
                    oRouteStopTask.Cancel(UserId)
                Next
            Case "IncompleteTask"
                For Each dr In ds.Tables(0).Rows
                    Dim oRouteStopTask As New WMS.Logic.RouteStopTask(Session("CurrentRouteId"), Convert.ToInt32(dr("stopnumber")), Convert.ToInt32(dr("stoptaskid")))
                    oRouteStopTask.InComplete(dr("reasoncode"), UserId)
                Next
            Case "LoadRoute"
                For Each dr In ds.Tables(0).Rows
                    Dim oRoute As New WMS.Logic.Route(dr("routeid"))
                    oRoute.Load(UserId, DateTime.Now)
                Next
            Case "DepartRoute"
                For Each dr In ds.Tables(0).Rows
                    Dim oRoute As New WMS.Logic.Route(dr("routeid"))
                    oRoute.Depart(UserId, DateTime.Now)
                Next
            Case "ReturnRoute"
                For Each dr In ds.Tables(0).Rows
                    Dim oRoute As New WMS.Logic.Route(dr("routeid"))
                    oRoute.ReturnRoute(UserId, DateTime.Now)
                Next
            Case "CloseRoute"
                For Each dr In ds.Tables(0).Rows
                    Dim oRoute As New WMS.Logic.Route(dr("routeid"))
                    oRoute.CloseRoute(UserId, DateTime.Now)
                Next
            Case "PrintWS"
                For Each rt As DataRow In ds.Tables(0).Rows

                Next
            Case "SetDriverVehicle"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                    If oRoute.Status < WMS.Logic.RouteStatus.Departed Then
                        oRoute.Driver = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("DRIVER"))
                        oRoute.Vehicle = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("VEHICLEID"))
                        oRoute.VehicleType = DataInterface.ExecuteScalar("SELECT VEHICLETYPENAME FROM VEHICLE WHERE VEHICLEID='" & Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("VEHICLEID")) & "'")
                        oRoute.Save(UserId)
                    Else
                        Throw New Made4Net.Shared.M4NException(New Exception, "Route Status incorrect", "Route Status incorrect")
                    End If
                Next
            Case "addpackage"
                dr = ds.Tables(0).Rows(0)
                Dim oPackage As New WMS.Logic.RoutePackage
                oPackage.Create(ReplaceDBNull(dr("packageid")), ReplaceDBNull(dr("packagetype")), ReplaceDBNull(dr("documenttype")), ReplaceDBNull(dr("documentid")), ReplaceDBNull(dr("consignee")), UserId)
            Case "editpackage"
                dr = ds.Tables(0).Rows(0)
                Dim oPackage As New WMS.Logic.RoutePackage(ReplaceDBNull(dr("packageid")))
                oPackage.Update(ReplaceDBNull(dr("packagetype")), ReplaceDBNull(dr("documenttype")), ReplaceDBNull(dr("documentid")), ReplaceDBNull(dr("consignee")), oPackage.Status, UserId)
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session.Remove("CurrentRouteId")
        End If
    End Sub

    Private Sub TEAssignPackages_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignPackages.CreatedChildControls
        TEAssignPackages.ActionBar.AddSpacer()
        TEAssignPackages.ActionBar.AddExecButton("AssignPackages", "Assign Packages To Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TEAssignPackages.ActionBar.Button("AssignPackages")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
    End Sub

    Private Sub TEPackages_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPackages.CreatedChildControls
        TEPackages.ActionBar.AddSpacer()
        TEPackages.ActionBar.AddExecButton("UnAssignPackages", "Un-Assign Packages From Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
        With TEPackages.ActionBar.Button("UnAssignPackages")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
        TEPackages.ActionBar.AddSpacer()
        TEPackages.ActionBar.AddExecButton("DeliverPackage", "Deliver Packages", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TEPackages.ActionBar.Button("DeliverPackage")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
    End Sub

    Private Sub TERoutes_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutes.CreatedChildControls
        TERoutes.ActionBar.AddExecButton("SetDriverVehicle", "Set Driver&Vehicle", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TERoutes.ActionBar.Button("SetDriverVehicle")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
            .CommandName = "SetDriverVehicle"
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        End With
        TERoutes.ActionBar.AddSpacer()
        TERoutes.ActionBar.AddExecButton("LoadRoute", "Route Loaded", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TERoutes.ActionBar.Button("LoadRoute")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
            .ConfirmRequired = True
            .ConfirmMessage = "Are you sure you want to load the route?"
        End With
        'TERoutes.ActionBar.AddSpacer()
        TERoutes.ActionBar.AddExecButton("DepartRoute", "Route Departed", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
        With TERoutes.ActionBar.Button("DepartRoute")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
            .ConfirmRequired = True
            .ConfirmMessage = "Are you sure you want to depart the route?"
        End With
        'TERoutes.ActionBar.AddSpacer()
        TERoutes.ActionBar.AddExecButton("ReturnRoute", "Return Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
        With TERoutes.ActionBar.Button("ReturnRoute")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
            .ConfirmRequired = True
            .ConfirmMessage = "Are you sure you want to return the route?"
        End With
        TERoutes.ActionBar.AddExecButton("CloseRoute", "Close Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
        With TERoutes.ActionBar.Button("CloseRoute")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
            .ConfirmRequired = True
            .ConfirmMessage = "Are you sure you want to close the route?"
        End With
        TERoutes.ActionBar.AddSpacer()
        TERoutes.ActionBar.AddExecButton("PrintWS", "Print Drivers Delivery Sheet", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
        With TERoutes.ActionBar.Button("PrintWS")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
    End Sub

    Private Sub TERoutes_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TERoutes.RecordSelected
        Dim tds As DataTable = TERoutes.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Add("ROUTEID", tds.Rows(0)("ROUTEID"))
        Session("CurrentRouteId") = tds.Rows(0)("ROUTEID")
        TEAssignTasks.PreDefinedValues = vals
        TEAssignPackages.PreDefinedValues = vals
        TS_SelectedIndexChange(Nothing, Nothing)
    End Sub

    Private Sub TS_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TS.SelectedIndexChange

    End Sub

    Private Sub TEAssignTasks_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignTasks.CreatedChildControls
        TEAssignTasks.ActionBar.AddSpacer()
        TEAssignTasks.ActionBar.AddExecButton("AssignTasks", "Assign Tasks To Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TEAssignTasks.ActionBar.Button("AssignTasks")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
    End Sub

    Private Sub TERouteStopTasks_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERouteStopTasks.CreatedChildControls
        TERouteStopTasks.ActionBar.AddSpacer()
        TERouteStopTasks.ActionBar.AddExecButton("UnAssignTasks", "Un-Assign Tasks From Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
        With TERouteStopTasks.ActionBar.Button("UnAssignTasks")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
        TERouteStopTasks.ActionBar.AddSpacer()
        TERouteStopTasks.ActionBar.AddExecButton("CompleteTask", "Complete Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        With TERouteStopTasks.ActionBar.Button("CompleteTask")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
        End With
        TERouteStopTasks.ActionBar.AddExecButton("CancelTask", "Cancel Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
        With TERouteStopTasks.ActionBar.Button("CancelTask")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.RouteTasks"
            .ConfirmMessage = "Are you sure you want to cancel selected tasks?"
        End With
        With TERouteStopTasks
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteTasks"
                If TERouteStopTasks.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                    .CommandName = "IncompleteTask"
                End If
            End With
        End With
    End Sub

    Private Sub TERoutesPackages_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutesPackages.CreatedChildControls
        With TERoutesPackages
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteTasks"
                If TERoutesPackages.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addpackage"
                ElseIf TERoutesPackages.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "editpackage"
                End If
            End With
        End With
    End Sub

End Class