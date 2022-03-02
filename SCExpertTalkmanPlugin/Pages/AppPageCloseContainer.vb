Imports WMS.Logic
Imports WMS.Lib

Public Class AppPageCloseContainer
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Closed
        [Error]
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
        Dim userid As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        Dim shouldDeliver As Boolean = False
        Try
            Dim ContId As String = _msg(0)("Container Id").FieldValue
            If Not oLogger Is Nothing Then
                oLogger.Write("Processing Close Container Request...")
                oLogger.Write("Container id received: " & ContId)
            End If
            shouldDeliver = CloseContainer(oLogger)
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while closing the container: " & ex.Description)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while closing the container: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Counting Process completed successfully.")
        End If
        Me._responseCode = ResponseCode.Closed

        Me.FillSingleRecord(oLogger)
        _resp.Record(0)("Delivery").FieldValue = Convert.ToInt32(shouldDeliver).ToString()
        Return _resp
    End Function

    Private Function CloseContainer(ByVal oLogger As WMS.Logic.LogHandler) As Boolean
        Dim shouldDeliver As Boolean = False
        Dim pcklist As Picklist = Session("PCKPicklist") 'New Picklist(pck.picklist)
        Dim relStrat As ReleaseStrategyDetail
        relStrat = pcklist.getReleaseStrategy()
        If Not Session("PCKPicklistActiveContainerID") Is Nothing AndAlso CType(Session("PCKPicklistActiveContainerID"), Container).ContainerId.Equals(_msg(0)("Container Id").FieldValue, StringComparison.OrdinalIgnoreCase) Then
            If Not relStrat Is Nothing Then
                If relStrat.DeliverContainerOnClosing Then
                    pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), True, Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
                    shouldDeliver = True
                Else
                    'Should close the container - go back to PCK to open a new one
                    pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), False, Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
                    shouldDeliver = False
                End If
                Session("PCKPicklistActiveContainerID") = Nothing
            End If
        Else
            If Not relStrat Is Nothing Then
                If relStrat.DeliverContainerOnClosing Then
                    pcklist.CloseContainer(_msg(0)("Container Id").FieldValue, True, Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
                    '--ADDED BY ODED
                    If Session("MultiContainerPicking") Then
                        shouldDeliver = False
                    Else
                        shouldDeliver = True
                    End If

                Else
                    'Should close the container - go back to PCK to open a new one
                    pcklist.CloseContainer(_msg(0)("Container Id").FieldValue, False, Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
                    shouldDeliver = False
                End If
                Session("PCKPicklistActiveContainerID") = Nothing
            End If
        End If
        Return shouldDeliver
    End Function

End Class
