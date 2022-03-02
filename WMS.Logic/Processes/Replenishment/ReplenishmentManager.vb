Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.General
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging

<CLSCompliant(False)> Public Class ReplenishmentManager


#Region "Constructors"

#End Region

#Region "Variables"

    Protected _replid As String

#End Region

#Region "Methods"

#Region "GetOutBoundDepartureDate()"
    ''' <summary>
    ''' Get the Get OutBound Departure Date for the Order ID
    ''' </summary>
    ''' <param name="pOrderId"></param>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''To Get the departure date/time of the shipment and/or outbound order
    ''' 
    Public Function GetOutBoundDepartureDate(ByVal pOrderId As String) As Date
        Try
            Dim SQL As String = String.Empty
            Dim dt As New DataTable, dr As DataRow
            Dim DepartureDate As Date
            Dim dao = New ReplenishmentDao
            dt = dao.GetOutBoundDepartureDateByOrder(pOrderId)
            For Each dr In dt.Rows
                DepartureDate = dr("SCHEDULEDDATE")
            Next

            Return DepartureDate

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try

    End Function
#End Region

#Region "GetWaveDepartureDate()"
    ''' <summary>
    ''' Get the  Wave Departure Date for the Wave ID
    ''' </summary>
    ''' <param name="pWaveId"></param>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''To Get the departure date/time of the shipment and/or for Wave Details
    Public Function GetWaveDepartureDate(ByVal pWaveId As String) As Date
        Try
            Dim SQL As String = String.Empty
            Dim dt As New DataTable, dr As DataRow
            Dim DepartureDate As Date
            Dim dao = New ReplenishmentDao
            dt = dao.GetWaveDepartureDateByWave(pWaveId)


            For Each dr In dt.Rows
                DepartureDate = dr("SCHEDULEDDATE")
            Next

            Return DepartureDate

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try

    End Function
#End Region

#Region "GetStandardTime()"

    'To Get the Standard Time from the Tasks table
    Private Function GetStandardTime(ByVal pUser As String) As Decimal
        Try
            Dim SQL As String = String.Empty
            Dim dt As New DataTable, dr As DataRow
            Dim StandardTime As Double = 0
            Try
                Dim dao = New ReplenishmentDao
                dt = dao.GetStandardTimeForUser(pUser)

            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                    Throw
                End If
            End Try


            For Each dr In dt.Rows
                StandardTime = dr("STDTIME")
            Next

            Return StandardTime

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try

    End Function
#End Region

#Region "CalculatePickTime()"
    ' Calculating the Picking time from the PICKList number for the User
    Private Function CalculatePickTime(ByVal pUser As String) As Decimal
        Try

            Dim SQL As String = String.Empty
            Dim dt As New DataTable, dr As DataRow
            Dim TotalStandardTime As Double = 0
            Dim StandardTime As Double = 0
            Dim TotalQty As Double = 0
            Try
                Dim dao = New ReplenishmentDao
                dt = dao.CalculatePickTimeForUser(pUser)


            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                    Throw
                End If
            End Try

            For Each dr In dt.Rows
                TotalQty = TotalQty + dr("QTY")
            Next
            StandardTime = GetStandardTime(pUser)
            TotalStandardTime = StandardTime / TotalQty

            Return TotalStandardTime

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If

        End Try
    End Function
#End Region

#Region "CalculateReplenishmentTime()"
    ''' <summary>
    ''' Get the Calculate Replenishment Time For User
    ''' </summary>
    ''' <param name="PickLocation"></param>    
    ''' <param name="pUser"></param>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function CalculateReplenishmentTime(ByVal PickLocation As String, ByVal pUser As String) As DateTime
        Try

            Dim SQL As String = String.Empty
            Dim dt As New DataTable, dr As DataRow
            Dim TotalStandardTime As Double = 0
            Dim StandardTime As Double = 0
            Dim TotalPickQuantity As Double = 0
            Dim PickReachLocation As Double
            Dim TotalSeconds As Integer
            Dim ReplenishmentDateTime As DateTime

            Try
                Dim dao = New ReplenishmentDao
                dt = dao.CalculateReplenishmentTimeForUser(PickLocation, pUser)

            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                    Throw
                End If
            End Try


            For Each dr In dt.Rows
                If PickLocation <> dr("FROMLOCATION") Then
                    TotalPickQuantity = TotalPickQuantity + dr("QTY")
                    TotalSeconds = TotalSeconds + dr("TotalStandardSeconds")
                    ReplenishmentDateTime = dr("STARTTIME")
                Else
                    Exit For
                End If
            Next

            TotalStandardTime = CalculatePickTime(pUser)
            PickReachLocation = (TotalPickQuantity * TotalStandardTime) + TotalSeconds
            ReplenishmentDateTime = ReplenishmentDateTime.AddSeconds(PickReachLocation)

            Return ReplenishmentDateTime

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Function
#End Region

