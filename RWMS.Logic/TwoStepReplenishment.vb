Imports System
Imports Made4Net.DataAccess
Imports Made4net.Shared
Imports Made4Net.Shared.Web
Imports System.Collections.Generic

Public Class TwoStepReplenishment

#Region "variables"

    Public alistRepl As New ArrayList
    Public alistDeliver As New ArrayList

    ' Public dictRepl As New Dictionary(Of String, String)
    Public SelectFields As String = "  TASK, TASKTYPE, STATUS, TASKSUBTYPE, FROMLOCATION, FROMLOCSORTORDER, ASSIGNED, USERID, Weight, CUBIC, SKU, CONSIGNEE, UNITS, FROMLOCUSAGETYPE, TOLOCATION, TOWAREHOUSEAREA, TOLOCSORTORDER, TOLOCUSAGETYPE, FROMWAREHOUSEAREA "

    'Weight, cubic
    Public trolley As String
    Public WHArea As String
    Private _totalcases As Double = 0
    Private _TotalWeight As Double = 0
    Private _totalcubic As Double = 0
    Private _lastLocSortorderFromTrolley As String = String.Empty
    Private _taskindex As Integer = 0
    Private _taskdeliveryindex As Integer = 0

    Public Property TaskDeliveryIndex() As Integer
        Get
            Return _taskdeliveryindex
        End Get
        Set(ByVal value As Integer)
            _taskdeliveryindex = value
        End Set
    End Property

    Public Property TaskIndex() As Integer
        Get
            Return _taskindex
        End Get
        Set(ByVal value As Integer)
            _taskindex = value
        End Set
    End Property

    Public Property LastLocSortorderFromTrolley() As String
        Get
            Return _lastLocSortorderFromTrolley
        End Get
        Set(ByVal value As String)
            _lastLocSortorderFromTrolley = value
        End Set
    End Property

    Public Property TotalCases() As Double
        Get
            Return _totalcases
        End Get
        Set(ByVal value As Double)
            _totalcases = value
        End Set
    End Property


    Public Property TotalCubic() As Double
        Get
            Return _totalcubic
        End Get
        Set(ByVal value As Double)
            _totalcubic = value
        End Set
    End Property

    Public Property TotalWeight() As Double
        Get
            Return _TotalWeight
        End Get
        Set(ByVal value As Double)
            _TotalWeight = value
        End Set
    End Property
