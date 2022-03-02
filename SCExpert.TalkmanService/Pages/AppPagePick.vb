Imports WMS.Logic
Imports Made4Net.Shared.Web

Public Class AppPagePick
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Picked
        [Error]
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to pick...")
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not Session("PCKPicklist") Is Nothing Then
            Return PickPicklist(oLogger)
        ElseIf Not Session("PARPCKPicklist") Is Nothing Then
            Return PickParallelPicklist(oLogger)
        End If
    End Function

    Private Function PickPicklist(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
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
            Dim tm As New WMS.Logic.TaskManager(pcklst.PicklistID, True)
            pck.container = Session("PCKPicklistActiveContainerID")
            If Session("CONFTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                pck = PickTask.Pick(pck.fromlocation, pcklst, pck, userId, True, pck.sku)
            Else
                pck = PickTask.Pick(pck.fromlocation, pcklst, pck, userId)
            End If
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
        Me.FillRecordsFromView(String.Format("picklistid='{0}'", pcklst.PicklistID), oLogger)
        Return _resp
    End Function

    Private Function PickParallelPicklist(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Dim userId As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
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
            Dim pcklsts As ParallelPicking = Session("PARPCKPicklist")
            pck.container = Session("PCKPicklistActiveContainerID")
            pck.oncontainer = pcklsts.ToContainer
            strPicklistID = pck.picklist
            pck = pcks.Pick(pck.fromlocation, pck, userId)
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
        Me.FillRecordsFromView(String.Format("picklistid='{0}'", strPicklistID), oLogger)
        Return _resp
    End Function

End Class
