Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Public Class AppPageDelivery
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Delivered
        [Error]
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to deliver...")
        End If
        Dim tm As New WMS.Logic.TaskManager
        Dim deltsk As DeliveryTask = tm.getAssignedTask(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, WMS.Lib.TASKTYPE.DELIVERY)
        If deltsk Is Nothing Then
            If Not oLogger Is Nothing Then
                oLogger.Write("No Delivery job assigned to user. Exiting function with error.")
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = "No Delivery job assigned to user"
            Me.FillSingleRecord(oLogger)
            Return _resp
        End If
        Session("PCKDeliveryTask") = deltsk
        Dim delobj As DeliveryJob = deltsk.getDeliveryJob()
        Session.Item("PCKDeliveryJob") = delobj
        If Not oLogger Is Nothing Then
            oLogger.Write("Delivery job set...")
            oLogger.Write("Will try to deliver Handling unit to: " & _msg(0)("Delivery Location").FieldValue)
        End If
        Try
            tm = New WMS.Logic.TaskManager(delobj.TaskId)
            CType(tm.Task, WMS.Logic.DeliveryTask).Deliver(_msg(0)("Delivery Location").FieldValue, delobj.IsHandOff, Nothing)
        Catch ex As Made4Net.Shared.M4NException
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Delivery job completed successfully.")
        End If
        Me._responseCode = ResponseCode.Delivered
        Me.FillSingleRecord(oLogger)
        Return _resp
    End Function
End Class
