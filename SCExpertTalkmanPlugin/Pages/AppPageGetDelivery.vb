Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Public Class AppPageGetDelivery
    Inherits AppPageProcessor

    Private Enum ResponseCode
        OK
        UserNotAssigned
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Dim delobj As DeliveryJob
        Dim deltsk As DeliveryTask
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
                oLogger.Write("Trying to get a delivery job for user...")
            End If
            Dim tm As New WMS.Logic.TaskManager
            deltsk = tm.getAssignedTask(Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, WMS.Lib.TASKTYPE.DELIVERY, oLogger)
            If deltsk Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.Write("No Delivery job assigned to user. Exiting function with error.")
                End If
                Me._responseCode = ResponseCode.UserNotAssigned
                Me._responseText = "No Delivery job assigned to user"
                Me.FillSingleRecord(oLogger)
                Return _resp
            Else
                If Not oLogger Is Nothing Then
                    oLogger.Write("delivery task loaded...")
                End If
            End If
            Session("PCKDeliveryTask") = deltsk
            delobj = deltsk.getDeliveryJob()
            Session("PCKDeliveryJob") = delobj
            If Not oLogger Is Nothing Then
                oLogger.Write("Delivery job set and will be returned to user...")
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
        'If Not oLogger Is Nothing Then
        '    oLogger.Write("Delivery job completed successfully.")
        'End If
        Me._responseCode = ResponseCode.OK
        Me.FillSingleRecord(oLogger)
        _resp(0)("Delivery Location").FieldValue = delobj.toLocation
        _resp(0)("Handling Unit").FieldValue = delobj.LoadId
        Return _resp
    End Function

End Class