Imports Made4Net.DataAccess
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager
Imports Made4Net.Shared.Web

Public Class AppPagePick
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Picked
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Try
            PrintMessageContent(oLogger)
            If Not ValidateUserLoggedIn() Then
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("User is not logged in. Please sign in again."))
                End If
                Me._responseCode = ResponseCode.NoUserLoggedIn
                Me._responseText = "User is not logged in"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to pick...")
            End If
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            If Not Session("PCKPicklist") Is Nothing Then
                Return PickPicklist(_msg(0)("Device ID").FieldValue, oLogger)
            ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                Return PickParallelPicklist(_msg(0)("Device ID").FieldValue, oLogger)
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Session is not set for picking.. Returning error to unit."))
                End If
                Me._responseCode = ResponseCode.Error
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("General error occured during picking operation. Error details: \r\n {0}", ex.ToString))
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        Return Nothing
    End Function

    'Begin RWMS-512

    Private Sub ValidateCaseWeight(ByVal strDeviceID As String, ByVal strPickListID As String, ByVal oLogger As WMS.Logic.LogHandler)
        Dim sql As String
        Dim user As String = WMS.Logic.GetCurrentUser

        Try
            oLogger.Write("Validating case weight for PickList " + strPickListID)
            sql = "SELECT * FROM PS_VOICEWEIGHTCAPTURE WHERE PICKLIST = '{0}'"
            sql = String.Format(sql, strPickListID)
            oLogger.Write("SQL to retrieve the captured case weights....... " + sql)
            Dim dtCaseWeights As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dtCaseWeights)
            If dtCaseWeights.Rows.Count > 0 Then
                sql = "INSERT INTO LOADDETWEIGHT(LOADID, UOM, UOMNUM, UOMWEIGHT, ADDDATE, ADDUSER) "
                Dim dr As DataRow
                Dim i As Integer = 0
                For Each dr In dtCaseWeights.Rows

                    Dim sql1 As String = vbCrLf + " SELECT (select TOLOAD from vPartialPickComplete WHERE PICKLIST = '{0}' AND PICKLISTLINE = {1}),'{2}',{3},{4},GETDATE(),'{5}'"
                    sql1 = String.Format(sql1, dr.Item("PICKLIST"), dr.Item("PICKLISTLINE"), dr.Item("UOM"), dr.Item("UOMNUM"), dr.Item("UOMWEIGHT"), user)
                    If i + 1 < (dtCaseWeights.Rows.Count) Then
                        sql1 = sql1 + vbCrLf + " UNION ALL "
                    End If
                    sql = sql + sql1
                    i = i + 1
                Next

                oLogger.Write("SQL for inserting the loadid in LOADDETWEIGHT............ " + sql)
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

                oLogger.Write("Proceeding to delete the records from PS_VOICEWEIGHTCAPTURE....... ")

                sql = "DELETE FROM PS_VOICEWEIGHTCAPTURE WHERE PICKLIST = '{0}'"
                sql = String.Format(sql, strPickListID)
                oLogger.Write("SQL to clear voice capture details for device....... " + sql)
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                oLogger.Write("Records deleted for device....... " + strDeviceID)

            Else
                oLogger.Write("Case weights not found for picklist " + strPickListID)
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Failed to update the case weights... " + ex.ToString())
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
        End Try

    End Sub

    'End RWMS-512

    Private Function PickPicklist(ByVal strDeviceID As String, ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Dim userId As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        Dim pcklst As Picklist = Session("PCKPicklist")
        If Not oLogger Is Nothing Then
            oLogger.Write("Picklist object retrieved from session, pick list id: " & pcklst.PicklistID)
        End If
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        If pck Is Nothing Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Pick Object is not set, exiting function...")
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End If
        Try
            'Dim sku As New SKU(pck.consingee, pck.sku)
            Dim sku As SKU
            If pck.oSku Is Nothing Then
                sku = New SKU(pck.consingee, pck.sku)
                pck.oSku = sku
            Else
                sku = pck.oSku
            End If
            pck.pickedqty = sku.ConvertToUnits(_msg(0)("Picked UOM").FieldValue) * Convert.ToDecimal(_msg(0)("Picked Quantity").FieldValue)
            If Not oLogger Is Nothing Then
                oLogger.Write("Setting pick object properties:")
                oLogger.Write("Pick object received UOM: " & _msg(0)("Picked UOM").FieldValue)
                oLogger.Write("Pick object received qty: " & _msg(0)("Picked Quantity").FieldValue)
                oLogger.Write("Pick object qty (after calculation): " & pck.pickedqty)
                'RWMS-94
                oLogger.Write("PickDetail SKU:" & pck.sku.ToString())
                'RWMS-94
            End If
            sku = Nothing
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Failed to extract and set pick quantities...")
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        Dim sContainerid As String
        Try
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pcklst.PicklistID)

            'set the to container for the current pick line
            pck.container = GetContainerID(pcklst, oLogger) 'Session("PCKPicklistActiveContainerID")
            sContainerid = pck.container
            '' Attributes handling
            Dim oAtt As New AttributesCollection
            oAtt.Add("WEIGHT", _msg(0)("Picked Weight").FieldValue)
            pck.oAttributeCollection = oAtt
            ''---
            If pck.units > pck.pickedqty Then
                PickShort(oLogger, pck)
            End If
            ''---
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to pick line..")
            End If

            'Begin: 'RWMS-94------------------------------------

            'Voice error : 07/21
            For Each iLineNumber As Integer In pck.PickDetLines 'For Each pckdet In Lines
                oLogger.Write("pck.PickDetLines.Line number." & iLineNumber.ToString())
            Next

            Dim sql1 As String = String.Format("select PICKLIST,PICKLISTLINE,ORDERLINE,STATUS from pickdetail where PICKLIST='{0}' and PICKLISTLINE ={1} AND STATUS='{2}'", pck.picklist, pck.PickDetLines(0).ToString(), WMS.Lib.Statuses.Picklist.COMPLETE.ToString())
            oLogger.Write("COMPLETE STATUS CHECK QUERY:" & sql1)

            Dim dtPickLineDetail As New DataTable
            DataInterface.FillDataset(sql1, dtPickLineDetail)
            oLogger.Write("COMPLETE STATUS CHECK ROW COUNT:" & dtPickLineDetail.Rows.Count.ToString())

            If dtPickLineDetail.Rows.Count > 0 Then
                oLogger.Write("dtPickLineDetail.Rows.Count :" & dtPickLineDetail.Rows.Count.ToString() & ", already COMPLETE and no picking needed, skip the picking and return the response")
            Else
                'RWMS-512
                Dim strPickListID As String = pck.picklist
                'RWMS-512

                'pck = PickTask.Pick(pck.fromlocation, pck.fromwarehousearea, pcklst, pck, userId, True, Nothing, False)
                pck = PickTask.Pick(pcklst, pck, userId, oLogger, False, Nothing, False)

                'RWMS-512
                oLogger.Write("Proceeding to validate case weights in method PickPicklist")
                ValidateCaseWeight(strDeviceID, strPickListID, oLogger)
                'RWMS-512
            End If

            'End: 'RWMS-94-------------------------------------------


        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while picking: " & ex.Description)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while picking: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        Me._responseCode = ResponseCode.Picked
        'Me.FillRecordsFromView(String.Format("picklistid='{0}'", pcklst.PicklistID), oLogger)
        Me.FillSingleRecord(oLogger)
        _resp(0)("ContainerID").FieldValue = sContainerid 'Session("PCKPicklistActiveContainerID")
        Return _resp
    End Function

    Private Function PickParallelPicklist(ByVal strDeviceID As String, ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Dim userId As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        Dim sContainerid As String
        Dim strPicklistID As String
        Dim pck As PickJob
        Dim pcks As ParallelPicking = Session("PARPCKPicklist")
        Session("PARPCKPicklist") = pcks
        If Not oLogger Is Nothing Then
            oLogger.Write("Picklist object retrieved from session, pick list id: " & pcks.ParallelPickId)
        End If
        pck = Session("PARPCKPicklistPickJob")
        If pck Is Nothing Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Pick Object is not set, exiting function...")
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End If
        Try
            Dim sku As New SKU(pck.consingee, pck.sku)
            pck.pickedqty = sku.ConvertToUnits(_msg(0)("Picked UOM").FieldValue) * Convert.ToDecimal(_msg(0)("Picked Quantity").FieldValue)
            If Not oLogger Is Nothing Then
                oLogger.Write("Setting pick object properties:")
                oLogger.Write("Pick object Received UOM: " & _msg(0)("Picked UOM").FieldValue)
                oLogger.Write("Pick object Received Qty: " & _msg(0)("Picked Quantity").FieldValue)
                oLogger.Write("Pick object Finalized Qty: " & pck.pickedqty)
            End If
            sku = Nothing
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Failed to extract and set pick quantities...")
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        Try
            ''---
            If pck.units > pck.pickedqty Then
                PickShort(oLogger, pck)
            End If
            ''---
            Dim pcklsts As ParallelPicking = Session("PARPCKPicklist")
            'set the to container for the current pick line
            pck.container = GetContainerID(pck.picklist, oLogger)
            pck.oncontainer = pcklsts.ToContainer
            sContainerid = pck.container
            strPicklistID = pck.picklist
            'pck = pcks.Pick(pck.fromlocation, pck.fromwarehousearea, pck, userId)
            pck = pcks.Pick(pck, userId, oLogger, True, False, False)

            'RWMS-512
            oLogger.Write("Proceeding to validate case weights in method PickParallelPicklist")
            ValidateCaseWeight(strDeviceID, strPicklistID, oLogger)
            'RWMS-512

        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while picking: " & ex.Description)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while picking: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        Me._responseCode = ResponseCode.Picked
        'Me.FillRecordsFromView(String.Format("picklistid='{0}'", strPicklistID), oLogger)
        Me.FillSingleRecord(oLogger)
        _resp(0)("ContainerID").FieldValue = sContainerid 'Session("PCKPicklistActiveContainerID")
        Return _resp
    End Function

    Private Function GetContainerID(ByVal sPicklist As String, ByVal oLogger As WMS.Logic.LogHandler) As String
        Dim sContainer As String = ""
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to set the active container for picking...")
        End If
        If _msg(0)("Container").FieldValue <> String.Empty Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Container id received in pick message: {0}. Setting id for current pick job.", _msg(0)("Container").FieldValue))
                Return _msg(0)("Container").FieldValue
            End If
        End If
        Try
            Dim pcklist As Picklist = New WMS.Logic.Picklist(sPicklist)
            sContainer = GetContainerID(pcklist, oLogger)
        Catch ex As Exception
        End Try
        Return sContainer
    End Function

    Private Function GetContainerID(ByVal oPicklist As WMS.Logic.Picklist, ByVal oLogger As WMS.Logic.LogHandler) As String
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to set the active container for picking...")
        End If
        If _msg(0)("Container").FieldValue <> String.Empty Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Container id received in pick message: {0}. Setting id for current pick job.", _msg(0)("Container").FieldValue))
                Return _msg(0)("Container").FieldValue
            End If
        End If
        Dim pcklist As Picklist
        If oPicklist Is Nothing Then
            pcklist = Session("PCKPicklist")
        Else
            pcklist = oPicklist
        End If
        Dim contid As String = ""
        If Not pcklist Is Nothing Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to get the active container from the current picklist...")
            End If
            If Session("PCKPicklistActiveContainerID") Is Nothing Or Session("PCKPicklistActiveContainerID") <> pcklist.ActiveContainer Then
                If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    If pcklist.ActiveContainer <> String.Empty Then
                        contid = pcklist.ActiveContainer
                    Else
                        contid = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                    End If
                    Session("PCKPicklistActiveContainerID") = contid
                End If
            ElseIf pcklist.ActiveContainer <> String.Empty Then
                contid = pcklist.ActiveContainer
                Session("PCKPicklistActiveContainerID") = contid
            End If
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Container {0} was set for picking.", contid))
        End If
        Return contid
    End Function


    Private Sub PickShort(ByVal oLogger As WMS.Logic.LogHandler, ByVal pckJob As WMS.Logic.PickJob)
        'Create a count job
        Dim shouldCreateCountJob As String = WMS.Logic.WarehouseParams.GetWarehouseParam("CrtCntJobPckShrt")
        If shouldCreateCountJob <> String.Empty AndAlso shouldCreateCountJob = "1" Then
            Dim C As New WMS.Logic.Counting(WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", False)
            C.CreateLocationCountJobs(pckJob.fromwarehousearea, "", pckJob.fromlocation, WMS.Lib.TASKTYPE.LOCATIONCOUNTING, "", "", WMS.Logic.GetCurrentUser)
            oLogger.Write("Location counting task created, COUNTID = " & C.COUNTID)
            Try
                Dim sql As String = String.Format("update tasks set PRIORITY=400 where TASKTYPE='LOCCNT' and COUNTID = '{0}' and FROMLOCATION='{1}' and FROMWAREHOUSEAREA='{2}'", C.COUNTID, C.LOCATION, C.WAREHOUSEAREA)
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
        End If

        'Send message for pick short
        sendMessageQ(pckJob)
        oLogger.Write("Message SHORTPICK was send")

        'and a manual replenishment
        SendManualReplenishMessage(pckJob.fromlocation, pckJob.fromwarehousearea, pckJob.consingee, pckJob.sku, "")
    End Sub

    Private Sub SendManualReplenishMessage(ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pConsignee As String, ByVal pSku As String, ByVal pPickRegion As String, Optional ByVal pReplenishAll As Boolean = False, Optional ByVal pMinQtyOverride As Decimal = -1, Optional ByVal pTaskTimeLimit As Decimal = -1)
        Dim em As New EventManagerQ
        Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ManualReplenishment
        em.Add("EVENT", EventType)
        em.Add("REPLMETHOD", Replenishment.ReplenishmentMethods.ManualReplenishment)
        em.Add("LOCATION", pLocation)
        em.Add("WAREHOUSEAREA", pWarehousearea)
        em.Add("PICKREGION", pPickRegion)
        If Not pReplenishAll Then
            em.Add("ALL", "0")
        Else
            em.Add("ALL", "1")
        End If
        em.Add("SKU", pSku)
        em.Add("CONSIGNEE", pConsignee)
        em.Add("MINQTYOVERRIDE", pMinQtyOverride)
        em.Add("TASKSTIMELIMIT", pTaskTimeLimit)
        em.Send(WMSEvents.EventDescription(EventType))
    End Sub


    Private Sub sendMessageQ(ByVal pck As WMS.Logic.PickJob)

        'Commented for PWMS-756 and made it generic
        'Dim MSG As String = "SHORTPICK"

        Dim aq As EventManagerQ = New EventManagerQ
        aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.SHORTPICK)
        aq.Add("ACTIVITYTYPE", WMS.Lib.TASKTYPE.SHORTPICK)
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")

        aq.Add("USERID", WMS.Logic.GetCurrentUser)

        aq.Add("DOCUMENT", pck.picklist)
        Dim strLines As String = "1"
        Try
            strLines = pck.PickDetLines(0)
        Catch ex As Exception
        End Try
        aq.Add("DOCUMENTLINE", strLines)
        aq.Add("CONSIGNEE", pck.consingee)
        aq.Add("SKU", pck.sku)
        aq.Add("FROMLOAD", pck.fromload)
        'aq.Add("TOLOAD", "") '??
        Dim L As New WMS.Logic.Load(pck.fromload)
        aq.Add("FROMSTATUS", L.STATUS) '??
        aq.Add("FROMLOC", pck.fromlocation)
        aq.Add("FROMQTY", pck.units)
        aq.Add("TOQTY", pck.pickedqty)
        aq.Add("MHEID", Session("MHEID"))
        aq.Add("MHType", Session("MHType"))
        aq.Add("FROMCONTAINER", pck.container)
        aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
        'aq.Add("TOLOC", "")
        'aq.Add("TOSTATUS", Session("CreateLoadStatus"))
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)

        'PWMS-756 and made it generic  for shortpick
        aq.Send(WMS.Lib.TASKTYPE.SHORTPICK)


    End Sub

End Class