#Region "CancelReplenishments()"

    ''' <summary>
    ''' Get the Cancel Replenishments for the task based on replenishment location
    ''' </summary>
    ''' <param name="psku"></param>
    ''' <param name="pconsignee"></param>
    ''' <param name="plocation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CancelReplenishments(psku As String, pconsignee As String, plocation As String, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        Try
            Dim SQL As String = String.Empty
            Dim SQLQuery As String = String.Empty
            Dim dt As New DataTable
            Dim i As Integer = 0
            Dim Available As Double
            Dim MaximumQty As Double
            Dim _status As String
            Dim _editdate As Date
            Dim _replid As String
            _status = WMS.Lib.Statuses.Replenishment.CANCELED
            _editdate = DateTime.Now

            If Not oLogger Is Nothing Then
                oLogger.Write("Calling CancelReplenishments Method for :" & "SKU :" & psku & " Consignee :" & pconsignee & " Location :" & plocation)
            End If

            Try
                Dim dao = New ReplenishmentDao
                dt = dao.GetCancelReplenishments(psku, pconsignee, plocation, oLogger)

            Catch ex As Exception
                If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                    Throw
                End If
            End Try

            If dt.Rows.Count > 0 Then

                Available = (dt.Rows(0)("CURRENTQTY") - dt.Rows(0)("ALLOCATEDQTY")) + dt.Rows(0)("PENDINGQTY")
                MaximumQty = dt.Rows(0)("MAXREPLQTY")

                If Not oLogger Is Nothing Then
                    oLogger.Write("Available Qty = CurrentQty - AllocatedQty + PendingQty : Available Qty = " & Available & " CurrentQty :" & dt.Rows(0)("CURRENTQTY") & " AllocatedQty :" & dt.Rows(0)("ALLOCATEDQTY") & " PendingQty :" & dt.Rows(0)("PENDINGQTY"))
                End If
                If Not oLogger Is Nothing Then
                    : oLogger.Write(" Maximum Replenishment Qty :" & MaximumQty)
                End If

                While Available > MaximumQty
                    _replid = dt.Rows(i)("REPLID")
                    Try
                        Dim daoupdate = New ReplenishmentDao
                        daoupdate.UpdateReplenishmentStatus(_status, _editdate, "SYSTEM", _replid, oLogger)

                        If Not oLogger Is Nothing Then
                            oLogger.Write(" If Available > MaximumQty : then Updateing Replenishment Status ")
                        End If

                    Catch ex As Exception
                        If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                            Throw
                        End If
                    End Try
                    'Subtracting the replenishment units that were pending
                    Available = Available - dt.Rows(i)("Units")

                    If Not oLogger Is Nothing Then
                        oLogger.Write(" Subtracting the replenishment units that were pending so Available Qty: " & Available)
                    End If

                    Dim daotask = New ReplenishmentDao
                    Dim TotalUnitsAllocated As Double
                    Dim daopayload = New ReplenishmentDao
                    Dim dtpayload As DataTable
                    dtpayload = daopayload.GetLoadIDForPayload(dt.Rows(i)("FROMLOAD"), oLogger)

                    If Not oLogger Is Nothing Then
                        oLogger.Write(" Calling GetLoadIDForPayload() for LoadID : " & dt.Rows(i)("FROMLOAD"))
                    End If
                    If Not oLogger Is Nothing Then
                        oLogger.Write(" Subtracting the replenishment units that were pending so Available Qty: after Calling GetLoadIDForPayload() " & Available)
                    End If

                    If daotask.hasTask(_replid, oLogger) Then
                        Dim ts As Task = daotask.getReplTask(True, _replid, oLogger)
                        If ts.STATUS <> WMS.Lib.Statuses.Task.CANCELED And ts.STATUS <> WMS.Lib.Statuses.Task.COMPLETE Then
                            ts.Cancel()
                            TotalUnitsAllocated = dt.Rows(i)("UNITS") - dtpayload.Rows(0)("UNITSALLOCATED")
                            If Not oLogger Is Nothing Then
                                oLogger.Write(" TotalUnitsAllocated : " & TotalUnitsAllocated)
                            End If

                            daopayload.UpdatePayloadStatus(TotalUnitsAllocated, _editdate, "SYSTEM", dtpayload.Rows(0)("LOADID"), oLogger)

                        End If
                    End If
                    i = i + 1
                End While

                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try




    End Function
#End Region



#Region "UpdateReplenishmentDate()"
    Sub UpdateReplenishmentDate(dueDate As Date, PickLocation As PickLoc)
        Try

            Dim dao As ReplenishmentDao
            dao.UpdateReplenishmentDate(dueDate, PickLocation)

        Catch ex As Exception
            If ExceptionPolicy.HandleException(ex, Constants.Unhandled_Policy) Then
                Throw
            End If
        End Try
    End Sub

#End Region

#End Region





End Class
