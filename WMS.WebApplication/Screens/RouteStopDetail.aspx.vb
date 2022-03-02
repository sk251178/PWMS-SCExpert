Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert

Public Class RouteStopDetail
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TERoutes As Made4Net.WebControls.TableEditor
    Protected WithEvents TEStops As Made4Net.WebControls.TableEditor
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents TEAssignOrders As Made4Net.WebControls.TableEditor

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "Ctors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower

            Case "resequenceroute"
                Try
                    For Each rt As DataRow In ds.Tables(0).Rows
                        Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                        Dim DistRoute As Double = oRoute.ResequenceRoute()
                        Message = "New Route Distance: " & DistRoute.ToString()
                    Next

                Catch ex As Exception
                    Throw ex
                End Try

            Case "checkfeasibility"
                Try
                    For Each rt As DataRow In ds.Tables(0).Rows
                        Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))

                        Dim msg As String
                        oRoute.CheckFeasibility(msg)
                        If msg = String.Empty Then
                            Message = "Route Feasible."
                        Else
                            Message = msg
                        End If
                    Next

                Catch ex As Exception
                    Throw ex
                End Try

            Case "printmanifest"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                    oRoute.PrintRouteDriverManifest(Made4Net.Shared.Translation.Translator.CurrentLanguageID, Common.GetCurrentUser)
                Next
            Case "setdriverveh"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                    oRoute.Driver = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("DRIVER")) 'Made4Net.Shared.Conversion.Convert.ReplaceDBNull(DataInterface.ExecuteScalar("SELECT DRIVERID FROM VEHICLE WHERE VEHICLEID='" & Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("VEHICLEID")) & "'"))
                    oRoute.Vehicle = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("VEHICLEID"))
                    oRoute.VehicleType = DataInterface.ExecuteScalar("SELECT VEHICLETYPENAME FROM VEHICLE WHERE VEHICLEID='" & Made4Net.Shared.Conversion.Convert.ReplaceDBNull(rt("VEHICLEID")) & "'")
                    oRoute.Save(UserId)
                Next
            Case "createnewroute"
                dr = ds.Tables(0).Rows(0)
                Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route
                oRoute.Depo = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("depo"))
                oRoute.EndPoint = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("endpoint"))
                oRoute.RouteDate = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("routedate"))
                oRoute.RouteName = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("routename"))
                oRoute.RouteSet = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("routeset"))
                oRoute.StartPoint = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("startpoint"))
                oRoute.Status = RouteStatus.[New]
                oRoute.Territory = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("TERRITORY"))
                oRoute.Driver = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("DRIVER"))
                oRoute.Vehicle = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLEID"))
                oRoute.VehicleType = DataInterface.ExecuteScalar("SELECT VEHICLETYPENAME FROM VEHICLE WHERE VEHICLEID='" & Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("VEHICLEID")) & "'")
                oRoute.Save(UserId)
            Case "deassignstopdetail"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                    'oRoute.CancelStopDetail(ReplaceDBNull(rt("CONSIGNEE")), ReplaceDBNull(rt("DOCUMENTID")), ReplaceDBNull(rt("company")), ReplaceDBNull(rt("companytype")), Common.GetCurrentUser)
                    Dim oRouteStop As New WMS.Logic.RouteStop(Convert.ToString(rt("ROUTEID")), Convert.ToInt32(rt("STOPNUMBER")))
                    Dim oRouteStopDet As New WMS.Logic.RouteStopTask(Convert.ToString(rt("ROUTEID")), Convert.ToInt32(rt("STOPNUMBER")), Convert.ToInt32(rt("STOPTASKID")))

                    Dim NewTotalVolume, NewTotalWeight As Double
                    If (oRouteStop.Status <> StopStatus.Completed) Then
                        If oRouteStopDet.Status = StopTaskStatus.Canceled Then
                            oRouteStopDet.SetStatus(StopTaskStatus.New, "", Common.GetCurrentUser)
                            NewTotalVolume = oRoute.TotalVolume + oRouteStopDet.StopDetailVolume
                            NewTotalWeight = oRoute.TotalWeight + oRouteStopDet.StopDetailWeight
                        Else : oRouteStopDet.Status = StopTaskStatus.New
                            oRouteStopDet.SetStatus(StopTaskStatus.Canceled, "", Common.GetCurrentUser)
                            NewTotalVolume = oRoute.TotalVolume - oRouteStopDet.StopDetailVolume
                            NewTotalWeight = oRoute.TotalWeight - oRouteStopDet.StopDetailWeight
                        End If
                    End If
                    oRoute.SetTotalVolume(Math.Round(NewTotalVolume, 3))
                    oRoute.SetTotalWeight(Math.Round(NewTotalWeight, 3))

                    oRouteStop.CancelStopDetail(oRouteStopDet, Common.GetCurrentUser)
                Next
            Case "setstatus"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRouteStopDetail As WMS.Logic.RouteStopTask = New WMS.Logic.RouteStopTask(rt("ROUTEID"), rt("STOPNUMBER"), Convert.ToInt32(rt("STOPTASKID")))
                    oRouteStopDetail.SetStatus(oRouteStopDetail.StopTaskStatusFromString(rt("STATUS")), "", Common.GetCurrentUser)
                Next
            Case "updatestopnumber"
                Try
                    Dim routeID As String = ds.Tables(0).Rows(0)("ROUTEID")
                    Dim oRoute As New WMS.Logic.Route(routeID)

                    For Each rt As DataRow In ds.Tables(0).Rows
                        Dim newStopNumber As Int32 = Convert.ToInt32(rt("NewStopNumber"))
                        Dim oldStopNumber As Int32 = Convert.ToInt32(rt("stopnumber"))

                        Dim whc As String = String.Format(" routeid = {0} and stopnumber = {1} ", _
                            Made4Net.Shared.Util.FormatField(routeID), _
                            Made4Net.Shared.Util.FormatField(newStopNumber))
                        Dim SQL As String = String.Format("select count(*) from routestop where {0} ", whc)
                        Dim isExistsNewStop As Integer = DataInterface.ExecuteScalar(SQL)
                        Dim tempStopNumber As Int32

                        If isExistsNewStop > 0 Then
                            tempStopNumber = (New Random).Next(9000000, 9999999)
                            Dim oNewRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(routeID, newStopNumber)
                            oNewRouteStop.UpdateStopNumber(tempStopNumber, Common.GetCurrentUser)
                        End If

                        Dim oRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(routeID, oldStopNumber)
                        oRouteStop.UpdateStopNumber(newStopNumber, Common.GetCurrentUser)

                        If isExistsNewStop > 0 Then
                            Dim otempRouteStop As WMS.Logic.RouteStop = New WMS.Logic.RouteStop(routeID, tempStopNumber)
                            otempRouteStop.UpdateStopNumber(oldStopNumber, Common.GetCurrentUser)
                        End If

                    Next
                Catch ex As Exception
                    Throw ex
                End Try

            Case "createnewroutetask"
                dr = ds.Tables(0).Rows(0)
                Dim stopNum As Int32 = -1
                Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(dr("ROUTEID"))
                Dim oRouteStopTask As New WMS.Logic.RouteStopTask
                Dim stdType As StopTaskType = RouteStopTask.StopTaskTypeFromString(Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("stoptasktype")))
                If Not IsDBNull(dr("stopnumber")) Then stopNum = dr("stopnumber")
                oRoute.AddStopDetail(stdType, oRoute.RouteDate, ReplaceDBNull(dr("consignee")), "", "", _
                    ReplaceDBNull(dr("company")), ReplaceDBNull(dr("companytype")), "", "", 0, "", 0, 0, 0, _
                    ReplaceDBNull(dr("comments")), StopTaskConfirmationType.None, Common.GetCurrentUser, stopNum)
                oRoute = Nothing
                oRouteStopTask = Nothing
            Case "assignordertoroute"
                dr = ds.Tables(0).Rows(0)
                Dim sql As String = String.Format("select count(*) from ROUTESTOPTASK where STATUS not in ('{1}','{2}') and DOCUMENTID='{0}' " & _
                        " and routeid in (select routeid from route where runid in (select runid from route where routeid='{3}'))", _
                    ReplaceDBNull(dr("orderid")), RouteStatus.Canceled, RouteStatus.Closed, ReplaceDBNull(dr("ROUTEID")))

                Dim cntTasks As Integer = DataInterface.ExecuteScalar(sql)
                If cntTasks > 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Cannot assign. This order is still assign to other routes.", "Cannot assign. This order is still assign to other routes.")
                    Return
                End If

                Dim vol, weight As Double
                Dim stopNum As Int32 = -1
                For Each dr In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(dr("ROUTEID"))
                    Dim oRouteStopDetail As New WMS.Logic.RouteStopTask
                    Dim oOrd As New OutboundOrderHeader(ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("orderid")))
                    If Not IsDBNull(dr("stopnumber")) Then stopNum = dr("stopnumber")
                    Dim stdType As StopTaskType = StopTaskType.Delivery
                    'Dim drord As DataRow() = dtord.Select(String.Format("consignee = {0} and orderid = {1}", oOrd.CONSIGNEE, oOrd.ORDERID))
                    Try
                        vol = dr("ordervolume")
                        weight = dr("orderweight")
                    Catch ex As Exception
                        vol = 0D
                        weight = 0D
                    End Try
                    oRoute.AddStopDetail(stdType, oOrd.SCHEDULEDDATE, oOrd.CONSIGNEE, oOrd.ORDERID, oOrd.ORDERTYPE, _
                        oOrd.TARGETCOMPANY, oOrd.COMPANYTYPE, "", "", 0, "", vol, weight, _
                        0, oOrd.NOTES, StopTaskConfirmationType.None, Common.GetCurrentUser, stopNum)
                    oRoute.SetTotalVolume(Math.Round(oRoute.TotalVolume + vol, 3))
                    oRoute.SetTotalWeight(Math.Round(oRoute.TotalWeight + weight, 3))
                    oRoute.SetStatus(RouteStatus.Assigned, DateTime.Now, Common.GetCurrentUser)

                    oRoute = Nothing
                    oRouteStopDetail = Nothing
                Next
                Message = "Orders Assigned To Route"

            Case "returnorder"
                For Each dr In ds.Tables(0).Rows
                    Dim consignee, orderid, sourceComp, SourceCompType, Status As String
                    Status = ReplaceDBNull(dr("status"))
                    If Status.ToLower = "completed" Or Status.ToLower = "canceled" Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Drop Status Incorrect", "Drop Status Incorrect")
                    End If
                    sourceComp = Made4Net.Shared.SysParam.Get("RoutingTransShipSourceComp")
                    SourceCompType = Made4Net.Shared.SysParam.Get("RoutingTransShipSourceCompType")
                    consignee = ReplaceDBNull(dr("consignee"))
                    orderid = ReplaceDBNull(dr("DOCUMENTID"))
                    If consignee = "" Or orderid = "" Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create transshipment.Invalid Order", "Cannot create transshipment.Invalid Order")
                    End If
                    'get the order vol and weight
                    Dim outord As New OutboundOrderHeader(consignee, orderid)
                    Dim dtOrdParam As New DataTable
                    Dim sql As String = String.Format("select * from ORDERSROUTINGPARAMS where consignee = '{0}' and orderid = '{1}'", consignee, orderid)
                    Made4Net.DataAccess.DataInterface.FillDataset(sql, dtOrdParam)
                    Dim ordVol As Double = ReplaceDBNull(dtOrdParam.Rows(0)("ordervolume"))
                    Dim ordQty As Double = ReplaceDBNull(dtOrdParam.Rows(0)("orderunits"))
                    Dim oTransShip As New TransShipment
                    oTransShip.CreateNew(consignee, orderid, outord.ORDERTYPE, outord.REFERENCEORD, sourceComp, SourceCompType, outord.TARGETCOMPANY, outord.COMPANYTYPE, _
                                     DateTime.Now, outord.NOTES, DateTime.Now, DateTime.Now, outord.REQUESTEDDATE, outord.SCHEDULEDDATE, outord.REQUESTEDDATE, outord.STAGINGLANE, outord.STAGINGWAREHOUSEAREA, _
                                     "", "", outord.LOADINGSEQ, outord.ROUTINGSET, "", "", "", outord.ORDERPRIORITY, _
                                    ordQty, ordQty, Convert.ToInt32(ordVol), Convert.ToInt32(ordVol), "", "", DateTime.Now, Common.GetCurrentUser, DateTime.Now, Common.GetCurrentUser)
                    'outord.SetRoute("", Common.GetCurrentUser)
                    'And cancel the order from the drop
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(dr("ROUTEID"))
                    oRoute.CancelStopDetail(ReplaceDBNull(dr("CONSIGNEE")), ReplaceDBNull(dr("DOCUMENTID")), ReplaceDBNull(dr("company")), ReplaceDBNull(dr("companytype")), Common.GetCurrentUser)
                Next
            Case "confirmroute"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                    oRoute.Confirm(Common.GetCurrentUser)
                Next
            Case "cancelroute"
                For Each rt As DataRow In ds.Tables(0).Rows
                    Dim oRoute As WMS.Logic.Route = New WMS.Logic.Route(rt("ROUTEID"))
                    oRoute.Cancel(Common.GetCurrentUser)
                Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEStops_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEStops.CreatedChildControls
        With TEStops.ActionBar
            .AddExecButton("DeAssignStopDetail", "UnAssign Stop Detail", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
            With .Button("DeAssignStopDetail")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "DeAssignStopDetail"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
            .AddSpacer()
            .AddExecButton("ReturnOrder", "Return Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
            With .Button("ReturnOrder")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "ReturnOrder"
                .ConfirmMessage = "Do you want to ReDeliver this order?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
            '.AddSpacer()
            '.AddExecButton("SetStatus", "Set Stop Detail Status", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            'With .Button("SetStatus")
            '    .ObjectDLL = "WMS.WebApp.dll"
            '    .ObjectName = "WMS.WebApp.RouteStopDetail"
            '    .CommandName = "SetStatus"
            '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            'End With
            .AddSpacer()
            .AddExecButton("UpdateStopNumber", "Update Stop Number", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("UpdateStopNumber")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "UpdateStopNumber"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
            With .Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "createnewroutetask"
            End With
        End With
    End Sub

    Private Sub TERoutes_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TERoutes.CreatedChildControls
        With TERoutes.ActionBar

            .AddExecButton("ResequenceRoute", "Resequence Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("ResequenceRoute")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "resequenceroute"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddExecButton("CheckFeasibility", "Check Feasibility", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("CheckFeasibility")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "checkfeasibility"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With


            .AddExecButton("SetDriverVehicle", "Set Driver&Vehicle", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SetDriverVehicle")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "setdriverveh"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("PrintManifest", "Print Driver Manifest", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            With .Button("PrintManifest")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "PrintManifest"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("ConfirmRoute", "Confirm Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarExecuteScheduler"))
            With .Button("ConfirmRoute")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "ConfirmRoute"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to Confirm the selected Routes?"
            End With

            .AddSpacer()
            .AddExecButton("CancelRoute", "Cancel Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
            With .Button("CancelRoute")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "CancelRoute"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to Cancel the selected Routes?"
            End With

            With .Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "createnewroute"
            End With

        End With
    End Sub

    Private Sub TEStops_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEStops.AfterItemCommand
        If e.CommandName.ToLower <> "new" And e.CommandName.ToLower <> "search" And e.CommandName.ToLower <> "find" Then
            TEStops.Restart()
        End If
    End Sub

    Private Sub TEAssignOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignOrders.CreatedChildControls
        With TEAssignOrders.ActionBar
            Dim nvcol As New Specialized.NameValueCollection
            nvcol.Add("ROUTEID", TERoutes.SelectedRecordPrimaryKeyValues.Get("ROUTEID"))
            TEAssignOrders.PreDefinedValues = nvcol
            .AddSpacer()
            .AddExecButton("AssignOrderToRoute", "Assign Order To Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("AssignOrderToRoute")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.RouteStopDetail"
                .CommandName = "AssignOrderToRoute"
                .ConfirmMessage = "Do you want to Assign Orders to Route?"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

    Private Sub TERoutes_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TERoutes.AfterItemCommand
        TERoutes.RefreshData()
    End Sub

    Private Sub TERoutes_BeforeItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TERoutes.BeforeItemCommand

    End Sub
End Class
