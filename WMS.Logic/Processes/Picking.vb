'Imports Made4Net.DataAccess
Imports System.Data.Linq
Imports System.Collections.Generic
<CLSCompliant(False)> Public Class Picking

    Public Shared BagOutProcess As Boolean
    'Public Shared BagOutContainerClosed As Boolean = True
    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim pk As String
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "pick"
                'RWMS-2863 
                For Each pk In Picking.DistinctPicks(ds.Tables(0))
                    Dim oPickList As New Picklist(pk)
                    oPickList.Pick(UserId)
                    'Pick(oPickList, UserId)
                    'Start PWMS-836 and RWMS-898   
                    Dim opickdetails As PicklistDetailCollection = New PicklistDetailCollection(oPickList.PicklistID)
                    For i As Integer = 1 To opickdetails.Count
                        Dim oPickListDetail As PicklistDetail = oPickList.Lines.PicklistLine(i)
                        Dim objStaging As New Staging()
                        Dim oDelTask As New WMS.Logic.DeliveryTask()
                        If oPickList.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                            oDelTask = objStaging.StageLoad(oPickListDetail.FromLoad, UserId)
                        Else
                            'RWMS-2857
                            If Not oPickListDetail.ToContainer Is Nothing And oPickListDetail.PickedQuantity <> 0 Then
                                oDelTask = objStaging.StageContainer(oPickListDetail.ToContainer, UserId)
                            End If
                            'RWMS-2857
                        End If
                        'RWMS-2857
                        If Not oDelTask Is Nothing Then
                            If Not oDelTask.TASK Is Nothing And Not String.IsNullOrEmpty(oDelTask.TASK) Then
                                'RWMS-2863 
                                oDelTask.Deliver(toLocation:=oPickListDetail.ToLocation, toWarehousearea:=oPickListDetail.ToWarehousearea, pickList:=oPickList.PicklistID)
                            End If
                        End If
                        'RWMS-2857
                    Next
                    'End PWMS-836 and RWMS-898   

                Next
            Case "approvepicks"
                For Each dr In ds.Tables(0).Rows
                    If IsDBNull(dr("UNITS")) Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Units field cannot be blank", "Units field cannot be blank")
                    End If

                    Dim oPickList As Picklist = New Picklist(dr("PICKLIST"))
                    Dim oAttributeCollection As AttributesCollection
                    Dim oSku As New SKU(oPickList(dr("PICKLISTLINE")).Consignee, oPickList(dr("PICKLISTLINE")).SKU)
                    oAttributeCollection = SkuClass.ExtractPickingAttributes(oSku.SKUClass, dr)
                    'Commented for RWMS-2780
                    'PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection, WMS.Logic.UserPickShort.PickPartialCreateException, dr("ToContainerInp"))
                    'Commented for RWMS-2780 END
                    'RWMS-2780
                    PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection)
                    'RWMS-2780 END
                    'RWMS-2807
                    Dim oPickListDetail As PicklistDetail = oPickList.Lines.PicklistLine(dr("PICKLISTLINE"))
                    Dim objStaging As New Staging()
                    Dim oDelTask As New WMS.Logic.DeliveryTask()
                    If oPickList.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                        oDelTask = objStaging.StageLoad(oPickListDetail.FromLoad, UserId)
                    Else
                        oDelTask = objStaging.StageContainer(oPickListDetail.ToContainer, UserId)
                    End If
                    oDelTask.Deliver(oPickListDetail.ToLocation, oPickListDetail.ToWarehousearea)
                    'RWMS-2807 END
                Next
            Case "pickshortcancelexceptions"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As Picklist = New Picklist(dr("PICKLIST"))
                    Dim oAttributeCollection As AttributesCollection
                    Dim oSku As New SKU(oPickList(dr("PICKLISTLINE")).Consignee, oPickList(dr("PICKLISTLINE")).SKU)
                    oAttributeCollection = SkuClass.ExtractPickingAttributes(oSku.SKUClass, dr)
                    PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection, WMS.Logic.UserPickShort.PickPartialCancelException, dr("ToContainerInp"))
                Next
            Case "pickshortleaveopen"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As Picklist = New Picklist(dr("PICKLIST"))
                    Dim oAttributeCollection As AttributesCollection
                    Dim oSku As New SKU(oPickList(dr("PICKLISTLINE")).Consignee, oPickList(dr("PICKLISTLINE")).SKU)
                    oAttributeCollection = SkuClass.ExtractPickingAttributes(oSku.SKUClass, dr)
                    PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection, WMS.Logic.UserPickShort.PickPartialLeaveOpen, dr("ToContainerInp"))
                Next
            Case "allocateloadforpickdetail"
                'Dim oPicklist As New Picklist(dr("PICKLIST")) 
                Dim oPicklist As New Picklist(ds.Tables(0).Rows(0)("PICKLIST"))
                Dim oRelStrat As WMS.Logic.ReleaseStrategyDetail = oPicklist.getReleaseStrategy
                For Each dr In ds.Tables(0).Rows
                    Dim oPickListDetail As PicklistDetail = oPicklist.Lines.PicklistLine(Convert.ToInt32(dr("PICKLISTLINE"))) 'New PicklistDetail(dr("PICKLIST"), Convert.ToInt32(dr("PICKLISTLINE")))
                    oPickListDetail.AllocateLoadFromPickLoc(oRelStrat)
                Next
        End Select
    End Sub

#Region "DistinctPicks"
    'RWMS-2863,2762 ,2822 
    Public Shared Function DistinctPicks(picklistTable As DataTable) As IEnumerable(Of String)
        Return picklistTable.AsEnumerable().Select(Function(dr) dr("PICKLIST").ToString()).Distinct()
    End Function
