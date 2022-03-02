Imports WMS.Logic
Imports Made4Net.AppSessionManagement

Public Class AppPageGetPick
    Inherits AppPageProcessor

    Private Enum ResponseCode
        NoError
        [Error]
        LastLine
        NoAvailablePickJob
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
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
            Me.FillRecordsFromView(String.Format("task='{0}' AND VType='B'", _tsk.TASK), oLogger)
        Else
            If Not oLogger Is Nothing Then
                oLogger.Write("Picking Mode is Online pick. Will try to get Next open pick job...")
            End If
            Dim pck As PickJob
            Dim tm As New WMS.Logic.TaskManager(_tsk.TASK, False)
            Dim isGroupPickDetails As Boolean = False
            If tm.Task.TASKTYPE = WMS.Lib.TASKTYPE.PARALLELPICKING Then
                Dim pcklst As New ParallelPicking(tm.Task.ParallelPicklist)
                pck = pcklst.GetNextPick
                Session("PARPCKPicklist") = pcklst
                If Not pck Is Nothing Then
                    'isLastPickJob = IsLastLine(tm.Task.ParallelPicklist)
                    Session("PARPCKPicklistPickJob") = pck
                    Dim pcklist As Picklist = New Picklist(pck.picklist)
                    SetContainerID(pcklist, oLogger)
                    isGroupPickDetails = pcklist.getReleaseStrategy().GroupPickDetails
                End If
            Else
                Dim pcklst As New Picklist(tm.Task.Picklist)
                pck = PickTask.getNextPick(pcklst)
                Session("PCKPicklist") = pcklst
                'isLastPickJob = IsLastLine(pcklst)
                Session("PCKPicklistPickJob") = pck
                SetContainerID(Nothing, oLogger)
                isGroupPickDetails = pcklst.getReleaseStrategy().GroupPickDetails
            End If
            If Not pck Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("Pick job Found...")
                End If
                If isLastPickJob Then
                    Me._responseCode = ResponseCode.LastLine
                Else
                    Me._responseCode = ResponseCode.NoError
                End If
                Me.FillRecordsFromView(String.Format("picklistid='{0}' AND sku='{1}' and fromload='{2}'", pck.picklist, pck.sku, pck.fromload), oLogger)

                If _resp.Count > 1 AndAlso isGroupPickDetails Then
                    For i As Integer = 1 To _resp.Count - 1
                        _resp.Record(0)("QUANTITY").FieldValue = Decimal.Parse(_resp.Record(0)("QUANTITY").FieldValue) + Decimal.Parse(_resp.Record(i)("QUANTITY").FieldValue)
                    Next
                    For i As Integer = _resp.Count - 1 To 1 Step -1
                        _resp.RemoveRecord(i)
                    Next
                End If

                Try
                    If Not oLogger Is Nothing Then
                        oLogger.Write("Pick Job set, updating count flag...")
                    End If
                    'Check For LowLimitCount
                    Dim oLoad As New WMS.Logic.Load(pck.fromload)
                    Dim oSku As New WMS.Logic.SKU(oLoad.CONSIGNEE, oLoad.SKU)
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

    Private Sub SetContainerID(ByVal oPicklist As WMS.Logic.Picklist, ByVal oLogger As WMS.Logic.LogHandler)
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to set the active container for picking...")
        End If
        Dim pcklist As Picklist
        If oPicklist Is Nothing Then
            pcklist = Session("PCKPicklist")
        Else
            pcklist = oPicklist
        End If
        If Not pcklist Is Nothing Then
            If Not oLogger Is Nothing Then
                oLogger.Write("Trying to get the active container from the current picklist...")
            End If
            If Session("PCKPicklistActiveContainerID") Is Nothing Or Session("PCKPicklistActiveContainerID") <> pcklist.ActiveContainer Then
                Dim contid As String
                If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    If pcklist.ActiveContainer <> String.Empty Then
                        contid = pcklist.ActiveContainer
                    Else
                        contid = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                    End If
                    Session("PCKPicklistActiveContainerID") = contid
                End If
            End If
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Container {0} was set for picking.", Session("PCKPicklistActiveContainerID")))
        End If
    End Sub

    'Private Function IsLastLine(ByVal pPicklist As WMS.Logic.Picklist) As Boolean
    '    Dim cnt As Int32 = 0
    '    For Each pickdet As WMS.Logic.PicklistDetail In pPicklist.Lines
    '        If pickdet.Status <> WMS.Lib.Statuses.Picklist.CANCELED And pickdet.Status <> WMS.Lib.Statuses.Picklist.COMPLETE Then
    '            cnt += 1
    '        End If
    '    Next
    '    If cnt = 1 Then
    '        Return True
    '    End If
    '    Return False
    'End Function

    'Private Function IsLastLine(ByVal pParallelPicklist As String) As Boolean
    '    Dim cnt As Int32 = 0
    '    Dim sql As String = String.Format("select count(1) from pickdetail join parallelpickdetail on pickdetail.picklist = parallelpickdetail.picklist " & _
    '            "where pickdetail.picklist in (select picklist from parallelpickdetail where parallelpickid = '{0}') " & _
    '            "and pickdetail.status not in ('{1}','{2}') ", pParallelPicklist, WMS.Lib.Statuses.Picklist.COMPLETE, WMS.Lib.Statuses.Picklist.CANCELED)
    '    cnt = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
    '    If cnt = 1 Then
    '        Return True
    '    End If
    '    Return False
    'End Function

End Class


