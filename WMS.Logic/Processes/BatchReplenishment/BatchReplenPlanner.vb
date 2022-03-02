Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections.Generic
Imports System.Runtime.CompilerServices
Imports System.Text

#Region "BatchReplenPlanner"
<CLSCompliant(False)> Public Class BatchReplenPlanner

#Region "Varibales"

    Protected _batchrepobjs As BatchReplenDetailCollection
    Protected _batchreplenheader As BatchReplenHeader

#End Region

#Region "Properties"


#End Region

#Region "Constructor"
    Public Sub New(ByVal batchreplenheader As BatchReplenHeader)
        _batchreplenheader = batchreplenheader
    End Sub

#End Region

#Region "Method"

#Region "Plan Batch Replenishment"

    Public Sub Plan(Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        Try
            PlanBatchReplen(UserId, oLogger)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error occured: " & ex.ToString)
            End If
        End Try
    End Sub

    Private Sub PlanBatchReplen(Optional ByVal UserId As String = WMS.Lib.USERS.SYSTEMUSER, Optional ByVal oLogger As LogHandler = Nothing)
        If Not oLogger Is Nothing Then
            oLogger.StartWrite()
            oLogger.WriteTimeStamp = True
            oLogger.Write("Planning Batch Replenishment for pick region: " & _batchreplenheader.PICKREGION)
            oLogger.writeSeperator(" ", 100)
        End If

        '1.Filter batch pick locaitons and select the pick loc by replen point or order demand and calculate the qty required to fill the pick loc
        '2.Create replencollection to fill all from and to details
        '3.Go through replen policy details and identify the payloads to fill the pick location and create line item for each payload 
        '4.Sort the replen line items by location sort order
        Dim brfilter As BatchReplenFilter = New BatchReplenFilter(_batchreplenheader)
        _batchrepobjs = brfilter.GenerateBatchReplenObjs(UserId, oLogger)

        '5.Break replens based of repl policy cube limit
        Dim brbreaker As BatchReplenBreaker = New BatchReplenBreaker(_batchreplenheader)
        Dim brcollectionArray As New ArrayList
        brcollectionArray = brbreaker.BreakBatchReplens(_batchrepobjs, oLogger)
        '6.Create replen container and add batch replen header and batch replen details
        '7.SET status to PLANNED for each batch replen
        Dim brheaderList As New ArrayList
        brheaderList = CreateBatchReplens(brcollectionArray, oLogger)

        '8.Create task for each batch replen and change status to RELEASED
        'ReleaseBatchReplens(brheaderList,userid,oLogger)
        For Each breplenheder As BatchReplenHeader In brheaderList
            CreateBatchReplTask(breplenheder, oLogger)
        Next

        If Not oLogger Is Nothing Then
            oLogger.Write("Planing Batch Replenishment for pick region : " & _batchreplenheader.PICKREGION & " finished.")
            oLogger.EndWrite()
        End If
    End Sub
    Function CreateBatchReplens(ByVal breplenList As ArrayList, Optional ByVal oLogger As LogHandler = Nothing) As ArrayList
        Dim brHeaderList As New ArrayList
        For Each breplencollection As BatchReplenDetailCollection In breplenList
            Dim brHeader As BatchReplenHeader
            Dim brreplpolicy As New BatchReplenishmentPolicy()
            _batchreplenheader.TARGETLOCATION = brreplpolicy.GetStagingLocation(_batchreplenheader.REPLENPOLICY)
            brHeader = breplencollection.Post(_batchreplenheader, breplencollection)
            'Get first replen line fromlocation
            If breplencollection.Count > 0 Then
                brHeader.FROMLOCATION = breplencollection(0).FROMLOCATION
            End If

            brHeaderList.Add(brHeader)
        Next
        Return brHeaderList
    End Function

    Protected Sub CreateBatchReplTask(ByVal breplenheader As BatchReplenHeader, Optional ByVal oLogger As LogHandler = Nothing)

        Dim pUser As String = WMS.Lib.USERS.SYSTEMUSER
        If Not oLogger Is Nothing Then
            oLogger.Write("Creating Batch Replenishment task - BATCHREPLID:" & breplenheader.BATCHREPLID & " - PICKREGION:" & breplenheader.PICKREGION & " - REPLCONTAINER:" & breplenheader.REPLCONTAINER & " - REPLENPOLICY:" & breplenheader.REPLENPOLICY & " - TARGETLOCATION:" & breplenheader.TARGETLOCATION)
            oLogger.writeSeperator(" ", 20)
        End If
        Dim tm As New TaskManager
        Dim drpolicydetail As DataRow
        GetBatchReplenPolicyDetail(breplenheader.REPLENPOLICY, drpolicydetail, oLogger)
        If Not drpolicydetail Is Nothing Then
            If drpolicydetail("CREATETASK") Then
                tm.CreateBatchReplenishmentTask(breplenheader, drpolicydetail("TASKPRIORITY"), drpolicydetail("TASKSUBTYPE"))
                breplenheader.ReleaseBatchReplen()
            End If
        End If

        'End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Batch Replenishment task created and Batch Replenishment status updated to Released")
            oLogger.writeSeperator()
        End If
    End Sub

    Sub GetBatchReplenPolicyDetail(ByVal strBatchReplPolicy As String, ByRef drpolicydetail As DataRow, Optional ByVal oLogger As LogHandler = Nothing)
        Dim sql As String = String.Format("SELECT * FROM REPLPOLICYDETAIL where POLICYID='{0}' ", strBatchReplPolicy)
        If Not oLogger Is Nothing Then
            oLogger.Write("SQL statement : " & sql)
            oLogger.writeSeperator("-", 100)
        End If
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count > 0 Then
            dr = dt.Rows(0)
            drpolicydetail = dr
        Else
            dr = Nothing
        End If

    End Sub

#End Region
#Region "Accessors"

#End Region

#End Region

End Class

#End Region