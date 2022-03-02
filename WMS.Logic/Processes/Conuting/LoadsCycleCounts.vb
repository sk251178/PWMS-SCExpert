Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class LoadsCycleCounts
    'Inherits Made4Net.Shared.JobScheduling.ScheduledJobBase

#Region "Variables"

    Protected _all As String
    Protected _consignee As String
    Protected _sku As String
    Protected _skugroup As String

#End Region

#Region "Ctors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal sConsignee As String, ByVal sSku As String, ByVal sSkuGroup As String)
        MyBase.New()
        Try
            If Not sConsignee Is Nothing Then _consignee = sConsignee Else _consignee = ""
        Catch ex As Exception
            _consignee = ""
        End Try
        Try
            If Not sSku Is Nothing Then _sku = sSku Else _sku = ""
        Catch ex As Exception
            _sku = ""
        End Try
        Try
            If Not sSkuGroup Is Nothing Then _skugroup = sSkuGroup Else _skugroup = ""
        Catch ex As Exception
            _skugroup = ""
        End Try
        If _consignee = "" And _sku = "" And _skugroup = "" Then _all = "1"
    End Sub

#End Region

#Region "Execute"

    ''!!!!
    ''Moved to scheduledtasks in 4.9.8!
    ''!!!!

    'Public Overrides Sub Execute()
    '    Dim em As New Made4Net.Shared.QMsgSender ' EventManagerQ
    '    Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.LoadCycleCounts
    '    em.Add("EVENT", EventType)
    '    em.Add("CONSIGNEE", _consignee)
    '    em.Add("SKU", _sku)
    '    em.Add("SKUGROUP", _skugroup)
    '    em.Add("ALL", "1")
    '    em.Send("Counting", WMSEvents.EventDescription(EventType))
    'End Sub

#End Region

#Region "Generate Counting Tasks"

    Public Function CreateCycleCountTasks(ByVal pConsignee As String, ByVal pSku As String, ByVal pSkuGroup As String, ByVal oLogger As LogHandler, ByVal pUser As String)
        If pUser = Nothing Or pUser = "" Then pUser = "SYSTEM"
        If Not oLogger Is Nothing Then
            oLogger.Write("Started Creating Loads Cycle Counts tasks.")
            If pConsignee <> "" Or pSku <> "" Or pSkuGroup <> "" Then
                oLogger.Write(String.Format("Received Filters: Consignee - {0}; SKU - {1}; SKU Group - {2}.", pConsignee, pSku, pSkuGroup))
            End If
            oLogger.writeSeperator("*", 100)
        End If
        'Init Cached Data
        Dim oCons As Consignee
        If Consignee.Exists(pConsignee) Then oCons = New Consignee(pConsignee)
        Dim dtSku As DataTable = InitSku(pConsignee, pSku, pSkuGroup)
        Dim dtLoads As DataTable = InitLoads(pConsignee, pSku, pSkuGroup)
        If Not oLogger Is Nothing Then
            oLogger.Write("Got " & dtLoads.Rows.Count & " Loads. Trying to create tasks")
        End If
        'Create Counting tasks
        Dim interval As Int32
        Dim arrLoads() As DataRow
        For Each dr As DataRow In dtSku.Rows
            If Not oLogger Is Nothing Then
                oLogger.Write("Working on consignee = " & dr("consignee") & " and sku = " & dr("sku"))
            End If
            If Not oCons Is Nothing Then
                If oCons.CYCLECOUNTING > 0 Then
                    interval = dtLoads.Rows.Count / oCons.CYCLECOUNTING
                Else
                    interval = dtLoads.Rows.Count / 1
                End If
            Else
                interval = dr("cyclecountint")
            End If
            arrLoads = dtLoads.Select("consignee = '" & dr("consignee") & "' and sku = '" & dr("sku") & "'")
            CreateTasks(arrLoads, interval, oLogger, pUser)
            If Not oLogger Is Nothing Then
                oLogger.writeSeperator("-", 50)
            End If
        Next
        If Not oLogger Is Nothing Then
            oLogger.writeSeperator(" ", 100)
            oLogger.writeSeperator("*", 100)
            oLogger.Write("Finished Creating Loads Cycle Counts tasks.")
        End If
    End Function

    Private Sub CreateTasks(ByVal arrLoads() As DataRow, ByVal pInterval As Int32, ByVal oLogger As LogHandler, ByVal pUser As String)
        Try
            If pInterval = 0 Then pInterval = 1
            Dim numtasks As Int32 = arrLoads.Length / pInterval
            If numtasks = 0 Then
                numtasks = arrLoads.Length
            End If
            Dim ldCnt As New Counting
            For i As Int32 = 0 To numtasks - 1
                ldCnt.CreateLoadCountJobs(arrLoads(i)("consignee"), arrLoads(i)("location"), arrLoads(i)("wharea"), arrLoads(i)("sku"), arrLoads(i)("loadid"), pUser)
            Next
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occurred...")
                oLogger.Write(ex.Description)
            End If
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occurred...")
                oLogger.Write(ex.ToString)
            End If
        End Try
    End Sub

#Region "Accessors"

    Private Function InitSku(ByVal pConsignee As String, ByVal pSku As String, ByVal pSkuGroup As String) As DataTable
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select distinct sku.consignee, sku.sku ,isnull(cyclecountint,1) as cyclecountint  from sku inner join vLoadCycleCount on sku.sku = vLoadCycleCount.sku and sku.consignee = vLoadCycleCount.consignee where SKU.consignee like '%{0}' and SKU.sku like '%{1}' and SKU.skugroup like '%{2}'", _
            pConsignee, pSku, pSkuGroup)
        DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

    Private Function InitLoads(ByVal pConsignee As String, ByVal pSku As String, ByVal pSkuGroup As String) As DataTable
        Dim SQL As String
        Dim dt As New DataTable
        SQL = String.Format("select * from vLoadCycleCount where consignee like '%{0}' and sku like '%{1}' and skugroup like '%{2}'", _
            pConsignee, pSku, pSkuGroup)
        DataInterface.FillDataset(SQL, dt)
        Return dt
    End Function

#End Region

#End Region

End Class