#End Region

    Public Sub New(ByVal trolley As String, ByVal WHArea As String)
        Me.trolley = trolley
        Me.WHArea = WHArea
    End Sub

    Public Sub clear()
        TotalWeight = 0
        TotalCubic = 0
        TotalCases = 0
        TaskDeliveryIndex = 0
        TaskIndex = 0
        LastLocSortorderFromTrolley = ""
        alistRepl.Clear()
        'alistRepl = Nothing
        alistDeliver.Clear()
        'alistDeliver = Nothing
    End Sub

    Public Sub Load()
        LoadictReplenishmentOnTrolley()
        LoadAvailable()
    End Sub

    Private Sub AddToDeliverList(ByVal dr As DataRow)
        alistDeliver.Add(dr("TASK"))
    End Sub


    Private Sub AddToReplList(ByVal dr As DataRow, Optional ByVal fAddRepl As Boolean = True)
        TotalWeight += Convert.ToDouble(dr("Weight"))
        TotalCubic += Convert.ToDouble(dr("CUBIC"))

        Dim sku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))

        If WMS.Logic.SKU.SKUUOM.Exists(dr("CONSIGNEE"), dr("SKU"), "CASE") Then
            TotalCases += Convert.ToDouble(sku.ConvertUnitsToUom("CASE", dr("UNITS")))
        End If

        '        dictRepl.Add(dr("TASK"), dr("LOCSORTORDER"))
        If fAddRepl Then
            alistRepl.Add(dr("TASK"))
        Else
            'delivery task count++
            TaskDeliveryIndex += 1
        End If
    End Sub


    Public Function LoadDeliveredOnTrolley(ByRef msg As String) As Boolean
        If alistDeliver.Count > 0 Then
            alistDeliver.Clear()
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'Dim sql As String = "SELECT {0} FROM vTASKSUBTYPE where  status = 'AVAILABLE' AND FROMLOCATION='{1}' order by TOLOCSORTORDER"
        Dim sql As String = "SELECT {0} FROM vTASKSUBTYPE where FROMLOCATION='{1}' and  (STATUS = 'AVAILABLE' or (STATUS = 'ASSIGNED' and USERID = '{2}')) order by TOLOCSORTORDER"

        sql = String.Format(sql, SelectFields, trolley, WMS.Logic.GetCurrentUser)
        Dim dt As New DataTable()

        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, "")

            'Dim loc As WMS.Logic.Location
            If dt.Rows.Count = 0 Then
                msg = t.Translate("no task where found")
                Return False
            End If
            For Each dr As DataRow In dt.Rows
                'If loc.CUBIC <= TotalCubic Or loc.WEIGHT <= TotalWeight Then
                AddToDeliverList(dr)
                'Dim tsk As New WMS.Logic.Task(dr("TASK"))
                'tsk.AssignUser(WMS.Logic.GetCurrentUser)
                'Else
                '    Exit For
                'End If
            Next

        Catch ex As Exception
            ' ThrowError("Error in constractor class " + ex.Message)
        End Try
        Return True
    End Function

    Public Sub LoadictReplenishmentOnTrolley()
        If alistRepl.Count > 0 Then
            clear()
        End If

        '    Dim sql As String = "SELECT {0} FROM vTASKSUBTYPE where  ISNULL(FROMLOCUSAGETYPE,'')<>'TROLLEY' and status = 'ASSIGNED' and userid='{1}'  order by FROMLOCSORTORDER"
        Dim sql As String = "SELECT {0} FROM vTASKSUBTYPE where  FROMLOCATION='{1}'  and  (STATUS = 'AVAILABLE' or (STATUS = 'ASSIGNED' and USERID = '{2}')) " 'order by FROMLOCSORTORDER"

        sql = String.Format(sql, SelectFields, trolley, WMS.Logic.GetCurrentUser)
        Dim dt As New DataTable()

        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, "")

            Dim loc As WMS.Logic.Location

            loc = New WMS.Logic.Location(trolley, WHArea)

            For Each dr As DataRow In dt.Rows

                AddToReplList(dr, False)

                'Dim tsk As New WMS.Logic.Task(dr("TASK"))
                'tsk.AssignUser(WMS.Logic.GetCurrentUser)

                If Not IsDBNull(dr("FROMLOCSORTORDER")) Then LastLocSortorderFromTrolley = dr("FROMLOCSORTORDER")
               
            Next

        Catch ex As Exception
            ' ThrowError("Error in constractor class " + ex.Message)
        End Try
    End Sub

   

    Public Sub LoadAvailable(Optional ByVal AddOneLine As Boolean = False)

        Dim LOCSORTORDER As String = LastLocSortorderFromTrolley

        Dim sql As String

        If Not String.IsNullOrEmpty(LOCSORTORDER) Then
            sql = "SELECT {0} FROM vTASKSUBTYPE where ISNULL(FROMLOCUSAGETYPE,'')<>'TROLLEY' and status = 'AVAILABLE' and FROMLOCSORTORDER > '{1}' order by FROMLOCSORTORDER"
            sql = String.Format(sql, SelectFields, LOCSORTORDER)
        Else
            sql = "SELECT {0} FROM vTASKSUBTYPE where ISNULL(FROMLOCUSAGETYPE,'')<>'TROLLEY' and status = 'AVAILABLE' order by FROMLOCSORTORDER"
            sql = String.Format(sql, SelectFields)
        End If

        Dim dt As New DataTable()

        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, "")
               Dim loc As WMS.Logic.Location
            'If dt.Rows.Count > 0 Then
            loc = New WMS.Logic.Location(trolley, WHArea)
            'End If

            For Each dr As DataRow In dt.Rows

                If AddOneLine Or _
                (loc.CUBIC >= TotalCubic + Convert.ToDouble(dr("Cubic")) And loc.WEIGHT >= TotalWeight + Convert.ToDouble(dr("Weight"))) Then

                    AddToReplList(dr)

                    Dim tsk As New WMS.Logic.Task(dr("TASK"))
                    tsk.AssignUser(WMS.Logic.GetCurrentUser)
                    If AddOneLine Then
                        Exit For
                    End If
                Else
                    'If the total weight/cubic of the available tasks from the trolley is more than the 
                    'location the system will assign the next one task and go to the next screen.
                    'AddToReplList(dr)

                    'Dim tsk As New WMS.Logic.Task(dr("TASK"))
                    'tsk.AssignUser(WMS.Logic.GetCurrentUser)
                    Exit For
                End If
            Next
        Catch ex As Exception
            ' ThrowError("Error in constractor class " + ex.Message)
        End Try
    End Sub

    Public Function deliver(ByRef msg As String) As Boolean
        Dim ret As Boolean
        ' If TaskIndex < alistRepl.Count Then
        UnAssignTaskAllRepl()
        'End If
        clear()
        ret = LoadDeliveredOnTrolley(msg)
        Return ret
    End Function

    Public Sub UnAssignTask()
        If TaskIndex < alistRepl.Count Then
            For i As Integer = TaskIndex To alistRepl.Count - 1
                Dim t As New WMS.Logic.Task(alistRepl.Item(i))
                t.DeAssignUser()
            Next i
        End If
    End Sub

    Public Sub UnAssignTaskAllRepl()

        Dim sql As String = "SELECT {0} FROM vTASKSUBTYPE where status = 'ASSIGNED' and userid='{1}'  order by FROMLOCSORTORDER"
        sql = String.Format(sql, SelectFields, WMS.Logic.GetCurrentUser)
        Dim dt As New DataTable()

        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, "")


            For Each dr As DataRow In dt.Rows
                Dim t As New WMS.Logic.Task(dr("TASK"))
                t.DeAssignUser()
            Next

        Catch ex As Exception
            ' ThrowError("Error in constractor class " + ex.Message)
        End Try
    End Sub
End Class
