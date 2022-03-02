Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports System.Collections.Generic

<CLSCompliant(False)> Public Class LOADING2DiffShip2
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            If IsNothing(Session("LoadingPalletDict")) Then
                Response.Redirect(MapVirtualPath("Screens/LOADING2DiffShip.aspx"))
            End If
            setScreen()
        End If
    End Sub

    Private Sub doNext()
        Try
            Dim SQL As String
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Dim dict As Dictionary(Of String, String) = Session("LoadingPalletDict")
            Dim oShip As New WMS.Logic.Shipment(dict.Item("NEWSHIPMENT"))

            If DO1.Value("CONFIRM").ToUpper <> oShip.DOOR.ToUpper Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Door confirmation incorrect"))
                Return
            End If
        
            If dict.Item("TYPE") = "LOAD" Then
                Dim l As New WMS.Logic.Load(dict.Item("PALLETID"))


                'Added for RWMS-2343 RWMS-2314 - Add the respective orderlines to new shipmentdetail and remove the same from the existing shipmentdetail
                Dim dt As New DataTable
                SQL = "SELECT LOADID,ORDERID,ORDERLINE FROM vshipmentloads1 "
                SQL += " WHERE SHIPMENT<>'' and LOADID = '{0}'"
                SQL = String.Format(SQL, dict.Item("PALLETID"))
                Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

                For Each DR As DataRow In dt.Rows
                    Dim oSql As String = String.Format("update shipmentdetail set shipment={0},edituser={1},editdate=GETDATE() where shipment={2} and orderid={3} and orderline={4}", _
                Made4Net.Shared.Util.FormatField(dict.Item("NEWSHIPMENT")), Made4Net.Shared.Util.FormatField(WMS.Logic.GetCurrentUser), Made4Net.Shared.Util.FormatField(dict.Item("SHIPMENT")), Made4Net.Shared.Util.FormatField(DR("ORDERID")), Made4Net.Shared.Util.FormatField(DR("ORDERLINE")))
                    DataInterface.RunSQL(oSql)
                Next
                'End Added for RWMS-2343 RWMS-2314

                l.Load(DO1.Value("CONFIRM"), oShip.STAGINGWAREHOUSEAREA, WMS.Logic.Common.GetCurrentUser)

                'SQL = String.Format("UPDATE SHIPMENTLOADS SET SHIPMENT = '{0}' WHERE LOADID = '{1}' ", dict.Item("NEWSHIPMENT"), dict.Item("SHIPMENT"))
                SQL = String.Format("INSERT INTO SHIPMENTLOADS(SHIPMENT,LOADID,ADDDATE,ADDUSER,EDITDATE,EDITUSER) VALUES( '{0}','{1}',GETDATE(),'{2}',GETDATE(),'{2}')", dict.Item("NEWSHIPMENT"), dict.Item("PALLETID"), WMS.Logic.GetCurrentUser)

                'Added for for RWMS-2343 RWMS-2314
                Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
                'End Added for RWMS-2343 RWMS-2314

            Else
                Dim dt As New DataTable
                'Commented for RWMS-2343 RWMS-2314
                'SQL = "SELECT LOADID FROM vLoadingContainer "
                'End Commented for RWMS-2343 RWMS-2314
                'Added for for RWMS-2343 RWMS-2314
                SQL = "SELECT LOADID,ORDERID,ORDERLINE FROM vLoadingContainer "
                'End Added for RWMS-2343 RWMS-2314
                SQL += " WHERE SHIPMENT<>'' and CONTAINERID = '{0}'"
                SQL = String.Format(SQL, dict.Item("PALLETID"))
                Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

                For Each DR As DataRow In dt.Rows
                    Dim l As New WMS.Logic.Load(DR("LOADID").ToString)
                    l.Load(DO1.Value("CONFIRM"), oShip.STAGINGWAREHOUSEAREA, WMS.Logic.Common.GetCurrentUser)

                    'Added for RWMS-2343 RWMS-2314 - Add the respective orderlines to new shipmentdetail and remove the same from the existing shipmentdetail
                    Dim oSql As String = String.Format("update shipmentdetail set shipment={0},edituser={1},editdate=GETDATE() where shipment={2} and orderid={3} and orderline={4}", _
                    Made4Net.Shared.Util.FormatField(dict.Item("NEWSHIPMENT")), Made4Net.Shared.Util.FormatField(WMS.Logic.GetCurrentUser), Made4Net.Shared.Util.FormatField(dict.Item("SHIPMENT")), Made4Net.Shared.Util.FormatField(DR("ORDERID")), Made4Net.Shared.Util.FormatField(DR("ORDERLINE")))
                    DataInterface.RunSQL(oSql)
                    'End Added for RWMS-2343 RWMS-2314
                Next
                '
                '                SQL = String.Format("UPDATE SHIPMENTLOADS SET SHIPMENT = '{0}' WHERE SHIPMENT = '{1}' ", dict.Item("NEWSHIPMENT"), dict.Item("SHIPMENT"))
                SQL = String.Format("INSERT INTO SHIPMENTLOADS(SHIPMENT,LOADID,ADDDATE,ADDUSER,EDITDATE,EDITUSER) SELECT '{0}',LOADID,GETDATE(),'{1}',GETDATE(),'{1}' FROM vLoadingContainer where SHIPMENT<>'' AND CONTAINERID  = '{2}'", dict.Item("NEWSHIPMENT"), WMS.Logic.GetCurrentUser, dict.Item("PALLETID"))

                'Added for RWMS-2343 RWMS-2314
                Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
                'End Added for RWMS-2343 RWMS-2314

                'Added for RWMS-2343 RWMS-2314
                SetContainerStatus(WMS.Logic.GetCurrentUser, dict.Item("NEWSHIPMENT"), dict.Item("PALLETID"))
                'End Added for RWMS-2343 RWMS-2314

            End If


            'Commented for RWMS-2343 RWMS-2314
            'Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
            'End Commented for RWMS-2343 RWMS-2314

            'oShip = New WMS.Logic.Shipment(dict.Item("SHIPMENT"))
            ''

            'oShip.LoadPallet(WMS.Logic.GetCurrentUser)

            LoadPallet(WMS.Logic.GetCurrentUser, dict.Item("SHIPMENT"))

            'Added for RWMS-2343 RWMS-2314
            LoadPallet(WMS.Logic.GetCurrentUser, dict.Item("NEWSHIPMENT"))
            'End Added for RWMS-2343 RWMS-2314

            Response.Redirect(MapVirtualPath("Screens/LOADING2DiffShip.aspx"))

        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

    'Added for RWMS-2343 RWMS-2314 - update container status
    Public Sub SetContainerStatus(ByVal pUser As String, ByVal SHIPMENT As String, ByVal ContainerId As String)
        Dim dtShipmentContainer As New DataTable()
        Dim sqlShipmentContainer As String = String.Format("select CONTAINERID from vShipmentContainers where shipment={0} and CONTAINERID={1}", _
        Made4Net.Shared.FormatField(SHIPMENT), Made4Net.Shared.FormatField(ContainerId))
        Made4Net.DataAccess.DataInterface.FillDataset(sqlShipmentContainer, dtShipmentContainer)
        If dtShipmentContainer.Rows.Count > 0 Then
            Dim containerStatus As String = WMS.Lib.Statuses.Container.LOADED
            Dim containeredituser As String = pUser
            Dim containereditdate As String = DateTime.Now
            Dim sqlContainer As String = String.Format("UPDATE CONTAINER SET status = {0},EDITUSER={1},EDITDATE={2} WHERE CONTAINER={3}", Made4Net.Shared.Util.FormatField(containerStatus), Made4Net.Shared.Util.FormatField(containeredituser), Made4Net.Shared.Util.FormatField(containereditdate), Made4Net.Shared.Util.FormatField(ContainerId))
            DataInterface.RunSQL(sqlContainer)
        End If
    End Sub
    'End Added for RWMS-2343 RWMS-2314

    Public Sub LoadPallet(ByVal pUser As String, ByVal SHIPMENT As String)
        'Added for RWMS-2343 RWMS-2314
        Dim dt As New DataTable()
        Dim sql As String = String.Format("select loadid from shipmentloads where shipment={0}", _
        Made4Net.Shared.FormatField(SHIPMENT))

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        'End Added for RWMS-2343 RWMS-2314

        Dim oShip As New WMS.Logic.Shipment(SHIPMENT)

        If oShip.STATUS = WMS.Lib.Statuses.Shipment.ASSIGNED Or oShip.STATUS = WMS.Lib.Statuses.Shipment.ATDOCK Then
            If IsLoadingCompleted(SHIPMENT) Then
                oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)
            Else
                'Commented for RWMS-2343 RWMS-2314
                'UpdateStartLoadingDate(pUser, SHIPMENT)
                'oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
                'End Commented for RWMS-2343 RWMS-2314
                'Added for RWMS-2343 RWMS-2314
                If dt.Rows.Count > 1 Then
                    UpdateStartLoadingDate(pUser, SHIPMENT)
                    oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
                Else
                    oShip.SetStatus(WMS.Lib.Statuses.Shipment.ATDOCK, pUser)
                End If
                'End Added for RWMS-2343 RWMS-2314
            End If
        ElseIf oShip.STATUS = WMS.Lib.Statuses.Shipment.LOADING Then
            If IsLoadingCompleted(SHIPMENT) Then
                oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADED, pUser)
                'Added for RWMS-2343 RWMS-2314
            Else
                If dt.Rows.Count > 1 Then
                    oShip.SetStatus(WMS.Lib.Statuses.Shipment.LOADING, pUser)
                Else
                    oShip.SetStatus(WMS.Lib.Statuses.Shipment.ATDOCK, pUser)
                End If
                'End Added for RWMS-2343 RWMS-2314
            End If
        End If
    End Sub

    Private Sub UpdateStartLoadingDate(ByVal pUser As String, ByVal SHIPMENT As String)
        Dim sql As String = String.Format("Update SHIPMENT SET STARTLOADINGDATE = {0},EditUser = {1},EditDate = {2} Where SHIPMENT='{3}'", _
            Made4Net.Shared.Util.FormatField(DateTime.Now), Made4Net.Shared.Util.FormatField(pUser), Made4Net.Shared.Util.FormatField(DateTime.Now), SHIPMENT)
        DataInterface.RunSQL(sql)
    End Sub


    Public Function IsLoadingCompleted(ByVal SHIPMENT As String) As Boolean

        'Commented for RWMS-2343 RWMS-2314
        'Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits  from (select sd.SHIPMENT,SUM(sd.units) as units from SHIPMENTDETAIL sd group by SHIPMENT) sd left outer join (SELECT     sp1.SHIPMENT, SUM(iv.UNITS) AS unitsLoaded FROM         dbo.SHIPMENTDETAIL AS sp1 INNER JOIN dbo.ORDERLOADS AS ol ON sp1.CONSIGNEE =ol.CONSIGNEE AND sp1.ORDERID = ol.ORDERID INNER JOIN dbo.INVLOAD AS iv ON ol.LOADID = iv.LOADID INNER JOIN dbo.SHIPMENTLOADS sl1 ON ol.LOADID = sl1.LOADID GROUP BY sp1.SHIPMENT) sl on sl.SHIPMENT = sd.SHIPMENT where sd.SHIPMENT = '{0}'", SHIPMENT)
        'End Commented for RWMS-2343 RWMS-2314
        'Added for for RWMS-2343 RWMS-2314
        Dim sql As String = String.Format("select sd.units - ISNULL(sl.unitsloaded,0) as RemainingUnits from (select sd.SHIPMENT,SUM(sd.units) as units from" & _
            " SHIPMENTDETAIL sd inner join outboundordetail od on sd.ORDERID=od.ORDERID and sd.ORDERLINE=od.ORDERLINE and sd.CONSIGNEE=od.CONSIGNEE and od.QTYMODIFIED<>0 group by SHIPMENT) sd left outer join (select sl.SHIPMENT,sum(iv.UNITS) as unitsLoaded from SHIPMENTLOADS sl inner join LOADS" & _
            " iv on sl.LOADID = iv.LOADID group by sl.SHIPMENT ) sl on sl.SHIPMENT = sd.SHIPMENT where sd.SHIPMENT = '{0}'", SHIPMENT)
        'End Added for RWMS-2343 RWMS-2314

        Dim remainingUnits As Decimal = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If remainingUnits > 0 Then
            Return False
        Else
            'Added for RWMS-2343 RWMS-2314 - check if Shipment has orders assigned to it. If no orders are assigned, then return FALSE
            Dim shipmentDetailCount As Integer
            sql = String.Format("select count(*) from SHIPMENTDETAIL where SHIPMENT='{0}'", SHIPMENT)
            shipmentDetailCount = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            If shipmentDetailCount <= 0 Then
                Return False
            End If
            'End Added for RWMS-2343 RWMS-2314
            Return True
        End If


    End Function

    Private Sub doMenu()
        Session.Remove("LoadingPalletDict")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("Screens/LOADING2DiffShip1.aspx"))
    End Sub

    Private Sub setScreen()

        DO1.Value("Note") = "Loading to a different shipment"

        Dim dict As Dictionary(Of String, String) = Session("LoadingPalletDict")

        DO1.Value("AssignedShipment") = dict.Item("SHIPMENT")

        DO1.Value("PalletId") = dict.Item("PALLETID")
        DO1.Value("CompanyName") = dict.Item("COMPANYNAME")

        DO1.Value("NewShipment") = dict.Item("NEWSHIPMENT").ToString()

        Dim sp As New WMS.Logic.Shipment(dict.Item("NEWSHIPMENT"))

        Try
            Dim cr As New WMS.Logic.Carrier(sp.CARRIER)
            DO1.Value("CarrierName") = cr.CARRIERNAME
        Catch ex As Exception
        End Try

        DO1.Value("Vehicle") = sp.VEHICLE
        DO1.Value("Door") = sp.DOOR

        DO1.Value("CONFIRM") = ""

        'If Not CType(WarehouseParams.GetWarehouseParam("ShowPositionLoading"), Boolean) Then
        '    DO1.setVisibility("Position", False)
        'End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Note")
        DO1.AddLabelLine("AssignedShipment", "Assigned Shipment")
        
        DO1.AddLabelLine("PalletId")
        DO1.AddLabelLine("CompanyName")
        DO1.AddSpacer()

        DO1.AddLabelLine("NewShipment", "New Shipment")
        DO1.AddLabelLine("CarrierName")
        DO1.AddLabelLine("Vehicle")
        DO1.AddLabelLine("Door")

        DO1.AddSpacer()
        DO1.AddTextboxLine("CONFIRM")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class


