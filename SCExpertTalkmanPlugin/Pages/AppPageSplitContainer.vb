Imports WMS.Logic
Imports WMS.Lib

Public Class AppPageSplitContainer
    Inherits AppPageProcessor
    Private Enum ResponseCode
        Closed
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub
    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        PrintMessageContent(oLogger)
        Dim plDetSplit As WMS.Logic.PicklistDetail
        Dim sErrormsg As String = String.Empty
        If Not ValidateUserLoggedIn() Then
            If Not oLogger Is Nothing Then
                sErrormsg = String.Format("User is not logged in. Please sign in again.")
                oLogger.Write(String.Format(sErrormsg))
            End If
            Me._responseCode = ResponseCode.NoUserLoggedIn
            Me._responseText = "User is not logged in"
            Me.FillSingleRecord(oLogger)
            Return _resp
        End If

        Try

            If Not oLogger Is Nothing Then
                oLogger.Write("Processing Pick Short Split Quantity Request...")
                oLogger.Write("Picklist  : " & _msg(0)("Picklist").FieldValue)
                oLogger.Write("Picklist line  : " & _msg(0)("Picklist line").FieldValue)
            End If

            plDetSplit = SplitPicklistDetail(oLogger)

        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                sErrormsg = String.Format("Error Occured while splitting the pick detail: {0}", ex.Description)
                oLogger.Write(sErrormsg)

            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                sErrormsg = String.Format("Error Occured while splitting the pick detail: {0}", ex.ToString)
                oLogger.Write(sErrormsg)
                Me._responseCode = ResponseCode.Error
                Me._responseText = "Error Occured while splitting the pick detail"

                Me.FillSingleRecord(oLogger)
            End If
           
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Pick Short Split Quantity Process completed successfully.")
            oLogger.Write(String.Format("Current Picked Units  : {0} ", plDetSplit.Quantity))
            oLogger.Write(String.Format("Total Picked Units  : {0} ", Session("TotalPickedUnits")))
            oLogger.Write(String.Format("Container used  for current pick  : {0} ", _msg(0)("Container").FieldValue))
            Session("TotalPickedUnits") = Nothing
            Me._responseCode = ResponseCode.Closed
            Me.FillSingleRecord(oLogger)
        End If
        'Me._responseCode = ResponseCode.Closed
        'Me.FillSingleRecord(oLogger)





        Return _resp
    End Function
    Private Function SplitPicklistDetail(ByVal oLogger As WMS.Logic.LogHandler) As WMS.Logic.PicklistDetail
        Dim sErrormsg As String

        Dim userId As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        ' Dim pck As PickJob = Session("PCKPicklistPickJob")
        ' Dim dictionaryContainerIdAndPickedQty As Dictionary(Of String, Decimal)
        '  If Not Session("MultiContaierPicking") = True Then
        ' Session("DictionaryContainerIdAndPickedQty") = Nothing 'clearing the session in oreder to insert the new splited line containers data
        ' dictionaryContainerIdAndPickedQty = New Dictionary(Of String, Decimal)
        Session("MultiContainerPicking") = True
        ' Else
        '  dictionaryContainerIdAndPickedQty = Session("DictionaryContainerIdAndPickedQty")
        ' End If



        Dim plSplitDet As WMS.Logic.PicklistDetail
        Dim plDet As WMS.Logic.PicklistDetail

        Dim pck As PickJob = Session("PCKPicklistPickJob")
        Dim qty As Decimal
        Dim PickListId As String
        Dim PickListLineid As Int32
        PickListId = _msg(0)("PickList").FieldValue
        Int32.TryParse(_msg(0)("Picklist Line").FieldValue, PickListLineid)
        plDet = New WMS.Logic.PicklistDetail(PickListId, PickListLineid, True)
        'Session("SplittedPickDetailLine") = plDet
        'Session("TotalQtyToPick") = plDet.Quantity
        Dim oSku As New SKU(plDet.Consignee, plDet.SKU)

        If (String.Equals(oSku.LOWESTUOM, _msg(0)("Picked UOM").FieldValue, StringComparison.OrdinalIgnoreCase)) Then
            qty = _msg(0)("Picked Quantity").FieldValue
        Else
            qty = oSku.ConvertToUnits(_msg(0)("Picked UOM").FieldValue) * _msg(0)("Picked Quantity").FieldValue
        End If

        If Session("TotalPickedUnits") Is Nothing Then
            Session("TotalPickedUnits") = qty
            oLogger.Write("START PICKING :")
        Else
            Session("TotalPickedUnits") += qty
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Current Picked Units  : {0} ", qty))
            oLogger.Write(String.Format("Total Picked Units  : {0} ", Session("TotalPickedUnits")))
            oLogger.Write(String.Format("Container used  for current pick  : {0} ", _msg(0)("Container").FieldValue))
        End If
        If plDet.AdjustedQuantity > qty And qty > 0 Then
            plSplitDet = plDet.SplitLine(qty)
            plSplitDet.FromLoad = plDet.FromLoad
            plDet.AdjustedQuantity = plDet.AdjustedQuantity - qty
            If plDet.AdjustedQuantity = 0 Then ' all of the line units were picked/splitted to other new lines (no qty left to pick)
                Session("MultiContainerPicking") = False ' end of MultiContainerPicking state (for the current picklist line)
                '  Session("TotalPickedUnits") = Nothing
                plDet.Status = WMS.Lib.Statuses.Picklist.COMPLETE

            End If
            plDet.Save(WMS.Logic.GetCurrentUser())

        Else
            If qty > 0 Then
                sErrormsg = String.Format("Cannot split adjusted quantity is not grater than splitted quantity ")
            Else

                sErrormsg = String.Format("Cannot split Splitted quantity must be grater than 0")
            End If

            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, sErrormsg, sErrormsg)
            Throw m4nEx

        End If

        ' set the container for the splitted qty of  pick detail
        plSplitDet.SetContainer(_msg(0)("Container").FieldValue, WMS.Logic.GetCurrentUser)

        '   plSplitDet.Post(plSplitDet.PickListLine)
        'plSplitDet.Save(WMS.Logic.GetCurrentUser())
        '' Attributes handling
        Dim oAtt As New AttributesCollection
        oAtt.Add("WEIGHT", _msg(0)("Picked Weight").FieldValue)
        pck.oAttributeCollection = oAtt
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to pick line..")
        End If
        'check if the is a 
        '----
        If Not Container.Exists(_msg(0)("Container").FieldValue) Then
            Dim cnt As New Container
            cnt.ContainerId = _msg(0)("Container").FieldValue
            Dim currentPickList As New Picklist(PickListId, True)

            cnt.HandlingUnitType = currentPickList.HandelingUnitType '_handelingunittype
            cnt.UsageType = Container.ContainerUsageType.PickingContainer
            cnt.Location = pck.fromlocation
            cnt.Warehousearea = pck.fromwarehousearea
            cnt.Post(WMS.Logic.GetCurrentUser)
            cnt = Nothing
            currentPickList = Nothing
        End If
        '---
        plSplitDet.Save(WMS.Logic.GetCurrentUser())
        plSplitDet.Pick(_msg(0)("Picked Quantity").FieldValue, _msg(0)("Picked UOM").FieldValue, userId, pck.oAttributeCollection)

        ' Return New WMS.Logic.PicklistDetail(plSplitDet.PickList, plSplitDet.PickListLine)
        Return plSplitDet

    End Function
 
End Class
