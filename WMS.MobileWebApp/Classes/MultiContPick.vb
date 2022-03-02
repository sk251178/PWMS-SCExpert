Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Made4Net.DataAccess
Imports System.Data
Imports WMS.Logic
Imports Made4Net.Shared

Public Class MultiContPick

    Public Class ContData

        Private _containerId As String
        Private _verifiedQty As Decimal
        Private _verifiedWeight As Decimal
        '  Private _attr As WMS.Logic.AttributesCollection

        Public Sub New(ByVal pContainerId As String, ByVal pVerifiedQty As Decimal, Optional ByVal pVerifiedWeight As Decimal = 0)
            _containerId = pContainerId
            _verifiedQty = pVerifiedQty
            _verifiedWeight = pVerifiedWeight
        End Sub
        Public Sub New(ByVal pContainerId As String)
            _containerId = pContainerId
            _verifiedQty = 0
            _verifiedWeight = 0
        End Sub

        Public Property ContainerId() As String
            Get
                Return _containerId
            End Get
            Set(ByVal value As String)
                _containerId = value
            End Set
        End Property

        Public Property VerifiedQty() As Decimal
            Get
                Return _verifiedQty
            End Get
            Set(ByVal value As Decimal)
                _verifiedQty = value
            End Set
        End Property

        Public Property VerifiedWeight() As Decimal
            Get
                Return _verifiedWeight
            End Get
            Set(ByVal value As Decimal)
                _verifiedWeight = value
            End Set
        End Property

        'Public Property Attr() As WMS.Logic.AttributesCollection
        '    Get
        '        Return _attr
        '    End Get
        '    Set(ByVal value As WMS.Logic.AttributesCollection)
        '        _attr = value
        '    End Set
        'End Property

        'Public Sub AddCont(ByVal pHandlingUnit As String, ByVal pConsigneeSKU As String, ByVal pQty As Decimal)

        'End Sub

    End Class


    Public dictPickedCont As New Dictionary(Of String, MultiContPick.ContData)
    Public dictCont As New ArrayList
    Public VerifiedWeightLastPick As Decimal = 0

    Public PickedContainerCount As Integer = 0
    Public TotalPickedContainerUnits As Integer = 0
    Public TotalPickedContainerWeight As Integer = 0

    Public Sub AddContainers(ByVal Containers As ArrayList)
        For Each Container As WMS.Logic.Container In Containers
            If WMS.Logic.Container.Exists(Container.ContainerId) Then

            Dim oCont As New WMS.Logic.Container(Container.ContainerId, True)
                If oCont.Status = WMS.Lib.Statuses.Container.STATUSNEW Then
                    If Not dictCont.Contains(Container.ContainerId) Then
                        dictCont.Add(Container.ContainerId)
                    End If
                End If
            End If

        Next
    End Sub


    Public Sub AddContainer(ByVal ContainerID As String)
        If Not dictCont.Contains(ContainerID) Then
            dictCont.Add(ContainerID)
        End If
    End Sub

    'Public Sub AddPickedContainers(ByVal Containers As ArrayList)
    '    For Each ContainerID As String In Containers
    '        If Not dictPickedCont.ContainsKey(ContainerID) Then
    '            dictPickedCont.Add(ContainerID, New MultiContPick.ContData(ContainerID))
    '        End If
    '    Next
    'End Sub


    Public Sub AddPickedContainer(ByVal ContainerID As String, Optional ByVal VerifiedQty As Decimal = 0, Optional ByVal VerifiedWeight As Decimal = 0)
        If Not dictPickedCont.ContainsKey(ContainerID) Then
            dictPickedCont.Add(ContainerID, New MultiContPick.ContData(ContainerID, VerifiedQty, VerifiedWeight))
            PickedContainerCount += 1
            TotalPickedContainerUnits += VerifiedQty
            TotalPickedContainerWeight += VerifiedWeight
        Else
            EditPickedContainer(ContainerID, VerifiedQty, VerifiedWeight)
        End If
    End Sub

    Public Sub EditPickedContainer(ByVal ContainerID As String, Optional ByVal VerifiedQty As Decimal = 0, Optional ByVal VerifiedWeight As Decimal = 0)
        For Each pair As KeyValuePair(Of String, MultiContPick.ContData) In dictPickedCont
            If pair.Key = ContainerID Then
                If pair.Value.VerifiedQty = 0 And VerifiedQty > 0 Then
                    pair.Value.VerifiedQty = VerifiedQty
                    TotalPickedContainerUnits += VerifiedQty
                End If
                If pair.Value.VerifiedWeight = 0 And VerifiedWeight > 0 Then
                    pair.Value.VerifiedWeight = VerifiedWeight
                    TotalPickedContainerWeight += VerifiedWeight
                End If
            End If
        Next
    End Sub

    Public Sub finishPartial(logger As LogHandler)

        Dim pckJob As WMS.Logic.PickJob = HttpContext.Current.Session("WeightNeededPickJob")

        Dim pcklst As New WMS.Logic.Picklist(pckJob.picklist)

        For Each pair As KeyValuePair(Of String, MultiContPick.ContData) In dictPickedCont

            Try
                Dim osku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)

                'do i need it
                'If weightNeeded(osku) Then
                '    pckJob.oAttributeCollection = ExtractAttributes()
                'End If
                Try
                    If Not IsNothing(pckJob.oAttributeCollection) Then pckJob.oAttributeCollection.Item("WEIGHT") = pair.Value.VerifiedWeight
                Catch ex As Exception

                End Try

                HttpContext.Current.Session("PCKPickLineSecond") = splitPick(pckJob, pair.Key, pair.Value.VerifiedQty)
                If Not IsNothing(HttpContext.Current.Session("PCKPickLineSecond")) Then
                    PickRemaiderUnits(pckJob, pair.Key, pair.Value.VerifiedQty)
                End If

            Catch ex As Made4Net.Shared.M4NException
                'MessageQue.Enqueue(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                'MessageQue.Enqueue(ex.Message)
                Return
            End Try

        Next

        'pick first container from pckpart.aspx screen, must be picked like a last pick,
        'becouse it close task, and move to next pick line
        Try
            If Not IsNothing(pckJob.oAttributeCollection) Then pckJob.oAttributeCollection.Item("WEIGHT") = VerifiedWeightLastPick
        Catch ex As Exception

        End Try

        If Not String.IsNullOrEmpty(HttpContext.Current.Session("WeightNeededConfirm2")) Then
            pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), HttpContext.Current.Session("WeightNeededConfirm2"), pckJob.fromwarehousearea)
            pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

            '            pckJob = PickTask.Pick(HttpContext.Current.Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser, True, HttpContext.Current.Session("WeightNeededConfirm2"))
        Else
            pckJob.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pckJob.TaskConfirmation.ConfirmationType, HttpContext.Current.Session("WeightNeededConfirm1"), "", pckJob.fromwarehousearea)
            pckJob = PickTask.Pick(pcklst, pckJob, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
            'pckJob = PickTask.Pick(HttpContext.Current.Session("WeightNeededConfirm1"), pckJob.fromwarehousearea, pcklst, pckJob, WMS.Logic.Common.GetCurrentUser)
        End If

        If IsNothing(pckJob) And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY, logger) Then
            HttpContext.Current.Session.Remove("PCKPicklist")
            Try
                HttpContext.Current.Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/DEL.aspx?sourcescreen=PCK"))

            Catch ex As System.Threading.ThreadAbortException

            End Try

        End If
    End Sub

    <CLSCompliant(False)>
    Public Sub PickRemaiderUnits(ByVal pckJob As WMS.Logic.PickJob, ByVal ContainerID As String, ByVal qty As Decimal)

        Dim picking As New WMS.Logic.Picking()
        Dim pckLineSecond As WMS.Logic.PicklistDetail = HttpContext.Current.Session("PCKPickLineSecond")
        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(pckLineSecond.PickList)
        'Dim sku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)

        picking.PickLine(oPickList, pckLineSecond.PickListLine, qty, _
        WMS.Logic.GetCurrentUser, pckJob.oAttributeCollection, WMS.Logic.UserPickShort.PickPartialCreateException, _
        ContainerID)

        HttpContext.Current.Session.Remove("PCKPickLineSecond")

    End Sub

    <CLSCompliant(False)>
    Public Function splitPick(ByVal pckJob As WMS.Logic.PickJob, ByVal ContainerID As String, ByVal qty As Decimal) As WMS.Logic.PicklistDetail
        Dim plDet As WMS.Logic.PicklistDetail


        'If pckJob.uomunits = HttpContext.Current.Session("PCKOldUomUnits") Then Return Nothing

        Dim plSplitDet As New WMS.Logic.PicklistDetail


        'Dim sumPickedQTY As Decimal = pckJob.uomunits + HttpContext.Current.Session("UOMUnits_2")

        'If IsNothing(HttpContext.Current.Session("PCKPickLineSecond")) Then
        '    If sumPickedQTY <= HttpContext.Current.Session("PCKOldUomUnits") And qty > 0 Then


        plDet = New WMS.Logic.PicklistDetail(pckJob.picklist, pckJob.PickDetLines(0))
        plSplitDet = plDet.SplitLine(qty)

        plSplitDet.SetContainer(ContainerID, WMS.Logic.GetCurrentUser)

        plSplitDet.Post(plSplitDet.PickListLine)

        Return New WMS.Logic.PicklistDetail(pckJob.picklist, plSplitDet.PickListLine)

        '    Else
        'Return Nothing
        '    End If
        'Else
        'Return HttpContext.Current.Session("PCKPickLineSecond")
        'End If
    End Function

