Imports WMS.Logic
Imports Made4Net.AppSessionManagement
Imports WLTaskManager = WMS.Logic.TaskManager

Public Class AppPageGetPick
    Inherits AppPageProcessor

    Private Enum ResponseCode
        NoError
        [Error]
        NoAvailablePickJob
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
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
            oLogger.Write("Processing Picking Request...")
        End If

        'Clear the session from the previous picks
        Session("PCKPicklist") = Nothing
        Session("PARPCKPicklist") = Nothing

        Dim isLastPickJob As Boolean = False
        Dim _assigned As Boolean = False
        Dim _tsk As WMS.Logic.Task = New WMS.Logic.Task(_msg(0)("Task ID").FieldValue)
        If Not oLogger Is Nothing Then
            oLogger.Write("Task Received in message: " & _tsk.TASK)
        End If
        If _tsk.ASSIGNED Then
            If _tsk.USERID.Equals(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, StringComparison.OrdinalIgnoreCase) Then
                _assigned = True
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("Task Assigned to another user!...")
                End If
                Me._responseCode = ResponseCode.Error
                Me._responseText = "Assigned to another user"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
        Else
            Try
                If _tsk.STATUS = WMS.Lib.Statuses.Task.AVAILABLE Then
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Task is not Assigned. Trying to assign user...")
                    End If
                    _tsk.AssignUser(WMS.Logic.Common.GetCurrentUser)
                    _assigned = True
                End If
            Catch ex As Exception
                Me._responseCode = ResponseCode.Error
                Me._responseText = ex.Message
                Me.FillSingleRecord(oLogger)
                Return _resp
            End Try
        End If
        If _msg(0)("Pick Mode").FieldValue.ToUpper.Equals("B") Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Picking Mode is Batch Pick (B). Will try to get all details related.")
            End If
            Me.FillRecordsFromView(String.Format("task='{0}' AND VType='B'", _tsk.TASK), Nothing) ', oLogger)
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Picking Mode is Online pick. Will try to get Next open pick job...")
            End If
            Dim pck As PickJob
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForTask(_tsk.TASK)
            Dim isGroupPickDetails As Boolean = False
            If tm.Task.TASKTYPE = WMS.Lib.TASKTYPE.PARALLELPICKING Then
                Dim pcklst As New ParallelPicking(tm.Task.ParallelPicklist)
                pck = pcklst.GetNextPick
                Session("PARPCKPicklist") = pcklst
                If Not pck Is Nothing Then
                    'isLastPickJob = IsLastLine(tm.Task.ParallelPicklist)
                    Session("PARPCKPicklistPickJob") = pck
                    Dim pcklist As Picklist = New Picklist(pck.picklist)
                    'SetContainerID(pcklist, oLogger)
                    isGroupPickDetails = pcklist.getReleaseStrategy().GroupPickDetails
                End If
            Else
                Dim pcklst As New Picklist(tm.Task.Picklist)
                pck = PickTask.getNextPick(pcklst)
                Session("PCKPicklist") = pcklst
                'isLastPickJob = IsLastLine(pcklst)
                Session("PCKPicklistPickJob") = pck
                'SetContainerID(Nothing, oLogger)
                isGroupPickDetails = pcklst.getReleaseStrategy().GroupPickDetails
            End If
            If Not pck Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Pick job Found...")
                End If
                If isLastPickJob Then
                    'Me._responseCode = ResponseCode.LastLine
                Else
                    Me._responseCode = ResponseCode.NoError
                End If
                Try
                    Me.FillRecordsFromView(String.Format("picklistid='{0}' AND sku='{1}' and fromload='{2}' and uom='{3}'", pck.picklist, pck.sku, pck.fromload, pck.uom), Nothing) ' oLogger)
                    If _resp.Count > 1 AndAlso isGroupPickDetails Then
                        For i As Integer = _resp.Count - 1 To 1 Step -1
                            _resp.RemoveRecord(i)
                        Next
                    End If
                    _resp.Record(0)("QUANTITY").FieldValue = pck.uomunits
                    _resp.Record(0)("UOM").FieldValue = pck.uom.ToLower
                    _resp.Record(0)("UOMTOBASEUOM").FieldValue = pck.units
                Catch ex As Exception
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Error while constructing response message fields. Error details: " & ex.ToString)
                    End If
                    Throw ex
                End Try

                Try
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Pick Job set, updating count flag...")
                    End If
                    'Check For LowLimitCount
                    Dim oLoad As New WMS.Logic.Load(pck.fromload)
                    'Dim oSku As New WMS.Logic.SKU(oLoad.CONSIGNEE, oLoad.SKU)
                    Dim oSku As WMS.Logic.SKU
                    If pck.oSku Is Nothing Then
                        oSku = New WMS.Logic.SKU(oLoad.CONSIGNEE, oLoad.SKU)
                    Else
                        oSku = pck.oSku
                    End If
                    If oLoad.UNITS <= oSku.LOWLIMITCOUNT And oLoad.LASTCOUNTDATE.Date < DateTime.Now.Date Then
                        UpdateCountFlag(True)
                    Else
                        UpdateCountFlag(False)
                    End If
                Catch ex As Exception
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Error updating count flag: " & ex.ToString)
                    End If
                End Try
            Else
                Me._responseCode = ResponseCode.NoAvailablePickJob
                Me._responseText = "No available pick job found"
                Me.FillSingleRecord(oLogger)
                If Not oLogger Is Nothing Then
                    oLogger.Write("No available Pick job Found...")
                End If
                Return _resp
            End If
        End If
        PrintResponseMessageContent(oLogger)
        If Not oLogger Is Nothing Then
            oLogger.Write("Pick job request proccessed successfully...")
        End If
        Return _resp
    End Function

    Private Sub UpdateCountFlag(ByVal CountIsNeeded As Boolean)
        For i As Int32 = 0 To _resp.Count - 1
            _resp.Record(0)("CountFlag").FieldValue = CountIsNeeded
        Next
    End Sub

End Class