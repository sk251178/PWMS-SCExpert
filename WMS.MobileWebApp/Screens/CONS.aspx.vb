Imports Made4Net.Shared.Web
Imports Made4Net.Shared
Imports Made4Net.Mobile
Imports Made4Net.DataAccess


<CLSCompliant(False)> Public Class CONS
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
            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("HUTYPE")
            dd.AllOption = True
            dd.TableName = "HANDELINGUNITTYPE"
            dd.ValueField = "CONTAINER"
            dd.TextField = "CONTAINERDESC"
            dd.DataBind()
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("FROMLOAD")
        DO1.AddTextboxLine("FROMCONTAINER")
        DO1.AddTextboxLine("TOCONTAINER", False, "next")
        DO1.AddTextboxLine("PRINTER")
        DO1.AddDropDown("HUTYPE")

        DO1.AddSpacer()
    End Sub

    Sub CLEARFIELDS()
        DO1.Value("FROMLOAD") = ""
        DO1.Value("FROMCONTAINER") = ""
        DO1.Value("TOCONTAINER") = ""
    End Sub

    'in consolidation screen, add HU type, dropdown, empty in the beginning.
    'Allow the user to consolidate to an existing empty container if he scanned one. 
    'If he scanned a container that does not exist he must fill the HU type and the system will create the new container and consolidate to it
    Private Function GetToContainer() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        '  If String.IsNullOrEmpty(DO1.Value("TOCONTAINER")) Or WMS.Logic.Container.Exists(DO1.Value("TOCONTAINER")) Then
        If String.IsNullOrEmpty(DO1.Value("HUTYPE")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("HUType cannot be empty"))
            Return False
        End If

            Dim cntr As New WMS.Logic.Container
            Dim location As String
            If Not DO1.Value("FROMLOAD") = String.Empty Then
                Dim ld As New WMS.Logic.Load(DO1.Value("FROMLOAD"))
                location = ld.LOCATION
            ElseIf Not DO1.Value("FROMCONTAINER") Is String.Empty Then
                Dim ct As New WMS.Logic.Container(DO1.Value("FROMCONTAINER"), False)
                location = ct.Location
            End If

            If DO1.Value("TOCONTAINER") <> "" Then
                cntr.ContainerId = DO1.Value("TOCONTAINER")
                cntr.HandlingUnitType = DO1.Value("HUTYPE")
                cntr.Location = location
                cntr.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea() 'DO1.Value("WAREHOUSEAREA")
                'cntr.Serial = DO1.Value("TOCONTAINER")
            Else
                cntr.HandlingUnitType = DO1.Value("HUTYPE")
                cntr.Location = location
                cntr.Warehousearea = WMS.Logic.Warehouse.getUserWarehouseArea() 'DO1.Value("WAREHOUSEAREA")
                'DO1.Value("TOCONTAINER") = Made4Net.Shared.getNextCounter("CONTAINER")
                'cntr.Serial = DO1.Value("TOCONTAINER")
            End If

            Try
                cntr.Post(WMS.Logic.Common.GetCurrentUser)
            Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return Nothing
            Catch ex As Exception
                Return Nothing
            End Try
            DO1.Value("TOCONTAINER") = cntr.ContainerId
        '  End If

        Return True
    End Function

    Private Sub doNext()

        Dim tocnt As WMS.Logic.Container
        Dim fromcont As WMS.Logic.Container
        Dim fromload As WMS.Logic.Load
        Dim cons As WMS.Logic.Consolidation
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        'If String.IsNullOrEmpty(DO1.Value("FROMCONTAINER")) Or DO1.Value("FROMCONTAINER") = DO1.Value("TOCONTAINER") Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal Container"))
        '    CLEARFIELDS()
        '    Return
        'End If

        'Dim LoadCount As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select count(1) from INVLOAD where HANDLINGUNIT = '{0}'", DO1.Value("FROMCONTAINER")))
        'If LoadCount = 0 Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not have any payloads"))
        '    'DO1.Value("FROMCONTAINER") = ""
        '    CLEARFIELDS()
        '    Return
        'End If

        

        If Not DO1.Value("FROMLOAD") = String.Empty Then
            Try
                'Commented for PWMS-598
                'fromload = New WMS.Logic.Load(DO1.Value("FROMLOAD"))
                'If Not WMS.Logic.Container.Exists(fromload.ContainerId) Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload does not belong to any container"))
                '    Exit Sub
                'End If
                'fromcont = New WMS.Logic.Container(fromload.ContainerId, True)
                'If fromcont.Status = WMS.Lib.Statuses.Container.LOADED Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload from container is already loaded"))
                '    Exit Sub
                'End If

                'If String.IsNullOrEmpty(fromload.ContainerId) Or fromload.ContainerId = DO1.Value("TOCONTAINER") Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal Container"))
                '    CLEARFIELDS()
                '    Return
                'End If

                'Dim LoadCount As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select count(1) from INVLOAD where HANDLINGUNIT = '{0}'", fromload.ContainerId))
                'If LoadCount = 0 Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not have any payloads"))
                '    'DO1.Value("FROMCONTAINER") = ""
                '    CLEARFIELDS()
                '    Return
                'End If

                'If Not validateLoads(fromload.ContainerId) Then Exit Sub
                'End Commented for PWMS-598
                'Added for PWMS-598
                fromload = New WMS.Logic.Load(DO1.Value("FROMLOAD"))
                Dim sAllowPayload As String = DataInterface.ExecuteScalar("SELECT param_value FROM sys_param WHERE param_code='AllowPayloadToContainerConsolidation'", "Made4NetSchema")
                If String.IsNullOrEmpty(sAllowPayload) Or sAllowPayload = "0" Then
                    If Not WMS.Logic.Container.Exists(fromload.ContainerId) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload does not belong to any container"))
                        Exit Sub
                    End If
                    fromcont = New WMS.Logic.Container(fromload.ContainerId, True)
                    If fromcont.Status = WMS.Lib.Statuses.Container.LOADED Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload from container is already loaded"))
                        Exit Sub
                    End If
                    If String.IsNullOrEmpty(fromload.ContainerId) Or fromload.ContainerId = DO1.Value("TOCONTAINER") Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal Container"))
                        CLEARFIELDS()
                        Return
                    End If

                    Dim LoadCount As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select count(1) from INVLOAD where HANDLINGUNIT = '{0}'", fromload.ContainerId))
                    If LoadCount = 0 Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not have any payloads"))
                        'DO1.Value("FROMCONTAINER") = ""
                        CLEARFIELDS()
                        Return
                    End If

                    If Not validateLoads(fromload.ContainerId) Then Exit Sub
                End If
                'End Added for PWMS-598

            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                Return
            End Try


            If validateTOContainer(DO1.Value("TOCONTAINER")) Then
                If Not validateContainer(fromload.ContainerId) Then Exit Sub
            ElseIf Not WMS.Logic.Container.Exists(DO1.Value("TOCONTAINER")) Then
                If Not GetToContainer() Then Exit Sub
            End If
            Try
                tocnt = New WMS.Logic.Container(DO1.Value("TOCONTAINER"), True)
                If tocnt.Status = WMS.Lib.Statuses.Container.LOADED Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload to container is already loaded"))
                    Exit Sub
                End If
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                CLEARFIELDS()
                Return
            End Try

            'RWMS-1313 - check whether load belongs to the shipment
            If Not isShipmentLoad(fromload.LOADID, tocnt.ContainerId) Then Exit Sub

            Try
                cons = New WMS.Logic.Consolidation
                cons.Consolidate(fromload, tocnt, WMS.Logic.Common.GetCurrentUser)

                sendCONSMSGforLoad(fromload, tocnt)

                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Consolidated"))
                DO1.Value("FROMCONTAINER") = ""
                DO1.Value("TOCONTAINER") = ""
                DO1.Value("FROMLOAD") = ""

            Catch ex As Exception
                Return
            End Try
        ElseIf Not DO1.Value("FROMCONTAINER") Is String.Empty Then
            Try

                If String.IsNullOrEmpty(DO1.Value("FROMCONTAINER")) Or DO1.Value("FROMCONTAINER") = DO1.Value("TOCONTAINER") Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal Container"))
                    CLEARFIELDS()
                    Return
                End If

                Dim LoadCount As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select count(1) from INVLOAD where HANDLINGUNIT = '{0}'", DO1.Value("FROMCONTAINER")))
                If LoadCount = 0 Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("From container does not have any payloads"))
                    'DO1.Value("FROMCONTAINER") = ""
                    CLEARFIELDS()
                    Return
                End If
                fromcont = New WMS.Logic.Container(DO1.Value("FROMCONTAINER"), True)
                If fromcont.Status = WMS.Lib.Statuses.Container.LOADED Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("From container is already loaded"))
                    Exit Sub
                End If
                If Not validateLoads(DO1.Value("FROMCONTAINER")) Then Exit Sub
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                CLEARFIELDS()
                Return
            End Try

            If validateTOContainer(DO1.Value("TOCONTAINER")) Then
                If Not validateContainer(DO1.Value("FROMCONTAINER")) Then Exit Sub
            ElseIf Not WMS.Logic.Container.Exists(DO1.Value("TOCONTAINER")) Then
                If Not GetToContainer() Then Exit Sub
            End If
            Try
                tocnt = New WMS.Logic.Container(DO1.Value("TOCONTAINER"), True)
                If tocnt.Status = WMS.Lib.Statuses.Container.LOADED Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("To container is already loaded"))
                    Exit Sub
                End If
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                CLEARFIELDS()
                Return
            End Try

            'RWMS-1313- check whether container belongs to the shipment
            If Not isShipmentContainer(fromcont.ContainerId, tocnt.ContainerId) Then Exit Sub

            Try

                cons = New WMS.Logic.Consolidation
                cons.Consolidate(fromcont, tocnt, WMS.Logic.Common.GetCurrentUser)

                sendCONSMSG(fromcont, tocnt)

                DO1.Value("FROMCONTAINER") = ""
                DO1.Value("FROMLOAD") = ""
                DO1.Value("TOCONTAINER") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container Consolidated"))
            Catch ex As Exception
                Return
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load or Container not entered"))
            Return
        End If

    End Sub

    'Start RWMS-1313 - check whether it is a shipmentload
    Private Function isShipmentLoad(ByVal pLoadId As String, ByVal pToContainerId As String) As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim sql As String
        'sql = String.Format("select count(1) from VShipmentLoads where LOADID='{0}' and SHIPMENT='{1}'", pLoadId, contShipment)
        sql = String.Format("select count(1) from VShipmentLoads where LOADID='{0}' and SHIPMENT=(SELECT top 1 SHIPMENT FROM vShipmentContainers WHERE CONTAINERID='{1}')", pLoadId, pToContainerId)
        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If sql = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload must belong to same shipment"))
            CLEARFIELDS()
            Return False
        Else
            Return True
        End If

    End Function

    Private Function isShipmentContainer(ByVal pContainerId As String, ByVal pToContainerId As String) As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim sql As String
        'sql = String.Format("select count(1) from vShipmentContainers where CONTAINERID='{0}' and SHIPMENT='{1}'", pContainerId, contShipment)
        sql = String.Format("select count(1) from vShipmentContainers where CONTAINERID='{0}' and SHIPMENT=(SELECT top 1 SHIPMENT FROM vShipmentContainers WHERE CONTAINERID='{1}')", pContainerId, pToContainerId)
        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If sql = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container must belong to same shipment"))
            CLEARFIELDS()
            Return False
        Else
            Return True
        End If

    End Function
    'End RWMS-1313

    Private Function validateContainer(ByVal fromContainerId As String) As Boolean
        Dim sql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim fromdt As New DataTable
        Dim todt As New DataTable

        sql = String.Format("SELECT count(1) FROM dbo.vValidateLoads l1 where l1.HANDLINGUNIT= '{0}' and not exists(SELECT DISTINCT TARGETCOMPANY, CONSIGNEE, SHIPTO, HANDLINGUNIT FROM dbo.vValidateLoads l2 where l2.HANDLINGUNIT= '{1}' and  l1.CONSIGNEE=l2.CONSIGNEE and l1.TARGETCOMPANY = l2.TARGETCOMPANY )", fromContainerId, DO1.Value("TOCONTAINER"))
        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If sql > 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Containers are related to different companies"))
            CLEARFIELDS()
            Return False
        End If

        sql = String.Format("SELECT count(1) FROM dbo.vValidateLoads l1 where l1.HANDLINGUNIT= '{0}' and not exists(SELECT DISTINCT TARGETCOMPANY, CONSIGNEE, SHIPTO, HANDLINGUNIT FROM dbo.vValidateLoads l2 where l2.HANDLINGUNIT= '{1}' and  l1.CONSIGNEE=l2.CONSIGNEE and l1.SHIPTO = l2.SHIPTO )", fromContainerId, DO1.Value("TOCONTAINER"))
        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If sql > 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Containers are related to different shipto"))
            CLEARFIELDS()
            Return False
        End If
        'sql = String.Format("SELECT count(1) FROM dbo.vValidateLoads l1 where l1.HANDLINGUNIT= '{0}' and not exists(SELECT DISTINCT TARGETCOMPANY, CONSIGNEE, SHIPTO, HANDLINGUNIT FROM dbo.vValidateLoads l2 where l2.HANDLINGUNIT= '{1}' and  l1.CONSIGNEE=l2.CONSIGNEE and l1.TARGETCOMPANY = l2.TARGETCOMPANY )", DO1.Value("FROMCONTAINER"), DO1.Value("TOCONTAINER"))
        'sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'If sql > 0 Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Containers related to different companies"))
        '    CLEARFIELDS()
        '    Return False
        'End If

        'sql = String.Format("SELECT count(1) FROM dbo.vValidateLoads l1 where l1.HANDLINGUNIT= '{0}' and not exists(SELECT DISTINCT TARGETCOMPANY, CONSIGNEE, SHIPTO, HANDLINGUNIT FROM dbo.vValidateLoads l2 where l2.HANDLINGUNIT= '{1}' and  l1.CONSIGNEE=l2.CONSIGNEE and l1.SHIPTO = l2.SHIPTO )", DO1.Value("FROMCONTAINER"), DO1.Value("TOCONTAINER"))
        'sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        'If sql > 0 Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Containers related to different shipto"))
        '    CLEARFIELDS()
        '    Return False
        'End If

        Return True
    End Function

    Private Function validateTOContainer(ByVal tocontainer As String) As Boolean
        Dim sql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        sql = String.Format("select count(1) from(select distinct targetcompany,CONSIGNEE,shipto from vValidateLoads where handlingunit='{0}')a", tocontainer)
        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If sql = 0 Then         
            Return False        
        Else
            Return True
        End If
    End Function

    Private Function validateLoads(ByVal container As String, Optional ByVal bLoad As Boolean = False) As Boolean
        Dim sql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        sql = String.Format("select count(1) from(select distinct targetcompany,CONSIGNEE,shipto from vValidateLoads where handlingunit='{0}')a", container)
        sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        If sql = 0 Then
            If bLoad Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Payload does not belong to any container"))
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not have any payloads"))
            End If
            'DO1.Value("FROMCONTAINER") = ""
            CLEARFIELDS()
            Return False
        ElseIf sql > 1 Then
           HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container payloads have different company, consignee or shipto"))
            'DO1.Value("FROMCONTAINER") = ""
            CLEARFIELDS()
            Return False
        Else

            Return True
        End If
    End Function

    Private Sub sendCONSMSGforLoad(ByVal fromload As WMS.Logic.Load, ByVal tocnt As WMS.Logic.Container)
        'with user, date, from container, to container, 
        'from status (of the from container), to status (of the to container), 
        'from location, to location

        Dim aq As New WMS.Logic.EventManagerQ
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EVENT", "8006")
        aq.Add("ACTIVITYTYPE", "CONTCONS")
        aq.Add("FROMCONTAINER", fromload.ContainerId)
        aq.Add("TOCONTAINER", tocnt.ContainerId)
        aq.Add("FROMSTATUS", fromload.STATUS)
        aq.Add("TOSTATUS", tocnt.Status)
        aq.Add("FROMLOCATION", fromload.LOCATION)
        aq.Add("TOLOCATION", tocnt.Location)

        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send("CONTCONS")
    End Sub
    Private Sub sendCONSMSG(ByVal fromcont As WMS.Logic.Container, ByVal tocnt As WMS.Logic.Container)
        'with user, date, from container, to container, 
        'from status (of the from container), to status (of the to container), 
        'from location, to location

        Dim aq As New WMS.Logic.EventManagerQ
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EVENT", "8006")
        aq.Add("ACTIVITYTYPE", "CONTCONS")
        aq.Add("FROMCONTAINER", fromcont.ContainerId)
        aq.Add("TOCONTAINER", tocnt.ContainerId)
        aq.Add("FROMSTATUS", fromcont.Status)
        aq.Add("TOSTATUS", tocnt.Status)
        aq.Add("FROMLOCATION", fromcont.Location)
        aq.Add("TOLOCATION", tocnt.Location)
	        aq.Add("FROMLOC", fromcont.Location)
	        aq.Add("TOLOC", tocnt.Location)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Send("CONTCONS")
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "printlabel"
                PrintContainerReport()
        End Select
    End Sub

    Public Sub PrintContainerLabel()
        Dim strcont As String
        Dim lblPrinter As String
        Dim fromcont As WMS.Logic.Container
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If String.IsNullOrEmpty(DO1.Value("TOCONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("illegal TOCONTAINER"))
            Return
        Else
            Try
                strcont = DO1.Value("TOCONTAINER")
                fromcont = New WMS.Logic.Container(strcont, True)
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                Return
            End Try
        End If


        If lblPrinter Is Nothing Then
            If String.IsNullOrEmpty(DO1.Value("PRINTER")) Then
                lblPrinter = ""
            Else
                lblPrinter = DO1.Value("PRINTER")
            End If
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "SHIPCONT")
        qSender.Add("LabelType", "SHIPCONT")
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        qSender.Add("CONTAINERID", strcont)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("CONTAINERID", strcont)
        qSender.Add("LabelDataParameters", WMS.Logic.Common.GetHashTableXMLString(ht))
        qSender.Send("LABEL", String.Format("SHIPCONT Container Label ({0})", strcont))
    End Sub

    Public Sub PrintContainerReport()
        Dim repType As String
        Dim strcont As String
        Dim lblPrinter As String
        Dim fromcont As WMS.Logic.Container
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim Language As Int32 = Made4Net.Shared.Translation.Translator.CurrentLanguageID
        Dim pUser As String = WMS.Logic.GetCurrentUser

        If String.IsNullOrEmpty(DO1.Value("TOCONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("illegal TOCONTAINER"))
            Exit Sub
        Else
            Try
                strcont = DO1.Value("TOCONTAINER")
                fromcont = New WMS.Logic.Container(strcont, True)
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Exit Sub
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                Exit Sub
            End Try
        End If

        repType = "ShipSum"
        'End If
        If lblPrinter Is Nothing Then
            If String.IsNullOrEmpty(DO1.Value("PRINTER")) Then
                lblPrinter = ""
            Else
                lblPrinter = DO1.Value("PRINTER")
            End If
        End If
        Dim oQsender As New Made4Net.Shared.QMsgSender

        Dim dt As New DataTable

        Made4Net.DataAccess.DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False)
        'Dim rep As Made4Net.Reporting.Common.Report
        'rep = Made4Net.Reporting.Common.Report.getReportInstance("ShipMan")
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        oQsender.Add("DATASETID", Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("SELECT ParamValue FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = 'DataSetName'", repType))) '"repOutboundDelNote")
        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", lblPrinter)
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("CONTAINERID = '{0}'", strcont))
        oQsender.Send("Report", repType)
    End Sub
End Class