#End Region

    Public Sub Pick(ByVal oPickList As Picklist, ByVal sUserId As String)
        For Each oPickListLine As PicklistDetail In oPickList.Lines
            If oPickListLine.Status = WMS.Lib.Statuses.Picklist.PLANNED Or oPickListLine.Status = WMS.Lib.Statuses.Picklist.RELEASED Then
                'ValidateLine(oPickList, oPickListLine.PickListLine, oPickListLine.AdjustedQuantity - oPickListLine.PickedQuantity, sUserId, Nothing)
                ValidateLine(oPickListLine.Consignee, oPickListLine.SKU, oPickListLine.AdjustedQuantity - oPickListLine.PickedQuantity, sUserId, Nothing)
            End If
        Next
        oPickList.Pick(sUserId)
    End Sub

    'Public Shared Sub ValidateLine(ByVal oPickList As Picklist, ByVal iPickListLine As Int32, ByVal iUnits As Double, ByVal sUserId As String, ByVal oAttributesCollection As AttributesCollection)
    Public Shared Sub ValidateLine(ByVal pConsignee As String, ByVal pSKU As String, ByVal iUnits As Double, ByVal sUserId As String, ByVal oAttributesCollection As AttributesCollection, Optional ByVal oSKU As WMS.Logic.SKU = Nothing)
        If Not (Math.Round(iUnits) = iUnits) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "only full units can be confirmed.", "only full units can be confirmed.")

        End If
        'Attribute Validation'
        Dim Skuobj As SKU
        If oSKU Is Nothing Then
            Skuobj = New SKU(pConsignee, pSKU)
        Else
            Skuobj = oSKU
        End If
        Dim oSkuClass As SkuClass = Skuobj.SKUClass  'oSKU.SKUClass

        If Not oSkuClass Is Nothing Then
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                If oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oLoadAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                        If oAttributesCollection Is Nothing Then Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                        'If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                        If String.IsNullOrEmpty(oAttributesCollection(oLoadAtt.Name)) OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                            Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                        End If
                    End If

                    ' Validator
                    If Not oLoadAtt.PickingValidator Is Nothing Then
                        'If Not oLoadAtt.PickingValidator.Validate(oPickList, iPickListLine, iUnits, sUserId, oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name), oAttributesCollection) Then
                        '    Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        'End If
                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                        vals.Add(oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name))
                        Dim ret As String = oLoadAtt.Evaluate(SkuClassLoadAttribute.EvaluationType.Picking, vals)

                        If ret = "-1" Then
                            Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                    End If
                End If
            Next
        End If
        'End Of Attribute Validation
    End Sub

    Public Sub PickLine(ByVal oPickList As Picklist, ByVal iPickListLine As Int32, ByVal iUnits As Double, ByVal sUserId As String, ByVal oAttributesCollection As AttributesCollection, Optional ByVal pUserPickShortType As String = "", Optional ByVal pContainerID As String = "")

        'ValidateLine(oPickList, iPickListLine, iUnits, sUserId, oAttributesCollection)
        ValidateLine(oPickList(iPickListLine).Consignee, oPickList(iPickListLine).SKU, iUnits, sUserId, oAttributesCollection)

        If oPickList(iPickListLine).Status = WMS.Lib.Statuses.Picklist.CANCELED OrElse _
        oPickList(iPickListLine).Status = WMS.Lib.Statuses.Picklist.COMPLETE Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Incorrect line status", "Incorrect line status")
        End If
        oPickList.PickLine(iPickListLine, iUnits, sUserId, oAttributesCollection, pUserPickShortType, pContainerID)
    End Sub

    Public Sub Pick(ByVal oPickList As Picklist, ByVal oPck As PickJob, ByVal sUserID As String, ByVal logger As LogHandler)
        Dim unitspicked As Double = oPck.pickedqty
        Dim pckdet As PicklistDetail
        For Each pckdet In oPickList.Lines
            If pckdet.UOM = oPck.originaluom And pckdet.Status <> WMS.Lib.Statuses.Picklist.COMPLETE And pckdet.Status <> WMS.Lib.Statuses.Picklist.CANCELED Then
                If pckdet.AdjustedQuantity - pckdet.PickedQuantity < unitspicked Then
                    ValidateLine(pckdet.Consignee, pckdet.SKU, pckdet.AdjustedQuantity - pckdet.PickedQuantity, sUserID, oPck.oAttributeCollection, oPck.oSku)
                    unitspicked = unitspicked - (pckdet.AdjustedQuantity - pckdet.PickedQuantity)
                Else
                    ValidateLine(pckdet.Consignee, pckdet.SKU, unitspicked, sUserID, oPck.oAttributeCollection, oPck.oSku)
                End If
            End If
        Next
        oPickList.Pick(oPck, sUserID, logger)
        If oPickList.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK Then
            PutAwayLoad(oPickList, oPck, sUserID)
        End If
    End Sub

    Private Sub PutAwayLoad(ByVal oPickList As Picklist, ByVal oPck As PickJob, ByVal sUserID As String)
        Dim oLoad As WMS.Logic.Load
        Try
            oLoad = New WMS.Logic.Load(oPck.fromload)
            If oLoad.UNITS > 0 Then
                Dim pw As New Putaway
                pw.RequestDestinationForLoad(oLoad.LOADID, oLoad.DESTINATIONLOCATION, oLoad.DESTINATIONWAREHOUSEAREA, 0, "", False, True)
            End If
        Catch ex As Exception
        End Try
    End Sub

End Class
