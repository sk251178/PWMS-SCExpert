Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Partial Public Class SplitPickedContainer1
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            If Not IsNothing(Session("SplitContainerID")) Then DO1.Value("SplitContainerID") = Session("SplitContainerID")
            If Not IsNothing(Session("SplitItem")) Then DO1.Value("SKU") = Session("SplitItem")
            'If Not IsNothing(Session("SplitConsignee")) Then DO1.Value("Consignee") = Session("SplitConsignee")
            Dim sk As New WMS.Logic.SKU(Session("SplitConsignee"), Session("SplitItem"))
            DO1.Value("SKUDesc") = sk.SKUDESC
            Dim company, companytype As String
            If Not getCompany(company, companytype, Session("SplitContainerID"), Session("SplitConsignee"), Session("SplitItem")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("SKU not found on the container")) 'No data was found in vSplitPickedContainer"))
                Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/SplitPickedContainer1.aspx"))
            End If

            Dim units As Decimal
            If Not getUnits(units, Session("SplitContainerID"), Session("SplitConsignee"), Session("SplitItem")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("SKU not found on the container")) 'No data was found in vSplitPickedContainer"))
                Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/SplitPickedContainer1.aspx"))
            End If
            Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(Session("SplitConsignee"), Session("SplitItem"), "CASE", units)

            Dim cmp As New WMS.Logic.Company(Session("SplitConsignee"), company, companytype)
            DO1.Value("TargetCompany") = cmp.COMPANY
            DO1.Value("TargetCompanyName") = cmp.COMPANYNAME

            SetHUTYpe()
            setUOM()
        End If
    End Sub

    Private Function getUnits(ByRef units As Decimal, ByVal Container As String, ByVal consignee As String, ByVal sku As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'SQL = String.Format("select sum(units) as sumunits from vSplitPickedContainer where Container = '{0}' and CONSIGNEE='{1}' and SKU='{2}' ", Container, consignee, sku)
        'SQL = String.Format("select sum(units) from(select distinct units,Container, CONSIGNEE,SKU as sumunits from vSplitPickedContainer where Container = '{0}' and CONSIGNEE='{1}' and SKU='{2}')tbl", Container, consignee, sku)
        SQL = String.Format("select sum(units) from(select units,Container, CONSIGNEE,SKU as sumunits from vSplitPickedContainer where Container = '{0}' and CONSIGNEE='{1}' and SKU='{2}')tbl", Container, consignee, sku)

        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("SKU not found on the container")) 'No data was found in vSplitPickedContainer"))
            ret = False
        Else
            If IsDBNull(dt.Rows(0)(0)) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container cannot be used for splitting")) 'No data was found in vSplitPickedContainer"))
                ret = False
            ElseIf Convert.ToDecimal(dt.Rows(0)(0)) Then
                units = dt.Rows(0)(0)
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("SKU not found on the container")) 'No data was found in vSplitPickedContainer"))
                ret = False
            End If
        End If
        Return ret
    End Function

    Private Function getCompany(ByRef company As String, ByRef companytype As String, ByVal Container As String, ByVal consignee As String, ByVal sku As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select top 1  company ,companytype from vSplitPickedContainer where Container = '{0}' and CONSIGNEE='{1}' and SKU='{2}' ", Container, consignee, sku)
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            ret = False
        Else
            company = dt.Rows(0)("company")
            companytype = dt.Rows(0)("companytype")
        End If
        Return ret
    End Function

    Private Sub doBack()
        Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/SplitPickedContainer1.aspx"))
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Select Case e.CommandText.ToLower

            Case "moveunits"
                If IsNothing(Session("TotalQtyPerNextClick")) And DO1.Value("Quantity") <> "" Then
                    Dim errMessage As String = String.Empty
                    If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("Quantity"), errMessage) Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errMessage)
                        DO1.Value("Quantity") = ""
                        Return
                    End If
                    ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                End If
                DO1.Value("Quantity") = ""

                moveunits()

            Case "addunits"
                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("Quantity"), errMessage) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, errMessage)
                    DO1.Value("Quantity") = ""
                    Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("Quantity") = ""

            Case "clearunits"
                ManageMutliUOMUnits.Clear()
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
            Case "back"
                ManageMutliUOMUnits.Clear(True)
                doBack()
        End Select
    End Sub

    Private Function CompareContainersComonData(ByVal Container As String, ByVal DestinationContainer As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        'o	At least one payload on the destination container belongs to the same target company,
        ' contact (SHIPTO) and shipment as the payload being transferred.
        'This validation will be done once, according to one of the payloads of the SKU on the original
        ' container (assumption – not possible to have 2 payloads of the same SKU for different customers)
        SQL = String.Format("select count(1) from vSplitCompareContainer where container = '{0}' and DestinationContainer='{1}'", Container, DestinationContainer)
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function

    Private Sub moveunits()
        Dim errMessage As String = String.Empty
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Not IsNumeric(ManageMutliUOMUnits.GetTotalEachUnits()) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Quantity field is mandatory")
            Return
        End If

        If (DO1.Value("DestinationContainer") <> "") Then
            Dim err As String
            If ValidContainer(DO1.Value("DestinationContainer"), err) Then
                'move loads
                MoveLoads(Session("SplitContainerID"), DO1.Value("DestinationContainer"), Session("SplitConsignee"), Session("SplitItem"), ManageMutliUOMUnits.GetTotalEachUnits())

            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err)
                Return
            End If
        ElseIf (DO1.Value("HUTYPE") <> "") Then
            DO1.Value("DestinationContainer") = CreateContainer()
            'move loads
            MoveLoads(Session("SplitContainerID"), DO1.Value("DestinationContainer"), Session("SplitConsignee"), Session("SplitItem"), ManageMutliUOMUnits.GetTotalEachUnits())

        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("HU type not valid"))
            Return
        End If


        DO1.Value("Quantity") = ""
        ManageMutliUOMUnits.Clear(True)
        Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/SplitPickedContainer1.aspx"))


    End Sub

    Private Function CreateContainer() As String
        Dim cntrSorce As New WMS.Logic.Container(Session("SplitContainerID").ToString, False)
        Dim cntr As New WMS.Logic.Container

        cntr.ContainerId = Made4Net.Shared.getNextCounter("CONTAINER")
        cntr.HandlingUnitType = DO1.Value("HUTYPE")
        cntr.Location = cntrSorce.Location
        cntr.Warehousearea = cntrSorce.Warehousearea

        Try
            cntr.Post(WMS.Logic.Common.GetCurrentUser)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
        Return cntr.ContainerId
    End Function

    Private Function ValidContainer(ByVal DestinationContainer As String, ByRef err As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Not WMS.Logic.Container.Exists(DestinationContainer) Then
            err = trans.Translate("Destination container does not exist")
            Return False
        End If
        'if no loads on container continue to move units
        SQL = String.Format("select count(1) from loads where HANDLINGUNIT = '{0}' ", DestinationContainer)
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = True
        End If

        'SQL = String.Format("select count(1) from vSplitPickedContainer where container = '{0}'", DestinationContainer)
        'SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        'If SQL > "0" Then
        '    err = trans.Translate("Destination container not valid")
        '    ret = False
        'Else
        If Not CompareContainersComonData(Session("SplitContainerID"), DestinationContainer) Then
            err = trans.Translate("Destination container does not have same attributes with source container")
            ret = False
        End If
        Return ret
    End Function

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls

        DO1.AddLabelLine("SplitContainerID", "Split container ID")
        DO1.AddLabelLine("SKU", "Item")
        DO1.AddLabelLine("SKUDesc", "Item description")
        DO1.AddLabelLine("TargetCompany")
        DO1.AddLabelLine("TargetCompanyName")
        DO1.AddTextboxLine("DestinationContainer", False, "", "Destination container")
        DO1.AddDropDown("HUTYPE")

        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim units As Decimal
        If Not getUnits(units, Session("SplitContainerID"), Session("SplitConsignee"), Session("SplitItem")) Then
            Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/SplitPickedContainer1.aspx"))
        End If
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(Session("SplitConsignee"), Session("SplitItem"), "CASE", units)

        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)

        DO1.AddTextboxLine("Quantity", False, "Quantity")

        DO1.AddDropDown("UOM")
    End Sub

    Private Sub SetHUTYpe()
        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("HUTYPE")
        dd.AllOption = True
        dd.AllOptionText = ""
        dd.TableName = "HANDELINGUNITTYPE"
        dd.ValueField = "CONTAINER"
        dd.TextField = "CONTAINERDESC"
        dd.DataBind()


    End Sub

    Private Sub setUOM()

        Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
        dd.AllOption = False
        dd.TableName = "SKUUOMDESC"
        dd.ValueField = "UOM"
        dd.TextField = "DESCRIPTION"
        dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", Session("SplitConsignee"), Session("SplitItem"))
        dd.DataBind()
        Try
            dd.SelectedValue = "CASE"
        Catch ex As Exception
        End Try

    End Sub

    Private Sub doMenu()
        Session.Remove("SplitContainerID")
        Session.Remove("SplitItem")
        Session.Remove("SplitConsignee")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Function MoveLoads(ByVal fromContainer As String, ByVal toContainer As String, ByVal consignee As String, ByVal sku As String, ByVal ConfirmUnits As Decimal) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable

        SQL = String.Format("select distinct loadid,units,consignee,ORDERID,ORDERLINE,PICKLIST,PICKLISTLINE,DOCUMENTTYPE from vSplitPickedContainer where consignee = '{0}' and sku='{1}' and Container='{2}' order by units desc", consignee, sku, fromContainer)
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Return False
        End If

        For Each dr As DataRow In dt.Rows
            'first look for the largest payload that the units are equal or less than the reminder of quantity to move
            If Not tryMoveAllQTY(fromContainer, toContainer, consignee, sku, ConfirmUnits) Then
                MoveLoad(dr("loadid"), toContainer, dr)
                ConfirmUnits = ConfirmUnits - Convert.ToDecimal(dr("units"))
            Else
                Exit For
            End If
        Next

        Return ret
    End Function

    Private Function tryMoveAllQTY(ByVal fromContainer As String, ByVal toContainer As String, ByVal consignee As String, ByVal sku As String, ByVal units As Decimal) As Boolean
        Dim ret As Boolean = True
        Dim loadid, loadunits As String
        Dim dr As DataRow
        ret = getLoadid(fromContainer, toContainer, consignee, sku, units, loadid, loadunits, dr)
        If ret Then
            If units = loadunits Then
                'move whole load to another container
                MoveLoad(loadid, toContainer, dr)
            Else
                'split load and then move to another container
                SplitLoad(loadid, units, toContainer, dr)
            End If

        End If
        Return ret
    End Function

    Private Function SplitLoad(ByVal loadid As String, ByVal loadunits As Decimal, ByVal toContainer As String, ByVal dr As DataRow) As Boolean

        Dim ld As New WMS.Logic.Load(loadid)
        Dim fromLd As New WMS.Logic.Load(loadid)

        Dim newLoadid As String = Made4Net.Shared.Util.getNextCounter("LOAD")
        Try
            Dim ACTIVITYSTATUS As String = ld.ACTIVITYSTATUS
            Dim SQL As String
            'Dim SQL As String = String.Format("Update loads set activitystatus='' where loadid='{0}'", loadid)
            'Made4Net.DataAccess.DataInterface.RunSQL(SQL)

            ld.ACTIVITYSTATUS = ""
            ld.Split(ld.LOCATION, ld.WAREHOUSEAREA, loadunits, newLoadid, WMS.Logic.GetCurrentUser)


            ' fromLd = New WMS.Logic.Load(loadid)

            '      ld.ACTIVITYSTATUS = ACTIVITYSTATUS
            'SQL = String.Format("Update loads set activitystatus='{1}' where loadid='{0}'", loadid, ACTIVITYSTATUS)
            'Made4Net.DataAccess.DataInterface.RunSQL(SQL)

            SQL = String.Format("Update loads set activitystatus='{1}',destinationlocation='{2}',DESTINATIONWAREHOUSEAREA='{3}' where loadid='{0}'", newLoadid, ACTIVITYSTATUS, fromLd.DESTINATIONLOCATION, fromLd.DESTINATIONWAREHOUSEAREA)
            Made4Net.DataAccess.DataInterface.RunSQL(SQL)


            'loadid,units,consignee,ORDERID,ORDERLINE,PICKLIST,PICKLISTLINE,DOCUMENTTYPE
            Dim oOrderLoad As New OrderLoads()

            oOrderLoad.CreateOrderLoad(dr("DOCUMENTTYPE"), dr("consignee"), dr("ORDERID"), dr("ORDERLINE"), newLoadid, dr("PICKLIST"), dr("PICKLISTLINE"), WMS.Logic.GetCurrentUser)

            MoveLoad(newLoadid, toContainer, dr, fromLd)
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return False
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  (ex.Message))
            Return False
            'Return
        End Try
        Return True

    End Function

    Private Function MoveLoad(ByVal loadid As String, ByVal toContainer As String, ByVal dr As DataRow, Optional ByVal fromLdId As WMS.Logic.Load = Nothing) As Boolean

        Dim cons As WMS.Logic.Consolidation

        cons = New WMS.Logic.Consolidation

        Dim ld As New WMS.Logic.Load(loadid)
        Dim fromLd As WMS.Logic.Load = ld

        If Not IsNothing(fromLdId) Then
            fromLd = fromLdId
        Else
            fromLd = ld
        End If

        Dim cnt As New WMS.Logic.Container(toContainer, False)

        cons.Consolidate(ld, cnt, WMS.Logic.Common.GetCurrentUser)

        Dim toLd As New WMS.Logic.Load(loadid)

        sendMSG(Session("SplitContainerID"), toContainer, dr("ORDERID"), dr("consignee"), fromLd.SKU, fromLd.LOADID, toLd.LOADID, fromLd.UNITS, toLd.UNITS, fromLd.STATUS, toLd.STATUS, fromLd.LOCATION, toLd.LOCATION)

        Return True
    End Function

    Private Function getLoadid(ByVal fromContainer As String, ByVal toContainer As String, ByVal consignee As String, ByVal sku As String, ByVal units As Decimal, ByRef loadid As String, ByRef loadunits As String, ByRef dr As DataRow) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable

        SQL = String.Format("select top 1 loadid,units,consignee,ORDERID,ORDERLINE,PICKLIST,PICKLISTLINE,DOCUMENTTYPE from vSplitPickedContainer where consignee = '{0}' and sku='{1}' and Container='{2}' and units >={3} order by units asc", consignee, sku, fromContainer, units)
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            ret = False
        Else
            loadid = dt.Rows(0)("loadid")
            loadunits = dt.Rows(0)("units")
            dr = dt.Rows(0)
        End If
        Return ret
    End Function

    Private Sub sendMSG(ByVal fromContainer As String, ByVal toContainer As String, ByVal orderid As String, _
    ByVal consignee As String, ByVal sku As String, ByVal fromload As String, ByVal toload As String, ByVal fromqty As String, ByVal toqty As String, ByVal fromstatus As String, _
    ByVal tostatus As String, ByVal fromloc As String, ByVal toloc As String)

        Dim MSG As String = "CONTSPLIT"
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", "2010")
        aq.Add("ACTIVITYTYPE", MSG)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("DOCUMENT", orderid)
        aq.Add("CONSIGNEE", consignee)
        aq.Add("SKU", sku)


        aq.Add("FROMCONTAINER", fromContainer)
        aq.Add("TOCONTAINER", toContainer)

        'FROMLOAD (if split then put the original load split from)
        aq.Add("FROMLOAD", fromload)
        aq.Add("TOLOAD", toload)

        aq.Add("FROMSTATUS", fromstatus)
        aq.Add("TOSTATUS", tostatus)

        aq.Add("FROMLOC", fromloc)
        aq.Add("TOLOC", toloc)

        'FROMQTY (if split then put the original qty of the load)
        aq.Add("FROMQTY", fromqty)
        aq.Add("TOQTY", toqty)

        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

        aq.Send(MSG)

    End Sub
End Class