End Class


Public Class MultiContManage

    Public Shared Function GetMultiContPick() As MultiContPick
        Dim oCont As MultiContPick
        If Not IsNothing(HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond")) Then
            oCont = HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond")
        Else
            oCont = New MultiContPick()
        End If
        Return oCont
    End Function

    Public Shared Sub AddContainers(ByVal Containers As ArrayList)
        Dim oCont As MultiContPick = GetMultiContPick()

        'loads all active container
        oCont.AddContainers(Containers)
        HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond") = oCont
    End Sub

    Public Shared Sub AddContainer(ByVal ContainerID As String)
        Dim oCont As MultiContPick = GetMultiContPick()
        If String.IsNullOrEmpty(ContainerID) Then Exit Sub

        oCont.AddContainer(ContainerID)
        HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond") = oCont
    End Sub

    Public Shared Function ContainerCount() As Integer
        Dim oCont As MultiContPick = GetMultiContPick()

        Return oCont.dictCont.Count
    End Function

    Public Shared Function GetSecondContainer() As String
        Dim oCont As MultiContPick = GetMultiContPick()

        For Each container As String In oCont.dictCont
            If container <> HttpContext.Current.Session("PCKPicklistActiveContainerID") Then
                Return container
            End If
        Next

        Return ""
    End Function

    Public Shared Function ContainerContainKey(ByVal ContainerID As String) As Boolean
        Dim oCont As MultiContPick = GetMultiContPick()

        'if container in multi colection
        If oCont.dictCont.Contains(ContainerID) Then
            'container already scan
            If oCont.dictPickedCont.ContainsKey(ContainerID) Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function ActiveContainerContain(ByVal ContainerID As String) As Boolean
        Dim oCont As MultiContPick = GetMultiContPick()

        'if container in multi colection
        If oCont.dictCont.Contains(ContainerID) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function CloseActiveContainer(ByVal ContainerID As String) As Boolean
        Dim oCont As MultiContPick = GetMultiContPick()

        Dim ret As Boolean = True
        Try
            Dim i As Integer = oCont.dictCont.IndexOf(ContainerID)
            If i = -1 Then
                ret = False
                If HttpContext.Current.Session("PCKPicklistActiveContainerID") <> ContainerID And oCont.dictCont.Count > 0 Then
                    HttpContext.Current.Session("PCKPicklistActiveContainerID") = oCont.dictCont(0)
                Else
                    'Commented for RWMS-1643 and RWMS-745
                    'HttpContext.Current.Session("PCKPicklistActiveContainerID") = ""
                    'End Commented for RWMS-1643 and RWMS-745
                    'Added for RWMS-1643 and RWMS-745
                    If HttpContext.Current.Session("PCKPicklistActiveContainerID") <> ContainerID Then
                        HttpContext.Current.Session("PCKPicklistActiveContainerID") = ""
                    End If
                    'End Added for RWMS-1643 and RWMS-745

                End If

                Return ret
            End If
            oCont.dictCont.RemoveAt(i)

            If HttpContext.Current.Session("PCKPicklistActiveContainerID") = ContainerID And oCont.dictCont.Count > 0 Then
                HttpContext.Current.Session("PCKPicklistActiveContainerID") = oCont.dictCont(0)
            End If

            HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond") = oCont

        Catch ex As Exception
            ret = False
            'Commented for RWMS-1643 and RWMS-745
            'HttpContext.Current.Session("PCKPicklistActiveContainerID") = ""
            'End Commented for RWMS-1643 and RWMS-745

        End Try
        Return ret
    End Function

    Public Shared Sub SetPickedContainerUnits(ByVal ContainerID As String, ByVal Units As Decimal)
        Dim oCont As MultiContPick = GetMultiContPick()

        oCont.AddPickedContainer(ContainerID, Units)

        HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond") = oCont
    End Sub


    Public Shared Sub SetPickedContainerWeight(ByVal ContainerID As String, ByVal Weight As Decimal)
        Dim oCont As MultiContPick = GetMultiContPick()

        oCont.AddPickedContainer(ContainerID, 0, Weight)

        HttpContext.Current.Session("PCKPicklistActiveContainerIDSecond") = oCont
    End Sub

    Public Shared Function GetTotalPickedContainerUnits() As Decimal
        Dim oCont As MultiContPick = GetMultiContPick()

        Return oCont.TotalPickedContainerUnits
    End Function

    <CLSCompliant(False)>
    Public Shared Function GetTotalPickedContainerUOMUnits(ByVal pckJob As WMS.Logic.PickJob) As Decimal
        Dim oCont As MultiContPick = GetMultiContPick()
        Dim sku As New WMS.Logic.SKU(pckJob.consingee, pckJob.sku)

        Return oCont.TotalPickedContainerUnits / sku.ConvertToUnits(pckJob.uom)
    End Function

    Public Shared Function GetPickedContainerCount() As Decimal
        Dim oCont As MultiContPick = GetMultiContPick()

        Return oCont.dictPickedCont.Count 'PickedContainerCount
    End Function

    Public Shared Sub ClearPickedContainer()
        Dim oCont As MultiContPick = GetMultiContPick()

        oCont.dictPickedCont.Clear()

        oCont.PickedContainerCount = 0
        oCont.TotalPickedContainerUnits = 0
        oCont.TotalPickedContainerWeight = 0

    End Sub

    Public Shared Sub finishPartial(logger As LogHandler)
        Dim oCont As MultiContPick = GetMultiContPick()

        oCont.finishPartial(logger)

        ClearPickedContainer()
    End Sub

    Public Shared Function GetContainerKeyByIndex(ByVal index As Integer) As KeyValuePair(Of String, MultiContPick.ContData)
        Dim oCont As MultiContPick = GetMultiContPick()
        Dim i As Integer = 2

        For Each pair As KeyValuePair(Of String, MultiContPick.ContData) In oCont.dictPickedCont
            If i = index Then
                Return pair
            End If
            i += 1
        Next

        Return Nothing
    End Function

End Class