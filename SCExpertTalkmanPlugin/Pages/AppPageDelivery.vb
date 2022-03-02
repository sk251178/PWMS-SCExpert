Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Public Class AppPageDelivery
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Delivered
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Try
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
                oLogger.Write("Will try to deliver Handling unit to: " & _msg(0)("Delivery Location").FieldValue)
            End If
            Dim deltsk As DeliveryTask = GetDeliveryTask(oLogger)
            If deltsk Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("No delivery task assigned to user. Exiting function with error.")
                End If
                Me._responseCode = ResponseCode.Error
                Me._responseText = "No delivery task assigned to user"
                Me.FillSingleRecord(oLogger)
                Return _resp
            End If
            Dim delobj As DeliveryJob = GetDeliveryJob(oLogger)
            If Not oLogger Is Nothing Then
                oLogger.Write("Delivery job extracted...")
                oLogger.Write(String.Format("Trying to deliver contaienr to : {0} ({1})", _msg(0)("Delivery Location").FieldValue, delobj.toWarehousearea))

            End If
            'Dim tm As New WMS.Logic.TaskManager
            'tm = New WMS.Logic.TaskManager(delobj.TaskId)
            'CType(tm.Task, WMS.Logic.DeliveryTask).Deliver(_msg(0)("Delivery Location").FieldValue, delobj.toWarehousearea, delobj.IsHandOff, Nothing)
            deltsk.Deliver(_msg(0)("Delivery Location").FieldValue, delobj.toWarehousearea, delobj.IsHandOff, Nothing)
            If Not oLogger Is Nothing Then
                oLogger.Write("Delivery job completed.")
            End If
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

    Private Function GetDeliveryTask(logger As LogHandler) As WMS.Logic.DeliveryTask
        If Not Session("PCKDeliveryTask") Is Nothing Then
            Return Session("PCKDeliveryTask")
        Else
            Dim tm As New WMS.Logic.TaskManager
            Dim deltsk As DeliveryTask = tm.getAssignedTask(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, WMS.Lib.TASKTYPE.DELIVERY, logger)
            Return deltsk
        End If
    End Function

    Private Function GetDeliveryJob(logger As LogHandler) As WMS.Logic.DeliveryJob
        If Not Session("PCKDeliveryJob") Is Nothing Then
            Return Session("PCKDeliveryJob")
        ElseIf Not Session("PCKDeliveryTask") Is Nothing Then
            Dim deltsk As DeliveryTask = Session("PCKDeliveryTask")
            Return deltsk.getDeliveryJob
        Else
            Dim tm As New WMS.Logic.TaskManager
            Dim deltsk As DeliveryTask = tm.getAssignedTask(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, WMS.Lib.TASKTYPE.DELIVERY, logger)
            Return deltsk.getDeliveryJob
        End If
    End Function

End Class